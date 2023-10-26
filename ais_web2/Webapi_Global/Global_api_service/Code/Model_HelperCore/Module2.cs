using Dapper;
using Jose;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Model_HelperCore
{

        public class Module2
        {
        public readonly string  strDB;
        public readonly  string user_name;
        public readonly  string strConn;  
        public readonly  string type_db;
        MemoryCache cache = MemoryCache.Default;

        public static string strDB_;
        public static string user_name_;
        public static string strConn_;
        public static string type_db_;
        public Module2 Module2_;
       
        public Module2 Load()
        {
           
            //if (HttpContext.Current.Session["strDB_"] == null)
            //{
            //    HttpContext.Current.Session["strDB_"] = strDB_;
            //}
            //if (HttpContext.Current.Session["user_name_"] == null)
            //{
            //    HttpContext.Current.Session["user_name_"] = user_name_;
            //} 
            //if (HttpContext.Current.Session["strConn_"] == null)
            //{
            //    HttpContext.Current.Session["strConn_"] = strConn_;
            //} 
            //if (HttpContext.Current.Session["type_db_"] == null)
            //{
            //    HttpContext.Current.Session["type_db_"] = type_db_;
            //}
            return this;
        }
        public Module2()
            {
         
                if (type_db == null)
                {
                    type_db = type_db_;
                }
                if (user_name == null)
                {
                    user_name = user_name_;
                }
                if (strDB == null)
                {
                    strDB = strDB_;
                }
         
            if (type_db != null)
            {
                if (strDB == "Production")
                {
                    HttpContext.Current.Response.Cookies["strDB"].Value = strDB;
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
                else if (strDB == "Backup")
                {
                    HttpContext.Current.Response.Cookies["strDB"].Value = strDB;
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
            }
            else
            {

                // 'Dim Static_Cookie.strConnect As String = "Data Source=ORAIEC2;User ID=PREGOV;Password=RIUD6D;" 'base จริง
                // '  Dim Static_Cookie.strConnect As String = "Data Source=DTACIVR;User ID=YUMT;Password=PASSTB;"   '' Base สำรอง
                if (strDB == "Production")
                {
                    strConn = ConfigurationManager.AppSettings["pro"].ToString();
                    HttpContext.Current.Session["strcon"] = strConn;
                }
                else if (strDB == "Backup")
                {
                   strConn = ConfigurationManager.AppSettings["back"].ToString();
                    HttpContext.Current.Session["strcon"] = strConn;
                }
                else
                {
                    if (HttpContext.Current.Session["strcon"] != null)
                    {
                        strConn = HttpContext.Current.Session["strcon"].ToString();
                    }
                    else
                    {
                       strConn = ConfigurationManager.AppSettings["back"].ToString();
                        HttpContext.Current.Session["strcon"] = strConn;
                    }

                }
            }
        }
            public static Module2 Module2s = null;


            public static Module2 Instance
            {
                get
                {
                    if (Module2s == null)
                        Module2s = new Module2();
                    return Module2s;
                }
            }
            public static Dictionary<string, object> KeyValuePairs { get; set; }

            // Public Const Static_Cookie.strConnect As String = "Provider=OraOLEDB.Oracle;Data Source=ORAIEC2;User ID=PREGOV;Password=RIUD6D;" 'base จริง
            // Public Const Static_Cookie.strConnect As String = "Provider=OraOLEDB.Oracle;Data Source=ORAIEC2;User ID=PREGOW;Password=RIUD7D;"
            // Public Connect As New OracleClient.OracleConnection

            public static OracleConnection Connect;
            public static OracleConnection Connect1;

            public static OracleConnection Conn2;

            public OracleConnection ConnORAOCC;

            public OracleCommand com;

            public OracleCommand Cmd;
            public DataSet ds;
            public DataTable dt;
            private OracleTransaction transaction;

            public OracleDataAdapter da;
            public OracleDataReader dr;
            public string agent = "";
            // Public agent_id As String = ""
            public static string Agent_Id { get; set; }
            public static string EXTENSION { get; set; }
            public int Group_Id;
            public int Log_Ext;
            public string strUsername = "";
            public string strPassword = "";
            public string Phone_no = "";
            public string Lead_code = "";
            public string Lead_seq = "";
            public string leadcode;
            public string cbocity = "";
            public string statusedit;
            public string leadname = "";
            public string level_id = "";
            public string status2;
            public string namelead = ""; // เก็บชื่อ lead ของ frmshowtel
            public string statustel = ""; // เก็บสถานะ cbo ในฟอร์ม frmreporttel
            public string statusfrm = ""; // เก็บ status ว่ามาจากฟอร์มไหน
            public string Birth_day = "";
            public string Birth_MM = "";
            public string sex = "";
            public int year = 0;
            public string Type { get; set; }
            public string strOperr { get; set; }
            public string Log_Id { get; set; }
            public static string Agent_Ip { get; set; }
            public string Channel_No = "";
            public string status_Edit = "";
            public string status_CheckAux = "";
            public int Call_Count = 0;
            public Dictionary<string, object> keyValuePairs;
            public object Caching_set(string key, object o)
            {

                KeyValuePairs.Add(key, o);

                return o;
            }
            public object Caching_Get(string key)
            {
                object keyValues2 = KeyValuePairs[key];
                return keyValues2;
            }


            public string UpdateCNFG_Agent_Info(string status, string Agen = "", string IP = "")
            {
                if (Module2.Agent_Id == "" || Module2.Agent_Id == null)
                {
                    Module2.Agent_Id = Agen;
                }
                if (Module2.Agent_Ip == "" || Module2.Agent_Ip == "")
                {
                    Module2.Agent_Ip = IP;
                }
                string strUpdate;
                string strUpdate2 = "";
                strUpdate = "";
                strUpdate = "UPDATE CNFG_AGENT_INFO  ";
                //strUpdate = "UPDATE CNFG_AGENT_INFO  ";
                strUpdate += " SET  TERMINAL_IP   = '" + Module2.Agent_Ip + "' ,";
                strUpdate += " STATUS_ID = " + status + " ,";
                strUpdate += "CALL_COUNT = 0,";
                strUpdate += "LOGON_EXT = " + status + ",";
                strUpdate += " LOGIN_TIME   = sysdate ";
                strUpdate += " WHERE AGENT_ID = '" + Module2.Agent_Id + "'";

                try
                {
                    {
                        CommanEx(strUpdate);
                        return "200";
                    }
                }
                catch (Exception ex)
                {
                    return "ระบบมีปัญหา กรุณาติดต่อ Admin ค่ะ" + ex.Message + "ผลการตรวจสอบ";
                }
            }

            public void Connectdb()
            {
                try
                {

                    Connect = new OracleConnection(strConn);
                    {
                        Connect.Open();
                    }

                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ Connect : " + ex.Message.ToString());
                    WriteLog.instance.Log("ไม่สามารถบันทึกข้อมูลได้เนื่องจากปัญหาติดต่อฐานข้อมูล " + ex.Message + "ผลการตรวจสอบ");
                }
            }

            public void Connectdb3()
            {
                try
                {

                    Connect1 = new OracleConnection(strConn);
                    {
                        Connect1.Open();
                    }

                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ Connect3 : " + ex.Message.ToString());
                    WriteLog.instance.Log("ไม่สามารถบันทึกข้อมูลได้เนื่องจากปัญหาติดต่อฐานข้อมูล " + ex.Message + "ผลการตรวจสอบ");
                }
            }

            public void Common_static_reson(string sQL, string[] input, string[] parameter, ref DataTable dt)
            {
                WriteLog.instance.LogSql(sQL);
                DataTable dt2 = new DataTable();
                object connectionLock = new object();
                Connectdb();
                dt = Excutue_process_sql(sQL, input, parameter).Result;
            }
            public void Comman_Static(string sQL, string[] input, string[] parameter, ref DataTable dt)
            {

                try
                {
                    WriteLog.instance.LogSql(sQL);
                    DataTable dt2 = new DataTable();
                    object connectionLock = new object();
                    //Connectdb();
                    //while (Connect.State == ConnectionState.Closed)
                    //{
                    //    Connectdb();
                    //}
                    Connectdb();

                    dt = Excutue_process_sql(sQL, input, parameter).Result;

        
                 
                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ Comman_Static : " + ex.Message.ToString());

                }
                finally
                {

                }
            }


            private async Task<DataTable> Excutue_process_sql(string sQL, string[] input, string[] parameter)
            {
                Connectdb();
                string Paraname = string.Empty;
            DataTable dt1 =  Execute_Sql(sQL, input, parameter, Paraname);

                return dt1;
            }

            private DataTable Execute_Sql(string sQL, string[] input, string[] parameter, string Paraname)
            {

                DataTable dt2 = new DataTable();
                var paramList = new DynamicParameters();
 
                using (OracleConnection Connect = new OracleConnection(strConn))
                {

                    Connect.Open();
                    using (OracleTransaction transaction = Connect.BeginTransaction())
                    {
                        using (OracleCommand command = new OracleCommand(sQL, Connect))
                        {
                            WriteLog.instance.LogSql(Connect.ConnectionString);
                            if (input != null)
                            {
                                if (input.Length > 0)
                                {
                                    int i = 0;
                                    foreach (string s in parameter)
                                    {
                                        paramList.Add(s, input[i]);
                                        //command.Parameters.Add(s, input[i]);
                                        //parameterinput.Add(input[i]);
                                        //parametername.Add(s);
                                        i++;
                                    }
                                }
                            }

                            if (input != null)
                            {
                                if (input.Length == 2)
                                {
                                    if (Paraname.Split(',')[0] == ":UNVIST")
                                    {

                                        List<ViewModel> data = Connect.Query<ViewModel>(command.CommandText, paramList).ToList();
                                        dt2 = ToDataTable(data);
                                    }
                                    else
                                    {

                                        List<ViewModel> data = Connect.Query<ViewModel>(command.CommandText, paramList).ToList();
                                        dt2 = ToDataTable(data);
                                    }
                                }
                                else
                                {
                                    List<ViewModel> data = Connect.Query<ViewModel>(command.CommandText, paramList).ToList();
                                    dt2 = ToDataTable(data);

                                }

                            }
                            else
                            {
                                Task.Delay(2000);
                                List<ViewModel> data = Connect.Query<ViewModel>(command.CommandText).ToList();
                                dt2 = ToDataTable(data);

                            }
                        }

                        transaction.Commit();
                        transaction.Dispose();
                        Connect.Close();


                    }
                };
  

            return dt2;
        }


            public DataTable Comman_Static_All(string sQL)
            {
                try
                {
                    string Paraname = string.Empty;
                    OracleDataReader dr2;
                    DataTable dt = new DataTable();

                    Connectdb();


                    //// เปิดการเชื่อมต่อกับ Oracle
                    //Connect.Open();

                    using (OracleTransaction transaction = Connect.BeginTransaction())
                    {
                        OracleCommand command = new OracleCommand(sQL, Connect);
                        try
                        {
                            dr2 = command.ExecuteReader();
                            dt.Load(dr2);
                            command.Dispose();
                            transaction.Commit();
                            transaction.Dispose();
                            return dt;
                        }
                        catch
                        {
                            transaction.Commit();
                            transaction.Dispose();
                            return null;
                        }

                    }


                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ Comman_Static_All : " + ex.Message.ToString());
                    return null;
                }
                finally
                {



                    // ปิดการเชื่อมต่อเมื่อเสร็จสิ้น
                    Connect.Close();
                }
            }

            public void Comman_Static2(string sQL, string[] input, string[] parameter, ref DataTable datatable)
            {
                try
                {

                    string Paraname = string.Empty;
                    DataTable dt = new DataTable();
                    try
                    {

                        Connectdb3();

                        using (OracleConnection Connection = new OracleConnection(strConn))
                        {
                            Connection.Open();
                            using (OracleTransaction transaction = Connection.BeginTransaction())
                            {
                                OracleCommand command = new OracleCommand(sQL, Connection);
                                OracleParameter oracleParameter = null;
                                if (input != null)
                                {
                                    if (input.Length > 0)
                                    {
                                        int i = 0;
                                        foreach (string s in parameter)
                                        {
                                            WriteLog.instance.Log("Input :" + s);
                                            oracleParameter = new OracleParameter();
                                            oracleParameter.Value = input[i];
                                            oracleParameter.ParameterName = s;
                                            Paraname += s + ",";
                                            oracleParameter.DbType = DbType.String;
                                            oracleParameter.Direction = ParameterDirection.Input;

                                            command.Parameters.Add(oracleParameter);
                                            i++;
                                        }
                                    }
                                }

                                if (input != null)
                                {
                                    if (input.Length == 2)
                                    {
                                        if (Paraname.Split(',')[0] == ":UNVIST")
                                        {
                                            OracleDataReader sqls = command.ExecuteReader();
                                            //dr2 = new OracleDataAdapter(sqls, Conn2);
                                            int d = sqls.FieldCount;
                                            if (sqls.HasRows)
                                            {
                                                dt.Load(sqls);
                                            }

                                            command.Dispose();
                                        }
                                        else
                                        {
                                            OracleDataReader sqls = command.ExecuteReader();
                                            //dr2 = new OracleDataAdapter(sqls, Conn2);
                                            int d = sqls.FieldCount;
                                            if (sqls.HasRows)
                                            {
                                                dt.Load(sqls);
                                            }

                                            command.Dispose();
                                        }
                                    }
                                    else
                                    {
                                        OracleDataReader sqls = command.ExecuteReader();
                                        //dr2 = new OracleDataAdapter(sqls, Conn2);
                                        int d = sqls.FieldCount;
                                        if (sqls.HasRows)
                                        {
                                            dt.Load(sqls);
                                        }

                                        command.Dispose();
                                    }
                                }
                                else
                                {
                                    OracleDataReader sqls = command.ExecuteReader();
                                    int d = sqls.FieldCount;
                                    if (sqls.HasRows)
                                    {
                                        dt.Load(sqls);
                                    }


                                    command.Dispose();

                                }
                                transaction.Commit();
                            }

                            Connection.Close();
                        }

                        datatable = dt;

                    }
                    catch (Exception ex)
                    {
                        WriteLog.instance.Log("Error ที่ Comman_Static2 : " + ex.Message.ToString());
                    }


                }
                catch
                {

                }
                finally
                {

                }
            }



            public string CommanDataread(string textbox_search_number = "")
            {
                Thread thread = null;
                var paramList = new DynamicParameters();
                string sqlselect = "";
                List<Telclass2> telclassList = new List<Telclass2>();
                string strday = Strings.Format(Convert.ToDateTime(DateTime.Now).ToString("dd"));
                string strmm = Strings.Format(Convert.ToDateTime(DateTime.Now).ToString("MMM"));
                string stryy = Strings.Format(Convert.ToDateTime(DateTime.Now).ToString("yy"));
                try
                {


                    string Today = strday + "-" + strmm + "-" + stryy;
                    if (HttpContext.Current.Request.Cookies["Agen"] != null)
                    {
                        string Agens = JWT.Decode(HttpContext.Current.Request.Cookies["Agen"].Value).Split(':')[1].Split('}')[0].Replace(@"""", "");
                        Module2.Agent_Id = Agens;
                    }
                    var dt = new DataTable();

                    sqlselect = " select anumber as เบอร์โทรศัพท์ ,agent_id," + " lead_call_date,service_21,service_11," + " service_12,service_13,cust_name as ชื่อ,cust_sname as นามสกุล ,birth_day," + " birth_dd,birth_mm,birth_yyyy,cust_sex," + " decode(res_code,'01','สมัคร','02','ติดต่อกลับ','03','ไม่สนใจ','04','ไม่สามารถติดต่อได้ในขณะนี้','05','สายว่างไม่มีผู้รับ','06','ยังไม่เปิดใช้บริการ','07','ระงับบริการชั่วคราว' , '09' ,'สายไม่ว่าง' ,'11' ,'สายหลุด/เงียบ (ก่อนสนทนา)' , '12' , 'สายหลุด (ขณะสนทนา)' , '13' ,'สัญญาณเงียบ (ขณะสนทนา)' , '14','สัญญาณมีปัญหา (เสียงซ่าส์ ตี๊ดๆ)' , '15' ,'เครือข่ายอื่นๆ' , '16' , 'ไม่อนุญาตให้นำเสนอ (No consent)') as สถานะ" + " from MAS_LEADS_TRANS  WHERE AGENT_ID = '" + Module2.Agent_Id + "' AND TO_DATE(LEAD_CALL_DATE,'DD-MM-YYYY')= to_date('" + Today + "','DD-MM-YYYY') AND anumber  LIKE '%" + textbox_search_number + "%' ";

                        using (OracleConnection connect = new OracleConnection(strConn))
                        {

                            connect.Open();
                            using (OracleTransaction transaction = connect.BeginTransaction())
                            {
                                using(OracleCommand command =new OracleCommand(sqlselect, connect))
                        {
                            WriteLog.instance.LogSql(sqlselect);

                            List<ViewModel> da = connect.Query<ViewModel>(command.CommandText).ToList();

                            if (da.Count > 0)
                            {
                                foreach (ViewModel dt1 in da)
                                {

                                    Telclass2 Telclassss = new Telclass2();
                                    Telclassss.anumber = dt1.เบอร์โทรศัพท์;
                                    Telclassss.cust_name = dt1.ชื่อ;
                                    Telclassss.cust_sname = dt1.นามสกุล;
                                    if (dt1.CUST_SEX == "F")
                                    {
                                        Telclassss.cust_sex = "หญิง";
                                    }
                                    else if (dt1.CUST_SEX == "M")
                                    {
                                        Telclassss.cust_sex = "ชาย";
                                    }
                                    else if (dt1.CUST_SEX == "N")
                                    {
                                        Telclassss.cust_sex = "ไม่ระบุ";
                                    }
                                    Telclassss.service_01 = dt1.SERVICE_21.ToString().Equals(DBNull.Value) ? "ไม่ได้สมัคร" : dt1.SERVICE_21.ToString();
                                    Telclassss.service_02 = dt1.SERVICE_11.ToString().Equals(DBNull.Value) ? "ไม่ได้สมัคร" : dt1.SERVICE_11.ToString(); ;
                                    Telclassss.service_03 = dt1.SERVICE_12.ToString().Equals(DBNull.Value) ? "ไม่ได้สมัคร" : dt1.SERVICE_12.ToString(); ;
                                    Telclassss.service_04 = dt1.SERVICE_13.ToString().Equals(DBNull.Value) ? "ไม่ได้สมัคร" : dt1.SERVICE_13.ToString(); ;

                                    Telclassss.lead_call_date = dt1.LEAD_CALL_DATE.ToString();
                                    Telclassss.status = dt1.สถานะ.ToString();
                                    telclassList.Add(Telclassss);
                                }

                            }

                        }
                        transaction.Commit();

                    }

                    connect.Close();
                        }

                    string json = JsonConvert.SerializeObject(telclassList);
                    return json;
                }

                catch (OracleException ex)
                {
                    WriteLog.instance.Log("Error ที่ CommanDataread : " + ex.Message.ToString());
                    return "";
                }
                finally
                {
                }


            }

            public int CommanEx(string sQL, string[] input = null, string[] parameter = null)
            {
                try
                {
                    dt = new DataTable();

                    Connectdb();


                    //// เปิดการเชื่อมต่อกับ Oracle
                    //Connect.Open();

                    using (OracleTransaction transaction = Connect.BeginTransaction())
                    {
                        OracleCommand Cmd = new OracleCommand(sQL, Connect);
                        if (input != null)
                        {
                            int i = 0;
                            foreach (string s in input)
                            {
                                Cmd.Parameters.Add(parameter[i], s);
                                i++;
                            }
                        }
                        try
                        {
                            int i = Cmd.ExecuteNonQuery();
                            Cmd.Dispose();
                            transaction.Commit();
                            transaction.Dispose();
                            return i;
                        }
                        catch (OracleException ex)
                        {
                            WriteLog.instance.Log("Error ที่ Comman_Ex : " + ex.Message.ToString());
                            transaction.Commit();
                            transaction.Dispose();
                            return -1;
                        }
                    }


                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ Comman_Ex : " + ex.Message.ToString());
                    return 0;
                }
                finally
                {


                    // ปิดการเชื่อมต่อเมื่อเสร็จสิ้น
                    Connect.Close();
                }
            }


            public int CommanEx_Save(string sQL, string[] input = null, string[] parameter = null)
            {
                try
                {
                    dt = new DataTable();


                    Connectdb();

                    //// เปิดการเชื่อมต่อกับ Oracle
                    //Connect.Open();

                    using (OracleTransaction transaction = Connect.BeginTransaction())
                    {
                        OracleCommand Cmd = new OracleCommand(sQL, Connect);
                        if (input != null)
                        {
                            int i = 0;
                            foreach (string s in input)
                            {
                                Cmd.Parameters.Add(parameter[i], s);
                                i++;
                            }
                        }
                        try
                        {
                            Cmd.ExecuteNonQuery();
                            transaction.Commit();
                            transaction.Dispose();
                            Cmd.Dispose();
                            Connect.Close();
                            return 0;
                        }
                        catch (OracleException ex)
                        {
                            WriteLog.instance.Log("Error ที่ Comman_Ex_save : " + ex.Message.ToString());
                            transaction.Commit();
                            transaction.Dispose();
                            return -1;
                        }
                    }


                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ Comman_Ex_save : " + ex.Message.ToString());
                    return 0;
                }
                finally
                {


                    // ปิดการเชื่อมต่อเมื่อเสร็จสิ้น
                    Connect.Close();
                }
            }


            public DataSet CommandSet(string sQL, string table, string[] input = null, string[] parameter = null)
            {
                try
                {
                    DataSet ds2 = new DataSet();

                    //Connect = new OracleConnection(Static_Cookie.strConn);
                    //Connect.Open();
                    Connectdb();

                    //// เปิดการเชื่อมต่อกับ Oracle
                    //Connect.Open();

                    using (OracleTransaction transaction = Connect.BeginTransaction())
                    {
                        OracleCommand command = new OracleCommand(sQL, Connect);
                        int i = 0;
                        OracleParameter parameter1 = null;
                        if (input != null)
                        {
                            foreach (string s in input)
                            {
                                parameter1 = new OracleParameter();
                                parameter1.Value = s;
                                parameter1.ParameterName = parameter[i];
                                command.Parameters.Add(parameter1);
                                i++;
                            }

                        }
                        OracleDataAdapter da = new OracleDataAdapter(command);
                        da.Fill(ds2, table);
                        command.Dispose();
                        transaction.Commit();
                    }

                    return ds2;
                }
                catch (OracleException ex)
                {
                    WriteLog.instance.Log("Error ที่ Comman_Set : " + ex.Message.ToString());
                    return null;
                }

            }


            public List<string> GetFromToken(string jwt)
            {
                try
                {
                    string pass = string.Empty;
                    string passarr = string.Empty;
                    string userarr = string.Empty;
                    string date = string.Empty;
                    string user = string.Empty;
                    string userfromcookie = string.Empty;
                    string[] loginacc = null;
                    if (jwt.Contains(";"))
                    {
                        loginacc = jwt.Split(';');
                        user = JWT.Decode(loginacc[0]);
                    }
                    else
                    {

                        userfromcookie = JWT.Decode(jwt);
                        userarr = userfromcookie.Split(':')[1].Split(',')[0].Replace(@"""", "");

                    }

                    if (loginacc != null)
                    {
                        if (loginacc.Length > 1)
                        {
                            pass = JWT.Decode(loginacc[1]);
                            passarr = pass.Split(':')[1].Split(',')[0].Replace(@"""", "");
                            date = pass.Split(':')[2].Split(',')[0].Replace("}", "").Replace(@"""", "");


                        }

                    }
                    else
                    {

                        date = userfromcookie.Split(',')[1].Replace("}", "").Replace(@"""", "").Replace("exp:", "");

                    }



                    List<string> tokens = new List<string>();
                    tokens.Add(userarr);
                    tokens.Add(passarr);
                    tokens.Add(date);


                    return tokens;
                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ GetFromToken : " + ex.Message.ToString());
                    return null;
                }

            }

            //[Obsolete]
            //public void BeginTransaction_Con()
            //{
            //    transaction = Connect.BeginTransaction();
            //}

            //[Obsolete]
            //public void BeginTransaction_Con1()
            //{
            //    transaction = Connect1.BeginTransaction();
            //}

            //[Obsolete]
            //public void Commitransaction_Con()
            //{
            //    transaction.Commit();
            //}

            //[Obsolete]
            //public void Commitransaction_Con1()
            //{
            //    transaction.Commit();
            //}


            public DataTable Comman_Static2(string sQL)
            {
                try
                {
                    OracleDataAdapter dr2;
                    DataTable dt = new DataTable();

                    try
                    {
                        Connectdb();

                        //// เปิดการเชื่อมต่อกับ Oracle
                        //Connect.Open();

                        dr2 = new OracleDataAdapter(sQL, Connect);
                    }
                    catch (Exception ex)
                    {
                        WriteLog.instance.Log("Error ที่ Comman_Static2 : " + ex.Message.ToString());
                        return null;
                    }
                    if (!string.IsNullOrEmpty(sQL))
                    {

                        dr2.Fill(dt);

                    }
                    return dt;
                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ Comman_Static2 : " + ex.Message.ToString());
                    return null;
                }
                finally
                {



                    // ปิดการเชื่อมต่อเมื่อเสร็จสิ้น
                    Connect.Close();

                }
            }


            public DataTable Comman_Static3(string sQL)
            {
                try
                {
                    OracleDataAdapter dr2;
                    DataTable dt = new DataTable();
                    try
                    {
                        Connectdb3();

                        //// เปิดการเชื่อมต่อกับ Oracle
                        //Connect.Open();

                        dr2 = new OracleDataAdapter(sQL, Connect);
                    }
                    catch (Exception ex)
                    {
                        WriteLog.instance.Log("Error ที่ Comman_Static3 : " + ex.Message.ToString());
                        return null;
                    }
                    if (!string.IsNullOrEmpty(sQL))
                    {

                        dr2.Fill(dt);

                    }
                    return dt;
                }
                catch (Exception ex)
                {
                    WriteLog.instance.Log("Error ที่ Comman_Static3 : " + ex.Message.ToString());
                    return null;
                }
                finally
                {
                    // ปิดการเชื่อมต่อเมื่อเสร็จสิ้น

                    Connect.Close();
                }
            }
            public static DataTable ToDataTable<T>(IEnumerable<T> items)
            {
                var table = new DataTable();
                var props = typeof(T).GetProperties();

                foreach (var prop in props)
                {
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                foreach (var item in items)
                {
                    var values = props.Select(prop => prop.GetValue(item)).ToArray();
                    table.Rows.Add(values);
                }

                return table;
            }

        }

        public interface ICahce
        {
            object Caching_set(string username, object password);
            object Caching_Get(string username);
        }
    }
