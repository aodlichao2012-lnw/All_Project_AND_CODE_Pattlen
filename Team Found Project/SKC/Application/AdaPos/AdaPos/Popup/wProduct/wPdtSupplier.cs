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

namespace AdaPos.Popup.wProduct
{
    public partial class wPdtSupplier : Form
    {
        private ResourceManager oW_Resource;

        public wPdtSupplier()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtSupplier", "wPdtSupplier : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtSupplier", "OnPaintBackground : " + oEx.Message); }
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
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmSchSupplier.BackColor = cVB.oVB_ColNormal;
                ocmSchUser.BackColor = cVB.oVB_ColNormal;
                ocmDelete.BackColor = cVB.oVB_ColNormal;
                ocmAcceptSpl.BackColor = cVB.oVB_ColNormal;
                //ogdSupplier.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdSupplier); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtSupplier", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resProduct_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resProduct_EN));
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtSupplier", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Close Popup
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtSupplier", "ocmBack_Click : " + oEx.Message); }
        }
    }
}
