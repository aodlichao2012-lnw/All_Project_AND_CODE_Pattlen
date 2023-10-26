using ais_web3.Models;
using Jose;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Client;
//using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ais_web3.Controllers
{
    public class FrmSearchNumberController : Controller
    {
        string Agenids = string.Empty;
        [Obsolete]
        private readonly Module2 module;
        string type_db = string.Empty;
        string user_name = string.Empty;
        [Obsolete]
        public FrmSearchNumberController()
        {
            WriteLog.instance.Log_browser_Detail_page("FrmDetail/Index");
            module = new Module2();
        }

        [Obsolete]
        [HttpGet]

        public ActionResult Index( )
        {
            string StrSql = string.Empty;
            try { 
            type_db = HttpContext.Request.Cookies["type_db"].Value;
            user_name = HttpContext.Request.Cookies["user_name"].Value;
            TempData["type_db"] = type_db;
            TempData["user_name"] = user_name;
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            DateTime datet = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), new CultureInfo("en-US"));
            //string jwt = HttpContext.Request.QueryString["jwt"];
            string[] jwt = HttpContext.Request.Cookies.AllKeys;

            foreach (string key in jwt)
            {
                    if (key != "null" && key != "ASP.NET_SessionId" && key.Contains("Ag") != true && key.Contains("editv") != true && key.Contains("EXTENSION") != true && key.Contains("type_db") != true && key.Contains("user_name") != true && key.Contains("strDB") != true && key.Contains("Agent_Ip") != true)
                    {
                    List<string> userjson = Module2.Instance.GetFromToken(key);

                    if (datet <= Convert.ToDateTime(userjson[2].ToString()))
                    {

                        StrSql = "SELECT * FROM PREDIC_AGENTS";
                        StrSql += " WHERE (LOGIN = :userjson )";

                        DataSet ds = module.CommandSet(StrSql, "Login_agent", new string[] { userjson[0] }, new string[] { "userjson" });

                        if (ds != null)
                            if (ds.Tables["Login_agent"].Rows.Count != 0)
                            {
                                if (Session["Editv"] != null)
                                {
                                    return View();
                                }
                                return View();
                            }
                            else
                            {
                                return null;
                            }

                    }
                }
            }
            return null;
            }
            catch(Exception ex) 
            {
                WriteLog.instance.Log("Index FrmNumberSearch :" + ex.Message.ToString());
                WriteLog.instance.Log("Index FrmNumberSearch :" + StrSql);
                return Index();
            }
        }
        private int i;
        private string Today = "";
        private string strday;
        private string strmm;
        private string stryy;
        private int z;
        [Obsolete]
        [HttpGet]

        public string FrmSearchNumber_Load()
        {
            string json = "";
            try
            {
                 json = module.CommanDataread();
                if( json == null)
                {
                    FrmSearchNumber_Load();
                }
                return json;
            }
            catch(Exception ex)
            {
                WriteLog.instance.Log("Error ที่ FrmSearchNumber_Load : "+ex.Message.ToString());
                return FrmSearchNumber_Load();
            }
        }
        [HttpPost]
        private string btnEdit_Click(Telclass telclass)
        {
            List<string> list = new List<string>();
            list.Add(telclass.anumber);
            list.Add(telclass.res_code);
            return JsonConvert.SerializeObject(list);
        }
    }
    public class Telclass2
    {
        public string anumber { get; set; }
        public string cust_name;
        public string cust_sname { get; set; }
        public string cust_sex { get; set; }
        public string service_01 { get; set; }
        public string service_02 { get; set; }
        public string service_03 { get; set; }
        public string service_04 { get; set; }
        public string service_05 { get; set; }
        public string service_06 { get; set; }
        public string service_07 { get; set; }
        public string service_08 { get; set; }
        public string service_09 { get; set; }
        public string service_10 { get; set; }
        public string service_14 { get; set; }
        public string lead_call_date { get; set; }
        public string status { get; set; }
        public string city { get; set; }
    }
}