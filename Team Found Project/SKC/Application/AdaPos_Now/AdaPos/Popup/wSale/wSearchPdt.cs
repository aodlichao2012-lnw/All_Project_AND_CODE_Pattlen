using AdaPos.Class;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Respond.KADS.CheckStock;
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

namespace AdaPos.Popup.wSale
{
    public partial class wSearchPdt : Form
    {
        #region Variable
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_StartRow, nW_SortPdt = 1;
        private string tW_PgpChain;
        private int nW_CurPage = 1;              // Current Page
        private int nW_MaxPage;                  //*Arm 62-10-16 - totalPage
        private int nW_SelectRow;
        private decimal cW_SetQty;               //*Arm 63-05-04 กำหนดค่า Qty เริ่มต้น

        public string rtBarCode { get; set; }
        #endregion

        #region Constrictor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptPdtCode"></param>
        /// <param name="pcSetQty"></param>
        /// <param name="pnShowMode">*Arm 63-06-20  1:ค้นหาสินค้า 2:แสดงสินค้าทดแทน</param>
        /// <param name="paoPdtIntChg">Arm 63-06-20 ข้อมูลสินค้าทดแทน</param>
        public wSearchPdt(string ptPdtCode, decimal pcSetQty = 1, int pnShowMode=1, List<cmlPdtDetail> paoPdtIntChg = null)
        {
            InitializeComponent();
            try
            {
                cW_SetQty = pcSetQty; //*Arm 63-05-04
                W_SETxDesign();
                W_SETxText();

                oW_SP = new cSP();
                //if (string.IsNullOrEmpty(ptPdtCode))
                //{
                //    W_DATxLoadPdt();
                //}
                //else
                //{
                //    W_DATxLoadPdt(1, ptPdtCode);
                //}

                if(pnShowMode == 1)
                {
                    //ค้นหาสินค้า
                    if (string.IsNullOrEmpty(ptPdtCode))
                    {
                        W_DATxLoadPdt();
                    }
                    else
                    {
                        W_DATxLoadPdt(1, ptPdtCode);
                    }
                }
                else
                {
                    //แสดงสินค้าทดแทน
                    W_DATxLoadPdtIntChg(paoPdtIntChg);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "wSearchPdt : " + oEx.Message);
            }
        }
        #endregion

