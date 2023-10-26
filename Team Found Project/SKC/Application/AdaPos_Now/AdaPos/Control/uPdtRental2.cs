using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using AdaPos.Class;
using AdaPos.Models.Database;
using System.IO;
using System.Resources;
using AdaPos.Resources_String.Local;

namespace AdaPos.Control
{
    public partial class uPdtRental2 : UserControl
    {
        public event MouseEventHandler Event_uRentalPdt_Mouse_Click;
        public List<cmlPdtRental> aoW_Pdt;
        public ResourceManager oW_Resource;

        public uPdtRental2()
        {
            InitializeComponent();
            W_SETxText();
        }


        /// <summary>
        /// [ping][2019.06.06][ปรับมุมของ Panel]
        /// </summary>
        /// <param name="poG"></param>
        /// <param name="poP"></param>
        /// <param name="pcX"></param>
        /// <param name="pcY"></param>
        /// <param name="pcWidth"></param>
        /// <param name="pcHeight"></param>
        /// <param name="pcRadius"></param>
        public void W_DrawRoundRect(Graphics poG, Pen poP, float pcX, float pcY, float pcWidth, float pcHeight, float pcRadius)
        {
            try
            {
                GraphicsPath oGP = new GraphicsPath();
                oGP.AddLine(pcX + pcRadius, pcY, pcX + pcWidth - (pcRadius * 2), pcY);
                oGP.AddArc(pcX + pcWidth - (pcRadius * 2), pcY, pcRadius * 2, pcRadius * 2, 270, 90);
                oGP.AddLine(pcX + pcWidth, pcY + pcRadius, pcX + pcWidth, pcY + pcHeight - (pcRadius * 2));
                oGP.AddArc(pcX + pcWidth - (pcRadius * 2), pcY + pcHeight - (pcRadius * 2), pcRadius * 2, pcRadius * 2, 0, 90);
                oGP.AddLine(pcX + pcWidth - (pcRadius * 2), pcY + pcHeight, pcX + pcRadius, pcY + pcHeight);
                oGP.AddArc(pcX, pcY + pcHeight - (pcRadius * 2), pcRadius * 2, pcRadius * 2, 90, 90);
                oGP.AddLine(pcX, pcY + pcHeight - (pcRadius * 2), pcX, pcY + pcRadius);
                oGP.AddArc(pcX, pcY, pcRadius * 2, pcRadius * 2, 180, 90);
                oGP.CloseFigure();
                poG.DrawPath(poP, oGP);
                oGP.Dispose();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uPdtRental2", "W_DrawRoundRect " + oEx.Message);
            }
        }


        private void opnPdt_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics oGraphics = e.Graphics;
                Pen oPen = new Pen(Color.FromArgb(183, 183, 183));
                W_DrawRoundRect(oGraphics, oPen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1, 10);
                base.OnPaint(e);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uPdtRental2", "opnPdt_Paint " + oEx.Message);
            }
        }

        private void uPdtRental2_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Event_uRentalPdt_Mouse_Click(this, e);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uPdtRental2", "uPdtRental2_MouseClick " + oEx.Message);
            }

        }

        private void uPdtRental2_Load(object sender, EventArgs e)
        {
            try
            {
                W_SETxPdt(aoW_Pdt);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uPdtRental2", "uPdtRental2_Load " + oEx.Message);
            }
        }

        public void W_SETxPdt(List<cmlPdtRental> paoPdt)
        {
            try
            {
                opbPdt.Image = Properties.Resources.Product_256;
                if (paoPdt == null)
                {
                    return;
                }
                //Product Image

                if (File.Exists(paoPdt.FirstOrDefault().FTImgObj))
                {
                    opbPdt.Image = Image.FromFile(paoPdt.FirstOrDefault().FTImgObj);
                }
                //Product Name
                olaPdtName.Text = paoPdt.FirstOrDefault().FTPdtName;
                olaPdtName.Tag = paoPdt.FirstOrDefault().FTPdtCode;
                olaDepositPrice.Text = "฿" + paoPdt.FirstOrDefault().FCPdtDeposit;
                paoPdt = paoPdt.OrderBy(x => x.FNRtdSeqNo).ToList();
                for (int nIndex = 0; nIndex < 2; nIndex++)
                {
                    switch (nIndex)
                    {
                        case 0:
                            olaRentalTime1.Text = paoPdt[nIndex].FNRtdMinQty + GETtTmeType(paoPdt[nIndex].FTRtdTmeType);
                            olaRentalPrice1.Text = "฿" + paoPdt[nIndex].FCRtdPrice;
                            break;
                        case 1:
                            olaRentalTime2.Text = paoPdt[nIndex].FNRtdMinQty + GETtTmeType(paoPdt[nIndex].FTRtdTmeType);
                            olaRentalPrice2.Text = "฿" + paoPdt[nIndex].FCRtdPrice;
                            break;
                        case 2:
                            olaRentalTime3.Text = paoPdt[nIndex].FNRtdMinQty + GETtTmeType(paoPdt[nIndex].FTRtdTmeType);
                            olaRentalPrice3.Text = "฿" + paoPdt[nIndex].FCRtdPrice;
                            break;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uPdtRental2", "uPdtRental2_Load " + oEx.Message);
            }
        }

        public string GETtTmeType(string pnType)
        {
            string tType = string.Empty;
            try
            {
                switch (pnType)
                {
                    case "1":
                        tType = oW_Resource.GetString("TmeType1");
                        break;
                    case "2":
                        tType = oW_Resource.GetString("TmeType2");
                        break;
                    case "3":
                        tType = oW_Resource.GetString("TmeType3");
                        break;
                    case "4":
                        tType = oW_Resource.GetString("TmeType4");
                        break;
                    case "5":
                        tType = oW_Resource.GetString("TmeType5");
                        break;
                }
                return " " + tType;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uPdtRental2", "GETtTmeType " + oEx.Message);
                return tType;
            }
            finally
            {
                tType = null;
            }
        }

        /// <summary>
        /// Set text
        /// </summary>
        public void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resRentalPdt_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resRentalPdt_EN));
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uPdtRental2", "W_SETxText : " + oEx.Message);
            }
        }
    }
}
