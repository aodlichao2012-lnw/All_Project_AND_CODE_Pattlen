using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.Sync;
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
    public partial class wSyncData : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        private bool bW_Upload = false;

        //*Net 63-01-10 สร้าง DataTable มาเก็บข้อมูลตารางเพื่อไป Filter
        private DataTable oSyncDownloadTable;

        #endregion End Variable

        public wSyncData()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                if (cVB.oVB_MQ != null) cVB.oVB_MQ.oEv_Jump += new EventHandler(W_Notification);
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();
                //otbDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);
                W_GETxSyncHistory();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "wSyncData : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
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
        /// Close form
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Enable - Disable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ockRecentUpdate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ockRecentUpdate.Checked)
                {
                    otpDateSycn.Visible = false;
                    olaTitleDateCompare.Visible = false;
                }
                else
                {
                    otpDateSycn.Visible = true;
                    olaTitleDateCompare.Visible = true;
                    //otpDateSycn.Enabled = true;
                    otpDateSycn.ResetText();
                    otpDateSycn.Focus();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ockRecentUpdate_CheckedChanged : " + oEx.Message); }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                ocmDownload.BackColor = cVB.oVB_ColDark;
                ocmUpload.BackColor = cVB.oVB_ColDark;
                //ogdUpload.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;    //*Arm 62-10-21
                //ogdDownload.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdDownload); //*Net 63-03-03 Set Design Gridview
                oW_SP.SP_SETxSetGridviewFormat(ogdUpload); //*Net 63-03-03 Set Design Gridview
                ocmOKSync.BackColor = cVB.oVB_ColNormal;

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                ocmKB.BackColor = cVB.oVB_ColDark;
                ocmCalculate.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmHelp.BackColor = cVB.oVB_ColDark;
                ocmAbout.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set tex form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resSync_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSync_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "SYNC";
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
                ocmUpload.Text = "".PadLeft(10) + oW_Resource.GetString("tUpload");
                ocmDownload.Text = "".PadLeft(10) + oW_Resource.GetString("tDownload");

                olaTitle.Text = oW_Resource.GetString("tDownload");
                ockRecentUpdate.Text = oW_Resource.GetString("tRecentUpdate");
                olaTitleDateCompare.Text = oW_Resource.GetString("tDateCompare");
                olaTitleDateCompareU.Text = oW_Resource.GetString("tDateCompare"); //*Net 63-01-13 label date หน้า upload 

                otbTitleLastDateDwn.HeaderText = oW_Resource.GetString("tTitleLastDateDwn"); //*Net 62-12-26 tLastDate->tTitleLastDateDwn
                otbTitleNameDwn.HeaderText = oW_Resource.GetString("tTitleNameDwn");

                otbTitleLastDateUpl.HeaderText = oW_Resource.GetString("tTitleLastDateUpl"); //*Net 62-12-26 tLastDate->tTitleLastDateUpl
                otbTitleNameUpl.HeaderText = oW_Resource.GetString("tTitleNameUpl"); //*Net 62-12-26 tNameTable->tTitleNameUpl

                ockSelectAll.Text = oW_Resource.GetString("tSelectAll");
                ockSelectAllU.Text = oW_Resource.GetString("tSelectAll");

                olaType.Text = oW_Resource.GetString("tType"); //*Net 63-01-13 label type หน้า upload 

                olaSearchD.Text = oW_Resource.GetString("tSearch");
                // user
                olaUsrName.Text = new cUser().C_GETtUsername();
                opbUsr.Image = new cUser().C_GEToImageUsr(cVB.tVB_UsrCode, "TCNMUser"); //*Em 62-09-04
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "W_SETxText : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ocmMenu_Click : " + oEx.Message); }
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
            catch (Exception ex) { new cLog().C_WRTxLog("wSyncData", "ocmKB_Click : " + ex.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ocmShwKb_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ocmCalculate_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ocmHelp_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ocmAbout_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Closing Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSyncData_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "wSyncData_FormClosing : " + oEx.Message); }
        }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmDownload_Click(object sender, EventArgs e)
        {
            try
            {
                opnUpload.Visible = false;
                opnDownload.Visible = true;
                //otbLogSync.Visible = true;
                ogdDownload.Visible = true;
                ocmUpload.Visible = true;
                //ogdUpload.Visible = false;
                ocmDownload.Visible = false;
                bW_Upload = false;
                olaTitle.Text = oW_Resource.GetString("tDownload");
                opbSync.Image = Properties.Resources.DownloadDBB_32;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ocmDownload_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmUpload_Click(object sender, EventArgs e)
        {
            try
            {
                W_GETxUploadType();
                opnUpload.Visible = true;
                ocmOKSync.Visible = true;
                //otbLogSync.Visible = false;
                opnDownload.Visible = false;
                ocmDownload.Visible = true;
                ogdDownload.Visible = false;
                ocmUpload.Visible = false;
                bW_Upload = true;
                olaTitle.Text = oW_Resource.GetString("tUpload");
                opbSync.Image = Properties.Resources.UploadDBB_32;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "ocmUpload_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "otmClose_Tick : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSync:", "ocmNotify_Click : " + oEx.Message); }
        }

        private void W_GETxSyncHistory(bool pbUpload = false)
        {
            StringBuilder oSql;
            List<cmlSyncHistory> aoSyncH;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ");
                oSql.AppendLine("DT.FNSynSeqNo,");
                oSql.AppendLine("DT.FTSynTable,");  //*Em 62-09-04
                oSql.AppendLine("DT.FTSynTable_L,");
                oSql.AppendLine("DT.FDSynLast,");
                oSql.AppendLine("DT.FTSynUriDwn,");
                oSql.AppendLine("DT.FTSynUriUld,");
                oSql.AppendLine("DT_L.FTSynName ");
                oSql.AppendLine("FROM TSysSyncData DT WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TSysSyncData_L DT_L ON DT_L.FNSynSeqNo = DT.FNSynSeqNo AND DT_L.FNLngID = " + cVB.nVB_Language + " ");
                oSql.AppendLine("INNER JOIN TSysSyncModule MD ON MD.FNSynSeqNo = DT.FNSynSeqNo ");
                oSql.AppendLine("WHERE MD.FTAppCode = 'PS' AND DT.FTSynStaUse = '1' ");

                if (pbUpload)
                {
                    oSql.AppendLine("AND (DT.FTSynType != 1 OR DT.FTSynType != 3)");
                }
                else
                {
                    oSql.AppendLine("AND (DT.FTSynType = 1 OR DT.FTSynType = 3)");
                }

                aoSyncH = new List<cmlSyncHistory>();
                aoSyncH = new cDatabase().C_GETaDataQuery<cmlSyncHistory>(oSql.ToString());
                
                //*Net 63-01-10 Query Data มาใส่ DataTable เพื่อเอาไป filer
                oSyncDownloadTable = new cDatabase().C_GEToDataQuery(oSql.ToString());

                ogdDownload.Rows.Clear();
                if (aoSyncH != null)
                {
                    foreach (var oItem in aoSyncH)
                    {
                        ogdDownload.Rows.Add(false, oItem.FTSynName, oItem.FDSynLast, oItem.FTSynUriDwn,oItem.FNSynSeqNo,oItem.FTSynTable);
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "W_GETxSyncHistory : " + oEx.Message);
            }
            finally
            {
                aoSyncH = null;
                oSql = null;
            }
        }

        private void ogdDownload_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ogdDownload.CurrentCell == null || ogdDownload.CurrentRow == null)
                {
                    return;
                }

                int nColumnIndex = ogdDownload.CurrentCell.ColumnIndex;

                if (nColumnIndex == 0)
                {
                    bool bChk = Convert.ToBoolean(ogdDownload.CurrentRow.Cells[0].Value);
                    if (bChk)
                    {
                        ogdDownload.CurrentRow.Cells[0].Value = false;
                    }
                    else
                    {
                        ogdDownload.CurrentRow.Cells[0].Value = true;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "ogdUpload_CellClick : " + oEx.Message);
            }
        }

        private void ocmOKSync_Click(object sender, EventArgs e)
        {
            try
            {
                string tDateSelect = string.Empty;

                if (bW_Upload)
                {
                    W_SYNxUpload();
                }
                else
                {
                    W_SYNxDownload();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "ocmOKSync_Click : " + oEx.Message);
            }
        }

        private void W_SYNxDownload()
        {
            cmlMsgLng oMsgL;
            cmlTSysSyncData oSync;
            string tDate, tSyncData;
            try
            {
                oMsgL = new cmlMsgLng();
                List<DataGridViewRow> aoRow = (from DataGridViewRow oRw in ogdDownload.Rows
                                               where Convert.ToBoolean(oRw.Cells[0].Value) == true
                                               select oRw).ToList();
                opgStatus.Step = 0;
                opgStatus.Maximum = aoRow.Count;
                opgStatus.Minimum = 0;
                int nCount = 0;
                cVB.nVB_SyncSuccess = 0; //*Net 63-03-30 เริ่มต้นนับ
                foreach (DataGridViewRow oRw in aoRow)
                {
                    nCount += 1;
                    olaStatus.Text = string.Format("Status : Download ({0}/{1})", nCount, opgStatus.Maximum);
                    olaStatus.Update();
                    oSync = new cmlTSysSyncData();
                    oSync.FTSynName = oRw.Cells[1].Value.ToString();
                    oSync.FTSynUriDwn = oRw.Cells[3].Value.ToString();
                    oSync.FNSynSeqNo = Convert.ToInt32(oRw.Cells[4].Value.ToString());  //*Em 62-09-04
                    oSync.FTSynTable = oRw.Cells[5].Value.ToString(); //*Em 62-09-04
                    if (ockRecentUpdate.Checked)
                    {
                        tDate = Convert.ToDateTime(oRw.Cells[2].Value).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        tDate = otpDateSycn.Value.ToString("yyyy-MM-dd");
                    }

                    new cSyncData().C_SYNxDownload(oSync, tDate);
                    olaStatusTable.Text = string.Format("{0} of {1})", oSync.FTSynName, tDate);
                    olaStatusTable.Update();
                    opgStatus.Value = nCount;
                    opgStatus.PerformStep();
                    opgStatus.Update();

                    cSP.SP_GETxCountNotify(olaMsgCount, opnNotify); //*Net 63-02-04 แสดง Noti หลังจาก sync เสร็จ 1 รายการ
                }
                //if(cVB.nVB_SyncError >= 1) //*Net 63-03-30 Check ไม่สำเร็จ
                if(opgStatus.Maximum - cVB.nVB_SyncSuccess > 0)
                {
                    olaStatusTable.Text = oW_Resource.GetString("tSycDwnError");
                    olaStatusTable.Update();
                    tSyncData = string.Format(oW_Resource.GetString("tSycDwnError")+ ("\n") + oW_Resource.GetString("tSuccess")+(": {0}")+ ("\n") + oW_Resource.GetString("tUnSuccess")+(": {1}"), cVB.nVB_SyncSuccess, (opgStatus.Maximum - cVB.nVB_SyncSuccess)); //*Net 63-03-30 แสดงสำเร็จ/ไม่่สำเร็จ
                    new cSP().SP_SHWxMsg(tSyncData, 2);
                }
                else
                {
                    olaStatusTable.Text = oW_Resource.GetString("tSycDwnSuccess");
                    olaStatusTable.Update();
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tSycDwnSuccess"), 1);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "W_SYNxDownload : " + oEx.Message);
            }
            finally
            {
                cVB.nVB_SyncSuccess = 0;
                cVB.nVB_SyncError = 0;
                tSyncData = null;
                oSync = null;
                tDate = null;
                new cSP().SP_CLExMemory();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void W_SYNxUpload()
        {
            try
            {

                List<DataGridViewRow> aoRow = (from DataGridViewRow oRw in ogdUpload.Rows
                                               where Convert.ToBoolean(oRw.Cells[0].Value) == true
                                               select oRw).ToList();

                opgStatus.Step = 0;
                opgStatus.Maximum = aoRow.Count + 1;
                opgStatus.Minimum = 0;
                int nCount = 0;
                foreach (DataGridViewRow oRw in aoRow)
                {
                    nCount += 1;
                    olaStatus.Text = string.Format("Status : Upload ({0}/{1})", nCount, opgStatus.Maximum - 1);
                    olaStatus.Update();
                    string tDocNo = oRw.Cells[1].Value.ToString();
                    string tDate = string.Empty;

                    //if (ockRecentUpdateU.Checked)
                    //{
                    //    tDate = otpDateSycnU.Value.ToString("yyyy-MM-dd");
                    //}
                    //else
                    //{
                    tDate = Convert.ToDateTime(oRw.Cells[2].Value).ToString("yyyy-MM-dd");
                    //}

                    int nItemSelect = int.Parse(ocbType.SelectedValue.ToString());
                    string tValue = ocbType.SelectedText.ToString();
                    cSale.C_PRCxAdd2TmpLogChg(nItemSelect, tDocNo,true);

                    olaStatusTable.Text = string.Format("{0} of {1})", tDocNo, tDate);
                    olaStatusTable.Update();
                    opgStatus.Value = nCount;
                    opgStatus.PerformStep();
                    opgStatus.Update();
                }

                new cSyncData().C_PRCxSyncUld();
                opgStatus.Value += 1;
                opgStatus.PerformStep();
                opgStatus.Update();
                olaStatusTable.Text = oW_Resource.GetString("tUploadSuccess");
                olaStatusTable.Update();
                new cSP().SP_SHWxMsg(oW_Resource.GetString("tUploadSuccess"), 1);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "W_SYNxUpload : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        private void ocbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow oRw in ogdDownload.Rows)
                {
                    oRw.Cells[0].Value = ockSelectAll.Checked;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "ocbSelectAll_CheckedChanged : " + oEx.Message);
            }
        }

        private void W_GETxUploadType()
        {
            StringBuilder oSql;
            List<cmlTSysSyncData_L> aoSync_L;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ");
                oSql.AppendLine("FNSynSeqNo,");
                oSql.AppendLine("FTSynName ");
                oSql.AppendLine("FROM TSysSyncData_L ");
                oSql.AppendLine("WHERE FNLngID = " + cVB.nVB_Language + " AND (FNSynSeqNo = 80 OR FNSynSeqNo = 81 OR FNSynSeqNo = 82 OR FNSynSeqNo = 90) ");
                //if (ockRecentUpdateU.Checked)
                //{
                //    oSql.AppendLine(" AND (DT.FTSynType = 1 OR DT.FTSynType = 3) ");
                //}
                oSql.AppendLine("Order by FNSynSeqNo");

                aoSync_L = new List<cmlTSysSyncData_L>();
                aoSync_L = new cDatabase().C_GETaDataQuery<cmlTSysSyncData_L>(oSql.ToString());

                if (aoSync_L != null)
                {
                    if (ocbType.Items.Count > 0)
                    {
                        ocbType.Items.Clear();
                    }

                    ocbType.DataSource = aoSync_L;
                    ocbType.DisplayMember = "FTSynName";
                    ocbType.ValueMember = "FNSynSeqNo";

                    //ocbType.SelectedIndex = 0;
                    string tItemSelect = ocbType.SelectedValue.ToString();
                    W_GETxTCNTTmpLogChg(tItemSelect);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "W_GETxUploadType : " + oEx.Message);
            }
        }

        private void ocbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ocbType.SelectedValue == null)
                {
                    return;
                }

                string tItemSelect = ocbType.SelectedValue.ToString();
                W_GETxTCNTTmpLogChg(tItemSelect);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "W_GETxUploadType : " + oEx.Message);
            }
        }

        private void W_GETxTCNTTmpLogChg(string ptLogType)
        {
            StringBuilder oSql;
            List<cmlTCNTTmpLogChg> aoLogChg;
            try
            {
                //*Arm 62-10-29 - switch (ptLogType) 
                oSql = new StringBuilder();
                switch (ptLogType)
                {
                    case "80": //การขาย-คืน
                        oSql.AppendLine("SELECT ");
                        oSql.AppendLine("FTShfCode AS FTLogCode,");
                        oSql.AppendLine("FTXshDocNo AS FTLogDocNo, ");
                        oSql.AppendLine(ptLogType + "AS FNLogType,");
                        oSql.AppendLine("'' AS FTWahCode,");
                        oSql.AppendLine("'' AS FTLogStaPrc,");
                        oSql.AppendLine("FDLastUpdOn AS FDLastUpdOn "); //*Net 62-12-27 ADD
                        oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE CONVERT(varchar(10),FDXshDocDate,121) =  '" + otpDateSycnU.Value.ToString("yyyy-MM-dd") + "'");
                        break;
                    case "81":    //รอบการขาย
                        oSql.AppendLine("SELECT ");
                        oSql.AppendLine("FTShfCode AS FTLogCode,");
                        oSql.AppendLine("FTPosCode AS FTLogDocNo, ");
                        oSql.AppendLine(ptLogType + "AS FNLogType,");
                        oSql.AppendLine("'' AS FTWahCode,");
                        oSql.AppendLine("FDLastUpdOn AS FDLastUpdOn "); //*Net 62-12-27 ADD
                        oSql.AppendLine("FROM TPSTShiftHD WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE CONVERT(varchar(10),FDShdSaleDate,121) =  '" + otpDateSycnU.Value.ToString("yyyy-MM-dd") + "'");
                        break;
                    case "82":    //ยกเลิกบิล-ยกเลิกรายการ
                        oSql.AppendLine("SELECT DISTINCT");
                        oSql.AppendLine("FNVidNo AS FTLogCode,");
                        oSql.AppendLine("'' AS FTLogDocNo, ");
                        oSql.AppendLine(ptLogType + "AS FNLogType,");
                        oSql.AppendLine("'' AS FTWahCode,");
                        oSql.AppendLine("FDLastUpdOn AS FDLastUpdOn "); //*Net 62-12-27 ADD
                        oSql.AppendLine("FROM TPSTVoidDT WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE CONVERT(varchar(10),FDXihDocDate,121) =  '" + otpDateSycnU.Value.ToString("yyyy-MM-dd") + "'");
                        break;
                    case "90":    //ใบกำกำกับภาษี       //*Em 62-08-13
                        oSql.AppendLine("SELECT ");
                        oSql.AppendLine("FTShfCode AS FTLogCode,");
                        oSql.AppendLine("FTXshDocNo AS FTLogDocNo, ");
                        oSql.AppendLine(ptLogType + "AS FNLogType,");
                        oSql.AppendLine("FTWahCode AS FTWahCode," );
                        oSql.AppendLine("FDLastUpdOn AS FDLastUpdOn "); //*Net 62-12-27 ADD
                        oSql.AppendLine("FROM TPSTTaxHD WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE CONVERT(varchar(10),FDXshDocDate,121) =  '" + otpDateSycnU.Value.ToString("yyyy-MM-dd") + "'");
                        break;
                }
                //+++++++++++

                //*Arm 62-10-29 - Comment Code
                //oSql.AppendLine("SELECT DISTINCT ");
                //oSql.AppendLine("FTLogCode,");
                //oSql.AppendLine("FTLogDocNo,");
                //oSql.AppendLine("FNLogType,");
                //oSql.AppendLine("FTWahCode,");
                //oSql.AppendLine("FTLogStaPrc,");
                //oSql.AppendLine("FDCreateOn,");
                //oSql.AppendLine("FDLastUpdOn ");
                //oSql.AppendLine("'' AS FTLogStaPrc,"); //*Em 62-10-01
                //oSql.AppendLine("GETDATE() AS FDCreateOn,");  //*Em 62-10-01
                //oSql.AppendLine("GETDATE() AS FDLastUpdOn ");  //*Em 62-10-01
                //oSql.AppendLine("FROM TCNTTmpLogChg ");
                //oSql.AppendLine("WHERE FNLogType = " + ptLogType + "");
                //oSql.AppendLine(" AND CONVERT(varchar(10),FDCreateOn,121) =  '" + otpDateSycnU.Value.ToString("yyyy-MM-dd") + "'");

                aoLogChg = new List<cmlTCNTTmpLogChg>();
                aoLogChg = new cDatabase().C_GETaDataQuery<cmlTCNTTmpLogChg>(oSql.ToString());

                ogdUpload.Rows.Clear();

                if (aoLogChg != null)
                {
                    foreach (var oItem in aoLogChg)
                    {
                        string tDocNo = oItem.FTLogDocNo;

                        if (ptLogType.Equals("81") || ptLogType.Equals("82"))
                        {
                            tDocNo = oItem.FTLogCode;
                        }

                        ogdUpload.Rows.Add(
                                    false,
                                    tDocNo,
                                    oItem.FDLastUpdOn  //*Net 62-12-27 ADD
                        );
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "W_GETxTCNTTmpLogChg : " + oEx.Message);
            }
        }

        private void ockSelectAllU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow oRw in ogdUpload.Rows)
                {
                    oRw.Cells[0].Value = ockSelectAllU.Checked;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "ockSelectAllU_CheckedChanged : " + oEx.Message);
            }
        }

        private void ogdUpload_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ogdUpload.CurrentCell == null || ogdUpload.CurrentRow == null)
                {
                    return;
                }

                int nColumnIndex = ogdUpload.CurrentCell.ColumnIndex;

                if (nColumnIndex == 0)
                {
                    bool bChk = Convert.ToBoolean(ogdUpload.CurrentRow.Cells[0].Value);
                    if (bChk)
                    {
                        ogdUpload.CurrentRow.Cells[0].Value = false;
                    }
                    else
                    {
                        ogdUpload.CurrentRow.Cells[0].Value = true;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "ogdUpload_CellContentClick : " + oEx.Message);
            }
        }

        private void otpDateSycnU_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string tItemSelect = ocbType.SelectedValue.ToString();
                W_GETxTCNTTmpLogChg(tItemSelect);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSync:", "otpDateSycnU_ValueChanged : " + oEx.Message);
            }
        }

        //*Net 63-01-13 เพิ่ม Event Textbox สำหรับการ Filter
        private void otbSearchD_TextChanged(object sender, EventArgs e)
        {
            DataView oDataDownload = new DataView(oSyncDownloadTable);
            oDataDownload.RowFilter = "FTSynName like '%" + otbSearchD.Text + "%'";

            ogdDownload.Rows.Clear();
            if (oDataDownload != null)
            {
                foreach (DataRow oItem in oDataDownload.ToTable().Rows)
                {
                    ogdDownload.Rows.Add(false, oItem["FTSynName"], oItem["FDSynLast"], oItem["FTSynUriDwn"], oItem["FNSynSeqNo"], oItem["FTSynTable"]);
                }
            }
        }

        private void wSyncData_KeyDown(object sender, KeyEventArgs e)
        {
            //*Arm 63-02-06 - (HotKey) Created function wSyncData_KeyDown
            try
            {
                W_CALxByName(e);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "wSyncData_KeyDown " + oEx.Message); }

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "W_CALxByName : " + oEx.Message); }
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
                    case "C_KBDxCheckLastSync":
                        //ตรวจสอบ / ไม่ตรวจสอบจากการซิงค์ครั้งล่าสุด
                        if (ockRecentUpdate.Checked == true)
                        {

                            ockRecentUpdate.Checked = false;
                        }
                        else
                        {
                            ockRecentUpdate.Checked = true;
                        }

                        break;

                    case "C_KBDxChooseData":
                        //เลือกข้อมูลทั้งหมด
                        if (opnDownload.Visible == true) // Panel Download
                        {
                            if (ockSelectAll.Checked == true)
                            {
                                ockSelectAll.Checked = false;
                            }
                            else
                            {
                                ockSelectAll.Checked = true;
                            }
                        }

                        if (opnUpload.Visible == true)  // Panel Upload
                        {
                            if (ockSelectAllU.Checked == true)
                            {
                                ockSelectAllU.Checked = false;
                            }
                            else
                            {
                                ockSelectAllU.Checked = true;
                            }
                        }

                        break;

                    case "C_KBDxAccept":
                        // OKSync
                        ocmOKSync_Click(oSender, oEv);

                        break;

                    case "C_KBDxInputDate":

                        break;

                    case "C_KBDxDownload":
                        //เปิด Page Download
                        ocmDownload_Click(oSender, oEv);

                        break;

                    case "C_KBDxUpload":
                        //เปิด Page Upload
                        ocmUpload_Click(oSender, oEv);

                        break;

                    case "C_KBDxBack":
                        try
                        {
                            new wHome().Show();
                            otmClose.Start();
                        }
                        catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "W_GETxFuncByFuncName " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSyncData", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                oW_SP.SP_CLExMemory();
            }
        }
    }
}
