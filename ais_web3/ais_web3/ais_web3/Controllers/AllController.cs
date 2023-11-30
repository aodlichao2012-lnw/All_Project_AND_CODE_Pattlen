using ais_web3.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ais_web3.Controllers
{

    public class AllController : Controller
    {
        Db_connection _Connection = new Db_connection();
        private string myHost = Dns.GetHostName();
        // GET: All
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public string Before_CheckLogin(string type_db, string strDB)
        {

            string strConn = "";
            if (type_db != null)
            {
                if (strDB == "Production")
                {
                    if (type_db == "1200")
                    {
                        {
                            strConn = ConfigurationManager.AppSettings["pro1"].ToString();
                        }

                    }
                    else if (type_db == "2400")
                    {

                        strConn = ConfigurationManager.AppSettings["pro2"].ToString();

                    }
                    else if (type_db == "4800")
                    {

                        strConn = ConfigurationManager.AppSettings["pro3"].ToString();

                    }
                    else
                    {

                        strConn = ConfigurationManager.AppSettings["pro"].ToString();

                    }
                }
                else if (strDB == "Backup")
                {
                    if (type_db == "1200")
                    {

                        strConn = ConfigurationManager.AppSettings["back1"].ToString();

                    }
                    else if (type_db == "2400")
                    {

                        strConn = ConfigurationManager.AppSettings["back2"].ToString();

                    }
                    else if (type_db == "4800")
                    {

                        strConn = ConfigurationManager.AppSettings["back3"].ToString();

                    }
                    else
                    {

                        strConn = ConfigurationManager.AppSettings["back"].ToString();

                    }
                }
                else
                {
                    if (type_db == "1200")
                    {

                        strConn = ConfigurationManager.AppSettings["pro1"].ToString();

                    }
                    else if (type_db == "2400")
                    {

                        strConn = ConfigurationManager.AppSettings["pro2"].ToString();

                    }
                    else if (type_db == "4800")
                    {

                        strConn = ConfigurationManager.AppSettings["pro3"].ToString();

                    }
                    else
                    {

                        strConn = ConfigurationManager.AppSettings["pro"].ToString();

                    }
                }
            }
            else
            {
                if (strDB == "Production")
                {

                    strConn = ConfigurationManager.AppSettings["pro"].ToString();

                }
                else if (strDB == "Backup")
                {

                    strConn = ConfigurationManager.AppSettings["back"].ToString();

                }

            }
            return strConn + ":" +type_db;
        }
        public string CheckLogin(string txtUsername, string txtPassword, string strConn , string type_db)
        {
            try
            {
                string sql = "";
                if (string.IsNullOrEmpty(txtUsername))
                {
                    return "" + ";" + "'" + "" + "'" + ";" + "2" + ";|" + strConn + ":" + type_db;
                }
                if (string.IsNullOrEmpty(txtPassword))
                {
                    return "" + ";" + "'" + "" + "'" + ";" + "3" + ";|" + strConn + ":" + type_db;
                }
                if (!Regex.IsMatch(txtUsername, @"^[0-9a-zA-z]"))
                {
                    return "" + ";" + "'" + "" + "'" + ";" + "04" + ";|" + strConn + ":" + type_db;
                }
                if (!Regex.IsMatch(txtPassword, @"^[0-9a-zA-z]"))
                {
                    return "" + ";" + "'" + "" + "'" + ";" + "04" + ";|" + strConn + ":" + type_db;
                }
                if (Regex.IsMatch(txtPassword, @"^[ก-ฮ]"))
                {
                    return "" + ";" + "'" + "" + "'" + ";" + "104" + ";|" + strConn + ":" + type_db;
                }
                sql = $@"SELECT * FROM PREDIC_AGENTS  WHERE (LOGIN = '{txtUsername}') and (PASSWORD= '{txtPassword}' ) AND ROWNUM = 1 ";
                DataTable dt = _Connection.Query1( sql , strConn);
                if (dt.Rows.Count > 0)
                {
                    string Agen = dt.Rows[0]["Agent_Id"].ToString();
                    string user_name = " " + dt.Rows[0]["FIRST_NAME"].ToString() + " " + dt.Rows[0]["LAST_NAME"].ToString() + " ";

                    string ip = string.Empty;
                    IPHostEntry hostEntry = Dns.GetHostEntry(myHost);

                    foreach (IPAddress myIP in hostEntry.AddressList)
                    {
                        ip = myIP.ToString();
                    }
                    UpdateCNFG_Agent_Info_login("5", Agen, ip ,"", strConn);
                    string retrun_text = Agen + ";" + "'" + user_name + "'" + ";" + "1" + ";|" + strConn + ":" + type_db;
                    return retrun_text;
                }
                else
                {
                    return "" + ";" + "'" + "" + "'" + ";" + "06" + ";|" + strConn + ":" + type_db;
                }
                
            }
            
            catch (OracleException ex)
            {
                return "" + ";" + "05 " + ex.Message.ToString() + ";|" + "" + ":" + type_db;
            }

        }

        public string UpdateCNFG_Agent_Info_login(string status, string Agen = "", string IP = "", string DNIS = "" , string connectionstring ="")
        {

            string strUpdate;
            strUpdate = "";
            strUpdate = "UPDATE CNFG_AGENT_INFO  SET ";

            if (IP != "")
            {
                strUpdate += "TERMINAL_IP   = '" + IP + "' ,";
            }

            strUpdate += " STATUS_ID = " + status + " ,";
            strUpdate += "CALL_COUNT = 0,";
            strUpdate += "LOGON_EXT = " + status + ",";
            if (DNIS != "")
            {
                strUpdate += "DNIS = '' ,";
            }
            strUpdate += " LOGIN_TIME   = sysdate ";
            strUpdate += " WHERE AGENT_ID = '" + Agen + "'";

            try
            {
                {
                    _Connection.Execute(strUpdate , connectionstring);
                    return "200";
                }
            }
            catch (Exception ex)
            {
                return "ระบบมีปัญหา กรุณาติดต่อ Admin ค่ะ" + ex.Message + "ผลการตรวจสอบ";
            }
        }

        [HttpGet]
        public ActionResult Detail(string usernane = "")
        {

            List<string> values1 = new List<string>()
                        {
                            usernane
                        };
            return View(values1);

        }

        [HttpGet]
        public string list_Service2( string connectionstring = "")
        {
            string sql = string.Empty;
            DataTable dt1 = null;
            try
            {
                sql = $@"SELECT DISTINCT MAS_SERV_USED.SERVICE_ID as SER_ID , 
                MAS_SERV_USED.SERVICE_NAME as SER_NAME , MAS_SERV_USED.IS_ACTIVE as IS_ACTIVE , MAS_SERV_USED.is_active as active FROM  MAS_SERV_USED WHERE MAS_SERV_USED.is_active = '1'";
                dt1 = _Connection.Query2(sql, connectionstring);
                return JsonConvert.SerializeObject(dt1);
            }
            catch
            {
                return "";
            }

        }

        [HttpGet]
        public string showCity( string connectionstring = "")
        {
            string searchcity = string.Empty;
            try
            {
                DataTable dt = null;
                string json = string.Empty;
                searchcity = " SELECT CITY_CODE , CITY_NAME_T FROM CALL_SEARCH_CITY ORDER BY CITY_NAME_T ASC";
               dt =  _Connection.Query3(searchcity ,connectionstring);

                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        {
                            json = JsonConvert.SerializeObject(dt);
                        }
                    }
                }
                return json;
            }
            catch (Exception ex)
            {

                return "";
            }

        }

        [HttpGet]
        public string Save_service(string id, string values, string connectionstring = "")
        {
            string sql = string.Empty;
            try
            {
                id = id.Replace("editSERVICE_", "");

                sql = $@"UPDATE MAS_SERVICE SET  SERVICE_NAME = '"+ values + "', MDF_DATE = '"+ DateTime.Now.ToString("dd-MMM-yy") + "' WHERE SERVICE_ID = '"+ id + "' ";
             int   i = _Connection.Execute1(sql, connectionstring);

                if (i == 1)
                {
                    return "บันทึกสำเร็จ";
                }
                else
                {
                    return "บันทึกไม่สำเร็จ";
                }
            }
            catch (Exception ex)
            {
                return "บันทึกไม่สำเร็จ เนื่องจาก " + ex.Message.ToString();
            }
        }

        [HttpGet]
        public string FrmReportTel_Load( string connectionstring = "")
        {
            List<string> list = new List<string>();
            list.Add(DateTime.Now.ToString());
            string jsondata_1 = JsonConvert.SerializeObject(setcboStatus(connectionstring));
            list.Add(jsondata_1);
            if (jsondata_1 == "")
            {
                FrmReportTel_Load();
            }
            return JsonConvert.SerializeObject(list);
        }
        //[HttpGet]
        //public string showreportToday( string connectionstring = "", string Agen = "")
        //{
        //    string Agens = string.Empty;
        //    if (Agen != "")
        //    {
        //        Agens = Agen;
        //    }
        //    string sql2 = "select  ANUMBER, CUST_NAME , CUST_SNAME , MAS_REASON.RES_NAME as RES_NAME , MAS_RESON_DENY.DENY_NAME as DENY_NAME , " +
        //             "" +
        //             " CASE " +
        //             "   WHEN SERVICE_21 = 1 THEN 'สมัคร'" +
        //             "   WHEN SERVICE_21 = 0 THEN 'ไม่ได้สมัคร'" +
        //             "   END AS SERVICE_21" +
        //               ", CASE " +
        //             "   WHEN SERVICE_11 = 1 THEN 'สมัคร'" +
        //             "   WHEN SERVICE_11 = 0 THEN 'ไม่ได้สมัคร'" +
        //             "   END AS SERVICE_11" +
        //             " ,  CASE " +
        //             "   WHEN SERVICE_12 = 1 THEN 'สมัคร'" +
        //             "   WHEN SERVICE_12 = 0 THEN 'ไม่ได้สมัคร'" +
        //             "   END AS SERVICE_12" +
        //             " ,  CASE " +
        //             "   WHEN SERVICE_13 = 1 THEN 'สมัคร'" +
        //             "   WHEN SERVICE_13 = 0 THEN 'ไม่ได้สมัคร'" +
        //             "   END AS SERVICE_13" +
        //             "   from  MAS_LEADS_TRANS " +
        //             " LEFT JOIN MAS_REASON ON MAS_REASON.RES_CODE = MAS_LEADS_TRANS.RES_CODE LEFT JOIN MAS_RESON_DENY ON MAS_RESON_DENY.DENY_CODE = MAS_LEADS_TRANS.DENY_CODE" +
        //             " ";
        //    sql2 += " where   MAS_LEADS_TRANS.AGENT_ID = '" + Agens + "' AND MAS_LEADS_TRANS.RES_CODE = '01'";

        //    string dd = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[0];
        //    string mm = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[1];
        //    string yy = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[2];
        //    sql2 += " And to_date(LEAD_CALL_DATE,'YYYY/MM/DD') = to_date('" + dd + "/" + mm + "/" + yy + "','YYYY/MM/DD')";
        //    Thread.Sleep(1000);
        //    DataTable dt3 = _Connection.Query(sql2, connectionstring);
        //    List<string> json_list = new List<string>();
        //    List<string> list = new List<string>();
        //    DataTable dt2 = Service_Sum(new Model() { res_code = "01", agent_id = Agens });
        //    string data01 = JsonConvert.SerializeObject(dt2);
        //    string data02 = JsonConvert.SerializeObject(dt3);
        //    list.Add(data01);
        //    list.Add(data02);
        //    list.Add(list_Service(connectionstring));
        //    return JsonConvert.SerializeObject(list);
        //}

        public string UpdateCNFG_Agent_Info(string status, string Agen = "", string DNIS = "" , string Connectionstring ="")
        {

            string strUpdate;
            strUpdate = "";
            strUpdate = "UPDATE CNFG_AGENT_INFO  SET ";


            strUpdate += " STATUS_ID = " + status + " ,";
            strUpdate += "CALL_COUNT = 0,";
            strUpdate += "LOGON_EXT = " + status + ",";
            if (DNIS != "")
            {
                strUpdate += "DNIS = '' ,";
            }
            strUpdate += " LOGIN_TIME   = sysdate ";
            strUpdate += " WHERE AGENT_ID = '" + Agen + "'";

            try
            {
                {
                    _Connection.Execute2(strUpdate , Connectionstring);
                    return "200";
                }
            }
            catch (Exception ex)
            {
                return "500";
            }
        }

        [HttpGet]
        public string showreportToday( string connectionstring = "", string Agen = "")
        {

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
            sql2 += " where   MAS_LEADS_TRANS.AGENT_ID = '" + Agen + "' AND MAS_LEADS_TRANS.RES_CODE = '01'";

            string dd = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[0];
            string mm = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[1];
            string yy = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[2];
            sql2 += " And to_date(LEAD_CALL_DATE,'YYYY/MM/DD') = to_date('" + dd + "/" + mm + "/" + yy + "','YYYY/MM/DD')";
            Thread.Sleep(1000);
            DataTable dt3 = _Connection.Query4(sql2 , connectionstring);
            List<string> json_list = new List<string>();
            List<string> list = new List<string>();
            DataTable dt2 = Service_Sum(new form3() { res_code = "01", agent_id = Agen  ,connectionstring  = connectionstring });
            string data01 = JsonConvert.SerializeObject(dt2);
            string data02 = JsonConvert.SerializeObject(dt3);
            list.Add(data01);
            list.Add(data02);
            list.Add(list_Service(connectionstring));
            return JsonConvert.SerializeObject(list);
        }

        public DataTable Service_Sum(form3 telclass)
        {

            string sql = string.Empty;
            try
            {

                DataTable dt = sql_Select_Report(telclass);

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }



        }
        public DataTable Service_Sum2(form3 telclass)
        {

            string sql = string.Empty;
            try
            {

                DataTable dt = sql_Select_Report(telclass);

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }



        }
        public DataTable Service_Sum3(form3 telclass)
        {

            string sql = string.Empty;
            try
            {

                DataTable dt = sql_Select_Report(telclass);

                return dt;
            }
            catch (Exception ex)
            {
                //WriteLog.instance.Log("Service_Sum :" + ex.Message.ToString());
                //WriteLog.instance.Log("Service_Sum :" + sql);
                return null;
            }



        }

        public DataTable sql_Select_Report(form3 telclass)
        {
            string sql2 = string.Empty;
            if (telclass.Agen == null)
            {
                telclass.Agen = telclass.agent_id;
            }

            sql2 = "Select sum(service_21) as ser21 ,sum(service_11) as ser11 ,sum(service_12) as ser12 ,sum(service_13) as ser13 , sum(COALESCE(service_21,'0')+COALESCE(service_11,'0')+COALESCE(service_12,'0') + (CASE WHEN service_13 = null THEN '0' ELSE service_13 END )) AS    sum from mas_leads_trans";
            sql2 += " where  MAS_LEADS_TRANS.AGENT_ID = '" + telclass.Agen + "' and MAS_LEADS_TRANS.RES_CODE = '" + telclass.res_code + "' ";

            if (telclass.Day != "" && telclass.Day != null)
            {
                string dd = telclass.Day.Split('/')[0];
                string mm = telclass.Day.Split('/')[1];
                string yy = (Convert.ToInt32(telclass.Day.Split('/')[2]) - 543).ToString().Substring(2, 2);
                sql2 += " And to_date(LEAD_CALL_DATE,'YYYY/MM/DD') = to_date('" + dd + "/" + mm + "/" + yy + "','YYYY/MM/DD')";
            }
            else
            {
                string dd = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[0];
                string mm = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[1];
                string yy = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[2];
                sql2 += " And to_date(MAS_LEADS_TRANS.LEAD_CALL_DATE,'dd/MM/yyyy') = to_date('" + dd + "/" + mm + "/" + yy + "','dd/MM/yyyy')";
            }
            DataTable dt = new DataTable();
            dt = _Connection.Query9(sql2, telclass.connectionstring);
            return dt;
        }

        public string list_Service( string connectionstring = "")
        {
            string sql = string.Empty;
            DataTable dt1 = null;
            try
            {
                sql = $@"SELECT DISTINCT MAS_SERV_USED.SERVICE_ID as SER_ID , 
                MAS_SERV_USED.SERVICE_NAME as SER_NAME , MAS_SERV_USED.IS_ACTIVE as IS_ACTIVE , MAS_SERV_USED.is_active as active FROM  MAS_SERV_USED ORDER BY SERVICE_ID ASC";
                dt1 = _Connection.Query5(sql, connectionstring);
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

        [HttpGet]
        public string setcboStatus( string connectionstring = "")
        {
            string sql = string.Empty;
            DataTable dt = null;
            string json = string.Empty;
            try
            {
                sql = "SELECT RES_CODE , RES_NAME FROM MAS_REASON WHERE RES_STATUS = '1' ORDER BY RES_CODE ASC ";
                dt = _Connection.Query6(sql, connectionstring);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        {
                          return  json = JsonConvert.SerializeObject(dt);
                        }
                    }
                }
                return json;
            }
            catch (Exception ex)
            {

                return "";
            }

        }

        [HttpGet]
        public string FrmStatus_Load( string Agen = "", string connectionstring = "", string Tel = "")
        {
            string json = null;
            try
            {
            }
            catch (Exception ex)
            {
                //WriteLog.instance./*Log*/("Error ที่ FrmStatus_Load : " + ex.Message.ToString());
                //Module2.Agent_Id = "";
            }


            json = Get_Project(Agen, connectionstring, Tel);


            return json;
        }
        public string checkTelphone(DataTable dt2)
        {
            string Phone = "";
            if (dt2.Rows[0]["DNIS"] != null && dt2.Rows[0]["DNIS"].ToString().Length > 1)
            {
                Phone = "0" + dt2.Rows[0]["DNIS"].ToString();

                return Phone;
            }
            else
            {
                Phone = "''";
                return Phone;
            }
        }
        public string Get_Project(string Agen, string connectionstring = "", string Tel = "")
        {
            string Status = "";
            string Phone = "";
            string SQL = "";
            try
            {

                SQL = "select CNFG_STATUS_CODE.DESCRIPTION  as DESCRIPTION , CNFG_AGENT_INFO.DNIS as DNIS from CNFG_AGENT_INFO,CNFG_STATUS_CODE  where AGENT_ID = '"+ Agen + "' AND CNFG_AGENT_INFO.LOGON_EXT= CNFG_STATUS_CODE.STATUS_ID AND ROWNUM = 1";
                DataTable dt2 = null;
               dt2 = _Connection.Query7(SQL, connectionstring);
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
                    else if (Tel != "" && Tel != "null")
                    {

                        Status = "Busy";

                    }


                    if (Status == "Busy")
                    {
                        Phone = checkTelphone(dt2);
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

        [HttpGet]
        public ActionResult FrmReport(string usernane = "")
        {
            List<string> values1 = new List<string>()
                        {
                            usernane
                        };
            return View(values1);
        }

        [HttpGet]
        public string SingOut( string connectionstring = "", string Agen = "")
        {
            string strUpdate = string.Empty;
            string message = string.Empty;
            DataTable dt = null;
            try
            {
                if (Agen != "")
                {
                    strUpdate = "";
                    strUpdate = "UPDATE CNFG_AGENT_INFO SET STATUS_ID = '0' ,";
                    strUpdate += " TERMINAL_IP  = '',";
                    strUpdate += " CALL_COUNT  = 0,";
                    strUpdate += " LOGON_EXT  = 0,";
                    strUpdate += "DNIS = '' ,";
                    strUpdate += " LOGIN_TIME   = ''";
                    strUpdate += " WHERE  AGENT_ID = '" + Agen + "'";
                    try
                    {
                        {
                            _Connection.Execute3(strUpdate, connectionstring);
                            message = "200";

                        }
                    }
                    catch (Exception ex)
                    {
                        message = "server มี ปัญหา";
                    }
                    finally
                    {
                    }
                }
                return message;
            }
            catch
            {
                return "";
            }

        }

        [HttpPost]
        public string btnReport_Click(form3 telclass)
        {
            return showreport(telclass);
        }
        public string showreport(form3 telclass)
        {
            string sql2 = string.Empty;
            try
            {
                DataTable dt3 = new DataTable();
                string Agens = telclass.Agen;
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
                    sql2 += " where   MAS_LEADS_TRANS.AGENT_ID = '" + Agens + "' AND MAS_LEADS_TRANS.RES_CODE = '" + telclass.res_code + "'";
                    if (telclass.Day != null)
                    {
                        string dd = telclass.Day.Split('/')[0];
                        string mm = telclass.Day.Split('/')[1];
                        string yy = (Convert.ToInt32(telclass.Day.Split('/')[2]) - 543).ToString().Substring(2, 2);
                        sql2 += " And to_date(LEAD_CALL_DATE,'YYYY/MM/DD') = to_date('" + dd + "/" + mm + "/" + yy + "','YYYY/MM/DD')";
                    }
                    else
                    {
                        string dd = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[0];
                        string mm = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[1];
                        string yy = DateTime.Now.ToString("dd-MMM-yy", new CultureInfo("en-US")).Split('-')[2];
                        sql2 += " And to_date(LEAD_CALL_DATE,'YYYY/MM/DD') = to_date('" + dd + "/" + mm + "/" + yy + "','YYYY/MM/DD')";
                    }
                }
                catch
                {
                }

                try
                {
                    Thread.Sleep(1000);
                    dt3 = _Connection.Query8(sql2, telclass.connectionstring);
                    if (dt3 != null)
                    {
                        if (dt3.Rows != null)
                        {
                            if (dt3.Rows.Count > 0)
                            {
                                if (telclass.res_code == "01")
                                {
                                    List<string> list = new List<string>();
                                    DataTable dt2 = Service_Sum2(telclass);
                                    string data01 = JsonConvert.SerializeObject(dt2);
                                    string data02 = JsonConvert.SerializeObject(dt3);
                                    list.Add(data01);
                                    list.Add(data02);
                                    list.Add(list_Service( telclass.connectionstring));
                                    return JsonConvert.SerializeObject(list);
                                }
                                else
                                {
                                    List<string> list = new List<string>();
                                    DataTable dt2 = Service_Sum3(telclass);
                                    string data01 = JsonConvert.SerializeObject(dt2);
                                    string data02 = JsonConvert.SerializeObject(dt3);
                                    list.Add(data01);
                                    list.Add(data02);
                                    list.Add(list_Service( telclass.connectionstring));
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
                    //WriteLog.instance.Log("ไม่สามารถแสดงข้อมูลได้ เนื่องจาก" + ex.Message.ToString() + "ข้อผิดพลาด");
                    //WriteLog.instance.Log("showreport :" + sql2);
                    return ("ไม่สามารถแสดงข้อมูลได้ เนื่องจาก" + ex.Message.ToString() + "ข้อผิดพลาด");
                }
            }
            catch (Exception ex)
            {
                //WriteLog.instance.Log("showreport :" + sql2);
                return ("ไม่สามารถแสดงข้อมูลได้ เนื่องจาก" + ex.Message.ToString() + "ข้อผิดพลาด");
            }
        }

        [HttpGet]
        public string cboStatus_SelectedIndexChanged(string cboStatus, string res_code, string connectionstring = "")
        {
            List<string> list = new List<string>();
            string json = string.Empty;
            try
            {
                if (cboStatus == "03")
                {
                    json = setcboDeny(res_code.ToString(), connectionstring);
                }
                else if (cboStatus == "15")
                {
                    list.Add("เครือข่าย");
                    list.Add("DTAC");
                    list.Add("TRUE");
                    list.Add("AIS");
                    list.Add("ไม่ระบุ");
                }
                else
                {
                }
                if (json == null || json == "")
                {
                    return JsonConvert.SerializeObject(list);
                }
                return json;
            }
            catch
            {
                return "";
            }
            finally
            {
            }
        }
        private string setcboDeny(string res_code, string connectionstring = "")
        {
            DataTable dt = null;
            try
            {
                string sql = "SELECT DENY_CODE , DENY_NAME  FROM MAS_RESON_DENY WHERE RES_CODE = '"+ res_code + "'"; // WHERE RES_STATUS = '0' "
                dt = _Connection.Query (sql, connectionstring);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                //WriteLog.instance.Log("Error ที่ : setcboDeny" + ex.Message.ToString()); 
                return null;
            }
        }
    }
    public class form3
    {

        public string Date_thai { get; set; }
        public string agent_id { get; set; }
        public string Day { get; set; }
        public string connectionstring { get; set; } = "";
        public string id { get; set; }
        public string Agen { get; set; }
        public string txtTel_No { get; set; }
        public string txtOper { get; set; }
        public string txtDate_Tel { get; set; }
        public string cboStatus { get; set; }
        public string cboDeny { get; set; }
        public bool btnEdit { get; set; }
        public bool btnSave { get; set; }
        public bool Button1 { get; set; }
        public string cboSex { get; set; }
        public string txtYear { get; set; }
        public string cboDate { get; set; }
        public string txtName { get; set; }
        public string txtSName { get; set; }
        public string cboDate_No { get; set; }
        public string cboMouth { get; set; }
        public bool SERVICE_0 { get; set; }
        public bool SERVICE_01 { get; set; }
        public bool SERVICE_02 { get; set; }
        public bool SERVICE_03 { get; set; }
        public bool SERVICE_04 { get; set; }
        public bool SERVICE_05 { get; set; }
        public bool SERVICE_06 { get; set; }
        public bool SERVICE_07 { get; set; }
        public bool SERVICE_08 { get; set; }
        public bool SERVICE_09 { get; set; }
        public bool SERVICE_10 { get; set; }
        public bool SERVICE_11 { get; set; }
        public bool SERVICE_12 { get; set; }
        public bool SERVICE_13 { get; set; }
        public bool SERVICE_14 { get; set; }
        public bool SERVICE_15 { get; set; }
        public bool SERVICE_16 { get; set; }
        public bool SERVICE_17 { get; set; }
        public bool SERVICE_18 { get; set; }
        public bool SERVICE_19 { get; set; }
        public bool SERVICE_20 { get; set; }
        public bool SERVICE_21 { get; set; }
        public bool SERVICE_22 { get; set; }
        public bool SERVICE_23 { get; set; }
        public bool SERVICE_24 { get; set; }
        public bool SERVICE_25 { get; set; }
        public bool SERVICE_26 { get; set; }
        public bool SERVICE_27 { get; set; }
        public bool SERVICE_28 { get; set; }
        public bool SERVICE_29 { get; set; }
        public bool SERVICE_30 { get; set; }
        public bool SERVICE_31 { get; set; }
        public bool SERVICE_32 { get; set; }
        public bool SERVICE_33 { get; set; }
        public bool SERVICE_34 { get; set; }
        public bool SERVICE_35 { get; set; }
        public bool SERVICE_36 { get; set; }
        public bool SERVICE_37 { get; set; }
        public bool SERVICE_38 { get; set; }
        public bool SERVICE_39 { get; set; }
        public string strDeny { get; set; }
        public string cbocity { get; set; }
        public string cbocity_name { get; set; }
        public string statusfrm { get; set; }
        public string status2 { get; set; }
        public string statustel { get; set; }
        public string res_code { get; set; }
        public string VISIBLE { get; set; }
        public string strDenycode { get; set; }
    }
}
