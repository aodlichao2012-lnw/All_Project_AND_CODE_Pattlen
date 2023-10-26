using MQReceivePrc.Class;
using MQReceivePrc.Models.Webservice.Response;
using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Pos;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Sale;
using MQReceivePrc.Models.WebService.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MQReceivePrc.Models.WebService.Request;
using MQReceivePrc.Models.ExportSale;

namespace MQReceivePrc.Class
{
    public class cSale
    {
        private cmlRcvSalePos oC_SalePos;
        private cmlShopDB oC_ShopDB;
        private cmlTCNMPos oC_Pos;
        // Net 63-08-03 ยกมาจาก Moshi
        //public bool C_PRCbUploadSale(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        public bool C_PRCbUploadSale(string ptQueueID, cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDataReader<cmlTPSTSalHD> aoHD;
            cDataReader<cmlTPSTSalHDCst> aoHDCst;
            cDataReader<cmlTPSTSalHDDis> aoHDDis;
            cDataReader<cmlTPSTSalDT> aoDT;
            cDataReader<cmlTPSTSalDTDis> aoDTDis;
            cDataReader<cmlTPSTSalDTPmt> aoDTPmt;
            cDataReader<cmlTPSTSalRC> aoRC;
            cDataReader<cmlTPSTSalRD> aoRD;
            cDataReader<cmlTPSTSalPD> aoPD;
            cDataReader<cmlTCNTMemTxnSale> aoTxnSale;        //*Arm 63-05-07
            cDataReader<cmlTCNTMemTxnRedeem> aoTxnRedeem;   //*Arm 63-05-07

            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlTPSTSal oSalePos;
            SqlTransaction oTranscation;
            SqlConnection oConn;
            cSP oSP = new cSP();

            string tQueueID = ""; // Net 63-08-03 ยกมาจาก Moshi
            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oSalePos = JsonConvert.DeserializeObject<cmlTPSTSal>(poData.ptData);

                // Net 63-08-03 ยกมาจาก Moshi
                tQueueID = ptQueueID.Replace("-", "");

                //*Arm 63-05-30
                string tTblSalHD = "TSHD" + tQueueID;
                string tTblSalHDCst = "TSHDCst" + tQueueID;
                string tTblSalHDDis = "TSHDDis" + tQueueID;
                string tTblSalDT = "TSDT" + tQueueID;
                string tTblSalDTDis = "TSDTDis" + tQueueID;
                string tTblSalRD = "TSRD" + tQueueID;
                string tTblSalRC = "TSRC" + tQueueID;
                string tTblSalPD = "TSPD" + tQueueID;
                string tTblTxnSale = "TTxnSale" + tQueueID;
                string tTblTxnRedeem = "TTxnRedeem" + tQueueID;
                //+++++++++++// Net 63-08-03 ยกมาจาก Moshi


                //Create Temp
                #region Create Temp
                #region Comment

                //HD
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalHDTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalHDTmp FROM TPSTSalHD with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHD' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalHDTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalHDTmp FROM TPSTSalHD with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalHDTmp");
                //oSql.AppendLine("");
                ////HDCst
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalHDCstTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalHDCstTmp FROM TPSTSalHDCst with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDCstTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDCst' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalHDCstTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalHDCstTmp FROM TPSTSalHDCst with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalHDCstTmp");
                //oSql.AppendLine("");
                ////HDDis
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalHDDisTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalHDDisTmp FROM TPSTSalHDDis with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDDis' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalHDDisTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalHDDisTmp FROM TPSTSalHDDis with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalHDDisTmp");
                //oSql.AppendLine("");
                ////DT
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalDTTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalDTTmp FROM TPSTSalDT with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDT' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalDTTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalDTTmp FROM TPSTSalDT with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalDTTmp");
                //oSql.AppendLine("");
                ////DTDis
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalDTDisTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalDTDisTmp FROM TPSTSalDTDis with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTDis' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalDTDisTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalDTDisTmp FROM TPSTSalDTDis with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalDTDisTmp");
                //oSql.AppendLine("");
                ////DTPmt
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalDTPmtTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalDTPmtTmp FROM TPSTSalDTPmt with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTPmtTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTPmt' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalDTPmtTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalDTPmtTmp FROM TPSTSalDTPmt with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalDTPmtTmp");
                //oSql.AppendLine("");
                ////RC
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalRCTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalRCTmp FROM TPSTSalRC with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalRCTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalRC' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalRCTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalRCTmp FROM TPSTSalRC with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalRCTmp");
                //oSql.AppendLine("");
                ////RD
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalRDTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalRDTmp FROM TPSTSalRD with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalRDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalRD' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalRDTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalRDTmp FROM TPSTSalRD with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalRDTmp");
                //oSql.AppendLine("");

                ////PD *Arm 63-03-26
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTSalPDTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTSalPDTmp FROM TPSTSalPD with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalPDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalPD' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TPSTSalPDTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTSalPDTmp FROM TPSTSalPD with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TPSTSalPDTmp");
                //oSql.AppendLine("");
                ////+++++++++++++++

