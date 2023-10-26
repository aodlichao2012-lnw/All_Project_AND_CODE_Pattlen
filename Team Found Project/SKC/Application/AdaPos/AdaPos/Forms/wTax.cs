using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Popup.All;
using AdaPos.Popup.wTaxInvoice;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Forms
{
    public partial class wTax : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        public cmlTPSTTaxHD oW_TaxHD;
        public cmlTPSTTaxHDCst oW_TaxHDCst;
        private List<cmlTPSTTaxHDDis> aoW_TaxHDDis;
        private List<cmlTPSTTaxDT> aoW_TaxDT;
        private List<cmlTPSTTaxDTDis> aoW_TaxDTDis;
        private List<cmlTPSTTaxDTPmt> aoW_TaxDTPmt;
        private List<cmlTPSTTaxRC> aoW_TaxRC;

        private cmlTCNMAddress oW_Addr;
        private cmlTCNMTaxAddress oW_CstAddr;
        private string tW_Addr;
        private string tW_TaxDocNo;
        private string tW_AddTaxNo;
        private string tW_CstCode;
        private decimal cW_SumQty;
        private int nW_Mode;    //1:Wristband / Card 2:Document
        private bool bW_StaApv;
        #endregion End Variable

        public wTax()
        {
            InitializeComponent();
            try
            {
                //nW_Mode = pnMode;
                oW_SP = new cSP();
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();
                W_SHWxButtonBar();
                W_SETxDefualt();
                W_PRCxLock(false);
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in opnMenu.Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "wTaxInvoice : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }

        #region Function
        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark;
                opnMenuB.BackColor = cVB.oVB_ColDark;

                ogdPdtDetail.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                ocmBrwAddr.BackColor = cVB.oVB_ColDark;
                ocmSave.BackColor = cVB.oVB_ColDark;
                ocmBrwSlip.BackColor = cVB.oVB_ColDark;
                ocmBrwCst.BackColor = cVB.oVB_ColDark;

                otbCstName.BackColor = cVB.oVB_ColLight;
                otbTaxNo.BackColor = cVB.oVB_ColLight;
                oW_SP.SP_SETxSetGridviewFormat(ogdPdtDetail);
                ogdPdtDetail.ClearSelection();
                ogdPdtDetail.ReadOnly = true;
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resTaxInvoice_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resTaxInvoice_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "TAX";
                //*Em 62-09-09
                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                olaTax.Text = oW_Resource.GetString("tTax");
                switch (nW_Mode)
                {
                    case 1:
                        olaTitleSlipNo.Text = oW_Resource.GetString("tWBNo");
                        break;
                    case 2:
                    default:
                        olaTitleSlipNo.Text = oW_Resource.GetString("tTitleSlipNo");
                        break;
                }
                olaTitleCst.Text = oW_Resource.GetString("tTitleCst");
                olaTitleTaxNo.Text = oW_Resource.GetString("tTitleTaxNo");
                olaTitleIDCard.Text = oW_Resource.GetString("tTaxID");
                olaTitleCstTax.Text = oW_Resource.GetString("tTitleCstNameTax");
                olaTitleTel.Text = oW_Resource.GetString("tTel");
                olaTitleFax.Text = oW_Resource.GetString("tFax");
                olaTitleAddr1.Text = oW_Resource.GetString("tTitleAddr1");
                olaTitleAddr2.Text = oW_Resource.GetString("tTitleAddr2");
                olaTypeOfBus.Text = oW_Resource.GetString("tBsnType");
                olaTitleCmpType.Text = oW_Resource.GetString("tEstab");
                olaTitleBchCode.Text = oW_Resource.GetString("tBchCode");
                olaTitleRmk.Text = oW_Resource.GetString("tRemark");

                //Grid
                otbTitleSeq.HeaderText = oW_Resource.GetString("tSeq");
                otbTitlePdtID.HeaderText = oW_Resource.GetString("tPdtID");
                otbTitlePdtName.HeaderText = oW_Resource.GetString("tPdtName");
                otbTitleUnit.HeaderText = oW_Resource.GetString("tTitleUnit");
                otbTitleQty.HeaderText = cVB.oVB_GBResource.GetString("tQty");
                otbTitleDisChg.HeaderText = oW_Resource.GetString("tTitleDisChg");
                otbTitleAmt.HeaderText = cVB.oVB_GBResource.GetString("tAmount");

                //Summary
                olaTitleTotal.Text = oW_Resource.GetString("tTitleTotal");
                olaTitleDisChg.Text = oW_Resource.GetString("tTitleDisChg");
                olaTitleAftDisChg.Text = oW_Resource.GetString("tTitleAftDisChg");
                olaTitleTotalVat.Text = oW_Resource.GetString("tTitleTotalVat");
                olaTitleNet.Text = oW_Resource.GetString("tTitleNet");

                // user
                olaUsrName.Text = new cUser().C_GETtUsername();

                // Type of business
                ocbBsnType.Items.Add(oW_Resource.GetString("tJuristic"));
                ocbBsnType.Items.Add(oW_Resource.GetString("tIndividual"));
                ocbBsnType.SelectedIndex = 0;

                // Establishment
                ocbEstab.Items.Add(oW_Resource.GetString("tHQ"));
                ocbEstab.Items.Add(oW_Resource.GetString("tBranch"));
                ocbEstab.SelectedIndex = 0;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_SETxText : " + oEx.Message); }
        }

        private void W_GETxData()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT *");
                oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oW_TaxHD = new cmlTPSTTaxHD();
                oW_TaxHD = oDB.C_GEToDataQuery<cmlTPSTTaxHD>(oSql.ToString());

                if (oW_TaxHD != null)
                {

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT *");
                    oSql.AppendLine("FROM TPSTSalHDCst WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oW_TaxHDCst = new cmlTPSTTaxHDCst();
                    oW_TaxHDCst = oDB.C_GEToDataQuery<cmlTPSTTaxHDCst>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT *");
                    oSql.AppendLine("FROM TPSTSalHDDis WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    aoW_TaxHDDis = new List<cmlTPSTTaxHDDis>();
                    aoW_TaxHDDis = oDB.C_GETaDataQuery<cmlTPSTTaxHDDis>(oSql.ToString());

                    oSql = new StringBuilder();
                    //oSql.AppendLine("SELECT * ");
                    //oSql.AppendLine("FROM TPSTSalDT WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");

                    //*Em 63-04-30
                    oSql.AppendLine("SELECT DT.FTBchCode, DT.FTXshDocNo, DT.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("FCXsdSalePrice, DT.FCXsdQty, FCXsdQtyAll,ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice) AS FCXsdSetPrice, ");
                    oSql.AppendLine("(DT.FCXsdQty * ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice)) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt,ISNULL(DTD.FCXddValue,0) AS FCXsdDis, ISNULL(DTC.FCXddValue,0) AS FCXsdChg, ");
                    oSql.AppendLine("(DT.FCXsdQty * ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice))- ISNULL(DTD.FCXddValue,0) + ISNULL(DTC.FCXddValue,0) AS FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy ");
                    oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                    oSql.AppendLine("LEFT JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdQty,PD.FCXsdSetPrice");
                    oSql.AppendLine("   FROM TPSTSalPD PD WITH(NOLOCK)");
                    oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                    oSql.AppendLine("   	FROM TPSTSalPD WITH(NOLOCK)");
                    oSql.AppendLine("   	WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTXpdGetType = '4'");
                    oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                    oSql.AppendLine("   WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNo + "' AND PD.FTXpdGetType = '4') PD");
                    oSql.AppendLine("   ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                    oSql.AppendLine("LEFT JOIN(SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,SUM(FCXddValue) AS FCXddValue");
                    oSql.AppendLine("			FROM TPSTSalDTDis WITH(NOLOCK)");
                    oSql.AppendLine("			WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("			AND ((FTXddDisChgType IN('1','2') AND FNXddStaDis = 1)");
                    oSql.AppendLine("			OR (FTXddDisChgType IN('1','2') AND FNXddStaDis = 2 AND FTXddRefCode <> ''");
                    oSql.AppendLine("           AND FTXddRefCode NOT IN (SELECT DISTINCT FTPmhDocNo FROM TPSTSalPD WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTXpdGetType = '4')))");
                    oSql.AppendLine("			GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) DTD ON DTD.FTBchCode = DT.FTBchCode AND DTD.FTXshDocNo = DT.FTXshDocNo AND DTD.FNXsdSeqNo = DT.FNXsdSeqNo ");
                    oSql.AppendLine("LEFT JOIN(SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,SUM(FCXddValue) AS FCXddValue");
                    oSql.AppendLine("			FROM TPSTSalDTDis WITH(NOLOCK)");
                    oSql.AppendLine("			WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("			AND (FTXddDisChgType IN('3','4') AND FNXddStaDis = 1)");
                    oSql.AppendLine("			GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) DTC ON DTC.FTBchCode = DT.FTBchCode AND DTC.FTXshDocNo = DT.FTXshDocNo AND DTC.FNXsdSeqNo = DT.FNXsdSeqNo ");
                    oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                    //+++++++++++++++++++++++++
                    aoW_TaxDT = new List<cmlTPSTTaxDT>();
                    aoW_TaxDT = oDB.C_GETaDataQuery<cmlTPSTTaxDT>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT * ");
                    oSql.AppendLine("FROM TPSTSalDTDis WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    aoW_TaxDTDis = new List<cmlTPSTTaxDTDis>();
                    aoW_TaxDTDis = oDB.C_GETaDataQuery<cmlTPSTTaxDTDis>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT * ");
                    oSql.AppendLine("FROM TPSTSalDTPmt WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    aoW_TaxDTPmt = new List<cmlTPSTTaxDTPmt>();
                    aoW_TaxDTPmt = oDB.C_GETaDataQuery<cmlTPSTTaxDTPmt>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT * ");
                    oSql.AppendLine("FROM TPSTSalRC WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    aoW_TaxRC = new List<cmlTPSTTaxRC>();
                    aoW_TaxRC = oDB.C_GETaDataQuery<cmlTPSTTaxRC>(oSql.ToString());
                }

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ADR.*,SUD.FTSudName,DST.FTDstName,PVN.FTPvnName ");
                oSql.AppendLine("FROM TCNMAddress_L ADR WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMSubDistrict_L SUD WITH(NOLOCK) ON SUD.FTSudCode = ISNULL(ADR.FTAddV1SubDist,'') AND SUD.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("LEFT JOIN TCNMDistrict_L DST WITH(NOLOCK) ON DST.FTDstCode = ISNULL(ADR.FTAddV1DstCode,'') AND DST.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("LEFT JOIN TCNMProvince_L PVN WITH(NOLOCK) ON PVN.FTPvnCode = ISNULL(ADR.FTAddV1PvnCode,'') AND PVN.FNLngID = " + cVB.nVB_Language);
                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                {
                    oSql.AppendLine("WHERE FTAddGrpType = '1' AND FTAddRefNo = '1' AND FTAddRefCode = '" + cVB.tVB_BchCode + "' AND ADR.FNLngID = " + cVB.nVB_Language);
                }
                else
                {
                    oSql.AppendLine("WHERE FTAddGrpType = '7' AND FTAddRefNo = '1' AND FTAddRefCode = '" + cVB.tVB_Merchart + "' AND ADR.FNLngID = " + cVB.nVB_Language);
                }

                oW_Addr = new cmlTCNMAddress();
                oW_Addr = oDB.C_GEToDataQuery<cmlTCNMAddress>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_GETxData : " + oEx.Message); }
        }

        private void W_GETxDataTax()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT *");
                oSql.AppendLine("FROM TPSTTaxHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                oW_TaxHD = new cmlTPSTTaxHD();
                oW_TaxHD = oDB.C_GEToDataQuery<cmlTPSTTaxHD>(oSql.ToString());

                if (oW_TaxHD != null)
                {

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT *");
                    oSql.AppendLine("FROM TPSTTaxHDCst WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                    oW_TaxHDCst = new cmlTPSTTaxHDCst();
                    oW_TaxHDCst = oDB.C_GEToDataQuery<cmlTPSTTaxHDCst>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT *");
                    oSql.AppendLine("FROM TPSTTaxHDDis WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                    aoW_TaxHDDis = new List<cmlTPSTTaxHDDis>();
                    aoW_TaxHDDis = oDB.C_GETaDataQuery<cmlTPSTTaxHDDis>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT * ");
                    oSql.AppendLine("FROM TPSTTaxDT WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                    aoW_TaxDT = new List<cmlTPSTTaxDT>();
                    aoW_TaxDT = oDB.C_GETaDataQuery<cmlTPSTTaxDT>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT * ");
                    oSql.AppendLine("FROM TPSTTaxDTDis WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                    aoW_TaxDTDis = new List<cmlTPSTTaxDTDis>();
                    aoW_TaxDTDis = oDB.C_GETaDataQuery<cmlTPSTTaxDTDis>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT * ");
                    oSql.AppendLine("FROM TPSTTaxDTPmt WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                    aoW_TaxDTPmt = new List<cmlTPSTTaxDTPmt>();
                    aoW_TaxDTPmt = oDB.C_GETaDataQuery<cmlTPSTTaxDTPmt>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT * ");
                    oSql.AppendLine("FROM TPSTTaxRC WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                    aoW_TaxRC = new List<cmlTPSTTaxRC>();
                    aoW_TaxRC = oDB.C_GETaDataQuery<cmlTPSTTaxRC>(oSql.ToString());
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_GETxDataTax : " + oEx.Message); }
        }

        private void W_SETxData(int pnMode)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                if (oW_TaxHD != null)
                {
                    if (pnMode == 2)
                    {
                        otbSlipNo.Text = oW_TaxHD.FTXshRefExt.ToString();
                    }
                    if (string.IsNullOrEmpty(oW_TaxHD.FTCstCode))
                    {
                        tW_CstCode = cVB.tVB_CstDef;
                        otbCstName.Text = cVB.tVB_CstDefName;
                    }
                    else
                    {
                        tW_CstCode = oW_TaxHD.FTCstCode.ToString();
                    }

                    olaAmtTotal.Text = oW_SP.SP_SETtDecShwSve(1, (decimal)oW_TaxHD.FCXshTotal, cVB.nVB_DecShow);
                    olaAmtDisChg.Text = oW_SP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshDis-oW_TaxHD.FCXshChg), cVB.nVB_DecShow);
                    olaAmtAftDisChg.Text = oW_SP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshTotalAfDisChgV+oW_TaxHD.FCXshTotalAfDisChgNV+ oW_TaxHD.FCXshTotalNoDis), cVB.nVB_DecShow);
                    olaAmtTotalVat.Text = oW_SP.SP_SETtDecShwSve(1, (decimal)oW_TaxHD.FCXshVat, cVB.nVB_DecShow);
                    olaAmtNet.Text = oW_SP.SP_SETtDecShwSve(1, (decimal)oW_TaxHD.FCXshGrand, cVB.nVB_DecShow);
                    otbRemark.Text =  string.IsNullOrEmpty(oW_TaxHD.FTXshRmk)?"":oW_TaxHD.FTXshRmk ;  //*Em 63-05-05
                }
                else
                {
                    tW_CstCode = cVB.tVB_CstDef;
                    otbCstName.Text = cVB.tVB_CstDefName;
                }

                if (oW_TaxHDCst != null)
                {
                    otbIDCard.Text = oW_TaxHDCst.FTXshCardID;
                    otbCstName.Text = oW_TaxHDCst.FTXshCstName.ToString();
                    otbTel.Text = oW_TaxHDCst.FTXshCstTel;  //*Em 62-12-18
                    if (!string.IsNullOrEmpty(oW_TaxHDCst.FTXshCardID))
                    {
                        oSql.AppendLine("SELECT TOP 1 * FROM TCNMTaxAddress_L WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTAddTaxNo = '" + oW_TaxHDCst.FTXshCardID + "' AND FNLngID = " + cVB.nVB_Language);
                        oW_CstAddr = oDB.C_GEToDataQuery<cmlTCNMTaxAddress>(oSql.ToString());
                        if (oW_CstAddr != null)
                        {
                            otbCstTax.Text = oW_CstAddr.FTAddName.ToString();
                            if (!string.IsNullOrEmpty(oW_CstAddr.FTAddV2Desc1))
                            {
                                otbAddr1.Text = oW_CstAddr.FTAddV2Desc1;
                            }
                            if (!string.IsNullOrEmpty(oW_CstAddr.FTAddV2Desc2))
                            {
                                otbAddr2.Text = oW_CstAddr.FTAddV2Desc2;
                            }
                            if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaBusiness))
                            {
                                ocbBsnType.SelectedIndex = (oW_CstAddr.FTAddStaBusiness == "1" ? 0 : 1);
                            }
                            if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaHQ))
                            {
                                ocbEstab.SelectedIndex = (oW_CstAddr.FTAddStaHQ == "1" ? 0 : 1);
                                if (ocbEstab.SelectedIndex == 1)
                                {
                                    if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaBchCode))
                                    {
                                        otbBchCode.Text = oW_CstAddr.FTAddStaBchCode;
                                    }
                                }
                            }
                            otbTel.Text = oW_CstAddr.FTAddTel;  //*Em 63-05-05
                            otbFax.Text = oW_CstAddr.FTAddFax;  //*Em 63-05-05
                        }
                    }
                }

                ogdPdtDetail.Rows.Clear();
                if (aoW_TaxDT.Count > 0)
                {
                    foreach (cmlTPSTTaxDT oDT in aoW_TaxDT)
                    {
                        ogdPdtDetail.Rows.Add(oDT.FNXsdSeqNo, oDT.FTPdtCode, oDT.FTXsdPdtName, oDT.FTPunName, oDT.FCXsdQty,oDT.FCXsdDis-oDT.FCXsdChg, oDT.FCXsdNet);
                    }
                }

                cW_SumQty = ogdPdtDetail.Rows.Cast<DataGridViewRow>().Sum(oRow => Convert.ToDecimal(oRow.Cells["otbTitleQty"].Value));

                ogdPdtDetail.ClearSelection();  //*Em 63-02-03
                ogdPdtDetail.ReadOnly = true;  //*Em 63-02-03

                tW_Addr = "";
                tW_AddTaxNo = "";
                if (oW_Addr != null)
                {
                    if (oW_Addr.FTAddVersion == "1")
                    {
                        if (oW_Addr.FTAddV1No != null)
                        {
                            tW_Addr += oW_Addr.FTAddV1No;
                        }

                        if (oW_Addr.FTAddV1Soi != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            { tW_Addr += oW_Addr.FTAddV1Soi; }
                            else { tW_Addr += " " + oW_Addr.FTAddV1Soi; }
                        }

                        if (oW_Addr.FTAddV1Village != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            { tW_Addr += oW_Addr.FTAddV1Village; }
                            else
                            { tW_Addr += " " + oW_Addr.FTAddV1Village; }
                        }

                        if (oW_Addr.FTAddV1Road != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            { tW_Addr += oW_Addr.FTAddV1Road; }
                            else { tW_Addr += " " + oW_Addr.FTAddV1Road; }
                        }

                        if (oW_Addr.FTSudName != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            { tW_Addr += oW_Addr.FTSudName; }
                            else { tW_Addr += " " + oW_Addr.FTSudName; }
                        }

                        if (oW_Addr.FTDstName != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            { tW_Addr += oW_Addr.FTDstName; }
                            else { tW_Addr += " " + oW_Addr.FTDstName; }
                        }

                        if (oW_Addr.FTPvnName != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            { tW_Addr += oW_Addr.FTPvnName; }
                            else { tW_Addr += " " + oW_Addr.FTPvnName; }
                        }

                        if (oW_Addr.FTAddV1PostCode != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            { tW_Addr += oW_Addr.FTAddV1PostCode; }
                            else { tW_Addr += " " + oW_Addr.FTAddV1PostCode; }
                        }
                    }
                    else
                    {
                        tW_Addr = oW_Addr.FTAddV2Desc1.ToString() + " " + oW_Addr.FTAddV2Desc2.ToString();
                    }
                    tW_AddTaxNo = oW_Addr.FTAddTaxNo.ToString();

                    
                }
                if (pnMode == 2) W_PRCxLock(true);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_GETxData : " + oEx.Message); }
        }

        public void W_PRNxDrawTax(Graphics poGraphic)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine, tDeposit;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tAmt;
            decimal cChange = 0;
            Font oFont = new Font("CordiaUPC", Convert.ToSingle(11.5), FontStyle.Regular);
            string tMsg = "";
            int nStartX = 0, nStrWidth = 0;
            Image oLogo;
            try
            {
                W_GETxDataTax(); //*Arm 62-11-12 -Select ข้อมูล Tax

                nWidth = Convert.ToInt32(poGraphic.VisibleClipBounds.Width);
                tLine = "--------------------------------------------------------------------------------------------";

                if (cVB.nVB_PaperSize == 1)
                {
                    if (nWidth > 215)
                        nWidth = 215;
                }
                else
                {
                    if (nWidth > 280)
                        nWidth = 280;
                }

                oGraphic = poGraphic;
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };
                oMsg = new cSlipMsg();

                //*Em 62-10-29
                if (!string.IsNullOrEmpty(cVB.tVB_PathLogo))
                {
                    if (File.Exists(cVB.tVB_PathLogo))
                    {
                        oLogo = Image.FromFile(cVB.tVB_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle(nWidth - 200, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }
                //+++++++++++++++++

                //ใบกำกับภาษีเต็มรูป
                tMsg = oW_Resource.GetString("tTax");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                //ชื่อบริษัท
                nStartY += 18;
                if (string.IsNullOrEmpty(cVB.tVB_Merchart))
                {
                    tMsg = cVB.tVB_CmpName;
                }
                else
                {
                    tMsg = cVB.tVB_MerName;
                }

                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                //ที่อยู่บริษัท
                nStartY += 18;
                tMsg = tW_Addr;
                if (oGraphic.MeasureString(tMsg, oFont).Width > nWidth)
                {
                    string[] aMsg = tMsg.Split(' ');
                    tMsg = "";
                    for (int nIndex = 0; nIndex < aMsg.Length; nIndex++)
                    {
                        if (tMsg == "")
                        {
                            tMsg = aMsg[nIndex];
                        }
                        else
                        {
                            if (oGraphic.MeasureString(tMsg + " " + aMsg[nIndex], oFont).Width > nWidth)
                            {
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                                tMsg = aMsg[nIndex];

                            }
                            else
                            {
                                tMsg += " " + aMsg[nIndex];
                            }
                        }
                    }
                    if (tMsg != "")
                    {
                        nStartY += 18;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                    }
                }
                else
                {
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                }

                //เลขที่ประจำตัวผู้เสียภาษี(บริษัท)
                nStartY += 18;
                tMsg = oW_Resource.GetString("tTaxID") + " : " + cVB.tVB_TaxID;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                //---------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //ชื่อลูกค้า
                nStartY += 18;
                tMsg = oW_Resource.GetString("tCstName") + " : " + otbCstName.Text;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));
                //ที่อยู่ลูกค้า
                nStartY += 18;
                tMsg = oW_Resource.GetString("tAddress") + " : " + otbAddr1.Text + otbAddr2.Text;
                if (oGraphic.MeasureString(tMsg, oFont).Width > nWidth)
                {
                    string[] aMsg = tMsg.Split(' ');
                    tMsg = "";
                    for (int nIndex = 0; nIndex < aMsg.Length; nIndex++)
                    {
                        if (tMsg == "")
                        {
                            tMsg = aMsg[nIndex];
                        }
                        else
                        {
                            if (oGraphic.MeasureString(tMsg + " " + aMsg[nIndex], oFont).Width > nWidth)
                            {
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));
                                tMsg = "          " + aMsg[nIndex];

                            }
                            else
                            {
                                tMsg += " " + aMsg[nIndex];
                            }
                        }
                    }
                    if (tMsg != "")
                    {
                        nStartY += 18;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));
                    }
                }
                else
                {
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));
                }

                //โทรศัพท์
                nStartY += 18;
                tMsg = oW_Resource.GetString("tTel") + " : " + otbTel.Text;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                //เลขที่ประจำตัวผู้เสียภาษี(ลูกค้า)
                nStartY += 18;
                tMsg = oW_Resource.GetString("tTaxID") + " : " + otbIDCard.Text;
                if (ocbBsnType.SelectedIndex == 1)
                {
                    tMsg += " " + oW_Resource.GetString("tHQ");
                }
                else
                {
                    if (ocbEstab.SelectedIndex == 0)
                    {
                        tMsg += " " + oW_Resource.GetString("tHQ");
                    }
                    else
                    {
                        tMsg += " " + oW_Resource.GetString("tBranch") + " " + otbBchCode.Text;
                    }

                }
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                //----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //เลขที่เอกสาร, วันที่
                nStartY += 18;
                tMsg = oW_Resource.GetString("tNo") + " : " + oW_TaxHD.FTXshDocNo;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                tMsg = oW_Resource.GetString("tDate") + " : " + Convert.ToDateTime(oW_TaxHD.FDXshDocDate).ToString("dd/MM/yyyy");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(140, nStartY, nWidth, 18));

                //เครื่องจุดขาย,แคชเชียร์,พนักงานขาย
                nStartY += 18;
                nStartX = 0;
                tMsg = oW_Resource.GetString("tPos") + " : " + oW_TaxHD.FTPosCode.ToString();
                nStrWidth = (int)oGraphic.MeasureString(tMsg, oFont).Width + 2;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(nStartX, nStartY, nStrWidth, 18));

                nStartX += nStrWidth;
                tMsg = oW_Resource.GetString("tCasheir") + " : " + oW_TaxHD.FTUsrCode;
                nStrWidth = (int)oGraphic.MeasureString(tMsg, oFont).Width + 2;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(nStartX, nStartY, nStrWidth, 18));

                nStartX += nStrWidth;
                tMsg = oW_Resource.GetString("tSaleman") + " : " + oW_TaxHD.FTSpnCode;
                nStrWidth = (int)oGraphic.MeasureString(tMsg, oFont).Width + 2;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(nStartX, nStartY, nStrWidth, 18));

                //เอกสารอ้างอิง
                nStartY += 18;
                tMsg = oW_Resource.GetString("tRefDoc") + " : " + otbSlipNo.Text;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                //----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //หัวรายการ
                nStartY += 18;
                tMsg = oW_Resource.GetString("tItem");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 80, 18));
                tMsg = oW_Resource.GetString("tQty");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(70, nStartY, 40, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tPrice");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(110, nStartY, 50, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tDis");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(160, nStartY, 50, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tAmt");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(210, nStartY, 60, 18), oFormatFar);
                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รายการสินค้า
                foreach (cmlTPSTTaxDT oDT in aoW_TaxDT)
                {
                    nStartY += 18;
                    tMsg = oDT.FTXsdPdtName;
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 80, 18));
                    tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(70, nStartY, 40, 18), oFormatFar);
                    tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdSetPrice), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(110, nStartY, 50, 18), oFormatFar);
                    tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdDis), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(160, nStartY, 50, 18), oFormatFar);
                    tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdNet), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(210, nStartY, 60, 18), oFormatFar);
                }

                //รายการชำระ
                foreach (cmlTPSTTaxRC oRC in aoW_TaxRC)
                {
                    nStartY += 18;
                    tMsg = oRC.FTRcvName + " : " + oSP.SP_SETtDecShwSve(1, (decimal)(oRC.FCXrcNet), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth / 2, 18));
                }

                //------------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //จำนวนรวม,มูลค่ารวม
                nStartY += 18;
                tMsg = oW_Resource.GetString("tSumQty");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 65, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(cW_SumQty), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(65, nStartY, 65, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tTotalAmt");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130, nStartY, 90, 18));
                tMsg = olaAmtNet.Text;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(220, nStartY, 50, 18), oFormatFar);

                //ยกเว้นภาษี, ส่วนลด
                nStartY += 18;
                tMsg = oW_Resource.GetString("tNonVat");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 65, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshAmtNV), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(65, nStartY, 65, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tDis");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130, nStartY, 90, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshDis), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(220, nStartY, 50, 18), oFormatFar);

                //มีภาษี ,มัดจำ
                nStartY += 18;
                tMsg = oW_Resource.GetString("tWithTax");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 65, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshAmtV), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(65, nStartY, 65, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tDeposit");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130, nStartY, 90, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshRefAEAmt == null ? 0 : oW_TaxHD.FCXshRefAEAmt), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(220, nStartY, 50, 18), oFormatFar);

                //ก่อนภาษี ,ภาษี
                nStartY += 18;
                tMsg = oW_Resource.GetString("tB4Vat");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 65, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshVatable), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(65, nStartY, 65, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tVat") + "" + cVB.cVB_VatRate + "%";
                if (oGraphic.MeasureString(tMsg, oFont).Width < 90)
                {
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130, nStartY, 90, 18));
                    tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshVat), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(220, nStartY, 50, 18), oFormatFar);
                }
                else
                {
                    nStrWidth = (int)oGraphic.MeasureString(tMsg, oFont).Width + 3;
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130, nStartY, nStrWidth, 18));
                    tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshVat), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130 + nStrWidth, nStartY, 270 - (130 + nStrWidth), 18), oFormatFar);
                }


                //รวมจำนวเงินทั้งสิน
                nStartY += 18;
                tMsg = oW_Resource.GetString("tSumAmt");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(80, nStartY, 140, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshGrand - oW_TaxHD.FCXshRnd), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(220, nStartY, 50, 18), oFormatFar);

                //ยอดเงิน(ตัวอักษร)
                nStartY += 18;
                if (cVB.nVB_Language == 1)
                {
                    tMsg = cSale.C_GETtGndTextTH(Convert.ToString(oW_TaxHD.FCXshGrand - oW_TaxHD.FCXshRnd));
                }
                else
                {
                    tMsg = cSale.C_GETtGndTextEN(Convert.ToDecimal(oW_TaxHD.FCXshGrand - oW_TaxHD.FCXshRnd));
                }
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                //-------------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //ผู้อนุมัติ
                nStartY += 18;
                tMsg = oW_Resource.GetString("tApv");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 140, 18), oFormatCenter);

                //......................... ,ส่งโดย .......................
                nStartY += 18;
                tMsg = "........................................";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 140, 18), oFormatCenter);
                tMsg = oW_Resource.GetString("tSend");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(140, nStartY, 50, 18));
                tMsg = "...................................";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(190, nStartY, 80, 18));
                //(                        ) ,วันที่ ...../...../..........
                nStartY += 18;
                tMsg = "(                    )";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 140, 18), oFormatCenter);
                tMsg = oW_Resource.GetString("tDate");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(140, nStartY, 50, 18));
                tMsg = "......../......../.................";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(190, nStartY, 80, 18));

                //        ตัวบรรจง            ,รับของโดย ....................
                nStartY += 18;
                tMsg = oW_Resource.GetString("tElaborate");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 140, 18), oFormatCenter);
                tMsg = oW_Resource.GetString("tReceive");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(140, nStartY, 50, 18));
                tMsg = "...................................";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(190, nStartY, 80, 18));

                //วันที่ ...../...../......... ,วันที่ ...../...../..........
                nStartY += 18;
                tMsg = oW_Resource.GetString("tDate") + "   ......../......../............";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 140, 18), oFormatCenter);
                tMsg = oW_Resource.GetString("tDate");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(140, nStartY, 50, 18));
                tMsg = "......../......../.................";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(190, nStartY, 80, 18));

                //-------------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //if (W_CHKbIsPrintCopy())
                if (cVB.bVB_PrnTaxInvoiceCopy)
                {
                    nStartY += 18;
                    tMsg = "!!! " + oW_Resource.GetString("tCopy") + " " + oW_TaxHD.FNXshDocPrint + " !!!"; //*Arm 62-11-12 จำนวนครั้งที่พิมพ์สำเนา
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                }
                //ท้ายใบเสร็จ
                nStartY += 18;
                nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg

                cVB.nVB_StartY = nStartY; //*Arm 63-05-03
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_PRNxDrawTax : " + oEx.Message); }
            finally
            {
                if (oGraphic != null)
                    oGraphic.Dispose();

                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();

                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                oMsg = null;
                tLine = null;
                oSP.SP_CLExMemory();
            }
        }

        public void W_PRCxLock(bool pbLock)
        {
            try
            {
                if (pbLock)
                {
                    ocmBrwCst.Enabled = false;
                    ocmBrwCst.BackColor = SystemColors.Control;
                    ocmBrwSlip.Enabled = false;
                    ocmBrwSlip.BackColor = SystemColors.Control;
                    otbSlipNo.ReadOnly = true;
                    otbSlipNo.BackColor = otbTaxNo.BackColor;
                }
                else
                {
                    ocmBrwCst.Enabled = true;
                    ocmBrwCst.BackColor = cVB.oVB_ColDark;
                    ocmBrwSlip.Enabled = true;
                    ocmBrwSlip.BackColor = cVB.oVB_ColDark;
                    otbSlipNo.ReadOnly = false;
                    otbSlipNo.BackColor = Color.White;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_PRCxLock : " + oEx.Message); }
        }

        public void W_PRCxPrintUpd()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                oSql.AppendLine("UPDATE TPSTTaxHD WITH(ROWLOCK)");
                oSql.AppendLine("SET FNXshDocPrint = ISNULL(FNXshDocPrint,0) + 1");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_PRCxPrintUpd : " + oEx.Message); }
        }

        public bool W_CHKbIsPrintCopy()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            bool bCopy = false;
            int nPrn;
            try
            {
                oSql.AppendLine("SELECT ISNULL(FNXshDocPrint,0) AS FNXshDocPrint FROM TPSTTaxHD WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                nPrn = oDB.C_GEToDataQuery<int>(oSql.ToString());

                if (nPrn > 0) bCopy = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_PRCxPrintUpd : " + oEx.Message); }
            return bCopy;
        }
        public void W_GETxTaxAddrCst()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTCNMTaxAddress> aoTaxAddr;
            try
            {
                if (oW_CstAddr != null) return;

                oSql.AppendLine("SELECT *");
                oSql.AppendLine("FROM TCNMTaxAddress_L");
                oSql.AppendLine("WHERE 1 = 1");
                if (!string.IsNullOrEmpty(otbIDCard.Text))
                {
                    oSql.AppendLine("AND FTAddTaxNo = '" + otbIDCard.Text + "'");
                }
                else
                {
                    if (!string.IsNullOrEmpty(otbTel.Text)) oSql.AppendLine("AND FTAddTel = '" + otbTel.Text + "'");
                }
                aoTaxAddr = oDB.C_GETaDataQuery<cmlTCNMTaxAddress>(oSql.ToString());
                if (aoTaxAddr != null)
                {
                    if (aoTaxAddr.Count == 1)
                    {
                        oW_CstAddr = aoTaxAddr[0];
                    }
                    else
                    {
                        oW_CstAddr = aoTaxAddr.Where(o => o.FNLngID == cVB.nVB_Language).FirstOrDefault();
                    }
                    if (oW_CstAddr != null)
                    {
                        otbCstTax.Text = oW_CstAddr.FTAddName;
                        otbAddr1.Text = oW_CstAddr.FTAddV2Desc1;
                        otbAddr2.Text = oW_CstAddr.FTAddV2Desc2;
                        otbIDCard.Text = oW_CstAddr.FTAddTaxNo;
                        otbTel.Text = oW_CstAddr.FTAddTel;
                        otbFax.Text = oW_CstAddr.FTAddFax;
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaBusiness))
                        {
                            ocbBsnType.SelectedIndex = (oW_CstAddr.FTAddStaBusiness == "1" ? 0 : 1);
                        }
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaHQ))
                        {
                            ocbEstab.SelectedIndex = (oW_CstAddr.FTAddStaHQ == "1" ? 0 : 1);
                            if (ocbEstab.SelectedIndex == 1)
                            {
                                if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaBchCode))
                                {
                                    otbBchCode.Text = oW_CstAddr.FTAddStaBchCode;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_GETxTaxAddrCst : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                oW_SP.SP_CLExMemory();
            }
        }
        private void W_SETxDefualt()
        {
            try
            {
                otbSlipNo.Text = "";
                otbCstName.Text = "";
                otbCstName.ReadOnly = true;
                otbTaxNo.Text = cSale.C_GETtFormatDoc("TPSTTaxHD", 4, DateTime.Now.Date, cVB.tVB_PosCode, cVB.tVB_ShpCode);
                otbTaxNo.ReadOnly = true;
                otbIDCard.Text = "";
                otbCstTax.Text = "";
                otbTel.Text = "";
                otbFax.Text = "";
                otbRemark.Text = "";
                otbAddr1.Text = "";
                otbAddr2.Text = "";
                ocbBsnType.SelectedIndex = 0;
                otbBchCode.Text = "";

                ogdPdtDetail.Rows.Clear();

                olaAmtTotal.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaAmtDisChg.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaAmtAftDisChg.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaAmtTotalVat.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                olaAmtNet.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);

                tW_TaxDocNo = "";
                bW_StaApv = false;
                oW_CstAddr = null;
                W_PRCxLock(false);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "W_SETxDefualt : " + oEx.Message); }
        }

        private void W_KBDxTaxApvDoc()
        {
            string tDocNo;
            cTax oTax = new cTax();
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                if (string.IsNullOrEmpty(otbSlipNo.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputDoc"), 3);
                    otbSlipNo.Focus();
                    return;
                }


                if (string.IsNullOrEmpty(otbCstName.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputName"), 3);
                    otbCstName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(otbIDCard.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputTaxNo"), 3);
                    otbIDCard.Focus();
                    return;
                }

                if (bW_StaApv)
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgApproved"), 3);
                    return;
                }
                tW_TaxDocNo = oTax.C_GENtDocNo(oW_TaxHD.FNXshDocType.Value);
                if (!string.IsNullOrEmpty(tW_TaxDocNo))
                {
                    otbTaxNo.Text = tW_TaxDocNo;
                    if (oTax.C_DOCbInsertTaxWithSQL(tW_TaxDocNo, otbSlipNo.Text.Trim()))
                    {
                        oSql.AppendLine("UPDATE TPSTTaxHD WITH(ROWLOCK)");
                        oSql.AppendLine("SET FTCstCode = '" + tW_CstCode + "'");
                        oSql.AppendLine(",FTXshStaApv = '1'");
                        oSql.AppendLine(",FTXshRmk = '"+ otbRemark.Text +"'");  //*Em 63-05-05
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());

                        oSql = new StringBuilder();
                        oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK)");
                        oSql.AppendLine("SET FTXshDocVatFull = '" + tW_TaxDocNo + "'");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + otbSlipNo.Text.Trim() + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());

                        if (oW_TaxHDCst == null)
                        {
                            oW_TaxHDCst = new cmlTPSTTaxHDCst();
                            oW_TaxHDCst.FTBchCode = cVB.tVB_BchCode;
                            oW_TaxHDCst.FTXshDocNo = tW_TaxDocNo;
                            oW_TaxHDCst.FTXshCardID = otbIDCard.Text;
                            oW_TaxHDCst.FTXshCstName = otbCstTax.Text; //*Em 62-12-18
                            oW_TaxHDCst.FTXshCstTel = otbTel.Text;  //*Em 62-12-18
                            if (oW_CstAddr == null)
                            {
                                oW_TaxHDCst.FNXshAddrTax = 0;
                            }
                            else
                            {
                                if (oW_CstAddr.FNAddSeqNo != 0)
                                {
                                    oW_TaxHDCst.FNXshAddrTax = oW_CstAddr.FNAddSeqNo;
                                }
                                else
                                {
                                    oW_TaxHDCst.FNXshAddrTax = 0;
                                }
                            }

                            oTax.C_DATxInsertHDCst(oW_TaxHDCst);
                        }
                        else
                        {
                            oSql.AppendLine("UPDATE TPSTTaxHDCst WITH(ROWLOCK)");
                            oSql.AppendLine("SET FTXshCardID = '" + otbIDCard.Text + "'");
                            oSql.AppendLine(",FTXshCstName = '" + otbCstTax.Text.Trim() + "'");
                            oSql.AppendLine(",FTXshCstTel = '" + otbTel.Text.Trim() + "'"); //*Em 62-12-18
                            oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }

                        oSql.AppendLine("IF EXISTS(SELECT FTAddTaxNo FROM TCNMTaxAddress_L WITH(NOLOCK) WHERE FTAddTaxNo = '" + otbIDCard.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language + ")");
                        oSql.AppendLine("BEGIN");
                        oSql.AppendLine("	UPDATE TCNMTaxAddress_L WITH(ROWLOCK)");
                        oSql.AppendLine("	SET FTAddV2Desc1 ='" + otbAddr1.Text.Trim() + "'");
                        oSql.AppendLine("	,FTAddV2Desc2 ='" + otbAddr2.Text.Trim() + "'");
                        oSql.AppendLine("	,FTAddStaBusiness = '" + (ocbBsnType.SelectedIndex + 1) + "'");
                        oSql.AppendLine("	,FTAddStaHQ = '" + (ocbEstab.SelectedIndex == 0 ? "1" : "2") + "'");
                        oSql.AppendLine("	,FTAddStaBchCode = '" + (ocbEstab.SelectedIndex == 0 ? "" : otbBchCode.Text.Trim()) + "'");
                        oSql.AppendLine("	,FDLastUpdOn = GETDATE()");
                        oSql.AppendLine("	,FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                        oSql.AppendLine("   ,FTAddName = '" + otbCstTax.Text + "'");
                        oSql.AppendLine("   ,FTAddTel = '" + otbTel.Text + "'");
                        oSql.AppendLine("   ,FTAddFax = '" + otbFax.Text + "'");
                        oSql.AppendLine("	WHERE FTAddTaxNo = '" + otbIDCard.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language);

                        oSql.AppendLine("UPDATE TPSTTaxHDCst WITH(ROWLOCK)");
                        oSql.AppendLine("SET FNXshAddrTax = (");
                        oSql.AppendLine("SELECT FNAddSeqNo FROM TCNMTaxAddress_L WHERE FTAddTaxNo = '" + otbIDCard.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language+")");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");

                        oSql.AppendLine("END");
                        oSql.AppendLine("ELSE");
                        oSql.AppendLine("BEGIN");
                        oSql.AppendLine("	INSERT INTO TCNMTaxAddress_L WITH(ROWLOCK)(FTAddTaxNo,FNLngID,FTAddVersion,FTAddV2Desc1,FTAddV2Desc2,FTAddStaBusiness,FTAddStaHQ,FTAddStaBchCode");
                        oSql.AppendLine("   ,FTAddName,FTAddTel,FTAddFax"); //*Em 62-10-10
                        oSql.AppendLine("	,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                        oSql.AppendLine("	VALUES('" + otbIDCard.Text.Trim() + "'," + cVB.nVB_Language + ",'2','" + otbAddr1.Text.Trim() + "','" + otbAddr2.Text.Trim() + "','" + (ocbBsnType.SelectedIndex + 1) + "','" + (ocbEstab.SelectedIndex == 0 ? "1" : "2") + "','" + (ocbEstab.SelectedIndex == 0 ? "" : otbBchCode.Text.Trim()) + "',");
                        oSql.AppendLine("   '" + otbCstTax.Text + "','" + otbTel.Text + "','" + otbFax.Text + "',"); //*Em 62-10-10
                        oSql.AppendLine("   GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");

                        oSql.AppendLine("UPDATE TPSTTaxHDCst WITH(ROWLOCK)");
                        oSql.AppendLine("SET FNXshAddrTax = (");
                        oSql.AppendLine("SELECT FNAddSeqNo FROM TCNMTaxAddress_L WHERE FTAddTaxNo = '" + otbIDCard.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language + ")");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");

                        oSql.AppendLine("END");
                        oDB.C_SETxDataQuery(oSql.ToString());

                        cSale.C_PRCxAdd2TmpLogChg(90, tW_TaxDocNo); //*Em 62-08-13
                        new cSyncData().C_PRCxSyncUld();    //*Em 62-08-13
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgPrcComplete"), 1);

                        W_GETxDataTax();
                        W_PRCxLock(true);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocmAccept_Click : " + oEx.Message); }
        }

        private void W_KBDxTaxPrintDoc()
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            try
            {
                if (string.IsNullOrEmpty(tW_TaxDocNo))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgNotPrint"), 3);
                    return;
                } 

                wTaxSelectPrint oPrn = new wTaxSelectPrint();
                oPrn.ShowDialog();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocmPrint_Click : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                oSP.SP_CLExMemory();
            }
        }
        private void W_KBDxTaxFindDoc()
        {
            try
            {
                cVB.tVB_DocNo = "";
                new wTaxInvoiceSearch(2).ShowDialog();
                if (!string.IsNullOrEmpty(cVB.tVB_DocNo))
                {
                    otbTaxNo.Text = cVB.tVB_DocNo;
                    tW_TaxDocNo = cVB.tVB_DocNo;
                    W_GETxDataTax();
                    W_SETxData(2);
                    bW_StaApv = true;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocmBrwSlip_Click " + oEx.Message); }
        }
        #endregion Funciton

        #region Method / Events
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
                new cLog().C_WRTxLog("wTax", "W_Notification : " + oEx.Message);
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
                switch (tFuncName)
                {
                    case "C_KBDxBack":
                        try
                        {
                           new wHome().Show();
                           this.Close();
                        }
                        catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocmMenuBar_Click " + oEx.Message); }
                        break;
                    case "C_KBDxTaxNewDoc":
                        W_SETxDefualt();
                        break;
                    case "C_KBDxTaxFindDoc":
                        W_KBDxTaxFindDoc();
                        break;
                    case "C_KBDxTaxApvDoc":
                        W_KBDxTaxApvDoc();
                        break;
                    case "C_KBDxTaxPrintDoc":
                        W_KBDxTaxPrintDoc();
                        break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocmMenuBar_Click " + oEx.Message); }
        }
        private void ocmBrwSlip_Click(object sender, EventArgs e)
        {
            try
            {
                cVB.tVB_DocNo = "";
                new wTaxInvoiceSearch(1).ShowDialog();
                if (!string.IsNullOrEmpty(cVB.tVB_DocNo))
                {
                    otbSlipNo.Text = cVB.tVB_DocNo;
                    W_GETxData();
                    W_SETxData(1);
                }
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocmBrwSlip_Click " + oEx.Message); }
        }

        private void wTax_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;
            try
            {
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                switch (tFuncName)
                {
                    case "C_KBDxBack":
                        try
                        {
                            new wHome().Show();
                            this.Close();
                        }
                        catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "wTax_KeyDown " + oEx.Message); }
                        break;
                    case "C_KBDxTaxNewDoc": W_SETxDefualt(); break;
                    case "C_KBDxTaxFindDoc": W_KBDxTaxFindDoc(); break;
                    case "C_KBDxTaxApvDoc": W_KBDxTaxApvDoc(); break;
                    case "C_KBDxTaxPrintDoc": W_KBDxTaxPrintDoc(); break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "wTax_KeyDown " + oEx.Message); }
        }

        private void otbIDCard_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!string.IsNullOrEmpty(otbIDCard.Text)) W_GETxTaxAddrCst();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "otbIDCard_KeyDown : " + oEx.Message); }
            finally
            {
            }
        }

        private void otbIDCard_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(otbIDCard.Text)) W_GETxTaxAddrCst();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "otbIDCard_Leave : " + oEx.Message); }
            finally
            {
            }
        }
        private void otbTel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!string.IsNullOrEmpty(otbTel.Text)) W_GETxTaxAddrCst();
                    SendKeys.Send("{TAB}");
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "otbTel_KeyDown : " + oEx.Message); }
            finally
            {
            }
        }

        private void otbTel_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(otbTel.Text)) W_GETxTaxAddrCst();
                SendKeys.Send("{TAB}");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "otbTel_Leave : " + oEx.Message); }
            finally
            {
            }
        }
        private void ocbBsnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ocbBsnType.SelectedIndex == 0)   // นิติบุคคล
                {
                    ocbEstab.Enabled = true;
                    ocbEstab.SelectedIndex = 0;
                    otbBchCode.Text = string.Empty;
                    otbBchCode.ReadOnly = true;
                    otbBchCode.Enabled = false;
                }
                else    // บุคคลธรรมดา
                {
                    ocbEstab.Enabled = false;
                    ocbEstab.SelectedIndex = 0;
                    otbBchCode.Text = string.Empty;
                    otbBchCode.ReadOnly = true;
                    otbBchCode.Enabled = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocbBsnType_SelectedIndexChanged : " + oEx.Message); }
        }

        private void ocbEstab_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ocbEstab.SelectedIndex == 0)   // HQ
                {
                    otbBchCode.Text = string.Empty;
                    otbBchCode.ReadOnly = true;
                    otbBchCode.Enabled = false;
                }
                else    // สาขา
                {
                    otbBchCode.Text = string.Empty;
                    otbBchCode.ReadOnly = false;
                    otbBchCode.Enabled = true;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocbEstab_SelectedIndexChanged : " + oEx.Message); }
        }
        private void ocmBrwCst_Click(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                wSearch2Column oFrmCst;
                oFrmCst = new wSearch2Column("TCNMCst");
                oFrmCst.ShowDialog();
                if (!string.IsNullOrEmpty(oFrmCst.oW_DataSearch.tCode))
                {
                    tW_CstCode = oFrmCst.oW_DataSearch.tCode;
                    otbCstName.Text = oFrmCst.oW_DataSearch.tName;
                    otbCstTax.Text = oFrmCst.oW_DataSearch.tName;
                    oSql.AppendLine("SELECT TOP 1 ADR.* FROM TCNMTaxAddress_L ADR WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TCNMCst CST WITH(NOLOCK) ON ADR.FTAddTaxNo = ISNULL(CST.FTCstTaxNo,'') AND CST.FTCstCode ='" + tW_CstCode + "' ");
                    oSql.AppendLine("WHERE ADR.FNLngID = " + cVB.nVB_Language);
                    oW_CstAddr = oDB.C_GEToDataQuery<cmlTCNMTaxAddress>(oSql.ToString());
                    if (oW_CstAddr != null)
                    {
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddTaxNo))
                        {
                            otbIDCard.Text = oW_CstAddr.FTAddTaxNo;
                        }
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddV2Desc1))
                        {
                            otbAddr1.Text = oW_CstAddr.FTAddV2Desc1;
                        }
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddV2Desc2))
                        {
                            otbAddr2.Text = oW_CstAddr.FTAddV2Desc2;
                        }
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaBusiness))
                        {
                            ocbBsnType.SelectedIndex = (oW_CstAddr.FTAddStaBusiness == "1" ? 0 : 1);
                        }
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaHQ))
                        {
                            ocbEstab.SelectedIndex = (oW_CstAddr.FTAddStaHQ == "1" ? 0 : 1);
                            if (ocbEstab.SelectedIndex == 1)
                            {
                                if (!string.IsNullOrEmpty(oW_CstAddr.FTAddStaBchCode))
                                {
                                    otbBchCode.Text = oW_CstAddr.FTAddStaBchCode;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddTel))
                        {
                            otbTel.Text = oW_CstAddr.FTAddTel;
                        }
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax:", "ocmBrwCst_Click : " + oEx.Message); }
        }

        private void wTax_Shown(object sender, EventArgs e)
        {
            cVB.oVB_TaxInvoice = this;
        }
        private void ocmBrwAddr_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tW_CstCode))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputName"), 3);
                    otbCstName.Focus();
                    return;
                }
                cVB.oVB_TaxAddr = null;
                new wTaxAddrSearch().ShowDialog();
                if (cVB.oVB_TaxAddr != null)
                {
                    otbIDCard.Text = cVB.oVB_TaxAddr.FTAddTaxNo.ToString();
                    otbCstTax.Text = cVB.oVB_TaxAddr.FTAddName == null ? "" : cVB.oVB_TaxAddr.FTAddName;
                    otbTel.Text = cVB.oVB_TaxAddr.FTAddTel == null ? "" : cVB.oVB_TaxAddr.FTAddTel;
                    otbFax.Text = cVB.oVB_TaxAddr.FTAddFax == null ? "" : cVB.oVB_TaxAddr.FTAddFax;
                    otbAddr1.Text =  cVB.oVB_TaxAddr.FTAddV2Desc1 == null ? "" : cVB.oVB_TaxAddr.FTAddV2Desc1;
                    otbAddr2.Text =  cVB.oVB_TaxAddr.FTAddV2Desc2 == null ? "" : cVB.oVB_TaxAddr.FTAddV2Desc2;
                    ocbBsnType.SelectedIndex = (cVB.oVB_TaxAddr.FTAddStaBusiness == null ? 0 : Convert.ToInt32(cVB.oVB_TaxAddr.FTAddStaBusiness) - 1);
                    ocbEstab.SelectedIndex = (cVB.oVB_TaxAddr.FTAddStaHQ == null ? 0 : Convert.ToInt32(cVB.oVB_TaxAddr.FTAddStaHQ)-1);
                    otbBchCode.Text = cVB.oVB_TaxAddr.FTAddStaBchCode==null?"":cVB.oVB_TaxAddr.FTAddStaBchCode;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocmBrwAddr_Click : " + oEx.Message); }
        }
        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax:", "ocmNotify_Click : " + oEx.Message); }
        }
        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnMenu.Width <= 100)
                {
                    opnMenu.Width = 270;
                }
                else
                {
                    opnMenu.Width = 50;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTax", "ocmMenu_Click " + oEx.Message); }
        }
        private void ocmMenu_MouseLeave(object sender, EventArgs e)
        {

        }
        private void ocmSave_Click(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(otbIDCard.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputTaxNo"), 3);
                    otbIDCard.Focus();
                    return;
                }

                oSql.AppendLine("IF EXISTS(SELECT FTAddTaxNo FROM TCNMTaxAddress_L WITH(NOLOCK) WHERE FTAddTaxNo = '" + otbIDCard.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language + ")");
                oSql.AppendLine("BEGIN");
                oSql.AppendLine("	UPDATE TCNMTaxAddress_L WITH(ROWLOCK)");
                oSql.AppendLine("	SET FTAddV2Desc1 ='" + otbAddr1.Text.Trim() + "'");
                oSql.AppendLine("	,FTAddV2Desc2 ='" + otbAddr2.Text.Trim() + "'");
                oSql.AppendLine("	,FTAddStaBusiness = '" + (ocbBsnType.SelectedIndex + 1) + "'");
                oSql.AppendLine("	,FTAddStaHQ = '" + (ocbEstab.SelectedIndex == 0 ? "1" : "2") + "'");
                oSql.AppendLine("	,FTAddStaBchCode = '" + (ocbEstab.SelectedIndex == 0 ? "" : otbBchCode.Text.Trim()) + "'");
                oSql.AppendLine("	,FDLastUpdOn = GETDATE()");
                oSql.AppendLine("	,FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine("   ,FTAddName = '" + otbCstTax.Text + "'");
                oSql.AppendLine("   ,FTAddTel = '" + otbTel.Text + "'");
                oSql.AppendLine("   ,FTAddFax = '" + otbFax.Text + "'");
                oSql.AppendLine("	WHERE FTAddTaxNo = '" + otbIDCard.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("");
                oSql.AppendLine("   UPDATE TPSTTaxHDCst WITH(ROWLOCK)");
                oSql.AppendLine("   SET FTXshCardID = '" + otbIDCard.Text + "'");
                oSql.AppendLine("   WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                oSql.AppendLine("	");
                oSql.AppendLine("END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("BEGIN");
                oSql.AppendLine("	INSERT INTO TCNMTaxAddress_L WITH(ROWLOCK)(FTAddTaxNo,FNLngID,FTAddVersion,FTAddV2Desc1,FTAddV2Desc2,FTAddStaBusiness,FTAddStaHQ,FTAddStaBchCode");
                oSql.AppendLine("   ,FTAddName,FTAddTel,FTAddFax"); //*Em 62-10-10
                oSql.AppendLine("	,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                oSql.AppendLine("	VALUES('" + otbIDCard.Text.Trim() + "'," + cVB.nVB_Language + ",'2','" + otbAddr1.Text.Trim() + "','" + otbAddr2.Text.Trim() + "','" + (ocbBsnType.SelectedIndex + 1) + "','" + (ocbEstab.SelectedIndex == 0 ? "1" : "2") + "','" + (ocbEstab.SelectedIndex == 0 ? "" : otbBchCode.Text.Trim()) + "',");
                oSql.AppendLine("   '" + otbCstTax.Text + "','" + otbTel.Text + "','" + otbFax.Text + "',"); //*Em 62-10-10
                oSql.AppendLine("   GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                oSql.AppendLine("END");
                oDB.C_SETxDataQuery(oSql.ToString());
                new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgSave"), 1);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmSaveAddr_Click : " + oEx.Message); }
        }
        #endregion Method / Events

        private void wTax_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
