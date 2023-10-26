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
using AdaPos.Models.Database;

namespace AdaPos.Control
{
    public partial class uMsgQueue : UserControl
    {
        
        public uMsgQueue( string ptMsg, DateTime pdDate)
        {
            InitializeComponent();

            try
            {
                opbMsgType.Image = (Image)(Properties.Resources.ResourceManager.GetObject("CstDefault_256"));
                olaQData.Text = ptMsg;

                string[] aData = ptMsg.Split('|');
                olaMsg.Text = aData[0].ToString()+"\r\n"+ aData[1].ToString();
                olaDate.Text = pdDate.ToString("dd/MM/yyyy HH:mm:ss");
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uMsgQueue", "uMsgQueue : " + oEx.Message); }
            finally
            {
                ptMsg = null;
            }
        }

        private void uMsgQueue_MouseDown(object sender, MouseEventArgs e)
        {
            cmlTCNTMsgQueue oOrder;
            oOrder = new cmlTCNTMsgQueue();
            try
            {
                this.BorderStyle = BorderStyle.Fixed3D;
                this.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        oOrder = new cmlTCNTMsgQueue();
                        oOrder.FTMsgQData = olaDate.Text;
                        oOrder.FTMsgRemark = olaQData.Text;
                        //MessageBox.Show(olaQData.Text);
                        cVB.oVB_Sale.W_CSTxQueueMember(olaQData.Text);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uMsgQueue", "uMsgQueue_MouseDown " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void uMsgQueue_MouseHover(object sender, EventArgs e)
        {
            try
            {
                this.BackColor = cVB.oVB_ColNormal;
                this.BorderStyle = BorderStyle.Fixed3D;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uMsgQueue", "uMsgQueue_MouseHover " + oEx.Message); }
        }

        private void uMsgQueue_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                this.BorderStyle = BorderStyle.None;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uMsgQueue", "uMsgQueue_MouseLeave " + oEx.Message); }
        }

        private void uMsgQueue_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                this.BorderStyle = BorderStyle.None;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uMsgQueue", "uMsgQueue_MouseUp " + oEx.Message); }
        }
    }
}
