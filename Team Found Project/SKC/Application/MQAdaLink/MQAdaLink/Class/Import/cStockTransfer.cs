using MQAdaLink.Model.Receive;
using MQAdaLink.Models.Receive;
using MQAdaLink.Models.Stock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Class.Import
{
    class cStockTransfer
    {
        //public void C_PRCxStockTransfer(string ptData)
        //{
        //    StringBuilder oSql = new StringBuilder();
        //    cDatabase oDB = new cDatabase();
        //    cmlReqStkTF oStkTF = new cmlReqStkTF();
        //    SqlParameter[] aoSqlParam;
        //    string tDocNo = "";
        //    string tUser = "AdaLink";
        //    string[] aRole;
        //    int nCheckDTIns = 0;
        //    int nCheckDTErr = 0;
        //    bool bStaPrc = false;
        //    DataTable oDbTblChk = new DataTable();

        //    try
        //    {
        //        oStkTF = JsonConvert.DeserializeObject<cmlReqStkTF>(ptData);
        //        tDocNo = oStkTF.MatDocYear + oStkTF.MatDocNo;
        //        oSql.Clear();
        //        oSql.AppendLine("SELECT * FROM TCNTPdtTwxHD WHERE FTXthRefExt = '"+ oStkTF.MatDocYear + oStkTF.MatDocNo + "' ");
        //        oDbTblChk = oDB.C_GEToSQLToDatatable(oSql.ToString());
        //        if (oDbTblChk.Rows.Count > 0 )
        //        {
        //            // ซ้ำ
        //            new cLog().C_PRCxLog("C_PRCxInsertTLKLogAPI6", "มีเลขที่เอกสารซ้ำ");
        //        }
        //        else
        //        {
        //            foreach (cmlMatDocItem oMatItem in oStkTF.MatDocItemSet)
        //            {
        //                if (!string.IsNullOrEmpty(oMatItem.MaterialReceived))
        //                {
        //                    oSql.Clear();
        //                    oSql.AppendLine("INSERT INTO TCNTPdtTwxDT ");
        //                    oSql.AppendLine("(");
        //                    oSql.AppendLine(" FTBchCode ,FTXthDocNo ,FNXtdSeqNo ,FTPdtCode ,FTXtdPdtName");
        //                    oSql.AppendLine(" ,FTPunCode ,FTPunName ,FCXtdFactor ,FTXtdBarCode ,FTXtdVatType");
        //                    oSql.AppendLine(" ,FTVatCode ,FCXtdVatRate ,FCXtdQty ,FCXtdQtyAll ,FCXtdSetPrice");
        //                    oSql.AppendLine(" ,FCXtdAmt ,FCXtdVat ,FCXtdVatable ,FCXtdNet ,FCXtdCostIn");
        //                    oSql.AppendLine(" ,FCXtdCostEx ,FTXtdStaPrcStk ,FNXtdPdtLevel ,FTXtdPdtParent ,FCXtdQtySet");
        //                    oSql.AppendLine(" ,FTXtdPdtStaSet ,FTXtdRmk ,FDLastUpdOn ,FTLastUpdBy ,FDCreateOn");
        //                    oSql.AppendLine(" ,FTCreateBy");
        //                    oSql.AppendLine(")");
        //                    oSql.AppendLine("VALUES");
        //                    oSql.AppendLine("(");
        //                    oSql.AppendLine(" '" + oStkTF.PlantReceive + "' ,'" + tDocNo + "' ,'" + oMatItem.MatDocItem + "' ,'" + oMatItem.MaterialReceived + "' ,'" + oMatItem.MaterialReceived + "'");
        //                    oSql.AppendLine(" ,'" + oMatItem.Unit + "' ,'" + oMatItem.Unit + "' ,1 ,'" + oMatItem.MaterialReceived + "' ,1");
        //                    oSql.AppendLine(" ,'00002' ,20 ,'" + oMatItem.Qty + "' ,'" + oMatItem.Qty + "' ,0");
        //                    oSql.AppendLine(" ,0 ,0 ,0 ,0 ,0");
        //                    oSql.AppendLine(" ,0 ,null ,null ,null ,null");
        //                    oSql.AppendLine(" ,null ,null ,GETDATE() ,'" + tUser + "' ,GETDATE()");
        //                    oSql.AppendLine(" ,'" + tUser + "'");
        //                    oSql.AppendLine(")");
        //                    oDB.C_SETxDataQuery(oSql.ToString());
        //                    nCheckDTIns = nCheckDTIns + 1;
        //                }
        //                else
        //                {
        //                    nCheckDTErr = nCheckDTErr + 1;
        //                }


        //            }

        //            if (nCheckDTIns > 0)
        //            {
        //                oSql.Clear();
        //                oSql.AppendLine("INSERT INTO TCNTPdtTwxHD ");
        //                oSql.AppendLine("(");
        //                oSql.AppendLine(" FTBchCode ,FTXthDocNo ,FDXthDocDate ,FTXthVATInOrEx ,FTDptCode");
        //                oSql.AppendLine(" ,FTXthMerCode ,FTXthShopFrm ,FTXthShopTo ,FTXthWhFrm ,FTXthWhTo");
        //                oSql.AppendLine(" ,FTXthPosFrm ,FTXthPosTo ,FTUsrCode ,FTSpnCode ,FTXthApvCode");
        //                oSql.AppendLine(" ,FTXthRefExt ,FDXthRefExtDate ,FTXthRefInt ,FDXthRefIntDate ,FNXthDocPrint");
        //                oSql.AppendLine(" ,FCXthTotal ,FCXthVat ,FCXthVatable ,FTXthRmk ,FTXthStaDoc");
        //                oSql.AppendLine(" ,FTXthStaApv ,FTXthStaPrcStk ,FTXthStaDelMQ ,FNXthStaDocAct ,FNXthStaRef");
        //                oSql.AppendLine(" ,FTRsnCode ,FDLastUpdOn ,FTLastUpdBy ,FDCreateOn ,FTCreateBy");
        //                oSql.AppendLine(")");
        //                oSql.AppendLine("VALUES");
        //                oSql.AppendLine("(");
        //                oSql.AppendLine(" '" + oStkTF.PlantReceive + "','" + tDocNo + "' ,'" + oStkTF.MatDocDate + "' ,1 ,''");
        //                oSql.AppendLine(" ,'' ,'' ,'' ,'" + oStkTF.Sloc + "' ,'" + oStkTF.SlocReceive + "'");
        //                oSql.AppendLine(" ,null ,null ,'" + tUser + "' ,null ,''");
        //                oSql.AppendLine(" ,'" + oStkTF.MatDocYear + oStkTF.MatDocNo + "' ,'" + oStkTF.MatDocDate + "' ,'' ,'' ,0");
        //                oSql.AppendLine(" ,0 ,0 ,0 ,'' ,1");
        //                oSql.AppendLine(" ,'' ,'' ,'' ,null ,0");
        //                oSql.AppendLine(" ,'' ,GETDATE() ,'" + tUser + "' ,GETDATE() ,'" + tUser + "'");
        //                oSql.AppendLine(")");
        //                oDB.C_SETxDataQuery(oSql.ToString());
        //            }

        //            if (nCheckDTErr > 0)
        //            {
        //                //error
        //                new cLog().C_PRCxLog("C_PRCxInsertTLKLogAPI6", "ไม่มีรหัสสินค้าจำนวนทั้งหมด " + nCheckDTErr + " รายการ");
        //            }

        //            aoSqlParam = new SqlParameter[] {
        //            new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = oStkTF.PlantReceive},
        //            new SqlParameter ("@ptDocNo", SqlDbType.VarChar, 30){ Value = tDocNo },
        //            new SqlParameter ("@ptWho", SqlDbType.VarChar, 100) { Value = tUser},
        //            new SqlParameter ("@FNResult", SqlDbType.Int){ Direction = ParameterDirection.Output }
        //        };


        //            bStaPrc = new cDatabase().C_DATbExecuteStoreProcedure(cVB.oVB_Config, "STP_DOCxWahPdtTnf", ref aoSqlParam, "@FNResult");

        //            if (bStaPrc)
        //            {
        //                oSql.Clear();
        //                oSql.AppendLine("UPDATE TCNTPdtTwxHD");
        //                oSql.AppendLine("SET");
        //                oSql.AppendLine("FTXthStaApv = 1,");
        //                oSql.AppendLine("FTXthStaPrcStk = 1,");
        //                oSql.AppendLine("FTXthApvCode = 'AdaLink'");
        //                oSql.AppendLine("WHERE FTXthDocNo = '"+ tDocNo + "'");
        //                oDB.C_SETxDataQuery(oSql.ToString());
        //                string ptErrMsg = "";
        //                string tJsonNoti = "";
        //                DataTable oDbTblePosF = new DataTable();
        //                DataTable oDbTblePosT = new DataTable();
        //                //CN_QNotiMsg
        //                cmlDataNoti oDataNoti = new cmlDataNoti();
        //                oDataNoti.ptMsgName = "รายการโอน";
        //                oDataNoti.ptMsgGroup = "ใบโอนสินค้า";
        //                oDataNoti.ptMsgDesc = "รับโอนสินค้า " + nCheckDTIns + " รายการ";
        //                oDataNoti.ptMsgRef = oStkTF.MatDocYear + oStkTF.MatDocNo;
        //                oDataNoti.ptMsgDate = oStkTF.MatDocDate;
        //                cmlDataRole oDataRole = new cmlDataRole();
        //                oDataRole.ptRole = "";
        //                oDataRole.ptUser = tUser;
        //                cmlResNotiMsg oResNoti = new cmlResNotiMsg();
        //                oResNoti.ptFunction = "NotiMsgTNF";
        //                oResNoti.ptSource = "MQAdaLink";
        //                oResNoti.ptDest = "Pos";
        //                oResNoti.ptFilter = oDataRole;
        //                oResNoti.paData = oDataNoti;
        //                tJsonNoti = JsonConvert.SerializeObject(oResNoti);
        //                oDbTblePosF = C_GETxPosCode(oStkTF.Sloc);
        //                oDbTblePosT = C_GETxPosCode(oStkTF.SlocReceive);
        //                if (oDbTblePosF.Rows.Count > 0)
        //                {
        //                    cFunction.C_PRCxMQPublish("CN_QNotiMsg" + oStkTF.PlantReceive + oDbTblePosF.Rows[0]["FTWahRefCode"].ToString(), false, tJsonNoti, out ptErrMsg);
        //                }
        //                else if (oDbTblePosT.Rows.Count > 0)
        //                {
        //                    cFunction.C_PRCxMQPublish("CN_QNotiMsg" + oStkTF.PlantReceive + oDbTblePosT.Rows[0]["FTWahRefCode"].ToString(), false, tJsonNoti, out ptErrMsg);
        //                }
        //                else
        //                {
        //                    //ไม่มีค่า POS
        //                    new cLog().C_PRCxLog("C_PRCxInsertTLKLogAPI6", "ไม่มีค่า Pos ใน TCNMWaHouse");
        //                }

        //            }
        //        }
                
        //    }
        //    catch (Exception oEx) { new cLog().C_PRCxLog("C_PRCxStockTransfer", oEx.Message.ToString()); }
        //}
        
        //public DataTable C_GETxPosCode(string ptWH)
        //{
        //    StringBuilder oSql = new StringBuilder();
        //    cDatabase oDB = new cDatabase();
        //    DataTable oDbTbl = new DataTable();
        //    try
        //    {
        //        oSql.Clear();
        //        oSql.AppendLine("SELECT FTWahRefCode FROM TCNMWaHouse");
        //        oSql.AppendLine("WHERE FTWahStaType = 5 AND FTWahCode = '"+ ptWH + "'");
        //        oDbTbl = oDB.C_GEToSQLToDatatable(oSql.ToString());
        //    }
        //    catch (Exception oEx) { new cLog().C_PRCxLog("C_GETxPosCode", oEx.Message.ToString()); }
        //    return oDbTbl;
        //}




        /// <summary>
        /// ใบโอนสินค้าระหว่างคลัง
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbPdtStockTransfer(cmlRcvData poData, ref string ptErrMsg)
        {
            cmlResDataPdtStockTnf oData;
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            DataTable odtTmpDoc;
            SqlParameter[] aoSqlParam;

            bool bStaPrc = false;
            string tDocNo = "";
            string tQueueName = "";
            string tPOS = "";
            string tMsgJson = "";
            string tBchTo = "";
            string tWahFrm = "";
            string tWahTo = "";

            int nCnt = 0;

            try
            {
                ptErrMsg = "";

                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;

                oDB = new cDatabase();
                odtTmp = new DataTable();
                oSql = new StringBuilder();

                oData = JsonConvert.DeserializeObject<cmlResDataPdtStockTnf>(poData.ptData);
                
                if (oData.data != null && oData.data.Count > 0)
                {
                    //ตรวจสอบเอกสาร เคยทำไปแล้วหรือยัง ถ้าทำแล้ว ให้จบการทำงาน ถ้ายังให้ทำตามขั้นตอนต่อไป
                    oSql.Clear();
                    oSql.AppendLine("SELECT COUNT(FTXthRefExt) FROM TCNTPdtTwxHD WHERE FTXthRefExt = '" + oData.data[0].MatDocYear + oData.data[0].MatDocNo + "' ");
                    nCnt = oDB.C_DAToExecuteQuery<int>(oSql.ToString());
                    if (nCnt > 0)
                    {
                        //เคยทำไปแล้ว
                        return true;
                    }

                    // Set DataTable
                    odtTmp.Columns.Add("FTWahFrm", typeof(string)); // FTWahCode From
                    odtTmp.Columns.Add("FTBchTo", typeof(string)); //FTBchCode Receive
                    odtTmp.Columns.Add("FTWahTo", typeof(string)); //FTWahCode Receive

                    odtTmp.Columns.Add("FTMatDocNo", typeof(string));
                    odtTmp.Columns.Add("FTMatDocYear", typeof(string));
                    odtTmp.Columns.Add("FTMatDocDate", typeof(string));
                    odtTmp.Columns.Add("FTPlant", typeof(string));
                    odtTmp.Columns.Add("FTPlantReceived", typeof(string));
                    odtTmp.Columns.Add("FTSloc", typeof(string));
                    odtTmp.Columns.Add("FTSlocReceived", typeof(string));
                    odtTmp.Columns.Add("FNMatDocItem", typeof(int));
                    odtTmp.Columns.Add("FTMaterialReceived", typeof(string));
                    odtTmp.Columns.Add("FTUnit", typeof(string));
                    odtTmp.Columns.Add("FCQty", typeof(decimal));

                    //เอาข้อมูลที่ส่งมาลง DataTable
                    foreach(cmlPdtStockData oDetail in oData.data)
                    {
                        DataRow oRow = odtTmp.NewRow();
                        oRow["FTWahFrm"] = C_GETxWahCode(oDetail.Sloc, oDetail.Plant);
                        oRow["FTBchTo"] = C_GETxBchCode(oDetail.PlantReceived);
                        oRow["FTWahTo"] = C_GETxWahCode(oDetail.SlocReceived, oDetail.PlantReceived);

                        oRow["FTMatDocNo"] = oDetail.MatDocNo;
                        oRow["FTMatDocYear"] = oDetail.MatDocYear;
                        string tYear = "";
                        string tMonth = "";
                        string tDay = "";

                        tYear = oDetail.MatDocDate.Substring(0, 4).ToString();
                        tMonth = oDetail.MatDocDate.Substring(4, 2).ToString();
                        tDay = oDetail.MatDocDate.Substring(6, 2).ToString();

                        oRow["FTMatDocDate"] = tYear + "-" + tMonth + "-" + tDay;
                        oRow["FTPlant"] = oDetail.Plant;
                        oRow["FTPlantReceived"] = oDetail.PlantReceived;
                        oRow["FTSloc"] = oDetail.Sloc;
                        oRow["FTSlocReceived"] = oDetail.SlocReceived;
                        oRow["FNMatDocItem"] = Convert.ToUInt32(oDetail.MatDocItem);
                        oRow["FTMaterialReceived"] = oDetail.MaterialReceived;
                        oRow["FTUnit"] = oDetail.Unit;
                        oRow["FCQty"] = oDetail.Qty;
                        odtTmp.Rows.Add(oRow);
                    }
                }
                else
                {
                    return true;
                }

                // เอาข้อมูลจาก DataTable ลง Temp
                // Create Table Temp
                oSql.Clear();
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TTmpStockTnf'))");
                oSql.AppendLine("BEGIN ");
                oSql.AppendLine("    CREATE TABLE[dbo].[TTmpStockTnf](");
                oSql.AppendLine("        [FTBchTo][varchar](5) NULL,");
                oSql.AppendLine("        [FTWahFrm] [varchar] (5) NULL,");
                oSql.AppendLine("        [FTWahTo] [varchar] (5) NULL,");
                oSql.AppendLine("        [FTMatDocNo][varchar](10) NULL,");
                oSql.AppendLine("        [FTMatDocYear] [varchar] (4) NULL,");
                oSql.AppendLine("        [FTMatDocDate] [varchar] (10) NULL,");
                oSql.AppendLine("        [FTPlant] [varchar] (5) NULL,");
                oSql.AppendLine("        [FTPlantReceived] [varchar] (5) NULL,");
                oSql.AppendLine("        [FTSloc] [varchar] (5) NULL,");
                oSql.AppendLine("        [FTSlocReceived] [varchar] (5) NULL,");
                oSql.AppendLine("        [FNMatDocItem] [int] NULL,");
                oSql.AppendLine("        [FTMaterialReceived] [varchar] (20) NULL,");
                oSql.AppendLine("        [FTUnit] [varchar] (3) NULL,");
                oSql.AppendLine("        [FCQty] [numeric] (18,4) NULL,");
                oSql.AppendLine("    ) ON[PRIMARY]");
                oSql.AppendLine("END"); 
                oSql.AppendLine("TRUNCATE TABLE TTmpStockTnf");
                oDB.C_SETxDataQuery(oSql.ToString());

                // เอาข้อมูลจาก DataTable ลง Temp
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_Conn, SqlBulkCopyOptions.Default))
                {
                    foreach (DataColumn oColName in odtTmp.Columns)
                    {
                        oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
                    }
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TTmpStockTnf";
                    try
                    {
                        oBulkCopy.WriteToServer(odtTmp);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_PRCxLog("cStockTransfer", "C_PRCbStockTransfer : BulkCopy/TTmpStockTnf/ " + oEx.Message.ToString()); //*Arm 63-08-21
                        return false;
                    }
                }
                
                // รวบรวมเอกสาร
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Group Document.");
                oSql.Clear();
                oSql.AppendLine("SELECT FTBchTo, FTWahFrm, FTWahTo FROM TTmpStockTnf");
                oSql.AppendLine("GROUP BY FTBchTo, FTWahFrm, FTWahTo");
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Group Document/SQL : " + oSql.ToString());
                odtTmpDoc = oDB.C_GEToSQLToDatatable(oSql.ToString());


                if(odtTmpDoc != null && odtTmpDoc.Rows.Count >0)
                {
                    new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Loop insert by group document.");
                    foreach (DataRow oRow in odtTmpDoc.Rows)
                    {
                        // Clear
                        tBchTo = "";
                        tWahFrm = "";
                        tWahTo = "";
                        tDocNo = "";
                        tPOS = "";
                        tQueueName = "";
                        tMsgJson = "";
                        nCnt = 0;
                        //Set
                        tBchTo = oRow.Field<string>("FTBchTo");
                        tWahFrm = oRow.Field<string>("FTWahFrm");
                        tWahTo = oRow.Field<string>("FTWahTo");

                        if (string.IsNullOrEmpty(tBchTo) || string.IsNullOrEmpty(tWahFrm) || string.IsNullOrEmpty(tWahTo))
                        {
                            // ถ้าบางฟิลด์ไม่มีข้อมูล ให้ข้ามไป
                            new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Loop insert by group document./ BchCode=" + tBchTo + ", WahFrm=" + tWahFrm + ", WahTo=" + tWahTo + " --> continue.");
                            continue;
                        }

                        // Get PosCode
                        oSql.Clear();
                        oSql.AppendLine("SELECT TOP 1 PS.FTPosCode FROM TCNMPOS PS WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TCNMWaHouse WH WITH(NOLOCK) ON PS.FTBchCode = WH.FTBchCode AND PS.FTPosCode = WH.FTWahRefCode ");
                        oSql.AppendLine("WHERE WH.FTBchCode = '" + tBchTo + "' AND WH.FTWahCode in ('" + tWahFrm + "','" + tWahTo + "') AND PS.FTPosType = '6' ");

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Get Pos SQL : " + oSql.ToString());
                        tPOS = oDB.C_DAToExecuteQuery<string>(oSql.ToString());

                        if (string.IsNullOrEmpty(tPOS))
                        {
                            // ถ้าไม่มีข้อมูล Pos van sale ให้ข้ามไป
                            new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Loop insert by group document./ POS VAN Sale not found. --> continue.");
                            continue;
                        }

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Loop insert by group document./ BchCode=" + tBchTo + ", WahFrm=" + tWahFrm + ", WahTo=" + tWahTo);

                        // Gen. เลขที่เอกสาร call StoredProcedure.
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/Start.");

                        // set parameter values
                        DateTime dDate = DateTime.Now;

                        //*Arm 63-09-08
                        aoSqlParam = new SqlParameter[] {
                            new SqlParameter ("@ptTblName", SqlDbType.VarChar, 30){ Value = "TCNTPdtTwxHD"},
                            new SqlParameter ("@ptDocType", SqlDbType.VarChar, 10){ Value = "3"},
                            new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = tBchTo},
                            new SqlParameter ("@ptShpCode", SqlDbType.VarChar, 5){ Value = ""},
                            new SqlParameter ("@ptPosCode", SqlDbType.VarChar, 5){ Value = ""},
                            new SqlParameter ("@pdDocDate", SqlDbType.DateTime){ Value = dDate},
                            new SqlParameter ("@ptResult", SqlDbType.VarChar, 30) {
                            Direction = ParameterDirection.Output }
                        };

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ StoredProcedure = dbo.SP_CNtAUTAutoDocNo");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ parameter @ptTblName = TCNTPdtTwxHD");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ parameter @ptDocType = 3");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ parameter @ptBchCode = " + tBchTo);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ parameter @pdDocDate = " + dDate);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ parameter @ptPosCode = ");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ parameter @ptShpCode = ");

                        DataTable oDbTblPdt = new DataTable();
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ ExecuteNonQuery.");
                        bStaPrc = new cDatabase().C_DATbExecuteQueryStoreProcedure(cVB.oVB_Config, "SP_CNtAUTAutoDocNo", ref aoSqlParam, ref oDbTblPdt);

                        if (bStaPrc)
                        {
                            if (oDbTblPdt != null && oDbTblPdt.Rows.Count > 0)
                            {
                                tDocNo = oDbTblPdt.Rows[0].Field<string>("FTXxhDocNo");
                            }

                        }

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo/ DocNo : " + tDocNo);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Gen. DocNo end.");

                        if (string.IsNullOrEmpty(tDocNo))
                        {
                            //DocNo ว่าง
                            new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : DocNo Not Found !!! ");
                            continue;
                        }
                        //+++++++++++++++

                        // INSERT TCNTPdtTwxHD
                        oSql.Clear();
                        oSql.AppendLine("INSERT INTO TCNTPdtTwxHD ");
                        oSql.AppendLine("(");
                        oSql.AppendLine(" FTBchCode ,FTXthDocNo ,FDXthDocDate ,FTXthVATInOrEx ,FTDptCode");
                        oSql.AppendLine(" ,FTXthMerCode ,FTXthShopFrm ,FTXthShopTo ,FTXthWhFrm ,FTXthWhTo");
                        oSql.AppendLine(" ,FTXthPosFrm ,FTXthPosTo ,FTUsrCode ,FTSpnCode ,FTXthApvCode");
                        oSql.AppendLine(" ,FTXthRefExt ,FDXthRefExtDate ,FTXthRefInt ,FDXthRefIntDate ,FNXthDocPrint");
                        oSql.AppendLine(" ,FCXthTotal ,FCXthVat ,FCXthVatable ,FTXthRmk ,FTXthStaDoc");
                        oSql.AppendLine(" ,FTXthStaApv ,FTXthStaPrcStk ,FTXthStaDelMQ ,FNXthStaDocAct ,FNXthStaRef");
                        oSql.AppendLine(" ,FTRsnCode ,FDLastUpdOn ,FTLastUpdBy ,FDCreateOn ,FTCreateBy");
                        oSql.AppendLine(")");
                        oSql.AppendLine("SELECT TOP 1");
                        oSql.AppendLine(" FTBchTo AS FTBchCode,'" + tDocNo + "' AS FTXthDocNo, GETDATE() AS FDXthDocDate , '" + cVB.tVB_VATInOrEx + "' AS FTXthVATInOrEx, '' AS FTDptCode");
                        oSql.AppendLine(" , '' AS FTXthMerCode ,'' AS FTXthShopFrm, '' AS FTXthShopTo, FTWahFrm AS FTXthWhFrm, FTWahTo AS FTXthWhTo");
                        oSql.AppendLine(" , NULL AS FTXthPosFrm, NULL AS FTXthPosTo , 'MQAdaLink' AS FTUsrCode, NULL AS FTSpnCode,'MQAdaLink' AS FTXthApvCode");
                        oSql.AppendLine(" , FTMatDocYear+FTMatDocNo AS FTXthRefExt, FTMatDocDate AS FDXthRefExtDate, '' AS FTXthRefInt, NULL AS FDXthRefIntDate, 0 AS FNXthDocPrint");
                        oSql.AppendLine(" , 0 AS FCXthTotal, 0 AS FCXthVat, 0 AS FCXthVatable, '' AS FTXthRmk, '1' AS FTXthStaDoc");
                        oSql.AppendLine(" , '1' AS FTXthStaApv, '1' AS FTXthStaPrcStk, '1' AS FTXthStaDelMQ, NULL AS FNXthStaDocAct, 0 AS FNXthStaRef");
                        oSql.AppendLine(" ,'' AS FTRsnCode, GETDATE() AS FDLastUpdOn, 'MQAdaLink' AS FTLastUpdBy, GETDATE() AS FDCreateOn, 'MQAdaLink' AS FTCreateBy");
                        oSql.AppendLine("FROM TTmpStockTnf WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE FTBchTo = '" + tBchTo + "' AND FTWahFrm = '" + tWahFrm + "' AND FTWahTo = '" + tWahTo + "' ");

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : INSERT/TCNTPdtTwxHD SQL : " + oSql.ToString());
                        oDB.C_SETxDataQuery(oSql.ToString());

                        // INSERT TCNTPdtTwxDT
                        oSql.Clear();
                        oSql.AppendLine("INSERT INTO TCNTPdtTwxDT ( ");
                        oSql.AppendLine(" FTBchCode ,FTXthDocNo ,FNXtdSeqNo ,FTPdtCode ,FTXtdPdtName");
                        oSql.AppendLine(" ,FTPunCode ,FTPunName ,FCXtdFactor ,FTXtdBarCode ,FTXtdVatType");
                        oSql.AppendLine(" ,FTVatCode ,FCXtdVatRate ,FCXtdQty ,FCXtdQtyAll ,FCXtdSetPrice");
                        oSql.AppendLine(" ,FCXtdAmt ,FCXtdVat ,FCXtdVatable ,FCXtdNet ,FCXtdCostIn");
                        oSql.AppendLine(" ,FCXtdCostEx ,FTXtdStaPrcStk ,FNXtdPdtLevel ,FTXtdPdtParent ,FCXtdQtySet");
                        oSql.AppendLine(" ,FTXtdPdtStaSet ,FTXtdRmk ,FDLastUpdOn ,FTLastUpdBy ,FDCreateOn");
                        oSql.AppendLine(" ,FTCreateBy");
                        oSql.AppendLine(")");
                        oSql.AppendLine("SELECT");
                        oSql.AppendLine(" ST.FTBchTo AS FTBchCode,'" + tDocNo + "' AS FTXthDocNo, ST.FNMatDocItem AS FNXtdSeqNo , ST.FTMaterialReceived AS FTPdtCode, PDTL.FTPdtName AS FTXtdPdtName ");
                        oSql.AppendLine(" , ST.FTUnit AS FTPunCode, PUL.FTPunName AS FTPunName, 1 AS FCXtdFactor, ST.FTMaterialReceived AS FTXtdBarCode, PDT.FTPdtStaVat AS FTXtdVatType ");
                        oSql.AppendLine(" , '" + cVB.tVB_VatCode + "' AS FTVatCode, '" + cVB.cVB_VatRate + "' AS FCXtdVatRate, ST.FCQty AS FCXtdQty, ST.FCQty AS FCXtdQtyAll, 0 AS FCXtdSetPrice ");
                        oSql.AppendLine(" , 0 AS FCXtdAmt , 0 AS FCXtdVat, 0 AS FCXtdVatable, 0 AS FCXtdNet, 0 AS FCXtdCostIn ");
                        oSql.AppendLine(" , 0 AS FCXtdCostEx, NULL AS FTXtdStaPrcStk, NULL AS FNXtdPdtLevel, NULL AS FTXtdPdtParent, NULL AS FCXtdQtySet ");
                        oSql.AppendLine(" , NULL AS FTXtdPdtStaSet, NULL AS FTXtdRmk, GETDATE() AS FDLastUpdOn, 'MQAdaLink' AS FTLastUpdBy, GETDATE() AS FDCreateOn ");
                        oSql.AppendLine(" , 'MQAdaLink' AS FTCreateBy ");
                        oSql.AppendLine("FROM TTmpStockTnf ST WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT WITH(NOLOCK) ON ST.FTMaterialReceived = PDT.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdt_L PDTL WITH(NOLOCK) ON ST.FTMaterialReceived = PDTL.FTPdtCode AND PDTL.FNLngID = '1' ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L PUL WITH(NOLOCK) ON ST.FTUnit = PUL.FTPunCode AND PUL.FNLngID = '1' ");
                        oSql.AppendLine("WHERE FTBchTo = '" + tBchTo + "' AND FTWahFrm = '" + tWahFrm + "' AND FTWahTo = '" + tWahTo + "' ");
                        oSql.AppendLine("ORDER BY FNMatDocItem ASC ");


                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : INSERT/TCNTPdtTwxDT SQL : " + oSql.ToString());
                        oDB.C_SETxDataQuery(oSql.ToString());

                        //*Arm 63-09-08 TCNTPdtTwxHDRef
                        oSql.Clear();
                        oSql.AppendLine("INSERT INTO TCNTPdtTwxHDRef ");
                        oSql.AppendLine("( ");
                        oSql.AppendLine("FTBchCode, FTXthDocNo, FTXthCtrName, FDXthTnfDate, FTXthRefTnfID, ");
                        oSql.AppendLine("FTXthRefVehID, FTXthQtyAndTypeUnit, FNXthShipAdd, FTViaCode ");
                        oSql.AppendLine(") VALUES(");
                        oSql.AppendLine(" '" + tBchTo + "','" + tDocNo + "', '', CONVERT(date,GETDATE(),121),'',");
                        oSql.AppendLine(" '', '', 0, '' ");
                        oSql.AppendLine(")");

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : INSERT/TCNTPdtTwxHDRef SQL : " + oSql.ToString());
                        oDB.C_SETxDataQuery(oSql.ToString());
                        //++++++++++++++

                        
                        // ประมวลผล Stock
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock start.");
                        aoSqlParam = new SqlParameter[] {
                            new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = tBchTo},
                            new SqlParameter ("@ptDocNo", SqlDbType.VarChar, 30){ Value = tDocNo },
                            new SqlParameter ("@ptWho", SqlDbType.VarChar, 100) { Value = "MQAdaLink"},
                            new SqlParameter ("@FNResult", SqlDbType.Int){ Direction = ParameterDirection.Output}
                        };

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock/StoreProcedure = STP_DOCxWahPdtTnf");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock/Set Param : @ptBchCode = " + tBchTo);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock/Set Param : @ptDocNo = " + tDocNo);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock/Set Param : @ptWho = MQAdaLink");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock/Set Param : @FNResult Output");

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock/C_DATbExecuteStoreProcedure start. ");
                        // ExecuteStoreProcedure 
                        new cDatabase().C_DATbExecuteStoreProcedure(cVB.oVB_Config, "STP_DOCxWahPdtTnf", ref aoSqlParam, "@FNResult");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock/C_DATbExecuteStoreProcedure end. ");

                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock/Result = ");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Process Stock end.");

                        //หาจำนวนรายการ
                        oSql.Clear();
                        oSql.AppendLine("SELECT COUNT(*) FROM TCNTPdtTwxDT WITH(NOLOCK) WHERE  FTBchCode = '" + tBchTo + "' AND FTXthDocNo = '" + tDocNo + "'");
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Count Product DT/ SQL : " + oSql.ToString());
                        nCnt = oDB.C_DAToExecuteQuery<int>(oSql.ToString());
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Count Product DT/ Result : " + nCnt.ToString());

                        // ขั้นตอนส่ง PublicMQ
                        tQueueName = "CN_QNotiMsg" + tBchTo + tPOS;
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Publish/PosCode : " + tPOS);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Publish/BchCode : " + tBchTo);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Publish/QueueName : " + tQueueName);

                        // เตรียมข้อมูลสำหรับส่งเข้า Queue
                        cmlRcvNotiData oNotiData = new cmlRcvNotiData();
                        oNotiData.ptMsgName = "รายการโอน";  //ชื่อข้อความ
                        oNotiData.ptMsgGroup = "ใบโอนสินค้า"; //ชื่อข้อความ
                        oNotiData.ptMsgDesc = "รับโอนสินค้า " + nCnt + " รายการ";
                        oNotiData.ptMsgRef = tDocNo;
                        oNotiData.pdMsgDate = DateTime.Now;

                        cmlRcvNoti oRcv = new cmlRcvNoti();
                        oRcv.ptFunction = "NotiMsgTNF";     //ชื่อ Function
                        oRcv.ptSource = "MQAdaLink";        //ต้นทาง
                        oRcv.ptDest = "ptDest";             //ปลายทาง
                        oRcv.ptFilter = "";
                        oRcv.ptData = JsonConvert.SerializeObject(oNotiData);

                        tMsgJson = JsonConvert.SerializeObject(oRcv);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Publish/Message JSON : " + tMsgJson);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : C_PRCxMQPublish : start.");
                        cFunction.C_PRCxMQPublish(tQueueName, false, tMsgJson, out ptErrMsg);
                        new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : C_PRCxMQPublish : end.");


                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message;
                new cLog().C_PRCxLog("cStockTransfer", "C_PRCbStockTransfer : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_PRCbStockTransfer : Error/" + oEx.Message.ToString());
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                odtTmp = null;
                odtTmpDoc = null;
                poData = null;
                oData = null;
                new cSP().SP_CLExMemory();
            }
        }

        public string C_GETxWahCode(string ptSloc, string ptPlant)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            string tReturn = "";
            
            try
            {
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxWahCode : start.");
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxWahCode : Req Plent =" + ptPlant);
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxWahCode : Req Sloc =" + ptSloc);

                oSql.AppendLine("SELECT TOP 1 WH.FTWahCode FROM TLKMWahouse WH WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TCNMBranch BCH WITH(NOLOCK) ON WH.FTBchCode = BCH.FTBchCode  AND BCH.FTBchRefID = '" + ptPlant + "'");
                oSql.AppendLine("WHERE WH.FTWahRefNo = '" + ptSloc + "' ");

                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxWahCode : SQL/" + oSql.ToString());
                tReturn = oDB.C_DAToExecuteQuery<string>(oSql.ToString());

                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxWahCode : Return =" + tReturn);
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxWahCode : end.");
                return tReturn;

            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cStockTransfer", "C_GETxWahCode : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxWahCode : Error/" + oEx.Message.ToString());
                return "";
            }
            finally
            {
                oSql = null;
                oDB = null;
            }
        }

        public string C_GETxBchCode(string ptPlant)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            string tReturn = "";

            try
            {
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxBchCode : start.");
                oSql.AppendLine("SELECT TOP 1 FTBchCode FROM TCNMBranch WITH(NOLOCK) WHERE FTBchRefID = '" + ptPlant + "' ");

                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxBchCode : SQL/" + oSql.ToString());
                tReturn = oDB.C_DAToExecuteQuery<string>(oSql.ToString());
                
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxBchCode : Return =" + tReturn);
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxBchCode : end.");

                return tReturn;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cStockTransfer", "C_GETxBchCode : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cStockTransfer", "C_GETxBchCode : Error/" + oEx.Message.ToString());
                return "";
            }
            finally
            {
                oSql = null;
                oDB = null;
            }

        }
    }
}
