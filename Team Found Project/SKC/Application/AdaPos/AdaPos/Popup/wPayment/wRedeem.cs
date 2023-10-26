using AdaPos.Class;
using AdaPos.Control;
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

namespace AdaPos.Popup.wPayment
{
    public partial class wRedeem : Form
    {
        #region Variable

        private cSP oW_SP;
        private ResourceManager oW_Resource;
        private int nW_Mode;            // Mode 1:Redeem Get Product, 2:Redeem Get Discount
        private int nW_LimitPerBill;    // จำนวนครั้งที่ใช้ได้ต่อบิล
        private int nW_SumQtyUsed;      
        private int nW_SumQtyAval;  
        private string tW_CalType;      // การคำนวน 1: ส่วนลด(Default)  2: เงินสด (ไม่ re-cal Vat)
        private string tW_DocNo;        //*Arm 63-04-07
        private int nW_SelectRow;       
        #endregion End Variable

        #region Constructor

        public wRedeem(int pnMode)
        {
            InitializeComponent();
            try
            {
                oW_SP = new cSP();
                nW_Mode = pnMode;
               
                W_SETxDesign();
                W_SETxText();
                
                olaAvailable.Text = oW_SP.SP_SETtDecShwSve(1, cVB.nVB_CstPoint, 0);
                olaBalance.Text = oW_SP.SP_SETtDecShwSve(1, cVB.nVB_CstPoint, 0);
                //W_GETxCalType(); 
                W_GETxLoadDataRedeem(nW_Mode);

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "wRedeem : " + oEx.Message);
            }
        }

        #endregion End Constructor


