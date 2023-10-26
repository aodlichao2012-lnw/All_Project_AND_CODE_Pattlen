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

namespace AdaPos
{
    public partial class wTaxInvoice : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;
        //private cmlTPSTSalHD oW_HD;
        //private cmlTPSTSalHDCst oW_HDCst;
        //private List<cmlTPSTSalDT> aoW_DT;

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
        private int nW_Mode;                            //1:Wristband / Card 2:Document
        
        #endregion End Variable

        public wTaxInvoice(int pnMode)
        {
            InitializeComponent();

            try
            {
                nW_Mode = pnMode;
                oW_SP = new cSP();
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();
                W_GETxData();
                W_SETxData();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "wTaxInvoice : " + oEx.Message); }
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
                //ogdPdtDetail.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdPdtDetail); //*Net 63-03-03 Set Design Gridview
                ocmSearchCst.BackColor = cVB.oVB_ColNormal;
                ocmTax.BackColor = cVB.oVB_ColNormal;
                ocmPrint.BackColor = cVB.oVB_ColNormal;

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmSaveAddr.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_SETxDesign : " + oEx.Message); }
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

                cVB.tVB_KbdScreen = "TAXINVOICE";
                //*Em 62-09-09
                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                olaTax.Text = oW_Resource.GetString("tTax");
                switch (nW_Mode)
                {
                    case 1:
                        olaTitleNo.Text = oW_Resource.GetString("tWBNo");
                        break;
                    case 2:
                    default:
                        olaTitleNo.Text = oW_Resource.GetString("tDocNo");
                        break;
                }
                
                olaTitleCstNo.Text = oW_Resource.GetString("tCstCode");
                olaTitleCstName.Text = oW_Resource.GetString("tCstName");
                olaTitleAddress.Text = oW_Resource.GetString("tAddress");
                olaTitleTaxID.Text = oW_Resource.GetString("tTaxID");
                olaTitleBsnType.Text = oW_Resource.GetString("tBsnType");
                olaTitleEstab.Text = oW_Resource.GetString("tEstab");
                olaTitleBchCode.Text = oW_Resource.GetString("tBchCode");
                olaTitleRmk.Text = oW_Resource.GetString("tRemark");
                olaPdtDetail.Text = oW_Resource.GetString("tPdtDetail");
                olaTitleSum.Text = oW_Resource.GetString("tSum");
                otbTitleAmt.HeaderText = cVB.oVB_GBResource.GetString("tAmount");
                otbTitlePdtID.HeaderText = oW_Resource.GetString("tPdtID");
                otbTitlePdtName.HeaderText = oW_Resource.GetString("tPdtName");
                otbTitlePrice.HeaderText = oW_Resource.GetString("tPrice");
                otbTitleQty.HeaderText = cVB.oVB_GBResource.GetString("tQty");
                otbTitleSeq.HeaderText = oW_Resource.GetString("tSeq");
                olaTitleTel.Text = oW_Resource.GetString("tTel");
                olaTitleFax.Text = oW_Resource.GetString("tFax");

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

