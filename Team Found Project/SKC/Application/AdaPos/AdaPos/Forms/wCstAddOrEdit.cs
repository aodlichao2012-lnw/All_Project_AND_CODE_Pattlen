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

namespace AdaPos
{
    public partial class wCstAddOrEdit : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        #endregion End Variable

        #region Constructor

        public wCstAddOrEdit()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "wCstAddOrEdit : " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

        /// <summary>
        /// Show customer information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCstInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmCstInfo.BackColor == Color.Silver)
                {
                    ocmCstInfo.BackColor = cVB.oVB_ColDark;
                    olaLine1.BackColor = cVB.oVB_ColDark;

                    ocmAddress.BackColor = Color.Silver;
                    olaLine2.BackColor = Color.Silver;
                    ocmContact.BackColor = Color.Silver;
                    olaLine3.BackColor = Color.Silver;
                    ocmMemberCard.BackColor = Color.Silver;
                    olaLine4.BackColor = Color.Silver;
                    ocmCredit.BackColor = Color.Silver;
                    olaLine5.BackColor = Color.Silver;
                    ocmRFID.BackColor = Color.Silver;
                    olaLine6.BackColor = Color.Silver;

                    opnInformation.Visible = true;
                    opnContact.Visible = false;
                    oucCstAddress.Visible = false;
                    opnMemberCard.Visible = false;
                    opnCredit.Visible = false;
                    opnRFID.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmCstInfo_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Show Address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAddress_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmAddress.BackColor == Color.Silver)
                {
                    ocmAddress.BackColor = cVB.oVB_ColDark;
                    olaLine2.BackColor = cVB.oVB_ColDark;

                    ocmCstInfo.BackColor = Color.Silver;
                    olaLine1.BackColor = Color.Silver;
                    ocmContact.BackColor = Color.Silver;
                    olaLine3.BackColor = Color.Silver;
                    ocmMemberCard.BackColor = Color.Silver;
                    olaLine4.BackColor = Color.Silver;
                    ocmCredit.BackColor = Color.Silver;
                    olaLine5.BackColor = Color.Silver;
                    ocmRFID.BackColor = Color.Silver;
                    olaLine6.BackColor = Color.Silver;

                    oucCstAddress.Visible = true;
                    opnInformation.Visible = false;
                    opnContact.Visible = false;
                    opnMemberCard.Visible = false;
                    opnCredit.Visible = false;
                    opnRFID.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmAddress_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                new wCustomerM().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Contact
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmContact_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmContact.BackColor == Color.Silver)
                {
                    ocmContact.BackColor = cVB.oVB_ColDark;
                    olaLine3.BackColor = cVB.oVB_ColDark;

                    ocmAddress.BackColor = Color.Silver;
                    olaLine2.BackColor = Color.Silver;
                    ocmCstInfo.BackColor = Color.Silver;
                    olaLine1.BackColor = Color.Silver;
                    ocmMemberCard.BackColor = Color.Silver;
                    olaLine4.BackColor = Color.Silver;
                    ocmCredit.BackColor = Color.Silver;
                    olaLine5.BackColor = Color.Silver;
                    ocmRFID.BackColor = Color.Silver;
                    olaLine6.BackColor = Color.Silver;

                    opnContact.Visible = true;
                    opnInformation.Visible = false;
                    oucCstAddress.Visible = false;
                    opnMemberCard.Visible = false;
                    opnCredit.Visible = false;
                    opnRFID.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmContact_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Member Card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMemberCard_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmMemberCard.BackColor == Color.Silver)
                {
                    ocmMemberCard.BackColor = cVB.oVB_ColDark;
                    olaLine4.BackColor = cVB.oVB_ColDark;

                    ocmAddress.BackColor = Color.Silver;
                    olaLine2.BackColor = Color.Silver;
                    ocmCstInfo.BackColor = Color.Silver;
                    olaLine1.BackColor = Color.Silver;
                    ocmContact.BackColor = Color.Silver;
                    olaLine3.BackColor = Color.Silver;
                    ocmCredit.BackColor = Color.Silver;
                    olaLine5.BackColor = Color.Silver;
                    ocmRFID.BackColor = Color.Silver;
                    olaLine6.BackColor = Color.Silver;

                    opnMemberCard.Visible = true;
                    opnInformation.Visible = false;
                    oucCstAddress.Visible = false;
                    opnContact.Visible = false;
                    opnCredit.Visible = false;
                    opnRFID.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmMemberCard_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Save & Open Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAcceptCst_Click(object sender, EventArgs e)
        {
            try
            {
                ocmAddress.Visible = true;
                ocmContact.Visible = true;
                ocmMemberCard.Visible = true;
                ocmCredit.Visible = true;
                otbInfoCode.Enabled = false;
                otbInfoCode.ReadOnly = true;
                ocmGenInfoCode.Enabled = false;
                opnDetailCst.Visible = true;
                ocmRFID.Visible = true;
                ocmGenInfoCode.BackColor = Color.LightGray;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmAcceptCst_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Credit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCredit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmCredit.BackColor == Color.Silver)
                {
                    ocmCredit.BackColor = cVB.oVB_ColDark;
                    olaLine5.BackColor = cVB.oVB_ColDark;

                    ocmCstInfo.BackColor = Color.Silver;
                    olaLine1.BackColor = Color.Silver;
                    ocmAddress.BackColor = Color.Silver;
                    olaLine2.BackColor = Color.Silver;
                    ocmContact.BackColor = Color.Silver;
                    olaLine3.BackColor = Color.Silver;
                    ocmMemberCard.BackColor = Color.Silver;
                    olaLine4.BackColor = Color.Silver;
                    ocmRFID.BackColor = Color.Silver;
                    olaLine6.BackColor = Color.Silver;

                    opnCredit.Visible = true;
                    opnInformation.Visible = false;
                    oucCstAddress.Visible = false;
                    opnContact.Visible = false;
                    opnMemberCard.Visible = false;
                    opnRFID.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmCredit_Click : " + oEx.Message); }
        }

        /// <summary>
        /// RFID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmRFID_Click(object sender, EventArgs e)
        {
            try
            {
                if (ocmRFID.BackColor == Color.Silver)
                {
                    ocmRFID.BackColor = cVB.oVB_ColDark;
                    olaLine6.BackColor = cVB.oVB_ColDark;

                    ocmAddress.BackColor = Color.Silver;
                    olaLine2.BackColor = Color.Silver;
                    ocmCstInfo.BackColor = Color.Silver;
                    olaLine1.BackColor = Color.Silver;
                    ocmContact.BackColor = Color.Silver;
                    olaLine3.BackColor = Color.Silver;
                    ocmMemberCard.BackColor = Color.Silver;
                    olaLine4.BackColor = Color.Silver;
                    ocmCredit.BackColor = Color.Silver;
                    olaLine5.BackColor = Color.Silver;

                    opnRFID.Visible = true;
                    opnInformation.Visible = false;
                    oucCstAddress.Visible = false;
                    opnContact.Visible = false;
                    opnMemberCard.Visible = false;
                    opnCredit.Visible = false;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmRFID_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmAbout_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmMenu_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCstAddOrEdit_Shown(object sender, EventArgs e)
        {
            try
            {
                opnContentAddEdit2.ColumnStyles[0].Width = ocmCstInfo.Width;
                otbInfoCode.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "wCstAddOrEdit_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Change, Clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmCamera_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile;
            Bitmap oBitmap;

            try
            {
                if (string.Equals(opbCstAddEdit.Tag, "Customer"))
                {
                    oFile = new OpenFileDialog();
                    oFile.Filter = "Image Files(*.png)|*.png";

                    if (oFile.ShowDialog() == DialogResult.OK)
                    {
                        oBitmap = new Bitmap(oFile.FileName);
                        opbCstAddEdit.Image = oBitmap;
                        opbCstAddEdit.Tag = null;
                        ocmCamera.Image = Properties.Resources.ClearR_32;
                    }
                }
                else
                {
                    opbCstAddEdit.Image = Properties.Resources.Product_256;
                    opbCstAddEdit.Tag = "Customer";
                    ocmCamera.Image = Properties.Resources.CameraB_32;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "ocmCamera_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCstAddOrEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "wCstAddOrEdit_FormClosing : " + oEx.Message); }
        }

        #endregion End Event

        #region Function / Method

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                ocmCstInfo.BackColor = cVB.oVB_ColDark;
                olaLine1.BackColor = cVB.oVB_ColDark;

                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;

                ocmGenInfoCode.BackColor = cVB.oVB_ColNormal;
                ocmSchInfoLevel.BackColor = cVB.oVB_ColNormal;
                ocmSchInfoType.BackColor = cVB.oVB_ColNormal;
                ocmSchInfoGrp.BackColor = cVB.oVB_ColNormal;
                ocmSchInfoPriGrp.BackColor = cVB.oVB_ColNormal;
                ocmSchInfoPmtGrp.BackColor = cVB.oVB_ColNormal;
                ocmSchInfoCareer.BackColor = cVB.oVB_ColNormal;
                ocmSchInfoSpn.BackColor = cVB.oVB_ColNormal;
                ocmSchInfoUser.BackColor = cVB.oVB_ColNormal;
                ocmLanguage.BackColor = cVB.oVB_ColNormal;
                ocmAcceptCst.BackColor = cVB.oVB_ColNormal;
                ocmCntDelete.BackColor = cVB.oVB_ColNormal;
                ocmCntAdd.BackColor = cVB.oVB_ColNormal;
                ocmSchCrdIssueBy.BackColor = cVB.oVB_ColNormal;
                //ogdContact.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdContact); //*Net 63-03-03 Set Design Gridview
                ocmSchCrShipBy.BackColor = cVB.oVB_ColNormal;
                ocmRFIDDelete.BackColor = cVB.oVB_ColNormal;
                ocmRFIDAdd.BackColor = cVB.oVB_ColNormal;
                //ogdRFID.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdRFID); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resCustomer_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resCustomer_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "CUSTOMER";

                // Menu
                ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");
                olaCst.Text = oW_Resource.GetString("tCst");

                // user
                olaUsrName.Text = new cUser().C_GETtUsername();

                // Menu Cst
                ocmCstInfo.Text = oW_Resource.GetString("tInformation");
                ocmAddress.Text = oW_Resource.GetString("tAddress");
                ocmContact.Text = oW_Resource.GetString("tContact");
                ocmMemberCard.Text = oW_Resource.GetString("tMemberCard");
                ocmCredit.Text = oW_Resource.GetString("tCredit");
                ocmRFID.Text = oW_Resource.GetString("tRFID");

                // Cst Detail
                olaTitleDetCode.Text = oW_Resource.GetString("tCode");
                olaTitleDetName.Text = oW_Resource.GetString("tName");
                olaTitleDetEmail.Text = oW_Resource.GetString("tEmail");
                olaTitleDetTel.Text = oW_Resource.GetString("tTelephone");
                olaTitleDetBusType.Text = oW_Resource.GetString("tBusinessType");
                olaTitleDetEstab.Text = oW_Resource.GetString("tEstab");
                olaTitleDetBch.Text = oW_Resource.GetString("tBranch");

                // Information
                olaTitleInfoCode.Text = oW_Resource.GetString("tCode");
                olaTitleInfoName.Text = oW_Resource.GetString("tName");
                olaTitleInfoGrp.Text = oW_Resource.GetString("tGroup");
                olaTitleInfoNameOth.Text = oW_Resource.GetString("tNameOther");
                olaTitleInfoType.Text = oW_Resource.GetString("tType");
                olaTitleInfoCrdID.Text = oW_Resource.GetString("tIDCard");
                olaTitleInfoPmtGrp.Text = oW_Resource.GetString("tPmtGrp");
                olaTitleInfoEmail.Text = oW_Resource.GetString("tEmail");
                olaTitleInfoLevel.Text = oW_Resource.GetString("tLevel");
                olaTitleInfoTel.Text = oW_Resource.GetString("tTelephone");
                olaTitleInfoCstSex.Text = oW_Resource.GetString("tSex");
                olaTitleInfoPriGrp.Text = oW_Resource.GetString("tPriGrp");
                olaTitleInfoFax.Text = oW_Resource.GetString("tFax");
                olaTitleInfoDOB.Text = oW_Resource.GetString("tDOB");
                olaTitleInfoCareer.Text = oW_Resource.GetString("tCareer");
                olaTitleInfoBus.Text = oW_Resource.GetString("tBusinessType");
                olaTitleInfoEstab.Text = oW_Resource.GetString("tEstab");
                olaTitleInfoSpn.Text = oW_Resource.GetString("tSalePerson");
                olaTitleInfoBchNo.Text = oW_Resource.GetString("tBranch");
                olaTitleInfoStaContact.Text = oW_Resource.GetString("tStaContact");
                olaTitleInfoUsr.Text = oW_Resource.GetString("tUser");
                olaTitleInfoDisRet.Text = oW_Resource.GetString("tDisRet");
                olaTitleInfoDisWhols.Text = oW_Resource.GetString("tDisWhs");
                olaTitleInfoRemark.Text = oW_Resource.GetString("tRemark");

                // Sex
                ocbInfoSex.Items.Add(oW_Resource.GetString("tMen"));
                ocbInfoSex.Items.Add(oW_Resource.GetString("tFemale"));
                ocbInfoSex.SelectedIndex = 0;

                // Business Type
                ocbInfoBus.Items.Add(oW_Resource.GetString("tIndividual"));
                ocbInfoBus.Items.Add(oW_Resource.GetString("tJuristic"));
                ocbInfoBus.SelectedIndex = 0;

                // Establishment
                ocbInfoEstab.Items.Add(oW_Resource.GetString("tHQ"));
                ocbInfoEstab.Items.Add(oW_Resource.GetString("tBranch"));
                ocbInfoEstab.SelectedIndex = 0;

                // Contact Status
                ocbInfoStaContact.Items.Add(oW_Resource.GetString("tActive"));
                ocbInfoStaContact.Items.Add(oW_Resource.GetString("tDeActive"));
                ocbInfoStaContact.SelectedIndex = 0;

                // Contact
                otbCntName.HeaderText = oW_Resource.GetString("tName");
                otbCntTel.HeaderText = oW_Resource.GetString("tTelephone");
                otbCntEmail.HeaderText = oW_Resource.GetString("tEmail");
                olaCntAllPage.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), "0", "0", "0");

                // Card
                olaTitleCrdNo.Text = oW_Resource.GetString("tCardNo");
                olaTitleCrdReg.Text = oW_Resource.GetString("tCardRegis");
                olaTitleCrdIssue.Text = oW_Resource.GetString("tCardIssue");
                olaTitleCrdExp.Text = oW_Resource.GetString("tCardExp");
                olaTitleCrdIssueBy.Text = oW_Resource.GetString("tIssuedBy");
                olaTitleCrdStaActive.Text = oW_Resource.GetString("tStaActive");

                // Credit
                olaTitleCrTerm.Text = oW_Resource.GetString("tTerm");
                olaTitleCrLimit.Text = oW_Resource.GetString("tLimit");
                olaTitleCrPayCdt.Text = oW_Resource.GetString("tPayCdt");
                olaTitleCrStatementCdt.Text = oW_Resource.GetString("tStatementCdt");
                olaTitleCrShippingCdt.Text = oW_Resource.GetString("tShippingCdt");
                olaTitleCrLstRcv.Text = oW_Resource.GetString("tLstRcv");
                olaTitleCrLstContact.Text = oW_Resource.GetString("tLstContact");
                olaTitleCrShipBy.Text = oW_Resource.GetString("tShipBy");
                olaTitleCrLeadTime.Text = oW_Resource.GetString("tLeadTime");
                olaTitleCrPay.Text = oW_Resource.GetString("tPayment");
                olaTitleCrStaApv.Text = oW_Resource.GetString("tStaApprove");
                olaTitleCrContactDay.Text = oW_Resource.GetString("tContactDay");
                ockMonday.Text = oW_Resource.GetString("tMonday");
                ockTuesday.Text = oW_Resource.GetString("tTuesday");
                ockWednesday.Text = oW_Resource.GetString("tWednesday");
                ockThursday.Text = oW_Resource.GetString("tThursday");
                ockFriday.Text = oW_Resource.GetString("tFriday");
                ockSaturday.Text = oW_Resource.GetString("tSaturday");
                ockSunday.Text = oW_Resource.GetString("tSunday");

                // Payment
                ocbCrPay.Items.Add(oW_Resource.GetString("tSource"));
                ocbCrPay.Items.Add(oW_Resource.GetString("tDestination"));
                ocbCrPay.SelectedIndex = 0;

                // Approve
                ocbCrStaApv.Items.Add(oW_Resource.GetString("tApprove"));
                ocbCrStaApv.Items.Add(oW_Resource.GetString("tDisapprove"));
                ocbCrStaApv.SelectedIndex = 0;

                // RFID
                otbRFIDCode.HeaderText = oW_Resource.GetString("tCode");
                otbRFIDName.HeaderText = oW_Resource.GetString("tName");
                olaTitleRFIDPage.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), "0", "0", "0");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCstAddOrEdit", "W_SETxText : " + oEx.Message); }
        }

        #endregion End Function / Method
    }
}
