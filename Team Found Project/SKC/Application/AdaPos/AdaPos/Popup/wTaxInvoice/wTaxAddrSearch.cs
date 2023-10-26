using AdaPos.Class;
using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wTaxInvoice
{
    public partial class wTaxAddrSearch : Form
    {
        public wTaxAddrSearch()
        {
            InitializeComponent();
            try
            {
                W_SETxDesign();
                W_SETxText();
                W_DATxLoadData();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "wTaxAddrSearch : " + oEx.Message); }
        }

        #region Function
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
                //ogdSearch.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;

                new cSP().SP_SETxSetGridviewFormat(ogdSearch); //*Net 63-03-02 Set Style
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                olaTitleSearchTxt.Text = cVB.oVB_GBResource.GetString("tFilter");
                olaTitleSearch.Text = cVB.oVB_GBResource.GetString("tSearch");
                
                otbTitleCardID.HeaderText = cVB.oVB_GBResource.GetString("tTaxID");
                otbTitleTaxName.HeaderText = cVB.oVB_GBResource.GetString("tName");
                otbTitleTel.HeaderText = cVB.oVB_GBResource.GetString("tTel"); //*Em 63-05-05
                otbTitleFax.HeaderText = cVB.oVB_GBResource.GetString("tFax"); //*Em 63-05-05
                otbTitleAddr1.HeaderText = cVB.oVB_GBResource.GetString("tAddr1");
                //otbTitleAddr2.HeaderText = cVB.oVB_GBResource.GetString("tAddr2");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "W_SETxText : " + oEx.Message); }
        }

        private void W_DATxLoadData()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTCNMTaxAddress> aoTaxAddr = new List<cmlTCNMTaxAddress>();
            try
            {
                oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTAddTaxNo,FTAddName,FTAddV2Desc1,FTAddV2Desc1,");
                oSql.AppendLine("FTAddTel,FTAddFax");   //*Em 63-05-05
                oSql.AppendLine("FROM TCNMTaxAddress_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FNLngID = " + cVB.nVB_Language );
                if (!string.IsNullOrEmpty(otbSearch.Text))
                {
                    oSql.AppendLine("AND (FTAddTaxNo LIKE '%" + otbSearch.Text.Trim() + "%'");
                    oSql.AppendLine("OR FTAddName LIKE '%" + otbSearch.Text.Trim() + "%'");
                    oSql.AppendLine("OR FTAddTel LIKE '%" + otbSearch.Text.Trim() + "%'");  //*Em 63-05-05
                    oSql.AppendLine("OR FTAddFax LIKE '%" + otbSearch.Text.Trim() + "%'");  //*Em 63-05-05
                    //oSql.AppendLine("OR FTAddV2Desc1 LIKE '%" + otbSearch.Text.Trim() + "%'");
                    oSql.AppendLine("OR FTAddV2Desc1 LIKE '%" + otbSearch.Text.Trim() + "%')");
                }

                aoTaxAddr = oDB.C_GETaDataQuery<cmlTCNMTaxAddress>(oSql.ToString());
                ogdSearch.Rows.Clear();
                foreach (cmlTCNMTaxAddress oAddr in aoTaxAddr)
                {
                    //ogdSearch.Rows.Add(oAddr.FTAddTaxNo, oAddr.FTAddName, oAddr.FTAddV2Desc1, oAddr.FTAddV2Desc2);
                    ogdSearch.Rows.Add(oAddr.FTAddTaxNo, oAddr.FTAddName, oAddr.FTAddTel,oAddr.FTAddFax, oAddr.FTAddV2Desc1);   //*Em 63-05-05
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "W_DATxLoadData : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                aoTaxAddr = null;
                new cSP().SP_CLExMemory();
            }
        }
        #endregion End Function

        #region Method/Events
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "ocmBack_Click : " + oEx.Message); }
        }
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                cVB.oVB_TaxAddr = null;
                //oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " FTAddTaxNo,FTAddName,FTAddV2Desc1,FTAddV2Desc1");
                oSql.AppendLine("SELECT TOP " + cVB.nVB_MaxData + " *");    //*Em 63-05-06
                oSql.AppendLine("FROM TCNMTaxAddress_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("AND FTAddTaxNo = '" + ogdSearch.Rows[ogdSearch.CurrentRow.Index].Cells["otbTitleCardID"].Value.ToString() + "'");
                cVB.oVB_TaxAddr = oDB.C_GEToDataQuery<cmlTCNMTaxAddress>(oSql.ToString());
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "ocmAccept_Click : " + oEx.Message); }
        }

        private void wTaxAddrSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "wTaxAddrSearch_FormClosing : " + oEx.Message); }
        }
        private void wTaxAddrSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        ocmBack_Click(null,null);
                        break;
                    case Keys.F9:
                        ocmAccept_Click(null, null);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "wTaxAddrSearch_KeyDown : " + oEx.Message); }
        }
        private void otbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    ocmSearch_Click(sender, e);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "otbSearch_KeyDown : " + oEx.Message); }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            try
            {
                W_DATxLoadData();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wTaxAddrSearch", "ocmSearch_Click : " + oEx.Message); }
        }

        #endregion End Method/Events

        private void ogdSearch_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ocmAccept_Click(ocmAccept,new EventArgs());
        }
    }
}
