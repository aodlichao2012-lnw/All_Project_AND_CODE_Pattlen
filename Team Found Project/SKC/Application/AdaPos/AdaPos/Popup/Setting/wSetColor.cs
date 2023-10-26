using AdaPos.Class;
using AdaPos.Forms;
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

namespace AdaPos.Popup.Setting
{
    public partial class wSetColor : Form
    {
        
        private ResourceManager oW_Resource;
        private string tMsErrformat;
        private string tMsErrEmpty;
        public string rtCode{ get; set; }

        public wSetColor()
        {
            InitializeComponent();
            try
            {
                
                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetColor", "wRemark " + oEx.Message); }
        }

        private void W_SETxDesign()
        {
            //*Arm 62-09-19
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmClrDialog.BackColor = cVB.oVB_ColDark;
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetColor", "W_SETxDesign : " + oEx.Message); }
        }
        
        private void W_SETxText()
        {
            //*Arm 62-09-19
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
                             
                olaTitle.Text = oW_Resource.GetString("tSetColor");
                olaCode.Text = oW_Resource.GetString("tTitleSetColorCode");
                otbCode.Text = "";
                ocmClrDialog.Text = oW_Resource.GetString("tButtonSetColor");
                tMsErrformat = oW_Resource.GetString("tMsgSetColorFalse");
                tMsErrEmpty = oW_Resource.GetString("tMsgSetColorEmpty");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetColor", "W_SETxText : " + oEx.Message); }
        }
       
        private void ocmClrDialog_Click(object sender, EventArgs e)
        {
            //*Arm 62-09-19
            try
            {
                if (odgClrDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    opnExample.BackColor = odgClrDialog.Color;
                    string tCode = (odgClrDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6"); //แปลงค่า rgb เป็นเลขฐานสิบหก
                    otbCode.Text = "#" + tCode;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetColor", "ocmClrDialog_Click : " + oEx.Message); }
        }

        private void otbCode_KeyDown(object sender, KeyEventArgs e)
        {
            //*Arm 62-09-19
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (otbCode.Text != "")
                    {
                        try
                        {
                            opnExample.BackColor = ColorTranslator.FromHtml(otbCode.Text);//*Arm 62-09-19
                        }
                        catch (Exception oExp)
                        {
                            MessageBox.Show(tMsErrformat);
                        }
                    }
                    else
                    {
                        MessageBox.Show(tMsErrEmpty);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetColor", "otbCode_KeyDown : " + oEx.Message); }
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            //*Arm 62-09-19
            this.Close();
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            //*Arm 62-09-19
            try
            {
                if (otbCode.Text != "")
                {
                    this.rtCode = otbCode.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(tMsErrEmpty);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetColor", "ocmAccept_Click : " + oEx.Message); }
        }

        
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetColor", "ocmShwKb_Click : " + oEx.Message); }
            finally
            {
                otbCode.Focus();
            }
        }  
    }
}
