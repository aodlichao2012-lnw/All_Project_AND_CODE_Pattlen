using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
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
    public partial class wRentalPdtBudgetRevise : Form
    {
        public List<cmlPdtRental> oaW_PdtRentalOrder;

        private int nW_Time;
        private ResourceManager oW_Resource;
        private cSP oW_SP;

        private decimal cW_Sum_Deposit = 0;
        private decimal cW_Sum_Rate = 0;
        private decimal cW_Sum_Amount = 0;

        public wRentalPdtBudgetRevise()
        {
            InitializeComponent();
            W_SETxText();
            oW_SP.SP_SETxSetGridviewFormat(ogdProduct); //*Net 63-03-02 Set Style
        }

        private void wRentalPdtBudgetRevise_Shown(object sender, EventArgs e)
        {
            try
            {
                W_SETxData();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdtBudgetRevise", "wRentalPdtBudgetRevise_Shown : " + oEx.Message);
            }
        }

        private void W_SETxData()
        {
            try
            {
                ogdProduct.Rows.Clear();

                if (oaW_PdtRentalOrder == null)
                {
                    return;
                }
                var oOrder = oaW_PdtRentalOrder.Distinct().ToList();
                if (oOrder.Count > 0)
                {
                    foreach (var oPdtCode in oOrder)
                    {
                        string nNo = (ogdProduct.Rows.Count + 1).ToString();
                        string tProductID = oPdtCode.FTPdtCode;
                        string tProductName = oPdtCode.FTPdtName;
                        string tDeposit = oPdtCode.FCPdtDeposit;
                        string tStaPay = oPdtCode.FTPdtStaPay;

                        string tRate = "0";
                        if (tStaPay == "1")
                        {
                            tRate = oaW_PdtRentalOrder.Where(x => x.FTPdtCode == tProductID && x.FNRtdSeqNo == "1").Select(x => x.FCRtdPrice).FirstOrDefault();
                        }

                        string tQty = oaW_PdtRentalOrder.Where(x => x.FTPdtCode == tProductID).Count().ToString();
                        string tAmount = W_SUMtPrice(tDeposit, tRate, tQty);

                        ogdProduct.Rows.Add(nNo, tProductID, tProductName, tDeposit, tRate, tQty, tAmount);

                        olaCount.Text = "Qty : " + nNo;
                    }

                    olaSumDeposit.Text = cW_Sum_Deposit.ToString();
                    olaSumPrePaid.Text = cW_Sum_Rate.ToString();
                    olaSumTotalHD.Text = cW_Sum_Amount.ToString();
                    olaSumTotal.Text = cW_Sum_Amount.ToString();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdtBudgetRevise", "SETxData : " + oEx.Message);
            }
        }

        private void otmClose_Tick(object sender, EventArgs e)
        {
            try
            {
                if (nW_Time == 5)
                    this.Close();

                nW_Time++;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdtBudgetRevise", "otmClose_Tick : " + oEx.Message);
            }
        }

        private void wRentalPdtBudgetRevise_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                otmClose.Stop();
                oW_Resource = null;
                //oW_SP.SP_CLExMemory();
                oW_SP = null;

                this.Dispose();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdtBudgetRevise", "wRentalPdtBudgetRevise_FormClosing : " + oEx.Message);
            }
        }

        /// <summary>
        /// Set text
        /// </summary>
        public void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resRentalPdt_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resRentalPdt_EN));
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdtBudgetRevise", "W_SETxText : " + oEx.Message);
            }
        }

        public string W_SUMtPrice(string ptDeposit, string ptRtdPrice, string ptQty)
        {
            decimal cSum = 0;
            try
            {
                decimal cDeposit;
                decimal.TryParse(ptDeposit, out cDeposit);

                decimal cRtdPrice;
                decimal.TryParse(ptRtdPrice, out cRtdPrice);

                decimal cQty;
                decimal.TryParse(ptQty, out cQty);

                cSum = (cDeposit + cRtdPrice) * cQty;

                cW_Sum_Deposit = cW_Sum_Deposit + cDeposit;
                cW_Sum_Rate = cW_Sum_Rate + cRtdPrice;
                cW_Sum_Amount = cW_Sum_Amount + cSum;

                return cSum.ToString();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdtBudgetRevise", "W_SUMtPrice : " + oEx.Message);
                return cSum.ToString();
            }
        }

        private void ogdProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == ogdProduct.Columns[7].Index && e.RowIndex >= 0)
                {
                    if (this.ogdProduct.SelectedRows.Count > 0)
                    {
                        ogdProduct.Rows.RemoveAt(this.ogdProduct.SelectedRows[0].Index);
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRentalPdtBudgetRevise", "ogdProduct_CellClick : " + oEx.Message);
            }
        }
    }
}
