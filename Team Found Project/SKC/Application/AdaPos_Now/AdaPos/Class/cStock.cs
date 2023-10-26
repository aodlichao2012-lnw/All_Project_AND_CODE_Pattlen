using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Required.Stock;
using AdaPos.Models.Webservice.Respond.KADS;
using AdaPos.Models.Webservice.Respond.KADS.CheckStock;
using AdaPos.Models.Webservice.Respond.Stock;
using AdaPos.Popup.wSale;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    //public static class cStock
    public class cStock
    {
        private cmlResKADS<cmlResInfoResultStock> oC_ResItem;
        private DataTable oC_dtTmp;
        private ResourceManager oC_Resource;

        public cStock()
        {

        }

        public void C_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oC_Resource = new ResourceManager(typeof(resSale_TH));
                        break;

                    default:    // EN
                        oC_Resource = new ResourceManager(typeof(resSale_EN));

                        break;
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("cStock", "C_SETxText : " + oEx.Message.ToString());
            }
        }

        /// <summary>
        /// ตรวจสอบจำนวนสินค้า กรณีเปลี่ยนจำนวนสินค้า
        /// </summary>
        /// <returns></returns>
        public bool C_CHKbUpdQtyCheckStock()
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlPdtDetail> aoPdt;
            DataTable odtTmp;
            cSP oSP;
            string tPdtCode = "";
            string tFunc = "";
            string tMsgErr = "";
            decimal cSumQty = 0;
            
            try
            {
                new cLog().C_WRTxLog("cStock", "C_CHKbUpdQtyCheckStock : Start ", cVB.bVB_AlwPrnLog);
                if (C_CHKbverify() == false) return false;
                if (cSale.cC_DTQty <= 0) return false;

                oSql = new StringBuilder();
                oDB = new cDatabase();
                odtTmp = new DataTable();
                oSP = new cSP();
                C_SETxText();

                //หาจำนวนทั้งหมด จาก Product Code ที่เลือก และไม่นับรวม Seq ที่เลือก
                oSql.AppendLine("SELECT DT.FTPdtCode, SUM(ISNULL(DT.FCXsdQty, 0)) AS FCXsdQty ");
                oSql.AppendLine("FROM("); // หา PdtCode ของ SeqNo ที่เลือก
                oSql.AppendLine("    SELECT FTPdtCode, FTXsdBarCode ");
                oSql.AppendLine("    FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) ");
                oSql.AppendLine("    WHERE FTBchCode = '"+cVB.tVB_BchCode+"' AND FTXshDocNo = '"+cVB.tVB_DocNo+"' AND FNXsdSeqNo = '" + cSale.nC_DTSeqNo + "' ");
                oSql.AppendLine(") DTPdt ");
                oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalDT + " DT WITH(NOLOCK) ON DTPdt.FTPdtCode = DT.FTPdtCode AND DTPdt.FTXsdBarCode = DT.FTXsdBarCode ");
                oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND DT.FTXsdStaPdt <> '4'  AND DT.FNXsdSeqNo <> '" + cSale.nC_DTSeqNo + "' ");
                oSql.AppendLine("GROUP BY DT.FTBchCode,DT.FTXshDocNo,DT.FTPdtCode,DT.FTXsdBarCode ");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                //นำค่า Product Code จำนวนสินค้นของ PdtCode นี้ทั้งหมดที่เลือกไปแล้ว
                if(odtTmp != null && odtTmp.Rows.Count >0)
                {
                    tPdtCode = odtTmp.Rows[0].Field<string>("FTPdtCode"); // Product Code
                    cSumQty = odtTmp.Rows[0].Field<decimal>("FCXsdQty");  // จำนวนที่ถูกใช้แล้ว
                }
                else
                {
                    // ถ้าไม่มีสินค้า PdtCode นี้ที่ Seq ที่ไม่ได้เลือก  ให้หา Product Code จาก Seq ที่เลือก
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTPdtCode");
                    oSql.AppendLine("FROM "+cSale.tC_TblSalDT+" WITH(NOLOCK) ");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXsdSeqNo = '" + cSale.nC_DTSeqNo + "' ");
                    odtTmp = new DataTable();
                    odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        tPdtCode = odtTmp.Rows[0].Field<string>("FTPdtCode"); // Product Code
                        cSumQty = 0;
                    }
                }

                //tFunc = "/sap/opu/odata/SAP/ZDP_GWSRV008_SRV/StockSet(MatNo='" + tPdtCode + "',PlantCode='" + cVB.tVB_BchRefID + "',Sloc='" + cVB.tVB_WahRefNo + "')";
                tFunc = "(MatNo='" + tPdtCode + "',PlantCode='" + cVB.tVB_BchRefID + "',Sloc='" + cVB.tVB_WahRefNo + "')";
                // Call APIKADS ใช้ Model รับค่า
                if (C_CHKbCheckStockOnline(tFunc, 1) == true)
                {
                    if (oC_ResItem != null)
                    {
                        if ((decimal)(oC_ResItem.d.MatQty - cSumQty) >= cSale.cC_DTQty) // เช็คจำนวนคงเหลือ กับจำนวนที่ต้องการกำหนดใหม่ 
                        {
                            return true;
                        }
                        else
                        {
                            tMsgErr = oC_Resource.GetString("tMsgPdtQtyDef") + System.Environment.NewLine;
                            tMsgErr += oC_Resource.GetString("tMsgPdtQtyBalance") + " : " + oSP.SP_SETtDecShwSve(1, (decimal)(oC_ResItem.d.MatQty - cSumQty), 0) + " " + oC_ResItem.d.MatUnit;
                            oSP.SP_SHWxMsg(tMsgErr, 3);
                            return false;
                        }
                    }
                    else
                    {
                        oSP.SP_SHWxMsg(oC_Resource.GetString("tMsgStockNotFoundPdt"), 3);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cStock", "C_CHKbUpdQtyCheckStock : " + oEx.Message.ToString());
                return false;
            }
            finally
            {
                oC_dtTmp = null;
                oC_ResItem = null;
                oC_Resource = null;
                odtTmp = null;
                oSP = null;
                tMsgErr = null;
                new cLog().C_WRTxLog("cStock", "C_CHKbUpdQtyCheckStock : End ", cVB.bVB_AlwPrnLog);
                //new cSP().SP_CLExMemory();
            }
        }
        
        /// <summary>
        /// ตรวจสอบ Stock Online ตอนสแกนสินค้า หรือเลือกสินค้า
        /// </summary>
        /// <param name="poPdtOrder"></param>
        /// <returns></returns>
        public void C_CHKbScanPdtCheckStock(cmlPdtOrder poPdtOrder)
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlPdtDetail> aoPdt;
            cSP oSP;
            string tFunc = "";
            bool bStaChk = false;
            string tMsgErr = "";
            decimal cSumQty = 0;
            int nfind = 0;
            try
            {
                new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : Start ", cVB.bVB_AlwPrnLog);
                if (C_CHKbverify() == false) return; // ตรวจสอบตัวแปล
                if (poPdtOrder == null) return;      // ตรวจสอบ object parameter

                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSP = new cSP();

                C_SETxText();
                
                // หาจำนวนทั้้งหมดของสินค้า Product Code นี้ที่มีการ Scan ไปแล้วในบิลนี้ เพื่อนำไปหักกับ จำนวนคงเหลือใน Stock ที่ส่งมา
                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(SUM(ISNULL(FCXsdQty,0)),0) AS FCXsdQty FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTXsdStaPdt <> '4' ");
                oSql.AppendLine("AND FTPdtCode = '" + poPdtOrder.tPdtCode + "' AND FTXsdBarCode = '" + poPdtOrder.tBarcode + "'");
                cSumQty = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                // Check สินค้ามี  FTPgpChain หรือไม่ ?
                // ถ้ามี FTPgpChain KADS จะส่งรายการสินค้าทดแทนกลับมาด้วย)
                // ถ้าไม่มี FTPgpChain KADS จะส่งแค่สินค้านั้นมาอย่างเดียว

                if (!string.IsNullOrEmpty(poPdtOrder.tPgpChain))
                {
                    // #Check Stock กรณีมี FTPgpChain
                    //tFunc = "/sap/opu/odata/SAP/ZDP_GWSRV008_SRV/StockSet?$filter=PicNum eq '" + poPdtOrder.tPgpChain + "' and Sloc eq '" + cVB.tVB_WahRefNo + "'";
                    tFunc = "?$filter=PicNum eq '" + poPdtOrder.tPgpChain + "' and Sloc eq '" + cVB.tVB_WahRefNo + "'";

                    if (C_CHKbCheckStockOnline(tFunc,2) == true) // Call API Retrun DataTable
                    {
                        if(oC_dtTmp != null && oC_dtTmp.Rows.Count > 0)
                        {
                            // ตรวจสอบจำนวนสินค้าตัวที่เลือกก่อน
                            foreach (DataRow oRow in oC_dtTmp.Rows) // Loop หารายการสินค้าที่ต้องการ ข้อมูลที่ KADS ส่งมา
                            {
                                if (oRow.Field<string>("MatNo") == poPdtOrder.tPdtCode) // ถ้าเจอสินค้าตัวที่ Scan
                                {
                                    nfind = 1; // 0:ไม่เจอเจอสินค้า 1:เจอสินค้า
                                    decimal cMatQty = Convert.ToDecimal(oRow.Field<double>("MatQty")) - cSumQty; // จำนวนคงเหลือใน Stock ที่ KADS ส่งมา - จำนวนที่ Pos Scan ไแล้วก่อนหน้า
                                    decimal cQty = (decimal)poPdtOrder.cQty; //จำนวนสินค้าที่ต้องการ

                                    if(cMatQty >= cQty) //Check จำนวนสินค้าคงเหลือ ต้องมากกว่าเท่ากับจำนวนที่ต้องการ
                                    {
                                        new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : W_ADDxPdtToOrder Start.", cVB.bVB_AlwPrnLog);
                                        poPdtOrder.tRemark = oRow.Field<string>("BinLoc"); //*Arm 63-08-08 Location Product
                                        cVB.oVB_Sale.W_ADDxPdtToOrder(poPdtOrder); // เพิ่มสินค้าลง DT และแสดงใน DataGridView
                                        new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : W_ADDxPdtToOrder Start.", cVB.bVB_AlwPrnLog);
                                        bStaChk = true; // สถานะตรวจสอบจำนวนคงเหลือพอ
                                        break;
                                    }
                                    else
                                    {
                                        // จำนวนสินค้าคงเหลือ น้อยกว่าจำนวนที่ต้องการ แสดง Error Msg สินค้าคงเหลือไม่พอ และจำนวนคงเหลือ
                                        tMsgErr = oC_Resource.GetString("tMsgPdtQtyDef");
                                        tMsgErr += System.Environment.NewLine;
                                        tMsgErr += oC_Resource.GetString("tMsgPdtQtyBalance") + " " + oSP.SP_SETtDecShwSve(1, cMatQty, 0) + " " + oRow.Field<string>("MatUnit");
                                        oSP.SP_SHWxMsg(tMsgErr, 3);
                                        new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : (Scan) "+ tMsgErr, cVB.bVB_AlwPrnLog);
                                    }
                                    break;
                                }
                            }

                            if(nfind == 0) //ถ้าไม่พบสินค้าที่ Scan ให้แจ้ง Msg ไม่พบสินค้าในคลังสินค้า 
                            {
                                new cSP().SP_SHWxMsg(oC_Resource.GetString("tMsgStockNotFoundPdt"), 3);
                            }

                            // สินค้าทดแทน
                            if (bStaChk == false) // ถ้าสถานะตรวจสอบสินค้าที่ Scan มีจำนวนคงเหลือไม่พอกับที่ต้องการ ให้แสดงสินค้าทดแทนมาให้เลือกถ้ามี
                            {
                                if (oC_dtTmp.Rows.Count > 0) //ตรวจสอบสินค้าทดแทนที่มีจำนวนเพียงพอกับที่ต้องการ
                                {
                                    aoPdt = new List<cmlPdtDetail>();
                                    oSql.Clear();
                                    oSql.AppendLine("SELECT DISTINCT Tmp.MatNo AS tPdtCode,");
                                    oSql.AppendLine("PDT.FTBarCode AS tBarcode,");
                                    oSql.AppendLine("PDT.FTPdtName AS tPdtName,");
                                    oSql.AppendLine("PDT.FTPunName AS tUnitName,");
                                    oSql.AppendLine("PDT.FCPdtUnitFact AS cUnitFactor,");
                                    oSql.AppendLine("(CASE WHEN ISNULL(PriCst.FTPplCode,'') <> ''  THEN PriCst.FCPdtPrice ELSE PDT.FCPdtPrice END) AS cPdtPrice,");
                                    oSql.AppendLine("COUNT(*) OVER (PARTITION BY 1) nRowCount,");
                                    oSql.AppendLine("FTPdtPicPath AS tPicPath,");
                                    oSql.AppendLine("FTPdtSaleType AS tSaleType,");
                                    oSql.AppendLine("FTPdtStaAlwDis AS tStaAlwDis,");
                                    oSql.AppendLine("Tmp.BinLoc AS tRemark,"); //*Arm 63-08-08
                                    oSql.AppendLine("Tmp.MatQty - ISNULL((SELECT SUM(ISNULL(FCXsdQty,0)) AS FCXsdQty FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTXsdStaPdt <> '4' ");
                                    oSql.AppendLine("AND FTPdtCode = Tmp.MatNo AND FTXsdBarCode = Tmp.MatNo),0) AS cQty");
                                    //oSql.AppendLine("Tmp.MatQty AS cQty");
                                    oSql.AppendLine("FROM TTmpStockOnline Tmp WITH(NOLOCK)");
                                    oSql.AppendLine("INNER JOIN TPSMPdt PDT WITH(NOLOCK) ON Tmp.MatNo = PDT.FTPdtCode AND Tmp.MatNo = PDT.FTBarCode");
                                    oSql.AppendLine("LEFT JOIN TPSTPdtPrice PriCst WITH(NOLOCK) ON Tmp.MatNo = PriCst.FTPdtCode AND Tmp.MatUnit = PriCst.FTPunCode ");
                                    oSql.AppendLine("AND ISNULL(PriCst.FTPplCode,'') <> '' AND PriCst.FTPriType = '1' AND ISNULL(PriCst.FTPplCode,'') = '" + (string.IsNullOrEmpty(cVB.tVB_PriceGroup) ? "" : cVB.tVB_PriceGroup) + "'");
                                    //oSql.AppendLine("WHERE Tmp.MatQty >= '" + poPdtOrder.cQty + "'");
                                    oSql.AppendLine("WHERE (Tmp.MatQty - ISNULL((SELECT SUM(ISNULL(FCXsdQty,0)) AS FCXsdQty FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTXsdStaPdt <> '4' ");
                                    oSql.AppendLine("AND FTPdtCode = Tmp.MatNo AND FTXsdBarCode = Tmp.MatNo),0)) >= '" + poPdtOrder.cQty + "' ");
                                    aoPdt = oDB.C_GETaDataQuery<cmlPdtDetail>(oSql.ToString());
                                    
                                    if (aoPdt.Count > 0) // ถ้าพบ
                                    {
                                        //แสดงสินค้าทดแทนได้ให้เลือก
                                        new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : Show Interchange ", cVB.bVB_AlwPrnLog);
                                        wSearchPdt oShcPdt = new wSearchPdt("", poPdtOrder.cQty, 3, aoPdt);
                                        if (oShcPdt.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                        {
                                            new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : W_ADDxPdtToOrder Start.", cVB.bVB_AlwPrnLog);
                                            cVB.oVB_Sale.W_ADDxPdtToOrder(cVB.oVB_PdtOrder);  // เพิ่มสินค้าลง DT และแสดงใน DataGridView
                                            new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : W_ADDxPdtToOrder End.", cVB.bVB_AlwPrnLog);
                                        }
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            oSP.SP_SHWxMsg(oC_Resource.GetString("tMsgStockNotFoundPdt"), 3);
                        }
                    }
                }
                else
                {
                    // # Check Stock กรณีไม่มี FTPgpChain
                    //tFunc = "/sap/opu/odata/SAP/ZDP_GWSRV008_SRV/StockSet(MatNo='" + poPdtOrder.tPdtCode + "',PlantCode='" + cVB.tVB_BchRefID + "',Sloc='" + cVB.tVB_WahRefNo + "')";
                    tFunc = "(MatNo='" + poPdtOrder.tPdtCode + "',PlantCode='" + cVB.tVB_BchRefID + "',Sloc='" + cVB.tVB_WahRefNo + "')";

                    if (C_CHKbCheckStockOnline(tFunc,1) == true) //Call API รับค้าโดยใช้ Model
                    {
                        if (oC_ResItem != null)
                        {
                            if ((decimal)(oC_ResItem.d.MatQty - cSumQty) >= poPdtOrder.cQty) //Check จำนวนสินค้าคงเหลือ ต้องมากกว่าเท่ากับจำนวนที่ต้องการ
                            {
                                poPdtOrder.tRemark = oC_ResItem.d.BinLoc; //*Arm 63-08-08 Location Product
                                cVB.oVB_Sale.W_ADDxPdtToOrder(poPdtOrder); // เพิ่มสินค้าลง DT และแสดงใน DataGridView
                            }
                            else
                            {
                                // จำนวนสินค้าคงเหลือ น้อยกว่าจำนวนที่ต้องการ แสดง Error Msg สินค้าคงเหลือไม่พอ และจำนวนคงเหลือ
                                tMsgErr = oC_Resource.GetString("tMsgPdtQtyDef") + System.Environment.NewLine;
                                tMsgErr += oC_Resource.GetString("tMsgPdtQtyBalance") + " : " + oSP.SP_SETtDecShwSve(1, (decimal)(oC_ResItem.d.MatQty - cSumQty), 0) + " " + oC_ResItem.d.MatUnit;
                                oSP.SP_SHWxMsg(tMsgErr, 3);
                                new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : (Scan) :" + tMsgErr, cVB.bVB_AlwPrnLog);
                            }
                        }
                        else
                        {
                            //ไม่พบสินค้าในคลังสินค้า
                            oSP.SP_SHWxMsg(oC_Resource.GetString("tMsgStockNotFoundPdt"), 3);
                        }
                    }
                }
                new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : End ", cVB.bVB_AlwPrnLog);
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("cStock", "C_CHKbScanPdtCheckStock : " + oEx.Message.ToString());
            }
            finally
            {
                oC_dtTmp = null;
                oC_ResItem = null;
                oC_Resource = null;
                oSP = null;
                tMsgErr = "";
                //new cSP().SP_CLExMemory();
            }
        }
        
        /// <summary>
        /// ตรวจสอบ Stock Online ตอนกดรวมเงิน
        /// </summary>
        /// <returns></returns>
        public bool C_CHKbSubTotalCheckStock()
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            DataTable odtTmpStk;
            cSP oSP;

            string tFunc = "";
            string tfilter = "";
            string tMsgFalse = "";
            
            try
            {
                new cLog().C_WRTxLog("cStock", "C_CHKbSubTotalCheckStock : Start ", cVB.bVB_AlwPrnLog);
                if (C_CHKbverify() == false) return false;

                oSql = new StringBuilder();
                oDB = new cDatabase();
                odtTmp = new DataTable();
                oSP = new cSP();
                C_SETxText();

                // List รายการสินค้าที่ Scan ทั้งหมดของบิลนี้
                oSql.AppendLine("SELECT FTBchCode,FTXshDocNo,FTPdtCode, FTXsdBarCode, SUM(ISNULL(FCXsdQty,0)) AS FCXsdQty FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo ='" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FTXsdStaPdt <> '4' ");
                oSql.AppendLine("GROUP BY FTBchCode,FTXshDocNo,FTPdtCode, FTXsdBarCode");

                odtTmp = new DataTable();
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    int nRow = 0;
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        // Add Parameter filter เพื่อส่งไปตรวจสอบที่ KADS
                        if (nRow == 0)
                        {
                            tfilter = "MatNo eq '" + oRow.Field<string>("FTPdtCode").Replace("-","") + "'";
                        }
                        else
                        {
                            tfilter += " or MatNo eq '" + oRow.Field<string>("FTPdtCode").Replace("-", "") + "'";
                        }

                        nRow++;
                    }

                    // set Function
                    //tFunc = "/sap/opu/odata/SAP/ZDP_GWSRV008_SRV/StockSet?$filter=(" + tfilter + ") and Sloc eq '"+cVB.tVB_WahRefNo+"'";
                    tFunc = "?$filter=(" + tfilter + ") and Sloc eq '" + cVB.tVB_WahRefNo + "'";


                    if (C_CHKbCheckStockOnline(tFunc, 2) == true) // Process Call Api รับค่าเข้า DataTable
                    {
                        if(oC_dtTmp != null && oC_dtTmp.Rows.Count > 0)
                        {
                            // Check หาสินค้าที่มีจำนวนสินค้าใน Stock ไม่พอ 
                            oSql.Clear();
                            oSql.AppendLine("SELECT DT.FTPdtCode, DT.FTXsdBarCode, ISNULL(Tmp.MatQty,0) AS MatQty,Tmp.MatUnit");
                            oSql.AppendLine("FROM (");
                            oSql.AppendLine("   SELECT FTBchCode,FTXshDocNo,FTPdtCode, FTXsdBarCode, SUM(ISNULL(FCXsdQty,0)) AS FCXsdQty FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                            oSql.AppendLine("   WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo ='" + cVB.tVB_DocNo + "' ");
                            oSql.AppendLine("   AND FTXsdStaPdt <> '4' ");
                            oSql.AppendLine("   GROUP BY FTBchCode,FTXshDocNo,FTPdtCode, FTXsdBarCode ");
                            oSql.AppendLine(") DT ");
                            oSql.AppendLine("INNER JOIN TTmpStockOnline Tmp WITH(NOLOCK) ON DT.FTPdtCode = Tmp.MatNo AND DT.FTXsdBarCode = Tmp.MatNo ");
                            oSql.AppendLine("WHERE ISNULL(DT.FCXsdQty,0) > ISNULL(Tmp.MatQty,0) AND Tmp.Sloc = '"+cVB.tVB_WahRefNo + "'");

                            odtTmpStk = new DataTable();
                            odtTmpStk = oDB.C_GEToDataQuery(oSql.ToString());

                            if(odtTmpStk != null && odtTmpStk.Rows.Count >0) // ถ้ามีสินค้าที่จำนวนใน Stock คงเหลือไม่พอ
                            {
                                // Message แสดงสินค้นใน Stock ไม่พอ
                                tMsgFalse = oC_Resource.GetString("tMsgPdtQtyDef");
                                foreach (DataRow oRow in odtTmpStk.Rows)
                                {
                                    tMsgFalse += System.Environment.NewLine;
                                    tMsgFalse += oRow.Field<string>("FTPdtCode") + " " + oC_Resource.GetString("tMsgPdtQtyBalance") + " " + oSP.SP_SETtDecShwSve(1,oRow.Field<decimal>("MatQty"), 0) + " " +oRow.Field<string>("MatUnit"); 
                                }
                                oSP.SP_SHWxMsg(tMsgFalse, 3);
                                new cLog().C_WRTxLog("cStock", "C_CHKbSubTotalCheckStock : "+ tMsgFalse, cVB.bVB_AlwPrnLog);
                                return false;
                            }
                        }
                    }
                }
                new cLog().C_WRTxLog("cStock", "C_CHKbSubTotalCheckStock : End ", cVB.bVB_AlwPrnLog);
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cStock", "C_CHKbSubTotalCheckStock : " + oEx.Message.ToString());
                return false;
            }
            finally
            {
                oDB = null;
                oSql = null;
                oSP = null;
                odtTmp = null;
                oC_dtTmp = null;
                oC_ResItem = null;
                odtTmpStk = null;
                oC_Resource = null;
                //new cSP().SP_CLExMemory();
            }
        }
        
        /// <summary>
        /// Process Call API KADS
        /// </summary>
        /// <param name="ptFunction"></param>
        /// <param name="pnMode"></param>
        /// <returns></returns>
        public bool C_CHKbCheckStockOnline(string ptFunction, int pnMode)
        {
            cClientService oCall;
            HttpResponseMessage oRep;
            cmlResKADS<cmlResKADSResult<cmlResInfoResultStock>> oResListItem; 
            
            string tUrl = "";
            oC_dtTmp = null;
            oC_ResItem = null;

            try
            {
                new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : Start ", cVB.bVB_AlwPrnLog);

                //if (string.IsNullOrEmpty(cVB.tVB_APIKADS_ChkStock))
                if (string.IsNullOrEmpty(cVB.tVB_ApiChkStk)) //*Arm 63-08-09
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlChkStkNotDefine"), 3);
                    return false;
                }
                
                tUrl = cVB.tVB_ApiChkStk + ptFunction; //*Arm 63-08-09
                new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : Request :" + tUrl, cVB.bVB_AlwPrnLog);

                oCall = new cClientService();
                //oCall = new cClientService("Authorization", cVB.tVB_KADSAuth_ChkStock);
                oCall = new cClientService("Authorization", cVB.tVB_ApiChkStk_Auth); //*Arm 63-08-09

                oRep = new HttpResponseMessage();
                try
                {
                    new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : Start call api check stock  ", cVB.bVB_AlwPrnLog);
                    oRep = oCall.C_GEToInvoke(tUrl);
                    oRep.EnsureSuccessStatusCode(); //*Arm 63-08-19
                    new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : End call api check stock ", cVB.bVB_AlwPrnLog);
                }
                //*Arm 63-08-19 - HttpRequestException
                catch (HttpRequestException oEx)
                {
                    string X = oEx.Message.ToString().Replace(" ", "");
                    if (oEx.Message.ToString().Replace(" ","") == "Responsestatuscodedoesnotindicatesuccess:404(NotFound).")
                    {
                        new cSP().SP_SHWxMsg(oC_Resource.GetString("tMsgStockNotFoundPdt"), 3);
                    }
                    else
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + Environment.NewLine + "[" + oEx.Message + "]", 2); //*Arm 63-08-19

                    }
                    new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : call api check stock  Error " + oEx.Message.ToString());
                    oCall.C_PRCxCloseConn(); //*Arm 63-07-20
                    return false;
                }
                //+++++++++++++++
                catch (Exception oEx)
                {
                    //new cSP().SP_SHWxMsg("Process fail !!!", 2);
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"), 2); //*Arm 63-08-19
                    new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : call api check stock  Error " + oEx.Message.ToString());
                    oCall.C_PRCxCloseConn(); //*Arm 63-07-20
                    return false;
                }
                
                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : Response StatusCode "+ oRep.StatusCode, cVB.bVB_AlwPrnLog);
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : Response :" + tJSonRes, cVB.bVB_AlwPrnLog);

                    if (pnMode == 1)
                    {
                        //ใช้ Model รับค่า
                        oC_ResItem = JsonConvert.DeserializeObject<cmlResKADS<cmlResInfoResultStock>>(tJSonRes);
                    }
                    else
                    {
                        //ใช้ DataTable รับค่า
                        oResListItem = new cmlResKADS<cmlResKADSResult<cmlResInfoResultStock>>();
                        oResListItem = JsonConvert.DeserializeObject<cmlResKADS<cmlResKADSResult<cmlResInfoResultStock>>>(tJSonRes);

                        string results = JsonConvert.SerializeObject(oResListItem.d.results);
                        oC_dtTmp = JsonConvert.DeserializeObject<DataTable>(results);
                        new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : C_INSxTmpStockOnline Start", cVB.bVB_AlwPrnLog);
                        C_INSxTmpStockOnline(oC_dtTmp); //เอาข้อมูลลง TTmpStockOnline
                        new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : C_INSxTmpStockOnline End", cVB.bVB_AlwPrnLog);
                    }
                    oCall.C_PRCxCloseConn(); //*Arm 63-07-20
                    //return true;
                }
                //else
                //{
                //    if (oRep.StatusCode == System.Net.HttpStatusCode.NotFound)
                //    {

                //        new cSP().SP_SHWxMsg(oC_Resource.GetString("tMsgStockNotFoundPdt"), 3);
                //    }
                //    else
                //    {
                //        new cSP().SP_SHWxMsg("Check Stock Online Error : " + oRep.StatusCode, 3);
                //    }

                //    new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : call api check stock (Line 2/2.1) Error/" + oRep.StatusCode);

                //    oCall.C_PRCxCloseConn(); //*Arm 63-07-20
                //    return false;
                //}
                return true;
            }
            catch (Exception oEx)
            {
                //new cSP().SP_SHWxMsg("Process fail !!!", 2);
                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"), 2); //*Arm 63-08-19
                new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : " + oEx.Message);
                return false;
            }
            finally
            {
                oCall = null;
                oRep = null;
                new cLog().C_WRTxLog("cStock", "C_CHKbCheckStockOnline : End ", cVB.bVB_AlwPrnLog);
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Gen TTmpStockOnline
        /// </summary>
        /// <param name="poDbTbl"></param>
        private void C_INSxTmpStockOnline(DataTable poDbTbl)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            try
            {
                oSql.Clear();
                //oSql.AppendLine("DROP TABLE TTmpStockOnline");

                //oSql.AppendLine("IF OBJECT_ID(N'TTmpStockOnline') IS NULL BEGIN");
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TTmpStockOnline'))"); //*Arm 63-08-17
                oSql.AppendLine("BEGIN "); //*Arm 63-08-17
                oSql.AppendLine("    CREATE TABLE[dbo].[TTmpStockOnline](");
                oSql.AppendLine("        [BinLoc][varchar](10) NULL,");
                oSql.AppendLine("        [MatNo] [varchar] (18) NULL,");
                oSql.AppendLine("        [PicNum] [varchar] (18) NULL,");
                oSql.AppendLine("        [PlantCode] [varchar] (4) NULL,");
                oSql.AppendLine("        [Sloc] [varchar] (4) NULL,");
                oSql.AppendLine("        [SaleOrg] [varchar] (4) NULL,");
                oSql.AppendLine("        [MatQty] [numeric](13, 3) NULL,");
                oSql.AppendLine("        [MatUnit] [varchar] (3) NULL");
                oSql.AppendLine("    ) ON[PRIMARY]");
                oSql.AppendLine("END");
                oSql.AppendLine("TRUNCATE TABLE TTmpStockOnline");

                oDb.C_SETxDataQuery(oSql.ToString());
                
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_ConStr, SqlBulkCopyOptions.Default))
                {

                    foreach (DataColumn oColName in poDbTbl.Columns)
                    {
                        oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
                    }
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TTmpStockOnline";
                    try
                    {
                        oBulkCopy.WriteToServer(poDbTbl);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("cStock", "C_INSxTmpStockOnline/TTmpStockOnline"+ oEx.Message.ToString());
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cStock", "C_INSxTmpStockOnline : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDb = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// ตรวจสอบ Parameter
        /// </summary>
        /// <returns></returns>
        public bool C_CHKbverify()
        {
            try
            {
                //*Arm 63-08-09
                if (string.IsNullOrEmpty(cVB.tVB_ApiChkStk))       // Check URL API KADS
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlChkStkNotDefine"), 3);
                    new cLog().C_WRTxLog("cStock", "C_CHKbverify : url check stock not define.", cVB.bVB_AlwPrnLog);
                    return false;
                }
                if (string.IsNullOrEmpty(cVB.tVB_ApiChkStk_Auth))     //Check Authorization
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgAuthNotDefine"), 3);
                    new cLog().C_WRTxLog("cStock", "C_CHKbverify : Basic Authorization not define.", cVB.bVB_AlwPrnLog);
                    return false;
                }
                //++++++++++++++

                if (string.IsNullOrEmpty(cVB.tVB_BchRefID))     // Check Referent Branch
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgBchRefIDNotDefine"), 3);
                    new cLog().C_WRTxLog("cStock", "C_CHKbverify : PlantCode/BchRefID not define.", cVB.bVB_AlwPrnLog);
                    return false;
                }
                if (string.IsNullOrEmpty(cVB.tVB_WahRefNo))   // Check Referent Wahouse
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgWahRefCodeNotDefine"), 3);
                    new cLog().C_WRTxLog("cStock", "C_CHKbverify : Sloc/WahRefNo not define.", cVB.bVB_AlwPrnLog);
                    return false;
                }
                new cLog().C_WRTxLog("cStock", "C_CHKbverify : True", cVB.bVB_AlwPrnLog);
                return true;
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("cStock", "C_CHKbverify : " + oEx.Message);
                return false;
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
    }
}
