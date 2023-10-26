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
    public partial class wDiscount : Form
    {
        //private cSP oW_SP;
        private ResourceManager oW_Resource;

        public string tFTXshDocNo;
        //S0000100003190000037
        /// <summary>
        /// 1 ลดแบบจำนวน,2 ลดแบบเปอร์เซ็น,3 ชาร์ตแบบจำนวน,4 ชาร์ตแบบเปอร์เซ็น
        /// </summary>
        private int nW_DisType;

        /// <summary>
        /// 1 ส่วนลดรายการ  2 ส่วนลดท้ายบิล
        /// </summary>
        private int nW_StaDisChg;

        /// <summary>
        /// ยอดลด Chg
        /// </summary>
        public decimal cW_DisChg;

        /// <summary>
        /// มูลค่าลด/ชาร์จ
        /// </summary>
        public decimal cW_Amt;

        /// <summary>
        /// ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// </summary>
        public string tW_DisChgTxt;


        public decimal cW_B4DisChg;
        public cmlDiscountItem oDis;
        private cSP oW_SP;
        private List<cmlProrateDT> aoW_ProDT; //*Arm 63-06-11

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnDisType"></param>
        /// <param name="pnStaDisChg"></param>
        /// <param name="paoProDT">Prorate ให้รายการ (*Arm 63-06-11)</param>
        public wDiscount(int pnDisType, int pnStaDisChg, List<cmlProrateDT> paoProDT = null)
        {
            InitializeComponent();
            try
            {
                nW_DisType = pnDisType;
                nW_StaDisChg = pnStaDisChg;
                
                if (paoProDT != null) //*Arm 63-06-11 Prorate รายการ (รองรับส่วนลดรายการท้ายบิล)
                {
                    aoW_ProDT = paoProDT;
                }
                
                W_SETxDesign();
                W_SETxText();

                oucNumpad.oU_TextValue = otbDisAmount;
                oucNumpad.tU_TextValue = otbDisAmount.Text;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "wDiscount " + oEx.Message);
            }
        }

        /// <summary>
        /// Design form
        /// </summary>
        private void W_SETxDesign()
        {
            try
            {
               // this.BackColor = cVB.oVB_ColDark;   //*Em 63-03-02 //*Net 63-04-01
                //Ping 2019.10.08
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmAccept.BackColor = cVB.oVB_ColDark;

                otbB4Dis.BackColor = cVB.oVB_ColLight;
                otbAfDis.BackColor = cVB.oVB_ColLight;
                ocmReason.BackColor = cVB.oVB_ColNormal; //*Arm 63-08-06
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "W_SETxDesign " + oEx.Message);
            }
        }

        /// <summary>
        /// Text form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oW_Resource = new ResourceManager(typeof(resDiscount_TH));
                        break;

                    default:    // EN
                        oW_Resource = new ResourceManager(typeof(resDiscount_EN));
                        break;
                }



                switch (nW_DisType)
                {
                    case 1:
                        olaTitle.Text = oW_Resource.GetString("olaDisAmt");
                        olaB4Dis.Text = oW_Resource.GetString("olaB4DisAmt");
                        olaDisAmount.Text = oW_Resource.GetString("olaAmount");
                        olaAfDis.Text = oW_Resource.GetString("olaAfDisAmt");
                        olaReason.Text = oW_Resource.GetString("olaReason");
                        break;
                    case 2:
                        olaTitle.Text = oW_Resource.GetString("olaDisPer");
                        olaB4Dis.Text = oW_Resource.GetString("olaB4DisPer");
                        //olaDisAmount.Text = oW_Resource.GetString("olaAmount");
                        olaDisAmount.Text = oW_Resource.GetString("olaAmountPer"); //*Arm 63-09-18
                        olaAfDis.Text = oW_Resource.GetString("olaAfDisPer");
                        olaReason.Text = oW_Resource.GetString("olaReason");
                        break;
                    case 3:
                        olaTitle.Text = oW_Resource.GetString("olaChgAmt");
                        olaB4Dis.Text = oW_Resource.GetString("olaB4ChgAmt");
                        olaDisAmount.Text = oW_Resource.GetString("olaAmount");
                        olaAfDis.Text = oW_Resource.GetString("olaAfChgAmt");
                        olaReason.Text = oW_Resource.GetString("olaReason");
                        break;
                    case 4:
                        olaTitle.Text = oW_Resource.GetString("olaChgPer");
                        olaB4Dis.Text = oW_Resource.GetString("olaB4ChgPer");
                        //olaDisAmount.Text = oW_Resource.GetString("olaAmount");
                        olaDisAmount.Text = oW_Resource.GetString("olaAmountPer"); //*Arm 63-09-18
                        olaAfDis.Text = oW_Resource.GetString("olaAfChgPer");
                        olaReason.Text = oW_Resource.GetString("olaReason");
                        break;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "W_SETxText " + oEx.Message);
            }
        }
        /// <summary>
        /// Paint Background
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                if (nW_StaDisChg == 1)
                {
                    using (SolidBrush oBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                    {
                        e.Graphics.FillRectangle(oBrush, e.ClipRectangle);
                        oBrush.Dispose();
                    }
                }
                else
                {
                    using (SolidBrush oBrush = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                    {
                        e.Graphics.FillRectangle(oBrush, e.ClipRectangle);
                        oBrush.Dispose();
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDiscount", "OnPaintBackground " + oEx.Message); }
        }
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            cmlTPSTSalDTDis oSalDis = new cmlTPSTSalDTDis();
            cDiscPolicy oDiscPolicy; //*Arm 63-07-18
            string tShwDisAmt = ""; //*Arm 63-08-12
            try
            {
                oDiscPolicy = new cDiscPolicy();    //*Arm 63-07-18

                oSalDis.FTBchCode = cVB.tVB_BchCode;
                oSalDis.FTXshDocNo = cVB.tVB_DocNo;
                if (otbDisAmount.Text == string.Empty || decimal.Parse(otbDisAmount.Text) == 0)
                {
                    return;
                }
                if (string.IsNullOrEmpty(otbReason.Text))
                {
                    new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgNotChooseRsn"), 3); //*Arm 63-07-22
                    return;
                }
                oSalDis.FTDisCode = W_GETtDiscode(nW_DisType);
                oSalDis.FTRsnCode = cVB.oVB_Reason.FTRsnCode;
                oSalDis.FTDisCode = cVB.tVB_DisCode;    //*Arm 63-07-15

                tShwDisAmt = new cSP().SP_SETtDecShwSve(1,Convert.ToDecimal(otbDisAmount.Text), cVB.nVB_DecShow); //*Arm 63-08-12 เก็บค่าทดศนิยม FTXddDisChgTxt ตามโชว์ 

                if (nW_StaDisChg == 1)
                {
                    // ตามรายการ
                    oSalDis.FNXsdSeqNo = cVB.oVB_OrderRowIndex;
                    oSalDis.FCXddNet = decimal.Parse(otbB4Dis.Text);
                    oSalDis.FNXddStaDis = 1;

                    switch (nW_DisType)
                    {
                        case 1:
                            //oSalDis.FTXddDisChgTxt = otbDisAmount.Text;
                            oSalDis.FTXddDisChgTxt = tShwDisAmt; //*Arm 63-08-12
                            oSalDis.FTXddDisChgType = "1";
                            oSalDis.FCXddValue = decimal.Parse(otbDisAmount.Text);
                            break;
                        case 2:
                            //oSalDis.FTXddDisChgTxt = otbDisAmount.Text + "%";
                            oSalDis.FTXddDisChgTxt = tShwDisAmt + "%"; //*Arm 63-08-12
                            oSalDis.FTXddDisChgType = "2";
                            oSalDis.FCXddValue = decimal.Parse(otbAfDis.Tag.ToString());
                            break;
                        case 3:
                            //oSalDis.FTXddDisChgTxt = otbDisAmount.Text;
                            oSalDis.FTXddDisChgTxt = tShwDisAmt; //*Arm 63-08-12
                            oSalDis.FTXddDisChgType = "3";
                            oSalDis.FCXddValue = decimal.Parse(otbDisAmount.Text);
                            break;
                        case 4:
                            //oSalDis.FTXddDisChgTxt = otbDisAmount.Text + "%";
                            oSalDis.FTXddDisChgTxt = tShwDisAmt + "%"; //*Arm 63-08-12
                            oSalDis.FTXddDisChgType = "4";
                            oSalDis.FCXddValue = decimal.Parse(otbAfDis.Tag.ToString());
                            break;
                    }

                    //new cSale().C_PRCxDisChgItem(oSalDis);

                    //Arm 63-07-15 
                    decimal cNet = 0;
                    cNet = oDiscPolicy.C_GETcGetAmount(1, cSale.nC_DTSeqNo);
                    if (nW_DisType == 1 || nW_DisType == 2)
                    {
                        if (oSalDis.FCXddValue > cNet) //ถ้าส่วนลด มากกว่า Net ไม่ให้ทำ
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgAmtDisAlw"), 3);
                            return;
                        }
                        else
                        {
                            new cSale().C_PRCxDisChgItem(oSalDis);
                        }
                    }
                    else
                    {
                        new cSale().C_PRCxDisChgItem(oSalDis);
                    }
                    //++++++++++++
                }
                else
                {
                   
                    // ท้ายบิล
                    switch (nW_DisType)
                    {
                        case 1:
                            cW_DisChg = decimal.Parse(otbDisAmount.Text);
                            //tW_DisChgTxt = otbDisAmount.Text;
                            tW_DisChgTxt = tShwDisAmt; //*Arm 63-08-12
                            cW_Amt = decimal.Parse(otbDisAmount.Text);
                            break;
                        case 2:
                            cW_DisChg = decimal.Parse(otbDisAmount.Text);
                            //tW_DisChgTxt = otbDisAmount.Text + "%";
                            tW_DisChgTxt = tShwDisAmt + "%"; //*Arm 63-08-12
                            cW_Amt = decimal.Parse(otbAfDis.Tag.ToString());
                            break;
                        case 3:
                            cW_DisChg = decimal.Parse(otbDisAmount.Text);
                            //tW_DisChgTxt = "+" + otbDisAmount.Text;
                            tW_DisChgTxt = "+" + tShwDisAmt; //*Arm 63-08-12
                            cW_Amt = decimal.Parse(otbDisAmount.Text);
                            break;
                        case 4:
                            cW_DisChg = decimal.Parse(otbDisAmount.Text);
                            //tW_DisChgTxt = "+" + otbDisAmount.Text + "%";
                            tW_DisChgTxt = "+" + tShwDisAmt + "%"; //*Arm 63-08-12
                            cW_Amt = decimal.Parse(otbAfDis.Tag.ToString());
                            break;
                    }


                    //*Em 62-10-10
                    if (nW_DisType == 1 || nW_DisType == 2)
                    {
                        //*Arm 63-07-18 Check มูลค่าส่วนลด ห้ามมากกว่ามูลค่าที่สามารถลดได้
                        decimal cAmtDis = 0;
                        if (nW_StaDisChg==3)
                        {
                            //ลดท้ายบิล ตามรายการ
                            cAmtDis = oDiscPolicy.C_GETcGetAmtAlwDisc(3, "2", cSale.nC_DTSeqNo);
                        }
                        else
                        {
                            //ลดท้ายบิล
                            cAmtDis = oDiscPolicy.C_GETcGetAmtAlwDisc(2, "2");
                        }
                        
                        if (cW_Amt > cAmtDis)
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgAmtDisAlw"), 3);
                            return;
                        }
                        //+++++++++++++

                        if ((decimal)(cVB.oVB_Payment.cW_AmtTotalCal- cW_Amt) < (decimal)cVB.cVB_SmallBill)
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                            return;
                        }
                    }
                    //+++++++++++++++++
                    
                    //cPayment.C_ADDxDisChgBill(cW_Amt, cW_DisChg, tW_DisChgTxt, cW_B4DisChg, nW_DisType);
                    cPayment.C_ADDxDisChgBill(cW_Amt, cW_DisChg, tW_DisChgTxt, cW_B4DisChg, nW_DisType, "", aoW_ProDT, nW_StaDisChg, cVB.oVB_Reason.FTRsnCode); //*Arm 63-06-11 เพิ่ม nW_StaDisChg

                    cVB.oVB_Payment.W_ADDxDisChgBill(nW_DisType.ToString(), tW_DisChgTxt, cW_Amt);  //*Em 62-10-08
                    //*Net 63-07-31 ปรับตาม Moshi
                    //if(cVB.oVB_2ndScreen != null)
                    //cVB.oVB_2ndScreen.W_ADDxDisChgBill(nW_DisType.ToString(), tW_DisChgTxt, cW_Amt);  //*Zen 63-02-01
                }
                this.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "ocmAccept_Click : " + oEx.Message);
            }
            finally
            {
                oDiscPolicy = null;
                //cVB.oVB_Reason = null;
            }
        }

        private void wDiscount_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                oDis = null;
                //new cSP().SP_CLExMemory();
                this.Dispose();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "wDiscount_FormClosing : " + oEx.Message);
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
                new cLog().C_WRTxLog("wDiscount", "ocmBack_Click : " + oEx.Message);
            }
        }

        private void wDiscount_Shown(object sender, EventArgs e)
        {
            StringBuilder oSql = new StringBuilder();
            decimal cAmount = 0;
            try
            {
                otbDisAmount.Text = new cSP().SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);

                //if (nW_StaDisChg == 1)
                //{
                //    otbB4Dis.Text = new cSP().SP_SETtDecShwSve(1, cVB.oVB_PdtOrder.cSetPrice, cVB.nVB_DecShow);
                //    otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, cVB.oVB_PdtOrder.cSetPrice, cVB.nVB_DecShow);
                //    otbDisAmount.Focus();
                //    //*Arm 63-03-04 
                //    otbDisAmount.SelectionStart = 0;
                //    otbDisAmount.SelectAll();
                //    //+++++++++++++
                //}
                //else
                //{
                //    oSql.Clear();
                //    //oSql.AppendLine("SELECT ISNULL(FCXshTotalB4DisChgNV + FCXshTotalB4DisChgV,0.00) AS 'Amount'");
                //    oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");  //*Em 63-01-08
                //    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                //    oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                //    cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());
                //    otbB4Dis.Text = new cSP().SP_SETtDecShwSve(1, cAmount, cVB.nVB_DecShow);
                //    otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, cAmount, cVB.nVB_DecShow);
                //    otbDisAmount.Focus();
                //    //*Arm 63-03-04 
                //    otbDisAmount.SelectionStart = 0;
                //    otbDisAmount.SelectAll();
                //    //+++++++++++++
                //}


                // *Arm 63-06-08  nW_StaDisChg 1:ส่วนลดรายการ, 2:ส่วนลดท้ายบิล, 3:ส่วนลดท้ายบิล(รายการ)
                switch (nW_StaDisChg)
                {
                    case 1:
                    case 3:
                        otbB4Dis.Text = new cSP().SP_SETtDecShwSve(1, cVB.oVB_PdtOrder.cSetPrice, cVB.nVB_DecShow);
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, cVB.oVB_PdtOrder.cSetPrice, cVB.nVB_DecShow);
                        otbDisAmount.Focus();
                        otbDisAmount.SelectionStart = 0;
                        otbDisAmount.SelectAll();
                        break;

                    case 2:
                        oSql.Clear();
                        // *Arm 63-07-18 Comment Code
                        //oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");  //*Em 63-01-08
                        //oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                        //oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                        //cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());
                        //otbB4Dis.Text = new cSP().SP_SETtDecShwSve(1, cAmount, cVB.nVB_DecShow);
                        //otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, cAmount, cVB.nVB_DecShow);
                        otbB4Dis.Text = new cSP().SP_SETtDecShwSve(1, cW_B4DisChg, cVB.nVB_DecShow); // *Arm 63-07-18
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, cW_B4DisChg, cVB.nVB_DecShow); // *Arm 63-07-18
                        otbDisAmount.Focus();
                        otbDisAmount.SelectionStart = 0;
                        otbDisAmount.SelectAll();

                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "wDiscount_Shown : " + oEx.Message);
            }
        }

        private void otbDisAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                switch (nW_DisType)
                {
                    case 1:
                        W_PRCxDisAmount();
                        break;
                    case 2:
                        W_PRCxDisPercen();
                        break;
                    case 3:
                        W_PRCxChartsAmount();
                        break;
                    case 4:
                        W_PRCxChartsPercen();
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "otbDisAmount_TextChanged : " + oEx.Message);
            }
        }

        /// <summary>
        /// ลดราคาแบบระบุจำนวน
        /// </summary>
        private void W_PRCxDisAmount()
        {
            decimal cDis;
            decimal cNewDiscount;
            try
            {
                if (decimal.TryParse(otbDisAmount.Text, out cDis))
                {
                    //cNewDiscount = decimal.Parse(otbB4Dis.Text) - cDis;
                    cNewDiscount = Convert.ToDecimal(String.IsNullOrEmpty(otbB4Dis.Text) ? "0" : otbB4Dis.Text) - cDis; //*Net 63-07-31
                    if (cNewDiscount < 0)
                    {
                        W_PRCxMessageBox("tDisLess");
                        otbDisAmount.Text = new cSP().SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                        //otbAfDis.Text = otbB4Dis.Text;
                        //*Net 63-04-01 ยกมาจาก baseline
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbB4Dis.Text), cVB.nVB_DecShow);   //*Em 63-03-02
                        oucNumpad.tU_TextValue = "";    //*Em 62-10-08
                    }
                    else
                    {
                        //otbAfDis.Text = cNewDiscount.ToString();
                        //*Net 63-04-01 ยกมาจาก baseline
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, cNewDiscount, cVB.nVB_DecShow);   //*Em 63-03-02
                    }
                }
                else
                {
                    if (otbDisAmount.Text != string.Empty)
                    {
                        W_PRCxMessageBox("tIsNumberOnly");
                        otbDisAmount.Clear();
                        //otbAfDis.Text = otbB4Dis.Text;
                        //*Net 63-04-01 ยกมาจาก baseline
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbB4Dis.Text), cVB.nVB_DecShow);   //*Em 63-03-02
                        //oucNumpad.oU_TextValue.Clear();
                        //oucNumpad.tU_TextValue = string.Empty;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "W_PRCxDisAmount : " + oEx.Message);
            }
        }

        /// <summary>
        /// ลดราคาแบบระบุเปอร์เซ็น
        /// </summary>
        private void W_PRCxDisPercen()
        {
            decimal cDis;
            decimal cNewDiscount;
            try
            {
                if (decimal.TryParse(otbDisAmount.Text, out cDis))
                {
                    //*Em 62-10-10
                    //if (Convert.ToDecimal(otbDisAmount.Text) > (decimal)100)
                    if (Convert.ToDecimal(String.IsNullOrEmpty(otbDisAmount.Text) ? "0" : otbDisAmount.Text) > 100m) //*Net 63-07-31
                    {
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgNoPercen"), 3);
                        otbDisAmount.Text = new cSP().SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                        //otbAfDis.Text = otbB4Dis.Text;
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbB4Dis.Text), cVB.nVB_DecShow);   //*Em 63-03-02
                        oucNumpad.tU_TextValue = "";
                        return;
                    }
                    //+++++++++++++

                    //cNewDiscount = (decimal.Parse(otbB4Dis.Text) * cDis) / 100;
                    cNewDiscount = (decimal.Parse(String.IsNullOrEmpty(otbB4Dis.Text) ? "0" : otbB4Dis.Text) * cDis) / 100; //*Net 63-07-06
                    otbAfDis.Tag = cNewDiscount;
                    //cNewDiscount = decimal.Parse(otbB4Dis.Text) - cNewDiscount;
                    cNewDiscount = decimal.Parse(String.IsNullOrEmpty(otbB4Dis.Text) ? "0" : otbB4Dis.Text) - cNewDiscount; //*Net 63-07-06
                    if (cNewDiscount < 0)
                    {
                        W_PRCxMessageBox("tDisLess");
                        otbDisAmount.Text = new cSP().SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                        //otbAfDis.Text = otbB4Dis.Text;
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbB4Dis.Text), cVB.nVB_DecShow);   //*Em 63-03-02
                        oucNumpad.tU_TextValue = "";    //*Em 62-10-10
                    }
                    else
                    {
                        //otbAfDis.Text = cNewDiscount.ToString();
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, cNewDiscount, cVB.nVB_DecShow);   //*Em 63-03-02
                    }
                }
                else
                {
                    if (otbDisAmount.Text != string.Empty)
                    {
                        W_PRCxMessageBox("tIsNumberOnly");
                        otbDisAmount.Clear();
                        //otbAfDis.Text = otbB4Dis.Text;
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbB4Dis.Text), cVB.nVB_DecShow);   //*Em 63-03-02
                        //oucNumpad.oU_TextValue.Clear();
                        //oucNumpad.tU_TextValue = string.Empty;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "W_PRCxDisPercen : " + oEx.Message);
            }
        }

        /// <summary>
        /// ชาทร์ราคาแบบระบุจำนวน
        /// </summary>
        private void W_PRCxChartsAmount()
        {
            decimal cCharts;
            decimal cNewDiscount;
            try
            {
                if (decimal.TryParse(otbDisAmount.Text, out cCharts))
                {
                    //cNewDiscount = decimal.Parse(otbB4Dis.Text) + cCharts;
                    cNewDiscount = decimal.Parse(String.IsNullOrEmpty(otbB4Dis.Text) ? "0" : otbB4Dis.Text) + cCharts; //*Net 63-07-31
                    //otbAfDis.Text = cNewDiscount.ToString();
                    otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, cNewDiscount, cVB.nVB_DecShow);   //*Em 63-03-02
                }
                else
                {
                    if (otbDisAmount.Text != string.Empty)
                    {
                        W_PRCxMessageBox("tIsNumberOnly");
                        otbDisAmount.Clear();
                        //otbAfDis.Text = otbB4Dis.Text;
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbB4Dis.Text), cVB.nVB_DecShow);   //*Em 63-03-02
                        //oucNumpad.oU_TextValue.Clear();
                        //oucNumpad.tU_TextValue = string.Empty;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "W_PRCxChartsAmount : " + oEx.Message);
            }
        }

        /// <summary>
        /// ชาทร์ราคาแบบระบุเปอร์เซ็น
        /// </summary>
        private void W_PRCxChartsPercen()
        {
            decimal cCharts;
            decimal cNewDiscount;
            try
            {
                if (decimal.TryParse(otbDisAmount.Text, out cCharts))
                {
                    //*Em 62-10-10
                    //if (Convert.ToDecimal(otbDisAmount.Text) > (decimal)100)
                    if (Convert.ToDecimal(String.IsNullOrEmpty(otbDisAmount.Text) ? "0" : otbDisAmount.Text) > 100m) //*Net 63-07-06
                    {
                        new cSP().SP_SHWxMsg(oW_Resource.GetString("tMsgNoPercen"), 3);
                        otbDisAmount.Text = new cSP().SP_SETtDecShwSve(1, 0, cVB.nVB_DecShow);
                        //otbAfDis.Text = otbB4Dis.Text;
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbB4Dis.Text), cVB.nVB_DecShow);   //*Em 63-03-02
                        oucNumpad.tU_TextValue = "";
                        return;
                    }
                    //+++++++++++++

                    //cNewDiscount = (decimal.Parse(otbB4Dis.Text) * cCharts) / 100;
                    cNewDiscount = (decimal.Parse(String.IsNullOrEmpty(otbB4Dis.Text) ? "0" : otbB4Dis.Text) * cCharts) / 100; //*Net 63-07-06
                    otbAfDis.Tag = cNewDiscount;
                    //otbAfDis.Text = (decimal.Parse(otbB4Dis.Text) + cNewDiscount).ToString();
                    otbAfDis.Text = (decimal.Parse(String.IsNullOrEmpty(otbB4Dis.Text) ? "0" : otbB4Dis.Text) + cNewDiscount).ToString(); //*Net 63-07-06
                    otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbAfDis.Text), cVB.nVB_DecShow);   //*Em 63-03-02

                }
                else
                {
                    if (otbDisAmount.Text != string.Empty)
                    {
                        W_PRCxMessageBox("tIsNumberOnly");
                        otbDisAmount.Clear();
                        //otbAfDis.Text = otbB4Dis.Text;
                        otbAfDis.Text = new cSP().SP_SETtDecShwSve(1, Convert.ToDecimal(otbB4Dis.Text), cVB.nVB_DecShow);   //*Em 63-03-02
                        //oucNumpad.oU_TextValue.Clear();
                        //oucNumpad.tU_TextValue = string.Empty;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "W_PRCxChartsPercen : " + oEx.Message);
            }
        }

        /// <summary>
        /// Show MessageBox
        /// </summary>
        /// <param name="tMessage"></param>
        private void W_PRCxMessageBox(string tMessage)
        {
            string tMsg = string.Empty;
            try
            {
                tMsg = oW_Resource.GetString(tMessage);
                MessageBox.Show(tMsg, "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wDiscount", "W_PRCxMessageBox : " + oEx.Message);
            }
            finally
            {
                tMsg = null;
            }
        }

        private void ocmReason_Click(object sender, EventArgs e)
        {
            try
            {
                wReason oReason;
                //*Em 63-06-19
                if (nW_StaDisChg == 1)
                {
                    oReason = new wReason("011");
                }
                else
                {
                    oReason = new wReason("012");
                }
                //++++++++++++++++++++++

                oReason.ShowDialog();
                if (oReason.DialogResult == DialogResult.OK)
                {
                    otbReason.Text = cVB.oVB_Reason.FTRsnName;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDiscount", "ocmReason_Click : " + oEx.Message); }
            finally
            {

            }
        }

        public string W_GETtDiscode(int pnDistype)
        {
            string tDiscode = "";
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine("SELECT FTDisCode FROM TSysDisPolicy WITH (NOLOCK)");
                oSql.AppendLine("WHERE FNDisType=" + pnDistype + "");
                tDiscode = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wDiscount", "W_GETtDiscode : " + oEx.Message); }
            finally
            {

            }
            return tDiscode;
        }
    }
}
