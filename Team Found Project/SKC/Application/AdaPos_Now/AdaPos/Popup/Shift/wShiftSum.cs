using AdaPos.Class;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.Shift
{
    public partial class wShiftSum : Form
    {
        #region Variable
        #endregion End Variable

        public wShiftSum()
        {
            InitializeComponent();
            try
            {
                W_SETxText();
                W_SHWxPreview();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShiftSum", "wShiftSum : " + oEx.Message); }
        }

        #region Function
        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                olaTitleForm.Text = cVB.oVB_GBResource.GetString("tSaleSumRpt");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShiftSum", "W_SETxText : " + oEx.Message); }
        }

        private void W_SHWxPreview()
        {
            try
            {
                opnContent.Refresh();
                opnContent.AutoScroll = true;
                opnContent.VerticalScroll.Visible = true;
                opnContent.Paint += opnContent_Paint;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShiftSum", "W_SHWxPreview : " + oEx.Message); }
        }
        #endregion End Function

        #region Method/Events
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wShiftSum", "OnPaintBackground : " + oEx.Message); }
        }

        private void opnContent_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                cVB.nVB_StartY = 0; //*Arm 63-05-03

                cShift.C_PRNxSaleSumRpt(e.Graphics,false);

                //*Arm 63-05-03
                cVB.nVB_StartY += 100;

                if (cVB.nVB_StartY > 700 && cVB.nVB_StartY < 830)
                {
                    opnContent.Height = 1000 + (830 - cVB.nVB_StartY);
                }
                else if (cVB.nVB_StartY > 830)
                {
                    opnContent.Height = cVB.nVB_StartY;
                }
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShiftSum", "opnContent_Paint : " + oEx.Message); }
        }
        private void ocmPrint_Click(object sender, EventArgs e)
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            try
            {
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();

                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                oDoc.PrintController = new StandardPrintController();
                oDoc.PrintPage += oDoc_PrintPage;
                oDoc.Print();
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShiftSum", "ocmPrint_Click : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                //oSP.SP_CLExMemory();
            }
        }

        private void oDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                cShift.C_PRNxSaleSumRpt(e.Graphics);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShiftSum", "oDoc_PrintPage : " + oEx.Message); }
        }
        private void wShiftSum_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShiftSum", "wShiftSum_FormClosing : " + oEx.Message); }
        }
        #endregion Method/Events


    }
}
