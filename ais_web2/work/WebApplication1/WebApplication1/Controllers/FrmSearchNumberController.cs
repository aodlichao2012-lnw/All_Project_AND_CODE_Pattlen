
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic;
using Model_Helper;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebApplication1.Models;

namespace ais_web3.Controllers
{
    [OutputCache(Duration = 3600)]
    public class FrmSearchNumberController : Controller
    {
        string Agenids = string.Empty;
        private Module2 module;
        string type_db = string.Empty;
        string user_name = string.Empty;
        public FrmSearchNumberController()
        {
            module = new Module2();
        }
        [HttpGet]
        public ActionResult Index( )
        {
            string StrSql = string.Empty;
            int return1 = 0;
            try
            {
                type_db = "";
                user_name = "";
                CultureInfo cultureInfo = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                DateTime datet = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), new CultureInfo("en-US"));
                string jwt = HttpContext.Request.Cookies["login"].Value;
                if (jwt != "" && jwt != null)
                {
                    string keys = jwt;
                    List<string> userjson = module.GetFromToken(keys);
                    if (datet <= Convert.ToDateTime(userjson[2].ToString()))
                    {
                        StrSql = "SELECT * FROM PREDIC_AGENTS";
                        StrSql += " WHERE  ROWNUM = 1 AND (LOGIN = :userjson )";
                        DataSet ds = module.CommandSet(StrSql, "Login_agent", new string[] { userjson[0] }, new string[] { "userjson" });
                        if (ds != null)
                        {
                            if (ds.Tables["Login_agent"].Rows.Count != 0)
                            {
                                if (Session["Editv"] != null)
                                {
                                    return1 = 1;
                                }
                                return1 = 1;
                            }
                            else
                            {
                                return1 = 0;
                            }
                        }
                        else
                        {
                            return1 = 0;
                        }
                    }
                    if (return1 == 1)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "FrmLogin");
                    }
                }
                return RedirectToAction("Index", "FrmLogin");
            }
            catch (Exception ex)
            {
                ////WriteLog.instance.Log("Index FrmNumberSearch :" + ex.Message.ToString());
                //WriteLog.instance.Log("Index FrmNumberSearch :" + StrSql);
                return RedirectToAction("Index", "FrmSearchNumber");
            }
        }
        private int i;
        private string Today = "";
        private string strday;
        private string strmm;
        private string stryy;
        private int z;
        [HttpPost]
        public string Send_localstoreless(localstoreless localstoreless)
        {
            Module2.strConn_ = localstoreless.strConn;
            Module2.strDB_ = localstoreless.strDB;
            return "";
        }
        [HttpGet]
        public string FrmSearchNumber_Load(string textbox_search_number = "")
        {
            string json = "";
            try
            {
                json = module.CommanDataread(textbox_search_number);
                return json;
            }
            catch(Exception ex)
            {
                //WriteLog.instance.Log("Error ที่ FrmSearchNumber_Load : "+ex.Message.ToString());
                return json;
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
}