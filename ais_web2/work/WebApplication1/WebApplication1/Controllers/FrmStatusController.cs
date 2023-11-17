
using Jose;
using Model_Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ais_web3.Controllers
{
    public class FrmStatusController : Controller
    {
        string session_ID = string.Empty;
        private Module2 module = new Module2();
        string type_db = string.Empty;
        string user_name = string.Empty;
        public FrmStatusController()
        {

        }
        string Agenids = string.Empty;
        [HttpGet]
        public string Index()
        {
   
            return "";
        }
        [HttpGet]
        public string FrmStatus_Load(string id ="" , string Agen ="" , string connectionstring = "" , string Tel = "")
        {
            session_ID = id;

            string json = null;
            try
            {
                    if(Agen != "")
                    {
                        Agenids = Agen;
                    }
                    else  if (HttpContext.Request.Cookies["Agen" + session_ID].Value != null)
                    {
                        Agenids = HttpContext.Request.Cookies["Agen" + session_ID].Value;
                        Module2.Agent_Id = Agenids;
                    }
            }
            catch (Exception ex)
            {
                //WriteLog.instance./*Log*/("Error ที่ FrmStatus_Load : " + ex.Message.ToString());
                //Module2.Agent_Id = "";
            }

        
                json = Get_Project(session_ID , connectionstring , Tel);

          
            return json;
        }
        public string checkTelphone(DataTable dt2, string id, string connectionstring = "")
        {
            string Phone = "";
            if (dt2.Rows[0]["DNIS"] != null && dt2.Rows[0]["DNIS"].ToString().Length > 1)
            {
                Phone = "0"+ dt2.Rows[0]["DNIS"].ToString();
                HttpContext.Response.Cookies.Add(new HttpCookie("Tel" + id, Phone));
                return Phone;
            }
            else
            {
                Phone = "''";
                return Phone;
            }
        }
        public  string Get_Project(string id , string connectionstring ="" , string Tel ="")
        {
            string Status = "";
            string Phone = "";
            string SQL = "";
            try
            {
              
                SQL = "select CNFG_STATUS_CODE.DESCRIPTION  as DESCRIPTION , CNFG_AGENT_INFO.DNIS as DNIS from CNFG_AGENT_INFO,CNFG_STATUS_CODE  where AGENT_ID = :AGENT_ID AND CNFG_AGENT_INFO.LOGON_EXT= CNFG_STATUS_CODE.STATUS_ID AND ROWNUM = 1";
                DataTable dt2 = null;
                module = new Module2(id ,connectionstring);
                  module.Comman_Static2(SQL, new string[] { Agenids }, new string[] { ":AGENT_ID" },ref dt2);
                if (dt2 == null)
                {
                    return "Unknow";
                }
                if (dt2.Rows.Count > 0)
                {
                 
                    if (Tel == "")
                    {

                        Status = dt2.Rows[0]["DESCRIPTION"].ToString();
                    }
                    else if (Tel != "")
                    {

                        Status = "Busy";

                    }

                  
                    if(Status == "Busy")
                    {
                      Phone =  checkTelphone(dt2, id);
                    }

                    return Status + ";" + Phone;
            
                }
                else
                {
                    return "Not Login";
                }


            }
            catch (Exception ex)
            {
                return "Unknow";
            }
        }
    }
}