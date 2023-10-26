using AdaPos.Class;
using AdaPos.Control;
using AdaPos.Models.Database;
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
    public partial class wBanknote : Form
    {
        public List<cmlBankNote> aoW_BankNote;
        private cSP oW_SP = new cSP();
        private ResourceManager oW_Resource;

        public wBanknote()
        {
            InitializeComponent();
            try
            {
                aoW_BankNote = new List<cmlBankNote>();

                W_SETxDesign();//*Net 63-04-01 ยกมาจาก baseline
                W_SETxText();
                W_SETxGridDefualt();
                W_GETxBankNote();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBanknote", "wBanknote : " + oEx.Message);
            }
            
        }

        //*Net 63-04-01 ยกมาจาก baseline
        /// <summary>
        /// Set design form
        /// </summary>
        private void W_SETxDesign() //*Net 63-03-02
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                //ogdSearch.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdBankNote); //*Net 63-03-02 Set Style gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSearch2Column", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// GET ข้อมูลมาแสดง
        /// </summary>
        private void W_GETxBankNote()
        {
            StringBuilder oSql;
            try
            {

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ISNULL(BNTL.FTBntName, (SELECT TOP 1 FTBntName FROM TFNMBankNote_L with(nolock) WHERE FTRteCode = BNT.FTRteCode AND FTBntCode = BNT.FTBntCode)) AS FTBntName");
                oSql.AppendLine(",0 AS FCBntQty, 0 AS FCBntAmt, BNT.FTBntCode,BNT.FCBntRateAmt");
                oSql.AppendLine(" FROM TFNMBankNote BNT with(nolock)");
                oSql.AppendLine(" LEFT JOIN TFNMBankNote_L BNTL with(nolock) ON BNT.FTRteCode = BNTL.FTRteCode AND BNT.FTBntCode = BNTL.FTBntCode AND BNTL.FNLngID = 1");
                oSql.AppendLine(" ORDER BY BNT.FTBntCode");

                aoW_BankNote = new cDatabase().C_GETaDataQuery<cmlBankNote>(oSql.ToString());
                ogdBankNote.Rows.Clear();
                if (aoW_BankNote == null)
                {
                    return;
                }

                foreach (var oItem in aoW_BankNote)
                {
                    int nRow = ogdBankNote.Rows.Count;
                    ogdBankNote.Rows.Add();
                    ogdBankNote.Rows[nRow].Cells[0].Value = oItem.FTBntName;
                    ogdBankNote.Rows[nRow].Cells[1].Value = oItem.FCBntQty;
                    ogdBankNote.Rows[nRow].Cells[2].Value = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oItem.FCBntAmt),cVB.nVB_DecShow);
                    ogdBankNote.Rows[nRow].Cells[3].Value = oItem.FCBntRateAmt;
                    ogdBankNote.Rows[nRow].Cells[4].Value = oItem.FCBntQty;

                    uQty oQty = new uQty();
                    ogdBankNote.Rows[nRow].Height = oQty.Height + 1;
                    //Set properties and register event handlers
                    oQty.otbQty.Text = ogdBankNote.Rows[nRow].Cells[1].Value.ToString().Trim();
                    TextBox oTb = new TextBox();
                    oTb.TextChanged += W_OTb_TextChanged;
                    oQty.oU_Value = oTb;
                    oTb.Tag = nRow;
                    //Make it invisible
                    oQty.Visible = false;

                    //Set tag of your wanted cell to control
                    ogdBankNote.Rows[nRow].Cells[1].Tag = oQty;

                    //Add control to Controls collection of grid
                    this.ogdBankNote.Controls.Add(oQty);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBanknote", "W_GET_BankNote : " + oEx.Message);
            }
        }

        private void W_SETxGridDefualt()
        {
            try
            {
                string tFmtAmt = cVB.nVB_DecShow < 1 ? "#,###,##0" : "#,###,##0." + new string('0', cVB.nVB_DecShow);

                ogdBankNote.Columns["otbAmt"].DefaultCellStyle.Format = tFmtAmt;
                ogdBankNote.Columns["otbAmt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wBanknote", "W_SETxGridDefualt : " + oEx.Message); }
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

                opnHD.BackColor = cVB.oVB_ColDark;
                //ogdBankNote.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdBankNote); //*Net 63-03-03 Set Design Gridview

                //*Em 62-06-19
                otbType.HeaderText = oW_Resource.GetString("tType");
                otbQuantity.HeaderText = oW_Resource.GetString("tQty");
                otbAmt.HeaderText = oW_Resource.GetString("tAmt");
                //+++++++++++++++++

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCloseShift", "W_SETxText : " + oEx.Message); }
        }
        /// <summary>
        /// check UC value Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void W_OTb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox oTextbox = (TextBox)sender;
                int nRow = int.Parse(oTextbox.Tag.ToString());
                decimal cRateAmt = ogdBankNote.Rows[nRow].Cells[3].Value == null ? 0 : decimal.Parse(ogdBankNote.Rows[nRow].Cells[3].Value.ToString());
                int nAmtInput = int.Parse(oTextbox.Text.ToString());
                decimal cResult = cRateAmt * nAmtInput;
                ogdBankNote.Rows[nRow].Cells[2].Value = oW_SP.SP_SETtDecShwSve(1, cResult,cVB.nVB_DecShow);
                ogdBankNote.Rows[nRow].Cells[4].Value =  oW_SP.SP_SETtDecShwSve(1, nAmtInput,cVB.nVB_DecShow);
                W_SUMxRateAmt();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBanknote", "W_OTb_TextChanged : " + oEx.Message);
            }
        }

        /// <summary>
        /// สรุปยอด
        /// </summary>
        private void W_SUMxRateAmt()
        {
            decimal cResult = 0;
            try
            {
                foreach (DataGridViewRow oRow in ogdBankNote.Rows)
                {
                    string tAmnt = oRow.Cells[2].Value.ToString();
                    cResult = cResult + decimal.Parse(tAmnt);
                }
                olaResult.Text = new cSP().SP_SETtDecShwSve(1,cResult,cVB.nVB_DecShow);
                olaResult.Update();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBanknote", "W_SUMxRateAmt : " + oEx.Message);
            }
            finally
            {

            }
        }

        /// <summary>
        /// สั่งวาด UC ลงบน Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdBankNote_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                if (e.ColumnIndex == 1)
                {
                    for (int i = 0; i < ogdBankNote.RowCount; i++)
                    {
                        uQty oQty = (uQty)ogdBankNote.Rows[i].Cells[1].Tag;
                        Rectangle oCellRectangle = ogdBankNote.GetCellDisplayRectangle(1, i, true);
                        oQty.Location = new Point(oCellRectangle.X, oCellRectangle.Y);
                        if (oCellRectangle.IsEmpty)
                        {
                            oQty.Visible = false;
                        }
                        else
                        {
                            oQty.Location = new Point(oCellRectangle.X, oCellRectangle.Y);
                            oQty.Visible = true;
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBanknote", "ogdBankNote_CellPainting : " + oEx.Message);
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
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRemark", "OnPaintBackground " + oEx.Message);
            }
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRemark", "ocmBack_Click " + oEx.Message);
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRemark", "ocmAccept_Click " + oEx.Message);
            }
        }

        private void W_INSxTPSTShiftSKeyBN()
        {
            StringBuilder oSql;
            cmlBankNote oBankNote;
            try
            {
                foreach (DataGridViewRow oRow in ogdBankNote.Rows)
                {
                    oBankNote = new cmlBankNote();
                    oBankNote = aoW_BankNote.Where(x => x.FTBntName == oRow.Cells["otbType"].Value.ToString()).FirstOrDefault();
                    //oBankNote.FCBntAmt = oRow.Cells["otbAmtPay"].Value.ToString();
                    //oBankNote.FCBntQty = oRow.Cells["otbQuantityValue"].Value.ToString();

                    oBankNote.FCBntAmt = oW_SP.SP_SETtDecShwSve(2, Convert.ToDecimal(oRow.Cells["otbAmt"].Value), cVB.nVB_DecSave);
                    oBankNote.FCBntQty = oW_SP.SP_SETtDecShwSve(2, Convert.ToDecimal(oRow.Cells["otbQuantityValue"].Value), cVB.nVB_DecSave);

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
                    oSql.AppendLine(",'"+ cVB.tVB_PosCode +"'");
                    oSql.AppendLine(",'"+ cVB.tVB_ShfCode + "'");
                    oSql.AppendLine(","+ cVB.nVB_ShfSeq);
                    oSql.AppendLine(",'" + oBankNote.FTBntCode + "'");
                    oSql.AppendLine("," + oBankNote.FCBntRateAmt);
                    oSql.AppendLine(","+ oBankNote.FCBntQty);
                    oSql.AppendLine("," + oBankNote.FCBntAmt + ")");

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

        private void ogdBankNote_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((uQty)ogdBankNote.Rows[ogdBankNote.SelectedCells[0].RowIndex].Cells[1].Tag).otbQty.Focus();

                ((uQty)ogdBankNote.Rows[ogdBankNote.SelectedCells[0].RowIndex].Cells[1].Tag).otbQty.SelectAll();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wBanknote", "ogdBankNote_SelectionChanged " + oEx.Message);
            }
        }

        private void wBanknote_Shown(object sender, EventArgs e)
        {
            ogdBankNote.Focus();
        }

        private void ogdBankNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.Handled = true;
        }
    }
}
