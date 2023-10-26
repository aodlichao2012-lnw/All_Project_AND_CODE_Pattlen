using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Popup.All;
using AdaPos.Resources_String.Global;
using AdaPos.Resources_String.Local;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wSignin : Form
    {
        #region Variable

        private IDisposable oW_Boardcast;
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private List<cmlTCNMUser> aoW_User;
        private string tW_ScreenRole;
        private int nW_Mode;    // 0:Sign in, 1:Role Function, 2:Switch user, 4:lock Splash
        private int nW_Time;
        private List<cmlTCNMAppModule> aoW_AppModule;
        private string tLocSplashLastUser;  //*Arm 62-09-30
        private string tLocSplasMsghigh;    //*Arm 62-09-30
        private string tLocSplasMsgLow;     //*Arm 62-09-30
        private cmlTSysLanguage oW_Language; //*Arm 63-02-24 เพิ่มฟังก์ชั่นเปลี่ยนภาษา
        private string tW_Type;  //*Arm 63-02-24 เก็บ Type การ Login ที่เลือก ใช้ในตอนเปลี่ยนเปลี่ยนภาษา
        #endregion End Variable


        #region Constructor

        public wSignin(int pnMode, string ptScreenRole)
        {
            new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Create Start", cVB.bVB_AlwPrnLog); //*Net Stamp
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //*Net 63-07-31 ย้ายไป Shown
                //if(cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                //oW_SP.SP_PRCxFlickering(this.Handle);

                nW_Mode = pnMode;
                tW_ScreenRole = ptScreenRole;
                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Set BG", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_SETxBackgroud();
                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Set Design", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_SETxDesign();
                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Set Text", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_SETxText();
                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Get Lang", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_GETxLanguageName(); //*Arm 63-02-24 เพิ่มฟังก์ชั่นเปลี่ยนภาษา
                //W_GETxUser(pnMode);         //*[AnUBiS][][2019-01-08] - Increase parameter mode.
                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Show Button Bar", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_SHWxButtonBar();
                //W_GETxCountNotify(pnMode);  //*[AnUBiS][][2019-01-08] - Increase parameter mode.
                //W_PRCxAcceptSignalR();
                aoW_AppModule = oW_SP.SP_GEToAppSignType();
                W_SETxPanelSignType();
                otbPin.MaxLength = cVB.nVB_MaxLenPasscode;
                oucNumpad.oU_TextValue = otbPin;
                uNumpadPwd.oU_TextValue = otbPassword;


                this.KeyPreview = true; //*Arm 63-02-06 - (HotKey)
                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }
                oW_SP.SP_PRCxFlickering(this.Handle);   //*Em 63-08-09
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "wSignin : " + oEx.Message); }
            finally
            {
                ptScreenRole = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        #endregion End Constructor

        #region Event

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }
        /// <summary>
        /// Check password > 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmNext_Click(object sender, EventArgs e)
        {
            try
            {
                //W_CHKxPincode();
                //W_CHKxUser("2", otbUserPin.Tag.ToString(), otbPin.Text);
                W_CHKxUser("2", otbUserPin.Tag.ToString(), otbPin.Text, otbUserPin.Text);   //*Em 62-09-10
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmNext_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmKB_Click : " + oEx.Message); }
            finally
            {
                otbUserPin.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbPin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        //W_CHKxPincode();
                        //W_CHKxUser("2", otbUserPin.Tag.ToString(), otbPin.Text);
                        W_CHKxUser("2", otbUserPin.Tag.ToString(), otbPin.Text, otbUserPin.Text);   //*Em 62-09-10
                        break;

                    case Keys.Escape:
                        //if (ocmBack.Visible)
                        //    otmClose.Start();
                        break;

                    //default:
                    //    W_CALxByName(e);
                    //    break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "otbPin_KeyDown : " + oEx.Message); }
        }

        /// <summary>
        /// Clear selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSignin_Shown(object sender, EventArgs e)
        {
            try
            {
                //*Arm 63-08-10 Comment Code
                ////*Net 63-07-31 ย้ายมาจาก constructor
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                //W_GETxCountNotify(nW_Mode);
                ////++++++++++++++++++++++++

                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Show", cVB.bVB_AlwPrnLog); //*Net Stamp
                otbUserPin.Focus();
                ogdUserPin.ClearSelection();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "wSignin_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Choose user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdUser_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ////W_PRCxChooseLastUser();
                //W_PRCxChooseLastUserForPwdAndPin("GETUserPin");
                W_PRCxChooseLastUserForPwdAndPin("GETUserLogin", ogdUserPin.CurrentRow.Cells[1].Value.ToString()); //*Arm 63-08-07
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ogdUser_CellMouseClick : " + oEx.Message); }
        }

        /// <summary>
        /// Key number only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbPin_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "otbPin_KeyPress : " + oEx.Message); }
        }

        /// <summary>
        /// When press enter, focus password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocbUser_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        otbPin.Focus();
                        break;

                    case Keys.Escape:
                        //if (ocmBack.Visible)
                        //    otmClose.Start();
                        break;

                    default:
                        W_CALxByName(e);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocbUser_KeyDown : " + oEx.Message); }
        }

        /// <summary>
        /// Close Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                cVB.tVB_KbdScreen = tW_ScreenRole;
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmBack_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmCalculate_Click : " + oEx.Message); }
            finally
            {
                otbUserPin.Focus();
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
                    W_GETxFuncByFuncName(cVB.tVB_KbdCallByName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmShwKb_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmHelp_Click : " + oEx.Message); }
            finally
            {
                otbUserPin.Focus();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmAbout_Click : " + oEx.Message); }
            finally
            {
                otbUserPin.Focus();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmMenu_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Focus textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdUser_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        otbUserPin.Focus();
                        ogdUserPin.ClearSelection();
                        break;

                    case Keys.Up:
                        if (ogdUserPin.SelectedRows[0].Index > 0)
                            ogdUserPin.Rows[ogdUserPin.SelectedRows[0].Index - 1].Selected = true;
                        break;

                    case Keys.Down:
                        if (ogdUserPin.Rows.Count - 1 > ogdUserPin.SelectedRows[0].Index)
                            ogdUserPin.Rows[ogdUserPin.SelectedRows[0].Index + 1].Selected = true;
                        break;

                    case Keys.Enter:
                        W_PRCxChooseLastUser();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ogdUser_KeyDown : " + oEx.Message); }
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
                {
                    if (nW_Mode == 0)
                        this.Hide();
                    else
                        this.Close();
                    otmClose.Enabled = false;
                }

                nW_Time++;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSignin_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();

                if (oW_Boardcast != null)
                    oW_Boardcast.Dispose();

                oW_Boardcast = null;
                oW_Resource = null;
                aoW_User = null;
                //oW_SP.SP_CLExMemory();
                tW_ScreenRole = null;
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "wSignin_FormClosing : " + oEx.Message); }
        }
        //*[ping][2019.06.05][ปิดฟังก์ชั่นเนื่องจากไมได้ใช้ Combobox แล้ว]
        ///// <summary>
        ///// Select User and Change Image
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ocbUser_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        opbUser.Image = new cUser().C_GEToImageUsr(otbUserPin.SelectedValue.ToString());
        //    }
        //    catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocbUser_SelectedIndexChanged " + oEx.Message); }
        //}

        /// <summary>
        /// แจ้งเตือน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                //W_CHKxNotify();
                //*Em 62-09-17
                if (opnNotify.Visible == false)
                {
                   cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                }
                //+++++++++++++++++
                //cSP.SP_CHKxNotify(olaMsgCount, opnNotify);  //*Em 62-09-15
                //cSP.SP_CHKxNotify(olaMsgCount, opnNotify, olaMsgCountQMem, opnQMember);//*Arm 62-10-25
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmNotify_Click : " + oEx.Message); }
        }

        private void ocmMenuBar_Click(object sender, EventArgs e)
        {
            string tFuncName;
            try
            {
                Button ocmMenu;
                ocmMenu = (Button)sender;
                tFuncName = ocmMenu.Tag.ToString();
                switch (tFuncName)
                {
                    case "C_KBDxBack":
                        try
                        {
                            cVB.tVB_KbdScreen = tW_ScreenRole;
                            otmClose.Start();
                        }
                        catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "ocmMenuBar_Click : " + oEx.Message); }
                        break;
                    //*Arm 63-02-24 เพิ่มฟังก์ชั่นเปลี่ยนภาษา
                    case "C_KBDxLanguage":
                        new cFunctionKeyboard().C_KBDxLanguage();
                        W_GETxLanguageName();
                        W_SHWxButtonBar();
                        W_SETxText();
                        W_SHOWxSignType(tW_Type);
                        break;
                    //++++++++++++++++++
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch
            { }
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
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmNext.BackColor = cVB.oVB_ColNormal;
                ocmNextPwd.BackColor = cVB.oVB_ColNormal; //*Arm 62-10-15  - ปรับให้เข้าตามธีม
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-01-28  AdaPos5.0
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-01-28  AdaPos5.0
                //ocmKB.BackColor = cVB.oVB_ColDark;
                //ocmBack.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                //ocmCalculate.BackColor = cVB.oVB_ColDark;
                //ocmShwKb.BackColor = cVB.oVB_ColDark;
                //ocmHelp.BackColor = cVB.oVB_ColDark;
                //ocmAbout.BackColor = cVB.oVB_ColDark;
                opbLogo.Image = new cCompany().C_GEToImageLogo();

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_SETxDesign : " + oEx.Message); }
        }

        private void W_SETxBackgroud()
        {
           
            string tImagePath = string.Empty;
            try
            {
                tImagePath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Background\splash.png";
                if (!File.Exists(tImagePath)) tImagePath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Background\splash.jpg";
                if (File.Exists(tImagePath))
                {
                    opnSignInPin.BackgroundImage = Image.FromFile(tImagePath);
                    opnSignInPin.BackgroundImageLayout = ImageLayout.Stretch;
                    opnSignInPwd.BackgroundImage = Image.FromFile(tImagePath);
                    opnSignInPwd.BackgroundImageLayout = ImageLayout.Stretch;
                    opnSignInRFID.BackgroundImage = Image.FromFile(tImagePath);
                    opnSignInRFID.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    opnSignInPin.BackgroundImage = Properties.Resources.BG;
                    opnSignInPin.BackgroundImageLayout = ImageLayout.Stretch;
                    opnSignInPwd.BackgroundImage = Properties.Resources.BG;
                    opnSignInPwd.BackgroundImageLayout = ImageLayout.Stretch;
                    opnSignInRFID.BackgroundImage = Properties.Resources.BG;
                    opnSignInRFID.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch (Exception oEx)
            {

                new cLog().C_WRTxLog("wSignin", "W_SETxBackgroud : " + oEx.Message);
            }
            finally
            {

            }
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
                        cVB.oVB_GBResource = new ResourceManager(typeof(resGlobal_TH));     //*Arm 63-02-24 
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        cVB.oVB_GBResource = new ResourceManager(typeof(resGlobal_EN));     //*Arm 63-02-24
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                olaSignIn.Text = oW_Resource.GetString("tSignin");
                olaTitleUsernamePin.Text = cVB.oVB_GBResource.GetString("tUsername");
                olaTitlePin.Text = oW_Resource.GetString("tPincode");

                // *Arm 62-10-31 
                olaTitleUsernamePwd.Text = cVB.oVB_GBResource.GetString("tUsername");
                olaPassword.Text = cVB.oVB_GBResource.GetString("tPassword");


                // *Arm 62-09-30  Msg lock Splash
                tLocSplashLastUser = oW_Resource.GetString("tLocSplashLastUser");
                tLocSplasMsghigh = oW_Resource.GetString("tLocSplasMsghigh");
                tLocSplasMsgLow = oW_Resource.GetString("tLocSplasMsgLow");


                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                // switch Image mode + Name
                switch (nW_Mode)
                {
                    case 0:
                    case 4:     //*Arm 62-09-30
                        opbSignIn.Image = Properties.Resources.SigninB_32;
                        olaSignIn.Text = oW_Resource.GetString("tSignin");
                        cVB.tVB_KbdScreen = "SIGNIN";
                        break;

                    case 1:
                        opbSignIn.Image = Properties.Resources.RoleB_32;
                        olaSignIn.Text = oW_Resource.GetString("tRole");
                        cVB.tVB_KbdScreen = "ROLE";
                        break;

                    case 2:
                        opbSignIn.Image = Properties.Resources.SwitchUserB_32;
                        olaSignIn.Text = oW_Resource.GetString("tSwitch");
                        cVB.tVB_KbdScreen = "SWITCHUSER";
                        break;
                    
                }

                //*Net 63-07-31 ปรับตาม Moshi
                this.Text = Assembly.GetExecutingAssembly().GetName().Name;   //*Em 63-07-11
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Check user password.
        /// </summary>

        /// <summary>
        /// Check user.
        /// </summary>
        private void W_CHKxUser(string ptSigninType, string ptInput_Code, string ptInput_Pwd, string ptUsrLogin)
        {
            List<cmlTCNMUser> aoUser;
            StringBuilder oSql = new StringBuilder();
            try
            {
                string tJoin = string.Empty;
                string tSelect = string.Empty;

                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                //ping 2019
                //oSql.AppendLine("SELECT USR.FTUsrCode, USL.FTUsrLogin as FTUsrName, USL.FTUsrLoginPwd as FTUsrPwd, USR.FTRolCode, USG.FTUsrStaShop ,USG.FTShpCode ,FTMerCode, ROL.FNRolLevel");
                //oSql.AppendLine("FROM TCNMUser USR WITH(NOLOCK) ");
                //oSql.AppendLine("INNER JOIN TCNTUsrGroup USG WITH(NOLOCK) ON USG.FTUsrCode = USR.FTUsrCode");
                //oSql.AppendLine("LEFT JOIN TCNMShop SHP WITH(NOLOCK) ON SHP.FTShpCode = ISNULL(USG.FTShpCode,'')");
                //oSql.AppendLine("INNER JOIN TCNMUsrLogin USL WITH(NOLOCK) ON USL.FTUsrCode = USR.FTUsrCode ");
                //oSql.AppendLine("INNER JOIN TCNMUsrRole ROL WITH(NOLOCK) ON ROL.FTRolCode = USR.FTRolCode");    //*Em 62-09-03
                //oSql.AppendLine("WHERE USL.FTUsrLogType = '"+ ptSigninType + "' ");
                //oSql.AppendLine("AND (ISNULL(USG.FTBchCode,'') = '' OR ISNULL(USG.FTBchCode,'') = '"+ cVB.tVB_BchCode +"')");   //*Em 62-08-07
                //oSql.AppendLine("AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),USL.FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),USL.FDUsrPwdExpired,121)) "); //*Em 62-09-10
                //if (ptSigninType != "3") oSql.AppendLine("AND USR.FTUsrCode = '"+ ptInput_Code + "'");  //*Em 62-09-10
                //if (ptSigninType != "3") oSql.AppendLine("AND USL.FTUsrLogin = '" + ptUsrLogin + "'");   //*Em 62-09-10
                //oSql.AppendLine("ORDER BY USR.FTUsrCode");

                //*Arm 63-06-16
                //oSql.AppendLine("SELECT DISTINCT USR.FTUsrCode, USL.FTUsrLogin as FTUsrName, USL.FTUsrLoginPwd as FTUsrPwd, AROL.FTRolCode, USG.FTUsrStaShop ,USG.FTShpCode ,FTMerCode");
                oSql.AppendLine("SELECT DISTINCT USR.FTUsrCode, USL.FTUsrLogin as FTUsrName, USL.FTUsrLoginPwd as FTUsrPwd, AROL.FTRolCode, USG.FTShpCode ,USG.FTMerCode, USL.FTUsrStaActive"); //*Arm 63-06-22 เพิ่ม FTUsrStaActive
                oSql.AppendLine("FROM TCNMUser USR WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TCNTUsrGroup USG WITH(NOLOCK) ON USG.FTUsrCode = USR.FTUsrCode");
                oSql.AppendLine("LEFT JOIN TCNMShop SHP WITH(NOLOCK) ON SHP.FTShpCode = ISNULL(USG.FTShpCode,'')");
                oSql.AppendLine("INNER JOIN TCNMUsrLogin USL WITH(NOLOCK) ON USL.FTUsrCode = USR.FTUsrCode ");
                oSql.AppendLine("LEFT JOIN TCNMUsrActRole AROL WITH(NOLOCK) ON AROL.FTUsrCode = USR.FTUsrCode"); //*Net 63-06-17 เปลี่ยนเป็น LEFT JOIN เพื่อให้ยังสามารถ login ได้
                oSql.AppendLine("WHERE USL.FTUsrLogType = '" + ptSigninType + "' ");
                oSql.AppendLine("AND (ISNULL(USG.FTBchCode,'') = '' OR ISNULL(USG.FTBchCode,'') = '" + cVB.tVB_BchCode + "')");
                oSql.AppendLine("AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),USL.FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),USL.FDUsrPwdExpired,121)) ");
                if (ptSigninType != "3") oSql.AppendLine("AND USR.FTUsrCode = '" + ptInput_Code + "'");
                if (ptSigninType != "3") oSql.AppendLine("AND USL.FTUsrLogin = '" + ptUsrLogin + "'"); 
                oSql.AppendLine("ORDER BY USR.FTUsrCode");
                //+++++++++++++++

                aoUser = new cDatabase().C_GETaDataQuery<cmlTCNMUser>(oSql.ToString());

                string tInput_Code = ptInput_Code;
                string tInput_Pwd = ptInput_Pwd;
                cmlTCNMUser oUser = new cmlTCNMUser();
                List<string> atUserRoleCode = new List<string>(); //*Net 63-06-17
                if (ptSigninType == "3")
                {
                    oUser = aoUser.Where(x => x.FTUsrPwd == new cEncryptDecrypt("2").C_CALtEncrypt(ptInput_Pwd)).FirstOrDefault();
                    atUserRoleCode = aoUser.Where(x => x.FTUsrPwd == new cEncryptDecrypt("2").C_CALtEncrypt(ptInput_Pwd)).Select(oUsr => oUsr.FTRolCode).Distinct().ToList(); //*Net 63-06-17 ดึง RoleCode ทั้งหมดออกมา
                    //oUser = aoUser.Where(x => x.FTUsrPwd == ptInput_Pwd).FirstOrDefault();
                }
                else
                {
                    oUser = aoUser.Where(x => x.FTUsrCode == tInput_Code).FirstOrDefault();
                    atUserRoleCode = aoUser.Where(x => x.FTUsrCode == tInput_Code).Select(oUsr => oUsr.FTRolCode).Distinct().ToList(); //*Net 63-06-17 ดึง RoleCode ทั้งหมดออกมา
                }
                oUser.FTRolCode = String.Join(",", atUserRoleCode.Select(oUsr => "'" + oUsr + "'").ToArray()); //*Net 63-06-12 join RoleCode ทั้งหมด เพื่อเอาไป Where IN
                //W_CHKxLogin(oUser, tInput_Pwd); //*Arm 63-06-22 Comment Code
                W_CHKxLogin(oUser, tInput_Pwd, ptUsrLogin); //*Arm 63-06-22
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_CHKxUser : " + oEx.Message);
            }
        }

        /// <summary>
        /// Check Login
        /// </summary>
        /// <param name="opUser"></param>
        /// <param name="ptInput_Pwd"></param>
        /// <param name="ptUsrLogin">User login *Arm 63-06-22 </param>
        private void W_CHKxLogin(cmlTCNMUser opUser, string ptInput_Pwd, string ptUsrLogin)
        {
            string tPwdEnc, tChkRole, tFuncCode;
            cmlTCNMUser oUser = new cmlTCNMUser();
            int nChkClear = 0;
            wProgress oAutoSync;
            wProgress oSetupPos;
            wBlank oBlankPage;
            wChangePassword oChangePassword;
            StringBuilder oSql; //*Arm 63-08-11
            try
            {
                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                oUser = opUser;
                if (oUser != null)
                {
                    tPwdEnc = new cEncryptDecrypt("2").C_CALtEncrypt(ptInput_Pwd); //[Ping][2019.06.05][ปิดไว้เพื่อเทสระบบ]
                    //tPwdEnc = ptInput_Pwd;
                    if (string.Equals(tPwdEnc, oUser.FTUsrPwd))
                    {
                        //*Arm 63-06-22 Check StaActive = 3 --> Change Password
                        if (oUser.FTUsrStaActive == "3")
                        {
                            oChangePassword = new wChangePassword(tPwdEnc, ptInput_Pwd, ptUsrLogin, oUser.FTUsrCode, tW_Type); //*Arm 63-08-12
                            if (oChangePassword.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }
                            else
                            {
                                return; //*Arm 63-08-20
                            }
                        }
                        //+++++++++++++

                        oBlankPage = new wBlank();
                        oSetupPos = new wProgress(2, false);
                        switch (nW_Mode)
                        {
                            case 0: // Sign in
                            case 2: // Switch User
                                //AutoSync
                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Auto Sync Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                                oBlankPage.Show();
                                oAutoSync = new wProgress(1, false);
                                oAutoSync.ShowDialog(); //*Net 63-05-20
                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Auto Sync End", cVB.bVB_AlwPrnLog); //*Net Stamp
                                
                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Setup Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                                oBlankPage.Show();
                                oBlankPage.Refresh();
                                oSetupPos.Owner = oBlankPage;
                                //Load Data
                                oSetupPos.Show(); //*Net 63-05-20
                                oSetupPos.W_SETxProgress(0);

                                cVB.tVB_UsrCode = oUser.FTUsrCode;
                                cVB.tVB_UsrName = oUser.FTUsrName;
                                cVB.tVB_RolCode = oUser.FTRolCode;
                                cVB.tVB_DptCode = oUser.FTDptCode;
                                //cVB.nVB_RolLevel = Convert.ToInt32(oUser.FNRolLevel);   //*Em 62-09-03

                                //*Arm 63-08-11
                                oSql = new StringBuilder();
                                oSql.AppendLine("SELECT ISNULL(FTUsrName,(SELECT TOP 1 FTUsrName FROM TCNMUser_L WITH(NOLOCK) WHERE FTUsrCode = Usr.FTUsrCode)) AS FTUsrName");
                                oSql.AppendLine("FROM TCNMUser_L Usr WITH(NOLOCK)");
                                oSql.AppendLine("WHERE FTUsrCode = '" + cVB.tVB_UsrCode + "'");
                                oSql.AppendLine("AND FNLngID = " + cVB.nVB_Language);

                                cVB.tVB_ShwUsrName = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                                //++++++++++++++

                                //*Net 63-07-31 แสดงผล User ที่ log in หน้าจอ 2
                                if (cVB.oVB_CstScreen != null)
                                {
                                    cVB.oVB_CstScreen.W_SETxClearDoc();
                                    cVB.oVB_CstScreen.W_SETxHeader(ptUsrName: cVB.tVB_UsrName);
                                }
                                //+++++++++++++++++++++++++++


                                oSetupPos.W_SETxProgress(10); //*Net 63-05-20 Setup Progress

                                this.Hide();
                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Get Comp", cVB.bVB_AlwPrnLog); //*Net Stamp
                                if (string.Equals(oUser.FTUsrStaShop, "3"))
                                {
                                    cVB.tVB_ShpCode = oUser.FTShpCode;
                                    cVB.tVB_Merchart = oUser.FTMerCode;
                                    if (!string.IsNullOrEmpty(cVB.tVB_ShpCode))
                                    {
                                        new cCompany().C_GETxShpName();
                                        new cCompany().C_GETxMerInfo(); //*Em 62-10-04
                                    }
                                }
                                else
                                {
                                    cVB.tVB_ShpCode = "";
                                    cVB.tVB_Merchart = "";
                                    cVB.tVB_MerName = "";   //*Em 62-10-04
                                    new cCompany().C_GETxCompany(); //*Em 62-10-04
                                }
                                cShift.C_PRCxFixDocByShfEvent(); //*Net 63-07-31 ปรับตาม Moshi
                                new Thread(() => cShift.C_GENxListenFixDoc(true)).Start(); //*Net 63-06-04

                                oSetupPos.W_SETxProgress(20); //*Net 63-05-20 Setup Progress

                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Process Pdt Price", cVB.bVB_AlwPrnLog); //*Net Stamp
                                //*Em 63-04-22
                                new cPdtPmt().C_PRCxPdtPrice();
                                oSetupPos.W_SETxProgress(30); //*Net 63-05-20 Setup Progress

                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Process Promotion Expired", cVB.bVB_AlwPrnLog);
                                new cPdtPmt().C_PRCxDelPmtExpired();    //*Em 63-06-02

                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Process PdtPmt", cVB.bVB_AlwPrnLog); //*Net Stamp
                                new cPdtPmt().C_PRCxPdtPmt();
                                oSetupPos.W_SETxProgress(50); //*Net 63-05-20 Setup Progress

                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Process PdtSale", cVB.bVB_AlwPrnLog); //*Net Stamp
                                new cPdt().C_PRCxPreparePdtSale();
                                oSetupPos.W_SETxProgress(70); //*Net 63-05-20 Setup Progress

                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Get SlipMsg", cVB.bVB_AlwPrnLog); //*Net Stamp
                                cSlipMsg.C_GETxSlipMsg();   //*Em 63-05-16
                                oSetupPos.W_SETxProgress(80); //*Net 63-05-20 Setup Progress

                                new cLog().C_CLRxDataLog(); //*Em 63-05-16
                                cSale.C_DATxUsrLog();   //*Em 63-06-09
                                if(nW_Mode == 0) cSale.C_PRCxUpdateStatistics(); //*Em 63-07-11 //*Net 63-07-31 ปรับตาม Moshi
                                //++++++++++++++++++
                                //*Net 63-05-21 ย้ายไปเช็คข้างล่าง
                                //new cShift().C_CHKxTypeOpenShift(oUser.FTUsrCode);  // ตรวจสอบการเปิดรอบ

                                DialogResult = DialogResult.OK;

                                break;
                            case 1: // Role
                                if (string.IsNullOrEmpty(tW_ScreenRole)) tW_ScreenRole = cVB.tVB_KbdScreen;

                                //*[AnUBiS][][2019-01-09] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(tW_ScreenRole);
                                tChkRole = new cUser().C_CHKtUserRole(oUser.FTRolCode, tW_ScreenRole, tFuncCode); //*Net 63-06-17
                                //tChkRole = new cUser().C_CHKtUserRole((int)oUser.FNRolLevel, tW_ScreenRole, tFuncCode);  //*Em 62-09-03

                                switch (tChkRole)
                                {
                                    case "1":   // allowed.
                                        this.W_SVNxSaveOverrideEvent(oUser.FTRolCode, tW_ScreenRole);
                                        DialogResult = DialogResult.OK;
                                        break;
                                    case "0":   // not permission.
                                    case "800": // data not found.
                                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotOpen"), 3);
                                        return;
                                    case "900":
                                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                                        return;
                                }

                                /*
                                tChkRole = new cUser().C_CHKtUserRole(oUser.FTRolCode, tW_ScreenRole);

                                if (string.IsNullOrEmpty(tChkRole))
                                {
                                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotOpen"), 3);
                                    return;
                                }
                                else
                                    DialogResult = DialogResult.OK;
                                */
                                break;

                            case 4: //*Arm 62-09-30 Lock Splash

                                if (cVB.tVB_UsrCode == oUser.FTUsrCode)
                                {
                                    // ผู้ใช้งานคนเดิม
                                    cVB.tVB_KbdScreen = "Home";
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else 
                                {
                                    // ผู้ใช้ไม่ใช่งานคนเดิม
                                    //if (Convert.ToInt32(oUser.FNRolLevel) > cVB.nVB_RolLevel)   
                                    if (oUser.FTRolCode != cVB.tVB_RolCode)
                                    {
                                        // ผู้ใช้ไม่ใช่งานคนเดิม สิทธิสูงกว่าคนเดิม
                                        DialogResult odgMessage = MessageBox.Show(tLocSplasMsghigh, tLocSplashLastUser + " " + cVB.tVB_UsrName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                        if (odgMessage == DialogResult.Yes)
                                        {
                                            Environment.Exit(1);
                                            //new cFunctionKeyboard().C_KBDxExit();
                                        }
                                        else if (odgMessage == DialogResult.No)
                                        {
                                            this.DialogResult = DialogResult.No;
                                            this.Close();
                                        }

                                    }
                                    else 
                                    {
                                        MessageBox.Show(tLocSplasMsgLow + ", " + tLocSplashLastUser + " " + cVB.tVB_UsrName);
                                        this.DialogResult = DialogResult.No;
                                        this.Close();

                                    }
                                }
                                break;
                        }
                        new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Get RedeemTmp", cVB.bVB_AlwPrnLog); //*Net Stamp
                        //*Arm 63-03-24 - Get Redeem ลง Table Temp
                        if (W_PRCbGetRedeem2Tmp() == false) //Get Redeem Point ลง TPSTRedeemHD_Tmp
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgGetRedeemNotSuccess"), 1);
                        }

                        new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Get CpnTmp", cVB.bVB_AlwPrnLog); //*Net Stamp
                        if (W_PRCbGetRedeemCoupon2Tmp() == false) //Get Coupon  ลง TPSTCouponHDTmp
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgGetCouponNotSuccess"), 1);
                        }
                        //+++++++++++++++++

                        //*Arm 62-10-30 - QMember
                        if (cVB.oVB_MQ_Member != null)
                        {
                            cVB.oVB_MQ_Member.C_DISxDisConnect();
                        }

                        if (!string.IsNullOrEmpty(cVB.tVB_ShpCode))
                        {
                            cVB.oVB_MQ_Member = new cRabbitMQ("PS_QMember" + cVB.tVB_ShpCode, "");  //*Arm 62-10-25 - QMember
                        }
                        //new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Sync Upload"); //*Net Stamp
                        //new cSyncData().C_PRCxSyncUld();

                        oSetupPos.W_SETxProgress(90); //*Net 63-05-20 Setup Progress

                        new cSale().C_PRCxCheckAndCreateTableTemp(); //*Net 63-05-25

                        //*Arm 63-08-13 List PlayNow ใหม่ สำหรับเล่นหน้าจอ 2
                        if (cVB.oVB_CstScreen != null)
                        {
                            cVB.oVB_CstScreen.aoW_AdMsg = cCstScreen.C_GETaListAdMsg();
                            cVB.oVB_CstScreen.W_SETxMedia();
                        }
                        //++++++++++++

                        oSetupPos.W_SETxProgress(100); //*Net 63-05-20 Setup Progress
                        oSetupPos.Close();
                        switch (nW_Mode)
                        {
                            case 0:
                            case 2:
                                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Check Open Shift", cVB.bVB_AlwPrnLog); //*Net Stamp
                                new cShift().C_CHKxTypeOpenShift(oUser.FTUsrCode); //*Net 63-05-21
                                
                                break;
                        }

                        otmClose.Start();
                        nChkClear = 1;

                        //*Net 63-05-21
                        if(oBlankPage!=null && oBlankPage.Visible)
                        {
                            oBlankPage.Close();
                        }
                        //+++++++++++++
                    }
                    else
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsr"), 3);
                }
                else
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsrname"), 3);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_CHKxLogin : " + oEx.Message); }
            finally
            {
                oSql = null; //*Arm 63-08-11
                tPwdEnc = null;
                tChkRole = null;
                oUser = null;
                oAutoSync = null; //*Net 63-05-21
                oBlankPage = null; //*Net 63-05-21
                oSetupPos = null; //*Net 63-05-21
                oChangePassword = null; //*Arm 63-06-22
                if (nChkClear == 0)
                {
                    otbPin.Clear();
                    otbUserPin.Focus();
                    oucNumpad.oU_TextValue.Clear();
                }
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// *Arm 63-03-09 สร้างฟังก์ชัน
        /// *Arm 63-03-24 ย้ายมาจาก wOpenShif
        /// Call Redeem Poit ลง TPSTRedeemHD_Tmp
        /// </summary>
        /// <returns></returns>
        public bool W_PRCbGetRedeem2Tmp()
        {
            cDatabase oDatabase;
            SqlParameter[] aoSqlParam;

            try
            {
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptSaleDate", SqlDbType.VarChar, 10){ Value = DateTime.Now.ToString("yyyy-MM-dd")},
                    new SqlParameter ("@ptBchReq", SqlDbType.VarChar, 5){ Value = cVB.tVB_BchCode ?? string.Empty},
                    new SqlParameter ("@ptMerchantReq", SqlDbType.VarChar, 5){ Value = cVB.tVB_Merchart ?? string.Empty},
                    new SqlParameter ("@ptShopReq", SqlDbType.VarChar, 5){ Value = cVB.tVB_ShpCode ?? string.Empty},
                    new SqlParameter ("@FNResult", SqlDbType.Int) {
                        Direction = ParameterDirection.Output }
                };

                oDatabase = new cDatabase();

                if (oDatabase.C_DATbExecuteStoreProcedure(cVB.oVB_Config, "STP_PRCxGetRedeem", ref aoSqlParam, "@FNResult") == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wOpenShift", "W_PRCbGetRedeem : " + oEx.Message);
                return false;
            }
            finally
            {
                oDatabase = null;
                aoSqlParam = null;
            }
        }

        /// <summary>
        /// *Arm 63-03-24 ย้ายมาจาก wOpenShif
        /// Get RedeemCoupon ลง TPSTCouponHDTmp
        /// </summary>
        /// <returns></returns>
        public bool W_PRCbGetRedeemCoupon2Tmp()
        {
            cDatabase oDatabase;
            SqlParameter[] aoSqlParam;
            try
            {
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptSaleDate", SqlDbType.VarChar, 10){ Value = DateTime.Now.ToString("yyyy-MM-dd")},
                    new SqlParameter ("@ptBchReq", SqlDbType.VarChar, 5){ Value = cVB.tVB_BchCode ?? string.Empty},
                    new SqlParameter ("@ptMerchantReq", SqlDbType.VarChar, 5){ Value = cVB.tVB_Merchart ?? string.Empty},
                    new SqlParameter ("@ptShopReq", SqlDbType.VarChar, 5){ Value = cVB.tVB_ShpCode ?? string.Empty},
                    new SqlParameter ("@FNResult", SqlDbType.Int) {
                        Direction = ParameterDirection.Output }
                };

                oDatabase = new cDatabase();

                if (oDatabase.C_DATbExecuteStoreProcedure(cVB.oVB_Config, "STP_PRCxGetCoupon", ref aoSqlParam, "@FNResult") == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wOpenShift", "W_PRCbGetRedeemCoupon2Tmp : " + oEx.Message);
                return false;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Check pin code.
        /// </summary>
        private void W_CHKxPincode()
        {
            string tPwdEnc, tChkRole, tFuncCode;
            cmlTCNMUser oUser;
            int nChkClear = 0;

            try
            {
                if (string.IsNullOrEmpty(otbUserPin.Text) || string.IsNullOrEmpty(otbPin.Text))
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputUsrPwd"), 3);
                else
                {
                    oUser = (from User in aoW_User where User.FTUsrName == otbUserPin.Text select User).FirstOrDefault();

                    if (oUser != null)
                    {
                        tPwdEnc = new cEncryptDecrypt("2").C_CALtEncrypt(otbPin.Text);

                        if (string.Equals(tPwdEnc, oUser.FTUsrPwd))
                        {
                            switch (nW_Mode)
                            {
                                case 0: // Sign in
                                case 2: // Switch User
                                    cVB.tVB_UsrCode = oUser.FTUsrCode;
                                    cVB.tVB_UsrName = oUser.FTUsrName;
                                    cVB.tVB_RolCode = oUser.FTRolCode;
                                    cVB.tVB_DptCode = oUser.FTDptCode;
                                    if (string.Equals(oUser.FTUsrStaShop, "1"))
                                    {
                                        cVB.tVB_ShpCode = oUser.FTShpCode;
                                        new cCompany().C_GETxShpName();
                                    }

                                    new cShift().C_CHKxTypeOpenShift(oUser.FTUsrCode);  // ตรวจสอบการเปิดรอบ
                                    DialogResult = DialogResult.OK;

                                    break;

                                case 1: // Role

                                    //*[AnUBiS][][2019-01-09] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                                    tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(tW_ScreenRole);
                                    tChkRole = new cUser().C_CHKtUserRole(oUser.FTRolCode, tW_ScreenRole, tFuncCode); //*Net 63-06-17
                                    //tChkRole = new cUser().C_CHKtUserRole((int)oUser.FNRolLevel, tW_ScreenRole, tFuncCode);  //*Em 62-09-03

                                    switch (tChkRole)
                                    {
                                        case "1":   // allowed.
                                            this.W_SVNxSaveOverrideEvent(oUser.FTRolCode, tW_ScreenRole);
                                            DialogResult = DialogResult.OK;
                                            break;
                                        case "0":   // not permission.
                                        case "800": // data not found.
                                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotOpen"), 3);
                                            return;
                                        case "900":
                                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                                            return;
                                    }

                                    /*
                                    tChkRole = new cUser().C_CHKtUserRole(oUser.FTRolCode, tW_ScreenRole);

                                    if (string.IsNullOrEmpty(tChkRole))
                                    {
                                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCannotOpen"), 3);
                                        return;
                                    }
                                    else
                                        DialogResult = DialogResult.OK;
                                    */
                                    break;

                            }

                            otmClose.Start();
                            nChkClear = 1;
                        }
                        else
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsr"), 3);
                    }
                    else
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsrname"), 3);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_CHKxPincode : " + oEx.Message); }
            finally
            {
                tPwdEnc = null;
                tChkRole = null;
                oUser = null;

                if (nChkClear == 0)
                {
                    otbPin.Clear();
                    otbUserPin.Focus();
                    oucNumpad.oU_TextValue.Clear();
                }

                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="pnMode">
        /// Show form mode.
        /// <para>0: show in mode signin.</para>
        /// <para>1: show in mode override permission (role).</para>
        /// </param>
        private void W_GETxUser(DataGridView pogdGrid)
        {
            List<cmlTCNMUser> aoUser;
            String tTypePwd = "";
            try
            {
                switch (pogdGrid.Name.ToString())
                {
                    case "ogdUserPwd":
                        tTypePwd = "1";
                        break;
                    case "ogdUserPin":
                        tTypePwd = "2";
                        break;
                }

                aoUser = new cUser().C_GETaLastUser(tTypePwd);    // Get last User

                //if (aoUser.Count == 0)
                //{
                //    aoUser = new cUser().C_GETaUser();
                //    aoW_User = aoUser;
                //}
                //else
                //    aoW_User = new cUser().C_GETaUser();

                ////*[AnUBiS][][2019-01-08] - Check mode show form signin or override permission (role).
                //if (pnMode == 1)
                //{
                //    // show form in mode override permission (role).
                //    // not set user in interface.
                //    return;
                //}

                //// ดึงข้อมูลใส่ Combobox
                //ocbUserPin.DisplayMember = "FTUsrName";
                //ocbUserPin.ValueMember = "FTUsrCode";
                //ocbUserPin.DataSource = aoW_User;
                if (aoUser != null || aoUser.Count > 0)
                {
                    for (int nCount = 0; nCount < 3; nCount++)
                    {
                        if (nCount < aoUser.Count)
                            pogdGrid.Rows.Add(null, aoUser[nCount].FTUsrName, aoUser[nCount].FTUsrCode);
                    }
                }

                pogdGrid.Rows.Add(null, "Other", "");
                pogdGrid.ClearSelection(); //*Arm 63-08-15
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_GETxUser : " + oEx.Message); }
            finally
            {
                aoUser = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT;  //*Em 62-01-28  AdaPos 5.0
            List<cmlTPSMFunc> aoMenuB;  //*Em 62-01-28  AdaPos 5.0
            int nItem;  //*Em 62-01-28  AdaPos 5.0
            try
            {
                aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                //foreach (cmlTPSMFunc oKb in aoKb)
                //{
                //    switch (oKb.FTSysCode)
                //    {
                //        case "KB010":
                //            ocmHelp.Visible = true;
                //            ocmHelp.Text = "".PadLeft(10) + oKb.FTGdtName;
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

                //        case "KB003":
                //            ocmBack.Visible = true;
                //            ocmBack.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;
                //    }
                //}

                //*Em 62-01-28  AdaPos 5.0
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

                        //*Arm 63-02-24 เพิ่มฟังก์ชั่นเปลี่ยนภาษา
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
                        //+++++++++++++++++++++++++

                        //try
                        //{
                        //    ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        //}
                        //catch { }

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 260;
                        opnMenuB.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuB.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuB.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }
                //+++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_SHWxButtonBar : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_SETxOpenCloseMenu : " + oEx.Message); }
            finally
            {
                otbUserPin.Focus();
            }
        }

        /// <summary>
        /// Process Choose last user
        /// </summary>
        private void W_PRCxChooseLastUser()
        {
            int nIndex;

            try
            {
                if (ogdUserPin.CurrentRow.Cells[1].Value.ToString() == "Other")
                {
                    cmlSearch oSearch;
                    wSearch2Column oSch2Col;

                    oSch2Col = new wSearch2Column("GETUserPin");
                    oSch2Col.ShowDialog();

                    if (oSch2Col.DialogResult == DialogResult.OK)
                    {
                        oSearch = oSch2Col.oW_DataSearch;
                        otbUserPin.Text = oSearch.tName;
                        otbUserPin.Tag = oSearch.tCode;
                        otbPin.Focus();
                    }
                    return;
                }

                nIndex = aoW_User.FindIndex(c => c.FTUsrCode == ogdUserPin.CurrentRow.Cells[2].Value.ToString());
                opbUser.Image = new cUser().C_GEToImageUsr(otbUserPin.Text,"TCNMUser");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_PRCxChooseLastUser : " + oEx.Message); }
            finally
            {
                ogdUserPin.ClearSelection();
                otbPin.Focus();
            }
        }

        /// <summary>
        /// Process Choose last user for type password and pin
        /// </summary>
        private void W_PRCxChooseLastUserForPwdAndPin(string ptProcess, string tSelectGridValue="")
        {
            try
            {
                //*Em 62-06-18
                switch (ptProcess)
                {
                    
                    case "GETUserPin":
                        if (ogdUserPin.CurrentRow.Cells[1].Value.ToString() == "Other")
                        {
                            cmlSearch oSearch;
                            wSearch2Column oSch2Col;

                            oSch2Col = new wSearch2Column(ptProcess);
                            oSch2Col.ShowDialog();

                            if (oSch2Col.DialogResult == DialogResult.OK)
                            {
                                oSearch = oSch2Col.oW_DataSearch;
                                if (ptProcess == "GETUserPin")
                                {
                                    otbUserPin.Text = oSearch.tName;
                                    otbUserPin.Tag = oSearch.tCode;
                                    opbUser.Image = new cUser().C_GEToImageUsr(oSearch.tCode, "TCNMUser");
                                    otbPin.Focus();
                                }
                            }
                            else
                            {
                                otbUserPin.Text = "";
                                otbUserPin.Tag = "";
                                opbUser.Image = null;
                                otbPin.Focus();
                            }
                            return;
                        }
                        else
                        {
                            otbUserPin.Text = ogdUserPin.CurrentRow.Cells[1].Value.ToString();
                            otbUserPin.Tag = ogdUserPin.CurrentRow.Cells[2].Value.ToString();
                            opbUser.Image = new cUser().C_GEToImageUsr(ogdUserPin.CurrentRow.Cells[2].Value.ToString(), "TCNMUser");
                            otbPin.Focus();
                        }
                        break;
                    case "GETUserPwd":
                        if (ogdUserPwd.CurrentRow.Cells[1].Value.ToString() == "Other")
                        {
                            cmlSearch oSearch;
                            wSearch2Column oSch2Col;

                            oSch2Col = new wSearch2Column(ptProcess);
                            oSch2Col.ShowDialog();

                            if (oSch2Col.DialogResult == DialogResult.OK)
                            {
                                oSearch = oSch2Col.oW_DataSearch;
                                if (ptProcess == "GETUserPwd")
                                {
                                    otbUsername.Text = oSearch.tName;
                                    otbUsername.Tag = oSearch.tCode;
                                    opbSigninPwd.Image = new cUser().C_GEToImageUsr(oSearch.tCode, "TCNMUser");
                                    otbPassword.Focus();
                                }
                            }
                            else
                            {
                                otbUsername.Text = "";
                                otbUsername.Tag = "";
                                opbSigninPwd.Image = null;
                                otbPassword.Focus();
                            }
                            return;
                        }
                        else
                        {
                            otbUsername.Text = ogdUserPwd.CurrentRow.Cells[1].Value.ToString();
                            otbUsername.Tag = ogdUserPwd.CurrentRow.Cells[2].Value.ToString();
                            opbSigninPwd.Image = new cUser().C_GEToImageUsr(ogdUserPwd.CurrentRow.Cells[2].Value.ToString(), "TCNMUser");
                            otbPassword.Focus();
                        }
                        break;

                    case "GETUserLogin": //*Arm 63-08-06 ตรวจสอบ Type การ Login จากการเลือก User
                        if (tSelectGridValue == "Other")
                        {
                            cmlSearch oSearch;
                            wSearch2Column oSch2Col;

                            oSch2Col = new wSearch2Column(ptProcess);
                            oSch2Col.ShowDialog();

                            if (oSch2Col.DialogResult == DialogResult.OK)
                            {
                                oSearch = oSch2Col.oW_DataSearch;
                                W_GETxLogin(oSearch.tCode); //*Arm 63-08-15

                                //switch (oSearch.tType)
                                //{
                                //    case "1":
                                //        ogdUserPwd.Rows.Clear();
                                //        ogdUserPwd.Visible = true;
                                //        ogdUserPin.Visible = false;
                                //        W_SETxPanelSignType("1");

                                //        otbUsername.Text = oSearch.tName;
                                //        otbUsername.Tag = oSearch.tCode;
                                //        opbSigninPwd.Image = new cUser().C_GEToImageUsr(oSearch.tCode, "TCNMUser");
                                //        otbPassword.Focus();

                                //        break;
                                //    case "2":

                                //        ogdUserPin.Rows.Clear();
                                //        ogdUserPwd.Visible = false;
                                //        ogdUserPin.Visible = true;
                                //        W_SETxPanelSignType("2");

                                //        otbUserPin.Text = oSearch.tName;
                                //        otbUserPin.Tag = oSearch.tCode;
                                //        opbUser.Image = new cUser().C_GEToImageUsr(oSearch.tCode, "TCNMUser");
                                //        otbPin.Focus();
                                //        break;
                                //}

                                ////*Arm 63-08-19
                                //string tUsrLogin = "";
                                //StringBuilder oSql = new StringBuilder();

                                //// Pin
                                //oSql.Clear();
                                //oSql.AppendLine("SELECT TOP 1 FTUsrLogin as 'tName'");
                                //oSql.AppendLine("FROM TCNMUsrLogin  WITH(NOLOCK) ");
                                //oSql.AppendLine("WHERE FTUsrCode = '" + oSearch.tCode + "' AND FTUsrStaActive != '2' AND FTUsrLogType = '2' AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),FDUsrPwdExpired,121)) ");
                                
                                //tUsrLogin = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                                //if (!string.IsNullOrEmpty(tUsrLogin))
                                //{
                                //    ogdUserPin.Rows.Clear();
                                //    ogdUserPwd.Visible = false;
                                //    ogdUserPin.Visible = true;
                                //    W_SETxPanelSignType("2");

                                //    otbUserPin.Text = tUsrLogin;
                                //    otbUserPin.Tag = oSearch.tCode;
                                //    opbUser.Image = new cUser().C_GEToImageUsr(oSearch.tCode, "TCNMUser");
                                //    otbPin.Focus();
                                //}
                                //else
                                //{
                                //    otbUserPin.Text = "";
                                //    otbUserPin.Tag = "";
                                //    opbUser.Image = null;
                                //    otbPin.Focus();
                                //}

                                ////Password
                                //tUsrLogin = "";

                                //oSql.Clear();
                                //oSql.AppendLine("SELECT TOP 1 FTUsrLogin as 'tName'");
                                //oSql.AppendLine("FROM TCNMUsrLogin  WITH(NOLOCK) ");
                                //oSql.AppendLine("WHERE FTUsrCode = '" + oSearch.tCode + "' AND FTUsrStaActive != '2' AND FTUsrLogType = '1' AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),FDUsrPwdExpired,121)) ");
                                
                                //tUsrLogin = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                                //if (!string.IsNullOrEmpty(tUsrLogin))
                                //{
                                //    ogdUserPwd.Rows.Clear();
                                //    ogdUserPwd.Visible = true;
                                //    ogdUserPin.Visible = false;
                                //    W_SETxPanelSignType("1");

                                //    otbUsername.Text = tUsrLogin;
                                //    otbUsername.Tag = oSearch.tCode;
                                //    opbSigninPwd.Image = new cUser().C_GEToImageUsr(oSearch.tCode, "TCNMUser");
                                //    otbPassword.Focus();
                                //}
                                //else
                                //{
                                //    otbUsername.Text = "";
                                //    otbUsername.Tag = "";
                                //    opbSigninPwd.Image = null;
                                //    otbPassword.Focus();
                                //}
                                ////++++++++++++++
                            }
                            else
                            {
                                //if (opnSignInPin.Visible == true)
                                //{
                                //    otbUserPin.Text = "";
                                //    otbUserPin.Tag = "";
                                //    opbUser.Image = null;
                                //    otbPin.Focus();
                                //}
                                //if (opnSignInPwd.Visible == true)
                                //{
                                //    otbUsername.Text = "";
                                //    otbUsername.Tag = "";
                                //    opbSigninPwd.Image = null;
                                //    otbPassword.Focus();
                                //}
                            }
                            return;
                        }
                        else
                        {
                            //if (opnSignInPin.Visible == true)
                            //{
                            //    otbUserPin.Text = ogdUserPin.CurrentRow.Cells[1].Value.ToString();
                            //    otbUserPin.Tag = ogdUserPin.CurrentRow.Cells[2].Value.ToString();
                            //    opbUser.Image = new cUser().C_GEToImageUsr(ogdUserPin.CurrentRow.Cells[2].Value.ToString(), "TCNMUser");
                            //    otbPin.Focus();
                            //}

                            //if (opnSignInPwd.Visible == true)
                            //{
                            //    otbUsername.Text = ogdUserPwd.CurrentRow.Cells[1].Value.ToString();
                            //    otbUsername.Tag = ogdUserPwd.CurrentRow.Cells[2].Value.ToString();
                            //    opbSigninPwd.Image = new cUser().C_GEToImageUsr(ogdUserPwd.CurrentRow.Cells[2].Value.ToString(), "TCNMUser");
                            //    otbPassword.Focus();
                            //}

                            //*Arm 63-08-15
                            if (opnSignInPin.Visible == true)
                            {
                                W_GETxLogin(ogdUserPin.CurrentRow.Cells[2].Value.ToString());
                            }
                            else
                            {
                                W_GETxLogin(ogdUserPwd.CurrentRow.Cells[2].Value.ToString());
                            }
                            //+++++++++++++++
                        }
                        break;
                }
                //++++++++++++++++
               

                //nIndex = aoW_User.FindIndex(c => c.FTUsrCode == ogdUserPin.CurrentRow.Cells[2].Value.ToString());
                //ocbUserPin.SelectedIndex = nIndex;
                //opbUser.Image = new cUser().C_GEToImageUsr(ocbUserPin.SelectedValue.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_PRCxChooseLastUser : " + oEx.Message);
            }
            finally
            {
                ogdUserPin.ClearSelection();
                ogdUserPwd.ClearSelection();
                //otbPin.Focus();
            }
        }

        /// <summary>
        /// แสดง Login (*Arm 63-08-15)
        /// </summary>
        /// <param name="ptUsrCode"></param>
        public void W_GETxLogin(string ptUsrCode)
        {
            string tUsrLogin = "";
            try
            {
                if (string.IsNullOrEmpty(ptUsrCode)) return;

                otbUserPin.Text = "";
                otbUserPin.Tag = "";
                otbPin.Text = "";
                otbUsername.Text = "";
                otbUsername.Tag = "";
                otbPassword.Text = "";

                if (opnSignInPin.Visible == true)
                {
                    //ถ้าอยู่ page login type PIN
                    tUsrLogin = "";
                    tUsrLogin = cUser.C_GETtUsrLogin(ptUsrCode, "2"); //Get user login
                    if (!string.IsNullOrEmpty(tUsrLogin))
                    {
                        //กรณีมี Login PIN
                        //Show Login PIN
                        otbUserPin.Text = tUsrLogin;
                        otbUserPin.Tag = ptUsrCode;
                        opbUser.Image = new cUser().C_GEToImageUsr(ptUsrCode, "TCNMUser");
                        otbPin.Focus();

                        //เตรียม Login Password
                        tUsrLogin = "";
                        tUsrLogin = cUser.C_GETtUsrLogin(ptUsrCode, "1"); //Get user login
                        otbUsername.Text = tUsrLogin;
                        otbUsername.Tag = ptUsrCode;
                        opbSigninPwd.Image = new cUser().C_GEToImageUsr(ptUsrCode, "TCNMUser");
                        otbPassword.Focus();

                    }
                    else
                    {
                        //กรณีไม่มี Login PIN
                        //Clear PIN
                        otbUserPin.Text = "";
                        otbUserPin.Tag = ptUsrCode;
                        opbUser.Image = new cUser().C_GEToImageUsr(ptUsrCode, "TCNMUser");
                        otbPin.Focus();

                        //หา Login Password
                        tUsrLogin = "";
                        tUsrLogin = cUser.C_GETtUsrLogin(ptUsrCode, "1");

                        if (!string.IsNullOrEmpty(tUsrLogin))
                        {
                            W_SETxPanelSignType("1"); //Open page Password
                        }
                        otbUsername.Text = tUsrLogin;
                        otbUsername.Tag = ptUsrCode;
                        opbSigninPwd.Image = new cUser().C_GEToImageUsr(ptUsrCode, "TCNMUser");
                        otbPassword.Focus();
                    }
                }
                else
                {
                    //ถ้าอยู่ page login type Password

                    tUsrLogin = "";
                    tUsrLogin = cUser.C_GETtUsrLogin(ptUsrCode, "1");
                    if (!string.IsNullOrEmpty(tUsrLogin))
                    {
                        //กรณีมี Login Password
                        //Show Login Password
                        otbUsername.Text = tUsrLogin;
                        otbUsername.Tag = ptUsrCode;
                        opbSigninPwd.Image = new cUser().C_GEToImageUsr(ptUsrCode, "TCNMUser");
                        otbPassword.Focus();

                        //เตรียม Login PIN
                        tUsrLogin = "";
                        tUsrLogin = cUser.C_GETtUsrLogin(ptUsrCode, "2");
                        otbUserPin.Text = tUsrLogin;
                        otbUserPin.Tag = ptUsrCode;
                        opbUser.Image = new cUser().C_GEToImageUsr(ptUsrCode, "TCNMUser");
                        otbPin.Focus();
                    }
                    else
                    {
                        //กรณีไม่มี Login Password
                        //Clear Password
                        otbUsername.Text = "";
                        otbUsername.Tag = ptUsrCode;
                        opbSigninPwd.Image = new cUser().C_GEToImageUsr(ptUsrCode, "TCNMUser");
                        otbPassword.Focus();

                        //หา pin
                        tUsrLogin = "";
                        tUsrLogin = cUser.C_GETtUsrLogin(ptUsrCode, "2");
                        if (!string.IsNullOrEmpty(tUsrLogin))
                        {
                            W_SETxPanelSignType("2");  //Open page PIN
                        }
                        otbUserPin.Text = tUsrLogin;
                        otbUserPin.Tag = ptUsrCode;
                        opbUser.Image = new cUser().C_GEToImageUsr(ptUsrCode, "TCNMUser");
                        otbPin.Focus();
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_GETxLogin : " + oEx.Message);
            }
            finally
            {
                ptUsrCode = string.Empty;
            }
        }

        /// <summary>
        /// Focus last user
        /// </summary>
        private void W_PRCxFocusLastUser()
        {
            try
            {
                if (ogdUserPin.Rows.Count > 0)
                {
                    ogdUserPin.Focus();
                    ogdUserPin.Rows[0].Selected = true;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_PRCxFocusLastUser : " + oEx.Message); }
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
                new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                W_GETxFuncByFuncName(tFuncName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_CALxByName : " + oEx.Message); }
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

                    case "C_KBDxLastUser":
                        W_PRCxFocusLastUser();
                        break;

                    case "C_KBDxBack":
                        otmClose.Start();
                        break;

                    case "C_KBDxChooseUser":
                        //otbUserPin.Focus();
                        //otbUserPin.DroppedDown = true;
                        switch (tW_Type)
                        {
                            case "1":
                                W_PRCxChooseUser("GETUserPwd");
                                break;
                            case "2":
                                W_PRCxChooseUser("GETUserPin");
                                break;
                        }
                        break;

                    case "C_KBDxInputPassword":
                        //ocbUserPin.DroppedDown = false;
                        otbPin.Focus();
                        break;

                    case "C_KBDxNotify":
                        W_CHKxNotify();
                        break;

                    case "C_KBDxNext":
                        W_CHKxPincode();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }
        /// <summary>
        /// ChooseUser By HotKey
        /// </summary>
        /// <param name="ptGETUserType"></param>
        public void W_PRCxChooseUser(string ptGETUserType)
        {
            try
            {
                cmlSearch oSearch;
                wSearch2Column oSch2Col;

                oSch2Col = new wSearch2Column(ptGETUserType);
                oSch2Col.ShowDialog();

                if (oSch2Col.DialogResult == DialogResult.OK)
                {
                    oSearch = oSch2Col.oW_DataSearch;
                    switch (ptGETUserType)
                    {
                        case "GETUserPwd":
                            otbUsername.Text = oSearch.tName;
                            otbUsername.Tag = oSearch.tCode;
                            opbSigninPwd.Image = new cUser().C_GEToImageUsr(oSearch.tCode, "TCNMUser");
                            otbPassword.Focus();
                            break;
                        case "GETUserPin":
                            otbUserPin.Text = oSearch.tName;
                            otbUserPin.Tag = oSearch.tCode;
                            opbUser.Image = new cUser().C_GEToImageUsr(oSearch.tCode, "TCNMUser");
                            otbPin.Focus();
                            break;
                    }

                }
                else
                {
                    switch (ptGETUserType)
                    {
                        case "GETUserPwd":
                            otbUsername.Text = "";
                            otbUsername.Tag = "";
                            opbSigninPwd.Image = null;
                            otbPassword.Focus();
                            break;

                        case "GETUserPin":
                            otbUserPin.Text = "";
                            otbUserPin.Tag = "";
                            opbUser.Image = null;
                            otbPin.Focus();
                            break;
                    }

                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_PRCxChooseUser : " + oEx.Message);
            }
            finally
            {
                ogdUserPin.ClearSelection();
                ogdUserPwd.ClearSelection();
                //otbPin.Focus();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_PRCxAcceptSignalR : " + oEx.Message); }
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
                            W_GETxCountNotify(0);   //*[AnUBiS][][2019-01-08] - fix mode signin.
                        }));
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_CHKxMsg : " + oEx.Message); }
        }

        /// <summary>
        /// Get Notify
        /// </summary>
        /// <param name="pnMode">
        /// Show form mode.
        /// <para>0: show in mode signin.</para>
        /// <para>1: show in mode override permission (role).</para>
        /// </param>
        private void W_GETxCountNotify(int pnMode)
        {
            List<cmlTCNTMsgRemind> aoMsgRemind;
            int nCountMsg;

            try
            {
                //*[AnUBiS][][2019-01-08] - Check mode show form signin or override permission (role).
                if (pnMode == 1)
                {
                    // show form in mode override permission (role).
                    // not show notify.
                    ocmNotify.Visible = false;
                    return;
                }

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_GETxCountNotify : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "W_CHKxNotify : " + oEx.Message); }
        }

        /// <summary>
        /// Save shift event override permission.
        /// </summary>
        /// 
        /// <param name="ptAvpCode">Approve code.</param>
        /// <param name="ptScreenRole">shift event remark.</param>
        /// 
        /// <remarks>*[AnUBiS][][2019-01-09] - create function.</remarks>
        private void W_SVNxSaveOverrideEvent(string ptAvpCode, string ptScreenRole)
        {
            StringBuilder oSql;
            cmlTPSTShiftEvent oEvent;
            string tEvnCode;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTEvnCode");
                oSql.AppendLine("FROM TSysShiftEvent_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTEvnFuncRef = 'fC_Allow'");
                oSql.AppendLine("AND FTEvnStaUsed = '1'");
                oSql.AppendLine("AND FNLngID = " + cVB.nVB_Language);

                tEvnCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                if (string.IsNullOrEmpty(tEvnCode))
                {
                    return;
                }

                oEvent = new cmlTPSTShiftEvent();
                oEvent.FTBchCode = cVB.tVB_BchCode;
                oEvent.FTShfCode = cVB.tVB_ShfCode + "";
                oEvent.FTPosCode = cVB.tVB_PosCode;
                oEvent.FNSdtSeqNo = 1;
                oEvent.FDHisDateTime = DateTime.Now;
                oEvent.FTEvnCode = tEvnCode;
                oEvent.FNSvnQty = 1;
                oEvent.FCSvnAmt = 0;
                oEvent.FTRsnCode = (cVB.oVB_Reason == null) ? "" : cVB.oVB_Reason.FTRsnCode + "";
                oEvent.FTSvnApvCode = ptAvpCode;
                oEvent.FTSvnRemark = cVB.oVB_GBResource.GetString("tOverride") + ptScreenRole;

                new cShiftEvent().C_INSxShiftEvent(oEvent);
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                oSql = null;
                oEvent = null;
                tEvnCode = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// SET ข้อมูล SignType [ping][2019.06.05]
        /// </summary>
        /// <param name="ptType">รับข้อมูลประเภท SignType</param>
        //private void W_SETxPanelSignType(string ptType = "1")
        private void W_SETxPanelSignType(string ptType="")
        {
            try
            {
                //*Arm 62-10-31 แก้ไข string ptType = "1" : การเข้าเปิดรอบ Default ที่การเข้าโดย Username และรหัสผ่าน
                //if (ptType == null)
                if (string.IsNullOrEmpty(ptType)) //*Arm 63-02-24 
                {
                    if (aoW_AppModule == null)
                    {
                        return;
                    }

                    string tType = string.Empty;
                    if (aoW_AppModule == null || aoW_AppModule.Count == 0)
                    {
                        Label olaSignType = new Label();
                        olaSignType.AutoSize = true;
                        olaSignType.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
                        olaSignType.Dock = DockStyle.Left;
                        olaSignType.BringToFront();
                        olaSignType.ForeColor = Color.Gray;
                        olaSignType.Cursor = Cursors.No;
                        olaSignType.ForeColor = Color.Black;
                        
                        switch (cVB.tVB_PosType)
                        {
                            case "":
                            case "1": // Store
                                tType = "1";
                                W_SHOWxSignType(tType);
                                opnSignTypePin.Controls.Clear();
                                olaSignType.Text = "Pincode";
                                olaSignType.Name = "olaPincode";
                                olaSignType.Tag = "2";
                                opnSignTypePin.Controls.Add(olaSignType);
                                break;

                            case "2": // Cashier
                                tType = "2";
                                W_SHOWxSignType(tType);
                                opnSignTypeRFID.Controls.Clear();
                                olaSignType.Text = "RFID";
                                olaSignType.Name = "olaRFID";
                                olaSignType.Tag = "3";
                                opnSignTypeRFID.Controls.Add(olaSignType);
                                break;
                        }
                    }
                    else
                    {
                        tType = aoW_AppModule[0].FTAppSignType.FirstOrDefault().ToString();
                        W_SHOWxSignType(tType);
                    }
                }
                else
                {
                    W_SHOWxSignType(ptType);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_SELECTxPanelSignType : " + oEx.Message);
            }

        }

        /// <summary>
        /// แสดง SignType สำหรับการ Login ตามประเภท [ping][2019.06.05]
        /// </summary>
        /// <param name="ptType">รับข้อมูลประเภท SignType</param>
        public void W_SHOWxSignType(string ptType)
        {
            tW_Type = ptType; //*Arm 63-02-24 
            try
            {
                opnSignTypePwd.Controls.Clear();
                opnSignTypePin.Controls.Clear();
                opnSignTypeRFID.Controls.Clear();
                switch (ptType)
                {
                    case "1":
                        opnSignInPwd.Visible = true;
                        opnSignInRFID.Visible = false;
                        opnSignInPin.Visible = false;
                        W_GETxSelectSignType(opnSignTypePwd, ptType);
                        ogdUserPwd.Rows.Clear();
                        ogdUserPwd.Visible = true;
                        ogdUserPin.Visible = false;
                        W_GETxUser(ogdUserPwd);
                        break;
                    case "2":
                        opnSignInPin.Visible = true;
                        opnSignInRFID.Visible = false;
                        opnSignInPwd.Visible = false;
                        W_GETxSelectSignType(opnSignTypePin, ptType);
                        ogdUserPin.Rows.Clear();
                        ogdUserPwd.Visible = false;
                        ogdUserPin.Visible = true;
                        W_GETxUser(ogdUserPin);
                        break;
                    case "3":
                        opnSignInRFID.Visible = true;
                        opnSignInPin.Visible = false;
                        opnSignInPwd.Visible = false;
                        otbInputRFID.Clear();
                        otbInputRFID.Focus();
                        W_GETxSelectSignType(opnSignTypeRFID, ptType);
                        break;
                    case "4":

                        break;
                    case "5":

                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_SELECTxPanelSignType : " + oEx.Message);
            }
        }

        /// <summary>
        /// แสดง Text สำหรับให้ User เลือก Type [ping][2019.06.05]
        /// </summary>
        /// <param name="opPanel">Obj Panel</param>
        /// <param name="ptType">Type</param>
        private void W_GETxSelectSignType(Panel opPanel, string ptType)
        {
            int nRender = 0;
            try
            {
                if (aoW_AppModule == null)
                {
                    return;
                }

                Font oFont = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
                int nColumn = 0;
                foreach (var oItem in aoW_AppModule.OrderByDescending(x => x.FNAppSeqNo).ToList())
                {
                    nRender = CHKnRenderSignType(opPanel);
                    if (nRender > 0)
                    {
                        Label olaCenter = new Label();
                        olaCenter.AutoSize = true;
                        olaCenter.Font = oFont;
                        olaCenter.Text = "|";
                        olaCenter.Dock = DockStyle.Right;
                        //tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                        //tableLayoutPanel2.Controls.Add(olaCenter, nColumn, 0);

                        nColumn += 1;
                        opPanel.Controls.Add(olaCenter);
                    }

                    Label olaSignType = new Label();
                    olaSignType.AutoSize = true;
                    olaSignType.Font = oFont;
                    olaSignType.Dock = DockStyle.Right; //*Arm 63-02-24 
                    olaSignType.BringToFront();
                    olaSignType.ForeColor = Color.Gray;
                    olaSignType.Click += OlaSignType_Click;
                    
                    switch (oItem.FTAppSignType)
                    {
                        case "1":
                            if (ptType == "1")
                            {
                                olaSignType.Cursor = Cursors.No;
                                olaSignType.ForeColor = Color.Black;
                            }
                            //olaSignType.Text = "Password";
                            olaSignType.Text = cVB.oVB_GBResource.GetString("tPassword"); //*Arm 62-10-31 แสดงภาษาตามที่ตั้งค่า
                            olaSignType.Name = "olaPassword";
                            olaSignType.Tag = "1";
                            break;
                        case "2":
                            if (ptType == "2")
                            {
                                olaSignType.Cursor = Cursors.No;
                                olaSignType.ForeColor = Color.Black;
                            }
                            //olaSignType.Text = "Pincode";
                            olaSignType.Text = oW_Resource.GetString("tPincode");   //*Arm 62-10-31 แสดงภาษาตามที่ตั้งค่า
                            olaSignType.Name = "olaPincode";
                            olaSignType.Tag = "2";
                            break;
                        case "3":
                            if (ptType == "3")
                            {
                                olaSignType.Cursor = Cursors.No;
                                olaSignType.ForeColor = Color.Black;
                            }
                            olaSignType.Text = "RFID";
                            olaSignType.Tag = "3";
                            break;
                        case "4":
                            if (ptType == "4")
                            {
                                olaSignType.Cursor = Cursors.No;
                                olaSignType.ForeColor = Color.Black;
                            }
                            olaSignType.Name = "olaFinger";
                            olaSignType.Text = "Finger";
                            olaSignType.Tag = "4";
                            break;
                        case "5":
                            if (ptType == "5")
                            {
                                olaTitleUsernamePwd.Cursor = Cursors.No;
                                olaSignType.ForeColor = Color.Black;
                            }
                            olaSignType.Name = "olaFace";
                            olaSignType.Text = "Face";
                            olaSignType.Tag = "5";
                            break;
                    }
                    opPanel.Controls.Add(olaSignType);
                    //tableLayoutPanel2.ColumnStyles.Add(new ColumnHeaderStyle)
                    //tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F),);
                    //tableLayoutPanel2.Controls.Add(olaSignType, nColumn, 0);
                    nColumn += 1;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_LOADxSignType : " + oEx.Message);
            }
        }

        /// <summary>
        /// Click เลือก Type [ping][2019.06.05]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OlaSignType_Click(object sender, EventArgs e)
        {
            try
            {
                Label olaLabel = (Label)sender;
                W_SETxPanelSignType(olaLabel.Tag.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_LOADxSignType : " + oEx.Message);
            }
        }

        /// <summary>
        /// เช็คว่ามีการวาดประเภท SignType ลง Panel แล้วหรือยัง [ping][2019.06.05]
        /// </summary>
        /// <param name="opPanel">ชื่อ Obj Panel</param>
        /// <returns></returns>
        private int CHKnRenderSignType(Panel opPanel)
        {
            int nCount = 0;
            try
            {
                foreach (var oCtrl in opPanel.Controls)
                {
                    if (oCtrl.GetType() == typeof(Label))
                    {
                        nCount = 1;
                        return nCount;
                    }
                }
                return nCount;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "GETnRenderSignType : " + oEx.Message);
                return nCount;
            }
        }

        private void ocmNextPwd_Click(object sender, EventArgs e)
        {
            try
            {
                //W_CHKxUser("1", otbUsername.Tag.ToString(), otbPassword.Text);
                W_CHKxUser("1", otbUsername.Tag.ToString(), otbPassword.Text,otbUsername.Text); //*Em 62-09-10
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "ocmNextPwd_Click : " + oEx.Message);
            }
        }

        private void otbInputRFID_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //W_CHKxUser("3", string.Empty, otbInputRFID.Text);
                    W_CHKxUser("3", string.Empty, otbInputRFID.Text,string.Empty);  //*Em 62-09-10
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "otbInputRFID_KeyDown : " + oEx.Message);
            }
        }

        private void ogdUserPwd_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //W_PRCxChooseLastUserForPwdAndPin("GETUserPwd");
                W_PRCxChooseLastUserForPwdAndPin("GETUserLogin", ogdUserPwd.CurrentRow.Cells[1].Value.ToString()); //*Arm 63-08-07
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "ogdUserPwd_CellMouseClick : " + oEx.Message);
            }
        }

        private void otbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //W_CHKxUser("1", otbUsername.Tag.ToString(), otbPassword.Text);
                    W_CHKxUser("1", otbUsername.Tag.ToString(), otbPassword.Text,otbUsername.Text); //*Em 62-09-10
                }
            }
            catch (Exception oEx)
            {

                throw;
            }
        }


        /// <summary>
        /// Get language name
        /// *Arm 63-02-24 เพิ่มฟังก์ชั่นเปลี่ยนภาษา
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


        #endregion End Function / Method

        private void wSignin_KeyDown(object sender, KeyEventArgs e)
        {
            //*Arm 63-02-06 - (HotKey) Created function wSyncData_KeyDown
            try
            {
                W_CALxByName(e);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "wSyncData_KeyDown " + oEx.Message); }
        }
    }
}
