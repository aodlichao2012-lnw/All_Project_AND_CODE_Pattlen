using API2Wallet.Class.Standard;
using API2Wallet.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API2Wallet.Class
{
    /// <summary>
    /// Class Database
    /// </summary>
    public class cDatabase
    {
        private AdaFCEntities oC_AdaAcc;

        /// <summary>
        ///     Constructor
        /// </summary>
        public cDatabase()
        {
            EntityConnectionStringBuilder oEntityConnStr;
            SqlConnectionStringBuilder oSqlConnStr;
            string tConnStr;

            try
            {
                tConnStr = ConfigurationManager.ConnectionStrings["AdaFCEntities"].ConnectionString;
                oEntityConnStr = new EntityConnectionStringBuilder(tConnStr);
                oSqlConnStr = new SqlConnectionStringBuilder(oEntityConnStr.ProviderConnectionString);
                oSqlConnStr.ConnectTimeout = cCS.nCS_ConTme;
                oEntityConnStr.ProviderConnectionString = oSqlConnStr.ConnectionString;

                oC_AdaAcc = new AdaFCEntities(oEntityConnStr.ConnectionString);
                oC_AdaAcc.Database.Connection.Open();
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (EntityException oEtyExn)
            {
                throw oEtyExn;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// 
        /// <param name="pnConTme">Connection time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        public cDatabase(int pnConTme = cCS.nCS_ConTme)
        {
            EntityConnectionStringBuilder oEntityConnStr;
            SqlConnectionStringBuilder oSqlConnStr;
            string tConnStr;

            try
            {
                tConnStr = ConfigurationManager.ConnectionStrings["AdaFCEntities"].ConnectionString;
                oEntityConnStr = new EntityConnectionStringBuilder(tConnStr);
                oSqlConnStr = new SqlConnectionStringBuilder(oEntityConnStr.ProviderConnectionString);
                oSqlConnStr.ConnectTimeout = pnConTme;
                oEntityConnStr.ProviderConnectionString = oSqlConnStr.ConnectionString;

                oC_AdaAcc = new AdaFCEntities(oEntityConnStr.ConnectionString);
                oC_AdaAcc.Database.Connection.Open();
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (EntityException oEtyExn)
            {
                throw oEtyExn;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
        }

        /// <summary>
        ///     Execute sql command insert, update, delete etc.
        /// </summary>
        /// 
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Row effect of command.
        /// </returns>
        public int C_DATnExecuteSql(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            int nRowEff = 0;

            try
            {
                oC_AdaAcc.Database.CommandTimeout = pnCmdTme;
                nRowEff = oC_AdaAcc.Database.ExecuteSqlCommand(ptSqlCmd);
            }
            catch (SqlException oSqlExn)
            {
                throw oSqlExn;
            }
            catch (EntityException oEtyExn)
            {
                throw oEtyExn;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {

            }

            return nRowEff;
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <typeparam name="T">Type return.</typeparam>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Result of sql command in list of class model.
        /// </returns>
        public List<T> C_DATaSqlQuery<T>(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            try
            {
                List<T> aoResult = new List<T>();
                oC_AdaAcc.Database.CommandTimeout = pnCmdTme;
                aoResult = oC_AdaAcc.Database.SqlQuery<T>(ptSqlCmd).ToList();

                return aoResult;
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (EntityException oEtyExn)
            {
                throw oEtyExn;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {
                ;
            }
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <typeparam name="T">Type return.</typeparam>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Result of sql command in class model.
        /// </returns>
        public T C_DAToSqlQuery<T>(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            try
            {
                T oResult = default(T);
                oC_AdaAcc.Database.CommandTimeout = pnCmdTme;
                oResult = oC_AdaAcc.Database.SqlQuery<T>(ptSqlCmd).FirstOrDefault();

                //T oResult = (T)Activator.CreateInstance(typeof(T)); //*[ANUBIS][][2018-05-03] - ใช้ default(T) แทน เพราะใช้ได้ทั้ง string, int, model class.
                return oResult;
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (EntityException oEtyExn)
            {
                throw oEtyExn;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {

            }
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// <param name="ptTblName">Table name.</param>
        /// 
        /// <returns>
        ///     Result of sql command in DataTable.
        /// </returns>
        public DataTable C_DAToSqlQuery(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme, string ptTblName = "TableTemp")
        {
            DataTable oDbTblResult;

            try
            {
                DbProviderFactory oDbFactory = DbProviderFactories.GetFactory(oC_AdaAcc.Database.Connection);
                using (DbCommand oDbCmd = oDbFactory.CreateCommand())
                {
                    oDbCmd.Connection = oC_AdaAcc.Database.Connection;
                    oDbCmd.CommandType = CommandType.Text;
                    oDbCmd.CommandText = ptSqlCmd;
                    using (DbDataAdapter oDbAdp = oDbFactory.CreateDataAdapter())
                    {
                        oDbAdp.SelectCommand = oDbCmd;

                        oDbTblResult = new DataTable();
                        oDbTblResult.TableName = ptTblName;
                        oDbAdp.Fill(oDbTblResult);
                    }
                }
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (EntityException oEtyExn)
            {
                throw oEtyExn;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {

            }

            return oDbTblResult;
        }

        /// <summary>
        ///     Bulk copy table.
        /// </summary>
        /// 
        /// <param name="poDbTblData">Data.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnBcpTme">Bulk copy time out.</param>
        /// 
        /// <returns>
        ///     true : Bulk copy success.<br/>
        ///     false : Bulk copy false.
        /// </returns>
        public bool C_DAToBulkCopyTable(DataTable poDbTblData, int pnBcpTme = cCS.nCS_BcpTme)
        {
            try
            {
                using (SqlBulkCopy oSqlBcp = new SqlBulkCopy(new AdaFCEntities().Database.Connection.ConnectionString.ToString()))
                {
                    oSqlBcp.BulkCopyTimeout = pnBcpTme;
                    oSqlBcp.DestinationTableName = poDbTblData.TableName;
                    oSqlBcp.WriteToServer(poDbTblData);

                    oSqlBcp.Close();
                }

                return true;
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (EntityException oEtyExn)
            {
                throw oEtyExn;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {

            }
        }

        /// <summary>
        ///  Interface IDisposable.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)oC_AdaAcc).Dispose();
        }

        /// <summary>
        /// Get name DB
        /// </summary>
        /// <returns></returns>
        public string C_GETtDBName()
        {
            string tResult = "";
            try
            {
                tResult = oC_AdaAcc.Database.Connection.Database;
                return tResult;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                tResult = null;
            }

        }

        /// <summary>
        /// Query sql return Datatable
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
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
                    if (oConn.State == ConnectionState.Open) {
                        oConn.Close();
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    } else {
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                } catch {
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
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return oDbTbl;
        }

        /// <summary>
        /// 
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
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return nResult;
        }

        /// <summary>
        /// SQl Query ExecuteScalar String
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
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
                    if (oConn.State == ConnectionState.Open) {
                        oConn.Close();
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    } else {
                        oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                }
                catch { }
                oCmd = new SqlCommand(ptSql, oConn);
                tResult = oCmd.ExecuteScalar().ToString();
            }
            catch (Exception) { }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return tResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
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
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return nResult;
        }

        /// <summary>
        /// Query stored procedure 
        /// </summary>
        /// <param name="ptStoreName"></param>
        /// <param name="paPara"></param>
        /// <returns>DataTable</returns>
        public DataTable C_GEToQueryStoreDataTbl(string ptStoreName,SqlParameter[] paPara)
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
            catch (Exception oEx)
            {
                return null;
            }
            finally
            {
                oDbTbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptStoreName"></param>
        /// <param name="paPara"></param>
        /// <param name="pnResult"></param>
        /// <returns></returns>
        public int C_GETnExecuteSqlStored(string ptStoreName,SqlParameter[] paPara)
        {
            int nResult = 0;
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
                        nResult = oDbCmd.ExecuteNonQuery();
                    }
                    oDbConn.Close();
                    oDbConn.Dispose();
                }
            }
            catch (Exception oEx) { }
            finally { }
            return nResult;
        }

        public SqlConnection C_CONoDatabase()
        {
            //*Arm 63-01-23
            SqlConnection oConn = null;
            string tConnString;
            try
            {
                oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString.ToString());
                oConn.Open();
            }
            catch (Exception oEx) { throw oEx; }

            return oConn;
        }
    }
}