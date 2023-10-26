using AdaPos.Class;
using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Popup.wSale;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos
{
    public partial class wReason : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private string tW_RsgCode;
        private int nW_Time;

        #endregion End Variable

        public wReason(string ptRsgCode)
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
          
                oW_SP.SP_PRCxFlickering(this.Handle);

                tW_RsgCode = ptRsgCode;
                cVB.oVB_Reason = null;

                W_SETxDesign();
                W_SETxText();
                W_GETxReason();
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                this.KeyPreview = true; //*Arm 63-02-06 - (HotKey)
                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in opnMenu.Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "wReason : " + oEx.Message); }
            finally
            {
                ptRsgCode = null;
                oW_SP.SP_CLExMemory();
            }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wReason_Shown(object sender, EventArgs e)
        {
            Form oFormShow = null;
            try
            {
                ogdReason.ClearSelection();

                //*Em 63-05-12
                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wChooseItemRef);
                if (oFormShow != null) oFormShow.Hide();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "wReason_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wReason_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "wReason_FormClosing : " + oEx.Message); }
        }

        /// <summary>
        /// Get Reason
        /// </summary>
        private void W_GETxReason()
        {
            List<cmlTCNMRsn> aoRsn;

            try
            {
                aoRsn = new cReason().C_GETaReason(tW_RsgCode, "", 0);

                foreach (cmlTCNMRsn oRsn in aoRsn)
                {
                    ogdReason.Rows.Add(oRsn.FTRsnCode, oRsn.FTRsnName);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "W_GETxReason : " + oEx.Message); }
            finally
            {
                aoRsn = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;

                ocmSearch.BackColor = cVB.oVB_ColNormal;
                //ogdReason.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdReason); //*Net 63-03-03 Set Design Gridview
                ocmAccept.BackColor = cVB.oVB_ColNormal;
                otbTitlCode.HeaderText = cVB.oVB_GBResource.GetString("tCode");
                otbTitleName.HeaderText = cVB.oVB_GBResource.GetString("tName");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDeposit", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
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

                cVB.tVB_KbdScreen = "REASON";

                //*Em 62-09-09
                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                olaUsrName.Text = new cUser().C_GETtUsername();

                // Search Customer By
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tCode"));
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tName"));
                ocbSearchBy.SelectedIndex = 1; //*Net 63-03-25 Issue Baseline

                // Search : Match
                ocbSchMatch.Items.Add(cVB.oVB_GBResource.GetString("tPartField"));
                ocbSchMatch.Items.Add(cVB.oVB_GBResource.GetString("tWholeField"));
                ocbSchMatch.SelectedIndex = 0;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDeposit", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Choose Reaso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                cVB.oVB_Reason = null;
                cVB.oVB_Reason = new cmlTCNMRsn();
                cVB.oVB_Reason.FTRsnCode = ogdReason.Rows[ogdReason.CurrentRow.Index].Cells[0].Value.ToString();
                cVB.oVB_Reason.FTRsnName = ogdReason.Rows[ogdReason.CurrentRow.Index].Cells[1].Value.ToString();

                switch (tW_RsgCode)
                {
                    case "006": // Drawer
                        new cDrawer().C_OPNxCashDrawer();
                        break;
                    case "003": //ReturnSale    //*Em 63-05-12
                        cSale.C_PRCxSummary2HD();
                        new wPayment(3).ShowDialog(this);
                        break;
                }

                //otmClose.Start();
                this.Close();       //*Em 63-04-28
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 270;
                else
                    opnMenu.Width = 50;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmMenu_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Call Keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmKB_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wReason", "ocmKB_Click : " + ex.Message); }
        }

        /// <summary>
        /// Show function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDoShowKB();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmShwKb_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open Calculate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxCalculator();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmCalculate_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open popup help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmHelp_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxHelp();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmHelp_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open Popup about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAbout_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxAbout();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Timing to Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmClose_Tick(object sender, EventArgs e)
        {
            try
            {
                if (nW_Time == 5)
                    this.Close();

                nW_Time++;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<cmlTCNMRsn> aoRsn;
                ogdReason.Rows.Clear();
                ogdReason.Update();
                try
                {

                    aoRsn = new cReason().C_GETaReason(tW_RsgCode, otbSearchCst.Text, ocbSearchBy.SelectedIndex + 1, ocbSchMatch.SelectedIndex + 1);

                    foreach (cmlTCNMRsn oRsn in aoRsn)
                    {
                        ogdReason.Rows.Add(oRsn.FTRsnCode, oRsn.FTRsnName);
                    }
                }
                catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmSearch_Click : " + oEx.Message); }
                finally
                {
                    aoRsn = null;
                    oW_SP.SP_CLExMemory();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "ocmSearch_Click : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason:", "ocmNotify_Click : " + oEx.Message); }
        }

        private void ogdReason_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ocmAccept_Click(ocmAccept,new EventArgs());
        }

        private void otbSearchCst_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        ocmSearch_Click(ocmSearch, new EventArgs());
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog(this.Name, MethodBase.GetCurrentMethod().Name+" : " + oEx.Message); }
        }

        private void wReason_KeyDown(object sender, KeyEventArgs e)
        {
            //*Arm 63-02-06 - (HotKey) Created function wSyncData_KeyDown
            try
            {
                W_CALxByName(e);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason:", "ocmNotify_Click : " + oEx.Message); }

        }
        /// <summary>
        /// Call By Name
        /// </summary>
        private void W_CALxByName(KeyEventArgs poKey)
        {
            //*Arm 63-02-06 -(HotKey) Created function W_CALxByName 
            string tFuncName;

            try
            {
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(poKey);
                W_GETxFuncByFuncName(tFuncName);

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "W_CALxByName : " + oEx.Message); }
            finally
            {
                poKey = null;
                tFuncName = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get function in form 
        /// </summary>
        private void W_GETxFuncByFuncName(string ptFuncName)
        {
            //*Arm 63-02-06 -(HotKey) Created function W_GETxFuncByFuncName

            try
            {
                object oSender = new object();
                EventArgs oEv = new EventArgs();

                switch (ptFuncName)
                {
                    case "C_KBDxBack":
                        //try
                        //{
                        //    if (ogdOrder.Rows.Count == 0)
                        //    {
                        //        new wHome().Show();
                        //        otmClose.Start();
                        //    }
                        //    else
                        //        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantBack"), 3);
                        //}
                        //catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_GETxFuncByFuncName " + oEx.Message); }
                        break;

                    case "C_KBDxNotify":
                        //แจ้งเตือน
                        ocmNotify_Click(oSender, oEv);
                        break;

                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReason", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                oW_SP.SP_CLExMemory();
            }
        }
    }
}
