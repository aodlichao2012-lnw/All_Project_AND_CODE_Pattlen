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
    public partial class wReturnCard : Form
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

        public wReturnCard()
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
                W_SETxTempHD();
                onpNumpad.oU_TextValue = otbReceived;
                onpNumpad.tU_TextValue = otbReceived.Text;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReturnCard", "wReturnCard : " + oEx.Message); }
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
                this.W_PRCxBack();
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
                this.W_SETxOpenCloseMenu();
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
            DialogResult oResult;

            try
            {
                oResult = new cFunctionKeyboard().C_KBDoShowKB();

                if (oResult == DialogResult.OK)
                    this.W_GETxFuncByFuncName(cVB.tVB_KbdCallByName);
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
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wReturnCard_Shown(object sender, EventArgs e)
        {
            try
            {
                this.otbCardNo.Focus();

                //*[AnUBiS][][2019-02-07] - generate doucument.
                this.W_GENxDocumentCrdTouUp();
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
        private void wReturnCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.otmClose.Stop();

                if (this.oW_Boardcast != null)
                    this.oW_Boardcast.Dispose();

                this.oW_MiFare.C_CLOxComPort();

                this.oW_MiFare = null;
                this.oW_Boardcast = null;
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
        /// Payment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmPayment_Click(object sender, EventArgs e)
        {
            try
            {
                this.W_PRCxPayment();
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
        /// Check format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbReceived_KeyPress(object sender, KeyPressEventArgs e)
        {
            string[] atDot;

            try
            {
                if ((!char.IsDigit(e.KeyChar) && e.KeyChar != '.') || (e.KeyChar == '.' && this.otbReceived.Text.Contains(".")))
                    e.Handled = true;

                atDot = this.otbReceived.Text.Split('.');

                if (atDot.Length > 1)
                {
                    if (atDot[1].Length >= 2)
                        e.Handled = true;
                }

                if (e.KeyChar == (char)Keys.Back)
                    e.Handled = false;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                atDot = null;
                this.oW_SP.SP_CLExMemory();
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

                if (!string.IsNullOrEmpty(this.otbReceived.Text))
                    cReceived = Convert.ToDecimal(this.otbReceived.Text);

                this.otbReceived.Text = oW_SP.SP_SETtDecShwSve(1, cReceived + cValue, cVB.nVB_DecShow);
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
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

        private void wReturnCard_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    oW_SP = new cSP();
            //    //oW_MiFare = new cMiFare();
            //    oW_SP.SP_PRCxFlickering(this.Handle);

            //    //bW_OpenComPort = oW_MiFare.C_OPNbComPort();

            //    W_SETxDesign();
            //    W_SETxText();
            //    W_GENxBanknote();
            //    W_SHWxButtonBar();
            //    cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            //    W_PRCxAcceptSignalR();
            //    W_SETxTempHD();
            //    onpNumpad.oU_TextValue = otbReceived;
            //    onpNumpad.tU_TextValue = otbReceived.Text;
            //}
            //catch (Exception oEx) { new cLog().C_WRTxLog("wReturnCard", "wReturnCard : " + oEx.Message); }
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
                this.ocmAccept.BackColor = cVB.oVB_ColNormal;
                this.ocmPayment.BackColor = cVB.oVB_ColNormal;

                this.opnMenu.Width = 50;
                this.opnMenu.BackColor = cVB.oVB_ColDark;
                this.ocmMenu.BackColor = cVB.oVB_ColDark;
                this.ocmKB.BackColor = cVB.oVB_ColDark;
                this.ocmCalculate.BackColor = cVB.oVB_ColDark;
                this.ocmShwKb.BackColor = cVB.oVB_ColDark;
                this.ocmHelp.BackColor = cVB.oVB_ColDark;
                this.ocmAbout.BackColor = cVB.oVB_ColDark;
                this.ocmBack.BackColor = cVB.oVB_ColDark;

                this.opbLogo.Image = new cCompany().C_GEToImageLogo();
                this.opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode,"TCNMUser");

                if (this.opbLogo.Image != null)
                    this.opbLogo.Visible = true;

                //if (this.oW_SP.SP_CHKbConnection())
                //    this.opbPOS.Image = Properties.Resources.Online_32;
                //else
                //    this.opbPOS.Image = Properties.Resources.Offline_32;
                if (!String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                {
                    if (oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
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
                        this.oW_Resource = new ResourceManager(typeof(resReturnCard_TH));
                        break;

                    default:    // EN
                        this.oW_Resource = new ResourceManager(typeof(resReturnCard_EN));
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

                this.olaReturnCard.Text = this.oW_Resource.GetString("tReturnCard");
                this.olaSaleDate.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");
                this.olaTitleNo.Text = this.oW_Resource.GetString("tNo");
                this.olaTitleName.Text = this.oW_Resource.GetString("tName");
                this.olaTitleValue.Text = this.oW_Resource.GetString("tValue");
                this.olaTitleDeposit.Text = this.oW_Resource.GetString("tDeposit");
                this.olaTitleExpire.Text = this.oW_Resource.GetString("tExp");
                this.olaTitleAvailable.Text = this.oW_Resource.GetString("tAvailable");
                this.olaTitleReceived.Text = this.oW_Resource.GetString("tReceived");
                this.olaQuickAmt.Text = this.oW_Resource.GetString("tQuick");
                this.ocmPayment.Text = this.oW_Resource.GetString("tPayment");
                this.olaTitlePdtDeposit.Text = this.oW_Resource.GetString("tPdtDeposit");

                this.olaUsrName.Text = new cUser().C_GETtUsername();
                this.olaPos.Text = cVB.tVB_PosCode;

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                if (!string.IsNullOrEmpty(cVB.tVB_CardNo))
                {
                    this.otbCardNo.Text = cVB.tVB_CardNo;
                    this.otbReceived.Text = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_Amount, cVB.nVB_DecShow);
                    this.W_PRCxAccept();
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
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
        private void W_SETxOpenCloseMenu()
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
        /// Get function by name
        /// </summary>
        private void W_GETxFuncByFuncName(string ptFuncName)
        {
            try
            {
                switch (ptFuncName)
                {
                    case "C_KBDxBack":
                        this.W_PRCxBack();
                        break;

                    case "C_KBDxNotify":
                        cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
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
                        this.W_PRCxPayment();
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
        /// Get Notify
        /// </summary>
        private void W_PRCxAcceptSignalR()
        {
            try
            {
                //this.oW_Boardcast = cVB.oVB_HubProxyAI.On<string>(cVB.tVB_SgnAIBoardcast, (W_CHKxMsg));
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
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
                            cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                        }));
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
                if (string.IsNullOrEmpty(this.otbCardNo.Text))
                {
                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
                    this.otbCardNo.Focus();
                }
                else
                {
                    //if (this.oW_SP.SP_CHKbConnection())
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
                            this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoService"), 3);
                            this.otbCardNo.Focus();
                        }
                        else
                        {
                            oCall = new cClientService();
                            oResponse = new HttpResponseMessage();

                            oReqSpotChk = new cmlReqSpotChk();
                            oReqSpotChk.ptBchCode = cVB.tVB_BchCode;
                            oReqSpotChk.ptCrdCode = this.otbCardNo.Text;

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

                                            this.olaName.Text = oResSpotChk.rtCrdName;
                                            this.olaDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, (decimal)oResSpotChk.rcCrdDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaValue.Text = this.oW_SP.SP_SETtDecShwSve(1, (decimal)oResSpotChk.rcTxnValue.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            this.olaAvailable.Text = this.oW_SP.SP_SETtDecShwSve(1, (decimal)oResSpotChk.rcTxnValueAvb.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.olaPdtDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, (decimal)oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            this.otbReceived.Focus();
                                            this.bW_ChkAccept = true;
                                            break;

                                        case "800": // Card not found
                                            this.oW_SP.SP_SHWxMsg(string.Format(cVB.oVB_GBResource.GetString("tMsgNotfoundCard"), this.otbCardNo.Text), 3);
                                            this.otbCardNo.Focus();
                                            this.otbCardNo.Clear();
                                            break;

                                        default:
                                            this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                            this.otbCardNo.Focus();
                                            new cLog().C_WRTxLog("wReturnCard", "W_PRCxAccept : " + oResSpotChk.rtCode + " (" + oResSpotChk.rtDesc + ")");
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
        /// Process payment
        /// </summary>
        private void W_PRCxPayment()
        {
            try
            {
                if (string.IsNullOrEmpty(this.otbCardNo.Text))
                {
                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
                    this.otbCardNo.Focus();
                }
                else if (!bW_ChkAccept)
                {
                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgClickAccept"), 3);
                    this.otbCardNo.Focus();
                }
                else if (string.IsNullOrEmpty(this.otbReceived.Text))
                {
                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputAmount"), 3);
                    this.otbReceived.Focus();
                }
                else if (Convert.ToDouble(this.otbReceived.Text) <= 0)
                {
                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgAmountZero"), 3);
                    this.otbReceived.Focus();
                }
                else
                {
                    cVB.tVB_CardNo = otbCardNo.Text;
                    cVB.cVB_Amount = Convert.ToDecimal(this.otbReceived.Text);

                    //*[AnUBiS][][2019-02-06] - check null if not MiFare.
                    if (this.oW_WbData != null)
                    {
                        cVB.nVB_TxnOffline = this.oW_WbData.nTxnOffline;
                    }
                    
                    cVB.cVB_Available = Convert.ToDecimal(this.olaAvailable.Text);
                    //cVB.tVB_DocNo = "-";  //*[AnUBiS][][2019-02-08] - comment code.
                    new wPayment(1).Show();
                    this.otmClose.Start();
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
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
                            this.ocmHelp.Visible = true;
                            this.ocmHelp.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB022":
                            this.ocmShwKb.Visible = true;
                            this.ocmShwKb.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB027":
                            this.ocmCalculate.Visible = true;
                            this.ocmCalculate.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB046":
                            this.ocmKB.Visible = true;
                            this.ocmKB.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB047":
                            this.ocmAbout.Visible = true;
                            this.ocmAbout.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;

                        case "KB003":
                            this.ocmBack.Visible = true;
                            this.ocmBack.Text = "".PadLeft(10) + oKb.FTGdtName;
                            break;
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
                aoKb = null;
                this.oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// สร้างข้อมูลลงตาราง Temp HD
        /// </summary>
        private void W_SETxTempHD()
        {
            StringBuilder oSql;
            cmlTFNTCrdTopUpHD oTempTTHD;
            cmlTCNMCompany oComp;
            cDatabase oDatabase;
            try {
                oTempTTHD = new cmlTFNTCrdTopUpHD();
                oSql = new StringBuilder();
                oComp = new cmlTCNMCompany();
                oDatabase = new cDatabase();

                // Get Company 
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 ISNULL(V.FCVatRate,0) AS FCVatRate ");
                oSql.AppendLine("FROM TCNMComp C WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TCNMVatRate V  WITH(NOLOCK)ON(C.FTvatCode = V.FTVatCode) ");
                oSql.AppendLine("WHERE 1=1 ");
                oSql.AppendLine("AND C.FTCmpCode='" + cVB.tVB_CmpCode + "'");
                oSql.AppendLine("AND V.FDVatStart <= GETDATE()");
                oSql.AppendLine("ORDER BY V.FDVatStart DESC");
                oComp = oDatabase.C_GEToDataQuery<cmlTCNMCompany>(oSql.ToString());

                oTempTTHD.FTBchCode = cVB.tVB_BchCode;
                oTempTTHD.FTCthDocNo = "00001";    // จำลองเอกสารไปก่อน
                oTempTTHD.FTCthDocType = "1";
                // oTempTTHD.FDCthDocDate = getdate;
                oTempTTHD.FTCthDocFunc = "1";
                oTempTTHD.FTPosCode = cVB.tVB_PosCode;
                oTempTTHD.FTUsrCode = cVB.tVB_UsrCode;
                oTempTTHD.FTCthRmk = "";
                oTempTTHD.FTUsrName = cVB.tVB_UsrName;
                oTempTTHD.FTCthStaDoc = "";
                oTempTTHD.FTCthStaPrcDoc = "";
                oTempTTHD.FTShfCode = cVB.tVB_ShfCode;
                oTempTTHD.FDShfSaleDate = Convert.ToDateTime(cVB.tVB_SaleDate);
                oTempTTHD.FNCthStaDocAct = 1;
                // oTempTTHD.FTCthApvCode = "";
                oTempTTHD.FTVatCode = Convert.ToString(oComp.FCVatRate);
                // oTempTTHD.FDCthApvDate = "";
                // oTempTTHD.FCCthAmtTP = "";
                // oTempTTHD.FCCthTotalTP = "";
                // oTempTTHD.FCCthTotalQty = "";
                // oTempTTHD.FTCthStaPrc = "";
                // oTempTTHD.FDLastUpdOn = "";
                // oTempTTHD.FTLastUpdBy = "";
                // oTempTTHD.FDCreateOn = "";
                oTempTTHD.FTCreateBy = cVB.tVB_UsrCode;

                oSql.Clear();
                oSql.AppendLine("INSERT INTO TTHD" + cVB.tVB_PosCode);
                oSql.AppendLine("(");
                oSql.AppendLine("    FTBchCode,FTCthDocNo,FTCthDocType");
                oSql.AppendLine("   ,FDCthDocDate,FTCthDocFunc,FTPosCode");
                oSql.AppendLine("   ,FTUsrCode,FTCthRmk,FTUsrName");
                oSql.AppendLine("   ,FTCthStaDoc,FTCthStaPrcDoc,FTShfCode");
                oSql.AppendLine("   ,FDShfSaleDate,FNCthStaDocAct,FTCthApvCode");
                oSql.AppendLine("   ,FTVatCode,FDCthApvDate,FCCthAmtTP");
                oSql.AppendLine("   ,FCCthTotalTP,FCCthTotalQty,FTCthStaPrc");
                oSql.AppendLine("   ,FDLastUpdOn,FTLastUpdBy,FDCreateOn");
                oSql.AppendLine("   ,FTCreateBy");
                oSql.AppendLine(") ");
                oSql.AppendLine("VALUES");
                oSql.AppendLine("(");
                oSql.AppendLine("    '" + oTempTTHD.FTBchCode + "','" + oTempTTHD.FTCthDocNo + "','" + oTempTTHD.FTCthDocType + "'");
                oSql.AppendLine("   ,GETDATE(), '" + oTempTTHD.FTCthDocFunc + "', '" + oTempTTHD.FTPosCode + "'");
                oSql.AppendLine("   ,'" + oTempTTHD.FTUsrCode + "', '" + oTempTTHD.FTCthRmk + "', '" + oTempTTHD.FTUsrName + "'");
                oSql.AppendLine("   ,'" + oTempTTHD.FTCthStaDoc + "', '" + oTempTTHD.FTCthStaPrcDoc + "', '" + oTempTTHD.FTShfCode + "'");
                oSql.AppendLine("   ,'" + oTempTTHD.FDShfSaleDate + "', '" + oTempTTHD.FNCthStaDocAct + "','" + oTempTTHD.FTCthApvCode + "'");
                oSql.AppendLine("   ,'" + oTempTTHD.FTVatCode + "', '" + oTempTTHD.FDCthApvDate + "','" + oTempTTHD.FCCthAmtTP + "'");
                oSql.AppendLine("   ,'" + oTempTTHD.FCCthTotalTP + "', '" + oTempTTHD.FCCthTotalQty + "','" + oTempTTHD.FTCthStaPrc + "'");
                oSql.AppendLine("   ,'" + oTempTTHD.FDLastUpdOn + "', '" + oTempTTHD.FTLastUpdBy + "',GETDATE()");
                oSql.AppendLine("   ,'" + oTempTTHD.FTCreateBy + "'");
                oSql.AppendLine(")");
                oDatabase.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                oTempTTHD = null;
                oSql = null;
                oComp = null; 
                oDatabase = null;
                this.oW_SP.SP_CLExMemory();
            }
        }
        #endregion End Function / Method

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
                if (string.IsNullOrEmpty(this.otbCardNo.Text))
                    this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
                else
                {
                    tAPI = cVB.tVB_API2FNWallet.Substring(0, cVB.tVB_API2FNWallet.IndexOf("/V"));

                    bChkConn = this.oW_SP.SP_CHKbConnection(tAPI);

                    if (bChkConn)   // Online
                    {
                        oReqSpotChk = new cmlReqSpotChk();
                        oReqSpotChk.pcAvailable = this.oW_WbData.cAvailable.SP_CHKcDoubleNull();
                        oReqSpotChk.pnLngID = cVB.nVB_Language;
                        oReqSpotChk.pnTxnOffline = this.oW_WbData.nTxnOffline;
                        oReqSpotChk.ptBchCode = cVB.tVB_BchCode;
                        oReqSpotChk.ptCrdCode = this.otbCardNo.Text;

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
                                        this.oW_SP.SP_SHWxMsg(string.Format(cVB.oVB_GBResource.GetString("tMsgNotfoundCard"), this.otbCardNo.Text), 3);
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
                            cValue = this.oW_WbData.cDeposit.SP_CHKcDoubleNull() + this.oW_WbData.cDepositItem.SP_CHKcDoubleNull() + this.oW_WbData.cAvailable.SP_CHKcDoubleNull();
                            this.olaName.Text = "-";
                            this.olaExpire.Text = "-";
                            this.olaValue.Text = this.oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow); ;
                            this.olaDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, this.oW_WbData.cDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                            this.olaPdtDeposit.Text = this.oW_SP.SP_SETtDecShwSve(1, this.oW_WbData.cDepositItem.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                            this.olaAvailable.Text = this.oW_SP.SP_SETtDecShwSve(1, this.oW_WbData.cAvailable.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                        }
                        else
                            this.oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantService"), 3);
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
            DataTable oDbTblAutoFmt, oDbTblMaxCode;
            bool bStaPrc;

            try
            {
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
                        this.opnReturnCard.Enabled = false;
                        this.opnBTPayment.Enabled = false;
                        this.otmRead.Stop();

                        this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
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
                            this.opnReturnCard.Enabled = false;
                            this.opnBTPayment.Enabled = false;
                            this.otmRead.Stop();

                            this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                            this.ActiveControl = null;

                            return;
                        }

                        if (int.Parse(oDbTblMaxCode.Rows[0][0].ToString()) < 0)
                        {
                            this.opnSearch.Enabled = false;
                            this.opnReturnCard.Enabled = false;
                            this.opnBTPayment.Enabled = false;
                            this.otmRead.Stop();

                            this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                            this.ActiveControl = null;

                            return;
                        }

                        this.tW_DocCrdTopUp = aoSqlParam.Where(oItem => string.Equals(oItem.ParameterName, "@rtReturnResult"))
                            .Select(oItem => oItem.Value).FirstOrDefault().ToString();
                        cVB.tVB_DocNo = this.tW_DocCrdTopUp;

                        if (string.IsNullOrEmpty(this.tW_DocCrdTopUp))
                        {
                            this.opnSearch.Enabled = false;
                            this.opnReturnCard.Enabled = false;
                            this.opnBTPayment.Enabled = false;
                            this.otmRead.Stop();

                            this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                            this.ActiveControl = null;

                            return;
                        }
                    }
                }
                else
                {
                    this.opnSearch.Enabled = false;
                    this.opnReturnCard.Enabled = false;
                    this.opnBTPayment.Enabled = false;
                    this.otmRead.Stop();

                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                    this.ActiveControl = null;
                }
                
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(
                    this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
                this.opnSearch.Enabled = false;
                this.opnReturnCard.Enabled = false;
                this.opnBTPayment.Enabled = false;
                this.otmRead.Stop();

                this.oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                this.ActiveControl = null;
            }
            finally
            {
                oDatabase = null;
                aoSqlParam = null;
                oDbTblAutoFmt = null;
                oDbTblMaxCode = null;
                this.oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Clear data on interface topup.
        /// </summary>
        private void W_CLSxInterfaceTopUp()
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

    }
}
