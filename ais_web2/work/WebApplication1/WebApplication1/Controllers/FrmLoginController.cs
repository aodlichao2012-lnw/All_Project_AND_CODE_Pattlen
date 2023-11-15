
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
        string session_ID  = Guid.NewGuid().ToString().Substring(0, 6);
        public  ActionResult Index(string jwt = null)
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
                   
                }


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


            module = new Module2(session_ID);
            myHost = Dns.GetHostName();


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
                Response.Cookies.Add(new HttpCookie("strDB" + session_ID, Module2.strDB_));
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Production";

            }
            else if (local_Vars.checkbox_DB_Database == "Backup")
            {
                Module2.strDB_ = "Backup";
                Response.Cookies.Add(new HttpCookie("strDB" + session_ID, Module2.strDB_));
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Backup";

            }
            else
            {
                Module2.strDB_ = "Production";
                Response.Cookies.Add(new HttpCookie("strDB" + session_ID, Module2.strDB_));
                status = ConfigurationManager.AppSettings["title"].ToString() + " - Database Production";

            }
            status_list.Add(status);
            status_list.Add(Module2.strDB_);
            return JsonConvert.SerializeObject(status_list);
        }
        [HttpPost]
        public string btnLogin_Click(string txtUsername, string txtPassword , string type)
        {
            if (txtUsername == "")
            {
                return session_ID +";"+ "2";
            }
            else if (txtPassword == "")
            {
                return session_ID + ";" + "3";
            }
            else
            {
                result = LogIn(txtUsername, txtPassword,type);
                if (result.Split(';')[1]  == "1")
                {
                    var username = new Dictionary<string, object>()
{
    { "sub", txtUsername },
    { "exp", DateTime.Now.AddHours(10).ToString("MM/dd/yyyy HH:mm:ss" , new CultureInfo("en-US")) }
};
                    var password = new Dictionary<string, object>()
{
    { "subpass", txtPassword},
    { "exp",  DateTime.Now.AddHours(10).ToString("MM/dd/yyyy HH:mm:ss" , new CultureInfo("en-US")) }
};
                    string tokenuser = JWT.Encode(username, null, JwsAlgorithm.none);
                    string tokempass = JWT.Encode(password, null, JwsAlgorithm.none);
                    Response.Cookies.Add(new HttpCookie("login"+ session_ID, tokenuser + ";" + tokempass));
                    return session_ID +";"+"1";
                }
                return session_ID+";"+ result;
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
                    return session_ID + ";" + "04";
                }
                if (!Regex.IsMatch(txtPassword, @"^[0-9a-zA-z]"))
                {
                    return session_ID + ";" + "04";
                }
                if (Regex.IsMatch(txtPassword, @"^[ก-ฮ]"))
                {
                    return session_ID + ";" + "04";
                }
                string type_keep = type;
                txtUsername = $"'{txtUsername}'";
                txtPassword = $"'{txtPassword}'";
                if(type_keep == "1200")
                {
                    Response.Cookies.Add(new HttpCookie("type_title" + session_ID, ConfigurationManager.AppSettings["type_title"].Split(';')[0]));
                    Response.Cookies.Add(new HttpCookie("type_db" + session_ID, "1200"));
                }
                else if(type_keep == "2400")
                {
                    Response.Cookies.Add(new HttpCookie("type_title" + session_ID, ConfigurationManager.AppSettings["type_title"].Split(';')[1]));
                              Response.Cookies.Add(new HttpCookie("type_db" + session_ID, "2400"));
                }
                else if (type_keep == "4800")
                {
                    Response.Cookies.Add(new HttpCookie("type_title" + session_ID, ConfigurationManager.AppSettings["type_title"].Split(';')[2]));
                    Response.Cookies.Add(new HttpCookie("type_db" + session_ID, "4800"));
                }
                else
                {
                    Response.Cookies.Add(new HttpCookie("type_title" + session_ID, ConfigurationManager.AppSettings["type_title"].Split(';')[3]));
                    Response.Cookies.Add(new HttpCookie("type_db" + session_ID, "test"));
                }
                    Module2.type_db_ = type_keep;
                StrSql = "";
                int i = 0;
                StrSql = "SELECT * FROM PREDIC_AGENTS";
                StrSql += " WHERE (LOGIN = "+ txtUsername + " )";
                StrSql += " and (PASSWORD= "+ txtPassword + " )";
                StrSql += " AND ROWNUM = 1 ";
                module = new Module2(session_ID);
                DataSet ds = module.CommandSet(StrSql, "Login_agent");
                if (ds.Tables["Login_agent"].Rows.Count != 0)
                {
                    Module2.Agent_Id = ds.Tables["Login_agent"].Rows[0]["Agent_Id"].ToString();
                    Module2.EXTENSION = ds.Tables["Login_agent"].Rows[0]["EXTENSION"].ToString();
                    WriteLog.instance.Log_Save_information(Module2.Agent_Id, DateTime.Now.ToString("yyyyMMdd"));
                    Module2.Instance.Group_Id = Convert.ToInt32(ds.Tables["Login_agent"].Rows[0]["Group_Id"]);
                    Module2.Instance.agent = ds.Tables["Login_agent"].Rows[i]["FIRST_NAME"].ToString();
                    Module2.Agent_Id = ds.Tables["Login_agent"].Rows[i]["AGENT_ID"].ToString();
                    Module2.Instance.strUsername = ds.Tables["Login_agent"].Rows[i]["LOGIN"].ToString();
                    Module2.Instance.strPassword = ds.Tables["Login_agent"].Rows[i]["PASSWORD"].ToString();
                    string user_name = " " + ds.Tables["Login_agent"].Rows[i]["FIRST_NAME"].ToString() + " " + ds.Tables["Login_agent"].Rows[i]["LAST_NAME"].ToString() + " ";
                    Module2.user_name_ = user_name;
                    Response.Cookies.Add(new HttpCookie("user_name" + session_ID, HttpUtility.UrlEncode(user_name)));
                    Response.Cookies.Add(new HttpCookie("Agen" + session_ID, Module2.Agent_Id));
                    Response.Cookies.Add(new HttpCookie("Agen" , Module2.Agent_Id));
                    Response.Cookies.Add(new HttpCookie("id" , session_ID));
                    Response.Cookies.Add(new HttpCookie("EXTENSION" + session_ID, Module2.EXTENSION));
                    string ip = string.Empty;
                    myIPs = System.Net.Dns.GetHostByName(myHost);
                    foreach (IPAddress myIP in myIPs.AddressList)
                    {
                       ip= myIP.ToString();
                        Response.Cookies.Add(new HttpCookie("Agent_Ip" + session_ID, ip));
                    }
                   module.UpdateCNFG_Agent_Info_login("5",Module2.Agent_Id,ip);
                    return session_ID + ";" + "1";
                }
                else
                {
                    return session_ID + ";" + "06";
                }
            }
            catch(OracleException ex )
            {
                return session_ID + ";" + "05 " + ex.Message.ToString();
            }
        }
    }
}