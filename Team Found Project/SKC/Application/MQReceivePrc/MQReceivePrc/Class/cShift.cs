using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Bch2Bch;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Shift;
using MQReceivePrc.Models.Webservice.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cShift
    {
        //* Net 63-08-03 ยกมาจาก Moshi
        //public bool C_PRCbUploadShift(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        public bool C_PRCbUploadShift(string ptQueueID, cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDataReader<cmlTPSTShiftHD> aoHD;
            cDataReader<cmlTPSTShiftDT> aoDT;
            cDataReader<cmlTPSTShiftEvent> aoEvent;
            cDataReader<cmlTPSTShiftSKeyBN> aoKeyBN;
            cDataReader<cmlTPSTShiftSKeyRcv> aoKeyRcv;
            cDataReader<cmlTPSTShiftSLastDoc> aoLastDoc;
            cDataReader<cmlTPSTShiftSRatePdt> aoRatePdt;
            cDataReader<cmlTPSTShiftSSumRcv> aoSumRcv;
            cDataReader<cmlTPSTUsrLog> aoUsrLog;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlTPSTShift oShift;
            SqlTransaction oTranscation;
            SqlConnection oConn;
            cSP oSP = new cSP();
            string tQueueID = "";

            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oShift = JsonConvert.DeserializeObject<cmlTPSTShift>(poData.ptData);

                //*Net 63-06-09
                tQueueID = ptQueueID.Replace("-", "");

                string tTblHD = "TSFHD" + tQueueID;
                string tTblDT = "TSFDT" + tQueueID;
                string tTblEvent = "TSFEvn" + tQueueID;
                string tTblKeyBN = "TSFKybn" + tQueueID;
                string tTblKeyRcv = "TSFKrcv" + tQueueID;
                string tTblLastDoc = "TSFLdoc" + tQueueID;
                string tTblRatePdt = "TSFRpdt" + tQueueID;
                string tTblSumRcv = "TSFSrcv" + tQueueID;
                string tTblUsrLog = "TSFUlog" + tQueueID;
                //+++++++++++++++++++++++

                //Create Temp
                #region Create Temp
                #region Comment
                // //TPSTShiftHD
                // #region TPSTShiftHD
                // oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTShiftHDTmp'))");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTShiftHDTmp FROM TPSTShiftHD with(nolock)");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("ELSE");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftHD' ),0)");
                // oSql.AppendLine("       BEGIN");
                // oSql.AppendLine("   	    DROP TABLE TPSTShiftHDTmp");
                // oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTShiftHDTmp FROM TPSTShiftHD with(nolock)");
                // oSql.AppendLine("       END");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("TRUNCATE TABLE TPSTShiftHDTmp");
                // oSql.AppendLine("");
                // #endregion TPSTShiftHD

                // //TPSTShiftDT
                // #region TPSTShiftDT
                // oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTShiftDTTmp'))");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTShiftDTTmp FROM TPSTShiftDT with(nolock)");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("ELSE");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftDT' ),0)");
                // oSql.AppendLine("       BEGIN");
                // oSql.AppendLine("   	    DROP TABLE TPSTShiftDTTmp");
                // oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTShiftDTTmp FROM TPSTShiftDT with(nolock)");
                // oSql.AppendLine("       END");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("TRUNCATE TABLE TPSTShiftDTTmp");
                // oSql.AppendLine("");
                // #endregion End TPSTShiftDT

                // //TPSTShiftEvent
                // #region TPSTShiftEvent
                // oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTShiftEventTmp'))");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTShiftEventTmp FROM TPSTShiftEvent with(nolock)");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("ELSE");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftEventTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftEvent' ),0)");
                // oSql.AppendLine("       BEGIN");
                // oSql.AppendLine("   	    DROP TABLE TPSTShiftEventTmp");
                // oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTShiftEventTmp FROM TPSTShiftEvent with(nolock)");
                // oSql.AppendLine("       END");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("TRUNCATE TABLE TPSTShiftEventTmp");
                // oSql.AppendLine("");
                // #endregion End TPSTShiftEvent

                // //TPSTShiftSKeyBN
                // #region TPSTShiftSKeyBN
                // oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTShiftSKeyBNTmp'))");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTShiftSKeyBNTmp FROM TPSTShiftSKeyBN with(nolock)");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("ELSE");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSKeyBNTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSKeyBN' ),0)");
                // oSql.AppendLine("       BEGIN");
                // oSql.AppendLine("   	    DROP TABLE TPSTShiftSKeyBNTmp");
                // oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTShiftSKeyBNTmp FROM TPSTShiftSKeyBN" +
                //     " with(nolock)");
                // oSql.AppendLine("       END");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("TRUNCATE TABLE TPSTShiftSKeyBNTmp");
                // oSql.AppendLine("");
                // #endregion End TPSTShiftSKeyBN

                // //TPSTShiftSKeyRcv
                //#region TPSTShiftSKeyRcv
                // oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTShiftSKeyRcvTmp'))");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTShiftSKeyRcvTmp FROM TPSTShiftSKeyRcv with(nolock)");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("ELSE");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSKeyRcvTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSKeyRcv' ),0)");
                // oSql.AppendLine("       BEGIN");
                // oSql.AppendLine("   	    DROP TABLE TPSTShiftSKeyRcvTmp");
                // oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTShiftSKeyRcvTmp FROM TPSTShiftSKeyRcv with(nolock)");
                // oSql.AppendLine("       END");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("TRUNCATE TABLE TPSTShiftSKeyRcvTmp");
                // oSql.AppendLine("");
                // #endregion End TPSTShiftSKeyRcv

                // //TPSTShiftSLastDoc
                // #region TPSTShiftSLastDoc
                // oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTShiftSLastDocTmp'))");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTShiftSLastDocTmp FROM TPSTShiftSLastDoc with(nolock)");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("ELSE");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSLastDocTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSLastDoc' ),0)");
                // oSql.AppendLine("       BEGIN");
                // oSql.AppendLine("   	    DROP TABLE TPSTShiftSLastDocTmp");
                // oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTShiftSLastDocTmp FROM TPSTShiftSLastDoc with(nolock)");
                // oSql.AppendLine("       END");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("TRUNCATE TABLE TPSTShiftSLastDocTmp");
                // oSql.AppendLine("");
                // #endregion End TPSTShiftSLastDoc

                // //TPSTShiftSRatePdt
                // #region TPSTShiftSRatePdt
                // oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTShiftSRatePdtTmp'))");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTShiftSRatePdtTmp FROM TPSTShiftSRatePdt with(nolock)");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("ELSE");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSRatePdtTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSRatePdt' ),0)");
                // oSql.AppendLine("       BEGIN");
                // oSql.AppendLine("   	    DROP TABLE TPSTShiftSRatePdtTmp");
                // oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTShiftSRatePdtTmp FROM TPSTShiftSRatePdt with(nolock)");
                // oSql.AppendLine("       END");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("TRUNCATE TABLE TPSTShiftSRatePdtTmp");
                // oSql.AppendLine("");
                // #endregion End TPSTShiftSRatePdt

                // //TPSTShiftSSumRcv
                // #region TPSTShiftSSumRcv
                // oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTShiftSSumRcvTmp'))");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTShiftSSumRcvTmp FROM TPSTShiftSSumRcv with(nolock)");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("ELSE");
                // oSql.AppendLine("   BEGIN");
                // oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSSumRcvTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTShiftSSumRcv' ),0)");
                // oSql.AppendLine("       BEGIN");
                // oSql.AppendLine("   	    DROP TABLE TPSTShiftSSumRcvTmp");
                // oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTShiftSSumRcvTmp FROM TPSTShiftSSumRcv with(nolock)");
                // oSql.AppendLine("       END");
                // oSql.AppendLine("   END");
                // oSql.AppendLine("TRUNCATE TABLE TPSTShiftSSumRcvTmp");
                // oSql.AppendLine("");
                // #endregion End TPSTShiftSSumRcv

                // oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion

                //*Net 63-06-09
                oDB.C_PRCxCreateDatabaseTmp("TPSTShiftHD", tTblHD, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTShiftDT", tTblDT, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTShiftEvent", tTblEvent, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTShiftSKeyBN", tTblKeyBN, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTShiftSKeyRcv", tTblKeyRcv, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTShiftSLastDoc", tTblLastDoc, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTShiftSRatePdt", tTblRatePdt, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTShiftSSumRcv", tTblSumRcv, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTUsrLog", tTblUsrLog, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                #endregion

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTranscation = oConn.BeginTransaction();

                //insert to DB
                if (oShift.aoTPSTShiftHD != null)
                {
                    aoHD = new cDataReader<cmlTPSTShiftHD>(oShift.aoTPSTShiftHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTShiftHDTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblHD}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoHD);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oShift.aoTPSTShiftDT != null)
                {
                    aoDT = new cDataReader<cmlTPSTShiftDT>(oShift.aoTPSTShiftDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTShiftDTTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblDT}"; //* Net 63-08-03 ยกมาจาก Moshi

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

                if (oShift.aoTPSTShiftEvent != null)
                {
                    aoEvent = new cDataReader<cmlTPSTShiftEvent>(oShift.aoTPSTShiftEvent);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoEvent.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTShiftEventTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblEvent}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoEvent);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oShift.aoTPSTShiftSKeyBN != null)
                {
                    aoKeyBN = new cDataReader<cmlTPSTShiftSKeyBN>(oShift.aoTPSTShiftSKeyBN);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoKeyBN.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTShiftSKeyBNTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblKeyBN}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoKeyBN);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oShift.aoTPSTShiftSKeyRcv != null)
                {
                    aoKeyRcv = new cDataReader<cmlTPSTShiftSKeyRcv>(oShift.aoTPSTShiftSKeyRcv);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoKeyRcv.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTShiftSKeyRcvTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblKeyRcv}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoKeyRcv);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oShift.aoTPSTShiftSLastDoc != null)
                {
                    aoLastDoc = new cDataReader<cmlTPSTShiftSLastDoc>(oShift.aoTPSTShiftSLastDoc);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoLastDoc.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTShiftSLastDocTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblLastDoc}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoLastDoc);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oShift.aoTPSTShiftSRatePdt != null)
                {
                    aoRatePdt = new cDataReader<cmlTPSTShiftSRatePdt>(oShift.aoTPSTShiftSRatePdt);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoRatePdt.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTShiftSRatePdtTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblRatePdt}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoRatePdt);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oShift.aoTPSTShiftSSumRcv != null)
                {
                    aoSumRcv = new cDataReader<cmlTPSTShiftSSumRcv>(oShift.aoTPSTShiftSSumRcv);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoSumRcv.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTShiftSSumRcvTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblSumRcv}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoSumRcv);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                //* Net 63-08-03 ยกมาจาก Moshi
                if (oShift.aoTPSTUsrLog != null)
                {
                    aoUsrLog = new cDataReader<cmlTPSTUsrLog>(oShift.aoTPSTUsrLog);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoUsrLog.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = $"dbo.{tTblUsrLog}";

                        try
                        {
                            oBulkCopy.WriteToServer(aoUsrLog);
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
                oSql.AppendLine("   DELETE HD ");
                oSql.AppendLine("   FROM TPSTShiftHD HD WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTShiftHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTPosCode = TMP.FTPosCode AND HD.FTShfCode = TMP.FTShfCode");
                oSql.AppendLine($"   INNER JOIN {tTblHD} TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTPosCode = TMP.FTPosCode AND HD.FTShfCode = TMP.FTShfCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTShiftHD");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTShiftHDTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblHD} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TPSTShiftDT DT WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTShiftDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTPosCode = TMPDT.FTPosCode AND DT.FTShfCode = TMPDT.FTShfCode ");
                oSql.AppendLine($"   INNER JOIN {tTblDT} TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTPosCode = TMPDT.FTPosCode AND DT.FTShfCode = TMPDT.FTShfCode ");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTShiftDT");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTShiftDTTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblDT} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE EVN ");
                oSql.AppendLine("   FROM TPSTShiftEvent EVN WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTShiftEventTmp TMPEVN WITH(NOLOCK) ON EVN.FTBchCode = TMPEVN.FTBchCode AND EVN.FTPosCode = TMPEVN.FTPosCode AND EVN.FTShfCode = TMPEVN.FTShfCode");
                oSql.AppendLine($"   INNER JOIN {tTblEvent} TMPEVN WITH(NOLOCK) ON EVN.FTBchCode = TMPEVN.FTBchCode AND EVN.FTPosCode = TMPEVN.FTPosCode AND EVN.FTShfCode = TMPEVN.FTShfCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTShiftEvent");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTShiftEventTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblEvent} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE BNT ");
                oSql.AppendLine("   FROM TPSTShiftSKeyBN BNT WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTShiftSKeyBNTmp TMPBNT WITH(NOLOCK) ON BNT.FTBchCode = TMPBNT.FTBchCode AND BNT.FTPosCode = TMPBNT.FTPosCode AND BNT.FTShfCode = TMPBNT.FTShfCode");
                oSql.AppendLine($"   INNER JOIN {tTblKeyBN} TMPBNT WITH(NOLOCK) ON BNT.FTBchCode = TMPBNT.FTBchCode AND BNT.FTPosCode = TMPBNT.FTPosCode AND BNT.FTShfCode = TMPBNT.FTShfCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTShiftSKeyBN");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTShiftSKeyBNTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblKeyBN} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE RCV ");
                oSql.AppendLine("   FROM TPSTShiftSKeyRcv RCV WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTShiftSKeyRcvTmp TMPRCV WITH(NOLOCK) ON RCV.FTBchCode = TMPRCV.FTBchCode AND RCV.FTPosCode = TMPRCV.FTPosCode AND RCV.FTShfCode = TMPRCV.FTShfCode");
                oSql.AppendLine($"   INNER JOIN {tTblKeyRcv} TMPRCV WITH(NOLOCK) ON RCV.FTBchCode = TMPRCV.FTBchCode AND RCV.FTPosCode = TMPRCV.FTPosCode AND RCV.FTShfCode = TMPRCV.FTShfCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTShiftSKeyRcv");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTShiftSKeyRcvTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblKeyRcv} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE SLD ");
                oSql.AppendLine("   FROM TPSTShiftSLastDoc SLD WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTShiftSLastDocTmp TMPSLD WITH(NOLOCK) ON SLD.FTBchCode = TMPSLD.FTBchCode AND SLD.FTPosCode = TMPSLD.FTPosCode AND SLD.FTShfCode = TMPSLD.FTShfCode");
                oSql.AppendLine($"   INNER JOIN {tTblLastDoc} TMPSLD WITH(NOLOCK) ON SLD.FTBchCode = TMPSLD.FTBchCode AND SLD.FTPosCode = TMPSLD.FTPosCode AND SLD.FTShfCode = TMPSLD.FTShfCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTShiftSLastDoc");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTShiftSLastDocTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblLastDoc} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE SRP ");
                oSql.AppendLine("   FROM TPSTShiftSRatePdt SRP WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTShiftSRatePdtTmp TMPSRP WITH(NOLOCK) ON SRP.FTBchCode = TMPSRP.FTBchCode AND SRP.FTPosCode = TMPSRP.FTPosCode AND SRP.FTShfCode = TMPSRP.FTShfCode");
                oSql.AppendLine($"   INNER JOIN {tTblRatePdt} TMPSRP WITH(NOLOCK) ON SRP.FTBchCode = TMPSRP.FTBchCode AND SRP.FTPosCode = TMPSRP.FTPosCode AND SRP.FTShfCode = TMPSRP.FTShfCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTShiftSRatePdt");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTShiftSRatePdtTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblRatePdt} WITH(NOLOCK) ");
                oSql.AppendLine();

                oSql.AppendLine("   DELETE SSR ");
                oSql.AppendLine("   FROM TPSTShiftSSumRcv SSR WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTShiftSSumRcvTmp TMPSSR WITH(NOLOCK) ON SSR.FTBchCode = TMPSSR.FTBchCode AND SSR.FTPosCode = TMPSSR.FTPosCode AND SSR.FTShfCode = TMPSSR.FTShfCode");
                oSql.AppendLine($"   INNER JOIN {tTblSumRcv} TMPSSR WITH(NOLOCK) ON SSR.FTBchCode = TMPSSR.FTBchCode AND SSR.FTPosCode = TMPSSR.FTPosCode AND SSR.FTShfCode = TMPSSR.FTShfCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTShiftSSumRcv");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTShiftSSumRcvTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblSumRcv} WITH(NOLOCK) ");
                oSql.AppendLine();

                oSql.AppendLine("   DELETE USL ");
                oSql.AppendLine("   FROM TPSTUsrLog USL WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTUsrLogTmp TMPUSL WITH(NOLOCK) ON USL.FTComName = TMPUSL.FTComName AND USL.FTBchCode = TMPUSL.FTBchCode AND USL.FTPosCode = TMPUSL.FTPosCode AND USL.FTUsrCode = TMPUSL.FTUsrCode AND USL.FDShdSignIn = TMPUSL.FDShdSignIn ");//*Net 63-06-09 
                oSql.AppendLine($"   INNER JOIN {tTblUsrLog} TMPUSL WITH(NOLOCK) ON USL.FTComName = TMPUSL.FTComName AND USL.FTBchCode = TMPUSL.FTBchCode AND USL.FTPosCode = TMPUSL.FTPosCode AND USL.FTUsrCode = TMPUSL.FTUsrCode AND USL.FDShdSignIn = TMPUSL.FDShdSignIn ");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTUsrLog");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT DISTINCT * FROM TPSTUsrLogTmp WITH(NOLOCK) ");//*Net 63-06-09 
                oSql.AppendLine($"   SELECT DISTINCT * FROM {tTblUsrLog} WITH(NOLOCK) ");
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
                oSql.AppendLine("DROP TABLE " + tTblHD + "");
                oSql.AppendLine("DROP TABLE " + tTblDT + "");
                oSql.AppendLine("DROP TABLE " + tTblEvent + "");
                oSql.AppendLine("DROP TABLE " + tTblKeyBN + "");
                oSql.AppendLine("DROP TABLE " + tTblKeyRcv + "");
                oSql.AppendLine("DROP TABLE " + tTblLastDoc + "");
                oSql.AppendLine("DROP TABLE " + tTblRatePdt + "");
                oSql.AppendLine("DROP TABLE " + tTblSumRcv + "");
                oSql.AppendLine("DROP TABLE " + tTblUsrLog + "");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //+++++++++++++

                foreach (cmlTPSTShiftHD oHD in oShift.aoTPSTShiftHD)
                {
                    if (oHD.FDShdSignOut != null)
                    {
                        //Public MQ Process stock
                        cmlRcvSalePos oSale = new cmlRcvSalePos();
                        oSale.ptBchCode = oHD.FTBchCode.ToString();
                        oSale.ptXihDocNo = oHD.FTShfCode.ToString();
                        oSale.ptPosCode = oHD.FTPosCode.ToString();
                        oSale.pnXihDocType = 2; //*Net 63-07-14 type รอบการขาย //* Net 63-08-03 ยกมาจาก Moshi
                        oSale.ptConnStr = poData.ptConnStr;

                        string tMessage = JsonConvert.SerializeObject(oSale);
                        cFunction.C_PRCxMQPublish("POSEJ", tMessage, out ptErrMsg); //*Em 62-09-16
                    }
                }

                //*Arm 63-03-31 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                {

                    //GET fromat Message Json string to API2PSSale
                    //*Em 62-10-18
                    if (oSP.SP_CHKbIsHQBch(poData.ptConnStr, (int)poShopDB.nCommandTimeOut) == false)
                    {
                        string tAPIUrl = "";
                        string tUrlFunc = "/Service/Upload/ShiftSale";
                        string tAPIHeader = "";
                        string tXKey = "";
                        string tBchHQ = "";
                        tBchHQ = oSP.SP_GETtBchHQ(poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                        tAPIUrl = oSP.SP_GETtUrlAPI(poData.ptConnStr, (int)poShopDB.nCommandTimeOut, tBchHQ, 5, ref tXKey, ref tAPIHeader);

                        if (!string.IsNullOrEmpty(tAPIUrl))
                        {
                            string tJSonCall = JsonConvert.SerializeObject(oShift);
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(tAPIHeader, tXKey);
                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tAPIUrl + tUrlFunc, tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("cShift", "C_PRCbUploadShift : " + oEx.Message);
                            }

                            if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                                cmlResResult oRes = JsonConvert.DeserializeObject<cmlResResult>(tJSonRes);
                                if (oRes.rtCode == "001")
                                {
                                    //
                                }
                                else
                                {
                                    new cLog().C_WRTxLog("cShift", "C_PRCbUploadShift/ToHQ : " + oRes.rtMsg);
                                }
                            }
                        }
                    }
                    //+++++++++++++++++++
                }
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadShift");
                return false;
            }
            finally
            {
                aoHD = null;
                aoDT = null;
                aoEvent = null;
                aoKeyBN = null;
                aoKeyRcv = null;
                aoLastDoc = null;
                aoRatePdt = null;
                aoSumRcv = null;
                oSql = null;
                oShift = null;
                oTranscation = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
                new cSP().SP_CLExMemory();
            }
        }
    }
}