                ////*Arm 63-05-07
                ////TxnSale 
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TCNTMemTxnRedeemTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TCNTMemTxnRedeemTmp FROM TCNTMemTxnRedeem with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTMemTxnRedeemTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTMemTxnRedeem' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TCNTMemTxnRedeemTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TCNTMemTxnRedeemTmp FROM TCNTMemTxnRedeem with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TCNTMemTxnRedeemTmp");
                //oSql.AppendLine("");

                ////TxnSale
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TCNTMemTxnSaleTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TCNTMemTxnSaleTmp FROM TCNTMemTxnSale with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTMemTxnSaleTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTMemTxnSale' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TCNTMemTxnSaleTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TCNTMemTxnSaleTmp FROM TCNTMemTxnSale with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TCNTMemTxnSaleTmp");
                //oSql.AppendLine("");
                ////+++++++++++++++
                //oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion

                //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalHD", tTblSalHD, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalHDCst", tTblSalHDCst, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalHDDis", tTblSalHDDis, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalDT", tTblSalDT, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalDTDis", tTblSalDTDis, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalRC", tTblSalRC, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalRD", tTblSalRD, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalPD", tTblSalPD, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TCNTMemTxnSale", tTblTxnSale, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TCNTMemTxnRedeem", tTblTxnRedeem, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                #endregion

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTranscation = oConn.BeginTransaction();

