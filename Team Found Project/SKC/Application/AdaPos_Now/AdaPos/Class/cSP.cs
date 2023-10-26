using AdaPos.Control;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Respond;
using AdaPos.Models.Webservice.Respond.Product;
using AdaPos.Popup.wSale;
using C1.Win.C1FlexGrid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AdaPos.Class
{
    public class cSP
    {
        #region Const

        private const int GEOCLASS_NATION = 16;
        private const int GEO_NATION = 1;
        private const int GEO_LATITUDE = 2;
        private const int GEO_LONGITUDE = 3;
        private const int GEO_ISO2 = 4;
        private const int GEO_ISO3 = 5;
        private const int GEO_RFC1766 = 6;
        private const int GEO_LCID = 7;
        private const int GEO_FRIENDLYNAME = 8;
        private const int GEO_OFFICIALNAME = 9;
        private const int GEO_TIMEZONES = 10;
        private const int GEO_OFFICIALLANGUAGES = 11;

        #endregion End Const

        #region Extenal

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [DllImport("kernel32.dll")]
        static extern int GetUserGeoID(int geoId);

        [DllImport("kernel32.dll")]
        static extern int GetGeoInfo(int geoid, int GeoType, StringBuilder lpGeoData, int cchData, int langid);

        [DllImport("kernel32.dll")]
        static extern int GetUserDefaultLCID();

        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        #endregion End Extenal

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public cSP()
        {

        }

        #endregion End Constructor

        #region Function

        /// <summary>
        /// Clear Memory
        /// </summary>
        public void SP_CLExMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
                //GC.Collect(); //*Net 63-07-30 ปรับตาม Moshi
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CLExMemory : " + oEx.Message); }
        }
        public void SP_CLExLogFile()
        {
            //string tPathFile = "";
            try
            {

                string[] aFileArray = Directory.GetFiles(System.Windows.Forms.Application.StartupPath + @"\Log\", "*.txt");
                foreach (string tFile in aFileArray)
                {
                    FileInfo oFileInfo = new FileInfo(tFile);
                    //string tName = oFileInfo.Name.Replace(".txt", " 23:59:59");
                    //if (Convert.ToDateTime(tName) < DateTime.Today.AddDays(((cVB.nVB_TimeClearLog - 1) * (-1))))
                    //{
                    //    oFileInfo.Delete();
                    //}

                    //*Em 63-06-09
                    if (Convert.ToDateTime(oFileInfo.LastWriteTime) < DateTime.Today.AddDays(((cVB.nVB_TimeClearLog - 1) * (-1))))
                    {
                        oFileInfo.Delete();
                    }
                    //+++++++++++++
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_CLExLogFile : " + oEx.Message);
            }
        }
        /// <summary>
        /// Check เชื่อมต่อ Internet
        /// </summary>
        /// <returns></returns>
        //public bool SP_CHKbConnection(string ptUrl = "https://www.google.com/")
        public bool SP_CHKbConnection(string ptUrl) //*Net 63-04-01 ยกมาจาก baseline
        {
            bool bChkNet = false;

            //*Net 63-04-01 ยกมาจาก baseline
            cClientService oCall = new cClientService();
            HttpResponseMessage oResponse = new HttpResponseMessage();
            string tResponse = "";
            cmlResIsOnline oResIsOnline = new cmlResIsOnline();
            //++++++++++++++++++++++++++++++++
            try
            {
                //*Net 63-04-01 ยกมาจาก baseline
                tResponse = oCall.C_GETtInvoke(ptUrl);
                oResIsOnline = JsonConvert.DeserializeObject<cmlResIsOnline>(tResponse);


                if (oResIsOnline.rtCode == "001" || oResIsOnline.rtCode == "1") // API2PSMaster : Code Success = 001, API2PSSale : Code Success = 1
                {
                    bChkNet = true;
                }
                else
                {
                    bChkNet = false;
                }
                //+++++++++++++++++++++++++++++++
            }
            catch
            {
                bChkNet = false;
            }
            //*Net 63-07-30 ปรับตาม Moshi
            oResponse.Dispose();    //*Em 63-07-17
            oCall.C_PRCxCloseConn();    //*Em 63-07-18

            return bChkNet;
        }

        /// <summary>
        /// นำตัวเลขมาตัดทศนิยมตามตำหน่งที่กำหนดไว้ใน TSysConfig
        /// </summary>
        /// <param name="ptType">บอกชนิดว่านำไปใช้ทำอะไร (1.Show, 2.Save)</param>
        /// <param name="pcNumber">ตัวเลขที่ต้องการโชว์หรือเซฟ</param>
        /// <param name="pnDigit">ตัวเลขที่ระบุจำนวนทศนิยม</param>
        /// <returns>String ที่ทำการปัดเศษแล้ว</returns>
        public string SP_SETtDecShwSve(int ptType, decimal pcNumber, int pnDigit)
        {
            string tFormat = "";

            try
            {
                switch (ptType)
                {
                    case 1:
                        tFormat = string.Format("{0:N" + pnDigit + "}", pcNumber);
                        break;
                    case 2:
                        tFormat = string.Format("{0:F" + pnDigit + "}", pcNumber);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_SETtDecShwSve : " + oEx.Message); }
            finally
            {
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }

            return tFormat;
        }

        /// <summary>
        /// Method Show Dialog
        /// 1 : Information
        /// 2 : Error
        /// 3 : Warning
        /// 4 : Question
        /// </summary>
        /// <param name="ptMsg">Text ที่ต้องการจะแสดง</param>
        //public void SP_SHWxMsg(string ptMsg, int pnCaseIcon)
        public void SP_SHWxMsg(string ptMsg, int pnCaseIcon, Form pwOwner = null)
        {
            Form wOwner = new Form();
            try
            {
                //*Net 63-07-30 ให้ Message box ขึ้นมาที่หน้าหลัก
                if (pwOwner == null)
                {
                    wOwner.Location = Screen.PrimaryScreen.WorkingArea.Location;
                    wOwner.StartPosition = FormStartPosition.Manual;
                    wOwner.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    wOwner = pwOwner;
                }
                wOwner.TopMost = true;
                //++++++++++++++++++++++++++++++++++++++++
                switch (pnCaseIcon)
                {
                    case 1:
                        MessageBox.Show(wOwner, ptMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case 2:
                        MessageBox.Show(wOwner, ptMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case 3:
                        MessageBox.Show(wOwner, ptMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case 4:
                        MessageBox.Show(wOwner, ptMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        break;
                   
                    
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_SHWxMsg : " + oEx.Message); }
            finally
            {
                if (wOwner != null) wOwner.Dispose();
                wOwner = null;
                ptMsg = null;
                //SP_CLExMemory();
            }
        }
        /// <summary>
        /// Method Return DialogResult
        /// 1 : Question Yes/No
        /// 2 : Question OK/Cancel
        /// </summary>
        /// <param name="ptMsg">Text ที่ต้องการจะแสดง</param>
        public DialogResult SP_SHWoMsg(string ptMsg, int pnCaseIcon ,string ptCaption = "")    //*Arm 62-10-07
        {
            DialogResult odgResult = DialogResult.OK; 
            try
            {
                if (string.IsNullOrEmpty(ptCaption)) ptCaption = Application.ProductName;
                switch (pnCaseIcon)
                {
                    case 1:
                        odgResult = MessageBox.Show(ptMsg, ptCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        break;

                    case 2:
                        odgResult = MessageBox.Show(ptMsg, ptCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        break;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_SHWoMsg : " + oEx.Message);  }
            finally
            {
                ptMsg = null;
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }
            return odgResult;
        }

        /// <summary>
        /// ตรวจสอบ format email
        /// </summary>
        /// <param name="ptEmail">email address</param>
        /// <returns></returns>
        public bool SP_CHKxFormatEmail(string ptEmail)
        {
            bool bChkEmail = false;

            try
            {
                bChkEmail = new EmailAddressAttribute().IsValid(ptEmail);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CHKxFormatEmail : " + oEx.Message); }
            finally
            {
                ptEmail = null;
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }

            return bChkEmail;
        }

        /// <summary>
        /// Send email
        /// </summary>
        /// <returns></returns>
        public bool SP_SNDxEmail(cmlEmail poEmail)
        {
            bool bChkSend = false;
            SmtpClient oClient = null;
            MailMessage oMail = null;

            try
            {
                oMail = new MailMessage();
                oMail.From = new MailAddress(poEmail.tEmailFrom);

                oMail.Subject = poEmail.tSubject;
                oMail.To.Add(new MailAddress(poEmail.tEmailTo));
                oMail.IsBodyHtml = true;
                oMail.BodyEncoding = Encoding.UTF8;
                oMail.Body = poEmail.tBody;

                oClient = new SmtpClient();

                // Send email use authen
                if (!string.IsNullOrEmpty(poEmail.tPwdCred) && !string.IsNullOrEmpty(poEmail.tEmailCred))
                {
                    oClient.Port = poEmail.nPort;
                    oClient.Host = poEmail.tSMTPHost; // SMTP
                    oClient.EnableSsl = true;
                    oClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    oClient.Timeout = poEmail.nTimeout * 1000;
                    oClient.UseDefaultCredentials = false;
                    oClient.Credentials = new NetworkCredential(poEmail.tEmailCred, poEmail.tPwdCred); // User & Password
                }

                oClient.Send(oMail);
                bChkSend = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_SNDxEmail : " + oEx.Message); }
            finally
            {
                if (oClient != null)
                    oClient.Dispose();

                if (oMail != null)
                    oMail.Dispose();

                oClient = null;
                oMail = null;
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }

            return bChkSend;
        }

        /// <summary>
        /// Open Calculator
        /// </summary>
        public void SP_OPNxCalculator()
        {
            try
            {
                Process.Start("Calc");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_OPNxCalculator : " + oEx.Message); }
        }

        /// <summary>
        /// Resign Screen
        /// </summary>
        public void SP_PRCxResizeScreen()
        {
            try
            {
                if (cVB.bVB_StaLockScr)
                {
                    foreach (Form oForm in Application.OpenForms)
                        if (oForm.Name == "wLockKb")
                            oForm.WindowState = FormWindowState.Maximized;
                        else
                            oForm.Hide();

                    cVB.bVB_StaLockScr = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_PRCxResizeScreen : " + oEx.Message); }
        }

        /// <summary>
        /// Check date format
        /// </summary>
        /// <param name="ptDate"></param>
        /// <returns></returns>
        public bool SP_CHKbDateFormat(string ptDate)
        {
            bool bChkFormat = false;
            DateTime dFormat;

            try
            {
                //dFormat = DateTime.Parse(ptDate);
                dFormat = Convert.ToDateTime(ptDate);
                bChkFormat = true;
            }
            catch { bChkFormat = false; }
            finally
            {
                ptDate = null;
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }

            return bChkFormat;
        }

        /// <summary>
        /// Get current language
        /// </summary>
        public void SP_GETxCurrentLanguage()
        {
            string tGetlang;

            try
            {
                //tGetlang = InputLanguage.CurrentInputLanguage.Culture.Parent.Name;
                tGetlang = "TH"; //*Arm 62-10-31 แก้ไข Default ที่ภาษาไทย
                switch (tGetlang.ToUpper())
                {
                    case "TH":
                        cVB.nVB_Language = 1;
                        break;

                    default:    // EN
                        cVB.nVB_Language = 2;
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_GETxCurrentLanguage : " + oEx.Message); }
            finally
            {
                tGetlang = null;
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }
        }

        /// <summary>
        /// Get region location
        /// </summary>
        /// <returns></returns>
        public string SP_GETtRegionLocation()
        {
            StringBuilder oString = null;
            int nGeoI, nLcid;

            try
            {
                oString = new StringBuilder(50);
                nGeoI = GetUserGeoID(GEOCLASS_NATION);
                nLcid = GetUserDefaultLCID();
                GetGeoInfo(nGeoI, 8, oString, oString.Capacity, nLcid);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_GETtRegionLocation : " + oEx.Message); }

            return oString.ToString();
        }

        /// <summary>
        /// Check format email address.
        /// </summary>
        /// <param name="ptEmail">Email address.</param>
        /// <return>
        /// true : format email is correct., 
        /// false : format email is incorrect.
        /// </return>
        public bool SP_CHKbValidateEmail(string ptEmail)
        {
            bool bStaPass = false;
            MailAddress oAddress;

            try
            {
                oAddress = new MailAddress(ptEmail);

                if (oAddress.Address == ptEmail)
                    bStaPass = true;
            }
            catch
            {
                bStaPass = false;
            }
            finally
            {
                oAddress = null;
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }

            return bStaPass;
        }

        /// <summary>
        /// แปลงจาก Text ให้เป็น double
        /// </summary>
        /// <param name="ptValue"></param>
        /// <returns></returns>
        public decimal SP_COVcStringToDouble(string ptValue)
        {
            decimal cValue = 0;

            try
            {
                if (!string.IsNullOrEmpty(ptValue))
                    cValue = Convert.ToDecimal(ptValue);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_COVcStringToDouble : " + oEx.Message); }
            finally
            {
                ptValue = null;
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }

            return cValue;
        }

        /// <summary>
        /// แก้หน้าจอกระพริบ
        /// </summary>
        /// <param name="poIntPtr"></param>
        public void SP_PRCxFlickering(IntPtr poIntPtr)
        {
            int nStyle;

            try
            {
                nStyle = cNativeWinAPI.GetWindowLong(poIntPtr, cNativeWinAPI.GWL_EXSTYLE);
                nStyle |= cNativeWinAPI.WS_EX_COMPOSITED;
                cNativeWinAPI.SetWindowLong(poIntPtr, cNativeWinAPI.GWL_EXSTYLE, nStyle);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_PRCxFlickering : " + oEx.Message); }
        }

        /// <summary>
        /// Process Start App
        /// </summary>
        public async void SP_PRCxStartApp()
        {
            cEncryptDecrypt oDecrypt;

            try
            {
                oDecrypt = new cEncryptDecrypt("1");
                cVB.oVB_Config = new cmlConfigDB();

                //*Arm 63-03-23 [Comment Code]
                //cVB.oVB_Config.tServerDB = oDecrypt.C_CALtDecrypt(Properties.Settings.Default.ServerDB);
                //cVB.oVB_Config.tAuthenDB = oDecrypt.C_CALtDecrypt(Properties.Settings.Default.AuthenDB);
                //cVB.oVB_Config.tUser = oDecrypt.C_CALtDecrypt(Properties.Settings.Default.UserDB);
                //cVB.oVB_Config.tPassword = oDecrypt.C_CALtDecrypt(Properties.Settings.Default.PwdDB);
                //cVB.oVB_Config.tNameDB = oDecrypt.C_CALtDecrypt(Properties.Settings.Default.NameDB);
                //+++++++++++++

                //*Arm 63-03-23 
                Configuration oConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                cVB.oVB_Config.tServerDB = oConfig.AppSettings.Settings["ServerDB"].Value;
                cVB.oVB_Config.tAuthenDB = oConfig.AppSettings.Settings["AuthenDB"].Value;
                cVB.oVB_Config.tUser = oConfig.AppSettings.Settings["UserDB"].Value;
                cVB.oVB_Config.tPassword = oDecrypt.C_CALtDecrypt(oConfig.AppSettings.Settings["PwdDB"].Value);
                cVB.oVB_Config.tNameDB = oConfig.AppSettings.Settings["NameDB"].Value;
                
                //+++++++++++++

                cVB.oVB_ConnDB = new cDatabase().C_CONoDatabase(cVB.oVB_Config);
                if (cVB.oVB_ConnDB.State == ConnectionState.Closed) return; //*Em 63-08-24

                //*[AnUBiS][][2019-01-28]
                new cSetting().C_GETxCfgSetting();
                new cPos();
                new cCompany().C_GETxCompany();
                new cConfig().C_GETxConfig();
                new cRate().C_GETxCurrencyLocal();
                new cSetting().C_GETxCfgSetting();  //*Em 62-11-03
                SP_GETxRateUnit();  //*Em 62-10-08
                                    //new cSetting().C_GETxCfgSetting();
                                    //new cPos();

                ////await new cSignalR().C_CONoSignalRAI();
                ////*Em 62-01-07  WaterPark
                //if (!string.IsNullOrEmpty(cVB.tVB_SgnRPosSrv))
                //{
                //    await new cSignalR().C_CONoSignalRAI();
                //}
                ////+++++++++++++++++++++++++

                //*Net 63-07-08 สร้างหน้าจอ 2
                //if (cVB.bVB_AlwShw2Screen)
                if (cVB.bVB_AlwShwCstScreen) //*Net 63-07-10 เปลี่ยน option
                {
                    if (cVB.oVB_CstScreen != null)
                    {
                        cVB.oVB_CstScreen.Dispose();
                        cVB.oVB_CstScreen = null;
                    }
                    cVB.oVB_CstScreen = new Forms.wCstScreen();
                    cVB.oVB_CstScreen.Show();
                    cVB.oVB_CstScreen.W_SETxHeader(cVB.tVB_BchName, "", cVB.tVB_PosCode, cVB.tVB_UsrName);
                }
                else
                {
                    if (cVB.oVB_CstScreen != null)
                    {
                        cVB.oVB_CstScreen.Close();
                        cVB.oVB_CstScreen.Dispose();
                        cVB.oVB_CstScreen = null;
                    }
                }
                //++++++++++++++++++++++++++++

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_PRCxStartApp : " + oEx.Message); }
        }

        /// <summary>
        /// Convert List to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paoData"></param>
        /// <returns></returns>
        public DataTable SP_COVxDataTable<T>(IList<T> paoData)
        {
            DataTable oTable = new DataTable();
            PropertyDescriptorCollection oProperties;
            DataRow oRow;

            try
            {
                oProperties = TypeDescriptor.GetProperties(typeof(T));

                foreach (PropertyDescriptor oProp in oProperties)
                {

                    oTable.Columns.Add(oProp.Name, Nullable.GetUnderlyingType(oProp.PropertyType) ?? oProp.PropertyType);
                }

                foreach (T oItem in paoData)
                {
                    oRow = oTable.NewRow();

                    foreach (PropertyDescriptor prop in oProperties)
                    {
                        oRow[prop.Name] = prop.GetValue(oItem) ?? DBNull.Value;
                    }

                    oTable.Rows.Add(oRow);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_COVxDataTable : " + oEx.Message); }

            return oTable;
        }

        /// <summary>
        /// ฟังก์ชั่นการนับตัวอักษร (ถ้าเป็นภาษาไทยจะไม่นับสระ)
        /// </summary>
        /// <param name="ptLengthText"></param>
        /// <returns></returns>
        public int SP_PRCnLengthString(string ptLengthText)
        {
            StringInfo tInfo;
            int nLength = 0;

            try
            {
                tInfo = new StringInfo(ptLengthText);
                nLength = tInfo.LengthInTextElements;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_PRCnLengthString : " + oEx.Message); }
            finally
            {
                ptLengthText = null;
                tInfo = null;
                //SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }

            return nLength;
        }

        /// <summary>
        /// ฟังก์ชั่น Notify
        /// </summary>
        /// <param name="polaMsg">Label แสดงจำนวน Notify</param>
        /// <param name="popnNotify">Panel แสดงรายการ Notify</param>
        public static void SP_GETxCountNotify2(ref Label polaMsg, ref Panel popnNotify)
        {
            List<cmlTCNTMsgRemind> aoMsgRemind;
            int nCountMsg;
            //Label olaMsg = new Label();
            //Panel opnNotify = new Panel();
            try
            {
                nCountMsg = new cMsgRemind().C_GETnMaxSeq();

                if (nCountMsg == 0)
                    polaMsg.Visible = false;
                else
                {
                    polaMsg.Text = nCountMsg.ToString();

                    if (popnNotify.Visible)
                        polaMsg.Visible = false;
                    else
                        polaMsg.Visible = true;
                }

                popnNotify.Controls.Clear();
                aoMsgRemind = new cMsgRemind().C_GETaMsgRemind();
                foreach (cmlTCNTMsgRemind oMsg in aoMsgRemind)
                {
                    popnNotify.Controls.Add(new uNotify(oMsg.FNMsgType, string.Format(oMsg.FTMsgData, oMsg.FTSynName), oMsg.FDCreateOn));
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_GETxCountNotify : " + oEx.Message); }
        }

        /// <summary>
        /// ฟังก์ชั่น Notify
        /// </summary>
        /// <param name="polaMsg">Label แสดงจำนวน Notify</param>
        /// <param name="popnNotify">Panel แสดงรายการ Notify</param>
        public static void SP_GETxCountNotify(Label polaMsg, Panel popnNotify)
        {
            List<cmlTCNTMsgRemind> aoMsgRemind;
            int nCountMsg;

            try
            {
                nCountMsg = new cMsgRemind().C_GETnMaxSeq();

                if (nCountMsg == 0)
                    polaMsg.Visible = false;
                else
                {
                    polaMsg.Invoke(new Action(() =>
                    {
                        polaMsg.Text = nCountMsg.ToString();
                        polaMsg.Visible = true;

                        if (popnNotify.Visible)
                            polaMsg.Visible = false;
                        else
                            polaMsg.Visible = true;
                    }));
                }

                popnNotify.Invoke(new Action(() =>
                {
                    popnNotify.Controls.Clear();
                    aoMsgRemind = new cMsgRemind().C_GETaMsgRemind();
                    foreach (cmlTCNTMsgRemind oMsg in aoMsgRemind)
                    {
                        popnNotify.Controls.Add(new uNotify(oMsg.FNMsgType, string.Format(oMsg.FTMsgData, oMsg.FTSynName), oMsg.FDCreateOn));
                    }
                }));
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_GETxCountNotify : " + oEx.Message); }
        }
        
        /*
        public static void SP_CHKxNotify(Label polaMsg, Panel popnNotify)
        {
            try
            {
                if (popnNotify.Visible)
                    popnNotify.Visible = false;
                else
                {
                    polaMsg.Visible = false;
                    polaMsg.Text = "";
                    popnNotify.Visible = true;
                    popnNotify.BringToFront();
                    new cMsgRemind().C_UPDxReadMsg();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "W_CHKxNotify : " + oEx.Message); }
        }
        */
        public static void SP_CHKxNotify(Label polaMsg, Panel popnNotify)
        {
            try
            {
                if (popnNotify.Visible)
                    popnNotify.Visible = false;
                else
                {
                    polaMsg.Visible = false;
                    polaMsg.Text = "";
                    popnNotify.Visible = true;
                    popnNotify.BringToFront();
                    new cMsgRemind().C_UPDxReadMsg();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "W_CHKxNotify : " + oEx.Message); }
        }

        /// <summary>
        /// ฟังก์ชั่น Queue Member
        /// </summary>
        /// <param name="polaMsg">Label แสดงจำนวน Notify</param>
        /// <param name="popnQMember">Panel แสดงรายการ Notify</param>
        public static void SP_GETxCountQMember(Label polaMsg, Panel popnQMember) //*Arm 62-10-24 -Check QMember
        {
            List<cmlTCNTMsgQueue> aoMsgQueue;
            int nCountMsg;

            try
            {
                nCountMsg = new cMsgQueue().C_GETnMaxSeq();
                if (nCountMsg == 0)
                    polaMsg.Visible = false;
                else
                {
                    polaMsg.Invoke(new Action(() =>
                    {
                        if (nCountMsg > 9)
                        {
                            polaMsg.Text = "+9";
                            polaMsg.Visible = true;
                        }
                        else
                        {
                            polaMsg.Text = nCountMsg.ToString();
                            polaMsg.Visible = true;
                        }

                        if (popnQMember.Visible)
                            polaMsg.Visible = false;
                        else
                            polaMsg.Visible = true;
                    }));
                }
                popnQMember.Invoke(new Action(() =>
                {
                    popnQMember.Controls.Clear();
                    aoMsgQueue = new cMsgQueue().C_GETaMsgQueue();
                    foreach (cmlTCNTMsgQueue oMsg in aoMsgQueue)
                    {
                        popnQMember.Controls.Add(new uMsgQueue(oMsg.FTMsgQData + "|" + oMsg.FNMsgID, oMsg.FDCreateOn));
                          
                    }
                }));
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_GETxCountQMember : " + oEx.Message); }
        }
        public static void SP_CHKxQMember(Label polaMsg, Panel popnQMember)   //*Arm 62-10-24 -Check QMember
        {
            try
            {
                if (popnQMember.Visible)
                    popnQMember.Visible = false;
                else
                {
                    polaMsg.Visible = false;
                    polaMsg.Text = "";
                    popnQMember.Visible = true;
                    popnQMember.BringToFront();
                    new cMsgQueue().C_UPDxReadMsg();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CHKxQMember : " + oEx.Message); }
        }

        /// <summary>
        /// [ping][2019.06.06][ฟังก์ชั่นสำหรับ Load AppSignType] 
        /// </summary>
        /// <returns></returns>
        public List<cmlTCNMAppModule> SP_GEToAppSignType()
        {
            List<cmlTCNMAppModule> aoAppSignType = new List<cmlTCNMAppModule>();
            StringBuilder oSql;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTAppSignType FROM TCNMAppModule WITH(NOLOCK) ");
                switch (cVB.tVB_PosType)
                {
                    case "":
                    case "1": // Store
                        oSql.AppendLine("WHERE FTAppCode = 'PS' AND FTAppModule = 1 ");
                        break;

                    case "2": // Cashier
                        oSql.AppendLine("WHERE FTAppCode = 'FC' AND FTAppModule = 1 ");
                        break;
                    default:
                        oSql.AppendLine("WHERE FTAppCode = 'PS' AND FTAppModule = 1 ");
                        break;
                }
                oSql.AppendLine("ORDER BY FNAppSeqNo ASC");

                aoAppSignType = new cDatabase().C_GETaDataQuery<cmlTCNMAppModule>(oSql.ToString());
                return aoAppSignType;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_GEToAppModule : " + oEx.Message);
                return null;
            }
            finally
            {
                oSql = null;
                aoAppSignType = null;
            }
        }

        /// <summary>
        /// Create image from base64
        /// </summary>
        /// <param name="ptBase64"></param>
        /// <returns></returns>
        public string SP_PRCtBase64ToImagePath(cmlResInfoImgPdt poImgPdt)
        {
            string tPathImag = "";
            try
            {
                byte[] abytes = Convert.FromBase64String(poImgPdt.rtImgObj);
                tPathImag = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Product";
                if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                tPathImag += @"\" + poImgPdt.rtImgRefID + (string.IsNullOrEmpty(poImgPdt.rtImgKey) ? "" : "_" + poImgPdt.rtImgKey) + ".jpg";
                if (!File.Exists(tPathImag)) File.Delete(tPathImag);
                using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                {
                    oImageFile.Write(abytes, 0, abytes.Length);
                    oImageFile.Flush();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_PRCtBase64ToImagePath : " + oEx.Message);
                tPathImag = "";
            }
            return tPathImag;
        }

        /// <summary>
        /// Create image from base64
        /// </summary>
        /// <param name="ptBase64"></param>
        /// <returns></returns>
        public string SP_PRCtBase64ImageObj(cmlResInfoImgObj poImgObj)
        {
            string tPathImag = "";
            try
            {
                //if (poImgObj.rtImgTable == "TFNMBankNote")
                //{
                //    byte[] abytes = Convert.FromBase64String(poImgObj.rtImgObj);
                //    tPathImag = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Others\BankNote";
                //    if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                //    tPathImag += @"\" + poImgObj.rtImgRefID  + ".jpg";
                //    if (File.Exists(tPathImag)) File.Delete(tPathImag);
                //    using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                //    {
                //        oImageFile.Write(abytes, 0, abytes.Length);
                //        oImageFile.Flush();
                //    }
                //}
                //else
                //{
                //    byte[] abytes = Convert.FromBase64String(poImgObj.rtImgObj);
                //    tPathImag = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Others";
                //    if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                //    tPathImag += @"\" + poImgObj.rtImgRefID + poImgObj.rnImgSeq.ToString() + poImgObj.rtImgTable.ToString() + (string.IsNullOrEmpty(poImgObj.rtImgKey) ? "" : "_" + poImgObj.rtImgKey) + ".jpg";
                //    if (File.Exists(tPathImag)) File.Delete(tPathImag);
                //    using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                //    {
                //        oImageFile.Write(abytes, 0, abytes.Length);
                //        oImageFile.Flush();
                //    }
                //}


                //*Em 63-08-11
                byte[] abytes = Convert.FromBase64String(poImgObj.rtImgObj);
                switch (poImgObj.rtImgTable)
                {
                    case "TFNMBankNote":                      
                        tPathImag = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Others\BankNote";
                        if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                        tPathImag += @"\" + poImgObj.rtImgRefID + ".jpg";
                        if (File.Exists(tPathImag)) File.Delete(tPathImag);
                        using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                        {
                            oImageFile.Write(abytes, 0, abytes.Length);
                            oImageFile.Flush();
                        }
                        break;
                    case "TCNMAdMsg":
                        
                        tPathImag = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Advertise";
                        if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                        tPathImag += @"\" + poImgObj.rtImgRefID + ".jpg";
                        if (File.Exists(tPathImag)) File.Delete(tPathImag);
                        using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                        {
                            oImageFile.Write(abytes, 0, abytes.Length);
                            oImageFile.Flush();
                        }
                        break;
                    default:
                        tPathImag = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Others";
                        if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                        tPathImag += @"\" + poImgObj.rtImgRefID + poImgObj.rnImgSeq.ToString() + poImgObj.rtImgTable.ToString() + (string.IsNullOrEmpty(poImgObj.rtImgKey) ? "" : "_" + poImgObj.rtImgKey) + ".jpg";
                        if (File.Exists(tPathImag)) File.Delete(tPathImag);
                        using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                        {
                            oImageFile.Write(abytes, 0, abytes.Length);
                            oImageFile.Flush();
                        }
                        break;
                }
                //++++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_PRCtBase64ImageObj : " + oEx.Message);
                tPathImag = "";
            }
            return tPathImag;
        }

        /// <summary>
        /// Create image from base64
        /// </summary>
        /// <param name="ptBase64"></param>
        /// <returns></returns>
        public string SP_PRCtBase64ImagePerson(cmlResInfoImgPerson poImgPsn)
        {
            string tPathImag = "";
            try
            {
                byte[] abytes = Convert.FromBase64String(poImgPsn.rtImgObj);
                tPathImag = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\User";
                if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                tPathImag += @"\" + poImgPsn.rtImgRefID + poImgPsn.rnImgSeq.ToString() + poImgPsn.rtImgTable.ToString() + (string.IsNullOrEmpty(poImgPsn.rtImgKey) ? "" : "_" + poImgPsn.rtImgKey) + ".jpg";
                if (!File.Exists(tPathImag)) File.Delete(tPathImag);
                using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                {
                    oImageFile.Write(abytes, 0, abytes.Length);
                    oImageFile.Flush();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_PRCtBase64ImagePerson : " + oEx.Message);
                tPathImag = "";
            }
            return tPathImag;
        }

        /// <summary>
        /// Create image from base64
        /// </summary>
        /// <param name="ptBase64"></param>
        /// <param name="ptFldName"></param>
        /// <returns></returns>
        public string SP_PRCtBase64Image(string ptBase64, string ptFileName, string ptFldName)
        {
            string tPathImag = "";
            try
            {
                if (string.IsNullOrEmpty(ptBase64)) return tPathImag;

                byte[] abytes = Convert.FromBase64String(ptBase64);
                tPathImag = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\" + ptFldName;
                if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                tPathImag += @"\" + ptFileName + ".jpg";
                if (!File.Exists(tPathImag)) File.Delete(tPathImag);
                using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                {
                    oImageFile.Write(abytes, 0, abytes.Length);
                    oImageFile.Flush();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_PRCtBase64Image : " + oEx.Message);
                tPathImag = "";
            }
            return tPathImag;
        }

        /// <summary>
        /// Function การดึงข้อมูล Rate Unit ในสกุลเงินนั้นๆ
        /// </summary>
        /// <param name="ptRteCode"></param>
        public static void SP_GETxRateUnit()
        {
            StringBuilder oSql;

            try
            {
                cVB.aoVB_RateUnit = new List<cmlTFNMRateUnit>();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRteCode, FNRtuSeq, FCRtuFac ");
                oSql.AppendLine("FROM TFNMRateUnit");
                oSql.AppendLine("WHERE FTRteCode = '" + cVB.tVB_RateCode + "' ");

                cVB.aoVB_RateUnit = new cDatabase().C_GETaDataQuery<cmlTFNMRateUnit>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_GETxRateUnit : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// คิดค่าเศษ
        /// </summary>
        /// <param name="pcAmt">เลขที่ต้องการจะปัดเศษ</param>
        /// <returns></returns>
        public static decimal SP_CALcRoundDiff(decimal pcAmt)
        {
            decimal cSweep = 0, cRangS = 0, cRangE = 0, cAmtDiff;
            int nLoop = 1;
            long nAmt;

            try
            {
                if (cVB.bVB_PSysRnd)
                {
                    nAmt = (long)pcAmt;        // pcAmt = 10.12 ถ้าใช้ long nAmt = 10 (แปลงเป็นจำนวนเต็ม)
                    cAmtDiff = pcAmt - nAmt; // เช็คว่ามีเศษมั้ย

                    if (cVB.aoVB_RateUnit == null || cAmtDiff == 0) { return 0; }

                    switch (cVB.tVB_RateType)
                    {
                        case "1":   // ปัดขึ้น
                            for (int nCount = 0; nCount < cVB.aoVB_RateUnit.Count; nCount++)
                            {
                                if (cAmtDiff > cVB.aoVB_RateUnit[nCount].FCRtuFac)   //ถ้าตัวเศษ มีค่ามากกว่า RateUnit
                                {
                                    if (nLoop == cVB.aoVB_RateUnit.Count)    //ถ้ารันมาจนครบค่า RateUnit ที่จะปัดจะ Set เป็นค่า Rate จำนวนเต็ม (ครบ 0.25 0.5 0.75 แล้ว Set เป็น 1)
                                        cSweep = cVB.cVB_Rate;
                                    else    //ให้ RateUnit ตัวถัดไป (0.5)
                                        cSweep = (decimal)cVB.aoVB_RateUnit[nLoop].FCRtuFac;
                                }
                                else // ถ้าตัวเศษ มีค่าน้อยกว่า RateUnit
                                {
                                    if (nLoop == 1)     //ใช้ RateUnit ตัวนี้ในการปัด
                                        cSweep = (decimal)cVB.aoVB_RateUnit[nCount].FCRtuFac;

                                    break;
                                }

                                nLoop += 1;
                            }

                            return cSweep - cAmtDiff;

                        case "2":   // ปัดลง
                            for (int nCount = 0; nCount < cVB.aoVB_RateUnit.Count; nCount++)
                            {
                                if (cAmtDiff >= cVB.aoVB_RateUnit[nCount].FCRtuFac)  //ถ้าตัวเศษ มีค่ามากกว่าหรือเท่ากับ RateUnit
                                    cSweep = (decimal)cVB.aoVB_RateUnit[nCount].FCRtuFac;
                                else
                                {
                                    if (nLoop == 1)     //ถ้า RateUnit ที่ใช้เป็นตัวแรก
                                        cSweep = 0;

                                    break;
                                }

                                nLoop += 1;
                            }

                            return cSweep - cAmtDiff;

                        case "3":   // ค่ากลาง
                            for (int nCount = 0; nCount < cVB.aoVB_RateUnit.Count; nCount++)
                            {
                                // มากกว่า
                                if (cAmtDiff > cVB.aoVB_RateUnit[nCount].FCRtuFac)
                                {
                                    if (nLoop == cVB.aoVB_RateUnit.Count)      //เอา (ค่า Rate + ตัวสุดท้าย) / 2
                                    {
                                        cSweep = (decimal)(cVB.aoVB_RateUnit[nCount].FCRtuFac + cVB.cVB_Rate) / 2;

                                        return cSweep - cAmtDiff;
                                    }
                                }

                                // เท่ากับ
                                if (cAmtDiff == cVB.aoVB_RateUnit[nCount].FCRtuFac)
                                { return 0; }

                                // น้อยกว่า
                                if (cAmtDiff < cVB.aoVB_RateUnit[nCount].FCRtuFac)
                                {
                                    if (nLoop == 1)  // เอาค่า RateUnit ตัวแรก / 2
                                        cSweep = (decimal)cVB.aoVB_RateUnit[nCount].FCRtuFac / 2;
                                    else     //(เอาค่า RateUnit ปัจจุบัน + RateUnit ตัวก่อนหน้านี้) / 2
                                        cSweep = ((decimal)cVB.aoVB_RateUnit[nCount].FCRtuFac + (decimal)cVB.aoVB_RateUnit[nLoop - 2].FCRtuFac) / 2;

                                    return cSweep - cAmtDiff;
                                }

                                nLoop += 1;
                            }
                            break;

                        case "4":   // ไม่ปัด
                            return 0;

                        case "5":   // ปัดตามคณิตศาสตร์
                            for (int nCount = 0; nCount < cVB.aoVB_RateUnit.Count; nCount++)
                            {
                                cRangE = (decimal)cVB.aoVB_RateUnit[nCount].FCRtuFac;

                                if (cAmtDiff >= cRangS && cAmtDiff <= cRangE)
                                {
                                    if (cAmtDiff >= (cRangS + ((cRangE - cRangS) / 2)))
                                        cSweep = cRangE;
                                    else
                                        cSweep = cRangS;
                                    break;
                                }

                                if (nLoop == cVB.aoVB_RateUnit.Count)
                                {
                                    if (cAmtDiff >= (cVB.cVB_Rate + ((cRangE - cVB.cVB_Rate) / 2)))
                                        cSweep = cVB.cVB_Rate;
                                    else
                                        cSweep = cRangE;
                                    break;
                                }
                                else
                                    cRangS = (decimal)cVB.aoVB_RateUnit[nCount].FCRtuFac;

                                nLoop += 1;
                            }

                            return cSweep - cAmtDiff;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CALcRoundDiff : " + oEx.Message); }

            return 0;
        }

        //*Net 63-03-03 ยกมาจาก baseline
        /// <summary>
        /// Set Gridview Design
        /// </summary>
        /// <param name="poGridview"></param>
        ///*Ought 63-09-18 เปลี่ยนจาก grid มาใช้ c1
        public void SP_SETxSetGridviewFormat(DataGridView poGridview)
        {
            try
            {

                // Header
                poGridview.EnableHeadersVisualStyles = false;
                poGridview.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                poGridview.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                //poGridview.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

                //Select Row
                poGridview.DefaultCellStyle.SelectionBackColor = cVB.oVB_ColNormal;
                poGridview.DefaultCellStyle.SelectionForeColor = Color.White;
                poGridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //AlternatingRows
                poGridview.AlternatingRowsDefaultCellStyle.BackColor = cVB.oVB_ColLight;
                poGridview.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_SETxSetDataGridview : " + oEx.Message);
            }
        }

        //*Em 63-05-31
        public void SP_SETxSetGridFormat(C1FlexGrid poGD)
        {
            try
            {
                // Header
                poGD.Styles.Fixed.BackColor = cVB.oVB_ColDark;
                poGD.Styles.Fixed.ForeColor = Color.White;
                //poGridview.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

                //Select Row
                poGD.Styles.Highlight.BackColor = cVB.oVB_ColNormal;
                poGD.Styles.Highlight.ForeColor = Color.White;
                poGD.SelectionMode = SelectionModeEnum.Row;
                //AlternatingRows
                poGD.Styles.Alternate.BackColor = cVB.oVB_ColLight;
                poGD.Styles.Alternate.ForeColor = Color.Black;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_SETxSetGridFormat : " + oEx.Message);
            }
        }
        //++++++++++++++++++++++

        /// <summary>
        /// Net 63-06-09
        /// Resize Label by Panel Overflow
        /// </summary>
        /// <param name="poParent"></param>
        /// <param name="polaAdjTxt"></param>
        public static void SP_SETxFixPanelOverFlow(System.Windows.Forms.Control poParent, Label polaAdjTxt)
        {
            int nSize = 0, nPadding = 0;
            Size oNewSize;
            foreach (System.Windows.Forms.Control oChild in poParent.Controls)
            {
                nSize += oChild.Width;
            }
            if (nSize > poParent.Width && polaAdjTxt.Width - (nSize - poParent.Width) > 0)
            {
                oNewSize = new Size(polaAdjTxt.Width - (nSize - poParent.Width) - 5, polaAdjTxt.Height);

                nPadding = (polaAdjTxt.Height - polaAdjTxt.PreferredHeight) / 2;
                polaAdjTxt.AutoSize = false;
                polaAdjTxt.AutoEllipsis = true;
                polaAdjTxt.Padding = new Padding(0, nPadding, 0, nPadding);
                polaAdjTxt.Size = oNewSize;
            }
        }

        /// <summary>
        /// Get Mac. Address (Arm 63-07-09)
        /// </summary>
        /// <returns></returns>
        public string SP_GETtMacAddress()
        {
            string tMac = "";
            try
            {
                NetworkInterface[] oNet = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface oData in oNet)
                {
                    if (oData.Name == "Ethernet")
                    {
                        IPInterfaceProperties properties = oData.GetIPProperties();
                        tMac = oData.GetPhysicalAddress().ToString();
                        break;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_GETtMacAddress : " + oEx.Message);
            }

            return tMac;
        }

        
        /// <summary>
        /// Arm 63-08-18 ฟังชั่นตรวจสอบรายการเดียว ตอนกดปุ่ม Menu bill
        /// </summary>
        /// <param name="ptFuncName"></param>
        public static void SP_GETxListItem(string ptFuncName)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            DataTable odtTmp = new DataTable();
            wChooseItemRef oChooseItem;
            try
            {
                cSale.nC_DTSeqNo = 0; //*Arm 63-09-11

                oSql.Clear();
                oSql.AppendLine("SELECT FNXsdSeqNo AS otbTitleSeqNo,FTPdtCode AS otbTitlePdtCode,FTXsdPdtName AS otbTitlePdtName,");
                oSql.AppendLine("FTPunName AS otbTitleUnit,FCXsdSetPrice AS otbTitleSetPrice,FCXsdQty AS otbTitleQty,FCXsdQty AS otbTitleQtyRfn,");

                //*Arm 63-06-10 เงื่อนถ้าส่วนท้ายบิล ให้ดึง FCXsdNetAfHD มาแสดง
                if (ptFuncName == "C_KBDxDisAmtBillByDT" || ptFuncName == "C_KBDxDisPerBillByDT")
                {
                    oSql.AppendLine("FCXsdNetAfHD AS otbTitleAmount, ");
                }
                else
                {
                    oSql.AppendLine("FCXsdNet AS otbTitleAmount, ");
                }

                oSql.AppendLine("FCXsdFactor AS otbTitleFactor,FTXsdBarCode AS otbTitleBarcode,'' AS otbTitleRedeem,'' AS otbTitlePmt");
                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " DT WITH(NOLOCK)");
                oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                if (ptFuncName == "C_KBDxVoidItem")
                {
                    oSql.AppendLine("AND DT.FTXsdStaPdt != '4' ");
                }
                else
                {
                    oSql.AppendLine("AND (DT.FTXsdStaPdt != '4' AND DT.FTXsdStaPdt != '3')");
                }
                switch(ptFuncName)
                {
                    case "C_KBDxChgAmt":
                    case "C_KBDxChgPer":
                    case "C_KBDxDisAmt":
                    case "C_KBDxDisPer":
                    case "C_KBDxDisAmtBillByDT":
                    case "C_KBDxDisPerBillByDT":
                        if(ptFuncName != "C_KBDxDisAmtBillByDT" || ptFuncName != "C_KBDxDisPerBillByDT")
                        {
                            //ระบบต้องไม่แสดงสินค้าราคา 0 ให้เลือก
                            oSql.AppendLine("AND DT.FCXsdSetPrice > 0 ");
                        }
                        oSql.AppendLine("AND DT.FTXsdStaAlwDis = '1' ");
                        break;
                }
                oSql.AppendLine("ORDER BY DT.FNXsdSeqNo ASC");

                //*Em 63-05-12
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                
                int nRowsCnt = 0;
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    if (odtTmp.Rows.Count == 1)
                    {
                        //ตรวจสอบกรณี 1 rows
                        switch (ptFuncName)
                        {
                            case "C_KBDxChgAmt":
                            case "C_KBDxChgPer":
                            case "C_KBDxDisAmt":
                            case "C_KBDxDisPer":
                            case "C_KBDxDisAmtBillByDT":
                            case "C_KBDxDisPerBillByDT":
                                // กรณี ลด/ชาจน์
                                oSql.Clear();
                                oSql.AppendLine("SELECT COUNT(*) ");
                                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                                oSql.AppendLine("AND (FTXsdStaPdt != '4' AND FTXsdStaPdt != '3')");
                                nRowsCnt = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());
                                if (nRowsCnt == 1)
                                {
                                    cSale.nC_DTSeqNo = odtTmp.Rows[0].Field<int>("otbTitleSeqNo");
                                    cSale.cC_DTQty = odtTmp.Rows[0].Field<decimal>("otbTitleQty");

                                    cVB.oVB_PdtOrder = new cmlPdtOrder();
                                    cVB.oVB_PdtOrder.tPdtCode = odtTmp.Rows[0].Field<string>("otbTitlePdtCode"); 
                                    cVB.oVB_PdtOrder.tBarcode = odtTmp.Rows[0].Field<string>("otbTitleBarcode");
                                    cVB.oVB_PdtOrder.tPdtName = odtTmp.Rows[0].Field<string>("otbTitlePdtName");
                                    cVB.oVB_PdtOrder.cSetPrice = odtTmp.Rows[0].Field<decimal>("otbTitleAmount");

                                    cVB.oVB_OrderRowIndex = odtTmp.Rows[0].Field<int>("otbTitleSeqNo") - 1;
                                }
                                else
                                {
                                    oChooseItem = new wChooseItemRef(1, ptFuncName);
                                    oChooseItem.ShowDialog();
                                }
                                break;
                            default:
                                //กรณีไม่ใช่ ลด/ชาจน์
                                cSale.nC_DTSeqNo = odtTmp.Rows[0].Field<int>("otbTitleSeqNo");
                                cSale.cC_DTQty = odtTmp.Rows[0].Field<decimal>("otbTitleQty");
                                cVB.oVB_PdtOrder = new cmlPdtOrder();
                                cVB.oVB_PdtOrder.tPdtCode = odtTmp.Rows[0].Field<string>("otbTitlePdtCode");
                                cVB.oVB_PdtOrder.tBarcode = odtTmp.Rows[0].Field<string>("otbTitleBarcode");
                                cVB.oVB_PdtOrder.tPdtName = odtTmp.Rows[0].Field<string>("otbTitlePdtName");
                                cVB.oVB_PdtOrder.cSetPrice = odtTmp.Rows[0].Field<decimal>("otbTitleAmount");
                                cVB.oVB_OrderRowIndex = odtTmp.Rows[0].Field<int>("otbTitleSeqNo") - 1;
                                break;
                        }
                    }
                    else
                    {
                        //มากว่า 1
                        oChooseItem = new wChooseItemRef(1, ptFuncName);
                        oChooseItem.ShowDialog();
                    }
                }
                else
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1); //*Arm 63-09-11
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_GETxListItem : " + oEx.Message);
            }
            finally
            {
                ptFuncName = "";
                oChooseItem = null;
                oSql = null;
                oDB = null;
                odtTmp = null;
            }
        }

        #endregion End Function
    }

    public static class cExtensionMethods
    {
        /// <summary>
        /// Check string null.
        /// </summary>
        /// <param name="ptValue">Value will check null.</param>
        /// <returns>If value is null will return ""</returns>
        public static string SP_CHKtStringNull(this string ptValue, string ptDefault = null)
        {
            try
            {
                if (string.IsNullOrEmpty(ptValue))
                {
                    if (ptDefault != null)
                        ptValue = ptDefault;
                    else
                        ptValue = "";
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CHKtStringNull : " + oEx.Message); }

            return ptValue;
        }

        /// <summary>
        /// Check int null.
        /// </summary>
        /// <param name="pnValue">Value will check null.</param>
        /// <returns>If value is null will return 0</returns>
        public static long SP_CHKnIntegerNull(this long? pnValue, long? pnDefault = null)
        {
            try
            {
                if (pnValue == null)
                {
                    if (pnDefault != null)
                        pnValue = pnDefault;
                    else
                        pnValue = 0;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CHKtStringNull : " + oEx.Message); }

            return (long)pnValue;
        }

        /// <summary>
        /// Check double null.
        /// </summary>
        /// <param name="pnValue">Value will check null.</param>
        /// <returns>If value is null will return 0.0</returns>
        public static decimal SP_CHKcDoubleNull(this decimal? pcValue, decimal? pcDefault = null)
        {
            try
            {
                if (pcValue == null)
                {
                    if (pcDefault != null)
                        pcValue = pcDefault;
                    else
                        pcValue = 0;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CHKtStringNull : " + oEx.Message); }

            return (decimal)pcValue;
        }

        /// <summary>
        /// Check float null.
        /// </summary>
        /// <param name="pnValue">Value will check null.</param>
        /// <returns>If value is null will return 0</returns>
        public static float SP_CHKcFloatNull(this float? pcValue, float? pcDefault = null)
        {
            try
            {
                if (pcValue == null)
                {
                    if (pcDefault != null)
                        pcValue = pcDefault;
                    else
                        pcValue = 0;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CHKcFloatNull : " + oEx.Message); }

            return (float)pcValue;
        }

        /// <summary>
        /// Check float null.
        /// </summary>
        /// <param name="pnValue">Value will check null.</param>
        /// <returns>If value is null will return 0</returns>
        public static DateTime SP_CHKdDateNull(this DateTime? pdValue, DateTime? pdDefault = null)
        {
            try
            {
                if (pdValue == null)
                {
                    if (pdDefault != null)
                        pdValue = pdDefault;
                    else
                        pdValue = DateTime.Now;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CHKdDateNull : " + oEx.Message); }

            return (DateTime)pdValue;
        }

        /// <summary>
        /// Convert string to datetime
        /// </summary>
        /// <returns></returns>
        public static DateTime? SP_COVdStringToDate(this string ptValue)
        {
            DateTime? dDate = null;

            try
            {
                if (!string.IsNullOrEmpty(ptValue))
                    dDate = Convert.ToDateTime(ptValue);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_CHKdDateNull : " + oEx.Message);
            }

            return dDate;
        }
        
    }
}
