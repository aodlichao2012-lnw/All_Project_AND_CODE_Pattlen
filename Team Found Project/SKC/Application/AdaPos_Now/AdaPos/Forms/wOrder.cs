using AdaPos.Class;
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
    public partial class wOrder : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        #endregion End Variable

        public wOrder()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);
                //*Net 63-07-31 ปรับตาม Moshi
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOrder", "wOrder : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wOrder", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Forn Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCancelTopUp", "wOrder_FormClosing : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOrder:", "ocmNotify_Click : " + oEx.Message); }
        }

        //*Net 63-07-31 ปรับตาม Moshi
        private void wOrder_Shown(object sender, EventArgs e)
        {
            if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
            cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
        }
    }
}
