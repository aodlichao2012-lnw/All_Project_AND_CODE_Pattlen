using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.SaleOrder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using MQReceivePrc.Models.Doc;
using RabbitMQ.Client;

namespace MQReceivePrc.Class
{
    public class cGenSO
    {
        private string tBchCode;
        private string tShpCode;
        private string tDocNo;
        private string tDocType;
        private int nSeqCode;
        private int nMaxDocSeq;
        private string tFDXshDocDate;
        private string t_ConnStr;

        private string tPubEx = "AR_XSaleOrder";
        private string tPubQ = "AR_QNotiMsg";

        cDatabaseSP oDBSP;
        cmlShopDB oShopDB;

        public bool C_PRCbGenSO(cmlRcvData poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            List<cmlReqTARTSo> aoTARTSo;
            oShopDB = poShopDB;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                CultureInfo oCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                oCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
                oCulture.DateTimeFormat.LongTimePattern = "";
                Thread.CurrentThread.CurrentCulture = oCulture;

                t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                tBchCode = cVB.tVB_BchCode;

                oDBSP = new cDatabaseSP(t_ConnStr, poShopDB);

                aoTARTSo = JsonConvert.DeserializeObject<List<cmlReqTARTSo>>(poData.ptData);
                if (aoTARTSo != null)
                {
                    //...Start Save to DB
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [AR_QGenTARTSO] SO Insert to Tmp Start...");
                    //...Create Tmp
                    if (!(oDBSP.C_PRCbCreateTableTemp("TARTSoHD", "TARTSoHDTmp", ref ptErrMsg) &&
                          oDBSP.C_PRCbCreateTableTemp("TARTSoHDCst", "TARTSoHDCstTmp", ref ptErrMsg) &&
                          oDBSP.C_PRCbCreateTableTemp("TARTSoDT", "TARTSoDTTmp", ref ptErrMsg) &&
                          oDBSP.C_PRCbCreateTableTemp("TARTDocApvTxn", "TARTDocApvTxnTmp", ref ptErrMsg)))
                    {
                        ptErrMsg = "Create Tmp Fail";
                        throw new Exception();
                    }

                    //...INSERT to Tmp
                    if (!(C_PRCbDBInsertSOHDTmp(aoTARTSo[0]) &&
                        C_PRCbDBInsertSOHDCstTmp(aoTARTSo[0])))
                    {

                        ptErrMsg = "Insert HD Fail";
                        throw new Exception();

                    }
                    int nSeq = 1;
                    foreach (var oTARTSoDT in aoTARTSo)
                    {
                        if (C_PRCbDBInsertSODTTmp(oTARTSoDT, nSeq))
                        {
                            nSeq++;
                        }
                        else
                        {
                            ptErrMsg = "Insert DT Fail";
                            throw new Exception();
                        }
                    }


                    nMaxDocSeq = C_GETnMaxDocSeq();
                    if (nMaxDocSeq <= 0)
                    {
                        ptErrMsg = "Get DocApv Fail";
                        throw new Exception();
                    }
                    for (int nApv = 1; nApv <= nMaxDocSeq; nApv++)
                    {
                        if (!C_PRCbDBInsertDocApvTxnTmp(aoTARTSo[0], nApv))
                        {
                            ptErrMsg = "Insert DocApv Fail";
                            throw new Exception();
                        }
                    }

                    //...Insert Tmp to DB
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [AR_QGenTARTSO] Insert to Database Start...");
                    if (!(C_PRCbCopyTmptoDB("TARTSoHD", "TARTSoHDTmp", ref ptErrMsg) &&
                        C_PRCbCopyTmptoDB("TARTSoHDCst", "TARTSoHDCstTmp", ref ptErrMsg) &&
                        C_PRCbCopyTmptoDB("TARTSoDT", "TARTSoDTTmp", ref ptErrMsg) &&
                        C_PRCbCopyTmptoDB("TARTDocApvTxn", "TARTDocApvTxnTmp", ref ptErrMsg) &&
                        C_PRCbDBUpdateAutoHis("TARTSoHD", "FTXshDocNo")))
                    {
                        ptErrMsg = "Copy Tmp Fail";
                        C_PRCbDBDeleteDocNo();
                        throw new Exception();
                    }

                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [AR_QGenTARTSO] Publish to MQ Start...");
                    //...Publish to Q
                    cmlNotiTARTDocApvTxn oResDT = new cmlNotiTARTDocApvTxn();
                    oResDT.tFTBchCode = cVB.tVB_BchCode;
                    oResDT.tFTXshDocNo = tDocNo;
                    oResDT.tFDXshDocDate = tFDXshDocDate;
                    oResDT.tFTHNCode = aoTARTSo[0].tFTCstCode.ToString();
                    oResDT.tFTDptName = aoTARTSo[0].tFTDptName;
                    oResDT.tFTXshApvCode = "HIS";
                    oResDT.tFTStaDoc = "2";
                    oResDT.tFTStaAct = "1";

                    cmlRcvData oResHD = new cmlRcvData();
                    oResHD.ptFunction = "TARTSoHD";
                    oResHD.ptSource = "MQReceivePrc";
                    oResHD.ptDest = "Vending";
                    oResHD.ptFilter = poData.ptFilter;
                    oResHD.ptData = JsonConvert.SerializeObject(oResDT);

                    string tMsgNotiJson = JsonConvert.SerializeObject(oResHD);

                    if (C_PRCbMQDeclareExchange(tPubEx + cVB.tVB_BchCode, ExMode.fanout) &&
                        C_PRCbMQDeclareQueue($"AR_QNotiMsgPrc{tBchCode}") &&
                        C_PRCbMQDeclareQueue($"AR_QNotiMsg{tBchCode}") &&
                        C_PRCbMQDeclareQueue($"AR_QNotiMsg{tBchCode}{aoTARTSo[0].tFTPosCode}") &&
                        C_PRCbMQBindRouting(tPubEx + cVB.tVB_BchCode, $"AR_QNotiMsgPrc{tBchCode}", "AR_QNotiMsg") &&
                        C_PRCbMQBindRouting(tPubEx + cVB.tVB_BchCode, $"AR_QNotiMsg{tBchCode}", "AR_QNotiMsg") &&
                        C_PRCbMQBindRouting(tPubEx + cVB.tVB_BchCode, $"AR_QNotiMsg{tBchCode}{aoTARTSo[0].tFTPosCode}", "AR_QNotiMsg") &&
                        C_PRCbMQPublish2Exchange(tPubEx + cVB.tVB_BchCode, tPubEx + cVB.tVB_BchCode, tMsgNotiJson))
                    {
                        return true;
                    }
                    else
                    {
                        ptErrMsg = "Publish Fail";
                        throw new Exception();
                    }

                }

                return true;
            }
            catch (Exception oEx)
            {
                if (ptErrMsg == "") ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUplSO");
                new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }

