using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model_HelperCore;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Text.Json.Serialization;
using Webapi_Global.Class_pool;

namespace Webapi_Global.Controllers
{
    [Route("Service")]
    [ApiController]
    public class Oracle_Class_Controller : ControllerBase
    {
        Funtion_Sql function_;
        Extention Extention_;
        DataTable dt;
        public Oracle_Class_Controller(Funtion_Sql function_Sql_ , Extention extention)
        {
            function_ = function_Sql_;
            Extention_ = extention;
        }
        [HttpPost]
        [Route("UpdateCNFG_Agent_Info")]
        public string UpdateCNFG_Agent_Info(string status, string Agen = "", string IP = "")
        {
           
            string strUpdate;
            strUpdate = "";
            strUpdate = "UPDATE CNFG_AGENT_INFO  ";
            strUpdate += " SET  TERMINAL_IP   = '" + IP + "' ,";
            strUpdate += " STATUS_ID = " + status + " ,";
            strUpdate += "CALL_COUNT = 0,";
            strUpdate += "LOGON_EXT = " + status + ",";
            strUpdate += " LOGIN_TIME   = sysdate ";
            strUpdate += " WHERE AGENT_ID = '" + Agen+ "'";

            try
            {
                {
                  function_.Function_Excute_Update_And_Insert_And_Delete (strUpdate);
                    return "200";
                }
            }
            catch (Exception ex)
            {
                return "ระบบมีปัญหา กรุณาติดต่อ Admin ค่ะ" + ex.Message + "ผลการตรวจสอบ";
            }


        }
        [HttpGet]
        [Route("GetJigSaw")]
        public string GetJigSaw()
        {
            try
            {
                return Extention_.Jigsaw();
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }  
        
        [HttpGet]
        [Route("GetPredictAgenT")]
        public string GetPreditAgenT(string txtUsername ,string txtPassword , string type_db ,string strDB)
        {
            try
            {
                dt = new DataTable();
                function_.Funtion_Select_Sql("SELECT * FROM PREDIC_AGENTS" +
                    "WHERE (LOGIN = " + txtUsername + " )"
               + " and (PASSWORD= " + txtPassword + " ) AND ROWNUM = 1 ", null, null, ref dt, type_db, strDB);
                string json = JsonConvert.SerializeObject(dt);
                return json;
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
          
        }     
        
        [HttpGet]
        [Route("Get_All_DataFromTable_Where")]
        public string Get_All_DataFromTable_Where( string type_db ,string strDB ,string Table , string column_where, string values_where)
        {
            string sql = string.Empty;
            try
            {
                sql += $@"SELECT * FROM {Table}  ";
                int i = 0;
                if (column_where != null)
                {
                    sql += $@" WHERE ";
                    foreach (string column_item in column_where.Split(';'))
                    {
                        if (i < column_where.Split(';').Length - 1)
                        {
                            sql += $@"{column_item} = {values_where.Split(';')[i]} AND ";

                        }
                        else
                        {
                            sql += $@"{column_item} = {values_where.Split(';')[i]} ";
                        }

                        i++;
                    }
                }
                dt = new DataTable();
                function_.Funtion_Select_Sql(sql, null, null, ref dt, type_db, strDB);
                string json = JsonConvert.SerializeObject(dt);
                return json;
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
         
        } 
        
        
        [HttpGet]
        [Route("Get_column_DataFromTable_Where")]
        public string Get_column_DataFromTable_Where(string type_db , string strDB, string Table ,string column_select,string column_where ,string values_where )
        {
            string sql = string.Empty;
            try
            {
                sql += $@"SELECT ";
                int i = 0;
                foreach (string item in column_select.Split(';'))
                {
                    if (i < column_select.Split(';').Length - 1)
                    {
                        sql += $@"{item} , ";

                    }
                    else
                    {
                        sql += $@"{item} ";
                    }

                    i++;
                }
                sql += $@" FROM {Table} ";
                string sql_where = string.Empty;
                if (column_where != null)
                {
                    int e = 0;
                    sql_where += $@" WHERE ";
                    foreach (string column_item in column_where.Split(';'))
                    {
                        if (e < column_where.Split(';').Length - 1)
                        {
                            //if(values_where_int != null)
                            //{
                            //    sql_where += $@"{column_item} = {values_where_int.Split(',')[e]} AND ";
                            //}
                            //else
                            {
                                sql_where += $@"{column_item} = {values_where.Split(';')[e]} AND ";
                            }

                        }
                        else
                        {
                            //if (values_where_string != null)
                            //{
                            sql_where += $@"{column_item} = {values_where.Split(';')[e]}";
                            //}
                            //else
                            //{
                            //    sql_where += $@"{column_item} = {values_where_int.Split(',')[e]} ";
                            //}
                        }

                        e++;
                    }
                }
                sql += sql_where;
                dt = new DataTable();
                function_.Funtion_Select_Sql(sql, null, null, ref dt, type_db, strDB);
                string json = JsonConvert.SerializeObject(dt);
                return json;
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
           
        }

        [HttpPost]
        [Route("Post_Save")]
        public ObjectResult Post_Save(string type_db, string strDB, string Table, string column_Insert, string values_Insert)
        {
            string sql = string.Empty;
            string sqlValues = string.Empty;
            string sqlWhere = string.Empty;
            try
            {
                sql += $@"INSERT INTO {Table} ";
                sqlValues += $@" (";
                int i = 0;
                foreach (string item in column_Insert.Split(';'))
                {
                    if (i < column_Insert.Split(';').Length - 1)
                    {
                        sqlValues += $@"{item} , ";

                    }
                    else
                    {
                        sqlValues += $@"{item} ";
                    }

                    i++;
                }
                sqlValues += " ) VALUES ( ";
                int e = 0;
                foreach (string item in values_Insert.Split(';'))
                {
                    if (e < values_Insert.Split(';').Length - 1)
                    {
                        sqlValues += $@"{item} , ";

                    }
                    else
                    {
                        sqlValues += $@"{item} )";
                    }

                    e++;
                }
                sql += sqlValues;
                int g = function_.Function_Excute_Update_And_Insert_And_Delete(sql, null, null, type_db, strDB);
                if (g == 0)
                {
                    return Ok("ข้อมูลไม่เข้า กรุณาติดต่อผู้ดูแลระบบ");
                }
                return Ok("ข้อมูลเข้าเรียบร่อย");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());

            }
        }
        [HttpGet]
        [Route("GetJigSaw_SaveToDrive")]
        public string GetJigSaw_SaveToDrive( string Imagename)

        {
            string imageUrl = $@"http://172.21.140.104:8084/imageOutput/" +Imagename +".jpg";
            string localImagePath = Imagename +".jpg"; // Specify the local path to save the image
          
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response =  client.GetAsync(imageUrl).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        using (Stream imageStream =  response.Content.ReadAsStreamAsync().Result)
                        using (FileStream fileStream = new FileStream(localImagePath, FileMode.Create))
                        {
                             imageStream.CopyToAsync(fileStream);
                        }

                        function_.Log("Image downloaded and saved successfully.");
                        return "Image downloaded and saved successfully.";
                    }
                    else
                    {
                        function_.Log($"Failed to download the image. Status code: {response.StatusCode}");
                        return $"Failed to download the image. Status code: {response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    function_.Log($"An error occurred: {ex.Message}");
                    return $"An error occurred: {ex.Message}";
                }
            }
        }
    }
}