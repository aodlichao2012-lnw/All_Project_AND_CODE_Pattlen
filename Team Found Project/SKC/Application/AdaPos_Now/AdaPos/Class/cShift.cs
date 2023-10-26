using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Class
{
    public class cShift
    {
        public cShift()
        {

        }

        /// <summary>
        /// Type : 1 : Open Shift Manual, 2 : Open Shift Auto
        /// </summary>
        public void C_CHKxTypeOpenShift(string ptUsrCode)
        {
            DialogResult oResult;
            wSignin oSignIn = null;
            string tChkRole, tFuncCode;
            int nChkUsr;

            try
            {
                if (string.Equals(cVB.tVB_PosStaShift, "2"))   // Auto
                {
                    C_CHKxSaleDateHD();

                    new wHome().Show();
                }
                else    // Manual
                {
                    C_CHKxOpenShiftHD();    // ตรวจสอบการเปิดรอบ HD

                    if (string.IsNullOrEmpty(cVB.tVB_OpenShiftHD))   // ปิดรอบแล้ว 
                        new wHome().Show();
                    else
                    {
                        C_CHKxOpenShiftDT();    // ตรวจสอบการเปิดรอบ DT

                        if (string.IsNullOrEmpty(cVB.tVB_OpenShiftDT))   // ปิดรอบย่อยอยู่
                        {
                            C_INSxOpenShiftDT();
                            new wHome().Show();
                        }
                        else    // ถ้ามีการเปิดรอบการขายจะตรวจสอบ User
                        {
                            nChkUsr = C_CHKnOpenShiftUsr(ptUsrCode);

                            if (nChkUsr == 0)    // ไม่ใช่ User เดียวกับที่เปิดรอบ
                            {
                                oResult = MessageBox.Show(string.Format(cVB.oVB_GBResource.GetString("tMsgOpenShift"), cVB.tVB_UsrName), "Openshift User", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (oResult == DialogResult.Yes)
                                {
                                    //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                                    cVB.tVB_KbdCallByName = "C_KBDxCloseShift";
                                    cVB.tVB_KbdScreen = "HOME";
                                    tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                                    tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode); //*Net 63-06-17 เปลี่ยนการเช็คสิทธิ์
                                    //tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                                    switch (tChkRole)
                                    {
                                        case "1":   // allowed.
                                            C_UPDxClosShift();
                                            break;
                                        case "0":   // not permission.
                                        case "800": // data not found.
                                            oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                                            oSignIn.ShowDialog();

                                            if (oSignIn.DialogResult == DialogResult.OK)
                                                C_UPDxClosShift();

                                            break;
                                        case "900":
                                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                                            break;
                                    }

                                    /*
                                    cVB.tVB_KbdCallByName = "C_KBDxCloseShift";
                                    cVB.tVB_KbdScreen = "HOME";
                                    tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                                    if (string.IsNullOrEmpty(tChkRole))
                                    {
                                        oSignIn = new wSignin(1, cVB.tVB_KbdScreen);
                                        oSignIn.ShowDialog();

                                        if (oSignIn.DialogResult == DialogResult.OK)
                                            C_UPDxClosShift();
                                    }
                                    else
                                        C_UPDxClosShift();
                                    */
                                }
                            }
                            else    // User เดียวกับที่เปิดรอบ
                            {
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgWelcome"), 1);
                                new wHome().Show();
                            }
                        }
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxTypeOpenShift : " + oEx.Message); }
            finally
            {
                ptUsrCode = null;
                tFuncCode = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Open shift
        /// </summary>
        public void C_OPNxShift()
        {
            try
            {
                C_GENxShiftCode();
                C_INSxOpenShiftHD();
                C_INSxOpenShiftDT();
                C_CHKxOpenShiftHD();    //*Em 63-08-09

                new cSale().C_PRCxCheckAndCreateTableTemp(); //*Net 63-07-30 ปรับตาม Moshi
                cSale.C_PRCxAdd2TmpLogChg(81, cVB.tVB_ShfCode); //*Em 62-08-23
                new cSyncData().C_PRCxSyncUld();    //*Em 62-08-24
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_OPNxShift : " + oEx.Message); }
        }

        /// <summary>
        /// Check Sale date = System date
        /// </summary>
        private void C_CHKxSaleDateHD()
        {
            StringBuilder oSql;
            int nRows;

            try
            {
                cVB.tVB_SaleDate = DateTime.Now.Date.ToString("yyyy-MM-dd");

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT COUNT(*) FROM TPSTShiftHD  WITH(NOLOCK)");
                oSql.AppendLine("WHERE CONVERT (DATE, FDShdSaleDate) = CONVERT (DATE, GETDATE())");

                nRows = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());

                if (nRows > 0)   //  Sale Date == System date
                    C_CHKxSaleDateDT();
                else    // Sale Date != System date
                {
                    C_UPDxCloseShiftHD();
                    C_UPDxCloseShiftDT();
                    C_GENxShiftCode();
                    C_INSxOpenShiftHD();
                    C_INSxOpenShiftDT();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxSaleDateHD : " + oEx.Message); }
            finally
            {
                oSql = null;
            }
        }

        /// <summary>
        /// Check Sale date = System date
        /// </summary>
        private void C_CHKxSaleDateDT()
        {
            StringBuilder oSql;
            int nRows;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT COUNT(*) FROM TPSTShiftDT WITH(NOLOCK) ");
                //oSql.AppendLine("WHERE CONVERT (DATE, FDSdtDSignIn) = CONVERT (DATE, GETDATE())");
                oSql.AppendLine("WHERE CONVERT (DATE, FDSdtSignIn) = CONVERT (DATE, GETDATE())");   //*Em 62-05-25  AdaFC

                nRows = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());

                if (nRows > 0)   //  Sale Date == System date
                    C_CHKxLastUsr();
                else    // Sale Date != System date
                {
                    C_UPDxCloseShiftDT();
                    C_INSxOpenShiftDT();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxSaleDateDT : " + oEx.Message); }
        }

        /// <summary>
        /// Close shift TPSTShiftHD
        /// </summary>
        private void C_UPDxCloseShiftHD()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE TPSTShiftHD WITH(ROWLOCK) SET");
                //oSql.AppendLine("FDShdDSignOut = CONVERT(DATE, GETDATE()),");
                //oSql.AppendLine("FTShdTSignOut = CONVERT(VARCHAR(10),GETDATE(), 108)");
                //oSql.AppendLine("WHERE FDShdDSignOut IS NULL");
                oSql.AppendLine("FDShdSignOut = GETDATE(),");    //*Em 62-05-25  AdaFC
                oSql.AppendLine("FTShdUsrClosed = '" + cVB.tVB_UsrCode + "'");     //*Em 62-05-25  AdaFC
                oSql.AppendLine("WHERE FDShdSignOut IS NULL");  //*Em 62-05-25  AdaFC

                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_UPDxCloseShiftHD : " + oEx.Message); }
        }

        /// <summary>
        /// Check Last user Open shift
        /// </summary>
        private void C_CHKxLastUsr()
        {
            StringBuilder oSql;
            List<cmlTPSTShiftDT> aoShfDT;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTShfCode FROM TPSTShiftDT WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTUsrCode = '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine("AND CONVERT (DATE, FDSdtDSignIn) = CONVERT (DATE, GETDATE())");

                aoShfDT = new cDatabase().C_GETaDataQuery<cmlTPSTShiftDT>(oSql.ToString());

                if (aoShfDT.Count == 0)   // User code Open shift != User code Login
                {
                    C_UPDxCloseShiftDT();
                    C_INSxOpenShiftDT();
                }
                else
                {
                    cVB.tVB_ShfCode = aoShfDT[0].FTShfCode;
                    cVB.nVB_ShfSeq = Convert.ToInt32(aoShfDT[0].FNSdtSeqNo);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxLastUsr " + oEx.Message); }
        }

        /// <summary>
        /// Close shift TPSTShiftDT
        /// </summary>
        private void C_UPDxCloseShiftDT()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE TPSTShiftDT WITH(ROWLOCK) SET");
                //oSql.AppendLine("FDSdtDSignOut = CONVERT(DATE, GETDATE()),");
                //oSql.AppendLine("FTSdtTSignOut = CONVERT(VARCHAR(10),GETDATE(), 108)");
                //oSql.AppendLine("WHERE FDSdtDSignOut IS NULL");
                oSql.AppendLine("FDSdtSignOut = GETDATE(),");   //*Em 62-05-25  AdaFC
                oSql.AppendLine("FTSdtUsrClosed = '" + cVB.tVB_UsrCode + "'");     //*Em 62-05-25  AdaFC
                oSql.AppendLine("WHERE FDSdtSignOut IS NULL");  //*Em 62-05-25  AdaFC


                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_UPDxCloseShiftDT : " + oEx.Message); }
        }

        /// <summary>
        /// Gen shift code
        /// </summary>
        private void C_GENxShiftCode()
        {
            cDatabase oDB = new cDatabase();
            cmlTPSTShiftHD oShfHD;
            StringBuilder oSql;
            string tRunning;
            int nMax = 0;
            int nMaxTmp = 0;

            try
            {
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT TOP(1) FTShfCode");
                //oSql.AppendLine("FROM TPSTShiftHD WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                //oSql.AppendLine("AND FTShfCode LIKE FORMAT(GETDATE(), 'yyMMdd', 'en-US' ) + '%'");
                //oSql.AppendLine("ORDER BY FTShfCode DESC");

                //oShfHD = new cDatabase().C_GEToDataQuery<cmlTPSTShiftHD>(oSql.ToString());

                //if(oShfHD == null)  // วันที่ปัจจุบัน ไม่เคยเปิดรอบ
                //    cVB.tVB_ShfCode = DateTime.Now.Date.ToString("yyMMdd") + "01";
                //else
                //{
                //    tRunning = oShfHD.FTShfCode.Substring(6, 2);
                //    tRunning = string.Format("{0:00}", Convert.ToInt32(tRunning) + 1);  // ใส่ format ให้เป็น 5 หลัก เช่น 00001
                //    cVB.tVB_ShfCode = DateTime.Now.Date.ToString("yyMMdd") + tRunning;
                //}

                //*Em 62-06-20
                string tTblName = "TPSTShiftHD";
                string tFedDocNo = "FTShfCode";
                string tDocLeft = "";
                string tDocNo = "";


                //cSale.C_GETxFormatDoc("TPSTShiftHD", 0, DateTime.Now.Date, cVB.tVB_PosCode, cVB.tVB_ShpCode);
                cSale.C_GETtFormatDoc("TPSTShiftHD", 0, DateTime.Now.Date, cVB.tVB_PosCode, cVB.tVB_ShpCode);   //*Arm 63-03-04

                string tFmt = new string('0', cSale.nC_DocRuningLength);
                tDocLeft = cSale.tC_DocFmtLeft;
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + cSale.nC_DocRuningLength + ") AS FTMax");
                oSql.AppendLine("FROM " + tTblName + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + cSale.nC_DocRuningLength));
                nMax = oDB.C_GEToDataQuery<int>(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + cSale.nC_DocRuningLength + ") AS FTMax");
                oSql.AppendLine("FROM " + tTblName + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + cSale.nC_DocRuningLength));
                nMaxTmp = oDB.C_GEToDataQuery<int>(oSql.ToString());
                if (nMaxTmp > nMax) nMax = nMaxTmp;

                cVB.tVB_ShfCode = cSale.tC_DocFmtLeft + string.Format("{0:" + tFmt + "}", nMax + 1);
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GENxShiftCode : " + oEx.Message); }
        }

        /// <summary>
        /// Insert Openshift TPSTShiftDT
        /// </summary>
        private void C_INSxOpenShiftHD()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO TPSTShiftHD ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("(");
                oSql.AppendLine("   FTBchCode, FTShfCode, FTPosCode,");
                oSql.AppendLine("   FTUsrCode, FDShdSaleDate, FDShdSignIn,");
                //oSql.AppendLine("   FTShdTSignIn, FTShdStaPrc, ");
                oSql.AppendLine("   FTShdStaPrc, ");    //*Em 62-05-25  AdaFC
                oSql.AppendLine("   FDLastUpdOn, FDCreateOn, FTLastUpdBy, ");
                oSql.AppendLine("   FTCreateBy");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES");
                oSql.AppendLine("(");
                oSql.AppendLine("   '" + cVB.tVB_BchCode + "', '" + cVB.tVB_ShfCode + "', '" + cVB.tVB_PosCode + "',");
                //oSql.AppendLine("   '" + cVB.tVB_UsrCode + "', '" + cVB.tVB_SaleDate + "', CONVERT(DATE, GETDATE()),");
                //oSql.AppendLine("   CONVERT(VARCHAR(10),GETDATE(), 108), NULL, ");
                oSql.AppendLine("   '" + cVB.tVB_UsrCode + "', '" + cVB.tVB_SaleDate + "', GETDATE(),");    //*Em 62-05-25  AdaFC
                oSql.AppendLine("   NULL, ");    //*Em 62-05-25  AdaFC
                oSql.AppendLine("   CONVERT(DATETIME, GETDATE()), CONVERT(DATETIME, GETDATE()), '" + cVB.tVB_UsrCode + "',");
                oSql.AppendLine("   '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine(")");

                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxOpenShiftHD : " + oEx.Message); }
        }

        /// <summary>
        /// Insert Openshift TPSTShiftDT
        /// </summary>
        private void C_INSxOpenShiftDT()
        {
            StringBuilder oSql;
            int nSeqNo = 0;

            try
            {
                // Check Seq No
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FNSdtSeqNo");
                oSql.AppendLine("FROM TPSTShiftDT WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + cVB.tVB_PosCode + "'");   //*Em 61-12-27  Water Park
                oSql.AppendLine("ORDER BY FNSdtSeqNo DESC");

                nSeqNo = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());

                if (nSeqNo == 0)
                {
                    nSeqNo = 1;
                }
                else
                {
                    nSeqNo = nSeqNo + 1;    //*Em 62-01-28 AdaPos 5.0
                }

                oSql.Clear();
                oSql.AppendLine("INSERT INTO TPSTShiftDT ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("(");
                //oSql.AppendLine("   FTBchCode, FTShfCode, FNSdtSeqNo,");
                oSql.AppendLine("   FTBchCode, FTShfCode, FTPosCode, FNSdtSeqNo,"); //*Em 61-12-27  Water Park
                oSql.AppendLine("   FTShpCode, FTUsrCode, FDSdtSignIn");
                //oSql.AppendLine("   FTSdtTSignIn");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES");
                oSql.AppendLine("(");
                //oSql.AppendLine("   '" + cVB.tVB_BchCode + "', '" + cVB.tVB_ShfCode + "', " + nSeqNo + ",");
                oSql.AppendLine("   '" + cVB.tVB_BchCode + "', '" + cVB.tVB_ShfCode + "', '" + cVB.tVB_PosCode + "'," + nSeqNo + ",");    //*Em 61-12-27  Water Park
                oSql.AppendLine("   '" + cVB.tVB_ShpCode + "', '" + cVB.tVB_UsrCode + "', GETDATE()");
                //oSql.AppendLine("   CONVERT(VARCHAR(10),GETDATE(), 108)");
                oSql.AppendLine(")");

                new cDatabase().C_SETxDataQuery(oSql.ToString());

                cVB.nVB_ShfSeq = nSeqNo;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxOpenShiftDT : " + oEx.Message); }
        }

        /// <summary>
        /// Check openshift HD
        /// </summary>
        private void C_CHKxOpenShiftHD()
        {
            StringBuilder oSql;
            List<cmlTPSTShiftHD> aoShiftHD;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTShfCode, FDShdSaleDate, FDCreateOn");
                oSql.AppendLine("FROM TPSTShiftHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND FDShdSignOut IS NULL");

                aoShiftHD = new cDatabase().C_GETaDataQuery<cmlTPSTShiftHD>(oSql.ToString());

                if (aoShiftHD.Count > 0)
                {
                    cVB.tVB_OpenShiftHD = aoShiftHD[0].FTShfCode;
                    cVB.tVB_SaleDate = Convert.ToDateTime(aoShiftHD[0].FDShdSaleDate).ToString("yyyy-MM-dd");
                    cVB.tVB_ShfCode = cVB.tVB_OpenShiftHD;
                    cVB.tVB_ShfCreateOn = Convert.ToDateTime(aoShiftHD[0].FDCreateOn).ToString("yyyy-MM-dd"); //*Em 63-08-09
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxOpenShiftHD : " + oEx.Message); }
        }

        /// <summary>
        /// Check openshift DT
        /// </summary>
        private void C_CHKxOpenShiftDT()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ISNULL(FNSdtSeqNo, 0) AS FNSdtSeqNo");
                oSql.AppendLine("FROM TPSTShiftDT WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTShfCode = '" + cVB.tVB_OpenShiftHD + "'");
                oSql.AppendLine("AND FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTShpCode = '" + cVB.tVB_ShpCode + "'");
                oSql.AppendLine("AND FTUsrCode = '" + cVB.tVB_UsrCode + "'"); //*Em 62-01-28  AdaPos 5.0
                oSql.AppendLine("AND FDSdtSignOut IS NULL");

                cVB.tVB_OpenShiftDT = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                cVB.nVB_ShfSeq = Convert.ToInt32(cVB.tVB_OpenShiftDT);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxOpenShiftDT : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Check User code
        /// </summary>
        private int C_CHKnOpenShiftUsr(string ptUsrCode)
        {
            StringBuilder oSql;
            int nChkUsr = 0;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT COUNT(*)");
                oSql.AppendLine("FROM TPSTShiftDT WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTShfCode = '" + cVB.tVB_OpenShiftHD + "'");
                oSql.AppendLine("AND FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTShpCode = '" + cVB.tVB_ShpCode + "'");
                oSql.AppendLine("AND FTUsrCode = '" + ptUsrCode + "'");

                nChkUsr = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKnOpenShiftUsr : " + oEx.Message); }
            finally
            {
                ptUsrCode = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return nChkUsr;
        }

        /// <summary>
        /// Close Shift
        /// </summary>
        public void C_UPDxClosShift(bool pbOpenHome = true)
        {
            try
            {
                cVB.tVB_OpenShiftDT = null;
                cVB.tVB_OpenShiftHD = null;
                C_UPDxCloseShiftHD();
                C_UPDxCloseShiftDT();
                cSale.C_PRCxAdd2TmpLogChg(81, cVB.tVB_ShfCode); //*Em 62-08-23
                //new cSyncData().C_PRCxSyncUld();    //*Em 62-08-24
                if (pbOpenHome) new wHome().Show();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_UPDxClosShift : " + oEx.Message); }
        }

        private static List<cmlShiftRcv> C_GETaShiftRcv()
        {
            List<cmlShiftRcv> aoShiftRcv = new List<cmlShiftRcv>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT FNSdtSeqNo,ISNULL(RCVL.FTRcvName,(SELECT TOP 1 FTRcvName FROM TFNMRcv_L WITH(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode)) AS FTRcvName,FCRcvPayAmt ");
                oSql.AppendLine("FROM TPSTShiftSKeyRcv RCV WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + cVB.tVB_ShfCode + "'");
                aoShiftRcv = oDB.C_GETaDataQuery<cmlShiftRcv>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftRcv : " + oEx.Message); }
            return aoShiftRcv;
        }

        /// <summary>
        /// Insert LastDoc
        /// </summary>
        //public void C_INSxLastDoc()
        public void C_INSxLastDoc(bool pbTrans = false) //*Net 63-06-04 Default ที่ตาราง Temp
        {
            string tTblSalHD;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                tTblSalHD = (pbTrans) ? "TPSTSalHD" : cSale.tC_TblSalHD;
                oSql.AppendLine("INSERT INTO TPSTShiftSLastDoc (FTBchCode, FTPosCode, FTShfCode, FNSdtSeqNo, FNLstDocType, FTLstDocNoFrm, FTLstDocNoTo)");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("SELECT SHF.FTBchCode,SHF.FTPosCode,SHF.FTShfCode,1 AS FNSdtSeqNo,1 AS FNXshDocType,MIN(FTXshDocNo) AS FTLstDocNoFrm,MAX(FTXshDocNo) AS FTLstDocNoTo");
                oSql.AppendLine("FROM TPSTShiftHD SHF WITH(NOLOCK)");
                //oSql.AppendLine("LEFT JOIN TPSTSalHD SAL WITH(NOLOCK) ON SAL.FTBchCode = SHF.FTBchCode AND SAL.FTShfCode = SHF.FTShfCode ");
                oSql.AppendLine($"LEFT JOIN {tTblSalHD} SAL WITH(NOLOCK) ON SAL.FTBchCode = SHF.FTBchCode AND SAL.FTShfCode = SHF.FTShfCode ");//*Net 63-06-04
                oSql.AppendLine("	AND SAL.FNXshDocType = 1");
                oSql.AppendLine("   AND SAL.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                oSql.AppendLine("WHERE SHF.FTBchCode = '" + cVB.tVB_BchCode + "' AND SHF.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("GROUP BY SHF.FTBchCode,SHF.FTPosCode,SHF.FTShfCode,FNXshDocType");
                oSql.AppendLine("UNION");
                oSql.AppendLine("SELECT SHF.FTBchCode,SHF.FTPosCode,SHF.FTShfCode,1 AS FNSdtSeqNo,9 AS FNXshDocType,MIN(FTXshDocNo) AS FTLstDocNoFrm,MAX(FTXshDocNo) AS FTLstDocNoTo");
                oSql.AppendLine("FROM TPSTShiftHD SHF WITH(NOLOCK)");
                //oSql.AppendLine("LEFT JOIN TPSTSalHD SAL WITH(NOLOCK) ON SAL.FTBchCode = SHF.FTBchCode AND SAL.FTShfCode = SHF.FTShfCode ");
                oSql.AppendLine($"LEFT JOIN {tTblSalHD} SAL WITH(NOLOCK) ON SAL.FTBchCode = SHF.FTBchCode AND SAL.FTShfCode = SHF.FTShfCode "); //*Net 63-06-04
                oSql.AppendLine("	AND SAL.FNXshDocType = 9");
                oSql.AppendLine("   AND SAL.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                oSql.AppendLine("WHERE SHF.FTBchCode = '" + cVB.tVB_BchCode + "' AND SHF.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("GROUP BY SHF.FTBchCode,SHF.FTPosCode,SHF.FTShfCode,FNXshDocType");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxLastDoc : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        private static List<cmlShiftBN> C_GETaShiftBN()
        {
            List<cmlShiftBN> aoShiftBN = new List<cmlShiftBN>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT ISNULL(BNL.FTBntName,(SELECT TOP 1 FTBntName FROM TFNMBankNote_L WITH(NOLOCK) WHERE FTBntCode = BN.FTBntCode)) AS FTBntName,BN.FNKbnQty,BN.FCKbnAmt ");
                oSql.AppendLine("FROM TPSTShiftSKeyBN BN WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMBankNote_L BNL WITH(NOLOCK) ON BN.FTBntCode = BNL.FTBntCode AND BNL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("ORDER BY BN.FTBntCode");
                aoShiftBN = oDB.C_GETaDataQuery<cmlShiftBN>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftBN : " + oEx.Message); }
            return aoShiftBN;
        }

        /// <summary>
        /// Insert Product spacial in shift.
        /// </summary>
        //public void C_INSxPdtSpc()
        public void C_INSxPdtSpc(bool pbTrans = false) //*Net 63-06-04 Default ที่ตาราง Temp
        {
            string tTblSalHD;
            string tTblSalDT;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                tTblSalHD = (pbTrans) ? "TPSTSalHD" : cSale.tC_TblSalHD;
                tTblSalDT = (pbTrans) ? "TPSTSalDT" : cSale.tC_TblSalDT;
                oSql.AppendLine("INSERT INTO TPSTShiftSRatePdt ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("(FTBchCode,FTPosCode,FTShfCode,FNSdtSeqNo,FTSrpCodeRef,FTSrpType,FTSrpNameRef,FCSrpQty,FCSrpAmt)");
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,1 AS FNSdtSeqNo,PDT.FTPdtCode,2 AS FTSrpType,");
                oSql.AppendLine("PDTL.FTPdtNameABB,");
                oSql.AppendLine("SUM(DT.FCXsdQty) AS FCSrpQty,SUM(DT.FCXsdNet) AS FCSrpAmt");
                oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK)");
                //oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON DT.FTPdtCode = PDT.FTPdtCode");
                //oSql.AppendLine("INNER JOIN TPSTSalHD HD WITH(NOLOCK) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                oSql.AppendLine($"INNER JOIN {tTblSalDT} DT WITH(NOLOCK) ON DT.FTPdtCode = PDT.FTPdtCode");
                oSql.AppendLine($"INNER JOIN {tTblSalHD} HD WITH(NOLOCK) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                oSql.AppendLine(" AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                oSql.AppendLine("LEFT JOIN TCNMPdt_L PDTL WITH(NOLOCK) ON PDT.FTPdtCode = PDTL.FTPdtCode AND PDTL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("AND ISNULL(PDT.FTPdtGrpControl,'') = '1'");
                oSql.AppendLine("GROUP BY HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,PDT.FTPdtCode,PDTL.FTPdtNameABB");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxPdtSpc : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Insert Currencry in Shift
        /// </summary>
        //public void C_INSxCurrency()
        public void C_INSxCurrency(bool pbTrans = false) //*Net 63-06-04 Default ที่ตาราง Temp
        {
            string tTblSalRC;
            string tTblSalHD;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                tTblSalRC = (pbTrans) ? "TPSTSalRC" : cSale.tC_TblSalRC;
                tTblSalHD = (pbTrans) ? "TPSTSalHD" : cSale.tC_TblSalHD;
                oSql.AppendLine("INSERT INTO TPSTShiftSRatePdt ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("(FTBchCode,FTPosCode,FTShfCode,FNSdtSeqNo,FTSrpCodeRef,FTSrpType,FTSrpNameRef,FCSrpQty,FCSrpAmt)");
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,1 AS FNSdtSeqNo,RTE.FTRteCode,1 AS FTSrpType,");
                oSql.AppendLine("RTEL.FTRteName,");
                //oSql.AppendLine("COUNT(RC.FTRteCode) AS FCSrpQty,SUM(RC.FCXrcNet) AS FCSrpAmt");
                oSql.AppendLine("COUNT(RC.FTRteCode) AS FCSrpQty,SUM(CASE WHEN HD.FNXshDocType = 9 THEN RC.FCXrcNet*(-1) ELSE RC.FCXrcNet END) AS FCSrpAmt");    //*Em 63-05-30
                oSql.AppendLine("FROM TFNMRate RTE WITH(NOLOCK)");
                //oSql.AppendLine("INNER JOIN TPSTSalRC RC WITH(NOLOCK) ON RC.FTRteCode = RTE.FTRteCode");
                oSql.AppendLine($"INNER JOIN {tTblSalRC} RC WITH(NOLOCK) ON RC.FTRteCode = RTE.FTRteCode");
                //oSql.AppendLine("INNER JOIN TPSTSalHD HD WITH(NOLOCK) ON HD.FTBchCode = RC.FTBchCode AND HD.FTXshDocNo = RC.FTXshDocNo");
                oSql.AppendLine($"INNER JOIN {tTblSalHD} HD WITH(NOLOCK) ON HD.FTBchCode = RC.FTBchCode AND HD.FTXshDocNo = RC.FTXshDocNo");
                oSql.AppendLine(" AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                oSql.AppendLine("LEFT JOIN TFNMRate_L RTEL WITH(NOLOCK) ON RTEL.FTRteCode = RTE.FTRteCode AND RTEL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RC.FTRcvCode = RCV.FTRcvCode");
                oSql.AppendLine("WHERE (RCV.FTFmtCode = '001' OR RCV.FTFmtCode = '010')");
                oSql.AppendLine("AND HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("GROUP BY HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,RTE.FTRteCode,RTEL.FTRteName");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxCurrency : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Get data product spacial in shift
        /// </summary>
        /// <returns></returns>
        private static List<cmlTPSTShiftSRatePdt> C_GETaShiftSPdt()
        {
            List<cmlTPSTShiftSRatePdt> aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT ISNULL(FTSrpNameRef,(SELECT TOP 1 FTPdtNameABB FROM TCNMPdt_L WITH(NOLOCK) WHERE FTPdtCode = SRP.FTSrpCodeRef)) AS FTSrpNameRef,FCSrpQty,FCSrpAmt");
                oSql.AppendLine("FROM TPSTShiftSRatePdt SRP WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMPdt_L PDTL WITH(NOLOCK) ON SRP.FTSrpCodeRef = PDTL.FTPdtCode AND PDTL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE SRP.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND SRP.FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND SRP.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("AND SRP.FTSrpType = 2");
                aoShiftSRatePdt = oDB.C_GETaDataQuery<cmlTPSTShiftSRatePdt>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftSPdt : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return aoShiftSRatePdt;
        }

        /// <summary>
        /// Get data currency in shift
        /// </summary>
        /// <returns></returns>
        private static List<cmlTPSTShiftSRatePdt> C_GETaShiftSRate()
        {
            List<cmlTPSTShiftSRatePdt> aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT FTSrpNameRef,FCSrpQty,FCSrpAmt FROM ("); //*Net 63-06-19
                //oSql.AppendLine("SELECT ISNULL(FTSrpNameRef,(SELECT TOP 1 FTRteName FROM TFNMRate_L WITH(NOLOCK) WHERE FTRteCode = SRP.FTSrpCodeRef)) AS FTSrpNameRef,FCSrpQty,FCSrpAmt");
                oSql.AppendLine("SELECT ISNULL(FTSrpNameRef,(SELECT TOP 1 FTRteName FROM TFNMRate_L WITH(NOLOCK) WHERE FTRteCode = SRP.FTSrpCodeRef)) AS FTSrpNameRef,FCSrpQty,FCSrpAmt,RTE.FTRteStaLocal,COUNT(*) OVER() AS nCount"); //*Net 63-06-19
                oSql.AppendLine("FROM TPSTShiftSRatePdt SRP WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMRate_L RTEL WITH(NOLOCK) ON SRP.FTSrpCodeRef = RTEL.FTRteCode AND RTEL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TFNMRate RTE WITH(NOLOCK) ON RTEL.FTRteCode=RTE.FTRteCode"); //*Net 63-07-30 ปรับตาม Moshi
                oSql.AppendLine("WHERE SRP.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND SRP.FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND SRP.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("AND SRP.FTSrpType = 1");
                oSql.AppendLine(")t WHERE (nCount>1) OR (nCount=1 AND ISNULL(FTRteStaLocal,'')<>'1')"); //*Net 63-07-30 ปรับตาม Moshi
                aoShiftSRatePdt = oDB.C_GETaDataQuery<cmlTPSTShiftSRatePdt>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftSRate : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return aoShiftSRatePdt;
        }

        /// <summary>
        /// Get data Shift Last Doc.
        /// </summary>
        /// <returns></returns>
        private static List<cmlTPSTShiftSLastDoc> C_GETaShiftSLastDoc()
        {
            List<cmlTPSTShiftSLastDoc> aoData = new List<cmlTPSTShiftSLastDoc>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT FNLstDocType,ISNULL(FTLstDocNoFrm,'--------N/A--------') AS FTLstDocNoFrm ,ISNULL(FTLstDocNoTo,'--------N/A--------') AS FTLstDocNoTo ");
                oSql.AppendLine("FROM TPSTShiftSLastDoc WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + cVB.tVB_ShfCode + "'");
                aoData = oDB.C_GETaDataQuery<cmlTPSTShiftSLastDoc>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftSLastDoc : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return aoData;
        }

        //public void C_INSxShiftSumRcv()
        public void C_INSxShiftSumRcv(bool pbTrans = false) //*Net 63-06-04 Default ตารางที่ Temp
        {
            string tTblSalRC;
            string tTblSalHD;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                tTblSalRC = (pbTrans) ? "TPSTSalRC" : cSale.tC_TblSalRC;
                tTblSalHD = (pbTrans) ? "TPSTSalHD" : cSale.tC_TblSalHD;
                oSql.AppendLine("INSERT INTO TPSTShiftSSumRcv ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("(FTBchCode,FTPosCode,FTShfCode,FNSdtSeqNo,FTRcvCode,FTRcvDocType,FCRcvPayAmt)");
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,1 AS FNSdtSeqNo,RC.FTRcvCode,HD.FNXshDocType,SUM(RC.FCXrcNet) AS FCRcvPayAmt");
                //oSql.AppendLine("FROM TPSTSalRC RC WITH(NOLOCK)");
                oSql.AppendLine($"FROM {tTblSalRC} RC WITH(NOLOCK)"); //*Net 63-06-04 เลือกตาราง Trans/Temp
                //oSql.AppendLine("INNER JOIN TPSTSalHD HD WITH(NOLOCK) ON HD.FTBchCode = RC.FTBchCode AND HD.FTXshDocNo = RC.FTXshDocNo");
                oSql.AppendLine($"INNER JOIN {tTblSalHD} HD WITH(NOLOCK) ON HD.FTBchCode = RC.FTBchCode AND HD.FTXshDocNo = RC.FTXshDocNo"); //*Net 63-06-04 เลือกตาราง Trans/Temp
                oSql.AppendLine(" AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode ='" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("GROUP BY HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,RC.FTRcvCode,HD.FNXshDocType");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxShiftSumRcv : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        private static List<cmlTPSTShiftSSumRcv> C_GETaShiftSumRcv()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTShiftSSumRcv> aoData = new List<cmlTPSTShiftSSumRcv>();
            try
            {
                oSql.AppendLine("SELECT ISNULL(RCVL.FTRcvName,(SELECT TOP 1 FTRcvName FROM TFNMRcv_L WITH(NOLOCK) WHERE FTRcvCode = SSR.FTRcvCode)) AS FTRcvName,");
                //oSql.AppendLine("SSR.FTRcvCode,SSR.FCRcvPayAmt,SSR.FTRcvDocType");
                oSql.AppendLine("SSR.FTRcvCode,SSR.FCRcvPayAmt,SSR.FTRcvDocType,RCV.FTFmtCode"); //*Net 63-07-30 ปรับตาม Moshi
                oSql.AppendLine("FROM TPSTShiftSSumRcv SSR WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RCV.FTRcvCode=SSR.FTRcvCode"); //*Net 63-07-30 ปรับตาม Moshi
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCVL.FTRcvCode = SSR.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE SSR.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND SSR.FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND SSR.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("ORDER BY SSR.FTShfCode,SSR.FTRcvCode,SSR.FTRcvDocType");
                aoData = oDB.C_GETaDataQuery<cmlTPSTShiftSSumRcv>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftSumRcv : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return aoData;
        }

        //private static cmlShiftSum C_GEToShiftSum()
        private static cmlShiftSum C_GEToShiftSum(bool pbTrans = false)//*Net 63-06-04 Default ที่ตาราง Temp
        {
            string tTblSalHD;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            cmlShiftSum oData = new cmlShiftSum();
            DataTable odtTmp;
            try
            {
                oData.nCntOpDrw = 0;
                oData.nCntCancelBill = 0;
                oData.nCntHoldBill = 0;
                oData.nMnyInCnt = 0;
                oData.cMnyInAmt = 0;
                oData.nMnyOutCnt = 0;
                oData.cMnyOutAmt = 0;
                oData.nSaleCnt = 0;
                oData.cSaleAmt = 0;
                oData.nRefundCnt = 0;
                oData.cRefundAmt = 0;
                oData.cRoundAmt = 0; //*Net 63-07-30 ปรับตาม Moshi

                tTblSalHD = (pbTrans) ? "TPSTSalHD" : cSale.tC_TblSalHD; //*Net 63-06-04

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTShfCode,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 1 THEN 1 ELSE 0 END) AS FNCntSale,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 1 THEN HD.FCXshGrand-HD.FCXshRnd ELSE 0 END) AS FCAmtSale,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 9 THEN 1 ELSE 0 END) AS FNCntRef,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 9 THEN HD.FCXshGrand-HD.FCXshRnd ELSE 0 END) AS FCAmtRef,");
                oSql.AppendLine("SUM(HD.FCXshRnd) AS FCXshRnd"); //*Net 63-07-30 ปรับตาม Moshi
                //oSql.AppendLine("FROM TPSTSalHD HD WITH(NOLOCK)");
                oSql.AppendLine($"FROM {tTblSalHD} HD WITH(NOLOCK)");
                oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "' ");
                oSql.AppendLine("AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' ");
                oSql.AppendLine(" AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                oSql.AppendLine("GROUP BY HD.FTBchCode,HD.FTShfCode,HD.FTPosCode");
                odtTmp = new DataTable();
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        oData.nSaleCnt = odtTmp.Rows[0].Field<int>("FNCntSale");
                        oData.cSaleAmt = odtTmp.Rows[0].Field<decimal>("FCAmtSale");
                        oData.nRefundCnt = odtTmp.Rows[0].Field<int>("FNCntRef");
                        oData.cRefundAmt = odtTmp.Rows[0].Field<decimal>("FCAmtRef");
                        oData.cRoundAmt = odtTmp.Rows[0].Field<decimal>("FCXshRnd"); //*Net 63-07-30 ปรับตาม Moshi
                    }
                }

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode,FTPosCode,FTShfCode,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '004' THEN 1 ELSE 0 END) AS FNCntOpDrw,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '006'  THEN 1 ELSE 0 END) AS FNCntCancelBill,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '003'  THEN 1 ELSE 0 END) AS FNCntHoldBill,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '001'  THEN 1 ELSE 0 END) AS FNMnyInCnt,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '001'  THEN FCSvnAmt ELSE 0 END) AS FCMnyInAmt,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '002'  THEN 1 ELSE 0 END) AS FNMnyOutCnt,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '002'  THEN FCSvnAmt ELSE 0 END) AS FCMnyOutAmt");
                oSql.AppendLine("FROM TPSTShiftEvent WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("GROUP BY FTBchCode,FTPosCode,FTShfCode");
                odtTmp = new DataTable();
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        oData.nCntOpDrw = odtTmp.Rows[0].Field<int>("FNCntOpDrw");
                        oData.nCntCancelBill = odtTmp.Rows[0].Field<int>("FNCntCancelBill");
                        oData.nCntHoldBill = odtTmp.Rows[0].Field<int>("FNCntHoldBill");
                        oData.nMnyInCnt = odtTmp.Rows[0].Field<int>("FNMnyInCnt");
                        oData.cMnyInAmt = odtTmp.Rows[0].Field<decimal>("FCMnyInAmt");
                        oData.nMnyOutCnt = odtTmp.Rows[0].Field<int>("FNMnyOutCnt");
                        oData.cMnyOutAmt = odtTmp.Rows[0].Field<decimal>("FCMnyOutAmt");
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GEToShiftSum : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return oData;
        }

        /// <summary>
        /// Check ยอดขาย ก่อนจะปิดรอบ
        /// 2020-02-18 Create By Zen
        /// </summary>
        /// <returns></returns>
        //public int C_CHKnShiftofSale()
        public int C_CHKnShiftofSale(bool pbTrans = false)
        {
            int nCheck = 0;
            string tTblSalHD;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            //DataTable odtTmp; //*Net 63-06-04
            try
            {
                oSql = new StringBuilder();
                tTblSalHD = (pbTrans) ? "TPSTSalHD" : cSale.tC_TblSalHD;
                //oSql.AppendLine("SELECT HD.FTBchCode,HD.FTShfCode,HD.FDXshDocDate,HD.FCXshGrand,HD.FCXshRnd");
                oSql.AppendLine("SELECT COUNT(*)"); //*Net 63-06-04
                //oSql.AppendLine("FROM TPSTSalHD HD WITH(NOLOCK)");
                oSql.AppendLine($"FROM {tTblSalHD} HD WITH(NOLOCK)");
                oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "' ");
                oSql.AppendLine($" AND HD.FTPosCode='{cVB.tVB_PosCode}' AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 ตรวจสอบ Pos และสถานะเอกสาร
                //odtTmp = new DataTable();
                //odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                //if (odtTmp.Rows.Count > 0)
                //{
                //    nCheck = 1;
                //}
                nCheck = oDB.C_GEToDataQuery<int>(oSql.ToString());
                return nCheck;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GEToShiftSum : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return nCheck;
        }

        /// <summary>
        /// ซ่อมข้อมูลของ Shf ที่ส่งซ่อมไม่สำเร็จ
        /// </summary>
        public static void C_PRCxFixDocByShfEvent()
        {
            List<string> atShfNotFix;
            try
            {
                atShfNotFix = new cShiftEvent().C_GETaShiftbyEvent("007", "N");
                if (atShfNotFix != null && atShfNotFix.Count > 0)
                {
                    foreach (string tShfCode in atShfNotFix)
                    {
                        C_CHKbShift2BackOffice(tShfCode);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_PRCxFixDocByShfEvent : " + oEx.Message); }
            finally
            {
                atShfNotFix = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// ดึงบิลขายตามรอบ Bch,Pos,ShfCode,DocNo,DocDate, ส่งไปให้ MQ
        /// Queue Name : PS_QListDocByShift
        /// สร้าง Q มารอรับ : PS_QFixDoc+[BCH]+[POS]
        /// </summary>
        /// <returns></returns>
        //public static void C_CHKxShift2BackOffice()
        public static int C_CHKbShift2BackOffice(string ptShfCode) //*Net 63-07-30 void->return int
        {
            string tMsg;
            cmlCheckListSalDoc oChkSalDoc;
            List<cmlListSalDoc> aoSalDoc;
            try
            {
                aoSalDoc = C_GETaDocNoFrmShift(cVB.tVB_BchCode, cVB.tVB_PosCode, ptShfCode);
                //if (aoSalDoc == null || aoSalDoc.Count == 0) return;
                //*Net 63-07-25 ถ้าไม่มีบิลให้บันทึก Y จะได้ไม่ต้องดึงมาอีก
                if (aoSalDoc == null || aoSalDoc.Count == 0)
                {
                    C_SETxShiftFixDoc(ptShfCode, 0m, "Y");
                    return 0;
                }

                oChkSalDoc = new cmlCheckListSalDoc();
                oChkSalDoc.ptFunction = "Upload Doc No by Shift";
                oChkSalDoc.ptSource = "POS.AdaStoreFront";
                oChkSalDoc.ptDest = "HQ.MQRcvProcess";
                oChkSalDoc.ptFilter = cVB.tVB_BchCode;
                oChkSalDoc.paData = aoSalDoc;

                tMsg = JsonConvert.SerializeObject(oChkSalDoc);

                if (cRabbitMQ.C_PRCxPub2Queue(true, tMsg, "PS_QListDocByShift"))
                {
                    C_SETxShiftFixDoc(ptShfCode, aoSalDoc.Count, "Y");
                    return 1; //*Net 63-07-25 return success
                }
                else
                {
                    C_SETxShiftFixDoc(ptShfCode, aoSalDoc.Count, "N");
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxShift2BackOffice : " + oEx.Message); }
            finally
            {
                aoSalDoc = null;
                oChkSalDoc = null;
                //new cSP().SP_CLExMemory();
            }
            return -1; //*Net 63-07-25 return fail
        }

        public static void C_SETxShiftFixDoc(string ptShfCode, decimal pcAmt, string ptRmk)
        {
            cmlTPSTShiftEvent oEvent;
            try
            {
                oEvent = new cShiftEvent().C_GEToShiftSumEvent(ptShfCode, "007");
                if (oEvent.FNSvnQty == 0)
                {
                    oEvent = new cmlTPSTShiftEvent();
                    oEvent.FTBchCode = cVB.tVB_BchCode;
                    oEvent.FTShfCode = cVB.tVB_ShfCode;
                    oEvent.FTPosCode = cVB.tVB_PosCode;
                    oEvent.FNSdtSeqNo = cVB.nVB_ShfSeq;
                    oEvent.FDHisDateTime = DateTime.Now;
                    oEvent.FTEvnCode = "007";
                    oEvent.FNSvnQty = 1;
                    oEvent.FCSvnAmt = pcAmt;
                    oEvent.FTRsnCode = "";
                    oEvent.FTSvnApvCode = cVB.tVB_UsrCode;
                    oEvent.FTSvnRemark = ptRmk;
                    new cShiftEvent().C_INSxShiftEvent(oEvent);
                }
                else
                {
                    new cShiftEvent().C_UPDxShiftEvent(ptShfCode, "007", oEvent.FNSvnQty.Value + 1, pcAmt, ptRmk);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_SETxShiftFixDoc : " + oEx.Message); }
            finally
            {
                oEvent = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_SETxDisconnectFixDoc()
        {
            try
            {
                if (cVB.oVB_MQModel_FixDocNo != null || cVB.oVB_MQModel_FixDocNo.IsOpen)
                {
                    cVB.oVB_MQModel_FixDocNo.Close();
                    cVB.oVB_MQModel_FixDocNo.Dispose();
                    cVB.oVB_MQModel_FixDocNo = null; 
                }
                if (cVB.oVB_MQConn_FixDocNo != null || cVB.oVB_MQConn_FixDocNo.IsOpen)
                {
                    cVB.oVB_MQConn_FixDocNo.Close();
                    cVB.oVB_MQConn_FixDocNo.Dispose();
                    cVB.oVB_MQConn_FixDocNo = null;
                }
            }
            catch (Exception oEx) {  }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        public static void C_GENxListenFixDoc(bool pbFrmHQ)
        {
            try
            {
                C_SETxDisconnectFixDoc();
                cRabbitMQ.C_SETxMQEnpoint(pbFrmHQ);
                cVB.oVB_MQConn_FixDocNo = cVB.oVB_MQFactory.CreateConnection();
                cVB.oVB_MQModel_FixDocNo = cVB.oVB_MQConn_FixDocNo.CreateModel();
                cVB.oVB_MQModel_FixDocNo.QueueDeclare($"PS_QFixDoc{cVB.tVB_BchCode}{cVB.tVB_PosCode}", false, false, false, null);

                cVB.oVB_MQEvent_FixDocNo = new EventingBasicConsumer(cVB.oVB_MQModel_FixDocNo);
                cVB.oVB_MQEvent_FixDocNo.Received += C_PRCxFixDocEvent;
                cVB.oVB_MQModel_FixDocNo.BasicConsume(queue: $"PS_QFixDoc{cVB.tVB_BchCode}{cVB.tVB_PosCode}",
                                                       autoAck: true,
                                                       consumer: cVB.oVB_MQEvent_FixDocNo);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GENxListenFixDoc : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }


        public static void C_PRCxFixDocEvent(object sender, BasicDeliverEventArgs e)
        {
            cDatabase oDB;
            StringBuilder oSql;
            cmlCheckListSalDoc oFixDoc;
            string tMsg = Encoding.UTF8.GetString(e.Body);
            try
            {
                oDB = new cDatabase();
                oSql = new StringBuilder();

                oFixDoc = JsonConvert.DeserializeObject<cmlCheckListSalDoc>(tMsg);
                if (oFixDoc == null) return;
                foreach(cmlListSalDoc oDoc in oFixDoc.paData)
                {
                    //C_PRCxFixDocFrmBackOffice(80, oDoc.ptFTShfCode, oDoc.ptFTBdhDocNo);
                    new cLog().C_WRTxLog("cShift", MethodBase.GetCurrentMethod().Name + $" : Please Fix Bill {oDoc.ptFTBdhDocNo}....", cVB.bVB_AlwPrnLog); //*Net Stamp
                    cSale.C_PRCxAdd2TmpLogChg(80, oDoc.ptFTBdhDocNo);
                    cSale.C_PRCxAdd2TmpLogChg(80, oDoc.ptFTBdhDocNo, true);
                }
                cVB.nVB_FixDoc++; //*Net 63-07-30 ตรวจสอบว่ารับการซ่อมมากี่ครั้ง
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxShift2BackOffice : " + oEx.Message); }
            finally
            {
                oFixDoc = null;
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        public static void C_PRCxFixDocFrmBackOffice(int pnType,string ptShfCode, string ptDocNo)
        {
            cDatabase oDB;
            StringBuilder oSql;
            int nDoc;
            try
            {
                oDB = new cDatabase();
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine($"SELECT COUNT(*) FROM TCNTTmpLogChg WITH(NOLOCK)");
                oSql.AppendLine($"WHERE FTLogCode='{ptShfCode}' AND FTLogDocNo='{ptDocNo}' AND FNLogType={pnType}");
                nDoc = oDB.C_GEToDataQuery<int>(oSql.ToString());
                if (nDoc==0)
                {
                    cSale.C_PRCxAdd2TmpLogChg(pnType, ptDocNo);
                    cSale.C_PRCxAdd2TmpLogChg(pnType, ptDocNo, true);
                }
                else
                {
                    oSql.Clear();
                    oSql.AppendLine($"UPDATE TCNTTmpLogChg");
                    oSql.AppendLine($"SET FTLogStaPrc=''");
                    oSql.AppendLine($"WHERE FTLogCode='{ptShfCode}' AND FTLogDocNo='{ptDocNo}' AND FNLogType={pnType}");
                    oDB.C_SETxDataQuery(oSql.ToString());
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_PRCxFixDocFrmBackOffice : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

            /// <summary>
            /// Net 63-06-04
            /// Get all bill by Shift
            /// </summary>
            /// <param name="ptBchCode"></param>
            /// <param name="ptPosCode"></param>
            /// <param name="ptShfCode"></param>
            /// <returns></returns>
            public static List<cmlListSalDoc> C_GETaDocNoFrmShift(string ptBchCode,string ptPosCode,string ptShfCode)
        {
            cDatabase oDB;
            StringBuilder oSql;
            List<cmlListSalDoc> aoDocNo;
            try
            {
                oDB = new cDatabase();
                oSql = new StringBuilder();
                oSql.Clear();
                //oSql.AppendLine($"SELECT FTBchCode AS ptFTBchCode,FTShfCode AS ptFTShfCode,FTPosCode AS ptFTPosCode");
                //oSql.AppendLine($",FDXshDocDate AS ptFDXshDocDate,FTXshDocNo AS ptFTBdhDocNo");
                //oSql.AppendLine($"FROM {cSale.tC_TblSalHD} HD WITH(NOLOCK)");
                //oSql.AppendLine($"WHERE HD.FTBchCode='{ptBchCode}' AND HD.FTPosCode='{ptPosCode}' AND HD.FTShfCode='{ptShfCode}' AND HD.FTXshStaDoc='1'");
                //oSql.AppendLine("UNION");
                //oSql.AppendLine($"SELECT FTBchCode AS ptFTBchCode,FTShfCode AS ptFTShfCode,FTPosCode AS ptFTPosCode");
                //oSql.AppendLine($",FDXshDocDate AS ptFDXshDocDate,FTXshDocNo AS ptFTBdhDocNo");
                //oSql.AppendLine($"FROM TPSTSalHD HD WITH(NOLOCK)");
                //oSql.AppendLine($"WHERE HD.FTBchCode='{ptBchCode}' AND HD.FTPosCode='{ptPosCode}' AND HD.FTShfCode='{ptShfCode}' AND HD.FTXshStaDoc='1'");
                //*Net 63-07-25 เอาบิลมาจากตาราง Tmp ด้วย
                oSql.AppendLine($"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TSHD{ptPosCode}'))");
                oSql.AppendLine("BEGIN");
                oSql.AppendLine($"  SELECT FTBchCode AS ptFTBchCode,FTShfCode AS ptFTShfCode,FTPosCode AS ptFTPosCode");
                oSql.AppendLine($"  ,FDXshDocDate AS ptFDXshDocDate,FTXshDocNo AS ptFTBdhDocNo");
                oSql.AppendLine($"  FROM TSHD{ptPosCode} HD WITH(NOLOCK)");
                oSql.AppendLine($"  WHERE HD.FTBchCode='{ptBchCode}' AND HD.FTPosCode='{ptPosCode}' AND HD.FTShfCode='{ptShfCode}' AND HD.FTXshStaDoc='1'");
                oSql.AppendLine("   UNION");
                oSql.AppendLine($"  SELECT FTBchCode AS ptFTBchCode,FTShfCode AS ptFTShfCode,FTPosCode AS ptFTPosCode");
                oSql.AppendLine($"  ,FDXshDocDate AS ptFDXshDocDate,FTXshDocNo AS ptFTBdhDocNo");
                oSql.AppendLine($"  FROM TPSTSalHD HD WITH(NOLOCK)");
                oSql.AppendLine($"  WHERE HD.FTBchCode='{ptBchCode}' AND HD.FTPosCode='{ptPosCode}' AND HD.FTShfCode='{ptShfCode}' AND HD.FTXshStaDoc='1'");
                oSql.AppendLine("END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("BEGIN");
                oSql.AppendLine($"  SELECT FTBchCode AS ptFTBchCode,FTShfCode AS ptFTShfCode,FTPosCode AS ptFTPosCode");
                oSql.AppendLine($"  ,FDXshDocDate AS ptFDXshDocDate,FTXshDocNo AS ptFTBdhDocNo");
                oSql.AppendLine($"  FROM TPSTSalHD HD WITH(NOLOCK)");
                oSql.AppendLine($"  WHERE HD.FTBchCode='{ptBchCode}' AND HD.FTPosCode='{ptPosCode}' AND HD.FTShfCode='{ptShfCode}' AND HD.FTXshStaDoc='1'");
                oSql.AppendLine("END");
                //++++++++++++++++++++++++++++++++++++
                aoDocNo = oDB.C_GETaDataQuery<cmlListSalDoc>(oSql.ToString());
                return aoDocNo;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxShift2BackOffice : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return null;
        }

        /// <summary>
        /// Shif Sum Redeem
        /// </summary>
        /// <param name="pnDocType"> 1: แลกรับสินค้า, 2:แลกรับส่วนลด </param>
        /// <returns></returns>
        //private static List<cmlShiftSumRedeem> C_GETaShiftSumRedeem(int pnDocType)
        private static List<cmlShiftSumRedeem> C_GETaShiftSumRedeem(int pnDocType, bool pbTrans=false)//*Net 63-06-04 Default ที่ตาราง Temp
        {
            string tTblSalHD = cSale.tC_TblSalHD;
            string tTblSalDT = cSale.tC_TblSalDT;
            string tTblSalHDCst = cSale.tC_TblSalHDCst;
            string tTblSalDTDis = cSale.tC_TblSalDTDis;
            string tTblSalHDDis = cSale.tC_TblSalHDDis;
            string tTblSalRD = cSale.tC_TblSalRD;
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlShiftSumRedeem> aoSumRed = new List<cmlShiftSumRedeem>();
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                if (pbTrans)
                {
                    tTblSalHD = "TPSTSalHD";
                    tTblSalDT = "TPSTSalDT";
                    tTblSalHDCst = "TPSTSalHDCst";
                    tTblSalDTDis = "TPSTSalDTDis";
                    tTblSalHDDis = "TPSTSalHDDis";
                    tTblSalRD = "TPSTSalRD";
                }

                //oSql.AppendLine("SELECT FTXshCardNo,SUM(FNXrdPntUse) AS FNXrdPntUse, SUM(FCXhdAmt) AS FCXhdAmt");
                //oSql.AppendLine("FROM ( ");
                //oSql.AppendLine("       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo,RD.FNXrdSeqNo,RD.FNXrdPntUse, HDDis.FCXhdAmt FROM TPSTSalHD HD");
                //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                //oSql.AppendLine("       INNER JOIN TPSTSalRD RD ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo ");
                //oSql.AppendLine("       INNER JOIN TPSTSalHDDis HDDis ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
                //oSql.AppendLine("       WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfcode = '" + cVB.tVB_ShfCode + "' AND RD.FTRdhDocType = '2' ");
                //oSql.AppendLine(" )TRD");
                //oSql.AppendLine("GROUP BY FTXshCardNo");
                //oSql.AppendLine("ORDER BY FTXshCardNo");
                //aoSumRed = oDB.C_GETaDataQuery<cmlShiftSumRedeem>(oSql.ToString());

                //*Arm 63-05-28
                if (pnDocType == 1)
                {
                    //oSql.AppendLine("SELECT FTXshCardNo, SUM(FNXrdPntUse) AS FNXrdPntUse, FTXsdBarCode");
                    //oSql.AppendLine("FROM ( ");
                    //oSql.AppendLine("       SELECT DISTINCT HD.FTBchCode,HD.FTXshDocNo,HD.FTCstCode,CST.FTXshCardNo AS FTXshCardNo,RD.FNXrdSeqNo,ISNULL(RD.FNXrdPntUse,0) AS FNXrdPntUse, DT.FTXsdBarCode AS FTXsdBarCode");
                    //oSql.AppendLine("       FROM TPSTSalHD HD with(nolock)");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalDT DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalDTDis DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode");
                    //oSql.AppendLine("       WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfcode = '" + cVB.tVB_ShfCode + "' AND RD.FTRdhDocType = '1' ");
                    //oSql.AppendLine(" )TRD");
                    //oSql.AppendLine("GROUP BY FTXshCardNo,FTXsdBarCode");
                    //oSql.AppendLine("ORDER BY FTXshCardNo");

                    //*Arm 63-05-29 - แก้ไขแสดงแต้มที่ใช้ไปจริง
                    oSql.AppendLine("SELECT FTXshCardNo, FNXrdPntUse, FTXsdBarCode");
                    oSql.AppendLine("FROM( ");
                    oSql.AppendLine("   SELECT FTXshCardNo AS FTXshCardNo, SUM(FNXrdPntUse) AS FNXrdPntUse, FTXsdBarCode AS FTXsdBarCode");
                    oSql.AppendLine("   FROM( "); // Use Redeem 
                    oSql.AppendLine("       SELECT DISTINCT HD.FTBchCode, HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo AS FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) AS FNXrdPntUse, DT.FTXsdBarCode AS FTXsdBarCode ");
                    //oSql.AppendLine("       FROM TPSTSalHD HD with(nolock) ");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo ");
                    //oSql.AppendLine("       INNER JOIN TPSTSalDT DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo ");
                    //oSql.AppendLine("       INNER JOIN TPSTSalDTDis DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo ");
                    //oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode ");
                    oSql.AppendLine($"       FROM {tTblSalHD} HD with(nolock) ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDCst} CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalDT} DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalDTDis} DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalRD} RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode ");
                    oSql.AppendLine("       WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfcode = '" + cVB.tVB_ShfCode + "' AND RD.FTRdhDocType = '1'  AND HD.FNXshDocType = 1 ");
                    oSql.AppendLine("           AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                    oSql.AppendLine("       UNION ALL "); // Refund Redeem
                    oSql.AppendLine("       SELECT DISTINCT HD.FTBchCode, HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo AS FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) * (-1) AS FNXrdPntUse, DT.FTXsdBarCode AS FTXsdBarCode");
                    //oSql.AppendLine("       FROM TPSTSalHD HD with(nolock) ");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalDT DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalDTDis DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode");
                    oSql.AppendLine($"       FROM {tTblSalHD} HD with(nolock) ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDCst} CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalDT} DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalDTDis} DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalRD} RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode");
                    oSql.AppendLine("       WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfcode = '" + cVB.tVB_ShfCode + "' AND RD.FTRdhDocType = '1'  AND HD.FNXshDocType = 9");
                    oSql.AppendLine("         AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                    oSql.AppendLine("   )TRD");
                    oSql.AppendLine("   GROUP BY FTXshCardNo, FTXsdBarCode");
                    oSql.AppendLine(")T");
                    oSql.AppendLine("WHERE FNXrdPntUse > 0");
                    oSql.AppendLine("ORDER BY FTXshCardNo");
                }
                else
                {
                    //oSql.AppendLine("SELECT FTXshCardNo,SUM(FNXrdPntUse) AS FNXrdPntUse, SUM(FCXhdAmt) AS FCXhdAmt");
                    //oSql.AppendLine("FROM ( ");
                    //oSql.AppendLine("       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo,RD.FNXrdSeqNo,ISNULL(RD.FNXrdPntUse,0) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt,0) AS FCXhdAmt FROM TPSTSalHD HD with(nolock)");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo ");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDDis HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
                    //oSql.AppendLine("       WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfcode = '" + cVB.tVB_ShfCode + "' AND RD.FTRdhDocType = '2' ");
                    //oSql.AppendLine(" )TRD");
                    //oSql.AppendLine("GROUP BY FTXshCardNo");
                    //oSql.AppendLine("ORDER BY FTXshCardNo");

                    //*Arm 63-05-29 - แก้ไขแสดงแต้มที่ใช้ไปจริง
                    oSql.AppendLine("SELECT FTXshCardNo, FNXrdPntUse, FCXhdAmt");
                    oSql.AppendLine("FROM(");
                    oSql.AppendLine("   SELECT FTXshCardNo AS FTXshCardNo, SUM(FNXrdPntUse) AS FNXrdPntUse, SUM(FCXhdAmt) AS FCXhdAmt");
                    oSql.AppendLine("   FROM(");
                    //oSql.AppendLine("       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt, 0) AS FCXhdAmt FROM TPSTSalHD HD with(nolock)");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDDis HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
                    oSql.AppendLine($"       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt, 0) AS FCXhdAmt FROM {tTblSalHD} HD with(nolock)");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDCst} CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalRD} RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDDis} HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
                    oSql.AppendLine("       WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfcode = '" + cVB.tVB_ShfCode + "' AND RD.FTRdhDocType = '2' AND HD.FNXshDocType = 1");
                    oSql.AppendLine("           AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                    oSql.AppendLine("       UNION ALL");
                    //oSql.AppendLine("       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) * (-1) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt, 0) * (-1) AS FCXhdAmt FROM TPSTSalHD HD with(nolock)");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo");
                    //oSql.AppendLine("       INNER JOIN TPSTSalHDDis HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
                    oSql.AppendLine($"       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) * (-1) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt, 0) * (-1) AS FCXhdAmt FROM {tTblSalHD} HD with(nolock)");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDCst} CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalRD} RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDDis} HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
                    oSql.AppendLine("       WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfcode = '" + cVB.tVB_ShfCode + "' AND RD.FTRdhDocType = '2' AND HD.FNXshDocType = 9");
                    oSql.AppendLine("           AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                    oSql.AppendLine("   )TRD");
                    oSql.AppendLine("   GROUP BY FTXshCardNo");
                    oSql.AppendLine(")T");
                    oSql.AppendLine("WHERE FNXrdPntUse > 0");
                    oSql.AppendLine("ORDER BY FTXshCardNo");
                }
                //+++++++++++++

                aoSumRed = oDB.C_GETaDataQuery<cmlShiftSumRedeem>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cShift", "C_GETaShiftSumRedeem : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return aoSumRed;
        }


        #region Print
        /// <summary>
        /// Process Print Shift Receive
        /// </summary>
        public static void C_PRCxPrintShiftRCV()
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            try
            {
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();

                oDoc.DocumentName = "ShiftRCV" + cVB.tVB_ShfCode;
                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                oDoc.PrintController = new StandardPrintController();
                oDoc.PrintPage += C_PRNxShiftRCV;
                oDoc.Print();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_PRCxPrintShiftRCV : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                //oSP.SP_CLExMemory();
            }
        }

        private static void C_PRNxShiftRCV(object sender, PrintPageEventArgs e)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            //cSlipMsg oMsg;
            string tLine, tDeposit;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tAmt;
            decimal cChange = 0;
            List<cmlShiftRcv> aoShiftRcv;
            Image oLogo;
            string tPrint;  //*Em 63-06-09

            try
            {
                nWidth = Convert.ToInt32(e.Graphics.VisibleClipBounds.Width);
                tLine = "------------------------------------------------------------------------";

                if (cVB.nVB_PaperSize == 1)
                {
                    if (nWidth > 215)
                        nWidth = 215;
                }
                else
                {
                    if (nWidth > 280)
                        nWidth = 280;
                }

                oGraphic = e.Graphics;
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

                //*Em 62-10-29
                if (!string.IsNullOrEmpty(cVB.tVB_PathLogo))
                {
                    if (File.Exists(cVB.tVB_PathLogo))
                    {
                        oLogo = Image.FromFile(cVB.tVB_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle(nWidth - 200, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }

                //oMsg = new cSlipMsg();
                //nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg

                //// Get ค่า ID เครื่อง จาก DB
                //oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                //nStartY += 18;

                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCS_ShiftRcv").ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                //*Em 63-06-09
                nStartY += 18;
                tPrint = cVB.oVB_GBResource.GetString("tTime").ToString() + " : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                //+++++++++++++++++++++

                //*Em 63-08-11
                // Get ค่า ID เครื่อง จาก DB
                nStartY += 18;
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                //+++++++++++++

                nStartY += 10;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 18;
                oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tCS_Seq"), cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tCS_RcvType"), cVB.aoVB_PInvLayout[2], Brushes.Black, 30, nStartY);
                oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tCS_Amt"), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(110, nStartY, nWidth - 110, 18), oFormatFar);

                aoShiftRcv = C_GETaShiftRcv();
                if (aoShiftRcv != null)
                {
                    foreach (cmlShiftRcv oShift in aoShiftRcv)
                    {
                        nStartY += 18;
                        oGraphic.DrawString(" " + oShift.FNSdtSeqNo.ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                        oGraphic.DrawString(" " + oShift.FTRcvName, cVB.aoVB_PInvLayout[3], Brushes.Black, 30, nStartY);

                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oShift.FCRcvPayAmt), cVB.nVB_DecShow);
                        oGraphic.DrawString(" " + tAmt, cVB.aoVB_PInvLayout[4], Brushes.Black, new RectangleF(110, nStartY, nWidth - 110, 18), oFormatFar);
                    }
                }
                nStartY += 10;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 18;
                oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tCS_TotalAmt"), cVB.aoVB_PInvLayout[5], Brushes.Black, 0, nStartY);
                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(aoShiftRcv.Sum(a => a.FCRcvPayAmt)), cVB.nVB_DecShow);
                oGraphic.DrawString(" " + tAmt, cVB.aoVB_PInvLayout[6], Brushes.Black, new RectangleF(110, nStartY, nWidth - 110, 18), oFormatFar);
                nStartY += 18;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 30;
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCS_Receiver").ToString() + " (..............................)", cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 30;
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCS_Sender").ToString() + " (..............................)", cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 18;
                //oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_PRNxShiftRCV : " + oEx.Message); }
            finally
            {
                if (oGraphic != null)
                    oGraphic.Dispose();

                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();

                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                //oMsg = null;
                tLine = null;
                //oSP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process print banknote
        /// </summary>
        public static void C_PRCxPrintShiftBN()
        {
            try
            {
                PrintDocument oDoc = null;
                PrinterSettings oSettings;
                cSP oSP = new cSP();
                try
                {
                    oDoc = new PrintDocument();
                    oSettings = new PrinterSettings();

                    oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                    oDoc.PrintController = new StandardPrintController();
                    oDoc.PrintPage += C_PRNxShiftBN;
                    oDoc.Print();
                }
                catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_PRCxPrintShiftRCV : " + oEx.Message); }
                finally
                {
                    if (oDoc != null)
                        oDoc.Dispose();

                    oDoc = null;
                    oSettings = null;
                    //oSP.SP_CLExMemory();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_PRCxPrintShiftBN : " + oEx.Message); }
        }

        private static void C_PRNxShiftBN(object sender, PrintPageEventArgs e)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            //cSlipMsg oMsg;
            string tLine;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tAmt;
            List<cmlShiftBN> aoShiftBN;
            string tMsg = "";
            Image oLogo;

            try
            {
                nWidth = Convert.ToInt32(e.Graphics.VisibleClipBounds.Width);
                tLine = "........................................................................";

                if (cVB.nVB_PaperSize == 1)
                {
                    if (nWidth > 215)
                        nWidth = 215;
                }
                else
                {
                    if (nWidth > 280)
                        nWidth = 280;
                }

                oGraphic = e.Graphics;
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

                //*Em 62-10-29
                if (!string.IsNullOrEmpty(cVB.tVB_PathLogo))
                {
                    if (File.Exists(cVB.tVB_PathLogo))
                    {
                        oLogo = Image.FromFile(cVB.tVB_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle(nWidth - 200, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }

                //oMsg = new cSlipMsg();
                //nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg

                tMsg = cVB.oVB_GBResource.GetString("tBanknote").ToString();
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tCashier").ToString() + " : " + cVB.tVB_UsrName;
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tShiftCode").ToString() + " : " + cVB.tVB_ShfCode;
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tTime").ToString() + " : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                //*Em 63-08-12
                // Get ค่า ID เครื่อง จาก DB
                nStartY += 18;
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                //+++++++++++++

                nStartY += 10;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tType") + "  (" + cVB.oVB_GBResource.GetString("tQty") + ")";
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth / 2, 18));
                tMsg = cVB.oVB_GBResource.GetString("tCS_Amt");
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(nWidth / 2, nStartY, (nWidth / 2) - 10, 18), oFormatFar);

                nStartY += 10;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                aoShiftBN = C_GETaShiftBN();
                if (aoShiftBN != null)
                {
                    foreach (cmlShiftBN oShift in aoShiftBN)
                    {
                        nStartY += 18;
                        tMsg = oShift.FTBntName + "  (" + oSP.SP_SETtDecShwSve(1, (decimal)(oShift.FNKbnQty), cVB.nVB_DecShow) + ")";
                        oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(0, nStartY, nWidth / 2, 15));
                        tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oShift.FCKbnAmt), cVB.nVB_DecShow);
                        oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[4], Brushes.Black, new RectangleF(nWidth / 2, nStartY, (nWidth / 2) - 10, 18), oFormatFar);
                    }
                }
                nStartY += 10;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tSumCash");
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth / 2, 18));
                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(aoShiftBN.Sum(a => a.FCKbnAmt)), cVB.nVB_DecShow);
                oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[6], Brushes.Black, new RectangleF(nWidth / 2, nStartY, (nWidth / 2) - 10, 18), oFormatFar);

                nStartY += 10;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                nStartY += 30;
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCS_Receiver").ToString() + " (..............................)", cVB.aoVB_PInvLayout[7], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 30;
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCS_Sender").ToString() + " (..............................)", cVB.aoVB_PInvLayout[7], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 18;
                //oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_PRNxShiftBN : " + oEx.Message); }
            finally
            {
                if (oGraphic != null)
                    oGraphic.Dispose();

                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();

                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                //oMsg = null;
                tLine = null;
                //oSP.SP_CLExMemory();
            }
        }

        public static void C_PRNxSaleSumRpt(Graphics poGraphic, bool pbDrawHF = true)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            //cSlipMsg oMsg;
            string tLine;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tLastRcv;
            decimal cLastAmt, cSumShift;
            decimal cSumSale, cSumRet, cSumCashSale; //*Net 63-07-30
            Font oFont = new Font("CordiaUPC", Convert.ToSingle(11.5), FontStyle.Regular);
            string tMsg = "";
            List<cmlTPSTShiftSRatePdt> aoShiftSRatePdt;
            List<cmlTPSTShiftSLastDoc> aoShiftSLastDoc;
            List<cmlTPSTShiftSSumRcv> aoShiftSSumRcv;
            cmlShiftSum oSumShift;
            Image oLogo;
            //List<cmlShiftSumRedeem> aoShiftSumRedee; //*Arm 63-05-27
            List<cmlShiftSumRedeem> aoShiftSumRedeemDis; //*Net 63-07-30 ปรับตาม Moshi
            List<cmlShiftSumRedeem> aoShiftSumRedeemPdt; //*Net 63-07-30 ปรับตาม Moshi
            try
            {
                nWidth = Convert.ToInt32(poGraphic.VisibleClipBounds.Width);
                tLine = "--------------------------------------------------------------------------------------------";

                if (cVB.nVB_PaperSize == 1)
                {
                    if (nWidth > 215)
                        nWidth = 215;
                }
                else
                {
                    if (nWidth > 280)
                        nWidth = 280;
                }

                oGraphic = poGraphic;
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

                //*Em 62-10-29
                if (!string.IsNullOrEmpty(cVB.tVB_PathLogo))
                {
                    if (File.Exists(cVB.tVB_PathLogo))
                    {
                        oLogo = Image.FromFile(cVB.tVB_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle(nWidth - 200, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }

                //oMsg = new cSlipMsg();
                //if (pbDrawHF)
                //{
                //    nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg
                //}

                tMsg = cVB.oVB_GBResource.GetString("tCS_ShiftSUM").ToString();
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                oFont = cVB.aoVB_PInvLayout[3];
                //----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                ////!!!ปิดการขาย!!!
                //nStartY += 18;
                //tMsg = cVB.oVB_GBResource.GetString("tSaleDate") + ":" + Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");
                //tMsg += " " + cVB.oVB_GBResource.GetString("tPos") + ":" + cVB.tVB_PosCode;
                //tMsg += " " + cVB.oVB_GBResource.GetString("tUsr") + ":" + cVB.tVB_UsrName;
                //oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);

                //*Em 63-08-12
                // Get ค่า ID เครื่อง จาก DB
                nStartY += 18;
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                //+++++++++++++

                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รายการ ความถี่ มูลค่า
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tItem");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = cVB.oVB_GBResource.GetString("tFreq");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatCenter);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = cVB.oVB_GBResource.GetString("tAmount");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รายการต่างๆ
                cSumShift = 0;
                oSumShift = new cmlShiftSum();
                oSumShift = C_GEToShiftSum();

                //เปิดลิ้นชักไม่ขาย
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tOpDrwNoS");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nCntOpDrw, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

                //ยกเลิกบิล
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tCancelBill");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nCntCancelBill, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

                //พักบิล
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tHoldBill");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nCntHoldBill, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

                //นำเงินเข้า
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tMnyIn");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nMnyInCnt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cMnyInAmt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //นำเงินออก //*Net 63-07-30 ปรับตาม Moshi
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tMnyOut");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nMnyOutCnt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cMnyOutAmt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //ยอดขาย
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tSaleAmt");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nSaleCnt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cSaleAmt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //ยอดคืนสินค้า
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tRefundAmt");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nRefundCnt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cRefundAmt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //ยอดปัดเศษ //*Net 63-07-30 เพิ่ม
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tRoundRcv");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);

                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cRoundAmt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รวมเงิน
                nStartY += 18;
                tMsg = "    " + cVB.oVB_GBResource.GetString("tTotal") + ":";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                //cSumShift = (oSumShift.cMnyInAmt + oSumShift.cSaleAmt) - (oSumShift.cMnyOutAmt + oSumShift.cRefundAmt);
                cSumShift = (oSumShift.cMnyInAmt + oSumShift.cSaleAmt + oSumShift.cRoundAmt) - (oSumShift.cMnyOutAmt + oSumShift.cRefundAmt); //*Net 63-07-30 เพิ่มยอดปัดเศษ
                tMsg = oSP.SP_SETtDecShwSve(1, cSumShift, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(151, nStartY, 120, 18), oFormatFar);

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //สรุปยอดรับชำระตามประเภทการชำระเงิน
                aoShiftSSumRcv = new List<cmlTPSTShiftSSumRcv>();
                aoShiftSSumRcv = C_GETaShiftSumRcv();
                tLastRcv = "";
                cLastAmt = 0;
                //*Net 63-07-30 ปรับตาม Moshi
                cSumSale = 0;
                cSumRet = 0;
                cSumCashSale = 0;
                //++++++++++++++++++
                if (aoShiftSSumRcv.Count > 0)
                {
                    foreach (cmlTPSTShiftSSumRcv oShift in aoShiftSSumRcv)
                    {
                        //*Net 63-07-30 ปรับตาม Moshi
                        if (!String.IsNullOrEmpty(tLastRcv) && tLastRcv != oShift.FTRcvCode)
                        {
                            //-----------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                        }


                        //*Net 63-07-30 ปรับตาม Moshi มีทั้งบิลขาย และคืน
                        if (aoShiftSSumRcv.Count(oCountShf => oCountShf.FTRcvCode == oShift.FTRcvCode) > 1)
                        {
                            //if (string.Equals(oShift.FTRcvCode, tLastRcv))
                            //{
                            if (string.Equals(oShift.FTRcvDocType, "9"))
                            {
                                tMsg = cVB.oVB_GBResource.GetString("tRefund") + " " + oShift.FTRcvName;
                                cSumRet += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                if (oShift.FTFmtCode == "001") cSumCashSale -= Convert.ToDecimal(oShift.FCRcvPayAmt);

                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                                //-----------------------
                                nStartY += 10;
                                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                                //รวมเงิน
                                nStartY += 18;
                                tMsg = cVB.oVB_GBResource.GetString("tTotal") + " " + oShift.FTRcvName; ;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(cLastAmt - oShift.FCRcvPayAmt), cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                            }
                            else
                            {
                                tMsg = cVB.oVB_GBResource.GetString("tSale") + " " + oShift.FTRcvName;
                                cSumSale += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                if (oShift.FTFmtCode == "001") cSumCashSale += Convert.ToDecimal(oShift.FCRcvPayAmt);

                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                            }
                        }
                        //* มีแค่ขาย หรือคืนอย่างเดียว
                        else
                        {
                            //*มียอดคืนอย่างเดียว
                            if (string.Equals(oShift.FTRcvDocType, "9"))
                            {
                                //*พิมพ์ยอดบิลขายเป็น 0.00
                                tMsg = cVB.oVB_GBResource.GetString("tSale") + " " + oShift.FTRcvName;
                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                                //*พิมพ์ยอดบิลคืนปกติ
                                tMsg = cVB.oVB_GBResource.GetString("tRefund") + " " + oShift.FTRcvName;
                                cSumRet += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                if (oShift.FTFmtCode == "001") cSumCashSale -= Convert.ToDecimal(oShift.FCRcvPayAmt);
                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                                cLastAmt = -Convert.ToDecimal(oShift.FCRcvPayAmt);
                            }
                            //*มียอดขายอย่างเดียว
                            else
                            {
                                tMsg = cVB.oVB_GBResource.GetString("tSale") + " " + oShift.FTRcvName;
                                cSumSale += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                if (oShift.FTFmtCode == "001") cSumCashSale += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                                cLastAmt = Convert.ToDecimal(oShift.FCRcvPayAmt);

                                //*พิมพ์ยอดบิลคืนเป็น 0.00
                                tMsg = cVB.oVB_GBResource.GetString("tRefund") + " " + oShift.FTRcvName;
                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                            }
                            //-----------------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                            //รวมเงิน
                            nStartY += 18;
                            tMsg = cVB.oVB_GBResource.GetString("tTotal") + " " + oShift.FTRcvName; ;
                            oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                            tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(cLastAmt), cVB.nVB_DecShow);
                            oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                        }
                        tLastRcv = oShift.FTRcvCode;
                        cLastAmt = Convert.ToDecimal(oShift.FCRcvPayAmt);

                        //*Net 63-06-19 พิมพ์ขีดคั่นตอนจบรายการสุดท้าย
                        if (oShift == aoShiftSSumRcv.LastOrDefault())
                        {
                            //-----------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                        }
                    }
                }

                //*Net 63-07-30 ย้ายไปพิมพ์ใน loop บน
                ////-----------------
                //nStartY += 10;
                //oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //*Net 63-07-30 ย้ายไป if ล่าง
                ////สรุปการรับชำระสกุลเงิน
                //nStartY += 18;
                //tMsg = "    " + cVB.oVB_GBResource.GetString("tSumCurrency");
                //oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                ////-----------------
                //nStartY += 10;
                //oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                ////ส่วนหัวสกุลเงิน
                //nStartY += 18;
                //tMsg = cVB.oVB_GBResource.GetString("tCurrency");
                //oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                //tMsg = cVB.oVB_GBResource.GetString("tAmount");
                //oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                ////-----------------
                //nStartY += 10;
                //oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รายละเอียดสกุลเงิน
                aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
                aoShiftSRatePdt = C_GETaShiftSRate();
                if (aoShiftSRatePdt.Count > 0)
                {
                    //สรุปการรับชำระสกุลเงิน
                    nStartY += 18;
                    tMsg = "    " + cVB.oVB_GBResource.GetString("tSumCurrency");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                    //ส่วนหัวสกุลเงิน
                    nStartY += 18;
                    tMsg = cVB.oVB_GBResource.GetString("tCurrency");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                    tMsg = cVB.oVB_GBResource.GetString("tAmount");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                    foreach (cmlTPSTShiftSRatePdt oData in aoShiftSRatePdt)
                    {
                        nStartY += 18;
                        tMsg = oData.FTSrpNameRef;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                        tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpAmt), cVB.nVB_DecShow);
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                    }

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                }

                //*Net 63-07-30 ย้ายไป if บน
                ////-----------------
                //nStartY += 10;
                //oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รายการสินค้าควบคุม
                aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
                aoShiftSRatePdt = C_GETaShiftSPdt();
                if (aoShiftSRatePdt.Count > 0)
                {
                    //สรุปยอดขายสินค้าควบคุมพิเศษ
                    nStartY += 18;
                    tMsg = "    " + cVB.oVB_GBResource.GetString("tPdtSpcSum");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                    //ส่วนหัวสินค้าควบคุม
                    nStartY += 18;
                    tMsg = cVB.oVB_GBResource.GetString("tItem");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = cVB.oVB_GBResource.GetString("tQty");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, 50, 18), oFormatCenter);
                    tMsg = cVB.oVB_GBResource.GetString("tAmount");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                    foreach (cmlTPSTShiftSRatePdt oData in aoShiftSRatePdt)
                    {
                        nStartY += 18;
                        tMsg = oData.FTSrpNameRef;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpQty), cVB.nVB_DecShow);
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, 50, 18), oFormatFar);
                        tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpAmt), cVB.nVB_DecShow);
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
                    }

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                }

                //*Net 63-07-30 ย้ายไป if บน
                ////-----------------
                //nStartY += 10;
                //oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);


                //*Net 63-07-30 พิมพ์สรุปยอด
                //สรุปยอดเงินขาย
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tSumSale");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, cSumSale, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //สรุปยอดเงินคืน
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tSumRet");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, cSumRet, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //สรุปยอดเงินในลิ้นชัก
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tSumDrw");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                //tMsg = oSP.SP_SETtDecShwSve(1, cSumCashSale + oSumShift.cRoundAmt + oSumShift.cMnyInAmt - oSumShift.cMnyOutAmt, cVB.nVB_DecShow);
                tMsg = oSP.SP_SETtDecShwSve(1, cSumCashSale + oSumShift.cMnyInAmt - oSumShift.cMnyOutAmt, cVB.nVB_DecShow); //*Net 63-07-10 ไม่รวมยอดปัดเศษ
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                //+++++++++++++++++++++++++++++++++++

                //ช่วงบิล
                aoShiftSLastDoc = new List<cmlTPSTShiftSLastDoc>();
                aoShiftSLastDoc = C_GETaShiftSLastDoc();

                if (aoShiftSLastDoc.Count == 0)
                {
                    //บิลขาย จาก
                    nStartY += 18;
                    tMsg = cVB.oVB_GBResource.GetString("tSaleFrm");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = "--------N/A--------";
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    //บิลขาย ถึง
                    nStartY += 18;
                    tMsg = cVB.oVB_GBResource.GetString("tSaleTo");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = "--------N/A--------";
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    //บิลคืน จาก
                    nStartY += 18;
                    tMsg = cVB.oVB_GBResource.GetString("tRefundFrm");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = "--------N/A--------";
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    //บิลคืน ถึง
                    nStartY += 18;
                    tMsg = cVB.oVB_GBResource.GetString("tRefundTo");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = "--------N/A--------";
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                }
                else
                {
                    cmlTPSTShiftSLastDoc oShiftSLastDoc = new cmlTPSTShiftSLastDoc();
                    oShiftSLastDoc = aoShiftSLastDoc.Where(a => a.FNLstDocType == 1).FirstOrDefault();
                    if (oShiftSLastDoc == null)
                    {
                        //บิลขาย จาก
                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tSaleFrm");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = "--------N/A--------";
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                        //บิลขาย ถึง
                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tSaleTo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = "--------N/A--------";
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    }
                    else
                    {
                        //บิลขาย จาก
                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tSaleFrm");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oShiftSLastDoc.FTLstDocNoFrm;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                        //บิลขาย ถึง
                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tSaleTo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oShiftSLastDoc.FTLstDocNoTo;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    }

                    oShiftSLastDoc = new cmlTPSTShiftSLastDoc();
                    oShiftSLastDoc = aoShiftSLastDoc.Where(a => a.FNLstDocType == 9).FirstOrDefault();
                    if (oShiftSLastDoc == null)
                    {
                        //บิลคืน จาก
                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tRefundFrm");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = "--------N/A--------";
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                        //บิลคืน ถึง
                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tRefundTo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = "--------N/A--------";
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    }
                    else
                    {
                        //บิลคืน จาก
                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tRefundFrm");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oShiftSLastDoc.FTLstDocNoFrm;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                        //บิลคืน ถึง
                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tRefundTo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oShiftSLastDoc.FTLstDocNoTo;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    }
                }


                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //(*Arm 63-05-27)
                //สรุปการแลกแต้ม รับส่วนลด
                if (cVB.bVB_PrnShiftSumRedeem == true)
                {
                    //*Net 63-07-30 ย้ายมาไว้ข้างบน
                    //รายละเอียดการแลกแต้ม รับส่วนลด
                    aoShiftSumRedeemDis = new List<cmlShiftSumRedeem>();
                    aoShiftSumRedeemDis = C_GETaShiftSumRedeem(2);

                    //รายละเอียดการแลกแต้ม รับสินค้า
                    aoShiftSumRedeemPdt = new List<cmlShiftSumRedeem>();
                    aoShiftSumRedeemPdt = C_GETaShiftSumRedeem(1);
                    //++++++++++++++++++++++++++++++++++++

                    //*Net 63-07-30 ถ้ามีข้อมูลค่อยพิมพ์หัว
                    if (aoShiftSumRedeemDis.Count > 0 || aoShiftSumRedeemPdt.Count > 0)
                    {
                        nStartY += 18;
                        tMsg = "    " + cVB.oVB_GBResource.GetString("tSumRedeem");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                        //-----------------
                        nStartY += 10;
                        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                        //ส่วนหัวการแลกแต้ม รับส่วนลด

                        nStartY += 18;
                        tMsg = cVB.oVB_GBResource.GetString("tCardNo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
                        tMsg = cVB.oVB_GBResource.GetString("tUsePnt");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatCenter);
                        tMsg = cVB.oVB_GBResource.GetString("tAmtDis");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);

                        //-----------------
                        nStartY += 10;
                        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                        //*Net 63-07-30 ย้ายไปข้างบน
                        ////รายละเอียดการแลกแต้ม รับส่วนลด
                        //aoShiftSumRedee = new List<cmlShiftSumRedeem>();
                        //aoShiftSumRedee = C_GETaShiftSumRedeem(2);

                        //if (aoShiftSumRedee.Count > 0)
                        if(aoShiftSumRedeemDis.Count>0)
                        {
                            //foreach (cmlShiftSumRedeem oData in aoShiftSumRedee)
                            foreach (cmlShiftSumRedeem oData in aoShiftSumRedeemDis)
                            {
                                nStartY += 18;
                                tMsg = oData.FTXshCardNo;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FNXrdPntUse), cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatFar);
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCXhdAmt), cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
                            }
                            //*Net 63-07-30 ย้ายมาไว้ใน if
                            //-----------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                        }

                        //*Net 63-07-30 ย้ายไปไว้ข้างบน
                        ////รายละเอียดการแลกแต้ม รับสินค้า (*Arm 63-05-28)
                        //aoShiftSumRedee = new List<cmlShiftSumRedeem>();
                        //aoShiftSumRedee = C_GETaShiftSumRedeem(1);

                        //if (aoShiftSumRedee.Count > 0)
                        if (aoShiftSumRedeemPdt.Count > 0)
                        {
                            //foreach (cmlShiftSumRedeem oData in aoShiftSumRedee)
                            foreach (cmlShiftSumRedeem oData in aoShiftSumRedeemPdt)
                            {
                                nStartY += 18;
                                tMsg = oData.FTXshCardNo;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FNXrdPntUse), cVB.nVB_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatFar);
                                tMsg = oData.FTXsdBarCode;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
                            }

                            //*Net 63-07-30 ย้ายมาไว้ใน if
                            //-----------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                        }
                    }
                }
                //End(*Arm 63-05-27)



                //Print Date,Time
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tPrintDate") + ":" + DateTime.Now.ToString("dd/MM/yyyy");
                tMsg += " " + cVB.oVB_GBResource.GetString("tTime") + ":" + DateTime.Now.ToString("HH:mm:ss");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                ////ท้ายใบเสร็จ
                //if (pbDrawHF)
                //{
                //    nStartY += 30;
                //    nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                //}

                cVB.nVB_StartY = nStartY; //*Arm 63-05-03
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_PRNxDrawTax : " + oEx.Message); }
            finally
            {
                if (oGraphic != null)
                    oGraphic.Dispose();

                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();

                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                //oMsg = null;
                tLine = null;
                //oSP.SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        #endregion Print
    }
}
