using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Required;
using AdaPos.Models.Webservice.Respond;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wCancelTopUp : Form
    {
        #region Variable

        private cSP oW_SP;
        private cMiFare oW_MiFare;
        private ResourceManager oW_Resource;
        private cmlWristband oW_WbData;
        private int nW_Time;
        private bool bW_ChkAccept;
        private bool bW_OpenComPort = false;

        #endregion End Variable

        #region Constructor

        public wCancelTopUp()
        {
            InitializeComponent();

            try
            {
                this.oW_SP = new cSP();
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                this.oW_MiFare = new cMiFare();
                this.oW_SP.SP_PRCxFlickering(this.Handle);

                this.bW_OpenComPort = this.oW_MiFare.C_OPNbComPort();

                this.W_SETxDesign();
                this.W_SETxText();
                this.W_GENxBanknote();
                cSP.SP_GETxCountNotify(this.olaMsgCount, this.opnNotify);
                foreach(System.Windows.Forms.Control opnCon in opnMenu.Controls)
                {
                    foreach (System.Windows.Forms.Control oC in opnCon.Controls)
                    {
                        oC.MouseLeave += opnMenu_MouseLeave;
                    }
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        #endregion End Constructor

        #region Event

        /// <summary>
        /// GET Notification
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void W_Notification(object s, EventArgs e)
        {
            try
            {
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wHome", "W_Notification : " + oEx.Message);
            }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCancelTopUp_Shown(object sender, EventArgs e)
        {
            try
            {
                this.otbCardNo.Focus();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Closing Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCancelTopUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.otmClose.Stop();
                this.oW_MiFare.C_CLOxComPort();

                this.oW_MiFare = null;
                this.oW_Resource = null;
                this.oW_SP.SP_CLExMemory();
                this.oW_SP = null;

                this.Dispose();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                new wHome().Show();
                this.otmClose.Start();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.opnMenu.Width <= 100)
                    this.opnMenu.Width = 270;
                else
                    this.opnMenu.Width = 50;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Call Keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Show function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDoShowKB();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Open Calculate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxCalculator();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Open popup help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHelp_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxHelp();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Open Popup about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAbout_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAbout();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Timing to Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmClose_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.nW_Time == 5)
                    this.Close();

                this.nW_Time++;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Read Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmRead_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.bW_OpenComPort)
                {
                    this.oW_WbData = this.oW_MiFare.C_GEToReadSID();

                    if (string.IsNullOrEmpty(this.oW_WbData.tMsgError))
                    {
                        this.otbCardNo.Text = this.oW_WbData.tUID;
                        this.otmRead.Stop();
                        this.W_GETxSpotCheck();
                    }
                }
                else
                    this.otmRead.Stop();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// แจ้งเตือน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Scan Wristband / Card No.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                this.W_PRCxAccept();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;

            try
            {
                // Call by name
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                cVB.tVB_KbdCallByName = tFuncName;
                new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                this.W_GETxFuncByFuncName(tFuncName);

                cVB.tVB_KbdScreen = "TOPUP";
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                tFuncName = null;
                this.oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;

            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        this.W_PRCxAccept();
                        break;

                    default:
                        // Call by name
                        tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                        cVB.tVB_KbdCallByName = tFuncName;
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                        this.W_GETxFuncByFuncName(tFuncName);

                        cVB.tVB_KbdScreen = "TOPUP";
                        break;
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                tFuncName = null;
                this.oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// เมื่อมีการเปลี่ยนแปลงหมายเลขริสแบนด์ / บัตร
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbCardNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.bW_ChkAccept = false;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        #endregion End Event
        
        /// <summary>
        /// Gen banknote
        /// </summary>
        private void W_GENxBanknote()
        {
            List<cmlTFNMBankNote> aoBanknote;

            try
            {
                aoBanknote = new cBankNote().C_GETaBanknote();

                if (aoBanknote.Count <= 5)
                    this.opnQuick.RowStyles[1].Height = 0;

                foreach (cmlTFNMBankNote oBanknote in aoBanknote)
                {
                    Button ocmBanknote = new Button();
                    ocmBanknote.Name = "ocm-" + oBanknote.FTBntCode;
                    ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                    ocmBanknote.FlatStyle = FlatStyle.Flat;
                    ocmBanknote.FlatAppearance.BorderSize = 0;
                    ocmBanknote.BackColor = Color.DimGray;
                    ocmBanknote.Margin = new Padding(0, 0, 10, 10);
                    ocmBanknote.ForeColor = Color.White;
                    ocmBanknote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    ocmBanknote.Font = new Font(new FontFamily("Segoe UI Light"), 16f);
                    opnQuick.Controls.Add(ocmBanknote);
                    ocmBanknote = null;
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                aoBanknote = null;
                this.oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                ocmAccept.BackColor = cVB.oVB_ColNormal;
                ocmPayment.BackColor = cVB.oVB_ColNormal;

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;

                //if (this.oW_SP.SP_CHKbConnection())
                //*Net 63-04-01 ยกมาจาก baseline
                if (!String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                {
                    if (this.oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
                        this.opbPOS.Image = Properties.Resources.Online_32;
                    else
                        this.opbPOS.Image = Properties.Resources.Offline_32;
                }
                else
                {
                    this.opbPOS.Image = Properties.Resources.Offline_32;
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Set tex form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        this.oW_Resource = new ResourceManager(typeof(resTopUp_TH));
                        break;

                    default:    // EN
                        this.oW_Resource = new ResourceManager(typeof(resTopUp_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "TOPUP";

                // Menu
                this.ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                this.ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                this.ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                this.ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                this.ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                this.ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                this.olaCancel.Text = oW_Resource.GetString("tCancel");
                this.olaSaleDate.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");
                this.olaTitleNo.Text = oW_Resource.GetString("tNo");
                this.olaTitleName.Text = oW_Resource.GetString("tName");
                this.olaValue.Text = oW_Resource.GetString("tValue");
                this.olaTitleDeposit.Text = oW_Resource.GetString("tDeposit");
                this.olaTitleExpire.Text = oW_Resource.GetString("tExp");
                this.olaTitleAvailable.Text = oW_Resource.GetString("tAvailable");
                this.olaTitleCancel.Text = oW_Resource.GetString("tCancelAmt");
                this.olaQuickAmt.Text = oW_Resource.GetString("tQuick");
                this.ocmPayment.Text = oW_Resource.GetString("tPayment");

                // user
                this.olaUsrName.Text = new cUser().C_GETtUsername();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Process Accept
        /// </summary>
        private void W_PRCxAccept()
        {
            HttpResponseMessage oResponse = null;
            cClientService oCall;
            cmlReqSpotChk oReqSpotChk;
            cmlResSpotChk oResSpotChk;
            DialogResult oDlgRes;
            DateTime dCrdValueExp, dCurDate;
            string tJsonResult, tJson;
            int nCompare;

            try
            {
                if (string.IsNullOrEmpty(otbCardNo.Text))
                {
                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
                    this.otbCardNo.Focus();
                }
                else
                {
                    //if (this.oW_SP.SP_CHKbConnection())
                    //Net 63-04-01 ยกมาจาก baseline
                    if (String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                    {
                        this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrCon"), 3);
                        this.otbCardNo.Focus();
                        return;
                    }
                    if (this.oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
                    {
                        //++++++++++++++++++++++++++++++
                        if (string.IsNullOrEmpty(cVB.tVB_API2FNWallet))
                        {
                            this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoService"), 3);
                            this.otbCardNo.Focus();
                        }
                        else
                        {
                            oCall = new cClientService();
                            oResponse = new HttpResponseMessage();

                            oReqSpotChk = new cmlReqSpotChk();
                            oReqSpotChk.ptBchCode = cVB.tVB_BchCode;
                            oReqSpotChk.ptCrdCode = otbCardNo.Text;

                            tJson = JsonConvert.SerializeObject(oReqSpotChk);

                            try
                            {
                                oResponse = oCall.C_POSToInvoke(cVB.tVB_API2FNWallet + "/SpotCheck/Check", tJson);

                                if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                    oResSpotChk = JsonConvert.DeserializeObject<cmlResSpotChk>(tJsonResult);

                                    switch (oResSpotChk.rtCode)
                                    {
                                        case "1":
                                            switch (oResSpotChk.rtCrdStaActive)
                                            {
                                                case "1":
                                                    // check card expired.
                                                    dCurDate = DateTime.Now.Date;
                                                    nCompare = DateTime.Compare(dCurDate, oResSpotChk.rdCrdExpireDate.GetValueOrDefault());

                                                    if (nCompare > 0)
                                                    {
                                                        // card expired.
                                                        this.oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCrdExp"), 3);
                                                        this.W_CLSxInterfaceCancelTopUp();
                                                        this.otbCardNo.Focus();
                                                        this.otbCardNo.Clear();
                                                        return;
                                                    }

                                                    // check card value expired.
                                                    if (oResSpotChk.rnCtyExpirePeriod > 0 && oResSpotChk.rdCrdLastTopup != null)
                                                    {
                                                        switch (oResSpotChk.rnCtyExpiredType)
                                                        {
                                                            case 1:
                                                                // hour.
                                                                dCrdValueExp = oResSpotChk.rdCrdLastTopup.Value.AddHours(oResSpotChk.rnCtyExpirePeriod);
                                                                dCurDate = DateTime.Now;
                                                                break;
                                                            case 2:
                                                                // day.
                                                                dCrdValueExp = oResSpotChk.rdCrdLastTopup.Value.AddDays(oResSpotChk.rnCtyExpirePeriod);
                                                                break;
                                                            case 3:
                                                                // month.
                                                                dCrdValueExp = oResSpotChk.rdCrdLastTopup.Value.AddMonths(oResSpotChk.rnCtyExpirePeriod);
                                                                break;
                                                            case 4:
                                                                // year.
                                                                dCrdValueExp = oResSpotChk.rdCrdLastTopup.Value.AddYears(oResSpotChk.rnCtyExpirePeriod);
                                                                break;
                                                            default:
                                                                dCrdValueExp = DateTime.Now.Date;
                                                                break;
                                                        }

                                                        nCompare = DateTime.Compare(dCurDate, dCrdValueExp);
                                                        if (nCompare > 0)
                                                        {
                                                            // card value expired.
                                                            oDlgRes = MessageBox.Show(
                                                                oW_Resource.GetString("tMsgCrdValueExp"), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                                            if (oDlgRes == DialogResult.Cancel)
                                                            {
                                                                this.W_CLSxInterfaceCancelTopUp();
                                                                this.otbCardNo.Focus();
                                                                this.otbCardNo.Clear();
                                                                return;
                                                            }
                                                        }
                                                    }

                                                    break;
                                                case "2":
                                                    this.oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCrdInactive"), 3);
                                                    this.W_CLSxInterfaceCancelTopUp();
                                                    this.otbCardNo.Focus();
                                                    this.otbCardNo.Clear();
                                                    return;
                                                case "3":
                                                    this.oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCrdCancel"), 3);
                                                    this.W_CLSxInterfaceCancelTopUp();
                                                    this.otbCardNo.Focus();
                                                    this.otbCardNo.Clear();
                                                    return;
                                            }

                                            this.olaName.Text = oResSpotChk.rtCrdName;
                                            this.olaDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaValue.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValue.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            this.olaAvailable.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValueAvb.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaPdtDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.otbReceived.Focus();
                                            this.bW_ChkAccept = true;
                                            break;

                                        case "800": // Card not found
                                            this.oW_SP.SP_SHWxMsg(string.Format(cVB.oVB_GBResource.GetString("tMsgNotfoundCard"), otbCardNo.Text), 3);
                                            this.otbCardNo.Focus();
                                            this.otbCardNo.Clear();
                                            break;

                                        default:
                                            this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                            this.otbCardNo.Focus();
                                            new cLog().C_WRTxLog(
                                                this.GetType().Name, "W_PRCxAccept : " + oResSpotChk.rtCode + " (" + oResSpotChk.rtDesc + ")");
                                            break;
                                    }
                                }
                                else
                                {
                                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                    this.otbCardNo.Focus();
                                }
                            }
                            catch
                            {
                                this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                this.otbCardNo.Focus();
                            }
                        }
                    }
                    else
                    {
                        this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrCon"), 3);
                        this.otbCardNo.Focus();
                    }
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                if (oResponse != null)
                    oResponse.Dispose();

                oResponse = null;
                oCall = null;
                oReqSpotChk = null;
                oResSpotChk = null;
                tJsonResult = null;
                tJson = null;
                this.oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get function by name
        /// </summary>
        private void W_GETxFuncByFuncName(string ptFuncName)
        {
            try
            {
                switch (ptFuncName)
                {
                    case "C_KBDxBack":
                        //W_PRCxBack();
                        break;

                    case "C_KBDxNotify":
                        cSP.SP_CHKxNotify(this.olaMsgCount, this.opnNotify);
                        break;

                    case "C_KBDxAccept":
                        this.W_PRCxAccept();
                        break;

                    case "C_KBDxInputWristband":
                        this.otbCardNo.Focus();
                        break;

                    case "C_KBDxInputAmount":
                        this.otbReceived.Focus();
                        break;

                    case "C_KBDxPayment":
                        //W_PRCxPayment();
                        break;
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Clear data on interface cancel topup.
        /// </summary>
        private void W_CLSxInterfaceCancelTopUp()
        {
            try
            {
                this.olaName.Text = "-";
                this.olaExpire.Text = "-";
                this.olaDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                this.olaValue.Text = this.oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                this.olaAvailable.Text = this.oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                this.olaPdtDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        /// <summary>
        /// Spot check
        /// </summary>
        private void W_GETxSpotCheck()
        {
            HttpResponseMessage oResponse = null;
            cClientService oCall;
            cmlReqSpotChk oReqSpotChk;
            cmlResSpotChk oResSpotChk;
            string tAPI, tJson, tJsonResult;
            bool bChkConn;
            decimal cValue;

            try
            {
                if (string.IsNullOrEmpty(otbCardNo.Text))
                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
                else
                {
                    tAPI = cVB.tVB_API2FNWallet.Substring(0, cVB.tVB_API2FNWallet.IndexOf("/V"));
           
                    bChkConn = oW_SP.SP_CHKbConnection(tAPI);

                    if (bChkConn)   // Online
                    {
                        oReqSpotChk = new cmlReqSpotChk();
                        oReqSpotChk.pcAvailable = oW_WbData.cAvailable.SP_CHKcDoubleNull();
                        oReqSpotChk.pnLngID = cVB.nVB_Language;
                        oReqSpotChk.pnTxnOffline = oW_WbData.nTxnOffline;
                        oReqSpotChk.ptBchCode = cVB.tVB_BchCode;
                        oReqSpotChk.ptCrdCode = otbCardNo.Text;

                        tJson = JsonConvert.SerializeObject(oReqSpotChk);

                        oCall = new cClientService();
                        oResponse = new HttpResponseMessage();

                        try
                        {
                            oResponse = oCall.C_POSToInvoke(cVB.tVB_API2FNWallet + "/SpotCheck/Check", tJson);

                            if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                oResSpotChk = JsonConvert.DeserializeObject<cmlResSpotChk>(tJsonResult);

                                switch (oResSpotChk.rtCode)
                                {
                                    case "1":

                                        //[Pong][2019-01-15][ตรวจสอบสถานะ Online Offline]
                                        if (oResSpotChk.rnTxnOffline < this.oW_WbData.nTxnOffline)
                                        {
                                            cValue = this.oW_WbData.cDeposit.SP_CHKcDoubleNull() + this.oW_WbData.cDepositItem.SP_CHKcDoubleNull() + this.oW_WbData.cAvailable.SP_CHKcDoubleNull();

                                            this.olaName.Text = oResSpotChk.rtCrdName;
                                            this.olaDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, this.oW_WbData.cDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaValue.Text = this.oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow);
                                            this.olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            this.olaAvailable.Text = this.oW_SP.SP_SETtDecShwSve(1, this.oW_WbData.cAvailable.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaPdtDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                        }
                                        else
                                        {
                                            this.olaName.Text = oResSpotChk.rtCrdName;
                                            this.olaDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaValue.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValue.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            this.olaAvailable.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValueAvb.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaPdtDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                        }
                                        this.bW_ChkAccept = true;
                                        break;

                                    case "800": // Card not found
                                        this.oW_SP.SP_SHWxMsg(string.Format(cVB.oVB_GBResource.GetString("tMsgNotfoundCard"), otbCardNo.Text), 3);
                                        this.W_SETxClearData();
                                        break;

                                    default:
                                        this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                        this.W_SETxClearData();
                                        new cLog().C_WRTxLog("wSpotCheck", "W_GETxSpotCheck : " + oResSpotChk.rtCode + " (" + oResSpotChk.rtDesc + ")");
                                        break;
                                }
                            }
                            else
                                this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                        }
                        catch
                        {
                            this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                        }
                    }
                    else            // Offline
                    {
                        if (cVB.bVB_ReadWriteWB)     // เปิดใช้งาน Option : Read Write Wristband
                        {
                            cValue = this.oW_WbData.cDeposit.SP_CHKcDoubleNull() 
                                + this.oW_WbData.cDepositItem.SP_CHKcDoubleNull() + this.oW_WbData.cAvailable.SP_CHKcDoubleNull();
                            this.olaName.Text = "-";
                            this.olaExpire.Text = "-";
                            this.olaValue.Text = this.oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow); ;
                            this.olaDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, this.oW_WbData.cDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                            this.olaPdtDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, this.oW_WbData.cDepositItem.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                            this.olaAvailable.Text = this.oW_SP.SP_SETtDecShwSve(1, this.oW_WbData.cAvailable.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                        }
                        else
                            this.oW_SP.SP_SHWxMsg(this.oW_Resource.GetString("tMsgCantService"), 3);
                    }
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                this.otbCardNo.Focus();
                this.otmRead.Start();
            }
        }

        /// <summary>
        /// Clear Data
        /// </summary>
        private void W_SETxClearData()
        {
            try
            {
                this.olaName.Text = "-";
                this.olaDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                this.olaValue.Text = this.oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                this.olaExpire.Text = "-";
                this.olaAvailable.Text = this.oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                this.olaPdtDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 245)
            {
                opnMenu.Width = 55;
            }
        }

        ///// <summary>
        ///// Check Notify
        ///// </summary>
        //private void W_CHKxNotify()
        //{
        //    try
        //    {
        //        if (opnNotify.Visible)
        //            opnNotify.Visible = false;
        //        else
        //        {
        //            olaMsgCount.Visible = false;
        //            olaMsgCount.Text = "";
        //            opnNotify.Visible = true;

        //            new cMsgRemind().C_UPDxReadMsg();
        //        }
        //    }
        //    catch (Exception oEx) { new cLog().C_WRTxLog("wCancelTopUp", "W_CHKxNotify : " + oEx.Message); }
        //}
    }
}
