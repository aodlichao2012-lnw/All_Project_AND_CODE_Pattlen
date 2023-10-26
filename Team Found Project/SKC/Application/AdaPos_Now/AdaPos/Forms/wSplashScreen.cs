using AdaPos.Class;
using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Resources_String.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace AdaPos
{
    public partial class wSplashScreen : Form
    {
        #region Variable

        private cSP oW_SP;
        private cmlTSysLanguage oW_Language;
        private int nW_Time;
        private int nW_mode;    //*Arm 62-09-30
        

        #endregion End Variable

        #region Constructor

        public wSplashScreen(int pnMode = 0)
        {
            InitializeComponent();

            try
            {
                




                oW_SP = new cSP();
                oW_SP.SP_PRCxFlickering(this.Handle);
                
                cVB.nVB_SettingFrom = 1;
                W_GETxLanguageName();   //*Em 62-01-22  Waterpark
                W_SETxText();
                W_SETxBackgroud();
                W_SETxDesign();
                W_SHWxButtonBar();

                nW_mode = pnMode;       //*Arm 62-09-30

                this.KeyPreview = true; //*Arm 63-02-06 - (HotKey)
                foreach(System.Windows.Forms.Control opnC in opnMenu.Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "wSplashScreen : " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

        /// <summary>
        /// Open Form Main Menu
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
                //new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                W_CALxByName(tFuncName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmMenu_KeyDown " + oEx.Message); }
            finally
            {
                tFuncName = null;
                this.ActiveControl = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// ขยาย - ย่อ Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                W_SETxMenuBurger();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmMenu_Click " + oEx.Message); }
            finally
            {
                this.ActiveControl = null;
            }
        }

        /// <summary>
        /// Change Language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmLanguage_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxLanguage();
                W_SHWxButtonBar();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmLanguage_Click " + oEx.Message); }
        }

        /// <summary>
        /// Open form Video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmVideo_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxVideo();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmVideo_Click " + oEx.Message); }
        }

        /// <summary>
        /// Open Form Setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSetting_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxSetting();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmSetting_Click " + oEx.Message); }
        }

        /// <summary>
        /// Open Popup About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAbout_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAbout();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmAbout_Click " + oEx.Message); }
        }

        /// <summary>
        /// Open form Register
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmRegister_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxRegister();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmRegister_Click " + oEx.Message); }
        }

        /// <summary>
        /// Open Form Main Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSplashScreen_Click(object sender, EventArgs e)
        {
            try
            {
                W_OPNxMenu();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "wSplashScreen_Click : " + oEx.Message); }
        }

        /// <summary>
        /// เคลีย Active หลังจากโหลดข้อมูลเสร็จ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSplashScreen_Shown(object sender, EventArgs e)
        {
            try
            {
                if (ockShowForm.Checked)
                    otmClose.Start();
                else
                    this.ActiveControl = null;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "wSplashScreen_Shown : " + oEx.Message); }
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
                    W_CALxByName(cVB.tVB_KbdCallByName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wSplashScreen", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmCalculate_Click : " + oEx.Message); }
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
                cVB.tVB_KbdCallByName = "C_KBDxHelp";
                new cFunctionKeyboard().C_KBDxHelp();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ocmHelp_Click : " + oEx.Message); }
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
                    this.Hide();

                nW_Time++;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Language = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "wSplashScreen_FormClosing : " + oEx.Message); }
        }

        /// <summary>
        /// Menu bar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenuBar_Click(object sender, EventArgs e)
        {
            try
            {
                Button ocmMenu;
                ocmMenu = (Button)sender;
                W_CALxByName(ocmMenu.Tag.ToString());
            }
            catch
            { }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }
        #endregion End Event

        #region Function

        /// <summary>
        /// Open Menu
        /// </summary>
        private void W_OPNxMenu()
        {
            try
            {
                //*Arm 63-01-22 - Check Config ก่อนเข้าหน้า Signin
                if (!string.IsNullOrEmpty(cVB.tVB_PosCode))
                {
                    //*Arm 62-09-30
                    wSignin owSignin = new wSignin(nW_mode, "");
                    if (nW_mode == 4)
                    {
                        if (owSignin.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }

                    }
                    else
                    {
                        owSignin.Show();
                        otmClose.Start();
                    } //*Arm 62-09-30
                }
                else
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgPosNotDefine"), 1);
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "W_OPNxMenu : " + oEx.Message); }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                //opnMenuHD.BackColor = cVB.oVB_ColDark;
                //ocmLanguage.BackColor = cVB.oVB_ColDark;
                //opnMenuBT.BackColor = cVB.oVB_ColDark;
                //ocmVideo.BackColor = cVB.oVB_ColDark;
                //ocmRegister.BackColor = cVB.oVB_ColDark;
                //ocmSetting.BackColor = cVB.oVB_ColDark;
                //ocmAbout.BackColor = cVB.oVB_ColDark;
                //ocmShwKb.BackColor = cVB.oVB_ColDark;
                //ocmKB.BackColor = cVB.oVB_ColDark;
                //ocmCalculate.BackColor = cVB.oVB_ColDark;
                //ocmHelp.BackColor = cVB.oVB_ColDark;

                ockShowForm.ForeColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        cVB.oVB_GBResource = new ResourceManager(typeof(resGlobal_TH));
                        break;

                    default:    // EN
                        cVB.oVB_GBResource = new ResourceManager(typeof(resGlobal_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "SPLASHSCREEN";
                this.Text = Assembly.GetExecutingAssembly().GetName().Name;   //*Em 63-07-11
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Get language name
        /// </summary>
        private void W_GETxLanguageName()
        {
            try
            {
                oW_Language = new cLanguage().C_GEToLanguage(cVB.nVB_Language);

                if (oW_Language == null)
                {
                    oW_Language = new cLanguage().C_GEToLanguage();
                    cVB.nVB_Language = Convert.ToInt32(oW_Language.FNLngID);
                }

                if (oW_Language != null)
                {
                    //if (opnMenu.Width <= 100)
                    //    ocmLanguage.Text = oW_Language.FTLngShortName.ToUpper();
                    //else
                    //    ocmLanguage.Text = oW_Language.FTLngNameEng.ToUpper();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "W_GETxLanguageName " + oEx.Message); }
        }

        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT;  //*Em 62-01-22  Waterpark
            List<cmlTPSMFunc> aoMenuB;  //*Em 62-01-22  Waterpark
            int nItem;  //*Em 62-01-22  Waterpark
            try
            {
                aoKb = new cFunctionKeyboard().C_GETaMenuBar("SPLASHSCREEN");
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                //foreach (cmlTPSMFunc oKb in aoKb)
                //{
                //    switch (oKb.FTSysCode)
                //    {
                //        case "KB010":
                //            ocmHelp.Visible = true;
                //            ocmHelp.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB011":
                //            ocmLanguage.Visible = true;
                //            W_GETxLanguageName();
                //            break;

                //        case "KB022":
                //            ocmShwKb.Visible = true;
                //            ocmShwKb.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB027":
                //            ocmCalculate.Visible = true;
                //            ocmCalculate.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB046":
                //            ocmKB.Visible = true;
                //            ocmKB.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB047":
                //            ocmAbout.Visible = true;
                //            ocmAbout.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB054":
                //            ocmVideo.Visible = true;
                //            ocmVideo.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB055":
                //            ocmRegister.Visible = true;
                //            ocmRegister.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB057":
                //            ocmSetting.Visible = true;
                //            ocmSetting.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB021":
                //            ockShowForm.Text = oKb.FTGdtName;
                //            break;
                //    }
                //}

                //*Em 62-01-22  Waterpark
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

                        if (oMenu.FTGdtCallByName != "C_KBDxLanguage")
                        {
                            try
                            {
                                ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                            }
                            catch
                            { }
                            ocmMenu.TextImageRelation = TextImageRelation.ImageBeforeText;
                        }
                        else
                        {
                            ocmMenu.TextImageRelation = TextImageRelation.Overlay;
                            ocmMenu.Font = new Font("Segoe UI Semibold", 12F);
                            ocmMenu.ForeColor = Color.White;
                            if (oW_Language != null)
                            {
                                if (opnMenu.Width <= 100)
                                    ocmMenu.Text = oW_Language.FTLngShortName.ToUpper();
                                else
                                    ocmMenu.Text = oW_Language.FTLngNameEng.ToUpper();
                            }
                        }

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 249;
                        opnMenuT.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuT.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuT.RowStyles[nItem].Height = 55;
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

                        //*Arm 63-02-24  
                        if (oMenu.FTGdtCallByName != "C_KBDxLanguage")
                        {
                            try
                            {
                                ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                            }
                            catch
                            { }

                        }
                        else
                        {
                            ocmMenu.TextImageRelation = TextImageRelation.Overlay;
                            ocmMenu.Font = new Font("Segoe UI Semibold", 12F);
                            ocmMenu.ForeColor = Color.White;
                            if (oW_Language != null)
                            {
                                if (opnMenu.Width <= 100)
                                    ocmMenu.Text = oW_Language.FTLngShortName.ToUpper();
                                else
                                    ocmMenu.Text = oW_Language.FTLngNameEng.ToUpper();
                            }
                        }
                        //+++++++++++++++++++++++++

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 249;
                        opnMenuB.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuB.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuB.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }
                //++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set menu open / close menu
        /// </summary>
        private void W_SETxMenuBurger()
        {
            try
            {
                if (opnMenu.Width <= 100)
                {
                    opnMenu.Width = 270;

                    //if (oW_Language != null)
                    //ocmLanguage.Text = oW_Language.FTLngNameEng.ToUpper();
                    W_SHWxButtonBar();  //*Em 62-01-22  Waterpark
                }
                else
                {
                    opnMenu.Width = 55;

                    //if (oW_Language != null)
                    //    ocmLanguage.Text = oW_Language.FTLngShortName.ToUpper();
                    W_SHWxButtonBar();  //*Em 62-01-22  Waterpark
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "W_SETxMenuBurger : " + oEx.Message); }
        }

        /// <summary>
        /// Call by name
        /// </summary>
        private void W_CALxByName(string ptFuncName)
        {
            try
            {
                switch (ptFuncName)
                {
                    case "C_KBDxLanguage":
                        //case "C_KBDoShowKB":
                        new cFunctionKeyboard().C_KBDxLanguage();
                        W_GETxLanguageName();   //*Em 62-01-22  Waterpark
                        W_SHWxButtonBar();
                        W_SETxText();
                        break;

                    case "C_KBDxMenu":
                        W_SETxMenuBurger();
                        break;

                    case "C_KBDxShwSplash":
                        if (ockShowForm.Checked)
                            ockShowForm.Checked = false;
                        else
                            ockShowForm.Checked = true;
                        break;

                    case "C_KBDxSignIn":
                        W_OPNxMenu();
                        break;

                    default:    //*Em 62-01-22  Waterpark
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "W_CALxByName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
            }
        }

        #endregion End Function

        /// <summary>
        /// Check Show Form Splash Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ockShowForm_CheckedChanged(object sender, EventArgs e)
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE TSysSetting SET");

                if (ockShowForm.Checked)
                    oSql.AppendLine("FTSysStaUsrValue = '" + 0 + "'");
                else
                    oSql.AppendLine("FTSysStaUsrValue = '" + 1 + "'");

                oSql.AppendLine("");
                oSql.AppendLine("");
                oSql.AppendLine("");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSplashScreen", "ockShowForm_CheckedChanged : " + oEx.Message); }
        }

        private void W_SETxBackgroud()
        {
            string tImagePath = string.Empty;
            try
            {
                tImagePath =Directory.GetParent(Application.StartupPath) + @"\AdaImage\Background\splash.png";
                if(!File.Exists(tImagePath)) tImagePath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Background\splash.jpg";
                if (File.Exists(tImagePath))
                {
                    this.BackgroundImage = Image.FromFile(tImagePath);
                }
                else
                {
                    this.BackgroundImage = Properties.Resources.BG;
                }
            }
            catch (Exception oEx)
            {

                new cLog().C_WRTxLog("wSplashScreen", "W_SETxBackgroud : " + oEx.Message);
            }
            finally
            {

            }
        }
    }
}
