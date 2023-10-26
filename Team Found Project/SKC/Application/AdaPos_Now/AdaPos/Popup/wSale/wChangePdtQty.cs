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
        private int nW_Mode;    //1 : แก้ไขจำนวน 2:แก้ไขโควต้า
        cSP oW_SP;  //*Em 63-07-16

        public wChangePdtQty(int pnMode = 1)
        {
            InitializeComponent();

            try
            {
                nW_Mode = pnMode;   //*Em 63-07-14
                oW_SP = new cSP();  //*Em 63-07-16
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

                //*Em 63-07-16
                switch (nW_Mode)
                {
                    case 1:
                        olaLimitQty.Visible = false;
                        otbLimitQty.Visible = false;
                        break;
                    case 2:
                        olaLimitQty.Visible = true;
                        otbLimitQty.Visible = true;
                        break;
                }
                //+++++++++++++
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

                //*Em 63-07-16
                switch (nW_Mode)
                {
                    case 1:
                        olaTitle.Text = oW_Resource.GetString("tTitleChangePdtQty");
                        break;
                    case 2:
                        olaTitle.Text = oW_Resource.GetString("tTitleChangeQuota");
                        olaLimitQty.Text = oW_Resource.GetString("tLimitQty");
                        otbLimitQty.Text = oW_SP.SP_SETtDecShwSve(1, cSale.cC_QuotaLimit, cVB.nVB_DecShow);
                        break;
                }
                //++++++++++++++
                
                olaQtyOld.Text = oW_Resource.GetString("tPdtQtyOld");
                olaQtyNew.Text = oW_Resource.GetString("tPdtQtyNew");

                //otbQtyOld.Text = cSale.cC_DTQty.ToString();
                //otbQtyNew.Text = cSale.cC_DTQty.ToString();

                //*Em 63-06-06
                otbQtyOld.Text = oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow);
                otbQtyNew.Text = oW_SP.SP_SETtDecShwSve(1, cSale.cC_DTQty, cVB.nVB_DecShow);
                //++++++++++++++

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
            decimal cQty; //*Net 63-07-31
            try
            {
                //*Net 63-07-06
                if (String.IsNullOrEmpty(otbQtyNew.Text) || decimal.TryParse(otbQtyNew.Text, out cQty) == false)
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgEditQtyFale"), 1);
                    onpNumpad.tU_TextValue = "";
                    otbQtyNew.Focus();
                    otbQtyNew.SelectAll();
                    return;
                }
                cSale.cC_DTQty = Convert.ToDecimal(otbQtyNew.Text);
                cVB.cVB_PriceAfEditQty = cSale.C_GETcSalDTPriceEditQty(cSale.nC_DTSeqNo);

                if (nW_Mode == 1)
                {
                    if (cSale.cC_DTQty <= 0) //*Arm 63-04-14 : เช็คจำนวนต้องมากกว่า 0
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgEditQtyFale"), 1);
                        onpNumpad.tU_TextValue = "";
                        otbQtyNew.Focus();
                        otbQtyNew.SelectAll();
                        return;
                    }
                    else
                    {
                        if (cVB.cVB_PriceAfEditQty < 0)
                        {
                            if (oW_SP.SP_SHWoMsg(oW_Resource.GetString("tMsgEditQtyPriceless"), 1) == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }
                }
                    
                //*Em 63-07-16
                if (nW_Mode == 2)
                {
                    if (cSale.cC_DTQty < 0)
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgEditQtyFale"), 1);
                        onpNumpad.tU_TextValue = "";
                        otbQtyNew.Focus();
                        otbQtyNew.SelectAll();
                        return;
                    }
                    //if (Convert.ToDecimal(otbQtyNew.Text) > Convert.ToDecimal(otbLimitQty.Text))
                    if ((Convert.ToDecimal(otbQtyNew.Text) > Convert.ToDecimal(otbLimitQty.Text)) || (Convert.ToDecimal(otbQtyNew.Text) > cSale.cC_QtyLimit && cSale.cC_QtyLimit > 0))      //*Em 63-09-16
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgOverLimit"), 3);
                        onpNumpad.tU_TextValue = "";
                        //otbQtyNew.Text = otbLimitQty.Text;
                        if (Convert.ToDecimal(otbQtyNew.Text) > cSale.cC_QtyLimit && cSale.cC_QtyLimit > 0)
                        {
                            otbQtyNew.Text = oW_SP.SP_SETtDecShwSve(1, cSale.cC_QtyLimit,cVB.nVB_DecShow);
                        }
                        else
                        {
                            otbQtyNew.Text = otbLimitQty.Text;
                        }
                        otbQtyNew.Focus();
                        otbQtyNew.SelectAll();
                        return;
                    }
                }
                //+++++++++++++
                

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

        //*Net 63-07-31
        private void wChangePdtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ocmAccept_Click(ocmAccept, new EventArgs());
            }
        }
    }
}
