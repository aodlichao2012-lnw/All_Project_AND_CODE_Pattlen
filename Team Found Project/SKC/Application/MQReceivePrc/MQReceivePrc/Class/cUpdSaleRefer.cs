using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Database;
using MQReceivePrc.Models.DatabaseTmp;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.SaleRefer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cUpdSaleRefer
    {
        public bool C_UPDbUpddateQtyRefer(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            StringBuilder oSql;
            cDatabase oDB;
            SqlConnection oConn;
            SqlTransaction oTranscation;
            cmlReqUpdSaleRefer oUpdSaleRefer;
            List<cmlTSRF> aoRF;
            cDataReader<cmlTSRF> oRF;
            string tConnStr = "";
            string tTblRefund = "TSRF" + cVB.tVB_BchCode ;
            int nRowEff;
            int nQtyLef = 0;
            try
            {
                if (poData == null) return false;
                
                ptErrMsg = "";
                oUpdSaleRefer = new cmlReqUpdSaleRefer();
                oUpdSaleRefer = JsonConvert.DeserializeObject<cmlReqUpdSaleRefer>(poData.ptData);
                oSql = new StringBuilder();
                oDB = new cDatabase();

                if (oUpdSaleRefer.pnSaleType == 9 && oUpdSaleRefer.pnOptionRfn == 3) // Check ถ้าเป็นประเภทการคืน และ Option การคืน = 3:คืนได้จนกว่าจะครบจำนวนที่ซื้อ
                {
                    if (oUpdSaleRefer.aoRfn != null && oUpdSaleRefer.aoRfn.Count > 0)
                    {
                        // Create Temp
                        oSql.AppendLine("IF OBJECT_ID('" + tTblRefund + "', 'U') IS NULL BEGIN");
                        oSql.AppendLine("   CREATE TABLE [dbo].[" + tTblRefund + "](");
                        oSql.AppendLine("   [FNXsdSeqNo] [int] NOT NULL,");
                        oSql.AppendLine("   [FNXsdSeqNoOld] [int] NOT NULL,");
                        oSql.AppendLine("   [FCXsdQty] [numeric](18, 2) NULL,");
                        oSql.AppendLine("   [FCXsdQtyRfn] [numeric](18, 2) NULL");
                        oSql.AppendLine("   ) ON [PRIMARY]");
                        oSql.AppendLine("END");
                        oSql.AppendLine("TRUNCATE TABLE " + tTblRefund);
                        oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEff);

                        oConn = new SqlConnection(poData.ptConnStr);
                        oConn.Open();

                        oTranscation = oConn.BeginTransaction();

                        aoRF = C_PRCaListRefund(oUpdSaleRefer.aoRfn);
                        oRF = new cDataReader<cmlTSRF>(aoRF);
                        using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTranscation))
                        {
                            foreach (string tColName in oRF.ColumnNames)
                            {
                                oBulkCopy.ColumnMappings.Add(tColName, tColName);
                            }

                            oBulkCopy.BatchSize = 100;
                            oBulkCopy.DestinationTableName = "dbo." + tTblRefund;
                            try
                            {
                                oBulkCopy.WriteToServer(oRF);
                            }
                            catch (Exception oEx)
                            {

                                oTranscation.Rollback();
                                ptErrMsg = oEx.Message.ToString();
                                cFunction.C_LOGxKeepLogErr("BulkCopy/" + tTblRefund + " : " + oEx.Message.ToString(), "C_PRCbUploadSale"); //*Arm 63-05-30
                                return false;
                            }
                        }
                        oTranscation.Commit();
                    }

                    //อัพเดตจำนวน ที่ DT ของเอกสารที่อ้างอิง
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TPSTSalDT WITH(ROWLOCK)");
                    //oSql.AppendLine("SET FCXsdQtyLef = DT.FCXsdQtyLef - Rfn.FCXsdQty, ");
                    //oSql.AppendLine("FCXsdQtyRfn = DT.FCXsdQtyRfn + Rfn.FCXsdQty ");
                    oSql.AppendLine("SET FCXsdQtyLef = DT.FCXsdQtyLef - Rfn.FCXsdQtyRfn, ");
                    oSql.AppendLine("FCXsdQtyRfn = DT.FCXsdQtyRfn + Rfn.FCXsdQtyRfn ");
                    oSql.AppendLine("FROM TPSTSalDT DT ");
                    oSql.AppendLine("INNER JOIN " + tTblRefund + " Rfn WITH(NOLOCK) ON DT.FNXsdSeqNo = Rfn.FNXsdSeqNoOld");
                    oSql.AppendLine("WHERE DT.FTBchCode = '" + oUpdSaleRefer.ptBchCode + "'");
                    oSql.AppendLine("AND DT.FTXshDocNo = '" + oUpdSaleRefer.ptRefDocNo + "'");
                    oSql.AppendLine("AND DT.FTXsdStaPdt = '1' ");
                    oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEff);

                    //หา Qty คงเหลือ
                    oSql.Clear();
                    oSql.AppendLine("SELECT SUM(ISNULL(FCXsdQtyLef,0)) AS FCXsdQtyLef FROM TPSTSalDT WITH(NOLOCK) ");
                    oSql.AppendLine("WHERE FTBchCode = '" + oUpdSaleRefer.ptBchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + oUpdSaleRefer.ptRefDocNo + "'");
                    nQtyLef = oDB.C_DAToExecuteQuery<int>(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut);
                }

                // อัพเดต เอกสาร HD ที่ถูกอ้างอิง 
                oSql.Clear();
                oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK) SET ");
                if (oUpdSaleRefer.pnSaleType == 9) //Check ประเภทการขายที่ส่งมา
                {
                    //ถ้าเป็นการคืน
                    if (oUpdSaleRefer.pnOptionRfn == 3) // Check Option = 3
                    {
                        if (nQtyLef > 0) // Check Qty 
                        {
                            // Qty ยังเหลืออยู่ => FNXshStaRef = 1
                            oSql.AppendLine(" FNXshStaRef = '1',");
                        }
                        else  
                        {
                            // Qty คืนหมดแล้ว => FNXshStaRef = 2
                            oSql.AppendLine(" FNXshStaRef = '2',");
                        }
                    }
                    oSql.AppendLine("FTXshStaRefund = '2',");
                }
                else
                {
                    // เป็นการขาย
                    oSql.AppendLine(" FNXshStaRef = '2',");
                }
                oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                oSql.AppendLine("FTLastUpdBy = 'MQReceivePrc'");
                oSql.AppendLine("WHERE FTBchCode = '" + oUpdSaleRefer.ptBchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + oUpdSaleRefer.ptRefDocNo + "'");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowEff);
                
                return true;
            }
            catch(Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_UPDbUpddateQtyRefer");
                return false;
            }
        }

        public List<cmlTSRF> C_PRCaListRefund(List<cmlTblRefund> paoRF)
        {
            List<cmlTSRF> aoData = new List<cmlTSRF>();
            try
            {
                aoData = paoRF.Select(oItem => new cmlTSRF()
                {
                    FNXsdSeqNo = oItem.pnSeqNo,
                    FNXsdSeqNoOld = oItem.pnSeqNoOld,
                    FCXsdQty = oItem.pcQty,
                    FCXsdQtyRfn = oItem.pcQtyRfn
                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListTxnRedeem : " + oEx.Message); }
            finally
            {
                paoRF = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }


    }
}
