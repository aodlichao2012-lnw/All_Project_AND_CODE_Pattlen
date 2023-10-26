using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    class cDiscPolicy
    {
        /// <summary>
        /// function Check ลำดับการให้ส่วนลด ว่าสามารถใช้ส่วนลดชนิดนี้ได้หรือไม่
        /// </summary>
        /// <param name="pnDiscGroup"></param>
        /// <param name="ptDiscCode"></param>
        /// <returns></returns>
        public bool C_CHKbkDiscPolicy(string ptFunc, int pnDiscGroup)
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            int nCnt = 0;
            bool bStaChk = false;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                odtTmp = new DataTable();

                // หา DisCode, DisCodeRef, Status Price
                oSql.AppendLine("SELECT FTDisCode, FTDisCodeRef, FTDisStaPrice FROM TSysDisPolicy WHERE FTDisPosFunc = '" + ptFunc + "'");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    //DisCodeY
                    string tDisCodeY = odtTmp.Rows[0].Field<string>("FTDisCode") == null ? string.Empty : odtTmp.Rows[0].Field<string>("FTDisCode");
                    
                    if (!string.IsNullOrEmpty(tDisCodeY))
                    {
                        // Check Discount Policy
                        oSql.Clear();
                        oSql.AppendLine("SELECT COUNT(DPC.FTDpcStaAlw) AS nCnt FROM (");
                        
                        //หา DiscodeX
                        switch (pnDiscGroup)
                        {
                            case 1: // Item
                                oSql.AppendLine("SELECT TOP 1 FTDisCode FROM " + cSale.tC_TblSalDTDis + " WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXddStaDis = '" + pnDiscGroup + "' AND FNXsdSeqNo = " + cSale.nC_DTSeqNo + "  ORDER BY FDXddDateIns DESC ");
                                break;
                            case 2: // Bill
                                oSql.AppendLine("SELECT TOP 1 FTDisCode FROM " + cSale.tC_TblSalDTDis + " WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXddStaDis = '" + pnDiscGroup + "' ORDER BY FDXddDateIns DESC");
                                break;
                        }
                        oSql.AppendLine(") AS DTDis");
                        oSql.AppendLine("INNER JOIN TPSTDiscPolicy DPC WITH(NOLOCK) ON DTDis.FTDisCode = DPC.FTDpcDisCodeX ");
                        oSql.AppendLine("WHERE DPC.FTDpcDisCodeY = '" + tDisCodeY + "' AND DPC.FTDpcStaAlw = '2' ");

                        nCnt = oDB.C_GEToDataQuery<int>(oSql.ToString());
                    }
                    if (nCnt == 0)
                    {
                        bStaChk = true;
                        cVB.tVB_DisCode = tDisCodeY;
                        cVB.tVB_DisCodeRef = odtTmp.Rows[0].Field<string>("FTDisCodeRef");
                        cVB.tVB_StaPrice = odtTmp.Rows[0].Field<string>("FTDisStaPrice");
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDiscPolicy", "C_CHKbkDiscPolicy : " + oEx.Message.ToString());
            }
            finally
            {
                odtTmp = null;
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return bStaChk;
        }


        /// <summary>
        /// หามูลค่าก่อนลด/ชาจน์ ที่อนุญาตให้ให้ลดชาจน์
        /// </summary>
        /// <param name="pnDiscGroup"></param>
        /// <param name="ptDiscStaPrice">1:Full Net Price, 2:Net Price</param>
        /// <param name="pnSeq">SeqNo ส่งแค่ pnDiscGroup = 1 และ 3 </param>
        /// <returns></returns>
        public decimal C_GETcGetAmtAlwDisc(int pnDiscGroup, string ptDiscStaPrice, int pnSeq = 0)
        {
            StringBuilder oSql;
            cDatabase oDB;

            decimal cAmount = 0;

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                switch (ptDiscStaPrice) //
                {
                    case "1": // 1: Full Net price
                        
                        switch (pnDiscGroup)
                        {
                            case 1:
                            case 3:
                                //Item
                                oSql.AppendLine("SELECT CAST(ISNULL((FCXsdSetPrice * FCXsdQty),0) AS decimal(18, 4)) AS cAmt");
                                break;
                            case 2:
                                // Bill
                                oSql.AppendLine("SELECT CAST(ISNULL(SUM(FCXsdSetPrice * FCXsdQty),0) AS decimal(18, 4)) AS cAmt");
                                break;
                        }

                        break;

                    case "2": //Net Price
                        
                        switch(pnDiscGroup)
                        {
                            case 1:
                                //Item
                                oSql.AppendLine("SELECT ISNULL(FCXsdNet,0) AS cAmt");
                                break;
                            case 2:
                                //Bill
                                oSql.AppendLine("SELECT ISNULL(SUM(FCXsdNetAfHD),0) AS cAmt");
                                break;
                            case 3:
                                //Bill Item
                                oSql.AppendLine("SELECT ISNULL(FCXsdNetAfHD,0) AS cAmt");
                                break;
                        }

                        break;
                }

                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTBchCode ='" + cVB.tVB_BchCode +"' AND FTXshDocNo= '"+ cVB.tVB_DocNo + "' AND  FTXsdStaAlwDis = 1 AND FTXsdStaPdt ='1' ");

                if (pnDiscGroup == 1 || pnDiscGroup == 3)
                {
                    //Item
                    oSql.AppendLine("AND FNXsdSeqNo = '"+ pnSeq + "' ");
                }

                cAmount = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDiscPolicy", "C_GETcGetAmtAlwDisc : " + oEx.Message.ToString());
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return cAmount;
        }

        /// <summary>
        /// มูลค่าสุทธิ
        /// </summary>
        /// <param name="pnDiscGroup">1:Item,2:ท้ายบิล,3:ท้ายบิล By Item</param>
        /// <param name="pnSeq">(FTXsdSeqNo) ถ้าส่ง pnDiscGroup = 1 ให้กำหนด pnSeq มาด้วย </param>
        /// <returns></returns>
        public decimal C_GETcGetAmount(int pnDiscGroup,int pnSeq = 0)
        {
            StringBuilder oSql;
            cDatabase oDB;
            decimal cAmount = 0;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                switch (pnDiscGroup)
                {
                    case 1: // Item
                    case 3:
                        
                        if (pnSeq<=0) //บังคับต้องส่ง SeqNo มาด้วย
                        {
                            return cAmount;
                        }

                        if (pnDiscGroup == 1)
                        {
                            // By Item
                            oSql.AppendLine("SELECT ISNULL((FCXsdNet),0) AS cAmt");
                        }
                        else
                        {
                            // ท้ายบิล By Item
                            oSql.AppendLine("SELECT ISNULL((FCXsdNetAfHD),0) AS cAmt");
                        }
                        oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE FTBchCode ='" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTXsdStaPdt ='1' ");
                        oSql.AppendLine("AND FNXsdSeqNo = '" + pnSeq + "' ");
                        break;

                    case 2: // ท้ายบิล
                        oSql.AppendLine("SELECT ISNULL(SUM(FCXsdNetAfHD),0) AS cAmt");
                        oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH (NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode ='" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTXsdStaPdt ='1' ");
                        break;
                }
                
                cAmount = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cDiscPolicy", "C_GETcGetAmount : " + oEx.Message.ToString());
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return cAmount;
        }
    }
}
