using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Sale;
using MQReceivePrc.Models.Tax;
using MQReceivePrc.Models.Void;
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
    public class cExpress
    {
        public bool C_PRCbUploadExpress(string ptQueueID, string ptData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlRcvDataUpload oUploadExpress;
            string tFunction;
            string tQResponse;
            cmlTPSTSal oSalePos;
            cmlTPSTTax oTax;
            cmlTPSTVoid oVoid;
            try
            {
                if (string.IsNullOrEmpty(ptData)) return false;

                oUploadExpress = JsonConvert.DeserializeObject<cmlRcvDataUpload>(ptData);
                if (oUploadExpress.ptConnStr.Split(';').Length != 2)
                {
                    ptErrMsg = "Invalid Connection String";
                    return true;
                }
                tFunction = oUploadExpress.ptConnStr.Split(';')[0];
                tQResponse = oUploadExpress.ptConnStr.Split(';')[1];

                switch (tFunction)
                {
                    case "SaleExpress":
                        C_PRCbUploadSale(ptQueueID, oUploadExpress, poShopDB,ref ptErrMsg);
                        break;
                    case "VoidExpress":
                        C_PRCbUploadVoid(ptQueueID, oUploadExpress, poShopDB, ref ptErrMsg);
                        break;
                    case "TaxExpress":
                        C_PRCbUploadTax(ptQueueID, oUploadExpress, poShopDB, ref ptErrMsg);
                        break;
                }

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadExpress");
            }
            finally
            {
                //new cFunction().C_CLExMemory();
            }
            return false;
        }
        public bool C_PRCbUploadSale(string ptQueueID, cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDataReader<cmlTPSTSalHD> aoHD;
            cDataReader<cmlTPSTSalHDCst> aoHDCst;
            cDataReader<cmlTPSTSalHDDis> aoHDDis;
            cDataReader<cmlTPSTSalDT> aoDT;
            cDataReader<cmlTPSTSalDTDis> aoDTDis;
            cDataReader<cmlTPSTSalDTPmt> aoDTPmt;
            cDataReader<cmlTPSTSalRC> aoRC;
            cDataReader<cmlTPSTSalRD> aoRD;
            cDataReader<cmlTPSTSalPD> aoPD;
            cDataReader<cmlTCNTMemTxnSale> aoTxnSale;        //*Arm 63-05-07
            cDataReader<cmlTCNTMemTxnRedeem> aoTxnRedeem;   //*Arm 63-05-07

            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlTPSTSal oSalePos;
            SqlTransaction oTranscation;
            SqlConnection oConn;
            cSP oSP = new cSP();

            
            string tQueueID = "";
            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oSalePos = JsonConvert.DeserializeObject<cmlTPSTSal>(poData.ptData);

                tQueueID = ptQueueID.Replace("-", "");

                //*Arm 63-05-30
                string tTblSalHD = "TSHD" + tQueueID;
                string tTblSalHDCst = "TSHDCst" + tQueueID;
                string tTblSalHDDis = "TSHDDis" + tQueueID;
                string tTblSalDT = "TSDT" + tQueueID;
                string tTblSalDTDis = "TSDTDis" + tQueueID;
                string tTblSalRD = "TSRD" + tQueueID;
                string tTblSalRC = "TSRC" + tQueueID;
                string tTblSalPD = "TSPD" + tQueueID;
                string tTblTxnSale = "TTxnSale" + tQueueID;
                string tTblTxnRedeem = "TTxnRedeem" + tQueueID;
                //+++++++++++

                //Create Temp
                #region Create Temp
                
                //*Arm 63-05-30
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalHD", tTblSalHD, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalHDCst", tTblSalHDCst, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalHDDis", tTblSalHDDis, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalDT", tTblSalDT, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalDTDis", tTblSalDTDis, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalRC", tTblSalRC, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalRD", tTblSalRD, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTSalPD", tTblSalPD, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TCNTMemTxnSale", tTblTxnSale, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TCNTMemTxnRedeem", tTblTxnRedeem, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);

                // oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion Create Temp

                oConn = new SqlConnection(cVB.tVB_ConnStr);
                oConn.Open();

                oTranscation = oConn.BeginTransaction();

                //insert to DB
                if (oSalePos.aoTPSTSalHD != null)
                {
                    aoHD = new cDataReader<cmlTPSTSalHD>(oSalePos.aoTPSTSalHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalHDTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHD; //*Arm 63-05-30
                        try
                        {
                            oBulkCopy.WriteToServer(aoHD);
                        }
                        catch (Exception oEx)
                        {

                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalHD + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalHDCst != null)
                {
                    aoHDCst = new cDataReader<cmlTPSTSalHDCst>(oSalePos.aoTPSTSalHDCst);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDCst.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalHDCstTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHDCst; //*Arm 63-05-30
                        try
                        {
                            oBulkCopy.WriteToServer(aoHDCst);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalHDCst + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalHDDis != null)
                {
                    aoHDDis = new cDataReader<cmlTPSTSalHDDis>(oSalePos.aoTPSTSalHDDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalHDDisTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalHDDis; //*Arm 63-05-30
                        try
                        {
                            oBulkCopy.WriteToServer(aoHDDis);
                        }
                        catch (Exception oEx)
                        {

                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalHDDis + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalDT != null)
                {
                    aoDT = new cDataReader<cmlTPSTSalDT>(oSalePos.aoTPSTSalDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalDTTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalDT;
                        try
                        {
                            oBulkCopy.WriteToServer(aoDT);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalDT + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30

                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalDTDis != null)
                {
                    aoDTDis = new cDataReader<cmlTPSTSalDTDis>(oSalePos.aoTPSTSalDTDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalDTDisTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalDTDis; //*Arm 63-05-30

                        try
                        {
                            oBulkCopy.WriteToServer(aoDTDis);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalDTDis + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30

                            return false;
                        }
                    }
                }

                //if (oSalePos.aoTPSTSalDTPmt != null)
                //{
                //    aoDTPmt = new cDataReader<cmlTPSTSalDTPmt>(oSalePos.aoTPSTSalDTPmt);

                //    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                //    {
                //        foreach (string tColName in aoDTPmt.ColumnNames)
                //        {
                //            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                //        }

                //        oBulkCopy.BatchSize = 100;
                //        oBulkCopy.DestinationTableName = "dbo.TPSTSalDTPmtTmp";

                //        try
                //        {
                //            oBulkCopy.WriteToServer(aoDTPmt);
                //        }
                //        catch (Exception oEx)
                //        {
                //            oTranscation.Rollback();
                //            ptErrMsg = oEx.Message.ToString();
                //            return false;
                //        }
                //    }
                //}

                if (oSalePos.aoTPSTSalRC != null)
                {
                    aoRC = new cDataReader<cmlTPSTSalRC>(oSalePos.aoTPSTSalRC);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoRC.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalRCTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalRC; // *Arm 63-05-30
                        try
                        {
                            oBulkCopy.WriteToServer(aoRC);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalRC + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30

                            return false;
                        }
                    }
                }

                if (oSalePos.aoTPSTSalRD != null)
                {
                    aoRD = new cDataReader<cmlTPSTSalRD>(oSalePos.aoTPSTSalRD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoRD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalRDTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalRD; //*Arm 63-05-30
                        try
                        {
                            oBulkCopy.WriteToServer(aoRD);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalRD + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                //*Arm 63-03-26
                if (oSalePos.aoTPSTSalPD != null)
                {
                    aoPD = new cDataReader<cmlTPSTSalPD>(oSalePos.aoTPSTSalPD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoPD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTSalPDTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblSalPD; //*Arm 63-05-30
                        try
                        {
                            oBulkCopy.WriteToServer(aoPD);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblSalPD + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                //*Arm 63-05-07
                if (oSalePos.aoTCNTMemTxnSale != null)
                {
                    aoTxnSale = new cDataReader<cmlTCNTMemTxnSale>(oSalePos.aoTCNTMemTxnSale);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoTxnSale.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TCNTMemTxnSaleTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblTxnSale; //*Arm 63-05-30
                        try
                        {
                            oBulkCopy.WriteToServer(aoTxnSale);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblTxnSale + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTCNTMemTxnRedeem != null)
                {
                    aoTxnRedeem = new cDataReader<cmlTCNTMemTxnRedeem>(oSalePos.aoTCNTMemTxnRedeem);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoTxnRedeem.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TCNTMemTxnRedeemTmp";
                        oBulkCopy.DestinationTableName = "dbo." + tTblTxnRedeem; //*Arm 63-05-30

                        try
                        {
                            oBulkCopy.WriteToServer(aoTxnRedeem);
                        }
                        catch (Exception oEx)
                        {
                            oTranscation.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblTxnRedeem + " " + oSalePos.aoTPSTSalHD[0].FTXshDocNo + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                            return false;
                        }
                    }
                }
                //++++++++++++++++++
                oTranscation.Commit();

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                // *Arm 62-12-24 Update FTXshStaPrcStk กรณีส่งซ้ำ
                oSql.AppendLine("   UPDATE THDTMP SET ");
                oSql.AppendLine("   THDTMP.FTXshStaPrcStk = THD.FTXshStaPrcStk,");
                oSql.AppendLine("   THDTMP.FTXshDocVatFull = THD.FTXshDocVatFull"); //*Arm 63-05-26
                //oSql.AppendLine("   FROM TPSTSalHDTmp THDTMP WITH(ROWLOCK)");
                oSql.AppendLine("   FROM " + tTblSalHD + " THDTMP WITH(ROWLOCK)"); // *Arm 63-05-30
                oSql.AppendLine("   INNER JOIN TPSTSalHD THD  WITH(NOLOCK) ON THDTMP.FTBchCode = THD.FTBchCode AND THDTMP.FTXshDocNo = THD.FTXshDocNo");
                oSql.AppendLine("   WHERE THDTMP.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND THDTMP.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "' "); //*Arm 63-05-30

                oSql.AppendLine("   UPDATE TDTTMP SET ");
                oSql.AppendLine("   TDTTMP.FTXsdStaPrcStk = TDT.FTXsdStaPrcStk");
                //oSql.AppendLine("   FROM TPSTSalDTTmp TDTTMP WITH(ROWLOCK)");
                oSql.AppendLine("   FROM " + tTblSalDT + " TDTTMP WITH(ROWLOCK)"); //*Arm 63-05-30
                oSql.AppendLine("   INNER JOIN TPSTSalDT TDT WITH(NOLOCK) ON TDTTMP.FTBchCode = TDT.FTBchCode AND TDTTMP.FTXshDocNo = TDT.FTXshDocNo");
                oSql.AppendLine("   WHERE TDTTMP.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND TDTTMP.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                //--------------------------------------------

                oSql.AppendLine("   DELETE HD ");
                oSql.AppendLine("   FROM TPSTSalHD HD WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine("   WHERE HD.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND HD.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalHD");
                //oSql.AppendLine("   SELECT * FROM TPSTSalHDTmp HDTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalHD + " HDTmp WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDCst ");
                oSql.AppendLine("   FROM TPSTSalHDCst HDCst WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalHDCstTmp TMPCst WITH(NOLOCK) ON HDCst.FTBchCode = TMPCst.FTBchCode AND HDCst.FTXshDocNo = TMPCst.FTXshDocNo");
                oSql.AppendLine("   WHERE HDCst.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND HDCst.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalHDCst");
                //oSql.AppendLine("   SELECT * FROM TPSTSalHDCstTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalHDCst + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDDis ");
                oSql.AppendLine("   FROM TPSTSalHDDis HDDis WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalHDDisTmp TMPHDDis WITH(NOLOCK) ON HDDis.FTBchCode = TMPHDDis.FTBchCode AND HDDis.FTXshDocNo = TMPHDDis.FTXshDocNo");
                oSql.AppendLine("   WHERE HDDis.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND HDDis.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalHDDis");
                //oSql.AppendLine("   SELECT * FROM TPSTSalHDDisTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalHDDis + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TPSTSalDT DT WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo");
                oSql.AppendLine("   WHERE DT.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND DT.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalDT");
                //oSql.AppendLine("   SELECT * FROM TPSTSalDTTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalDT + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTDis ");
                oSql.AppendLine("   FROM TPSTSalDTDis DTDis WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalDTDisTmp TMPDTDis WITH(NOLOCK) ON DTDis.FTBchCode = TMPDTDis.FTBchCode AND DTDis.FTXshDocNo = TMPDTDis.FTXshDocNo");
                oSql.AppendLine("   WHERE DTDis.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND DTDis.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalDTDis");
                //oSql.AppendLine("   SELECT * FROM TPSTSalDTDisTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalDTDis + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                //oSql.AppendLine("   DELETE DTPmt ");
                //oSql.AppendLine("   FROM TPSTSalDTPmt DTPmt WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalDTPmtTmp TMPDTPmt WITH(NOLOCK) ON DTPmt.FTBchCode = TMPDTPmt.FTBchCode AND DTPmt.FTXshDocNo = TMPDTPmt.FTXshDocNo");
                //oSql.AppendLine();
                //oSql.AppendLine("   INSERT INTO TPSTSalDTPmt");
                //oSql.AppendLine("   SELECT * FROM TPSTSalDTPmtTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE RC ");
                oSql.AppendLine("   FROM TPSTSalRC RC WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalRCTmp TMPRC WITH(NOLOCK) ON RC.FTBchCode = TMPRC.FTBchCode AND RC.FTXshDocNo = TMPRC.FTXshDocNo");
                oSql.AppendLine("   WHERE RC.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND RC.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalRC");
                //oSql.AppendLine("   SELECT * FROM TPSTSalRCTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalRC + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                //*Arm 63-03-13
                oSql.AppendLine("   DELETE RD ");
                oSql.AppendLine("   FROM TPSTSalRD RD WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalRDTmp TMPRD WITH(NOLOCK) ON RD.FTBchCode = TMPRD.FTBchCode AND RD.FTXshDocNo = TMPRD.FTXshDocNo AND RD.FNXrdSeqNo = TMPRD.FNXrdSeqNo");
                oSql.AppendLine("   WHERE RD.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND RD.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalRD");
                //oSql.AppendLine("   SELECT * FROM TPSTSalRDTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalRD + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                //++++++++++++++
                //*Arm 63-03-26 SalePD
                oSql.AppendLine("   DELETE PD ");
                oSql.AppendLine("   FROM TPSTSalPD PD WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTSalPDTmp TMPPD WITH(NOLOCK) ON PD.FTBchCode = TMPPD.FTBchCode AND PD.FTXshDocNo = TMPPD.FTXshDocNo AND PD.FNXsdSeqNo = TMPPD.FNXsdSeqNo");
                //oSql.AppendLine("   AND PD.FTPmhDocNo = TMPPD.FTPmhDocNo AND PD.FTPmdGrpName = TMPPD.FTPmdGrpName ");
                oSql.AppendLine("   WHERE PD.FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND PD.FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTSalPD");
                //oSql.AppendLine("   SELECT * FROM TPSTSalPDTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblSalPD + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine("   WHERE FTBchCode = '" + oSalePos.aoTPSTSalHD[0].FTBchCode + "' AND FTXshDocNo = '" + oSalePos.aoTPSTSalHD[0].FTXshDocNo + "'"); //*Arm 63-05-30
                oSql.AppendLine();
                //++++++++++++++
                //*Arm 63-03-26 TxnSale
                oSql.AppendLine("   DELETE TSAL ");
                oSql.AppendLine("   FROM TCNTMemTxnSale TSAL WITH(ROWLOCK)");
                //*Net 63-06-06 join จากตาราง Tmp
                //oSql.AppendLine("   INNER JOIN TCNTMemTxnSaleTmp TMPTSAL WITH(NOLOCK) ON TSAL.FTCgpCode = TMPTSAL.FTCgpCode AND TSAL.FTMemCode = TMPTSAL.FTMemCode");
                oSql.AppendLine("   INNER JOIN " + tTblTxnSale + " TMPTSAL WITH(NOLOCK) ON TSAL.FTCgpCode = TMPTSAL.FTCgpCode AND TSAL.FTMemCode = TMPTSAL.FTMemCode");
                oSql.AppendLine("   AND TSAL.FTTxnRefDoc = TMPTSAL.FTTxnRefDoc AND TSAL.FTTxnRefInt = TMPTSAL.FTTxnRefInt AND TSAL.FTTxnRefSpl = TMPTSAL.FTTxnRefSpl");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TCNTMemTxnSale");
                //oSql.AppendLine("   SELECT * FROM TCNTMemTxnSaleTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblTxnSale + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine();

                oSql.AppendLine("   DELETE TRD ");
                oSql.AppendLine("   FROM TCNTMemTxnRedeem TRD WITH(ROWLOCK)");
                //*Net 63-06-06 join จากตาราง Tmp
                //oSql.AppendLine("   INNER JOIN TCNTMemTxnRedeemTmp TMPTRD WITH(NOLOCK) ON TRD.FTCgpCode = TMPTRD.FTCgpCode AND TRD.FTMemCode = TMPTRD.FTMemCode");
                oSql.AppendLine("   INNER JOIN " + tTblTxnRedeem + " TMPTRD WITH(NOLOCK) ON TRD.FTCgpCode = TMPTRD.FTCgpCode AND TRD.FTMemCode = TMPTRD.FTMemCode");
                oSql.AppendLine("   AND TRD.FTRedRefDoc = TMPTRD.FTRedRefDoc AND TRD.FTRedRefInt = TMPTRD.FTRedRefInt ");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TCNTMemTxnRedeem");
                //oSql.AppendLine("   SELECT * FROM TCNTMemTxnRedeemTmp WITH(NOLOCK) ");
                oSql.AppendLine("   SELECT * FROM " + tTblTxnRedeem + " WITH(NOLOCK) "); //*Arm 63-05-30
                oSql.AppendLine();
                //++++++++++++++

                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                //*Arm 63-05-30 Drop Temp
                oSql.Clear();
                oSql.AppendLine("DROP TABLE " + tTblSalHD + "");
                oSql.AppendLine("DROP TABLE " + tTblSalHDCst + "");
                oSql.AppendLine("DROP TABLE " + tTblSalHDDis + "");
                oSql.AppendLine("DROP TABLE " + tTblSalDT + "");
                oSql.AppendLine("DROP TABLE " + tTblSalDTDis + "");
                oSql.AppendLine("DROP TABLE " + tTblSalRC + "");
                oSql.AppendLine("DROP TABLE " + tTblSalRD + "");
                oSql.AppendLine("DROP TABLE " + tTblSalPD + "");
                oSql.AppendLine("DROP TABLE " + tTblTxnSale + "");
                oSql.AppendLine("DROP TABLE " + tTblTxnRedeem + "");
                oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //+++++++++++++


                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadSale");
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
                aoRC = null;
                oSql = null;
                oSalePos = null;
                oTranscation = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }
        }

        public bool C_PRCbUploadTax(string ptQueueID, cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDataReader<cmlTPSTTaxHD> aoHD;
            cDataReader<cmlTPSTTaxHDCst> aoHDCst;
            cDataReader<cmlTPSTTaxHDDis> aoHDDis;
            cDataReader<cmlTPSTTaxDT> aoDT;
            cDataReader<cmlTPSTTaxDTDis> aoDTDis;
            cDataReader<cmlTPSTTaxDTPmt> aoDTPmt;
            cDataReader<cmlTPSTTaxRC> aoRC;
            cDataReader<cmlTCNMTaxAddress> aoADDR;  //*Arm 62-10-09  - Upload TaxAddress
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlTPSTTax oTax;
            SqlTransaction oTranscation;
            SqlConnection oConn;
            cSP oSP = new cSP();

            string tQueueID = "";
            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oTax = JsonConvert.DeserializeObject<cmlTPSTTax>(poData.ptData);

                tQueueID = ptQueueID.Replace("-", "");

                //*Arm 63-05-30
                string tTblTaxHD = "TSTHD" + tQueueID;
                string tTblTaxHDCst = "TSTHDCst" + tQueueID;
                string tTblTaxHDDis = "TSTHDDis" + tQueueID;
                string tTblTaxDT = "TSTDT" + tQueueID;
                string tTblTaxDTDis = "TSTDTDis" + tQueueID;
                string tTblTaxDTPmt = "TSTDTPmt" + tQueueID;
                string tTblTaxRC = "TSTRC" + tQueueID;
                string tTblTaxADDR = "TSTAddr" + tQueueID;
                //+++++++++++

                //Create Temp
                #region Create Temp
                
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxHD", tTblTaxHD, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxHDCst", tTblTaxHDCst, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxHDDis", tTblTaxHDDis, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxDT", tTblTaxDT, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxDTDis", tTblTaxDTDis, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxDTPmt", tTblTaxDTPmt, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTTaxRC", tTblTaxRC, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TCNMTaxAddress_L", tTblTaxADDR, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);

                //oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion Create Temp

                oConn = new SqlConnection(cVB.tVB_ConnStr);
                oConn.Open();

                oTranscation = oConn.BeginTransaction();

                //insert to DB
                if (oTax.aoTPSTTaxHD != null)
                {
                    aoHD = new cDataReader<cmlTPSTTaxHD>(oTax.aoTPSTTaxHD);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxHD}";

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

                if (oTax.aoTPSTTaxHDCst != null)
                {
                    aoHDCst = new cDataReader<cmlTPSTTaxHDCst>(oTax.aoTPSTTaxHDCst);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDCst.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDCstTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxHDCst}";

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

                if (oTax.aoTPSTTaxHDDis != null)
                {
                    aoHDDis = new cDataReader<cmlTPSTTaxHDDis>(oTax.aoTPSTTaxHDDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoHDDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDDisTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxHDDis}";

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

                if (oTax.aoTPSTTaxDT != null)
                {
                    aoDT = new cDataReader<cmlTPSTTaxDT>(oTax.aoTPSTTaxDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxDT}";

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

                if (oTax.aoTPSTTaxDTDis != null)
                {
                    aoDTDis = new cDataReader<cmlTPSTTaxDTDis>(oTax.aoTPSTTaxDTDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTDisTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxDTDis}";

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

                if (oTax.aoTPSTTaxDTPmt != null)
                {
                    aoDTPmt = new cDataReader<cmlTPSTTaxDTPmt>(oTax.aoTPSTTaxDTPmt);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTPmt.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTPmtTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxDTPmt}";

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

                if (oTax.aoTPSTTaxRC != null)
                {
                    aoRC = new cDataReader<cmlTPSTTaxRC>(oTax.aoTPSTTaxRC);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoRC.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTTaxRCTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxRC}";

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

                //*Arm 62-10-09  - Upload TaxAddress
                if (oTax.aoTCNMTaxAddress != null)
                {
                    aoADDR = new cDataReader<cmlTCNMTaxAddress>(oTax.aoTCNMTaxAddress);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoADDR.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TCNMTaxAddress_LTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblTaxADDR}";

                        try
                        {
                            oBulkCopy.WriteToServer(aoADDR);
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

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE HD ");
                oSql.AppendLine("   FROM TPSTTaxHD HD WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTTaxHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxHD} TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxHD");
                //oSql.AppendLine("   SELECT * FROM TPSTTaxHDTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxHD} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDCst ");
                oSql.AppendLine("   FROM TPSTTaxHDCst HDCst WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTTaxHDCstTmp TMPCst WITH(NOLOCK) ON HDCst.FTBchCode = TMPCst.FTBchCode AND HDCst.FTXshDocNo = TMPCst.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxHDCst} TMPCst WITH(NOLOCK) ON HDCst.FTBchCode = TMPCst.FTBchCode AND HDCst.FTXshDocNo = TMPCst.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxHDCst");
                //oSql.AppendLine("   SELECT * FROM TPSTTaxHDCstTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxHDCst} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE HDDis ");
                oSql.AppendLine("   FROM TPSTTaxHDDis HDDis WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTTaxHDDisTmp TMPHDDis WITH(NOLOCK) ON HDDis.FTBchCode = TMPHDDis.FTBchCode AND HDDis.FTXshDocNo = TMPHDDis.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxHDDis} TMPHDDis WITH(NOLOCK) ON HDDis.FTBchCode = TMPHDDis.FTBchCode AND HDDis.FTXshDocNo = TMPHDDis.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxHDDis");
                //oSql.AppendLine("   SELECT * FROM TPSTTaxHDDisTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxHDDis} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TPSTTaxDT DT WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTTaxDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxDT} TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxDT");
                //oSql.AppendLine("   SELECT * FROM TPSTTaxDTTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxDT} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTDis ");
                oSql.AppendLine("   FROM TPSTTaxDTDis DTDis WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTTaxDTDisTmp TMPDTDis WITH(NOLOCK) ON DTDis.FTBchCode = TMPDTDis.FTBchCode AND DTDis.FTXshDocNo = TMPDTDis.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxDTDis} TMPDTDis WITH(NOLOCK) ON DTDis.FTBchCode = TMPDTDis.FTBchCode AND DTDis.FTXshDocNo = TMPDTDis.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxDTDis");
                //oSql.AppendLine("   SELECT * FROM TPSTTaxDTDisTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxDTDis} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTPmt ");
                oSql.AppendLine("   FROM TPSTTaxDTPmt DTPmt WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTTaxDTPmtTmp TMPDTPmt WITH(NOLOCK) ON DTPmt.FTBchCode = TMPDTPmt.FTBchCode AND DTPmt.FTXshDocNo = TMPDTPmt.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxDTPmt} TMPDTPmt WITH(NOLOCK) ON DTPmt.FTBchCode = TMPDTPmt.FTBchCode AND DTPmt.FTXshDocNo = TMPDTPmt.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxDTPmt");
                //oSql.AppendLine("   SELECT * FROM TPSTTaxDTPmtTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxDTPmt} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE RC ");
                oSql.AppendLine("   FROM TPSTTaxRC RC WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTTaxRCTmp TMPRC WITH(NOLOCK) ON RC.FTBchCode = TMPRC.FTBchCode AND RC.FTXshDocNo = TMPRC.FTXshDocNo");
                oSql.AppendLine($"   INNER JOIN {tTblTaxRC} TMPRC WITH(NOLOCK) ON RC.FTBchCode = TMPRC.FTBchCode AND RC.FTXshDocNo = TMPRC.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTTaxRC");
                //oSql.AppendLine("   SELECT * FROM TPSTTaxRCTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblTaxRC} WITH(NOLOCK) ");
                oSql.AppendLine();
                //*Arm 62-10-09  - Upload TaxAddress
                oSql.AppendLine("   DELETE ADDR ");
                oSql.AppendLine("   FROM TCNMTaxAddress_L ADDR WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TCNMTaxAddress_LTmp TMPADDR WITH(NOLOCK) ON ADDR.FTAddTaxNo = TMPADDR.FTAddTaxNo AND ADDR.FNLngID = TMPADDR.FNLngID");
                oSql.AppendLine($"   INNER JOIN {tTblTaxADDR} TMPADDR WITH(NOLOCK) ON ADDR.FTAddTaxNo = TMPADDR.FTAddTaxNo AND ADDR.FNLngID = TMPADDR.FNLngID");
                oSql.AppendLine();
                //oSql.AppendLine("   INSERT INTO TCNMTaxAddress_L");
                //oSql.AppendLine("   SELECT * FROM TCNMTaxAddress_LTmp WITH(NOLOCK) ");
                oSql.AppendLine("   INSERT INTO TCNMTaxAddress_L(FTAddTaxNo,FNLngID,FTAddRefNo,FTAddName,FTAddRmk,FTAddCountry,FTAreCode,FTZneCode,FTAddVersion,FTAddV1No,FTAddV1Soi,FTAddV1Village,FTAddV1Road,FTAddV1SubDist,FTAddV1DstCode,FTAddV1PvnCode,FTAddV1PostCode,FTAddV2Desc1,FTAddV2Desc2,FTAddWebsite,FTAddLongitude,FTAddLatitude,FTAddStaBusiness,FTAddStaHQ,FTAddStaBchCode,FTAddTel,FTAddFax,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");   //*Em 62-12-18
                oSql.AppendLine("   SELECT FTAddTaxNo,FNLngID,FTAddRefNo,FTAddName,FTAddRmk,FTAddCountry,FTAreCode,FTZneCode,FTAddVersion,FTAddV1No,FTAddV1Soi,FTAddV1Village,FTAddV1Road,FTAddV1SubDist,FTAddV1DstCode,FTAddV1PvnCode,FTAddV1PostCode,FTAddV2Desc1,FTAddV2Desc2,FTAddWebsite,FTAddLongitude,FTAddLatitude,FTAddStaBusiness,FTAddStaHQ,FTAddStaBchCode,FTAddTel,FTAddFax,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy ");  //*Em 62-12-18
                //oSql.AppendLine("   FROM TCNMTaxAddress_LTmp WITH(NOLOCK) "); //*Em 62-12-18
                oSql.AppendLine($"   FROM {tTblTaxADDR} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                //*Net 63-06-09 Drop Temp
                oSql.Clear();
                oSql.AppendLine("DROP TABLE " + tTblTaxHD + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxHDCst + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxHDDis + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxDT + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxDTDis + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxDTPmt + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxRC + "");
                oSql.AppendLine("DROP TABLE " + tTblTaxADDR + "");
                oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //+++++++++++++

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadTax");
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
                aoRC = null;
                aoADDR = null;
                oSql = null;
                oTax = null;
                oTranscation = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }
        }

        public bool C_PRCbUploadVoid(string ptQueueID, cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cDataReader<cmlTPSTVoidDT> aoDT;
            cDataReader<cmlTPSTVoidDTDis> aoDTDis;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlTPSTVoid oVoid;
            SqlTransaction oTranscation;
            SqlConnection oConn;

            string tQueueID = "";
            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oVoid = JsonConvert.DeserializeObject<cmlTPSTVoid>(poData.ptData);

                tQueueID = ptQueueID.Replace("-", "");

                string tTblVoidDT = "TSVDT" + tQueueID;
                string tTblVoidDTDis = "TSVDTDis" + tQueueID;

                //Create Temp

                oDB.C_PRCxCreateDatabaseTmp("TPSTVoidDT", tTblVoidDT, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);
                oDB.C_PRCxCreateDatabaseTmp("TPSTVoidDTDis", tTblVoidDTDis, cVB.tVB_ConnStr, (int)poShopDB.nCommandTimeOut);

                //oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                oConn = new SqlConnection(cVB.tVB_ConnStr);
                oConn.Open();

                oTranscation = oConn.BeginTransaction();

                //insert to DB
                if (oVoid.aoTPSTVoidDT != null)
                {
                    aoDT = new cDataReader<cmlTPSTVoidDT>(oVoid.aoTPSTVoidDT);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTVoidDTTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblVoidDT}";

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

                if (oVoid.aoTPSTVoidDTDis != null)
                {
                    aoDTDis = new cDataReader<cmlTPSTVoidDTDis>(oVoid.aoTPSTVoidDTDis);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                    {
                        foreach (string tColName in aoDTDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        //oBulkCopy.DestinationTableName = "dbo.TPSTVoidDTDisTmp";
                        oBulkCopy.DestinationTableName = $"dbo.{tTblVoidDTDis}";

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
                oTranscation.Commit();

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TPSTVoidDT DT WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTVoidDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FNVidNo = TMPDT.FNVidNo AND DT.FNXidSeqNo = TMPDT.FNXidSeqNo");
                oSql.AppendLine($"   INNER JOIN {tTblVoidDT} TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FNVidNo = TMPDT.FNVidNo AND DT.FNXidSeqNo = TMPDT.FNXidSeqNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTVoidDT");
                //oSql.AppendLine("   SELECT * FROM TPSTVoidDTTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblVoidDT} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTD ");
                oSql.AppendLine("   FROM TPSTVoidDTDis DTD WITH(ROWLOCK)");
                //oSql.AppendLine("   INNER JOIN TPSTVoidDTDisTmp TMPDTD WITH(NOLOCK) ON DTD.FTBchCode = TMPDTD.FTBchCode AND DTD.FNVidNo = TMPDTD.FNVidNo AND DTD.FNXidSeqNo = TMPDTD.FNXidSeqNo AND DTD.FDXddDateIns = TMPDTD.FDXddDateIns");
                oSql.AppendLine($"   INNER JOIN {tTblVoidDTDis} TMPDTD WITH(NOLOCK) ON DTD.FTBchCode = TMPDTD.FTBchCode AND DTD.FNVidNo = TMPDTD.FNVidNo AND DTD.FNXidSeqNo = TMPDTD.FNXidSeqNo AND DTD.FDXddDateIns = TMPDTD.FDXddDateIns");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TPSTVoidDTDis");
                //oSql.AppendLine("   SELECT * FROM TPSTVoidDTDisTmp WITH(NOLOCK) ");
                oSql.AppendLine($"   SELECT * FROM {tTblVoidDTDis} WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                //*Net 63-06-09 Drop Temp
                oSql.Clear();
                oSql.AppendLine("DROP TABLE " + tTblVoidDT + "");
                oSql.AppendLine("DROP TABLE " + tTblVoidDTDis + "");
                oDB.C_DATbExecuteNonQuery(cVB.tVB_ConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                //+++++++++++++

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadVoid");
                return false;
            }
            finally
            {
                aoDT = null;
                aoDTDis = null;
                oSql = null;
                oVoid = null;
                oTranscation = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }
        }
    }
}
