using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Required;
using AdaPos.Models.Webservice.Respond;
using AdaPos.Resources_String.Local;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wTopUp : Form
    {
        #region Variable

        private IDisposable oW_Boardcast;
        private cSP oW_SP;
        private cMiFare oW_MiFare;
        private ResourceManager oW_Resource;
        private cmlWristband oW_WbData;
        private int nW_Time;
        private bool bW_ChkAccept;
        private bool bW_OpenComPort = false;
        private string tW_DocCrdTopUp;

        #endregion End Variable

        #region Constructor

        public wTopUp()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_MiFare = new cMiFare();
                oW_SP.SP_PRCxFlickering(this.Handle);

                bW_OpenComPort = oW_MiFare.C_OPNbComPort();

                W_SETxDesign();
                W_SETxText();
                W_GENxBanknote();
                W_SHWxButtonBar();
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                W_PRCxAcceptSignalR();
                onpNumpad.oU_TextValue = otbReceived;
                onpNumpad.tU_TextValue = otbReceived.Text;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "wTopUp : " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

        /// <summary>
        /// Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxBack();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmBack_Click : " + oEx.Message); }
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
                W_SETxOpenCloseMenu();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmMenu_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wTopUp", "ocmKB_Click : " + ex.Message); }
        }

        /// <summary>
        /// Show function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            DialogResult oResult;

            try
            {
                oResult = new cFunctionKeyboard().C_KBDoShowKB();

                if (oResult == DialogResult.OK)
                    W_GETxFuncByFuncName(cVB.tVB_KbdCallByName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmShwKb_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wTopUp_Shown(object sender, EventArgs e)
        {
            try
            {
                otbCardNo.Focus();

                //*[AnUBiS][][2019-02-07] - generate doucument.
                W_GENxDocumentCrdTouUp();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "wTopUp_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Closing Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wTopUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();

                if (oW_Boardcast != null)
                    oW_Boardcast.Dispose();

                oW_MiFare.C_CLOxComPort();

                oW_MiFare = null;
                oW_Boardcast = null;
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "wTopUp_FormClosing : " + oEx.Message); }
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
                if (nW_Time == 5)
                    this.Close();

                nW_Time++;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Payment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmPayment_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxPayment();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmPayment_Click : " + oEx.Message); }
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
                        W_PRCxAccept();
                        break;

                    default:
                        // Call by name
                        tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                        cVB.tVB_KbdCallByName = tFuncName;
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                        W_GETxFuncByFuncName(tFuncName);

                        cVB.tVB_KbdScreen = "TOPUP";
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "otbCardNo_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                oW_SP.SP_CLExMemory();
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
                W_PRCxAccept();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmAccept_Click : " + oEx.Message); }
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

                W_GETxFuncByFuncName(tFuncName);

                cVB.tVB_KbdScreen = "TOPUP";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmMenu_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                oW_SP.SP_CLExMemory();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmNotify_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Check format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbReceived_KeyPress(object sender, KeyPressEventArgs e)
        {
            string[] atDot;

            try
            {
                if ((!char.IsDigit(e.KeyChar) && e.KeyChar != '.') || (e.KeyChar == '.' && otbReceived.Text.Contains(".")))
                    e.Handled = true;

                atDot = otbReceived.Text.Split('.');

                if (atDot.Length > 1)
                {
                    if (atDot[1].Length >= 2)
                        e.Handled = true;
                }

                if (e.KeyChar == (char)Keys.Back)
                    e.Handled = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "otbReceived_KeyPress : " + oEx.Message); }
            finally
            {
                atDot = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Banknote 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBanknote_Click(object sender, EventArgs e)
        {
            Button ocmButton;
            decimal cValue, cReceived = 0;

            try
            {
                ocmButton = (Button)sender;
                cValue = Convert.ToDecimal(ocmButton.Text);

                if (!string.IsNullOrEmpty(otbReceived.Text))
                    cReceived = Convert.ToDecimal(otbReceived.Text);

                otbReceived.Text = oW_SP.SP_SETtDecShwSve(1, cReceived + cValue, cVB.nVB_DecShow);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "ocmBanknote_Click : " + oEx.Message); }
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
                bW_ChkAccept = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "otbCardNo_TextChanged : " + oEx.Message); }
        }

        #endregion End Event

        #region Function / Method

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
        /// Gen banknote
        /// </summary>
        private void W_GENxBanknote()
        {
            List<cmlTFNMBankNote> aoBanknote;

            try
            {
                aoBanknote = new cBankNote().C_GETaBanknote();

                if (aoBanknote.Count <= 5)
                    opnQuick.RowStyles[1].Height = 0;

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
                    ocmBanknote.Click += ocmBanknote_Click;
                    opnQuick.Controls.Add(ocmBanknote);
                    ocmBanknote = null;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_GENxBanknote : " + oEx.Message); }
            finally
            {
                aoBanknote = null;
                oW_SP.SP_CLExMemory();
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

                opbLogo.Image = new cCompany().C_GEToImageLogo();
                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode,"TCNMUser");

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;

                //if (oW_SP.SP_CHKbConnection())
                //    opbPOS.Image = Properties.Resources.Online_32;
                //else
                //    opbPOS.Image = Properties.Resources.Offline_32;
                if (!String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                {
                    if (oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
                        opbPOS.Image = Properties.Resources.Online_32;
                    else
                        opbPOS.Image = Properties.Resources.Offline_32;
                }
                else
                    opbPOS.Image = Properties.Resources.Offline_32;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resTopUp_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resTopUp_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "TOPUP";

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                olaTopUp.Text = oW_Resource.GetString("tTopup");
                olaSaleDate.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");
                olaTitleNo.Text = oW_Resource.GetString("tNo");
                olaTitleName.Text = oW_Resource.GetString("tName");
                olaTitleValue.Text = oW_Resource.GetString("tValue");
                olaTitleDeposit.Text = oW_Resource.GetString("tDeposit");
                olaTitleExpire.Text = oW_Resource.GetString("tExp");
                olaTitleAvailable.Text = oW_Resource.GetString("tAvailable");
                olaTitleReceived.Text = oW_Resource.GetString("tReceived");
                olaQuickAmt.Text = oW_Resource.GetString("tQuick");
                ocmPayment.Text = oW_Resource.GetString("tPayment");
                olaTitlePdtDeposit.Text = oW_Resource.GetString("tPdtDeposit");

                olaUsrName.Text = new cUser().C_GETtUsername();
                olaPos.Text = cVB.tVB_PosCode;

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                if (!string.IsNullOrEmpty(cVB.tVB_CardNo))
                {
                    otbCardNo.Text = cVB.tVB_CardNo;
                    otbReceived.Text = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_Amount, cVB.nVB_DecShow);
                    W_PRCxAccept();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Process Back
        /// </summary>
        private void W_PRCxBack()
        {
            try
            {
                cVB.tVB_CardNo = null;
                new wHome().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_PRCxBack : " + oEx.Message); }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
        /// </summary>
        private void W_SETxOpenCloseMenu()
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_SETxOpenCloseMenu : " + oEx.Message); }
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
                        W_PRCxBack();
                        break;

                    case "C_KBDxNotify":
                        cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
                        break;

                    case "C_KBDxAccept":
                        W_PRCxAccept();
                        break;

                    case "C_KBDxInputWristband":
                        otbCardNo.Focus();
                        break;

                    case "C_KBDxInputAmount":
                        otbReceived.Focus();
                        break;

                    case "C_KBDxPayment":
                        W_PRCxPayment();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_GETxFuncByFuncName : " + oEx.Message); }
        }

        /// <summary>
        /// Get Notify
        /// </summary>
        private void W_PRCxAcceptSignalR()
        {
            try
            {
                //oW_Boardcast = cVB.oVB_HubProxyAI.On<string>(cVB.tVB_SgnAIBoardcast, (W_CHKxMsg));
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_PRCxAcceptSignalR : " + oEx.Message); }
        }

        /// <summary>
        /// Check Message
        /// </summary>
        /// <param name="ptMessage"></param>
        private void W_CHKxMsg(string ptMessage)
        {
            try
            {
                switch (ptMessage)
                {
                    case "AdaPosFront|MsgRemind":
                        Invoke(new Action(() =>
                        {
                           // W_GETxCountNotify();  //[Pong][2018-01-28][Comment code]
                            //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                        }));
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_CHKxMsg : " + oEx.Message); }
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
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
                    otbCardNo.Focus();
                }
                else
                {
                    //if (oW_SP.SP_CHKbConnection())
                    if (String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                    {
                        this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrCon"), 3);
                        this.otbCardNo.Focus();
                        return;
                    }
                    if (oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
                    {
                        if (string.IsNullOrEmpty(cVB.tVB_API2FNWallet))
                        {
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoService"), 3);
                            otbCardNo.Focus();
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
                                            //*[AnUBiS][][2019-04-07] - check status card. (Pandora)
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
                                                        this.W_CLSxInterfaceTopUp();
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
                                                                this.W_CLSxInterfaceTopUp();
                                                                this.otbCardNo.Focus();
                                                                this.otbCardNo.Clear();
                                                                return;
                                                            }
                                                        }
                                                    }

                                                    break;
                                                case "2":
                                                    this.oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCrdInactive"), 3);
                                                    this.W_CLSxInterfaceTopUp();
                                                    this.otbCardNo.Focus();
                                                    this.otbCardNo.Clear();
                                                    return;
                                                case "3":
                                                    this.oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCrdCancel"), 3);
                                                    this.W_CLSxInterfaceTopUp();
                                                    this.otbCardNo.Focus();
                                                    this.otbCardNo.Clear();
                                                    return;
                                            }

                                            olaName.Text = oResSpotChk.rtCrdName;
                                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValue.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValueAvb.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            otbReceived.Focus();
                                            bW_ChkAccept = true;
                                            break;

                                        case "800": // Card not found
                                            oW_SP.SP_SHWxMsg(string.Format(cVB.oVB_GBResource.GetString("tMsgNotfoundCard"), otbCardNo.Text), 3);
                                            otbCardNo.Focus();
                                            otbCardNo.Clear();
                                            break;

                                        default:
                                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                            otbCardNo.Focus();
                                            new cLog().C_WRTxLog("wTopUp", "W_PRCxAccept : " + oResSpotChk.rtCode + " (" + oResSpotChk.rtDesc + ")");
                                            break;
                                    }
                                }
                                else
                                {
                                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                    otbCardNo.Focus();
                                }
                            }
                            catch
                            {
                                oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                otbCardNo.Focus();
                            }
                        }
                    }
                    else
                    {
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrCon"), 3);
                        otbCardNo.Focus();
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_PRCxAccept : " + oEx.Message); }
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
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process payment
        /// </summary>
        private void W_PRCxPayment()
        {
            try
            {
                if (string.IsNullOrEmpty(otbCardNo.Text))
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
                    otbCardNo.Focus();
                }
                else if (!bW_ChkAccept)
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgClickAccept"), 3);
                    otbCardNo.Focus();
                }
                else if (string.IsNullOrEmpty(otbReceived.Text))
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputAmount"), 3);
                    otbReceived.Focus();
                }
                else if (Convert.ToDouble(otbReceived.Text) <= 0)
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgAmountZero"), 3);
                    otbReceived.Focus();
                }
                else
                {
                    cVB.tVB_CardNo = otbCardNo.Text;
                    cVB.cVB_Amount = Convert.ToDecimal(otbReceived.Text);

                    //*[AnUBiS][][2019-02-06] - check null if not MiFare.
                    if (oW_WbData != null)
                    {
                        cVB.nVB_TxnOffline = oW_WbData.nTxnOffline;
                    }
                    
                    cVB.cVB_Available = Convert.ToDecimal(olaAvailable.Text);
                    //cVB.tVB_DocNo = "-";  //*[AnUBiS][][2019-02-08] - comment code.

                    // [Pong][2019-05-17][Insert TopUp HD DT]
                    W_SAVxTopUpHDAndDT();

                    new wPayment(1).Show();
                    otmClose.Start();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_PRCxPayment : " + oEx.Message); }
        }

        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;

            try
            {
                aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                foreach (cmlTPSMFunc oKb in aoKb)
                {
                    switch (oKb.FTSysCode)
                    {
                        case "KB010":
                            ocmHelp.Visible = true;
                            ocmHelp.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB022":
                            ocmShwKb.Visible = true;
                            ocmShwKb.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB027":
                            ocmCalculate.Visible = true;
                            ocmCalculate.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB046":
                            ocmKB.Visible = true;
                            ocmKB.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB047":
                            ocmAbout.Visible = true;
                            ocmAbout.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB003":
                            ocmBack.Visible = true;
                            ocmBack.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopup", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Save TopUp HD,DT
        /// </summary>
        private void W_SAVxTopUpHDAndDT()
        {
            StringBuilder oSql;
            cmlTFNTCrdTopUpHD oTopUpHD;
            cmlTFNTCrdTopUpDT oTopUpDT;
            cDatabase oDatabase;
            try {
                oTopUpHD = new cmlTFNTCrdTopUpHD();
                oTopUpDT = new cmlTFNTCrdTopUpDT();
                oSql = new StringBuilder();
                oDatabase = new cDatabase();

                // Call function Gen Doc No.
                cSale.C_GETtFormatDoc("TFNTCrdTopUpHD", 3, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);
                cVB.tVB_DocNo = cSale.C_GENtDocNo(3);
                // เตรียมข้อมูล HD
                oTopUpHD.FTBchCode = cVB.tVB_BchCode;
                oTopUpHD.FTCthDocNo = cVB.tVB_DocNo;

                oTopUpHD.FTCthDocType = "1";
              //  oTopUpHD.FDCthDocDate = DateTime.Now;
                oTopUpHD.FTCthDocFunc = "1";
                oTopUpHD.FTPosCode = cVB.tVB_PosCode;
                oTopUpHD.FTUsrCode = cVB.tVB_UsrCode;     // รหัสแคชเชียร์
                oTopUpHD.FTCthRmk = "";
                oTopUpHD.FTUsrName = cVB.tVB_UsrName;     // ชื่อแคชเชียร์
                oTopUpHD.FTCthStaDoc = "0";
                oTopUpHD.FTCthStaPrcDoc = "";
                oTopUpHD.FTShfCode = cVB.tVB_ShfCode;
              //  oTopUpHD.FDShfSaleDate = DateTime.Parse(cVB.tVB_SaleDate);
                oTopUpHD.FNCthStaDocAct = 1;
                oTopUpHD.FTCthApvCode = "";
                oTopUpHD.FTVatCode = cVB.tVB_VatCode;
                oTopUpHD.FDCthApvDate = null;
                oTopUpHD.FCCthAmtTP = Convert.ToDouble(cVB.cVB_Amount);
                oTopUpHD.FCCthTotalTP = Convert.ToDouble(cVB.cVB_Amount);
                oTopUpHD.FCCthTotalQty = 1;
                oTopUpHD.FTCthStaPrc = "1";
                oTopUpHD.FTCthStaDelMQ = "";
                oTopUpHD.FTLastUpdBy = cVB.tVB_UsrName;
                oTopUpHD.FTCreateBy = cVB.tVB_UsrName;

                // เตรียมข้อมูล DT
                oTopUpDT.FTBchCode = cVB.tVB_BchCode;
                oTopUpDT.FTCthDocNo = cVB.tVB_DocNo;
                oTopUpDT.FNCtdSeqNo = 1;
                oTopUpDT.FTCrdCode = otbCardNo.Text;
                oTopUpDT.FCCtdCrdTP = Convert.ToDouble(cVB.cVB_Amount);
                oTopUpDT.FTCtdStaCrd = "1";
                oTopUpDT.FTCtdStaPrc = "";
                oTopUpDT.FTCtdRmk = "";
                oTopUpDT.FTLastUpdBy = cVB.tVB_UsrName;
                oTopUpDT.FTCreateBy = cVB.tVB_UsrName;

                oSql.Clear();
                oSql.AppendLine("BEGIN TRANSACTION ");
                oSql.AppendLine("  SAVE TRANSACTION Topup ");
                oSql.AppendLine("  BEGIN TRY ");

                oSql.AppendLine("     INSERT INTO TFNTCrdTopUpHD WITH (ROWLOCK)");
                oSql.AppendLine("     (");
                oSql.AppendLine("      FTBchCode,FTCthDocNo,FTCthDocType");
                oSql.AppendLine("     ,FDCthDocDate,FTCthDocFunc,FTPosCode");
                oSql.AppendLine("     ,FTUsrCode,FTCthRmk,FTUsrName");
                oSql.AppendLine("     ,FTCthStaDoc,FTCthStaPrcDoc,FTShfCode");
                oSql.AppendLine("     ,FDShfSaleDate,FNCthStaDocAct,FTCthApvCode");
                oSql.AppendLine("     ,FTVatCode,FDCthApvDate,FCCthAmtTP");
                oSql.AppendLine("     ,FCCthTotalTP,FCCthTotalQty,FTCthStaPrc");
                oSql.AppendLine("     ,FTCthStaDelMQ");
                oSql.AppendLine("     ,FDLastUpdOn,FTLastUpdBy");
                oSql.AppendLine("     ,FDCreateOn,FTCreateBy");
                oSql.AppendLine("     ) VALUES (");
                oSql.AppendLine("      '" + oTopUpHD.FTBchCode + "','" + oTopUpHD.FTCthDocNo + "', '" + oTopUpHD.FTCthDocType + "'");
                oSql.AppendLine("     ,GETDATE(), '" + oTopUpHD.FTCthDocFunc + "','" + oTopUpHD.FTPosCode+ "'");
                oSql.AppendLine("     ,'" + oTopUpHD.FTUsrCode + "', '" + oTopUpHD.FTCthRmk + "','" + oTopUpHD.FTUsrName + "'");
                oSql.AppendLine("     ,'" + oTopUpHD.FTCthStaDoc + "', '" + oTopUpHD.FTCthStaPrcDoc + "','" + oTopUpHD.FTShfCode + "'");
                oSql.AppendLine("     ,'" + cVB.tVB_SaleDate + "', '" + oTopUpHD.FNCthStaDocAct + "','" + oTopUpHD.FTCthApvCode + "'");
                oSql.AppendLine("     ,'" + oTopUpHD.FTVatCode + "', '" + oTopUpHD.FDCthApvDate + "','" + oTopUpHD.FCCthAmtTP + "'");
                oSql.AppendLine("     ,'" + oTopUpHD.FCCthTotalTP + "', '" + oTopUpHD.FCCthTotalQty + "','" + oTopUpHD.FTCthStaPrc + "'");
                oSql.AppendLine("     ,'" + oTopUpHD.FTCthStaDelMQ + "'");
                oSql.AppendLine("     ,GETDATE(),'" + oTopUpHD.FTLastUpdBy + "'");
                oSql.AppendLine("     ,GETDATE(),'" + oTopUpHD.FTCreateBy + "'");
                oSql.AppendLine("     )");

                oSql.AppendLine("     INSERT INTO TFNTCrdTopUpDT WITH (ROWLOCK)");
                oSql.AppendLine("     (");
                oSql.AppendLine("      FTBchCode,FTCthDocNo,FNCtdSeqNo");
                oSql.AppendLine("     ,FTCrdCode,FCCtdCrdTP,FTCtdStaCrd");
                oSql.AppendLine("     ,FTCtdStaPrc,FTCtdRmk");
                oSql.AppendLine("     ,FDLastUpdOn,FTLastUpdBy");
                oSql.AppendLine("     ,FDCreateOn,FTCreateBy");
                oSql.AppendLine("     ) VALUES (");
                oSql.AppendLine("      '" + oTopUpDT.FTBchCode + "', '" + oTopUpDT.FTCthDocNo + "','" + oTopUpDT.FNCtdSeqNo + "'");
                oSql.AppendLine("     ,'" + oTopUpDT.FTCrdCode + "', '" + oTopUpDT.FCCtdCrdTP + "','" + oTopUpDT.FTCtdStaCrd + "'");
                oSql.AppendLine("     ,'" + oTopUpDT.FTCtdStaPrc + "', '" + oTopUpDT.FTCtdRmk + "'");
                oSql.AppendLine("     ,GETDATE(),'" + oTopUpDT.FTLastUpdBy + "'");
                oSql.AppendLine("     ,GETDATE(),'" + oTopUpDT.FTCreateBy + "'");
                oSql.AppendLine("     )");
   
                oSql.AppendLine("     COMMIT TRANSACTION Topup");
                oSql.AppendLine("  END TRY");

                oSql.AppendLine("  BEGIN CATCH");
                oSql.AppendLine("     ROLLBACK TRANSACTION Topup");
                oSql.AppendLine("  END CATCH");

                oDatabase.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopup", "W_SAVxTopUpHDAndDT : " + oEx.Message); }
            finally
            {
                oTopUpHD = null;
                oSql = null;
                oDatabase = null;
                oW_SP.SP_CLExMemory();
            }
        }
        #endregion End Function / Method

        /// <summary>
        /// Read Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmRead_Tick(object sender, EventArgs e)
        {

            try
            {
                if (bW_OpenComPort)
                {
                    oW_WbData = oW_MiFare.C_GEToReadSID();

                    if (string.IsNullOrEmpty(oW_WbData.tMsgError))
                    {
                        otbCardNo.Text = oW_WbData.tUID;
                        otmRead.Stop();
                        W_GETxSpotCheck();
                    }
                }
                else
                    otmRead.Stop();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopup", "otmRead_Tick : " + oEx.Message); }
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
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
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
                                        if (oResSpotChk.rnTxnOffline < oW_WbData.nTxnOffline)
                                        {
                                            cValue = oW_WbData.cDeposit.SP_CHKcDoubleNull() + oW_WbData.cDepositItem.SP_CHKcDoubleNull() + oW_WbData.cAvailable.SP_CHKcDoubleNull();

                                            olaName.Text = oResSpotChk.rtCrdName;
                                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow);
                                            olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cAvailable.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                        }
                                        else
                                        {
                                            olaName.Text = oResSpotChk.rtCrdName;
                                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValue.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValueAvb.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                        }
                                        bW_ChkAccept = true;
                                        break;

                                    case "800": // Card not found
                                        oW_SP.SP_SHWxMsg(string.Format(cVB.oVB_GBResource.GetString("tMsgNotfoundCard"), otbCardNo.Text), 3);
                                        W_SETxClearData();
                                        break;

                                    default:
                                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                        W_SETxClearData();
                                        new cLog().C_WRTxLog("wSpotCheck", "W_GETxSpotCheck : " + oResSpotChk.rtCode + " (" + oResSpotChk.rtDesc + ")");
                                        break;
                                }
                            }
                            else
                                oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                        }
                        catch
                        {
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                        }
                    }
                    else            // Offline
                    {
                        if (cVB.bVB_ReadWriteWB)     // เปิดใช้งาน Option : Read Write Wristband
                        {
                            cValue = oW_WbData.cDeposit.SP_CHKcDoubleNull() + oW_WbData.cDepositItem.SP_CHKcDoubleNull() + oW_WbData.cAvailable.SP_CHKcDoubleNull();
                            olaName.Text = "-";
                            olaExpire.Text = "-";
                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow); ;
                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                            olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cDepositItem.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cAvailable.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                        }
                        else
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantService"), 3);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopup", "W_GETxSpotCheck : " + oEx.Message); }
            finally
            {
                otbCardNo.Focus();
                otmRead.Start();
            }
        }

        /// <summary>
        /// Clear Data
        /// </summary>
        private void W_SETxClearData()
        {
            try
            {
                olaName.Text = "-";
                olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaExpire.Text = "-";
                olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopup", "W_SETxClearData : " + oEx.Message); }
        }

        /// <summary>
        /// Generate ducument card void.
        /// </summary>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-02-07] - add new function/method.
        /// </remarks>
        private void W_GENxDocumentCrdTouUp()
        {
            cDatabase oDatabase;
            SqlParameter[] aoSqlParam;
            DataTable oDbTblAutoFmt;
            bool bStaPrc;

            try
            {
                //*[AnUBiS][][2019-04-10] - ปรับให้เรียก stored generate document. ตัวใหม่
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptField", SqlDbType.VarChar, 50){ Value = "FTCthDocNo"},
                    new SqlParameter ("@ptTableName", SqlDbType.VarChar, 50){ Value = "TFNTCrdTopUpHD"},
                    new SqlParameter ("@ptDocType", SqlDbType.VarChar, 50){ Value = "0"},
                    new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = cVB.tVB_BchCode ?? string.Empty},
                    new SqlParameter ("@ptPos", SqlDbType.VarChar, 5){ Value = cVB.tVB_PosCode ?? string.Empty},
                    new SqlParameter ("@ptShp", SqlDbType.VarChar, 5){ Value = cVB.tVB_ShpCode ?? string.Empty},
                    new SqlParameter ("@ptReturnResult", SqlDbType.VarChar, 50) {
                        Direction = ParameterDirection.Output }
                };

                oDatabase = new cDatabase();
                oDbTblAutoFmt = new DataTable();

                bStaPrc = oDatabase.C_DATbExecuteQueryStoreProcedure(
                    cVB.oVB_Config, "STP_GENtAutoFmtCode", ref aoSqlParam, ref oDbTblAutoFmt);

                if (bStaPrc == true)
                {
                    if (int.Parse(oDbTblAutoFmt.Rows[0][0].ToString()) < 0)
                    {
                        this.opnSearch.Enabled = false;
                        this.opnTopup.Enabled = false;
                        this.opnBTPayment.Enabled = false;
                        this.otmRead.Stop();

                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                        this.ActiveControl = null;

                        return;
                    }

                    tW_DocCrdTopUp = aoSqlParam.Where(oItem => string.Equals(oItem.ParameterName, "@ptReturnResult"))
                        .Select(oItem => oItem.Value).FirstOrDefault().ToString();
                    cVB.tVB_DocNo = tW_DocCrdTopUp;

                    if (string.IsNullOrEmpty(tW_DocCrdTopUp))
                    {
                        this.opnSearch.Enabled = false;
                        this.opnTopup.Enabled = false;
                        this.opnBTPayment.Enabled = false;
                        this.otmRead.Stop();

                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                        this.ActiveControl = null;

                        return;
                    }
                }
                else
                {
                    this.opnSearch.Enabled = false;
                    this.opnTopup.Enabled = false;
                    this.opnBTPayment.Enabled = false;
                    this.otmRead.Stop();

                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                    this.ActiveControl = null;
                }

                //*[AnUBiS][][2019-04-10] - comment code. use this above code.
                /*
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptField", SqlDbType.NVarChar, 50){ Value = "FTCthDocNo"},
                    new SqlParameter ("@ptTableName", SqlDbType.VarChar, 50){ Value = "TFNTCrdTopUpHD"},
                    new SqlParameter ("@ptDocType", SqlDbType.VarChar, 50){ Value = "0"},
                    new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = cVB.tVB_BchCode}
                };

                oDatabase = new cDatabase();
                oDbTblAutoFmt = new DataTable();

                bStaPrc = oDatabase.C_DATbExecuteQueryStoreProcedure(
                    cVB.oVB_Config, "STP_CN_GETtAutoFmtCode", ref aoSqlParam, ref oDbTblAutoFmt);

                if (bStaPrc == true)
                {
                    if (oDbTblAutoFmt == null || oDbTblAutoFmt.Rows.Count == 0)
                    {
                        this.opnSearch.Enabled = false;
                        this.opnTopup.Enabled = false;
                        this.opnBTPayment.Enabled = false;
                        this.otmRead.Stop();

                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                        this.ActiveControl = null;

                        return;
                    }

                    aoSqlParam = new SqlParameter[] {
                        new SqlParameter ("@ptSatStaResetBill", SqlDbType.NVarChar, 1){
                            Value = oDbTblAutoFmt.Rows[0]["FTSatStaResetBill"].ToString()},
                        new SqlParameter ("@ptSatRetFmtChar", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTSatRetFmtChar"].ToString()},
                        new SqlParameter ("@ptSatRetFmtYear", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTSatRetFmtYear"].ToString()},
                        new SqlParameter ("@ptSatRetFmtMonth", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTSatRetFmtMonth"].ToString()},
                        new SqlParameter ("@ptSatRetFmtDay", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTSatRetFmtDay"].ToString()},
                        new SqlParameter ("@ptSatRetFmtNum", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTSatRetFmtNum"].ToString()},
                        new SqlParameter ("@ptSatRetFmtAll", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTSatRetFmtAll"].ToString()},
                        new SqlParameter ("@ptBchCode", SqlDbType.NVarChar, 5){
                            Value = oDbTblAutoFmt.Rows[0]["FTBchCode"].ToString()},
                        new SqlParameter ("@ptField", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTPKField"].ToString()},
                        new SqlParameter ("@ptTableName", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTTableName"].ToString()},
                        new SqlParameter ("@ptDocType", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTDocType"].ToString()},
                        new SqlParameter ("@ptSatDocTypeName", SqlDbType.NVarChar, 50){
                            Value = oDbTblAutoFmt.Rows[0]["FTSatDocTypeName"].ToString()},
                        new SqlParameter ("@rtReturnResult", SqlDbType.NVarChar, 100){
                            Direction = ParameterDirection.Output}
                    };

                    oDbTblMaxCode = new DataTable();
                    bStaPrc = oDatabase.C_DATbExecuteQueryStoreProcedure(
                        cVB.oVB_Config, "STP_CN_GETtMaxCode", ref aoSqlParam, ref oDbTblMaxCode);

                    if (bStaPrc == true)
                    {
                        if (oDbTblMaxCode == null || oDbTblMaxCode.Rows.Count == 0)
                        {
                            this.opnSearch.Enabled = false;
                            this.opnTopup.Enabled = false;
                            this.opnBTPayment.Enabled = false;
                            this.otmRead.Stop();

                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                            this.ActiveControl = null;

                            return;
                        }

                        if (int.Parse(oDbTblMaxCode.Rows[0][0].ToString()) < 0)
                        {
                            this.opnSearch.Enabled = false;
                            this.opnTopup.Enabled = false;
                            this.opnBTPayment.Enabled = false;
                            this.otmRead.Stop();

                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                            this.ActiveControl = null;

                            return;
                        }

                        tW_DocCrdTopUp = aoSqlParam.Where(oItem => string.Equals(oItem.ParameterName, "@rtReturnResult"))
                            .Select(oItem => oItem.Value).FirstOrDefault().ToString();
                        cVB.tVB_DocNo = tW_DocCrdTopUp;

                        if (string.IsNullOrEmpty(tW_DocCrdTopUp))
                        {
                            this.opnSearch.Enabled = false;
                            this.opnTopup.Enabled = false;
                            this.opnBTPayment.Enabled = false;
                            this.otmRead.Stop();

                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                            this.ActiveControl = null;

                            return;
                        }
                    }
                }
                else
                {
                    this.opnSearch.Enabled = false;
                    this.opnTopup.Enabled = false;
                    this.opnBTPayment.Enabled = false;
                    this.otmRead.Stop();

                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                    this.ActiveControl = null;
                }
                */
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
                this.opnSearch.Enabled = false;
                this.opnTopup.Enabled = false;
                this.opnBTPayment.Enabled = false;
                this.otmRead.Stop();

                oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                this.ActiveControl = null;
            }
            finally
            {
                oDatabase = null;
                aoSqlParam = null;
                oDbTblAutoFmt = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Clear data on interface topup.
        /// </summary>
        private void W_CLSxInterfaceTopUp()
        {
            try
            {
                olaName.Text = "-";
                olaExpire.Text = "-";
                olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
            }
            catch (Exception)
            {

            }
        }
    }
}
