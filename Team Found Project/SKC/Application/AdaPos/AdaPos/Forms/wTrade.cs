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
    public partial class wTrade : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        #endregion End Variable

        public wTrade()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();

                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "wTrade : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Clear Variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wTrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "wTrade_FormClosing : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wTrade_Shown(object sender, EventArgs e)
        {
            try
            {
                otbTicketNo.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "wTrade_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                ocmAccept.BackColor = cVB.oVB_ColNormal;
                //ogdContent.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdContent); //*Net 63-03-03 Set Design Gridview

                opnPkg.BackColor = cVB.oVB_ColNormal;

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set tex form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resTrade_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resTrade_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "TRADE";

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                olaTrade.Text = oW_Resource.GetString("tTrade");
                olaTitleMode.Text = oW_Resource.GetString("tMode");
                olaTitleTKNo.Text = oW_Resource.GetString("tTKNo");
                ockOneByOne.Text = oW_Resource.GetString("tOneByOne");
                olaTitleTKQty.Text = oW_Resource.GetString("tTKQty");
                olaTitleWBQty.Text = oW_Resource.GetString("tWBQty");
                otbTitlePdtName.HeaderText = oW_Resource.GetString("tPdtName");
                otbTitleSeq.HeaderText = oW_Resource.GetString("tSeq");
                otbTitleTicket.HeaderText = oW_Resource.GetString("tTKNo");
                otbTitleWBNo.HeaderText = oW_Resource.GetString("tWBNo");

                // user
                olaUsrName.Text = new cUser().C_GETtUsername();

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "W_SETxText : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "ocmMenu_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wTrade", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "ocmAbout_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade", "otmClose_Tick : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTrade:", "ocmNotify_Click : " + oEx.Message); }
        }
    }
}
