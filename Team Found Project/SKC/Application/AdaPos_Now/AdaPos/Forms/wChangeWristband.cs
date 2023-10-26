using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Models.Database;
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
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wChangeWristband : Form
    {
        #region Variable

        private IDisposable oW_Boardcast;
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;
        private bool bW_ChkAccept;
        private string tW_DocCrdVoid, tW_RsnCode;    //*[AnUBiS][][2019-01-30]

        #endregion End Variable

        public wChangeWristband()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);  //*Net 63-07-31 ปรับตาม Moshi
                oW_SP.SP_PRCxFlickering(this.Handle);
                //cSP.SP_GETxCountNotify(this.olaMsgCount, this.opnNotify); //*Net 63-07-31 ปรับตาม Moshi
                W_SETxDesign();
                W_SETxText();
                W_SHWxButtonBar();
                //W_GETxCountNotify(); //*Net 63-07-31 ปรับตาม Moshi
                W_PRCxAcceptSignalR();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "wChangeWristband : " + oEx.Message); }
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
        /// Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxBack();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmBack_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmMenu_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wChangeWristband", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmShwKb_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wChangeWristband_Shown(object sender, EventArgs e)
        {
            try
            {
                //*Net 63-07-31 ปรับตาม Moshi
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                cSP.SP_GETxCountNotify(this.olaMsgCount, this.opnNotify);
                W_GETxCountNotify();

                W_GENxDocumentCrdVoid();    //*[AnUBiS][][2019-01-31] - create document no.

                ogdContent.ClearSelection();
                otbTicketNo.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "wChangeWristband_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wChangeWristband_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();

                if (oW_Boardcast != null)
                    oW_Boardcast.Dispose();

                oW_Boardcast = null;
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "wChangeWristband_FormClosing : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbTicketNo_KeyDown(object sender, KeyEventArgs e)
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

                        cVB.tVB_KbdScreen = "CHANGE";
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "otbTicketNo_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Scan Ticket or Wristband / Card No.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxAccept();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmAccept_Click : " + oEx.Message); }
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

                cVB.tVB_KbdScreen = "CHANGE";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmMenu_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
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
                W_CHKxNotify();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmNotify_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Text Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbTicketNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bW_ChkAccept = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "otbTicketNo_TextChanged : " + oEx.Message); }
        }

        /// <summary>
        /// Change Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(ocbMode.SelectedIndex == 0)  // Ticket
                {
                    opnTicket.BringToFront();
                    opnTicket.Visible = true;
                    opnWristband.Visible = false;
                }
                else    // Wristband
                {
                    opnWristband.BringToFront();
                    opnWristband.Visible = true;
                    opnTicket.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocbMode_SelectedIndexChanged : " + oEx.Message); }
        }

        /// <summary>
        /// Show Ticket Description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmTicketDesc_Click(object sender, EventArgs e)
        {
            try
            {
                opnTicket.BringToFront();
                opnTicket.Visible = true;
                opnWristband.Visible = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmTicketDesc_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbNewWristband_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;

            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        W_PRCxChange();
                        break;

                    default:
                        // Call by name
                        tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                        cVB.tVB_KbdCallByName = tFuncName;
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                        W_GETxFuncByFuncName(tFuncName);

                        cVB.tVB_KbdScreen = "CHANGE";
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "otbNewWristband_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Change Wristband
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmChange_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxChange();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "ocmChange_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Event print change wristband.
        /// </summary>
        /// 
        /// <param name="poSender">Sender.</param>
        /// <param name="poPrnEvnArgs">Print page event args.</param>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-02-01] - add new event.
        /// </remarks>
        private void W_PRNxChangeWristband(object poSender, PrintPageEventArgs poPrnEvnArgs)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine;
            int nStartY = 0, nWidth, nHalfWidth;

            try
            {
                nWidth = Convert.ToInt32(poPrnEvnArgs.Graphics.VisibleClipBounds.Width);
                tLine = "------------------------------------------------------------------------";

                if (cVB.nVB_PaperSize == 1)
                {
                    if (nWidth > 215)
                        nWidth = 215;
                }
                else
                {
                    if (nWidth > 280)
                        nWidth = 280;
                }

                nHalfWidth = nWidth / 2;

                oGraphic = poPrnEvnArgs.Graphics;
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

                // header slip message.
                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg

                // user, terminal and datetime.
                /*
                 * USR: 001   T: 600            01/01/2019 12:12
                 * 
                 */
                oGraphic.DrawString("USR: " + cVB.tVB_UsrCode + "   T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                oGraphic.DrawString(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), cVB.aoVB_PInvLayout[1], Brushes.Black,
                    new RectangleF(0, nStartY, nWidth, 15), new StringFormat { Alignment = StringAlignment.Far });

                // branch..
                /*
                 * Branch: HQ
                 * 
                 */
                nStartY += 15;
                oGraphic.DrawString("Branch: " + cVB.tVB_BchName, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                // doc no.
                /*
                 * No. CV19-00001
                 * 
                 */
                nStartY += 15;
                oGraphic.DrawString("No. " + tW_DocCrdVoid, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                // line.
                /*
                 * ---------------------------------------------
                 * 
                 */
                nStartY += 10;
                oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black,
                    new RectangleF(0, nStartY, nWidth + 17, 15), new StringFormat { Alignment = StringAlignment.Near });

                // detail.
                /*
                 * Change Card.
                 * Card No. 
                 *    From 1234 
                 *    To 2234
                 * 
                 */
                nStartY += 15;
                oGraphic.DrawString(oW_Resource.GetString("tChgWsbPrnChgCrd"), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 15;
                oGraphic.DrawString(oW_Resource.GetString("tChgWsbPrnCrdNo"), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 15;
                oGraphic.DrawString("   " + string.Format(oW_Resource.GetString("tChgWsbPrnCrdFrm"), olaNo.Text), 
                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 15;
                oGraphic.DrawString("   " + string.Format(oW_Resource.GetString("tChgWsbPrnCrdTo"), otbNewWristband.Text), 
                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                // line.
                /*
                 * ---------------------------------------------
                 * 
                 */
                nStartY += 10;
                oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black,
                    new RectangleF(0, nStartY, nWidth + 17, 15), new StringFormat { Alignment = StringAlignment.Near });

                // balance.
                /*
                 *                   Card Balance:          1,000.00
                 *               Card Net Balance:            900.00
                 * 
                 */
                nStartY += 15;
                oGraphic.DrawString("Card Balance: ", cVB.aoVB_PInvLayout[1], Brushes.Black,
                    new RectangleF(0, nStartY, nHalfWidth, 15), new StringFormat { Alignment = StringAlignment.Far });
                oGraphic.DrawString(olaValue.Text, cVB.aoVB_PInvLayout[1], Brushes.Black,
                    new RectangleF(0, nStartY, nWidth, 15), new StringFormat { Alignment = StringAlignment.Far });
                nStartY += 15;
                oGraphic.DrawString("Card Net Balance: ", cVB.aoVB_PInvLayout[2], Brushes.Black,
                    new RectangleF(0, nStartY, nHalfWidth, 15), new StringFormat { Alignment = StringAlignment.Far });
                oGraphic.DrawString(olaAvailable.Text, cVB.aoVB_PInvLayout[2], Brushes.Black,
                    new RectangleF(0, nStartY, nWidth, 15), new StringFormat { Alignment = StringAlignment.Far });

                // footer slip message.
                nStartY += 30;
                oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY);
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                oMsg = null;
                //new cSP().SP_CLExMemory();
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
                ocmTicketDesc.BackColor = cVB.oVB_ColNormal;
                ocmChange.BackColor = cVB.oVB_ColNormal;
                //ogdContent.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdContent); //*Net 63-03-03 Set Design Gridview

                opnPkg.BackColor = cVB.oVB_ColNormal;

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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resChangeWristband_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resChangeWristband_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "CHANGE";

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                //*[AnUBiS][][2019-01-28] - set interface by language.
                // Set mode
                ocbMode.Items.Add(oW_Resource.GetString("tModeTicket"));
                ocbMode.Items.Add(oW_Resource.GetString("tModeCrdOrWristband"));
                ocbMode.SelectedIndex = 1;
                ocbMode.Enabled = false;

                // Set interface title by language.
                olaChange.Text = oW_Resource.GetString("tChange");
                olaTitleMode.Text = oW_Resource.GetString("tChgWsbMode");
                olaTitleNo.Text = oW_Resource.GetString("tChgWsbWBNo");
                olaTitleName.Text = oW_Resource.GetString("tChgWsbName");
                olaTitleType.Text = oW_Resource.GetString("tChgWsbType");
                olaTitleExpire.Text = oW_Resource.GetString("tChgWsbDateExp");
                olaTitleValue.Text = oW_Resource.GetString("tChgWsbValue");
                olaTitleDeposit.Text = oW_Resource.GetString("tChgWsbDeposit");
                olaTitleAvailable.Text = oW_Resource.GetString("tChgWsbAvailable");
                olaTitleNewTKNo.Text = oW_Resource.GetString("tChgWsbNewWBNo");

                if (ocbMode.SelectedIndex == 0)
                    olaTitleTKNo.Text = oW_Resource.GetString("tChgWsbTKNo");
                else
                    olaTitleTKNo.Text = oW_Resource.GetString("tChgWsbWBNo");

                olaUsrName.Text = new cUser().C_GETtUsername();

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Process Back
        /// </summary>
        private void W_PRCxBack()
        {
            try
            {
                new wHome().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_PRCxBack : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_SETxOpenCloseMenu : " + oEx.Message); }
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
                        W_CHKxNotify();
                        break;

                    case "C_KBDxAccept":
                        W_PRCxAccept();
                        break;

                    case "C_KBDxInputNo":
                        otbTicketNo.Focus();
                        break;

                    case "C_KBDxInputNewNo":
                        otbNewWristband.Focus();
                        break;

                    case "C_KBDxChange":
                        W_PRCxChange();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_GETxFuncByFuncName : " + oEx.Message); }
        }

        /// <summary>
        /// Get Notify
        /// </summary>
        private void W_PRCxAcceptSignalR()
        {
            if (string.IsNullOrEmpty(cVB.tVB_SgnRPosSrv)) return;   //*Em 62-01-07  WaterPark
            try
            {
                //oW_Boardcast = cVB.oVB_HubProxyAI.On<string>(cVB.tVB_SgnAIBoardcast, (W_CHKxMsg));
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_PRCxAcceptSignalR : " + oEx.Message); }
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
                            W_GETxCountNotify();
                        }));
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_CHKxMsg : " + oEx.Message); }
        }

        /// <summary>
        /// Get Notify
        /// </summary>
        private void W_GETxCountNotify()
        {
            List<cmlTCNTMsgRemind> aoMsgRemind;
            int nCountMsg;

            try
            {
                nCountMsg = new cMsgRemind().C_GETnMaxSeq();

                if (nCountMsg == 0)
                    olaMsgCount.Visible = false;
                else
                {
                    olaMsgCount.Text = nCountMsg.ToString();

                    if (opnNotify.Visible)
                        olaMsgCount.Visible = false;
                    else
                        olaMsgCount.Visible = true;
                }

                opnNotify.Controls.Clear();

                aoMsgRemind = new cMsgRemind().C_GETaMsgRemind();

                foreach (cmlTCNTMsgRemind oMsg in aoMsgRemind)
                {
                    opnNotify.Controls.Add(new uNotify(oMsg.FNMsgType, string.Format(oMsg.FTMsgData, oMsg.FTSynName), oMsg.FDCreateOn));
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_GETxCountNotify : " + oEx.Message); }
        }

        /// <summary>
        /// Check Notify
        /// </summary>
        private void W_CHKxNotify()
        {
            try
            {
                if (opnNotify.Visible)
                    opnNotify.Visible = false;
                else
                {
                    olaMsgCount.Visible = false;
                    olaMsgCount.Text = "";
                    opnNotify.Visible = true;

                    new cMsgRemind().C_UPDxReadMsg();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_CHKxNotify : " + oEx.Message); }
        }

        /// <summary>
        /// Accept Ticket or Wristband
        /// </summary>
        private void W_PRCxAccept()
        {
            cClientService oClient;
            cmlReqSpotChk oReqSpotChk;
            cmlResSpotChk oResSpotChk;
            HttpResponseMessage oHttpResponse;
            string tJsonReq, tJsonResult;

            try
            {
                //*[AnUBiS][][2019-01-28] - เพิ่ม code การทำงานดึงข้อมูลมาแสดงบนหน้าจอ
                if (string.IsNullOrEmpty(otbTicketNo.Text))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tChgWsbMsgInputWristband"), 3);
                    otbTicketNo.Focus();
                }
                else
                {
                    //if (oW_SP.SP_CHKbConnection())
                    //*Net 63-04-01 ยกมาจาก baseline
                    if (String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgErrCon"), 3);
                        otbTicketNo.Focus();
                        return;
                    }
                    if (oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
                    {
                        //++++++++++++++++++++++++++++++++
                        if (string.IsNullOrEmpty(cVB.tVB_API2FNWallet))
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoService"), 3);
                            otbTicketNo.Focus();
                        }
                        else
                        {
                            oClient = new cClientService();
                            oHttpResponse = new HttpResponseMessage();

                            oReqSpotChk = new cmlReqSpotChk();
                            oReqSpotChk.ptCrdCode = otbTicketNo.Text;
                            oReqSpotChk.ptBchCode = cVB.tVB_BchCode;
                            oReqSpotChk.pnLngID = cVB.nVB_Language;
                            oReqSpotChk.pcAvailable = 0;
                            oReqSpotChk.pnTxnOffline = 0;

                            tJsonReq = JsonConvert.SerializeObject(oReqSpotChk);

                            try
                            {
                                oHttpResponse = oClient.C_POSToInvoke(cVB.tVB_API2FNWallet + "/SpotCheck/Check", tJsonReq);

                                if (oHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    tJsonResult = oHttpResponse.Content.ReadAsStringAsync().Result;
                                    oResSpotChk = JsonConvert.DeserializeObject<cmlResSpotChk>(tJsonResult);

                                    switch (oResSpotChk.rtCode)
                                    {
                                        case "1":
                                            olaNo.Text = otbTicketNo.Text;
                                            olaName.Text = oResSpotChk.rtCrdName;
                                            olaType.Text = oResSpotChk.rtCtyName;
                                            olaExpire.Text = (oResSpotChk.rdCrdExpireDate != null) ? oResSpotChk.rdCrdExpireDate.Value.ToString("dd/MM/yyyy") : "";
                                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValue.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcCrdDeposit.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, oResSpotChk.rcTxnValueAvb.SP_CHKcDoubleNull(), cVB.nVB_DecShow);
                                            ocmChange.Enabled = true;
                                            otbNewWristband.Enabled = true;
                                            otbNewWristband.Clear();
                                            otbNewWristband.Focus();
                                            bW_ChkAccept = true;
                                            break;

                                        case "800": // Card not found
                                            olaNo.Text = "-";
                                            olaName.Text = "-";
                                            olaType.Text = "-";
                                            olaExpire.Text = "-";
                                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                                            otbTicketNo.Focus();
                                            otbTicketNo.Clear();
                                            oW_SP.SP_SHWxMsg(string.Format(oW_Resource.GetString("tMsgNotfoundCard"), oReqSpotChk.ptCrdCode), 3);
                                            break;

                                        default:
                                            olaNo.Text = "-";
                                            olaName.Text = "-";
                                            olaType.Text = "-";
                                            olaExpire.Text = "-";
                                            olaValue.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                                            olaDeposit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                                            olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                                            otbTicketNo.Focus();
                                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantService"), 3);
                                            new cLog().C_WRTxLog("wTopUp", "W_PRCxAccept : " + oResSpotChk.rtCode + " (" + oResSpotChk.rtDesc + ")");
                                            break;
                                    }
                                }
                                else
                                {
                                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantService"), 3);
                                    otbTicketNo.Focus();
                                }
                            }
                            catch
                            {
                                oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantService"), 3);
                                otbTicketNo.Focus();
                            }
                        }
                    }
                    else
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgErrCon"), 3);
                        otbTicketNo.Focus();
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_PRCxAccept : " + oEx.Message); }
            finally
            {
                oClient = null; 
                oReqSpotChk = null;
                oResSpotChk = null;
                oHttpResponse = null;
                tJsonReq = null;
                tJsonResult = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Change Wristband
        /// </summary>
        private void W_PRCxChange()
        {
            cClientService oClient;
            cmlReqChgCrd oReqChgCrd;
            List<cmlReqChgCrd> aoReqChgCrd;
            cmlResChgCrd oResChgCrd;
            HttpResponseMessage oHttpResponse;
            wReason oReason;
            string tJsonReq, tJsonResult;

            try
            {
                if (string.IsNullOrEmpty(otbNewWristband.Text))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tChgWsbMsgInputNewWristband"), 3);
                    otbNewWristband.Focus();
                }
                else
                {
                    //if (oW_SP.SP_CHKbConnection())
                    //*Net 63-04-01 ยกมาจาก baseline
                    if (String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgErrCon"), 3);
                        otbNewWristband.Focus();
                        return;
                    }
                    if (oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
                    {
                        //+++++++++++++++++++++++++++++
                        if (string.IsNullOrEmpty(cVB.tVB_API2FNWallet))
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoService"), 3);
                            otbNewWristband.Focus();
                        }
                        else
                        {
                            oReason = new wReason("007");
                            oReason.ShowDialog();
                            if (oReason.DialogResult == DialogResult.OK)
                            {
                                tW_RsnCode = cVB.oVB_Reason.FTRsnCode;

                                oClient = new cClientService();
                                oHttpResponse = new HttpResponseMessage();

                                oReqChgCrd = new cmlReqChgCrd();
                                oReqChgCrd.ptFrmCrdCode = olaNo.Text;
                                oReqChgCrd.ptToCrdCode = otbNewWristband.Text;
                                oReqChgCrd.ptBchCode = cVB.tVB_BchCode;

                                aoReqChgCrd = new List<cmlReqChgCrd>();
                                aoReqChgCrd.Add(oReqChgCrd);

                                tJsonReq = JsonConvert.SerializeObject(aoReqChgCrd);

                                try
                                {
                                    oHttpResponse = oClient.C_POSToInvoke(cVB.tVB_API2FNWallet + "/Card/ChangeCardLis", tJsonReq);

                                    if (oHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        tJsonResult = oHttpResponse.Content.ReadAsStringAsync().Result;
                                        oResChgCrd = JsonConvert.DeserializeObject<cmlResChgCrd>(tJsonResult);

                                        switch (oResChgCrd.rtCode)
                                        {
                                            case "1":
                                                switch (oResChgCrd.raoChangeCard[0].rtStatus)
                                                {
                                                    case "1":
                                                        // process success.
                                                        otbNewWristband.Enabled = false;
                                                        ocmChange.Enabled = false;

                                                        // save void dt, hd
                                                        if (W_PRCbSaveCrdVoid() == true)
                                                        {
                                                            // print.
                                                            W_PRCxPrintChangeWristband();
                                                        }

                                                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tChgWsbPrcSuccess"), 1);
                                                        break;
                                                    default:
                                                        // process fasle.
                                                        new cLog().C_WRTxLog("wChangeWristband", "W_PRCxChange : " + oResChgCrd.rtCode + " (" + oResChgCrd.rtDesc + ")");
                                                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tChgWsbPrcFalse"), 3);
                                                        break;
                                                }

                                                break;

                                            default:
                                                new cLog().C_WRTxLog("wChangeWristband", "W_PRCxChange : " + oResChgCrd.rtCode + " (" + oResChgCrd.rtDesc + ")");
                                                oW_SP.SP_SHWxMsg(oW_Resource.GetString("tChgWsbPrcFalse"), 3);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantService"), 3);
                                        otbTicketNo.Focus();
                                    }
                                }
                                catch
                                {
                                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantService"), 3);
                                    otbTicketNo.Focus();
                                }
                            }
                        }
                    }
                    else
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgErrCon"), 3);
                        otbNewWristband.Focus();
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_PRCxChange : " + oEx.Message); }
            finally
            {
                oClient = null;
                oReqChgCrd = null;
                aoReqChgCrd = null;
                oResChgCrd = null;
                oHttpResponse = null;
                oReason = null;
                //oW_SP.SP_CLExMemory();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangeWristband", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Save card void header and detail.
        /// </summary>
        /// 
        /// <returns>
        /// true : save success.
        /// false : save false.
        /// </returns>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-01-31] - add new function/method.
        /// </remarks>
        private bool W_PRCbSaveCrdVoid()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("	BEGIN TRANSACTION");
                oSql.AppendLine("		INSERT INTO TFNTCrdVoidDT "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("		(");
                oSql.AppendLine("			FTBchCode, FTCvhDocNo,");
                oSql.AppendLine("			FNCvdSeqNo, FTCvdOldCode,");
                oSql.AppendLine("			FCCvdOldBal, FTCvdNewCode,");
                oSql.AppendLine("			FTCvdStaCrd, FTCvdStaPrc,");
                oSql.AppendLine("			FTCvdRmk, FTRsnCode,");
                oSql.AppendLine("			FDLastUpdOn, FTLastUpdBy,");
                oSql.AppendLine("			FDCreateOn, FTCreateBy");
                oSql.AppendLine("		)");
                oSql.AppendLine("		VALUES");
                oSql.AppendLine("		(");
                oSql.AppendLine("			'" + cVB.tVB_BchCode + "', '" + tW_DocCrdVoid + "',");
                oSql.AppendLine("			1, '" + olaNo.Text + "',");
                oSql.AppendLine("			" + Convert.ToDecimal(olaAvailable.Text) + ", '" + otbNewWristband.Text + "',");
                oSql.AppendLine("			'1', '1',");
                oSql.AppendLine("			NULL, '" + tW_RsnCode + "',");
                oSql.AppendLine("			NULL, NULL,");
                oSql.AppendLine("			GETDATE(), '" + cVB.tVB_UsrName + "'");
                oSql.AppendLine("		);");
                oSql.AppendLine("");
                oSql.AppendLine("		INSERT INTO TFNTCrdVoidHD ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("		(");
                oSql.AppendLine("			FTBchCode, FTCvhDocNo,");
                oSql.AppendLine("			FTCvhDocType, FDCvhDocDate,");
                oSql.AppendLine("			FTUsrCode, FTCvhRmk,");
                oSql.AppendLine("			FTCvhApvCode, FDCvhApvDate,");
                oSql.AppendLine("			FTCvhStaPrcDoc, FNCvhStaDocAct,");
                oSql.AppendLine("			FTCvhStaDoc, FNCvhCardQty,");
                oSql.AppendLine("			FTCvhStaCrdActive, FDLastUpdOn,");
                oSql.AppendLine("			FDCreateOn, FTLastUpdBy,");
                oSql.AppendLine("			FTCreateBy");
                oSql.AppendLine("		)");
                oSql.AppendLine("		VALUES");
                oSql.AppendLine("		(");
                oSql.AppendLine("			'" + cVB.tVB_BchCode + "', '" + tW_DocCrdVoid + "',");
                oSql.AppendLine("			'2', CONVERT(VARCHAR(10), GETDATE() ,120),");
                oSql.AppendLine("			'" + cVB.tVB_UsrCode + "', NULL,");
                oSql.AppendLine("			'1', CONVERT(VARCHAR(10), GETDATE() ,120),");
                oSql.AppendLine("			'1', '1',");
                oSql.AppendLine("			'1', 1,");
                oSql.AppendLine("			NULL, NULL,");
                oSql.AppendLine("			GETDATE(), NULL,");
                oSql.AppendLine("			'" + cVB.tVB_UsrName + "'");
                oSql.AppendLine("		)");
                oSql.AppendLine("	COMMIT");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("	ROLLBACK");
                oSql.AppendLine("END CATCH");

                new cDatabase().C_SETxDataQuery(oSql.ToString());

                return true;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);

                return false;
            }
            finally
            {
                oSql = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Generate ducument card void.
        /// </summary>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-01-31] - add new function/method.
        /// </remarks>
        private void W_GENxDocumentCrdVoid()
        {
            cDatabase oDatabase;
            SqlParameter[] aoSqlParam;
            DataTable oDbTblAutoFmt;
            bool bStaPrc;

            try
            {
                //*[AnUBiS][][2019-04-11] - ปรับให้เรียก stored generate document. ตัวใหม่
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptField", SqlDbType.VarChar, 50){ Value = "FTCvhDocNo"},
                    new SqlParameter ("@ptTableName", SqlDbType.VarChar, 50){ Value = "TFNTCrdVoidHD"},
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
                    if (Convert.ToInt32(oDbTblAutoFmt.Rows[0][0].ToString()) < 0)
                    {
                        this.otbTicketNo.Enabled = false;
                        this.otbNewWristband.Enabled = false;
                        this.ocmAccept.Enabled = false;
                        this.ocmChange.Enabled = false;
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);

                        return;
                    }

                    tW_DocCrdVoid = aoSqlParam.Where(oItem => string.Equals(oItem.ParameterName, "@ptReturnResult"))
                        .Select(oItem => oItem.Value).FirstOrDefault().ToString();
                    cVB.tVB_DocNo = tW_DocCrdVoid;

                    if (string.IsNullOrEmpty(tW_DocCrdVoid))
                    {
                        this.otbTicketNo.Enabled = false;
                        this.otbNewWristband.Enabled = false;
                        this.ocmAccept.Enabled = false;
                        this.ocmChange.Enabled = false;
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);

                        return;
                    }
                }
                else
                {
                    this.otbTicketNo.Enabled = false;
                    this.otbNewWristband.Enabled = false;
                    this.ocmAccept.Enabled = false;
                    this.ocmChange.Enabled = false;
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                }

                this.otbTicketNo.Text = tW_DocCrdVoid;

                //*[AnUBiS][][2019-04-11] - comment code. use this above code.
                /*
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptField", SqlDbType.NVarChar, 50){ Value = "FTCvhDocNo"},
                    new SqlParameter ("@ptTableName", SqlDbType.VarChar, 50){ Value = "TFNTCrdVoidHD"},
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
                        otbTicketNo.Enabled = false;
                        otbNewWristband.Enabled = false;
                        ocmAccept.Enabled = false;
                        ocmChange.Enabled = false;
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);

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
                            otbTicketNo.Enabled = false;
                            otbNewWristband.Enabled = false;
                            ocmAccept.Enabled = false;
                            ocmChange.Enabled = false;
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);

                            return;
                        }

                        if (int.Parse(oDbTblMaxCode.Rows[0][0].ToString()) < 0)
                        {
                            otbTicketNo.Enabled = false;
                            otbNewWristband.Enabled = false;
                            ocmAccept.Enabled = false;
                            ocmChange.Enabled = false;
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);

                            return;
                        }

                        tW_DocCrdVoid = aoSqlParam.Where(oItem => string.Equals(oItem.ParameterName, "@rtReturnResult"))
                            .Select(oItem => oItem.Value).FirstOrDefault().ToString();
                    }
                }
                else
                {
                    otbTicketNo.Enabled = false;
                    otbNewWristband.Enabled = false;
                    ocmAccept.Enabled = false;
                    ocmChange.Enabled = false;
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
                }
                */
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
                this.otbTicketNo.Enabled = false;
                this.otbNewWristband.Enabled = false;
                this.ocmAccept.Enabled = false;
                this.ocmChange.Enabled = false;
                oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotCreateDoc"), 3);
            }
            finally
            {
                oDatabase = null;
                aoSqlParam = null;
                oDbTblAutoFmt = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process print change wristband.
        /// </summary>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-02-01] - add new function/method.
        /// </remarks>
        private void W_PRCxPrintChangeWristband()
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;

            try
            {
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();

                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                oDoc.PrintController = new StandardPrintController();
                oDoc.PrintPage += W_PRNxChangeWristband;
                oDoc.Print();
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                //oW_SP.SP_CLExMemory();
            }
        }
    }
}
