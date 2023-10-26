using AdaPos.Class;
using AdaPos.Forms;
using AdaPos.Popup.wTaxInvoice;
using AdaPos.Resources_String.Local;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wTaxInvoiceMode : Form
    {
        #region Variable

        private ResourceManager oW_Resource;
        private int nW_Mode;
        #endregion End Variable

        public wTaxInvoiceMode()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();

                switch (cVB.tVB_PosType)
                {
                    case "1":
                        //ocmDoc_Click(ocmDoc, null); *Net 62-12-26
                        olaDocument_Click(olaDocument, null);
                        break;
                    case "2":
                        //ocmWristband_Click(ocmWristband, null);   *Net 62-12-26
                        olaWristband_Click(olaWristband,null); 
                        break;
                    default:
                        //ocmDoc_Click(ocmDoc, null); *Net 62-12-26
                        olaDocument_Click(olaDocument, null);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceMode", "wTaxInvoiceMode : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceMode", "ocmBack_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceMode", "OnPaintBackground " + oEx.Message); }
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
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                //ocmWristband.BackColor = cVB.oVB_ColDark; *Net 62-12-26
                ocmSchDoc.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceMode", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
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

                //cVB.tVB_KbdScreen = "TAXINVOICE";

                olaTitleTax.Text = oW_Resource.GetString("tTax");

                //ocmWristband.Text = oW_Resource.GetString("tWristband"); *Net 62-12-26
                olaWristband.Text = oW_Resource.GetString("tWristband"); //*Net 62-12-26 ocm->ola
                //ocmDoc.Text = cVB.oVB_GBResource.GetString("tDoc");
                olaDocument.Text = cVB.oVB_GBResource.GetString("tDoc"); //*Net 62-12-26 ocm->ola

                olaTitleDocNo.Text = cVB.oVB_GBResource.GetString("tDocNo");
                olaTitleWBNo.Text = oW_Resource.GetString("tWBNo");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceMode", "W_SETxText : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wTaxInvoiceMode", "ocmKB_Click : " + ex.Message); }
        }

        /// <summary>
        /// เข้าสู่หน้า Tax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            wTaxInvoiceABB oABB;

            try
            {
                //if (ocmWristband.BackColor == cVB.oVB_ColDark)  // โหมด Wristband *Net 62-12-26
                if(olaWristband.ForeColor == Color.Black) //*Net 62-12-26 โหมด Wristband check forecolor
                {
                    oABB = new wTaxInvoiceABB();
                    oABB.ShowDialog();

                    if (oABB.DialogResult == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else    // โหมด Document
                {
                    if (string.IsNullOrEmpty(otbDocument.Text))
                    {
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputDoc"), 3);
                        return;
                    }
                    this.DialogResult = DialogResult.OK;
                    new wTaxInvoice(nW_Mode).Show();
                    this.Close();
                }
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wTaxInvoiceMode", "ocmAccept_Click : " + ex.Message); }
        }

        /// <summary>
        /// Search Document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSchDoc_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                cVB.tVB_DocNo = "";
                new wTaxInvoiceSearch(1).ShowDialog();
                otbDocument.Text = cVB.tVB_DocNo;
                this.Visible = true;
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wTaxInvoiceMode", "ocmSchDoc_Click : " + ex.Message); }
        }

        /// <summary>
        /// Clear Value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wTaxInvoiceMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                oW_Resource = null;
                this.Dispose();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wTaxInvoiceMode", "wTaxInvoiceMode_FormClosing : " + ex.Message); }
        }

        /// <summary>
        /// Mode Wristband
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void olaWristband_Click(object sender, EventArgs e)
        {
            try
            {
                olaWristband.Cursor = Cursors.No;
                olaWristband.ForeColor = Color.Black;
                olaDocument.Cursor = Cursors.Default;
                olaDocument.ForeColor = Color.Gray;

                opnWristband.Visible = true;
                opnDocument.Visible = false;
                nW_Mode = 1;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceMode", "olaWristband_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Mode Document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void olaDocument_Click(object sender, EventArgs e)
        {
            try
            {
                olaDocument.Cursor = Cursors.No;
                olaDocument.ForeColor = Color.Black;
                olaWristband.Cursor = Cursors.Default;
                olaWristband.ForeColor = Color.Gray;

                opnDocument.Visible = true;
                opnWristband.Visible = false;
                nW_Mode = 2;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceMode", "olaDocument_Click : " + oEx.Message); }
        }
    }
}
