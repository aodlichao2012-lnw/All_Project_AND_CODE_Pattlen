using AdaPos.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdaPos.Forms;
using System.Drawing.Printing;

namespace AdaPos.Popup.wTaxInvoice
{
    public partial class wTaxInvoicePreview : Form
    {
        public wTaxInvoicePreview()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
                W_SHWxPreview();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "wTaxInvoicePreview : " + oEx.Message); }
        }

        #region Function
        private void W_SHWxPreview()
        {
            try
            {
                opnContent.Refresh();
                opnContent.AutoScroll = true;
                opnContent.VerticalScroll.Visible = true;
                opnContent.Paint += opnContent_Paint;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "W_SHWxPreview : " + oEx.Message); }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmPrint.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                olaTitlePreview.Text = cVB.oVB_GBResource.GetString("tPreview");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "W_SETxText : " + oEx.Message); }
        }
        #endregion Function

        #region Method / Events
        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Paint Background
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                using (SolidBrush oBrush = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(oBrush, e.ClipRectangle);
                    oBrush.Dispose();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "OnPaintBackground : " + oEx.Message); }
        }

        private void opnContent_Paint(object sender, PaintEventArgs e)
        {
            
            try
            {
                cVB.nVB_StartY = 0; //*Arm 63-05-03
                cVB.bVB_PrnTaxInvoiceCopy = false;
                cVB.oVB_TaxInvoice.W_PRNxDrawTax(e.Graphics);
                
                //*Arm 63-05-03
                cVB.nVB_StartY += 100;

                if (cVB.nVB_StartY > 700 && cVB.nVB_StartY < 830)
                {
                    opnContent.Height = 1000 + (830 - cVB.nVB_StartY);
                }
                else if (cVB.nVB_StartY > 830)
                {
                    opnContent.Height = cVB.nVB_StartY;
                }
                
                //+++++++++++++++
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "opnContent_Paint : " + oEx.Message); }
        }

        private void ocmPrint_Click(object sender, EventArgs e)
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            try
            {
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();

                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                oDoc.PrintController = new StandardPrintController();
                oDoc.PrintPage += oDoc_PrintPage;

                //*Net 63-02-25 พิมพ์จำนวนต้นฉบับ/สำเนา ตาม Option
                cVB.bVB_PrnTaxInvoiceCopy = false;
                for (int nOri = 0; nOri < cVB.nVB_PrnTaxMaster; nOri++)
                {
                    oDoc.Print();
                }
                cVB.bVB_PrnTaxInvoiceCopy = true;
                for (int nCopy = 0; nCopy < cVB.nVB_PrnTaxCopy; nCopy++)
                {
                    oDoc.Print();
                }
                //+++++++++++++++++++++++++++++++++++++++++
                cVB.oVB_TaxInvoice.W_PRCxPrintUpd();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "ocmPrint_Click : " + oEx.Message); }
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
                 cVB.oVB_TaxInvoice.W_PRNxDrawTax(e.Graphics);
                //if (e.Graphics.VisibleClipBounds.Height > opnContent.Height)
                //{
                //    ScrollBar oScrBar = new VScrollBar();
                //    oScrBar.Dock = DockStyle.Right;
                //    oScrBar.Scroll += (sender, e) => { opnContent.VerticalScroll.Value = oScrBar.Value; };
                //    opnContent.Controls.Add(oScrBar);
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoicePreview", "oDoc_PrintPage : " + oEx.Message); }
        }


        #endregion Method / Events

        private void opnFlowLayoutPanel_Scroll(object sender, ScrollEventArgs e)
        {
            
        }
    }
}
