using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic;
using MQReceivePrc.Class;
using System.Drawing;
using MQReceivePrc.Models.Sale;
using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Config;
using System.Data;
using MQReceivePrc.Models.Pos;
using System.Drawing.Imaging;
using MQReceivePrc.Models.Shift;
using System.Resources;
using MQReceivePrc.Resources_String;
using MQReceivePrc.Models.SlipMsg;
using ZXing.QrCode;
using ZXing;
using BarcodeLib;
using AdaPos.Class;
using System.Runtime.CompilerServices;

namespace MQReceivePrc.Class
{
    public class cEJ
    {
        private static List<Font> aoC_Font;
        private static ResourceManager oC_Resource;
        private static string tC_tConnStr;
        private static string tC_PathLogo;
        private static int nC_CmdTimeout;
        private static int nC_Language;
        private static int nC_DecShow;
        private static int nC_ChkShowPdtBarCode;
        private static int nC_ChkShowBarQR;
        private static int nC_PaperSize;
        private static bool bC_AlwPrnVoid;
        private static bool bC_PrnShiftSumRedeem;
        private static bool bC_PrnRowNum;

        private cSP oC_SP;
        public cEJ(string ptConnStr, int pnCmdTimeout)
        {
            oC_SP = new cSP();

            //*Net 63-07-15 ถ้าไม่ใช่ DB เดิมให้ไป get Config มาใหม่
            if (ptConnStr != tC_tConnStr)
            {
                tC_tConnStr = ptConnStr;
                nC_CmdTimeout = pnCmdTimeout;
                C_GETxConfig();
            }
        }
        #region Function
        private void C_GETxConfig()
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                //Default
                #region Default
                ////////////////////////////////////

                aoC_Font = new List<Font>();

                bC_AlwPrnVoid = false;
                bC_PrnShiftSumRedeem = false;
                bC_PrnRowNum = false;

                ///////////////////////////////////
                #endregion

                //Get Language
                #region Get Lang
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 ISNULL(FNBchDefLang,1)");
                oSql.AppendLine("FROM TCNMBranch WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchStaHQ='1'");
                nC_Language = oDB.C_DAToExecuteQuery<int>(tC_tConnStr, oSql.ToString(), nC_CmdTimeout);

                switch (nC_Language)
                {
                    case 1:
                        oC_Resource = new ResourceManager(typeof(resGlobal_TH));
                        break;
                    case 2:
                        oC_Resource = new ResourceManager(typeof(resGlobal_EN));
                        break;
                    default:
                        oC_Resource = new ResourceManager(typeof(resGlobal_TH));
                        break;
                }
                #endregion

                //Get TSysConfig
                #region Get TSysConfig
                /////////////////////////////////////////////

                oSql.Clear();
                oSql.AppendLine("SELECT FTSysCode, FTSysSeq, FTSysKey, FTSysStaUsrValue, FTSysStaUsrRef");
                oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSysCode IN (");
                oSql.AppendLine(" 'bPS_AlwPrnVoid', 'bPS_AlwPrintShift'");
                oSql.AppendLine(" ,'bPS_PrnRownum','nVB_ChkShowBarQR'");
                oSql.AppendLine(" ,'nPS_ChkShowPdtBar','tPS_Print','ADecPntShw'");
                oSql.AppendLine(" ,'PInvLayout'");
                oSql.AppendLine(")");
                oSql.AppendLine("ORDER BY FTSysCode,CONVERT(INT,FTSysSeq)");
                odtTmp = oDB.C_DAToExecuteQuery(tC_tConnStr, oSql.ToString(), nC_CmdTimeout);
                if (odtTmp != null)
                {
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        switch (oRow.Field<string>("FTSysCode"))
                        {
                            case "bPS_AlwPrnVoid":
                                bC_AlwPrnVoid = (oRow.Field<string>("FTSysStaUsrValue") == "1") ? true : false;
                                break;
                            case "bPS_AlwPrintShift":   //*Em 62-10-02
                                if (oRow.Field<string>("FTSysSeq") == "4")
                                {
                                    bC_PrnShiftSumRedeem = (oRow.Field<string>("FTSysStaUsrValue") == "1") ? true : false;
                                }
                                break;
                            case "bPS_PrnRownum":
                                bC_PrnRowNum = (oRow.Field<string>("FTSysStaUsrValue") == "1") ? true : false;
                                break;
                            case "nVB_ChkShowBarQR":
                                if (!String.IsNullOrEmpty(oRow.Field<string>("FTSysStaUsrValue")))
                                {
                                    nC_ChkShowBarQR = Convert.ToInt32(oRow.Field<string>("FTSysStaUsrValue"));
                                }
                                break;
                            case "nPS_ChkShowPdtBar":
                                if (!String.IsNullOrEmpty(oRow.Field<string>("FTSysStaUsrValue")))
                                {
                                    nC_ChkShowPdtBarCode = Convert.ToInt32(oRow.Field<string>("FTSysStaUsrValue"));
                                }
                                break;
                            case "tPS_Print":
                                if (oRow.Field<string>("FTSysSeq") == "4")
                                {
                                    nC_PaperSize = Convert.ToInt32(oRow.Field<string>("FTSysStaUsrValue"));
                                }
                                break;
                            case "PInvLayout":
                                if (Convert.ToInt32(oRow.Field<string>("FTSysSeq")) >= 1 &&
                                   Convert.ToInt32(oRow.Field<string>("FTSysSeq")) <= 9)
                                {
                                    string[] atFontStyle;
                                    if (String.IsNullOrEmpty(oRow.Field<string>("FTSysStaUsrValue")))
                                    {
                                        atFontStyle = oRow.Field<string>("FTSysStaDefValue").Split(',');

                                    }
                                    else
                                    {
                                        atFontStyle = oRow.Field<string>("FTSysStaUsrValue").Split(',');
                                    }
                                    FontStyle oFontStyle = new FontStyle();
                                    oFontStyle = FontStyle.Regular;
                                    if (!String.IsNullOrEmpty(atFontStyle[2])) oFontStyle |= FontStyle.Bold;
                                    if (!String.IsNullOrEmpty(atFontStyle[3])) oFontStyle |= FontStyle.Italic;
                                    if (!String.IsNullOrEmpty(atFontStyle[4])) oFontStyle |= FontStyle.Underline;

                                    aoC_Font.Add(new Font(atFontStyle[0], Convert.ToSingle(atFontStyle[1]), oFontStyle));
                                }
                                if (Convert.ToInt32(oRow.Field<string>("FTSysSeq")) == 98)
                                {
                                    tC_PathLogo = oRow.Field<string>("FTSysStaUsrValue");
                                }
                                break;
                            case "ADecPntShw":
                                nC_DecShow = Convert.ToInt32(oRow.Field<string>("FTSysStaUsrValue"));
                                break;
                        }

                    }
                }

                /////////////////////////////////////////////
                #endregion

                //Fix Value
                #region Fix Value
                /////////////////////////////////////////////

                for (int nCount = aoC_Font.Count; nCount < 9; nCount++)
                {
                    aoC_Font.Add(new Font("CordiaUPC", 11.5f, FontStyle.Regular));
                }
                if (nC_ChkShowBarQR == 0) nC_ChkShowBarQR = 1;
                //if (nC_ChkShowPdtBarCode == 0) nC_ChkShowPdtBarCode = 1;
                if (nC_DecShow == 0) nC_DecShow = 2;
                if (nC_PaperSize == 0) nC_DecShow = 2;

