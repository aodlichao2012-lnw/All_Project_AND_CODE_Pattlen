using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Model_Helper
{
    public class WriteLog
    {
            public WriteLog() { }
            private static WriteLog Write = null;
            public static WriteLog instance
            {
                get
                {
                    if (Write == null)
                    {
                        Write = new WriteLog();
                        return Write;
                    }
                    return Write;
                }
            }

            public void Log(string message)
            {
                try
                {
                    string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\InformationLog_Sql_And_Event\\";
                    if (!Directory.Exists(path))

                    {
                        Directory.CreateDirectory(path);
                    }
                    using (StreamWriter steam = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
                    {

                        steam.WriteLine(": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": = " + message);
                    }
                }
                catch
                {

                }


            }
      
            public void LogSql(string message)
            {
                try
                {
                    string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Information_sql\\";
                    if (!Directory.Exists(path))

                    {
                        Directory.CreateDirectory(path);
                    }
                    using (StreamWriter steam = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
                    {

                        steam.WriteLine(": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": = " + message);
                    }
                }
                catch
                {

                }


            }
           
        public void Log_Save_information(string Cookie_AgenId, string Datetimes)
            {
                try
                {
              
                    string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Log_Save_AgenID\\"+ DateTime.Now.ToString("yyyyMMdd") +"\\";
                if (!Directory.Exists(path))

                    {
                        Directory.CreateDirectory(path);
                    }
                    using (StreamWriter steam = new StreamWriter(path +"_"+ Cookie_AgenId +"_" + Datetimes + ".txt", true))
                    {

                        steam.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") +"=" + Cookie_AgenId + ",");
                    }
                }
                catch
                {

                }


            } 
        
        public string Log_Get_information(  string Cookie_AgenId, string Datetimes  )
            {
            string messages = string.Empty;
            try
                {
              
                    string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Log_Save_AgenID\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
        
                    using (StreamReader steam = new StreamReader(path + "_" + Cookie_AgenId + "_" + Datetimes+".txt",true))
                    {

                    messages = steam.ReadToEnd( );
                    }
                    return messages;
                }
                catch (Exception ex) 
                {
                     WriteLog.instance.LogSql(ex.Message.ToString());
                return string.Empty;
                }


            }  
        
        public string Log_Get_information_SaveData_And_Edit(string result  ="", string type = "" , string Cookie_AgenId = "", string Datetimes = "", form3  Model = null)

            {
            string messages = string.Empty;
            try
                {
              
                    string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\SaveData_And_Edit\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + "_" + Cookie_AgenId + "_" + Datetimes+".txt",true))
                    {
                     steam.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") +" , Result = " + result+ " , Type = " +type+" ," + "="+ $@" txtTel_No :{Model.txtTel_No} , txtName : {Model.txtName} , txtSName : {Model.txtSName} , cboDate : {Model.cboDate} , cboMouth : {Model.cboMouth} , txtYear : {Model.txtYear} , cboSex : { Model.cboSex} , cboStatus : {Model.cboStatus.ToString().Replace(" ", "")} , cbocity :{  Model.cbocity}" );

                }


                    return messages;
                }
                catch (Exception ex) 
                {
                     WriteLog.instance.LogSql(ex.Message.ToString());
                return string.Empty;
                }


            }
        
        public int Log_Get_information_lenght(  string Cookie_AgenId, string Datetimes )
            {
            int count = 0;
            try
                {
              
                    string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Log_Save_AgenID\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
        
                    using (StreamReader steam = new StreamReader(path + "_" + Cookie_AgenId + "_" + Datetimes+".txt",true))
                    {

                    count = steam.ReadToEnd( ).Split(',').Length - 1;
                    }
                return count;

                }
                catch (Exception ex) 
                {
                     WriteLog.instance.LogSql(ex.Message.ToString());
                return count = 0;
                }


            }


        }
    }
