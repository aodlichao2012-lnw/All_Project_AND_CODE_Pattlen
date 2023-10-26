using AdaPos.Class;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdaPos.Forms
{
    public partial class wVideo : Form
    {
        #region Variable

        private cSP oW_SP;
        private int nW_Time;

        #endregion End Variable

        #region Constructor

        public wVideo()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wYoutube", "wYoutube : " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

        #endregion End Event

        #region Function

        #endregion End Function

        /// <summary>
        /// ขยาย - ย่อ Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "ocmMenu_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Set Design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                //ogvYoutube.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdYoutube); //*Net 63-03-03 Set Design Gridview

                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuBT.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;

                if (opbLogo.Image == null)
                    opbLogo.Size = new Size(0, opbLogo.Size.Height);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                // Menu
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Open form Login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                new wSplashScreen().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Show video youtube
        /// </summary>
        private void W_SHWxVideoYoutube(string ptYoutube)
        {
            try
            {
                //"https://www.youtube.com/embed/Ms-SRDXoUPw";
                owbYoutube.Navigate(ptYoutube);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "W_SHWxVideoYoutube : " + oEx.Message); }
            finally
            {
                ptYoutube = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Open Popup Help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHelp_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxHelp();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "ocmHelp_Click " + oEx.Message); }
        }

        /// <summary>
        /// Open Popup About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAbout_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAbout();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "ocmAbout_Click " + oEx.Message); }
        }

        /// <summary>
        /// Timing to Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmClose_Tick(object sender, EventArgs e)
        {
            try
            {
                if (nW_Time == 5)
                    this.Close();

                nW_Time++;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wVideo", "wVideo_FormClosing : " + oEx.Message); }
        }
    }
}
