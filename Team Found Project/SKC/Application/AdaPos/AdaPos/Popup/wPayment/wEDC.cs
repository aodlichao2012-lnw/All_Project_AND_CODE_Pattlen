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
    public partial class wEDC : Form
    {
        private AdaEDC_NET.cEDC oW_EDC = new AdaEDC_NET.cEDC();
        private bool bW_StatusEDC;
        private ResourceManager oW_Resource;
        wSearch2Column oSch2Col;
        public string tCodeBank;
        public string tCrdCode;


        public wEDC()
        {
            InitializeComponent();
            W_SETxDesign();
            W_SETxText();
            //W_DATxCreditCard2Combo();
        }

        #region Function
        private void W_DATxCreditCard2Combo()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            try
            {
                //ocbSelectBank.Items.Clear();

                oSql.AppendLine("SELECT ISNULL(CCDL.FTCrdName,(SELECT TOP 1 FTCrdName FROM TFNMCreditCard_L with(nolock) WHERE FTCrdCode = CCD.FTCrdCode)) AS FTCrdName,CCD.FTBnkCode ");
                oSql.AppendLine("FROM TFNMCreditCard CCD with(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMCreditCard_L CCDL with(NOLOCK) ON CCD.FTCrdCode = CCDL.FTCrdCode AND CCDL.FNLngID = " + cVB.nVB_Language);
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                //ocbSelectBank.DataSource = odtTmp;
                //ocbSelectBank.DisplayMember = "FTCrdName";
                //ocbSelectBank.ValueMember = "FTBnkCode";

            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "W_DATxCreditCard2Combo : " + ex.Message); }
        }

        private void W_PRCxPayment()
        {
            try
            {
                oW_EDC = new AdaEDC_NET.cEDC();
                oW_EDC.EventRunAbortWork += new AdaEDC_NET.cEDC.RunAbortWorkDelegate(oW_EDC_EventRunAbortWork);
                oW_EDC.EventRunCompleted += new AdaEDC_NET.cEDC.RunCompletedDelegate(oW_EDC_EventRunCompleted);
                oW_EDC.EventRunNotCompleted += new AdaEDC_NET.cEDC.RunNotCompletedDelegate(oW_EDC_EventRunNotCompleted);
                oW_EDC.SetModel = cPayment.oC_EDCSel.FTSedModel;
                oW_EDC.SetNetAmt =Convert.ToDouble( cVB.cVB_Amount);
                oW_EDC.SetComPortName = "COM" + cPayment.oC_EDCSel.FTPhwConnRef;
                oW_EDC.SetUseAck = cPayment.oC_EDCSel.FNSedAck == 1 ? true:false ;
                oW_EDC.SetWaitTime = cPayment.oC_EDCSel.FNSedTimeOut;
                oW_EDC.SetLogFile = Environment.CurrentDirectory + @"\Log\EDC" + DateTime.Now.ToString("yyMMdd") + ".log";
                oW_EDC.SetUseTestPortFirst = false;
                oW_EDC.SetTransactionCode = "20";
                oW_EDC.RunWork();
                this.Enabled = false;

            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "W_PRCxPayment : " + ex.Message); }
        }

        #endregion Function
        /// <summary>
        /// Set design
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
                //this.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;
                ocmSearch.BackColor = cVB.oVB_ColDark;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wEDC", "W_SETxDesign : " + oEx.Message); }
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

                olaTitleEdc.Text = oW_Resource.GetString("tTitleCreditCard");
                olaCreditNumber.Text = oW_Resource.GetString("tTitleNo");
                olaBank.Text = oW_Resource.GetString("tTitleBank");
                olaApproveCode.Text = oW_Resource.GetString("tTitleApvCode");
                olaTraceCode.Text = oW_Resource.GetString("tTitleTraceCode");
                olaTitleCardName.Text = cVB.oVB_GBResource.GetString("tCardType");

                //*Em 63-03-08
                if (cVB.cVB_Amount == cVB.oVB_Payment.cW_AmtTotalShw)
                {
                    cVB.cVB_Amount = cVB.oVB_Payment.cW_AmtTotalCal;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wChange", "W_SETxText : " + oEx.Message); }
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
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "ocmBack_Click : " + ex.Message); }
        }

        /// <summary>
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wEDC_Shown(object sender, EventArgs e)
        {
            try
            {
                W_PRCxPayment();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "wEDC_Shown : " + ex.Message); }
        }

        /// <summary>
        /// Form Closing 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wEDC_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "wEDC_FormClosing : " + ex.Message); }
        }

        private void oW_EDC_EventRunAbortWork(string ptMessage)
        {
            try
            {
                string tMsg = "";

                this.Enabled = true;
                bW_StatusEDC = true;

                tMsg = ptMessage.Split(';')[cVB.nVB_Language] + Environment.NewLine + oW_Resource.GetString("tMsgManualKey");
                if (MessageBox.Show(tMsg,this.Text,MessageBoxButtons.YesNo) ==DialogResult.Yes)
                {
                    olaStatusEDC.Text = "Offline";
                    olaStatusEDC.ForeColor = Color.Red;
                    this.otbCreditNo.Focus();
                }
                else
                    this.Close();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "oW_EDC_EventRunAbortWork : " + ex.Message); }
            
        }

        private void oW_EDC_EventRunCompleted(AdaEDC_NET.cResult poResult)
        {
            try
            {
                string tCreditNo = "";
                otbCreditNo.Text = poResult.GetCardNumber;
                //ocbSelectBank.Text = poResult.GetCardBankCode;
                cPayment.tC_BnkCode = poResult.GetCardBankCode;
                otbTraceCode.Text = poResult.GetTraceCode;
                otbApproveCode.Text = poResult.GetApproveCode;
                if (string.IsNullOrEmpty(otbApproveCode.Text))
                    otbApproveCode.Text = otbTraceCode.Text ;
                bW_StatusEDC = true;
                this.Enabled = true;
                // By Zen
                tCreditNo = cPayment.W_PRCtCheckCreditNo(otbCreditNo.Text);
                W_CHKxBankCode(cPayment.W_GETtBankCode(tCreditNo));

                if (this.otbCreditNo.Text != "" & this.otbSelectBank.Text != "")
                {
                    this.ocbSelectBank_SelectedIndexChanged(this.otbSelectBank, new System.EventArgs());
                    if (this.otbSelectBank.Text != "")
                    {
                        if (this.bW_StatusEDC == true)
                        {
                            this.ocmAccept_Click(ocmAccept, null);
                        }
                    }
                }
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "oW_EDC_EventRunCompleted : " + ex.Message); }

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
        private void oW_EDC_EventRunNotCompleted(string ptMessage)
        {
            try
            {
                this.Enabled = true;
                bW_StatusEDC = true;
                olaStatusEDC.Text = "Offline";
                olaStatusEDC.ForeColor = Color.Red;
                this.otbCreditNo.Focus();
                oW_EDC.AbortWork();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "oW_EDC_EventRunNotCompleted : " + ex.Message); }

        }

        private void ocbSelectBank_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ocmAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(otbCreditNo.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputNum"), 3);
                    otbCreditNo.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(otbSelectBank.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgInputBank"), 3);
                    otbSelectBank.Focus();
                    return;
                }

                if (bW_StatusEDC == false)
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgWorking"), 3);
                    return;
                }

                cPayment.tC_CrdNo = otbCreditNo.Text.Trim();
                if( string.IsNullOrEmpty(cPayment.tC_BnkCode)) cPayment.tC_BnkCode = tCodeBank;
                cPayment.tC_BnkName = otbSelectBank.Text;
                cPayment.tC_CrdTrans = otbTraceCode.Text.Trim();
                cPayment.tC_CrdApvCode = otbApproveCode.Text.Trim();
                cPayment.tC_CrdCode = tCrdCode;
                this.Close();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("wEDC", "ocmAccept_Click : " + ex.Message); }
        }

        private void otbCreditNo_Enter(object sender, EventArgs e)
        {
            oucNumpad.oU_TextValue = otbCreditNo;
            oucNumpad.tU_TextValue = otbCreditNo.Text;
            
        }

        private void otbApproveCode_Enter(object sender, EventArgs e)
        {
            oucNumpad.oU_TextValue = otbApproveCode;
            oucNumpad.tU_TextValue = otbApproveCode.Text;
        }

        private void otbTraceCode_Enter(object sender, EventArgs e)
        {
            oucNumpad.oU_TextValue = otbTraceCode;
            oucNumpad.tU_TextValue = otbTraceCode.Text;
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

        private void ocmSearch_Click(object sender, EventArgs e)
        {
            string tCreditNo = "";

            //if (otbCreditNo.TextLength == 0)
            //{
            //    cVB.tVB_CardBin = "";                
            //}
            cVB.tVB_CardBin = "";   //*Em 63-03-08
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

