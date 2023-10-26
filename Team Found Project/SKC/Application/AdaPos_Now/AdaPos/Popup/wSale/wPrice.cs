using AdaPos.Class;
using AdaPos.Forms;
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

namespace AdaPos.Popup.wSale
{
    public partial class wPrice : Form
    {
        private ResourceManager oW_Resource;
        public string tW_Old;
        public decimal cW_Net;
        public string tW_New;

        public wPrice()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "wPrice " + oEx.Message); }
        }

        /// <summary>
        /// Design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                //Ping 2019.10.08
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmOk.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "W_SETxDesign " + oEx.Message); }
        }

        /// <summary>
        /// Text form
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

                ocmOk.Text = "".PadLeft(10) + "OK";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "W_SETxText " + oEx.Message); }
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
                otbNewPrice.Text = "";
                W_SETxNewPrice();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "ocmBack_Click " + oEx.Message); }
        }

        /// <summary>
        /// Show data after open Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wPrice_Shown(object sender, EventArgs e)
        {
            try
            {
                W_SETxPriceValue();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "wPrice_Shown " + oEx.Message); }
        }

        /// <summary>
        /// Clear Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wPrice_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "wPrice_FormClosing " + oEx.Message); }
        }

        /// <summary>
        /// เมื่อมีการเปลี่ยนแปลง Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbNewPrice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(otbNewPrice.Text))
                {
                    //switch (cVB.tVB_KbdCode)
                    //{
                    //    case "KB044":
                    //        otbNewPrice.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDouble(otbNewPrice.Text), 0);
                    //        break;
                    //}
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "otbNewPrice_TextChanged " + oEx.Message); }
        }

        /// <summary>
        /// ปิดการกรอกข้อมูลลง Textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbNewPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wPrice", "otbNewPrice_KeyPress " + ex.Message); }
        }

        /// <summary>
        /// Change data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmOk_Click(object sender, EventArgs e)
        {
            try
            {
                W_SETxNewPrice();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "ocmOk_Click " + oEx.Message); }
        }

        /// <summary>
        /// Set new price value
        /// </summary>
        private void W_SETxNewPrice()
        {
            try
            {
                if (string.IsNullOrEmpty(otbNewPrice.Text))
                    otbNewPrice.Text = otbOldPrice.Text;

                //switch (cVB.tVB_KbdCode)
                //{
                //    case "KB044":
                //        tW_New = new cSP().SP_SETtDecShwSve(1, Convert.ToDouble(otbNewPrice.Text), 0);
                //        break;
                //}

                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "W_SETxNewPrice " + oEx.Message); }
        }

        /// <summary>
        /// Set price value for this form
        /// </summary>
        private void W_SETxPriceValue()
        {
            try
            {
                otbNetPrice.Text = new cSP().SP_SETtDecShwSve(1, cW_Net, 0);

                //switch (cVB.tVB_KbdCode)
                //{
                //    case "KB044":   // Qty
                //        olaTitlePrice.Text = oW_Resource.GetString("tChgQty");
                //        olaPriceOld.Text = oW_Resource.GetString("tOldQty");
                //        olaPriceNew.Text = oW_Resource.GetString("tNewQty");
                //        onpKB.ocmDot.Enabled = false;
                //        onpKB.oU_TextValue = otbNewPrice;
                //        opnWeight.Visible = false;
                //        otbOldPrice.Text = tW_Old;
                //        break;
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "W_SETxPriceValue " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPrice", "OnPaintBackground " + oEx.Message); }
        }
    }
}
