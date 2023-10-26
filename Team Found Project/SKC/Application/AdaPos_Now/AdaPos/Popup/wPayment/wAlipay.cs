using AdaPos.Class;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdaPaymentSvc.xClass;
using AdaPaymentSvc.xModel.Alipay;
namespace AdaPos.Popup.wPayment
{
    public partial class wAlipay : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private string tW_Currency;
        private string tW_PaymentType;
        private int nW_Timeout;
        private int nW_TimeQuery;
        private string tW_Url;
        private string tW_XKey;
        private int nW_Reverse;
        private string tW_MerchantID;
        private int nW_TimeQrySum;
        private int nW_TimeQryCnt;
        private int nW_TimeRevCnt;
        private int nW_TimeRevRnd;

        private string tW_AliStatus;
        private string tW_AliErrCode;
        private string tW_AliErrDesc;
        private string tW_AliDateTime;
        private string tW_AliTransID;
        private string tW_AliPaySta;

        private string tW_AliDesc;
        private string tW_AliStoreID;
        public string tW_AliReason;
        public string tW_AliLng;
        public string tW_Mode;
        const string tW_SqlGetMsg = "SELECT FTErrDesc FROM TSysMsgAlipay_L WITH(NOLOCK) WHERE FTErrFunc = '{0}' AND FTErrCode ='{1}' AND FNLngID = {2}";
        #endregion End Variable

        public wAlipay()
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();

