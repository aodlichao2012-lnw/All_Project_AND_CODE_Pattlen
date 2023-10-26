using AdaPos.Class;
using AdaPos.Models.Database;
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

namespace AdaPos.Forms
{
    public partial class wPmtPriConfirm : Form
    {
        #region Variable

        private cSP oW_SP;
        private cPdtPmt oW_PdtPmt;
        private ResourceManager oW_Resource;
        private int nW_ModeClose = 2;    // 1:Back, 2:Close
        public static string tW_PmtDoc;
        #endregion

        #region Constructor
        public wPmtPriConfirm()
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                W_SETxDesign();
                W_SETxText();

                W_SHWxButtonBar();
                W_PRCxLoadData();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtPriConfirm:", "wPmtPriConfirm : " + oEx.Message);
            }
        }
        #endregion Constructor

        #region Function
        private void W_SETxDesign()
        {
            // Set Design ในส่วน Header And Menu  +++++++++++++++++++++++++++++++++++++++++++++++

            try
            {
                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark

                opbLogo.Image = new cCompany().C_GEToImageLogo();
                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode, "TCNMUser");

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;

                // Set Design ในส่วน Content  ++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                opnCashPay.BackColor = cVB.oVB_ColDark;
                olaTitleCashPay.ForeColor = Color.White;
                olaCashPayment.ForeColor = Color.White;
                oW_SP.SP_SETxSetGridviewFormat(ogdPdt);

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                // Set Design ในส่วน Footer ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                ocmConfirm.BackColor = cVB.oVB_ColDark;

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtPriConfirm:", "W_SETxDesign : " + oEx.Message);
            }
        }

        private void W_SETxText()
        {
            cVB.tVB_KbdScreen = "PROMOTION";
            switch (cVB.nVB_Language)
            {
                case 1:     // TH
                    oW_Resource = new ResourceManager(typeof(resSale_TH));
                    break;

                default:    // EN
                    oW_Resource = new ResourceManager(typeof(resSale_EN));

                    break;
            }

            if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                olaBranch.Text = cVB.tVB_BchName;
            else
                olaBranch.Text = cVB.tVB_ShpName;

            olaPos.Text = cVB.tVB_PosCode;
            olaDocNo.Text = cVB.tVB_DocNo;
            // User
            olaUsrName.Text = new cUser().C_GETtUsername();

            olaTitleCashPayOld.Text = cVB.oVB_GBResource.GetString("tTotalOldPri");
            olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tTotalNewPri");

            otbColSeqStd.HeaderText = cVB.oVB_GBResource.GetString("tSeq");
            otbColBarcodeStd.HeaderText = cVB.oVB_GBResource.GetString("tBarcode");
            otbColPdtNameStd.HeaderText = cVB.oVB_GBResource.GetString("tPdtName");
            otbColPdtQtyStd.HeaderText = cVB.oVB_GBResource.GetString("tQty");
            otbColUnitNameStd.HeaderText = cVB.oVB_GBResource.GetString("tUnit");
            otbColSalePrice.HeaderText = cVB.oVB_GBResource.GetString("tOriPrice");
            otbColSetPriceStd.HeaderText = cVB.oVB_GBResource.GetString("tNewPrice");
            otbColDiscount.HeaderText = cVB.oVB_GBResource.GetString("tDis");
            otbColPdtSumPriceStd.HeaderText = cVB.oVB_GBResource.GetString("tTotalNewPri");

            new cSP().SP_SETxSetGridviewFormat(ogdPdt);
            ocmConfirm.Text = cVB.oVB_GBResource.GetString("tConfirmNewPri");
            ocmCancel.Text = cVB.oVB_GBResource.GetString("tCancelNewPri");
            olaWarning.Text = cVB.oVB_GBResource.GetString("tMsgWarnigClear");
        }

        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT;  //*Em 62-01-24  Waterpark
            List<cmlTPSMFunc> aoMenuB;  //*Em 62-01-24  Waterpark
            int nItem;  //*Em 62-01-24  Waterpark
            try
            {
                aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                //*Em 62-01-22  Waterpark
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
                            ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        }
                        catch { }
                        ocmMenu.TextImageRelation = TextImageRelation.ImageBeforeText;

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 249;
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
                        ocmMenu.Width = 249;
                        opnMenuB.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuB.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuB.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }
                //++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtPriConfirm", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process Back
        /// </summary>
        private void W_PRCxBack()
        {
            try
            {
                switch (cVB.oVB_Sale.nW_Mode)
                {
                    case 1:     // Topup
                        new wTopUp().Show();
                        break;

                    case 2:     // Cancel Topup
                        new wCancelTopUp().Show();
                        break;

                    case 3:     // Sale
                        //new wSale(nW_Mode).Show();
                        if (cSale.nC_DocType == 1) if (cPayment.C_PRCbCancelCoupon(1, cVB.tVB_DocNo) == false) return; //*Em 63-01-09  

                        if (cVB.oVB_2ndScreen != null)
                        {
                            cVB.oVB_2ndScreen.W_PRCxBack();
                            cVB.oVB_2ndScreen.Update();
                        }

                        cVB.oVB_Sale.Show();
                        this.Close();
                        break;

                    case 4:     // Return Sale
                        new wReturn().Show();
                        break;

                    case 5:     // Rental 
                        new wSale(cVB.oVB_Sale.nW_Mode).Show();
                        break;

                    case 6:     // Return Rental
                        new wReturn().Show();
                        break;

                    case 7:     // Ticket
                        new wTicket().Show();
                        break;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtPriConfirm", "W_PRCxBack : " + oEx.Message); }
        }

        private void W_PRCxLoadData()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            try
            {
                oSql.Clear();
                oSql.AppendLine("DECLARE @tBchCode VARCHAR(5)");
                oSql.AppendLine("DECLARE @tDocNo VARCHAR(20)");
                oSql.AppendLine("SET @tBchCode = '"+ cVB.tVB_BchCode +"'");
                oSql.AppendLine("SET @tDocNo = '"+ cVB.tVB_DocNo + "'");
                oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTXsdBarCode,DT.FTXsdPdtName,DT.FCXsdQty,");
                oSql.AppendLine("DT.FTPunName,DT.FCXsdSetPrice AS FCXsdSalePrice,");
                oSql.AppendLine("ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice) AS FCXsdSetPrice,");
                oSql.AppendLine("0.00 AS FCXsdDis,");
                oSql.AppendLine("ROUND((ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice) * DT.FCXsdQty),"+ cVB.nVB_DecShow +") AS FCXsdNet");
                oSql.AppendLine("FROM "+ cSale.tC_TblSalDT +" DT WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdQty,PD.FCXsdSetPrice");
                oSql.AppendLine("   FROM " + cSale.tC_TblSalPD + " PD WITH(NOLOCK)");
                oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                oSql.AppendLine("   	FROM " + cSale.tC_TblSalPD + " WITH(NOLOCK)");
                oSql.AppendLine("   	WHERE FTBchCode = @tBchCode AND FTXshDocNo = @tDocNo AND FTXpdGetType = '4'");
                oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                oSql.AppendLine("   WHERE PD.FTBchCode = @tBchCode AND PD.FTXshDocNo = @tDocNo AND PD.FTXpdGetType = '4') PD");
                oSql.AppendLine("   ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                oSql.AppendLine("WHERE DT.FTBchCode = @tBchCode");
                oSql.AppendLine("AND DT.FTXshDocNo = @tDocNo");
                oSql.AppendLine("ORDER BY DT.FNXsdSeqNo");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                ogdPdt.Rows.Clear();
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        ogdPdt.Rows.Add(oRow["FNXsdSeqNo"],oRow["FTXsdBarCode"],oRow["FTXsdPdtName"],
                            oRow["FCXsdQty"],
                            oRow["FTPunName"],
                            oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdSalePrice"], cVB.nVB_DecShow),
                            oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdSetPrice"], cVB.nVB_DecShow),
                            oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdDis"], cVB.nVB_DecShow),
                            oW_SP.SP_SETtDecShwSve(1, (decimal)oRow["FCXsdNet"], cVB.nVB_DecShow));
                    }
                }
                decimal cTotal = Convert.ToDecimal(odtTmp.Compute("SUM(FCXsdNet)", string.Empty));
                olaCashPayment.Text =  oW_SP.SP_SETtDecShwSve(1, cTotal, cVB.nVB_DecShow);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtPriConfirm", "W_PRCxLoadData : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                odtTmp = null;
                new cSP().SP_CLExMemory();
            }
        }

        private void W_PRCxConfirm()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine("DECLARE @tBchCode VARCHAR(5)");
                oSql.AppendLine("DECLARE @tDocNo VARCHAR(20)");
                oSql.AppendLine("SET @tBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("SET @tDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("UPDATE DT");
                oSql.AppendLine("SET FTPplCode = PD.FTPplCode");
                oSql.AppendLine(",FCXsdSetPrice = PD.FCXsdSetPrice");
                oSql.AppendLine(",FCXsdAmtB4DisChg = (DT.FCXsdQty * PD.FCXsdSetPrice)");
                oSql.AppendLine(",FCXsdNet = (DT.FCXsdQty * PD.FCXsdSetPrice)");
                oSql.AppendLine(",FCXsdNetAfHD = (DT.FCXsdQty * PD.FCXsdSetPrice)");
                oSql.AppendLine("FROM "+ cSale.tC_TblSalDT +" DT WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdSetPrice,PD.FTPplCode");
                oSql.AppendLine("   FROM " + cSale.tC_TblSalPD + " PD WITH(NOLOCK)");
                oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                oSql.AppendLine("   	FROM " + cSale.tC_TblSalPD + " WITH(NOLOCK)");
                oSql.AppendLine("   	WHERE FTBchCode = @tBchCode AND FTXshDocNo = @tDocNo AND FTXpdGetType = '4'");
                oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo,FTPplCode) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                oSql.AppendLine("   WHERE PD.FTBchCode = @tBchCode AND PD.FTXshDocNo = @tDocNo AND PD.FTXpdGetType = '4') PD");
                oSql.AppendLine("   ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                oSql.AppendLine("WHERE DT.FTBchCode = @tBchCode");
                oSql.AppendLine("AND DT.FTXshDocNo = @tDocNo");
                oSql.AppendLine();
                oSql.AppendLine("DELETE DTDis");
                oSql.AppendLine("FROM " + cSale.tC_TblSalDTDis + " DTDis WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdSetPrice,PD.FTPplCode");
                oSql.AppendLine("   FROM " + cSale.tC_TblSalPD + " PD WITH(NOLOCK)");
                oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                oSql.AppendLine("   	FROM " + cSale.tC_TblSalPD + " WITH(NOLOCK)");
                oSql.AppendLine("   	WHERE FTBchCode = @tBchCode AND FTXshDocNo = @tDocNo AND FTXpdGetType = '4'");
                oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo,FTPplCode) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                oSql.AppendLine("   WHERE PD.FTBchCode = @tBchCode AND PD.FTXshDocNo = @tDocNo AND PD.FTXpdGetType = '4') PD");
                oSql.AppendLine("   ON DTDis.FTBchCode = PD.FTBchCode AND DTDis.FTXshDocNo = PD.FTXshDocNo AND DTDis.FNXsdSeqNo = PD.FNXsdSeqNo");
                oSql.AppendLine("WHERE DTDis.FTBchCode = @tBchCode");
                oSql.AppendLine("AND DTDis.FTXshDocNo = @tDocNo");
                oSql.AppendLine("AND DTDis.FNXddStaDis = 1");
                oDB.C_SETxDataQuery(oSql.ToString());

                cVB.tVB_PriceGroup = oDB.C_GETtFunction("TOP 1", "FTPplCode", cSale.tC_TblSalPD, "WHERE FTBchCode = '"+ cVB.tVB_BchCode +"' AND FTXshDocNo = '"+ cVB.tVB_DocNo +"' AND FTXpdGetType = '4'");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtPriConfirm", "W_PRCxConfirm : " + oEx.Message); }

        }
        #endregion Function

        #region Method/Events
        private void ocmMenuBar_Click(object sender, EventArgs e)
        {
            string tFuncName;
            try
            {
                Button ocmMenu;
                ocmMenu = (Button)sender;
                tFuncName = ocmMenu.Tag.ToString();
                switch (tFuncName)
                {
                    case "C_KBDxBack": W_PRCxBack(); break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtPriConfirm", "ocmMenuBar_Click : " + oEx.Message); }

        }

        private void ocmCancel_Click(object sender, EventArgs e)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDatabase = new cDatabase();
            try
            {
                oSql.AppendLine("DELETE FROM " + cSale.tC_TblSalPD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDatabase.C_SETxDataQuery(oSql.ToString());

                cVB.oVB_Sale.bW_CalPmtPrice = false;
                cVB.oVB_Sale.W_PRCxCoPmt();
                if (cVB.oVB_GetPmt.Rows.Count > 0 || cVB.oVB_PmtSug.Rows.Count > 0)
                {
                    wPmtGetorSug oGetORSug = new wPmtGetorSug();
                    oGetORSug.olaCashPayment.Text = olaCashPayOld.Text;
                    oGetORSug.olaCstName.Text = cVB.tVB_CstName;
                    oGetORSug.ShowDialog();
                    oGetORSug.Update();
                    this.Close();
                }
                else
                {
                    cSale.C_PRCxSummary2HD();
                    new wPayment(3).ShowDialog();
                    this.Close();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtPriConfirm", "ocmCancel_Click : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDatabase = null;
                new cSP().SP_CLExMemory();
            }
            
        }
        private void ocmConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxConfirm();
                cVB.bVB_PriceConfirm = true;
                cVB.oVB_Sale.bW_CalPmtPrice = false;
                cVB.oVB_Sale.W_PRCxCoPmt();
                if (cVB.oVB_GetPmt.Rows.Count > 0 || cVB.oVB_PmtSug.Rows.Count > 0)
                {
                    wPmtGetorSug oGetORSug = new wPmtGetorSug();
                    oGetORSug.olaCashPayment.Text = olaCashPayment.Text;
                    oGetORSug.olaCstName.Text = cVB.tVB_CstName;
                    oGetORSug.ShowDialog();
                    oGetORSug.Update();
                    this.Close();
                }
                else
                {
                    cSale.C_PRCxSummary2HD();
                    new wPayment(3).ShowDialog();
                    this.Close();
                }
                    
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtPriConfirm", "ocmConfirm_Click : " + oEx.Message); }
            
        }

        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtPriConfirm", "ocmMenu_Click " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        #endregion Method/Events


    }
}
