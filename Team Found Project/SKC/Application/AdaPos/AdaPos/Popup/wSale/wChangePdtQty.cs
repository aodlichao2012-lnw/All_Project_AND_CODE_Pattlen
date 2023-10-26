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
using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Resources_String.Local;

namespace AdaPos.Popup.wSale
{
    public partial class wChangePdtQty : Form
    {
        private ResourceManager oW_Resource;
        
        public wChangePdtQty()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
                
                onpNumpad.oU_TextValue = otbQtyNew;
                onpNumpad.tU_TextValue = "";
                onpNumpad.ocmDot.Enabled = false;
                onpNumpad.ocmDot.BackColor = ColorTranslator.FromHtml("#CCCCCC");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangePdtQty", "wChangPdtQty " + oEx.Message); }
           
        }

        /// <summary>
        /// Design form
        /// </summary>
        private void W_SETxDesign()     //*Arm 62-10-07
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;

                otbQtyOld.BackColor = cVB.oVB_ColLight; //*Net 63-04-01 ยกมาจาก baseline
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangePdtQty", "W_SETxDesign : " + oEx.Message); }

        }

        /// <summary>
        /// Text form
        /// </summary>
        private void W_SETxText()   //*Arm 62-10-07
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

                olaTitle.Text = oW_Resource.GetString("tTitleChangePdtQty");
                olaQtyOld.Text = oW_Resource.GetString("tPdtQtyOld");
                olaQtyNew.Text = oW_Resource.GetString("tPdtQtyNew");

                otbQtyOld.Text = cSale.cC_DTQty.ToString();
                otbQtyNew.Text = cSale.cC_DTQty.ToString();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangePdtQty", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Close Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)  //*Arm 62-10-07
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangePdtQty", "ocmBack_Click " + oEx.Message); }
        }

        private void ocmAccept_Click(object sender, EventArgs e)  
        {
            try
            {
                cSale.cC_DTQty = Convert.ToDecimal(otbQtyNew.Text);
                cVB.cVB_PriceAfEditQty = cSale.C_GETcSalDTPriceEditQty(cSale.nC_DTSeqNo);

                if (cSale.cC_DTQty <= 0) //*Arm 63-04-14 : เช็คจำนวนต้องมากกว่า 0
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgEditQtyFale"), 1);
                    onpNumpad.tU_TextValue = "";
                    otbQtyNew.Focus();
                    otbQtyNew.SelectAll();
                    return;
                }
                else
                {
                    if (cVB.cVB_PriceAfEditQty < 0)
                    {
                        if (new cSP().SP_SHWoMsg(oW_Resource.GetString("tMsgEditQtyPriceless"), 1) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                

                DialogResult = DialogResult.OK; //*Arm 62-10-07
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChangePdtQty", "ocmAccept_Click " + oEx.Message); }
        }

        private void wChangePdtQty_Shown(object sender, EventArgs e)
        {
            otbQtyNew.Focus();
            otbQtyNew.SelectionStart = 0;
            otbQtyNew.SelectAll();
        }
    }
}
