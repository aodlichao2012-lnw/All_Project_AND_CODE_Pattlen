using AdaPos.Class;
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
    public partial class wReturnBillCode : Form
    {
        private ResourceManager oW_Resource;
        public string rtReturnBillCode { get; set; }

        public wReturnBillCode()
        {
            InitializeComponent();
            try
            {
                W_SETxDesign();
                W_SETxText();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wReturnBillCode", "W_SETxDesign : " + oEx.Message);
            }
        }
        private void W_SETxDesign()
        {
            try
            {
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReturnBillCode", "W_SETxDesign : " + oEx.Message); }
        }
        private void W_SETxText()
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
                olaTitleReturnbillcode.Text = oW_Resource.GetString("tTitleReturnBillCode");
                olaRtnBillCode.Text = oW_Resource.GetString("tTitleReturnBillCodeTel");
                
                if(!string.IsNullOrEmpty(cVB.tVB_CstTel))
                {
                    otbRtnBillCode.Text = cVB.tVB_CstTel;   // Defualt เบอร์โทรกรณีเลือกลูกค้า
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReturnBillCode", "W_SETxText : " + oEx.Message); }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(otbRtnBillCode.Text))
                {
                    rtReturnBillCode = otbRtnBillCode.Text;
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgReqReturnBillCode"), 3);
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wReturnBillCode", "ocmAccept_Click : " + oEx.Message);
            }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReturnBillCode", "ocmShwKb_Click : " + oEx.Message);
            }
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReturnBillCode", "ocmBack_Click : " + oEx.Message);
            }
        }

        private void wReturnBillCode_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wReturnBillCode", "wReturnBillCode_FormClosing : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        private void wReturnBillCode_Shown(object sender, EventArgs e)
        {
            try
            {
                otbRtnBillCode.Focus();
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wReturnBillCode", "wReturnBillCode_Shown : " + oEx.Message);
            }
        }
    }
}
