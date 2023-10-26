using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Resources_String.Local;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AdaPos
{
    public partial class wHome : Form
    {
        #region Variable

        private IDisposable oW_Boardcast;
        private cSP oW_SP;
        private cRabbitMQ oW_MQ;
        private ResourceManager oW_Resource;
        private string[] atW_RateType;
        private int[] anW_Summary;
        private int nW_ChartSaleSum, nW_ChartPaymentSum;
        private int nW_CurrentPage = 1;
        private int nW_MaxPage = 0;
        private int nW_Time;

        #endregion End Variable

        #region Constructor

        public wHome()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();

                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);

                //Clear Log File /*Zen 63-05-15
                oW_SP.SP_CLExLogFile();

                cVB.nVB_SettingFrom = 2;
                nW_MaxPage = new cFunctionKeyboard().C_GETnPageFuncHome();
                if (cVB.nVB_Check2nd == 1)
                {
                    cVB.oVB_2ndScreen = new wShw2ndScreen();
                }
                W_SETxDesign();
                W_SETxText();
                W_GENxButtonFunc();
                W_SETxDashboard();
                W_SHWxButtonBar();
                //new cPdtPmt().C_PRCxPdtPrice();
                //new cPdtPmt().C_PRCxPdtPmt();
                //new cPdt().C_PRCxPreparePdtSale();  //*Em 63-04-22

                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                //W_PRCxAcceptSignalR();

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "wHome " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        #endregion End Constructor

        #region Event


        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if(System.Windows.Forms.Cursor.Position.X>245)
            {
                opnMenu.Width = 55;
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
                W_SETxOpenCloseMenu();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmMenu_Click " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }


        /// <summary>
        /// Exit Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmExit_Click(object sender, EventArgs e)
        {

            try
            {
                new cFunctionKeyboard().C_KBDxExit();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmExit_Click " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }


        /// <summary>
        /// Dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                W_GETxDashboard();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmDashboard_Click " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// เปิดหน้า Menu เสร็จ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wHome_Shown(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);

                //*Arm 63-02-24
                if (!string.IsNullOrEmpty(cVB.tVB_ShfCode))
                {
                    if (cVB.tVB_SaleDate != DateTime.Today.ToString("yyyy-MM-dd")) // รอบการขายไม่ใช้ของปัจจุบัน
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgShiftDateInvalid"), 1);
                    }
                }
                //++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "wHome_Shown " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Clear Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                atW_RateType = null;
                anW_Summary = null;
                new cSP().SP_CLExMemory();
                oW_SP = null;

                new cFunctionKeyboard().C_KBDxClose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "wHome_FormClosing : " + oEx.Message); }
           
        }


        /// <summary>
        /// Open Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHome_Click(object sender, EventArgs e)
        {
            try
            {
                W_GETxHome();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmHome_Click " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void octSaleSum_PrePaint(object sender, ChartPaintEventArgs e)
        {
            try
            {
                if (nW_ChartSaleSum < octSaleSum.Series[0].Points.Count)
                {
                    octSaleSum.Series[0].Points[nW_ChartSaleSum].Color = e.Chart.Series[0].Points[nW_ChartSaleSum].Color;
                    octSaleSum.Legends[0].CustomItems[nW_ChartSaleSum].MarkerColor = e.Chart.Series[0].Points[nW_ChartSaleSum].Color;
                    nW_ChartSaleSum++;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDashboard", "octSaleSum_PrePaint : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set color marker of chart payment type.
        /// </summary>
        /// 
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Chart paint event.</param>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-01-22] - add new event.
        /// </remarks>
        private void octPayment_PrePaint(object sender, ChartPaintEventArgs e)
        {
            try
            {
                if (nW_ChartPaymentSum < octPayment.Series[0].Points.Count)
                {
                    octPayment.Series[0].Points[nW_ChartPaymentSum].Color = e.Chart.Series[0].Points[nW_ChartPaymentSum].Color;
                    octPayment.Legends[0].CustomItems[nW_ChartPaymentSum].MarkerColor = e.Chart.Series[0].Points[nW_ChartPaymentSum].Color;
                    nW_ChartPaymentSum++;
                }
            }
            catch (Exception oExn) { new cLog().C_WRTxLog("wDashboard", "octPayment_PrePaint : " + oExn.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
            catch (Exception ex) { new cLog().C_WRTxLog("wHome", "ocmKB_Click : " + ex.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        //*Arm 63-02-04  -[Comment Code]
        ///// <summary>
        ///// Show function keyboard
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ocmShwKb_Click(object sender, EventArgs e)
        //{

        //    DialogResult oResult;

        //    try
        //    {
        //        oResult = new cFunctionKeyboard().C_KBDoShowKB();

        //        if (oResult == DialogResult.OK)
        //            W_GETxFuncByFuncName(cVB.tVB_KbdCallByName);

        //        if (ocmHome.BackColor == cVB.oVB_ColNormal)  // เลือกปุ่ม Home
        //            cVB.tVB_KbdScreen = "HOME";
        //        else
        //            cVB.tVB_KbdScreen = "DASHBOARD";
        //    }
        //    catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmShwKb_Click : " + oEx.Message); }
        //}


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
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmCalculate_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Lock Splash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmLockSplash_Click(object sender, EventArgs e)     //*Arm 62-09-27
        {
            try
            {
                new cFunctionKeyboard().C_KBDxLock();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmLockSplash_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Call Function Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHome_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;

            try
            {
                //*Arm 63-02-04  -[Comment Code]
                // Call by name
                //tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                //cVB.tVB_KbdCallByName = tFuncName;
                //new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                //W_GETxFuncByFuncName(tFuncName);

                //if (ocmHome.BackColor == cVB.oVB_ColNormal)  // เลือกปุ่ม Home
                //    cVB.tVB_KbdScreen = "HOME";
                //else
                //    cVB.tVB_KbdScreen = "DASHBOARD";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmHome_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Function main Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMainMenu_Click(object sender, EventArgs e)
        {
            Button ocmMainMenu;
            string tFuncName;

            try
            {

                ocmMainMenu = (Button)sender;

                if (ocmMainMenu.Tag != null)
                {
                    //ocmHome.Focus(); //*Arm 63-02-04 [Comment Code]
                    tFuncName = ocmMainMenu.Tag.ToString();
                    cVB.tVB_KbdCallByName = tFuncName;
                    new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                    W_GETxFuncByFuncName(tFuncName);

                    //*Em 62-09-10
                    if (tFuncName == "C_KBDxSwitchUser")
                    {
                        W_SETxDesign();
                        W_SETxText();
                    }
                    //+++++++++++++
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmMainMenu_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmHelp_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmAbout_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Next Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (nW_CurrentPage < nW_MaxPage)
                {
                    nW_CurrentPage++;
                    W_GENxButtonFunc();
                }
                else
                    nW_CurrentPage = nW_MaxPage;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmNextPage_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Previous Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (nW_CurrentPage > 1)
                {
                    nW_CurrentPage--;
                    W_GENxButtonFunc();
                }
                else
                {
                    nW_CurrentPage = 1;
                    ocmPrevious.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmPrevious_Click " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }


        /// <summary>
        /// Show Passcode > Setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSetting_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxSetting();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmSetting_Click " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Form Close
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "otmClose_Tick : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
                if (opnNotify.Visible == false)
                {
                    cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                }
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmNotify_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
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
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        #endregion End Event

        #region Function

        /// <summary>
        /// Set Design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Arm 63-02-04 Moshi Moshi
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Arm 63-02-04 Moshi Moshi
                ocmPrint.BackColor = cVB.oVB_ColNormal;

                //*Arm 63-02-04 Moshi Moshi [Comment Code]
                //ocmHome.BackColor = cVB.oVB_ColNormal;
                //ocmDashboard.BackColor = cVB.oVB_ColDark;
                //ocmKB.BackColor = cVB.oVB_ColDark;
                //ocmCalculate.BackColor = cVB.oVB_ColDark;
                //ocmShwKb.BackColor = cVB.oVB_ColDark;
                //ocmHelp.BackColor = cVB.oVB_ColDark;
                //ocmAbout.BackColor = cVB.oVB_ColDark;
                //ocmExit.BackColor = cVB.oVB_ColDark;
                //ocmSetting.BackColor = cVB.oVB_ColDark;
                //ocmLockSplash.BackColor = cVB.oVB_ColDark; // *Arm 62-09-27
                // +++++++++++++++++++++

                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode, "TCNMUser");
                opbLogo.Image = new cCompany().C_GEToImageLogo();

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;

                //if (oW_SP.SP_CHKbConnection())
                //    opbPOS.Image = Properties.Resources.Online_32;
                //else
                //    opbPOS.Image = Properties.Resources.Offline_32;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_SETxDesign : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Text
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resHome_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resHome_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "HOME";

                // Dashboard
                olaTitleSaleSum.Text = oW_Resource.GetString("tSaleSum");
                olaTitlePayment.Text = oW_Resource.GetString("tPayment");
                olaTitleVoid.Text = oW_Resource.GetString("tVoid");
                olaTitleQtyCancel.Text = cVB.oVB_GBResource.GetString("tQty");
                olaTitleHold.Text = oW_Resource.GetString("tHold");
                olaTitleQtyHold.Text = cVB.oVB_GBResource.GetString("tQty");
                olaTitleSummary.Text = cVB.oVB_GBResource.GetString("tSummary");
                olaTitleAmtTotal.Text = cVB.oVB_GBResource.GetString("tAmount");
                olaTitleQtyMnyIn.Text = cVB.oVB_GBResource.GetString("tQty");
                olaTitleAmtMnyIn.Text = cVB.oVB_GBResource.GetString("tAmount");
                olaTitleQtyMnyOut.Text = cVB.oVB_GBResource.GetString("tQty");
                olaTitleAmtMnyOut.Text = cVB.oVB_GBResource.GetString("tAmount");
                olaTitleQtyTicket.Text = cVB.oVB_GBResource.GetString("tQty");
                olaTitleAmtTicket.Text = cVB.oVB_GBResource.GetString("tAmount");
                olaTitleQtyTopup.Text = cVB.oVB_GBResource.GetString("tQty");
                olaTitleAmtTopup.Text = cVB.oVB_GBResource.GetString("tAmount");

                // Header
                olaHome.Text = oW_Resource.GetString("tHome");
                olaVersion.Text = Application.ProductVersion;
                olaUsrName.Text = new cUser().C_GETtUsername();
                olaPos.Text = cVB.tVB_PosCode;
                olaUsrName.Text = cVB.tVB_UsrName;

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_SETxText : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Gen Button Function
        /// </summary>
        /// <param name="paoKb"></param>
        private void W_GENxButtonFunc()
        {
            Button ocmFunction;
            List<cmlTPSMFunc> aoKb;
            string tImageName = "";
            int nCountButton = 0;

            try
            {
                opnFunction.Controls.Clear();

                if (nW_CurrentPage == 1)
                    ocmPrevious.Visible = false;
                else
                    ocmPrevious.Visible = true;

                if (nW_CurrentPage == nW_MaxPage)
                    ocmNextPage.Visible = false;
                else
                    ocmNextPage.Visible = true;

                if (string.Equals(cVB.tVB_PosType, "1"))
                    aoKb = new cFunctionKeyboard().C_GETaFuncList("003");
                else
                    aoKb = new cFunctionKeyboard().C_GETaFuncList("004");

                aoKb = (from oFunc in aoKb where oFunc.FNGdtPage == nW_CurrentPage select oFunc).ToList();

                foreach (cmlTPSMFunc oKb in aoKb)
                {
                    if (nCountButton < opnFunction.RowCount * opnFunction.ColumnCount)
                    {
                        //tImageName = oKb.FTKbdScreen + "_" + oKb.FTSysCode;

                        ocmFunction = new Button();
                        ocmFunction.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        ocmFunction.Margin = new Padding(2);
                        ocmFunction.FlatStyle = FlatStyle.Flat;
                        ocmFunction.FlatAppearance.BorderSize = 0;
                        //ocmFunction.Font = new Font("Segoe UI Semibold", 10F);    //*Arm 63-04-28 Comment Code
                        ocmFunction.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);      //*Arm 63-04-28 ปรับเป็น 13F FontStyle.Bold
                        ocmFunction.ForeColor = Color.White;
                        ocmFunction.Text = oKb.FTGdtName;
                        ocmFunction.TextAlign = ContentAlignment.BottomLeft;
                        ocmFunction.Name = "ocm-" + oKb.FTSysCode;
                        ocmFunction.BackgroundImageLayout = ImageLayout.Center;
                        ocmFunction.BackColor = cVB.oVB_ColNormal;
                        ocmFunction.Enabled = true;

                        //if (oKb.FNGdtBtnSizeX > 1 && oKb.FNGdtBtnSizeY > 1)
                        //    tImageName += "_2";
                        //else
                        //    tImageName += "_1";

                        //ocmFunction.BackgroundImage = ((Image)(Properties.Resources.ResourceManager.GetObject(tImageName)));

                        //*Em 62-01-29  AdaPos 5.0
                        try
                        {
                            ocmFunction.BackgroundImage = ((Image)(Properties.Resources.ResourceManager.GetObject(oKb.FTGdtCallByName)));
                        }
                        catch
                        { }
                        //+++++++++++++++++++++++++

                        ocmFunction.Click += ocmMainMenu_Click;
                        ocmFunction.Tag = oKb.FTGdtCallByName;
                        opnFunction.Controls.Add(ocmFunction);
                        opnFunction.SetColumnSpan(ocmFunction, oKb.FNGdtBtnSizeX.Value);
                        opnFunction.SetRowSpan(ocmFunction, oKb.FNGdtBtnSizeY.Value);

                        if (oKb.FNGdtBtnSizeX.Value > 1 || oKb.FNGdtBtnSizeY.Value > 1)
                        {
                            if (oKb.FNGdtBtnSizeX.Value > 1)
                                nCountButton += oKb.FNGdtBtnSizeX.Value;

                            if (oKb.FNGdtBtnSizeY.Value > 1)
                                nCountButton += oKb.FNGdtBtnSizeY.Value;
                        }
                        else
                            nCountButton += 1;

                        if (string.IsNullOrEmpty(cVB.tVB_ShfCode))  // ปิดรอบอยู่
                        {
                            switch (oKb.FTSysCode)
                            {
                                case "KB012":   // Sale
                                case "KB024":   // Return Sale
                                case "KB013":   // Rental
                                case "KB025":   // Return Rental
                                case "KB038":   // Deposit
                                case "KB039":   // Withdraw
                                case "KB015":   // Trade Wristband
                                case "KB006":   // Open drawer
                                case "KB016":   // Top-up
                                case "KB028":   // Cancel Top-up
                                case "KB029":   // Ticket
                                case "KB056":   // Reprint
                                case "KB023":   // Tax invoice
                                case "KB045":   // Close shift
                                case "KB051":   // Return Wristband
                                    ocmFunction.BackColor = Color.LightGray;
                                    ocmFunction.Enabled = false;
                                    break;
                            }
                        }
                        else    // เปิดรอบ
                        {
                            //*Arm 63-02-24 - เช็ครอบการขายเป็นวันที่ปัจจุบันหรือไม่
                            if (cVB.tVB_SaleDate != DateTime.Today.ToString("yyyy-MM-dd"))   //รอบการขายไม่ใช้ของปัจจุบัน
                            {
                                switch (oKb.FTSysCode)
                                {
                                    case "KB021":   // ปิดการใช้งาน : เปิดรอบ
                                    case "KB012":   // Sale
                                    case "KB024":   // Return Sale
                                    case "KB013":   // Rental
                                    case "KB025":   // Return Rental
                                    case "KB038":   // Deposit
                                    case "KB039":   // Withdraw
                                    case "KB015":   // Trade Wristband
                                    case "KB006":   // Open drawer
                                    case "KB016":   // Top-up
                                    case "KB028":   // Cancel Top-up
                                    case "KB029":   // Ticket
                                    case "KB056":   // Reprint
                                    case "KB023":   // Tax invoice
                                    //case "KB045":   // Close shift
                                    case "KB051":   // Return Wristband
                                        ocmFunction.BackColor = Color.LightGray;
                                        ocmFunction.Enabled = false;
                                        break;
                                }
                            }
                            else
                            {
                                switch (oKb.FTSysCode)
                                {
                                    case "KB021":   // ปิดการใช้งาน : เปิดรอบ
                                        ocmFunction.BackColor = Color.LightGray;
                                        ocmFunction.Enabled = false;
                                        break;
                                }
                            }
                            //++++++++++++++++++++


                            //switch (oKb.FTSysCode)
                            //{
                            //    case "KB021":   // ปิดการใช้งาน : เปิดรอบ
                            //        ocmFunction.BackColor = Color.LightGray;
                            //        ocmFunction.Enabled = false;
                            //        break;
                            //}
                        }
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_GENxButtonFunc : " + oEx.Message); }
            finally
            {
                ocmFunction = null;
                aoKb = null;
                tImageName = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Color / Value Chart
        /// </summary>
        private void W_SETxChartShiftSummary(List<cmlShiftSummary> paoShiftSummary, string ptPosType)
        {
            string[] atRateType;
            int[] anSummary;
            bool bSetChart;

            try
            {
                if (string.Equals(ptPosType, "1"))
                {
                    // Store
                    if (paoShiftSummary == null || paoShiftSummary.Count == 0)
                    {
                        anSummary = new int[4];
                        Array.Clear(anSummary, 0, anSummary.Length);
                    }
                    else
                    {
                        anSummary = new int[]
                        {
                        Convert.ToInt32(paoShiftSummary.Where(
                                oItem => string.Equals(oItem.FTEvnFuncRef, "SALE")).Select(
                                oItem => oItem.FCSvnAmt).FirstOrDefault()),
                        Convert.ToInt32(paoShiftSummary.Where(
                                oItem => string.Equals(oItem.FTEvnFuncRef, "RETURN_SALE")).Select(
                                oItem => oItem.FCSvnAmt).FirstOrDefault())
                                /*
                        Convert.ToInt32(paoShiftSummary.Where(
                                oItem => string.Equals(oItem.FTEvnFuncRef, "RETAIN")).Select(
                                oItem => oItem.FCSvnAmt).FirstOrDefault()),
                        Convert.ToInt32(paoShiftSummary.Where(
                                oItem => string.Equals(oItem.FTEvnFuncRef, "RETURN_RETAIN")).Select(
                                oItem => oItem.FCSvnAmt).FirstOrDefault())
                                */
                        };
                    }

                    bSetChart = anSummary.Where(oItem => oItem > 0).Select(oItem => oItem).FirstOrDefault() > 0;
                    if (bSetChart)
                    {
                        atRateType = new string[]
                        {
                        oW_Resource.GetString("tSale"),
                        oW_Resource.GetString("tReturnSale")
                        /*
                        oW_Resource.GetString("tRental"),
                        oW_Resource.GetString("tReturnRental")
                        */
                        };

                        for (int nCount = 0; nCount < atRateType.Length; nCount++)
                        {
                            octSaleSum.Series[0].Points.AddY(anSummary[nCount]);
                            W_SETxChartDesign(octSaleSum, atRateType[nCount], nCount);
                        }
                    }
                }
                else
                {
                    // Cashier
                    //*[AnUBiS][][2019-01-24] - ปรับให้ใช้ข้อมูลจากฐานข้อมูล
                    if (paoShiftSummary == null || paoShiftSummary.Count == 0)
                    {
                        anSummary = new int[3];
                        Array.Clear(anSummary, 0, anSummary.Length);
                    }
                    else
                    {
                        anSummary = new int[]
                        {
                        Convert.ToInt32(paoShiftSummary.Where(
                                oItem => string.Equals(oItem.FTEvnFuncRef, "TICKET")).Select(
                                oItem => oItem.FCSvnAmt).FirstOrDefault()),
                        Convert.ToInt32(paoShiftSummary.Where(
                                oItem => string.Equals(oItem.FTEvnFuncRef, "TOPUP")).Select(
                                oItem => oItem.FCSvnAmt).FirstOrDefault()),
                        Convert.ToInt32(paoShiftSummary.Where(
                                oItem => string.Equals(oItem.FTEvnFuncRef, "CANCEL TOPUP")).Select(
                                oItem => oItem.FCSvnAmt).FirstOrDefault())
                        };
                    }

                    bSetChart = anSummary.Where(oItem => oItem > 0).Select(oItem => oItem).FirstOrDefault() > 0;
                    if (bSetChart)
                    {
                        atRateType = new string[]
                        {
                        oW_Resource.GetString("tTicket"),
                        oW_Resource.GetString("tTopup"),
                        oW_Resource.GetString("tCancelTopup")
                        };

                        for (int nCount = 0; nCount < atRateType.Length; nCount++)
                        {
                            octSaleSum.Series[0].Points.AddY(anSummary[nCount]);
                            W_SETxChartDesign(octSaleSum, atRateType[nCount], nCount);
                        }
                    }
                }

                //*[AnUBiS][][2019-01-21] - comment code.
                /*
                atW_RateType = new string[] { "Ticket", "Top-up", "Cancel Top-up" };
                anW_Summary = new int[] { 2000, 1000, 200 };

                // Payment Type
                for (int nCount = 0; nCount < atW_RateType.Length; nCount++)
                {
                    octSaleSum.Series[0].Points.AddY(anW_Summary[nCount]);
                    W_SETxChartDesign(octSaleSum, atW_RateType[nCount], nCount);
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_SETxChart : " + oEx.Message); }
            finally
            {
                atRateType = null;
                anSummary = null;

                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set color and value chart payment type.
        /// </summary>
        /// 
        /// <param name="paoPaymentType">Payment type.</param>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-01-22] - add new function.
        /// </remarks>
        private void W_SETxChartPaymentType(List<cmlPaymentType> paoPaymentType)
        {
            try
            {
                if (paoPaymentType == null || paoPaymentType.Count == 0)
                {
                    return;
                }

                for (int nLoop = 0; nLoop < paoPaymentType.Count; nLoop++)
                {
                    octPayment.Series[0].Points.AddY(paoPaymentType[nLoop].FCXrcNet);
                    W_SETxChartDesign(octPayment, paoPaymentType[nLoop].FTRcvName, nLoop);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Design Chart
        /// </summary>
        private void W_SETxChartDesign(Chart poChart, string ptName, int pnIndex)
        {
            try
            {
                poChart.Legends[0].CustomItems.Add(Color.Transparent, ptName);
                poChart.Legends[0].CustomItems[pnIndex].ImageStyle = LegendImageStyle.Marker;
                poChart.Legends[0].CustomItems[pnIndex].MarkerBorderWidth = 0;
                poChart.Legends[0].CustomItems[pnIndex].MarkerSize = 20;
                poChart.Legends[0].CustomItems[pnIndex].MarkerStyle = MarkerStyle.Circle;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_SETxChartDesign : " + oEx.Message); }
            finally
            {
                poChart = null;
                ptName = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set dashboard
        /// </summary>
        private void W_SETxDashboard()
        {
            cDashboard oDashboard;
            List<cmlShiftSummary> aoShiftSummary;
            List<cmlPaymentType> aoPaymentTpye;

            try
            {
                //*[AnUBiS][][2019-01-24] - ปรับการแสดง interface ใหม่ตามเงื่อนไข
                olaTitleOpnDrw.Text = oW_Resource.GetString("tOpnDrw");
                olaTitleQtyDrw.Text = cVB.oVB_GBResource.GetString("tQty");
                olaTitleMnyIn.Text = oW_Resource.GetString("tMnyIn");
                olaTitleMnyOut.Text = oW_Resource.GetString("tMnyOut");

                oDashboard = new cDashboard();
                if (string.Equals(cVB.tVB_PosType, "1"))
                {
                    // Store
                    opnSumCnlTopup.Visible = false;

                    opnSummarySale.RowStyles[13].Height = 0;
                    opnSummarySale.RowStyles[14].Height = 0;

                    olaTitleTicketSale.Text = oW_Resource.GetString("tSale");
                    olaTitleTopUpRetSale.Text = oW_Resource.GetString("tReturnSale");

                    aoShiftSummary = oDashboard.C_GETaStoreShiftSummary(
                        cVB.tVB_BchCode, cVB.tVB_ShfCode, cVB.tVB_PosCode, cVB.nVB_Language);
                    aoPaymentTpye = oDashboard.C_GETaStorePaymentType(
                        cVB.tVB_BchCode, cVB.tVB_ShfCode, cVB.tVB_PosCode, cVB.nVB_Language);

                    W_SETxChartPaymentType(aoPaymentTpye);
                }
                else
                {
                    // Cashier
                    olaTitleCnlTopupRental.Text = oW_Resource.GetString("tCancelTopup");
                    olaTitleQtyCnlTopup.Text = cVB.oVB_GBResource.GetString("tQty");
                    olaTitleAmtCnlTopup.Text = cVB.oVB_GBResource.GetString("tAmount");
                    olaTitleTicketSale.Text = oW_Resource.GetString("tTicket");
                    olaTitleTopUpRetSale.Text = oW_Resource.GetString("tTopup");

                    aoShiftSummary = oDashboard.C_GETaCashierShiftSummary(
                        cVB.tVB_BchCode, cVB.tVB_ShfCode, cVB.tVB_PosCode, cVB.nVB_Language);
                    aoPaymentTpye = oDashboard.C_GETaCashierPaymentType(
                        cVB.tVB_BchCode, cVB.tVB_ShfCode, cVB.tVB_PosCode, cVB.nVB_Language);

                    W_SETxChartPaymentType(aoPaymentTpye);
                }

                W_SETxChartShiftSummary(aoShiftSummary, cVB.tVB_PosType);
                W_SETxShiftSummary(aoShiftSummary);

                //*[AnUBiS][][2019-01-24] - comment code.
                /*
                if (string.Equals(cVB.tVB_PosType, "1")) // Store
                {
                    opnSummarySale.RowStyles[0].Height = 0;//
                    opnSummarySale.RowStyles[1].Height = 0;//
                    opnSummarySale.RowStyles[13].Height = 0;
                    opnSummarySale.RowStyles[14].Height = 0;

                    opnSumOpDrw.Visible = false;//
                    opnSumCnlTopup.Visible = false;

                    olaTitleMnyIn.Text = oW_Resource.GetString("tSale");//
                    olaTitleMnyOut.Text = oW_Resource.GetString("tReturnSale");//
                    olaTitleTicketSale.Text = oW_Resource.GetString("tRental");
                    olaTitleTopUpRetSale.Text = oW_Resource.GetString("tReturnRental");
                }
                else    // Cashier
                {
                    olaTitleOpnDrw.Text = oW_Resource.GetString("tOpnDrw");//
                    olaTitleQtyDrw.Text = cVB.oVB_GBResource.GetString("tQty");//
                    olaTitleCnlTopupRental.Text = oW_Resource.GetString("tCancelTopup");
                    olaTitleQtyCnlTopup.Text = cVB.oVB_GBResource.GetString("tQty");
                    olaTitleAmtCnlTopup.Text = cVB.oVB_GBResource.GetString("tAmount");
                    olaTitleMnyIn.Text = oW_Resource.GetString("tMnyIn");//
                    olaTitleMnyOut.Text = oW_Resource.GetString("tMnyOut");//
                    olaTitleTicketSale.Text = oW_Resource.GetString("tTicket");
                    olaTitleTopUpRetSale.Text = oW_Resource.GetString("tTopup");
                }
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_SETxDashboard : " + oEx.Message); }
            finally
            {
                oDashboard = null;
                aoShiftSummary = null;
                aoPaymentTpye = null;

                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Show Main Menu : Function
        /// </summary>
        private void W_GETxHome()
        {
            try
            {
                //*Arm 63-02-04 [Comment Code]
                //ocmDashboard.BackColor = cVB.oVB_ColDark;
                //ocmHome.BackColor = cVB.oVB_ColNormal;
                //opnDashboard.Visible = false;
                //opnContentFunction.Visible = true;
                //olaHome.Text = oW_Resource.GetString("tHome");
                //opbHome.Image = Properties.Resources.HomeB_32;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_GETxHome : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Show Dashboard
        /// </summary>
        public void W_GETxDashboard()
        {
            try
            {
                //*Arm 63-02-04 [Comment Code] 
                //ocmDashboard.BackColor = cVB.oVB_ColNormal;
                //ocmHome.BackColor = cVB.oVB_ColDark;
                //opnDashboard.Visible = true;
                //opnContentFunction.Visible = false;
                //olaHome.Text = oW_Resource.GetString("tDashboard");
                //opbHome.Image = Properties.Resources.ChartB_32;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_GETxDashboard : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get function in form 
        /// </summary>
        private void W_GETxFuncByFuncName(string ptFuncName)
        {
            string tChkRole, tFuncCode;
            wSignin oSignIn = null;

            try
            {
                switch (ptFuncName)
                {
                    case "C_KBDxHome":
                        W_GETxHome();
                        break;

                    case "C_KBDxDashboard":
                        //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                        tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                        //tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode);
                        tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                        switch (tChkRole)
                        {
                            case "1":   // allowed.
                                W_GETxDashboard();
                                break;
                            case "0":   // not permission.
                            case "800": // data not found.
                                oSignIn = new wSignin(1, "DASHBOARD");
                                oSignIn.ShowDialog();

                                if (oSignIn.DialogResult == DialogResult.OK)
                                    W_GETxDashboard();

                                break;
                            case "900":
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                                break;
                        }

                        /*
                        tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                        if (string.IsNullOrEmpty(tChkRole))
                        {
                            oSignIn = new wSignin(1, "DASHBOARD");
                            oSignIn.ShowDialog();

                            if (oSignIn.DialogResult == DialogResult.OK)
                                W_GETxDashboard();
                        }
                        else
                            W_GETxDashboard();
                        */
                        break;

                    case "C_KBDxOpenShift":
                        W_GENxButtonFunc();
                        break;

                    case "C_KBDxMenu":
                        W_SETxOpenCloseMenu();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                ptFuncName = null;
                tChkRole = null;
                oSignIn = null;

                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT;  //*Arm 63-02-04 Moshi Moshi
            List<cmlTPSMFunc> aoMenuB;  //*Arm 63-02-04 Moshi Moshi
            int nItem;  //*Em 62-01-25  Waterpark
            try
            {
                aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                //foreach (cmlTPSMFunc oKb in aoKb)
                //{
                //    switch (oKb.FTSysCode)
                //    {
                //        case "KB010":   // Help
                //            ocmHelp.Visible = true;
                //            ocmHelp.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB022":   // Show Kb
                //            ocmShwKb.Visible = true;
                //            ocmShwKb.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB027":   // Calculate
                //            ocmCalculate.Visible = true;
                //            ocmCalculate.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;
                //        case "KB034":   // LockSplash  *Arm 62-11-13
                //            ocmLockSplash.Visible = true;
                //            ocmLockSplash.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;
                //        case "KB046":   //Kb
                //            ocmKB.Visible = true;
                //            ocmKB.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB047":   // About
                //            ocmAbout.Visible = true;
                //            ocmAbout.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB057":   // Setting
                //            ocmSetting.Visible = true;
                //            ocmSetting.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB003":   // Exit
                //            ocmExit.Visible = true;
                //            ocmExit.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB011":   // Home
                //            ocmHome.Visible = true;
                //            ocmHome.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB026":   // Dashboard
                //            //ocmDashboard.Visible = true;
                //            //ocmDashboard.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;
                //    }
                //}


                //*Arm 63-02-04 Moshi Moshi
                aoMenuT = (from oBar in aoKb where oBar.FNGdtPage == 1 orderby oBar.FNGdtUsrSeq select oBar).ToList();
                aoMenuB = (from oBar in aoKb where oBar.FNGdtPage == 2 orderby oBar.FNGdtUsrSeq select oBar).ToList();

                if (aoMenuT.Count > 0)
                {
                    opnMenuT.Controls.Clear();
                    opnMenuT.RowCount = aoMenuT.Count + 1;
                    nItem = 0;
                    foreach (cmlTPSMFunc oMenu in aoMenuT)
                    {
                        ocmMenu = new Button();
                        ocmMenu.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                        ocmMenu.FlatStyle = FlatStyle.Flat;
                        ocmMenu.FlatAppearance.BorderSize = 0;
                        ocmMenu.Font = new Font("Segoe UI Semibold", 10F);
                        ocmMenu.ForeColor = Color.White;
                        ocmMenu.Text = "".PadLeft(5) + oMenu.FTGdtName;
                        ocmMenu.TextAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.Name = oMenu.FTGdtCallByName;
                        ocmMenu.BackColor = cVB.oVB_ColDark;
                        ocmMenu.Enabled = true;
                        ocmMenu.MouseLeave += opnMenu_MouseLeave;
                        try
                        {
                            ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        }
                        catch { }
                        ocmMenu.TextImageRelation = TextImageRelation.ImageBeforeText;

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 260;
                        opnMenuT.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuT.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuT.RowStyles[nItem].Height = 55;
                        opnMenuT.MouseLeave += opnMenu_MouseLeave;
                        nItem = nItem + 1;
                    }
                }

                if (aoMenuB.Count > 0)
                {
                    opnMenuB.Controls.Clear();
                    opnMenuB.RowCount = aoMenuB.Count + 1;
                    nItem = 1;

                    Panel oPanel;
                    oPanel = new Panel();
                    oPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                    oPanel.Height = opnMenuB.Height - ((opnMenuB.RowCount - 1) * 55);
                    opnMenuB.Controls.Add(oPanel);

                    opnMenuB.RowStyles[0].SizeType = SizeType.AutoSize;
                    foreach (cmlTPSMFunc oMenu in aoMenuB)
                    {
                        ocmMenu = new Button();
                        ocmMenu.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                        //ocmMenu.Margin = new Padding(2);
                        ocmMenu.FlatStyle = FlatStyle.Flat;
                        ocmMenu.FlatAppearance.BorderSize = 0;
                        ocmMenu.Font = new Font("Segoe UI Semibold", 10F);
                        ocmMenu.ForeColor = Color.White;
                        ocmMenu.Text = "".PadLeft(5) + oMenu.FTGdtName;
                        ocmMenu.TextAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.Name = oMenu.FTGdtCallByName;
                        ocmMenu.TextImageRelation = TextImageRelation.ImageBeforeText;
                        ocmMenu.BackColor = cVB.oVB_ColDark;
                        ocmMenu.Enabled = true;
                        ocmMenu.MouseLeave += opnMenu_MouseLeave;
                        try
                        {
                            ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        }
                        catch { }

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 260;
                        opnMenuB.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuB.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuB.RowStyles[nItem].Height = 55;
                        opnMenuB.MouseLeave += opnMenu_MouseLeave;
                        nItem = nItem + 1;
                    }
                }
                // ++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                new cSP().SP_CLExMemory();
            }
        }
        private void ocmMenuBar_Click(object sender, EventArgs e)
        {
            //*Arm 63-02-04 - Moshi Moshi
            string tFuncName;
            try
            {
                Button ocmMenu;
                ocmMenu = (Button)sender;
                tFuncName = ocmMenu.Tag.ToString();
                //switch (tFuncName)
                //{
                //    case "C_KBDxBack":
                //        try
                //        {
                //            if (ogdOrder.Rows.Count == 0)
                //            {
                //                new wHome().Show();
                //                otmClose.Start();
                //            }
                //            else
                //                oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantBack"), 3);
                //        }
                //        catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmMenuBar_Click " + oEx.Message); }
                //        break;
                //    case "C_KBDxPdtDetail": break;
                //    case "C_KBDxSortAsc":
                //        W_PRCxSortPdt(1);
                //        break;
                //    case "C_KBDxSortDesc":
                //        W_PRCxSortPdt(2);
                //        break;
                //    default:
                //        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                //        break;
                //}
                new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
            }
            catch
            { }
            finally
            {
                new cSP().SP_CLExMemory();
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
                    opnMenu.Width = 55;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_SETxOpenCloseMenu : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_PRCxAcceptSignalR : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
                            //cSP.SP_GETxCountNotify(olaMsgCount,opnNotify);
                        }));
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "W_CHKxMsg : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }





        /// <summary>
        /// Set shift summary to interface.
        /// </summary>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-01-21] - add new function.
        /// </remarks>
        private void W_SETxShiftSummary(List<cmlShiftSummary> paoSaleSummary)
        {
            cDashboard oDashboard;
            cSP oFunc;
            decimal cTopup, cCancelTopup, cSumTotal;

            try
            {
                oFunc = new cSP();
                oDashboard = new cDashboard();

                if (paoSaleSummary != null && paoSaleSummary.Count > 0)
                {
                    olaQtyOpnDrw.Text = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "fC_Drawer")).Select(
                        oItem => oItem.FNSvnQty).FirstOrDefault().ToString();

                    olaQtyCancel.Text = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "fC_VoidBill")).Select(
                        oItem => oItem.FNSvnQty).FirstOrDefault().ToString();

                    olaQtyHold.Text = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "fC_HoldBill")).Select(
                        oItem => oItem.FNSvnQty).FirstOrDefault().ToString();

                    olaQtyMnyIn.Text = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "fC_MnyIn")).Select(
                        oItem => oItem.FNSvnQty).FirstOrDefault().ToString();

                    olaSumMnyIn.Text = oFunc.SP_SETtDecShwSve(1,
                        paoSaleSummary.Where(
                            oItem => string.Equals(oItem.FTEvnFuncRef, "fC_MnyIn")).Select(
                            oItem => oItem.FCSvnAmt).FirstOrDefault(), cVB.nVB_DecShow);

                    olaQtyMnyOut.Text = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "fC_MnyOut")).Select(
                        oItem => oItem.FNSvnQty).FirstOrDefault().ToString();

                    olaSumMnyOut.Text = oFunc.SP_SETtDecShwSve(1,
                        paoSaleSummary.Where(
                            oItem => string.Equals(oItem.FTEvnFuncRef, "fC_MnyOut")).Select(
                            oItem => oItem.FCSvnAmt).FirstOrDefault(), cVB.nVB_DecShow);

                    olaQtySale.Text = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "TICKET")).Select(
                        oItem => oItem.FNSvnQty).FirstOrDefault().ToString();

                    olaSumSale.Text = oFunc.SP_SETtDecShwSve(1,
                        paoSaleSummary.Where(
                            oItem => string.Equals(oItem.FTEvnFuncRef, "TICKET")).Select(
                            oItem => oItem.FCSvnAmt).FirstOrDefault(), cVB.nVB_DecShow);

                    cTopup = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "TOPUP")).Select(
                        oItem => oItem.FCSvnAmt).FirstOrDefault();

                    cCancelTopup = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "CANCEL TOPUP")).Select(
                        oItem => oItem.FCSvnAmt).FirstOrDefault();

                    cSumTotal = cTopup - cCancelTopup;

                    olaQtyReturn.Text = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "TOPUP")).Select(
                        oItem => oItem.FNSvnQty).FirstOrDefault().ToString();

                    olaSumReturn.Text = oFunc.SP_SETtDecShwSve(1, cTopup, cVB.nVB_DecShow);

                    olaQtyCnlTopup.Text = paoSaleSummary.Where(
                        oItem => string.Equals(oItem.FTEvnFuncRef, "CANCEL TOPUP")).Select(
                        oItem => oItem.FNSvnQty).FirstOrDefault().ToString();

                    olaAmtCnlTopup.Text = oFunc.SP_SETtDecShwSve(1, cCancelTopup, cVB.nVB_DecShow);

                    olaSumTotal.Text = oFunc.SP_SETtDecShwSve(1, cSumTotal, cVB.nVB_DecShow);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        #endregion End Function
    }
}
