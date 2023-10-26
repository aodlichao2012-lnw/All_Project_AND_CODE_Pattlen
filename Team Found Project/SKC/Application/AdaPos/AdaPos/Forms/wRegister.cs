using AdaPos.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wRegister : Form
    {
        private int nW_Time;

        public wRegister()
        {
            InitializeComponent();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wRegister", "ocmBack_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wRegister", "otmClose_Tick : " + oEx.Message); }
        }

        private void wRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRegister", "wRegister_FormClosing : " + oEx.Message); }
        }
    }
}
