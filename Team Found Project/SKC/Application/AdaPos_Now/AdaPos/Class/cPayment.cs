using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.RabbitMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cPayment
    {
        #region Variable
        public static string tC_RcvCode;
        public static string tC_RcvName;
        public static string tC_FmtCode;
        public static string tC_CrdNo;
        public static string tC_BnkCode;
        public static string tC_BnkName;
        public static string tC_CrdTrans;
        public static string tC_CrdApvCode;
        public static string tC_TblTopUpRC;
        public static string tC_AliTransID;
        public static string tC_AliPaymentCode;
        public static string tC_XrcRef1;           //ข้อมูลอ้างอิง 1
        public static string tC_XrcRef2;           //ข้อมูลอ้างอิง 2
        public static string tC_ChequeNo;          //*Arm 62-10-11  หมายเลข Cheque
        public static string tC_CrdCode;          //*Net 63-04-01 ยกมาจาก baseline
        public static DateTime dC_ChequeDate;      //*Arm 62-10-11  วันที่ Cheque 
        public static int nC_SeqRC;

        public static bool bC_StaAlwRet;
        public static bool bC_StaAlwCancel;
        public static bool bC_StaPayLast;
        public static List<cmlEDC> aoC_EDC;
        public static cmlEDC oC_EDCSel;

        #endregion End Variable

        public cPayment()
        {

        }

        /// <summary>
        /// Get EDC
        /// </summary>
        /// <returns></returns>
        public List<cmlEDC> C_GETaEdc()
        {
            List<cmlEDC> aoPosHW = new List<cmlEDC>();
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT EDC.FTEdcShwFont,EDC.FTEdcShwBkg,ISNULL(EDCL.FTEdcName,(SELECT TOP 1 FTEdcName FROM TFNMEdc_L with(nolock) WHERE FTEdcCode = EDC.FTEdcCode)) AS FTEdcName");
                oSql.AppendLine(",SED.FTSedModel,SED.FNSedAck,FNSedTimeOut,EDC.FTEdcCode,PHW.FTPhwConnType,PHW.FTPhwConnRef");
                oSql.AppendLine("FROM TCNMPosHW PHW with(nolock)");
                oSql.AppendLine("INNER JOIN TSysPosHW SHW with(nolock) ON PHW.FTShwCode = SHW.FTShwCode AND FTShwHWKey = 'pEDC'");
                oSql.AppendLine("INNER JOIN TFNMEdc EDC with(nolock) ON PHW.FTPhwCodeRef = EDC.FTEdcCode");
                oSql.AppendLine("LEFT JOIN TFNMEdc_L EDCL with(nolock) ON EDC.FTEdcCode = EDCL.FTEdcCode AND EDCL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TSysEdc SED with(nolock) ON EDC.FTSedCode = SED.FTSedCode");
                oSql.AppendLine("WHERE PHW.FTPosCode = '" + cVB.tVB_PosCode + "'");
                oSql.AppendLine("ORDER BY PHW.FTPhwCode");

                aoPosHW = new cDatabase().C_GETaDataQuery<cmlEDC>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_GETaEdc : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return aoPosHW;
        }

        public static void C_PRCxPaymentCash()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            decimal cAmtLeft = 0, cAmt = 0, cChange = 0, cAmtDep = 0;
            decimal cNet = 0, cAmtFLeft = 0;
            string tMsg = "";
            cSP oSP = new cSP();
            try
            {
                if (cVB.oVB_Payment.W_CHKbVerify2Payment() == false) return;

                nC_SeqRC = nC_SeqRC + 1;
                cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw;
                cAmtFLeft = cAmtLeft;
                cAmt = cVB.cVB_Amount;
                if (cAmt >= cAmtLeft)
                {
                    cChange = cAmt - cAmtLeft;
                    cNet = cAmtLeft;
                    cAmtLeft = 0;

                    //*Em 62-10-10
                    if ((decimal)cChange > (decimal)cVB.cVB_MaxChg)
                    {
                        tMsg = cVB.oVB_GBResource.GetString("tMsgChgOver");
                        tMsg = string.Format(tMsg, oSP.SP_SETtDecShwSve(1, cVB.cVB_MaxChg, cVB.nVB_DecShow));
                        oSP.SP_SHWxMsg(tMsg, 3);
                        return;
                    }
                    //+++++++++++++++
                }
                else
                {
                    cNet = cAmt;
                    cChange = 0;
                    cAmtLeft = cAmtLeft - cNet;
                }

                switch (cVB.tVB_PosType)
                {
                    case "1":
                        oSql = new StringBuilder();
                        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRC + " (FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, ");
                        oSql.AppendLine("FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                        oSql.AppendLine("FCXrcDep, FCXrcNet, FCXrcChg, ");
                        oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                        oSql.AppendLine("VALUES('" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + ",'" + tC_RcvCode + "','" + tC_RcvName + "',");
                        oSql.AppendLine("'" + cVB.tVB_RateCode + "'," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cAmt + ",");
                        oSql.AppendLine(cAmtDep + "," + cNet + "," + cChange + ",");
                        oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                        //*Net 63-07-30 ปรับจาก Moshi
                        //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02 เช็ค insert RC
                        string tErr = "";
                        oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                        if (!String.IsNullOrEmpty(tErr))
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                            Environment.Exit(1);
                        }
                        //++++++++++++++++++++++++

                        if (cAmtLeft == 0)
                        {
                            cVB.cVB_Change = cChange;
                            //cSale.C_PRCxSetComplete();
                            cSale.bC_SetComplete = true;    //*Em 63-04-22
                        }

                        cVB.oVB_Payment.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                        //*Net 63-07-30 ปรับจอ 2 ใหม่
                        //if (cVB.oVB_2ndScreen != null)
                        //{
                        //    cVB.oVB_2ndScreen.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                        //}
                        break;
                    case "2":
                        oSql = new StringBuilder();
                        oSql.AppendLine("INSERT INTO " + cPayment.tC_TblTopUpRC + "");
                        oSql.AppendLine("(");
                        oSql.AppendLine("  FTBchCode,FTCthDocNo,FNXrcSeqNo");
                        oSql.AppendLine(" ,FTRcvCode,FTRcvName,FTRteCode");
                        oSql.AppendLine(" ,FCXrcRteFac,FCXrcFrmLeftAmt,FCXrcUsrPayAmt");
                        oSql.AppendLine(" ,FCXrcNet,FCXrcChg");
                        oSql.AppendLine(") VALUES (");
                        oSql.AppendLine(" '" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + "");
                        oSql.AppendLine(" ,'" + tC_RcvCode + "','" + tC_RcvName + "','" + cVB.tVB_RateCode + "'");
                        oSql.AppendLine(" ," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cAmt + "");
                        oSql.AppendLine(" ," + cNet + "," + cChange + "");
                        oSql.AppendLine(")");
                        oDB.C_SETxDataQuery(oSql.ToString());
                        // cVB.cVB_Amount = cAmtFLeft - cAmt;
                        cVB.oVB_Payment.cW_AmtTotalShw = cAmtFLeft - cAmt;
                        cVB.cVB_Change = cChange;
                        break;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_PRCxPaymentCash : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                oSP = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxPaymentConfirm()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            decimal cAmtLeft = 0, cAmt = 0, cChange = 0, cAmtDep = 0;
            decimal cNet = 0, cAmtFLeft = 0;
            try
            {
                nC_SeqRC = nC_SeqRC + 1;
                //cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw;
                cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw - cVB.cVB_RoundDiff; //*Net 63-04-01 ยกมาจาก baseline
                cAmtFLeft = cAmtLeft;
                cAmt = cVB.cVB_Amount;
                if (cAmt >= cAmtLeft)
                {
                    cChange = 0;
                    cNet = cAmtLeft;
                    cAmtLeft = 0;
                    cVB.cVB_RoundDiff = 0; //*Net 63-04-01 ยกมาจาก baseline
                }
                else
                {
                    cNet = cAmt;
                    cChange = 0;
                    cAmtLeft = cAmtLeft - cNet;
                }

                oSql = new StringBuilder();
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRC + " (FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, ");
                oSql.AppendLine("FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                oSql.AppendLine("FCXrcDep, FCXrcNet, FCXrcChg, ");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("VALUES('" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + ",'" + tC_RcvCode + "','" + tC_RcvName + "',");
                oSql.AppendLine("'" + cVB.tVB_RateCode + "'," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cNet + ",");
                oSql.AppendLine(cAmtDep + "," + cNet + "," + cChange + ",");
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                //*Net 63-07-30 ปรับจาก Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02 เช็ค insert RC
                string tErr = "";
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }
                //+++++++++++++++++++++++++++++++

                cVB.oVB_Payment.W_DATxAddPay2Grid(tC_RcvName, cNet);
                //*Net 63-07-30 ปรับจาก Moshi
                //if (cVB.oVB_2ndScreen != null)
                //{
                //    cVB.oVB_2ndScreen.W_DATxAddPay2Grid(tC_RcvName, cNet);
                //}
                //
                if (cAmtLeft == 0)
                {
                    cVB.cVB_Change = cChange;
                    //cSale.C_PRCxSetComplete();
                    cSale.bC_SetComplete = true;    //*Em 63-04-22
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_PRCxPaymentCash : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxPaymentCreditCard()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            decimal cAmtLeft = 0, cAmt = 0, cChange = 0, cAmtDep = 0;
            decimal cNet = 0, cAmtFLeft = 0;
            try
            {

                nC_SeqRC = nC_SeqRC + 1;
                //cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw;
                //cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw - cVB.cVB_RoundDiff; //*Net 63-04-01 ยกมาจาก baseline //*Arm 63-09-01 Comment Code
                cAmtLeft = Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, (cVB.oVB_Payment.cW_AmtTotalShw - cVB.cVB_RoundDiff), 2)); //*Arm 63-09-01 ปัดเศษทศนิยม 2 ตำแหน่ง

                cAmtFLeft = cAmtLeft;
                //cVB.cVB_Amount = cAmtLeft;
                //cAmt = cVB.cVB_Amount; //*Arm 63-09-01 Comment Code
                cAmt = Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, cVB.cVB_Amount, 2)); //*Arm 63-09-01 ปัดเศษทศนิยม 2 ตำแหน่ง
                if (cAmt >= cAmtLeft)
                {
                    //cVB.cVB_Amount = cAmtLeft; //*Arm 63-09-02 Comment Code
                    cAmt = cAmtLeft;
                    cChange = cAmt - cAmtLeft;
                    cNet = cAmtLeft;
                    cAmtLeft = 0;
                    //cVB.cVB_RoundDiff = 0; //*Net 63-04-01 ยกมาจาก baseline
                    cVB.cVB_RoundDiff = cNet - cVB.cVB_Amount; //*Arm 63-09-02
                }
                else
                {
                    cNet = cAmt;
                    cChange = 0;
                    cAmtLeft = cAmtLeft - cNet;
                }

                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRC + " (FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, ");
                oSql.AppendLine("FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                oSql.AppendLine("FCXrcDep, FCXrcNet, FCXrcChg, ");
                oSql.AppendLine("FTXrcRefNo1,FTXrcRefNo2,FTBnkCode,FDXrcRefDate,");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("VALUES('" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + ",'" + tC_RcvCode + "','" + tC_RcvName + "',");
                oSql.AppendLine("'" + cVB.tVB_RateCode + "'," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cAmt + ",");
                oSql.AppendLine(cAmtDep + "," + cNet + "," + cChange + ",");
                //oSql.AppendLine("'" + tC_CrdNo + "','" + tC_CrdTrans + ";" + tC_CrdApvCode + "','" + tC_BnkCode + "',CONVERT(VARCHAR(10),GETDATE(),121),");
                oSql.AppendLine("'" + tC_CrdNo + "','" + tC_CrdTrans + ";" + tC_CrdApvCode + ";" + tC_CrdCode + "','" + tC_BnkCode + "',CONVERT(VARCHAR(10),GETDATE(),121),"); //*Net 63-04-01 ยกมาจาก baseline
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                //Net 63-07-30 ปรับจาก Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02 เช็ค insert RC
                string tErr = "";
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }
                //++++++++++++++++++++++++++

                cVB.oVB_Payment.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //*Net 63-07-30 ปรับจาก Moshi
                //if (cVB.oVB_2ndScreen != null)
                //{
                //    cVB.oVB_2ndScreen.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //}
                //
                if (cAmtLeft == 0)
                {
                    cVB.cVB_Change = cChange;
                    //cVB.cVB_RoundDiff = 0;    //*Arm 63-09-02 -comment Code ต้องเก็บค่า Round
                    //cSale.C_PRCxSetComplete();
                    cSale.bC_SetComplete = true;    //*Em 63-04-22
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_PRCxPaymentCreditCard : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxPaymentAlipay()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            decimal cAmtLeft = 0, cAmt = 0, cChange = 0, cAmtDep = 0;
            decimal cNet = 0, cAmtFLeft = 0;
            try
            {

                nC_SeqRC = nC_SeqRC + 1;
                //cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw - cVB.cVB_RoundDiff;
                cAmtLeft = Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, (cVB.oVB_Payment.cW_AmtTotalShw - cVB.cVB_RoundDiff), 2)); //*Arm 63-09-02
                cAmtFLeft = cAmtLeft;
                //cAmt = cVB.cVB_Amount;
                cAmt = Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, cVB.cVB_Amount,2)); //*Arm 63-09-02
                if (cAmt >= cAmtLeft)
                {
                    cChange = cAmt - cAmtLeft;
                    cNet = cAmtLeft;
                    cAmtLeft = 0;
                    //cVB.cVB_RoundDiff = 0;
                    cVB.cVB_RoundDiff = cNet - cVB.cVB_Amount; //*Arm 63-09-02
                }
                else
                {
                    cNet = cAmt;
                    cChange = 0;
                    cAmtLeft = cAmtLeft - cNet;
                }

                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRC + " (FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, ");
                oSql.AppendLine("FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                oSql.AppendLine("FCXrcDep, FCXrcNet, FCXrcChg, ");
                oSql.AppendLine("FTXrcRefNo1,FTXrcRefNo2,FDXrcRefDate,");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("VALUES('" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + ",'" + tC_RcvCode + "','" + tC_RcvName + "',");
                oSql.AppendLine("'" + cVB.tVB_RateCode + "'," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cAmt + ",");
                oSql.AppendLine(cAmtDep + "," + cNet + "," + cChange + ",");
                oSql.AppendLine("'" + tC_AliTransID + "','" + tC_AliPaymentCode + "',CONVERT(VARCHAR(10),GETDATE(),121),");
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                //Net 63-07-30 ปรับจาก Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02 เช็ค insert RC
                string tErr = "";
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }
                //+++++++++++++++++++++++++++++++++++

                cVB.oVB_Payment.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //*Net 63-07-30 ปรับจาก Moshi 
                //if (cVB.oVB_2ndScreen != null)
                //{
                //    cVB.oVB_2ndScreen.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //}
                ////
                if (cAmtLeft == 0)
                {
                    cVB.cVB_Change = cChange;
                    //cVB.cVB_RoundDiff = 0; //*Arm 63-09-02 -comment Code ต้องเก็บค่า Round
                    //cSale.C_PRCxSetComplete();
                    cSale.bC_SetComplete = true;    //*Em 63-04-22
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_PRCxPaymentCreditCard : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxPaymentPromptPay()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            decimal cAmtLeft = 0, cAmt = 0, cChange = 0, cAmtDep = 0;
            decimal cNet = 0, cAmtFLeft = 0;
            try
            {

                nC_SeqRC = nC_SeqRC + 1;
                //cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw;
                //cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw - cVB.cVB_RoundDiff; //*Net 63-04-01 ยกมาจาก baseline
                cAmtLeft = Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, (cVB.oVB_Payment.cW_AmtTotalShw - cVB.cVB_RoundDiff), 2)); //*Arm 63-09-01

                cAmtFLeft = cAmtLeft;

                //cAmt = cVB.cVB_Amount;
                cAmt = Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, cVB.cVB_Amount, 2)); //*Arm 63-09-01

                if (cAmt >= cAmtLeft)
                {
                    cChange = cAmt - cAmtLeft;
                    cNet = cAmtLeft;
                    cAmtLeft = 0;
                    //cVB.cVB_RoundDiff = 0; //*Net 63-04-01 ยกมาจาก baseline
                    cVB.cVB_RoundDiff = cNet - cVB.cVB_Amount; //*Arm 63-09-02
                }
                else
                {
                    cNet = cAmt;
                    cChange = 0;
                    cAmtLeft = cAmtLeft - cNet;
                }

                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRC + " (FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, ");
                oSql.AppendLine("FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                oSql.AppendLine("FCXrcDep, FCXrcNet, FCXrcChg, ");
                oSql.AppendLine("FTXrcRefNo1,FTXrcRefNo2,FDXrcRefDate,");
                oSql.AppendLine("FTBnkCode,");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("VALUES('" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + ",'" + tC_RcvCode + "','" + tC_RcvName + "',");
                oSql.AppendLine("'" + cVB.tVB_RateCode + "'," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cAmt + ",");
                oSql.AppendLine(cAmtDep + "," + cNet + "," + cChange + ",");
                oSql.AppendLine("'" + tC_XrcRef1 + "','" + tC_XrcRef2 + "',CONVERT(VARCHAR(10),GETDATE(),121),");
                oSql.AppendLine("'" + tC_BnkCode + "',");
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                //*Net 63-07-30 ปรับตาม Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02 เช็ค insert RC
                string tErr = "";
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }
                //++++++++++++++++++++++++++++++

                cVB.oVB_Payment.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //*Net 63-07-30 ปรับตาม Moshi
                //if (cVB.oVB_2ndScreen != null)
                //{
                //    cVB.oVB_2ndScreen.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //}

                if (cAmtLeft == 0)
                {
                    cVB.cVB_Change = cChange;
                    //cVB.cVB_RoundDiff = 0; //*Arm 63-09-02 -comment Code ต้องเก็บค่า Round
                    //cSale.C_PRCxSetComplete();
                    cSale.bC_SetComplete = true;    //*Em 63-04-22
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_PRCxPaymentCreditCard : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// การชำระด้วยคูปอง
        /// </summary>
        /// <param name="pnMode">1:คูปองเงินสด 2:คูปองส่วนลด</param>
        public static void C_PRCxPaymentCoupon(int pnMode)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            decimal cAmtLeft = 0, cAmt = 0, cChange = 0, cAmtDep = 0;
            decimal cNet = 0, cAmtFLeft = 0;
            try
            {
                //if (pnMode == 2)
                //{
                //    if (!C_CHKbDiscountCoupon())
                //    {
                //        return;
                //    }
                //}

                nC_SeqRC = nC_SeqRC + 1;
                cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw;
                cAmtFLeft = cAmtLeft;
                cAmt = cVB.cVB_Amount;

                if (pnMode == 1)
                {
                    if (cAmt >= cAmtLeft)
                    {
                        cChange = 0;
                        cNet = cAmtLeft;
                        cAmtLeft = 0;
                    }
                    else
                    {
                        cNet = cAmt;
                        cChange = 0;
                        cAmtLeft = cAmtLeft - cNet;
                    }
                }
                else
                {
                    cNet = cAmt;
                    cAmtFLeft = cAmtFLeft + cAmt;
                }


                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRC + " (FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, ");
                oSql.AppendLine("FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                oSql.AppendLine("FCXrcDep, FCXrcNet, FCXrcChg, ");
                oSql.AppendLine("FTXrcRefNo1,FTXrcRefNo2,FDXrcRefDate,");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("VALUES('" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + ",'" + tC_RcvCode + "','" + tC_RcvName + "',");
                oSql.AppendLine("'" + cVB.tVB_RateCode + "'," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cAmt + ",");
                oSql.AppendLine(cAmtDep + "," + cNet + "," + cChange + ",");
                oSql.AppendLine("'" + tC_XrcRef1 + "','" + tC_XrcRef2 + "',CONVERT(VARCHAR(10),GETDATE(),121),");
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                //*Net 63-07-30 ปรับตาม Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02 เช็ค insert RC
                string tErr = "";
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }
                //+++++++++++++++++++++++++++++

                switch (pnMode)
                {
                    case 1: //คูปองเงินสด
                        cVB.oVB_Payment.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                        //*Net 63-07-30 ปรับตาม Moshi
                        //if (cVB.oVB_2ndScreen != null)
                        //{
                        //    cVB.oVB_2ndScreen.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                        //}

                        break;
                    case 2: //คูปองส่วนลด
                        break;
                }

                if (cAmtLeft == 0)
                {
                    cVB.cVB_Change = cChange;
                    cVB.cVB_RoundDiff = 0;
                    //cSale.C_PRCxSetComplete();
                    cSale.bC_SetComplete = true;    //*Em 63-04-22
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_PRCxPaymentCreditCard : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// การชำระด้วยเช็ค
        /// </summary>
        public static void C_PRCxPaymentCheque()    //*Arm 62-10-11
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            decimal cAmtLeft = 0, cAmt = 0, cChange = 0, cAmtDep = 0;
            decimal cNet = 0, cAmtFLeft = 0;
            try
            {

                nC_SeqRC = nC_SeqRC + 1;
                cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw;
                cAmtFLeft = cAmtLeft;
                cAmt = cVB.cVB_Amount;
                if (cAmt >= cAmtLeft)
                {
                    //cChange = cAmt - cAmtLeft;
                    cChange = 0; // *Arm 62-10-16  - เงินทอน = 0
                    cNet = cAmtLeft;
                    cAmtLeft = 0;
                }
                else
                {
                    cNet = cAmt;
                    cChange = 0;
                    cAmtLeft = cAmtLeft - cNet;
                }

                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRC + " (FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, ");
                oSql.AppendLine("FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                oSql.AppendLine("FCXrcDep, FCXrcNet, FCXrcChg, ");
                oSql.AppendLine("FTXrcRefNo1,FTXrcRefNo2,FDXrcRefDate,");
                oSql.AppendLine("FTBnkCode,");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("VALUES('" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + ",'" + tC_RcvCode + "','" + tC_RcvName + "',");
                oSql.AppendLine("'" + cVB.tVB_RateCode + "'," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cAmt + ",");
                oSql.AppendLine(cAmtDep + "," + cNet + "," + cChange + ",");
                //oSql.AppendLine("'" + tC_ChequeNo + "','" + tC_XrcRef2 + "','" + dC_ChequeDate + "',");
                oSql.AppendLine("'" + tC_ChequeNo + "','" + tC_XrcRef2 + "','" + string.Format("{0:yyyy-MM-dd}", dC_ChequeDate) + "',"); // *Arm 62-10-16  -แก้ไข formart Date
                oSql.AppendLine("'" + tC_BnkCode + "',");
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                //*Net 63-07-30 ปรับตาม Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02 เช็ค insert RC
                string tErr = "";
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }
                //+++++++++++++++++++++++++++++++++++++

                cVB.oVB_Payment.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //*Net 63-07-30 ปรับตาม Moshi
                //if (cVB.oVB_2ndScreen != null)
                //{
                //    cVB.oVB_2ndScreen.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //}

                if (cAmtLeft == 0)
                {
                    cVB.cVB_Change = cChange;
                    cVB.cVB_RoundDiff = 0;
                    //cSale.C_PRCxSetComplete();
                    cSale.bC_SetComplete = true;    //*Em 63-04-22
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_PRCxPaymentCheque : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }


        /// <summary>
        /// Dis Chg ท้ายบิล
        /// </summary>
        /// <param name="pcAmt">คำนวณจาก Dis Chg เป็นมูลค่า</param>
        /// <param name="pcDisChg">ยอด Dis Chg</param>
        /// <param name="ptDisChgTxt">Dis Chg Txt</param>
        /// <param name="pcB4DisChg">B4DisChg</param>
        /// <param name="nW_DisType">Dis Type 1 ลดแบบจำนวน,2 ลดแบบเปอร์เซ็น,3 ชาร์ตแบบจำนวน,4 ชาร์ตแบบเปอร์เซ็น ,5 คูปองส่วนลด,6 คูปองแลกซื้อ</param>
        /// <param name="ptRefCode">*Arm 63-03-12 เลขที่อ้างอิง Redeem</param>
        /// <param name="paoProDT">*Net List SeqNo รายการที่จะ Prorate ลง DTDis ถ้าเป็น null จะ Prorate ลงทุกตัว</param>
        /// <param name="pnStaDis">*Arm 63-06-11 2:ส่วนลดท้ายบิล, 3:ส่วนลดท้ายบิลตามรายการ</param>
        public static void C_ADDxDisChgBill(decimal pcAmt, decimal pcDisChg, string ptDisChgTxt, decimal pcB4DisChg, int pnDisType, string ptRefCode = "", List<cmlProrateDT> paoProDT = null, int pnStaDis = 2, string ptRsnCode ="")
        {
            StringBuilder oSql = new StringBuilder();
            decimal cTotalAfDisChg, cDisChg;
            string tDisChgTxt, tDisChgTxtAll;
            decimal cFCXshDis;
            decimal cDisV;
            decimal cDisNV;
            decimal cFCXshChg;
            decimal cChgV;
            decimal cChgNV;
            decimal cB4V;
            decimal cB4NV;
            cDatabase oDB = new cDatabase();
            List<cmlTPSTSalHDDis> aoTPSTSalHDDis = new List<cmlTPSTSalHDDis>();
            try
            {
                // คำนวณ TotalAfDisChg
                //cTotalAfDisChg = pcB4DisChg - pcAmt;
                //*Em 63-01-08
                cTotalAfDisChg = pcB4DisChg;
                switch (pnDisType)
                {
                    case 1:
                    case 2:
                    case 5:
                    case 6:
                        cTotalAfDisChg = pcB4DisChg - pcAmt;
                        break;
                    case 3:
                    case 4:
                        cTotalAfDisChg = pcB4DisChg + pcAmt;
                        break;
                }
                //+++++++++++++

                // Assign DisChgTxt
                tDisChgTxt = ptDisChgTxt;

                // Replace + %  หายอด Dis Chg เท่านั้น
                ptDisChgTxt = ptDisChgTxt.Replace("+", "");
                ptDisChgTxt = ptDisChgTxt.Replace("%", "");
                cDisChg = Convert.ToDecimal(ptDisChgTxt);

                // INSERT TPSTSalHDDis
                oSql.Clear();
                oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalHDDis + " ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("(");
                oSql.AppendLine("  FTBchCode,FTXshDocNo,FDXhdDateIns");
                oSql.AppendLine(" ,FTXhdDisChgTxt,FTXhdDisChgType,FCXhdTotalAfDisChg");
                oSql.AppendLine(" ,FCXhdDisChg,FCXhdAmt");
                oSql.AppendLine(" ,FTXhdRefCode,FTDisCode,FTRsnCode");      //*Arm 63-03-12 เลขที่อ้างอิง Redeem //*Arm 63-06-30 เพิ่ม FTDisCode,FTRsnCode 
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES ");
                oSql.AppendLine("(");
                oSql.AppendLine("  '" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "',GETDATE()");
                oSql.AppendLine(" ,'" + tDisChgTxt + "'," + pnDisType + ",'" + cTotalAfDisChg + "'");
                oSql.AppendLine(" ,'" + cDisChg + "','" + pcAmt + "'");
                oSql.AppendLine(" ,'" + ptRefCode + "','"+cVB.tVB_DisCode+"', '"+ ptRsnCode + "'");       //*Arm 63-03-12 เลขที่อ้างอิง Redeem //*Arm 63-06-30 เพิ่ม FTDisCode,FTRsnCode //*Arm 63-07-13 FTDisCode = tVB_DisCode
                oSql.AppendLine(")");
                new cDatabase().C_SETxDataQuery(oSql.ToString());

                // Get FCXshDis
                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(SUM(FCXhdAmt),0) FROM " + cSale.tC_TblSalHDDis + " WITH(NOLOCK) WHERE FTXhdDisChgType IN ('1','2','5','6') AND FTXshDocNo='" + cVB.tVB_DocNo + "'"); //*Net 63-03-19 Add5,6
                cFCXshDis = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                // Get FCXshChg
                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(SUM(FCXhdAmt),0) FROM " + cSale.tC_TblSalHDDis + " WITH(NOLOCK) WHERE FTXhdDisChgType IN ('3','4') AND FTXshDocNo='" + cVB.tVB_DocNo + "'");
                cFCXshChg = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                // Get B4V
                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(FCXshTotalB4DisChgV,0) FROM " + cSale.tC_TblSalHD + " WITH(NOLOCK) WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");
                cB4V = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                pcB4DisChg = Convert.ToDecimal(cSale.oC_SalHD.FCXshTotalB4DisChgNV + cSale.oC_SalHD.FCXshTotalB4DisChgV);   //*Em 63-01-14

                // Get B4NV
                cB4NV = pcB4DisChg - cB4V;

                // Get DisV
                cDisV = (cB4V / pcB4DisChg) * cFCXshDis;

                // Get DisNV
                cDisNV = cFCXshDis - cDisV;

                // Get ChgV
                cChgV = (cB4V / pcB4DisChg) * cFCXshChg;

                // Get DisNV
                cChgNV = cFCXshChg - cChgV;

                // Get Dis Chg Txt
                oSql.Clear();
                oSql.AppendLine("SELECT FTXhdDisChgTxt FROM " + cSale.tC_TblSalHDDis + " WITH(NOLOCK) WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");

                // Get DisChgTxt HDDis
                aoTPSTSalHDDis = oDB.C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());
                tDisChgTxtAll = "";
                for (int nRow = 0; nRow < aoTPSTSalHDDis.Count; nRow++)
                {

                    if (tDisChgTxtAll == "")
                    {
                        tDisChgTxtAll += aoTPSTSalHDDis[nRow].FTXhdDisChgTxt;
                    }
                    else
                    {
                        tDisChgTxtAll += "," + aoTPSTSalHDDis[nRow].FTXhdDisChgTxt;
                    }
                }

                // UPDATE TPSTSalHD Dis Chg FCXshTotalAfDisChgV FCXshTotalAfDisChgNV
                oSql.Clear();
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK) SET ");
                oSql.AppendLine("  FCXshDis=ROUND(" + cFCXshDis + "," + cVB.nVB_DecSave + ")");
                oSql.AppendLine(" ,FCXshChg=ROUND(" + cFCXshChg + "," + cVB.nVB_DecSave + ")");
                // oSql.AppendLine(" ,FCXshTotalAfDisChgV=FCXshTotalB4DisChgV - ((FCXshTotalB4DisChgV * (" + cFCXshDis + "-" + cFCXshChg + ")) / " + pcB4DisChg + ")");
                // oSql.AppendLine(" ,FCXshTotalAfDisChgNV=FCXshTotalB4DisChgNV - ((FCXshTotalB4DisChgNV * (" + cFCXshDis + "-" + cFCXshChg + ")) / " + pcB4DisChg + ")");
                oSql.AppendLine(" ,FCXshTotalAfDisChgV=ROUND(" + cB4V + " - " + cDisV + " + " + cChgV + "," + cVB.nVB_DecSave + ")");
                oSql.AppendLine(" ,FCXshTotalAfDisChgNV=ROUND(" + cB4NV + " - " + cDisNV + " + " + cChgNV + "," + cVB.nVB_DecSave + ")");
                oSql.AppendLine(" ,FTXshDisChgTxt='" + tDisChgTxtAll + "'");
                oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");
                new cDatabase().C_SETxDataQuery(oSql.ToString());

                if (cSale.nC_DocType == 9) return;  //*Em 63-05-08
                if (cVB.oVB_ReferSO != null)
                {
                    if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1") return; //*Arm 63-05-11
                }

                // Prorate DT
                //C_PRCxDisChgProrateDT(pcAmt, tDisChgTxt, pnDisType);

                //Arm 63-03-12
                if (string.IsNullOrEmpty(ptRefCode))
                {
                    //C_PRCxDisChgProrateDT(pcAmt, tDisChgTxt, pnDisType);

                    if (paoProDT != null && paoProDT.Count > 0) //*Arm 63-06-11 ถ้า paoProDT มีรายการ = มีการกำหนดให้ ทำ Prorate DT บางรายการ, ไม่มีรายการ : Prorate DT ทุกรายการที่อนุญาตลด
                    {
                        C_PRCxDisChgProrateDT(pcAmt, tDisChgTxt, pnDisType, ptRefCode, paoProDT, pnStaDis, ptRsnCode);
                    }
                    else
                    {
                        C_PRCxDisChgProrateDT(pcAmt, tDisChgTxt, pnDisType,"",null, pnStaDis, ptRsnCode);
                    }
                }
                else
                {
                    // ใช้แต้มแลกส่วนลด
                    C_PRCxDisChgProrateDT(pcAmt, tDisChgTxt, pnDisType, ptRefCode, paoProDT, pnStaDis, ptRsnCode);
                }
                //++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPayment", "C_PRCxDisChgProrateDT : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                ptRefCode = "";
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Prorate DT
        /// </summary>
        /// <param name="pcAmt">มูลค่า</param>
        /// <param name="ptDisChgTxt">Dis Chg Text</param>
        /// <param name="nW_DisType">Dis Type 1 ลดแบบจำนวน,2 ลดแบบเปอร์เซ็น,3 ชาร์ตแบบจำนวน,4 ชาร์ตแบบเปอร์เซ็น <</param>
        /// <param name="ptRefCode">*Arm 63-03-12 เลขที่อ้างอิง Redeem</param>
        /// <param name="paoProDT"></param>
        /// <param name="pnStaDis">*Arm 63-06-08 2:ส่วนลดท้ายบิล, 3:ส่วนลดท้ายบิลตามรายการ</param>
        private static void C_PRCxDisChgProrateDT(decimal pcAmt, string ptDisChgTxt, int pnDisType, string ptRefCode = "", List<cmlProrateDT> paoProDT = null, int pnStaDis = 2, string ptRsnCode="")
        {
            List<cmlTPSTSalDT> aoSalDT;
            StringBuilder oSql;
            cDatabase oDB;
            decimal cDisChg, cAmtDisChg, cSumDTNet, cLastRowDisChg;
            string tSign = "";
            try
            {
                aoSalDT = new List<cmlTPSTSalDT>();
                oSql = new StringBuilder();
                oDB = new cDatabase();

                //*Arm 63-07-17 GET ข้อมมูลสินค้าที่อนุญาตลด (Dicount Policy)
                //***************************************
                oSql.Clear();
                oSql.AppendLine("SELECT FNXsdSeqNo, ");

                if(cVB.tVB_StaPrice == "1")
                {
                    oSql.AppendLine("CAST(ISNULL(FCXsdSetPrice * FCXsdQty,0) AS decimal(18, 4)) AS FCXsdNetAfHD");
                }
                else
                {
                    oSql.AppendLine("ISNULL(FCXsdNetAfHD,0) AS FCXsdNetAfHD");
                }

                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "' AND FCXsdNetAfHD>0 AND FTXsdStaAlwDis='1'");

                // Prorate เฉพาะสินค้า
                if (paoProDT != null) 
                {
                    if (paoProDT.Count > 0)
                    {
                        oSql.AppendLine($"AND FNXsdSeqNo IN ({String.Join(",", paoProDT.Select(DT => DT.FNSeqNo.ToString()).ToArray())}) ");
                    }

                }

                //Check การทำ DisChgProrateDT บิลคืน
                if (cSale.nC_DocType == 9)
                {
                    oSql.AppendLine("AND FTXsdStaPdt = '2'");
                }
                else
                {
                    oSql.AppendLine("AND FTXsdStaPdt = '1'");
                }

                oSql.AppendLine("ORDER BY FNXsdSeqNo ASC");
                //***************************************
                //*Arm 63-07-17 End  GET ข้อมมูลสินค้าที่อนุญาตลด

                //*Arm 63-07-17 Comment Code
                //// Get ข้อมูล sale dt สินค้าที่อนุญาติลด
                //oSql.Clear();
                //oSql.AppendLine("SELECT * FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "' AND FCXsdNetAfHD>0 AND FTXsdStaAlwDis='1'");
                //if (paoProDT != null) //*Net 63-03-21 Prorate เฉพาะสินค้า
                //{
                //    if (paoProDT.Count > 0)
                //    {
                //        oSql.AppendLine($"AND FNXsdSeqNo IN ({String.Join(",", paoProDT.Select(DT => DT.FNSeqNo.ToString()).ToArray())}) ");
                //    }

                //}
                ////*Arm 63-04-04 Check การทำ DisChgProrateDT บิลคืน
                //if (cSale.nC_DocType == 9)
                //{
                //    oSql.AppendLine("AND FTXsdStaPdt = '2'");
                //}
                //else
                //{
                //    oSql.AppendLine("AND FTXsdStaPdt = '1'");
                //}
                ////oSql.AppendLine("AND FTXsdStaPdt = '1'");
                //oSql.AppendLine("ORDER BY FNXsdSeqNo ASC");
                //*Arm 63-07-17 End Comment Code

                aoSalDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());

                var oSum = aoSalDT.Sum(o => o.FCXsdNetAfHD); //*Net 63-03-21
                cSumDTNet = Convert.ToDecimal(oSum);

                if (aoSalDT.Count > 0)
                {

                    switch (pnDisType)
                    {
                        case 1:
                        case 2:
                        case 5:
                        case 6:
                            tSign = "-";
                            break;
                        case 3:
                        case 4:
                            tSign = "+";
                            break;
                    }

                    #region Insert TPSTSalDTDis
                    if (aoSalDT.Count == 1)
                    {

                        // Set ค่ามูลค่า Dis Chg เลยกรณีมีแค่ 1 Row
                        oSql.Clear();
                        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + " ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                        oSql.AppendLine("(");
                        oSql.AppendLine("  FTBchCode,FTXshDocNo,FNXsdSeqNo");
                        oSql.AppendLine(" ,FDXddDateIns,FNXddStaDis,FTXddDisChgTxt");
                        oSql.AppendLine(" ,FTXddDisChgType,FCXddNet,FCXddValue");
                        oSql.AppendLine(" ,FTXddRefCode,FTDisCode,FTRsnCode"); //*Arm 63-03-12 เลขที่อ้างอิง Redeem //*Arm 63-06-30 เพิ่ม FTDisCode,FTRsnCode
                        oSql.AppendLine(")");
                        oSql.AppendLine("VALUES");
                        oSql.AppendLine("(");
                        oSql.AppendLine("  '" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + aoSalDT[0].FNXsdSeqNo + "");
                        //oSql.AppendLine(" ,GETDATE(),2,'" + ptDisChgTxt + "'");
                        oSql.AppendLine(" ,GETDATE()," + pnStaDis + ",'" + ptDisChgTxt + "'"); //*Arm 63-06-11
                        oSql.AppendLine(" ,'" + pnDisType + "','" + aoSalDT[0].FCXsdNetAfHD + "','" + pcAmt + "'");
                        oSql.AppendLine(" ,'" + ptRefCode + "','"+ cVB.tVB_DisCode +"', '"+ ptRsnCode + "'");//*Arm 63-03-12 เลขที่อ้างอิง Redeem //*Arm 63-06-30 เพิ่ม FTDisCode,FTRsnCode //*Arm 63-07-14 FTDisCode รับค่าจาก tVB_DisCode
                        oSql.AppendLine(")");
                        oDB.C_SETxDataQuery(oSql.ToString());


                        // Set NetAfHD เลยกรณีมีแค่ 1 Row
                        oSql.Clear();
                        oSql.AppendLine("UPDATE " + cSale.tC_TblSalDT + " WITH(ROWLOCK) SET ");
                        oSql.AppendLine(" FCXsdNetAfHD=FCXsdNetAfHD " + tSign + " " + pcAmt);
                        oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "' AND FNXsdSeqNo=" + aoSalDT[0].FNXsdSeqNo + " AND FTBchCode='" + cVB.tVB_BchCode + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());
                    }
                    else
                    {

                        //Set ค่ามูลค่า Dis Chg มากกว่า 1 Row
                        cLastRowDisChg = 0;
                        cAmtDisChg = 0;
                        for (int nRow = 0; nRow < aoSalDT.Count - 1; nRow++)
                        {

                            cDisChg = Math.Round(Convert.ToDecimal(aoSalDT[nRow].FCXsdNetAfHD * (pcAmt / cSumDTNet)), cVB.nVB_DecSave);
                            cAmtDisChg += cDisChg;

                            oSql.Clear();
                            oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + " "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                            oSql.AppendLine("(");
                            oSql.AppendLine("  FTBchCode,FTXshDocNo,FNXsdSeqNo");
                            oSql.AppendLine(" ,FDXddDateIns,FNXddStaDis,FTXddDisChgTxt");
                            oSql.AppendLine(" ,FTXddDisChgType,FCXddNet,FCXddValue");
                            oSql.AppendLine(" ,FTXddRefCode,FTDisCode,FTRsnCode");  //*Arm 63-03-12 เลขที่อ้างอิง Redeem //*Arm 63-06-30 เพิ่ม FTDisCode,FTRsnCode
                            oSql.AppendLine(")");
                            oSql.AppendLine("VALUES");
                            oSql.AppendLine("(");
                            oSql.AppendLine("  '" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + aoSalDT[nRow].FNXsdSeqNo + "");
                            //oSql.AppendLine(" ,GETDATE(),2,'" + ptDisChgTxt + "'");
                            oSql.AppendLine(" ,GETDATE()," + pnStaDis + ",'" + ptDisChgTxt + "'"); //*Arm 63-06-11
                            oSql.AppendLine(" ,'" + pnDisType + "','" + aoSalDT[nRow].FCXsdNetAfHD + "','" + cDisChg + "'");
                            oSql.AppendLine(" ,'" + ptRefCode + "','" + cVB.tVB_DisCode + "', '" + ptRsnCode + "'");   //*Arm 63-03-12 เลขที่อ้างอิง Redeem //*Arm 63-06-30 เพิ่ม FTDisCode,FTRsnCode //*Arm 63-07-14 FTDisCode รับค่าจาก tVB_DisCode
                            oSql.AppendLine(")");
                            oDB.C_SETxDataQuery(oSql.ToString());

                            oSql.Clear();
                            oSql.AppendLine("UPDATE " + cSale.tC_TblSalDT + " WITH(ROWLOCK) SET ");
                            oSql.AppendLine(" FCXsdNetAfHD=FCXsdNetAfHD " + tSign + " " + cDisChg);
                            oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "' AND FNXsdSeqNo=" + aoSalDT[nRow].FNXsdSeqNo + " AND FTBchCode='" + cVB.tVB_BchCode + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());

                        }

                        cLastRowDisChg = pcAmt - cAmtDisChg;
                        oSql.Clear();
                        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + " ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                        oSql.AppendLine("(");
                        oSql.AppendLine("  FTBchCode,FTXshDocNo,FNXsdSeqNo");
                        oSql.AppendLine(" ,FDXddDateIns,FNXddStaDis,FTXddDisChgTxt");
                        oSql.AppendLine(" ,FTXddDisChgType,FCXddNet,FCXddValue");
                        oSql.AppendLine(" ,FTXddRefCode, FTDisCode, FTRsnCode");  //*Arm 63-03-12 เลขที่อ้างอิง Redeem //*Arm 63-06-30 เพิ่ม FTDisCode,FTRsnCode
                        oSql.AppendLine(")");
                        oSql.AppendLine("VALUES");
                        oSql.AppendLine("(");
                        oSql.AppendLine("  '" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + aoSalDT[aoSalDT.Count - 1].FNXsdSeqNo + "");
                        //oSql.AppendLine(" ,GETDATE(),2,'" + ptDisChgTxt + "'");
                        oSql.AppendLine(" ,GETDATE()," + pnStaDis + ",'" + ptDisChgTxt + "'"); //*Arm 63-06-11
                        oSql.AppendLine(" ,'" + pnDisType + "','" + aoSalDT[aoSalDT.Count - 1].FCXsdNetAfHD + "','" + cLastRowDisChg + "'");
                        oSql.AppendLine(" ,'" + ptRefCode + "', '" + cVB.tVB_DisCode + "', '" + ptRsnCode + "'");   //*Arm 63-03-12 เลขที่อ้างอิง Redeem //*Arm 63-06-30 เพิ่ม FTDisCode,FTRsnCode //*Arm 63-07-14 FTDisCode รับค่าจาก tVB_DisCode
                        oSql.AppendLine(")");
                        oDB.C_SETxDataQuery(oSql.ToString());

                        oSql.Clear();
                        oSql.AppendLine("UPDATE " + cSale.tC_TblSalDT + " WITH(ROWLOCK) SET ");
                        oSql.AppendLine(" FCXsdNetAfHD=FCXsdNetAfHD " + tSign + " " + cLastRowDisChg);
                        oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "' AND FNXsdSeqNo=" + aoSalDT[aoSalDT.Count - 1].FNXsdSeqNo + " AND FTBchCode='" + cVB.tVB_BchCode + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());

                    }
                    #endregion 

                    //cSale.C_DATxUpdVat(); //*Net 63-07-30 ปรับตาม Moshi
                    //cSale.C_DATxUpdCost();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPayment", "C_PRCxDisChgProrateDT : " + oEx.Message);
            }
            finally
            {
                aoSalDT = null;
                oSql = null;
                oDB = null;
                ptRefCode = "";
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// ถ้าเป็นคูปองส่วนลด ให้ตรวจสอบว่ามีการชาร์จหรือชำระเงินหรือยัง
        /// </summary>
        private static bool C_CHKbDiscountCoupon()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            try
            {
                //ตรวจสอบยอดเงินที่สามารรถลดได้กับมูลค่าคูปอง


                oSql.AppendLine("SELECT FTXshDocNo FROM TPSTSalHDDis WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND (FTXhdDisChgType = '3' OR FTXhdDisChgType = '4')");
                oSql.AppendLine("UNION ALL");
                oSql.AppendLine("SELECT FTXshDocNo FROM TPSTSalRC RC WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RC.FTRcvCode = RCV.FTRcvCode");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND RCV.FTFmtCode <> '020'");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                if (odtTmp == null)
                {
                    return true;
                }
                else
                {
                    if (odtTmp.Rows.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPayment", "C_CHKbPaymentAndChang : " + oEx.Message);
                return false;
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Process ยกเลิกคูปอง
        /// </summary>
        /// <param name="pnMode">1:Back from Paymant 2:Bill Refund</param>
        /// <param name="ptDocNo"></param>
        /// <returns></returns>
        public static bool C_PRCbCancelCoupon(int pnMode, string ptDocNo)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            string tTblCpn = "";
            try
            {
                new cLog().C_WRTxLog("cPayment", MethodBase.GetCurrentMethod().Name + $" : Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                oSql = new StringBuilder();
                //switch (pnMode)
                //{
                //    case 1: //Back from payment
                //        oSql.AppendLine("SELECT DISTINCT DTHis.FTCpbFrmSalRef,ISNULL(CPT.FTCptStaChkHQ,'2') AS FTCptStaChkHQ");
                //        oSql.AppendLine("FROM TFNTCouponDTHis DTHis WITH(NOLOCK)");
                //        oSql.AppendLine("INNER JOIN TFNTCouponHD HD WITH(NOLOCK) ON HD.FTCphDocNo = DTHis.FTCphDocNo AND HD.FTBchCode = DTHis.FTBchCode");
                //        oSql.AppendLine("INNER JOIN TFNMCouponType CPT WITH(NOLOCK) ON CPT.FTCptCode = HD.FTCptCode");
                //        oSql.AppendLine("WHERE DTHis.FTCpbFrmSalRef = '" + ptDocNo + "'");
                //        break;
                //    case 2: //Bill refund
                //        tTblCpn = "TPSTSalRC";
                //        oSql.AppendLine("SELECT DISTINCT RC.FTXshDocNo,ISNULL(CPT.FTCptStaChkHQ,'2') AS FTCptStaChkHQ");
                //        oSql.AppendLine("FROM TPSTSalRC RC WITH(NOLOCK)");
                //        oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RCV.FTRcvCode = RC.FTRcvCode AND (RCV.FTFmtCode = '004' OR RCV.FTFmtCode = '020')");
                //        oSql.AppendLine("INNER JOIN TFNMCouponType CPT WITH(NOLOCK) ON CPT.FTCptCode = RC.FTXrcRefNo2 AND FTCptStaChk = '1'");
                //        oSql.AppendLine("WHERE RC.FTXshDocNo = '" + ptDocNo + "'");
                //        break;
                //    default:
                //        oSql.AppendLine("SELECT DISTINCT DTHis.FTCpbFrmSalRef,ISNULL(CPT.FTCptStaChkHQ,'2') AS FTCptStaChkHQ");
                //        oSql.AppendLine("FROM TFNTCouponDTHis DTHis WITH(NOLOCK)");
                //        oSql.AppendLine("INNER JOIN TFNTCouponHD HD WITH(NOLOCK) ON HD.FTCphDocNo = DTHis.FTCphDocNo AND HD.FTBchCode = DTHis.FTBchCode");
                //        oSql.AppendLine("INNER JOIN TFNMCouponType CPT WITH(NOLOCK) ON CPT.FTCptCode = HD.FTCptCode");
                //        oSql.AppendLine("WHERE DTHis.FTCpbFrmSalRef = '" + ptDocNo + "'");
                //        break;
                //}
                oSql.AppendLine("SELECT DISTINCT DTHis.FTCpbFrmSalRef,ISNULL(CPT.FTCptStaChkHQ,'2') AS FTCptStaChkHQ");
                oSql.AppendLine("FROM TFNTCouponDTHis DTHis WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TFNTCouponHD HD WITH(NOLOCK) ON HD.FTCphDocNo = DTHis.FTCphDocNo");
                oSql.AppendLine("INNER JOIN TFNMCouponType CPT WITH(NOLOCK) ON CPT.FTCptCode = HD.FTCptCode");
                oSql.AppendLine("WHERE DTHis.FTCpbFrmSalRef = '" + ptDocNo + "'");

                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                if (odtTmp == null || odtTmp.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        string tUrl = "";

                        if (oRow.Field<String>("FTCptStaChkHQ") == "1")
                        {
                            tUrl = cVB.tVB_API2FNWalletHQ;
                        }
                        else
                        {
                            tUrl = cVB.tVB_API2FNWallet;
                        }

                        if (string.IsNullOrEmpty(tUrl))
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlWalletNotDefine"), 3);
                            return false;
                        }
                        else
                        {
                            new cLog().C_WRTxLog("cPayment", MethodBase.GetCurrentMethod().Name + $" : Call Api Cancel Cpn", cVB.bVB_AlwPrnLog); //*Net Stamp
                            cmlReqCancelCoupon oReqCoupon = new cmlReqCancelCoupon();
                            oReqCoupon.ptCpbFrmBch = cVB.tVB_BchCode;
                            oReqCoupon.ptCpbFrmPos = cVB.tVB_PosCode;
                            oReqCoupon.ptCpbFrmSaleRef = ptDocNo;
                            string tJSonCall = JsonConvert.SerializeObject(oReqCoupon);
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);
                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tUrl + "/Coupon/CancelPayCoupon", tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("cPayment", "C_PRCbCancelCoupon : " + oEx.Message);
                                return false;
                            }

                            new cLog().C_WRTxLog("cPayment", MethodBase.GetCurrentMethod().Name + $" : Get Api Response", cVB.bVB_AlwPrnLog); //*Net Stamp
                            if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                                cmlResCoupon oRes = JsonConvert.DeserializeObject<cmlResCoupon>(tJSonRes);
                                if (oRes.rtCode == "1")
                                {
                                   
                                }
                                else
                                {
                                    new cSP().SP_SHWxMsg(oRes.rtCode + " : " + oRes.rtDesc, 2);
                                    new cLog().C_WRTxLog("cPayment", "C_PRCbCancelCoupon : " + oRes.rtDesc, cVB.bVB_AlwPrnLog);
                                    return false;
                                }
                            }
                            //*Net 63-07-30 ปรับตาม Moshi
                            oCall.C_PRCxCloseConn();    //*Em 63-07-18
                        }
                        oSql = new StringBuilder();
                        oSql.AppendLine("UPDATE TFNTCouponDTHis WITH(ROWLOCK) SET ");
                        oSql.AppendLine("FTCpbStaBook = '3',");
                        oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                        oSql.AppendLine($"FTLastUpdBy = 'AdaPosFront'");
                        oSql.AppendLine($"WHERE FTCpbFrmBch ='{cVB.tVB_BchCode}' AND FTCpbFrmPos ='{cVB.tVB_PosCode}' AND FTCpbFrmSalRef ='{ptDocNo}'");
                        oDB.C_GEToDataQuery(oSql.ToString());
                    }
                }

                new cLog().C_WRTxLog("cPayment", MethodBase.GetCurrentMethod().Name + $" : End", cVB.bVB_AlwPrnLog); //*Net Stamp
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPayment", "C_PRCbCancelCoupon : " + oEx.Message);
                return false;
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// *Arm 63-03-12
        /// C_PRCxPaymentRedeem 
        /// </summary>
        /// <param name="ptRdhDocType"></param>
        /// <param name="pnSeqNo">RD Type 1 อ้างอิงลำดับใน DT ,RD Type 2 อ้างอิงลำดับใน RC</param>
        /// <param name="cQty"></param>
        /// <param name="pnUsePoint"></param>
        public static void C_PRCxPaymentRedeem(string ptRdhDocType, decimal cQty, int pnUsePoint, int pnSeqNo = 0)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            cmlRdSalRD oSalRD;
            decimal cAmtLeft = 0, cAmt = 0, cChange = 0, cAmtDep = 0;
            decimal cNet = 0, cAmtFLeft = 0;
            try
            {
                nC_SeqRC = nC_SeqRC + 1;
                cAmtLeft = cVB.oVB_Payment.cW_AmtTotalShw;
                cAmtFLeft = cAmtLeft;
                cAmt = cVB.cVB_Amount;
                if (cAmt >= cAmtLeft)
                {
                    cChange = cAmt - cAmtLeft;
                    cNet = cAmtLeft;
                    cAmtLeft = 0;
                }
                else
                {
                    cNet = cAmt;
                    cChange = 0;
                    cAmtLeft = cAmtLeft - cNet;
                }

                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRC + " (FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, ");
                oSql.AppendLine("FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                oSql.AppendLine("FCXrcDep, FCXrcNet, FCXrcChg, ");
                oSql.AppendLine("FTXrcRefNo1,FTXrcRefNo2,FDXrcRefDate,");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("VALUES('" + cVB.tVB_BchCode + "','" + cVB.tVB_DocNo + "'," + nC_SeqRC + ",'" + tC_RcvCode + "','" + tC_RcvName + "',");
                oSql.AppendLine("'" + cVB.tVB_RateCode + "'," + cVB.cVB_Rate + "," + cAmtFLeft + "," + cAmt + ",");
                oSql.AppendLine(cAmtDep + "," + cNet + "," + cChange + ",");
                oSql.AppendLine("'" + tC_XrcRef1 + "','" + tC_XrcRef2 + "',CONVERT(VARCHAR(10),GETDATE(),121),");
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                //*Net 63-07-30 ปรับตาม Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02 เช็ค insert RC
                string tErr = "";
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }

                // Insert SalRD
                oSalRD = new cmlRdSalRD();
                oSalRD.FTBchCode = cVB.tVB_BchCode;
                oSalRD.FTXshDocNo = cVB.tVB_DocNo;
                oSalRD.FTXrdRefCode = tC_XrcRef1;
                oSalRD.FTRdhDocType = ptRdhDocType;
                if (pnSeqNo == 0)
                {
                    //ใช้แต้มแลกส่วนลด
                    oSalRD.FNXrdRefSeq = nC_SeqRC;
                }
                else
                {
                    //ใช้แต้มแลก+เงิน แลกสิค้า
                    oSalRD.FNXrdRefSeq = pnSeqNo;
                }
                oSalRD.FCXrdPdtQty = cQty;
                oSalRD.FNXrdPntUse = pnUsePoint;
                new cSale().C_PRCxInsertSalRD(oSalRD);

                cVB.oVB_Payment.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //*Net 63-07-30 ปรับตาม Moshi
                //if(cVB.oVB_2ndScreen != null)
                //{
                //    cVB.oVB_2ndScreen.W_DATxAddPay2Grid(tC_RcvName, cAmt);
                //}


                //*Arm 63-04-07 Comment Code
                //if (cAmtLeft == 0)
                //{
                //    cVB.cVB_Change = cChange;
                //    cVB.cVB_RoundDiff = 0;
                //    cSale.C_PRCxSetComplete();
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "C_PRCxPaymentCreditCard : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                oSalRD = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        //*Net 63-04-01 ยกมาจาก baseline
        public static string W_PRCtCheckCreditNo(string ptCreditNo)
        {
            string tCreditNo = "";
            int nIndex = 0;
            char[] aCreditNo = ptCreditNo.ToCharArray(0, ptCreditNo.Length);
            if (ptCreditNo.Length >= 6)
            {
                nIndex = 6;
            }
            else
            {
                nIndex = ptCreditNo.Length;
            }
            for (int i = 0; i < nIndex; i++)
            {
                if (aCreditNo[i].ToString() == "x" || aCreditNo[i].ToString() == "X")
                {
                    // กรณี ค่าเป็น X
                }
                else
                {
                    tCreditNo += aCreditNo[i].ToString();
                }

            }
            return tCreditNo;
        }

        //*Net 63-04-01 ยกมาจาก baseline
        public static DataTable W_GETtBankCode(string ptCrdCode)
        {

            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            cVB.tVB_CardBin = ptCrdCode;
            DataTable oDbTbl = new DataTable();
            try
            {

                oSql.AppendLine("SELECT distinct Crd.FTBnkCode,Crd_L.FTCrdName,Bank.FTBnkName,Crd.FTCrdCode FROM TFNMCreditCard Crd with(nolock)");
                oSql.AppendLine("LEFT JOIN TFNMCreditCard_L Crd_L with(nolock) ON Crd.FTCrdCode = Crd_L.FTCrdCode");
                oSql.AppendLine("LEFT JOIN TFNMCreditCardBIN Crdbin with(nolock) ON Crd.FTCrdCode = Crdbin.FTCrdCode");
                oSql.AppendLine("LEFT JOIN TFNMBank_L Bank with(nolock) ON Bank.FTBnkCode = Crd.FTBnkCode");
                oSql.AppendLine("WHERE Bank.FNLngID = " + cVB.nVB_Language + "");
                if (ptCrdCode.Length == 6)
                {
                    oSql.AppendLine("AND Crdbin.FTCrdBinCode = '" + ptCrdCode + "'");
                }
                else
                {
                    oSql.AppendLine("AND LEFT(Crdbin.FTCrdBinCode,4) = '" + ptCrdCode + "'");
                }

                oDbTbl = oDB.C_GEToDataQuery(oSql.ToString());



            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "W_GETtBankCode : " + oEx.Message); }
            finally
            {
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem

            }
            return oDbTbl;
        }

        //*Net 63-04-01 ยกมาจาก baseline
        public static string W_GETtNameBankByCode(string ptCode)
        {
            string ptName = "";
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();

            try
            {

                oSql.AppendLine(" SELECT FTBnkName FROM TFNMBank_L with(nolock) WHERE FTBnkCode = '" + ptCode + "' AND FNLngID = " + cVB.nVB_Language);

                DataTable oDT = oDB.C_GEToDataQuery(oSql.ToString());
                ptName = oDT.Rows[0]["FTBnkName"].ToString();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "W_GETtNameBankByCode : " + oEx.Message); }
            finally
            {
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem

            }
            return ptName;
        }

        //*Net 63-04-01 ยกมาจาก baseline
        public static string W_GETtCrdCodeByName(string ptName)
        {
            string ptCode = "";
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();

            try
            {

                oSql.AppendLine("SELECT TOP 1 FTCrdCode FROM TFNMCreditCard_L  with(nolock) WHERE FTCrdName = '" + ptName + "' ");

                DataTable oDT = oDB.C_GEToDataQuery(oSql.ToString());
                ptCode = oDT.Rows[0]["FTCrdCode"].ToString();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "W_GETtCrdCodeByName : " + oEx.Message); }
            finally
            {
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem

            }
            return ptCode;
        }
    }

}

