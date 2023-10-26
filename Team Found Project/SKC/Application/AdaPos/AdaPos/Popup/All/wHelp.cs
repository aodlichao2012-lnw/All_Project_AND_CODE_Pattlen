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

namespace AdaPos.Popup.All
{
    public partial class wHelp : Form
    {
        public wHelp()
        {
            InitializeComponent();

            try
            {

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHelp", "wHelp " + oEx.Message); }
        }

        /// <summary>
        /// Close popup 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wHelp_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wHelp", "wHelp_Click " + oEx.Message); }
        }
    }
}
