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

namespace AdaPos.Popup.wSale
{
    public partial class wChooseBill : Form
    {
        public wChooseBill()
        {
            InitializeComponent();

            //*Net 63-04-01 ยกมาจาก baseline
            new cSP().SP_SETxSetGridviewFormat(ogdPerson); //*Net 63-03-02 Set Style
        }

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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Accept
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReferBill", "ocmAccept_Click : " + oEx.Message); }
        }
    }
}
