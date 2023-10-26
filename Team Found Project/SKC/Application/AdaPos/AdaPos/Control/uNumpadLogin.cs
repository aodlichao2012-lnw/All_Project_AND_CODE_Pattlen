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
    public partial class uNumpadLogin : UserControl
    {
        #region Variable

        public double cU_NetPrice;
        public TextBox oU_TextValue;
        public MaskedTextBox oU_TextDateValue;
        public string tU_TextValue;
        public int nU_MaxLength;

        #endregion End Variable

        #region Constructor
        public uNumpadLogin()
        {
            InitializeComponent();
        }
        #endregion End Constructor

        #region Event
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm1_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm2_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm3_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm4_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm5_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm6_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm7_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm8_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm9_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocm0_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad Del
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (oU_TextValue.Text.Length > 0)
                    tU_TextValue = oU_TextValue.Text.Substring(0, oU_TextValue.Text.Length - 1);

                W_SETxValueToObject();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocmDel_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmC_Click(object sender, EventArgs e)
        {
            try
            {
                tU_TextValue = string.Empty;
                W_SETxValueToObject();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "ocmC_Click " + oEx.Message); }
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

                if (oU_TextValue.MaxLength > 0)
                {
                    if (oU_TextValue.Text.Length >= oU_TextValue.MaxLength)
                        return;
                }
                oU_TextValue.Text += pnNum;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "W_SHWxValue " + oEx.Message); }
        }

        /// <summary>
        /// Set value to object
        /// </summary>
        private void W_SETxValueToObject()
        {
            try
            {
                if (oU_TextValue == null)
                {
                    oU_TextDateValue.Text = tU_TextValue;
                    oU_TextDateValue.Focus();
                }
                else
                {
                    oU_TextValue.Text = tU_TextValue;
                    //oU_TextValue.Focus();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpadLogin", "W_SETxValueToObject " + oEx.Message); }
        }
        #endregion
    }
}
