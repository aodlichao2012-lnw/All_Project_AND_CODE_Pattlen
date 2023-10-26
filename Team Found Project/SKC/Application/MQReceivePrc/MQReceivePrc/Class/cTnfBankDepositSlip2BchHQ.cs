using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.BankDepositSlip;
using MQReceivePrc.Models.Bch2Bch;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
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
    public class cTnfBankDepositSlip2BchHQ
    {
        /// <summary>
        /// Process Bch Tranfer Bank Deposit Slip To HQ
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbTnfBankDepositSlip2BchHQ(cmlBchDownload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlDataBnkDpl oData;
            cmlTFNTBnkDpl oTFNTBnkDpl;
            StringBuilder oSql;
            cDatabase oDB;
            cSP oSP = new cSP();
            int nCmdTime = 60;
            string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);

            try
            {
                //*Arm 63-03-31 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                {

                    if (poData == null) return false;
                    if (string.IsNullOrEmpty(poData.ptData)) return false;
                    oData = JsonConvert.DeserializeObject<cmlDataBnkDpl>(poData.ptData);

                    oDB = new cDatabase();
                    oTFNTBnkDpl = new cmlTFNTBnkDpl();
                    oSql = new StringBuilder();

                    // TFNTBnkDplHD
                    oSql.AppendLine("SELECT FTBchCode, FTBdhDocNo, FTBdtCode, FDBdhDate, FTMerCode,");
                    oSql.AppendLine("FTShpCode, FTUsrCode, FTBdhUsrSender, FTBdhUsrApv, FTBbkCode,");
                    oSql.AppendLine("FTBdhRefExt, FDBdhRefExtDate, FCBdhTotCash, FCBdhTotCheque, FCBdhTotChqChg,");
                    oSql.AppendLine("FCBdhTotChqVat, FCBdhTotal, FTBdhRmk, FTBdhStaDoc, FTBdhStaApv,");
                    oSql.AppendLine("FNBdhStaDocAct, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                    oSql.AppendLine("FROM TFNTBnkDplHD WITH(NOLOCK) ");
                    oSql.AppendLine("WHERE FTBchCode = '" + oData.ptFTBchCode + "' AND FTBdhDocNo='" + oData.ptFTBdhDocNo + "'");
                    oTFNTBnkDpl.aoTFNTBnkDplHD = oDB.C_GETaDataQuery<cmlTFNTBnkDplHD>(t_ConnStr, oSql.ToString(), nCmdTime);

                    if (oTFNTBnkDpl.aoTFNTBnkDplHD.Count > 0)
                    {
                        // TFNTBnkDplDT
                        oSql.Clear();
                        oSql.AppendLine("SELECT TFNTBnkDplDT.FTBchCode, TFNTBnkDplDT.FTBdhDocNo, TFNTBnkDplDT.FNBddSeq, TFNTBnkDplDT.FTBddType, ");
                        oSql.AppendLine("TFNTBnkDplDT.FTBddRefNo, TFNTBnkDplDT.FDBddRefDate, TFNTBnkDplDT.FCBddRefAmt,");
                        oSql.AppendLine("TFNTBnkDplDT.FDLastUpdOn, TFNTBnkDplDT.FTLastUpdBy, ");
                        oSql.AppendLine("TFNTBnkDplDT.FDCreateOn, TFNTBnkDplDT.FTCreateBy ");
                        oSql.AppendLine("FROM TFNTBnkDplDT  WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN  TFNTBnkDplHD WITH(NOLOCK) ON TFNTBnkDplDT.FTBchCode =  TFNTBnkDplHD.FTBchCode AND TFNTBnkDplDT.FTBdhDocNo = TFNTBnkDplHD.FTBdhDocNo ");
                        oSql.AppendLine("WHERE TFNTBnkDplHD.FTBchCode = '" + oData.ptFTBchCode + "' AND TFNTBnkDplHD.FTBdhDocNo='" + oData.ptFTBdhDocNo + "'");
                        oTFNTBnkDpl.aoTFNTBnkDplDT = oDB.C_GETaDataQuery<cmlTFNTBnkDplDT>(t_ConnStr, oSql.ToString(), nCmdTime);

                        // TFNMBnkDepType
                        oSql.Clear();
                        oSql.AppendLine("SELECT TFNMBnkDepType.FTBdtCode, ");
                        oSql.AppendLine("TFNMBnkDepType.FDLastUpdOn, TFNMBnkDepType.FTLastUpdBy, ");
                        oSql.AppendLine("TFNMBnkDepType.FDCreateOn, TFNMBnkDepType.FTCreateBy ");
                        oSql.AppendLine("FROM TFNMBnkDepType  WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN  TFNTBnkDplHD WITH(NOLOCK) ON TFNMBnkDepType.FTBdtCode = TFNTBnkDplHD.FTBdtCode");
                        oSql.AppendLine("WHERE TFNTBnkDplHD.FTBchCode = '" + oData.ptFTBchCode + "' AND TFNTBnkDplHD.FTBdhDocNo='" + oData.ptFTBdhDocNo + "'");
                        oTFNTBnkDpl.aoTFNMBnkDepType = oDB.C_GETaDataQuery<cmlTFNMBnkDepType>(t_ConnStr, oSql.ToString(), nCmdTime);

                        // TFNMBookBank
                        oSql.Clear();
                        oSql.AppendLine("SELECT TFNMBookBank.FTBchCode, TFNMBookBank.FTBbkCode, TFNMBookBank.FTMerCode,");
                        oSql.AppendLine("TFNMBookBank.FTBbkType, TFNMBookBank.FTBbkAccNo, TFNMBookBank.FTBnkCode, ");
                        oSql.AppendLine("TFNMBookBank.FDBbkOpen, TFNMBookBank.FCBbkBalance, TFNMBookBank.FDBbkUpd, ");
                        oSql.AppendLine("TFNMBookBank.FTBbkStaActive, ");
                        oSql.AppendLine("TFNMBookBank.FDLastUpdOn, TFNMBookBank.FTLastUpdBy,");
                        oSql.AppendLine("TFNMBookBank.FDCreateOn, TFNMBookBank.FTCreateBy ");
                        oSql.AppendLine("FROM TFNMBookBank WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN  TFNTBnkDplHD WITH(NOLOCK) ON TFNMBookBank.FTBbkCode = TFNTBnkDplHD.FTBbkCode AND TFNMBookBank.FTBchCode = TFNTBnkDplHD.FTBchCode");
                        oSql.AppendLine("WHERE TFNTBnkDplHD.FTBchCode = '" + oData.ptFTBchCode + "' AND TFNTBnkDplHD.FTBdhDocNo='" + oData.ptFTBdhDocNo + "'");
                        oTFNTBnkDpl.aoTFNMBookBank = oDB.C_GETaDataQuery<cmlTFNMBookBank>(t_ConnStr, oSql.ToString(), nCmdTime);

                        // TFNMBank
                        oSql.Clear();
                        oSql.AppendLine("SELECT TFNMBank.FTBnkCode, ");
                        oSql.AppendLine("TFNMBank.FDLastUpdOn, TFNMBank.FTLastUpdBy, ");
                        oSql.AppendLine("TFNMBank.FDCreateOn, TFNMBank.FTCreateBy ");
                        oSql.AppendLine("FROM TFNMBank WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TFNMBookBank WITH(NOLOCK) ON TFNMBank.FTBnkCode = TFNMBookBank.FTBnkCode ");
                        oSql.AppendLine("INNER JOIN TFNTBnkDplHD WITH(NOLOCK) ON TFNMBookBank.FTBbkCode = TFNTBnkDplHD.FTBbkCode AND TFNMBookBank.FTBchCode = TFNTBnkDplHD.FTBchCode ");
                        oSql.AppendLine("WHERE TFNTBnkDplHD.FTBchCode = '" + oData.ptFTBchCode + "' AND TFNTBnkDplHD.FTBdhDocNo='" + oData.ptFTBdhDocNo + "'");
                        oTFNTBnkDpl.aoTFNMBank = oDB.C_GETaDataQuery<cmlTFNMBank>(t_ConnStr, oSql.ToString(), nCmdTime);

                        // TFNMBookCheque
                        oSql.Clear();
                        oSql.AppendLine("SELECT TFNMBookCheque.FTBchCode, TFNMBookCheque.FTChqCode, TFNMBookCheque.FTBbkCode, TFNMBookCheque.FNChqMin,");
                        oSql.AppendLine("TFNMBookCheque.FNChqMax, TFNMBookCheque.FTChqStaAct, TFNMBookCheque.FTChqStaPrcDoc,");
                        oSql.AppendLine("TFNMBookCheque.FDLastUpdOn, TFNMBookCheque.FTLastUpdBy,");
                        oSql.AppendLine("TFNMBookCheque.FDCreateOn, TFNMBookCheque.FTCreateBy ");
                        oSql.AppendLine("FROM TFNMBookCheque WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TFNTBnkDplDT WITH(NOLOCK) ON TFNMBookCheque.FTChqCode = TFNTBnkDplDT.FTBddRefNo AND TFNMBookCheque.FTBchCode = TFNTBnkDplDT.FTBchCode ");
                        oSql.AppendLine("INNER JOIN  TFNTBnkDplHD WITH(NOLOCK) ON TFNTBnkDplDT.FTBchCode = TFNTBnkDplHD.FTBchCode AND TFNTBnkDplDT.FTBdhDocNo = TFNTBnkDplHD.FTBdhDocNo ");
                        oSql.AppendLine("WHERE TFNTBnkDplHD.FTBchCode = '" + oData.ptFTBchCode + "' AND TFNTBnkDplHD.FTBdhDocNo='" + oData.ptFTBdhDocNo + "'");
                        oTFNTBnkDpl.aoTFNMBookCheque = oDB.C_GETaDataQuery<cmlTFNMBookCheque>(t_ConnStr, oSql.ToString(), nCmdTime);

                        // TFNTBnkStatement
                        oSql.Clear();
                        oSql.AppendLine("SELECT TFNTBnkStatement.FTBchCode, TFNTBnkStatement.FTBbkCode, TFNTBnkStatement.FTBktType, TFNTBnkStatement.FDBktDate,");
                        oSql.AppendLine("TFNTBnkStatement.FTBktAccFrom,	TFNTBnkStatement.FTBktAccTo, TFNTBnkStatement.FTBktRefChq, TFNTBnkStatement.FCBktAmt,");
                        oSql.AppendLine("TFNTBnkStatement.FCBktFree, TFNTBnkStatement.FTBktRmk, TFNTBnkStatement.FTBktStaPrcDoc, TFNTBnkStatement.FTBktApvCode,");
                        oSql.AppendLine("TFNTBnkStatement.FDLastUpdOn, TFNTBnkStatement.FTLastUpdBy,");
                        oSql.AppendLine("TFNTBnkStatement.FDCreateOn, TFNTBnkStatement.FTCreateBy ");
                        oSql.AppendLine("FROM TFNTBnkStatement WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TFNTBnkDplHD WITH(NOLOCK) ON TFNTBnkStatement.FTBchCode = TFNTBnkDplHD.FTBchCode AND TFNTBnkStatement.FTBbkCode = TFNTBnkDplHD.FTBbkCode");
                        oSql.AppendLine("WHERE TFNTBnkDplHD.FTBchCode = '" + oData.ptFTBchCode + "' AND TFNTBnkDplHD.FTBdhDocNo='" + oData.ptFTBdhDocNo + "'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TFNTBnkStatement.FTLastUpdBy,121) >= CONVERT(VARCHAR(10),TFNTBnkStatement.FTLastUpdBy,121)");
                        oTFNTBnkDpl.aoTFNTBnkStatement = oDB.C_GETaDataQuery<cmlTFNTBnkStatement>(t_ConnStr, oSql.ToString(), nCmdTime);

                    }

                    //GET fromat Message Json string to API2PSSale
                    cmlBchDownload oUpload = new cmlBchDownload();
                    oUpload.ptFunction = "BankDepositSlip";
                    oUpload.ptSource = cVB.tVB_BchCode;
                    oUpload.ptDest = "HQ";
                    oUpload.ptFilter = cVB.tVB_BchHQ;
                    oUpload.ptData = JsonConvert.SerializeObject(oTFNTBnkDpl);
                    //oUpload.ptConnStr = "Data Source=.\\SQL2016;Initial Catalog=PTT_BackOffice00003;User ID=sa;Password=Ada2000;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=30;Max Pool Size=100;Pooling=true;";
                    //string tJSonCall = JsonConvert.SerializeObject(oUpload);
                    //Send API2PSSale
                    if (oSP.SP_CHKbIsHQBch(t_ConnStr, (int)poShopDB.nCommandTimeOut) == false)
                    {
                        string tAPIUrl = "";
                        string tUrlFunc = "/Service/Upload/Finance";
                        string tAPIHeader = "";
                        string tXKey = "";
                        string tBchHQ = "";
                        tBchHQ = oSP.SP_GETtBchHQ(t_ConnStr, (int)poShopDB.nCommandTimeOut);
                        tAPIUrl = oSP.SP_GETtUrlAPI(t_ConnStr, (int)poShopDB.nCommandTimeOut, tBchHQ, 5, ref tXKey, ref tAPIHeader);

                        if (!string.IsNullOrEmpty(tAPIUrl))
                        {
                            string tJSonCall = JsonConvert.SerializeObject(oUpload);
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(tAPIHeader, tXKey);
                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tAPIUrl + tUrlFunc, tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbTnfBankDepositSlip2BchHQ : " + oEx.Message);
                            }

                            if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                                cmlResResult oRes = JsonConvert.DeserializeObject<cmlResResult>(tJSonRes);
                                if (oRes.rtCode == "001")
                                {

                                }
                                else
                                {
                                    new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbTnfBankDepositSlip2BchHQ/ToHQ : " + oRes.rtMsg);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbTnfBankDepositSlip2BchHQ : " + oEx.Message);
                return false;
            }
        }

        /// <summary>
        /// Process HQ Insert Bank Deposit Slip
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbDownloadBankDepositSlip2BchHQ(cmlBchDownload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlTFNTBnkDpl oTFNTBnkDpl;
            StringBuilder oSql;
            cDatabase oDB;
            SqlConnection oConn;
            SqlTransaction oTransaction;
            cSP oSP = new cSP();
            cDataReader<cmlTFNTBnkDplHD> aoBnkDplHD;
            cDataReader<cmlTFNTBnkDplDT> aoBnkDplDT;
            cDataReader<cmlTFNMBnkDepType> aoBnkDepType;
            cDataReader<cmlTFNMBookBank> aoBookBank;
            cDataReader<cmlTFNMBank> aoBank;
            cDataReader<cmlTFNMBookCheque> aoBookCheque;
            cDataReader<cmlTFNTBnkStatement> aoBnkStatement;
            int nRowAffect = 0;

            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oTFNTBnkDpl = JsonConvert.DeserializeObject<cmlTFNTBnkDpl>(poData.ptData);

                oDB = new cDatabase();
                oSql = new StringBuilder();
                //TFNTBnkDplHDTmp
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TFNTBnkDplHDTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       SELECT TOP 0 * INTO TFNTBnkDplHDTmp FROM TFNTBnkDplHD with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtPrice4CSTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNTBnkDplHD' ),0) ");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("           DROP TABLE TFNTBnkDplHDTmp");
                oSql.AppendLine("           SELECT TOP 0 * INTO TFNTBnkDplHDTmp FROM TFNTBnkDplHD with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TFNTBnkDplHDTmp");
                oSql.AppendLine("");
                //TFNTBnkDplDTTmp
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TFNTBnkDplDTTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       SELECT TOP 0 * INTO TFNTBnkDplDTTmp FROM TFNTBnkDplDT with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNTBnkDplDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNTBnkDplDT' ),0) ");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("           DROP TABLE TFNTBnkDplDTTmp");
                oSql.AppendLine("           SELECT TOP 0 * INTO TFNTBnkDplDTTmp FROM TFNTBnkDplDT with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TFNTBnkDplDTTmp");
                oSql.AppendLine("");
                //TFNMBnkDepTypeTmp
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TFNMBnkDepTypeTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       SELECT TOP 0 * INTO TFNMBnkDepTypeTmp FROM TFNMBnkDepType with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNMBnkDepTypeTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNMBnkDepType' ),0) ");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("           DROP TABLE TFNMBnkDepTypeTmp");
                oSql.AppendLine("           SELECT TOP 0 * INTO TFNMBnkDepTypeTmp FROM TFNMBnkDepType with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TFNMBnkDepTypeTmp");
                oSql.AppendLine("");
                //TFNMBookBankTmp
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TFNMBookBankTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       SELECT TOP 0 * INTO TFNMBookBankTmp FROM TFNMBookBank with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNMBookBankTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNMBookBank' ),0) ");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("           DROP TABLE TFNMBookBankTmp");
                oSql.AppendLine("           SELECT TOP 0 * INTO TFNMBookBankTmp FROM TFNMBookBank with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TFNMBookBankTmp");
                oSql.AppendLine("");
                //TFNMBankTmp
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TFNMBankTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       SELECT TOP 0 * INTO TFNMBankTmp FROM TFNMBank with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNMBankTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNMBank' ),0) ");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("           DROP TABLE TFNMBankTmp");
                oSql.AppendLine("           SELECT TOP 0 * INTO TFNMBankTmp FROM TFNMBank with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TFNMBankTmp");
                oSql.AppendLine("");
                //TFNMBookChequeTmp
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TFNMBookChequeTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       SELECT TOP 0 * INTO TFNMBookChequeTmp FROM TFNMBookCheque with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNMBookChequeTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNMBookCheque' ),0) ");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("           DROP TABLE TFNMBookChequeTmp");
                oSql.AppendLine("           SELECT TOP 0 * INTO TFNMBookChequeTmp FROM TFNMBookCheque with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TFNMBookChequeTmp");
                oSql.AppendLine("");
                //TFNTBnkStatementTmp
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TFNTBnkStatementTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       SELECT TOP 0 * INTO TFNTBnkStatementTmp FROM TFNTBnkStatement with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNTBnkStatementTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TFNTBnkStatement' ),0) ");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("           DROP TABLE TFNTBnkStatementTmp");
                oSql.AppendLine("           SELECT TOP 0 * INTO TFNTBnkStatementTmp FROM TFNTBnkStatement with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TFNTBnkStatementTmp");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);


                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();
                oTransaction = oConn.BeginTransaction();

                // Bulk Copy :TFNTBnkDplHD
                if (oTFNTBnkDpl.aoTFNTBnkDplHD != null)
                {
                    aoBnkDplHD = new cDataReader<cmlTFNTBnkDplHD>(oTFNTBnkDpl.aoTFNTBnkDplHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoBnkDplHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TFNTBnkDplHDTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoBnkDplHD);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbDownloadBankDepositSlip2BchHQ/TFNTBnkDplHDTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }


                // Bulk Copy :TFNTBnkDplDT
                if (oTFNTBnkDpl.aoTFNTBnkDplDT != null)
                {
                    aoBnkDplDT = new cDataReader<cmlTFNTBnkDplDT>(oTFNTBnkDpl.aoTFNTBnkDplDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoBnkDplDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TFNTBnkDplDTTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoBnkDplDT);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbDownloadBankDepositSlip2BchHQ/TFNTBnkDplDTTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                // Bulk Copy :TFNMBnkDepType
                if (oTFNTBnkDpl.aoTFNMBnkDepType != null)
                {
                    aoBnkDepType = new cDataReader<cmlTFNMBnkDepType>(oTFNTBnkDpl.aoTFNMBnkDepType);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoBnkDepType.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TFNMBnkDepTypeTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoBnkDepType);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbDownloadBankDepositSlip2BchHQ/TFNMBnkDepTypeTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }


                // Bulk Copy :TFNMBookBank
                if (oTFNTBnkDpl.aoTFNMBookBank != null)
                {
                    aoBookBank = new cDataReader<cmlTFNMBookBank>(oTFNTBnkDpl.aoTFNMBookBank);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoBookBank.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TFNMBookBankTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoBookBank);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbDownloadBankDepositSlip2BchHQ/TFNMBookBankTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                // Bulk Copy :TFNMBank
                if (oTFNTBnkDpl.aoTFNMBank != null)
                {
                    aoBank = new cDataReader<cmlTFNMBank>(oTFNTBnkDpl.aoTFNMBank);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoBank.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TFNMBankTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoBank);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbDownloadBankDepositSlip2BchHQ/TFNMBankTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                // Bulk Copy :TFNMBookCheque
                if (oTFNTBnkDpl.aoTFNMBookCheque != null)
                {
                    aoBookCheque = new cDataReader<cmlTFNMBookCheque>(oTFNTBnkDpl.aoTFNMBookCheque);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoBookCheque.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TFNMBookChequeTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoBookCheque);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbDownloadBankDepositSlip2BchHQ/TFNMBookChequeTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                // Bulk Copy :TFNTBnkStatement 
                if (oTFNTBnkDpl.aoTFNTBnkStatement != null)
                {
                    aoBnkStatement = new cDataReader<cmlTFNTBnkStatement>(oTFNTBnkDpl.aoTFNTBnkStatement);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoBnkStatement.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TFNTBnkStatementTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoBnkStatement);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            new cLog().C_WRTxLog("cTnfBankDepositSlip2BchHQ", "C_PRCbDownloadBankDepositSlip2BchHQ/TFNTBnkStatementTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                oTransaction.Commit();

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE BDHD ");
                oSql.AppendLine("   FROM TFNTBnkDplHD BDHD WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTBnkDplHDTmp BDHDTmp WITH(NOLOCK) ON BDHD.FTBchCode = BDHDTmp.FTBchCode AND BDHD.FTBdhDocNo = BDHDTmp.FTBdhDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TFNTBnkDplHD");
                oSql.AppendLine("   SELECT * FROM TFNTBnkDplHDTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE BDDT ");
                oSql.AppendLine("   FROM TFNTBnkDplDT BDDT WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTBnkDplDTTmp BDDTTmp WITH(NOLOCK) ON BDDT.FTBchCode = BDDTTmp.FTBchCode AND BDDT.FTBdhDocNo = BDDTTmp.FTBdhDocNo AND BDDT.FNBddSeq = BDDTTmp.FNBddSeq");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TFNTBnkDplDT");
                oSql.AppendLine("   SELECT * FROM TFNTBnkDplDTTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE BDT ");
                oSql.AppendLine("   FROM TFNMBnkDepType BDT WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNMBnkDepTypeTmp BDTTmp WITH(NOLOCK) ON BDT.FTBdtCode = BDTTmp.FTBdtCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TFNMBnkDepType");
                oSql.AppendLine("   SELECT * FROM TFNMBnkDepTypeTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE BB ");
                oSql.AppendLine("   FROM TFNMBookBank BB WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNMBookBankTmp BBTmp WITH(NOLOCK) ON BB.FTBchCode = BBTmp.FTBchCode AND BB.FTBbkCode = BBTmp.FTBbkCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TFNMBookBank");
                oSql.AppendLine("   SELECT * FROM TFNMBookBankTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE BNK ");
                oSql.AppendLine("   FROM TFNMBank BNK WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNMBankTmp BNKTmp WITH(NOLOCK) ON BNK.FTBnkCode = BNKTmp.FTBnkCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TFNMBank");
                oSql.AppendLine("   SELECT * FROM TFNMBankTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE BC ");
                oSql.AppendLine("   FROM TFNMBookCheque BC WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNMBookChequeTmp BCTmp WITH(NOLOCK) ON BC.FTBchCode = BCTmp.FTBchCode AND BC.FTChqCode = BCTmp.FTChqCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TFNMBookCheque");
                oSql.AppendLine("   SELECT * FROM TFNMBookChequeTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE BSM ");
                oSql.AppendLine("   FROM TFNTBnkStatement BSM WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TFNTBnkStatementTmp BSMTmp WITH(NOLOCK) ON BSM.FTBchCode = BSMTmp.FTBchCode AND BSM.FTBbkCode = BSMTmp.FTBbkCode");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TFNTBnkStatement");
                oSql.AppendLine("   SELECT * FROM TFNTBnkStatementTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbDownloadBankDepositSlip2BchHQ");
                return false;
            }
            finally
            {
                aoBnkDplHD = null;
                aoBnkDplDT = null;
                aoBnkDepType = null;
                aoBookBank = null;
                aoBank = null;
                aoBookCheque = null;
                aoBnkStatement = null;
                oSql = null;
                oTFNTBnkDpl = null;
                oTransaction = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }
        }
    }
}
