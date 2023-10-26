using AdaPos.Class;
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
    public partial class wListCoupon : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        public cmlCoupon oW_Conpon;
        private List<cmlCoupon> aoW_Conpon;
        public bool bW_Select;
        public wListCoupon(List<cmlCoupon> paoCoupon,string ptCouponNo)
        {
            InitializeComponent();
            try
            {
                aoW_Conpon = paoCoupon;
                otbCouponNo.Text = ptCouponNo;
                oW_SP = new cSP();
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
                oW_SP.SP_SETxSetGridviewFormat(ogdForm); //*Net 63-03-03 Set Design Gridview
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
                FDCpnDateFrm.HeaderText = oW_Resource.GetString("tTitleDateFrm");
                FDCpnDateTo.HeaderText = oW_Resource.GetString("tTitleDateTo");
                FTCpnCond.HeaderText = oW_Resource.GetString("tTitleCond");
                FCCpnValue.HeaderText = oW_Resource.GetString("tValue");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "W_SETxText : " + oEx.Message); }
        }
        private void W_DATxLoadData()
        {
            try
            {
                ogdForm.Rows.Clear();
                foreach (cmlCoupon oCoupon in aoW_Conpon)
                {
                    ogdForm.Rows.Add(oCoupon.rtCpnName,
                        oCoupon.rtCpnMsg1,
                        oCoupon.rtCpnMsg2,
                        string.Format("{0:dd/MM/yyyy}",oCoupon.rdCphDateStart),
                        string.Format("{0:dd/MM/yyyy}", oCoupon.rdCphDateStop),
                        oCoupon.rtCpnCond,
                        oW_SP.SP_SETtDecShwSve(1, oCoupon.rcCphDisValue, cVB.nVB_DecShow));
                }
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
                oW_Conpon = aoW_Conpon[ogdForm.CurrentRow.Index];
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wListCoupon", "ocmAccept_Click : " + oEx.Message); }
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
