using AdaPos.Class;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.All
{
    public partial class wCountDown : Form
    {
        private int nC_CountDown;
        public wCountDown(int pnCountTimes, string ptTitle, int pnInterval_ms = 1000)
        {
            InitializeComponent();
            nC_CountDown = pnCountTimes;
            otmCountDown.Interval = pnInterval_ms;
            olaTitle.Text = ptTitle;
            olaCount.Text = nC_CountDown.ToString("N0");

            W_SETxDesign();
        }
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCountDown", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void wCountDown_Shown(object sender, EventArgs e)
        {
            otmCountDown.Start();
        }

        private void otmCountDown_Tick(object sender, EventArgs e)
        {
            if (nC_CountDown > 0)
            {
                nC_CountDown--;
                olaCount.Text = nC_CountDown.ToString("N0");

            }
            else
            {
                otmCountDown.Stop();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
