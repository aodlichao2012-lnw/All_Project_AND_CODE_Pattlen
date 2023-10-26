using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using AdaPos.Class;
using AdaPos.Models.Other.PriceCheck;
using System.Resources;
using AdaPos.Resources_String.Local;

namespace AdaPos.Forms
{
    public partial class wPriceCheck : Form
    {
        private string tW_Barcode = string.Empty;

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        public string rtBarcode { get; set; } //*Arm 63-09-13

        public wPriceCheck()
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                W_SETxDesign();
                W_SETxText();
                oucPincode.oU_TextValue = otbBarcode;
                otbBarcode.Focus();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "wPriceCheck : " + oEx.Message); }
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

        /// <summary>
        /// Set Design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmHome.BackColor = cVB.oVB_ColNormal;
                ocmDashboard.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmSetting.BackColor = cVB.oVB_ColDark;
                ocmAllUnit.BackColor = cVB.oVB_ColDark;    //*Em 62-10-08
                //ogdDataPrice.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;   //*Em 62-10-08
                oW_SP.SP_SETxSetGridviewFormat(ogdDataPrice); //*Net 63-03-03 Set Design Gridview
                oW_SP.SP_SETxSetGridviewFormat(ogdDataPmt);
                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode,"TCNMUser");
                opbLogo.Image = new cCompany().C_GEToImageLogo();

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;

                // if (oW_SP.SP_CHKbConnection())
                //if (oW_SP.SP_CHKbConnection())
                //if (!String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                //{
                //    if (oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
                //    {
                //        opbPOS.Image = Properties.Resources.Online_32;
                //    }
                //    else
                //    {
                //        opbPOS.Image = Properties.Resources.Offline_32;
                //    }
                //}
                //else
                //{
                //    opbPOS.Image = Properties.Resources.Offline_32;
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set Text
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPriceCheck_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPriceCheck_EN));
                        break;
                }

                //cVB.tVB_KbdScreen = "PRICECHECK";

                ocmBack.Text = "".PadLeft(10) + oW_Resource.GetString("tBack");
                ocmKB.Text = "".PadLeft(10) + oW_Resource.GetString("tKeyboard");
                olaTitlePriceCheck.Text = oW_Resource.GetString("tChkPrice");
                olaTitleBarcode.Text = oW_Resource.GetString("tScanBar");
                olaTitleCode.Text = oW_Resource.GetString("tCode");
                olaTitleName.Text = oW_Resource.GetString("tName");
                olaTitlePdtGroup.Text = oW_Resource.GetString("tPdtGrp");
                ocmAllUnit.Text = oW_Resource.GetString("tAllUnit");

                otbTitleBarcode.HeaderText = oW_Resource.GetString("tBarcode");
                otbTitleUnit.HeaderText = oW_Resource.GetString("tUnit");
                otbTitleQty.HeaderText = oW_Resource.GetString("tFactor");
                otbTitleStart.HeaderText = oW_Resource.GetString("tStart");
                otbTitleFinish.HeaderText = oW_Resource.GetString("tFinish");
                otbTitlePrice.HeaderText = oW_Resource.GetString("tPrice");

                //*Arm 63-04-13
                otbPplName.HeaderText = oW_Resource.GetString("tPriceGgroup");
                otbPghDocNo.HeaderText = oW_Resource.GetString("tDocNo");

                otbPmtBarcode.HeaderText = oW_Resource.GetString("tBarcode");
                otbPmtStart.HeaderText = oW_Resource.GetString("tStart");
                otbPmtFinish.HeaderText = oW_Resource.GetString("tFinish");
                otbPmtPromotion.HeaderText = oW_Resource.GetString("tPromotion");
                otbPmtDocNo.HeaderText = oW_Resource.GetString("tDocNo");

                olaCheckPrice.Text = oW_Resource.GetString("tBtnCheckPrice");
                olaCheckPromotion.Text = oW_Resource.GetString("tBtnCheckPromotion");
                //++++++++++++++

                // Header
                olaHome.Text = oW_Resource.GetString("tChkPrice");
                olaVersion.Text = Application.ProductVersion;
                //olaUsrName.Text = new cUser().C_GETtUsername();
                olaUsrName.Text = cVB.tVB_UsrName;
                olaPos.Text = cVB.tVB_PosCode;
                olaUsrName.Text = cVB.tVB_UsrName;

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// GET ราคาของสินค้าที่ต้องการเช็คตาม Barcode ที่รับเข้ามา
        /// </summary>
        /// <param name="ptBarcode">Barcode</param>
        /// <param name="pbShowAll">true:แสดงสินค้านทุกรายการ false:แสดงแค่สินค้าตาม Barcode</param>
        public void W_GETxPdtPrice(string ptBarcode, bool pbShowAll = false)
        {
            StringBuilder oSql;
            cmlPdtBarcode oPdt;
            List<cmlPriceCheck> aoPriceCheck;
            List<cmlPriceCheck> aoPromoCheck; //*Arm 63-04-13
            cVB.nVB_Language = 1;
            try
            {
                oSql = new StringBuilder();

                oSql.AppendLine("SELECT PDT.FTPdtCode,PDT_L.FTPdtName,GRP_L.FTPgpName FROM TCNMPdtBar BAR ");
                oSql.AppendLine("INNER JOIN TCNMPdt PDT ON PDT.FTPdtCode = BAR.FTPdtCode ");
                oSql.AppendLine("LEFT JOIN TCNMPdt_L PDT_L ON PDT_L.FTPdtCode = PDT.FTPdtCode AND PDT_L.FNLngID = " + cVB.nVB_Language + " ");
                oSql.AppendLine("LEFT JOIN TCNMPdtGrp_L GRP_L ON GRP_L.FTPgpChain = PDT.FTPgpChain AND GRP_L.FNLngID = " + cVB.nVB_Language + " ");
                oSql.AppendLine("WHERE BAR.FTBarCode = '" + ptBarcode + "'");
                oSql.AppendLine("OR PDT.FTPdtCode = '"+ ptBarcode +"'");    //*Em 62-09-10

                oPdt = new cmlPdtBarcode();
                oPdt = new cDatabase().C_GETaDataQuery<cmlPdtBarcode>(oSql.ToString()).FirstOrDefault();

                if (oPdt != null)
                {
                    otbPdtCode.Text = oPdt.FTPdtCode;
                    otbName.Text = oPdt.FTPdtName == null ? string.Empty : oPdt.FTPdtName;
                    otbPdtGroup.Text = oPdt.FTPgpName == null ? string.Empty : oPdt.FTPgpName;
                }
                else
                {
                    //*Em 62-09-12
                    otbPdtCode.Text = "";
                    otbName.Text = "";
                    otbPdtGroup.Text = "";
                    //++++++++++++++++
                }

                oSql = new StringBuilder();

                //oSql.AppendLine("SELECT PDT.FTPdtCode,PDTL.FTPdtName,BAR.FTBarCode,PUN.FTPunName,PKS.FCPdtUnitFact,PRI.FDPghDStart,PRI.FDPghDStop,PRI.FCPgdPriceRet, Pmt_L.FTPmhName ");
                //oSql.AppendLine("FROM TCNMPdtBar BAR LEFT JOIN TCNTPdtPrice4PDT PRI ON BAR.FTPdtCode = PRI.FTPdtCode AND BAR.FTPunCode = PRI.FTPunCode ");
                ////oSql.AppendLine("AND Getdate() BETWEEN PRI.FDPghDStart AND PRI.FDPghDStop ");
                //oSql.AppendLine("INNER JOIN TCNMPdtPackSize PKS ON BAR.FTPdtCode = PKS.FTPdtCode AND BAR.FTPunCode = PKS.FTPunCode ");
                //oSql.AppendLine("INNER JOIN TCNMPdt PDT ON BAR.FTPdtCode = PDT.FTPdtCode ");
                //oSql.AppendLine("LEFT JOIN TCNMPdtGrp_L GRP ON PDT.FTPgpChain = GRP.FTPgpChain AND GRP.FNLngID = " + cVB.nVB_Language + " ");
                //oSql.AppendLine("INNER JOIN TCNMPdt_L PDTL ON PDT.FTPdtCode = PDTL.FTPdtCode ");
                //oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L PUN ON BAR.FTPunCode = PUN.FTPunCode AND PUN.FNLngID = " + cVB.nVB_Language + " ");
                //oSql.AppendLine("LEFT JOIN TCNTPdtPmtDT PmtDT ON PmtDT.FTPmdBarCode = BAR.FTBarCode OR PmtDT.FTPmdRefCode = PDT.FTPdtCode ");
                //oSql.AppendLine("LEFT JOIN TCNTPdtPmtHD_L Pmt_L ON PmtDT.FTBchCode = Pmt_L.FTBchCode AND PmtDT.FTPmhDocNo = Pmt_L.FTPmhDocNo ");
                //oSql.AppendLine("WHERE BAR.FTPdtCode = '" + otbPdtCode.Text + "'  AND PDTL.FNLngID = " + cVB.nVB_Language + " ");
                //oSql.AppendLine("AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),PRI.FDPghDStart,121) AND CONVERT(VARCHAR(19),PRI.FDPghDStop,121))");  //*Em 62-09-10
                //if (!pbShowAll)
                //{
                //    //oSql.AppendLine("AND BAR.FTBarCode = '" + ptBarcode + "'");
                //    oSql.AppendLine("AND (BAR.FTBarCode = '" + ptBarcode + "'");
                //    oSql.AppendLine("OR PDT.FTPdtCode = '" + ptBarcode + "')");
                //}
                //oSql.AppendLine("ORDER BY BAR.FTBarCode");

                //*Arm 63-04-13

                oSql.Clear();
                //oSql.AppendLine("SELECT DISTINCT PMT.FTPmhDocNo, PMTHD_L.FTPmhName, PMTHD.FDPmhDStart, PMTHD.FDPmhDStop, PMT.FTPmdBarCode FROM TPMTPmt  PMT WITH(NOLOCK)");
                oSql.AppendLine("SELECT DISTINCT PMT.FTPmhDocNo, PMTHD_L.FTPmhName, ");
                oSql.AppendLine("CONVERT(VARCHAR(10), PMTHD.FDPmhDStart, 121)+' '+CONVERT(VARCHAR(10), PMTHD.FDPmhTStart, 108)  AS FDPmhDStart,");
                oSql.AppendLine("CONVERT(VARCHAR(10), PMTHD.FDPmhDStop, 121)+' '+CONVERT(VARCHAR(10), PMTHD.FDPmhTStop, 108) AS FDPmhDStop,");
                oSql.AppendLine("PMT.FTPmdBarCode");
                oSql.AppendLine("FROM TPMTPmt  PMT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNTPdtPmtHD PMTHD WITH(NOLOCK) ON PMT.FTPmhDocNo = PMTHD.FTPmhDocNo");
                oSql.AppendLine("INNER JOIN TCNTPdtPmtHD_L PMTHD_L WITH(NOLOCK) ON PMTHD.FTPmhDocNo = PMTHD_L.FTPmhDocNo");
                oSql.AppendLine("WHERE PMT.FTPmdRefCode = '"+ otbPdtCode.Text + "' AND PMTHD_L.FNLngID ='" + cVB.nVB_Language + "'");

                aoPromoCheck = new List<cmlPriceCheck>();   //*Arm 63-04-13
                aoPromoCheck = new cDatabase().C_GETaDataQuery<cmlPriceCheck>(oSql.ToString()); //*Arm 63-04-13
                
                oSql.Clear();
                //*Net 63-07-31 ปรับตาม Moshi
                //oSql.AppendLine("SELECT PDT.FTPdtCode,PDTL.FTPdtName,BAR.FTBarCode,PUN.FTPunName,PKS.FCPdtUnitFact,PRI.FDPghDStart,PRI.FDPghDStop,PRI.FCPgdPriceRet, PRIL.FTPplName, PRI.FTPghDocNo");
                //oSql.AppendLine("FROM TCNMPdtBar BAR ");
                //oSql.AppendLine("LEFT JOIN( SELECT TOP 1 *  ");
                //oSql.AppendLine("           From TCNTPdtPrice4PDT WITH(NOLOCK) ");
                //oSql.AppendLine("           WHERE GETDATE() BETWEEN FDPghDStart AND FDPghDStop  ");
                //oSql.AppendLine("               AND CONVERT(time(0), GETDATE(),108) BETWEEN FTPghTStart AND FTPghTStop  ");
                //oSql.AppendLine("               AND FTPdtCode = '" + otbPdtCode.Text + "'  ");
                //oSql.AppendLine("               AND FTPplCode = '" + cVB.tVB_PriceGroup + "'  ");
                //oSql.AppendLine("           ORDER BY FDPghDStart DESC, FTPghTStart DESC) PRI  ");
                //oSql.AppendLine("   ON BAR.FTPdtCode = PRI.FTPdtCode AND BAR.FTPunCode = PRI.FTPunCode  ");
                //oSql.AppendLine("LEFT JOIN TCNMPdtPriList_L PRIL ON PRI.FTPplCode = PRIL.FTPplCode AND PRIL.FNLngID = " + cVB.nVB_Language + " ");
                //oSql.AppendLine("INNER JOIN TCNMPdtPackSize PKS ON BAR.FTPdtCode = PKS.FTPdtCode AND BAR.FTPunCode = PKS.FTPunCode ");
                //oSql.AppendLine("INNER JOIN TCNMPdt PDT ON BAR.FTPdtCode = PDT.FTPdtCode ");
                //oSql.AppendLine("LEFT JOIN TCNMPdtGrp_L GRP ON PDT.FTPgpChain = GRP.FTPgpChain AND GRP.FNLngID = " + cVB.nVB_Language + " ");
                //oSql.AppendLine("INNER JOIN TCNMPdt_L PDTL ON PDT.FTPdtCode = PDTL.FTPdtCode ");
                //oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L PUN ON BAR.FTPunCode = PUN.FTPunCode AND PUN.FNLngID = " + cVB.nVB_Language + " ");
                //oSql.AppendLine("WHERE BAR.FTPdtCode = '" + otbPdtCode.Text + "'  AND PDTL.FNLngID = " + cVB.nVB_Language + " ");
                //oSql.AppendLine("AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),PRI.FDPghDStart,121) AND CONVERT(VARCHAR(19),PRI.FDPghDStop,121))");  //*Em 62-09-10
                //if (!pbShowAll)
                //{
                //    oSql.AppendLine("AND (BAR.FTBarCode = '" + ptBarcode + "'");
                //    oSql.AppendLine("OR PDT.FTPdtCode = '" + ptBarcode + "')");
                //}
                //oSql.AppendLine("ORDER BY BAR.FTBarCode");
                oSql.AppendLine($"SELECT PDT.FTPdtCode,PDTL.FTPdtName,BAR.FTBarCode,ISNULL(PUN.FTPunName,'') AS FTPunName,");
                //oSql.AppendLine($"PKS.FCPdtUnitFact,PRI.FCPdtPrice AS FCPgdPriceRet,ISNULL(PRIL.FTPplName,'') AS FTPplName,");
                oSql.AppendLine($"PKS.FCPdtUnitFact,P4PDT.FCPgdPriceRet AS FCPgdPriceRet,ISNULL(PRIL.FTPplName,'') AS FTPplName,");
                oSql.AppendLine($"CONVERT(VARCHAR(10), P4PDT.FDPghDStart, 121) + ' ' + P4PDT.FTPghTStart AS FDPghDStart,");
                oSql.AppendLine($"CONVERT(VARCHAR(10), P4PDT.FDPghDStop, 121) + ' ' + P4PDT.FTPghTStop AS FDPghDStop,");
                oSql.AppendLine($"P4PDT.FTPghDocNo");
                oSql.AppendLine($"FROM TCNMPdtBar BAR WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN TCNMPdt PDT ON BAR.FTPdtCode = PDT.FTPdtCode AND PDT.FTPdtCode = '{otbPdtCode.Text}'");
                oSql.AppendLine($"INNER JOIN TCNMPdt_L PDTL ON PDT.FTPdtCode = PDTL.FTPdtCode AND PDTL.FNLngID='{cVB.nVB_Language}' AND PDTL.FTPdtCode = '{otbPdtCode.Text}'");
                oSql.AppendLine($"LEFT JOIN TCNMPdtUnit_L PUN ON BAR.FTPunCode = PUN.FTPunCode AND PUN.FNLngID = '{cVB.nVB_Language}' ");
                oSql.AppendLine($"INNER JOIN TCNMPdtPackSize PKS WITH(NOLOCK) ON BAR.FTPdtCode = PKS.FTPdtCode AND BAR.FTPunCode = PKS.FTPunCode  AND PKS.FTPdtCode = '{otbPdtCode.Text}'");
                //*Net 63-09-02 เอามาจากตารางจริง
                //oSql.AppendLine($"INNER JOIN TPSTPdtPrice PRI WITH(NOLOCK) ON BAR.FTPdtCode=PRI.FTPdtCode AND BAR.FTPunCode=PRI.FTPunCode  AND PRI.FTPdtCode = '{otbPdtCode.Text}'");
                //oSql.AppendLine($"LEFT JOIN TCNMPdtPriList_L PRIL WITH(NOLOCK) ON PRI.FTPplCode = PRIL.FTPplCode AND PRIL.FNLngID = '{cVB.nVB_Language}'");
                oSql.AppendLine($"INNER JOIN TCNTPdtPrice4PDT P4PDT WITH(NOLOCK) ");
                //*Net 63-09-02 เอามาจากตารางจริง
                //oSql.AppendLine($"ON PRI.FTPdtCode=P4PDT.FTPdtCode AND PRI.FTPunCode=P4PDT.FTPunCode ");
                //oSql.AppendLine($"AND PRI.FTPplCode=P4PDT.FTPplCode AND PRI.FTPriType=P4PDT.FTPghDocType AND P4PDT.FTPdtCode='{otbPdtCode.Text}'");
                oSql.AppendLine($"ON PDT.FTPdtCode=P4PDT.FTPdtCode AND BAR.FTPunCode=P4PDT.FTPunCode  AND P4PDT.FTPdtCode='{otbPdtCode.Text}'");
                oSql.AppendLine($"LEFT JOIN TCNMPdtPriList_L PRIL WITH(NOLOCK) ON P4PDT.FTPplCode = PRIL.FTPplCode AND PRIL.FNLngID = '{cVB.nVB_Language}'");
                oSql.AppendLine($"WHERE (BAR.FTBarCode = '{ptBarcode}' OR PDT.FTPdtCode = '{ptBarcode}')");
                oSql.AppendLine($"ORDER BY PDT.FTPdtCode,BAR.FTBarCode,PRIL.FTPplName,P4PDT.FDPghDStart");


                aoPriceCheck = new List<cmlPriceCheck>();
                aoPriceCheck = new cDatabase().C_GETaDataQuery<cmlPriceCheck>(oSql.ToString());

                //+++++++++++++++++++


                if (ogdDataPrice.Rows.Count > 0)
                {
                    ogdDataPrice.Rows.Clear();
                }

                if (ogdDataPmt.Rows.Count > 0)  //*Arm 63-04-13
                {
                    ogdDataPmt.Rows.Clear();
                }

                foreach (var oItem in aoPriceCheck)
                {
                    ogdDataPrice.Rows.Add(
                        oItem.FTBarCode,
                        oItem.FTPunName,
                        //oItem.FCPdtUnitFact,
                        oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oItem.FCPdtUnitFact), cVB.nVB_DecShow),
                        Convert.ToDateTime(oItem.FDPghDStart).ToString("dd/MM/yyyy"),
                        Convert.ToDateTime(oItem.FDPghDStop).ToString("dd/MM/yyyy"),
                        //oItem.FCPgdPriceRet,
                        oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oItem.FCPgdPriceRet), cVB.nVB_DecShow),
                        oItem.FTPplName,    //*Arm 63-04-13
                        oItem.FTPghDocNo    //*Arm 63-04-13
                        );
                    
                }

                //*Arm 63-04-13
                foreach (var oItem in aoPromoCheck)
                {
                    ogdDataPmt.Rows.Add(
                    oItem.FTPmhDocNo,
                    oItem.FTPmhName,
                    //Convert.ToDateTime(oItem.FDPmhDStart).ToString("dd/MM/yyyy"),
                    //Convert.ToDateTime(oItem.FDPmhDStop).ToString("dd/MM/yyyy"),
                    Convert.ToDateTime(oItem.FDPmhDStart).ToString("dd/MM/yyyy HH:mm"),
                    Convert.ToDateTime(oItem.FDPmhDStop).ToString("dd/MM/yyyy HH:mm"),
                    oItem.FTPmdBarCode
                    );
                }
                //+++++++++++++++

                //*Em 62-09-12
                if (string.IsNullOrEmpty(otbPdtCode.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgNotFound"), 3);
                }
                //++++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPriceCheck", "GETxPdtPrice : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oPdt = null;
                aoPriceCheck = null;
            }
        }

        private void otbBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        tW_Barcode = otbBarcode.Text;
                        W_GETxPdtPrice(tW_Barcode);
                        otbBarcode.Clear();
                        otbBarcode.Focus();
                        break;
                    case Keys.Escape:
                        this.Close();
                        break;
                }
                //if (e.KeyCode == Keys.Enter)
                //{
                //    tW_Barcode = otbBarcode.Text;
                //    W_GETxPdtPrice(tW_Barcode,true);

                //    otbBarcode.Clear();
                //}
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPriceCheck", "otbBarcode_KeyDown : " + oEx.Message);
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
                new cLog().C_WRTxLog("wPriceCheck", "W_Notification : " + oEx.Message);
            }
        }

        private void ocmAllUnit_Click(object sender, EventArgs e)
        {
            try
            {
                W_GETxPdtPrice(tW_Barcode, true);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPriceCheck", "ocmAllUnit_Click : " + oEx.Message);
            }
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
                W_SETxOpenCloseMenu();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "ocmMenu_Click " + oEx.Message); }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
        /// </summary>
        private void W_SETxOpenCloseMenu()
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "W_SETxOpenCloseMenu : " + oEx.Message); }
        }

        private void ocmExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "ocmExit_Click : " + oEx.Message); }
        }

        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "ocmKB_Click : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            //*Arm 62-09-17 
            //เพิ่ม Event ปุ่ม ocmNotify และ code ใน Event
            try
            {
                //W_CHKxNotify();
                if (opnNotify.Visible == false)
                {
                    cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                }
                //+++++++++++++++++
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "ocmNotify_Click : " + oEx.Message); }
            //*Arm 62-09-17 
            
        }

        private void wPriceCheck_Shown(object sender, EventArgs e)
        {
            //*Net 63-07-31 ย้ายมาจาก ctor
            if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
            cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            otbBarcode.Focus();
            olaCheckPrice_Click(olaCheckPrice, null); //*Arm 63-04-13
        }

        private void olaCheckPrice_Click(object sender, EventArgs e)
        {
            try
            {
                ogdDataPrice.Visible = true;
                ogdDataPmt.Visible = false;
                olaCheckPromotion.ForeColor = Color.Gray;
                olaCheckPrice.ForeColor = Color.Black;
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wPriceCheck", "olaCheckPrice_Click : " + oEx.Message);
            }
        }

        private void olaCheckPromotion_Click(object sender, EventArgs e)
        {
            try
            {
                ogdDataPrice.Visible = false;
                ogdDataPmt.Visible = true;
                olaCheckPromotion.ForeColor = Color.Black;
                olaCheckPrice.ForeColor = Color.Gray;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPriceCheck", "olaCheckPromotion_Click : " + oEx.Message);
            }
        }

        private void wPriceCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPriceCheck", "wPriceCheck_FormClosing : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        
        /// <summary>
        /// *Arm 63-09-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdDataPrice_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                rtBarcode = ogdDataPrice.CurrentRow.Cells[otbTitleBarcode.Name].Value.ToString();
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPriceCheck", "ogdDataPrice_DoubleClick : " + oEx.Message);
            }
        }
    }
}
