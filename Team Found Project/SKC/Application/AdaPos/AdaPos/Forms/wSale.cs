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
        DataTable oW_dtTmp;                     //*Arm 63-05-15 - for So
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

                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                //if (cVB.oVB_MQ_Member != null) cVB.oVB_MQ_Member.oEv_Jump += new EventHandler(W_QueueMember);   //*Arm 62-10-25, *Arm 63-03-04 - ปิการใช้งาน
                oW_SP.SP_PRCxFlickering(this.Handle);   //*Arm 62-11-21 [ย้ายมาเนื่องจาก Event_Jump Notification ไม่ทำงาน]

                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
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
                this.Text = "." + cSale.nC_DocType.ToString() + "." + cVB.tVB_DocNo;
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

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "wSale : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
                    opbLogoCst.BackgroundImage = Image.FromFile(tImagePath);
                    opbLogoCst.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    opbLogoCst.BackgroundImage = Properties.Resources.Adasoft;
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
                new cSP().SP_CLExMemory();
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
            if (cVB.oVB_2ndScreen != null)
            {
                cVB.oVB_2ndScreen.W_PRCxSaleOrderTo2nd(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtName"].Value.ToString(),
                   ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString(),
                   oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow), oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow));
            }
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
                //if (cVB.oVB_Sale != null)
                //{
                //    cVB.oVB_Sale.Close();
                //    cVB.oVB_Sale = null;
                //}
                //cVB.oVB_Sale = this; //*Arm 63-03-21

                ////if (ogdOrder.Rows.Count > 0)
                ////    ogdOrder.CurrentCell = ogdOrder.Rows[ogdOrder.RowCount - 1].Cells[2];

                //if (cVB.nVB_SaleModeStd == 1)
                //{
                //    if (ogdPdtStd.Rows.Count > 0)
                //        ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[2];
                //    cSale.nC_CntItem = ogdPdtStd.RowCount;
                //}
                //else
                //{
                //    if (ogdOrder.Rows.Count > 0)
                //        ogdOrder.CurrentCell = ogdOrder.Rows[ogdOrder.RowCount - 1].Cells[2];
                //    cSale.nC_CntItem = ogdOrder.RowCount;
                //}

                //ogdOrder.ClearSelection();
                //ogdProduct.ClearSelection();
                //ogdPdtStd.ClearSelection();

                //otbScan.Focus();
                //cSale.nC_CntItem = ogdOrder.RowCount;
                //if (cVB.cVB_QRPayAmt > 0)
                //{
                //    //หาสินค้าบริการตาม Option มาใส่ grid
                //    otbScan.Text = cVB.tVB_PdtCodeSrv;
                //    W_PRCxScan();
                //    //Call ปุ่มชำระเงิน พร้อมชำระ
                //    if (ogdOrder.Rows.Count > 0)
                //    {
                //        ocmPayment_Click(ocmPayment, null);
                //    }
                //}

                //////*Em 62-08-15
                ////if (nW_Mode == 4)
                ////{
                ////    if (cVB.aoVB_PdtRefund.Count > 0)
                ////    {
                ////        foreach (cmlPdtOrder oPdt in cVB.aoVB_PdtRefund)
                ////        {
                ////            W_ADDxPdtToOrder(oPdt);
                ////        }
                ////        if (ogdOrder.Rows.Count > 0)
                ////        {
                ////            cVB.cVB_QRPayAmt = 0;
                ////            ocmPayment_Click(ocmPayment, null);
                ////        }
                ////    }
                ////}
                //////+++++++++++++++++
                //if (cVB.nVB_SaleModeStd == 1)
                //{
                //    //*Arm 63-03-06
                //    if (nW_Mode == 4)
                //    {
                //        if (cVB.aoVB_PdtRefund.Count > 0)
                //        {
                //            //*Arm 63-03-18 Insert SaleHDCst บิลคืน
                //            StringBuilder oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT FTCstCode FROM TPSTSalHD WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //            cVB.tVB_CstCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                //            if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                //            {
                //                //List<cmlTCNMCst> aoCst = new List<cmlTCNMCst>();
                //                //aoCst = new cCustomer().C_SCHaCstLocal(0, 0, cVB.tVB_CstCode, 1);

                //                //foreach (cmlTCNMCst oCst in aoCst)
                //                //{
                //                //    cVB.tVB_CstName = oCst.FTCstName;
                //                //    cVB.tVB_CstTel = oCst.FTCstTel;
                //                //    cVB.tVB_CstStaAlwPosCalSo = oCst.FTCstStaAlwPosCalSo;
                //                //    cVB.tVB_PriceGroup = oCst.FTPplCodeRet;
                //                //    cVB.oVB_2ndScreen.olaGrpCst.Text = oCst.FTPplCodeRet;
                //                //    cVB.bVB_ScanQR = false;
                //                //    //cVB.nVB_CstPoint = (int)oCst.cCstPoint;
                //                //    //cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                //                //    cVB.tVB_CstCardID = oCst.FTCstCardID;   //*Arm 63-04-04

                //                //}

                //                //*Arm 63-04-04 เอาข้อมูลแต้มลูกค้าล่าสุดมาจาก หลังบ้าน
                //                cmlReqCstSch oReq = new cmlReqCstSch();
                //                oReq.ptCstName = "";
                //                oReq.ptCstCode = cVB.tVB_CstCode;
                //                oReq.ptCstTel = "";
                //                oReq.ptCstCardID = "";
                //                oReq.ptCstCrdNo = "";
                //                oReq.ptCstTaxNo = "";

                //                string tJSonCall = JsonConvert.SerializeObject(oReq);
                //                string tUrl = cVB.tVB_API2PSMaster;
                //                cClientService oCall = new cClientService();
                //                oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                //                HttpResponseMessage oRep = new HttpResponseMessage();
                //                try
                //                {
                //                    oRep = oCall.C_POSToInvoke(tUrl + "/Customer/CstSearch", tJSonCall);
                //                }
                //                catch (Exception oEx)
                //                {
                //                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click/Call API Error : " + oEx.Message);
                //                }
                //                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                //                {
                //                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                //                    cmlResCst oCstSch = new cmlResCst();
                //                    oCstSch = JsonConvert.DeserializeObject<cmlResCst>(tJSonRes);

                //                    switch (oCstSch.rtCode)
                //                    {
                //                        case "1":
                //                            if (oCstSch.raItems.Count > 0)
                //                            {
                //                                foreach (cmlResCstSch oData in oCstSch.raItems)
                //                                {
                //                                    cVB.nVB_CstPoint = Convert.ToInt32(oData.rtTxnPntQty);
                //                                    cVB.tVB_CstName = oData.rtCstName;
                //                                    cVB.tVB_CstTel = oData.rtCstTel;
                //                                    cVB.tVB_CstStaAlwPosCalSo = oData.rtCstStaAlwPosCalSo;
                //                                    cVB.tVB_PriceGroup = oData.rtPplCodeRet;
                //                                    cVB.tVB_CstCardID = oData.rtCstCardID;
                //                                    cVB.tVB_MemberCard = oData.rtCstCrdNo;
                //                                    cVB.tVB_ExpiredDate = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(oData.rdCstCrdExpire));
                //                                }
                //                            }
                //                            break;

                //                        case "800":
                //                            break;
                //                        default:
                //                            //ERROR
                //                            new cLog().C_WRTxLog("wSale", "wSale_Shown/API Response Error : " + oCstSch.rtDesc);
                //                            break;
                //                    }

                //                    cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                //                    oCstSch = null;

                //                }

                //                oCall = null;
                //                tJSonCall = null;
                //                oReq = null;
                //                oRep = null;
                //                //+++++++++++++++

                //                // Show ข้อมูลลูกค้า
                //                W_SETxTextCst();

                //                // Insert TPSTSalHDCst
                //                cSale.C_DATxInsHDCst(cVB.tVB_CstCode);

                //                // แสดงข้อมูลลูกค้า
                //                opnCst.Visible = true;
                //                opnNoCst.Visible = false;


                //            }
                //            else
                //            {
                //                cVB.tVB_PriceGroup = cVB.tVB_BchPriceGroup; //* Net 63-03-24
                //            }
                //            //++++++++++++++++++++++++


                //            foreach (cmlPdtOrder oPdt in cVB.aoVB_PdtRefund)
                //            {

                //                W_ADDxPdtToOrder(oPdt);

                //            }

                //            //*Arm 63-03-20
                //            foreach (cmlPdtDisChg oDisChg in cVB.aoVB_PdtDisChgRefund)
                //            {
                //                int nRows = 1;
                //                if (oDisChg.pnStaDis == 1)
                //                {

                //                    if (ogdPdtStd.Rows.Count > 0)
                //                    {
                //                        foreach (DataGridViewRow oRow in ogdPdtStd.Rows)
                //                        {

                //                            if (oDisChg.ptPdtCode == oRow.Cells[otbColPdtCodeStd.Name].Value.ToString() &&
                //                                oDisChg.ptBarcode == oRow.Cells[otbColBarcodeStd.Name].Value.ToString() &&
                //                                (decimal)oDisChg.pcSetPrice == Convert.ToDecimal(oRow.Cells[otbColSetPriceStd.Name].Value)) //*Arm 63-03-25 
                //                            {
                //                                cSale.nC_DTSeqNo = nRows;
                //                                cVB.oVB_OrderRowIndex = cSale.nC_DTSeqNo - 1;

                //                                cmlTPSTSalDTDis oSalDTDis = new cmlTPSTSalDTDis();
                //                                // มูลค่าสุทธิก่อนลดชาร์จ

                //                                string tB4DisChg = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oRow.Cells[otbColPdtSumPriceStd.Name].Value), cVB.nVB_DecShow);

                //                                oSalDTDis.FTBchCode = cVB.tVB_BchCode;
                //                                oSalDTDis.FTXshDocNo = cVB.tVB_DocNo;
                //                                oSalDTDis.FNXsdSeqNo = nRows;
                //                                oSalDTDis.FNXddStaDis = oDisChg.pnStaDis;

                //                                //*Arm 63-04-10
                //                                if (cVB.nVB_ReturnType == 1)
                //                                {
                //                                    oSalDTDis.FTXddDisChgTxt = oDisChg.ptDisChgTxt; //คืน Option1
                //                                }
                //                                else
                //                                {
                //                                    oSalDTDis.FTXddDisChgTxt = oW_SP.SP_SETtDecShwSve(1, (decimal)oDisChg.pcValue, cVB.nVB_DecShow).ToString();
                //                                }
                //                                //+++++++++++++
                //                                oSalDTDis.FTXddDisChgType = oDisChg.ptDisChgType;
                //                                oSalDTDis.FCXddNet = Convert.ToDecimal(tB4DisChg);
                //                                oSalDTDis.FCXddValue = oDisChg.pcValue;
                //                                new cSale().C_PRCxDisChgItem(oSalDTDis);
                //                            }
                //                            nRows++;
                //                        }
                //                    }
                //                }
                //            }

                //            //++++++++++++++


                //            switch (cVB.nVB_SaleModeStd)
                //            {
                //                case 1:
                //                    if (ogdPdtStd.Rows.Count > 0)
                //                    {
                //                        cVB.cVB_QRPayAmt = 0;
                //                        ocmPayment_Click(ocmPayment, null);
                //                    }
                //                    break;
                //                case 2:
                //                    if (ogdOrder.Rows.Count > 0)
                //                    {
                //                        cVB.cVB_QRPayAmt = 0;
                //                        ocmPayment_Click(ocmPayment, null);
                //                    }
                //                    break;
                //            }
                //        }
                //    }
                //    //+++++++++++++++++
                //}
                //else
                //{
                //    //*Em 62-08-15
                //    if (nW_Mode == 4)
                //    {
                //        if (cVB.aoVB_PdtRefund.Count > 0)
                //        {
                //            foreach (cmlPdtOrder oPdt in cVB.aoVB_PdtRefund)
                //            {
                //                W_ADDxPdtToOrder(oPdt);
                //            }
                //            if (ogdOrder.Rows.Count > 0)
                //            {
                //                cVB.cVB_QRPayAmt = 0;
                //                ocmPayment_Click(ocmPayment, null);
                //            }
                //        }
                //    }
                //}

                //*Em 62-08-15
                if (nW_Mode == 4)
                {
                    if (cVB.aoVB_PdtRefund.Count > 0)
                    {
                        switch (cVB.nVB_SaleModeStd)
                        {
                            case 1:
                                if (ogdPdtStd.Rows.Count > 0)
                                {
                                    cVB.cVB_QRPayAmt = 0;
                                    ocmPayment_Click(ocmPayment, null);
                                }
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
                cVB.oVB_Sale = this; //*Arm 63-03-05
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "wSale_Shown " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
                        if (ogdPdtStd.Rows.Count == 0)
                        {
                            new wHome().Show();
                            nW_Time = 0;
                            this.Close();
                        }
                        else
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantBack"), 3);
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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

                oW_SP.SP_SETxSetGridviewFormat(ogdPdtStd); //*Arm 63-03-03

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
                
                //ogdPdtStd.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //ogdPdtStd.RowHeadersDefaultCellStyle.Font = new Font("TH Sarabun New", 16F);

                //ogdPdtStd.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //ogdPdtStd.ColumnHeadersDefaultCellStyle.Font = new Font("TH Sarabun New", 16F);

                //ogdPdtStd.DefaultCellStyle.Font = new Font("TH Sarabun New", 16F);

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SETxDesign " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
                otbColSeqStd.HeaderText = cVB.oVB_GBResource.GetString("tSeq");
                otbColBarcodeStd.HeaderText = cVB.oVB_GBResource.GetString("tBarcode");
                otbColPdtNameStd.HeaderText = cVB.oVB_GBResource.GetString("tPdtName");
                otbColPdtQtyStd.HeaderText = cVB.oVB_GBResource.GetString("tQty");
                otbColUnitNameStd.HeaderText = cVB.oVB_GBResource.GetString("tUnit");
                otbColSetPriceStd.HeaderText = cVB.oVB_GBResource.GetString("tPrice");
                otbColDiscount.HeaderText = cVB.oVB_GBResource.GetString("tDis");
                otbColPdtSumPriceStd.HeaderText = cVB.oVB_GBResource.GetString("tSummary");

                ocmBackMenuPdt.Text = cVB.oVB_GBResource.GetString("tBack");


                olaPagePdt.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), "0", "0", "0");
                ocmPayment.Text = cVB.oVB_GBResource.GetString("tPayment");
                //olaDisplayGrp.Text = "";    //*Em 62-11-18

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SETxText " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
                olaCstName.Text = cVB.tVB_CstName;
                olaPoint.Text = cVB.nVB_CstPiontB4Used.ToString();
                olaPriLev.Text = cVB.tVB_PriceGroup;
                if(string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm  63-04-29
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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SETxTextCst " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
            }
        }


        /// <summary>
        /// Add Product
        /// </summary>
        public void W_ADDxPdtToOrder(cmlPdtOrder poOrder)
        {
            DataGridViewRow oOrder;
            decimal cSum, cQty;
            try
            {

                switch (cVB.nVB_SaleModeStd)
                {
                    case 1: // Standrad
                            //*Arm 63-03-03
                            //*********************************************************

                        oOrder = null;
                        if (nW_Mode == 4)   //*Em 63-04-29
                        {
                            ogdPdtStd.Rows.Add((Convert.ToInt32(ogdPdtStd.RowCount.ToString()) + 1).ToString(), poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), poOrder.tUnit, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), "0.00", oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow), poOrder.tPdtCode, poOrder.cFactor, poOrder.tStaPdt, poOrder.tStaAlwDis);
                            cSale.nC_DTSeqNo = ogdPdtStd.Rows.GetRowCount(DataGridViewElementStates.None);
                            cSale.C_DATxInsDT(poOrder);
                        }
                        else
                        {
                            //if (cVB.nPS_StaSumPdt == 2)
                            if(cVB.nVB_StaSumScan == 1)
                            {

                                oOrder = (from oPdt in ogdPdtStd.Rows.Cast<DataGridViewRow>()
                                          where
                                            oPdt.Cells[otbColPdtCodeStd.Name].Value.ToString() == poOrder.tPdtCode &&
                                            oPdt.Cells[otbColBarcodeStd.Name].Value.ToString() == poOrder.tBarcode &&
                                            oPdt.Cells[otbColStaPdtStd.Name].Value.ToString() == poOrder.tStaPdt &&
                                            Convert.ToDecimal(oPdt.Cells[otbColSetPriceStd.Name].Value) == poOrder.cSetPrice &&
                                            Convert.ToDecimal(oPdt.Cells[otbColFactorStd.Name].Value) == poOrder.cFactor
                                          select oPdt).FirstOrDefault();
                            }

                            if (oOrder == null || poOrder.tStaPdt == "4" || poOrder.tStaPdt == "3")
                            {

                                if (poOrder.tStaPdt == "4")
                                {
                                    if(oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                                    {
                                        ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColStaPdtStd.Name].Value = "4";
                                        oW_dtTmp.Rows.Add((Convert.ToInt32(ogdPdtStd.RowCount.ToString()) + 1).ToString(), poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), poOrder.tUnit, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells["otbColDiscount"].Value.ToString(), oW_SP.SP_SETtDecShwSve(1, (poOrder.cSetPrice * poOrder.cQty) + decimal.Parse(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells["otbColDiscount"].Value.ToString()), cVB.nVB_DecShow), poOrder.tPdtCode, poOrder.cFactor, poOrder.tStaPdt, poOrder.tStaAlwDis); //*Net 63-04-08 ดึง Discount มาไม่ถูก
                                        cSale.nC_DTSeqNo = ogdPdtStd.Rows.GetRowCount(DataGridViewElementStates.None);
                                    }
                                    else
                                    {
                                        ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColStaPdtStd.Name].Value = "4";
                                        ogdPdtStd.Rows.Add((Convert.ToInt32(ogdPdtStd.RowCount.ToString()) + 1).ToString(), poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), poOrder.tUnit, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells["otbColDiscount"].Value.ToString(), oW_SP.SP_SETtDecShwSve(1, (poOrder.cSetPrice * poOrder.cQty) + decimal.Parse(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells["otbColDiscount"].Value.ToString()), cVB.nVB_DecShow), poOrder.tPdtCode, poOrder.cFactor, poOrder.tStaPdt, poOrder.tStaAlwDis); //*Net 63-04-08 ดึง Discount มาไม่ถูก
                                        cSale.nC_DTSeqNo = ogdPdtStd.Rows.GetRowCount(DataGridViewElementStates.None);
                                    }
                                    //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColStaPdtStd.Name].Value = "4";

                                    ////ogdPdtStd.Rows.Add((Convert.ToInt32(ogdPdtStd.RowCount.ToString()) + 1).ToString(), poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), poOrder.tUnit, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), ogdPdtStd.SelectedRows[0].Cells["otbColDiscount"].Value.ToString(), oW_SP.SP_SETtDecShwSve(1, (poOrder.cSetPrice * poOrder.cQty) + decimal.Parse(ogdPdtStd.SelectedRows[0].Cells["otbColDiscount"].Value.ToString()), cVB.nVB_DecShow), poOrder.tPdtCode, poOrder.cFactor, poOrder.tStaPdt, poOrder.tStaAlwDis);  //*Arm 62-12-20 -เพิ่มกำหนดจุดทศนิยม Qty 2 ตำแหน่ง
                                    //ogdPdtStd.Rows.Add((Convert.ToInt32(ogdPdtStd.RowCount.ToString()) + 1).ToString(), poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), poOrder.tUnit, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells["otbColDiscount"].Value.ToString(), oW_SP.SP_SETtDecShwSve(1, (poOrder.cSetPrice * poOrder.cQty) + decimal.Parse(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells["otbColDiscount"].Value.ToString()), cVB.nVB_DecShow), poOrder.tPdtCode, poOrder.cFactor, poOrder.tStaPdt, poOrder.tStaAlwDis); //*Net 63-04-08 ดึง Discount มาไม่ถูก
                                    //cSale.nC_DTSeqNo = ogdPdtStd.Rows.GetRowCount(DataGridViewElementStates.None);
                                }
                                else
                                {

                                    if(oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                                    {
                                        oW_dtTmp.Rows.Add((Convert.ToInt32(ogdPdtStd.RowCount.ToString()) + 1).ToString(), poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), poOrder.tUnit, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), "0.00", oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow), poOrder.tPdtCode, poOrder.cFactor, poOrder.tStaPdt, poOrder.tStaAlwDis);
                                        cSale.nC_DTSeqNo = ogdPdtStd.Rows.GetRowCount(DataGridViewElementStates.None);
                                    }
                                    else
                                    {
                                        ogdPdtStd.Rows.Add((Convert.ToInt32(ogdPdtStd.RowCount.ToString()) + 1).ToString(), poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), poOrder.tUnit, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), "0.00", oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow), poOrder.tPdtCode, poOrder.cFactor, poOrder.tStaPdt, poOrder.tStaAlwDis);
                                        cSale.nC_DTSeqNo = ogdPdtStd.Rows.GetRowCount(DataGridViewElementStates.None);

                                    }
                                    //ogdPdtStd.Rows.Add((Convert.ToInt32(ogdPdtStd.RowCount.ToString()) + 1).ToString(), poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), poOrder.tUnit, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), "0.00", oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow), poOrder.tPdtCode, poOrder.cFactor, poOrder.tStaPdt, poOrder.tStaAlwDis);
                                    //cSale.nC_DTSeqNo = ogdPdtStd.Rows.GetRowCount(DataGridViewElementStates.None);

                                    if (cVB.bVB_RetriveBill == false)
                                    {
                                        //*Arm 62-11-18  Check การคืน
                                        if (cSale.nC_DocType == 9)
                                        {
                                            cSale.C_DATxInsDT(poOrder, "2");
                                        }
                                        else
                                        {
                                            cSale.C_DATxInsDT(poOrder);
                                        }
                                    }
                                }
                            }
                            else
                            { 
                                if(oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                                {
                                    DataRow oRow = oW_dtTmp.Rows[oOrder.Index];
                                    oRow["otbColPdtQtyStd"] = oW_dtTmp.Rows[oOrder.Index].Field<decimal>("otbColPdtQtyStd") + poOrder.cQty;
                                    oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1,oW_dtTmp.Rows[oOrder.Index].Field<decimal>("otbColPdtSumPriceStd") + (poOrder.cSetPrice * poOrder.cQty), cVB.nVB_DecShow));
                                    
                                    cSale.nC_DTSeqNo = oOrder.Index + 1;
                                    if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty(poOrder.cQty);
                                }
                                else
                                {
                                    // Qty
                                    //ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtQtyStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtQtyStd.Name].Value) + 1, cVB.nVB_DecShow);
                                    ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtQtyStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtQtyStd.Name].Value) + poOrder.cQty, cVB.nVB_DecShow); //*Arm 63-05-07
                                                                                                                                                                                                                                                   // Price
                                                                                                                                                                                                                                                   //ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtSumPriceStd.Name].Value) + poOrder.cSetPrice, cVB.nVB_DecShow);   //*Em 62-10-08
                                    ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtSumPriceStd.Name].Value) + (poOrder.cSetPrice * poOrder.cQty), cVB.nVB_DecShow); //*Arm 63-05-07
                                    cSale.nC_DTSeqNo = oOrder.Index + 1;
                                    //if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty();
                                    if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty(poOrder.cQty); //*Arm 63-05-07
                                }
                                //// Qty
                                ////ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtQtyStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtQtyStd.Name].Value) + 1, cVB.nVB_DecShow);
                                //ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtQtyStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtQtyStd.Name].Value) + poOrder.cQty, cVB.nVB_DecShow); //*Arm 63-05-07
                                //// Price
                                ////ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtSumPriceStd.Name].Value) + poOrder.cSetPrice, cVB.nVB_DecShow);   //*Em 62-10-08
                                //ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[oOrder.Index].Cells[otbColPdtSumPriceStd.Name].Value) + (poOrder.cSetPrice * poOrder.cQty), cVB.nVB_DecShow); //*Arm 63-05-07
                                //cSale.nC_DTSeqNo = oOrder.Index + 1;
                                ////if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty();
                                //if (cVB.bVB_RetriveBill == false) cSale.C_PRCxUpdateQty(poOrder.cQty); //*Arm 63-05-07
                            }
                        }
                        

                        ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                        cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdPdtStd.RowCount;

                        // Select And Show Rows Last Update
                        ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Selected = true; // Select ข้อมูลล่าสุด 
                        ogdPdtStd.FirstDisplayedScrollingRowIndex = ogdPdtStd.Rows.Count - 1;   // Scrol ล่างสุด 

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
                                cSale.nC_DTSeqNo = ogdOrder.Rows.GetRowCount(DataGridViewElementStates.None);

                            }
                            else
                            {
                                ogdOrder.Rows.Add(oW_SP.SP_SETtDecShwSve(1, poOrder.cQty, cVB.nVB_DecShow), "x", poOrder.cFactor, poOrder.tUnit, poOrder.tPdtCode, poOrder.tBarcode, poOrder.tPdtName, oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice, cVB.nVB_DecShow), oW_SP.SP_SETtDecShwSve(1, poOrder.cSetPrice * poOrder.cQty, cVB.nVB_DecShow), poOrder.tStaPdt, poOrder.tStaAlwDis);
                                cSale.nC_DTSeqNo = ogdOrder.Rows.GetRowCount(DataGridViewElementStates.None);

                                //*Arm 62-11-18  Check การคืน
                                if (cSale.nC_DocType == 9)
                                {
                                    cSale.C_DATxInsDT(poOrder, "2");
                                }
                                else
                                {
                                    cSale.C_DATxInsDT(poOrder);
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


                //*Em 63-04-25
                if (nW_Mode == 3)
                {
                    if (cVB.oVB_ReferSO != null && cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                    {
                        //not thing
                    }
                    else
                    {
                        bW_CalPmtPrice = true;  //*Em 63-05-05
                       Thread oCalPro = new Thread(W_PRCxCoPmt);
                        oCalPro.IsBackground = true;
                        oCalPro.Priority = ThreadPriority.Highest;
                        oCalPro.Start();
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
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_ADDxPdtToOrder " + oEx.Message); }
            finally
            {
                poOrder = null;
                oOrder = null;
                cW_SetQty = 1; //*Arm 63-05-03 กำหนดจำนวนสินค้าเริ่มต้นกลับเป็น 1 สำหรับรายการต่อไป
                ogdOrder.ClearSelection();

                new cSP().SP_CLExMemory();
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
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1: // Standrad

                        cSale.C_PRCxChangeQty(cSale.cC_DTQty);

                        if(oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                        {
                            oW_dtTmp.Columns[otbColDiscount.Name].ReadOnly = false;

                            DataRow oRow = oW_dtTmp.Rows[cSale.nC_DTSeqNo - 1];
                            oRow["otbColPdtQtyStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow));
                            oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow));
                            oRow["otbColDiscount"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1,
                             (Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) *
                             Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value)) -
                             cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow));
                        }
                        else
                        {
                            // Qty
                            ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow);
                            // Price
                            //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value), cVB.nVB_DecShow);
                            //*Net 63-04-10
                            ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                            ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColDiscount.Name].Value = oW_SP.SP_SETtDecShwSve(1,
                                 (Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) *
                                 Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value)) -
                                 cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                            //++++++++++++++++++++
                        }
                        //// Qty
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow);
                        //// Price
                        ////ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value), cVB.nVB_DecShow);
                        ////*Net 63-04-10
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColDiscount.Name].Value = oW_SP.SP_SETtDecShwSve(1,
                        //     (Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * 
                        //     Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value)) -
                        //     cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                        ////++++++++++++++++++++

                        ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                        // Show Total Price
                        cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        // Show Totol Qty
                        cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        // total Item
                        cSale.nC_CntItem = ogdPdtStd.RowCount;

                        // Show Last Event
                        olaCashPayment.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value.ToString();
                        olaTitleCashPay.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        // Select And Show Rows Last Update
                        ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Selected = true; // Select ข้อมูลล่าสุด 
                        ogdPdtStd.FirstDisplayedScrollingRowIndex = cSale.nC_DTSeqNo - 1; // Scrol 

                        break;

                    case 2:


                        cSale.C_PRCxChangeQty(cSale.cC_DTQty);
                        // Qty
                        ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtQty"].Value = oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow);
                        // Price
                        //ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtQty"].Value) * Convert.ToDecimal(ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaSetPrice"].Value), cVB.nVB_DecShow);
                        ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value = oW_SP.SP_SETtDecShwSve(1, cSale.C_GETcSalDTSeqPrice(cSale.nC_DTSeqNo), cVB.nVB_DecShow);
                        
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
                        olaCashPayment.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString();
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
                        bW_CalPmtPrice = true;  //*Em 63-05-05
                        Thread oCalPro = new Thread(W_PRCxCoPmt);
                        oCalPro.IsBackground = true;
                        oCalPro.Priority = ThreadPriority.Highest;
                        oCalPro.Start();
                    }
                }
                //+++++++++++++++++++++++++

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_CHAxChangePdtQty " + oEx.Message); }
            finally
            {
                //poOrder = null;
                //oOrder = null;
                ogdOrder.ClearSelection();

                new cSP().SP_CLExMemory();
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

                        if(oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                        {
                            DataRow oRow = oW_dtTmp.Rows[cSale.nC_DTSeqNo - 1];
                            oRow["otbColPdtNameStd"] = "Free-" + ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();
                            oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * 0, cVB.nVB_DecShow));
                            oRow["otbColStaPdtStd"] = "3";
                            oRow["otbColSetPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow));
                        }
                        else
                        {
                            // PdtName = 'Free-'+ PdtName
                            ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value = "Free-" + ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                            // SumPrice = Qty * SetPrice  
                            ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * 0, cVB.nVB_DecShow);

                            // StaPrd = 3 : สินค้าฟรี
                            ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColStaPdtStd.Name].Value = "3";

                            // SetPrice = 0
                            ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                        }

                        //// PdtName = 'Free-'+ PdtName
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value = "Free-" + ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        //// SumPrice = Qty * SetPrice  
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value) * 0, cVB.nVB_DecShow);

                        //// StaPrd = 3 : สินค้าฟรี
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColStaPdtStd.Name].Value = "3";

                        //// SetPrice = 0
                        //ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColSetPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);

                        // Process Update
                        cSale.C_PRCxItemFree(Convert.ToDecimal(ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtQtyStd.Name].Value)); // ส่งค่า Qty
                        ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                        // Show Total Price
                        cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        // Show Totol Qty
                        cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        // total Item
                        cSale.nC_CntItem = ogdPdtStd.RowCount;

                        // Show Last Event
                        olaCashPayment.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value.ToString();
                        olaTitleCashPay.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        // Select And Show Rows Last Update
                        ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Selected = true; // Select ข้อมูลล่าสุด 
                        ogdPdtStd.FirstDisplayedScrollingRowIndex = cSale.nC_DTSeqNo - 1; // Scrol 

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
                        olaCashPayment.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString();
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

                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                //W_SETxText();
                //*Arm 63-05-11
                if (ogdPdtStd.DataSource != null)
                {
                    ogdPdtStd.DataSource = null;
                    W_ADDxColumn(ogdPdtStd);
                    W_SETxColumn(ogdPdtStd);
                }
                //+++++++++++++
                oW_dtTmp = null; //*Arm 63-05-12

                ogdOrder.Rows.Clear();
                ogdPdtStd.Rows.Clear();
                ogdProduct.Rows.Clear();
                
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
                //+++++++++++++
                W_SETxText();
                //*Em 63-04-22
                if (cSale.bC_SetComplete)
                {
                    olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tChange");
                    olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_Change, cVB.nVB_DecShow);
                }
                //++++++++++++
                Thread oSQL = new Thread(new cDatabase().C_CLExMemorySQL);
                oSQL.IsBackground = true;
                oSQL.Priority = ThreadPriority.Highest;
                oSQL.Start();

                cVB.oVB_Sale = null;
                cVB.oVB_Sale = this;
                
                cVB.bVB_Flag = true; //*Arm 63-05-18 - สถาณะอนุญาตให้ใช้ e Couponc หรือ Loyalty หรือไม่
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wHome", "W_SETxNewDoc : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Insert รายการอ้างอิงบิลคืนลง Gridview (Arm 63-06-04)
        /// </summary>
        public void W_PRCxReferBill()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            oW_dtTmp = new DataTable();
            decimal cSum = 0;
            decimal cQty = 0;

            try
            {
                if (cVB.nVB_SaleModeStd == 1)
                {
                    cSale.C_PRCbInsReferBill();
                    oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                    oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                    oSql.AppendLine("(FCXsdDis+FCXsdChg) AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                    oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd");
                    oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("ORDER BY FNXsdSeqNo");
                    oW_dtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                    if (oW_dtTmp != null)
                    {
                        if (oW_dtTmp.Rows.Count > 0)
                        {
                            ogdPdtStd.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //*Em 63-04-29
                            ogdOrder.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //*Em 63-04-29
                            ogdPdtStd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //*Em 63-04-29
                            ogdOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //*Em 63-04-29
                            
                            ogdPdtStd.Rows.Clear();
                            ogdPdtStd.Columns.Clear();
                            ogdPdtStd.DataSource = oW_dtTmp;
                            W_SETxColumn(ogdPdtStd);

                            ogdPdtStd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //*Em 63-04-29
                            ogdOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //*Em 63-04-29

                            ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                            cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                            olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                            cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                            olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                            cSale.nC_CntItem = ogdPdtStd.RowCount;

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
                new cSP().SP_CLExMemory();
            }
        }
        public void W_PRCxRefund()
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
                    cSale.C_PRCbInsRefund();

                    //oSql.AppendLine("SELECT FNXsdSeqNo,FTXsdBarCode,FTXsdPdtName,FCXsdQty,FTPunName,FCXsdSetPrice,FCXsdDis,FCXsdNet,FTPdtCode,FCXsdFactor,FTXsdStaPdt,FTXsdStaAlwDis");
                    //*Em 63-05-12
                    oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                    oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                    oSql.AppendLine("(FCXsdDis+FCXsdChg) AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                    oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd");
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
                            ogdPdtStd.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //*Em 63-04-29
                            ogdOrder.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //*Em 63-04-29
                            ogdPdtStd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //*Em 63-04-29
                            ogdOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //*Em 63-04-29

                            //ogdPdtStd.Rows.Clear();
                            //foreach (DataRow oRow in odtTmp.Rows)
                            //{
                            //    ogdPdtStd.Rows.Add(oRow["FNXsdSeqNo"],
                            //        oRow["FTXsdBarCode"],
                            //        oRow["FTXsdPdtName"],
                            //        oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdQty"], cVB.nVB_DecShow),
                            //        oRow["FTPunName"],
                            //        oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdSetPrice"], cVB.nVB_DecShow),
                            //        oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdDis"], cVB.nVB_DecShow),
                            //        oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdNet"], cVB.nVB_DecShow),
                            //        oRow["FTPdtCode"],
                            //        oRow["FCXsdFactor"],
                            //        oRow["FTXsdStaPdt"],
                            //        oRow["FTXsdStaAlwDis"]);
                            //}

                            ogdPdtStd.Rows.Clear();
                            ogdPdtStd.Columns.Clear();
                            ogdPdtStd.DataSource = odtTmp;
                            W_SETxColumn(ogdPdtStd);

                            ogdPdtStd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //*Em 63-04-29
                            ogdOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //*Em 63-04-29

                            ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                            cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                            olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                            cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                            olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                            cSale.nC_CntItem = ogdPdtStd.RowCount;

                        }
                    }

                    ////*Arm 63-05-20 - Comment Code
                    ////oSql.Clear();
                    ////oSql.AppendLine("SELECT FTCstCode FROM TPSTSalHD WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    ////+++++++++++++

                    ////*Arm 63-05-20 - เช็คกรณีข้อมูลมาจากหลังบ้านให้ Select ที่ตาราง Temp
                    //if (cVB.bVB_RefundTrans == true)
                    //{
                    //    //ข้อมูลจากเครื่อง
                    //    oSql.Clear();
                    //    oSql.AppendLine("SELECT FTCstCode FROM TPSTSalHD WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");

                    //}
                    //else
                    //{
                    //    //ข้อมูลจากหลังบ้านผ่าน API2ARDoc
                    //    oSql.Clear();
                    //    oSql.AppendLine("SELECT FTCstCode FROM TPSTSalHDTmp WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");

                    //}
                    ////+++++++++++++


                    //*Arm 63-06-01 Set Table ที่ต้องใช้ Query ตามเงื่อนไข
                    //string tTblSalHD ="";
                    //if (cVB.bVB_RefundDataFrom == true) // bVB_RefundDataFrom = ข้อมูลมาจาก True: ข้อมูลในเครื่อง, False: ข้อมูลการขายมาจากหลังบ้านโดย Call API2ARDoc
                    //{
                    //    // ถ้าเป็นข้อมูลการขายมาภายในเครื่องจุดขายเดิม
                    //    if (cVB.bVB_RefundTrans == true) // bVB_RefundTrans = true: ตาราง Transaction, false: ตาราง Temp
                    //    {
                    //        //ตาราง Transaction
                    //        tTblSalHD = "TPSTSalHD";
                    //    }
                    //    else
                    //    {
                    //        //ตาราง Temp
                    //        tTblSalHD = cSale.tC_TblSalHD;
                    //    }
                    //}
                    //else
                    //{
                    //    // ถ้าเป็นข้อมูลการขายข้ามเครื่องจุดขาย (Call API2ARDoc)
                    //    tTblSalHD = "TPSTSalHDTmp";
                    //}

                    oSql.Clear();
                    oSql.AppendLine("SELECT FTCstCode FROM " + cSale.tC_Ref_TblSalHD + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //++++++++++++++++

                    cVB.tVB_CstCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                    if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                    {
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
                new cSP().SP_CLExMemory();
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

                oSql.AppendLine("SELECT FNXsdSeqNo,FTXsdBarCode,FTXsdPdtName,FCXsdQty,FTPunName,FCXsdSetPrice,FCXsdDis,FCXsdNet,FTPdtCode,FCXsdFactor,FTXsdStaPdt,FTXsdStaAlwDis");
                oSql.AppendLine("FROM "+ cSale.tC_TblSalDT +" WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"'");
                oSql.AppendLine("AND FTXshDocNo = '"+ cVB.tVB_DocNo +"'");
                oSql.AppendLine("ORDER BY FNXsdSeqNo");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        ogdPdtStd.Rows.Clear();
                        foreach (DataRow oRow in odtTmp.Rows)
                        {
                            ogdPdtStd.Rows.Add(oRow["FNXsdSeqNo"],
                                oRow["FTXsdBarCode"],
                                oRow["FTXsdPdtName"],
                                oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdQty"], cVB.nVB_DecShow),
                                oRow["FTPunName"],
                                oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdSetPrice"], cVB.nVB_DecShow),
                                oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdDis"], cVB.nVB_DecShow),
                                oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdNet"], cVB.nVB_DecShow),
                                oRow["FTPdtCode"],
                                oRow["FCXsdFactor"],
                                oRow["FTXsdStaPdt"],
                                oRow["FTXsdStaAlwDis"]);
                        }

                        ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                        cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdPdtStd.RowCount;

                        // Select And Show Rows Last Update
                        ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Selected = true; // Select ข้อมูลล่าสุด 
                        ogdPdtStd.FirstDisplayedScrollingRowIndex = ogdPdtStd.Rows.Count - 1;   // Scrol ล่างสุด 
                    }
                }
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
                new cSP().SP_CLExMemory();
            }
        }

        public void W_PRCxLoadItemRetriveBill()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            decimal cSum = 0;
            decimal cQty = 0;
            try
            {
                //*Em 63-05-12
                oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                oSql.AppendLine("(FCXsdDis+FCXsdChg) AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd");
                //+++++++++++++
                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("ORDER BY FNXsdSeqNo");
                oW_dtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (oW_dtTmp != null)
                {
                    if (oW_dtTmp.Rows.Count > 0)
                    {
                        ogdPdtStd.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //*Em 63-04-29
                        ogdOrder.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //*Em 63-04-29
                        ogdPdtStd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //*Em 63-04-29
                        ogdOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //*Em 63-04-29

                        ogdPdtStd.Rows.Clear();
                        ogdPdtStd.Columns.Clear();
                        ogdPdtStd.DataSource = oW_dtTmp;
                        W_SETxColumn(ogdPdtStd);

                        ogdPdtStd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //*Em 63-04-29
                        ogdOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //*Em 63-04-29

                        ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                        cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdPdtStd.RowCount;

                    }
                }

                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(FTCstCode,'') AS FTCstCode FROM " + cSale.tC_TblSalHD +" WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                cVB.tVB_CstCode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                if (cVB.tVB_CstCode == null) cVB.tVB_CstCode = "";
                if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                {
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
                                new cLog().C_WRTxLog("wSale", "W_PRCxRefund/API Response Error : " + oCstSch.rtDesc);
                                break;
                        }

                        cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                        oCstSch = null;

                    }

                    oCall = null;
                    tJSonCall = null;
                    oReq = null;
                    oRep = null;
                    //+++++++++++++++

                    // Show ข้อมูลลูกค้า
                    W_SETxTextCst();


                    // แสดงข้อมูลลูกค้า
                    opnCst.Visible = true;
                    opnNoCst.Visible = false;


                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxRefund : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
            try
            {
                cVB.oVB_Sale = this;
                //cVB.oVB_PmtGetorSug = new wPmtGetorSug();
                //wPmtGetorSug oGetORSug = new wPmtGetorSug();

                if (cSale.nC_CntItem == 0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                    return;
                }
                if (ogdPdtStd.Rows.Count > 0 && olaTotalStd.Text == "0.00")
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                    return;
                }
                if (cVB.oVB_2ndScreen != null)
                {
                    cVB.oVB_2ndScreen.W_PRCxSaleToPay(olaTotal.Text);
                    cVB.oVB_2ndScreen.Update();
                }
                
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
                    cSale.C_PRCxSummary2HD();
                    new wPayment(3).ShowDialog();
                    return;
                    //this.Hide();
                }
                //++++++++++++++++

                //*Arm 63-04-10 กรณีใช้ใบสั่งขาย และไม่อนุญาตให้ คำนวณรายการใหม่ไม่ให้คำนวณ Promotion
                if (cVB.oVB_ReferSO != null && cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                {
                    cSale.C_PRCxSummary2HD();
                    new wPayment(3).ShowDialog();
                    //this.Hide();
                }
                else
                {
                    //new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : Start");
                    //W_PRCxCoPmt();
                    //new cLog().C_WRTxLog("wSale", "W_PRCxCoPmt : End");
                    if (cVB.oVB_GetPmt.Rows.Count > 0 || cVB.oVB_PmtSug.Rows.Count > 0)
                    {
                        //wPmtGetorSug oGetORSug = new wPmtGetorSug();
                        //oGetORSug.olaCashPayment.Text = olaTotalStd.Text;
                        //oGetORSug.olaCstName.Text = cVB.tVB_CstName;
                        //oGetORSug.ShowDialog();
                        //oGetORSug.Update();

                        //*Em 63-05-05
                        if (bW_CalPmtPrice)
                        {
                            wPmtPriConfirm oPmtPri = new wPmtPriConfirm();
                            oPmtPri.olaCashPayOld.Text = olaTotalStd.Text;
                            oPmtPri.olaCstName.Text = cVB.tVB_CstName;
                            oPmtPri.ShowDialog();
                        }
                        else
                        {
                            wPmtGetorSug oGetORSug = new wPmtGetorSug();
                            oGetORSug.olaCashPayment.Text = olaTotalStd.Text;
                            oGetORSug.olaCstName.Text = cVB.tVB_CstName;
                            oGetORSug.ShowDialog();
                            oGetORSug.Update();
                        }
                        //++++++++++++++++
                    }
                    else
                    {
                        cSale.C_PRCxSummary2HD();
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

                otbScan.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmPayment_Click " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        public void W_PRCxCoPmt()
        {
            if (bW_CalPmtPrice) //*Em 63-05-05 โปรโมชั่นกลุ่มราคา
            {
                new cPdtPmt().C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);
                new cPdtPmt().C_PRCoCalPmt("1");
                //new cPdtPmt().C_PRCbCalPmtSuggest();

                if (cVB.oVB_GetPmt.Rows.Count > 0)
                {
                    new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                    // C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                }
                else
                {
                    bW_CalPmtPrice = false;
                    W_PRCxCoPmt();
                }
            }
            else
            {
                new cPdtPmt().C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);
                new cPdtPmt().C_PRCoCalPmt("2");
                //new cPdtPmt().C_PRCbCalPmtSuggest();

                if (cVB.oVB_GetPmt.Rows.Count > 0)
                {
                    new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                    // C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                }
            }
            

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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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

                        cVB.nVB_MenuPerPage = (int)Math.Ceiling(Convert.ToDecimal(olaQtyStd.Height / 62)) * 2;
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
                new cSP().SP_CLExMemory();
            }
        }
        private void W_GETxButtonMenuBillStd(int pnStartRow, int pnEndRow)
        {
            try
            {
                if (pnEndRow > aW_oFunc.Count) pnEndRow = aW_oFunc.Count;

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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                            olaCashPayment.Text = "0.00";
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                switch (cVB.nVB_SaleModeStd)
                {
                    case 1:

                        cPdtQty = Convert.ToDecimal(ogdPdtStd.Rows[nSeq].Cells[otbColPdtQtyStd.Name].Value);
                        cSetPrice = Convert.ToDecimal(ogdPdtStd.Rows[nSeq].Cells[otbColSetPriceStd.Name].Value);
                        cSumPrice = cPdtQty * cSetPrice;

                        cSumPrice = (cSumPrice - pcDis) + pcChg;

                        if (oW_dtTmp != null) // *Arm 63-05-12 กรณี SO อนุญาตคำนวณใหม่
                        {
                            oW_dtTmp.Columns[otbColDiscount.Name].ReadOnly = false;
                            //oW_dtTmp.Rows[nSeq].SetField("otbColDiscount", Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(pcDis - pcChg), cVB.nVB_DecShow)));
                            //oW_dtTmp.Rows[nSeq].SetField("otbColPdtSumPriceStd", Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow)));
                            DataRow oRow = oW_dtTmp.Rows[nSeq];
                            oRow["otbColDiscount"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(pcDis - pcChg), cVB.nVB_DecShow));
                            oRow["otbColPdtSumPriceStd"] = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow));
                        }
                        else
                        {
                            // Discount
                            ogdPdtStd.Rows[nSeq].Cells[otbColDiscount.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(pcDis - pcChg), cVB.nVB_DecShow);
                            // SumPrice
                            ogdPdtStd.Rows[nSeq].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow);

                        }
                        //// Discount
                        //ogdPdtStd.Rows[nSeq].Cells[otbColDiscount.Name].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(pcDis - pcChg), cVB.nVB_DecShow);
                        //// SumPrice
                        //ogdPdtStd.Rows[nSeq].Cells[otbColPdtSumPriceStd.Name].Value = oW_SP.SP_SETtDecShwSve(1, cSumPrice, cVB.nVB_DecShow);


                        // Show Total Price
                        cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);

                        // Show Totol Qty
                        cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);

                        // total Item
                        cSale.nC_CntItem = ogdPdtStd.RowCount;

                        // Show Last Event
                        olaCashPayment.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtSumPriceStd.Name].Value.ToString();
                        olaTitleCashPay.Text = ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Cells[otbColPdtNameStd.Name].Value.ToString();

                        // Select And Show Rows Last Update
                        ogdPdtStd.Rows[cSale.nC_DTSeqNo - 1].Selected = true; // Select ข้อมูลล่าสุด 
                        ogdPdtStd.FirstDisplayedScrollingRowIndex = cSale.nC_DTSeqNo - 1; // Scrol 

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
                        olaCashPayment.Text = ogdOrder.Rows[cSale.nC_DTSeqNo - 1].Cells["olaPdtSumPrice"].Value.ToString();
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
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_PRCxOrder : " + oEx.Message);
            }
            finally
            {
                ogdOrder.ClearSelection();
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                        oImage = new Bitmap(tPath);
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                        oImage = new Bitmap(tPath);
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                W_GETxPdt(2, otbSearch.Text, ocbSearchBy.SelectedIndex);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmSearch_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
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

                W_SCHxSearchPdt(otbScan.Text, cW_SetQty);   //*Arm 63-05-04 
                otbScan.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmScan_Click : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process Scan
        /// </summary>
        private void W_PRCxScan()
        {
            List<cmlPdtDetail> aoPdt;
            cmlPdtOrder oOrder;

            try
            {
                switch (nW_Mode)
                {
                    case 5:     // Rental
                        aoPdt = new List<cmlPdtDetail>();
                        break;

                    default:     // Sale
                        //aoPdt = new cPdt().C_GETaPdtSale(otbScan.Text, 1, 0, nW_StartRow);
                        // Zen 63-03-12 Comment ^ และ เพิ่ม Code ดึงข้อมูลใหม่
                        aoPdt = new cPdt().C_GETaPdt(otbScan.Text);

                        break;
                }

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
                            wEditSalePrice owEditSalePrice = new wEditSalePrice(oOrder.tPdtCode, oOrder.tPdtName, aoPdt[0].cPdtPrice);
                            owEditSalePrice.ShowDialog();
                            if (owEditSalePrice.DialogResult == DialogResult.OK)
                            {
                                oOrder.cSetPrice = owEditSalePrice.rcPrice;
                            }
                        } //*Arm 62-09-23
                    }

                    if(cStock.C_CHKbSalStock(ref oOrder)) //*Net 63-05-22
                    W_ADDxPdtToOrder(oOrder);

                }
                else if (aoPdt.Count > 1)
                {
                    W_SCHxSearchPdt(otbScan.Text, cW_SetQty); //*Arm 63-02-28
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
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_PRCxScan : " + oEx.Message); }
            finally
            {
                otbScan.Clear();
                otbScan.Focus();
                cW_SetQty = 1; //*Arm 63-05-03 กำหนดจำนวนสินค้าเริ่มต้นกลับเป็น 1 สำหรับรายการต่อไป
                aoPdt = null;
                oOrder = null;
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                    if (cVB.oVB_ReferSO != null && cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1") //*Arm 63-05-13
                    {
                        return;
                    }

                    W_PRCxScan();
                }

            //}
            //catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "otbScan_KeyDown : " + oEx.Message); }
            //finally
            //{
            //    new cSP().SP_CLExMemory();
            //}
        }

        private void wSale_Activated(object sender, EventArgs e)
        {
            new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
            //DataTable odtTmp;
            decimal cSum = 0;
            decimal cQty = 0;
            
            //string tScanNotPdt = "";
            //int nCountNotFound = 0;
            //cmlTPSTSalDTDis oSalDTDis;
            try
            {
                

                olaSO.Text = cVB.tVB_RefDocNo; //Show SO DocNo.

                //*Arm 63-05-11 ปรับ Process SO
                oSql = new StringBuilder();
                oDB = new cDatabase();
                //odtTmp = new DataTable();
                oW_dtTmp = new DataTable();


                if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo =="1")
                {
                    new cSale().C_PRCbInsSO(cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo);
                }
                else
                {
                    new cSale().C_PRCbInsSO();
                    
                }
                
                new cLog().C_WRTxLog("wSale", "Select DT For Add to GridView : Strat... ");
                Cursor.Current = Cursors.WaitCursor;

                oSql.AppendLine("SELECT FNXsdSeqNo AS otbColSeqStd,FTXsdBarCode AS otbColBarcodeStd,FTXsdPdtName AS otbColPdtNameStd,");
                oSql.AppendLine("FCXsdQty AS otbColPdtQtyStd,FTPunName AS otbColUnitNameStd,FCXsdSetPrice AS otbColSetPriceStd,");
                oSql.AppendLine("(FCXsdDis-FCXsdChg) AS otbColDiscount,FCXsdNet AS otbColPdtSumPriceStd,FTPdtCode AS otbColPdtCodeStd,");
                oSql.AppendLine("FCXsdFactor AS otbColFactorStd,FTXsdStaPdt AS otbColStaPdtStd,FTXsdStaAlwDis AS otbColAlwDisStd");
                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("ORDER BY FNXsdSeqNo");
                //odtTmp = null;
                //odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                oW_dtTmp = null;
                oW_dtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                new cLog().C_WRTxLog("wSale", "Select DT For Add to GridView : End... ");

                new cLog().C_WRTxLog("wSale", "Add DT to GridView : Strat... ");
                //if (odtTmp != null)
                if (oW_dtTmp != null)
                {
                    //if (odtTmp.Rows.Count > 0)
                    if (oW_dtTmp.Rows.Count > 0)
                    {
                        ogdPdtStd.Rows.Clear();
                        ogdPdtStd.Columns.Clear();
                        ogdPdtStd.DataSource = oW_dtTmp;
                        W_SETxColumn(ogdPdtStd);

                        new cLog().C_WRTxLog("wSale", "Add DT to GridView : End... ");

                        new cLog().C_WRTxLog("wSale", "Sum GridView : Start... ");
                        ogdPdtStd.CurrentCell = ogdPdtStd.Rows[ogdPdtStd.RowCount - 1].Cells[0];

                        cSum = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oSum => Convert.ToDecimal(oSum.Cells[otbColPdtSumPriceStd.Name].Value));
                        olaTotalStd.Text = oW_SP.SP_SETtDecShwSve(1, cSum, cVB.nVB_DecShow);
                        cQty = ogdPdtStd.Rows.Cast<DataGridViewRow>().Sum(oQty => Convert.ToDecimal(oQty.Cells[otbColPdtQtyStd.Name].Value));
                        olaTotalQtyStd.Text = oW_SP.SP_SETtDecShwSve(1, cQty, cVB.nVB_DecShow);
                        cSale.nC_CntItem = ogdPdtStd.RowCount;

                        new cLog().C_WRTxLog("wSale", "Sum GridView : End... ");
                    }
                }
                
                if(cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                {
                    new cLog().C_WRTxLog("wSale", "Close Button Function : Start... ");
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
                    new cLog().C_WRTxLog("wSale", "Close Button Function : End... ");
                }
                Cursor.Current = Cursors.Default;

                if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo == "1") //*Arm 63-05-13
                {
                    bW_CalPmtPrice = true;  
                    Thread oCalPro = new Thread(W_PRCxCoPmt);
                    oCalPro.IsBackground = true;
                    oCalPro.Priority = ThreadPriority.Highest;
                    oCalPro.Start();
                }
                
                //++++++++++++++



                //olaSO.Text = cVB.tVB_RefDocNo; //Show SO DocNo.

                ////*Arm 63-03-30 แก้ไข Check  rtXshStaAlwPosCalSo ใหม่
                //if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo == "1")
                //{
                //    nW_ScanSO = 1; //เปลี่ยน Mode Scan SO

                //    if (cVB.aoVB_PdtReferSO.Count > 0)
                //    {
                //        foreach (cmlPdtOrder oPdt in cVB.aoVB_PdtReferSO)   // Process Scan Barcode ที่ละตัว
                //        {
                //            for (int nNum = 1; nNum <= oPdt.cQty; nNum++)
                //            {
                //                otbScan.Text = oPdt.tBarcode;
                //                cW_SetQty = 1; //*Arm 63-05-03 กำหนดจำนวนสินค้าเริ่มต้นกลับเป็น 1 สำหรับรายการต่อไป
                //                W_PRCxScan();

                //                if (bW_ResultScanSO == false)  //Scan ไม่พบสินค้า
                //                {
                //                    nCountNotFound++;
                //                    tScanNotPdt += oPdt.tBarcode + " " + oPdt.tPdtName;
                //                    tScanNotPdt += Environment.NewLine;
                //                }
                //                bW_ResultScanSO = true;
                //            }
                //        }
                //    }

                //    if (nCountNotFound > 0) // แสดง Msg รายการที่ไม่พบสินค้า
                //    {
                //        string tMsg = oW_Resource.GetString("tMsgNotFoundPdt");
                //        tMsg += Environment.NewLine;
                //        tMsg += tScanNotPdt;
                //        oW_SP.SP_SHWxMsg(tMsg, 3);
                //    }

                //    nW_ScanSO = 0; //เปลี่ยน Mode Scan เป็นปกติ
                //    otbScan.Focus();

                //}
                //else
                //{
                //    if (cVB.aoVB_PdtReferSO.Count > 0)
                //    {
                //        foreach (cmlPdtOrder oPdt in cVB.aoVB_PdtReferSO)
                //        {
                //            W_ADDxPdtToOrder(oPdt); // Process โหลดข้อมูลลงตาราง

                //            switch (cVB.nVB_SaleModeStd)   // Check Mode 1:Standrad, 2:TouchScreen
                //            {
                //                case 1:
                //                    //ไม่อนุญาตให้เลือก Menu บางรายการ
                //                    W_GETxButtonMenuBillStd(0, cVB.nVB_MenuPerPage);
                //                    break;

                //                case 2:
                //                    opnPdt.Enabled = false; //ไม่อนุญาตให้เลือกสินค้าเพิ่ม
                //                    break;
                //            }
                //        }

                //        // ทำ Process Dis/Chg Item
                //        if (cVB.oVB_ReferSO.aoTARTSoDTDis.Count > 0)
                //        {
                //            foreach (cmlResInfoTARTSoDTDis oSoDTDis in cVB.oVB_ReferSO.aoTARTSoDTDis)
                //            {
                //                if (oSoDTDis.rnXddStaDis == 1)  // Dis/Chg Item
                //                {
                //                    cSale.nC_DTSeqNo = (int)oSoDTDis.rnXsdSeqNo;
                //                    cVB.oVB_OrderRowIndex = cSale.nC_DTSeqNo - 1;

                //                    StringBuilder oSql = new StringBuilder();

                //                    // มูลค่าสุทธิก่อนลดชาร์จ
                //                    string tB4DisChg = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cVB.oVB_OrderRowIndex].Cells[otbColPdtSumPriceStd.Name].Value), cVB.nVB_DecShow);

                //                    oSalDTDis = new cmlTPSTSalDTDis();
                //                    oSalDTDis.FTBchCode = oSoDTDis.rtBchCode;
                //                    oSalDTDis.FTXshDocNo = cVB.tVB_DocNo;
                //                    oSalDTDis.FNXsdSeqNo = oSoDTDis.rnXsdSeqNo;
                //                    oSalDTDis.FDXddDateIns = oSoDTDis.rdXddDateIns;
                //                    oSalDTDis.FNXddStaDis = oSoDTDis.rnXddStaDis;
                //                    oSalDTDis.FTXddDisChgTxt = oSoDTDis.rtXddDisChgTxt; //*Arm 63-04-10
                //                    //oSalDTDis.FTXddDisChgTxt = oW_SP.SP_SETtDecShwSve(1, (decimal)oSoDTDis.rcXddValue, cVB.nVB_DecShow).ToString();
                //                    oSalDTDis.FTXddDisChgType = oSoDTDis.rtXddDisChgType;
                //                    oSalDTDis.FCXddNet = Convert.ToDecimal(tB4DisChg);
                //                    oSalDTDis.FCXddValue = oSoDTDis.rcXddValue;
                //                    new cSale().C_PRCxDisChgItem(oSalDTDis);

                //                }

                //            }
                //        }

                //    }
                //}
                //+++++++++++++

                //*Arm 63-03-30 Comment Code
                //if (cVB.tVB_CstStaAlwPosCalSo == "1")  //อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต, 2:ไม่อนุญาต(default)
                //{
                //    nW_ScanSO = 1; //เปลี่ยน Mode Scan SO

                //    if (cVB.aoVB_PdtReferSO.Count > 0)
                //    {
                //        foreach (cmlPdtOrder oPdt in cVB.aoVB_PdtReferSO)   // Process Scan Barcode ที่ละตัว
                //        {
                //            for (int nNum = 1; nNum <= oPdt.cQty; nNum++)
                //            {
                //                otbScan.Text = oPdt.tBarcode;
                //                W_PRCxScan();

                //                if (bW_ResultScanSO == false)  //Scan ไม่พบสินค้า
                //                {
                //                    nCountNotFound++;
                //                    tScanNotPdt += oPdt.tBarcode + " " + oPdt.tPdtName;
                //                    tScanNotPdt += Environment.NewLine;
                //                }
                //                bW_ResultScanSO = true;
                //            }
                //        }
                //    }

                //    if (nCountNotFound > 0) // แสดง Msg รายการที่ไม่พบสินค้า
                //    {
                //        string tMsg = oW_Resource.GetString("tMsgNotFoundPdt");
                //        tMsg += Environment.NewLine;
                //        tMsg += tScanNotPdt;
                //        oW_SP.SP_SHWxMsg(tMsg, 3);
                //    }

                //    nW_ScanSO = 0; //เปลี่ยน Mode Scan เป็นปกติ
                //    otbScan.Focus();
                //}
                //else
                //{
                //    if (cVB.aoVB_PdtReferSO.Count > 0)
                //    {
                //        foreach (cmlPdtOrder oPdt in cVB.aoVB_PdtReferSO)
                //        {
                //            W_ADDxPdtToOrder(oPdt); // Process โหลดข้อมูลลงตาราง

                //            switch (cVB.nVB_SaleModeStd)   // Check Mode 1:Standrad, 2:TouchScreen
                //            {
                //                case 1:
                //                    //ไม่อนุญาตให้เลือก Menu บางรายการ
                //                    W_GETxButtonMenuBillStd(0, cVB.nVB_MenuPerPage);
                //                    break;

                //                case 2:
                //                    opnPdt.Enabled = false; //ไม่อนุญาตให้เลือกสินค้าเพิ่ม
                //                    break;
                //            }
                //        }

                //        // ทำ Process Dis/Chg Item
                //        if (cVB.oVB_ReferSO.aoTARTSoDTDis.Count > 0)
                //        {
                //            foreach (cmlResInfoTARTSoDTDis oSoDTDis in cVB.oVB_ReferSO.aoTARTSoDTDis)
                //            {
                //                if (oSoDTDis.rnXddStaDis == 1)  // Dis/Chg Item
                //                {
                //                    cSale.nC_DTSeqNo = (int)oSoDTDis.rnXsdSeqNo;
                //                    cVB.oVB_OrderRowIndex = cSale.nC_DTSeqNo - 1;

                //                    StringBuilder oSql = new StringBuilder();

                //                    // มูลค่าสุทธิก่อนลดชาร์จ
                //                    string tB4DisChg = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(ogdPdtStd.Rows[cVB.oVB_OrderRowIndex].Cells[otbColPdtSumPriceStd.Name].Value), cVB.nVB_DecShow);

                //                    oSalDTDis = new cmlTPSTSalDTDis();
                //                    oSalDTDis.FTBchCode = oSoDTDis.rtBchCode;
                //                    oSalDTDis.FTXshDocNo = cVB.tVB_DocNo;
                //                    oSalDTDis.FNXsdSeqNo = oSoDTDis.rnXsdSeqNo;
                //                    oSalDTDis.FDXddDateIns = oSoDTDis.rdXddDateIns;
                //                    oSalDTDis.FNXddStaDis = oSoDTDis.rnXddStaDis;
                //                    oSalDTDis.FTXddDisChgTxt = oW_SP.SP_SETtDecShwSve(1, (decimal)oSoDTDis.rcXddValue, cVB.nVB_DecShow).ToString();
                //                    oSalDTDis.FTXddDisChgType = oSoDTDis.rtXddDisChgType;
                //                    oSalDTDis.FCXddNet = Convert.ToDecimal(tB4DisChg);
                //                    oSalDTDis.FCXddValue = oSoDTDis.rcXddValue;
                //                    new cSale().C_PRCxDisChgItem(oSalDTDis);

                //                }

                //            }
                //        }

                //    }

                //}

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_DATxLoadSO2Order " + oEx.Message);
            }
            finally
            {
                cVB.aoVB_PdtReferSO = null;
                new cSP().SP_CLExMemory();
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
                                olaCashPayment.Text = "0.00";
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
                new cSP().SP_CLExMemory();
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
                object oSender = new object();
                EventArgs oEv = new EventArgs();

                if (ptFuncName == "C_KBDxVoidItem" || ptFuncName == "C_KBDxPdtQty" || ptFuncName == "C_KBDxChgAmt" || ptFuncName == "C_KBDxChgPer" ||
                    ptFuncName == "C_KBDxDisAmt" || ptFuncName == "C_KBDxDisPer" || ptFuncName == "C_KBDxItemFree" || ptFuncName == "C_KBDxPdtRmk")
                {

                    if (cSale.nC_CntItem > 0)
                    {
                        
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
                                    if (ogdPdtStd.Rows.Count == 0)
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
                        //*Arm 63-02-28 -Call Function เปิดฟอร์มค้นหาสินค้า
                        W_SCHxSearchPdt(""); 

                        break;

                    //*Arm 63-03-02 Menu bill
                    case "C_KBDxItemFree":
                        oChooseItem = new wChooseItemRef(1, "C_KBDxItemFree");
                        oChooseItem.ShowDialog();
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                    case "C_KBDxPdtQty":
                        oChooseItem = new wChooseItemRef(1, "C_KBDxPdtQty");
                        oChooseItem.ShowDialog();

                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                    case "C_KBDxChgAmt":
                        oChooseItem = new wChooseItemRef(1, "C_KBDxChgAmt");
                        oChooseItem.ShowDialog();
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                    case "C_KBDxChgPer":
                        oChooseItem = new wChooseItemRef(1, "C_KBDxChgPer");
                        oChooseItem.ShowDialog();
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;
                    case "C_KBDxDisAmt":
                        oChooseItem = new wChooseItemRef(1, "C_KBDxChgAmt");
                        oChooseItem.ShowDialog();
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                    case "C_KBDxDisPer":
                        oChooseItem = new wChooseItemRef(1, "C_KBDxDisPer");
                        oChooseItem.ShowDialog();
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                    case "C_KBDxVoidItem":
                        oChooseItem = new wChooseItemRef(1, "C_KBDxVoidItem");
                        oChooseItem.ShowDialog();
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                    case "C_KBDxPdtRmk":
                        oChooseItem = new wChooseItemRef(1, "C_KBDxPdtRmk");
                        oChooseItem.ShowDialog();
                        if (cSale.nC_DTSeqNo == 0)
                        {
                            return;
                        }
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
                                new cLog().C_WRTxLog("wReferSO", "Call API2PSMaster ==> Search Customer Start..."); //*Arm 63-05-11
                                new cFunctionKeyboard().C_PRCxCallByName("C_KBDxCustomer");
                                new cLog().C_WRTxLog("wReferSO", "Call API2PSMaster ==> Search Customer End..."); //*Arm 63-05-11
                                if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                                {
                                    new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                                }
                            }
                            //++++++++++++++
                            ///new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        }
                        break;

                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                new cSP().SP_CLExMemory();
            }
        }
        /// <summary>
        /// *Arm 63-02-28
        /// Function เปิดฟอร์มค้นหาสินค้า
        /// </summary>
        /// <param name="ptValue"></param>
        public void W_SCHxSearchPdt(string ptValue = "", decimal pcSetQty = 1)
        {
            //wSearchPdt oSchPdt = new wSearchPdt(ptValue);   //*Arm 63-05-04 Comment Code
            wSearchPdt oSchPdt = new wSearchPdt(ptValue, pcSetQty);     //*Arm 63-05-04
            if (oSchPdt.ShowDialog() == DialogResult.OK)
            {
                if (cVB.oVB_PdtOrder.tSaleType == "2")
                {
                    wEditSalePrice owEditSalePrice = new wEditSalePrice(cVB.oVB_PdtOrder.tPdtCode, cVB.oVB_PdtOrder.tPdtName, cVB.oVB_PdtOrder.cSetPrice);
                    owEditSalePrice.ShowDialog();
                    if (owEditSalePrice.DialogResult == DialogResult.OK)
                    {
                        cVB.oVB_PdtOrder.cSetPrice = owEditSalePrice.rcPrice;
                    }
                }

                if (cStock.C_CHKbSalStock(ref cVB.oVB_PdtOrder)) //*Net 63-05-22
                    W_ADDxPdtToOrder(cVB.oVB_PdtOrder);
            }
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
            //    new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// *Arm 63-05-11
        /// Add Colunm Gridview
        /// </summary>
        /// <param name="poGridView"></param>
        private void W_ADDxColumn(DataGridView poGridView)
        {
            try
            {
                if(poGridView.ColumnCount > 0)
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
        /// Set Colounm DataGridview
        /// </summary>
        /// <param name="poGridView"></param>
        private void W_SETxColumn(DataGridView poGridView)
        {
            try
            {
                switch(poGridView.Name)
                {
                    case "ogdPdtStd":

                        oW_SP.SP_SETxSetGridviewFormat(poGridView);

                        poGridView.Columns[otbColSeqStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tSeq");
                        poGridView.Columns[otbColBarcodeStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tBarcode");
                        poGridView.Columns[otbColPdtNameStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tPdtName");
                        poGridView.Columns[otbColPdtQtyStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tQty");
                        poGridView.Columns[otbColUnitNameStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tUnit");
                        poGridView.Columns[otbColSetPriceStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tPrice");
                        poGridView.Columns[otbColDiscount.Name].HeaderText = cVB.oVB_GBResource.GetString("tDis");
                        poGridView.Columns[otbColPdtSumPriceStd.Name].HeaderText = cVB.oVB_GBResource.GetString("tSummary");

                        poGridView.Columns[otbColFactorStd.Name].Visible = false;
                        poGridView.Columns[otbColPdtCodeStd.Name].Visible = false;
                        poGridView.Columns[otbColStaPdtStd.Name].Visible = false;
                        poGridView.Columns[otbColAlwDisStd.Name].Visible = false;
                        
                        poGridView.Columns[otbColSeqStd.Name].FillWeight = 35;
                        poGridView.Columns[otbColBarcodeStd.Name].FillWeight = 100;
                        poGridView.Columns[otbColPdtNameStd.Name].FillWeight = 150;
                        poGridView.Columns[otbColPdtQtyStd.Name].FillWeight = 50;
                        poGridView.Columns[otbColUnitNameStd.Name].FillWeight = 70;
                        poGridView.Columns[otbColSetPriceStd.Name].FillWeight = 50;
                        poGridView.Columns[otbColDiscount.Name].FillWeight = 50;
                        poGridView.Columns[otbColPdtSumPriceStd.Name].FillWeight = 50;

                        poGridView.Columns[otbColSeqStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        poGridView.Columns[otbColBarcodeStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        poGridView.Columns[otbColPdtNameStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        poGridView.Columns[otbColPdtQtyStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        poGridView.Columns[otbColUnitNameStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        poGridView.Columns[otbColSetPriceStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        poGridView.Columns[otbColDiscount.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        poGridView.Columns[otbColPdtSumPriceStd.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        
                        break;
                }
                
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_SETxColumn : " + oEx.Message);
            }
        }

        private void OgdPdtStd_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {



        }
    }
}
