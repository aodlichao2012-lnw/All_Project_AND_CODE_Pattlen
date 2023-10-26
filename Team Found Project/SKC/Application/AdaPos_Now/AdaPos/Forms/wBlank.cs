using AdaPos.Class;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Forms
{
    public partial class wBlank : Form
    {
        private ResourceManager oW_Resource;

        public wBlank()
        {
            InitializeComponent();
            try
            {
                //*Net 63-07-31 ปิด update notify
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                new cSP().SP_PRCxFlickering(this.Handle);

                W_SETxBackgroud();
                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBlank", "wAutoSync_Load : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                W_SETxOpenCloseMenu();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBlank", "wAutoSync_Load : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                //if (opnNotify.Visible == false)
                //{
                //    cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                //}
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBlank", "wAutoSync_Load : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_SETxDesign()
        {
            try
            {
                opnMenu.Width = 55;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;


                opbLogo.Image = new cCompany().C_GEToImageLogo();

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBlank", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resSync_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSync_TH));
                        break;
                }


                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBlank", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        private void W_SETxBackgroud()
        {

            string tImagePath = string.Empty;
            try
            {
                tImagePath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Background\splash.png";
                if (!File.Exists(tImagePath)) tImagePath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Background\splash.jpg";
                if (File.Exists(tImagePath))
                {
                    opnBG.BackgroundImage = Image.FromFile(tImagePath);
                    opnBG.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    opnBG.BackgroundImage = Properties.Resources.BG;
                    opnBG.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBlank", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_Notification(object s, EventArgs e)
        {
            try
            {
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBlank", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_SETxOpenCloseMenu()
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBlank", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void wBlank_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Thread.Sleep(1000);
        }

        private void wBlank_Shown(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
