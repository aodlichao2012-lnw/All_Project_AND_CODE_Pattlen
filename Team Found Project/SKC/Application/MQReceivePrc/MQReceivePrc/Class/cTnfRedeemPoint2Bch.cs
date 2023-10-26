using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Bch2Bch;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.RedeemPoint;
using MQReceivePrc.Models.WebService.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class cTnfRedeemPoint2Bch
    {
        /// <summary>
        /// *Arm 63-02-25
        /// HQ สร้าง URL สำหรับดาวน์โหลดเอกสาร Redeem Point ตามที่ Store Back ส่งมา
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbCreateUrlRedeemPoint(cmlBchDownload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlDataRedeemPoint oData;
            cmlTARTRedeem oTARTRedeem;
            //cmlDataUrl oDataUrl;
            StringBuilder oSql;
            cDatabase oDB;
            cSP oSP = new cSP();
            int nCmdTime = (int)poShopDB.nCommandTimeOut;
            string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);

            // for Cerate File JSON
            string tPath = "";
            string tPathData = "";
            string tUrl = "";

            // For Call API
            string tAPI2PSMaster = "";
            string tAPIUrl = "";
            string tXKey = "";
            string tAPIHeader = "";
            string tUrlFunc = "/FileManage/CreateURL";
            
            try
            {
                //*Arm 63-03-31 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                {
                    
                    if (poData == null) return false;
                    if (string.IsNullOrEmpty(poData.ptData)) return false;
                    oData = JsonConvert.DeserializeObject<cmlDataRedeemPoint>(poData.ptData);

                    oDB = new cDatabase();
                    oTARTRedeem = new cmlTARTRedeem();
                    oSql = new StringBuilder();

                    // TARTRedeemHD
                    oSql.AppendLine("SELECT FTBchCode, FTRdhDocNo, FDRdhDocDate, FTRdhDocType, FTRdhCalType,");
                    oSql.AppendLine("FDRdhDStart, FDRdhDStop, FDRdhTStart, FDRdhTStop,");
                    oSql.AppendLine("FTRdhStaClosed, FTRdhStaDoc, FTRdhStaApv, FTRdhStaPrcDoc, FNRdhStaDocAct,");
                    oSql.AppendLine("FTUsrCode, FTRdhUsrApv, FTRdhStaOnTopPmt, FNRdhLimitQty,");
                    oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                    oSql.AppendLine("FROM TARTRedeemHD WITH(NOLOCK) ");
                    oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTRdhDocNo='" + oData.ptDocNo + "'");
                    oTARTRedeem.aoTARTRedeemHD = oDB.C_GETaDataQuery<cmlTARTRedeemHD>(t_ConnStr, oSql.ToString(), nCmdTime);

                    if (oTARTRedeem.aoTARTRedeemHD.Count > 0)
                    {
                        // TARTRedeemHD_L
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FTBchCode, FTRdhDocNo, FNLngID,");
                        oSql.AppendLine("FTRdhName, FTRdhNameSlip, FTRdhRmk");
                        oSql.AppendLine("FROM TARTRedeemHD_L WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTRdhDocNo='" + oData.ptDocNo + "'");

                        oTARTRedeem.aoTARTRedeemHD_L = oDB.C_GETaDataQuery<cmlTARTRedeemHD_L>(t_ConnStr, oSql.ToString(), nCmdTime);


                        // TARTRedeemHDBch
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FTBchCode, FTRdhDocNo, FTRdhBchTo,");
                        oSql.AppendLine("FTRdhMerTo, FTRdhShpTo, FTRdhStaType");
                        oSql.AppendLine("FROM TARTRedeemHDBch WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTRdhDocNo='" + oData.ptDocNo + "'");

                        oTARTRedeem.aoTARTRedeemHDBch = oDB.C_GETaDataQuery<cmlTARTRedeemHDBch>(t_ConnStr, oSql.ToString(), nCmdTime);

                        // TARTRedeemHDCstPri
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FTBchCode, FTRdhDocNo, FTPplCode, FTRdhStaType");
                        oSql.AppendLine("FROM TARTRedeemHDCstPri WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTRdhDocNo='" + oData.ptDocNo + "'");
                        oTARTRedeem.aoTARTRedeemHDCstPri = oDB.C_GETaDataQuery<cmlTARTRedeemHDCstPri>(t_ConnStr, oSql.ToString(), nCmdTime);

                        // TARTRedeemDT
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FTBchCode, FTRdhDocNo, FNRddSeq,");
                        oSql.AppendLine("FTRddStaType, FTRddGrpName, FTPdtCode,");
                        oSql.AppendLine("FTPunCode, FTRddBarCode");
                        oSql.AppendLine("FROM TARTRedeemDT WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTRdhDocNo='" + oData.ptDocNo + "'");
                        oTARTRedeem.aoTARTRedeemDT = oDB.C_GETaDataQuery<cmlTARTRedeemDT>(t_ConnStr, oSql.ToString(), nCmdTime);

                        // TARTRedeemCD
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FTBchCode, FTRdhDocNo, FNRdcSeq,");
                        oSql.AppendLine("FTRddGrpName, FTRdcRefCode, FCRdcUsePoint,");
                        oSql.AppendLine("FCRdcUseMny, FCRdcMinTotBill");
                        oSql.AppendLine("FROM TARTRedeemCD WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTRdhDocNo='" + oData.ptDocNo + "'");
                        oTARTRedeem.aoTARTRedeemCD = oDB.C_GETaDataQuery<cmlTARTRedeemCD>(t_ConnStr, oSql.ToString(), nCmdTime);

                    }

                    // # Set Path สำหรับเก็บ File
                    tPath = Directory.GetCurrentDirectory() + @"\FileSender\RedeemPoint\";

                    if (!Directory.Exists(tPath))
                    {
                        Directory.CreateDirectory(tPath);
                    }
                    else
                    {
                        // เช็ค Path มีไฟล์ RedeemPoint.json อยู่ใน Path   
                        if (File.Exists(Path.Combine(tPath, "RedeemPoint.json")))
                        {
                            // ถ้ามี มีไฟล์ RedeemPoint.json , ลบทิ้ง
                            File.Delete(Path.Combine(tPath, "RedeemPoint.json"));
                        }

                    }

                    // # Convert Model เป็น JSON File บันทึกลง Path
                    if (oTARTRedeem.aoTARTRedeemHD.Count > 0)
                    {
                        tPathData = tPath + @"RedeemPoint.json";
                        File.WriteAllText(tPathData, JsonConvert.SerializeObject(oTARTRedeem));

                        string tMsgPath = JsonConvert.SerializeObject(tPathData);
                        tAPI2PSMaster = new cSP().SP_GETtUrlAPI(t_ConnStr, nCmdTime, cVB.tVB_BchCode, 4, ref tXKey, ref tAPIHeader);
                        tAPIUrl = tAPI2PSMaster + tUrlFunc;

                        cClientService oCall = new cClientService();
                        oCall = new cClientService(tAPIHeader, tXKey);
                        HttpResponseMessage oRep = new HttpResponseMessage();
                        try
                        {
                            oRep = oCall.C_POSToInvoke(tAPIUrl, tMsgPath);
                        }
                        catch (Exception oEx)
                        {
                            new cLog().C_WRTxLog("cTnfRedeemPoint2Bch", "C_PRCbCreateUrlRedeemPoint/GenerateURL : " + oEx.Message);
                            ptErrMsg = oEx.Message;
                            return false;
                        }

                        if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string tJsonResult = oRep.Content.ReadAsStringAsync().Result;
                            if (!string.IsNullOrEmpty(tJsonResult))
                            {
                                cmlResItem<string> oResult = JsonConvert.DeserializeObject<cmlResItem<string>>(tJsonResult);
                                if (oResult.rtCode == "1")
                                {
                                    tUrl = oResult.roItem;
                                }
                                else
                                {
                                    ptErrMsg = oResult.rtDesc;
                                    return false;
                                }
                            }

                        }

                        if (!string.IsNullOrEmpty(tUrl))
                        {
                            //GET fromat Message Json
                            cmlBchDownload oUpload = new cmlBchDownload();
                            oUpload.ptFunction = "RedeemPoint";
                            oUpload.ptSource = "HQ.MQReceivePrc";
                            oUpload.ptDest = "BCH.MQReceivePrc";
                            oUpload.ptFilter = cVB.tVB_BchCode;
                            oUpload.ptData = JsonConvert.SerializeObject(tUrl);
                            string tMsgJSON = JsonConvert.SerializeObject(oUpload);

                            //Publish to Exchange
                            cFunction.C_PRCxMQPublishExchange("BR_XTransfer", "", "fanout", tMsgJSON, out ptErrMsg);
                        }

                    }
                }
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cTnfRedeemPoint2Bch", "C_PRCbCreateUrlRedeemPoint : " + oEx.Message);
                return false;
            }
        }

        /// <summary>
        /// *Arm 63-02-25
        /// สาขาดาวน์โหลดเอกสาร Redeem Point จาก Url ที่ส่งมา
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbDownloadRedeemPoint(cmlBchDownload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cSP oSP = new cSP();
            cmlDataUrl oData;

            cmlTARTRedeem oTARTRedeem;
            cDataReader<cmlTARTRedeemHD> aoHD;
            cDataReader<cmlTARTRedeemHD_L> aoHD_L;
            cDataReader<cmlTARTRedeemHDBch> aoHDBch;
            cDataReader<cmlTARTRedeemHDCstPri> aoHDCstPri;
            cDataReader<cmlTARTRedeemDT> aoDT;
            cDataReader<cmlTARTRedeemCD> aoCD;

            StringBuilder oSql;
            
            cDatabase oDB;
            SqlTransaction oTransaction;
            SqlConnection oConn;
            int nCmdTime = (int)poShopDB.nCommandTimeOut;
            string tConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);

            string tURL = "";
            string tPath = "";
            string tMsgErrDwn = "";
            int nRowAffect;

            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                //oData = JsonConvert.DeserializeObject<cmlDataUrl>(poData.ptData);
                tURL = JsonConvert.DeserializeObject<string>(poData.ptData);
                //tURL = oData.ptURLJson; // URL สำหรับ Download file

                oTARTRedeem = new cmlTARTRedeem();
                oSql = new StringBuilder();
                oDB = new cDatabase();
                

                // # Start Process Download file
                //*******************************************
                
                tPath = oSP.SP_PRCtDownloadFileDefault(tURL,"RedeemPoint", ref tMsgErrDwn);
                if (string.IsNullOrEmpty(tPath))
                {
                    ptErrMsg = tMsgErrDwn;
                    return false;
                }

                //*******************************************
                //# End Process Download file



                // # Start Process Import Data
                //*******************************************
                oTARTRedeem = JsonConvert.DeserializeObject<cmlTARTRedeem>(File.ReadAllText(tPath));

                //Create Temp
                // TARTRedeemHD
                oSql.Clear();
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TARTRedeemHDTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TARTRedeemHDTmp FROM TARTRedeemHD with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemHD' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TARTRedeemHDTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TARTRedeemHDTmp FROM TARTRedeemHD with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TARTRedeemHDTmp");
                oSql.AppendLine("");
                // TARTRedeemHD_L
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TARTRedeemHDTmp_L'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TARTRedeemHDTmp_L FROM TARTRedeemHD_L with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemHDTmp_L' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemHD_L' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TARTRedeemHDTmp_L");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TARTRedeemHDTmp_L FROM TARTRedeemHD_L with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TARTRedeemHDTmp_L");
                oSql.AppendLine("");
                // TARTRedeemHDBch
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TARTRedeemHDBchTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TARTRedeemHDBchTmp FROM TARTRedeemHDBch with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemHDBchTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemHDBch' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TARTRedeemHDBchTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TARTRedeemHDBchTmp FROM TARTRedeemHDBch with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TARTRedeemHDBchTmp");
                // TARTRedeemHDCstPri
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TARTRedeemHDCstPriTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TARTRedeemHDCstPriTmp FROM TARTRedeemHDCstPri with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemHDCstPriTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemHDCstPri' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TARTRedeemHDCstPriTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TARTRedeemHDCstPriTmp FROM TARTRedeemHDCstPri with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TARTRedeemHDCstPriTmp");
                // TARTRedeemDT
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TARTRedeemDTTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TARTRedeemDTTmp FROM TARTRedeemDT with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemDT' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TARTRedeemDTTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TARTRedeemDTTmp FROM TARTRedeemDT with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TARTRedeemDTTmp");
                // TARTRedeemCD
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TARTRedeemCDTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TARTRedeemCDTmp FROM TARTRedeemCD with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemCDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TARTRedeemCD' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TARTRedeemCDTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TARTRedeemCDTmp FROM TARTRedeemCD with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TARTRedeemCDTmp");
                oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), nCmdTime, out nRowAffect);

                oConn = new SqlConnection(tConnStr);
                oConn.Open();

                oTransaction = oConn.BeginTransaction();

                // Bulk Copy : TARTRedeemHD
                if (oTARTRedeem.aoTARTRedeemHD != null)
                {
                    aoHD = new cDataReader<cmlTARTRedeemHD>(oTARTRedeem.aoTARTRedeemHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TARTRedeemHDTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoHD);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                // Bulk Copy : TARTRedeemHD_L
                if (oTARTRedeem.aoTARTRedeemHD_L != null)
                {
                    aoHD_L = new cDataReader<cmlTARTRedeemHD_L>(oTARTRedeem.aoTARTRedeemHD_L);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHD_L.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TARTRedeemHDTmp_L";

                        try
                        {
                            oBulkCopy.WriteToServer(aoHD_L);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }
                
                // Bulk Copy : TARTRedeemHDBch
                if (oTARTRedeem.aoTARTRedeemHDBch != null)
                {
                    aoHDBch = new cDataReader<cmlTARTRedeemHDBch>(oTARTRedeem.aoTARTRedeemHDBch);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHDBch.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TARTRedeemHDBchTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoHDBch);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                // Bulk Copy : TARTRedeemHDBCstPri
                if (oTARTRedeem.aoTARTRedeemHDCstPri != null)
                {
                    aoHDCstPri = new cDataReader<cmlTARTRedeemHDCstPri>(oTARTRedeem.aoTARTRedeemHDCstPri);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHDCstPri.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TARTRedeemHDCstPriTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoHDCstPri);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                // Bulk Copy : TARTRedeemDT
                if (oTARTRedeem.aoTARTRedeemDT != null)
                {
                    aoDT = new cDataReader<cmlTARTRedeemDT>(oTARTRedeem.aoTARTRedeemDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TARTRedeemDTTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoDT);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                // Bulk Copy : TARTRedeemCD
                if (oTARTRedeem.aoTARTRedeemCD != null)
                {
                    aoCD = new cDataReader<cmlTARTRedeemCD>(oTARTRedeem.aoTARTRedeemCD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoCD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TARTRedeemCDTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoCD);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                oTransaction.Commit();

                // INSERT 
                oSql.Clear();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine();
                // INSERT TARTRedeemHD
                oSql.AppendLine("   DELETE HD");
                oSql.AppendLine("   FROM TARTRedeemHD HD WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TARTRedeemHDTmp HDTmp WITH(NOLOCK) ON HD.FTBchCode = HDTmp.FTBchCode AND HD.FTRdhDocNo = HDTmp.FTRdhDocNo");
                oSql.AppendLine("   ");
                oSql.AppendLine("   INSERT INTO TARTRedeemHD");
                oSql.AppendLine("   SELECT * FROM TARTRedeemHDTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TARTRedeemHD_L
                oSql.AppendLine("   DELETE HDL");
                oSql.AppendLine("   FROM TARTRedeemHD_L HDL WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TARTRedeemHDTmp_L HDLTmp WITH(NOLOCK) ON HDL.FTBchCode = HDLTmp.FTBchCode AND HDL.FTRdhDocNo = HDLTmp.FTRdhDocNo AND HDL.FNLngID = HDLTmp.FNLngID");
                oSql.AppendLine("   ");
                oSql.AppendLine("   INSERT INTO TARTRedeemHD_L");
                oSql.AppendLine("   SELECT * FROM TARTRedeemHDTmp_L WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TARTRedeemHDBch
                oSql.AppendLine("   DELETE BCH");
                oSql.AppendLine("   FROM TARTRedeemHDBch BCH WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TARTRedeemHDBchTmp BCHTmp WITH(NOLOCK) ON BCH.FTBchCode = BCHTmp.FTBchCode AND BCH.FTRdhDocNo = BCHTmp.FTRdhDocNo ");
                oSql.AppendLine("   ");
                oSql.AppendLine("   INSERT INTO TARTRedeemHDBch");
                oSql.AppendLine("   SELECT * FROM TARTRedeemHDBchTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TARTRedeemHDCstPri
                oSql.AppendLine("   DELETE CSTP");
                oSql.AppendLine("   FROM TARTRedeemHDCstPri CSTP WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TARTRedeemHDCstPriTmp CSTPTmp WITH(NOLOCK) ON CSTP.FTBchCode = CSTPTmp.FTBchCode AND CSTP.FTRdhDocNo = CSTPTmp.FTRdhDocNo AND CSTP.FTPplCode = CSTPTmp.FTPplCode");
                oSql.AppendLine("   ");
                oSql.AppendLine("   INSERT INTO TARTRedeemHDCstPri");
                oSql.AppendLine("   SELECT * FROM TARTRedeemHDCstPriTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TARTRedeemDT
                oSql.AppendLine("   DELETE DT");
                oSql.AppendLine("   FROM TARTRedeemDT DT WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TARTRedeemDTTmp DTTmp WITH(NOLOCK) ON DT.FTBchCode = DTTmp.FTBchCode AND DT.FTRdhDocNo = DTTmp.FTRdhDocNo AND DT.FNRddSeq = DTTmp.FNRddSeq");
                oSql.AppendLine("   ");
                oSql.AppendLine("   INSERT INTO TARTRedeemDT");
                oSql.AppendLine("   SELECT * FROM TARTRedeemDTTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TARTRedeemCD
                oSql.AppendLine("   DELETE CD");
                oSql.AppendLine("   FROM TARTRedeemCD CD WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TARTRedeemCDTmp CDTmp WITH(NOLOCK) ON CD.FTBchCode = CDTmp.FTBchCode AND CD.FTRdhDocNo = CDTmp.FTRdhDocNo AND CD.FNRdcSeq = CDTmp.FNRdcSeq");
                oSql.AppendLine("   ");
                oSql.AppendLine("   INSERT INTO TARTRedeemCD");
                oSql.AppendLine("   SELECT * FROM TARTRedeemCDTmp WITH(NOLOCK)");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");

                oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), nCmdTime, out nRowAffect);

                //*******************************************
                //# End Process Import Data

                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cTnfRedeemPoint2Bch", "C_PRCbDownloadRedeemPoint : " + oEx.Message);
                return false;
            }
        } 
    }
}
