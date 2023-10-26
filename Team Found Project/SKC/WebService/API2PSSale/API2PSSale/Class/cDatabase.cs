using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace API2PSSale.Class
{
    /// <summary>
    /// Class database
    /// </summary>
    public class cDatabase
    {
        public SqlConnection C_CONoDatabase()
        {
            //*Arm 62-09-20
            SqlConnection oConn = null;
            string tConnString;
            try
            {
                oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString.ToString());
                oConn.Open();
            }
            catch (Exception oEx) { throw oEx; }

            return oConn;
            //*Arm 62-09-20
        }


        /// <summary>
        /// Get Datatable
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns>Datatable</returns>
        public DataTable C_GEToQuerySQLTbl(string ptSql)
        {
            SqlConnection oConn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            SqlDataAdapter oDbAdt = new SqlDataAdapter();
            DataTable oDbTbl = new DataTable();
            try
            {
                try
                {
                    if (oConn.State == ConnectionState.Open)
                    {
                        oConn.Close();
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                    else
                    {
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                }
                catch
                {
                }
                oCmd = new SqlCommand(ptSql, oConn);
                oDbAdt = new SqlDataAdapter(oCmd);
                oDbAdt.Fill(oDbTbl);
            }
            catch (Exception) { }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            return oDbTbl;
        }

        /// <summary>
        /// Execute Query Sql
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public int C_GETnQuerySQL(string ptSql)
        {
            SqlConnection oConn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            SqlDataAdapter oDbAdt = new SqlDataAdapter();
            DataTable oDbTbl = new DataTable();
            int nResult = 0;
            try
            {
                try
                {
                    if (oConn.State == ConnectionState.Open)
                    {
                        oConn.Close();
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                    else
                    {
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                }
                catch { }
                oCmd = new SqlCommand(ptSql, oConn);
                nResult = oCmd.ExecuteNonQuery();
            }
            catch (Exception) { }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            return nResult;
        }

        /// <summary>
        /// Query get data type string
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns>string</returns>
        public string C_GETtSQLScalarString(string ptSql)
        {
            SqlConnection oConn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            SqlDataAdapter oDbAdt = new SqlDataAdapter();
            DataTable oDbTbl = new DataTable();
            string tResult = "";
            try
            {
                try
                {
                    if (oConn.State == ConnectionState.Open)
                    {
                        oConn.Close();
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                    else
                    {
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                }
                catch { }
                oCmd = new SqlCommand(ptSql, oConn);
                var oResult = oCmd.ExecuteScalar();
                tResult = oResult == null ? "" : oResult.ToString();
            }
            catch (Exception oEx)
            {
                tResult = "-1";
            }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            return tResult;
        }

        /// <summary>
        /// Query get data type int
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns>int</returns>
        public int C_GETnSQLScalarInt(string ptSql)
        {
            SqlConnection oConn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            SqlDataAdapter oDbAdt = new SqlDataAdapter();
            DataTable oDbTbl = new DataTable();
            int nResult = 0;
            try
            {
                try
                {
                    if (oConn.State == ConnectionState.Open)
                    {
                        oConn.Close();
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                    else
                    {
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                }
                catch { }
                oCmd = new SqlCommand(ptSql, oConn);
                var oResult = oCmd.ExecuteScalar();
                nResult = oResult != null ? Convert.ToInt32(oResult) : 0;
            }
            catch (Exception oEx) { }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            return nResult;
        }

        /// <summary>
        /// Call stored procedure get datatable
        /// </summary>
        /// <param name="ptStoreName"></param>
        /// <param name="paPara"></param>
        /// <returns>Datatable</returns>
        public DataTable C_GEToQueryStoreDataTbl(string ptStoreName, SqlParameter[] paPara)
        {
            DataTable oDbTbl = new DataTable();
            try
            {
                using (SqlConnection oDbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString))
                {
                    oDbConn.Open();
                    using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbConn))
                    {
                        oDbCmd.CommandType = CommandType.StoredProcedure;
                        oDbCmd.CommandTimeout = 60;
                        oDbCmd.Parameters.AddRange(paPara);
                        using (SqlDataAdapter oDbAdt = new SqlDataAdapter(oDbCmd))
                        {
                            oDbAdt.Fill(oDbTbl);
                        }
                    }
                    oDbConn.Close();
                    oDbConn.Dispose();
                }
                return oDbTbl;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                oDbTbl = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// SQL Query Transection
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public bool C_SAVbSQLTran(ArrayList ptSql)
        {
            SqlTransaction oDbTran = null;
            try
            {
                using (SqlConnection oDbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString))
                {
                    oDbConn.Open();
                    oDbTran = oDbConn.BeginTransaction(IsolationLevel.ReadCommitted);
                    using (SqlCommand oDbCmd = new SqlCommand())
                    {
                        oDbCmd.Connection = oDbConn;
                        oDbCmd.CommandType = CommandType.Text;
                        oDbCmd.CommandTimeout = 60;
                        oDbCmd.Transaction = oDbTran;
                        foreach (string tSql in ptSql)
                        {
                            oDbCmd.CommandText = tSql;
                            oDbCmd.ExecuteNonQuery();
                        }
                    }
                    oDbTran.Commit();
                }
                return true;
            }
            catch (Exception oEx)
            {
                if (oDbTran != null)
                {
                    oDbTran.Rollback();
                }
                return false;
            }
            finally
            {
                oDbTran.Dispose();
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// get data query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public List<T> C_GETaDataQuery<T>(string ptSql)
        {
            List<T> aoItem = new List<T>();
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString);
                oConn.Open();
                aoItem = oConn.Query<T>(ptSql, commandTimeout: 60).ToList();
            }
            catch (Exception oEx)
            {
                return null;
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
            }
            return aoItem;
        }
    }
}