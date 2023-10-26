using AdaPos.Class;
using AdaPos.Models.Database;
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
    public partial class wTaxInvoiceSearch : Form
    {
        private string tW_DocNo;
        private int nW_Mode;        //1:Sale   2:Tax
        private cSP oW_SP;
        public wTaxInvoiceSearch(int pnMode)
        {
            InitializeComponent();

            try
            {
                nW_Mode = pnMode;
                oW_SP = new cSP();
                W_SETxDesign();
                W_SETxText();
                switch (nW_Mode)
                {
                    case 1: //Sale
                        W_DATxLoadData();
                        break;
                    case 2: //Tax
                        W_DATxLoadDataTax();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "wTaxInvoiceSearch : " + oEx.Message); }
        }

        /// <summary>
        /// Close Form
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "ocmBack_Click : " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "OnPaintBackground : " + oEx.Message); }
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

                ocmSearch.BackColor = cVB.oVB_ColNormal;
                //ogdSelectABB.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdSelectABB); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                olaTitleSearchDoc.Text = cVB.oVB_GBResource.GetString("tSchDoc");
                olaTitleSearch.Text = cVB.oVB_GBResource.GetString("tSearch");
                otbTitleAmt.HeaderText = cVB.oVB_GBResource.GetString("tAmount");
                otbTitleDatetime.HeaderText = cVB.oVB_GBResource.GetString("tDatetime");
                otbTitleDocNo.HeaderText = cVB.oVB_GBResource.GetString("tDocNo");
                otbTitlePos.HeaderText = cVB.oVB_GBResource.GetString("tPos");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "W_SETxText : " + oEx.Message); }
        }

        private void W_DATxLoadData()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalHD> aHD = new List<cmlTPSTSalHD>();
            try
            {
                oSql.AppendLine("SELECT TOP "+ cVB.nVB_MaxData + " FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand");
                oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND ISNULL(FTXshDocVatFull,'') = ''");
                if (!string.IsNullOrEmpty(otbTicketNo.Text))
                {
                    oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                }
                //*Em 62-10-08
                if (string.IsNullOrEmpty(cVB.tVB_Merchart))
                {
                    oSql.AppendLine("AND ISNULL(FTShpCode,'') = ''");
                }
                else
                {
                    oSql.AppendLine("AND ISNULL(FTShpCode,'') IN (SELECT FTShpCode FROM TCNMShop WITH(NOLOCK) WHERE FTMerCode = '"+ cVB.tVB_Merchart +"')");
                }
                //++++++++++++++++

                oSql.AppendLine("ORDER BY FDCreateOn DESC");
                aHD = oDB.C_GETaDataQuery<cmlTPSTSalHD>(oSql.ToString());
                ogdSelectABB.Rows.Clear();
                foreach (cmlTPSTSalHD oHD in aHD)
                {
                    ogdSelectABB.Rows.Add(oHD.FTXshDocNo, oHD.FDXshDocDate, oHD.FTPosCode, oHD.FCXshGrand);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "W_DATxLoadData : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                aHD = null;
                new cSP().SP_CLExMemory();
            }
        }

        private void W_DATxLoadDataTax()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalHD> aHD = new List<cmlTPSTSalHD>();
            try
            {
                oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTXshDocNo,FDXshDocDate,FTPosCode,FCXshGrand");
                oSql.AppendLine("FROM TPSTTaxHD WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                if (!string.IsNullOrEmpty(otbTicketNo.Text))
                {
                    oSql.AppendLine("AND FTXshDocNo LIKE '%" + otbTicketNo.Text.Trim() + "%'");
                }
                //*Em 62-10-08
                if (string.IsNullOrEmpty(cVB.tVB_Merchart))
                {
                    oSql.AppendLine("AND ISNULL(FTShpCode,'') = ''");
                }
                else
                {
                    oSql.AppendLine("AND ISNULL(FTShpCode,'') IN (SELECT FTShpCode FROM TCNMShop WITH(NOLOCK) WHERE FTMerCode = '" + cVB.tVB_Merchart + "')");
                }
                //++++++++++++++++

                oSql.AppendLine("ORDER BY FDCreateOn DESC");
                aHD = oDB.C_GETaDataQuery<cmlTPSTSalHD>(oSql.ToString());
                ogdSelectABB.Rows.Clear();
                foreach (cmlTPSTSalHD oHD in aHD)
                {
                    ogdSelectABB.Rows.Add(oHD.FTXshDocNo, oHD.FDXshDocDate, oHD.FTPosCode, oHD.FCXshGrand);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "W_DATxLoadDataTax : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                aHD = null;
                new cSP().SP_CLExMemory();
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tW_DocNo))
                {
                    tW_DocNo = ogdSelectABB.Rows[ogdSelectABB.CurrentRow.Index].Cells["otbTitleDocNo"].Value.ToString();
                }
                cVB.tVB_DocNo = tW_DocNo;
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "ocmAccept_Click : " + oEx.Message); }
        }

        /// <summary>
        /// Closing form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wTaxInvoiceSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "wTaxInvoiceSearch_FormClosing : " + oEx.Message); }
        }

        private void ogdSelectABB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;
                if (e.RowIndex < 0) return;
                tW_DocNo = ogdSelectABB.Rows[e.RowIndex].Cells["otbTitleDocNo"].Value.ToString();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "ogdSelectABB_CellClick : " + oEx.Message); }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                switch (nW_Mode)
                {
                    case 1: //Sale
                        W_DATxLoadData();
                        break;
                    case 2: //Tax
                        W_DATxLoadDataTax();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "ocmSearch_Click : " + oEx.Message); }
        }

        private void ogdSelectABB_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ocmAccept_Click(ocmAccept,new EventArgs());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "ogdSelectABB_CellDoubleClick : " + oEx.Message); }
        }

        private void otbTicketNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter: ocmAccept_Click(ocmAccept, new EventArgs());
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "otbTicketNo_KeyDown : " + oEx.Message); }
        }

        private void wTaxInvoiceSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        ocmBack_Click(null, null);
                        break;
                    case Keys.F9:
                        ocmAccept_Click(null, null);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxInvoiceSearch", "wTaxInvoiceSearch_KeyDown : " + oEx.Message); }
        }
    }
}
