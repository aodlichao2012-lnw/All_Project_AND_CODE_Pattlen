using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Respond.SaleOrder;
using AdaPos.Popup.wSale;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AdaPos.Models.Webservice.Required.Customer;
using AdaPos.Models.Webservice.Respond.Customer;
using System.Net.Http;
using AdaPos.Models.DatabaseTmp;
using C1.Win.C1FlexGrid;

namespace AdaPos.Forms
{
    public partial class wSale : Form
    {
        #region Variable

        private cSP oW_SP;
        public cSale oW_Sale;
        public cPdtPmt oW_PdtPmt;
        public bool bW_Activate = false;
        private ResourceManager oW_Resource;
        private int nW_Time;
        private int nW_DelayChg;               //*Em 63-07-26 //*Net 63-07-31 ปรับตาม Moshi
        public int nW_Mode;                    // 3:Sale, 4:Refund, 5:Rental
        public int nW_StartRow;                 // StartRow
        public int nW_CurPage = 1;              // Current Page
        public int nW_MaxPage;                  //*Arm 62-10-16 - totalPage
        private int nW_TotalCount;              //*Arm 62-10-16 - จำนวนต่อหน้า
        public string tW_PgpChain;              // FTPgpChain
        private int nW_SortPdt = 1;             // 1:Ascending, 2:Decending
        private decimal cW_SetQty = 1;          //*Arm 63-05-03 จำนวนสินค้าเริ่มต้น 

        List<cmlTCNMPdtTouchGrp> aW_oPdtTouchGrp;    //*Net 63-01-13 - เปลี่ยนจาก PdtGrp ไปใช้ TouchGroup
        //List<cmlTCNMPdtGrp> aW_oPdtGrp;         //*Arm 62-11-15 //*Net เปลี่ยนไปใช้ TouchGroup
        private int nW_GrpStartRow = 0;         //*Arm 62-11-15 - StartRow for Group
        private int nW_GrpMaxPage;              //*Arm 62-11-15 - total Page for Group
        private int nW_GrpCurPage = 1;          //*Arm 62-11-15 - Current Page for Group

        List<cmlTPSMFunc> aW_oFunc;             //*Arm 62-11-15
        private int nW_MenuMaxPage;             //*Arm 62-11-15 - total Page for Group
        private int nW_MenuCurPage = 1;         //*Arm 62-11-15 - Current Page for Group
        private int nW_ScanSO = 0;              //*Arm 63-03-04 - 0:ปกติ, 1: ScanSO
        private bool bW_ResultScanSO = true;    //*Arm 63-03-04 - true:พบสินค้า ,flase:ไม่พบ 
        private int nW_CheckPmt = 0;
        private bool bCoCal = false;
        private bool bCoCalSu = false;
        private int nW_a = 0;
        public bool bW_CalPmtPrice = false;
        DataTable oW_dtTmp;                     //*Arm 63-05-12

        List<Thread> aoW_CalPro;                //*Net 63-05-21 รายการ thread คำนวนโปรโมชั่น 
        #endregion End Variable


        #region Constructor

        public wSale(int pnMode)
        {
            InitializeComponent();

            uQtyList.ocmAdd.Click += OtbQty_TextChanged;
            uQtyList.ocmDel.Click += OtbQty_TextChanged;
            //cVB.nVB_CheckModeStd = 2;

            try
            {

                if (cVB.nVB_SaleModeStd == 1)
                {
                    cVB.tVB_KbdScreen = "SALESTD"; //*Arm 63-04-09
                    //Mode Standrad
                    panel1.Visible = true;
                    opnPdtSTD.Visible = true;
                    opnModeStd.Visible = true;
                }
                else
                {
                    cVB.tVB_KbdScreen = "SALE"; //*Arm 63-04-09
                    panel1.Visible = false;
                    opnModeStd.Visible = false;
                    opnPdtSTD.Visible = false;

                    W_GETxPdtGrp();
                    W_GETxMenuPdt();
                }

                oW_SP = new cSP();
                oW_PdtPmt = new cPdtPmt();

                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                //if (cVB.oVB_MQ_Member != null) cVB.oVB_MQ_Member.oEv_Jump += new EventHandler(W_QueueMember);   //*Arm 62-10-25, *Arm 63-03-04 - ปิการใช้งาน
                oW_SP.SP_PRCxFlickering(this.Handle);   //*Arm 62-11-21 [ย้ายมาเนื่องจาก Event_Jump Notification ไม่ทำงาน]

                //*Net 63-07-31 ปรับตาม Moshi
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                //oW_SP.SP_PRCxFlickering(this.Handle); //*Arm 62-11-21 [comment Code]
                oW_Sale = new cSale();
                oW_Sale.C_Initial();
                cSale.C_GETxLastDocNo();    //*Em 63-05-15

                nW_Mode = pnMode;

                switch (pnMode)
                {
                    case 3:

                        cSale.nC_DocType = 1;
                        //cSale.C_GETxFormatDoc("TPSTSalHD", cSale.nC_DocType, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);
                        //cSale.C_GETtFormatDoc("TPSTSalHD", cSale.nC_DocType, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);   //*Arm 63-03-04
                        cSale.C_DATxGenNewDoc();
                        olaDocNo.Text = cVB.tVB_DocNo;
                        break;

                    case 4:
                        cSale.nC_DocType = 9;
                        //cSale.C_GETxFormatDoc("TPSTSalHD", cSale.nC_DocType, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);
                        //cSale.C_GETtFormatDoc("TPSTSalHD", cSale.nC_DocType, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);   //*Arm 63-03-04
                        cSale.C_DATxGenNewDoc();
                        olaDocNo.Text = cVB.tVB_DocNo;
                        break;
                }


                W_SETxDesign();
                W_SETxText();
                W_SETxImageCus();
                W_GETxMenuBill();
                //W_GETxPdtGrp();
                //W_GETxMenuPdt();
                if (cVB.nVB_SaleModeStd == 1)
                {
                    opnContent.Visible = false;
                    opnList.Visible = false;
                    opnPdtSTD.Visible = true;
                    opnModeStd.Visible = true;
                }
                else
                {
                    opnPdtSTD.Visible = false;
                    opnModeStd.Visible = false;
                    opnContent.Visible = true;
                    opnList.Visible = true;
                    panel1.Visible = false;
                    W_GETxPdtGrp();
                    W_GETxMenuPdt();
                }

                W_SHWxButtonBar();
                W_CHKxReturnFromPayment();


                //cSP.SP_GETxCountQMember(olaMsgCountQMem, opnQMember);   //*Arm 62-10-25, *Arm 63-03-04 - ปิการใช้งาน
                opnDetailCmp.Visible = false;
                opnCst.Visible = true;
                //*Arm 62-12-23 คืนบิลแบบอ้างอิงเอกสารการขาย เพิ่มสินค้าไม่ได้
                if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo) && cSale.nC_DocType == 9)
                {
                    opnPdt.Enabled = false;
                }
                //this.Text = "." + cSale.nC_DocType.ToString() + "." + cVB.tVB_DocNo;
                this.KeyPreview = true;

                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in opnMenu.Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }

                //*Em 63-04-25
                //โหลดข้อมูลหน้า Design wPayment
                if (string.Equals(cVB.tVB_PosType, "1"))     // 1:Store, 2:Cashier
                    cVB.oVB_PayType = new cFunctionKeyboard().C_GETaFuncList("031");
                else
                    cVB.oVB_PayType = new cFunctionKeyboard().C_GETaFuncList("032");

                cVB.oVB_PayMenuBar = new cFunctionKeyboard().C_GETaMenuBar("PAYMENT");
                cVB.oVB_QuickAmt = new cBankNote().C_GETaBanknote();
                //++++++++++++++++++


