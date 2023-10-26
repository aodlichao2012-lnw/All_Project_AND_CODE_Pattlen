using AdaPos.Class;
using AdaPos.Popup.All;
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
    public partial class wCreditCard : Form
    {
        private ResourceManager oW_Resource;
        wSearch2Column oSch2Col;
        public string tCodeBank;
        public string tCrdCode;
        public wCreditCard()
        {
            InitializeComponent();
            try
            {
                W_SETxDesign();
                W_SETxText();
               // W_GETxBnkToCombo();
                onpNumpad.oU_TextValue = otbCreditNo;
                onpNumpad.tU_TextValue = otbCreditNo.Text;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCreditCard", "wCreditCard : " + oEx.Message); }
        }

        #region Function
        /// <summary>
        /// Set Design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                //this.BackColor = cVB.oVB_ColDark;   //*Em 63-03-02
                ocmBack.BackColor = cVB.oVB_ColDark;
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmSearch.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCreditCard", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set Text
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


                olaTitleEdc.Text = oW_Resource.GetString("tTitleCreditCard");
                olaCreditNumber.Text = oW_Resource.GetString("tTitleNo");
                olaCreditBank.Text = oW_Resource.GetString("tTitleBank");
                olaTitleCardName.Text = cVB.oVB_GBResource.GetString("tCardType");

                //*Em 63-03-08
                if (cVB.cVB_Amount == cVB.oVB_Payment.cW_AmtTotalShw)
                {
                    cVB.cVB_Amount = cVB.oVB_Payment.cW_AmtTotalCal;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCreditCard", "W_SETxText : " + oEx.Message); }
        }
        private void W_GETxBnkToCombo(string ptCreditNo = "")
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                if (ptCreditNo == "")
                {
                    oSql.AppendLine("SELECT BNK.FTBnkCode,CASE WHEN ISNULL(BNKL.FTBnkName,'') = '' THEN (SELECT TOP 1 FTBnkName FROM TFNMBank_L with(nolock) WHERE FTBnkCode = BNK.FTBnkCode) ELSE BNKL.FTBnkName END AS FTBnkName");
                oSql.AppendLine("FROM TFNMBank BNK with(nolock)");
                oSql.AppendLine("LEFT JOIN TFNMBank_L BNKL with(nolock) ON BNK.FTBnkCode = BNKL.FTBnkCode AND BNKL.FNLngID = " + cVB.nVB_Language);
                DataTable oDT = oDB.C_GEToDataQuery(oSql.ToString());
                    //ocbSelectBank.Items.Clear();
                    //ocbSelectBank.DataSource = oDT;
                    //ocbSelectBank.DisplayMember = "FTBnkName";
                    //ocbSelectBank.ValueMember = "FTBnkCode";
                }
                else
                {
                    //string tBankCode = W_GETtBankCode(ptCreditNo);                   
                    //ocbSelectBank.SelectedValue = tBankCode;

                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCreditCard", "W_GETxBnkToCombo : " + oEx.Message); }
            finally
            {
                oDB = null;

            }

        }
        #endregion Function
        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCreditCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCreditCard", "wCreditCard_FormClosing : " + oEx.Message); }

        }

        private void wCreditCard_Shown(object sender, EventArgs e)
        {
            otbCreditNo.Focus();
        }

        private void ocmBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                cPayment.tC_CrdNo = otbCreditNo.Text;
                //cPayment.tC_BnkCode = ocbSelectBank.SelectedValue.ToString();
                //cPayment.tC_BnkName = ocbSelectBank.Text;
                //*Net 63-04-01 ยกมาจาก baseline
                cPayment.tC_BnkCode = tCodeBank;
                cPayment.tC_BnkName = otbSelectBank.Text;
                cPayment.tC_CrdCode = tCrdCode;
                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wCreditCard", "ocmAccept_Click : " + oEx.Message); }
        }

        private void otbCreditNo_KeyDown(object sender, KeyEventArgs e)
        {
            string tCreditNo = "";
            switch (e.KeyData)
            {
                case Keys.Enter:
                    tCreditNo = cPayment.W_PRCtCheckCreditNo(otbCreditNo.Text);
                    W_CHKxBankCode(cPayment.W_GETtBankCode(tCreditNo));

                    break;
            }

        }
        private void W_CHKxBankCode(DataTable poData)
        {
            if (poData.Rows.Count == 1)
            {
                otbSelectBank.Text = poData.Rows[0]["FTBnkName"].ToString();
                otbCardName.Text = poData.Rows[0]["FTCrdName"].ToString();
                tCodeBank = poData.Rows[0]["FTBnkCode"].ToString();
                tCrdCode = poData.Rows[0]["FTCrdCode"].ToString();
            }
            else
            {

                oSch2Col = new wSearch2Column("TFNMCreditCardBIN");

                if (oSch2Col.ShowDialog() == DialogResult.OK)
                {
                    otbSelectBank.Text = cPayment.W_GETtNameBankByCode(oSch2Col.oW_DataSearch.tCode);
                    otbCardName.Text = oSch2Col.oW_DataSearch.tName;
                    tCodeBank = oSch2Col.oW_DataSearch.tCode;
                    tCrdCode = cPayment.W_GETtCrdCodeByName(oSch2Col.oW_DataSearch.tName);
                }


            }
        }

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            string tCreditNo = "";
            if (otbCreditNo.TextLength == 0)
            {
                cVB.tVB_CardBin = "";
            }
            oSch2Col = new wSearch2Column("TFNMCreditCardBIN");
            //if (otbCreditNo.TextLength == 0)
            //{
            //    oSch2Col.otbSearchPdt.Text = otbSelectBank.Text;
            //}
            if (oSch2Col.ShowDialog() == DialogResult.OK)
            {
                otbSelectBank.Text = cPayment.W_GETtNameBankByCode(oSch2Col.oW_DataSearch.tCode);
                otbCardName.Text = oSch2Col.oW_DataSearch.tName;
                tCodeBank = oSch2Col.oW_DataSearch.tCode;
                tCrdCode = cPayment.W_GETtCrdCodeByName(oSch2Col.oW_DataSearch.tName);
            }
        }
    }
}
