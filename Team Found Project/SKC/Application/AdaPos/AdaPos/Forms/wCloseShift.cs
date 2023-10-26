using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Popup.Shift;
using AdaPos.Popup.wSale;
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
    public partial class wCloseShift : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;
        private List<cmlTCNMUser> aoW_User;
        private List<cmlTCNMAppModule> aoW_AppModule;   //*Em 62-06-07
        public wBanknote oBankNote = new wBanknote();
        #endregion End Variable

        #region Constructor

        public wCloseShift()
        {
            InitializeComponent();

            try
            {
                cVB.tVB_KbdScreen = "CLOSESHIFT";
                oW_SP = new cSP();
                oW_SP.SP_PRCxFlickering(this.Handle);
                aoW_AppModule = oW_SP.SP_GEToAppSignType(); //*Em 62-06-07

                W_SETxDesign();
                W_SETxText();
                W_SHWxButtonBar();
                W_GETxUser();
                W_GETxShwDataShift();
                W_SETxGridDefualt();
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "wCloseShift : " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                new wHome().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ocmBack_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ocmMenu_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wCloseShift", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ocmAbout_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCloseShift_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "wCloseShift_FormClosing : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }

        private void ocmMenuBar_Click(object sender, EventArgs e)
        {
            string tFuncName;
            try
            {
                Button ocmMenu;
                ocmMenu = (Button)sender;
                tFuncName = ocmMenu.Tag.ToString();
                switch (tFuncName)
                {
                    case "C_KBDxBack":
                        try
                        {
                            ocmBack_Click(null, null);
                        }
                        catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "ocmMenuBar_Click " + oEx.Message); }
                        break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch
            { }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift:", "ocmNotify_Click : " + oEx.Message); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbPin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        //W_CHKxPincode();  //*Net 63-04-02
                        ocmNext_Click(ocmNext, new EventArgs());
                        break;

                    case Keys.Escape:
                        //if (ocmBack.Visible)
                        //    otmClose.Start();
                        break;

                    default:
                        W_CALxByName(e);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSignin", "otbPin_KeyDown : " + oEx.Message); }
        }

        private void ocmNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (otbPin.Enabled)
                {
                    W_CHKxPincode();
                    if (otbPin.Enabled) return;
                }
                W_GETxPaymentType();
                cSale.C_OPNxCashDrawer(); //*Em 62-10-02
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ocmNext_Click : " + oEx.Message); }
        }

        private void ogdBanknote_CellBeginEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ogdBanknote_CellBeginEdit : " + oEx.Message); }
        }

        private void olaMode1_Click(object sender, EventArgs e)
        {
            olaMode1.Cursor = Cursors.No;
            olaMode1.ForeColor = Color.Black;
            olaMode2.Cursor = Cursors.Hand;
            olaMode2.ForeColor = Color.Gray;
        }

        private void olaMode2_Click(object sender, EventArgs e)
        {
            olaMode1.Cursor = Cursors.Hand;
            olaMode1.ForeColor = Color.Gray;
            olaMode2.Cursor = Cursors.No;
            olaMode2.ForeColor = Color.Black;
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            cShift oShift = new cShift();

            try
            {
                // C_CHKnShiftofSale() | ถ้าเป็น 1 = มียอดขาย | 2 = ไม่มียอดขาย
                if (cVB.bVB_PrnShiftRCVRef)
                {
                    if (oShift.C_CHKnShiftofSale() == 1)
                    {
                        if (otbTotal.Text == "0.00")
                        {
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgShiftCheck"), 4);
                        }
                        else
                        {
                            W_PRCxShiftPrint();
                        }
                    }
                    else
                    {
                        W_PRCxShiftPrint();
                    }
                }
                else
                {
                    W_PRCxShiftPrint();
                }
                
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "ocmAccept_Click : " + oEx.Message); }
        }
        #endregion End Event

        #region Function / Method
        /// <summary>
        /// ย้ายคำสั่งจาก ocmAccept_Click มาเป็น Function เรียกใช้
        /// 2020-02-19 by Zen
        /// </summary>
        private void W_PRCxShiftPrint()
        {
            cShift oShift = new cShift();
            if (otbPin.Enabled == false)
            {
                //update SignOut
                oShift.C_UPDxClosShift(false);
                W_SAVxShiftRCV();
                //print receviec slip
                if (cVB.bVB_PrnShiftRCV) cShift.C_PRCxPrintShiftRCV();
                oShift.C_INSxShiftSumRcv();
                oShift.C_INSxLastDoc();
                oShift.C_INSxCurrency();
                oShift.C_INSxPdtSpc();

                //banknote 
                //*Zen แก้ให้เอา BankNote ไปใส่ในช่องเงินสด
                W_INSxTPSTShiftSKeyBN(oBankNote.ogdBankNote);
                cShift.C_PRCxPrintShiftBN();    //*Em 62-08-14                
                
                //if (cVB.bVB_PrnShiftBNK)
                //{
                //    //new wBanknote().ShowDialog();
                //    wBanknote oBankNoteStep2 = new wBanknote();
                //    oBankNoteStep2.ShowDialog();
                //    if (oBankNoteStep2.DialogResult == DialogResult.OK)
                //    {
                //        cShift oShiftBN = new cShift();
                //        if (cVB.bVB_PrnShiftRCVRef)
                //        {
                //            if (oShiftBN.C_CHKnShiftofSale() == 1)
                //            {
                //                if (new wBanknote().olaResult.Text == "0.00")
                //                {
                //                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgShiftCheck"), 4);
                //                }
                //                else
                //                {
                //                    W_INSxTPSTShiftSKeyBN(oBankNoteStep2.ogdBankNote);
                //                    cShift.C_PRCxPrintShiftBN();    //*Em 62-08-14
                //                    this.Close();
                //                    this.Dispose();
                //                }

                //            }
                //            else
                //            {
                //                W_INSxTPSTShiftSKeyBN(oBankNoteStep2.ogdBankNote);
                //                cShift.C_PRCxPrintShiftBN();    //*Em 62-08-14
                //                this.Close();
                //                this.Dispose();
                //            }
                //        }
                //        else
                //        {
                //            W_INSxTPSTShiftSKeyBN(oBankNoteStep2.ogdBankNote);
                //            cShift.C_PRCxPrintShiftBN();    //*Em 62-08-14
                //            this.Close();
                //            this.Dispose();
                //        }
                //    }

                //}

                //summary
                if (cVB.bVB_PrnShiftSUM) new wShiftSum().ShowDialog();
                new cSyncData().C_PRCxSyncUld();    //*Em 62-12-18
                cVB.tVB_ShfCode = null;
                new wHome().Show();
                otmClose.Start();
            }
        }

        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT;  //*Em 62-01-25  Waterpark
            List<cmlTPSMFunc> aoMenuB;  //*Em 62-01-25  Waterpark
            int nItem;  //*Em 62-01-25  Waterpark
            try
            {
                aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                //*Em 62-01-25  Waterpark
                aoMenuT = (from oBar in aoKb where oBar.FNGdtPage == 1 orderby oBar.FNGdtUsrSeq select oBar).ToList();
                aoMenuB = (from oBar in aoKb where oBar.FNGdtPage == 2 orderby oBar.FNGdtUsrSeq select oBar).ToList();

                if (aoMenuT.Count > 0)
                {
                    opnMenuT.Controls.Clear();
                    opnMenuT.RowCount = aoMenuT.Count + 1;
                    nItem = 0;
                    foreach (cmlTPSMFunc oMenu in aoMenuT)
                    {
                        ocmMenu = new Button();
                        ocmMenu.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                        ocmMenu.FlatStyle = FlatStyle.Flat;
                        ocmMenu.FlatAppearance.BorderSize = 0;
                        ocmMenu.Font = new Font("Segoe UI Semibold", 10F);
                        ocmMenu.ForeColor = Color.White;
                        ocmMenu.Text = "".PadLeft(5) + oMenu.FTGdtName;
                        ocmMenu.TextAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.Name = oMenu.FTGdtCallByName;
                        ocmMenu.BackColor = cVB.oVB_ColDark;
                        ocmMenu.Enabled = true;

                        try
                        {
                            ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        }
                        catch { }
                        ocmMenu.TextImageRelation = TextImageRelation.ImageBeforeText;

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 260;
                        opnMenuT.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuT.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuT.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }

                if (aoMenuB.Count > 0)
                {
                    opnMenuB.Controls.Clear();
                    opnMenuB.RowCount = aoMenuB.Count + 1;
                    nItem = 1;

                    Panel oPanel;
                    oPanel = new Panel();
                    oPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                    oPanel.Height = opnMenuB.Height - ((opnMenuB.RowCount - 1) * 55);
                    opnMenuB.Controls.Add(oPanel);

                    opnMenuB.RowStyles[0].SizeType = SizeType.AutoSize;
                    foreach (cmlTPSMFunc oMenu in aoMenuB)
                    {
                        ocmMenu = new Button();
                        ocmMenu.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                        //ocmMenu.Margin = new Padding(2);
                        ocmMenu.FlatStyle = FlatStyle.Flat;
                        ocmMenu.FlatAppearance.BorderSize = 0;
                        ocmMenu.Font = new Font("Segoe UI Semibold", 10F);
                        ocmMenu.ForeColor = Color.White;
                        ocmMenu.Text = "".PadLeft(5) + oMenu.FTGdtName;
                        ocmMenu.TextAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.Name = oMenu.FTGdtCallByName;
                        ocmMenu.TextImageRelation = TextImageRelation.ImageBeforeText;
                        ocmMenu.BackColor = cVB.oVB_ColDark;
                        ocmMenu.Enabled = true;
                        try
                        {
                            ocmMenu.Image = ((Image)(Properties.Resources.ResourceManager.GetObject(oMenu.FTGdtCallByName)));
                        }
                        catch { }

                        ocmMenu.ImageAlign = ContentAlignment.MiddleLeft;
                        ocmMenu.BackgroundImageLayout = ImageLayout.Zoom;
                        ocmMenu.Click += ocmMenuBar_Click;
                        ocmMenu.Tag = oMenu.FTGdtCallByName;
                        ocmMenu.Height = 50;
                        ocmMenu.Width = 260;
                        opnMenuB.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuB.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuB.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }
                //++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
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
                ocmNext.BackColor = cVB.oVB_ColNormal;
                //ogdPaymentType.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdPaymentType); //*Net 63-03-03 Set Design Gridview

                ocmAccept.BackColor = cVB.oVB_ColNormal;

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark; //*Arm 62-10-15
                opnMenuB.BackColor = cVB.oVB_ColDark; //*Arm 62-10-15
                //ocmKB.BackColor = cVB.oVB_ColDark;
                //ocmCalculate.BackColor = cVB.oVB_ColDark;
                //ocmShwKb.BackColor = cVB.oVB_ColDark;
                //ocmHelp.BackColor = cVB.oVB_ColDark;
                //ocmAbout.BackColor = cVB.oVB_ColDark;
                //ocmBack.BackColor = cVB.oVB_ColDark;
                otbTotal.BackColor = cVB.oVB_ColLight; //*Arm 62-10-15
                oucNumpad.oU_TextValue = otbPin;
                ocmBanknote.FlatStyle = FlatStyle.Popup;
                ocmBanknote.DefaultCellStyle.BackColor = cVB.oVB_ColNormal;
                ocmBanknote.DefaultCellStyle.ForeColor = Color.White;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                int nItem = 0;
                string tCap = "";
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resCloseShift_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resCloseShift_EN));
                        break;
                }

                //*Em 62-09-09
                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;
                // Menu
                //ocmBack.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tBack");
                //ocmHelp.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tHelp");
                //ocmAbout.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tAbout");
                //ocmKB.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tKeyboard");
                //ocmCalculate.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tCalculate");
                //ocmShwKb.Text = "".PadLeft(10) + cVB.oVB_GBResource.GetString("tShowKb");

                olaCloseShift.Text = oW_Resource.GetString("tCloseShift");
                olaTitleUsername.Text = cVB.oVB_GBResource.GetString("tUsername");
                olaTitlePin.Text = cVB.oVB_GBResource.GetString("tPassword");
                otbPayType.HeaderText = oW_Resource.GetString("tPayType");
                otbAmtPay.HeaderText = cVB.oVB_GBResource.GetString("tAmount");

                //*Em 62-06-19
                olaTitleShiftDetail.Text = oW_Resource.GetString("tTitleShiftDetail");
                olaTitleLastUsr.Text = oW_Resource.GetString("tTitleLastUsr");
                olaTitleShiftCode.Text = oW_Resource.GetString("tShiftCode");
                olaTitleSignIn.Text = oW_Resource.GetString("tTitleSignIn");
                olaTitleSignOut.Text = oW_Resource.GetString("tTitleSignOut");
                olaTitleTotal.Text = oW_Resource.GetString("tTitleTotal");
                //+++++++++++++++++

                if (aoW_AppModule.Count > 0)
                {
                    nItem = 0;
                    Font oFont = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
                    foreach (cmlTCNMAppModule oAppModule in aoW_AppModule)
                    {
                        switch (oAppModule.FTAppSignType)
                        {
                            case "1": tCap = oW_Resource.GetString("tMode1"); break;
                            case "2": tCap = oW_Resource.GetString("tMode2"); break;
                            case "3": tCap = oW_Resource.GetString("tMode3"); break;
                        }

                        switch (nItem)
                        {
                            case 0:
                                olaMode1.Text = tCap;
                                olaMode1.Tag = oAppModule.FTAppSignType;
                                olaMode1.Font = oFont;
                                olaMode1.ForeColor = Color.Black;
                                olaMode1.Cursor = Cursors.No;
                                break;
                            case 1:
                                olaMode2.Text = tCap;
                                olaMode2.Tag = oAppModule.FTAppSignType;
                                olaMode2.Font = oFont;
                                olaMode2.ForeColor = Color.Gray;
                                olaMode2.Cursor = Cursors.Hand;

                                break;
                            default:
                                break;
                        }
                        nItem++;
                    }
                }
                // user
                olaUsrName.Text = new cUser().C_GETtUsername();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_SETxText : " + oEx.Message); }
        }

        private void W_GETxShwDataShift()
        {
            try
            {
                ocbUser.Text = cVB.tVB_UsrName;
                //olaInfoUsrName.Text = cVB.tVB_UsrName;
                //olaShiftCode.Text = cVB.tVB_ShfCode;
                //olaInfoSaleDate.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");

                //*Em 62-06-19
                olaLastUsr.Text = cVB.tVB_UsrName;
                olaShiftCode.Text = cVB.tVB_ShfCode;
                olaSignIn.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");
                olaSignOut.Text = "-";
                //++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift:", "W_GETxShwDataShift : " + oEx.Message); }
        }

        /// <summary>
        /// Check pin code
        /// </summary>
        private void W_CHKxPincode()
        {
            string tPwdEnc, tChkRole, tFuncCode;
            cmlTCNMUser oUser;
            int nChkClear = 0;

            try
            {
                if (string.IsNullOrEmpty(ocbUser.Text) || string.IsNullOrEmpty(otbPin.Text))
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputUsrPwd"), 3);
                else
                {
                    oUser = (from User in aoW_User where User.FTUsrName == ocbUser.Text select User).FirstOrDefault();

                    if (oUser != null)
                    {
                        tPwdEnc = new cEncryptDecrypt("2").C_CALtEncrypt(otbPin.Text);

                        if (string.Equals(tPwdEnc, oUser.FTUsrPwd))
                        {
                            ocbUser.Enabled = false;
                            otbPin.Enabled = false;
                            ocmNext.Focus();
                            nChkClear = 1;
                        }
                        else
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsr"), 3);
                    }
                    else
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInvalidUsrname"), 3);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_CHKxPincode : " + oEx.Message); }
            finally
            {
                tPwdEnc = null;
                tChkRole = null;
                oUser = null;

                if (nChkClear == 0)
                {
                    otbPin.Clear();
                    ocbUser.Focus();
                }

                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Call By Name
        /// </summary>
        private void W_CALxByName(KeyEventArgs poKey)
        {
            string tFuncName;

            try
            {
                // Call by name
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(poKey);
                new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                W_GETxFuncByFuncName(tFuncName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_CALxByName : " + oEx.Message); }
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
            try
            {
                switch (ptFuncName)
                {

                    case "C_KBDxBack":
                        otmClose.Start();
                        break;

                    case "C_KBDxChooseUser":
                        ocbUser.Focus();
                        ocbUser.DroppedDown = true;
                        break;

                    case "C_KBDxInputPassword":
                        ocbUser.DroppedDown = false;
                        otbPin.Focus();
                        break;

                    case "C_KBDxNotify":
                        break;

                    case "C_KBDxNext":
                        W_CHKxPincode();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="pnMode">
        /// Show form mode.
        /// <para>0: show in mode signin.</para>
        /// <para>1: show in mode override permission (role).</para>
        /// </param>
        private void W_GETxUser()
        {
            try
            {
                aoW_User = new cUser().C_GETaUser();

                //// ดึงข้อมูลใส่ Combobox
                //ocbUser.DisplayMember = "FTUsrName";
                //ocbUser.ValueMember = "FTUsrCode";
                //ocbUser.DataSource = aoW_User;
                ocbUser.Text = cVB.tVB_UsrName; //*Em 62-09-10
                ocbUser.Enabled = false;

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_GETxUser : " + oEx.Message); }
            finally
            {
                oW_SP.SP_CLExMemory();
            }
        }

        private void W_GETxPaymentType()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tApp = (cVB.tVB_PosType == "1" ? "PS" : "FC");
            try
            {
                ogdPaymentType.Rows.Clear();
                oSql.AppendLine("SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY RCV.FTRcvCode) AS FNRcvSeqNo, (CASE WHEN ISNULL(RCVL.FTRcvName,'') = '' THEN (SELECT TOP 1 FTRcvName FROM TFNMRcv_L WITH(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode) ELSE RCVL.FTRcvName END) AS FTRcvName,");
                oSql.AppendLine("0 AS FCRcvAmt, RCV.FTRcvCode, RCV.FTFmtCode");
                oSql.AppendLine("FROM TFNMRcv RCV WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode =  RCVL.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);
                //oSql.AppendLine("INNER JOIN TSysRcvApp APP WITH(NOLOCK) ON RCV.FTFmtCode = APP.FTFmtCode AND FTAppCode = '" + tApp + "'");
                //*Em 63-01-10
                oSql.AppendLine("INNER JOIN TFNMRcvSpc APP WITH(NOLOCK) ON RCV.FTRcvCode = APP.FTRcvCode AND APP.FTAppCode = '"+ tApp +"'");
                oSql.AppendLine("AND (ISNULL(APP.FTBchCode,'') = '' OR ISNULL(APP.FTBchCode,'') = '" + cVB.tVB_BchCode + "')");
                oSql.AppendLine("AND (ISNULL(APP.FTMerCode,'') = '' OR ISNULL(APP.FTMerCode,'') = '" + cVB.tVB_Merchart + "')");
                oSql.AppendLine("AND (ISNULL(APP.FTShpCode,'') = '' OR ISNULL(APP.FTShpCode,'') = '" + cVB.tVB_ShpCode + "')");
                //++++++++++++++++++
                oSql.AppendLine("WHERE FTRcvStaUse = '1'");
                oSql.AppendLine("GROUP BY RCV.FTRcvCode,RCVL.FTRcvName,RCV.FTFmtCode");
                DataTable odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                
              
                if (odtTmp != null)
                {
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        ogdPaymentType.Rows.Add(oRow["FNRcvSeqNo"], oRow["FTRcvName"], "+", oRow["FCRcvAmt"], oRow["FTRcvCode"], oRow["FTFmtCode"]);

                    }
                    
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_GETxPaymentType : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                oW_SP.SP_CLExMemory();
            }
        }

        private void W_SETxGridDefualt()
        {
            try
            {
                string tFmtAmt = cVB.nVB_DecShow < 1 ? "#,###,##0" : "#,###,##0." + new string('0', cVB.nVB_DecShow);
                //ogdBanknote.Columns["otbTitleQty"].DefaultCellStyle.Format = "#,###,##0";
                //ogdBanknote.Columns["otbTitleAmt"].DefaultCellStyle.Format = tFmtAmt;
                //ogdBanknote.Columns["otbTitleAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                ogdPaymentType.Columns["otbAmtPay"].DefaultCellStyle.Format = tFmtAmt;
                ogdPaymentType.Columns["otbAmtPay"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_SETxGridDefualt : " + oEx.Message); }
        }

        private void W_SAVxShiftRCV()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            int nSeq = 0;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO TPSTShiftSKeyRcv WITH(ROWLOCK)(FTBchCode,FTPosCode,FTShfCode,FNSdtSeqNo,FTRcvCode,FCRcvPayAmt)");
                oSql.AppendLine("VALUES");
                foreach (DataGridViewRow oRow in ogdPaymentType.Rows)
                {
                    nSeq++;
                    if (nSeq == 1)
                    {
                        oSql.AppendLine("('" + cVB.tVB_BchCode + "','" + cVB.tVB_PosCode + "','" + cVB.tVB_ShfCode + "',");
                        oSql.AppendLine(nSeq + ",'" + oRow.Cells["otbRcvCode"].Value.ToString() + "'," + Convert.ToDecimal(oRow.Cells["otbAmtPay"].Value) + ")");
                    }
                    else
                    {
                        oSql.AppendLine(",('" + cVB.tVB_BchCode + "','" + cVB.tVB_PosCode + "','" + cVB.tVB_ShfCode + "',");
                        oSql.AppendLine(nSeq + ",'" + oRow.Cells["otbRcvCode"].Value.ToString() + "'," + Convert.ToDecimal(oRow.Cells["otbAmtPay"].Value) + ")");
                    }
                }
                if (!string.IsNullOrEmpty(oSql.ToString()))
                {
                    oDB.C_SETxDataQuery(oSql.ToString());
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_SAVxShiftRCV : " + oEx.Message); }
        }

        #endregion End Function / Method

        private void ogdPaymentType_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ogdPaymentType.CurrentRow == null)
                {
                    return;
                }
                if (ogdPaymentType.CurrentRow.Cells[5].Value.ToString() == "001")
                {
                    if(oBankNote.olaResult.Text == "0.00")
                    {
                        oBankNote.ShowDialog();
                        if (oBankNote.DialogResult == DialogResult.OK)
                        {
                            ogdPaymentType.CurrentRow.Cells[3].Value = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(oBankNote.olaResult.Text), cVB.nVB_DecShow);
                        }
                        //oBankNote.Dispose();
                        oBankNote.Hide();
                    }
                    
                }
                else
                {
                    wEnterAmount oFrm = new wEnterAmount();
                    oFrm.ShowDialog();
                    if (!string.IsNullOrEmpty(oFrm.otbAmount.Text))
                    {
                        ogdPaymentType.CurrentRow.Cells[3].Value = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(oFrm.otbAmount.Text), cVB.nVB_DecShow);
                    }
                    oFrm.Dispose();
                }
                
                W_SumAmount();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCloseShift", "ogdPaymentType_CellDoubleClick : " + oEx.Message);
            }
        }

        private void W_SumAmount()
        {
            try
            {
                decimal cSum = 0;
                for (int nRow = 0; nRow <= ogdPaymentType.RowCount - 1; nRow++) //*Arm 62-10-27 - แก้ไข nRow <= ogdPaymentType.RowCount-1
                {
                    decimal cGet = decimal.Parse(ogdPaymentType.Rows[nRow].Cells[3].Value.ToString());
                    cSum = cSum + cGet;
                }
                otbTotal.Text = new cSP().SP_SETtDecShwSve(1,cSum,cVB.nVB_DecShow);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCloseShift", "W_SumAmount : " + oEx.Message);
            }
        }

        private void opnMenu_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void wCloseShift_Shown(object sender, EventArgs e)
        {
            otbPin.Focus();
        }

        private void W_INSxTPSTShiftSKeyBN(DataGridView pogdBankNote)
        {
            StringBuilder oSql;
            cmlBankNote oBankNoteModel;
            try
            {
                foreach (DataGridViewRow oRow in pogdBankNote.Rows)
                {
                    oBankNoteModel = new cmlBankNote();
                    oBankNoteModel = oBankNote.aoW_BankNote.Where(x => x.FTBntName == oRow.Cells["otbType"].Value.ToString()).FirstOrDefault();
                    //oBankNote.FCBntAmt = oRow.Cells["otbAmtPay"].Value.ToString();
                    //oBankNote.FCBntQty = oRow.Cells["otbQuantityValue"].Value.ToString();

                    oBankNoteModel.FCBntAmt = oW_SP.SP_SETtDecShwSve(2, Convert.ToDecimal(oRow.Cells["otbAmt"].Value), cVB.nVB_DecSave);
                    oBankNoteModel.FCBntQty = oW_SP.SP_SETtDecShwSve(2, Convert.ToDecimal(oRow.Cells["otbQuantityValue"].Value), cVB.nVB_DecSave);

                    oSql = new StringBuilder();
                    oSql.AppendLine("INSERT INTO TPSTShiftSKeyBN WITH(ROWLOCK)");
                    oSql.AppendLine("(FTBchCode");
                    oSql.AppendLine(",FTPosCode");
                    oSql.AppendLine(",FTShfCode");
                    oSql.AppendLine(",FNSdtSeqNo");
                    oSql.AppendLine(",FTBntCode");
                    oSql.AppendLine(",FCKbnRateAmt");
                    oSql.AppendLine(",FNKbnQty");
                    oSql.AppendLine(",FCKbnAmt) ");
                    oSql.AppendLine("VALUES (");
                    oSql.AppendLine("'" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine(",'" + cVB.tVB_PosCode + "'");
                    oSql.AppendLine(",'" + cVB.tVB_ShfCode + "'");
                    oSql.AppendLine("," + cVB.nVB_ShfSeq);
                    oSql.AppendLine(",'" + oBankNoteModel.FTBntCode + "'");
                    oSql.AppendLine("," + oBankNoteModel.FCBntRateAmt);
                    oSql.AppendLine("," + oBankNoteModel.FCBntQty);
                    oSql.AppendLine("," + oBankNoteModel.FCBntAmt + ")");

                    new cDatabase().C_SETxDataQuery(oSql.ToString());
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRemark", "W_INSxTPSTShiftSKeyBN " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oBankNote = null;
            }
        }

        private void OgdPaymentType_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var oSenderGrid = (DataGridView)sender;

            if (oSenderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                try
                {
                    if (ogdPaymentType.CurrentRow == null)
                    {
                        return;
                    }
                    if (ogdPaymentType.CurrentRow.Cells[5].Value.ToString() == "001")
                    {
                        if (oBankNote.olaResult.Text == "0.00")
                        {
                            oBankNote.ShowDialog();
                            if (oBankNote.DialogResult == DialogResult.OK)
                            {
                                ogdPaymentType.CurrentRow.Cells[3].Value = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(oBankNote.olaResult.Text), cVB.nVB_DecShow);
                            }
                            //oBankNote.Dispose();
                            oBankNote.Hide();
                        }
                        
                    }
                    else
                    {
                        wEnterAmount oFrm = new wEnterAmount();
                        oFrm.ShowDialog();
                        if (!string.IsNullOrEmpty(oFrm.otbAmount.Text))
                        {
                            ogdPaymentType.CurrentRow.Cells[3].Value = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(oFrm.otbAmount.Text), cVB.nVB_DecShow);
                        }
                        oFrm.Dispose();
                    }

                    W_SumAmount();
                }
                catch (Exception oEx)
                {
                    new cLog().C_WRTxLog("wCloseShift", "ogdPaymentType_CellDoubleClick : " + oEx.Message);
                }
            }
        }

        private void OgdPaymentType_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            W_SumAmount();
        }
    }
}
