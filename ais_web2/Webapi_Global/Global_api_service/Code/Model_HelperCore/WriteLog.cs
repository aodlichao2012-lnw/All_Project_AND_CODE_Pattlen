using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model_HelperCore
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
                    string path = HttpContext.MapPath("~\\bin") + "\\Information_sql\\";
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


        }
    }
