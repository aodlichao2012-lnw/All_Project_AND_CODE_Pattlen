using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdaPos.Class;

namespace AdaPos.Control
{
    public partial class uPasscode : UserControl
    {
        public TextBox oU_TextValue;

        public uPasscode()
        {
            InitializeComponent();

            try
            {
                U_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "uPasscode " + oEx.Message); }
        }

        /// <summary>
        /// Set text user control
        /// </summary>
        private void U_SETxText()
        {
            try
            {
                ocmDel.Text = cVB.oVB_GBResource.GetString("tDel");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "U_SETxText " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm0_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(0);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm0_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm1_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(1);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm1_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm2_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(2);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm2_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm3_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(3);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm3_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm4_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(4);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm4_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm5_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(5);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm5_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm6_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(6);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm6_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm7_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(7);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm7_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 8
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm8_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(8);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm8_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad 9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocm9_Click(object sender, EventArgs e)
        {
            try
            {
                U_SHWxValue(9);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocm9_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Del
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (oU_TextValue.Text.Length > 0)
                    oU_TextValue.Text = oU_TextValue.Text.Substring(0, oU_TextValue.Text.Length - 1);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPasscode", "ocmDel_Click " + oEx.Message); }
        }

        /// <summary>
        /// Show Value ที่กด Numpad
        /// </summary>
        /// <param name="ptValue"></param>
        private void U_SHWxValue(int pnNum)
        {
            double cValue;

            try
            {
                if (string.IsNullOrEmpty(oU_TextValue.Text))
                    cValue = Convert.ToDouble(pnNum.ToString());
                else
                    cValue = Convert.ToDouble(oU_TextValue.Text + pnNum.ToString());

                if (oU_TextValue.Text.Length >= oU_TextValue.MaxLength)
                    return;

                oU_TextValue.Text += pnNum;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "W_SHWxValue " + oEx.Message); }
        }
    }
}
