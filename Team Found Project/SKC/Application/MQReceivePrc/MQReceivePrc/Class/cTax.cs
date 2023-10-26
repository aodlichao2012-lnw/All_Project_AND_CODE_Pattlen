using MQReceivePrc.Class;
using MQReceivePrc.Models.Webservice.Response;
using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Tax;
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
    public class cTax
    {
        //* Net 63-08-03 ยกมาจาก Moshi
        //public bool C_PRCbUploadTax(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        public bool C_PRCbUploadTax(string ptQueueID, cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDataReader<cmlTPSTTaxHD> aoHD;
            cDataReader<cmlTPSTTaxHDCst> aoHDCst;
            cDataReader<cmlTPSTTaxHDDis> aoHDDis;
            cDataReader<cmlTPSTTaxDT> aoDT;
            cDataReader<cmlTPSTTaxDTDis> aoDTDis;
            cDataReader<cmlTPSTTaxDTPmt> aoDTPmt;
            cDataReader<cmlTPSTTaxRC> aoRC;
            cDataReader<cmlTCNMTaxAddress> aoADDR;  //*Arm 62-10-09  - Upload TaxAddress
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlTPSTTax oTax;
            SqlTransaction oTranscation;
            SqlConnection oConn;
            cSP oSP = new cSP();
            string tQueueID = "";

            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oTax = JsonConvert.DeserializeObject<cmlTPSTTax>(poData.ptData);

                tQueueID = ptQueueID.Replace("-", "");

                //*Arm 63-05-30
                string tTblTaxHD = "TSTHD" + tQueueID;
                string tTblTaxHDCst = "TSTHDCst" + tQueueID;
                string tTblTaxHDDis = "TSTHDDis" + tQueueID;
                string tTblTaxDT = "TSTDT" + tQueueID;
                string tTblTaxDTDis = "TSTDTDis" + tQueueID;
                string tTblTaxDTPmt = "TSTDTPmt" + tQueueID;
                string tTblTaxRC = "TSTRC" + tQueueID;
                string tTblTaxADDR = "TSTAddr" + tQueueID;
                //+++++++++++

                //Create Temp
                #region Create Temp
                #region Comment
                //HD
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxHDTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxHDTmp FROM TPSTTaxHD with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHD' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxHDTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxHDTmp FROM TPSTTaxHD with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxHDTmp");
                oSql.AppendLine("");
                //HDCst
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxHDCstTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxHDCstTmp FROM TPSTTaxHDCst with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDCstTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDCst' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxHDCstTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxHDCstTmp FROM TPSTTaxHDCst with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxHDCstTmp");
                oSql.AppendLine("");
                //HDDis
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxHDDisTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxHDDisTmp FROM TPSTTaxHDDis with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDDis' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxHDDisTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxHDDisTmp FROM TPSTTaxHDDis with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxHDDisTmp");
                oSql.AppendLine("");
                //DT
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxDTTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxDTTmp FROM TPSTTaxDT with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDT' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxDTTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxDTTmp FROM TPSTTaxDT with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxDTTmp");
                oSql.AppendLine("");
                //DTDis
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxDTDisTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxDTDisTmp FROM TPSTTaxDTDis with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTDis' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxDTDisTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxDTDisTmp FROM TPSTTaxDTDis with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxDTDisTmp");
                oSql.AppendLine("");
                //DTPmt
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxDTPmtTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxDTPmtTmp FROM TPSTTaxDTPmt with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTPmtTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTPmt' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxDTPmtTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxDTPmtTmp FROM TPSTTaxDTPmt with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxDTPmtTmp");
                oSql.AppendLine("");
                //RC
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxRCTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxRCTmp FROM TPSTTaxRC with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxRCTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxRC' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxRCTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxRCTmp FROM TPSTTaxRC with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxRCTmp");
                oSql.AppendLine("");
                //*Arm 62-10-09  - Upload TaxAddress
                //ADDR  
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TCNMTaxAddress_LTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TCNMTaxAddress_LTmp FROM TCNMTaxAddress_L with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNMTaxAddress_LTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNMTaxAddress_L' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TCNMTaxAddress_LTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TCNMTaxAddress_LTmp FROM TCNMTaxAddress_L with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TCNMTaxAddress_LTmp");
                oSql.AppendLine("");

                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion

                //* Net 63-08-03 ยกมาจาก Moshi
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxHD", tTblTaxHD, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxHDCst", tTblTaxHDCst, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxHDDis", tTblTaxHDDis, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxDT", tTblTaxDT, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxDTDis", tTblTaxDTDis, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxDTPmt", tTblTaxDTPmt, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxRC", tTblTaxRC, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TCNMTaxAddress_L", tTblTaxADDR, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);

                #endregion Create Temp

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTranscation = oConn.BeginTransaction();

                //insert to DB
                if (oTax.aoTPSTTaxHD != null)
                {
                    aoHD = new cDataReader<cmlTPSTTaxHD>(oTax.aoTPSTTaxHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxHD}"; //* Net 63-08-03 ยกมาจาก Moshi

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

                if (oTax.aoTPSTTaxHDCst != null)
                {
                    aoHDCst = new cDataReader<cmlTPSTTaxHDCst>(oTax.aoTPSTTaxHDCst);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDCst.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDCstTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxHDCst}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoHDCst);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oTax.aoTPSTTaxHDDis != null)
                {
                    aoHDDis = new cDataReader<cmlTPSTTaxHDDis>(oTax.aoTPSTTaxHDDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDDisTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxHDDis}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoHDDis);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oTax.aoTPSTTaxDT != null)
                {
                    aoDT = new cDataReader<cmlTPSTTaxDT>(oTax.aoTPSTTaxDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxDT}"; //* Net 63-08-03 ยกมาจาก Moshi

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

                if (oTax.aoTPSTTaxDTDis != null)
                {
                    aoDTDis = new cDataReader<cmlTPSTTaxDTDis>(oTax.aoTPSTTaxDTDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTDisTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxDTDis}"; //* Net 63-08-03 ยกมาจาก Moshi

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

                if (oTax.aoTPSTTaxDTPmt != null)
                {
                    aoDTPmt = new cDataReader<cmlTPSTTaxDTPmt>(oTax.aoTPSTTaxDTPmt);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTPmt.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTPmtTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxDTPmt}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoDTPmt);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oTax.aoTPSTTaxRC != null)
                {
                    aoRC = new cDataReader<cmlTPSTTaxRC>(oTax.aoTPSTTaxRC);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoRC.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxRCTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxRC}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoRC);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                //*Arm 62-10-09  - Upload TaxAddress
                if (oTax.aoTCNMTaxAddress != null)
                {
                    aoADDR = new cDataReader<cmlTCNMTaxAddress>(oTax.aoTCNMTaxAddress);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoADDR.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TCNMTaxAddress_LTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxADDR}"; //* Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoADDR);
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
                oSql.AppendLine("   FROM TPSTTaxHD HD WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTTaxHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxHD} TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxHD");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTTaxHDTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxHD} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDCst ");
                oSql.AppendLine("   FROM TPSTTaxHDCst HDCst WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTTaxHDCstTmp TMPCst WITH(NOLOCK) ON HDCst.FTBchCode = TMPCst.FTBchCode AND HDCst.FTXshDocNo = TMPCst.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxHDCst} TMPCst WITH(NOLOCK) ON HDCst.FTBchCode = TMPCst.FTBchCode AND HDCst.FTXshDocNo = TMPCst.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxHDCst");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTTaxHDCstTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxHDCst} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDDis ");
                oSql.AppendLine("   FROM TPSTTaxHDDis HDDis WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTTaxHDDisTmp TMPHDDis WITH(NOLOCK) ON HDDis.FTBchCode = TMPHDDis.FTBchCode AND HDDis.FTXshDocNo = TMPHDDis.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxHDDis} TMPHDDis WITH(NOLOCK) ON HDDis.FTBchCode = TMPHDDis.FTBchCode AND HDDis.FTXshDocNo = TMPHDDis.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxHDDis");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTTaxHDDisTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxHDDis} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TPSTTaxDT DT WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTTaxDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxDT} TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxDT");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTTaxDTTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxDT} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTDis ");
                oSql.AppendLine("   FROM TPSTTaxDTDis DTDis WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTTaxDTDisTmp TMPDTDis WITH(NOLOCK) ON DTDis.FTBchCode = TMPDTDis.FTBchCode AND DTDis.FTXshDocNo = TMPDTDis.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxDTDis} TMPDTDis WITH(NOLOCK) ON DTDis.FTBchCode = TMPDTDis.FTBchCode AND DTDis.FTXshDocNo = TMPDTDis.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxDTDis");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTTaxDTDisTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxDTDis} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTPmt ");
                oSql.AppendLine("   FROM TPSTTaxDTPmt DTPmt WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTTaxDTPmtTmp TMPDTPmt WITH(NOLOCK) ON DTPmt.FTBchCode = TMPDTPmt.FTBchCode AND DTPmt.FTXshDocNo = TMPDTPmt.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxDTPmt} TMPDTPmt WITH(NOLOCK) ON DTPmt.FTBchCode = TMPDTPmt.FTBchCode AND DTPmt.FTXshDocNo = TMPDTPmt.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxDTPmt");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTTaxDTPmtTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxDTPmt} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE RC ");
                oSql.AppendLine("   FROM TPSTTaxRC RC WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTTaxRCTmp TMPRC WITH(NOLOCK) ON RC.FTBchCode = TMPRC.FTBchCode AND RC.FTXshDocNo = TMPRC.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxRC} TMPRC WITH(NOLOCK) ON RC.FTBchCode = TMPRC.FTBchCode AND RC.FTXshDocNo = TMPRC.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxRC");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTTaxRCTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxRC} WITH(NOLOCK) ");
                oSql.AppendLine();
                //*Arm 62-10-09  - Upload TaxAddress
                oSql.AppendLine("   DELETE ADDR ");
                oSql.AppendLine("   FROM TCNMTaxAddress_L ADDR WITH(ROWLOCK)");
                //* Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TCNMTaxAddress_LTmp TMPADDR WITH(NOLOCK) ON ADDR.FTAddTaxNo = TMPADDR.FTAddTaxNo AND ADDR.FNLngID = TMPADDR.FNLngID");
                oSql.AppendLine($"   INNER JOIN {tTblTaxADDR} TMPADDR WITH(NOLOCK) ON ADDR.FTAddTaxNo = TMPADDR.FTAddTaxNo AND ADDR.FNLngID = TMPADDR.FNLngID");
                oSql.AppendLine();
                //oSql.AppendLine("   INSERT INTO TCNMTaxAddress_L");
                //oSql.AppendLine("   SELECT * FROM TCNMTaxAddress_LTmp WITH(NOLOCK) ");
                oSql.AppendLine("   INSERT INTO TCNMTaxAddress_L(FTAddTaxNo,FNLngID,FTAddRefNo,FTAddName,FTAddRmk,FTAddCountry,FTAreCode,FTZneCode,FTAddVersion,FTAddV1No,FTAddV1Soi,FTAddV1Village,FTAddV1Road,FTAddV1SubDist,FTAddV1DstCode,FTAddV1PvnCode,FTAddV1PostCode,FTAddV2Desc1,FTAddV2Desc2,FTAddWebsite,FTAddLongitude,FTAddLatitude,FTAddStaBusiness,FTAddStaHQ,FTAddStaBchCode,FTAddTel,FTAddFax,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");   //*Em 62-12-18
                oSql.AppendLine("   SELECT FTAddTaxNo,FNLngID,FTAddRefNo,FTAddName,FTAddRmk,FTAddCountry,FTAreCode,FTZneCode,FTAddVersion,FTAddV1No,FTAddV1Soi,FTAddV1Village,FTAddV1Road,FTAddV1SubDist,FTAddV1DstCode,FTAddV1PvnCode,FTAddV1PostCode,FTAddV2Desc1,FTAddV2Desc2,FTAddWebsite,FTAddLongitude,FTAddLatitude,FTAddStaBusiness,FTAddStaHQ,FTAddStaBchCode,FTAddTel,FTAddFax,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy ");  //*Em 62-12-18
                //oSql.AppendLine("   FROM TCNMTaxAddress_LTmp WITH(NOLOCK) "); //*Em 62-12-18
                //* Net 63-08-03 ยกมาจาก Moshi
                oSql.AppendLine($"   FROM {tTblTaxADDR} WITH(NOLOCK) ");
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
                oSql.AppendLine("DROP TABLE " + tTblTaxHD + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxHDCst + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxHDDis + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxDT + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxDTDis + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxDTPmt + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxRC + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxADDR + "");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //+++++++++++++

                //*Arm 63-03-31 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                {

                    //*Em 62-10-18
                    if (oSP.SP_CHKbIsHQBch(poData.ptConnStr, (int)poShopDB.nCommandTimeOut) == false)
                    {
                        string tAPIUrl = "";
                        string tUrlFunc = "/Service/Upload/Tax";
                        string tAPIHeader = "";
                        string tXKey = "";
                        string tBchHQ = "";
                        tBchHQ = oSP.SP_GETtBchHQ(poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                        tAPIUrl = oSP.SP_GETtUrlAPI(poData.ptConnStr, (int)poShopDB.nCommandTimeOut, tBchHQ, 5, ref tXKey, ref tAPIHeader);

                        if (!string.IsNullOrEmpty(tAPIUrl))
                        {
                            string tJSonCall = JsonConvert.SerializeObject(oTax);
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(tAPIHeader, tXKey);
                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tAPIUrl + tUrlFunc, tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("cTax", "C_PRCbUploadTax : " + oEx.Message);
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
                                    new cLog().C_WRTxLog("cTax", "C_PRCbUploadTax/ToHQ : " + oRes.rtMsg);
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
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadTax");
                return false;
            }
            finally
            {
                aoHD = null;
                aoHDCst = null;
                aoHDDis = null;
                aoDT = null;
                aoDTDis = null;
                aoDTPmt = null;
                aoRC = null;
                aoADDR = null;
                oSql = null;
                oTax = null;
                oTranscation = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
                new cSP().SP_CLExMemory();
            }
        }
    }
}
