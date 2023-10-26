using AdaPos.Models.Database;
using AdaPos.Models.Other;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public static class cExportSale
    {
        public static bool C_GENbExportBill(string ptDocNo, string ptPath, bool pbIsTrans)
        {
            cmlTPSTSal oSaleDoc;
            try
            {
                oSaleDoc = C_GEToSaleDoc(ptDocNo, pbIsTrans);
                if (oSaleDoc == null) return false;
                return C_SAVbFileBill(ptPath, oSaleDoc);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cExportSale:", "C_GENbExportBill : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static cmlTPSTSal C_GEToSaleDoc(string ptDocNo, bool pbIsTrans)
        {
            cmlTPSTSal oSale;
            string tTblSalHD = "TSHD" + cVB.tVB_PosCode;
            string tTblSalHDCst = "TSHDCst" + cVB.tVB_PosCode;
            string tTblSalHDDis = "TSHDDis" + cVB.tVB_PosCode;
            string tTblSalDT = "TSDT" + cVB.tVB_PosCode;
            string tTblSalDTDis = "TSDTDis" + cVB.tVB_PosCode;
            string tTblSalDTPmt = "TSDTPmt" + cVB.tVB_PosCode;
            string tTblSalRC = "TSRC" + cVB.tVB_PosCode;
            string tTblSalRD = "TSRD" + cVB.tVB_PosCode;
            string tTblSalPD = "TSPD" + cVB.tVB_PosCode;
            string tTblTxnSal = "TSHDPsl" + cVB.tVB_PosCode;
            string tTblTxnRd = "TSHDPrd" + cVB.tVB_PosCode;
            try
            {
                cDatabase oDB = new cDatabase();
                StringBuilder oSql = new StringBuilder();
                oSale = new cmlTPSTSal();
                if (pbIsTrans)
                {
                    tTblSalHD = "TPSTSalHD";
                    tTblSalHDCst = "TPSTSalHDCst";
                    tTblSalHDDis = "TPSTSalHDDis";
                    tTblSalDT = "TPSTSalDT";
                    tTblSalDTDis = "TPSTSalDTDis";
                    tTblSalDTPmt = "TPSTSalDTPmt";
                    tTblSalRC = "TPSTSalRC";
                    tTblSalRD = "TPSTSalRD";
                    tTblSalPD = "TPSTSalPD";
                    tTblTxnSal = "TCNTMemTxnSale";
                    tTblTxnRd = "TCNTMemTxnRedeem";
                }
                oSql.Clear();
                oSql.AppendLine("SELECT DISTINCT HD.FTBchCode, HD.FTXshDocNo, HD.FTShpCode, HD.FNXshDocType, HD.FDXshDocDate, HD.FTXshCshOrCrd, HD.FTXshVATInOrEx, HD.FTDptCode, HD.FTWahCode, HD.FTPosCode,");
                oSql.AppendLine("HD.FTShfCode, HD.FNSdtSeqNo, HD.FTUsrCode, HD.FTSpnCode, HD.FTXshApvCode, HD.FTCstCode, HD.FTXshDocVatFull, HD.FTXshRefExt, HD.FDXshRefExtDate, HD.FTXshRefInt,");
                oSql.AppendLine("HD.FDXshRefIntDate, HD.FTXshRefAE, HD.FNXshDocPrint, HD.FTRteCode, HD.FCXshRteFac, HD.FCXshTotal, HD.FCXshTotalNV, HD.FCXshTotalNoDis, HD.FCXshTotalB4DisChgV, HD.FCXshTotalB4DisChgNV,");
                oSql.AppendLine("HD.FTXshDisChgTxt, HD.FCXshDis, HD.FCXshChg, HD.FCXshTotalAfDisChgV, HD.FCXshTotalAfDisChgNV, HD.FCXshRefAEAmt, HD.FCXshAmtV, HD.FCXshAmtNV, HD.FCXshVat, HD.FCXshVatable,");
                oSql.AppendLine("HD.FTXshWpCode, HD.FCXshWpTax, HD.FCXshGrand, HD.FCXshRnd, HD.FTXshGndText, HD.FCXshPaid, HD.FCXshLeft, HD.FTXshRmk, HD.FTXshStaRefund, HD.FTXshStaDoc,");
                oSql.AppendLine("HD.FTXshStaApv, HD.FTXshStaPrcStk, HD.FTXshStaPaid, HD.FNXshStaDocAct, HD.FNXshStaRef, HD.FDLastUpdOn, HD.FTLastUpdBy, HD.FDCreateOn, HD.FTCreateBy, HD.FTRsnCode, HD.FTXshAppVer"); //*Net 63-07-30 เพิ่ม FTRsnCode, FTXshAppVer
                oSql.AppendLine($"FROM {tTblSalHD} HD with(nolock)");
                oSql.AppendLine($"WHERE HD.FTBchCode='{cVB.tVB_BchCode}' AND HD.FTPosCode='{cVB.tVB_PosCode}' ");
                oSql.AppendLine($"AND HD.FTXshStaDoc='1' AND HD.FTXshDocNo='{ptDocNo}'");
                oSale.aoTPSTSalHD = oDB.C_GETaDataQuery<cmlTPSTSalHD>(oSql.ToString());
                if (oSale.aoTPSTSalHD != null && oSale.aoTPSTSalHD.Count > 0)
                {
                    //DT
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT DT.FTBchCode,DT.FTXshDocNo,DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTPunCode,DT.FTPunName,");
                    oSql.AppendLine("DT.FCXsdFactor,DT.FTXsdBarCode,DT.FTSrnCode,DT.FTPplCode,DT.FTXsdVatType,DT.FTVatCode,DT.FCXsdVatRate,DT.FTXsdSaleType,");
                    oSql.AppendLine("DT.FCXsdSalePrice,DT.FCXsdQty,DT.FCXsdQtyAll,DT.FCXsdSetPrice,DT.FCXsdAmtB4DisChg,DT.FTXsdDisChgTxt,DT.FCXsdDis,");
                    oSql.AppendLine("DT.FCXsdChg,DT.FCXsdNet,DT.FCXsdNetAfHD,DT.FCXsdVat,DT.FCXsdVatable,DT.FCXsdWhtAmt,DT.FTXsdWhtCode,");
                    oSql.AppendLine("DT.FCXsdWhtRate,DT.FCXsdCostIn,DT.FCXsdCostEx,DT.FTXsdStaPdt,DT.FCXsdQtyLef,DT.FCXsdQtyRfn,DT.FTXsdStaPrcStk,");
                    oSql.AppendLine("DT.FTXsdStaAlwDis,DT.FNXsdPdtLevel,DT.FTXsdPdtParent,DT.FCXsdQtySet,DT.FTPdtStaSet,DT.FTXsdRmk,");
                    oSql.AppendLine("DT.FDLastUpdOn,DT.FTLastUpdBy,DT.FDCreateOn,DT.FTCreateBy");
                    oSql.AppendLine($"FROM {tTblSalDT} DT with(nolock)");
                    oSql.AppendLine($"WHERE DT.FTBchCode = '{cVB.tVB_BchCode}' AND DT.FTXshDocNo = '{ptDocNo}'");
                    oSale.aoTPSTSalDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());
                    //* เช็ค Model DT ว่ามีหรือไม่
                    if (oSale.aoTPSTSalDT == null || oSale.aoTPSTSalDT.Count == 0)
                    {
                        new cLog().C_WRTxLog("cExportSale:", "C_GEToSaleDoc : " + $"{ptDocNo} {tTblSalDT} invalid");
                        return null;
                    }

                    //RC
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT RC.FTBchCode,RC.FTXshDocNo,RC.FNXrcSeqNo,RC.FTRcvCode,RC.FTRcvName,RC.FTXrcRefNo1,RC.FTXrcRefNo2,");
                    oSql.AppendLine("RC.FDXrcRefDate,RC.FTXrcRefDesc,RC.FTBnkCode,RC.FTRteCode,RC.FCXrcRteFac,RC.FCXrcFrmLeftAmt,RC.FCXrcUsrPayAmt,");
                    oSql.AppendLine("RC.FCXrcDep,RC.FCXrcNet,RC.FCXrcChg,RC.FTXrcRmk,RC.FTPhwCode,RC.FTXrcRetDocRef,RC.FTXrcStaPayOffline,");
                    oSql.AppendLine("RC.FDLastUpdOn,RC.FTLastUpdBy,RC.FDCreateOn,RC.FTCreateBy");
                    oSql.AppendLine($"FROM {tTblSalRC} RC with(nolock)");
                    oSql.AppendLine($"WHERE RC.FTBchCode = '{cVB.tVB_BchCode}' AND RC.FTXshDocNo = '{ptDocNo}'");
                    oSale.aoTPSTSalRC = oDB.C_GETaDataQuery<cmlTPSTSalRC>(oSql.ToString());
                    //*เช็ค Model RC ว่ามีหรือไม่
                    if (oSale.aoTPSTSalRC == null || oSale.aoTPSTSalRC.Count == 0)
                    {
                        new cLog().C_WRTxLog("cExportSale:", "C_GEToSaleDoc : " + $"{ptDocNo} {tTblSalDT} invalid");
                        return null;
                    }

                    //HDCst
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT HDCst.FTBchCode,HDCst.FTXshDocNo,HDCst.FTXshCardID,HDCst.FTXshCardNo,HDCst.FNXshCrTerm,HDCst.FDXshDueDate,");
                    oSql.AppendLine("HDCst.FDXshBillDue,HDCst.FTXshCtrName,HDCst.FDXshTnfDate,HDCst.FTXshRefTnfID,HDCst.FNXshAddrShip,HDCst.FNXshAddrTax");
                    oSql.AppendLine(",HDCst.FTXshCstName,HDCst.FTXshCstTel,HDCst.FCXshCstPnt,HDCst.FCXshCstPntPmt");
                    oSql.AppendLine($"FROM {tTblSalHDCst} HDCst with(nolock)");
                    oSql.AppendLine($"WHERE HDCst.FTBchCode = '{cVB.tVB_BchCode}' AND HDCst.FTXshDocNo = '{ptDocNo}'");
                    oSale.aoTPSTSalHDCst = oDB.C_GETaDataQuery<cmlTPSTSalHDCst>(oSql.ToString());

                    //HDDis
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT HDDis.FTBchCode,HDDis.FTXshDocNo,HDDis.FDXhdDateIns,HDDis.FTXhdDisChgTxt,HDDis.FTXhdDisChgType,");
                    oSql.AppendLine("HDDis.FCXhdTotalAfDisChg,HDDis.FCXhdDisChg,HDDis.FCXhdAmt,HDDis.FTXhdRefCode, ISNULL(HDDis.FTDisCode,'') AS FTDisCode, ISNULL(HDDis.FTRsnCode,'') AS FTRsnCode");      //*Arm 63-03-13 เพิ่ม FTXhdRefCode , *Arm 63-07-21 เพิ่ม FTDisCode, FTRsnCode
                    oSql.AppendLine($"FROM {tTblSalHDDis} HDDis with(nolock)");
                    oSql.AppendLine($"WHERE HDDis.FTBchCode = '{cVB.tVB_BchCode}' AND HDDis.FTXshDocNo = '{ptDocNo}'");
                    oSale.aoTPSTSalHDDis = oDB.C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());


                    //DTDis
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT DTDis.FTBchCode,DTDis.FTXshDocNo,DTDis.FNXsdSeqNo,DTDis.FDXddDateIns,DTDis.FNXddStaDis,");
                    oSql.AppendLine("DTDis.FTXddDisChgTxt,DTDis.FTXddDisChgType,DTDis.FCXddNet,DTDis.FCXddValue,DTDis.FTXddRefCode, ISNULL(DTDis.FTDisCode,'') AS FTDisCode, ISNULL(DTDis.FTRsnCode,'') AS FTRsnCode"); //*Arm 63-03-13 เพิ่ม FTXddRefCode, *Arm 63-07-21 เพิ่ม FTDisCode, FTRsnCode
                    oSql.AppendLine($"FROM {tTblSalDTDis} DTDis with(nolock)");
                    oSql.AppendLine($"WHERE DTDis.FTBchCode = '{cVB.tVB_BchCode}' AND DTDis.FTXshDocNo = '{ptDocNo}'");
                    oSale.aoTPSTSalDTDis = oDB.C_GETaDataQuery<cmlTPSTSalDTDis>(oSql.ToString());

                    //RD
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT RD.FTBchCode, RD.FTXshDocNo,RD.FNXrdSeqNo,RD.FTRdhDocType,");
                    oSql.AppendLine("RD.FNXrdRefSeq,RD.FTXrdRefCode,RD.FCXrdPdtQty,RD.FNXrdPntUse");
                    oSql.AppendLine($"FROM {tTblSalRD} RD with(nolock)");
                    oSql.AppendLine($"WHERE RD.FTBchCode = '{cVB.tVB_BchCode}' AND RD.FTXshDocNo = '{ptDocNo}'");
                    oSale.aoTPSTSalRD = oDB.C_GETaDataQuery<cmlTPSTSalRD>(oSql.ToString());

                    //PD
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT PD.FTBchCode,PD.FTXshDocNo,PD.FTPmhDocNo,PD.FNXsdSeqNo,PD.FTPmdGrpName,");
                    oSql.AppendLine("PD.FTPdtCode, PD.FTPunCode, PD.FCXsdQty, PD.FCXsdQtyAll, PD.FCXsdSetPrice,");
                    oSql.AppendLine("PD.FCXsdNet, PD.FCXpdGetQtyDiv, PD.FTXpdGetType, PD.FCXpdGetValue, PD.FCXpdDis,");
                    oSql.AppendLine("PD.FCXpdPerDisAvg, PD.FCXpdDisAvg, PD.FCXpdPoint, PD.FTXpdStaRcv, PD.FTPplCode,");
                    oSql.AppendLine("PD.FTXpdCpnText, PD.FTCpdBarCpn ");
                    oSql.AppendLine($"FROM {tTblSalPD} PD with(nolock)");
                    oSql.AppendLine($"WHERE PD.FTBchCode = '{cVB.tVB_BchCode}' AND PD.FTXshDocNo = '{ptDocNo}'");
                    oSale.aoTPSTSalPD = oDB.C_GETaDataQuery<cmlTPSTSalPD>(oSql.ToString());
                    //+++++++++++++++

                    //TxnSale
                    oSql.Clear();
                    oSql.AppendLine("SELECT DISTINCT TSAL.FTCgpCode,TSAL.FTMemCode,TSAL.FTTxnRefDoc,TSAL.FTTxnRefInt,TSAL.FTTxnRefSpl,");
                    oSql.AppendLine("TSAL.FDTxnRefDate,TSAL.FCTxnRefGrand,TSAL.FCTxnPntOptBuyAmt,TSAL.FCTxnPntOptGetQty,TSAL.FCTxnPntB4Bill,");
                    oSql.AppendLine("TSAL.FDTxnPntStart,TSAL.FDTxnPntExpired,TSAL.FCTxnPntBillQty,TSAL.FCTxnPntUsed,TSAL.FCTxnPntExpired,");
                    oSql.AppendLine("TSAL.FTTxnPntStaClosed,TSAL.FDLastUpdOn,TSAL.FTLastUpdBy,TSAL.FDCreateOn,TSAL.FTCreateBy,");
                    oSql.AppendLine("TSAL.FTTxnPntDocType");
                    oSql.AppendLine($"FROM {tTblTxnSal} TSAL with(nolock)");
                    oSql.AppendLine($"WHERE TSAL.FTTxnRefDoc = '{ptDocNo}' OR TSAL.FTTxnRefInt = '{oSale.aoTPSTSalHD[0].FTXshRefInt}'");
                    oSale.aoTCNTMemTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());


                    //TxnRedeem
                    oSql.Clear();
                    oSql.AppendLine("SELECT DISTINCT TRD.FTCgpCode,TRD.FTMemCode,TRD.FTRedRefDoc,TRD.FTRedRefInt,TRD.FTRedRefSpl,");
                    oSql.AppendLine("TRD.FDRedRefDate,TRD.FCRedPntB4Bill,TRD.FCRedPntBillQty,TRD.FTRedPntStaClosed,TRD.FDRedPntStart,");
                    oSql.AppendLine("TRD.FDRedPntExpired,TRD.FDLastUpdOn,TRD.FTLastUpdBy,TRD.FDCreateOn,TRD.FTCreateBy,");
                    oSql.AppendLine("TRD.FTRedPntDocType");
                    oSql.AppendLine($"FROM {tTblTxnRd} TRD with(nolock)");
                    oSql.AppendLine($"WHERE TRD.FTRedRefDoc = '{ptDocNo}' OR TRD.FTRedRefInt = '{oSale.aoTPSTSalHD[0].FTXshRefInt}'");
                    oSale.aoTCNTMemTxnRedeem = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                    //+++++++++++++


                }

                return oSale;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cExportSale:", "C_GEToSaleDoc : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return null;
        }
        public static bool C_SAVbFileBill(string ptPath, cmlTPSTSal poSaleDoc)
        {
            string tJson;
            try
            {
                tJson = JsonConvert.SerializeObject(poSaleDoc, Formatting.Indented);
                if (File.Exists(ptPath +"\\"+ poSaleDoc.aoTPSTSalHD[0].FTXshDocNo + ".json"))
                {
                    File.Delete(ptPath + "\\" + poSaleDoc.aoTPSTSalHD[0].FTXshDocNo + ".json");
                }
                File.WriteAllText(ptPath + "\\" + poSaleDoc.aoTPSTSalHD[0].FTXshDocNo + ".json", tJson);
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cExportSale:", "C_SAVbFileBill : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
    }
}
