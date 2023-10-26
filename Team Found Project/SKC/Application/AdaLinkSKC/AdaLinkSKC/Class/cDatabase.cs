using AdaLinkSKC.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AdaLinkSKC.Class
{
    public class cDatabase
    {
        public cDatabase()
        {

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
                cVB.tVB_Conn = tConnString;
                oConn = new SqlConnection(tConnString);
                oConn.Open();
            }
            catch (Exception oEx) { 
                //new cLog().C_WRTxLog("cDatabase", "C_CONoDatabase " + oEx.Message); 
            }

            return oConn;
        }

        public List<T> C_GETaDataQuery<T>(string ptSql)
        {
            List<T> aoItem = new List<T>();
            SqlConnection oConn = null;

            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);
                aoItem = oConn.Query<T>(ptSql, commandTimeout: 60).ToList();
            }
            catch (Exception oEx)
            {
                //new cLog().C_WRTxLog("cDatabase", "C_GETaDataQuery List " + oEx.Message);
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
                new cSP().SP_CLExMemory();
            }

            return aoItem;
        }

        public void C_SETxDataQuery(string ptSql)
        {
            int nCount;
            SqlConnection oConn = null;

            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);
                nCount = oConn.Execute(ptSql, commandTimeout: 60);
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cDatabase", "C_SETxDataQuery " + oEx.Message + "/" + ptSql);
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
                new cSP().SP_CLExMemory();
            }
        }

        public DataTable C_GEToTblSQLWithConnStr(string ptSQL)
        {
            DataTable oDbTbl = new DataTable();
            SqlDataAdapter oDbApt;
            SqlConnection oConn = null;
            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);
                SqlCommand oSqlCmd = new SqlCommand(ptSQL, oConn);
                oDbApt = new SqlDataAdapter(oSqlCmd);
                oDbApt.Fill(oDbTbl);
                return oDbTbl;
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("C_GEToTblSQLWithConnStr", oEx.Message.ToString());
                return null;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        public string C_GETtSQLScalarWithConnStr(string ptSQL)
        {
            string tResult;
            SqlConnection oConn = null;
            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);                
                SqlCommand oSqlCmd = new SqlCommand(ptSQL, oConn);
                tResult = Convert.ToString(oSqlCmd.ExecuteScalar());
                return tResult;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_GETtSQLScalarWithConnStr", oEx.Message.ToString());
                return oEx.Message;
            }
        }
        public DataTable C_GEToSQLToDatatable(string ptSQL)
        {
            DataTable oTbl;
            SqlConnection oSqlConn = null;
            try
            {
                oTbl = new DataTable();
                oSqlConn = C_CONoDatabase(cVB.oVB_Config);
                SqlCommand oSqlCmd = new SqlCommand(ptSQL, oSqlConn);
                SqlDataAdapter oAdtr = new SqlDataAdapter(oSqlCmd);
                oAdtr.Fill(oTbl);
                return oTbl;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_GEToSQLToDatatable", oEx.Message.ToString());
                throw;
            }
        }
        public int C_GETnSQLExecuteWithConnStr(string ptSQL, string ptConnstr)
        {
            try
            {
                SqlConnection oSqlConn = new SqlConnection(ptConnstr);
                oSqlConn.Open();
                SqlCommand oSqlCmd = new SqlCommand(ptSQL, oSqlConn);
                oSqlCmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_GETnSQLExecuteWithConnStr", oEx.Message.ToString());
                return 0;
            }
        }

        public int C_GETnSQLLoopExecuteWithConnStr(string ptConnstr, List<string> paSQL)
        {
            try
            {
                SqlConnection oSqlConn = new SqlConnection(ptConnstr);
                oSqlConn.Open();
                SqlCommand oSqlCmd = new SqlCommand();
                oSqlCmd.Connection = oSqlConn;
                foreach (string tSQL in paSQL)
                {
                    oSqlCmd.CommandText = tSQL;
                    oSqlCmd.ExecuteNonQuery();
                }
                return 1;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_GETnSQLLoopExecuteWithConnStr", oEx.Message.ToString());
                return 0;
            }
        }


        /// <summary>
        /// *Arm 63-07-02
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptConStr"></param>
        /// <param name="ptSqlCmd"></param>
        /// <param name="pnExeTime"></param>
        /// <returns></returns>
        public T C_DAToExecuteQuery<T>(string ptSql)
        {
            T oItem = default(T);
            SqlConnection oConn = null;
            try
            {
                oConn = C_CONoDatabase(cVB.oVB_Config);
                oItem = oConn.Query<T>(ptSql, commandTimeout: 60).FirstOrDefault();
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cDatabase", "C_DAToExecuteQuery : " + oEx.Message + "(" + ptSql + ")");
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return oItem;
        }
    }
}
