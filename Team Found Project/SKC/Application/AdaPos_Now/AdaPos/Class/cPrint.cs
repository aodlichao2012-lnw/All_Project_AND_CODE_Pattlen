using AdaPos.Models.Database;
using AdaPos.Models.Other;
using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public static class cPrint
    {
        public static void C_PRNxBarcode(Graphics poGraphic, string ptStrBar, int pnWidht, int pnY)
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
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrint", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
        }

        public static void C_PRNxPickinglist(string ptDocNo, bool pbIsTemp = true)
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            try
            {
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();

                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                oDoc.PrintController = new StandardPrintController();
                oDoc.PrintPage += (sender, e) =>
                {
                    C_GETxPrnPicklist(ref e, ptDocNo);
                };

                oDoc.Print();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrint", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                //new cSP().SP_CLExMemory();
            }
        }
        public static void C_GETxPrnPicklist(ref PrintPageEventArgs e, string ptDocNo, bool pbIsTemp = true)
        {
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            cDatabase oDB;
            StringBuilder oSql;
            Image oLogo;

            List<cmlTPSTSalDT> aoDT;

            string tLine;
            string tTblSalDT = "TSDT" + cVB.tVB_PosCode;
            int nStartY = 0, nWidth;
            try
            {
                #region SetupPrint

                oDB = new cDatabase();
                oSql = new StringBuilder();
                aoDT = new List<cmlTPSTSalDT>();
                oMsg = new cSlipMsg();
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

                nWidth = Convert.ToInt32(e.Graphics.VisibleClipBounds.Width);
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
                if (pbIsTemp == false)
                {
                    tTblSalDT = "TPSTSalDT";
                }


                //*Em 62-10-29
                if (!string.IsNullOrEmpty(cVB.tVB_PathLogo))
                {
                    if (File.Exists(cVB.tVB_PathLogo))
                    {
                        oLogo = Image.FromFile(cVB.tVB_PathLogo);
                        if (oLogo.Width < 200)
                        {
                            e.Graphics.DrawImage(oLogo, new Rectangle((nWidth - oLogo.Width) / 2, nStartY, oLogo.Width, oLogo.Height));
                            nStartY += oLogo.Height;
                        }
                        else
                        {
                            e.Graphics.DrawImage(oLogo, new Rectangle((nWidth - 200) / 2, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }
                //+++++++++++++++++

                #endregion

                #region PrintHeader

                //nStartY = oMsg.C_GETnSlipMsg("1", e.Graphics, nWidth, nStartY);   //*Arm 63-07-29 Header Slip Msg
                e.Graphics.DrawString(cVB.oVB_GBResource.GetString("tPickinglist"), cVB.aoVB_PInvLayout[6], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                nStartY += 18;
                // Get ค่า ID เครื่อง จาก DB
                e.Graphics.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) , cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                e.Graphics.DrawString("USR: " + cVB.tVB_UsrCode.PadRight(10) + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatFar);
                nStartY += 18;
                e.Graphics.DrawString(Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm"), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                e.Graphics.DrawString(" BNO:" + ptDocNo, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatFar);


                #endregion

                #region PrintDT
                oSql.Clear();
                oSql.AppendLine("SELECT DT.FTPdtCode, ");
                oSql.AppendLine("DT.FTXsdPdtName, ");
                oSql.AppendLine("DT.FTXsdBarCode, ");
                oSql.AppendLine("SUM(DT.FCXsdQty) AS FCXsdQty,");
                oSql.AppendLine("DT.FTXsdRmk");
                oSql.AppendLine($"FROM {tTblSalDT} DT WITH(NOLOCK)");
                oSql.AppendLine($"WHERE DT.FTBchCode = '{cVB.tVB_BchCode}'");
                oSql.AppendLine($"AND DT.FTXshDocNo = '{ptDocNo}' ");
                oSql.AppendLine("AND DT.FTXsdStaPdt != '4'"); //*Arm 63-09-18
                oSql.AppendLine("GROUP BY DT.FTBchCode,DT.FTXshDocNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FTXsdRmk");

                aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());
                if (aoDT != null)
                {
                    foreach (cmlTPSTSalDT oDT in aoDT)
                    {
                        //nStartY += 18;
                        //e.Graphics.DrawString(oDT.FTPdtCode.PadRight(15) + "    " + oDT.FTXsdRmk, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        //e.Graphics.DrawString(oDT.FCXsdQty.Value.ToString("N0"), cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatFar);

                        //nStartY += 18;
                        //e.Graphics.DrawString($"  {oDT.FTXsdPdtName}", cVB.aoVB_PInvLayout[1], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));

                        //*Arm 63-07-24
                        string tRow1 = "";
                        string tRow2 = "";

                        //nStartY += 18;
                        nStartY += 30; //*Arm 63-09-18
                        tRow1 = C_GETxPdtLeft(oDT.FTPdtCode, 15) + "    " + C_GETxPdtLeft(oDT.FTXsdRmk, 15);
                        e.Graphics.DrawString(tRow1, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        e.Graphics.DrawString(oDT.FCXsdQty.Value.ToString("N0"), cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatFar);

                        nStartY += 18;
                        tRow2 = " " + C_GETxPdtLeft(oDT.FTXsdPdtName, 10);
                        e.Graphics.DrawString(tRow2, cVB.aoVB_PInvLayout[1], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));
                        //++++++++++++
                    }
                }
                #endregion

                #region PrintFooter
                nStartY += 30;
                //nStartY = oMsg.C_GETnSlipMsg("2", e.Graphics, nWidth, nStartY); // Footer Slip Msg
                //nStartY += 18;
                C_PRNxBarcode(e.Graphics, ptDocNo, nWidth, nStartY);
                #endregion

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrint", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();

                oDB = null;
                oSql = null;
                oLogo = null;
                oFormatFar = null;
                oFormatCenter = null;
                oMsg = null;
                tLine = null;
                //new cSP().SP_CLExMemory();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptPdtName"></param>
        /// <param name="pnPdtWidth"></param>
        /// <returns></returns>
        public static string C_GETxPdtLeft(string ptPdtName, int pnPdtWidth)
        {
            int nCountSara;
            string tResult = "";
            try
            {
                nCountSara = C_PRCnThaAbsoluteCount(ptPdtName);

                if (ptPdtName.Length < (pnPdtWidth + nCountSara))
                {
                    for (int ni = ptPdtName.Length; ni <= (pnPdtWidth + nCountSara); ni++)
                    {
                        ptPdtName += " ";
                    }
                }

                ptPdtName = ptPdtName.Substring(0, (pnPdtWidth + nCountSara));

                nCountSara = C_PRCnThaAbsoluteCount(ptPdtName);
                while (ptPdtName.Length > (pnPdtWidth + nCountSara))
                {
                    ptPdtName = ptPdtName.Substring(0, ptPdtName.Length - 1);
                    nCountSara = C_PRCnThaAbsoluteCount(ptPdtName);
                }

                tResult = ptPdtName;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrint","C_GETxPdtLeft : " + oEx.Message);
            }
            return tResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptPrint"></param>
        /// <returns></returns>
        public static int C_PRCnThaAbsoluteCount(string ptPrint)
        {
            int ni, nByte = 0, nBytep = 0, nP = 0;
            int nT1 = 0, nT2 = 0, nT3 = 0;
            bool bJust = false;
            bool bTFill = false;
            bool bBFill = false;
            string tText = ptPrint;
            string tT1 = "", tT2 = "", tT3 = "", tBlank = "";
            int nResult = 0;

            try
            {
                // เช็คก่อนว่าต้องตัดคำหรือไม่
                for (ni = 0; ni < tText.Length; ni++)
                {
                    var tSub = tText.Substring(ni, 1);
                    byte[] abyte = Encoding.Default.GetBytes(tSub);
                    foreach (var b in abyte)
                    {
                        nByte = Convert.ToInt32(b);
                    }

                    switch (nByte)
                    {
                        case 209:   // ั
                        case 212:   // ิ
                        case 213:   // ี
                        case 214:   // ึ
                        case 215:   // ื
                        case 216:   // ุ
                        case 217:   // ู
                        case 218:   // .
                        case 231:   // ็
                        case 232:   // ่
                        case 233:   // ้
                        case 234:   // ๊
                        case 235:   // ๋
                        case 236:   // ์
                        case 237:   // ํ
                            bJust = true;
                            break;
                        default:
                            break;
                    }

                    if (bJust)
                    {
                        break;
                    }
                }

                //ต้องตัดคำ
                if (bJust)
                {
                    for (ni = 0; ni < tText.Length; ni++)
                    {
                        var tSub = tText.Substring(ni, 1);
                        byte[] abyte = Encoding.Default.GetBytes(tSub);
                        foreach (var b in abyte)
                        {
                            nByte = Convert.ToInt32(b);
                        }
                        switch (nByte)
                        {
                            case 209:   // ั
                            case 212:   // ิ
                            case 213:   // ี
                            case 214:   // ึ
                            case 215:   // ื
                                tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                nT1 = nT1 + 1;
                                bTFill = true;
                                nP = 1;
                                break;

                            case 216:   // ุ
                            case 217:   // ู
                                tT3 = tT3 + Encoding.Default.GetString(abyte).ToString();
                                nT3 = nT3 + 1;
                                bTFill = true;
                                nP = 5;
                                break;

                            case 231:   // ็
                                tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                nT1 = nT1 + 1;
                                bTFill = true;
                                nP = 2;
                                break;

                            case 236:   // ์
                                if (nP == 1 && nBytep == 212)
                                {
                                    // ถ้าก่อนหน้านี้เป็นสระอิ ต้องลบสระอิก่อนหน้านี้ก่อน แล้วเปลี่ยนเป็น  ->  ิ์
                                    tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                    nT1 = nT1 + 1; //*Arm 63-08-05
                                }
                                else
                                {
                                    tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                    nT1 = nT1 + 1;
                                    bTFill = true;
                                    nP = 3;
                                }
                                break;

                            case 232:   // ่
                            case 233:   // ้
                            case 234:   // ๊
                            case 235:   // ๋
                                if (nP == 1)
                                {
                                    // ถ้าก่อนหน้านี้เป็น ชุดที่ 1 ต้องลบตัวก่อนหน้านี้ก่อนเปลี่ยนให้เป็น -> สระติดกัน แยกตาม case ตัวเดิม
                                    tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                    nT1 = nT1 + 1; //*Arm 63-08-05
                                }
                                else
                                {
                                    tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                    nT1 = nT1 + 1;
                                    bTFill = true;
                                    nP = 4;
                                }
                                break;

                            default:
                                tT2 = tT2 + Encoding.Default.GetString(abyte).ToString();
                                switch (nP)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                    case 4:
                                    case 5:
                                        if (bTFill == false)
                                        {
                                            tT1 = tT1 + tBlank;
                                        }
                                        if ((bBFill == false))
                                        {
                                            tT3 = tT3 + tBlank;
                                        }
                                        break;

                                    case 6:
                                        tT1 = tT1 + tBlank;
                                        tT3 = tT3 + tBlank;
                                        break;
                                }
                                bBFill = false;
                                bTFill = false;
                                nP = 6;
                                break;
                        }

                        nBytep = nByte; //เก็บค่าเก่า
                    }
                    nResult = nT1 + nT3;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPrint", "C_PRCnThaAbsoluteCount : " + oEx.Message.ToString());
            }
            return nResult;
        }

    }
}
