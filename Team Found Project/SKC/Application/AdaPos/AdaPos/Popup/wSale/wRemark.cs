using AdaPos.Class;
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
    public partial class wRemark : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        public int nW_RmkType;      //1:หมายเหตุบิล, 2:หมายเหตุสินค้า
        #endregion End Variable

        public wRemark()
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();

                W_SETxDesign();
                W_SETxText();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRemark", "wRemark " + oEx.Message); }
        }

        #region Function
        /// <summary>
        /// Set design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRemark", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set text
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

                switch (nW_RmkType)
                {
                    case 1:
                        olaTitleRemark.Text = oW_Resource.GetString("tTitleRemarkBill");
                        break;
                    case 2:
                        olaTitleRemark.Text = oW_Resource.GetString("tTitleRemrkPdt");
                        break;
                }
                
                olaRemark.Text = cVB.oVB_GBResource.GetString("tRemark");
                otbRmk.Text = "";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRemark", "W_SETxText : " + oEx.Message); }
        }

        private void W_GETxDataOld()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                switch (nW_RmkType)
                {
                    case 1:
                        oSql.AppendLine("SELECT FTXshRmk FROM "+ cSale.tC_TblSalHD +" WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"' AND FTXshDocNo = '"+cVB.tVB_DocNo +"'");
                        break;
                    case 2:
                        oSql.AppendLine("SELECT FTXsdRmk FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                        oSql.AppendLine("AND FNXsdSeqNo = " + cSale.nC_DTSeqNo);
                        break;
                }
                otbRmk.Text = oDB.C_GEToDataQuery<string>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRemark", "W_GETxDataOld : " + oEx.Message); }
        }
        #endregion Function

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wRemark", "OnPaintBackground " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wRemark", "ocmBack_Click : " + oEx.Message); }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                switch (nW_RmkType)
                {
                    case 1:
                        oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK) ");
                        oSql.AppendLine("SET FTXshRmk = '"+ otbRmk.Text.Trim() +"'");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                        break;
                    case 2:
                        oSql.AppendLine("UPDATE " + cSale.tC_TblSalDT + " WITH(ROWLOCK) ");
                        oSql.AppendLine("SET FTXsdRmk = '" + otbRmk.Text.Trim() + "'");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                        oSql.AppendLine("AND FNXsdSeqNo = " + cSale.nC_DTSeqNo);
                        break;
                }
                oDB.C_SETxDataQuery(oSql.ToString());

                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRemark", "ocmAccept_Click : " + oEx.Message); }
        }

        private void wRemark_Load(object sender, EventArgs e)
        {
            W_GETxDataOld();
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wRemark", "ocmShwKb_Click : " + oEx.Message); }
            finally
            {
                otbRmk.Focus();
            }
        }
    }
}
