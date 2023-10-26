using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.DatabaseTmp;
using AdaPos.Models.Other;
using AdaPos.Models.RabbitMQ;
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
    public partial class wCouponRedeem : Form
    {
        #region Variable

        public string tFTXshDocNo;

        /// <summary>
        /// ยอดก่อนลด
        /// </summary>
        public decimal cW_B4DisChg;

        /// <summary>
        /// ยอดหลังลด
        /// </summary>
        public decimal cW_AfDisChg;

        /// <summary>
        /// มูลค่าลด/ชาร์จ
        /// </summary>
        public decimal cW_Amt;

        /// <summary>
        /// ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// </summary>
        public string tW_DisChgTxt;

        private int nW_StaChkHQ = 1;    //1:HQ 2:Branch

        private cSP oW_SP;
        private DataTable oW_PdtUse;
        private ResourceManager oW_Resource;
        private wChooseCpn oW_ChooseCpn;
        private cmlTPSTCouponHD_Tmp oW_SelectCpn;
        private cmlReqData oW_ReqData;

        public List<cmlProrateDT> aoW_ProrateDT;
        #endregion

        public wCouponRedeem()
        {
            InitializeComponent();
            oW_SP = new cSP();
            aoW_ProrateDT = new List<cmlProrateDT>();
            oW_PdtUse = new DataTable();
            W_SETxDesign();
            W_SETxText();
        }

        #region Event
        private void wCouponRedeem_Shown(object sender, EventArgs e)
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

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                cPayment.tC_XrcRef1 = "";
                cPayment.tC_XrcRef2 = "";
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            new cFunctionKeyboard().C_KBDxKeyboard();
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            string tMsg = "";
            try
            {
                if (oW_SelectCpn == null || String.IsNullOrEmpty(oW_SelectCpn.FTCphDocNo))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgInputCoupon"), 3);
                    otbCouponNo.Focus();
                    return;
                }

                if (ogdRedeemPdt.SelectedRows.Count < 1)
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgSelPdt"), 3);
                    otbCouponNo.Focus();
                    return;
                }

                aoW_ProrateDT = new List<cmlProrateDT>();
                cmlProrateDT oProDT = new cmlProrateDT();
                oProDT.FTBchCode = cVB.tVB_BchCode;
                oProDT.FTSalDocNo = cVB.tVB_DocNo;
                oProDT.FNSeqNo = ((DataTable)ogdRedeemPdt.DataSource).Rows[ogdRedeemPdt.SelectedRows[0].Index].Field<int>("FNSeqNo");
                oProDT.FTSalPdtCode = ((DataTable)ogdRedeemPdt.DataSource).Rows[ogdRedeemPdt.SelectedRows[0].Index].Field<string>("FTPdtCode");
                oProDT.FTSalPunCode = ((DataTable)ogdRedeemPdt.DataSource).Rows[ogdRedeemPdt.SelectedRows[0].Index].Field<string>("FTPunCode");
                aoW_ProrateDT.Add(oProDT);

                cW_B4DisChg = decimal.Parse(ogdRedeemPdt.SelectedRows[0].Cells[4].Value.ToString());
                cW_AfDisChg = decimal.Parse(ogdRedeemPdt.SelectedRows[0].Cells[5].Value.ToString());

                cW_Amt = cW_B4DisChg - cW_AfDisChg;

                if ((decimal)(cVB.oVB_Payment.cW_AmtTotalCal - cW_Amt) < (decimal)cVB.cVB_SmallBill)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                    return;
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
                /*cmlTPSTSalDTDis oSalDis = new cmlTPSTSalDTDis();
                oSalDis.FTBchCode = cVB.tVB_BchCode;
                oSalDis.FTXshDocNo = cVB.tVB_DocNo;
                // ตามรายการ
                oSalDis.FNXsdSeqNo = ((DataTable)ogdRedeemPdt.DataSource).Rows[ogdRedeemPdt.SelectedRows[0].Index].Field<int>("FNSeqNo");
                oSalDis.FCXddNet = cW_B4DisChg;
                oSalDis.FNXddStaDis = 2;
                oSalDis.FTXddDisChgTxt = cW_Amt.ToString();
                oSalDis.FTXddDisChgType = "1";
                oSalDis.FCXddValue = cW_Amt;
                oSalDis.FTXddRefCode = oW_SelectCpn.FTCpdBarCpn;
                cSale.nC_DTSeqNo = oSalDis.FNXsdSeqNo.Value;
                new cSale().C_PRCxDisChgItem(oSalDis);*/
                //cSale.C_PRCxSummary2HD();
                /*if (!(oW_SelectCpn.FNCpdAlwMaxUse == 0 && oW_SelectCpn.FTStaChkMember == "1"))
                {*/
                //opnPage.Enabled = false;

                W_SETxFormLock(true);
                if (oW_SelectCpn.FNCpdAlwMaxUse == 0 && oW_SelectCpn.FTStaChkMember == "1")
                {
                    cPayment.tC_XrcRef1 = "OK";
                    oVB_Coupon_Receive(sender, e);

                    W_SETxFormLock(false);
                }
                else
                {

                    nW_StaChkHQ = Convert.ToInt32(oW_SelectCpn.FTCptStaChkHQ);
                    //opbWait.Visible = true;
                    cPayment.tC_XrcRef1 = "";
                    cPayment.tC_XrcRef2 = "";
                    if (W_PRCbCoupon() == false)
                    {
                        //opbWait.Visible = false;
                        W_SETxFormLock(false);
                        return;
                    }
                }
                //}

                cPayment.tC_XrcRef1 = oW_SelectCpn.FTCphDocNo;
                cPayment.tC_XrcRef2 = oW_SelectCpn.FTCptCode;
                cVB.cVB_Amount = cW_Amt;
                
                //if (nW_Mode == 2) cVB.oVB_Payment.W_ADDxDisChgBill("5", olaCpnVal.Text, cVB.cVB_Amount);
                //this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "ocmAccept_Click : " + oEx.Message); }
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
                    W_SETxText();
                    return;
                }

                oW_SelectCpn = aoListCpn.FirstOrDefault();
                if (aoListCpn.Count > 1)
                {
                    //Choose Coupon
                    oW_ChooseCpn = new wChooseCpn(aoListCpn, 2, 3);
                    oW_ChooseCpn.ShowDialog();
                    W_SETxText();
                    if (oW_ChooseCpn.bW_Select == false) return;
                    oW_SelectCpn = oW_ChooseCpn.oW_Conpon;
                }

                if (W_CHKbSalDT(oW_SelectCpn) == false)
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                    W_SETxText();
                    return;
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
                    W_SETxText();
                    return;
                }


            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }


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

        private void ogdRedeemPdt_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ocmAccept_Click(ocmAccept, new EventArgs());
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
                new cLog().C_WRTxLog("wCouponRedeem", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
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

                opnHeader.BackColor = cVB.oVB_ColNormal;
                ocmSearch.BackColor = cVB.oVB_ColNormal;

                oW_SP.SP_SETxSetGridviewFormat(ogdRedeemPdt); //*Net 63-03-16 Set Design Gridview
                ogdRedeemPdt.AutoGenerateColumns = false;
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


                olaTitle.Text = oW_Resource.GetString("tRedeemCoupon");

                olaCpnName.Text = oW_Resource.GetString("tTitleCpnName");

                olaCpnDate.Text = "";
                olaCpnTime.Text = "";

                olaTitleCpnLimitPerBill.Text = oW_Resource.GetString("tTitleRedeemLimitQty");
                olaTitleCpnMinVal.Text = oW_Resource.GetString("tRdColMinTotBill");
                olaCpnLimitPerBill.Text = "0";
                olaCpnMinVal.Text = oW_SP.SP_SETtDecShwSve(1, decimal.Zero, cVB.nVB_DecShow);

                FTBarCode.HeaderText = oW_Resource.GetString("tRdColBarcode");
                FTPdtName.HeaderText = oW_Resource.GetString("tRdColPdtName");
                FTPunName.HeaderText = oW_Resource.GetString("tRdColUnitName");
                FNQty.HeaderText = oW_Resource.GetString("tRdColQty");
                FCB4Price.HeaderText = oW_Resource.GetString("tOldPrice");
                FCAfPrice.HeaderText = oW_Resource.GetString("tNewPrice");

                ogdRedeemPdt.DataSource = null;

                cVB.tVB_KbdCallByName = "C_KBDxCashCoupon";

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
                oSql.AppendLine($"FTCphDisType,HD.FTPplCode, FTCpdBarCpn,FNCpdSeqNo, FNCpdAlwMaxUse, ");
                oSql.AppendLine($"FTStaChkMember,FTCptStaChkHQ, FNCphLimitUsePerBill, FCCphDisValue, ");
                oSql.AppendLine($"FCCphMinValue, FTCphStaClosed, FTCphStaOnTopPmt, ");
                oSql.AppendLine($"FTCphTimeStart, FTCphTimeStop FROM TPSTCouponHDTmp HD WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN TFNMCouponType CPT WITH(NOLOCK) ON CPT.FTCptCode=HD.FTCptCode");
                oSql.AppendLine($"LEFT JOIN TFNTCouponHDCstPri HDCST  WITH (NOLOCK) ON HD.FTBchCode=HDCST.FTBchCode AND HD.FTCphDocNo =HDCST.FTCphDocNo");
                oSql.AppendLine($"WHERE HD.FTCpdBarCpn = '{ptBarCpn}' AND CPT.FTCptType='2' AND FTCphDisType='3' AND ISNULL(HD.FCCphMinValue,0)<={cVB.cVB_Amount}");
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
            DataTable oPdtDetail;
            string tDisTypeUnit = "";
            try
            {
                string tMsg = JsonConvert.SerializeObject(poCpnDT);
                new cLog().C_WRTxLog("W_SHWxCouponDetail", tMsg);

                olaCpnName.Text = poCpnDT.tCpnName;
                olaCpnDate.Text = $"{poCpnDT.tDateStart} - {poCpnDT.tDateStop}";
                olaCpnTime.Text = $"{poCpnDT.tTimeStart} - {poCpnDT.tTimeStop}";

                tDisTypeUnit = (poCpnDT.tCpnDisType == "1") ? "" : "%";
                poCpnDT.tCpnDisValue = oW_SP.SP_SETtDecShwSve(1, decimal.Parse(poCpnDT.tCpnDisValue), cVB.nVB_DecShow);


                olaCpnLimitPerBill.Text = "0";
                olaCpnMinVal.Text = oW_SP.SP_SETtDecShwSve(1, decimal.Parse("0.00"), cVB.nVB_DecShow);
                if (oW_SelectCpn.FNCphLimitUsePerBill.HasValue)
                {
                    olaCpnLimitPerBill.Text = oW_SelectCpn.FNCphLimitUsePerBill.Value.ToString();
                }
                if (oW_SelectCpn.FCCphMinValue.HasValue)
                {
                    olaCpnMinVal.Text = oW_SP.SP_SETtDecShwSve(1, oW_SelectCpn.FCCphMinValue.Value, cVB.nVB_DecShow);
                }

                oPdtDetail = W_GEToPdtbyPpl(oW_SelectCpn);

                ogdRedeemPdt.DataSource = oPdtDetail;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCoupon", "W_SHWxCouponDetail : " + oEx.Message);
            }
        }


        private cmlCouponDisDetail W_CHKxAPICoupon(cmlTPSTCouponHD_Tmp poCpn)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            cmlCouponDisDetail oCpnDT = new cmlCouponDisDetail();
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
                    return null;
                }
                else
                {
                    this.UseWaitCursor = true;
                    cmlReqCoupon oReqCoupon = new cmlReqCoupon();
                    oReqCoupon.ptCouponType = "2";
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
                        oRep = oCall.C_POSToInvoke(tUrl + "/Coupon/CheckCouponHD", tJSonCall);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("wCoupon", "W_CHKxAPICoupon : " + oEx.Message);
                        return null;
                    }

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
                                return null;
                            }
                        }
                        else
                        {
                            //oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                            new cLog().C_WRTxLog("wCoupon", "W_CHKxAPICoupon : " + oRes.rtDesc);
                            return null;
                        }
                    }
                    else
                    {
                        new cLog().C_WRTxLog("wCoupon", "W_CHKxAPICoupon : " + tUrl + " " + oRep.StatusCode);
                        return null;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "W_CHKxAPICoupon : " + oEx.Message); }
            finally
            {
                this.UseWaitCursor = false;
            }
            return oCpnDT;
        }

        private DataTable W_GEToPdtbyPpl(cmlTPSTCouponHD_Tmp poSelectCpn)
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable oTblResult;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oTblResult = new DataTable();
                oSql.AppendLine($"DECLARE @ptDocNo varchar(20)='{cVB.tVB_DocNo}';");
                oSql.AppendLine($"DECLARE @ptPplCode varchar(20)='{poSelectCpn.FTPplCode}';");
                oSql.AppendLine($"DECLARE @ptBarCpn varchar(20)='{poSelectCpn.FTCpdBarCpn}';");
                oSql.AppendLine($"WITH P4PDTByDate AS");
                oSql.AppendLine($"(");
                oSql.AppendLine($"SELECT FTPplCode, FTPdtCode, FTPunCode, ");
                oSql.AppendLine($"FDPghDStart, FTPghTStart, FDPghDStop, ");
                oSql.AppendLine($"FTPghTStop, FTPghDocNo, FTPghDocType, ");
                oSql.AppendLine($"FTPghStaAdj, FCPgdPriceRet, FCPgdPriceWhs, ");
                oSql.AppendLine($"FCPgdPriceNet, FDLastUpdOn, FTLastUpdBy, ");
                oSql.AppendLine($"FDCreateOn, FTCreateBy FROM TCNTPdtPrice4PDT P4PDT WITH(NOLOCK)");
                oSql.AppendLine($"WHERE P4PDT.FDPghDStart >=");
                oSql.AppendLine($"(");
                oSql.AppendLine($"SELECT TOP 1 P4PDTT.FDPghDStart FROM TCNTPdtPrice4PDT P4PDTT");
                oSql.AppendLine($"WHERE P4PDT.FTPplCode=P4PDTT.FTPplCode AND");
                oSql.AppendLine($"P4PDT.FTPdtCode=P4PDTT.FTPdtCode AND");
                oSql.AppendLine($"P4PDT.FTPunCode=P4PDTT.FTPunCode");
                oSql.AppendLine($"ORDER BY P4PDTT.FDPghDStart DESC");
                oSql.AppendLine($")");
                oSql.AppendLine($"),");
                oSql.AppendLine($"P4PDT AS");
                oSql.AppendLine($"(");
                oSql.AppendLine($"SELECT FTPplCode, FTPdtCode, FTPunCode, ");
                oSql.AppendLine($"FDPghDStart, FTPghTStart, FDPghDStop, ");
                oSql.AppendLine($"FTPghTStop, FTPghDocNo, FTPghDocType, ");
                oSql.AppendLine($"FTPghStaAdj, FCPgdPriceRet, FCPgdPriceWhs, ");
                oSql.AppendLine($"FCPgdPriceNet, FDLastUpdOn, FTLastUpdBy, ");
                oSql.AppendLine($"FDCreateOn, FTCreateBy ");
                oSql.AppendLine($"FROM P4PDTByDate P4PDT");
                oSql.AppendLine($"WHERE CONVERT(DATETIME,P4PDT.FTPghTStart) >=");
                oSql.AppendLine($"(");
                oSql.AppendLine($"SELECT TOP 1 CONVERT(DATETIME,P4PDTT.FTPghTStart) FROM P4PDTByDate P4PDTT");
                oSql.AppendLine($"WHERE P4PDT.FTPplCode=P4PDTT.FTPplCode AND");
                oSql.AppendLine($"P4PDT.FTPdtCode=P4PDTT.FTPdtCode AND");
                oSql.AppendLine($"P4PDT.FTPunCode=P4PDTT.FTPunCode");
                oSql.AppendLine($"ORDER BY CONVERT(DATETIME,P4PDTT.FTPghTStart) DESC");
                oSql.AppendLine($") AND FTPplCode=@ptPplCode");
                oSql.AppendLine($"),");
                oSql.AppendLine($"DTDis AS");
                oSql.AppendLine($"(");
                oSql.AppendLine($"SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,FTXddRefCode,COUNT(*) AS FNQtyUse ");
                oSql.AppendLine($"FROM TSDTDis{cVB.tVB_PosCode} DTDis WITH(NOLOCK)");
                oSql.AppendLine($"WHERE FNXddStaDis=2 AND DTDis.FTXddRefCode=@ptBarCpn AND DTDis.FTXddDisChgType='6'");
                oSql.AppendLine($"GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo,FTXddRefCode");
                oSql.AppendLine($")");
                oSql.AppendLine($"SELECT DISTINCT FTXsdBarCode AS FTBarCode,DT.FTPdtCode,DT.FTPunCode, FTXsdPdtName AS FTPdtName,");
                oSql.AppendLine($"FTPunName AS FTPunName, DT.FCXsdQty AS FNQty,DT.FCXsdNet AS FCB4Price,");
                oSql.AppendLine($"P4PDT.FCPgdPriceRet AS FCAfPrice,DT.FNXsdSeqNo AS FNSeqNo,");
                oSql.AppendLine($"ISNULL(DTDis.FNQtyUse,0) AS FNQtyUse");
                oSql.AppendLine($"FROM TSDT{cVB.tVB_PosCode} DT WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN P4PDT ON DT.FTPdtCode=P4PDT.FTPdtCode AND DT.FTPunCode=P4PDT.FTPunCode");
                oSql.AppendLine($"LEFT JOIN DTDis WITH(NOLOCK) ON DT.FTBchCode=DTDis.FTBchCode AND DT.FTXshDocNo=DTDis.FTXshDocNo AND DT.FNXsdSeqNo=DTDis.FNXsdSeqNo");
                oSql.AppendLine($"WHERE P4PDT.FTPplCode=@ptPplCode AND DT.FTXshDocNo=@ptDocNo AND ISNULL(DTDis.FNQtyUse,0)< DT.FCXsdQty AND DT.FTXsdStaAlwDis='1'");
                if (oW_PdtUse.Rows.Count > 0)
                {
                    oSql.AppendLine($"AND (DT.FNXsdSeqNo IN ({String.Join(",", oW_PdtUse.AsEnumerable().Select(oCol => oCol["FNXsdSeqNo"].ToString()).ToList())}))");
                }

                oTblResult = oDB.C_GEToDataQuery(oSql.ToString());

                foreach (DataRow oRow in oTblResult.Rows)
                {
                    oRow["FCB4Price"] = oW_SP.SP_SETtDecShwSve(1, decimal.Parse(oRow["FCB4Price"].ToString()) / decimal.Parse(oRow["FNQty"].ToString()), cVB.nVB_DecShow);
                    oRow["FNQty"] = oW_SP.SP_SETtDecShwSve(1, decimal.Parse("1"), cVB.nVB_DecShow);
                }

                return oTblResult;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCoupon", "W_SHWxCouponDetail : " + oEx.Message);
            }
            return new DataTable();
        }

        private bool W_PRCbCoupon()
        {
            string tQueueReq = "";
            string tQueueRes = "";
            try
            {
                if (nW_StaChkHQ == 1)
                {
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
            catch (Exception oEx) 
            { 
                new cLog().C_WRTxLog("wCoupon", "W_PRCxCoupon : " + oEx.Message); return false; 
            }
        }
        private void oVB_Coupon_Receive(object sender, EventArgs e)
        {

            decimal cAmt = 0m;
            try
            {
                if (oW_SelectCpn.FNCpdAlwMaxUse != 0 || oW_SelectCpn.FTStaChkMember != "1")
                {
                    cVB.oVB_Coupon.C_DISxDisConnect();
                    cVB.oVB_Coupon = null;
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
                            cVB.cVB_Amount = cW_Amt;
                            cVB.oVB_Payment.W_ADDxDisChgBill("7", oW_SP.SP_SETtDecShwSve(1, cW_Amt, cVB.nVB_DecShow), cVB.cVB_Amount);
                            cVB.tVB_DisChgTxt = oW_SP.SP_SETtDecShwSve(1, cW_Amt, cVB.nVB_DecShow);

                            W_SAVxCouponUse(oW_ReqData);
                            this.Close();
                        }
                        else
                        {
                            W_SETxFormLock(false);
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
                        cVB.cVB_Amount = cW_Amt;
                        cVB.oVB_Payment.W_ADDxDisChgBill("5", oW_SP.SP_SETtDecShwSve(1, cW_Amt, cVB.nVB_DecShow), cVB.cVB_Amount);
                        cVB.tVB_DisChgTxt = oW_SP.SP_SETtDecShwSve(1, cW_Amt, cVB.nVB_DecShow);

                        W_SAVxCouponUse(oW_ReqData);
                        this.Close();
                    }
                    else
                    {
                        W_SETxFormLock(false);
                        //opbWait.Visible = false;
                        //oW_SP.SP_SHWxMsg(cPayment.tC_XrcRef2, 2);
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemOrUsed"), 1);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCoupon", "oVB_Coupon_Receive : " + oEx.Message); }
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
                oSql.AppendLine($"FROM TFNTCouponDTHis DTHis WITH(NOLOCK)");
                oSql.AppendLine($"WHERE FTCpbFrmSalRef='{cVB.tVB_DocNo}' AND FTCphDocNo='{poCoupon.FTCphDocNo}' AND FTCpdBarCpn='{poCoupon.FTCpdBarCpn}' AND FTCpbStaBook='1'");
                oSql.AppendLine($"GROUP BY FTCpbFrmSalRef,FTCphDocNo,FTCpdBarCpn,FTBchCode");

                return oDB.C_GETaDataQuery<bool>(oSql.ToString()).FirstOrDefault();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCouponRedeem", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
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
                new cLog().C_WRTxLog("wCouponRedeem", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return false;
        }

        private void W_SETxFormLock(bool ptLock)
        {
            this.Invoke(new MethodInvoker(() => {

                ocmAccept.Enabled = !ptLock;
                opnSearch.Enabled = !ptLock;
            }));
        }
        #endregion

        
    }
}
