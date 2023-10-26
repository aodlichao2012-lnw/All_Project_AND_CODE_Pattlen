using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Models.Other;
using AdaPos.Resources_String.Local;
using C1.Win.C1FlexGrid;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using File = System.IO.File;
using AdaPos.Properties;

namespace AdaPos.Forms
{
    public partial class wCstScreen : Form
    {
        private ResourceManager oW_Resource;
        private cSP oW_SP;
        private IWMPPlaylist oW_Playlist;
        private uMarqueeTxt oW_WelcomeTxt;
        private uMarqueeTxt oW_InfoTxt;
        private uShowQR oW_QRPP;

        //private List<cmlAdMsgMedia> aoW_AdMsg;
        public List<cmlAdMsgMedia> aoW_AdMsg;   //*Arm 63-08-13 ปรับเป็น Public
        private List<cmlAdMsgMedia> aoW_AdMedia;
        private List<string> atW_InfoMsg;
        private List<string> atW_Welcome;
        private List<string> atW_Thanks;


        public wCstScreen()
        {
            try
            {
                if (Screen.AllScreens.Length > 1)
                {
                    this.StartPosition = FormStartPosition.Manual;
                    this.Location = Screen.AllScreens.Where(oScrn => oScrn != Screen.PrimaryScreen).FirstOrDefault().WorkingArea.Location;
                }

                cVB.tVB_SaleDate = DateTime.Now.Date.ToString("yyyy-MM-dd");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                atW_InfoMsg = new List<string>();
                aoW_AdMsg = cCstScreen.C_GETaListAdMsg();


                W_SETxText();
                W_SETxDesign();
                //W_SETxMedia(); //*Arm 63-08-14 Comment Code
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }


        private void wCstScreen_Load(object sender, EventArgs e)
        {
            try
            {
                W_SETxColGrid(ogdPdtDT);
                W_SETxColGrid(ogdBillDT);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                oW_SP.SP_SETxSetGridFormat(ogdPdtDT);
                oW_SP.SP_SETxSetGridFormat(ogdBillDT);

                opnPdtSale.BackColor = cVB.oVB_ColDark;

                opnSummary.BackColor = cVB.oVB_ColLight;
                opnFooter.BackColor = cVB.oVB_ColLight;
                opnCstDT.BackColor = cVB.oVB_ColLight;


                opbIconUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode, "TCNMUser");
                opbLogo.Image = new cCompany().C_GEToImageLogo();
                if (opbLogo.Image != null)
                    opbLogo.Visible = true;

                oaxPlayer.uiMode = "none";

                cSP.SP_SETxFixPanelOverFlow(opnHeader, olaBranch);

                W_SETxHeaderLabelSize();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
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

                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                olaPosCode.Text = cVB.tVB_PosCode;
                //olaTitlePage.Text = oW_Resource.GetString("tSale");
                olaTitlePage.Text = "";
                olaUsrName.Text = "";

                if (aoW_AdMsg != null && aoW_AdMsg.Count > 0)
                {
                    //*คลิปวิดีโอ
                    aoW_AdMedia = aoW_AdMsg.Where(oAdMsg => oAdMsg.tAdvType == "3" || oAdMsg.tAdvType == "5" || oAdMsg.tAdvType == "6").ToList();

                    //*ประชาสัมพันธ์
                    atW_InfoMsg = aoW_AdMsg.Where(oMsg => oMsg.tAdvType == "2").Select(oMsg => oMsg.tAdvMsg).ToList();
                    if (atW_InfoMsg == null || atW_InfoMsg.Count == 0)
                    {
                        atW_InfoMsg.Add(cVB.oVB_GBResource.GetString("tInformation"));
                    }

                    //*ข้อความต้อนรับ
                    atW_Welcome = aoW_AdMsg.Where(oMsg => oMsg.tAdvType == "1").Select(oMsg => oMsg.tAdvMsg).ToList();
                    if (atW_Welcome == null || atW_Welcome.Count == 0)
                    {
                        atW_Welcome.Add(cVB.oVB_GBResource.GetString("tWelcome"));
                    }

                    //*ข้อความขอบคุณ
                    atW_Thanks = aoW_AdMsg.Where(oMsg => oMsg.tAdvType == "4").Select(oMsg => oMsg.tAdvMsg).ToList();
                    if (atW_Thanks == null || atW_Thanks.Count == 0)
                    {
                        atW_Thanks.Add(cVB.oVB_GBResource.GetString("tThanks"));
                    }
                }
                else
                {
                    atW_InfoMsg = new List<string>();
                    atW_Welcome = new List<string>();
                    atW_Thanks = new List<string>();

                    atW_InfoMsg.Add(cVB.oVB_GBResource.GetString("tInformation"));
                    atW_Welcome.Add(cVB.oVB_GBResource.GetString("tWelcome"));
                    atW_Thanks.Add(cVB.oVB_GBResource.GetString("tThanks"));
                }

                oW_WelcomeTxt = new uMarqueeTxt(patMqTxt: atW_Welcome,
                                    pnInterval: (atW_Welcome.Count > 1) ? 500 : 0,
                                    pnStep: 15,
                                    pnSpace: (opnWelcome.Width) / TextRenderer.MeasureText(" ", new Font("Segoe UI Light", 20f, FontStyle.Bold)).Width,
                                    poFont: new Font("Segoe UI Light", 20f, FontStyle.Bold),
                                    ptQuoteF: "",   //*Arm 63-08-13 เอา \" ออก
                                    ptQuoteR: "");  //*Arm 63-08-13 เอา \" ออก
                oW_WelcomeTxt.Dock = DockStyle.Fill;

                opnWelcome.Controls.Add(oW_WelcomeTxt);

                oW_InfoTxt = new uMarqueeTxt(patMqTxt: atW_InfoMsg,
                                    pnInterval: 500,
                                    pnStep: 15,
                                    pnSpace:(opnFooter.Width) / TextRenderer.MeasureText(" ", new Font("Segoe UI Light", 12f, FontStyle.Regular)).Width,
                                    ptQuoteF: "",     //*Arm 63-08-13 เอา \" ออก
                                    ptQuoteR: "");    //*Arm 63-08-13 เอา \" ออก
                oW_InfoTxt.Dock = DockStyle.Fill;
                opnFooter.Controls.Add(oW_InfoTxt);

                //olaCstName.Text = tW_Welcome;
                olaTitleCstPoint.Text = oW_Resource.GetString("tTitlePoint");
                opnCstPoint.Visible = false;

                olaTitleSummary.Text = cVB.oVB_GBResource.GetString("tPayment") + " :";

                this.Text = "Customer Screen";  //*Em 63-07-13
                W_SETxClearDoc();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        //private void W_SETxMedia()
        public void W_SETxMedia()
        {
            //*Arm 63-08-13 ปรับเป็น Public
            try
            {
                if (oW_Playlist != null)
                    oW_Playlist.clear();

                if (aoW_AdMedia == null || aoW_AdMedia.Count == 0) return;

                oW_Playlist = oaxPlayer.playlistCollection.newPlaylist("PosAds");

                W_CLExClearMediaUse();
                foreach(string tMediaPath in aoW_AdMedia.Select(oMedia => oMedia.tFTMedPath).ToList())
                {
                    if (!String.IsNullOrEmpty(tMediaPath) && File.Exists(tMediaPath))
                    {
                        oW_Playlist.appendItem(oaxPlayer.newMedia(W_GENxUseMedia(tMediaPath)));
                    }
                }
                oaxPlayer.settings.setMode("loop", true);
                oaxPlayer.currentPlaylist.clear();
                oaxPlayer.currentPlaylist = oW_Playlist;

                oaxPlayer.Ctlcontrols.play();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        private void W_CLExClearMediaUse()
        {
            string tPathFile;
            try
            {
                //tPathFile = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Media\Use";
                tPathFile = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Advertise\PlayNow"; //*Arm 63-08-13
                if (Directory.Exists(tPathFile)) Directory.Delete(tPathFile, true);

                Directory.CreateDirectory(tPathFile);

                //*Em 63-08-13
                tPathFile = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaMedia\Advertise\PlayNow";
                if (Directory.Exists(tPathFile)) Directory.Delete(tPathFile, true);

                Directory.CreateDirectory(tPathFile);
                //++++++++++++++

                //tPathFile = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Others\Use";
                tPathFile = Directory.GetParent(Application.StartupPath).ToString() + @"\AdaImage\Others\PlayNow"; //*Arm 63-08-13
                if (Directory.Exists(tPathFile)) Directory.Delete(tPathFile, true);

                Directory.CreateDirectory(tPathFile);

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        private string W_GENxUseMedia(string ptMediaPath)
        {
            List<string> atPathSection = new List<string>();
            string tNewMediaPath = "";
            string tPathFld = "";   //*Em 63-08-13
            try
            {
                atPathSection = ptMediaPath.Split('\\').ToList();
                if (atPathSection == null || atPathSection.Count == 0) return null;
                //atPathSection.Insert(atPathSection.Count - 1, "Use");
                atPathSection.Insert(atPathSection.Count - 1, "PlayNow"); //*Arm 63-08-13
                tNewMediaPath = String.Join("\\", atPathSection);

                //*Em 63-08-12
                tPathFld = Path.GetDirectoryName(tNewMediaPath);
                if (!Directory.Exists(tPathFld)) Directory.CreateDirectory(tPathFld);
                if (File.Exists(tNewMediaPath)) File.Delete(tNewMediaPath);
                //++++++++++++++

                File.Copy(ptMediaPath, tNewMediaPath, true);

                return tNewMediaPath;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                if (atPathSection != null) atPathSection.Clear();
                atPathSection = null;
                //new cSP().SP_CLExMemory();
            }
            return null;
        }

        /// <summary>
        /// Set Size of Label in Header
        /// </summary>
        public void W_SETxHeaderLabelSize()
        {
            Size oSTmp;
            try
            {
                oSTmp = olaBranch.Size;
                olaBranch.AutoSize = false;
                olaBranch.Size = new Size(oSTmp.Width, olaBranch.Size.Height);


                oSTmp = olaTitlePage.Size;
                olaTitlePage.AutoSize = false;
                olaTitlePage.Size = new Size(oSTmp.Width, olaTitlePage.Size.Height);

                oSTmp = olaPosCode.Size;
                olaPosCode.AutoSize = false;
                olaPosCode.Size = new Size(oSTmp.Width, olaPosCode.Size.Height);

                oSTmp = olaPosCode.Size;
                olaPosCode.AutoSize = false;
                olaPosCode.Size = new Size(oSTmp.Width, olaPosCode.Size.Height);

                oSTmp = olaDocNo.Size;
                olaDocNo.AutoSize = false;
                olaDocNo.Size = new Size(oSTmp.Width, olaDocNo.Size.Height);

                oSTmp = olaUsrName.Size;
                olaUsrName.AutoSize = false;
                olaUsrName.Size = new Size(oSTmp.Width, olaUsrName.Size.Height);
                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set Column Header of Grid
        /// </summary>
        /// <param name="poGD"></param>
        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                nWidth = poGD.Width;
                switch (poGD.Name)
                {
                    case "ogdPdtDT":
                        poGD.ExtendLastCol = false;
                        poGD.Cols["otbColSeqStd"].Width = (int)(nWidth * (15 / 100m));
                        poGD.Cols["otbColPdtNameStd"].Width = (int)(nWidth * (45 / 100m));
                        poGD.Cols["otbColPdtQtyStd"].Width = (int)(nWidth * (20 / 100m));
                        poGD.Cols["otbColPdtSumPriceStd"].Width = (int)(nWidth * (20 / 100m));


                        poGD.Cols["otbColSeqStd"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                        poGD.Cols["otbColPdtNameStd"].Caption = cVB.oVB_GBResource.GetString("tPdtList");
                        poGD.Cols["otbColPdtQtyStd"].Caption = cVB.oVB_GBResource.GetString("tQty");
                        poGD.Cols["otbColPdtSumPriceStd"].Caption = cVB.oVB_GBResource.GetString("tSummary");

                        poGD.Cols["otbColSeqStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColPdtNameStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColPdtQtyStd"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColPdtSumPriceStd"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["otbColSeqStd"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColPdtNameStd"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["otbColPdtQtyStd"].TextAlign = TextAlignEnum.RightCenter;
                        poGD.Cols["otbColPdtSumPriceStd"].TextAlign = TextAlignEnum.RightCenter;

                        poGD.Cols["otbColPdtQtyStd"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        poGD.Cols["otbColPdtSumPriceStd"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        break;
                    case "ogdBillDT":
                        poGD.ExtendLastCol = false;
                        poGD.Cols["otbColSeqBill"].Width = (int)(nWidth * (15 / 100m));
                        poGD.Cols["otbColDetailBill"].Width = (int)(nWidth * (65 / 100m));
                        poGD.Cols["otbColAmtBill"].Width = (int)(nWidth * (20 / 100m));

                        poGD.Cols["otbColSeqBill"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                        poGD.Cols["otbColDetailBill"].Caption = cVB.oVB_GBResource.GetString("tBillList");
                        poGD.Cols["otbColAmtBill"].Caption = cVB.oVB_GBResource.GetString("tSummary");

                        poGD.Cols["otbColSeqBill"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColDetailBill"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColAmtBill"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["otbColSeqBill"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["otbColDetailBill"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["otbColAmtBill"].TextAlign = TextAlignEnum.RightCenter;

                        poGD.Cols["otbColAmtBill"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }


        /// <summary>
        /// Clear All Page
        /// </summary>
        public void W_SETxClearDoc()
        {
            try
            {
                //olaCstName.Text = tW_Welcome;
                oW_WelcomeTxt = new uMarqueeTxt(patMqTxt: atW_Welcome,
                                    pnInterval: (atW_Welcome.Count > 1) ? 500 : 0,
                                    pnStep: 15,
                                    pnSpace: (opnWelcome.Width) / TextRenderer.MeasureText(" ", new Font("Segoe UI Light", 20f, FontStyle.Bold)).Width,
                                    poFont: new Font("Segoe UI Light", 20f, FontStyle.Bold),
                                    ptQuoteF: "",   //*Arm 63-08-13 เอา \" ออก
                                    ptQuoteR: "");  //*Arm 63-08-13 เอา \" ออก
                oW_WelcomeTxt.Dock = DockStyle.Fill;
                for (int nIndex = 0; nIndex < opnWelcome.Controls.Count; nIndex++)
                {
                    opnWelcome.Controls[nIndex].Dispose();
                }
                opnWelcome.Controls.Clear();
                opnWelcome.Controls.Add(oW_WelcomeTxt);

                olaCstPoint.Text = "";
                opnCstPoint.Visible = false;

                olaSummary.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow);
                olaLastAmtPdt.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow);
                olaLastPdt.Text = "";

                olaDocNo.Text = "";

                ogdBillDT.DataSource = null;
                ogdBillDT.Clear();
                ogdBillDT.Rows.Count = ogdBillDT.Rows.Fixed;

                ogdPdtDT.DataSource = null;
                ogdPdtDT.Clear();
                ogdPdtDT.Rows.Count = ogdPdtDT.Rows.Fixed;

                W_SETxColGrid(ogdPdtDT);
                W_SETxColGrid(ogdBillDT);

                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_SETxHeader(string ptBchName = "", string ptPageName = "", string ptPosCode = "", string ptDocNo = "", string ptUsrName = "")
        {
            try
            {
                olaBranch.AutoSize = true;
                olaTitlePage.AutoSize = true;
                olaPosCode.AutoSize = true;
                olaDocNo.AutoSize = true;
                olaUsrName.AutoSize = true;

                if (!String.IsNullOrEmpty(ptBchName))
                    olaBranch.Text = ptBchName;

                if (!String.IsNullOrEmpty(ptPageName))
                    olaTitlePage.Text = ptPageName;

                if (!String.IsNullOrEmpty(ptPosCode))
                    olaPosCode.Text = ptPosCode;

                if (!String.IsNullOrEmpty(ptDocNo))
                    olaDocNo.Text = ptDocNo;

                if (!String.IsNullOrEmpty(ptUsrName))
                    olaUsrName.Text = ptUsrName;

                W_SETxHeaderLabelSize();

                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_SETxThanks()
        {
            try
            {
                oW_WelcomeTxt = new uMarqueeTxt(patMqTxt: atW_Thanks,
                                    pnInterval: (atW_Thanks.Count > 1) ? 500 : 0,
                                    pnStep: 15,
                                    pnSpace: (opnWelcome.Width) / TextRenderer.MeasureText(" ", new Font("Segoe UI Light", 20f, FontStyle.Bold)).Width,
                                    poFont: new Font("Segoe UI Light", 20f, FontStyle.Bold),
                                    ptQuoteF: "",   //*Arm 63-08-13 เอา \" ออก
                                    ptQuoteR: "");  //*Arm 63-08-13 เอา \" ออก
                oW_WelcomeTxt.Dock = DockStyle.Fill;
                for (int nIndex = 0; nIndex < opnWelcome.Controls.Count; nIndex++)
                {
                    opnWelcome.Controls[nIndex].Dispose();
                }
                opnWelcome.Controls.Clear();
                opnWelcome.Controls.Add(oW_WelcomeTxt);

                olaCstPoint.Text = "";
                opnCstPoint.Visible = false;
                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        public void W_SETxCustomerOfDef(string ptCstName = "", string ptPoint = "")
        {
            try
            {
                if (!String.IsNullOrEmpty(ptCstName))
                {
                    //oW_WelcomeTxt = new uMarqueeTxt(new List<string> { ptCstName },
                    //                pnInterval: 0,
                    //                pnStep: 15,
                    //                poFont: new Font("Segoe UI Light", 20f, FontStyle.Bold));

                    //*Em 63-08-14
                    if (string.IsNullOrEmpty(cVB.tVB_MemCode))
                    {
                        oW_WelcomeTxt = new uMarqueeTxt(new List<string> { ptCstName },
                                    pnInterval: 0,
                                    pnStep: 15,
                                    poFont: new Font("Segoe UI Light", 20f, FontStyle.Bold));
                    }
                    else
                    {
                        oW_WelcomeTxt = new uMarqueeTxt(new List<string> { ptCstName },
                                    pnInterval: 0,
                                    pnStep: 15,
                                    poFont: new Font("Segoe UI Light", 20f, FontStyle.Bold),
                                    poForeColor: Color.Blue);
                    }
                    //+++++++++++++

                    oW_WelcomeTxt.Dock = DockStyle.Fill;
                    //olaCstName.Text = ptCstName;
                    olaCstPoint.Text = ptPoint;
                }
                else
                {
                    oW_WelcomeTxt = new uMarqueeTxt(new List<string> { cVB.tVB_CstDefName },
                                    pnInterval: 0,
                                    pnStep: 15,
                                    poFont: new Font("Segoe UI Light", 20f, FontStyle.Bold));
                    oW_WelcomeTxt.Dock = DockStyle.Fill;
                    //olaCstName.Text = cVB.tVB_CstDefName;
                    olaCstPoint.Text = "0";
                }

                for (int nIndex = 0; nIndex < opnWelcome.Controls.Count; nIndex++)
                {
                    opnWelcome.Controls[nIndex].Dispose();
                }
                opnWelcome.Controls.Clear();
                opnWelcome.Controls.Add(oW_WelcomeTxt);
                opnCstPoint.Visible = true;

                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_SETxSummaryAmt(string ptSumAmt)
        {
            try
            {
                if (!String.IsNullOrEmpty(ptSumAmt))
                    olaSummary.Text = ptSumAmt;
                else
                    olaSummary.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow);


                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_SETxLastPDT(string ptPDTName = "", string ptPDTAmt = "")
        {
            try
            {
                olaLastPdt.Text = ptPDTName;
                olaLastAmtPdt.Text = ptPDTAmt;

                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_ADDxPDTGrid(int pnSeq, string ptPDTName, string ptQty, string ptTotalAmt)
        {
            try
            {
                ogdPdtDT.Rows.Add();
                ogdPdtDT.SetData(ogdPdtDT.Rows.Count - 1, 0, pnSeq);
                ogdPdtDT.SetData(ogdPdtDT.Rows.Count - 1, 1, ptPDTName);
                ogdPdtDT.SetData(ogdPdtDT.Rows.Count - 1, 2, ptQty);
                ogdPdtDT.SetData(ogdPdtDT.Rows.Count - 1, 3, ptTotalAmt);

                ogdPdtDT.Select(ogdPdtDT.Rows.Count - 1, 0);
                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_SETxPDTGrid(int pnSeq, int pnCol, string ptEdit)
        {
            try
            {
                ogdPdtDT.SetData(pnSeq, pnCol, ptEdit);
                ogdPdtDT.Select(pnSeq, 0);

                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_SETxPDTGridSource(DataTable poSource)
        {
            try
            {
                ogdPdtDT.Clear();
                ogdPdtDT.Rows.Count = ogdPdtDT.Rows.Fixed;
                ogdPdtDT.AutoGenerateColumns = false;
                ogdPdtDT.DataSource = poSource.Copy();

                W_SETxColGrid(ogdPdtDT);

                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_ADDxBillGrid(string ptBillDTName, string ptTotalAmt)
        {
            try
            {
                ogdBillDT.Rows.Add();
                ogdBillDT.SetData(ogdBillDT.Rows.Count - 1, 0, ogdBillDT.Rows.Count - 1);
                ogdBillDT.SetData(ogdBillDT.Rows.Count - 1, 1, ptBillDTName);
                ogdBillDT.SetData(ogdBillDT.Rows.Count - 1, 2, ptTotalAmt);

                ogdBillDT.Select(ogdBillDT.Rows.Count - 1, 0);
                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_SETxBillGrid(int pnSeq, int pnCol, string ptEdit)
        {
            try
            {
                ogdBillDT.SetData(pnSeq, pnCol, ptEdit);
                ogdPdtDT.Select(pnSeq, 0);

                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_CLExClearBillGrid()
        {
            try
            {
                ogdBillDT.DataSource = null;
                ogdBillDT.Clear();
                ogdBillDT.Rows.Count = ogdBillDT.Rows.Fixed;

                W_SETxColGrid(ogdBillDT);

                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void W_SETxShowQRPromptPay(string ptDescript, string ptCountDown, Image poQR, Image poHD, bool pbIsSetCD)
        {
            try
            {
                if (pbIsSetCD)
                {
                    if (oW_QRPP != null)
                    {
                        oW_QRPP.W_SETxDescript(ptDescript, ptCountDown);
                    }
                }
                else
                {
                    if (oW_QRPP != null)
                    {
                        this.Controls.Remove(oW_QRPP);
                        oW_QRPP.Dispose();
                    }
                    oW_QRPP = null;
                    oW_QRPP = new uShowQR(18f, ptDescript, poQR, poHD, ptCountDown);

                    this.Controls.Add(oW_QRPP);
                    oW_QRPP.Size = new Size(ogdPdtDT.Size.Width, opnBodyRight.Size.Height);
                    oW_QRPP.Location = W_CALoPoint2Form(ogdPdtDT);
                    oW_QRPP.W_SETxTableSize();
                    oW_QRPP.BringToFront();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        public void W_SETxHideQRPromptPay()
        {
            try
            {
                if (oW_QRPP != null)
                {
                    this.Controls.Remove(oW_QRPP);
                    oW_QRPP.Dispose();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private Point W_CALoPoint2Form(System.Windows.Forms.Control oObj)
        {
            Point oPosition;
            try
            {
                oPosition = oObj.PointToScreen(Point.Empty);

                return new Point(oPosition.X - this.Location.X, oPosition.Y - this.Location.Y);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstScreen", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return new Point();
        }
    }
}
