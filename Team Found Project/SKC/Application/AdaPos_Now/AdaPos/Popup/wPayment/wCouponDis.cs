using AdaPos.Class;
using AdaPos.Models.DatabaseTmp;
using AdaPos.Models.Other;
using AdaPos.Models.RabbitMQ;
using AdaPos.Popup.All;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wPayment
{
    public partial class wCouponDis : Form
    {
        #region Variable
        private int nW_Mode;    // 1:คูปองเงินสด 2:คูปองส่วนลด
        private cSP oW_SP;
        private int nW_StaChkHQ = 1;    //1:HQ 2:Branch
        private bool bPublish=false;
        public decimal cW_AlwDisChg = 0;


        private cmlTPSTCouponHD_Tmp oW_SelectCpn;
        private DataTable oW_PdtUse;
        private ResourceManager oW_Resource;
        private wChooseCpn oW_ChooseCpn;
        private cmlReqData oW_ReqData;

        public List<cmlProrateDT> aoW_ProrateDT;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnMode">1:คูปองเงินสด 2:คูปองส่วนลด</param>
        public wCouponDis(int pnMode)
        {
            InitializeComponent();
            try
            {
                nW_Mode = pnMode;
                oW_SP = new cSP();
                aoW_ProrateDT = new List<cmlProrateDT>();
                oW_PdtUse = new DataTable();
                W_SETxDesign();
                W_SETxText();

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        #region Event
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCouponDis", "OnPaintBackground " + oEx.Message); }
        }
        /// <summary>
        /// Form Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCouponDis_Shown(object sender, EventArgs e)
        {
            try
            {
                switch (nW_Mode)
                {
                    case 1:
                        olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cVB.oVB_Payment.cW_AmtTotalShw, cVB.nVB_DecShow);
                        break;
                    case 2:
                        olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cW_AlwDisChg, cVB.nVB_DecShow);
                        break;
                }
                otbCouponNo.Focus();
                otbCouponNo.SelectAll();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        /// <summary>
        /// Leave Textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbCouponNo_Leave(object sender, EventArgs e)
        {
            try
            {
                otbCouponNo.Focus();
                otbCouponNo.SelectAll();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            string tMsg = "";
            decimal cAmt = 0m;
            try
            {
                if (oW_SelectCpn == null || String.IsNullOrEmpty(oW_SelectCpn.FTCphDocNo))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgInputCoupon"), 3);
                    otbCouponNo.Focus();
                    return;
                }
                cAmt = decimal.Parse(olaCpnVal.Text);

                if (nW_Mode == 2)
                {
                    if (aoW_ProrateDT.Count > 0)
                    {
                        cAmt = 0m;
                        foreach (cmlProrateDT oDT in aoW_ProrateDT)
                        {
                            cAmt += oDT.FCXsdNetAfHD;
                        }
                        if (cAmt <= 0)
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                            return;
                        }

                    }

                    cAmt = (cAmt < decimal.Parse(olaCpnVal.Text)) ? cAmt : decimal.Parse(olaCpnVal.Text);

                    if ((decimal)(cW_AlwDisChg - cAmt) < (decimal)cVB.cVB_SmallBill)
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                        return;
                    }
                }

                if (nW_Mode == 1)
                {
                    if (cAmt > Convert.ToDecimal(cVB.oVB_Payment.cW_AmtTotalCal))
                    {
                        tMsg = cVB.oVB_GBResource.GetString("tMsgPayMost");
                        tMsg += Environment.NewLine + cVB.oVB_GBResource.GetString("tMsgPayConfirm");
                        if (new cSP().SP_SHWoMsg(tMsg, 1) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                if (W_CHKbLimitPerBill(oW_SelectCpn))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCpnLimitPerBill"), 3);
                    otbCouponNo.Focus();
                    return;
                }

                if (W_CHKbOnTopPromotion(oW_SelectCpn))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoOnTopPmt"), 3);
                    otbCouponNo.Focus();
                    return;
                }

                /*if (!(oW_SelectCpn.FNCpdAlwMaxUse == 0 && oW_SelectCpn.FTStaChkMember == "1"))
                {*/
                //opnPage.Enabled = false;
                W_SETxFormLock(true);
                if (oW_SelectCpn.FNCpdAlwMaxUse == 0 && oW_SelectCpn.FTStaChkMember == "1")
                {
                    cPayment.tC_XrcRef1 = "OK";
                    oVB_Coupon_Receive(sender,e);

                    W_SETxFormLock(false);
                }
                else
                {
                    nW_StaChkHQ = Convert.ToInt32(oW_SelectCpn.FTCptStaChkHQ);
                    //opbWait.Visible = true;
                    cPayment.tC_XrcRef1 = "";
                    cPayment.tC_XrcRef2 = "";
                    bPublish = W_PRCbCoupon();
                    if (bPublish == false)
                    {
                        //opbWait.Visible = false;
                        //opnPage.Enabled = true;

                        W_SETxFormLock(false);
                        return;
                    }
                }
                //}

                //cPayment.tC_XrcRef1 = otbCouponNo.Text;
                //cPayment.tC_XrcRef2 = oW_SelectCpn.FTCptCode;
                //if (nW_Mode == 2) cVB.oVB_Payment.W_ADDxDisChgBill("5", olaCpnVal.Text, cVB.cVB_Amount);
                //this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCouponDis", "ocmAccept_Click : " + oEx.Message); }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            new cFunctionKeyboard().C_KBDxKeyboard();
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                cPayment.tC_XrcRef1 = "";
                cPayment.tC_XrcRef2 = "";
                if(bPublish)
                {
                   
                }
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            List<cmlTPSTCouponHD_Tmp> aoListCpn;
            cmlCouponDisDetail oCpnDisDetail;
            try
            {
                aoListCpn = new List<cmlTPSTCouponHD_Tmp>();
                oW_SelectCpn = new cmlTPSTCouponHD_Tmp();
                oCpnDisDetail = new cmlCouponDisDetail();

                aoListCpn = W_GEToLocal(otbCouponNo.Text);

                if (aoListCpn.Count == 0)
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                    return;
                }

                oW_SelectCpn = aoListCpn.FirstOrDefault();
                if (aoListCpn.Count > 1)
                {
                    //Choose Coupon
                    oW_ChooseCpn = new wChooseCpn(aoListCpn, nW_Mode);
                    oW_ChooseCpn.ShowDialog();
                    W_SETxText();
                    if (oW_ChooseCpn.bW_Select == false) return;
                    oW_SelectCpn = oW_ChooseCpn.oW_Conpon;
                }
                if (W_CHKbSalDT(oW_SelectCpn) == false)
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                    return;
                }

                aoW_ProrateDT = new List<cmlProrateDT>();
                foreach (DataRow oPdt in oW_PdtUse.Rows)
                {
                    cmlProrateDT oProDT = new cmlProrateDT();
                    oProDT.FTBchCode = cVB.tVB_BchCode;
                    oProDT.FTSalDocNo = cVB.tVB_DocNo;
                    oProDT.FNSeqNo = oPdt.Field<int>("FNXsdSeqNo");
                    oProDT.FTSalPdtCode = oPdt.Field<string>("FTPdtCode");
                    oProDT.FTSalPunCode = oPdt.Field<string>("FTPunCode");
                    oProDT.FCXsdNetAfHD = oPdt.Field<decimal>("FCXsdNetAfHD");
                    aoW_ProrateDT.Add(oProDT);
                }

                nW_StaChkHQ = Convert.ToInt32(oW_SelectCpn.FTCptStaChkHQ);
                if (oW_SelectCpn.FNCpdAlwMaxUse == 0 && oW_SelectCpn.FTStaChkMember == "1")
                {
                    oCpnDisDetail = W_GEToCouponDetail(oW_SelectCpn.FTCphDocNo, oW_SelectCpn.FTCpdBarCpn);
                }
                else
                {
                    oCpnDisDetail = W_CHKxAPICoupon(oW_SelectCpn);
                }

                if (oCpnDisDetail != null)
                {
                    W_SHWxCouponDetail(oCpnDisDetail);
                }
                else
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                    return;
                }
                //if AlwMaxUse=0 && ChkMember==0
                //  get local
                //else 
                //  Call API


            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void otbCouponNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        ocmSearch_Click(ocmSearch, new EventArgs());
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }
        #endregion

        #region Functions


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

                //opnHeader.BackColor = cVB.oVB_ColNormal; //*Arm 63-08-11
                ocmSearch.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
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
                        olaTitle.Text = oW_Resource.GetString("tCashCoupon");
                        break;
                    case 2: // 2:คูปองส่วนลด
                        olaTitle.Text = oW_Resource.GetString("tDiscountCoupon");
                        break;
                }

                olaTitleAval.Text = oW_Resource.GetString("tTitleAvailableDis");
                olaTitleDate.Text = oW_Resource.GetString("tUseableDate");
                olaTitleTime.Text = oW_Resource.GetString("tUseableTime");
                olaTitleCondition.Text = oW_Resource.GetString("tDiscCond");
                olaCpnName.Text = oW_Resource.GetString("tTitleCpnName");
                olaTitleValue.Text = oW_Resource.GetString("tCpnValue");

                switch (nW_Mode)
                {
                    case 1:
                        olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cVB.oVB_Payment.cW_AmtTotalShw, cVB.nVB_DecShow);
                        break;
                    case 2:
                        olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cW_AlwDisChg, cVB.nVB_DecShow);
                        break;
                }
                olaCpnDate.Text = "";
                olaCpnTime.Text = "";
                olaCpnCondition.Text = "";
                olaCpnVal.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);

                switch (nW_Mode)
                {
                    case 1: cVB.tVB_KbdCallByName = "C_KBDxCashCoupon"; break;
                    case 2: cVB.tVB_KbdCallByName = "C_KBDxDiscountCoupon"; break;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private List<cmlTPSTCouponHD_Tmp> W_GEToLocal(string ptBarCpn)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                oSql.AppendLine($"SELECT DISTINCT HD.FTBchCode, HD.FTCphDocNo, HD.FTCptCode, ");
                oSql.AppendLine($"FTCphDisType, FTCpdBarCpn,FNCpdSeqNo, FNCpdAlwMaxUse, ");
                oSql.AppendLine($"FTStaChkMember,FTCptStaChkHQ, FNCphLimitUsePerBill, FCCphDisValue, ");
                oSql.AppendLine($"FCCphMinValue, FTCphStaClosed, FTCphStaOnTopPmt, ");
                oSql.AppendLine($"FTCphTimeStart, FTCphTimeStop FROM TPSTCouponHDTmp HD WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN TFNMCouponType CPT WITH(NOLOCK) ON CPT.FTCptCode=HD.FTCptCode");
                oSql.AppendLine($"LEFT JOIN TFNTCouponHDCstPri HDCST  WITH (NOLOCK) ON HD.FTBchCode=HDCST.FTBchCode AND HD.FTCphDocNo =HDCST.FTCphDocNo");
                oSql.AppendLine($"WHERE HD.FTCpdBarCpn = '{ptBarCpn}' AND CPT.FTCptType='{nW_Mode}' AND FTCphDisType<>'3' AND ISNULL(HD.FCCphMinValue,0)<={cVB.cVB_Amount}");
                oSql.AppendLine($"AND CONVERT(time(0), GETDATE(),108) BETWEEN HD.FTCphTimeStart AND HD.FTCphTimeStop");
                oSql.AppendLine($"AND ((ISNULL(HDCST.FTCphStaType,'1')='1' AND (HDCST.FTPplCode='{cVB.tVB_PriceGroup}' OR ISNULL(HDCST.FTPplCode,'')=''))OR (HDCST.FTCphStaType='2' AND HDCST.FTPplCode<>'{cVB.tVB_PriceGroup}' )) ");
                return oDB.C_GETaDataQuery<cmlTPSTCouponHD_Tmp>(oSql.ToString());

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return new List<cmlTPSTCouponHD_Tmp>();
        }

        private cmlCouponDisDetail W_GEToCouponDetail(string ptCpnDocNo, string ptBarCpn)
        {
            StringBuilder oSql;
            cDatabase oDB;

            cmlCouponDisDetail oDt = new cmlCouponDisDetail();
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine($"SELECT (CASE WHEN ISNULL(HD_L.FTCpnName,'')='' THEN CPT_L.FTCptName ELSE FTCpnName END) AS tCpnName,");
                oSql.AppendLine($"CONVERT(VARCHAR(MAX),HD.FDCphDateStart,103) AS tDateStart,");
                oSql.AppendLine($"CONVERT(VARCHAR(MAX),HD.FDCphDateStop,103) AS tDateStop,");
                oSql.AppendLine($"HD.FTCphTimeStart AS tTimeStart, HD.FTCphTimeStop AS tTimeStop,");
                oSql.AppendLine($"HD.FTCphDisType AS tCpnDisType,HD.FCCphDisValue AS tCpnDisValue");
                oSql.AppendLine($"FROM TFNTCouponHD HD WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN TFNTCouponHD_L HD_L WITH(NOLOCK) ON HD_L.FTCphDocNo=HD.FTCphDocNo AND HD_L.FNLngID={cVB.nVB_Language}");
                oSql.AppendLine($"INNER JOIN TFNMCouponType_L CPT_L WITH(NOLOCK) ON CPT_L.FTCptCode=HD.FTCptCode AND CPT_L.FNLngID={cVB.nVB_Language}");
                oSql.AppendLine($"INNER JOIN TFNTCouponDT DT WITH(NOLOCK) ON DT.FTBchCode=HD.FTBchCode AND DT.FTCphDocNo=HD.FTCphDocNo");
                oSql.AppendLine($"WHERE DT.FTCphDocNo='{ptCpnDocNo}' AND DT.FTCpdBarCpn='{ptBarCpn}'");

                return oDB.C_GETaDataQuery<cmlCouponDisDetail>(oSql.ToString()).FirstOrDefault();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return oDt;
        }

        private void W_SHWxCouponDetail(cmlCouponDisDetail poCpnDT)
        {
            decimal cAmount = 0;
            string tDisTypeUnit = "";
            try
            {
                string tMsg = JsonConvert.SerializeObject(poCpnDT);
                new cLog().C_WRTxLog("W_SHWxCouponDetail", tMsg, cVB.bVB_AlwPrnLog);

                olaCpnName.Text = poCpnDT.tCpnName;
                olaCpnDate.Text = $"{poCpnDT.tDateStart} - {poCpnDT.tDateStop}";
                olaCpnTime.Text = $"{poCpnDT.tTimeStart} - {poCpnDT.tTimeStop}";

                tDisTypeUnit = (poCpnDT.tCpnDisType == "1") ? "" : "%";
                poCpnDT.tCpnDisValue = oW_SP.SP_SETtDecShwSve(1, decimal.Parse(poCpnDT.tCpnDisValue), cVB.nVB_DecShow);
                switch (nW_Mode)
                {
                    case 1: //คูปองเงินสด
                        olaCpnCondition.Text = $"{oW_Resource.GetString("tCashCoupon")} ({poCpnDT.tCpnDisValue}{tDisTypeUnit})";
                        break;
                    case 2: //คูปองส่วนลด
                        olaCpnCondition.Text = $"{oW_Resource.GetString("tDiscountCoupon")} ({poCpnDT.tCpnDisValue}{tDisTypeUnit})"; break;
                }
                switch (poCpnDT.tCpnDisType)
                {
                    case "1": //ลดบาท
                        olaCpnVal.Text = oW_SP.SP_SETtDecShwSve(1, decimal.Parse(poCpnDT.tCpnDisValue), cVB.nVB_DecShow);
                        break;
                    case "2": //ลด %
                        //cAmount = (cW_AlwDisChg * decimal.Parse(poCpnDT.tCpnDisValue)) / 100;
                        cAmount = (cW_AlwDisChg * Convert.ToDecimal(poCpnDT.tCpnDisValue)) / 100;
                        olaCpnVal.Text = oW_SP.SP_SETtDecShwSve(1, cAmount, cVB.nVB_DecShow);
                        break;
                }


            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponDis", "W_SHWxCouponDetail : " + oEx.Message);
            }
        }

        private cmlCouponDisDetail W_CHKxAPICoupon(cmlTPSTCouponHD_Tmp poCpn)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            cmlCouponDisDetail oCpnDT = new cmlCouponDisDetail();
            try
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                string tUrl = "";

                if (nW_StaChkHQ == 1)
                {
                    tUrl = cVB.tVB_API2FNWalletHQ;
                    new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Get API HQ", cVB.bVB_AlwPrnLog); //*Net Stamp
                }
                else
                {
                    tUrl = cVB.tVB_API2FNWallet;
                    new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : GET API Bch", cVB.bVB_AlwPrnLog); //*Net Stamp
                }

                if (string.IsNullOrEmpty(tUrl))
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlWalletNotDefine"), 3);
                    return null;
                }
                else
                {
                    this.UseWaitCursor = true;
                    cmlReqCoupon oReqCoupon = new cmlReqCoupon();
                    oReqCoupon.ptCouponType = nW_Mode.ToString();
                    oReqCoupon.ptCpnDocNo = poCpn.FTCphDocNo;
                    oReqCoupon.ptBarCpn = poCpn.FTCpdBarCpn.Trim();
                    oReqCoupon.ptBranch = cVB.tVB_BchCode;
                    oReqCoupon.ptPriceGroup = cVB.tVB_PriceGroup;
                    oReqCoupon.ptMerchant = cVB.tVB_Merchart;
                    oReqCoupon.pnLangID = cVB.nVB_Language;
                    oReqCoupon.ptCstCode = cVB.tVB_CstCode;
                    string tJSonCall = JsonConvert.SerializeObject(oReqCoupon);
                    cClientService oCall = new cClientService();
                    oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);
                    HttpResponseMessage oRep = new HttpResponseMessage();
                    try
                    {
                        new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Call API2Wallet CheckCouponHD", cVB.bVB_AlwPrnLog); //*Net Stamp
                        oRep = oCall.C_POSToInvoke(tUrl + "/Coupon/CheckCouponHD", tJSonCall);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("wCouponDis", "W_CHKxAPICoupon : " + oEx.Message);
                        return null;
                    }

                    new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Get Response API Status {oRep.StatusCode}", cVB.bVB_AlwPrnLog); //*Net Stamp
                    if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                        cmlResCoupon oRes = JsonConvert.DeserializeObject<cmlResCoupon>(tJSonRes);

                        if (oRes.rtCode == "1")
                        {
                            if (oRes.raoCoupon.Count == 1)
                            {
                                oCpnDT.tCpnName = oRes.raoCoupon[0].rtCpnName;
                                oCpnDT.tCpnDisType = oRes.raoCoupon[0].rtCphDisType;
                                oCpnDT.tCpnDisValue = oRes.raoCoupon[0].rcCphDisValue.ToString();
                                oCpnDT.tDateStart = oRes.raoCoupon[0].rdCphDateStart != null ? oRes.raoCoupon[0].rdCphDateStart.Value.ToString("dd/MM/yyyy") : "n/a";
                                oCpnDT.tDateStop = oRes.raoCoupon[0].rdCphDateStop != null ? oRes.raoCoupon[0].rdCphDateStop.Value.ToString("dd/MM/yyyy") : "n/a";
                                oCpnDT.tTimeStart = oRes.raoCoupon[0].rtCphTimeStart.ToString();
                                oCpnDT.tTimeStop = oRes.raoCoupon[0].rtCphTimeStop.ToString();
                                oCpnDT.tCondition = oRes.raoCoupon[0].rtCpnCond;
                            }
                            else
                            {
                                oCall.C_PRCxCloseConn();    //*Em 63-07-18
                                return null;
                            }
                        }
                        else
                        {
                            oCall.C_PRCxCloseConn();    //*Em 63-07-18
                            /// oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                            new cLog().C_WRTxLog("wCoupon", "W_CHKxAPICoupon : " + oRes.rtDesc);
                            return null;
                        }
                    }
                    else
                    {
                        oCall.C_PRCxCloseConn();    //*Em 63-07-18
                        new cLog().C_WRTxLog("wCouponDis", "W_CHKxAPICoupon : " + tUrl + " " + oRep.StatusCode);
                        return null;
                    }
                    oCall.C_PRCxCloseConn();    //*Em 63-07-18
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCouponDis", "W_CHKxAPICoupon : " + oEx.Message); }
            finally
            {
                this.UseWaitCursor = false;
            }
            return oCpnDT;
        }
        private bool W_PRCbCoupon()
        {
            string tQueueReq = "";
            string tQueueRes = "";
            try
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                //if (cVB.tVB_API2FNWalletHQ == cVB.tVB_API2FNWallet)
                //{
                //    new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : API Centralized"); //*Net Stamp
                //    nW_StaChkHQ = 1;
                //}
                if (nW_StaChkHQ == 1)
                {
                    new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Get MQ HQ", cVB.bVB_AlwPrnLog); //*Net Stamp
                    //if (cVB.tVB_API2FNWalletHQ == cVB.tVB_API2FNWallet)
                    //{
                    //    tQueueReq = "FN_PayReqCoupon";
                    //}
                    //else
                    //{
                    //    tQueueReq = "FN_PayReqCoupon" + cVB.tVB_HQBchCode;
                    //}
                    tQueueReq = "FN_PayReqCoupon";
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
                    new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Get MQ Bch", cVB.bVB_AlwPrnLog); //*Net Stamp
                    //tQueueReq = "FN_PayReqCoupon" + cVB.tVB_BchCode;
                    tQueueReq = "FN_PayReqCoupon";
                    if (string.IsNullOrEmpty(cVB.tVB_BCHMQHost)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_BCHMQUsr)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_BCHMQPwd)) return false; //*Em 62-09-03
                    if (string.IsNullOrEmpty(cVB.tVB_BCHMQVirtual)) return false; //*Em 62-09-03

                    cVB.oVB_RabbitMQConfig.tHostName = cVB.tVB_BCHMQHost;
                    cVB.oVB_RabbitMQConfig.tUserName = cVB.tVB_BCHMQUsr;
                    cVB.oVB_RabbitMQConfig.tPassword = cVB.tVB_BCHMQPwd;
                    cVB.oVB_RabbitMQConfig.tVirtual = cVB.tVB_BCHMQVirtual;
                }

                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Connect MQ", cVB.bVB_AlwPrnLog); //*Net Stamp
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

                oReqData.ptCphDocNo = oW_SelectCpn.FTCphDocNo;
                oReqData.ptCpdBarCpn = otbCouponNo.Text.Trim();
                oReqData.ptCpdSeqNo = Convert.ToString(oW_SelectCpn.FNCpdSeqNo);
                oReqData.ptSaleDocNo = cVB.tVB_DocNo;
                oReqData.ptCpbFrmBch = cVB.tVB_BchCode;
                oReqData.ptCpbFrmPos = cVB.tVB_PosCode;
                oW_ReqData = oReqData;
                oReq.ptData = JsonConvert.SerializeObject(oReqData);

                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : Start Publish", cVB.bVB_AlwPrnLog); //*Net Stamp
                string tMsg = JsonConvert.SerializeObject(oReq);
                var oBody = Encoding.UTF8.GetBytes(tMsg);
                cVB.oVB_MQModel.BasicPublish("", tQueueReq, false, null, oBody);
                cVB.oVB_MQModel.Close();
                cVB.oVB_MQConn.Close();
                cVB.oVB_MQFactory = null;

                tQueueRes = "FN_PayRetCoupon" + cVB.tVB_BchCode + cVB.tVB_PosCode;
                cVB.oVB_Coupon = new cRabbitMQ(tQueueRes, "");
                cVB.oVB_Coupon.oEv_Jump += new EventHandler(oVB_Coupon_Receive);
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + $" : End", cVB.bVB_AlwPrnLog); //*Net Stamp
                return true;
            }
            catch (Exception oEx) 
            {
                new cLog().C_WRTxLog("wCouponDis", "W_PRCxCoupon : " + oEx.Message); 
                return false; 
            }
        }

        private void oVB_Coupon_Receive(object sender, EventArgs e)
        {
            decimal cAmt = 0m;
            try
            {
                bPublish = false;
                if (oW_SelectCpn.FNCpdAlwMaxUse != 0 || oW_SelectCpn.FTStaChkMember != "1")
                {
                    cVB.oVB_Coupon.C_DISxDisConnect();
                    cVB.oVB_Coupon = null;
                }
                if (oW_ReqData == null)
                {
                    oW_ReqData = new cmlReqData();
                    oW_ReqData.ptCphDocNo = oW_SelectCpn.FTCphDocNo;
                    oW_ReqData.ptCpdBarCpn = oW_SelectCpn.FTCpdBarCpn;
                    oW_ReqData.ptCpdSeqNo = Convert.ToString(oW_SelectCpn.FNCpdSeqNo);
                    oW_ReqData.ptSaleDocNo = cVB.tVB_DocNo;
                    oW_ReqData.ptCpbFrmBch = cVB.tVB_BchCode;
                    oW_ReqData.ptCpbFrmPos = cVB.tVB_PosCode;

                }

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        //opbWait.Visible = false;
                        opnDetail.Enabled = true;

                        if (cPayment.tC_XrcRef1 == "OK")
                        {
                            cPayment.tC_XrcRef1 = oW_SelectCpn.FTCpdBarCpn;
                            cPayment.tC_XrcRef2 = oW_SelectCpn.FTCptCode;

                            cAmt = decimal.Parse(olaCpnVal.Text);
                            cVB.cVB_Amount = cAmt;
                            //cVB.cVB_Amount = Convert.ToDecimal(olaCpnVal.Text);
                            if (nW_Mode == 2)
                            {
                                if (aoW_ProrateDT.Count > 0)
                                {
                                    cAmt = 0m;
                                    foreach (cmlProrateDT oDT in aoW_ProrateDT)
                                    {
                                        cAmt += oDT.FCXsdNetAfHD;
                                    }
                                }
                                cVB.cVB_Amount = (cAmt < decimal.Parse(olaCpnVal.Text)) ? cAmt : decimal.Parse(olaCpnVal.Text);
                                switch (oW_SelectCpn.FTCphDisType)
                                {
                                    case "1":
                                        cVB.oVB_Payment.W_ADDxDisChgBill("5", Convert.ToString(oW_SelectCpn.FCCphDisValue), cVB.cVB_Amount);
                                        cVB.tVB_DisChgTxt = Convert.ToDouble(oW_SelectCpn.FCCphDisValue).ToString();
                                        break;
                                    case "2":
                                        cVB.oVB_Payment.W_ADDxDisChgBill("5", Convert.ToString(oW_SelectCpn.FCCphDisValue) + "%", cVB.cVB_Amount);
                                        cVB.tVB_DisChgTxt = Convert.ToDouble(oW_SelectCpn.FCCphDisValue).ToString() + "%";
                                        break;
                                }
                            }
                            W_SAVxCouponUse(oW_ReqData);
                            this.Close();
                        }
                        else
                        {

                            W_SETxFormLock(false);
                            //opnPage.Enabled = true;
                            //opbWait.Visible = false;
                            //oW_SP.SP_SHWxMsg(cPayment.tC_XrcRef2, 2);
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                        }
                    }));
                }
                else
                {
                    //opbWait.Visible = false;
                    opnDetail.Enabled = true;
                    if (cPayment.tC_XrcRef1 == "OK")
                    {
                        cPayment.tC_XrcRef1 = oW_SelectCpn.FTCpdBarCpn;
                        cPayment.tC_XrcRef2 = oW_SelectCpn.FTCptCode;
                        cAmt = decimal.Parse(olaCpnVal.Text);
                        if (aoW_ProrateDT.Count > 0)
                        {
                            cAmt = 0m;
                            foreach (cmlProrateDT oDT in aoW_ProrateDT)
                            {
                                cAmt += oDT.FCXsdNetAfHD;
                            }
                        }
                        cVB.cVB_Amount = (cAmt < decimal.Parse(olaCpnVal.Text)) ? cAmt : decimal.Parse(olaCpnVal.Text);
                        //cVB.cVB_Amount = Convert.ToDecimal(olaCpnVal.Text);
                        if (nW_Mode == 2)
                        {
                            switch (oW_SelectCpn.FTCphDisType)
                            {
                                case "1":
                                    cVB.oVB_Payment.W_ADDxDisChgBill("5", Convert.ToString(oW_SelectCpn.FCCphDisValue), cVB.cVB_Amount);
                                    cVB.tVB_DisChgTxt = Convert.ToDouble(oW_SelectCpn.FCCphDisValue).ToString();
                                    break;
                                case "2":
                                    cVB.oVB_Payment.W_ADDxDisChgBill("5", Convert.ToString(oW_SelectCpn.FCCphDisValue) + "%", cVB.cVB_Amount);
                                    cVB.tVB_DisChgTxt = Convert.ToDouble(oW_SelectCpn.FCCphDisValue).ToString() + "%";
                                    break;
                            }
                        }
                        W_SAVxCouponUse(oW_ReqData);
                        this.Close();
                    }
                    else
                    {

                        W_SETxFormLock(false);
                        //opnPage.Enabled = true;
                        //opbWait.Visible = false;
                        //oW_SP.SP_SHWxMsg(cPayment.tC_XrcRef2, 2);
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                    }
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCouponDis", "oVB_Coupon_Receive : " + oEx.Message); }
        }

        private void W_SAVxCouponUse(cmlReqData poResData)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine("INSERT INTO TFNTCouponDTHis (FTBchCode,FTCphDocNo,FTCpdBarCpn,FDCpbFrmStart,");
                oSql.AppendLine("FTCpbFrmBch,FTCpbFrmPos,FTCpbFrmSalRef,FTCpbStaBook,FDLastUpdOn,");
                oSql.AppendLine("FTLastUpdBy,FDCreateOn,FTCreateBy) VALUES (");
                oSql.AppendLine("'" + cVB.tVB_BchCode + "', ");
                oSql.AppendLine("'" + poResData.ptCphDocNo + "', ");
                oSql.AppendLine("'" + poResData.ptCpdBarCpn + "', ");
                //oSql.AppendLine("'" + oPayCoupon.pnCpdSeqNo + "', ");
                oSql.AppendLine("GETDATE(), ");
                oSql.AppendLine("'" + poResData.ptCpbFrmBch + "', ");
                oSql.AppendLine("'" + poResData.ptCpbFrmPos + "', ");
                oSql.AppendLine("'" + poResData.ptSaleDocNo + "', ");
                oSql.AppendLine("'1',");
                oSql.AppendLine("GETDATE(),");
                oSql.AppendLine($"'{cVB.tVB_PosCode}',");
                oSql.AppendLine("GETDATE(),");
                oSql.AppendLine($"'{cVB.tVB_PosCode}' )");

                oDB.C_GETaDataQuery<string>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private bool W_CHKbLimitPerBill(cmlTPSTCouponHD_Tmp poCoupon)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                string tLimitperBill = (poCoupon.FNCphLimitUsePerBill == null) ? "0" : poCoupon.FNCphLimitUsePerBill.Value.ToString();
                oSql.AppendLine($"SELECT (CASE WHEN COUNT(*)>={tLimitperBill} AND {tLimitperBill}<>0 THEN 1 ELSE 0 END) AS FNQtyUse");
                oSql.AppendLine($"FROM TFNTCouponDTHis");
                oSql.AppendLine($"WHERE FTCpbFrmSalRef='{cVB.tVB_DocNo}' AND FTCphDocNo='{poCoupon.FTCphDocNo}' AND FTCpdBarCpn='{poCoupon.FTCpdBarCpn}' AND FTCpbStaBook='1'");
                oSql.AppendLine($"GROUP BY FTCpbFrmSalRef,FTCphDocNo,FTCpdBarCpn,FTBchCode");

                return oDB.C_GETaDataQuery<bool>(oSql.ToString()).FirstOrDefault();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return false;
        }

        private bool W_CHKbOnTopPromotion(cmlTPSTCouponHD_Tmp poCoupon)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                return false;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return true;
        }

        private bool W_CHKbSalDT(cmlTPSTCouponHD_Tmp poSelectCpn)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oW_PdtUse = new DataTable();
                oSql.AppendLine($"SELECT FTCphDocNo,FTPdtCode,FTPunCode");
                oSql.AppendLine($"FROM TFNTCouponHDPdt HDPdt WITH(NOLOCK)");
                oSql.AppendLine($"WHERE HDPdt.FTCphDocNo='{poSelectCpn.FTCphDocNo}'");
                oW_PdtUse = oDB.C_GEToDataQuery(oSql.ToString());
                if (oW_PdtUse.Rows.Count == 0)
                {
                    oSql.Clear();
                    oSql.AppendLine($"SELECT DISTINCT FNXsdSeqNo,FTPdtCode,FTPunCode,FCXsdNetAfHD FROM TSDT{cVB.tVB_PosCode} WITH(NOLOCK) WHERE FTXshDocNo='{cVB.tVB_DocNo}' AND FTXsdStaAlwDis='1' AND FTXsdStaPdt<>'4'");
                    oW_PdtUse = oDB.C_GEToDataQuery(oSql.ToString());
                    return true;
                }

                oSql.Clear();
                oSql.AppendLine($"SELECT DT.FNXsdSeqNo,FTCphDocNo,DT.FTPdtCode,DT.FTPunCode,FCXsdNetAfHD");
                oSql.AppendLine($"FROM TFNTCouponHDPdt HDPdt WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN (SELECT DISTINCT FNXsdSeqNo,FTPdtCode,FTPunCode,FCXsdNetAfHD FROM TSDT{cVB.tVB_PosCode} WITH(NOLOCK) WHERE FTXshDocNo='{cVB.tVB_DocNo}' AND FTXsdStaAlwDis='1' AND FTXsdStaPdt<>'4') DT ON");
                oSql.AppendLine($"((HDPdt.FTCphStaType='1' AND DT.FTPdtCode = HDPdt.FTPdtCode AND DT.FTPunCode=HDPdt.FTPunCode) OR ");
                oSql.AppendLine($"(HDPdt.FTCphStaType='2' AND (DT.FTPdtCode <> HDPdt.FTPdtCode OR DT.FTPunCode <> HDPdt.FTPunCode)))");
                oSql.AppendLine($"WHERE HDPdt.FTCphDocNo='{poSelectCpn.FTCphDocNo}'");
                oW_PdtUse = oDB.C_GEToDataQuery(oSql.ToString());
                if (oW_PdtUse.Rows.Count == 0) return false;

                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponDis", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return false;
        }

        private void W_SETxFormLock(bool ptLock)
        {
            this.Invoke(new MethodInvoker(()=>{

                ocmAccept.Enabled = !ptLock;
                opnSearch.Enabled = !ptLock;
            }));
        }
        #endregion

    }
}
