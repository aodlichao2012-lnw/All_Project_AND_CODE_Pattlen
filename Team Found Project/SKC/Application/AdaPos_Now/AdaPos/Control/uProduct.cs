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
using AdaPos.Forms;
using AdaPos.Popup.wSale;
using AdaPos.Models.Other;

namespace AdaPos.Control
{
    public partial class uProduct : UserControl
    {
        #region Variable


        #endregion End Variable

        #region Constructor

        public uProduct()
        {
            InitializeComponent();
        }

        public uProduct(Image poPdtImg, Color poColorBg, cmlPdtDetail poDetail)
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                
                opbPdt.Image = poPdtImg;
                olaPdtName.Text = poDetail.tPdtName;
                olaPdtPrice.Text = new cSP().SP_SETtDecShwSve(1, poDetail.cPdtPrice, cVB.nVB_DecShow);
                olaPdtName.Name = "Pdt-" + poDetail.tPdtCode;
                opbPdt.BackColor = poColorBg;
                olaUnit.Text = poDetail.tUnitName;
                olaBarcode.Text = poDetail.tBarcode;
                olaFactor.Text = poDetail.cUnitFactor.ToString();
                olaSaleType.Text = poDetail.tSaleType;  //*Em 62-09-23
                olaStaAlwDis.Text = poDetail.tStaAlwDis;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "uProduct : " + oEx.Message); }
            finally
            {
                poPdtImg = null;
                poDetail = null;
                //new cSP().SP_CLExMemory();
            }
        }

        #endregion End Constructor

        #region Event

        /// <summary>
        /// Choose Product / Show Detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uProduct_MouseDown(object sender, MouseEventArgs e)
        {
            cmlPdtOrder oOrder;
            oOrder = new cmlPdtOrder();
            try
            {
                //this.BackColor = cVB.oVB_ColNormal; //*Arm 62-10-15 - comment Code
                this.BorderStyle = BorderStyle.Fixed3D;

                switch (e.Button)
                {
                    case MouseButtons.Left:
                        oOrder = new cmlPdtOrder();
                        oOrder.cFactor = Convert.ToDecimal(olaFactor.Text);
                        oOrder.cSetPrice = Convert.ToDecimal(olaPdtPrice.Text);
                        oOrder.cQty = 1;
                        oOrder.tBarcode = olaBarcode.Text;
                        oOrder.tPdtCode = olaPdtName.Name.Substring(4);
                        oOrder.tPdtName = olaPdtName.Text;
                        oOrder.tUnit = olaUnit.Text;
                        oOrder.tStaPdt = "1";
                        oOrder.tStaAlwDis = olaStaAlwDis.Text;
                        //*Arm 62-09-23
                        if (olaSaleType.Text == "2")
                        {
                            wEditSalePrice owEditSalePrice = new wEditSalePrice(oOrder.tPdtCode, oOrder.tPdtName, oOrder.cSetPrice);
                            owEditSalePrice.ShowDialog();
                            if (owEditSalePrice.DialogResult == DialogResult.OK)
                            {
                                oOrder.cSetPrice = owEditSalePrice.rcPrice;
                            }
                        } //*Arm 62-09-23

                        cVB.oVB_Sale.W_ADDxPdtToOrder(oOrder);
                        break;

                    case MouseButtons.Right:
                        new wDetailPdt(olaPdtName.Name.Substring(4)).ShowDialog();
                        break;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "uProduct_MouseDown " + oEx.Message); }
            finally
            {
                oOrder = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Mouse up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //this.BackColor =  Color.Black; //*Arm 62-10-15 - comment Code
                this.BorderStyle = BorderStyle.None;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "uProduct_MouseUp " + oEx.Message); }
        }

        /// <summary>
        /// Mouse Hover
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uProduct_MouseHover(object sender, EventArgs e)
        {
            try
            {
                //this.BackColor = cVB.oVB_ColNormal;//*Arm 62-10-15 - comment Code
                this.BorderStyle = BorderStyle.Fixed3D;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "uProduct_MouseHover " + oEx.Message); }
        }

        /// <summary>
        /// Mouse Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uProduct_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                //this.BackColor = Color.Black; //*Arm 62-10-15 - comment Code
                this.BorderStyle = BorderStyle.None;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "uProduct_MouseLeave " + oEx.Message); }
        }

        #endregion End Event

        #region Function

        /// <summary>
        /// Get form sale
        /// </summary>
        /// <param name="poSale"></param>
        public void U_GETxFormSale(wSale poSale)
        {
            try
            {
                cVB.oVB_Sale = poSale;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "U_GETxFormSale " + oEx.Message); }
            finally
            {
                poSale = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                olaPdtPrice.BackColor = cVB.oVB_ColDark; //*Arm 62-10-15 - comment Code
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "W_SETxDesign " + oEx.Message); }
        }

        #endregion Function
    }
}
