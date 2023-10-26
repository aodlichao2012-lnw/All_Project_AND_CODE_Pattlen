using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace AdaPos.Popup.All
{
    public partial class wAuthentication : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private List<cmlTCNMAppModule> aoW_AppModule;
        private string tSelectType;
        private string tW_ScreenRole;
        private string tW_UserCode; //*Arm 63-08-10
        #endregion End Variable

        #region Constructor
        public wAuthentication(int pnMode, string ptScreenRole)
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();

                tSelectType = "2";
                tW_ScreenRole = ptScreenRole;
                aoW_AppModule = oW_SP.SP_GEToAppSignType();
                W_SETxPanelSignType();

                uNumpadPwd.oU_TextValue = otbPassword;

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAuthentication", "wAuthentication : " + oEx.Message); }
            finally
            {

            }
        }

        #endregion End Constructor

        #region Event

        private void wAuthentication_FormClosing(object sender, FormClosingEventArgs e)
        {
            cVB.tVB_KbdScreen = tW_ScreenRole;
        }

        private void wAuthentication_Shown(object sender, EventArgs e)
        {
            otbPassword.Focus();
        }

        private void otbPassword_Leave(object sender, EventArgs e)
        {
            otbPassword.Focus();
        }

        private void otbPassword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tSelectType == "2")
                {
                    otbPassword.Text = new String((otbPassword.Text.Where(tChar => Char.IsDigit(tChar)).ToArray()));
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void otbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        W_CHKxUser(tSelectType, otbUsername.Tag.ToString(), otbPassword.Text, otbUsername.Text);
                        break;

                    case Keys.Escape:
                        if (ocmBack.Visible)
                            otmClose.Start();
                        break;

                    default:
                        W_CALxByName(e);
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void olaSignType_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Label oSignInType in opnSignTypePwd.Controls)
                {
                    oSignInType.Cursor = Cursors.Hand;
                    oSignInType.ForeColor = Color.Gray;
                }
                Label olaClickType = ((Label)sender);
                olaClickType.Cursor = Cursors.No;
                olaClickType.ForeColor = Color.Black;
                W_GETxSelectSignType(olaClickType.Tag.ToString());
                W_SETxText();

                //*Arm 63-08-10
                if(!string.IsNullOrEmpty(tW_UserCode))
                {
                    if (tSelectType == "1" || tSelectType == "2")
                    {
                        string tUsrLogin = "";
                        StringBuilder oSql = new StringBuilder();
                        
                        oSql.Clear();
                        oSql.AppendLine("SELECT TOP 1 FTUsrLogin as 'tName'");
                        oSql.AppendLine("FROM TCNMUsrLogin  WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE FTUsrCode = '" + tW_UserCode + "' AND FTUsrStaActive != '2' AND FTUsrLogType = '" + tSelectType + "' ");
                        oSql.AppendLine("AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),FDUsrPwdExpired,121)) ");
                        tUsrLogin = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                        if (!string.IsNullOrEmpty(tUsrLogin))
                        {
                            W_SETxPanelSignType();

                            otbUsername.Text = tUsrLogin;
                            otbUsername.Tag = tW_UserCode;
                            otbPassword.Focus();
                        }
                        oSql = null;
                    }
                }
                //++++++++++++++++
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }

        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            cmlSearch oSearch;
            wSearch2Column oSch2Col;
            try
            {
                //*Arm 63-08-07 Comment Code
                //switch (tSelectType)
                //{
                //    case "1":
                //        oSch2Col = new wSearch2Column("GETUserPwd");
                //        break;
                //    case "2":
                //        oSch2Col = new wSearch2Column("GETUserPin");
                //        break;
                //    default:
                //        oSch2Col = new wSearch2Column("GETUserPwd");
                //        break;
                //}

                oSch2Col = new wSearch2Column("GETUserLogin"); //*Arm 63-08-07

                oSch2Col.ShowDialog();

                if (oSch2Col.DialogResult == DialogResult.OK)
                {
                    oSearch = oSch2Col.oW_DataSearch;
                    tW_UserCode = oSearch.tCode;
                    string tUsrLogin = "";
                    StringBuilder oSql = new StringBuilder();

                    // Password
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 FTUsrLogin as 'tName'");
                    oSql.AppendLine("FROM TCNMUsrLogin  WITH(NOLOCK) ");
                    oSql.AppendLine("WHERE FTUsrCode = '" + oSearch.tCode + "' AND FTUsrStaActive != '2' AND FTUsrLogType = '1' AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),FDUsrPwdExpired,121)) ");
                    tUsrLogin = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                    if (!string.IsNullOrEmpty(tUsrLogin))
                    {
                        tSelectType = "1";
                        W_SETxPanelSignType();

                        otbUsername.Text = tUsrLogin;
                        otbUsername.Tag = oSearch.tCode;
                        otbPassword.Focus();
                    }
                    else
                    {
                        tUsrLogin = "";

                        oSql.Clear();
                        oSql.AppendLine("SELECT TOP 1 FTUsrLogin as 'tName'");
                        oSql.AppendLine("FROM TCNMUsrLogin  WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE FTUsrCode = '" + oSearch.tCode + "' AND FTUsrStaActive != '2' AND FTUsrLogType = '2' AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),FDUsrPwdExpired,121)) ");
                        tUsrLogin = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                        if (!string.IsNullOrEmpty(tUsrLogin))
                        {
                            tSelectType = "2";
                            W_SETxPanelSignType();
                            otbUsername.Text = tUsrLogin;
                            otbUsername.Tag = oSearch.tCode;
                            otbPassword.Focus();
                        }
                        else
                        {
                            otbUsername.Text = "";
                            otbUsername.Tag = "";
                            otbPassword.Focus();
                        }
                    }
                    //tSelectType = oSearch.tType;
                    //W_SETxPanelSignType();

                    //otbUsername.Text = oSearch.tName;
                    //otbUsername.Tag = oSearch.tCode;
                    //otbPassword.Focus();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oSearch = null;
                oSch2Col = null;
                otbPassword.Focus();
            }
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                cVB.tVB_KbdScreen = tW_ScreenRole;
                otmClose.Start();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void otmClose_Tick(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }

        }

        private void ocmNextPwd_Click(object sender, EventArgs e)
        {
            try
            {
                W_CHKxUser(tSelectType, otbUsername.Tag.ToString(), otbPassword.Text, otbUsername.Text);

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_PRCxCallByName("C_KBDxKeyboard");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }

        #endregion

        #region Function / Method


        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmNextPwd.BackColor = cVB.oVB_ColDark;

                ocmSearch.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
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
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }
                olaTitleForm.Text = oW_Resource.GetString("tTitleAuthen");
                olaUsername.Text = cVB.oVB_GBResource.GetString("tUsername");
                switch (tSelectType)
                {
                    case "1":
                        olaPassword.Text = oW_Resource.GetString("tPassword");
                        break;
                    case "2":
                        olaPassword.Text = oW_Resource.GetString("tPincode");
                        break;
                    default:
                        olaPassword.Text = oW_Resource.GetString("tPassword");
                        break;
                }
                cVB.tVB_KbdScreen = "ROLE";

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        /// <summary>
        /// SET ข้อมูล SignType [ping][2019.06.05]
        /// </summary>
        /// <param name="ptType">รับข้อมูลประเภท SignType</param>
        private void W_SETxPanelSignType()
        {
            try
            {
                if (aoW_AppModule == null || aoW_AppModule.Count == 0)
                {
                    otbUsername.Enabled = false;
                    otbPassword.Enabled = true; //*Arm 63-07-31 เปลี่ยนเป็น true (ยกมาจาก Moshi)
                    return;
                }

                opnSignTypePwd.Controls.Clear();
                opnSignTypePwd.Size = new Size(10, 41);
                opnSignTypePwd.AutoSize = true;
                foreach (cmlTCNMAppModule oSignInType in aoW_AppModule)
                {
                    Label olaSignType = new Label();
                    olaSignType.AutoSize = true;
                    olaSignType.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
                    olaSignType.Dock = DockStyle.Right;

                    //if (oSignInType.Equals(aoW_AppModule.First()))
                    if (oSignInType.FTAppSignType == tSelectType)
                    {
                        olaSignType.Cursor = Cursors.No;
                        olaSignType.ForeColor = Color.Black;
                    }
                    else
                    {
                        olaSignType.Cursor = Cursors.Hand;
                        olaSignType.ForeColor = Color.Gray;
                    }

                    switch (oSignInType.FTAppSignType)
                    //switch (tSelectType)
                    {
                        case "1":
                            olaSignType.Name = "olaPassword";
                            olaSignType.Text = oW_Resource.GetString("tPassword");
                            break;
                        case "2":
                            olaSignType.Name = "olaPincode";
                            olaSignType.Text = oW_Resource.GetString("tPincode");
                            break;
                        case "3":
                            olaSignType.Name = "RFID";
                            olaSignType.Text = "RFID";
                            break;
                        case "4":
                            olaSignType.Name = "olaFinger";
                            olaSignType.Text = "Finger";
                            break;
                        case "5":
                            olaSignType.Name = "olaFace";
                            olaSignType.Text = "Face";
                            break;
                    }
                    olaSignType.Tag = oSignInType.FTAppSignType;
                    olaSignType.Click += olaSignType_Click;

                    opnSignTypePwd.Controls.Add(olaSignType);


                    if (!oSignInType.Equals(aoW_AppModule.Last()))
                    {
                        Label olaSeparate = new Label();
                        olaSeparate.AutoSize = true;
                        olaSeparate.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
                        olaSeparate.Text = "|";
                        olaSeparate.Dock = DockStyle.Right;
                        opnSignTypePwd.Controls.Add(olaSeparate);
                    }
                }
                //W_GETxSelectSignType(opnSignTypePwd.Controls[0].Tag.ToString());
                W_GETxSelectSignType(tSelectType); //*Arm 63-08-07
                opnSignTypePwd.Location = new Point((opnPage.Width / 2) - (opnSignTypePwd.Width / 2), opnSignTypePwd.Location.Y);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }

        }

        private void W_GETxSelectSignType(string ptType)
        {
            otbUsername.Text = String.Empty;
            otbPassword.Text = String.Empty;
            switch (ptType)
            {
                case "1":
                    otbPassword.MaxLength = 0;
                    uNumpadPwd.Enabled = true; //*Arm 63-08-07
                    break;
                case "2":
                    otbPassword.MaxLength = 6;
                    uNumpadPwd.Enabled = true;
                    break;
            }
            tSelectType = ptType;
            W_SETxText();
        }

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

                new cLog().C_WRTxLog("wSignin", MethodBase.GetCurrentMethod().Name + $" : Start", cVB.bVB_AlwPrnLog); //*Arm 63-07-31 Stamp (ยกมาจาก Moshi)
                //ping 2019
                ////oSql.AppendLine("SELECT USR.FTUsrCode, USL.FTUsrLogin as FTUsrName, USL.FTUsrLoginPwd as FTUsrPwd, USR.FTRolCode, USG.FTUsrStaShop ,USG.FTShpCode ,FTMerCode, ROL.FNRolLevel");
                //oSql.AppendLine("SELECT DISTINCT USR.FTUsrCode, USL.FTUsrLogin as FTUsrName, USL.FTUsrLoginPwd as FTUsrPwd, AROL.FTRolCode, USG.FTUsrStaShop ,USG.FTShpCode ,FTMerCode"); //*Net 63-06-17 ไม่ใช้ RolLevel แล้ว ใช้ FTRolCode จาก TCNMUsrActRole แทน
                //oSql.AppendLine("FROM TCNMUser USR WITH(NOLOCK) ");
                //oSql.AppendLine("INNER JOIN TCNTUsrGroup USG WITH(NOLOCK) ON USG.FTUsrCode = USR.FTUsrCode");
                //oSql.AppendLine("LEFT JOIN TCNMShop SHP WITH(NOLOCK) ON SHP.FTShpCode = ISNULL(USG.FTShpCode,'')");
                //oSql.AppendLine("INNER JOIN TCNMUsrLogin USL WITH(NOLOCK) ON USL.FTUsrCode = USR.FTUsrCode ");
                ////oSql.AppendLine("INNER JOIN TCNMUsrRole ROL WITH(NOLOCK) ON ROL.FTRolCode = USR.FTRolCode");    //*Em 62-09-03
                //oSql.AppendLine("LEFT JOIN TCNMUsrActRole AROL WITH(NOLOCK) ON AROL.FTUsrCode = USR.FTUsrCode"); //*Net 63-06-17
                //oSql.AppendLine("WHERE USL.FTUsrLogType = '" + ptSigninType + "' ");
                //oSql.AppendLine("AND (ISNULL(USG.FTBchCode,'') = '' OR ISNULL(USG.FTBchCode,'') = '" + cVB.tVB_BchCode + "')");   //*Em 62-08-07
                //oSql.AppendLine("AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),USL.FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),USL.FDUsrPwdExpired,121)) "); //*Em 62-09-10
                //if (ptSigninType != "3") oSql.AppendLine("AND USR.FTUsrCode = '" + ptInput_Code + "'");  //*Em 62-09-10
                //if (ptSigninType != "3") oSql.AppendLine("AND USL.FTUsrLogin = '" + ptUsrLogin + "'");   //*Em 62-09-10
                //oSql.AppendLine("ORDER BY USR.FTUsrCode");

                //*Arm 63-08-07
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

                aoUser = new cDatabase().C_GETaDataQuery<cmlTCNMUser>(oSql.ToString());

                string tInput_Code = ptInput_Code;
                string tInput_Pwd = ptInput_Pwd;
                cmlTCNMUser oUser = new cmlTCNMUser();
                List<string> atUserRoleCode = new List<string>();
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
                oUser.FTRolCode = String.Join(",", atUserRoleCode.Select(oUsr => "'" + oUsr + "'").ToArray()); //*Net 63-06-17 join RoleCode ทั้งหมด เพื่อเอาไป Where IN
                W_CHKxLogin(oUser, tInput_Pwd);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        /// <summary>
        /// Check Login.
        /// </summary>
        private void W_CHKxLogin(cmlTCNMUser poUser, string ptInput_Pwd)
        {
            string tPwdEnc, tChkRole, tFuncCode;
            cmlTCNMUser oUser = new cmlTCNMUser();
            int nChkClear = 0;

            try
            {
                oUser = poUser;
                if (oUser != null)
                {
                    tPwdEnc = new cEncryptDecrypt("2").C_CALtEncrypt(ptInput_Pwd); //[Ping][2019.06.05][ปิดไว้เพื่อเทสระบบ]
                    //tPwdEnc = ptInput_Pwd;
                    if (string.Equals(tPwdEnc, oUser.FTUsrPwd))
                    {
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

                        otmClose.Start();
                        nChkClear = 1;
                    }
                    else
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsr"), 3);
                }
                else
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsrname"), 3);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                tPwdEnc = null;
                tChkRole = null;
                oUser = null;

                if (nChkClear == 0)
                {
                    otbPassword.Clear();
                    otbPassword.Focus();
                    uNumpadPwd.oU_TextValue.Clear();
                }
                //oW_SP.SP_CLExMemory();
            }
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
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
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
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
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
                    //case "C_KBDxMenu":
                    //    W_SETxOpenCloseMenu();
                    //    break;

                    //case "C_KBDxLastUser":
                    //    W_PRCxFocusLastUser();
                    //    break;

                    case "C_KBDxBack":
                        otmClose.Start();
                        break;

                    case "C_KBDxChooseUser":
                        ocmSearch_Click(ocmSearch, new EventArgs());
                        break;

                    case "C_KBDxInputPassword":
                        //ocbUserPin.DroppedDown = false;
                        otbPassword.Focus();
                        break;

                    case "C_KBDxNext":
                        W_CHKxUser(tSelectType, otbUsername.Tag.ToString(), otbPassword.Text, otbUsername.Text);
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                ptFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }
        #endregion End Function / Method

    }
}
