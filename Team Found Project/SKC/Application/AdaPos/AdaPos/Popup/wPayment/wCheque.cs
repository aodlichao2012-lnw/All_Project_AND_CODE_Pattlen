using AdaPos.Class;
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
using Dapper;
using System.Data.SqlClient;
using AdaPos.Resources_String.Local;

namespace AdaPos.Popup.wPayment
{
    public partial class wCheque : Form
    {
        private ResourceManager oW_Resource;
        public wCheque()
        {
            InitializeComponent();

            try
            {
                W_SETxDesign();
                W_SETxText();
                W_DATxBankCombo();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCheque", "wCheque : " + oEx.Message); }
        }

        #region Function
        /// <summary>
        /// Set design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                //this.BackColor = cVB.oVB_ColDark;   //*Em 63-03-02 //*Net 63-04-01 ยกมจาก baseline
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmShwKb.BackColor = cVB.oVB_ColDark;
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCheque", "W_SETxDesign : " + oEx.Message); }
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
                olaTitleCheck.Text = cPayment.tC_RcvName;
                olaBank.Text = oW_Resource.GetString("tTitleBank");
                olaChequeNo.Text = oW_Resource.GetString("tChequeNo");
                olaDate.Text = oW_Resource.GetString("tChequeDate");
                otbChequeNo.Text = "";
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCheque", "W_SETxText : " + oEx.Message); }
        }
        private void W_DATxBankCombo()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            //SqlConnection oConn = null;

            try
            {
                ocbSelectBank.Items.Clear();
                
                oSql.AppendLine("SELECT ISNULL(BNKL.FTBnkName, (SELECT TOP 1 FTBnkName FROM TFNMBank_L with(nolock) WHERE FTBnkCode = BNK.FTBnkCode)) AS FTBnkName, BNK.FTBnkCode");
                oSql.AppendLine("FROM TFNMBank BNK with(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMBank_L BNKL with(NOLOCK) ON BNK.FTBnkCode = BNKL.FTBnkCode AND BNKL.FNLngID = " + cVB.nVB_Language);


                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                
                //oConn = new SqlConnection(@"Data Source=.\SQLSERVER2016;Initial Catalog=PTT_PosFront;User ID=sa;Password=adasoft");
                //IDataReader oDR = oConn.ExecuteReader(oSql.ToString(), commandTimeout: 60);
                //odtTmp.Load(oDR);

                ocbSelectBank.DataSource = odtTmp;
                ocbSelectBank.DisplayMember = "FTBnkName";
                ocbSelectBank.ValueMember = "FTBnkCode";
                
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wCheque", "W_DATxBankCombo : " + ex.Message); }
        }
        #endregion End Function

        #region Method/Events
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if(otbChequeNo.Text =="") return; //*Arm 62-10-15
                cPayment.tC_ChequeNo = otbChequeNo.Text;
                cPayment.tC_BnkCode = ocbSelectBank.SelectedValue.ToString();
                cPayment.dC_ChequeDate = Convert.ToDateTime(otpDate.Value.ToString("yyyy-MM-dd"));
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCheque", "ocmAccept_Click : " + oEx.Message); }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCheque", "ocmShwKb_Click : " + oEx.Message); }
            finally
            {
                otbChequeNo.Focus();
            }
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCheque", "ocmBack_Click : " + oEx.Message); }
        }
        
        #endregion End Method/Events
    }
}
