using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
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

namespace AdaPos.Popup.All
{
    public partial class wSearch2Column : Form
    {
        private ResourceManager oW_Resource;
        public cmlSearch oW_DataSearch;
        private DataTable oW_TblData; //*Net 63-03-02
        public wSearch2Column(string ptTable,string ptWhereBch="")
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
                if (string.IsNullOrEmpty(ptWhereBch))
                {
                    W_GETxDataSearch(ptTable);
                }
                else
                {
                    W_GETxDataSearch(ptTable, ptWhereBch);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "wSearch2Column : " + oEx.Message); }
            finally
            {
                ptTable = null;
                new cSP().SP_CLExMemory();
            }
        }

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "OnPaintBackground : " + oEx.Message); }
        }

        /// <summary>
        /// Close Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "ocmBack_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Form Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSearch2Column_Shown(object sender, EventArgs e)
        {
            otbSearchPdt.Focus(); //*Net 63-02-27 เริ่มต้นให้ Focus ที่ Textbox
        }

        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                
                ocmSearch.BackColor = cVB.oVB_ColNormal;
                //ogdSearch.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdSearch); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "W_SETxDesign : " + oEx.Message); }
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
                        oW_Resource = new ResourceManager(typeof(resSeach_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resSeach_EN));
                        break;
                }

                otbCode.HeaderText = oW_Resource.GetString("tCode");
                otbName.HeaderText = oW_Resource.GetString("tName");

                // Search By
                ocbSearchBy.Items.Add(oW_Resource.GetString("tCode"));
                ocbSearchBy.Items.Add(oW_Resource.GetString("tName"));
                ocbSearchBy.SelectedIndex = 1; //*Net 63-03-02 default ที่ชื่อ

                // Search : Match
                ocbSchMatch.Items.Add(cVB.oVB_GBResource.GetString("tPartField"));
                ocbSchMatch.Items.Add(cVB.oVB_GBResource.GetString("tWholeField"));
                ocbSchMatch.SelectedIndex = 0;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "W_SETxText : " + oEx.Message); }
        }

        /// <summary>
        /// Get data Search
        /// </summary>
        private void W_GETxDataSearch(string ptTable,string ptBchCode="")
        {
            StringBuilder oSql;
            List<cmlSearch> aoSearch;

            try
            {
                oSql = new StringBuilder();
                aoSearch = new List<cmlSearch>();
                List<cmlTCNMUser> aoUser;

                //*Net 63-03-02
                oW_TblData = new DataTable();
                oW_TblData.Columns.Add("tCode");
                oW_TblData.Columns.Add("tName");
                switch (ptTable)
                {
                    case "TCNMPdtUnit":
                        olaTitle.Text = oW_Resource.GetString("tUnit");

                        oSql.AppendLine("SELECT PUT.FTPunCode 'tCode', PUTL.FTPunName 'tName'");
                        oSql.AppendLine("FROM TCNMPdtUnit PUT WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMPdtUnit_L PUTL WITH(NOLOCK) ON PUTL.FTPunCode = PUT.FTPunCode");
                        oSql.AppendLine("   AND PUTL.FNLngID = " + cVB.nVB_Language);
                        break;

                    case "TCNMPdtColor":
                        olaTitle.Text = oW_Resource.GetString("tColor");

                        oSql.AppendLine("SELECT PCL.FTClrCode 'tCode', PCLL.FTClrName 'tName'");
                        oSql.AppendLine("FROM TCNMPdtColor PCL WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMPdtColor_L PCLL WITH(NOLOCK) ON PCLL.FTClrCode = PCL.FTClrCode");
                        oSql.AppendLine("   AND PCLL.FNLngID = " + cVB.nVB_Language);
                        break;
                    //Ping
                    case "GETUserPwd":
                        olaTitle.Text = oW_Resource.GetString("tUser");//*Net 63-03-04
                        //oSql.AppendLine("SELECT DISTINCT USR.FTUsrCode as 'tCode', ISNULL(USRL.FTUsrName,(SELECT TOP 1 FTUsrName FROM TCNMUser_L WITH(NOLOCK) WHERE FTUsrCode = USR.FTUsrCode)) as 'tName'");
                        oSql.AppendLine("SELECT DISTINCT USR.FTUsrCode as 'tCode', USL.FTUsrLogin as 'tName'"); //*Em 62-09-10
                        oSql.AppendLine("FROM TCNMUser USR WITH(NOLOCK)");
                        oSql.AppendLine(" LEFT JOIN TCNMUser_L USRL WITH(NOLOCK) ON USRL.FTUsrCode = USR.FTUsrCode");
                        oSql.AppendLine(" AND USRL.FNLngID = " + cVB.nVB_Language);
                        oSql.AppendLine(" INNER JOIN TCNMUsrLogin USL WITH(NOLOCK) ON USL.FTUsrCode = USR.FTUsrCode AND USL.FTUsrLogType = '1'");
                        oSql.AppendLine($" INNER JOIN TCNTUsrGroup USG WITH(NOLOCK) ON USG.FTUsrCode = USR.FTUsrCode AND (USG.FTBchCode='{cVB.tVB_BchCode}' OR USG.FTBchCode='')"); //*Net เพิ่ม Filter User ที่สามารถ Login ได้เท่านั้น
                        oSql.AppendLine(" WHERE (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),USL.FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),USL.FDUsrPwdExpired,121)) "); //*Em 62-09-10
                        oSql.AppendLine(" ORDER BY USR.FTUsrCode");
                        break;

                    case "GETUserPin":
                        olaTitle.Text = oW_Resource.GetString("tUser");//*Net 63-03-04
                        //oSql.AppendLine("SELECT DISTINCT USR.FTUsrCode as 'tCode', ISNULL(USRL.FTUsrName,(SELECT TOP 1 FTUsrName FROM TCNMUser_L WITH(NOLOCK) WHERE FTUsrCode = USR.FTUsrCode)) as 'tName'");
                        oSql.AppendLine("SELECT DISTINCT USR.FTUsrCode as 'tCode', USL.FTUsrLogin as 'tName'"); //*Em 62-09-10
                        oSql.AppendLine("FROM TCNMUser USR WITH(NOLOCK) ");
                        oSql.AppendLine("LEFT JOIN TCNMUser_L USRL WITH(NOLOCK) ON USRL.FTUsrCode = USR.FTUsrCode ");
                        oSql.AppendLine("AND USRL.FNLngID = " + cVB.nVB_Language);
                        oSql.AppendLine("INNER JOIN TCNMUsrLogin USL WITH(NOLOCK) ON USL.FTUsrCode = USR.FTUsrCode AND USL.FTUsrLogType = '2'");
                        oSql.AppendLine($" INNER JOIN TCNTUsrGroup USG WITH(NOLOCK) ON USG.FTUsrCode = USR.FTUsrCode AND (USG.FTBchCode='{cVB.tVB_BchCode}' OR USG.FTBchCode='')"); //*Net เพิ่ม Filter User ที่สามารถ Login ได้เท่านั้น
                        oSql.AppendLine("WHERE (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(19),USL.FDUsrPwdStart,121) AND CONVERT(VARCHAR(19),USL.FDUsrPwdExpired,121)) "); //*Em 62-09-10
                        oSql.AppendLine("ORDER BY USR.FTUsrCode");
                        break;

                    case "TCNMCst":
                        olaTitle.Text = oW_Resource.GetString("tCst");

                        oSql.AppendLine("SELECT CST.FTCstCode AS 'tCode', ISNULL(CSTL.FTCstName,(SELECT TOP 1 FTCstName FROM TCNMCst_L WITH(NOLOCK) WHERE FTCstCode = CST.FTCstCode)) AS 'tName'");
                        oSql.AppendLine("FROM TCNMCst CST WITH(NOLOCK)");
                        oSql.AppendLine("LEFT JOIN TCNMCst_L CSTL WITH(NOLOCK) ON CSTL.FTCstCode = CST.FTCstCode");
                        oSql.AppendLine("   AND CSTL.FNLngID = " + cVB.nVB_Language);
                        break;

                    case "TCNMPos": //*Em 62-08-20
                        olaTitle.Text = oW_Resource.GetString("tTitlePos");

                        oSql.Clear();
                        oSql.AppendLine("SELECT FTPosCode AS 'tCode',FTPosRegNo AS 'tName'");
                        oSql.AppendLine("FROM TCNMPos WITH(NOLOCK)");
                        oSql.AppendLine("WHERE (FTPosType = '1' OR FTPosType = '2')");
                        oSql.AppendLine($"AND FTBchCode='{ptBchCode}'");
                        break;

                    case "TFNMCouponType":

                        olaTitle.Text = oW_Resource.GetString("tCouponType"); //*Net 63-03-04
                        oSql.Clear();
                        oSql.AppendLine("SELECT CPT.FTCptCode AS 'tCode',");
                        oSql.AppendLine("ISNULL(CPTL.FTCptName,(SELECT TOP 1 FTCptName FROM TFNMCouponType_L WITH(NOLOCK) WHERE FTCptCode = CPT.FTCptCode)) AS 'tName'");
                        oSql.AppendLine("FROM TFNMCouponType CPT WITH(NOLOCK)");
                        oSql.AppendLine("LEFT JOIN TFNMCouponType_L CPTL WITH(NOLOCK) ON CPTL.FTCptCode = CPT.FTCptCode AND CPTL.FNLngID = " + cVB.nVB_Language);
                        oSql.AppendLine("WHERE ISNULL(CPT.FTCptStaUse,'') = '1'");
                        if (string.Equals(cVB.tVB_KbdCallByName, "C_KBDxCashCoupon"))
                        {
                            oSql.AppendLine("AND CPT.FTCptType = '1'");
                        }
                        else
                        {
                            oSql.AppendLine("AND CPT.FTCptType = '2'");
                        }

                        break;

                    case "TCNMBranch": //*Zen 63-03-31
                        olaTitle.Text = oW_Resource.GetString("tTitleBch");

                        oSql.Clear();
                        oSql.AppendLine("SELECT Bch.FTBchCode AS 'tCode', Bch_L.FTBchName AS 'tName'");
                        oSql.AppendLine("FROM TCNMBranch Bch WITH(NOLOCK)");
                        oSql.AppendLine("LEFT JOIN TCNMBranch_L Bch_L WITH(NOLOCK) ON Bch.FTBchCode = Bch_L.FTBchCode AND Bch_L.FNLngID = 1");
                        break;

                    case "TFNMCreditCardBIN":
                        olaTitle.Text = oW_Resource.GetString("tBank");
                        oSql.Clear();
                        oSql.AppendLine("SELECT distinct Crd.FTBnkCode AS 'tCode',Crd_L.FTCrdName AS 'tName',Bank.FTBnkName FROM TFNMCreditCard Crd with(nolock)");
                        oSql.AppendLine("LEFT JOIN TFNMCreditCard_L Crd_L ON Crd.FTCrdCode = Crd_L.FTCrdCode");
                        oSql.AppendLine("LEFT JOIN TFNMCreditCardBIN Crdbin with(nolock) ON Crd.FTCrdCode = Crdbin.FTCrdCode");
                        oSql.AppendLine("LEFT JOIN TFNMBank_L Bank ON Bank.FTBnkCode = Crd.FTBnkCode");
                        oSql.AppendLine("WHERE Bank.FNLngID = '1'");
                        if (string.IsNullOrEmpty(cVB.tVB_CardBin))
                        {

                        }
                        else
                        {
                            oSql.AppendLine("AND LEFT(Crdbin.FTCrdBinCode," + cVB.tVB_CardBin.Length + ") = '" + cVB.tVB_CardBin + "'");
                        }
                        break;
                }

                aoSearch = new cDatabase().C_GETaDataQuery<cmlSearch>(oSql.ToString());

                if (aoSearch.Count > 0)
                {
                    foreach (cmlSearch oSearch in aoSearch)
                    {
                        ogdSearch.Rows.Add(oSearch.tCode, oSearch.tName);
                        oW_TblData.Rows.Add(oSearch.tCode, oSearch.tName);
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "W_GETxDataSearch : " + oEx.Message); }
        }

        /// <summary>
        /// Choose item 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdSearch_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                W_SETxDataSearch();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "ogdSearch_CellMouseDoubleClick : " + oEx.Message); }
        }

        /// <summary>
        /// Set data search
        /// </summary>
        private void W_SETxDataSearch()
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                oW_DataSearch = new cmlSearch();
                oW_DataSearch.tCode = ogdSearch.CurrentRow.Cells[0].Value.ToString();
                oW_DataSearch.tName = ogdSearch.CurrentRow.Cells[1].Value.ToString();
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "W_SETxDataSearch : " + oEx.Message); }
        }

        /// <summary>
        /// Set data search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                W_SETxDataSearch();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //*Net 63-03-02 ไปใช้ Form Keydown แทน
                /*switch (e.KeyCode)
                {
                    case Keys.Enter:
                        e.Handled = true;
                        W_SETxDataSearch();
                        break;

                    case Keys.Up:
                        if (ogdSearch.CurrentRow.Index > 0)
                        {
                            ogdSearch.ClearSelection();
                            ogdSearch.Rows[ogdSearch.CurrentRow.Index - 1].Selected = true;
                        }
                        break;

                    case Keys.Down:
                        if (ogdSearch.Rows.Count - 1 > ogdSearch.CurrentRow.Index)
                        {
                            ogdSearch.ClearSelection();
                            ogdSearch.Rows[ogdSearch.CurrentRow.Index + 1].Selected = true;
                        }
                        break;

                    case Keys.Escape:
                        this.Close();
                        this.Dispose();
                        break;
                }*/
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "ogdSearch_KeyDown : " + oEx.Message); }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_PRCxCallByName("C_KBDxKeyboard");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearch2Column", "ocmShwKb_Click : " + oEx.Message);
            }
        }


        private void wSearch2Column_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {

                    case Keys.Escape:
                        this.Close();
                        this.Dispose();
                        break;
                    case Keys.F10:
                        ocmAccept_Click(ocmAccept, new EventArgs());
                        break;
                    case Keys.F4:
                        ocmSearch_Click(ocmSearch, new EventArgs());
                        break;
                    case Keys.F3:
                        otbSearchPdt.Focus();
                        break;
                    case Keys.F2:
                        ocbSearchBy.DroppedDown = !ocbSearchBy.DroppedDown;
                        if (ocbSearchBy.DroppedDown) ocbSearchBy.Focus();
                        else otbSearchPdt.Focus();
                        ocbSearchBy.DropDownClosed += (oSender, oE) => { otbSearchPdt.Focus(); };
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "wSearch2Column_KeyDown : " + oEx.Message); }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                W_SETxFilterData();
                otbSearchPdt.SelectAll();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "ocmSearch_Click : " + oEx.Message); }
        }

        //*Net 63-03-04
        /// <summary>
        /// Filter Data %LIKE%
        /// </summary>
        private void W_SETxFilterData()
        {

            DataView oDataFilter = new DataView(oW_TblData);
            switch (ocbSearchBy.SelectedIndex)
            {
                case 0:
                    oDataFilter.RowFilter = "tCode like '%" + otbSearchPdt.Text + "%'";
                    break;
                default:
                    oDataFilter.RowFilter = "tName like '%" + otbSearchPdt.Text + "%'";
                    break;
            }

            ogdSearch.Rows.Clear();
            if (oDataFilter != null)
            {
                foreach (DataRow oItem in oDataFilter.ToTable().Rows)
                {
                    ogdSearch.Rows.Add(oItem[0], oItem[1]);
                }
            }
        }

        //*Net 63-03-04
        /// <summary>
        /// Press Key on Textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbSearchPdt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        W_SETxFilterData();
                        otbSearchPdt.SelectAll();
                        break;
                    case Keys.Up:
                        if (ogdSearch.CurrentRow.Index > 0)
                        {
                            ogdSearch.ClearSelection();
                            //ogdSearch.Rows[ogdSearch.CurrentRow.Index - 1].Selected = true; //*Net 63-03-02 กดแล้ว current row index ไม่เปลี่ยน
                            ogdSearch.CurrentCell = ogdSearch.Rows[ogdSearch.CurrentRow.Index - 1].Cells[0]; //*Net 63-03-02 เลือกแถวถัดไป
                        }
                        e.Handled = true;
                        break;

                    case Keys.Down:
                        if (ogdSearch.Rows.Count - 1 > ogdSearch.CurrentRow.Index)
                        {
                            ogdSearch.ClearSelection();
                            //ogdSearch.Rows[ogdSearch.CurrentRow.Index + 1].Selected = true; //*Net 63-03-02 กดแล้ว current row index ไม่เปลี่ยน
                            ogdSearch.CurrentCell = ogdSearch.Rows[ogdSearch.CurrentRow.Index + 1].Cells[0]; //*Net 63-03-02 เลือกแถวถัดไป
                        }
                        e.Handled = true;
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "otbSearchPdt_KeyDown : " + oEx.Message); }
        }
    }
}
