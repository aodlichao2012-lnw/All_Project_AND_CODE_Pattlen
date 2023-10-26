using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Popup.All;
using AdaPos.Popup.Setting;
using AdaPos.Resources_String.Local;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Configuration;
using System.Configuration;
using System.Collections.Specialized;
using AdaPos.Models.RabbitMQ;
using Newtonsoft.Json;
using C1.Win.C1FlexGrid;

namespace AdaPos.Forms
{
    public partial class wSetting : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private cEncryptDecrypt oEncDec;
        private int nW_Time;
        private int nW_Theme;   //*Em 61-11-19
        wSetColor owSetColor;  //*Arm 62-09-18
        private string tW_API2ARDoc;

        private string tW_OldBch;
        private string tW_OldPos;
        private cmlRabbitMQConfig oW_OldMQ;

        public object ConfigurationManager { get; private set; }

        #endregion End Variable

        public wSetting()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                oEncDec = new cEncryptDecrypt("1");
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();

                if (cVB.nVB_SettingFrom != 0)
                {
                    W_CONxDatabase();
                    W_SHWxButtonBar();
                    W_GETxService();
                }
                else
                {
                    W_SHWxButtonBar();  //*Em 62-09-17
                }
                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in opnMenu.Controls) //ใช้ในเวลา ทำให้ หน้าต่างยืดหด ไม่ค้าง
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "wSetting : " + oEx.Message); }
        }


        #region Function
        /// <summary>
        /// Set Design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.Width = 55;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                //ocmMenu.BackColor = cVB.oVB_ColDark; //*Arm 62-10-15 Comment Code
                //ocmKB.BackColor = cVB.oVB_ColDark;
                //ocmCalculate.BackColor = cVB.oVB_ColDark;
                //ocmShwKb.BackColor = cVB.oVB_ColDark;
                //ocmHelp.BackColor = cVB.oVB_ColDark;
                //ocmAbout.BackColor = cVB.oVB_ColDark;
                //ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColNormal;
                //ogdAPI.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                //oW_SP.SP_SETxSetGridviewFormat(ogdAPI); //*Net 63-03-03 Set Design Gridview
                oW_SP.SP_SETxSetGridFormat(ogdAPI);  //*Ought 63-09-18 เปลี่ยนมาใช้ C1

                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode, "TCNMUser");
                opbLogo.Image = new cCompany().C_GEToImageLogo();

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;

                //if (oW_SP.SP_CHKbConnection())
                //*Net 63-03-28 ยกมาจาก baseline
                if (!String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                {
                    if (oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
                        opbPOS.Image = Properties.Resources.Online_32;
                    else
                        opbPOS.Image = Properties.Resources.Offline_32;
                }
                //++++++++++++++++++++++++++++++++

                //if (cVB.nVB_SettingFrom == 0)
                //{
                opnSystem.Visible = true;
                //ocmGeneral.BackColor = cVB.oVB_ColDark;
                //ocmStyle.BackColor = cVB.oVB_ColDark;
                //ocmSystem.BackColor = cVB.oVB_ColNormal;
                //}
                //else
                //{
                //    //ocmGeneral.BackColor = cVB.oVB_ColNormal;
                //    //ocmStyle.BackColor = cVB.oVB_ColDark;
                //    //ocmSystem.BackColor = cVB.oVB_ColDark;
                //}

                // System
                ocmConnDB.BackColor = cVB.oVB_ColNormal;
                //Ping 2019.10.08
                ocmBrwPos.BackColor = cVB.oVB_ColNormal;
                ocmBchCode.BackColor = cVB.oVB_ColNormal; //*Net 63-04-15
                //ocmMenu.BackColor = cVB.oVB_ColNormal; //*Arm 62-10-15 Comment Code

                //*Em 61-11-19
                switch (cVB.nVB_CNTheme)
                {
                    case 0:
                        orbCustom.Checked = true;
                        break;
                    case 1:
                        orbGreen.Checked = true;
                        break;
                    case 2:
                        orbOrange.Checked = true;
                        break;
                    case 3:
                        orbSky.Checked = true;
                        break;
                    case 4:
                        orbBrown.Checked = true;
                        break;
                    case 5:
                        orbPink.Checked = true;
                        break;
                    default:
                        orbGreen.Checked = true;
                        break;
                }
                olaClrNormal.BackColor = cVB.oVB_ColNormal;
                olaClrDark.BackColor = cVB.oVB_ColDark;
                olaClrLight.BackColor = cVB.oVB_ColLight;
                nW_Theme = cVB.nVB_CNTheme;
                W_SETxMorkUpView();

                //+++++++++++++


                //*Net 63-03-28 ยกมาจาก baseline
                oucQtyGrp.W_SETxDesign();
                oucQtyMenu.W_SETxDesign();
                oucQtyPdt.W_SETxDesign();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set Text
        /// </summary>
        private void W_SETxText()
        {
            string tAuthen;

            try
            {

                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resSetting_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSetting_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "SETTING";

                // Header
                olaSetting.Text = oW_Resource.GetString("tSetting");
                olaVersion.Text = Application.ProductVersion;
                olaPos.Text = cVB.tVB_PosCode;

                if (!string.IsNullOrEmpty(cVB.tVB_UsrCode))
                    olaUsrName.Text = new cUser().C_GETtUsername();

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                // System DB
                ocmConnDB.Text = oW_Resource.GetString("tConnect");
                olaTitleDB.Text = oW_Resource.GetString("tDatabase");
                olaTitlSrvDB.Text = oW_Resource.GetString("tServerDB");
                olaTitleAuthen.Text = oW_Resource.GetString("tAuthen");
                olaTitleUsrDB.Text = oW_Resource.GetString("tUser");
                olaTitlePwdDB.Text = cVB.oVB_GBResource.GetString("tPassword");
                olaTitleDBName.Text = oW_Resource.GetString("tNameDB");

                ocbAuthen.Items.Clear(); //*Arm 63-03-24
                ocbAuthen.Items.Add("Windows Authentication");
                ocbAuthen.Items.Add("SQL Server Authentication");
                //*Arm 63-03-23 [Comment Code]
                //otbSrvDB.Text = oEncDec.C_CALtDecrypt(Properties.Settings.Default.ServerDB);
                //otbUsrDB.Text = oEncDec.C_CALtDecrypt(Properties.Settings.Default.UserDB);
                //otbPwdDB.Text = oEncDec.C_CALtDecrypt(Properties.Settings.Default.PwdDB);
                //ocbDBName.Text = oEncDec.C_CALtDecrypt(Properties.Settings.Default.NameDB);
                //tAuthen = oEncDec.C_CALtDecrypt(Properties.Settings.Default.AuthenDB);
                //++++++++++++++


                //*Arm 63-03-23
                //otbSrvDB.Text = System.Configuration.ConfigurationManager.AppSettings.Get("ServerDB").ToString();
                //otbUsrDB.Text = System.Configuration.ConfigurationManager.AppSettings.Get("UserDB").ToString();
                //otbPwdDB.Text = oEncDec.C_CALtDecrypt(System.Configuration.ConfigurationManager.AppSettings.Get("PwdDB").ToString());
                //ocbDBName.Text = System.Configuration.ConfigurationManager.AppSettings.Get("NameDB").ToString();
                //tAuthen = System.Configuration.ConfigurationManager.AppSettings.Get("AuthenDB").ToString();
                // ++++++++++++++

                //*Arm 63-03-24
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

                otbSrvDB.Text = config.AppSettings.Settings["ServerDB"].Value;
                otbUsrDB.Text = config.AppSettings.Settings["UserDB"].Value;
                otbPwdDB.Text = oEncDec.C_CALtDecrypt(config.AppSettings.Settings["PwdDB"].Value);
                ocbDBName.Text = config.AppSettings.Settings["NameDB"].Value;
                tAuthen = config.AppSettings.Settings["AuthenDB"].Value;
                // ++++++++++++++
                if (string.IsNullOrEmpty(tAuthen))
                    ocbAuthen.SelectedIndex = 0;
                else
                    ocbAuthen.SelectedIndex = Convert.ToInt32(tAuthen);

                //*Em 61-11-19
                //Style Panel
                olaNormal.Text = oW_Resource.GetString("tClrNormal");
                olaDark.Text = oW_Resource.GetString("tClrDark");
                olaLight.Text = oW_Resource.GetString("tClrLight");
                olaTheme.Text = oW_Resource.GetString("tTheme");
                orbCustom.Text = oW_Resource.GetString("tClrCustom");
                orbGreen.Text = oW_Resource.GetString("tClrGreen");
                orbOrange.Text = oW_Resource.GetString("tClrOrange");
                orbSky.Text = oW_Resource.GetString("tClrSky");
                orbBrown.Text = oW_Resource.GetString("tClrBrown");
                orbPink.Text = oW_Resource.GetString("tClrPink");
                //++++++++++++++++

                //*Em 62-08-20
                otbPosCode.Text = cVB.tVB_PosCode;
                otbPwdAdmin.Text = cVB.tVB_Passcode;
                otbBchCode.Text = cVB.tVB_BchCode;
                //*Arm 63-04-08
                if (!string.IsNullOrEmpty(otbBchCode.Text))
                {
                    tW_API2ARDoc = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + otbBchCode.Text + "' AND FNUrlType = 12");
                }
                else
                {
                    tW_API2ARDoc = "";
                }
                //+++++++++++++++

                olaTitlePosCode.Text = oW_Resource.GetString("tTitlePosCode");
                olaTitlePwdAdmin.Text = oW_Resource.GetString("tTitlePasscode");

                //*Ought 63-09-18 เปลี่ยนมาใช้ format C1
                //otbTitleService.HeaderText = oW_Resource.GetString("tTitleGrp");
                //otbTitleAPIName.HeaderText = oW_Resource.GetString("tTitleName");
                //otbTitleAPIValue.HeaderText = oW_Resource.GetString("tTitleValue");


                //+++++++++++++++++++++++++++

                //*Em 62-11-15
                olaTitleQtyMenu.Text = oW_Resource.GetString("tMenuPerPage");
                olaTitleQtyGrp.Text = oW_Resource.GetString("tGrpPerPage");
                olaTitleQtyPdt.Text = oW_Resource.GetString("tPdtPerPage");
                oucQtyPdt.otbQty.Text = cVB.nVB_PdtPerPage.ToString();
                oucQtyGrp.otbQty.Text = cVB.nVB_GrpPerPage.ToString();
                oucQtyMenu.otbQty.Text = cVB.nVB_MenuPerPage.ToString();
                //++++++++++++++

                //*Net 63-07-31
                olaTitleRunBillSale.Text = oW_Resource.GetString("tLastBillS");
                olaTitleRunBillRet.Text = oW_Resource.GetString("tLastBillR");
                otbRunBillSale.Text = "0";
                otbRunBillRet.Text = "0";
                otbRunBillSale.Enabled = false;
                otbRunBillRet.Enabled = false;

                if (!String.IsNullOrEmpty(cVB.tVB_BchCode) && !String.IsNullOrEmpty(cVB.tVB_PosCode))
                {
                    otbRunBillSale.Text = cVB.nVB_LastBillS.ToString();
                    otbRunBillRet.Text = cVB.nVB_LastBillR.ToString();
                    otbRunBillSale.Enabled = true;
                    otbRunBillRet.Enabled = true;
                }

                //*Net 63-02-05
                //ockShow2ndScreen.Text = oW_Resource.GetString("tShow2ndScreen");
                ////ockShow2ndScreen.Checked = (cVB.nVB_Check2nd == 1);
                //ockShow2ndScreen.Checked = cVB.bVB_AlwShw2Screen; //*Net 63-03-28 ยกมาจาก baseline

                //// Zen 63-02-26
                //olaModeStd.Text = "รูปแบบหน้าจอการขาย";
                //ocbModeStd.DisplayMember = "Text";
                //ocbModeStd.ValueMember = "Value";
                //var aItems = new[] {
                //    new { Text = "แบบมาตรฐาน", Value = 1 },
                //    new { Text = "แบบ Touch Screen", Value = 2 },                   
                //};

                //ocbModeStd.DataSource = aItems;

                ////*Net 63-03-28 ยกมาจาก baseline
                //switch (cVB.nVB_SaleModeStd)
                //{
                //    case 1:
                //        ocbModeStd.SelectedIndex = 0;
                //        break;
                //    case 2:
                //        ocbModeStd.SelectedIndex = 1;
                //        break;
                //}
                ////+++++++++++++++++++++++++++++++
                //ocbModeStd.SelectedValue = Properties.Settings.Default.ModeSale.Trim().ToString();

                //*Zen 63-03-31
                olaBchCode.Text = oW_Resource.GetString("tTitleBch");
                W_SETxColGrid(ogdAPI); //*Ought 63-09-21 
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT;  //*Em 62-01-23  Waterpark
            List<cmlTPSMFunc> aoMenuB;  //*Em 62-01-23  Waterpark
            int nItem;  //*Em 62-01-23  Waterpark

            try
            {

                aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();
                //ocmMenu.BackColor = cVB.oVB_ColDark;
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

                //        case "KB046":   //Kb
                //            ocmKB.Visible = true;
                //            ocmKB.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB047":   // About
                //            ocmAbout.Visible = true;
                //            ocmAbout.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB011":   // General
                //            ocmGeneral.Visible = true;
                //            ocmGeneral.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB012":   // Style
                //            ocmStyle.Visible = true;
                //            ocmStyle.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB014":   // System
                //            ocmSystem.Visible = true;
                //            ocmSystem.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB003":   // Back
                //            ocmBack.Visible = true;
                //            ocmBack.Text = "".PadLeft(10) + oKb.FTGdtName;
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
                        Button ocmMenuBar = new Button(); //*Arm 62-10-15 - เปลี่ยนชื่อ Button จาก ocmMenu เป็น ocmMenuBar
                        ocmMenuBar.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                        ocmMenuBar.FlatStyle = FlatStyle.Flat;
                        ocmMenuBar.FlatAppearance.BorderSize = 0;
                        ocmMenuBar.Font = new Font("Segoe UI Semibold", 10F);
                        ocmMenuBar.ForeColor = Color.White;
                        ocmMenuBar.Text = "".PadLeft(5) + oMenu.FTGdtName;
                        ocmMenuBar.TextAlign = ContentAlignment.MiddleLeft;
                        ocmMenuBar.Name = oMenu.FTGdtCallByName;
                        ocmMenuBar.BackColor = cVB.oVB_ColDark;
                        ocmMenuBar.Enabled = true;

                        try
                        {
                            ocmMenuBar.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        }
                        catch { }
                        ocmMenuBar.TextImageRelation = TextImageRelation.ImageBeforeText;

                        ocmMenuBar.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenuBar.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenuBar.Click += ocmMenuBar_Click;
                        ocmMenuBar.Tag = oMenu.FTGdtCallByName;
                        ocmMenuBar.Height = 50;
                        ocmMenuBar.Width = 249;
                        opnMenuT.Controls.Add(ocmMenuBar, 1, nItem);
                        opnMenuT.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuT.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;

                        //*Arm 63-07-10 ตรวจสอบ Option ปุ่ม ลงทะเบียนเครื่องจุดขาย
                        if (oMenu.FTGdtCallByName == "C_KBDxPosRegister")
                        {
                            if (cVB.bVB_ChkPosRegister == false)
                            {
                                ocmMenuBar.Visible = false;
                            }
                            else
                            {
                                ocmMenuBar.Visible = true;
                            }
                        }
                        //+++++++++++++++
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
                        Button ocmMenuBar = new Button(); //*Arm 62-10-15 - เปลี่ยนชื่อ Button จาก ocmMenu เป็น ocmMenuBar
                        ocmMenuBar.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                        //ocmMenu.Margin = new Padding(2);
                        ocmMenuBar.FlatStyle = FlatStyle.Flat;
                        ocmMenuBar.FlatAppearance.BorderSize = 0;
                        ocmMenuBar.Font = new Font("Segoe UI Semibold", 10F);
                        ocmMenuBar.ForeColor = Color.White;
                        ocmMenuBar.Text = "".PadLeft(5) + oMenu.FTGdtName;
                        ocmMenuBar.TextAlign = ContentAlignment.MiddleLeft;
                        ocmMenuBar.Name = oMenu.FTGdtCallByName;
                        ocmMenuBar.TextImageRelation = TextImageRelation.ImageBeforeText;
                        ocmMenuBar.BackColor = cVB.oVB_ColDark;
                        ocmMenuBar.Enabled = true;
                        try
                        {
                            ocmMenuBar.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        }
                        catch { }

                        ocmMenuBar.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenuBar.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenuBar.Click += ocmMenuBar_Click;
                        ocmMenuBar.Tag = oMenu.FTGdtCallByName;
                        ocmMenuBar.Height = 50;
                        ocmMenuBar.Width = 249;
                        opnMenuB.Controls.Add(ocmMenuBar, 1, nItem);
                        opnMenuB.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuB.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }
                else
                {
                    //*Em 62-09-17
                    opnMenuB.Controls.Clear();
                    opnMenuB.RowCount = 2;
                    nItem = 1;

                    Panel oPanel;
                    oPanel = new Panel();
                    oPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                    oPanel.Height = opnMenuB.Height - ((opnMenuB.RowCount - 1) * 55);
                    opnMenuB.Controls.Add(oPanel);

                    Button ocmMenuBar = new Button(); //*Arm 62-10-15 - เปลี่ยนชื่อ Button จาก ocmMenu เป็น ocmMenuBar
                    ocmMenuBar.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                    ocmMenuBar.FlatStyle = FlatStyle.Flat;
                    ocmMenuBar.FlatAppearance.BorderSize = 0;
                    ocmMenuBar.Font = new Font("Segoe UI Semibold", 10F);
                    ocmMenuBar.ForeColor = Color.White;
                    ocmMenuBar.Text = "".PadLeft(5) + cVB.oVB_GBResource.GetString("tExit");
                    ocmMenuBar.TextAlign = ContentAlignment.MiddleLeft;
                    ocmMenuBar.Name = "C_KBDxExit";
                    ocmMenuBar.TextImageRelation = TextImageRelation.ImageBeforeText;
                    ocmMenuBar.BackColor = cVB.oVB_ColDark;
                    ocmMenuBar.Enabled = true;
                    try
                    {
                        ocmMenuBar.Image = ((Image)(Properties.Resources.ResourceManager.GetObject("C_KBDxExit")));
                    }
                    catch { }

                    ocmMenuBar.ImageAlign = ContentAlignment.MiddleLeft;
                    ocmMenuBar.BackgroundImageLayout = ImageLayout.Zoom;
                    ocmMenuBar.Click += ocmMenuBar_Click;
                    ocmMenuBar.Tag = "C_KBDxExit";
                    ocmMenuBar.Height = 50;
                    ocmMenuBar.Width = 249;
                    opnMenuB.Controls.Add(ocmMenuBar, 1, 1);
                    opnMenuB.RowStyles[1].SizeType = SizeType.Absolute;
                    opnMenuB.RowStyles[1].Height = 55;
                    //+++++++++++++
                }
                //++++++++++++++++++++++++++

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_SHWxButtonBar : " + oEx.Message); }
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
                    opnMenu.Width = 55;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_SETxOpenCloseMenu : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "ocmMenu_Click : " + oEx.Message); }
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
                        W_PRCxBack();
                        break;

                    case "C_KBDxSystem":
                        W_GETxSystem();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_GETxFuncByFuncName " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Connect Database
        /// </summary>
        private void W_CONxDatabase(bool pbGetDB = false) //*Net 63-08-18
        {
            cmlConfigDB oCfg;
            List<string> atNameDB;
            StringBuilder oSql;
            SqlConnection oConn;

            try
            {
                oCfg = new cmlConfigDB();
                oCfg.tServerDB = otbSrvDB.Text;
                oCfg.tAuthenDB = ocbAuthen.SelectedIndex.ToString();
                oCfg.tUser = otbUsrDB.Text;
                oCfg.tPassword = otbPwdDB.Text;
                //*Net 63-08-18 ถ้ากดปุ่มเชื่อมต่อ ไม่ต้องใช้ชื่อ DB
                if (pbGetDB) oCfg.tNameDB = "master";
                else oCfg.tNameDB = ocbDBName.Text;

                oConn = new cDatabase().C_CONoDatabase(oCfg);

                if (oConn != null)
                {
                    using (SqlCommand oCmd = new SqlCommand("SELECT NAME FROM SYS.DATABASES", oConn))
                    {
                        if (oConn.State == ConnectionState.Closed) oConn.Open();
                        oCmd.CommandType = CommandType.Text;
                        SqlDataReader oDR = oCmd.ExecuteReader();
                        DataTable oDT = new DataTable();
                        oDT.Load(oDR);
                        ocbDBName.DataSource = oDT;
                        ocbDBName.ValueMember = oDT.Columns[0].ToString();
                        ocbDBName.DisplayMember = oDT.Columns[0].ToString();

                    }
                    if (string.IsNullOrEmpty(ocbDBName.Text))
                        ocbDBName.SelectedIndex = 0;
                    if (!string.IsNullOrEmpty(oCfg.tNameDB)) ocbDBName.SelectedValue = oCfg.tNameDB;
                }
                else
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tConnFail"), 3);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_CONxDatabase : " + oEx.Message); }
            finally
            {
                oCfg = null;
                atNameDB = null;
                oSql = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Check value System
        /// </summary>
        private void W_CHKxValueSystem()
        {
            int nChkDBName;

            try
            {
                // Database
                if (string.IsNullOrEmpty(otbSrvDB.Text))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgInputSrvDB"), 3);
                    otbSrvDB.Focus();
                }
                else if (string.IsNullOrEmpty(otbUsrDB.Text) && ocbAuthen.SelectedIndex != 0)
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgInputUsrDB"), 3);
                    otbUsrDB.Focus();
                }
                else if (string.IsNullOrEmpty(otbPwdDB.Text) && ocbAuthen.SelectedIndex != 0)
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgInputPwdDB"), 3);
                    otbPwdDB.Focus();
                }
                else if (ocbDBName.Items.Count == 0)
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgConnDB"), 3);
                    ocmConnDB.Focus();
                }
                else if (string.IsNullOrEmpty(ocbDBName.Text))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgSelectNameDB"), 3);
                    ocbDBName.Focus();
                }
                else
                {
                    nChkDBName = ocbDBName.FindStringExact(ocbDBName.Text);

                    if (nChkDBName <= 0)
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgSelectNameDB"), 3);
                        ocbDBName.Focus();
                    }
                    else
                        W_SAVxSystem();
                }
               
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_CHKxValueSystem : " + oEx.Message); }
        }

        /// <summary>
        /// Process System
        /// </summary>
        private void W_GETxSystem()
        {
            try
            {
                opnSystem.Visible = true;
                //ocmGeneral.BackColor = cVB.oVB_ColDark;
                //ocmStyle.BackColor = cVB.oVB_ColDark;
                //ocmSystem.BackColor = cVB.oVB_ColNormal;
                opnStyle.Visible = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_GETxSystem : " + oEx.Message); }
        }

        /// <summary>
        /// Process Back
        /// </summary>
        private void W_PRCxBack()
        {
            try
            {
                switch (cVB.nVB_SettingFrom)
                {
                    case 0:     // Start Program
                        if (cVB.bVB_SplashScreen)     // Show Splash screen form
                            new wSplashScreen().Show();
                        else    // Show home form
                            new wSignin(0, "").Show();
                        break;

                    case 1:     // Splash Screen
                        new wSplashScreen().Show();
                        break;

                    case 2:     // Home
                        new wHome().Show();
                        break;
                }

                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_PRCxBack : " + oEx.Message); }
        }

        /// <summary>
        /// Save System
        /// </summary>
        private void W_SAVxSystem()
        {
            StringBuilder oSql;

            try
            {
                // Database
                //Properties.Settings.Default.ServerDB = oEncDec.C_CALtEncrypt(otbSrvDB.Text);
                //Properties.Settings.Default.AuthenDB = oEncDec.C_CALtEncrypt(ocbAuthen.SelectedIndex.ToString());
                //Properties.Settings.Default.UserDB = oEncDec.C_CALtEncrypt(otbUsrDB.Text);
                //Properties.Settings.Default.PwdDB = oEncDec.C_CALtEncrypt(otbPwdDB.Text);
                //Properties.Settings.Default.NameDB = oEncDec.C_CALtEncrypt(ocbDBName.Text);


                //Properties.Settings.Default.Save();

                ////*Em 62-08-30
                //if (cVB.oVB_Config == null) cVB.oVB_Config = new cmlConfigDB();
                //cVB.oVB_Config.tServerDB = otbSrvDB.Text;
                //cVB.oVB_Config.tAuthenDB = ocbAuthen.SelectedIndex.ToString();
                //cVB.oVB_Config.tUser = otbUsrDB.Text;
                //cVB.oVB_Config.tPassword = otbPwdDB.Text;
                //cVB.oVB_Config.tNameDB = ocbDBName.Text;
                ////+++++++++++++

                //*Arm 63-03-23

                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

                config.AppSettings.Settings["ServerDB"].Value = otbSrvDB.Text;
                config.AppSettings.Settings["AuthenDB"].Value = ocbAuthen.SelectedIndex.ToString();
                config.AppSettings.Settings["UserDB"].Value = otbUsrDB.Text;
                config.AppSettings.Settings["PwdDB"].Value = oEncDec.C_CALtEncrypt(otbPwdDB.Text);
                config.AppSettings.Settings["NameDB"].Value = ocbDBName.Text;
                config.AppSettings.Settings["Passcode"].Value = oEncDec.C_CALtEncrypt(otbPwdAdmin.Text.Trim());
                //config.AppSettings.Settings["MaxLenPasscode"].Value = oEncDec.C_CALtEncrypt():
                config.Save(ConfigurationSaveMode.Modified);


                if (cVB.oVB_Config == null) cVB.oVB_Config = new cmlConfigDB();
                cVB.oVB_Config.tServerDB = otbSrvDB.Text;
                cVB.oVB_Config.tAuthenDB = ocbAuthen.SelectedIndex.ToString();
                cVB.oVB_Config.tUser = otbUsrDB.Text;
                cVB.oVB_Config.tPassword = otbPwdDB.Text;
                cVB.oVB_Config.tNameDB = ocbDBName.Text;
                //++++++++++++++

                oSql = new StringBuilder();

                //*Em 61-11-19
                string tNormal = olaClrNormal.BackColor.R.ToString("X2") + olaClrNormal.BackColor.G.ToString("X2") + olaClrNormal.BackColor.B.ToString("X2");
                string tDark = olaClrDark.BackColor.R.ToString("X2") + olaClrDark.BackColor.G.ToString("X2") + olaClrDark.BackColor.B.ToString("X2");
                string tLight = olaClrLight.BackColor.R.ToString("X2") + olaClrLight.BackColor.G.ToString("X2") + olaClrLight.BackColor.B.ToString("X2");

                oSql.Clear();
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + tNormal + "' WHERE FTSysCode = 'tPS_AppColor' AND FTSysSeq = '1'");
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + tDark + "' WHERE FTSysCode = 'tPS_AppColor' AND FTSysSeq = '2'");
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + tLight + "' WHERE FTSysCode = 'tPS_AppColor' AND FTSysSeq = '3'");
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + nW_Theme + "' WHERE FTSysCode = 'tPS_AppTheme' AND FTSysSeq = '1'");
                //*Em 62-08-20
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + otbPosCode.Text.Trim() + "' WHERE FTSysCode = 'tPS_PosNo' AND FTSysSeq = '1'");
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + otbPwdAdmin.Text.Trim() + "' WHERE FTSysCode = 'tPS_Password' AND FTSysSeq = '1'");
                //*Zen 63-03-31
                oSql.AppendLine($"IF NOT EXISTS  (SELECT * FROM [TSysSetting] WHERE [FTSysCode]='tPS_Branch') ");
                oSql.AppendLine($"BEGIN");
                oSql.AppendLine($"INSERT INTO [TSysSetting]");
                oSql.AppendLine($"([FTSysCode],[FTSysApp],[FTSysKey],[FTSysSeq]");
                oSql.AppendLine($",[FTGmnCode],[FTSysStaAlwEdit],[FTSysStaDataType]");
                oSql.AppendLine($",[FNSysMaxLength],[FTSysStaDefValue],[FTSysStaDefRef]");
                oSql.AppendLine($",[FTSysStaUsrValue],[FTSysStaUsrRef],[FDLastUpdOn]");
                oSql.AppendLine($",[FTLastUpdBy],[FDCreateOn],[FTCreateBy])");
                oSql.AppendLine($"VALUES('tPS_Branch','AdaPos','wSetting','1','MPOS','1','0',5,'00001',NULL,'{otbBchCode.Text.Trim()}',NULL,GETDATE(),'Ponwarut Z.',GETDATE(),'Ponwarut Z.')");

                oSql.AppendLine($"INSERT INTO [TSysSetting_L]");
                oSql.AppendLine($"([FTSysCode],[FTSysApp],[FTSysKey],[FTSysSeq]");
                oSql.AppendLine($",[FNLngID],[FTSysName],[FTSysDesc],[FTSysRmk])");
                oSql.AppendLine($"VALUES('tPS_Branch','AdaPos','wSetting','1',1,'รหัสสาขา',NULL,NULL)");

                oSql.AppendLine($"INSERT INTO [TSysSetting_L]");
                oSql.AppendLine($"([FTSysCode],[FTSysApp],[FTSysKey],[FTSysSeq]");
                oSql.AppendLine($",[FNLngID],[FTSysName],[FTSysDesc],[FTSysRmk])");
                oSql.AppendLine($"VALUES('tPS_Branch','AdaPos','wSetting','1',2,'Branch Code',NULL,NULL)");
                oSql.AppendLine($"END");

                oSql.AppendLine($"ELSE");
                oSql.AppendLine($"BEGIN");
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + otbBchCode.Text.Trim() + "' WHERE FTSysCode = 'tPS_Branch' AND FTSysSeq = '1'");
                oSql.AppendLine($"END");

                //*Net 63-04-09 update Bch ที่เลือก ในตาราง Comp ด้วย
                oSql.AppendLine("UPDATE TCNMComp with(rowlock) SET FTBchcode = '" + otbBchCode.Text.Trim() + "'");

                //+++++++++++++
                //*Em 62-11-15
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + oucQtyPdt.otbQty.Text.Trim() + "' WHERE FTSysCode = 'nPS_PdtPerPage' AND FTSysSeq = '1'");
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + oucQtyGrp.otbQty.Text.Trim() + "' WHERE FTSysCode = 'nPS_PdtGrpPerPage' AND FTSysSeq = '1'");
                oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + oucQtyMenu.otbQty.Text.Trim() + "' WHERE FTSysCode = 'nPS_MenuBillPerPage' AND FTSysSeq = '1'");
                //+++++++++++++


                //*Net 63-07-31 update running  bill
                int nTryParse;
                if (String.IsNullOrEmpty(otbRunBillSale.Text) || int.TryParse(otbRunBillSale.Text, out nTryParse) == false)
                {
                    otbRunBillSale.Text = "0";
                }
                if (String.IsNullOrEmpty(otbRunBillRet.Text) || int.TryParse(otbRunBillRet.Text, out nTryParse) == false)
                {
                    otbRunBillRet.Text = "0";
                }
                if (!String.IsNullOrEmpty(otbBchCode.Text) && !String.IsNullOrEmpty(otbPosCode.Text))
                {
                    oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + Convert.ToInt32(otbRunBillSale.Text) + "' WHERE FTSysCode = 'tPS_LastBillS' AND FTSysSeq = '1'");
                    oSql.AppendLine("UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '" + Convert.ToInt32(otbRunBillRet.Text) + "' WHERE FTSysCode = 'tPS_LastBillR' AND FTSysSeq = '1'");
                }

                //*Net 63-03-28 ยกมาจาก baseline
                //oSql.AppendLine($"IF NOT EXISTS  (SELECT * FROM [TSysSetting] WHERE [FTSysCode]='nPS_SaleMode') ");
                //oSql.AppendLine($"BEGIN");

                //oSql.AppendLine($"INSERT INTO [TSysSetting]");
                //oSql.AppendLine($"([FTSysCode],[FTSysApp],[FTSysKey],[FTSysSeq]");
                //oSql.AppendLine($",[FTGmnCode],[FTSysStaAlwEdit],[FTSysStaDataType]");
                //oSql.AppendLine($",[FNSysMaxLength],[FTSysStaDefValue],[FTSysStaDefRef]");
                //oSql.AppendLine($",[FTSysStaUsrValue],[FTSysStaUsrRef],[FDLastUpdOn]");
                //oSql.AppendLine($",[FTLastUpdBy],[FDCreateOn],[FTCreateBy])");
                //oSql.AppendLine($"VALUES('nPS_SaleMode','AdaPos','wSetting','1','MPOS','1','1',1,'1',NULL,'{(ocbModeStd.SelectedIndex + 1).ToString()}',NULL,'2020-03-04 18:26:00.000','Jirayu S.','2020-03-04 18:26:00.000','Jirayu S.')");

                //oSql.AppendLine($"INSERT INTO [TSysSetting_L]");
                //oSql.AppendLine($"([FTSysCode],[FTSysApp],[FTSysKey],[FTSysSeq]");
                //oSql.AppendLine($",[FNLngID],[FTSysName],[FTSysDesc],[FTSysRmk])");
                //oSql.AppendLine($"VALUES('nPS_SaleMode','AdaPos','wSetting','1',1,'โหมดการแสดงหน้าจอการขาย',NULL,'1:คลาสสิค 2:ทัชสกรีน')");

                //oSql.AppendLine($"INSERT INTO [TSysSetting_L]");
                //oSql.AppendLine($"([FTSysCode],[FTSysApp],[FTSysKey],[FTSysSeq]");
                //oSql.AppendLine($",[FNLngID],[FTSysName],[FTSysDesc],[FTSysRmk])");
                //oSql.AppendLine($"VALUES('nPS_SaleMode','AdaPos','wSetting','1',2,'Sale Mode',NULL,'1:Classic 2:Touch Screen')");
                //oSql.AppendLine($"END");

                //oSql.AppendLine($"ELSE");
                //oSql.AppendLine($"BEGIN");
                //oSql.AppendLine($"UPDATE TSysSetting with(rowlock) SET FTSysStaUsrValue = '{(ocbModeStd.SelectedIndex + 1).ToString()}' WHERE FTSysCode = 'nPS_SaleMode' AND FTSysSeq = '1'");
                //oSql.AppendLine($"END");

                //+++++++++++++


                new cDatabase().C_SETxDataQuery(oSql.ToString());
                cVB.oVB_ColNormal = olaClrNormal.BackColor;
                cVB.oVB_ColDark = olaClrDark.BackColor;
                cVB.oVB_ColLight = olaClrLight.BackColor;
                cVB.nVB_CNTheme = nW_Theme;
                //+++++++++++++

                //*Em 62-11-15
                cVB.nVB_PdtPerPage = Convert.ToInt32(oucQtyPdt.otbQty.Text);
                cVB.nVB_GrpPerPage = Convert.ToInt32(oucQtyGrp.otbQty.Text);
                cVB.nVB_MenuPerPage = Convert.ToInt32(oucQtyMenu.otbQty.Text);
                //++++++++++++++

                //*Net 63-05-12
                tW_OldBch = cVB.tVB_BchCode;
                tW_OldPos = cVB.tVB_PosCode;
                oW_OldMQ = cVB.oVB_RabbitMQConfig;
                W_PRCxMonitorServ();
                //++++++++++++++

                oW_SP.SP_PRCxStartApp();

                W_SETxDesign();
                W_SETxText();
                if (opnSystem.Visible) W_GETxSystem();
                //if (opnStyle.Visible) ocmStyle_Click(ocmStyle, new EventArgs());
                W_SHWxButtonBar();
                W_GETxService();

                oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgSaveSuccess"), 1);
                //W_PRCxBack();

                //*Net 63-05-12
                if (String.IsNullOrEmpty(tW_OldBch) || String.IsNullOrEmpty(tW_OldPos) || oW_OldMQ == null)
                {
                    W_PRCxMonitorServ();
                }
                //++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_SAVxSystem : " + oEx.Message); }
            finally
            {
                oSql = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Send req to restart service
        /// </summary>
        public void W_PRCxMonitorServ()
        {
            string tExPub = "AR_XPepairingSale";
            string tMsg;
            string tConnString;
            cmlMonitorReq oMonitor;
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_HQMQHost)) return;
                if (string.IsNullOrEmpty(cVB.tVB_HQMQUsr)) return;
                if (string.IsNullOrEmpty(cVB.tVB_HQMQPwd)) return;
                if (string.IsNullOrEmpty(cVB.tVB_HQMQVirtual)) return;
                if (string.IsNullOrEmpty(otbBchCode.Text)) return;
                if (string.IsNullOrEmpty(otbPosCode.Text)) return;

                cVB.oVB_RabbitMQConfig.tHostName = cVB.tVB_HQMQHost;
                cVB.oVB_RabbitMQConfig.tUserName = cVB.tVB_HQMQUsr;
                cVB.oVB_RabbitMQConfig.tPassword = cVB.tVB_HQMQPwd;
                cVB.oVB_RabbitMQConfig.tVirtual = cVB.tVB_HQMQVirtual;

                cVB.oVB_MQFactory = new RabbitMQ.Client.ConnectionFactory();
                cVB.oVB_MQFactory.HostName = cVB.oVB_RabbitMQConfig.tHostName;
                cVB.oVB_MQFactory.UserName = cVB.oVB_RabbitMQConfig.tUserName;
                cVB.oVB_MQFactory.Password = cVB.oVB_RabbitMQConfig.tPassword;
                cVB.oVB_MQFactory.VirtualHost = cVB.oVB_RabbitMQConfig.tVirtual;

                cVB.oVB_MQConn = cVB.oVB_MQFactory.CreateConnection();
                cVB.oVB_MQModel = cVB.oVB_MQConn.CreateModel();

                // Windows Authen
                if (string.Equals(cVB.oVB_Config.tAuthenDB, "0"))
                {
                    tConnString = @"Server = " + cVB.oVB_Config.tServerDB + ";";

                    if (!string.IsNullOrEmpty(cVB.oVB_Config.tNameDB))
                        tConnString += "Database = " + cVB.oVB_Config.tNameDB + ";";

                    tConnString += "Integrated Security = True;";
                }
                // SQL Authen 
                else
                {
                    tConnString = @"Data Source = " + cVB.oVB_Config.tServerDB + ";";

                    if (!string.IsNullOrEmpty(cVB.oVB_Config.tNameDB))
                        tConnString += "Initial Catalog = " + cVB.oVB_Config.tNameDB + ";";

                    tConnString += "Persist Security Info = True;";
                    tConnString += "User ID = " + cVB.oVB_Config.tUser + ";";
                    tConnString += "Password = " + cVB.oVB_Config.tPassword + ";";
                }

                tConnString += "MultipleActiveResultSets=true;";

                oMonitor = new cmlMonitorReq();
                oMonitor.tBchCode = cVB.tVB_BchCode;
                oMonitor.tPosCode = cVB.tVB_PosCode;
                oMonitor.tType = "0";
                oMonitor.tConnStr = tConnString;
                tMsg = JsonConvert.SerializeObject(oMonitor);

                cVB.oVB_MQModel.BasicPublish(tExPub, "",
                                            false, null, Encoding.UTF8.GetBytes(tMsg));
                cVB.oVB_MQModel.Close();
                cVB.oVB_MQConn.Close();
                cVB.oVB_MQFactory = null;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSyncData", "C_PRCxSyncUld : " + oEx.Message);
            }
            finally
            {
                oMonitor = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get Service
        /// </summary>

        //ฟังก์ชั่น Add ข้อมูลเข้า grid
        private void W_GETxService()
        {
            List<cmlTSysConfig> aoCfg;
            List<cmlGetURL> aoURL;

            try
            {
                //*Ought 63-09-18 Clear 

                aoCfg = new cConfig().C_GETaCfgService();
                aoURL = new cConfig().C_GETaCfgUrl(cVB.tVB_BchCode); //*Arm 63-06-26

                //ลูป แอดเข้า grid
                foreach (cmlTSysConfig oCfg in aoCfg)
                {
                    //ogdAPI.Rows.Add(oCfg.FTSysKey, oCfg.FTSysName, oCfg.FTSysStaUsrValue, oCfg.FTSysCode);
                 
                    ogdAPI.Rows.Add();
                    ogdAPI.SetData(ogdAPI.Rows.Count - ogdAPI.Rows.Fixed, ogdAPI.Cols["otbTitleService"].Index, oCfg.FTSysKey.ToString());
                    ogdAPI.SetData(ogdAPI.Rows.Count - ogdAPI.Rows.Fixed, ogdAPI.Cols["otbTitleAPIName"].Index, oCfg.FTSysName.ToString());
                    ogdAPI.SetData(ogdAPI.Rows.Count - ogdAPI.Rows.Fixed, ogdAPI.Cols["otbTitleAPIValue"].Index, oCfg.FTSysStaUsrValue.ToString());
                    ogdAPI.SetData(ogdAPI.Rows.Count - ogdAPI.Rows.Fixed, ogdAPI.Cols["otbSysCode"].Index, oCfg.FTSysCode.ToString());

                }

                ////*Arm 63-04-08 Show API2ARDoc
                //if (cVB.nVB_Language == 1)
                //{
                //    ogdAPI.Rows.Add("ALL", "ที่อยู่ Service API2ARDoc", tW_API2ARDoc, "");
                //}
                //else
                //{
                //    ogdAPI.Rows.Add("ALL", "API2ARDoc Service Url", tW_API2ARDoc, "");
                //}
                ////+++++++++++++++

                //*Arm 63-06-26
                foreach (cmlGetURL oURL in aoURL)
                {
                    //ogdAPI.Rows.Add(oURL.tUrlGroup, oURL.tUrlName, oURL.tUrl,"");

                    //*Ought 63-09-18 Additem 
                    //string tJsonPrivil = Newtonsoft.Json.JsonConvert.SerializeObject(oURL);
                    ogdAPI.Rows.Add();
                    ogdAPI.SetData(ogdAPI.Rows.Count - ogdAPI.Rows.Fixed, ogdAPI.Cols["otbTitleService"].Index, oURL.tUrlGroup.ToString());
                    ogdAPI.SetData(ogdAPI.Rows.Count - ogdAPI.Rows.Fixed, ogdAPI.Cols["otbTitleAPIName"].Index, oURL.tUrlName.ToString());
                    ogdAPI.SetData(ogdAPI.Rows.Count - ogdAPI.Rows.Fixed, ogdAPI.Cols["otbTitleAPIValue"].Index, oURL.tUrl.ToString());

                }
                //+++++++++++++
                
                //ogdAPI.ClearSelection();
              
                new cSetting().C_GETxCfgSetting();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_GETxService : " + oEx.Message); }
            finally
            {
                aoCfg = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// set view morkup
        /// </summary>
        private void W_SETxMorkUpView()
        {
            try
            {
                olaFunc1.BackColor = olaClrNormal.BackColor;
                olaFunc2.BackColor = olaClrNormal.BackColor;
                olaFunc3.BackColor = olaClrNormal.BackColor;
                olaFunc4.BackColor = olaClrNormal.BackColor;
                olaFunc5.BackColor = olaClrNormal.BackColor;
                olaPay.BackColor = olaClrNormal.BackColor;

                olaPdt1.BackColor = olaClrLight.BackColor;
                olaPdt2.BackColor = olaClrLight.BackColor;
                olaPdt3.BackColor = olaClrLight.BackColor;

                opnMrkMenu.BackColor = olaClrDark.BackColor;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "W_SETxMorkUpView : " + oEx.Message); }

        }

        #endregion End Function

        #region Method/Events
        /// <summary>
        /// Change Show2nd Screen Flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ockShow2ndScreen_CheckedChanged(object sender, EventArgs e)
        {
            //cVB.nVB_Check2nd = (ockShow2ndScreen.Checked) ? 1 : 0;
        }

        /// <summary>
        /// Clear Data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                otmClose.Stop();
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;
                //*Net 63-02-05 แสดงหน้าจอโฆษณา 

                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.nVB_Check2nd == 1)
                //{
                //    cVB.oVB_2ndScreen = new wShw2ndScreen();
                //    cVB.oVB_2ndScreen.Show();
                //}
                //else if (cVB.nVB_Check2nd == 0)
                //{
                //    cVB.oVB_2ndScreen.Hide();
                //    cVB.oVB_2ndScreen.Dispose();
                //}
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "wSetting_FormClosing : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSetting_Shown(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(otbSrvDB.Text))
                    otbSrvDB.Focus();
                //*Ought 63-09-18 Clear

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "wSetting_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Input / don't input user and password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocbAuthen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ocbAuthen.SelectedIndex == 0)    // Windows
                {
                    otbUsrDB.Enabled = false;
                    otbUsrDB.ReadOnly = true;
                    otbPwdDB.Enabled = false;
                    otbPwdDB.ReadOnly = true;
                }
                else
                {
                    otbUsrDB.Enabled = true;
                    otbUsrDB.ReadOnly = false;
                    otbPwdDB.Enabled = true;
                    otbPwdDB.ReadOnly = false;
                }

                if (ocbDBName.DataSource == null) //*Net 63-07-31
                    ocbDBName.Items.Clear();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "ocbAuthen_SelectedIndexChanged : " + oEx.Message); }
        }

        /// <summary>
        /// Clear Database name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbSrvDB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ocbDBName.Items.Clear();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "otbSrvDB_TextChanged : " + oEx.Message); }
        }

        /// <summary>
        /// Connect Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmConnDB_Click(object sender, EventArgs e)
        {
            try
            {
                ocbDBName.DataSource = null;
                ocbDBName.Items.Clear();

                W_CONxDatabase(true); //*Net 63-08-18 ไม่ต้องใช้ชื่อ DB
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "ocmConnDB_Click : " + oEx.Message); }
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
                if (opnSystem.Visible)   // System
                    W_CHKxValueSystem();
                if (opnStyle.Visible) W_CHKxValueSystem();  //*Em 61-11-19  Style

                //*Net 63-07-31 ปรับตาม Moshi
                //cVB.nVB_SaleModeStd = ocbModeStd.SelectedIndex + 1; //*Net 63-03-28 ยกมาจาก baseline
                ogdAPI.AllowAddNew = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "ocmAccept_Click : " + oEx.Message); }
        }

        private void orbGreen_CheckedChanged(object sender, EventArgs e)
        {
            //*Net 63-03-28 ยกมาจาก baseline
            //olaClrNormal.BackColor = ColorTranslator.FromHtml("#4CAF50");
            //olaClrDark.BackColor = ColorTranslator.FromHtml("#388E3C");
            //olaClrLight.BackColor = ColorTranslator.FromHtml("#C8E6C9");
            olaClrNormal.BackColor = ColorTranslator.FromHtml("#4CAF50");
            olaClrDark.BackColor = ColorTranslator.FromHtml("#388E3C");
            olaClrLight.BackColor = ColorTranslator.FromHtml("#C8E6C9");
            nW_Theme = 1;
            W_SETxMorkUpView();
        }

        private void orbOrange_CheckedChanged(object sender, EventArgs e)
        {
            //*Net 63-03-28 ยกมาจาก baseline
            //olaClrNormal.BackColor = ColorTranslator.FromHtml("#FFA500");
            //olaClrDark.BackColor = ColorTranslator.FromHtml("#FF8C00");
            //olaClrLight.BackColor = ColorTranslator.FromHtml("#FFD700");
            olaClrNormal.BackColor = ColorTranslator.FromHtml("#FF5722");
            olaClrDark.BackColor = ColorTranslator.FromHtml("#E64A19");
            olaClrLight.BackColor = ColorTranslator.FromHtml("#FFCCBC");
            nW_Theme = 2;
            W_SETxMorkUpView();
        }

        private void orbSky_CheckedChanged(object sender, EventArgs e)
        {
            //*Net 63-03-28 ยกมาจาก baseline
            //olaClrNormal.BackColor = ColorTranslator.FromHtml("#0000ff");
            //olaClrDark.BackColor = ColorTranslator.FromHtml("#00008b");
            //olaClrLight.BackColor = ColorTranslator.FromHtml("#add8e6");
            olaClrNormal.BackColor = ColorTranslator.FromHtml("#2196F3");
            olaClrDark.BackColor = ColorTranslator.FromHtml("#1976D2");
            olaClrLight.BackColor = ColorTranslator.FromHtml("#BBDEFB");
            nW_Theme = 3;
            W_SETxMorkUpView();
        }

        private void orbBrown_CheckedChanged(object sender, EventArgs e)
        {
            //*Net 63-03-28 ยกมาจาก baseline
            //olaClrNormal.BackColor = ColorTranslator.FromHtml("#A52A2A");
            //olaClrDark.BackColor = ColorTranslator.FromHtml("#654321");
            //olaClrLight.BackColor = ColorTranslator.FromHtml("#b5651d");
            olaClrNormal.BackColor = ColorTranslator.FromHtml("#795548");
            olaClrDark.BackColor = ColorTranslator.FromHtml("#5D4037");
            olaClrLight.BackColor = ColorTranslator.FromHtml("#D7CCC8");
            nW_Theme = 4;
            W_SETxMorkUpView();
        }

        private void orbPink_CheckedChanged(object sender, EventArgs e)
        {
            //*Net 63-03-28 ยกมาจาก baseline
            //olaClrNormal.BackColor = ColorTranslator.FromHtml("#FFC0CB");
            //olaClrDark.BackColor = ColorTranslator.FromHtml("#e75480");
            //olaClrLight.BackColor = ColorTranslator.FromHtml("#FFB6C1");
            olaClrNormal.BackColor = ColorTranslator.FromHtml("#E91E63");
            olaClrDark.BackColor = ColorTranslator.FromHtml("#C2185B");
            olaClrLight.BackColor = ColorTranslator.FromHtml("#F8BBD0");
            nW_Theme = 5;
            W_SETxMorkUpView();
        }

        private void orbCustom_CheckedChanged(object sender, EventArgs e)
        {
            nW_Theme = 0;
            W_SETxMorkUpView();
        }


        private void olaClrNormal_Click(object sender, EventArgs e)
        {
            /*
            if (orbCustom.Checked == false) return;
            DialogResult oResult = ocdColor.ShowDialog();
            if (oResult == DialogResult.OK)
            {
                olaClrNormal.BackColor = ocdColor.Color;
                W_SETxMorkUpView();
            }
            */
            try
            {
                if (orbCustom.Checked == false) return;
                owSetColor = new wSetColor();
                if (owSetColor.ShowDialog() == DialogResult.OK)
                {
                    olaClrNormal.BackColor = ColorTranslator.FromHtml(owSetColor.rtCode);
                    W_SETxMorkUpView();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "olaClrNormal_Click : " + oEx.Message); }

        }

        private void olaClrDark_Click(object sender, EventArgs e)
        {
            /*
            if (orbCustom.Checked == false) return;
            DialogResult oResult = ocdColor.ShowDialog();
            if (oResult == DialogResult.OK)
            {
                olaClrDark.BackColor = ocdColor.Color;
                W_SETxMorkUpView();
            }
            */

            //*Arm 62-09-19
            try
            {
                if (orbCustom.Checked == false) return;
                owSetColor = new wSetColor();
                if (owSetColor.ShowDialog() == DialogResult.OK)
                {
                    olaClrDark.BackColor = ColorTranslator.FromHtml(owSetColor.rtCode);
                    W_SETxMorkUpView();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "olaClrDark_Click : " + oEx.Message); }

        }

        private void olaClrLight_Click(object sender, EventArgs e)
        {
            /*
            if (orbCustom.Checked == false) return;
            DialogResult oResult = ocdColor.ShowDialog();
            if (oResult == DialogResult.OK)
            {
                olaClrLight.BackColor = ocdColor.Color;
                W_SETxMorkUpView();
            }
            */
            //*Arm 62-09-19
            try
            {
                if (orbCustom.Checked == false) return;
                owSetColor = new wSetColor();
                if (owSetColor.ShowDialog() == DialogResult.OK)
                {
                    olaClrLight.BackColor = ColorTranslator.FromHtml(owSetColor.rtCode);
                    W_SETxMorkUpView();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "olaClrLight_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Menu bar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    case "C_KBDxBack": W_PRCxBack(); break;
                    case "C_KBDxStyle":
                        opnStyle.Visible = true;
                        //ocmGeneral.BackColor = cVB.oVB_ColDark;
                        //ocmStyle.BackColor = cVB.oVB_ColNormal;
                        //ocmSystem.BackColor = cVB.oVB_ColDark;
                        opnSystem.Visible = false;
                        W_SETxMorkUpView();
                        break;
                    case "C_KBDxSystem":
                        W_GETxSystem();
                        //ogdAPI.ClearSelection();
                        break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "ocmMenuBar_Click : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }

        private void ocmBrwPos_Click(object sender, EventArgs e)
        {
            wSearch2Column oForm;
            try
            {
                // Net* 63-04-09 ถ้าเปิดรอบอยู่ไม่ให้แก้ Pos
                if (!string.IsNullOrEmpty(cVB.tVB_ShfCode))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgClosedShift"), 3);
                    return;
                }

                if (string.IsNullOrEmpty(otbBchCode.Text))
                {
                    oForm = new wSearch2Column("TCNMPos", cVB.tVB_BchCode);
                }
                else
                {
                    oForm = new wSearch2Column("TCNMPos", otbBchCode.Text);
                }
                oForm.ShowDialog();
                if (oForm.DialogResult == DialogResult.OK)
                {
                    otbPosCode.Text = oForm.oW_DataSearch.tCode;

                    otbRunBillSale.Text = "0"; //*Net 63-07-31
                    otbRunBillRet.Text = "0"; //*Net 63-07-31
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "otbSrvDB_TextChanged : " + oEx.Message); }

        }
        //*Ought 63-09-21 เพิ่ม Hittest 
        //event ดับเบิ้ลคลิ้ก ที่ Grid 
        private void ogdAPI_CellMouseDoubleClick(object sender, MouseEventArgs e)
        {
            cmlTSysConfig oCfg;
            wEditUrl oFrmEdit;
            
            try
            {
                //*Ought 63-09-21 เปลี่ยนเป็น hittest
               
              
              
                //if (ogdAPI.Rows[e.RowIndex].Cells["otbTitleService"].Value.ToString() == "ALL") return; //*Arm 63-04-08
                //if (ogdAPI.Rows[e.RowIndex].Cells["otbTitleService"].Value.ToString() == "KADS") return; //*Arm 63-06-26
               
                //*Ought 63-09-18 เปลี่ยนมาใช้ Getdata
                if (ogdAPI.GetData(ogdAPI.RowSel, "otbTitleService").ToString() == "ALL") return; //*Ought 63-09-18 เปลี่ยนมาใช้ C1
                if (ogdAPI.GetData(ogdAPI.RowSel, "otbTitleService").ToString() == "KADS") return;
                //if (!string.IsNullOrEmpty(ogdAPI.Rows[e.RowIndex].Cells["otbTitleAPIValue"].Value.ToString()))
                //{
                //    return;
                // //}
                // else
                //{ 
               
                oCfg = new cmlTSysConfig();

                //oCfg.FTSysCode = ogdAPI.Rows[e.RowIndex].Cells["otbSysCode"].Value.ToString();
                //oCfg.FTSysKey = ogdAPI.Rows[e.RowIndex].Cells["otbTitleService"].Value.ToString();
                //oCfg.FTSysName = ogdAPI.Rows[e.RowIndex].Cells["otbTitleAPIName"].Value.ToString();
                //oCfg.FTSysStaUsrValue = ogdAPI.Rows[e.RowIndex].Cells["otbTitleAPIValue"].Value.ToString();
                //oFrmEdit = new wEditUrl(oCfg);
                //oFrmEdit.ShowDialog();

                //*Ought 63-09-18 เปลี่ยนมามาใช้ Getdata
              
                oCfg.FTSysCode = ogdAPI.GetData(ogdAPI.RowSel, "otbSysCode").ToString();
                oCfg.FTSysKey = ogdAPI.GetData(ogdAPI.RowSel, "otbTitleService").ToString();
                oCfg.FTSysName = ogdAPI.GetData(ogdAPI.RowSel, "otbTitleAPIName").ToString();
                oCfg.FTSysStaUsrValue = ogdAPI.GetData(ogdAPI.RowSel, "otbTitleAPIValue").ToString();
                oFrmEdit = new wEditUrl(oCfg);
                oFrmEdit.ShowDialog();

                if (oFrmEdit.DialogResult == DialogResult.OK)
                {
                    //ogdAPI.Rows[e.RowIndex].Cells["otbTitleAPIValue"].Value = oFrmEdit.tW_Url;
                    //ogdAPI.GetData(e.RowIndex, "otbSysCode").ToString() = oFrmEdit.tW_Url.ToString();
                    //*Arm 63-08-07 เก็บค่าลงตัวแปล
                    //if (ogdAPI.Rows[e.RowIndex].Cells["otbSysCode"].Value.ToString() == "tCN_API2PSMaster")
                    //{
                    //cVB.tVB_API2PSMaster = oFrmEdit.tW_Url;
                    //}

                    //if (ogdAPI.Rows[e.RowIndex].Cells["otbSysCode"].Value.ToString() == "tCN_API2PSSale")
                    //{
                    //cVB.tVB_API2PSSale = oFrmEdit.tW_Url;
                    //}

                    //*Ought 63-09-18 เปลี่ยนมาใช้ Getdata
                    if (ogdAPI.GetData(ogdAPI.RowSel, "otbSysCode").ToString() == "tCN_API2PSMaster")
                    {
                        cVB.tVB_API2PSMaster = oFrmEdit.tW_Url;
                    }

                    if (ogdAPI.GetData(ogdAPI.RowSel, "otbSysCode").ToString() == "tCN_API2PSSale")
                    {
                        cVB.tVB_API2PSSale = oFrmEdit.tW_Url;
                    }
                    // +++++++++++++
                }
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "ogdAPI_CellMouseClick : " + oEx.Message); }
        }


        #endregion End Medthod/Events

        private void ocbModeStd_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ocbModeStd.SelectedItem = Properties.Settings.Default.ModeSale.Trim().ToString();

            //string tCheckMode = ""; // Properties.Settings.Default.Save();

            //if (Convert.ToInt32(ocbModeStd.SelectedValue) == -1)
            //{
            //    ocbModeStd.SelectedValue = Properties.Settings.Default.ModeSale.Trim().ToString();
            //}
            //else
            //{
            //    if (ocbModeStd.SelectedValue.ToString() == "1")
            //    {
            //        Properties.Settings.Default.ModeSale = ocbModeStd.SelectedValue.ToString();
            //        Properties.Settings.Default.Save();
            //        cVB.nVB_SaleModeStd = 1;
            //    }
            //    else if (ocbModeStd.SelectedValue.ToString() == "2")
            //    {
            //        Properties.Settings.Default.ModeSale = ocbModeStd.SelectedValue.ToString();
            //        Properties.Settings.Default.Save();
            //        cVB.nVB_SaleModeStd = 2;
            //    }
            //    else
            //    {
            //        ocbModeStd.SelectedValue = Properties.Settings.Default.ModeSale.Trim().ToString();
            //    }
            //}

        }


        private void ocbModeStd_BindingContextChanged(object sender, EventArgs e)
        {

            //if (ocbModeStd.SelectedValue.ToString() == "1")
            //{
            //    Properties.Settings.Default.ModeSale = ocbModeStd.SelectedValue.ToString();
            //    Properties.Settings.Default.Save();
            //}
            //else if (ocbModeStd.SelectedValue.ToString() == "2")
            //{
            //    Properties.Settings.Default.ModeSale = ocbModeStd.SelectedValue.ToString();
            //    Properties.Settings.Default.Save();
            //}
            //else
            //{
            //    ocbModeStd.SelectedValue = Properties.Settings.Default.ModeSale.Trim().ToString();
            //}
        }

        private void OcmBchCode_Click(object sender, EventArgs e)
        {
            wSearch2Column oForm;
            try
            {
                // Net* 63-04-09 ถ้าเปิดรอบอยู่ไม่ให้แก้ Bch
                if (!string.IsNullOrEmpty(cVB.tVB_ShfCode))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgClosedShift"), 3);
                    return;
                }

                oForm = new wSearch2Column("TCNMBranch");
                oForm.ShowDialog();
                if (oForm.DialogResult == DialogResult.OK)
                {
                    otbBchCode.Text = oForm.oW_DataSearch.tCode;
                    otbPosCode.Clear();

                    otbRunBillSale.Text = "0"; //*Net 63-07-31
                    otbRunBillRet.Text = "0"; //*Net 63-07-31

                    tW_API2ARDoc = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + otbBchCode.Text + "' AND FNUrlType = 12");
                    W_GETxService();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "otbSrvDB_TextChanged : " + oEx.Message); }
        }

        //*Net 63-07-31 ปรับตาม Moshi
        private void otbRunBillSale_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetting", "otbRunBillSale_KeyPress : " + oEx.Message); }
        }

        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                switch (poGD.Name)
                {
                    case "ogdAPI":
                        nWidth = poGD.Width;
                        poGD.Cols["otbTitleAPIName"].Width = nWidth * 35 / 100;
                        poGD.Cols["otbTitleAPIValue"].Width = nWidth * 65 / 100;

                        poGD.Cols["otbTitleAPIName"].Caption = oW_Resource.GetString("tTitleName");
                        poGD.Cols["otbTitleAPIValue"].Caption = oW_Resource.GetString("tTitleValue");

                        poGD.Cols["otbTitleAPIName"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbTitleAPIValue"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["otbTitleAPIName"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["otbTitleAPIValue"].TextAlign = TextAlignEnum.LeftCenter;

                        break;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstList", "W_SETxColGrid : " + oEx.Message);
            }
        }


    }
}
