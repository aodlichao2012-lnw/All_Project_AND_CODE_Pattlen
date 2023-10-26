using AdaPos.Class;
using AdaPos.Control;
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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wSpotCheck : Form
    {
        #region Variable

        private IDisposable oW_Boardcast;
        private cSP oW_SP;
        private cMiFare oW_MiFare;
        private ResourceManager oW_Resource;
        private cmlWristband oW_WbData;
        private bool bW_OpenComPort = false;
        private int nW_Time;

        #endregion End Variable

        #region Constructor

        public wSpotCheck()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_MiFare = new cMiFare();
                oW_SP.SP_PRCxFlickering(this.Handle);

                bW_OpenComPort = oW_MiFare.C_OPNbComPort();

                W_SETxDesign();
                W_SETxText();
                W_SHWxButtonBar();
                //*Net 63-07-31 ปรับตาม Moshi
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                W_PRCxAcceptSignalR();
                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in opnMenu.Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "wSpotCheck : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
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
                new wHome().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Show popup History
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHistory_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxHistorySpotChk();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmHistory_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmMenu_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wSpotCheck", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSpotCheck_Shown(object sender, EventArgs e)
        {
            try
            {
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                otbCardNo.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "wSpotCheck_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSpotCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_MiFare.C_CLOxComPort();

                if (oW_Boardcast != null)
                    oW_Boardcast.Dispose();

                oW_Boardcast = null;
                oW_MiFare = null;
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "wSpotCheck_FormClosing : " + oEx.Message); }
        }

        /// <summary>
        /// Accept
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                W_GETxSpotCheck();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Call Function Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        W_GETxSpotCheck();
                        break;

                    default:    // Call by name
                        W_CALxByName(e);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "otbCardNo_KeyDown : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Keydown : Call By Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                W_CALxByName(e);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmMenu_KeyDown : " + oEx.Message); }
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
                // W_CHKxNotify();
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "ocmNotify_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "otmRead_Tick : " + oEx.Message); }
        }

        #endregion End Event

        #region Function / Method

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                ocmAccept.BackColor = cVB.oVB_ColNormal;

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;

                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode,"TCNMUser");
                opbLogo.Image = new cCompany().C_GEToImageLogo();

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resSpotCheck_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSpotCheck_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "SPOTCHECK";

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                olaSpotCheck.Text = oW_Resource.GetString("tSpotCheck");
                olaTitleWBNo.Text = oW_Resource.GetString("tWBNo");
                olaTitleNo.Text = oW_Resource.GetString("tWBNo");
                olaTitleName.Text = oW_Resource.GetString("tName");
                olaTitleValue.Text = oW_Resource.GetString("tValue");
                olaTitleDeposit.Text = oW_Resource.GetString("tDeposit");
                olaTitleExpire.Text = oW_Resource.GetString("tExp");
                olaTitleCrdSta.Text = oW_Resource.GetString("tCrdSta");
                olaTitlePdtDeposit.Text = oW_Resource.GetString("tPdtDepo");
                olaTitleAvailable.Text = oW_Resource.GetString("tAvailable");
                olaTitleType.Text = oW_Resource.GetString("tType");

                olaUsrName.Text = new cUser().C_GETtUsername();

                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_SETxText : " + oEx.Message); }
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
                        // [Pong][2019-03-01][Check Status MiFare]
                        //  bMiFare = oW_MiFare.C_OPNbComPort();
                        if (bW_OpenComPort)
                        {
                            oReqSpotChk = new cmlReqSpotChk();
                            oReqSpotChk.pcAvailable = oW_WbData.cAvailable.SP_CHKcDoubleNull();
                            oReqSpotChk.pnLngID = cVB.nVB_Language;
                            oReqSpotChk.pnTxnOffline = oW_WbData.nTxnOffline;
                            oReqSpotChk.ptBchCode = cVB.tVB_BchCode;
                            oReqSpotChk.ptCrdCode = otbCardNo.Text;
                        } else {
                            oReqSpotChk = new cmlReqSpotChk();
                            oReqSpotChk.ptBchCode = cVB.tVB_BchCode;
                            oReqSpotChk.ptCrdCode = otbCardNo.Text;
                        }

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
                                        ocmHistory.Enabled = true;
                                        ocmHistory.BackColor = cVB.oVB_ColNormal;

                                        if (bW_OpenComPort)
                                        {
                                            //[Pong][2019-01-15][ตรวจสอบสถานะ Online Offline]
                                            if (oResSpotChk.rnTxnOffline < oW_WbData.nTxnOffline)
                                            {
                                                cValue = oW_WbData.cDeposit.SP_CHKcDoubleNull() + oW_WbData.cDepositItem.SP_CHKcDoubleNull() + oW_WbData.cAvailable.SP_CHKcDoubleNull();

                                                olaNo.Text = otbCardNo.Text;
                                                olaType.Text = oResSpotChk.rtCtyName;
                                                olaName.Text = oResSpotChk.rtCrdName;
                                                olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow);
                                                olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                                olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cAvailable.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                                olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                                olaStaCrd.Text = W_GETtStaCard(oResSpotChk.rtCrdStaActive);
                                            }
                                            else
                                            {
                                                olaNo.Text = otbCardNo.Text;
                                                olaType.Text = oResSpotChk.rtCtyName;
                                                olaName.Text = oResSpotChk.rtCrdName;
                                                olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValue.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                                olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                                olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValueAvb.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                                olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                                olaStaCrd.Text = W_GETtStaCard(oResSpotChk.rtCrdStaActive);
                                            }
                                        }
                                        else
                                        {
                                            olaNo.Text = otbCardNo.Text;
                                            olaType.Text = oResSpotChk.rtCtyName;
                                            olaName.Text = oResSpotChk.rtCrdName;
                                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValue.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValueAvb.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDepositPdt.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaStaCrd.Text = W_GETtStaCard(oResSpotChk.rtCrdStaActive);
                                        }
                                     
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
                        //*Net 63-07-31 ปรับตาม Moshi
                        oCall.C_PRCxCloseConn();    //*Em 63-07-18
                    }
                    else            // Offline
                    {
                        ocmHistory.Enabled = false;
                        ocmHistory.BackColor = Color.LightGray;

                        if (cVB.bVB_ReadWriteWB)     // เปิดใช้งาน Option : Read Write Wristband
                        {
                            cValue = oW_WbData.cDeposit.SP_CHKcDoubleNull() + oW_WbData.cDepositItem.SP_CHKcDoubleNull() + oW_WbData.cAvailable.SP_CHKcDoubleNull();
                            olaNo.Text = oW_WbData.tUID;
                            olaName.Text = "-";
                            olaType.Text = "-";
                            olaExpire.Text = "-";
                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow);
                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                            olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cDepositItem.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oW_WbData.cAvailable.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                        }
                        else
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantService"), 3);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_GETxSpotCheck : " + oEx.Message); }
            finally
            {
                otbCardNo.Focus();
                otbCardNo.Clear();
                otmRead.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptSta"></param>
        /// <returns></returns>
        private string W_GETtStaCard(string ptSta)
        {
            string tResutl = "";
            try
            {
                switch (ptSta)
                {
                    case "1":
                        tResutl = oW_Resource.GetString("tActive");
                        break;
                    case "2":
                        tResutl = oW_Resource.GetString("tNoActive");
                        break;
                    case "3":
                        tResutl = oW_Resource.GetString("tCancel");
                        break;
                    default:
                        tResutl = "";
                        break;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_GETtStaCard : " + oEx.Message); }
            finally {
            }
            return tResutl;
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                //oW_SP.SP_CLExMemory();
            }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_SETxOpenCloseMenu : " + oEx.Message); }
            finally
            {
                otbCardNo.Focus();
            }
        }

        /// <summary>
        /// Call By Name
        /// </summary>
        private void W_CALxByName(KeyEventArgs poKey)
        {
            string tFuncName;

            try
            {
                // Call by name
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(poKey);
                cVB.tVB_KbdCallByName = tFuncName;
                new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                W_GETxFuncByFuncName(tFuncName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_CALxByName : " + oEx.Message); }
            finally
            {
                poKey = null;
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get function in form 
        /// </summary>
        private void W_GETxFuncByFuncName(string ptFuncName)
        {
            try
            {
                switch (ptFuncName)
                {
                    case "C_KBDxMenu":
                        W_SETxOpenCloseMenu();
                        break;

                    case "C_KBDxBack":
                        new wHome().Show();
                        otmClose.Start();
                        break;

                    case "C_KBDxNotify":
                      //  W_CHKxNotify();
                        cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
                        break;

                    case "C_KBDxAccept":
                        W_GETxSpotCheck();
                        break;

                    case "C_KBDxInputWristband":
                        otbCardNo.Focus();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_PRCxAcceptSignalR : " + oEx.Message); }
        }

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
                            cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                        }));
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_CHKxMsg : " + oEx.Message); }
        }

        /// <summary>
        /// Clear Data
        /// </summary>
        private void W_SETxClearData()
        {
            try
            {
                olaNo.Text = otbCardNo.Text;
                olaType.Text = "-";
                olaName.Text = "-";
                olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaValue.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaExpire.Text = "-";
                olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaPdtDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaStaCrd.Text = "-";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSpotCheck", "W_SETxClearData : " + oEx.Message); }
        }

        #endregion End Function / Method
    }
}
