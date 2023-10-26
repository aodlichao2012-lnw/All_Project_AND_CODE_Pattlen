using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Popup.wReprint;
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
    public partial class wReprint : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Time;

        #endregion End Variable

        public wReprint()
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                oW_SP.SP_PRCxFlickering(this.Handle);

                W_SETxDesign();
                W_SETxText();
                W_SHWxButtonBar();
                W_DATxBillType2Combo();
                W_DATxLoadSale();
                //*Net 63-07-31 ปรับตาม Moshi
                //cSP.SP_GETxCountNotify(olaMsgCount, opnNotify);

                this.KeyPreview = true; //*Arm 63-02-06
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "wReprint : " + oEx.Message); }
        }

        private void opnMenu_MouseLeave(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Cursor.Position.X > 240)
            {
                opnMenu.Width = 55;
            }
        }

        #region Function
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "W_SHWxButtonBar : " + oEx.Message); }
            finally
            {
                aoKb = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                //ocmSearch.BackColor = cVB.oVB_ColNormal;
                //ogdReprint.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdReprint); //*Net 63-03-03 Set Design Gridview

                opnMenu.Width = 50;
                opnMenu.BackColor = cVB.oVB_ColDark;
                ocmMenu.BackColor = cVB.oVB_ColDark;
                opnMenuT.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                opnMenuB.BackColor = cVB.oVB_ColDark;   //*Em 62-01-25  Waterpark
                //ocmKB.BackColor = cVB.oVB_ColDark;
                //ocmCalculate.BackColor = cVB.oVB_ColDark;
                //ocmShwKb.BackColor = cVB.oVB_ColDark;
                //ocmHelp.BackColor = cVB.oVB_ColDark;
                //ocmAbout.BackColor = cVB.oVB_ColDark;
                //ocmBack.BackColor = cVB.oVB_ColDark;
                //*Net 63-07-31 ปรับตาม Moshi
                ocmAccept.BackColor = cVB.oVB_ColDark;  //*Em 63-07-28
                ocmSearch.BackColor = cVB.oVB_ColDark;  //*Em 63-07-28
                opbLogo.Image = new cCompany().C_GEToImageLogo();   //*Em 63-07-28

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resReprint_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resReprint_EN));
                        break;
                }

                cVB.tVB_KbdScreen = "REPRINT";
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

                olaReprint.Text = oW_Resource.GetString("tReprint");
                olaTitleDocNo.Text = oW_Resource.GetString("tDocNo");
                olaTitleSaleDate.Text = oW_Resource.GetString("tSaleDate");
                olaTitleBillType.Text = oW_Resource.GetString("tBillType");
                otbTitleAmt.HeaderText = oW_Resource.GetString("tAmount");
                otbTitleDatetime.HeaderText = oW_Resource.GetString("tDatetime");
                otbTitleDocNo.HeaderText = oW_Resource.GetString("tDocNo");
                otbTitlePos.HeaderText = cVB.oVB_GBResource.GetString("tPos");

                odtSaleDate.Text = Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy");

                // user
                olaUsrName.Text = new cUser().C_GETtUsername();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "W_SETxText : " + oEx.Message); }
        }

        private void W_DATxLoadSale()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            try
            {
                oSql.AppendLine("SELECT FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand,'1' AS FTStaFrm");
                oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"'");
                oSql.AppendLine("AND CONVERT(VARCHAR(10),FDXshDocDate,121) = '"+ odtSaleDate.Value.ToString("yyyy-MM-dd") +"'");
                if (!string.IsNullOrEmpty(otbSearch.Text))
                {
                    oSql.AppendLine("AND FTXshDocNo LIKE'%"+ otbSearch.Text.Trim() +"%'");
                }
                switch (ocbBillType.SelectedIndex)
                {
                    case 1:
                        oSql.AppendLine("AND FNXshDocType = 1");
                        break;
                    case 2:
                        oSql.AppendLine("AND FNXshDocType = 9");
                        break;
                }
                //*Em 63-05-25
                oSql.AppendLine("UNION");
                oSql.AppendLine("SELECT FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand,'2' AS FTStaFrm");
                oSql.AppendLine("FROM "+ cSale.tC_TblSalHD +" WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshStaDoc = '1'");
                oSql.AppendLine("AND CONVERT(VARCHAR(10),FDXshDocDate,121) = '" + odtSaleDate.Value.ToString("yyyy-MM-dd") + "'");
                if (!string.IsNullOrEmpty(otbSearch.Text))
                {
                    oSql.AppendLine("AND FTXshDocNo LIKE'%" + otbSearch.Text.Trim() + "%'");
                }
                switch (ocbBillType.SelectedIndex)
                {
                    case 1:
                        oSql.AppendLine("AND FNXshDocType = 1");
                        break;
                    case 2:
                        oSql.AppendLine("AND FNXshDocType = 9");
                        break;
                }
                //++++++++++++++++
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                ogdReprint.Rows.Clear();
                if (odtTmp.Rows.Count > 0)
                {
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        //ogdReprint.Rows.Add(oRow["FTXshDocNo"], oRow["FDXshDocDate"], oRow["FTPosCode"], oRow["FCXshGrand"]);
                        //ogdReprint.Rows.Add(oRow["FTXshDocNo"], oRow["FDXshDocDate"], oRow["FTPosCode"], oRow["FCXshGrand"],oRow["FTStaFrm"]);//*Em 63-05-25
                        ogdReprint.Rows.Add(oRow["FTXshDocNo"], oRow["FDXshDocDate"], oRow["FTPosCode"], new cSP().SP_SETtDecShwSve(1, (decimal)oRow["FCXshGrand"], cVB.nVB_DecShow), oRow["FTStaFrm"]);//*Em 63-06-07
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "W_DATxLoadSale : " + oEx.Message); }
        }

        private void W_DATxBillType2Combo()
        {
            try
            {
                ocbBillType.Items.Clear();
                ocbBillType.Items.Add(oW_Resource.GetString("tAll"));
                ocbBillType.Items.Add(oW_Resource.GetString("tSale"));
                ocbBillType.Items.Add(oW_Resource.GetString("tReturn"));
                ocbBillType.SelectedIndex = 0;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "W_DATxBillType2Combo : " + oEx.Message); }
        }
        #endregion Function

        #region Method/Events
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "ocmMenu_Click : " + oEx.Message); }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            W_DATxLoadSale();
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "otmClose_Tick : " + oEx.Message); }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wReprint_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "wReprint_FormClosing : " + oEx.Message); }
        }

        private void ocmNotify_Click(object sender, EventArgs e)
        {
            try
            {
                cSP.SP_CHKxNotify(olaMsgCount, opnNotify);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "ocmNotify_Click : " + oEx.Message); }
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
                            new wHome().Show();
                            otmClose.Start();
                        }
                        catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "ocmMenuBar_Click " + oEx.Message); }
                        break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(tFuncName);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "ocmMenuBar_Click : " + oEx.Message); }
        }

        private void odtSaleDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                W_DATxLoadSale();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "odtSaleDate_ValueChanged : " + oEx.Message); }
        }

        private void ogdReprint_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            wReprintPreview oFormView;
            try
            {
                if (e.ColumnIndex < 0) return;
                if (e.RowIndex < 0) return;

                //oFormView = new wReprintPreview(ogdReprint.Rows[e.RowIndex].Cells["otbTitleDocNo"].Value.ToString());
                oFormView = new wReprintPreview(ogdReprint.Rows[e.RowIndex].Cells["otbTitleDocNo"].Value.ToString(), ogdReprint.Rows[e.RowIndex].Cells["otbTitleStaFrm"].Value.ToString()); //*Em 63-05-25
                oFormView.ShowDialog();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "ogdReprint_CellDoubleClick : " + oEx.Message); }
        }

        private void ocbBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                W_DATxLoadSale();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "ocbBillType_SelectedIndexChanged : " + oEx.Message); }
        }


        private void otbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //switch(e.KeyCode)
                //{
                //    case Keys.Enter: ocmSearch_Click(ocmSearch, new EventArgs());
                //        break;
                //}
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        ocmSearch_Click(ocmSearch, new EventArgs());
                        otbSearch.SelectAll();
                        break;
                    case Keys.Up:
                        if (ogdReprint.CurrentRow.Index > 0)
                        {
                            ogdReprint.ClearSelection();
                            //ogdSearch.Rows[ogdSearch.CurrentRow.Index - 1].Selected = true; //*Net 63-03-02 กดแล้ว current row index ไม่เปลี่ยน
                            ogdReprint.CurrentCell = ogdReprint.Rows[ogdReprint.CurrentRow.Index - 1].Cells[0]; //*Net 63-03-02 เลือกแถวถัดไป
                        }
                        e.Handled = true;
                        break;

                    case Keys.Down:
                        if (ogdReprint.Rows.Count - 1 > ogdReprint.CurrentRow.Index)
                        {
                            ogdReprint.ClearSelection();
                            //ogdSearch.Rows[ogdSearch.CurrentRow.Index + 1].Selected = true; //*Net 63-03-02 กดแล้ว current row index ไม่เปลี่ยน
                            ogdReprint.CurrentCell = ogdReprint.Rows[ogdReprint.CurrentRow.Index + 1].Cells[0]; //*Net 63-03-02 เลือกแถวถัดไป
                        }
                        e.Handled = true;
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "ocbBillType_SelectedIndexChanged : " + oEx.Message); }
        }
        #endregion Method/Events

        private void wReprint_KeyDown(object sender, KeyEventArgs e)
        {
            //*Arm 63-02-06 - (HotKey) Created function wSyncData_KeyDown
            try
            {
                switch (e.KeyCode) //*Net 63-03-04
                {
                    case Keys.F10:
                        ocmAccept_Click(ocmAccept, new EventArgs());
                        break;
                    case Keys.F4:
                        ocmSearch_Click(ocmSearch, new EventArgs());
                        break;
                    case Keys.F3:
                        otbSearch.Focus();
                        break;
                    case Keys.F2:
                        ocbBillType.DroppedDown = !ocbBillType.DroppedDown;
                        if (ocbBillType.DroppedDown) ocbBillType.Focus();
                        else otbSearch.Focus();
                        ocbBillType.DropDownClosed += (oSender, oE) => { otbSearch.Focus(); };
                        break;
                    default:
                        W_CALxByName(e);
                        break;
                }
            }
            catch (Exception oEx)
            {

            }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "W_CALxByName : " + oEx.Message); }
            finally
            {
                poKey = null;
                tFuncName = null;
                //oW_SP.SP_CLExMemory();
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
                switch (ptFuncName)
                {
                    case "C_KBDxBack":
                        try
                        {
                            new wHome().Show();
                            otmClose.Start();
                        }
                        catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "ocmMenuBar_Click " + oEx.Message); }
                        break;
                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprint", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                //oW_SP.SP_CLExMemory();
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            if (ogdReprint.CurrentCell != null)
                ogdReprint_CellDoubleClick(ogdReprint, new DataGridViewCellEventArgs(ogdReprint.CurrentCell.ColumnIndex, ogdReprint.CurrentCell.RowIndex));
        }

        private void wReprint_Shown(object sender, EventArgs e)
        {
            cSP.SP_GETxCountNotify(olaMsgCount, opnNotify); //*Net 63-07-31 ย้ายมาจาก ctor
            otbSearch.Focus();
        }
    }
}
