using AdaPos.Class;
using AdaPos.Models.DatabaseTmp;
using AdaPos.Models.RabbitMQ;
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

namespace AdaPos.Popup.wPayment
{
    public partial class wChooseCpn : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        public cmlTPSTCouponHD_Tmp oW_Conpon;
        private List<cmlTPSTCouponHD_Tmp> aoW_Conpon;
        private DataTable oTblListCpn;
        private int nW_Mode;
        private int nDistype;
        public bool bW_Select;
        public wChooseCpn(List<cmlTPSTCouponHD_Tmp> paoCoupon,int pnType,int pnDisType=1)
        {
            InitializeComponent();
            try
            {
                nW_Mode = pnType;
                aoW_Conpon = paoCoupon;
                nDistype = pnDisType;
                otbBarCpn.Text = paoCoupon.FirstOrDefault().FTCpdBarCpn;
                oW_SP = new cSP();
                ogdListCpn.AutoGenerateColumns = false;
                W_SETxDesign();
                W_SETxText();
                W_DATxLoadData();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "wListCoupon : " + oEx.Message); }
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
                //ogdForm.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
                oW_SP.SP_SETxSetGridviewFormat(ogdListCpn); //*Net 63-03-03 Set Design Gridview
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "W_SETxDesign : " + oEx.Message); }
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

                olaTitleCoupon.Text = oW_Resource.GetString("tTitleChooseCpn");
                olaTitleNumber.Text = oW_Resource.GetString("tCouponNo");

                FTCpnName.HeaderText = oW_Resource.GetString("tTitleCpnName");
                FTCpnMsg1.HeaderText = oW_Resource.GetString("tTitleCpnMsg1");
                FTCpnMsg2.HeaderText = oW_Resource.GetString("tTitleCpnMsg2");
                FDCphDateStart.HeaderText = oW_Resource.GetString("tTitleDateFrm");
                FDCphDateStop.HeaderText = oW_Resource.GetString("tTitleDateTo");
                FTCpnCond.HeaderText = oW_Resource.GetString("tTitleCond");
                FCCpnValue.HeaderText = oW_Resource.GetString("tValue");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "W_SETxText : " + oEx.Message); }
        }
        private void W_DATxLoadData()
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                oSql.AppendLine($"SELECT (CASE WHEN ISNULL(HD_L.FTCpnName,'')='' THEN CPT_L.FTCptName ELSE FTCpnName END) AS FTCpnName,");
                oSql.AppendLine($"HD_L.FTCpnMsg1,HD_L.FTCpnMsg2,CONVERT(VARCHAR(MAX),HD.FDCphDateStart,103) AS FDCphDateStart,CONVERT(VARCHAR(MAX),HD.FDCphDateStop,103) AS FDCphDateStop,HD_L.FTCpnCond,HD.FCCphDisValue");
                oSql.AppendLine($"FROM TPSTCouponHDTmp HDT WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN TFNTCouponHD HD WITH(NOLOCK) ON HD.FTBchCode=HDT.FTBchCode AND HD.FTCphDocNo=HDT.FTCphDocNo");
                oSql.AppendLine($"INNER JOIN TFNTCouponHD_L HD_L WITH(NOLOCK) ON HD_L.FTCphDocNo=HDT.FTCphDocNo AND HD_L.FNLngID={cVB.nVB_Language}");
                oSql.AppendLine($"INNER JOIN TFNMCouponType CPT WITH(NOLOCK) ON CPT.FTCptCode=HDT.FTCptCode");
                oSql.AppendLine($"INNER JOIN TFNMCouponType_L CPT_L WITH(NOLOCK) ON CPT_L.FTCptCode=HDT.FTCptCode AND CPT_L.FNLngID={cVB.nVB_Language}");
                oSql.AppendLine($"WHERE CPT.FTCptType='{nW_Mode}' AND ISNULL(HD.FCCphMinValue,0)<={cVB.cVB_Amount} ");
                oSql.AppendLine($"AND (HDT.FTCpdBarCpn = '{aoW_Conpon[0].FTCpdBarCpn}')");
                if(nDistype==3)
                {
                    oSql.AppendLine($"AND HD.FTCphDisType='3'");
                }
                else
                {
                    oSql.AppendLine($"AND HD.FTCphDisType<>'3'");
                }
                oTblListCpn = oDB.C_GEToDataQuery(oSql.ToString());
                ogdListCpn.DataSource = oTblListCpn;

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "W_DATxLoadData : " + oEx.Message); }
        }
        #endregion End Function

        #region Method/Events
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
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "OnPaintBackground " + oEx.Message); }
        }
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                bW_Select = false;
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "ocmBack_Click : " + oEx.Message); }
        }
        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            new cFunctionKeyboard().C_KBDxKeyboard();
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                bW_Select = true;
                oW_Conpon = aoW_Conpon[ogdListCpn.SelectedCells[0].RowIndex];
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "ocmAccept_Click : " + oEx.Message); }
        }

        private void ogdListCpn_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ocmAccept_Click(ocmAccept, new EventArgs());
        }

        private void wListCoupon_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "wListCoupon_FormClosing : " + oEx.Message); }
        }

        #endregion End Method/Events

    }
}
