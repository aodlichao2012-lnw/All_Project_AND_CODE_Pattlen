using MQReceivePrc.Class;
using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Bch2Bch;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Doc.PdtTnfBch;
using MQReceivePrc.Models.WebService.Response;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cPrcBch2Bch
    {
        public bool C_PRCbDataBch2Bch(cmlCallSendBch poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            string tFileName = "";
            string tUrlFile = "";
            string tPathFile = "";
            cmlTCNTSync2BchHis oSync2Bch;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            int nSeq = 0;
            string tConnStr = poData.ptConnStr;
            int nCmdTime = (int)poShopDB.nCommandTimeOut;
            cmlRabbitMQ oMQ = new cmlRabbitMQ();
            ConnectionFactory oFactory;
            string tMsgMQ = "";
            try
            {
                if (poData == null) return  false;

                switch (poData.ptDocType)
                {
                    case "1":   //ใบโอนสินค้าระหว่างสาขา
                        tPathFile = C_CRTtFileDocTnfBch(poData, poShopDB, ref ptErrMsg);
                        tUrlFile = C_CRTtUrlFile(tPathFile, poShopDB, ref ptErrMsg);
                        break;
                    case "2":
                        break;
                }

                tFileName = new FileInfo(tPathFile).Name;
                oSql.AppendLine("DECLARE @nSeq int");
                oSql.AppendLine("SET @nSeq = (SELECT ISNULL(MAX(FNSynSeqNo),0) AS FNSynSeqNo FROM TCNTSync2BchHis WITH(NOLOCK)) + 1");
                oSql.AppendLine("");
                oSql.AppendLine("INSERT INTO TCNTSync2BchHis(FNSynSeqNo,FDSynDate,FTSynBchFrm,FTSynBchTo,FTSynFileName,FTSynFileURL)");
                oSql.AppendLine("VALUES(@nSeq,GETDATE(),'"+ poData.ptBchFrm +"','"+ tFileName + "','" + tUrlFile + "','')");
                oSql.AppendLine("");
                oSql.AppendLine("SELECT @nSeq");
                nSeq = oDB.C_DAToExecuteQuery<int>(tConnStr,oSql.ToString(),nCmdTime);

                oSync2Bch = new cmlTCNTSync2BchHis();
                oSync2Bch.FNSynSeqNo = nSeq;
                oSync2Bch.FTSynBchFrm = poData.ptBchFrm;
                oSync2Bch.FTSynBchTo = poData.ptBchTo;
                oSync2Bch.FTSynFileName = tFileName;
                oSync2Bch.FTSynFileURL = tUrlFile;
                oSync2Bch.FTSynRmk = poData.ptDocType;

                oMQ = new cSP().SP_GEToMQBranch(poData.ptBchTo, "MQDocument", tConnStr, nCmdTime);
                if (oMQ != null)
                {
                    oFactory = new ConnectionFactory();
                    oFactory.HostName = oMQ.tMQHostName;
                    oFactory.UserName = oMQ.tMQUserName;
                    oFactory.Password = oMQ.tMQPassword;
                    oFactory.VirtualHost = oMQ.tMQVirtualHost;
                    using (var oConn = oFactory.CreateConnection())
                    {
                        using (var oChannel = oConn.CreateModel())
                        {
                            tMsgMQ = JsonConvert.SerializeObject(oSync2Bch);
                            var body = Encoding.UTF8.GetBytes(tMsgMQ);
                            oChannel.QueueDeclare("BCHRECEIVE", false, false, false, null);
                            oChannel.BasicPublish("", "BCHRECEIVE", false, null, body);
                            ptErrMsg = "";
                        }
                    }
                }
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbDataBch2Bch");
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCbDataBch2Bch : " + oEx.Message);
                return false;
            }
        }

        public bool C_PRCbBchReceive(cmlTCNTSync2BchHis poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDatabase oDB = new cDatabase();
            cmlRabbitMQ oMQ = new cmlRabbitMQ();
            ConnectionFactory oFactory;
            string tMsgMQ = "";
            string tPathFile = "";
            string tConnStr = oDB.C_GETtConnectString(poShopDB.tServer,poShopDB.tUser,poShopDB.tPassword,poShopDB.tDatabase,(int)poShopDB.nConnectTimeOut,poShopDB.tAuthenMode);
            int nCmdTime = (int)poShopDB.nCommandTimeOut;

            try
            {
                switch (poData.FTSynRmk)
                {
                    case "1":   //ใบโอนสินค้า
                        tPathFile = C_PRCtDownloadFile(poData.FTSynFileURL);
                        if (C_PRCbImpDataPdtTnfBch(tPathFile,tConnStr,nCmdTime))
                        {
                            poData.FTSynStatus = "1";
                        }
                        else
                        {
                            poData.FTSynStatus = "2";
                        }
                        poData.FDSynRcvDate = DateTime.Now;

                        oMQ = new cSP().SP_GEToMQBranch(poData.FTSynBchFrm, "MQDocument", tConnStr, nCmdTime);
                        if (oMQ != null)
                        {
                            oFactory = new ConnectionFactory();
                            oFactory.HostName = oMQ.tMQHostName;
                            oFactory.UserName = oMQ.tMQUserName;
                            oFactory.Password = oMQ.tMQPassword;
                            oFactory.VirtualHost = oMQ.tMQVirtualHost;
                            using (var oConn = oFactory.CreateConnection())
                            {
                                using (var oChannel = oConn.CreateModel())
                                {
                                    tMsgMQ = JsonConvert.SerializeObject(poData);
                                    var body = Encoding.UTF8.GetBytes(tMsgMQ);
                                    oChannel.QueueDeclare("UPDRECEIVE", false, false, false, null);
                                    oChannel.BasicPublish("", "UPDRECEIVE", false, null, body);
                                    ptErrMsg = "";
                                }
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    default:
                        return true;
                        break;
                        
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCbBchReceive : " + oEx.Message);
                return false;
            }
        }

        private string C_CRTtFileDocTnfBch(cmlCallSendBch poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tPathFile = "";
            string tFileName = "";
            cmlTCNTPdtTbx oPdtTnfBch;
            string tConnStr = poData.ptConnStr;
            int nCmdTime = (int)poShopDB.nCommandTimeOut;
            try
            {
                oPdtTnfBch = new cmlTCNTPdtTbx();

                oSql.AppendLine("SELECT FTBchCode,FTXthDocNo,FDXthDocDate,FTXthVATInOrEx,FTDptCode,FTXthBchFrm,FTXthBchTo,");
                oSql.AppendLine("FTXthMerchantFrm,FTXthMerchantTo,FTXthShopFrm,FTXthShopTo,FTXthWhFrm,FTXthWhTo,FTUsrCode,");
                oSql.AppendLine("FTSpnCode,FTXthApvCode,FTXthRefExt,FDXthRefExtDate,FTXthRefInt,FDXthRefIntDate,FNXthDocPrint,");
                oSql.AppendLine("FCXthTotal,FCXthVat,FCXthVatable,FTXthRmk,FTXthStaDoc,FTXthStaApv,FTXthStaPrcStk,");
                oSql.AppendLine("FTXthStaDelMQ,FNXthStaDocAct,FNXthStaRef,FTRsnCode,");
                oSql.AppendLine("FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy");
                oSql.AppendLine("FROM TCNTPdtTbxHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTXthDocNo = '"+ poData.ptDocNo +"'");
                oPdtTnfBch.oTCNTPdtTbxHD = oDB.C_DAToExecuteQuery<cmlTCNTPdtTbxHD>(tConnStr,oSql.ToString(),nCmdTime);
                if (oPdtTnfBch.oTCNTPdtTbxHD != null)
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTBchCode,FTXthDocNo,FTXthCtrName,FDXthTnfDate,FTXthRefTnfID,");
                    oSql.AppendLine("FTXthRefVehID,FTXthQtyAndTypeUnit,FNXthShipAdd,FTViaCode");
                    oSql.AppendLine("FROM TCNTPdtTbxHDRef WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTXthDocNo = '" + poData.ptDocNo + "'");
                    oPdtTnfBch.oTCNTPdtTbxHDRef = oDB.C_DAToExecuteQuery<cmlTCNTPdtTbxHDRef>(tConnStr, oSql.ToString(), nCmdTime);

                    oSql.Clear();
                    oSql.AppendLine("SELECT FTBchCode,FTXthDocNo,FNXtdSeqNo,FTPdtCode,FTXtdPdtName,FTPunCode,FTPunName,");
                    oSql.AppendLine("FCXtdFactor,FTXtdBarCode,FTXtdVatType,FTVatCode,FCXtdVatRate,FCXtdQty,FCXtdQtyAll,");
                    oSql.AppendLine("FCXtdSetPrice,FCXtdAmt,FCXtdVat,FCXtdVatable,FCXtdNet,FCXtdCostIn,FCXtdCostEx,");
                    oSql.AppendLine("FTXtdStaPrcStk,FNXtdPdtLevel,FTXtdPdtParent,FCXtdQtySet,FTXtdPdtStaSet,FTXtdRmk,");
                    oSql.AppendLine("FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy");
                    oSql.AppendLine("FROM TCNTPdtTbxDT WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTXthDocNo = '" + poData.ptDocNo + "'");
                    oPdtTnfBch.aoTCNTPdtTbxDT = oDB.C_GETaDataQuery<cmlTCNTPdtTbxDT>(tConnStr, oSql.ToString(), nCmdTime);

                    tPathFile = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + @"\FileSend";
                    if (!Directory.Exists(tPathFile)) Directory.CreateDirectory(tPathFile);

                    tFileName = poData.ptBchFrm + "_" + poData.ptBchTo + "_TBX_" + poData.ptDocNo  +"_" + DateTime.Now.ToString("yyMMddHHmmss") + ".JSON";
                    tPathFile += tFileName;
                    File.WriteAllText(tPathFile, Newtonsoft.Json.JsonConvert.SerializeObject(oPdtTnfBch));

                    if (File.Exists(tPathFile))
                    {
                        return tPathFile;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_CRTtFileDocTnfBch");
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_CRTtFileDocTnfBch : " + oEx.Message);
                return "";
            }
        }

        private string C_CRTtUrlFile(string ptFile , cmlShopDB poShopDB,ref string ptErrMsg)
        {
            string tUrlFile = "";
            string tURLFunc = "/FileManage/CreateURL";
            try
            {
                cClientService oCall = new cClientService();
                HttpResponseMessage oResponse = new HttpResponseMessage();
                try
                {
                    oResponse = oCall.C_POSToInvoke(poShopDB.tUrlAPI2PSMaster + tURLFunc, ptFile);
                }
                catch (Exception oEx)
                {
                    ptErrMsg = oEx.Message.ToString();     //*Em 62-01-30  Pandora
                    return "";
                }

                if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(tJsonResult))
                    {
                        cmlResItem<string> oResult = JsonConvert.DeserializeObject<cmlResItem<string>>(tJsonResult);
                        if (oResult.rtCode == "1")
                        {
                            tUrlFile = oResult.roItem;
                        }
                    }
                }
                return tUrlFile;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_CRTtUrlFile : " + oEx.Message);
                return "";
            }
        }

        private string C_PRCtDownloadFile(string ptUrlFile)
        {
            string tPathFile = "";
            string tFileName = "";
            string tAppPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            try
            {
                if (string.IsNullOrEmpty(ptUrlFile)) return tPathFile;

                using (WebClient oClient = new WebClient())
                {
                    Uri oUri = new Uri(ptUrlFile);
                    if (oUri.IsAbsoluteUri)
                    {
                        tFileName = Path.GetFileName(oUri.LocalPath);
                        tPathFile = tAppPath + @"\FileReceive";
                        if (!Directory.Exists(tPathFile)) Directory.CreateDirectory(tPathFile);

                        oClient.DownloadFile(ptUrlFile, tPathFile + @"\" + tFileName);
                        tPathFile = tPathFile + @"\" + tFileName;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCtDownloadFile : " + oEx.Message);
            }
            return tPathFile;
        }

        private bool C_PRCbImpDataPdtTnfBch(string ptPathFile,string ptConnStr,int pnCmdTime)
        {
            cmlTCNTPdtTbx oPdtTbx;
            List<cmlTCNTPdtTbxHD> aoHD;
            List<cmlTCNTPdtTbxHDRef> aoHDRef;
            List<cmlTCNTPdtTbxDT> aoDT;
            cDataReader<cmlTCNTPdtTbxHD> oHD;
            cDataReader<cmlTCNTPdtTbxHDRef> oHDRef;
            cDataReader<cmlTCNTPdtTbxDT> oDT;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            SqlTransaction oTrans;
            string tDocNo = "";
            int nRowAffect = 0;
            try
            {
                if (string.IsNullOrEmpty(ptPathFile)) return false;

                if (!File.Exists(ptPathFile)) return false;
                oPdtTbx = new cmlTCNTPdtTbx();
                oPdtTbx = JsonConvert.DeserializeObject<cmlTCNTPdtTbx>(File.ReadAllText(ptPathFile));

                if (oPdtTbx != null)
                {

                    //ตรวจสอบเอกสารว่ามีหรือยัง
                    oSql.AppendLine("SELECT FTXthDocNo FROM TCNTPdtTbxHD WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTXthDocNo = '" + oPdtTbx.oTCNTPdtTbxHD.FTXthDocNo + "' AND FTXthStaPrcStk = '1'");
                    tDocNo = oDB.C_DAToExecuteQuery<string>(ptConnStr, oSql.ToString(), pnCmdTime);
                    if (string.IsNullOrEmpty(tDocNo))
                    {
                        oPdtTbx.oTCNTPdtTbxHD.FTXthStaPrcStk = "";  //เปลี่ยน status เพื่อประมวลสต๊อก

                        // Create Tmp
                        oSql.Clear();
                        oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TCNTPdtTbxHDTmp'))");
                        oSql.AppendLine("   BEGIN");
                        oSql.AppendLine("	    SELECT TOP 0 * INTO TCNTPdtTbxHDTmp FROM TCNTPdtTbxHD with(nolock)");
                        oSql.AppendLine("   END");
                        oSql.AppendLine("ELSE");
                        oSql.AppendLine("   BEGIN");
                        oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtTbxHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtTbxHD' ),0)");
                        oSql.AppendLine("       BEGIN");
                        oSql.AppendLine("   	    DROP TABLE TCNTPdtTbxHDTmp");
                        oSql.AppendLine("   	    SELECT TOP 0 * INTO TCNTPdtTbxHDTmp FROM TCNTPdtTbxHD with(nolock)");
                        oSql.AppendLine("       END");
                        oSql.AppendLine("   END");
                        oSql.AppendLine("TRUNCATE TABLE TCNTPdtTbxHDTmp");
                        oSql.AppendLine();
                        oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TCNTPdtTbxHDRefTmp'))");
                        oSql.AppendLine("   BEGIN");
                        oSql.AppendLine("	    SELECT TOP 0 * INTO TCNTPdtTbxHDRefTmp FROM TCNTPdtTbxHDRef with(nolock)");
                        oSql.AppendLine("   END");
                        oSql.AppendLine("ELSE");
                        oSql.AppendLine("   BEGIN");
                        oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtTbxHDRefTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtTbxHDRef' ),0)");
                        oSql.AppendLine("       BEGIN");
                        oSql.AppendLine("   	    DROP TABLE TCNTPdtTbxHDRefTmp");
                        oSql.AppendLine("   	    SELECT TOP 0 * INTO TCNTPdtTbxHDRefTmp FROM TCNTPdtTbxHDRef with(nolock)");
                        oSql.AppendLine("       END");
                        oSql.AppendLine("   END");
                        oSql.AppendLine("TRUNCATE TABLE TCNTPdtTbxHDRefTmp");
                        oSql.AppendLine();
                        oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TCNTPdtTbxDTTmp'))");
                        oSql.AppendLine("   BEGIN");
                        oSql.AppendLine("	    SELECT TOP 0 * INTO TCNTPdtTbxDTTmp FROM TCNTPdtTbxHDRef with(nolock)");
                        oSql.AppendLine("   END");
                        oSql.AppendLine("ELSE");
                        oSql.AppendLine("   BEGIN");
                        oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtTbxDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtTbxDT' ),0)");
                        oSql.AppendLine("       BEGIN");
                        oSql.AppendLine("   	    DROP TABLE TCNTPdtTbxDTTmp");
                        oSql.AppendLine("   	    SELECT TOP 0 * INTO TCNTPdtTbxDTTmp FROM TCNTPdtTbxHDRef with(nolock)");
                        oSql.AppendLine("       END");
                        oSql.AppendLine("   END");
                        oSql.AppendLine("TRUNCATE TABLE TCNTPdtTbxDTTmp");
                        oDB.C_DAToExecuteQuery(ptConnStr, oSql.ToString(), pnCmdTime);

                        using (SqlConnection oConn = new SqlConnection(ptConnStr))
                        {
                            oConn.Open();
                            aoHD = new List<cmlTCNTPdtTbxHD>();
                            aoHD.Add(oPdtTbx.oTCNTPdtTbxHD);
                            oHD = new cDataReader<cmlTCNTPdtTbxHD>(aoHD);
                            oTrans = oConn.BeginTransaction();

                            using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTrans))
                            {
                                foreach (string tColName in oHD.ColumnNames)
                                {
                                    oBulkCopy.ColumnMappings.Add(tColName, tColName);
                                }
                                oBulkCopy.BatchSize = 100;
                                oBulkCopy.DestinationTableName = "dbo.TCNTPdtTbxHDTmp";

                                try
                                {
                                    oBulkCopy.WriteToServer(oHD);
                                }
                                catch (Exception oEx)
                                {
                                    oTrans.Rollback();
                                    new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCbImpDataPdtTnfBch/TCNTPdtTbxHDTmp : " + oEx.Message);//*Em 61-11-15
                                    return false;
                                }
                            }

                            if(oPdtTbx.oTCNTPdtTbxHDRef != null)
                            {
                                aoHDRef = new List<cmlTCNTPdtTbxHDRef>();
                                aoHDRef.Add(oPdtTbx.oTCNTPdtTbxHDRef);
                                oHDRef = new cDataReader<cmlTCNTPdtTbxHDRef>(aoHDRef);

                                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTrans))
                                {
                                    foreach (string tColName in oHDRef.ColumnNames)
                                    {
                                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                                    }
                                    oBulkCopy.BatchSize = 100;
                                    oBulkCopy.DestinationTableName = "dbo.TCNTPdtTbxHDRefTmp";

                                    try
                                    {
                                        oBulkCopy.WriteToServer(oHDRef);
                                    }
                                    catch (Exception oEx)
                                    {
                                        oTrans.Rollback();
                                        new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCbImpDataPdtTnfBch/TCNTPdtTbxHDRefTmp : " + oEx.Message);//*Em 61-11-15
                                        return false;
                                    }
                                }
                            }

                            if (oPdtTbx.aoTCNTPdtTbxDT != null)
                            {
                                
                                oDT = new cDataReader<cmlTCNTPdtTbxDT>(oPdtTbx.aoTCNTPdtTbxDT);

                                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTrans))
                                {
                                    foreach (string tColName in oDT.ColumnNames)
                                    {
                                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                                    }
                                    oBulkCopy.BatchSize = 100;
                                    oBulkCopy.DestinationTableName = "dbo.TCNTPdtTbxDTTmp";

                                    try
                                    {
                                        oBulkCopy.WriteToServer(oDT);
                                    }
                                    catch (Exception oEx)
                                    {
                                        oTrans.Rollback();
                                        new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCbImpDataPdtTnfBch/TCNTPdtTbxDTTmp : " + oEx.Message);//*Em 61-11-15
                                        return false;
                                    }
                                }
                            }

                            oTrans.Commit();

                            // JOIN DELETE & Insert & Drop Table
                            oSql.Clear();
                            oSql.AppendLine("BEGIN TRY");
                            oSql.AppendLine("   BEGIN TRANSACTION");
                            oSql.AppendLine("   DELETE HD ");
                            oSql.AppendLine("   FROM TCNTPdtTbxHD HD WITH(ROWLOCK)");
                            oSql.AppendLine("   INNER JOIN TCNTPdtTbxHDTmp HDT WITH(NOLOCK) ON HD.FTBchCode = HDT.FTBchCode AND HD.FTXthDocNo = HDT.FTXthDocNo");
                            oSql.AppendLine();
                            oSql.AppendLine("   INSERT INTO TCNTPdtTbxHD");
                            oSql.AppendLine("   SELECT * FROM TCNTPdtTbxHDTmp WITH(NOLOCK) ");
                            oSql.AppendLine();
                            oSql.AppendLine("   DELETE HDR ");
                            oSql.AppendLine("   FROM TCNTPdtTbxHDRef HDR WITH(ROWLOCK)");
                            oSql.AppendLine("   INNER JOIN TCNTPdtTbxHDRefTmp HDRT WITH(NOLOCK) ON HDR.FTBchCode = HDRT.FTBchCode AND HDR.FTXthDocNo = HDRT.FTXthDocNo");
                            oSql.AppendLine();
                            oSql.AppendLine("   INSERT INTO TCNTPdtTbxHDRef");
                            oSql.AppendLine("   SELECT * FROM TCNTPdtTbxHDRefTmp WITH(NOLOCK) ");
                            oSql.AppendLine();
                            oSql.AppendLine("   DELETE DT ");
                            oSql.AppendLine("   FROM TCNTPdtTbxDT DT WITH(ROWLOCK)");
                            oSql.AppendLine("   INNER JOIN TCNTPdtTbxDTTmp DTT WITH(NOLOCK) ON DT.FTBchCode = DTT.FTBchCode AND DT.FTXthDocNo = DTT.FTXthDocNo");
                            oSql.AppendLine();
                            oSql.AppendLine("   INSERT INTO TCNTPdtTbxDT");
                            oSql.AppendLine("   SELECT * FROM TCNTPdtTbxDTTmp WITH(NOLOCK) ");
                            oSql.AppendLine();
                            oSql.AppendLine("   COMMIT TRANSACTION");
                            oSql.AppendLine("END TRY");
                            oSql.AppendLine("BEGIN CATCH");
                            oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                            oSql.AppendLine("       ROLLBACK TRAN;");
                            oSql.AppendLine("   THROW;");
                            oSql.AppendLine("END CATCH");
                            oDB.C_DATbExecuteNonQuery(ptConnStr, oSql.ToString(), pnCmdTime,out int nRowEff);
                        }

                        //ตรวจสอบว่ามีเอกสารหรือยัง
                        oSql.AppendLine("SELECT FTXthDocNo FROM TCNTPdtTbxHD WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTXthDocNo = '" + oPdtTbx.oTCNTPdtTbxHD.FTXthDocNo + "'");
                        tDocNo = oDB.C_DAToExecuteQuery<string>(ptConnStr, oSql.ToString(), pnCmdTime);
                        if (string.IsNullOrEmpty(tDocNo))
                        {
                            return false;
                        }
                        else
                        {
                            SqlParameter[] oPara = new SqlParameter[] {
                                new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = oPdtTbx.oTCNTPdtTbxHD.FTBchCode},
                                new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = tDocNo},
                                new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = "MQReceivePrc"},
                                new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                            };
                            if (oDB.C_DATbExecuteStoreProcedure(ptConnStr, "STP_DOCxBchPdtTnf", ref oPara, pnCmdTime, "@FNResult"))
                            {
                                oSql = new StringBuilder();
                                oSql.AppendLine("UPDATE TCNTPdtTbxHD with(rowlock)");
                                oSql.AppendLine("SET FTXthStaApv = '1'");
                                oSql.AppendLine(",FTXthStaPrcStk = '1' ");
                                oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                oSql.AppendLine(",FTLastUpdBy = 'MQReceivePrc' ");
                                oSql.AppendLine("WHERE FTBchCode = '" + oPdtTbx.oTCNTPdtTbxHD.FTBchCode + "'");
                                oSql.AppendLine("AND FTXthDocNo = '" + tDocNo + "'");
                                oDB.C_DATbExecuteNonQuery(ptConnStr, oSql.ToString(), pnCmdTime, out nRowAffect);
                                if (nRowAffect == 0)
                                {
                                    return false;
                                }
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //มีแล้วไม่ทำอะไร
                    }

                }

                //Backup file
                C_PRCxBackupFile(ptPathFile);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCbImpDataPdtTnfBch : " + oEx.Message);
            }
            finally
            {
                
            }
            return true;
        }

        private void C_PRCxBackupFile(string ptPathFile)
        {
            string tPathBackup = "";
            string tFileName = "";
            string tAppPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            try
            {
                if (!File.Exists(ptPathFile)) return;

                tFileName = new FileInfo(ptPathFile).Name;
                tPathBackup = tAppPath + @"\FileBackup";
                if (!Directory.Exists(tPathBackup)) Directory.CreateDirectory(tPathBackup);

                File.Copy(ptPathFile, tPathBackup + @"\" + tFileName);

                File.Delete(ptPathFile);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCxBackupFile : " + oEx.Message);
            }
        }

        public bool C_PRCbUpdBchReceive(cmlTCNTSync2BchHis poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tConnStr = oDB.C_GETtConnectString(poShopDB.tServer, poShopDB.tUser, poShopDB.tPassword, poShopDB.tDatabase, (int)poShopDB.nConnectTimeOut, poShopDB.tAuthenMode);
            int nCmdTime = (int)poShopDB.nCommandTimeOut;
            int nRowEff =0;
            try
            {
                oSql.AppendLine("UPDATE TCNTSync2BchHis WITH(ROWLOCK)");
                oSql.AppendLine("SET FTSynStatus = '" + poData.FTSynStatus + "'");
                oSql.AppendLine(",FDSynRcvDate = '"+  Convert.ToDateTime(poData.FDSynRcvDate).ToString("yyyy-MM-dd HH:mm:ss") +"'");
                oSql.AppendLine("WHERE FTSynBchFrm = '" + poData.FTSynBchFrm + "'");
                oSql.AppendLine("AND FTSynBchTo = '" + poData.FTSynBchTo +"'");
                oSql.AppendLine("AND FTSynFileName = '" + poData.FTSynFileName + "'");
                oSql.AppendLine("AND FNSynSeqNo = " + poData.FNSynSeqNo);
                if (oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), nCmdTime, out nRowEff))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCbUpdBchReceive : " + oEx.Message);
                return false;
            }
        }

        public bool C_PRCbBchDownload(cmlBchDownload poBchDwn,ref string ptErrMsg)
        {
            cSyncData oSyncData;
            try
            {
                if (poBchDwn == null) return false;

                oSyncData = new cSyncData();
                if (cVB.tVB_BchCode == poBchDwn.ptFilter || string.IsNullOrEmpty(poBchDwn.ptFilter) || string.Equals(poBchDwn.ptFilter, "*"))
                {
                    oSyncData.C_PRCxSyncDwn(oSyncData.C_GETaTableSyncDB(), DateTime.Now.ToString("yyyy-MM-dd"));
                }
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_WRTxLog("cPrcBch2Bch", "C_PRCbBchDownload : " + oEx.Message);
                return false;
            }
        }
    }
}