                otbTicketNo.Text = cVB.tVB_DocNo;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_SETxText : " + oEx.Message); }
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
                oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"' AND FTXshDocNo = '"+ cVB.tVB_DocNo +"'");
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
                    oSql.AppendLine("SELECT * ");
                    oSql.AppendLine("FROM TPSTSalDT WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_GETxData : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_GETxDataTax : " + oEx.Message); }
        }

        private void W_SETxData()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                if (oW_TaxHD != null)
                {
                    if (string.IsNullOrEmpty(oW_TaxHD.FTCstCode))
                    {
                        otbCstNo.Text = cVB.tVB_CstDef;
                        otbCstName.Text = cVB.tVB_CstDefName;
                    }
                    else
                    {
                        otbCstNo.Text = oW_TaxHD.FTCstCode.ToString();
                    }
                }
                else
                {
                    otbCstNo.Text = cVB.tVB_CstDef;
                    otbCstName.Text = cVB.tVB_CstDefName;
                }

                if (oW_TaxHDCst != null)
                {
                    otbTaxID.Text = oW_TaxHDCst.FTXshCardID;
                    otbCstName.Text = oW_TaxHDCst.FTXshCstName.ToString();
                    otbTel.Text = oW_TaxHDCst.FTXshCstTel;  //*Em 62-12-18
                    if (!string.IsNullOrEmpty(oW_TaxHDCst.FTXshCardID))
                    {
                        oSql.AppendLine("SELECT TOP 1 * FROM TCNMTaxAddress_L WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTAddTaxNo = '" + oW_TaxHDCst.FTXshCardID +"' AND FNLngID = " + cVB.nVB_Language);
                        oW_CstAddr = oDB.C_GEToDataQuery<cmlTCNMTaxAddress>(oSql.ToString());
                        if (oW_CstAddr != null)
                        {
                            if (!string.IsNullOrEmpty(oW_CstAddr.FTAddV2Desc1))
                            {
                                otbAddress.Text = oW_CstAddr.FTAddV2Desc1;
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
                        }
                    }
                }

                ogdPdtDetail.Rows.Clear();
                if (aoW_TaxDT.Count > 0)
                {
                    foreach (cmlTPSTTaxDT oDT in aoW_TaxDT)
                    {
                        ogdPdtDetail.Rows.Add(oDT.FNXsdSeqNo, oDT.FTPdtCode, oDT.FTXsdPdtName, oDT.FCXsdSetPrice, oDT.FCXsdQty, oDT.FCXsdNet);
                    }
                }

                olaSumQty.Text = ogdPdtDetail.Rows.Cast<DataGridViewRow>().Sum(oRow => Convert.ToDecimal(oRow.Cells["otbTitleQty"].Value)).ToString();
                olaSumAmt.Text = ogdPdtDetail.Rows.Cast<DataGridViewRow>().Sum(oRow => Convert.ToDecimal(oRow.Cells["otbTitleAmt"].Value)).ToString();

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
                            {   tW_Addr += oW_Addr.FTAddV1Soi; }
                            else {  tW_Addr += " " + oW_Addr.FTAddV1Soi; }  
                        }

                        if (oW_Addr.FTAddV1Village != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            {   tW_Addr += oW_Addr.FTAddV1Village; }
                            else
                            {tW_Addr += " " + oW_Addr.FTAddV1Village;   }
                        }

                        if (oW_Addr.FTAddV1Road != null)
                        {
                            if (string.IsNullOrEmpty(tW_Addr))
                            {tW_Addr += oW_Addr.FTAddV1Road; }
                            else  {   tW_Addr += " " + oW_Addr.FTAddV1Road; }
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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_GETxData : " + oEx.Message); }
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
            int nStartX = 0,nStrWidth=0;
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
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18),oFormatCenter);
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
                tMsg = oW_Resource.GetString("tAddress") + " : " + otbAddress.Text;
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
                            if (oGraphic.MeasureString(tMsg + " " + aMsg[nIndex],oFont).Width > nWidth)
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
                tMsg = oW_Resource.GetString("tTaxID") + " : " + otbTaxID.Text;
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

                tMsg = oW_Resource.GetString("tDate") + " : " + Convert.ToDateTime (oW_TaxHD.FDXshDocDate).ToString("dd/MM/yyyy");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(140, nStartY, nWidth, 18));

                //เครื่องจุดขาย,แคชเชียร์,พนักงานขาย
                nStartY += 18;
                nStartX = 0;
                tMsg = oW_Resource.GetString("tPos") + " : " + oW_TaxHD.FTPosCode.ToString() ;
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
                tMsg = oW_Resource.GetString("tRefDoc") + " : " + otbTicketNo.Text;
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
                    tMsg = oRC.FTRcvName + " : " + oSP.SP_SETtDecShwSve(1, (decimal)(oRC.FCXrcNet), cVB.nVB_DecShow) ;
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 80, 18));
                }

                //------------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //จำนวนรวม,มูลค่ารวม
                nStartY += 18;
                tMsg = oW_Resource.GetString("tSumQty");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 65, 18));
                tMsg = olaSumQty.Text;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(65, nStartY, 65, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tTotalAmt");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130, nStartY, 90, 18));
                tMsg = olaSumAmt.Text;
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
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshRefAEAmt == null ?0:oW_TaxHD.FCXshRefAEAmt), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(220, nStartY, 50, 18), oFormatFar);

                //ก่อนภาษี ,ภาษี
                nStartY += 18;
                tMsg = oW_Resource.GetString("tB4Vat");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 65, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshVatable), cVB.nVB_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(65, nStartY, 65, 18), oFormatFar);
                tMsg = oW_Resource.GetString("tVat")+ "" + cVB.cVB_VatRate + "%";
                if (oGraphic.MeasureString(tMsg, oFont).Width < 90)
                {
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130, nStartY, 90, 18));
                    tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshVat), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(220, nStartY, 50, 18), oFormatFar);
                }
                else
                {
                    nStrWidth = (int)oGraphic.MeasureString(tMsg, oFont).Width+3;
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130, nStartY, nStrWidth, 18));
                    tMsg = oSP.SP_SETtDecShwSve(1, (decimal)(oW_TaxHD.FCXshVat), cVB.nVB_DecShow);
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(130 + nStrWidth, nStartY, 270-(130+nStrWidth), 18), oFormatFar);
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
                    tMsg = cSale.C_GETtGndTextTH( Convert.ToString(oW_TaxHD.FCXshGrand - oW_TaxHD.FCXshRnd));
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
                tMsg = oW_Resource.GetString("tSend") ;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(140, nStartY, 50, 18));
                tMsg =  "...................................";
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
                tMsg = oW_Resource.GetString("tDate") ;
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(140, nStartY, 50, 18));
                tMsg = "......../......../.................";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(190, nStartY, 80, 18));

                //-------------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                if(cVB.bVB_PrnTaxInvoiceCopy) //*Net 63-02-25 พิมพ์ต้นฉบับ/สำเนา ตาม option
                {
                    nStartY += 18;
                    if (oW_TaxHD.FNXshDocPrint != 0)  //*Net 63-02-25 ถ้าพิมพ์ใบกำกับภาษีครั้งแรก ไม่แสดงเลขจำนวนครั้ง
                    {
                        tMsg = "!!! " + oW_Resource.GetString("tCopy") + " ครั้งที่ " + (oW_TaxHD.FNXshDocPrint+1).ToString() + " !!!"; //*Arm 62-11-12 จำนวนครั้งที่พิมพ์สำเนา
                    }
                    else
                    {
                        tMsg = "!!! " + oW_Resource.GetString("tCopy") + " !!!";
                    }
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                }
                //ท้ายใบเสร็จ
                nStartY += 18;
                nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg


            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_PRNxDrawTax : " + oEx.Message); }
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
                    otbCstNo.Enabled = false;
                    ocmSearchCst.Enabled = false;
                    ocmSearchCst.BackColor = SystemColors.Control;
                    ocmAccept.Enabled = false;
                    ocmAccept.BackColor = SystemColors.Control;
                    ocmTax.Enabled = true;
                    ocmTax.BackColor = cVB.oVB_ColDark;
                    ocmPrint.Enabled = true;
                    ocmPrint.BackColor = cVB.oVB_ColDark;
                }
                else
                {
                    otbCstNo.Enabled = true;
                    ocmSearchCst.Enabled = true;
                    ocmSearchCst.BackColor = cVB.oVB_ColDark;
                    ocmAccept.Enabled = true;
                    ocmAccept.BackColor = cVB.oVB_ColDark;
                    ocmTax.Enabled = false;
                    ocmTax.BackColor = SystemColors.Control;
                    ocmPrint.Enabled = false;
                    ocmPrint.BackColor = SystemColors.Control;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_PRCxLock : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_PRCxPrintUpd : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_PRCxPrintUpd : " + oEx.Message); }
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
                if (!string.IsNullOrEmpty(otbTaxID.Text))
                {
                    oSql.AppendLine("AND FTAddTaxNo = '" + otbTaxID.Text + "'");
                }
                else
                {
                    if (!string.IsNullOrEmpty(otbTel.Text)) oSql.AppendLine("AND FTAddTel = '"+ otbTel.Text +"'");
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
                        otbCstName.Text = oW_CstAddr.FTAddName;
                        if (String.IsNullOrEmpty(otbAddress.Text)) otbAddress.Text = oW_CstAddr.FTAddV2Desc1;
                        if (String.IsNullOrEmpty(otbTaxID.Text)) otbTaxID.Text = oW_CstAddr.FTAddTaxNo;
                        if (String.IsNullOrEmpty(otbTel.Text)) otbTel.Text = oW_CstAddr.FTAddTel;
                        if (String.IsNullOrEmpty(otbFax.Text)) otbFax.Text = oW_CstAddr.FTAddFax;
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "W_GETxTaxAddrCst : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
            }
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
                new cLog().C_WRTxLog("wHome", "W_Notification : " + oEx.Message);
            }
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
                new wHome().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmBack_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmMenu_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wTaxInvoice", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>D:\Project\TeamWaterPark\AdaPos\AdaPos\AdaPos\Forms\wSyncData.cs
        /// Show Preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmTax_Click(object sender, EventArgs e)
        {
            try
            {
                new wTaxInvoicePreview().ShowDialog();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Change Type of Business
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocbBsnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(ocbBsnType.SelectedIndex == 0)   // นิติบุคคล
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocbBsnType_SelectedIndexChanged : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocbEstab_SelectedIndexChanged : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wTaxInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "wTaxInvoice_FormClosing : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice:", "ocmNotify_Click : " + oEx.Message); }
        }

        private void ocmSearchCst_Click(object sender, EventArgs e)
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
                    otbCstNo.Text = oFrmCst.oW_DataSearch.tCode;
                    otbCstName.Text = oFrmCst.oW_DataSearch.tName;

                    oSql.AppendLine("SELECT TOP 1 ADR.* FROM TCNMTaxAddress_L ADR WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TCNMCst CST WITH(NOLOCK) ON ADR.FTAddTaxNo = ISNULL(CST.FTCstTaxNo,'') AND CST.FTCstCode ='"+ otbCstNo.Text.Trim() +"' ");
                    oSql.AppendLine("WHERE ADR.FNLngID = " + cVB.nVB_Language);
                    oW_CstAddr = oDB.C_GEToDataQuery<cmlTCNMTaxAddress>(oSql.ToString());
                    if (oW_CstAddr != null)
                    {
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddTaxNo))
                        {
                            otbTaxID.Text = oW_CstAddr.FTAddTaxNo;
                        }
                        if (!string.IsNullOrEmpty(oW_CstAddr.FTAddV2Desc1))
                        {
                            otbAddress.Text = oW_CstAddr.FTAddV2Desc1;
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice:", "ocmSearchCst_Click : " + oEx.Message); }
        }

        private void wTaxInvoice_Shown(object sender, EventArgs e)
        {
            //cVB.oVB_TaxInvoice = this;
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            string tDocNo;
            cTax oTax = new cTax();
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                if (string.IsNullOrEmpty(otbCstNo.Text))
                {
                    otbCstNo.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(otbCstName.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputName"), 3);
                    otbCstName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(otbTaxID.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputTaxNo"), 3);
                    otbTaxID.Focus();
                    return;
                }

                tW_TaxDocNo = oTax.C_GENtDocNo(oW_TaxHD.FNXshDocType.Value);
                if (!string.IsNullOrEmpty(tW_TaxDocNo))
                {
                    if (oTax.C_DOCbInsertTaxWithSQL(tW_TaxDocNo, otbTicketNo.Text.Trim()))
                    {
                        oSql.AppendLine("UPDATE TPSTTaxHD WITH(ROWLOCK)");
                        oSql.AppendLine("SET FTCstCode = '" + otbCstNo.Text +"'");
                        oSql.AppendLine(",FTXshStaApv = '1'");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());

                        oSql = new StringBuilder();
                        oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK)");
                        oSql.AppendLine("SET FTXshDocVatFull = '" + tW_TaxDocNo + "'");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + otbTicketNo.Text.Trim() + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());

                        if (oW_TaxHDCst == null)
                        {
                            oW_TaxHDCst = new cmlTPSTTaxHDCst();
                            oW_TaxHDCst.FTBchCode = cVB.tVB_BchCode;
                            oW_TaxHDCst.FTXshDocNo = tW_TaxDocNo;
                            oW_TaxHDCst.FTXshCardID = otbTaxID.Text;
                            oW_TaxHDCst.FTXshCstName = otbCstName.Text; //*Em 62-12-18
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
                            oSql.AppendLine("SET FTXshCardID = '" + otbTaxID.Text + "'");
                            oSql.AppendLine(",FTXshCstName = '"+ otbCstName.Text.Trim() + "'");
                            oSql.AppendLine(",FTXshCstTel = '" + otbTel.Text.Trim() + "'"); //*Em 62-12-18
                            oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_TaxDocNo + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }

                        oSql.AppendLine("IF EXISTS(SELECT FTAddTaxNo FROM TCNMTaxAddress_L WITH(NOLOCK) WHERE FTAddTaxNo = '" + otbTaxID.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language + ")");
                        oSql.AppendLine("BEGIN");
                        oSql.AppendLine("	UPDATE TCNMTaxAddress_L WITH(ROWLOCK)");
                        oSql.AppendLine("	SET FTAddV2Desc1 ='" + otbAddress.Text.Trim() + "'");
                        oSql.AppendLine("	,FTAddStaBusiness = '" + (ocbBsnType.SelectedIndex + 1) + "'");
                        oSql.AppendLine("	,FTAddStaHQ = '" + (ocbEstab.SelectedIndex == 0 ? "1" : "2") + "'");
                        oSql.AppendLine("	,FTAddStaBchCode = '" + (ocbEstab.SelectedIndex == 0 ? "" : otbBchCode.Text.Trim()) + "'");
                        oSql.AppendLine("	,FDLastUpdOn = GETDATE()");
                        oSql.AppendLine("	,FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                        oSql.AppendLine("   ,FTAddName = '" + otbCstName.Text + "'");
                        oSql.AppendLine("   ,FTAddTel = '" + otbTel.Text + "'");
                        oSql.AppendLine("   ,FTAddFax = '" + otbFax.Text + "'");
                        oSql.AppendLine("	WHERE FTAddTaxNo = '" + otbTaxID.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language);
                        oSql.AppendLine("END");
                        oSql.AppendLine("ELSE");
                        oSql.AppendLine("BEGIN");
                        oSql.AppendLine("	INSERT INTO TCNMTaxAddress_L WITH(ROWLOCK)(FTAddTaxNo,FNLngID,FTAddVersion,FTAddV2Desc1,FTAddStaBusiness,FTAddStaHQ,FTAddStaBchCode");
                        oSql.AppendLine("   ,FTAddName,FTAddTel,FTAddFax"); //*Em 62-10-10
                        oSql.AppendLine("	,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                        oSql.AppendLine("	VALUES('" + otbTaxID.Text.Trim() + "'," + cVB.nVB_Language + ",'2','" + otbAddress.Text.Trim() + "','" + (ocbBsnType.SelectedIndex + 1) + "','" + (ocbEstab.SelectedIndex == 0 ? "1" : "2") + "','" + (ocbEstab.SelectedIndex == 0 ? "" : otbBchCode.Text.Trim()) + "',");
                        oSql.AppendLine("   '" + otbCstName.Text + "','" + otbTel.Text + "','" + otbFax.Text + "',"); //*Em 62-10-10
                        oSql.AppendLine("   GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice:", "ocmAccept_Click : " + oEx.Message); }
        }

        private void ocmPrint_Click(object sender, EventArgs e)
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            try
            {
                //oDoc = new PrintDocument();
                //oSettings = new PrinterSettings();

                //oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                //oDoc.PrintController = new StandardPrintController();
                //oDoc.PrintPage += oDoc_PrintPage;

                //cVB.bVB_PrnTaxInvoiceCopy = false;
                //for (int nMaster = 0; nMaster < cVB.nVB_PrnTaxMaster; nMaster++)
                //{
                //    oDoc.Print();
                //}
                //cVB.bVB_PrnTaxInvoiceCopy = true;
                //for (int nCopy = 0; nCopy < cVB.nVB_PrnTaxCopy; nCopy++)
                //{
                //    oDoc.Print();
                //}
                //W_PRCxPrintUpd();
                //*Net 63-04-01 ยกมาจาก baseline
                wTaxSelectPrint oPrn = new wTaxSelectPrint();
                oPrn.ShowDialog();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmPrint_Click : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                oSP.SP_CLExMemory();
            }
        }

        private void oDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                W_PRNxDrawTax(e.Graphics);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "oDoc_PrintPage : " + oEx.Message); }
        }

        private void ocmSaveAddr_Click(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(otbTaxID.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputTaxNo"), 3);
                    otbTaxID.Focus();
                    return;
                }

                oSql.AppendLine("IF EXISTS(SELECT FTAddTaxNo FROM TCNMTaxAddress_L WITH(NOLOCK) WHERE FTAddTaxNo = '" + otbTaxID.Text.Trim() + "' AND FNLngID = " + cVB.nVB_Language + ")");
                oSql.AppendLine("BEGIN");
                oSql.AppendLine("	UPDATE TCNMTaxAddress_L WITH(ROWLOCK)");
                oSql.AppendLine("	SET FTAddV2Desc1 ='" + otbAddress.Text.Trim() + "'");
                oSql.AppendLine("	,FTAddStaBusiness = '" + (ocbBsnType.SelectedIndex + 1) + "'");
                oSql.AppendLine("	,FTAddStaHQ = '" + (ocbEstab.SelectedIndex == 0 ? "1" : "2") + "'");
                oSql.AppendLine("	,FTAddStaBchCode = '" + (ocbEstab.SelectedIndex == 0 ? "" : otbBchCode.Text.Trim()) + "'");
                oSql.AppendLine("	,FDLastUpdOn = GETDATE()");
                oSql.AppendLine("	,FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine("   ,FTAddName = '" + otbCstName.Text + "'");
                oSql.AppendLine("   ,FTAddTel = '" + otbTel.Text + "'");
                oSql.AppendLine("   ,FTAddFax = '" + otbFax.Text +"'");
                oSql.AppendLine("	WHERE FTAddTaxNo = '"+ otbTaxID.Text.Trim() +"' AND FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("BEGIN");
                oSql.AppendLine("	INSERT INTO TCNMTaxAddress_L WITH(ROWLOCK)(FTAddTaxNo,FNLngID,FTAddVersion,FTAddV2Desc1,FTAddStaBusiness,FTAddStaHQ,FTAddStaBchCode");
                oSql.AppendLine("   ,FTAddName,FTAddTel,FTAddFax"); //*Em 62-10-10
                oSql.AppendLine("	,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                oSql.AppendLine("	VALUES('" + otbTaxID.Text.Trim() + "',"+ cVB.nVB_Language + ",'2','" + otbAddress.Text.Trim() + "','" + (ocbBsnType.SelectedIndex + 1) + "','" + (ocbEstab.SelectedIndex == 0 ? "1" : "2") + "','" + (ocbEstab.SelectedIndex == 0 ? "" : otbBchCode.Text.Trim()) + "',");
                oSql.AppendLine("   '"+ otbCstName.Text +"','"+ otbTel.Text +"','"+ otbFax.Text +"',"); //*Em 62-10-10
                oSql.AppendLine("   GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "')");
                oSql.AppendLine("END");
                oDB.C_SETxDataQuery(oSql.ToString());
                new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgSave"), 1);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "ocmSaveAddr_Click : " + oEx.Message); }
        }

        private void otbTaxID_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!string.IsNullOrEmpty(otbTaxID.Text)) W_GETxTaxAddrCst();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "otbTaxID_KeyDown : " + oEx.Message); }
            finally
            {
            }
        }

        private void otbTaxID_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(otbTaxID.Text)) W_GETxTaxAddrCst();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "otbTaxID_Leave : " + oEx.Message); }
            finally
            {
            }
        }

        private void otbTel_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(otbTel.Text)) W_GETxTaxAddrCst();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "otbTel_Leave : " + oEx.Message); }
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
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoice", "otbTel_KeyDown : " + oEx.Message); }
            finally
            {
            }
        }
        #endregion Method / Events


    }
}
