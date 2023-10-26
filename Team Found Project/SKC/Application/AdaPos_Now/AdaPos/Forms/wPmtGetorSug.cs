using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Popup.wSale;
using AdaPos.Resources_String.Local;
using C1.Win.C1FlexGrid;
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
            new cLog().C_WRTxLog("wPmtGetorSug", "wPmtGetorSug : Start", cVB.bVB_AlwPrnLog);
            InitializeComponent();
            oW_SP = new cSP();

            new cLog().C_WRTxLog("wPmtGetorSug", "wPmtGetorSug : W_SETxDesign", cVB.bVB_AlwPrnLog);
            W_SETxDesign();

            new cLog().C_WRTxLog("wPmtGetorSug", "wPmtGetorSug : W_SETxText", cVB.bVB_AlwPrnLog);
            W_SETxText();

            new cLog().C_WRTxLog("wPmtGetorSug", "wPmtGetorSug : W_SHWxButtonBar", cVB.bVB_AlwPrnLog);
            W_SHWxButtonBar();
            //oW_PdtPmt = new cPdtPmt();
            new cLog().C_WRTxLog("wPmtGetorSug", "wPmtGetorSug : End", cVB.bVB_AlwPrnLog);
            // W_PRCoCalPmt();

            cSP.SP_SETxFixPanelOverFlow(opnHDMenuBar, olaBranch);  //*Net 63-06-09 Resize Branch
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
            //oW_SP.SP_SETxSetGridviewFormat(ogdGetPmt);
            //oW_SP.SP_SETxSetGridviewFormat(ogdRecPmt);

            oW_SP.SP_SETxSetGridFormat(ogdGetPmt);  //*Em 63-06-01
            oW_SP.SP_SETxSetGridFormat(ogdSugPmt);  //*Em 63-06-01
            ogdGetPmt.Rows.Count = ogdGetPmt.Rows.Fixed;    //*Em 63-06-01
            ogdSugPmt.Rows.Count = ogdSugPmt.Rows.Fixed;    //*Em 63-06-01
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // Set Design ในส่วน Footer ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ocmCashPay.BackColor = cVB.oVB_ColDark;
            //ocmCheckPmt.BackColor = cVB.oVB_ColDark;

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

            ocmCashPay.Text = cVB.oVB_GBResource.GetString("tPay");
            //ocmCheckPmt.Text = cVB.oVB_GBResource.GetString("tChkPmt");
            olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tPayment");
            olaGetPmt.Text = cVB.oVB_GBResource.GetString("tGetPromotion");
            olaRecomed.Text = cVB.oVB_GBResource.GetString("tRecomed");
        }

        //public void W_SETxDataPmtGet(cmlPmtGet poPmtGet)
        //{
        //    new cLog().C_WRTxLog("wPmtGetorSug", "W_SETxDataPmtGet : Start Function");
        //    try
        //    {
                
        //        ogdGetPmt.Rows.Add(poPmtGet.tPrmCode, poPmtGet.tSeq, poPmtGet.tPromotion, poPmtGet.tDiscount, poPmtGet.tRedame,poPmtGet.tPoint,true,true);
        //        ogdGetPmt.Update();

        //    }
        //    catch (Exception oEx)
        //    {
        //        new cLog().C_WRTxLog("wPmtGetorSug:", "W_SETxDataPmtGet : " + oEx.Message);
        //    }
        //    new cLog().C_WRTxLog("wPmtGetorSug", "W_SETxDataPmtGet : End Function");
        //}

        //public void W_SETxDataPmtSug(cmlPmtSug poPmtSug)
        //{
        //    new cLog().C_WRTxLog("wPmtGetorSug", "W_SETxDataPmtSug : Start Function");
        //    try
        //    {

        //        ogdRecPmt.Rows.Add(poPmtSug.tSeq, poPmtSug.tPromotion, poPmtSug.tDiscount, poPmtSug.tRedame, poPmtSug.tPoint,poPmtSug.tPmtAdd);
        //        ogdRecPmt.Update();


        //    }
        //    catch (Exception oEx)
        //    {
        //        new cLog().C_WRTxLog("wPmtGetorSug:", "W_SETxDataPmtSug : " + oEx.Message);
        //    }
        //    new cLog().C_WRTxLog("wPmtGetorSug", "W_SETxDataPmtSug : End Function");
        //}

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
                new cLog().C_WRTxLog("wPmtGetorSug", "W_SHWxButtonBar : Start Function C_GETaMenuBar", cVB.bVB_AlwPrnLog);
                aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                new cLog().C_WRTxLog("wPmtGetorSug", "W_SHWxButtonBar : End Function C_GETaMenuBar", cVB.bVB_AlwPrnLog);
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                //*Em 62-01-22  Waterpark
                aoMenuT = (from oBar in aoKb where oBar.FNGdtPage == 1 orderby oBar.FNGdtUsrSeq select oBar).ToList();
                aoMenuB = (from oBar in aoKb where oBar.FNGdtPage == 2 orderby oBar.FNGdtUsrSeq select oBar).ToList();

                new cLog().C_WRTxLog("wPmtGetorSug", "W_SHWxButtonBar : Start Function GEN Menubar", cVB.bVB_AlwPrnLog);
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

                new cLog().C_WRTxLog("wPmtGetorSug", "W_SHWxButtonBar : End Function GEN Menubar", cVB.bVB_AlwPrnLog);
                //++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtGetorSug", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process Back
        /// </summary>
        private void W_PRCxBack()
        {
            new cLog().C_WRTxLog("wPmtGetorSug", "W_PRCxBack : Start Button", cVB.bVB_AlwPrnLog);
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

                        new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxClearCallBack : Start Function", cVB.bVB_AlwPrnLog);

                        if (cSale.nC_DocType == 1) if (cPayment.C_PRCbCancelCoupon(1, cVB.tVB_DocNo) == false) return; //*Em 63-01-09  
                        cSale.C_PRCxClearCallBack();    //*Em 62-10-09

                        new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxClearCallBack : End Function", cVB.bVB_AlwPrnLog);

                        cVB.nVB_CstPoint = cVB.nVB_CstPiontB4Used; //*Arm 63-03-11 คืนค่า point
                        cSale.nC_RDSeqNo = 0; //*Arm 63-03-11 คืนค่า Seq SalRD

                        //cVB.tVB_KbdScreen = "SALE";

                        //*Net 63-07-31 ปรับหน้าจอ 2 ใหม่
                        //if (cVB.oVB_2ndScreen != null)
                        //{
                        //    cVB.oVB_2ndScreen.W_PRCxBack();
                        //    cVB.oVB_2ndScreen.Update();
                        //}
                        nw_ModeClose = 1;
                        new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxClearExc : Start Function", cVB.bVB_AlwPrnLog);
                        C_PRCxClearExc();
                        new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxClearExc : End Function", cVB.bVB_AlwPrnLog);

                        if (cVB.bVB_PriceConfirm) cVB.oVB_Sale.W_PRCxPriceConfirm();    //*Em 63-05-07

                        if (cVB.bVB_PmtPriGrp)
                        {
                            cVB.oVB_Sale.bW_CalPmtPrice = true; //*Em 63-05-05
                            Thread oCalPro = new Thread(cVB.oVB_Sale.W_PRCxCoPmt);
                            oCalPro.IsBackground = true;
                            oCalPro.Priority = ThreadPriority.Highest;
                            oCalPro.Start();
                        }

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
            new cLog().C_WRTxLog("wPmtGetorSug", "W_PRCxBack : End Button", cVB.bVB_AlwPrnLog);
        }

        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                switch (poGD.Name)
                {
                    case "ogdGetPmt":
                        
                        nWidth = poGD.Width;
                        poGD.Cols["FNSeqNo"].Width = nWidth *8 / 100;
                        poGD.Cols["FTPmhName"].Width = nWidth * 40 / 100;
                        poGD.Cols["FCPmhDis"].Width = nWidth * 12 / 100;
                        poGD.Cols["FCPmhQtyDiv"].Width = nWidth * 12 / 100;
                        poGD.Cols["FNPmhPnt"].Width = nWidth * 12 / 100;
                        //poGD.Cols["FTPmhStaRcvPmt"].Width = nWidth * 8 / 100;
                        poGD.Cols["FCPmhQtySet"].Width = nWidth * 8 / 100;   //*Em 63-09-15
                        poGD.Cols["FTPmhStaEdit"].Width = nWidth * 8/ 100;

                        poGD.Cols["FNSeqNo"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                        poGD.Cols["FTPmhName"].Caption = cVB.oVB_GBResource.GetString("tPromotion");
                        poGD.Cols["FCPmhDis"].Caption = cVB.oVB_GBResource.GetString("tDis");
                        poGD.Cols["FCPmhQtyDiv"].Caption = cVB.oVB_GBResource.GetString("tPrivilege");
                        poGD.Cols["FNPmhPnt"].Caption = cVB.oVB_GBResource.GetString("tPoint");
                        //poGD.Cols["FTPmhStaRcvPmt"].Caption = cVB.oVB_GBResource.GetString("tReceive");
                        poGD.Cols["FCPmhQtySet"].Caption = cVB.oVB_GBResource.GetString("tSet"); //*Em 63-09-14
                        poGD.Cols["FTPmhStaEdit"].Caption = cVB.oVB_GBResource.GetString("tItem");

                        poGD.Cols["FNSeqNo"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPmhName"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FCPmhDis"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FCPmhQtyDiv"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FNPmhPnt"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPmhStaRcvPmt"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPmhStaEdit"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["FTPmhStaEdit"].DataType = typeof(Image);
                        poGD.Cols["FTPmhStaEdit"].ImageAlign = ImageAlignEnum.CenterCenter;

                        poGD.Cols["FNSeqNo"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPmhName"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FCPmhDis"].TextAlign = TextAlignEnum.RightCenter;
                        poGD.Cols["FCPmhQtyDiv"].TextAlign = TextAlignEnum.RightCenter;
                        poGD.Cols["FNPmhPnt"].TextAlign = TextAlignEnum.RightCenter;
                        poGD.Cols["FCPmhQtySet"].TextAlign = TextAlignEnum.CenterCenter; //*Em 63-09-14

                        poGD.Cols["FCPmhDis"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        poGD.Cols["FCPmhQtyDiv"].Format = "###,###,##0";
                        poGD.Cols["FNPmhPnt"].Format = "###,###,##0";
                        poGD.Cols["FCPmhQty"].Format = "###,###,##0";   //*Em 63-07-14
        
                        break;

                    case "ogdSugPmt":
                        nWidth = poGD.Width;
                        poGD.Cols["FNSeqNo"].Width = nWidth * 8 / 100;
                        poGD.Cols["FTPmhName"].Width = nWidth * 44 / 100;
                        poGD.Cols["FCPmhDis"].Width = nWidth * 12 / 100;
                        poGD.Cols["FCPmhQtyDiv"].Width = nWidth * 12 / 100;
                        poGD.Cols["FNPmhPnt"].Width = nWidth * 12 / 100;
                        poGD.Cols["FNPmhAdd"].Width = nWidth * 12 / 100;

                        poGD.Cols["FNSeqNo"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                        poGD.Cols["FTPmhName"].Caption = cVB.oVB_GBResource.GetString("tPromotion");
                        poGD.Cols["FCPmhDis"].Caption = cVB.oVB_GBResource.GetString("tDis");
                        poGD.Cols["FCPmhQtyDiv"].Caption = cVB.oVB_GBResource.GetString("tPrivilege");
                        poGD.Cols["FNPmhPnt"].Caption = cVB.oVB_GBResource.GetString("tPoint");
                        poGD.Cols["FNPmhAdd"].Caption = cVB.oVB_GBResource.GetString("tAdd");

                        poGD.Cols["FNSeqNo"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPmhName"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FCPmhDis"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FCPmhQtyDiv"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FNPmhPnt"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FNPmhAdd"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["FNSeqNo"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPmhName"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FCPmhDis"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FCPmhQtyDiv"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FNPmhPnt"].TextAlign = TextAlignEnum.RightCenter;
                        poGD.Cols["FNPmhAdd"].TextAlign = TextAlignEnum.RightCenter;

                        poGD.Cols["FCPmhDis"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        poGD.Cols["FCPmhQtyDiv"].Format = "###,###,##0";
                        poGD.Cols["FNPmhPnt"].Format = "###,###,##0";
                        poGD.AllowEditing = false;
                        break;

                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_SETxColGrid : " + oEx.Message);
            }
        }
        #endregion

        #region Event 

        private void OcmCashPay_Click(object sender, EventArgs e)
        {
            new cLog().C_WRTxLog("wPmtGetorSug", "OcmCashPay_Click : Start Button", cVB.bVB_AlwPrnLog);
            try
            {
                if (ogdGetPmt.Rows.Count > 0)
                {
                    new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxPmtDisProratePD : Start Function", cVB.bVB_AlwPrnLog);
                    new cPdtPmt().C_PRCxPmtDisProratePD();
                    new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxPmtDisProratePD : End Function", cVB.bVB_AlwPrnLog);
                }

                //*Arm 63-06-19 Cmoment Code
                ////oW_PdtPmt.C_PRCxPmtDisHD();
                //new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxSummary2HD : Start Function");
                //cSale.C_PRCxSummary2HD();
                //new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxSummary2HD : End Function");
                ////cVB.cVB_Amount = Convert.ToDecimal(olaCashPayment.Text);
                //new wPayment(3).ShowDialog();
                //this.Close();

                W_CHKbCheckStock();     //*Arm 63-06-19 P1RFP-002 ตรวจสอบ Stock แบบ Realtime จากระบบ KADS

            }
            catch (Exception oEX) { }
            new cLog().C_WRTxLog("wPmtGetorSug", "OcmCashPay_Click : End Button", cVB.bVB_AlwPrnLog);
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
            try
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "C_SETxPdtPmtToGrid : Start Function", cVB.bVB_AlwPrnLog);
                DataTable oDbTblDist;
                //wPmtGetorSug oW_PmtGetorSug;
                oW_PdtPmt = new cPdtPmt();
                var distinctRows = (from DataRow dRow in poDbTblPdt.Rows
                                    where dRow.Field<decimal?>("FCXpdGetQtyDiv") > 0
                                    select new
                                    {
                                        FTPmhCode = dRow.Field<string>("FTPmhCode"),
                                        FTPmhName = oW_PdtPmt.C_GETtNamePmt(dRow.Field<string>("FTPmhCode")),        //*Em 63-06-01
                                        FCXpdDis = dRow.Field<decimal?>("FCXpdDis"),
                                        FCXpdGetQtyDiv = dRow.Field<decimal?>("FCXpdGetQtyDiv"),
                                        FCXpdPoint = dRow.Field<Int64?>("FCXpdPoint"),
                                        FTPgtCpnText = dRow.Field<string>("FTPgtCpnText"),
                                        FTCpdBarCpn = dRow.Field<string>("FTCpdBarCpn"),
                                        FTPmhStaChkQuota = dRow.Field<string>("FTPmhStaChkQuota"),
                                        FTPmhStaRcvFree = dRow.Field<string>("FTPmhStaRcvFree")
                                    }
                                                ).Distinct();

                oDbTblDist = oW_PdtPmt.C_CONoFill(distinctRows);
                //int nIndex = 0;
                new cLog().C_WRTxLog("wPmtGetorSug", "C_SETxPdtPmtToGrid : Loop Start", cVB.bVB_AlwPrnLog);
                //foreach (DataRow oRow in oDbTblDist.Rows)
                //{

                //    nIndex = nIndex + 1;
                //    cmlPmtGet oPmtGet = new cmlPmtGet();
                //    oPmtGet.tSeq = nIndex.ToString();
                //    oPmtGet.tPromotion = oW_PdtPmt.C_GETtNamePmt(oRow.Field<string>("FTPmhCode"));
                //    oPmtGet.tPrmCode = oRow.Field<string>("FTPmhCode");
                //    oPmtGet.tDiscount = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow);
                //    if (oRow.Field<decimal?>("FCXpdGetQtyDiv") == null)
                //    {
                //        oPmtGet.tRedame = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                //    }
                //    else
                //    {
                //        if (!string.IsNullOrEmpty(oRow.Field<string>("FTPgtCpnText")) || !string.IsNullOrEmpty(oRow.Field<string>("FTCpdBarCpn")))
                //        {
                //            oPmtGet.tRedame = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdGetQtyDiv"), cVB.nVB_DecShow);
                //        }
                //        else
                //        {
                //            oPmtGet.tRedame = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                //        }

                //    }

                //    if (oRow.Field<Int64?>("FCXpdPoint") == null)
                //    {
                //        oPmtGet.tPoint = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                //    }
                //    else
                //    {
                //        oPmtGet.tPoint = oW_SP.SP_SETtDecShwSve(1, oRow.Field<Int64>("FCXpdPoint"), cVB.nVB_DecShow);
                //    }
                //    W_SETxDataPmtGet(oPmtGet);

                //}

                //*Em 63-06-01
                ogdGetPmt.Rows.Count = ogdGetPmt.Rows.Fixed;
                int nIndex = 0;
                foreach (DataRow oRow in oDbTblDist.Rows)
                {
                    if (oRow.Field<decimal>("FCXpdGetQtyDiv") > 0)
                    {
                        nIndex = nIndex + 1;
                        ogdGetPmt.Rows.Add();
                        ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FTPmhCode"].Index, oRow.Field<string>("FTPmhCode"));
                        ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FNSeqNo"].Index, nIndex);
                        ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FTPmhName"].Index, oRow.Field<string>("FTPmhName"));
                        ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FCPmhDis"].Index, oRow.Field<decimal>("FCXpdDis"));
                        if (oRow.Field<decimal?>("FCXpdGetQtyDiv") == null)
                        {
                            ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FCPmhQtyDiv"].Index, 0);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(oRow.Field<string>("FTPgtCpnText")) || !string.IsNullOrEmpty(oRow.Field<string>("FTCpdBarCpn")))
                            {
                                ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FCPmhQtyDiv"].Index, oRow.Field<decimal>("FCXpdGetQtyDiv"));
                            }
                            else
                            {
                                ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FCPmhQtyDiv"].Index, 0);
                            }

                        }

                        if (oRow.Field<Int64?>("FCXpdPoint") == null)
                        {
                            ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FNPmhPnt"].Index, 0);
                        }
                        else
                        {
                            ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FNPmhPnt"].Index, Convert.ToInt64(oRow.Field<Int64?>("FCXpdPoint")));
                        }
                        ////ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FTPmhStaRcvPmt"].Index, true);
                        ////*Em 63-07-14
                        //if (oRow.Field<string>("FTPmhStaRcvFree") == "2")
                        //{
                        //    CellStyle oCellStyle;
                        //    oCellStyle = ogdGetPmt.Styles.Add("Receive");
                        //    oCellStyle.DataType = typeof(Boolean);
                        //    ogdGetPmt.SetCellStyle(nIndex, ogdGetPmt.Cols["FTPmhStaRcvPmt"].Index, oCellStyle);
                        //    ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FTPmhStaRcvPmt"].Index, true);
                        //}

                        //*Em 63-09-14
                        ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FCPmhQtySet"].Index, "*" + oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdGetQtyDiv"),0));
                        ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FTPmhStaRcvPmt"].Index, oRow.Field<string>("FTPmhStaRcvFree"));
                        //++++++++++++++

                        ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FTPmhStaChkQuota"].Index, oRow.Field<string>("FTPmhStaChkQuota"));

                        //*Em 63-07-14
                        ogdGetPmt.SetData(ogdGetPmt.Rows.Count - ogdGetPmt.Rows.Fixed, ogdGetPmt.Cols["FCPmhQty"].Index, oRow.Field<decimal>("FCXpdGetQtyDiv"));
                        //if (oRow.Field<string>("FTPmhStaChkQuota") == "1")
                        if (oRow.Field<string>("FTPmhStaRcvFree") == "2")   //*Em 63-09-14
                        {
                            ogdGetPmt.SetCellImage(nIndex, ogdGetPmt.Cols["FTPmhStaEdit"].Index, global::AdaPos.Properties.Resources.Edit_32);
                            CellStyle oCellStyle;
                            oCellStyle = ogdGetPmt.Styles.Add("Edit");
                            oCellStyle.BackColor = cVB.oVB_ColDark;
                            ogdGetPmt.SetCellStyle(nIndex, ogdGetPmt.Cols["FTPmhStaEdit"].Index, oCellStyle);
                        }
                    }
                }
                //+++++++++++++++++
                new cLog().C_WRTxLog("wPmtGetorSug", "C_SETxPdtPmtToGrid : Loop End", cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("wPmtGetorSug", "C_SETxPdtPmtToGrid : End Function", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "C_SETxPdtPmtToGrid : " + oEx.Message);
            }
            finally
            {
                ////new cSP().SP_CLExMemory();
            }
            
        }

        public void C_SETxPdtSugToGrid(DataTable poDbTbl)
        {
            int nRowFind = 0;
            try
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "C_SETxPdtSugToGrid : Loop Start", cVB.bVB_AlwPrnLog);
                //int nIndex = 0;
                //foreach (DataRow oRow in poDbTbl.Rows)
                //{
                //    nIndex = nIndex + 1;
                //    cmlPmtSug oPmtSug = new cmlPmtSug();
                //    oPmtSug.tSeq = nIndex.ToString();
                //    oPmtSug.tPromotion = new cPdtPmt().C_GETtNamePmt(oRow.Field<string>("FTPmhCode"));
                //    oPmtSug.tDiscount = oPmtSug.tRedame = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow);
                //    if (oRow.Field<decimal?>("FCQtyCpn") == null)
                //    {
                //        oPmtSug.tRedame = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                //    }
                //    else
                //    {
                //        oPmtSug.tRedame = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCQtyCpn"), cVB.nVB_DecShow);
                //    };
                //    if (oRow.Field<Int64>("FNQtyPnt") == 0)
                //    {
                //        oPmtSug.tPoint = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                //    }
                //    else
                //    {
                //        oPmtSug.tPoint = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FNQtyPnt"), cVB.nVB_DecShow);
                //    };
                //    if (oRow.Field<decimal?>("FCQtyAdd") == null)
                //    {
                //        oPmtSug.tPmtAdd = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                //    }
                //    else
                //    {
                //        oPmtSug.tPmtAdd = oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCQtyAdd"), cVB.nVB_DecShow);
                //    };
                //    W_SETxDataPmtSug(oPmtSug);

                //}
                
                //*Em 63-06-01
                ogdSugPmt.Rows.Count = ogdSugPmt.Rows.Fixed;
                int nIndex = 0;
                foreach (DataRow oRow in poDbTbl.Rows)
                {
                    nRowFind = ogdSugPmt.FindRow(oRow.Field<string>("FTPmhName"), ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FTPmhName"].Index, true);
                    if (nRowFind >= ogdSugPmt.Rows.Fixed)
                    {
                        ogdSugPmt.SetData(nRowFind, ogdSugPmt.Cols["FCPmhQtyDiv"].Index, oRow.Field<string>("FCQtyCpn"));
                        if (oRow.Field<Int64>("FNQtyPnt") == 0)
                        {
                            ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FNPmhPnt"].Index, 0);
                        }
                        else
                        {
                            ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FNPmhPnt"].Index, oRow.Field<Int64>("FNQtyPnt"));
                        };
                    }
                    else
                    {
                        nIndex = nIndex + 1;
                        ogdSugPmt.Rows.Add();
                        ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FNSeqNo"].Index, nIndex);
                        ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FTPmhName"].Index, oRow.Field<string>("FTPmhName"));
                        //ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhDis"].Index, oRow.Field<decimal>("FCXpdDis"));
                        //*Em 63-07-16
                        switch (oRow.Field<string>("FTPgtStaGetType"))
                        {
                            case "1":
                                ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhDis"].Index, oW_Resource.GetString("tPmtDis") + " " + oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow));
                                break;
                            case "2":
                                ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhDis"].Index, oW_Resource.GetString("tPmtDis") + " " + oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow) + " %");
                                break;
                            case "3":
                                ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhDis"].Index, oW_Resource.GetString("tPmtAdjPrice"));
                                break;
                            case "4":
                                ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhDis"].Index, oW_Resource.GetString("tPmtPriGrp"));
                                break;
                            case "5":
                                ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhDis"].Index, oW_Resource.GetString("tPmtFree"));
                                break;
                            default:
                                ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhDis"].Index, "");
                                break;
                        }
                        //+++++++++++++++++++
                        //if (Convert.ToDecimal(oRow.Field<string>("FCQtyCpn").ToString()) == Convert.ToDecimal(0))
                        //{
                        //    ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhQtyDiv"].Index, "");
                        //}
                        //else
                        //{
                        ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FCPmhQtyDiv"].Index, oRow.Field<string>("FCQtyCpn"));
                        //};
                        if (oRow.Field<Int64>("FNQtyPnt") == 0)
                        {
                            ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FNPmhPnt"].Index, 0);
                        }
                        else
                        {
                            ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FNPmhPnt"].Index, oRow.Field<Int64>("FNQtyPnt"));
                        };
                        if (Convert.ToInt64(oRow.Field<decimal>("FCQtyAdd")) == 0)
                        {
                            ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FNPmhAdd"].Index, 0);
                        }
                        else
                        {
                            ogdSugPmt.SetData(ogdSugPmt.Rows.Count - ogdSugPmt.Rows.Fixed, ogdSugPmt.Cols["FNPmhAdd"].Index, Convert.ToInt64(oRow.Field<decimal>("FCQtyAdd")));
                        };
                    }
                    
                }
                new cLog().C_WRTxLog("wPmtGetorSug", "C_SETxPdtSugToGrid : Loop End", cVB.bVB_AlwPrnLog);
                //+++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "C_SETxPdtSugToGrid : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            
            
        }

        private void WPmtGetorSug_Shown(object sender, EventArgs e)
        {
            new cLog().C_WRTxLog("wPmtGetorSug", "WPmtGetorSug_Shown : Start Function", cVB.bVB_AlwPrnLog);
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

                //*Em 63-06-01
                W_SETxColGrid(ogdGetPmt);
                W_SETxColGrid(ogdSugPmt);
                //++++++++++++++++

                //*Net 63-07-31 sync ข้อความแถบด้านบนกับหน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                }

                //*Em 63-05-26
                Form oFormShow = null;
                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wPmtPriConfirm);
                if (oFormShow != null) oFormShow.Hide();
            }
            catch(Exception oEx)
            {

            }
            new cLog().C_WRTxLog("wPmtGetorSug", "WPmtGetorSug_Shown : End Function", cVB.bVB_AlwPrnLog);


        }

        private void OgdGetPmt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //new cLog().C_WRTxLog("wPmtGetorSug", "OgdGetPmt_CellContentClick : Start Function");
            //if (e.ColumnIndex == ogdGetPmt.Columns[6].Index)
            //{
            //    if (Convert.ToBoolean(ogdGetPmt.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue))
            //    {
            //       var s = ogdGetPmt.Rows[e.RowIndex].Cells[0].Value;
            //    }
            //    else
            //    {
            //        new cLog().C_WRTxLog("wPmtGetorSug", "OgdGetPmt_CellContentClick : Start C_PRCxPmtExc");
            //        C_PRCxPmtExc(ogdGetPmt.Rows[e.RowIndex].Cells[0].Value.ToString());

            //        ogdGetPmt.Rows.Clear();
            //        new cLog().C_WRTxLog("wPmtGetorSug", "OgdGetPmt_CellContentClick : Start C_PRCxPrepareDT");
            //        new cPdtPmt().C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);

            //        new cLog().C_WRTxLog("wPmtGetorSug", "OgdGetPmt_CellContentClick : Start C_PRCoCalPmt");
            //        new cPdtPmt().C_PRCoCalPmt("2");


            //        if (cVB.oVB_GetPmt.Rows.Count > 0)
            //        {
            //            new cLog().C_WRTxLog("wPmtGetorSug", "OgdGetPmt_CellContentClick : Start C_PRCbINSPmtPD");
            //            new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
            //            C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
            //        }
            //    }
            //}
            //else
            //{

            //}
            //new cLog().C_WRTxLog("wPmtGetorSug", "OgdGetPmt_CellContentClick : End Function");
        }

        public void C_PRCxPmtExc(string ptPmtCode)
        {
            new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxPmtExc : Start Insert TPMTPmtExc", cVB.bVB_AlwPrnLog);
            StringBuilder oSql = new StringBuilder();
            cDatabase oDatabase = new cDatabase();

            try
            {

                oSql.Clear();
                //oSql.AppendLine("TRUNCATE TABLE TPMTPmtExc ");
                oSql.AppendLine("INSERT INTO TPMTPmtExc "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("(");
                oSql.AppendLine("  FTXshDocNo,FTPmhDocNo");
                oSql.AppendLine(")VALUES (");
                oSql.AppendLine("  '" + cVB.tVB_DocNo + "','" + ptPmtCode + "'");
                oSql.AppendLine(")");

                //*Em 63-06-01
                oSql.AppendLine("");
                oSql.AppendLine("DELETE FROM " + cSale.tC_TblSalPD);
                oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"' AND FTXshDocNo = '"+ cVB.tVB_DocNo + "' AND FTPmhDocNo = '" + ptPmtCode + "'");
                //+++++++++++++++

                oDatabase.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxPmtExc : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDatabase = null;

                //new cSP().SP_CLExMemory();
            }
            new cLog().C_WRTxLog("wPmtGetorSug", "C_PRCxPmtExc : End Insert TPMTPmtExc", cVB.bVB_AlwPrnLog);
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

                //new cSP().SP_CLExMemory();
            }
        }

        private void ogdGetPmt_CellChecked(object sender, RowColEventArgs e)
        {
            try
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "ogdGetPmt_CellChecked : Start Function", cVB.bVB_AlwPrnLog);
                if (e.Col == ogdGetPmt.Cols["FTPmhStaRcvPmt"].Index)
                {
                    if (Convert.ToBoolean(ogdGetPmt.GetData(e.Row,e.Col)) == false)
                    {
                        new cLog().C_WRTxLog("wPmtGetorSug", "ogdGetPmt_CellChecked : Start C_PRCxPmtExc", cVB.bVB_AlwPrnLog);
                        C_PRCxPmtExc(ogdGetPmt.GetData(e.Row,ogdGetPmt.Cols["FTPmhCode"].Index).ToString());

                        ogdGetPmt.Rows.Count = ogdGetPmt.Rows.Fixed;
                        new cLog().C_WRTxLog("wPmtGetorSug", "ogdGetPmt_CellChecked : Start C_PRCxPrepareDT", cVB.bVB_AlwPrnLog);
                        new cPdtPmt().C_PRCxPrepareDT(cSale.tC_TblSalDT, cVB.tVB_DocNo);

                        new cLog().C_WRTxLog("wPmtGetorSug", "ogdGetPmt_CellChecked : Start C_PRCoCalPmt", cVB.bVB_AlwPrnLog);
                        new cPdtPmt().C_PRCoCalPmt("2");

                        if (cVB.oVB_GetPmt.Rows.Count > 0)
                        {
                            new cLog().C_WRTxLog("wPmtGetorSug", "ogdGetPmt_CellChecked : Start C_PRCbINSPmtPD", cVB.bVB_AlwPrnLog);
                            new cPdtPmt().C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                            C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                        }
                    }
                }
                new cLog().C_WRTxLog("wPmtGetorSug", "OgdGetPmt_CellContentClick : End Function", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "ogdGetPmt_CellChecked : " + oEx.Message);
            }
            finally
            {

            }
        }

        /// <summary>
        /// *Arm 63-06-19 P1RFP-002 ตรวจสอบ Stock แบบ Realtime จากระบบ KADS 
        /// </summary>
        /// <returns></returns>
        public void W_CHKbCheckStock()
        {
            try
            {
                if (cSale.nC_DocType != 9 && !string.IsNullOrEmpty(cVB.tVB_WahStaChkStk) && cVB.tVB_WahStaChkStk == "3") //(TCNMWaHouse.FTWahStaChkStk = 3 :ใช้ตรวจสอบในขั้นตอนการขาย 3: Check Online  )
                {
                    if (new cStock().C_CHKbSubTotalCheckStock() == false)
                    {
                        W_PRCxBack();
                    }
                    else
                    {
                        cSale.C_PRCxSummary2HD();
                        new wPayment(3).ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    cSale.C_PRCxSummary2HD();
                    new wPayment(3).ShowDialog();
                    this.Close();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "W_CHKbCheckStock " + oEx.Message);
            }
        }

        private void ogdGetPmt_EnterCell(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tPmhDocNo = "";
            cPdtPmt oPmt = new cPdtPmt();
            try
            {
                if (ogdGetPmt.Col != ogdGetPmt.Cols["FTPmhStaEdit"].Index)
                {
                    ogdGetPmt.FocusRect = FocusRectEnum.Light;
                    return;
                }

                //if (ogdGetPmt.GetData(ogdGetPmt.Row, ogdGetPmt.Cols["FTPmhStaChkQuota"].Index).ToString() != "1")
                if (ogdGetPmt.GetData(ogdGetPmt.Row, ogdGetPmt.Cols["FTPmhStaRcvPmt"].Index).ToString() == "1")   //*Em 63-09-15
                {
                    ogdGetPmt.FocusRect = FocusRectEnum.Light;
                    return;
                }
                else
                {

                    tPmhDocNo = ogdGetPmt.GetData(ogdGetPmt.Row, ogdGetPmt.Cols["FTPmhCode"].Index).ToString();
                    cSale.cC_DTQty = Convert.ToDecimal( ogdGetPmt.GetData(ogdGetPmt.Row, ogdGetPmt.Cols["FCPmhQty"].Index).ToString());
                    //cSale.cC_QuotaLimit = oPmt.C_GETcLimitQuotaByPmt(tPmhDocNo);

                    //*Em 63-09-15
                    if (ogdGetPmt.GetData(ogdGetPmt.Row, ogdGetPmt.Cols["FTPmhStaChkQuota"].Index).ToString() == "1")
                    {
                        cSale.cC_QuotaLimit = oPmt.C_GETcLimitQuotaByPmt(tPmhDocNo);
                        cSale.cC_QtyLimit = Convert.ToDecimal(cVB.oVB_GetPmt.Select("FTPmhCode = '" + tPmhDocNo + "'").FirstOrDefault().Field<decimal>("FCXsdQty").ToString());
                    }
                    else
                    {
                        cSale.cC_QtyLimit = 0;
                        cSale.cC_QuotaLimit = Convert.ToDecimal(ogdGetPmt.GetData(ogdGetPmt.Row, ogdGetPmt.Cols["FCPmhQty"].Index).ToString());
                    }
                    //++++++++++++++++++

                    if (new wChangePdtQty(2).ShowDialog() == DialogResult.OK)
                    {   
                        //*Em 63-08-16
                        if (cSale.cC_DTQty == Convert.ToDecimal(0))
                        {
                            oSql.Clear();
                            oSql.AppendLine("DELETE FROM TPMTPmtTmp WITH(ROWLOCK)");
                            //oSql.AppendLine("WHERE FTPmhStaChkQuota = '1'");
                            //oSql.AppendLine("AND FTPmhDocNo = '" + tPmhDocNo + "'");
                            oSql.AppendLine("WHERE FTPmhDocNo = '" + tPmhDocNo + "'");  //*Em 63-09-15
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }
                        //++++++++++++++++
                        else
                        {
                            oSql.Clear();
                            oSql.AppendLine("UPDATE TPMTPmtTmp WITH(ROWLOCK)");
                            oSql.AppendLine("SET FCPbyMaxValue = FCPbyMinValue * " + cSale.cC_DTQty);
                            //oSql.AppendLine("WHERE FTPmhStaChkQuota = '1'");
                            //oSql.AppendLine("AND FTPmhDocNo = '" + tPmhDocNo + "'");
                            oSql.AppendLine("WHERE FTPmhDocNo = '" + tPmhDocNo + "'");  //*Em 63-09-15
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }
                        

                        oPmt.C_PRCoCalPmt("2");
                        if (cVB.oVB_GetPmt.Rows.Count > 0)
                        {
                            oPmt.C_PRCbINSPmtPD(cVB.oVB_GetPmt);
                            C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                        }
                        else
                        {
                            //*Em 63-08-17
                            oSql.Clear();
                            oSql.AppendLine("DELETE FROM " + cSale.tC_TblSalPD + " WITH(ROWLOCK)");
                            oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                            oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                            if (cVB.oVB_Sale.bW_CalPmtPrice == false && cVB.bVB_PriceConfirm == true) oSql.AppendLine("AND FTXpdGetType <> '4' ");
                            oDB.C_SETxDataQuery(oSql.ToString());
                            C_SETxPdtPmtToGrid(cVB.oVB_GetPmt);
                            //++++++++++++++
                        }
                    }
                    ogdGetPmt.Select(ogdGetPmt.Row, ogdGetPmt.Cols["FTPmhStaRcvPmt"].Index);  //*Em 63-09-15
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "ogdGetPmt_EnterCell : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                oPmt = null;
                //new cSP().SP_CLExMemory();
            }
        }

        private void ogdGetPmt_BeforeEdit(object sender, RowColEventArgs e)
        {
            try
            {
                //if (e.Col == ogdGetPmt.Cols["FTPmhStaChkQuota"].Index)
                //{
                //    if (ogdGetPmt.GetData(ogdGetPmt.Row, ogdGetPmt.Cols["FTPmhStaChkQuota"].Index).ToString() != "1")
                //    {
                //        e.Cancel = true;
                //        ogdGetPmt.FocusRect = FocusRectEnum.Light;
                //        return;
                //    }
                //}
                
                //*Em 63-09-14
                switch (ogdGetPmt.Cols[e.Col].Name)
                {
                    case "FTPmhStaEdit":
                        if (ogdGetPmt.GetData(ogdGetPmt.Row, ogdGetPmt.Cols["FTPmhStaChkQuota"].Index).ToString() != "1")
                        {
                            e.Cancel = true;
                            ogdGetPmt.FocusRect = FocusRectEnum.Light;
                            return;
                        }
                        break;
                    default:
                        e.Cancel = true;
                        ogdGetPmt.FocusRect = FocusRectEnum.Light;
                        break;
                }
                //+++++++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPmtGetorSug", "ogdGetPmt_BeforeEdit : " + oEx.Message);
            }
        }
    }
}
