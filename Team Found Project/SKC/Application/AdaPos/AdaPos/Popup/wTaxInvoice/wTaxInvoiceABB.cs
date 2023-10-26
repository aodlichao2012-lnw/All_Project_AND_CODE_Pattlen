using AdaPos.Class;
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
    public partial class wTaxInvoiceABB : Form
    {
        public wTaxInvoiceABB()
        {
            InitializeComponent();

            try
            {
                W_SETXDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceABB", "wTaxInvoiceABB : " + oEx.Message); }
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
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceABB", "ocmBack_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceABB", "OnPaintBackground : " + oEx.Message); }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETXDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                //ogdSelectABB.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdSelectABB); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceABB", "W_SETXDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                otbTitleAmt.HeaderText = cVB.oVB_GBResource.GetString("tAmount");
                otbTitleDatetime.HeaderText = cVB.oVB_GBResource.GetString("tDatetime");
                otbTitleDocNo.HeaderText = cVB.oVB_GBResource.GetString("tDocNo");
                otbTitlePos.HeaderText = cVB.oVB_GBResource.GetString("tPos");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceABB", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Open form Tax Invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                new wTaxInvoice(1).Show();
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceABB", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Closing Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wTaxInvoiceABB_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceABB", "wTaxInvoiceABB_FormClosing : " + oEx.Message); }
        }
    }
}
