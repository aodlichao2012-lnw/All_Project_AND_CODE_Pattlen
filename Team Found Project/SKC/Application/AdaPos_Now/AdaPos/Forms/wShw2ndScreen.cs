using AdaPos.Class;
using AdaPos.Resources_String.Local;
using System;
using System.Collections;
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

namespace AdaPos.Forms
{
    public partial class wShw2ndScreen : Form
    {
        #region Variable

        public cSP oW_SP = new cSP();
        public decimal cW_AmtTotalShw, cW_AmtTotalCal, cW_AmtTotalPay, cW_CashPayment;
        public decimal cW_DisChgAmt;    
        private ResourceManager oW_Resource;
        private Control.uVideo2ndScreen oVideo2nd; //*Net 63-05-02 ย้ายมากจา FormLoad ให้สามารถเรียกใช้ได้ทุก Fn
        int nX = 1050;
        int nY = 10;
        int nXFoot = 600;
        int nYFoot = 50;

        #endregion

        #region Constructor
        public wShw2ndScreen()
        {
            InitializeComponent();
           
            //*Net 63-05-02 ย้ายการประกาศ UserControl ไปไว้ส่วน GlobalVar
            oVideo2nd = new Control.uVideo2ndScreen();
            oVideo2nd.Dock = DockStyle.Fill;
            opnVideo.Controls.Add(oVideo2nd);
        }

        #endregion

        #region Event

        /// <summary>
        /// Event Load 2nd Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wShw2ndScreen_Load(object sender, EventArgs e)
        {

            olaTotal2nd.BackColor = Color.Transparent;
            olaTotal2nd.ForeColor = Color.White;

            opnHead.BackColor = cVB.oVB_ColDark;

            opnFooter.BackColor = cVB.oVB_ColLight;
            opnSale.BackColor = cVB.oVB_ColLight;
            opnPayment.BackColor = cVB.oVB_ColLight;

            olaTitleAmt.BackColor = cVB.oVB_ColLight;
            olaTitleAmt.Font = new Font("", 25f, FontStyle.Bold);
            olaTitleAmt.ForeColor = Color.White;
            otbAmount.BackColor = cVB.oVB_ColLight;
            otbAmount.Font = new Font("", 45f, FontStyle.Bold);
            otbAmount.ForeColor = Color.White;

            otl2ndSale.BackColor = cVB.oVB_ColDark;
            tableLayoutPanel1.BackColor = cVB.oVB_ColDark;
            tableLayoutPanel2.BackColor = cVB.oVB_ColDark;
            //opnBG.BackColor = cVB.oVB_ColLight;

            olbCashPayment.Text = cVB.oVB_GBResource.GetString("tWelcome");
            olbCashPayment.Font = new Font("", 25f, FontStyle.Bold);
            olbCashPayment.ForeColor = Color.White;
            olbCashPayment.TextAlign = ContentAlignment.MiddleLeft;

            otbTextMove.Text = "ยินดีต้อนรับ";
            otbTextMove.Font = new Font("", 15f, FontStyle.Bold);
            otbTextMove.ForeColor = cVB.oVB_ColDark;

            otbFooterText.Text = "ประกาศ !!!";
            otbFooterText.Font = new Font("", 25f, FontStyle.Bold);
            otbFooterText.ForeColor = cVB.oVB_ColDark;
            
            olaTotalLabel.Text = "รวมเป็นเงิน";
            olaTotalLabel.Font = new Font("", 20f, FontStyle.Bold);
            olaTotalLabel.ForeColor = Color.White;
            olaTotalLabel.TextAlign = ContentAlignment.MiddleLeft;

            olaQtyLabel.Text = "จำนวนสินค้า";
            olaQtyLabel.Font = new Font("", 20f, FontStyle.Bold);
            olaQtyLabel.ForeColor = Color.White;
            olaQtyLabel.TextAlign = ContentAlignment.MiddleLeft;

            olaQtyTotal.Text = "0.00";
            olaQtyTotal.Font = new Font("", 20f, FontStyle.Bold);
            olaQtyTotal.ForeColor = Color.White;
            olaQtyTotal.TextAlign = ContentAlignment.MiddleRight;

            olaTotal.Text = "0.00";
            olaTotal.Font = new Font("", 20f, FontStyle.Bold);
            olaTotal.ForeColor = Color.White;
            olaTotal.TextAlign = ContentAlignment.MiddleRight;
            olaQtyHide.ForeColor = Color.Transparent;
            olaTotalHide.ForeColor = Color.Transparent;

            opnSale.Visible = true;
            opnPayment.Visible = false;
            opnSale.Update();
            //W_SETxText();
            //ogdCash.ColumnHeadersDefaultCellStyle.BackColor = cVB.oVB_ColDark;
            oW_SP.SP_SETxSetGridviewFormat(ogdCash); //*Net 63-03-03 Set Design Gridview


            otmTimeMoveText.Interval = 1;

            otmTimeMoveText.Start();

        }

