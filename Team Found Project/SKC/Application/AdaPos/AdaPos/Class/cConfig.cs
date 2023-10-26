using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AdaPos.Class
{
    public class cConfig
    {
        public cConfig()
        {

        }

        /// <summary>
        /// Get config
        /// </summary>
        public void C_GETxConfig()
        {
            StringBuilder oSql;
            List<cmlTSysConfig> aoConfig;

            try
            {
                cVB.aoVB_PInvLayout = null;
                cVB.aoVB_PInvLayout = new List<Font>();

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSysCode, FTSysSeq, FTSysKey, FTSysStaUsrValue, FTSysStaUsrRef");
                oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                oSql.Append("WHERE FTSysApp IN ('ALL', 'POS'");

                switch (cVB.tVB_PosType)
                {
                    case "1": // Store
                        oSql.Append(", 'POS'");
                        break;

                    case "2": // Cashier
                        oSql.Append(", 'FC'");
                        break;
                }

                oSql.AppendLine(")");
                oSql.AppendLine("AND FTSysCode IN('tCN_UsrSignInType', 'tCN_AddressType', 'tCN_API2PSMaster', ");
                oSql.AppendLine("   'tCN_API2FNWallet', 'tCN_API2PSSale', 'tCN_SgnRStoreSrv', 'tCN_API2TKOrder', ");
                oSql.AppendLine("   'tCN_API2TKMaster', 'tCN_API2TKImage', 'tCN_API2FNAdaPay', 'tCN_API2FNAliPay', ");
                oSql.AppendLine("   'tCN_API2FNPromptPay', 'tCN_AgnKeyAPI', 'PInvLayout', 'tPS_DisplayOrder',");
                oSql.AppendLine("   'tPS_Print','tPS_CstDef','bPS_AlwPrintShift',"); //*Em 62-01-04  เพิ่ม tPS_Print
                oSql.AppendLine("   'bPS_PrnQRCode','tPS_PdtDef','nPS_QRTimeout',");     //*Em 62-08-01  AdaPos5.0 PTT
                oSql.AppendLine("   'cPS_MaxChg','cPS_SmallBill',");    //*Em 62-10-10
                oSql.AppendLine("   'tPS_PSysRnd', 'tPS_ModeSale', 'tPS_MaxData', 'tPS_ReadWriteWB', 'nPS_Refund',"); //*Arm 62-12-27  เพิ่ม nPS_Refund
                oSql.AppendLine("   'bPS_AlwPrnVoid','nPS_PrnRefund','nPS_PrnTax','AMQMember','bPS_AlwShw2ndScreen',");    //*Net 63-02-25 เพิ่มการเลือกพิมพ์รายการ Void , จำนวนการพิมพ์ ต้นฉบับ/สำเนา คืนบิล และใบกำกับภาษี   //*Arm 63-03-30 เพิ่ม AMQMember
                oSql.AppendLine("   'nVB_ChkShowBarQR', 'ACstPnt', 'nPS_ChkShowPdtBar',"); //*Net 63-04-03 //*Arm 63-04-03 เพิ่ม ACstPnt //*Arm 63-04-13 เพิ่ม nPS_ChkShowPdtBar
                oSql.AppendLine("   'nPS_StaSumPdt',"); //*Net 63-04-20 //*Arm 63-05-18 เอาวงเล็บออก เนื่องจากทำให้ Query ผิด
                oSql.AppendLine("   'tPS_TimeClearLog',");
                oSql.AppendLine("   'nPS_PrnSlip')"); //*Net 63-05-21


                aoConfig = new cDatabase().C_GETaDataQuery<cmlTSysConfig>(oSql.ToString());

                

                foreach (cmlTSysConfig oConfig in aoConfig)
                {
                    switch (oConfig.FTSysCode)
                    {
                        case "tCN_UsrSignInType":
                            switch (oConfig.FTSysKey)
                            {
                                case "ADMIN":
                                    switch (oConfig.FTSysSeq)
                                    {
                                        case "1":   // ประเภทของ Password > 1:Pincode
                                            break;
                                    }
                                    break;
                            }
                            break;

                        case "tCN_AddressType": // Address type
                            switch(oConfig.FTSysSeq)
                            {
                                case "6":   // Customer
                                    cVB.nVB_AddressTypeCst = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                                    break;
                            }
                            break;

                        case "tCN_API2PSMaster":
                            cVB.tVB_API2PSMaster = oConfig.FTSysStaUsrValue;
                            break;

                        case "tCN_API2FNWallet":

                            //*[AnUBiS][][2019-01-28] - เพิ่ม case แยก SysKey
                            switch (oConfig.FTSysKey.ToUpper())
                            {
                                case "POS":
                                    //cVB.tVB_API2FNWallet = oConfig.FTSysStaUsrValue;
                                    //*Em 62-12-24
                                    switch (oConfig.FTSysSeq)
                                    {
                                        case "1":   //HQ
                                            cVB.tVB_API2FNWalletHQ = oConfig.FTSysStaUsrValue;
                                            break;
                                        case "2":   //Branch
                                            cVB.tVB_API2FNWallet = oConfig.FTSysStaUsrValue;
                                            break;
                                    }
                                    //+++++++++++++
                                    break;
                            }
                            
                            break;

                        case "tCN_API2PSSale":
                            cVB.tVB_API2PSSale = oConfig.FTSysStaUsrValue;
                            break;

                        case "tCN_AgnKeyAPI":
                            cVB.tVB_AgnKeyAPI = oConfig.FTSysStaUsrValue;
                            cVB.tVB_APIHeader = oConfig.FTSysStaUsrRef;
                            break;

                        case "PInvLayout": // การพิมพ์สลิป (เฉพาะรวมพิมพ์)
                            switch (oConfig.FTSysSeq)
                            {
                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                case "5":
                                case "6":
                                case "7":
                                case "8":
                                case "9":
                                    C_SETxFont(oConfig.FTSysStaUsrValue);
                                    break;
                                case "98":  //*Em 62-10-29
                                    cVB.tVB_PathLogo = oConfig.FTSysStaUsrValue;
                                    break;
                            }

                            break;

                        case "tPS_Print":
                            switch(oConfig.FTSysSeq)
                            {
                                case "1":   // เปิดใช้งานเครื่องพิมพ์
                                    cVB.bVB_ChkPrint = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                                    break;

                                case "2":   // ประเภทการเชื่อมต่อเครื่องพิมพ์
                                    cVB.nVB_PrnType = Convert.ToInt32(oConfig.FTSysStaUsrValue);        //*Em 62-01-04
                                    break;

                                case "3":   // การเชื่อมต่อเครื่องพิมพ์
                                    cVB.tVB_PrnConn = oConfig.FTSysStaUsrValue;     
                                    break;

                                case "4":   // ขนาดกระดาษ
                                    cVB.nVB_PaperSize = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                                    break;

                            }
                            break;

                        case "tPS_PSysRnd":  // อนุญาตให้ปัดเศษ
                            cVB.bVB_PSysRnd = oConfig.FTSysStaUsrValue == "1" ? true : false;
                            break;

                        case "tPS_ModeSale":    // เปิดใช้งานโหมดสแกน?
                            cVB.bVB_ModeScan = oConfig.FTSysStaUsrValue == "1" ? true : false;
                            break;

                        case "tPS_MaxData":     // Max Data Limit
                            cVB.nVB_MaxData = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            break;

                        case "tPS_DisplayOrder":     // Order Display Format
                            cVB.nVB_DisplayOrder = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            break;

                        case "tPS_ReadWriteWB":     // Read Wrist Wristband
                            cVB.bVB_ReadWriteWB = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                            break;

                        case "bPS_PrnQRCode":
                            switch (oConfig.FTSysSeq)
                            {
                                case "1":
                                    cVB.bVB_PrnQRCode = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                                    break;
                            }
                            break;
                        case "tPS_PdtDef":
                            cVB.tVB_PdtCodeSrv = oConfig.FTSysStaUsrValue;
                            break;
                        case "nPS_QRTimeout":
                            switch (oConfig.FTSysSeq)
                            {
                                case "1":
                                    cVB.nVB_QRTimeout = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                                    break;
                            }
                            break;
                        case "tPS_CstDef":  //*Em 62-08-10
                            cVB.tVB_CstDef = oConfig.FTSysStaUsrValue;
                            cVB.tVB_CstDefName = oConfig.FTSysStaUsrRef;
                            break;

                        case "bPS_AlwPrintShift":   //*Em 62-10-02
                            switch (oConfig.FTSysSeq)
                            {
                                case "1":
                                    cVB.bVB_PrnShiftRCV = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                                    cVB.bVB_PrnShiftRCVRef = (string.Equals(oConfig.FTSysStaUsrRef, "1")) ? true : false;
                                    break;
                                case "2":
                                    cVB.bVB_PrnShiftBNK = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                                    cVB.bVB_PrnShiftBNKRef = (string.Equals(oConfig.FTSysStaUsrRef, "1")) ? true : false;
                                    break;
                                case "3":
                                    cVB.bVB_PrnShiftSUM = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                                    break;
                            }
                            break;
                        case "cPS_MaxChg":  //*Em 62-10-10
                            cVB.cVB_MaxChg = Convert.ToDecimal(oConfig.FTSysStaUsrValue);
                            break;
                        case "cPS_SmallBill":  //*Em 62-10-10
                            cVB.cVB_SmallBill = Convert.ToDecimal(oConfig.FTSysStaUsrValue);
                            break;
                        case "nPS_Refund":  //*Arm 62-12-27
                            cVB.nVB_ReturnType = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                              break;
                        case "bPS_AlwPrnVoid": //*Net 63-02-25
                            cVB.bVB_AlwPrnVoid = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                            break;
                        case "nPS_PrnRefund": //*Net 63-02-25
                            cVB.nVB_PrnRefundMaster = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            cVB.nVB_PrnRefundCopy = Convert.ToInt32(oConfig.FTSysStaUsrRef);
                            break;
                        case "nPS_PrnTax": //*Net 63-02-25
                            cVB.nVB_PrnTaxMaster = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            cVB.nVB_PrnTaxCopy = Convert.ToInt32(oConfig.FTSysStaUsrRef);
                            break;

                        case "nPS_PrnSlip": //*Net 63-05-21
                            cVB.nVB_PrnSlipMaster = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            cVB.nVB_PrnSlipCopy = Convert.ToInt32(oConfig.FTSysStaUsrRef);
                            break;

                        case "AMQMember": //*Arm 63-03-30
                           
                            switch (oConfig.FTSysSeq)
                            {
                                case "1":
                                    cVB.tVB_MemCgpCode = oConfig.FTSysStaUsrValue;
                                    break;
                                case "2":
                                    cVB.tVB_MemBchCode = oConfig.FTSysStaUsrValue;
                                    break;
                            }
                            break;
                        case "ACstPnt": //*Arm 63-04-03  เงื่อนไขการแลกแต้ม
                            cVB.cVB_PntOptBuyAmt = Convert.ToDecimal(oConfig.FTSysStaUsrValue);
                            cVB.cVB_PntOptGetQty = Convert.ToDecimal(oConfig.FTSysStaUsrRef) ;

                            break;
                        case "bPS_AlwShw2ndScreen":  //*Net 63-04-01 ยกมาจาก baseline
                            cVB.bVB_AlwShw2Screen = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                            break;
                        case "nVB_ChkShowBarQR":  //*Net 63-04-03
                            //cVB.nVB_ChkShowBarQR = Convert.ToInt32(oConfig.FTSysStaUsrRef);

                            //*Arm 63-04-10
                            if (string.IsNullOrEmpty(oConfig.FTSysStaUsrValue))
                            {
                                cVB.nVB_ChkShowBarQR = 0;
                            }
                            else
                            {
                                cVB.nVB_ChkShowBarQR = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            }
                            //++++++++++++++
                            break;

                        case "nPS_ChkShowPdtBar":   //*Arm 63-04-13 พิมพ์รหัสสินค้าหรือบาร์โค้ดในแสดงสลิป  0:ไม่แสดง, 1:แสดง Product Code, 2:แสดง Barcode

                            if (string.IsNullOrEmpty(oConfig.FTSysStaUsrValue))
                            {
                                cVB.nVB_ChkShowPdtBarCode = 0;
                            }
                            else
                            {
                                cVB.nVB_ChkShowPdtBarCode = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            }

                            break;
                        case "nPS_StaSumPdt":  //*Net 63-04-20 จำนวนสินค้าหน้าจอขาย 1:แยกรายการ 2:รวมรายการ
                            cVB.nPS_StaSumPdt = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            //if (cVB.nPS_StaSumPdt == 0) cVB.nPS_StaSumPdt = 1; //Net 63-05-21 ย้ายไปข้างล่าง
                            break;
                        case "tPS_TimeClearLog":  //*Zen 63-05-15 เวลาที่ใช้ในการเก็บ Log
                            cVB.nVB_TimeClearLog = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            //if (cVB.nVB_TimeClearLog == 0) cVB.nVB_TimeClearLog = 100; //Net 63-05-21 ย้ายไปข้างล่าง
                            break;
                    }
                    //cVB.nVB_ChkShowBarQR = 1; //*Arm 62-10-31 -กำหนดเงื่อนไขการแสดงบิล 1 : แสดง Barcode, 2 : แสดง QRcode, 3 : แสดงทั้ง Barcode และ QRcode
                }


                cVB.bVB_AlwSaleChkStk = false; //*Net 63-05-13 (ชั่วคราว) 
                
                if (cVB.nVB_PrnRefundMaster == 0) cVB.nVB_PrnRefundMaster = 1; //*Net 63-05-21
                if (cVB.nVB_PrnSlipMaster == 0) cVB.nVB_PrnSlipMaster = 1; //*Net 63-05-21
                if (cVB.nPS_StaSumPdt == 0) cVB.nPS_StaSumPdt = 1;
                if (cVB.nVB_TimeClearLog == 0) cVB.nVB_TimeClearLog = 100;
                if (cVB.nVB_ChkShowBarQR == 0) cVB.nVB_ChkShowBarQR = 1; //*Net 63-04-03
                if (cVB.nVB_MaxData == 0) cVB.nVB_MaxData = 100;

                //*Em 63-01-08
                cVB.tVB_HQBchCode = new cDatabase().C_GETtFunction("TOP 1", "FTBchCode", "TCNMBranch", "WHERE ISNULL(FTBchStaHQ,'') = '1'");
                cVB.tVB_API2FNWallet = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "' AND FNUrlType = 8");
                cVB.tVB_API2FNWalletHQ = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_HQBchCode + "' AND FNUrlType = 8");
                //++++++++++++++

                //*Arm 63-02-19
                cVB.tVB_API2ARDoc = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "' AND FNUrlType = 12"); //*Arm 63-03-30 แก้ไข FNUrlType = 12"
                //++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cConfig", "C_GETxConfig : " + oEx.Message); }
        }

        /// <summary>
        /// Get config service
        /// </summary>
        public List<cmlTSysConfig> C_GETaCfgService()
        {
            List<cmlTSysConfig> aoCfg = new List<cmlTSysConfig>();
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT CFG.FTSysCode, CFG.FTSysKey, FTSysName, FTSysStaUsrValue");
                oSql.AppendLine("FROM TSysConfig CFG WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TSysConfig_L CFGL WITH(NOLOCK) ON CFGL.FTSysCode = CFG.FTSysCode ");
                oSql.AppendLine("   AND CFGL.FTSysSeq = CFG.FTSysSeq");
                oSql.AppendLine("   AND CFGL.FTSysKey = CFG.FTSysKey"); //*Em 61-11-19
                oSql.AppendLine("   AND CFGL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE FTGmnCode = 'SERVICES'");
                oSql.AppendLine("AND CFG.FTSysKey <> 'ADAPAY'");    //*Em 62-08-30
                switch (cVB.tVB_PosType) 
                {
                    case "1":   //*Em 61-11-19
                    case "2":   // Cashier
                        oSql.AppendLine("AND CFG.FTSysKey != 'TICKET'");
                        break;
                    default:
                        oSql.AppendLine("AND CFG.FTSysKey != 'TICKET'");
                        break;
                }

                aoCfg = new cDatabase().C_GETaDataQuery<cmlTSysConfig>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cConfig", "C_GETaCfgService : " + oEx.Message); }

            return aoCfg;
        }

        /// <summary>
        /// Set Font to List
        /// </summary>
        /// <param name="ptFormatFont"></param>
        private void C_SETxFont(string ptFormatFont)
        {
            FontStyle oFontStyle;
            Font oFont;
            string[] atFormatFont;

            try
            {
                atFormatFont = ptFormatFont.Split(',');
                oFontStyle = FontStyle.Regular;

                if (!string.IsNullOrEmpty(atFormatFont[2]))
                    oFontStyle |= FontStyle.Bold;

                if (!string.IsNullOrEmpty(atFormatFont[3]))
                    oFontStyle |= FontStyle.Italic;

                if (!string.IsNullOrEmpty(atFormatFont[4]))
                    oFontStyle |= FontStyle.Underline;

                oFont = new Font(atFormatFont[0], Convert.ToSingle(atFormatFont[1]), oFontStyle);

                cVB.aoVB_PInvLayout.Add(oFont);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cConfig", "C_SETxFont : " + oEx.Message); }
            finally
            {
                ptFormatFont = null;
                atFormatFont = null;
                oFont = null;
                new cSP().SP_CLExMemory();
            }
        }
    }
}
