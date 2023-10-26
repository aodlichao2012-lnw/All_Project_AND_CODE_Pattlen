using AdaPos.Class;
using AdaPos.Models.Webservice.Respond.Customer;
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

namespace AdaPos.Popup.All
{
    public partial class wCstDetail : Form
    {
        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private cmlResSKCCstByCst oW_CstDetail;
        public wCstDetail(cmlResSKCCstByCst poCstDetail)
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                oW_CstDetail = poCstDetail;
                W_SETxDesign();
                W_SETxText();
                W_DATxLoadCst(oW_CstDetail);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "wCstDetail : " + oEx.Message);
            }
        }

        private void W_SETxDesign()
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                new cSP().SP_SETxSetGridviewFormat(ogdCst);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "W_SETxDesign : " + oEx.Message);
            }
            finally
            {
                oW_SP.SP_CLExMemory();
            }
        }

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

                //Title
                olaTitleCstSearch.Text = oW_Resource.GetString("tTitleCstDetail");
                olaPoint.Text = oW_Resource.GetString("tTitleRwdPnt"); 
                // DataGrid
                otbColPdtName.HeaderText = oW_Resource.GetString("tTitlePdtName");
                otbColPdtCode.HeaderText = oW_Resource.GetString("tTitlePdtCode");
                otbColQuotas.HeaderText = oW_Resource.GetString("tTitleQuotas");
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "W_SETxText : " + oEx.Message);
            }
        }

        public void W_DATxLoadCst(cmlResSKCCstByCst poCstDetail)
        {
            List<cmlQuotas> aoQuotas;
            try
            {
                aoQuotas = new List<cmlQuotas>();
                aoQuotas = poCstDetail.Privileges.Quotas;
                olaName.Text = poCstDetail.FirstName + " " + poCstDetail.LastName;
                olaPoint.Text = olaPoint.Text + ": "+ poCstDetail.Privileges.Point.ToString()==null? "0" : poCstDetail.Privileges.Point.ToString();

                foreach (cmlQuotas oRow in aoQuotas)
                {
                    ogdCst.Rows.Add(oRow.MatNo, "", oRow.Qty);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "W_DATxLoadCst : " + oEx.Message);
            }
            finally
            {
                oW_SP.SP_CLExMemory();
            }
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                cVB.nVB_CstPoint = oW_CstDetail.Privileges.Point;
                cVB.nVB_CstPiontB4Used = oW_CstDetail.Privileges.Point;
                cVB.tVB_CstName = oW_CstDetail.FirstName + " " + oW_CstDetail.LastName;
                cVB.oVB_Sale.W_SETxTextCst();

                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wCstDetail", "ocmAccept_Click : " + oEx.Message);
            }
            finally
            {
                oW_SP.SP_CLExMemory();
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
                new cLog().C_WRTxLog("wCstDetail", "ocmBack_Click : " + oEx.Message);
            }
        }
    }
}