        /// <summary>
        /// Event Timer Text Move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otmTimeMoveText_Tick(object sender, EventArgs e)
        {
            otbTextMove.SetBounds(nX, nY, 1, 1);
            nX--;
            if (nX <= 1)
            {
                nX = 1050;
            }

            otbFooterText.SetBounds(nXFoot, nYFoot, 1, 1);
            nXFoot--;
            if (nXFoot <= 1)
            {
                nXFoot = 600;
            }

        }

        /// <summary>
        /// Event Play/Pause Follow by Form Visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wShw2ndScreen_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) oVideo2nd.owm2ndScreen.Ctlcontrols.play();
            else oVideo2nd.owm2ndScreen.Ctlcontrols.pause();
        }

        #endregion

        #region Function

        /// <summary>
        /// Function Clear Data 
        /// </summary>
        public void W_DATxClear()
        {
            ogdCash.Rows.Clear();
            ogdSum.Rows.Clear();
            olaTotal2nd.Text = "0.00";
            otbAmount.Text = "0.00";
            cW_AmtTotalShw = 0;
            cW_AmtTotalCal = 0;
            cW_AmtTotalPay = 0;
            cW_CashPayment = 0;
            cW_DisChgAmt = 0;
            olaQtyTotal.Text = "0.00";
            olaTotal.Text = "0.00";
            olbCashPayment.Text = cVB.oVB_GBResource.GetString("tWelcome");
            olaCstName.Text = "";
            olaGrpCst.Text = "";
            opnPayment.Visible = false;
            opnSale.Visible = true;

        }

        /// <summary>
        /// Function Payment Back To Sale
        /// </summary>
        public void W_PRCxBack()
        {
            opnPayment.Visible = false;
            opnSale.Visible = true;
            olaTotal.Text = olaTotalHide.Text;
            olaQtyTotal.Text = olaQtyHide.Text;
            olbCashPayment.Text = cVB.oVB_GBResource.GetString("tWelcome");
            olaTotal2nd.Text = "0.00";
        }

        /// <summary>
        /// Function Sale To Payment
        /// </summary>
        /// <param name="ptTotal"></param>
        public void W_PRCxSaleToPay(string ptTotal)
        {            
            cVB.oVB_2ndScreen.opnPayment.Visible = true;
            cVB.oVB_2ndScreen.opnSale.Visible = false;
            ogdSum.Rows.Clear();
            olaTotalHide.Text = olaTotal.Text;
            olaQtyHide.Text = olaQtyTotal.Text;
            olaTotal2nd.Text = ptTotal;
            W_SETxText();
            ogdSum.Update();
        }
        
        public void W_PRCxSaleOrderTo2nd(string ptPdtName,string ptPdtPrice,string ptPdtQtyAll,string ptPdtAmount)
        {
            olbCashPayment.Text = ptPdtName;
            olaTotal2nd.Text = ptPdtPrice;

            olaQtyTotal.Text = ptPdtQtyAll;
            olaTotal.Text = ptPdtAmount;

            olbCashPayment.Update();
            olaTotal2nd.Update();
            olaQtyTotal.Update();
            olaTotal.Update();
        }

