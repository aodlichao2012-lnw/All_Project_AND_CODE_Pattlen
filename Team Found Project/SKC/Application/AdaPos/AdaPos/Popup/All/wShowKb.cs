using AdaPos.Class;
using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AdaPos.Popup.All
{
    public partial class wShowKb : Form
    {
        #region Constructor

        public wShowKb()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
                W_GETxFuncKb();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "wShowKb " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

        /// <summary>
        /// Open function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmOk_Click(object sender, EventArgs e)
        {
            try
            {
                W_SHWxFuncKb();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "ocmOk_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "ocmBack_Click " + oEx.Message); }
        }

        /// <summary>
        /// Open function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdFuncKb_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                W_SHWxFuncKb();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "ogdFuncKb_CellDoubleClick " + oEx.Message); }
        }

        /// <summary>
        /// Function ตาม Keycode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdFuncKb_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        e.Handled = true;
                        W_SHWxFuncKb();
                        break;

                    case Keys.Up:
                        if (ogdFuncKb.SelectedRows[0].Index > 0)
                            ogdFuncKb.Rows[ogdFuncKb.SelectedRows[0].Index - 1].Selected = true;
                        break;

                    case Keys.Down:
                        if (ogdFuncKb.Rows.Count - 1 > ogdFuncKb.SelectedRows[0].Index)
                            ogdFuncKb.Rows[ogdFuncKb.SelectedRows[0].Index + 1].Selected = true;
                        break;

                    case Keys.Escape:
                        this.Close();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "ogdFuncKb_KeyDown " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "OnPaintBackground " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wShowKb_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "wShowKb_FormClosing " + oEx.Message); }
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
                ocmOk.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdFuncKb); //*Net 63-03-02 Set Style gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "W_SETxDesign " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                olaTitleFuncKb.Text = cVB.oVB_GBResource.GetString("tShowKb");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "W_SETxText " + oEx.Message); }
        }

        /// <summary>
        /// Get function keyboard
        /// </summary>
        private void W_GETxFuncKb()
        {
            List<cmlTPSMFunc> aoKb;

            try
            {
                aoKb = new cFunctionKeyboard().C_GETaFuncKb();

                for (int nCount = 0; nCount < aoKb.Count; nCount++)
                    ogdFuncKb.Rows.Add((nCount + 1), aoKb[nCount].FTSysKeyFunc, aoKb[nCount].FTGdtName, aoKb[nCount].FTSysCode, aoKb[nCount].FTGdtCallByName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "W_GETxFuncKb " + oEx.Message); }
            finally
            {
                aoKb = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Show Function keyboard after select
        /// </summary>
        private void W_SHWxFuncKb()
        {
            try
            {
                cVB.tVB_KbdCallByName = ogdFuncKb.CurrentRow.Cells[4].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();

                //Arm 63-03-04 ถ้าเป็น Sale ให้ไปใช้ W_GETxFuncByFuncName ที่ wSale
                if (cVB.tVB_KbdScreen == "SALE" || cVB.tVB_KbdScreen == "SALESTD")
                {
                    cVB.oVB_Sale.W_GETxFuncByFuncName(cVB.tVB_KbdCallByName);
                }
                else
                {
                    new cFunctionKeyboard().C_PRCxCallByName(cVB.tVB_KbdCallByName);
                }
                //+++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShowKb", "W_SHWxFuncKb " + oEx.Message); }
        }

        #endregion End Function
    }
}