                /////////////////////////////////////////////
                #endregion
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cEJ", "C_GETxConfig : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //oC_SP.SP_CLExMemory();
            }
        }



        //*Net 63-07-15 C_GENbSlip เก่า
        //public bool C_GENbSlip(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, cmlTCNMPos poPos)
        //{
        //    Graphics oGraphic = null;
        //    StringFormat oFormatFar = null, oFormatCenter = null;
        //    cSlipMsg oMsg;
        //    string tLine, tDeposit;
        //    int nStartY = 0, nWidth;
        //    cDatabase oDB = new cDatabase();
        //    StringBuilder oSql = new StringBuilder();
        //    string tAmt, tVat;
        //    decimal cChange = 0;
        //    string tDataA = "";
        //    string tDataB = "";
        //    string tCstTel = "";
        //    List<cmlTPSTSalDT> aoDT;
        //    List<cmlTPSTSalDTDis> aoDTDis;
        //    string tPrint = ""; //*Em 62-09-06
        //    string[] atRmk;
        //    List<Font> aoFont = new List<Font>();
        //    Font oFont = new Font("CordiaUPC", 11.5f, FontStyle.Regular);
        //    string tConnStr = poSalePos.ptConnStr;
        //    int nCmdTime = Convert.ToInt32(poShopDB.nCommandTimeOut);
        //    cSP oSP = new cSP();
        //    int nDecShw = 2;
        //    string tPathFile;

        //    //*Arm 63-05-06
        //    int nStaSumPrn = 2;
        //    bool bAlwPrnVoid = false;
        //    int nChkShowPdtBarCode = 0;
        //    List<cmlPrnSplipDTDis> aoPrnDTDis;
        //    List<cmlTPSTSalPD> aoPD;
        //    cmlTPSTSalHDCst oHDCst;
        //    decimal cTotalQty = 0;
        //    int nLng = 1;
        //    string tDisc = "";
        //    string tCharge = "";
        //    string tRedPdt = "";
        //    string tRedDisc = "";
        //    string tRedCpn = "";
        //    string tRef = "";
        //    string tMsg = "";
        //    string tRefundPdt = "";
        //    string tRefer = "";
        //    string tCopy = "";
        //    //+++++++++++++++
        //    try
        //    {
        //        tLine = "------------------------------------------------------------------------";

        //        nWidth = 280;

        //        oSql.Clear();
        //        oSql.AppendLine("SELECT FTSysSeq,FTSysStaDefValue,FTSysStaUsrValue");
        //        oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
        //        oSql.AppendLine("WHERE FTSysCode = 'PInvLayout'");
        //        oSql.AppendLine("AND (CONVERT(INT,FTSysSeq) BETWEEN 1 AND 9)");
        //        oSql.AppendLine("ORDER BY CONVERT(INT,FTSysSeq)");
        //        DataTable odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), nCmdTime);
        //        if (odtTmp != null)
        //        {
        //            aoFont = new List<Font>();
        //            foreach (DataRow oRow in odtTmp.Rows)
        //            {
        //                string[] atFont;
        //                if (string.IsNullOrEmpty(oRow.Field<string>("FTSysStaUsrValue")))
        //                {
        //                    atFont = oRow.Field<string>("FTSysStaDefValue").Split(',');

        //                }
        //                else
        //                {
        //                    atFont = oRow.Field<string>("FTSysStaDefValue").Split(',');
        //                }
        //                FontStyle oFontStyle = new FontStyle();
        //                oFontStyle = FontStyle.Regular;
        //                if (!string.IsNullOrEmpty(atFont[2]))
        //                    oFontStyle |= FontStyle.Bold;

        //                if (!string.IsNullOrEmpty(atFont[3]))
        //                    oFontStyle |= FontStyle.Italic;

        //                if (!string.IsNullOrEmpty(atFont[4]))
        //                    oFontStyle |= FontStyle.Underline;

        //                oFont = new Font(atFont[0], Convert.ToSingle(atFont[1]), oFontStyle);
        //                aoFont.Add(oFont);
        //            }
        //        }

        //        //*Arm 63-05-06 Load Option
        //        oSql.Clear();
        //        oSql.AppendLine("SELECT FTSysCode, FTSysSeq, FTSysKey, FTSysStaUsrValue, FTSysStaUsrRef");
        //        oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
        //        oSql.Append("WHERE FTSysCode IN ('bPS_AlwPrnVoid', 'nPS_ChkShowPdtBar')");
        //        DataTable oDTbConf = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), nCmdTime);
        //        if (oDTbConf != null)
        //        {
        //            foreach (DataRow oRow in oDTbConf.Rows)
        //            {
        //                switch (oRow.Field<string>("FTSysCode"))
        //                {
        //                    case "bPS_AlwPrnVoid":
        //                        if (oRow.Field<string>("FTSysStaUsrValue") == "1")
        //                        {
        //                            bAlwPrnVoid = true;
        //                        }
        //                        break;

        //                    case "nPS_ChkShowPdtBar":
        //                        if (!string.IsNullOrEmpty(oRow.Field<string>("FTSysStaUsrValue")))
        //                        {
        //                            nChkShowPdtBarCode = Convert.ToInt32(oRow.Field<string>("FTSysStaUsrValue"));
        //                        }
        //                        break;
        //                }
        //            }
        //        }
        //        //SET TEXT
        //        if (nLng == 1)
        //        {
        //            tDisc = "ลด";
        //            tCharge = "ชาจน์";
        //            tRedPdt = "แลกแต้มรับสินค้า";
        //            tRedDisc = "แลกแต้มรับส่วนลด";
        //            tRedCpn = "คูปองแลกซื้อ";
        //            tRef = "อ้างอิง";
        //            tRefundPdt = "! คืนสินค้า !";
        //            tRefer = "อ้างอิงเลขที่";
        //            tCopy = "สำเนา";
        //        }
        //        else
        //        {
        //            tDisc = "Dis.";
        //            tCharge = "Chg.";
        //            tRedPdt = "Redeem Get Product";
        //            tRedDisc = "Redeem Get Discount";
        //            tRedCpn = "Coupon Redeem";
        //            tRef = "Ref.";
        //            tRefundPdt = "! Refund !";
        //            tRefer = "Ref. No";
        //            tCopy = "Copy";
        //        }
        //        //++++++++++++++

        //        //Bitmap oNewBitmap = new Bitmap(nWidth, 1000, PixelFormat.Format32bppArgb);
        //        //Bitmap oNewBitmap = new Bitmap(nWidth, 5000, PixelFormat.Format64bppArgb);  //*Em 62-10-16
        //        Bitmap oNewBitmap = new Bitmap(nWidth, 55000, PixelFormat.Format64bppArgb);  //*Arm 63-05-22
        //        oGraphic = Graphics.FromImage(oNewBitmap);
        //        //oGraphic.FillRectangle(Brushes.White, new RectangleF(0, 0, nWidth, 1000));
        //        oGraphic.FillRectangle(Brushes.White, new RectangleF(0, 0, nWidth, 55000));  //*Arm 63-05-22
        //        oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
        //        oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

        //        oMsg = new cSlipMsg();
        //        nStartY = oMsg.C_GETnSlipMsg(poSalePos.ptConnStr, poPos.FTSmgCode, "1", oGraphic, nWidth, nStartY, aoFont[0]);    // Header Slip Msg
        //        oSql = new StringBuilder();
        //        oSql.AppendLine("SELECT FCXshTotal,FCXshTotalNV,FCXshTotalNoDis,FCXshTotalB4DisChgV,FCXshTotalB4DisChgNV,FTXshDisChgTxt,");
        //        oSql.AppendLine("FCXshDis,FCXshChg,FCXshTotalAfDisChgV,FCXshTotalAfDisChgNV,FCXshRefAEAmt,FCXshAmtV,FCXshAmtNV,");
        //        oSql.AppendLine("FCXshVat,FCXshVatable,FCXshGrand,FDXshDocDate,FTPosCode,FTUsrCode,FTCstCode,FTXshRmk,FCXshRnd,FNXshDocType, FTXshRefExt, FTXshRefInt");
        //        oSql.AppendLine("FROM TPSTSalHD HD with(nolock)");
        //        oSql.AppendLine("WHERE FTBchCode =  '" + poSalePos.ptBchCode + "'");
        //        oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //        cmlTPSTSalHD oHD = oDB.C_DAToExecuteQuery<cmlTPSTSalHD>(tConnStr, oSql.ToString(), nCmdTime);
        //        if (oHD == null) return true;

        //        // Get ค่า ID เครื่อง จาก DB
        //        oGraphic.DrawString("ID:" + poPos.FTPosRegNo.PadRight(30) + "USR: " + oHD.FTUsrCode + " T: " + oHD.FTPosCode, aoFont[1], Brushes.Black, 0, nStartY);
        //        nStartY += 18;
        //        oGraphic.DrawString(Convert.ToDateTime(oHD.FDXshDocDate.Value).ToString("dd/MM/yyyy HH:mm") + " BNO:" + poSalePos.ptXihDocNo,
        //                            aoFont[1], Brushes.Black, 0, nStartY);

        //        //Print DT
        //        //++++++ *Arm 63-05-05  สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต +++++++
        //        if (!string.IsNullOrEmpty(poPos.FTPosStaSumPrn)) { nStaSumPrn = Convert.ToInt32(poPos.FTPosStaSumPrn); }

        //        if (nStaSumPrn == 1) // 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์
        //        {
        //            oSql.Clear();
        //            oSql.AppendLine("SELECT DT1.FTPdtCode, ");
        //            oSql.AppendLine("DT1.FTXsdPdtName, ");
        //            oSql.AppendLine("DT1.FTXsdBarCode, ");
        //            oSql.AppendLine("SUM(DT1.FCXsdQty) AS FCXsdQty,");
        //            oSql.AppendLine("DT1.FCXsdSetPrice,");
        //            oSql.AppendLine("SUM(DT1.FCXsdNet) AS FCXsdNet,");
        //            oSql.AppendLine("DT1.FTXsdVatType,");
        //            oSql.AppendLine("SUM(DT1.FCXsdAmtB4DisChg) AS FCXsdAmtB4DisChg");
        //            //oSql.AppendLine("SUM(DT1.FCXsdQtyAll) AS FCXsdQtyAll,");
        //            //oSql.AppendLine("DT1.FCXsdSalePrice AS FCXsdSalePrice ");
        //            oSql.AppendLine("FROM(SELECT(SELECT TOP 1 FNXsdSeqNo FROM TPSTSalDT WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FTPdtCode = DT.FTPdtCode AND  FTXsdBarCode = DT.FTXsdBarCode ORDER BY FNXsdSeqNo ASC) AS FNXsdSeqNo,");
        //            oSql.AppendLine("        DT.FTPdtCode,");
        //            oSql.AppendLine("        DT.FTXsdPdtName,");
        //            oSql.AppendLine("        DT.FTXsdBarCode,");
        //            oSql.AppendLine("        DT.FCXsdQty AS FCXsdQty,");
        //            oSql.AppendLine("        DT.FCXsdSetPrice,");
        //            oSql.AppendLine("        DT.FCXsdNet AS FCXsdNet,");
        //            oSql.AppendLine("        DT.FTXsdVatType,");
        //            oSql.AppendLine("        DT.FCXsdAmtB4DisChg AS FCXsdAmtB4DisChg");
        //            //oSql.AppendLine("        ISNULL(PD.FCXsdQty, DT.FCXsdQty) AS FCXsdQtyAll,");
        //            //oSql.AppendLine("        ISNULL(PD.FCXsdSetPrice, DT.FCXsdSetPrice) AS FCXsdSalePrice");
        //            oSql.AppendLine("    FROM TPSTSalDT DT WITH(NOLOCK)");
        //            //oSql.AppendLine("    LEFT JOIN(SELECT PD.FTBchCode, PD.FTXshDocNo, PD.FNXsdSeqNo, PD.FCXsdQty, PD.FCXsdSetPrice");
        //            //oSql.AppendLine("                FROM TPSTSalPD PD WITH(NOLOCK)");
        //            //oSql.AppendLine("                INNER JOIN(SELECT FTBchCode, FTXshDocNo, FNXsdSeqNo, MIN(FCXsdSetPrice) AS FCXsdSetPrice");
        //            //oSql.AppendLine("                            FROM TPSTSalPD WITH(NOLOCK)");
        //            //oSql.AppendLine("                            WHERE FTBchCode = '" + poSalePos.ptBchCode + "' AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "' AND FTXpdGetType = '4'");
        //            //oSql.AppendLine("                            GROUP BY FTBchCode, FTXshDocNo, FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
        //            //oSql.AppendLine("                WHERE PD.FTBchCode = '" + poSalePos.ptBchCode + "' AND PD.FTXshDocNo = '" + poSalePos.ptXihDocNo + "' AND PD.FTXpdGetType = '4') PD");
        //            //oSql.AppendLine("        ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
        //            oSql.AppendLine("    WHERE DT.FTBchCode = '" + poSalePos.ptBchCode + "'");
        //            oSql.AppendLine("    AND DT.FTXshDocNo = '" + poSalePos.ptXihDocNo + "' ");

        //            if (bAlwPrnVoid == false)
        //            {
        //                oSql.AppendLine(" AND DT.FTXsdStaPdt <> '4' ");
        //            }

        //            oSql.AppendLine(") AS DT1 ");
        //            oSql.AppendLine("GROUP BY DT1.FNXsdSeqNo, DT1.FTPdtCode,DT1.FTXsdPdtName,DT1.FTXsdBarCode, ");
        //            //oSql.AppendLine("DT1.FCXsdSetPrice,DT1.FTXsdVatType,DT1.FCXsdSalePrice ");
        //            oSql.AppendLine("DT1.FCXsdSetPrice,DT1.FTXsdVatType ");
        //            oSql.AppendLine("ORDER BY DT1.FNXsdSeqNo ASC ");

        //            aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(tConnStr, oSql.ToString(), nCmdTime);

        //            oSql.Clear();
        //            oSql.AppendLine("SELECT (SELECT FTXsdBarCode FROM TPSTSalDT WHERE FTBchCode = DTDis.FTBchCode AND FTXshDocNo = DTDis.FTXshDocNo AND FNXsdSeqNo = DTDis.FNXsdSeqNo ) AS FTXsdBarCode,");
        //            oSql.AppendLine("DTDis.FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
        //            oSql.AppendLine("FROM TPSTSalDTDis DTDis with(nolock)");
        //            oSql.AppendLine("WHERE DTDis.FTBchCode = '" + poSalePos.ptBchCode + "'");
        //            oSql.AppendLine("AND DTDis.FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //            oSql.AppendLine("AND FNXddStaDis = 1");
        //            oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
        //            oSql.AppendLine("ORDER BY FDXddDateIns");
        //            aoPrnDTDis = oDB.C_GETaDataQuery<cmlPrnSplipDTDis>(tConnStr, oSql.ToString(), nCmdTime);

        //            if (aoDT != null)
        //            {
        //                foreach (cmlTPSTSalDT oDT in aoDT)
        //                {
        //                    switch (nChkShowPdtBarCode) // Check Option พิมพ์รหัสสินค้าหรือบาร์โค้ดในแสดงสลิป  0:ไม่แสดง, 1:แสดง Product Code, 2:แสดง Barcode
        //                    {
        //                        case 1:
        //                            nStartY += 18;
        //                            //oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                            oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));     //*Arm 63-05-12
        //                            break;
        //                        case 2:
        //                            nStartY += 18;
        //                            //oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                            oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));     //*Arm 63-05-12
        //                            break;
        //                        default:
        //                            nStartY += 18;
        //                            //oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                            oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));     //*Arm 63-05-12
        //                            break;
        //                    }

        //                    if (oDT.FTXsdVatType != null || oDT.FTXsdVatType == "1")
        //                    {
        //                        tVat = "V";
        //                    }
        //                    else
        //                    {
        //                        tVat = " ";
        //                    }

        //                    //*Arm 63-05-12
        //                    if (oDT.FCXsdQty > 1)
        //                    {
        //                        nStartY += 18;
        //                        oGraphic.DrawString("  " + oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                        oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar);
        //                        oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF((nWidth - 70) + 60, nStartY, 10, 18), oFormatCenter);
        //                    }
        //                    else
        //                    {
        //                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                        oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar);
        //                        oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF((nWidth - 70) + 60, nStartY, 10, 18), oFormatCenter);
        //                    }
        //                    //+++++++++++++
        //                    //if (nChkShowPdtBarCode > 0) // Option กำหนดให้แสดง PdtCode หรือ Barcode
        //                    //{
        //                    //    nStartY += 18;
        //                    //    oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                    //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

        //                    //    //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
        //                    //    //{
        //                    //    //    nStartY += 18;
        //                    //    //    oGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), nDecShw);
        //                    //    //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //    //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

        //                    //    //    switch (nChkShowPdtBarCode)
        //                    //    //    {
        //                    //    //        case 1:
        //                    //    //            nStartY += 18;
        //                    //    //            oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //            break;
        //                    //    //        case 2:
        //                    //    //            nStartY += 18;
        //                    //    //            oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //            break;

        //                    //    //        default:
        //                    //    //            nStartY += 18;
        //                    //    //            oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //            break;
        //                    //    //    }

        //                    //    //    nStartY += 18;
        //                    //    //    oGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), nDecShw);
        //                    //    //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //    //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //    //}
        //                    //    //else
        //                    //    //{
        //                    //    //    nStartY += 18;
        //                    //    //    oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), nDecShw);
        //                    //    //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //    //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //    //}
        //                    //}
        //                    //else
        //                    //{
        //                    //    if (oDT.FCXsdQty > 1)
        //                    //    {
        //                    //        nStartY += 18;
        //                    //        oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                    //        oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

        //                    //        //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
        //                    //        //{
        //                    //        //    nStartY += 18;
        //                    //        //    oGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), nDecShw);
        //                    //        //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

        //                    //        //    switch (nChkShowPdtBarCode)
        //                    //        //    {
        //                    //        //        case 1:
        //                    //        //            nStartY += 18;
        //                    //        //            oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //            break;
        //                    //        //        case 2:
        //                    //        //            nStartY += 18;
        //                    //        //            oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //            break;

        //                    //        //        default:
        //                    //        //            nStartY += 18;
        //                    //        //            oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //            break;
        //                    //        //    }

        //                    //        //    nStartY += 18;
        //                    //        //    oGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), nDecShw);
        //                    //        //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //        //}
        //                    //        //else
        //                    //        //{
        //                    //        //    nStartY += 18;
        //                    //        //    oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), nDecShw);
        //                    //        //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //        //}
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                    //        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdSalePrice, nDecShw); 
        //                    //        oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //    }
        //                    //}

        //                    if (aoPrnDTDis != null)
        //                    {
        //                        decimal cAmt = (decimal)(oDT.FCXsdAmtB4DisChg);
        //                        decimal cDis = 0; //เก็บผลรวมส่วนลด
        //                        decimal cChg = 0; //เก็บผลรวมชาจน์
        //                        int nRow = 0;
        //                        foreach (cmlPrnSplipDTDis oDTDis in aoPrnDTDis.Where(c => c.FTXsdBarCode == oDT.FTXsdBarCode))
        //                        {
        //                            switch (oDTDis.FTXddDisChgType)
        //                            {
        //                                case "1":
        //                                case "2":
        //                                    cDis += (decimal)oDTDis.FCXddValue;
        //                                    break;
        //                                case "3":
        //                                case "4":
        //                                    cChg += (decimal)oDTDis.FCXddValue;
        //                                    break;
        //                            }
        //                            nRow++;
        //                        }

        //                        if (nRow > 0)   //มี Transaction ส่วนลดรายการ
        //                        {
        //                            if (cDis > 0)     // แสดง แสดงส่วนลด
        //                            {
        //                                nStartY += 18;
        //                                oGraphic.DrawString("  " + tDisc + "(" + cDis + ")", aoFont[2], Brushes.Black, 0, nStartY);
        //                                cAmt = (cAmt - cDis);
        //                                tAmt = oSP.SP_SETtDecShwSve(1, cAmt, nDecShw);
        //                                //oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                                oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar); //*Arm 63-05-12
        //                            }

        //                            if (cChg > 0)   // แสดงชาจน์
        //                            {
        //                                nStartY += 18;
        //                                oGraphic.DrawString("  " + tCharge + "(" + cChg + ")", aoFont[2], Brushes.Black, 0, nStartY);
        //                                cAmt = (cAmt + cChg);
        //                                tAmt = oSP.SP_SETtDecShwSve(1, cAmt, nDecShw);
        //                                //oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                                oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar); //*Arm 63-05-12
        //                            }
        //                        }//End  Transaction ส่วนลดรายการ
        //                    }//End if (aoDTDis != null)
        //                }//End foreach (cmlTPSTSalDT oDT in aoDT)
        //            }// End if (aoDT != null)
        //        }//End 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์ *Arm 63-05-05 
        //        else
        //        {
        //            // 2:ไม่อนุญาตให้รวมรายการสินค้าตอนพิมพ์
        //            oSql.Clear();
        //            oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FCXsdQty,");
        //            oSql.AppendLine("DT.FTPdtCode,DT.FTXsdBarCode,");
        //            oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdNet ,DT.FTXsdVatType,DT.FCXsdAmtB4DisChg");
        //            //oSql.AppendLine("ISNULL(PD.FCXsdQty,DT.FCXsdQty) AS FCXsdQtyAll,ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice) AS FCXsdSalePrice");
        //            oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
        //            //oSql.AppendLine("LEFT JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdQty,PD.FCXsdSetPrice");
        //            //oSql.AppendLine("   FROM TPSTSalPD PD WITH(NOLOCK)");
        //            //oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
        //            //oSql.AppendLine("   	FROM TPSTSalPD WITH(NOLOCK)");
        //            //oSql.AppendLine("   	WHERE FTBchCode = '" + poSalePos.ptBchCode + "' AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "' AND FTXpdGetType = '4'");
        //            //oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
        //            //oSql.AppendLine("   WHERE PD.FTBchCode = '" + poSalePos.ptBchCode + "' AND PD.FTXshDocNo = '" + poSalePos.ptXihDocNo + "' AND PD.FTXpdGetType = '4') PD");
        //            //oSql.AppendLine("   ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
        //            oSql.AppendLine("WHERE DT.FTBchCode = '" + poSalePos.ptBchCode + "'");
        //            oSql.AppendLine("AND DT.FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //            //++++++++++++++++++
        //            aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(tConnStr, oSql.ToString(), nCmdTime);

        //            oSql.Clear();
        //            oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
        //            oSql.AppendLine("FROM TPSTSalDTDis with(nolock)");
        //            oSql.AppendLine("WHERE FTBchCode =  '" + poSalePos.ptBchCode + "'");
        //            oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //            oSql.AppendLine("AND FNXddStaDis = 1");
        //            oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");
        //            aoPrnDTDis = oDB.C_GETaDataQuery<cmlPrnSplipDTDis>(tConnStr, oSql.ToString(), nCmdTime);

        //            if (aoDT != null)
        //            {
        //                foreach (cmlTPSTSalDT oDT in aoDT)
        //                {
        //                    switch (nChkShowPdtBarCode) // Check Option พิมพ์รหัสสินค้าหรือบาร์โค้ดในแสดงสลิป  0:ไม่แสดง, 1:แสดง Product Code, 2:แสดง Barcode
        //                    {
        //                        case 1:
        //                            nStartY += 18;
        //                            //oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                            oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));     //*Arm 63-05-12
        //                            break;
        //                        case 2:
        //                            nStartY += 18;
        //                            //oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                            oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));     //*Arm 63-05-12
        //                            break;
        //                        default:
        //                            nStartY += 18;
        //                            //oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                            oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));     //*Arm 63-05-12
        //                            break;
        //                    }

        //                    if (oDT.FTXsdVatType != null || oDT.FTXsdVatType == "1")
        //                    {
        //                        tVat = "V";
        //                    }
        //                    else
        //                    {
        //                        tVat = " ";
        //                    }

        //                    //*Arm 63-05-12
        //                    if (oDT.FCXsdQty > 1)
        //                    {
        //                        nStartY += 18;
        //                        oGraphic.DrawString("  " + oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                        oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar);
        //                        oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF((nWidth - 70) + 60, nStartY, 10, 18), oFormatCenter);
        //                    }
        //                    else
        //                    {
        //                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                        oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar);
        //                        oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF((nWidth - 70) + 60, nStartY, 10, 18), oFormatCenter);
        //                    }
        //                    //+++++++++++++

        //                    //if (nChkShowPdtBarCode > 0) // Option กำหนดให้แสดง PdtCode หรือ Barcode
        //                    //{
        //                    //    nStartY += 18;
        //                    //    oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                    //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

        //                    //    //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
        //                    //    //{
        //                    //    //    nStartY += 18;
        //                    //    //    oGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), nDecShw);
        //                    //    //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //    //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

        //                    //    //    switch (nChkShowPdtBarCode)
        //                    //    //    {
        //                    //    //        case 1:
        //                    //    //            nStartY += 18;
        //                    //    //            oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //            break;
        //                    //    //        case 2:
        //                    //    //            nStartY += 18;
        //                    //    //            oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //            break;

        //                    //    //        default:
        //                    //    //            nStartY += 18;
        //                    //    //            oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //            break;
        //                    //    //    }

        //                    //    //    nStartY += 18;
        //                    //    //    oGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), nDecShw);
        //                    //    //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //    //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //    //}
        //                    //    //else
        //                    //    //{
        //                    //    //    nStartY += 18;
        //                    //    //    oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), nDecShw);
        //                    //    //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //    //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //    //}
        //                    //}
        //                    //else
        //                    //{
        //                    //    if (oDT.FCXsdQty > 1)
        //                    //    {
        //                    //        nStartY += 18;
        //                    //        oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                    //        oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

        //                    //        //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
        //                    //        //{
        //                    //        //    nStartY += 18;
        //                    //        //    oGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), nDecShw);
        //                    //        //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);

        //                    //        //    switch (nChkShowPdtBarCode)
        //                    //        //    {
        //                    //        //        case 1:
        //                    //        //            nStartY += 18;
        //                    //        //            oGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //            break;
        //                    //        //        case 2:
        //                    //        //            nStartY += 18;
        //                    //        //            oGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //            break;

        //                    //        //        default:
        //                    //        //            nStartY += 18;
        //                    //        //            oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //            break;
        //                    //        //    }

        //                    //        //    nStartY += 18;
        //                    //        //    oGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), nDecShw);
        //                    //        //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //        //}
        //                    //        //else
        //                    //        //{
        //                    //        //    nStartY += 18;
        //                    //        //    oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, aoFont[2], Brushes.Black, 0, nStartY);
        //                    //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), nDecShw);
        //                    //        //    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        //    oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //        //}
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, nDecShw);
        //                    //        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdSalePrice, nDecShw);
        //                    //        oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                    //        oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 18), oFormatCenter);
        //                    //    }
        //                    //}

        //                    if (aoPrnDTDis != null)
        //                    {
        //                        foreach (cmlPrnSplipDTDis oDTDis in aoPrnDTDis.Where(c => c.FNXsdSeqNo == oDT.FNXsdSeqNo))
        //                        {
        //                            nStartY += 18;
        //                            switch (oDTDis.FTXddDisChgType)
        //                            {
        //                                case "1":
        //                                case "2":
        //                                    oGraphic.DrawString(tDisc + "(" + oDTDis.FTXddDisChgTxt + ")", aoFont[2], Brushes.Black, 0, nStartY);
        //                                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDTDis.FCXddNet - oDTDis.FCXddValue), nDecShw);
        //                                    //oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                                    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar); //*Arm 63-05-12
        //                                    break;
        //                                case "3":
        //                                case "4":
        //                                    oGraphic.DrawString("  " + tCharge + "(" + oDTDis.FTXddDisChgTxt + ")", aoFont[2], Brushes.Black, 0, nStartY);
        //                                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDTDis.FCXddNet + oDTDis.FCXddValue), nDecShw);
        //                                    //oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                                    oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar); //*Arm 63-05-12
        //                                    break;
        //                            }
        //                        }

        //                    }//End if (aoDTDis != null)
        //                }//End foreach (cmlTPSTSalDT oDT in aoDT)
        //            }// End if (aoDT != null)

        //        }
        //        //++++++++++++++

        //        //*Em 63-03-29 ส่วนลดโปรโมชั่น
        //        oSql.Clear();
        //        //oSql.AppendLine("SELECT (CASE WHEN ISNULL(HDL.FTPmhNameSlip,'') = '' THEN HDL.FTPmhName ELSE HDL.FTPmhNameSlip END) AS FTPmdGrpName,PD.FCXpdDis");
        //        oSql.AppendLine("SELECT FTPmdGrpName, PD.FCXpdDis "); //*Arm 63-05-09
        //        oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
        //        //oSql.AppendLine("INNER JOIN TPMTPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo");
        //        oSql.AppendLine("WHERE PD.FTBchCode = '" + poSalePos.ptBchCode + "' AND PD.FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //        oSql.AppendLine("AND PD.FTXpdCpnText = '' AND PD.FTCpdBarCpn = ''");
        //        oSql.AppendLine("AND PD.FCXpdDis <> 0 ");   //*Em 63-04-12
        //        oSql.AppendLine("AND PD.FTXpdGetType <> '4' "); //*Em 63-04-29
        //        //oSql.AppendLine("GROUP BY HDL.FTPmhNameSlip,HDL.FTPmhName,PD.FCXpdDis");
        //        oSql.AppendLine("GROUP BY FTPmdGrpName,PD.FCXpdDis"); //*Arm 63-05-09
        //        aoPD = oDB.C_GETaDataQuery<cmlTPSTSalPD>(tConnStr, oSql.ToString(), nCmdTime);
        //        if (aoPD != null && aoPD.Count > 0)
        //        {
        //            foreach (cmlTPSTSalPD oPD in aoPD)
        //            {
        //                nStartY += 18;
        //                oGraphic.DrawString("  " + tDisc + " " + oPD.FTPmdGrpName, aoFont[2], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));

        //                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oPD.FCXpdDis, nDecShw);
        //                //oGraphic.DrawString("-" + tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 100, nStartY, 90, 18), oFormatFar);
        //                oGraphic.DrawString("-" + tAmt, aoFont[3], Brushes.Black, new RectangleF(nWidth - 70, nStartY, 60, 18), oFormatFar); //*Arm 63-05-12
        //            }
        //        }
        //        //+++++++++++++++++++++++++

        //        //Arm 63-05-06 Comment Code เก่า
        //        //oSql = new StringBuilder();
        //        //oSql.AppendLine("SELECT FNXsdSeqNo,FTPdtCode,FTXsdPdtName,FTXsdBarCode,FCXsdQty,FCXsdSetPrice,FCXsdNet,FTXsdVatType");
        //        //oSql.AppendLine("FROM TPSTSalDT with(nolock)");
        //        //oSql.AppendLine("WHERE FTBchCode =  '" + poSalePos.ptBchCode + "'");
        //        //oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //        //oSql.AppendLine("ORDER BY FNXsdSeqNo");
        //        //aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(tConnStr, oSql.ToString(), nCmdTime);

        //        //oSql = new StringBuilder();
        //        //oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
        //        //oSql.AppendLine("FROM TPSTSalDTDis");
        //        //oSql.AppendLine("WHERE FTBchCode =  '" + poSalePos.ptBchCode + "'");
        //        //oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //        //oSql.AppendLine("AND FNXddStaDis = 1");
        //        //oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");
        //        //aoDTDis = oDB.C_GETaDataQuery<cmlTPSTSalDTDis>(tConnStr, oSql.ToString(), nCmdTime);

        //        //if (aoDT != null)
        //        //{
        //        //    foreach (cmlTPSTSalDT oDT in aoDT)
        //        //    {
        //        //        nStartY += 15;
        //        //        oGraphic.DrawString(oDT.FTXsdPdtName, aoFont[2], Brushes.Black, 0, nStartY);

        //        //        if (oDT.FTXsdVatType.ToString() == "1")
        //        //        {
        //        //            tVat = "V";
        //        //        }
        //        //        else
        //        //        {
        //        //            tVat = " ";
        //        //        }
        //        //        if (oDT.FCXsdQty > 1)
        //        //        {
        //        //            nStartY += 15;
        //        //            oGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, aoFont[2], Brushes.Black, 0, nStartY);
        //        //            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdNet, nDecShw);
        //        //            oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //        //            oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 15), oFormatCenter);
        //        //        }
        //        //        else
        //        //        {
        //        //            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdNet, nDecShw);
        //        //            oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //        //            oGraphic.DrawString(tVat, aoFont[3], Brushes.Black, new RectangleF(140 + nWidth - 150, nStartY, 10, 15), oFormatCenter);
        //        //        }
        //        //        if (aoDTDis != null)
        //        //        {
        //        //            foreach (cmlTPSTSalDTDis oDTDis in aoDTDis.Where(c => c.FNXsdSeqNo == oDT.FNXsdSeqNo))
        //        //            {
        //        //                nStartY += 15;
        //        //                oGraphic.DrawString("  " + oDTDis.FTXddDisChgTxt, aoFont[2], Brushes.Black, 0, nStartY);
        //        //                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDTDis.FCXddNet - oDTDis.FCXddValue), nDecShw);
        //        //                oGraphic.DrawString(tAmt, aoFont[3], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //        //            }
        //        //        }

        //        //    }

        //        //}
        //        //Arm 63-05-06 Comment Code เก่า

        //        nStartY += 30;

        //        //Total

        //        if (oHD != null)
        //        {
        //            //*Em 63-01-13
        //            if (oHD.FCXshDis != 0 || oHD.FCXshChg != 0 || oHD.FCXshRnd != 0)
        //            {
        //                oGraphic.DrawString("Subtotal", aoFont[4], Brushes.Black, 0, nStartY);
        //                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshTotal, nDecShw);
        //                oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //                nStartY += 18;

        //                //*Arm 63-05-01 - Print HDDis
        //                //==========================================
        //                oSql = new StringBuilder();
        //                oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
        //                oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt, FTXhdRefCode "); //*Arm 63-04-16
        //                oSql.AppendLine("FROM TPSTSalHDDis WITH(NOLOCK) ");
        //                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "' ");
        //                oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'  ");
        //                List<cmlTPSTSalHDDis> aoHDDis = oDB.C_GETaDataQuery<cmlTPSTSalHDDis>(tConnStr, oSql.ToString(), nCmdTime);
        //                if (aoHDDis.Count > 0)
        //                {
        //                    foreach (cmlTPSTSalHDDis oHDDis in aoHDDis)
        //                    {
        //                        if (string.IsNullOrEmpty(oHDDis.FTXhdRefCode))
        //                        {
        //                            //ส่วนลด 
        //                            if (oHDDis.FTXhdDisChgType == "1" || oHDDis.FTXhdDisChgType == "2")
        //                            {
        //                                oGraphic.DrawString(tDisc + "(" + oHDDis.FTXhdDisChgTxt + ")", aoFont[4], Brushes.Black, 0, nStartY);
        //                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, nDecShw); //*Arm 63-04-16
        //                                oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                                nStartY += 18;
        //                            }
        //                            //ชาจน์
        //                            if (oHDDis.FTXhdDisChgType == "3" || oHDDis.FTXhdDisChgType == "4")
        //                            {
        //                                oGraphic.DrawString(tCharge + "(" + oHDDis.FTXhdDisChgTxt + ")", aoFont[4], Brushes.Black, 0, nStartY);
        //                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, nDecShw); //*Arm 63-04-16
        //                                oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                                nStartY += 18;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //Redeem
        //                            if (oHDDis.FTXhdDisChgType == "1")
        //                            {
        //                                oSql = new StringBuilder();
        //                                oSql.AppendLine("SELECT FTRdhDocType");
        //                                oSql.AppendLine("FROM TPSTSalRD WITH(NOLOCK) ");
        //                                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "' ");
        //                                oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'  ");
        //                                oSql.AppendLine("AND FTXrdRefCode = '" + oHDDis.FTXhdRefCode + "'  ");
        //                                string tRdhDocType = oDB.C_DAToExecuteQuery<string>(tConnStr, oSql.ToString(), nCmdTime);
        //                                if (tRdhDocType == "1")
        //                                {
        //                                    oGraphic.DrawString(tRedPdt + "(" + oHDDis.FTXhdDisChgTxt + ")", aoFont[4], Brushes.Black, 0, nStartY);
        //                                }
        //                                else
        //                                {
        //                                    oGraphic.DrawString(tRedDisc + "(" + oHDDis.FTXhdDisChgTxt + ")", aoFont[4], Brushes.Black, 0, nStartY);
        //                                }
        //                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, nDecShw); //*Arm 63-04-16
        //                                oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                                nStartY += 18;
        //                            }
        //                        }

        //                        //Coupon
        //                        if (oHDDis.FTXhdDisChgType == "5" || oHDDis.FTXhdDisChgType == "6")
        //                        {
        //                            oGraphic.DrawString(tRedCpn + "(" + oHDDis.FTXhdDisChgTxt + ")", aoFont[4], Brushes.Black, 0, nStartY);
        //                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, nDecShw); //*Arm 63-04-16
        //                            oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                            nStartY += 18;
        //                        }

        //                    }
        //                }
        //                //================================================================

        //                //*Arm 63-05-06 Comment Code
        //                //if ((decimal)oHD.FCXshDis > (decimal)0)
        //                //{
        //                //    oGraphic.DrawString("Disc", aoFont[4], Brushes.Black, 0, nStartY);
        //                //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshDis, nDecShw);
        //                //    oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //                //    nStartY += 15;
        //                //}

        //                //if ((decimal)oHD.FCXshChg > (decimal)0)
        //                //{
        //                //    oGraphic.DrawString("Charge", aoFont[4], Brushes.Black, 0, nStartY);
        //                //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshChg, nDecShw);
        //                //    oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //                //    nStartY += 15;
        //                //}
        //                //+++++++++++++

        //                if ((decimal)oHD.FCXshRnd > (decimal)0)
        //                {
        //                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshRnd, nDecShw);
        //                    oGraphic.DrawString("Round Rcv: " + tAmt, aoFont[4], Brushes.Black, 0, nStartY);
        //                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oHD.FCXshTotal - oHD.FCXshDis + oHD.FCXshChg), nDecShw);
        //                    oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //                    nStartY += 18;
        //                }
        //            }
        //            //+++++++++++++++

        //            //*Arm 63-04-20  แสดงจำนวนสินค้ารวม
        //            oSql.Clear();
        //            oSql.AppendLine("SELECT SUM(ISNULL(FCXsdQty,0)) FROM TPSTSalDT  with(nolock)");
        //            oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "' ");
        //            oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "' ");
        //            oSql.AppendLine("AND FTXsdStaPdt <> '4' ");
        //            cTotalQty = oDB.C_DAToExecuteQuery<decimal>(tConnStr, oSql.ToString(), nCmdTime);

        //            oGraphic.DrawString("TOTAL " + oSP.SP_SETtDecShwSve(1, (decimal)cTotalQty, nDecShw) + " Items", aoFont[4], Brushes.Black, 0, nStartY);
        //            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshGrand, nDecShw);
        //            oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);

        //            //++++++++++++++++++

        //            //*Arm 63-05-06 Comment Code
        //            ////oGraphic.DrawString("TOTAL", aoFont[4], Brushes.Black, 0, nStartY);
        //            //oGraphic.DrawString("TOTAL (VAT Included)", aoFont[4], Brushes.Black, 0, nStartY); //*Arm 62-10-27 -เพิ่ม Wording  (VAT Included) ใบกำกับภาษีอย่างย่อ
        //            //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshGrand, nDecShw);
        //            //oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);

        //            nStartY += 18;
        //            tAmt = "Vatable : " + oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshVatable, nDecShw);
        //            tAmt += " " + "VAT : " + oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshVat, nDecShw);
        //            oGraphic.DrawString(tAmt, aoFont[4], Brushes.Black, 0, nStartY);
        //        }

        //        oSql.Clear();
        //        oSql.AppendLine("SELECT RCV.FTRcvCode,(CASE WHEN ISNULL(RCVL.FTRcvName,'') = '' THEN (SELECT TOP 1 FTRcvName FROM TFNMRcv_L with(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode) ELSE RCVL.FTRcvName END) AS FTRcvName,");
        //        oSql.AppendLine("RCV.FCXrcUsrPayAmt,FCXrcDep,FCXrcChg,RCV.FTXrcRefNo1,RCV.FTXrcRefNo2 "); //*Net 63-03-28 ยกมาจาก baseline
        //        oSql.AppendLine("FROM TPSTSalRC RCV WITH(NOLOCK)"); //*Arm 63-03-02
        //        oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);
        //        oSql.AppendLine("INNER JOIN TFNMRcv RCVM WITH(NOLOCK) ON RCV.FTRcvCode = RCVM.FTRcvCode AND RCVM.FTFmtCode <> '020'");   //*Em 62-12-27
        //        oSql.AppendLine("WHERE RCV.FTBchCode =  '" + poSalePos.ptBchCode + "'");
        //        oSql.AppendLine("AND RCV.FTXshDocNo = '" + poSalePos.ptXihDocNo + "'"); //*Arm 63-03-05 
        //        oSql.AppendLine("ORDER BY RCV.FNXrcSeqNo");
        //        List<cmlTPSTSalRC> aoRC = oDB.C_GETaDataQuery<cmlTPSTSalRC>(tConnStr, oSql.ToString(), nCmdTime);
        //        if (aoRC != null)
        //        {
        //            foreach (cmlTPSTSalRC oRC in aoRC)
        //            {
        //                nStartY += 18;
        //                if (string.IsNullOrEmpty(oRC.FTXrcRefNo1))
        //                {
        //                    oGraphic.DrawString(oRC.FTRcvName, aoFont[6], Brushes.Black, 0, nStartY);
        //                }
        //                else
        //                {
        //                    if (string.IsNullOrEmpty(oRC.FTXrcRefNo2.Split(';')[oRC.FTXrcRefNo2.Split(';').Length - 1]))
        //                    {
        //                        oGraphic.DrawString(oRC.FTRcvName + "(" + oRC.FTXrcRefNo1 + ")", aoFont[6], Brushes.Black, 0, nStartY);
        //                    }
        //                    else
        //                    {
        //                        oGraphic.DrawString(oRC.FTXrcRefNo2.Split(';')[oRC.FTXrcRefNo2.Split(';').Length - 1] + " (" + oRC.FTXrcRefNo1 + ")", aoFont[6], Brushes.Black, 0, nStartY);
        //                    }

        //                }
        //                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oRC.FCXrcUsrPayAmt, nDecShw);
        //                oGraphic.DrawString(tAmt, aoFont[7], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //                cChange = (decimal)oRC.FCXrcChg;
        //            }

        //            nStartY += 18;
        //            oGraphic.DrawString("Change", aoFont[4], Brushes.Black, 0, nStartY);
        //            tAmt = oSP.SP_SETtDecShwSve(1, cChange, nDecShw);
        //            oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
        //        }

        //        //Print Payment
        //        //oSql = new StringBuilder();
        //        //oSql.AppendLine("SELECT RCV.FTRcvCode,(CASE WHEN ISNULL(RCVL.FTRcvName,'') = '' THEN (SELECT TOP 1 FTRcvName FROM TFNMRcv_L with(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode) ELSE RCVL.FTRcvName END) AS FTRcvName,");
        //        //oSql.AppendLine("RCV.FCXrcUsrPayAmt,FCXrcDep,FCXrcChg ");
        //        //oSql.AppendLine("FROM TPSTSalRC RCV with(nolock) ");
        //        //oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL with(nolock) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + 1);
        //        //oSql.AppendLine("INNER JOIN TFNMRcv RCVM ON RCV.FTRcvCode = RCVM.FTRcvCode AND RCVM.FTFmtCode <> '020'");   //*Em 62-12-27
        //        //oSql.AppendLine("WHERE RCV.FTBchCode =  '" + poSalePos.ptBchCode + "'");
        //        //oSql.AppendLine("AND RCV.FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //        //oSql.AppendLine("ORDER BY RCV.FNXrcSeqNo");

        //        //List<cmlTPSTSalRC> aoRC = oDB.C_GETaDataQuery<cmlTPSTSalRC>(tConnStr, oSql.ToString(), nCmdTime);
        //        //if (aoRC != null)
        //        //{
        //        //    foreach (cmlTPSTSalRC oRC in aoRC)
        //        //    {
        //        //        nStartY += 15;
        //        //        oGraphic.DrawString(oRC.FTRcvName, aoFont[6], Brushes.Black, 0, nStartY);
        //        //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oRC.FCXrcUsrPayAmt, nDecShw);
        //        //        oGraphic.DrawString(tAmt, aoFont[7], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //        //        cChange = (decimal)oRC.FCXrcChg;
        //        //    }
        //        //    nStartY += 15;
        //        //    oGraphic.DrawString("Change", aoFont[4], Brushes.Black, 0, nStartY);
        //        //    tAmt = oSP.SP_SETtDecShwSve(1, cChange, nDecShw);
        //        //    oGraphic.DrawString(tAmt, aoFont[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 15), oFormatFar);
        //        //}

        //        //*Arm 63-05-06 อ้างอิง SO
        //        if (!string.IsNullOrEmpty(oHD.FTXshRefExt))
        //        {
        //            nStartY += 18;
        //            oGraphic.DrawString(tRef + " : " + oHD.FTXshRefExt, aoFont[1], Brushes.Black, 0, nStartY);

        //        }
        //        //++++++++++++++++++++++

        //        //*Arm 63-05-06 - Print Point
        //        oSql = new StringBuilder();
        //        oSql.AppendLine("SELECT FTBchCode, FTXshDocNo,FTXshCardID,FTXshCardNo,FTXshCstName,");
        //        oSql.AppendLine("FTXshCstTel,ISNULL(FCXshCstPnt,0) AS FCXshCstPnt, ISNULL(FCXshCstPntPmt,0) AS FCXshCstPntPmt");
        //        oSql.AppendLine("FROM TPSTSalHDCst WITH(NOLOCK) ");
        //        oSql.AppendLine("WHERE FTBchCode =  '" + poSalePos.ptBchCode + "'");
        //        oSql.AppendLine("AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //        oHDCst = oDB.C_DAToExecuteQuery<cmlTPSTSalHDCst>(tConnStr, oSql.ToString(), nCmdTime);
        //        if (oHDCst != null)
        //        {
        //            string tTxnDocNo = poSalePos.ptXihDocNo; //*Arm 63-05-01
        //            decimal cCstPiontB4Used = 0;    //แต้มก่อนใช้
        //            decimal cSumPnt = 0;  //แต้มรวม

        //            if (oHD.FNXshDocType == 9) //*Arm 63-05-01 การณีบินคืนต้องหา TranSaction 
        //            {
        //                oSql.Clear();
        //                oSql.AppendLine("SELECT FTTxnRefDoc FROM TCNTMemTxnSale with(nolock)");
        //                oSql.AppendLine("WHERE CONVERT(Datetime,FDTxnRefDate,121) = CONVERT(Datetime,'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "',121)");

        //                tTxnDocNo = oDB.C_DAToExecuteQuery<string>(tConnStr, oSql.ToString(), nCmdTime);
        //            }

        //            //Transaction การขาย
        //            oSql = new StringBuilder();
        //            oSql.AppendLine("SELECT * FROM TCNTMemTxnSale with(nolock) WHERE FTTxnRefDoc = '" + tTxnDocNo + "'");
        //            cmlTCNTMemTxnSale oTxnSale = oDB.C_DAToExecuteQuery<cmlTCNTMemTxnSale>(tConnStr, oSql.ToString(), nCmdTime);

        //            nStartY += 30;
        //            oGraphic.DrawString("Mem ID : " + oHD.FTCstCode, aoFont[1], Brushes.Black, 0, nStartY);
        //            nStartY += 18;
        //            oGraphic.DrawString(oHDCst.FTXshCstName == null ? string.Empty : oHDCst.FTXshCstName, aoFont[1], Brushes.Black, 10, nStartY);
        //            nStartY += 18;
        //            oGraphic.DrawString("Card No. : " + oHDCst.FTXshCardNo == null ? " - " : oHDCst.FTXshCardNo, aoFont[1], Brushes.Black, 0, nStartY);

        //            if (oTxnSale != null)
        //            {

        //                if (oTxnSale.FDTxnPntExpired != null)
        //                {
        //                    nStartY += 18;
        //                    oGraphic.DrawString("Expired : " + string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oTxnSale.FDTxnPntExpired)), aoFont[1], Brushes.Black, 0, nStartY);
        //                }
        //                else
        //                {
        //                    nStartY += 18;
        //                    oGraphic.DrawString("Expired : ", aoFont[1], Brushes.Black, 0, nStartY);
        //                }

        //                nStartY += 18;
        //                oGraphic.DrawString("Last Pt.", aoFont[1], Brushes.Black, 0, nStartY);
        //                oGraphic.DrawString("Reg. Pt.", aoFont[1], Brushes.Black, 70, nStartY);
        //                oGraphic.DrawString("Promo Pt.", aoFont[1], Brushes.Black, 140, nStartY);
        //                oGraphic.DrawString("Total Point", aoFont[1], Brushes.Black, 210, nStartY);


        //                //หา Point ก่อนใช้
        //                oSql.Clear();
        //                oSql.AppendLine("SELECT count(*) FROM TCNTMemTxnRedeem with(nolock) WHERE FTRedRefDoc = '" + tTxnDocNo + "'");

        //                if (oDB.C_DAToExecuteQuery<int>(tConnStr, oSql.ToString(), nCmdTime) > 0)
        //                {
        //                    oSql.Clear();
        //                    oSql.AppendLine("SELECT FCRedPntB4Bill FROM TCNTMemTxnRedeem with(nolock) WHERE FTRedRefDoc = '" + tTxnDocNo + "'");

        //                    cCstPiontB4Used = oDB.C_DAToExecuteQuery<decimal>(tConnStr, oSql.ToString(), nCmdTime);
        //                }
        //                else
        //                {
        //                    oSql.Clear();
        //                    oSql.AppendLine("SELECT FCTxnPntB4Bill FROM TCNTMemTxnSale with(nolock) WHERE FTTxnRefDoc = '" + tTxnDocNo + "'");

        //                    cCstPiontB4Used = oDB.C_DAToExecuteQuery<decimal>(tConnStr, oSql.ToString(), nCmdTime);
        //                }

        //                //*Arm 63-05-07
        //                if (oHD.FNXshDocType != 9)
        //                {
        //                    //แต้มที่ใช้
        //                    oSql.Clear();
        //                    oSql.AppendLine("SELECT * FROM TCNTMemTxnRedeem with(nolock) WHERE FTRedRefDoc = '" + tTxnDocNo + "'");
        //                    cmlTCNTMemTxnRedeem oTxnRedeem = oDB.C_DAToExecuteQuery<cmlTCNTMemTxnRedeem>(tConnStr, oSql.ToString(), nCmdTime);
        //                    if (oTxnRedeem != null)
        //                    {
        //                        nStartY += 18;
        //                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)oTxnRedeem.FCRedPntB4Bill, 0), aoFont[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
        //                        decimal cRedPnt = (decimal)(oTxnRedeem.FCRedPntBillQty * (-1));
        //                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRedPnt, 0), aoFont[1], Brushes.Black, 70, nStartY);
        //                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), aoFont[1], Brushes.Black, 140, nStartY);
        //                        cSumPnt = (decimal)(oTxnRedeem.FCRedPntB4Bill + cRedPnt);
        //                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), aoFont[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29
        //                    }
        //                    //แต้มที่ได้
        //                    nStartY += 18;
        //                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)oTxnSale.FCTxnPntB4Bill, 0), aoFont[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
        //                    decimal cSalePnt = (decimal)(oTxnSale.FCTxnPntBillQty - oHDCst.FCXshCstPntPmt);
        //                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSalePnt, 0), aoFont[1], Brushes.Black, 70, nStartY);
        //                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPntPmt, 0), aoFont[1], Brushes.Black, 140, nStartY);
        //                    cSumPnt = (decimal)(oTxnSale.FCTxnPntB4Bill + (cSalePnt + oHDCst.FCXshCstPntPmt));
        //                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), aoFont[1], Brushes.Black, 210, nStartY);

        //                    oTxnRedeem = null;
        //                }
        //                else
        //                {
        //                    decimal cRefundRed = 0;
        //                    oSql.Clear();
        //                    oSql.AppendLine("SELECT SUM(ISNULL(FNXrdPntUse,0)) AS FNXrdPntUse  FROM TPSTSalRD WITH(NOLOCK) WHERE FTBchCode =  '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + poSalePos.ptXihDocNo + "'");
        //                    cRefundRed = oDB.C_DAToExecuteQuery<decimal>(tConnStr, oSql.ToString(), nCmdTime);

        //                    //แต้มที่ใช้คืน
        //                    nStartY += 18;
        //                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)cCstPiontB4Used, 0), aoFont[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
        //                    decimal cRefundPnt = ((cRefundRed - oHDCst.FCXshCstPnt) * (-1));
        //                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRefundPnt, 0), aoFont[1], Brushes.Black, 70, nStartY);
        //                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPntPmt, 0), aoFont[1], Brushes.Black, 140, nStartY);
        //                    cSumPnt = (cCstPiontB4Used + (cRefundPnt + oHDCst.FCXshCstPntPmt));
        //                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), aoFont[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29

        //                    cCstPiontB4Used = cSumPnt;

        //                    //แต้มที่ได้คืน

        //                    if (cRefundRed > 0)
        //                    {
        //                        nStartY += 18;
        //                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)cCstPiontB4Used, 0), aoFont[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
        //                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)cRefundRed, 0), aoFont[1], Brushes.Black, 70, nStartY);
        //                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), aoFont[1], Brushes.Black, 140, nStartY);
        //                        cSumPnt = (decimal)(cCstPiontB4Used + cRefundRed);
        //                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), aoFont[1], Brushes.Black, 210, nStartY);
        //                    }
        //                }
        //                //++++++++++++++++
        //            }
        //            oTxnSale = null;
        //        }

        //        //*Arm 63-05-08 Print ชื่อผู้ขาย
        //        oSql.Clear();
        //        oSql.AppendLine("SELECT FTUsrName FROM TCNMUser_L with(nolock) WHERE FTUsrCode ='" + oHD.FTUsrCode + "' ");
        //        string tUserName = oDB.C_DAToExecuteQuery<string>(tConnStr, oSql.ToString(), nCmdTime);
        //        nStartY += 30;
        //        oGraphic.DrawString("User : " + tUserName, aoFont[1], Brushes.Black, 0, nStartY);
        //        //+++++++++++++


        //        //*Arm 63-05-06 Print อ้างอิงบิลขาย
        //        if (oHD.FNXshDocType == 9)
        //        {
        //            //อ้างอิง
        //            nStartY += 30;
        //            tMsg = tRefundPdt;
        //            if (!string.IsNullOrEmpty(oHD.FTXshRefInt))
        //            {
        //                tMsg += tRefer + ":" + oHD.FTXshRefInt;
        //            }
        //            oGraphic.DrawString(tMsg, aoFont[1], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

        //        }
        //        //++++++++++++++++++


        //        //*Em 62-09-06
        //        //Remark
        //        if (!string.IsNullOrEmpty(oHD.FTXshRmk))
        //        {
        //            nStartY += 18;
        //            tPrint = oHD.FTXshRmk;
        //            atRmk = tPrint.Split((char)10);
        //            foreach (string tStr in atRmk)
        //            {
        //                nStartY += 18;
        //                oGraphic.DrawString(tStr, aoFont[4], Brushes.Black, 0, nStartY);
        //            }
        //        }
        //        nStartY += 30;
        //        nStartY = oMsg.C_GETnSlipMsg(tConnStr, poPos.FTSmgCode, "2", oGraphic, nWidth, nStartY, aoFont[0]); //*Arm 63-05-06
        //        //nStartY = oMsg.C_GETnSlipMsg(tConnStr, poPos.FTSmgCode, "2", oGraphic, nWidth, nStartY, aoFont[8]);

        //        // Footer Slip Msg
        //        //nStartY += 15;
        //        //cSale.C_PRNxBarcode(ref oGraphic, tW_DocNo, nWidth, nStartY);
        //        //nStartY += 30;
        //        //tDataA = tW_DocNo;
        //        //tDataB = tW_DocNo + "|" + oHD.FTCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + Convert.ToDateTime(oHD.FDXshDocDate.Value).ToString("yyyy-MM-dd HH:mm:ss");
        //        //tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
        //        //cSale.C_PRNxQRCode(ref oGraphic, tDataA + "|" + tDataB, nWidth, nStartY);

        //        Rectangle oCropRect = new Rectangle(0, 0, 280, nStartY + 18);
        //        Bitmap oBitmapEJ = new Bitmap(280, nStartY + 5, PixelFormat.Format64bppArgb);
        //        using (Graphics oGrp = Graphics.FromImage(oBitmapEJ))
        //        {
        //            oGrp.DrawImage(oNewBitmap, oCropRect, oCropRect, GraphicsUnit.Pixel);
        //        }
        //        tPathFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
        //        tPathFile = Directory.GetParent(Directory.GetParent(tPathFile).FullName).FullName;
        //        tPathFile += @"\AdaEJ\Image\" + Convert.ToDateTime(oHD.FDXshDocDate).ToString("yyMMdd");

        //        if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "DOC", tPathFile, poSalePos.ptConnStr, (int)poShopDB.nCommandTimeOut))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception oEx)
        //    {
        //        new cLog().C_WRTxLog("cEJ", "C_GENbSlip : " + oEx.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        if (oGraphic != null)
        //            oGraphic.Dispose();

        //        if (oFormatFar != null)
        //            oFormatFar.Dispose();

        //        if (oFormatCenter != null)
        //            oFormatCenter.Dispose();

        //        oGraphic = null;
        //        oFormatFar = null;
        //        oFormatCenter = null;
        //        aoDT = null;
        //        aoDTDis = null;
        //        aoPrnDTDis = null;  //*Arm 63-05-07
        //        oHDCst = null;      //*Arm 63-05-07
        //        aoPD = null;        //*Arm 63-05-07
        //        aoPrnDTDis = null;  //*Arm 63-05-07
        //        oMsg = null;
        //        tLine = null;
        //        //oSP.SP_CLExMemory();
        //    }
        //}
        public void C_PRNxQRCode(ref Graphics poGraphic, string ptStrBar, int pnWidth, int pnY)
        {
            try
            {
                QrCodeEncodingOptions oOption = new QrCodeEncodingOptions { DisableECI = true, CharacterSet = "UTF-8", Width = 150, Height = 150 };
                BarcodeWriter oWrite = new BarcodeWriter();
                oWrite.Format = BarcodeFormat.QR_CODE;
                oWrite.Options = oOption;

                Image oImg = new Bitmap(oWrite.Write(ptStrBar));
                poGraphic.DrawImage(oImg, new Rectangle((pnWidth - oImg.Width) / 2, pnY, oImg.Width, oImg.Height));
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRNxQRCode : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }
        public void C_PRNxBarcode(ref Graphics poGraphic, string ptStrBar, int pnWidht, int pnY)
        {
            try
            {
                Barcode oBar = new Barcode();
                int nHeight = 20;
                int nWidth = 200;

                oBar.Alignment = AlignmentPositions.CENTER;
                oBar.IncludeLabel = false;
                oBar.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
                oBar.LabelPosition = LabelPositions.BOTTOMCENTER;
                Image oImg = oBar.Encode(TYPE.CODE128, ptStrBar, Color.Black, Color.White, nWidth, nHeight);
                poGraphic.DrawImage(oImg, new Rectangle((pnWidht - oImg.Width) / 2, pnY, oImg.Width, oImg.Height));
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRNxBarcode : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get ข้อมูลบิลขาย
        /// </summary>
        /// <param name="ptBchCode"></param>
        /// <param name="ptDocNo"></param>
        /// <param name="ptRefDocNo"></param>
        /// <param name="ptStaSumPrn"></param>
        /// <param name="pbAlwPrnVoid"></param>
        /// <param name="ptConnStr"></param>
        /// <param name="pnTimeOut"></param>
        /// <returns></returns>
        public DataSet C_GETxDataPrint(string ptBchCode, string ptDocNo, string ptStaSumPrn, bool pbAlwPrnVoid, string ptConnStr, int pnTimeOut)
        {
            cDatabase oDB;
            StringBuilder oSql;
            string tTblSalHD;
            string tTblSalHDCst;
            string tTblSalHDDis;
            string tTblSalDT;
            string tTblSalDTDis;
            string tTblSalRC;
            string tTblSalRD;
            string tTblSalPD;
            string tTblTxnSal;
            string tTblTxnRD;
            try
            {
                tTblSalHD = "TPSTSalHD";
                tTblSalHDCst = "TPSTSalHDCst";
                tTblSalHDDis = "TPSTSalHDDis";
                tTblSalDT = "TPSTSalDT";
                tTblSalDTDis = "TPSTSalDTDis";
                tTblSalRC = "TPSTSalRC";
                tTblSalRD = "TPSTSalRD";
                tTblSalPD = "TPSTSalPD";
                tTblTxnSal = "TCNTMemTxnSale";
                tTblTxnRD = "TCNTMemTxnRedeem";
                oDB = new cDatabase();
                oSql = new StringBuilder();

                oSql.Clear();
                // Table 1 & 2
                // 1 :อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                // 2 :ไม่อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                if (ptStaSumPrn == "1")
                {
                    // Table 1
                    // รายการสินค้า
                    oSql.AppendLine("SELECT DT1.FTPdtCode, ");
                    oSql.AppendLine("DT1.FTXsdPdtName, ");
                    oSql.AppendLine("DT1.FTXsdBarCode, ");
                    oSql.AppendLine("SUM(DT1.FCXsdQty) AS FCXsdQty,");
                    oSql.AppendLine("DT1.FCXsdSetPrice,");
                    oSql.AppendLine("SUM(DT1.FCXsdNet) AS FCXsdNet,");
                    oSql.AppendLine("DT1.FTXsdVatType,");
                    oSql.AppendLine("SUM(DT1.FCXsdAmtB4DisChg) AS FCXsdAmtB4DisChg");
                    oSql.AppendLine("FROM(SELECT(SELECT TOP 1 FNXsdSeqNo FROM " + tTblSalDT + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FTPdtCode = DT.FTPdtCode AND  FTXsdBarCode = DT.FTXsdBarCode ORDER BY FNXsdSeqNo ASC) AS FNXsdSeqNo,");
                    oSql.AppendLine("        DT.FTPdtCode,");
                    oSql.AppendLine("        DT.FTXsdPdtName,");
                    oSql.AppendLine("        DT.FTXsdBarCode,");
                    oSql.AppendLine("        DT.FCXsdQty AS FCXsdQty,");
                    oSql.AppendLine("        DT.FCXsdSetPrice,");
                    oSql.AppendLine("        DT.FCXsdNet AS FCXsdNet,");
                    oSql.AppendLine("        DT.FTXsdVatType,");
                    oSql.AppendLine("        DT.FCXsdAmtB4DisChg AS FCXsdAmtB4DisChg");
                    oSql.AppendLine("    FROM " + tTblSalDT + " DT WITH(NOLOCK)");
                    oSql.AppendLine("    WHERE DT.FTBchCode = '" + ptBchCode + "'");
                    oSql.AppendLine("    AND DT.FTXshDocNo = '" + ptDocNo + "' ");

                    if (pbAlwPrnVoid == false)
                    {
                        oSql.AppendLine(" AND DT.FTXsdStaPdt <> '4' ");
                    }

                    oSql.AppendLine(") AS DT1 ");
                    oSql.AppendLine("GROUP BY DT1.FNXsdSeqNo, DT1.FTPdtCode,DT1.FTXsdPdtName,DT1.FTXsdBarCode, ");
                    oSql.AppendLine("DT1.FCXsdSetPrice,DT1.FTXsdVatType ");
                    oSql.AppendLine("ORDER BY DT1.FNXsdSeqNo ASC ");

                    // Table 2
                    // ส่วนลดรายการ
                    oSql.AppendLine("SELECT (SELECT FTXsdBarCode FROM " + tTblSalDT + " WHERE FTBchCode = DTDis.FTBchCode AND FTXshDocNo = DTDis.FTXshDocNo AND FNXsdSeqNo = DTDis.FNXsdSeqNo ) AS FTXsdBarCode,");
                    oSql.AppendLine("DTDis.FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    oSql.AppendLine("FROM " + tTblSalDTDis + " DTDis");
                    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + ptBchCode + "'");
                    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + ptDocNo + "'");
                    oSql.AppendLine("AND FNXddStaDis = 1");
                    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
                    oSql.AppendLine("ORDER BY FDXddDateIns");

                }
                else
                {
                    // Table 1
                    // รายการสินค้า
                    oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FCXsdQty,");
                    oSql.AppendLine("DT.FTPdtCode,DT.FTXsdBarCode,");
                    oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdNet ,DT.FTXsdVatType,DT.FCXsdAmtB4DisChg");
                    oSql.AppendLine("FROM " + tTblSalDT + " DT WITH(NOLOCK)");
                    oSql.AppendLine("WHERE DT.FTBchCode = '" + ptBchCode + "'");
                    oSql.AppendLine("AND DT.FTXshDocNo = '" + ptDocNo + "'");

                    if (pbAlwPrnVoid == false)
                    {
                        oSql.AppendLine(" AND DT.FTXsdStaPdt <> '4' ");
                    }

                    oSql.AppendLine($"ORDER BY DT.FNXsdSeqNo");

                    // Table 2
                    // ส่วนลดรายการ
                    oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    oSql.AppendLine("FROM " + tTblSalDTDis + " DTDis WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode =  '" + ptBchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + ptDocNo + "'");
                    oSql.AppendLine("AND FNXddStaDis = 1");
                    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
                    oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");

                }

                // Table 3
                // โปรโมชั่น
                oSql.AppendLine("SELECT (CASE WHEN ISNULL(HDL.FTPmhNameSlip,'') = '' THEN HDL.FTPmhName ELSE HDL.FTPmhNameSlip END) AS FTPmdGrpName,PD.FCXpdDis");
                oSql.AppendLine("FROM " + tTblSalPD + " PD WITH(NOLOCK)");
                //oSql.AppendLine("INNER JOIN TPMTPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo");
                oSql.AppendLine("INNER JOIN TCNTPdtPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo"); //*Net 63-07-19 ใช้ตารางหลังบ้าน
                oSql.AppendLine("WHERE PD.FTBchCode = '" + ptBchCode + "' AND PD.FTXshDocNo = '" + ptDocNo + "'");
                oSql.AppendLine("AND PD.FTXpdCpnText = '' AND PD.FTCpdBarCpn = ''");
                oSql.AppendLine("AND PD.FCXpdDis <> 0 ");
                oSql.AppendLine("AND PD.FTXpdGetType <> '4' ");
                oSql.AppendLine("GROUP BY HDL.FTPmhNameSlip,HDL.FTPmhName,PD.FCXpdDis");

                // Table 4
                // รวมเงินท้ายบิล
                oSql.AppendLine("Declare @FTXhdRefCode varchar(30);");
                oSql.AppendLine("SELECT CONVERT(VARCHAR,HD.FDXshDocDate,120) AS FDXshDocDate,HD.FCXshTotal,HD.FCXshTotalNV,HD.FCXshTotalNoDis,HD.FCXshTotalB4DisChgV,HD.FCXshTotalB4DisChgNV,HD.FTXshDisChgTxt,");
                oSql.AppendLine("HD.FCXshDis,HD.FCXshChg,HD.FCXshTotalAfDisChgV,HD.FCXshTotalAfDisChgNV,HD.FCXshRefAEAmt,HD.FCXshAmtV,HD.FCXshAmtNV,");
                oSql.AppendLine("HD.FNXshDocType, HD.FTXshRefInt,");
                oSql.AppendLine("HD.FCXshVat,HD.FCXshVatable,HD.FCXshGrand,HD.FTXshRmk,HD.FCXshRnd,HD.FTCstCode, HD.FTXshRefExt,HD.FTUsrCode,User_L.FTUsrName,HD.FNXshDocPrint,");
                oSql.AppendLine("LEFT(CONVERT(VARCHAR,CstC.FDCstCrdExpire,120),10) AS FDCstCrdExpire,");
                oSql.AppendLine("CASE WHEN ISNULL(TXNRed.FCRedPntB4Bill,0) <> 0 THEN TXNRed.FCRedPntB4Bill");
                oSql.AppendLine("WHEN ISNULL(TXNSale.FCTxnPntB4Bill,0) <> 0 THEN TXNSale.FCTxnPntB4Bill ");
                oSql.AppendLine("ELSE (SELECT TOP 1 FCTxnPntQty FROM TCNTMemPntActive WHERE FTMemCode=HD.FTCstCode AND FTCgpCode=Cst.FTCgpCode ORDER BY FDTxnPntLast DESC) ");
                oSql.AppendLine("END AS FCPntB4Bill");
                oSql.AppendLine("FROM " + tTblSalHD + " HD WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMUser_L User_L ON HD.FTUsrCode = User_L.FTUsrCode ");
                oSql.AppendLine("LEFT JOIN TCNMCstCard CstC ON CstC.FTCstCode = HD.FTCstCode ");
                oSql.AppendLine("LEFT JOIN TCNMCst Cst ON Cst.FTCstCode = HD.FTCstCode ");
                if (ptDocNo.StartsWith("R"))
                {
                    oSql.AppendLine($"LEFT JOIN {tTblTxnSal} TXNSale ON TXNSale.FTTxnRefInt = HD.FTXshRefInt ");
                    oSql.AppendLine($"LEFT JOIN {tTblTxnRD} TXNRed ON TXNRed.FTRedRefInt = HD.FTXshRefInt ");
                }
                else
                {
                    oSql.AppendLine($"LEFT JOIN {tTblTxnSal} TXNSale ON TXNSale.FTTxnRefDoc = HD.FTXshDocNo ");
                    oSql.AppendLine($"LEFT JOIN {tTblTxnRD} TXNRed ON TXNRed.FTRedRefDoc = HD.FTXshDocNo ");
                }
                oSql.AppendLine("WHERE HD.FTBchCode =  '" + ptBchCode + "'");
                oSql.AppendLine("AND HD.FTXshDocNo = '" + ptDocNo + "'");

                // Table 5
                // ส่วนลดท้ายบิล
                oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt, FTXhdRefCode = @FTXhdRefCode ");
                oSql.AppendLine("FROM " + tTblSalHDDis + " WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + ptDocNo + "'  ");

                // Table 6
                // Redeem ส่วนลด
                oSql.AppendLine("SELECT FTRdhDocType");
                oSql.AppendLine("FROM " + tTblSalRD + " WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + ptDocNo + "'  ");
                oSql.AppendLine("AND FTXrdRefCode = @FTXhdRefCode  ");

                // Table 7
                // หาจำนวนสินค้าทั้งหมด
                oSql.AppendLine("SELECT SUM(ISNULL(FCXsdQty,0)) AS FCXsdQty FROM " + tTblSalDT + " with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + ptDocNo + "' ");
                oSql.AppendLine("AND FTXsdStaPdt <> '4' ");

                // Table 8
                // ชำระเงิน
                oSql.AppendLine("SELECT RCV.FTRcvCode,(CASE WHEN ISNULL(RCVL.FTRcvName,'') = '' THEN (SELECT TOP 1 FTRcvName FROM TFNMRcv_L with(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode) ELSE RCVL.FTRcvName END) AS FTRcvName,");
                oSql.AppendLine("RCV.FCXrcUsrPayAmt,FCXrcDep,FCXrcChg,RCV.FTXrcRefNo1,RCV.FTXrcRefNo2 ");
                oSql.AppendLine("FROM " + tTblSalRC + " RCV WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + nC_Language);
                oSql.AppendLine("INNER JOIN TFNMRcv RCVM WITH(NOLOCK) ON RCV.FTRcvCode = RCVM.FTRcvCode AND RCVM.FTFmtCode <> '020'");
                oSql.AppendLine("WHERE RCV.FTBchCode =  '" + ptBchCode + "'");
                oSql.AppendLine("AND RCV.FTXshDocNo = '" + ptDocNo + "'");
                oSql.AppendLine("ORDER BY RCV.FNXrcSeqNo");

                // Table 9
                // ข้อมูลลูกค้า
                oSql.AppendLine("SELECT HDCst.FTBchCode, HDCst.FTXshDocNo,HDCst.FTXshCardID,HDCst.FTXshCardNo,HDCst.FTXshCstName,");
                oSql.AppendLine("HDCst.FTXshCstTel,ISNULL(HDCst.FCXshCstPnt,0) AS FCXshCstPnt, ISNULL(HDCst.FCXshCstPntPmt,0) AS FCXshCstPntPmt");
                oSql.AppendLine("FROM " + tTblSalHDCst + " HDCst WITH(NOLOCK) ");
                oSql.AppendLine("WHERE HDCst.FTBchCode =  '" + ptBchCode + "'");
                oSql.AppendLine("AND HDCst.FTXshDocNo = '" + ptDocNo + "'");

                // Table 10
                // แต้มการขาย
                if (ptDocNo.StartsWith("R"))
                {
                    oSql.AppendLine($"SELECT FCTxnPntBillQty,FTMemCode FROM {tTblTxnSal} WHERE FTTxnRefInt = '{ptDocNo}'");
                }
                else
                {
                    oSql.AppendLine($"SELECT FCTxnPntBillQty,FTMemCode FROM {tTblTxnSal} WHERE FTTxnRefDoc = '{ptDocNo}'");
                }

                // Table 11
                // แต้มการ Redeem
                if (ptDocNo.StartsWith("R"))
                {
                    oSql.AppendLine($"SELECT FCRedPntBillQty,FTMemCode FROM {tTblTxnRD} WHERE FTRedRefInt = '{ptDocNo}'");
                }
                else
                {
                    oSql.AppendLine($"SELECT FCRedPntBillQty,FTMemCode FROM {tTblTxnRD} WHERE FTRedRefDoc = '{ptDocNo}'");
                }

                // Table 12
                // แต้มที่ใช้
                oSql.AppendLine("SELECT ISNULL(SUM(ISNULL(FNXrdPntUse,0)),0) AS FNXrdPntUse  FROM " + tTblSalRD + " WITH(NOLOCK) WHERE FTBchCode =  '" + ptBchCode + "' AND FTXshDocNo = '" + ptDocNo + "'");

                // Table 13
                // พิมพ์สิทธิ์
                oSql.AppendLine("SELECT DISTINCT FTPmhDocNo,FTXpdCpnText,FCXpdGetQtyDiv ");
                oSql.AppendLine("FROM " + tTblSalPD + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode =  '" + ptBchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + ptDocNo + "'");
                oSql.AppendLine("AND ISNULL(FTXpdCpnText,'') <> ''");

                // Table 14
                // พิมพ์คูปอง
                oSql.AppendLine("SELECT DISTINCT PD.FTPmhDocNo,PD.FTCpdBarCpn,PD.FCXpdGetQtyDiv ,ISNULL(IMG.FTImgObj,'') AS FTImgPath");
                oSql.AppendLine("FROM " + tTblSalPD + " PD WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNTPdtPmtCG CG WITH(NOLOCK) ON CG.FTPmhDocNo = PD.FTPmhDocNo AND FTPgtStaGetType = '6'");
                oSql.AppendLine("LEFT JOIN TCNMImgObj IMG WITH(NOLOCK) ON IMG.FTImgTable = 'TFNTCouponHD' AND IMG.FTImgRefID = CG.FTCphDocNo");
                oSql.AppendLine("WHERE PD.FTBchCode =  '" + ptBchCode + "'");
                oSql.AppendLine("AND PD.FTXshDocNo = '" + ptDocNo + "'");
                oSql.AppendLine("AND ISNULL(PD.FTCpdBarCpn,'') <> ''");

                return oDB.C_GEToDataSetQuery(ptConnStr, oSql.ToString(), pnTimeOut);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cEJ", "C_GETxDataPrint : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
            }
            return null;
        }
        private void C_PRNxPrintDT(ref Graphics poGraphic, cmlTCNMPos poPos, cmlRcvSalePos poSalePos, ref int pnStartY, int pnWidth, DataSet paoTblPrn)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalDT> aoDT;
            List<cmlTPSTSalDTDis> aoDTDis;
            List<cmlPrnSplipDTDis> aoPrnDTDis;
            List<cmlTPSTSalPD> aoPD;
            string tAmt, tVat;
            cSP oSP = new cSP();
            StringFormat oFormatFar = null, oFormatCenter = null;

            DataTable oDbTblDT = new DataTable();
            DataTable oDbTblDTDis = new DataTable();
            DataTable oDbTblPD = new DataTable();
            int nRownum = 0;
            try
            {
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };
                oDbTblDT = paoTblPrn.Tables[0];
                oDbTblDTDis = paoTblPrn.Tables[1];
                oDbTblPD = paoTblPrn.Tables[2];


                // สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต +++++++
                if (poPos.FTPosStaSumPrn == "1") // 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                {
                    if (oDbTblDT != null)
                    {
                        foreach (DataRow oDT in oDbTblDT.Rows)
                        {
                            string tRownum = "";
                            if (bC_PrnRowNum)
                            {
                                nRownum = oDbTblDT.Rows.IndexOf(oDT) + 1;
                                tRownum = nRownum + ".";
                            }
                            else
                            {
                                tRownum = "";
                            }

                            switch (nC_ChkShowPdtBarCode)
                            {
                                case 1:
                                    pnStartY += 18;
                                    poGraphic.DrawString(tRownum + oDT["FTPdtCode"].ToString() + " " + oDT["FTXsdPdtName"].ToString(), aoC_Font[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    break;
                                case 2:
                                    pnStartY += 18;
                                    poGraphic.DrawString(tRownum + oDT["FTXsdBarCode"].ToString() + " " + oDT["FTXsdPdtName"].ToString(), aoC_Font[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    break;

                                default:
                                    pnStartY += 18;
                                    poGraphic.DrawString(tRownum + oDT["FTXsdPdtName"].ToString(), aoC_Font[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    break;
                            }

                            if (oDT["FTXsdVatType"].ToString() != null || oDT["FTXsdVatType"].ToString() == "1")
                            {
                                tVat = "V";
                            }
                            else
                            {
                                tVat = " ";
                            }

                            if (Convert.ToInt32(oDT["FCXsdQty"]) > 1)
                            {
                                pnStartY += 18;
                                poGraphic.DrawString("  " + oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDT["FCXsdQty"]), nC_DecShow) + " x " + oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDT["FCXsdSetPrice"]), nC_DecShow), aoC_Font[2], Brushes.Black, 0, pnStartY);
                                tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDT["FCXsdAmtB4DisChg"]), nC_DecShow);
                                poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, aoC_Font[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            else
                            {
                                tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDT["FCXsdAmtB4DisChg"]), nC_DecShow);
                                poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, aoC_Font[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            //+++++++++++++


                            if (oDbTblDTDis != null)
                            {
                                decimal cAmt = (decimal)(oDT["FCXsdAmtB4DisChg"]);
                                decimal cDis = 0; //เก็บผลรวมส่วนลด
                                decimal cChg = 0; //เก็บผลรวมชาจน์
                                int nRow = 0;
                                foreach (DataRow oDTDis in oDbTblDTDis.Rows)
                                {
                                    if (oDTDis["FTXsdBarCode"].ToString() == oDT["FTXsdBarCode"].ToString())
                                    {
                                        switch (oDTDis["FTXddDisChgType"].ToString())
                                        {
                                            case "1":
                                            case "2":
                                                cDis += (decimal)(oDTDis["FCXddValue"]);
                                                break;
                                            case "3":
                                            case "4":
                                                cChg += (decimal)(oDTDis["FCXddValue"]);
                                                break;
                                        }
                                        nRow++;
                                    }
                                }

                                if (nRow > 0)   //มี Transaction ส่วนลดรายการ
                                {
                                    if (cDis > 0)     // แสดง แสดงส่วนลด
                                    {
                                        pnStartY += 18;
                                        poGraphic.DrawString("  " + oC_Resource.GetString("tDis") + " (" + cDis + ")", aoC_Font[2], Brushes.Black, 0, pnStartY);
                                        cAmt = (cAmt - cDis);
                                        tAmt = oSP.SP_SETtDecShwSve(1, cAmt, nC_DecShow);
                                        //poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                        poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                    }

                                    if (cChg > 0)   // แสดงชาจน์
                                    {
                                        pnStartY += 18;
                                        poGraphic.DrawString("  " + oC_Resource.GetString("tChg") + " (" + cChg + ")", aoC_Font[2], Brushes.Black, 0, pnStartY);
                                        cAmt = (cAmt + cChg);
                                        tAmt = oSP.SP_SETtDecShwSve(1, cAmt, nC_DecShow);
                                        //poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                        poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                    }
                                }//End  Transaction ส่วนลดรายการ
                            }//End if (aoDTDis != null)
                        }//End foreach (cmlTPSTSalDT oDT in aoDT)
                    }//End if (aoDT != null)

                }//End 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์ 
                else
                {

                    if (oDbTblDT != null)
                    {
                        foreach (DataRow oDT in oDbTblDT.Rows)
                        {

                            //+++++++++++++++

                            // พิมพ์รหัสสินค้าหรือบาร์โค้ดในแสดงสลิป 0:ไม่แสดง, 1:แสดง Product Code, 2:แสดง Barcode
                            switch (nC_ChkShowPdtBarCode)
                            {
                                case 1:
                                    pnStartY += 18;
                                    poGraphic.DrawString(oDT["FTPdtCode"].ToString() + " " + oDT["FTXsdPdtName"].ToString(), aoC_Font[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    break;
                                case 2:
                                    pnStartY += 18;
                                    poGraphic.DrawString(oDT["FTXsdBarCode"].ToString() + " " + oDT["FTXsdPdtName"].ToString(), aoC_Font[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    break;

                                default:
                                    pnStartY += 18;
                                    poGraphic.DrawString(oDT["FTXsdPdtName"].ToString(), aoC_Font[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    break;
                            }

                            if (oDT["FTXsdVatType"].ToString() != null || oDT["FTXsdVatType"].ToString() == "1")
                            {
                                tVat = "V";
                            }
                            else
                            {
                                tVat = " ";
                            }

                            if (Convert.ToInt32(oDT["FCXsdQty"]) > 1)
                            {
                                pnStartY += 18;
                                poGraphic.DrawString("  " + oSP.SP_SETtDecShwSve(1, (decimal)(oDT["FCXsdQty"]), nC_DecShow) + " x " + oSP.SP_SETtDecShwSve(1, (decimal)(oDT["FCXsdSetPrice"]), nC_DecShow), aoC_Font[2], Brushes.Black, 0, pnStartY);
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT["FCXsdAmtB4DisChg"]), nC_DecShow);
                                poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, aoC_Font[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            else
                            {
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT["FCXsdAmtB4DisChg"]), nC_DecShow);
                                poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, aoC_Font[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            //+++++++++++++

                            //+++++++++++++
                            if (oDbTblDTDis != null)
                            {
                                DataRow[] oDR = oDbTblDTDis.Select("FNXsdSeqNo = " + (int)oDT["FNXsdSeqNo"]);
                                foreach (DataRow oDTDis in oDR) // aoDTDis.Where(c => c.FNXsdSeqNo == oDT.FNXsdSeqNo))
                                {
                                    pnStartY += 18;
                                    switch (oDTDis["FTXddDisChgType"].ToString())
                                    {
                                        case "1":
                                        case "2":
                                            poGraphic.DrawString("  " + oC_Resource.GetString("tDis") + " (" + oDTDis["FTXddDisChgTxt"].ToString() + ")", aoC_Font[2], Brushes.Black, 0, pnStartY);
                                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((decimal)(oDTDis["FCXddNet"]) - (decimal)(oDTDis["FCXddValue"])), nC_DecShow);
                                            poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                            break;
                                        case "3":
                                        case "4":
                                            poGraphic.DrawString("  " + oC_Resource.GetString("tChg") + " (" + oDTDis["FTXddDisChgTxt"].ToString() + ")", aoC_Font[2], Brushes.Black, 0, pnStartY);
                                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((decimal)(oDTDis["FCXddNet"]) + (decimal)(oDTDis["FCXddValue"])), nC_DecShow);
                                            poGraphic.DrawString(tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }// End 2 :ไม่อนุญาตให้รวมรายการสินค้าตอนพิมพ์


                // ส่วนลดโปรโมชั่น

                if (oDbTblPD != null && oDbTblPD.Rows.Count > 0)
                {
                    foreach (DataRow oPD in oDbTblPD.Rows)
                    {
                        pnStartY += 18;
                        poGraphic.DrawString("  " + oC_Resource.GetString("tDis") + " " + oPD["FTPmdGrpName"].ToString(), aoC_Font[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 100, 18));

                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oPD["FCXpdDis"]), nC_DecShow);
                        poGraphic.DrawString("-" + tAmt, aoC_Font[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar); //*Arm 63-05-12

                    }
                }
                //+++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cEJ", "C_PRNxPrintDT : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
        }
        public bool C_PRNxSlip(ref Graphics poGraphic, cmlTCNMPos poPos, cmlRcvSalePos poSalePos, out DateTime pdDocDate, bool pbCopy, int pnWidth, ref int pnStartY, ref string ptErrMsg)
        {
            DataSet oTblPrn;
            StringFormat oFormatFar, oFormatCenter;
            DataTable oDbTblHD, oDbTblHDDis, oDbTblHDCst;
            DataTable oDbTblRD;
            DataTable oDbTblQty;
            DataTable oDbTblRC;
            DataTable oDbTblMem, oDbTblMemTxnRed, oDbTblMemRD;
            DataTable oDbTblPD2, oDbTblPDCpn;
            cSlipMsg oMsg;
            Image oLogo;
            string[] atRmk;
            decimal cChange, cRefundRed, cCstPiontB4Used, cPntB4Bill;
            string tPrintText;
            string tCstTel;
            string tAmt;
            string tMsg;
            string tLine;
            string tDataA, tDataB;
            string tPicPath;

            pdDocDate = DateTime.Now;
            try
            {
                #region Initial
                ////////////////////////////////////////////

                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };
                oMsg = new cSlipMsg();

                cChange = 0m;
                cRefundRed = 0m;
                cCstPiontB4Used = 0m;
                cPntB4Bill = 0m;


                tLine = "------------------------------------------------------------------------";
                tCstTel = "";
                tPicPath = "";
                ///////////////////////////////////////////
                #endregion

                #region Config
                ///////////////////////////////////////////

                ///////////////////////////////////////////
                #endregion

                oTblPrn = C_GETxDataPrint(poSalePos.ptBchCode, poSalePos.ptXihDocNo, poPos.FTPosStaSumPrn, bC_AlwPrnVoid,
                                           poSalePos.ptConnStr, nC_CmdTimeout);

                if (oTblPrn == null || oTblPrn.Tables.Count != 14)
                    throw new Exception($"Cannot Get Data BchCode{poSalePos.ptBchCode} DocNo{poSalePos.ptXihDocNo}");
                oDbTblHD = oTblPrn.Tables[3];
                oDbTblHDDis = oTblPrn.Tables[4];
                oDbTblRD = oTblPrn.Tables[5];
                oDbTblQty = oTblPrn.Tables[6];
                oDbTblRC = oTblPrn.Tables[7];
                oDbTblHDCst = oTblPrn.Tables[8];
                oDbTblMem = oTblPrn.Tables[9];
                oDbTblMemTxnRed = oTblPrn.Tables[10];
                oDbTblMemRD = oTblPrn.Tables[11];
                oDbTblPD2 = oTblPrn.Tables[12];
                oDbTblPDCpn = oTblPrn.Tables[13];


                if (nC_PaperSize == 1)
                {
                    if (pnWidth > 215)
                        pnWidth = 215;
                }
                else
                {
                    if (pnWidth > 280)
                        pnWidth = 280;
                }


                if (!String.IsNullOrEmpty(tC_PathLogo))
                {
                    if (File.Exists(tC_PathLogo))
                    {
                        oLogo = Image.FromFile(tC_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            poGraphic.DrawImage(oLogo, new Rectangle((pnWidth - oLogo.Width) / 2, pnStartY, oLogo.Width, oLogo.Height));
                            pnStartY += oLogo.Height;
                        }
                        else
                        {
                            poGraphic.DrawImage(oLogo, new Rectangle((pnWidth - 200) / 2, pnStartY, 200, 200));
                            pnStartY += 200;
                        }

                    }
                }

                // Header Slip Msg
                pnStartY = oMsg.C_GETnSlipMsg(poSalePos.ptConnStr, poPos.FTSmgCode, "1", poGraphic, pnWidth, pnStartY, aoC_Font[0]);

                if (oDbTblHD != null && oDbTblHD.Rows.Count > 0)
                {
                    // พิมพ์ User ด้วย UsrCode ของบิล
                    poGraphic.DrawString("ID:" + poPos.FTPosRegNo.PadRight(30) +
                                         "USR: " + oDbTblHD.Rows[0].Field<string>("FTUsrCode") + " " +
                                         "T: " + poSalePos.ptPosCode,
                                        aoC_Font[1], Brushes.Black, 0, pnStartY);
                    pnStartY += 18;
                    // พิมพ์วันที่โดยใช้ Docdate
                    string tFullDate = oDbTblHD.Rows[0].Field<string>("FDXshDocDate");
                    pdDocDate = DateTime.ParseExact(tFullDate, "yyyy-MM-dd HH:mm:ss", null);
                    string tDate = DateTime.ParseExact(tFullDate, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm");
                    poGraphic.DrawString(tDate + " BNO:" + poSalePos.ptXihDocNo,
                                   aoC_Font[1], Brushes.Black, 0, pnStartY);
                }

                //Print DT
                C_PRNxPrintDT(ref poGraphic, poPos, poSalePos, ref pnStartY, pnWidth, oTblPrn);

                pnStartY += 30;
                if (oDbTblHD != null)
                {
                    if (Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshDis"]) != (decimal)0 || Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshChg"]) != (decimal)0 || Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshRnd"]) != (decimal)0)
                    {
                        poGraphic.DrawString("Subtotal", aoC_Font[4], Brushes.Black, 0, pnStartY);
                        tAmt = oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshTotal"]), nC_DecShow);
                        poGraphic.DrawString(tAmt, aoC_Font[5], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                        pnStartY += 18;

                        if (oDbTblHDDis.Rows.Count > 0)
                        {
                            foreach (DataRow oHDDis in oDbTblHDDis.Rows)
                            {
                                if (string.IsNullOrEmpty(oHDDis.Field<string>("FTXhdRefCode")))
                                {
                                    //ส่วนลด 
                                    if (oHDDis.Field<string>("FTXhdDisChgType") == "1" || oHDDis.Field<string>("FTXhdDisChgType") == "2")
                                    {
                                        poGraphic.DrawString(oC_Resource.GetString("tDis") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", aoC_Font[4], Brushes.Black, 0, pnStartY);
                                        tAmt = oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oHDDis.Field<Decimal>("FCXhdAmt")), nC_DecShow); //*Arm 63-04-16
                                        poGraphic.DrawString(tAmt, aoC_Font[5], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                        pnStartY += 18;
                                    }
                                    //ชาจน์
                                    if (oHDDis.Field<string>("FTXhdDisChgType") == "3" || oHDDis.Field<string>("FTXhdDisChgType") == "4")
                                    {
                                        poGraphic.DrawString(oC_Resource.GetString("tChg") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", aoC_Font[4], Brushes.Black, 0, pnStartY);
                                        tAmt = oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oHDDis.Field<Decimal>("FCXhdAmt")), nC_DecShow);
                                        poGraphic.DrawString(tAmt, aoC_Font[5], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                        pnStartY += 18;
                                    }
                                }
                                else
                                {
                                    //Redeem
                                    if (oHDDis.Field<string>("FTXhdDisChgType") == "1")
                                    {

                                        if (oDbTblRD.Rows[0]["FTRdhDocType"].ToString() == "1")
                                        {
                                            poGraphic.DrawString(oC_Resource.GetString("tRdPdt") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", aoC_Font[4], Brushes.Black, 0, pnStartY);
                                        }
                                        else
                                        {
                                            poGraphic.DrawString(oC_Resource.GetString("tRdDis") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", aoC_Font[4], Brushes.Black, 0, pnStartY);
                                        }
                                        tAmt = oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oHDDis.Field<Decimal>("FCXhdAmt")), nC_DecShow); //*Arm 63-04-16
                                        poGraphic.DrawString(tAmt, aoC_Font[5], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                        pnStartY += 18;
                                    }
                                }

                                //Coupon
                                if (oHDDis.Field<string>("FTXhdDisChgType") == "5" || oHDDis.Field<string>("FTXhdDisChgType") == "6")
                                {
                                    poGraphic.DrawString(oC_Resource.GetString("tCpnRd") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", aoC_Font[4], Brushes.Black, 0, pnStartY);
                                    tAmt = oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oHDDis.Field<Decimal>("FCXhdAmt")), nC_DecShow);
                                    poGraphic.DrawString(tAmt, aoC_Font[5], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                    pnStartY += 18;
                                }

                            }
                        }

                        if (Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshRnd"]) != 0m)
                        {
                            tAmt = oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshRnd"]), nC_DecShow);
                            poGraphic.DrawString("Round Rcv: " + tAmt, aoC_Font[4], Brushes.Black, 0, pnStartY);
                            tAmt = oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshTotal"]) - Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshDis"]) + Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshChg"])), nC_DecShow);
                            poGraphic.DrawString(tAmt, aoC_Font[5], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            pnStartY += 18;
                        }

                    }


                    poGraphic.DrawString("TOTAL " + oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblQty.Rows[0]["FCXsdQty"]), nC_DecShow) + " Items", aoC_Font[4], Brushes.Black, 0, pnStartY);
                    tAmt = oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshGrand"]), nC_DecShow);
                    poGraphic.DrawString(tAmt, aoC_Font[5], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);

                    pnStartY += 18;
                    tAmt = "Vatable : " + oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshVatable"]), nC_DecShow);
                    tAmt += " " + "VAT : " + oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshVat"]), nC_DecShow);
                    poGraphic.DrawString(tAmt, aoC_Font[4], Brushes.Black, 0, pnStartY);
                }

                if (oDbTblRC != null)
                {

                    foreach (DataRow oRC in oDbTblRC.Rows)
                    {
                        pnStartY += 18;
                        if (string.IsNullOrEmpty(oRC.Field<string>("FTXrcRefNo1")))
                        {
                            poGraphic.DrawString(oRC.Field<string>("FTRcvName"), aoC_Font[6], Brushes.Black, 0, pnStartY);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(oRC.Field<string>("FTXrcRefNo2").Split(';')[oRC.Field<string>("FTXrcRefNo2").Split(';').Length - 1]))
                            {
                                poGraphic.DrawString(oRC.Field<string>("FTRcvName") + "(" + oRC.Field<string>("FTXrcRefNo1") + ")", aoC_Font[6], Brushes.Black, 0, pnStartY);
                            }
                            else
                            {
                                //Net 63-09-02 ปรับ EJ แสดงประเภทการชำระแทน กรณีที่มี Ref2
                                //poGraphic.DrawString(oRC.Field<string>("FTXrcRefNo2").Split(';')[oRC.Field<string>("FTXrcRefNo2").Split(';').Length - 1] + " (" + oRC.Field<string>("FTXrcRefNo1") + ")", aoC_Font[6], Brushes.Black, 0, pnStartY);
                                poGraphic.DrawString(oRC.Field<string>("FTRcvName") + " (" + oRC.Field<string>("FTXrcRefNo1") + ")", aoC_Font[6], Brushes.Black, 0, pnStartY);

                            }

                        }
                        tAmt = oC_SP.SP_SETtDecShwSve(1, oRC.Field<Decimal>("FCXrcUsrPayAmt"), nC_DecShow);
                        poGraphic.DrawString(tAmt, aoC_Font[7], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                        cChange = oRC.Field<Decimal>("FCXrcChg");
                    }


                    pnStartY += 18;
                    poGraphic.DrawString("Change", aoC_Font[4], Brushes.Black, 0, pnStartY);
                    tAmt = oC_SP.SP_SETtDecShwSve(1, cChange, nC_DecShow);
                    poGraphic.DrawString(tAmt, aoC_Font[5], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);

                }

                // อ้างอิง SO
                if (!string.IsNullOrEmpty(oDbTblHD.Rows[0]["FTXshRefExt"].ToString()))
                {
                    pnStartY += 18;
                    poGraphic.DrawString(oC_Resource.GetString("tRef") + " : " + oDbTblHD.Rows[0]["FTXshRefExt"].ToString(), aoC_Font[1], Brushes.Black, 0, pnStartY);

                }
                //++++++++++++++++++++++

                // Print Point
                if (!string.IsNullOrEmpty(oDbTblHD.Rows[0]["FTCstCode"].ToString()))
                {

                    if (oDbTblHDCst != null)
                    {
                        tCstTel = string.IsNullOrEmpty(oDbTblHDCst.Rows[0]["FTXshCstTel"].ToString()) ? "" : oDbTblHDCst.Rows[0]["FTXshCstTel"].ToString(); // ใช้ใน QR Code

                        cPntB4Bill = oDbTblHD.Rows[0].Field<decimal>("FCPntB4Bill");

                        decimal cSumPnt = 0;

                        if (!string.IsNullOrEmpty(oDbTblHDCst.Rows[0]["FTXshDocNo"].ToString()))
                        {

                            pnStartY += 30;
                            poGraphic.DrawString("Mem ID : " + oDbTblHD.Rows[0]["FTCstCode"].ToString(), aoC_Font[1], Brushes.Black, 0, pnStartY);

                            pnStartY += 18;
                            poGraphic.DrawString(oDbTblHDCst.Rows[0]["FTXshCstName"].ToString(), aoC_Font[1], Brushes.Black, 10, pnStartY);

                            string tCardNo = string.IsNullOrEmpty(oDbTblHDCst.Rows[0]["FTXshCardNo"].ToString()) ? " - " : oDbTblHDCst.Rows[0]["FTXshCardNo"].ToString();
                            pnStartY += 18;
                            poGraphic.DrawString("Card No. : " + tCardNo, aoC_Font[1], Brushes.Black, 0, pnStartY);

                            pnStartY += 18;
                            if (!string.IsNullOrEmpty(oDbTblHD.Rows[0].Field<string>("FDCstCrdExpire")))
                            {
                                poGraphic.DrawString("Expired : " + DateTime.ParseExact(oDbTblHD.Rows[0].Field<string>("FDCstCrdExpire"), "yyyy-MM-dd", null).ToString("dd-MM-yyyy"), aoC_Font[1], Brushes.Black, 0, pnStartY);
                            }
                            else
                            {
                                poGraphic.DrawString("Expired : -", aoC_Font[1], Brushes.Black, 0, pnStartY);
                            }

                            pnStartY += 18;
                            poGraphic.DrawString("Last Pt.", aoC_Font[1], Brushes.Black, 0, pnStartY);
                            poGraphic.DrawString("Reg. Pt.", aoC_Font[1], Brushes.Black, 70, pnStartY);
                            poGraphic.DrawString("Promo Pt.", aoC_Font[1], Brushes.Black, 140, pnStartY);
                            poGraphic.DrawString("Total Point", aoC_Font[1], Brushes.Black, 210, pnStartY);
                            //Print ข้อมมูล Member
                            if (oDbTblHD.Rows[0].Field<int>("FNXshDocType") != 9)
                            {
                                //แต้มที่ใช้

                                if (oDbTblMemTxnRed != null && oDbTblMemTxnRed.Rows.Count > 0) // เช็คว่ามีข้อมูล TxnRedeem หรือไม่
                                {
                                    pnStartY += 18;
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cPntB4Bill, 0), aoC_Font[1], Brushes.Black, 0, pnStartY);
                                    decimal cRedPnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMemTxnRed.Rows[0]["FCRedPntBillQty"]) * (-1));
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cRedPnt, 0), aoC_Font[1], Brushes.Black, 70, pnStartY);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, 0, 0), aoC_Font[1], Brushes.Black, 140, pnStartY);
                                    cSumPnt = Convert.ToDecimal(cPntB4Bill + cRedPnt);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cSumPnt, 0), aoC_Font[1], Brushes.Black, 210, pnStartY);
                                    cPntB4Bill = cSumPnt;
                                }
                                if (oDbTblMem != null && oDbTblMem.Rows.Count > 0) // เช็คว่ามีข้อมูล TxnSal หรือไม่
                                {
                                    pnStartY += 18;
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cPntB4Bill, 0), aoC_Font[1], Brushes.Black, 0, pnStartY);
                                    decimal cSalePnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntBillQty"]) - Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]));
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cSalePnt, 0), aoC_Font[1], Brushes.Black, 70, pnStartY);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]), 0), aoC_Font[1], Brushes.Black, 140, pnStartY);
                                    cSumPnt = Convert.ToDecimal(cPntB4Bill + (cSalePnt + Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"])));
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cSumPnt, 0), aoC_Font[1], Brushes.Black, 210, pnStartY);
                                }

                                // ถ้าไม่มีข้อมูลเลย
                                if ((oDbTblMemTxnRed == null || oDbTblMemTxnRed.Rows.Count == 0) && (oDbTblMem == null || oDbTblMem.Rows.Count == 0))
                                {
                                    pnStartY += 18;
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cPntB4Bill, 0), aoC_Font[1], Brushes.Black, 0, pnStartY);

                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, 0, 0), aoC_Font[1], Brushes.Black, 70, pnStartY);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, 0, 0), aoC_Font[1], Brushes.Black, 140, pnStartY);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cPntB4Bill, 0), aoC_Font[1], Brushes.Black, 210, pnStartY);
                                }
                            }
                            else
                            {
                                //หา Point ก่อนใช้
                                cCstPiontB4Used = cPntB4Bill;
                                cRefundRed = Convert.ToDecimal(oDbTblMemRD.Rows[0]["FNXrdPntUse"]);
                                decimal cRefundPnt = ((cRefundRed - Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPnt"])) * (-1));

                                if (Math.Abs(cRefundPnt) > 0)
                                {
                                    //# แต้มที่ต้องคืนร้าน
                                    pnStartY += 18;
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(cCstPiontB4Used), 0), aoC_Font[1], Brushes.Black, 0, pnStartY);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cRefundPnt, 0), aoC_Font[1], Brushes.Black, 70, pnStartY);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]), 0), aoC_Font[1], Brushes.Black, 140, pnStartY);
                                    cSumPnt = (cCstPiontB4Used + (cRefundPnt + Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"])));
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cSumPnt, 0), aoC_Font[1], Brushes.Black, 210, pnStartY);

                                    cCstPiontB4Used = cSumPnt;
                                }

                                //# แต้มที่ได้รับคืน
                                if (cRefundRed > 0)
                                {
                                    pnStartY += 18;
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, Convert.ToDecimal(cCstPiontB4Used), 0), aoC_Font[1], Brushes.Black, 0, pnStartY);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cRefundRed, 0), aoC_Font[1], Brushes.Black, 70, pnStartY);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, 0, 0), aoC_Font[1], Brushes.Black, 140, pnStartY);
                                    cSumPnt = Convert.ToDecimal(cCstPiontB4Used + cRefundRed);
                                    poGraphic.DrawString(oC_SP.SP_SETtDecShwSve(1, cSumPnt, 0), aoC_Font[1], Brushes.Black, 210, pnStartY);
                                }
                            }
                        }


                    }
                }

                // Print ชื่อผู้ขาย
                string tUserName = oDbTblHD.Rows[0]["FTUsrName"].ToString();
                pnStartY += 30;
                poGraphic.DrawString("User : " + tUserName, aoC_Font[1], Brushes.Black, 0, pnStartY);


                if (oDbTblHD.Rows[0].Field<int>("FNXshDocType") == 9)
                {
                    pnStartY += 30;
                    tMsg = oC_Resource.GetString("tRefundPdt");
                    if (!string.IsNullOrEmpty(oDbTblHD.Rows[0].Field<string>("FTXshRefInt")))
                    {
                        tMsg += oC_Resource.GetString("tRefer") + ":" + oDbTblHD.Rows[0].Field<string>("FTXshRefInt");
                    }
                    poGraphic.DrawString(tMsg, aoC_Font[8], Brushes.Black, new RectangleF(0, pnStartY, pnWidth, 18), oFormatCenter);

                }

                if (pbCopy) // เมื่อพิมพ์สำเนา
                {
                    //สำเนา
                    pnStartY += 18;
                    tPrintText = "!!! " + oC_Resource.GetString("tCopy") + " " + Convert.ToInt32(oDbTblHD.Rows[0]["FNXshDocPrint"]) + " !!!";
                    poGraphic.DrawString(tPrintText, aoC_Font[5], Brushes.Black, new RectangleF(0, pnStartY, pnWidth, 18), oFormatCenter);
                    //++++++++++++++
                }

                //Remark
                if (!string.IsNullOrEmpty(oDbTblHD.Rows[0]["FTXshRmk"].ToString()))
                {
                    pnStartY += 18;
                    tPrintText = oDbTblHD.Rows[0]["FTXshRmk"].ToString();
                    atRmk = tPrintText.Split((char)10);
                    foreach (string tStr in atRmk)
                    {
                        pnStartY += 18;
                        poGraphic.DrawString(tStr, aoC_Font[4], Brushes.Black, 0, pnStartY);
                    }
                }

                string tCstCode = oDbTblHD.Rows[0]["FTCstCode"].ToString();
                switch (nC_ChkShowBarQR)
                {
                    case 1:    //1 : แสดง Barcode
                        pnStartY += 30;
                        pnStartY = oMsg.C_GETnSlipMsg(poSalePos.ptConnStr, poPos.FTSmgCode, "2", poGraphic, pnWidth, pnStartY, aoC_Font[0]); // Footer Slip Msg
                        pnStartY += 18;
                        C_PRNxBarcode(ref poGraphic, poSalePos.ptXihDocNo, pnWidth, pnStartY);
                        break;

                    case 2:     //2 : แสดง QRCode,
                        pnStartY += 30;
                        pnStartY = oMsg.C_GETnSlipMsg(poSalePos.ptConnStr, poPos.FTSmgCode, "2", poGraphic, pnWidth, pnStartY, aoC_Font[0]); // Footer Slip Msg
                        pnStartY += 18;
                        tDataA = poSalePos.ptXihDocNo;
                        tDataB = poSalePos.ptXihDocNo + "|" + tCstCode == null ? "" : tCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", (decimal)(oDbTblHD.Rows[0]["FCXshGrand"])) + " | " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
                        C_PRNxQRCode(ref poGraphic, tDataA + "|" + tDataB, pnWidth, pnStartY);
                        pnStartY += 138;
                        break;

                    case 3:     //3 : แสดงทั้ง Barcode และ QRcode
                        pnStartY += 30;
                        pnStartY = oMsg.C_GETnSlipMsg(poSalePos.ptConnStr, poPos.FTSmgCode, "2", poGraphic, pnWidth, pnStartY, aoC_Font[0]); // Footer Slip Msg
                        pnStartY += 18;
                        C_PRNxBarcode(ref poGraphic, poSalePos.ptXihDocNo, pnWidth, pnStartY);
                        pnStartY += 30;
                        tDataA = poSalePos.ptXihDocNo;
                        tDataB = poSalePos.ptXihDocNo + "|" + tCstCode == null ? "" : tCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", (decimal)(oDbTblHD.Rows[0]["FCXshGrand"])) + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
                        C_PRNxQRCode(ref poGraphic, tDataA + "|" + tDataB, pnWidth, pnStartY);
                        pnStartY += 138;
                        break;

                    default:    // ไม่แสดง
                        pnStartY += 30;
                        pnStartY = oMsg.C_GETnSlipMsg(poSalePos.ptConnStr, poPos.FTSmgCode, "2", poGraphic, pnWidth, pnStartY, aoC_Font[0]); // Footer Slip Msg
                        pnStartY += 18;
                        break;
                }

                // โปรโมชั่น
                if (oDbTblPD2.Rows.Count > 0)
                {

                    foreach (DataRow oRow in oDbTblPD2.Rows)
                    {
                        pnStartY += 18;
                        poGraphic.DrawString(tLine, aoC_Font[5], Brushes.Black, new RectangleF(0, pnStartY, pnWidth, 18), oFormatCenter);

                        pnStartY += 18;
                        tPrintText = oRow.Field<string>("FTXpdCpnText") + " " + Convert.ToInt16(oRow.Field<Decimal>("FCXpdGetQtyDiv")).ToString() + " " + oC_Resource.GetString("tPrivilege");
                        poGraphic.DrawString(tPrintText, aoC_Font[5], Brushes.Black, new RectangleF(0, pnStartY, pnWidth, 18), oFormatCenter);
                    }

                }

                // โปรโมชั่นคูปอง
                if (oDbTblPDCpn != null && oDbTblPDCpn.Rows.Count > 0)
                {
                    foreach (DataRow oRow in oDbTblPDCpn.Rows)
                    {
                        pnStartY += 18;
                        poGraphic.DrawString(tLine, aoC_Font[5], Brushes.Black, new RectangleF(0, pnStartY, pnWidth, 18), oFormatCenter);

                        pnStartY += 18;
                        tPrintText = oC_Resource.GetString("tCoupon");
                        poGraphic.DrawString(tPrintText, aoC_Font[5], Brushes.Black, new RectangleF(0, pnStartY, pnWidth, 18), oFormatCenter);

                        tPicPath = oRow.Field<string>("FTImgPath");
                        if (!string.IsNullOrEmpty(tPicPath))
                        {
                            if (File.Exists(tPicPath))
                            {
                                using (Image oCpnPic = Image.FromFile(tPicPath))
                                {
                                    oLogo = new Bitmap(oCpnPic);
                                    if (oLogo.Width < 200)
                                    {
                                        poGraphic.DrawImage(oLogo, new Rectangle((pnWidth - oLogo.Width) / 2, pnStartY, oLogo.Width, oLogo.Height));
                                        pnStartY += oLogo.Height;
                                    }
                                    else
                                    {
                                        poGraphic.DrawImage(oLogo, new Rectangle((pnWidth - 200) / 2, pnStartY, 200, 200));
                                        pnStartY += 200;
                                    }
                                }

                            }
                        }

                        pnStartY += 20;
                        tPrintText = oRow.Field<string>("FTCpdBarCpn");
                        C_PRNxBarcode(ref poGraphic, tPrintText, pnWidth, pnStartY);

                        pnStartY += 20;
                        tPrintText = Convert.ToInt16(oRow.Field<Decimal>("FCXpdGetQtyDiv")).ToString() + " " + oC_Resource.GetString("tPrivilege");
                        poGraphic.DrawString(tPrintText, aoC_Font[5], Brushes.Black, new RectangleF(0, pnStartY, pnWidth, 18), oFormatCenter);
                    }

                }
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message;
                new cLog().C_WRTxLog("cEJ", "C_GETxDataPrint : " + oEx.Message);
            }
            finally
            {
                //oC_SP.SP_CLExMemory();
            }
            return false;
        }
        public bool C_GENbSlip(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, cmlTCNMPos poPos, ref string ptErrMsg)
        {
            Graphics oGraphic = null;
            DateTime dDocDate;
            int nStartY, nWidth;
            string tPathFile;

            try
            {
                nStartY = 0;
                nWidth = 280;

                Bitmap oNewBitmap = new Bitmap(nWidth, 55000, PixelFormat.Format64bppArgb);
                oGraphic = Graphics.FromImage(oNewBitmap);
                oGraphic.FillRectangle(Brushes.White, new RectangleF(0, 0, nWidth, 55000));

                if (C_PRNxSlip(ref oGraphic, poPos, poSalePos, out dDocDate, false, nWidth, ref nStartY, ref ptErrMsg) == false)
                {
                    throw new Exception(ptErrMsg);
                }
                nStartY += 18;
                Rectangle oCropRect = new Rectangle(0, 0, 280, nStartY);
                Bitmap oBitmapEJ = new Bitmap(280, nStartY, PixelFormat.Format64bppArgb);
                using (Graphics oGrp = Graphics.FromImage(oBitmapEJ))
                {
                    oGrp.DrawImage(oNewBitmap, oCropRect, oCropRect, GraphicsUnit.Pixel);
                }
                tPathFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
                tPathFile = Directory.GetParent(Directory.GetParent(tPathFile).FullName).FullName;
                //tPathFile += @"\AdaEJ\Image\" + dDocDate.ToString("yyMMdd"); //*Net 63-07-23 ปรับ Folder ที่เก็บ
                tPathFile += $@"\AdaEJ\{poSalePos.ptBchCode}\" + dDocDate.ToString("yyMMdd");


                if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "DOC", tPathFile, ref ptErrMsg))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message;
                new cLog().C_WRTxLog("cEJ", "C_GENbSlip : " + oEx.Message);
                return false;
            }
            finally
            {
                if (oGraphic != null)
                    oGraphic.Dispose();

                oGraphic = null;
                //oC_SP.SP_CLExMemory();
            }
        }




        //*Net 63-07-17 C_GENbSlipSUM เก่า
        //public bool C_GENbSlipSUM(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, cmlTCNMPos poPos)
        //{
        //    Graphics oGraphic = null;
        //    StringFormat oFormatFar = null, oFormatCenter = null;
        //    cSlipMsg oMsg;
        //    string tLine, tDeposit;
        //    int nStartY = 0, nWidth;
        //    cDatabase oDB = new cDatabase();
        //    StringBuilder oSql = new StringBuilder();
        //    string tAmt, tVat;
        //    decimal cChange = 0;
        //    string tCstTel = "";
        //    List<cmlTPSTSalDT> aoDT;
        //    List<cmlTPSTSalDTDis> aoDTDis;
        //    string tPrint = ""; //*Em 62-09-06
        //    string[] atRmk;
        //    List<Font> aoFont = new List<Font>();
        //    Font oFont = new Font("CordiaUPC", 11.5f, FontStyle.Regular);
        //    string tConnStr = poSalePos.ptConnStr;
        //    int nCmdTime = Convert.ToInt32(poShopDB.nCommandTimeOut);
        //    cSP oSP = new cSP();
        //    int nDecShw = 2;
        //    string tPathFile;
        //    decimal cSumShift = 0;
        //    cmlShiftSum oSumShift;
        //    List<cmlShiftSumRcv> aoShiftSSumRcv;
        //    List<cmlTPSTShiftSLastDoc> aoShiftSLastDoc;
        //    List<cmlTPSTShiftSRatePdt> aoShiftSRatePdt;
        //    cmlTPSTShiftHD oShiftHD = new cmlTPSTShiftHD();
        //    string tLastRcv = "";
        //    decimal cLastAmt = 0;
        //    bool bPrnShiftSumRedeem = false; //*Arm 63-05-28
        //    List<cmlShiftSumRedeem> aoShiftSumRedeem; //*Arm 63-05-28
        //    try
        //    {

        //        tLine = "--------------------------------------------------------------------------------------------";
        //        nWidth = 280;

        //        oSql.Clear();
        //        oSql.AppendLine("SELECT FTSysSeq,FTSysStaDefValue,FTSysStaUsrValue");
        //        oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
        //        oSql.AppendLine("WHERE FTSysCode = 'PInvLayout'");
        //        oSql.AppendLine("AND (CONVERT(INT,FTSysSeq) BETWEEN 1 AND 9)");
        //        oSql.AppendLine("ORDER BY CONVERT(INT,FTSysSeq)");
        //        DataTable odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), nCmdTime);
        //        if (odtTmp != null)
        //        {
        //            aoFont = new List<Font>();
        //            foreach (DataRow oRow in odtTmp.Rows)
        //            {
        //                string[] atFont;
        //                if (string.IsNullOrEmpty(oRow.Field<string>("FTSysStaUsrValue")))
        //                {
        //                    atFont = oRow.Field<string>("FTSysStaDefValue").Split(',');

        //                }
        //                else
        //                {
        //                    atFont = oRow.Field<string>("FTSysStaDefValue").Split(',');
        //                }
        //                FontStyle oFontStyle = new FontStyle();
        //                oFontStyle = FontStyle.Regular;
        //                if (!string.IsNullOrEmpty(atFont[2]))
        //                    oFontStyle |= FontStyle.Bold;

        //                if (!string.IsNullOrEmpty(atFont[3]))
        //                    oFontStyle |= FontStyle.Italic;

        //                if (!string.IsNullOrEmpty(atFont[4]))
        //                    oFontStyle |= FontStyle.Underline;

        //                oFont = new Font(atFont[0], Convert.ToSingle(atFont[1]), oFontStyle);
        //                aoFont.Add(oFont);
        //            }
        //        }

        //        oSql.Clear();
        //        oSql.AppendLine("SELECT HD.FDShdSaleDate,ISNULL(USR.FTUsrName,HD.FTShdUsrClosed) AS FTUsrCode  ");
        //        oSql.AppendLine("FROM TPSTShiftHD HD WITH(NOLOCK)");
        //        oSql.AppendLine("LEFT JOIN  TCNMUser_L USR WITH(NOLOCK) ON HD.FTShdUsrClosed = USR.FTUsrCode AND USR.FNLngID = " + nC_Language);
        //        oSql.AppendLine("WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTPosCode = '" + poSalePos.ptPosCode + "' AND HD.FTShfCode = '" + poSalePos.ptXihDocNo + "'");
        //        oShiftHD = oDB.C_DAToExecuteQuery<cmlTPSTShiftHD>(tConnStr, oSql.ToString(), nCmdTime);
        //        if (oShiftHD == null) return false;

        //        //Bitmap oNewBitmap = new Bitmap(nWidth, 2000, PixelFormat.Format32bppArgb);
        //        //Bitmap oNewBitmap = new Bitmap(nWidth, 5000, PixelFormat.Format64bppArgb);  //*Em 62-10-16
        //        Bitmap oNewBitmap = new Bitmap(nWidth, 20000, PixelFormat.Format64bppArgb); //*Arm 63-05-28
        //        oGraphic = Graphics.FromImage(oNewBitmap);
        //        //oGraphic.FillRectangle(Brushes.White, new RectangleF(0, 0, nWidth, 2000));
        //        oGraphic.FillRectangle(Brushes.White, new RectangleF(0, 0, nWidth, 20000)); //*Arm 63-05-28

        //        oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
        //        oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };
        //        oMsg = new cSlipMsg();

        //        nStartY = oMsg.C_GETnSlipMsg(poSalePos.ptConnStr, poPos.FTSmgCode, "1", oGraphic, nWidth, nStartY, aoFont[0]);    // Header Slip Msg

        //        tPrint = oC_Resource.GetString("tShiftSUM").ToString(); ;
        //        oGraphic.DrawString(tPrint, aoFont[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

        //        //----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //!!!ปิดการขาย!!!
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tSaleDate").ToString() + ":" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("dd/MM/yyyy");
        //        tPrint += " " + oC_Resource.GetString("tPos").ToString() + ":" + poSalePos.ptPosCode;
        //        tPrint += " " + oC_Resource.GetString("tUsr").ToString() + ":" + oShiftHD.FTUsrCode;
        //        oGraphic.DrawString(tPrint, aoFont[2], Brushes.Black, 0, nStartY);

        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //รายการ ความถี่ มูลค่า
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tItem");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oC_Resource.GetString("tFreq");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatCenter);
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oC_Resource.GetString("tAmount");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

        //        //----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //รายการต่างๆ
        //        cSumShift = 0;
        //        oSumShift = new cmlShiftSum();
        //        oSumShift = C_GEToShiftSum(poSalePos, tConnStr, nCmdTime);

        //        //เปิดลิ้นชักไม่ขาย
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tOpDrwNoS");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.nCntOpDrw, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

        //        //ยกเลิกบิล
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tCancelBill");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.nCntCancelBill, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

        //        //พักบิล
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tHoldBill");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.nCntHoldBill, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

        //        //นำเงินเข้า
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tMnyIn");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.nMnyInCnt, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.cMnyInAmt, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

        //        //ยอดขาย
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tSaleAmt");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.nSaleCnt, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.cSaleAmt, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

        //        //นำเงินออก
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tMnyOut");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.nMnyOutCnt, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.cMnyOutAmt, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

        //        //ยอดคืนสินค้า
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tRefundAmt");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.nRefundCnt, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
        //        oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
        //        tPrint = oSP.SP_SETtDecShwSve(1, oSumShift.cRefundAmt, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //รวมเงิน
        //        nStartY += 18;
        //        tPrint = "    " + oC_Resource.GetString("tTotal") + ":";
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
        //        cSumShift = (oSumShift.cMnyInAmt + oSumShift.cSaleAmt) - (oSumShift.cMnyOutAmt + oSumShift.cRefundAmt);
        //        tPrint = oSP.SP_SETtDecShwSve(1, cSumShift, nC_DecShow);
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(151, nStartY, 120, 18), oFormatFar);

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //สรุปยอดรับชำระตามประเภทการชำระเงิน
        //        aoShiftSSumRcv = new List<cmlShiftSumRcv>();
        //        aoShiftSSumRcv = C_GETaShiftSumRcv(poSalePos, tConnStr, nCmdTime);
        //        tLastRcv = "";
        //        cLastAmt = 0;
        //        if (aoShiftSSumRcv.Count > 0)
        //        {
        //            foreach (cmlShiftSumRcv oShift in aoShiftSSumRcv)
        //            {
        //                if (string.Equals(oShift.FTRcvCode, tLastRcv))
        //                {
        //                    if (string.Equals(oShift.FTRcvDocType, "9"))
        //                    {
        //                        tPrint = oC_Resource.GetString("tRefund") + " " + oShift.FTRcvName;
        //                    }
        //                    else
        //                    {
        //                        tPrint = oC_Resource.GetString("tSale") + " " + oShift.FTRcvName;
        //                    }
        //                    nStartY += 18;
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
        //                    tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), nC_DecShow);
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

        //                    //-----------------------
        //                    nStartY += 10;
        //                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //                    //รวมเงิน
        //                    nStartY += 18;
        //                    tPrint = oC_Resource.GetString("tTotal") + " " + oShift.FTRcvName; ;
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
        //                    tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(cLastAmt - oShift.FCRcvPayAmt), nC_DecShow);
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

        //                    //-----------------------
        //                    nStartY += 10;
        //                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
        //                }
        //                else
        //                {
        //                    if (string.Equals(oShift.FTRcvDocType, "9"))
        //                    {
        //                        tPrint = oC_Resource.GetString("tRefund") + " " + oShift.FTRcvName;
        //                    }
        //                    else
        //                    {
        //                        tPrint = oC_Resource.GetString("tSale") + " " + oShift.FTRcvName;
        //                    }
        //                    nStartY += 18;
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
        //                    tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), nC_DecShow);
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
        //                }
        //                tLastRcv = oShift.FTRcvCode;
        //                cLastAmt = Convert.ToDecimal(oShift.FCRcvPayAmt);
        //            }
        //        }

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //สรุปการรับชำระสกุลเงิน
        //        nStartY += 18;
        //        tPrint = "    " + oC_Resource.GetString("tSumCurrency");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //ส่วนหัวสกุลเงิน
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tCurrency");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
        //        tPrint = oC_Resource.GetString("tAmount");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //รายละเอียดสกุลเงิน
        //        aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
        //        aoShiftSRatePdt = C_GETaShiftSRate(poSalePos, tConnStr, nCmdTime);
        //        if (aoShiftSRatePdt.Count > 0)
        //        {
        //            foreach (cmlTPSTShiftSRatePdt oData in aoShiftSRatePdt)
        //            {
        //                nStartY += 18;
        //                tPrint = oData.FTSrpNameRef;
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
        //                tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpAmt), nC_DecShow);
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
        //            }
        //        }

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);


        //        //(*Arm 63-05-28)
        //        //สรุปการแลกแต้ม รับส่วนลด
        //        oSql.Clear();
        //        oSql.AppendLine("SELECT FTSysStaUsrValue");
        //        oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
        //        oSql.AppendLine("WHERE FTSysCode = 'bPS_AlwPrintShift' ");
        //        oSql.AppendLine("AND FTSysSeq = '4' ");
        //        string tSysStaUsrValue = oDB.C_DAToExecuteQuery<string>(tConnStr, oSql.ToString(), nCmdTime);
        //        bPrnShiftSumRedeem = (string.Equals(tSysStaUsrValue, "1")) ? true : false;

        //        if (bPrnShiftSumRedeem == true)
        //        {
        //            nStartY += 18;
        //            tPrint = "    " + oC_Resource.GetString("tSumRedeem");
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

        //            //-----------------
        //            nStartY += 10;
        //            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //            //ส่วนหัวการแลกแต้ม รับส่วนลด

        //            nStartY += 18;
        //            tPrint = oC_Resource.GetString("tCardNo");
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
        //            tPrint = oC_Resource.GetString("tUsePnt");
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatCenter);
        //            tPrint = oC_Resource.GetString("tAmtDis");
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);

        //            //-----------------
        //            nStartY += 10;
        //            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //            //รายละเอียดการแลกแต้ม รับส่วนลด
        //            aoShiftSumRedeem = new List<cmlShiftSumRedeem>();
        //            aoShiftSumRedeem = C_GETaShiftSumRedeem(poSalePos, 2, tConnStr, nCmdTime);

        //            if (aoShiftSumRedeem.Count > 0)
        //            {
        //                foreach (cmlShiftSumRedeem oData in aoShiftSumRedeem)
        //                {
        //                    nStartY += 18;
        //                    tPrint = oData.FTXshCardNo;
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
        //                    tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FNXrdPntUse), nC_DecShow);
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatFar);
        //                    tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCXhdAmt), nC_DecShow);
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
        //                }
        //            }
        //            //-----------------
        //            nStartY += 10;
        //            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);


        //            //รายละเอียดการแลกแต้ม รับสินค้า
        //            aoShiftSumRedeem = new List<cmlShiftSumRedeem>();
        //            aoShiftSumRedeem = C_GETaShiftSumRedeem(poSalePos, 1, tConnStr, nCmdTime);

        //            if (aoShiftSumRedeem.Count > 0)
        //            {
        //                foreach (cmlShiftSumRedeem oData in aoShiftSumRedeem)
        //                {
        //                    nStartY += 18;
        //                    tPrint = oData.FTXshCardNo;
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
        //                    tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FNXrdPntUse), nC_DecShow);
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatFar);
        //                    tPrint = oData.FTXsdBarCode;
        //                    oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
        //                }
        //            }
        //            //-----------------
        //            nStartY += 10;
        //            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
        //        }
        //        //End(*Arm 63-05-28)



        //        //สรุปยอดขายสินค้าควบคุมพิเศษ
        //        nStartY += 18;
        //        tPrint = "    " + oC_Resource.GetString("tPdtSpcSum");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //ส่วนหัวสินค้าควบคุม
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tItem");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //        tPrint = oC_Resource.GetString("tQty");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, 50, 18), oFormatCenter);
        //        tPrint = oC_Resource.GetString("tAmount");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //รายการสินค้าควบคุม
        //        aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
        //        aoShiftSRatePdt = C_GETaShiftSPdt(poSalePos, tConnStr, nCmdTime);
        //        if (aoShiftSRatePdt.Count > 0)
        //        {
        //            foreach (cmlTPSTShiftSRatePdt oData in aoShiftSRatePdt)
        //            {
        //                nStartY += 18;
        //                tPrint = oData.FTSrpNameRef;
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpQty), nC_DecShow);
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, 50, 18), oFormatFar);
        //                tPrint = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpAmt), nC_DecShow);
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
        //            }
        //        }

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        ////รอบ/ลำดับ
        //        //nStartY += 18;
        //        //tMsg = cVB.oVB_GBResource.GetString("tItem");
        //        //oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));

        //        ////-----------------
        //        //nStartY += 10;
        //        //oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //ช่วงบิล
        //        aoShiftSLastDoc = new List<cmlTPSTShiftSLastDoc>();
        //        aoShiftSLastDoc = C_GETaShiftSLastDoc(poSalePos, tConnStr, nCmdTime);

        //        if (aoShiftSLastDoc.Count == 0)
        //        {
        //            //บิลขาย จาก
        //            nStartY += 18;
        //            tPrint = oC_Resource.GetString("tSaleFrm");
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //            tPrint = "--------N/A--------";
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //            //บิลขาย ถึง
        //            nStartY += 18;
        //            tPrint = oC_Resource.GetString("tSaleTo");
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //            tPrint = "--------N/A--------";
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //            //บิลคืน จาก
        //            nStartY += 18;
        //            tPrint = oC_Resource.GetString("tRefundFrm");
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //            tPrint = "--------N/A--------";
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //            //บิลคืน ถึง
        //            nStartY += 18;
        //            tPrint = oC_Resource.GetString("tRefundTo");
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //            tPrint = "--------N/A--------";
        //            oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //        }
        //        else
        //        {
        //            cmlTPSTShiftSLastDoc oShiftSLastDoc = new cmlTPSTShiftSLastDoc();
        //            oShiftSLastDoc = aoShiftSLastDoc.Where(a => a.FNLstDocType == 1).FirstOrDefault();
        //            if (oShiftSLastDoc == null)
        //            {
        //                //บิลขาย จาก
        //                nStartY += 18;
        //                tPrint = oC_Resource.GetString("tSaleFrm");
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = "--------N/A--------";
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //                //บิลขาย ถึง
        //                nStartY += 18;
        //                tPrint = oC_Resource.GetString("tSaleTo");
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = "--------N/A--------";
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //            }
        //            else
        //            {
        //                //บิลขาย จาก
        //                nStartY += 18;
        //                tPrint = oC_Resource.GetString("tSaleFrm");
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = oShiftSLastDoc.FTLstDocNoFrm;
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //                //บิลขาย ถึง
        //                nStartY += 18;
        //                tPrint = oC_Resource.GetString("tSaleTo");
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = oShiftSLastDoc.FTLstDocNoTo;
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //            }

        //            oShiftSLastDoc = new cmlTPSTShiftSLastDoc();
        //            oShiftSLastDoc = aoShiftSLastDoc.Where(a => a.FNLstDocType == 9).FirstOrDefault();
        //            if (oShiftSLastDoc == null)
        //            {
        //                //บิลคืน จาก
        //                nStartY += 18;
        //                tPrint = oC_Resource.GetString("tRefundFrm");
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = "--------N/A--------";
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //                //บิลคืน ถึง
        //                nStartY += 18;
        //                tPrint = oC_Resource.GetString("tRefundTo");
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = "--------N/A--------";
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //            }
        //            else
        //            {
        //                //บิลคืน จาก
        //                nStartY += 18;
        //                tPrint = oC_Resource.GetString("tRefundFrm");
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = oShiftSLastDoc.FTLstDocNoFrm;
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //                //บิลคืน ถึง
        //                nStartY += 18;
        //                tPrint = oC_Resource.GetString("tRefundTo");
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
        //                tPrint = oShiftSLastDoc.FTLstDocNoTo;
        //                oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
        //            }
        //        }

        //        //-----------------
        //        nStartY += 10;
        //        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

        //        //Print Date,Time
        //        nStartY += 18;
        //        tPrint = oC_Resource.GetString("tPrintDate") + ":" + DateTime.Now.ToString("dd/MM/yyyy");
        //        tPrint += " " + oC_Resource.GetString("tTime") + ":" + DateTime.Now.ToString("HH:mm:ss");
        //        oGraphic.DrawString(tPrint, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

        //        //ท้ายใบเสร็จ

        //        nStartY += 30;
        //        nStartY = oMsg.C_GETnSlipMsg(tConnStr, poPos.FTSmgCode, "2", oGraphic, nWidth, nStartY, aoFont[8]); // Footer Slip Msg

        //        Rectangle oCropRect = new Rectangle(0, 0, 280, nStartY + 15);
        //        Bitmap oBitmapEJ = new Bitmap(280, nStartY + 15, PixelFormat.Format64bppArgb);
        //        using (Graphics oGrp = Graphics.FromImage(oBitmapEJ))
        //        {
        //            oGrp.DrawImage(oNewBitmap, oCropRect, oCropRect, GraphicsUnit.Pixel);
        //        }
        //        tPathFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
        //        tPathFile = Directory.GetParent(Directory.GetParent(tPathFile).FullName).FullName;
        //        tPathFile += @"\AdaEJ\Image\" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("yyMMdd");

        //        //if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "SUM", tPathFile, poSalePos.ptConnStr, (int)poShopDB.nCommandTimeOut))
        //        if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "SUM", tPathFile, ref ptErrMsg)) //*Net 63-07-15
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception oEX)
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        if (oGraphic != null)
        //            oGraphic.Dispose();

        //        if (oFormatFar != null)
        //            oFormatFar.Dispose();

        //        if (oFormatCenter != null)
        //            oFormatCenter.Dispose();

        //        oGraphic = null;
        //        oFormatFar = null;
        //        oFormatCenter = null;
        //        aoDT = null;
        //        aoDTDis = null;
        //        oMsg = null;
        //        tLine = null;
        //        //oSP.SP_CLExMemory();
        //    }
        //}

        /// <summary>
        /// Get data Shift Last Doc.
        /// </summary>
        /// <returns></returns>
        private List<cmlTPSTShiftSLastDoc> C_GETaShiftSLastDoc(cmlRcvSalePos poSalePos, string ptConnStr, int pnCmdTime = 60)
        {
            List<cmlTPSTShiftSLastDoc> aoData = new List<cmlTPSTShiftSLastDoc>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT FNLstDocType,ISNULL(FTLstDocNoFrm,'--------N/A--------') AS FTLstDocNoFrm ,ISNULL(FTLstDocNoTo,'--------N/A--------') AS FTLstDocNoTo ");
                oSql.AppendLine("FROM TPSTShiftSLastDoc WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + poSalePos.ptPosCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                aoData = oDB.C_GETaDataQuery<cmlTPSTShiftSLastDoc>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cEJ", "C_GETaShiftSLastDoc : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }

        /// <summary>
        /// สรุปการแลกแต้มรับส่วนลด (Arm 63-05-28)
        /// </summary>
        /// <param name="pnDocType"> 1: แลกรับสินค้า, 2:แลกรับส่วนลด </param>
        /// <returns></returns>
        //*Net 63-07-17 C_GETaShiftSumRedeem เก่า
        //private List<cmlShiftSumRedeem> C_GETaShiftSumRedeem(cmlRcvSalePos poSalePos, int pnDocType, string ptConnStr, int pnCmdTime = 6)
        //{
        //    StringBuilder oSql;
        //    cDatabase oDB;
        //    List<cmlShiftSumRedeem> aoSumRed = new List<cmlShiftSumRedeem>();
        //    try
        //    {
        //        oSql = new StringBuilder();
        //        oDB = new cDatabase();

        //        if (pnDocType == 1)
        //        {
        //            //oSql.AppendLine("SELECT FTXshCardNo, SUM(FNXrdPntUse) AS FNXrdPntUse, FTXsdBarCode");
        //            //oSql.AppendLine("FROM ( ");
        //            //oSql.AppendLine("       SELECT DISTINCT HD.FTBchCode,HD.FTXshDocNo,HD.FTCstCode,CST.FTXshCardNo AS FTXshCardNo,RD.FNXrdSeqNo,ISNULL(RD.FNXrdPntUse,0) AS FNXrdPntUse, DT.FTXsdBarCode AS FTXsdBarCode");
        //            //oSql.AppendLine("       FROM TPSTSalHD HD with(nolock)");
        //            //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
        //            //oSql.AppendLine("       INNER JOIN TPSTSalDT DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
        //            //oSql.AppendLine("       INNER JOIN TPSTSalDTDis DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo");
        //            //oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode");
        //            //oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '1' ");
        //            //oSql.AppendLine(" )TRD");
        //            //oSql.AppendLine("GROUP BY FTXshCardNo,FTXsdBarCode");
        //            //oSql.AppendLine("ORDER BY FTXshCardNo");

        //            //*Arm 63-05-29 
        //            oSql.AppendLine("SELECT FTXshCardNo, FNXrdPntUse, FTXsdBarCode");
        //            oSql.AppendLine("FROM( ");
        //            oSql.AppendLine("   SELECT FTXshCardNo AS FTXshCardNo, SUM(FNXrdPntUse) AS FNXrdPntUse, FTXsdBarCode AS FTXsdBarCode");
        //            oSql.AppendLine("   FROM( "); // Use Redeem 
        //            oSql.AppendLine("       SELECT DISTINCT HD.FTBchCode, HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo AS FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) AS FNXrdPntUse, DT.FTXsdBarCode AS FTXsdBarCode ");
        //            oSql.AppendLine("       FROM TPSTSalHD HD with(nolock) ");
        //            oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo ");
        //            oSql.AppendLine("       INNER JOIN TPSTSalDT DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo ");
        //            oSql.AppendLine("       INNER JOIN TPSTSalDTDis DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo ");
        //            oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode ");
        //            oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '1'  AND HD.FNXshDocType = 1 ");
        //            oSql.AppendLine("       UNION ALL "); // Refund Redeem
        //            oSql.AppendLine("       SELECT DISTINCT HD.FTBchCode, HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo AS FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) * (-1) AS FNXrdPntUse, DT.FTXsdBarCode AS FTXsdBarCode");
        //            oSql.AppendLine("       FROM TPSTSalHD HD with(nolock) ");
        //            oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
        //            oSql.AppendLine("       INNER JOIN TPSTSalDT DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
        //            oSql.AppendLine("       INNER JOIN TPSTSalDTDis DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo");
        //            oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode");
        //            oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '1'  AND HD.FNXshDocType = 9");
        //            oSql.AppendLine("           AND HD.FTXshStaDoc='1'"); //*Net 63-07-17 สถานบิลสมบูรณ์
        //            oSql.AppendLine("   )TRD");
        //            oSql.AppendLine("   GROUP BY FTXshCardNo, FTXsdBarCode");
        //            oSql.AppendLine(")T");
        //            oSql.AppendLine("WHERE FNXrdPntUse > 0");
        //            oSql.AppendLine("ORDER BY FTXshCardNo");
        //        }
        //        else
        //        {
        //            //oSql.AppendLine("SELECT FTXshCardNo,SUM(FNXrdPntUse) AS FNXrdPntUse, SUM(FCXhdAmt) AS FCXhdAmt");
        //            //oSql.AppendLine("FROM ( ");
        //            //oSql.AppendLine("       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo,RD.FNXrdSeqNo,ISNULL(RD.FNXrdPntUse,0) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt,0) AS FCXhdAmt FROM TPSTSalHD HD with(nolock)");
        //            //oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
        //            //oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo ");
        //            //oSql.AppendLine("       INNER JOIN TPSTSalHDDis HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
        //            //oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '2' ");
        //            //oSql.AppendLine(" )TRD");
        //            //oSql.AppendLine("GROUP BY FTXshCardNo");
        //            //oSql.AppendLine("ORDER BY FTXshCardNo");

        //            //*Arm 63-05-29
        //            oSql.AppendLine("SELECT FTXshCardNo, FNXrdPntUse, FCXhdAmt");
        //            oSql.AppendLine("FROM(");
        //            oSql.AppendLine("   SELECT FTXshCardNo AS FTXshCardNo, SUM(FNXrdPntUse) AS FNXrdPntUse, SUM(FCXhdAmt) AS FCXhdAmt");
        //            oSql.AppendLine("   FROM(");
        //            oSql.AppendLine("       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt, 0) AS FCXhdAmt FROM TPSTSalHD HD with(nolock)");
        //            oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
        //            oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo");
        //            oSql.AppendLine("       INNER JOIN TPSTSalHDDis HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
        //            oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '2' AND HD.FNXshDocType = 1");
        //            oSql.AppendLine("       UNION ALL");
        //            oSql.AppendLine("       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) * (-1) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt, 0) * (-1) AS FCXhdAmt FROM TPSTSalHD HD with(nolock)");
        //            oSql.AppendLine("       INNER JOIN TPSTSalHDCst CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
        //            oSql.AppendLine("       INNER JOIN TPSTSalRD RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo");
        //            oSql.AppendLine("       INNER JOIN TPSTSalHDDis HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
        //            oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '2' AND HD.FNXshDocType = 9");
        //            oSql.AppendLine("   )TRD");
        //            oSql.AppendLine("   GROUP BY FTXshCardNo");
        //            oSql.AppendLine(")T");
        //            oSql.AppendLine("WHERE FNXrdPntUse > 0");
        //            oSql.AppendLine("ORDER BY FTXshCardNo");
        //        }

        //        aoSumRed = oDB.C_GETaDataQuery<cmlShiftSumRedeem>(ptConnStr, oSql.ToString(), pnCmdTime);
        //    }
        //    catch (Exception oEx)
        //    {
        //        new cLog().C_WRTxLog("cEJ", "C_GETaShiftSumRedeem : " + oEx.Message);
        //    }
        //    finally
        //    {
        //        oDB = null;
        //        oSql = null;
        //        //new cSP().SP_CLExMemory();
        //    }
        //    return aoSumRed;
        //}
        private List<cmlShiftSumRedeem> C_GETaShiftSumRedeem(cmlRcvSalePos poSalePos, int pnDocType, string ptConnStr, int pnCmdTime = 6)
        {
            List<cmlShiftSumRedeem> aoSumRed = new List<cmlShiftSumRedeem>();
            StringBuilder oSql;
            cDatabase oDB;

            string tTblSalHD;
            string tTblSalDT;
            string tTblSalHDCst;
            string tTblSalDTDis;
            string tTblSalHDDis;
            string tTblSalRD;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                tTblSalHD = "TPSTSalHD";
                tTblSalDT = "TPSTSalDT";
                tTblSalHDCst = "TPSTSalHDCst";
                tTblSalDTDis = "TPSTSalDTDis";
                tTblSalHDDis = "TPSTSalHDDis";
                tTblSalRD = "TPSTSalRD";
                if (pnDocType == 1)
                {

                    //*Arm 63-05-29 - แก้ไขแสดงแต้มที่ใช้ไปจริง
                    oSql.AppendLine("SELECT FTXshCardNo, FNXrdPntUse, FTXsdBarCode");
                    oSql.AppendLine("FROM( ");
                    oSql.AppendLine("   SELECT FTXshCardNo AS FTXshCardNo, SUM(FNXrdPntUse) AS FNXrdPntUse, FTXsdBarCode AS FTXsdBarCode");
                    oSql.AppendLine("   FROM( "); // Use Redeem 
                    oSql.AppendLine("       SELECT DISTINCT HD.FTBchCode, HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo AS FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) AS FNXrdPntUse, DT.FTXsdBarCode AS FTXsdBarCode ");
                    oSql.AppendLine($"       FROM {tTblSalHD} HD with(nolock) ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDCst} CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalDT} DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalDTDis} DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalRD} RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode ");
                    oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '1'  AND HD.FNXshDocType = 1 ");
                    oSql.AppendLine("           AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                    oSql.AppendLine("       UNION ALL "); // Refund Redeem
                    oSql.AppendLine("       SELECT DISTINCT HD.FTBchCode, HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo AS FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) * (-1) AS FNXrdPntUse, DT.FTXsdBarCode AS FTXsdBarCode");
                    oSql.AppendLine($"       FROM {tTblSalHD} HD with(nolock) ");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDCst} CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalDT} DT with(nolock) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalDTDis} DTDis with(nolock) ON DT.FTBchCode = DTDis.FTBchCode AND DT.FTXshDocNo = DTDis.FTXshDocNo AND DT.FNXsdSeqNo = DTDis.FNXsdSeqNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalRD} RD with(nolock) ON DTDis.FTBchCode = RD.FTBchCode AND DTDis.FTXshDocNo = RD.FTXshDocNo AND DTDis.FNXsdSeqNo = RD.FNXrdRefSeq AND DTDis.FTXddRefCode = RD.FTXrdRefCode");
                    oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '1'  AND HD.FNXshDocType = 9");
                    oSql.AppendLine("         AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                    oSql.AppendLine("   )TRD");
                    oSql.AppendLine("   GROUP BY FTXshCardNo, FTXsdBarCode");
                    oSql.AppendLine(")T");
                    oSql.AppendLine("WHERE FNXrdPntUse > 0");
                    oSql.AppendLine("ORDER BY FTXshCardNo");
                }
                else
                {

                    //*Arm 63-05-29 - แก้ไขแสดงแต้มที่ใช้ไปจริง
                    oSql.AppendLine("SELECT FTXshCardNo, FNXrdPntUse, FCXhdAmt");
                    oSql.AppendLine("FROM(");
                    oSql.AppendLine("   SELECT FTXshCardNo AS FTXshCardNo, SUM(FNXrdPntUse) AS FNXrdPntUse, SUM(FCXhdAmt) AS FCXhdAmt");
                    oSql.AppendLine("   FROM(");
                    oSql.AppendLine($"       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt, 0) AS FCXhdAmt FROM {tTblSalHD} HD with(nolock)");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDCst} CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalRD} RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDDis} HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
                    oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '2' AND HD.FNXshDocType = 1");
                    oSql.AppendLine("           AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                    oSql.AppendLine("       UNION ALL");
                    oSql.AppendLine($"       SELECT DISTINCT HD.FTXshDocNo, HD.FTCstCode, CST.FTXshCardNo, RD.FNXrdSeqNo, ISNULL(RD.FNXrdPntUse, 0) * (-1) AS FNXrdPntUse, ISNULL(HDDis.FCXhdAmt, 0) * (-1) AS FCXhdAmt FROM {tTblSalHD} HD with(nolock)");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDCst} CST with(nolock) ON HD.FTBchCode = CST.FTBchCode AND HD.FTXshDocNo = CST.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalRD} RD with(nolock) ON HD.FTBchCode = RD.FTBchCode AND HD.FTXshDocNo = RD.FTXshDocNo");
                    oSql.AppendLine($"       INNER JOIN {tTblSalHDDis} HDDis with(nolock) ON  RD.FTBchCode = HDDis.FTBchCode AND  RD.FTXshDocNo = HDDis.FTXshDocNo AND RD.FTXrdRefCode = HDDis.FTXhdRefCode");
                    oSql.AppendLine("       WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfcode = '" + poSalePos.ptXihDocNo + "' AND RD.FTRdhDocType = '2' AND HD.FNXshDocType = 9");
                    oSql.AppendLine("           AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                    oSql.AppendLine("   )TRD");
                    oSql.AppendLine("   GROUP BY FTXshCardNo");
                    oSql.AppendLine(")T");
                    oSql.AppendLine("WHERE FNXrdPntUse > 0");
                    oSql.AppendLine("ORDER BY FTXshCardNo");
                }
                //+++++++++++++

                aoSumRed = oDB.C_GETaDataQuery<cmlShiftSumRedeem>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cEJ", "C_GETaShiftSumRedeem : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
            return aoSumRed;
        }

        /// <summary>
        /// Get data currency in shift
        /// </summary>
        /// <returns></returns>
        private List<cmlTPSTShiftSRatePdt> C_GETaShiftSRate(cmlRcvSalePos poSalePos, string ptConnStr, int pnCmdTime = 60)
        {
            List<cmlTPSTShiftSRatePdt> aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT FTSrpNameRef,FCSrpQty,FCSrpAmt FROM ("); //*Net 63-07-17
                //oSql.AppendLine("SELECT ISNULL(FTSrpNameRef,(SELECT TOP 1 FTRteName FROM TFNMRate_L WITH(NOLOCK) WHERE FTRteCode = SRP.FTSrpCodeRef)) AS FTSrpNameRef,FCSrpQty,FCSrpAmt");
                oSql.AppendLine("SELECT ISNULL(FTSrpNameRef,(SELECT TOP 1 FTRteName FROM TFNMRate_L WITH(NOLOCK) WHERE FTRteCode = SRP.FTSrpCodeRef)) AS FTSrpNameRef,FCSrpQty,FCSrpAmt,RTE.FTRteStaLocal,COUNT(*) OVER() AS nCount"); //*Net 63-07-17
                oSql.AppendLine("FROM TPSTShiftSRatePdt SRP WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMRate_L RTEL WITH(NOLOCK) ON SRP.FTSrpCodeRef = RTEL.FTRteCode AND RTEL.FNLngID = " + nC_Language);
                oSql.AppendLine("INNER JOIN TFNMRate RTE WITH(NOLOCK) ON RTEL.FTRteCode=RTE.FTRteCode"); //*Net 63-07-17
                oSql.AppendLine("WHERE SRP.FTBchCode = '" + poSalePos.ptBchCode + "'");
                oSql.AppendLine("AND SRP.FTPosCode = '" + poSalePos.ptPosCode + "'");
                oSql.AppendLine("AND SRP.FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                oSql.AppendLine("AND SRP.FTSrpType = 1");
                oSql.AppendLine(")t WHERE (nCount>1) OR (nCount=1 AND ISNULL(FTRteStaLocal,'')<>'1')"); //*Net 63-07-17
                aoShiftSRatePdt = oDB.C_GETaDataQuery<cmlTPSTShiftSRatePdt>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cEJ", "C_GETaShiftSRate : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
            return aoShiftSRatePdt;
        }

        /// <summary>
        /// Get data product spacial in shift
        /// </summary>
        /// <returns></returns>
        private List<cmlTPSTShiftSRatePdt> C_GETaShiftSPdt(cmlRcvSalePos poSalePos, string ptConnStr, int pnCmdTime = 60)
        {
            List<cmlTPSTShiftSRatePdt> aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT ISNULL(FTSrpNameRef,(SELECT TOP 1 FTPdtNameABB FROM TCNMPdt_L WITH(NOLOCK) WHERE FTPdtCode = SRP.FTSrpCodeRef)) AS FTSrpNameRef,FCSrpQty,FCSrpAmt");
                oSql.AppendLine("FROM TPSTShiftSRatePdt SRP WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMPdt_L PDTL WITH(NOLOCK) ON SRP.FTSrpCodeRef = PDTL.FTPdtCode AND PDTL.FNLngID = " + nC_Language);
                oSql.AppendLine("WHERE SRP.FTBchCode = '" + poSalePos.ptBchCode + "'");
                oSql.AppendLine("AND SRP.FTPosCode = '" + poSalePos.ptPosCode + "'");
                oSql.AppendLine("AND SRP.FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                oSql.AppendLine("AND SRP.FTSrpType = 2");
                aoShiftSRatePdt = oDB.C_GETaDataQuery<cmlTPSTShiftSRatePdt>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cShift", "C_GETaShiftSPdt : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
            return aoShiftSRatePdt;
        }
        private List<cmlShiftSumRcv> C_GETaShiftSumRcv(cmlRcvSalePos poSalePos, string ptConnStr, int pnCmdTime = 60)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlShiftSumRcv> aoData = new List<cmlShiftSumRcv>();
            try
            {
                oSql.AppendLine("SELECT ISNULL(RCVL.FTRcvName,(SELECT TOP 1 FTRcvName FROM TFNMRcv_L WITH(NOLOCK) WHERE FTRcvCode = SSR.FTRcvCode)) AS FTRcvName,");
                //oSql.AppendLine("SSR.FTRcvCode,SSR.FCRcvPayAmt,SSR.FTRcvDocType");
                oSql.AppendLine("SSR.FTRcvCode,SSR.FCRcvPayAmt,SSR.FTRcvDocType,RCV.FTFmtCode"); //*Net 63-06-19
                oSql.AppendLine("FROM TPSTShiftSSumRcv SSR WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TFNMRcv RCV WITH(NOLOCK) ON RCV.FTRcvCode=SSR.FTRcvCode"); //*Net 63-06-19
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCVL.FTRcvCode = SSR.FTRcvCode AND RCVL.FNLngID = " + nC_Language);
                oSql.AppendLine("WHERE SSR.FTBchCode = '" + poSalePos.ptBchCode + "'");
                oSql.AppendLine("AND SSR.FTPosCode = '" + poSalePos.ptPosCode + "'");
                oSql.AppendLine("AND SSR.FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                oSql.AppendLine("ORDER BY SSR.FTShfCode,SSR.FTRcvCode,SSR.FTRcvDocType");
                aoData = oDB.C_GETaDataQuery<cmlShiftSumRcv>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cEJ", "C_GETaShiftSumRcv : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
            return aoData;
        }
        private cmlShiftSum C_GEToShiftSum(cmlRcvSalePos poSalePos, string ptConnStr, int pnCmdTime = 60)
        {
            string tTblSalHD;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            cmlShiftSum oData = new cmlShiftSum();
            DataTable odtTmp;
            try
            {
                oData.nCntOpDrw = 0;
                oData.nCntCancelBill = 0;
                oData.nCntHoldBill = 0;
                oData.nMnyInCnt = 0;
                oData.cMnyInAmt = 0;
                oData.nMnyOutCnt = 0;
                oData.cMnyOutAmt = 0;
                oData.nSaleCnt = 0;
                oData.cSaleAmt = 0;
                oData.nRefundCnt = 0;
                oData.cRefundAmt = 0;
                oData.cRoundAmt = 0; //*Net 63-07-17

                tTblSalHD = "TPSTSalHD";

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT HD.FTBchCode,HD.FTShfCode,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 1 THEN 1 ELSE 0 END) AS FNCntSale,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 1 THEN HD.FCXshGrand-HD.FCXshRnd ELSE 0 END) AS FCAmtSale,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 9 THEN 1 ELSE 0 END) AS FNCntRef,");
                oSql.AppendLine("SUM(CASE WHEN HD.FNXshDocType = 9 THEN HD.FCXshGrand-HD.FCXshRnd ELSE 0 END) AS FCAmtRef,");
                oSql.AppendLine("SUM(HD.FCXshRnd) AS FCXshRnd"); //*Net 63-06-19
                //oSql.AppendLine("FROM TPSTSalHD HD WITH(NOLOCK)");
                oSql.AppendLine($"FROM {tTblSalHD} HD WITH(NOLOCK)");
                oSql.AppendLine("WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTShfCode = '" + poSalePos.ptXihDocNo + "' ");
                oSql.AppendLine("AND HD.FTPosCode = '" + poSalePos.ptPosCode + "' ");
                oSql.AppendLine(" AND HD.FTXshStaDoc='1'"); //*Net 63-06-04 สถานบิลสมบูรณ์
                oSql.AppendLine("GROUP BY HD.FTBchCode,HD.FTShfCode,HD.FTPosCode");
                odtTmp = new DataTable();
                odtTmp = oDB.C_DAToExecuteQuery(ptConnStr, oSql.ToString(), pnCmdTime);
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        oData.nSaleCnt = odtTmp.Rows[0].Field<int>("FNCntSale");
                        oData.cSaleAmt = odtTmp.Rows[0].Field<decimal>("FCAmtSale");
                        oData.nRefundCnt = odtTmp.Rows[0].Field<int>("FNCntRef");
                        oData.cRefundAmt = odtTmp.Rows[0].Field<decimal>("FCAmtRef");
                        oData.cRoundAmt = odtTmp.Rows[0].Field<decimal>("FCXshRnd"); //*Net 63-06-19
                    }
                }

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode,FTPosCode,FTShfCode,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '004' THEN 1 ELSE 0 END) AS FNCntOpDrw,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '006'  THEN 1 ELSE 0 END) AS FNCntCancelBill,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '003'  THEN 1 ELSE 0 END) AS FNCntHoldBill,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '001'  THEN 1 ELSE 0 END) AS FNMnyInCnt,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '001'  THEN FCSvnAmt ELSE 0 END) AS FCMnyInAmt,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '002'  THEN 1 ELSE 0 END) AS FNMnyOutCnt,");
                oSql.AppendLine("SUM(CASE WHEN FTEvnCode = '002'  THEN FCSvnAmt ELSE 0 END) AS FCMnyOutAmt");
                oSql.AppendLine("FROM TPSTShiftEvent WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + poSalePos.ptPosCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                oSql.AppendLine("GROUP BY FTBchCode,FTPosCode,FTShfCode");
                odtTmp = new DataTable();
                odtTmp = oDB.C_DAToExecuteQuery(ptConnStr, oSql.ToString(), pnCmdTime);
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        oData.nCntOpDrw = odtTmp.Rows[0].Field<int>("FNCntOpDrw");
                        oData.nCntCancelBill = odtTmp.Rows[0].Field<int>("FNCntCancelBill");
                        oData.nCntHoldBill = odtTmp.Rows[0].Field<int>("FNCntHoldBill");
                        oData.nMnyInCnt = odtTmp.Rows[0].Field<int>("FNMnyInCnt");
                        oData.cMnyInAmt = odtTmp.Rows[0].Field<decimal>("FCMnyInAmt");
                        oData.nMnyOutCnt = odtTmp.Rows[0].Field<int>("FNMnyOutCnt");
                        oData.cMnyOutAmt = odtTmp.Rows[0].Field<decimal>("FCMnyOutAmt");
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cEJ", "C_GEToShiftSum : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
            return oData;
        }
        public bool C_GENbSlipSUM(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, cmlTCNMPos poPos, ref string ptErrMsg)
        {
            List<cmlTPSTShiftSRatePdt> aoShiftSRatePdt;
            List<cmlTPSTShiftSLastDoc> aoShiftSLastDoc;
            List<cmlShiftSumRedeem> aoShiftSumRedeemDis;
            List<cmlShiftSumRedeem> aoShiftSumRedeemPdt;
            List<cmlShiftSumRcv> aoShiftSSumRcv;

            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            cmlTPSTShiftHD oShiftHD;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            Font oFont = new Font("CordiaUPC", Convert.ToSingle(11.5), FontStyle.Regular);
            cmlShiftSum oSumShift;
            Image oLogo;

            decimal cLastAmt, cSumShift;
            decimal cSumSale, cSumRet, cSumCashSale;

            int nStartY = 0, nWidth;

            string tLine;
            string tLastRcv;
            string tMsg = "";
            string tPathFile;
            try
            {

                tLine = "--------------------------------------------------------------------------------------------";
                nWidth = 280;


                oSumShift = new cmlShiftSum();
                oSumShift = C_GEToShiftSum(poSalePos, tC_tConnStr, nC_CmdTimeout);
                if (oSumShift == null)
                {
                    throw new Exception($"Bch{poSalePos.ptBchCode} Pos{poSalePos.ptPosCode} Shf{poSalePos.ptXihDocNo} not found");
                }
                oSql.Clear();
                oSql.AppendLine("SELECT HD.FDShdSaleDate,ISNULL(USR.FTUsrName,HD.FTShdUsrClosed) AS FTUsrCode  ");
                oSql.AppendLine("FROM TPSTShiftHD HD WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN  TCNMUser_L USR WITH(NOLOCK) ON HD.FTShdUsrClosed = USR.FTUsrCode AND USR.FNLngID = " + nC_Language);
                oSql.AppendLine("WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTPosCode = '" + poSalePos.ptPosCode + "' AND HD.FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                oShiftHD = oDB.C_DAToExecuteQuery<cmlTPSTShiftHD>(tC_tConnStr, oSql.ToString(), nC_CmdTimeout);
                if (oShiftHD == null) return false;

                Bitmap oNewBitmap = new Bitmap(nWidth, 20000, PixelFormat.Format64bppArgb);
                oGraphic = Graphics.FromImage(oNewBitmap);
                oGraphic.FillRectangle(Brushes.White, new RectangleF(0, 0, nWidth, 20000));

                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };
                oMsg = new cSlipMsg();

                if (!String.IsNullOrEmpty(tC_PathLogo))
                {
                    if (File.Exists(tC_PathLogo))
                    {
                        oLogo = Image.FromFile(tC_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - 200) / 2, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }

                nStartY = oMsg.C_GETnSlipMsg(tC_tConnStr, poPos.FTSmgCode, "1", oGraphic, nWidth, nStartY, aoC_Font[0]);    // Header Slip Msg

                tMsg = oC_Resource.GetString("tShiftSUM").ToString(); ;
                oGraphic.DrawString(tMsg, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                //----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //!!!ปิดการขาย!!!
                nStartY += 18;
                tMsg = oC_Resource.GetString("tSaleDate").ToString() + ":" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("dd/MM/yyyy");
                tMsg += " " + oC_Resource.GetString("tPos").ToString() + ":" + poSalePos.ptPosCode;
                tMsg += " " + oC_Resource.GetString("tUsr").ToString() + ":" + oShiftHD.FTUsrCode;
                oGraphic.DrawString(tMsg, aoC_Font[2], Brushes.Black, 0, nStartY);

                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รายการ ความถี่ มูลค่า
                nStartY += 18;
                tMsg = oC_Resource.GetString("tItem");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oC_Resource.GetString("tFreq");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatCenter);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oC_Resource.GetString("tAmount");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //เปิดลิ้นชักไม่ขาย
                nStartY += 18;
                tMsg = oC_Resource.GetString("tOpDrwNoS");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nCntOpDrw, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

                //ยกเลิกบิล
                nStartY += 18;
                tMsg = oC_Resource.GetString("tCancelBill");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nCntCancelBill, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

                //พักบิล
                nStartY += 18;
                tMsg = oC_Resource.GetString("tHoldBill");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nCntHoldBill, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);

                //นำเงินเข้า
                nStartY += 18;
                tMsg = oC_Resource.GetString("tMnyIn");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nMnyInCnt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cMnyInAmt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //นำเงินออก
                nStartY += 18;
                tMsg = oC_Resource.GetString("tMnyOut");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nMnyOutCnt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cMnyOutAmt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //ยอดขาย
                nStartY += 18;
                tMsg = oC_Resource.GetString("tSaleAmt");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nSaleCnt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cSaleAmt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //ยอดคืนสินค้า
                nStartY += 18;
                tMsg = oC_Resource.GetString("tRefundAmt");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.nRefundCnt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(106, nStartY, 50, 18), oFormatFar);
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cRefundAmt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);


                //ยอดปัดเศษ //*Net 63-06-19 เพิ่ม
                nStartY += 18;
                tMsg = oC_Resource.GetString("tRoundRcv");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(101, nStartY, 5, 18), oFormatCenter);

                oGraphic.DrawString("|", oFont, Brushes.Black, new RectangleF(156, nStartY, 5, 18), oFormatCenter);
                tMsg = oSP.SP_SETtDecShwSve(1, oSumShift.cRoundAmt, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //รวมเงิน
                nStartY += 18;
                tMsg = "    " + oC_Resource.GetString("tTotal") + ":";
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                //cSumShift = (oSumShift.cMnyInAmt + oSumShift.cSaleAmt) - (oSumShift.cMnyOutAmt + oSumShift.cRefundAmt);
                cSumShift = (oSumShift.cMnyInAmt + oSumShift.cSaleAmt + oSumShift.cRoundAmt) - (oSumShift.cMnyOutAmt + oSumShift.cRefundAmt); //*Net 63-07-17 เพิ่มยอดปัดเศษ
                tMsg = oSP.SP_SETtDecShwSve(1, cSumShift, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(151, nStartY, 120, 18), oFormatFar);

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //สรุปยอดรับชำระตามประเภทการชำระเงิน
                aoShiftSSumRcv = new List<cmlShiftSumRcv>();
                aoShiftSSumRcv = C_GETaShiftSumRcv(poSalePos, tC_tConnStr, nC_CmdTimeout);
                tLastRcv = "";
                cLastAmt = 0;
                //*Net 63-06-19
                cSumSale = 0;
                cSumRet = 0;
                cSumCashSale = 0;
                //++++++++++++++++++
                if (aoShiftSSumRcv.Count > 0)
                {
                    foreach (cmlShiftSumRcv oShift in aoShiftSSumRcv)
                    {
                        //*Net 63-06-19 ปรับการพิมพ์ใหม่
                        if (!String.IsNullOrEmpty(tLastRcv) && tLastRcv != oShift.FTRcvCode)
                        {
                            //-----------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                        }

                        //* มีทั้งบิลขาย และคืน
                        if (aoShiftSSumRcv.Count(oCountShf => oCountShf.FTRcvCode == oShift.FTRcvCode) > 1)
                        {
                            if (string.Equals(oShift.FTRcvDocType, "9"))
                            {
                                tMsg = oC_Resource.GetString("tRefund") + " " + oShift.FTRcvName;
                                cSumRet += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                if (oShift.FTFmtCode == "001") cSumCashSale -= Convert.ToDecimal(oShift.FCRcvPayAmt);

                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                                //-----------------------
                                nStartY += 10;
                                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                                //รวมเงิน
                                nStartY += 18;
                                tMsg = "    " + oC_Resource.GetString("tTotal") + " " + oShift.FTRcvName; ;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(cLastAmt - oShift.FCRcvPayAmt), nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                            }
                            else
                            {
                                tMsg = oC_Resource.GetString("tSale") + " " + oShift.FTRcvName;
                                cSumSale += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                if (oShift.FTFmtCode == "001") cSumCashSale += Convert.ToDecimal(oShift.FCRcvPayAmt);

                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                            }

                        }
                        //* มีแค่ขาย หรือคืนอย่างเดียว
                        else
                        {
                            //*มียอดคืนอย่างเดียว
                            if (string.Equals(oShift.FTRcvDocType, "9"))
                            {
                                //*พิมพ์ยอดบิลขายเป็น 0.00
                                tMsg = oC_Resource.GetString("tSale") + " " + oShift.FTRcvName;
                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, 0m, nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                                //*พิมพ์ยอดบิลคืนปกติ
                                tMsg = oC_Resource.GetString("tRefund") + " " + oShift.FTRcvName;
                                cSumRet += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                if (oShift.FTFmtCode == "001") cSumCashSale -= Convert.ToDecimal(oShift.FCRcvPayAmt);
                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                                cLastAmt = -Convert.ToDecimal(oShift.FCRcvPayAmt);
                            }
                            //*มียอดขายอย่างเดียว
                            else
                            {
                                //* พิมพ์ยอดบิลขายปกติ
                                tMsg = oC_Resource.GetString("tSale") + " " + oShift.FTRcvName;
                                cSumSale += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                if (oShift.FTFmtCode == "001") cSumCashSale += Convert.ToDecimal(oShift.FCRcvPayAmt);
                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oShift.FCRcvPayAmt), nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                                cLastAmt = Convert.ToDecimal(oShift.FCRcvPayAmt);

                                //*พิมพ์ยอดบิลคืนเป็น 0.00
                                tMsg = oC_Resource.GetString("tRefund") + " " + oShift.FTRcvName;
                                nStartY += 18;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, 0m, nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                            }
                            //-----------------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                            //รวมเงิน
                            nStartY += 18;
                            tMsg = "    " + oC_Resource.GetString("tTotal") + " " + oShift.FTRcvName; ;
                            oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                            tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(cLastAmt), nC_DecShow);
                            oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                        }
                        tLastRcv = oShift.FTRcvCode;
                        cLastAmt = Convert.ToDecimal(oShift.FCRcvPayAmt);

                        //*Net 63-06-19 พิมพ์ขีดคั่นตอนจบรายการสุดท้าย
                        if (oShift == aoShiftSSumRcv.LastOrDefault())
                        {
                            //-----------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                        }
                    }
                }


                //รายละเอียดสกุลเงิน
                aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
                aoShiftSRatePdt = C_GETaShiftSRate(poSalePos, tC_tConnStr, nC_CmdTimeout);
                if (aoShiftSRatePdt.Count > 0)
                {
                    //สรุปการรับชำระสกุลเงิน
                    nStartY += 18;
                    tMsg = "    " + oC_Resource.GetString("tSumCurrency");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                    //ส่วนหัวสกุลเงิน
                    nStartY += 18;
                    tMsg = oC_Resource.GetString("tCurrency");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                    tMsg = oC_Resource.GetString("tAmount");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                    foreach (cmlTPSTShiftSRatePdt oData in aoShiftSRatePdt)
                    {
                        nStartY += 18;
                        tMsg = oData.FTSrpNameRef;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                        tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpAmt), nC_DecShow);
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);
                    }

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                }

                //รายการสินค้าควบคุม
                aoShiftSRatePdt = new List<cmlTPSTShiftSRatePdt>();
                aoShiftSRatePdt = C_GETaShiftSPdt(poSalePos, tC_tConnStr, nC_CmdTimeout);
                if (aoShiftSRatePdt.Count > 0)
                {
                    //สรุปยอดขายสินค้าควบคุมพิเศษ
                    nStartY += 18;
                    tMsg = "    " + oC_Resource.GetString("tPdtSpcSum");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                    //ส่วนหัวสินค้าควบคุม
                    nStartY += 18;
                    tMsg = oC_Resource.GetString("tItem");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = oC_Resource.GetString("tQty");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, 50, 18), oFormatCenter);
                    tMsg = oC_Resource.GetString("tAmount");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                    foreach (cmlTPSTShiftSRatePdt oData in aoShiftSRatePdt)
                    {
                        nStartY += 18;
                        tMsg = oData.FTSrpNameRef;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpQty), nC_DecShow);
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, 50, 18), oFormatFar);
                        tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCSrpAmt), nC_DecShow);
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
                    }

                    //-----------------
                    nStartY += 10;
                    oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                }

                //*Net 63-06-19 พิมพ์สรุปยอด
                //สรุปยอดเงินขาย
                nStartY += 18;
                tMsg = oC_Resource.GetString("tSumSale");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, cSumSale, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //สรุปยอดเงินคืน
                nStartY += 18;
                tMsg = oC_Resource.GetString("tSumRet");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                tMsg = oSP.SP_SETtDecShwSve(1, cSumRet, nC_DecShow);
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //สรุปยอดเงินในลิ้นชัก
                nStartY += 18;
                tMsg = oC_Resource.GetString("tSumDrw");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 150, 18));
                //tMsg = oSP.SP_SETtDecShwSve(1, cSumCashSale + oSumShift.cRoundAmt + oSumShift.cMnyInAmt - oSumShift.cMnyOutAmt, nC_DecShow);
                tMsg = oSP.SP_SETtDecShwSve(1, cSumCashSale + oSumShift.cMnyInAmt - oSumShift.cMnyOutAmt, nC_DecShow); //*Net 63-07-10 ไม่รวมยอดปัดเศษ
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(161, nStartY, 110, 18), oFormatFar);

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                //+++++++++++++++++++++++++++++++++++


                //ช่วงบิล
                aoShiftSLastDoc = new List<cmlTPSTShiftSLastDoc>();
                aoShiftSLastDoc = C_GETaShiftSLastDoc(poSalePos, tC_tConnStr, nC_CmdTimeout);

                if (aoShiftSLastDoc.Count == 0)
                {
                    //บิลขาย จาก
                    nStartY += 18;
                    tMsg = oC_Resource.GetString("tSaleFrm");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = "--------N/A--------";
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    //บิลขาย ถึง
                    nStartY += 18;
                    tMsg = oC_Resource.GetString("tSaleTo");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = "--------N/A--------";
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    //บิลคืน จาก
                    nStartY += 18;
                    tMsg = oC_Resource.GetString("tRefundFrm");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = "--------N/A--------";
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    //บิลคืน ถึง
                    nStartY += 18;
                    tMsg = oC_Resource.GetString("tRefundTo");
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                    tMsg = "--------N/A--------";
                    oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                }
                else
                {
                    cmlTPSTShiftSLastDoc oShiftSLastDoc = new cmlTPSTShiftSLastDoc();
                    oShiftSLastDoc = aoShiftSLastDoc.Where(a => a.FNLstDocType == 1).FirstOrDefault();
                    if (oShiftSLastDoc == null)
                    {
                        //บิลขาย จาก
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tSaleFrm");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = "--------N/A--------";
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                        //บิลขาย ถึง
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tSaleTo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = "--------N/A--------";
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    }
                    else
                    {
                        //บิลขาย จาก
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tSaleFrm");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oShiftSLastDoc.FTLstDocNoFrm;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                        //บิลขาย ถึง
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tSaleTo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oShiftSLastDoc.FTLstDocNoTo;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    }

                    oShiftSLastDoc = new cmlTPSTShiftSLastDoc();
                    oShiftSLastDoc = aoShiftSLastDoc.Where(a => a.FNLstDocType == 9).FirstOrDefault();
                    if (oShiftSLastDoc == null)
                    {
                        //บิลคืน จาก
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tRefundFrm");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = "--------N/A--------";
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                        //บิลคืน ถึง
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tRefundTo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = "--------N/A--------";
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    }
                    else
                    {
                        //บิลคืน จาก
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tRefundFrm");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oShiftSLastDoc.FTLstDocNoFrm;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                        //บิลคืน ถึง
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tRefundTo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 120, 18));
                        tMsg = oShiftSLastDoc.FTLstDocNoTo;
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(121, nStartY, nWidth - 120, 18));
                    }
                }

                //-----------------
                nStartY += 10;
                oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                //สรุปการแลกแต้ม รับส่วนลด
                if (bC_PrnShiftSumRedeem == true)
                {
                    //รายละเอียดการแลกแต้ม รับส่วนลด
                    aoShiftSumRedeemDis = new List<cmlShiftSumRedeem>();
                    aoShiftSumRedeemDis = C_GETaShiftSumRedeem(poSalePos, 2, tC_tConnStr, nC_CmdTimeout);

                    //รายละเอียดการแลกแต้ม รับสินค้า
                    aoShiftSumRedeemPdt = new List<cmlShiftSumRedeem>();
                    aoShiftSumRedeemPdt = C_GETaShiftSumRedeem(poSalePos, 1, tC_tConnStr, nC_CmdTimeout);

                    if (aoShiftSumRedeemDis.Count > 0 || aoShiftSumRedeemPdt.Count > 0)
                    {
                        nStartY += 18;
                        tMsg = "    " + oC_Resource.GetString("tSumRedeem");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                        //-----------------
                        nStartY += 10;
                        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                        //ส่วนหัวการแลกแต้ม รับส่วนลด
                        nStartY += 18;
                        tMsg = oC_Resource.GetString("tCardNo");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
                        tMsg = oC_Resource.GetString("tUsePnt");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatCenter);
                        tMsg = oC_Resource.GetString("tAmtDis");
                        oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);

                        //-----------------
                        nStartY += 10;
                        oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                        if (aoShiftSumRedeemDis.Count > 0)
                        {
                            foreach (cmlShiftSumRedeem oData in aoShiftSumRedeemDis)
                            {
                                nStartY += 18;
                                tMsg = oData.FTXshCardNo;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FNXrdPntUse), nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatFar);
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FCXhdAmt), nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
                            }

                            //-----------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                        }

                        if (aoShiftSumRedeemPdt.Count > 0)
                        {
                            foreach (cmlShiftSumRedeem oData in aoShiftSumRedeemPdt)
                            {
                                nStartY += 18;
                                tMsg = oData.FTXshCardNo;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, 100, 18));
                                tMsg = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oData.FNXrdPntUse), nC_DecShow);
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(101, nStartY, 70, 18), oFormatFar);
                                tMsg = oData.FTXsdBarCode;
                                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(171, nStartY, 100, 18), oFormatFar);
                            }

                            //-----------------
                            nStartY += 10;
                            oGraphic.DrawString(tLine, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                        }
                    }

                }
                //End(*Arm 63-05-28)



                //Print Date,Time
                nStartY += 18;
                tMsg = oC_Resource.GetString("tMsgDate") + ":" + DateTime.Now.ToString("dd/MM/yyyy");
                tMsg += " " + oC_Resource.GetString("tTime") + ":" + DateTime.Now.ToString("HH:mm:ss");
                oGraphic.DrawString(tMsg, oFont, Brushes.Black, new RectangleF(0, nStartY, nWidth, 18));

                //ท้ายใบเสร็จ

                nStartY += 30;
                nStartY = oMsg.C_GETnSlipMsg(tC_tConnStr, poPos.FTSmgCode, "2", oGraphic, nWidth, nStartY, aoC_Font[8]); // Footer Slip Msg


                Rectangle oCropRect = new Rectangle(0, 0, 280, nStartY + 15);
                Bitmap oBitmapEJ = new Bitmap(280, nStartY + 15, PixelFormat.Format64bppArgb);
                using (Graphics oGrp = Graphics.FromImage(oBitmapEJ))
                {
                    oGrp.DrawImage(oNewBitmap, oCropRect, oCropRect, GraphicsUnit.Pixel);
                }
                tPathFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
                tPathFile = Directory.GetParent(Directory.GetParent(tPathFile).FullName).FullName;
                //tPathFile += @"\AdaEJ\Image\" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("yyMMdd"); //*Net 63-07-23 ปรับ Folder ที่เก็บ
                tPathFile += $@"\AdaEJ\{poSalePos.ptBchCode}\" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("yyMMdd");

                //if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "SUM", tPathFile, poSalePos.ptC_tConnStr, (int)poShopDB.nCommandTimeOut))
                if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "SUM", tPathFile, ref ptErrMsg)) //*Net 63-07-15
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message;
                new cLog().C_WRTxLog("cEJ", "C_GENbSlipSUM : " + oEx.Message);
                return false;
            }
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
                oMsg = null;
                tLine = null;
                //oSP.SP_CLExMemory();
            }
        }



        private List<cmlShiftBN> C_GETaShiftBN(cmlRcvSalePos poSalePos, string ptConnStr, int pnCmdTime = 60)
        {
            List<cmlShiftBN> aoShiftBN = new List<cmlShiftBN>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT ISNULL(BNL.FTBntName,(SELECT TOP 1 FTBntName FROM TFNMBankNote_L WITH(NOLOCK) WHERE FTBntCode = BN.FTBntCode)) AS FTBntName,BN.FNKbnQty,BN.FCKbnAmt ");
                oSql.AppendLine("FROM TPSTShiftSKeyBN BN WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMBankNote_L BNL WITH(NOLOCK) ON BN.FTBntCode = BNL.FTBntCode AND BNL.FNLngID = " + nC_Language);
                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + poSalePos.ptPosCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                oSql.AppendLine("ORDER BY BN.FTBntCode");
                aoShiftBN = oDB.C_GETaDataQuery<cmlShiftBN>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cEJ", "C_GETaShiftBN : " + oEx.Message); }
            return aoShiftBN;
        }
        //public bool C_GENbSlipBNK(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, cmlTCNMPos poPos) //*Net 63-07-14 เพิ่ม ptErrMsg
        public bool C_GENbSlipBNK(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, cmlTCNMPos poPos, ref string ptErrMsg)
        {
            List<cmlShiftBN> aoShiftBN;
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            cmlTPSTShiftHD oShiftHD = new cmlTPSTShiftHD();
            Image oLogo;
            string tConnStr = poSalePos.ptConnStr;
            int nCmdTime = Convert.ToInt32(poShopDB.nCommandTimeOut);
            string tPathFile;
            string tPrint = "";
            string tAmt;
            string tLine;
            int nStartY = 0, nWidth;

            try
            {
                nWidth = 280;
                tLine = "........................................................................";

                #region Comment
                //oSql.Clear();
                //oSql.AppendLine("SELECT FTSysSeq,FTSysStaDefValue,FTSysStaUsrValue");
                //oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTSysCode = 'PInvLayout'");
                //oSql.AppendLine("AND (CONVERT(INT,FTSysSeq) BETWEEN 1 AND 9)");
                //oSql.AppendLine("ORDER BY CONVERT(INT,FTSysSeq)");
                //DataTable odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), nCmdTime);
                //if (odtTmp != null)
                //{
                //    aoC_Font = new List<Font>();
                //    foreach (DataRow oRow in odtTmp.Rows)
                //    {
                //        string[] atFont;
                //        if (string.IsNullOrEmpty(oRow.Field<string>("FTSysStaUsrValue")))
                //        {
                //            atFont = oRow.Field<string>("FTSysStaDefValue").Split(',');

                //        }
                //        else
                //        {
                //            atFont = oRow.Field<string>("FTSysStaDefValue").Split(',');
                //        }
                //        FontStyle oFontStyle = new FontStyle();
                //        oFontStyle = FontStyle.Regular;
                //        if (!string.IsNullOrEmpty(atFont[2]))
                //            oFontStyle |= FontStyle.Bold;

                //        if (!string.IsNullOrEmpty(atFont[3]))
                //            oFontStyle |= FontStyle.Italic;

                //        if (!string.IsNullOrEmpty(atFont[4]))
                //            oFontStyle |= FontStyle.Underline;

                //        oFont = new Font(atFont[0], Convert.ToSingle(atFont[1]), oFontStyle);
                //        aoC_Font.Add(oFont);
                //    }
                //}
                #endregion

                oSql.Clear();
                oSql.AppendLine("SELECT HD.FDShdSaleDate,ISNULL(USR.FTUsrName,HD.FTShdUsrClosed) AS FTUsrCode  ");
                oSql.AppendLine("FROM TPSTShiftHD HD WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN  TCNMUser_L USR WITH(NOLOCK) ON HD.FTShdUsrClosed = USR.FTUsrCode AND USR.FNLngID = " + nC_Language);
                oSql.AppendLine("WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTPosCode = '" + poSalePos.ptPosCode + "' AND HD.FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                oShiftHD = oDB.C_DAToExecuteQuery<cmlTPSTShiftHD>(tConnStr, oSql.ToString(), nCmdTime);
                if (oShiftHD == null) return false;

                //Bitmap oNewBitmap = new Bitmap(nWidth, 2000, PixelFormat.Format32bppArgb);
                Bitmap oNewBitmap = new Bitmap(nWidth, 5000, PixelFormat.Format64bppArgb);  //*Em 62-10-16

                oGraphic = Graphics.FromImage(oNewBitmap);
                oGraphic.FillRectangle(Brushes.White, new RectangleF(0, 0, nWidth, 2000));
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

                //*Net 63-07-17
                if (!String.IsNullOrEmpty(tC_PathLogo))
                {
                    if (File.Exists(tC_PathLogo))
                    {
                        oLogo = Image.FromFile(tC_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - 200) / 2, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }

                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg(tConnStr, poPos.FTSmgCode, "1", oGraphic, nWidth, nStartY, aoC_Font[0]);    // Header Slip Msg

                tPrint = oC_Resource.GetString("tBanknote").ToString();
                oGraphic.DrawString(tPrint, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                nStartY += 18;
                tPrint = oC_Resource.GetString("tCashier").ToString() + " : " + oShiftHD.FTUsrCode;
                oGraphic.DrawString(tPrint, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                nStartY += 18;
                tPrint = oC_Resource.GetString("tShiftCode").ToString() + " : " + poSalePos.ptXihDocNo;
                oGraphic.DrawString(tPrint, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                nStartY += 18;
                tPrint = oC_Resource.GetString("tTime").ToString() + " : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                oGraphic.DrawString(tPrint, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                nStartY += 10;
                oGraphic.DrawString(tLine, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                nStartY += 18;
                tPrint = oC_Resource.GetString("tType") + "  (" + oC_Resource.GetString("tQty") + ")";
                oGraphic.DrawString(tPrint, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth / 2, 18));
                tPrint = oC_Resource.GetString("tAmt");
                oGraphic.DrawString(tPrint, aoC_Font[2], Brushes.Black, new RectangleF(nWidth / 2, nStartY, (nWidth / 2) - 10, 18), oFormatFar);

                nStartY += 10;
                oGraphic.DrawString(tLine, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                aoShiftBN = C_GETaShiftBN(poSalePos, tConnStr, nCmdTime);
                if (aoShiftBN != null)
                {
                    foreach (cmlShiftBN oShift in aoShiftBN)
                    {
                        nStartY += 18;
                        tPrint = oShift.FTBntName + "  (" + oSP.SP_SETtDecShwSve(1, (decimal)(oShift.FNKbnQty), nC_DecShow) + ")";
                        oGraphic.DrawString(tPrint, aoC_Font[3], Brushes.Black, new RectangleF(0, nStartY, nWidth / 2, 15));
                        tPrint = oSP.SP_SETtDecShwSve(1, (decimal)(oShift.FCKbnAmt), nC_DecShow);
                        oGraphic.DrawString(tPrint, aoC_Font[4], Brushes.Black, new RectangleF(nWidth / 2, nStartY, (nWidth / 2) - 10, 18), oFormatFar);
                    }
                }
                nStartY += 10;
                oGraphic.DrawString(tLine, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                nStartY += 18;
                tPrint = oC_Resource.GetString("tSumCash");
                oGraphic.DrawString(tPrint, aoC_Font[5], Brushes.Black, new RectangleF(0, nStartY, nWidth / 2, 18));
                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(aoShiftBN.Sum(a => a.FCKbnAmt)), nC_DecShow);
                oGraphic.DrawString(tAmt, aoC_Font[6], Brushes.Black, new RectangleF(nWidth / 2, nStartY, (nWidth / 2) - 10, 18), oFormatFar);

                nStartY += 10;
                oGraphic.DrawString(tLine, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);

                nStartY += 30;
                oGraphic.DrawString(oC_Resource.GetString("tReceiver").ToString() + " (..............................)", aoC_Font[7], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 30;
                oGraphic.DrawString(oC_Resource.GetString("tSender").ToString() + " (..............................)", aoC_Font[7], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 18;
                nStartY = oMsg.C_GETnSlipMsg(tConnStr, poPos.FTSmgCode, "2", oGraphic, nWidth, nStartY, aoC_Font[8]); // Footer Slip Msg

                Rectangle oCropRect = new Rectangle(0, 0, 280, nStartY + 15);
                Bitmap oBitmapEJ = new Bitmap(280, nStartY + 15, PixelFormat.Format64bppArgb);
                using (Graphics oGrp = Graphics.FromImage(oBitmapEJ))
                {
                    oGrp.DrawImage(oNewBitmap, oCropRect, oCropRect, GraphicsUnit.Pixel);
                }
                tPathFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
                tPathFile = Directory.GetParent(Directory.GetParent(tPathFile).FullName).FullName;
                //tPathFile += @"\AdaEJ\Image\" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("yyMMdd"); //*Net 63-07-23 ปรับ Folder ที่เก็บ
                tPathFile += $@"\AdaEJ\{poSalePos.ptBchCode}\" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("yyMMdd");

                //if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "BNK", tPathFile, poSalePos.ptConnStr, (int)poShopDB.nCommandTimeOut))
                if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "BNK", tPathFile, ref ptErrMsg)) //*Net 63-07-15
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message;
                new cLog().C_WRTxLog("cEJ", "C_PRNxShiftBN : " + oEx.Message);
                return false;
            }
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
                oMsg = null;
                tLine = null;
                //oSP.SP_CLExMemory();
            }
        }




        private List<cmlShiftRcv> C_GETaShiftRcv(cmlRcvSalePos poSalePos, string ptConnStr, int pnCmdTime = 60)
        {
            List<cmlShiftRcv> aoShiftRcv = new List<cmlShiftRcv>();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT FNSdtSeqNo,ISNULL(RCVL.FTRcvName,(SELECT TOP 1 FTRcvName FROM TFNMRcv_L WITH(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode)) AS FTRcvName,FCRcvPayAmt ");
                oSql.AppendLine("FROM TPSTShiftSKeyRcv RCV WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + nC_Language);
                oSql.AppendLine("WHERE FTBchCode = '" + poSalePos.ptBchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + poSalePos.ptPosCode + "'");
                oSql.AppendLine("AND FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                aoShiftRcv = oDB.C_GETaDataQuery<cmlShiftRcv>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cEJ", "C_GETaShiftRcv : " + oEx.Message); }
            return aoShiftRcv;
        }
        //public bool C_GENbSlipRCV(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, cmlTCNMPos poPos) //*Net 63-07-14 เพิ่ม ptErrMsg
        public bool C_GENbSlipRCV(cmlRcvSalePos poSalePos, cmlShopDB poShopDB, cmlTCNMPos poPos, ref string ptErrMsg)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlShiftRcv> aoShiftRcv;
            Font oFont = new Font("CordiaUPC", 11.5f, FontStyle.Regular);
            cmlTPSTShiftHD oShiftHD = new cmlTPSTShiftHD();
            Image oLogo;
            string tAmt;
            string tLine,tMsg;
            string tPathFile;
            string tConnStr = poSalePos.ptConnStr;
            int nCmdTime = Convert.ToInt32(poShopDB.nCommandTimeOut);
            int nStartY = 0, nWidth;

            try
            {
                nWidth = 280;
                tLine = "........................................................................";
                tMsg = "";
                #region Comment
                //oSql.Clear();
                //oSql.AppendLine("SELECT FTSysSeq,FTSysStaDefValue,FTSysStaUsrValue");
                //oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTSysCode = 'PInvLayout'");
                //oSql.AppendLine("AND (CONVERT(INT,FTSysSeq) BETWEEN 1 AND 9)");
                //oSql.AppendLine("ORDER BY CONVERT(INT,FTSysSeq)");
                //DataTable odtTmp = oDB.C_DAToExecuteQuery(tConnStr, oSql.ToString(), nCmdTime);
                //if (odtTmp != null)
                //{
                //    aoC_Font = new List<Font>();
                //    foreach (DataRow oRow in odtTmp.Rows)
                //    {
                //        string[] atFont;
                //        if (string.IsNullOrEmpty(oRow.Field<string>("FTSysStaUsrValue")))
                //        {
                //            atFont = oRow.Field<string>("FTSysStaDefValue").Split(',');

                //        }
                //        else
                //        {
                //            atFont = oRow.Field<string>("FTSysStaDefValue").Split(',');
                //        }
                //        FontStyle oFontStyle = new FontStyle();
                //        oFontStyle = FontStyle.Regular;
                //        if (!string.IsNullOrEmpty(atFont[2]))
                //            oFontStyle |= FontStyle.Bold;

                //        if (!string.IsNullOrEmpty(atFont[3]))
                //            oFontStyle |= FontStyle.Italic;

                //        if (!string.IsNullOrEmpty(atFont[4]))
                //            oFontStyle |= FontStyle.Underline;

                //        oFont = new Font(atFont[0], Convert.ToSingle(atFont[1]), oFontStyle);
                //        aoC_Font.Add(oFont);
                //    }
                //}
                #endregion

                oSql.Clear();
                oSql.AppendLine("SELECT HD.FDShdSaleDate,ISNULL(USR.FTUsrName,HD.FTShdUsrClosed) AS FTUsrCode  ");
                oSql.AppendLine("FROM TPSTShiftHD HD WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN  TCNMUser_L USR WITH(NOLOCK) ON HD.FTShdUsrClosed = USR.FTUsrCode AND USR.FNLngID = " + nC_Language);
                oSql.AppendLine("WHERE HD.FTBchCode = '" + poSalePos.ptBchCode + "' AND HD.FTPosCode = '" + poSalePos.ptPosCode + "' AND HD.FTShfCode = '" + poSalePos.ptXihDocNo + "'");
                oShiftHD = oDB.C_DAToExecuteQuery<cmlTPSTShiftHD>(tConnStr, oSql.ToString(), nCmdTime);
                if (oShiftHD == null) return false;

                //Bitmap oNewBitmap = new Bitmap(nWidth, 2000, PixelFormat.Format32bppArgb);
                Bitmap oNewBitmap = new Bitmap(nWidth, 5000, PixelFormat.Format64bppArgb);  //*Em 62-10-16

                oGraphic = Graphics.FromImage(oNewBitmap);
                oGraphic.FillRectangle(Brushes.White, new RectangleF(0, 0, nWidth, 2000));
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

                if (!String.IsNullOrEmpty(tC_PathLogo))
                {
                    if (File.Exists(tC_PathLogo))
                    {
                        oLogo = Image.FromFile(tC_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - 200) / 2, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }
                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg(tConnStr, poPos.FTSmgCode, "1", oGraphic, nWidth, nStartY, aoC_Font[0]);    // Header Slip Msg

                // Get ค่า ID เครื่อง จาก DB
                oGraphic.DrawString("ID:" + poPos.FTPosRegNo.PadRight(30) + "USR: " + oShiftHD.FTUsrCode + " T: " + poSalePos.ptPosCode, aoC_Font[2], Brushes.Black, 0, nStartY);
                nStartY += 18;

                oGraphic.DrawString(oC_Resource.GetString("tShiftRcv").ToString(), aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 18;

                //*Net 63-07-17
                tMsg = oC_Resource.GetString("tTime").ToString() + " : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                oGraphic.DrawString(tMsg, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                //+++++++++++++++++++++

                nStartY += 10;
                oGraphic.DrawString(tLine, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                
                nStartY += 18;
                oGraphic.DrawString(" " + oC_Resource.GetString("tSeq"), aoC_Font[2], Brushes.Black, 0, nStartY);
                oGraphic.DrawString(" " + oC_Resource.GetString("tRcvType"), aoC_Font[2], Brushes.Black, 30, nStartY);
                oGraphic.DrawString(" " + oC_Resource.GetString("tAmt"), aoC_Font[2], Brushes.Black, new RectangleF(110, nStartY, nWidth - 110, 18), oFormatFar);

                aoShiftRcv = C_GETaShiftRcv(poSalePos, tConnStr, nCmdTime);
                if (aoShiftRcv != null)
                {
                    foreach (cmlShiftRcv oShift in aoShiftRcv)
                    {
                        nStartY += 18;
                        oGraphic.DrawString(" " + oShift.FNSdtSeqNo.ToString(), aoC_Font[2], Brushes.Black, 0, nStartY);
                        oGraphic.DrawString(" " + oShift.FTRcvName, aoC_Font[3], Brushes.Black, 30, nStartY);

                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oShift.FCRcvPayAmt), nC_DecShow);
                        oGraphic.DrawString(" " + tAmt, aoC_Font[4], Brushes.Black, new RectangleF(110, nStartY, nWidth - 110, 18), oFormatFar);
                    }
                }
                nStartY += 10;
                oGraphic.DrawString(tLine, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 18;
                oGraphic.DrawString(" " + oC_Resource.GetString("tCS_TotalAmt"), aoC_Font[5], Brushes.Black, 0, nStartY);
                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(aoShiftRcv.Sum(a => a.FCRcvPayAmt)), nC_DecShow);
                oGraphic.DrawString(" " + tAmt, aoC_Font[6], Brushes.Black, new RectangleF(110, nStartY, nWidth - 110, 18), oFormatFar);
                nStartY += 18;
                oGraphic.DrawString(tLine, aoC_Font[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15), oFormatCenter);
                nStartY += 30;
                oGraphic.DrawString(oC_Resource.GetString("tReceiver").ToString() + " (..............................)", aoC_Font[8], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 30;
                oGraphic.DrawString(oC_Resource.GetString("tSender").ToString() + " (..............................)", aoC_Font[8], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 18;
                nStartY = oMsg.C_GETnSlipMsg(tConnStr, poPos.FTSmgCode, "2", oGraphic, nWidth, nStartY, aoC_Font[8]); // Footer Slip Msg

                Rectangle oCropRect = new Rectangle(0, 0, 280, nStartY + 15);
                Bitmap oBitmapEJ = new Bitmap(280, nStartY + 15, PixelFormat.Format64bppArgb);
                using (Graphics oGrp = Graphics.FromImage(oBitmapEJ))
                {
                    oGrp.DrawImage(oNewBitmap, oCropRect, oCropRect, GraphicsUnit.Pixel);
                }
                tPathFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
                tPathFile = Directory.GetParent(Directory.GetParent(tPathFile).FullName).FullName;
                //tPathFile += @"\AdaEJ\Image\" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("yyMMdd"); //*Net 63-07-23 แรับ Folder ที่เก็บ
                tPathFile += $@"\AdaEJ\{poSalePos.ptBchCode}\" + Convert.ToDateTime(oShiftHD.FDShdSaleDate).ToString("yyMMdd");

                //if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "RCV", tPathFile, poSalePos.ptConnStr, (int)poShopDB.nCommandTimeOut))
                if (C_CRTbFileEJ(oBitmapEJ, poSalePos.ptBchCode, poSalePos.ptXihDocNo, "RCV", tPathFile, ref ptErrMsg)) //*Net 63-07-15
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message;
                new cLog().C_WRTxLog("cEJ", "C_PRNxShiftRCV : " + oEx.Message);
                return false;
            }
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
                oMsg = null;
                tLine = null;
                //oSP.SP_CLExMemory();
            }
        }




        //private bool C_CRTbFileEJ(Bitmap poBitmap, string ptBchCode, string ptDocNo, string ptDocType, string ptPath, string ptConnStr, int pnCmdTime = 60)
        private bool C_CRTbFileEJ(Bitmap poBitmap, string ptBchCode, string ptDocNo, string ptDocType, string ptPath, ref string ptErrMsg) //*Net 63-07-15
        {
            int nWidth = 0;
            int nHeight = 0;
            long nFileSize = 0;
            int nRowEff = 0;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                if (!Directory.Exists(ptPath)) Directory.CreateDirectory(ptPath);

                Bitmap oImageEJ = poBitmap;
                string tPathFile = ptPath + @"\{0}.EJ";
                string tPwd = "POSEJ";
                string tDocNo = ptDocNo;

                switch (ptDocType)
                {
                    case "DOC":
                        tPathFile = string.Format(tPathFile, ptDocNo);
                        break;
                    case "SUM":
                        tPathFile = string.Format(tPathFile, "SUM" + ptDocNo);
                        tDocNo = "SUM" + ptDocNo;
                        break;
                    case "BNK":
                        tPathFile = string.Format(tPathFile, "BNK" + ptDocNo);
                        tDocNo = "BNK" + ptDocNo;
                        break;
                    case "RCV":
                        tPathFile = string.Format(tPathFile, "RCV" + ptDocNo);
                        tDocNo = "RCV" + ptDocNo;
                        break;
                }

                try
                {
                    if (File.Exists(tPathFile)) File.Delete(tPathFile);
                    //oImageEJ.Save(tPathFile + ".jpg", ImageFormat.Jpeg);
                    oImageEJ.Save(tPathFile, ImageFormat.Gif);
                    C_FLExEnCipher(tPathFile, tPwd);
                }
                catch (Exception oEx)
                {
                    ptErrMsg = oEx.Message; //*Net 63-07-15
                    new cLog().C_WRTxLog("cEJ", "C_CRTxFileEJ : " + oEx.Message);
                    return false;
                }

                FileInfo oFile = new FileInfo(tPathFile);
                nHeight = oImageEJ.Height;
                nWidth = oImageEJ.Width;
                nFileSize = oFile.Length;

                switch (ptDocType)
                {
                    case "DOC":
                        oSql.AppendLine("DELETE FROM TPSTSlipEJ WITH(ROWLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "' AND FTXshDocNo = '" + ptDocNo + "'");
                        oSql.AppendLine("");
                        oSql.AppendLine("INSERT INTO TPSTSlipEJ(FTBchCode,FTXshDocNo,FTShpCode,FDXshDocDate,FTShfCode,FTUsrCode,FTJnlPicPath,FNJnlPrintCount,");
                        oSql.AppendLine("	FNJnlPicHeight,FNJnlPicWidth,FNJnlFileSize,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                        oSql.AppendLine("SELECT FTBchCode,FTXshDocNo,FTShpCode,FDXshDocDate,FTShfCode,FTUsrCode,");
                        oSql.AppendLine("'" + tPathFile + "' AS FTJnlPicPath,0 AS FNJnlPrintCount,");
                        oSql.AppendLine(nHeight + " AS FNJnlPicHeight," + nWidth + " AS FNJnlPicWidth," + nFileSize + " AS FNJnlFileSize,");
                        oSql.AppendLine("GETDATE() AS FDLastUpdOn, 'MQReceivePrc' AS FTLastUpdBy,");
                        oSql.AppendLine("GETDATE() AS FDCreateOn, 'MQReceivePrc' AS FTCreateBy");
                        oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "' AND FTXshDocNo = '" + ptDocNo + "'");
                        break;
                    case "SUM":
                    case "BNK":
                    case "RCV":
                        oSql.AppendLine("DELETE FROM TPSTSlipEJ WITH(ROWLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "' AND FTXshDocNo = '" + tDocNo + "'");
                        oSql.AppendLine("");
                        oSql.AppendLine("INSERT INTO TPSTSlipEJ(FTBchCode,FTXshDocNo,FTShpCode,FDXshDocDate,FTShfCode,FTUsrCode,FTJnlPicPath,FNJnlPrintCount,");
                        oSql.AppendLine("	FNJnlPicHeight,FNJnlPicWidth,FNJnlFileSize,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                        oSql.AppendLine("SELECT FTBchCode,'" + tDocNo + "' AS FTXshDocNo,'' AS FTShpCode,FDShdSaleDate,FTShfCode,FTUsrCode,");
                        oSql.AppendLine("'" + tPathFile + "' AS FTJnlPicPath,0 AS FNJnlPrintCount,");
                        oSql.AppendLine(nHeight + " AS FNJnlPicHeight," + nWidth + " AS FNJnlPicWidth," + nFileSize + " AS FNJnlFileSize,");
                        oSql.AppendLine("GETDATE() AS FDLastUpdOn, 'MQReceivePrc' AS FTLastUpdBy,");
                        oSql.AppendLine("GETDATE() AS FDCreateOn, 'MQReceivePrc' AS FTCreateBy");
                        oSql.AppendLine("FROM TPSTShiftHD WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "' AND FTShfCode = '" + ptDocNo + "'");
                        break;
                }

                if (oSql.Length > 0)
                {
                    //if (oDB.C_DATbExecuteNonQuery(ptConnStr, oSql.ToString(), pnCmdTime, out nRowEff))
                    if (oDB.C_DATbExecuteNonQuery(tC_tConnStr, oSql.ToString(), nC_CmdTimeout, out nRowEff)) //*Net 63-07-15
                    {
                        return true;
                    }
                    else
                    {
                        ptErrMsg = $"Execute TPSTSlipEJ BchCode{ptBchCode} DocNo{ptDocNo}"; //*Net 63-07-15
                        return false;
                    }
                }
                else
                {
                    ptErrMsg = "DocType not Found"; //*Net 63-07-15
                    return false;
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = "DocType not Found"; //*Net 63-07-15
                new cLog().C_WRTxLog("cEJ", "C_CRTxFileEJ : " + oEx.Message);
                return false;
            }
        }


        #endregion End Function

        #region Encript/Decript
        public void C_FLExEnCipher(string ptFile, string ptPwd)
        {
            // ----------------------------------------------------------------
            // Cmt:
            // 
            // ----------------------------------------------------------------
            string tFileStr, tFileHD;

            try
            {
                FileSystem.FileOpen(1, ptFile, OpenMode.Binary);
                tFileStr = Strings.Space(Convert.ToInt32(FileSystem.LOF(1)));
                FileSystem.FileGet(1, ref tFileStr);
                tFileHD = "[Secret]" + C_FLEtCipherHash(ref ptPwd) + Constants.vbCrLf;
                C_FLExCipherProcess(ref tFileStr, ptPwd);
                FileSystem.FilePut(1, tFileHD, 1);
                FileSystem.FilePut(1, tFileStr);
                FileSystem.FileClose(1);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cEJ", "C_FLExEnCipher : " + oEx.Message);
            }
        }

        private string C_FLEtCipherHash(ref string ptFileStr)
        {
            // ---------------------------------------------------------------------------------
            // Cmt:
            // 
            // ---------------------------------------------------------------------------------
            int nCount, nRet = 0;
            string tFileHD;

            try
            {
                foreach (char oChr in ptFileStr)
                {
                    nRet = nRet + (int)oChr;
                    nRet = (nRet * 1717 + 1717) % 1048576;
                }

                for (nCount = 1; nCount <= 7; nCount++)
                    nRet = (nRet * 997 + 997) % 1048576;

                tFileHD = int.Parse(nRet.ToString(), System.Globalization.NumberStyles.HexNumber).ToString("0000");
                foreach (char oChr in ptFileStr)
                {
                    nRet = nRet + (int)oChr;
                    nRet = (nRet * 997 + 997) % 1048576;
                }

                for (nCount = 1; nCount <= 7; nCount++)
                    nRet = (nRet * 1717 + 1717) % 1048576;

                return tFileHD + int.Parse(nRet.ToString(), System.Globalization.NumberStyles.HexNumber).ToString("0000");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cEJ", "C_FLEtCipherHash : " + oEx.Message);
                return "";
            }

        }

        private void C_FLExCipherProcess(ref string ptFileStr, string ptPwd)
        {
            // ----------------------------------------------------------------
            // Cmt:
            // 
            // ----------------------------------------------------------------
            long vFstValue = 0;
            int nRet = 0;
            long vSndValue = 0;
            int nI;
            try
            {
                foreach (char oChr in ptPwd)
                {
                    nRet = nRet + (int)oChr;
                    nRet = (nRet * 367 + 331) % 0xFFF;
                    vFstValue = ((vFstValue + nRet) * 743 + 599) % 0xFFF;
                    vSndValue = ((vSndValue + vFstValue) * 563 + 787) % 0xFFF;
                }
                C_FLExCipher(ref ptFileStr, nRet, vFstValue, vSndValue);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cEJ", "C_FLExCipherProcess : " + oEx.Message);
            }
        }

        private void C_FLExCipher(ref string ptFileStr, object pnRet = null, object pvFstValue = null, object pvSndValue = null)
        {

            //---------------------------------------------------------------------------------
            //   Cmt:
            //
            //---------------------------------------------------------------------------------
            int nRValue = 0;
            int nMValue = 0;
            int nNValue = 0;
            const int nBigNum = 32768;
            int nCValue, nCount, nDValue;

            try
            {
                if (pnRet != null)
                    nRValue = Convert.ToInt32(pnRet);
                if (Information.IsNothing(pvFstValue))
                {
                    if (nMValue == 0)
                        nMValue = 69;
                }
                else
                    nMValue = (Convert.ToInt32(pvFstValue) * 4 + 1) % nBigNum;
                if (Information.IsNothing(pvSndValue))
                {
                    if (nNValue == 0)
                        nNValue = 47;
                }
                else
                    nNValue = (Convert.ToInt32(pvSndValue) * 2 + 1) % nBigNum;

                foreach (char oChr in ptFileStr)
                {
                    nCValue = (int)oChr;
                    switch (nCValue)
                    {
                        case object _ when 48 <= nCValue && nCValue <= 57:
                            {
                                nDValue = nCValue - 48;
                                break;
                            }

                        case object _ when 63 <= nCValue && nCValue <= 90:
                            {
                                nDValue = nCValue - 53;
                                break;
                            }

                        case object _ when 97 <= nCValue && nCValue <= 122:
                            {
                                nDValue = nCValue - 59;
                                break;
                            }

                        default:
                            {
                                nDValue = -1;
                                break;
                            }
                    }

                    if (nDValue >= 0)
                    {
                        nRValue = (nRValue * nMValue + nNValue) % nBigNum;
                        nDValue = (nRValue & 63) ^ nDValue;
                        switch (nDValue)
                        {
                            case object _ when 0 <= nDValue && nDValue <= 9:
                                {
                                    nCValue = nDValue + 48;
                                    break;
                                }

                            case object _ when 10 <= nDValue && nDValue <= 37:
                                {
                                    nCValue = nDValue + 53;
                                    break;
                                }

                            case object _ when 38 <= nDValue && nDValue <= 63:
                                {
                                    nCValue = nDValue + 59;
                                    break;
                                }
                        };
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cEJ", "C_FLExCipherProcess : " + oEx.Message);
            }

        }
        #endregion End Encript/Decript


    }
}
