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
    public partial class uPincode : UserControl
    {
        // *Arm 62-10-01 แก้ไขฟังก์ชั่นใหม่ทั้งหมด
        #region Variable

        public double cU_NetPrice;
        public TextBox oU_TextValue;
        public MaskedTextBox oU_TextDateValue;
        public string tU_TextValue;

        #endregion End Variable

        #region Constructor

        public uPincode()
        {
            InitializeComponent();

            try
            {
                U_SETxDesign();
                U_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "uNumpad " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm0_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm1_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm2_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm3_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm4_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm5_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm6_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm7_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm8_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocm9_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDot_Click(object sender, EventArgs e)
        {
            string[] atDot;

            try
            {
                atDot = tU_TextValue.Split('.');

                if (atDot.Count() < 2)
                    tU_TextValue += ".";

                W_SETxValueToObject();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocmDot_Click " + oEx.Message); }
        }

        /// <summary>
        /// ปุ่ม Numpad del
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (tU_TextValue.Length > 0)
                    tU_TextValue = tU_TextValue.Substring(0, tU_TextValue.Length - 1);

                W_SETxValueToObject();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "ocmDel_Click " + oEx.Message); }
        }

        #endregion End Variable

        #region Function

        /// <summary>
        /// Show Value ที่กด Numpad
        /// </summary>
        /// <param name="ptValue"></param>
        private void U_SHWxValue(int pnNum)
        {
            decimal cLimit = 0, cValue;
            string[] atDot;

            try
            {
                if (string.IsNullOrEmpty(tU_TextValue))
                    cValue = Convert.ToDecimal(pnNum.ToString());
                else
                    cValue = Convert.ToDecimal(tU_TextValue + pnNum.ToString());

                switch (cVB.tVB_KbdCallByName)
                {
                    case "KB044":
                        cLimit = 99999;
                        break;

                    case "MenuDisPer":
                        cLimit = 100;
                        break;

                    case "MenuDisAmt":
                        cLimit = Convert.ToDecimal(cU_NetPrice);
                        break;

                    default:
                        cLimit = 9999999999999999999;
                        break;
                }

                if (cValue > cLimit)
                    return;

                if (oU_TextValue != null)
                {
                    if (tU_TextValue.Length >= oU_TextValue.MaxLength)    // ลบทศนิยมและจุด
                        return;
                }
                else
                {
                    if (tU_TextValue.Length >= 8)
                        return;
                }

                atDot = tU_TextValue.Split('.');

                if (atDot.Count() > 1)
                {
                    if (atDot[1].Count() < cVB.nVB_DecShow)
                        tU_TextValue += pnNum;
                    else
                    {
                        if (Convert.ToDouble(tU_TextValue) == 0)
                            tU_TextValue = pnNum.ToString();
                    }
                }
                else
                    tU_TextValue += pnNum;

                W_SETxValueToObject();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "W_SHWxValue " + oEx.Message); }
            finally
            {
                atDot = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set design user control
        /// </summary>
        private void U_SETxDesign()
        {
            try
            {
                ocm0.BackColor = cVB.oVB_ColNormal;
                ocm1.BackColor = cVB.oVB_ColNormal;
                ocm2.BackColor = cVB.oVB_ColNormal;
                ocm3.BackColor = cVB.oVB_ColNormal;
                ocm4.BackColor = cVB.oVB_ColNormal;
                ocm5.BackColor = cVB.oVB_ColNormal;
                ocm6.BackColor = cVB.oVB_ColNormal;
                ocm7.BackColor = cVB.oVB_ColNormal;
                ocm8.BackColor = cVB.oVB_ColNormal;
                ocm9.BackColor = cVB.oVB_ColNormal;
                ocmDel.BackColor = cVB.oVB_ColNormal;
                ocmDot.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "U_SETxDesign " + oEx.Message); }
        }

        /// <summary>
        /// Set text user control
        /// </summary>
        private void U_SETxText()
        {
            try
            {
                tU_TextValue = "";
                ocmDel.Text = cVB.oVB_GBResource.GetString("tDel");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "U_SETxText " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "W_SETxValueToObject " + oEx.Message); }
        }

        #endregion End Function





        // โค้ดเก่าก่อนแก้ไข 62-10-01
        /*
        public TextBox oU_TextValue;

        public uPincode()
        {
            InitializeComponent();

            try
            {
                U_SETxDesign();
                U_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "uPincode " + oEx.Message); }
        }

        /// <summary>
        /// Set design user control
        /// </summary>
        private void U_SETxDesign()
        {
            try
            {
                ocm0.BackColor = cVB.oVB_ColNormal;
                ocm1.BackColor = cVB.oVB_ColNormal;
                ocm2.BackColor = cVB.oVB_ColNormal;
                ocm3.BackColor = cVB.oVB_ColNormal;
                ocm4.BackColor = cVB.oVB_ColNormal;
                ocm5.BackColor = cVB.oVB_ColNormal;
                ocm6.BackColor = cVB.oVB_ColNormal;
                ocm7.BackColor = cVB.oVB_ColNormal;
                ocm8.BackColor = cVB.oVB_ColNormal;
                ocm9.BackColor = cVB.oVB_ColNormal;
                ocmDot.BackColor = cVB.oVB_ColNormal;
                ocmDel.BackColor = cVB.oVB_ColNormal;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "U_SETxDesign " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "U_SETxText " + oEx.Message); }
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
                U_SHWxValue(0.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm0_Click " + oEx.Message); }
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
                U_SHWxValue(1.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm1_Click " + oEx.Message); }
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
                U_SHWxValue(2.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm2_Click " + oEx.Message); }
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
                U_SHWxValue(3.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm3_Click " + oEx.Message); }
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
                U_SHWxValue(4.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm4_Click " + oEx.Message); }
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
                U_SHWxValue(5.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm5_Click " + oEx.Message); }
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
                U_SHWxValue(6.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm6_Click " + oEx.Message); }
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
                U_SHWxValue(7.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm7_Click " + oEx.Message); }
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
                U_SHWxValue(8.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm8_Click " + oEx.Message); }
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
                U_SHWxValue(9.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocm9_Click " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uPincode", "ocmDel_Click " + oEx.Message); }
        }

        /// <summary>
        /// Show Value ที่กด Numpad
        /// </summary>
        /// <param name="ptValue"></param>
        private void U_SHWxValue(string pnNum)
        {
            //double cValue;

            try
            {
                //if (string.IsNullOrEmpty(oU_TextValue.Text))
                //    cValue = Convert.ToDouble(pnNum);
                //else
                //    cValue = Convert.ToDouble(oU_TextValue.Text + pnNum);
            
                if (oU_TextValue.Text.Length >= oU_TextValue.MaxLength)
                    return;

                oU_TextValue.Text += pnNum;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNumpad", "W_SHWxValue " + oEx.Message); }
        }

        private void ocmDot_Click(object sender, EventArgs e)
        {
            try
            {
                if (oU_TextValue.Text == string.Empty)
                {
                    return;
                }
                U_SHWxValue(".");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uNumpad", "ocmDot_Click " + oEx.Message);
            }
        }   */

    }
}
