using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Void;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cVoid
    {

        //* Net 63-08-03 ยกมาจาก Moshi
        //public bool C_PRCbUploadVoid(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        public bool C_PRCbUploadVoid(string ptQueueID, cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDataReader<cmlTPSTVoidDT> aoDT;
            cDataReader<cmlTPSTVoidDTDis> aoDTDis;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlTPSTVoid oVoid;
            SqlTransaction oTranscation;
            SqlConnection oConn;
            string tQueueID = "";

            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oVoid = JsonConvert.DeserializeObject<cmlTPSTVoid>(poData.ptData);

                tQueueID = ptQueueID.Replace("-", "");

                string tTblVoidDT = "TSVDT" + tQueueID;
                string tTblVoidDTDis = "TSVDTDis" + tQueueID;

                //Create Temp
                #region Create Temp
                #region Comment
                //TPSTVoidDT
                //#region TPSTVoidDT
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTVoidDTTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTVoidDTTmp FROM TPSTVoidDT with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTVoidDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTVoidDT' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTVoidDTTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTVoidDTTmp FROM TPSTVoidDT with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTVoidDTTmp");
                //oSql.AppendLine("");
                //#endregion End TPSTVoidDT

                ////TPSTVoidDTDis
                //#region TPSTVoidDTDis
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTVoidDTDisTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTVoidDTDisTmp FROM TPSTVoidDTDis with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTVoidDTDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTVoidDTDis' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTVoidDTDisTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTVoidDTDisTmp FROM TPSTVoidDTDis with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTVoidDTDisTmp");
                //oSql.AppendLine("");
                //#endregion End TPSTVoidDTDis


                // oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion

                oDB.C_PRCxCreateDatabaseTmp("TPSTVoidDT", tTblVoidDT, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTVoidDTDis", tTblVoidDTDis, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);

                #endregion

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTranscation = oConn.BeginTransaction();

                //insert to DB
                if (oVoid.aoTPSTVoidDT != null)
                {
                    aoDT = new cDataReader<cmlTPSTVoidDT>(oVoid.aoTPSTVoidDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTVoidDTTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblVoidDT}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoDT);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oVoid.aoTPSTVoidDTDis != null)
                {
                    aoDTDis = new cDataReader<cmlTPSTVoidDTDis>(oVoid.aoTPSTVoidDTDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTVoidDTDisTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblVoidDTDis}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoDTDis);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }
                oTranscation.Commit();

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TPSTVoidDT DT WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTVoidDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FNVidNo = TMPDT.FNVidNo AND DT.FNXidSeqNo = TMPDT.FNXidSeqNo");
                oSql.AppendLine($"   INNER JOIN {tTblVoidDT} TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FNVidNo = TMPDT.FNVidNo AND DT.FNXidSeqNo = TMPDT.FNXidSeqNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTVoidDT");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTVoidDTTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblVoidDT} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTD ");
                oSql.AppendLine("   FROM TPSTVoidDTDis DTD WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTVoidDTDisTmp TMPDTD WITH(NOLOCK) ON DTD.FTBchCode = TMPDTD.FTBchCode AND DTD.FNVidNo = TMPDTD.FNVidNo AND DTD.FNXidSeqNo = TMPDTD.FNXidSeqNo AND DTD.FDXddDateIns = TMPDTD.FDXddDateIns");
                oSql.AppendLine($"   INNER JOIN {tTblVoidDTDis} TMPDTD WITH(NOLOCK) ON DTD.FTBchCode = TMPDTD.FTBchCode AND DTD.FNVidNo = TMPDTD.FNVidNo AND DTD.FNXidSeqNo = TMPDTD.FNXidSeqNo AND DTD.FDXddDateIns = TMPDTD.FDXddDateIns");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTVoidDTDis");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTVoidDTDisTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblVoidDTDis} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                //* Net 63-08-03 ยกมาจาก Moshi
                //*Net 63-06-09 Drop Temp
                oSql.Clear();
                oSql.AppendLine("DROP TABLE " + tTblVoidDT + "");
                oSql.AppendLine("DROP TABLE " + tTblVoidDTDis + "");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //+++++++++++++

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadVoid");
                return false;
            }
            finally
            {
                aoDT = null;
                aoDTDis = null;
                oSql = null;
                oVoid = null;
                oTranscation = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
                new cSP().SP_CLExMemory();
            }
        }
    }
}
