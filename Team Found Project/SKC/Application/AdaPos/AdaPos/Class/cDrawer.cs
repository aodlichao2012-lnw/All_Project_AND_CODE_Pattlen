using AdaPos.Models.Database;
using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace AdaPos.Class
{
    public class cDrawer
    {
        public cDrawer()
        { }

        /// <summary>
        /// Open Cash Drawer
        /// </summary>
        public void C_OPNxCashDrawer()
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            IntPtr nUnmanagedBytes;
            byte[] anCodeOpenCashDrawer;

            try
            {
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();
                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;

                anCodeOpenCashDrawer = new byte[] { 27, 112, 48, 55, 121 };
                nUnmanagedBytes = new IntPtr(0);
                nUnmanagedBytes = Marshal.AllocCoTaskMem(5);
                Marshal.Copy(anCodeOpenCashDrawer, 0, nUnmanagedBytes, 5);
                cRawPrinterHelper.SendBytesToPrinter(oSettings.PrinterName, nUnmanagedBytes, 5);
                Marshal.FreeCoTaskMem(nUnmanagedBytes);

                C_SAVxDrawer();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDrawer", "C_OPNxCashDrawer : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                anCodeOpenCashDrawer = null;
                oDoc = null;
                oSettings = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Save drawer
        /// </summary>
        private void C_SAVxDrawer()
        {
            cmlTPSTShiftEvent oEvent;
            cShiftEvent oShfEvn;

            try
            {
                oShfEvn = new cShiftEvent();

                oEvent = new cmlTPSTShiftEvent();
                oEvent.FCSvnAmt = 0;
                oEvent.FDHisDateTime = DateTime.Now;
                oEvent.FNSdtSeqNo = cVB.nVB_ShfSeq;
                oEvent.FNSvnQty = 1;
                oEvent.FTBchCode = cVB.tVB_BchCode;
                oEvent.FTEvnCode = "004";
                oEvent.FTShfCode = cVB.tVB_ShfCode;
                oEvent.FTSvnApvCode = cVB.tVB_UsrCode;
                oEvent.FTRsnCode = cVB.oVB_Reason.FTRsnCode;
                oEvent.FTPosCode = cVB.tVB_PosCode; //*Em 62-01-03  เพิ่ม FTPosCode

                oShfEvn.C_INSxShiftEvent(oEvent);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDrawer", "C_SAVxDrawer : " + oEx.Message); }
        }
    }
}
