using MQAdaLink.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MQAdaLink.Class
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
                //Server='202.44.55.96';Database='SKC_Fullloop2';User Id='SA';Password='GvFhk@61';
                cVB.tVB_Conn = tConnString;
                cVB.tVB_ConnMQ = "Data Source=" + poConfig.tServerDB + ";Initial Catalog=" + poConfig.tNameDB + ";User ID=" + poConfig.tUser + ";Password="+ poConfig.tPassword + ";Connection Timeout=30;Connection Lifetime=0;Min Pool Size=30;Max Pool Size=100;Pooling=true;";
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
                oSqlCmd.CommandTimeout = 500;
                tResult = Convert.ToString(oSqlCmd.ExecuteScalar());
                return tResult;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_GETtSQLScalarWithConnStr", oEx.Message.ToString() + " Sql : " + ptSQL +"");
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
                oSqlCmd.CommandTimeout = 0;
                SqlDataAdapter oAdtr = new SqlDataAdapter(oSqlCmd);
                oAdtr.Fill(oTbl);
                return oTbl;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_GEToSQLToDatatable", oEx.Message.ToString() + " : Sql " + ptSQL);
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
                oSqlCmd.CommandTimeout = 0;
                oSqlCmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_GETnSQLExecuteWithConnStr", oEx.Message.ToString() + "(" + ptSQL + ")");
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
                new cLog().C_PRCxLog("C_GETnSQLLoopExecuteWithConnStr", oEx.Message.ToString() );
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

        public bool C_DATbExecuteQueryStoreProcedure(cmlConfigDB poConfigDb, string ptStoreName, ref SqlParameter[] paPara)
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

                    //*Em 63-05-26
                    SqlDataReader oDbRdr = oDbCmd.ExecuteReader();
                    //poResult.Load(oDbRdr);
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
                new cSP().SP_CLExMemory();
            }
        }

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
                //new cLog().C_WRTxLog("cDatabase", "C_DATbExecuteStoreProcedure : " + oEx.Message + "(" + ptStoreName + ")");
                return false;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

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
                    SqlDataReader oDbRdr = oDbCmd.ExecuteReader();
                    poResult.Load(oDbRdr);

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
            }
        }
    }
}
