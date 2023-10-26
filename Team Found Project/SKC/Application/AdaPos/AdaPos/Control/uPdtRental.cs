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

namespace AdaPos.Control
{
    public partial class uPdtRental : UserControl
    {
        public uPdtRental()
        {
            InitializeComponent();
        }

        public uPdtRental(string ptPdtName, Image poPdtImg, decimal pcPdtPrice, string ptPdtCode)
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();

                opbPdt.Image = poPdtImg;
                olaPdtName.Text = ptPdtName;
                olaPdtPrice.Text = "฿" + new cSP().SP_SETtDecShwSve(1, pcPdtPrice, cVB.nVB_DecShow);
                olaPdtName.Name = "Pdt-" + ptPdtCode;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "uProduct " + oEx.Message); }
            finally
            {
                ptPdtName = null;
                poPdtImg = null;
                ptPdtCode = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                olaPdtPrice.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("uProduct", "W_SETxDesign " + oEx.Message); }
        }
    }
}
