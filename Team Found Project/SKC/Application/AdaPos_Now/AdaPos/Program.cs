using AdaPos.Class;
using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Popup.wLogin;
using AdaPos.Resources_String.Global;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// //*Net 63-07-31 ย้ายมาเป็น static ให้ lock single instance ได้
        static bool bSingleInstance;
        static Mutex oMutex = new Mutex(true, Assembly.GetExecutingAssembly().FullName.Split(',')[0], out bSingleInstance);
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            cEncryptDecrypt oEncDec;
            cSyncData oSyncData;

            ////*Net 63-07-09 Lock ไม่ให้เปิดโปรแกรมซ้ำ
            if (bSingleInstance == false)
            {
                new cSP().SP_SHWxMsg(Assembly.GetExecutingAssembly().FullName.Split(',')[0] + " is already running!", 3);
                Environment.Exit(0);
            }

            //*Net 62-12-27 ตั้งรูปแบบวันที่เป็น วัน/เดือน/ปี(ค.ศ.) HH:mm:ss
            CultureInfo oCulture = new CultureInfo("en-GB");
            oCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
            oCulture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = oCulture;
            Thread.CurrentThread.CurrentUICulture = oCulture;   //*Em 63-08-24

            try
            {
                oEncDec = new cEncryptDecrypt("1");
                new cSP().SP_GETxCurrentLanguage();

                //*Arm 63-03-23 [Comment Code]
                //cVB.tVB_Passcode = oEncDec.C_CALtDecrypt(Properties.Settings.Default.Passcode);
                //cVB.nVB_MaxLenPasscode = Convert.ToInt32(oEncDec.C_CALtDecrypt(Properties.Settings.Default.MaxLenPasscode));
                //+++++++++++++

                //*Arm 63-03-23 [Comment Code]
                cVB.tVB_Passcode = oEncDec.C_CALtDecrypt(System.Configuration.ConfigurationManager.AppSettings.Get("Passcode").ToString());
                cVB.nVB_MaxLenPasscode = Convert.ToInt32(oEncDec.C_CALtDecrypt(System.Configuration.ConfigurationManager.AppSettings.Get("MaxLenPasscode").ToString()));
                //+++++++++++++

                //*Net 63-06-06 option เปิด ปิด การพิมพ์ LogInfo
                if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("AlwPrnLogInfo")))
                {
                    if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("AlwPrnLogInfo")) == 1)
                        cVB.bVB_AlwPrnLog = true;
                    else cVB.bVB_AlwPrnLog = false;
                }
                else
                {
                    //*Em 63-06-08
                    var oConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var oSettings = oConfigFile.AppSettings.Settings;
                    if (oSettings["AlwPrnLogInfo"] == null)
                    {
                        oSettings.Add("AlwPrnLogInfo", "0");
                    }
                    else
                    {
                        oSettings["AlwPrnLogInfo"].Value = "0";
                    }
                    oConfigFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(oConfigFile.AppSettings.SectionInformation.Name);
                    cVB.bVB_AlwPrnLog = false;
                    //+++++++++++++++++++
                }
                    

                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        cVB.oVB_GBResource = new ResourceManager(typeof(resGlobal_TH));
                        break;

                    default:    // EN
                        cVB.oVB_GBResource = new ResourceManager(typeof(resGlobal_EN));
                        break;
                }

                //cVB.nVB_Setting2nd = 1;
                //cVB.nVB_Check2nd = 0;
                //cVB.oVB_2ndScreen = new wShw2ndScreen();

                //if (string.IsNullOrEmpty(oEncDec.C_CALtDecrypt(Properties.Settings.Default.NameDB))) //*Arm 63-03-23 [Comment Code]
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("NameDB").ToString())) //*Arm 63-03-23 ไม่ถอดรหัส
                {
                    cVB.oVB_ColDark = ColorTranslator.FromHtml("#096954");
                    cVB.oVB_ColLight = ColorTranslator.FromHtml("#7FD2BF");
                    cVB.oVB_ColNormal = ColorTranslator.FromHtml("#00ab84");
                    cVB.nVB_SettingFrom = 0;
                    Application.Run(new wPasscode());
                    new cSP().SP_PRCxStartApp();    //*Em 62-09-06

                    //*Em 63-08-24
                    if (cVB.oVB_ConnDB.State == ConnectionState.Closed)
                    {
                        Environment.Exit(1);
                    }
                    //++++++++++++
                        
                }
                else
                {
                    new cSP().SP_PRCxStartApp();

                    //if (cVB.oVB_ConnDB == null)
                    if (cVB.oVB_ConnDB == null || cVB.oVB_ConnDB.State == System.Data.ConnectionState.Closed)       //*Em 62-03-06  กรณีที่ Connect DB ไม่ได้
                    {
                        cVB.oVB_ColDark = ColorTranslator.FromHtml("#096954");
                        cVB.oVB_ColLight = ColorTranslator.FromHtml("#7FD2BF");
                        cVB.oVB_ColNormal = ColorTranslator.FromHtml("#00ab84");
                        cVB.nVB_SettingFrom = 0;
                        Application.Run(new wPasscode());
                        new cSP().SP_PRCxStartApp();    //*Em 62-09-06
                                                        
                        //*Em 63-08-24
                        if (cVB.oVB_ConnDB.State == ConnectionState.Closed)
                        {
                            Environment.Exit(1);
                        }
                        //++++++++++++
                    }
                    else
                    {
                        //oSyncData = new cSyncData();
                        //oSyncData.C_PRCxSyncDwn(oSyncData.C_GETaTableSyncDB(), ""); Net 63-05-20 ไป sync ตอน Signin สำเร็จ
                        cVB.oVB_MQ = new cRabbitMQ("STOCKVD2POS", "STOCKVD2POS");
                        if (cVB.bVB_SplashScreen)     // Show Splash screen form
                            Application.Run(new wSplashScreen());
                        else    // Show home form
                            Application.Run(new wSignin(0, ""));
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("Program", "Main : " + oEx.Message); }
        }

    }
}
