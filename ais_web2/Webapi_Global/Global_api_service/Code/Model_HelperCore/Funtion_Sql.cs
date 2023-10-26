
using Dapper;
using Jose;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using HttpClient = Microsoft.AspNetCore.Http;



namespace Model_HelperCore
{
    public class Funtion_Sql
    {
        public readonly IHostEnvironment _environment;
        public readonly string user_name;
        public readonly string strConn;
        public readonly string type_db;

        public static string strDB_;
        public static string user_name_;
        public static string strConn_;
        public static string type_db_;
        HttpClient.HttpContext context;

        //public void Log(string message)
        //{
        //    try
        //    {
        //        string path = _environment.ContentRootPath + "\\InformationLog_Sql_And_Event\\";
        //        if (!Directory.Exists(path))

        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        using (StreamWriter steam = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
        //        {

        //            steam.WriteLine(": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": = " + message);
        //        }
        //    }
        //    catch
        //    {

        //    }


        //}

        //public void LogSql(string message)
        //{
        //    try
        //    {
        //        string path = _environment.ContentRootPath + "\\Information_sql\\";
        //        if (!Directory.Exists(path))

        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        using (StreamWriter steam = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
        //        {

        //            steam.WriteLine(": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": = " + message);
        //        }
        //    }
        //    catch
        //    {

        //    }


        //}

        public void Log_Save_information(string Cookie_AgenId, string Datetimes)
        {
            try
            {

                string path = _environment.ContentRootPath + "\\Log_Save_AgenID\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + "_" + Cookie_AgenId + "_" + Datetimes + ".txt", true))
                {

                    steam.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "=" + Cookie_AgenId + ",");
                }
            }
            catch
            {

            }


        }

        public string Log_Get_information(string Cookie_AgenId, string Datetimes)
        {
            string messages = string.Empty;
            try
            {

                string path = _environment.ContentRootPath + "\\Log_Save_AgenID\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";

                using (StreamReader steam = new StreamReader(path + "_" + Cookie_AgenId + "_" + Datetimes + ".txt", true))
                {

                    messages = steam.ReadToEnd();
                }
                return messages;
            }
            catch (Exception ex)
            {
                LogSql(ex.Message.ToString());
                return string.Empty;
            }
        }

        public string Log_Get_information_SaveData_And_Edit(string result = "", string type = "", string Cookie_AgenId = "", string Datetimes = "", ViewModel Model = null)

        {
            string messages = string.Empty;
            try
            {

                string path = _environment.ContentRootPath + "\\SaveData_And_Edit\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + "_" + Cookie_AgenId + "_" + Datetimes + ".txt", true))
                {
                    steam.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " , Result = " + result + " , Type = " + type + " ," + "=" + $@" txtTel_No :{Model.txtTel_No} , txtName : {Model.txtName} , txtSName : {Model.txtSName} , cboDate : {Model.cboDate} , cboMouth : {Model.cboMouth} , txtYear : {Model.txtYear} , cboSex : {Model.cboSex} , cboStatus : {Model.cboStatus.ToString().Replace(" ", "")} , cbocity :{Model.cbocity}");

                }


                return messages;
            }
            catch (Exception ex)
            {
                LogSql(ex.Message.ToString());
                return string.Empty;
            }


        }

        public int Log_Get_information_lenght(string Cookie_AgenId, string Datetimes)
        {
            int count = 0;
            try
            {

                string path = _environment.ContentRootPath + "\\Log_Save_AgenID\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";

                using (StreamReader steam = new StreamReader(path + "_" + Cookie_AgenId + "_" + Datetimes + ".txt", true))
                {

                    count = steam.ReadToEnd().Split(',').Length - 1;
                }
                return count;

            }
            catch (Exception ex)
            {
                LogSql(ex.Message.ToString());
                return count = 0;
            }


        }

        public Funtion_Sql(IHostEnvironment environment, string type_db = "", string strDB = "")

