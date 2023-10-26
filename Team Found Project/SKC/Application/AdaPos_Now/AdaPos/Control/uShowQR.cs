using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using AdaPos.Class;

namespace AdaPos.Control
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox potbBox, string ptText, Color oColor)
        {
            potbBox.SelectionStart = potbBox.TextLength;
            potbBox.SelectionLength = 0;

            potbBox.SelectionFont = potbBox.Font;
            potbBox.SelectionColor = oColor;
            potbBox.AppendText(ptText);
            potbBox.SelectionColor = potbBox.ForeColor;
        }
    }
    public partial class uShowQR : UserControl
    {

        public uShowQR(float pcFontSize, string ptDescript, Image poQR, Image poHD, string ptCountDown)
        {
            InitializeComponent();
            otbDescript.Font = new System.Drawing.Font("Segoe UI", pcFontSize);
            otbDescript.Text = ptDescript;
            otbDescript.AppendText($" ({ptCountDown})", Color.Red);

            otbDescript.SelectionAlignment = HorizontalAlignment.Center;
            opbQR.Image = poQR;
            opbHDPic.Margin = new Padding(0, 5, 0, 0);
            opbHDPic.Image = poHD;
        }

        private void uShowQR_Load(object sender, EventArgs e)
        {
        }
        public void W_SETxTableSize()
        {
            try
            {
                if (opbHDPic.Image != null && opbQR.Image != null)
                {
                    opnPage.RowStyles[0].Height = this.Height * 0.2f;
                    opnPage.RowStyles[2].Height = this.Height * 0.15f;
                    opnPage.RowStyles[1].Height = this.Height - opnPage.RowStyles[0].Height - opnPage.RowStyles[2].Height;

                    int nActualWidth = (int)(opbHDPic.Image.Width * opbHDPic.ClientSize.Height / opbHDPic.Image.Height);
                    if (nActualWidth < opnPage.RowStyles[1].Height)
                    {
                        opbQR.Size = new Size(nActualWidth, nActualWidth);
                        opbQR.Margin = new Padding((int)((opnPage.Width - opbQR.Size.Width) / 2),
                                                   (int)((opnPage.RowStyles[1].Height - opbQR.Size.Height) / 2), 0, 0);
                    }
                    else
                    {
                        opbQR.Dock = DockStyle.Fill;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uShowQR", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }

        }
        public void W_SETxDescript(string ptDescript, string ptCountDown)
        {
            otbDescript.Text = ptDescript;
            otbDescript.AppendText($" ({ptCountDown})", Color.Red);
            otbDescript.SelectionAlignment = HorizontalAlignment.Center;
        }
    }
}
