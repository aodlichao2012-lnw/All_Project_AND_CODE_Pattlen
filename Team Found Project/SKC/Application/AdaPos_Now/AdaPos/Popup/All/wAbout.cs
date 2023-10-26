using AdaPos.Class;
using AdaPos.Resources_String.Local;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace AdaPos.Popup.All
{
    public partial class wAbout : Form
    {
        #region Variable

        private ResourceManager oW_Resource;

        #endregion End Variable

        public wAbout()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAbout", "wAbout : " + oEx.Message); }
        }

        /// <summary>
        /// Clsoe Popup
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wAbout", "ocmBack_Click : " + oEx.Message); }
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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAbout", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch(cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                olaTitleAbout.Text = oW_Resource.GetString("tAbout");
                olaVersion.Text = Application.ProductVersion;       // Get version
                olaUsrCom.Text = Environment.UserName;              // Get Username computer
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wAbout", "W_SETxText : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wAbout", "OnPaintBackground : " + oEx.Message); }
        }
    }
}
