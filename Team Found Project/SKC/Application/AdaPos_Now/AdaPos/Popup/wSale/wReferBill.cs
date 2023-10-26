using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.DatabaseTmp;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Required.SaleDocRefer;
using AdaPos.Models.Webservice.Respond.SaleDocRefer;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wSale
{
    public partial class wReferBill : Form
    {
        #region Variable

        private ResourceManager oW_Resource;
        public static string tVB_StatusReBill;
        private int nW_RefMode;    //*Arm 63-06-10 Mode 1:ขายอ้างอิงบิลคืน, 2:คืนอ้างอิงขาย
        #endregion End Variable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnRefMode">Modem ที่เรียกใช้งาน 1:ขายอ้างอิงบิลคืน, 2:คืนอ้างอิงขาย (*Arm 63-06-10)</param>
        public wReferBill(int pnRefMode = 2)
        {
            InitializeComponent();

            try
            {
                nW_RefMode = pnRefMode; //*Arm 63-06-10

                W_SETxDesign();
                W_SETxText();
                tVB_StatusReBill = "1";
                ockRefer.Checked = true;    //*Arm 62-12-26 -แก้ไขกดคืนสินค้าให้ Defualt Check อ้างอิงบิล
               
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "wReferBill : " + oEx.Message); }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                //ocmAccept.BackColor = cVB.oVB_ColNormal;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ockRefer.Enabled = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set tex form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resSale_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSale_EN));
                        break;
                }

                //cVB.tVB_KbdScreen = "RETURN";
                
                // Menu
                if (nW_RefMode == 1) //*Arm 63-06-10
                {
                    olaTitleReferBill.Text = oW_Resource.GetString("tTitleReferBillRtn");
                }
                else
                {
                    olaTitleReferBill.Text = oW_Resource.GetString("tTitleReferBill");
                }
                otpSaleDate.Text = cVB.tVB_SaleDate; //*Arm 63-06-10

                ockRefer.Text = oW_Resource.GetString("tRefer");
                olaTitleWB.Text = cVB.oVB_GBResource.GetString("tWristbandNo");
                olaTitleDocNo.Text = cVB.oVB_GBResource.GetString("tDocNo");

                olaWristband.Text = cVB.oVB_GBResource.GetString("tWristband");     //*Arm 62-12-20
                olaDocument.Text = cVB.oVB_GBResource.GetString("tDoc");            //*Arm 62-12-20
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wReferBill_Shown(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "wReferBill_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Call keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wReferBill", "ocmKB_Click : " + ex.Message); }
        }

        /// <summary>
        /// Close popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                cVB.tVB_RefDocNo = ""; //*Arm 63-05-25
                tVB_StatusReBill = "0";

                //*Arm 63-06-01
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
                //+++++++++++++

                this.Close();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wReferBill", "ocmBack_Click : " + ex.Message); }
        }

        /// <summary>
        /// Closing form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wReferBill_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                oW_Resource = null;
                this.Dispose();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wReferBill", "wReferBill_FormClosing : " + ex.Message); }
        }

        /// <summary>
        /// Check change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ockRefer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (nW_RefMode == 1) //*Arm 63-06-10
                //{
                //    ockRefer.Checked = true;
                //}

                ockRefer.Checked = true; //*Arm 63-07-23

                if (ockRefer.Checked)
                {
                    if ( string.Equals( cVB.tVB_PosType, "2"))
                    {
                        opnWristband.Visible = true;
                        opnDocument.Visible = false;
                        opnWristband.BackColor = Color.White;
                        otbWristband.Enabled = true;
                        otbWristband.ReadOnly = false;
                        otbWristband.Focus();
                        cVB.nVB_ReferBy = 1;

                        //*Arm 62-12-20
                        olaWristband.Enabled = false;
                        olaDocument.Enabled = true;
                    }
                    else
                    {
                        opnDocument.Visible = true;
                        opnWristband.Visible = false;
                        opnDocument.BackColor = Color.White;
                        otbDocument.Enabled = true;
                        otbDocument.ReadOnly = false;
                        ocmSchDoc.Enabled = true;
                        ocmSchDoc.BackColor = cVB.oVB_ColDark;
                        otbDocument.Focus();
                        cVB.nVB_ReferBy = 2;

                        //*Arm 62-12-20
                        olaWristband.Enabled = false;
                        olaWristband.Cursor = Cursors.No;
                        olaDocument.Cursor = Cursors.No;
                        olaDocument.ForeColor = Color.Black;
                        olaDocument.Enabled = true;

                        otpSaleDate.Enabled = true; //*Arm 63-06-10
                    }
                }
                else
                {
                    
                    opnDocument.BackColor = Color.DimGray;
                    opnWristband.BackColor = Color.DimGray;
                    ocmSchDoc.BackColor = Color.Silver;
                    otbDocument.Enabled = false;
                    otbDocument.ReadOnly = true;
                    otbWristband.Enabled = false;
                    otbWristband.ReadOnly = true;
                    ocmSchDoc.Enabled = false;
                   

                    //*Arm 62-12-20
                    olaWristband.Enabled = false;
                    olaDocument.Enabled = false;
                }
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wReferBill", "ockRefer_CheckedChanged : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "OnPaintBackground : " + oEx.Message); }
        }

        /// <summary>
        /// Accept
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            wReason oReason;
            try
            {
                if (ockRefer.Checked)
                {
                    if (opnWristband.Visible)
                    {
                        if (string.IsNullOrEmpty(otbWristband.Text.Trim()))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(otbDocument.Text.Trim()))
                        {
                            return;
                        }
                        if (C_CHKnCheckConditionDoc() > 0)  //*ARM 62-12-20 ตรวจสอบเงื่อนไข Document
                        {
                            cVB.tVB_RefDocNo = otbDocument.Text;
                            cmlPdtOrder oPdt = new cmlPdtOrder();
                            cVB.aoVB_PdtRefund = new List<cmlPdtOrder>();
                            cVB.aoVB_PdtDisChgRefund = new List<cmlPdtDisChg>();    //*Arm 63-03-21  ส่วนลดรายการ/ท้ายบิล
                            cVB.aoVB_PdtRdDocType1 = new List<cmlPdtRedeem>();      //*Arm 63-03-21  แต้มแลกส่วนลด DocType1
                            cVB.aoVB_PdtRdDocType2 = new List<cmlPdtRedeem>();      //*Arm 63-03-21  แต้มแลกส่วนลด DocType2 

                            ////*ARM 62-12-19 
                            //if (cVB.nVB_ReturnType == 1)
                            //{
                            //    //List<cmlTPSTSalDT> aoDT = new List<cmlTPSTSalDT>();
                            //    //StringBuilder oSql = new StringBuilder();
                            //    //cDatabase oDB = new cDatabase();
                            //    //oSql.Clear();
                            //    ////oSql.AppendLine("SELECT DT.FTPdtCode,ISNULL(PDTL.FTPdtNameABB,(SELECT TOP 1 FTPdtNameABB FROM TCNMPdt_L WITH(NOLOCK) WHERE FTPdtCode = DT.FTPdtCode)) AS FTXsdPdtName,");
                            //    ////oSql.AppendLine("ISNULL(PUNL.FTPunName,(SELECT TOP 1 FTPunName FROM TCNMPdtUnit_L WITH(NOLOCK) WHERE FTPunCode = DT.FTPunCode)) AS FTPunName,");
                            //    //oSql.AppendLine("SELECT DT.FTPdtCode,DT.FTXsdPdtName,DT.FTPunName,");    //*Em 63-04-28
                            //    //oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdQty,DT.FCXsdNet,DT.FCXsdFactor,DT.FTXsdBarCode,DT.FCXsdQtyLef");
                            //    //oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                            //    ////oSql.AppendLine("LEFT JOIN TCNMPdt_L PDTL WITH(NOLOCK) ON PDTL.FTPdtCode = DT.FTPdtCode AND PDTL.FNLngID = " + cVB.nVB_Language);
                            //    ////oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L PUNL WITH(NOLOCK) ON PUNL.FTPunCode = DT.FTPunCode AND PUNL.FNLngID = " + cVB.nVB_Language);
                            //    //oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' ");
                            //    //oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                            //    //oSql.AppendLine("AND DT.FTXsdStaPdt = '1'");
                            //    //oSql.AppendLine("ORDER BY DT.FNXsdSeqNo");
                            //    //aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());
                            //    //if (aoDT.Count > 0)
                            //    //{
                            //    //    foreach (cmlTPSTSalDT oDT in aoDT)
                            //    //    {
                            //    //        oPdt = new cmlPdtOrder();
                            //    //        oPdt.tPdtCode = oDT.FTPdtCode;
                            //    //        oPdt.tPdtName = oDT.FTXsdPdtName;
                            //    //        oPdt.tBarcode = oDT.FTXsdBarCode;
                            //    //        oPdt.tUnit = oDT.FTPunName;
                            //    //        oPdt.cSetPrice = Convert.ToDecimal(oDT.FCXsdSetPrice);
                            //    //        oPdt.cQty = Convert.ToDecimal(oDT.FCXsdQtyLef);
                            //    //        oPdt.cFactor = Convert.ToDecimal(oDT.FCXsdFactor);
                            //    //        oPdt.tStaPdt = "1";
                            //    //        cVB.aoVB_PdtRefund.Add(oPdt);

                            //    //    }



                            //    //    // *Arm 63-03-21  - ส่วนลดรายการ/ท้ายบิล
                            //    //    oSql.Clear();
                            //    //    oSql.AppendLine("SELECT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice, ");
                            //    //    oSql.AppendLine("DTDis.FNXddStaDis AS pnStaDis, DTDis.FTXddDisChgType AS ptDisChgType, DTDis.FCXddNet AS pcNet,DTDis.FCXddValue AS pcValue");
                            //    //    oSql.AppendLine("FROM TPSTSalDTDis DTDis with(nolock)");
                            //    //    oSql.AppendLine("INNER JOIN TPSTSalDT DT with(nolock) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                            //    //    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                            //    //    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                            //    //    oSql.AppendLine("AND ISNULL(DTDis.FTXddRefCode,'') =''");
                            //    //    oSql.AppendLine("ORDER BY DTDis.FDXddDateIns ASC");
                            //    //    cVB.aoVB_PdtDisChgRefund = new cDatabase().C_GETaDataQuery<cmlPdtDisChg>(oSql.ToString());

                            //    //    oSql.Clear();
                            //    //    oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                            //    //    //oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg AS FCXpdDisChg, FCXhdAmt AS FCXpdAmt , FTXhdRefCode ");
                            //    //    oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt , FTXhdRefCode "); //*Arm 63-04-16
                            //    //    oSql.AppendLine("FROM TPSTSalHDDis WITH(NOLOCK) ");
                            //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                            //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'  ");
                            //    //    oSql.AppendLine("AND (ISNULL(FTXhdRefCode,'') ='' OR FTXhdDisChgType = '5'  OR FTXhdDisChgType = '6')");
                            //    //    oSql.AppendLine("ORDER BY FDXhdDateIns ASC");
                            //    //    cVB.aoVB_PdtHDDisRefund = new cDatabase().C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());
                            //    //    //+++++++++++++++

                            //    //    // *Arm 63-03-21  - ส่วนลดจากการแลกแต้ม
                            //    //    // DocType 1
                            //    //    oSql.Clear();
                            //    //    oSql.AppendLine("SELECT DISTINCT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice,");
                            //    //    oSql.AppendLine("DTDis.FTBchCode AS ptBchCode, DTDis.FTXshDocNo AS tDocNo, DTDis.FNXddStaDis AS pnStaDis,DTDis.FTXddDisChgType AS ptDisChgType,DTDis.FCXddValue AS pcValue, DTDis.FTXddRefCode AS ptRefCode,");
                            //    //    oSql.AppendLine("RD.FTRdhDocType AS ptDocType, RD.FNXrdPntUse AS pnUsePnt, RD.FCXrdPdtQty AS pcPdtQty");
                            //    //    oSql.AppendLine("FROM TPSTSalDTDis DTDis with(nolock)");
                            //    //    oSql.AppendLine("INNER JOIN TPSTSalDT DT with(nolock) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                            //    //    oSql.AppendLine("INNER JOIN TPSTSalRD RD with(nolock) ON RD.FTBchCode = DTDis.FTBchCode AND RD.FTXshDocNo = DTDis.FTXshDocNo AND RD.FTXrdRefCode = DTDis.FTXddRefCode  AND RD.FNXrdRefSeq = DTDis.FNXsdSeqNo");
                            //    //    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                            //    //    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                            //    //    oSql.AppendLine("AND DTDis.FNXddStaDis = '2'");
                            //    //    oSql.AppendLine("AND ISNULL(DTDis.FTXddRefCode,'') != ''");
                            //    //    oSql.AppendLine("AND RD.FTRdhDocType = '1'");
                            //    //    cVB.aoVB_PdtRdDocType1 = new cDatabase().C_GETaDataQuery<cmlPdtRedeem>(oSql.ToString());

                            //    //    // DocType 2
                            //    //    oSql.Clear();
                            //    //    oSql.AppendLine("SELECT RD.FTBchCode AS ptBchCode, RD.FTXshDocNo AS tDocNo, RD.FTRdhDocType AS ptDocType, RD.FNXrdPntUse AS pnUsePnt,");
                            //    //    oSql.AppendLine("DTDis.FTXddRefCode AS ptRefCode, DTDis.FNXddStaDis AS pnStaDis,DTDis.FTXddDisChgType AS ptDisChgType, SUM(DTDis.FCXddValue) AS pcUseMny");
                            //    //    oSql.AppendLine("FROM TPSTSalRD RD with(nolock)");
                            //    //    oSql.AppendLine("INNER JOIN TPSTSalDTDis DTDis with(nolock) ON RD.FTBchCode = DTDis.FTBchCode AND RD.FTXshDocNo = DTDis.FTXshDocNo AND RD.FTXrdRefCode = DTDis.FTXddRefCode");
                            //    //    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                            //    //    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                            //    //    oSql.AppendLine("AND DTDis.FNXddStaDis = '2'");
                            //    //    oSql.AppendLine("AND ISNULL(DTDis.FTXddRefCode,'') != ''");
                            //    //    oSql.AppendLine("AND RD.FNXrdRefSeq = '0'");
                            //    //    oSql.AppendLine("AND RD.FTRdhDocType = '2'");
                            //    //    oSql.AppendLine("GROUP BY RD.FTBchCode,RD.FTXshDocNo, DTDis.FTXddRefCode, DTDis.FNXddStaDis, DTDis.FTXddDisChgType, RD.FTRdhDocType,RD.FNXrdPntUse");
                            //    //    cVB.aoVB_PdtRdDocType2 = new cDatabase().C_GETaDataQuery<cmlPdtRedeem>(oSql.ToString());
                            //    //    //+++++++++++++++
                            //    //}

                            //    //*Em 63-05-13
                            //    cVB.bVB_RefundFullBill = true;
                            //    cVB.oVB_Sale.W_PRCxRefund();
                                
                            //    oReason = new wReason("003");
                            //    oReason.ShowDialog();
                            //    this.Close();
                            //    //+++++++++++++++++
                            //}
                            //else
                            //{
                            //    //*Em 63-05-12
                            //    new wChooseItemRef(2).ShowDialog();
                            //    this.Close();
                            //    //+++++++++++++++++++++++
                            //}

                            //*Arm 63-06-10  Start Process 
                            if (nW_RefMode == 1) // Mode 1:เปิดบิลขายอ้างบิลคืน
                            {
                                new wChooseItemRef(3).ShowDialog();
                                this.Close();
                            }
                            else
                            {
                                if (cVB.nVB_ReturnType == 1) // ถ้าเป็น Option การคืน = 1: คืนทั้งบิล
                                {
                                    //*Em 63-05-13
                                    cVB.bVB_RefundFullBill = true;
                                    //cVB.oVB_Sale.W_PRCxRefund();
                                    //oReason = new wReason("003");
                                    //oReason.ShowDialog();
                                    //this.Close();

                                    if (cVB.oVB_Sale.W_PRCxRefund() == true) //*Arm 63-08-17
                                    {
                                        oReason = new wReason("003");
                                        oReason.ShowDialog();
                                        this.Close();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                    //+++++++++++++++++

                                }
                                else
                                {
                                    //*Em 63-05-12
                                    new wChooseItemRef(2).ShowDialog();
                                    this.Close();
                                    //+++++++++++++++++++++++
                                }
                            }
                            //*Arm 63-06-10  End Process 
                        }
                        else
                        {
                            new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgErrRetrunBill"), 2);
                            otbDocument.Text = "";
                        }
                    }
                    
                }
                else
                {
                    //cVB.aoVB_PdtRefund = null;  //*Arm 62-12-20
                    //cVB.tVB_RefDocNo = "";      //*Arm 62-12-20
                    //cVB.oVB_Sale.W_PRCxRefund();    //*Em 63-04-28
                    //oReason = new wReason("003");
                    //oReason.ShowDialog();
                    //this.Close();
                    
                    if (nW_RefMode == 2) //*Arm 63-06-10 เช็คกรณี RefMode = 2: คืนอ้างบิลขาย
                    {
                        cVB.aoVB_PdtRefund = null;  //*Arm 62-12-20
                        cVB.tVB_RefDocNo = "";      //*Arm 62-12-20
                        //cVB.oVB_Sale.W_PRCxRefund();    //*Em 63-04-28
                        //oReason = new wReason("003");
                        //oReason.ShowDialog();
                        //this.Close();

                        if (cVB.oVB_Sale.W_PRCxRefund() == true) //*Arm 63-08-17
                        {
                            oReason = new wReason("003");
                            oReason.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDocument_Click(object sender, EventArgs e)
        {
            try
            {
                otbDocument.Focus();
                opnWristband.Visible = false;
                opnDocument.Visible = true;
                opnDocument.BackColor = Color.White;
                otbDocument.Enabled = true;
                ocmSchDoc.Enabled = true;
                otbDocument.Focus();
                cVB.nVB_ReferBy = 2;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "ocmDocument_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Wristband
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmWristband_Click(object sender, EventArgs e)
        {
            try
            {
                otbWristband.Focus();
                opnDocument.Visible = false;
                opnWristband.Visible = true;
                opnWristband.BackColor = Color.White;
                otbWristband.Enabled = true;
                otbWristband.Focus();
                cVB.nVB_ReferBy = 1;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "ocmWristband_Click : " + oEx.Message); }
        }

        private void ocmSchDoc_Click(object sender, EventArgs e)
        {
            try
            {
                ////this.Visible = false;
                ////cVB.tVB_DocNo = "";
                //if(new wSearchDoc().ShowDialog() == DialogResult.OK)
                //{
                //    otbDocument.Text = cVB.tVB_RefDocNo;
                //}
                ////otbDocument.Text = cVB.tVB_DocNo;
                //otbDocument.Focus(); //*Arm 63-05-25
                ////this.Visible = true;

                //*Arm 63-06-10
                if (new wSearchDoc(Convert.ToDateTime(otpSaleDate.Value.ToString("yyyy-MM-dd")), nW_RefMode).ShowDialog() == DialogResult.OK)
                {
                    otbDocument.Text = cVB.tVB_RefDocNo;
                }
                otbDocument.Focus();
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "ocmSchDoc_Click : " + oEx.Message); }
        }
        
        private int C_CHKnCheckConditionDoc()
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nResult = 0;
            string tPosCode;
            string tBchCode; //*Arm 63-09-11
            try
            {
                //*Arm 62-12-19 Check Option การคืน 
                //================================
                //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                //2:คืนได้ครังเดียว บางรายการได้ 
                //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 
                //5:ห้ามคืน

                //if (!string.IsNullOrEmpty(otbDocument.Text))
                //{
                //    oDB = new cDatabase();
                //    oSql = new StringBuilder();

                //    //*Arm 63-05-25 
                //    // หาจาก Temp
                //    //oSql.AppendLine("SELECT Count(*) ");
                //    oSql.AppendLine("SELECT Count(FTXshDocNo) ");   //*Em 63-04-28
                //    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH(NOLOCK)");
                //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                //    oSql.AppendLine("AND FNXshDocType = 1 ");
                //    oSql.AppendLine("AND FTXshDocNo = '" + otbDocument.Text.Trim() + "'");
                //    switch (cVB.nVB_ReturnType)
                //    {
                //        case 1:   //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                //            oSql.AppendLine("AND FTXshStaRefund != '2' ");

                //            break;

                //        case 2:   //2:คืนได้ครังเดียว บางรายการได้ 
                //            oSql.AppendLine("AND FTXshStaRefund != '2' ");
                //            break;

                //        case 3:   //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                //            oSql.AppendLine("AND FNXshStaRef != '2' ");
                //            break;

                //        case 4:   //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 

                //            break;
                //    }
                //    nResult = oDB.C_GEToDataQuery<int>(oSql.ToString());
                //    cVB.bVB_RefundTrans = false;

                //    if (nResult == 0) //*Arm 63-05-25 ถ้าไม่เจอหาจากตารางจริง Temp
                //    {
                //        //oSql.AppendLine("SELECT Count(*) ");
                //        oSql.Clear();
                //        oSql.AppendLine("SELECT Count(FTXshDocNo) ");   //*Em 63-04-28
                //        oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                //        oSql.AppendLine("AND FNXshDocType = 1 ");
                //        oSql.AppendLine("AND FTXshDocNo = '" + otbDocument.Text.Trim() + "'");
                //        switch (cVB.nVB_ReturnType)
                //        {
                //            case 1:   //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                //                oSql.AppendLine("AND FTXshStaRefund != '2' ");

                //                break;

                //            case 2:   //2:คืนได้ครังเดียว บางรายการได้ 
                //                oSql.AppendLine("AND FTXshStaRefund != '2' ");
                //                break;

                //            case 3:   //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                //                oSql.AppendLine("AND FNXshStaRef != '2' ");
                //                break;

                //            case 4:   //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 

                //                break;
                //        }
                //        nResult = oDB.C_GEToDataQuery<int>(oSql.ToString());
                //        cVB.bVB_RefundTrans = true; //*Arm 63-05-25
                //    }
                //}


                //*Arm 63-06-10  Start Process 

                oDB = new cDatabase();
                oSql = new StringBuilder();

                if (!string.IsNullOrEmpty(otbDocument.Text))
                {

                    //*Arm 63-09-18
                    string tBillType = otbDocument.Text.Substring(0, 1);
                    if (nW_RefMode == 1)  //1:ขายอ้างคืน,2:คืนอ้างขาย
                    {
                        //โหมดขายอ้างคืน ต้องเป็นบิล R เท่านั้น
                        if (tBillType != "R") return 0;
                    }
                    else
                    {
                        //โหมดคืนอ้างขาย ต้องเป็นบิล S เท่านั้น
                        if (tBillType != "S") return 0;
                    }
                    //++++++++++++++

                    tPosCode = otbDocument.Text.Substring(8, 5);    //*Arm 63-05-23  เอา PosCode จากเลขที่บิลที่ระบุ เริ่มตำแหน่งที่ 8 เอาแค่ 5 ตัว 
                    tBchCode = otbDocument.Text.Substring(3, 5);    //*Arm 63-09-11  เอา BchCode จากเลขที่บิลที่ระบุ เริ่มตำแหน่งที่ 3 เอาแค่ 5 ตัว

                    //if (tPosCode == cVB.tVB_PosCode)  //*Arm 63-05-23 Check PosCode ที่ได้จากเลขที่บิล 
                    if (tPosCode == cVB.tVB_PosCode && tBchCode == cVB.tVB_BchCode)  //*Arm 63-09-11 Check PosCode และ BchCode ที่ได้จากเลขที่บิล 
                    {
                        // 2.1 ตรงกับ PosCode ของ local --> ทำตาม Process เดิม

                        //cVB.bVB_RefundTrans = false; //ค้นข้อมูลใน Temp(local) //*Arm 63-09-11 Comment Code
                        nResult = W_SCHnSearchByLocal(false); // ค้นข้อมูลในตาราง Temp 

                        if (nResult > 0)  //ถ้าพบข้อมูลในตาราง Tmp
                        {
                            //ให้กำหนด ชื่อ tC_Ref_XXXXXXX = ชื่อตาราง Temp
                            cSale.tC_Ref_TblSalHD = cSale.tC_TblSalHD;
                            cSale.tC_Ref_TblSalHDDis = cSale.tC_TblSalHDDis;
                            cSale.tC_Ref_TblSalHDCst = cSale.tC_TblSalHDCst;
                            cSale.tC_Ref_TblSalDT = cSale.tC_TblSalDT;
                            cSale.tC_Ref_TblSalDTDis = cSale.tC_TblSalDTDis;
                            cSale.tC_Ref_TblSalRC = cSale.tC_TblSalRC;
                            cSale.tC_Ref_TblSalRD = cSale.tC_TblSalRD;
                            cSale.tC_Ref_TblSalPD = cSale.tC_TblSalPD;
                            cSale.tC_Ref_TblTxnSale = cSale.tC_TblTxnSal;
                            cSale.tC_Ref_TblTxnRedeem = cSale.tC_TblTxnRD;
                        }
                        else
                        {
                            //cVB.bVB_RefundTrans = true; //ค้นข้อมูลใน Transaction(local) //*Arm 63-09-11 Comment Code
                            nResult = W_SCHnSearchByLocal(true);
                            if (nResult > 0) //ถ้าพบข้อมูลในตาราง Transaction
                            {
                                //ให้กำหนด ชื่อ tC_Ref_XXXXXXX = ชื่อตาราง Transaction
                                cSale.tC_Ref_TblSalHD = "TPSTSalHD";
                                cSale.tC_Ref_TblSalHDDis = "TPSTSalHDDis";
                                cSale.tC_Ref_TblSalHDCst = "TPSTSalHDCst";
                                cSale.tC_Ref_TblSalDT = "TPSTSalDT";
                                cSale.tC_Ref_TblSalDTDis = "TPSTSalDTDis";
                                cSale.tC_Ref_TblSalRC = "TPSTSalRC";
                                cSale.tC_Ref_TblSalRD = "TPSTSalRD";
                                cSale.tC_Ref_TblSalPD = "TPSTSalPD";
                                cSale.tC_Ref_TblTxnSale = "TCNTMemTxnSale";
                                cSale.tC_Ref_TblTxnRedeem = "TCNTMemTxnRedeem";
                            }
                        }

                        cVB.bVB_RefundDataFrom = true; //คืนข้อมูลภายในเครื่องจุดขายเดิม
                    }
                    else
                    {
                        // 2.2 ไม่ตรงกับ PosCode ของ local --> Call API2ARDOC

                        new cLog().C_WRTxLog("wReferBill", " Process Download Tansaction Sale start...");
                        Cursor.Current = Cursors.WaitCursor;
                        nResult = W_SCHnSearchByAPI();  // Process เตรียมข้อมูลจากหลังบ้านผ่าน API2ARDoc
                        if (nResult > 0)
                        {
                            //ให้กำหนด ชื่อ tC_Ref_XXXXXXX = ชื่อตารางที่รับจาก API มาเก็บไว้
                            cSale.tC_Ref_TblSalHD = "TPSTSalHDTmp";
                            cSale.tC_Ref_TblSalHDDis = "TPSTSalHDDisTmp";
                            cSale.tC_Ref_TblSalHDCst = "TPSTSalHDCstTmp";
                            cSale.tC_Ref_TblSalDT = "TPSTSalDTTmp";
                            cSale.tC_Ref_TblSalDTDis = "TPSTSalDTDisTmp";
                            cSale.tC_Ref_TblSalRC = "TPSTSalRCTmp";
                            cSale.tC_Ref_TblSalRD = "TPSTSalRDTmp";
                            cSale.tC_Ref_TblSalPD = "TPSTSalPDTmp";
                            cSale.tC_Ref_TblTxnSale = "TCNTMemTxnSaleTmp";
                            cSale.tC_Ref_TblTxnRedeem = "TCNTMemTxnRedeemTmp";
                        }
                        Cursor.Current = Cursors.Default;
                        new cLog().C_WRTxLog("wReferBill", " Process Download Tansaction Sale End...");

                        cVB.bVB_RefundDataFrom = false; //คืนข้อมูลข้ามเครื่อง (ข้อมูลจากหลังบ้าน ผ่าน API2ARDoc)
                    }
                }
                //*Arm 63-06-10  End Process 
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferBill", "C_CHKnCheckConditionDoc : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return nResult;
        }

        private void otbDocument_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ocmAccept_Click(ocmAccept, null);
                    break;
            }
            
        }
        /// <summary>
        /// ค้นหาข้อมูลภายในเครื่องจุดขาย (*Arm 63-06-10)
        /// </summary>
        /// <param name="pbTrans"></param>
        /// <returns></returns>
        public int W_SCHnSearchByLocal(bool pbTrans = false)
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nResult = 0;
            string tTblSalHD = "";
            try
            {
                if (pbTrans == true)
                {
                    tTblSalHD = "TPSTSalHD";
                }
                else
                {
                    tTblSalHD = "TSHD" + cVB.tVB_PosCode;
                }

                oDB = new cDatabase();
                oSql = new StringBuilder();

                oSql.AppendLine("SELECT Count(FTXshDocNo) ");
                oSql.AppendLine("FROM " + tTblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + otbDocument.Text.Trim() + "'");
                oSql.AppendLine("AND FTXshStaDoc = 1 ");  //*Arm 63-07-22
                if (nW_RefMode == 1) // ตรวจสอบโหมดการเรียกใช้งาน 1:ขายอ้างอิงบิลคืน, 2:คืนอ้างอิงขาย
                {
                    oSql.AppendLine("AND FNXshDocType = 9 ");
                    oSql.AppendLine("AND FNXshStaRef != '2' ");
                }
                else
                {
                    oSql.AppendLine("AND FNXshDocType = 1 ");

                    // ** ตรวจสอบเงื่อนไขตาม Option การคืน 
                    //=======================================
                    switch (cVB.nVB_ReturnType) //Option การคืน 
                    {
                        case 1:   //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                        case 2:   //2:คืนได้ครังเดียว บางรายการได้ 
                            oSql.AppendLine("AND FTXshStaRefund != '2' ");
                            break;
                        case 3:   //3:คืนได้หลายครัง ตรวจสอบจํานวน 
                            oSql.AppendLine("AND FNXshStaRef != '2' ");
                            break;
                        default:   //4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน 
                            break;
                    }
                }
                nResult = oDB.C_GEToDataQuery<int>(oSql.ToString());

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferBill", "W_SCHnSearchByLocal : " + oEx.Message.ToString());
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }

            return nResult;
        }

        #region Search From Back office

        public int W_SCHnSearchByAPI()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalHD> aHD = new List<cmlTPSTSalHD>();
            cmlReqSaleDwn oReq;
            cmlResItemSaleDwn oRes;
            cClientService oCall;
            HttpResponseMessage oRep;
            string tJSonCall;
            string tUrl;
            bool bStaChk = false;
            int nRow = 0;
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_API2ARDoc)) // Check Url API2ArDoc
                {
                    new cLog().C_WRTxLog("wReferBill", "W_SCHnSearchByAPI/URL API2ARDoc is null or empty ...");
                    return nRow;
                }

                tUrl = cVB.tVB_API2ARDoc + "/SaleDocRefer/Data";
                oReq = new cmlReqSaleDwn();
                oRes = new cmlResItemSaleDwn();

                // 2. Set Request Parameter 
                oReq.ptBchCode = cVB.tVB_BchCode;
                oReq.pdSaleDate = Convert.ToDateTime(otpSaleDate.Value.ToString("yyyy-MM-dd"));
                oReq.ptDocNo = otbDocument.Text;
                oReq.ptMerCode = cVB.tVB_Merchart;  //*Arm 63-09-11
                oReq.ptAgnCode = cVB.tVB_AgnCode;   //*Arm 63-09-11

                if (nW_RefMode == 1) oReq.pnDoctype = 9; //ขายอ้างอิงบิลคืน
                else oReq.pnDoctype = 1;              //คืนอ้างอิงบิลขาย

                tJSonCall = JsonConvert.SerializeObject(oReq);

                new cLog().C_WRTxLog("wReferBill", " W_SCHnSearchByAPI : Call API2ARDoc start...");
                oCall = new cClientService();
                oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);
                oRep = new HttpResponseMessage();
                try
                {
                    oRep = oCall.C_POSToInvoke(tUrl, tJSonCall);
                }
                catch (Exception oEx)
                {
                    new cLog().C_WRTxLog("wReferBill", "W_SCHnSearchByAPI/Call API2ARDoc Error : " + oEx.Message);
                    return nRow;
                }
                new cLog().C_WRTxLog("wReferBill", " W_SCHnSearchByAPI : Call API2ARDoc End...");

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    oRes = JsonConvert.DeserializeObject<cmlResItemSaleDwn>(tJSonRes);

                    if (oRes.rtCode == "001")
                    {
                        if (oRes.roItem.aoTPSTSalHD.Count > 0)
                        {
                            if (nW_RefMode == 1)
                            {
                                // Check Refund bill has never been referenced. (rnXshStaRef 0:ยังไม่เคยอ้างอิง)
                                if ((oRes.roItem.aoTPSTSalHD[0].rnXshStaRef == null ? 0 : oRes.roItem.aoTPSTSalHD[0].rnXshStaRef) == 0) bStaChk = true;
                            }
                            else
                            {
                                // Check refund options (nVB_ReturnType)
                                switch (cVB.nVB_ReturnType)
                                {
                                    case 1:   //1;คืนได้ครังเดียว เต็มบิลเท่านัน
                                    case 2:   //2:คืนได้ครังเดียว บางรายการได้ 
                                        if (oRes.roItem.aoTPSTSalHD[0].rtXshStaRefund != "2") bStaChk = true;
                                        break;
                                    case 3:   //3:คืนได้หลายครัง ตรวจสอบจํานวน
                                        if ((int)oRes.roItem.aoTPSTSalHD[0].rnXshStaRef != 2) bStaChk = true;
                                        break;
                                    case 4:   //4:คืนได้หลายครั้ง ไม่ตรวจสอบจํานวน 
                                        bStaChk = true;
                                        break;
                                }
                            }

                            if (bStaChk == true)
                            {
                                new cLog().C_WRTxLog("wReferBill", " W_SCHnSearchByAPI : W_PRCbInsert2Temp Start...");
                                if (W_PRCbInsert2Temp(oRes) == true) { nRow = oRes.roItem.aoTPSTSalHD.Count; }
                                new cLog().C_WRTxLog("wReferBill", " W_SCHnSearchByAPI : W_PRCbInsert2Temp End...");
                            }

                        } // end Check aoTPSTSalHD.Count > 0
                    } // end check oRes.rtCode = 001
                    else
                    {
                        new cLog().C_WRTxLog("wReferBill", "W_SCHnSearchByAPI/API2ARDoc Response Code " + oRes.rtCode + " " + oRes.rtDesc);
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferBill", "W_SCHnSearchByAPI : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                aHD = null;
                oReq = null;
                oCall = null;
                oRep = null;
                oRes = null;
                //new cSP().SP_CLExMemory();
            }
            return nRow;
        }
        public bool W_PRCbInsert2Temp(cmlResItemSaleDwn poData)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            SqlTransaction oTranscation;

            List<cmlTPSTSalHDTmp> aoHD;
            List<cmlTPSTSalHDCstTmp> aoHDCst;
            List<cmlTPSTSalHDDisTmp> aoHDDis;
            List<cmlTPSTSalDTTmp> aoDT;
            List<cmlTPSTSalDTDisTmp> aoDTDis;
            List<cmlTPSTSalRDTmp> aoRD;
            List<cmlTPSTSalRCTmp> aoRC;
            List<cmlTPSTSalPDTmp> aoPD;
            List<TCNTMemTxnSaleTmp> aoTxnSale;
            List<TCNTMemTxnRedeemTmp> aoTxnRedeem;

            cDataReaderAdapter<cmlTPSTSalHDTmp> oHD;
            cDataReaderAdapter<cmlTPSTSalHDCstTmp> oHDCst;
            cDataReaderAdapter<cmlTPSTSalHDDisTmp> oHDDis;
            cDataReaderAdapter<cmlTPSTSalDTTmp> oDT;
            cDataReaderAdapter<cmlTPSTSalDTDisTmp> oDTDis;
            cDataReaderAdapter<cmlTPSTSalRDTmp> oRD;
            cDataReaderAdapter<cmlTPSTSalRCTmp> oRC;
            cDataReaderAdapter<cmlTPSTSalPDTmp> oPD;
            cDataReaderAdapter<TCNTMemTxnSaleTmp> oTxnSale;
            cDataReaderAdapter<TCNTMemTxnRedeemTmp> oTxnRedeem;

            try
            {
                new cLog().C_WRTxLog("wReferBill", " W_PRCbInsert2Temp : Insert Data to TableTemp Start...");

                //1.Created Temp
                oSql = new StringBuilder();
                oDB = new cDatabase();

                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalHD", "TPSTSalHDTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalHDCst", "TPSTSalHDCstTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalHDDis", "TPSTSalHDDisTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalDT", "TPSTSalDTTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalDTDis", "TPSTSalDTDisTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalRD", "TPSTSalRDTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalRC", "TPSTSalRCTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalPD", "TPSTSalPDTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TPSTSalDTPmt", "TPSTSalDTPmtTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TCNTMemTxnSale", "TCNTMemTxnSaleTmp");
                new cDatabase().C_PRCxCreateDatabaseTmp("TCNTMemTxnRedeem", "TCNTMemTxnRedeemTmp");

                oTranscation = cVB.oVB_ConnDB.BeginTransaction();

                //2.Bluk Copy

                // Bulk Copy : TPSTSalHD
                aoHD = C_PRCaListSalHD(poData.roItem.aoTPSTSalHD);
                oHD = new cDataReaderAdapter<cmlTPSTSalHDTmp>(aoHD);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oHD.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalHDTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oHD);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TPSTSalHDTmp : " + oEx.Message);
                        return false;
                    }
                }

                // Bulk Copy : TPSTSalHDCst
                aoHDCst = C_PRCaListSalHDCst(poData.roItem.aoTPSTSalHDCst);
                oHDCst = new cDataReaderAdapter<cmlTPSTSalHDCstTmp>(aoHDCst);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oHDCst.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalHDCstTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oHDCst);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TPSTSalHDCstTmp : " + oEx.Message);
                        return false;
                    }
                }

                // Bulk Copy : TPSTSalHDDis
                aoHDDis = C_PRCaListSalHDDis(poData.roItem.aoTPSTSalHDDis);
                oHDDis = new cDataReaderAdapter<cmlTPSTSalHDDisTmp>(aoHDDis);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oHDDis.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalHDDisTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oHDDis);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TPSTSalHDDisTmp : " + oEx.Message);
                        return false;
                    }
                }

                // Bulk Copy : TPSTSalDT
                aoDT = C_PRCaListSalDT(poData.roItem.aoTPSTSalDT);
                oDT = new cDataReaderAdapter<cmlTPSTSalDTTmp>(aoDT);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oDT.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalDTTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oDT);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TPSTSalDTTmp : " + oEx.Message);
                        return false;
                    }
                }

                // Bulk Copy : TPSTSalDTDis
                aoDTDis = C_PRCaListSalDTDis(poData.roItem.aoTPSTSalDTDis);
                oDTDis = new cDataReaderAdapter<cmlTPSTSalDTDisTmp>(aoDTDis);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oDTDis.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalDTDisTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oDTDis);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TPSTSalDTDisTmp : " + oEx.Message);
                        return false;
                    }
                }

                // Bulk Copy : TPSTSalRD
                aoRD = C_PRCaListSalRD(poData.roItem.aoTPSTSalRD);
                oRD = new cDataReaderAdapter<cmlTPSTSalRDTmp>(aoRD);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oRD.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalRDTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oRD);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TPSTSalRDTmp : " + oEx.Message.ToString());
                        return false;
                    }
                }

                // Bulk Copy : TPSTSalRC
                aoRC = C_PRCaListSalRC(poData.roItem.aoTPSTSalRC);
                oRC = new cDataReaderAdapter<cmlTPSTSalRCTmp>(aoRC);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oRC.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalRCTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oRC);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TPSTSalRCTmp : " + oEx.Message.ToString());
                        return false;
                    }
                }

                // Bulk Copy : TPSTSalPD
                aoPD = C_PRCaListSalPD(poData.roItem.aoTPSTSalPD);
                oPD = new cDataReaderAdapter<cmlTPSTSalPDTmp>(aoPD);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oPD.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTSalPDTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oPD);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TPSTSalPDTmp : " + oEx.Message.ToString());
                        return false;
                    }
                }
                
                // Bulk Copy : TCNTMemTxnSale
                aoTxnSale = C_PRCaListTxnSale(poData.roItem.aoTCNTMemTxnSale);
                oTxnSale = new cDataReaderAdapter<TCNTMemTxnSaleTmp>(aoTxnSale);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oTxnSale.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TCNTMemTxnSaleTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oTxnSale);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TCNTMemTxnSaleTmp : " + oEx.Message);
                        return false;
                    }
                }

                // Bulk Copy : TCNTMemTxnRedeem
                aoTxnRedeem = C_PRCaListTxnRedeem(poData.roItem.aoTCNTMemTxnRedeem);
                oTxnRedeem = new cDataReaderAdapter<TCNTMemTxnRedeemTmp>(aoTxnRedeem);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oTxnRedeem.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TCNTMemTxnRedeemTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oTxnRedeem);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp/TCNTMemTxnRedeemTmp : " + oEx.Message.ToString());
                        return false;
                    }
                }

                oTranscation.Commit();
                new cLog().C_WRTxLog("wReferBill", " W_PRCbInsert2Temp : Insert Data to TableTemp End...");

                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReferBill", "W_PRCbInsert2Temp : " + oEx.Message.ToString());
                return false;
            }
            finally
            {
                oDB = null;
                oSql = null;
                oTranscation = null;
                aoHD = null;
                aoHDCst = null;
                aoHDDis = null;
                aoDT = null;
                aoDTDis = null;
                aoRD = null;
                aoRC = null;
                aoPD = null;
                aoTxnSale = null;
                aoTxnRedeem = null;
                oHD = null;
                oHDCst = null;
                oHDDis = null;
                oDT = null;
                oDTDis = null;
                oRD = null;
                oRC = null;
                oPD = null;
                oTxnSale = null;
                oTxnRedeem = null;
                //new cSP().SP_CLExMemory();
            }
        }

        private List<cmlTPSTSalHDTmp> C_PRCaListSalHD(List<cmlResInfoSalHD> paoSalHD)
        {
            List<cmlTPSTSalHDTmp> aoData = new List<cmlTPSTSalHDTmp>();
            try
            {
                aoData = paoSalHD.Select(oItem => new cmlTPSTSalHDTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FTShpCode = oItem.rtShpCode,
                    FNXshDocType = oItem.rnXshDocType,
                    FDXshDocDate = oItem.rdXshDocDate,
                    FTXshCshOrCrd = oItem.rtXshCshOrCrd,
                    FTXshVATInOrEx = oItem.rtXshVATInOrEx,
                    FTDptCode = oItem.rtDptCode,
                    FTWahCode = oItem.rtWahCode,
                    FTPosCode = oItem.rtPosCode,
                    FTShfCode = oItem.rtShfCode,
                    FNSdtSeqNo = oItem.rnSdtSeqNo,
                    FTUsrCode = oItem.rtUsrCode,
                    FTSpnCode = oItem.rtSpnCode,
                    FTXshApvCode = oItem.rtXshApvCode,
                    FTCstCode = oItem.rtCstCode,
                    FTXshDocVatFull = oItem.rtXshDocVatFull,
                    FTXshRefExt = oItem.rtXshRefExt,
                    FDXshRefExtDate = oItem.rdXshRefExtDate,
                    FTXshRefInt = oItem.rtXshRefInt,
                    FDXshRefIntDate = oItem.rdXshRefIntDate,
                    FTXshRefAE = oItem.rtXshRefAE,
                    FNXshDocPrint = oItem.rnXshDocPrint,
                    FTRteCode = oItem.rtRteCode,
                    FCXshRteFac = oItem.rcXshRteFac,
                    FCXshTotal = oItem.rcXshTotal,
                    FCXshTotalNV = oItem.rcXshTotalNV,
                    FCXshTotalNoDis = oItem.rcXshTotalNoDis,
                    FCXshTotalB4DisChgV = oItem.rcXshTotalB4DisChgV,
                    FCXshTotalB4DisChgNV = oItem.rcXshTotalB4DisChgNV,
                    FTXshDisChgTxt = oItem.rtXshDisChgTxt,
                    FCXshDis = oItem.rcXshDis,
                    FCXshChg = oItem.rcXshChg,
                    FCXshTotalAfDisChgV = oItem.rcXshTotalAfDisChgV,
                    FCXshTotalAfDisChgNV = oItem.rcXshTotalAfDisChgNV,
                    FCXshRefAEAmt = oItem.rcXshRefAEAmt,
                    FCXshAmtV = oItem.rcXshAmtV,
                    FCXshAmtNV = oItem.rcXshAmtNV,
                    FCXshVat = oItem.rcXshVat,
                    FCXshVatable = oItem.rcXshVatable,
                    FTXshWpCode = oItem.rtXshWpCode,
                    FCXshWpTax = oItem.rcXshWpTax,
                    FCXshGrand = oItem.rcXshGrand,
                    FCXshRnd = oItem.rcXshRnd,
                    FTXshGndText = oItem.rtXshGndText,
                    FCXshPaid = oItem.rcXshPaid,
                    FCXshLeft = oItem.rcXshLeft,
                    FTXshRmk = oItem.rtXshRmk,
                    FTXshStaRefund = oItem.rtXshStaRefund,
                    FTXshStaDoc = oItem.rtXshStaDoc,
                    FTXshStaApv = oItem.rtXshStaApv,
                    FTXshStaPrcStk = oItem.rtXshStaPrcStk,
                    FTXshStaPaid = oItem.rtXshStaPaid,
                    FNXshStaDocAct = oItem.rnXshStaDocAct,
                    FNXshStaRef = oItem.rnXshStaRef,
                    FDLastUpdOn = oItem.rdLastUpdOn,
                    FTLastUpdBy = oItem.rtLastUpdBy,
                    FDCreateOn = oItem.rdCreateOn,
                    FTCreateBy = oItem.rtCreateBy
                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListSalHD : " + oEx.Message); }
            finally
            {
                paoSalHD = null;
                //new cSP().SP_CLExMemory();
            }

            return aoData;
        }

        private List<cmlTPSTSalHDCstTmp> C_PRCaListSalHDCst(List<cmlResInfoSalHDCst> paoSalHDCst)
        {
            List<cmlTPSTSalHDCstTmp> aoData = new List<cmlTPSTSalHDCstTmp>();
            try
            {
                aoData = paoSalHDCst.Select(oItem => new cmlTPSTSalHDCstTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FTXshCardID = oItem.rtXshCardID,
                    FTXshCardNo = oItem.rtXshCardNo,
                    FNXshCrTerm = oItem.rnXshCrTerm,
                    FDXshDueDate = oItem.rdXshDueDate,
                    FDXshBillDue = oItem.rdXshBillDue,
                    FTXshCtrName = oItem.rtXshCtrName,
                    FDXshTnfDate = oItem.rdXshTnfDate,
                    FTXshRefTnfID = oItem.rtXshRefTnfID,
                    FNXshAddrShip = oItem.rnXshAddrShip,
                    FNXshAddrTax = oItem.rnXshAddrTax,
                    FTXshCstName = oItem.rtXshCstName,
                    FTXshCstTel = oItem.rtXshCstTel,
                    FCXshCstPnt = (decimal)oItem.rcXshCstPnt,
                    FCXshCstPntPmt = (decimal)oItem.rcXshCstPntPmt

                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListSalHDCst : " + oEx.Message); }
            finally
            {
                paoSalHDCst = null;
                //new cSP().SP_CLExMemory();
            }

            return aoData;
        }

        private List<cmlTPSTSalHDDisTmp> C_PRCaListSalHDDis(List<cmlResInfoSalHDDis> paoSalHDDis)
        {
            List<cmlTPSTSalHDDisTmp> aoData = new List<cmlTPSTSalHDDisTmp>();
            try
            {
                aoData = paoSalHDDis.Select(oItem => new cmlTPSTSalHDDisTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FDXhdDateIns = oItem.rdXhdDateIns,
                    FTXhdDisChgTxt = oItem.rtXhdDisChgTxt,
                    FTXhdDisChgType = oItem.rtXhdDisChgType,
                    FCXhdTotalAfDisChg = oItem.rcXhdTotalAfDisChg,
                    FCXhdDisChg = oItem.rcXhdDisChg,
                    FCXhdAmt = oItem.rcXhdAmt,
                    FTXhdRefCode = oItem.rtXhdRefCode,
                    FTDisCode = oItem.rtDisCode,
                    FTRsnCode = oItem.rtRsnCode

                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListSalHDCst : " + oEx.Message); }
            finally
            {
                paoSalHDDis = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }

        private List<cmlTPSTSalDTTmp> C_PRCaListSalDT(List<cmlResInfoSalDT> paoSalDT)
        {
            List<cmlTPSTSalDTTmp> aoData = new List<cmlTPSTSalDTTmp>();
            try
            {
                aoData = paoSalDT.Select(oItem => new cmlTPSTSalDTTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FNXsdSeqNo = oItem.rnXsdSeqNo,
                    FTPdtCode = oItem.rtPdtCode,
                    FTXsdPdtName = oItem.rtXsdPdtName,
                    FTPunCode = oItem.rtPunCode,
                    FTPunName = oItem.rtPunName,
                    FCXsdFactor = oItem.rcXsdFactor,
                    FTXsdBarCode = oItem.rtXsdBarCode,
                    FTSrnCode = oItem.rtSrnCode,
                    FTXsdVatType = oItem.rtXsdVatType,
                    FTVatCode = oItem.rtVatCode,
                    FCXsdVatRate = oItem.rcXsdVatRate,
                    FTXsdSaleType = oItem.rtXsdSaleType,
                    FCXsdSalePrice = oItem.rcXsdSalePrice,
                    FCXsdQty = oItem.rcXsdQty,
                    FCXsdQtyAll = oItem.rcXsdQtyAll,
                    FCXsdSetPrice = oItem.rcXsdSetPrice,
                    FCXsdAmtB4DisChg = oItem.rcXsdAmtB4DisChg,
                    FTXsdDisChgTxt = oItem.rtXsdDisChgTxt,
                    FCXsdDis = oItem.rcXsdDis,
                    FCXsdChg = oItem.rcXsdChg,
                    FCXsdNet = oItem.rcXsdNet,
                    FCXsdNetAfHD = oItem.rcXsdNetAfHD,
                    FCXsdVat = oItem.rcXsdVat,
                    FCXsdVatable = oItem.rcXsdVatable,
                    FCXsdWhtAmt = oItem.rcXsdWhtAmt,
                    FTXsdWhtCode = oItem.rtXsdWhtCode,
                    FCXsdWhtRate = oItem.rcXsdWhtRate,
                    FCXsdCostIn = oItem.rcXsdCostIn,
                    FCXsdCostEx = oItem.rcXsdCostEx,
                    FTXsdStaPdt = oItem.rtXsdStaPdt,
                    FCXsdQtyLef = oItem.rcXsdQtyLef,
                    FCXsdQtyRfn = oItem.rcXsdQtyRfn,
                    FTXsdStaPrcStk = oItem.rtXsdStaPrcStk,
                    FTXsdStaAlwDis = oItem.rtXsdStaAlwDis,
                    FNXsdPdtLevel = oItem.rnXsdPdtLevel,
                    FTXsdPdtParent = oItem.rtXsdPdtParent,
                    FCXsdQtySet = oItem.rcXsdQtySet,
                    FTPdtStaSet = oItem.rtPdtStaSet,
                    FTXsdRmk = oItem.rtXsdRmk,
                    FDLastUpdOn = oItem.rdLastUpdOn,
                    FTLastUpdBy = oItem.rtLastUpdBy,
                    FDCreateOn = oItem.rdCreateOn,
                    FTCreateBy = oItem.rtCreateBy,
                    FTPplCode = oItem.rtPplCode

                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListSalDT : " + oEx.Message); }
            finally
            {
                paoSalDT = null;
                //new cSP().SP_CLExMemory();
            }

            return aoData;
        }

        private List<cmlTPSTSalDTDisTmp> C_PRCaListSalDTDis(List<cmlResInfoSalDTDis> paoSalDTDis)
        {
            List<cmlTPSTSalDTDisTmp> aoData = new List<cmlTPSTSalDTDisTmp>();
            try
            {
                aoData = paoSalDTDis.Select(oItem => new cmlTPSTSalDTDisTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FNXsdSeqNo = oItem.rnXsdSeqNo,
                    FDXddDateIns = oItem.rdXddDateIns,
                    FNXddStaDis = oItem.rnXddStaDis,
                    FTXddDisChgTxt = oItem.rtXddDisChgTxt,
                    FTXddDisChgType = oItem.rtXddDisChgType,
                    FCXddNet = oItem.rcXddNet,
                    FCXddValue = oItem.rcXddValue,
                    FTXddRefCode = oItem.rtXddRefCode,
                    FTDisCode = oItem.rtDisCode,
                    FTRsnCode = oItem.rtRsnCode

                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListSalDTDis : " + oEx.Message); }
            finally
            {
                paoSalDTDis = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }

        private List<cmlTPSTSalRDTmp> C_PRCaListSalRD(List<cmlResInfoSalRD> paoSalRD)
        {
            List<cmlTPSTSalRDTmp> aoData = new List<cmlTPSTSalRDTmp>();
            try
            {
                aoData = paoSalRD.Select(oItem => new cmlTPSTSalRDTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FNXrdSeqNo = oItem.rnXrdSeqNo,
                    FTRdhDocType = oItem.rtRdhDocType,
                    FNXrdRefSeq = oItem.rnXrdRefSeq,
                    FTXrdRefCode = oItem.rtXrdRefCode,
                    FCXrdPdtQty = oItem.rcXrdPdtQty,
                    FNXrdPntUse = oItem.rnXrdPntUse
                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListSalRD : " + oEx.Message); }
            finally
            {
                paoSalRD = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }

        private List<cmlTPSTSalRCTmp> C_PRCaListSalRC(List<cmlResInfoSalRC> paoSalRC)
        {
            List<cmlTPSTSalRCTmp> aoData = new List<cmlTPSTSalRCTmp>();
            try
            {
                aoData = paoSalRC.Select(oItem => new cmlTPSTSalRCTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FNXrcSeqNo = oItem.rnXrcSeqNo,
                    FTRcvCode = oItem.rtRcvCode,
                    FTRcvName = oItem.rtRcvName,
                    FTXrcRefNo1 = oItem.rtXrcRefNo1,
                    FTXrcRefNo2 = oItem.rtXrcRefNo2,
                    FDXrcRefDate = oItem.rdXrcRefDate,
                    FTXrcRefDesc = oItem.rtXrcRefDesc,
                    FTBnkCode = oItem.rtBnkCode,
                    FTRteCode = oItem.rtRteCode,
                    FCXrcRteFac = oItem.rcXrcRteFac,
                    FCXrcFrmLeftAmt = oItem.rcXrcFrmLeftAmt,
                    FCXrcUsrPayAmt = oItem.rcXrcUsrPayAmt,
                    FCXrcDep = oItem.rcXrcDep,
                    FCXrcNet = oItem.rcXrcNet,
                    FCXrcChg = oItem.rcXrcChg,
                    FTXrcRmk = oItem.rtXrcRmk,
                    FTPhwCode = oItem.rtPhwCode,
                    FTXrcRetDocRef = oItem.rtXrcRetDocRef,
                    FTXrcStaPayOffline = oItem.rtXrcStaPayOffline,
                    FDLastUpdOn = oItem.rdLastUpdOn,
                    FTLastUpdBy = oItem.rtLastUpdBy,
                    FDCreateOn = oItem.rdCreateOn,
                    FTCreateBy = oItem.rtCreateBy
                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListSalRC : " + oEx.Message); }
            finally
            {
                paoSalRC = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }

        private List<cmlTPSTSalPDTmp> C_PRCaListSalPD(List<cmlResInfoSalPD> paoSalPD)
        {
            List<cmlTPSTSalPDTmp> aoData = new List<cmlTPSTSalPDTmp>();
            try
            {
                aoData = paoSalPD.Select(oItem => new cmlTPSTSalPDTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FTPmhDocNo = oItem.rtPmhDocNo,
                    FNXsdSeqNo = oItem.rnXsdSeqNo,
                    FTPmdGrpName = oItem.rtPmdGrpName,
                    FTPdtCode = oItem.rtPdtCode,
                    FTPunCode = oItem.rtPunCode,
                    FCXsdQty = oItem.rcXsdQty,
                    FCXsdQtyAll = oItem.rcXsdQtyAll,
                    FCXsdSetPrice = oItem.rcXsdSetPrice,
                    FCXsdNet = oItem.rcXsdNet,
                    FCXpdGetQtyDiv = oItem.rcXpdGetQtyDiv,
                    FTXpdGetType = oItem.rtXpdGetType,
                    FCXpdGetValue = oItem.rcXpdGetValue,
                    FCXpdDis = oItem.rcXpdDis,
                    FCXpdPerDisAvg = oItem.rcXpdPerDisAvg,
                    FCXpdDisAvg = oItem.rcXpdDisAvg,
                    FCXpdPoint = oItem.rcXpdPoint,
                    FTXpdStaRcv = oItem.rtXpdStaRcv,
                    FTPplCode = oItem.rtPplCode,
                    FTXpdCpnText = oItem.rtXpdCpnText,
                    FTCpdBarCpn = oItem.rtCpdBarCpn,
                    FTPmhStaGrpPriority = oItem.rtPmhStaGrpPriority

                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListSalPD : " + oEx.Message); }
            finally
            {
                paoSalPD = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }
        
        private List<TCNTMemTxnSaleTmp> C_PRCaListTxnSale(List<cmlResInfoTxnSale> paoTxnSale)
        {
            List<TCNTMemTxnSaleTmp> aoData = new List<TCNTMemTxnSaleTmp>();
            try
            {
                aoData = paoTxnSale.Select(oItem => new TCNTMemTxnSaleTmp()
                {
                    FTCgpCode = oItem.rtCgpCode,
                    FTMemCode = oItem.rtMemCode,
                    FTTxnRefDoc = oItem.rtTxnRefDoc,
                    FTTxnRefInt = oItem.rtTxnRefInt,
                    FTTxnRefSpl = oItem.rtTxnRefSpl,
                    FDTxnRefDate = oItem.rdTxnRefDate,
                    FCTxnRefGrand = oItem.rcTxnRefGrand,
                    FCTxnPntOptBuyAmt = oItem.rcTxnPntOptBuyAmt,
                    FCTxnPntOptGetQty = oItem.rcTxnPntOptGetQty,
                    FCTxnPntB4Bill = oItem.rcTxnPntB4Bill,
                    FDTxnPntStart = oItem.rdTxnPntStart,
                    FDTxnPntExpired = oItem.rdTxnPntExpired,
                    FCTxnPntBillQty = oItem.rcTxnPntBillQty,
                    FCTxnPntUsed = oItem.rcTxnPntUsed,
                    FCTxnPntExpired = oItem.rcTxnPntExpired,
                    FTTxnPntStaClosed = oItem.rtTxnPntStaClosed,
                    FDLastUpdOn = oItem.rdLastUpdOn,
                    FTLastUpdBy = oItem.rtLastUpdBy,
                    FDCreateOn = oItem.rdCreateOn,
                    FTCreateBy = oItem.rtCreateBy,
                    FTTxnPntDocType = oItem.rtTxnPntDocType
                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListTxnSale : " + oEx.Message); }
            finally
            {
                paoTxnSale = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }

        private List<TCNTMemTxnRedeemTmp> C_PRCaListTxnRedeem(List<cmlResInfoTxnRedeem> paoTxnRedeem)
        {
            List<TCNTMemTxnRedeemTmp> aoData = new List<TCNTMemTxnRedeemTmp>();
            try
            {
                aoData = paoTxnRedeem.Select(oItem => new TCNTMemTxnRedeemTmp()
                {
                    FTCgpCode = oItem.rtCgpCode,
                    FTMemCode = oItem.rtMemCode,
                    FTRedRefDoc = oItem.rtRedRefDoc,
                    FTRedRefInt = oItem.rtRedRefInt,
                    FTRedRefSpl = oItem.rtRedRefSpl,
                    FDRedRefDate = oItem.rdRedRefDate,
                    FCRedPntB4Bill = oItem.rcRedPntB4Bill,
                    FCRedPntBillQty = oItem.rcRedPntBillQty,
                    FTRedPntStaClosed = oItem.rtRedPntStaClosed,
                    FDRedPntStart = oItem.rdRedPntStart,
                    FDRedPntExpired = oItem.rdRedPntExpired,
                    FDLastUpdOn = oItem.rdLastUpdOn,
                    FTLastUpdBy = oItem.rtLastUpdBy,
                    FDCreateOn = oItem.rdCreateOn,
                    FTCreateBy = oItem.rtCreateBy,
                    FTRedPntDocType = oItem.rtRedPntDocType
                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "C_PRCaListTxnRedeem : " + oEx.Message); }
            finally
            {
                paoTxnRedeem = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }
        #endregion End Search From Back office
    }
}
