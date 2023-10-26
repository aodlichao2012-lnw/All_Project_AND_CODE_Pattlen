using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Notification;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.SaleVD;
using MQReceivePrc.Models.Webservice.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cSaleVD
    {
        public bool C_PRCbSalePos(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            DataTable odtTmp = new DataTable();
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            string tConnStr;
            DataTable odtResult = new DataTable();
            try
            {
                ptErrMsg = "";

                //1.ตรวจสอบข้อมูล
                if (poSalePos == null) return false;
                tConnStr = oDB.C_GETtConnectString4PrcConsolidate(poSalePos.ptConnStr, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ISNULL(FTXshStaPrcStk,'') AS FTXshStaPrcStk");
                oSql.AppendLine("FROM TVDTSalHD with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + poSalePos.ptPosCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);
                if (odtTmp == null)
                {
                    return false;
                }
                else
                {
                    if (odtTmp.Rows.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (odtTmp.Rows[0]["FTXshStaPrcStk"].ToString() == "1")
                        {
                            return true;
                        }
                        else
                        {
                            SqlParameter[] aPara = new SqlParameter[] {
                            new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = poSalePos.ptBchCode},
                            new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = poSalePos.ptXihDocNo},
                            new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = "MQReceivePrc"},
                            new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                            };
                            //if (oDB.C_DATbExecuteStoreProcedure(tConnStr, "STP_DOCxSaleVDPrcStk", ref aPara, (int)poShopDB.nCommandTimeOut, "@FNResult"))
                            if (oDB.C_DATbExecuteStoreProcedure(tConnStr, "STP_DOCxSaleVDPrcStk", ref aPara, (int)poShopDB.nCommandTimeOut, ref odtResult, "@FNResult")) //*Em 62-08-19
                            {
                                oSql = new StringBuilder();
                                oSql.AppendLine("UPDATE TVDTSalDT with(rowlock)");
                                oSql.AppendLine("SET FTXsdStaPrcStk='1'");
                                oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                oSql.AppendLine(",FTLastUpdBy = 'MQReceivePrc'");
                                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "'");
                                oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
                                oSql.AppendLine("AND ISNULL(FTXsdStaPdt,'') NOT IN('4','5')");
                                oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                                oSql = new StringBuilder();
                                oSql.AppendLine("UPDATE TVDTSalHD with(rowlock)");
                                oSql.AppendLine("SET FTXshStaPrcStk='1'");
                                oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                oSql.AppendLine(",FTLastUpdBy = 'MQReceivePrc'");
                                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "'");
                                oSql.AppendLine("AND FTPosCode = '" + poSalePos.ptPosCode + "'");
                                oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
                                oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                                C_SENDxMsgNoti(odtResult);

                                //*Arm 63-03-31 Check StaUseCentralized
                                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                                {

                                    //*Arm 62-11-20  [UPLOADSTKBAL / UPLOADSTKCRD]
                                    //*********************************
                                    string tErrMsg = "";

                                    cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                                    oDocApv.ptBchCode = poSalePos.ptBchCode;
                                    oDocApv.ptDocNo = poSalePos.ptXihDocNo;
                                    oDocApv.ptDocType = "SALEPOSVD";
                                    oDocApv.ptUser = "MQReceivePrc";
                                    oDocApv.ptConnStr = poSalePos.ptConnStr;

                                    string tMsgStock = Newtonsoft.Json.JsonConvert.SerializeObject(oDocApv);
                                    cFunction.C_PRCxMQPublish("UPLOADSTKCRD", tMsgStock, out tErrMsg);
                                    cFunction.C_PRCxMQPublish("UPLOADSTKBAL", tMsgStock, out tErrMsg);

                                    //**********************************
                                }
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
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbSalePos");
                return false;
            }
            finally
            {
                //new cFunction().C_CLExMemory();
            }
        }

        public void C_SENDxMsgNoti(DataTable poDt)
        {
            cmlNotification oNoti = new cmlNotification();
            try
            {
                if (poDt.Rows.Count > 0)
                {
                    oNoti.ptPOSCode = poDt.Rows[0][0].ToString();
                    foreach (DataRow oDr in poDt.Rows)
                    {
                        oNoti.aoDetail.Add(new cmlNotiDetail
                        {
                            ptFTPdtCode = oDr["FTPdtCode"].ToString(),
                            pnFNLayRow = oDr["FNLayRow"].ToString(),
                            pnFNLayCol = oDr["FNLayCol"].ToString(),
                            pnFCStkQty = oDr["FCStkQty"].ToString(),
                            pnFCPdtMin = ""
                        });
                    }
                    string tMessage = JsonConvert.SerializeObject(oNoti, Formatting.Indented);
                    string ptErrMsg = string.Empty;
                    cFunction.C_PRCxMQPublish("STOCKVD2POS", tMessage, out ptErrMsg);
                }
            }
            catch (Exception oEx)
            {
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_SENDxMsgNoti");
            }
            finally
            {
                oNoti = null;
                //new cFunction().C_CLExMemory();
            }
        }

        //public bool C_PRCbUploadSaleVD(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        public bool C_PRCbUploadSaleVD(string ptQueueID,cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)   //*Em 63-08-17
        {
            cDataReader<cmlTVDTSalHD> aoHD;
            cDataReader<cmlTVDTSalHDCst> aoHDCst;
            cDataReader<cmlTVDTSalHDDis> aoHDDis;
            cDataReader<cmlTVDTSalDT> aoDT;
            cDataReader<cmlTVDTSalDTDis> aoDTDis;
            cDataReader<cmlTVDTSalDTPmt> aoDTPmt;
            cDataReader<cmlTVDTSalDTVD> aoDTVD;
            cDataReader<cmlTVDTSalRC> aoRC;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlTVDTSal oSalePos;
            SqlTransaction oTranscation;
            SqlConnection oConn;
            cSP oSP = new cSP();
            string tQueueID = "";   //*Em 63-08-17
            string tBchCode = "";   //*Em 63-08-17
            string tDocNo = "";     //*Em 63-08-17

            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oSalePos = JsonConvert.DeserializeObject<cmlTVDTSal>(poData.ptData);

                //*Em 63-08-17
                tQueueID = ptQueueID.Replace("-", "");
                string tTblSalHD = "TSHD" + tQueueID;
                string tTblSalHDCst = "TSHDCst" + tQueueID;
                string tTblSalHDDis = "TSHDDis" + tQueueID;
                string tTblSalDT = "TSDT" + tQueueID;
                string tTblSalDTDis = "TSDTDis" + tQueueID;
                string tTblSalDTPmt = "TSDTPmt" + tQueueID;
                string tTblSalDTVD = "TSDTVD" + tQueueID;
                string tTblSalRC = "TSRC" + tQueueID;
                //+++++++++++++++++++++

                //Create Temp
                #region Create Temp
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TVDTSalHDTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TVDTSalHDTmp FROM TVDTSalHD with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalHD' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TVDTSalHDTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TVDTSalHDTmp FROM TVDTSalHD with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TVDTSalHDTmp");
                //oSql.AppendLine("");
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TVDTSalHDCstTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TVDTSalHDCstTmp FROM TVDTSalHDCst with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalHDCstTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalHDCst' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TVDTSalHDCstTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TVDTSalHDCstTmp FROM TVDTSalHDCst with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TVDTSalHDCstTmp");
                //oSql.AppendLine("");
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TVDTSalHDDisTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TVDTSalHDDisTmp FROM TVDTSalHDDis with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalHDDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalHDDis' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TVDTSalHDDisTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TVDTSalHDDisTmp FROM TVDTSalHDDis with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TVDTSalHDDisTmp");
                //oSql.AppendLine("");
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TVDTSalDTTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TVDTSalDTTmp FROM TVDTSalDT with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalDT' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TVDTSalDTTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TVDTSalDTTmp FROM TVDTSalDT with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TVDTSalDTTmp");
                //oSql.AppendLine("");
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TVDTSalDTDisTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TVDTSalDTDisTmp FROM TVDTSalDTDis with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalDTDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalDTDis' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TVDTSalDTDisTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TVDTSalDTDisTmp FROM TVDTSalDTDis with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TVDTSalDTDisTmp");
                //oSql.AppendLine("");
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TVDTSalDTPmtTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TVDTSalDTPmtTmp FROM TVDTSalDTPmt with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalDTPmtTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalDTPmt' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TVDTSalDTPmtTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TVDTSalDTPmtTmp FROM TVDTSalDTPmt with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TVDTSalDTPmtTmp");
                //oSql.AppendLine("");
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TVDTSalDTVDTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TVDTSalDTVDTmp FROM TVDTSalDTVD with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalDTVDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalDTVD' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TVDTSalDTVDTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TVDTSalDTVDTmp FROM TVDTSalDTVD with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TVDTSalDTVDTmp");
                //oSql.AppendLine("");
                //oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TVDTSalRCTmp'))");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("	    SELECT TOP 0 * INTO TVDTSalRCTmp FROM TVDTSalRC with(nolock)");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("ELSE");
                //oSql.AppendLine("   BEGIN");
                //oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalRCTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TVDTSalRC' ),0)");
                //oSql.AppendLine("       BEGIN");
                //oSql.AppendLine("   	    DROP TABLE TVDTSalRCTmp");
                //oSql.AppendLine("   	    SELECT TOP 0 * INTO TVDTSalRCTmp FROM TVDTSalRC with(nolock)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("TRUNCATE TABLE TVDTSalRCTmp");
                //oSql.AppendLine("");
                //oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                //*Em 63-08-17
                oDB.C_PRCxCreateDatabaseTmp("TVDTSalHD", tTblSalHD, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TVDTSalHDCst", tTblSalHDCst, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TVDTSalHDDis", tTblSalHDDis, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TVDTSalDT", tTblSalDT, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TVDTSalDTDis", tTblSalDTDis, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TVDTSalDTPmt", tTblSalDTPmt, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TVDTSalDTVD", tTblSalDTVD, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TVDTSalRC", tTblSalRC, poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                //+++++++++++++++++++
                #endregion

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTranscation = oConn.BeginTransaction();

                //insert to DB
                if (oSalePos.aoTVDTSalHD != null)
                {
                    aoHD = new cDataReader<cmlTVDTSalHD>(oSalePos.aoTVDTSalHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TVDTSalHDTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHD;    //*Em 63-08-17

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

                if (oSalePos.aoTVDTSalHDCst != null)
                {
                    aoHDCst = new cDataReader<cmlTVDTSalHDCst>(oSalePos.aoTVDTSalHDCst);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDCst.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TVDTSalHDCstTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHDCst;    //*Em 63-08-17

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

                if (oSalePos.aoTVDTSalHDDis != null)
                {
                    aoHDDis = new cDataReader<cmlTVDTSalHDDis>(oSalePos.aoTVDTSalHDDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TVDTSalHDDisTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHDDis;    //*Em 63-08-17

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

                if (oSalePos.aoTVDTSalDT != null)
                {
                    aoDT = new cDataReader<cmlTVDTSalDT>(oSalePos.aoTVDTSalDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TVDTSalDTTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalDT;    //*Em 63-08-17

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

                if (oSalePos.aoTVDTSalDTDis != null)
                {
                    aoDTDis = new cDataReader<cmlTVDTSalDTDis>(oSalePos.aoTVDTSalDTDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TVDTSalDTDisTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalDTDis;    //*Em 63-08-17

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

                if (oSalePos.aoTVDTSalDTPmt != null)
                {
                    aoDTPmt = new cDataReader<cmlTVDTSalDTPmt>(oSalePos.aoTVDTSalDTPmt);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTPmt.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TVDTSalDTPmtTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalDTPmt;    //*Em 63-08-17

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

                if (oSalePos.aoTVDTSalDTVD != null)
                {
                    aoDTVD = new cDataReader<cmlTVDTSalDTVD>(oSalePos.aoTVDTSalDTVD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTVD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TVDTSalDTVDTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalDTVD;    //*Em 63-08-17

                        try
                        {
                            oBulkCopy.WriteToServer(aoDTVD);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTVDTSalRC != null)
                {
                    aoRC = new cDataReader<cmlTVDTSalRC>(oSalePos.aoTVDTSalRC);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoRC.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TVDTSalRCTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalRC;    //*Em 63-08-17

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

                oTranscation.Commit();

                //*Em 63-08-17
                tBchCode = oSalePos.aoTVDTSalHD[0].FTBchCode;
                tDocNo = oSalePos.aoTVDTSalHD[0].FTXshDocNo;
                //++++++++++++++

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                //*Em 63-08-17 Update FTXshStaPrcStk กรณีส่งซ้ำ
                oSql.AppendLine("   UPDATE THDTMP SET ");
                oSql.AppendLine("   THDTMP.FTXshStaPrcStk = THD.FTXshStaPrcStk,");
                oSql.AppendLine("   THDTMP.FTXshDocVatFull = THD.FTXshDocVatFull"); //*Arm 63-05-26 // Net 63-08-03 ยกมาจาก Moshi
                oSql.AppendLine("   FROM " + tTblSalHD + " THDTMP WITH(ROWLOCK)"); // *Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi
                oSql.AppendLine("   INNER JOIN TVDTSalHD THD  WITH(NOLOCK) ON THDTMP.FTBchCode = THD.FTBchCode AND THDTMP.FTXshDocNo = THD.FTXshDocNo");
                oSql.AppendLine("   WHERE THDTMP.FTBchCode = '" + tBchCode + "' AND THDTMP.FTXshDocNo = '" + tDocNo + "' "); //*Arm 63-05-30  // Net 63-08-03 ยกมาจาก Moshi

                oSql.AppendLine("   UPDATE TDTTMP SET ");
                oSql.AppendLine("   TDTTMP.FTXsdStaPrcStk = TDT.FTXsdStaPrcStk");
                oSql.AppendLine("   FROM " + tTblSalDT + " TDTTMP WITH(ROWLOCK)"); //*Arm 63-05-30 // Net 63-08-03 ยกมาจาก Moshi
                oSql.AppendLine("   INNER JOIN TVDTSalDT TDT WITH(NOLOCK) ON TDTTMP.FTBchCode = TDT.FTBchCode AND TDTTMP.FTXshDocNo = TDT.FTXshDocNo");
                oSql.AppendLine("   WHERE TDTTMP.FTBchCode = '" + tBchCode + "' AND TDTTMP.FTXshDocNo = '" + tDocNo + "'");
                //++++++++++++++++++++++++++++++

                oSql.AppendLine("   DELETE HD ");
                oSql.AppendLine("   FROM TVDTSalHD HD WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TVDTSalHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine("   WHERE FTBchCode = '"+ tBchCode +"' AND FTXshDocNo ='"+ tDocNo +"'");    //*Em 63-08-17

                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TVDTSalHD");
                //oSql.AppendLine("   SELECT * FROM TVDTSalHDTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM "+ tTblSalHD +" WITH(NOLOCK) "); //*Em 63-08-17
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDCst ");
                oSql.AppendLine("   FROM TVDTSalHDCst HDCst WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TVDTSalHDCstTmp TMPCst WITH(NOLOCK) ON HDCst.FTBchCode = TMPCst.FTBchCode AND HDCst.FTXshDocNo = TMPCst.FTXshDocNo");
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TVDTSalHDCst");
                //oSql.AppendLine("   SELECT * FROM TVDTSalHDCstTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalHDCst + " WITH(NOLOCK) "); //*Em 63-08-17
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDDis ");
                oSql.AppendLine("   FROM TVDTSalHDDis HDDis WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TVDTSalHDDisTmp TMPHDDis WITH(NOLOCK) ON HDDis.FTBchCode = TMPHDDis.FTBchCode AND HDDis.FTXshDocNo = TMPHDDis.FTXshDocNo");
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TVDTSalHDDis");
                //oSql.AppendLine("   SELECT * FROM TVDTSalHDDisTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalHDDis + " WITH(NOLOCK) "); //*Em 63-08-17
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TVDTSalDT DT WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TVDTSalDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo");
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TVDTSalDT");
                //oSql.AppendLine("   SELECT * FROM TVDTSalDTTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalDT + " WITH(NOLOCK) "); //*Em 63-08-17
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTDis ");
                oSql.AppendLine("   FROM TVDTSalDTDis DTDis WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TVDTSalDTDisTmp TMPDTDis WITH(NOLOCK) ON DTDis.FTBchCode = TMPDTDis.FTBchCode AND DTDis.FTXshDocNo = TMPDTDis.FTXshDocNo");
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TVDTSalDTDis");
                //oSql.AppendLine("   SELECT * FROM TVDTSalDTDisTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalDTDis + " WITH(NOLOCK) "); //*Em 63-08-17
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTPmt ");
                oSql.AppendLine("   FROM TVDTSalDTPmt DTPmt WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TVDTSalDTPmtTmp TMPDTPmt WITH(NOLOCK) ON DTPmt.FTBchCode = TMPDTPmt.FTBchCode AND DTPmt.FTXshDocNo = TMPDTPmt.FTXshDocNo");
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TVDTSalDTPmt");
                //oSql.AppendLine("   SELECT * FROM TVDTSalDTPmtTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalDTPmt + " WITH(NOLOCK) "); //*Em 63-08-17
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTVD ");
                oSql.AppendLine("   FROM TVDTSalDTVD DTVD WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TVDTSalDTVDTmp TMPDTVD WITH(NOLOCK) ON DTVD.FTBchCode = TMPDTVD.FTBchCode AND DTVD.FTXshDocNo = TMPDTVD.FTXshDocNo");
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TVDTSalDTVD");
                //oSql.AppendLine("   SELECT * FROM TVDTSalDTVDTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalDTVD + " WITH(NOLOCK) "); //*Em 63-08-17
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   DELETE RC ");
                oSql.AppendLine("   FROM TVDTSalRC RC WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TVDTSalRCTmp TMPRC WITH(NOLOCK) ON RC.FTBchCode = TMPRC.FTBchCode AND RC.FTXshDocNo = TMPRC.FTXshDocNo");
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TVDTSalRC");
                //oSql.AppendLine("   SELECT * FROM TVDTSalRCTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalRC + " WITH(NOLOCK) "); //*Em 63-08-17
                oSql.AppendLine("   WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo ='" + tDocNo + "'");    //*Em 63-08-17
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);


                //foreach (cmlTVDTSalHD oHD in oSalePos.aoTVDTSalHD)
                //{
                //    //update sale Pos
                //    if (!string.IsNullOrEmpty(oHD.FTXshRefInt))
                //    {
                //        oSql = new StringBuilder();
                //        oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK)");
                //        oSql.AppendLine("SET FTXshRefInt = '" + oHD.FTXshDocNo.ToString() + "'");
                //        oSql.AppendLine(",FDXshRefIntDate = GETDATE()");
                //        oSql.AppendLine("WHERE FTBchCode = '" + oHD.FTBchCode.ToString() + "' AND FTXshDocNo = '" + oHD.FTXshRefInt.ToString() + "'");
                //        oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //    }

                //    //if (oHD.FTBchCode == cVB.tVB_BchCode)   //*Arm 62-11-18 Check ถ้าเป็นสาขาตัวเองให้ทำ Process
                //    if (oHD.FTBchCode == cVB.tVB_BchCode || cVB.bVB_StaUseCentralized == true) //*Net 63-04-07 ถ้าเป็น centalized ให้ทำด้วย
                //    {
                //        //Public MQ Process stock
                //        cmlRcvSalePos oSale = new cmlRcvSalePos();
                //        oSale.ptBchCode = oHD.FTBchCode.ToString();
                //        oSale.ptXihDocNo = oHD.FTXshDocNo.ToString();
                //        oSale.ptPosCode = oHD.FTPosCode.ToString();
                //        oSale.ptConnStr = poData.ptConnStr;

                //        string tMessage = JsonConvert.SerializeObject(oSale);
                //        cFunction.C_PRCxMQPublish("SALEPOSVD", tMessage, out ptErrMsg);
                //    }
                //}

                //*Arm 63-03-31 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                {
                    foreach (cmlTVDTSalHD oHD in oSalePos.aoTVDTSalHD)
                    {
                        //update sale Pos
                        if (!string.IsNullOrEmpty(oHD.FTXshRefInt))
                        {
                            oSql = new StringBuilder();
                            oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK)");
                            oSql.AppendLine("SET FTXshRefInt = '" + oHD.FTXshDocNo.ToString() + "'");
                            oSql.AppendLine(",FDXshRefIntDate = GETDATE()");
                            oSql.AppendLine("WHERE FTBchCode = '" + oHD.FTBchCode.ToString() + "' AND FTXshDocNo = '" + oHD.FTXshRefInt.ToString() + "'");
                            oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                        }

                        //Public MQ Process stock
                        cmlRcvSalePos oSale = new cmlRcvSalePos();
                        oSale.ptBchCode = oHD.FTBchCode.ToString();
                        oSale.ptXihDocNo = oHD.FTXshDocNo.ToString();
                        oSale.ptPosCode = oHD.FTPosCode.ToString();
                        oSale.ptConnStr = poData.ptConnStr;

                        string tMessage = JsonConvert.SerializeObject(oSale);
                        cFunction.C_PRCxMQPublish("SALEPOSVD", tMessage, out ptErrMsg);

                    }
                }
                else
                {
                    // ไม่ใช้งานระบบ Centralized

                    foreach (cmlTVDTSalHD oHD in oSalePos.aoTVDTSalHD)
                    {
                        //update sale Pos
                        if (!string.IsNullOrEmpty(oHD.FTXshRefInt))
                        {
                            oSql = new StringBuilder();
                            oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK)");
                            oSql.AppendLine("SET FTXshRefInt = '" + oHD.FTXshDocNo.ToString() + "'");
                            oSql.AppendLine(",FDXshRefIntDate = GETDATE()");
                            oSql.AppendLine("WHERE FTBchCode = '" + oHD.FTBchCode.ToString() + "' AND FTXshDocNo = '" + oHD.FTXshRefInt.ToString() + "'");
                            oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                        }

                        if (oHD.FTBchCode == cVB.tVB_BchCode)   //*Arm 62-11-18 Check ถ้าเป็นสาขาตัวเองให้ทำ Process
                        {
                            //Public MQ Process stock
                            cmlRcvSalePos oSale = new cmlRcvSalePos();
                            oSale.ptBchCode = oHD.FTBchCode.ToString();
                            oSale.ptXihDocNo = oHD.FTXshDocNo.ToString();
                            oSale.ptPosCode = oHD.FTPosCode.ToString();
                            oSale.ptConnStr = poData.ptConnStr;

                            string tMessage = JsonConvert.SerializeObject(oSale);
                            cFunction.C_PRCxMQPublish("SALEPOSVD", tMessage, out ptErrMsg);
                        }
                    }

                    //*Em 62-10-18
                    if (oSP.SP_CHKbIsHQBch(poData.ptConnStr, (int)poShopDB.nCommandTimeOut) == false)
                    {
                        string tAPIUrl = "";
                        string tUrlFunc = "/Service/Data/UplSaleVD";
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

                //*Em 63-08017
                oSql.Clear();
                oSql.AppendLine("DROP TABLE " + tTblSalHD + "");
                oSql.AppendLine("DROP TABLE " + tTblSalHDCst + "");
                oSql.AppendLine("DROP TABLE " + tTblSalHDDis + "");
                oSql.AppendLine("DROP TABLE " + tTblSalDT + "");
                oSql.AppendLine("DROP TABLE " + tTblSalDTDis + "");
                oSql.AppendLine("DROP TABLE " + tTblSalDTPmt + "");
                oSql.AppendLine("DROP TABLE " + tTblSalDTVD + "");
                oSql.AppendLine("DROP TABLE " + tTblSalRC + "");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //++++++++++++++++++
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadSaleVD");
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
                aoDTVD = null;
                aoRC = null;
                oSql = null;
                oSalePos = null;
                oTranscation = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }
        }

    }
}
