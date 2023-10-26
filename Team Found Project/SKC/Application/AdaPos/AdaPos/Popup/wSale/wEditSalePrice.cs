using AdaPos.Class;
using AdaPos.Control;
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

namespace AdaPos.Popup.wSale
{
    public partial class wEditSalePrice : Form
    {
        private ResourceManager oW_Resource;
        public Decimal rcPrice { get; set; }
        public wEditSalePrice(string ptPdtCode, string ptPdtName, Decimal pcPdtPrice)
        {
            InitializeComponent();
            try
            {
                W_SETxDesign();
                W_SETxText();
                
                
                onpNumpad.oU_TextValue = otbNewPrice;
                onpNumpad.tU_TextValue = otbNewPrice.Text;
                otbPdtCode.Text = ptPdtCode;

                otbPdtName.Text = ptPdtName;
                otbOldPrice.Text = new cSP().SP_SETtDecShwSve(1, pcPdtPrice, cVB.nVB_DecShow);
                otbNewPrice.Text = new cSP().SP_SETtDecShwSve(1, pcPdtPrice, cVB.nVB_DecShow);

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditSalePrice", "wEditSalePrice " + oEx.Message); }
        }

        /// <summary>
        /// Design form
        /// </summary>
        private void W_SETxDesign()
        {
            //*Arm 62-09-23
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditSalePrice", "W_SETxDesign : " + oEx.Message); }
            
        }

        /// <summary>
        /// Text form
        /// </summary>
        private void W_SETxText()
        {
            //*Arm 62-09-23
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

                olaTitle.Text = oW_Resource.GetString("tEditSalePrice");
                olaPdtCode.Text = oW_Resource.GetString("tTitlePdtCode");
                olaPdtName.Text = oW_Resource.GetString("tTitlePdtName");
                olaPriceOld.Text = oW_Resource.GetString("tOldPrice");
                olaPriceNew.Text = oW_Resource.GetString("tNewPrice");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditSalePrice", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Close Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditSalePrice", "ocmBack_Click " + oEx.Message); }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                rcPrice = Convert.ToDecimal(otbNewPrice.Text);
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditSalePrice", "ocmAccept_Click " + oEx.Message); }
        }

        /// <summary>
        /// Show data after open Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wEditSalePrice_Shown(object sender, EventArgs e)
        {
            try
            {
                otbNewPrice.Focus();
                otbNewPrice.SelectionStart = 0;
                otbNewPrice.SelectAll();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEditSalePrice", "wEditSalePrice_Shown " + oEx.Message); }
        }
    }
}
