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
    public class cPrint
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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

                //nStartY = oMsg.C_GETnSlipMsg("1", e.Graphics, nWidth, nStartY);    // Header Slip Msg
                e.Graphics.DrawString(cVB.oVB_GBResource.GetString("tPickinglist"), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
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
                oSql.AppendLine("GROUP BY DT.FTBchCode,DT.FTXshDocNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FTXsdRmk");

                aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());
                if (aoDT != null)
                {
                    foreach (cmlTPSTSalDT oDT in aoDT)
                    {
                        nStartY += 18;
                        e.Graphics.DrawString(oDT.FTPdtCode.PadRight(15) + "    " + oDT.FTXsdRmk, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        e.Graphics.DrawString(oDT.FCXsdQty.Value.ToString("N0"), cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatFar);

                        nStartY += 18;
                        e.Graphics.DrawString($"  {oDT.FTXsdPdtName}", cVB.aoVB_PInvLayout[1], Brushes.Black, new RectangleF(0, nStartY, nWidth - 70, 18));
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
                new cSP().SP_CLExMemory();
            }

        }
    }
}
