using AdaPos.Class;
using AdaPos.Popup.wProduct;
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

namespace AdaPos
{
    public partial class wPdtAddOrEdit : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        #endregion End Variable

        public wPdtAddOrEdit()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();
                //*Net 63-07-31 ปรับตาม Moshi
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "wPdtAddOrEdit : " + oEx.Message); }
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
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                ocmInfo.BackColor = cVB.oVB_ColDark;
                olaLineInfo.BackColor = cVB.oVB_ColDark;

                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;

                // Information
                ocmGenInfCode.BackColor = cVB.oVB_ColNormal;
                ocmLanguage.BackColor = cVB.oVB_ColNormal;
                ocmAcceptPdt.BackColor = cVB.oVB_ColNormal;
                ocmSchInfPdtGrp.BackColor = cVB.oVB_ColNormal;
                ocmSchInfTchGrp.BackColor = cVB.oVB_ColNormal;
                ocmSchInfVat.BackColor = cVB.oVB_ColNormal;
                //ogdUnit.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                //ogdBarcode.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                //ogdSupplier.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdUnit); //*Net 63-03-03 Set Design Gridview
                oW_SP.SP_SETxSetGridviewFormat(ogdBarcode); //*Net 63-03-03 Set Design Gridview
                oW_SP.SP_SETxSetGridviewFormat(ogdSupplier); //*Net 63-03-03 Set Design Gridview
                ocmAddBarcode.BackColor = cVB.oVB_ColNormal;
                ocmAddUnit.BackColor = cVB.oVB_ColNormal;
                ocmAddSupplier.BackColor = cVB.oVB_ColNormal;

                // Information 2
                ocmSchInfType.BackColor = cVB.oVB_ColNormal;
                ocmSchInfBrand.BackColor = cVB.oVB_ColNormal;
                ocmSchInfModel.BackColor = cVB.oVB_ColNormal;
                ocmSchInfEvn.BackColor = cVB.oVB_ColNormal;
                ocmSchInfShop.BackColor = cVB.oVB_ColNormal;

                // Fashion
                ocmSchFhnClass.BackColor = cVB.oVB_ColNormal;
                ocmSchFhnColor.BackColor = cVB.oVB_ColNormal;
                ocmSchFhnDcs.BackColor = cVB.oVB_ColNormal;
                ocmSchFhnDep.BackColor = cVB.oVB_ColNormal;
                ocmSchFhnSize.BackColor = cVB.oVB_ColNormal;
                ocmSchFhnSsn.BackColor = cVB.oVB_ColNormal;
                ocmSchFhnSubClass.BackColor = cVB.oVB_ColNormal;

                opbPdtAddEdit.Tag = "Product";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resProduct_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resProduct_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "PRODUCT";

                // Information
                olaPdt.Text = oW_Resource.GetString("tProduct");
                ocmInfo.Text = oW_Resource.GetString("tInformation");
                ocmInfo2.Text = oW_Resource.GetString("tInformation") + "2";
                ocmAge.Text = oW_Resource.GetString("tAge");
                ocmFashion.Text = oW_Resource.GetString("tFashion");
                ocmRental.Text = oW_Resource.GetString("tRental");
                olaTitleInfPdtType.Text = oW_Resource.GetString("tPdtType");
                ockControlSpc.Text = oW_Resource.GetString("tControlSpc");
                olaTitleInfCode.Text = oW_Resource.GetString("tCode");
                olaTitleInfName.Text = oW_Resource.GetString("tName");
                olaTitleInfOthName.Text = oW_Resource.GetString("tOthName");
                olaTitleInfNameABB.Text = oW_Resource.GetString("tNameABB");
                olaTitleInfStkFac.Text = oW_Resource.GetString("tStkFac");
                olaTitleInfVat.Text = oW_Resource.GetString("tVat");
                olaTitleInfPdtGrp.Text = oW_Resource.GetString("tPdtGrp");
                olaTitleInfTchGrp.Text = oW_Resource.GetString("tTchGrp");
                olaTitleInfSaleType.Text = oW_Resource.GetString("tSaleType");
                olaTitleInfPntTime.Text = oW_Resource.GetString("tPntTime");
                ockInfVat.Text = oW_Resource.GetString("tVat");
                ockInfStkCtl.Text = oW_Resource.GetString("tStkCtl");
                ockInfPoint.Text = oW_Resource.GetString("tPoint");
                ockInfReturn.Text = oW_Resource.GetString("tReturn");
                ockInfActive.Text = oW_Resource.GetString("tActive");
                ockInfDisChg.Text = oW_Resource.GetString("tDisChg");
                ockInfSetShwDT.Text = oW_Resource.GetString("tSetShwDt");
                ockInfCsm.Text = oW_Resource.GetString("tCsm");
                olaTitleInfUnit.Text = oW_Resource.GetString("tUnit");
                olaTitleInfBarcode.Text = oW_Resource.GetString("tBarcode");
                olaTitleSupplier.Text = oW_Resource.GetString("tSupplier");
                otbTitleUnit.HeaderText = oW_Resource.GetString("tUnit");
                otbTitleFactor.HeaderText = oW_Resource.GetString("tFactor");
                otbTitleRetPri.HeaderText = oW_Resource.GetString("tRetpri");
                otbTitleWhsPri.HeaderText = oW_Resource.GetString("tWhsPri");
                otbTitleOlnPri.HeaderText = oW_Resource.GetString("tOlnPri");
                otbTitleBarcode.HeaderText = oW_Resource.GetString("tBarcode");
                otbTitleSupplier.HeaderText = oW_Resource.GetString("tSupplier");

                // Product Type 1:สินค้าทั่วไป 2:สินค้าบริการ 3:สินค้าอื่นๆ 4:ของแถม 5:พิเศษ
                ocbInfPdtType.Items.Add(oW_Resource.GetString("tGeneral"));
                ocbInfPdtType.Items.Add(oW_Resource.GetString("tService"));
                ocbInfPdtType.Items.Add(oW_Resource.GetString("tOther"));
                ocbInfPdtType.Items.Add(oW_Resource.GetString("tGift"));
                ocbInfPdtType.Items.Add(oW_Resource.GetString("tSpecial"));
                ocbInfPdtType.SelectedIndex = 0;

                // Sale Type  1:บังคับ, 2:แก้ไข, 3:เครื่องชั่ง, 4:น้ำหนัก
                ocbInfSaleType.Items.Add(oW_Resource.GetString("tSalePrice"));
                ocbInfSaleType.Items.Add(oW_Resource.GetString("tOpenPLU"));
                ocbInfSaleType.Items.Add(oW_Resource.GetString("tScales"));
                ocbInfSaleType.Items.Add(oW_Resource.GetString("tWeight"));
                ocbInfSaleType.SelectedIndex = 0;

                // Detail
                olaTitleDetName.Text = oW_Resource.GetString("tName");
                olaTitleDetOthName.Text = oW_Resource.GetString("tOthName");
                olaTitleDetNameABB.Text = oW_Resource.GetString("tNameABB");
                olaTitleDetStkFac.Text = oW_Resource.GetString("tStkFac");
                olaTitleDetVat.Text = oW_Resource.GetString("tVat");
                olaTitleDetPdtGrp.Text = oW_Resource.GetString("tPdtGrp");
                olaTitleDetTchGrp.Text = oW_Resource.GetString("tTchGrp");
                olaTitleDetPdtType.Text = oW_Resource.GetString("tPdtType");
                olaTitleDetSaleType.Text = oW_Resource.GetString("tSaleType");

                // Information 2
                olaTitleInfType.Text = oW_Resource.GetString("tType");
                olaTitleInfBrand.Text = oW_Resource.GetString("tBrand");
                olaTitleInfModel.Text = oW_Resource.GetString("tModel");
                olaTitleInfEvn.Text = oW_Resource.GetString("tEvent");
                olaTitleInfShop.Text = oW_Resource.GetString("tShop");
                olaTitleInfRefShop.Text = oW_Resource.GetString("tRefShop");
                olaTitleInfCostAvg.Text = oW_Resource.GetString("tCostAvg");
                olaTitleInfCostFiFo.Text = oW_Resource.GetString("tCostFiFo");
                olaTitleInfCostLst.Text = oW_Resource.GetString("tCostLst");
                olaTitleInfCostDfn.Text = oW_Resource.GetString("tCostDfn");
                olaTitleInfCostOth.Text = oW_Resource.GetString("tCostOth");
                olaTitleInfCostAmt.Text = oW_Resource.GetString("tCostAmt");
                olaTitleInfCostStd.Text = oW_Resource.GetString("tCostStd");
                olaTitleInfMin.Text = oW_Resource.GetString("tMin");
                olaTitleInfMax.Text = oW_Resource.GetString("tMax");
                olaTitleInfSetOrSN.Text = oW_Resource.GetString("tSetOrSN");
                olaTitleInfSetPriSta.Text = oW_Resource.GetString("tSetPriSta");
                olaTitleInfCal.Text = oW_Resource.GetString("tCal");
                olaTitleInfQtyOrdBuy.Text = oW_Resource.GetString("tQtyOrdBuy");
                olaTitleInfSaleStart.Text = oW_Resource.GetString("tSaleStart");
                olaTitleInfSaleStop.Text = oW_Resource.GetString("tSaleStop");
                olaTitleInfRmk.Text = oW_Resource.GetString("tRemark");

                // Set or Serial 1:สินค้าปกติ 2:สินค้าปกติชุด 3: สินค้าSerial 4:สินค้าSerial Set
                ocbInfSetOrSN.Items.Add(oW_Resource.GetString("tProductN"));
                ocbInfSetOrSN.Items.Add(oW_Resource.GetString("tProductSet"));
                ocbInfSetOrSN.Items.Add(oW_Resource.GetString("tSerial"));
                ocbInfSetOrSN.Items.Add(oW_Resource.GetString("tSerialSet"));
                ocbInfSetOrSN.SelectedIndex = 0;

                // Set Price Status 1:ใช้ราคาชุด, 2:ใช้ราคารายการย่อย
                ocbInfSetPriSta.Items.Add(oW_Resource.GetString("tSetPrice"));
                ocbInfSetPriSta.Items.Add(oW_Resource.GetString("tSumPrice"));
                ocbInfSetPriSta.SelectedIndex = 0;

                // Allow Calculation  1:Repack 2:Step Price 3:ไม่เปิด
                ocbInfCal.Items.Add(oW_Resource.GetString("tRepack"));
                ocbInfCal.Items.Add(oW_Resource.GetString("tStepPrice"));
                ocbInfCal.Items.Add(oW_Resource.GetString("tNotAllowAll"));
                ocbInfCal.SelectedIndex = 0;

                // Age
                olaTitleAgeMfg.Text = oW_Resource.GetString("tMfg");
                olaTitleAgeExp.Text = cVB.oVB_GBResource.GetString("tExpire");
                olaTitleAge.Text = oW_Resource.GetString("tAge");
                olaTiltleAgeMaxHeat.Text = oW_Resource.GetString("tMaxHeart");

                // Fashion
                olaTitleFhnSsn.Text = oW_Resource.GetString("tSeason");
                olaTitleFhnDcs.Text = oW_Resource.GetString("tDcs");
                olaTitleFhnColor.Text = oW_Resource.GetString("tColor");
                olaTitleFhnDep.Text = oW_Resource.GetString("tDepart");
                olaTitleFhnSize.Text = oW_Resource.GetString("tSize");
                olaTitleFhnArticle.Text = oW_Resource.GetString("tArticle");
                olaTitleFhnClass.Text = oW_Resource.GetString("tClass");
                olaTitleFhnSubClass.Text = oW_Resource.GetString("tSubClass");

                // Rental
                olaTitleRtlType.Text = oW_Resource.GetString("tType");
                olaTitleRtlTime.Text = oW_Resource.GetString("tTime");
                olaTitleRental.Text = oW_Resource.GetString("tRental");
                olaTitleRtlDeposit.Text = oW_Resource.GetString("tDeposit");
                olaTitleRtlFee.Text = oW_Resource.GetString("tFee");
                ockReqRet.Text = oW_Resource.GetString("tRequestReturn");

                // Rental Type 1:ไม่ระบุ 2:รายชั่วโมง 3:รายวัน 4:รายเดือน 5:รายปี 6: Custom(days)
                ocbRtlType.Items.Add(oW_Resource.GetString("tUnknown"));
                ocbRtlType.Items.Add(oW_Resource.GetString("tHourly"));
                ocbRtlType.Items.Add(oW_Resource.GetString("tDaily"));
                ocbRtlType.Items.Add(oW_Resource.GetString("tMonthly"));
                ocbRtlType.Items.Add(oW_Resource.GetString("tAnnual"));
                ocbRtlType.Items.Add(oW_Resource.GetString("tCustomDay"));
                ocbRtlType.SelectedIndex = 0;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Add Price / Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAddPrice_Click(object sender, EventArgs e)
        {
            try
            {
                new wPdtUnit().ShowDialog();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmAddPrice_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Information 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmInfo2_Click(object sender, EventArgs e)
        {
            try
            {
                if(ocmInfo2.BackColor == Color.Silver)
                {
                    ocmInfo2.BackColor = cVB.oVB_ColDark;
                    olaLineInfo2.BackColor = cVB.oVB_ColDark;

                    ocmInfo.BackColor = Color.Silver;
                    olaLineInfo.BackColor = Color.Silver;
                    ocmAge.BackColor = Color.Silver;
                    olaLineAge.BackColor = Color.Silver;
                    ocmFashion.BackColor = Color.Silver;
                    olaLineFashion.BackColor = Color.Silver;
                    ocmRental.BackColor = Color.Silver;
                    olaLineRental.BackColor = Color.Silver;

                    opnInfo2.Visible = true;
                    opnContent.Visible = true;
                    opnInfo.Visible = false;
                    opnAge.Visible = false;
                    opnFashion.Visible = false;
                    opnRental.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmInfo2_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmInfo.BackColor == Color.Silver)
                {
                    ocmInfo.BackColor = cVB.oVB_ColDark;
                    olaLineInfo.BackColor = cVB.oVB_ColDark;

                    ocmInfo2.BackColor = Color.Silver;
                    olaLineInfo2.BackColor = Color.Silver;
                    ocmAge.BackColor = Color.Silver;
                    olaLineAge.BackColor = Color.Silver;
                    ocmFashion.BackColor = Color.Silver;
                    olaLineFashion.BackColor = Color.Silver;
                    ocmRental.BackColor = Color.Silver;
                    olaLineRental.BackColor = Color.Silver;

                    opnInfo.Visible = true;
                    opnContent.Visible = false;
                    opnInfo2.Visible = false;
                    opnAge.Visible = false;
                    opnFashion.Visible = false;
                    opnRental.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmInfo_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                new wProductM().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Show Popup Barcode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAddUnit_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAddUnit();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmAddUnit_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Show Popup Barcode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAddBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAddBarcode();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmAddBarcode_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Show Popup Supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAddSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAddSupplier();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmAddSupplier_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Change, Clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCamera_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile;
            Bitmap oBitmap;

            try
            {
                if(string.Equals(opbPdtAddEdit.Tag, "Product"))
                {
                    oFile = new OpenFileDialog();
                    oFile.Filter = "Image Files(*.png)|*.png";

                    if (oFile.ShowDialog() == DialogResult.OK)
                    {
                        //*Net 63-08-01 ปรับการดึงรูปไม่ให้ lock file
                        using (Image oImg = Image.FromFile(oFile.FileName))
                        {
                            //oBitmap = new Bitmap(oFile.FileName);
                            oBitmap = new Bitmap(oImg);
                            opbPdtAddEdit.Image = oBitmap;
                            opbPdtAddEdit.Tag = null;
                            ocmCamera.Image = Properties.Resources.ClearR_32;
                        }
                    }
                }
                else
                {
                    opbPdtAddEdit.Image = Properties.Resources.Product_256;
                    opbPdtAddEdit.Tag = "Product";
                    ocmCamera.Image = Properties.Resources.CameraB_32;
                }

                opbPdtDetail.Image = opbPdtAddEdit.Image;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmCamera_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Age
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAge_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmAge.BackColor == Color.Silver)
                {
                    ocmAge.BackColor = cVB.oVB_ColDark;
                    olaLineAge.BackColor = cVB.oVB_ColDark;

                    ocmInfo2.BackColor = Color.Silver;
                    olaLineInfo2.BackColor = Color.Silver;
                    ocmInfo.BackColor = Color.Silver;
                    olaLineInfo.BackColor = Color.Silver;
                    ocmFashion.BackColor = Color.Silver;
                    olaLineFashion.BackColor = Color.Silver;
                    ocmRental.BackColor = Color.Silver;
                    olaLineRental.BackColor = Color.Silver;

                    opnContent.Visible = true;
                    opnAge.Visible = true;
                    opnInfo.Visible = false;
                    opnInfo2.Visible = false;
                    opnFashion.Visible = false;
                    opnRental.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmAge_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Accpet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAcceptPdt_Click(object sender, EventArgs e)
        {
            try
            {
                ocmAge.Visible = true;
                ocmFashion.Visible = true;
                ocmRental.Visible = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmAcceptPdt_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Fashion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmFashion_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmFashion.BackColor == Color.Silver)
                {
                    ocmFashion.BackColor = cVB.oVB_ColDark;
                    olaLineFashion.BackColor = cVB.oVB_ColDark;

                    ocmInfo2.BackColor = Color.Silver;
                    olaLineInfo2.BackColor = Color.Silver;
                    ocmInfo.BackColor = Color.Silver;
                    olaLineInfo.BackColor = Color.Silver;
                    ocmAge.BackColor = Color.Silver;
                    olaLineAge.BackColor = Color.Silver;
                    ocmRental.BackColor = Color.Silver;
                    olaLineRental.BackColor = Color.Silver;

                    opnContent.Visible = true;
                    opnFashion.Visible = true;
                    opnAge.Visible = false;
                    opnInfo.Visible = false;
                    opnInfo2.Visible = false;
                    opnRental.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmFashion_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Rental
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmRental_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmRental.BackColor == Color.Silver)
                {
                    ocmRental.BackColor = cVB.oVB_ColDark;
                    olaLineRental.BackColor = cVB.oVB_ColDark;

                    ocmInfo2.BackColor = Color.Silver;
                    olaLineInfo2.BackColor = Color.Silver;
                    ocmInfo.BackColor = Color.Silver;
                    olaLineInfo.BackColor = Color.Silver;
                    ocmAge.BackColor = Color.Silver;
                    olaLineAge.BackColor = Color.Silver;
                    ocmFashion.BackColor = Color.Silver;
                    olaLineFashion.BackColor = Color.Silver;

                    opnContent.Visible = true;
                    opnRental.Visible = true;
                    opnFashion.Visible = false;
                    opnAge.Visible = false;
                    opnInfo.Visible = false;
                    opnInfo2.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmRental_Click : " + oEx.Message); }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmMenu_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Call Keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "ocmAbout_Click : " + oEx.Message); }
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

                nW_Time++;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wPdtAddOrEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit", "wPdtAddOrEdit_FormClosing : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtAddOrEdit:", "ocmNotify_Click : " + oEx.Message); }
        }
        //*Net 63-07-31 ปรับตาม Moshi
        private void wPdtAddOrEdit_Shown(object sender, EventArgs e)
        {
            if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
            cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
        }
    }
}
