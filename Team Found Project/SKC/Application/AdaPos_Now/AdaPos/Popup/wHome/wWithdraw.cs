using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Resources_String.Local;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace AdaPos.Popup.wHome
{
    public partial class wWithdraw : Form
    {
        #region Variable

        private ResourceManager oW_Resource;
        private cSP oW_SP;
        private string tW_SpaceFrq = "";
        private string tW_SpaceMnyNow = "";
        private string tW_SpaceMnyLast = "";
        private decimal cW_AcceptAmt; //*Net 63-06-24
        private decimal cW_SumAmt; //*Net 63-06-24

        #endregion End Variable

        #region Constructor

        public wWithdraw()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                //new cShiftEvent().C_GETxEventCode("fC_Withdraw");
                new cShiftEvent().C_GETxEventCode("fC_MnyOut");

                W_SETxDesign();
                W_SETxText();
                W_GETxSumWithdraw();

                onpNumpad.oU_TextValue = otbWithdraw;
                onpNumpad.tU_TextValue = otbWithdraw.Text;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "wWithdraw : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "OnPaintBackground : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wWithdraw_Shown(object sender, EventArgs e)
        {
            try
            {
                otbWithdraw.Focus();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "wWithdraw_Shown : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wWithdraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                cVB.tVB_KbdScreen = "HOME";
                oW_Resource = null;
                tW_SpaceFrq = null;
                tW_SpaceMnyNow = null;
                tW_SpaceMnyLast = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "wWithdraw_FormClosed : " + oEx.Message); }
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
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Reason
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSchReason_Click(object sender, EventArgs e)
        {
            try
            {
                W_GETxReason();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "ocmSchReason_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Accept
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxWithdraw();

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Print
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //if (cVB.bVB_ChkPrint)
                //{
                    W_PRCxPrint();
                    ocmBack_Click(ocmBack,new EventArgs());
                //}
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "ocmPrint_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Enter, Function keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbWithdraw_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;

            try
            {
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                W_GETxFuncByFuncName(tFuncName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "otbWithdraw_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Check format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbWithdraw_KeyPress(object sender, KeyPressEventArgs e)
        {
            string[] atDot;

            try
            {
                if ((!char.IsDigit(e.KeyChar) && e.KeyChar != '.') || (e.KeyChar == '.' && otbWithdraw.Text.Contains(".")))
                    e.Handled = true;

                atDot = otbWithdraw.Text.Split('.');

                if (atDot.Length > 1)
                {
                    if (atDot[1].Length >= cVB.nVB_DecShow && Convert.ToDecimal(otbWithdraw.Text) != 0)
                        e.Handled = true;
                }

                if (e.KeyChar == (char)Keys.Back)
                    e.Handled = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "otbWithdraw_KeyPress : " + oEx.Message); }
            finally
            {
                atDot = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// set value to text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbWithdraw_TextChanged(object sender, EventArgs e)
        {
            decimal cCurAmt = 0, cWithdraw;

            try
            {
                onpNumpad.tU_TextValue = otbWithdraw.Text;
                cWithdraw = oW_SP.SP_COVcStringToDouble(otbWithdraw.Text);
                //cCurAmt = Convert.ToDecimal(olaSumAmt.Text) + cWithdraw;
                cCurAmt = cWithdraw; //*Net 63-04-20
                olaCurAmt.Text = oW_SP.SP_SETtDecShwSve(1, cCurAmt, cVB.nVB_DecShow);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "otbWithdraw_TextChanged : " + oEx.Message); }
        }

        /// <summary>
        /// Print Withdraw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void W_PRNxWithdraw(object sender, PrintPageEventArgs e)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine, tWithdraw;
            int nStartY = 0, nWidth;

            try
            {
                nWidth = Convert.ToInt32(e.Graphics.VisibleClipBounds.Width);
                tLine = "------------------------------------------------------------------------";

                if (cVB.nVB_PaperSize == 1)
                {
                    if (nWidth > 215)
                        nWidth = 215;
                }
                else
                {
                    if (nWidth > 280)
                        nWidth = 280;
                }

                oGraphic = e.Graphics;
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg

                // Get ค่า ID เครื่อง จาก DB
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 15;
                oGraphic.DrawString(oW_Resource.GetString("tWithdrawHeader"), cVB.aoVB_PInvLayout[1], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 10;
                //oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, 0, nStartY);
                oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter); //*Net 63-06-24 วาดเส้นให้พอดีหน้า
                nStartY += 15;
                oGraphic.DrawString("Sale-Date:" + Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " Pos:" + cVB.tVB_PosCode + " Usr:" + cVB.tVB_UsrCode,
                                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 10;
                //oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, 0, nStartY);
                oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter); //*Net 63-06-24 วาดเส้นให้พอดีหน้า

                W_CHKxLengthString(); // ตรวจสอบความยาวของตัวอักษร

                // Head Detail
                nStartY += 15;
                oGraphic.DrawString(oW_Resource.GetString("tDetail"), cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                oGraphic.DrawString("| " + tW_SpaceFrq + oW_Resource.GetString("tFrq") + " |", cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(80, nStartY, 60, 15),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    });
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tAmount"), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(140, nStartY, nWidth - 140, 15), oFormatFar);
                nStartY += 10;
                //oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, 0, nStartY);
                oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter); //*Net 63-06-24 วาดเส้นให้พอดีหน้า

                // Detail : Withdraw Last
                if (Convert.ToInt32(olaSumFeq.Text) > 1)
                {
                    nStartY += 15;
                    oGraphic.DrawString(oW_Resource.GetString("tPreviousSum"), cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);

                    tWithdraw = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(olaSumFeq.Text) - 1, cVB.nVB_DecShow); //*Arm 63-07-31 ปรับตาม Moshi
                    //tWithdraw = oW_SP.SP_SETtDecShwSve(1, cW_SumAmt - cW_AcceptAmt, cVB.nVB_DecShow); //*Net 63-06-24
                    oGraphic.DrawString("| " + tW_SpaceMnyLast + tWithdraw + " |", cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(80, nStartY, 60, 15),
                        new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            FormatFlags = StringFormatFlags.NoWrap,
                            Trimming = StringTrimming.EllipsisCharacter
                        });

                    //tWithdraw = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(olaSumAmt.Text) - Convert.ToDecimal(otbWithdraw.Text), cVB.nVB_DecShow);
                    tWithdraw = oW_SP.SP_SETtDecShwSve(1, cW_SumAmt - cW_AcceptAmt, cVB.nVB_DecShow); //*Arm 63-07-31 ยกมาจาก Moshi
                    oGraphic.DrawString(tWithdraw, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 140, 15), oFormatFar);
                }

                // Detail : Withdraw Now
                nStartY += 15;
                oGraphic.DrawString(oW_Resource.GetString("tWithdrawNow"), cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);

                tWithdraw = oW_SP.SP_SETtDecShwSve(1, 1, cVB.nVB_DecShow);
                oGraphic.DrawString("| " + tW_SpaceMnyNow + tWithdraw + " |", cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(80, nStartY, 60, 15),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    });

                //tWithdraw = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(otbWithdraw.Text), cVB.nVB_DecShow);
                tWithdraw = oW_SP.SP_SETtDecShwSve(1, cW_AcceptAmt, cVB.nVB_DecShow);
                oGraphic.DrawString(tWithdraw, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 140, 15), oFormatFar);
                nStartY += 10;
                //oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, 0, nStartY);
                oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter); //*Net 63-06-24 วาดเส้นให้พอดีหน้า

                // Total
                nStartY += 15;
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tTotal") + ":", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);

                tWithdraw = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(olaCurAmt.Text), cVB.nVB_DecShow);
                oGraphic.DrawString(tWithdraw, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 140, 15), oFormatFar);
                nStartY += 10;
                //oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, 0, nStartY);
                oGraphic.DrawString(tLine, new Font("Microsoft Sans Serif", 10), Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter); //*Net 63-06-24 วาดเส้นให้พอดีหน้า

                // แสดงวันและเวลาในการปริ้น, แสดงพนักงานขาย
                nStartY += 15;
                oGraphic.DrawString("Print-Date:" + string.Format("{0:dd/MM/yyyy}", DateTime.Now).PadRight(10) +
                                    "  Time:" + string.Format("{0:HH:mm:ss}", DateTime.Now), cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                nStartY += 30;
                //oGraphic.DrawString("(_______________________)", cVB.aoVB_PInvLayout[6], Brushes.Black, new RectangleF(0, nStartY, 140, 15), oFormatCenter);
                oGraphic.DrawString("(_______________________)", cVB.aoVB_PInvLayout[6], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 15;
                //oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCashier"), cVB.aoVB_PInvLayout[6], Brushes.Black, new RectangleF(0, nStartY, 140, 15), oFormatCenter);
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCashier"), cVB.aoVB_PInvLayout[6], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 30;

                oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_PRNxWithdraw : " + oEx.Message); }
            finally
            {
                if (oGraphic != null)
                    oGraphic.Dispose();

                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();

                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                oMsg = null;
                tLine = null;
                //oW_SP.SP_CLExMemory();
            }
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

                cVB.tVB_KbdScreen = "WITHDRAW";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wWithdraw", "ocmKB_Click : " + ex.Message); }
        }

        /// <summary>
        /// Remark
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbRemark_KeyDown(object sender, KeyEventArgs e)
        {
            string tFuncName;

            try
            {
                tFuncName = new cFunctionKeyboard().C_KBDtFunction(e);
                new cFunctionKeyboard().C_PRCxCallByName(tFuncName);

                W_GETxFuncByFuncName(tFuncName);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "otbRemark_KeyDown : " + oEx.Message); }
            finally
            {
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
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
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmPrint.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmSchReason.BackColor = cVB.oVB_ColNormal;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_SETxDesign " + oEx.Message); }
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

                cVB.tVB_KbdScreen = "WITHDRAW";

                olaTitleWithdraw.Text = oW_Resource.GetString("tWithdraw");
                olaTitleCurAmt.Text = oW_Resource.GetString("tCurAmt");
                olaTitleSumFeq.Text = oW_Resource.GetString("tSumFeq");
                olaTitleSumAmt.Text = oW_Resource.GetString("tSumAmt");
                olaTitleReason.Text = cVB.oVB_GBResource.GetString("tReason");
                olaTitleRemark.Text = cVB.oVB_GBResource.GetString("tRemark");

                olaCurAmt.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม
                olaSumAmt.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม
                olaSumFeq.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม
                otbWithdraw.Text = oW_SP.SP_SETtDecShwSve(1, 0m, cVB.nVB_DecShow); //*Net 63-06-24 ปรับทศนิยม
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_SETxText " + oEx.Message); }
        }

        /// <summary>
        /// Get Reasonห
        /// </summary>
        private void W_GETxReason()
        {
            wReason oReason = null;

            try
            {
                oReason = new wReason("005");
                oReason.ShowDialog();

                if (oReason.DialogResult == DialogResult.OK)
                {
                    otbReason.Text = cVB.oVB_Reason.FTRsnName;
                    otbRemark.Focus();
                }

                cVB.tVB_KbdScreen = "WITHDRAW";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_GETxReason : " + oEx.Message); }
            finally
            {
                if (oReason != null)
                    oReason.Dispose();

                oReason = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process withdraw
        /// </summary>
        private void W_PRCxWithdraw()
        {
            cmlTPSTShiftEvent oEvent;
            decimal cBalance;
            try
            {
                if (string.IsNullOrEmpty(otbWithdraw.Text))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgInputWithdraw"), 3);
                    otbWithdraw.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(otbReason.Text))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgChooseReason"), 3);
                    ocmSchReason.Focus();
                    return;
                }

                cBalance = new cShiftEvent().C_GETcShiftBalance();
                if (cBalance < Convert.ToDecimal(otbWithdraw.Text))
                {
                    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgNoWithdraw"), 3);
                    otbWithdraw.Text = new cSP().SP_SETtDecShwSve(1, cBalance, cVB.nVB_DecShow);
                    otbWithdraw.Focus();
                    otbWithdraw.ReadOnly = false;
                    return;
                }

                oEvent = new cmlTPSTShiftEvent();
                oEvent.FTBchCode = cVB.tVB_BchCode;
                oEvent.FTShfCode = cVB.tVB_ShfCode;
                oEvent.FTPosCode = cVB.tVB_PosCode; //*Em 62-01-03  เพิ่ม FTPosCode
                                                    //oEvent.FNSdtSeqNo = Convert.ToInt32(Math.Round(Convert.ToDouble(olaSumFeq.Text))) + 1;
                oEvent.FNSdtSeqNo = cVB.nVB_ShfSeq; //*Em 62-08-15
                oEvent.FDHisDateTime = DateTime.Now;
                oEvent.FTEvnCode = cVB.tVB_EvnCode;
                oEvent.FNSvnQty = 1;
                oEvent.FCSvnAmt = Convert.ToDecimal(otbWithdraw.Text);
                oEvent.FTRsnCode = cVB.oVB_Reason.FTRsnCode;
                oEvent.FTSvnApvCode = cVB.tVB_UsrCode;
                oEvent.FTSvnRemark = otbRemark.Text;

                cW_AcceptAmt = Convert.ToDecimal(otbWithdraw.Text); //*Net 63-06-24 เก็บจำนวนเงินไว้

                new cShiftEvent().C_INSxShiftEvent(oEvent);

                ocmAccept.Enabled = false;
                ocmAccept.Image = Properties.Resources.AcceptDis_32;

                ocmPrint.Enabled = true;
                ocmPrint.Image = Properties.Resources.Print_32;

                W_GETxSumWithdraw();
                cSale.C_OPNxCashDrawer(); //*Em 62-10-03

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_PRCxWithdraw : " + oEx.Message); }
            finally
            {
                oEvent = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get summary Amount, Frequency
        /// </summary>
        private void W_GETxSumWithdraw()
        {
            cmlTPSTShiftEvent oEvent;

            try
            {
                //oEvent = new cShiftEvent().C_GEToShiftEvent();
                oEvent = new cShiftEvent().C_GEToShiftSumEvent(cVB.tVB_ShfCode, cVB.tVB_EvnCode);

                if (oEvent == null)
                {
                    cW_SumAmt = 0m; //*Net 63-06-24
                    olaCurAmt.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                    olaSumAmt.Text = oW_SP.SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                    olaSumFeq.Text = oW_SP.SP_SETtDecShwSve(1, 0, 0);
                }
                else
                {
                    cW_SumAmt = oEvent.FCSvnAmt.Value; //*Net 63-06-24
                    olaCurAmt.Text = oW_SP.SP_SETtDecShwSve(1, oEvent.FCSvnAmt.Value, cVB.nVB_DecShow);
                    olaSumAmt.Text = oW_SP.SP_SETtDecShwSve(1, oEvent.FCSvnAmt.Value, cVB.nVB_DecShow);
                    olaSumFeq.Text = oW_SP.SP_SETtDecShwSve(1, oEvent.FNSvnQty.Value, 0);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_GETxSumWithdraw : " + oEx.Message); }
            finally
            {
                oEvent = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process Print
        /// </summary>
        private void W_PRCxPrint()
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;

            try
            {
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();

                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                oDoc.PrintController = new StandardPrintController();
                oDoc.PrintPage += W_PRNxWithdraw;
                oDoc.Print();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_PRCxPrint : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                //oW_SP.SP_CLExMemory();
            }
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
                    case "C_KBDxAccept":
                        if (ocmAccept.Enabled)
                            W_PRCxWithdraw();
                        break;

                    case "C_KBDxWithdrawInput":
                        otbWithdraw.Focus();
                        break;

                    case "C_KBDxWithdrawReason":
                        W_GETxReason();
                        break;

                    case "C_KBDxWithdrawRemark":
                        otbRemark.Focus();
                        break;

                    case "C_KBDxWithdrawPrint":
                        if (ocmPrint.Enabled)
                            if (cVB.bVB_ChkPrint)
                                W_PRCxPrint();
                        break;

                    case "C_KBDxBack":
                        this.Close();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// ตรวจสอบความยาวของตัวอักษร
        /// </summary>
        private void W_CHKxLengthString()
        {
            int nLengthFrq, nSpaceMnyNow, nSpaceMnyLast, nMax;

            try
            {
                nLengthFrq = oW_SP.SP_PRCnLengthString(oW_Resource.GetString("tFrq"));
                nSpaceMnyNow = oW_SP.SP_PRCnLengthString(oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(olaSumFeq.Text) + 1, cVB.nVB_DecShow));
                nSpaceMnyLast = oW_SP.SP_PRCnLengthString(oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(olaSumFeq.Text), cVB.nVB_DecShow));

                // Check Length String
                nMax = new[] { nLengthFrq, nSpaceMnyNow, nSpaceMnyLast }.Max();

                for (int nNum = nLengthFrq; nNum < nMax; nNum++)
                {
                    tW_SpaceFrq += " ";
                }

                for (int nNum = nSpaceMnyNow; nNum < nMax; nNum++)
                {
                    tW_SpaceMnyNow += " ";
                }
                for (int nNum = nSpaceMnyLast; nNum < nMax; nNum++)
                {
                    tW_SpaceMnyLast += " ";
                }

                nMax = 0;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wWithdraw", "W_CHKxLengthString : " + oEx.Message); }
        }

        #endregion End Function / Method
    }
}
