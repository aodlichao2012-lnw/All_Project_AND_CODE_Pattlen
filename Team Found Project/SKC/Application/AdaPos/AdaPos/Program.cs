using AdaPos.Class;
using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Popup.wLogin;
using AdaPos.Resources_String.Global;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
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
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            cEncryptDecrypt oEncDec;
            cSyncData oSyncData;

            //*Net 62-12-27 ตั้งรูปแบบวันที่เป็น วัน/เดือน/ปี(ค.ศ.) HH:mm:ss
            CultureInfo oCulture = new CultureInfo("en-US");
            oCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
            oCulture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = oCulture;

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

                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        cVB.oVB_GBResource = new ResourceManager(typeof(resGlobal_TH));
                        break;

                    default:    // EN
                        cVB.oVB_GBResource = new ResourceManager(typeof(resGlobal_EN));
                        break;
                }

                cVB.nVB_Setting2nd = 1;
                cVB.nVB_Check2nd = 0;
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
                    }
                    else
                    {
                        oSyncData = new cSyncData();
                        oSyncData.C_PRCxSyncDwn(oSyncData.C_GETaTableSyncDB(), "");
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