        private bool C_PRCbDBInsertSOHDTmp(cmlReqTARTSo poTARTSo)
        {
            List<cmlTARTSoHD> aoSOHD;
            cmlTARTSoHD oNewSOHD;
            try
            {
                tShpCode = C_GETtShopCode(poTARTSo.tFTPosCode);
                if (!String.IsNullOrEmpty(tShpCode))
                {
                    tDocType = C_GETtDocType("TARTSoHD", "FTXshDocNo");
                    int nDocType;
                    if (!String.IsNullOrEmpty(tDocType) && int.TryParse(tDocType, out nDocType))
                    {
                        tDocNo = C_GETtDocNo(poTARTSo.tFTPosCode, tShpCode, nDocType);
                        if (!String.IsNullOrEmpty(tDocNo))
                        {
                            aoSOHD = new List<cmlTARTSoHD>();
                            string tWahCode = C_GETtWahCode(tShpCode);
                            string tDocDate = $"{poTARTSo.tFDXshDocDate.Split('/')[2]}-" +
                                              $"{poTARTSo.tFDXshDocDate.Split('/')[1]}-" +
                                              $"{poTARTSo.tFDXshDocDate.Split('/')[0]}";
                            tFDXshDocDate = $" {tDocDate} {poTARTSo.tFDXshTime}";

                            oNewSOHD = new cmlTARTSoHD();
                            oNewSOHD.FTBchCode = tBchCode;
                            oNewSOHD.FTXshDocNo = tDocNo;
                            oNewSOHD.FTShpCode = tShpCode;
                            oNewSOHD.FNXshDocType = int.Parse(tDocType);
                            oNewSOHD.FDXshDocDate = DateTime.Now;
                            oNewSOHD.FTXshCshOrCrd = "1";
                            oNewSOHD.FTXshVATInOrEx = "1";
                            oNewSOHD.FTDptCode = poTARTSo.tFTDptCode;
                            oNewSOHD.FTWahCode = tWahCode;
                            oNewSOHD.FTPosCode = poTARTSo.tFTPosCode;
                            oNewSOHD.FNSdtSeqNo = 0;
                            oNewSOHD.FTCstCode = poTARTSo.tFTCstCode;
                            oNewSOHD.FTXshRefExt = poTARTSo.tFTXshRefExt;
                            oNewSOHD.FDXshRefExtDate = DateTime.Parse(tFDXshDocDate);
                            oNewSOHD.FNXshDocPrint = 0;
                            oNewSOHD.FCXshRteFac = 0;
                            oNewSOHD.FCXshTotal = 0;
                            oNewSOHD.FCXshTotalNV = 0;
                            oNewSOHD.FCXshTotalNoDis = 0;
                            oNewSOHD.FCXshTotalB4DisChgV = 0;
                            oNewSOHD.FCXshTotalB4DisChgNV = 0;
                            oNewSOHD.FCXshDis = 0;
                            oNewSOHD.FCXshChg = 0;
                            oNewSOHD.FCXshTotalAfDisChgV = 0;
                            oNewSOHD.FCXshTotalAfDisChgNV = 0;
                            oNewSOHD.FCXshRefAEAmt = 0;
                            oNewSOHD.FCXshAmtV = 0;
                            oNewSOHD.FCXshAmtNV = 0;
                            oNewSOHD.FCXshVat = 0;
                            oNewSOHD.FCXshVatable = 0;
                            oNewSOHD.FCXshWpTax = 0;
                            oNewSOHD.FCXshGrand = 0;
                            oNewSOHD.FCXshRnd = 0;
                            oNewSOHD.FCXshPaid = 0;
                            oNewSOHD.FCXshLeft = 0;
                            oNewSOHD.FTXshStaRefund = "1";
                            oNewSOHD.FTXshStaDoc = "1";
                            oNewSOHD.FTXshStaApv = "1";
                            oNewSOHD.FTXshStaPrcDoc = "1";
                            oNewSOHD.FTXshStaPaid = "1";
                            oNewSOHD.FNXshStaDocAct = 1;
                            oNewSOHD.FNXshStaRef = 0;
                            oNewSOHD.FDLastUpdOn = DateTime.Now;
                            oNewSOHD.FTLastUpdBy = poTARTSo.tFTDptCode;
                            oNewSOHD.FDCreateOn = DateTime.Now;
                            oNewSOHD.FTCreateBy = poTARTSo.tFTDptCode;

                            aoSOHD.Add(oNewSOHD);
                            string tErrMsg = "";
                            oDBSP.C_PRCbDBInsertbyList<cmlTARTSoHD>(aoSOHD, "TARTSOHDTmp", ref tErrMsg);
                            if (String.IsNullOrEmpty(tErrMsg)) return true;
                            else return false;
                        }
                        else new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + "Error AutoDocNo");
                    }
                    else new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + "Error DocType");
                }
                else new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + "Error ShopCode");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                aoSOHD = null;
                oNewSOHD = null;
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        private bool C_PRCbDBInsertSOHDCstTmp(cmlReqTARTSo poTARTSo)
        {
            List<cmlTARTSoHDCst> aoSOHDCst;
            cmlTARTSoHDCst oNewSOHDCst;
            try
            {
                if (!String.IsNullOrEmpty(tDocNo))
                {
                    aoSOHDCst = new List<cmlTARTSoHDCst>();
                    oNewSOHDCst = new cmlTARTSoHDCst();
                    oNewSOHDCst.FTBchCode = tBchCode;
                    oNewSOHDCst.FTXshDocNo = tDocNo;
                    oNewSOHDCst.FTXshCardID = "";
                    oNewSOHDCst.FTXshCardNo = poTARTSo.tFTCstCode;
                    oNewSOHDCst.FNXshCrTerm = 0;
                    oNewSOHDCst.FTXshCtrName = poTARTSo.tFTCstName;

                    aoSOHDCst.Add(oNewSOHDCst);
                    string tErrMsg = "";
                    oDBSP.C_PRCbDBInsertbyList<cmlTARTSoHDCst>(aoSOHDCst, "TARTSOHDCstTmp", ref tErrMsg);
                    if (String.IsNullOrEmpty(tErrMsg)) return true;
                    else return false;
                }
                else new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + "Error AutoDocNo");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return false;
        }
        private bool C_PRCbDBInsertSODTTmp(cmlReqTARTSo poTARTSo, int pnSeq)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                int nRowAffect;

