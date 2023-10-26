using AdaPos.Class;
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

namespace AdaPos
{
    public partial class wProductM : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        #endregion End Variable

        public wProductM()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();
                //*Net 63-07-31 ปรับตาม Moshi
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in opnMenu.Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "wProductM : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }

        /// <summary>
        /// GET Notification
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void W_Notification(object s, EventArgs e)
        {
            try
            {
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wHome", "W_Notification : " + oEx.Message);
            }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmSearch.BackColor = cVB.oVB_ColNormal;
                ocmDelete.BackColor = cVB.oVB_ColNormal;
                ocmAdd.BackColor = cVB.oVB_ColNormal;

                //ogdProduct.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdProduct); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resProduct_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resProduct_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "Product";

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");
                olaTitleProduct.Text = oW_Resource.GetString("tProduct");

                // user
                olaUsrName.Text = new cUser().C_GETtUsername();

                // Main
                olaTitleOtherName.Text = oW_Resource.GetString("tOtherName") + " :";
                olaTitleGroup.Text = oW_Resource.GetString("tGroup") + " :";
                olaTitleType.Text = oW_Resource.GetString("tType") + " :";
                olaTitleSaleType.Text = oW_Resource.GetString("tSaleType") + " :";
                olaTitleShop.Text = oW_Resource.GetString("tShop") + " :";
                olaTitleSaleStart.Text = oW_Resource.GetString("tSaleStart") + " :";
                olaTitleSaleStop.Text = oW_Resource.GetString("tSaleStop") + " :";
                otbTitleCode.HeaderText = oW_Resource.GetString("tCode");
                otbTitleName.HeaderText = oW_Resource.GetString("tName");
                otbTitleBarcode.HeaderText = oW_Resource.GetString("tBarcode");
                otbTitleUnit.HeaderText = oW_Resource.GetString("tUnit");
                otbTitlePrice.HeaderText = oW_Resource.GetString("tPrice");
                otbTitleBalance.HeaderText = oW_Resource.GetString("tBalance");
                olaTitleSearch.Text = oW_Resource.GetString("tSearch");
                olaTitleShowData.Text = oW_Resource.GetString("tShowData");

                // Search Customer By
                ocbSearchPdtBy.Items.Add(oW_Resource.GetString("tCode"));
                ocbSearchPdtBy.Items.Add(oW_Resource.GetString("tName"));
                ocbSearchPdtBy.Items.Add(oW_Resource.GetString("tBarcode"));
                ocbSearchPdtBy.Items.Add(oW_Resource.GetString("tUnit"));
                ocbSearchPdtBy.Items.Add(oW_Resource.GetString("tPrice"));
                ocbSearchPdtBy.Items.Add(oW_Resource.GetString("tBalance"));
                ocbSearchPdtBy.SelectedIndex = 1;

                // Search : Match
                ocbSchMatch.Items.Add(cVB.oVB_GBResource.GetString("tPartField"));
                ocbSchMatch.Items.Add(cVB.oVB_GBResource.GetString("tWholeField"));
                ocbSchMatch.SelectedIndex = 0;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                new wHome().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wProductM_Shown(object sender, EventArgs e)
        {
            try
            {
                //*Net 63-07-31 ปรับตาม Moshi
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                otbSearchPdt.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "wProductM_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Call Keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wProductM", "ocmKB_Click : " + ex.Message); }
        }

        /// <summary>
        /// Show function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDoShowKB();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmShwKb_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open Calculate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxCalculator();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmCalculate_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open popup help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHelp_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxHelp();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmHelp_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open Popup about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAbout_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAbout();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmMenu_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Search Customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                olaPagePdt.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), "0", "0", "0");
                olaTitleShowData.Visible = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmSearch_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Add Customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAdd_Click(object sender, EventArgs e)
        {
            try
            {
                new wPdtAddOrEdit().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmAdd_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wProductM_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "wProductM_FormClosing : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wProductM", "ocmNotify_Click : " + oEx.Message); }
        }
    }
}
