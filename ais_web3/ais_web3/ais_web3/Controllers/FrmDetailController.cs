using ais_web3.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ais_web3.Controllers
{
    public class FrmDetailController : Controller
    {
        Db_connection _Connection = new Db_connection();
        // GET: FrmDetail
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string btnSave_Click(form4 form)
        {
            string sqlsearch = string.Empty;
            try
            {
                string Agens = form.Agen;
                int year2;
                string year3;
                int age;
                int rowInsert = 0;
                int day_no = 0;

                //string Agenid = Module2.Agent_Id;
                if (form.txtYear == null)
                {
                    form.txtYear = string.Empty;
                }
                else
                {
                    if (form.txtYear != "-")
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
                sqlsearch += " '" + form.txtTel_No + "' ,";
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
                sqlsearch += "'" + form.txtName + "' , ";
                sqlsearch += "'" + form.txtSName + "' , ";
                sqlsearch += "'" + form.cboDate_No + "' , ";
                sqlsearch += "'" + form.cboDate + "' , ";
                sqlsearch += "'" + form.cboMouth + "' , ";
                sqlsearch += "'" + form.txtYear + "' , ";
                sqlsearch += "'" + form.cboSex + "' , ";
                sqlsearch += "'" + form.cboStatus.Replace(" ", "") + "' , ";
                sqlsearch += "'" + form.cbocity + "' , ";
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
                sqlsearch += Agens + "')";
                try
                {
                    {
                        rowInsert = _Connection.Execute4(sqlsearch, form.connectionstring);

                        if (rowInsert == -1)
                        {
                            return "server มี ปัญหา";
                        }
                        string status = UpdateCNFG_Agent_Info("5", Agens, form.txtTel_No, form.connectionstring);
                        if (status == "500")
                        {
                            return "server มี ปัญหา..";
                        }

                        return "บันทึกข้อมูลเรียบร้อย";


                    }
                }
                catch (Exception ex)
                {

                    return "ไม่สามารถบันทึกข้อมูลได้เนื่องจาก" + ex.Message;
                }
            }
            catch (Exception ex)
            {
                return "ไม่สามารถบันทึกได้เนื่องจาก " + ex.Message.ToString();
            }

        }
        public string UpdateCNFG_Agent_Info(string status, string Agen = "", string DNIS = ""  ,string Connectionstring = "")
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
                    _Connection.Execute5(strUpdate , Connectionstring);
                    return "200";
                }
            }
            catch (Exception ex)
            {
                return "500";
            }
        }
    }
    public class form4
    {

        public string Date_thai { get; set; }
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