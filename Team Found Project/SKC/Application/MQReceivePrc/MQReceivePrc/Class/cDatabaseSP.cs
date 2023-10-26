using MQReceivePrc.Models.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cDatabaseSP
    {
        string tConnStr;
        cmlShopDB oShopDB;
        /// <summary>
        /// Clear Memory
        /// </summary>
        public void C_CLExMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CLExMemory : " + oEx.Message); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tConnectionString">Connection String to Database</param>
        /// <param name="poShopDB"></param>
        public cDatabaseSP( string tConnectionString, cmlShopDB poShopDB)
        {
            tConnStr = tConnectionString;
            oShopDB = poShopDB;
        }
        /// <summary>
        /// Copy List of T Data to Database
        /// </summary>
        /// <typeparam name="T">Type of Database Model</typeparam>
        /// <param name="poTblData">List of Database Model</param>
        /// <param name="ptTableName">Database Name</param>
        /// <param name="ptErrMsg">ref Error Message</param>
        /// <returns></returns>
        public bool C_PRCbDBInsertbyList<T>(List<T> poTblData,string ptTableName, ref string ptErrMsg)
        {
            cDataReader<T> aoTblData;
            SqlTransaction oTranscation;
            try
            {
                using (SqlConnection oConn = new SqlConnection(tConnStr))
                {
                    oConn.Open();
                    oTranscation = oConn.BeginTransaction();
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {

                        try
                        {
                            aoTblData = new cDataReader<T>(poTblData);
                            foreach (string tColName in aoTblData.ColumnNames)
                            {
                                oBulkCopy.ColumnMappings.Add(tColName, tColName);
                            }

                            oBulkCopy.BatchSize = 100;
                            oBulkCopy.DestinationTableName = $"dbo.{ptTableName}";

                            oBulkCopy.WriteToServer(aoTblData);

                            oTranscation.Commit();
                            ptErrMsg = "";
                            return true;
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }
            }
            catch(Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
            }
            finally
            {
                aoTblData = null;
                oTranscation = null;
                C_CLExMemory();
            }
            return false;
        }
        /// <summary>
        /// Create TableDst by schema from TableSrc
        /// </summary>
        /// <param name="ptTableSrc_Name">Table Source Name</param>
        /// <param name="ptTableDst_Name">Table Destination Name</param>
        /// <param name="ptErrMsg">ref Error Message</param>
        /// <returns></returns>
        public bool C_PRCbCreateTableTemp(string ptTableSrc_Name,string ptTableDst_Name, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nRowEff = 0;
            try
            {
                oDB = new cDatabase();
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

                oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowEff);

                ptErrMsg = "";
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
            }
            finally
            {
                oSql = null;
                oDB = null;
                C_CLExMemory();
            }
            return false;
        }
    }
}