        /// <summary>
        /// Function Set Text , Set GridView Summary And Set User Point
        /// </summary>
        public void W_SETxText()
        {
            try
            {

                // Language
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resPayment_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resPayment_EN));
                        break;
                }

                // Label
                olbCashPayment.Text = oW_Resource.GetString("tCashPayment");

                // Set Header Grid Payment
                otbTitleCshSeq.HeaderText = cVB.oVB_GBResource.GetString("tSeq");
                otbTitleCshDetail.HeaderText = cVB.oVB_GBResource.GetString("tDetail");
                otbTitleCshAmt.HeaderText = cVB.oVB_GBResource.GetString("tAmount");

                // Summary
                //cVB.cVB_RoundDiff = cSP.SP_CALcRoundDiff(Convert.ToDecimal(olaTotal2nd.Text));   
                cW_AmtTotalShw = Convert.ToDecimal(olaTotal2nd.Text) + cVB.cVB_RoundDiff;    
                cW_AmtTotalCal = Convert.ToDecimal(olaTotal2nd.Text);    
                cW_AmtTotalPay = 0; 
                cW_DisChgAmt = 0;
                // end Summary

                // Set Grid Summary
                ogdSum.Rows.Add(cVB.oVB_GBResource.GetString("tTotal"), oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalCal, 2));
                ogdSum.Rows.Add(oW_Resource.GetString("tBillDis"), oW_SP.SP_SETtDecShwSve(1, 0, 2));
                ogdSum.Rows.Add(oW_Resource.GetString("tNetSale"), oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, 2));
                ogdSum.Rows.Add(oW_Resource.GetString("tAmtTendered"), oW_SP.SP_SETtDecShwSve(1, 0, 2));
                ogdSum.Rows.Add(oW_Resource.GetString("tCashRnd"), oW_SP.SP_SETtDecShwSve(1, cVB.cVB_RoundDiff, 2));
                ogdSum.Rows.Add(oW_Resource.GetString("tCashPayment"), oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, 2));
                // end Set Grid Summary
                
                // User Label And Value
                olaTitleAmt.Text = cVB.oVB_GBResource.GetString("tAmount");
                olaTitlePoint.Text = cVB.oVB_GBResource.GetString("tPoint") + " : ";
                olaTitleGrpCst.Text = cVB.oVB_GBResource.GetString("tGrpCst") + " : ";
                olaTitleCstExp.Text = cVB.oVB_GBResource.GetString("tExpire") + " : ";

                olaCstName.Text = cVB.tVB_CstName;
                olaGrpCst.Text = cVB.tVB_PriceGroup;
                // end User

                // Price Total And Price Amount
                //cVB.cVB_RoundDiff = cSP.SP_CALcRoundDiff(Convert.ToDecimal(olaTotal2nd.Text));   
                olaTotal2nd.Text = oW_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(olaTotal2nd.Text) + cVB.cVB_RoundDiff, cVB.nVB_DecShow);
                otbAmount.Text = olaTotal2nd.Text;
                // End
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShw2ndScreen", "W_SETxText : " + oEx.Message); }
        }


        /// <summary>
        /// Function Add Pay To Grid Payment Action Form Payment 
        /// </summary>
        /// <param name="ptRcvName"></param>
        /// <param name="pcValue"></param>
        public void W_DATxAddPay2Grid(string ptRcvName, decimal pcValue)
        {
            try
            {
                ogdCash.Rows.Add(ogdCash.RowCount + 1, ptRcvName, oW_SP.SP_SETtDecShwSve(1, pcValue, cVB.nVB_DecShow));

                cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                if (cW_AmtTotalCal < 0)
                {
                    cW_AmtTotalCal = 0;
                }
                else
                {
                    //cVB.cVB_RoundDiff = cSP.SP_CALcRoundDiff(cW_AmtTotalCal);
                }
                cW_AmtTotalShw = cW_AmtTotalCal + cVB.cVB_RoundDiff;
                cW_AmtTotalPay = cW_AmtTotalPay + pcValue;


                olaTotal2nd.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
                otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);

                W_PRCxSumTender();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShw2ndScreen", "W_DATxAddPay2Grid : " + oEx.Message); }
            finally
            {

            }
        }
        
        /// <summary>
        /// Function Summary Form Payment
        /// </summary>
        public void W_PRCxSumTender()
        {
            try
            {
                ogdSum.Rows[3].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalPay, cVB.nVB_DecShow);
                ogdSum.Rows[4].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_RoundDiff, cVB.nVB_DecShow);
                ogdSum.Rows[5].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wShw2ndScreen", "W_PRCxSumTender : " + oEx.Message); }
            finally
            { }
        }

        /// <summary>
        /// Function Add Discound To Grid Payment And Summary
        /// </summary>
        /// <param name="ptStaDisChg"></param>
        /// <param name="ptDisChgTxt"></param>
        /// <param name="pcValue"></param>
        public void W_ADDxDisChgBill(string ptStaDisChg, string ptDisChgTxt, decimal pcValue)
        {
            string tDisChg = "";
            try
            {
                switch (ptStaDisChg)
                {
                    case "1":
                    case "2":
                        tDisChg = cVB.oVB_GBResource.GetString("tDis") + "(" + ptDisChgTxt + ")";
                        cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                        cW_DisChgAmt = cW_DisChgAmt + pcValue;
                        break;
                    case "3":
                    case "4":
                        tDisChg = cVB.oVB_GBResource.GetString("tChg") + "(" + ptDisChgTxt + ")";
                        cW_DisChgAmt = cW_DisChgAmt - pcValue;
                        cW_AmtTotalCal = cW_AmtTotalCal + pcValue;  //*Em 63-01-08
                        break;
                    case "5":   //คูปองส่วนลด
                        tDisChg = cVB.oVB_GBResource.GetString("tCpnDis") + "(" + ptDisChgTxt + ")";
                        cW_AmtTotalCal = cW_AmtTotalCal - pcValue;
                        cW_DisChgAmt = cW_DisChgAmt + pcValue;
                        break;
                }
                ogdCash.Rows.Add(ogdCash.RowCount + 1, tDisChg, oW_SP.SP_SETtDecShwSve(1, pcValue, cVB.nVB_DecShow));

                //cVB.cVB_RoundDiff = cSP.SP_CALcRoundDiff(cW_AmtTotalCal);
                cW_AmtTotalShw = cW_AmtTotalCal + cVB.cVB_RoundDiff;

                ogdSum.Rows[1].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_DisChgAmt, cVB.nVB_DecShow);
                ogdSum.Rows[2].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
                ogdSum.Rows[3].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalPay, cVB.nVB_DecShow);
                ogdSum.Rows[4].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cVB.cVB_RoundDiff, cVB.nVB_DecShow);
                ogdSum.Rows[5].Cells[1].Value = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);

                olaTotal2nd.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);
                otbAmount.Text = oW_SP.SP_SETtDecShwSve(1, cW_AmtTotalShw, cVB.nVB_DecShow);

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wPayment", "W_ADDxDisChgBill : " + oEx.Message); }
            finally
            {

            }
        }
        #endregion



      



    }
}
