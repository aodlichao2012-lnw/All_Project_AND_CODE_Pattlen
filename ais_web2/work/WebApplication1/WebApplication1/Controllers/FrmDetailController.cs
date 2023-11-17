using Jose;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using Model_Helper;
using WebApplication1.Models;
namespace ais_web3.Controllers
{
    [OutputCache(Duration = 3600)]
    public class FrmDetailController : Controller
    {
        string session_ID = string.Empty;
        private Module2 module;
        string type_db = string.Empty;
        string user_name = string.Empty;
        public FrmDetailController()
        {



            //WriteLog.instance.Log_browser_Detail_page("FrmDetail/Index");

        }
        public Dictionary<string, string> keyValuePairs;
        public delegate void updatestatus(string strstat);
        private DataSet ds = new DataSet();
        private DataTable dt2 = new DataTable();
        private string sql;
        private int i;
        private OracleDataAdapter da;
        private OracleCommand cmd;
        private string sqlsearch = "";
        private string strUpdate = "";
        private string service = "";
        private string strRec_Code = "";
        private string strDeny_Code = "";
        private string strDeny = "";
        private DateTime myDate;
        private string mystr, myMonth;
        private string num;
        private int num_year = 0;
        private int num_month;
        private string showDay = "";
        private Process myProcess;
        private string strData, pAni1, sLang1; // ...AVAYA CTI
        private string Anumber = "";
        static string tel_phone;
        private   string setcboDeny(string res_code ,string id =""  , string connectionstring ="")
        {
            module = new Module2(id , connectionstring);
            DataTable dt = null;
            try
            {
                string sql = "SELECT DENY_CODE , DENY_NAME  FROM MAS_RESON_DENY WHERE RES_CODE = :RES_CODE "; // WHERE RES_STATUS = '0' "
               module = new Module2(id,connectionstring);
                module.Common_static_reson(sql, new string[] { res_code }, new string[] { ":RES_CODE" }, ref dt);
               return JsonConvert.SerializeObject(dt);
            }
            catch(Exception ex)
            {
                //WriteLog.instance.Log("Error ที่ : setcboDeny" + ex.Message.ToString()); 
                return null;
            }
        }
        public string showdata2(Telclass2 telclass2)
        {
            DataTable dt = null;
            try
            {
                if (HttpContext.Request.Cookies["Agen" + session_ID] != null)
                {
                    string Agens = HttpContext.Request.Cookies["Agen" + session_ID].Value;
                    Module2.Agent_Id = Agens;
                }
                string sqlselect = "";
                form1 form = new form1();
                sqlselect = "SELECT * FROM MAS_LEADS_TRANS WHERE ANUMBER = :ANUMBER AND AGENT_ID = :AGENT_ID";
               module = new Module2(session_ID, form.  connectionstring);
                module.Comman_Static(sqlselect, new string[] { Module2.Agent_Id, telclass2.anumber }, new string[] { ":ANUMBER", ":AGENT_ID" }, ref dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RES_CODE"].ToString() == "01")
                    {
                    }
                    else if (dt.Rows[0]["RES_CODE"].ToString() == "02")
                    {
                    }
                    else if (dt.Rows[0]["RES_CODE"].ToString() == "03")
                    {
                    }
                    else if (dt.Rows[0]["RES_CODE"].ToString() == "04")
                    {
                    }
                    else if (dt.Rows[0]["RES_CODE"].ToString() == "05")
                    {
                    }
                    else if (dt.Rows[0]["RES_CODE"].ToString() == "06")
                    {
                    }
                    else if (dt.Rows[0]["RES_CODE"].ToString() == "07")
                    {
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["DENY_CODE"].ToString()))
                    {
                    }
                    else if (string.IsNullOrEmpty(dt.Rows[0]["DENY_CODE"].ToString()))
                    {
                        form.cboDeny = "";
                    }
                    if (dt.Rows[0]["CUST_SEX"].ToString() == "M")
                    {
                        form.cboSex = "ชาย";
                    }
                    else if (dt.Rows[0]["CUST_SEX"].ToString() == "F")
                    {
                        form.cboSex = "หญิง";
                    }
                    else if (dt.Rows[0]["CUST_SEX"].ToString() == " ")
                    {
                        form.cboSex = "";
                    }
                    if (dt.Rows[0]["BIRTH_DAY"].ToString() == "1")
                    {
                        form.cboDate = "อาทิตย์";
                    }
                    else if (dt.Rows[0]["BIRTH_DAY"].ToString() == "2")
                    {
                        form.cboDate = "จันทร์";
                    }
                    else if (dt.Rows[0]["BIRTH_DAY"].ToString() == "3")
                    {
                        form.cboDate = "อังคาร";
                    }
                    else if (dt.Rows[0]["BIRTH_DAY"].ToString() == "4")
                    {
                        form.cboDate = "พุธ";
                    }
                    else if (dt.Rows[0]["BIRTH_DAY"].ToString() == "5")
                    {
                        form.cboDate = "พฤหัสบดี";
                    }
                    else if (dt.Rows[0]["BIRTH_DAY"].ToString() == "6")
                    {
                        form.cboDate = "ศุกร์";
                    }
                    else if (dt.Rows[0]["BIRTH_DAY"].ToString() == "7")
                    {
                        form.cboDate = "เสาร์";
                    }
                    else if (dt.Rows[0]["BIRTH_DAY"].ToString() == "0")
                    {
                        form.cboDate = "";
                    }
                    if (dt.Rows[0][" Birth_MM"].ToString() == "01")
                    {
                        form.cboMouth = "มกราคม";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "02")
                    {
                        form.cboMouth = "กุมภาพันธ์";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "03")
                    {
                        form.cboMouth = "มีนาคม";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "04")
                    {
                        form.cboMouth = "เมษายน";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "05")
                    {
                        form.cboMouth = "พฤษภาคม";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "06")
                    {
                        form.cboMouth = "มิถุนายน";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "07")
                    {
                        form.cboMouth = "กรกฏาคม";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "08")
                    {
                        form.cboMouth = "สิงหาคม";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "09")
                    {
                        form.cboMouth = "กันยายน";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "10")
                    {
                        form.cboMouth = "ตุลาคม";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "11")
                    {
                        form.cboMouth = "พฤศจิกายน";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "12")
                    {
                        form.cboMouth = "ธันวาคม";
                    }
                    else if (dt.Rows[0][" Birth_MM"].ToString() == "00")
                    {
                        form.cboMouth = "";
                    }
                    form.txtYear = dt.Rows[0]["BIRTH_YYYY"].ToString();
                    if (dt.Rows[0]["SERVICE_01"].ToString() == "1")
                    {
                        form.SERVICE_1 = true;
                        form.SERVICE_8 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_01"].ToString() == "0")
                    {
                        form.SERVICE_1 = false;
                        form.SERVICE_8 = false;
                    }

                    if (dt.Rows[0]["SERVICE_02"].ToString() == "1")
                    {
                        form.SERVICE_2 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_02"].ToString() == "0")
                    {
                        form.SERVICE_2 = false;
                    }
                    if (dt.Rows[0]["SERVICE_03"].ToString() == "1")
                    {
                        form.SERVICE_3 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_03"].ToString() == "0")
                    {
                        form.SERVICE_3 = false;
                    }
                    if (dt.Rows[0]["SERVICE_04"].ToString() == "1")
                    {
                        form.SERVICE_4 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_04"].ToString() == "0")
                    {
                        form.SERVICE_4 = false;
                    }
                    if (dt.Rows[0]["SERVICE_05"].ToString() == "1")
                    {
                        form.SERVICE_5 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_05"].ToString() == "0")
                    {
                        form.SERVICE_5 = false;
                    }
                    if (dt.Rows[0]["SERVICE_06"].ToString() == "1")
                    {
                        form.SERVICE_6 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_06"].ToString() == "0")
                    {
                        form.SERVICE_6 = false;
                    }
                    if (dt.Rows[0]["SERVICE_07"].ToString() == "1")
                    {
                        form.SERVICE_7 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_07"].ToString() == "0")
                    {
                        form.SERVICE_7 = false;
                    }
                    if (dt.Rows[0]["SERVICE_08"].ToString() == "1")
                    {
                        form.SERVICE_8 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_08"].ToString() == "0")
                    {
                        form.SERVICE_8 = false;
                    }
                    if (dt.Rows[0]["SERVICE_09"].ToString() == "1")
                    {
                        form.SERVICE_9 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_09"].ToString() == "0")
                    {
                        form.SERVICE_9 = false;
                    }
                    if (dt.Rows[0]["SERVICE_10"].ToString() == "1")
                    {
                        form.SERVICE_10 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_10"].ToString() == "0")
                    {
                        form.SERVICE_10 = false;
                    }
                    if (dt.Rows[0]["SERVICE_11"].ToString() == "1")
                    {
                        form.SERVICE_11 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_11"].ToString() == "0")
                    {
                        form.SERVICE_11 = false;
                    }
                    if (dt.Rows[0]["SERVICE_12"].ToString() == "1")
                    {
                        form.SERVICE_12 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_12"].ToString() == "0")
                    {
                        form.SERVICE_12 = false;
                    }
                    if (dt.Rows[0]["SERVICE_13"].ToString() == "1")
                    {
                        form.SERVICE_13 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_13"].ToString() == "0")
                    {
                        form.SERVICE_13 = false;
                    }
                    if (dt.Rows[0]["SERVICE_14"].ToString() == "1")
                    {
                        form.SERVICE_14 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_14"].ToString() == "0")
                    {
                        form.SERVICE_14 = false;
                    }
                    if (dt.Rows[0]["SERVICE_16"].ToString() == "1")
                    {
                        form.SERVICE_16 = true;
                    }
                    else if (dt.Rows[0]["SERVICE_16"].ToString() == "0")
                    {
                        form.SERVICE_16 = false;
                    }
                    if (string.IsNullOrEmpty(dt.Rows[0]["CITY_NAME_T"].ToString()))
                    {
                        form.cbocity = "";
                    }
                    else if (!string.IsNullOrEmpty(dt.Rows[0]["CITY_NAME_T"].ToString()))
                    {
                        form.cbocity = dt.Rows[0]["CITY_NAME_T"].ToString();
                    }
                    form.statustel = telclass2.status;
                    return JsonConvert.SerializeObject(form);
                }
                return JsonConvert.SerializeObject(null);
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
        public string cboStatus_SelectedIndexChanged(string cboStatus, string res_code ,string id ="" , string connectionstring ="")
        {
            List<string> list = new List<string>();
            string json = string.Empty;
            try
            {
                    if (cboStatus == "03")
                    {
                        json = setcboDeny(res_code.ToString(), id);
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
        [HttpGet]
        public string showCity(string id ="" , string connectionstring ="")
        {
            string searchcity = string.Empty ;
            try
            {
                DataTable dt = null;
                string json = string.Empty;
                searchcity = " SELECT CITY_CODE , CITY_NAME_T FROM CALL_SEARCH_CITY ORDER BY CITY_NAME_T ASC";
               module = new Module2(id,connectionstring);
                module.Comman_Static(searchcity , null, null, ref dt);
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
                //WriteLog.instance.Log("Error ที่ showCity : "+ ex.Message.ToString());
                //WriteLog.instance.Log("Error ที่ showCity : "+ searchcity);
                return "";
            }
            finally
            {
            }
        }
        [HttpGet]
        public string setcboStatus(string id ="" , string connectionstring ="")
        {
            string sql = string.Empty;
            DataTable dt = null;
            string json = string.Empty;
            try
            {
                sql = "SELECT RES_CODE , RES_NAME FROM MAS_REASON WHERE RES_STATUS = '1' ORDER BY RES_CODE ASC ";
               module = new Module2(id,connectionstring);
                module.Comman_Static(sql , null,null,ref dt);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            {
                                json  = JsonConvert.SerializeObject(dt);
                            }
                        }
                    }
                return json;
            }
            catch(Exception ex)
            {
                //WriteLog.instance.Log("Error ที่ setcboStatus : " + ex.Message.ToString());
                //WriteLog.instance.Log("Error ที่ setcboStatus : " + sql);
                return "";
            }
            finally
            {
            }
        }
        [HttpGet]
        public string SingOut(string id ,string connectionstring = "" ,string Agen ="")
        {
            session_ID = id;
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
                        strUpdate += " LOGIN_TIME   = ''";
                        strUpdate += " WHERE  AGENT_ID = '" + Agen + "'";
                        try
                        {
                            {
                           module = new Module2(id,connectionstring);
                            module.Comman_Static(strUpdate, null, null, ref dt);
                                message = "200";
                            //string[] list_cookie = HttpContext.Request.Cookies.AllKeys;
                            //foreach (string item in list_cookie)
                            //{
                            //if (item != null)
                            //    {
                            //        HttpContext.Response.Cookies[item].Expires = Convert.ToDateTime("2000/01/01 00:00:00");
                            //        HttpContext.Response.Cookies.Add(new HttpCookie(item));
                            //    }
                            //}
                            //HttpContext.Response.Cookies["strDB"+ id].Expires = Convert.ToDateTime("2000/01/01 00:00:00");
                            //HttpContext.Response.Cookies.Add(new HttpCookie("strDB"));  
                            //HttpContext.Response.Cookies["Agen"+ id].Expires = Convert.ToDateTime("2000/01/01 00:00:00");
                            //HttpContext.Response.Cookies.Add(new HttpCookie("strDB"));
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
            finally
            {
            }
        }
        private void btnSingout_Click(object sender, EventArgs e)
        {
        }
        [HttpPost]
        public string btnSave_Click(form3 form)
        {
            string sqlsearch = string.Empty;
            try
            {
                session_ID = form.id;
               module = new Module2(session_ID,form.connectionstring);
                int year2;
                string year3;
                int age;
                int rowInsert = 0;
                int day_no = 0;
                if (form.Agen != null)
                {
                    string Agens = form. Agen;
                    Module2.Agent_Id = Agens;
                }
                string Agenid = Module2.Agent_Id;
                if (form.txtYear == null)
                {
                    form.txtYear = string.Empty;
                }
                else
                {
                    if(form.txtYear != "-")
                    {
                        year2 = int.Parse(form.txtYear);
                        year3 = DateTime.Now.ToString("yyyy", new CultureInfo("th-TH"));
                        age = Convert.ToInt32(year3) - year2;
                    }
                    else
                    {
                        age = 0;
                    }
                    if (age < 21 & form.SERVICE_03 == true)
                    {
                        return "ไม่สามารถรับบริการ Lotto guru ได้  เพราะปีเกิดที่ระบุไม่อยู่ในช่วงที่กำหนด";
                    }
                    if (age < 15 && form.cboStatus == "01")
                    {
                        return "ลูกค้าอายุน้อยกว่า 15 ปี ไม่สามารถรับบริการได้ค่ะ";
                    }
                    if (age > 55 && form.cboStatus == "01")
                    {
                        return "ลูกค้าอายุมากกว่า 55 ปี ไม่สามารถรับบริการได้ค่ะ";
                    }

                }
                if (form.cboMouth == null)
                {
                    form.cboMouth = string.Empty;
                }
                if (form.cboDate == null)
                {
                    form.cboDate = string.Empty;
                }
                if (form.Date_thai == null)
                {
                    day_no = 0;
                }
          
                bool isMatch = System.Text.RegularExpressions.Regex.IsMatch(form.txtTel_No, @"^[0-9]");
                if (!isMatch)
                {
                    return "กรุณาพิมพ์ ตัวเลขเท่านั้น ห้ามพิมพ์ตัวอักษร และ ห้ามพิมพ์อักขระ";
                }
                Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
 
                if (form.cboStatus == "01")
                {
                    if (form.SERVICE_21 == false & form.SERVICE_11 == false && form.SERVICE_12 == false & form.SERVICE_13 == false)
                    {
                        return "กรุณาเลือกบริการค่ะ ";
                    }
                    if (form.txtName == "" | form.cbocity == "" | form.txtYear == "")
                    {
                        return "กรุณากรอกข้อมูลให้ครบค่ะ";
                    }
                    else if (form.SERVICE_02 == true & form.cboDate == "")
                    {
                        return "กรุณากรอกวันเกิดลูกค้าค่ะ";
                    }
                    else
                    {
                    }
                }
                else if (form.cboStatus == "03")
                {
                    if (form.strDenycode == null)
                    {
                        return "กรุณาเลือกเหตุผล";
                    }
                    if (form.SERVICE_21 == true || form.SERVICE_11 == true || form.SERVICE_12 == true || form.SERVICE_13 == true)
                    {
                        return "คุณเลือกไม่สนใจ โปรดยกเลิกการเลือกบริการ";
                    }
                }
                else
                {
                    if (form.SERVICE_21 == true || form.SERVICE_11 == true || form.SERVICE_12 == true || form.SERVICE_13 == true)
                    {
                        return "คุณเลือกไม่สนใจ โปรดยกเลิกการเลือกบริการ";
                    }
                }
                    sqlsearch = "";
                    sqlsearch = "INSERT INTO MAS_LEADS_TRANS(ANUMBER,LEAD_CALL_DATE,SERVICE_01,SERVICE_02,SERVICE_03,SERVICE_04,SERVICE_05,SERVICE_06,SERVICE_07,SERVICE_08,SERVICE_09,SERVICE_10,SERVICE_11,SERVICE_12,SERVICE_13,SERVICE_14,SERVICE_15,SERVICE_16,SERVICE_17,SERVICE_18,SERVICE_19,SERVICE_20,SERVICE_21,SERVICE_22,SERVICE_23,SERVICE_24,SERVICE_25,SERVICE_26,SERVICE_27,SERVICE_28,SERVICE_29,SERVICE_33,";
                    sqlsearch += "CUST_NAME,CUST_SNAME,BIRTH_DAY ,BIRTH_DD,Birth_MM,BIRTH_YYYY,CUST_SEX,RES_CODE,CITY_NAME_T,OPERATION,DENY_CODE,PREDICT_STATUS,AGENT_ID)VALUES(";
                    sqlsearch += " :txtTel_No ,";
                    sqlsearch += " sysdate,'";
                    // -----------------  for predictive AIS --------------------
                    if (form.SERVICE_01 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    }

                    if (form.SERVICE_02 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // service_02 
                    }                          // End If
                    if (form.SERVICE_03 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // service_03 
                    }
                    // End If
                    if (form.SERVICE_04 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // service_04 
                    }                    // End If
                    if (form.SERVICE_05 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    } // DTV News service_05
                    if (form.SERVICE_06 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    }// อ.ไพศาล service_06
                    if (form.SERVICE_07 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // Disney Horo service_07
                    }
                    if (form.SERVICE_08 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // อ.ลักษณ์ (ความรัก) service_08
                    }
                    if (form.SERVICE_09 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // อ.เก่งกาจ (ความรัก) service_09
                    }
                    if (form.SERVICE_10 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";   // Hot News service_10
                    }
                    if (form.SERVICE_11 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";   // Hot News service_11                         // End If
                    }
                    if (form.SERVICE_12 == true)
                    {
                        sqlsearch += "1" + "','"; // service_12 Service เสริมทรัพย์เสริมโชค
                    }
                    else if (form.SERVICE_12 == false)
                    {
                        sqlsearch += "0" + "','"; // service_12
                    }
                    if (form.SERVICE_13 == true)
                    {
                        sqlsearch += "1" + "','"; // service_13 VDO ดวง อ.เก่งกาจ
                    }
                    else if (form.SERVICE_13 == false)
                    {
                        sqlsearch += "0" + "','"; // service_13
                    }
                    if (form.SERVICE_14 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    }
                    if (form.SERVICE_15 == true) // Lotto guru  service_15
                    {
                        sqlsearch += "1" + "','";
                    }
                    else if (form.SERVICE_15 == false)
                    {
                        sqlsearch += "0" + "','";
                    }
                    if (form.SERVICE_16 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    }
                    if (form.SERVICE_17 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else if (form.SERVICE_17 == false)
                    {
                        sqlsearch += "0" + "','"; // ยิปซีพยากรณ์ service_17
                    }
                    if (form.SERVICE_18 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else if (form.SERVICE_19 == false)
                    {
                        sqlsearch += "0" + "','";  // เคล็ดลับเสริมโชค service_18
                    }
                    if (form.SERVICE_19 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else if (form.SERVICE_19 == false)
                    {
                        sqlsearch += "0" + "','"; // ข่าวเด่นสายตรง service_19
                    }
                    if (form.SERVICE_20 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // service_20       
                    }
                    if (form.SERVICE_21 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // service_21 Service เสริมทรัพย์เสริมโชค อ.เทพประสิทธิ์                       // End If
                    }
                    if (form.SERVICE_22 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; // service_22                
                    }
                    if (form.SERVICE_23 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    } // service_23
                    if (form.SERVICE_24 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    }// service_24
                    if (form.SERVICE_25 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; 
                    }
                    if (form.SERVICE_26 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','"; 
                    }
                    if (form.SERVICE_27 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    }
                    if (form.SERVICE_28 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    }
                    if (form.SERVICE_29 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "','";
                    }
                    if (form.SERVICE_33 == true)
                    {
                        sqlsearch += "1" + "','";
                    }
                    else
                    {
                        sqlsearch += "0" + "',";
                    }
                    sqlsearch += " :txtName , ";
                    sqlsearch += " :txtSName , ";
                    sqlsearch += " :cboDate_No , ";
                    sqlsearch += " :cboDate, ";
                    sqlsearch += " :cboMouth, ";
                    sqlsearch += " :txtYear, ";
                    sqlsearch += " :cboSex, ";
                    sqlsearch += " :cboStatus, ";
                    sqlsearch += " :cbocity, ";
                    if (form.cboStatus.ToString() == "15")
                    {
                        sqlsearch += "'Other', '";
                    }
                    else
                    {
                        sqlsearch += "'AIS" + "', '";
                    }
                    if (form.cboStatus.ToString() == "03" | form.cboStatus.ToString() == "08")
                    {
                        if (form.strDenycode != null)
                        {
                            sqlsearch += form.strDenycode.ToString() + "','";
                        }
                        else
                        {
                            return "กรุณาเลือกเหตุผล";
                        }
                    }
                    else
                    {
                        sqlsearch += "" + "','";
                    }
                    sqlsearch += "1" + "', '";
                    sqlsearch += Agenid + "')";
                try
                {
                    {
                           module = new Module2(session_ID,form.connectionstring);
                            rowInsert = module.CommanEx_Save(sqlsearch, new string[] { form.txtTel_No, form.txtName, form.txtSName, day_no.ToString(), form.cboDate, form.cboMouth, form.txtYear, form.cboSex, form.cboStatus.ToString().Replace(" ", ""), form.cbocity }, new string[] { ":txtTel_No", ":txtName", ":txtSName", ":cboDate_No", ":cboDate", ":cboMouth", ":txtYear", ":cboSex", ":cboStatus", ":cbocity" });
                            Module2.Instance.status_Edit = "";
                            Module2.Instance.cbocity = form.cbocity;
                            if (rowInsert == -1)
                            {
                                return "server มี ปัญหา";
                            }
                           module = new Module2(session_ID,form.connectionstring);
                          string status =  module.UpdateCNFG_Agent_Info("5", Module2.Agent_Id, form.txtTel_No);
                            if(status == "500")
                            {
                                return "server มี ปัญหา..";
                            }

                            Response.Cookies.Add(new HttpCookie("Isave" + session_ID, "save"));
                            if (HttpContext.Request.Cookies["Tel" + session_ID] != null && HttpContext.Request.Cookies["Tel" + session_ID].Expires != Convert.ToDateTime("2000/01/01 00:00:00"))
                            {
                                Response.Cookies.Add(new HttpCookie("Tel" + session_ID) {  Expires = Convert.ToDateTime("2000/01/01 00:00:00") });
                            }
                            return "บันทึกข้อมูลเรียบร้อย";
                      
                   
                    }
                }
                catch (Exception ex)
                {

                    return "ไม่สามารถบันทึกข้อมูลได้เนื่องจาก" + ex.Message;
                }
            }
            catch(Exception ex)
            {
                return "ไม่สามารถบันทึกได้เนื่องจาก " + ex.Message.ToString();
            }

        }

        [HttpGet]
        public ActionResult Index(string id = "" , string connectionstring = "")
        {
            string StrSql = string.Empty;
            int return1 = 0;
            try
            {
                if(id != "")
                {
                    session_ID = id;
                }
                else
                {
                    session_ID = HttpContext.Request.Cookies["id"].Value;
                }
              
                type_db = "";
                user_name = "";
                CultureInfo cultureInfo = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                DateTime datet = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), new CultureInfo("en-US"));
                string jwt = HttpContext.Request.Cookies["login" + session_ID].Value;
                if (jwt != "" && jwt != null)
                {
                    string keys = jwt;
                   module = new Module2(session_ID,connectionstring);
          
                    List<string> userjson = module.GetFromToken(keys);

                    if (datet <= Convert.ToDateTime(userjson[2].ToString()))
                    {
                        StrSql = "SELECT * FROM PREDIC_AGENTS";
                        StrSql += " WHERE ROWNUM = 1 AND (LOGIN = :userjson )";
                       module = new Module2(session_ID,connectionstring);
                        DataSet ds = module.CommandSet(StrSql, "Login_agent", new string[] { userjson[0] }, new string[] { "userjson" });
                        if (ds != null)
                        {
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
                                    Response.Cookies.Add(new HttpCookie("Agen", Module2.Agent_Id));
                                    Response.Cookies.Add(new HttpCookie("id", session_ID));
                                    Response.Cookies.Add(new HttpCookie("EXTENSION" + session_ID, Module2.EXTENSION));
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
                       
                        return  View();
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
                //WriteLog.instance.Log("Index FrmDetail :" + ex.Message.ToString());
                //WriteLog.instance.Log("Index FrmDetail :" + StrSql);
                return RedirectToAction("Index", "FrmDetail");
            }
            finally
            {
            }
        }
        [HttpGet]
        public string list_Service2(string id ="" , string connectionstring ="")
        {
            string sql = string.Empty;
            DataTable dt1 = null;
            try
            {
                sql = $@"SELECT DISTINCT MAS_SERV_USED.SERVICE_ID as SER_ID , 
                MAS_SERV_USED.SERVICE_NAME as SER_NAME , MAS_SERV_USED.IS_ACTIVE as IS_ACTIVE , MAS_SERV_USED.is_active as active FROM  MAS_SERV_USED WHERE MAS_SERV_USED.is_active = '1'";
                session_ID = id;
               module = new Module2(session_ID,connectionstring);
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
        public string SetVisible(form2 form)
        {
            string sql = string.Empty;
            try
            {
                DataTable dt = null;
              string new_unvisible = string.Empty;
                //string[] unvisible1 = dt2.Rows[0][0].ToString().Split(',');
                string[] unvisible2 = form.VISIBLE.Replace("'", "").Replace("addSERVICE_", "").Split(',');
                int len = unvisible2.Length - 1;
                foreach (string item in unvisible2)
                {
                    new_unvisible += "'" + item + "',";
                }
                new_unvisible = new_unvisible.Remove(new_unvisible.Length - 4, 4);
                 sql = $@"UPDATE MAS_SERVICE SET IS_ACTIVE = '0' , MDF_DATE = '{DateTime.Now.ToString("dd-MMM-yy")}' WHERE  SERVICE_ID IN ({new_unvisible})";
               module = new Module2(session_ID,form.connectionstring);
                module.Comman_Static(sql , null,null,ref dt);
                if (dt.Rows.Count == 0)
                {
                    return "บันทึกสำเร็จ";
                }
            }
            catch(Exception ex)
            {
                //WriteLog.instance.Log("SetVisible :" + ex.Message.ToString());
                //WriteLog.instance.Log("SetVisible :" + sql);
                return "บันทึกไม่สำเร็จ";
            }
            return "กรุณาเลือกข้อมูล";
        }
        [HttpGet]
        public string SetVisible_remove(string txt_tel, string connectionstring = "")
        {
            DataTable dt1 = null;
            string sql = string.Empty;
            sql = $@"SELECT DISTINCT MAS_SERVICE.SERVICE_ID as SER_ID , 
                MAS_SERVICE.SERVICE_NAME as SER_NAME , CASE WHEN MAS_SERVICE.IS_ACTIVE = '1' THEN 'เปิดให้ใช้บริการ' ELSE 'ปิดการใช้บริการ' END as IS_ACTIVE FROM MAS_SERVICE ORDER BY IS_ACTIVE DESC";
           module = new Module2(session_ID,connectionstring);
            dt1 = module.Comman_Static_All(sql);
            return JsonConvert.SerializeObject(dt1);
        }
        public string list_Service(string id = "", string connectionstring = "")
        {
            session_ID = id;
            string sql = string.Empty;
            DataTable dt1 = null;
            try
            {
                sql = $@"SELECT DISTINCT MAS_SERV_USED.SERVICE_ID as SER_ID , 
                MAS_SERV_USED.SERVICE_NAME as SER_NAME , MAS_SERV_USED.IS_ACTIVE as IS_ACTIVE , MAS_SERV_USED.is_active as active FROM  MAS_SERV_USED ORDER BY SERVICE_ID ASC";
               module = new Module2(session_ID,connectionstring);
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
        [HttpGet]
        public string showreportToday(string id = "", string connectionstring = "")
        {
            session_ID = id;
            string Agens = string.Empty;
            if (HttpContext.Request.Cookies["Agen" + session_ID] != null)
            {
                Agens = HttpContext.Request.Cookies["Agen" + session_ID].Value;
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
           module = new Module2(session_ID,connectionstring);
            DataTable dt3 = module.Comman_Static2(sql2);
            List<string> json_list = new List<string>();
            List<string> list = new List<string>();
           module = new Module2(session_ID,connectionstring);
            DataTable dt2 = module.Service_Sum(new Telclass() { res_code = "01", agent_id = Agens });
            string data01 = JsonConvert.SerializeObject(dt2);
            string data02 = JsonConvert.SerializeObject(dt3);
            list.Add(data01);
            list.Add(data02);
            list.Add(list_Service(session_ID));
            return JsonConvert.SerializeObject(list);
        }
        public void updateUI(string strStat)
        {
            if (strStat.Length == 10)
            {
                tel_phone = strStat;
            }
            else if (strStat.Length == 9)
            {
                tel_phone = "0" + strStat;
            }
        }
        [HttpGet]
        public string Save_service(string id, string values,string connectionstring ="")
        {
            string sql = string.Empty;
            try
            {
                id = id.Replace("editSERVICE_", "");

                DataTable dataTable = null;
                sql = $@"UPDATE MAS_SERVICE SET  SERVICE_NAME = :values2 , MDF_DATE = :Date1 WHERE SERVICE_ID = :id2 ";
               module = new Module2(session_ID,connectionstring);
                module.Comman_Static(sql, new string[] { values, DateTime.Now.ToString("dd-MMM-yy"), id }, new string[] { "values2", "Date1", "id2", } , ref dataTable);
                if (dataTable.Rows.Count == 0)
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
                //WriteLog.instance.Log("Save_service :" + ex.Message.ToString());
                //WriteLog.instance.Log("Save_service :" + sql);
                return "บันทึกไม่สำเร็จ เนื่องจาก " + ex.Message.ToString();
            }
        }
        [HttpGet]
        public string Clear_edit(string id ="" , string connectionstring ="")
        {
            //session_ID = id;
            //if (HttpContext.Response.Cookies["editv" + session_ID] != null)
            //{
            //    HttpContext.Response.Cookies["editv" + session_ID].Expires = Convert.ToDateTime("2000/01/01 00:00:00");
            //    HttpContext.Request.Cookies["editv" + session_ID].Expires = Convert.ToDateTime("2000/01/01 00:00:00");
            //}
            //if (HttpContext.Request.Cookies["Tel" + session_ID] != null && HttpContext.Request.Cookies["Tel"+ session_ID].Expires != Convert.ToDateTime("2000/01/01 00:00:00"))
            //{
            //    HttpContext.Response.Cookies["Tel" + session_ID].Expires = Convert.ToDateTime("2000/01/01 00:00:00");
            //}
            return "";
        }
    }
}