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
using AdaPos.Popup.wSale;

namespace AdaPos.Control
{
    public partial class uQty : UserControl
    {
        public TextBox oU_Value = new TextBox();
        wEnterAmount oEnterAmount = new wEnterAmount(); //*Net 63-04-01 ยกมาจาก baseline
        private string tW_Max;  //*Arm 63-03-10  - เช็คค่าที่สามารถกดได้สูงสุด
        private string tW_Min;  //*Arm 63-03-10  - เช็คค่าที่สามารถกดได้ต่ำสุด/
        public uQty()
        {
            InitializeComponent();
            W_SETxDesign(); //*Net 63-03-28 ยกมาจาก baseline
        }
        /// <summary>
        /// * Arm 63-03-11 - ปรับแก้ ค่าที่สามารถกำหนดค่าที่กดได้สูงสุด - และต่ำสุด 
        /// </summary>
        /// <param name="ptMaxValue">ค่าที่สามารถกดได้สูงสุด</param>
        /// <param name="ptMinValue">ค่าที่สามารถกดได้ต่ำสุด</param>
        public uQty(string ptMaxValue = "", string ptMinValue = "")
        {
            InitializeComponent();
            tW_Max = ptMaxValue;  //*Arm 63-03-10
            tW_Min = ptMinValue;  //*Arm 63-03-10
            //ocmAdd.BackColor = cVB.oVB_ColDark; //*Arm 63-03-10  ปรับเป็น cVB.oVB_ColDark
            //ocmDel.BackColor = cVB.oVB_ColDark; //*Arm 63-03-10  ปรับเป็น cVB.oVB_ColDark
            //otbQty.BackColor = cVB.oVB_ColLight;
            W_SETxDesign(); //*Net 63-03-28 ยกมาจาก baseline
        }

        //*Net 63-03-05
        public void W_SETxDesign()
        {
            ocmAdd.BackColor = cVB.oVB_ColNormal;
            ocmDel.BackColor = cVB.oVB_ColNormal;
            if (cVB.nVB_SaleModeStd == 1)
            {
                otbQty.ReadOnly = false;
                otbQty.BackColor = Color.White;
            }
            else
            {
                //otbQty.ReadOnly = true;
                otbQty.BackColor = cVB.oVB_ColLight;
            }
        }
        private void ocmAdd_Click(object sender, EventArgs e)
        {

            try
            {
                //Arm 63-03-10 - เช็คค่าที่สามารถกดได้สูงสุด
                if (!string.IsNullOrEmpty(tW_Max))
                {
                    if (otbQty.Text == tW_Max)
                    {
                        return;
                    }
                }
                //+++++++++++

                int nNumber;
                if (int.TryParse(otbQty.Text, out nNumber))
                {
                    nNumber += 1;
                }
                else
                {
                    nNumber = 1;
                }

                otbQty.Text = nNumber.ToString();
                oU_Value.Text = otbQty.Text;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uQty", "ocmAdd_Click " + oEx.Message);
            }
        }

        private void ocmDel_Click(object sender, EventArgs e)
        {
            try
            {
                //*Arm 63-03-10  
                if (string.IsNullOrEmpty(tW_Min)) //ไม่กำหนดค่าที่สามารถกดได้ต่ำสุด
                {
                    if (otbQty.Text == "0")
                    {
                        return;
                    }
                }
                else
                {
                    if (otbQty.Text == tW_Min)
                    {
                        return;
                    }
                }
                //++++++++++++++++

                int nNumber;
                if (int.TryParse(otbQty.Text, out nNumber))
                {
                    nNumber -= 1;
                }
                else
                {
                    nNumber = 0;
                }
                otbQty.Text = nNumber.ToString();
                oU_Value.Text = otbQty.Text;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uQty", "ocmDel_Click " + oEx.Message);
            }
        }

        //*Net 63-04-01 ยกมาจาก baseline
        private void otbQty_Click(object sender, EventArgs e)
        {
            //cVB.tVB_KbdScreen == "SALESTD"
            if (cVB.nVB_SaleModeStd == 1)
            {
                otbQty.ReadOnly = false;
            }
            else
            {
                otbQty.ReadOnly = true;
                oEnterAmount.ShowDialog();
                otbQty.Text = oEnterAmount.otbAmount.Text; //*Net 63-03-05
            }
        }

        //*Net 63-04-01 ยกมาจาก baseline
        private void otbQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(otbQty.Text))
                {
                    if (string.IsNullOrEmpty(tW_Min))
                    {
                        otbQty.Text = "0";
                        oU_Value.Text = "0";
                    }
                    else
                    {
                        otbQty.Text = tW_Min;
                        oU_Value.Text = tW_Min;
                    }
                    otbQty.SelectAll();
                }
                oU_Value.Text = Convert.ToInt32(otbQty.Text).ToString();
                //*Net 63-04-13
                otbQty.Text = Convert.ToInt32(otbQty.Text).ToString();
                otbQty.SelectionStart = otbQty.Text.Length;
                otbQty.SelectionLength = 0;
                //++++++++++++++++

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uQty", "otbQty_TextChanged " + oEx.Message);
            }
        }

        //*Net 63-04-01 ยกมาจาก baseline
        private void otbQty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uQty", "otbQty_KeyDown " + oEx.Message);
            }
        }

        //*Net 63-04-01 ยกมาจาก baseline
        private void otbQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.')) || e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uQty", "otbQty_KeyPress : " + oEx.Message); }
        }
    }
}
