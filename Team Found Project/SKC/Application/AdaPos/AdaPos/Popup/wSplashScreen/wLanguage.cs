using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace AdaPos.Popup.wLogin
{
    public partial class wLanguage : Form
    {
        #region Variable

        private ResourceManager oW_Resource;

        #endregion End Variable

        #region Constructor

        public wLanguage()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
                W_GETxLanguage();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "wLanguage " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "OnPaintBackground " + oEx.Message); }
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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "ocmBack_Click " + oEx.Message); }
        }

        /// <summary>
        /// Change language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmOK_Click(object sender, EventArgs e)
        {
            try
            {
                W_CHGxLanguage();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "ocmOK_Click " + oEx.Message); }
        }

        /// <summary>
        /// Change Language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdLanguage_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                W_CHGxLanguage();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "ogdLanguage_CellDoubleClick " + oEx.Message); }
        }

        /// <summary>
        /// Function ตาม Keycode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdLanguage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        e.Handled = true;
                        W_CHGxLanguage();
                        break;

                    case Keys.Up:
                        if (ogdLanguage.SelectedRows[0].Index > 0)
                            ogdLanguage.Rows[ogdLanguage.SelectedRows[0].Index - 1].Selected = true;
                        break;

                    case Keys.Down:
                        if (ogdLanguage.Rows.Count - 1 > ogdLanguage.SelectedRows[0].Index)
                            ogdLanguage.Rows[ogdLanguage.SelectedRows[0].Index + 1].Selected = true;
                        break;

                    case Keys.Escape:
                        this.Close();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "ogdLanguage_KeyDown " + oEx.Message); }
        }

        /// <summary>
        /// Clear variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wLanguage_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                oW_Resource = null;
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "wLanguage_FormClosing " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wLanguage_Shown(object sender, EventArgs e)
        {
            int nIndex;

            try
            {
                // Focus current Language
                ogdLanguage.ClearSelection();
                nIndex = (ogdLanguage.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToInt32(r.Cells[0].Value) == cVB.nVB_Language).Select(r => r.Index)).FirstOrDefault();
                ogdLanguage.Rows[nIndex].Selected = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "wLanguage_Shown " + oEx.Message); }
        }

        #endregion End Event

        #region Function

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmOK.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "W_SETxDesign " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                olaTitleLanguage.Text = oW_Resource.GetString("tLanguage");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "W_SETxText " + oEx.Message); }
        }

        /// <summary>
        /// Change Language
        /// </summary>
        private void W_CHGxLanguage()
        {
            try
            {
                cVB.nVB_Language = Convert.ToInt32(ogdLanguage.Rows[ogdLanguage.SelectedRows[0].Index].Cells[0].Value);

                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "W_CHGxLanguage " + oEx.Message); }
        }

        /// <summary>
        /// Get language to gridview
        /// </summary>
        private void W_GETxLanguage()
        {
            List<cmlTSysLanguage> aoLanguage;

            try
            {
                aoLanguage = new cLanguage().C_GETaLanguage();

                foreach (cmlTSysLanguage oLng in aoLanguage)
                    ogdLanguage.Rows.Add(oLng.FNLngID, oLng.FTLngShortName, oLng.FTLngNameEng);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wLanguage", "W_GETxLanguage " + oEx.Message); }
            finally
            {
                aoLanguage = null;
                new cSP().SP_CLExMemory();
            }
        }

        #endregion End Function
    }
}
