using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

using System.Data;
using System.Configuration;

namespace IVRCall_Dll_Agen

{
    class Config{
       
        private static string myPathLog = string.Empty;     //= Environment.CurrentDirectory;      
        public static string  myPortCon  { get; set; }
        public static string myUrlConetAgen { get; set; }
        public static string myUrlConetServer { get; set; }


        private static string errorLogPath = string.Empty;
        private static string eventLogPath = string.Empty;
        private static string strConnection = string.Empty;
             
      //  WriteLog myWriteEvenLog;

        public static string EventLogPath
        {
            get { return Config.eventLogPath; }
            set { Config.eventLogPath = value; }
        }

        public static string ErrorLogPath
        {
            get { return Config.errorLogPath; }
            set { Config.errorLogPath = value; }
        }

        public static string StrConnection
        {
            get { return Config.strConnection; }
            set { Config.strConnection = value; }
        }

        public static string PathLog
        {
            get { return myPathLog; }
            set { myPathLog = value; }
        }

        public Config()
        {
            Initialize();

        }

        public Boolean Initialize()
        {

            try
            {
                //strConnection = "User Id=TELE;Password=TELE;Data Source=ORATSA;";     
              myUrlConetServer = "http://172.21.70.60:5555/LogIn";
            //  myUrlConetServer = "http://172.21.50.146.:5555/LogIn";
                //strConnection = ConfigurationManager.AppSettings["strConn"];                
                //errorLogPath = Environment.CurrentDirectory + ConfigurationManager.AppSettings["ErrorLogPath"];
                //eventLogPath = Environment.CurrentDirectory + ConfigurationManager.AppSettings["EventLogPath"];
                myPortCon = "5555";//ConfigurationManager.AppSettings["PortConect"];
                //myUrlConetAgen = ConfigurationManager.AppSettings["UrlConetAgen"];    

                return true;
            }
            catch (Exception ex)
            {
            //    myWriteEvenLog = new WriteLog();
              //  myWriteEvenLog.WriteEventLog(WriteLog.EventLog, "Error Can not Load file Config :" + ex.Message);
                return false;
            }
        }
    }
}

