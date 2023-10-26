using AdaPos.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdaPaymentSvc.xClass;
using AdaPaymentSvc.xModel.PromptPay;
using System.Resources;
using AdaPos.Resources_String.Local;

namespace AdaPos.Popup.wPayment
{
    public partial class wPromptPay : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private string tW_Url;
        private string tW_XKey;
        private int nW_TimeQuery;
        private int nW_Timeout;
        private string tW_MerchantID;
        private string tW_BillerfID;
        private string tW_Prefix;
        private string tW_Suffix;
        private int nW_Mode;
        private int nW_Tag;
        private string tW_MerchantRef;
        private string tW_URLGenQR;
        
        private int nW_TimeQrySum;
        private int nW_TimeQryCnt;
        private int nW_TimeRevCnt;
        private int nW_TimeRevRnd;

        const string tW_SqlGetMsg = "SELECT FTErrDesc FROM TSysMsgAlipay_L WITH(NOLOCK) WHERE FTErrFunc = '{0}' AND FTErrCode ='{1}' AND FNLngID = {2}";
        #endregion End Variable

        public wPromptPay()
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();

                W_SETxDesign();
                W_SETxText();
                W_GETxConfigPay();
                W_GETxBnkToCombo();
                W_CRTxQRCode();
                otmQry.Enabled = true;
                opnHD.Enabled = false;  //*Em 62-09-14
                olaCntDwn.Visible = true; //*Em 62-09-14

                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(oW_Resource.GetString("tTitleAmt"), olaAmt.Text);
                    cVB.oVB_CstScreen.W_SETxShowQRPromptPay(olaScanQR.Text, olaCntDwn.Text, opbQR.Image, opbForm.Image, false);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "wPromptPay " + oEx.Message); }
        }

        #region Funciton
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "W_SETxDesign : " + oEx.Message); }
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

                olaTitlePromptPay.Text = oW_Resource.GetString("tTitlePromptPay");
                olaTitleNumber.Text = oW_Resource.GetString("tTitleNo"); //*Net 63-02-21
                olaTitleBank.Text = oW_Resource.GetString("tTitleBank"); //*Net 63-02-21
                olaScanQR.Text = oW_Resource.GetString("tScanQR");
                otbTransID.Text = "";
                ocmBack.Text = "";
                olaDescription.Text = "";
                cVB.cVB_Amount = cVB.cVB_Amount - cVB.cVB_RoundDiff;    //*Em 62-10-08
                olaAmt.Text = new cSP().SP_SETtDecShwSve(1, cVB.cVB_Amount, cVB.nVB_DecShow);
                olaDescription.Visible = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "W_SETxText : " + oEx.Message); }
        }

        private void W_GETxConfigPay()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            try
            {
                //oSql.AppendLine("SELECT FNSysSeq,FTSysKey,FTSysStaUsrValue,FTSysStaUsrRef ");
                //oSql.AppendLine("FROM TSysRcvConfig WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTFmtCode = '013'");

                //*Em 63-08-06
                oSql.AppendLine("SELECT FTSysSeq,FTSysKey,FTSysStaUsrValue,FTSysStaUsrRef ");
                oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSysCode = 'PPromptPay'");
                //+++++++++++++
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null)
                {
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        switch (oRow["FTSysKey"].ToString().ToLower())
                        {
                            case "url":
                                tW_Url = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "x-key":
                                tW_XKey = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "timequery"://TimeQuery
                                nW_TimeQuery = Convert.ToInt32(oRow["FTSysStaUsrValue"]);
                                break;
                            case "timeout"://Timeout
                                nW_Timeout = Convert.ToInt32(oRow["FTSysStaUsrValue"]);
                                break;
                            case "merchantid"://MerchantID
                                tW_MerchantID = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "billerid"://BillerID
                                tW_BillerfID = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "prefix"://Prefix
                                tW_Prefix = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "suffix"://Suffix
                                tW_Suffix = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "mode"://Mode
                                nW_Mode = Convert.ToInt32(oRow["FTSysStaUsrValue"]);
                                break;
                            case "tag"://Tag
                                nW_Tag = Convert.ToInt32( oRow["FTSysStaUsrValue"]);
                                break;
                            case "merchantref"://MerchantRef
                                tW_MerchantRef = oRow["FTSysStaUsrValue"].ToString();
                                break;
                            case "urlgenqr"://URLGenQR
                                tW_URLGenQR = oRow["FTSysStaUsrValue"].ToString();
                                break;
                        }
                    }
                }
                else
                {
                    tW_MerchantID = "";
                    tW_Url = "";
                    tW_XKey = "";
                    nW_Timeout = 60;
                    nW_TimeQuery = 5;
                    tW_BillerfID = "";
                    tW_Prefix = "";
                    tW_Suffix = "";
                    nW_Mode = 1;
                    nW_Tag = 1;
                    tW_MerchantRef = "";
                    tW_URLGenQR = "";
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAlipay", "W_GETxConfigPay " + oEx.Message); }
        }

        private void W_GETxBnkToCombo()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT BNK.FTBnkCode,CASE WHEN ISNULL(BNKL.FTBnkName,'') = '' THEN (SELECT TOP 1 FTBnkName FROM TFNMBank_L with(nolock) WHERE FTBnkCode = BNK.FTBnkCode) ELSE BNKL.FTBnkName END AS FTBnkName");
                oSql.AppendLine("FROM TFNMBank BNK with(nolock)");
                oSql.AppendLine("LEFT JOIN TFNMBank_L BNKL with(nolock) ON BNK.FTBnkCode = BNKL.FTBnkCode AND BNKL.FNLngID = " + cVB.nVB_Language);
                DataTable oDT = oDB.C_GEToDataQuery(oSql.ToString());
                ocbSelectBank.Items.Clear();
                ocbSelectBank.DataSource = oDT;
                ocbSelectBank.DisplayMember = "FTBnkName";
                ocbSelectBank.ValueMember = "FTBnkCode";

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCreditCard", "W_GETxBnkToCombo : " + oEx.Message); }
            finally
            {
                oDB = null;

            }

        }
        private void W_PRCxQRQuery()
        {
            cPromptPPay oPrompt = new cPromptPPay();
            cmlPPYQryReq oReq = new cmlPPYQryReq();
            cmlPPYQryRes oRes = new cmlPPYQryRes();
            string tPaySta = "";
            string tDocNo = "";
            try
            {
                if (oPrompt != null)
                {
                    if (cSale.tC_DocFmtSep.Length == 0)
                    {
                        tDocNo = cVB.tVB_DocNo.Substring(0, cVB.tVB_DocNo.Length - cSale.nC_DocRuningLength) + "-" + cVB.tVB_DocNo.Substring(cVB.tVB_DocNo.Length - cSale.nC_DocRuningLength);
                    }
                    else
                    {
                        tDocNo = cVB.tVB_DocNo;
                    }
                    oReq.MerchantID = tW_MerchantID;
                    oReq.InvoiceID = tDocNo;
                    oReq.InvoiceDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                    oReq.TerminalID = cVB.tVB_PosCode;
                    oReq.BranchID = cVB.tVB_BchCode;
                    oReq.StoreID = cVB.tVB_BchCode;
                    oReq.Prefix = tW_Prefix;
                    oReq.InvoiceAmt = cVB.cVB_Amount.ToString("###,###,###.00");
                    oReq.MerchantRef = tW_MerchantRef;

                    oPrompt.XKey = tW_XKey;
                    oPrompt.PaymentURL = tW_Url + "/v2/ADA_Query";
                    oPrompt.TimeOut = nW_Timeout * 1000;
                    oRes = oPrompt.C_QRYoRequest(oReq);

                    if (oRes != null)
                    {
                        tPaySta = oRes.PaymentSta;
                        switch (tPaySta.ToUpper())
                        {
                            case "SUCCESS":
                                otmQry.Enabled = false;
                                olaDescription.Text = "Payment Status : " + tPaySta;

                                opnHD.Enabled = true;  //*Em 62-09-14
                                cPayment.tC_XrcRef1 = oRes.TransID;
                                cPayment.tC_XrcRef2 = oRes.ADATransID + ";" + oRes.TransID + ";" + oRes.Sending_Bnk + ";" + oRes.Rcv_Bnk;
                                ocmAccept_Click(null,null);
                                break;
                            case "CLOSED":
                                otmQry.Enabled = false;
                                olaDescription.Text = "Payment Status : " + tPaySta;
                                //Unload wCNQRScan;
                                olaDescription.Visible = false;
                                break;
                            case "FAILED":
                                olaDescription.Text = "Payment Status : PENDING";
                                break;
                            default:
                                olaDescription.Text = "Payment Status : " + tPaySta;
                                break;
                        }
                        if (tPaySta.ToUpper() == "UNKNOWN")
                        {
                            olaDescription.Text = "Payment Status : " + tPaySta;
                        }
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "W_PRCxQRQuery : " + oEx.Message); }
            finally
            {
                oPrompt = null;
                oReq = null;
                oRes = null;
            }
        }

        private void W_CRTxQRCode()
        {
            cPromptPPay oPromptPay = new cPromptPPay();
            cmlPPYQRTag oTag = new cmlPPYQRTag();
            
            try
            {
                if (oPromptPay != null)
                {
                    oPromptPay.QR_Height = 200;
                    oPromptPay.QR_Width = 200;

                    switch (nW_Mode)
                    {
                        case 1:
                            oPromptPay.PPY_TYPE = cCN.PPY_TYPE.TaxID;
                            break;
                        case 2:
                            oPromptPay.PPY_TYPE = cCN.PPY_TYPE.eWallet;
                            break;
                        case 3:
                            oPromptPay.PPY_TYPE = cCN.PPY_TYPE.BnkAccNo;
                            break;
                    }
                    switch (nW_Tag)
                    {
                        case 1:
                            oPromptPay.TAG_MODE = cCN.TAG_MODE.TAG30;
                            break;
                        case 2:
                            oPromptPay.TAG_MODE = cCN.TAG_MODE.TAG31;
                            break;
                    }

                    oTag.POS_NO = cVB.tVB_PosCode;
                    oTag.Prefix = tW_Prefix;
                    oTag.PromptID = tW_BillerfID;
                    oTag.Suffix = tW_Suffix;
                    oTag.STORE_NO = cVB.tVB_BchCode;
                    oTag.TRANSAC_NO = cVB.tVB_DocNo.Substring(cVB.tVB_DocNo.Length - cSale.nC_DocRuningLength);
                    //oTag.TRANSAC_AMT =  new cSP().SP_SETtDecShwSve(2,cVB.cVB_Amount, cVB.nVB_DecShow);
                    //oTag.TRANSAC_AMT = new cSP().SP_SETtDecShwSve(2, cVB.cVB_Amount, cVB.nVB_DecSave); //*Net 63-06-23 ใช้ Sav  //*Arm 63-09-01 Comment Code
                    oTag.TRANSAC_AMT = new cSP().SP_SETtDecShwSve(2, cVB.cVB_Amount, 2); //*Arm 63-09-01 ปัดเศษทศนิยม 2 ตำแหน่ง
                    oTag.MctRef = tW_MerchantRef;
                    oTag.TRANSAC_HD =cVB.tVB_DocNo.Substring(0, cVB.tVB_DocNo.Length - cSale.tC_DocFmtSep.Length - cSale.nC_DocRuningLength);

                    opbQR.Image = oPromptPay.C_GEToQRPic(oTag);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "W_CRTxQRCode : " + oEx.Message); }
            finally
            {

            }
        }
        #endregion Function

        #region Method
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "OnPaintBackground " + oEx.Message); }
        }
        private void otmQry_Tick(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                nW_TimeQryCnt = nW_TimeQryCnt + 1;
                olaCntDwn.Text = Convert.ToString(nW_Timeout - nW_TimeQrySum - nW_TimeQryCnt); //*Em 62-09-14

                //*Net 63-08-07 อัพเดต CountDown
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxShowQRPromptPay(olaScanQR.Text, olaCntDwn.Text, null, null, true);
                }


                if (nW_TimeQryCnt >= nW_TimeQuery)
                {
                    this.Enabled = false;
                    W_PRCxQRQuery();
                    nW_TimeQrySum = nW_TimeQrySum + nW_TimeQryCnt;
                    nW_TimeQryCnt = 0;
                }

                if (nW_TimeQrySum == nW_Timeout)
                {
                    //*Net 63-08-07 ถ้าหมดเวลาให้ปิด QR
                    if (cVB.oVB_CstScreen != null)
                    {
                        cVB.oVB_CstScreen.W_SETxHideQRPromptPay();
                    }

                    otmQry.Enabled = false;
                    //Unload wCNQRScan
                    olaDescription.Visible = false;
                    this.Enabled = true;
                    opnHD.Enabled = true;  //*Em 62-09-14
                    olaCntDwn.Visible = false; //*Em 62-09-14
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "otmQry_Tick : " + oEx.Message); }
        }
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (olaDescription.Visible == false)
                {
                    if (otbTransID.Text.Trim() == "" || ocbSelectBank.Text.Trim() == "")
                    {
                        return;
                    }
                    cPayment.tC_XrcRef1 = otbTransID.Text.Trim();
                    cPayment.tC_BnkCode = ocbSelectBank.SelectedValue.ToString();
                    cPayment.tC_BnkName = ocbSelectBank.Text;
                }

                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "ocmAccept_Click : " + oEx.Message); }
        }
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPromptPay", "ocmBack_Click : " + oEx.Message); }
        }
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            new cFunctionKeyboard().C_KBDxKeyboard();
        }

        //*Net 63-08-04 ปิดหน้า QR ที่จอ 2
        private void wPromptPay_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (cVB.oVB_CstScreen != null)
            {
                cVB.oVB_CstScreen.W_SETxHideQRPromptPay();
            }
        }
        #endregion Method

    }
}