                cSP.SP_SETxFixPanelOverFlow(opnHDMenuBar, olaBranch);  //*Net 63-06-09 Resize Branch

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "wSale : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Image Customer
        /// </summary>
        private void W_SETxImageCus()
        {

            StringBuilder oSql;
            string tImagePath = string.Empty;
            try
            {
                //*Arm 63-04-17
                //ถ้าสังกัด Merchart ให้ดึงรูป Merchart มาโชว์ ถ้าสังกัดสาขาให้ดึงรูปของสาขามาโชว์

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTImgObj");
                oSql.AppendLine("FROM TCNMImgObj IMG WITH(NOLOCK)");

                if (string.IsNullOrEmpty(cVB.tVB_Merchart))
                {
                    oSql.AppendLine("WHERE IMG.FTImgRefID = '" + cVB.tVB_CmpCode + "' AND FTImgTable='TCNMComp' ");
                }
                else
                {
                    oSql.AppendLine("WHERE IMG.FTImgRefID = '" + cVB.tVB_Merchart + "' AND FTImgTable='TCNMMerchant' ");
                }
                oSql.AppendLine("ORDER BY FDLastUpdOn DESC");

                tImagePath = new cDatabase().C_GETaDataQuery<string>(oSql.ToString()).FirstOrDefault();

                if (!string.IsNullOrEmpty(tImagePath))
                {
                    //*Net 63-07-31 ป้องกันการล๊อคไฟล์รูปภาพ
                    using (Image oImg = Image.FromFile(tImagePath))
                    {
                        //opbLogoCst.BackgroundImage = Image.FromFile(tImagePath);
                        opbLogoCst.BackgroundImage = new Bitmap(oImg);
                        opbLogoCst.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
                else
                {
                    //opbLogoCst.BackgroundImage = Properties.Resources.Adasoft;
                    opbLogoCst.BackgroundImage = Properties.Resources.CstDefault_256; //*Arm 63-08-12
                    opbLogoCst.BackgroundImageLayout = ImageLayout.Stretch;
                }
                //++++++++++++++

                //*Arm 63-04-17 Comment Code
                //tImagePath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Logo";
                //// if (!File.Exists(tImagePath)) tImagePath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Logo\Logo.png";
                //if (Directory.Exists(tImagePath))
                //{
                //    if (File.Exists(tImagePath + @"\Logo.png"))
                //    {
                //        opbLogoCst.BackgroundImage = Image.FromFile(tImagePath + @"\Logo.png");
                //        opbLogoCst.BackgroundImageLayout = ImageLayout.Stretch;
                //    }
                //    else
                //    {
                //        opbLogoCst.BackgroundImage = Properties.Resources.Adasoft;
                //        opbLogoCst.BackgroundImageLayout = ImageLayout.Stretch;
                //    }

                //}
                //else
                //{
                //    Directory.CreateDirectory(tImagePath);
                //    opbLogoCst.BackgroundImage = Properties.Resources.Adasoft;
                //    opbLogoCst.BackgroundImageLayout = ImageLayout.Stretch;
                //}

            }
            catch (Exception oEx)
            {

                new cLog().C_WRTxLog("wSignin", "W_SETxBackgroud : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void OtbQty_TextChanged(object sender, EventArgs e)
        {
            //var a = uQtyList.otbQty.Text;
            ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtQty"].Value = oW_SP.SP_SETtDecShwSve(1, decimal.Parse(uQtyList.otbQty.Text), 0);
            // Price
            ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtQty"].Value) * Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaSetPrice"].Value), cVB.nVB_DecShow);

            cSale.C_PRCxChangeQty(decimal.Parse(uQtyList.otbQty.Text));

            ogdOrder.CurrentCell = ogdOrder.Rows[ogdOrder.RowCount - 1].Cells[0];

            decimal cSum = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells["olaPdtSumPrice"].Value));
            olaTotal.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
            decimal cQty = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells["olaPdtQty"].Value));
            olaQty.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
            cSale.nC_CntItem = ogdOrder.RowCount;
            //*Net 63-07-31 ปรับหน้าจอ 2 ใหม่
            //if (cVB.oVB_2ndScreen != null)
            //{
            //    cVB.oVB_2ndScreen.W_PRCxSaleOrderTo2nd(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtName"].Value.ToString(),
            //       ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString(),
            //       oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow), oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow));
            //}
            //if (cVB.nVB_Check2nd == 1)
            //{

            //}
        }

        #endregion End Constructor

        #region Event


        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSale_Shown(object sender, EventArgs e)
        {
            try
            {
                //*Net 63-07-31 ย้ายมาจาก ctor
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                //W_SETxColGrid(ogdDetail); //*Em 63-07-28
                //*Em 62-08-15
                if (nW_Mode == 4)
                {
                    if (cVB.aoVB_PdtRefund.Count > 0)
                    {
                        switch (cVB.nVB_SaleModeStd)
                        {
                            case 1:
                                //if (ogdPdtStd.Rows.Count > 0)
                                //{
                                //    cVB.cVB_QRPayAmt = 0;
                                //    ocmPayment_Click(ocmPayment, null);
                                //}

                                //*Em 63-05-31
                                if (ogdDetail.Rows.Count-ogdDetail.Rows.Fixed > 0)
                                {
                                    cVB.cVB_QRPayAmt = 0;
                                    ocmPayment_Click(ocmPayment, null);
                                }
                                //+++++++++++++
                                break;
                            case 2:
                                if (ogdOrder.Rows.Count > 0)
                                {
                                    cVB.cVB_QRPayAmt = 0;
                                    ocmPayment_Click(ocmPayment, null);
                                }
                                break;
                        }
                    }
                }
                otbScan.Focus();

                aoW_CalPro = new List<Thread>(); //*Net 63-05-21
                cVB.oVB_Sale = this; //*Arm 63-03-05
                //ogdPdtStd.ClearSelection(); //*Em 63-05-27
                ogdDetail.Select(-1,-1);    //*Em 63-05-31
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "wSale_Shown " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Open form menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1:
                        //if (ogdPdtStd.Rows.Count == 0)
                        //{
                        //    new wHome().Show();
                        //    nW_Time = 0;
                        //    this.Close();
                        //}
                        //else
                        //    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantBack"), 3);

                        //*Em 63-05-31
                        if (ogdDetail.Rows.Count-ogdDetail.Rows.Fixed == 0)
                        {
                            new wHome().Show();
                            nW_Time = 0;
                            this.Close();
                        }
                        else
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantBack"), 3);
                        //+++++++++++++
                        break;
                    case 2:
                        if (ogdOrder.Rows.Count == 0)
                        {
                            new wHome().Show();
                            nW_Time = 0;
                            this.Close();
                        }
                        else
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantBack"), 3);
                        break;
                }


            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmBack_Click " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>opnMenuPdt
        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmMenu_Click " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Favorite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmFavorite_Click(object sender, EventArgs e)
        {
            try
            {
                //if (string.Equals(ocmFavorite.Tag.ToString(), "0"))
                //{
                //    ocmFavorite.Image = Properties.Resources.Favor_32;
                //    ocmFavorite.Tag = 1;
                //}
                //else
                //{
                //    ocmFavorite.Image = Properties.Resources.NoFavor_32;
                //    ocmFavorite.Tag = 0;
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmFavorite_Click " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Open Menu Product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogvOrder_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (opnMenuPdtList.Visible == true || opnListSmallPdt.Visible == true)
                {
                    opnMenuPdtList.Visible = false;
                    opnListSmallPdt.Visible = false;
                    opnListSmallPdt.Location = new Point(opnListSmallPdt.Location.X, 206);
                }
                else
                {

                    if (ogdOrder.Rows[e.RowIndex].Cells["otbStaPdt"].Value.ToString() == "4") return;

                    //*Arm 62-10-08 เช็ค disable ปุ่ม เมื่อ otbStaPdt = 3 : สินค้าฟรี
                    if (ogdOrder.Rows[e.RowIndex].Cells["otbStaPdt"].Value.ToString() == "3")
                    {
                        foreach (Button ocm in opnMenuPdt.Controls)
                        {
                            if (ocm.Tag.ToString() == "C_KBDxPdtQty" ||
                                ocm.Tag.ToString() == "C_KBDxItemFree" ||
                                ocm.Tag.ToString() == "C_KBDxDisAmt" ||
                                ocm.Tag.ToString() == "C_KBDxDisPer" ||
                                ocm.Tag.ToString() == "C_KBDxChgAmt" ||
                                ocm.Tag.ToString() == "C_KBDxChgPer" ||
                                ocm.Tag.ToString() == "C_KBDxPriceOverride"
                                )
                            {
                                ocm.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (Button ocm in opnMenuPdt.Controls)
                        {
                            ocm.Enabled = true;
                        }
                    } // *Arm 62-10-08


                    if (ogdOrder.Rows[e.RowIndex].Cells["otbAlwDis"].Value.ToString() != "1")
                    {
                        W_PRCxCheckDis();
                    }

                    cVB.oVB_PdtOrder = new cmlPdtOrder();
                    cVB.oVB_PdtOrder.tPdtCode = ogdOrder.Rows[e.RowIndex].Cells["olaPdtCode"].Value.ToString();
                    cVB.oVB_PdtOrder.tBarcode = ogdOrder.Rows[e.RowIndex].Cells["otbBarcode"].Value.ToString();
                    cVB.oVB_PdtOrder.tPdtName = ogdOrder.Rows[e.RowIndex].Cells["olaPdtName"].Value.ToString();
                    cVB.oVB_PdtOrder.cSetPrice = decimal.Parse(ogdOrder.Rows[e.RowIndex].Cells["olaPdtSumPrice"].Value.ToString());
                    cVB.oVB_OrderRowIndex = e.RowIndex;

                    cSale.cC_DTQty = Convert.ToDecimal(ogdOrder.Rows[e.RowIndex].Cells["olaPdtQty"].Value); //*Arm 62-10-07

                    //*Zen 63-02-05
                    uQtyList.otbQty.Text = decimal.ToInt32(cSale.cC_DTQty).ToString();

                    var oCell = ogdOrder.GetCellDisplayRectangle(0, e.RowIndex, false);
                    opnListSmallPdt.Visible = true;
                    opnListSmallPdt.Location = new Point(opnListSmallPdt.Location.X, 206 + oCell.Bottom);
                    opnListSmallPdt.BringToFront();
                    //opnMenuPdtList.Visible = true;

                    cSale.nC_DTSeqNo = e.RowIndex + 1;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ogvOrder_CellMouseClick " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// เช็คการลดราคา
        /// </summary>
        private void W_PRCxCheckDis()
        {
            try
            {
                foreach (Button ocm in opnMenuPdt.Controls)
                {
                    if (ocm.Tag.ToString() == "C_KBDxDisAmt" || ocm.Tag.ToString() == "C_KBDxDisPer" || ocm.Tag.ToString() == "C_KBDxChgAmt" || ocm.Tag.ToString() == "C_KBDxChgPer")
                    {
                        ocm.Enabled = false;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "ogvOrder_CellMouseClick " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Close Menu Product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBackMenuPdt_Click(object sender, EventArgs e)
        {
            try
            {
                opnMenuPdtList.Visible = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmBackMenuPdt_Click " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Open Keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
                otbScan.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmKB_Click " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Product Qty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmPdtQty_Click(object sender, EventArgs e)
        {
            try
            {
                W_GETxFunctionPdt("KB044");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmPdtQty_Click " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Clear Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSale_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //otmClose.Stop();
                oW_Resource = null;
                oW_Sale = null;
                //new cSP().SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "wSale_FormClosing : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Change style view product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmViewList_Click(object sender, EventArgs e)
        {
            try
            {
                //if (string.Equals(ocmViewList.Tag.ToString(), "0"))
                //{
                //    ocmViewList.Image = Properties.Resources.All_32;
                //    ocmViewList.Text = "".PadLeft(10) + oW_Resource.GetString("tViewPdtImg");
                //    ocmViewList.Tag = 1;
                //    opnPdt.Visible = false;
                //    ogdProduct.Visible = true;
                //}
                //else
                //{
                //    ocmViewList.Image = Properties.Resources.List_32;
                //    ocmViewList.Text = "".PadLeft(10) + oW_Resource.GetString("tViewPdtList");
                //    ocmViewList.Tag = 0;
                //    opnPdt.Visible = true;
                //    ogdProduct.Visible = false;
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmViewList_Click " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmMenuBar_Click(object sender, EventArgs e)
        {
            string tFuncName;
            try
            {
                Button ocmMenu;
                ocmMenu = (Button)sender;
                tFuncName = ocmMenu.Tag.ToString();

                W_GETxFuncByFuncName(tFuncName); //*Arm 63-03-04
            }
            catch
            { }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Event Next Page Group Product
        /// </summary>
        private void ocmGrpNext_Click(object sender, EventArgs e)
        {
            try
            {
                //*Arm 62-11-15 Event Next Page Group Product
                //===========================================

                nW_GrpCurPage++;

                int nStartRow = (cVB.nVB_GrpPerPage * nW_GrpCurPage) - cVB.nVB_GrpPerPage;

                int nEndRow = 0;
                if (nW_GrpCurPage == nW_GrpMaxPage)     // หน้าสุดท้าย
                {
                    //nEndRow = aW_oPdtGrp.Count;
                    nEndRow = aW_oPdtTouchGrp.Count; //*Net 63-01-13 - เปลี่ยนมาใช้ TouchGroup
                }
                else
                {
                    nEndRow = cVB.nVB_GrpPerPage * nW_GrpCurPage;
                }

                W_GETxButtonPdtGrp(nStartRow, nEndRow);

                //**************************************
                // เช็คเงื่อนไขแสดงปุ่ม Next และปุ่ม Previous
                if (nW_GrpCurPage == nW_GrpMaxPage)     // หน้าสุดท้าย : แสดงปุ่มย้อนกลับ ปิดปุ่มถัดไป
                {
                    ocmGrpPrevious.Enabled = true;
                    ocmGrpNext.Enabled = false;
                }
                else
                {
                    ocmGrpPrevious.Enabled = true;
                    ocmGrpNext.Enabled = true;
                }

                //**************************************
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmGrpNext_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Event Previous Page Group Product
        /// </summary>
        private void ocmGrpPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                //*Arm 62-11-15 Event Previous Page Group Product
                //===============================================

                nW_GrpCurPage--;

                int nStartRow = (cVB.nVB_GrpPerPage * nW_GrpCurPage) - cVB.nVB_GrpPerPage;
                int nEndRow = cVB.nVB_GrpPerPage * (nW_GrpCurPage);

                W_GETxButtonPdtGrp(nStartRow, nEndRow);

                //**************************************
                // เช็คเงื่อนไขแสดงปุ่ม Next และปุ่ม Previous

                if (nW_GrpCurPage == 1)     // หน้าแรก : ปิดปุ่มย้อนกลับ
                {
                    ocmGrpPrevious.Enabled = false;
                    ocmGrpNext.Enabled = true;
                }
                else
                {
                    ocmGrpPrevious.Enabled = true;
                    ocmGrpNext.Enabled = true;
                }

                //**************************************
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmGrpPrevious_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Event Next Page MenuBill
        /// </summary>
        private void ocmMenuNext_Click(object sender, EventArgs e)
        {
            try
            {
                //*Arm 62-11-15 เงื่อนไขการกดปุ่ม Next Panel MenuBill
                //=============================================

                nW_MenuCurPage++;

                int nStartRow = (cVB.nVB_MenuPerPage * nW_MenuCurPage) - cVB.nVB_MenuPerPage;
                int nEndRow = 0;
                if (nW_MenuCurPage == nW_MenuMaxPage)   // หน้าสุดท้าย
                {
                    nEndRow = aW_oFunc.Count;
                }
                else
                {
                    nEndRow = cVB.nVB_MenuPerPage * nW_MenuCurPage;
                }

                W_GETxButtonMenuBill(nStartRow, nEndRow);

                //**************************************
                // เช็คเงื่อนไขแสดงปุ่ม Next และปุ่ม Previous
                if (nW_MenuCurPage == nW_MenuMaxPage)   // หน้าสุดท้าย : แสดงปุ่มย้อนกลับ ปิดปุ่มถัดไป
                {
                    ocmMenuPrev.Enabled = true;
                    ocmMenuNext.Enabled = false;
                }
                else
                {
                    ocmMenuPrev.Enabled = true;
                    ocmMenuNext.Enabled = true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "ocmMenuNext_Click : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Event Previous Page MenuBill
        /// </summary>
        private void ocmMenuPrev_Click(object sender, EventArgs e)
        {
            try
            {
                //*Arm 62-11-15 เงื่อนไขการกดปุ่ม Preview Panel MenuBill
                //================================================

                nW_MenuCurPage--;

                int nStartRow = ((nW_MenuCurPage) * cVB.nVB_MenuPerPage) - cVB.nVB_MenuPerPage;
                int nEndRow = cVB.nVB_MenuPerPage * (nW_MenuCurPage);

                W_GETxButtonMenuBill(nStartRow, nEndRow);

                //**************************************
                // เช็คเงื่อนไขแสดงปุ่ม Next และปุ่ม Previous
                if (nW_MenuCurPage == 1)            // หน้าแรก : ปิดปุ่มย้อนกลับ
                {
                    ocmMenuPrev.Enabled = false;
                    ocmMenuNext.Enabled = true;
                }
                else
                {
                    ocmMenuPrev.Enabled = true;
                    ocmMenuNext.Enabled = true;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmMenuPrev_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }
        #endregion End Event

        #region Function

        /// <summary>
        /// Set design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                ocmPayment.BackColor = cVB.oVB_ColDark;

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                opnCashPay.BackColor = cVB.oVB_ColDark;

                // Zen 63-03-11 ปรับสีในส่วน User
                opnNoCst.BackColor = cVB.oVB_ColDark;


                ocmSearch.BackColor = cVB.oVB_ColNormal;
                ocmScan.BackColor = cVB.oVB_ColNormal;
                opnMenuPdtList.BackColor = cVB.oVB_ColLight;
                //opnListSmallPdt.BackColor = cVB.oVB_ColLight;
                opnMenuPdtBack.BackColor = cVB.oVB_ColLight;
                opnMenuPdt.BackColor = cVB.oVB_ColLight;
                ocmBackMenuPdt.BackColor = cVB.oVB_ColDark;
                //ocmClear.BackColor = cVB.oVB_ColDark;
                //ocmAdd.BackColor = cVB.oVB_ColDark;
                //ocmValueQty.BackColor = cVB.oVB_ColLight;
                ocmMore.BackColor = cVB.oVB_ColDark;
                ocmBin.BackColor = cVB.oVB_ColDark;
                ocmFree.BackColor = cVB.oVB_ColDark;
                // olaDisplayGrp.BackColor = cVB.oVB_ColLight; //*Em 62-11-18 Edit By Zen 63-02-04

                // Gridview : Product
                //ogdProduct.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdProduct); //*Net 63-03-03 Set Design Gridview

                //oW_SP.SP_SETxSetGridviewFormat(ogdPdtStd); //*Arm 63-03-03
                oW_SP.SP_SETxSetGridFormat(ogdDetail);  //*Em 63-05-31

                //if (oW_SP.SP_CHKbConnection())
                //    opbPOS.Image = Properties.Resources.Online_32;
                //else
                //    opbPOS.Image = Properties.Resources.Offline_32;

                if (cVB.bVB_ModeScan)
                    opnScanMode.Visible = true;
                else
                    opnImgLstMode.Visible = true;

                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode, "TCNMUser");
                opbLogo.Image = new cCompany().C_GEToImageLogo();

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;

                if (cVB.nVB_DisplayOrder == 1)   // 1:Qty x Factor, 2:Qty Unit name
                {
                    ogdOrder.Columns[1].Visible = true;
                    ogdOrder.Columns[2].Visible = true;
                }
                else
                    ogdOrder.Columns[3].Visible = true;

                W_SETxColGrid(ogdDetail);   //*Em 63-05-31

                //ogdPdtStd.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //ogdPdtStd.RowHeadersDefaultCellStyle.Font = new Font("TH Sarabun New", 16F);

                //ogdPdtStd.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //ogdPdtStd.ColumnHeadersDefaultCellStyle.Font = new Font("TH Sarabun New", 16F);

                //ogdPdtStd.DefaultCellStyle.Font = new Font("TH Sarabun New", 16F);

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SETxDesign " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Text
        /// </summary>
        private void W_SETxText()
        {
            string tTitleSmg;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1:
                        cVB.tVB_KbdScreen = "SALESTD";
                        break;
                    case 2:
                        cVB.tVB_KbdScreen = "SALE";
                        break;
                }

                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resSale_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSale_EN));

                        break;
                }
                if (string.IsNullOrEmpty(cVB.tVB_CstCode))
                {
                    opnNoCst.Visible = true;
                    opnCst.Visible = false;
                }
                else
                {
                    opnCst.Visible = true;
                    opnNoCst.Visible = false;
                }
                //cVB.tVB_KbdScreen = "SALE"; //*Arm 63-04-08 comment code

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                olaPos.Text = cVB.tVB_PosCode;

                // Company
                if (!string.IsNullOrEmpty(cVB.tVB_Merchart))
                {
                    olaCompany.Text = cVB.tVB_MerName;  //*Em 62-10-04
                }
                else
                {
                    olaCompany.Text = cVB.tVB_CmpName;  //*Em 62-09-06
                }

                olaDetail1.Text = cVB.oVB_GBResource.GetString("tTaxInvoice");

                //*Em 62-10-03
                if (!string.IsNullOrEmpty(cVB.tVB_SmgCode))
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 ISNULL(FTSmgTitle,'') AS FTSmgTitle ");
                    oSql.AppendLine("FROM TCNMSlipMsgHD_L WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTSmgCode = '" + cVB.tVB_SmgCode + "' AND FNLngID = " + cVB.nVB_Language);
                    tTitleSmg = oDB.C_GEToDataQuery<string>(oSql.ToString());
                    olaDetail1.Text = tTitleSmg;
                }
                //+++++++++++++++

                olaDetail2.Text = cVB.oVB_GBResource.GetString("tTaxID") + " " + cVB.tVB_TaxID;
                if (cVB.tVB_VATInOrEx == "1")
                {
                    olaDetail3.Text = cVB.oVB_GBResource.GetString("tVatIncluded");
                }
                else
                {
                    olaDetail3.Text = cVB.oVB_GBResource.GetString("tVatExcluded");
                }


                // Content
                olaTitleTotal.Text = cVB.oVB_GBResource.GetString("tTotal") + ":";
                olaTitleQty.Text = cVB.oVB_GBResource.GetString("tQty") + ":";
                olaTitleSchBy.Text = cVB.oVB_GBResource.GetString("tSchBy");
                olaTitleSearch.Text = cVB.oVB_GBResource.GetString("tSearch");
                olaSale.Text = oW_Resource.GetString("tSale");
                olaSale.ForeColor = Color.Black;  //*Arm 63-04-28
                olaDocNo.ForeColor = Color.Black;  //*Arm 63-04-28

                //  olaTitleScan.Text = oW_Resource.GetString("tScan");

                // Customer
                //*Arm 63-03-23
                olaTitleTotalStd.Text = cVB.oVB_GBResource.GetString("tTotal") + ":";
                olaTitleQtyStd.Text = cVB.oVB_GBResource.GetString("tQty") + ":";
                //olaDisplayGrp.Text = "";    //*Em 62-11-18
                olaCstName.Text = oW_Resource.GetString("tSelectCst");
                olaTitlePriLev.Text = oW_Resource.GetString("tTitlePriLev");
                olaTitlePoint.Text = oW_Resource.GetString("tTitlePoint"); ;
                olaTitleExpd.Text = oW_Resource.GetString("tTitleExpd"); ;
                olaTitleSO.Text = oW_Resource.GetString("tTitleSO"); ;
                //++++++++++++++++++

                //olaTitleCstPoint.Text = cVB.oVB_GBResource.GetString("tPoint") + " : ";
                //olaTitleCstCrd.Text = cVB.oVB_GBResource.GetString("tCstCredit") + " : ";
                //olaTitleCstExp.Text = cVB.oVB_GBResource.GetString("tExpire") + " : ";
                //olaCstCredit.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                //olaCstPoint.Text = oW_SP.SP_SETtDecShwSve(1, 0, 0);


                // Search & Product Grp
                //olaTitleGrp.Text = oW_Resource.GetString("tGroupPdt");

                // User
                olaUsrName.Text = new cUser().C_GETtUsername();

                // Filter
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tPdtCode"));
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tPdtName"));
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tBarcode"));
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tUnitName"));
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tPrice"));
                ocbSearchBy.SelectedIndex = 0;

                // Gridview : Product
                otbTitlePdtCode.HeaderText = cVB.oVB_GBResource.GetString("tPdtCode");
                otbTitlePdtName.HeaderText = cVB.oVB_GBResource.GetString("tPdtName");
                otbTitlePdtBarcode.HeaderText = cVB.oVB_GBResource.GetString("tBarcode");
                otbTitlePdtPrice.HeaderText = cVB.oVB_GBResource.GetString("tPrice");

                // GridView : Product Std
                //otbColSeqStd.HeaderText = cVB.oVB_GBResource.GetString("tSeq");
                //otbColBarcodeStd.HeaderText = cVB.oVB_GBResource.GetString("tBarcode");
                //otbColPdtNameStd.HeaderText = cVB.oVB_GBResource.GetString("tPdtName");
                //otbColPdtQtyStd.HeaderText = cVB.oVB_GBResource.GetString("tQty");
                //otbColUnitNameStd.HeaderText = cVB.oVB_GBResource.GetString("tUnit");
                //otbColSetPriceStd.HeaderText = cVB.oVB_GBResource.GetString("tPrice");
                //otbColDiscount.HeaderText = cVB.oVB_GBResource.GetString("tDis");
                //otbColPdtSumPriceStd.HeaderText = cVB.oVB_GBResource.GetString("tSummary");

                //*Em 63-05-31
                ogdDetail.Cols["otbColSeqStd"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                ogdDetail.Cols["otbColBarcodeStd"].Caption = cVB.oVB_GBResource.GetString("tBarcode");
                ogdDetail.Cols["otbColPdtNameStd"].Caption = cVB.oVB_GBResource.GetString("tPdtName");
                ogdDetail.Cols["otbColPdtQtyStd"].Caption = cVB.oVB_GBResource.GetString("tQty");
                ogdDetail.Cols["otbColUnitNameStd"].Caption = cVB.oVB_GBResource.GetString("tUnit");
                ogdDetail.Cols["otbColSetPriceStd"].Caption = cVB.oVB_GBResource.GetString("tPrice");
                ogdDetail.Cols["otbColDiscount"].Caption = cVB.oVB_GBResource.GetString("tDis");
                ogdDetail.Cols["otbColPdtSumPriceStd"].Caption = cVB.oVB_GBResource.GetString("tSummary");
                //++++++++++++++++

                ocmBackMenuPdt.Text = cVB.oVB_GBResource.GetString("tBack");


                olaPagePdt.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), "0", "0", "0");
                ocmPayment.Text = cVB.oVB_GBResource.GetString("tPayment");
                //olaDisplayGrp.Text = "";    //*Em 62-11-18

                //*Net 63-07-31 ปรับตาม Moshi
                olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tWelcome");    //*Em 63-07-26
                olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม
                olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม
                olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม

                this.Text = Assembly.GetExecutingAssembly().GetName().Name;   //*Em 63-07-11

                //*Em 63-07-26
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                }
                //+++++++++++++

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SETxText " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        
        /// <summary>
        /// แสดงข้อมูลลูกค้าในหน้าจอขาย
        /// </summary>
        public void W_SETxTextCst()
        {
            try
            {
                ////*Arm 62-10-26
                ////olaCstCode.Text = cVB.tVB_CstCode;
                //olaCstName.Text = cVB.oVB_CstCard.tCstName;
                //olaPoint.Text = cVB.oVB_CstCard.nCstPoint.ToString();
                //olaPriLev.Text = string.IsNullOrEmpty(cVB.oVB_CstCard.tPriceGrpName)? cVB.oVB_CstCard.tCstPriceGroup: cVB.oVB_CstCard.tPriceGrpName;
                //olaExpired.Text = (cVB.oVB_CstCard.dCstCrdExpire.HasValue) ? cVB.oVB_CstCard.dCstCrdExpire.Value.ToString("dd/MM/yyyy") : "";
                ////olaCstTel.Text = cVB.tVB_CstTel;
                ////olaCstPriGrp.Text = cVB.tVB_PriceGroup;


                //*Arm  63-04-03 
                //olaCstName.Text = cVB.tVB_CstName;

                //*Em 63-08-14
                if (string.IsNullOrEmpty(cVB.tVB_MemCode))
                {
                    olaCstName.Text = cVB.tVB_CstName;
                    olaCstName.ForeColor = Color.Black;
                }
                else
                {
                    olaCstName.Text = "*" + cVB.tVB_CstName;
                    olaCstName.ForeColor = Color.Blue;
                }
                //++++++++++++++

                //olaPoint.Text = cVB.nVB_CstPiontB4Used.ToString();
                olaPoint.Text = oW_SP.SP_SETtDecShwSve(1, cVB.nVB_CstPiontB4Used, 0); //*Em 63-07-11
                olaPriLev.Text = cVB.tVB_PriceGroup;
                if (string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm  63-04-29
                {
                    olaExpired.Text = "";
                }
                else
                {
                    olaExpired.Text = string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(cVB.tVB_ExpiredDate));
                }

                //++++++++++++++


                opnDetailCmp.Visible = false;
                opnCst.Visible = true;
                opnNoCst.Visible = false;
                opnQMember.Visible = false;

                //*Net 63-07-31 แสดงผล ลูกค้า หน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxCustomerOfDef(olaCstName.Text, olaPoint.Text);
                }
                //+++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SETxTextCst " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Select QueueName
        /// </summary>
        public void W_CSTxQueueMember(string ptQData)
        {
            try
            {
                //*Arm 62-10-26
                string[] aData = ptQData.Split('|');
                if (cSale.nC_CntItem > 0)
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantSelectCst"), 3);
                    opnQMember.Visible = false;
                    cSP.SP_GETxCountQMember(olaMsgCountQMem, opnQMember); //*Arm 62-11-07
                    return;
                }
                cVB.tVB_CstCode = aData[0].ToString();
                cVB.tVB_CstName = aData[1].ToString();
                cVB.tVB_CstTel = aData[2].ToString();
                cVB.tVB_PriceGroup = aData[3].ToString();
                cVB.tVB_QMemMsgID = aData[4].ToString();
                cVB.bVB_ScanQR = true;  //*Arm 62-12-23

                opnQMember.Visible = false;
                W_SETxTextCst();
                cSP.SP_GETxCountQMember(olaMsgCountQMem, opnQMember); //*Arm 62-10-30

                cSale.C_DATxInsHDCst(cVB.tVB_CstCode);  //*Arm 62-11-08 
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_CSTxQueueMember " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Add Product
        /// </summary>
        public void W_ADDxPdtToOrder(cmlPdtOrder poOrder)
        {
            DataGridViewRow oOrder;
            decimal cSum, cQty;
            int nRowFind = 0; //*Em 63-05-31
            string tStaPdt = "";    //*Em 63-06-09
            try
            {
                //new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : Start"); //*Arm 63-05-21
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1: // Standrad

                        //*Em 63-05-31
                        oOrder = null;
                        if (nW_Mode == 4)   //*Em 63-04-29
                        {
                            //*Net 63-06-26 ถ้าเป็นแบบรวมรายการ และไม่ใช่รายการ void ให้ค้นหา row ที่มีอยู่
                            if (cVB.nVB_StaSumScan == 1 && poOrder.tStaPdt != "4")
                            {
                                nRowFind = ogdDetail.FindRow(poOrder.tBarcode, ogdDetail.Rows.Fixed, 1, false);

                                if (nRowFind > 0)
                                {
                                NextFind:
                                    tStaPdt = ogdDetail.GetData(nRowFind, ogdDetail.Cols["otbColStaPdtStd"].Index).ToString();
                                    if (tStaPdt == "4")
                                    {
                                        nRowFind = ogdDetail.FindRow(poOrder.tBarcode, nRowFind + 1, 1, false);
                                        if (nRowFind > 0) goto NextFind;
                                    }
                                }
                                //++++++++++++++++
                            }

                            if (nRowFind > 0) //ถ้าเจอรายการซ้ำ
                            {
                                new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : C_PRCxUpdateQty", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                                if (oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                                {
                                    DataRow oRow = oW_dtTmp.Rows[oOrder.Index];
                                    oRow["otbColPdtQtyStd"] = oW_dtTmp.Rows[oOrder.Index].Field<decimal>("otbColPdtQtyStd") + poOrder.cQty;
                                    oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, oW_dtTmp.Rows[oOrder.Index].Field<decimal>("otbColPdtSumPriceStd") + (poOrder.cSetPrice * poOrder.cQty), cVB.nVB_DecShow));

                                    cSale.nC_DTSeqNo = oOrder.Index + 1;
                                    if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty(poOrder.cQty);
                                }
                                else
                                {
                                    // Qty
                                    ogdDetail.SetData(nRowFind, 3, oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nRowFind, 3)) + poOrder.cQty, cVB.nVB_DecShow));
                                    ogdDetail.SetData(nRowFind, 7, oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nRowFind, 7)) + (poOrder.cSetPrice * poOrder.cQty), cVB.nVB_DecShow));
                                    //cSale.nC_DTSeqNo = nRowFind + ogdDetail.Rows.Fixed;
                                    cSale.nC_DTSeqNo = nRowFind; //*Net 63-06-03 เอา Row Index ที่เจอมาใช้เลย เพราะ RowData เริ่มที่ 1 อยู่แล้ว
                                    if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty(poOrder.cQty); //*Arm 63-05-07

                                    //*Net 63-07-08 เอารายการที่ +qty แล้วมา show หน้าจอ 2
                                    if (cVB.oVB_CstScreen != null)
                                    {
                                        cVB.oVB_CstScreen.W_SETxPDTGrid(nRowFind, 2,
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nRowFind, 3)), cVB.nVB_DecShow));
                                        cVB.oVB_CstScreen.W_SETxPDTGrid(nRowFind, 3,
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nRowFind, 7)), cVB.nVB_DecShow));
                                    }
                                    //++++++++++++++++++++++++++++++++++++++++++
                                }
                            }
                            else //ถ้าเป็นแบบ แยกรายการ หรือ ไม่ใช่รายการซ้ำ หรือรายการ Void
                            {
                                if (poOrder.tStaPdt == "4") //*ถ้าเป็นรายการ Void
                                {
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 10, "4");

                                    ogdDetail.Rows.Add();
                                    cSale.nC_DTSeqNo = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 0, cSale.nC_DTSeqNo);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 1, poOrder.tBarcode);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 2, poOrder.tPdtName);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 3, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 4, poOrder.tUnit);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 5, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 6, oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 7, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 8, poOrder.tPdtCode);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 9, poOrder.cFactor);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 10, poOrder.tStaPdt);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 11, poOrder.tStaAlwDis);

                                    //*Net 63-07-08 เพิ่มรายการสินค้าใหม่ หน้าจอ 2
                                    if (cVB.oVB_CstScreen != null)
                                    {
                                        cVB.oVB_CstScreen.W_ADDxPDTGrid(cSale.nC_DTSeqNo,
                                            poOrder.tPdtName,
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 3)), cVB.nVB_DecShow),
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 7)), cVB.nVB_DecShow));
                                    }
                                    //+++++++++++++++++++++++++++++++++++++++
                                }
                                else //*ถ้าเป็นแบบแยก หรือไม่ใช่รายการซ้ำ
                                {
                                    ogdDetail.Rows.Add();
                                    cSale.nC_DTSeqNo = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 0, cSale.nC_DTSeqNo);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 1, poOrder.tBarcode);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 2, poOrder.tPdtName);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 3, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 4, poOrder.tUnit);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 5, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 6, oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 7, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 8, poOrder.tPdtCode);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 9, poOrder.cFactor);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 10, poOrder.tStaPdt);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 11, poOrder.tStaAlwDis);

                                    //*Net 63-07-08 เพิ่มรายการสินค้าใหม่ หน้าจอ 2
                                    if (cVB.oVB_CstScreen != null)
                                    {
                                        cVB.oVB_CstScreen.W_ADDxPDTGrid(cSale.nC_DTSeqNo,
                                            poOrder.tPdtName,
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 3)), cVB.nVB_DecShow),
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 7)), cVB.nVB_DecShow));
                                    }
                                    //+++++++++++++++++++++++++++++++++++++++

                                    cSale.C_DATxInsDT(poOrder, cSale.nC_DTSeqNo, "2"); //*Net 63-06-26 ถ้าเป็นรายการคืน ให้ใช้ StaPdt=2
                                }

                            }


                            //new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : C_DATxInsDT"); //*Arm 63-05-21

                            //cSale.C_DATxInsDT(poOrder, cSale.nC_DTSeqNo);
                        }
                        else
                        {

                            //if (cVB.nPS_StaSumPdt == 2)
                            if (cVB.nVB_StaSumScan == 1 && poOrder.tStaPdt != "4")
                            {
                                nRowFind = ogdDetail.FindRow(poOrder.tBarcode, ogdDetail.Rows.Fixed, 1, false);

                                //*Em 63-06-09
                                if (nRowFind > 0)
                                {
                                NextFind:
                                    tStaPdt = ogdDetail.GetData(nRowFind, ogdDetail.Cols["otbColStaPdtStd"].Index).ToString();
                                    if (tStaPdt == "4")
                                    {
                                        nRowFind = ogdDetail.FindRow(poOrder.tBarcode, nRowFind + 1, 1, false);
                                        if (nRowFind > 0) goto NextFind;
                                    }
                                }
                                //++++++++++++++++
                            }

                            if (nRowFind > 0)
                            {
                                new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : C_PRCxUpdateQty", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                                if (oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                                {
                                    DataRow oRow = oW_dtTmp.Rows[oOrder.Index];
                                    oRow["otbColPdtQtyStd"] = oW_dtTmp.Rows[oOrder.Index].Field<decimal>("otbColPdtQtyStd") + poOrder.cQty;
                                    oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, oW_dtTmp.Rows[oOrder.Index].Field<decimal>("otbColPdtSumPriceStd") + (poOrder.cSetPrice * poOrder.cQty), cVB.nVB_DecShow));

                                    cSale.nC_DTSeqNo = oOrder.Index + 1;
                                    if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty(poOrder.cQty);
                                }
                                else
                                {
                                    //*Arm 63-09-18 Clear ส่วนลด
                                    if (Convert.ToDecimal(ogdDetail.GetData(nRowFind, ogdDetail.Cols["otbColDiscount"].Index).ToString()) != 0m)
                                    {
                                        if (new cSP().SP_SHWoMsg(cVB.oVB_GBResource.GetString("tMsgClearDisc"), 1) == DialogResult.No)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            ogdDetail.SetData(nRowFind, 6, 0);
                                            cSale.C_PRCxClearDisItem(nRowFind);
                                        }
                                    }
                                    //++++++++++++++++++++++++++++

                                    // Qty
                                    ogdDetail.SetData(nRowFind, 3, oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nRowFind, 3)) + poOrder.cQty, cVB.nVB_DecShow));
                                    //ogdDetail.SetData(nRowFind, 7, oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nRowFind, 7)) + (poOrder.cSetPrice * poOrder.cQty), cVB.nVB_DecShow)); //*Arm 63-09-18 Comment Code
                                    ogdDetail.SetData(nRowFind, 7, oW_SP.SP_SETtDecShwSve(1, (Convert.ToDecimal(ogdDetail.GetData(nRowFind, 3))* poOrder.cSetPrice), cVB.nVB_DecShow)); //*Arm 63-09-18
                                    //cSale.nC_DTSeqNo = nRowFind + ogdDetail.Rows.Fixed;
                                    cSale.nC_DTSeqNo = nRowFind; //*Net 63-06-03 เอา Row Index ที่เจอมาใช้เลย เพราะ RowData เริ่มที่ 1 อยู่แล้ว
                                    if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty(poOrder.cQty); //*Arm 63-05-07

                                    //*Net 63-07-31 เอารายการที่ +qty แล้วมา show หน้าจอ 2
                                    if (cVB.oVB_CstScreen != null)
                                    {
                                        cVB.oVB_CstScreen.W_SETxPDTGrid(nRowFind, 2,
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nRowFind, 3)), cVB.nVB_DecShow));
                                        cVB.oVB_CstScreen.W_SETxPDTGrid(nRowFind, 3,
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nRowFind, 7)), cVB.nVB_DecShow));
                                    }
                                    //++++++++++++++++++++++++++++++++++++++++++
                                }
                            }
                            else
                            {
                                if (poOrder.tStaPdt == "4")
                                {
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 10, "4");

                                    ogdDetail.Rows.Add();
                                    cSale.nC_DTSeqNo = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 0, cSale.nC_DTSeqNo);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 1, poOrder.tBarcode);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 2, poOrder.tPdtName);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 3, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 4, poOrder.tUnit);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 5, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 6, oW_SP.SP_SETtDecShwSve(1, poOrder.cDis - poOrder.cChg, cVB.nVB_DecShow)); //*Net 63-06-05 ใส่ส่วนลด/ชาร์จ
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 7, oW_SP.SP_SETtDecShwSve(1, (poOrder.cSetPrice * poOrder.cQty) + (poOrder.cDis - poOrder.cChg), cVB.nVB_DecShow)); //*Net 63-06-05 บวกส่วนลดชาร์จเข้าไป (qty เป็นลบ)
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 8, poOrder.tPdtCode);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 9, poOrder.cFactor);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 10, poOrder.tStaPdt);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 11, poOrder.tStaAlwDis);

                                    //*Net 63-07-31 เพิ่มรายการสินค้าใหม่ หน้าจอ 2
                                    if (cVB.oVB_CstScreen != null)
                                    {
                                        cVB.oVB_CstScreen.W_ADDxPDTGrid(cSale.nC_DTSeqNo,
                                            poOrder.tPdtName,
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 3)), cVB.nVB_DecShow),
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 7)), cVB.nVB_DecShow));
                                    }
                                    //+++++++++++++++++++++++++++++++++++++++

                                }
                                else
                                {
                                    ogdDetail.Rows.Add();
                                    cSale.nC_DTSeqNo = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 0, cSale.nC_DTSeqNo);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 1, poOrder.tBarcode);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 2, poOrder.tPdtName);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 3, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 4, poOrder.tUnit);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 5, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 6, oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 7, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow));
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 8, poOrder.tPdtCode);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 9, poOrder.cFactor);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 10, poOrder.tStaPdt);
                                    ogdDetail.SetData(cSale.nC_DTSeqNo, 11, poOrder.tStaAlwDis);

                                    //*Net 63-07-31 เพิ่มรายการสินค้าใหม่ หน้าจอ 2
                                    if (cVB.oVB_CstScreen != null)
                                    {
                                        cVB.oVB_CstScreen.W_ADDxPDTGrid(cSale.nC_DTSeqNo,
                                            poOrder.tPdtName,
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 3)), cVB.nVB_DecShow),
                                            oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 7)), cVB.nVB_DecShow));
                                    }
                                    //+++++++++++++++++++++++++++++++++++++++


                                    //new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : C_DATxInsDT"); //*Arm 63-05-21
                                    if (cVB.bVB_RetriveBill == false)
                                    {
                                        //*Arm 62-11-18  Check การคืน
                                        if (cSale.nC_DocType == 9)
                                        {
                                            cSale.C_DATxInsDT(poOrder, cSale.nC_DTSeqNo, "2");
                                        }
                                        else
                                        {
                                            cSale.C_DATxInsDT(poOrder, cSale.nC_DTSeqNo);
                                        }
                                    }
                                }
                            }
                        }


                        //ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];


                        //new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : Summary Qty"); //*Arm 63-05-21
                        cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 7, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 7));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        //new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : Summary Price"); //*Arm 63-05-21
                        cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 3, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 3));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                        ogdDetail.Row = cSale.nC_DTSeqNo;

                        //*Net 63-08-17 เมื่อเพิ่มสินค้าให้คำนวนความกว้างคอลัมสุดท้ายใหม่
                        ogdDetail.Cols["otbColPdtSumPriceStd"].Width = ((ogdDetail.Width - 50) * 18 / 100) - (ogdDetail.Width - ogdDetail.ScrollableRectangle.Width+5);

                        //*Net 63-07-31 update ยอดรวมที่หน้าจอ 2
                        if (cVB.oVB_CstScreen != null)
                        {
                            cVB.oVB_CstScreen.W_SETxSummaryAmt(olaTotalStd.Text);
                        }
                        //+++++++++++++++++++++++++
                        break;

                    case 2: // TouchScreen
                        oOrder = (from oPdt in ogdOrder.Rows.Cast<DataGridViewRow>()
                                  where
                                    oPdt.Cells["olaPdtCode"].Value.ToString() == poOrder.tPdtCode &&
                                    oPdt.Cells["otbBarcode"].Value.ToString() == poOrder.tBarcode &&
                                    oPdt.Cells["otbStaPdt"].Value.ToString() == poOrder.tStaPdt &&
                                    Convert.ToDecimal(oPdt.Cells["olaSetPrice"].Value) == poOrder.cSetPrice &&
                                    Convert.ToDecimal(oPdt.Cells["otbFactor"].Value) == poOrder.cFactor
                                  select oPdt).FirstOrDefault();

                        if (oOrder == null || poOrder.tStaPdt == "4" || poOrder.tStaPdt == "3")
                        {

                            if (poOrder.tStaPdt == "4")
                            {
                                ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["otbStaPdt"].Value = "4";

                                ogdOrder.Rows.Add(oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), "x", poOrder.cFactor, poOrder.tUnit, poOrder.tPdtCode, poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow), poOrder.tStaPdt, poOrder.tStaAlwDis);
                                ogdOrder.Refresh(); //*Net 63-05-25
                                this.Refresh(); //*Net 63-05-25
                                cSale.nC_DTSeqNo = ogdOrder.Rows.GetRowCount(DataGridViewElementStates.None);

                            }
                            else
                            {
                                ogdOrder.Rows.Add(oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), "x", poOrder.cFactor, poOrder.tUnit, poOrder.tPdtCode, poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow), poOrder.tStaPdt, poOrder.tStaAlwDis);
                                ogdOrder.Refresh(); //*Net 63-05-25
                                this.Refresh(); //*Net 63-05-25
                                cSale.nC_DTSeqNo = ogdOrder.Rows.GetRowCount(DataGridViewElementStates.None);

                                //*Arm 62-11-18  Check การคืน
                                if (cSale.nC_DocType == 9)
                                {
                                    //cSale.C_DATxInsDT(poOrder, "2");
                                    new Thread(()=> cSale.C_DATxInsDT(poOrder, cSale.nC_DTSeqNo, "2"))
                                    {
                                        IsBackground = true
                                    }.Start(); //*Net 63-05-26 สร้าง Thread Insert DT
                                }
                                else
                                {
                                    //cSale.C_DATxInsDT(poOrder);
                                    new Thread(()=> cSale.C_DATxInsDT(poOrder, cSale.nC_DTSeqNo))
                                    {
                                        IsBackground = true
                                    }.Start(); //*Net 63-05-26 สร้าง Thread Insert DT
                                }
                            }
                        }
                        else
                        {
                            // Qty
                            ogdOrder.Rows[oOrder.Index].Cells["olaPdtQty"].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[oOrder.Index].Cells["olaPdtQty"].Value) + 1, cVB.nVB_DecShow);
                            // Price
                            ogdOrder.Rows[oOrder.Index].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[oOrder.Index].Cells["olaPdtSumPrice"].Value) + poOrder.cSetPrice, cVB.nVB_DecShow);   //*Em 62-10-08

                            cSale.nC_DTSeqNo = oOrder.Index + 1;
                            cSale.C_PRCxUpdateQty();
                        }

                        ogdOrder.CurrentCell = ogdOrder.Rows[ogdOrder.RowCount - 1].Cells[0];
                        ogdOrder.Refresh(); //*Net 63-05-25
                        this.Refresh(); //*Net 63-05-25

                        cSum = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells["olaPdtSumPrice"].Value));
                        olaTotal.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells["olaPdtQty"].Value));
                        olaQty.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdOrder.RowCount;

                        break;
                }

                // Show Last Event
                olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(poOrder.cSetPrice * poOrder.cQty), cVB.nVB_DecShow).ToString();
                olaTitleCashPay.Text = poOrder.tPdtName.ToString();
                //*Net 63-07-31 แสดงรายการล่าสุดที่หน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                }
                //++++++++++++++++++++++++++++++++++++++

                //Application.DoEvents(); //*Em 63-05-26
                //this.Refresh(); //*Em 63-05-27
                //*Em 63-04-25
                if (nW_Mode == 3)
                {
                    if (cVB.oVB_ReferSO != null && cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                    {
                        //not thing
                    }
                    else
                    {
                        if (cSale.nC_DTSeqNo == 1 && (aoW_CalPro == null || aoW_CalPro.Count == 0))
                        {
                            cSale.C_DATxStamDateTimeHD();  //*Em 63-06-09

                            //*Arm 63-05-27
                            Thread oCalPro;
                            if (cVB.bVB_PmtPriGrp == true) //ถ้ามีโปรโมชั่นกลุ่มราคา
                            {
                                bW_CalPmtPrice = true; //คำนวณโปรโมชั่นกลุ่มราคา
                                new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : Thread(W_PRCxCoPmt)", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                                if (aoW_CalPro == null) aoW_CalPro = new List<Thread>(); //*Net 63-05-24
                                oCalPro = new Thread(W_PRCxCoPmt);
                                oCalPro.IsBackground = true;
                                oCalPro.Priority = ThreadPriority.Highest;
                                oCalPro.Start();
                                aoW_CalPro.Add(oCalPro); //*Net 63-05-21
                            }
                            else
                            {
                                if (cVB.bVB_PmtDis == true)  //Check ถ้ามีโปรโมชั่นส่วนลด
                                {
                                    bW_CalPmtPrice = false; //ไม่คำนวณโปรโมชั่นกลุ่มราคา
                                    new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder : Thread(W_PRCxCoPmt)", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                                    if (aoW_CalPro == null) aoW_CalPro = new List<Thread>(); //*Net 63-05-24
                                    oCalPro = new Thread(W_PRCxCoPmt);
                                    oCalPro.IsBackground = true;
                                    oCalPro.Priority = ThreadPriority.Highest;
                                    oCalPro.Start();
                                    aoW_CalPro.Add(oCalPro); //*Net 63-05-21
                                }
                            }
                            //+++++++++++++
                        }
                    }
                }
                //+++++++++++++++++++++++++
                //if (nW_a == 0)
                //{
                //    if (bCoCal == false)
                //    {
                //        bCoCal = new cPdtPmt().C_PRCoCalPmt();
                //        if (cVB.oVB_GetPmt.Rows.Count > 0)
                //        {
                //            bCoCal = true;
                //            nW_a = 1;
                //        }
                //        else
                //        {
                //            bCoCal = false;
                //            nW_a = 0;
                //        }
                //    }
                //    else if (cVB.oVB_GetPmt.Rows.Count > 0)
                //    {
                //        bCoCal = true;
                //        nW_a = 1;
                //    }
                //    else
                //    {
                //        bCoCal = new cPdtPmt().C_PRCoCalPmt();
                //        if (cVB.oVB_GetPmt.Rows.Count > 0)
                //        {
                //            bCoCal = true;
                //            nW_a = 1;
                //        }
                //        else
                //        {
                //            bCoCal = false;
                //            nW_a = 0;
                //        }
                //    }
                //}


                //if (cVB.oVB_GetPmt.Rows.Count > 0)
                //{
                //    bCoCal = true;
                //}
                //else
                //{
                //    bCoCal = new cPdtPmt().C_PRCoCalPmt();
                //}
                //if (cVB.oVB_PmtSug.Rows.Count > 0)
                //{
                //    bCoCalSu = true;
                //}
                //else
                //{
                //    bCoCalSu = new cPdtPmt().C_PRCbCalPmtSuggest();
                //}
                //new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder End"); //*Arm 63-05-21
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder " + oEx.Message); }
            finally
            {
                poOrder = null;
                oOrder = null;
                cW_SetQty = 1; //*Arm 63-05-03 กำหนดจำนวนสินค้าเริ่มต้นกลับเป็น 1 สำหรับรายการต่อไป
                ogdOrder.ClearSelection();

                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Change PdtQty
        /// </summary>
        public void W_CHAxChangePdtQty()  //*Arm 62-10-07
        {
            decimal cSum, cQty;

            try
            {
                new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : Start", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1: // Standrad
                        new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : C_PRCxChangeQty", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                        
                        //*Arm 63-09-18 Check แจ้งเตือนกรณีมีส่วนลด
                        if (Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, ogdDetail.Cols["otbColDiscount"].Index).ToString()) != 0m)
                        {
                            if (new cSP().SP_SHWoMsg(cVB.oVB_GBResource.GetString("tMsgClearDisc"), 1) == DialogResult.No)
                            {
                                return;
                            }
                        }
                        //++++++++++++++++++++++++++++

                        cSale.C_PRCxChangeQty(cSale.cC_DTQty);

                        //if (oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                        //{
                        //    oW_dtTmp.Columns[otbColDiscount.Name].ReadOnly = false;

                        //    DataRow oRow = oW_dtTmp.Rows[cSale.nC_DTSeqNo - 1];
                        //    oRow["otbColPdtQtyStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow));
                        //    oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow));
                        //    oRow["otbColDiscount"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1,
                        //     (Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) *
                        //     Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value)) -
                        //     cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow));
                        //}
                        //else
                        //{
                        //    // Qty
                        //    ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow);
                        //    // Price
                        //    //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value), cVB.nVB_DecShow);
                        //    //*Net 63-04-10
                        //    ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                        //    ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColDiscount.Name].Value = oW_SP.SP_SETtDecShwSve(1,
                        //         (Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) *
                        //         Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value)) -
                        //         cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                        //    //++++++++++++++++++++
                        //}
                        ////// Qty
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow);
                        ////// Price
                        //////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value), cVB.nVB_DecShow);
                        //////*Net 63-04-10
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColDiscount.Name].Value = oW_SP.SP_SETtDecShwSve(1,
                        ////     (Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * 
                        ////     Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value)) -
                        ////     cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                        //////++++++++++++++++++++

                        //ogdPdtStd.CurrentCell = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[0];  //*Em 63-05-27

                        //new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : Summary Price"); //*Arm 63-05-21
                        //// Show Total Price
                        //cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        //olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        //new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : Summary Qty"); //*Arm 63-05-21
                        //// Show Totol Qty
                        //cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        //olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        //// total Item
                        //cSale.nC_CntItem = ogdPdtStd.RowCount;

                        //// Show Last Event
                        //olaCashPayment.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value.ToString();
                        //olaTitleCashPay.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        //// Select And Show Rows Last Update
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Selected = true; // Select ข้อมูลล่าสุด 
                        //ogdPdtStd.FirstDisplayedScrollingRowIndex = cSale.nC_DTSeqNo - 1; // Scrol 

                        //*Em 63-05-31
                        ogdDetail.SetData(cSale.nC_DTSeqNo,ogdDetail.Cols["otbColPdtQtyStd"].Index, oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow));
                        // Price
                        ogdDetail.SetData(cSale.nC_DTSeqNo, ogdDetail.Cols["otbColPdtSumPriceStd"].Index,  oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow));
                        ogdDetail.SetData(cSale.nC_DTSeqNo, ogdDetail.Cols["otbColDiscount"].Index, oW_SP.SP_SETtDecShwSve(1,
                             (Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, ogdDetail.Cols["otbColPdtQtyStd"].Index)) *
                             Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, ogdDetail.Cols["otbColSetPriceStd"].Index))) -
                             cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow));

                        ogdDetail.RowSel = cSale.nC_DTSeqNo;

                        new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : Summary Price", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                        // Show Total Price
                        cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 7, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 7));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : Summary Qty", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                        // Show Totol Qty
                        cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 3, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 3));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        // total Item
                        cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;

                        // Show Last Event
                        //olaCashPayment.Text = ogdDetail.GetData(cSale.nC_DTSeqNo,7).ToString();
                        olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 7).ToString()), cVB.nVB_DecShow); //*Net 63-06-26
                        olaTitleCashPay.Text = ogdDetail.GetData(cSale.nC_DTSeqNo, 2).ToString();

                        // Select And Show Rows Last Update
                        //+++++++++++++++
                        break;

                    case 2:


                        cSale.C_PRCxChangeQty(cSale.cC_DTQty);
                        // Qty
                        ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtQty"].Value = oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow);
                        // Price
                        //ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtQty"].Value) * Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaSetPrice"].Value), cVB.nVB_DecShow);
                        ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);

                        //ogdOrder.CurrentCell = ogdOrder.Rows[ogdOrder.RowCount - 1].Cells[0];

                        // Show Total Price
                        cSum = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells["olaPdtSumPrice"].Value));
                        olaTotal.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        // Show Totol Qty
                        cQty = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells["olaPdtQty"].Value));
                        olaQty.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        // total Item
                        cSale.nC_CntItem = ogdOrder.RowCount;

                        // Show Last Event
                        //olaCashPayment.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString();
                        olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString()), cVB.nVB_DecShow); //*Net 63-06-26
                        olaTitleCashPay.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtName"].Value.ToString();

                        break;
                }

                //*Em 63-04-29
                if (nW_Mode == 3)
                {
                    if (cVB.oVB_ReferSO != null && cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                    {
                        //not thing
                    }
                    else
                    {
                        //new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : Thread(W_PRCxCoPmt)"); //*Arm 63-05-21
                        //bW_CalPmtPrice = true;  //*Em 63-05-05
                        //if (aoW_CalPro == null) aoW_CalPro = new List<Thread>(); //*Net 63-05-24
                        //Thread oCalPro = new Thread(W_PRCxCoPmt);
                        //oCalPro.IsBackground = true;
                        //oCalPro.Priority = ThreadPriority.Highest;
                        //oCalPro.Start();
                        //aoW_CalPro.Add(oCalPro); //*Net 63-05-21

                        ////*Arm 63-05-27
                        //Thread oCalPro;
                        //if (cVB.bVB_PmtPriGrp == true) //ถ้ามีโปรโมชั่นกลุ่มราคา
                        //{
                        //    bW_CalPmtPrice = true; //คำนวณโปรโมชั่นกลุ่มราคา
                        //    new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : Thread(W_PRCxCoPmt)"); //*Arm 63-05-21
                        //    if (aoW_CalPro == null) aoW_CalPro = new List<Thread>(); //*Net 63-05-24
                        //    oCalPro = new Thread(W_PRCxCoPmt);
                        //    oCalPro.IsBackground = true;
                        //    oCalPro.Priority = ThreadPriority.Highest;
                        //    oCalPro.Start();
                        //    aoW_CalPro.Add(oCalPro); //*Net 63-05-21
                        //}
                        //else
                        //{
                        //    if (cVB.bVB_PmtDis == true)  //Check ถ้ามีโปรโมชั่นส่วนลด
                        //    {
                        //        bW_CalPmtPrice = false; //ไม่คำนวณโปรโมชั่นกลุ่มราคา
                        //        new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : Thread(W_PRCxCoPmt)"); //*Arm 63-05-21
                        //        if (aoW_CalPro == null) aoW_CalPro = new List<Thread>(); //*Net 63-05-24
                        //        oCalPro = new Thread(W_PRCxCoPmt);
                        //        oCalPro.IsBackground = true;
                        //        oCalPro.Priority = ThreadPriority.Highest;
                        //        oCalPro.Start();
                        //        aoW_CalPro.Add(oCalPro); //*Net 63-05-21
                        //    }
                        //}
                        ////+++++++++++++
                    }
                }
                //+++++++++++++++++++++++++
                new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty : End", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty " + oEx.Message); }
            finally
            {
                //poOrder = null;
                //oOrder = null;
                ogdOrder.ClearSelection();

                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// สินค้าฟรี
        /// </summary>
        public void W_FRExItemFree()  //*Arm 62-10-07
        {
            decimal cSum, cQty;

            try
            {
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1: // Standrad

                        //if (oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                        //{
                        //    DataRow oRow = oW_dtTmp.Rows[cSale.nC_DTSeqNo - 1];
                        //    oRow["otbColPdtNameStd"] = "Free-" + ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();
                        //    oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * 0, cVB.nVB_DecShow));
                        //    oRow["otbColStaPdtStd"] = "3";
                        //    oRow["otbColSetPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow));
                        //}
                        //else
                        //{
                        //    // PdtName = 'Free-'+ PdtName
                        //    ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value = "Free-" + ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        //    // SumPrice = Qty * SetPrice  
                        //    ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * 0, cVB.nVB_DecShow);

                        //    // StaPrd = 3 : สินค้าฟรี
                        //    ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColStaPdtStd.Name].Value = "3";

                        //    // SetPrice = 0
                        //    ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                        //}

                        ////// PdtName = 'Free-'+ PdtName
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value = "Free-" + ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        ////// SumPrice = Qty * SetPrice  
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * 0, cVB.nVB_DecShow);

                        ////// StaPrd = 3 : สินค้าฟรี
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColStaPdtStd.Name].Value = "3";

                        ////// SetPrice = 0
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);

                        //// Process Update
                        //cSale.C_PRCxItemFree(Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value)); // ส่งค่า Qty
                        //ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                        //// Show Total Price
                        //cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        //olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        //// Show Totol Qty
                        //cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        //olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        //// total Item
                        //cSale.nC_CntItem = ogdPdtStd.RowCount;

                        //// Show Last Event
                        //olaCashPayment.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value.ToString();
                        //olaTitleCashPay.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        //// Select And Show Rows Last Update
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Selected = true; // Select ข้อมูลล่าสุด 
                        //ogdPdtStd.FirstDisplayedScrollingRowIndex = cSale.nC_DTSeqNo - 1; // Scrol 


                        //*Em 63-05-31
                        // PdtName = 'Free-'+ PdtName
                        ogdDetail.SetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed,ogdDetail.Cols["otbColPdtNameStd"].Index,"Free-" + ogdDetail.GetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtNameStd"].Index));

                        // SumPrice = Qty * SetPrice  
                        ogdDetail.SetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index, oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow));

                        // StaPrd = 3 : สินค้าฟรี
                        ogdDetail.SetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColStaPdtStd"].Index, "3");

                        // SetPrice = 0
                        ogdDetail.SetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColSetPriceStd"].Index, oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow));

                        cSale.C_PRCxItemFree(Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtQtyStd"].Index))); // ส่งค่า Qty
                        ogdDetail.Select(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, 0);

                        // Show Total Price
                        cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        // Show Totol Qty
                        cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtQtyStd"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtQtyStd"].Index));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        // total Item
                        cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;

                        // Show Last Event
                        //olaCashPayment.Text = ogdDetail.GetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index).ToString();
                        olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index).ToString()), cVB.nVB_DecShow); //*Net 63-06-26
                        olaTitleCashPay.Text = ogdDetail.GetData(cSale.nC_DTSeqNo - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtNameStd"].Index).ToString();

                        // Select And Show Rows Last Update
                        //+++++++++++++++
                        break;

                    case 2:

                        // PdtName = 'Free-'+ PdtName
                        ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtName"].Value = "Free-" + ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtName"].Value.ToString();

                        // SumPrice = Qty * SetPrice  
                        ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtQty"].Value) * 0, cVB.nVB_DecShow);

                        // StaPrd = 3 : สินค้าฟรี
                        ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["otbStaPdt"].Value = "3";

                        // SetPrice = 0
                        ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaSetPrice"].Value = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);  //*Em 62-10-08

                        // Process Update
                        cSale.C_PRCxItemFree(Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtQty"].Value)); //ส่งค่า Qty

                        ogdOrder.CurrentCell = ogdOrder.Rows[ogdOrder.RowCount - 1].Cells[0];

                        // Show Total Price
                        cSum = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells["olaPdtSumPrice"].Value));
                        olaTotal.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        // Show Totol Qty
                        cQty = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells["olaPdtQty"].Value));
                        olaQty.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        // total Item
                        cSale.nC_CntItem = ogdOrder.RowCount;

                        // Show Last Event
                        //olaCashPayment.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString();
                        olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString()), cVB.nVB_DecShow);
                        olaTitleCashPay.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtName"].Value.ToString();

                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_FRExItemFree " + oEx.Message); }
            finally
            {
                //poOrder = null;
                //oOrder = null;
                ogdOrder.ClearSelection();

                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Show Popup for function product
        /// </summary>
        /// <param name="ptKbdCode"></param>
        private void W_GETxFunctionPdt(string ptKbdCode)
        {
            wPrice oPrice = null;

            try
            {
                //cVB.tVB_KbdCode = ptKbdCode;

                switch (ptKbdCode)
                {
                    case "KB044":   // ใส่จำนวนทวีคูณ
                        oPrice = new wPrice();

                        // Get Value
                        oPrice.tW_Old = ogdOrder.Rows[ogdOrder.CurrentRow.Index].Cells[0].Value.ToString();
                        oPrice.ShowDialog();

                        // Set Value
                        ogdOrder.Rows[ogdOrder.CurrentRow.Index].Cells[0].Value = oPrice.tW_New;
                        break;
                }

                W_CALxValuePdt();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxFunctionPdt " + oEx.Message); }
            finally
            {
                if (oPrice != null)
                    oPrice.Dispose();

                oPrice = null;
                ptKbdCode = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Calculator Qty / Price product
        /// </summary>
        private void W_CALxValuePdt()
        {
            decimal cQty;
            decimal cPrice;

            try
            {
                cQty = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDecimal(t.Cells[0].Value));
                cPrice = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(t => Convert.ToDecimal(t.Cells[0].Value) * Convert.ToDecimal(t.Cells[5].Value));

                olaQty.Text = oW_SP.SP_SETtDecShwSve(1, cQty, 0);
                olaTotal.Text = oW_SP.SP_SETtDecShwSve(1, cPrice, 0);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_CALxValuePdt " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_PRCxSortPdt(int pnSort)
        {
            try
            {
                switch (pnSort)
                {
                    case 1: //Ascending
                        nW_SortPdt = 1;
                        nW_StartRow = 0;    //*Arm 62-10-16
                        nW_TotalCount = 0;  //*Arm 62-10-16
                        nW_MaxPage = 0;     //*Arm 62-10-16
                        nW_CurPage = 1;     //*Arm 62-10-16
                        W_GETxPdt();
                        break;
                    case 2: //Decending
                        nW_SortPdt = 2;
                        nW_StartRow = 0;    //*Arm 62-10-16
                        nW_TotalCount = 0;  //*Arm 62-10-16
                        nW_MaxPage = 0;     //*Arm 62-10-16
                        nW_CurPage = 1;     //*Arm 62-10-16
                        W_GETxPdt();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_PRCxSortPdt " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }


        /// <summary>
        /// 
        /// ปรับตาม Baseline
        /// </summary>
        public void W_SETxNewDoc()
        {
            try
            {
                new cLog().C_WRTxLog("wSale", "W_SETxNewDoc : Start", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                //W_SETxText();
                ////*Arm 63-05-11
                //oW_dtTmp = null; //*Arm 63-05-12
                //if (ogdPdtStd.DataSource != null)
                //{
                //    ogdPdtStd.DataSource = null;
                //    new cLog().C_WRTxLog("wSale", "W_SETxNewDoc : W_ADDxColumn"); //*Arm 63-05-21
                //    W_ADDxColumn(ogdPdtStd);
                //    new cLog().C_WRTxLog("wSale", "W_SETxNewDoc : W_SETxColumn"); //*Arm 63-05-21
                //    W_SETxColumn(ogdPdtStd);
                //}
                ////+++++++++++++

                ogdOrder.Rows.Clear();
                //ogdPdtStd.Rows.Clear();
                ogdProduct.Rows.Clear();

                ogdDetail.Clear(ClearFlags.UserData);  //*Em 63-07-22
                ogdDetail.DataSource = null; //*Em 63-05-31
                ogdDetail.Rows.Count = ogdDetail.Rows.Fixed;    //*Em 63-05-31
                W_SETxColGrid(ogdDetail); //*Em 63-07-22

                olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaTitleCashPay.Text = "";

                olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaTotal.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaQty.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);

                olaPriLev.Text = "";
                olaPoint.Text = "";
                olaExpired.Text = "";
                olaSO.Text = "";
                nW_Mode = 3;
                if (cSale.nC_DocType == 9)
                {
                    cSale.nC_DocType = 1;
                    //cSale.C_GETtFormatDoc("TPSTSalHD", cSale.nC_DocType, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);
                }

                new cLog().C_WRTxLog("wSale", "W_SETxNewDoc : C_DATxGenNewDoc", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                cSale.C_DATxGenNewDoc();
                olaDocNo.Text = cVB.tVB_DocNo;
                //olaDocNo.ForeColor = Color.Black; //*Arm 63-04-28
                cSale.nC_CntItem = 0;

                //*Arm 63-03-05
                cVB.oVB_ReferSO = null;
                cVB.oVB_Payment = null;
                cVB.tVB_CstStaAlwPosCalSo = "";
                cVB.tVB_RefDocNo = "";
                W_GETxButtonMenuBillStd(0, cVB.nVB_MenuPerPage);


                //*Net 63-08-17 เมื่อโหลดเมนูหน้าแรกให้ แสดงหน้าให้ถูกต้อง
                // เช็คเงื่อนไขแสดงปุ่ม Next และปุ่ม Previous
                ocmPrevPage.Enabled = false;
                ocmNextPage.Enabled = true;

                olaShowPage.Text = oW_Resource.GetString("tPage") + " " + nW_MenuCurPage + "/" + nW_MenuMaxPage;

                //+++++++++++++
                W_SETxText();
                //*Em 63-04-22
                if (cSale.bC_SetComplete)
                {
                    olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tChange");
                    olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_Change, cVB.nVB_DecShow);

                    //*Net 63-07-31 แสดงเงินทอนเมื่อจบบิล
                    if (cVB.oVB_CstScreen != null)
                    {
                        cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                    }
                    //++++++++++++++++++++++++++++++++++++
                    if (cVB.nVB_DelayTimeChg > 0) otmDelayChg.Enabled = true;   //*Em 63-07-26
                }
                //++++++++++++
                //*Net 63-05-26 Comment
                //Thread oSQL = new Thread(new cDatabase().C_CLExMemorySQL);
                //oSQL.IsBackground = true;
                //oSQL.Priority = ThreadPriority.Highest;
                //oSQL.Start();

                cVB.oVB_Sale = null;
                cVB.oVB_Sale = this;

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
                //+++++++++++++

                cVB.bVB_Flag = false; //*Arm 63-06-26
                cVB.tVB_KubotaID = ""; //*Arm 63-06-26

                new cLog().C_WRTxLog("wSale", "W_SETxNewDoc : End", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                new cLog().C_WRTxLog("wSale", $"W_SETxNewDoc : New Doc {cVB.tVB_DocNo}", cVB.bVB_AlwPrnLog); //*Net Stamp
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wHome", "W_SETxNewDoc : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// GET Notification
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void W_Notification(object s, EventArgs e)
        {
            try
            {
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wHome", "W_Notification : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// GET Queue Member
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void W_QueueMember(object s, EventArgs e)   //*Arm 62-10-25
        {
            try
            {
                cSP.SP_GETxCountQMember(olaMsgCountQMem, opnQMember);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wHome", "W_QueueMember : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        /// <summary>
        /// Insert รายการอ้างอิงบิลคืนลง Gridview (Arm 63-06-10)
        /// </summary>
        public void W_PRCxReferBill()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            //oW_dtTmp = new DataTable();
            DataTable odtTmp = new DataTable();
            decimal cSum = 0;
            decimal cQty = 0;

            try
            {
                if (cVB.nVB_SaleModeStd == 1)
                {
                    cSale.C_PRCbInsReferBill();
                    oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                    oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                    oSql.AppendLine("FCXsdDis AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                    oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd,");
                    oSql.AppendLine("FCXsdChg AS otbColCharge");
                    oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("ORDER BY FNXsdSeqNo");
                    odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                    if (odtTmp != null)
                    {
                        if (odtTmp.Rows.Count > 0)
                        {
                            ogdDetail.Rows.Count = ogdDetail.Rows.Fixed;
                            ogdDetail.DataSource = odtTmp;
                            W_SETxColGrid(ogdDetail);
                            if (ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index) > 0)
                            {
                                for (int nRow = ogdDetail.Rows.Fixed; nRow < ogdDetail.Rows.Count; nRow++)
                                {
                                    decimal cDisChg = (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColDiscount"].Index) - (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColCharge"].Index);
                                    ogdDetail.SetData(nRow, ogdDetail.Cols["otbColDiscount"].Index, cDisChg);
                                }
                            }

                            cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index));
                            olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                            cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 3, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 3));
                            olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                            olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                            cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxReferBill : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
        }

        //public void W_PRCxRefund()
        public bool W_PRCxRefund()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            decimal cSum = 0;
            decimal cQty = 0;
            try
            {
                nW_Mode = 4;

                cSale.nC_DocType = 9;
                cSale.C_GETtFormatDoc("TPSTSalHD", cSale.nC_DocType, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);
                cSale.C_DATxGenNewDoc();
                olaDocNo.Text = cVB.tVB_DocNo;
                olaDocNo.ForeColor = Color.Red; //*Arm 63-04-28
                olaSale.Text = oW_Resource.GetString("tRefund");//*Arm 63-04-28
                olaSale.ForeColor = Color.Red;//*Arm 63-04-28
                cSale.nC_CntItem = 0;
                W_GETxButtonMenuBillStd(0, cVB.nVB_MenuPerPage);

                if (cVB.nVB_SaleModeStd == 1)
                {
                    //*Em 63-05-08
                    if(!string.IsNullOrEmpty( cVB.tVB_RefDocNo)) cSale.C_PRCbInsRefund();

                    //oSql.AppendLine("SELECT FNXsdSeqNo,FTXsdBarCode,FTXsdPdtName,FCXsdQty,FTPunName,FCXsdSetPrice,FCXsdDis,FCXsdNet,FTPdtCode,FCXsdFactor,FTXsdStaPdt,FTXsdStaAlwDis");
                    //*Em 63-05-12
                    oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                    oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                    //oSql.AppendLine("(FCXsdDis+FCXsdChg) AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                    oSql.AppendLine("FCXsdDis AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,"); //'*Em 63-06-06
                    oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd,");
                    oSql.AppendLine("FCXsdChg AS otbColCharge");    //'*Em 63-06-06
                    //+++++++++++++
                    oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("ORDER BY FNXsdSeqNo");
                    odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                    if (odtTmp != null)
                    {
                        if (odtTmp.Rows.Count > 0)
                        {

                            //*Em 63-05-31
                            ogdDetail.Rows.Count = ogdDetail.Rows.Fixed;
                            ogdDetail.DataSource = odtTmp;
                            W_SETxColGrid(ogdDetail);
                            if (ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index) > 0)
                            {
                                for (int nRow = ogdDetail.Rows.Fixed; nRow < ogdDetail.Rows.Count; nRow++)
                                {
                                    decimal cDisChg = (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColDiscount"].Index) - (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColCharge"].Index);
                                    ogdDetail.SetData(nRow, ogdDetail.Cols["otbColDiscount"].Index, cDisChg);
                                }
                            }

                            cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index));
                            olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                            cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 3, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 3));
                            olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                            olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                            cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                            //++++++++++++++

                            //*Net 63-07-31 โหลดรายการสินค้าคืนไปที่หน้าจอ 2
                            if (cVB.oVB_CstScreen != null)
                            {
                                cVB.oVB_CstScreen.W_SETxPDTGridSource(odtTmp);
                                cVB.oVB_CstScreen.W_SETxSummaryAmt(olaTotalStd.Text);
                            }
                            //++++++++++++++++++++++++++++++++++++++++++++
                        }
                    }

                    //*Arm 63-09-11 -Comment Code
                    //oSql.Clear();
                    //if (cVB.bVB_RefundTrans) //*Arm 63-05-29
                    //{
                    //    oSql.AppendLine("SELECT FTCstCode FROM TPSTSalHD WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //}
                    //else
                    //{
                    //    oSql.AppendLine("SELECT FTCstCode FROM " + cSale.tC_TblSalHD + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //}
                    //cVB.tVB_CstCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                    //+++++++++++++

                    //*Arm 63-09-11
                    oSql.Clear();
                    oSql.AppendLine("SELECT ISNULL(FTCstCode,'') AS FTCstCode FROM " + cSale.tC_Ref_TblSalHD + " WITH(NOLOCK) WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    cVB.tVB_CstCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                    //+++++++++++++

                    if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                    {
                        if (!string.IsNullOrEmpty(cVB.tVB_ApiCstSch) && cVB.tVB_ApiCstSch_Fmt == "SKC") //ตรวจสอบเช็ค Service SKC หรือ ADA
                        {
                            #region SKC(KADS)
                            new cLog().C_WRTxLog("wSale", "W_PRCxRefund : get CstTel & CardID.", cVB.bVB_AlwPrnLog);
                            
                            string tCtrName = "";
                            bool bSta = false;

                            //*Arm 63-09-11 Comment Code
                            //// หา ข้อมูลเบอร์โทร หรือเลขบัตรประชาชน
                            //oSql.Clear();
                            //oSql.AppendLine("SELECT FTXshCstTel,FTXshCardID,FTXshCtrName");
                            //oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalHDCst + " WITH(NOLOCK)");
                            ////oSql.AppendLine("WHERE FTXshDocNo  = '" + cVB.tVB_RefDocNo + "' AND FTBchCode = '"+cVB.tVB_BchCode+"' ");
                            //oSql.AppendLine("WHERE FTXshDocNo  = '" + cVB.tVB_RefDocNo + "'");  //*Arm 63-09-11

                            //DataTable odtCst = new DataTable();
                            //odtCst = oDB.C_GEToDataQuery(oSql.ToString());

                            //if (odtCst != null && odtCst.Rows.Count > 0)
                            //{
                            //    cVB.tVB_CstTel = odtCst.Rows[0].Field<string>("FTXshCstTel");       //เบอร์โทร
                            //    cVB.tVB_CstCardID = odtCst.Rows[0].Field<string>("FTXshCardID");    //รหัสบัตรประชาชน
                            //    tCtrName = odtCst.Rows[0].Field<string>("FTXshCtrName");            //เลขรถ
                            //}
                            //new cLog().C_WRTxLog("wSale", "W_PRCxRefund : Result CstTel : " + cVB.tVB_CstTel + "/ CardID : " + cVB.tVB_CstCardID, cVB.bVB_AlwPrnLog);
                            //new cLog().C_WRTxLog("wSale", "W_PRCxRefund : search customer SKC start.", cVB.bVB_AlwPrnLog);

                            ////Get Customer
                            //bSta = cServiceKADS.C_GETxHDCst(cVB.tVB_CstTel, cVB.tVB_CstCardID); //*Arm 63-08-15 //*Arm 63-09-11 Comment Code
                            //*Arm 63-09-11 End Comment Code

                            new cLog().C_WRTxLog("wSale", "W_PRCxRefund : CstCode : " + cVB.tVB_CstCode , cVB.bVB_AlwPrnLog);
                            new cLog().C_WRTxLog("wSale", "W_PRCxRefund : search customer SKC start.", cVB.bVB_AlwPrnLog);

                            bSta = cServiceKADS.C_GETxHDCst(cVB.tVB_CstCode); //*Arm 63-09-11

                            if (bSta == true) 
                            {
                                //*Arm 63-09-11
                                oSql.Clear();
                                oSql.AppendLine("SELECT FTXshCtrName ");
                                oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalHDCst + " WITH(NOLOCK)");
                                oSql.AppendLine("WHERE FTXshDocNo  = '" + cVB.tVB_RefDocNo + "'");
                                tCtrName = oDB.C_GEToDataQuery<string>(oSql.ToString()) == null ? "" : oDB.C_GEToDataQuery<string>(oSql.ToString());
                                //+++++++++++++

                                // Get Customer สำเร็จ
                                if (!string.IsNullOrEmpty(tCtrName))
                                {
                                    //ถ้ามี ข้อมูลรถ --> Update
                                    new cLog().C_WRTxLog("wSale", "W_PRCxRefund : Update " + cSale.tC_TblSalHDCst + ".FTXshCtrName = Vehical(" + tCtrName + ")", cVB.bVB_AlwPrnLog);
                                    oSql.Clear();
                                    oSql.AppendLine("UPDATE " + cSale.tC_TblSalHDCst + " WITH(ROWLOCK) SET ");
                                    oSql.AppendLine("FTXshCtrName = '" + tCtrName + "'");
                                    oSql.AppendLine("WHERE FTBchCode ='" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                                    oDB.C_SETxDataQuery(oSql.ToString());
                                }
                            }
                            else
                            {
                                // Get Customer ไม่สำเร็จ
                                W_SETxNewDoc();
                                return false; //*Arm 63-08-17
                            }
                            #endregion END SKC(KADS)
                        }
                        else
                        {
                            #region ADA(STD)
                            //*Arm 63-04-04 เอาข้อมูลแต้มลูกค้าล่าสุดมาจาก หลังบ้าน
                            cmlReqCstSch oReq = new cmlReqCstSch();
                            oReq.ptCstName = "";
                            oReq.ptCstCode = cVB.tVB_CstCode;
                            oReq.ptCstTel = "";
                            oReq.ptCstCardID = "";
                            oReq.ptCstCrdNo = "";
                            oReq.ptCstTaxNo = "";

                            string tJSonCall = JsonConvert.SerializeObject(oReq);
                            string tUrl = cVB.tVB_API2PSMaster;
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tUrl + "/Customer/CstSearch", tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click/Call API Error : " + oEx.Message);
                            }
                            if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                                cmlResCst oCstSch = new cmlResCst();
                                oCstSch = JsonConvert.DeserializeObject<cmlResCst>(tJSonRes);

                                switch (oCstSch.rtCode)
                                {
                                    case "1":
                                        if (oCstSch.raItems.Count > 0)
                                        {
                                            foreach (cmlResCstSch oData in oCstSch.raItems)
                                            {
                                                cVB.nVB_CstPoint = Convert.ToInt32(oData.rtTxnPntQty);
                                                cVB.tVB_CstName = oData.rtCstName;
                                                cVB.tVB_CstTel = oData.rtCstTel;
                                                cVB.tVB_CstStaAlwPosCalSo = oData.rtCstStaAlwPosCalSo;
                                                cVB.tVB_PriceGroup = oData.rtPplCodeRet;
                                                cVB.tVB_CstCardID = oData.rtCstCardID;
                                                cVB.tVB_MemberCard = oData.rtCstCrdNo;
                                                cVB.tVB_ExpiredDate = oData.rdCstCrdExpire == null ? string.Empty : string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(oData.rdCstCrdExpire)); //*Arm 63-04-29
                                            }
                                        }
                                        break;

                                    case "800":
                                        break;
                                    default:
                                        //ERROR
                                        new cLog().C_WRTxLog("wSale", "wSale_Shown/API Response Error : " + oCstSch.rtDesc);
                                        break;
                                }

                                cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                                oCstSch = null;

                            }
                            oCall.C_PRCxCloseConn();    //*Em 63-07-18

                            oCall = null;
                            tJSonCall = null;
                            oReq = null;
                            oRep = null;
                            //+++++++++++++++

                            // Show ข้อมูลลูกค้า
                            W_SETxTextCst();

                            // Insert TPSTSalHDCst
                            cSale.C_DATxInsHDCst(cVB.tVB_CstCode);

                            // แสดงข้อมูลลูกค้า
                            opnCst.Visible = true;
                            opnNoCst.Visible = false;

                            #endregion END ADA(STD)
                        }
                    }

                    cSale.C_PRCxSummary2HD();
                    cVB.oVB_Sale = null;
                    cVB.oVB_Sale = this;
                    //+++++++++++++

                    ////*Arm 63-03-06
                    //if (nW_Mode == 4)
                    //{
                    //    if (cVB.aoVB_PdtRefund.Count > 0)
                    //    {
                    //        //*Arm 63-03-18 Insert SaleHDCst บิลคืน
                    //        StringBuilder oSql = new StringBuilder();
                    //        oSql.AppendLine("SELECT FTCstCode FROM TPSTSalHD WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        cVB.tVB_CstCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                    //        if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                    //        {
                    //            //*Arm 63-04-04 เอาข้อมูลแต้มลูกค้าล่าสุดมาจาก หลังบ้าน
                    //            cmlReqCstSch oReq = new cmlReqCstSch();
                    //            oReq.ptCstName = "";
                    //            oReq.ptCstCode = cVB.tVB_CstCode;
                    //            oReq.ptCstTel = "";
                    //            oReq.ptCstCardID = "";
                    //            oReq.ptCstCrdNo = "";
                    //            oReq.ptCstTaxNo = "";

                    //            string tJSonCall = JsonConvert.SerializeObject(oReq);
                    //            string tUrl = cVB.tVB_API2PSMaster;
                    //            cClientService oCall = new cClientService();
                    //            oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                    //            HttpResponseMessage oRep = new HttpResponseMessage();
                    //            try
                    //            {
                    //                oRep = oCall.C_POSToInvoke(tUrl + "/Customer/CstSearch", tJSonCall);
                    //            }
                    //            catch (Exception oEx)
                    //            {
                    //                new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click/Call API Error : " + oEx.Message);
                    //            }
                    //            if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    //            {
                    //                string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    //                cmlResCst oCstSch = new cmlResCst();
                    //                oCstSch = JsonConvert.DeserializeObject<cmlResCst>(tJSonRes);

                    //                switch (oCstSch.rtCode)
                    //                {
                    //                    case "1":
                    //                        if (oCstSch.raItems.Count > 0)
                    //                        {
                    //                            foreach (cmlResCstSch oData in oCstSch.raItems)
                    //                            {
                    //                                cVB.nVB_CstPoint = Convert.ToInt32(oData.rtTxnPntQty);
                    //                                cVB.tVB_CstName = oData.rtCstName;
                    //                                cVB.tVB_CstTel = oData.rtCstTel;
                    //                                cVB.tVB_CstStaAlwPosCalSo = oData.rtCstStaAlwPosCalSo;
                    //                                cVB.tVB_PriceGroup = oData.rtPplCodeRet;
                    //                                cVB.tVB_CstCardID = oData.rtCstCardID;
                    //                                cVB.tVB_MemberCard = oData.rtCstCrdNo;
                    //                                cVB.tVB_ExpiredDate = oData.rdCstCrdExpire == null ? string.Empty : string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(oData.rdCstCrdExpire)); //*Arm 63-04-29
                    //                            }
                    //                        }
                    //                        break;

                    //                    case "800":
                    //                        break;
                    //                    default:
                    //                        //ERROR
                    //                        new cLog().C_WRTxLog("wSale", "wSale_Shown/API Response Error : " + oCstSch.rtDesc);
                    //                        break;
                    //                }

                    //                cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                    //                oCstSch = null;

                    //            }

                    //            oCall = null;
                    //            tJSonCall = null;
                    //            oReq = null;
                    //            oRep = null;
                    //            //+++++++++++++++

                    //            // Show ข้อมูลลูกค้า
                    //            W_SETxTextCst();

                    //            // Insert TPSTSalHDCst
                    //            cSale.C_DATxInsHDCst(cVB.tVB_CstCode);

                    //            // แสดงข้อมูลลูกค้า
                    //            opnCst.Visible = true;
                    //            opnNoCst.Visible = false;


                    //        }
                    //        else
                    //        {
                    //            cVB.tVB_PriceGroup = cVB.tVB_BchPriceGroup; //* Net 63-03-24
                    //        }
                    //        //++++++++++++++++++++++++

                    //        ogdPdtStd.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //*Em 63-04-29
                    //        ogdOrder.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //*Em 63-04-29
                    //        ogdPdtStd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //*Em 63-04-29
                    //        ogdOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //*Em 63-04-29
                    //        foreach (cmlPdtOrder oPdt in cVB.aoVB_PdtRefund)
                    //        {

                    //            W_ADDxPdtToOrder(oPdt);

                    //        }

                    //        //*Arm 63-03-20
                    //        foreach (cmlPdtDisChg oDisChg in cVB.aoVB_PdtDisChgRefund)
                    //        {
                    //            int nRows = 1;
                    //            if (oDisChg.pnStaDis == 1)
                    //            {

                    //                if (ogdPdtStd.Rows.Count > 0)
                    //                {
                    //                    foreach (DataGridViewRow oRow in ogdPdtStd.Rows)
                    //                    {

                    //                        if (oDisChg.ptPdtCode == oRow.Cells[otbColPdtCodeStd.Name].Value.ToString() &&
                    //                            oDisChg.ptBarcode == oRow.Cells[otbColBarcodeStd.Name].Value.ToString() &&
                    //                            (decimal)oDisChg.pcSetPrice == Convert.ToDecimal(oRow.Cells[otbColSetPriceStd.Name].Value)) //*Arm 63-03-25 
                    //                        {
                    //                            cSale.nC_DTSeqNo = nRows;
                    //                            cVB.oVB_OrderRowIndex = cSale.nC_DTSeqNo - 1;

                    //                            cmlTPSTSalDTDis oSalDTDis = new cmlTPSTSalDTDis();
                    //                            // มูลค่าสุทธิก่อนลดชาร์จ

                    //                            string tB4DisChg = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oRow.Cells[otbColPdtSumPriceStd.Name].Value), cVB.nVB_DecShow);

                    //                            oSalDTDis.FTBchCode = cVB.tVB_BchCode;
                    //                            oSalDTDis.FTXshDocNo = cVB.tVB_DocNo;
                    //                            oSalDTDis.FNXsdSeqNo = nRows;
                    //                            oSalDTDis.FNXddStaDis = oDisChg.pnStaDis;

                    //                            //*Arm 63-04-10
                    //                            if (cVB.nVB_ReturnType == 1)
                    //                            {
                    //                                oSalDTDis.FTXddDisChgTxt = oDisChg.ptDisChgTxt; //คืน Option1
                    //                            }
                    //                            else
                    //                            {
                    //                                oSalDTDis.FTXddDisChgTxt = oW_SP.SP_SETtDecShwSve(1, (decimal)oDisChg.pcValue, cVB.nVB_DecShow).ToString();
                    //                            }
                    //                            //+++++++++++++
                    //                            oSalDTDis.FTXddDisChgType = oDisChg.ptDisChgType;
                    //                            oSalDTDis.FCXddNet = Convert.ToDecimal(tB4DisChg);
                    //                            oSalDTDis.FCXddValue = oDisChg.pcValue;
                    //                            new cSale().C_PRCxDisChgItem(oSalDTDis);
                    //                        }
                    //                        nRows++;
                    //                    }
                    //                }
                    //            }
                    //        }

                    //        ogdPdtStd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //*Em 63-04-29
                    //        ogdOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //*Em 63-04-29

                    //        if (cVB.atVB_PmtRefund != null && cVB.atVB_PmtRefund.Length > 0)
                    //        {
                    //            cSale.C_DATxInsPDRefund();
                    //        }

                    //        cSale.C_PRCxSummary2HD();
                    //        cVB.oVB_Sale = null;
                    //        cVB.oVB_Sale = this;
                    //    }
                    //}
                }
                return true; //*Arm 63-08-17
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxRefund : " + oEx.Message);
                return false; //*Arm 63-08-17
            }
            finally
            {
                oDB = null;
                oSql = null;
                odtTmp = null;
                //new cSP().SP_CLExMemory();
            }
        }

        public void W_PRCxPriceConfirm()    //*Em 63-05-07
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            decimal cSum = 0;
            decimal cQty = 0;
            try
            {
                W_GETxButtonMenuBillStd(0, cVB.nVB_MenuPerPage);

                //*Em 63-05-28
                oW_dtTmp = null;
                //ogdPdtStd.DataSource = null;
                ogdDetail.DataSource = null;    //*Em 63-05-31

                oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                //oSql.AppendLine("(FCXsdDis+FCXsdChg) AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                oSql.AppendLine("FCXsdDis AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,"); //'*Em 63-06-06
                oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd,");
                oSql.AppendLine("FCXsdChg AS otbColCharge");    //'*Em 63-06-06
                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("ORDER BY FNXsdSeqNo");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        //*Em 63-05-31
                        ogdDetail.Rows.Count = ogdDetail.Rows.Fixed;
                        ogdDetail.DataSource = odtTmp;
                        W_SETxColGrid(ogdDetail);
                        if (ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index) > 0)
                        {
                            for (int nRow = ogdDetail.Rows.Fixed; nRow < ogdDetail.Rows.Count; nRow++)
                            {
                                decimal cDisChg = (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColDiscount"].Index) - (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColCharge"].Index);
                                ogdDetail.SetData(nRow, ogdDetail.Cols["otbColDiscount"].Index, cDisChg);
                            }
                        }

                        cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 3, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 3));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                        ogdDetail.Select(-1, -1);
                        //++++++++++++++++

                        //Net 63-07-31 ปรับราคาใหม่ โหลดรายการไปที่หน้าจอ 2 ใหม่
                        if (cVB.oVB_CstScreen != null)
                        {
                            cVB.oVB_CstScreen.W_SETxClearDoc();
                            cVB.oVB_CstScreen.W_SETxPDTGridSource(odtTmp);
                            cVB.oVB_CstScreen.W_SETxSummaryAmt(olaTotalStd.Text);
                        }
                        //++++++++++++++++++++++++++++++++++++++++++++++++
                    }
                }
                //++++++++++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxPriceConfirm : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                odtTmp = null;
                //new cSP().SP_CLExMemory();
            }
        }

        public void W_PRCxLoadItemRetriveBill()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            decimal cSum = 0;
            decimal cQty = 0;
            DataTable odtTmp = new DataTable();
            try
            {
                //*Em 63-05-12
                //ogdPdtStd.DataSource = null;
                //ogdDetail.DataSource = null;  //*Em 63-05-31
                ogdDetail.Clear();  //*Em 63-08-11

                oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                //oSql.AppendLine("CONVERT(DECIMAL(18,4),FCXsdDis+FCXsdChg) AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                oSql.AppendLine("FCXsdDis AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd,");
                oSql.AppendLine("FCXsdChg AS otbColCharge");    //63-06-06
                //+++++++++++++
                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("ORDER BY FNXsdSeqNo");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {

                        //*Em 63-05-31
                        ogdDetail.Rows.Count = ogdDetail.Rows.Fixed;
                        ogdDetail.DataSource = odtTmp;
                        W_SETxColGrid(ogdDetail);
                        if (ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index) > 0)
                        {
                            for (int nRow = ogdDetail.Rows.Fixed; nRow < ogdDetail.Rows.Count; nRow++)
                            {
                                decimal cDisChg = (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColDiscount"].Index) - (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColCharge"].Index);
                                ogdDetail.SetData(nRow, ogdDetail.Cols["otbColDiscount"].Index, cDisChg);
                            }
                        }

                        cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 3, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 3));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;
                        ogdDetail.Select(-1, -1);
                        //++++++++++++++++

                        //*Net 63-07-31 โหลดรายการสินค้าค้นพักบิลไปหน้าจอ 2
                        if (cVB.oVB_CstScreen != null)
                        {
                            cVB.oVB_CstScreen.W_SETxPDTGridSource(odtTmp);
                            cVB.oVB_CstScreen.W_SETxSummaryAmt(olaTotalStd.Text);
                        }
                        //+++++++++++++++++++++++++++++++++++++++++++++
                    }
                }

                oW_dtTmp = null; //*Net 63-06-03 Clear Para

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxRefund : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                odtTmp = null;
                //new cSP().SP_CLExMemory();
            }
        }
        #endregion End Function


        /// <summary>
        /// Payment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void ocmPayment_Click(object sender, EventArgs e)
        public void ocmPayment_Click(object sender, EventArgs e)  //*Em 63-04-24
        {
            wProgress oPmtProgress;
            try
            {
                ocmPayment.Enabled = false;
                new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Click Start", cVB.bVB_AlwPrnLog);
                cVB.oVB_Sale = this;
                //cVB.oVB_PmtGetorSug = new wPmtGetorSug();
                //wPmtGetorSug oGetORSug = new wPmtGetorSug();

                if (cSale.nC_CntItem == 0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                    return;
                }
                //if (ogdPdtStd.Rows.Count > 0 && olaTotalStd.Text == "0.00")
                //if(ogdDetail.Rows.Count-ogdDetail.Rows.Fixed > 0 && olaTotalStd.Text == "0.00") //*Em 63-05-31
                if (ogdDetail.Rows.Count - ogdDetail.Rows.Fixed > 0 && Convert.ToDecimal(olaTotalStd.Text) == 0) //*Net 63-06-24
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                    return;
                }
                //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                //if (cVB.oVB_2ndScreen != null)
                //{
                //    new cLog().C_WRTxLog("wSale", "ocmPayment_Click : W_PRCxSaleToPay");
                //    cVB.oVB_2ndScreen.W_PRCxSaleToPay(olaTotal.Text);
                //    cVB.oVB_2ndScreen.Update();
                //}

                //// Zen 63-03-11 เช็คว่ามีหน้า Payment เปิดอยู่ไหม ถ้ามีให้ปิด แล้วเข้า Step 
                //var oFrm = Application.OpenForms.Cast<Form>().Where(x => x.Name == "wPayment").FirstOrDefault();
                //if (null != oFrm)
                //{
                //    oFrm.Close();
                //    oFrm = null;
                //}

                //*Em 63-04-17
                if (nW_Mode == 4)
                {
                    new cLog().C_WRTxLog("wSale", "ocmPayment_Click : C_PRCxSummary2HD", cVB.bVB_AlwPrnLog);
                    cSale.C_PRCxSummary2HD();
                    new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Open wPayment", cVB.bVB_AlwPrnLog);
                    new wPayment(3).ShowDialog();
                    return;
                    //this.Hide();
                }
                //++++++++++++++++

                //*Arm 63-04-10 กรณีใช้ใบสั่งขาย และไม่อนุญาตให้ คำนวณรายการใหม่ไม่ให้คำนวณ Promotion
                if (cVB.oVB_ReferSO != null && cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                {
                    new cLog().C_WRTxLog("wSale", "ocmPayment_Click : C_PRCxSummary2HD", cVB.bVB_AlwPrnLog);
                    cSale.C_PRCxSummary2HD();
                    new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Open wPayment", cVB.bVB_AlwPrnLog);
                    new wPayment(3).ShowDialog();
                    //this.Hide();
                }
                else
                {

                    new cLog().C_WRTxLog("wSale", "ocmPayment_Click : ตรวจสอบ Thread คำนวน Promotion", cVB.bVB_AlwPrnLog); //*Net Stamp
                    try
                    {
                        if (aoW_CalPro != null && aoW_CalPro.Count > 0)
                        {
                            new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Show Progress Thread Pmt Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                            if (aoW_CalPro.Count(oThread => oThread.IsAlive == true) > 0)
                            {
                                oPmtProgress = new wProgress(3, false);
                                oPmtProgress.Show();
                                oPmtProgress.Refresh();
                                while (aoW_CalPro.Count(oThread => oThread.IsAlive == true) > 0)
                                {
                                    oPmtProgress.W_SETxProgress((aoW_CalPro.Count(oThread => oThread.IsAlive == false) * 100) / aoW_CalPro.Count);
                                    oPmtProgress.Refresh();
                                }
                                oPmtProgress.W_SETxProgress(100);
                                oPmtProgress.Close();
                                oPmtProgress = null;
                            }
                            for (int count = 0; count < aoW_CalPro.Count; count++)
                            {
                                aoW_CalPro[count].Abort();
                                aoW_CalPro[count] = null;
                            }
                            if (aoW_CalPro != null)
                            {
                                aoW_CalPro.Clear();
                                aoW_CalPro = null;
                            }

                            ////*Em 63-06-02
                            //if (Convert.ToDecimal(olaTotalQtyStd.Text) > Convert.ToDecimal(1))
                            //{
                            //    if (cVB.bVB_PmtPriGrp == true) //ถ้ามีโปรโมชั่นกลุ่มราคา
                            //    {
                            //        bW_CalPmtPrice = true; //คำนวณโปรโมชั่นกลุ่มราคา
                            //        W_PRCxCoPmt();
                            //    }
                            //    else
                            //    {
                            //        if (cVB.bVB_PmtDis == true)  //Check ถ้ามีโปรโมชั่นส่วนลด
                            //        {
                            //            bW_CalPmtPrice = false; //ไม่คำนวณโปรโมชั่นกลุ่มราคา
                            //            W_PRCxCoPmt();
                            //        }
                            //    }
                            //}
                            ////++++++++++++++
                            new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Show Progress Thread Pmt End", cVB.bVB_AlwPrnLog); //*Net Stamp
                        }
                        //else
                        //{
                        //    //*Em 63-06-02
                        //    if (Convert.ToDecimal(olaTotalQtyStd.Text) > Convert.ToDecimal(1))
                        //    {
                        //        if (cVB.bVB_PmtPriGrp == true) //ถ้ามีโปรโมชั่นกลุ่มราคา
                        //        {
                        //            bW_CalPmtPrice = true; //คำนวณโปรโมชั่นกลุ่มราคา
                        //            W_PRCxCoPmt();
                        //        }
                        //        else
                        //        {
                        //            if (cVB.bVB_PmtDis == true)  //Check ถ้ามีโปรโมชั่นส่วนลด
                        //            {
                        //                bW_CalPmtPrice = false; //ไม่คำนวณโปรโมชั่นกลุ่มราคา
                        //                W_PRCxCoPmt();
                        //            }
                        //        }
                        //    }
                        //    //++++++++++++++
                        //}

                        //*Em 63-07-07
                        if (cVB.bVB_PmtPriGrp == true) //ถ้ามีโปรโมชั่นกลุ่มราคา
                        {
                            bW_CalPmtPrice = true; //คำนวณโปรโมชั่นกลุ่มราคา
                            W_PRCxCoPmt();
                        }
                        else
                        {
                            if (cVB.bVB_PmtDis == true)  //Check ถ้ามีโปรโมชั่นส่วนลด
                            {
                                bW_CalPmtPrice = false; //ไม่คำนวณโปรโมชั่นกลุ่มราคา
                                W_PRCxCoPmt();
                            }
                        }
                        //+++++++++++++
                    }
                    catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmPayment_Click " + oEx.Message); }

                    //new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : Start");
                    //W_PRCxCoPmt();
                    //new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : End");
                    if (cVB.oVB_GetPmt != null && (cVB.oVB_GetPmt.Rows.Count > 0 || cVB.oVB_PmtSug.Rows.Count > 0))
                    {
                        //wPmtGetorSug oGetORSug = new wPmtGetorSug();
                        //oGetORSug.olaCashPayment.Text = olaTotalStd.Text;
                        //oGetORSug.olaCstName.Text = cVB.tVB_CstName;
                        //oGetORSug.ShowDialog();
                        //oGetORSug.Update();

                        //*Em 63-05-05
                        if (bW_CalPmtPrice)
                        {
                            //*Em 63-06-02
                            new cLog().C_WRTxLog("wSale", "ocmPayment_Click : C_PRCbINSPmtPD", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                            new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                            //++++++++++++++

                            new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Open wPmtPriConfirm", cVB.bVB_AlwPrnLog);
                            wPmtPriConfirm oPmtPri = new wPmtPriConfirm();
                            oPmtPri.olaCashPayOld.Text = olaTotalStd.Text;
                            oPmtPri.olaCstName.Text = cVB.tVB_CstName;
                            oPmtPri.ShowDialog();

                            //*Net 63-07-31 เมื่อจบหน้า Confirm Price ให้ปรับหัวหน้าจอ 2 
                            if (cVB.oVB_CstScreen != null)
                            {
                                cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                                cVB.oVB_CstScreen.W_SETxSummaryAmt(olaTotalStd.Text);
                            }
                            //++++++++++++++++++++++++++++++
                        }
                        else
                        {
                            //*Em 63-06-02
                            new cLog().C_WRTxLog("wSale", "ocmPayment_Click : C_PRCbINSPmtPD", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                            new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                            //++++++++++++++

                            new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Open wPmtGetorSug", cVB.bVB_AlwPrnLog);
                            wPmtGetorSug oGetORSug = new wPmtGetorSug();
                            oGetORSug.olaCashPayment.Text = olaTotalStd.Text;
                            oGetORSug.olaCstName.Text = cVB.tVB_CstName;
                            oGetORSug.ShowDialog();
                            oGetORSug.Update();
                            //*Net 63-07-31 เมื่อจบหน้า Confirm Price ให้ปรับหัวหน้าจอ 2 
                            if (cVB.oVB_CstScreen != null)
                            {
                                cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                                cVB.oVB_CstScreen.W_SETxSummaryAmt(olaTotalStd.Text);
                            }
                            //++++++++++++++++++++++++++++++
                        }
                        //++++++++++++++++
                    }
                    else
                    {
                        //*Arm 63-06-19 P1RFP-002 ตรวจสอบ Stock แบบ Realtime จากระบบ KADS 
                        if (cSale.nC_DocType != 9 && !string.IsNullOrEmpty(cVB.tVB_WahStaChkStk) && cVB.tVB_WahStaChkStk == "3") //(TCNMWaHouse.FTWahStaChkStk = 3 :ใช้ตรวจสอบในขั้นตอนการขาย 3: Check Online  )
                        {
                            bool bStaChkStk = false;
                            Cursor.Current = Cursors.WaitCursor;
                            bStaChkStk = new cStock().C_CHKbSubTotalCheckStock();
                            Cursor.Current = Cursors.Default; 
                            if (bStaChkStk == false)
                            {
                                return;
                            }
                        }
                        //++++++++++++
                        new cLog().C_WRTxLog("wSale", "ocmPayment_Click : C_PRCxSummary2HD", cVB.bVB_AlwPrnLog);
                        cSale.C_PRCxSummary2HD();
                        new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Open wPayment", cVB.bVB_AlwPrnLog);
                        new wPayment(3).ShowDialog();
                        nW_Time = 10;
                        //this.Hide();
                    }
                }

                //+++++++++++++


                //// Zen 63-04-03 เพิ่มในส่วนโปรโมชั่นมา เพื่อเอาไว้ทดสอบ
                //W_PRCxCoPmt();

                //if (cVB.oVB_GetPmt.Rows.Count > 0 || cVB.oVB_PmtSug.Rows.Count > 0)
                //{
                //    //*Arm 63-04-08 กรณีใช้ใบสั่งขาย และไม่อนุญาตให้ คำนวณรายการใหม่ไม่ให้คำนวณ Promotion
                //    if (cVB.oVB_ReferSO != null) 
                //    {
                //        if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                //        {
                //            cSale.C_PRCxSummary2HD();
                //            new wPayment(3).Show();
                //            this.Hide();
                //        }
                //        else
                //        {
                //            oGetORSug.olaCashPayment.Text = olaTotalStd.Text;
                //            oGetORSug.olaCstName.Text = cVB.tVB_CstName;
                //            oGetORSug.Show();
                //            oGetORSug.Update();
                //        }

                //    }
                //    else
                //    {
                //        oGetORSug.olaCashPayment.Text = olaTotalStd.Text;
                //        oGetORSug.olaCstName.Text = cVB.tVB_CstName;
                //        oGetORSug.Show();
                //        oGetORSug.Update();
                //    }
                //}
                //else
                //{
                //    cSale.C_PRCxSummary2HD();
                //    new wPayment(3).Show();

                //    nW_Time = 10;
                //    otmClose.Start();
                //}
                //*Net 63-07-31 เมื่อกลับมาหน้า sale ให้ปรับหัวหน้าจอ 2 
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxSummaryAmt(olaTotalStd.Text);
                }
                //++++++++++++++++++++++++++++++

                otbScan.Focus();
                aoW_CalPro = new List<Thread>(); //*Net 63-05-21

                new cLog().C_WRTxLog("wSale", "ocmPayment_Click : Click End", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmPayment_Click " + oEx.Message); }
            finally
            {
                oPmtProgress = null;
                //new cSP().SP_CLExMemory();
                ocmPayment.Enabled = true;
            }
        }
        public void W_PRCxCoPmt()
        {
            cPdtPmt oPdtPmt = new cPdtPmt();    //*Em 63-07-08

            new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : Start", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
            if (bW_CalPmtPrice) //*Em 63-05-05 โปรโมชั่นกลุ่มราคา
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : C_PRCxPrepareDT", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                //new cPdtPmt().C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);
                oPdtPmt.C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);  //*Em 63-07-08

                new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : C_PRCoCalPmt", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                //new cPdtPmt().C_PRCoCalPmt("1");
                oPdtPmt.C_PRCoCalPmt("1");  //*Em 63-07-08
                //new cPdtPmt().C_PRCbCalPmtSuggest();

                if (cVB.oVB_GetPmt.Rows.Count > 0)
                {
                    //new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : C_PRCbINSPmtPD"); //*Arm 63-05-21
                    //new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                    // C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                }
                else
                {
                    new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : ReCalPmt PmtPrice=false", cVB.bVB_AlwPrnLog); //*Arm 63-05-21 //*Net 63-05-26 เปลี่ยน Wording
                    bW_CalPmtPrice = false;

                    //*Em 63-07-08
                    if (cVB.oVB_CstPrivilege != null && cVB.oVB_CstPrivilege.Rows.Count > 0)
                    {
                        oPdtPmt.C_DATxUpdateQuotaPmt();
                    }
                    //++++++++++++++++++

                    //W_PRCxCoPmt();
                    //if (cVB.bVB_PmtDis == true) W_PRCxCoPmt(); //*Arm 63-05-27
                    //if(cVB.bVB_PmtDis == true) new cPdtPmt().C_PRCoCalPmt("2"); //*Em 63-06-04
                    if (cVB.bVB_PmtDis == true) oPdtPmt.C_PRCoCalPmt("2");  //*Em 63-07-08
                }
            }
            else
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : C_PRCxPrepareDT", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                //new cPdtPmt().C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);
                oPdtPmt.C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);  //*Em 63-07-08

                //*Em 63-07-08
                if (cVB.oVB_CstPrivilege != null && cVB.oVB_CstPrivilege.Rows.Count > 0)
                {
                    oPdtPmt.C_DATxUpdateQuotaPmt();
                }
                //++++++++++++++++++

                new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : C_PRCoCalPmt", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                //new cPdtPmt().C_PRCoCalPmt("2");
                oPdtPmt.C_PRCoCalPmt("2");
                //new cPdtPmt().C_PRCbCalPmtSuggest();

                if (cVB.oVB_GetPmt.Rows.Count > 0)
                {
                    new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : C_PRCbINSPmtPD", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                    //new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                    oPdtPmt.C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                    // C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                }
            }
            new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : End", cVB.bVB_AlwPrnLog); //*Arm 63-05-21

            oPdtPmt = null; //*Em 63-07-08
        }
        /// <summary>
        /// Show function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDoShowKB();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmShwKb_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Open Calculate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxCalculator();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmCalculate_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Open popup help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHelp_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxHelp();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmHelp_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Open Popup about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAbout_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAbout();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmAbout_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Scan or Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSeachScan_Click(object sender, EventArgs e)
        {
            try
            {
                //if (string.Equals(ocmSeachScan.Tag.ToString(), "0")) // Scan
                //{
                //    opnSearch.Visible = true;
                //    opnScan.Visible = false;
                //    ocmSeachScan.Image = Properties.Resources.ScanW_32;
                //    ocmSeachScan.Tag = "1";
                //    ocmSeachScan.Text = "".PadLeft(10) + oW_Resource.GetString("tScan");
                //    otbSearch.Focus();
                //}
                //else // Search
                //{
                //    opnScan.Visible = true;
                //    opnSearch.Visible = false;
                //    ocmSeachScan.Image = Properties.Resources.SearchW_32;
                //    ocmSeachScan.Tag = "0";
                //    ocmSeachScan.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tSearch");
                //    otbScan.Focus();
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmSeachScan_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get menu Bill
        /// </summary>
        private void W_GETxMenuBill()
        {
            int nRowCount = 0;
            double cMaxPage;

            try
            {
                //aoFunc = new cFunctionKeyboard().C_GETaFuncList("039");

                //foreach (cmlTPSMFunc oFunc in aoFunc)
                //{
                //    Button ocmMenuBill = new Button();
                //    ocmMenuBill.Name = "ocm-" + oFunc.FTSysCode;
                //    ocmMenuBill.Tag = oFunc.FTGdtCallByName;
                //    ocmMenuBill.Size = new Size(100, 50);
                //    ocmMenuBill.FlatStyle = FlatStyle.Flat;
                //    ocmMenuBill.BackColor = cVB.oVB_ColNormal;
                //    ocmMenuBill.ForeColor = Color.White;
                //    ocmMenuBill.Font = new Font(new FontFamily("Segoe UI Light"), 10f);
                //    ocmMenuBill.Margin = new Padding(4, 0, 4, 10);
                //    ocmMenuBill.Text = oFunc.FTGdtName;
                //    ocmMenuBill.Click += ocmMenuBill_Click;
                //    opnMenuBill.Controls.Add(ocmMenuBill);
                //    ocmMenuBill = null;
                //}

                //*Arm 62-11-15
                //**************************************

                //*Arm 63-03-03 - GET Menu Bill
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1:
                        aW_oFunc = new cFunctionKeyboard().C_GETaFuncList("048");
                        if (aW_oFunc.Count > 0)
                            nRowCount = aW_oFunc.Count;

                        //*Net 63-07-31 ปรับให้คำนวนจากค่าที่น้อยกว่า
                        //cVB.nVB_MenuPerPage = (int)Math.Ceiling(Convert.ToDecimal(olaQtyStd.Height / 62)) * 2;
                        cVB.nVB_MenuPerPage = Math.Min((int)Math.Ceiling(Convert.ToDecimal(olaQtyStd.Height / 64)) * 2, cVB.nVB_MenuPerPage);
                        cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / cVB.nVB_MenuPerPage); // *Arm 62-10-16 - คำนวณจำนวนหน้า
                        break;
                    case 2:
                        aW_oFunc = new cFunctionKeyboard().C_GETaFuncList("039");

                        if (aW_oFunc.Count > 0)
                            nRowCount = aW_oFunc.Count;

                        cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / cVB.nVB_MenuPerPage); // *Arm 62-10-16 - คำนวณจำนวนหน้า
                        break;
                }
                W_GETxButtonMenuBill(0, cVB.nVB_MenuPerPage);
                W_GETxButtonMenuBillStd(0, cVB.nVB_MenuPerPage);

                //**************************************
                // เงื่อนไขการแบ่งหน้า

                if (aW_oFunc.Count > 0)
                    nRowCount = aW_oFunc.Count;

                cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / cVB.nVB_MenuPerPage); // *Arm 62-10-16 - คำนวณจำนวนหน้า

                if (cMaxPage == 0)          // ถ้าคำนวณได้ 0 ให้ มีค่าเท่า 1
                {
                    cMaxPage = 1;
                }

                nW_MenuMaxPage = Convert.ToInt32(cMaxPage);

                if (nW_MenuCurPage == 1)    // เช็คเงื่อนไขกรณีอยู่หน้าแรก
                {
                    if (cMaxPage > 1)       // จำนวนหน้าทั้งหมดมากกว่า 1 : ไม่แสดงปุ่มย้อนกลับ แต่แสดงปุ่มถัดไป
                    {
                        ocmMenuPrev.Enabled = false;
                        ocmMenuNext.Enabled = true;

                        ocmNextPage.Enabled = true;
                        ocmPrevPage.Enabled = false;

                    }
                    else                    // จำนวนหน้าทั้งหมด 1 หน้า : ไม่แสดงปุ่ม ย้อนกลับและถัดไป
                    {
                        ocmMenuPrev.Enabled = false;
                        ocmMenuNext.Enabled = false;

                        ocmNextPage.Enabled = false;
                        ocmPrevPage.Enabled = false;
                    }
                }

                olaShowPage.Text = oW_Resource.GetString("tPage") + " " + nW_MenuCurPage + "/" + nW_MenuMaxPage;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxMenuBill : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        private void W_GETxButtonMenuBillStd(int pnStartRow, int pnEndRow)
        {
            try
            {
                if (pnEndRow > aW_oFunc.Count) pnEndRow = aW_oFunc.Count;
                if (pnStartRow == 0) nW_MenuCurPage = 1; //*Net 63-08-18 เมื่อโหลดเมนูหน้าแรกให้ ตั้งค่าหน้าปัจจุบันเป็น 1
                olaQtyStd.Controls.Clear();
                for (int nRow = pnStartRow; nRow < pnEndRow; nRow++)
                {
                    Button ocmMenuBillStd = new Button();
                    ocmMenuBillStd.Name = "ocm-" + aW_oFunc[nRow].FTSysCode;
                    ocmMenuBillStd.Tag = aW_oFunc[nRow].FTGdtCallByName;
                    //ocmMenuBillStd.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                    ocmMenuBillStd.Size = new Size(130, 60);

                    ocmMenuBillStd.FlatStyle = FlatStyle.Flat;
                    ocmMenuBillStd.BackColor = cVB.oVB_ColNormal;
                    ocmMenuBillStd.ForeColor = Color.White;
                    ocmMenuBillStd.Font = new Font(new FontFamily("Segoe UI Light"), 10f); //*Arm 63-03-06 ปรับ Size เดิม 12f เป็น 10f
                    ocmMenuBillStd.Margin = new Padding(2, 2, 2, 2);
                    ocmMenuBillStd.Text = aW_oFunc[nRow].FTGdtName;
                    ocmMenuBillStd.TextAlign = ContentAlignment.MiddleCenter;
                    ocmMenuBillStd.Click += ocmMenuBill_Click;

                    //*Em 63-05-07
                    switch (aW_oFunc[nRow].FTSysCode)
                    {
                        case "KB005":   // ใส่จำนวนทวีคูณ
                        case "KB007":   // ค้นหาข้อมูลรถ *Arm 63-08-17
                        case "KB012":   // ยกเลิกรายการ

                        case "KB014":   // ชาร์จเป็นจำนวน
                        case "KB015":   // ชาร์จเป็น %
                        case "KB016":   // ลดเป็นจำนวน
                        case "KB017":   // ลดเป็น %
                        case "KB018":   // ใส่รายการสินค้าฟรี
                        case "KB024":   // คืนรายการขาย
                        case "KB036":   // ข้อมูลสมาชิก
                        case "KB040":   // พักบิลชั่วคราว
                        case "KB041":   // ค้นบิลกลับมาขายต่อ
                        case "KB054":   // หมายเหตุของสินค้า
                        case "KB070":   // ใบสั่งขาย
                        case "KB056":   // เปิดบิลขายอ้างบิลคืน (*Arm 63-06-10)
                            if ((!string.IsNullOrEmpty(cVB.tVB_RefDocNo) && cSale.nC_DocType == 9) || ((cVB.oVB_ReferSO != null) && (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")) || (cVB.bVB_PriceConfirm))
                            {
                                ocmMenuBillStd.BackColor = Color.LightGray;
                                ocmMenuBillStd.Enabled = false;

                            }
                            else
                            {
                                ocmMenuBillStd.Enabled = true;
                            }
                            break;
                        case "KB013":   // ค้นหาสินค้า
                            if ((!string.IsNullOrEmpty(cVB.tVB_RefDocNo) && cSale.nC_DocType == 9) || ((cVB.oVB_ReferSO != null) && (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")))
                            {
                                ocmMenuBillStd.BackColor = Color.LightGray;
                                ocmMenuBillStd.Enabled = false;

                            }
                            else
                            {
                                ocmMenuBillStd.Enabled = true;
                            }
                            break;
                        default:
                            ocmMenuBillStd.Enabled = true;
                            break;
                    }
                    //++++++++++++++++++
                    //if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo) && cSale.nC_DocType == 9) // ถ้าเป็นการคืน
                    //{
                    //    switch (aW_oFunc[nRow].FTSysCode)
                    //    {
                    //        case "KB005":   // ใส่จำนวนทวีคูณ
                    //        case "KB012":   // ยกเลิกรายการ
                    //        case "KB013":   // ค้นหาสินค้า
                    //        case "KB014":   // ชาร์จเป็นจำนวน
                    //        case "KB015":   // ชาร์จเป็น %
                    //        case "KB016":   // ลดเป็นจำนวน
                    //        case "KB017":   // ลดเป็น %
                    //        case "KB018":   // ใส่รายการสินค้าฟรี
                    //        case "KB024":   // คืนรายการขาย
                    //        case "KB036":   // ข้อมูลสมาชิก
                    //        case "KB040":   // พักบิลชั่วคราว
                    //        case "KB041":   // ค้นบิลกลับมาขายต่อ
                    //        case "KB054":   // หมายเหตุของสินค้า
                    //        case "KB070":   // ใบสั่งขาย

                    //            ocmMenuBillStd.BackColor = Color.LightGray;
                    //            ocmMenuBillStd.Enabled = false;
                    //            break;

                    //    }
                    //}
                    //else
                    //{
                    //    //*Arm 63-03-06
                    //    if (cVB.oVB_ReferSO != null) // กรณีใช้ใบสั่งขาย และไม่อนุญาตให้ คำนวณรายการใหม่
                    //    {
                    //        if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1") //*Arm 63-03-31
                    //        {
                    //            switch (aW_oFunc[nRow].FTSysCode)
                    //            {
                    //                case "KB005":   // ใส่จำนวนทวีคูณ
                    //                case "KB012":   // ยกเลิกรายการ
                    //                case "KB013":   // ค้นหาสินค้า
                    //                case "KB014":   // ชาร์จเป็นจำนวน
                    //                case "KB015":   // ชาร์จเป็น %
                    //                case "KB016":   // ลดเป็นจำนวน
                    //                case "KB017":   // ลดเป็น %
                    //                case "KB018":   // ใส่รายการสินค้าฟรี
                    //                case "KB024":   // คืนรายการขาย
                    //                case "KB036":   // ข้อมูลสมาชิก
                    //                case "KB040":   // พักบิลชั่วคราว
                    //                case "KB041":   // ค้นบิลกลับมาขายต่อ
                    //                case "KB054":   // หมายเหตุของสินค้า
                    //                case "KB070":   // ใบสั่งขาย

                    //                    ocmMenuBillStd.BackColor = Color.LightGray;
                    //                    ocmMenuBillStd.Enabled = false;
                    //                    break;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            ocmMenuBillStd.Enabled = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        ocmMenuBillStd.Enabled = true;
                    //    }
                    //}

                    olaQtyStd.Controls.Add(ocmMenuBillStd);
                    ocmMenuBillStd = null;


                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxButtonMenuBillStd : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }

        }
        /// <summary>
        /// Get Button Menu Bill
        /// </summary>
        private void W_GETxButtonMenuBill(int pnStartRow, int pnEndRow)
        {
            try
            {

                if (pnEndRow > aW_oFunc.Count) pnEndRow = aW_oFunc.Count;

                opnMenuBill.Controls.Clear();
                for (int nRow = pnStartRow; nRow < pnEndRow; nRow++)
                {
                    Button ocmMenuBill = new Button();
                    ocmMenuBill.Name = "ocm-" + aW_oFunc[nRow].FTSysCode;
                    ocmMenuBill.Tag = aW_oFunc[nRow].FTGdtCallByName;
                    ocmMenuBill.Size = new Size(100, 50);
                    ocmMenuBill.FlatStyle = FlatStyle.Flat;
                    ocmMenuBill.BackColor = cVB.oVB_ColNormal;
                    ocmMenuBill.ForeColor = Color.White;
                    ocmMenuBill.Font = new Font(new FontFamily("Segoe UI Light"), 10f);
                    ocmMenuBill.Margin = new Padding(4, 0, 4, 10);
                    ocmMenuBill.Text = aW_oFunc[nRow].FTGdtName;
                    ocmMenuBill.Click += ocmMenuBill_Click;
                    opnMenuBill.Controls.Add(ocmMenuBill);
                    ocmMenuBill = null;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxButtonMenuBill : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Menu Bill
        /// *Arm 63-03-03 แก้ไข
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenuBill_Click(object sender, EventArgs e)
        {
            Button ocmMenuBill = (Button)sender;
            string tFuncName;
            decimal cScanQty = 0;
            try
            {
                tFuncName = ocmMenuBill.Tag.ToString();

                //if (tFuncName == "C_KBDxVoidItem" || tFuncName == "C_KBDxPdtQty" || tFuncName == "C_KBDxChgAmt" || tFuncName == "C_KBDxChgPer" ||
                //    tFuncName == "C_KBDxDisAmt" || tFuncName == "C_KBDxDisPer" || tFuncName == "C_KBDxItemFree" || tFuncName == "C_KBDxPdtRmk")
                //{

                //    //if (cSale.nC_CntItem > 0)
                //    //{

                //    //}
                //    //else
                //    //{
                //    //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                //    //    return;
                //    //}
                //}

                //W_GETxFuncByFuncName(tFuncName);
                //otbScan.Focus();

                //ogdPdtStd.ClearSelection(); //*Em 63-05-27
                ogdDetail.Select(-1, -1);   //*Em 63-05-31

                //*Arm 63-05-04 - Check เงื่อนไขแก้ไข Qty ตอน Scan Barcode
                if (tFuncName == "C_KBDxPdtQty")
                {
                    try
                    {
                        cScanQty = Convert.ToDecimal(otbScan.Text); //Convert ถ้าไม่ใช่ตัวเลข ตก catch และทำ Process แบบเดิม

                        if (cScanQty > 1 && cScanQty <= 9999)
                        {
                            cW_SetQty = (decimal)cScanQty;
                            olaTitleCashPay.Text = "x " + cScanQty.ToString();
                            //olaCashPayment.Text = "0.00";
                            olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม;
                            otbScan.Text = "";
                            otbScan.Focus();
                            return;
                        }
                        else
                        {
                            W_GETxFuncByFuncName(tFuncName);
                            otbScan.Focus();
                        }
                    }
                    catch (Exception oEx)
                    {
                        W_GETxFuncByFuncName(tFuncName);
                        otbScan.Focus();
                    }
                }
                else
                {
                    W_GETxFuncByFuncName(tFuncName);
                    otbScan.Focus();
                }
                //+++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmMenuBill_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmPdtGrp_Click(object sender, EventArgs e)
        {
            Button ocmPdtGrp = (Button)sender;
            string tFuncName;

            try
            {
                if (ocmPdtGrp.Tag != null)
                {
                    tW_PgpChain = ocmPdtGrp.Tag.ToString();
                }
                else
                {
                    tW_PgpChain = "";
                }
                W_GETxPdt(2, otbSearch.Text);   //*Em 62-11-13
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmPdtGrp_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Update ราคา
        /// </summary>
        /// 
        public void W_PRCxOrder(int nSeq, decimal pcDis, decimal pcChg)
        {
            decimal cPdtQty;
            decimal cSetPrice;
            decimal cSumPrice;
            decimal cSum;
            decimal cQty;
            try
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxOrder : Start", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1:

                        //cPdtQty = Convert.ToDecimal(ogdPdtStd.Rows[nSeq].Cells[otbColPdtQtyStd.Name].Value);
                        //cSetPrice = Convert.ToDecimal(ogdPdtStd.Rows[nSeq].Cells[otbColSetPriceStd.Name].Value);
                        //cSumPrice = cPdtQty * cSetPrice;

                        //cSumPrice = (cSumPrice - pcDis) + pcChg;

                        //if (oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                        //{
                        //    oW_dtTmp.Columns[otbColDiscount.Name].ReadOnly = false;
                        //    //oW_dtTmp.Rows[nSeq].SetField("otbColDiscount", Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(pcDis - pcChg), cVB.nVB_DecShow)));
                        //    //oW_dtTmp.Rows[nSeq].SetField("otbColPdtSumPriceStd", Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow)));
                        //    DataRow oRow = oW_dtTmp.Rows[nSeq];
                        //    oRow["otbColDiscount"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(pcDis - pcChg), cVB.nVB_DecShow));
                        //    oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow));
                        //}
                        //else
                        //{
                        //    // Discount
                        //    ogdPdtStd.Rows[nSeq].Cells[otbColDiscount.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(pcDis - pcChg), cVB.nVB_DecShow);
                        //    // SumPrice
                        //    ogdPdtStd.Rows[nSeq].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow);

                        //}
                        ////// Discount
                        ////ogdPdtStd.Rows[nSeq].Cells[otbColDiscount.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(pcDis - pcChg), cVB.nVB_DecShow);
                        ////// SumPrice
                        ////ogdPdtStd.Rows[nSeq].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow);

                        //new cLog().C_WRTxLog("wSale", "W_PRCxOrder : Sum Price"); //*Arm 63-05-21
                        //// Show Total Price
                        //cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        //olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        //new cLog().C_WRTxLog("wSale", "W_PRCxOrder : Sum Qty"); //*Arm 63-05-21
                        //// Show Totol Qty
                        //cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        //olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        //// total Item
                        //cSale.nC_CntItem = ogdPdtStd.RowCount;

                        //// Show Last Event
                        //olaCashPayment.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value.ToString();
                        //olaTitleCashPay.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        //// Select And Show Rows Last Update
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Selected = true; // Select ข้อมูลล่าสุด 
                        //ogdPdtStd.FirstDisplayedScrollingRowIndex = cSale.nC_DTSeqNo - 1; // Scrol 


                        //*Em 63-05-31
                        nSeq = nSeq + ogdDetail.Rows.Fixed; //*Em 63-06-05
                        cPdtQty = Convert.ToDecimal(ogdDetail.GetData(nSeq,ogdDetail.Cols["otbColPdtQtyStd"].Index));
                        cSetPrice = Convert.ToDecimal(ogdDetail.GetData(nSeq,ogdDetail.Cols["otbColSetPriceStd"].Index));
                        cSumPrice = cPdtQty * cSetPrice;

                        cSumPrice = (cSumPrice - pcDis) + pcChg;

                        // Discount
                        pcDis = pcDis - pcChg;
                        ogdDetail.SetData(nSeq, ogdDetail.Cols["otbColDiscount"].Index, oW_SP.SP_SETtDecShwSve(1, pcDis, cVB.nVB_DecShow));

                        // SumPrice
                        ogdDetail.SetData(nSeq, ogdDetail.Cols["otbColPdtSumPriceStd"].Index , oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow));
                        new cLog().C_WRTxLog("wSale", "W_PRCxOrder : Sum Price", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                        // Show Total Price
                        cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        new cLog().C_WRTxLog("wSale", "W_PRCxOrder : Sum Qty", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                        // Show Totol Qty
                        cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 3, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 3));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        // total Item
                        cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;

                        // Show Last Event
                        //*Net 63-07-31 ปรับตาม Moshi
                        //olaCashPayment.Text = ogdDetail.GetData(nSeq, ogdDetail.Cols["otbColPdtSumPriceStd"].Index).ToString();
                        olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(nSeq, ogdDetail.Cols["otbColPdtSumPriceStd"].Index).ToString()), cVB.nVB_DecShow);
                        olaTitleCashPay.Text = ogdDetail.GetData(nSeq, ogdDetail.Cols["otbColPdtNameStd"].Index).ToString();

                        // Select And Show Rows Last Update
                        //++++++++++++++
                        break;

                    case 2:
                        cPdtQty = Convert.ToDecimal(ogdOrder.Rows[nSeq].Cells["olaPdtQty"].Value);
                        cSetPrice = Convert.ToDecimal(ogdOrder.Rows[nSeq].Cells["olaSetPrice"].Value);
                        cSumPrice = cPdtQty * cSetPrice;

                        cSumPrice = (cSumPrice - pcDis) + pcChg;
                        ogdOrder.Rows[nSeq].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow);

                        cSum = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells["olaPdtSumPrice"].Value));
                        olaTotal.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells["olaPdtQty"].Value));
                        olaQty.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        //Show Last Event
                        //*Net 63-07-31 ปรับตาม Moshi
                        //olaCashPayment.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString();
                        olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString()), cVB.nVB_DecShow);
                        olaTitleCashPay.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtName"].Value.ToString();

                        break;
                }
                //cPdtQty = Convert.ToDecimal(ogdOrder.Rows[nSeq].Cells["olaPdtQty"].Value);
                //cSetPrice =Convert.ToDecimal(ogdOrder.Rows[nSeq].Cells["olaSetPrice"].Value);
                //cSumPrice = cPdtQty * cSetPrice;

                //cSumPrice = (cSumPrice - pcDis) + pcChg;
                //ogdOrder.Rows[nSeq].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow);

                //decimal cSum = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells["olaPdtSumPrice"].Value));
                //olaTotal.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                //decimal cQty = ogdOrder.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells["olaPdtQty"].Value));
                //olaQty.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                new cLog().C_WRTxLog("wSale", "W_PRCxOrder : End", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxOrder : " + oEx.Message);
            }
            finally
            {
                ogdOrder.ClearSelection();
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get menu pdt
        /// </summary>
        private void W_GETxMenuPdt()
        {
            List<cmlTPSMFunc> aoFunc;

            try
            {
                aoFunc = new cFunctionKeyboard().C_GETaFuncList("037");

                foreach (cmlTPSMFunc oFunc in aoFunc)
                {
                    Button ocmMenuPdt = new Button();
                    ocmMenuPdt.Name = "ocm-" + oFunc.FTSysCode;
                    ocmMenuPdt.Tag = oFunc.FTGdtCallByName;
                    ocmMenuPdt.Size = new Size(166, 42);
                    ocmMenuPdt.FlatStyle = FlatStyle.Flat;
                    ocmMenuPdt.BackColor = cVB.oVB_ColDark;
                    ocmMenuPdt.ForeColor = Color.White;
                    ocmMenuPdt.Font = new Font(new FontFamily("Segoe UI Light"), 10f);
                    ocmMenuPdt.Margin = new Padding(5, 4, 0, 0);
                    ocmMenuPdt.Text = oFunc.FTGdtName;
                    ocmMenuPdt.Click += ocmMenuBill_Click;
                    opnMenuPdt.Controls.Add(ocmMenuPdt);
                    ocmMenuPdt = null;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxMenuPdt : " + oEx.Message); }
            finally
            {
                aoFunc = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Timing to Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmClose_Tick(object sender, EventArgs e)
        {
            try
            {
                if (nW_Time == 5)
                    this.Close();
                if (nW_Time == 15)
                    this.Hide();

                nW_Time++;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "otmClose_Tick : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get Product Group
        /// </summary>
        private void W_GETxPdtGrp()
        {
            int nRowCount = 0;
            double cMaxPage;
            try
            {
                //aoPdtGrp = new cPdt().C_GETaPdtGroup();

                //ocbGroup.Items.Clear();
                //ocbGroup.Sorted = false;
                //ocbGroup.Items.Add(new cComboboxItem(cVB.oVB_GBResource.GetString("tAll"), "0"));

                //foreach (cmlTCNMPdtGrp oGrp in aoPdtGrp)
                //{
                //    ocbGroup.Items.Add(new cComboboxItem(oGrp.FTPgpChainName, oGrp.FTPgpChain));
                //}
                //ocbGroup.SelectedIndex = 0;

                //*Em 62-11-13
                //Button ocmPdtGrp = new Button();
                //ocmPdtGrp.Name = "ocm-All";
                //ocmPdtGrp.Size = new Size(110, 50);
                //ocmPdtGrp.FlatStyle = FlatStyle.Flat;
                //ocmPdtGrp.BackColor = cVB.oVB_ColNormal;
                //ocmPdtGrp.ForeColor = Color.White;
                //ocmPdtGrp.Font = new Font(new FontFamily("Segoe UI Light"), 10f);
                //ocmPdtGrp.Margin = new Padding(4, 0, 4, 10);
                //ocmPdtGrp.Text = cVB.oVB_GBResource.GetString("tAll");
                //ocmPdtGrp.Click += ocmPdtGrp_Click;
                //opnGrpDetail.Controls.Add(ocmPdtGrp);
                //ocmPdtGrp = null;

                //foreach (cmlTCNMPdtGrp oGrp in aoPdtGrp)
                //{
                //    ocmPdtGrp = new Button();
                //    ocmPdtGrp.Name = "ocm-" + oGrp.FTPgpChain;
                //    ocmPdtGrp.Tag = oGrp.FTPgpChain;
                //    ocmPdtGrp.Size = new Size(110, 50);
                //    ocmPdtGrp.FlatStyle = FlatStyle.Flat;
                //    ocmPdtGrp.BackColor = cVB.oVB_ColNormal;
                //    ocmPdtGrp.ForeColor = Color.White;
                //    ocmPdtGrp.Font = new Font(new FontFamily("Segoe UI Light"), 10f);
                //    ocmPdtGrp.Margin = new Padding(4, 0, 4, 10);
                //    ocmPdtGrp.Text = oGrp.FTPgpChainName;
                //    ocmPdtGrp.Click += ocmPdtGrp_Click;
                //    opnGrpDetail.Controls.Add(ocmPdtGrp);
                //    ocmPdtGrp = null;
                //}


                //*Arm 62-11-16 Get Product Group
                //aW_oPdtGrp = new cPdt().C_GETaPdtGroup(nW_GrpStartRow);
                //cmlTCNMPdtGrp oGrp = new cmlTCNMPdtGrp();

                //*Net 63-01-13 Get Product TouchGroup
                aW_oPdtTouchGrp = new cPdt().C_GETaPdtTouchGroup(nW_GrpStartRow);
                cmlTCNMPdtTouchGrp oTGrp = new cmlTCNMPdtTouchGrp(); //รายการทั้งหมด

                //oGrp.FTPgpChain = "All";
                //oGrp.FTPgpChainName = cVB.oVB_GBResource.GetString("tAll");
                //oGrp.FTImgObj = "";
                //olaDisplayGrp.Text = cVB.oVB_GBResource.GetString("tAll");      //*Arm 62-11-18  แสดงชื่อกลุ่มสินค้า ที่กำลังแสดงรายการ
                //aW_oPdtGrp.Insert(0, oGrp);

                oTGrp.FTTcgCode = "All";
                oTGrp.FTTcgName = cVB.oVB_GBResource.GetString("tAll");
                oTGrp.FTImgObj = "";
                //olaDisplayGrp.Text = cVB.oVB_GBResource.GetString("tAll");
                aW_oPdtTouchGrp.Insert(0, oTGrp);


                W_GETxButtonPdtGrp(0, cVB.nVB_GrpPerPage);

                //**************************************
                // เงื่อนไขการแบ่งหน้า
                //if (aW_oPdtGrp.Count > 0)
                //nRowCount = aW_oPdtGrp.Count;

                //*Net 63-01-13 เปลี่ยนจาก PdtGrp เป็น PdtTouchGrp
                if (aW_oPdtTouchGrp.Count > 0)
                    nRowCount = aW_oPdtTouchGrp.Count;

                cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / cVB.nVB_GrpPerPage); // *Arm 62-10-16 - คำนวณจำนวนหน้า

                if (cMaxPage == 0)      // ถ้าคำนวณได้ 0 ให้ มีค่าเท่า 1
                {
                    cMaxPage = 1;
                }

                nW_GrpMaxPage = Convert.ToInt32(cMaxPage);

                if (nW_GrpCurPage == 1)    // เช็คเงื่อนไขกรณีอยู่หน้าแรก
                {
                    if (cMaxPage > 1)   // จำนวนหน้าทั้งหมดมากกว่า 1 : ไม่แสดงปุ่มย้อนกลับ แต่แสดงปุ่มถัดไป
                    {
                        ocmGrpPrevious.Enabled = false;
                        ocmGrpNext.Enabled = true;
                    }
                    else                // จำนวนหน้าทั้งหมด 1 หน้า : ไม่แสดงปุ่ม ย้อนกลับและถัดไป
                    {
                        ocmGrpPrevious.Enabled = false;
                        ocmGrpNext.Enabled = false;

                        ocmGrpNext.Visible = false;
                        ocmGrpPrevious.Visible = false;
                    }
                }

                //**************************************

                W_GETxPdt();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxPdtGrp : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_GETxButtonPdtGrp(int pnStartRow, int pnEndRow)
        {
            uProductGroup oProductGroup;
            Bitmap oImage;
            Color oColor = Color.White;
            string tPath;
            int nEndRow = 0; //*Em 63-01-06
            try
            {
                opnGrpDetail.Controls.Clear();  // Clear Panel Control

                //tPath = Application.StartupPath + @"/AdaImage/ProductSale/";
                //*Em 63-01-06
                //if (aW_oPdtGrp.Count < pnEndRow)
                //{
                //    nEndRow = aW_oPdtGrp.Count;
                //}
                //else
                //{
                //    nEndRow = pnEndRow;
                //}
                //++++++++++++++

                //*Net 63-01-13 เปลี่ยนจาก PdtGrp เป็น PdtTouchGrp
                if (aW_oPdtTouchGrp.Count < pnEndRow)
                {
                    nEndRow = aW_oPdtTouchGrp.Count;
                }
                else
                {
                    nEndRow = pnEndRow;
                }

                // ปุ่มแสดงกลุ่มสินค้า
                //for (int nRow = pnStartRow; nRow < pnEndRow; nRow++)
                for (int nRow = pnStartRow; nRow < nEndRow; nRow++) //*Em 63-01-06
                {
                    //tPath = aW_oPdtGrp[nRow].FTImgObj;
                    tPath = aW_oPdtTouchGrp[nRow].FTImgObj; //*Net 63-01-13 เปลี่ยนจาก PdtGrp เป็น PdtTouchGrp
                    if (File.Exists(tPath))
                    {
                        //*Net 63-08-01 ปรับการดึงรูปไม่ให้ lock file
                        using (Image oImg = Image.FromFile(tPath))
                        {
                            oImage = new Bitmap(oImg);
                        }
                    }
                    else
                    {
                        //oImage = Properties.Resources.HOME_KB044_1;
                        oImage = null;//Properties.Resources.C_KBDxProduct;
                        oColor = cVB.oVB_ColNormal;
                    }

                    //*Net เปลี่ยนจาก PdtGrp เป็น PdtTouchGrp
                    //oProductGroup = new uProductGroup(oImage, oColor, aW_oPdtGrp[nRow].FTPgpChain, aW_oPdtGrp[nRow].FTPgpChainName);
                    oProductGroup = new uProductGroup(oImage, oColor, aW_oPdtTouchGrp[nRow].FTTcgCode, aW_oPdtTouchGrp[nRow].FTTcgName);
                    opnGrpDetail.Controls.Add(oProductGroup);

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxButtonPdtGrp : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT;  //*Em 62-01-25  Waterpark
            List<cmlTPSMFunc> aoMenuB;  //*Em 62-01-25  Waterpark
            int nItem;  //*Em 62-01-25  Waterpark
            try
            {
                aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                //foreach (cmlTPSMFunc oKb in aoKb)
                //{
                //    switch (oKb.FTSysCode)
                //    {
                //        case "KB010":
                //            ocmHelp.Visible = true;
                //            ocmHelp.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB022":
                //            ocmShwKb.Visible = true;
                //            ocmShwKb.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB027":
                //            ocmCalculate.Visible = true;
                //            ocmCalculate.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB046":
                //            ocmKB.Visible = true;
                //            ocmKB.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB047":
                //            ocmAbout.Visible = true;
                //            ocmAbout.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB003":
                //            ocmBack.Visible = true;
                //            ocmBack.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB019":
                //            if (!cVB.bVB_ModeScan)
                //            {
                //                ocmFavorite.Visible = true;
                //                ocmFavorite.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            }
                //            break;

                //        case "KB020":
                //            if (!cVB.bVB_ModeScan)
                //            {
                //                ocmSeachScan.Visible = true;
                //                ocmSeachScan.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            }
                //            break;

                //        case "KB021":
                //            if (!cVB.bVB_ModeScan)
                //            {
                //                ocmViewList.Visible = true;
                //                ocmViewList.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            }
                //            break;

                //        case "KB028":
                //            if (!cVB.bVB_ModeScan)
                //            {
                //                ocmSortAZ.Visible = true;
                //                ocmSortAZ.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            }
                //            break;

                //        case "KB029":
                //            if (!cVB.bVB_ModeScan)
                //            {
                //                ocmSortZA.Visible = true;
                //                ocmSortZA.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            }
                //            break;

                //        case "KB023":   // Change Unit
                //            break;
                //    }
                //}

                //*Em 62-01-25  Waterpark
                aoMenuT = (from oBar in aoKb where oBar.FNGdtPage == 1 orderby oBar.FNGdtUsrSeq select oBar).ToList();
                aoMenuB = (from oBar in aoKb where oBar.FNGdtPage == 2 orderby oBar.FNGdtUsrSeq select oBar).ToList();

                if (aoMenuT.Count > 0)
                {
                    opnMenuT.Controls.Clear();
                    opnMenuT.RowCount = aoMenuT.Count + 1;
                    nItem = 0;
                    foreach (cmlTPSMFunc oMenu in aoMenuT)
                    {
                        ocmMenu = new Button();
                        ocmMenu.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                        ocmMenu.FlatStyle = FlatStyle.Flat;
                        ocmMenu.FlatAppearance.BorderSize = 0;
                        ocmMenu.Font = new Font("Segoe UI Semibold", 10F);
                        ocmMenu.ForeColor = Color.White;
                        ocmMenu.Text = "".PadLeft(5) + oMenu.FTGdtName;
                        ocmMenu.TextAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.Name = oMenu.FTGdtCallByName;
                        ocmMenu.BackColor = cVB.oVB_ColDark;
                        ocmMenu.Enabled = true;

                        try
                        {
                            //*Arm 63-02-20
                            if (oMenu.FTGdtCallByName == "C_KBDxDrawer")
                            {
                                ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName + "_32")));
                            }
                            else
                            {
                                ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                            }

                        }
                        catch { }
                        ocmMenu.TextImageRelation = TextImageRelation.ImageBeforeText;

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 260;
                        opnMenuT.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuT.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuT.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }

                if (aoMenuB.Count > 0)
                {
                    opnMenuB.Controls.Clear();
                    opnMenuB.RowCount = aoMenuB.Count + 1;
                    nItem = 1;

                    Panel oPanel;
                    oPanel = new Panel();
                    oPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                    oPanel.Height = opnMenuB.Height - ((opnMenuB.RowCount - 1) * 55);
                    opnMenuB.Controls.Add(oPanel);

                    opnMenuB.RowStyles[0].SizeType = SizeType.AutoSize;
                    foreach (cmlTPSMFunc oMenu in aoMenuB)
                    {
                        ocmMenu = new Button();
                        ocmMenu.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                        //ocmMenu.Margin = new Padding(2);
                        ocmMenu.FlatStyle = FlatStyle.Flat;
                        ocmMenu.FlatAppearance.BorderSize = 0;
                        ocmMenu.Font = new Font("Segoe UI Semibold", 10F);
                        ocmMenu.ForeColor = Color.White;
                        ocmMenu.Text = "".PadLeft(5) + oMenu.FTGdtName;
                        ocmMenu.TextAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.Name = oMenu.FTGdtCallByName;
                        ocmMenu.TextImageRelation = TextImageRelation.ImageBeforeText;
                        ocmMenu.BackColor = cVB.oVB_ColDark;
                        ocmMenu.Enabled = true;
                        try
                        {
                            ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        }
                        catch { }

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 260;
                        opnMenuB.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuB.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuB.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }
                //++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get Product
        /// </summary>
        public void W_GETxPdt(int pnMode = 0, string ptValue = "", int pnSchBy = 0)
        {
            List<cmlPdtDetail> aoPdt;
            uProduct oProduct = null;
            Bitmap oImage;
            Color oColor = Color.White;
            string tPath;
            int nRowCount = 0;
            double cMaxPage;
            ocmSchPrevious.Visible = true;
            ocmSchNextPage.Visible = true;
            try
            {
                opnPdt.Controls.Clear();
                ogdProduct.Rows.Clear();

                switch (nW_Mode)
                {
                    case 5:     // Rental
                        aoPdt = new List<cmlPdtDetail>();
                        tPath = Application.StartupPath + @"/AdaImage/ProductRental/";
                        break;

                    default:     // Sale
                        //aoPdt = new cPdt().C_GETaPdtSale(ptValue, pnMode, pnSchBy, nW_StartRow, tW_PgpChain, ocmViewList.Tag.ToString());
                        //aoPdt = new cPdt().C_GETaPdtSale(ptValue, pnMode, pnSchBy, nW_StartRow, tW_PgpChain);       //*Em 62-01-25  WaterPark
                        aoPdt = new cPdt().C_GETaPdtSale(ptValue, pnMode, pnSchBy, nW_StartRow, tW_PgpChain, "", nW_SortPdt); //*Em 62-06-27
                        tPath = Application.StartupPath + @"/AdaImage/ProductSale/";
                        break;
                }

                foreach (cmlPdtDetail oPdt in aoPdt)
                {
                    //tPath += oPdt.tPdtCode + ".png";
                    tPath = oPdt.tPicPath;  //*Em 62-06-26
                    if (File.Exists(tPath))
                    {
                        //*Net 63-08-01 ปรับการดึงรูปไม่ให้ lock file
                        using (Image oImg = Image.FromFile(tPath))
                        {
                            oImage = new Bitmap(oImg);
                        }
                    }
                    else
                    {
                        //oImage = Properties.Resources.HOME_KB044_1;
                        oImage = null;//Properties.Resources.C_KBDxProduct;    //*Em 62-01-29  AdaPos 5.0
                        oColor = cVB.oVB_ColLight;
                    }

                    //nRowCount = oPdt.nRowCount;


                    oProduct = new uProduct(oImage, oColor, oPdt);
                    opnPdt.Controls.Add(oProduct);

                    ogdProduct.Rows.Add(oPdt.tPdtCode, oPdt.tBarcode, oPdt.tPdtName, oPdt.tUnitName, oPdt.cUnitFactor, oW_SP.SP_SETtDecShwSve(1, oPdt.cPdtPrice, cVB.nVB_DecShow), oPdt.tStaAlwDis);
                    ogdProduct.Refresh(); //*Net 63-05-25
                    this.Refresh(); //*Net 63-05-25
                }

                if (aoPdt.Count > 0)
                    nRowCount = aoPdt[0].nRowCount;

                //cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / cVB.nVB_MaxData);
                cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / cVB.nVB_PdtPerPage); // *Arm 62-11-15 [ปรับ]


                nW_MaxPage = Convert.ToInt32(cMaxPage);// *Arm 62-10-16 - จำนวนหน้าทั้งหมด
                //nW_TotalCount = ((nW_CurPage - 1) * cVB.nVB_MaxData) + aoPdt.Count;// *Arm 62-10-16 จำนวนต่อหน้า

                if (cMaxPage == 0)
                {
                    cMaxPage = 1;
                }

                // *Arm 62-10-29 - กรณีอยู่หน้าแรก
                if (nW_CurPage == 1)    // *Arm 62-10-16
                {
                    if (cMaxPage > 1) // จำนวนหน้าทั้งหมดมากกว่า 1 : ไม่แสดงปุ่มย้อนกลับ แต่แสดงปุ่มถัดไป
                    {
                        ocmSchPrevious.Enabled = false;
                        ocmSchNextPage.Enabled = true;
                        //ocmSchPrevious.Visible = false; 
                        //ocmSchNextPage.Visible = true; 
                    }
                    else // จำนวนหน้าทั้งหมด 1 หน้า : ไม่แสดงปุ่ม ย้อนกลับและถัดไป
                    {
                        ocmSchPrevious.Enabled = false;
                        ocmSchNextPage.Enabled = false;
                        ocmSchPrevious.Visible = false;
                        ocmSchNextPage.Visible = false;

                    }
                }
                //else
                //{
                //ocmSchNextPage.Visible = false;  // *Arm 62-10-16 
                //}

                olaPagePdt.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), nRowCount, nW_CurPage, cMaxPage);

                //if (oProduct != null)
                //    oProduct.U_GETxFormSale(this);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxPdt : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmSchNextPage_Click(object sender, EventArgs e)   // *Arm 62-10-16
        {
            try
            {
                //nW_StartRow = (cVB.nVB_MaxData * nW_CurPage);
                nW_StartRow = (cVB.nVB_PdtPerPage * nW_CurPage); // *Arm 62-11-15 []
                nW_CurPage++;
                W_GETxPdt(2);
                if (nW_CurPage == nW_MaxPage) // หน้าสุดท้าย : แสดงปุ่มย้อนกลับ ปิดปุ่มถัดไป
                {
                    ocmSchPrevious.Enabled = true;
                    ocmSchNextPage.Enabled = false;
                    //ocmSchPrevious.Visible = true;  //*Arm 62-11-12 เพิ่ม
                    //ocmSchNextPage.Visible = false;
                }
                else
                {
                    ocmSchNextPage.Enabled = true;
                    ocmSchPrevious.Enabled = true;
                    //ocmSchNextPage.Visible = true;
                    //ocmSchPrevious.Visible = true;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmSchNextPage_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmSchPrevious_Click(object sender, EventArgs e)   // *Arm 62-10-16
        {
            try
            {
                //nW_StartRow = ((nW_CurPage - 1) * cVB.nVB_MaxData) - cVB.nVB_MaxData;
                nW_StartRow = ((nW_CurPage - 1) * cVB.nVB_PdtPerPage) - cVB.nVB_PdtPerPage;// *Arm 62-11-15 []
                nW_CurPage--;
                W_GETxPdt(2);
                if (nW_CurPage == 1) // หน้าแรก : ปิดปุ่มย้อนกลับ
                {
                    ocmSchPrevious.Enabled = false;
                    ocmSchNextPage.Enabled = true;
                    //ocmSchPrevious.Visible = false;
                }
                else
                {
                    ocmSchNextPage.Enabled = true;
                    ocmSchPrevious.Enabled = true;
                    //ocmSchPrevious.Visible = true;
                    //ocmSchNextPage.Visible = true;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmSchPrevious_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// แสดงสินค้าตามกลุ่มสินค้า
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tPgpChain = "";

            try
            {
                //if (ocbGroup.SelectedIndex != 0)
                //    tPgpChain = ((cComboboxItem)ocbGroup.SelectedItem).HiddenValue;

                tW_PgpChain = tPgpChain;
                nW_CurPage = 1;
                nW_StartRow = 0;
                W_GETxPdt();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocbGroup_SelectedIndexChanged : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Search Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //*Net 63-07-31
                if (!String.IsNullOrEmpty(cVB.tVB_RefDocNo) && cVB.tVB_DocNo.StartsWith("R"))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoEditItemRet"), 3);
                    return;
                }
                W_GETxPdt(2, otbSearch.Text, ocbSearchBy.SelectedIndex);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmSearch_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Scan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmScan_Click(object sender, EventArgs e)
        {
            try
            {
                //W_PRCxScan();
                //W_SCHxSearchPdt(otbScan.Text);    //*Arm 63-05-04  Comment Code

                if (cVB.oVB_ReferSO != null && cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1") //*Arm 63-05-13
                {
                    return;
                }

                //*Net 63-07-31
                if (!String.IsNullOrEmpty(cVB.tVB_RefDocNo) && cVB.tVB_DocNo.StartsWith("R"))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoEditItemRet"), 3);
                    return;
                }
                W_SCHxSearchPdt(otbScan.Text, cW_SetQty);   //*Arm 63-05-04 
                otbScan.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmScan_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process Scan (*Arm 63-09-13 ปรับเป็น Public)
        /// </summary>
        //private void W_PRCxScan()
        public void W_PRCxScan()
        {
            List<cmlPdtDetail> aoPdt;
            cmlPdtOrder oOrder;
            string tTxtScan = "";
            cStock oStock = new cStock();
            try
            {
                //new cLog().C_WRTxLog("wSale", "W_PRCxScan : Start");
                switch (nW_Mode)
                {
                    case 5:     // Rental
                        aoPdt = new List<cmlPdtDetail>();
                        break;

                    default:     // Sale
                        //aoPdt = new cPdt().C_GETaPdtSale(otbScan.Text, 1, 0, nW_StartRow);
                        // Zen 63-03-12 Comment ^ และ เพิ่ม Code ดึงข้อมูลใหม่
                        //new cLog().C_WRTxLog("wSale", "W_PRCxScan : C_GETaPdt");
                        aoPdt = new cPdt().C_GETaPdt(otbScan.Text);
                        break;
                }
                tTxtScan = otbScan.Text;//*Em 63-05-27
                otbScan.Clear();    //*Em 63-05-26
                if (aoPdt.Count == 1)
                {
                    // ตรวจสอบ ต้นทุนเป็นศูนย์
                    // เช็คราคาขายเป็น 0

                    oOrder = new cmlPdtOrder();
                    oOrder.cFactor = aoPdt[0].cUnitFactor;

                    //oOrder.cQty = 1;  //*Arm 63-05-03 Comment Code
                    oOrder.cQty = cW_SetQty;  //*Arm 63-05-03

                    oOrder.tBarcode = aoPdt[0].tBarcode;
                    oOrder.tPdtCode = aoPdt[0].tPdtCode;
                    oOrder.tPdtName = aoPdt[0].tPdtName;
                    oOrder.tUnit = aoPdt[0].tUnitName;
                    oOrder.tStaPdt = "1";
                    oOrder.tPgpChain = aoPdt[0].tPgpChain; //*Arm 63-06-19
                    oOrder.tStkControl = aoPdt[0].tStkControl;  //*Em 63-08-14

                    if (cVB.cVB_QRPayAmt > 0 && string.Equals(cVB.tVB_PdtCodeSrv, aoPdt[0].tPdtCode))
                    {
                        oOrder.cSetPrice = cVB.cVB_QRPayAmt;
                    }
                    else
                    {

                        oOrder.cSetPrice = aoPdt[0].cPdtPrice;
                        //Zen 63-03-12 Comment ^ และแก้ Code ให้ดึง Price จาก Store
                        //oOrder.cSetPrice = new cPdt().C_GETaPrice(aoPdt);

                        //*Arm 62-09-23
                        if (aoPdt[0].tSaleType == "2")
                        {
                            //new cLog().C_WRTxLog("wSale", "W_PRCxScan : Open wEditSalePrice");
                            wEditSalePrice owEditSalePrice = new wEditSalePrice(oOrder.tPdtCode, oOrder.tPdtName, aoPdt[0].cPdtPrice);
                            owEditSalePrice.ShowDialog();
                            if (owEditSalePrice.DialogResult == DialogResult.OK)
                            {
                                oOrder.cSetPrice = owEditSalePrice.rcPrice;
                            }
                        } //*Arm 62-09-23
                    }
                    //new cLog().C_WRTxLog("wSale", "W_PRCxScan : W_ADDxPdtToOrder");

                    //if (cStock.C_CHKbSalStock(ref oOrder)) //*Net 63-05-22 //*Arm 63-06-11 ยกมาจาก SCK เดิม (P1RFP-002 โปรแกรมขายสามารถตรวจสอบ Stock แบบ Realtime จากระบบ KADS)
                    //W_ADDxPdtToOrder(oOrder);

                    new cLog().C_WRTxLog("wSale", "W_PRCxScan : tVB_WahStaChkStk = " + cVB.tVB_WahStaChkStk, cVB.bVB_AlwPrnLog);
                    new cLog().C_WRTxLog("wSale", "W_PRCxScan : tStkControl = " + oOrder.tStkControl, cVB.bVB_AlwPrnLog);
                    //*Arm 63-06-19 P1RFP-002 ตรวจสอบ Stock แบบ Realtime จากระบบ KADS 
                    if (!string.IsNullOrEmpty(cVB.tVB_WahStaChkStk) && cVB.tVB_WahStaChkStk == "3" && oOrder.tStkControl == "1") //(TCNMWaHouse.FTWahStaChkStk = 3 :ใช้ตรวจสอบในขั้นตอนการขาย 3: Check Online  )
                    {
                        new cLog().C_WRTxLog("wSale", "W_PRCxScan : Check Stock "+ oOrder.tPdtCode, cVB.bVB_AlwPrnLog); //*Arm 63-08-19
                        Cursor.Current = Cursors.WaitCursor; //*Arm 63-08-19
                        oStock.C_CHKbScanPdtCheckStock(oOrder);
                        Cursor.Current = Cursors.Default; //*Arm 63-08-19
                    }
                    else
                    {
                        W_ADDxPdtToOrder(oOrder);
                    }
                    //+++++++++++++++

                }
                else if (aoPdt.Count > 1)
                {
                    new cLog().C_WRTxLog("wSale", "W_PRCxScan : W_SCHxSearchPdt", cVB.bVB_AlwPrnLog);
                    //W_SCHxSearchPdt(otbScan.Text, cW_SetQty); //*Arm 63-02-28
                    W_SCHxSearchPdt(tTxtScan, cW_SetQty); //*Em 63-05-27
                }
                else
                {
                    //*Arm 63-03-04
                    if (nW_ScanSO == 1)
                    {
                        // Mode Scanl SO
                        bW_ResultScanSO = false;
                        otbScan.Focus();
                    }
                    else
                    {
                        // Mode Scanl แบบปกติ
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNotFoundPdt"), 3);
                        otbScan.Focus();
                    }
                }
                //new cLog().C_WRTxLog("wSale", "W_PRCxScan : End");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_PRCxScan : " + oEx.Message); }
            finally
            {
                oStock = null; //*Arm 63-06-12
                //otbScan.Clear();
                otbScan.Focus();
                cW_SetQty = 1; //*Arm 63-05-03 กำหนดจำนวนสินค้าเริ่มต้นกลับเป็น 1 สำหรับรายการต่อไป
                aoPdt = null;
                oOrder = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Check Return From Payment
        /// </summary>
        private void W_CHKxReturnFromPayment()
        {
            try
            {
                // ถ้ามาจากหน้า Payment จะดึงข้อมูลเก่ามาแสดง
                // ถ้ายังไม่ได้ Gen Bill จะ Gen Bill ใหม่

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_CHKxReturnFromPayment : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                //*Net 63-07-31
                if (!String.IsNullOrEmpty(cVB.tVB_RefDocNo) && cVB.tVB_DocNo.StartsWith("R"))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoEditItemRet"), 3);
                    return;
                }
                switch (e.KeyData)
                {
                    case Keys.Enter:
                        W_GETxPdt(2, otbSearch.Text, ocbSearchBy.SelectedIndex);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "otbSearch_KeyDown : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbScan_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{

            //switch (e.KeyData)
            //{
            //    case Keys.Enter:
            //        W_PRCxScan();
            //        break;
            //}

            if (e.KeyCode == Keys.Enter)
            {
                new cLog().C_WRTxLog("wSale", "otbScan_KeyDown : Process Scan " + otbScan.Text + " Start...", cVB.bVB_AlwPrnLog);
                if (cVB.oVB_ReferSO != null && cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1") //*Arm 63-05-13
                {
                    return;
                }

                if (!String.IsNullOrEmpty(cVB.tVB_RefDocNo) && cVB.tVB_DocNo.StartsWith("R"))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoEditItemRet"), 3);
                    return;
                }
                W_PRCxScan();
                new cLog().C_WRTxLog("wSale", "otbScan_KeyDown : Process Scan " + otbScan.Text + " End...", cVB.bVB_AlwPrnLog);
            }

            //}
            //catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "otbScan_KeyDown : " + oEx.Message); }
            //finally
            //{
            //    //new cSP().SP_CLExMemory();
            //}
        }

        private void wSale_Activated(object sender, EventArgs e)
        {
            //new cSP().SP_CLExMemory();
            if (bW_Activate == false)
            {
                ////*Arm 63-03-03
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1:
                        cVB.tVB_KbdScreen = "SALESTD";
                        break;
                    case 2:
                        cVB.tVB_KbdScreen = "SALE";
                        break;
                }
                //++++++++++++++

                //if (!string.IsNullOrEmpty(cVB.tVB_PriceGroup))
                //{
                //    W_GETxPdt();
                //}

                if (cVB.cVB_QRPayAmt > 0)
                {
                    //หาสินค้าบริการตาม Option มาใส่ grid
                    otbScan.Text = cVB.tVB_PdtCodeSrv;
                    W_PRCxScan();
                    //Call ปุ่มชำระเงิน พร้อมชำระ
                    if (ogdOrder.Rows.Count > 0)
                    {
                        cVB.cVB_QRPayAmt = 0;
                        ocmPayment_Click(ocmPayment, null);
                    }
                }

                bW_Activate = true;

                //cW_SetQty = 1; //*Arm 63-05-03 กำหนดจำนวนสินค้าเริ่มต้นกลับเป็น 1 สำหรับรายการต่อไป
                //olaTitleCashPay.Text = ""; //*Arm 63-05-04
                //olaCashPayment.Text = "0.00"; //*Arm 63-05-04
            }
            otbScan.Focus();
        }

        private void wSale_Deactivate(object sender, EventArgs e)
        {
            bW_Activate = false;
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnQMember.Visible == true)
                {
                    opnQMember.Visible = false;
                    cSP.SP_GETxCountQMember(olaMsgCountQMem, opnQMember); //*Arm 62-10-30
                }

                //*Em 62-09-17
                if (opnNotify.Visible == false)
                {
                    cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                }
                //+++++++++++++

                //*Arm 62-10-26
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmNotify_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmQMember_Click(object sender, EventArgs e)   //*Arm 62-10-25
        {
            try
            {
                if (opnNotify.Visible == true)
                {
                    opnNotify.Visible = false;
                }

                if (opnQMember.Visible == false)
                {
                    cSP.SP_GETxCountQMember(olaMsgCountQMem, opnQMember);
                }

                //*Arm 62-10-26
                cSP.SP_CHKxQMember(olaMsgCountQMem, opnQMember);
                cSP.SP_GETxCountQMember(olaMsgCountQMem, opnQMember); //*Arm 62-10-30

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHome", "ocmQMember_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        private void opnMenuPdtList_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (!opnMenuPdtList.Visible)
                {
                    foreach (Button ocm in opnMenuPdt.Controls)
                    {
                        if (ocm.Tag.ToString() == "C_KBDxDisAmt" || ocm.Tag.ToString() == "C_KBDxDisPer")
                        {
                            ocm.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "ogvOrder_CellMouseClick " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    if (opnListSmallPdt.Right >= opnListSmallPdt.Left + opnListSmallPdt.Width)
        //    {
        //        opnListSmallPdt.Height = opnListSmallPdt.Height - 5;           //Move UserControl1 to left side
        //    }
        //    else
        //    {
        //        timer1.Stop();
        //    }

        //}

        private void opbToolUp_Click(object sender, EventArgs e)
        {

            opnListSmallPdt.Visible = false;
            opnMenuPdtList.Visible = false;
            //opnListSmallPdt.BringToFront();
        }

        private void ocmMore_Click(object sender, EventArgs e)
        {
            opnMenuPdtList.Visible = true;
            opnMenuPdtList.BringToFront();
        }

        private void ocmBin_Click(object sender, EventArgs e)
        {
            new cFunctionKeyboard().C_PRCxCallByName("C_KBDxVoidItem");
            opnListSmallPdt.Visible = false;
            opnMenuPdtList.Visible = false;
        }

        private void ocmFree_Click(object sender, EventArgs e)
        {
            W_FRExItemFree();
        }

        //private void otbScan_Leave(object sender, EventArgs e)
        //{
        //    otbScan.Focus();
        //    //oW_PdtPmt.C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);
        //    //oW_PdtPmt.C_PRCoCalPmt();
        //    //if (cVB.oVB_GetPmt.Rows.Count > 0)
        //    //{
        //    //    nW_CheckPmt = 1;
        //    //}
        //}

        private void opnMenu_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// โหลดรายการสินค้าจาก SO ลงรายการขาย
        /// *Arm 63-02-19
        /// </summary>
        public void W_DATxLoadSO2Order()
        {
            StringBuilder oSql; //*Arm 63-05-11
            cDatabase oDB; //*Arm 63-05-11
            DataTable odtTmp; //*Arm 63-06-24
            decimal cSum = 0;
            decimal cQty = 0;
            try
            {


                olaSO.Text = cVB.tVB_RefDocNo; //Show SO DocNo.

                //*Arm 63-05-11 ปรับ Process SO
                oSql = new StringBuilder();
                oDB = new cDatabase();
                odtTmp = new DataTable(); //*Arm 63-06-24
                DataTable odtTmpChk; //*Arm 63-06-24

                if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo == "1")
                {
                    new cSale().C_PRCbInsSO(cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo);
                }
                else
                {
                    new cSale().C_PRCbInsSO();

                }

                new cLog().C_WRTxLog("wSale", "Select DT For Add to GridView : Strat... ", cVB.bVB_AlwPrnLog);
                Cursor.Current = Cursors.WaitCursor;

                oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                oSql.AppendLine("FCXsdDis AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,"); //Arm 63-06-24
                oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd");
                oSql.AppendLine("FCXsdChg AS otbColCharge");    //Arm 63-06-24
                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("ORDER BY FNXsdSeqNo");

                //*Arm 63-06-24
                odtTmp = null;
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                //+++++++++++++

                new cLog().C_WRTxLog("wSale", "Select DT For Add to GridView : End... ", cVB.bVB_AlwPrnLog);

                new cLog().C_WRTxLog("wSale", "Add DT to GridView : Strat... ", cVB.bVB_AlwPrnLog);
                if (odtTmp != null)//*Arm 63-06-24
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        //*Arm 63-06-24 Check รายการสินค้าใบสั่งขายกับที่ Map ได้เท่ากันหรือไม่
                        if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo == "1")
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT SoDT.FTPdtCode FROM TARTSoDTTmp SoDT WITH(NOLOCK)");
                            oSql.AppendLine("LEFT JOIN " + cSale.tC_TblSalDT + " DT WITH(NOLOCK) ON DT.FNXsdSeqNo = SoDT.FNXsdSeqNo AND DT.FTXshDocNo ='" + cVB.tVB_DocNo + "' AND DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                            oSql.AppendLine("WHERE SoDT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND DT.FNXsdSeqNo is null");
                            odtTmpChk = new DataTable();
                            odtTmpChk = oDB.C_GEToDataQuery(oSql.ToString());

                            if (odtTmpChk != null && odtTmpChk.Rows.Count > 0)
                            {
                                string tMsg = "";
                                tMsg += oW_Resource.GetString("tMsgSoPdtNotFound");

                                foreach (DataRow oRow in odtTmpChk.Rows)
                                {
                                    tMsg += System.Environment.NewLine;
                                    tMsg += oRow.Field<string>("FTPdtCode");
                                }

                                oW_SP.SP_SHWxMsg(tMsg, 3);


                                //Clear ข้อมูล
                                oSql.Clear();
                                oSql.AppendLine("DELETE FROM " + cSale.tC_TblSalDT + " WITH(ROWLOCK)");
                                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                                oDB.C_GEToDataQuery(oSql.ToString());

                                cVB.tVB_RefDocNo = "";
                                olaSO.Text = "";
                                odtTmpChk = null;
                                cVB.oVB_ReferSO = null;
                                new cLog().C_WRTxLog("wSale", "W_DATxLoadSO2Order : " + tMsg, cVB.bVB_AlwPrnLog);
                                return;
                            }
                            else
                            {
                                ogdDetail.Rows.Count = ogdDetail.Rows.Fixed;
                                ogdDetail.DataSource = odtTmp; //*Arm 63-06-18
                                W_SETxColGrid(ogdDetail);

                                if (ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index) > 0)
                                {
                                    for (int nRow = ogdDetail.Rows.Fixed; nRow < ogdDetail.Rows.Count; nRow++)
                                    {
                                        decimal cDisChg = (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColDiscount"].Index) - (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColCharge"].Index);
                                        ogdDetail.SetData(nRow, ogdDetail.Cols["otbColDiscount"].Index, cDisChg);
                                    }
                                }

                            }
                        }
                        else
                        {
                            ogdDetail.Rows.Count = ogdDetail.Rows.Fixed;
                            ogdDetail.DataSource = odtTmp;
                            W_SETxColGrid(ogdDetail);

                            if (ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColCharge"].Index) > 0)
                            {
                                for (int nRow = ogdDetail.Rows.Fixed; nRow < ogdDetail.Rows.Count; nRow++)
                                {
                                    decimal cDisChg = (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColDiscount"].Index) - (decimal)ogdDetail.GetData(nRow, ogdDetail.Cols["otbColCharge"].Index);
                                    ogdDetail.SetData(nRow, ogdDetail.Cols["otbColDiscount"].Index, cDisChg);
                                }
                            }
                        }
                        
                        new cLog().C_WRTxLog("wSale", "Add DT to GridView : End... ", cVB.bVB_AlwPrnLog);

                        new cLog().C_WRTxLog("wSale", "Sum GridView : Start... ", cVB.bVB_AlwPrnLog);

                        cSum = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, ogdDetail.Cols["otbColPdtSumPriceStd"].Index));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = Convert.ToDecimal(ogdDetail.Aggregate(AggregateEnum.Sum, ogdDetail.Rows.Fixed, 3, ogdDetail.Rows.Count - ogdDetail.Rows.Fixed, 3));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdDetail.Rows.Count - ogdDetail.Rows.Fixed;

                        new cLog().C_WRTxLog("wSale", "Sum GridView : End... ", cVB.bVB_AlwPrnLog);
                        
                    }
                }

                if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                {
                    new cLog().C_WRTxLog("wSale", "Close Button Function : Start... ", cVB.bVB_AlwPrnLog);
                    switch (cVB.nVB_SaleModeStd)   // Check Mode 1:Standrad, 2:TouchScreen
                    {
                        case 1:
                            //ไม่อนุญาตให้เลือก Menu บางรายการ
                            W_GETxButtonMenuBillStd(0, cVB.nVB_MenuPerPage);

                            break;

                        case 2:
                            opnPdt.Enabled = false; //ไม่อนุญาตให้เลือกสินค้าเพิ่ม
                            break;
                    }
                    new cLog().C_WRTxLog("wSale", "Close Button Function : End... ", cVB.bVB_AlwPrnLog);
                }

                Cursor.Current = Cursors.Default;

                if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo == "1") //*Arm 63-05-13
                {
                    //*Arm 63-05-27
                    Thread oCalPro;
                    if (cVB.bVB_PmtPriGrp == true) //ถ้ามีโปรโมชั่นกลุ่มราคา
                    {
                        bW_CalPmtPrice = true; //คำนวณโปรโมชั่นกลุ่มราคา
                        if (aoW_CalPro == null) aoW_CalPro = new List<Thread>(); //*Net 63-05-24
                        oCalPro = new Thread(W_PRCxCoPmt);
                        oCalPro.IsBackground = true;
                        oCalPro.Priority = ThreadPriority.Highest;
                        oCalPro.Start();
                        aoW_CalPro.Add(oCalPro);
                    }
                    else
                    {
                        if (cVB.bVB_PmtDis == true)  //Check ถ้ามีโปรโมชั่นส่วนลด
                        {
                            bW_CalPmtPrice = false; //ไม่คำนวณโปรโมชั่นกลุ่มราคา
                            if (aoW_CalPro == null) aoW_CalPro = new List<Thread>(); //*Net 63-05-24
                            oCalPro = new Thread(W_PRCxCoPmt);
                            oCalPro.IsBackground = true;
                            oCalPro.Priority = ThreadPriority.Highest;
                            oCalPro.Start();
                            aoW_CalPro.Add(oCalPro);
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_DATxLoadSO2Order " + oEx.Message);
            }
            finally
            {
                odtTmp = null;
                cVB.aoVB_PdtReferSO = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Call By Name
        /// </summary>
        private void W_CALxByName(KeyEventArgs poKey)
        {
            //*Arm 63-02-06 -(HotKey) Created function W_CALxByName 
            string tFuncName;
            decimal cScanQty = 0;
            //bool bChkChgPdt = false;
            try
            {
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(poKey);
                if (!string.IsNullOrEmpty(tFuncName))
                {
                    //W_GETxFuncByFuncName(tFuncName);

                    //*Arm 63-05-04 - Check เงื่อนไขแก้ไข Qty ตอน Scan Barcode
                    if (tFuncName == "C_KBDxPdtQty")
                    {
                        try
                        {
                            cScanQty = Convert.ToDecimal(otbScan.Text);

                            if (cScanQty > 1 && cScanQty <= 9999)
                            {
                                cW_SetQty = (decimal)cScanQty;
                                olaTitleCashPay.Text = "x " + cScanQty.ToString();
                                //olaCashPayment.Text = "0.00";
                                olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม
                                otbScan.Text = "";
                                otbScan.Focus();
                                return;
                            }
                            else
                            {
                                W_GETxFuncByFuncName(tFuncName);
                                otbScan.Focus();
                            }
                        }
                        catch (Exception oEx)
                        {
                            W_GETxFuncByFuncName(tFuncName);
                            otbScan.Focus();
                        }
                    }
                    else
                    {
                        W_GETxFuncByFuncName(tFuncName);
                        otbScan.Focus();
                    }

                    //+++++++++++++++++++++
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_CALxByName : " + oEx.Message); }
            finally
            {
                poKey = null;
                tFuncName = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get function in form 
        /// </summary>
        public void W_GETxFuncByFuncName(string ptFuncName)
        {
            //*Arm 63-02-06 -(HotKey) Created function W_GETxFuncByFuncName
            wChooseItemRef oChooseItem;

            try
            {
                new cLog().C_WRTxLog("wSale", "W_GETxFuncByFuncName : Start", cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("wSale", "W_GETxFuncByFuncName : " + ptFuncName, cVB.bVB_AlwPrnLog);
                object oSender = new object();
                EventArgs oEv = new EventArgs();

                if (ptFuncName == "C_KBDxVoidItem" || ptFuncName == "C_KBDxPdtQty" || ptFuncName == "C_KBDxChgAmt" || ptFuncName == "C_KBDxChgPer" ||
                    ptFuncName == "C_KBDxDisAmt" || ptFuncName == "C_KBDxDisPer" || ptFuncName == "C_KBDxItemFree" || ptFuncName == "C_KBDxPdtRmk")
                {

                    if (cSale.nC_CntItem > 0)
                    {

                        //*Net 63-07-31
                        if (!String.IsNullOrEmpty(cVB.tVB_RefDocNo) && cVB.tVB_DocNo.StartsWith("R"))
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoEditItemRet"), 3);
                            return;
                        }
                    }
                    else
                    {

                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                        return;
                    }
                }


                switch (ptFuncName)
                {
                    case "C_KBDxBack":
                        try
                        {
                            switch (cVB.nVB_SaleModeStd)
                            {
                                case 1:
                                    //if (ogdPdtStd.Rows.Count == 0)
                                    if(ogdDetail.Rows.Count - ogdDetail.Rows.Fixed == 0)    //*Em 63-05-31
                                    {
                                        new wHome().Show();
                                        nW_Time = 0;
                                        cSale.C_PRCxUpdateLstDocNum(); //*Net 63-06-19
                                        //otmClose.Start();
                                        this.Close();
                                    }
                                    else
                                    {
                                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantBack"), 3);
                                    }
                                    break;

                                case 2:
                                    if (ogdOrder.Rows.Count == 0)
                                    {
                                        new wHome().Show();
                                        nW_Time = 0;
                                        //otmClose.Start();
                                        this.Close();
                                    }
                                    else
                                    {
                                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantBack"), 3);
                                    }

                                    break;
                            }

                        }
                        catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxFuncByFuncName " + oEx.Message); }
                        break;

                    case "C_KBDxPdtDetail":

                        break;

                    case "C_KBDxSortAsc":
                        W_PRCxSortPdt(1);
                        break;

                    case "C_KBDxSortDesc":
                        W_PRCxSortPdt(2);
                        break;
                    case "C_KBDxPayment":
                        //ชำระเงิน
                        ocmPayment_Click(oSender, oEv);
                        break;

                    case "C_KBDxNotify":
                        //แจ้งเตือน
                        ocmNotify_Click(oSender, oEv);
                        break;

                    case "C_KBDxSearchPdt":

                        //*Net 63-07-31
                        if (!String.IsNullOrEmpty(cVB.tVB_RefDocNo) && cVB.tVB_DocNo.StartsWith("R"))
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoEditItemRet"), 3);
                            return;
                        }
                        //*Arm 63-02-28 -Call Function เปิดฟอร์มค้นหาสินค้า
                        if (cW_SetQty <= 0) cW_SetQty = 1;
                        W_SCHxSearchPdt("",cW_SetQty); //*Net 63-08-04 ค้นหาสินค้าแบบมีจำนวน

                        break;

                    //*Arm 63-03-02 Menu bill
                    case "C_KBDxItemFree":
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxItemFree");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        goto case "Set2CstScrn"; //*Net 63-05-21
                                              //break;
                    case "C_KBDxPdtQty":
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxPdtQty");
                        //oChooseItem.ShowDialog();

                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        goto case "Set2CstScrn"; //*Net 63-05-21
                    //break;

                    case "C_KBDxChgAmt":
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxChgAmt");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        goto case "Set2CstScrn"; //*Net 63-05-21
                    //break;

                    case "C_KBDxChgPer":
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxChgPer");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        goto case "Set2CstScrn"; //*Net 63-05-21
                    //break;
                    case "C_KBDxDisAmt":
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxChgAmt");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        goto case "Set2CstScrn"; //*Net 63-05-21
                    //break;

                    case "C_KBDxDisPer":
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxDisPer");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        goto case "Set2CstScrn"; //*Net 63-05-21
                    //break;

                    case "C_KBDxVoidItem":
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxVoidItem");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        goto case "Set2CstScrn"; //*Net 63-05-21
                    //break;

                    case "C_KBDxPdtRmk":
                        //oChooseItem = new wChooseItemRef(1, "C_KBDxPdtRmk");
                        //oChooseItem.ShowDialog();
                        //if (cSale.nC_DTSeqNo == 0)
                        //{
                        //    return;
                        //}
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;
                    //+++++++++++++++++++++

                    case "C_KBDxReferSO":
                        if (cSale.nC_CntItem > 0)
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantSO"), 3);
                            return;
                        }
                        else
                        {
                            //*Arm 63-04-06
                            if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                            {
                                new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                            }
                            else
                            {
                                //new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotChooseCst"), 1);
                                new cLog().C_WRTxLog("wReferSO", "Call API2PSMaster ==> Search Customer Start...", cVB.bVB_AlwPrnLog); //*Arm 63-05-11
                                new cFunctionKeyboard().C_PRCxCallByName("C_KBDxCustomer");
                                new cLog().C_WRTxLog("wReferSO", "Call API2PSMaster ==> Search Customer End...", cVB.bVB_AlwPrnLog); //*Arm 63-05-11
                                if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                                {
                                    new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                                }
                            }
                            //++++++++++++++
                            ///new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        }
                        break;
                    case "Set2CstScrn": //*Net 63-07-08 เมื่อเปลี่ยนแปลงสินค้า ให้ไปทำที่หน้าจอ 2 ด้วย
                        if (cVB.oVB_CstScreen != null && cSale.nC_DTSeqNo != 0)
                        {
                            cVB.oVB_CstScreen.W_SETxPDTGrid(cSale.nC_DTSeqNo, 2,
                                oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 3)), cVB.nVB_DecShow));
                            cVB.oVB_CstScreen.W_SETxPDTGrid(cSale.nC_DTSeqNo, 3,
                                oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdDetail.GetData(cSale.nC_DTSeqNo, 7)), cVB.nVB_DecShow));

                            cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                            cVB.oVB_CstScreen.W_SETxSummaryAmt(olaTotalStd.Text);
                        }
                        break;
                    case "C_KBDxVehicle": //*Arm 63-06-26 GET Vehicle
                        if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                        {
                            new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        }
                        else
                        {
                            new cFunctionKeyboard().C_PRCxCallByName("C_KBDxCustomer");
                            if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                            {
                                new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                            }
                        }
                        break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                }
                new cLog().C_WRTxLog("wSale", "W_GETxFuncByFuncName : End", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                //new cSP().SP_CLExMemory();
            }
        }
        /// <summary>
        /// *Arm 63-02-28
        /// Function เปิดฟอร์มค้นหาสินค้า
        /// </summary>
        /// <param name="ptValue"></param>
        public void W_SCHxSearchPdt(string ptValue = "", decimal pcSetQty = 1)
        {

            new cLog().C_WRTxLog("wSale", "W_SCHxSearchPdt : Start", cVB.bVB_AlwPrnLog);
            //wSearchPdt oSchPdt = new wSearchPdt(ptValue);   //*Arm 63-05-04 Comment Code
            new cLog().C_WRTxLog("wSale", "W_SCHxSearchPdt : Open wSearchPdt", cVB.bVB_AlwPrnLog);
            wSearchPdt oSchPdt = new wSearchPdt(ptValue, pcSetQty);     //*Arm 63-05-04
            if (oSchPdt.ShowDialog() == DialogResult.OK)
            {
                if (cVB.oVB_PdtOrder.tSaleType == "2")
                {
                    new cLog().C_WRTxLog("wSale", "W_SCHxSearchPdt : Open wEditSalePrice", cVB.bVB_AlwPrnLog);
                    wEditSalePrice owEditSalePrice = new wEditSalePrice(cVB.oVB_PdtOrder.tPdtCode, cVB.oVB_PdtOrder.tPdtName, cVB.oVB_PdtOrder.cSetPrice);
                    owEditSalePrice.ShowDialog();
                    if (owEditSalePrice.DialogResult == DialogResult.OK)
                    {
                        cVB.oVB_PdtOrder.cSetPrice = owEditSalePrice.rcPrice;
                    }
                }
                new cLog().C_WRTxLog("wSale", "W_SCHxSearchPdt : W_ADDxPdtToOrder", cVB.bVB_AlwPrnLog);

                //if (cStock.C_CHKbSalStock(ref cVB.oVB_PdtOrder)) //*Net 63-05-22 //*Arm 63-06-11 ยกมาจาก SCK เดิม (P1RFP-002 โปรแกรมขายสามารถตรวจสอบ Stock แบบ Realtime จากระบบ KADS)
                //    W_ADDxPdtToOrder(cVB.oVB_PdtOrder);
                //W_ADDxPdtToOrder(cVB.oVB_PdtOrder);

                //*Arm 63-06-19 P1RFP-002 ตรวจสอบ Stock แบบ Realtime จากระบบ KADS 
                if (!string.IsNullOrEmpty(cVB.tVB_WahStaChkStk) && cVB.tVB_WahStaChkStk == "3" && cVB.oVB_PdtOrder.tStkControl == "1") //(TCNMWaHouse.FTWahStaChkStk = 3 :ใช้ตรวจสอบในขั้นตอนการขาย 3: Check Online  )
                {
                    new cLog().C_WRTxLog("wSale", "W_SCHxSearchPdt : Check Stock " + cVB.oVB_PdtOrder.tPdtCode, cVB.bVB_AlwPrnLog); //*Arm 63-08-19
                    Cursor.Current = Cursors.WaitCursor; //*Arm 63-08-19
                    new cStock().C_CHKbScanPdtCheckStock(cVB.oVB_PdtOrder);
                    Cursor.Current = Cursors.Default; //*Arm 63-08-19
                }
                else
                {
                    W_ADDxPdtToOrder(cVB.oVB_PdtOrder);
                }
                //+++++++++++++++
                
            }
            new cLog().C_WRTxLog("wSale", "W_SCHxSearchPdt : End", cVB.bVB_AlwPrnLog);
        }

        private void wSale_KeyDown(object sender, KeyEventArgs e)
        {
            //*Arm 63 - 02 - 06 - (HotKey)Created function wSyncData_KeyDown

            //try
            //{
            if (!((e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
                (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode == Keys.Enter)))
            {
                W_CALxByName(e);
            }

            //}
            //catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "wSale_KeyDown " + oEx.Message); }
            //finally
            //{
            //    //new cSP().SP_CLExMemory();
            //}

        }

        private void ocmPrevPage_Click(object sender, EventArgs e)
        {
            try
            {
                //*Arm 62-11-15 เงื่อนไขการกดปุ่ม Preview Panel MenuBill
                //================================================

                nW_MenuCurPage--;

                int nStartRow = ((nW_MenuCurPage) * cVB.nVB_MenuPerPage) - cVB.nVB_MenuPerPage;
                int nEndRow = cVB.nVB_MenuPerPage * (nW_MenuCurPage);

                W_GETxButtonMenuBillStd(nStartRow, nEndRow);

                //**************************************
                // เช็คเงื่อนไขแสดงปุ่ม Next และปุ่ม Previous
                if (nW_MenuCurPage == 1)            // หน้าแรก : ปิดปุ่มย้อนกลับ
                {
                    ocmPrevPage.Enabled = false;
                    ocmNextPage.Enabled = true;
                }
                else
                {
                    ocmPrevPage.Enabled = true;
                    ocmNextPage.Enabled = true;
                }

                olaShowPage.Text = oW_Resource.GetString("tPage") + " " + nW_MenuCurPage + "/" + nW_MenuMaxPage;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmPrevPage_Click : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                //*Arm 62-11-15 เงื่อนไขการกดปุ่ม Next Panel MenuBill
                //=============================================

                nW_MenuCurPage++;

                int nStartRow = (cVB.nVB_MenuPerPage * nW_MenuCurPage) - cVB.nVB_MenuPerPage;
                int nEndRow = 0;
                if (nW_MenuCurPage == nW_MenuMaxPage)   // หน้าสุดท้าย
                {
                    nEndRow = aW_oFunc.Count;
                }
                else
                {
                    nEndRow = cVB.nVB_MenuPerPage * nW_MenuCurPage;
                }

                W_GETxButtonMenuBillStd(nStartRow, nEndRow);

                //**************************************
                // เช็คเงื่อนไขแสดงปุ่ม Next และปุ่ม Previous
                if (nW_MenuCurPage == nW_MenuMaxPage)   // หน้าสุดท้าย : แสดงปุ่มย้อนกลับ ปิดปุ่มถัดไป
                {
                    ocmPrevPage.Enabled = true;
                    ocmNextPage.Enabled = false;
                }
                else
                {
                    ocmPrevPage.Enabled = true;
                    ocmNextPage.Enabled = true;
                }

                olaShowPage.Text = oW_Resource.GetString("tPage") + " " + nW_MenuCurPage + "/" + nW_MenuMaxPage;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "ocmMenuNext_Click : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// *Arm 63-05-11
        /// </summary>
        /// <param name="poGridView"></param>
        private void W_ADDxColumn(DataGridView poGridView)
        {
            try
            {
                if (poGridView.ColumnCount > 0)
                {
                    poGridView.Columns.Clear();
                }

                switch (poGridView.Name)
                {
                    case "ogdPdtStd":
                        poGridView.Columns.Add("otbColSeqStd", "");
                        poGridView.Columns.Add("otbColBarcodeStd", "");
                        poGridView.Columns.Add("otbColPdtNameStd", "");
                        poGridView.Columns.Add("otbColPdtQtyStd", "");
                        poGridView.Columns.Add("otbColUnitNameStd", "");
                        poGridView.Columns.Add("otbColSetPriceStd", "");
                        poGridView.Columns.Add("otbColDiscount", "");
                        poGridView.Columns.Add("otbColPdtSumPriceStd", "");
                        poGridView.Columns.Add("otbColPdtCodeStd", "");
                        poGridView.Columns.Add("otbColFactorStd", "");
                        poGridView.Columns.Add("otbColStaPdtStd", "");
                        poGridView.Columns.Add("otbColAlwDisStd", "");

                        break;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_ADDxColumn : " + oEx.Message);
            }
        }

        /// <summary>
        /// *Arm 63-05-11
        /// </summary>
        /// <param name="poGridView"></param>
        private void W_SETxColumn(DataGridView poGridView)
        {
            try
            {
                switch (poGridView.Name)
                {
                    case "ogdPdtStd":

                        oW_SP.SP_SETxSetGridviewFormat(poGridView);

                        //poGridView.Columns[otbColSeqStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tSeq");
                        //poGridView.Columns[otbColBarcodeStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tBarcode");
                        //poGridView.Columns[otbColPdtNameStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tPdtName");
                        //poGridView.Columns[otbColPdtQtyStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tQty");
                        //poGridView.Columns[otbColUnitNameStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tUnit");
                        //poGridView.Columns[otbColSetPriceStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tPrice");
                        //poGridView.Columns[otbColDiscount.Name].HeaderText = cVB.oVB_GBResource.GetString("tDis");
                        //poGridView.Columns[otbColPdtSumPriceStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tSummary");

                        //poGridView.Columns[otbColFactorStd.Name].Visible = false;
                        //poGridView.Columns[otbColPdtCodeStd.Name].Visible = false;
                        //poGridView.Columns[otbColStaPdtStd.Name].Visible = false;
                        //poGridView.Columns[otbColAlwDisStd.Name].Visible = false;

                        //poGridView.Columns[otbColSeqStd.Name].FillWeight = 35;
                        //poGridView.Columns[otbColBarcodeStd.Name].FillWeight = 100;
                        //poGridView.Columns[otbColPdtNameStd.Name].FillWeight = 150;
                        //poGridView.Columns[otbColPdtQtyStd.Name].FillWeight = 50;
                        //poGridView.Columns[otbColUnitNameStd.Name].FillWeight = 70;
                        //poGridView.Columns[otbColSetPriceStd.Name].FillWeight = 50;
                        //poGridView.Columns[otbColDiscount.Name].FillWeight = 50;
                        //poGridView.Columns[otbColPdtSumPriceStd.Name].FillWeight = 50;

                        ////*Em 63-05-28
                        //poGridView.Columns[otbColPdtQtyStd.Name].DefaultCellStyle.Format = "###,###,###." + new string('0', cVB.nVB_DecShow);
                        //poGridView.Columns[otbColSetPriceStd.Name].DefaultCellStyle.Format = "###,###,###." + new string('0', cVB.nVB_DecShow);
                        //poGridView.Columns[otbColDiscount.Name].DefaultCellStyle.Format = "###,###,###." + new string('0', cVB.nVB_DecShow);
                        //poGridView.Columns[otbColPdtSumPriceStd.Name].DefaultCellStyle.Format = "###,###,###." + new string('0', cVB.nVB_DecShow);
                        ////++++++++++++++

                        //poGridView.Columns[otbColSeqStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //poGridView.Columns[otbColBarcodeStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        //poGridView.Columns[otbColPdtNameStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        //poGridView.Columns[otbColPdtQtyStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //poGridView.Columns[otbColUnitNameStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //poGridView.Columns[otbColSetPriceStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        //poGridView.Columns[otbColDiscount.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        //poGridView.Columns[otbColPdtSumPriceStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        break;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_SETxColumn : " + oEx.Message);
            }
        }

        private void OgdPdtStd_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {



        }

        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                switch (poGD.Name)
                {
                    case "ogdDetail":
                        //*Net 63-07-31 ปรับตาม Moshi
                        //nWidth = poGD.Width;
                        //poGD.Cols["otbColSeqStd"].Width =nWidth*10/100 ;
                        //poGD.Cols["otbColBarcodeStd"].Width = nWidth * 15 / 100;
                        //poGD.Cols["otbColPdtNameStd"].Width = nWidth * 20 / 100;
                        //poGD.Cols["otbColPdtQtyStd"].Width = nWidth * 10 / 100;
                        //poGD.Cols["otbColUnitNameStd"].Width = nWidth * 10 / 100;
                        //poGD.Cols["otbColSetPriceStd"].Width = nWidth * 10 / 100;
                        //poGD.Cols["otbColDiscount"].Width = nWidth * 10 / 100;
                        //poGD.Cols["otbColPdtSumPriceStd"].Width = nWidth * 10 / 100;
                        nWidth = poGD.Width - 50;
                        poGD.Cols["otbColSeqStd"].Width = 50;
                        poGD.Cols["otbColBarcodeStd"].Width = nWidth * 24 / 100;
                        poGD.Cols["otbColPdtNameStd"].Width = nWidth * 20 / 100;
                        poGD.Cols["otbColPdtQtyStd"].Width = nWidth * 8 / 100;
                        poGD.Cols["otbColUnitNameStd"].Width = nWidth * 7 / 100;
                        poGD.Cols["otbColSetPriceStd"].Width = nWidth * 14 / 100;
                        poGD.Cols["otbColDiscount"].Width = nWidth * 9 / 100;
                        poGD.Cols["otbColPdtSumPriceStd"].Width = 80;   //*Em 63-08-11

                        //poGD.Cols["otbColPdtSumPriceStd"].Width = nWidth * 12 / 100;
                        //*Net 63-08-17 คำนวนความกว้าง column สุดท้ายให้รองรับ scrollbar
                        poGD.Cols["otbColPdtSumPriceStd"].Width = (nWidth * 18 / 100) - (poGD.Width - poGD.ScrollableRectangle.Width);


                        poGD.Cols["otbColSeqStd"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                        poGD.Cols["otbColBarcodeStd"].Caption = cVB.oVB_GBResource.GetString("tBarcode");
                        poGD.Cols["otbColPdtNameStd"].Caption = cVB.oVB_GBResource.GetString("tPdtName");
                        poGD.Cols["otbColPdtQtyStd"].Caption = cVB.oVB_GBResource.GetString("tQty");
                        poGD.Cols["otbColUnitNameStd"].Caption = cVB.oVB_GBResource.GetString("tUnit");
                        poGD.Cols["otbColSetPriceStd"].Caption = cVB.oVB_GBResource.GetString("tPrice");
                        poGD.Cols["otbColDiscount"].Caption = cVB.oVB_GBResource.GetString("tDis");
                        poGD.Cols["otbColPdtSumPriceStd"].Caption = cVB.oVB_GBResource.GetString("tSummary");

                        //poGD.ExtendLastCol = true; //*Net 63-07-31 ปรับตาม Moshi //*Net 63-08-17 ปรับใหม่
                        poGD.Cols["otbColPdtCodeStd"].Visible = false;
                        poGD.Cols["otbColFactorStd"].Visible = false;
                        poGD.Cols["otbColStaPdtStd"].Visible = false;
                        poGD.Cols["otbColAlwDisStd"].Visible = false;

                        poGD.Cols["otbColSeqStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColBarcodeStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColPdtNameStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColPdtQtyStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColUnitNameStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColSetPriceStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColDiscount"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColPdtSumPriceStd"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["otbColSeqStd"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColBarcodeStd"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["otbColPdtNameStd"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["otbColPdtQtyStd"].TextAlign = TextAlignEnum.RightCenter;
                        poGD.Cols["otbColUnitNameStd"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColSetPriceStd"].TextAlign = TextAlignEnum.RightCenter;
                        poGD.Cols["otbColDiscount"].TextAlign = TextAlignEnum.RightCenter;
                        poGD.Cols["otbColPdtSumPriceStd"].TextAlign = TextAlignEnum.RightCenter;

                        poGD.Cols["otbColPdtQtyStd"].Format = "###,###,##0." + new string('0',cVB.nVB_DecShow);
                        poGD.Cols["otbColSetPriceStd"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        poGD.Cols["otbColDiscount"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        poGD.Cols["otbColPdtSumPriceStd"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);

                        if (poGD.DataSource != null)
                        {
                            poGD.Cols["otbColCharge"].Visible = false;
                            poGD.Cols["otbColCharge"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        }
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_SETxColGrid : " + oEx.Message);
            }
        }

        /// <summary>
        /// '*Em 63-07-26
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmDelayChg_Tick(object sender, EventArgs e)
        {
            nW_DelayChg++;
            if (nW_DelayChg >= cVB.nVB_DelayTimeChg)
            {
                nW_DelayChg = 0;
                otmDelayChg.Enabled = false;
                olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tWelcome");
                olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow);

                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                }
            }
            else
            {
                if (ogdDetail.Rows.Count - ogdDetail.Rows.Fixed > 0)
                {
                    nW_DelayChg = 0;
                    otmDelayChg.Enabled = false;
                }
            }

        }
    }
}