                W_SETxDesign();
                W_SETxText();
                W_GETxConfigPay();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "wAlipay " + oEx.Message); }
        }

        #region Function
        /// <summary>
        /// Set design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                olaTitleAlipay.Text = oW_Resource.GetString("tTitleAlipay");
                olaKeyScan.Text = oW_Resource.GetString("tKeyScan");
                otbPaymentCode.Text = "";
                olaDescription.Text = "";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_SETxText : " + oEx.Message); }
        }

        private void W_GETxConfigPay()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            try
            {
                oSql.AppendLine("SELECT FNSysSeq,FTSysKey,FTSysStaUsrValue,FTSysStaUsrRef ");
                oSql.AppendLine("FROM TSysRcvConfig WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTFmtCode = '012'");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null)
                {
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        switch (oRow["FTSysKey"].ToString().ToLower())
                        {
                            case "currency":
                                tW_Currency = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "paymenttype":
                                tW_PaymentType = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "timequery":
                                nW_TimeQuery = Convert.ToInt32(oRow["FTSysStaUsrValue"].ToString());
                                break;
                            case "timeout":
                                nW_Timeout = Convert.ToInt32( oRow["FTSysStaUsrValue"].ToString());
                                break;
                            case "url":
                                tW_Url = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "x-key":
                                tW_XKey = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "reverse":
                                nW_Reverse = Convert.ToInt32( oRow["FTSysStaUsrValue"].ToString());
                                break;
                            case "merchantid":
                                tW_MerchantID = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "storeid": //'*Em 62-11-18
                                tW_AliStoreID = oRow["FTSysStaUsrValue"].ToString();
                                break;
                        }
                    }
                }
                else
                {
                    tW_Currency = "thb";
                    tW_PaymentType = "barcode";
                    tW_MerchantID = "";
                    tW_Url = "";
                    tW_XKey = "";
                    nW_Reverse = 3;
                    nW_Timeout = 60;
                    nW_TimeQuery = 5;
                }

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 CMP.FTCmpCode,ISNULL(CMPL.FTCmpName,(SELECT TOP 1 FTCmpName FROM TCNMComp_L WITH(NOLOCK) WHERE FTCmpCode = CMP.FTCmpCode)) AS FTCmpName");
                oSql.AppendLine("FROM TCNMComp CMP WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMComp_L CMPL WITH(NOLOCK) ON CMP.FTCmpCode = CMPL.FTCmpCode AND CMPL.FNLngID = 1");

                odtTmp = new DataTable();
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        //tW_AliStoreID = odtTmp.Rows[0].Field<string>("FTCmpCode");
                        tW_AliDesc = odtTmp.Rows[0].Field<string>("FTCmpName");
                    }
                }
                
                switch (cVB.nVB_Language)
                {
                    case 1: tW_AliLng = "THA"; break;
                    case 2: tW_AliLng = "ENG"; break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_GETxConfigPay " + oEx.Message); }
        }

        private void W_PRCxKeepLog(string ptFunc)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            decimal cAmt = 0; //*Arm 63-09-02 
            try
            {
                cAmt = Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, cVB.cVB_Amount, 2)); //*Arm 63-09-02 
                tW_AliErrDesc = tW_AliErrDesc.Replace("'", "");
                if (ptFunc == "Refund")
                {
                    oSql.AppendLine("INSERT INTO TPSTLogAlipay "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine("( FTLogFunc, FTLogInvNo, FTLogInvAmt, FTLogCurrency, FTLogDescription, FTLogPaymentCode, ");
                    oSql.AppendLine("FTLogPaymentType, FTLogPosCode, FTLogStatus, FTLogErrCode, FTLogErrDesc, FTLogDateTime, ");
                    oSql.AppendLine("FTLogTransID, FTLogPaySta, FTLogRefundAmt, FTLogRefundNo, FTLogReason, ");
                    oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("VALUES");
                    oSql.AppendLine("('" + ptFunc + "','" + cVB.tVB_RefDocNo + "','','" + tW_Currency + "','" + tW_AliDesc + "','" + otbPaymentCode.Text + "',");
                    oSql.AppendLine("'" + tW_PaymentType + "','" + cVB.tVB_PosCode + "','" + tW_AliStatus + "','" + tW_AliErrCode + "','" + tW_AliErrDesc + "','" + tW_AliDateTime + "',");
                    //oSql.AppendLine("'" + tW_AliTransID + "','','" + cVB.cVB_Amount + "','" + cVB.tVB_DocNo + "','" + tW_AliReason + "',");
                    oSql.AppendLine("'" + tW_AliTransID + "','','" + cAmt + "','" + cVB.tVB_DocNo + "','" + tW_AliReason + "',"); //*Arm 63-09-02
                    oSql.AppendLine("GETDATE(),'"+ cVB.tVB_UsrName +"',GETDATE(),'" + cVB.tVB_UsrName + "')");
                }
                else
                {
                    oSql.AppendLine("INSERT INTO TPSTLogAlipay ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine("( FTLogFunc, FTLogInvNo, FTLogInvAmt, FTLogCurrency, FTLogDescription, FTLogPaymentCode, ");
                    oSql.AppendLine("FTLogPaymentType, FTLogPosCode, FTLogStatus, FTLogErrCode, FTLogErrDesc, FTLogDateTime, ");
                    oSql.AppendLine("FTLogTransID, FTLogPaySta, FTLogRefundAmt, FTLogRefundNo, FTLogReason, ");
                    oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("VALUES");
                    //oSql.AppendLine("('" + ptFunc + "','" + cVB.tVB_DocNo + "','"+ cVB.cVB_Amount + "','" + tW_Currency + "','" + tW_AliDesc + "','" + otbPaymentCode.Text + "',");
                    oSql.AppendLine("('" + ptFunc + "','" + cVB.tVB_DocNo + "','" + cAmt + "','" + tW_Currency + "','" + tW_AliDesc + "','" + otbPaymentCode.Text + "',"); //*Arm 63-09-02
                    oSql.AppendLine("'" + tW_PaymentType + "','" + cVB.tVB_PosCode + "','" + tW_AliStatus + "','" + tW_AliErrCode + "','" + tW_AliErrDesc + "','" + tW_AliDateTime + "',");
                    oSql.AppendLine("'" + tW_AliTransID + "','','','','',");
                    oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrName + "',GETDATE(),'" + cVB.tVB_UsrName + "')");
                }
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_PRCxKeepLog " + oEx.Message); }
        }
        private void W_PRCxPayment()
        {
            cAlipayPay oAlipay = new cAlipayPay();
            cmlALIPayReq oReq = new cmlALIPayReq();
            cmlALIPayRes oRes = new cmlALIPayRes();
            string tMsg = "";
            try
            {
                if (oAlipay != null)
                {
                    this.Enabled = false;
                    oReq.MerchantID = tW_MerchantID;
                    oReq.InvoiceNO = cVB.tVB_DocNo;
                    oReq.InvoiceTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //oReq.InvoiceAmt = (double)cVB.cVB_Amount; //*Arm 63-09-02 Comment Code
                    oReq.InvoiceAmt = Convert.ToDouble(new cSP().SP_SETtDecShwSve(2, cVB.cVB_Amount, 2)); //*Arm 63-09-02 ปัดเศษทศนิยม 2 ตำแหน่ง
                    //oReq.Currency = tW_Currency.ToLower();
                    oReq.Currency = tW_Currency.ToUpper();  //*Em 62-11-14
                    oReq.Description = tW_AliDesc;
                    oReq.Barcode = otbPaymentCode.Text;
                    oReq.BarcodeType = tW_PaymentType.ToLower();
                    oReq.TerminalID = cVB.tVB_PosCode;
                    oReq.BranchID = cVB.tVB_BchCode;
                    oReq.StoreID = tW_AliStoreID;
                    oReq.Language = tW_AliLng;
                    oReq.Memo = "Shopping";//*Em 62-11-14

                    oAlipay.PaymentURL = tW_Url + "/ada_payment";
                    oAlipay.XKey = tW_XKey;
                    oAlipay.TimeOut = nW_Timeout * 1000;
                    oRes = oAlipay.C_PAYoRequest(oReq);

                    if (oRes == null)
                    { }
                    else
                    {
                        tW_AliStatus = oRes.Result_Code;
                        tW_AliErrCode = oRes.Error_Code;
                        tW_AliErrDesc = oRes.Error_Desc;
                        tW_AliDateTime = oRes.ADATransTime;
                        tW_AliTransID = oRes.ADATransID;
                    }
                    W_PRCxKeepLog("ada_payment");
                    olaDescription.Text = "STATUS : " + tW_AliStatus;
                    switch (tW_AliStatus.ToUpper())
                    {
                        case "SUCCESS":
                            ocmAccept_Click(ocmAccept,null);
                            break;
                        case "PENDING":
                            if (tW_AliErrCode.ToUpper() == "SYSTEM_ADA_ERR")
                            {
                                this.Enabled = true;
                                olaDescription.Text += Environment.NewLine + "ErrCode : " + tW_AliErrCode;
                                if (tW_AliErrDesc != "")
                                {
                                    tMsg = tW_AliErrDesc;
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(tW_AliErrCode))
                                    {
                                        tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                    else
                                    {
                                        tMsg = "";  // get message from TSysMsgAlipay
                                        if (string.IsNullOrEmpty(tMsg))
                                        {
                                            tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                            new cSP().SP_SHWxMsg(tMsg, 2);
                                        }
                                        else
                                        {
                                            new cSP().SP_SHWxMsg(tMsg, 2);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                otmQry.Enabled = true;
                            }
                            break;
                        case "SYSTEM_ERR":
                            W_PRCxReverse();
                            break;
                        case "FAILED":
                            this.Enabled = true;
                            olaDescription.Text += Environment.NewLine + "ErrCode : " + tW_AliErrCode;
                            if (tW_AliErrDesc != "")
                            {
                                tMsg = tW_AliErrDesc;
                                new cSP().SP_SHWxMsg(tMsg, 2);
                                new cLog().C_WRTxLog("wAlipay", "W_PRCxPayment " + tMsg);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(tW_AliErrCode))
                                {
                                    tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                    new cLog().C_WRTxLog("wAlipay", "W_PRCxPayment " + tMsg);
                                }
                                else
                                {
                                    tMsg = new cDatabase().C_GEToDataQuery(string.Format(tW_SqlGetMsg, "Payment",tW_AliErrCode,cVB.nVB_Language)).Rows[0].Field<string>("FTErrDesc").ToString();  // get message from TSysMsgAlipay
                                    if (string.IsNullOrEmpty(tMsg))
                                    {
                                        tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                        new cLog().C_WRTxLog("wAlipay", "W_PRCxPayment " + tMsg);
                                    }
                                    else
                                    {
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                        new cLog().C_WRTxLog("wAlipay", "W_PRCxPayment " + tMsg);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_PRCxPayment " + oEx.Message); }
            finally
            {
                oAlipay = null;
                oReq = null;
                oRes = null;
            }
        }

        private void W_PRCxReverse()
        {
            cAlipayPay oAlipay = new cAlipayPay();
            cmlALIRevReq oReq = new cmlALIRevReq();
            cmlALIRevRes oRes = new cmlALIRevRes();
            string tMsg = "";
            try
            {
                if (oAlipay != null)
                {
                    oReq.MerchantID = tW_MerchantID;
                    oReq.InvoiceNO = cVB.tVB_DocNo;
                    oReq.TerminalID = cVB.tVB_PosCode;
                    oReq.BranchID = cVB.tVB_BchCode;
                    oReq.StoreID = tW_AliStoreID;
                    oReq.Language = tW_AliLng;

                    oAlipay.PaymentURL = tW_Url + "/ada_reverse";
                    oAlipay.XKey = tW_XKey;
                    oAlipay.TimeOut = nW_Timeout * 1000;
                    oRes = oAlipay.C_REVoRequest(oReq);
                    if (oRes == null)
                    { }
                    else
                    {
                        tW_AliStatus = oRes.Result_Code;
                        tW_AliErrCode = oRes.Error_Code;
                        tW_AliErrDesc = oRes.Error_Desc;
                        tW_AliDateTime = oRes.ADATransTime;
                        tW_AliTransID = oRes.ADATransID;
                    }
                    W_PRCxKeepLog("ada_reverse");
                    
                    switch (tW_AliStatus.ToUpper())
                    {
                        case "SUCCESS":
                            olaDescription.Text = "STATUS : REVERSE SUCCESS";
                            otmReverse.Enabled = false;
                            tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                            new cSP().SP_SHWxMsg(tMsg, 2);
                            this.Dispose();
                            break;
                        case "FAILED":
                            olaDescription.Text = "STATUS : REVERSE FAILED";
                            if (otmReverse.Enabled == false) otmReverse.Enabled = true;
                            nW_TimeRevRnd += 1;
                            break;
                        case "PENDING":
                            if (otmReverse.Enabled == false) otmReverse.Enabled = true;
                            if (tW_AliErrCode == "SYSTEM_ADA_ERR")
                            {
                                olaDescription.Text = "STATUS : REVERSE SYSTEM_ADA_ERR";
                            }
                            else
                            {
                                olaDescription.Text = "STATUS : REVERSE PENDING";
                            }
                            nW_TimeRevRnd += 1;
                            break;
                        default:
                            olaDescription.Text = "STATUS : REVERSE " + tW_AliStatus;
                            if (otmReverse.Enabled == false) otmReverse.Enabled = true;
                            nW_TimeRevRnd += 1;
                            break;
                        
                    }

                    if (nW_TimeRevRnd >= nW_Reverse)
                    {
                        otmReverse.Enabled = false;
                        if (tW_AliErrDesc != "")
                        {
                            tMsg = tW_AliErrDesc;
                            new cSP().SP_SHWxMsg(tMsg, 2);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(tW_AliErrCode))
                            {
                                tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                            else
                            {
                                tMsg = new cDatabase().C_GEToDataQuery(string.Format(tW_SqlGetMsg, "Reverse", tW_AliErrCode, cVB.nVB_Language)).Rows[0].Field<string>("FTErrDesc").ToString();  // get message from TSysMsgAlipay
                                if (string.IsNullOrEmpty(tMsg))
                                {
                                    tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                                else
                                {
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                            }
                        }

                        //ถ้า Reverse ไม่สำเร็จให้ทำการ Void ซ้ำ
                        if (tW_AliStatus.ToUpper() == "PENDING" && tW_AliStatus.ToUpper() == "SYSTEM_ADA_ERR")
                        {
                            olaDescription.Text = "STATUS : VOID";
                            cVB.tVB_RefDocNo = cVB.tVB_DocNo;
                            W_PRCxVoid();
                            if (tW_AliStatus.ToUpper() == "SUCCESS")
                            {
                                tMsg = oW_Resource.GetString("tMsgAlipayAutoVoid");
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                            else
                            {
                                tMsg = oW_Resource.GetString("tMsgAlipayProblem");
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                        }
                        this.Close();
                        this.Dispose();
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_PRCxReverse " + oEx.Message); }
            finally
            {
                oAlipay = null;
                oReq = null;
                oRes = null;
            }
        }

        public void W_PRCxVoid()
        {
            cAlipayPay oAlipay = new cAlipayPay();
            cmlALICancelReq oReq = new cmlALICancelReq();
            cmlALICancelRes oRes = new cmlALICancelRes();
            string tMsg = "";

            try
            {
                if (oAlipay != null)
                {
                    oReq.MerchantID = tW_MerchantID;
                    oReq.InvoiceNO = cVB.tVB_RefDocNo;
                    oReq.TerminalID = cVB.tVB_PosCode;
                    oReq.BranchID = cVB.tVB_BchCode;
                    oReq.StoreID = tW_AliStoreID;
                    oReq.Language = tW_AliLng;

                    oAlipay.PaymentURL = tW_Url + "/ada_void";
                    oAlipay.XKey = tW_XKey;
                    oAlipay.TimeOut = nW_Timeout * 1000;
                    oRes = oAlipay.C_CCoRequest(oReq);
                    if (oRes == null)
                    { }
                    else
                    {
                        tW_AliStatus = oRes.Result_Code;
                        tW_AliErrCode = oRes.Error_Code;
                        tW_AliErrDesc = oRes.Error_Desc;
                        tW_AliDateTime = oRes.ADATransTime;
                        tW_AliTransID = oRes.ADATransID;
                    }
                    W_PRCxKeepLog("ada_void");
                    olaDescription.Text = "STATUS : VOID " + tW_AliStatus;
                    if (tW_AliStatus.ToUpper() == "FAILED") olaDescription.Text += Environment.NewLine + "ErrCode : " + tW_AliErrCode;
                    switch (tW_AliStatus)
                    {
                        case "SUCCESS":
                            ocmAccept_Click(ocmAccept, null);
                            break;
                        case "FAILED":
                            if (tW_AliErrDesc != "")
                            {
                                tMsg = tW_AliErrDesc;
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(tW_AliErrCode))
                                {
                                    tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                                else
                                {
                                    tMsg = new cDatabase().C_GEToDataQuery(string.Format(tW_SqlGetMsg, "Void", tW_AliErrCode, cVB.nVB_Language)).Rows[0].Field<string>("FTErrDesc").ToString();  // get message from TSysMsgAlipay
                                    if (string.IsNullOrEmpty(tMsg))
                                    {
                                        tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                    else
                                    {
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                }
                            }
                            if (tW_Mode.ToLower() != "payment")
                            {
                                this.Close();
                                this.Dispose();
                            }
                            break;
                        case "PENDING":
                            if (tW_AliErrCode == "SYSTEM_ADA_ERR")
                            {
                                olaDescription.Text = "STATUS : VOID SYSTEM_ADA_ERR" + Environment.NewLine + "ErrCode : " + tW_AliErrDesc;
                            }
                            else
                            {
                                olaDescription.Text = "STATUS : VOID PENDING";
                            }
                            break;
                        case "SYSTEM_ERR":
                            olaDescription.Text = "STATUS : VOID SYSTEM_ERR";
                            if (tW_AliErrDesc != "")
                            {
                                tMsg = tW_AliErrDesc;
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(tW_AliErrCode))
                                {
                                    tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                                else
                                {
                                    tMsg = new cDatabase().C_GEToDataQuery(string.Format(tW_SqlGetMsg, "Void", tW_AliErrCode, cVB.nVB_Language)).Rows[0].Field<string>("FTErrDesc").ToString();  // get message from TSysMsgAlipay
                                    if (string.IsNullOrEmpty(tMsg))
                                    {
                                        tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                    else
                                    {
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                }
                            }
                            if (tW_Mode.ToLower() != "payment")
                            {
                                this.Close();
                                this.Dispose();
                            }
                            break;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_PRCxVoid " + oEx.Message); }
            finally
            {
                oAlipay = null;
                oReq = null;
                oRes = null;
            }
        }
        private void W_PRCxQuery()
        {
            cAlipayPay oAlipay = new cAlipayPay();
            cmlALIQryReq oReq = new cmlALIQryReq();
            cmlALIQryRes oRes = new cmlALIQryRes();
            string tMsg = "";
            try
            {
                if (oAlipay != null)
                {
                    oReq.MerchantID = tW_MerchantID;
                    oReq.InvoiceNO = cVB.tVB_DocNo;
                    oReq.TerminalID = cVB.tVB_PosCode;
                    oReq.BranchID = cVB.tVB_BchCode;
                    oReq.StoreID = tW_AliStoreID;
                    oReq.Language = tW_AliLng;

                    oAlipay.PaymentURL = tW_Url + "/ada_query";
                    oAlipay.XKey = tW_XKey;
                    oAlipay.TimeOut = nW_Timeout * 1000;
                    oRes = oAlipay.C_QRYoRequest(oReq);
                    if (oRes == null)
                    { }
                    else
                    {
                        tW_AliStatus = oRes.Result_Code;
                        tW_AliErrCode = oRes.Error_Code;
                        tW_AliErrDesc = oRes.Error_Desc;
                        tW_AliDateTime = oRes.ADATransTime;
                        tW_AliTransID = oRes.ADATransID;
                        tW_AliPaySta = oRes.PaymentSta;
                    }
                    W_PRCxKeepLog("ada_query");
                    olaDescription.Text = "STATUS : " + tW_AliStatus;
                    switch (tW_AliStatus.ToUpper())
                    {
                        case "SUCCESS":
                            switch(tW_AliPaySta.ToUpper())
                            {
                                case "SUCCESS":
                                    otmQry.Enabled = false;
                                    olaDescription.Text += Environment.NewLine + "Payment Status : " + tW_AliPaySta;
                                    ocmAccept_Click(ocmAccept, null);
                                    this.Enabled = true;
                                    break;
                                case "CLOSED":
                                    W_PRCxReverse();
                                    otmQry.Enabled = true;
                                    this.Enabled = true;
                                    break;
                                case "PENDING":
                                    olaDescription.Text += Environment.NewLine + "Payment Status : " + tW_AliPaySta;
                                    break;
                            }
                            break;
                        case "FAILED":
                            otmQry.Enabled = false;
                            this.Enabled = true;
                            olaDescription.Text += Environment.NewLine + "ErrCode : " + tW_AliErrCode;
                            if (tW_AliErrDesc != "")
                            {
                                tMsg = tW_AliErrDesc;
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(tW_AliErrCode))
                                {
                                    tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                                else
                                {
                                    tMsg = new cDatabase().C_GEToDataQuery(string.Format(tW_SqlGetMsg, "Query", tW_AliErrCode, cVB.nVB_Language)).Rows[0].Field<string>("FTErrDesc").ToString();  // get message from TSysMsgAlipay
                                    if (string.IsNullOrEmpty(tMsg))
                                    {
                                        tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                    else
                                    {
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                }
                            }
                            break;
                        case "PENDING":
                            break;
                        case "SYSTEM_ERR":
                            otmQry.Enabled = false;
                            W_PRCxReverse();
                            this.Enabled = true;
                            break;
                    }

                    if (nW_TimeRevRnd >= nW_Reverse)
                    {
                        otmReverse.Enabled = false;
                        if (tW_AliErrDesc != "")
                        {
                            tMsg = tW_AliErrDesc;
                            new cSP().SP_SHWxMsg(tMsg, 2);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(tW_AliErrCode))
                            {
                                tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                            else
                            {
                                tMsg = new cDatabase().C_GEToDataQuery(string.Format(tW_SqlGetMsg, "Query", tW_AliErrCode, cVB.nVB_Language)).Rows[0].Field<string>("FTErrDesc").ToString();  // get message from TSysMsgAlipay
                                if (string.IsNullOrEmpty(tMsg))
                                {
                                    tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                                else
                                {
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                            }
                        }

                        //ถ้า Reverse ไม่สำเร็จให้ทำการ Void ซ้ำ
                        if (tW_AliStatus.ToUpper() == "PENDING" && tW_AliStatus.ToUpper() == "SYSTEM_ADA_ERR")
                        {
                            olaDescription.Text = "STATUS : VOID";
                            cVB.tVB_RefDocNo = cVB.tVB_DocNo;

                            if (tW_AliStatus.ToUpper() == "SUCCESS")
                            {
                                tMsg = oW_Resource.GetString("tMsgAlipayAutoVoid");
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                            else
                            {
                                tMsg = oW_Resource.GetString("tMsgAlipayProblem");
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                        }
                        this.Close();
                        this.Dispose();
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_PRCxQuery " + oEx.Message); }
            finally
            {
                oAlipay = null;
                oReq = null;
                oRes = null;
            }
        }

        public void W_PRCxRefund()
        {
            cAlipayPay oAlipay = new cAlipayPay();
            cmlALIRefReq oReq = new cmlALIRefReq();
            cmlALIRefRes oRes = new cmlALIRefRes();
            string tMsg = "";
            try
            {
                if (oAlipay != null)
                {
                    this.Enabled = false;
                    oReq.MerchantID = tW_MerchantID;
                    oReq.InvoiceNO = cVB.tVB_RefDocNo;
                    oReq.RefundID = cVB.tVB_DocNo;
                    oReq.RefundTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    oReq.RefundAmt = (double)cVB.cVB_Amount;
                    oReq.Currency = tW_Currency.ToLower();
                    oReq.Reason = tW_AliReason;
                    oReq.TerminalID = cVB.tVB_PosCode;
                    oReq.BranchID = cVB.tVB_BchCode;
                    oReq.StoreID = tW_AliStoreID;
                    oReq.Language = tW_AliLng;

                    oAlipay.PaymentURL = tW_Url + "/ada_refund";
                    oAlipay.XKey = tW_XKey;
                    oAlipay.TimeOut = nW_Timeout * 1000;
                    oRes = oAlipay.C_REFoRequest(oReq);
                    if (oRes == null)
                    { }
                    else
                    {
                        tW_AliStatus = oRes.Result_Code;
                        tW_AliErrCode = oRes.Error_Code;
                        tW_AliErrDesc = oRes.Error_Desc;
                        tW_AliDateTime = oRes.ADATransTime;
                        tW_AliTransID = oRes.ADATransID;
                    }
                    W_PRCxKeepLog("ada_refund");
                    olaDescription.Text = "STATUS : " + tW_AliStatus;
                    if (tW_AliStatus.ToUpper() == "FAILED") olaDescription.Text += Environment.NewLine + "ErrCode : " + tW_AliErrCode;
                    switch (tW_AliStatus.ToUpper())
                    {
                        case "SUCCESS":
                            ocmAccept_Click(ocmAccept, null);
                            break;
                        case "PENDING":
                        case "SYSTEM_ERR":
                        case "FAILED":
                            if (tW_AliErrDesc != "")
                            {
                                tMsg = tW_AliErrDesc;
                                new cSP().SP_SHWxMsg(tMsg, 2);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(tW_AliErrCode))
                                {
                                    tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                    new cSP().SP_SHWxMsg(tMsg, 2);
                                }
                                else
                                {
                                    tMsg = new cDatabase().C_GEToDataQuery(string.Format(tW_SqlGetMsg, "Refund", tW_AliErrCode, cVB.nVB_Language)).Rows[0].Field<string>("FTErrDesc").ToString();  // get message from TSysMsgAlipay
                                    if (string.IsNullOrEmpty(tMsg))
                                    {
                                        tMsg = oW_Resource.GetString("tMsgAliPayNotPay");
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                    else
                                    {
                                        new cSP().SP_SHWxMsg(tMsg, 2);
                                    }
                                }
                            }
                            this.Close();
                            this.Dispose();
                            break;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_PRCxRefund " + oEx.Message); }
            finally
            {
                oAlipay = null;
                oReq = null;
                oRes = null;
            }
        }

        #endregion Function

        #region Event Form
        /// <summary>
        /// Paint Background
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                using (SolidBrush oBrush = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(oBrush, e.ClipRectangle);
                    oBrush.Dispose();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "OnPaintBackground " + oEx.Message); }
        }

        /// <summary>
        /// Close Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "ocmBack_Click : " + oEx.Message); }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                cPayment.tC_AliPaymentCode = otbPaymentCode.Text;
                cPayment.tC_AliTransID = tW_AliTransID;

                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "ocmAccept_Click : " + oEx.Message); }
        }
        private void otbPaymentCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    W_PRCxPayment();
                    nW_TimeQryCnt = 0;
                    nW_TimeQrySum = 0;
                    nW_TimeRevRnd = 0;
                    nW_TimeRevCnt = 0;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "otbPaymentCode_KeyDown : " + oEx.Message); }
        }

        private void otmReverse_Tick(object sender, EventArgs e)
        {
            nW_TimeRevCnt += 1;
            if (nW_TimeRevCnt >= nW_TimeQuery)
            {
                W_PRCxReverse();
                nW_TimeRevCnt = 0;
            }
        }
        private void otmQry_Tick(object sender, EventArgs e)
        {
            nW_TimeQryCnt += 1;
            if (nW_TimeQryCnt >= nW_TimeQuery)
            {
                W_PRCxQuery();
                nW_TimeQrySum += nW_TimeQryCnt;
                nW_TimeQryCnt = 0;
            }
            if (nW_TimeQrySum == nW_Timeout)
            {
                otmQry.Enabled = false;
                W_PRCxReverse();
            }
        }
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            new cFunctionKeyboard().C_KBDxKeyboard();
        }

        #endregion Event Form

        private void wAlipay_Shown(object sender, EventArgs e)
        {
            try
            {
                otbPaymentCode.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "wAlipay_Shown : " + oEx.Message); }
        }
    }
}