                string tPdtName = $"(SELECT FTPdtName FROM TCNMPDT_L WHERE FTPdtCode='{poTARTSo.tFTPdtCode.ToString()}' and FnLngID=1)";
                string tPdtPunCode = $"(SELECT FTPunCode FROM [TCNMPdtPackSize] WHERE FTPdtCode = '{poTARTSo.tFTPdtCode.ToString()}')";
                string tPdtPunName = $"(SELECT FTPunName FROM TCNMPdtUnit_L WHERE FTPunCode = {tPdtPunCode} AND FNLngID = 1)";
                string tFTPdtStaVat = $"(SELECT FTPdtStaVat FROM TCNMPdt where FTPdtCode='{poTARTSo.tFTPdtCode.ToString()}')";
                string tFTVatCode = $"(SELECT FTVatCode FROM TCNMPdt WHERE FTPdtCode='{poTARTSo.tFTPdtCode.ToString()}')";
                string tcFCVatRate = $"(SELECT TOP(1) FCVatRate FROM TCNMPdt PDT " +
                                     $"INNER JOIN TCNMVatRate VAT ON PDT.FTVatCode=VAT.FTVatCode " +
                                     $"WHERE FDVatStart < GETDATE() AND PDT.FTPdtCode='{poTARTSo.tFTPdtCode.ToString()}' " +
                                     $"ORDER BY FDVatStart DESC)";
                double cFCPdtUnitFact = C_GETcPdtUnitFact(poTARTSo.tFTPdtCode.ToString(), tPdtPunCode);
                if (cFCPdtUnitFact != -1)
                {
                    oSql.AppendLine($"INSERT INTO [TARTSoDTTmp] with(rowlock) (");
                    oSql.AppendLine($"[FTBchCode],[FTXshDocNo],[FNXsdSeqNo],[FTPdtCode],");
                    oSql.AppendLine($"[FTXsdPdtName],[FTPunCode],[FTPunName],[FCXsdFactor],");
                    oSql.AppendLine($"[FTXsdBarCode],[FTSrnCode],[FTXsdVatType],[FTVatCode],");
                    oSql.AppendLine($"[FCXsdVatRate],[FTXsdSaleType],[FCXsdSalePrice],[FCXsdQty],");
                    oSql.AppendLine($"[FCXsdQtyAll],[FCXsdSetPrice],[FCXsdAmtB4DisChg],[FTXsdDisChgTxt],");
                    oSql.AppendLine($"[FCXsdDis],[FCXsdChg],[FCXsdNet],[FCXsdNetAfHD],");
                    oSql.AppendLine($"[FCXsdVat],[FCXsdVatable],[FCXsdWhtAmt],[FTXsdWhtCode],");
                    oSql.AppendLine($"[FCXsdWhtRate],[FCXsdCostIn],[FCXsdCostEx],[FTXsdStaPdt],");
                    oSql.AppendLine($"[FCXsdQtyLef],[FCXsdQtyRfn],[FTXsdStaPrcStk],[FTXsdStaAlwDis],");
                    oSql.AppendLine($"[FNXsdPdtLevel],[FTXsdPdtParent],[FCXsdQtySet],[FTPdtStaSet],");
                    oSql.AppendLine($"[FTXsdRmk],[FDLastUpdOn],[FTLastUpdBy],[FDCreateOn],");
                    oSql.AppendLine($"[FTCreateBy])");
                    oSql.AppendLine($" VALUES(");
                    oSql.AppendLine($"'{tBchCode}','{tDocNo}',{pnSeq},'{poTARTSo.tFTPdtCode.ToString()}',");
                    oSql.AppendLine($"{tPdtName},{tPdtPunCode},{tPdtPunName},{cFCPdtUnitFact},");
                    oSql.AppendLine($"'','',{tFTPdtStaVat},{tFTVatCode},");
                    oSql.AppendLine($"{tcFCVatRate},'1',0,{poTARTSo.tFCXsdQty},");
                    oSql.AppendLine($"{double.Parse(poTARTSo.tFCXsdQty) * cFCPdtUnitFact},0,0,'0',");
                    oSql.AppendLine($"0,0,0,0,");
                    oSql.AppendLine($"0,0,0,'0',");
                    oSql.AppendLine($"0,0,0,'1',");
                    oSql.AppendLine($"0,0,'','2',");
                    oSql.AppendLine($"0,'',0,'1',");
                    oSql.AppendLine($"'{poTARTSo.tFTXsdLbl}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{poTARTSo.tFTDptCode.ToString()}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',");
                    oSql.AppendLine($"'{poTARTSo.tFTDptCode.ToString()}')");


                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowAffect);
                    if (nRowAffect > 0) return true;
                    else return false;
                }
                else return false;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return false;
        }
        private bool C_PRCbCopyTmptoDB(string ptTable_Name, string ptTableTmp_Name, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect;

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine($"INSERT INTO {ptTable_Name}");
                oSql.AppendLine($"SELECT * FROM {ptTableTmp_Name}");
                oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowAffect);


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
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        private bool C_PRCbDBInsertDocApvTxnTmp(cmlReqTARTSo poTARTSo, int pnSeq)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                if (!String.IsNullOrEmpty(tDocNo))
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();
                    int nRowAffect;
                    string tDocType = $"(SELECT FTSatStaDocType FROM TCNTAuto WHERE FTSatTblName='TARTSOHD')";
                    string tUser = "HIS";

                    string tUserApv = "null";
                    string tDateApv = "null";
                    string tStaPrc = "null";

                    oSql.AppendLine($"INSERT INTO [TARTDocApvTxnTmp] with(rowlock) (");
                    oSql.AppendLine($"[FTBchCode],[FTDatRefCode],[FTDatRefType],[FNDatApvSeq],");
                    oSql.AppendLine($"[FTDatUsrApv],[FDDatDateApv],[FTDatStaPrc],[FTDatRmk],");
                    oSql.AppendLine($"[FDLastUpdOn],[FTLastUpdBy],[FDCreateOn],[FTCreateBy])");
                    oSql.AppendLine($" VALUES(");
                    oSql.AppendLine($"'{tBchCode}','{tDocNo}',{tDocType},{pnSeq},");
                    oSql.AppendLine($"{tUserApv},{tDateApv},{tStaPrc},'',");
                    oSql.AppendLine($"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{tUser}',");
                    oSql.AppendLine($"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{tUser}')");


                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowAffect);


                    if (nRowAffect > 0) return true;
                    else return false;
                }
                else new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + "Error AutoDocNo");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return true;
        }
        private bool C_PRCbDBUpdateAutoHis(string ptTbl_Name, string ptTbl_Field)
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect, nPrefix;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine("SELECT FTAhmFmtAll ");
                oSql.AppendLine("FROM TCNTAutoHisMas ");
                oSql.AppendLine($"WHERE FTAhmTblName='{ptTbl_Name}' AND FTAhmFedCode='{ptTbl_Field}'");
                nPrefix = oDB.C_GETaDataQuery<string>(t_ConnStr, oSql.ToString(), oShopDB.nConnectTimeOut.Value)[0].Replace("#", "").Length + 1;

                if (nPrefix > 0)
                {
                    oSql.Clear();
                    oSql.AppendLine($"UPDATE [TCNTAutoHisMas]");
                    oSql.AppendLine($" SET [FNAhmLastNum] = {int.Parse(tDocNo.Remove(0, nPrefix)) + 1}");
                    oSql.AppendLine($" ,[FTLastUpdBy] = 'HIS'");
                    oSql.AppendLine($" ,[FDLastUpdOn] = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
                    oSql.AppendLine($" WHERE FTAhmTblName='{ptTbl_Name}' AND FTAhmFedCode='{ptTbl_Field}'");
                    oSql.AppendLine($"");

                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowAffect);
                    if (nRowAffect > 0) return true;
                    else return false;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return false;
        }

        private bool C_PRCbDBDeleteDocNo()
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect;
            try
            {
                if (!String.IsNullOrEmpty(tDocNo))
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();

                    oSql.AppendLine($" DELETE FROM [dbo].[TARTSoHD]");
                    oSql.AppendLine($" WHERE FTBchCode = '{cVB.tVB_BchCode}' AND FTXshDocNo = '{tDocNo}';");
                    oSql.AppendLine($" DELETE FROM [dbo].[TARTSoHDCst]");
                    oSql.AppendLine($" WHERE FTBchCode = '{cVB.tVB_BchCode}' AND FTXshDocNo = '{tDocNo}';");
                    oSql.AppendLine($" DELETE FROM [dbo].[TARTSoDT]");
                    oSql.AppendLine($" WHERE FTBchCode = '{cVB.tVB_BchCode}' AND FTXshDocNo = '{tDocNo}';");
                    oSql.AppendLine($" DELETE FROM [dbo].[TARTDocApvTxn]");
                    oSql.AppendLine($" WHERE FTBchCode = '{cVB.tVB_BchCode}' AND FTDatRefCode = '{tDocNo}';");

                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowAffect);


                    return true;
                }
                else new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + "Error AutoDocNo");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSO", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return false;
        }

        string C_GETtShopCode(string ptPosCode)
        {
            StringBuilder oSql;
            cDatabase oDB;

            Exception oExept = new Exception();
            string tShpCode = null;

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                oSql.AppendLine("SELECT FTShpCode ");
                oSql.AppendLine("FROM TVDMPosShop ");
                oSql.AppendLine($"WHERE FTPosCode='{ptPosCode}' AND FTBchCode='{cVB.tVB_BchCode}'");

                tShpCode = oDB.C_GETaDataQuery<string>(t_ConnStr, oSql.ToString(), oShopDB.nConnectTimeOut.Value)[0];
                return tShpCode;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return "";
        }
        string C_GETtDocType(string ptFTSatTblName, string ptFTSatFedCode)
        {
            StringBuilder oSql;
            cDatabase oDB;

            Exception oExept = new Exception();
            string tDocType = null;

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                oSql.AppendLine("SELECT FTSatStaDocType ");
                oSql.AppendLine("FROM TCNTAuto ");
                oSql.AppendLine($"WHERE FTSatTblName='{ptFTSatTblName}' AND FTSatFedCode='{ptFTSatFedCode}'");

                tDocType = oDB.C_GETaDataQuery<string>(t_ConnStr, oSql.ToString(), oShopDB.nConnectTimeOut.Value)[0];
                return tDocType;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return "";
        }
        string C_GETtDocNo(string ptPosCode, string ptShpCode, int nDocType)
        {
            StringBuilder oSql;
            cDatabase oDB;

            string tDocNo = null;

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);

                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptTblName",SqlDbType.VarChar, 30){ Value = "TARTSoHD"},
                    new SqlParameter ("@ptFedCode",SqlDbType.VarChar, 30){ Value = "FTXshDocNo"},
                    new SqlParameter ("@pnDocType",SqlDbType.Int){ Value = nDocType},
                    new SqlParameter ("@pdDate",SqlDbType.DateTime){ Value =DateTime.Now.ToString("yyyy-MM-dd") },
                    new SqlParameter ("@ptPosCode",SqlDbType.VarChar, 5){ Value = ptPosCode},
                    new SqlParameter ("@ptShpCode",SqlDbType.VarChar, 5){ Value = ptShpCode},
                    new SqlParameter ("@FTResult",SqlDbType.VarChar, 30){ Direction = ParameterDirection.Output}
                };
                oDB.C_DATbExecuteStoreProcedure(t_ConnStr, "STP_GETtAutoGen", ref oPara, (int)oShopDB.nCommandTimeOut, "@FTResult");


                tDocNo = oPara.Where(oP => oP.ParameterName == "@FTResult").FirstOrDefault().Value.ToString();
                return tDocNo;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return "";
        }
        double C_GETcPdtUnitFact(string ptPdtCode, string tPdtPunCode)
        {
            StringBuilder oSql;
            cDatabase oDB;

            Exception oExept = new Exception();
            double cPdtUnitFact;

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                oSql.AppendLine($"SELECT [FCPdtUnitFact] ");
                oSql.AppendLine($"FROM [TCNMPdtPackSize] ");
                if (!tPdtPunCode.StartsWith("("))
                    tPdtPunCode = $"'{tPdtPunCode}'";
                oSql.AppendLine($"WHERE FTPdtCode = '{ptPdtCode}' AND FTPunCode = {tPdtPunCode}");


                cPdtUnitFact = oDB.C_GETaDataQuery<double>(t_ConnStr, oSql.ToString(), oShopDB.nConnectTimeOut.Value)[0];
                return cPdtUnitFact;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return -1;
        }
        string C_GETtWahCode(string tShpCode)
        {
            StringBuilder oSql;
            cDatabase oDB;
            string tWahCode = "";

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                oSql.AppendLine($"SELECT FTWahCode FROM TCNMWaHouse WHERE FTBchCode='{tBchCode}' AND FTWahStaType='6' and FTWahRefCode='{tShpCode}' ");

                tWahCode = oDB.C_GETaDataQuery<string>(t_ConnStr, oSql.ToString(), oShopDB.nConnectTimeOut.Value)[0];
                return tWahCode;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return "";
        }
        int C_GETnMaxDocSeq()
        {
            StringBuilder oSql;
            cDatabase oDB;

            Exception oExept = new Exception();
            int nMaxDocSeq = 0;

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                string t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                oSql.AppendLine("SELECT MAX([FNDapSeq])");
                oSql.AppendLine("FROM [TSysDocApv]");
                oSql.AppendLine($"WHERE FTDapTable = 'TARTSoHD' AND FTDapRefType = '{tDocType}'");

                nMaxDocSeq = oDB.C_GETaDataQuery<int>(t_ConnStr, oSql.ToString(), oShopDB.nConnectTimeOut.Value)[0];

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return nMaxDocSeq;
        }

        #region RabbitMQ
        static ConnectionFactory oMQFactory;
        static IConnection oMQConn;
        static IModel oMQPubChannel;
        static IBasicProperties oMQPubProp;
        public enum ExMode
        {
            direct, fanout, headers, topic
        }
        public static bool C_PRCbMQDeclareExchange(string ptExchange, ExMode nMode)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                if (!String.IsNullOrEmpty(ptExchange))
                {
                    oMQPubChannel.ExchangeDeclare(ptExchange, Enum.GetName(typeof(ExMode), nMode), true, false, null);
                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static bool C_PRCbMQDeclareQueue(string ptQueue)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                if (!String.IsNullOrEmpty(ptQueue))
                {
                    oMQPubChannel.QueueDeclare(ptQueue, true, false, false, null);
                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static bool C_PRCbMQBindRouting(string ptExchange, string ptQueue, string ptRoutekey)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                if (!String.IsNullOrEmpty(ptExchange) && !String.IsNullOrEmpty(ptQueue))
                {
                    oMQPubChannel.QueueBind(ptQueue, ptExchange, ptRoutekey, null);
                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static bool C_PRCbMQPublish2Queue(string ptQueueName, string ptMessage)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                var body = Encoding.UTF8.GetBytes(ptMessage);
                oMQPubChannel.BasicPublish("", ptQueueName, false, oMQPubProp, body);

                return true;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static bool C_PRCbMQPublish2Exchange(string ptExchangeName, string ptRouting, string ptMessage)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                if (!String.IsNullOrEmpty(ptExchangeName) && !String.IsNullOrEmpty(ptRouting))
                {
                    var body = Encoding.UTF8.GetBytes(ptMessage);
                    oMQPubChannel.BasicPublish(ptExchangeName, ptRouting, false, oMQPubProp, body);

                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cGenSOSP", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }

        #endregion

    }
}
