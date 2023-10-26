using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AdaPos.Popup.wSale
{
    public partial class wChooseItemRef : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Mode;        //*Arm 63-02-07 1:ขาย , 2:คืน
        private string tW_FuncName; //*Arm 63-02-07 - Function Name
        private int nW_SelectRow;   //*Arm 63-03-02 for Sale
        
        private int nW_ChkPayRedeem = 0;    //*Arm 63-03-20    0: การคืนปกติ, 1:กรณีบิลขายที่มีการแลกแต้มเป็นชำระเงินสดด้วย
        //private int nW_EditQty = 1;         //*Arm 63-03-20    แก้ไขจำนวนการคืน 1:อนุญาต , 2:ไม่อนุญาต
        private DataTable aW_SalPD;    //*Em 63-04-16

        public wChooseItemRef(int pnMode, string ptFuncName="")
        {
            new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef : Start", cVB.bVB_AlwPrnLog);
            InitializeComponent();
            try
            {
                oW_SP = new cSP();

                nW_Mode = pnMode;           //*Arm 63-02-07
                tW_FuncName = ptFuncName;   //*Arm 63-02-07
                new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef : W_SETxDesign", cVB.bVB_AlwPrnLog);
                W_SETxDesign();
                new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef : W_SETxText", cVB.bVB_AlwPrnLog);
                W_SETxText();
                new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef : W_DATxLoadData", cVB.bVB_AlwPrnLog);
                W_DATxLoadData();

                //*Em 63-04-16
                if(nW_Mode == 2)
                {
                    new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef : W_DATxLoadPD", cVB.bVB_AlwPrnLog);
                    W_DATxLoadPD();
                }
                //++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef : " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
            new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef : End", cVB.bVB_AlwPrnLog);
        }

        #region Function
        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                //ogdDetail.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdDetail);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "W_SETxDesign : " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                 
                //*Arm 63-02-07
                switch (nW_Mode)
                {
                    case 1: // กด HotKey จากหน้า Sale
                        olaTitleAbout.Text = oW_Resource.GetString("tTitleChooseItem");
                        otbTitleSeqNo.HeaderText = cVB.oVB_GBResource.GetString("tCS_Seq");
                        otbTitlePdtCode.HeaderText = oW_Resource.GetString("tTitlePdtCode");
                        otbTitlePdtName.HeaderText = oW_Resource.GetString("tTitlePdtName");
                        otbTitleUnit.HeaderText = oW_Resource.GetString("tTitleUnit");
                        otbTitleSetPrice.HeaderText = oW_Resource.GetString("tTitlePrice");
                        otbTitleQty.HeaderText = cVB.oVB_GBResource.GetString("tQty");
                        otbTitleQtyRfn.HeaderText = oW_Resource.GetString("tQtyRfn");
                        otbTitleAmount.HeaderText = oW_Resource.GetString("tAmt");
                        ockTitleChoose.HeaderText = oW_Resource.GetString("tChoose");
                        ockSelectAll.Visible = false;
                        ockSelectAll.Text = oW_Resource.GetString("tSelectAll");
                        break;
                    case 2: // คืนรายการขาย
                    case 3: // *Arm 63-06-10 ขายอ้างอิงคืน
                        //olaTitleAbout.Text = oW_Resource.GetString("tTitleChooseItemRef");

                        if (nW_Mode == 3) //*Arm 63-06-10
                        {
                            olaTitleAbout.Text = oW_Resource.GetString("tTitleChooseItemRefRtn");
                            otbTitleQty.HeaderText = cVB.oVB_GBResource.GetString("tQty");
                        }
                        else
                        {
                            olaTitleAbout.Text = oW_Resource.GetString("tTitleChooseItemRef");
                            otbTitleQty.HeaderText = oW_Resource.GetString("tQty");
                        }

                        otbTitlePdtCode.HeaderText = oW_Resource.GetString("tTitlePdtCode");
                        otbTitlePdtName.HeaderText = oW_Resource.GetString("tTitlePdtName");
                        otbTitleUnit.HeaderText = oW_Resource.GetString("tTitleUnit");
                        otbTitleSetPrice.HeaderText = oW_Resource.GetString("tTitlePrice");
                        //otbTitleQty.HeaderText = oW_Resource.GetString("tQty");
                        
                        otbTitleQtyRfn.HeaderText = oW_Resource.GetString("tQtyRfn");
                        otbTitleAmount.HeaderText = oW_Resource.GetString("tAmt");
                        ockTitleChoose.HeaderText = oW_Resource.GetString("tChoose");



                        ockSelectAll.Visible = true;
                        ockSelectAll.Text = oW_Resource.GetString("tSelectAll");
                        break;
                }

                
                //olaTitleAbout.Text = oW_Resource.GetString("tTitleChooseItemRef");
                //otbTitlePdtCode.HeaderText = oW_Resource.GetString("tTitlePdtCode");
                //otbTitlePdtName.HeaderText = oW_Resource.GetString("tTitlePdtName");
                //otbTitleUnit.HeaderText = oW_Resource.GetString("tTitleUnit");
                //otbTitleSetPrice.HeaderText = oW_Resource.GetString("tTitlePrice");
                //otbTitleQty.HeaderText = oW_Resource.GetString("tQty");
                //otbTitleQtyRfn.HeaderText = oW_Resource.GetString("tQtyRfn");
                //otbTitleAmount.HeaderText = oW_Resource.GetString("tAmt");
                //ockTitleChoose.HeaderText = oW_Resource.GetString("tChoose");
                //ocmEditQtyRfn.Text = oW_Resource.GetString("tEditQtyRfn");


            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "W_SETxText : " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }
        /// <summary>
        /// *Arm 63-03-25
        /// Check ส่วนลดจากการแลกแต้ม
        /// </summary>
        /// <param name="nSeqNo"></param>
        /// <returns></returns>
        private string W_CHKxRedeem(int nSeqNo)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            string tCase = "0"; // 0:แบบปกติ 1: coupon 2:Redeem รายการ 3:Redeem ส่วนลดท้ายบิล
            try
            {
                oSql.Clear();
                //oSql.AppendLine("SELECT DISTINCT COUNT(*) FROM TPSTSalDT DT ");
                oSql.AppendLine("SELECT DISTINCT COUNT(DT.FTXshDocNo) FROM TPSTSalDT DT WITH(NOLOCK)"); //*Em 63-04-28
                oSql.AppendLine("INNER JOIN TPSTSalDTDis DTDis ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo ");
                oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND DT.FNXsdSeqNo ='" + nSeqNo + "' ");
                oSql.AppendLine("AND DTDis.FNXddStaDis = 2 AND ISNULL(DTDis.FTXddRefCode,'') != '' ");
                oSql.AppendLine("AND(DTDis.FTXddDisChgType = 5 OR DTDis.FTXddDisChgType = 6) ");
                if (oDB.C_GEToDataQuery<int>(oSql.ToString()) > 0)
                {
                    tCase = "1";
                }

                oSql.Clear();
                //oSql.AppendLine("SELECT DISTINCT COUNT(*) FROM TPSTSalDT DT ");
                oSql.AppendLine("SELECT DISTINCT COUNT(DT.FTXshDocNo) FROM TPSTSalDT DT WITH(NOLOCK)"); //*Em 63-04-28
                oSql.AppendLine("INNER JOIN TPSTSalDTDis DTDis ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo ");
                oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND DT.FNXsdSeqNo ='" + nSeqNo + "' ");
                oSql.AppendLine("AND DTDis.FNXddStaDis = 2 AND ISNULL(DTDis.FTXddRefCode,'') != '' ");
                oSql.AppendLine("AND(DTDis.FTXddDisChgType = 1) ");
                if (oDB.C_GEToDataQuery<int>(oSql.ToString()) > 0)
                {
                    tCase ="2";
                }

                oSql.Clear();
                //oSql.AppendLine("SELECT DISTINCT COUNT(*) FROM TPSTSalDT DT WITH(NOLOCK)");
                oSql.AppendLine("SELECT DISTINCT COUNT(DT.FTXshDocNo) FROM TPSTSalDT DT WITH(NOLOCK)"); //*Em 63-04-28
                oSql.AppendLine("INNER JOIN TPSTSalDTDis DTDis WITH(NOLOCK) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo ");
                oSql.AppendLine("INNER JOIN TPSTSalRD RD WITH(NOLOCK) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FTXddRefCode = RD.FTXrdRefCode ");
                oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND DT.FNXsdSeqNo ='" + nSeqNo + "' ");
                oSql.AppendLine("AND DTDis.FNXddStaDis = 2 AND DTDis.FTXddDisChgType = 1 AND ISNULL(DTDis.FTXddRefCode,'') != '' ");
                oSql.AppendLine("AND RD.FTRdhDocType = 2 ");
                oSql.AppendLine("AND RD.FNXrdRefSeq = 0 ");
                if (oDB.C_GEToDataQuery<int>(oSql.ToString()) > 0)
                {
                    tCase = "3";
                }
                
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wChooseItemRef", "W_CHKxRedeem : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //oW_SP.SP_CLExMemory();
            }
            return tCase;

        }
        private void W_DATxLoadData()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable(); //*Em 63-05-12
            List<cmlTPSTSalDT> aoDT;
            int nRow = 0;
            int nChkPay = 0;
            bool bChoose = false;
            try
            {
                //*Arm 62-12-19 Check Option การคืน 
                //================================
                //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                //2:คืนได้ครังเดียว บางรายการได้ 
                //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 
                //5:ห้ามคืน

                //*Arm 63-02-07 Check Mode
                switch (nW_Mode)
                {
                    case 1: // กด HotKey จากหน้า Sale

                        //*Arm 63-02-07 
                        string tC_TblSalDT = "TSDT" + cVB.tVB_PosCode;
                        aoDT = new List<cmlTPSTSalDT>();
                        oSql.Clear();
                        oSql.AppendLine("SELECT FNXsdSeqNo AS otbTitleSeqNo,FTPdtCode AS otbTitlePdtCode,FTXsdPdtName AS otbTitlePdtName,");
                        oSql.AppendLine("FTPunName AS otbTitleUnit,FCXsdSetPrice AS otbTitleSetPrice,FCXsdQty AS otbTitleQty,FCXsdQty AS otbTitleQtyRfn,");

                        //*Arm 63-06-10 เงื่อนถ้าส่วนท้ายบิล ให้ดึง FCXsdNetAfHD มาแสดง
                        if (tW_FuncName == "C_KBDxDisAmtBillByDT" || tW_FuncName == "C_KBDxDisPerBillByDT")
                        {
                            oSql.AppendLine("FCXsdNetAfHD AS otbTitleAmount, ");
                        }
                        else
                        {
                            oSql.AppendLine("FCXsdNet AS otbTitleAmount, ");
                        }
                        oSql.AppendLine("FCXsdFactor AS otbTitleFactor,FTXsdBarCode AS otbTitleBarcode,'' AS otbTitleRedeem,'' AS otbTitlePmt");
                        //oSql.AppendLine("FCXsdNet AS otbTitleAmount,FCXsdFactor AS otbTitleFactor,FTXsdBarCode AS otbTitleBarcode,'' AS otbTitleRedeem,'' AS otbTitlePmt");
                        oSql.AppendLine("FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                        oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' ");
                        oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                        if (tW_FuncName == "C_KBDxVoidItem")
                        {
                            oSql.AppendLine("AND DT.FTXsdStaPdt != '4' ");
                        }
                        else
                        {
                            oSql.AppendLine("AND (DT.FTXsdStaPdt != '4' AND DT.FTXsdStaPdt != '3')");
                        }

                        if(tW_FuncName == "C_KBDxChgAmt" || tW_FuncName == "C_KBDxChgPer" || tW_FuncName == "C_KBDxDisAmt" || tW_FuncName == "C_KBDxDisPer" || tW_FuncName == "C_KBDxDisAmtBillByDT" || tW_FuncName == "C_KBDxDisPerBillByDT") //*Arm 63-06-10 เพิ่ม tW_FuncName == "C_KBDxDisAmtBillByDT" || tW_FuncName == "C_KBDxDisPerBillByDT"
                        {
                            if (tW_FuncName == "C_KBDxDisAmt" || tW_FuncName == "C_KBDxDisPer" || tW_FuncName == "C_KBDxDisAmtBillByDT" || tW_FuncName == "C_KBDxDisPerBillByDT") //*Arm 63-07-22 
                            {
                                //ระบบต้องไม่แสดงสินค้าราคา 0 ให้เลือก (แก้ไขตาม issue 101)
                                oSql.AppendLine("AND DT.FCXsdSetPrice > 0 ");
                            }
                            oSql.AppendLine("AND DT.FTXsdStaAlwDis = '1' ");
                        }
                        
                        oSql.AppendLine("ORDER BY DT.FNXsdSeqNo ASC");

                        //*Em 63-05-12
                        odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                        if (odtTmp == null || odtTmp.Rows.Count == 0)
                        {
                            new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgNoItemDisChrg"), 1);
                            ocmBack_Click(ocmBack, new EventArgs());
                            return;
                        }

                        ogdDetail.Rows.Clear();
                        ogdDetail.Columns.Clear();
                        ogdDetail.DataSource = odtTmp;
                        W_SETxGridColumns();
                        //+++++++++++++++++++++++++++++++++++++++++
                        break;

                    case 2: // คืนรายการขาย
                        new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : Start", cVB.bVB_AlwPrnLog);
                        aoDT = new List<cmlTPSTSalDT>();

                        ////*Arm 63-05-25
                        //if (cVB.bVB_RefundTrans == true)
                        //{
                        //    oSql.Clear();
                        //    oSql.AppendLine("SELECT FNXsdSeqNo AS otbTitleSeqNo,FTPdtCode AS otbTitlePdtCode,FTXsdPdtName AS otbTitlePdtName,");
                        //    oSql.AppendLine("FTPunName AS otbTitleUnit,FCXsdSetPrice AS otbTitleSetPrice,FCXsdQty AS otbTitleQty,FCXsdQty AS otbTitleQtyRfn,");
                        //    oSql.AppendLine("FCXsdNet AS otbTitleAmount,FCXsdFactor AS otbTitleFactor,FTXsdBarCode AS otbTitleBarcode,");
                        //    oSql.AppendLine("ISNULL((SELECT FTXrdRefCode + ',' FROM TPSTSalRD WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXrdRefSeq = DT.FNXsdSeqNo FOR XML PATH('')),'') AS otbTitleRedeem,");
                        //    oSql.AppendLine("ISNULL((SELECT FTPmhDocNo + ',' FROM TPSTSalPD WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo = DT.FNXsdSeqNo FOR XML PATH('')),'') AS otbTitlePmt");
                        //    oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                        //    oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' ");
                        //    oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        //    oSql.AppendLine("AND DT.FTXsdStaPdt <> '4'");
                        //    if (cVB.nVB_ReturnType == 3) oSql.AppendLine("AND ISNULL(DT.FCXsdQtyLef,0) > 0");
                        //    oSql.AppendLine("ORDER BY DT.FNXsdSeqNo");
                        //}
                        //else
                        //{
                        //    oSql.Clear();
                        //    oSql.AppendLine("SELECT FNXsdSeqNo AS otbTitleSeqNo,FTPdtCode AS otbTitlePdtCode,FTXsdPdtName AS otbTitlePdtName,");
                        //    oSql.AppendLine("FTPunName AS otbTitleUnit,FCXsdSetPrice AS otbTitleSetPrice,FCXsdQty AS otbTitleQty,FCXsdQty AS otbTitleQtyRfn,");
                        //    oSql.AppendLine("FCXsdNet AS otbTitleAmount,FCXsdFactor AS otbTitleFactor,FTXsdBarCode AS otbTitleBarcode,");
                        //    oSql.AppendLine("ISNULL((SELECT FTXrdRefCode + ',' FROM " + cSale.tC_TblSalRD + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXrdRefSeq = DT.FNXsdSeqNo FOR XML PATH('')),'') AS otbTitleRedeem,");
                        //    oSql.AppendLine("ISNULL((SELECT FTPmhDocNo + ',' FROM " + cSale.tC_TblSalPD + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo = DT.FNXsdSeqNo FOR XML PATH('')),'') AS otbTitlePmt");
                        //    oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " DT WITH(NOLOCK)");
                        //    oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' ");
                        //    oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        //    oSql.AppendLine("AND DT.FTXsdStaPdt <> '4'");
                        //    if (cVB.nVB_ReturnType == 3) oSql.AppendLine("AND ISNULL(DT.FCXsdQtyLef,0) > 0");
                        //    oSql.AppendLine("ORDER BY DT.FNXsdSeqNo");

                        //}
                        ////aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());

                        //*Arm 63-06-10
                        oSql.Clear();
                        oSql.AppendLine("SELECT FNXsdSeqNo AS otbTitleSeqNo,FTPdtCode AS otbTitlePdtCode,FTXsdPdtName AS otbTitlePdtName,");
                        //oSql.AppendLine("FTPunName AS otbTitleUnit,FCXsdSetPrice AS otbTitleSetPrice,FCXsdQty AS otbTitleQty,FCXsdQty AS otbTitleQtyRfn,");
                        oSql.AppendLine("FTPunName AS otbTitleUnit,FCXsdSetPrice AS otbTitleSetPrice,FCXsdQtyLef AS otbTitleQty,FCXsdQtyLef AS otbTitleQtyRfn,"); //*Arm 63-09-16
                        oSql.AppendLine("FCXsdNet AS otbTitleAmount,FCXsdFactor AS otbTitleFactor,FTXsdBarCode AS otbTitleBarcode,");
                        //oSql.AppendLine("ISNULL((SELECT FTXrdRefCode + ',' FROM " + cSale.tC_Ref_TblSalRD + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXrdRefSeq = DT.FNXsdSeqNo FOR XML PATH('')),'') AS otbTitleRedeem,"); //*Arm 63-09-17 Comment Code
                        //oSql.AppendLine("ISNULL((SELECT FTPmhDocNo + ',' FROM " + cSale.tC_Ref_TblSalPD + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo = DT.FNXsdSeqNo FOR XML PATH('')),'') AS otbTitlePmt"); //*Arm 63-09-17 Comment Code
                        //*Arm 63-09-17
                        oSql.AppendLine("CASE ");
                        oSql.AppendLine("    WHEN ISNULL((SELECT FTXrdRefCode + ',' FROM TPSTSalRDTmp WITH(NOLOCK) WHERE FTXshDocNo = DT.FTXshDocNo  FOR XML PATH('')),'') != ''");
                        oSql.AppendLine("    THEN ISNULL((SELECT FTXrdRefCode +',' FROM TPSTSalRDTmp WITH(NOLOCK) WHERE FTXshDocNo = DT.FTXshDocNo  FOR XML PATH('')),'')");
                        oSql.AppendLine("    ELSE ISNULL((SELECT FTXrdRefCode +',' FROM TPSTSalRDTmp WITH(NOLOCK) WHERE FTXshDocNo = DT.FTXshDocNo AND FNXrdRefSeq = DT.FNXsdSeqNo FOR XML PATH('')),'')");
                        oSql.AppendLine("END AS otbTitleRedeem,");
                        oSql.AppendLine("ISNULL((SELECT FTPmhDocNo + ',' FROM " + cSale.tC_Ref_TblSalPD + " WITH(NOLOCK) WHERE FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo = DT.FNXsdSeqNo FOR XML PATH('')),'') AS otbTitlePmt");
                        //+++++++++++++

                        oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " DT WITH(NOLOCK)");
                        //oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' ");
                        //oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        oSql.AppendLine("WHERE DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' ");//*Arm 63-09-11 รองรับให้คืนข้ามสาขาได้
                        oSql.AppendLine("AND DT.FTXsdStaPdt <> '4'");
                        if (cVB.nVB_ReturnType == 3) oSql.AppendLine("AND ISNULL(DT.FCXsdQtyLef,0) > 0");
                        oSql.AppendLine("ORDER BY DT.FNXsdSeqNo");
                        odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                        //*Em 63-05-12
                        ogdDetail.Rows.Clear();
                        ogdDetail.Columns.Clear();

                        DataGridViewCheckBoxColumn oColChk = new DataGridViewCheckBoxColumn();
                        oColChk.FillWeight = 20F;
                        oColChk.Name = "ockTitleChoose";
                        oColChk.CellTemplate.Style.BackColor = Color.White;
                        oColChk.ValueType = typeof(Boolean);
                        ogdDetail.Columns.Add(oColChk);

                        ogdDetail.DataSource = odtTmp;
                        W_SETxGridColumns();
                        //+++++++++++++++

                        new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : start button", cVB.bVB_AlwPrnLog);
                        DataGridViewDisableButtonColumn ocmEditQtyRfn = new DataGridViewDisableButtonColumn();
                        ocmEditQtyRfn.FillWeight = 40F;
                        ocmEditQtyRfn.Name = "ocmEditQtyRfn";
                        ocmEditQtyRfn.HeaderText = oW_Resource.GetString("tEditQtyRfn");
                        ocmEditQtyRfn.UseColumnTextForButtonValue = true;
                        ocmEditQtyRfn.Text = oW_Resource.GetString("tEditQtyRfn");
                        ocmEditQtyRfn.CellTemplate.Style.BackColor = Color.White;
                        ogdDetail.Columns.Add(ocmEditQtyRfn);
                        ogdDetail.CurrentCellDirtyStateChanged += new EventHandler(ogdDetail_CurrentCellDirtyStateChanged);

                        //for (int ni = 0; ni < ogdDetail.Rows.Count; ni++)
                        //{
                        //    DataGridViewDisableButtonCell ocmCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[ni].Cells["ocmEditQtyRfn"];
                        //    DataGridViewCheckBoxCell ockCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[ni].Cells["ockTitleChoose"];
                        //    if (nW_ChkPayRedeem == 1)
                        //    {
                        //        ocmCell.Enabled = false;
                        //    }
                        //    else
                        //    {
                        //        ocmCell.Enabled = ockCell.Value == null ? false :(Boolean)ockCell.Value;
                        //    }

                        //}
                        new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : end button", cVB.bVB_AlwPrnLog);

                        new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : check redeem", cVB.bVB_AlwPrnLog);
                        //*Arm 63-03-19 เช็คการแลกแต้ม แบบชำระแทนเงินสด

                        //oSql.Clear();
                        ////oSql.AppendLine("SELECT COUNT(*) FROM TPSTSalRC TSRC WITH(NOLOCK)");
                        //oSql.AppendLine("SELECT COUNT(TSRC.FTRcvCode) FROM TPSTSalRC TSRC WITH(NOLOCK)");   //*Em 63-04-28
                        //oSql.AppendLine("INNER JOIN TFNMRcv TRCV WITH(NOLOCK) ON TSRC.FTRcvCode = TRCV.FTRcvCode AND TRCV.FTRcvStaUse = '1' AND(TRCV.FTFmtCode = '022' OR TRCV.FTFmtCode = '023')"); //*Arm 63-03-31
                        ////oSql.AppendLine("INNER JOIN TFNMRcvSpc RCVA WITH(NOLOCK) ON TRCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'PS'");
                        ////oSql.AppendLine("AND(ISNULL(RCVA.FTBchCode, '') = '' OR ISNULL(RCVA.FTBchCode, '') = '" + cVB.tVB_BchCode + "')");
                        ////oSql.AppendLine("AND(ISNULL(RCVA.FTMerCode, '') = '' OR ISNULL(RCVA.FTMerCode, '') = '" + cVB.tVB_Merchart + "')");
                        ////oSql.AppendLine("AND(ISNULL(RCVA.FTShpCode, '') = '' OR ISNULL(RCVA.FTShpCode, '') = '" + cVB.tVB_ShpCode + "')");
                        //oSql.AppendLine("WHERE TSRC.FTBchCode = '" + cVB.tVB_BchCode + "' AND TSRC.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");

                        ////*Arm 63-05-25
                        //if (cVB.bVB_RefundTrans == true)
                        //{
                        //    oSql.Clear();
                        //    //oSql.AppendLine("SELECT COUNT(*) FROM TPSTSalRC TSRC WITH(NOLOCK)");
                        //    oSql.AppendLine("SELECT COUNT(TSRC.FTRcvCode) FROM TPSTSalRC TSRC WITH(NOLOCK)");   //*Em 63-04-28
                        //    oSql.AppendLine("INNER JOIN TFNMRcv TRCV WITH(NOLOCK) ON TSRC.FTRcvCode = TRCV.FTRcvCode AND TRCV.FTRcvStaUse = '1' AND(TRCV.FTFmtCode = '022' OR TRCV.FTFmtCode = '023')"); //*Arm 63-03-31
                        //    oSql.AppendLine("WHERE TSRC.FTBchCode = '" + cVB.tVB_BchCode + "' AND TSRC.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        //}
                        //else
                        //{
                        //    oSql.Clear();
                        //    //oSql.AppendLine("SELECT COUNT(*) FROM TPSTSalRC TSRC WITH(NOLOCK)");
                        //    oSql.AppendLine("SELECT COUNT(TSRC.FTRcvCode) FROM " + cSale.tC_TblSalRC + " TSRC WITH(NOLOCK)");   //*Em 63-04-28
                        //    oSql.AppendLine("INNER JOIN TFNMRcv TRCV WITH(NOLOCK) ON TSRC.FTRcvCode = TRCV.FTRcvCode AND TRCV.FTRcvStaUse = '1' AND(TRCV.FTFmtCode = '022' OR TRCV.FTFmtCode = '023')"); //*Arm 63-03-31
                        //    oSql.AppendLine("WHERE TSRC.FTBchCode = '" + cVB.tVB_BchCode + "' AND TSRC.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        //}

                        //*Arm 63-06-01
                        oSql.Clear();
                        oSql.AppendLine("SELECT COUNT(TSRC.FTRcvCode) FROM " + cSale.tC_Ref_TblSalRC + " TSRC WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TFNMRcv TRCV WITH(NOLOCK) ON TSRC.FTRcvCode = TRCV.FTRcvCode AND TRCV.FTRcvStaUse = '1' AND(TRCV.FTFmtCode = '022' OR TRCV.FTFmtCode = '023')"); //*Arm 63-03-31
                        //oSql.AppendLine("WHERE TSRC.FTBchCode = '" + cVB.tVB_BchCode + "' AND TSRC.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        oSql.AppendLine("WHERE TSRC.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'"); //*Arm 63-09-11 รองรับให้คืนข้ามสาขาได้
                        //+++++++++++++

                        nChkPay = oDB.C_GEToDataQuery<int>(oSql.ToString());
                        new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : End check redeem", cVB.bVB_AlwPrnLog);
                        if (nChkPay >0)
                        {
                            //ถ้ามีการแลกแต้ม แบบชำระแทนเงินสด : บังคับคืนทั้งบิล
                            nW_ChkPayRedeem = 1;
                            bChoose = true;
                        }
                        else
                        {
                            nW_ChkPayRedeem = 0;
                            bChoose = false;
                        }
                        new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : End");
                        break;

                    case 3: //*Arm 63-06-10 ขายอ้างอิงคืน 
                        new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : Start");
                        oSql.Clear();
                        oSql.AppendLine("SELECT FNXsdSeqNo AS otbTitleSeqNo,FTPdtCode AS otbTitlePdtCode,FTXsdPdtName AS otbTitlePdtName,");
                        oSql.AppendLine("FTPunName AS otbTitleUnit,FCXsdSetPrice AS otbTitleSetPrice,FCXsdQty AS otbTitleQty,FCXsdQty AS otbTitleQtyRfn,");
                        oSql.AppendLine("FCXsdNet AS otbTitleAmount,FCXsdFactor AS otbTitleFactor,FTXsdBarCode AS otbTitleBarcode,'' AS otbTitleRedeem, '' AS otbTitlePmt");
                        oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " DT WITH(NOLOCK)");
                        //oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' ");
                        //oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        oSql.AppendLine("WHERE DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' "); //*Arm 63-09-11 รองรับให้คืนข้ามสาขาได้
                        oSql.AppendLine("ORDER BY DT.FNXsdSeqNo");

                        odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                        ogdDetail.Rows.Clear();
                        ogdDetail.Columns.Clear();

                        oColChk = new DataGridViewCheckBoxColumn();
                        oColChk.FillWeight = 20F;
                        oColChk.Name = "ockTitleChoose";
                        oColChk.CellTemplate.Style.BackColor = Color.White;
                        oColChk.ValueType = typeof(Boolean);
                        ogdDetail.Columns.Add(oColChk);

                        ogdDetail.DataSource = odtTmp;
                        W_SETxGridColumns();

                        new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : End", cVB.bVB_AlwPrnLog);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadData : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        private void W_DATxLoadPD()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                aW_SalPD = new DataTable();
                //oSql.Clear();
                //oSql.AppendLine("SELECT FTPmhDocNo,FNXsdSeqNo,FTPdtCode");
                //oSql.AppendLine("FROM TPSTSalPD WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //aW_SalPD = oDB.C_GEToDataQuery(oSql.ToString());

                ////*Arm 63-05-25
                //oSql.Clear();
                //oSql.AppendLine("SELECT FTPmhDocNo,FNXsdSeqNo,FTPdtCode");
                //if (cVB.bVB_RefundTrans == true)
                //{
                //    oSql.AppendLine("FROM TPSTSalPD WITH(NOLOCK)");
                //}
                //else
                //{
                //    oSql.AppendLine("FROM " + cSale.tC_TblSalPD + " WITH(NOLOCK)");
                //}
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //aW_SalPD = oDB.C_GEToDataQuery(oSql.ToString());
                ////+++++++++++++


                //*Arm 63-06-10
                oSql.Clear();
                oSql.AppendLine("SELECT FTPmhDocNo,FNXsdSeqNo,FTPdtCode");
                oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalPD + " WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'"); //*Arm 63-09-11 รองรับการคืนข้ามสาขาได้
                aW_SalPD = oDB.C_GEToDataQuery(oSql.ToString());
                //+++++++++++++


            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "W_DATxLoadPD : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDB = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        private void W_CHKxPmtSelect(string ptPmtCode,string ptRedeem,bool pbSelect)
        {
            string[] atPmtCode = { };
            string[] atRedeem = { };
            DataGridViewDisableButtonCell ocmCell;
            try
            {
                //if (aW_SalPD == null || aW_SalPD.Rows.Count == 0) return;

                if (string.IsNullOrEmpty(ptPmtCode) && string.IsNullOrEmpty(ptRedeem)) return;

                atPmtCode = ptPmtCode.Split(',');
                atRedeem = ptRedeem.Split(',');

                foreach(DataGridViewRow oRowGrid in ogdDetail.Rows)
                {
                    if (Convert.ToBoolean(oRowGrid.Cells[ockTitleChoose.Name].Value) != pbSelect)
                    {
                        foreach (string tPmt in atPmtCode)
                        {
                            if (oRowGrid.Cells[otbTitlePmt.Name].Value.ToString().IndexOf(tPmt) != -1 && !string.IsNullOrEmpty(oRowGrid.Cells[otbTitlePmt.Name].Value.ToString()))
                            {
                                oRowGrid.Cells[ockTitleChoose.Name].Value = pbSelect;
                                ocmCell = (DataGridViewDisableButtonCell)oRowGrid.Cells["ocmEditQtyRfn"];
                                ocmCell.Enabled = false;
                                ogdDetail.Invalidate();
                                if (oRowGrid.Cells[otbTitlePmt.Name].Value.ToString() != ptPmtCode) W_CHKxPmtSelect(oRowGrid.Cells[otbTitlePmt.Name].Value.ToString(), oRowGrid.Cells[otbTitleRedeem.Name].Value.ToString(), pbSelect);
                                break;
                            }
                        }
                        if (Convert.ToBoolean(oRowGrid.Cells[ockTitleChoose.Name].Value) != pbSelect)
                        {
                            foreach (string tRedeem in atRedeem)
                            {
                                if (oRowGrid.Cells[otbTitleRedeem.Name].Value.ToString().IndexOf(tRedeem) != -1 && !string.IsNullOrEmpty(oRowGrid.Cells[otbTitleRedeem.Name].Value.ToString()))
                                {
                                    oRowGrid.Cells[ockTitleChoose.Name].Value = pbSelect;
                                    ocmCell = (DataGridViewDisableButtonCell)oRowGrid.Cells["ocmEditQtyRfn"];
                                    ocmCell.Enabled = false;
                                    ogdDetail.Invalidate();
                                    if (oRowGrid.Cells[otbTitlePmt.Name].Value.ToString() != ptPmtCode) W_CHKxPmtSelect(oRowGrid.Cells[otbTitlePmt.Name].Value.ToString(), oRowGrid.Cells[otbTitleRedeem.Name].Value.ToString(), pbSelect);
                                    break;
                                }
                            }
                        }
                    }
                }
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "W_CHKxPmtSelect : " + oEx.Message); }
            finally
            {
                ocmCell = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        private void W_SETxGridColumns()
        {
            try
            {
                switch (nW_Mode)
                {
                    case 1:
                        ogdDetail.Columns[otbTitlePdtCode.Name].Visible = false;
                        ogdDetail.Columns[otbTitleQtyRfn.Name].Visible = false;
                        ogdDetail.Columns[otbTitleFactor.Name].Visible = false;
                        ogdDetail.Columns[otbTitleBarcode.Name].Visible = false;
                        ogdDetail.Columns[otbTitleRedeem.Name].Visible = false;
                        ogdDetail.Columns[otbTitlePmt.Name].Visible = false;

                        ogdDetail.Columns[otbTitleSeqNo.Name].FillWeight = 20;
                        ogdDetail.Columns[otbTitlePdtCode.Name].FillWeight = 48;
                        ogdDetail.Columns[otbTitlePdtName.Name].FillWeight = 68;
                        ogdDetail.Columns[otbTitleUnit.Name].FillWeight = 48;
                        ogdDetail.Columns[otbTitleSetPrice.Name].FillWeight = 28;
                        ogdDetail.Columns[otbTitleQty.Name].FillWeight = 34;
                        ogdDetail.Columns[otbTitleQtyRfn.Name].FillWeight = 34;
                        ogdDetail.Columns[otbTitleAmount.Name].FillWeight = 41;

                        ogdDetail.Columns[otbTitleSeqNo.Name].HeaderText = cVB.oVB_GBResource.GetString("tCS_Seq");
                        ogdDetail.Columns[otbTitlePdtCode.Name].HeaderText = oW_Resource.GetString("tTitlePdtCode");
                        ogdDetail.Columns[otbTitlePdtName.Name].HeaderText = oW_Resource.GetString("tTitlePdtName");
                        ogdDetail.Columns[otbTitleUnit.Name].HeaderText = oW_Resource.GetString("tTitleUnit");
                        ogdDetail.Columns[otbTitleSetPrice.Name].HeaderText = oW_Resource.GetString("tTitlePrice");
                        ogdDetail.Columns[otbTitleQty.Name].HeaderText = cVB.oVB_GBResource.GetString("tQty");
                        ogdDetail.Columns[otbTitleQtyRfn.Name].HeaderText = oW_Resource.GetString("tQtyRfn");
                        ogdDetail.Columns[otbTitleAmount.Name].HeaderText = oW_Resource.GetString("tAmt");

                        ogdDetail.Columns[otbTitleSeqNo.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitlePdtCode.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitlePdtName.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleUnit.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleSetPrice.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleQty.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleQtyRfn.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleAmount.Name].ReadOnly = true;

                        ogdDetail.Columns[otbTitleSeqNo.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        ogdDetail.Columns[otbTitleUnit.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        ogdDetail.Columns[otbTitleSetPrice.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        ogdDetail.Columns[otbTitleQty.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        ogdDetail.Columns[otbTitleQtyRfn.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        ogdDetail.Columns[otbTitleAmount.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        //*Em 63-06-06
                        ogdDetail.Columns[otbTitleSetPrice.Name].DefaultCellStyle.Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        ogdDetail.Columns[otbTitleQty.Name].DefaultCellStyle.Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        ogdDetail.Columns[otbTitleQtyRfn.Name].DefaultCellStyle.Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        ogdDetail.Columns[otbTitleAmount.Name].DefaultCellStyle.Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        break;
                    case 2:
                    case 3: //*Arm 63-06-10 ขายอ้างคืน
                        ogdDetail.Columns[otbTitleSeqNo.Name].Visible = false;
                        ogdDetail.Columns[otbTitleFactor.Name].Visible = false;
                        ogdDetail.Columns[otbTitleBarcode.Name].Visible = false;
                        ogdDetail.Columns[otbTitleRedeem.Name].Visible = false;
                        ogdDetail.Columns[otbTitlePmt.Name].Visible = false;
                        if (nW_Mode == 3) ogdDetail.Columns[otbTitleQtyRfn.Name].Visible = false; //*Arm 63-06-10

                        ogdDetail.Columns[otbTitlePdtCode.Name].FillWeight = 48;
                        ogdDetail.Columns[otbTitlePdtName.Name].FillWeight = 68;
                        ogdDetail.Columns[otbTitleUnit.Name].FillWeight = 48;
                        ogdDetail.Columns[otbTitleSetPrice.Name].FillWeight = 28;
                        ogdDetail.Columns[otbTitleQty.Name].FillWeight = 34;
                        ogdDetail.Columns[otbTitleQtyRfn.Name].FillWeight = 34;
                        ogdDetail.Columns[otbTitleAmount.Name].FillWeight = 41;

                        ogdDetail.Columns[otbTitlePdtCode.Name].HeaderText = oW_Resource.GetString("tTitlePdtCode");
                        ogdDetail.Columns[otbTitlePdtName.Name].HeaderText = oW_Resource.GetString("tTitlePdtName");
                        ogdDetail.Columns[otbTitleUnit.Name].HeaderText = oW_Resource.GetString("tTitleUnit");
                        ogdDetail.Columns[otbTitleSetPrice.Name].HeaderText = oW_Resource.GetString("tTitlePrice");
                        ogdDetail.Columns[otbTitleQtyRfn.Name].HeaderText = oW_Resource.GetString("tQtyRfn");
                        ogdDetail.Columns[otbTitleAmount.Name].HeaderText = oW_Resource.GetString("tAmt");
                        ogdDetail.Columns[ockTitleChoose.Name].HeaderText = oW_Resource.GetString("tChoose");

                        if (nW_Mode == 3) //*Arm 63-06-10
                        {
                            ogdDetail.Columns[otbTitleQty.Name].HeaderText = cVB.oVB_GBResource.GetString("tQty");
                        }
                        else
                        {
                            ogdDetail.Columns[otbTitleQty.Name].HeaderText = oW_Resource.GetString("tQty");
                        }

                        ogdDetail.Columns[otbTitlePdtCode.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitlePdtName.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleUnit.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleSetPrice.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleQty.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleQtyRfn.Name].ReadOnly = true;
                        ogdDetail.Columns[otbTitleAmount.Name].ReadOnly = true;


                        ogdDetail.Columns[otbTitleUnit.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        ogdDetail.Columns[otbTitleSetPrice.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        ogdDetail.Columns[otbTitleQty.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        ogdDetail.Columns[otbTitleQtyRfn.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        ogdDetail.Columns[otbTitleAmount.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        //*Em 63-06-06
                        ogdDetail.Columns[otbTitleSetPrice.Name].DefaultCellStyle.Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        ogdDetail.Columns[otbTitleQty.Name].DefaultCellStyle.Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        ogdDetail.Columns[otbTitleQtyRfn.Name].DefaultCellStyle.Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        ogdDetail.Columns[otbTitleAmount.Name].DefaultCellStyle.Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        break;
                }
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "W_SETxGridColumns : " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }
        #endregion End Function

        #region Method/Events

        public void ogdDetail_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //*Arm 63-01-08 
            try
            {
                if (ogdDetail.IsCurrentCellDirty)
                {
                    ogdDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wChooseItemRef", "ogdDetail_CurrentCellDirtyStateChanged : " + oEx.Message);
            }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Paint Background
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                using (SolidBrush oBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(oBrush, e.ClipRectangle);
                    oBrush.Dispose();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "OnPaintBackground " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            cmlPdtOrder oPdt = new cmlPdtOrder();
            int nCheck = 0;
            int[] anSeq = { };//*Em 63-05-07
            cDatabase oDB = new cDatabase();    //*Em 63-05-14
            StringBuilder oSql = new StringBuilder();   //*Em 63-05-14
            int nSelect = 0;
            int nDTCount = 0; //*Arm 63-06-10
            int nSelectQty = 0; //*Arm 63-09-17
            try
            {
                new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click : start", cVB.bVB_AlwPrnLog);
                //* Arm 63-02-07 - switch Case Mode การเรียกใช้
                switch (nW_Mode)
                {
                    case 1: // กด HotKey จากหน้า Sale
                        //*Arm 63-03-02 - แก้ไข 
                        if (nW_SelectRow < 0)
                        {

                        }
                        else
                        {
                            cSale.nC_DTSeqNo = Convert.ToInt32(ogdDetail.Rows[nW_SelectRow].Cells[otbTitleSeqNo.Name].Value);
                            cSale.cC_DTQty = Convert.ToDecimal(ogdDetail.Rows[nW_SelectRow].Cells[otbTitleQty.Name].Value);

                            cVB.oVB_PdtOrder = new cmlPdtOrder();
                            cVB.oVB_PdtOrder.tPdtCode = (string)(ogdDetail.Rows[nW_SelectRow].Cells[otbTitlePdtCode.Name].Value);
                            cVB.oVB_PdtOrder.tBarcode = (string)ogdDetail.Rows[nW_SelectRow].Cells[otbTitleBarcode.Name].Value;
                            cVB.oVB_PdtOrder.tPdtName = (string)ogdDetail.Rows[nW_SelectRow].Cells[otbTitlePdtName.Name].Value;
                            cVB.oVB_PdtOrder.cSetPrice = Convert.ToDecimal(ogdDetail.Rows[nW_SelectRow].Cells[otbTitleAmount.Name].Value);

                            cVB.oVB_OrderRowIndex = Convert.ToInt32(ogdDetail.Rows[nW_SelectRow].Cells[otbTitleSeqNo.Name].Value) - 1;
                            nCheck++;
                        }

                        if (nCheck > 0)
                        {
                            this.Close();
                        }
                        //++++++++++++++++

                        //if (ogdDetail.Rows.Count == 0)
                        //{ }
                        //else
                        //{
                        //    foreach (DataGridViewRow oRow in ogdDetail.Rows)
                        //    {
                        //        if (Convert.ToBoolean(oRow.Cells[ockTitleChoose.Name].Value) == true)
                        //        {
                        //            cSale.nC_DTSeqNo = Convert.ToInt32(oRow.Cells[otbTitleSeqNo.Name].Value);
                        //            cSale.cC_DTQty = Convert.ToDecimal(oRow.Cells[otbTitleQty.Name].Value);

                        //            cVB.oVB_PdtOrder = new cmlPdtOrder();
                        //            cVB.oVB_PdtOrder.tPdtCode = (string)(oRow.Cells[otbTitlePdtCode.Name].Value);
                        //            cVB.oVB_PdtOrder.tBarcode = (string)oRow.Cells[otbTitleBarcode.Name].Value;
                        //            cVB.oVB_PdtOrder.tPdtName = (string)oRow.Cells[otbTitlePdtName.Name].Value;
                        //            cVB.oVB_PdtOrder.cSetPrice = Convert.ToDecimal(oRow.Cells[otbTitleAmount.Name].Value);

                        //            cVB.oVB_OrderRowIndex = Convert.ToInt32(oRow.Cells[otbTitleSeqNo.Name].Value) - 1;
                        //            nCheck++;
                        //        }
                        //    }

                        //    if (nCheck > 0)
                        //    {
                        //        this.Close();
                        //    }
                        //}
                        break;

                    case 2: // คืนรายการขาย

                        if (ogdDetail.Rows.Count == 0)
                        { }
                        else
                        {
                            cVB.aoVB_PdtRefund = new List<cmlPdtOrder>();
                            cVB.atVB_PmtRefund = null;
                            cVB.bVB_RefundFullBill = false; //*Em 63-05-07
                            cVB.aoVB_PdtDisChgRefund = new List<cmlPdtDisChg>();

                            new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click : start check select", cVB.bVB_AlwPrnLog);

                            oSql.Clear();
                            oSql.AppendLine("TRUNCATE TABLE " + cSale.tC_TblRefund);
                            oSql.AppendLine();
                            oSql.AppendLine("INSERT INTO TSRF"+cVB.tVB_PosCode+" (FNXsdSeqNo,FNXsdSeqNoOld,FCXsdQty,FCXsdQtyRfn)");
                            oSql.AppendLine("VALUES");
                            foreach (DataGridViewRow oRow in ogdDetail.Rows)
                            {
                                if (Convert.ToBoolean(oRow.Cells[ockTitleChoose.Name].Value) == true)
                                {
                                    //oPdt = new cmlPdtOrder();
                                    //oPdt.tPdtCode = (string)oRow.Cells[otbTitlePdtCode.Name].Value;
                                    ////oPdt.tPdtName = (string)oRow.Cells[otbTitlePdtName.Name].Value;
                                    ////oPdt.tBarcode = (string)oRow.Cells[otbTitleBarcode.Name].Value;
                                    ////oPdt.tUnit = (string)oRow.Cells[otbTitleUnit.Name].Value;
                                    ////oPdt.cSetPrice = Convert.ToDecimal(oRow.Cells[otbTitleSetPrice.Name].Value);
                                    //////oPdt.cQty =  Convert.ToDecimal(oRow.Cells[otbTitleQty.Name].Value);      //*ARM 62-12-19  [Comment Code]
                                    //oPdt.cQty = Convert.ToDecimal(oRow.Cells[otbTitleQtyRfn.Name].Value);      //*ARM 62-12-19
                                    ////oPdt.cFactor = Convert.ToDecimal(oRow.Cells[otbTitleFactor.Name].Value);
                                    ////oPdt.tStaPdt = "1";
                                    //cVB.aoVB_PdtRefund.Add(oPdt);

                                    ////*Arm 63-03-20 
                                    //foreach (cmlPdtDisChg oData in W_CHKxDisChg(Convert.ToInt32(oRow.Cells[otbTitleSeqNo.Name].Value), Convert.ToDecimal(oRow.Cells[otbTitleQty.Name].Value), Convert.ToDecimal(oRow.Cells[otbTitleQtyRfn.Name].Value)))
                                    //{
                                    //    cVB.aoVB_PdtDisChgRefund.Add(oData);
                                    //}
                                    //// +++++++++++++

                                    ////*Em 63-05-07
                                    //Array.Resize<int>(ref anSeq, anSeq.Length + 1);
                                    //anSeq[anSeq.Length - 1] = Convert.ToInt32(oRow.Cells[otbTitleSeqNo.Name].Value);
                                    ////+++++++++++++++++++

                                    //*Em 63-05-14
                                    nSelect++;
                                    if (nSelect == 1)
                                    {
                                        oSql.AppendLine("(" + nSelect + "," + oRow.Cells[otbTitleSeqNo.Name].Value + "," + oRow.Cells[otbTitleQty.Name].Value + "," + oRow.Cells[otbTitleQtyRfn.Name].Value + ")");
                                    }
                                    else
                                    {
                                        oSql.AppendLine(",(" + nSelect + "," + oRow.Cells[otbTitleSeqNo.Name].Value + "," + oRow.Cells[otbTitleQty.Name].Value + "," + oRow.Cells[otbTitleQtyRfn.Name].Value + ")");
                                    }
                                    
                                    //+++++++++++++
                                }

                            }
                            if (nSelect > 0) oDB.C_SETxDataQuery(oSql.ToString());

                            new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click : end check select", cVB.bVB_AlwPrnLog);

                            //*Arm 63-06-10 จำนวนรายการทั้งหมดของบิลนี้
                            nDTCount = 0;
                            //oSql.Clear();
                            //oSql.AppendLine("SELECT Count(*) AS nCount FROM " + cSale.tC_Ref_TblSalDT + " with(nolock)");
                            ////oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                            ////oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                            //oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "' "); //*Arm 63-09-11 รองรับการอ้างอิงบิลข้ามสาขาได้
                            //oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                            //nDTCount = oDB.C_GEToDataQuery<int>(oSql.ToString());
                            
                            //*Arm 63-09-17 หาจำนวนสินค้าจากบิลขายทั้งหมด
                            oSql.Clear();
                            oSql.AppendLine("SELECT SUM(FCXsdQtyAll) AS nCount FROM " + cSale.tC_Ref_TblSalDT + " with(nolock)");
                            oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "' "); //*Arm 63-09-11 รองรับการอ้างอิงบิลข้ามสาขาได้
                            oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                            nDTCount = oDB.C_GEToDataQuery<int>(oSql.ToString());
                            //จำนวนที่คืน
                            nSelectQty = ogdDetail.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToInt32(oSum.Cells[otbTitleQtyRfn.Name].Value));
                            //++++++++++++

                            if (nSelect == 0) return;
                            //if (nSelect == ogdDetail.RowCount && nSelect == nDTCount) cVB.bVB_RefundFullBill = true;
                            if (nSelect == ogdDetail.RowCount && nSelectQty == nDTCount) cVB.bVB_RefundFullBill = true; //*Arm 63-09-17
                            //+++++++++++++

                            ////if (cVB.aoVB_PdtRefund.Count == 0) return;
                            //if (nSelect == 0) return;  //*Em 63-05-07
                            //if (nSelect == ogdDetail.RowCount) cVB.bVB_RefundFullBill = true; //*Em 63-05-07

                            ////*Arm 63-03-20 
                            //W_CHKxDisChgRedeem();
                            //// +++++++++++++

                            new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click : start W_PRCxRefund", cVB.bVB_AlwPrnLog);
                            // ย้าย by Zen 28-04-2020
                            //cVB.oVB_Sale.W_PRCxRefund();    //*Em 63-04-23
                            //new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click : end W_PRCxRefund", cVB.bVB_AlwPrnLog);

                            //*Em 63-05-12
                            //wReason oReason;
                            //oReason = new wReason("003");
                            //oReason.ShowDialog();

                            if (cVB.oVB_Sale.W_PRCxRefund() == true) //*Arm63-08-17
                            {
                                new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click : end W_PRCxRefund", cVB.bVB_AlwPrnLog);

                                //*Em 63-05-12
                                wReason oReason;
                                oReason = new wReason("003");
                                oReason.ShowDialog();
                            }
                            else
                            {
                                return;
                            }
                        }
                        this.Close();
                        break;

                    case 3:
                        oSql.Clear();
                        oSql.AppendLine("TRUNCATE TABLE " + cSale.tC_TblRefund);
                        oSql.AppendLine();
                        oSql.AppendLine("INSERT INTO " + cSale.tC_TblRefund + "(FNXsdSeqNo,FNXsdSeqNoOld,FCXsdQty,FCXsdQtyRfn)");
                        oSql.AppendLine("VALUES");
                        foreach (DataGridViewRow oRow in ogdDetail.Rows)
                        {
                            if (Convert.ToBoolean(oRow.Cells[ockTitleChoose.Name].Value) == true)
                            {
                                nSelect++;
                                if (nSelect == 1)
                                {
                                    oSql.AppendLine("(" + nSelect + "," + oRow.Cells[otbTitleSeqNo.Name].Value + "," + oRow.Cells[otbTitleQty.Name].Value + "," + oRow.Cells[otbTitleQtyRfn.Name].Value + ")");
                                }
                                else
                                {
                                    oSql.AppendLine(",(" + nSelect + "," + oRow.Cells[otbTitleSeqNo.Name].Value + "," + oRow.Cells[otbTitleQty.Name].Value + "," + oRow.Cells[otbTitleQtyRfn.Name].Value + ")");
                                }
                            }
                        }
                        if (nSelect > 0) oDB.C_SETxDataQuery(oSql.ToString());

                        if (nSelect > 0)
                        {
                            new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click : end check select", cVB.bVB_AlwPrnLog);
                            nDTCount = 0;
                            oSql.Clear();
                            oSql.AppendLine("SELECT Count(*) AS nCount FROM " + cSale.tC_Ref_TblSalDT + " with(nolock)");
                            //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                            //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                            oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "' "); //*Arm 63-09-11 รองรับการอ้างอิงบิลข้ามสาขาได้
                            oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                            nDTCount = oDB.C_GEToDataQuery<int>(oSql.ToString());
                            
                            if (nSelect == ogdDetail.RowCount && nSelect == nDTCount) cVB.bVB_RefundFullBill = true;
                            else cVB.bVB_RefundFullBill = false;

                            cVB.oVB_Sale.W_PRCxReferBill();
                        }
                        this.Close();
                        break;
                }
                new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click : End", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "ocmAccept_Click " + oEx.Message); }
            finally
            {
                oPdt = null;
                oSql = null;
                oDB = null;
                //oW_SP.SP_CLExMemory();
            }
        }
        
        /// <summary>
        /// *Arm 63-03-23
        ///  ส่วนลดรายการ/ท้ายบิล
        /// </summary>
        public List<cmlPdtDisChg> W_CHKxDisChg(int pnSeqNo, decimal pcQty, decimal pcQtyRfn)
        {
            StringBuilder oSql = new StringBuilder();
            List<cmlPdtDisChg> oPdtDisChg = new List<cmlPdtDisChg>();
            
            try
            {
                //*Arm 63-03-23
                oSql.AppendLine("SELECT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice,");
                oSql.AppendLine("DTDis.FNXddStaDis AS pnStaDis, DTDis.FTXddDisChgType AS ptDisChgType, DTDis.FCXddNet AS pcNet,  ");
                oSql.AppendLine("(ISNULL(DTDis.FCXddValue,0.00)/"+ pcQty + ") * "+ pcQtyRfn + " AS pcValue, DTDis.FTXddRefCode AS ptRefCode");
                oSql.AppendLine("FROM TPSTSalDTDis DTDis WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND DTDis.FNXsdSeqNo = '" + pnSeqNo + "'");
                //oSql.AppendLine("AND ISNULL(DTDis.FTXddRefCode,'') =''"); //*Arm 63-03-23
                oSql.AppendLine("ORDER BY DTDis.FDXddDateIns ASC");
                oPdtDisChg = new cDatabase().C_GETaDataQuery<cmlPdtDisChg>(oSql.ToString());
                //+++++++++++++
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChooseItemRef", "W_CHKxDisChg " + oEx.Message);
            }
            finally
            {
                oSql = null;
                //oW_SP.SP_CLExMemory();
            }
            return oPdtDisChg;
        }

        /// <summary>
        /// *Arm 63-03-20
        ///  ส่วนลดจากการแลกแต้ม
        /// </summary>
        public void W_CHKxDisChgRedeem()
        {
            StringBuilder oSql = new StringBuilder();

            try
            {
                
                cVB.aoVB_PdtRdDocType1 = new List<cmlPdtRedeem>();
                cVB.aoVB_PdtRdDocType2 = new List<cmlPdtRedeem>();

                if (ogdDetail.Rows.Count == 0)
                {

                }
                else
                {
                    // 1: แลกแต้มส่วนลด DocType 1
                    int ni = 0;
                    oSql.Clear();
                    oSql.AppendLine("SELECT DISTINCT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice, ");
                    oSql.AppendLine("DTDis.FTBchCode AS ptBchCode, DTDis.FTXshDocNo AS tDocNo, DTDis.FNXddStaDis AS pnStaDis,DTDis.FTXddDisChgType AS ptDisChgType,DTDis.FCXddValue AS pcValue, DTDis.FTXddRefCode AS ptRefCode,");
                    oSql.AppendLine("RD.FTRdhDocType AS ptDocType, RD.FNXrdPntUse AS pnUsePnt, RD.FCXrdPdtQty AS pcPdtQty");
                    oSql.AppendLine("FROM TPSTSalDTDis DTDis WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                    oSql.AppendLine("INNER JOIN TPSTSalRD RD WITH(NOLOCK) ON RD.FTBchCode = DTDis.FTBchCode AND RD.FTXshDocNo = DTDis.FTXshDocNo AND RD.FTXrdRefCode = DTDis.FTXddRefCode  AND RD.FNXrdRefSeq = DTDis.FNXsdSeqNo");
                    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("AND DTDis.FNXddStaDis = '2'");
                    oSql.AppendLine("AND ISNULL(DTDis.FTXddRefCode,'') != ''");
                    oSql.AppendLine("AND ( ");
                    foreach (DataGridViewRow oRow in ogdDetail.Rows)
                    {
                        if (Convert.ToBoolean(oRow.Cells[ockTitleChoose.Name].Value) == true)
                        {

                            if (ni == 0)
                            {
                                oSql.AppendLine("DTDis.FNXsdSeqNo = '" + oRow.Cells[otbTitleSeqNo.Name].Value + "'");
                            }
                            else
                            {
                                oSql.AppendLine("OR DTDis.FNXsdSeqNo = '" + oRow.Cells[otbTitleSeqNo.Name].Value + "'");
                            }

                            ni++;
                        }
                    }
                    oSql.AppendLine(") ");
                    oSql.AppendLine("AND RD.FTRdhDocType = '1'");
                    cVB.aoVB_PdtRdDocType1 = new cDatabase().C_GETaDataQuery<cmlPdtRedeem>(oSql.ToString());
                    
                    // 2: แลกแต้มส่วนลด DocType 2
                    int ni2 = 0;
                    oSql.Clear();
                    oSql.AppendLine("SELECT DISTINCT RD.FTBchCode AS ptBchCode, RD.FTXshDocNo AS tDocNo, RD.FTRdhDocType AS ptDocType, RD.FNXrdPntUse AS pnUsePnt,");
                    oSql.AppendLine("DTDis.FTXddRefCode AS ptRefCode, DTDis.FNXddStaDis AS pnStaDis,DTDis.FTXddDisChgType AS ptDisChgType, SUM(DTDis.FCXddValue) AS pcUseMny");
                    oSql.AppendLine("FROM TPSTSalRD RD WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TPSTSalDTDis DTDis WITH(NOLOCK) ON RD.FTBchCode = DTDis.FTBchCode AND RD.FTXshDocNo = DTDis.FTXshDocNo AND RD.FTXrdRefCode = DTDis.FTXddRefCode");
                    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("AND DTDis.FNXddStaDis = '2'");
                    oSql.AppendLine("AND ISNULL(DTDis.FTXddRefCode,'') != ''");
                    oSql.AppendLine("AND(");
                    foreach (DataGridViewRow oRow in ogdDetail.Rows)
                    {
                        if (Convert.ToBoolean(oRow.Cells[ockTitleChoose.Name].Value) == true)
                        {

                            if (ni2 == 0)
                            {
                                oSql.AppendLine("DTDis.FNXsdSeqNo = '" + oRow.Cells[otbTitleSeqNo.Name].Value + "'");
                            }
                            else
                            {
                                oSql.AppendLine("OR DTDis.FNXsdSeqNo = '" + oRow.Cells[otbTitleSeqNo.Name].Value + "'");
                            }

                            ni2++;
                        }
                    }
                    oSql.AppendLine(")");
                    oSql.AppendLine("AND RD.FNXrdRefSeq = '0'");
                    oSql.AppendLine("AND RD.FTRdhDocType = '2'");
                    oSql.AppendLine("GROUP BY RD.FTBchCode,RD.FTXshDocNo, RD.FNXrdSeqNo, DTDis.FTXddRefCode, DTDis.FNXddStaDis, DTDis.FTXddDisChgType, RD.FTRdhDocType,RD.FNXrdPntUse");
                    cVB.aoVB_PdtRdDocType2 = new cDatabase().C_GETaDataQuery<cmlPdtRedeem>(oSql.ToString());
 
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChooseItemRef", "W_CHKxDisChgRedeem " + oEx.Message);
            }
            finally
            {
                oSql = null;
                //oW_SP.SP_CLExMemory();
            }
        }
        
        #endregion End Method/Events

        private void wChooseItemRef_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef_FormClosing " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }
        
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                //* Arm 63-02-07
                if (nW_Mode == 1) 
                {
                    cSale.nC_DTSeqNo = 0;
                }
                // ++++++++++
                if (nW_Mode == 2 || nW_Mode == 3) //*Arm 63-06-10 เพิ่มเช็ค nW_Mode = 3
                {
                    cVB.tVB_RefDocNo = ""; //*Arm 63-05-25
                    
                    //*Arm 63-06-10
                    cSale.tC_Ref_TblSalHD = "";
                    cSale.tC_Ref_TblSalHDDis = "";
                    cSale.tC_Ref_TblSalHDCst = "";
                    cSale.tC_Ref_TblSalDT = "";
                    cSale.tC_Ref_TblSalDTDis = "";
                    cSale.tC_Ref_TblSalRC = "";
                    cSale.tC_Ref_TblSalRD = "";
                    cSale.tC_Ref_TblSalPD = "";
                    cSale.tC_Ref_TblTxnSale = "";
                    cSale.tC_Ref_TblTxnRedeem = "";
                    //++++++++++++++
                    cSale.C_PRCxClearTmpRef(); //*Arm 63-09-18
                }
                
                this.Close();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wChooseItemRef", "ocmBack_Click : " + ex.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }
        
        private void ogdDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridViewDisableButtonCell buttonCell;
            //DataGridViewCheckBoxCell checkCell;
            DataGridViewDisableButtonCell ocmCell;
            DataGridViewCheckBoxCell ockCell;
            try
            {
                if (ogdDetail.CurrentRow == null)
                {
                    return;
                }
                ogdDetail.Enabled = false;  //*Em 63-07-11
                //* Arm 63-02-07 - switch Case Check Mode การเรียกใช้
                switch (nW_Mode)
                {
                    case 1:  // กด HotKey จากหน้า Sale
                        if (ogdDetail.Columns[ockTitleChoose.Name] != null) //*Net 63-07-06
                        {
                            if (e.ColumnIndex == ogdDetail.Columns[ockTitleChoose.Name].Index)
                            {
                                foreach (DataGridViewRow oRow in ogdDetail.Rows)
                                {
                                    oRow.Cells[ockTitleChoose.Name].Value = "false";
                                }
                                ogdDetail.CurrentRow.Cells[ockTitleChoose.Name].Value = "true";
                            }
                        }
                        break;

                    case 2: // คืนรายการขาย
                        wEnterAmount oFrm = new wEnterAmount(2);
                        if (ogdDetail.Columns["ockTitleChoose"] != null)
                        {
                            //*Arm 63-03-25 Check รายการที่มีการใช้ Redeem
                            if (e.ColumnIndex == ogdDetail.Columns["ockTitleChoose"].Index)
                            {
                                if (nW_ChkPayRedeem == 1) //* มีการชำระเงินด้วยการแลกแต้ม
                                {
                                    for (int ni = 0; ni < ogdDetail.Rows.Count; ni++)
                                    {
                                        if (Convert.ToBoolean(ogdDetail.CurrentRow.Cells[ockTitleChoose.Name].Value) == false)
                                        {
                                            ogdDetail.Rows[ni].Cells[ockTitleChoose.Name].Value = false;
                                        }
                                        else
                                        {
                                            ogdDetail.Rows[ni].Cells[ockTitleChoose.Name].Value = true;
                                        }
                                        //ปิดปุ่มแก้ไขจำนวนสินค้า
                                        //buttonCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[ni].Cells["ocmEditQtyRfn"];
                                        //checkCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[ni].Cells["ockTitleChoose"];
                                        //buttonCell.Enabled = false;
                                        ocmCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[ni].Cells["ocmEditQtyRfn"];
                                        ockCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[ni].Cells["ockTitleChoose"];
                                        ocmCell.Enabled = false;
                                    }

                                }
                                else  //* ไม่มีการชำระเงินด้วยการแลกแต้ม
                                {
                                    //switch (ogdDetail.CurrentRow.Cells[otbTitleRedeem.Name].Value.ToString()) //* เช็คแต่ละมีการแลกแต้มแบบไหน 0:ปกติ 1:ใช้ Coupon อย่างเดียว, 2:ใช้แต้มแลกสินค้าอย่างเดียว 3:ใช้แต้มแลกส่วนลดท้ายบิลอย่างเดี๋ยว หรือใช้ Couponfh ด้วย หรือ ใช้แต้มแลกสินค้าด้วย
                                    //{
                                    //    case "0":
                                    //        //เปิดปุ่ม แก้ไขจำนวนสินค้าได้
                                    //        //buttonCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                    //        //checkCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                    //        //buttonCell.Enabled = (Boolean)checkCell.Value;
                                    //        ocmCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                    //        ockCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                    //        ocmCell.Enabled = (Boolean)ockCell.Value;
                                    //        ogdDetail.Invalidate();
                                    //        break;

                                    //    case "1":
                                    //        foreach (DataGridViewRow oRow in ogdDetail.Rows) //เลือกหรือไม่เลือก รายการทั้งหมดที่สถานะ Redeem =1
                                    //        {
                                    //            if (oRow.Cells[otbTitleRedeem.Name].Value.ToString() == "1" )
                                    //            {
                                    //                if (Convert.ToBoolean(ogdDetail.CurrentRow.Cells[ockTitleChoose.Name].Value) == false)
                                    //                {
                                    //                    oRow.Cells[ockTitleChoose.Name].Value = false;
                                    //                }
                                    //                else
                                    //                {
                                    //                    oRow.Cells[ockTitleChoose.Name].Value = true;
                                    //                }
                                    //            }
                                    //            //ปิดปุ่มแก้ไขจำนวนสินค้า
                                    //            //buttonCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                    //            //checkCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                    //            //buttonCell.Enabled = false;
                                    //            ocmCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                    //            ockCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                    //            ocmCell.Enabled = false;
                                    //            ogdDetail.Invalidate();
                                    //        }

                                    //        break;
                                    //    case "2":
                                    //        //ปิดปุ่มแก้ไขจำนวนสินค้า สถานะ Redeem =2
                                    //        //buttonCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                    //        //checkCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                    //        //buttonCell.Enabled = false;
                                    //        ocmCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                    //        ockCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                    //        ocmCell.Enabled = false;
                                    //        ogdDetail.Invalidate();
                                    //        break;

                                    //    case "3":
                                    //        bool bChkSta = Convert.ToBoolean(ogdDetail.CurrentRow.Cells[ockTitleChoose.Name].Value); //*Arm 63-04-29

                                    //        foreach (DataGridViewRow oRow in ogdDetail.Rows)
                                    //        {
                                    //            if (oRow.Cells[otbTitleRedeem.Name].Value.ToString() == "3") //เลือกหรือไม่เลือก รายการทั้งหมดที่สถานะ Redeem =3 
                                    //            {
                                    //                //if (Convert.ToBoolean(ogdDetail.CurrentRow.Cells[ockTitleChoose.Name].Value) == false)
                                    //                if(bChkSta == true) //*Arm 63-04-29
                                    //                {
                                    //                    oRow.Cells[ockTitleChoose.Name].Value = false;
                                    //                }
                                    //                else
                                    //                {
                                    //                    oRow.Cells[ockTitleChoose.Name].Value = true;
                                    //                }
                                    //            }
                                    //            //ปิดปุ่มแก้ไขจำนวนสินค้า
                                    //            //buttonCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                    //            //checkCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                    //            //buttonCell.Enabled = false;
                                    //            ocmCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                    //            ockCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                    //            ocmCell.Enabled = false;
                                    //            ogdDetail.Invalidate();
                                    //        }
                                    //        break;
                                    //}
                                    DataGridViewRow oRow2 = ogdDetail.Rows[e.RowIndex];
                                    if (Convert.ToBoolean(oRow2.Cells[ockTitleChoose.Name].Value) && (!string.IsNullOrEmpty(oRow2.Cells[otbTitlePmt.Name].Value.ToString()) || !string.IsNullOrEmpty(oRow2.Cells[otbTitleRedeem.Name].Value.ToString())))
                                    {
                                        ocmCell = (DataGridViewDisableButtonCell)oRow2.Cells["ocmEditQtyRfn"];
                                        ocmCell.Enabled = false;
                                        ogdDetail.Invalidate();
                                    }

                                    W_CHKxPmtSelect(oRow2.Cells[otbTitlePmt.Name].Value.ToString(), oRow2.Cells[otbTitleRedeem.Name].Value.ToString(), Convert.ToBoolean(oRow2.Cells[ockTitleChoose.Name].Value));  //*Em 63-04-17
                                }


                                //+++++++++++++

                                //*Arm 63-03-25 [comment code]
                                ////*Arm 63-03-19
                                //W_CHKxPdtRedeem(e.RowIndex);
                                ////++++++++++++++

                                //if (nW_ChkPayRedeem == 0 && nW_EditQty == 1)
                                //{
                                //    DataGridViewDisableButtonCell buttonCell = (DataGridViewDisableButtonCell)ogdDetail.Rows[e.RowIndex].Cells["ocmEditQtyRfn"];
                                //    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)ogdDetail.Rows[e.RowIndex].Cells["ockTitleChoose"];
                                //    buttonCell.Enabled = (Boolean)checkCell.Value;
                                //    ogdDetail.Invalidate();
                                //}
                                //++++++++++++++
                            }
                        }
                        if (ogdDetail.Columns["ocmEditQtyRfn"] != null)
                        {
                            if (e.ColumnIndex == ogdDetail.Columns["ocmEditQtyRfn"].Index && Convert.ToBoolean(ogdDetail.CurrentRow.Cells[ockTitleChoose.Name].Value) == true && ogdDetail.CurrentRow.Cells[otbTitleRedeem.Name].Value.ToString() == "" && nW_ChkPayRedeem == 0)
                            {
                                oFrm.ShowDialog();

                                if (!string.IsNullOrEmpty(oFrm.otbAmount.Text))
                                {
                                    if (Convert.ToDecimal(oFrm.otbAmount.Text) <= Convert.ToDecimal(ogdDetail.CurrentRow.Cells[otbTitleQty.Name].Value))   // ตรวจสอบการระบุจำนวนคืน ต้องไม่มากกว่าจำนวนที่เหลือ
                                    {
                                        ogdDetail.CurrentRow.Cells[otbTitleQtyRfn.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oFrm.otbAmount.Text), cVB.nVB_DecShow);
                                        ogdDetail.CurrentRow.Cells[otbTitleAmount.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oFrm.otbAmount.Text) * Convert.ToDecimal(ogdDetail.CurrentRow.Cells[otbTitleSetPrice.Name].Value), cVB.nVB_DecShow);

                                        if (Convert.ToDecimal(oFrm.otbAmount.Text) == 0)
                                        {
                                            ogdDetail.CurrentRow.Cells[otbTitleQtyRfn.Name].Value = Convert.ToDecimal(ogdDetail.CurrentRow.Cells[otbTitleQty.Name].Value);
                                            ogdDetail.CurrentRow.Cells[otbTitleAmount.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.CurrentRow.Cells[otbTitleQty.Name].Value) * Convert.ToDecimal(ogdDetail.CurrentRow.Cells[otbTitleSetPrice.Name].Value), cVB.nVB_DecShow);
                                            ogdDetail.CurrentRow.Cells[ockTitleChoose.Name].Value = false;
                                            return;
                                        }

                                    }
                                    else
                                    {
                                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgErrQtyRfn"), 2);
                                        ogdDetail.CurrentRow.Cells[otbTitleQtyRfn.Name].Value = Convert.ToDecimal(ogdDetail.CurrentRow.Cells[otbTitleQty.Name].Value);
                                        ogdDetail.CurrentRow.Cells[otbTitleAmount.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.CurrentRow.Cells[otbTitleQty.Name].Value) * Convert.ToDecimal(ogdDetail.CurrentRow.Cells[otbTitleSetPrice.Name].Value), cVB.nVB_DecShow);
                                    }

                                    ogdDetail.CurrentRow.Cells[ockTitleChoose.Name].Value = true;
                                }
                                oFrm.Dispose();
                            }
                        }
                        break;
                }
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChooseItemRef", "ogdDetail_CellContentClick : " + oEx.Message);
            }
            finally
            {
                //oW_SP.SP_CLExMemory();
                ogdDetail.Enabled = true;  //*Em 63-07-11
            }
        }

        private void ogdDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //*Arm 63-03-02 - แก้ไข Baseline
                if (nW_Mode == 1)
                {
                    if (e.ColumnIndex < 0) return;
                    if (e.RowIndex < 0) return;
                    nW_SelectRow = (int)e.RowIndex;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wChooseItemRef", "ogdDetail_CellMouseClick : " + oEx.Message);
            }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }

        //*Net 63-03-02
        private void wChooseItemRef_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        this.Close();
                        this.Dispose();
                        break;
                    case Keys.F10:
                        ocmAccept_Click(ocmAccept, new EventArgs());
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef_KeyDown : " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }

        private void ogdDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           if(nW_Mode == 1) ocmAccept_Click(ocmAccept, new EventArgs()); //*Net 63-03-26
        }

        private void ockSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (nW_ChkPayRedeem == 1) //* มีการชำระเงินด้วยการแลกแต้ม
                //{
                //    foreach (DataGridViewRow oRow in ogdDetail.Rows)
                //    {
                //        ((DataGridViewCheckBoxCell)oRow.Cells["ockTitleChoose"]).Value = ockSelectAll.Checked;
                //        ((DataGridViewDisableButtonCell)oRow.Cells["ocmEditQtyRfn"]).Enabled = false;
                //    }
                //}
                //else  //* ไม่มีการชำระเงินด้วยการแลกแต้ม
                //{
                //    foreach (DataGridViewRow oRow in ogdDetail.Rows)
                //    {
                //        ((DataGridViewCheckBoxCell)oRow.Cells["ockTitleChoose"]).Value = ockSelectAll.Checked;
                //        switch (oRow.Cells[otbTitleRedeem.Name].Value.ToString()) //* เช็คแต่ละมีการแลกแต้มแบบไหน 0:ปกติ 1:ใช้ Coupon อย่างเดียว, 2:ใช้แต้มแลกสินค้าอย่างเดียว 3:ใช้แต้มแลกส่วนลดท้ายบิลอย่างเดี๋ยว หรือใช้ Couponfh ด้วย หรือ ใช้แต้มแลกสินค้าด้วย
                //        {
                //            case "0":
                //                //เปิดปุ่ม แก้ไขจำนวนสินค้าได้
                //                ((DataGridViewDisableButtonCell)oRow.Cells["ocmEditQtyRfn"]).Enabled = ockSelectAll.Checked;
                //                ogdDetail.Invalidate();
                //                break;

                //            default:
                //                ((DataGridViewDisableButtonCell)oRow.Cells["ocmEditQtyRfn"]).Enabled = false;
                //                ogdDetail.Invalidate();
                //                break;
                //        }
                //    }
                //}


                //*Arm 63-06-10  Start
                switch(nW_Mode) 
                {
                    case 2:
                        if (nW_ChkPayRedeem == 1) //* มีการชำระเงินด้วยการแลกแต้ม
                        {
                            foreach (DataGridViewRow oRow in ogdDetail.Rows)
                            {
                                ((DataGridViewCheckBoxCell)oRow.Cells["ockTitleChoose"]).Value = ockSelectAll.Checked;
                                ((DataGridViewDisableButtonCell)oRow.Cells["ocmEditQtyRfn"]).Enabled = false;
                                oRow.Cells[ockTitleChoose.Name].Value = ockSelectAll.Checked;
                            }
                            ogdDetail.Refresh();
                        }
                        else  //* ไม่มีการชำระเงินด้วยการแลกแต้ม
                        {
                            foreach (DataGridViewRow oRow in ogdDetail.Rows)
                            {
                                ((DataGridViewCheckBoxCell)oRow.Cells["ockTitleChoose"]).Value = ockSelectAll.Checked;
                                switch (oRow.Cells[otbTitleRedeem.Name].Value.ToString()) //* เช็คแต่ละมีการแลกแต้มแบบไหน 0:ปกติ 1:ใช้ Coupon อย่างเดียว, 2:ใช้แต้มแลกสินค้าอย่างเดียว 3:ใช้แต้มแลกส่วนลดท้ายบิลอย่างเดี๋ยว หรือใช้ Couponfh ด้วย หรือ ใช้แต้มแลกสินค้าด้วย
                                {
                                    case "0":
                                        //เปิดปุ่ม แก้ไขจำนวนสินค้าได้
                                        ((DataGridViewDisableButtonCell)oRow.Cells["ocmEditQtyRfn"]).Enabled = ockSelectAll.Checked;
                                        ogdDetail.Invalidate();
                                        break;

                                    default:
                                        ((DataGridViewDisableButtonCell)oRow.Cells["ocmEditQtyRfn"]).Enabled = false;
                                        ogdDetail.Invalidate();
                                        break;
                                }
                            }
                        }

                        break;
                    case 3:
                        foreach (DataGridViewRow oRow in ogdDetail.Rows)
                        {
                            ((DataGridViewCheckBoxCell)oRow.Cells["ockTitleChoose"]).Value = ockSelectAll.Checked;
                        }
                        break;
                }
                //*Arm 63-06-10  End
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "ockSelectAll_CheckedChanged : " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }

        private void wChooseItemRef_Shown(object sender, EventArgs e)
        {
            Form oFormShow = null;
            try
            {
                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wReferBill);
                if (oFormShow != null) oFormShow.Hide();

                //*Em 63-07-11
                if (nW_Mode == 2 && nW_ChkPayRedeem == 1)
                {
                    ockSelectAll.Checked = true;
                    ockSelectAll_CheckedChanged(ockSelectAll, new EventArgs());
                    ockSelectAll.Enabled = false;
                    ogdDetail.Enabled = false;
                }
                //+++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChooseItemRef", "wChooseItemRef_Shown : " + oEx.Message); }
            finally
            {
                //oW_SP.SP_CLExMemory();
            }
        }
    }

    //*Arm 62-01-08
    //Class Disble For DataGridViewButtonColumn
    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }

    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;
        public bool Enabled
        {
            get
            {
                return enabledValue;
            }
            set
            {
                enabledValue = value;
            }
        }

        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewDisableButtonCell cell = (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the button cell.
        public DataGridViewDisableButtonCell()
        {
            this.enabledValue = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,  
            // background, and disabled button for the cell.
            if (!this.enabledValue)
            {
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground = new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified.
                if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,advancedBorderStyle);
                }

                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment = this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                // Draw the disabled button.                
                ButtonRenderer.DrawButton(graphics, buttonArea, PushButtonState.Disabled);

                // Draw the disabled button text. 
                if (this.FormattedValue is String)
                {
                    TextRenderer.DrawText(graphics,(string)this.FormattedValue,this.DataGridView.Font,buttonArea, SystemColors.GrayText);
                }
            }
            else
            {
                // The button cell is enabled, so let the base class 
                // handle the painting.
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
}
