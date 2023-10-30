﻿
using Jose;
using Microsoft.VisualBasic;
using Model_Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class FrmReportTelController : Controller
    {

        private Module2 module;
        string type_db = string.Empty;
        string user_name = string.Empty;

        public FrmReportTelController()
        {
            module = new Module2();
            //WriteLog.instance.Log_browser_Detail_page("FrmReportTel/Index");

        }
        static string  Agenids = string.Empty;


        public async Task< ActionResult> Index(string values ="")
        {
        

            string StrSql = string.Empty;
            int return1 = 0;
            try
            {


                //CultureInfo cultureInfo = new CultureInfo("en-US");
                //Thread.CurrentThread.CurrentCulture = cultureInfo;
                //DateTime datet = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), new CultureInfo("en-US"));
                //string jwt = HttpContext.Request.Cookies["login"].Value;


                //if (jwt != "" && jwt != null)
                //{
                //    string keys = jwt;
                //    //WriteLog.instance.Log("HttpContext.Request.Cookies[\"login\"].Value");
                //    List<string> userjson = module.GetFromToken(keys);

                //    if (datet <= Convert.ToDateTime(userjson[2].ToString()))
                //    {
                //        //WriteLog.instance.Log(" login by user " + WindowsIdentity.DefaultIssuer);
                //        StrSql = "SELECT * FROM PREDIC_AGENTS";
                //        StrSql += " WHERE ROWNUM = 1 AND (LOGIN = :userjson )";

                //        DataSet ds = module.CommandSet(StrSql, "Login_agent", new string[] { userjson[0] }, new string[] { "userjson" });

                //        if (ds != null)
                //        {
                //            if (ds.Tables["Login_agent"].Rows.Count != 0)
                //            {
                //                if (Session["Editv"] != null)
                //                {
                //                    return1 = 1;
                //                }
                //                return1 = 1;
                //            }
                //            else
                //            {

                //                return1 = 0;
                //            }
                //        }
                //        else
                //        {

                //            return1 = 0;
                //        }

                //    }

                //}

                //    if (return1 == 1)
                //{

                //    return View();
                //}
                //else
                //{
                //    return RedirectToAction("Index", "FrmLogin");
                //}
                type_db = "";
                user_name = "";

                CultureInfo cultureInfo = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                DateTime datet = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), new CultureInfo("en-US"));
                string jwt = HttpContext.Request.Cookies["login"].Value;


                if (jwt != "" && jwt != null)
                {
                    string keys = jwt;
                    //WriteLog.instance.Log("HttpContext.Request.Cookies[\"login\"].Value");
                    List<string> userjson = module.GetFromToken(keys);

                    if (datet <= Convert.ToDateTime(userjson[2].ToString()))
                    {
                        //WriteLog.instance.Log(" login by user " + WindowsIdentity.DefaultIssuer);
                        StrSql = "SELECT * FROM PREDIC_AGENTS";
                        StrSql += " WHERE ROWNUM = 1 AND (LOGIN = :userjson )";

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
                    string session = string.Empty;
                    string agenid = JWT.Decode(HttpContext.Request.Cookies["Agen"].Value).Split(':')[1].Split('}')[0].Replace(@"""", "");
                    int count = WriteLog.instance.Log_Get_information_lenght(agenid, DateTime.Now.ToString("yyyyMMdd"));
                    session = WriteLog.instance.Log_Get_information(agenid, DateTime.Now.ToString("yyyyMMdd"));
                    session = session.Split(',')[count - 1].Split('=')[1].ToString();

                    values = session;
                    if (return1 == 1)
                    {
                        List<string> values1 = new List<string>()
                        {
                            values
                        };


                        return View(values1);
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
                WriteLog.instance.Log("Index FrmReport : "+ ex.Message.ToString());
                WriteLog.instance.Log("Index FrmReport : "+ StrSql);
                return  RedirectToAction("Index","FrmReportTel");
            }
            finally
            {
            }

        }
        private string sql = "";

        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        private int i;
        private string status1;
        private int sumservice;
        private string actionflag;
        private int z;
        [HttpGet]

        public string FrmReportTel_Load()
        {
        
            List<string> list = new List<string>();
            list.Add(DateTime.Now.ToString());
            string jsondata_1 = JsonConvert.SerializeObject(setcboStatus());
            list.Add(jsondata_1);
            if(jsondata_1 == "")
            {
                FrmReportTel_Load();
            }
            return JsonConvert.SerializeObject(list);
        }

        private DataTable setcboStatus()
        {
        
            DataTable dt = null;
           string sql = string.Empty;
            try
            {
                sql = "SELECT * FROM MAS_REASON ORDER BY RES_CODE ASC ";
               module.Comman_Static(sql,null,null, ref dt);

                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        {
                            return dt;
                        }
                    }
                }

                return null;
            }
            catch(Exception ex)
            {
                WriteLog.instance.Log("setcboStatus :" + ex.Message.ToString());
                WriteLog.instance.Log("setcboStatus :" + sql);
                return null;
            }
            finally
            {
            
            }
         
        }

        [HttpPost]
        public string btnReport_Click(Telclass telclass)
        {
        
            return showreport(telclass);

        }
        public string list_Service()
        {
            string sql = string.Empty;
            DataTable dt1 = null;
            try
            {
                sql = $@"SELECT DISTINCT MAS_SERV_USED.SERVICE_ID as SER_ID , 
                MAS_SERV_USED.SERVICE_NAME as SER_NAME , MAS_SERV_USED.IS_ACTIVE as IS_ACTIVE , MAS_SERV_USED.is_active as active FROM  MAS_SERV_USED ";


                module.Comman_Static(sql, null, null, ref dt1);

                return JsonConvert.SerializeObject(dt1);
            }
            catch
            {
                return "";
            }
            finally
            {

            }



        }

        [HttpPost]
        public string showreportToday()
        {
            string Agens = string.Empty;
            if (HttpContext.Request.Cookies["Agen"] != null)
            {
                Agens = JWT.Decode(HttpContext.Request.Cookies["Agen"].Value).Split(':')[1].Split('}')[0].Replace(@"""", "");
            }

            string sql2 = "select  ANUMBER, CUST_NAME , CUST_SNAME , MAS_REASON.RES_NAME as RES_NAME , MAS_RESON_DENY.DENY_NAME as DENY_NAME , " +
                     "" +
                     " CASE " +
                     "   WHEN SERVICE_21 = 1 THEN 'สมัคร'" +
                     "   WHEN SERVICE_21 = 0 THEN 'ไม่ได้สมัคร'" +
                     "   END AS SERVICE_21" +
                       ", CASE " +
                     "   WHEN SERVICE_11 = 1 THEN 'สมัคร'" +
                     "   WHEN SERVICE_11 = 0 THEN 'ไม่ได้สมัคร'" +
                     "   END AS SERVICE_11" +
                     " ,  CASE " +
                     "   WHEN SERVICE_12 = 1 THEN 'สมัคร'" +
                     "   WHEN SERVICE_12 = 0 THEN 'ไม่ได้สมัคร'" +
                     "   END AS SERVICE_12" +
                     " ,  CASE " +
                     "   WHEN SERVICE_13 = 1 THEN 'สมัคร'" +
                     "   WHEN SERVICE_13 = 0 THEN 'ไม่ได้สมัคร'" +
                     "   END AS SERVICE_13" +
                     "   from  MAS_LEADS_TRANS " +
                     " LEFT JOIN MAS_REASON ON MAS_REASON.RES_CODE = MAS_LEADS_TRANS.RES_CODE LEFT JOIN MAS_RESON_DENY ON MAS_RESON_DENY.DENY_CODE = MAS_LEADS_TRANS.DENY_CODE" +
                     " ";
           sql2 += " where   MAS_LEADS_TRANS.AGENT_ID = '" + Agens + "' AND MAS_LEADS_TRANS.RES_CODE = '01'";
         
                string dd = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[0];
                string mm = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[1];
                string yy = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[2];
                sql2 += " And to_date(LEAD_CALL_DATE,'YYYY/MM/DD') = to_date('" + dd + "/" + mm + "/" + yy + "','YYYY/MM/DD')";
            Thread.Sleep(1000);
            DataTable  dt3 = module.Comman_Static2(sql2);
            List<string> json_list = new List<string>();
            //DataTable dt2 = Service_Sum(new Telclass() { res_code = "01", agent_id = Agenid });
            List<string> list = new List<string>();
            DataTable dt2 =module. Service_Sum(new Telclass() { res_code = "01", agent_id = Agens });
            string data01 = JsonConvert.SerializeObject(dt2);
            string data02 = JsonConvert.SerializeObject(dt3);
            list.Add(data01);
            list.Add(data02);
            list.Add(list_Service());
            return JsonConvert.SerializeObject(list);


        }
        public string showreport(Telclass telclass)
        {
        
            string sql2 = string.Empty;
            try
            {
                DataTable dt3 = new DataTable();


                if (HttpContext.Request.Cookies["Agen"] != null)
                {
                    string Agens = JWT.Decode(HttpContext.Request.Cookies["Agen"].Value).Split(':')[1].Split('}')[0].Replace(@"""", "");
                    Module2.Agent_Id = Agens;
                }
                string Agenid = Module2.Agent_Id;
                if (Agenid == "")
                {
                    Agenid = Agenids;
                }
                try
                {
                    sql2 = "select  ANUMBER, CUST_NAME , CUST_SNAME , MAS_REASON.RES_NAME as RES_NAME , MAS_RESON_DENY.DENY_NAME as DENY_NAME , " +
                        "" +
                        " CASE " +
                        "   WHEN SERVICE_21 = 1 THEN 'สมัคร'" +
                        "   WHEN SERVICE_21 = 0 THEN 'ไม่ได้สมัคร'" +
                        "   END AS SERVICE_21" +
                          ", CASE " +
                        "   WHEN SERVICE_11 = 1 THEN 'สมัคร'" +
                        "   WHEN SERVICE_11 = 0 THEN 'ไม่ได้สมัคร'" +
                        "   END AS SERVICE_11" + 
                        " ,  CASE " +
                        "   WHEN SERVICE_12 = 1 THEN 'สมัคร'" +
                        "   WHEN SERVICE_12 = 0 THEN 'ไม่ได้สมัคร'" +
                        "   END AS SERVICE_12" +  
                        " ,  CASE " +
                        "   WHEN SERVICE_13 = 1 THEN 'สมัคร'" +
                        "   WHEN SERVICE_13 = 0 THEN 'ไม่ได้สมัคร'" +
                        "   END AS SERVICE_13" +
                        "   from  MAS_LEADS_TRANS  " +

                        "LEFT JOIN MAS_REASON ON MAS_REASON.RES_CODE = MAS_LEADS_TRANS.RES_CODE LEFT JOIN MAS_RESON_DENY ON MAS_RESON_DENY.DENY_CODE = MAS_LEADS_TRANS.DENY_CODE" +
                        " ";
                    sql2 += " where   MAS_LEADS_TRANS.AGENT_ID = '" + Agenid + "' AND MAS_LEADS_TRANS.RES_CODE = '" + telclass.res_code + "'";
                    if (telclass.Day != null)
                    {
                        string dd = telclass.Day.Split('/')[0];
                        string mm = telclass.Day.Split('/')[1];
                        string yy = (Convert.ToInt32(telclass.Day.Split('/')[2] )- 543).ToString().Substring(2, 2);
                        sql2 += " And to_date(LEAD_CALL_DATE,'YYYY/MM/DD') = to_date('" + dd + "/" + mm + "/" + yy + "','YYYY/MM/DD')";
                    }
                    else
                    {
                        string dd = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[0];
                        string mm = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[1];
                        string yy = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[2]; 
                        sql2 += " And to_date(LEAD_CALL_DATE,'YYYY/MM/DD') = to_date('" + dd + "/" + mm + "/" + yy + "','YYYY/MM/DD')";
                    }
                    status1 = telclass.res_code;
                }
                catch
                {
                }
                try
                {
                    ds.Clear();
                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("ไม่สามารถแสดงข้อมูลได้ เนื่องจาก" + ex.Message.ToString() + "ข้อผิดพลาด");
                    WriteLog.instance.Log("showreport :" + sql2);
                    return ("ไม่สามารถแสดงข้อมูลได้ เนื่องจากมีข้อผิดพลาด");
                }
                try
                {
                    telclass.agent_id = Agenid;
                    Thread.Sleep(1000);
                    dt3 = module.Comman_Static4(sql2);
                    if (dt3 != null)
                    {
                        if (dt3.Rows != null)
                        {
                            if (dt3.Rows.Count > 0)
                            {
                                if (status1 == "01")
                                {
                                    telclass.res_code = status1;
                                    List<string> list = new List<string>();
                                    DataTable dt2 = module.Service_Sum2(telclass);
                                    string data01 = JsonConvert.SerializeObject(dt2);
                                    string data02 = JsonConvert.SerializeObject(dt3);
                                    list.Add(data01);
                                    list.Add(data02);
                                    list.Add(list_Service());
                                    return JsonConvert.SerializeObject(list);
                                }
                                else
                                {
                                    telclass.res_code = status1;
                                    List<string> list = new List<string>();
                                    DataTable dt2 = module. Service_Sum3(telclass);
                                    string data01 = JsonConvert.SerializeObject(dt2);
                                    string data02 = JsonConvert.SerializeObject(dt3);
                                    list.Add(data01);
                                    list.Add(data02);
                                    list.Add(list_Service());
                                    return JsonConvert.SerializeObject(list);
                                }
                            }
                            else
                            {

                                return "ไม่มีข้อมูลที่คุณค้นหา";
                            }
                        }
                        else
                        {

                            return "ไม่มีข้อมูลที่คุณค้นหา";
                        }
                    }
                    else
                    {

                        return "ไม่มีข้อมูลที่คุณค้นหา";
                    }

                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("ไม่สามารถแสดงข้อมูลได้ เนื่องจาก" + ex.Message.ToString() + "ข้อผิดพลาด");
                    WriteLog.instance.Log("showreport :" + sql2);
                    return ("ไม่สามารถแสดงข้อมูลได้ เนื่องจาก" + ex.Message.ToString() + "ข้อผิดพลาด");
                }
            }
            catch(Exception ex)
            {
                WriteLog.instance.Log("showreport :" + sql2);
                return ("ไม่สามารถแสดงข้อมูลได้ เนื่องจาก" + ex.Message.ToString() + "ข้อผิดพลาด");
            }
         
          
        }
       
        [HttpPost]
        public string DataGridView1_DoubleClick(Telclass telclass)
        {
        
            Thread.Sleep(2000);
            Module2.Instance.statusfrm = "1";
            Module2.Instance.status_Edit = "Edit";
            List<string> list = new List<string>();
            list.Add(telclass.anumber);
            list.Add(telclass.res_code);
            return JsonConvert.SerializeObject(list);
        }

        
        [HttpPost]
        public string Send_localstoreless(localstoreless localstoreless)
        {
            Module2.strConn_ = localstoreless.strConn;
            Module2.strDB_ = localstoreless.strDB;
            return "";
        }
    }
}