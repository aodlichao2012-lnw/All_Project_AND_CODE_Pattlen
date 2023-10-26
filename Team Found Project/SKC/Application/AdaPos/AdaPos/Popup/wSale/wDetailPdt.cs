using AdaPos.Class;
using AdaPos.Resources_String.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wSale
{
    public partial class wDetailPdt : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private string tW_PdtCode;
        #endregion End Variable

        #region Constructor

        public wDetailPdt(string ptPdtCode)
        {
            InitializeComponent();

            try
            {
                oW_SP = new cSP();
                tW_PdtCode = ptPdtCode; //*Em 62-08-28

                W_SETxDesign();
                W_SETxText();
                W_DATxLoadPdt();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "wDetailPdt " + oEx.Message); }
        }

        #endregion End Constructor

        #region Event

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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "wDetailPdt " + oEx.Message); }
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "OnPaintBackground " + oEx.Message); }
        }

        #endregion End Event

        /// <summary>
        /// Accept
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                W_PRCxAccept();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "ocmAccept_Click " + oEx.Message); }
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

                cVB.tVB_KbdScreen = "DETAILPDT";
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "ocmShwKb_Click : " + oEx.Message); }
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
                        W_PRCxAccept();
                        break;

                    case "C_KBDxBack":
                        this.Close();
                        break;

                    case "C_KBDxFocusPdt":
                        ogdPdtBarcode.Focus();
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "W_GETxFuncByFuncName : " + oEx.Message); }
            finally
            {
                ptFuncName = null;
                oW_SP.SP_CLExMemory();
            }
        }

        #region Function
        /// <summary>
        /// Process Accept
        /// </summary>
        private void W_PRCxAccept()
        {
            try
            {

                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "W_PRCxAccept : " + oEx.Message); }
        }

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
                //ogdPdtBarcode.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdPdtBarcode); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "W_SETxDesign : " + oEx.Message); }
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

                cVB.tVB_KbdScreen = "DETAILPDT";

                olaTitlePdtDesc.Text = oW_Resource.GetString("tPdtDesc");
                olaTitlePdtCode.Text = cVB.oVB_GBResource.GetString("tPdtCode");
                olaTitlePdtName.Text = cVB.oVB_GBResource.GetString("tPdtName");
                ocmTitleFavorite.HeaderText = cVB.oVB_GBResource.GetString("tFavorite");
                otbTitleBarcode.HeaderText = cVB.oVB_GBResource.GetString("tBarcode");
                otbTitleFactor.HeaderText = cVB.oVB_GBResource.GetString("tFactor");
                otbTitlePrice.HeaderText = cVB.oVB_GBResource.GetString("tPrice");
                otbTitleUnit.HeaderText = cVB.oVB_GBResource.GetString("tUnit");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "W_SETxText : " + oEx.Message); }
        }

        private void W_DATxLoadPdt()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            string tPdtImg = "";
            try
            {
                oSql.AppendLine("SELECT PDT.FTPdtCode,PDTL.FTPdtName,BAR.FTBarCode, ISNULL(UNTL.FTPunName,'') AS FTPunName, PKS.FCPdtUnitFact, IMG.FTImgObj,");
                oSql.AppendLine("PRI.FCPgdPriceRet, PRI.FCPgdPriceWhs, PRI.FCPgdPriceNet");
                oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMPdt_L PDTL WITH(NOLOCK) ON PDTL.FTPdtCode = PDT.FTPdtCode AND PDTL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TCNMPdtBar BAR WITH(NOLOCK) ON BAR.FTPdtCode = PDT.FTPdtCode AND BAR.FTBarStaUse = '1'");
                oSql.AppendLine("INNER JOIN TCNMPdtPackSize PKS WITH(NOLOCK) ON PKS.FTPdtCode = PDT.FTPdtCode ");
                oSql.AppendLine("INNER JOIN TCNMPdtUnit UNT WITH(NOLOCK) ON UNT.FTPunCode = PKS.FTPunCode AND UNT.FTPunCode = BAR.FTPunCode");
                oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L UNTL WITH(NOLOCK) ON UNTL.FTPunCode = BAR.FTPunCode  AND UNTL.FNLngID = 1");
                oSql.AppendLine("LEFT JOIN TCNMImgPdt IMG WITH(NOLOCK) ON IMG.FTImgRefID = PDT.FTPdtCode AND FTImgTable = 'TCNMPdt' AND FTImgKey = 'master'");
                oSql.AppendLine("INNER JOIN TCNTPdtPrice4PDT PRI WITH(NOLOCK) ON PRI.FTPdtCode = PDT.FTPdtCode AND PRI.FTPunCode = PKS.FTPunCode AND PRI.FTPghStaAdj = '1'");
                oSql.AppendLine("		AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(11),PRI.FDPghDStart+PRI.FTPghTStart,121) AND CONVERT(VARCHAR(19),PRI.FDPghDStop+PRI.FTPghTStop,121))");
                oSql.AppendLine("INNER JOIN (SELECT FTPdtCode,FTPunCode,MAX(FDLastUpdOn) AS FDLastUpdOn");
                oSql.AppendLine("			FROM TCNTPdtPrice4PDT WITH(NOLOCK)");
                oSql.AppendLine("			WHERE FTPghStaAdj = '1'");
                oSql.AppendLine("			AND (CONVERT(VARCHAR(19),GETDATE(),121) BETWEEN CONVERT(VARCHAR(11),FDPghDStart+FTPghTStart,121) AND CONVERT(VARCHAR(19),FDPghDStop+FTPghTStop,121))");
                oSql.AppendLine("			GROUP BY FTPdtCode,FTPunCode");
                oSql.AppendLine("		) PRI2 ON PRI2.FTPdtCode = PRI.FTPdtCode AND PRI2.FTPunCode = PRI.FTPunCode AND PRI2.FDLastUpdOn = PRI.FDLastUpdOn");
                oSql.AppendLine("WHERE PDT.FTPdtCode = '"+ tW_PdtCode +"'");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                ogdPdtBarcode.Rows.Clear();
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        olaPdtCode.Text = tW_PdtCode;
                        olaPdtName.Text = odtTmp.Rows[0].Field<string>("FTPdtName");
                        tPdtImg = odtTmp.Rows[0].Field<string>("FTImgObj");
                        if (!string.IsNullOrEmpty(tPdtImg))
                        {
                            if (File.Exists(tPdtImg))
                            {
                                opbPdt.Image = Image.FromFile(tPdtImg);
                            }
                        }

                        foreach (DataRow oRow in odtTmp.Rows)
                        {
                            ogdPdtBarcode.Rows.Add(oW_Resource.GetString("tAdd"),
                                oRow.Field<string>("FTBarCode"),
                                oRow.Field<string>("FTPunName"),
                                oRow.Field<decimal>("FCPdtUnitFact"),
                                oRow.Field<decimal>("FCPgdPriceRet"));
                        }
                    }
                    
                }
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDetailPdt", "W_DATxLoadPdt : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                oW_SP.SP_CLExMemory();
            }
        }
        #endregion End Function
    }
}
