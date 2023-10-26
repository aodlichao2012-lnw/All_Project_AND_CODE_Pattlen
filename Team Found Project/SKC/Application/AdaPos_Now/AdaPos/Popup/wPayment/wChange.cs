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

namespace AdaPos.Popup.wPayment
{
    public partial class wChange : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        

        public wChange()
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();

                W_SETxDesign();
                W_SETxText();
                otmClose.Interval = 10000;  //*Arm 62-10-01
                otmClose.Start();           //*Arm 62-10-01
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChange", "wChange " + oEx.Message); }
        }

        #region Function
        /// <summary>
        /// Set design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                olaChange.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChange", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                olaChange.Text = oW_Resource.GetString("tChange");
                olaNumChange.Text = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_Change, cVB.nVB_DecShow);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChange", "W_SETxText : " + oEx.Message); }
        }
        #endregion Function

        #region Method
        /// <summary>
        /// Paint Background
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                using (SolidBrush oBrush = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(oBrush, e.ClipRectangle);
                    oBrush.Dispose();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChange", "OnPaintBackground " + oEx.Message); }
        }

        private void opnChange_Click(object sender, EventArgs e)
        {
                this.Close();
                this.Dispose();
        }
        #endregion Method

        private void otmClose_Tick(object sender, EventArgs e)
        {
            //*Arm 62-10-01
            this.Close();
            this.Dispose();
        }
    }
}
