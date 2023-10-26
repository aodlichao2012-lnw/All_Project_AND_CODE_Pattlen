using MQAdaLink.Model.Receive;
using MQAdaLink.Model.ExportSale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MQAdaLink.Models.ExportSale;

namespace MQAdaLink.Class.Export
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
            cmlDataExpSale oData;
            cmlRcvData oReqPending;

            //ตัวแปลตามเอกสารขาย
            DataTable odtTmp;
            string tBchCode = "";
            string tSloc = "";
            string tPlantCode = "";
            string tChannel = "11";
            string tSaleOrg = "";
            string tWahCode = "";
            //++++++++++++++ 
            string tAgnCode = "";   //*Arm 63-08-14
            string tError = "";
            string tMessage = "";
            string tTask = "Vender"; //*Arm 63-08-27
            int nCntHis = 0;
            bool bStaResult = false;
            int nSndRnd = 0;

            cVB.tVB_ApiToken_Url = string.Empty;
            cVB.tVB_ApiToken_Name = string.Empty;
            cVB.tVB_ApiToken_UserName = string.Empty;
            cVB.tVB_ApiToken_Password = string.Empty;
            cVB.tVB_ApiToken_Token = string.Empty;

            cVB.tVB_ApiExport_Url = string.Empty;
            cVB.tVB_ApiExport_Name = string.Empty;
            cVB.tVB_ApiExport_UserName = string.Empty;
            cVB.tVB_ApiExport_Password = string.Empty;
            cVB.tVB_ApiExport_Token = string.Empty;

            try
            {
                ptErrMsg = "";

                if (poData == null)
                {
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Receive Parameter [poData] is null.", tTask);
                    ptErrMsg = "Receive Parameter [poData] is null.";
                    return false;
                }

                if (string.IsNullOrEmpty(poData.ptData))
                {
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Receive Parameter [oData.ptData] is null or Empty.", tTask);
                    ptErrMsg = "Receive Parameter [oData.ptData] is null or Empty.";
                    return false;
                }

                oExpSale = JsonConvert.DeserializeObject<cmlDataExpSale>(poData.ptData);

                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Process Export Sale to KADS Start.", tTask);
                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Sender From : " + poData.ptSource, tTask);
                
                nSndRnd = Convert.ToInt32(oExpSale.ptRound); // รอบการส่ง
                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Send Rounding = " + nSndRnd.ToString(), tTask);
                
                // * Start หาเลขที่เอกสารที่ต้องส่ง
                odtHD = new DataTable();
                //*Arm 63-08-10 
                oSql.Clear();
                oSql.AppendLine("SELECT HD.FTBchCode, HD.FTXshDocNo, HD.FTPosCode, HD.FNXshDocType, HD.FTRteCode,HD.FTXshRefInt");
                oSql.AppendLine("FROM TPSTSalHD HD WITH(NOLOCK) ");
                //oSql.AppendLine("INNER JOIN TFNMRate RT WITH(NOLOCK) ON HD.FTRteCode = RT.FTRteCode");
                oSql.AppendLine("WHERE HD.FTBchCode = '" + oExpSale.ptFilter + "'");
                
                if (!string.IsNullOrEmpty(oExpSale.ptPosCode))
                {
                    //*Arm 63-08-10 ถ้าส่ง POS มา
                    oSql.AppendLine("AND HD.FTPosCode = '"+ oExpSale.ptPosCode + "'");
                }

                if (!string.IsNullOrEmpty(oExpSale.ptDocNoFrm) && !string.IsNullOrEmpty(oExpSale.ptDocNoTo))
                {
                    //*Arm 63-08-10 ถ้าส่งเลขที่เอกสารมา
                    oSql.AppendLine("AND HD.FTXshDocNo BETWEEN '" + oExpSale.ptDocNoFrm + "' AND '" + oExpSale.ptDocNoTo + "' ");
                }

                if (!string.IsNullOrEmpty(oExpSale.ptDateFrm) && !string.IsNullOrEmpty(oExpSale.ptDateTo))
                {
                    //*Arm 63-08-10 ถ้าส่งวันที่มา
                    oSql.AppendLine("AND Convert(Date,HD.FDXshDocDate,121) BETWEEN '" + oExpSale.ptDateFrm + "' AND '" + oExpSale.ptDateTo + "' ");
                }
                new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "SQL find DocNo. for Send : "+ oSql.ToString(), tTask); //*Arm 63-08-10
                odtHD = oDB.C_GEToSQLToDatatable(oSql.ToString());

                // * End หาเลขที่เอกสารที่ต้องส่ง 


                if (odtHD != null && odtHD.Rows.Count > 0)
                {
                    // * Start Load Config ข้อมูลสาขาตามเอกสารขาย
                    tBchCode = oExpSale.ptFilter; //สาขาส่ง
                     
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 ISNULL(BCH.FTBchRefID,'') AS FTBchRefID, ISNULL(BCH.FTAgnCode,'') AS FTAgnCode, ISNULL(BCH.FTWahCode,'') AS FTWahCode, ISNULL(AGN.FTAgnRefCode,'') AS FTAgnRefCode, ISNULL(WAH.FTWahRefNo,'') AS FTWahRefNo, ISNULL(FTWahStaChannel,'') AS FTWahStaChannel");
                    oSql.AppendLine("FROM TCNMBranch BCH WITH(NOLOCK)");
                    oSql.AppendLine("LEFT JOIN TCNMAgency AGN WITH(NOLOCK) ON BCH.FTAgnCode = AGN.FTAgnCode");
                    oSql.AppendLine("LEFT JOIN TLKMWaHouse WAH  WITH(NOLOCK) ON  BCH.FTAgnCode =WAH.FTAgnCode AND BCH.FTBchCode = WAH.FTBchCode AND BCH.FTWahCode = WAH.FTWahCode");
                    oSql.AppendLine("WHERE BCH.FTBchCode = '" + tBchCode + "' ");

                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : SQL find (PlantCode/WahCode/SaleOrg/Sloc) for Send : " + oSql.ToString(), tTask); //*Arm 63-08-10
                    odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        tPlantCode = odtTmp.Rows[0].Field<string>("FTBchRefID"); 
                        tWahCode = odtTmp.Rows[0].Field<string>("FTWahCode");
                        tSaleOrg = odtTmp.Rows[0].Field<string>("FTAgnRefCode");
                        tSloc = odtTmp.Rows[0].Field<string>("FTWahRefNo");
                        tAgnCode = odtTmp.Rows[0].Field<string>("FTAgnCode"); ;   //*Arm 63-08-14

                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "C_PRCbExportSale :Set PlantCode : " + tPlantCode, tTask);   //*Arm 63-08-10
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "WahCode : " + tWahCode, tTask);       //*Arm 63-08-10
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "SaleOrg : " + tSaleOrg, tTask);       //*Arm 63-08-10
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Sloc : " + tSloc, tTask);             //*Arm 63-08-10
                    }
                    // * End Load Config ข้อมูลสาขาตามเอกสารขาย 

                    // Get Api
                    // หาข้อมูลลิ้ง api สำหรับ Get Token
                    if (cVB.oVB_UrlGetToken != null)
                    {
                        // ค้นข้อมูล api แบบ TCNMTxnSpcApi
                        DataRow[] oApiGetToken = cVB.oVB_UrlGetToken.Select("tBchCode = '" + oExpSale.ptFilter + "' AND tAgnCode = '" + tAgnCode + "'  AND tCmpCode = '" + cVB.tVB_CmpCode + "'");

                        if (oApiGetToken.Count<DataRow>() == 0)
                        {
                            // ถ้าไม่เจอ ค้นข้อมูล api แบบ TCNMTxnApi
                            oApiGetToken = cVB.oVB_UrlGetToken.Select("tBchCode = '' AND tAgnCode = '' AND tCmpCode ='' ");
                        }

                        if (oApiGetToken.Count<DataRow>() > 0)
                        {
                            // ถ้าเจอข้อมูล
                            foreach (DataRow oRow in oApiGetToken)
                            {
                                // loop เอาข้อมูลตัวแรก ได้ข้อมูลแล้ว brak
                                cVB.tVB_ApiToken_Url = oRow.Field<string>("tApiURL");
                                cVB.tVB_ApiToken_Name = oRow.Field<string>("tApiName");
                                cVB.tVB_ApiToken_UserName = oRow.Field<string>("tApiUser");
                                if (!string.IsNullOrEmpty(oRow.Field<string>("tApiPwd")))
                                {
                                    cVB.tVB_ApiToken_Password = new cEncryptDecrypt("2").C_CALtDecrypt(oRow.Field<string>("tApiPwd"));
                                }
                                else
                                {
                                    cVB.tVB_ApiToken_Password = oRow.Field<string>("tApiPwd");
                                }
                                cVB.tVB_ApiToken_Token = oRow.Field<string>("tApiToken");
                                break;
                            }
                        }
                        else
                        {
                            // ถ้าไม่เจอข้อมูล
                            ptErrMsg = "Api for get token not found.";
                            new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Load config api : " + ptErrMsg, tTask);
                            return false;
                        }
                    }
                    else
                    {
                        // ถ้าไม่มีข้อมูล
                        ptErrMsg = "Api for get token not found.";
                        new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Load config api : " +ptErrMsg, tTask);
                        return false;
                    }

                    // หาข้อมูลลิ้ง api สำหรับ Export sale (เส้น 6)
                    if (cVB.oVB_UrlExport != null)
                    {
                        DataRow[] oApiExport;
                        oApiExport = cVB.oVB_UrlExport.Select("tBchCode = '" + oExpSale.ptFilter + "' AND tAgnCode = '" + tAgnCode + "' AND tCmpCode = '"+cVB.tVB_CmpCode+"'");
                        // ค้นข้อมูล api แบบ TCNMTxnSpcApi
                        if (oApiExport.Count<DataRow>() == 0)
                        {
                            // ถ้าไม่เจอ ค้นข้อมูล api แบบ TCNMTxnApi
                            oApiExport = cVB.oVB_UrlExport.Select("tBchCode = '' AND tAgnCode = '' AND tCmpCode = '' ");
                        }

                        if (oApiExport.Count<DataRow>() > 0)
                        {
                            // ถ้าเจอข้อมูล
                            foreach (DataRow oRow in oApiExport)
                            {
                                // loop เอาข้อมูลตัวแรก ได้ข้อมูลแล้ว brak
                                cVB.tVB_ApiExport_Url = oRow.Field<string>("tApiURL");
                                cVB.tVB_ApiExport_Name = oRow.Field<string>("tApiName");
                                cVB.tVB_ApiExport_UserName = oRow.Field<string>("tApiUser");
                                if (!string.IsNullOrEmpty(oRow.Field<string>("tApiPwd")))
                                {
                                    cVB.tVB_ApiExport_Password = new cEncryptDecrypt("2").C_CALtDecrypt(oRow.Field<string>("tApiPwd"));
                                }
                                else
                                {
                                    cVB.tVB_ApiExport_Password = oRow.Field<string>("tApiPwd");
                                }
                                cVB.tVB_ApiExport_Token = oRow.Field<string>("tApiToken");
                                break;
                            }
                        }
                        else
                        {
                            // ถ้าไม่เจอข้อมูล
                            ptErrMsg = "Api for Export not found.";
                            new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Load config api/ " + ptErrMsg, tTask);
                            return false;
                        }
                    }
                    else
                    {
                        // ถ้าไม่มีข้อมูล
                        ptErrMsg = "Api for Export not found.";
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Load config api/ " + ptErrMsg, tTask);
                        return false;
                    }

                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : set/ApiExport_Url = " + cVB.tVB_ApiExport_Url, tTask);
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : set/ApiExport_Authen = " + cVB.tVB_ApiExport_Auth, tTask);
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : set/ApiExport_Name = " + cVB.tVB_ApiExport_Name, tTask);

                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : set/ApiToken_Url = " + cVB.tVB_ApiToken_Url, tTask);
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : set/ApiToken_Authen = " + cVB.tVB_ApiToken_Auth, tTask);
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : set/ApiToken_Name = " + cVB.tVB_ApiToken_Name, tTask);
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : set/ApiToken_Token = " + cVB.tVB_ApiToken_Token, tTask);

                    //++++++++

                    //new cLog().C_PRCxLogMonitor("C_PRCbExportSale", "Total Document for Export : " + odtHD.Rows.Count.ToString()+ " Items");

                    //Loop ส่งทีละเอกสาร
                    foreach (DataRow oRow in odtHD.Rows)
                    {
                        string tDocNo = "";
                        string tRteSign = "";
                        string tLogRefNo = "";

                        int nChkStaPrc = 0;
                        tDocNo = oRow.Field<string>("FTXshDocNo");
                        //tRteSign = oRow.Field<string>("FTRteSign");
                        tRteSign = oRow.Field<string>("FTRteCode"); //*Arm 63-08-17

                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start Export DocNo : " + tDocNo +" ...", tTask);

                        // * Start ตรวจสอบเอกสารนี้เคยส่งหรือยังถ้าเคยแล้วเช็คว่าส่งสำเร็จหรือยังถ้ายังให้ทำ Process ต่อ 
                        oSql.Clear();
                        oSql.AppendLine("SELECT COUNT(FTLogTaskRef) AS nCnt FROM TLKTLogHis WITH(NOLOCK) WHERE FTLogTaskRef = '" + tDocNo + "' AND FTLogStaPrc = '1' ");

                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : SQL for Check this document, have you sent it yet? If ever sent. Check if sent successfully or not. : " + oSql.ToString(), tTask); //*Arm 63-08-10
                        nChkStaPrc = oDB.C_DAToExecuteQuery<int>(oSql.ToString());
                        if (nChkStaPrc > 0)
                        {
                            new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : This document ever has been exported successfully. --> Skip.", tTask);
                            continue;
                        }
                        // * End  ตรวจสอบเอกสารนี้เคยส่งหรือยังถ้าเคยแล้วเช็คว่าส่งสำเร็จหรือยังถ้ายังให้ทำ Process ต่อ 

                        // @Log
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Prepare information Data for Export start.", tTask);
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Prepare information OrderHeaderSet.", tTask);

                        // * Start ถ้าเป็นบิลคืน ให้หา SO No. จาก TLKTLogAPI6
                        if (oRow.Field<int>("FNXshDocType") == 9)
                        {
                            oSql.Clear();
                            //oSql.AppendLine("SELECT FTLogRefNo FROM TLKTLogAPI6 WITH(NOLOCK) WHERE FTXshDocNo = '" + tDocNo + "' AND FTLogRefNo !=''");
                            oSql.AppendLine("SELECT FTLogRefNo FROM TLKTLogAPI6 WITH(NOLOCK) WHERE FTXshDocNo = '" + oRow.Field<string>("FTXshRefInt") + "' AND FTLogRefNo !=''"); //*Arm 63-08-18
                            new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : if Bill Refund. --> SQL find SODocNo. : " + oSql.ToString(), tTask);  //*Arm 63-08-10
                            tLogRefNo = oDB.C_DAToExecuteQuery<string>(oSql.ToString());

                            new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : if Bill Refund. --> SODocNo. : " + tLogRefNo, tTask); //*Arm 63-08-10
                        }
                        // * End ถ้าเป็นบิลคืน ให้หา SO No. จาก TLKTLogAPI6


                        // * Start Query Data HeaderSet
                        oHD = new cmlOrderHeaderSet();

                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT ISNULL('"+ tLogRefNo + "','') AS SODocNo, HD.FTXshDocNo AS POSDocNo, FORMAT(Convert(Datetime,HD.FDXshDocDate,121),'yyyyMMdd') AS SalesDocDate, 'ZPOS' AS SalesOrderType,");
                        oSql.AppendLine("'" + tSaleOrg + "' AS SalesOrg, '" + tPlantCode + "' AS SalesPlantCode, HD.FTCreateBy AS SalesEmpCode, '" + tChannel + "' AS SalesChannel, CAST(ISNULL(HD.FCXshRnd, 0) AS Decimal(15,"+ cVB.nVB_Desc + ")) AS SalesRounding, ");
                        oSql.AppendLine("ISNULL(HD.FTXshRmk, '') AS SalesRemark, ISNULL(HD.FTCstCode, '"+cVB.tVB_CstCode+"') AS SalesCustCode,");
                        oSql.AppendLine("CAST(ISNULL((SELECT SUM(FNXrdPntUse) FROM TPSTSalRD WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo = '" + tDocNo + "'),0) + ISNULL(CST.FCXshCstPnt, 0) + ISNULL(CST.FCXshCstPntPmt, 0) AS int) AS SalesPointEarn,");
                        oSql.AppendLine("ISNULL((SELECT SUM(FNXrdPntUse) FROM TPSTSalRD WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo = '" + tDocNo + "'),0) AS SalesPointRedeem, ISNULL(CST.FTXshCtrName, '') AS SalesVIN, ");
                        oSql.AppendLine("ISNULL((CASE WHEN RCVFM.FTFmtCode = '001' THEN 'Z1' "); 
                        oSql.AppendLine("   WHEN RCVFM.FTFmtCode = '003' THEN 'Z2' ");
                        oSql.AppendLine("   WHEN RCVFM.FTFmtCode = '002' THEN 'Z3' ");
                        oSql.AppendLine("   WHEN RCVFM.FTFmtCode = '005' THEN 'Z4' ");
                        oSql.AppendLine("   WHEN RCVFM.FTFmtCode = '013' THEN 'Z5' ");
                        oSql.AppendLine("   END),'') AS SalesRcvCode ");
                        oSql.AppendLine("FROM TPSTSalHD HD WITH(NOLOCK)");
                        oSql.AppendLine("LEFT JOIN TPSTSalHDCst CST WITH(NOLOCK) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo ");
                        oSql.AppendLine("INNER JOIN TPSTSalRC RC WITH(NOLOCK) ON HD.FTBchCode = RC.FTBchCode AND HD.FTXshDocNo = RC.FTXshDocNo ");
                        oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RC.FTRcvCode = RCV.FTRcvCode "); //*Arm 63-08-18
                        oSql.AppendLine("INNER JOIN TSysRcvFmt RCVFM WITH(NOLOCK) ON RCV.FTFmtCode = RCVFM.FTFmtCode "); //*Arm 63-08-18
                        oSql.AppendLine("WHERE HD.FTBchCode = '" + tBchCode + "' AND HD.FTXshDocNo = '" + tDocNo + "' ");

                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : " + tDocNo + " : OrderHeaderSet SQL/" + oSql.ToString(), tTask); //*Arm 63-08-09
                        oHD = oDB.C_DAToExecuteQuery<cmlOrderHeaderSet>(oSql.ToString());
                        // * End Query Data HeaderSet


                        // * Start Sub Query Data HeaderSet
                        if (oHD != null && !string.IsNullOrEmpty(oHD.POSDocNo))
                        {

                            // * Start Query Data OrderItemSet (SalDT)
                            new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Prepare information OrderItemSet.", tTask);

                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL('" + tLogRefNo + "','') AS SODocNo, FNXsdSeqNo AS SalesSeqNo, FTPdtCode AS SalesMatNo,");
                            oSql.AppendLine("FTPunCode AS SalesUnit, '" + tSloc + "' AS SalesSloc,");
                            oSql.AppendLine("CAST(ROUND(FCXsdSetPrice, " + cVB.nVB_Desc + ") AS decimal(15, " + cVB.nVB_Desc + ")) AS SalesUnitPrice, ");
                            oSql.AppendLine("CAST(ROUND(FCXsdQtyAll, " + cVB.nVB_Desc + ") AS decimal(15, " + cVB.nVB_Desc + ")) AS SalesQtyAll");
                            oSql.AppendLine("FROM TPSTSalDT WITH(NOLOCK) ");
                            oSql.AppendLine("WHERE FTBchCode = '" + tBchCode + "' AND FTXshDocNo = '" + tDocNo + "' ");
                            oSql.AppendLine("ORDER BY FNXsdSeqNo ASC ");

                            new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : " + tDocNo + " : OrderItemSet SQL/" + oSql.ToString(), tTask); //*Arm 63-08-09
                            oHD.OrderItemSet = oDB.C_GETaDataQuery<cmlOrderItemSet>(oSql.ToString());
                            // * End Query Data OrderItemSet (SalDT)

                            // * Start Query Data OrderCondSet (Discount)
                            new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Prepare information OrderCondSet.", tTask);

                            oSql.Clear();
                            oSql.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY DTDis.FNXsdSeqNo, DTDis.FDXddDateIns ASC) AS SalesDiscSeqNo,ISNULL('" + tLogRefNo + "','') AS SODocNo, DTDis.FNXsdSeqNo AS SalesSeqNo,");
                            oSql.AppendLine("ISNULL(DCP.FTDisCodeRef,'') AS SalesCondType,");
                            oSql.AppendLine("CAST(ROUND((DTDis.FCXddValue - (DTDis.FCXddValue * DT.FCXsdVatRate) / (100 + DT.FCXsdVatRate)), " + cVB.nVB_Desc + ") AS decimal(15, " + cVB.nVB_Desc + ")) AS SalesDiscAmt, ");
                            oSql.AppendLine("'"+ tRteSign + "' AS SalesCurr ");
                            oSql.AppendLine("FROM TPSTSalDTDis DTDis WITH(NOLOCK)");
                            oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo ");
                            oSql.AppendLine("LEFT JOIN TSysDisPolicy DCP WITH(NOLOCK) ON DTDis.FTDisCode = DCP.FTDisCode");
                            oSql.AppendLine("WHERE DTDis.FTBchCode = '" + tBchCode + "' AND DTDis.FTXshDocNo = '" + tDocNo + "' ");
                            oSql.AppendLine("ORDER BY DTDis.FNXsdSeqNo,DTDis.FDXddDateIns ASC ");

                            new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : "+ tDocNo + " : OrderCondSet SQL/" + oSql.ToString(), tTask); //*Arm 63-08-09
                            oHD.OrderCondSet = oDB.C_GETaDataQuery<cmlOrderCondSet>(oSql.ToString());
                            // * End Query Data OrderCondSet (Discount)

                            oHD.OrderMessageSet = new List<cmlOrderMessageSet>();
                        }

                        tMessage = "";
                        tMessage = JsonConvert.SerializeObject(oHD);
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : " + tDocNo + " : JSON (Send)/" + tMessage, tTask); //*Arm 63-08-09
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Prepare information Data for Export Success.", tTask);
                        // * End Sub Query Data HeaderSet


                        // * Start Export to Call Api6
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxExport", tTask);
                        string tJsonResponse = "";
                        tJsonResponse = cFunction.C_PRCxExport(oRow.Field<int>("FNXshDocType"), tMessage, out tError, tLogRefNo); //*Arm 63-08-18 เพิ่ม DocType
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : " + tDocNo + " :  JSON (Response)/" + tJsonResponse, tTask); //*Arm 63-08-09
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxExport", tTask);
                        // * End Export to Call Api6


                        // * Start Api6 Respons Message(1)
                        cmlResExport od = new cmlResExport();
                        if (oRow.Field<int>("FNXshDocType") != 9)
                        {
                            od = JsonConvert.DeserializeObject<cmlResExport>(tJsonResponse);
                        }
                        string tSODocNo = "";

                        // * Start (2) Start หา LogHis ของเอกสารนี้ที่ Process ไม่สำเร็จ ในวันนี้
                        oSql.Clear();
                        oSql.AppendLine("SELECT COUNT(FTLogTaskRef) FROM TLKTLogHis WITH(NOLOCK)");
                        oSql.AppendLine("WHERE Convert(Date,FDLogCreate,121) = Convert(Date,GETDATE(),121) AND FTLogType = '2'");
                        oSql.AppendLine("AND FTLogTaskRef ='" + oHD.POSDocNo + "' AND FTLogStaPrc='2' ");
                        nCntHis = oDB.C_DAToExecuteQuery<int>(oSql.ToString());

                        // * Start (3) Start Check C_CALoApiExportSale Retrun Message Error / ค่าว่าง : Call สำเร็จ, ไม่ว่าง : Call Api ไม่สำเร็จ
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Check Respons", tTask);

                        if (string.IsNullOrEmpty(tError))
                        {
                            //Call Api สำเร็จ
                            //if (od != null && od.d != null && od.d.OrderMessageSet != null && od.d.OrderMessageSet.results != null && od.d.OrderMessageSet.results.Count > 0)
                            if (od != null && od.d != null)
                            {
                                tSODocNo = od.d.SODocNo == null ? string.Empty : od.d.SODocNo; // SO

                                if (od.d.OrderMessageSet != null && od.d.OrderMessageSet.results != null && od.d.OrderMessageSet.results.Count > 0)
                                {
                                    //Check Error Message จาก Model ที Response จาก Api6
                                    foreach (cmlOrderMessageSet oMsgSet in od.d.OrderMessageSet.results)
                                    {
                                        // ** ตรวจสอบ Case Error (SODocNo = "" หรือ MsgType = E )  = Process Error
                                        if (!string.IsNullOrEmpty(tSODocNo))
                                        {
                                            if (oMsgSet.MsgType != "E") // Error
                                            {
                                                bStaResult = true;
                                            }
                                        }

                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Result = " + bStaResult.ToString(), tTask);
                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxInsertTLKTLogHis", tTask);

                                        //Inssert LogHis
                                        C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiExport_Name, oMsgSet.ErrorCode, oMsgSet.ErrorDesc);

                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxInsertTLKTLogHis", tTask);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(tSODocNo))
                                    {
                                        //*arm 63-08-16
                                        bStaResult = true;

                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Result = " + bStaResult.ToString(), tTask);
                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxInsertTLKTLogHis", tTask);
                                        C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiExport_Name, "", "");
                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxInsertTLKTLogHis", tTask);
                                    }
                                    else
                                    {
                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Result = " + bStaResult.ToString(), tTask);
                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxInsertTLKTLogHis", tTask);

                                        //Inssert LogHis
                                        C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiExport_Name, "", "Unknow");

                                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxInsertTLKTLogHis", tTask);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //*arm 63-08-16
                                //ไม่สำเร็จ
                                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Result = " + bStaResult.ToString(), tTask);
                                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxInsertTLKTLogHis", tTask);

                                // Process Call API Retrune Message Error
                                C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiExport_Name, "", tError);

                                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxInsertTLKTLogHis", tTask);
                            }
                        }
                        else
                        {
                            //*Arm 63-08-21
                            if (oRow.Field<int>("FNXshDocType") == 9)
                            {
                                if(tError == "E")
                                {
                                    //ไม่สำเร็จ
                                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Result = " + bStaResult.ToString(), tTask);
                                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxInsertTLKTLogHis", tTask);

                                    // Process Call API Retrune Message Error
                                    C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiExport_Name, tError, tJsonResponse);

                                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxInsertTLKTLogHis", tTask);
                                }
                                else
                                {
                                    bStaResult = true;
                                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Result = " + bStaResult.ToString(), tTask);
                                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxInsertTLKTLogHis", tTask);
                                    C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiExport_Name, "", "");
                                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxInsertTLKTLogHis", tTask);
                                }
                            }
                            else
                            {
                                //ไม่สำเร็จ
                                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Result = " + bStaResult.ToString(), tTask);
                                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxInsertTLKTLogHis", tTask);

                                // Process Call API Retrune Message Error
                                C_PRCxInsertTLKTLogHis(nCntHis, bStaResult, tDocNo, tSODocNo, cVB.tVB_ApiExport_Name, "", tError);

                                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxInsertTLKTLogHis", tTask);
                            }
                        }
                        // * End (3) Start Check C_CALoApiExportSale Retrun Message Error / ค่าว่าง : Call สำเร็จ, ไม่ว่าง : Call Api ไม่สำเร็จ
                        // * End Api6 Respons Message(1)


                        // * Start Insert LogApi6
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Start C_PRCxInsertTLKTLogAPI6", tTask);
                        C_PRCxInsertTLKTLogAPI6(poData.ptSource, tBchCode, tDocNo, tSODocNo, nSndRnd);
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End C_PRCxInsertTLKTLogAPI6", tTask);
                        // * End Insert LogApi6


                        // * Start Case ส่งไม่สำเร็จ
                        int nRound = 0;
                        if (bStaResult == false) //*ไม่สำเร็จส่งใหม่อีกครั้ง
                        {
                            if (poData.ptSource == "MQReceivePrc" && nSndRnd < cVB.nVB_ToSnd) // จำนวนส่งน้อยกว่า จำครั้งส่งที่กำหนด
                            {
                                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Export Result = false --> Re-Send ", tTask);

                                nRound = nSndRnd + 1;

                                oData = new cmlDataExpSale();
                                oData.ptFilter = tBchCode;
                                oData.ptWaHouse = tWahCode;
                                oData.ptRound = nRound.ToString();
                                oData.ptPosCode = oRow.Field<string>("FTPosCode");
                                oData.ptDateFrm = "";
                                oData.ptDateTo = "";
                                oData.ptDocNoFrm = tDocNo;
                                oData.ptDocNoTo = tDocNo;

                                oReqPending = new cmlRcvData();
                                oReqPending.ptFunction = "SalePos";
                                oReqPending.ptSource = "MQReceivePrc";
                                oReqPending.ptDest = "MQAdaLink";
                                oReqPending.ptData = JsonConvert.SerializeObject(oData);

                                string tMsgExpSale = JsonConvert.SerializeObject(oReqPending);
                                cFunction.C_PRCxMQPublish("LK_QSale2Pending",false, tMsgExpSale, out ptErrMsg, tTask);
                                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Publish  Document No." + tDocNo + " to RabbitMQ in Queue Name LK_QSale2Pending. ", tTask);
                                tMsgExpSale = string.Empty;
                            }
                        }
                        new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : End Export DocNo : " + tDocNo+ " ...", tTask);
                        // * End Case ส่งไม่สำเร็จ


                    } // End Loop ส่งทีละเอกสาร
                }

                //Clear
                tBchCode = string.Empty;
                tSloc = string.Empty;
                tWahCode = string.Empty;
                tPlantCode = string.Empty;
                tSaleOrg = string.Empty;
                tChannel = string.Empty;
                

                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Process Export Sale to KADS End.", tTask);
                ptErrMsg = "";
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_PRCxLog("cSale", "C_PRCbExportSale : Error/" + ptErrMsg);
                new cLog().C_PRCxLogMonitor("cSale", "C_PRCbExportSale : Error/" + ptErrMsg, tTask);
                return false;
            }
            finally
            {
                oReqPending = null;
                oData = null;
                odtTmp = null;
                odtHD = null;
                oDB = null;
                oSql = null;
                oHD = null;
                oExpSale = null;
                poData = null;

                cVB.tVB_ApiToken_Url = string.Empty;
                cVB.tVB_ApiToken_Name = string.Empty;
                cVB.tVB_ApiToken_UserName = string.Empty;
                cVB.tVB_ApiToken_Password = string.Empty;
                cVB.tVB_ApiToken_Token = string.Empty;

                cVB.tVB_ApiExport_Url = string.Empty;
                cVB.tVB_ApiExport_Name = string.Empty;
                cVB.tVB_ApiExport_UserName = string.Empty;
                cVB.tVB_ApiExport_Password = string.Empty;
                cVB.tVB_ApiExport_Token = string.Empty;

                new cSP().SP_CLExMemory();
            }
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
            string tTask = "Vender"; //*Arm 63-08-27
            try
            {
                new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKLogAPI6 : Process Insert TLKTLogAPI6 start.", tTask);

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

                new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKLogAPI6 : SQL/" + oSql.ToString(), tTask); //*Arm 63-08-10
                oDB.C_SETxDataQuery(oSql.ToString());

                new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKLogAPI6 : Process Insert TLKTLogAPI6 End.", tTask);
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cSale","C_PRCxInsertTLKLogAPI6 : Error/ "+ oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKLogAPI6 : Error/ " + oEx.Message.ToString(), tTask);
            }
            finally
            {
                ptBchCode = string.Empty;
                ptDocNo = string.Empty;
                ptSODocNo = string.Empty;
                ptSource = string.Empty;
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
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
            string tTask = "Vender"; //*Arm 63-08-27
            try
            {
                new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKTLogHis : Process start.", tTask);
                if (pnCntHis == 0)
                {
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKTLogHis : Insert DocNo : " + ptDocNo + " to TLKTLogHis.", tTask);
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TLKTLogHis (");
                    oSql.AppendLine("FDLogCreate,FTLogType,FTLogTask,FTLogTaskRef,");
                    oSql.AppendLine("FTLogStaPrc,FNLogQtyDone,");
                    oSql.AppendLine("FNLogQtyAll,FTLogStaSend,");
                    oSql.AppendLine("FDCreateOn,FTCreateBy,FDLastUpdOn,FTLastUpdBy");
                    oSql.AppendLine(")VALUES(");
                    oSql.AppendLine("GETDATE() , '2', '" + ptApiName + "', '" + ptDocNo + "',");

                    // สถานะ FTLogStaPrc
                    if (pbStaResult == true) { oSql.AppendLine("'1','1',"); } //สำเร็จ
                    else { oSql.AppendLine("'2','0',"); } //ไม่สำเร็จ

                    oSql.AppendLine("'1','2',");
                    oSql.AppendLine("GETDATE(), 'MQAdaLink', GETDATE(), 'MQAdaLink'");
                    oSql.AppendLine(")");

                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKTLogHis : Insert TLKTLogHis SQL/" + oSql.ToString(), tTask);
                    oDB.C_SETxDataQuery(oSql.ToString());
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKTLogHis : Update DocNo : " + ptDocNo + " to TLKTLogHis.", tTask);
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TLKTLogHis WITH(ROWLOCK) SET");
                    if (pbStaResult == true) { oSql.AppendLine("FTLogStaPrc = '1', FNLogQtyDone = '1',"); } // สถานะ FTLogStaPrc
                    oSql.AppendLine("FTLogStaSend = '2',");
                    oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("FTLastUpdBy = 'MQAdaLink'");
                    oSql.AppendLine("WHERE Convert(Date,FDLogCreate,121) = Convert(Date,GETDATE(),121) AND FTLogType = '2'");
                    oSql.AppendLine("AND FTLogTaskRef ='" + ptDocNo + "' AND FTLogStaPrc='2'");

                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKTLogHis : UPDATE TLKTLogHis SQL/" + oSql.ToString(), tTask);
                    oDB.C_SETxDataQuery(oSql.ToString());
                }

                // ** กรณี Response Error บันทึกข้อมูลลง TLKTLogError
                if (pbStaResult == false)
                {
                    new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKTLogHis : Insert Error Message to TLKTLogError.", tTask);
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TLKTLogError (");
                    oSql.AppendLine("FDErrDate, FTErrCode,FTErrDesc,FTErrRef,");
                    oSql.AppendLine("FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy");
                    oSql.AppendLine(")VALUES(");
                    oSql.AppendLine("GETDATE(),'" + ptErrorCode + "', '" + ptErrorDesc.Replace("'","''") + "', '" + ptDocNo + "',");
                    oSql.AppendLine("GETDATE(), 'MQAdaLink', GETDATE(), 'MQAdaLink'");
                    oSql.AppendLine(")");

                    new cLog().C_PRCxLogMonitor("C_PRCxInsertTLKTLogHis", "Insert TLKTLogError SQL/" + oSql.ToString(), tTask);
                    oDB.C_SETxDataQuery(oSql.ToString());
                }
                new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKTLogHis : Process End.", tTask);
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cSale", "C_PRCxInsertTLKTLogHis : "+ oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cSale", "C_PRCxInsertTLKTLogHis : " + oEx.Message.ToString(), tTask); //*Arm 63-08-27
            }
            finally
            {
                ptApiName = string.Empty;
                ptDocNo = string.Empty;
                ptErrorCode = string.Empty;
                ptErrorDesc = string.Empty;
                ptSODocNo = string.Empty;
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
        }
        
    }
}
