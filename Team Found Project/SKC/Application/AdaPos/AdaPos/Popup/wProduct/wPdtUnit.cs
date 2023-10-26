using AdaPos.Class;
using AdaPos.Models.Other;
using AdaPos.Popup.All;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wProduct
{
    public partial class wPdtUnit : Form
    {
        public wPdtUnit()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "wPdtPrice : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Accept
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Search Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSchUnit_Click(object sender, EventArgs e)
        {
            cmlSearch oSearch;
            wSearch2Column oSch2Col;

            try
            {
                oSch2Col = new wSearch2Column("TCNMPdtUnit");
                oSch2Col.ShowDialog();

                if (oSch2Col.DialogResult == DialogResult.OK)
                {
                    oSearch = oSch2Col.oW_DataSearch;
                    otbUnit.Text = oSearch.tName;
                    otbUnit.Tag = oSearch.tCode;
                    otbFactor.Focus();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "ocmSchUnit_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "OnPaintBackground : " + oEx.Message); }
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
                ocmSchUnit.BackColor = cVB.oVB_ColNormal;
                ocmSchColor.BackColor = cVB.oVB_ColNormal;
                ocmSchSize.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Search color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSchColor_Click(object sender, EventArgs e)
        {
            cmlSearch oSearch;
            wSearch2Column oSch2Col;

            try
            {
                oSch2Col = new wSearch2Column("TCNMPdtColor");
                oSch2Col.ShowDialog();

                if (oSch2Col.DialogResult == DialogResult.OK)
                {
                    oSearch = oSch2Col.oW_DataSearch;
                    otbColor.Text = oSearch.tName;
                    otbColor.Tag = oSearch.tCode;
                    ocmSchSize.Focus();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "ocmSchColor_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wPdtUnit_Shown(object sender, EventArgs e)
        {
            try
            {
                ocmSchUnit.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPdtPrice", "wPdtUnit_Shown : " + oEx.Message); }
        }
    }
}
