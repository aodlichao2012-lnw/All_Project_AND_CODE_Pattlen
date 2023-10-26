using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Required;
using AdaPos.Models.Webservice.Respond;
using AdaPos.Models.Webservice.Respond.SaleOrder;
using AdaPos.Popup.All;
using AdaPos.Popup.wPayment;
using AdaPos.Resources_String.Local;
using C1.Win.C1FlexGrid;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AdaPos.Forms
{
    public partial class wPayment : Form
    {
        #region Variable

        private IDisposable oW_Boardcast;
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private List<cmlTFNMRateUnit> aoW_RateUnit;
        private int nW_Time;
        private int nW_Mode;        // 1:Topup, 2:Cancel Topup, 3:Sale, 4:Return Sale, 5:Rental, 6:Return Rental, 7:Ticket
        private int nW_TxnID;
        private int nw_ModeClose = 2;    // 1:Back, 2:Close
        public decimal cW_AmtTotalShw, cW_AmtTotalCal, cW_AmtTotalPay, cW_CashPayment;
        public decimal cW_DisChgAmt;    //*Em 62-10-08
        private List<cmlTFNMRcv> aoW_RcvInfo;
        private List<cmlPayItem> aoW_PayItems;
        public bool bW_Activate = false;
        public int nW_StartRow = 0;
        public int nW_CurPage = 1;
        int nW_MaxPage = 1;  //*Net 63-03-26 ย้ายมาจาก W_GETxPaymentType
        bool bW_IsQuickAmt = false;
        //private int nW_CstPiontB4Used;   //Arm 63-03-11 แต้มก่อนใช้
        #endregion End Variable

        #region Constructor

        public wPayment(int pnMode)
        {
            new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : new Form Payment Start", cVB.bVB_AlwPrnLog); //*Net Stamp
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //*Net 63-07-31 ปรับตาม Moshi
                //if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);
                nW_Mode = pnMode;

                // W_GETxSelectSignType(panel1, "1");
                //panel1.Controls.Add(olaSignType);
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Set Design", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_SETxDesign();

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Set Text", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_SETxText();


                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Get Payment Type", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_GETxPaymentType();
                //W_GETxDisChg();

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Show Button Bar", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_SHWxButtonBar();

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Get Quick Amt", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_GETxQuickAmount();

                //new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Get Notify"); //*Net Stamp
                //W_GETxCountNotify();
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                oucPincode.oU_TextValue = otbAmount;

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Set Promotion", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_SETxPmtPdt();
                //nW_CstPiontB4Used = cVB.nVB_CstPoint;//*Arm 63-03-11
                //oucPincode.tU_TextValue = "";   //*Arm 62-10-01

                this.KeyPreview = true; //*Arm 63-02-06 - (HotKey)

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Add Leave Menubar Event", cVB.bVB_AlwPrnLog); //*Net Stamp
                opnMenu.MouseLeave += opnMenu_MouseLeave;
                foreach (System.Windows.Forms.Control opnC in opnMenu.Controls)
                {
                    opnC.MouseLeave += opnMenu_MouseLeave;
                    foreach (System.Windows.Forms.Control opnButton in opnMenu.Controls)
                    {
                        opnButton.MouseLeave += opnMenu_MouseLeave;
                    }
                }
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : new Form Payment End", cVB.bVB_AlwPrnLog); //*Net Stamp
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "wPayment : " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event 

        private void W_GETxSelectSignType(Panel opPanel, string ptType)
        {

            try
            {

                Font oFont = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
                for (var i = 0; i <= 2; i++)
                {
                    if (i == 1)
                    {
                        Label olaCenter = new Label();
                        olaCenter.AutoSize = true;
                        olaCenter.Font = oFont;
                        olaCenter.Text = "|";
                        olaCenter.Dock = DockStyle.Left;
                        opPanel.Controls.Add(olaCenter);
                    }
                    Label olaSignType = new Label();
                    olaSignType.AutoSize = true;
                    olaSignType.Font = oFont;
                    olaSignType.Dock = DockStyle.Left;
                    olaSignType.BringToFront();
                    olaSignType.ForeColor = Color.Gray;
                    olaSignType.Click += OlaSignType_Click;
                    switch (i)
                    {
                        case 0:

                            if (ptType == "1")
                            {
                                olaSignType.Cursor = Cursors.No;
                                olaSignType.ForeColor = Color.Black;
                            }
                            //olaSignType.Text = "Password";
                            olaSignType.Text = "ส่วนลด"; //*Arm 62-10-31 แสดงภาษาตามที่ตั้งค่า
                            olaSignType.Name = "olaDiscount";
                            olaSignType.Tag = "1";
                            break;
                        case 1:

                            if (ptType == "2")
                            {
                                olaSignType.Cursor = Cursors.No;
                                olaSignType.ForeColor = Color.Black;
                            }
                            //olaSignType.Text = "Pincode";
                            olaSignType.Text = "โปรโมชั่น";   //*Arm 62-10-31 แสดงภาษาตามที่ตั้งค่า
                            olaSignType.Name = "olaPromotion";
                            olaSignType.Tag = "2";
                            break;

                    }
                    opPanel.Controls.Add(olaSignType);
                }



            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_LOADxSignType : " + oEx.Message);
            }
        }

        private void OlaSignType_Click(object sender, EventArgs e)
        {
            try
            {
                Label olaLabel = (Label)sender;
                W_SETxPanelType(olaLabel.Tag.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSignin", "W_LOADxSignType : " + oEx.Message);
            }
        }

        private void W_SETxPanelType(string ptType)
        {
            try
            {
                //switch (ptType)
                //{
                //    case "1":
                //        opnDisChg.Visible = false;
                //        break;
                //    case "2":
                //        opnDisChg.Visible = true;
                //        break;
                //}
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPayment", "W_SETxPanelType : " + oEx.Message);
            }
        }


        /// <summary>
        /// GET Notification
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void W_Notification(object s, EventArgs e)
        {
            try
            {
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wHome", "W_Notification : " + oEx.Message);
            }
        }

        /// <summary>
        /// Open form Sae
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Press Back", cVB.bVB_AlwPrnLog); //*Net Stamp
                W_PRCxBack();

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wPayment_Shown(object sender, EventArgs e)
        {
            try
            {
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification); //*Net 63-07-31 ย้ายมาจาก ctor
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify); //*Arm 63-08-10
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Get Receive Info", cVB.bVB_AlwPrnLog); //*Net Stamp
                //*[AnUBiS][][2019-02-06] - load receive information.
                if (cVB.tVB_PosType == "2") aoW_RcvInfo = W_RCVaGetReceiveInfo();

                olaDocNo.Text = cVB.tVB_DocNo;
                //ogdCash.Rows.Add(1, "xxxx", 30.5);

                //var nXX = ogdCash.Rows.Count;
                //var oXX = ogdCash.Rows[ogdCash.Rows.Count-1].Cells[0].Value;


                //ogdCash.ClearSelection();
                ogdSum.ClearSelection();
                otbAmount.Focus();
                cPayment.nC_SeqRC = 0;
                cVB.oVB_Payment = this;

                //*Arm 62-11-18 
                if (cSale.nC_DocType == 9 && !string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                {
                    //otmStart.Enabled = true;
                    //*Arm 63-05-12 ยกออกมาจาก otmStart
                    new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Check Ref Redeem", cVB.bVB_AlwPrnLog); //*Net Stamp
                    W_CHKxRefRedeem(); //*Arm 63-03-21 เช็คการแลกแต้ม

                    new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Check DisChg Bill", cVB.bVB_AlwPrnLog); //*Net Stamp
                    W_CHKxRefDisChgBill(); //*Arm 63-03-21 เช็คการทำส่วนลด/ชาจน์ ท้ายบิล

                    new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Check Ref Payment", cVB.bVB_AlwPrnLog); //*Net Stamp
                    W_CHKxRefPayment();
                    //+++++++++++++
                }

                //*Arm 63-03-06 ตรวจสอบส่วนลดใบ SO
                if (cVB.oVB_ReferSO != null)
                {
                    //otmStart.Enabled = true;  //*Arm 63-05-12 Comment otmStart
                    new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Check DisChg Bill SO", cVB.bVB_AlwPrnLog); //*Net Stamp
                    W_PRCxDisChgAmtBillBySo();
                }
                //+++++++++++++

                W_SETxColGrid(ogdPmt);  //*Em 63-06-01
                W_SETxColGrid(ogdRcv);  //*Em 63-06-01

                //*Em 63-05-12
                Form oFormShow = null;
                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wReason);
                if (oFormShow != null) oFormShow.Hide();

                //*Em 63-05-26
                oFormShow = null;
                oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wPmtGetorSug);
                if (oFormShow != null) oFormShow.Hide();

                cSP.SP_SETxFixPanelOverFlow(opnHDMenuBar, olaBranch);  //*Net 63-06-09 Resize Branch
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "wPayment_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Clear selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdCash_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //ogdCash.ClearSelection();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "wPayment_Shown : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmMenu_Click : " + oEx.Message); }
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
                //Zen 13/04/2020 เหมือนกดปุ่มซ่อนโปร
                opnPromo.Visible = false;
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmKB_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Payment Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmPaymentType_Click(object sender, EventArgs e)
        {
            Button ocmPaymentType = (Button)sender;
            string[] aPay;
            //Zen 13/04/2020 เหมือนกดปุ่มซ่อนโปร
            opnPromo.Visible = false;
            try
            {

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Click " + ocmPaymentType.Text, cVB.bVB_AlwPrnLog); //*Net Stamp
                switch (nW_Mode)
                {
                    case 1:     // Topup

                        new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : " + ocmPaymentType.Text + "case Topup", cVB.bVB_AlwPrnLog); //*Net Stamp
                        aPay = ocmPaymentType.Name.Split('-');
                        cPayment.tC_RcvCode = aPay[1].ToString();
                        cPayment.tC_FmtCode = aPay[2].ToString();
                        cPayment.bC_StaAlwRet = aPay[3].ToString() == "1" ? true : false;
                        cPayment.bC_StaAlwCancel = aPay[4].ToString() == "1" ? true : false;
                        cPayment.bC_StaPayLast = aPay[5].ToString() == "1" ? true : false;
                        cPayment.tC_RcvName = ocmPaymentType.Text;
                        cPayment.tC_TblTopUpRC = "TFNTCrdTopUpRC";
                        W_PRCxTopUpRCCash();
                        break;
                    case 2:     // Cancel Topup
                                // W_PRCxPayment(ocmPaymentType.Tag.ToString());
                        break;
                    default:

                        if (string.IsNullOrEmpty(otbAmount.Text))   //*Arm 63-04-30
                        {
                            cVB.cVB_Amount = Convert.ToDecimal(olaCashPayment.Text);
                        }
                        else
                        {
                            cVB.cVB_Amount = Convert.ToDecimal(otbAmount.Text); //*Em 62-10-07
                        }

                        aPay = ocmPaymentType.Name.Split('-');
                        if (aPay.Length > 2)    //*Em 63-03-29
                        {
                            cPayment.tC_RcvCode = aPay[1].ToString();
                            cPayment.tC_FmtCode = aPay[2].ToString();
                            cPayment.bC_StaAlwRet = aPay[3].ToString() == "1" ? true : false;
                            cPayment.bC_StaAlwCancel = aPay[4].ToString() == "1" ? true : false;
                            cPayment.bC_StaPayLast = aPay[5].ToString() == "1" ? true : false;
                        }

                        cPayment.tC_RcvName = ocmPaymentType.Text;
                        cVB.tVB_KbdCallByName = ocmPaymentType.Tag.ToString();
                        new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Call {cVB.tVB_KbdCallByName} ", cVB.bVB_AlwPrnLog); //*Net Stamp
                        new cFunctionKeyboard().C_PRCxCallByName(cVB.tVB_KbdCallByName);

                        //*Net 63-07-08 sync ยอดชำระกับหน้าจอ 2
                        if (cVB.oVB_CstScreen != null)
                        {
                            cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                            cVB.oVB_CstScreen.W_SETxSummaryAmt(olaCashPayment.Text);
                        }
                        //++++++++++++++++++++++++++++++++++
                        oucPincode.tU_TextValue = "";   //*Em 62-10-10
                        otbAmount.Focus();
                        break;
                }
                otbAmount.Focus();
                otbAmount.SelectAll();

                cVB.tVB_KbdScreen = "PAYMENT"; //*Arm 63-06-15

                //*Em 63-04-22
                if (cSale.bC_SetComplete)
                {
                    new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Set Complete", cVB.bVB_AlwPrnLog); //*Net Stamp
                    olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tChange");
                    olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_Change, cVB.nVB_DecShow);
                    //cSale.C_PRCxSetComplete();
                    if (cSale.C_PRCbSetComplete())
                    {
                        cSale.bC_SetComplete = false;
                        this.Close();
                    }
                    else
                    {
                        cSale.bC_SetComplete = false;
                        return;
                    }
                }
                //+++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmPaymentType_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Discount Charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDisChg_Click(object sender, EventArgs e)
        {
            string tFuncName;
            try
            {
                Button ocmDisChg = (Button)sender;
                tFuncName = ocmDisChg.Tag.ToString();
                //Zen 13/04/2020 เหมือนกดปุ่มซ่อนโปร
                opnPromo.Visible = false;
                switch (tFuncName)
                {
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmDisChg_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name, cVB.bVB_AlwPrnLog); //*Net Stamp
                //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                //if (cVB.oVB_2ndScreen != null)
                //{
                //    if (nw_ModeClose == 1)
                //    {
                //        cVB.oVB_2ndScreen.Update();
                //    }
                //    else if (nw_ModeClose == 2)
                //    {
                //        cVB.oVB_2ndScreen.W_DATxClear();
                //        cVB.oVB_2ndScreen.Update();
                //    }
                //}

                otmClose.Stop();

                if (oW_Boardcast != null)
                    oW_Boardcast.Dispose();

                oW_Boardcast = null;
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;


                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "wPayment_FormClosing : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// แจ้งเตือน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                // W_CHKxNotify(); //[Pong][2019-01-30][Comment code]
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmNotify_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Banknote 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBanknote_Click(object sender, EventArgs e)
        {
            Button ocmButton;
            decimal cValue, cReceived = 0;

            try
            {

                ocmButton = (Button)sender;
                cValue = Convert.ToDecimal(ocmButton.AccessibleDescription);
                //cValue = Convert.ToDecimal(ocmButton.Text); //*Net 63-04-01 ยกมาจาก baseline
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Click Quick Amt {cValue}", cVB.bVB_AlwPrnLog); //*Net Stamp

                if (!string.IsNullOrEmpty(otbAmount.Text))
                    cReceived = Convert.ToDecimal(otbAmount.Text);

                /*if (bW_IsQuickAmt)
                {
                    if (Convert.ToDecimal((cReceived + cValue) - cVB.oVB_Payment.cW_AmtTotalShw) <= (decimal)cVB.cVB_MaxChg)
                    {
                        otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cReceived + cValue, cVB.nVB_DecShow);
                    }
                }
                else
                {
                    otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow);
                    bW_IsQuickAmt = true;
                }*/
                /*if (Convert.ToDecimal(cReceived + cValue) > (decimal)cVB.cVB_MaxChg)
                {
                    if (Convert.ToDecimal(cVB.oVB_Payment.cW_AmtTotalShw) == Convert.ToDecimal(cReceived))
                    {
                        otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow);
                    }
                }
                else
                {
                    if (Convert.ToDecimal(cVB.oVB_Payment.cW_AmtTotalShw) == Convert.ToDecimal(cReceived))
                    {
                        otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow);
                    }
                    else
                    {
                        otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cReceived + cValue, cVB.nVB_DecShow);
                    }
                }*/

                //*Net 63-07-31 ปรับตาม Moshi
                //*Em 63-07-28
                if (Convert.ToDecimal(cReceived) == cW_AmtTotalShw)
                {
                    otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cValue, cVB.nVB_DecShow);
                }
                else
                {
                    if (Convert.ToDecimal((cReceived + cValue) - cVB.oVB_Payment.cW_AmtTotalShw) <= (decimal)cVB.cVB_MaxChg)
                    {
                        otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cReceived + cValue, cVB.nVB_DecShow);
                    }
                }
                //+++++++++++++++

                //Zen 13/04/2020 เหมือนกดปุ่มซ่อนโปร
                opnPromo.Visible = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmBanknote_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;

            try
            {   // Call by name
                //    tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                //    cVB.tVB_KbdCallByName = tFuncName;
                //    new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                //    W_GETxFuncByFuncName(tFuncName);

                //    cVB.tVB_KbdScreen = "PAYMENT";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "ocmMenu_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
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
                    case "C_KBDxBack": W_PRCxBack(); break;
                    case "C_KBDxPdtDetail": break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch
            { }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }
        #endregion End Event

        #region Function / Method
        /// <summary>
        /// Check App Receive
        /// </summary>
        private void W_CHKxRefPayment() //*Arm 62-11-13
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable oDbTbl;
            cmlTPSTSalRD oSalRD; //*Arm 63-03-18 Redeem
            decimal cAmount;
            try
            {
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : RefPayment Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                oSql = new StringBuilder();

                oSql.AppendLine("SELECT TSRC.FTBchCode,TSRC.FTXshDocNo,TSRC.FNXrcSeqNo,TSRC.FTRcvCode,TSRC.FTRcvName,");
                oSql.AppendLine("TSRC.FTXrcRefNo1,TSRC.FTXrcRefNo2,TSRC.FDXrcRefDate,TSRC.FTXrcRefDesc,TSRC.FTBnkCode,");
                oSql.AppendLine("TSRC.FTRteCode,TSRC.FCXrcRteFac,TSRC.FCXrcFrmLeftAmt,TSRC.FCXrcUsrPayAmt,TSRC.FCXrcDep,");
                oSql.AppendLine("TSRC.FCXrcNet,TSRC.FCXrcChg,TSRC.FTXrcRmk,TSRC.FTPhwCode,TSRC.FTXrcRetDocRef,TRCV.FTFmtCode,");
                //oSql.AppendLine("RCVA.FTAppStaAlwRet,RCVA.FTAppStaAlwCancel,RCVA.FTAppStaPayLast");
                oSql.AppendLine("TRCV.FTAppStaAlwRet,TRCV.FTAppStaAlwCancel,TRCV.FTAppStaPayLast"); //*Arm 63-09-18
                //oSql.AppendLine("TSRC.FTXrcStaPayOffline,TSRC.FDLastUpdOn,TSRC.FTLastUpdBy,TSRC.FDCreateOn,TSRC.FTCreateBy,");
                //oSql.AppendLine("TRCV.FTRcvCode,TRCV.FTFmtCode,TRCV.FTRcvStaUse,TRCV.FTRcvStaShwInSlip,TRCV.FTRcv4Ret,TRCV.FTRcv4ChkOut,");
                //oSql.AppendLine("TRCV.FDLastUpdOn,TRCV.FTLastUpdBy,TRCV.FDCreateOn,TRCV.FTCreateBy");
                oSql.AppendLine("FROM TPSTSalRC TSRC WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TFNMRcv TRCV WITH(NOLOCK) ON TSRC.FTRcvCode = TRCV.FTRcvCode AND TRCV.FTRcvStaUse = '1'");
                if (cVB.tVB_PosType == "1")
                {
                    //oSql.AppendLine("INNER JOIN TSysRcvApp RCVA WITH(NOLOCK) ON TRCV.FTFmtCode = RCVA.FTFmtCode AND RCVA.FTAppCode = 'PS'");
                    //*Em 63-01-10
                    oSql.AppendLine("INNER JOIN TFNMRcvSpc RCVA WITH(NOLOCK) ON TRCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'PS'");
                    oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = '" + cVB.tVB_BchCode + "')");
                    oSql.AppendLine("AND (ISNULL(RCVA.FTMerCode,'') = '' OR ISNULL(RCVA.FTMerCode,'') = '" + cVB.tVB_Merchart + "')");
                    oSql.AppendLine("AND (ISNULL(RCVA.FTShpCode,'') = '' OR ISNULL(RCVA.FTShpCode,'') = '" + cVB.tVB_ShpCode + "')");
                    //++++++++++++++++++
                }
                else
                {
                    //oSql.AppendLine("INNER JOIN TSysRcvApp RCVA WITH(NOLOCK) ON TRCV.FTFmtCode = RCVA.FTFmtCode AND RCVA.FTAppCode = 'FC'");
                    //*Em 63-01-10
                    oSql.AppendLine("INNER JOIN TFNMRcvSpc RCVA WITH(NOLOCK) ON TRCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'PS'");
                    oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = '" + cVB.tVB_BchCode + "')");
                    oSql.AppendLine("AND (ISNULL(RCVA.FTMerCode,'') = '' OR ISNULL(RCVA.FTMerCode,'') = '" + cVB.tVB_Merchart + "')");
                    oSql.AppendLine("AND (ISNULL(RCVA.FTShpCode,'') = '' OR ISNULL(RCVA.FTShpCode,'') = '" + cVB.tVB_ShpCode + "')");
                    //++++++++++++++++++
                }
                oSql.AppendLine("WHERE TSRC.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' ");
                //oSql.AppendLine("AND RCVA.FTAppStaAlwRet = '1'");
                oSql.AppendLine("AND TRCV.FTAppStaAlwRet = '1'"); //*Arm 63-09-18

                oDB = new cDatabase();
                oDbTbl = new DataTable();
                oDbTbl = oDB.C_GEToDataQuery(oSql.ToString());

                if (oDbTbl != null)
                {
                    if (oDbTbl.Rows.Count > 0)
                    {
                        foreach (DataRow oRow in oDbTbl.Rows)
                        {
                            switch (oRow.Field<string>("FTFmtCode"))
                            {
                                case "012": // Alipay
                                    cVB.cVB_Amount = oRow.Field<decimal>("FCXrcNet");

                                    //*Arm 62-11-18 
                                    cPayment.tC_RcvCode = oRow.Field<string>("FTRcvCode");
                                    cPayment.tC_FmtCode = oRow.Field<string>("FTFmtCode");
                                    cPayment.bC_StaAlwRet = oRow.Field<string>("FTAppStaAlwRet") == "1" ? true : false;
                                    cPayment.bC_StaAlwCancel = oRow.Field<string>("FTAppStaAlwCancel") == "1" ? true : false;
                                    cPayment.bC_StaPayLast = oRow.Field<string>("FTAppStaPayLast") == "1" ? true : false;
                                    cPayment.tC_RcvName = oRow.Field<string>("FTRcvName");
                                    cVB.tVB_KbdCallByName = "C_KBDxAlipay";
                                    new cFunctionKeyboard().C_PRCxCallByName(cVB.tVB_KbdCallByName);

                                    //*******************************
                                    break;
                                case "004": //คูปองเงินสด
                                    cPayment.tC_RcvCode = oRow.Field<string>("FTRcvCode");
                                    cPayment.tC_RcvName = oRow.Field<string>("FTRcvName");
                                    cPayment.tC_XrcRef1 = oRow.Field<string>("FTXrcRefNo1");
                                    cPayment.tC_XrcRef2 = oRow.Field<string>("FTXrcRefNo2");
                                    cVB.cVB_Amount = oRow.Field<decimal>("FCXrcUsrPayAmt");
                                    cPayment.C_PRCxPaymentCoupon(1);
                                    break;
                                case "020": //คูปองส่วนลด
                                    cPayment.tC_RcvCode = oRow.Field<string>("FTRcvCode");
                                    cPayment.tC_RcvName = oRow.Field<string>("FTRcvName");
                                    cPayment.tC_XrcRef1 = oRow.Field<string>("FTXrcRefNo1");
                                    cPayment.tC_XrcRef2 = oRow.Field<string>("FTXrcRefNo2");
                                    cVB.cVB_Amount = oRow.Field<decimal>("FCXrcUsrPayAmt");
                                    W_ADDxDisChgBill("5", Convert.ToString(oRow.Field<decimal>("FCXrcUsrPayAmt")), oRow.Field<decimal>("FCXrcUsrPayAmt"));

                                    oSql.Clear();
                                    oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                                    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                                    oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                                    cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());
                                    cPayment.C_ADDxDisChgBill(cVB.cVB_Amount, cVB.cVB_Amount, cVB.cVB_Amount.ToString(), cAmount, 1);
                                    //cPayment.C_PRCxPaymentCoupon(2); //*Net 63-03-17
                                    break;

                                case "022": // แต้มแลกสินค้า *Arm 63-03-18 Redeem

                                    cPayment.tC_RcvCode = oRow.Field<string>("FTRcvCode");
                                    cPayment.tC_RcvName = oRow.Field<string>("FTRcvName");
                                    cPayment.tC_XrcRef1 = oRow.Field<string>("FTXrcRefNo1");
                                    cPayment.tC_XrcRef2 = oRow.Field<string>("FTXrcRefNo2");
                                    cVB.cVB_Amount = oRow.Field<decimal>("FCXrcUsrPayAmt");

                                    oSql.Clear();
                                    oSql.AppendLine("SELECT RD.FNXrdRefSeq, RD.FCXrdPdtQty, RD.FNXrdPntUse");
                                    oSql.AppendLine("FROM TPSTSalRC RC WITH(NOLOCK)");
                                    oSql.AppendLine("INNER JOIN TPSTSalRD RD ON RC.FTXrcRefNo1 = RD.FTXrdRefCode AND RC.FTXshDocNo = RD.FTXshDocNo AND RC.FTBchCode = RD.FTBchCode AND RC.FNXrcSeqNo = '" + oRow.Field<int>("FNXrcSeqNo") + "' ");
                                    oSql.AppendLine("WHERE RD.FTRdhDocType = 2 ");
                                    oSql.AppendLine("AND RC.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                                    oSql.AppendLine("AND RC.FTBchCode = '" + cVB.tVB_BchCode + "' ");

                                    oSalRD = new cmlTPSTSalRD();
                                    oSalRD = new cDatabase().C_GEToDataQuery<cmlTPSTSalRD>(oSql.ToString());
                                    cPayment.C_PRCxPaymentRedeem("2", (decimal)oSalRD.FCXrdPdtQty, (int)oSalRD.FNXrdPntUse);

                                    break;

                                case "023": // แต้มแลกสินค้า  *Arm 63-03-31 Redeem

                                    cPayment.tC_RcvCode = oRow.Field<string>("FTRcvCode");
                                    cPayment.tC_RcvName = oRow.Field<string>("FTRcvName");
                                    cPayment.tC_XrcRef1 = oRow.Field<string>("FTXrcRefNo1");
                                    cPayment.tC_XrcRef2 = oRow.Field<string>("FTXrcRefNo2");
                                    cVB.cVB_Amount = oRow.Field<decimal>("FCXrcUsrPayAmt");

                                    oSql.Clear();
                                    oSql.AppendLine("SELECT RD.FNXrdRefSeq, RD.FCXrdPdtQty, RD.FNXrdPntUse");
                                    oSql.AppendLine("FROM TPSTSalRC RC WITH(NOLOCK)");
                                    oSql.AppendLine("INNER JOIN TPSTSalRD RD ON RC.FTXrcRefNo1 = RD.FTXrdRefCode AND RC.FTXshDocNo = RD.FTXshDocNo AND RC.FTBchCode = RD.FTBchCode AND RC.FNXrcSeqNo = '" + oRow.Field<int>("FNXrcSeqNo") + "' ");
                                    oSql.AppendLine("WHERE RD.FTRdhDocType = 1 ");
                                    oSql.AppendLine("AND RC.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                                    oSql.AppendLine("AND RC.FTBchCode = '" + cVB.tVB_BchCode + "' ");

                                    oSalRD = new cmlTPSTSalRD();
                                    oSalRD = new cDatabase().C_GEToDataQuery<cmlTPSTSalRD>(oSql.ToString());
                                    cPayment.C_PRCxPaymentRedeem("1", (decimal)oSalRD.FCXrdPdtQty, (int)oSalRD.FNXrdPntUse, (int)oSalRD.FNXrdRefSeq);

                                    break;

                                default:
                                    return;
                            }
                        }

                        new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Process FmtCode End", cVB.bVB_AlwPrnLog); //*Net Stamp
                    }
                }
                else
                {
                    return;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_CHKxRefPayment : " + oEx.Message); }
            finally
            {
                oSalRD = null;  //*Arm 63-03-18
            }
        }

        /// <summary>
        /// *Arm 63-03-21
        /// Check Refun DisChg Redeem
        /// </summary>
        private void W_CHKxRefRedeem()
        {
            StringBuilder oSql;
            cDatabase oDB;
            cmlRdSalDTDis oSalDTDis;
            List<cmlProrateDT> aoProDT; // Arm 63-03-25
            cmlRdSalRD oSalRD;
            decimal cB4Dis = 0;
            int nSeqNo = 0;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                ////DocType 1
                //if (cVB.aoVB_PdtRdDocType1.Count > 0)
                //{
                //    foreach (cmlPdtRedeem oData in cVB.aoVB_PdtRdDocType1)
                //    {
                //        // มูลค่าสุทธิก่อนลด
                //        oSql.Clear();
                //        oSql.AppendLine("SELECT CASE WHEN ISNULL(FCXsdNetAfHD,0.00) = 0.00 THEN ISNULL(FCXsdAmtB4DisChg,0.00) ELSE ISNULL(FCXsdNetAfHD,0.00) END AS cAmount, FNXsdSeqNo");
                //        oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH (NOLOCK)");
                //        oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                //        oSql.AppendLine("AND FTPdtCode='" + oData.ptPdtCode + "' ");
                //        oSql.AppendLine("AND FTXsdBarCode='" + oData.ptBarcode + "' ");
                //        DataTable oDbTbl = oDB.C_GEToDataQuery(oSql.ToString());

                //        if (oDbTbl != null)
                //        {
                //            if (oDbTbl.Rows.Count > 0)
                //            {
                //                foreach (DataRow oRow in oDbTbl.Rows)
                //                {
                //                    cB4Dis = oRow.Field<decimal>("cAmount");
                //                    nSeqNo = oRow.Field<int>("FNXsdSeqNo");
                //                }
                //            }
                //        }

                //        cSale.nC_DTSeqNo = nSeqNo;
                //        cVB.oVB_OrderRowIndex = cSale.nC_DTSeqNo - 1;

                //        //*Arm 63-03-25
                //        aoProDT = new List<cmlProrateDT>();
                //        cmlProrateDT oProDT = new cmlProrateDT();
                //        oProDT.FNSeqNo = nSeqNo;
                //        oProDT.FTBchCode = cVB.tVB_BchCode; ;
                //        oProDT.FTSalDocNo = cVB.tVB_DocNo;


                //        aoProDT.Add(oProDT);
                //        cPayment.C_ADDxDisChgBill((decimal)oData.pcValue, (decimal)oData.pcValue, oW_SP.SP_SETtDecShwSve(1, oData.pcValue, cVB.nVB_DecShow).ToString(), cB4Dis, 1, oData.ptRefCode, aoProDT);

                //        oSalRD = new cmlRdSalRD();
                //        oSalRD.FTBchCode = cVB.tVB_BchCode;
                //        oSalRD.FTXshDocNo = cVB.tVB_DocNo;
                //        oSalRD.FTRdhDocType = "1";
                //        oSalRD.FNXrdRefSeq = nSeqNo;
                //        oSalRD.FTXrdRefCode = oData.ptRefCode;
                //        oSalRD.FCXrdPdtQty = oData.pcPdtQty;
                //        oSalRD.FNXrdPntUse = (int)oData.pnUsePnt;
                //        new cSale().C_PRCxInsertSalRD(oSalRD);

                //        // Show
                //        cVB.oVB_Payment.W_ADDxDisChgBill("8", oW_SP.SP_SETtDecShwSve(1, (decimal)oData.pcValue, cVB.nVB_DecShow).ToString(), (decimal)oData.pcValue);
                //        if (cVB.oVB_2ndScreen != null)
                //        {
                //            cVB.oVB_2ndScreen.W_ADDxDisChgBill("8", oW_SP.SP_SETtDecShwSve(1, (decimal)oData.pcValue, cVB.nVB_DecShow).ToString(), (decimal)oData.pcValue);


                //        }

                //        //++++++++++++++


                //        //oSalDTDis = new cmlRdSalDTDis();
                //        //oSalDTDis.FTBchCode = cVB.tVB_BchCode;
                //        //oSalDTDis.FTXshDocNo = cVB.tVB_DocNo;
                //        //oSalDTDis.FNXsdSeqNo = nSeqNo;
                //        //oSalDTDis.FNXddStaDis = 2;
                //        //oSalDTDis.FTXddDisChgTxt = oW_SP.SP_SETtDecShwSve(1, oData.pcValue, cVB.nVB_DecShow).ToString();
                //        //oSalDTDis.FTXddDisChgType = "1";
                //        //oSalDTDis.FCXddNet = cB4Dis;
                //        //oSalDTDis.FCXddValue = oData.pcValue;
                //        //oSalDTDis.FTXddRefCode = oData.ptRefCode;

                //        ////****** SalRD ***********
                //        //oSalDTDis.FCXrdPdtQty = oData.pcPdtQty;
                //        //oSalDTDis.FNXrdPntUse = oData.pnUsePnt;
                //        //oSalDTDis.FTRdhDocType = "1";
                //        //new cSale().C_PRCxRedeemDiscountItem(oSalDTDis);

                //        //oSalDTDis = null;
                //    }

                //}

                ////DocType 2
                //if (cVB.aoVB_PdtRdDocType2.Count > 0)
                //{
                //    foreach (cmlPdtRedeem oData in cVB.aoVB_PdtRdDocType2)
                //    {
                //        // มูลค่าสุทธิก่อนลดชาร์จ
                //        oSql.Clear();
                //        oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                //        oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                //        oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                //        cB4Dis = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                //        // คำนวณส่วนลดท้ายบิล
                //        cPayment.C_ADDxDisChgBill(oData.pcUseMny, oData.pcUseMny, oW_SP.SP_SETtDecShwSve(1, oData.pcUseMny, cVB.nVB_DecShow).ToString(), cB4Dis, 1, oData.ptRefCode);

                //        // Insert SaleRD
                //        oSalRD = new cmlRdSalRD();
                //        oSalRD.FTBchCode = cVB.tVB_BchCode;
                //        oSalRD.FTXshDocNo = cVB.tVB_DocNo;
                //        oSalRD.FTRdhDocType = "2";
                //        oSalRD.FCXrdPdtQty = 0;     //*Arm 63-03-30
                //        oSalRD.FTXrdRefCode = oData.ptRefCode;
                //        oSalRD.FNXrdPntUse = oData.pnUsePnt;
                //        new cSale().C_PRCxInsertSalRD(oSalRD);

                //        cVB.oVB_Payment.W_ADDxDisChgBill("6", oW_SP.SP_SETtDecShwSve(1, oData.pcUseMny, cVB.nVB_DecShow).ToString(), oData.pcUseMny);
                //        if (cVB.oVB_2ndScreen != null)
                //        {
                //            cVB.oVB_2ndScreen.W_ADDxDisChgBill("6", oW_SP.SP_SETtDecShwSve(1, oData.pcUseMny, cVB.nVB_DecShow).ToString(), oData.pcUseMny);
                //        }

                //    }
                //}


                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Process RefRedeem Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                //*Arm 63-05-09 
                DataTable oDTb = new DataTable();
                // Arm 63-05-09 DocType1
                oSql.Clear();
                oSql.AppendLine("SELECT RD.FTBchCode, RD.FTXshDocNo,RD.FTXrdRefCode,RD.FTRdhDocType,DTDis.FTXddDisChgType,DTDis.FCXddValue,RD.FNXrdPntUse ");
                oSql.AppendLine("FROM ( SELECT FTBchCode, FTXshDocNo, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, SUM(FCXddValue) AS FCXddValue, FTXddRefCode From " + cSale.tC_TblSalDTDis + " ");
                oSql.AppendLine("       WHERE FTBchCode= '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXddStaDis = '2' AND ISNULL(FTXddRefCode,'') != '' ");
                oSql.AppendLine("       GROUP BY FTBchCode, FTXshDocNo,FNXddStaDis,FTXddDisChgTxt, FTXddDisChgType,FTXddRefCode ");
                oSql.AppendLine(") DTDis ");
                oSql.AppendLine("INNER JOIN ");
                oSql.AppendLine("(SELECT FTBchCode, FTXshDocNo, FTXrdRefCode, FTRdhDocType, SUM(FNXrdPntUse) AS FNXrdPntUse  FROM " + cSale.tC_TblSalRD + " ");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTRdhDocType = 1 ");
                oSql.AppendLine("GROUP BY FTBchCode, FTXshDocNo, FTXrdRefCode, FTRdhDocType) As RD ");
                oSql.AppendLine("ON DTDis.FTXddRefCode = RD.FTXrdRefCode ");
                oDTb = new DataTable();
                oDTb = oDB.C_GEToDataQuery(oSql.ToString());

                if (oDTb != null & oDTb.Rows.Count > 0)
                {
                    foreach (DataRow oRow in oDTb.Rows)
                    {
                        // มูลค่าสุทธิก่อนลดชาร์จ
                        oSql.Clear();
                        oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                        oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                        oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                        cB4Dis = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                        // คำนวณส่วนลดท้ายบิล
                        //cPayment.C_ADDxDisChgBill(oRow.Field<decimal>("FCXddValue"), oRow.Field<decimal>("FCXddValue"), oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXddValue"), cVB.nVB_DecShow).ToString(), cB4Dis, 1, oRow.Field<string>("FTXrdRefCode"));
                        cPayment.C_ADDxDisChgBill(oRow.Field<decimal>("FCXddValue"), oRow.Field<decimal>("FCXddValue"), oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXddValue"), cVB.nVB_DecShow).ToString(), (cB4Dis- oRow.Field<decimal>("FCXddValue")), 1, oRow.Field<string>("FTXrdRefCode")); //*Arm 63-09-17
                        cVB.oVB_Payment.W_ADDxDisChgBill("8", oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXddValue"), cVB.nVB_DecShow).ToString(), oRow.Field<decimal>("FCXddValue"));
                        //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                        //if (cVB.oVB_2ndScreen != null)
                        //{
                        //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("8", oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXddValue"), cVB.nVB_DecShow).ToString(), oRow.Field<decimal>("FCXddValue"));
                        //}
                    }

                }
                oDTb = null;
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Process RefRedeem End", cVB.bVB_AlwPrnLog); //*Net Stamp

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Process RefRedeem DocType 2 Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                // Arm 63-05-09 DocType2
                oSql.Clear();
                oSql.AppendLine("SELECT RD.FTBchCode, RD.FTXshDocNo,RD.FTXrdRefCode,RD.FTRdhDocType,DTDis.FTXddDisChgType,DTDis.FCXddValue,RD.FNXrdPntUse ");
                oSql.AppendLine("FROM ( SELECT FTBchCode, FTXshDocNo, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, SUM(FCXddValue) AS FCXddValue, FTXddRefCode From " + cSale.tC_TblSalDTDis + " ");
                oSql.AppendLine("       WHERE FTBchCode= '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXddStaDis = '2' AND ISNULL(FTXddRefCode,'') != '' ");
                oSql.AppendLine("       GROUP BY FTBchCode, FTXshDocNo,FNXddStaDis,FTXddDisChgTxt, FTXddDisChgType,FTXddRefCode ");
                oSql.AppendLine(") DTDis ");
                oSql.AppendLine("INNER JOIN ");
                oSql.AppendLine("(SELECT FTBchCode, FTXshDocNo, FTXrdRefCode, FTRdhDocType, SUM(FNXrdPntUse) AS FNXrdPntUse  FROM " + cSale.tC_TblSalRD + " ");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTRdhDocType = 2 ");
                oSql.AppendLine("GROUP BY FTBchCode, FTXshDocNo, FTXrdRefCode, FTRdhDocType) As RD ");
                oSql.AppendLine("ON DTDis.FTXddRefCode = RD.FTXrdRefCode ");
                oDTb = new DataTable();
                oDTb = oDB.C_GEToDataQuery(oSql.ToString());

                if (oDTb != null & oDTb.Rows.Count > 0)
                {
                    foreach (DataRow oRow in oDTb.Rows)
                    {
                        // มูลค่าสุทธิก่อนลดชาร์จ
                        oSql.Clear();
                        oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                        oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                        oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                        cB4Dis = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                        // คำนวณส่วนลดท้ายบิล
                        //cPayment.C_ADDxDisChgBill(oRow.Field<decimal>("FCXddValue"), oRow.Field<decimal>("FCXddValue"), oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXddValue"), cVB.nVB_DecShow).ToString(), cB4Dis, 1, oRow.Field<string>("FTXrdRefCode"));
                        cPayment.C_ADDxDisChgBill(oRow.Field<decimal>("FCXddValue"), oRow.Field<decimal>("FCXddValue"), oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXddValue"), cVB.nVB_DecShow).ToString(), (cB4Dis + oRow.Field<decimal>("FCXddValue")), 1, oRow.Field<string>("FTXrdRefCode")); //*Arm 63-09-17
                        cVB.oVB_Payment.W_ADDxDisChgBill("6", oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXddValue"), cVB.nVB_DecShow).ToString(), oRow.Field<decimal>("FCXddValue"));
                        //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                        //if (cVB.oVB_2ndScreen != null)
                        //{
                        //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("6", oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXddValue"), cVB.nVB_DecShow).ToString(), oRow.Field<decimal>("FCXddValue"));
                        //}
                    }
                }
                oDTb = null;
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Process RefRedeem DocType2 End", cVB.bVB_AlwPrnLog); //*Net Stamp
                //+++++++++++++
                //*Arm 63-05-09 
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPayment", "W_CHKxRefRedeem : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                oSalDTDis = null;
                oSalRD = null;
                aoProDT = null;
            }

        }

        /// <summary>
        /// *Arm 63-03-20
        /// Check Dis/Chg ท้ายบิล
        /// </summary>
        private void W_CHKxRefDisChgBill()
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlTPSTSalHDDis> aoSalHDDis;
            decimal cAmount = 0;
            decimal cDis = 0;
            decimal cDisPer = 0;
            decimal cChg = 0;
            decimal cChgPer = 0;
            decimal cChgDisCpn = 0;
            decimal cChgRdmCpn = 0;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                aoSalHDDis = new List<cmlTPSTSalHDDis>();

                //*Arm 63-04-03
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                if (cAmount <= 0)
                {
                    // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                    //new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tNoAmtDisChg"), 2);
                    return;
                }
                else
                {
                    new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : AddDisChg Bill ReturnType {cVB.nVB_ReturnType} Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                    if (cVB.nVB_ReturnType == 1 || cVB.bVB_RefundFullBill)
                    {
                        if (cVB.aoVB_PdtHDDisRefund.Count > 0)
                        {
                            foreach (cmlTPSTSalHDDis oHDDis in cVB.aoVB_PdtHDDisRefund)
                            {
                                //cPayment.C_ADDxDisChgBill((decimal)oHDDis.FCXhdAmt, (decimal)oHDDis.FCXhdDisChg, oHDDis.FTXhdDisChgTxt, (decimal)oHDDis.FCXhdTotalAfDisChg, Convert.ToInt32(oHDDis.FTXhdDisChgType));

                                //*Arm 63-09-17
                                decimal cB4DisChg = (decimal)oHDDis.FCXhdTotalAfDisChg;
                                switch (oHDDis.FTXhdDisChgType)
                                {
                                    case "1":
                                    case "2":
                                    case "5":
                                    case "6":
                                        cB4DisChg = ((decimal)oHDDis.FCXhdTotalAfDisChg + (decimal)oHDDis.FCXhdAmt);
                                        break;
                                    case "3":
                                    case "4":
                                        cB4DisChg = ((decimal)oHDDis.FCXhdTotalAfDisChg - (decimal)oHDDis.FCXhdAmt);
                                        break;
                                }
                                cPayment.C_ADDxDisChgBill((decimal)oHDDis.FCXhdAmt, (decimal)oHDDis.FCXhdDisChg, oHDDis.FTXhdDisChgTxt, cB4DisChg, Convert.ToInt32(oHDDis.FTXhdDisChgType));
                                //+++++++++++++

                                cVB.oVB_Payment.W_ADDxDisChgBill(oHDDis.FTXhdDisChgType, oHDDis.FTXhdDisChgTxt, (decimal)oHDDis.FCXhdAmt);
                                //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                                //if (cVB.oVB_2ndScreen != null)
                                //{
                                //    cVB.oVB_2ndScreen.W_ADDxDisChgBill(oHDDis.FTXhdDisChgType, oHDDis.FTXhdDisChgTxt, (decimal)oHDDis.FCXhdAmt);
                                //}
                            }
                        }
                    }
                    else
                    {

                        //*Arm 63-03-21

                        if (cVB.aoVB_PdtDisChgRefund.Count > 0)
                        {
                            foreach (cmlPdtDisChg oDisChg in cVB.aoVB_PdtDisChgRefund)
                            {
                                if (oDisChg.pnStaDis == 2)
                                {
                                    if (string.IsNullOrEmpty(oDisChg.ptRefCode))
                                    {
                                        if (oDisChg.ptDisChgType == "1") //ประเภทลดชาร์จ 1:ลดบาท 
                                        {
                                            cDis += (decimal)oDisChg.pcValue;
                                        }

                                        if (oDisChg.ptDisChgType == "2") //ประเภทลดชาร์จ 2: ลด % 
                                        {
                                            cDisPer += (decimal)oDisChg.pcValue;
                                        }

                                        if (oDisChg.ptDisChgType == "3") //ประเภทลดชาร์จ 3: ชาร์จบาท 
                                        {
                                            cChg += (decimal)oDisChg.pcValue;
                                        }

                                        if (oDisChg.ptDisChgType == "4") //ประเภทลดชาร์จ  4: ชาร์จ %
                                        {
                                            cChgPer += (decimal)oDisChg.pcValue;
                                        }
                                    }
                                    else
                                    {
                                        if (oDisChg.ptDisChgType == "5") //ประเภทคูปองส่วนลด  5
                                        {
                                            cChgDisCpn += (decimal)oDisChg.pcValue;
                                        }

                                        if (oDisChg.ptDisChgType == "6") //ประเภทคูปองส่วนลด  6
                                        {
                                            cChgRdmCpn += (decimal)oDisChg.pcValue;
                                        }
                                    }

                                }
                            }
                        }


                        if (cDis > 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                            oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                            oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");
                            cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                            decimal cB4DisChg = cAmount;

                            if ((decimal)(cVB.oVB_Payment.cW_AmtTotalCal - cDis) < (decimal)cVB.cVB_SmallBill)
                            {
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                                return;
                            }

                            //cPayment.C_ADDxDisChgBill(cDis, cDis, cDis.ToString(), cB4DisChg, 1);
                            //cVB.oVB_Payment.W_ADDxDisChgBill("1", cDis.ToString(), cDis);
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("1", cDis.ToString(), cDisPer);
                            //}

                            //*Arm 63-04-10
                            //cPayment.C_ADDxDisChgBill(cDis, cDis, new cSP().SP_SETtDecShwSve(1, (decimal)cDis, cVB.nVB_DecShow).ToString(), cB4DisChg, 1);
                            cPayment.C_ADDxDisChgBill(cDis, cDis, new cSP().SP_SETtDecShwSve(1, (decimal)cDis, cVB.nVB_DecShow).ToString(), (cB4DisChg + cDis), 1); //*Arm 63-09-17
                            cVB.oVB_Payment.W_ADDxDisChgBill("1", new cSP().SP_SETtDecShwSve(1, (decimal)cDis, cVB.nVB_DecShow).ToString(), cDis);
                            //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("1", new cSP().SP_SETtDecShwSve(1, (decimal)cDis, cVB.nVB_DecShow).ToString(), cDisPer);
                            //}
                            //++++++++++++++



                        }

                        if (cDisPer > 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                            oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                            oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");
                            cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                            decimal cB4DisChg = cAmount;

                            if ((decimal)(cVB.oVB_Payment.cW_AmtTotalCal - cDisPer) < (decimal)cVB.cVB_SmallBill)
                            {
                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                                return;
                            }

                            //cPayment.C_ADDxDisChgBill(cDisPer, cDisPer, cDisPer.ToString(), cB4DisChg, 2);
                            //cVB.oVB_Payment.W_ADDxDisChgBill("2", cDisPer.ToString(), cDisPer);
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("2", cDisPer.ToString(), cDisPer);
                            //}

                            //*Arm 63-04-10
                            //cPayment.C_ADDxDisChgBill(cDisPer, cDisPer, new cSP().SP_SETtDecShwSve(1, (decimal)cDisPer, cVB.nVB_DecShow), cB4DisChg, 2);
                            cPayment.C_ADDxDisChgBill(cDisPer, cDisPer, new cSP().SP_SETtDecShwSve(1, (decimal)cDisPer, cVB.nVB_DecShow), (cB4DisChg + cDisPer), 2); //*Arm 63-09-17
                            cVB.oVB_Payment.W_ADDxDisChgBill("2", new cSP().SP_SETtDecShwSve(1, (decimal)cDisPer, cVB.nVB_DecShow), cDisPer);
                            //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("2", new cSP().SP_SETtDecShwSve(1, (decimal)cDisPer, cVB.nVB_DecShow), cDisPer);
                            //}
                            //++++++++++++++
                        }

                        if (cChg > 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                            oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                            oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");
                            cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                            decimal cB4DisChg = cAmount;

                            //cPayment.C_ADDxDisChgBill(cChg, cChg, cChg.ToString(), cB4DisChg, 3);
                            //cVB.oVB_Payment.W_ADDxDisChgBill("3", cChg.ToString(), cChg);
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("3", cChg.ToString(), cChg);
                            //}

                            //*Arm 63-04-10
                            //cPayment.C_ADDxDisChgBill(cChg, cChg, new cSP().SP_SETtDecShwSve(1, (decimal)cChg, cVB.nVB_DecShow), cB4DisChg, 3);
                            cPayment.C_ADDxDisChgBill(cChg, cChg, new cSP().SP_SETtDecShwSve(1, (decimal)cChg, cVB.nVB_DecShow), (cB4DisChg- cChg), 3); //*Arm 63-09-17
                            cVB.oVB_Payment.W_ADDxDisChgBill("3", new cSP().SP_SETtDecShwSve(1, (decimal)cChg, cVB.nVB_DecShow), cChg);
                            //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("3", new cSP().SP_SETtDecShwSve(1, (decimal)cChg, cVB.nVB_DecShow), cChg);
                            //}
                            //+++++++++++++

                        }

                        if (cChgPer > 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                            oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                            oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");
                            cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                            decimal cB4DisChg = cAmount;

                            //cPayment.C_ADDxDisChgBill(cChgPer, cChgPer, cChgPer.ToString(), cB4DisChg, 4);
                            //cVB.oVB_Payment.W_ADDxDisChgBill("4", cChgPer.ToString(), cChgPer);
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("4", cChgPer.ToString(), cChgPer);
                            //}

                            //*Arm 63-04-10
                            //cPayment.C_ADDxDisChgBill(cChgPer, cChgPer, new cSP().SP_SETtDecShwSve(1, (decimal)cChgPer, cVB.nVB_DecShow), cB4DisChg, 4);
                            cPayment.C_ADDxDisChgBill(cChgPer, cChgPer, new cSP().SP_SETtDecShwSve(1, (decimal)cChgPer, cVB.nVB_DecShow), (cB4DisChg- cChgPer), 4); //*Arm 63-09-17
                            cVB.oVB_Payment.W_ADDxDisChgBill("4", new cSP().SP_SETtDecShwSve(1, (decimal)cChgPer, cVB.nVB_DecShow), cChgPer);
                            //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("4", new cSP().SP_SETtDecShwSve(1, (decimal)cChgPer, cVB.nVB_DecShow), cChgPer);
                            //}
                            //+++++++++++++

                        }

                        if (cChgDisCpn > 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                            oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                            oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");
                            cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                            decimal cB4DisChg = cAmount;

                            //cPayment.C_ADDxDisChgBill(cChgDisCpn, cChgDisCpn, cChgDisCpn.ToString(), cB4DisChg, 5);
                            //cVB.oVB_Payment.W_ADDxDisChgBill("5", cChgDisCpn.ToString(), cChgDisCpn);
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("5", cChgDisCpn.ToString(), cChgDisCpn);
                            //}

                            //*Arm 63-04-10
                            //cPayment.C_ADDxDisChgBill(cChgDisCpn, cChgDisCpn, new cSP().SP_SETtDecShwSve(1, (decimal)cChgDisCpn, cVB.nVB_DecShow), cB4DisChg, 5);
                            cPayment.C_ADDxDisChgBill(cChgDisCpn, cChgDisCpn, new cSP().SP_SETtDecShwSve(1, (decimal)cChgDisCpn, cVB.nVB_DecShow), (cB4DisChg + cChgDisCpn), 5);  //*Arm 63-09-17
                            cVB.oVB_Payment.W_ADDxDisChgBill("5", new cSP().SP_SETtDecShwSve(1, (decimal)cChgDisCpn, cVB.nVB_DecShow), cChgDisCpn);
                            //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("5", new cSP().SP_SETtDecShwSve(1, (decimal)cChgDisCpn, cVB.nVB_DecShow), cChgDisCpn);
                            //}
                            //+++++++++++++
                        }

                        if (cChgRdmCpn > 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                            oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                            oSql.AppendLine("WHERE FTXshDocNo='" + cVB.tVB_DocNo + "'");
                            cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                            decimal cB4DisChg = cAmount;

                            //cPayment.C_ADDxDisChgBill(cChgRdmCpn, cChgRdmCpn, cChgRdmCpn.ToString(), cB4DisChg, 6);
                            //cVB.oVB_Payment.W_ADDxDisChgBill("7", cChgRdmCpn.ToString(), cChgRdmCpn);
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("7", cChgRdmCpn.ToString(), cChgRdmCpn);
                            //}

                            //*Arm 63-04-10
                            //cPayment.C_ADDxDisChgBill(cChgRdmCpn, cChgRdmCpn, new cSP().SP_SETtDecShwSve(1, (decimal)cChgRdmCpn, cVB.nVB_DecShow), cB4DisChg, 6);
                            cPayment.C_ADDxDisChgBill(cChgRdmCpn, cChgRdmCpn, new cSP().SP_SETtDecShwSve(1, (decimal)cChgRdmCpn, cVB.nVB_DecShow), (cB4DisChg + cChgRdmCpn), 6); //*Arm 63-09-17
                            cVB.oVB_Payment.W_ADDxDisChgBill("7", new cSP().SP_SETtDecShwSve(1, (decimal)cChgRdmCpn, cVB.nVB_DecShow), cChgRdmCpn);
                            //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                            //if (cVB.oVB_2ndScreen != null)
                            //{
                            //    cVB.oVB_2ndScreen.W_ADDxDisChgBill("7", new cSP().SP_SETtDecShwSve(1, (decimal)cChgRdmCpn, cVB.nVB_DecShow), cChgRdmCpn);
                            //}
                            //+++++++++++++

                        }
                    }

                    new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : AddDisChg Bill ReturnType {cVB.nVB_ReturnType} End", cVB.bVB_AlwPrnLog); //*Net Stamp
                }
                //+++++++++++++++

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPayment", "W_CHKxRefDisChgBill : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
            }
        }


        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {

                // Edit By Zen 63-03-26 
                opnPromo.Visible = false;
                opnPromo.SendToBack();

                // Gridview : Cash
                //ogdCash.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColNormal;
                //ogdProList.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                //oW_SP.SP_SETxSetGridviewFormat(ogdCash); //*Net 63-03-03 Set Design Gridview
                //oW_SP.SP_SETxSetGridviewFormat(ogdProList); //*Net 63-03-03 Set Design Gridview

                oW_SP.SP_SETxSetGridFormat(ogdPmt); //*Em 63-06-01
                ogdPmt.Rows.Count = ogdPmt.Rows.Fixed; //*Em 63-06-01
                oW_SP.SP_SETxSetGridFormat(ogdRcv); //*Em 63-06-01
                ogdRcv.Rows.Count = ogdRcv.Rows.Fixed; //*Em 63-06-01

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                ocmShowNumpad.BackColor = cVB.oVB_ColDark;
                opnCashPay.BackColor = cVB.oVB_ColDark;
                //opnCst.BackColor = cVB.oVB_ColDark;

                opbLogo.Image = new cCompany().C_GEToImageLogo();
                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode, "TCNMUser");

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;

                //*Arm 63-04-28
                if (cSale.nC_DocType == 9)
                {
                    olaDocNo.ForeColor = Color.Red;
                    opbDocNo.Visible = false;
                }
                else
                {
                    olaDocNo.ForeColor = Color.Black;
                    opbDocNo.Visible = false;
                }
                //+++++++++++++
                //if (oW_SP.SP_CHKbConnection())
                //    opbPOS.Image = Properties.Resources.Online_32;
                //else
                //    opbPOS.Image = Properties.Resources.Offline_32;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                cVB.tVB_KbdScreen = "PAYMENT";

                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPayment_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPayment_EN));
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
                //ocmPdtDetail.Text = "".PadLeft(10) + oW_Resource.GetString("tPdtDetail");

                cVB.cVB_RoundDiff = cSP.SP_CALcRoundDiff(cVB.cVB_Amount);   //*Em 62-10-08
                cW_AmtTotalShw = cVB.cVB_Amount + cVB.cVB_RoundDiff;    //*Em 62-10-08
                cW_AmtTotalCal = cVB.cVB_Amount;    //*Em 62-10-08
                cW_AmtTotalPay = 0; //*Em 62-10-08
                cW_DisChgAmt = 0;   //*Em 62-10-09

                ogdSum.Rows.Add(cVB.oVB_GBResource.GetString("tTotal"), oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalCal, cVB.nVB_DecShow)); //*Net 63-07-31 เปลี่ยนจาก fix 2 เป็น cVB.nVB_DecShow
                ogdSum.Rows.Add(oW_Resource.GetString("tBillDis"), oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow)); //*Net 63-07-31 เปลี่ยนจาก fix 2 เป็น cVB.nVB_DecShow
                ogdSum.Rows.Add(oW_Resource.GetString("tNetSale"), oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalCal, cVB.nVB_DecShow)); //*Net 63-07-31 เปลี่ยนจาก fix 2 เป็น cVB.nVB_DecShow
                ogdSum.Rows.Add(oW_Resource.GetString("tAmtTendered"), oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow)); //*Net 63-07-31 เปลี่ยนจาก fix 2 เป็น cVB.nVB_DecShow
                ogdSum.Rows.Add(oW_Resource.GetString("tCashRnd"), oW_SP.SP_SETtDecShwSve(1, cVB.cVB_RoundDiff, cVB.nVB_DecShow)); //*Net 63-07-31 เปลี่ยนจาก fix 2 เป็น cVB.nVB_DecShow
                ogdSum.Rows.Add(oW_Resource.GetString("tCashPayment"), oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow)); //*Net 63-07-31 เปลี่ยนจาก fix 2 เป็น cVB.nVB_DecShow

                olaTitleCashPay.Text = oW_Resource.GetString("tCashPayment");
                // olaTitleAmt.Text = cVB.oVB_GBResource.GetString("tAmount");
                olaPayment.Text = cVB.oVB_GBResource.GetString("tPayment");
                //olaTitlePoint.Text = cVB.oVB_GBResource.GetString("tPoint") + " : ";
                //olaTitleGrpCst.Text = cVB.oVB_GBResource.GetString("tGrpCst") + " : ";
                //olaTitleCstExp.Text = cVB.oVB_GBResource.GetString("tExpire") + " : ";
                // olaTitleQuickAmt.Text = cVB.oVB_GBResource.GetString("tQuickAmt");

                //otbTitleCshSeq.HeaderText = cVB.oVB_GBResource.GetString("tSeq");
                //otbTitleCshDetail.HeaderText = cVB.oVB_GBResource.GetString("tDetail");
                //otbTitleCshAmt.HeaderText = cVB.oVB_GBResource.GetString("tAmount");

                // User
                olaUsrName.Text = new cUser().C_GETtUsername();
                olaPos.Text = cVB.tVB_PosCode;
                olaSaleDate.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");
                cVB.cVB_RoundDiff = cSP.SP_CALcRoundDiff(cVB.cVB_Amount);    //*Em 62-10-08
                olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_Amount + cVB.cVB_RoundDiff, cVB.nVB_DecShow);
                otbAmount.Text = olaCashPayment.Text;


                //*Net 63-07-31 sync ยอดชำระกับหน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                    cVB.oVB_CstScreen.W_SETxSummaryAmt(olaCashPayment.Text);
                }
                //++++++++++++++++++++++++++++++++++

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Get payment type
        /// </summary>
        private void W_GETxPaymentType()
        {
            List<cmlTPSMFunc> aoFunc;
            //int nRow = 0, nCol = 0;
            //int nW_PgpChain = 0;
            //int? nRowCount = 0;
            //double cMaxPage;
            int[,] anTblTmp;
            int nFuncIndex;
            try
            {
                opnPaymentType.Controls.Clear();

                //if (string.Equals(cVB.tVB_PosType, "1"))     // 1:Store, 2:Cashier
                //    //*Net 63-04-13 get มาหมดเลย
                //    aoFunc = W_GEToDisChgPage("031", 0, 100);//new cFunctionKeyboard().C_GETaFuncList("031");
                //else
                //    aoFunc = new cFunctionKeyboard().C_GETaFuncList("032");

                aoFunc = cVB.oVB_PayType;   //*Em 63-04-25

                nW_MaxPage = 0;
                nFuncIndex = 0;

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Cal Max Page PaymentType", cVB.bVB_AlwPrnLog); //*Net Stamp
                while (nFuncIndex < aoFunc.Count)
                {
                    anTblTmp = new int[opnPaymentType.RowStyles.Count, opnPaymentType.ColumnStyles.Count];

                    if (nW_MaxPage + 1 == nW_CurPage)
                        nW_StartRow = nFuncIndex;

                    nW_MaxPage++;
                    for (int nTblRow = 0; nTblRow < anTblTmp.GetLength(0); nTblRow++)
                    {
                        for (int nTblCol = 0; nTblCol < anTblTmp.GetLength(1); nTblCol++)
                        {
                            bool bHasControl = false;
                            if (!aoFunc[nFuncIndex].FNGdtBtnSizeY.HasValue &&
                               !aoFunc[nFuncIndex].FNGdtBtnSizeX.HasValue)
                            {
                                nFuncIndex++;
                                break;
                            }

                            for (int nOcmRow = 0; nOcmRow < aoFunc[nFuncIndex].FNGdtBtnSizeY; nOcmRow++)
                            {
                                for (int nOcmCol = 0; nOcmCol < aoFunc[nFuncIndex].FNGdtBtnSizeX; nOcmCol++)
                                {
                                    //if (nTblCol + nOcmCol > anTblTmp.GetLength(1) || nTblRow + nOcmRow > anTblTmp.GetLength(0))
                                    if (nTblCol + nOcmCol > anTblTmp.GetLength(1) - 1 || nTblRow + nOcmRow > anTblTmp.GetLength(0) - 1)     //*Em 63-04-29
                                    {
                                        bHasControl = true;
                                        nOcmCol = aoFunc[nFuncIndex].FNGdtBtnSizeX.Value;
                                        nOcmRow = aoFunc[nFuncIndex].FNGdtBtnSizeY.Value;
                                        break;
                                    }
                                    if (anTblTmp[nTblRow + nOcmRow, nTblCol + nOcmCol] == 1)
                                    {
                                        bHasControl = true;
                                        nOcmCol = aoFunc[nFuncIndex].FNGdtBtnSizeX.Value;
                                        nOcmRow = aoFunc[nFuncIndex].FNGdtBtnSizeY.Value;
                                        break;
                                    }
                                }
                            }

                            if (bHasControl == false)
                            {
                                for (int nSpanRow = 0; nSpanRow < aoFunc[nFuncIndex].FNGdtBtnSizeY.Value; nSpanRow++)
                                {
                                    for (int nSpanCol = 0; nSpanCol < aoFunc[nFuncIndex].FNGdtBtnSizeX.Value; nSpanCol++)
                                    {
                                        anTblTmp[nTblRow + nSpanRow, nTblCol + nSpanCol] = 1;
                                    }
                                }
                                nTblCol += aoFunc[nFuncIndex].FNGdtBtnSizeX.Value - 1;
                                nFuncIndex++;
                                if (nFuncIndex == aoFunc.Count)
                                {
                                    nTblRow = anTblTmp.GetLength(0);
                                    nTblCol = anTblTmp.GetLength(1);
                                    break;
                                }
                            }
                        }
                    }
                }
                opnPaymentType.Controls.Clear();
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Add Payment Button Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                for (int nTblRow = 0; nTblRow < opnPaymentType.RowStyles.Count; nTblRow++)
                {
                    for (int nTblCol = 0; nTblCol < opnPaymentType.ColumnStyles.Count; nTblCol++)
                    {
                        bool bHasControl = false;
                        if (!aoFunc[nW_StartRow].FNGdtBtnSizeY.HasValue &&
                           !aoFunc[nW_StartRow].FNGdtBtnSizeX.HasValue)
                        {
                            nW_StartRow++;
                            break;
                        }

                        for (int nOcmRow = 0; nOcmRow < aoFunc[nW_StartRow].FNGdtBtnSizeY; nOcmRow++)
                        {
                            for (int nOcmCol = 0; nOcmCol < aoFunc[nW_StartRow].FNGdtBtnSizeX; nOcmCol++)
                            {
                                if (nTblCol + nOcmCol > opnPaymentType.ColumnCount - 1 || nTblRow + nOcmRow > opnPaymentType.ColumnCount - 1)
                                {
                                    bHasControl = true;
                                    nOcmCol = aoFunc[nW_StartRow].FNGdtBtnSizeX.Value;
                                    nOcmRow = aoFunc[nW_StartRow].FNGdtBtnSizeY.Value;
                                    break;
                                }
                                if (opnPaymentType.GetControlFromPosition(nTblCol + nOcmCol, nTblRow + nOcmRow) != null)
                                {
                                    bHasControl = true;
                                    nOcmCol = aoFunc[nW_StartRow].FNGdtBtnSizeX.Value;
                                    nOcmRow = aoFunc[nW_StartRow].FNGdtBtnSizeY.Value;
                                }
                            }
                        }

                        if (bHasControl == false)
                        {
                            Button ocmPaymentType = new Button();
                            ocmPaymentType.Name = "ocm-" + aoFunc[nW_StartRow].FTSysCode;
                            ocmPaymentType.Tag = aoFunc[nW_StartRow].FTGdtCallByName;
                            ocmPaymentType.FlatStyle = FlatStyle.Flat;
                            ocmPaymentType.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                            ocmPaymentType.BackColor = (aoFunc[nW_StartRow].FTGdtSysUse == "1") ? cVB.oVB_ColNormal : cVB.oVB_ColLight;
                            ocmPaymentType.ForeColor = Color.White;
                            ocmPaymentType.Font = new Font(new FontFamily("Segoe UI Semibold"), 14f, FontStyle.Bold);
                            ocmPaymentType.Margin = new Padding(2);
                            ocmPaymentType.Text = aoFunc[nW_StartRow].FTGdtName;
                            ocmPaymentType.Click += ocmPaymentType_Click;
                            ocmPaymentType.KeyDown += ocmMenu_KeyDown;
                            ocmPaymentType.Enabled = (aoFunc[nW_StartRow].FTGdtSysUse == "1");
                            opnPaymentType.Controls.Add(ocmPaymentType, nTblCol, nTblRow);
                            opnPaymentType.SetColumnSpan(ocmPaymentType, aoFunc[nW_StartRow].FNGdtBtnSizeX.Value);
                            opnPaymentType.SetRowSpan(ocmPaymentType, aoFunc[nW_StartRow].FNGdtBtnSizeY.Value);
                            ocmPaymentType = null;
                            nW_StartRow++;
                            if (nW_StartRow == aoFunc.Count)
                            {
                                nTblRow = opnPaymentType.RowStyles.Count;
                                nTblCol = opnPaymentType.ColumnStyles.Count;
                                break;
                            }
                        }
                    }
                }

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Add Payment Button End ", cVB.bVB_AlwPrnLog); //*Net Stamp

                /*foreach (cmlTPSMFunc oFunc in aoFunc)
                {
                    nW_StartRow++;
                    //n_StartRow = n_StartRow + oFunc.FNGdtBtnSizeY.Value;
                    if (nRow <= 4)
                    {
                        Button ocmPaymentType = new Button();
                        ocmPaymentType.Name = "ocm-" + oFunc.FTSysCode;
                        ocmPaymentType.Tag = oFunc.FTGdtCallByName;
                        ocmPaymentType.FlatStyle = FlatStyle.Flat;
                        ocmPaymentType.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        ocmPaymentType.BackColor = (oFunc.FTGdtSysUse == "1") ? cVB.oVB_ColNormal : cVB.oVB_ColLight;
                        ocmPaymentType.ForeColor = Color.White;
                        ocmPaymentType.Font = new Font(new FontFamily("Segoe UI Semibold"), 14f, FontStyle.Bold);
                        ocmPaymentType.Margin = new Padding(2);
                        ocmPaymentType.Text = oFunc.FTGdtName;
                        ocmPaymentType.Click += ocmPaymentType_Click;
                        ocmPaymentType.KeyDown += ocmMenu_KeyDown;
                        ocmPaymentType.Enabled = (oFunc.FTGdtSysUse == "1");
                        opnPaymentType.Controls.Add(ocmPaymentType, nCol, nRow);
                        opnPaymentType.SetColumnSpan(ocmPaymentType, oFunc.FNGdtBtnSizeX.Value);
                        opnPaymentType.SetRowSpan(ocmPaymentType, oFunc.FNGdtBtnSizeY.Value);
                        ocmPaymentType = null;

                        nCol++;

                        if (nCol >= opnPaymentType.ColumnStyles.Count)
                        {
                            nRow++;
                            nCol = 0;
                        }
                    }

                }*/

                /*if (aoFunc.Count > 0)
                    nRowCount = aoFunc[0].nRowCount;

                //cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / cVB.nVB_MaxData);
                cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / 8); // *Arm 62-11-15 [ปรับ]


                nW_MaxPage = Convert.ToInt32(cMaxPage);// *Arm 62-10-16 - จำนวนหน้าทั้งหมด
                //nW_TotalCount = ((nW_CurPage - 1) * cVB.nVB_MaxData) + aoPdt.Count;// *Arm 62-10-16 จำนวนต่อหน้า

                if (cMaxPage == 0)
                {
                    cMaxPage = 1;
                }*/


                if (opnPaymentType.RowCount > 0)
                {
                    for (int nCount = 0; nCount < opnPaymentType.RowStyles.Count; nCount++)
                    {
                        opnPaymentType.RowStyles[nCount].SizeType = SizeType.Percent;
                        opnPaymentType.RowStyles[nCount].Height = 100 / opnPaymentType.RowStyles.Count;
                    }
                }
                olaPagePdt.Text = string.Format(cVB.oVB_GBResource.GetString("tPagePay"), nW_CurPage, nW_MaxPage);

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_GETxPaymentType : " + oEx.Message); }
            finally
            {
                aoFunc = null;
                anTblTmp = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Discount Charge
        /// </summary>
        private void W_GETxDisChg()
        {
            List<cmlTPSMFunc> aoFunc;
            int nRow = 0, nCol = 0;

            int tW_PgpChain = 4;
            try
            {
                //opnDisChg.Controls.Clear();

                if (string.Equals(cVB.tVB_PosType, "1"))     // 1:Store, 2:Cashier
                    aoFunc = W_GEToDisChgPage("015", nW_StartRow, tW_PgpChain);//new cFunctionKeyboard().C_GETaFuncList("015");
                else
                    aoFunc = new cFunctionKeyboard().C_GETaFuncList("016");

                foreach (cmlTPSMFunc oFunc in aoFunc)
                {
                    //if (aoFunc.Count <= 4)
                    //{
                    Button ocmDisChg = new Button();
                    ocmDisChg.Name = "ocm-" + oFunc.FTSysCode;
                    ocmDisChg.Tag = oFunc.FTGdtCallByName;
                    ocmDisChg.FlatStyle = FlatStyle.Flat;
                    ocmDisChg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    ocmDisChg.BackColor = cVB.oVB_ColNormal;
                    ocmDisChg.ForeColor = Color.White;
                    ocmDisChg.Font = new Font(new FontFamily("Segoe UI Semibold"), 14f, FontStyle.Bold);
                    ocmDisChg.Margin = new Padding(2);
                    ocmDisChg.Text = oFunc.FTGdtName;
                    ocmDisChg.Click += ocmDisChg_Click;
                    ocmDisChg.KeyDown += ocmMenu_KeyDown;
                    //opnDisChg.Controls.Add(ocmDisChg, nCol, nRow);
                    //opnDisChg.SetColumnSpan(ocmDisChg, oFunc.FNGdtBtnSizeX.Value);
                    //opnDisChg.SetRowSpan(ocmDisChg, oFunc.FNGdtBtnSizeY.Value);
                    ocmDisChg = null;

                    nCol++;

                    if (nCol >= 2) //opnPaymentType.ColumnStyles.Count
                    {
                        nRow++;
                        nCol = 0;
                    }

                    //}
                    //else
                    //{
                    //    aoFunc = W_GEToDisChgPage("015", nW_StartRow, tW_PgpChain);
                    //}

                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_GETxDisChg : " + oEx.Message); }
            finally
            {
                aoFunc = null;
                //oW_SP.SP_CLExMemory();
            }
        }


        private List<cmlTPSMFunc> W_GEToDisChgPage(string ptGhdCode, int pnStartRow, int pnPgpChain)
        {
            List<cmlTPSMFunc> aoFunc = new List<cmlTPSMFunc>();
            StringBuilder oSql;

            try
            {
                switch (ptGhdCode)
                {
                    case "031":
                    case "032":
                        oSql = new StringBuilder();
                        switch (cVB.tVB_PosType)
                        {
                            case "1": // Store
                                oSql.AppendLine("DECLARE @ptAppCode varchar(5) = 'PS';");
                                break;

                            case "2": // Cashier
                                oSql.AppendLine("DECLARE @ptAppCode varchar(5) = 'FC';");
                                break;
                        }
                        oSql.AppendLine($"DECLARE @ptBchCode varchar(5) = '{cVB.tVB_BchCode}';");
                        oSql.AppendLine($"WITH RCV AS ");
                        oSql.AppendLine($"(");
                        oSql.AppendLine($"SELECT * FROM TFNMRcvSpc WITH(NOLOCK) WHERE FTAppCode=@ptAppCode AND FTBchCode=@ptBchCode");
                        oSql.AppendLine($"),");
                        oSql.AppendLine($"RCVA AS");
                        oSql.AppendLine($"(");
                        oSql.AppendLine($"SELECT * FROM RCV");
                        oSql.AppendLine($"UNION");
                        oSql.AppendLine($"SELECT * FROM TFNMRcvSpc WITH(NOLOCK)");
                        oSql.AppendLine($"WHERE  FTAppCode=@ptAppCode AND FTRcvCode NOT IN (SELECT RCV.FTRcvCode FROM RCV) AND ISNULL(FTBchCode,'')=''");
                        oSql.AppendLine($")");
                        //++++++++++++++++++++++++++++++++++++++++++++++++++++
                        oSql.AppendLine("SELECT DISTINCT Count(*) OVER(PARTITION BY 1 ) AS nRowCount,* FROM (");
                        oSql.AppendLine("SELECT DISTINCT FDT.FNGdtUsrSeq,FDT.FTSysCode, FDT.FNGdtPage, FDT.FNGdtBtnSizeX, FDT.FNGdtBtnSizeY, ");
                        oSql.AppendLine("       FDT.FTGdtCallByName, FDTL.FTGdtName, FDT.FTGdtSysUse, FHD.FTKbdScreen, FKB.FTSysKeyFunc");
                        oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TPSMFuncDT_L FDTL  WITH(NOLOCK) ON FDTL.FTGhdCode = FDT.FTGhdCode ");
                        oSql.AppendLine("   AND FDTL.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("   AND FDTL.FNLngID = " + cVB.nVB_Language);
                        oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                        oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("WHERE FDT.FTGdtStaUse = '1'");

                        switch (cVB.tVB_PosType)
                        {
                            case "1": // Store
                                oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                                break;

                            case "2": // Cashier
                                oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                                break;
                        }

                        oSql.AppendLine("AND FHD.FTGhdCode = '015'");
                        oSql.AppendLine("UNION");
                        oSql.AppendLine("SELECT DISTINCT FDT.FNGdtUsrSeq,RCV.FTRcvCode + '-' + RCV.FTFmtCode + '-' + RCVA.FTAppStaAlwRet + '-' + RCVA.FTAppStaAlwCancel  + '-' + RCVA.FTAppStaPayLast AS FTSysCode, FDT.FNGdtPage, FDT.FNGdtBtnSizeX, FDT.FNGdtBtnSizeY, ");
                        oSql.AppendLine("       FDT.FTGdtCallByName, ISNULL(RCVL.FTRcvName,(SELECT TOP 1 FTRcvName FROM TFNMRcv_L WITH(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode)) AS FTGdtName, FDT.FTGdtSysUse, FHD.FTKbdScreen, FKB.FTSysKeyFunc");
                        oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                        oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("INNER JOIN TSysRcvFmt RCVF WITH(NOLOCK) ON FDT.FTGdtCallByName = RCVF.FTFmtRef");
                        oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RCVF.FTFmtCode = RCV.FTFmtCode AND RCV.FTRcvStaUse = '1'");
                        oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);

                        switch (cVB.tVB_PosType)
                        {
                            case "1": // Store
                                      //oSql.AppendLine("INNER JOIN TSysRcvApp RCVA WITH(NOLOCK) ON RCVF.FTFmtCode = RCVA.FTFmtCode AND RCVA.FTAppCode = 'PS'");
                                      //*Em 63-01-10
                                      //oSql.AppendLine("INNER JOIN TFNMRcvSpc RCVA WITH(NOLOCK) ON RCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'PS'");
                                oSql.AppendLine("INNER JOIN RCVA WITH(NOLOCK) ON RCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'PS'");
                                //oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = '" + cVB.tVB_BchCode + "')");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = @ptBchCode)");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTMerCode,'') = '' OR ISNULL(RCVA.FTMerCode,'') = '" + cVB.tVB_Merchart + "')");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTShpCode,'') = '' OR ISNULL(RCVA.FTShpCode,'') = '" + cVB.tVB_ShpCode + "')");
                                //++++++++++++++++++
                                oSql.AppendLine("WHERE FDT.FTGdtStaUse = '1'");
                                oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                                if (cSale.nC_DocType == 9)
                                {
                                    oSql.AppendLine("AND RCVA.FTAppStaAlwRet = '1'");
                                }
                                break;

                            case "2": // Cashier
                                      //oSql.AppendLine("INNER JOIN TSysRcvApp RCVA WITH(NOLOCK) ON RCVF.FTFmtCode = RCVA.FTFmtCode AND RCVA.FTAppCode = 'FC'");
                                      //*Em 63-01-10
                                      //oSql.AppendLine("INNER JOIN TFNMRcvSpc RCVA WITH(NOLOCK) ON RCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'FC'");
                                oSql.AppendLine("INNER JOIN RCVA WITH(NOLOCK) ON RCV.FTRcvCode = RCVA.FTRcvCode AND RCVA.FTAppCode = 'FC'");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTBchCode,'') = '' OR ISNULL(RCVA.FTBchCode,'') = '" + cVB.tVB_BchCode + "')");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTMerCode,'') = '' OR ISNULL(RCVA.FTMerCode,'') = '" + cVB.tVB_Merchart + "')");
                                oSql.AppendLine("AND (ISNULL(RCVA.FTShpCode,'') = '' OR ISNULL(RCVA.FTShpCode,'') = '" + cVB.tVB_ShpCode + "')");
                                //++++++++++++++++++
                                oSql.AppendLine("WHERE FDT.FTGdtStaUse = '1'");
                                oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                                if (cSale.nC_DocType == 9)
                                {
                                    oSql.AppendLine("AND RCVA.FTAppStaAlwRet = '1'");
                                }
                                break;
                        }

                        oSql.AppendLine("AND FHD.FTGhdCode = '" + ptGhdCode + "') TPAY");
                        oSql.AppendLine("ORDER BY TPAY.FNGdtUsrSeq");
                        oSql.AppendLine("OFFSET " + pnStartRow + " ROWS");
                        oSql.AppendLine("FETCH NEXT  " + pnPgpChain + "  ROWS ONLY;");
                        break;
                    default:
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT DISTINCT FDT.FTSysCode, FDT.FNGdtPage, FDT.FNGdtBtnSizeX, FDT.FNGdtBtnSizeY, ");
                        oSql.AppendLine("       FDT.FTGdtCallByName, FDTL.FTGdtName, FHD.FTKbdScreen, FKB.FTSysKeyFunc");
                        oSql.AppendLine("FROM TPSMFuncDT FDT WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TPSMFuncDT_L FDTL  WITH(NOLOCK) ON FDTL.FTGhdCode = FDT.FTGhdCode ");
                        oSql.AppendLine("   AND FDTL.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("   AND FDTL.FNLngID = " + cVB.nVB_Language);
                        oSql.AppendLine("INNER JOIN TPSMFuncHD FHD WITH(NOLOCK) ON FHD.FTGhdCode = FDT.FTGhdCode");
                        oSql.AppendLine("INNER JOIN TSysFuncKB FKB WITH(NOLOCK) ON FKB.FTSysCode = FDT.FTSysCode");
                        oSql.AppendLine("WHERE FDT.FTGdtStaUse = '1'");

                        switch (cVB.tVB_PosType)
                        {
                            case "1": // Store
                                oSql.AppendLine("AND FHD.FTGhdApp = 'PS'");
                                break;

                            case "2": // Cashier
                                oSql.AppendLine("AND FHD.FTGhdApp = 'FC'");
                                break;
                        }

                        oSql.AppendLine("AND FHD.FTGhdCode = '" + ptGhdCode + "'");
                        oSql.AppendLine("ORDER BY FNGdtUsrSeq");
                        oSql.AppendLine("OFFSET " + pnStartRow + " ROWS");
                        oSql.AppendLine("FETCH NEXT  " + pnPgpChain + "  ROWS ONLY;");


                        break;
                }
                aoFunc = new cDatabase().C_GETaDataQuery<cmlTPSMFunc>(oSql.ToString());
            }
            catch (Exception oEx)
            {

            }

            return aoFunc;
        }
        /// <summary>
        /// Quick amount
        /// </summary>
        private void W_GETxQuickAmount()
        {
            List<cmlTFNMBankNote> aoBanknote;

            string tImagePath = string.Empty;

            try
            {
                //aoBanknote = new cBankNote().C_GETaBanknote();
                aoBanknote = cVB.oVB_QuickAmt;  //*Em 63-04-25
                tImagePath = Directory.GetParent(Application.StartupPath) + @"\AdaImage\Others\BankNote";
                if (Directory.Exists(tImagePath))
                {

                    foreach (cmlTFNMBankNote oBanknote in aoBanknote)
                    {
                        Button ocmBanknote = new Button();

                        //เช็คเงื่อนไขว่า ถ้ามี Path รูป ให้เอารูปใส่ ถ้าไม่มี ให้เอา Text ใส่

                        if (File.Exists(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg"))
                        {
                            //*Net 63-07-31 ป้องกันการล๊อคไฟล์รูปภาพ
                            using (Image oImg = Image.FromFile(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg"))
                            {
                                //ocmBanknote.BackgroundImage = Image.FromFile(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg");
                                ocmBanknote.BackgroundImage = new Bitmap(oImg);
                                ocmBanknote.BackgroundImageLayout = ImageLayout.Stretch;
                                ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                            }
                        }
                        else
                        {
                            ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                            ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        }
                        #region เก็บ Comment ไว้กรณี มีการแก้ไข By Zen 29-04-2020
                        //switch (oBanknote.FCBntRateAmt.Value.ToString())
                        //{
                        //    case "1000.00":
                        //        if (File.Exists(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg"))
                        //        {
                        //            ocmBanknote.BackgroundImage = Image.FromFile(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg");
                        //            ocmBanknote.BackgroundImageLayout = ImageLayout.Stretch;
                        //        }
                        //        else
                        //        {                                    
                        //            ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //            ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //        }                                
                        //        break;
                        //    case "500.00":
                        //        if (File.Exists(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg"))
                        //        {
                        //            ocmBanknote.BackgroundImage = Image.FromFile(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg");
                        //            ocmBanknote.BackgroundImageLayout = ImageLayout.Stretch;
                        //        }
                        //        else
                        //        {
                        //            ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //            ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //        }
                        //        break;
                        //    case "100.00":
                        //        if (File.Exists(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg"))
                        //        {
                        //            ocmBanknote.BackgroundImage = Image.FromFile(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg");
                        //            ocmBanknote.BackgroundImageLayout = ImageLayout.Stretch;
                        //        }
                        //        else
                        //        {
                        //            ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //            ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //        }
                        //        break;
                        //    case "50.00":
                        //        if (File.Exists(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg"))
                        //        {
                        //            ocmBanknote.BackgroundImage = Image.FromFile(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg");
                        //            ocmBanknote.BackgroundImageLayout = ImageLayout.Stretch;
                        //        }
                        //        else
                        //        {
                        //            ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //            ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //        }
                        //        break;
                        //    case "20.00":
                        //        if (File.Exists(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg"))
                        //        {
                        //            ocmBanknote.BackgroundImage = Image.FromFile(tImagePath + @"\" + oBanknote.FTBntCode + ".jpg");
                        //            ocmBanknote.BackgroundImageLayout = ImageLayout.Stretch;
                        //        }
                        //        else
                        //        {
                        //            ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //            ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //        }
                        //        break;
                        //}
                        #endregion


                        ocmBanknote.Name = "ocm-" + oBanknote.FTBntCode;
                        // ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        //ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        ocmBanknote.FlatStyle = FlatStyle.Flat;
                        ocmBanknote.FlatAppearance.BorderSize = 0;
                        ocmBanknote.BackColor = cVB.oVB_ColNormal;
                        ocmBanknote.Margin = new Padding(0, 0, 10, 10);
                        ocmBanknote.ForeColor = Color.White;
                        ocmBanknote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        ocmBanknote.Font = new Font(new FontFamily("Segoe UI Light"), 16f);
                        ocmBanknote.Click += ocmBanknote_Click;
                        opnQuick.Controls.Add(ocmBanknote);
                        ocmBanknote = null;
                    }
                }
                else
                {
                    foreach (cmlTFNMBankNote oBanknote in aoBanknote)
                    {
                        Button ocmBanknote = new Button();
                        ocmBanknote.Name = "ocm-" + oBanknote.FTBntCode;
                        ocmBanknote.AccessibleDescription = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow); //*Arm 63-03-31
                        ocmBanknote.Text = oW_SP.SP_SETtDecShwSve(1, oBanknote.FCBntRateAmt.Value, cVB.nVB_DecShow);
                        ocmBanknote.FlatStyle = FlatStyle.Flat;
                        ocmBanknote.FlatAppearance.BorderSize = 0;
                        ocmBanknote.BackColor = cVB.oVB_ColNormal;
                        ocmBanknote.Margin = new Padding(0, 0, 10, 10);
                        ocmBanknote.ForeColor = Color.White;
                        ocmBanknote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        ocmBanknote.Font = new Font(new FontFamily("Segoe UI Light"), 12f);
                        ocmBanknote.Click += ocmBanknote_Click;
                        opnQuick.Controls.Add(ocmBanknote);
                        ocmBanknote = null;
                    }
                }

                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + " : Get Quick Amt End", cVB.bVB_AlwPrnLog); //*Net Stamp

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_GETxQuickAmount : " + oEx.Message); }
            finally
            {
                aoBanknote = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process Back
        /// </summary>
        private void W_PRCxBack()
        {
            try
            {
                switch (nW_Mode)
                {
                    case 1:     // Topup
                        new wTopUp().Show();
                        break;

                    case 2:     // Cancel Topup
                        new wCancelTopUp().Show();
                        break;

                    case 3:     // Sale
                        ////*Em 63-05-06
                        //if (cVB.bVB_PriceConfirm)
                        //{
                        //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNotBackSale"), 3);
                        //    return;
                        //}
                        ////++++++++++++++++++

                        //new wSale(nW_Mode).Show();
                        if (cSale.nC_DocType == 1) if (cPayment.C_PRCbCancelCoupon(1, cVB.tVB_DocNo) == false) return; //*Em 63-01-09  
                        cSale.C_PRCxClearCallBack();    //*Em 62-10-09

                        cVB.nVB_CstPoint = cVB.nVB_CstPiontB4Used; //*Arm 63-03-11 คืนค่า point
                        cSale.nC_RDSeqNo = 0; //*Arm 63-03-11 คืนค่า Seq SalRD

                        //cVB.tVB_KbdScreen = "SALE";

                        //*Em 63-04-23
                        switch (cVB.nVB_SaleModeStd)
                        {
                            case 1:
                                cVB.tVB_KbdScreen = "SALESTD";
                                break;
                            case 2:
                                cVB.tVB_KbdScreen = "SALE";
                                break;
                        }
                        //++++++++++++++++++++++++++

                        //*Net 63-07-08 ปรับหน้าจอ 2 ใหม่
                        //if (cVB.oVB_2ndScreen != null) //*Net 63-04-21
                        //{
                        //    cVB.oVB_2ndScreen.W_PRCxBack();
                        //    cVB.oVB_2ndScreen.Update();
                        //}
                        nw_ModeClose = 1;
                        new wPmtGetorSug().C_PRCxClearExc();

                        if (cVB.bVB_PriceConfirm) cVB.oVB_Sale.W_PRCxPriceConfirm();    //*Em 63-05-07

                        if (cVB.bVB_PmtPriGrp)
                        {
                            cVB.oVB_Sale.bW_CalPmtPrice = true; //*Em 63-05-05
                            Thread oCalPro = new Thread(cVB.oVB_Sale.W_PRCxCoPmt);
                            oCalPro.IsBackground = true;
                            oCalPro.Priority = ThreadPriority.Highest;
                            oCalPro.Start();
                        }

                        //cVB.oVB_Sale.Show();
                        this.Close();
                        break;

                    case 4:     // Return Sale
                        new wReturn().Show();
                        break;

                    case 5:     // Rental 
                        new wSale(nW_Mode).Show();
                        break;

                    case 6:     // Return Rental
                        new wReturn().Show();
                        break;

                    case 7:     // Ticket
                        new wTicket().Show();
                        break;
                }

                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_PRCxBack : " + oEx.Message); }
        }



        /// <summary>
        /// Set button bar 
        /// </summary>
        private void W_SHWxButtonBar()
        {
            List<cmlTPSMFunc> aoKb;
            List<cmlTPSMFunc> aoMenuT;  //*Em 62-01-24  Waterpark
            List<cmlTPSMFunc> aoMenuB;  //*Em 62-01-24  Waterpark
            int nItem;  //*Em 62-01-24  Waterpark
            try
            {
                //aoKb = new cFunctionKeyboard().C_GETaMenuBar(cVB.tVB_KbdScreen);
                aoKb = cVB.oVB_PayMenuBar;  //*Em 63-04-25
                aoKb = (from oBar in aoKb where oBar.FNLngID == cVB.nVB_Language select oBar).ToList();

                //foreach (cmlTPSMFunc oKb in aoKb)
                //{
                //    switch (oKb.FTSysCode)
                //    {
                //        case "KB010":
                //            ocmHelp.Visible = true;
                //            ocmHelp.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB022":
                //            ocmShwKb.Visible = true;
                //            ocmShwKb.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB027":
                //            ocmCalculate.Visible = true;
                //            ocmCalculate.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB046":
                //            ocmKB.Visible = true;
                //            ocmKB.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB047":
                //            ocmAbout.Visible = true;
                //            ocmAbout.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB003":
                //            ocmBack.Visible = true;
                //            ocmBack.Text = "".PadLeft(10) + oKb.FTGdtName;
                //            break;

                //        case "KB024":
                //            ocmPdtDetail.Visible = true;
                //            ocmPdtDetail.Text = "".PadLeft(10) + oKb.FTGdtName;

                //            switch (nW_Mode)
                //            {
                //                case 1:
                //                case 2:
                //                    ocmPdtDetail.Enabled = false;
                //                    break;
                //            }
                //            break;
                //    }
                //}

                //*Em 62-01-22  Waterpark
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
                        ocmMenu.Width = 249;
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
                        ocmMenu.Width = 249;
                        opnMenuB.Controls.Add(ocmMenu, 1, nItem);
                        opnMenuB.RowStyles[nItem].SizeType = SizeType.Absolute;
                        opnMenuB.RowStyles[nItem].Height = 55;
                        nItem = nItem + 1;
                    }
                }
                //++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get function by name
        /// </summary>
        private void W_GETxFuncByFuncName(string ptFuncName)
        {
            try
            {
                switch (ptFuncName)
                {
                    case "C_KBDxBack":
                        W_PRCxBack();
                        break;

                    case "C_KBDxNotify":
                        W_CHKxNotify();
                        break;

                    case "C_KBDxInputAmount":
                        otbAmount.Focus();
                        break;

                    case "C_KBDxPdtDetail":
                        //if (ocmPdtDetail.Enabled)
                        //{

                        //}
                        break;
                }

                W_PRCxPayment(ptFuncName);
                W_PRCxDisChg(ptFuncName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_GETxFuncByFuncName : " + oEx.Message); }
        }

        /// <summary>
        /// Check Message
        /// </summary>
        /// <param name="ptMessage"></param>
        private void W_CHKxMsg(string ptMessage)
        {
            try
            {
                switch (ptMessage)
                {
                    case "AdaPosFront|MsgRemind":
                        Invoke(new Action(() =>
                        {
                            W_GETxCountNotify();
                        }));
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_CHKxMsg : " + oEx.Message); }
        }

        /// <summary>
        /// Get Notify
        /// </summary>
        private void W_GETxCountNotify()
        {
            List<cmlTCNTMsgRemind> aoMsgRemind;
            int nCountMsg;

            try
            {
                nCountMsg = new cMsgRemind().C_GETnMaxSeq();

                if (nCountMsg == 0)
                    olaMsgCount.Visible = false;
                else
                {
                    olaMsgCount.Text = nCountMsg.ToString();

                    if (opnNotify.Visible)
                        olaMsgCount.Visible = false;
                    else
                        olaMsgCount.Visible = true;
                }

                opnNotify.Controls.Clear();

                aoMsgRemind = new cMsgRemind().C_GETaMsgRemind();

                foreach (cmlTCNTMsgRemind oMsg in aoMsgRemind)
                {
                    opnNotify.Controls.Add(new uNotify(oMsg.FNMsgType, string.Format(oMsg.FTMsgData, oMsg.FTSynName), oMsg.FDCreateOn));
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_GETxCountNotify : " + oEx.Message); }
        }

        /// <summary>
        /// Check Notify
        /// </summary>
        private void W_CHKxNotify()
        {
            try
            {
                if (opnNotify.Visible)
                    opnNotify.Visible = false;
                else
                {
                    olaMsgCount.Visible = false;
                    olaMsgCount.Text = "";
                    opnNotify.Visible = true;

                    new cMsgRemind().C_UPDxReadMsg();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_CHKxNotify : " + oEx.Message); }
        }

        public bool W_CHKbVerify2Payment()
        {
            try
            {
                //*Arm 63-04-07 Comment Code
                //if (Convert.ToDecimal(otbAmount.Text) == 0)
                //{
                //    return false;
                //}

                if (Convert.ToDecimal(otbAmount.Text) == 0 && cW_AmtTotalShw > 0)
                {
                    return false;
                }

                //if (cPayment.bC_StaPayLast && Convert.ToDecimal(otbAmount.Text) != cW_AmtTotalShw)
                //if (cPayment.bC_StaPayLast == false && Convert.ToDecimal(otbAmount.Text) != cW_AmtTotalShw) //*Em 63-06-19
                if (cPayment.bC_StaPayLast == false && Convert.ToDecimal(otbAmount.Text) < cW_AmtTotalShw ) //*Arm 63-08-08
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgPayLast"), 3);
                    return false;
                }
                cVB.cVB_Amount = Convert.ToDecimal(otbAmount.Text);
                return true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_CHKbVerify2Payment : " + oEx.Message); return false; }
            finally
            { }
        }

        public void W_DATxAddPay2Grid(string ptRcvName, decimal pcValue)
        {
            try
            {
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Pay 2 Grid Start"); //*Net Stamp
                //ogdCash.Rows.Add(ogdCash.RowCount + 1, ptRcvName, oW_SP.SP_SETtDecShwSve(1, pcValue, cVB.nVB_DecShow));

                //*Em 63-06-01
                ogdRcv.Rows.Add();
                ogdRcv.SetData(ogdRcv.Rows.Count - ogdRcv.Rows.Fixed, ogdRcv.Cols["FNSeqNo"].Index, ogdRcv.Rows.Count - ogdRcv.Rows.Fixed);
                ogdRcv.SetData(ogdRcv.Rows.Count - ogdRcv.Rows.Fixed, ogdRcv.Cols["FTRcvDetail"].Index, ptRcvName);
                ogdRcv.SetData(ogdRcv.Rows.Count - ogdRcv.Rows.Fixed, ogdRcv.Cols["FCRcvAmt"].Index, oW_SP.SP_SETtDecShwSve(1, pcValue, cVB.nVB_DecShow));
                //++++++++++++++

                //*Net 63-07-31 เพิ่มรายการชำระไปที่หน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_ADDxBillGrid(ptRcvName, oW_SP.SP_SETtDecShwSve(1, pcValue, cVB.nVB_DecShow));
                }
                //+++++++++++++++++++++++++++

                cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                if (cW_AmtTotalCal < 0)
                {
                    cW_AmtTotalCal = 0;
                }
                else
                {
                    cVB.cVB_RoundDiff = cSP.SP_CALcRoundDiff(cW_AmtTotalCal);
                    //if (cVB.tVB_KbdCallByName == "C_KBDxConfirm")
                    //{
                    //    cVB.cVB_RoundDiff = cW_AmtTotalCal;
                    //}
                    //else
                    //{

                    //}

                }
                cW_AmtTotalShw = cW_AmtTotalCal + cVB.cVB_RoundDiff;
                cW_AmtTotalPay = cW_AmtTotalPay + pcValue;

                if (cW_AmtTotalShw < 0) cW_AmtTotalShw = 0; //*Em 63-05-21

                //*Em 63-05-22
                if (cSale.bC_SetComplete)
                {
                    olaTitleCashPay.Text = cVB.oVB_GBResource.GetString("tChange");
                    olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_Change, cVB.nVB_DecShow);
                    otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                }
                else
                {
                    olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
                    otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
                }
                //++++++++++++++++++

                //*Net 63-07-08 sync ยอดชำระกับหน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                    cVB.oVB_CstScreen.W_SETxSummaryAmt(olaCashPayment.Text);
                }
                //++++++++++++++++++++++++++++++++++

                W_PRCxSumTender();
                //*Arm 63-03-02
                //ogdCash.Rows[ogdCash.Rows.Count - 1].Selected = true; // Select ข้อมูลล่าสุด 
                //ogdCash.FirstDisplayedScrollingRowIndex = ogdCash.Rows.Count - 1;   // Scrol ล่างสุด 
                ogdRcv.Row = ogdRcv.Rows.Count - ogdRcv.Rows.Fixed; //*Em 63-06-01
                //++++++++++++++
                new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : Pay 2 Grid End", cVB.bVB_AlwPrnLog); //*Net Stamp
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_DATxAddPay2Grid : " + oEx.Message); }
            finally
            {

            }
        }

        private void W_PRCxSumTender()
        {
            try
            {
                ogdSum.Rows[3].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalPay, cVB.nVB_DecShow);
                ogdSum.Rows[4].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_RoundDiff, cVB.nVB_DecShow);
                ogdSum.Rows[5].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_PRCxSumTender : " + oEx.Message); }
            finally
            { }
        }

        /// <summary>
        /// Process Payment
        /// </summary>
        /// <param name="ptFuncName"></param>
        private void W_PRCxPayment(string ptFuncName)
        {
            bool bChkSuccess = false;
            try
            {
                //[Pong][2019-05-21][Comment code]
                //switch (nW_Mode)
                //{
                //    case 1:     // Topup
                //        if (W_PRCbTopup()) { bChkSuccess = true; };
                //        break;
                //    case 2:     // Cancel Topup
                //        W_PRCxCancelTopup();
                //        bChkSuccess = true;
                //        break;
                //}

                //if(bChkSuccess)
                //{
                //    switch (ptFuncName)
                //    {
                //        case "C_KBDxCash":
                //            C_KBDxCash();
                //            break;
                //        case "C_KBDxCreditCard":
                //            break;

                //        case "C_KBDxCheque":
                //            break;

                //        case "C_KBDxCoupon":
                //            break;

                //        case "C_KBDxTranfer":
                //            break;

                //        case "C_KBDxCreditNote":
                //            break;

                //        case "C_KBDxClearDebtor":
                //            break;

                //        case "C_KBDxVoucher":
                //            break;

                //        case "C_KBDxStoreDebit":
                //            break;

                //        case "C_KBDxPromptPay":
                //            break;

                //        case "C_KBDxAlipay":
                //            break;

                //        case "C_KBDxPoint":
                //            break;

                //        case "C_KBDxInstallment":
                //            break;

                //        case "C_KBDxCreditLine":
                //            break;

                //        case "C_KBDxCurrency":
                //            break;
                //    }
                //}

                switch (ptFuncName)
                {
                    case "C_KBDxCash":
                        //   C_KBDxCash();
                        break;
                    case "C_KBDxCreditCard":
                        break;

                    case "C_KBDxCheque":
                        break;

                    case "C_KBDxCoupon":
                        break;

                    case "C_KBDxTranfer":
                        break;

                    case "C_KBDxCreditNote":
                        break;

                    case "C_KBDxClearDebtor":
                        break;

                    case "C_KBDxVoucher":
                        break;

                    case "C_KBDxStoreDebit":
                        break;

                    case "C_KBDxPromptPay":
                        break;

                    case "C_KBDxAlipay":
                        break;

                    case "C_KBDxPoint":
                        break;

                    case "C_KBDxInstallment":
                        break;

                    case "C_KBDxCreditLine":
                        break;

                    case "C_KBDxCurrency":
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_PRCxPayment : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //private void C_KBDxCash()
        //{
        //    cmlTFNTCrdTopUpRC oTopRC;
        //    StringBuilder oSql;
        //    cDatabase oDatabase;
        //    try
        //    {
        //        oTopRC = new cmlTFNTCrdTopUpRC();
        //        oTopRC.FTBchCode = cVB.tVB_BchCode;
        //        oTopRC.FTCthDocNo = cVB.tVB_DocNo;
        //        oTopRC.FTRcvCode = cPayment.tC_RcvCode;
        //        oTopRC.FTRcvName = cPayment.tC_RcvName;
        //        oTopRC.FTXrcRefNo1 = "";
        //        oTopRC.FTXrcRefNo2 = "";
        //        oTopRC.FTXrcRefDesc = "";
        //        oTopRC.FTBnkCode = "";
        //        oTopRC.FTRteCode = cVB.tVB_RteCode;
        //      //  oTopRC.FCXrcRteFac = Convert.ToDouble(oRate.FCRteRate);
        //        oTopRC.FCXrcFrmLeftAmt = 0;
        //        oTopRC.FCXrcUsrPayAmt = Convert.ToDouble(otbAmount.Text);
        //        oTopRC.FCXrcNet = Convert.ToDouble(olaCashPayment.Text);
        //        oTopRC.FCXrcChg = Convert.ToDouble(otbAmount.Text) - Convert.ToDouble(olaCashPayment.Text);





        //    }
        //    catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "C_KBDxCash : " + oEx.Message); }
        //    finally {
        //         //oTopHD = null;
        //         //oTopDT = null;
        //         oTopRC = null;
        //    }
        //}

        //private void W_SAVxTopupTemp2Tbl()
        //{
        //    StringBuilder oSql;
        //    try
        //    {
        //        oSql = new StringBuilder();

        //    }
        //    catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_PRCxAccept : " + oEx.Message); }
        //    finally
        //    {
        //        oSql = null;
        //        //oW_SP.SP_CLExMemory();
        //    }
        //}



        /// <summary>
        /// Process Discount Charge
        /// </summary>
        /// <param name="ptFuncName"></param>
        private void W_PRCxDisChg(string ptFuncName)
        {
            cFunctionKeyboard oKB;
            try
            {

                switch (ptFuncName)
                {
                    case "C_KBDxChgAmtBill":
                        break;

                    case "C_KBDxChgPerBill":
                        break;

                    case "C_KBDxDisAmtBill":
                        // new cFunctionKeyboard().
                        break;

                    case "C_KBDxDisPerBill":
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_PRCxDisChg : " + oEx.Message); }
        }

        /// <summary>
        /// Process Topup
        /// </summary>
        private bool W_PRCbTopup()
        {
            HttpResponseMessage oResponse = null;
            cClientService oCall;
            cmlReqTopup oReqTopup;
            cmlResTopup oResTopup;
            string tJsonResult, tJson, tAPI;

            try
            {
                if (string.IsNullOrEmpty(otbAmount.Text))
                {
                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgInputWristband"), 3);
                    otbAmount.Focus();
                    return false;
                }
                else
                {
                    tAPI = cVB.tVB_API2FNWallet.Substring(0, cVB.tVB_API2FNWallet.IndexOf("/V"));
                    if (oW_SP.SP_CHKbConnection(tAPI))
                    {
                        if (string.IsNullOrEmpty(cVB.tVB_API2FNWallet))
                        {
                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoService"), 3);
                            otbAmount.Focus();
                            return false;
                        }
                        else
                        {
                            oCall = new cClientService();
                            oResponse = new HttpResponseMessage();

                            oReqTopup = new cmlReqTopup();
                            oReqTopup.ptBchCode = cVB.tVB_BchCode;
                            oReqTopup.ptCrdCode = cVB.tVB_CardNo;

                            if (Convert.ToDecimal(otbAmount.Text) > Convert.ToDecimal(olaCashPayment.Text))
                            {
                                //oReqTopup.pcTxnValue = decimal.Parse(olaCashPayment.Text);
                                oReqTopup.pcTxnValue = cVB.cVB_Amount;
                            }
                            else
                            {
                                // oReqTopup.pcTxnValue = decimal.Parse(otbAmount.Text);
                                oReqTopup.pcTxnValue = cVB.cVB_Amount;
                            }

                            //oReqTopup.pcTxnValue = Convert.ToDouble(olaCashPayment.Text);
                            oReqTopup.ptAuto = "0";
                            oReqTopup.ptTxnPosCode = cVB.tVB_PosCode;
                            oReqTopup.pnLngID = 1;
                            oReqTopup.pcAvailable = cVB.cVB_Available;
                            oReqTopup.pnTxnOffline = cVB.nVB_TxnOffline;
                            oReqTopup.ptShpCode = cVB.tVB_ShpCode;

                            tJson = JsonConvert.SerializeObject(oReqTopup);

                            try
                            {
                                oResponse = oCall.C_POSToInvoke(cVB.tVB_API2FNWallet + "/Topup/Topup", tJson);

                                if (oResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    tJsonResult = oResponse.Content.ReadAsStringAsync().Result;
                                    oResTopup = JsonConvert.DeserializeObject<cmlResTopup>(tJsonResult);

                                    switch (oResTopup.rtCode)
                                    {
                                        case "1":
                                            nW_TxnID = oResTopup.rnTxnID;
                                            //return true;

                                            //W_ADDxItemCash(decimal.Parse(otbAmount.Text));
                                            W_ADDxItemCash(Convert.ToDecimal(otbAmount.Text));
                                            if (Convert.ToDouble(olaCashPayment.Text) == 0)
                                            {
                                                return true;
                                            }
                                            return false;

                                        default:
                                            oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                            otbAmount.Focus();
                                            new cLog().C_WRTxLog("wTopUp", "W_PRCxAccept : " + oResTopup.rtCode + " (" + oResTopup.rtDesc + ")", cVB.bVB_AlwPrnLog);
                                            return false;
                                            // break;
                                    }
                                }
                                else
                                {
                                    oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                    otbAmount.Focus();
                                    return false;
                                }
                            }
                            catch
                            {
                                oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantService"), 3);
                                otbAmount.Focus();
                                return false;
                            }

                            //*Net 63-07-31 ปรับตาม Moshi
                            oCall.C_PRCxCloseConn();    //*Em 63-07-18
                        }
                    }
                    else
                    {
                        oW_SP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrCon"), 3);
                        otbAmount.Focus();
                        return false;
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTopUp", "W_PRCxAccept : " + oEx.Message); return false; }
            finally
            {
                if (oResponse != null)
                    oResponse.Dispose();

                oResponse = null;
                oCall = null;
                oReqTopup = null;
                oResTopup = null;
                tJsonResult = null;
                tJson = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        private void opnMenu_MouseLeave(object sender, MouseEventArgs e)
        {

        }


        private void ocmShowNumpad_Click(object sender, EventArgs e)
        {
            try
            {
                if (opnuNumpad.Visible)
                {
                    opnuNumpad.Visible = false;
                    //  olaTitleQuickAmt.Visible = true;
                }
                else
                {
                    opnuNumpad.Visible = true;
                    // olaTitleQuickAmt.Visible = false;

                    otbAmount.Focus();              //*Arm 62-10-01
                    otbAmount.SelectionStart = 0;   //*Arm 62-10-01
                    otbAmount.SelectAll();          //*Arm 62-10-01
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
        }

        /// <summary>
        /// Get receive information.
        /// </summary>
        /// 
        /// <returns>
        /// Receive information.
        /// </returns>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-02-06] - add new function/method.
        /// </remarks>
        private List<cmlTFNMRcv> W_RCVaGetReceiveInfo()
        {
            List<cmlTFNMRcv> aoRcvInfo;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RCV.FTRcvCode, RCVL.FTRcvName");
                oSql.AppendLine("FROM TFNMRcv RCV WITH(NOLOCK)");
                oSql.AppendLine("	INNER JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode");
                oSql.AppendLine("	AND RCVL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE RCV.FTRcvStaUse = 1");

                aoRcvInfo = new cDatabase().C_GETaDataQuery<cmlTFNMRcv>(oSql.ToString());
                return aoRcvInfo;
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
                return null;
            }
            finally
            {
                aoRcvInfo = null;
                oSql = null;
                //oW_SP.SP_CLExMemory();

            }
        }

        private void W_ADDxItemCash(decimal pcAmount)
        {
            cmlPayItem oPayItem;
            string tRcvCode, tRcvName, tSeq;
            int nSeq;

            try
            {
                if (aoW_PayItems == null)
                {
                    aoW_PayItems = new List<cmlPayItem>();
                }

                //if (ogdCash.Rows.Count == 0)
                if(ogdRcv.Rows.Count-ogdRcv.Rows.Fixed == 0)    //*Em 63-06-01
                {
                    tSeq = "0";
                }
                else
                {
                    //tSeq = ogdCash.Rows[ogdCash.Rows.Count - 1].Cells[0].Value.ToString();
                    tSeq = ogdRcv.GetData(ogdRcv.Rows.Count - ogdRcv.Rows.Fixed, ogdRcv.Cols["FNSeqNo"].Index).ToString();  //*Em 63-06-01
                }

                nSeq = Convert.ToInt32(tSeq) + 1;

                tRcvCode = "001";
                tRcvName = (from oItem in aoW_RcvInfo
                            where string.Equals(oItem.FTRcvCode, tRcvCode)
                            select oItem.FTRcvName).FirstOrDefault();

                oPayItem = new cmlPayItem();
                oPayItem.nSeq = nSeq;
                oPayItem.tRcvCode = tRcvCode;
                oPayItem.tRcvName = tRcvName;
                oPayItem.cXrcUsrPayAmt = pcAmount;
                if (pcAmount <= cW_AmtTotalShw)
                {
                    oPayItem.cXrcNet = pcAmount;
                }
                else
                {
                    oPayItem.cXrcNet = cW_AmtTotalShw;
                }
                cW_AmtTotalShw -= pcAmount;
                cW_AmtTotalCal -= pcAmount;

                if (cW_AmtTotalShw >= 0)
                {
                    oPayItem.cXrcChg = 0;
                    olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
                    otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
                }
                else
                {
                    oPayItem.cXrcChg = Math.Abs(cW_AmtTotalShw);
                    olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                    otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                }

                //*Net 63-07-31 sync ยอดชำระกับหน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                    cVB.oVB_CstScreen.W_SETxSummaryAmt(olaCashPayment.Text);
                }
                //++++++++++++++++++++++++++++++++++

                aoW_PayItems.Add(oPayItem);
                //ogdCash.Rows.Add(nSeq, tRcvName, oW_SP.SP_SETtDecShwSve(1,cItemAmt,cVB.nVB_DecShow));
            }
            catch (Exception)
            {

            }
            finally
            {

            }
        }

        private void opbClosePro_Click(object sender, EventArgs e)
        {
            opnPromo.Visible = false;
        }

        private void ocmSchNextPage_Click(object sender, EventArgs e)
        {
            //int nW_CurrRow;
            //nW_CurrRow = nW_StartRow + 8;
            //nW_StartRow = nW_CurrRow;
            //nW_StartRow = (8 * nW_CurPage); // *Arm 62-11-15 [] //*Net 63-04-13
            nW_CurPage++;
            if (nW_CurPage > nW_MaxPage)
            {
                nW_CurPage = nW_MaxPage; //*Net 63-03-26
                return;
            }
            W_GETxPaymentType();
        }

        private void ocmSchPrevious_Click(object sender, EventArgs e)
        {
            //int nW_CurrRow;
            //nW_CurrRow = nW_StartRow - 8;
            //nW_StartRow = nW_CurrRow;

            //nW_StartRow = ((nW_CurPage - 1) * 8) - 8;// *Arm 62-11-15 [] //*Net 63-04-13
            nW_CurPage--;
            if (nW_CurPage < 1)
            {
                nW_CurPage = 1; //*Net 63-03-26
                return;
            }
            W_GETxPaymentType();
        }

        private void otmStart_Tick(object sender, EventArgs e)
        {
            //*Arm 62-11-18

            W_CHKxRefRedeem(); //*Arm 63-03-21 เช็คการแลกแต้ม
            W_CHKxRefDisChgBill(); //*Arm 63-03-21 เช็คการทำส่วนลด/ชาจน์ ท้ายบิล
            W_CHKxRefPayment();
            otmStart.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void W_PRCxTopUpRCCash()
        {
            StringBuilder oSql;
            cDatabase oDatabase;
            try
            {
                cPayment.C_PRCxPaymentCash();
                if (cVB.oVB_Payment.cW_AmtTotalShw <= 0)
                {
                    // if (true)
                    if (W_PRCbTopup())
                    {
                        // Call Wallet succes update doc
                        oSql = new StringBuilder();
                        oDatabase = new cDatabase();
                        oSql.AppendLine("UPDATE TFNTCrdTopUpHD WITH (ROWLOCK) SET ");
                        oSql.AppendLine("FTCthStaDoc='1'");
                        oSql.AppendLine("WHERE FTCthDocNo='" + cVB.tVB_DocNo + "'");
                        oDatabase.C_SETxDataQuery(oSql.ToString());
                    }
                    else
                    {

                    }

                    if (cVB.cVB_Change > 0)
                    {
                        MessageBox.Show("จำนวนเงินทอน " + cVB.cVB_Change + " ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cVB.oVB_Payment.cW_AmtTotalShw, cVB.nVB_DecShow);
                    aoW_RcvInfo = W_RCVaGetReceiveInfo();
                    // cW_AmtTotalShw = decimal.Parse(olaCashPayment.Text);
                    cW_AmtTotalCal = Convert.ToDecimal(olaCashPayment.Text);
                    cW_AmtTotalPay = 0;

                    //*Net 63-07-31 sync ยอดชำระกับหน้าจอ 2
                    if (cVB.oVB_CstScreen != null)
                    {
                        cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                        cVB.oVB_CstScreen.W_SETxSummaryAmt(olaCashPayment.Text);
                    }
                    //++++++++++++++++++++++++++++++++++

                    if (cVB.cVB_Change > 0)
                    {
                        MessageBox.Show("จำนวนเงินทอน " + cVB.cVB_Change + " ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception oExn)
            {
                new cLog().C_WRTxLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name + " : " + oExn.Message);
            }
            finally
            {
                oSql = null;
                oDatabase = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        public void W_ADDxDisChgBill(string ptStaDisChg, string ptDisChgTxt, decimal pcValue)
        {
            oW_SP = new cSP(); //*Arm 63-03-06
            string tDisChg = "";
            try
            {
                switch (ptStaDisChg)
                {
                    case "1":
                    case "2":
                        tDisChg = cVB.oVB_GBResource.GetString("tDis") + "(" + ptDisChgTxt + ")";
                        cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                        cW_DisChgAmt = cW_DisChgAmt + pcValue;
                        break;
                    case "3":
                    case "4":
                        tDisChg = cVB.oVB_GBResource.GetString("tChg") + "(" + ptDisChgTxt + ")";
                        cW_DisChgAmt = cW_DisChgAmt - pcValue;
                        cW_AmtTotalCal = cW_AmtTotalCal + pcValue;  //*Em 63-01-08
                        break;
                    case "5":   //คูปองส่วนลด
                        tDisChg = cVB.oVB_GBResource.GetString("tCpnDis") + "(" + ptDisChgTxt + ")";
                        cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                        cW_DisChgAmt = cW_DisChgAmt + pcValue;
                        break;
                    case "6":   //แลกแต้ม     //*Arm 63-03-06
                        tDisChg = cVB.oVB_GBResource.GetString("tRdDis") + "(" + ptDisChgTxt + ")";
                        cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                        cW_DisChgAmt = cW_DisChgAmt + pcValue;
                        break;
                    case "7":   //คูปองแลกซื้อ  //*Net 63-03-17
                        tDisChg = cVB.oVB_GBResource.GetString("tCpnRd") + "(" + ptDisChgTxt + ")";
                        cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                        cW_DisChgAmt = cW_DisChgAmt + pcValue;
                        break;
                    case "8":   //แลกแต้มสินค้า     //*Arm 63-03-22
                        tDisChg = cVB.oVB_GBResource.GetString("tRdPdt") + "(" + ptDisChgTxt + ")";
                        cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                        cW_DisChgAmt = cW_DisChgAmt + pcValue;
                        break;
                }
                //ogdCash.Rows.Add(ogdCash.RowCount + 1, tDisChg, oW_SP.SP_SETtDecShwSve(1, pcValue, cVB.nVB_DecShow));

                //*Em 63-06-01
                ogdRcv.Rows.Add();
                ogdRcv.SetData(ogdRcv.Rows.Count - ogdRcv.Rows.Fixed, ogdRcv.Cols["FNSeqNo"].Index, ogdRcv.Rows.Count - ogdRcv.Rows.Fixed);
                ogdRcv.SetData(ogdRcv.Rows.Count - ogdRcv.Rows.Fixed, ogdRcv.Cols["FTRcvDetail"].Index, tDisChg);
                ogdRcv.SetData(ogdRcv.Rows.Count - ogdRcv.Rows.Fixed, ogdRcv.Cols["FCRcvAmt"].Index, oW_SP.SP_SETtDecShwSve(1, pcValue, cVB.nVB_DecShow));
                //++++++++++++++++

                //*Net 63-07-31 เพิ่มรายการส่วนลดไปที่หน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_ADDxBillGrid(tDisChg, oW_SP.SP_SETtDecShwSve(1, pcValue, cVB.nVB_DecShow));
                }
                //+++++++++++++++++++++++++++++++++++++++++

                cVB.cVB_RoundDiff = cSP.SP_CALcRoundDiff(cW_AmtTotalCal);
                cW_AmtTotalShw = cW_AmtTotalCal + cVB.cVB_RoundDiff;

                ogdSum.Rows[1].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_DisChgAmt, cVB.nVB_DecShow);
                ogdSum.Rows[2].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalCal, cVB.nVB_DecShow);
                ogdSum.Rows[3].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalPay, cVB.nVB_DecShow);
                ogdSum.Rows[4].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_RoundDiff, cVB.nVB_DecShow);
                ogdSum.Rows[5].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);

                olaCashPayment.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
                otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);

                //*Net 63-07-31 sync ยอดชำระกับหน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxLastPDT(olaTitleCashPay.Text, olaCashPayment.Text);
                    cVB.oVB_CstScreen.W_SETxSummaryAmt(olaCashPayment.Text);
                }
                //++++++++++++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_ADDxDisChgBill : " + oEx.Message); }
            finally
            {

            }
        }

        private void wPayment_KeyDown(object sender, KeyEventArgs e)
        {
            //*Arm 63-02-06 - (HotKey) Created function wSyncData_KeyDown
            try
            {
                W_CALxByName(e);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "wSale_KeyDown " + oEx.Message); }
        }
        private void W_CALxByName(KeyEventArgs poKey)
        {
            //*Arm 63-02-06 -(HotKey) Created function W_CALxByName 
            string tFuncName;

            try
            {
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(poKey);
                W_GETxFuncByFuncName(tFuncName);

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSale", "W_CALxByName : " + oEx.Message); }
            finally
            {
                poKey = null;
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        //*Net 63-03-04
        /// <summary>
        /// Accept only number and dot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //Zen 13/04/2020 เหมือนกดปุ่มซ่อนโปร
                opnPromo.Visible = false;
                if ((!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.')) || e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "otbAmount_KeyDown : " + oEx.Message); }
        }
        //private void wPayment_Activated(object sender, EventArgs e)
        //{
        //    if (bW_Activate == false)
        //    {
        //        if (cSale.nC_DocType == 9 && !string.IsNullOrEmpty(cVB.tVB_RefDocNo))
        //        {
        //            //if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo))
        //            if (cVB.oVB_Payment == null)
        //            {
        //                cPayment.nC_SeqRC = 0;
        //                cVB.oVB_Payment = this;
        //            }

        //            W_CHKxRefPayment();
        //        }

        //        bW_Activate = true;
        //    }
        //}

        /// <summary>
        /// *Arm 63-03-06
        /// คำนวณส่วนลด/ชาจน์ ท้ายบิล จาก SO
        /// </summary>
        public void W_PRCxDisChgAmtBillBySo()
        {
            StringBuilder oSql;
            decimal cAmt;
            decimal cDisChg;
            string tDisChgTxt;
            decimal cB4DisChg;
            int nDisType;
            decimal cAmount;
            try
            {
                oSql = new StringBuilder();

                //*Arm 63-03-31 - แก้เงื่อนไขเช็ค StaAlwPosCalSo
                if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                {
                    if (cVB.oVB_ReferSO.aoTARTSoHDDis.Count > 0)
                    {
                        new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : DisChg SO Start", cVB.bVB_AlwPrnLog); //*Net Stamp
                        foreach (cmlResInfoTARTSoHDDis oSoHDDis in cVB.oVB_ReferSO.aoTARTSoHDDis)   // Process Scan Barcode ที่ละตัว
                        {
                            // cB4DisChg : ยอดก่อน DisChg
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                            oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                            oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");

                            cB4DisChg = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                            if (oSoHDDis.rtXhdDisChgType == "1" || oSoHDDis.rtXhdDisChgType == "2")  //*Arm 63-04-03 เช็คเงื่อนไขสามารถลด/ชาจน์ ได้
                            {
                                if (cB4DisChg > 0)
                                {
                                    // cAmt : มูลค่าลด / ชาร์จ
                                    cAmt = Convert.ToDecimal(oSoHDDis.rcXhdAmt);

                                    // cDisChg : ยอดลด Chg
                                    cDisChg = Convert.ToDecimal(oSoHDDis.rcXhdDisChg);

                                    // tDisChgTxt : ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5 %, ยอดก่อน DisChg
                                    //tDisChgTxt = oW_SP.SP_SETtDecShwSve(1, cAmt, cVB.nVB_DecShow).ToString();
                                    tDisChgTxt = oSoHDDis.rtXhdDisChgTxt; //*Arm 63-04-10

                                    // nDisType : (1 ลดแบบจำนวน,2 ลดแบบเปอร์เซ็น,3 ชาร์ตแบบจำนวน,4 ชาร์ตแบบเปอร์เซ็น)
                                    nDisType = Convert.ToInt32(oSoHDDis.rtXhdDisChgType);

                                    // Process C_ADDxDisChgBill
                                    cPayment.C_ADDxDisChgBill(cAmt, cDisChg, tDisChgTxt, cB4DisChg, nDisType);

                                    cVB.oVB_Payment.W_ADDxDisChgBill(nDisType.ToString(), tDisChgTxt, cAmt);
                                }
                            }
                            else
                            {
                                if (cB4DisChg >= 0)
                                {
                                    // cAmt : มูลค่าลด / ชาร์จ
                                    cAmt = Convert.ToDecimal(oSoHDDis.rcXhdAmt);

                                    // cDisChg : ยอดลด Chg
                                    cDisChg = Convert.ToDecimal(oSoHDDis.rcXhdDisChg);

                                    // tDisChgTxt : ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5 %, ยอดก่อน DisChg
                                    //tDisChgTxt = oW_SP.SP_SETtDecShwSve(1, cAmt, cVB.nVB_DecShow).ToString();
                                    tDisChgTxt = oSoHDDis.rtXhdDisChgTxt; //*Arm 63-04-10

                                    // nDisType : (1 ลดแบบจำนวน,2 ลดแบบเปอร์เซ็น,3 ชาร์ตแบบจำนวน,4 ชาร์ตแบบเปอร์เซ็น)
                                    nDisType = Convert.ToInt32(oSoHDDis.rtXhdDisChgType);

                                    // Process C_ADDxDisChgBill
                                    cPayment.C_ADDxDisChgBill(cAmt, cDisChg, tDisChgTxt, cB4DisChg, nDisType);

                                    cVB.oVB_Payment.W_ADDxDisChgBill(nDisType.ToString(), tDisChgTxt, cAmt);
                                }
                            }
                        }

                        new cLog().C_WRTxLog("wPayment", MethodBase.GetCurrentMethod().Name + $" : DisChg SO End", cVB.bVB_AlwPrnLog); //*Net Stamp
                    }
                }
                //++++++++++++


                //if (cVB.tVB_CstStaAlwPosCalSo == "1")
                //{

                //}
                //else
                //{
                //    if (cVB.oVB_ReferSO.aoTARTSoHDDis.Count > 0)
                //    {
                //        foreach (cmlResInfoTARTSoHDDis oSoHDDis in cVB.oVB_ReferSO.aoTARTSoHDDis)   // Process Scan Barcode ที่ละตัว
                //        {
                //            // cB4DisChg : ยอดก่อน DisChg
                //            oSql.Clear();
                //            oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                //            oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                //            oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");

                //            cB4DisChg = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                //            // cAmt : มูลค่าลด / ชาร์จ
                //            cAmt = Convert.ToDecimal(oSoHDDis.rcXhdAmt);

                //            // cDisChg : ยอดลด Chg
                //            cDisChg = Convert.ToDecimal(oSoHDDis.rcXhdDisChg);

                //            // tDisChgTxt : ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5 %, ยอดก่อน DisChg
                //            tDisChgTxt = oW_SP.SP_SETtDecShwSve(1, cAmt, cVB.nVB_DecShow).ToString();

                //            // nDisType : (1 ลดแบบจำนวน,2 ลดแบบเปอร์เซ็น,3 ชาร์ตแบบจำนวน,4 ชาร์ตแบบเปอร์เซ็น)
                //            nDisType = Convert.ToInt32(oSoHDDis.rtXhdDisChgType);

                //            // Process C_ADDxDisChgBill
                //            cPayment.C_ADDxDisChgBill(cAmt, cDisChg, tDisChgTxt, cB4DisChg, nDisType);

                //            cVB.oVB_Payment.W_ADDxDisChgBill(nDisType.ToString(), tDisChgTxt, cAmt); 

                //        }
                //    }
                //}
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wPayment", "W_PRCxDisChgAmtBillBySo : " + oEx.Message);
            }
        }

        public void W_SETxPmtPdt()
        {
            StringBuilder oSql;
            DataTable oDbTbl = new DataTable();
            cPdtPmt oPdtPmt = new cPdtPmt();
            try
            {
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT distinct FTPmhDocNo,FCXpdDis FROM " + cSale.tC_TblSalPD + "");
                //*Em 63-05-26
                oSql.AppendLine("SELECT distinct FTPmhName,FCXpdDis");
                oSql.AppendLine("FROM " + cSale.tC_TblSalPD + " PD WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TPMTPmtHD_L PMT WITH(NOLOCK) ON PD.FTBchCode = PMT.FTBchCode AND PD.FTPmhDocNo = PMT.FTPmhDocNo");
                //++++++++++++++
                oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNo + "' AND PD.FCXpdDisAvg <> 0");
                oSql.AppendLine("AND PD.FTXpdGetType <> '4' ");        //*Em 63-05-05

                ogdPmt.Rows.Count = ogdPmt.Rows.Fixed;
                oDbTbl = new cDatabase().C_GEToDataQuery(oSql.ToString());
                if (oDbTbl.Rows.Count > 0)
                {
                    int nIndex = 0;
                    foreach (DataRow oRow in oDbTbl.Rows)
                    {
                        nIndex = nIndex + 1;
                        ////ogdProList.Rows.Add(nIndex.ToString(), oPdtPmt.C_GETtNamePmt(oRow.Field<string>("FTPmhDocNo")), oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow));
                        //ogdProList.Rows.Add(nIndex.ToString(), oRow.Field<string>("FTPmhName"), oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow));

                        //*Em 06-01
                        ogdPmt.Rows.Add();
                        ogdPmt.SetData(ogdPmt.Rows.Count - ogdPmt.Rows.Fixed, ogdPmt.Cols["FNSeqNo"].Index, nIndex);
                        ogdPmt.SetData(ogdPmt.Rows.Count - ogdPmt.Rows.Fixed, ogdPmt.Cols["FTPmhName"].Index, oRow.Field<string>("FTPmhName"));
                        ogdPmt.SetData(ogdPmt.Rows.Count - ogdPmt.Rows.Fixed, ogdPmt.Cols["FCPmhDis"].Index, oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow));
                        //+++++++++++++++++

                        //*Net 63-07-31 เพิ่มรายการโปรโมชั่นไปที่หน้าจอ 2
                        if (cVB.oVB_CstScreen != null)
                        {
                            cVB.oVB_CstScreen.W_ADDxBillGrid(oRow.Field<string>("FTPmhName"),
                                oW_SP.SP_SETtDecShwSve(1, oRow.Field<decimal>("FCXpdDis"), cVB.nVB_DecShow));
                        }
                        //++++++++++++++++++++++++++++++++++++++++++
                    }
                    //ogdProList.Update();
                    //opnPromo.Visible = true;
                    opnPromo.BringToFront();

                    //*Arm 63-04-09 กรณีใช้ใบสั่งขาย และไม่อนุญาตให้ คำนวณรายการใหม่ไม่ให้แสดง Promotion
                    if (cVB.oVB_ReferSO != null)
                    {
                        if (cVB.oVB_ReferSO.aoTARTSoHDCst[0].rtXshStaAlwPosCalSo != "1")
                        {
                            opnPromo.Visible = false;
                        }
                        else
                        {
                            opnPromo.Visible = true;

                        }

                    }
                    else
                    {
                        opnPromo.Visible = true;

                    }
                    //++++++++++++++++++++++++

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPayment", "W_SETxPmtPdt : " + oEx.Message); }
        }

        private void W_SETxColGrid(C1FlexGrid poGD)
        {
            int nWidth = 0;
            try
            {
                switch (poGD.Name)
                {
                    case "ogdPmt":
                        nWidth = poGD.Width;
                        poGD.Cols["FNSeqNo"].Width = nWidth * 10 / 100;
                        poGD.Cols["FTPmhName"].Width = nWidth * 60 / 100;
                        //*Net 63-07-31 ปรับตาม Moshi
                        //poGD.Cols["FCPmhDis"].Width = nWidth * 30 / 100;
                        poGD.ExtendLastCol = true;  //*Em 63-07-29

                        poGD.Cols["FNSeqNo"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                        poGD.Cols["FTPmhName"].Caption = cVB.oVB_GBResource.GetString("tPromotion");
                        poGD.Cols["FCPmhDis"].Caption = cVB.oVB_GBResource.GetString("tDis");

                        poGD.Cols["FNSeqNo"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPmhName"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FCPmhDis"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["FNSeqNo"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTPmhName"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FCPmhDis"].TextAlign = TextAlignEnum.RightCenter;

                        poGD.Cols["FCPmhDis"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);

                        break;

                    case "ogdRcv":
                        nWidth = poGD.Width;
                        poGD.Cols["FNSeqNo"].Width = nWidth * 10 / 100;
                        poGD.Cols["FTRcvDetail"].Width = nWidth * 60 / 100;
                        //*Net 63-07-31 ปรับตาม Moshi
                        //poGD.Cols["FCRcvAmt"].Width = nWidth * 30 / 100;
                        poGD.ExtendLastCol = true;  //*Em 63-07-29

                        poGD.Cols["FNSeqNo"].Caption = cVB.oVB_GBResource.GetString("tSeq");
                        poGD.Cols["FTRcvDetail"].Caption = cVB.oVB_GBResource.GetString("tDetail");
                        poGD.Cols["FCRcvAmt"].Caption = cVB.oVB_GBResource.GetString("tAmount");

                        poGD.Cols["FNSeqNo"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTRcvDetail"].TextAlignFixed = TextAlignEnum.CenterCenter;
                        poGD.Cols["FCRcvAmt"].TextAlignFixed = TextAlignEnum.CenterCenter;

                        poGD.Cols["FNSeqNo"].TextAlign = TextAlignEnum.CenterCenter;
                        poGD.Cols["FTRcvDetail"].TextAlign = TextAlignEnum.LeftCenter;
                        poGD.Cols["FCRcvAmt"].TextAlign = TextAlignEnum.RightCenter;

                        poGD.Cols["FCRcvAmt"].Format = "###,###,##0." + new string('0', cVB.nVB_DecShow);
                        break;

                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSale", "W_SETxColGrid : " + oEx.Message);
            }
        }
        #endregion End Function / Method


    }
}