        #region Method
        public void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                ocmSearch.BackColor = cVB.oVB_ColNormal;
                //ogdPdt.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                //ogdPdt.DefaultCellStyle.SelectionBackColor = cVB.oVB_ColNormal;
                //ogdPdt.DefaultCellStyle.SelectionForeColor = Color.White;
                //ogdPdt.AlternatingRowsDefaultCellStyle.BackColor=cVB.oVB_ColLight;
                new cSP().SP_SETxSetGridviewFormat(ogdPdt);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "W_SETxDesign : " + oEx.Message);
            }
        }
        public void W_SETxText()
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
                
                olaTitleSearchPdt.Text = oW_Resource.GetString("tTitleSearchPdt");
                olaTitleSearch.Text = cVB.oVB_GBResource.GetString("tSearch");
                otbTitlePdtCode.HeaderText = oW_Resource.GetString("tTitlePdtCode");
                otbTitlePdtName.HeaderText = oW_Resource.GetString("tTitlePdtName");
                otbTitlePdtNameOth.HeaderText = oW_Resource.GetString("tTitlePdtNameOth");
                otbTitlePunName.HeaderText = oW_Resource.GetString("tTitleUnit");
                otbTitleBarCode.HeaderText = oW_Resource.GetString("tTitleBarCode");
                otbTitlePrice.HeaderText = oW_Resource.GetString("tTitlePrice");
                otbTitleQty.HeaderText = oW_Resource.GetString("tQty");

                //ocbSearchPdtBy.
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tPdtCode"));
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tPdtName"));
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tBarcode"));
                ocbSearchBy.Items.Add(cVB.oVB_GBResource.GetString("tPrice"));
                ocbSearchBy.SelectedIndex = 1; //*Arm 63-03-02 - แก้ไข Default ชื่อสินค้า



            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "W_SETxText : " + oEx.Message);
            }
        }
        public void W_DATxLoadPdt(int pnMode = 0, string ptValue = "", int pnSchBy = 0)
        {
            ocmSchPrevious.Enabled = false;
            ocmSchNextPage.Enabled = false;
            List<cmlPdtDetail> aoPdt;
            int nRowCount = 0;
            double cMaxPage;
            try
            {
                ogdPdt.Rows.Clear();
                aoPdt = new List<cmlPdtDetail>();
                aoPdt = new cPdt().C_GETaPdtSale(ptValue, pnMode, pnSchBy, nW_StartRow, tW_PgpChain, "", nW_SortPdt);


                if (aoPdt.Count > 0)
                {
                    foreach (cmlPdtDetail oPdt in aoPdt)
                    {
                        //ogdPdt.Rows.Add(oPdt.tPdtCode, oPdt.tPdtName, oPdt.tPdtNameOth, oPdt.tUnitName, oPdt.tBarcode, oPdt.cPdtPrice, oPdt.cUnitFactor, oPdt.tSaleType);
                        //*Net 63-04-13
                        ogdPdt.Rows.Add(oPdt.tPdtCode, oPdt.tPdtName, oPdt.tPdtNameOth, 
                            oPdt.tUnitName, oPdt.tBarcode, 
                            oW_SP.SP_SETtDecShwSve(1,oPdt.cPdtPrice,cVB.nVB_DecShow), 
                            oPdt.cUnitFactor, oPdt.tSaleType, cW_SetQty, oPdt.tPgpChain, oPdt.tRemark, //*Arm 63-08-08 เพิ่ม remark
                            oPdt.tStkControl); //*Em 63-08-15

                    }
                    nRowCount = aoPdt[0].nRowCount;
                }


                if (nRowCount <= cVB.nVB_PdtPerPage)
                {
                    nW_MaxPage = 1;
                    ocmSchPrevious.Enabled = false;
                    ocmSchNextPage.Enabled = false;
                }
                else
                {
                    cMaxPage = Math.Ceiling(Convert.ToDouble(nRowCount) / cVB.nVB_PdtPerPage);
                    nW_MaxPage = Convert.ToInt32(cMaxPage);

                    if (nW_CurPage == nW_MaxPage)
                    {
                        ocmSchNextPage.Enabled = false;
                    }
                    else
                    {
                        ocmSchNextPage.Enabled = true;
                    }

                    if (nW_CurPage == 1)
                    {
                        ocmSchPrevious.Enabled = false;
                    }
                    else
                    {
                        ocmSchPrevious.Enabled = true;
                    }

                }
                
                olaPagePdt.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), nRowCount, nW_CurPage, nW_MaxPage);
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "W_DATxLoadPdt : " + oEx.Message);
            }
        }

        /// <summary>
        /// แสดงสินค้าทดแทน
        /// </summary>
        /// <param name="paoPdtIntChg"></param>
        public void W_DATxLoadPdtIntChg(List<cmlPdtDetail> paoPdtIntChg)
        {
            try
            {
                ocbSearchBy.Enabled = false;
                otbSearch.Enabled = false;
                ocmSearch.Enabled = false;

                ogdPdt.Columns[otbTitlePdtNameOth.Name].Visible = false;
                ogdPdt.Columns[otbTitleBarCode.Name].Visible = false;
                ogdPdt.Columns[otbTitleQty.Name].Visible = true;
                ogdPdt.Columns[ocmEditQty.Name].Visible = true;

                ocmEditQty.Text = oW_Resource.GetString("tEditQtyRfn"); 
                ocmEditQty.UseColumnTextForButtonValue = true;

                if (paoPdtIntChg.Count > 0)
                {
                    foreach (cmlPdtDetail oPdt in paoPdtIntChg)
                    {
                        ogdPdt.Rows.Add(oPdt.tPdtCode, oPdt.tPdtName, "",
                            oPdt.tUnitName, oPdt.tBarcode,
                            oW_SP.SP_SETtDecShwSve(1, oPdt.cPdtPrice, cVB.nVB_DecShow),
                            oPdt.cUnitFactor, oPdt.tSaleType, oPdt.cQty, oPdt.tPgpChain, oPdt.tRemark, //*Arm 63-08-08 เพิ่ม remark
                            oPdt.tStkControl); //*Em 63-08-15
                    }
                }

                olaPagePdt.Text = string.Format(cVB.oVB_GBResource.GetString("tPage"), paoPdtIntChg.Count, 1, 1);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "W_DATxLoadPdtIntChg : " + oEx.Message);
            }
            finally
            {
                paoPdtIntChg = null;
                //new cSP().SP_CLExMemory();
            }
        }
        #endregion

        #region Event
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "ocmBack_Click : " + oEx.Message);
            }
        }

        private void ocmSchPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                nW_StartRow = ((nW_CurPage - 1) * cVB.nVB_PdtPerPage) - cVB.nVB_PdtPerPage;
                nW_CurPage--;
                //W_DATxLoadPdt(2);
                //*Net 63-04-04
                if (string.IsNullOrEmpty(otbSearch.Text))
                {
                    W_DATxLoadPdt(2);
                }
                else
                {
                    W_DATxLoadPdt(2, otbSearch.Text, ocbSearchBy.SelectedIndex);
                }
                //+++++++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "ocmSchPrevious_Click : " + oEx.Message);
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {

            try
            {
                if (nW_SelectRow >= 0)
                {
                    cVB.oVB_PdtOrder = new cmlPdtOrder();
                    cVB.oVB_PdtOrder.tPdtCode = (string)ogdPdt.Rows[nW_SelectRow].Cells[otbTitlePdtCode.Name].Value;
                    cVB.oVB_PdtOrder.tBarcode = (string)ogdPdt.Rows[nW_SelectRow].Cells[otbTitleBarCode.Name].Value;
                    cVB.oVB_PdtOrder.tPdtName = (string)ogdPdt.Rows[nW_SelectRow].Cells[otbTitlePdtName.Name].Value;
                    cVB.oVB_PdtOrder.cSetPrice = Convert.ToDecimal(ogdPdt.Rows[nW_SelectRow].Cells[otbTitlePrice.Name].Value);
                    cVB.oVB_PdtOrder.cFactor = Convert.ToDecimal(ogdPdt.Rows[nW_SelectRow].Cells[otbTitleFactor.Name].Value);
                    cVB.oVB_PdtOrder.tSaleType = (string)ogdPdt.Rows[nW_SelectRow].Cells[otbTitleSaleType.Name].Value;
                    cVB.oVB_PdtOrder.tUnit = (string)ogdPdt.Rows[nW_SelectRow].Cells[otbTitlePunName.Name].Value;
                    cVB.oVB_PdtOrder.cQty = cW_SetQty; //*Arm 63-05-04 
                    cVB.oVB_PdtOrder.tStaPdt = "1";
                    cVB.oVB_PdtOrder.tPgpChain = (string)ogdPdt.Rows[nW_SelectRow].Cells[otbTitlePgpChain.Name].Value; //*Arm 63-06-17
                    cVB.oVB_PdtOrder.tRemark = (string)ogdPdt.Rows[nW_SelectRow].Cells[otbTitleRemark.Name].Value;  //*Arm 63-08-08 Location Product (SKC)
                    cVB.oVB_PdtOrder.tStkControl = (string)ogdPdt.Rows[nW_SelectRow].Cells[FTPdtStkControl.Name].Value;  //*Em 63-08-15
                    this.DialogResult = DialogResult.OK;
                    this.Close();

                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "ocmAccept_Click : " + oEx.Message);
            }
        }

        private void ogdPdt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;
                if (e.RowIndex < 0) return;
                nW_SelectRow = e.RowIndex;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "ogdPdt_CellClick : " + oEx.Message);
            }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                nW_StartRow = 0;
                nW_MaxPage = 0;
                nW_CurPage = 1;

                if (string.IsNullOrEmpty(otbSearch.Text))
                {
                    W_DATxLoadPdt();
                }
                else
                {
                    W_DATxLoadPdt(2, otbSearch.Text, ocbSearchBy.SelectedIndex);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "ocmSearch_Click : " + oEx.Message);
            }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_PRCxCallByName("C_KBDxKeyboard");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "ocmShwKb_Click : " + oEx.Message);
            }
        }

        //*Net 63-03-02
        /// <summary>
        /// Search Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        nW_StartRow = 0;
                        nW_MaxPage = 0;
                        nW_CurPage = 1;

                        if (string.IsNullOrEmpty(otbSearch.Text))
                        {
                            W_DATxLoadPdt();
                        }
                        else
                        {
                            W_DATxLoadPdt(2, otbSearch.Text, ocbSearchBy.SelectedIndex);
                        }
                        otbSearch.SelectAll();
                        break;
                    case Keys.Up:
                        if (ogdPdt.CurrentRow.Index > 0)
                        {
                            ogdPdt.ClearSelection();
                            //ogdSearch.Rows[ogdSearch.CurrentRow.Index - 1].Selected = true; //*Net 63-03-02 กดแล้ว current row index ไม่เปลี่ยน
                            ogdPdt.CurrentCell = ogdPdt.Rows[ogdPdt.CurrentRow.Index - 1].Cells[0]; //*Net 63-03-02 เลือกแถวถัดไป
                        }
                        e.Handled = true;
                        break;

                    case Keys.Down:
                        if (ogdPdt.Rows.Count - 1 > ogdPdt.CurrentRow.Index)
                        {
                            ogdPdt.ClearSelection();
                            //ogdSearch.Rows[ogdSearch.CurrentRow.Index + 1].Selected = true; //*Net 63-03-02 กดแล้ว current row index ไม่เปลี่ยน
                            ogdPdt.CurrentCell = ogdPdt.Rows[ogdPdt.CurrentRow.Index + 1].Cells[0]; //*Net 63-03-02 เลือกแถวถัดไป
                        }
                        e.Handled = true;
                        break;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "otbSearch_KeyDown : " + oEx.Message);
            }
        }

        //*Net เริ่มต้นให้ Focus ที่ textbox
        /// <summary>
        /// Form Show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSearchPdt_Shown(object sender, EventArgs e)
        {
            otbSearch.Focus();
        }

        //*Net เลื่อนขึ้นลง gridview ได้ แม้ว่าจะ Focus ที่ textbox อยู่
        /// <summary>
        /// Form Key Preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wSearchPdt_KeyDown(object sender, KeyEventArgs e)
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
                        otbSearch.Focus();
                        break;
                    case Keys.F2:
                        ocbSearchBy.DroppedDown = !ocbSearchBy.DroppedDown;
                        if (ocbSearchBy.DroppedDown) ocbSearchBy.Focus();
                        else otbSearch.Focus();
                        ocbSearchBy.DropDownClosed += (oSender, oE) => { otbSearch.Focus(); };
                        break;
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearchPdt", "wSearchPdt_KeyDown : " + oEx.Message); }
        }

        private void ogdPdt_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ocmAccept_Click(ocmAccept, new EventArgs());
        }

        private void ogdPdt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                wEnterAmount oFrm = new wEnterAmount(3);
                oFrm.ShowDialog();
                if (!string.IsNullOrEmpty(oFrm.otbAmount.Text))
                {

                    decimal cSetQty = Convert.ToDecimal(oFrm.otbAmount.Text);
                    if (cSetQty > 0)
                    {
                        if (cSetQty > (decimal)ogdPdt.CurrentRow.Cells[otbTitleQty.Name].Value)
                        {
                            new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgPdtQtyDef"), 3);
                            return;
                        }
                        else
                        {
                            cW_SetQty = cSetQty;
                            ocmAccept_Click(ocmAccept, null);
                        }
                    }
                }
            }
        }

        private void ocmSchNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                nW_StartRow = (cVB.nVB_PdtPerPage * nW_CurPage);
                nW_CurPage++;
                //W_DATxLoadPdt(2);
                //*Net 63-04-04
                if (string.IsNullOrEmpty(otbSearch.Text))
                {
                    W_DATxLoadPdt(2);
                }
                else
                {
                    W_DATxLoadPdt(2, otbSearch.Text, ocbSearchBy.SelectedIndex);
                }
                //+++++++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wSearchPdt", "ocmSchNextPage_Click : " + oEx.Message);
            }
        }

        #endregion
    }
}
