using AdaPos.Class;
using AdaPos.Forms;
using AdaPos.Resources_String.Local;
using System;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace AdaPos.Popup.wLogin
{
    public partial class wPasscode : Form
    {
        #region Variable

        private ResourceManager oW_Resource;

        #endregion End Variable

        #region Constructor

        public wPasscode()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();

                otbPasscode.MaxLength = cVB.nVB_MaxLenPasscode;
                opcPincode.oU_TextValue = otbPasscode;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "wPasscode : " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Check passcode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmOK_Click(object sender, EventArgs e)
        {
            try
            {
                W_CHKxPasscode();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "ocmOK_Click : " + oEx.Message); }
        }

        /// <summary>
        /// แสดงข้อมูล
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wPasscode_Shown(object sender, EventArgs e)
        {
            try
            {
                otbPasscode.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "wPasscode_Shown : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "OnPaintBackground : " + oEx.Message); }
        }

        /// <summary>
        /// Passcode 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbPasscode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        this.Close();
                        this.Dispose();
                        break;

                    case Keys.Enter:
                        W_CHKxPasscode();
                        break;
                }

                if (otbPasscode.Text.Length > otbPasscode.MaxLength)
                    e.Handled = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "otbPasscode_KeyDown : " + oEx.Message); }
        }

        /// <summary>
        /// Key number only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbPasscode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;

                if (e.KeyChar == (char)Keys.Back)
                    e.Handled = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "otbPasscode_KeyPress : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resSetting_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSetting_EN));
                        break;
                }

                olaTitlePasscode.Text = oW_Resource.GetString("tPasscode");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Check passcode
        /// </summary>
        private void W_CHKxPasscode()
        {
            int nChkForm;

            try
            {
                if (string.IsNullOrEmpty(otbPasscode.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tInputPwd"), 3);
                    return;
                }

                if (string.Equals(otbPasscode.Text, cVB.tVB_Passcode))
                {
                    nChkForm = Application.OpenForms.OfType<Form>().Count();
                    new wSetting().Show();

                    if (nChkForm == 1)
                        this.Hide();
                    else
                        this.Close();
                }
                else
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tPwdIncorrect"), 3);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPasscode", "W_CHKxPasscode : " + oEx.Message); }
        }

        #endregion End Function
    }
}
