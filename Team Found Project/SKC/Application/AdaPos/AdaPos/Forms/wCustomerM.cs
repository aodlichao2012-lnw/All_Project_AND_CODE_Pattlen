using AdaPos.Class;
using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using System.IO;
using AdaPos.Models.Other;

namespace AdaPos
{
    public partial class wCustomerM : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;
        private int nW_Rows;
        private int nW_Page = 1;
        private int nW_MaxPage = 0;
        private int nW_CountTime = 0;
        private int nW_RecPerPage = 10;
        private List<cmlTCNMCst> aoW_Cst = new List<cmlTCNMCst>();

        #endregion End Variable

        #region Constructor

        public wCustomerM()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);

                //cVB.tVB_CstCode = null; //*Arm 62-10-27 -Comment Code

                W_SETxDesign();
                W_SETxText();
                W_SHWxButtonBar();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "wCustomerM : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }

        #endregion End Constructor

        #region Event

        /// <summary>
        /// Close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                //new wHome().Show();
                //otmClose.Start();
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCustomerM_Shown(object sender, EventArgs e)
        {
            try
            {
                otbSearchCst.Focus();
                //olaMode2_Click(olaMode2, null);   //*Arm 63-03-30 Comment Code
                olaMode1_Click(olaMode1, null);     //*Arm 63-03-30
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "wCustomerM_Shown : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmKB_Click : " + oEx.Message); }
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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmAbout_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmMenu_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Search Customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                W_SCHxCustomer();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmSearch_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Edit Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdPerson_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex < 0) return;
                
                if (cSale.nC_CntItem > 0)
                {
                    //*Arm 62-10-27 - เช็คมีการเลือกลูกค้าแล้วหรือไม่
                    if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantSelectCst"), 3);
                        this.Close();
                        this.Dispose();
                        return;
                    }
                    //++++++++++

                    if (!string.Equals(cVB.tVB_PriceGroup, ogdPerson.Rows[e.RowIndex].Cells["otbTitlePriceGrp"].Value.ToString()))
                    {
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgDiffPriLev"), 3);
                        this.Close();
                        this.Dispose();
                        return;
                    }
                }

                //*Arm 62-11-07 -แก้ไข Check ค่า Null ถ้าเป็น Null ให้ Set เป็น string.Empty
                cVB.tVB_CstCode = ogdPerson.Rows[e.RowIndex].Cells["otbTitleCstCode"].Value == null ? string.Empty : ogdPerson.Rows[e.RowIndex].Cells["otbTitleCstCode"].Value.ToString();
                cVB.tVB_CstName = ogdPerson.Rows[e.RowIndex].Cells["otbTitleName"].Value == null ? string.Empty : ogdPerson.Rows[e.RowIndex].Cells["otbTitleName"].Value.ToString();    
                cVB.tVB_CstTel = ogdPerson.Rows[e.RowIndex].Cells["otbTitleTel"].Value == null ? string.Empty : ogdPerson.Rows[e.RowIndex].Cells["otbTitleTel"].Value.ToString();
                cVB.tVB_PriceGroup = ogdPerson.Rows[e.RowIndex].Cells["otbTitlePriceGrp"].Value == null ? string.Empty:ogdPerson.Rows[e.RowIndex].Cells["otbTitlePriceGrp"].Value.ToString();
                if (cVB.oVB_2ndScreen != null)
                {
                    cVB.oVB_2ndScreen.olaGrpCst.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitlePriceGrp"].Value == null ? string.Empty : ogdPerson.Rows[e.RowIndex].Cells["otbTitlePriceGrp"].Value.ToString();
                }
                
                cVB.tVB_QMemMsgID = "";
                cVB.bVB_ScanQR = false;         //*Arm 62-12-23

                cVB.tVB_CstStaAlwPosCalSo = ogdPerson.Rows[e.RowIndex].Cells[otbTitleCstStaAlwPosCalSo.Name].Value == null ? string.Empty : ogdPerson.Rows[e.RowIndex].Cells["otbTitleCstStaAlwPosCalSo"].Value.ToString(); //*Arm 63-03-03 อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต(default)
                cVB.nVB_CstPoint = Convert.ToInt32(ogdPerson.Rows[e.RowIndex].Cells[otbTitleCstPoint.Name].Value); //*Arm 63-03-13 Point
                cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  //*Arm 63-03-16 Point ก่อนใช้


                //*Net 63-03-31 เอาข้อมูลลูกค้าลง Model
                cVB.oVB_CstCard = W_GEToCstCard(ogdPerson.Rows[e.RowIndex].Cells["otbTitleCstCode"].Value == null ? string.Empty : ogdPerson.Rows[e.RowIndex].Cells["otbTitleCstCode"].Value.ToString());
                cVB.oVB_CstCard.tCstName = cVB.tVB_CstName;
                cVB.oVB_CstCard.nCstPoint = cVB.nVB_CstPoint;
                cVB.oVB_CstCard.tCstPriceGroup = cVB.tVB_PriceGroup;


                cVB.oVB_Sale.W_SETxTextCst();   //*Arm 62-10-26
                

                this.Close();
                this.Dispose();
               
                //new wCstAddOrEdit().Show();
                //otmClose.Start();
            }
            catch (Exception oEx) {
                new cLog().C_WRTxLog("wCustomerM", "ogdPerson_CellMouseDoubleClick : " + oEx.Message); }
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDelete_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmDelete_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Add Customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAdd_Click(object sender, EventArgs e)
        {
            try
            {
                new wCstAddOrEdit().Show();
                otmClose.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmAdd_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCustomerM_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "wCustomerM_FormClosing : " + oEx.Message); }
        }

        /// <summary>
        /// Call by name or Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbSearchCst_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        W_SCHxCustomer();
                        break;

                    default:
                        W_CALxByName(e);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "otbSearchCst_KeyDown : " + oEx.Message); }
        }

        /// <summary>
        /// Call By name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmMenu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                W_CALxByName(e);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmMenu_KeyDown : " + oEx.Message); }
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
                        catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "ocmMenuBar_Click " + oEx.Message); }
                        break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch
            { }
        }

        private void olaMode1_Click(object sender, EventArgs e)
        {
            olaMode1.Cursor = Cursors.No;
            olaMode1.ForeColor = Color.Black;
            olaMode2.Cursor = Cursors.Hand;
            olaMode2.ForeColor = Color.Gray;
            opnInfoCst.Visible = true;
            opnSearch.Visible = true;
            opnScanQR.Visible = false;
            opnCstInfoScan.Visible = false;  //*Em 62-08-28
            otmFocus.Enabled = false;
        }

        private void olaMode2_Click(object sender, EventArgs e)
        {
            olaMode1.Cursor = Cursors.Hand;
            olaMode1.ForeColor = Color.Gray;
            olaMode2.Cursor = Cursors.No;
            olaMode2.ForeColor = Color.Black;
            opnInfoCst.Visible = false;
            opnSearch.Visible = false;
            opnScanQR.Visible = true;
            opnCstInfoScan.Visible = true;  //*Em 62-08-28
            otmFocus.Enabled = true;
        }

        private void otmFocus_Tick(object sender, EventArgs e)
        {
            nW_CountTime += 1;
            if (nW_CountTime == 3)
            {
                otbScan.Focus();
                nW_CountTime = 0;
            }
        }

        private void otbScan_KeyDown(object sender, KeyEventArgs e)
        {
            string tDataScan = "";
            string[] aData;
            string tDataB = "";
            string[] aDataB;
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    tDataScan = otbScan.Text;
                    cVB.bVB_ScanQR = false;
                    if (!string.IsNullOrEmpty(tDataScan))
                    {
                        aData = tDataScan.Split('|');
                        switch (aData.Length)
                        {
                            case 5: //From Register ใหม่
                                olaTelNo.Text = aData[0];
                                olaPriGrp.Text = aData[3];

                                if (cSale.nC_CntItem > 0)
                                {
                                    //*Arm 62-10-27 - เช็คมีการเลือกลูกค้าแล้วหรือไม่
                                    if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                                    {
                                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgCantSelectCst"), 3);
                                        this.Close();
                                        this.Dispose();
                                        return;
                                    }
                                    // +++++++++

                                    if (!string.Equals(cVB.tVB_PriceGroup, aData[3]))
                                    {
                                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgDiffPriLev"), 3);
                                        this.Close();
                                        this.Dispose();
                                        return;
                                    }
                                }

                                cVB.tVB_CstTel = aData[0];
                                if (string.IsNullOrEmpty(aData[2]))
                                {
                                    cVB.cVB_QRPayAmt = 0;
                                }
                                else
                                {
                                    cVB.cVB_QRPayAmt = Convert.ToDecimal(aData[2]);
                                }
                                
                                cVB.tVB_PriceGroup = aData[3];
                                cVB.tVB_CstCode = aData[4];
                                cVB.bVB_ScanQR = true;

                                cVB.tVB_CstName = "";           //*Arm 62-10-26
                                cVB.tVB_QMemMsgID = "";         //*Arm 62-10-27
                                cVB.oVB_Sale.W_SETxTextCst();   //*Arm 62-10-26

                                otmClose.Start();

                                break;
                            //case 5: //From Smart Locker

                            //    tDataB = aData[4];
                            //    tDataB = new cEncryptDecrypt("1").C_CALtDecrypt(tDataB, "Adasoft");
                            //    aDataB = tDataB.Split('|');
                            //    if (aDataB.Length == 5)
                            //    {
                            //        if (aData[0] == aDataB[0] && aData[1] == aDataB[1] &&
                            //            aData[2] == aDataB[2] && aData[3] == aDataB[3])
                            //        {
                            //            cVB.dVB_QRDateChkExpire = Convert.ToDateTime(aDataB[4]);
                            //            if (cVB.nVB_QRTimeout > 0)
                            //            {
                            //                if (Convert.ToDateTime(cVB.dVB_QRDateChkExpire.AddMinutes(cVB.nVB_QRTimeout)) > Convert.ToDateTime(DateTime.Now))
                            //                {
                            //                    if (cSale.nC_CntItem > 0)
                            //                    {
                            //                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgHaveItem"), 3);
                            //                        this.Close();
                            //                        this.Dispose();
                            //                        return;
                            //                    }
                            //                    //olaTitleQRName.Text = oW_Resource.GetString("tTitleQRParcel");
                            //                    //olaTitleQRCardID.Text = oW_Resource.GetString("tTitelQRPayAmt");
                            //                    //opnCusScanInfo.Visible = true;
                            //                    //opbQRCode.Visible = false;
                            //                    //otbRefCode.Text = aData[0];
                            //                    //otbName.Text = aData[1];
                            //                    //otbIDCard.Text = aData[3];
                            //                    //otbPhoneNo.Text = aData[2];

                            //                    cVB.tVB_CstCode = aData[0];
                            //                    cVB.tVB_ParcelCode = aData[1];
                            //                    cVB.tVB_CstTel = aData[2];
                            //                    cVB.cVB_QRPayAmt = Convert.ToDecimal(aData[3]);
                            //                    //new wSale(3).Show();
                            //                    otmClose.Start();
                            //                }
                            //                else
                            //                {
                            //                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgQRExpire"), 3);
                            //                    otbScan.Text = "";
                            //                    otbScan.Focus();
                            //                    return;
                            //                }
                            //            }
                            //            else
                            //            {
                            //                if (cSale.nC_CntItem > 0)
                            //                {

                            //                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgHaveItem"), 3);
                            //                    this.Close();
                            //                    this.Dispose();
                            //                    return;
                            //                }

                            //                //olaTitleQRName.Text = oW_Resource.GetString("tTitleQRParcel");
                            //                //olaTitleQRCardID.Text = oW_Resource.GetString("tTitelQRPayAmt");
                            //                //opnCusScanInfo.Visible = true;
                            //                //opbQRCode.Visible = false;
                            //                //otbRefCode.Text = aData[0];
                            //                //otbName.Text = aData[1];
                            //                //otbIDCard.Text = aData[3];
                            //                //otbPhoneNo.Text = aData[2];

                            //                cVB.tVB_CstCode = aData[0];
                            //                cVB.tVB_ParcelCode = aData[1];
                            //                cVB.tVB_CstTel = aData[2];
                            //                cVB.cVB_QRPayAmt = Convert.ToDecimal(aData[3]);
                            //                otmClose.Start();
                            //            }
                            //        }
                            //        else
                            //        {
                            //            //รูปแบบข้อมูลไม่ถูกต้อง
                            //            new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgQRFmt"), 3);
                            //            otbScan.Text = "";
                            //            otbScan.Focus();
                            //            return;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        //รูปแบบข้อมูลไม่ถูกต้อง
                            //        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgQRFmt"), 3);
                            //        otbScan.Text = "";
                            //        otbScan.Focus();
                            //        return;
                            //    }
                            //    break;
                            //case 6: //From Register

                            //    tDataB = aData[5];
                            //    tDataB = new cEncryptDecrypt("1").C_CALtDecrypt(tDataB, "Adasoft");
                            //    aDataB = tDataB.Split('|');
                            //    if (aDataB.Length == 6)
                            //    {
                            //        if (aData[0] == aDataB[0] && aData[1] == aDataB[1] &&
                            //            aData[2] == aDataB[2] && aData[3] == aDataB[3] && aData[4] == aDataB[4])
                            //        {
                            //            if (cSale.nC_CntItem > 0)
                            //            {
                            //                if (!string.Equals(cVB.tVB_PriceGroup, aDataB[5]))
                            //                {
                            //                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgDiffPriLev"), 3);
                            //                    this.Close();
                            //                    this.Dispose();
                            //                    return;
                            //                }
                            //            }

                            //            //olaTitleQRName.Text = oW_Resource.GetString("tTitleQRName");
                            //            //olaTitleQRCardID.Text = oW_Resource.GetString("tTitleQRCardID");
                            //            //opnCusScanInfo.Visible = true;
                            //            //opbQRCode.Visible = false;
                            //            //otbRefCode.Text = aData[0];
                            //            //otbName.Text = aData[3];
                            //            //otbIDCard.Text = aData[1];
                            //            //otbPhoneNo.Text = aData[2];

                            //            cVB.tVB_PriceGroup = aDataB[5];
                            //            cVB.tVB_CstCode = aData[0];
                            //            cVB.tVB_ParcelCode = aData[1];
                            //            cVB.tVB_CstTel = aData[2];
                            //            cVB.tVB_CstName = aData[3];
                            //            //new wSale(3).Show();
                            //            otmClose.Start();
                            //        }
                            //        else
                            //        {
                            //            //รูปแบบข้อมูลไม่ถูกต้อง
                            //            new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgQRFmt"), 3);
                            //            otbScan.Text = "";
                            //            otbScan.Focus();
                            //            return;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        //รูปแบบข้อมูลไม่ถูกต้อง
                            //        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgQRFmt"), 3);
                            //        otbScan.Text = "";
                            //        otbScan.Focus();
                            //        return;
                            //    }
                            //    break;

                            default:
                                //รูปแบบข้อมูลไม่ถูกต้อง
                                new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgQRFmt"), 3);
                                otbScan.Text = "";
                                otbScan.Focus();
                                break;
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCustomerM", "otbScan_KeyDown " + oEx.Message);
                //รูปแบบข้อมูลไม่ถูกต้อง
                new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgQRFmt"), 3);
                otbScan.Text = "";
                otbScan.Focus();
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
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmSearch.BackColor = cVB.oVB_ColNormal;
                ocmDelete.BackColor = cVB.oVB_ColNormal;
                ocmAdd.BackColor = cVB.oVB_ColNormal;
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-10-08
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-10-08

                //ogdPerson.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdPerson); //*Net 63-03-03 Set Design Gridview

                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode,"TCNMUser");
                opbLogo.Image = new cCompany().C_GEToImageLogo();

                if (opbLogo.Image != null)
                    opbLogo.Visible = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "W_SETxDesign : " + oEx.Message); }
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

                cVB.tVB_KbdScreen = "CUSTOMERM";
                //*Em 62-09-09
                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;

                olaCst.Text = oW_Resource.GetString("tCst");

                // user
                olaUsrName.Text = new cUser().C_GETtUsername();

                // Main
                olaTitleAddress.Text = oW_Resource.GetString("tAddress") + " :";
                olaTitleTelephone.Text = oW_Resource.GetString("tTelephone") + " :";
                olaTitlePoint.Text = oW_Resource.GetString("tPoint") + " :";
                olaTitleLevel.Text = oW_Resource.GetString("tPriLevel") + " :";
                olaTitleEmail.Text = oW_Resource.GetString("tEmail") + " :";
                olaTitleGroup.Text = oW_Resource.GetString("tGroup") + " :";
                olaTitleExp.Text = oW_Resource.GetString("tExpire") + " :";
                otbTitleCstCode.HeaderText = cVB.oVB_GBResource.GetString("tCode");
                otbTitleName.HeaderText = cVB.oVB_GBResource.GetString("tName");
                otbTitleTel.HeaderText = oW_Resource.GetString("tTelephone");
                otbTitleEmail.HeaderText = oW_Resource.GetString("tEmail");
                otbTitleGrpCst.HeaderText = oW_Resource.GetString("tGroup");
                olaTitleSearch.Text = oW_Resource.GetString("tSearch");
                olaTitleShowData.Text = oW_Resource.GetString("tShowData");

                // Search Customer By
                ocbSearchCstBy.Items.Add(cVB.oVB_GBResource.GetString("tCode"));
                ocbSearchCstBy.Items.Add(cVB.oVB_GBResource.GetString("tName"));
                ocbSearchCstBy.Items.Add(oW_Resource.GetString("tTelephone"));
                ocbSearchCstBy.Items.Add(oW_Resource.GetString("tEmail"));
                ocbSearchCstBy.Items.Add(oW_Resource.GetString("tGroup"));
                ocbSearchCstBy.SelectedIndex = 1;

                // Search : Match
                ocbSchCstMatch.Items.Add(cVB.oVB_GBResource.GetString("tPartField"));
                ocbSchCstMatch.Items.Add(cVB.oVB_GBResource.GetString("tWholeField"));
                ocbSchCstMatch.SelectedIndex = 0;

                olaMode1.Text = oW_Resource.GetString("tModeSearch");
                olaMode2.Text = oW_Resource.GetString("tModeScan");
                olaTitleScanQR.Text = oW_Resource.GetString("tTitleScanQR");

                //olaTitleQRRefCode.Text = oW_Resource.GetString("tTitleQRRefCode");
                //olaTitleQRName.Text = oW_Resource.GetString("tTitleQRName");
                //olaTitleQRCardID.Text = oW_Resource.GetString("tTitleQRCardID");
                //olaTitleQRTel.Text = oW_Resource.GetString("tTitleQRTel");

                olaTitleTel.Text = oW_Resource.GetString("tTelephone");
                olaTitlePriGrp.Text = oW_Resource.GetString("tPriGrp");
                if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                    olaBranch.Text = cVB.tVB_BchName;
                else
                    olaBranch.Text = cVB.tVB_ShpName;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "W_SETxText : " + oEx.Message); }
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
        /// Get Customer
        /// </summary>
        private void W_SCHxCustomer()
        {
            aoW_Cst = new List<cmlTCNMCst>();
            int nRowMax = 0;
            StringBuilder oSql = new StringBuilder();
            try
            {

                //if (oW_SP.SP_CHKbConnection())
                //    aoW_Cst = new cCustomer().C_SCHaCstServer();
                if (aoW_Cst.Count == 0)
                    aoW_Cst = new cCustomer().C_SCHaCstLocal(nW_Rows, ocbSearchCstBy.SelectedIndex, otbSearchCst.Text, ocbSchCstMatch.SelectedIndex);

                if (aoW_Cst.Count == 0)
                    nW_MaxPage = 1;
                else
                {
                    nW_MaxPage = Convert.ToInt32(Math.Ceiling(aoW_Cst.Count / Convert.ToDecimal(nW_RecPerPage)));
                    if (nW_MaxPage == 0 || nW_MaxPage == 1)
                    {
                        nRowMax = aoW_Cst.Count;
                        ocmSchNextPage.Visible = false;
                        ocmSchPrevious.Visible = false;
                    }
                    else
                    {
                        nRowMax = nW_RecPerPage;
                        ocmSchNextPage.Visible = true;
                        ocmSchPrevious.Visible = true;
                        ocmSchPrevious.Enabled = false;
                    }
                }

                ogdPerson.Rows.Clear();
                for (int nRow = 0; nRow < nRowMax; nRow++)
                {
                    cmlTCNMCst oCst = aoW_Cst[nRow];
                    ogdPerson.Rows.Add(null, oCst.FTCstCode, oCst.FTCstName, oCst.FTCstTel, oCst.FTCstEmail, oCst.FTCgpName, oCst.FTCgpCode, oCst.FTPplCodeRet, oCst.FTClvName, oCst.FTCstImage, oCst.FTCstStaAlwPosCalSo, oCst.cCstPoint); //*Arm 63-03-04 เพิ่ม oCst.FTCstStaAlwPosCalSo
                }

                olaPageCst.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), aoW_Cst.Count, nW_Page, nW_MaxPage);
                olaTitleShowData.Visible = false;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "W_SCHxCustomer : " + oEx.Message); }
            finally
            {
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "W_CALxByName : " + oEx.Message); }
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
                    case "C_KBDxMenu":
                        W_SETxOpenCloseMenu();
                        break;

                    case "C_KBDxBack":
                        //new wHome().Show();
                        //otmClose.Start();
                        this.Close();
                        this.Dispose();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// เปิด Menu แบบเต็ม / เปิด Menu เป็น Icon
        /// </summary>
        private void W_SETxOpenCloseMenu()
        {
            try
            {
                if (opnMenu.Width <= 100)
                    opnMenu.Width = 250;
                else
                    opnMenu.Width = 55;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCustomerM", "W_SETxOpenCloseMenu : " + oEx.Message); }
            finally
            {
                otbSearchCst.Focus();
            }
        }

        #endregion End Function / Method

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCustomerM:", "ocmNotify_Click : " + oEx.Message);
            }
        }

        private void opnScanQR_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (opnScanQR.Visible == true)
                {
                    otbScan.Focus();
                    otmFocus.Start();
                }
                else
                {
                    otmFocus.Stop();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCustomerM:", "opnScanQR_VisibleChanged : " + oEx.Message);
            }
        }

        private void ogdPerson_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //*Arm 62-11-11 -Clear String
                olaTitleCstName.Text = "";
                olaTelephone.Text = "";
                olaEmail.Text = "";
                olaGrpcst.Text = "";
                olaLevel.Text = "";
                string tImagePath = "";
                olaPoint.Text = ""; //*Arm 63-03-13
                //+++++++


                if (e.RowIndex < 0) return;
                if (e.ColumnIndex < 0) return;
                opbCst.Image = Properties.Resources.CstDefault_256;
                
                olaTitleCstName.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitleName"].Value.ToString();
                olaTelephone.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitleTel"].Value.ToString();
                olaEmail.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitleEmail"].Value.ToString();
                olaGrpcst.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitleGrpCst"].Value.ToString();
                olaLevel.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitlePriceGrp"].Value.ToString();
                //string tImagePath = ogdPerson.Rows[e.RowIndex].Cells["otbFTCstImage"].Value == null ? string.Empty : ogdPerson.Rows[e.RowIndex].Cells["otbFTCstImage"].Value.ToString();
                tImagePath = ogdPerson.Rows[e.RowIndex].Cells["otbFTCstImage"].Value == null ? string.Empty : ogdPerson.Rows[e.RowIndex].Cells["otbFTCstImage"].Value.ToString();
                if (!string.IsNullOrEmpty(tImagePath) && !File.Exists(tImagePath))
                {
                    opbCst.Image = Image.FromFile(tImagePath);
                }
                olaPoint.Text = ogdPerson.Rows[e.RowIndex].Cells[otbTitleCstPoint.Name].Value.ToString(); //*Arm 63-03-13
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCustomerM:", "ogdPerson_CellClick : " + oEx.Message);
            }
        }

        private void ocmSchPrevious_Click(object sender, EventArgs e)
        {
            int nRowMax;

            try
            {
                nW_Page = nW_Page - 1;
                nRowMax = nW_Page * nW_RecPerPage;

                if (nRowMax > aoW_Cst.Count) nRowMax = aoW_Cst.Count;

                ocmSchNextPage.Enabled = true;
                if (nW_Page == 1) ocmSchPrevious.Enabled = false;

                ogdPerson.Rows.Clear();
                for (int nRow = ((nW_Page - 1) * nW_RecPerPage); nRow < nRowMax; nRow++)
                {
                    cmlTCNMCst oCst = aoW_Cst[nRow];
                    ogdPerson.Rows.Add(ogdPerson.Rows.Count, oCst.FTCstCode, oCst.FTCstName, oCst.FTCstTel, oCst.FTCstEmail, oCst.FTCgpName, oCst.FTCgpCode, oCst.FTPplCodeRet, oCst.FTClvName, otbFTCstImage);
                }

                olaPageCst.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), aoW_Cst.Count, nW_Page, nW_MaxPage);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCustomerM:", "ocmSchNextPage_Click : " + oEx.Message);
            }
        }

        private void ocmSchNextPage_Click(object sender, EventArgs e)
        {
            int nRowMax;

            try
            {
                nW_Page = nW_Page + 1;
                nRowMax = nW_Page * nW_RecPerPage;

                if (nRowMax > aoW_Cst.Count) nRowMax = aoW_Cst.Count;

                ocmSchPrevious.Enabled = true;
                if (nRowMax >= aoW_Cst.Count) ocmSchNextPage.Enabled = false;

                ogdPerson.Rows.Clear();
                for (int nRow = ((nW_Page - 1) * nW_RecPerPage); nRow < nRowMax; nRow++)
                {
                    cmlTCNMCst oCst = aoW_Cst[nRow];
                    ogdPerson.Rows.Add(null, oCst.FTCstCode, oCst.FTCstName, oCst.FTCstTel, oCst.FTCstEmail, oCst.FTCgpName, oCst.FTCgpCode, oCst.FTPplCodeRet);
                }

                olaPageCst.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), aoW_Cst.Count, nW_Page, nW_MaxPage);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCustomerM:", "ocmSchNextPage_Click : " + oEx.Message);
            }
        }

        private void ogdPerson_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex < 0) return;
                olaTitleCstName.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitleName"].Value.ToString();
                olaTelephone.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitleTel"].Value.ToString();
                olaEmail.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitleEmail"].Value.ToString();
                olaGrpcst.Text = ogdPerson.Rows[e.RowIndex].Cells["otbTitleGrpCst"].Value.ToString();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCustomerM:", "ogdPerson_RowEnter : " + oEx.Message);
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

        private cmlCstCard W_GEToCstCard(string ptCstCode)
        {
            cDatabase oDB;
            StringBuilder oSql;
            try
            {
                if (string.IsNullOrEmpty(ptCstCode)) return null;

                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine($"SELECT CST.FTCstCode AS tCstCode,");
                //oSql.AppendLine($"CSTL.FTCstName AS tCstName,");
                oSql.AppendLine($"CST.FTCstTel AS tCstTel,");
                oSql.AppendLine($"CST.FTCstEmail AS tCstEmail,");
                oSql.AppendLine($"CST.FTCstSex tCstSex,");
                oSql.AppendLine($"CST.FTCstCardID AS tCstIDCard,");
                oSql.AppendLine($"CST.FTClvCode AS tCstlvCode,");
                oSql.AppendLine($"CST.FTCstTaxNo AS tCstTaxNo,");
                oSql.AppendLine($"CST.FTPplCodeRet AS tCstPriceGroup,");
                //oSql.AppendLine($"PPL.FTPplName AS tPriceGrpName,");
                oSql.AppendLine($"CSTC.FTCstCrdNo AS tCstCardNo,");
                oSql.AppendLine($"CST.FTCstStaAlwPosCalSo AS tCstStaAlwPosCalSo,");
                //oSql.AppendLine($"PNT.FCTxnPntQty AS nCstPoint,");
                oSql.AppendLine($"CST.FDCstDob AS dCstDOB,");
                oSql.AppendLine($"CSTC.FDCstApply AS dCstApply,");
                oSql.AppendLine($"CSTC.FDCstCrdIssue AS dCstCardIssue,");
                oSql.AppendLine($"FDCstCrdExpire AS dCstCrdExpire");
                oSql.AppendLine($"FROM TCNMCst CST WITH(NOLOCK)");
                //oSql.AppendLine($"INNER JOIN TCNMCst_L CSTL WITH(NOLOCK) ON CST.FTCstCode=CSTL.FTCstCode AND CSTL.FNLngID={cVB.nVB_Language}");
                oSql.AppendLine($"LEFT JOIN TCNMCstCard CSTC WITH(NOLOCK) ON CST.FTCstCode=CSTC.FTCstCode");
                //oSql.AppendLine($"LEFT JOIN TCNTMemPntActive PNT WITH(NOLOCK) ON CST.FTCstCode = PNT.FTMemCode");
                //oSql.AppendLine($"LEFT JOIN TCNMPdtPriList_L PPL WITH(NOLOCK) ON PPL.FTPplCode = CST.FTPplCodeRet AND PPL.FNLngID={cVB.nVB_Language}");
                oSql.AppendLine($"WHERE CST.FTCstCode='{ptCstCode}'");



                return oDB.C_GEToDataQuery<cmlCstCard>(oSql.ToString());
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wCustomerM:", "W_GEToCstCard : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql=null;
                oW_SP.SP_CLExMemory();
            }
            return null;
        }
    }
}
