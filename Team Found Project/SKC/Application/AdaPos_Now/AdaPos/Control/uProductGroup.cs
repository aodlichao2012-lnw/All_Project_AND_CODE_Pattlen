using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdaPos.Models.Other;
using AdaPos.Class;
using AdaPos.Forms;
using AdaPos.Popup.wSale;
using AdaPos.Models.Database;

namespace AdaPos.Control
{
    public partial class uProductGroup : UserControl
    {
        #region Variable

        public string tW_PgpChain { get; set; }
       
        #endregion End Variable

        #region Constructor
        public uProductGroup()
        {
            InitializeComponent();
        }
        public uProductGroup(Image poPdtImg, Color poColorBg, string ptPgpChain, string ptPgpChainName)
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();

                opbPdt.Image = poPdtImg;
                olaPdtName.Text = ptPgpChainName;
                olaPdtName.Name = "ocm-" + ptPgpChain;
                opbPdt.BackColor = poColorBg;
                tW_PgpChain = ptPgpChain;
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProductGroup", "uProductGroup : " + oEx.Message); }
            finally
            {
                poPdtImg = null;
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
        private void uProductGroup_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                this.BorderStyle = BorderStyle.Fixed3D;

                cVB.oVB_Sale.nW_CurPage = 1;
                cVB.oVB_Sale.nW_StartRow = 0;
                cVB.oVB_Sale.nW_MaxPage = 0;
                //cVB.oVB_Sale.olaDisplayGrp.Text = olaPdtName.Text;  //*Arm 62-11-18  แสดงชื่อกลุ่มสินค้า ที่กำลังแสดงรายการ

                if (tW_PgpChain != "All")
                {
                    cVB.oVB_Sale.tW_PgpChain = tW_PgpChain;
                }
                else
                {
                    cVB.oVB_Sale.tW_PgpChain = "";
                }

                cVB.oVB_Sale.W_GETxPdt(2, "");


            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProductGroup", "uProduct_MouseDown " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Mouse up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uProductGroup_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                this.BorderStyle = BorderStyle.None;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProductGroup", "uProduct_MouseUp " + oEx.Message); }
        }

        /// <summary>
        /// Mouse Hover
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uProductGroup_MouseHover(object sender, EventArgs e)
        {
            try
            {
                this.BorderStyle = BorderStyle.Fixed3D;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProductGroup", "uProduct_MouseHover " + oEx.Message); }
        }

        /// <summary>
        /// Mouse Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uProductGroup_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                this.BorderStyle = BorderStyle.None;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProductGroup", "uProduct_MouseLeave " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("uProductGroup", "U_GETxFormSale " + oEx.Message); }
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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProductGroup", "W_SETxDesign " + oEx.Message); }
        }

        #endregion Function
    }
}
