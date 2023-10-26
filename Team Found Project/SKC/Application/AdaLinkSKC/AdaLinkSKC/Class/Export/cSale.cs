using AdaLinkSKC.Model.ExportSale;
using AdaLinkSKC.Model.Receive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class.Export
{
    class cSale
    {
        public bool C_PRCbExportSale(cmlRcvData poData, ref string ptErrMsg)
        {
            DataTable odtHD;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            
            cmlOrderHeaderSet oHD;
            cmlDataExpSale oExpSale;
            
            string tMessage = "";
            int nCntHis = 0;
            bool bStaResult = false;
            int nSndRnd = 0;
            try
            {
                ptErrMsg = "";
                if (poData == null)
                {
                    new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Receive Parameter [poData] is null.");
                    ptErrMsg = "Receive Parameter [poData] is null.";
                    return false;
                }

                if (string.IsNullOrEmpty(poData.ptData))
                {
                    new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Receive Parameter [oData.ptData] is null or Empty.");
                    ptErrMsg = "Receive Parameter [oData.ptData] is null or Empty.";
                    return false;
                }

                oExpSale = JsonConvert.DeserializeObject<cmlDataExpSale>(poData.ptData);

                new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Process Export Sale to KADS Start.");
                new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Send From : "+ poData.ptSource);

                //// ตรวจสอบเป็นเอกสารของสาขาหรือไม่
                //if (cVB.tVB_BchCode != oExpSale.ptFilter)
                //{
                //    new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "MQAdaLink Config BchCode != Receive Message BchCode");
                //    ptErrMsg = "MQAdaLink Config BchCode != Receive Message BchCode";
                //    return false;
                //}

                nSndRnd = Convert.ToInt32(oExpSale.ptRound); // รอบการส่ง
                new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Send Round = " + nSndRnd.ToString());
                
                // หาเลขที่เอกสารที่ต้องส่ง
                odtHD = new DataTable();
                if (string.IsNullOrEmpty(oExpSale.ptDateFrm) && string.IsNullOrEmpty(oExpSale.ptDateTo))
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FTPosCode, FNXshDocType FROM TPSTSalHD WITH(NOLOCK) WHERE FTBchCode = '" + oExpSale.ptFilter + "' AND FTXshDocNo BETWEEN '" + oExpSale.ptDocNoFrm + "' AND '" + oExpSale.ptDocNoTo + "' ");
                    odtHD = oDB.C_GEToSQLToDatatable(oSql.ToString());
                }
                else
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FTPosCode, FNXshDocType  FROM TPSTSalHD WITH(NOLOCK) WHERE FTBchCode = '" + oExpSale.ptFilter + "' AND Convert(Date,FDXshDocDate,121) BETWEEN '" + oExpSale.ptDateFrm + "' AND '" + oExpSale.ptDateTo + "' ");
                    odtHD = oDB.C_GEToSQLToDatatable(oSql.ToString());
                }

                
                if (odtHD != null && odtHD.Rows.Count > 0)
                {
                    new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Total Document for Export : " + odtHD.Rows.Count.ToString()+ " Items");

                    //Loop ส่งทีละเอกสาร
                    foreach (DataRow oRow in odtHD.Rows)
                    {
                        string tDocNo = "";
                        string tLogRefNo = "";
                        int nChkStaPrc = 0;
                        tDocNo = oRow.Field<string>("FTXshDocNo");
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Start Export DocNo : " + tDocNo +" ...");

                        //ตรวจสอบเอกสารนี้เคยส่งหรือยังถ้าเคยแล้วเช็คว่าส่งสำเร็จหรือยังถ้ายังให้ทำ Process ต่อ
                        oSql.Clear();
                        oSql.AppendLine("SELECT COUNT(FTLogTaskRef) AS nCnt FROM TLKTLogHis WITH(NOLOCK) WHERE FTLogTaskRef = '" + tDocNo + "' AND FTLogStaPrc = '1' ");
                        nChkStaPrc = oDB.C_DAToExecuteQuery<int>(oSql.ToString());

                        if (nChkStaPrc > 0)
                        {
                            new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "This document ever has been exported successfully.");
                            continue;
                        }
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Prepare information Data for Export start.");
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Prepare information OrderHeaderSet.");

                        //ถ้าเป็นบิลคืน ให้หา SO No. จาก TLKTLogAPI6
                        if (oRow.Field<int>("FNXshDocType") == 9)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT FTLogRefNo FROM TLKTLogAPI6 WITH(NOLOCK) WHERE FTXshDocNo = '" + tDocNo+"' AND FTLogRefNo !=''");
                            tLogRefNo = oDB.C_DAToExecuteQuery<string>(oSql.ToString());
                        }

                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT ISNULL('"+ tLogRefNo + "','') AS SODocNo, HD.FTXshDocNo AS POSDocNo, FORMAT(Convert(Datetime,HD.FDXshDocDate,121),'yyyyMMdd') AS SalesDocDate, 'ZPOS' AS SalesOrderType,");
                        oSql.AppendLine("'" + cVB.tVB_SaleOrg + "' AS SalesOrg, '" + cVB.tVB_BchRefID + "' AS SalesPlantCode, HD.FTCreateBy AS SalesEmpCode, '" + cVB.tVB_Channel + "' AS SalesChannel, ISNULL(HD.FCXshRnd, 0) AS SalesRounding, ");
                        oSql.AppendLine("ISNULL(HD.FTXshRmk, '') AS SalesRemark, ISNULL(HD.FTCstCode, '') AS SalesCustCode,");
                        oSql.AppendLine("CAST(ISNULL(CST.FCXshCstPnt, 0) + ISNULL(CST.FCXshCstPntPmt, 0) AS int) AS SalesPointEarn,");
                        oSql.AppendLine("ISNULL((SELECT SUM(FNXrdPntUse) FROM TPSTSalRD),0) AS SalesPointRedeem, ISNULL(CST.FTXshCtrName, '') AS SalesVIN, ");
                        oSql.AppendLine("ISNULL((CASE WHEN RC.FTRcvCode = '001' THEN 'Z1' ");
                        oSql.AppendLine("   WHEN RC.FTRcvCode = '010' THEN 'Z2' ");
                        oSql.AppendLine("   WHEN RC.FTRcvCode = '002' THEN 'Z3' ");
                        oSql.AppendLine("   WHEN RC.FTRcvCode = '' THEN 'Z4' ");
                        oSql.AppendLine("   WHEN RC.FTRcvCode = '013' THEN 'Z5' ");
                        oSql.AppendLine("   END),'') AS SalesRcvCode, ISNULL(QR.FTXrcRefNo1, '') AS SalesQrTransId ");
                        oSql.AppendLine("FROM TPSTSalHD HD WITH(NOLOCK)");
                        oSql.AppendLine("LEFT JOIN TPSTSalHDCst CST WITH(NOLOCK) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo ");
                        oSql.AppendLine("INNER JOIN TPSTSalRC RC WITH(NOLOCK) ON HD.FTBchCode = RC.FTBchCode AND HD.FTXshDocNo = RC.FTXshDocNo ");
                        oSql.AppendLine("LEFT JOIN (SELECT FTBchCode, FTXshDocNo, FTXrcRefNo1 FROM TPSTSalRC SRC WITH(NOLOCK)");
                        oSql.AppendLine("           INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON SRC.FTRcvCode = RCV.FTRcvCode ");
                        oSql.AppendLine("           WHERE SRC.FTBchCode = '" + oExpSale.ptFilter + "' AND SRC.FTXshDocNo = '" + tDocNo + "' AND RCV.FTFmtCode = '013') QR ");
                        oSql.AppendLine("   ON HD.FTBchCode = QR.FTBchCode AND HD.FTXshDocNo = QR.FTXshDocNo");
                        oSql.AppendLine("WHERE HD.FTBchCode = '" + oExpSale.ptFilter + "' AND HD.FTXshDocNo = '" + tDocNo + "' ");

                        oHD = new cmlOrderHeaderSet();
                        oHD = oDB.C_DAToExecuteQuery<cmlOrderHeaderSet>(oSql.ToString());
                        if (oHD != null && !string.IsNullOrEmpty(oHD.POSDocNo))
                        {
                            //DT
                            new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Prepare information OrderItemSet.");

                            oSql.Clear();
                            oSql.AppendLine("SELECT '' AS SODocNo, FNXsdSeqNo AS SalesSeqNo, FTPdtCode AS SalesMatNo,");
                            oSql.AppendLine("FTPunCode AS SalesUnit, '" + cVB.tVB_Sloc + "' AS SalesSloc,");
                            oSql.AppendLine("CAST(ROUND(FCXsdSetPrice, " + cVB.nVB_Desc + ") AS decimal(15, 3)) AS SalesUnitPrice, ");
                            oSql.AppendLine("CAST(ROUND(FCXsdQtyAll, " + cVB.nVB_Desc + ") AS decimal(15, 3)) AS SalesQtyAll");
                            oSql.AppendLine("FROM TPSTSalDT WITH(NOLOCK) ");
                            oSql.AppendLine("WHERE FTBchCode = '" + oExpSale.ptFilter + "' AND FTXshDocNo = '" + tDocNo + "' ");
                            oSql.AppendLine("ORDER BY FNXsdSeqNo ASC ");
                            oHD.OrderItemSet = oDB.C_GETaDataQuery<cmlOrderItemSet>(oSql.ToString());
                            
                            //Discount
                            new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Prepare information OrderCondSet.");

                            oSql.Clear();
                            oSql.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY DTDis.FNXsdSeqNo, DTDis.FDXddDateIns ASC) AS SalesDiscSeqNo,'' AS SODocNo, DTDis.FNXsdSeqNo AS SalesSeqNo,");
                            oSql.AppendLine("CASE ");
                            oSql.AppendLine("     WHEN DTDis.FNXddStaDis = 0  THEN ISNULL(PMT.FTPmhRefAccCode,'') ");
                            oSql.AppendLine("     WHEN DTDis.FNXddStaDis = 1  THEN  ");
                            oSql.AppendLine("        CASE ");
                            oSql.AppendLine("            WHEN DTDis.FTXddDisChgType = '1' THEN 'ZDI6' ");
                            oSql.AppendLine("            WHEN DTDis.FTXddDisChgType = '2' THEN 'ZDC8' ");
                            oSql.AppendLine("        END ");
                            oSql.AppendLine("     WHEN DTDis.FNXddStaDis = 2  THEN ");
                            oSql.AppendLine("        CASE ");
                            oSql.AppendLine("            WHEN ISNULL(FTXddRefCode,'') = '' THEN ");
                            oSql.AppendLine("                CASE ");
                            oSql.AppendLine("                    WHEN DTDis.FTXddDisChgType = '1' THEN 'ZDHV' WHEN DTDis.FTXddDisChgType = '2' THEN 'ZDHP' END ");
                            oSql.AppendLine("            ELSE ISNULL(RD.FTRdhRefAccCode,'')");
                            oSql.AppendLine("        END ");
                            oSql.AppendLine("END AS SalesCondType, ");
                            oSql.AppendLine("CAST(ROUND((DTDis.FCXddValue - (DTDis.FCXddValue * DT.FCXsdVatRate) / (100 + DT.FCXsdVatRate)), " + cVB.nVB_Desc + ") AS decimal(15, 3)) AS SalesDiscAmt, ");
                            oSql.AppendLine("'' AS SalesCurr ");
                            oSql.AppendLine("FROM TPSTSalDTDis DTDis WITH(NOLOCK)");
                            oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo ");
                            oSql.AppendLine("LEFT JOIN TCNTPdtPmtHD PMT WITH(NOLOCK) ON DTDis.FTXddRefCode = PMT.FTPmhDocNo ");
                            oSql.AppendLine("LEFT JOIN TARTRedeemHD RD WITH(NOLOCK) ON DTDis.FTXddRefCode = RD.FTRdhDocNo ");
                            oSql.AppendLine("WHERE DTDis.FTBchCode = '" + oExpSale.ptFilter + "' AND DTDis.FTXshDocNo = '" + tDocNo + "' ");
                            oSql.AppendLine("ORDER BY DTDis.FNXsdSeqNo,DTDis.FDXddDateIns ASC ");
                            oHD.OrderCondSet = oDB.C_GETaDataQuery<cmlOrderCondSet>(oSql.ToString());

                            
                        }
                        tMessage = "";
                        tMessage = JsonConvert.SerializeObject(oHD);
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Prepare information Data for Export Success.");

                        // Call API
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Start C_CALoApiExportSale");
                        string tJsonResponse = "";
                        string tError = "";
                        tJsonResponse = C_CALoApiExportSale(tMessage, out tError);
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "End C_CALoApiExportSale");

                        // Respons Messge
                        cmlOrderHeaderSet oRes = new cmlOrderHeaderSet();
                        oRes = JsonConvert.DeserializeObject<cmlOrderHeaderSet>(tJsonResponse);
                        string tSODocNo = "";

                        //หา LogHis ของเอกสารนี้ที่ Process ไม่สำเร็จ ในวันนี้
                        oSql.Clear();
                        oSql.AppendLine("SELECT COUNT(FTLogTaskRef) FROM TLKTLogHis WITH(NOLOCK)");
                        oSql.AppendLine("WHERE Convert(Date,FDLogCreate,121) = Convert(Date,GETDATE(),121) AND FTLogType = '2'");
                        oSql.AppendLine("AND FTLogTaskRef ='" + oHD.POSDocNo + "' AND FTLogStaPrc='2' ");
                        nCntHis = oDB.C_DAToExecuteQuery<int>(oSql.ToString());

                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Check Respons");
                        if (string.IsNullOrEmpty(tError)) // Process Call API Retrune Message Error = ""
                        {
                            if (oRes != null && oRes.OrderMessageSet.Count > 0)
                            {
                                tSODocNo = oRes.SODocNo == null ? string.Empty : oRes.SODocNo;
                                

                                //Check Error Message
                                foreach (cmlOrderMessageSet oMsgSet in oRes.OrderMessageSet)
                                {
                                    // ** ตรวจสอบ Case Error (SODocNo = "" หรือ MsgType = E )  
                                    if (!string.IsNullOrEmpty(tSODocNo))
                                    {
                                        if (oMsgSet.MsgType != "E") // Error
                                        {
                                            bStaResult = true;
                                        }
                                    }

                                    new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Result = "+ bStaResult.ToString());
                                    new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Start C_PRCxInsertTLKTLogHis");

                                    //Inssert LogHis
                                    C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiName, oMsgSet.ErrorCode, oMsgSet.ErrorDesc);

                                    new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "End C_PRCxInsertTLKTLogHis");
                                }
                            }
                        }
                        else
                        {
                            new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Result = " + bStaResult.ToString());
                            new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Start C_PRCxInsertTLKTLogHis");

                            // Process Call API Retrune Message Error
                            C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiName, "", tError);

                            new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "End C_PRCxInsertTLKTLogHis");
                        }

                        // Insert LogApi6
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Start C_PRCxInsertTLKTLogAPI6");
                        
                        C_PRCxInsertTLKTLogAPI6(poData.ptSource, oExpSale.ptFilter, tDocNo, tSODocNo, nSndRnd);

                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "End C_PRCxInsertTLKTLogAPI6");
                        // end Insert LogApi6

                        
                        int nRound = 0;
                        if (bStaResult == false) //*ไม่สำเร็จส่งใหม่อีกครั้ง
                        {
                            if (poData.ptSource == "MQReceivePrc" && nSndRnd < cVB.nVB_ToSnd)
                            {
                                new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Result = false --> Re-Send ");

                                nRound = nSndRnd + 1;

                                cmlDataExpSale oData = new cmlDataExpSale();
                                oData.ptFilter = oExpSale.ptFilter;
                                oData.ptWaHouse = cVB.tVB_WahCode;
                                oData.ptRound = nRound.ToString();
                                oData.ptPosCode = oRow.Field<string>("FTPosCode");
                                oData.ptDateFrm = "";
                                oData.ptDateTo = "";
                                oData.ptDocNoFrm = tDocNo;
                                oData.ptDocNoTo = tDocNo;

                                cmlRcvData oReqPending = new cmlRcvData();
                                oReqPending.ptFunction = "SalePos";
                                oReqPending.ptSource = "MQReceivePrc";
                                oReqPending.ptDest = "MQAdaLink";
                                oReqPending.ptData = JsonConvert.SerializeObject(oData);

                                string tMsgExpSale = JsonConvert.SerializeObject(oReqPending);
                                cFunction.C_PRCxMQPublish("LK_QSale2Pending",false, tMsgExpSale, out ptErrMsg);
                                new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Publish  Document No." + tDocNo + " to RabbitMQ in Queue Name LK_QSale2Pending. ");
                            }
                        }
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "End Export DocNo : " + tDocNo+ " ...");
                    } // End Loop ส่งทีละเอกสาร
                }
                new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Process Export Sale to KADS End.");
                ptErrMsg = "";
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_PRCxLog("C_PRCxMessage", ptErrMsg);
                return false;
            }
            finally
            {
                odtHD = null;
                oDB = null;
                oSql = null;
                oHD = null;
                oExpSale = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process Call Api (Arm 63-07-06)
        /// </summary>
        /// <param name="ptJsonCall"></param>
        /// <returns></returns>
        public string C_CALoApiExportSale(string ptJsonCall, out string ptErr)
        {
            cClientService oCall;
            HttpResponseMessage oRep;
            List<KeyValuePair<string, string>> aHeader;
            string tFnc = "";
            string tMsgJson = "";
            string tToken = "";
            try
            {
                new cLog().C_PRCxLogMonitor("C_CALoApiExportSale", " Function Call Api Export Sale start.");

                ptErr = "";
                cVB.tVB_ApiExport = "https://kads-mobi-test.siamkubotadealer.com:44330/sap/opu/odata/SAP/ZDP_GWSRV008_SRV";
                cVB.tVB_ApiAuth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes("D0390-OW:kubota1")));

                if (string.IsNullOrEmpty(cVB.tVB_ApiExport))
                {
                    ptErr = "the Config Api is not set...";
                    return "";
                }

                oCall = new cClientService();
                oRep = new HttpResponseMessage();

                //set Header
                aHeader = new List<KeyValuePair<string, string>>();
                aHeader.Add(new KeyValuePair<string, string>("Authorization", cVB.tVB_ApiAuth));
                aHeader.Add(new KeyValuePair<string, string>("x-csrf-token", "fetch"));

                oCall = new cClientService(aHeader);
                
                try
                {
                    oRep = oCall.C_GEToInvoke(cVB.tVB_ApiExport);
                    oRep.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException oHttpReqEx)
                {
                    ptErr = oHttpReqEx.Message.ToString();
                    new cLog().C_PRCxLog("C_CALoApiExportSale", "HttpRequestException : " + oHttpReqEx.Message.ToString());
                }

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (var oValue in oRep.Headers)
                    {
                        if (oValue.Key == "x-csrf-token")
                        {
                            tToken = oValue.Value.FirstOrDefault();
                        }
                    }
                }
                new cLog().C_PRCxLogMonitor("C_CALoApiExportSale", "End Call API GET Token...");

                // # CALL API เส้น 6 
                if (string.IsNullOrEmpty(tToken))
                {
                    new cLog().C_PRCxLog("C_CALoApiExportSale", "Check Token is Empty !!!");
                    return tMsgJson = "";
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("C_CALoApiExportSale", "Check Token = " + tToken);

                    new cLog().C_PRCxLogMonitor("C_CALoApiExportSale", "Start Call API6...");
                    aHeader = new List<KeyValuePair<string, string>>();
                    aHeader.Add(new KeyValuePair<string, string>("Authorization", cVB.tVB_ApiAuth));
                    aHeader.Add(new KeyValuePair<string, string>("x-csrf-token", tToken));

                    tFnc = "/OrderHeaderSet";

                    oCall = new cClientService();
                    oCall = new cClientService(aHeader);
                    oRep = new HttpResponseMessage();
                    try
                    {
                        oRep = oCall.C_POSToInvoke(cVB.tVB_ApiExport + tFnc, ptJsonCall);
                        oRep.EnsureSuccessStatusCode();
                    }
                    catch (HttpRequestException oHttpReqEx)
                    {
                        ptErr = oHttpReqEx.Message.ToString();
                        new cLog().C_PRCxLog("C_CALoApiExportSale", "HttpRequestException : " + oHttpReqEx.Message.ToString());
                    }

                    if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        tMsgJson = oRep.Content.ReadAsStringAsync().Result;
                    }
                }
                new cLog().C_PRCxLogMonitor("C_CALoApiExportSale", "End Call API6...");
                new cLog().C_PRCxLogMonitor("C_CALoApiExportSale", " Function Call Api Export Sale End.");
            }
            catch(Exception oEx)
            {
                ptErr = oEx.Message.ToString();
                new cLog().C_PRCxLog("C_CALoApiExportSale", oEx.Message.ToString());
            }
            finally
            {
                oRep = null;
                oCall = null;
                new cSP().SP_CLExMemory();
            }

            return tMsgJson;
        }

        /// <summary>
        /// จัดการ SODocNo บันทึกข้อมูล TLKTLogAPI6
        /// </summary>
        /// <param name="ptSource"></param>
        /// <param name="ptBchCode"></param>
        /// <param name="ptDocNo"></param>
        /// <param name="ptSODocNo"></param>
        /// <param name="pnSndRnd"></param>
        public void C_PRCxInsertTLKTLogAPI6(string ptSource, string ptBchCode,string ptDocNo, string ptSODocNo, int pnSndRnd)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();

            try
            {
                new cLog().C_PRCxLogMonitor("C_PRCxInsertTLKTLogAPI6", "Process Insert TLKTLogAPI6 start.");

                oSql.Clear();
                oSql.AppendLine("INSERT INTO TLKTLogAPI6 (");
                oSql.AppendLine("FTBchCode, FTXshDocNo,FTLogRefNo, FNLogSndRound, FTLogStaDoc,");
                oSql.AppendLine("FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy");
                oSql.AppendLine(")VALUES(");
                oSql.AppendLine("'" + ptBchCode + "', '" + ptDocNo + "', '" + ptSODocNo + "', '"+ pnSndRnd + "', ");

                //สถานะเอกสาร 1=Auto  2=Manual
                if (ptSource == "MQReceivePrc")
                {
                    oSql.AppendLine("'1',");
                }
                else
                {
                    oSql.AppendLine("'2',");
                }
                oSql.AppendLine("GETDATE(), 'MQAdaLink', GETDATE(), 'MQAdaLink'");
                oSql.AppendLine(")");
                oDB.C_SETxDataQuery(oSql.ToString());

                new cLog().C_PRCxLogMonitor("C_PRCxInsertTLKTLogAPI6", "Process Insert TLKTLogAPI6 End.");
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("C_PRCxInsertTLKLogAPI6", oEx.Message.ToString());
            }
            finally
            {
                oSql = null;
                oDB = null;
                new cSP().SP_CLExMemory();
            }
        }


        /// <summary>
        /// จัดการ SODocNo บันทึกข้อมูล TLKTLogHis
        /// </summary>
        /// <param name="pnCntHis"></param>
        /// <param name="pbStaResult"></param>
        /// <param name="ptDocNo"></param>
        /// <param name="ptSODocNo"></param>
        /// <param name="ptApiName"></param>
        /// <param name="ptErrorCode"></param>
        /// <param name="ptErrorDesc"></param>
        public void C_PRCxInsertTLKTLogHis(int pnCntHis, bool pbStaResult, string ptDocNo, string ptSODocNo, string ptApiName, string ptErrorCode, string ptErrorDesc)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();

            try
            {
                new cLog().C_PRCxLogMonitor("C_PRCxInsertTLKTLogHis", "Process start.");
                if (pnCntHis == 0)
                {
                    new cLog().C_PRCxLogMonitor("C_PRCxInsertTLKTLogHis", "Insert DocNo : " + ptDocNo + " to TLKTLogHis.");
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TLKTLogHis (");
                    oSql.AppendLine("FDLogCreate,FTLogType,FTLogTask,FTLogTaskRef,");
                    oSql.AppendLine("FTLogStaPrc,");
                    oSql.AppendLine("FNLogQtyAll,FNLogQtyDone,FTLogStaSend,");
                    oSql.AppendLine("FDCreateOn,FTCreateBy,FDLastUpdOn,FTLastUpdBy");
                    oSql.AppendLine(")VALUES(");
                    oSql.AppendLine("GETDATE() , '2', '" + ptApiName + "', '" + ptDocNo + "',");

                    // สถานะ FTLogStaPrc
                    if (pbStaResult == true) { oSql.AppendLine("'1',"); } //สำเร็จ
                    else { oSql.AppendLine("'2',"); } //ไม่สำเร็จ

                    oSql.AppendLine("'1', '1', '2',"); 
                    oSql.AppendLine("GETDATE(), 'MQAdaLink', GETDATE(), 'MQAdaLink'");
                    oSql.AppendLine(")");
                    oDB.C_SETxDataQuery(oSql.ToString());
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("C_PRCxInsertTLKTLogHis", "Update DocNo : " + ptDocNo + " to TLKTLogHis.");
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TLKTLogHis WITH(ROWLOCK) SET");
                    if (pbStaResult == true) { oSql.AppendLine("FTLogStaPrc = '1',"); } // สถานะ FTLogStaPrc
                    oSql.AppendLine("FTLogStaSend = '2',");
                    oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("FTLastUpdBy = 'MQAdaLink'");
                    oSql.AppendLine("WHERE Convert(Date,FDLogCreate,121) = Convert(Date,GETDATE(),121) AND FTLogType = '2'");
                    oSql.AppendLine("AND FTLogTaskRef ='" + ptDocNo + "' AND FTLogStaPrc='2'");
                    oDB.C_SETxDataQuery(oSql.ToString());
                }

                // ** กรณี Response Error บันทึกข้อมูลลง TLKTLogError
                if (pbStaResult == false)
                {
                    new cLog().C_PRCxLogMonitor("C_PRCxInsertTLKTLogHis", "Insert Error Message to TLKTLogError.");
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TLKTLogError (");
                    oSql.AppendLine("FDErrDate, FTErrCode,FTErrDesc,FTErrRef,");
                    oSql.AppendLine("FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy");
                    oSql.AppendLine(")VALUES(");
                    oSql.AppendLine("GETDATE(),'" + ptErrorCode + "', '" + ptErrorDesc + "', '" + ptDocNo + "',");
                    oSql.AppendLine("GETDATE(), 'MQAdaLink', GETDATE(), 'MQAdaLink'");
                    oSql.AppendLine(")");
                    oDB.C_SETxDataQuery(oSql.ToString());
                }
                new cLog().C_PRCxLogMonitor("C_PRCxInsertTLKTLogHis", "Process End.");
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("C_PRCxInsertTLKLogAPI6", oEx.Message.ToString());
            }
            finally
            {
                oSql = null;
                oDB = null;
                new cSP().SP_CLExMemory();
            }
        }
        
    }
}
