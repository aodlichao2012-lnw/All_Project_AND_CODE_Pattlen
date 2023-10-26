using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdaPos.Class;

namespace AdaPos.Control
{
    public partial class uNotify : UserControl
    {
        public uNotify(int pnMsgType, string ptMsg, DateTime pdDate)
        {
            InitializeComponent();

            try
            {
                opbMsgType.Image = (Image)(Properties.Resources.ResourceManager.GetObject("Notify_" + pnMsgType));
                olaMsg.Text = ptMsg;
                olaDate.Text = pdDate.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uNotify", "uNotify : " + oEx.Message); }
            finally
            {
                ptMsg = null;
            }
        }

        private void olaMsg_Resize(object sender, EventArgs e)
        {
            try
            {
                //this.Height = this.Height + olaMsg.Height; //*Arm 62-10-28 -Comment Code
                this.Height = this.Height + olaMsg.Height -25;  //*BOY 62-11-07
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uNotify", "olaMsg_Resize : " + oEx.Message);
            }
        }
    }
}