        {
      
            IConfiguration configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path if needed
           .AddJsonFile("appsettings.json")
           .Build();


            // สร้าง IConfiguration
            //if (type_db == null)
            //{
            //    type_db = type_db_;
            //}
            if (user_name == null)
            {
                user_name = user_name_;
            }

            if (type_db != null)
            {
                if (strDB == "Production")
                {
                    if (type_db == "1200")
                    {
                        strConn = configuration["pro1"].ToString();
                    }
                    else if (type_db == "2400")
                    {
                        strConn = configuration["pro2"].ToString();
                    }
                    else if (type_db == "4800")
                    {
                        strConn = configuration["pro3"].ToString();
                    }
                    else
                    {
                        strConn = configuration["pro"].ToString();
                    }
                }
                else if (strDB == "Backup")
                {
                    if (type_db == "1200")
                    {
                        strConn = configuration["back1"].ToString();
                    }
                    else if (type_db == "2400")
                    {
                        strConn = configuration["back2"].ToString();
                    }
                    else if (type_db == "4800")
                    {
                        strConn = configuration["back3"].ToString();
                    }
                    else
                    {
                        strConn = configuration["back"].ToString();
                    }
                }
                else
                {
                    if (type_db == "1200")
                    {
                        strConn = configuration["back1"].ToString();
                    }
                    else if (type_db == "2400")
                    {
                        strConn = configuration["back2"].ToString();
                    }
                    else if (type_db == "4800")
                    {
                        strConn = configuration["back3"].ToString();
                    }
                    else
                    {
                        strConn = configuration["back"].ToString();
                    }
                }
            }
            else
            {
                if (strDB == "Production")
                {
                    strConn = configuration["pro"].ToString();
                }
                else if (strDB == "Backup")
                {
                    strConn = configuration["back"].ToString();
                }
                else
                {
                    {
                        strConn = configuration["back"].ToString();
                    }

                }
            }

            _environment = environment;
        }
        public static OracleConnection Connect;


        public OracleCommand com;

        public OracleCommand Cmd;
        public DataSet ds;
        public DataTable dt;
        private OracleTransaction transaction;

        public OracleDataAdapter da;
        public OracleDataReader dr;
        public string agent = "";

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

        public void Connectdb(string type, string strDB = "Production")
        {
            try
            {
                Funtion_Sql function_Sql_Oracle = new Funtion_Sql(_environment, type, strDB);
                Connect = new OracleConnection(strConn);
                {
                    Connect.Open();
                }
            }
            catch (Exception ex)
            {
                Log("Error ที่ Connect : " + ex.Message.ToString());
                Log("ไม่สามารถบันทึกข้อมูลได้เนื่องจากปัญหาติดต่อฐานข้อมูล " + ex.Message + "ผลการตรวจสอบ");
            }
        }

        public void Funtion_Select_Sql(string sQL, string[] input, string[] parameter, ref DataTable dt, string type, string strDB = "Production")
        {

            try
            {
                LogSql(sQL);
                DataTable dt2 = new DataTable();
                object connectionLock = new object();
                //Connectdb();
                //while (Connect.State == ConnectionState.Closed)
                //{
                //    Connectdb();
                //}
                Connectdb(type, strDB);

                dt = Excutue_process_sql(sQL, input, parameter, type, strDB);



            }
            catch (Exception ex)
            {
                Log("Error ที่ Comman_Static : " + ex.Message.ToString());

            }
            finally
            {

            }
        }


        private DataTable Excutue_process_sql(string sQL, string[] input, string[] parameter, string type_db, string strDB = "Production")
        {
            Connectdb(type_db, strDB);
            string Paraname = string.Empty;
            DataTable dt1 = Execute_Sql(sQL, input, parameter, Paraname);

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
                        LogSql(Connect.ConnectionString);
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
        public int Function_Excute_Update_And_Insert_And_Delete(string sQL, string[] input = null, string[] parameter = null, string type = "", string strDB = "")
        {
            try
            {
                dt = new DataTable();

                Connectdb(type, strDB);

                var param = new DynamicParameters();
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
                            param.Add(parameter[i], s);
                            i++;
                        }
                    }
                    try
                    {
                        int i = 0;
                        if (input != null)
                        {
                            i = Connect.Execute(Cmd.CommandText, param);
                        }
                        else
                        {
                            i = Connect.Execute(Cmd.CommandText);
                        }
                        Cmd.Dispose();
                        transaction.Commit();
                        transaction.Dispose();
                        return i;
                    }
                    catch (OracleException ex)
                    {
                        Log("Error ที่ Comman_Ex : " + ex.Message.ToString());
                        transaction.Commit();
                        transaction.Dispose();
                        return -1;
                    }
                }


            }
            catch (Exception ex)
            {
                Log("Error ที่ Comman_Ex : " + ex.Message.ToString());
                return 0;
            }
            finally
            {


                // ปิดการเชื่อมต่อเมื่อเสร็จสิ้น
                Connect.Close();
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
                Log("Error ที่ GetFromToken : " + ex.Message.ToString());
                return null;
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
        public void Log(string message)
        {
            try
            {
                string path = _environment.ContentRootPath + "\\InformationLog_Sql_And_Event\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
                {

                    steam.WriteLine(": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": = " + message);
                }
            }
            catch
            {

            }


        }

        public void LogSql(string message)
        {
            try
            {
                string path = _environment.ContentRootPath + "\\Information_sql\\";
                if (!Directory.Exists(path))

                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter steam = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd") + ".txt", true))
                {

                    steam.WriteLine(": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": = " + message);
                }
            }
            catch
            {

            }


        }
    }
}
