using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ais_web3.Models
{
    public class WriteLog
    {
        private WriteLog() { }
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
        public void Log_browser_Detail_page(string url)
        {
            IWebDriver driver_chome = new ChromeDriver();
            IWebDriver driver_firefox = new FirefoxDriver();
            driver_chome.Navigate().GoToUrl("http://58.137.160.82:8081" + url);
            driver_firefox.Navigate().GoToUrl("http://58.137.160.82:8081" + url);
            IJavaScriptExecutor js1 = (IJavaScriptExecutor)driver_chome;
            IJavaScriptExecutor js2 = (IJavaScriptExecutor)driver_firefox;
            js1.ExecuteScript("console.log = function(msg) { window.myLogs.push(msg); }");
            js2.ExecuteScript("console.log = function(msg) { window.myLogs.push(msg); }");

            // Interact with the web page

            List<object> logs = (List<object>)js1.ExecuteScript("return window.myLogs;");
            foreach (object log in logs)
            {
                Log_browser_chome(log.ToString());
            }
            List<object> log2 = (List<object>)js2.ExecuteScript("return window.myLogs;");
            foreach (object log in log2)
            {
                Log_browser_firefox(log.ToString());
            }
            driver_chome.Quit();
            driver_firefox.Quit();
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

        public void Log_browser_chome(string message)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Information_Browser_chrome\\";
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
        public void Log_browser_firefox(string message)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~\\bin") + "\\Information_Browser_fire_fox\\";
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