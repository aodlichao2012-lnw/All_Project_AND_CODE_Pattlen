using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Popup.All;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wHome
{
    public partial class wOpenShift : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;

        #endregion End Variable

        #region Constructor

        public wOpenShift()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();

                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "wOpenShift : " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "OnPaintBackground : " + oEx.Message); }
        }

        /// <summary>
        /// Close popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxBack();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Open shift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxOpenShift();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Check level > Change date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmChgSaleDate_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxChgSaleDate();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "ocmChgSaleDate_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Enter to Process open shift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbSaleDate_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;

            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        W_PRCxBack();
                        break;

                    default:    // Call by name
                        tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        W_GETxFuncByFuncName(tFuncName);
                        break;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "otbSaleDate_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Focus 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wOpenShift_Shown(object sender, EventArgs e)
        {
            try
            {
                ocmAccept.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "wOpenShift_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wOpenShift_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                cVB.tVB_KbdScreen = "HOME";
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "wOpenShift_FormClosing : " + oEx.Message); }
        }

        /// <summary>
        /// Show function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            DialogResult oResult;

            try
            {
                oResult = new cFunctionKeyboard().C_KBDoShowKB();

                if (oResult == DialogResult.OK)
                    W_GETxFuncByFuncName(cVB.tVB_KbdCallByName);

                cVB.tVB_KbdScreen = "OPENSHIFT";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "ocmShwKb_Click : " + oEx.Message); }
        }

        #endregion End Event

        #region Function / Method

        /// <summary>
        /// Set design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmChgSaleDate.BackColor = cVB.oVB_ColNormal;


                //*Arm 63-03-02
                otbPos.BackColor = cVB.oVB_ColLight;
                otbSaleDate.BackColor = cVB.oVB_ColLight;
                //++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text
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

                cVB.tVB_KbdScreen = "OPENSHIFT";

                otbSaleDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                olaTitleOpenShift.Text = oW_Resource.GetString("tOpenShift");
                olaTitleSaleDate.Text = oW_Resource.GetString("tSaleDate");
                olaTitlePos.Text = cVB.oVB_GBResource.GetString("tPos");
                otbPos.Text = cVB.tVB_PosCode;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Process change sale date
        /// </summary>
        private void W_PRCxChgSaleDate()
        {
            string tChkRole, tFuncCode;
            wAuthentication oSignIn = null; //Net 63-02-24 แสดงหน้ายืนยันสิทธิ์

            try
            {
                //*[AnUBiS][][2019-01-10] - ปรับใช้ code ตรวจสอบสิทธิ์แบบมีสถานะ return
                cVB.tVB_KbdCallByName = "C_KBDxChgSaleDate";
                tFuncCode = new cFunctionKeyboard().C_GETtFuncCode(cVB.tVB_KbdScreen);
                //tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen, tFuncCode);
                tChkRole = new cUser().C_CHKtUserRole(cVB.nVB_RolLevel, cVB.tVB_KbdScreen, tFuncCode);  //*Em 62-09-03

                switch (tChkRole)
                {
                    case "1":   // allowed.
                        W_OPNxChgSaleDate();
                        break;
                    case "0":   // not permission.
                    case "800": // data not found.
                        oSignIn = new wAuthentication(1, "OPENSHIFT"); //Net 63-02-24 แสดงหน้ายืนยันสิทธิ์
                        oSignIn.ShowDialog();

                        if (oSignIn.DialogResult == DialogResult.OK)
                            W_OPNxChgSaleDate();

                        break;
                    case "900":
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 2);
                        break;
                }

                /*
                cVB.tVB_KbdCallByName = "C_KBDxChgSaleDate";
                tChkRole = new cUser().C_CHKtUserRole(cVB.tVB_RolCode, cVB.tVB_KbdScreen);

                if (string.IsNullOrEmpty(tChkRole))
                {
                    oSignIn = new wSignin(1, "OPENSHIFT");
                    oSignIn.ShowDialog();

                    if (oSignIn.DialogResult == DialogResult.OK)
                        W_OPNxChgSaleDate();
                }
                else
                    W_OPNxChgSaleDate();
                */
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "W_PRCxChgSaleDate : " + oEx.Message); }
            finally
            {
                if (oSignIn != null)
                    oSignIn.Dispose();

                oSignIn = null;
                tChkRole = null;
                tFuncCode = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process open shift
        /// </summary>
        private void W_PRCxOpenShift()
        {
            bool bChkFormat;
            DateTime dDate;

            try
            {
                bChkFormat = oW_SP.SP_CHKbDateFormat(otbSaleDate.Text);

                if (bChkFormat)
                {
                    dDate = DateTime.ParseExact(otbSaleDate.Text, "dd/MM/yyyy", null);

                    if (dDate <= DateTime.Now.AddYears(1) && dDate >= DateTime.Now.AddYears(-1))
                    {
                        if (W_CHKnDateOpenShift() != 0) // Check ว่าเปิดรอบย้อนหรือไม่
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgDateOpenAgain"), 3);
                        else
                        {
                            cVB.tVB_SaleDate = dDate.ToString("yyyy-MM-dd");
                            new cShift().C_OPNxShift();
                            
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgShiftOpenSuccess"), 1);

                            this.Close();
                        }
                    }
                    else
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgLongTime"), 3);
                }
                else
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidDate"), 3);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "W_PRCxOpenShift : " + oEx.Message); }
            finally
            {
                otbSaleDate.Focus();
                //*Net 63-03-20 แก้ Error Null
                if(oW_SP!=null)
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Open change sale date
        /// </summary>
        private void W_OPNxChgSaleDate()
        {
            try
            {
                otbSaleDate.Clear();
                otbSaleDate.Enabled = true;
                otbSaleDate.BackColor = Color.White; //*Arm 63-03-02
                onpNumpad.Enabled = true;
                onpNumpad.ocmDot.Enabled = false;
                onpNumpad.oU_TextDateValue = otbSaleDate;
                onpNumpad.tU_TextValue = "";
                otbSaleDate.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "W_OPNxChgSaleDate : " + oEx.Message); }
        }

        /// <summary>
        /// Check วันที่เปิดรอบไม่ย้อนกลับ เชคในตาราง TPSTShiftHD
        /// </summary>
        private int W_CHKnDateOpenShift()
        {
            List<cmlTPSTShiftHD> aoShift = new List<cmlTPSTShiftHD>();
            StringBuilder oSql = new StringBuilder();
            int nDateOpen = 0;

            try
            {
                oSql.AppendLine("SELECT FTShfCode FROM TPSTShiftHD");
                //oSql.AppendLine("WHERE FDShdDSignIn > '" + Convert.ToDateTime(otbSaleDate.Text).ToString("yyyy-MM-dd") + "'");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDShdSignIn,121) > '" + Convert.ToDateTime(otbSaleDate.Text).ToString("yyyy-MM-dd") + "'"); //*Em 62-06-20

                aoShift = new cDatabase().C_GETaDataQuery<cmlTPSTShiftHD>(oSql.ToString());

                nDateOpen = aoShift.Count;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "W_CHKnDateOpenShift : " + oEx.Message); }
            finally
            {
                aoShift = null;
                oSql = null;
                oW_SP.SP_CLExMemory();
            }

            return nDateOpen;
        }

        /// <summary>
        /// Process back
        /// </summary>
        private void W_PRCxBack()
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "W_PRCxBack : " + oEx.Message); }
        }

        /// <summary>
        /// Get function by function name
        /// </summary>
        private void W_GETxFuncByFuncName(string ptFuncName)
        {
            try
            {

                switch (ptFuncName)
                {
                    case "C_KBDxChgSaleDate":
                        W_PRCxChgSaleDate();
                        break;

                    case "C_KBDxAccept":
                        W_PRCxOpenShift();
                        break;

                    case "C_KBDxBack":
                        this.Close();
                        break;

                    case "C_KBDxSaleDateInput":
                        otbSaleDate.Focus();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wOpenShift", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                oW_SP.SP_CLExMemory();
            }
        }
        
        #endregion End Function / Method
    }
}
