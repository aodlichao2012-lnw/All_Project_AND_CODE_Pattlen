using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Bch2Bch;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Coupon;
using MQReceivePrc.Models.WebService.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    class cTnfCouponGiftVoucher2Bch
    {
        /// <summary>
        /// *Net 63-02-26
        /// HQ สร้าง URL สำหรับดาวน์โหลดเอกสาร CouponGiftVoucher ตามที่ Store Back ส่งมา
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbCreateUrlCouponGiftVoucher(cmlBchDownload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlDataCouponGiftVoucher oData;
            cmlTFNTCoupon oTFNTCoupon;
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
                    oData = JsonConvert.DeserializeObject<cmlDataCouponGiftVoucher>(poData.ptData);

                    oDB = new cDatabase();
                    oTFNTCoupon = new cmlTFNTCoupon();
                    oSql = new StringBuilder();

                    // TFNTCouponHD
                    oSql.AppendLine("SELECT FTBchCode, FTCphDocNo, FTCptCode, ");
                    oSql.AppendLine("FDCphDocDate, FTCphDisType, FCCphDisValue, ");
                    oSql.AppendLine("FTPplCode, FDCphDateStart, FDCphDateStop, ");
                    oSql.AppendLine("FTCphTimeStart, FTCphTimeStop, FTCphStaClosed, ");
                    oSql.AppendLine("FTUsrCode, FTCphUsrApv, FTCphStaDoc, ");
                    oSql.AppendLine("FTCphStaApv, FTCphStaPrcDoc, FTCphStaDelMQ, ");
                    oSql.AppendLine("FCCphMinValue, FTCphStaOnTopPmt, FNCphLimitUsePerBill, ");
                    oSql.AppendLine("FTCphRefAccCode, FTStaChkMember, FDLastUpdOn, ");
                    oSql.AppendLine("FTLastUpdBy, FDCreateOn, FTCreateBy");
                    oSql.AppendLine(" FROM TFNTCouponHD WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNTCouponHD = oDB.C_GETaDataQuery<cmlTFNTCouponHD>(t_ConnStr, oSql.ToString(), nCmdTime);

                    if (oTFNTCoupon.aoTFNTCouponHD.Count == 0)
                        throw new Exception($"{oData.ptDocNo} has no data.");

                    // TFNTCouponHD_L
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTCphDocNo, FNLngID, FTCpnName, ");
                    oSql.AppendLine("FTCpnMsg1, FTCpnMsg2, FTCpnCond");
                    oSql.AppendLine(" FROM TFNTCouponHD_L WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNTCouponHD_L = oDB.C_GETaDataQuery<cmlTFNTCouponHD_L>(t_ConnStr, oSql.ToString(), nCmdTime);

                    // TFNMCouponType
                    oSql = new StringBuilder();
                    oSql.AppendLine($"SELECT CPNT.FTCptCode, FTCptType, FTCptStaChk, ");
                    oSql.AppendLine($"FTCptStaChkHQ, FTCptStaUse, CPNT.FDLastUpdOn, ");
                    oSql.AppendLine($"CPNT.FTLastUpdBy, CPNT.FDCreateOn, CPNT.FTCreateBy");
                    oSql.AppendLine($" FROM TFNMCouponType CPNT WITH(NOLOCK)");
                    oSql.AppendLine($" INNER JOIN TFNTCouponHD HD WITH(NOLOCK) ON HD.FTCptCode = CPNT.FTCptCode");
                    oSql.AppendLine("WHERE HD.FTBchCode = '" + oData.ptBchCode + "' AND HD.FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNMCouponType = oDB.C_GETaDataQuery<cmlTFNMCouponType>(t_ConnStr, oSql.ToString(), nCmdTime);

                    // TFNMCouponType_L
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT CPNTL.FTCptCode, CPNTL.FNLngID, CPNTL.FTCptName, ");
                    oSql.AppendLine("FTCptRemark FROM TFNMCouponType_L CPNTL WITH(NOLOCK)");
                    oSql.AppendLine($" INNER JOIN TFNTCouponHD HD WITH(NOLOCK) ON HD.FTCptCode = CPNTL.FTCptCode");
                    oSql.AppendLine("WHERE HD.FTBchCode = '" + oData.ptBchCode + "' AND HD.FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNMCouponType_L = oDB.C_GETaDataQuery<cmlTFNMCouponType_L>(t_ConnStr, oSql.ToString(), nCmdTime);

                    // TFNTCouponDT
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTBchCode, FTCphDocNo, FTCpdBarCpn, ");
                    oSql.AppendLine("FNCpdSeqNo, FNCpdAlwMaxUse FROM TFNTCouponDT WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNTCouponDT = oDB.C_GETaDataQuery<cmlTFNTCouponDT>(t_ConnStr, oSql.ToString(), nCmdTime);

                    // TFNTCouponDTHis
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTBchCode, FTCphDocNo, FTCpdBarCpn, ");
                    oSql.AppendLine("FNCpbSeqID, FDCpbFrmStart, FTCpbFrmBch, ");
                    oSql.AppendLine("FTCpbFrmPos, FTCpbFrmSalRef, FTCpbStaBook, ");
                    oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, ");
                    oSql.AppendLine("FTCreateBy FROM TFNTCouponDTHis WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNTCouponDTHis = oDB.C_GETaDataQuery<cmlTFNTCouponDTHis>(t_ConnStr, oSql.ToString(), nCmdTime);

                    // TFNTCouponHDBch
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTBchCode, FTCphDocNo, FTCphBchTo, ");
                    oSql.AppendLine("FTCphMerTo, FTCphShpTo, FTCphStaType");
                    oSql.AppendLine(" FROM TFNTCouponHDBch WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNTCouponHDBch = oDB.C_GETaDataQuery<cmlTFNTCouponHDBch>(t_ConnStr, oSql.ToString(), nCmdTime);

                    // TFNTCouponHDCstPri
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTBchCode, FTCphDocNo, FTPplCode, ");
                    oSql.AppendLine("FTCphStaType FROM TFNTCouponHDCstPri WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNTCouponHDCstPri = oDB.C_GETaDataQuery<cmlTFNTCouponHDCstPri>(t_ConnStr, oSql.ToString(), nCmdTime);

                    // TFNTCouponHDPdt
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTBchCode, FTCphDocNo, FTPdtCode, ");
                    oSql.AppendLine("FTPunCode, FTCphStaType FROM TFNTCouponHDPdt WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + oData.ptBchCode + "' AND FTCphDocNo='" + oData.ptDocNo + "'");
                    oTFNTCoupon.aoTFNTCouponHDPdt = oDB.C_GETaDataQuery<cmlTFNTCouponHDPdt>(t_ConnStr, oSql.ToString(), nCmdTime);


                    // # Set Path สำหรับเก็บ File
                    tPath = Directory.GetCurrentDirectory() + @"\FileSender\CouponGiftVoucher\";

                    if (!Directory.Exists(tPath))
                    {
                        Directory.CreateDirectory(tPath);
                    }
                    else
                    {
                        // เช็ค Path มีไฟล์ RedeemPoint.json อยู่ใน Path   
                        if (File.Exists(Path.Combine(tPath, "CouponGiftVoucher.json")))
                        {
                            // ถ้ามี มีไฟล์ RedeemPoint.json , ลบทิ้ง
                            File.Delete(Path.Combine(tPath, "CouponGiftVoucher.json"));
                        }

                    }

                    // # Convert Model เป็น JSON File บันทึกลง Path
                    tPathData = tPath + @"CouponGiftVoucher.json";
                    File.WriteAllText(tPathData, JsonConvert.SerializeObject(oTFNTCoupon));

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
                        new cLog().C_WRTxLog("cTnfCouponGiftVoucher2Bch", "C_PRCbCreateUrlCouponGiftVoucher/GenerateURL : " + oEx.Message);
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
                        oUpload.ptFunction = "CouponGiftVoucher";
                        oUpload.ptSource = "HQ.MQReceivePrc";
                        oUpload.ptDest = "BCH.MQReceivePrc";
                        oUpload.ptFilter = cVB.tVB_BchCode;
                        oUpload.ptData = JsonConvert.SerializeObject(tUrl);
                        string tMsgJSON = JsonConvert.SerializeObject(oUpload);

                        //Publish to Exchange
                        cFunction.C_PRCxMQPublishExchange("BR_XTransfer", "", "fanout", tMsgJSON, out ptErrMsg);
                    }
                    
                }
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cTnfCouponGiftVoucher2Bch", "C_PRCbCreateUrlCouponGiftVoucher : " + oEx.Message);
                return false;
            }
        }

        /// <summary>
        /// *Net 63-03-13
        /// สาขาดาวน์โหลดเอกสาร CouponGiftVoucher จาก Url ที่ส่งมา
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbDownloadCouponGiftVoucher(cmlBchDownload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            
            cSP oSP = new cSP();
            cmlDataUrl oData;
            /*
            cmlTARTRedeem oTFNTCoupon;
            cDataReader<cmlTFNTCouponHD> aoHD;
            cDataReader<cmlTARTRedeemHD_L> aoHD_L;
            cDataReader<cmlTARTRedeemHDBch> aoHDBch;
            cDataReader<cmlTARTRedeemHDCstPri> aoHDCstPri;
            cDataReader<cmlTARTRedeemDT> aoDT;
            cDataReader<cmlTARTRedeemCD> aoCD;*/
            cmlTFNTCoupon oTFNTCoupon;
            cDataReader<cmlTFNTCouponHD> aoHD;
            cDataReader<cmlTFNTCouponHD_L> aoHD_L;
            cDataReader<cmlTFNTCouponHDBch> aoHDBch;
            cDataReader<cmlTFNTCouponHDCstPri> aoHDCstPri;
            cDataReader<cmlTFNTCouponHDPdt> aoHDPdt;
            cDataReader<cmlTFNTCouponDT> aoDT;
            cDataReader<cmlTFNTCouponDTHis> aoDTHis;
            cDataReader<cmlTFNMCouponType> aoType;
            cDataReader<cmlTFNMCouponType_L> aoType_L;
            StringBuilder oSql;

            cDatabase oDB;
            cDatabaseSP oDBSP;
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

                oTFNTCoupon = new cmlTFNTCoupon();
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oDBSP =new cDatabaseSP(tConnStr, poShopDB);

                // # Start Process Download file
                //*******************************************

                tPath = oSP.SP_PRCtDownloadFileDefault(tURL, "CouponGiftVoucher", ref tMsgErrDwn);
                if (string.IsNullOrEmpty(tPath))
                {
                    ptErrMsg = tMsgErrDwn;
                    return false;
                }

                //*******************************************
                //# End Process Download file



                // # Start Process Import Data
                //*******************************************
                oTFNTCoupon = JsonConvert.DeserializeObject<cmlTFNTCoupon>(File.ReadAllText(tPath));

                //Create Temp
                oDBSP.C_PRCbCreateTableTemp("TFNTCouponHD", "TFNTCouponHDTmp", ref ptErrMsg);
                oDBSP.C_PRCbCreateTableTemp("TFNTCouponHD_L", "TFNTCouponHDTmp_L", ref ptErrMsg);
                oDBSP.C_PRCbCreateTableTemp("TFNTCouponHDBch", "TFNTCouponHDBchTmp", ref ptErrMsg);
                oDBSP.C_PRCbCreateTableTemp("TFNTCouponHDCstPri", "TFNTCouponHDCstPriTmp", ref ptErrMsg);
                oDBSP.C_PRCbCreateTableTemp("TFNTCouponHDPdt", "TFNTCouponHDPdtTmp", ref ptErrMsg);
                oDBSP.C_PRCbCreateTableTemp("TFNTCouponDT", "TFNTCouponDTTmp", ref ptErrMsg);
                oDBSP.C_PRCbCreateTableTemp("TFNTCouponDTHis", "TFNTCouponDTHisTmp", ref ptErrMsg);
                oDBSP.C_PRCbCreateTableTemp("TFNMCouponType", "TFNMCouponTypeTmp", ref ptErrMsg);
                oDBSP.C_PRCbCreateTableTemp("TFNMCouponType_L", "TFNMCouponTypeTmp_L", ref ptErrMsg);

                oConn = new SqlConnection(tConnStr);
                oConn.Open();

                oTransaction = oConn.BeginTransaction();

                // Bulk Copy : TFNTCouponHD
                if (oTFNTCoupon.aoTFNTCouponHD != null)
                {
                    aoHD = new cDataReader<cmlTFNTCouponHD>(oTFNTCoupon.aoTFNTCouponHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNTCouponHDTmp";

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

                // Bulk Copy : TFNTCouponHD_L
                if (oTFNTCoupon.aoTFNTCouponHD_L != null)
                {
                    aoHD_L = new cDataReader<cmlTFNTCouponHD_L>(oTFNTCoupon.aoTFNTCouponHD_L);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHD_L.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNTCouponHDTmp_L";

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

                // Bulk Copy : TFNTCouponHDBch
                if (oTFNTCoupon.aoTFNTCouponHDBch != null)
                {
                    aoHDBch = new cDataReader<cmlTFNTCouponHDBch>(oTFNTCoupon.aoTFNTCouponHDBch);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHDBch.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNTCouponHDBchTmp";

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

                // Bulk Copy : TFNTCouponHDCstPri
                if (oTFNTCoupon.aoTFNTCouponHDCstPri != null)
                {
                    aoHDCstPri = new cDataReader<cmlTFNTCouponHDCstPri>(oTFNTCoupon.aoTFNTCouponHDCstPri);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHDCstPri.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNTCouponHDCstPriTmp";

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

                // Bulk Copy : TFNTCouponHDPdt
                if (oTFNTCoupon.aoTFNTCouponHDPdt != null)
                {
                    aoHDPdt = new cDataReader<cmlTFNTCouponHDPdt>(oTFNTCoupon.aoTFNTCouponHDPdt);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHDPdt.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNTCouponHDPdtTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoHDPdt);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                // Bulk Copy : TFNTCouponDT
                if (oTFNTCoupon.aoTFNTCouponDT != null)
                {
                    aoDT = new cDataReader<cmlTFNTCouponDT>(oTFNTCoupon.aoTFNTCouponDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNTCouponDTTmp";

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

                // Bulk Copy : TFNTCouponDTHis
                if (oTFNTCoupon.aoTFNTCouponDTHis != null)
                {
                    aoDTHis = new cDataReader<cmlTFNTCouponDTHis>(oTFNTCoupon.aoTFNTCouponDTHis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoDTHis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNTCouponDTHisTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoDTHis);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                // Bulk Copy : TFNMCouponType
                if (oTFNTCoupon.aoTFNMCouponType != null)
                {
                    aoType = new cDataReader<cmlTFNMCouponType>(oTFNTCoupon.aoTFNMCouponType);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoType.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNMCouponTypeTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoType);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                // Bulk Copy : TFNMCouponType_L
                if (oTFNTCoupon.aoTFNMCouponType_L != null)
                {
                    aoType_L = new cDataReader<cmlTFNMCouponType_L>(oTFNTCoupon.aoTFNMCouponType_L);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoType_L.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "TFNMCouponTypeTmp_L";

                        try
                        {
                            oBulkCopy.WriteToServer(aoType_L);
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
                // INSERT TFNTCouponHD
                oSql.AppendLine("   DELETE HD");
                oSql.AppendLine("   FROM TFNTCouponHD HD WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTCouponHDTmp HDTmp WITH(NOLOCK) ON ");
                oSql.AppendLine("       HD.FTBchCode = HDTmp.FTBchCode AND HD.FTCphDocNo = HDTmp.FTCphDocNo");
                oSql.AppendLine("   INSERT INTO TFNTCouponHD");
                oSql.AppendLine("   SELECT * FROM TFNTCouponHDTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TFNTCouponHD_L
                oSql.AppendLine("   DELETE HDL");
                oSql.AppendLine("   FROM TFNTCouponHD_L HDL WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTCouponHDTmp_L HDTmpL WITH(NOLOCK) ON ");
                oSql.AppendLine("       HDL.FNLngID = HDTmpL.FNLngID AND HDL.FTCphDocNo = HDTmpL.FTCphDocNo");
                oSql.AppendLine("   INSERT INTO TFNTCouponHD_L");
                oSql.AppendLine("   SELECT * FROM TFNTCouponHDTmp_L WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TFNTCouponHDBch
                oSql.AppendLine("   DELETE HDB");
                oSql.AppendLine("   FROM TFNTCouponHDBch HDB WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTCouponHDBchTmp HDBTmp WITH(NOLOCK) ON ");
                oSql.AppendLine("       HDB.FTBchCode = HDBTmp.FTBchCode AND HDB.FTCphBchTo = HDBTmp.FTCphBchTo AND");
                oSql.AppendLine("       HDB.FTCphDocNo = HDBTmp.FTCphDocNo AND HDB.FTCphMerTo = HDBTmp.FTCphMerTo AND");
                oSql.AppendLine("       HDB.FTCphShpTo = HDBTmp.FTCphShpTo ");
                oSql.AppendLine("   INSERT INTO TFNTCouponHDBch");
                oSql.AppendLine("   SELECT * FROM TFNTCouponHDBchTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TFNTCouponHDCstPri
                oSql.AppendLine("   DELETE HDCP");
                oSql.AppendLine("   FROM TFNTCouponHDCstPri HDCP WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTCouponHDCstPriTmp HDCPTmp WITH(NOLOCK) ON ");
                oSql.AppendLine("       HDCP.FTBchCode = HDCPTmp.FTBchCode AND HDCP.FTCphDocNo = HDCPTmp.FTCphDocNo AND");
                oSql.AppendLine("       HDCP.FTPplCode = HDCPTmp.FTPplCode ");
                oSql.AppendLine("   INSERT INTO TFNTCouponHDCstPri");
                oSql.AppendLine("   SELECT * FROM TFNTCouponHDCstPriTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TFNTCouponHDPdt
                oSql.AppendLine("   DELETE HDP");
                oSql.AppendLine("   FROM TFNTCouponHDPdt HDP WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTCouponHDPdtTmp HDPTmp WITH(NOLOCK) ON ");
                oSql.AppendLine("       HDP.FTBchCode = HDPTmp.FTBchCode AND HDP.FTCphDocNo = HDPTmp.FTCphDocNo AND");
                oSql.AppendLine("       HDP.FTPdtCode = HDPTmp.FTPdtCode AND HDP.FTPunCode = HDPTmp.FTPunCode");
                oSql.AppendLine("   INSERT INTO TFNTCouponHDPdt");
                oSql.AppendLine("   SELECT * FROM TFNTCouponHDPdtTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TFNTCouponDT
                oSql.AppendLine("   DELETE DT");
                oSql.AppendLine("   FROM TFNTCouponDT DT WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTCouponDTTmp DTTmp WITH(NOLOCK) ON ");
                oSql.AppendLine("       DT.FTBchCode = DTTmp.FTBchCode AND DT.FTCpdBarCpn = DTTmp.FTCpdBarCpn AND");
                oSql.AppendLine("       DT.FTCphDocNo = DTTmp.FTCphDocNo");
                oSql.AppendLine("   INSERT INTO TFNTCouponDT");
                oSql.AppendLine("   SELECT * FROM TFNTCouponDTTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TFNTCouponDTHis
                oSql.AppendLine("   DELETE DTH");
                oSql.AppendLine("   FROM TFNTCouponDTHis DTH WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTCouponDTHisTmp DTHTmp WITH(NOLOCK) ON ");
                oSql.AppendLine("       DTH.FNCpbSeqID = DTHTmp.FNCpbSeqID AND DTH.FTBchCode = DTHTmp.FTBchCode AND");
                oSql.AppendLine("       DTH.FTCpdBarCpn = DTHTmp.FTCpdBarCpn AND DTH.FTCphDocNo = DTHTmp.FTCphDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   SET IDENTITY_INSERT TFNTCouponDTHis ON");
                oSql.AppendLine("   INSERT INTO TFNTCouponDTHis");
                oSql.AppendLine("   (FTBchCode, FTCphDocNo, FTCpdBarCpn, ");
                oSql.AppendLine("    FNCpbSeqID, FDCpbFrmStart, FTCpbFrmBch, ");
                oSql.AppendLine("    FTCpbFrmPos, FTCpbFrmSalRef, FTCpbStaBook, ");
                oSql.AppendLine("    FDLastUpdOn, FTLastUpdBy, FDCreateOn, ");
                oSql.AppendLine("    FTCreateBy)");
                oSql.AppendLine("   SELECT FTBchCode, FTCphDocNo, FTCpdBarCpn, ");
                oSql.AppendLine("    FNCpbSeqID, FDCpbFrmStart, FTCpbFrmBch, ");
                oSql.AppendLine("    FTCpbFrmPos, FTCpbFrmSalRef, FTCpbStaBook, ");
                oSql.AppendLine("    FDLastUpdOn, FTLastUpdBy, FDCreateOn, ");
                oSql.AppendLine("    FTCreateBy FROM TFNTCouponDTHisTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TFNMCouponType
                oSql.AppendLine("   DELETE CPT");
                oSql.AppendLine("   FROM TFNMCouponType CPT WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNMCouponTypeTmp CPTTmp WITH(NOLOCK) ON ");
                oSql.AppendLine("       CPT.FTCptCode = CPTTmp.FTCptCode");
                oSql.AppendLine("   INSERT INTO TFNMCouponType");
                oSql.AppendLine("   SELECT * FROM TFNMCouponTypeTmp WITH(NOLOCK)");
                oSql.AppendLine();
                // INSERT TFNMCouponType_L
                oSql.AppendLine("   DELETE CPTL");
                oSql.AppendLine("   FROM TFNMCouponType_L CPTL WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNMCouponTypeTmp_L CPTTmpL WITH(NOLOCK) ON ");
                oSql.AppendLine("       CPTL.FNLngID = CPTTmpL.FNLngID AND CPTL.FTCptCode = CPTTmpL.FTCptCode");
                oSql.AppendLine("   INSERT INTO TFNMCouponType_L");
                oSql.AppendLine("   SELECT * FROM TFNMCouponTypeTmp_L WITH(NOLOCK)");
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
                //# End Process Import Data*/
                if (nRowAffect == 0) return false;
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cTnfCouponGiftVoucher2Bch", "C_PRCbDownloadCouponGiftVoucher : " + oEx.Message);
                return false;
            }
        }
    }
}
