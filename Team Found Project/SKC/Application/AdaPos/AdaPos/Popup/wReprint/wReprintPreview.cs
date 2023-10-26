using AdaPos.Class;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaPos.Popup.wReprint
{
    public partial class wReprintPreview : Form
    {
        public string tW_DocNo;
        private int nW_StartY = 0; //*Arm 63-05-03

        public wReprintPreview(string ptDocNo)
        {
            InitializeComponent();
            try
            {
                tW_DocNo = ptDocNo;
                //olaTitleDocNo.Text = tW_DocNo; //*Arm 62-10-21 - Comment Code ปรับ Standard SET ไว้ใน W_SETxText()
                
                W_SHWxPreview();
                W_SETxDesign();     //*Arm 62-10-21
                W_SETxText();       //*Arm 62-10-21

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "wReprintPreview : " + oEx.Message); }
        }

        #region Function

        /// <summary>
        /// Set tex form
        /// </summary>
        private void W_SETxDesign() //*Arm 62-10-21 - แก้ไข Design ส่วนหัวตามสีธีม
        {
            try
            {
                opnHD.BackColor = cVB.oVB_ColDark;
                ocmBack.BackColor = cVB.oVB_ColDark;
                ocmPrint.BackColor = cVB.oVB_ColDark;
                
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wSetColor", "W_SETxDesign : " + oEx.Message); }
        }

        /// <summary>
        /// Set tex form
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                olaTitleDocNo.Text = tW_DocNo;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "W_SETxText : " + oEx.Message); }
        }
        private void W_SHWxPreview()
        {
            try
            {

                //opnPreview.Refresh();
                //opnPreview.Paint += opnPreview_Paint;
                opnPreview1.Refresh();
                opnPreview1.Paint += opnPreview_Paint;

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "W_SHWxPreview : " + oEx.Message); }
        }

        private void W_DATxPrint()
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            try
            {
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();

                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                oDoc.PrintController = new StandardPrintController();
                oDoc.PrintPage += oDoc_PrintPage;
                oDoc.Print();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "W_DATxPrint : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                oSP.SP_CLExMemory();
            }
        }

        private void W_PRNxSlip(Graphics poGraphic)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine, tDeposit;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tAmt, tVat;
            decimal cChange = 0;
            string tDataA = "";
            string tDataB = "";
            string tCstTel = "";
            List<cmlTPSTSalDT> aoDT;
            List<cmlTPSTSalDTDis> aoDTDis;
            List<cmlTPSTSalPD> aoPD;    //*Em 63-03-29
            string tPrint = ""; //*Em 62-09-06
            string[] aRmk;
            Image oLogo;
            decimal cPntRcv = 0;
            cmlTPSTSalHDCst oHDCst; //*Arm 63-05-01
            decimal cTotalQty = 0; //*Arm 63-05-07
            string tMsg = "";   //*Arm 63-05-07
            List<cmlPrnSplipDTDis> aoPrnDTDis;
            try
            {
                nWidth = Convert.ToInt32(poGraphic.VisibleClipBounds.Width);
                tLine = "------------------------------------------------------------------------";

                if (cVB.nVB_PaperSize == 1)
                {
                    if (nWidth > 215)
                        nWidth = 215;
                }
                else
                {
                    if (nWidth > 280)
                        nWidth = 280;
                }

                oGraphic = poGraphic;
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };
                //*Em 62-10-29
                if (!string.IsNullOrEmpty(cVB.tVB_PathLogo))
                {
                    if (File.Exists(cVB.tVB_PathLogo))
                    {
                        oLogo = Image.FromFile(cVB.tVB_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle(nWidth - 200, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }
                //+++++++++++++++++
                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FCXshTotal,FCXshTotalNV,FCXshTotalNoDis,FCXshTotalB4DisChgV,FCXshTotalB4DisChgNV,FTXshDisChgTxt,");
                oSql.AppendLine("FCXshDis,FCXshChg,FCXshTotalAfDisChgV,FCXshTotalAfDisChgNV,FCXshRefAEAmt,FCXshAmtV,FCXshAmtNV,");
                oSql.AppendLine("FCXshVat,FCXshVatable,FCXshGrand,FDXshDocDate,FTPosCode,FTUsrCode,FTCstCode,FTXshRmk,FNXshDocPrint,FCXshRnd,FTXshRefExt,FNXshDocType,FTXshRefInt"); //*Arm 62-11-12 -เพิ่ม FNXshDocPrint, *Arm 63-05-01 FTXshRefExt FNXshDocType ,*Arm 63-05-07 FTXshRefInt
                oSql.AppendLine("FROM TPSTSalHD HD with(nolock)");
                oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + this.tW_DocNo + "'");
                cmlTPSTSalHD oHD = oDB.C_GEToDataQuery<cmlTPSTSalHD>(oSql.ToString());
                if (oHD == null) return;

                // Get ค่า ID เครื่อง จาก DB
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + oHD.FTUsrCode + " T: " + oHD.FTPosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 18;
                oGraphic.DrawString(Convert.ToDateTime(oHD.FDXshDocDate.Value).ToString("dd/MM/yyyy HH:mm") + " BNO:" + tW_DocNo,
                                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                //Print DT

                //++++++ *Arm 63-05-05  สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต +++++++
                if (cVB.nVB_StaSumPrn == 1) // 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT DT1.FTPdtCode, ");
                    oSql.AppendLine("DT1.FTXsdPdtName, ");
                    oSql.AppendLine("DT1.FTXsdBarCode, ");
                    oSql.AppendLine("SUM(DT1.FCXsdQty) AS FCXsdQty,");
                    oSql.AppendLine("DT1.FCXsdSetPrice,");
                    oSql.AppendLine("SUM(DT1.FCXsdNet) AS FCXsdNet,");
                    oSql.AppendLine("DT1.FTXsdVatType,");
                    oSql.AppendLine("SUM(DT1.FCXsdAmtB4DisChg) AS FCXsdAmtB4DisChg");
                    //oSql.AppendLine("SUM(DT1.FCXsdQtyAll) AS FCXsdQtyAll,");
                    //oSql.AppendLine("DT1.FCXsdSalePrice AS FCXsdSalePrice ");
                    oSql.AppendLine("FROM(SELECT(SELECT TOP 1 FNXsdSeqNo FROM TPSTSalDT WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FTPdtCode = DT.FTPdtCode AND  FTXsdBarCode = DT.FTXsdBarCode ORDER BY FNXsdSeqNo ASC) AS FNXsdSeqNo,");
                    oSql.AppendLine("        DT.FTPdtCode,");
                    oSql.AppendLine("        DT.FTXsdPdtName,");
                    oSql.AppendLine("        DT.FTXsdBarCode,");
                    oSql.AppendLine("        DT.FCXsdQty AS FCXsdQty,");
                    oSql.AppendLine("        DT.FCXsdSetPrice,");
                    oSql.AppendLine("        DT.FCXsdNet AS FCXsdNet,");
                    oSql.AppendLine("        DT.FTXsdVatType,");
                    oSql.AppendLine("        DT.FCXsdAmtB4DisChg AS FCXsdAmtB4DisChg");
                    //oSql.AppendLine("        ISNULL(PD.FCXsdQty, DT.FCXsdQty) AS FCXsdQtyAll,");
                    //oSql.AppendLine("        ISNULL(PD.FCXsdSetPrice, DT.FCXsdSetPrice) AS FCXsdSalePrice");
                    oSql.AppendLine("    FROM TPSTSalDT DT WITH(NOLOCK)");
                    //oSql.AppendLine("    LEFT JOIN(SELECT PD.FTBchCode, PD.FTXshDocNo, PD.FNXsdSeqNo, PD.FCXsdQty, PD.FCXsdSetPrice");
                    //oSql.AppendLine("                FROM TPSTSalPD PD WITH(NOLOCK)");
                    //oSql.AppendLine("                INNER JOIN(SELECT FTBchCode, FTXshDocNo, FNXsdSeqNo, MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                    //oSql.AppendLine("                            FROM TPSTSalPD WITH(NOLOCK)");
                    //oSql.AppendLine("                            WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_DocNo + "' AND FTXpdGetType = '4'");
                    //oSql.AppendLine("                            GROUP BY FTBchCode, FTXshDocNo, FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                    //oSql.AppendLine("                WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + tW_DocNo + "' AND PD.FTXpdGetType = '4') PD");
                    //oSql.AppendLine("        ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                    oSql.AppendLine("    WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("    AND DT.FTXshDocNo = '" + tW_DocNo + "' ");


                    if (cVB.bVB_AlwPrnVoid == false)
                    {
                        oSql.AppendLine(" AND DT.FTXsdStaPdt <> '4' ");
                    }

                    oSql.AppendLine(") AS DT1 ");
                    oSql.AppendLine("GROUP BY DT1.FNXsdSeqNo, DT1.FTPdtCode,DT1.FTXsdPdtName,DT1.FTXsdBarCode, ");
                    //oSql.AppendLine("DT1.FCXsdSetPrice,DT1.FTXsdVatType,DT1.FCXsdSalePrice ");
                    oSql.AppendLine("DT1.FCXsdSetPrice,DT1.FTXsdVatType ");
                    oSql.AppendLine("ORDER BY DT1.FNXsdSeqNo ASC ");

                    aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("SELECT (SELECT FTXsdBarCode FROM TPSTSalDT WHERE FTBchCode = DTDis.FTBchCode AND FTXshDocNo = DTDis.FTXshDocNo AND FNXsdSeqNo = DTDis.FNXsdSeqNo ) AS FTXsdBarCode,");
                    oSql.AppendLine("DTDis.FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    oSql.AppendLine("FROM TPSTSalDTDis DTDis");
                    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + tW_DocNo + "'");
                    oSql.AppendLine("AND FNXddStaDis = 1");
                    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
                    oSql.AppendLine("ORDER BY FDXddDateIns");
                    aoPrnDTDis = oDB.C_GETaDataQuery<cmlPrnSplipDTDis>(oSql.ToString());

                    if (aoDT != null)
                    {
                        foreach (cmlTPSTSalDT oDT in aoDT)
                        {
                            switch (cVB.nVB_ChkShowPdtBarCode)
                            {
                                case 1:
                                    nStartY += 18;
                                    oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    break;
                                case 2:
                                    nStartY += 18;
                                    oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    break;

                                default:
                                    nStartY += 18;
                                    oGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    break;
                            }

                            if (oDT.FTXsdVatType != null || oDT.FTXsdVatType == "1")
                            {
                                tVat = "V";
                            }
                            else
                            {
                                tVat = " ";
                            }
                            if (cVB.nVB_ChkShowPdtBarCode > 0)
                            {
                                nStartY += 18;
                                oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                oGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

                                //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
                                //{
                                //    nStartY += 18;
                                //    poGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

                                //    switch (cVB.nVB_ChkShowPdtBarCode)
                                //    {
                                //        case 1:
                                //            nStartY += 18;
                                //            poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //            break;
                                //        case 2:
                                //            nStartY += 18;
                                //            poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //            break;

                                //        default:
                                //            nStartY += 18;
                                //            poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //            break;
                                //    }

                                //    nStartY += 18;
                                //    poGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                //}
                                //else
                                //{
                                //    nStartY += 18;
                                //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                //}
                            }
                            else
                            {
                                if (oDT.FCXsdQty > 1)
                                {
                                    nStartY += 18;
                                    oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    oGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

                                    //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
                                    //{
                                    //    nStartY += 18;
                                    //    poGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

                                    //    switch (cVB.nVB_ChkShowPdtBarCode)
                                    //    {
                                    //        case 1:
                                    //            nStartY += 18;
                                    //            poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //            break;
                                    //        case 2:
                                    //            nStartY += 18;
                                    //            poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //            break;

                                    //        default:
                                    //            nStartY += 18;
                                    //            poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //            break;
                                    //    }

                                    //    nStartY += 18;
                                    //    poGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                    //}
                                    //else
                                    //{
                                    //    nStartY += 18;
                                    //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                    //}
                                }
                                else
                                {
                                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdSalePrice, cVB.nVB_DecShow);    //*Em 63-05-01
                                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    oGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                }
                            }

                            if (aoPrnDTDis != null)
                            {
                                decimal cAmt = (decimal)(oDT.FCXsdAmtB4DisChg);
                                decimal cDis = 0; //เก็บผลรวมส่วนลด
                                decimal cChg = 0; //เก็บผลรวมชาจน์
                                int nRow = 0;
                                foreach (cmlPrnSplipDTDis oDTDis in aoPrnDTDis.Where(c => c.FTXsdBarCode == oDT.FTXsdBarCode))
                                {
                                    switch (oDTDis.FTXddDisChgType)
                                    {
                                        case "1":
                                        case "2":
                                            cDis += (decimal)oDTDis.FCXddValue;
                                            break;
                                        case "3":
                                        case "4":
                                            cChg += (decimal)oDTDis.FCXddValue;
                                            break;
                                    }
                                    nRow++;
                                }

                                if (nRow > 0)   //มี Transaction ส่วนลดรายการ
                                {
                                    if (cDis > 0)     // แสดง แสดงส่วนลด
                                    {
                                        nStartY += 18;
                                        oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tDis") + " (" + cDis + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                        cAmt = (cAmt - cDis);
                                        tAmt = oSP.SP_SETtDecShwSve(1, cAmt, cVB.nVB_DecShow);
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    }

                                    if (cChg > 0)   // แสดงชาจน์
                                    {
                                        nStartY += 18;
                                        oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tChg") + " (" + cChg + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                        cAmt = (cAmt + cChg);
                                        tAmt = oSP.SP_SETtDecShwSve(1, cAmt, cVB.nVB_DecShow);
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    }
                                }//End  Transaction ส่วนลดรายการ
                            }//End if (aoDTDis != null)
                        }//End foreach (cmlTPSTSalDT oDT in aoDT)
                    }//End if (aoDT != null)

                }//End 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์ *Arm 63-05-05 

                else
                {
                    // 2:ไม่อนุญาตให้รวมรายการสินค้าตอนพิมพ์ *Arm 63-05-05 

                    oSql = new StringBuilder();
                    //oSql.AppendLine("SELECT FNXsdSeqNo,FTPdtCode,FTXsdPdtName,FTXsdBarCode,FCXsdQty,FCXsdSetPrice,FCXsdNet,FTXsdVatType");
                    //oSql.AppendLine("FROM TPSTSalDT with(nolock)");
                    //oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + tW_DocNo + "'");
                    //oSql.AppendLine("ORDER BY FNXsdSeqNo");

                    //*Em 63-04-30
                    oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FCXsdQty,");
                    oSql.AppendLine("DT.FTPdtCode,DT.FTXsdBarCode,");
                    oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdNet ,DT.FTXsdVatType,DT.FCXsdAmtB4DisChg");
                    //oSql.AppendLine("ISNULL(PD.FCXsdQty,DT.FCXsdQty) AS FCXsdQtyAll,ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice) AS FCXsdSalePrice");
                    oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                    //oSql.AppendLine("LEFT JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdQty,PD.FCXsdSetPrice");
                    //oSql.AppendLine("   FROM TPSTSalPD PD WITH(NOLOCK)");
                    //oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                    //oSql.AppendLine("   	FROM TPSTSalPD WITH(NOLOCK)");
                    //oSql.AppendLine("   	WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_DocNo + "' AND FTXpdGetType = '4'");
                    //oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                    //oSql.AppendLine("   WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + tW_DocNo + "' AND PD.FTXpdGetType = '4') PD");
                    //oSql.AppendLine("   ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                    oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DT.FTXshDocNo = '" + tW_DocNo + "'");
                    //++++++++++++++++++
                    aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    oSql.AppendLine("FROM TPSTSalDTDis");
                    oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + tW_DocNo + "'");
                    oSql.AppendLine("AND FNXddStaDis = 1");
                    oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");
                    aoDTDis = oDB.C_GETaDataQuery<cmlTPSTSalDTDis>(oSql.ToString());

                    if (aoDT != null)
                    {
                        foreach (cmlTPSTSalDT oDT in aoDT)
                        {
                            //nStartY += 15;
                            //oGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);

                            //*Em 63-04-30
                            switch (cVB.nVB_ChkShowPdtBarCode)
                            {
                                case 1:
                                    nStartY += 18;
                                    oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    break;
                                case 2:
                                    nStartY += 18;
                                    oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    break;

                                default:
                                    nStartY += 18;
                                    oGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    break;
                            }
                            //++++++++++++++++++

                            if (oDT.FTXsdVatType.ToString() == "1")
                            {
                                tVat = "V";
                            }
                            else
                            {
                                tVat = " ";
                            }

                            if (cVB.nVB_ChkShowPdtBarCode > 0)
                            {
                                nStartY += 18;
                                oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                oGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

                                ////*Em 63-04-29
                                //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
                                //{
                                //    nStartY += 18;
                                //    poGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

                                //    switch (cVB.nVB_ChkShowPdtBarCode)
                                //    {
                                //        case 1:
                                //            nStartY += 18;
                                //            poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //            break;
                                //        case 2:
                                //            nStartY += 18;
                                //            poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //            break;

                                //        default:
                                //            nStartY += 18;
                                //            poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //            break;
                                //    }

                                //    nStartY += 18;
                                //    poGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                //}
                                //else
                                //{
                                //    nStartY += 18;
                                //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                //}
                                //++++++++++++++++
                            }
                            else
                            {
                                if (oDT.FCXsdQty > 1)
                                {
                                    nStartY += 18;
                                    oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    oGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

                                    //nStartY += 15;
                                    //oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdNet, cVB.nVB_DecShow);
                                    //oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
                                    //poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 15), oFormatCenter);

                                    ////*Em 63-04-29
                                    //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
                                    //{
                                    //    nStartY += 18;
                                    //    poGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nStartY - 150, nStartY, 10, 18), oFormatCenter);

                                    //    switch (cVB.nVB_ChkShowPdtBarCode)
                                    //    {
                                    //        case 1:
                                    //            nStartY += 18;
                                    //            poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //            break;
                                    //        case 2:
                                    //            nStartY += 18;
                                    //            poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //            break;

                                    //        default:
                                    //            nStartY += 18;
                                    //            poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //            break;
                                    //    }

                                    //    nStartY += 18;
                                    //    poGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                    //}
                                    //else
                                    //{
                                    //    nStartY += 18;
                                    //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                                    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
                                    //}
                                    ////++++++++++++++++
                                }
                                else
                                {
                                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdSalePrice, cVB.nVB_DecShow);    //*Em 63-05-01
                                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
                                    oGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 15), oFormatCenter);
                                }
                            }

                            if (aoDTDis != null)
                            {
                                foreach (cmlTPSTSalDTDis oDTDis in aoDTDis.Where(c => c.FNXsdSeqNo == oDT.FNXsdSeqNo))
                                {
                                    //*Arm 63-05-05 Comment Code
                                    //nStartY += 15;
                                    //oGraphic.DrawString("  " + oDTDis.FTXddDisChgTxt, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDTDis.FCXddNet - oDTDis.FCXddValue), cVB.nVB_DecShow);
                                    //oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);

                                    //*Arm 63-05-05 แก้ไขให้แสดงลด/ชาจน์
                                    nStartY += 18;
                                    switch (oDTDis.FTXddDisChgType)
                                    {
                                        case "1":
                                        case "2":
                                            oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tDis") + " (" + oDTDis.FTXddDisChgTxt + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDTDis.FCXddNet - oDTDis.FCXddValue), cVB.nVB_DecShow);
                                            oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                            break;
                                        case "3":
                                        case "4":
                                            oGraphic.DrawString(" " + cVB.oVB_GBResource.GetString("tChg") + " (" + oDTDis.FTXddDisChgTxt + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, nStartY);
                                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDTDis.FCXddNet + oDTDis.FCXddValue), cVB.nVB_DecShow);
                                            oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                            break;
                                    }
                                    //+++++++++++++
                                }
                            }
                        }
                    }
                }
                //*Em 63-03-29 ส่วนลดโปรโมชั่น
                oSql.Clear();
                oSql.AppendLine("SELECT (CASE WHEN ISNULL(HDL.FTPmhNameSlip,'') = '' THEN HDL.FTPmhName ELSE HDL.FTPmhNameSlip END) AS FTPmdGrpName,PD.FCXpdDis");
                oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPMTPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo");
                oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + tW_DocNo + "'");
                oSql.AppendLine("AND PD.FTXpdCpnText = '' AND PD.FTCpdBarCpn = ''");
                oSql.AppendLine("AND PD.FCXpdDis <> 0 ");   //*Em 63-04-12
                oSql.AppendLine("AND PD.FTXpdGetType <> '4' "); //*Em 63-04-29
                oSql.AppendLine("GROUP BY HDL.FTPmhNameSlip,HDL.FTPmhName,PD.FCXpdDis");

                aoPD = oDB.C_GETaDataQuery<cmlTPSTSalPD>(oSql.ToString());
                if (aoPD != null && aoPD.Count > 0)
                {
                    foreach (cmlTPSTSalPD oPD in aoPD)
                    {
                        nStartY += 18;
                        poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " " + oPD.FTPmdGrpName, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth - 100, 18));

                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oPD.FCXpdDis, cVB.nVB_DecShow);
                        poGraphic.DrawString("-" + tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(nWidth - 100, nStartY, 90, 18), oFormatFar);
                    }
                }
                //+++++++++++++++++++++++++
                nStartY += 30;

                //Total

                if (oHD != null)
                {
                    if (oHD.FCXshDis != 0 || oHD.FCXshChg != 0 || oHD.FCXshRnd != 0)
                    {
                        oGraphic.DrawString("Subtotal", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshTotal, cVB.nVB_DecShow);
                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
                        nStartY += 18;

                        //*Arm 63-05-01 - Print HDDis
                        //==========================================
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                        oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt, FTXhdRefCode "); //*Arm 63-04-16
                        oSql.AppendLine("FROM TPSTSalHDDis WITH(NOLOCK) ");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                        oSql.AppendLine("AND FTXshDocNo = '" + tW_DocNo + "'  ");
                        List<cmlTPSTSalHDDis> aoHDDis = oDB.C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());

                        if (aoHDDis.Count > 0)
                        {
                            foreach (cmlTPSTSalHDDis oHDDis in aoHDDis)
                            {
                                if (string.IsNullOrEmpty(oHDDis.FTXhdRefCode))
                                {
                                    //ส่วนลด 
                                    if (oHDDis.FTXhdDisChgType == "1" || oHDDis.FTXhdDisChgType == "2")
                                    {
                                        oGraphic.DrawString(cVB.oVB_GBResource.GetString("tDis") + "(" + oHDDis.FTXhdDisChgTxt + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, cVB.nVB_DecShow); //*Arm 63-04-16
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                        nStartY += 18;
                                    }
                                    //ชาจน์
                                    if (oHDDis.FTXhdDisChgType == "3" || oHDDis.FTXhdDisChgType == "4")
                                    {
                                        oGraphic.DrawString(cVB.oVB_GBResource.GetString("tChg") + "(" + oHDDis.FTXhdDisChgTxt + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, cVB.nVB_DecShow); //*Arm 63-04-16
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                        nStartY += 18;
                                    }
                                }
                                else
                                {
                                    //Redeem
                                    if (oHDDis.FTXhdDisChgType == "1")
                                    {
                                        oSql = new StringBuilder();
                                        oSql.AppendLine("SELECT FTRdhDocType");
                                        oSql.AppendLine("FROM TPSTSalRD WITH(NOLOCK) ");
                                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                                        oSql.AppendLine("AND FTXshDocNo = '" + tW_DocNo + "'  ");
                                        oSql.AppendLine("AND FTXrdRefCode = '" + oHDDis.FTXhdRefCode + "'  ");
                                        string tRdhDocType = oDB.C_GEToDataQuery<string>(oSql.ToString());
                                        if (tRdhDocType == "1")
                                        {
                                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tRdPdt") + "(" + oHDDis.FTXhdDisChgTxt + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        }
                                        else
                                        {
                                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tRdDis") + "(" + oHDDis.FTXhdDisChgTxt + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        }
                                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, cVB.nVB_DecShow); //*Arm 63-04-16
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                        nStartY += 18;
                                    }
                                }

                                //Coupon
                                if (oHDDis.FTXhdDisChgType == "5" || oHDDis.FTXhdDisChgType == "6")
                                {
                                    oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCpnRd") + "(" + oHDDis.FTXhdDisChgTxt + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, cVB.nVB_DecShow); //*Arm 63-04-16
                                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    nStartY += 18;
                                }

                            }
                        }
                        //================================================================

                        //*Arm 63-05-01 Comment Code
                        //if ((decimal)oHD.FCXshDis > (decimal)0)
                        //{
                        //    oGraphic.DrawString("Disc", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshDis, cVB.nVB_DecShow);
                        //    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
                        //    nStartY += 15;
                        //}

                        //if ((decimal)oHD.FCXshChg > (decimal)0)
                        //{
                        //    oGraphic.DrawString("Charge", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshChg, cVB.nVB_DecShow);
                        //    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
                        //    nStartY += 15;
                        //}
                        //++++++++++++++++++++++++++++

                        if ((decimal)oHD.FCXshRnd > (decimal)0)
                        {
                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshRnd, cVB.nVB_DecShow);
                            oGraphic.DrawString("Round Rcv: " + tAmt, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oHD.FCXshTotal - oHD.FCXshDis + oHD.FCXshChg), cVB.nVB_DecShow);
                            oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
                            nStartY += 18;
                        }
                    }

                    oSql.Clear();
                    oSql.AppendLine("SELECT SUM(ISNULL(FCXsdQty,0)) FROM TPSTSalDT  with(nolock)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                    oSql.AppendLine("AND FTXshDocNo = '" + tW_DocNo + "' ");
                    oSql.AppendLine("AND FTXsdStaPdt <> '4' ");
                    cTotalQty = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                    oGraphic.DrawString("TOTAL " + oSP.SP_SETtDecShwSve(1, (decimal)cTotalQty, cVB.nVB_DecShow) + " Items", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshGrand, cVB.nVB_DecShow);
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);

                    ////oGraphic.DrawString("TOTAL", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    //oGraphic.DrawString("TOTAL (VAT Included)", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY); //*Arm 62-10-27 -เพิ่ม Wording  (VAT Included) ใบกำกับภาษีอย่างย่อ
                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshGrand, cVB.nVB_DecShow);
                    //oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);

                    nStartY += 18;
                    tAmt = "Vatable : " + oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshVatable, cVB.nVB_DecShow);
                    tAmt += " " + "VAT : " + oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshVat, cVB.nVB_DecShow);
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);

                }


                //Print Payment
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RCV.FTRcvCode,(CASE WHEN ISNULL(RCVL.FTRcvName,'') = '' THEN (SELECT TOP 1 FTRcvName FROM TFNMRcv_L with(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode) ELSE RCVL.FTRcvName END) AS FTRcvName,");
                oSql.AppendLine("RCV.FCXrcUsrPayAmt,FCXrcDep,FCXrcChg,RCV.FTXrcRefNo1,RCV.FTXrcRefNo2 "); //*Net 63-03-28 ยกมาจาก baseline
                oSql.AppendLine("FROM TPSTSalRC RCV with(nolock) ");
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL with(nolock) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TFNMRcv RCVM ON RCV.FTRcvCode = RCVM.FTRcvCode AND RCVM.FTFmtCode <> '020'");   //*Em 62-12-27
                oSql.AppendLine("WHERE RCV.FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND RCV.FTXshDocNo = '" + tW_DocNo + "'");
                oSql.AppendLine("ORDER BY RCV.FNXrcSeqNo");
                List<cmlTPSTSalRC> aoRC = oDB.C_GETaDataQuery<cmlTPSTSalRC>(oSql.ToString());
                if (aoRC != null)
                {
                    foreach (cmlTPSTSalRC oRC in aoRC)
                    {
                        nStartY += 18;
                        //oGraphic.DrawString(oRC.FTRcvName, cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                        //*Net 63-03-28 ยกมาจาก baseline
                        if (string.IsNullOrEmpty(oRC.FTXrcRefNo1))
                        {
                            oGraphic.DrawString(oRC.FTRcvName, cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(oRC.FTXrcRefNo2.Split(';')[oRC.FTXrcRefNo2.Split(';').Length - 1]))
                            {
                                oGraphic.DrawString(oRC.FTRcvName + "(" + oRC.FTXrcRefNo1 + ")", cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                            }
                            else
                            {
                                oGraphic.DrawString(oRC.FTXrcRefNo2.Split(';')[oRC.FTXrcRefNo2.Split(';').Length - 1] + " (" + oRC.FTXrcRefNo1 + ")", cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                            }

                        }
                        //++++++++++++++++++++++++++++++++
                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oRC.FCXrcUsrPayAmt, cVB.nVB_DecShow);
                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[7], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
                        cChange = (decimal)oRC.FCXrcChg;
                    }
                    nStartY += 18;
                    oGraphic.DrawString("Change", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    tAmt = oSP.SP_SETtDecShwSve(1, cChange, cVB.nVB_DecShow);
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
                }

                //*Arm 63-05-01 อ้างอิง SO
                if (!string.IsNullOrEmpty(oHD.FTXshRefExt))
                {
                    nStartY += 18;
                    oGraphic.DrawString("อ้างอิง : " + oHD.FTXshRefExt, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                }
                //++++++++++++++++++++++

                //*Arm 63-04-03 - Print Point
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode, FTXshDocNo,FTXshCardID,FTXshCardNo,FTXshCstName,");
                oSql.AppendLine("FTXshCstTel,ISNULL(FCXshCstPnt,0) AS FCXshCstPnt, ISNULL(FCXshCstPntPmt,0) AS FCXshCstPntPmt");
                oSql.AppendLine("FROM TPSTSalHDCst WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + tW_DocNo + "'");
                oHDCst = oDB.C_GEToDataQuery<cmlTPSTSalHDCst>(oSql.ToString());

                if (oHDCst != null) //*Net 63-04-03
                {
                    tCstTel = oHDCst.FTXshCstTel == null ? "" : oHDCst.FTXshCstTel; //*Arm 63-04-20 ใช้ใน QR Code

                    nStartY += 30;
                    oGraphic.DrawString("Mem ID : " + oHD.FTCstCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                    nStartY += 18;
                    oSql = new StringBuilder();
                    oGraphic.DrawString(oHDCst.FTXshCstName, cVB.aoVB_PInvLayout[1], Brushes.Black, 10, nStartY);

                    string tCardNo = oHDCst.FTXshCardNo == null ? " - " : oHDCst.FTXshCardNo;
                    nStartY += 18;
                    oGraphic.DrawString("Card No. : " + tCardNo, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);


                    if (!string.IsNullOrEmpty(oHDCst.FTXshDocNo)) //*Arm 63-04-03 
                    {
                        string tTxnDocNo = tW_DocNo; //*Arm 63-05-01
                        decimal cCstPiontB4Used = 0;
                        decimal cSumPnt = 0;

                        if (oHD.FNXshDocType ==9) //*Arm 63-05-01 การณีบินคืนต้องหา TranSaction 
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT FTTxnRefDoc FROM TCNTMemTxnSale with(nolock)");
                            oSql.AppendLine("WHERE CONVERT(Datetime,FDTxnRefDate,121) = CONVERT(Datetime,'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate))+"',121)");

                            tTxnDocNo = oDB.C_GEToDataQuery<string>(oSql.ToString());
                        }

                        //หา Transaction การขาย
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT * FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + tTxnDocNo + "'");
                        cmlTCNTMemTxnSale oTxnSale = oDB.C_GEToDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());

                        if (oTxnSale != null)
                        {
                            //Print ข้อมมูล Member
                            if (!string.IsNullOrEmpty(oTxnSale.FTMemCode))
                            {
                                //nStartY += 18;
                                //oGraphic.DrawString("Mem ID : " + oTxnSale.FTMemCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                                //nStartY += 18;
                                //oSql = new StringBuilder();
                                //oGraphic.DrawString(oHDCst.FTXshCstName, cVB.aoVB_PInvLayout[1], Brushes.Black, 10, nStartY);

                                //string tCardNo = oHDCst.FTXshCardNo == null ? " - " : oHDCst.FTXshCardNo;
                                //nStartY += 18;
                                //oGraphic.DrawString("Card No. : " + tCardNo, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                                nStartY += 18;
                                if (oTxnSale.FDTxnPntExpired != null)
                                {
                                    oGraphic.DrawString("Expired : " + string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oTxnSale.FDTxnPntExpired)), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                                }
                                else
                                {
                                    oGraphic.DrawString("Expired : -", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                                }
                                
                            }
                            else
                            {
                                nStartY += 18;
                                oGraphic.DrawString("Expired : -", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                            }
                        }
                        else
                        {
                            nStartY += 18;
                            oGraphic.DrawString("Expired : -", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        }
                        nStartY += 18;
                        oGraphic.DrawString("Last Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        oGraphic.DrawString("Reg. Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                        oGraphic.DrawString("Promo Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                        oGraphic.DrawString("Total Point", cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);

                        //nStartY += 18;
                        //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cCstPiontB4Used, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                        //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPntPmt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                        //cPntRcv = oHDCst.FCXshCstPnt + oHDCst.FCXshCstPntPmt;
                        //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(cCstPiontB4Used + cPntRcv), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);


                        //หา Point ก่อนใช้
                        oSql.Clear();
                        oSql.AppendLine("SELECT count(*) FROM TCNTMemTxnRedeem with(nolock) WHERE FTRedRefDoc = '" + tTxnDocNo + "'");

                        if (oDB.C_GEToDataQuery<int>(oSql.ToString()) > 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT FCRedPntB4Bill FROM TCNTMemTxnRedeem with(nolock) WHERE FTRedRefDoc = '" + tTxnDocNo + "'");

                            cCstPiontB4Used = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
                        }
                        else
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT FCTxnPntB4Bill FROM TCNTMemTxnSale with(nolock) WHERE FTTxnRefDoc = '" + tTxnDocNo + "'");

                            cCstPiontB4Used = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
                        }
                        
                        //*Arm 63-05-07
                        if (oHD.FNXshDocType != 9)
                        {
                            //แต้มที่ใช้
                            oSql.Clear();
                            oSql.AppendLine("SELECT * FROM TCNTMemTxnRedeem WHERE FTRedRefDoc = '" + tTxnDocNo + "'");
                            cmlTCNTMemTxnRedeem oTxnRedeem = oDB.C_GEToDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                            if (oTxnRedeem != null)
                            {
                                nStartY += 18;
                                oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)oTxnRedeem.FCRedPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                decimal cRedPnt = (decimal)(oTxnRedeem.FCRedPntBillQty * (-1));
                                oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRedPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                cSumPnt = (decimal)(oTxnRedeem.FCRedPntB4Bill + (oTxnRedeem.FCRedPntBillQty * (-1)));
                                oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29
                            }
                            //แต้มที่ได้
                            nStartY += 18;
                            oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)oTxnSale.FCTxnPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                            decimal cSalePnt = (decimal)(oTxnSale.FCTxnPntBillQty - oHDCst.FCXshCstPntPmt);
                            oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSalePnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                            oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPntPmt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                            cSumPnt = (decimal)(oTxnSale.FCTxnPntB4Bill + (cSalePnt + oHDCst.FCXshCstPntPmt));
                            oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);
                            
                        }
                        else
                        {
                            decimal cRefundRed = 0;
                            oSql.Clear();
                            oSql.AppendLine("SELECT SUM(ISNULL(FNXrdPntUse,0)) AS FNXrdPntUse  FROM TPSTSalRD WITH(NOLOCK) WHERE FTBchCode =  '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + tW_DocNo + "'");
                            cRefundRed = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
                            
                            //แต้มที่ใช้คืน
                            nStartY += 18;
                            oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)cCstPiontB4Used, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                            decimal cRefundPnt = ((cRefundRed - oHDCst.FCXshCstPnt) * (-1));
                            oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRefundPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                            oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPntPmt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                            cSumPnt = (cCstPiontB4Used + (cRefundPnt + oHDCst.FCXshCstPntPmt));
                            oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29

                            cCstPiontB4Used = cSumPnt;

                            //แต้มที่ได้คืน
                            if (cRefundRed > 0)
                            {
                                nStartY += 18;
                                oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)cCstPiontB4Used, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)cRefundRed, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                cSumPnt = (decimal)(cCstPiontB4Used + cRefundRed);
                                oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);
                            }
                        }
                        //++++++++++++++++

                        oTxnSale = null;
                    }
                    
                }
                ////+++++++++++++

                //*Arm 63-05-08 Print ชื่อผู้ขาย
                oSql.Clear();
                oSql.AppendLine("SELECT FTUsrName FROM TCNMUser_L with(nolock) WHERE FTUsrCode ='"+ oHD.FTUsrCode  + "' ");
                string tUserName = oDB.C_GEToDataQuery<string>(oSql.ToString());
                nStartY += 30;
                oGraphic.DrawString("User : "+ tUserName, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                //+++++++++++++


                if (!string.IsNullOrEmpty(oHD.FTCstCode))
                {
                    tCstTel = oDB.C_GEToDataQuery<string>("SELECT FTCstTel FROM TCNMCst with(nolock) WHERE FTCstCode = '" + oHD.FTCstCode + "'");
                }
                //*Arm 63-05-07
                if (oHD.FNXshDocType == 9)
                {
                    nStartY += 30;
                    tMsg = cVB.oVB_GBResource.GetString("tRefundPdt");
                    if (!string.IsNullOrEmpty(oHD.FTXshRefInt))
                    {
                        tMsg += cVB.oVB_GBResource.GetString("tRefer") + ":" + oHD.FTXshRefInt;
                    }
                    oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[8], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                    //if (bC_PrnCopy) //*Net 63-02-25 เมื่อพิมพ์บิลคืน ให้พิมพ์ สำเนา
                    //{
                    //    //สำเนา
                    //    nStartY += 18;
                    //    tPrint = "!!! " + cVB.oVB_GBResource.GetString("tCopy") + " " + oHD.FNXshDocPrint + " !!!";
                    //    oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                    //    //++++++++++++++
                    //}
                }
                //++++++++++++

                //*Em 62-09-06
                //Remark
                if (!string.IsNullOrEmpty(oHD.FTXshRmk))
                {
                    nStartY += 15;
                    tPrint = oHD.FTXshRmk;
                    aRmk = tPrint.Split((char)10);
                    foreach (string tStr in aRmk)
                    {
                        nStartY += 15;
                        oGraphic.DrawString(tStr, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    }
                }
                //สำเนา
                nStartY += 15;
                tPrint = "!!! " + cVB.oVB_GBResource.GetString("tCopy") + " " + oHD.FNXshDocPrint + " !!!"; // *Arm 62-11-12 - เพิ่มเลขที่สำเนา
                oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth , 15), oFormatCenter);
                //++++++++++++++

                //*Arm 62-10-31  เงื่อนไขการแสดง Barcode และ QRCode
                switch (cVB.nVB_ChkShowBarQR)
                {
                    case 1:    //1 : แสดง Barcode
                        nStartY += 30;
                        nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                        nStartY += 15;
                        cSale.C_PRNxBarcode(ref oGraphic, tW_DocNo, nWidth, nStartY);
                        break;

                    case 2:     //2 : แสดง QRCode,
                        nStartY += 30;
                        nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                        nStartY += 15;
                        tDataA = tW_DocNo;
                        tDataB = tW_DocNo + "|" + oHD.FTCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + Convert.ToDateTime(oHD.FDXshDocDate.Value).ToString("yyyy-MM-dd HH:mm:ss");
                        tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
                        cSale.C_PRNxQRCode(ref oGraphic, tDataA + "|" + tDataB, nWidth, nStartY);
                        break;

                    case 3:     //3 : แสดงทั้ง Barcode และ QRcode
                        nStartY += 30;
                        nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                        nStartY += 15;
                        cSale.C_PRNxBarcode(ref oGraphic, tW_DocNo, nWidth, nStartY);
                        nStartY += 30;
                        tDataA = tW_DocNo;
                        tDataB = tW_DocNo + "|" + oHD.FTCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + Convert.ToDateTime(oHD.FDXshDocDate.Value).ToString("yyyy-MM-dd HH:mm:ss");
                        tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
                        cSale.C_PRNxQRCode(ref oGraphic, tDataA + "|" + tDataB, nWidth, nStartY);
                        break;

                    default:    // ไม่แสดง
                        nStartY += 30;
                        nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                        nStartY += 15;
                        break;
                }
                //+++++++++++++++++++

                //nStartY += 30;
                //nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                //nStartY += 15;
                //cSale.C_PRNxBarcode(ref oGraphic, tW_DocNo, nWidth, nStartY);
                //nStartY += 30;
                //tDataA = tW_DocNo;
                //tDataB = tW_DocNo + "|" + oHD.FTCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + Convert.ToDateTime(oHD.FDXshDocDate.Value).ToString("yyyy-MM-dd HH:mm:ss");
                //tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
                //cSale.C_PRNxQRCode(ref oGraphic, tDataA + "|" + tDataB, nWidth, nStartY);


                nW_StartY = nStartY; //*Arm 63-05-03
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "W_PRNxSlip : " + oEx.Message); }
            finally
            {
                if (oGraphic != null)
                    oGraphic.Dispose();

                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();

                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                aoDT = null;
                aoDTDis = null;
                oMsg = null;
                tLine = null;
                oSP.SP_CLExMemory();
            }
        }
        #endregion Function

        #region Method/Events
        private void ocmBack_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void wReprintPreview_Shown(object sender, EventArgs e)
        {
            
        }

        private void opnPreview_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                W_PRNxSlip(e.Graphics);

                //*Arm 63-05-03
                nW_StartY += 100;

                if (nW_StartY > 700 && nW_StartY < 830)
                {
                    opnPreview1.Height = 1000+(830 - nW_StartY);
                }
                else if(nW_StartY > 830)
                {
                    opnPreview1.Height = nW_StartY;
                }

                nW_StartY = 0;
                //+++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "opnPreview_Paint : " + oEx.Message); }
        }

        private void ocmPrint_Click(object sender, EventArgs e)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                W_DATxPrint();

                oSql.AppendLine("UPDATE TPSTSalHD with(rowlock)");
                oSql.AppendLine("SET FNXshDocPrint = ISNULL(FNXshDocPrint,0) + 1");
                oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"' AND FTXshDocNo = '"+ tW_DocNo +"'");

                oDB.C_SETxDataQuery(oSql.ToString()); //*Arm 62-11-12

                this.Close();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "ocmPrint_Click : " + oEx.Message); }
        }

        private void oDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                W_PRNxSlip(e.Graphics);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "oDoc_PrintPage : " + oEx.Message); }
        }

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
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "OnPaintBackground " + oEx.Message); }
        }

        #endregion Method/Events

        //*Net 63-03-28 ยกมาจาก baseline
        private void wReprintPreview_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {

                    case Keys.Escape:
                        ocmBack_Click(ocmBack, new EventArgs());
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("wReprintPreview", "wReprintPreview_KeyDown : " + oEx.Message); }
        }
        
    }
}
