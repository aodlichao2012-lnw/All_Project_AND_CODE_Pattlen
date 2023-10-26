using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Forms
{
    public partial class wPmtGetorSug : Form
    {
        #region Variable

        private cSP oW_SP;
        private cPdtPmt oW_PdtPmt;
        private ResourceManager oW_Resource;
        private int nw_ModeClose = 2;    // 1:Back, 2:Close
        public static string tW_PmtDoc;
        #endregion

        #region Constructor
        /// <summary>
        /// wPmtGetorSug
        /// </summary>
        public wPmtGetorSug()
        {
            InitializeComponent();
            oW_SP = new cSP();
            W_SETxDesign();
            W_SETxText();
           
            W_SHWxButtonBar();
            //oW_PdtPmt = new cPdtPmt();

            // W_PRCoCalPmt();
        }

        #endregion

       
        #region Function / Method

        private void W_SETxDesign()
        {
            // Set Design ในส่วน Header And Menu  +++++++++++++++++++++++++++++++++++++++++++++++
           
            
            opnMenu.Width = 50;
            opnMenu.BackColor = cVB.oVB_ColDark;
            ocmMenu.BackColor = cVB.oVB_ColDark;
            opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
            opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
            
            opbLogo.Image = new cCompany().C_GEToImageLogo();
            opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode, "TCNMUser");

            if (opbLogo.Image != null)
                opbLogo.Visible = true;

            //if (oW_SP.SP_CHKbConnection())
            //    opbPOS.Image = Properties.Resources.Online_32;
            //else
            //    opbPOS.Image = Properties.Resources.Offline_32;
            //if (!String.IsNullOrEmpty(cVB.tVB_API2PSMaster))
            //{
            //    if (oW_SP.SP_CHKbConnection(cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"))   // Connect internet  //*Em 63-03-05
            //        opbPOS.Image = Properties.Resources.Online_32;
            //    else
            //        opbPOS.Image = Properties.Resources.Offline_32;
            //}
            //else
            //{
            //    opbPOS.Image = Properties.Resources.Offline_32;
            //}

            ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // Set Design ในส่วน Content  ++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            opnCashPay.BackColor = cVB.oVB_ColDark;
            olaTitleCashPay.ForeColor = Color.White;
            olaCashPayment.ForeColor = Color.White;
            oW_SP.SP_SETxSetGridviewFormat(ogdGetPmt);
            oW_SP.SP_SETxSetGridviewFormat(ogdRecPmt);

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // Set Design ในส่วน Footer ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ocmCashPay.BackColor = cVB.oVB_ColDark;
            ocmCheckPmt.BackColor = cVB.oVB_ColDark;

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //ocmCashPay.Enabled = false;
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

            ocmCashPay.Text = "ชำระเงิน";
            ocmCheckPmt.Text = "ตรวจสอบโปรโมชั่นใหม่";
            olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tPayment");
            olaGetPmt.Text = cVB.oVB_GBResource.GetString("tGetPromotion");
            olaRecomed.Text = cVB.oVB_GBResource.GetString("tRecomed");
        }

        public void W_SETxDataPmtGet(cmlPmtGet poPmtGet)
        {
            try
            {
                
                ogdGetPmt.Rows.Add(poPmtGet.tPrmCode, poPmtGet.tSeq, poPmtGet.tPromotion, poPmtGet.tDiscount, poPmtGet.tRedame,poPmtGet.tPoint,true,true);
                ogdGetPmt.Update();
                


            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtGetorSug:", "W_SETxDataPmt : " + oEx.Message);
            }
        }

        public void W_SETxDataPmtSug(cmlPmtSug poPmtSug)
        {
            try
            {

                ogdRecPmt.Rows.Add(poPmtSug.tSeq, poPmtSug.tPromotion, poPmtSug.tDiscount, poPmtSug.tRedame, poPmtSug.tPoint,poPmtSug.tPmtAdd);
                ogdRecPmt.Update();


            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtGetorSug:", "W_SETxDataPmt : " + oEx.Message);
            }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtGetorSug", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                oW_SP.SP_CLExMemory();
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
                        cSale.C_PRCxClearCallBack();    //*Em 62-10-09

                        cVB.nVB_CstPoint = cVB.nVB_CstPiontB4Used; //*Arm 63-03-11 คืนค่า point
                        cSale.nC_RDSeqNo = 0; //*Arm 63-03-11 คืนค่า Seq SalRD

                        //cVB.tVB_KbdScreen = "SALE";

                        if (cVB.oVB_2ndScreen != null)
                        {
                            cVB.oVB_2ndScreen.W_PRCxBack();
                            cVB.oVB_2ndScreen.Update();
                        }
                        nw_ModeClose = 1;
                        C_PRCxClearExc();

                        if (cVB.bVB_PriceConfirm) cVB.oVB_Sale.W_PRCxPriceConfirm();    //*Em 63-05-07

                        cVB.oVB_Sale.bW_CalPmtPrice = false;    //*Em 63-05-05
                        Thread oCalPro = new Thread(cVB.oVB_Sale.W_PRCxCoPmt);
                        oCalPro.IsBackground = true;
                        oCalPro.Priority = ThreadPriority.Highest;
                        oCalPro.Start();

                        //cVB.oVB_Sale.Show();
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

                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_PRCxBack : " + oEx.Message); }
        }


        #endregion

        #region Event 

        private void OcmCashPay_Click(object sender, EventArgs e)
        {
            try
            {
                if (ogdGetPmt.Rows.Count > 0)
                {
                    new cPdtPmt().C_PRCxPmtDisProratePD();
                }
                    
                //oW_PdtPmt.C_PRCxPmtDisHD();
                cSale.C_PRCxSummary2HD();
                //cVB.cVB_Amount = Convert.ToDecimal(olaCashPayment.Text);
                new wPayment(3).ShowDialog();
                this.Close();

            }
            catch(Exception oEX) { }
           
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
                    case "C_KBDxBack": W_PRCxBack(); break;
                    case "C_KBDxPdtDetail": break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtGetorSug", "ocmMenuBar_Click : " + oEx.Message); }

        }
        #endregion


        public void C_SETxPdtPmtToGrid(DataTable poDbTblPdt)
        {
            DataTable oDbTblDist;
            wPmtGetorSug oW_PmtGetorSug;
            oW_PdtPmt = new cPdtPmt();
            var distinctRows = (from DataRow dRow in poDbTblPdt.Rows
                                where dRow.Field<decimal?>("FCXpdGetQtyDiv") > 0
                                select new
                                {
                                    FTPmhCode = dRow.Field<string>("FTPmhCode"),
                                    FCXpdDis = dRow.Field<decimal?>("FCXpdDis"),
                                    FCXpdGetQtyDiv = dRow.Field<decimal?>("FCXpdGetQtyDiv"),
                                    FCXpdPoint = dRow.Field<Int64?>("FCXpdPoint"),
                                    FTPgtCpnText = dRow.Field<string>("FTPgtCpnText"),
                                    FTCpdBarCpn = dRow.Field<string>("FTCpdBarCpn")
                                }
                                            ).Distinct();

            oDbTblDist = oW_PdtPmt.C_CONoFill(distinctRows);
            int nIndex = 0;
            foreach (DataRow oRow in oDbTblDist.Rows)
            {

                nIndex = nIndex + 1;
                cmlPmtGet oPmtGet = new cmlPmtGet();
                oPmtGet.tSeq = nIndex.ToString();
                oPmtGet.tPromotion = oW_PdtPmt.C_GETtNamePmt(oRow.Field<string>("FTPmhCode"));
                oPmtGet.tPrmCode = oRow.Field<string>("FTPmhCode");
                oPmtGet.tDiscount = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow);
                if (oRow.Field<decimal?>("FCXpdGetQtyDiv") == null)
                {
                    oPmtGet.tRedame = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                }
                else
                {
                    if (!string.IsNullOrEmpty(oRow.Field<string>("FTPgtCpnText")) || !string.IsNullOrEmpty(oRow.Field<string>("FTCpdBarCpn")))
                    {
                        oPmtGet.tRedame = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdGetQtyDiv"), cVB.nVB_DecShow);
                    }
                    else
                    {
                        oPmtGet.tRedame = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                    }
                   
                }

                if (oRow.Field<Int64?>("FCXpdPoint") == null)
                {
                    oPmtGet.tPoint = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                }
                else
                {
                    oPmtGet.tPoint = oW_SP.SP_SETtDecShwSve(1, oRow.Field<Int64>("FCXpdPoint"), cVB.nVB_DecShow);
                }
                W_SETxDataPmtGet(oPmtGet);

            }
        }

        public void C_SETxPdtSugToGrid(DataTable poDbTbl)
        {
            int nIndex = 0;
            foreach (DataRow oRow in poDbTbl.Rows)
            {
                nIndex = nIndex + 1;
                cmlPmtSug oPmtSug = new cmlPmtSug();
                oPmtSug.tSeq = nIndex.ToString();
                oPmtSug.tPromotion = new cPdtPmt().C_GETtNamePmt(oRow.Field<string>("FTPmhCode"));
                oPmtSug.tDiscount = oPmtSug.tRedame = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow);
                if (oRow.Field<decimal?>("FCQtyCpn") == null)
                {
                    oPmtSug.tRedame = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                }
                else
                {
                    oPmtSug.tRedame = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCQtyCpn"), cVB.nVB_DecShow);
                };
                if (oRow.Field<Int64>("FNQtyPnt") == 0)
                {
                    oPmtSug.tPoint = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                }
                else
                {
                    oPmtSug.tPoint = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FNQtyPnt"), cVB.nVB_DecShow);
                };
                if (oRow.Field<decimal?>("FCQtyAdd") == null)
                {
                    oPmtSug.tPmtAdd = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                }
                else
                {
                    oPmtSug.tPmtAdd = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCQtyAdd"), cVB.nVB_DecShow);
                };
                W_SETxDataPmtSug(oPmtSug);

            }
        }

        private void WPmtGetorSug_Shown(object sender, EventArgs e)
        {
            try
            {
                //Thread.Sleep(4 * 1000);
                if (cVB.oVB_GetPmt.Rows.Count > 0)
                {
                    //new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                    C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                }
                if (cVB.oVB_PmtSug.Rows.Count > 0)
                {

                    C_SETxPdtSugToGrid(cVB.oVB_PmtSug);
                }
                //ocmCashPay.Enabled = true;
            }
            catch(Exception oEx)
            {

            }
            


        }

        private void OgdGetPmt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ogdGetPmt.Columns[6].Index)
            {
                if (Convert.ToBoolean(ogdGetPmt.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue))
                {
                   var s = ogdGetPmt.Rows[e.RowIndex].Cells[0].Value;
                }
                else
                {
                    
                    C_PRCxPmtExc(ogdGetPmt.Rows[e.RowIndex].Cells[0].Value.ToString());
                    ogdGetPmt.Rows.Clear();
                    new cPdtPmt().C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);
                    new cPdtPmt().C_PRCoCalPmt("2");
                    if (cVB.oVB_GetPmt.Rows.Count > 0)
                    {
                        new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                        C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                    }
                }
            }
            else
            {

            }
            
        }

        public void C_PRCxPmtExc(string ptPmtCode)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDatabase = new cDatabase();

            try
            {

                oSql.Clear();
                //oSql.AppendLine("TRUNCATE TABLE TPMTPmtExc ");
                oSql.AppendLine("INSERT INTO TPMTPmtExc WITH(ROWLOCK)");
                oSql.AppendLine("(");
                oSql.AppendLine("  FTXshDocNo,FTPmhDocNo");
                oSql.AppendLine(")VALUES (");
                oSql.AppendLine("  '" + cVB.tVB_DocNo + "','" + ptPmtCode + "'");
                oSql.AppendLine(")");

                oDatabase.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxPmtExc : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDatabase = null;

                new cSP().SP_CLExMemory();
            }
        }

        public void C_PRCxClearExc()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDatabase = new cDatabase();

            try
            {

                oSql.Clear();
                oSql.AppendLine("TRUNCATE TABLE TPMTPmtExc ");
               

                oDatabase.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxPmtExc : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDatabase = null;

                new cSP().SP_CLExMemory();
            }
        }
    }
}
