using AdaPos.Models.Other;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace AdaPos.Class
{
    public class cDatabase
    {
        #region Constructor

        public cDatabase()
        {

        }

        #endregion End Constructor

        /// <summary>
        /// Connect database
        /// </summary>
        public OleDbConnection C_CONoDatabaseOleDb(cmlConfigDB poConfig)
        {
            OleDbConnection oConn = null;
            string tConnString;

            try
            {
                // Windows Authen
                if (string.Equals(poConfig.tAuthenDB, "0"))
                {
                    tConnString = @"Provider= SQLOLEDB;Server = " + poConfig.tServerDB + ";";

                    if (!string.IsNullOrEmpty(poConfig.tNameDB))
                        tConnString += "Database = " + poConfig.tNameDB + ";";

                    tConnString += "Integrated Security = True;";
                }
                // SQL Authen 
                else
                {
                    tConnString = @"Provider= SQLOLEDB;Data Source = " + poConfig.tServerDB + ";";

                    if (!string.IsNullOrEmpty(poConfig.tNameDB))
                        tConnString += "Initial Catalog = " + poConfig.tNameDB + ";";

                    tConnString += "Persist Security Info = True;";
                    tConnString += "User ID = " + poConfig.tUser + ";";
                    tConnString += "Password = " + poConfig.tPassword + ";";
                }

                tConnString += "MultipleActiveResultSets=true;";
                // Zen 63-06-17 Update การเก็บ Connection เพื่อเอาไปใช้กรณีที่ต้องการแค่ Connection String
                cVB.tVB_ConStr = tConnString;
                oConn = new OleDbConnection(tConnString);
                oConn.Open();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDatabase", "C_CONoDatabase " + oEx.Message); }

            return oConn;
        }

        public SqlConnection C_CONoDatabase(cmlConfigDB poConfig)
        {
            SqlConnection oConn = null;
            string tConnString;

            try
            {
                // Windows Authen
                if (string.Equals(poConfig.tAuthenDB, "0"))
                {
                    tConnString = @"Server = " + poConfig.tServerDB + ";";

                    if (!string.IsNullOrEmpty(poConfig.tNameDB))
                        tConnString += "Database = " + poConfig.tNameDB + ";";

                    tConnString += "Integrated Security = True;";
                }
                // SQL Authen 
                else
                {
                    tConnString = @"Data Source = " + poConfig.tServerDB + ";";

                    if (!string.IsNullOrEmpty(poConfig.tNameDB))
                        tConnString += "Initial Catalog = " + poConfig.tNameDB + ";";

                    tConnString += "Persist Security Info = True;";
                    tConnString += "User ID = " + poConfig.tUser + ";";
                    tConnString += "Password = " + poConfig.tPassword + ";";
                }

                tConnString += "MultipleActiveResultSets=true;";
                // Zen 63-06-17 Update การเก็บ Connection เพื่อเอาไปใช้กรณีที่ต้องการแค่ Connection String
                cVB.tVB_ConStr = tConnString;
                oConn = new SqlConnection(tConnString);
                oConn.Open();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDatabase", "C_CONoDatabase " + oEx.Message); }

            return oConn;
        }

        /// <summary>
        /// get data query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public List<T> C_GETaDataQuery<T>(string ptSql, int nCmdTime = 60)
        {
            List<T> aoItem = new List<T>();
            SqlConnection oConn = null;

            try
            {
                if (cVB.oVB_Config == null) return new List<T>(); //*Net 63-07-30 ปรับตาม Moshi
                oConn = C_CONoDatabase(cVB.oVB_Config);
                //aoItem = oConn.Query<T>(ptSql, commandTimeout: 60).ToList();
                aoItem = oConn.Query<T>(ptSql, commandTimeout: nCmdTime).ToList(); //*Arm 63-08-26
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_GETaDataQuery List " + oEx.Message + "/" + ptSql);
            }
            finally
            {
                if (oConn != null)
                {
                    oConn.Close();
                    oConn.Dispose();
                }

                oConn = null;
                ptSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoItem;
        }

        /// <summary>
        /// get data query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public T C_GEToDataQuery<T>(string ptSql)
        {
            T oItem = default(T);
            SqlConnection oConn = null;

            try
            {
                if (cVB.oVB_Config == null) return default(T); //*Net 63-07-30 ปรับตาม Moshi
                oConn = C_CONoDatabase(cVB.oVB_Config);
                oItem = oConn.Query<T>(ptSql, commandTimeout: 60).FirstOrDefault();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDatabase", "C_GEToDataQuery " + oEx.Message + "/" + ptSql); }
            finally
            {
                if (oConn != null)
                {
                    oConn.Close();
                    oConn.Dispose();
                }

                oConn = null;
                ptSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return oItem;
        }

        /// <summary>
        /// get data query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public DataTable C_GEToDataQuery(string ptSql)
        {
            DataTable oItem = new DataTable();
            SqlConnection oConn = null;

            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);
                IDataReader oDR = oConn.ExecuteReader(ptSql, commandTimeout: 60);
                oItem.Load(oDR);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDatabase", "C_GEToDataQuery " + oEx.Message + "/" + ptSql); }
            finally
            {
                if (oConn != null)
                {
                    oConn.Close();
                    oConn.Dispose();
                }

                oConn = null;
                ptSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด ClearMem
            }

            return oItem;
        }

        /// <summary>
        /// get data query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public DataSet C_GEToDataSetQuery(string ptSql)
        {
            DataSet oData = new DataSet();
            SqlConnection oConn = null;

            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);
                using (SqlCommand oDbCmd = new SqlCommand(ptSql, oConn))
                {
                    oDbCmd.CommandType = CommandType.Text;
                    oDbCmd.CommandTimeout = 60;

                    using (SqlDataAdapter oDbAdp = new SqlDataAdapter())
                    {
                        oDbAdp.SelectCommand = oDbCmd;
                        oDbAdp.Fill(oData);

                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDatabase", "C_GEToDataQuery " + oEx.Message + "/" + ptSql); }
            finally
            {
                if (oConn != null)
                {
                    oConn.Close();
                    oConn.Dispose();
                }

                oConn = null;
                ptSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return oData;
        }

        /// <summary>
        /// Set data [Insert, Update, Delete]
        /// </summary>
        /// <param name="ptSql"></param>
        public void C_SETxDataQuery(string ptSql, int nCmdTime = 60)
        {
            int nCount;
            SqlConnection oConn = null;

            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);
                //nCount = oConn.Execute(ptSql, commandTimeout: 60);
                nCount = oConn.Execute(ptSql, commandTimeout: nCmdTime); //*Arm 63-08-06
            }
            catch (Exception oEx) {
                new cLog().C_WRTxLog("cDatabase", "C_SETxDataQuery " + oEx.Message + "/" + ptSql); }
            finally
            {
                if (oConn != null)
                {
                    oConn.Close();
                    oConn.Dispose();
                }

                oConn = null;
                ptSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Net 63-07-30 ยกมาจาก Moshi ให้ Get Error ด้วย
        /// Set data [Insert, Update, Delete] with Error
        /// </summary>
        /// <param name="ptSql"></param>
        public void C_SETxDataQuery(string ptSql, out string ptErr)
        {
            int nCount;
            SqlConnection oConn = null;
            try
            {
                ptErr = "";
                oConn = C_CONoDatabase(cVB.oVB_Config);
                nCount = oConn.Execute(ptSql, commandTimeout: 60);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_SETxDataQuery " + oEx.Message + "/" + ptSql);
                ptErr = oEx.Message;

            }
            finally
            {
                if (oConn != null)
                {
                    oConn.Close();
                    oConn.Dispose();
                }

                oConn = null;
                ptSql = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// 
        /// <param name="poConfigDb">Config database.</param>
        /// <param name="ptStoreName">Store procedure name.</param>
        /// <param name="paPara">ref Store procedure parameter.</param>
        /// <param name="pnRowEff">ref Row effect</param>
        /// 
        /// <returns>
        /// true: Execute success.
        /// false: Execute false.
        /// </returns>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-01-30] - add new function/method.
        /// </remarks>
        public bool C_DATbExecuteNonQueryStoreProcedure(cmlConfigDB poConfigDb, string ptStoreName, ref SqlParameter[] paPara, ref int pnRowEff)
        {
            SqlConnection oDbConn;

            try
            {
                oDbConn = C_CONoDatabase(poConfigDb);
                using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbConn))
                {
                    oDbCmd.CommandType = CommandType.StoredProcedure;
                    oDbCmd.CommandTimeout = 60;

                    if (paPara != null && paPara.Count() > 0)
                    {
                        oDbCmd.Parameters.AddRange(paPara);
                    }

                    pnRowEff = oDbCmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                oDbConn = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="poConfigDb">Config database.</param>
        /// <param name="ptStoreName">Store procedure name.</param>
        /// <param name="paPara">ref Store procedure parameter.</param>
        /// <param name="poResult">ref Rusult.</param>
        /// 
        /// <returns>
        /// true: Execute success.
        /// false: Execute false.
        /// </returns>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-01-30] - add new function/method.
        /// </remarks>
        public bool C_DATbExecuteQueryStoreProcedure(cmlConfigDB poConfigDb, string ptStoreName, ref SqlParameter[] paPara, ref DataTable poResult)
        {
            SqlConnection oDbConn;

            try
            {
                oDbConn = C_CONoDatabase(poConfigDb);
                using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbConn))
                {
                    oDbCmd.CommandType = CommandType.StoredProcedure;
                    oDbCmd.CommandTimeout = 60;

                    if (paPara != null && paPara.Count() > 0)
                    {
                        oDbCmd.Parameters.AddRange(paPara);
                    }
                     
                    //using(OleDbDataAdapter oDbAdp = new OleDbDataAdapter())
                    //{
                    //    oDbAdp.SelectCommand = oDbCmd;

                    //    poResult = new DataTable();
                    //    oDbAdp.Fill(poResult);

                    //}

                    //*Em 63-05-26
                    SqlDataReader oDbRdr = oDbCmd.ExecuteReader();
                    poResult.Load(oDbRdr);
                    //+++++++++++++

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                oDbConn = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Function for get one value.
        /// </summary>
        /// <param name="ptFunc">Function</param>
        /// <param name="ptField">Field Name</param>
        /// <param name="ptTable">Table Name</param>
        /// <param name="ptWhere">Condition</param>
        /// <returns></returns>
        public string C_GETtFunction(string ptFunc, string ptField, string ptTable, string ptWhere)
        {
            SqlConnection oConn = null;
            DataTable odtTmp = new DataTable();
            StringBuilder oSql = new StringBuilder();
            string tValue = "";
            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);
                oSql.AppendLine("SELECT " + ptFunc + " (" + ptField + ") AS FTValue");
                oSql.AppendLine("FROM " + ptTable + " WITH(NOLOCK)");
                oSql.AppendLine(ptWhere);
                IDataReader oDR = oConn.ExecuteReader(oSql.ToString(), commandTimeout: 60);
                odtTmp.Load(oDR);
                if (odtTmp != null && odtTmp.Rows.Count > 0) tValue = odtTmp.Rows[0].Field<string>("FTValue");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDatabase", "C_GEToDataQuery " + oEx.Message); }
            finally
            {
                if (oConn != null)
                {
                    oConn.Close();
                    oConn.Dispose();
                }

                oConn = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return tValue;
        }

        //*Net 63-03-09 สร้างตาราง Tmp ใน Database
        public void C_PRCxCreateDatabaseTmp(string ptTableSrc_Name, string ptTableDst_Name)
        {
            StringBuilder oSql;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine($"IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('{ptTableDst_Name}'))");
                oSql.AppendLine($"   BEGIN");
                oSql.AppendLine($"	    SELECT TOP 0 * INTO {ptTableDst_Name} FROM {ptTableSrc_Name} with(nolock)");
                oSql.AppendLine($"   END");
                oSql.AppendLine($"ELSE");
                oSql.AppendLine($"   BEGIN");
                oSql.AppendLine($"       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{ptTableDst_Name}' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{ptTableSrc_Name}' ),0)");
                oSql.AppendLine($"       BEGIN");
                oSql.AppendLine($"   	    DROP TABLE {ptTableDst_Name}");
                oSql.AppendLine($"   	    SELECT TOP 0 * INTO {ptTableDst_Name} FROM {ptTableSrc_Name} with(nolock)");
                oSql.AppendLine($"       END");
                oSql.AppendLine($"   END");
                oSql.AppendLine($"TRUNCATE TABLE {ptTableDst_Name}");
                oSql.AppendLine($"");

                C_SETxDataQuery( oSql.ToString());
            }
            catch (Exception oEx)
            { 
                new cLog().C_WRTxLog("cDatabase", "C_PRCxCreateDatabaseTmp " + oEx.Message); 
            }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// *Arm 63-03-09
        /// Execute store procedure. 
        /// </summary>
        /// <param name="ptConStr">Connection string.</param>
        /// <param name="ptStoreName">Store procedure name.</param>
        /// <param name="paPara">Store procedure parameter.</param>
        /// <param name="pnExeTime">Execute time out.</param>
        /// <returns>
        /// true: Execute success.
        /// false: Execute false.
        /// </returns>
        public bool C_DATbExecuteStoreProcedure(cmlConfigDB poConfigDb, string ptStoreName, ref SqlParameter[] paPara, string ptReturnName = "")
        {
            SqlConnection oDbConn;
            try
            {
                oDbConn = C_CONoDatabase(poConfigDb);
                using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbConn))
                {

                    oDbCmd.CommandType = CommandType.StoredProcedure;
                    oDbCmd.CommandTimeout = 60;

                    if (paPara != null && paPara.Count() > 0)
                    {
                        oDbCmd.Parameters.AddRange(paPara);
                    }

                    if (ptReturnName != "")
                    {
                        oDbCmd.ExecuteNonQuery();
                        int nReturn = (int)oDbCmd.Parameters[ptReturnName].Value;
                        if (nReturn == 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        int nReturn = oDbCmd.ExecuteNonQuery();
                        if (nReturn == 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_DATbExecuteStoreProcedure : " + oEx.Message + "(" + ptStoreName + ")");
                return false;
            }
            finally
            {
                //*Net 63-07-30 ปิด ClearMem
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="poConfigDb">Config Database</param>
        /// <param name="ptStoreName">Name Store Procedure</param>
        /// <param name="paPara">Parameter store procedure</param>
        /// <param name="paoResult">Result of dataset</param>
        /// <returns></returns>
        public bool C_DATbExecuteQueryStoreProcedure(cmlConfigDB poConfigDb, string ptStoreName, ref SqlParameter[] paPara, ref DataSet paoResult)
        {
            SqlConnection oDbConn;

            try
            {
                oDbConn = C_CONoDatabase(poConfigDb);
                using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbConn))
                {
                    oDbCmd.CommandType = CommandType.StoredProcedure;
                    oDbCmd.CommandTimeout = 60;

                    if (paPara != null && paPara.Count() > 0)
                    {
                        oDbCmd.Parameters.AddRange(paPara);
                    }

                    using (SqlDataAdapter oDbAdp = new SqlDataAdapter())
                    {
                        oDbAdp.SelectCommand = oDbCmd;

                        paoResult = new DataSet();
                        oDbAdp.Fill(paoResult);

                    }

                    return true;
                }
            }
            catch (Exception oEx)
            {
                //*Net 63-07-30 ยกมาจาก Moshi
                new cLog().C_WRTxLog("cDatabase", "C_DATbExecuteQueryStoreProcedure " + oEx.Message);   //*Em 63-06-17
                return false;
            }
            finally
            {
                oDbConn = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }


        /// <summary>
        /// *Em 63-04-20
        /// Clear memory SQL.
        /// </summary>
        public void C_CLExMemorySQL()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine("DBCC FREEPROCCACHE");
                oSql.AppendLine("DBCC DROPCLEANBUFFERS ");
                oSql.AppendLine("DBCC FREESYSTEMCACHE ('ALL')");
                oSql.AppendLine("DBCC FREESESSIONCACHE");
                
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDatabase", "C_CLExMemorySQL " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
    }
}