                //insert to DB
                if (oSalePos.aoTPSTSalHD != null)
                {
                    aoHD = new cDataReader<cmlTPSTSalHD>(oSalePos.aoTPSTSalHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalHDTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHD; //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoHD);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalHD + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalHDCst != null)
                {
                    aoHDCst = new cDataReader<cmlTPSTSalHDCst>(oSalePos.aoTPSTSalHDCst);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDCst.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalHDCstTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHDCst; //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoHDCst);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalHDCst + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalHDDis != null)
                {
                    aoHDDis = new cDataReader<cmlTPSTSalHDDis>(oSalePos.aoTPSTSalHDDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalHDDisTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHDDis; //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoHDDis);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalHDDis + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalDT != null)
                {
                    aoDT = new cDataReader<cmlTPSTSalDT>(oSalePos.aoTPSTSalDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalDTTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalDT;  // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoDT);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalDT + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalDTDis != null)
                {
                    aoDTDis = new cDataReader<cmlTPSTSalDTDis>(oSalePos.aoTPSTSalDTDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalDTDisTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalDTDis; //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoDTDis);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalDTDis + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                //*Net 63-08-03 ไม่ใช้ตารางแล้ว
                //if (oSalePos.aoTPSTSalDTPmt != null)
                //{
                //    aoDTPmt = new cDataReader<cmlTPSTSalDTPmt>(oSalePos.aoTPSTSalDTPmt);

                //    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                //    {
                //        foreach (string tColName in aoDTPmt.ColumnNames)
                //        {
                //            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                //        }

                //        oBulkCopy.BatchSize = 100;
                //        oBulkCopy.DestinationTableName = "dbo.TPSTSalDTPmtTmp";

                //        try
                //        {
                //            oBulkCopy.WriteToServer(aoDTPmt);
                //        }
                //        catch (Exception oEx)
                //        {
                //            oTranscation.Rollback();
                //            ptErrMsg = oEx.Message.ToString();
                //            return false;
                //        }
                //    }
                //}

                if (oSalePos.aoTPSTSalRC != null)
                {
                    aoRC = new cDataReader<cmlTPSTSalRC>(oSalePos.aoTPSTSalRC);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoRC.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalRCTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalRC; // *Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoRC);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalRC + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalRD != null)
                {
                    aoRD = new cDataReader<cmlTPSTSalRD>(oSalePos.aoTPSTSalRD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoRD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalRDTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalRD; //*Arm 63-05-30

                        try
                        {
                            oBulkCopy.WriteToServer(aoRD);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalRD + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                //*Arm 63-03-26
                if (oSalePos.aoTPSTSalPD != null)
                {
                    aoPD = new cDataReader<cmlTPSTSalPD>(oSalePos.aoTPSTSalPD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoPD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalPDTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalPD; //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoPD);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalPD + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                //*Arm 63-05-07
                if (oSalePos.aoTCNTMemTxnSale != null)
                {
                    aoTxnSale = new cDataReader<cmlTCNTMemTxnSale>(oSalePos.aoTCNTMemTxnSale);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoTxnSale.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TCNTMemTxnSaleTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblTxnSale; //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoTxnSale);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblTxnSale + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTCNTMemTxnRedeem != null)
                {
                    aoTxnRedeem = new cDataReader<cmlTCNTMemTxnRedeem>(oSalePos.aoTCNTMemTxnRedeem);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoTxnRedeem.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TCNTMemTxnRedeemTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblTxnRedeem; //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi

                        try
                        {
                            oBulkCopy.WriteToServer(aoTxnRedeem);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            // Net 63-08-03 ยกมาจาก Moshi
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblTxnRedeem + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }
                //++++++++++++++++++
                oTranscation.Commit();

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                // *Arm 62-12-24 Update FTXshStaPrcStk กรณีส่งซ้ำ
                oSql.AppendLine("   UPDATE THDTMP SET ");
                oSql.AppendLine("   THDTMP.FTXshStaPrcStk = THD.FTXshStaPrcStk,");
                oSql.AppendLine("   THDTMP.FTXshDocVatFull = THD.FTXshDocVatFull"); //*Arm 63-05-26 // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   FROM TPSTSalHDTmp THDTMP WITH(ROWLOCK)");
                oSql.AppendLine("   FROM " + tTblSalHD + " THDTMP WITH(ROWLOCK)"); // *Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi
                oSql.AppendLine("   INNER JOIN TPSTSalHD THD  WITH(NOLOCK) ON THDTMP.FTBchCode = THD.FTBchCode AND THDTMP.FTXshDocNo = THD.FTXshDocNo");
                oSql.AppendLine("   WHERE THDTMP.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND THDTMP.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "' "); //*Arm 63-05-30  // Net 63-08-03 ยกมาจาก Moshi

                oSql.AppendLine("   UPDATE TDTTMP SET ");
                oSql.AppendLine("   TDTTMP.FTXsdStaPrcStk = TDT.FTXsdStaPrcStk");
                //oSql.AppendLine("   FROM TPSTSalDTTmp TDTTMP WITH(ROWLOCK)");
                oSql.AppendLine("   FROM " + tTblSalDT + " TDTTMP WITH(ROWLOCK)"); //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi
                oSql.AppendLine("   INNER JOIN TPSTSalDT TDT WITH(NOLOCK) ON TDTTMP.FTBchCode = TDT.FTBchCode AND TDTTMP.FTXshDocNo = TDT.FTXshDocNo");
                oSql.AppendLine("   WHERE TDTTMP.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND TDTTMP.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30  // Net 63-08-03 ยกมาจาก Moshi
                //--------------------------------------------

                oSql.AppendLine("   DELETE HD ");
                oSql.AppendLine("   FROM TPSTSalHD HD WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTSalHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine("   WHERE HD.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND HD.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalHD");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTSalHDTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalHD + " HDTmp WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDCst ");
                oSql.AppendLine("   FROM TPSTSalHDCst HDCst WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTSalHDCstTmp TMPCst WITH(NOLOCK) ON HDCst.FTBchCode = TMPCst.FTBchCode AND HDCst.FTXshDocNo = TMPCst.FTXshDocNo");
                oSql.AppendLine("   WHERE HDCst.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND HDCst.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalHDCst");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTSalHDCstTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalHDCst + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDDis ");
                oSql.AppendLine("   FROM TPSTSalHDDis HDDis WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTSalHDDisTmp TMPHDDis WITH(NOLOCK) ON HDDis.FTBchCode = TMPHDDis.FTBchCode AND HDDis.FTXshDocNo = TMPHDDis.FTXshDocNo");
                oSql.AppendLine("   WHERE HDDis.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND HDDis.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalHDDis");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTSalHDDisTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalHDDis + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TPSTSalDT DT WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTSalDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo");
                oSql.AppendLine("   WHERE DT.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND DT.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalDT");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTSalDTTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalDT + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTDis ");
                oSql.AppendLine("   FROM TPSTSalDTDis DTDis WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTSalDTDisTmp TMPDTDis WITH(NOLOCK) ON DTDis.FTBchCode = TMPDTDis.FTBchCode AND DTDis.FTXshDocNo = TMPDTDis.FTXshDocNo");
                oSql.AppendLine("   WHERE DTDis.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND DTDis.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalDTDis");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTSalDTDisTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalDTDis + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                //oSql.AppendLine("   DELETE DTPmt ");
                //oSql.AppendLine("   FROM TPSTSalDTPmt DTPmt WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalDTPmtTmp TMPDTPmt WITH(NOLOCK) ON DTPmt.FTBchCode = TMPDTPmt.FTBchCode AND DTPmt.FTXshDocNo = TMPDTPmt.FTXshDocNo");
                //oSql.AppendLine();
                //oSql.AppendLine("   INSERT INTO TPSTSalDTPmt");
                //oSql.AppendLine("   SELECT * FROM TPSTSalDTPmtTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE RC ");
                // Net 63-08-03 ยกมาจาก Moshi
                oSql.AppendLine("   FROM TPSTSalRC RC WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalRCTmp TMPRC WITH(NOLOCK) ON RC.FTBchCode = TMPRC.FTBchCode AND RC.FTXshDocNo = TMPRC.FTXshDocNo");
                oSql.AppendLine("   WHERE RC.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND RC.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalRC");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTSalRCTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalRC + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                //*Arm 63-03-13
                oSql.AppendLine("   DELETE RD ");
                oSql.AppendLine("   FROM TPSTSalRD RD WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTSalRDTmp TMPRD WITH(NOLOCK) ON RD.FTBchCode = TMPRD.FTBchCode AND RD.FTXshDocNo = TMPRD.FTXshDocNo AND RD.FNXrdSeqNo = TMPRD.FNXrdSeqNo");
                oSql.AppendLine("   WHERE RD.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND RD.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalRD");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTSalRDTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalRD + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                //++++++++++++++
                //*Arm 63-03-26 SalePD
                oSql.AppendLine("   DELETE PD ");
                oSql.AppendLine("   FROM TPSTSalPD PD WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TPSTSalPDTmp TMPPD WITH(NOLOCK) ON PD.FTBchCode = TMPPD.FTBchCode AND PD.FTXshDocNo = TMPPD.FTXshDocNo AND PD.FNXsdSeqNo = TMPPD.FNXsdSeqNo");
                //oSql.AppendLine("   AND PD.FTPmhDocNo = TMPPD.FTPmhDocNo AND PD.FTPmdGrpName = TMPPD.FTPmdGrpName ");
                oSql.AppendLine("   WHERE PD.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND PD.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalPD");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TPSTSalPDTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalPD + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                //++++++++++++++
                //*Arm 63-03-26 TxnSale
                oSql.AppendLine("   DELETE TSAL ");
                oSql.AppendLine("   FROM TCNTMemTxnSale TSAL WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TCNTMemTxnSaleTmp TMPTSAL WITH(NOLOCK) ON TSAL.FTCgpCode = TMPTSAL.FTCgpCode AND TSAL.FTMemCode = TMPTSAL.FTMemCode");
                oSql.AppendLine("   INNER JOIN " + tTblTxnSale + " TMPTSAL WITH(NOLOCK) ON TSAL.FTCgpCode = TMPTSAL.FTCgpCode AND TSAL.FTMemCode = TMPTSAL.FTMemCode");
                oSql.AppendLine("   AND TSAL.FTTxnRefDoc = TMPTSAL.FTTxnRefDoc AND TSAL.FTTxnRefInt = TMPTSAL.FTTxnRefInt AND TSAL.FTTxnRefSpl = TMPTSAL.FTTxnRefSpl");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TCNTMemTxnSale");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TCNTMemTxnSaleTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblTxnSale + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine();

                oSql.AppendLine("   DELETE TRD ");
                oSql.AppendLine("   FROM TCNTMemTxnRedeem TRD WITH(ROWLOCK)");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   INNER JOIN TCNTMemTxnRedeemTmp TMPTRD WITH(NOLOCK) ON TRD.FTCgpCode = TMPTRD.FTCgpCode AND TRD.FTMemCode = TMPTRD.FTMemCode");
                oSql.AppendLine("   INNER JOIN " + tTblTxnRedeem + " TMPTRD WITH(NOLOCK) ON TRD.FTCgpCode = TMPTRD.FTCgpCode AND TRD.FTMemCode = TMPTRD.FTMemCode");
                oSql.AppendLine("   AND TRD.FTRedRefDoc = TMPTRD.FTRedRefDoc AND TRD.FTRedRefInt = TMPTRD.FTRedRefInt ");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TCNTMemTxnRedeem");
                // Net 63-08-03 ยกมาจาก Moshi
                //oSql.AppendLine("   SELECT * FROM TCNTMemTxnRedeemTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblTxnRedeem + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine();
                //++++++++++++++

                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);


                //// Net 63-08-03 ยกมาจาก Moshi
                ////*Arm 63-05-30 Drop Temp
                //oSql.Clear();
                //oSql.AppendLine("DROP TABLE " + tTblSalHD + "");
                //oSql.AppendLine("DROP TABLE " + tTblSalHDCst + "");
                //oSql.AppendLine("DROP TABLE " + tTblSalHDDis + "");
                //oSql.AppendLine("DROP TABLE " + tTblSalDT + "");
                //oSql.AppendLine("DROP TABLE " + tTblSalDTDis + "");
                //oSql.AppendLine("DROP TABLE " + tTblSalRC + "");
                //oSql.AppendLine("DROP TABLE " + tTblSalRD + "");
                //oSql.AppendLine("DROP TABLE " + tTblSalPD + "");
                //oSql.AppendLine("DROP TABLE " + tTblTxnSale + "");
                //oSql.AppendLine("DROP TABLE " + tTblTxnRedeem + "");
                //oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                ////+++++++++++++

                //*Arm 63-04-08 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == true) // ใช้งานระบบ Centralized
                {
                    foreach (cmlTPSTSalHD oHD in oSalePos.aoTPSTSalHD)
                    {
                        //Public MQ Process stock
                        cmlRcvSalePos oSale = new cmlRcvSalePos();
                        oSale.ptBchCode = oHD.FTBchCode.ToString();
                        oSale.ptXihDocNo = oHD.FTXshDocNo.ToString();
                        oSale.ptPosCode = oHD.FTPosCode.ToString();
                        oSale.pnXihDocType = 1; //*Net 63-07-14 เพิ่ม type การขาย/คืน // Net 63-08-03 ยกมาจาก Moshi
                        oSale.ptConnStr = poData.ptConnStr;

                        string tMessage = JsonConvert.SerializeObject(oSale);

                        //*Arm 63-08-04 
                        if (oSalePos.ptWahStaPrcStk == "2")  // คลังแบบตัด stock
                        {
                            string tStaPrcStk = "";
                            
                            //  check เคยตัด stock หรือยัง
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(FTXshStaPrcStk,'') AS FTXshStaPrcStk FROM " + tTblSalHD + " WHERE FTBchCode = '"+ oHD.FTBchCode.ToString() + "' AND FTXshDocNo = '"+ oHD.FTXshDocNo.ToString() + "' ");
                            tStaPrcStk = oDB.C_DAToExecuteQuery<string>(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                            if (tStaPrcStk != "1")  // ยังไม่ตัด stock
                            {
                                cFunction.C_PRCxMQPublish("SALEPOS", tMessage, out ptErrMsg);
                            }
                        }
                        //+++++++++++++

                        cFunction.C_PRCxMQPublish("POSEJ", tMessage, out ptErrMsg); //*Em 62-09-16

                        //*Arm 63-05-17 ปิดการใช้งาน X_QSalePS2Touch // Net 63-08-03 ยกมาจาก Moshi
                        ////*Arm 62-11-06
                        //cmlRcvSalePS2Touch oSalePS2Touch = new cmlRcvSalePS2Touch();
                        //oSalePS2Touch.ptFunction = "SalePos";
                        //oSalePS2Touch.ptSource = "MQReceivePrc";
                        //oSalePS2Touch.ptDest = "MQAdaLink";
                        //oSalePS2Touch.ptData = new cmlData();
                        //oSalePS2Touch.ptData.ptDocNo = oHD.FTXshDocNo.ToString();

                        //string tMsgSalePS2Touch = JsonConvert.SerializeObject(oSalePS2Touch);

                        //cFunction.C_PRCxMQPublish("X_QSalePS2Touch", tMsgSalePS2Touch, out ptErrMsg);
                        ////++++++++++

                        //*Arm 63-02-18 - สำหรับ Update SO
                        if (!string.IsNullOrEmpty(oHD.FTXshRefExt)) // Check DocType = 1 :บิลขาย และ มีการอ้างอิง SO (FNXshDocType) 
                        {
                            if (oHD.FNXshDocType.ToString() == "1")
                            {
                                oSql = new StringBuilder();
                                oSql.AppendLine("UPDATE TARTSoHD WITH(ROWLOCK) SET ");
                                oSql.AppendLine("FTXshRefInt = '" + oHD.FTXshDocNo + "',");
                                oSql.AppendLine("FDXshRefIntDate = '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "',");
                                oSql.AppendLine("FNXshStaRef ='2'");
                                oSql.AppendLine("WHERE FTXshDocNo ='" + oHD.FTXshRefExt + "'");
                                oSql.AppendLine("AND FTBchCode = '" + oHD.FTBchCode + "'");

                                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                            }
                        }
                        //*Net 63-05-13 // Net 63-08-03 ยกมาจาก Moshi //ปิด ไม่ต้องส่ง realtime Monitor
                        if (oHD != null)
                        {
                            //cmlReqMonitorSrv oReqMonitor = new cmlReqMonitorSrv();
                            //oReqMonitor.tBchCode = oHD.FTBchCode.ToString();
                            //oReqMonitor.tPosCode = oHD.FTPosCode.ToString();
                            //oReqMonitor.tType = "6";
                            //oReqMonitor.cPosGrand = oHD.FCXshGrand.Value;
                            //if (oHD.FNXshDocType.Value == 9)
                            //{
                            //    oReqMonitor.cPosGrand = -oHD.FCXshGrand.Value;
                            //}
                            //oReqMonitor.tPosLastDocNo = oHD.FTXshDocNo;
                            //oReqMonitor.tPosLastSync = oHD.FDLastUpdOn.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            //oReqMonitor.tDateRequest = oHD.FDXshDocDate.Value.ToString("yyyy-MM-dd");
                            //string tMsgReqMonitor = JsonConvert.SerializeObject(oReqMonitor);
                            //cFunction.C_PRCxMQPublishExchange("AR_XPepairingSale", oReqMonitor.tBchCode + oReqMonitor.tPosCode, "direct", tMsgReqMonitor, out ptErrMsg);
                        }
                    }
                    // Net 63-08-03 Comment from Moshi
                    //*Net 63-05-13
                    //if (oSalePos.aoTPSTSalHD[0] != null)
                    //{
                    //    cmlReqMonitorSrv oReqMonitor = new cmlReqMonitorSrv();
                    //    oReqMonitor.tBchCode = oSalePos.aoTPSTSalHD[0].FTBchCode.ToString();
                    //    oReqMonitor.tPosCode = oSalePos.aoTPSTSalHD[0].FTPosCode.ToString();
                    //    oReqMonitor.tType = "6";
                    //    oReqMonitor.cPosGrand = oSalePos.aoTPSTSalHD[0].FCXshGrand.Value;
                    //    if (oSalePos.aoTPSTSalHD[0].FNXshDocType.Value == 9)
                    //    {
                    //        oReqMonitor.cPosGrand = -oSalePos.aoTPSTSalHD[0].FCXshGrand.Value;
                    //    }
                    //    oReqMonitor.tPosLastSync = oSalePos.aoTPSTSalHD[0].FDLastUpdOn.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    //    oReqMonitor.tDateRequest = oSalePos.aoTPSTSalHD[0].FDXshDocDate.Value.ToString("yyyy-MM-dd");
                    //    string tMsgReqMonitor = JsonConvert.SerializeObject(oReqMonitor);
                    //    cFunction.C_PRCxMQPublishExchange("AR_XPepairingSale", oReqMonitor.tBchCode + oReqMonitor.tPosCode, "direct", tMsgReqMonitor, out ptErrMsg);
                    //}

                }
                else
                {
                    //*ไม่ใช้งานระบบ Centralized
                    foreach (cmlTPSTSalHD oHD in oSalePos.aoTPSTSalHD)
                    {
                        if (oHD.FTBchCode == cVB.tVB_BchCode)   //*Arm 62-11-18 Check ถ้าเป็นสาขาตัวเองให้ทำ Process
                        {
                            //Public MQ Process stock
                            cmlRcvSalePos oSale = new cmlRcvSalePos();
                            oSale.ptBchCode = oHD.FTBchCode.ToString();
                            oSale.ptXihDocNo = oHD.FTXshDocNo.ToString();
                            oSale.ptPosCode = oHD.FTPosCode.ToString();
                            oSale.ptConnStr = poData.ptConnStr;

                            string tMessage = JsonConvert.SerializeObject(oSale);

                            //*Arm 63-08-04 
                            if (oSalePos.ptWahStaPrcStk == "2")  // คลังแบบตัด stock
                            {
                                string tStaPrcStk = "";

                                //  check เคยตัด stock หรือยัง
                                oSql.Clear();
                                oSql.AppendLine("SELECT FTXshStaPrcStk FROM " + tTblSalHD + " WHERE FTBchCode = '" + oHD.FTBchCode.ToString() + "' FTXshDocNo = '" + oHD.FTXshDocNo.ToString() + "' ");
                                tStaPrcStk = oDB.C_DAToExecuteQuery<string>(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);

                                if (tStaPrcStk != "1")  // ยังไม่ตัด stock
                                {
                                    cFunction.C_PRCxMQPublish("SALEPOS", tMessage, out ptErrMsg);
                                }
                            }
                            //+++++++++++++

                            //cFunction.C_PRCxMQPublish("SALEPOS", tMessage, out ptErrMsg);
                            cFunction.C_PRCxMQPublish("POSEJ", tMessage, out ptErrMsg); //*Em 62-09-16

                            ////*Arm 62-11-06 // Net 63-08-03 ปิดตาม Moshi
                            //cmlRcvSalePS2Touch oSalePS2Touch = new cmlRcvSalePS2Touch();
                            //oSalePS2Touch.ptFunction = "SalePos";
                            //oSalePS2Touch.ptSource = "MQReceivePrc";
                            //oSalePS2Touch.ptDest = "MQAdaLink";
                            //oSalePS2Touch.ptData = new cmlData();
                            //oSalePS2Touch.ptData.ptDocNo = oHD.FTXshDocNo.ToString();

                            //string tMsgSalePS2Touch = JsonConvert.SerializeObject(oSalePS2Touch);

                            //cFunction.C_PRCxMQPublish("X_QSalePS2Touch", tMsgSalePS2Touch, out ptErrMsg);
                            //++++++++++

                            //*Arm 63-02-18 - สำหรับ Update SO
                            if (!string.IsNullOrEmpty(oHD.FTXshRefExt)) // Check DocType = 1 :บิลขาย และ มีการอ้างอิง SO (FNXshDocType) 
                            {
                                if (oHD.FNXshDocType.ToString() == "1")
                                {
                                    oSql = new StringBuilder();
                                    oSql.AppendLine("UPDATE TARTSoHD WITH(ROWLOCK) SET ");
                                    oSql.AppendLine("FTXshRefInt = '" + oHD.FTXshDocNo + "',");
                                    oSql.AppendLine("FDXshRefIntDate = '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "',");
                                    oSql.AppendLine("FNXshStaRef ='2'");
                                    oSql.AppendLine("WHERE FTXshDocNo ='" + oHD.FTXshRefExt + "'");
                                    oSql.AppendLine("AND FTBchCode = '" + oHD.FTBchCode + "'");

                                    oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                                }
                            }
                            //++++++++++
                        }
                    }
                    
                    //*Em 62-10-18
                    if (oSP.SP_CHKbIsHQBch(poData.ptConnStr, (int)poShopDB.nCommandTimeOut) == false)
                    {
                        string tAPIUrl = "";
                        string tUrlFunc = "/Service/Upload/Sale";
                        string tAPIHeader = "";
                        string tXKey = "";
                        string tBchHQ = "";
                        tBchHQ = oSP.SP_GETtBchHQ(poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                        tAPIUrl = oSP.SP_GETtUrlAPI(poData.ptConnStr, (int)poShopDB.nCommandTimeOut, tBchHQ, 5, ref tXKey, ref tAPIHeader);

                        if (!string.IsNullOrEmpty(tAPIUrl))
                        {
                            string tJSonCall = JsonConvert.SerializeObject(oSalePos);
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(tAPIHeader, tXKey);
                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tAPIUrl + tUrlFunc, tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("cSale", "C_PRCbUploadSale : " + oEx.Message);
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
                                    new cLog().C_WRTxLog("cSale", "C_PRCbUploadSale/ToHQ : " + oRes.rtMsg);
                                }
                            }
                        }
                    }
                    //+++++++++++++++++++
                }

                // Net 63-08-03 ยกมาจาก Moshi
                //*Arm 63-05-30 Drop Temp
                oSql.Clear();
                oSql.AppendLine("DROP TABLE " + tTblSalHD + "");
                oSql.AppendLine("DROP TABLE " + tTblSalHDCst + "");
                oSql.AppendLine("DROP TABLE " + tTblSalHDDis + "");
                oSql.AppendLine("DROP TABLE " + tTblSalDT + "");
                oSql.AppendLine("DROP TABLE " + tTblSalDTDis + "");
                oSql.AppendLine("DROP TABLE " + tTblSalRC + "");
                oSql.AppendLine("DROP TABLE " + tTblSalRD + "");
                oSql.AppendLine("DROP TABLE " + tTblSalPD + "");
                oSql.AppendLine("DROP TABLE " + tTblTxnSale + "");
                oSql.AppendLine("DROP TABLE " + tTblTxnRedeem + "");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //+++++++++++++


                //Arm 63-07-01 Export Sale
                if (cVB.bVB_StaAlwSendAdaLink == true) //*Arm 63-08-04
                {
                    foreach (cmlTPSTSalHD oData in oSalePos.aoTPSTSalHD)
                    {
                        cmlDataExportSale oExpSale = new cmlDataExportSale();
                        oExpSale.ptFilter = oData.FTBchCode;
                        oExpSale.ptWaHouse = oData.FTWahCode;
                        oExpSale.ptRound = "1";
                        oExpSale.ptPosCode = oData.FTPosCode;
                        oExpSale.ptDateFrm = "";
                        oExpSale.ptDateTo = "";
                        oExpSale.ptDocNoFrm = oData.FTXshDocNo;
                        oExpSale.ptDocNoTo = oData.FTXshDocNo;

                        cmlExpSale oExp = new cmlExpSale();
                        oExp.ptFunction = "SalePos";
                        oExp.ptSource = "MQReceivePrc";
                        oExp.ptDest = "MQAdaLink";
                        oExp.ptData = JsonConvert.SerializeObject(oExpSale);

                        string tMsgExpSale = JsonConvert.SerializeObject(oExp);
                        cFunction.C_PRCxMQPublish("LK_QSale2Vender", tMsgExpSale, out ptErrMsg);
                    }
                }
                //++++++++++++++

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadSale");
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
                oSql = null;
                oSalePos = null;
                oTranscation = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
                new cSP().SP_CLExMemory();
            }
        }

        //* Net 63-08-03 ยกมาจาก Moshi
        //public bool C_PRCbCrateEJ(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, ref string ptErrMsg)
        public bool C_PRCbCreateEJ(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, ref string ptErrMsg) //*Net 63-07-14 เปลี่ยนชื่อ
        {
            DataTable odtTmp;
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            bool bSuccess = false;
            string tConnStr = "";
            //string tDocNo = "";
            int nCmdTime = 0;
            //cEJ oEJ = new cEJ();
            
            try
            {
                if (poSalePos == null) return false;
                tConnStr = poSalePos.ptConnStr;
                nCmdTime = (int)poShopDB.nCommandTimeOut;
                oSql = new StringBuilder();

                oSql.Clear();
                oSql.AppendLine("SELECT FTPosCode,FTPosType,FTPosRegNo,FTSmgCode,FTPosStaPrnEJ, FTPosStaSumPrn");//*Arm 63-05-06 เพิ่ม FTPosStaSumPrn
                oSql.AppendLine("FROM TCNMPos WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode+ "' AND  FTPosCode = '"+ poSalePos.ptPosCode +"'"); //*Arm 63-05-12 WHERE FTBchCode
                cmlTCNMPos oPos = oDB.C_DAToExecuteQuery<cmlTCNMPos>(tConnStr, oSql.ToString(), nCmdTime);
                if (oPos != null)
                {
                    if(oPos.FTPosStaPrnEJ != "1") return true;
                }
                else
                {
                    //* Net 63-08-03 ยกมาจาก Moshi
                    //*Net 63-07-14 ถ้าไม่เจอ Pos/Bch ให้ fail
                    throw new Exception($"Pos{poSalePos.ptPosCode} Bch{poSalePos.ptBchCode} not found");
                }

                // Net 63-08-03 ยกมาจาก Moshi
                switch (poSalePos.pnXihDocType)
                {
                    case 1: //ประเภท ขาย/คืน
                        bSuccess = C_PRCbSaleEJ(poSalePos, oPos, poShopDB, ref ptErrMsg);
                        break;
                    case 2: //ประเภท รอบการขาย
                        bSuccess = C_PRCbShiftEJ(poSalePos, oPos, poShopDB, ref ptErrMsg);
                        break;
                    default:
                        throw new Exception($"Unknown Type {poSalePos.pnXihDocType}");
                }
                return bSuccess;

                //* Net 63-08-03 ปิดตาม Moshi
                #region Comment
                //oSql.Clear();
                //oSql.AppendLine("SELECT TOP 1 FTShfCode");
                //oSql.AppendLine("FROM TPSTShiftHD WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTBchCode = '"+ poSalePos.ptBchCode +"' AND FTPosCode = '"+ poSalePos.ptPosCode +"' AND FTShfCode = '"+ poSalePos.ptXihDocNo +"'");
                //tDocNo = oDB.C_DAToExecuteQuery<string>(tConnStr, oSql.ToString(), nCmdTime);
                //if (string.IsNullOrEmpty(tDocNo))
                //{
                //    oSql.Clear();
                //    oSql.AppendLine("SELECT TOP 1 FTXshDocNo");
                //    oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                //    oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "' AND FTPosCode = '" + poSalePos.ptPosCode + "' AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
                //    tDocNo = oDB.C_DAToExecuteQuery<string>(tConnStr, oSql.ToString(), nCmdTime);
                //    if (string.IsNullOrEmpty(tDocNo))
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        if (oEJ.C_GENbSlip(poSalePos, poShopDB, oPos))
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //}
                //else
                //{
                //    oSql.Clear();
                //    oSql.AppendLine("SELECT FTSysKey,FTSysSeq,FTSysStaUsrValue");
                //    oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                //    oSql.AppendLine("WHERE FTSysCode = 'bPS_AlwPrintShift'");
                //    odtTmp = new DataTable();
                //    odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), nCmdTime);
                //    if (odtTmp != null)
                //    {
                //        foreach (DataRow oRow in odtTmp.Rows)
                //        {
                //            switch (oRow.Field<string>("FTSysKey").ToUpper())
                //            {
                //                case "SHIFTRCV":
                //                    if (oRow.Field<string>("FTSysStaUsrValue") == "1")
                //                    {
                //                        if (!oEJ.C_GENbSlipRCV(poSalePos, poShopDB, oPos)) return false;
                //                    }
                //                    break;
                //                case "SHIFTBNK":
                //                    if (oRow.Field<string>("FTSysStaUsrValue") == "1")
                //                    {
                //                        if (!oEJ.C_GENbSlipBNK(poSalePos, poShopDB, oPos)) return false;
                //                    } 
                //                    break;
                //                case "SHIFTSUM":
                //                    if (oRow.Field<string>("FTSysStaUsrValue") == "1")
                //                    {
                //                        if (!oEJ.C_GENbSlipSUM(poSalePos, poShopDB, oPos)) return false;
                //                    }
                //                    break;
                //            }
                //        }
                //    }



                //    return true;
                //}
                #endregion

            }
            catch (Exception oEx)
            {
                //* Net 63-08-03 ยกมาจาก Moshi
                ptErrMsg = oEx.Message; //*Net 63-07-10 เก็บ error
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbCrateEJ");
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public bool C_PRCbSaleEJ(cmlRcvSalePos poSalePos, cmlTCNMPos poPos, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB;
            cEJ oEJ;
            string tDocNo;
            try
            {
                oDB = new cDatabase();
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTXshDocNo");
                oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "' AND FTPosCode = '" + poSalePos.ptPosCode + "' AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
                tDocNo = oDB.C_DAToExecuteQuery<string>(poSalePos.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);
                if (!String.IsNullOrEmpty(tDocNo))
                {
                    oEJ = new cEJ(poSalePos.ptConnStr, poShopDB.nCommandTimeOut.Value);
                    if (oEJ.C_GENbSlip(poSalePos, poShopDB, poPos, ref ptErrMsg) == false)
                    {
                        throw new Exception(ptErrMsg);
                    }
                }
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbSaleEJ");
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public bool C_PRCbShiftEJ(cmlRcvSalePos poSalePos, cmlTCNMPos poPos, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            cEJ oEJ;
            string tDocNo;
            try
            {
                oDB = new cDatabase();
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTShfCode");
                oSql.AppendLine("FROM TPSTShiftHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "' AND FTPosCode = '" + poSalePos.ptPosCode + "' AND FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                tDocNo = oDB.C_DAToExecuteQuery<string>(poSalePos.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);
                if (!String.IsNullOrEmpty(tDocNo))
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTSysKey,FTSysSeq,FTSysStaUsrValue");
                    oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTSysCode = 'bPS_AlwPrintShift'");
                    odtTmp = new DataTable();
                    odtTmp = oDB.C_DAToExecuteQuery(poSalePos.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);
                    if (odtTmp != null)
                    {
                        oEJ = new cEJ(poSalePos.ptConnStr, poShopDB.nCommandTimeOut.Value);
                        foreach (DataRow oRow in odtTmp.Rows)
                        {
                            switch (oRow.Field<string>("FTSysKey").ToUpper())
                            {
                                case "SHIFTRCV":
                                    if (oRow.Field<string>("FTSysStaUsrValue") == "1")
                                    {
                                        if (oEJ.C_GENbSlipRCV(poSalePos, poShopDB, poPos, ref ptErrMsg) == false)
                                            throw new Exception(ptErrMsg);
                                    }
                                    break;
                                case "SHIFTBNK":
                                    if (oRow.Field<string>("FTSysStaUsrValue") == "1")
                                    {
                                        if (oEJ.C_GENbSlipBNK(poSalePos, poShopDB, poPos, ref ptErrMsg) == false)
                                            throw new Exception(ptErrMsg);
                                    }
                                    break;
                                case "SHIFTSUM":
                                    if (oRow.Field<string>("FTSysStaUsrValue") == "1")
                                    {
                                        if (oEJ.C_GENbSlipSUM(poSalePos, poShopDB, poPos, ref ptErrMsg) == false)
                                            throw new Exception(ptErrMsg);
                                    }
                                    break;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbShiftEJ");
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return false;
        }

    }
}
