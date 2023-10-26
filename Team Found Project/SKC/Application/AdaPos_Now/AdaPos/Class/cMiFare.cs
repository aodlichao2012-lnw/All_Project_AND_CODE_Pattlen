using AdaPos.Models.Other;
using ReaderB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cMiFare
    {
        private byte nW_ReaderAddr = 0xFF;
        private int nW_FrmHandle = -1;
        private byte[] anW_SNR = new byte[4];

        public cMiFare()
        {

        }

        /// <summary>
        /// Open Com Port
        /// </summary>
        public bool C_OPNbComPort()
        {
            string tComPort;
            int nCmdRet = 0x30, nPort;
            bool bOpenComPort = false;

            try
            {
                tComPort = (from PosHW in cVB.aoVB_PosHW where PosHW.FTShwCode == "006" select PosHW.FTPhwConnRef).FirstOrDefault();

                nPort = Convert.ToInt32(tComPort.Substring(3));
                nW_ReaderAddr = Convert.ToByte("FF", 16);
                nCmdRet = StaticClassReaderB.OpenComPort(nPort, ref nW_ReaderAddr, ref nW_FrmHandle);

                if (nCmdRet == 0)
                    bOpenComPort = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMiFare", "C_OPNbComPort : " + oEx.Message); }
            finally
            {
                tComPort = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return bOpenComPort;
        }

        /// <summary>
        /// Close Com Port
        /// </summary>
        public void C_CLOxComPort()
        {
            try
            {
                StaticClassReaderB.CloseComPort();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMiFare", "C_CLOxComPort : " + oEx.Message); }
        }

        /// <summary>
        /// Get Read RFID
        /// </summary>
        public cmlWristband C_GEToReadSID()
        {
            byte[] anData = new byte[2];
            byte nErrCode = 0, nReserved = 0, nSize = 0;
            byte nMode = 1;
            int nCmdRet = 0x30;
            cmlWristband oWb = new cmlWristband();

            try
            {
                nCmdRet = StaticClassReaderB.ChangeTo14443A(ref nW_ReaderAddr, nW_FrmHandle);

                if (nCmdRet == 0)
                {
                    // Card Type
                    nCmdRet = StaticClassReaderB.ISO14443ARequest(ref nW_ReaderAddr, nMode, anData, ref nErrCode, nW_FrmHandle);

                    if (nCmdRet == 0)
                    {
                        // UID
                        nCmdRet = StaticClassReaderB.ISO14443AAnticoll(ref nW_ReaderAddr, nReserved, anW_SNR, ref nErrCode, nW_FrmHandle);

                        if (nCmdRet == 0)
                        {
                            oWb.tUID = C_SETtByteArrayToHexString(anW_SNR).Replace(" ", "");

                            // Card Capacity
                            nCmdRet = StaticClassReaderB.ISO14443ASelect(ref nW_ReaderAddr, anW_SNR, ref nSize, ref nErrCode, nW_FrmHandle);

                            if (nCmdRet == 0)
                            {
                                oWb.dDateCreate = C_GEToValueBlock<DateTime>(1, 0).SP_COVdStringToDate();                       // วันที่เปิดบัตร
                                oWb.tTimeCreate = C_GEToValueBlock<string>(1, 1);                                               // เวลาเปิดบัตร
                                oWb.cDeposit = Convert.ToDecimal(C_GEToValueBlock<decimal>(2, 0));                                // ยอดมัดจำ wristband
                                oWb.cDepositItem = Convert.ToDecimal(C_GEToValueBlock<decimal>(2, 1));                            // ยอดมัดจำสินค้า
                                oWb.cAvailable = Convert.ToDecimal(C_GEToValueBlock<decimal>(2, 2));                              // ยอดใช้ได้
                                oWb.nTxnOffline = Convert.ToInt32(C_GEToValueBlock<int>(3, 0));                                 // จำนวนครั้งที่ทำรายการ offline
                                oWb.dDateUpdate = C_GEToValueBlock<DateTime>(4, 0).SP_COVdStringToDate();                       // วันที่อัพเดทข้อมูล
                                oWb.tTimeUpdate = C_GEToValueBlock<string>(4, 1);                                               // เวลาอัพเดทข้อมูล
                                oWb.dDateStart = C_GEToValueBlock<DateTime>(5, 0).SP_COVdStringToDate();                        // วันที่เช่า จาก
                                oWb.tTimeStart = C_GEToValueBlock<string>(5, 1);                                                // เวลาเช่า จาก
                                oWb.dDateFinish = C_GEToValueBlock<DateTime>(6, 0).SP_COVdStringToDate();                       // วันที่เช่า ถึง *ชั่วคราวไม่มีการบันทึก
                                oWb.tTimeFinish = C_GEToValueBlock<string>(6, 1);                                               // เวลาเช่า ถึง *ชั่วคราวไม่มีการบันทึก
                            }
                            else
                                oWb.tMsgError = C_MSGtFunctionReturn(nCmdRet, nErrCode);
                        }
                        else
                            oWb.tMsgError = C_MSGtFunctionReturn(nCmdRet, nErrCode);

                        C_SETxBeep();
                    }
                    else
                    {
                        oWb.tMsgError = C_MSGtFunctionReturn(nCmdRet, nErrCode);

                        if (nCmdRet == 0x05) nCmdRet = StaticClassReaderB.OpenRf(ref nW_ReaderAddr, nW_FrmHandle);
                    }
                }
                else
                    oWb.tMsgError = C_MSGtFunctionReturn(nCmdRet, nErrCode);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMiFare", "C_GEToReadSID : " + oEx.Message); }

            return oWb;
        }

        /// <summary>
        /// Get value from block
        /// </summary>
        /// <returns></returns>
        private string C_GEToValueBlock<T>(int pnSector, int pnBlock)
        {
            T oValue = default(T);
            string tValue = "";
            string tTemp = "FFFFFFFFFFFF";  // fix
            byte[] anKey = new byte[6];
            byte[] anDataRead = new byte[16];
            byte nKeyStyle = 0, nBlock, nErrCode = 0, nSector;
            int nCmdRet = 0x30, nCheckZero;

            try
            {
                nSector = (byte)pnSector;   // fix
                anKey = C_SETnHexStringToByteArray(tTemp);
                nCmdRet = StaticClassReaderB.ISO14443AAuthKey(ref nW_ReaderAddr, nKeyStyle, nSector, anKey, ref nErrCode, nW_FrmHandle);

                if (nCmdRet == 0)
                {
                    nBlock = (byte)(pnSector * 4 + pnBlock);//fix
                    nCmdRet = StaticClassReaderB.ISO14443ARead(ref nW_ReaderAddr, nBlock, anDataRead, ref nErrCode, nW_FrmHandle);

                    if (nCmdRet == 0)
                    {
                        tTemp = C_SETtByteArrayToHexString(anDataRead).Replace(" ", "");
                        tTemp = C_SETtHexStringToString(tTemp);
                        tValue = tTemp;
                        nCheckZero = tValue.IndexOf('\0');

                        if (nCheckZero >= 0) tValue = tValue.Substring(0, nCheckZero);

                        switch (Type.GetTypeCode(oValue.GetType()))
                        {
                            case TypeCode.Int32:
                            case TypeCode.Double:
                                if (string.IsNullOrEmpty(tValue))
                                    tValue = "0";
                                break;

                            case TypeCode.DateTime:
                                if (string.IsNullOrEmpty(tValue))
                                    tValue = null;
                                break;
                        }
                    }
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMiFare", "C_GEToValueBlock : " + oEx.Message); }
            finally
            {
                tTemp = null;
                anKey = null;
                anDataRead = null;
            }

            return tValue;
        }

        /// <summary>
        /// Convert Btye Array to Hex String
        /// </summary>
        /// <param name="panData"></param>
        /// <returns></returns>
        private string C_SETtByteArrayToHexString(byte[] panData)
        {
            StringBuilder oStr = null;
            string tByteArray = "";

            try
            {
                oStr = new StringBuilder(panData.Length * 3);

                foreach (byte nData in panData)
                    oStr.Append(Convert.ToString(nData, 16).PadLeft(2, '0').PadRight(3, ' '));

                tByteArray = oStr.ToString().ToUpper();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMiFare", "C_SETtByteArrayToHexString : " + oEx.Message); }
            finally
            {
                panData = null;
                oStr = null;
            }

            return tByteArray;
        }

        /// <summary>
        /// Convert Hex String to Byte Array
        /// </summary>
        /// <param name="ptStr"></param>
        /// <returns></returns>
        private byte[] C_SETnHexStringToByteArray(string ptStr)
        {
            byte[] anBuffer = null;

            try
            {
                ptStr = ptStr.Replace(" ", "");
                anBuffer = new byte[ptStr.Length / 2];

                for (int nCount = 0; nCount < ptStr.Length; nCount += 2)
                    anBuffer[nCount / 2] = (byte)Convert.ToByte(ptStr.Substring(nCount, 2), 16);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMiFare", "C_SETnHexStringToByteArray : " + oEx.Message); }
            finally
            {
                ptStr = null;
            }

            return anBuffer;
        }

        /// <summary>
        /// Convert Hex String to String
        /// </summary>
        /// <param name="ptHex"></param>
        /// <returns></returns>
        private string C_SETtHexStringToString(string ptHex)
        {
            string tStr = "", tHexChar;
            int nHexValue;

            try
            {
                for (int nCount = 0; nCount < ptHex.Length / 2; nCount++)
                {
                    tHexChar = ptHex.Substring(nCount * 2, 2);
                    nHexValue = Convert.ToInt32(tHexChar, 16);
                    tStr += Char.ConvertFromUtf32(nHexValue);
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMiFare", "C_SETtHexStringToString : " + oEx.Message); }
            finally
            {
                ptHex = null;
                tHexChar = null;
            }

            return tStr;
        }

        /// <summary>
        /// Function Msg Error
        /// </summary>
        /// <param name="pnCmdRet"></param>
        /// <param name="pnErrCode"></param>
        /// <returns></returns>
        private string C_MSGtFunctionReturn(int pnCmdRet, byte pnErrCode)
        {
            string tMsg = "";

            switch (pnCmdRet)
            {
                case 0x01: tMsg = "Operation number error"; break;
                case 0x02: tMsg = "Command not support"; break;
                case 0x03: tMsg = "Operation number range number error"; break;
                case 0x04: tMsg = "Command can not execute at current"; break;
                case 0x05: tMsg = "RF field inactive"; break;
                case 0x06: tMsg = "EEPROM error"; break;
                case 0x0A: tMsg = "Have not collected one tag’s UID before user-defined Inventory - ScanTime overflows"; break;
                case 0x0B: tMsg = "Have not collected all tag’s UIDs before user-defined InventoryScanTime overflows or 16 tag’s UID has been collected but there are still other tag in the field to collect their UIDs."; break;
                case 0x0E: tMsg = "No active tag in the RF field"; break;
                case 0x0F: tMsg = "ISO156963 operation error."; break;
                case 0x10: tMsg = "ISO14443A operation error."; break;
                case 0x1B: tMsg = "ISO14443B operation error."; break;
                case 0x1F: tMsg = "Protocol mode error.For example: Running ISO14443A command under ISO15693 mode."; break;
                case 0x30: tMsg = "Communication error"; break;
                case 0x31: tMsg = "Return data’s CRC check error"; break;
                case 0x32: tMsg = "Length of return data error"; break;
                case 0x33: tMsg = "Communication busy"; break;
                case 0x35: tMsg = "Port opened"; break;
                case 0x36: tMsg = "Port closed"; break;
                case 0x37: tMsg = "Invalid handle"; break;
                case 0x38: tMsg = "Invalid port"; break;
                case 0x0C: tMsg = "No change done to backfiles,CommitTransaction / AbortTransaction not necessary."; break;
                //case 0x0E:   tMsg = "Insufficient NX-Memory to complete command"); break;
                case 0x1C: tMsg = "Command code not supported"; break;
                case 0x1E: tMsg = "CRC or MAC dose not match data Padding bytes not valid"; break;
                case 0x40: tMsg = "Invalid key number specified"; break;
                case 0x7E: tMsg = "Length of command string invalid"; break;
                case 0x9D: tMsg = "Current configuration/ status dose not allow the requested command"; break;
                case 0x9E: tMsg = "Value of the parameter(s) invalid"; break;
                case 0xA0: tMsg = "Requested AID not present on PICC"; break;
                case 0xA1: tMsg = "Unrecoverable error within application, application will be disabled"; break;
                case 0xAE: tMsg = "Current authentication status dose not allow the requested command"; break;
                case 0xAF: tMsg = "Additional data frame is expected to be sent"; break;
                case 0xBE: tMsg = "Attempt to read / write data from / to beyond the file’s record’s limit. Attempt to exceed the limits of a value file."; break;
                case 0xC1: tMsg = "Unrecoverable error within PICC, PICC will be disabled"; break;
                case 0xCD: tMsg = "PICC was disabled by an unrecoverable error."; break;
                case 0xCE: tMsg = "Number of Application limited to 28, no additional CreateApplication possible"; break;
                case 0xDE: tMsg = "Creation of file / application failed because file / application with same number already exists"; break;
                case 0xEE: tMsg = "Could not complete NV-write operation due to loss of power, internal backup/ rollback mechanism activated "; break;
                case 0xF0: tMsg = "Specified file number dose not exit"; break;
                case 0xF1: tMsg = "Unrecoverable error within file, file will be disable"; break;
            }

            switch (pnErrCode)
            {
                case 0x1F: tMsg += Environment.NewLine + "Halt failed"; break;
                case 0x20: tMsg += Environment.NewLine + "No active ISO14443A tag in the RF field."; break;
                case 0x21: tMsg += Environment.NewLine + "Select failed"; break;
                case 0x22: tMsg += Environment.NewLine + "Authentication failed"; break;
                case 0x23: tMsg += Environment.NewLine + "Read failed"; break;
                case 0x24: tMsg += Environment.NewLine + "Write failed"; break;
                case 0x25: tMsg += Environment.NewLine + "E-wallet initialization failed"; break;
                case 0x26: tMsg += Environment.NewLine + "Readvalue failed"; break;
                case 0x27: tMsg += Environment.NewLine + "Decrement/Increment failed"; break;
                case 0x28: tMsg += Environment.NewLine + "Transfer failed"; break;
                case 0x29: tMsg += Environment.NewLine + "Write/Read E2PROM failed"; break;
                case 0x2A: tMsg += Environment.NewLine + "Load key failed"; break;
                case 0x2B: tMsg += Environment.NewLine + "Checkwrite failed"; break;
                case 0x2C: tMsg += Environment.NewLine + "Data for Checkwrite error"; break;
                case 0x2D: tMsg += Environment.NewLine + "Value operation failed"; break;
                case 0x2E: tMsg += Environment.NewLine + "UltraLight tag write failed"; break;
                case 0x30: tMsg += Environment.NewLine + "Anticollision failed"; break;
                case 0x31: tMsg += Environment.NewLine + "Forbidding multiple tag in RF field at the same time"; break;
                case 0x32: tMsg += Environment.NewLine + "MifareOne and Ultralight collision error"; break;
                case 0x33: tMsg += Environment.NewLine + "UltraLight tag collision error"; break;
            }

            return tMsg;
        }

        /// <summary>
        /// Set Beep
        /// </summary>
        private void C_SETxBeep()
        {
            byte nOnTime = 1, nOffTime = 0, nRepeat = 1;
            int nCmdRet = 0x30;

            try
            {
                nCmdRet = StaticClassReaderB.SetBeep(ref nW_ReaderAddr, nOnTime, nOffTime, nRepeat, nW_FrmHandle);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMiFare", "C_SETxBeep : " + oEx.Message); }
        }
    }
}
