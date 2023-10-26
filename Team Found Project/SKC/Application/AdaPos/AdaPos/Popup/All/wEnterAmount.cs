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
using AdaPos.Resources_String.Local;

namespace AdaPos.Popup.wSale
{
    public partial class wEnterAmount : Form
    {
        private ResourceManager oW_Resource;
        private int nW_Page;    //*Arm 62-12-19  1:wCloseShift, 2: wChooseItemRef
        public wEnterAmount(int pnPage = 1)
        {
            InitializeComponent();
            try
            {
                nW_Page = pnPage;//*Arm 62-12-19 เพิ่มรับค่า Parameter pnPage ระบุ Form ที่เรียกใช้เพื่อกำหนด Title 
                oucNumpad.oU_TextValue = otbAmount;
                oucNumpad.tU_TextValue = otbAmount.Text;
                W_SETxDesign();     //*Arm 62-10-21
                W_SETxText();       //*Arm 62-10-21
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wEnterAmount", "wEnterAmount " + oEx.Message);
            }
        }
        /// <summary>
        /// Set Design form
        /// </summary>
        private void W_SETxDesign() //*Arm 62-10-21 - แก้ไข Design ส่วนหัวตามสีธีม
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEnterAmount", "W_SETxDesign : " + oEx.Message); }
        }
        // <summary>
        /// Set tex form
        /// </summary>
        private void W_SETxText()   //*Arm 62-10-21
        {
            try
            {
                //*Arm 62-12-19 
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPopup_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPopup_EN));
                        break;
                }

                //*Arm 62-12-19 
                switch (nW_Page)
                {
                    case 1:     // wCloseShift
                        
                        break;

                    case 2:     //wChooseItemRef
                        olaTitleRemark.Text = oW_Resource.GetString("tTitleQty");
                        break;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEnterAmount", "W_SETxText : " + oEx.Message); }
        }
        /// <summary>
        /// Paint Background
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                using (SolidBrush oBrush = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(oBrush, e.ClipRectangle);
                    oBrush.Dispose();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wEnterAmount", "OnPaintBackground " + oEx.Message);
            }
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEnterAmount", "ocmBack_Click : " + oEx.Message); }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                //tW_Amount = otbAmount.Text;
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("ocmAccept_Click", "ocmBack_Click : " + oEx.Message);
            }
        }

        //*Net 63-03-04
        private void wEnterAmount_Shown(object sender, EventArgs e)
        {
            otbAmount.Focus();
        }
    }
}
