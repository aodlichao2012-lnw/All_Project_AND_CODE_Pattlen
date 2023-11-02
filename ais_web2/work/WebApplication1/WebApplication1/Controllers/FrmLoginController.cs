
using Jose;
using Model_Helper;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace ais_web3.Controllers
{
    public class FrmLoginController : Controller
    {


        public async Task< ActionResult> Index(string jwt = null)
        {
            Thread thread = new Thread(() => {

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
                   
                }

            });
            thread.Start();
            thread.Join();
          
            return View();
        }
        private string StrSql = string.Empty;
        private string myHost = Dns.GetHostName();
        private IPHostEntry myIPs;
        private string result;
        private  Module2 module;

        public FrmLoginController()
        {
            //WriteLog.instance.Log_browser_Detail_page("FrmLogin/Index");
            myIPs = System.Net.Dns.GetHostByName(myHost);

            module = new Module2();
        }
        [HttpPost]
        public string Select_database(local_var local_Vars)
        {

            string status = string.Empty;
            List<string> status_list = new List<string>();
            if (local_Vars.checkbox_DB_Backup == true & local_Vars.checkbox_DB_Database == "Production")
            {
                status = "หากต้องการเลือกใช้ Database สำรอง กรุณาเลือก Database เป็น Backup";
            }
            else if (local_Vars.checkbox_DB_Backup == true & local_Vars.checkbox_DB_Database == "Backup")
            {
            }

            if (local_Vars.checkbox_DB_Database == "Production")
            {
                Module2.strDB_ = "Production";
                HttpContext.Response.Cookies["strDB"].Value = Module2.strDB_;
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Production";

            }
            else if (local_Vars.checkbox_DB_Database == "Backup")
            {
                Module2.strDB_ = "Backup";
                HttpContext.Response.Cookies["strDB"].Value = Module2.strDB_;
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Backup";

            }
            else
            {
                Module2.strDB_ = "Production";
                HttpContext.Response.Cookies["strDB"].Value = Module2.strDB_;
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Production";

            }
            status_list.Add(status);
            status_list.Add(Module2.strDB_);
            return JsonConvert.SerializeObject(status_list);
        }

        [HttpPost]
        public string btnLogin_Click(string txtUsername, string txtPassword , string type)
        {
            module = new Module2();
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
                    HttpContext.Response.Cookies["login"].Value = tokenuser + ";" + tokempass;
                    return tokenuser + ";" + tokempass ;

                }
                return result;
            }
        }

        [HttpPost]
        public string LogIn(string txtUsername, string txtPassword , string type)
        {

            try
            {
                if (module.strDB == null)
                {
                    Module2.strDB_ = "Production";
                }
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
             
                if(type_keep == "1200")
                {
                    HttpContext.Response.Cookies["type_title"].Value = ConfigurationManager.AppSettings["type_title"].Split(';')[1];
                    HttpContext.Response.Cookies["type_db"].Value = "1200";
                }
                else if(type_keep == "2400")
                {
                    HttpContext.Response.Cookies["type_title"].Value = ConfigurationManager.AppSettings["type_title"].Split(';')[2];
                    HttpContext.Response.Cookies["type_db"].Value = "2400";
                }
                else if (type_keep == "4800")
                {
                    HttpContext.Response.Cookies["type_title"].Value = ConfigurationManager.AppSettings["type_title"].Split(';')[3];
                    HttpContext.Response.Cookies["type_db"].Value = "4800";
                }
                else
                {
                    HttpContext.Response.Cookies["type_title"].Value = ConfigurationManager.AppSettings["type_title"].Split(';')[0];
                    HttpContext.Response.Cookies["type_db"].Value = "test";
                }
                    Module2.type_db_ = type_keep;
                StrSql = "";
                int i = 0;
                StrSql = "SELECT * FROM PREDIC_AGENTS";
                StrSql += " WHERE (LOGIN = "+ txtUsername + " )";
                StrSql += " and (PASSWORD= "+ txtPassword + " )";
                StrSql += " AND ROWNUM = 1 ";
                module = new Module2();
                DataSet ds = module.CommandSet(StrSql, "Login_agent");
                if (ds.Tables["Login_agent"].Rows.Count != 0)
                {


                    Module2.Agent_Id = ds.Tables["Login_agent"].Rows[0]["Agent_Id"].ToString();
                    Module2.EXTENSION = ds.Tables["Login_agent"].Rows[0]["EXTENSION"].ToString();
                    //HttpContext.Session.Add("Agent_Id" , Module2.Agent_Id);
                    WriteLog.instance.Log_Save_information(Module2.Agent_Id, DateTime.Now.ToString("yyyyMMdd"));
                    Module2.Instance.Group_Id = Convert.ToInt32(ds.Tables["Login_agent"].Rows[0]["Group_Id"]);
                    Module2.Instance.agent = ds.Tables["Login_agent"].Rows[i]["FIRST_NAME"].ToString();
                    Module2.Agent_Id = ds.Tables["Login_agent"].Rows[i]["AGENT_ID"].ToString();
                    Module2.Instance.strUsername = ds.Tables["Login_agent"].Rows[i]["LOGIN"].ToString();
                    Module2.Instance.strPassword = ds.Tables["Login_agent"].Rows[i]["PASSWORD"].ToString();
                    string user_name = " " + ds.Tables["Login_agent"].Rows[i]["FIRST_NAME"].ToString() + " " + ds.Tables["Login_agent"].Rows[i]["LAST_NAME"].ToString() + " ";
                    Module2.user_name_ = user_name;
                    HttpContext.Response.Cookies["user_name"].Value = HttpUtility.UrlEncode( user_name);
                    HttpContext.Response.Cookies["Agen"].Value =/* JWT.Encode( new Dictionary<string, string>() { { "Agen", Module2.Agent_Id } },null,JwsAlgorithm.none);*/Module2.Agent_Id;

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