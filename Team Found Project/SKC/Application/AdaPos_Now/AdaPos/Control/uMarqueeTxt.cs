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
using System.Reflection;

namespace AdaPos.Control
{
    public partial class uMarqueeTxt : UserControl
    {
        private List<string> atU_Txt;
        private int nU_Step;
        private int nU_TxtWidth;
        private string tU_MarqueeText;
        private string tU_SetText;
        private string tU_Space;
        private string tU_QuoteF;
        private string tU_QuoteR;
        private int nU_Rnd=0; //*Arm 63-08-13

        public uMarqueeTxt(List<string> patMqTxt = null, int pnSpace = 25, int pnInterval = 10, int pnStep = 1, Font poFont = null, string ptSplitter = "", string ptQuoteF = "", string ptQuoteR = "", Color? poForeColor = null)
        {
            InitializeComponent();
            try
            {
                //if (pnSpace > 0)
                //{

                //    tU_Space = new string(' ', pnSpace);
                //    if (patMqTxt != null)
                //    {
                //        if (patMqTxt.Count > 1)
                //        {
                //            tU_Space += ptSplitter;
                //        }
                //    }
                //    tU_Space += new string(' ', pnSpace);

                //}
                //else tU_Space = "";

                //tU_QuoteF = ptQuoteF;
                //tU_QuoteR = ptQuoteR;
                atU_Txt = patMqTxt;
                nU_Step = pnStep;
                if (atU_Txt == null || atU_Txt.Count == 0)
                {
                    atU_Txt = new List<string>();
                    atU_Txt.Add("");
                }

                if (poFont != null)
                {
                    olaText.Font = poFont;
                }

                //*Em 63-08-14
                if (poForeColor == null)
                {
                    olaText.ForeColor = Color.Black;
                }
                else
                {
                    olaText.ForeColor = poForeColor.Value;
                }
                 

                //tU_SetText = W_GENxLongText(atU_Txt, tU_Space);
                tU_SetText = atU_Txt[0].ToString(); //*Arm 63-08-13
                W_SETxLabelTxt(tU_SetText);

                tU_MarqueeText = tU_SetText;
                //nU_TxtWidth = olaText.Width;

                if (pnInterval > 0)
                {
                    //while (olaText.Location.X + olaText.Size.Width < this.Width)
                    //while (olaText.Location.X < -olaText.Size.Width-5)
                    //{
                    //    tU_SetText += (tU_SetText);
                    //    W_SETxLabelTxt(tU_SetText);
                    //}
                    //tU_MarqueeText = tU_SetText;
                    //nU_TxtWidth = olaText.Width;
                    tU_MarqueeText = ""; //*Arm 63-08-13
                    olaText.Location = new Point(-olaText.Size.Width, olaText.Location.Y); //*Arm 63-08-13
                    otmMoveText.Interval = pnInterval;
                    otmMoveText.Enabled = true;
                }
                else
                {
                    olaText.Location = new Point(0, olaText.Location.Y); //*Arm 63-08-13
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uMarqueeTxt", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }

        }

        private void uMarqueeTxt_Load(object sender, EventArgs e)
        {
        }

        private void otmMoveText_Tick(object sender, EventArgs e)
        {
            try
            {
                //*Arm 63-08-13
                if (olaText.Location.X < -olaText.Size.Width-2)
                {
                    tU_MarqueeText = atU_Txt[nU_Rnd].ToString();
                    W_SETxLabelTxt(tU_MarqueeText);
                    olaText.Location = new Point(this.Width, olaText.Location.Y);
                    if(nU_Rnd == atU_Txt.Count - 1) //*Arm 63-08-15
                    {
                        nU_Rnd = 0;
                    }
                    else
                    {
                        nU_Rnd += 1;
                    }
                }
                //+++++++++++++++
                //if (olaText.Location.X + olaText.Size.Width < this.Width)
                //{
                //    tU_MarqueeText += tU_SetText;
                //    W_SETxLabelTxt(tU_MarqueeText);

                //    //olaText.Location = new Point(this.Width, 0);
                //}
                else
                {
                    olaText.Location = new Point(olaText.Location.X - nU_Step, olaText.Location.Y);
                }
                //if (Math.Abs(olaText.Location.X) + 10 >= Math.Abs(nU_TxtWidth))
                //{
                //    tU_MarqueeText = tU_SetText;
                //    W_SETxLabelTxt(tU_MarqueeText);
                //    olaText.Location = new Point(0, olaText.Location.Y);
                //}
                this.Refresh();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uMarqueeTxt", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private void W_SETxLabelTxt(string ptText)
        {
            try
            {
                olaText.AutoSize = true;
                olaText.Text = ptText;
                int nWidth = olaText.Width;
                olaText.AutoSize = false;
                olaText.TextAlign = ContentAlignment.MiddleLeft;
                olaText.Size = new Size(nWidth, this.Height);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uMarqueeTxt", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        private string W_GENxLongText(List<string> patListTxt, string ptSpace)
        {
            try
            {
                return String.Join(ptSpace, patListTxt.Select(tTxt => tU_QuoteF + tTxt + tU_QuoteR).ToList()) + ptSpace;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("uMarqueeTxt", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return "";
        }
    }
}
