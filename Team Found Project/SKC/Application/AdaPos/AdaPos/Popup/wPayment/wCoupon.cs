using AdaPos.Class;
using AdaPos.Models.RabbitMQ;
using AdaPos.Popup.All;
using AdaPos.Popup.wSale;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wPayment
{
    public partial class wCoupon : Form
    {
        private int nW_Mode;    // 1:คูปองเงินสด 2:คูปองส่วนลด
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private string tW_CouponType = "";
        private bool bW_ChkCoupon = false;
        private int nW_StaChkHQ = 2;    //1:HQ 2:Branch
        private bool bW_CouponVerify = false;
        private cmlCoupon oW_Coupon;
        public decimal cW_B4DisChg = 0;
        private string tW_DisChgTxt = "";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnMode">1:คูปองเงินสด 2:คูปองส่วนลด</param>
        public wCoupon(int pnMode)
        {
            InitializeComponent();
            try
            {
                nW_Mode = pnMode;
                oW_SP = new cSP();
                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "wCoupon : " + oEx.Message); }
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
                ocmSchType.BackColor = cVB.oVB_ColDark;

                otbCouponType.Enabled = false;
                opbWait.Visible = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "W_SETxDesign : " + oEx.Message); }
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

                switch (nW_Mode)
                {
                    case 1: // 1:คูปองเงินสด
                        olaTitleCoupon.Text = oW_Resource.GetString("tCashCoupon");
                        break;
                    case 2: // 2:คูปองส่วนลด
                        olaTitleCoupon.Text = oW_Resource.GetString("tDiscountCoupon");
                        break;
                }

                
                olaTitleType.Text = oW_Resource.GetString("tType");
                olaTitleNumber.Text = oW_Resource.GetString("tCouponNo");
                otbCouponType.Text = "";
                otbCouponNo.Text = "";

                //*Em 62-12-23
                olaTitleBal.Text = oW_Resource.GetString("tTitleBalance");
                olaTitleAvai.Text = oW_Resource.GetString("tTitleAvailable");
                olaTitleDate.Text = oW_Resource.GetString("tUseableDate");
                olaTitleTime.Text = oW_Resource.GetString("tUseableTime");
                olaTitleCond.Text = oW_Resource.GetString("tDiscCond");
                olaTitleValue.Text = oW_Resource.GetString("tValue");

                olaBalance.Text = oW_SP.SP_SETtDecShwSve(1, cVB.oVB_Payment.cW_AmtTotalShw, cVB.nVB_DecShow);
                switch (nW_Mode)
                {
                    case 1:
                        olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cVB.oVB_Payment.cW_AmtTotalShw, cVB.nVB_DecShow);
                        break;
                    case 2:
                        olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cW_B4DisChg, cVB.nVB_DecShow);
                        break;
                }
                
                olaCpnName.Text = "";
                olaDate.Text = "";
                olaTime.Text = "";
                olaCondition.Text = "";
                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                opnR.Enabled = false;
                ogbDetail1.Visible = false;
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "W_SETxText : " + oEx.Message); }
        }

        private void W_CHKxCoupon()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();

            try
            {
                string tUrl = "";

                if (nW_StaChkHQ == 1)
                {
                    tUrl = cVB.tVB_API2FNWalletHQ;
                }
                else
                {
                    tUrl = cVB.tVB_API2FNWallet;
                }

                if (string.IsNullOrEmpty(tUrl))
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlWalletNotDefine"), 3);
                    return;
                }
                else
                {
                    this.UseWaitCursor = true;
                    cmlReqCoupon oReqCoupon = new cmlReqCoupon();
                    oReqCoupon.ptCouponType = Convert.ToString(nW_Mode);
                    oReqCoupon.ptBarCpn = otbCouponNo.Text.Trim();
                    oReqCoupon.ptBranch = cVB.tVB_BchCode;
                    oReqCoupon.ptPriceGroup = cVB.tVB_PriceGroup;
                    oReqCoupon.ptMerchant = cVB.tVB_Merchart;
                    oReqCoupon.pnLangID = cVB.nVB_Language;
                    string tJSonCall = JsonConvert.SerializeObject(oReqCoupon);
                    cClientService oCall = new cClientService();
                    oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);
                    HttpResponseMessage oRep = new HttpResponseMessage();
                    try
                    {
                        oRep = oCall.C_POSToInvoke(tUrl + "/Coupon/CheckCoupon", tJSonCall);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("wCoupon", "W_CHKxCoupon : " + oEx.Message);
                        return;
                    }

                    if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                        cmlResCoupon oRes = JsonConvert.DeserializeObject<cmlResCoupon>(tJSonRes);
                        if (oRes.rtCode == "1")
                        {
                            if (oRes.raoCoupon.Count > 1)
                            {
                                wListCoupon oList = new wListCoupon(oRes.raoCoupon, otbCouponNo.Text);
                                oList.ShowDialog();
                                if (oList.bW_Select)
                                {
                                    oW_Coupon = oList.oW_Conpon;
                                    oList = null;
                                }
                                else
                                {
                                    oList = null;
                                    this.UseWaitCursor = false;
                                    return;
                                }
                            }
                            else
                            {
                                oW_Coupon = oRes.raoCoupon[0];
                            }
                            W_SHWxCouponDetail();
                            bW_CouponVerify = true;
                        }
                        else
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 3);
                            new cLog().C_WRTxLog("wCoupon", "W_CHKxCoupon : " + oRes.rtDesc);
                            bW_CouponVerify = false;
                        }
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "W_CHKxCoupon : " + oEx.Message); }
            finally
            {
                this.UseWaitCursor = false;
            }
        }

        private bool W_PRCbCoupon()
        {
            string tQueueReq = "";
            string tQueueRes = "";
            try
            {
                

                

                if (nW_StaChkHQ == 1)
                {
                    tQueueReq = "FN_PayReqCoupon" + cVB.tVB_HQBchCode;
                    if (string.IsNullOrEmpty(cVB.tVB_HQMQHost)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_HQMQUsr)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_HQMQPwd)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_HQMQVirtual)) return false; //*Em 62-09-03

                    cVB.oVB_RabbitMQConfig.tHostName = cVB.tVB_HQMQHost;
                    cVB.oVB_RabbitMQConfig.tUserName = cVB.tVB_HQMQUsr;
                    cVB.oVB_RabbitMQConfig.tPassword = cVB.tVB_HQMQPwd;
                    cVB.oVB_RabbitMQConfig.tVirtual = cVB.tVB_HQMQVirtual;
                }
                else
                {
                    tQueueReq = "FN_PayReqCoupon" + cVB.tVB_BchCode;
                    if (string.IsNullOrEmpty(cVB.tVB_BCHMQHost)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_BCHMQUsr)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_BCHMQPwd)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_BCHMQVirtual)) return false; //*Em 62-09-03

                    cVB.oVB_RabbitMQConfig.tHostName = cVB.tVB_BCHMQHost;
                    cVB.oVB_RabbitMQConfig.tUserName = cVB.tVB_BCHMQUsr;
                    cVB.oVB_RabbitMQConfig.tPassword = cVB.tVB_BCHMQPwd;
                    cVB.oVB_RabbitMQConfig.tVirtual = cVB.tVB_BCHMQVirtual;
                }

                cVB.oVB_MQFactory = new ConnectionFactory();
                cVB.oVB_MQFactory.HostName = cVB.oVB_RabbitMQConfig.tHostName;
                cVB.oVB_MQFactory.UserName = cVB.oVB_RabbitMQConfig.tUserName;
                cVB.oVB_MQFactory.Password = cVB.oVB_RabbitMQConfig.tPassword;
                cVB.oVB_MQFactory.VirtualHost = cVB.oVB_RabbitMQConfig.tVirtual;

                cVB.oVB_MQConn = cVB.oVB_MQFactory.CreateConnection();
                cVB.oVB_MQModel = cVB.oVB_MQConn.CreateModel();

                cVB.oVB_MQModel.QueueDeclare(tQueueReq, false, false, false, null);

                cmlReqFunc oReq = new cmlReqFunc();
                cmlReqData oReqData = new cmlReqData();
                oReq.ptFucntion = "PayCoupon";
                oReq.ptSource = "AdaPos";
                oReq.ptSource = "MQReceivePrc";

                oReqData.ptCphDocNo = oW_Coupon.rtCphDocNo;
                oReqData.ptCpdBarCpn = otbCouponNo.Text.Trim();
                oReqData.ptCpdSeqNo = Convert.ToString(oW_Coupon.rnCpdSeqNo);
                oReqData.ptSaleDocNo = cVB.tVB_DocNo;
                oReqData.ptCpbFrmBch = cVB.tVB_BchCode;
                oReqData.ptCpbFrmPos = cVB.tVB_PosCode;
                oReq.ptData = JsonConvert.SerializeObject(oReqData);

                string tMsg = JsonConvert.SerializeObject(oReq);
                var oBody = Encoding.UTF8.GetBytes(tMsg);
                cVB.oVB_MQModel.BasicPublish("", tQueueReq, false, null, oBody);
                cVB.oVB_MQModel.Close();
                cVB.oVB_MQConn.Close();
                cVB.oVB_MQFactory = null;

                tQueueRes = "FN_PayRetCoupon" + cVB.tVB_BchCode + cVB.tVB_PosCode;
                cVB.oVB_Coupon = new cRabbitMQ(tQueueRes, "");
                cVB.oVB_Coupon.oEv_Jump += new EventHandler(oVB_Coupon_Receive);
                return true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "W_PRCxCoupon : " + oEx.Message); return false; }
        }

        private void W_GETxConsumerReceive(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                string tMsgMQ = Encoding.UTF8.GetString(e.Body);
                cmlResFunc oRes = JsonConvert.DeserializeObject<cmlResFunc>(tMsgMQ);
                cmlResData oResData = JsonConvert.DeserializeObject<cmlResData>(oRes.ptData);
                cVB.oVB_MQModel.BasicAck(e.DeliveryTag, false);

                if (oResData.rtCode == "200")
                {
                    cPayment.tC_XrcRef1 = otbCouponNo.Text;
                    cPayment.tC_XrcRef2 = tW_CouponType;
                }
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCoupon", "W_GETxConsumerReceive : " + oEx.Message);
            }
        }

        private void W_SHWxCouponDetail()
        {
            decimal cAmount = 0;
            try
            {
                ogbDetail1.Visible = true;
                olaCpnName.Text = oW_Coupon.rtCpnName;
                olaDate.Text = string.Format("{0:dd/MM/yyyy}", oW_Coupon.rdCphDateStart) + " - " + string.Format("{0:dd/MM/yyyy}", oW_Coupon.rdCphDateStop);
                olaTime.Text =  oW_Coupon.rtCphTimeStart + " - " + oW_Coupon.rtCphTimeStop;
                
                switch (nW_Mode)
                {
                    case 1: //คูปองเงินสด
                        //olaValue.Text = oW_SP.SP_SETtDecShwSve(1, oW_Coupon.rcCphDisValue, cVB.nVB_DecShow);
                        //olaCondition.Text = oW_Resource.GetString("tCashCoupon") + " (" + oW_SP.SP_SETtDecShwSve(1, oW_Coupon.rcCphDisValue, cVB.nVB_DecShow) + ")";
                        switch (oW_Coupon.rtCphDisType)
                        {
                            case "1":   //ลดบาท
                                tW_DisChgTxt = Convert.ToString(oW_Coupon.rcCphDisValue);
                                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, oW_Coupon.rcCphDisValue, cVB.nVB_DecShow);
                                olaCondition.Text = oW_Resource.GetString("tCashCoupon") + " (" + oW_SP.SP_SETtDecShwSve(1, oW_Coupon.rcCphDisValue, cVB.nVB_DecShow) + ")"; ;
                                break;
                            case "2":   //ลด%
                                tW_DisChgTxt = Convert.ToString(oW_Coupon.rcCphDisValue) + "%";
                                cAmount = (cVB.oVB_Payment.cW_AmtTotalCal * oW_Coupon.rcCphDisValue) / 100;
                                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, cAmount, cVB.nVB_DecShow);
                                olaCondition.Text = oW_Resource.GetString("tCashCoupon") + " (" + oW_SP.SP_SETtDecShwSve(1, oW_Coupon.rcCphDisValue, cVB.nVB_DecShow) + "%)"; ;
                                break;
                        }
                        break;
                    case 2: //คูปองส่วนลด
                        switch (oW_Coupon.rtCphDisType)
                        {
                            case "1":   //ลดบาท
                                tW_DisChgTxt = Convert.ToString(oW_Coupon.rcCphDisValue);
                                olaValue.Text =  oW_SP.SP_SETtDecShwSve(1, oW_Coupon.rcCphDisValue, cVB.nVB_DecShow);
                                olaCondition.Text = oW_Resource.GetString("tDisAmt") + " (" + oW_SP.SP_SETtDecShwSve(1, oW_Coupon.rcCphDisValue, cVB.nVB_DecShow) + ")"; ;
                                break;
                            case "2":   //ลด%
                                tW_DisChgTxt = Convert.ToString(oW_Coupon.rcCphDisValue) + "%";
                                cAmount = (cW_B4DisChg * oW_Coupon.rcCphDisValue) / 100;
                                olaValue.Text =  oW_SP.SP_SETtDecShwSve(1, cAmount, cVB.nVB_DecShow);
                                olaCondition.Text = oW_Resource.GetString("tDisPer") + " (" + oW_SP.SP_SETtDecShwSve(1, oW_Coupon.rcCphDisValue, cVB.nVB_DecShow) + "%)"; ;
                                break;
                        }
                        break;
                }
                otbCouponNo.Enabled = false;
                ocmVerify.Enabled = false;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCoupon", "W_SHWxCouponDetail : " + oEx.Message);
            }
        }
        #endregion End Function

        #region Method/Events
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                cPayment.tC_XrcRef1 = "";
                cPayment.tC_XrcRef2 = "";
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "ocmBack_Click : " + oEx.Message); }
        }
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            string tMsg = "";
            try
            {
                if (string.IsNullOrEmpty(tW_CouponType))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgSelCoupon"), 3);
                    return;
                }
                if (string.IsNullOrEmpty(otbCouponNo.Text))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgInputCoupon"), 3);
                    otbCouponNo.Focus();
                    return;
                }

                if (nW_Mode == 2)
                {
                    if ((decimal)(cVB.oVB_Payment.cW_AmtTotalCal - Convert.ToDecimal(olaValue.Text)) < (decimal)cVB.cVB_SmallBill)
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                        return;
                    }
                }

                if (nW_Mode == 1)
                {
                    if (Convert.ToDecimal(olaValue.Text) > Convert.ToDecimal(cVB.oVB_Payment.cW_AmtTotalCal))
                    {
                        tMsg = cVB.oVB_GBResource.GetString("tMsgPayMost");
                        tMsg += Environment.NewLine + cVB.oVB_GBResource.GetString("tMsgPayConfirm");
                        if (new cSP().SP_SHWoMsg(tMsg, 1) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                if (bW_ChkCoupon)
                {
                    if (bW_CouponVerify == false) return;
                    opnPage.Enabled = false;
                    opbWait.Visible = true;
                    cPayment.tC_XrcRef1 = "";
                    cPayment.tC_XrcRef2 = "";
                    if (W_PRCbCoupon() == false)
                    {
                        opbWait.Visible = false;
                        opnPage.Enabled = true;
                        return;
                    }

                    return;
                }

                cPayment.tC_XrcRef1 = otbCouponNo.Text;
                cPayment.tC_XrcRef2 = tW_CouponType;
                cVB.cVB_Amount = Convert.ToDecimal(olaValue.Text);
                if (nW_Mode == 2) cVB.oVB_Payment.W_ADDxDisChgBill("5", olaValue.Text, cVB.cVB_Amount);
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "ocmAccept_Click : " + oEx.Message); }
        }

        private void ocmSchType_Click(object sender, EventArgs e)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            DataTable odtTmp = new DataTable();
            try
            {
                wSearch2Column oSchCpn = new wSearch2Column("TFNMCouponType");
                oSchCpn.ShowDialog();
                if (oSchCpn.oW_DataSearch != null)
                {
                    otbCouponType.Text = oSchCpn.oW_DataSearch.tName;
                    tW_CouponType = oSchCpn.oW_DataSearch.tCode;

                    //ตรวจสอบว่า ต้องตรวจสอบคูปองหรือไม่
                    oSql.AppendLine("SELECT TOP 1 ISNULL(FTCptStaChk,'2') AS FTCptStaChk");
                    oSql.AppendLine(",ISNULL(FTCptStaChkHQ,'2') AS FTCptStaChkHQ");
                    oSql.AppendLine("FROM TFNMCouponType WITH(NOLOCK)");
                    oSql.AppendLine(" WHERE FTCptCode ='" + tW_CouponType + "'");
                    odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                    if (odtTmp != null)
                    {
                        bW_ChkCoupon = oDB.C_GEToDataQuery<string>(oSql.ToString()) == "1" ? true : false;
                        nW_StaChkHQ = Convert.ToInt32(odtTmp.Rows[0].Field<string>("FTCptStaChkHQ"));
                    }

                    //*Em 61-12-23
                    opnR.Enabled = true;
                    otbCouponNo.Focus();

                    if (bW_ChkCoupon)
                    { ocmVerify.Enabled = true; }
                    else
                    { ocmVerify.Enabled = false; }
                    //+++++++++++++
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "ocmSchType_Click : " + oEx.Message); }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            new cFunctionKeyboard().C_KBDxKeyboard();
        }

        private void otbCouponNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter) 
                {
                    if (string.IsNullOrEmpty(otbCouponNo.Text)) return;
                    if (bW_ChkCoupon)
                    {
                        W_CHKxCoupon();
                    }
                    else
                    {
                        ocmAccept.Focus();
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "otbCouponNo_KeyPress : " + oEx.Message); }
        }

        private void ocmVerify_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(otbCouponNo.Text)) return;
            if (bW_ChkCoupon)
            {
                W_CHKxCoupon();
            }
            else
            {
                ocmAccept.Focus();
            }
        }

        private void wCoupon_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
               this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "wCoupon_FormClosing : " + oEx.Message); } 
        }

        private void oVB_Coupon_Receive(object sender, EventArgs e)
        {
            try
            {
                cVB.oVB_Coupon.C_DISxDisConnect();
                cVB.oVB_Coupon = null;
                
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        opbWait.Visible = false;
                        opnDetail.Enabled = true;

                        if (cPayment.tC_XrcRef1 == "OK")
                        {
                            cPayment.tC_XrcRef1 = otbCouponNo.Text;
                            cPayment.tC_XrcRef2 = tW_CouponType;
                            cVB.cVB_Amount = Convert.ToDecimal(olaValue.Text);
                            if (nW_Mode == 2)
                            {
                                switch (oW_Coupon.rtCphDisType)
                                {
                                    case "1":
                                        cVB.oVB_Payment.W_ADDxDisChgBill("5", olaValue.Text, cVB.cVB_Amount);
                                        cVB.tVB_DisChgTxt = Convert.ToDouble(oW_Coupon.rcCphDisValue).ToString();
                                        break;
                                    case "2":
                                        cVB.oVB_Payment.W_ADDxDisChgBill("5",Convert.ToString(oW_Coupon.rcCphDisValue) + "%", cVB.cVB_Amount);
                                        cVB.tVB_DisChgTxt = Convert.ToDouble(oW_Coupon.rcCphDisValue).ToString() + "%";
                                        break;
                                }
                            }
                                
                            this.Close();
                        }
                        else
                        {
                            opnPage.Enabled = true;
                            opbWait.Visible = false;
                            oW_SP.SP_SHWxMsg(cPayment.tC_XrcRef2,2);
                        }
                    }));
                }
                else
                {
                    opbWait.Visible = false;
                    opnDetail.Enabled = true;
                    if (cPayment.tC_XrcRef1 == "OK")
                    {
                        cPayment.tC_XrcRef1 = otbCouponNo.Text;
                        cPayment.tC_XrcRef2 = tW_CouponType;
                        cVB.cVB_Amount = Convert.ToDecimal(olaValue.Text);
                        if (nW_Mode == 2)
                        {
                            switch (oW_Coupon.rtCphDisType)
                            {
                                case "1":
                                    cVB.oVB_Payment.W_ADDxDisChgBill("5", Convert.ToString(oW_Coupon.rcCphDisValue), cVB.cVB_Amount);
                                    cVB.tVB_DisChgTxt = Convert.ToDouble(oW_Coupon.rcCphDisValue).ToString();
                                    break;
                                case "2":
                                    cVB.oVB_Payment.W_ADDxDisChgBill("5", Convert.ToString(oW_Coupon.rcCphDisValue) + "%", cVB.cVB_Amount);
                                    cVB.tVB_DisChgTxt = Convert.ToDouble(oW_Coupon.rcCphDisValue).ToString()+ "%";
                                    break;
                            }
                        }
                        
                        this.Close();
                    }
                    else
                    {
                        opnPage.Enabled = true;
                        opbWait.Visible = false;
                        oW_SP.SP_SHWxMsg(cPayment.tC_XrcRef2, 2);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "oVB_Coupon_Receive : " + oEx.Message); }
        }

        private void otbCouponNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(otbCouponNo.Text)) return;
                if (bW_ChkCoupon)
                {
                }
                else
                {
                    wEnterAmount oFrm = new wEnterAmount();
                    oFrm.ShowDialog();
                    if (!String.IsNullOrEmpty(oFrm.otbAmount.Text))
                    {
                        olaValue.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oFrm.otbAmount.Text), cVB.nVB_DecShow);
                    }
                    oFrm.Dispose();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "otbCouponNo_Leave : " + oEx.Message); }
        }

        private void wCoupon_Shown(object sender, EventArgs e)
        {
            try
            {
                switch (nW_Mode)
                {
                    case 1:
                        olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cVB.oVB_Payment.cW_AmtTotalShw, cVB.nVB_DecShow);
                        break;
                    case 2:
                        olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cW_B4DisChg, cVB.nVB_DecShow);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "otbCouponNo_Leave : " + oEx.Message); }
        }
    }
    #endregion End Method/Events
}
