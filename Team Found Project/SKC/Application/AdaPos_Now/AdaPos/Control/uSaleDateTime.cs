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
    public partial class uSaleDateTime : UserControl
    {
        public uSaleDateTime()
        {
            InitializeComponent();
            olaSaleDate.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " +  DateTime.Now.ToString("HH:mm");
            otmSaleDate.Enabled = true;
            otmSaleDate.Start();
        }

        private void otmSaleDate_Tick(object sender, EventArgs e)
        {
           
                //new cSP().SP_CLExMemory();
                olaSaleDate.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm");
                        
        }
    }
}
