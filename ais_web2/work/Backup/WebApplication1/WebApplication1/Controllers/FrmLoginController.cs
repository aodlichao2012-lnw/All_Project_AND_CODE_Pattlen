using ais_web3.Models;
using Jose;
using Newtonsoft.Json;
//using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Client;
//using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace ais_web3.Controllers
{
    public class FrmLoginController : Controller
    {
        [Obsolete]
        public ActionResult Index(string jwt = null)
        {
            if (HttpContext.Request.Cookies.AllKeys.Length > 0)
            {
                HttpContext.Response.Cookies.Clear();
            }
            DateTime datet = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), new CultureInfo("en-US"));
            string StrSql = string.Empty;
            if (jwt == null)
            {
                CultureInfo cultureInfo = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                return View();
            }
            List<string> userjson = Module2.Instance.GetFromToken(jwt);
            if (datet <= Convert.ToDateTime(userjson[2].ToString()))
            {
                WriteLog.instance.Log(" login by user " + WindowsIdentity.DefaultIssuer);
                StrSql = "SELECT * FROM PREDIC_AGENTS";
                StrSql += " WHERE (LOGIN ='" + userjson[0] + "')";
                DataSet ds1 = new DataSet();
                ds1 = Module2.Instance.CommandSet(StrSql, "Login_agent");
                if (ds1.Tables["Login_agent"].Rows.Count != 0)
                {
                    return RedirectToAction("Index", "FrmDetail");
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        private string StrSql = string.Empty;
        private string myHost = Dns.GetHostName();
        private IPHostEntry myIPs;
        private string result;
        private readonly Module2 module;
        [Obsolete]
        public FrmLoginController()
        {
            WriteLog.instance.Log_browser_Detail_page("FrmLogin/Index");
            myIPs = System.Net.Dns.GetHostByName(myHost);
                module = new Module2() ;
        }
        [HttpPost]
        public string Select_database(local_var local_Vars)
        {
            string status = string.Empty;
            List<string> status_list = new List<string>();
            if (local_Vars.Chk_DB_Backup == true & local_Vars.Cbo_Database == "Production")
            {
                status = "หากต้องการเลือกใช้ Database สำรอง กรุณาเลือก Database เป็น Backup";
            }
            else if (local_Vars.Chk_DB_Backup == true & local_Vars.Cbo_Database == "Backup")
            {
            }

            if (local_Vars.Cbo_Database == "Production")
            {
                Module2.strDB = "Production";
                HttpContext.Response.Cookies["strDB"].Value = Module2.strDB;
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Production";
            }
            else if (local_Vars.Cbo_Database == "Backup")
            {
                Module2.strDB = "Backup";
                HttpContext.Response.Cookies["strDB"].Value = Module2.strDB;
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Backup";
            }
            else
            {
                Module2.strDB = "Production";
                HttpContext.Response.Cookies["strDB"].Value = Module2.strDB;
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Production";
            }
            status_list.Add(status);
            status_list.Add(Module2.strDB);
            return JsonConvert.SerializeObject(status_list);
        }
        [Obsolete]
        [HttpPost]
        public string btnLogin_Click(string txtUsername, string txtPassword , string type)
        {
            if (txtUsername == "")
            {
                return "2";
            }
            else if (txtPassword == "")
            {
                return "3";
            }
            else
            {
              if (HttpContext.Response.Cookies["strDB"].Value == null)
                {
                    HttpContext.Response.Cookies["strDB"].Value = "Production";
                }
                result = LogIn(txtUsername, txtPassword,type);
                if (result  == "1")
                {
                    var username = new Dictionary<string, object>()
{
    { "sub", txtUsername },
    { "exp", DateTime.Now.AddHours(5).ToString("MM/dd/yyyy HH:mm:ss" , new CultureInfo("en-US")) }
};
                    var password = new Dictionary<string, object>()
{
    { "subpass", txtPassword},
    { "exp",  DateTime.Now.AddHours(5).ToString("MM/dd/yyyy HH:mm:ss" , new CultureInfo("en-US")) }
};
                    string tokenuser = JWT.Encode(username, null, JwsAlgorithm.none);
                    string tokempass = JWT.Encode(password, null, JwsAlgorithm.none);
                    return tokenuser + ";" + tokempass ;
                }
                return result;
            }
        }
        [Obsolete]
        [HttpPost]
        public string LogIn(string txtUsername, string txtPassword , string type)
        {
            try
            {
                if (!Regex.IsMatch(txtUsername, @"^[0-9a-zA-z]"))
                {
                    return "04";
                }
                if (!Regex.IsMatch(txtPassword, @"^[0-9a-zA-z]"))
                {
                    return "04";
                }
                string type_keep = type;
                txtUsername = $"'{txtUsername}'";
                txtPassword = $"'{txtPassword}'";
                HttpContext.Response.Cookies["type_db"].Value = type_keep;
                StrSql = "";
                int i = 0;
                StrSql = "SELECT * FROM PREDIC_AGENTS";
                StrSql += " WHERE (LOGIN = "+ txtUsername + " )";
                StrSql += " and (PASSWORD= "+ txtPassword + " )";
                DataSet ds = module.CommandSet(StrSql, "Login_agent");
                if (ds.Tables["Login_agent"].Rows.Count != 0)
                {


                    Module2.Agent_Id = ds.Tables["Login_agent"].Rows[0]["Agent_Id"].ToString();
                    Module2.EXTENSION = ds.Tables["Login_agent"].Rows[0]["EXTENSION"].ToString();
                    Module2.Instance.Group_Id = Convert.ToInt32(ds.Tables["Login_agent"].Rows[0]["Group_Id"]);
                    Module2.Instance.agent = ds.Tables["Login_agent"].Rows[i]["FIRST_NAME"].ToString();
                    Module2.Agent_Id = ds.Tables["Login_agent"].Rows[i]["AGENT_ID"].ToString();
                    Module2.Instance.strUsername = ds.Tables["Login_agent"].Rows[i]["LOGIN"].ToString();
                    Module2.Instance.strPassword = ds.Tables["Login_agent"].Rows[i]["PASSWORD"].ToString();
                    string user_name = " " + ds.Tables["Login_agent"].Rows[i]["FIRST_NAME"].ToString() + " " + ds.Tables["Login_agent"].Rows[i]["LAST_NAME"].ToString() + " ";
                    HttpContext.Response.Cookies["user_name"].Value = user_name;
    
                    HttpContext.Response.Cookies["Agen"].Value = JWT.Encode( new Dictionary<string, string>() { { "Agen", Module2.Agent_Id } },null,JwsAlgorithm.none);

                    HttpContext.Response.Cookies["EXTENSION"].Value = Module2.EXTENSION;
                    foreach (IPAddress myIP in myIPs.AddressList)
                    {
                        Module2.Agent_Ip = myIP.ToString();
                        HttpContext.Response.Cookies["Agent_Ip"].Value = Module2.Agent_Ip;
                    }
                   module. UpdateCNFG_Agent_Info("5",Module2.Agent_Id,Module2.Agent_Ip);
                    //HttpContext.Response.Cookies["type_db"].Value = TempData["type_db"].ToString(); 
                    //HttpContext.Response.Cookies["user_name"].Value = TempData["user_name"].ToString();
                    return "1";
                }
                else
                {
                    return "04";
                }
           
            }
            catch(OracleException ex )
            {
                return "05 "+ ex.Message.ToString();
            }
        }
      
    }
}