using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wSale
{
    public partial class wRentalPdt : Form
    {
        private cSP oW_SP;
        private int nW_Time;

        public List<cmlPdtRental> oaW_PdtRentalOrder;

        public wRentalPdt()
        {
            InitializeComponent();

            oW_SP = new cSP();
            oaW_PdtRentalOrder = new List<cmlPdtRental>();
            oW_SP.SP_PRCxFlickering(this.Handle);

            W_SETxDesign();
            W_SETxText();
            W_GETxPdt();
            W_GETxMenuPdt();
            W_SHWxButtonBar();
            W_CHKxReturnFromPayment();
        }

        private void opnMenuHD_Paint(object sender, PaintEventArgs e)
        {

        }

        private void W_SETxDesign()
        {

        }

        private void W_SETxText()
        {

        }


        private void ocmMenuBill_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void W_GETxPdt()
        {
            StringBuilder oSQL = new StringBuilder();
            try
            {
                oSQL.Append("SELECT PdtRT.FTPdtCode"); //--รหัสสินค้า
                oSQL.Append(",PdtBar.FTBarCode"); //--บาร์โค้ดสินค้า
                oSQL.Append(",PdtL.FTPdtName"); //--ชื่อสินค้า
                oSQL.Append(",PdtImg.FTImgObj"); //--รูปภาพสินค้า
                oSQL.Append(",PdtRT.FCPdtDeposit"); //--ค่ามัดจำ
                oSQL.Append(",PdtRT.FTPdtStaPay"); //-- สถานะการชำระ 1:จ่ายขั้นต่ำตอนเช่า / Pre - Paid 2: จ่ายตอนคืน / PostPaid
                oSQL.Append(",Pdt.FTPdtSetOrSN"); //--Serial Number ถ้า FTPdtSetOrSN = 3 เมื่อกดเลือกสินค้าจะต้องแสดง pop-up ให้กรอก Serail number
                oSQL.Append(",Price.FTPghDocNo"); //--เลขที่เอกสารใบปรับราคา
                oSQL.Append(",Price.FTRthCode"); //--รหัสราคาค่าเช่า
                oSQL.Append(",Price.FTRthCalType"); //--1:ปัดขึ้น(Df); 2ปัดลง; 3:เฉลี่ย(ใช้ราคาจาก Rate Seq ก่อนหน้า)
                oSQL.Append(",Price.FNRtdSeqNo"); //-- ลำดับอัตราค่าเช่า
                oSQL.Append(",Price.FNRtdMinQty"); //--เวลาตามอัตรา
                oSQL.Append(",Price.FTRtdTmeType"); //--หน่วย 1:นาที 2:ชั่วโมง 3:วัน 4:เดือน 5: ปี
                oSQL.Append(",Price.FCRtdPrice"); //-- ราคา
                oSQL.Append(" FROM TRTMPdtRental AS PdtRT WITH(NOLOCK)"); //
                oSQL.Append(" INNER JOIN TCNMPdt As Pdt WITH(NOLOCK) ON PdtRT.FTPdtCode = Pdt.FTPdtCode"); //
                oSQL.Append(" INNER JOIN TCNMPdt_L As PdtL WITH(NOLOCK) ON PdtRT.FTPdtCode = PdtL.FTPdtCode"); //
                oSQL.Append(" INNER JOIN TCNMPdtBar As PdtBar WITH(NOLOCK) ON PdtRT.FTPdtCode = PdtBar.FTPdtCode"); //
                oSQL.Append(" LEFT JOIN TCNMImgPdt AS PdtImg WITH(NOLOCK) ON PdtRT.FTPdtCode = PdtImg.FTImgRefID"); //
                oSQL.Append(" LEFT JOIN "); //
                oSQL.Append(" (SELECT A.FTPdtCode"); // --รหัสสินค้า
                oSQL.Append(",A.FTPghDocNo"); //--เลขที่เอกสารใบปรับราคา
                oSQL.Append(",A.FTRthCode"); //--รหัสราคาค่าเช่า
                oSQL.Append(",HD.FTRthCalType"); //--1:ปัดขึ้น(Df); 2ปัดลง; 3:เฉลี่ย(ใช้ราคาจาก Rate Seq ก่อนหน้า)
                oSQL.Append(",DT.FNRtdSeqNo"); //-- ลำดับอัตราค่าเช่า
                oSQL.Append(",DT.FNRtdMinQty"); //--เวลาตามอัตรา
                oSQL.Append(",DT.FTRtdTmeType"); //--หน่วย 1:นาที 2:ชั่วโมง 3:วัน 4:เดือน 5: ปี
                oSQL.Append(",DT.FCRtdPrice"); //-- ราคา
                oSQL.Append(" FROM(SELECT MAX(FTPghDocNo) AS FTPghDocNo, FTPdtCode, FTRthCode");
                oSQL.Append(" FROM TRTTPdtPrice4PDT");
                oSQL.Append(" WHERE ");
                oSQL.Append(" getdate() BETWEEN CONCAT(CONVERT(varchar(10), FDPghDStart, 121), ' ', FTPghTStart) AND  CONCAT(CONVERT(varchar(10), FDPghDStop, 121), ' ', FTPghTStart)");
                oSQL.Append(" AND FTPghDocType = '1'");
                oSQL.Append(" GROUP BY FTPdtCode, FTRthCode) AS A");
                oSQL.Append(" INNER JOIN TRTMPriRateHD HD ON A.FTRthCode = HD.FTRthCode");
                oSQL.Append(" INNER JOIN TRTMPriRateDT DT ON A.FTRthCode = DT.FTRthCode) AS Price ON PdtRT.FTPdtCode = Price.FTPdtCode");
                oSQL.Append(" WHERE PdtL.FNLngID = 1");
                oSQL.Append(" AND Pdt.FTPdtForSystem = '4'");
                oSQL.Append(" AND Pdt.FTPdtStaActive = '1'");

                List<cmlPdtRental> aoPdt = new cDatabase().C_GETaDataQuery<cmlPdtRental>(oSQL.ToString());
                opnPdt.Controls.Clear();
                if (aoPdt != null)
                {
                    var aoPdt_Distinct = aoPdt.Select(x => x.FTPdtCode).Distinct().ToList();
                    foreach (var oPdtItem in aoPdt_Distinct)
                    {
                        uPdtRental2 oPdtRental = new uPdtRental2();
                        oPdtRental.aoW_Pdt = aoPdt.Where(x => x.FTPdtCode == oPdtItem).ToList();
                        oPdtRental.Event_uRentalPdt_Mouse_Click += new MouseEventHandler(W_RentalPdt_Mouse_Click);
                        opnPdt.Controls.Add(oPdtRental);
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdt", "W_GETxPdt : " + oEx.Message);
            }
        }

        private void W_RentalPdt_Mouse_Click(object sender, MouseEventArgs e)
        {
            try
            {
                uPdtRental2 oRentalPdt = sender as uPdtRental2;
              
                if (e.Button == MouseButtons.Left)
                {
                    cmlPdtRentalOrder oOrder = new cmlPdtRentalOrder();
                    oOrder.aoPdtRental = oRentalPdt.aoW_Pdt;
                    oaW_PdtRentalOrder.Add(oRentalPdt.aoW_Pdt.FirstOrDefault());
                    olaOrderCount.Text = oaW_PdtRentalOrder.Count.ToString();
                }

                if (e.Button == MouseButtons.Right)
                {
                    new wRentalPdtDsct().ShowDialog();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdt", "W_RentalPdt_Mouse_Click : " + oEx.Message);
            }
        }

        private void OPdtRental_Event_uRentalPdt_Mouse_Click(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void W_GETxMenuPdt()
        {

        }

        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT; 
            List<cmlTPSMFunc> aoMenuB;  
            int nItem;  
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        private void ocmMenuBar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void W_CHKxReturnFromPayment()
        {

        }

        private void ocmRemark_Click(object sender, EventArgs e)
        {
            //new wRemark().ShowDialog(); //*Arm 63-09-13 comment code
        }
        
        private void otmClose_Tick(object sender, EventArgs e)
        {
            try
            {
                if (nW_Time == 5)
                    this.Close();

                nW_Time++;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wTicket", "otmClose_Tick : " + oEx.Message);
            }
        }

        private void wRentalPdt_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                //oW_SP.SP_CLExMemory();
                oW_SP = null;
                oaW_PdtRentalOrder = null;
                this.Dispose();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdt", "wRentalPdt_FormClosing : " + oEx.Message);
            }
        }

        private void ocmPayment_Click(object sender, EventArgs e)
        {
            try
            {
                otmClose.Start();
                wRentalPdtBudgetRevise oFrm = new wRentalPdtBudgetRevise();
                oFrm.oaW_PdtRentalOrder = oaW_PdtRentalOrder;
                oFrm.Show();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdt", "ocmPayment_Click : " + oEx.Message);
            }
        }
    }
}
