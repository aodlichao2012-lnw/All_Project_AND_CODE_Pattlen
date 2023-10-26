using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MQReceivePrc.Class;

namespace MQReceivePrc.Class
{
    public class cDatabase
    {
        /// <summary>
        /// Get connection database string.
        /// </summary>
        /// <param name="ptSrvName">Server name.</param>
        /// <param name="ptUsr">User.</param>
        /// <param name="ptPwd">Password.</param>
        /// <param name="ptDBName">Database name.</param>
        /// <param name="pnTimeOut">Time out</param>
        /// <param name="ptAuthenMode">
        /// Authentication mode.
        /// <para/>1: Windows mode.
        /// <para/>2: SQL mode.
        /// </param>
        /// <returns>Connection string.</returns>
        public string C_GETtConnectString(string ptSrvName, string ptUsr, string ptPwd, string ptDBName, int pnTimeOut, string ptAuthenMode = "2")
        {
            StringBuilder oConnStr;

            try
            {
                // default database name.
                ptDBName = string.IsNullOrEmpty(ptDBName) ? "master" : ptDBName;

                oConnStr = new StringBuilder();
                if (string.Equals(ptAuthenMode, "1"))
                {
                    // authen windows mode.
                    oConnStr.Append("Data Source = " + ptSrvName);
                    oConnStr.Append(";Initial Catalog = " + ptDBName);
                    oConnStr.Append(";Integrated Security = SSPI;");
                    oConnStr.Append(";Connection Timeout = " + pnTimeOut);
                }
                else
                {
                    // authen SQL mode.
                    oConnStr.Append("Data Source = " + ptSrvName);
                    oConnStr.Append(";Initial Catalog = " + ptDBName);
                    oConnStr.Append(";User ID = " + ptUsr);
                    oConnStr.Append(";Password = " + ptPwd);
                    oConnStr.Append(";Connection Timeout = " + pnTimeOut);
                }

                return oConnStr.ToString();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_GETtConnectString : " + oEx.Message);
                return "";
            }
            finally
            {
                oConnStr = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Get connection database string for consolidate process.
        /// </summary>
        /// <param name="ptConStr">Connection database from RabbitMQ.</param>
        /// <param name="pnTimeOut">Time out.</param>
        /// <param name="ptAuthenMode">
        /// Authentication mode.
        /// <para/>1: Windows mode.
        /// <para/>2: SQL mode.
        /// </param>
        /// <returns>Connection string.</returns>
        public string C_GETtConnectString4PrcConsolidate(string ptConStr, int pnTimeOut, string ptAuthenMode = "2")
        {
            StringBuilder oConnStr;
            string[] atConStr, atDBInfo;

            try
            {
                oConnStr = new StringBuilder();
                atConStr = ptConStr.Split(';');

                if (string.Equals(ptAuthenMode, "1"))
                {
                    // authentication windows mode.
                    foreach (string tConInfo in atConStr)
                    {
                        atDBInfo = tConInfo.Split('=');
                        if (atDBInfo.Count() > 1)
                        {
                            switch (atDBInfo[0].Trim().ToUpper())
                            {
                                case "DATA SOURCE" :
                                    oConnStr.Append("Data Source = " + atDBInfo[1]);
                                    break;
                                case "INITIAL CATALOG":
                                    oConnStr.Append(";Initial Catalog = " + atDBInfo[1]);
                                    break;
                            }
                        }
                    }
                    oConnStr.Append(";Integrated Security = SSPI;");
                    oConnStr.Append(";Connection Timeout = " + pnTimeOut);
                }
                else
                {
                    // authentication sql mode.
                    foreach (string tConInfo in atConStr)
                    {
                        atDBInfo = tConInfo.Split('=');
                        if (atDBInfo.Count() > 1)
                        {
                            switch (atDBInfo[0].Trim().ToUpper())
                            {
                                case "DATA SOURCE":
                                    oConnStr.Append("Data Source = " + atDBInfo[1]);
                                    break;
                                case "INITIAL CATALOG":
                                    oConnStr.Append(";Initial Catalog = " + atDBInfo[1]);
                                    break;
                                case "USER ID":
                                    oConnStr.Append(";User ID = " + atDBInfo[1]);
                                    break;
                                case "PASSWORD":
                                    oConnStr.Append(";Password = " + atDBInfo[1]);
                                    break;
                            }
                        }
                    }
                    oConnStr.Append(";Connection Timeout = " + pnTimeOut);
                }

                return oConnStr.ToString();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_GETtConnectString4PrcConsolidate : " + oEx.Message + "(" + ptConStr + ")");
                return "";
            }
            finally
            {
                oConnStr = null;
                atConStr = null;
                atDBInfo = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Execute non query.
        /// </summary>
        /// <param name="ptConStr">Connection string.</param>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnExeTime">Execute time out.</param>
        /// <param name="pnRowEff">out Row effect.</param>
        /// <returns>
        /// true: Execute success.<para/>
        /// false: Execute false.
        /// </returns>
        public bool C_DATbExecuteNonQuery(string ptConStr, string ptSqlCmd, int pnExeTime, out int pnRowEff)
        {
            
            try
            {
                using (SqlConnection oDbCon = new SqlConnection(ptConStr))
                {
                    using (SqlCommand oDbCmd = new SqlCommand(ptSqlCmd, oDbCon))
                    {
                        oDbCon.Open();

                        oDbCmd.CommandType = CommandType.Text;
                        oDbCmd.CommandTimeout = pnExeTime;
                        pnRowEff = oDbCmd.ExecuteNonQuery();
                        
                        return true;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_DATbExecuteNonQuery : " + oEx.Message + "(" + ptSqlCmd + ")");
                pnRowEff = 0;
                return false;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Execute non query.
        /// </summary>
        /// <param name="ptConStr">Connection string.</param>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnExeTime">Execute time out.</param>
        /// <param name="pnRowEff">out Row effect.</param>
        /// <param name="ptErr">out Error.</param>
        /// <returns>
        /// true: Execute success.<para/>
        /// false: Execute false. and out Error Exception
        /// </returns>
        public bool C_DATbExecuteNonQuery(string ptConStr, string ptSqlCmd, int pnExeTime, out int pnRowEff, out string ptErr)
        {

            try
            {
                using (SqlConnection oDbCon = new SqlConnection(ptConStr))
                {
                    using (SqlCommand oDbCmd = new SqlCommand(ptSqlCmd, oDbCon))
                    {
                        oDbCon.Open();

                        oDbCmd.CommandType = CommandType.Text;
                        oDbCmd.CommandTimeout = pnExeTime;
                        pnRowEff = oDbCmd.ExecuteNonQuery();
                        ptErr = "";
                        return true;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_DATbExecuteNonQuery : " + oEx.Message + "(" + ptSqlCmd + ")");
                pnRowEff = 0;
                ptErr = oEx.Message.ToString();
                return false;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// Execute query.
        /// </summary>
        /// <param name="ptConStr">Connection string.</param>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnExeTime">Execute time out.</param>
        /// <returns>
        /// Data query.
        /// </returns>
        public DataTable C_DAToExecuteQuery(string ptConStr, string ptSqlCmd, int pnExeTime)
        {
            DataTable oDbTblTmp;
            SqlDataReader oDbRdrTmp;

            try
            {
                oDbTblTmp = new DataTable();
                using (SqlConnection oDbCon = new SqlConnection(ptConStr))
                {
                    using (SqlCommand oDbCmd = new SqlCommand(ptSqlCmd, oDbCon))
                    {
                        oDbCon.Open();

                        oDbCmd.CommandTimeout = pnExeTime;
                        oDbRdrTmp = oDbCmd.ExecuteReader(CommandBehavior.CloseConnection);

                        oDbTblTmp.Load(oDbRdrTmp);

                        return oDbTblTmp;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_DATbBulkCopyTable : " + oEx.Message + "(" + ptSqlCmd + ")");
                return null;
            }
            finally
            {
                oDbTblTmp = null;
                oDbRdrTmp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        /// <summary>
        /// Net 63-08-03 ยกมาจาก Moshi
        /// Get DataSet 
        /// </summary>
        /// <param name="ptConStr"></param>
        /// <param name="ptSqlCmd"></param>
        /// <param name="pnExeTime"></param>
        /// <returns></returns>
        public DataSet C_GEToDataSetQuery(string ptConStr, string ptSqlCmd, int pnExeTime)
        {
            DataSet oData = new DataSet();
            try
            {
                using (SqlConnection oDbCon = new SqlConnection(ptConStr))
                {
                    using (SqlCommand oDbCmd = new SqlCommand(ptSqlCmd, oDbCon))
                    {
                        oDbCmd.CommandType = CommandType.Text;
                        oDbCmd.CommandTimeout = pnExeTime;
                        using (SqlDataAdapter oDbAdp = new SqlDataAdapter())
                        {
                            oDbAdp.SelectCommand = oDbCmd;
                            oDbAdp.Fill(oData);

                            return oData;
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_GEToDataSetQuery : " + oEx.Message + "(" + ptSqlCmd + ")");
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return null;
        }
        public T C_DAToExecuteQuery<T>(string ptConStr, string ptSqlCmd, int pnExeTime)
        {
            T oItem = default(T);
            try
            { 
                using (SqlConnection oDbCon = new SqlConnection(ptConStr))
                {
                    oDbCon.Open();
                    oItem = oDbCon.Query<T>(ptSqlCmd, commandTimeout: 60).FirstOrDefault();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_DAToExecuteQuery : " + oEx.Message + "(" + ptSqlCmd + ")");
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return oItem;
        }

        public List<T> C_GETaDataQuery<T>(string ptConnStr, string ptSqlCmd, int pnExeTime)
        {
            List<T> aoItem = new List<T>();
            try
            {
                using (SqlConnection oDbCon = new SqlConnection(ptConnStr))
                {
                    oDbCon.Open();
                    aoItem = oDbCon.Query<T>(ptSqlCmd, commandTimeout: 60).ToList();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_GETaDataQuery : " + oEx.Message + "(" + ptSqlCmd + ")");
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return aoItem;
        }

        /// <summary>
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
        public bool C_DATbExecuteStoreProcedure(string ptConStr, string ptStoreName, ref SqlParameter[] paPara, int pnExeTime, string ptReturnName = "")
        {
            int nRowEff;

            try
            {
                using (SqlConnection oDbCon = new SqlConnection(ptConStr))
                {
                    using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbCon))
                    {
                        oDbCon.Open();

                        oDbCmd.CommandType = CommandType.StoredProcedure;
                        oDbCmd.CommandTimeout = pnExeTime;

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
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_DATbExecuteStoreProcedure : " + oEx.Message + "(" + ptStoreName + ")");
                return false;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        public bool C_DATbExecuteStoreProcedure(string ptConStr, string ptStoreName, ref SqlParameter[] paPara, int pnExeTime, ref DataTable podtResult, string ptReturnName = "")
        {
            int nRowEff;
            SqlDataReader oDR;
            try
            {
                using (SqlConnection oDbCon = new SqlConnection(ptConStr))
                {
                    using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbCon))
                    {
                        oDbCon.Open();

                        oDbCmd.CommandType = CommandType.StoredProcedure;
                        oDbCmd.CommandTimeout = pnExeTime;

                        if (paPara != null && paPara.Count() > 0)
                        {
                            oDbCmd.Parameters.AddRange(paPara);
                        }

                        if (ptReturnName != "")
                        {
                            oDR = oDbCmd.ExecuteReader();
                            //int nReturn = (int)oDbCmd.Parameters[ptReturnName].Value;
                            //if (nReturn == 0)
                            //{
                                podtResult = new DataTable();
                                podtResult.Load(oDR);
                                return true;
                            //}
                            //else
                            //{
                            //    return false;
                            //}
                        }
                        else
                        {
                            oDR = oDbCmd.ExecuteReader();
                            podtResult.Load(oDR);
                            return true;
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
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        /// <summary>
        /// Bulk copy table.
        /// </summary>
        /// <param name="ptConStr">Connection string.</param>
        /// <param name="ptDesTblName">Destination table name.</param>
        /// <param name="poDbTblSou">Data source.</param>
        /// <param name="pnExeTime">Execute time out.</param>
        /// <returns>
        /// true: bulk copy success.
        /// false: bulk copy false.
        /// </returns>
        public bool C_DATbBulkCopyTable(string ptConStr, string ptDesTblName, DataTable poDbTblSou, int pnExeTime)
        {
            try
            {
                using (SqlConnection oDbCon = new SqlConnection(ptConStr))
                {
                    oDbCon.Open();
                    using (SqlBulkCopy oDbBcp = new SqlBulkCopy(oDbCon))
                    {
                        oDbBcp.BulkCopyTimeout = pnExeTime;
                        oDbBcp.DestinationTableName = "[" + ptDesTblName + "]";

                        foreach (DataColumn oDbCol in poDbTblSou.Columns)
                        {
                            oDbBcp.ColumnMappings.Add(new SqlBulkCopyColumnMapping(oDbCol.ColumnName, oDbCol.ColumnName));
                        }

                        oDbBcp.WriteToServer(poDbTblSou);

                        oDbCon.Close();
                        return true;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_DATbBulkCopyTable : " + oEx.Message + "(" + ptDesTblName + ")");
                return false;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }


        /// <summary>
        /// Gen Temp 
        /// Arm 63-06-02
        /// </summary>
        /// <param name="ptTableSrc_Name"></param>
        /// <param name="ptTableDst_Name"></param>
        public void C_PRCxCreateDatabaseTmp(string ptTableSrc_Name, string ptTableDst_Name, string ptConnStr, int pnCommandTimeOut)
        {
            StringBuilder oSql;
            int nRowAffect;
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
                C_DATbExecuteNonQuery(ptConnStr, oSql.ToString(), pnCommandTimeOut, out nRowAffect);
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDatabase", "C_PRCxCreateDatabaseTmp " + oEx.Message);
            }
            finally
            {
                oSql = null;
            }
        }
    }
}