        #region Event
        
        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmBack_Click(object sender, EventArgs e)
        {
            try
            {
                W_GETxFuncByFuncName("C_KBDxBack");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "ocmBack_Click : " + oEx.Message);
            }
            finally
            {
                
            }
        }

        private void ocmShwKb_Click(object sender, EventArgs e)
        {
            try
            {
                new cFunctionKeyboard().C_KBDxKeyboard();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "ocmShwKb_Click : " + oEx.Message);
            }
        }

        /// <summary>
        /// Accept
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ocmAccept_Click(object sender, EventArgs e)
        {
            string tMsg;
            try
            {
                switch(nW_Mode)
                {
                    case 1:

                        if (Convert.ToDecimal(olaBalance.Text) < 0)
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdNotEnoughPoints"), 1);
                            return;
                        }

                        if (nW_LimitPerBill > 0)
                        {
                            if (Convert.ToDecimal(nW_SumQtyAval) < 0)
                            {
                                oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdLimitQty"), 1);
                                return;
                            }
                        }
                        break;

                    case 2:
                        //if (Convert.ToDecimal(ogdRedeem.Rows[nW_SelectRow].Cells[otbColUseMny.Name].Value)> cVB.oVB_Payment.cW_AmtTotalCal)
                        //{
                        //    tMsg = cVB.oVB_GBResource.GetString("tMsgPayMost");
                        //    tMsg += Environment.NewLine + cVB.oVB_GBResource.GetString("tMsgPayConfirm");
                        //    if (new cSP().SP_SHWoMsg(tMsg, 1) == DialogResult.No)
                        //    {
                        //        return;
                        //    } 
                        //}

                        break;
                }

                W_PRCxDiscount();
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "ocmAccept_Click : " + oEx.Message);
            }
            finally
            {

            }
        }

        /// <summary>
        /// check UC value Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void W_OTb_TextChanged(object sender, EventArgs e)
        {
            decimal cPoint;
            decimal cMoney;
            try
            {
                TextBox oTextbox = (TextBox)sender;
                int nRow = int.Parse(oTextbox.Tag.ToString());
                int nQtyInput = int.Parse(oTextbox.Text.ToString());

                // Qty
                ogdRedeem.Rows[nRow].Cells[otbColQty.Name].Value = nQtyInput.ToString();

                // คำนวนแต้มที่ต้องใช้
                cPoint = nQtyInput * Convert.ToDecimal(ogdRedeem.Rows[nRow].Cells[otbColSetPoint.Name].Value.ToString());
                ogdRedeem.Rows[nRow].Cells[otbColUsePoint.Name].Value = oW_SP.SP_SETtDecShwSve(1, cPoint, 0);

                // คำนวนเงินที่ต้องใช้
                cMoney = nQtyInput * Convert.ToDecimal(ogdRedeem.Rows[nRow].Cells[otbColSetMny.Name].Value.ToString());
                ogdRedeem.Rows[nRow].Cells[otbColUseMny.Name].Value = oW_SP.SP_SETtDecShwSve(1, cMoney, cVB.nVB_DecShow);


                // คำนวณแต้มคงเหลือ
                W_PRCxSumPoint();

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "W_OTb_TextChanged : " + oEx.Message);
            }
        }

        /// <summary>
        /// สั่งวาด UC ลงบน Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdRedeem_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                if (nW_Mode == 1)
                {
                    if (e.ColumnIndex == 1)
                    {
                        for (int i = 0; i < ogdRedeem.RowCount; i++)
                        {
                            uQty oQty = (uQty)ogdRedeem.Rows[i].Cells[otbColQty.Name].Tag;
                            Rectangle oCellRectangle = ogdRedeem.GetCellDisplayRectangle(5, i, true);
                            oQty.Location = new Point(oCellRectangle.X, oCellRectangle.Y);
                            if (oCellRectangle.IsEmpty)
                            {
                                oQty.Visible = false;
                            }
                            else
                            {
                                oQty.Visible = true;
                                oQty.Location = new Point(oCellRectangle.X, oCellRectangle.Y);
                                if (Convert.ToBoolean(ogdRedeem.Rows[i].Cells[ockChoose.Name].Value) == true)
                                {
                                    oQty.Enabled = true;
                                }
                                else
                                {
                                    oQty.Enabled = false;

                                }
                            }
                        }
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "ogdRedeem_CellPainting : " + oEx.Message);
            }
        }

        /// <summary>
        /// Event for Mode 1 : Redeem แต้ม+เงิน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdRedeem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (nW_Mode == 1)
                {
                    if (e.ColumnIndex == ogdRedeem.Columns[ockChoose.Name].Index)
                    {
                        if (Convert.ToBoolean(ogdRedeem.CurrentRow.Cells[ockChoose.Name].Value) == false)
                        {
                            ogdRedeem.CurrentRow.Cells[ockChoose.Name].Value = true;
                            uQty oQty = (uQty)ogdRedeem.CurrentRow.Cells[otbColQty.Name].Tag;
                            oQty.Enabled = true;
                        }
                        else
                        {
                            ogdRedeem.CurrentRow.Cells[ockChoose.Name].Value = false;
                            uQty oQty = (uQty)ogdRedeem.CurrentRow.Cells[otbColQty.Name].Tag;
                            oQty.Enabled = false;

                        }
                    }

                    //คำนวณแต้มคงเหลือ
                    W_PRCxSumPoint();
                }
                else
                {
                    return;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "ogdRedeem_CellContentClick : " + oEx.Message);
            }
        }

        /// <summary>
        /// Event for Mode 2 : Redeem ส่วนลด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ogdRedeem_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (nW_Mode == 2)
                {
                    if (e.ColumnIndex < 0) return;
                    if (e.RowIndex < 0) return;
                    nW_SelectRow = (int)e.RowIndex;

                    //คำนวณแต้มคงเหลือ
                    W_PRCxSumPoint();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "ogdRedeem_CellContentClick : " + oEx.Message);
            }
        }

        #endregion End Event


        #region Function/Method

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

                opnHDRedeem.BackColor = cVB.oVB_ColNormal; //*Net 63-03-28
                //opnBotRedeem.BackColor = cVB.oVB_ColNormal; //*Net 63-03-28
                

                // DataGridview
                oW_SP.SP_SETxSetGridviewFormat(ogdRedeem);
                
                switch (nW_Mode)
                {
                    case 1:     // 1:Redeem Get Product
                        ockChoose.Visible = true;
                        otbColRefCode.Visible = true;
                        otbColBarcode.Visible = true;
                        otbColPdtName.Visible = true;
                        otbColUnitName.Visible = true;
                        otbColQty.Visible = true;
                        otbColUsePoint.Visible = true;
                        otbColUseMny.Visible = true;
                        otbColMinTotBill.Visible = false;
                        break;

                    case 2:     // 2:Redeem Get Discount
                        ockChoose.Visible = false;
                        otbColRefCode.Visible = true; 
                        otbColBarcode.Visible = false;
                        otbColPdtName.Visible = false;
                        otbColUnitName.Visible = false;
                        otbColQty.Visible = false;
                        otbColUsePoint.Visible = true;
                        otbColUseMny.Visible = true;
                        otbColMinTotBill.Visible = true;

                        olaLimitQty.Visible = false;
                        break;
                }
                // End DataGridview

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "W_SETxDesign : " + oEx.Message);
            }
            finally
            {

            }
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

                olaCstName.Text = cVB.tVB_CstName;
                olaTitleAvai.Text = oW_Resource.GetString("tTitleRedeemAvailable");
                olaTitleBal.Text = oW_Resource.GetString("tTitleRedeemBalance");

                switch (nW_Mode)
                {
                    case 1:     // 1:Redeem Get Product
                        olaTitleRedeem.Text = oW_Resource.GetString("tTitleRedeemGetPdt");
                        ockChoose.HeaderText = oW_Resource.GetString("tRdColChoose");
                        otbColRefCode.HeaderText = oW_Resource.GetString("tRdColRefCode");
                        otbColBarcode.HeaderText = oW_Resource.GetString("tRdColBarcode");
                        otbColPdtName.HeaderText = oW_Resource.GetString("tRdColPdtName");
                        otbColUnitName.HeaderText = oW_Resource.GetString("tRdColUnitName");
                        otbColQty.HeaderText = oW_Resource.GetString("tRdColQty");
                        otbColUsePoint.HeaderText = oW_Resource.GetString("tRdColUsePoin");
                        otbColUseMny.HeaderText = oW_Resource.GetString("tRdColUseMny");

                        olaLimitQty.Text = oW_Resource.GetString("tTitleRedeemLimitQty");
                        break;

                    case 2:     // 2:Redeem Get Discount
                        olaTitleRedeem.Text = oW_Resource.GetString("tTitleRedeemGetDiscount");
                        
                        otbColRefCode.HeaderText = oW_Resource.GetString("tRdColRefCode");

                        otbColUsePoint.HeaderText = oW_Resource.GetString("tRdColUsePoinDis"); 
                        otbColUseMny.HeaderText = oW_Resource.GetString("tRdColUseMnyDis"); 
                        otbColMinTotBill.HeaderText = oW_Resource.GetString("tRdColMinTotBill"); 
                        break;
                }
            }   
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "W_SETxText : " + oEx.Message);
            }
            finally
            {

            }
        }


        /// <summary>
        /// Function Call By Name
        /// </summary>
        /// <param name="ptFuncName"></param>
        public void W_GETxFuncByFuncName(string ptFuncName)
        {
            try
            {
                switch (ptFuncName)
                {
                    case "C_KBDxBack":
                        this.Close();
                        break;

                    default:
                        new cFunctionKeyboard().C_PRCxCallByName(ptFuncName);
                        break;

                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "W_GETxFuncByFuncName : " + oEx.Message);
            }
            finally
            {
                ptFuncName = null;
            }

        }

        /// <summary>
        /// Load Item Redeem
        /// </summary>
        /// <param name="pnMode"></param>
        public void W_GETxLoadDataRedeem(int pnMode)
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlRedeemPointDetail> aoDetail;
            int nCountUsed=0;  // จำนวนครั้งที่ใช้ไปแล้ว ของ Redeem แต้มแลกส่วนลด
            decimal cAmount; //*Arm 63-04-03
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                //*Arm 63-04-07

                //เอกสารล่าสุด
                oSql.Clear();
                oSql.AppendLine(" SELECT TOP 1 FTRdhDocNo FROM TPSTRedeemHDTmp WHERE FTTmpDocType = '" + nW_Mode.ToString() + "' ORDER BY FDCreateOn DESC"); //*Arm 63-04-07
                tW_DocNo = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                
                //CalType
                oSql.Clear();
                oSql.AppendLine("Select TOP 1 FTTmpCalType From TPSTRedeemHDTmp WHERE FTRdhDocNo = '"+ tW_DocNo + "'"); //*Arm 63-04-07
                tW_CalType = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                if (tW_CalType == "1")
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT Count(*) FROM TSRC" + cVB.tVB_PosCode + " WITH(NOLOCK) WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    if (new cDatabase().C_GEToDataQuery<int>(oSql.ToString()) > 0)
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdDisAfterPay"), 1);
                        this.Close();
                        return;

                    }
                    else
                    {
                        //*Arm 63-04-03
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                        oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                        oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                        cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                        if (cAmount > 0)
                        {

                        }
                        else
                        {
                            // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdNoItem"), 1);

                            this.Close();
                            return;
                        }
                        //++++++++++++++
                    }
                }
                //++++++++++++++++

                switch (pnMode)
                {
                    case 1:     //ประเภทเอกสาร 1: Redeem แต้ม+เงิน 
                        if (!string.IsNullOrEmpty(tW_CalType))
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT DISTINCT");
                            oSql.AppendLine("	RdHDTmp.FTTmpRedeemCode AS tRefCode, RdHDTmp.FNTmpLimitPerBill AS nLimitPerBill, ISNULL(RdHDTmp.FCTmpUsePoint,0) AS cUsePoint,");
                            oSql.AppendLine("	ISNULL(RdHDTmp.FCTmpUseMny,0.00) AS cUseMny, RdHDTmp.FTTmpCalType AS tCalType,");
                            oSql.AppendLine("	DT.FTXsdBarCode AS tBarcode, DT.FTXsdPdtName AS tPdtName, DT.FTPunName AS tUnitName, ");
                            oSql.AppendLine("	DT.FNXsdSeqNo AS nSeqNo,ISNULL(DT.FCXsdQty,0) AS cQtyDT, ISNULL(DT.FCXsdSetPrice,0.00) AS cSetPrice,");
                            oSql.AppendLine("	( CASE ");
                            oSql.AppendLine("		WHEN RdHDTmp.FNTmpLimitPerBill = 0 THEN  DT.FCXsdQty");
                            oSql.AppendLine("		WHEN DT.FCXsdQty >= RdHDTmp.FNTmpLimitPerBill THEN RdHDTmp.FNTmpLimitPerBill");
                            oSql.AppendLine("		ELSE  DT.FCXsdQty");
                            oSql.AppendLine("	  END");
                            oSql.AppendLine("	) - ISNULL(RD.cQtyUsed,0) AS cQtyAval,");
                            oSql.AppendLine("	ISNULL(RD.cQtyUsed,0) AS cRdQtyUsed,");
                            oSql.AppendLine("	ISNULL(RD.nPntUsed, 0) AS cRdPntUsed");
                            oSql.AppendLine("FROM TPSTRedeemHDTmp RdHDTmp WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TSDT" + cVB.tVB_PosCode + " DT WITH(NOLOCK) ON RdHDTmp.FTTmpPdtCode = DT.FTPdtCode AND RdHDTmp.FTTmpPunCode = DT.FTPunCode ");
                            oSql.AppendLine("LEFT JOIN TSDTDis" + cVB.tVB_PosCode + " DTDis WITH(NOLOCK) ON DT.FNXsdSeqNo = DTDis.FNXsdSeqNo AND  DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FTBchCode = DTDis.FTBchCode");
                            oSql.AppendLine("LEFT JOIN TSRC" + cVB.tVB_PosCode + " RC WITH(NOLOCK) ON DT.FTXshDocNo = RC.FTXshDocNo AND DT.FTBchCode = RC.FTBchCode AND RC.FTRcvCode = '" + cPayment.tC_RcvCode + "'");

                            if (tW_CalType == "1")      //การคำนวน 1: ส่วนลด(Default)  2: เงินสด (ไม่ re-cal Vat)
                            {
                                oSql.AppendLine("LEFT JOIN (SELECT FTXshDocNo,FTBchCode,FNXrdRefSeq,FTXrdRefCode, SUM(FCXrdPdtQty)AS cQtyUsed, SUM(FNXrdPntUse) AS nPntUsed ");
                                oSql.AppendLine("			FROM TSRD" + cVB.tVB_PosCode + " WITH(NOLOCK) ");
                                oSql.AppendLine("			GROUP BY FTXshDocNo, FTBchCode, FNXrdRefSeq,FTXrdRefCode) RD ");
                                oSql.AppendLine("	ON DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FTBchCode = RD.FTBchCode ");
                                oSql.AppendLine("	AND DTDis.FNXsdSeqNo=RD.FNXrdRefSeq AND DTDis.FTXddRefCode=RD.FTXrdRefCode");
                            }

                            if (tW_CalType == "2")      //การคำนวน   2: เงินสด (ไม่ re-cal Vat)
                            {
                                oSql.AppendLine("LEFT JOIN (SELECT FTXshDocNo,FTBchCode,FNXrdRefSeq,FTXrdRefCode, SUM(FCXrdPdtQty)AS cQtyUsed, SUM(FNXrdPntUse) AS nPntUsed ");
                                oSql.AppendLine("			FROM TSRD" + cVB.tVB_PosCode + " WITH(NOLOCK) ");
                                oSql.AppendLine("			GROUP BY FTXshDocNo, FTBchCode, FNXrdRefSeq,FTXrdRefCode) RD ");
                                oSql.AppendLine("	ON RC.FTXshDocNo = RD.FTXshDocNo AND RC.FTBchCode = RD.FTBchCode ");
                                oSql.AppendLine("	AND DT.FNXsdSeqNo = RD.FNXrdRefSeq AND RC.FTXrcRefNo1=RD.FTXrdRefCode");
                            }

                            oSql.AppendLine("WHERE RdHDTmp.FTTmpDocType = '1' ");
                            oSql.AppendLine("AND RdHDTmp.FTRdhDocNo = '"+tW_DocNo+"' "); //*Arm 63-04-07
                            oSql.AppendLine("AND CONVERT(time(0), GETDATE(), 108) BETWEEN CONVERT(time(0), RdHDTmp.FDTmpTStart, 108) AND CONVERT(time(0), RdHDTmp.FDTmpTStop, 108) ");
                            oSql.AppendLine("AND RdHDTmp.FCTmpUsePoint <= '" + cVB.nVB_CstPoint + "'");
                            oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                            oSql.AppendLine("AND DT.FTXsdStaPdt = '1' ");
                            oSql.AppendLine("AND ((ISNULL(DTDis.FTXshDocNo,'') ='' AND ISNULL(DTDis.FTXddRefCode,'') ='') OR ISNULL(DTDis.FTXddRefCode,'') !='') ");
                            
                        }
                        aoDetail = oDB.C_GETaDataQuery<cmlRedeemPointDetail>(oSql.ToString());
                        

                        ogdRedeem.Rows.Clear();
                        if (aoDetail.Count >0)
                        {
                            nW_LimitPerBill = aoDetail[0].nLimitPerBill;

                            foreach (cmlRedeemPointDetail oRedeemPdt in aoDetail)
                            {
                                nW_SumQtyUsed += (int)oRedeemPdt.cRdQtyUsed;

                                if (oRedeemPdt.cQtyAval > 0)
                                {
                                    int nRow = ogdRedeem.Rows.Count;
                                    ogdRedeem.Rows.Add(
                                        false,
                                        oRedeemPdt.tRefCode,
                                        oRedeemPdt.tBarcode,
                                        oRedeemPdt.tPdtName,
                                        oRedeemPdt.tUnitName,
                                        oRedeemPdt.cQtyAval,
                                        oW_SP.SP_SETtDecShwSve(1, oRedeemPdt.cUsePoint * oRedeemPdt.cQtyAval, 0),
                                        oW_SP.SP_SETtDecShwSve(1, oRedeemPdt.cUseMny * oRedeemPdt.cQtyAval, cVB.nVB_DecShow),
                                        "",
                                        oRedeemPdt.cUsePoint,
                                        oRedeemPdt.cUseMny,
                                        oRedeemPdt.cQtyDT, //DT Max Qty
                                        oRedeemPdt.tCalType,
                                        oRedeemPdt.cSetPrice, // ราคา
                                        oRedeemPdt.nSeqNo
                                        );

                                    uQty oQty = new uQty(oW_SP.SP_SETtDecShwSve(1, oRedeemPdt.cQtyAval, 0), "1");
                                    ogdRedeem.Rows[nRow].Height = oQty.Height + 1;
                                    oQty.otbQty.Text = oW_SP.SP_SETtDecShwSve(1, oRedeemPdt.cQtyAval, 0);

                                    TextBox oTb = new TextBox();
                                    oTb.TextChanged += W_OTb_TextChanged;
                                    oQty.oU_Value = oTb;
                                    oTb.Tag = nRow;
                                    oQty.Visible = false;
                                    ogdRedeem.Rows[nRow].Cells[otbColQty.Name].Tag = oQty;
                                    this.ogdRedeem.Controls.Add(oQty);
                                }
                            }
                            
                        }
                        else
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdNoItem"), 1);
                            this.Close();
                        }

                        //if (nW_LimitPerBill == 0) // ไม่จำกัดรายการ
                        //{
                        //    olaLimitQty.Text = oW_Resource.GetString("tTitleRedeemNotLimitQty");
                        //}
                        //else
                        //{
                        //    if (nW_SumQtyUsed >= nW_LimitPerBill) //ใช้เกินรายการที่จำกัด
                        //    {
                        //        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdLimitQty"), 2);
                        //        this.Close();
                                
                                
                        //    }
                        //    nW_SumQtyAval = nW_LimitPerBill - nW_SumQtyUsed; //คงเหลือจำนวนที่ใช้ได้
                        //    olaLimitQty.Text = oW_Resource.GetString("tTitleRedeemLimitQty") + " : " + nW_LimitPerBill.ToString() + "   " + oW_Resource.GetString("tTitleRedeemQtyBalance") +" : "+ nW_SumQtyAval.ToString();
                        //}

                        break;

                    case 2: //ประเภทเอกสาร 2: Redeem ส่วนลด
                        oSql.Clear();
                        oSql.AppendLine("SELECT");
                        oSql.AppendLine("   RdHDTmp.FTTmpRedeemCode AS tRefCode, RdHDTmp.FNTmpLimitPerBill AS nLimitPerBill, ISNULL(RdHDTmp.FCTmpUsePoint, 0) AS cUsePoint");
                        oSql.AppendLine("   ,ISNULL(RdHDTmp.FCTmpUseMny, 0.00) AS cUseMny, RdHDTmp.FTTmpCalType AS tCalType, FCTmpMinTotBill AS cMinTotBill");
                        oSql.AppendLine("   ,ISNULL(RD.nCountUse, 0.00) AS nCountUsed");
                        oSql.AppendLine("FROM  TPSTRedeemHDTmp RdHDTmp WITH(NOLOCK)");
                        oSql.AppendLine("LEFT JOIN (SELECT FTXshDocNo,FTBchCode,FNXrdRefSeq,FTXrdRefCode, COUNT(*)AS nCountUse");
                        oSql.AppendLine("               FROM TSRD" + cVB.tVB_PosCode + " WITH(NOLOCK) WHERE FTRdhDocType = '2' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                        oSql.AppendLine("               GROUP BY FTXshDocNo, FTBchCode, FNXrdRefSeq,FTXrdRefCode) RD");
                        oSql.AppendLine("       ON RdHDTmp.FTTmpRedeemCode = RD.FTXrdRefCode ");
                        oSql.AppendLine("WHERE RdHDTmp.FTTmpDocType = '2'");
                        oSql.AppendLine("AND RdHDTmp.FTRdhDocNo = '" + tW_DocNo + "' "); //*Arm 63-04-07
                        oSql.AppendLine("AND CONVERT(time(0), GETDATE(), 108) BETWEEN CONVERT(time(0), RdHDTmp.FDTmpTStart, 108) AND CONVERT(time(0), RdHDTmp.FDTmpTStop, 108) ");
                        oSql.AppendLine("AND RdHDTmp.FCTmpUsePoint <= '" + cVB.nVB_CstPoint + "'");
                        oSql.AppendLine("AND RdHDTmp.FCTmpMinTotBill <= '" + cVB.oVB_Payment.cW_AmtTotalCal + "'"); 

                        aoDetail = oDB.C_GETaDataQuery<cmlRedeemPointDetail>(oSql.ToString());
                        
                        ogdRedeem.Rows.Clear();
                        if (aoDetail.Count > 0)
                        {
                            nW_LimitPerBill = aoDetail[0].nLimitPerBill;
                            foreach (cmlRedeemPointDetail oRedeemPdt in aoDetail)
                            {
                                nCountUsed += oRedeemPdt.nCountUsed; // จำนวนครั้งที่ใช้ไปแล้ว ของ Redeem แต้มแลกส่วนลด
                                
                                ogdRedeem.Rows.Add(
                                    false,
                                    oRedeemPdt.tRefCode,
                                    oRedeemPdt.tBarcode,
                                    oRedeemPdt.tPdtName,
                                    oRedeemPdt.tUnitName,
                                    "",
                                    oW_SP.SP_SETtDecShwSve(1, oRedeemPdt.cUsePoint, 0),
                                    oW_SP.SP_SETtDecShwSve(1, oRedeemPdt.cUseMny, cVB.nVB_DecShow),
                                    oW_SP.SP_SETtDecShwSve(1, oRedeemPdt.cMinTotBill, cVB.nVB_DecShow),
                                    oRedeemPdt.cUsePoint,
                                    oRedeemPdt.cUseMny,
                                    oRedeemPdt.cQtyDT, 
                                    oRedeemPdt.tCalType,
                                    oRedeemPdt.cSetPrice, // ราคา
                                    ""
                                    );
                                
                            }

                            //if (nCountUsed >= nW_LimitPerBill) //ใช้เกินรายการที่จำกัด
                            //{
                            //    oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdLimitQty"), 2);
                            //    this.Close();
                            //}

                            nW_SelectRow = -1;
                        }
                        else
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdNoItem"), 1);
                            this.Close();
                        }
                        break;
                }

                if (nW_LimitPerBill == 0) // ไม่จำกัดรายการ
                {
                    if (pnMode == 1)
                    {
                        olaLimitQty.Text = oW_Resource.GetString("tTitleRedeemNotLimitQty");
                    }
                }
                else
                {
                    if (pnMode == 1)
                    {
                        if (nW_SumQtyUsed >= nW_LimitPerBill) //ใช้เกินรายการที่จำกัด
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdLimitQty"), 3);
                            this.Close();
                        }
                        nW_SumQtyAval = nW_LimitPerBill - nW_SumQtyUsed; //คงเหลือจำนวนที่ใช้ได้
                        olaLimitQty.Text = oW_Resource.GetString("tTitleRedeemLimitQty") + " : " + nW_LimitPerBill.ToString() + "   " + oW_Resource.GetString("tTitleRedeemQtyBalance") + " : " + nW_SumQtyAval.ToString();
                    }

                    if (pnMode == 2)
                    {
                        if (nCountUsed >= nW_LimitPerBill) //ใช้เกินรายการที่จำกัด
                        {
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdLimitQty"), 3);
                            this.Close();
                        }
                    }
                }

            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "W_GETxRedeem : " + oEx.Message);
            }
            finally
            {
                aoDetail = null;
                oSql = null;
                oDB = null;
                
            }

        }

        /// <summary>
        /// คำนวณแต้มคงเหลือ
        /// </summary>
        public void W_PRCxSumPoint()
        {
            decimal cSumPoint = 0;
            decimal cUsePoint = 0;
            decimal cQty = 0;
            decimal cSumQty = 0;
            try
            {
                if (ogdRedeem.Rows.Count <= 0)
                {
                    return;
                }
                switch(nW_Mode)
                {
                    case 1:
                        foreach (DataGridViewRow oRow in ogdRedeem.Rows)
                        {
                            if (Convert.ToBoolean(oRow.Cells[ockChoose.Name].Value) == true)
                            {
                                // รวม Point ที่ต้องใช้
                                cUsePoint += Convert.ToDecimal(oRow.Cells[otbColUsePoint.Name].Value.ToString());
                                cQty += Convert.ToDecimal(oRow.Cells[otbColQty.Name].Value.ToString());
                            }
                        }
                        // จำนวนแต้มคงเหลือ
                        cSumPoint = Convert.ToDecimal(olaAvailable.Text) - cUsePoint;
                        olaBalance.Text = oW_SP.SP_SETtDecShwSve(1, cSumPoint, 0);

                        // จำนวนที่ใช้ได้คงเหลือ
                        if (nW_LimitPerBill > 0)
                        {
                            cSumQty = nW_LimitPerBill - (cQty + nW_SumQtyUsed);
                            nW_SumQtyAval = (int)cSumQty;

                            olaLimitQty.Text = oW_Resource.GetString("tTitleRedeemLimitQty") + " : " + nW_LimitPerBill.ToString() + "   " + oW_Resource.GetString("tTitleRedeemQtyBalance") + " : " + nW_SumQtyAval.ToString();
                        }

                        break;

                    case 2:
                        // จำนวนแต้มคงเหลือ
                        cSumPoint = Convert.ToDecimal(olaAvailable.Text) - Convert.ToDecimal(ogdRedeem.Rows[nW_SelectRow].Cells[otbColUsePoint.Name].Value.ToString());
                        olaBalance.Text = oW_SP.SP_SETtDecShwSve(1, cSumPoint, 0);
                        
                        break;
                }
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "W_PRCxSumPoint : " + oEx.Message);
            }
        }


        /// <summary>
        /// เช็คการคำนวน 1: ส่วนลด(Default)  2: เงินสด (ไม่ re-cal Vat)
        /// </summary>
        public void W_GETxCalType()
        {
            StringBuilder oSql;
            decimal cAmount; //*Arm 63-04-03
            try
            {
                
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT DISTINCT FTTmpCalType FROM TPSTRedeemHDTmp WITH(NOLOCK) WHERE FTTmpDocType = '" + nW_Mode + "'");
                oSql.AppendLine("Select TOP 1 FTTmpCalType From TPSTRedeemHDTmp WHERE FTRdhDocNo = (SELECT TOP 1 FTRdhDocNo FROM TPSTRedeemHDTmp WHERE FTTmpDocType = '" + nW_Mode.ToString() + "' ORDER BY FDCreateOn DESC)"); //*Arm 63-04-07
                tW_CalType = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine(" SELECT TOP 1 FTRdhDocNo FROM TPSTRedeemHDTmp WHERE FTTmpDocType = '" + nW_Mode.ToString() + "' ORDER BY FDCreateOn DESC"); //*Arm 63-04-07
                tW_DocNo = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                if (tW_CalType == "1")
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT Count(*) FROM TSRC" + cVB.tVB_PosCode + " WITH(NOLOCK) WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    if (new cDatabase().C_GEToDataQuery<int>(oSql.ToString()) > 0)
                    {
                        oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdDisAfterPay"), 1);
                        this.Close();
                        return;

                    }
                    else
                    {
                        //*Arm 63-04-03
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                        oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                        oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                        cAmount = new cDatabase().C_GEToDataQuery<decimal>(oSql.ToString());

                        if (cAmount > 0)
                        {
                            
                        }
                        else
                        {
                            // ไม่มียอดที่สามารถทำรายการลด/ชาต์ท ได้
                            oW_SP.SP_SHWxMsg(oW_Resource.GetString("tMsgRdNoItem"), 1);
                            
                            this.Close();
                            return;
                        }
                        //++++++++++++++
                    }
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "W_GETxCalType : " + oEx.Message);
            }
            finally
            {
                oSql = null;
            }
        }

        /// <summary>
        /// Process การใช้แต้ม
        /// </summary>
        public void W_PRCxDiscount()
        {
            StringBuilder oSql;
            cmlRdSalDTDis oSalDTDis;
            cmlRdSalRD oSalRD;
            List<cmlProrateDT> aoProDT; // Arm 63-03-23
            cDatabase oDB;
            decimal cQty = 0;       // จำนวนชิ้น / รายการ
            decimal cSetPrice = 0;  // ราคาต่อชิ้น
            decimal cSumPrice = 0;  // ราคารวม / สินค้า
            decimal cUseMny = 0;    // + ใช้เงิน
            decimal cUsePoint = 0;  // ใช้แต้ม
            decimal cDiscount = 0;  // ส่วนลด
            decimal cCash = 0;      // เงินสด
            decimal cB4Dis = 0;     // มูลค่าสุทธิก่อนลด

            string tRefCode = "";    // Redeem Code
            try
            {
                switch (nW_Mode)
                {
                    case 1:     // Redeem แต้ม+เงิน
                        if (ogdRedeem.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow oRow in ogdRedeem.Rows)
                            {
                                if (Convert.ToBoolean(oRow.Cells[ockChoose.Name].Value) == true)
                                {
                                    switch (oRow.Cells[otbColCalType.Name].Value.ToString()) // Check CalType 
                                    {
                                        case "1":   // CalType = การคำนวน 1: ส่วนลด(Default)

                                            // รหัสอ้างอิง Redeem 
                                            tRefCode = oRow.Cells[otbColRefCode.Name].Value.ToString();
                                            // ใช้แต้ม
                                            cUsePoint = Convert.ToDecimal(oRow.Cells[otbColUsePoint.Name].Value.ToString());

                                            // คำนวณส่วนลด
                                            cQty = Convert.ToDecimal(oRow.Cells[otbColQty.Name].Value.ToString());
                                            cSetPrice = Convert.ToDecimal(oRow.Cells[otbColSetPrice.Name].Value.ToString());
                                            cSumPrice = cQty * cSetPrice;

                                            cUseMny = Convert.ToDecimal(oRow.Cells[otbColUseMny.Name].Value.ToString());
                                            // ส่วนลด
                                            cDiscount = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSumPrice - cUseMny, cVB.nVB_DecShow));

                                            // มูลค่าสุทธิก่อนลด
                                            oSql = new StringBuilder();
                                            oDB = new cDatabase();
                                            oSql.AppendLine("SELECT CASE WHEN ISNULL(FCXsdNetAfHD,0.00) = 0.00 THEN ISNULL(FCXsdAmtB4DisChg,0.00) ELSE ISNULL(FCXsdNetAfHD,0.00) END AS cAmount"); 
                                            oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " WITH (NOLOCK)");
                                            oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                                            oSql.AppendLine("AND FNXsdSeqNo ='"+ oRow.Cells[otbColSeq.Name].Value + "'");
                                            cB4Dis = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
                                            
                                            // Set SeqNo 
                                            cSale.nC_DTSeqNo = (int)oRow.Cells[otbColSeq.Name].Value; // Seq
                                            cVB.oVB_OrderRowIndex = cSale.nC_DTSeqNo - 1; // Row sSale
                                                                                          //++++++++++++

                                            //oSalDTDis = new cmlRdSalDTDis();
                                            //oSalDTDis.FTBchCode = cVB.tVB_BchCode;
                                            //oSalDTDis.FTXshDocNo = cVB.tVB_DocNo;
                                            //oSalDTDis.FNXsdSeqNo = (int)oRow.Cells[otbColSeq.Name].Value;
                                            //oSalDTDis.FNXddStaDis = 2;
                                            //oSalDTDis.FTXddDisChgTxt = oW_SP.SP_SETtDecShwSve(1, cDiscount, cVB.nVB_DecShow).ToString();
                                            //oSalDTDis.FTXddDisChgType = "1";
                                            //oSalDTDis.FCXddNet = cB4Dis;
                                            //oSalDTDis.FCXddValue = cDiscount;
                                            //oSalDTDis.FTXddRefCode = oRow.Cells[otbColRefCode.Name].Value.ToString();
                                            ////****** SalRD ***********
                                            //oSalDTDis.FCXrdPdtQty = cQty;
                                            //oSalDTDis.FNXrdPntUse = (int)cUsePoint;
                                            //oSalDTDis.FTRdhDocType = "1";

                                            //new cSale().C_PRCxRedeemDiscountItem(oSalDTDis);
                                            
                                            //*Arm 63-03-23
                                            // Insert SaleRD
                                            aoProDT = new List<cmlProrateDT>();
                                            cmlProrateDT oProDT = new cmlProrateDT();
                                            oProDT.FNSeqNo = (int)oRow.Cells[otbColSeq.Name].Value;
                                            oProDT.FTBchCode = cVB.tVB_BchCode; ;
                                            oProDT.FTSalDocNo = cVB.tVB_DocNo;



                                            aoProDT.Add(oProDT);
                                            cPayment.C_ADDxDisChgBill(cDiscount, cDiscount, oW_SP.SP_SETtDecShwSve(1, cDiscount, cVB.nVB_DecShow).ToString(), cB4Dis, 1, tRefCode, aoProDT);
                                            
                                            oSalRD = new cmlRdSalRD();
                                            oSalRD.FTBchCode = cVB.tVB_BchCode;
                                            oSalRD.FTXshDocNo = cVB.tVB_DocNo;
                                            oSalRD.FTRdhDocType = "1";
                                            oSalRD.FNXrdRefSeq = (int)oRow.Cells[otbColSeq.Name].Value;
                                            oSalRD.FTXrdRefCode = tRefCode;
                                            oSalRD.FCXrdPdtQty = cQty;
                                            oSalRD.FNXrdPntUse = (int)cUsePoint;
                                            new cSale().C_PRCxInsertSalRD(oSalRD);

                                            // Show
                                            cVB.oVB_Payment.W_ADDxDisChgBill("8", oW_SP.SP_SETtDecShwSve(1, cDiscount, cVB.nVB_DecShow).ToString(), cDiscount);
                                            if (cVB.oVB_2ndScreen != null)
                                            {
                                                cVB.oVB_2ndScreen.W_ADDxDisChgBill("8", oW_SP.SP_SETtDecShwSve(1, cDiscount, cVB.nVB_DecShow).ToString(), cDiscount);

                                            }

                                            //+++++++++++++


                                            oSalDTDis = null;
                                            aoProDT = null;
                                            oProDT = null;
                                            tRefCode = "";
                                            break; // case "1"

                                        case "2":   //CalType = 2: เงินสด (ไม่ re-cal Vat)
                                            // RefCode
                                            tRefCode = oRow.Cells[otbColRefCode.Name].Value.ToString();

                                            //DT SeqNo
                                            int nSeq = (int)oRow.Cells[otbColSeq.Name].Value;

                                            // ใช้แต้ม
                                            cUsePoint = Convert.ToDecimal(oRow.Cells[otbColUsePoint.Name].Value.ToString());

                                            // คำนวณส่วนลด
                                            cQty = Convert.ToDecimal(oRow.Cells[otbColQty.Name].Value.ToString());
                                            cSetPrice = Convert.ToDecimal(oRow.Cells[otbColSetPrice.Name].Value.ToString());
                                            cSumPrice = cQty * cSetPrice;
                                            cUseMny = Convert.ToDecimal(oRow.Cells[otbColUseMny.Name].Value.ToString());
                                            
                                            // ส่วนลดเงินสด
                                            cCash = Convert.ToDecimal(oW_SP.SP_SETtDecShwSve(1, cSumPrice - cUseMny, cVB.nVB_DecShow)); ;

                                            if ((decimal)(cVB.oVB_Payment.cW_AmtTotalCal - cDiscount) < (decimal)cVB.cVB_SmallBill)
                                            {
                                                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                                                return;
                                            }
                                            
                                            cPayment.tC_XrcRef1 = tRefCode;
                                            cPayment.tC_XrcRef2 = cUsePoint.ToString() + "," + cUseMny.ToString();
                                            cVB.cVB_Amount = cCash;
                                            
                                            //insert RC
                                            cPayment.C_PRCxPaymentRedeem("1", cQty, (int)cUsePoint, nSeq);

                                            tRefCode = "";

                                            break; // case "2"

                                    }   // End switch CalType
                                }
                                
                            } //End foreach

                        }
                        break;
                        
                    case 2:     // Redeem ส่วนลด

                        if (nW_SelectRow < 0)
                        {
                            return;
                        }
                        else
                        {
                            switch (ogdRedeem.Rows[nW_SelectRow].Cells[otbColCalType.Name].Value.ToString()) // Check CalType : การคำนวน 1: ส่วนลด(Default)  2: เงินสด (ไม่ re-cal Vat)
                            {
                                case "1":   // CalType = การคำนวน 1: ส่วนลด(Default)
                                    // RefCode
                                    tRefCode = ogdRedeem.Rows[nW_SelectRow].Cells[otbColRefCode.Name].Value.ToString();

                                    // ใช้แต้ม
                                    cUsePoint = Convert.ToDecimal(ogdRedeem.Rows[nW_SelectRow].Cells[otbColUsePoint.Name].Value.ToString());

                                    // มูลค่าส่วนลด
                                    cDiscount = Convert.ToDecimal(ogdRedeem.Rows[nW_SelectRow].Cells[otbColUseMny.Name].Value.ToString());

                                    if ((decimal)(cVB.oVB_Payment.cW_AmtTotalCal - cDiscount) < (decimal)cVB.cVB_SmallBill)
                                    {
                                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgDisOver"), 3);
                                        return;
                                    }

                                    // มูลค่าสุทธิก่อนลดชาร์จ
                                    oSql = new StringBuilder();
                                    oDB = new cDatabase();
                                    oSql.Clear();
                                    oSql.AppendLine("SELECT ISNULL(FCXshTotalAfDisChgV + FCXshTotalAfDisChgNV,0.00) AS 'Amount'");
                                    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH (NOLOCK)");
                                    oSql.AppendLine("WHERE FTXshDocNO='" + cVB.tVB_DocNo + "'");
                                    cB4Dis = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                                    // คำนวณส่วนลดท้ายบิล
                                    cPayment.C_ADDxDisChgBill(cDiscount, cDiscount, oW_SP.SP_SETtDecShwSve(1, cDiscount, cVB.nVB_DecShow).ToString(), cB4Dis, 1, tRefCode);
                                    
                                    // Insert SaleRD
                                    oSalRD = new cmlRdSalRD();
                                    oSalRD.FTBchCode = cVB.tVB_BchCode;
                                    oSalRD.FTXshDocNo = cVB.tVB_DocNo;
                                    oSalRD.FTRdhDocType = "2";
                                    oSalRD.FTXrdRefCode = tRefCode;
                                    oSalRD.FCXrdPdtQty = 0; //*Arm 63-03-30
                                    oSalRD.FNXrdPntUse = (int)cUsePoint;
                                    new cSale().C_PRCxInsertSalRD(oSalRD);

                                    tRefCode = "";

                                    // Show
                                    cVB.oVB_Payment.W_ADDxDisChgBill("6", oW_SP.SP_SETtDecShwSve(1, cDiscount, cVB.nVB_DecShow).ToString(), cDiscount);
                                    if (cVB.oVB_2ndScreen != null)
                                    {
                                        cVB.oVB_2ndScreen.W_ADDxDisChgBill("6", oW_SP.SP_SETtDecShwSve(1, cDiscount, cVB.nVB_DecShow).ToString(), cDiscount);
                                    }
                                    
                                    break;

                                case "2":   //CalType = 2: เงินสด (ไม่ re-cal Vat)

                                    // RefCode
                                    tRefCode = ogdRedeem.Rows[nW_SelectRow].Cells[otbColRefCode.Name].Value.ToString();

                                    // ใช้แต้ม
                                    cUsePoint = Convert.ToDecimal(ogdRedeem.Rows[nW_SelectRow].Cells[otbColUsePoint.Name].Value.ToString());

                                    // มูลค่าส่วนลด
                                    cCash = Convert.ToDecimal(ogdRedeem.Rows[nW_SelectRow].Cells[otbColUseMny.Name].Value.ToString());

                                    if (cCash > cVB.oVB_Payment.cW_AmtTotalCal)
                                    {
                                        string tMsg = "";
                                        tMsg = cVB.oVB_GBResource.GetString("tMsgPayMost");
                                        tMsg += Environment.NewLine + cVB.oVB_GBResource.GetString("tMsgPayConfirm");
                                        if (new cSP().SP_SHWoMsg(tMsg, 1) == DialogResult.No)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            cCash = cVB.oVB_Payment.cW_AmtTotalCal;
                                        }
                                    }

                                    //insert RC
                                    cPayment.tC_XrcRef1 = tRefCode;
                                    cPayment.tC_XrcRef2 = cUsePoint.ToString() + "," + cCash.ToString();
                                    cVB.cVB_Amount = cCash;
                                    cPayment.C_PRCxPaymentRedeem("2", cQty, (int)cUsePoint);

                                    tRefCode = "";

                                    break;
                            }
                        }
                        
                        break;

                } // End switch nW_Mode


                // Point คงเหลือ
                decimal cBalancePoint = Convert.ToDecimal(olaBalance.Text);
                cVB.nVB_CstPoint = (int)cBalancePoint;
                
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("wRedeem", "W_PRCxDiscount : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                oSalDTDis = null;
                oSalRD = null;
                tRefCode = "";
                aoProDT = null;
                
            }
        }


        #endregion End Function/Method

        
    }
}
