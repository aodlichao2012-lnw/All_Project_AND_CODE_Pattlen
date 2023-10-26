using AdaPos.Models.Database;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
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
                    
                    if(string.IsNullOrEmpty(cVB.tVB_OpenShiftHD))   // ปิดรอบแล้ว 
                        new wHome().Show();
                    else
                    {
                        C_CHKxOpenShiftDT();    // ตรวจสอบการเปิดรอบ DT

                        if(string.IsNullOrEmpty(cVB.tVB_OpenShiftDT))   // ปิดรอบย่อยอยู่
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
                                    //tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode);
                                    tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

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
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("FTShdUsrClosed = '"+ cVB.tVB_UsrCode +"'");     //*Em 62-05-25  AdaFC
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
                oSql.AppendLine("FTSdtUsrClosed = '"+ cVB.tVB_UsrCode +"'");     //*Em 62-05-25  AdaFC
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

                cVB.tVB_ShfCode =  cSale.tC_DocFmtLeft + string.Format("{0:" + tFmt + "}", nMax + 1);
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
                oSql.AppendLine("INSERT INTO TPSTShiftHD WITH(ROWLOCK)");
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
                oSql.AppendLine("INSERT INTO TPSTShiftDT WITH(ROWLOCK)");
                oSql.AppendLine("(");
                //oSql.AppendLine("   FTBchCode, FTShfCode, FNSdtSeqNo,");
                oSql.AppendLine("   FTBchCode, FTShfCode, FTPosCode, FNSdtSeqNo,"); //*Em 61-12-27  Water Park
                oSql.AppendLine("   FTShpCode, FTUsrCode, FDSdtSignIn");
                //oSql.AppendLine("   FTSdtTSignIn");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES");
                oSql.AppendLine("(");
                //oSql.AppendLine("   '" + cVB.tVB_BchCode + "', '" + cVB.tVB_ShfCode + "', " + nSeqNo + ",");
                oSql.AppendLine("   '" + cVB.tVB_BchCode + "', '" + cVB.tVB_ShfCode + "', '"+ cVB.tVB_PosCode +"'," + nSeqNo + ",");    //*Em 61-12-27  Water Park
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
                oSql.AppendLine("SELECT FTShfCode, FDShdSaleDate");
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
                oSql.AppendLine("AND FTUsrCode = '"+ cVB.tVB_UsrCode +"'"); //*Em 62-01-28  AdaPos 5.0
                oSql.AppendLine("AND FDSdtSignOut IS NULL");

                cVB.tVB_OpenShiftDT = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                cVB.nVB_ShfSeq = Convert.ToInt32(cVB.tVB_OpenShiftDT);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_CHKxOpenShiftDT : " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"'");
                oSql.AppendLine("AND FTPosCode = '"+ cVB.tVB_PosCode +"'");
                oSql.AppendLine("AND FTShfCode = '"+ cVB.tVB_ShfCode +"'");
                aoShiftRcv = oDB.C_GETaDataQuery<cmlShiftRcv>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftRcv : " + oEx.Message); }
            return aoShiftRcv;
        }

        /// <summary>
        /// Insert LastDoc
        /// </summary>
        public void C_INSxLastDoc()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("INSERT INTO TPSTShiftSLastDoc WITH(ROWLOCK)(FTBchCode, FTPosCode, FTShfCode, FNSdtSeqNo, FNLstDocType, FTLstDocNoFrm, FTLstDocNoTo)");
                oSql.AppendLine("SELECT SHF.FTBchCode,SHF.FTPosCode,SHF.FTShfCode,1 AS FNSdtSeqNo,1 AS FNXshDocType,MIN(FTXshDocNo) AS FTLstDocNoFrm,MAX(FTXshDocNo) AS FTLstDocNoTo");
                oSql.AppendLine("FROM TPSTShiftHD SHF WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TPSTSalHD SAL WITH(NOLOCK) ON SAL.FTBchCode = SHF.FTBchCode AND SAL.FTShfCode = SHF.FTShfCode ");
                oSql.AppendLine("	AND SAL.FNXshDocType = 1");
                oSql.AppendLine("WHERE SHF.FTBchCode = '"+ cVB.tVB_BchCode +"' AND SHF.FTShfCode = '"+ cVB.tVB_ShfCode +"'");
                oSql.AppendLine("GROUP BY SHF.FTBchCode,SHF.FTPosCode,SHF.FTShfCode,FNXshDocType");
                oSql.AppendLine("UNION");
                oSql.AppendLine("SELECT SHF.FTBchCode,SHF.FTPosCode,SHF.FTShfCode,1 AS FNSdtSeqNo,9 AS FNXshDocType,MIN(FTXshDocNo) AS FTLstDocNoFrm,MAX(FTXshDocNo) AS FTLstDocNoTo");
                oSql.AppendLine("FROM TPSTShiftHD SHF WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TPSTSalHD SAL WITH(NOLOCK) ON SAL.FTBchCode = SHF.FTBchCode AND SAL.FTShfCode = SHF.FTShfCode ");
                oSql.AppendLine("	AND SAL.FNXshDocType = 9");
                oSql.AppendLine("WHERE SHF.FTBchCode = '" + cVB.tVB_BchCode + "' AND SHF.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("GROUP BY SHF.FTBchCode,SHF.FTPosCode,SHF.FTShfCode,FNXshDocType");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxLastDoc : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
        public void C_INSxPdtSpc()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("INSERT INTO TPSTShiftSRatePdt WITH(ROWLOCK)");
                oSql.AppendLine("(FTBchCode,FTPosCode,FTShfCode,FNSdtSeqNo,FTSrpCodeRef,FTSrpType,FTSrpNameRef,FCSrpQty,FCSrpAmt)");
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,1 AS FNSdtSeqNo,PDT.FTPdtCode,2 AS FTSrpType,");
                oSql.AppendLine("PDTL.FTPdtNameABB,");
                oSql.AppendLine("SUM(DT.FCXsdQty) AS FCSrpQty,SUM(DT.FCXsdNet) AS FCSrpAmt");
                oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON DT.FTPdtCode = PDT.FTPdtCode");
                oSql.AppendLine("INNER JOIN TPSTSalHD HD WITH(NOLOCK) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                oSql.AppendLine("LEFT JOIN TCNMPdt_L PDTL WITH(NOLOCK) ON PDT.FTPdtCode = PDTL.FTPdtCode AND PDTL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE HD.FTBchCode = '"+  cVB.tVB_BchCode +"' AND HD.FTShfCode = '"+cVB.tVB_ShfCode +"'");
                oSql.AppendLine("AND ISNULL(PDT.FTPdtGrpControl,'') = '1'");
                oSql.AppendLine("GROUP BY HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,PDT.FTPdtCode,PDTL.FTPdtNameABB");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxPdtSpc : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Insert Currencry in Shift
        /// </summary>
        public void C_INSxCurrency()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("INSERT INTO TPSTShiftSRatePdt WITH(ROWLOCK)");
                oSql.AppendLine("(FTBchCode,FTPosCode,FTShfCode,FNSdtSeqNo,FTSrpCodeRef,FTSrpType,FTSrpNameRef,FCSrpQty,FCSrpAmt)");
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,1 AS FNSdtSeqNo,RTE.FTRteCode,1 AS FTSrpType,");
                oSql.AppendLine("RTEL.FTRteName,");
                oSql.AppendLine("COUNT(RC.FTRteCode) AS FCSrpQty,SUM(RC.FCXrcNet) AS FCSrpAmt");
                oSql.AppendLine("FROM TFNMRate RTE WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSTSalRC RC WITH(NOLOCK) ON RC.FTRteCode = RTE.FTRteCode");
                oSql.AppendLine("INNER JOIN TPSTSalHD HD WITH(NOLOCK) ON HD.FTBchCode = RC.FTBchCode AND HD.FTXshDocNo = RC.FTXshDocNo");
                oSql.AppendLine("LEFT JOIN TFNMRate_L RTEL WITH(NOLOCK) ON RTEL.FTRteCode = RTE.FTRteCode AND RTEL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RC.FTRcvCode = RCV.FTRcvCode");
                oSql.AppendLine("WHERE (RCV.FTFmtCode = '001' OR RCV.FTFmtCode = '010')");
                oSql.AppendLine("AND HD.FTBchCode = '"+ cVB.tVB_BchCode +"' AND HD.FTShfCode = '"+ cVB.tVB_ShfCode +"'");
                oSql.AppendLine("GROUP BY HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,RTE.FTRteCode,RTEL.FTRteName");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxCurrency : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("SELECT ISNULL(FTSrpNameRef,(SELECT TOP 1 FTRteName FROM TFNMRate_L WITH(NOLOCK) WHERE FTRteCode = SRP.FTSrpCodeRef)) AS FTSrpNameRef,FCSrpQty,FCSrpAmt");
                oSql.AppendLine("FROM TPSTShiftSRatePdt SRP WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMRate_L RTEL WITH(NOLOCK) ON SRP.FTSrpCodeRef = RTEL.FTRteCode AND RTEL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE SRP.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND SRP.FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("AND SRP.FTShfCode = '" + cVB.tVB_ShfCode + "'");
                oSql.AppendLine("AND SRP.FTSrpType = 1");
                aoShiftSRatePdt = oDB.C_GETaDataQuery<cmlTPSTShiftSRatePdt>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftSRate : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"'");
                oSql.AppendLine("AND FTPosCode = '"+ cVB.tVB_PosCode +"'");
                oSql.AppendLine("AND FTShfCode = '"+ cVB.tVB_ShfCode +"'");
                aoData = oDB.C_GETaDataQuery<cmlTPSTShiftSLastDoc>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftSLastDoc : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
            }
            return aoData;
        }

        public void C_INSxShiftSumRcv()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("INSERT INTO TPSTShiftSSumRcv WITH(ROWLOCK)");
                oSql.AppendLine("(FTBchCode,FTPosCode,FTShfCode,FNSdtSeqNo,FTRcvCode,FTRcvDocType,FCRcvPayAmt)");
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,1 AS FNSdtSeqNo,RC.FTRcvCode,HD.FNXshDocType,SUM(RC.FCXrcNet) AS FCRcvPayAmt");
                oSql.AppendLine("FROM TPSTSalRC RC WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSTSalHD HD WITH(NOLOCK) ON HD.FTBchCode = RC.FTBchCode AND HD.FTXshDocNo = RC.FTXshDocNo");
                oSql.AppendLine("WHERE HD.FTBchCode = '"+cVB.tVB_BchCode +"' AND HD.FTShfCode ='"+ cVB.tVB_ShfCode +"'");
                oSql.AppendLine("GROUP BY HD.FTBchCode,HD.FTPosCode,HD.FTShfCode,RC.FTRcvCode,HD.FNXshDocType");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_INSxShiftSumRcv : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("SSR.FTRcvCode,SSR.FCRcvPayAmt,SSR.FTRcvDocType");
                oSql.AppendLine("FROM TPSTShiftSSumRcv SSR WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCVL.FTRcvCode = SSR.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE SSR.FTBchCode = '"+ cVB.tVB_BchCode +"'");
                oSql.AppendLine("AND SSR.FTPosCode = '"+ cVB.tVB_PosCode +"'");
                oSql.AppendLine("AND SSR.FTShfCode = '"+ cVB.tVB_ShfCode +"'");
                oSql.AppendLine("ORDER BY SSR.FTShfCode,SSR.FTRcvCode,SSR.FTRcvDocType");
                aoData = oDB.C_GETaDataQuery<cmlTPSTShiftSSumRcv>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftSumRcv : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
            }
            return aoData;
        }

        private static cmlShiftSum C_GEToShiftSum()
        {
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

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTShfCode,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 1 THEN 1 ELSE 0 END) AS FNCntSale,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 1 THEN HD.FCXshGrand ELSE 0 END) AS FCAmtSale,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 9 THEN 1 ELSE 0 END) AS FNCntRef,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 9 THEN HD.FCXshGrand ELSE 0 END) AS FCAmtRef");
                oSql.AppendLine("FROM TPSTSalHD HD WITH(NOLOCK)");
                oSql.AppendLine("WHERE HD.FTBchCode = '"+ cVB.tVB_BchCode +"' AND HD.FTShfCode = '"+ cVB.tVB_ShfCode +"' ");
                oSql.AppendLine("AND HD.FTPosCode = '"+ cVB.tVB_PosCode +"' ");
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
                oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"'");
                oSql.AppendLine("AND FTPosCode = '"+ cVB.tVB_PosCode+"'");
                oSql.AppendLine("AND FTShfCode = '"+ cVB.tVB_ShfCode +"'");
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
                new cSP().SP_CLExMemory();
            }
            return oData;
        }

        /// <summary>
        /// Check ยอดขาย ก่อนจะปิดรอบ
        /// 2020-02-18 Create By Zen
        /// </summary>
        /// <returns></returns>
        public int C_CHKnShiftofSale()
        {
            int nCheck = 0;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            DataTable odtTmp;

            oSql = new StringBuilder();
            oSql.AppendLine("SELECT HD.FTBchCode,HD.FTShfCode,HD.FDXshDocDate,HD.FCXshGrand,HD.FCXshRnd");          
            oSql.AppendLine("FROM TPSTSalHD HD WITH(NOLOCK)");
            oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTShfCode = '" + cVB.tVB_ShfCode + "' ");         
            odtTmp = new DataTable();
            odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
            if (odtTmp.Rows.Count > 0)
            {
                nCheck = 1;
            }
            return nCheck;
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
                oSP.SP_CLExMemory();
            }
        }

        private static void C_PRNxShiftRCV(object sender, PrintPageEventArgs e)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine, tDeposit;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tAmt;
            decimal cChange = 0;
            List<cmlShiftRcv> aoShiftRcv;
            Image oLogo;

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

                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg

                // Get ค่า ID เครื่อง จาก DB
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                nStartY += 18;

                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCS_ShiftRcv").ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18),oFormatCenter);
                nStartY += 10;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 18;
                oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tCS_Seq"), cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tCS_RcvType"), cVB.aoVB_PInvLayout[2], Brushes.Black, 30, nStartY);
                oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tCS_Amt"), cVB.aoVB_PInvLayout[2], Brushes.Black,new RectangleF(110,nStartY,nWidth - 110, 18),oFormatFar);
                
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
                oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
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
                oMsg = null;
                tLine = null;
                oSP.SP_CLExMemory();
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
                    oSP.SP_CLExMemory();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_PRCxPrintShiftBN : " + oEx.Message); }
        }

        private static void C_PRNxShiftBN(object sender, PrintPageEventArgs e)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
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

                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg

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

                nStartY += 10;
                oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tType") + "  (" + cVB.oVB_GBResource.GetString("tQty") + ")";
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth/2, 18));
                tMsg = cVB.oVB_GBResource.GetString("tCS_Amt");
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(nWidth / 2, nStartY, (nWidth / 2)-10, 18), oFormatFar);

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
                oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
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
                oMsg = null;
                tLine = null;
                oSP.SP_CLExMemory();
            }
        }

        public static void C_PRNxSaleSumRpt(Graphics poGraphic,bool pbDrawHF = true)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tLastRcv;
            decimal cLastAmt,cSumShift;
            Font oFont = new Font("CordiaUPC", Convert.ToSingle(11.5), FontStyle.Regular);
            string tMsg = "";
            List<cmlTPSTShiftSRatePdt> aoShiftSRatePdt;
            List<cmlTPSTShiftSLastDoc> aoShiftSLastDoc;
            List<cmlTPSTShiftSSumRcv> aoShiftSSumRcv;
            cmlShiftSum oSumShift;
            Image oLogo;

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

                oMsg = new cSlipMsg();
                if (pbDrawHF)
                {
                    nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg
                }

                tMsg = cVB.oVB_GBResource.GetString("tCS_ShiftSUM").ToString();
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                oFont = cVB.aoVB_PInvLayout[3];
                //----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //!!!ปิดการขาย!!!
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tSaleDate") + ":" + Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");
                tMsg += " " + cVB.oVB_GBResource.GetString("tPos") + ":" + cVB.tVB_PosCode;
                tMsg += " " + cVB.oVB_GBResource.GetString("tUsr") + ":" + cVB.tVB_UsrName;
                oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);

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

                //นำเงินออก
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tMnyOut");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nMnyOutCnt, cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cMnyOutAmt, cVB.nVB_DecShow);
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

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รวมเงิน
                nStartY += 18;
                tMsg = "    " + cVB.oVB_GBResource.GetString("tTotal") + ":";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                cSumShift = (oSumShift.cMnyInAmt + oSumShift.cSaleAmt) - (oSumShift.cMnyOutAmt + oSumShift.cRefundAmt);
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
                if (aoShiftSSumRcv.Count > 0)
                {
                    foreach (cmlTPSTShiftSSumRcv oShift in aoShiftSSumRcv)
                    {
                        if (string.Equals(oShift.FTRcvCode, tLastRcv))
                        {
                            if (string.Equals(oShift.FTRcvDocType, "9"))
                            {
                                tMsg = cVB.oVB_GBResource.GetString("tRefund") + " " + oShift.FTRcvName;
                            }
                            else
                            {
                                tMsg = cVB.oVB_GBResource.GetString("tSale") + " " + oShift.FTRcvName;
                            }
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

                            //-----------------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                        }
                        else
                        {
                            if (string.Equals(oShift.FTRcvDocType, "9"))
                            {
                                tMsg = cVB.oVB_GBResource.GetString("tRefund") + " " + oShift.FTRcvName;
                            }
                            else
                            {
                                tMsg = cVB.oVB_GBResource.GetString("tSale") + " " + oShift.FTRcvName;
                            }
                            nStartY += 18;
                            oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                            tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), cVB.nVB_DecShow);
                            oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                        }
                        tLastRcv = oShift.FTRcvCode;
                        cLastAmt = Convert.ToDecimal(oShift.FCRcvPayAmt);
                    }
                }

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

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

                //รายละเอียดสกุลเงิน
                aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
                aoShiftSRatePdt = C_GETaShiftSRate();
                if (aoShiftSRatePdt.Count > 0)
                {
                    foreach (cmlTPSTShiftSRatePdt oData in aoShiftSRatePdt)
                    {
                        nStartY += 18;
                        tMsg = oData.FTSrpNameRef;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                        tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpAmt),cVB.nVB_DecShow);
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                    }
                }

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

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

                //รายการสินค้าควบคุม
                aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
                aoShiftSRatePdt = C_GETaShiftSPdt();
                if (aoShiftSRatePdt.Count > 0)
                {
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
                }

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                ////รอบ/ลำดับ
                //nStartY += 18;
                //tMsg = cVB.oVB_GBResource.GetString("tItem");
                //oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));

                ////-----------------
                //nStartY += 10;
                //oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

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

                //Print Date,Time
                nStartY += 18;
                tMsg = cVB.oVB_GBResource.GetString("tPrintDate") + ":" + DateTime.Now.ToString("dd/MM/yyyy");
                tMsg += " " + cVB.oVB_GBResource.GetString("tTime") + ":" + DateTime.Now.ToString("HH:mm:ss");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                //ท้ายใบเสร็จ
                if (pbDrawHF)
                {
                    nStartY += 30;
                    nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                }

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
                oMsg = null;
                tLine = null;
                oSP.SP_CLExMemory();
            }
        }
        #endregion Print
    }
}
