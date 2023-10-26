using AdaPos.Models.Database;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.Data;
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
                cVB.bPS_PrintHoldBill = true;
                cVB.bVB_ChkPosRegister = false; //*Arm 63-07-10 option Check Pos Register
                //*Net 63-07-30 ยกมาจาก Moshi
                cVB.bVB_AlwChkShfRCV = true; //*Net 63-07-10 default ค่า ก่อนไป get ค่า config
                cVB.bVB_AlwShwCstScreen = false; //*Net 63-07-10 default แสดงจอ 2 เป็น false
                cVB.bVB_PrnRownum = false; //*Net 63-07-16 พิมพ์ลำดับสินค้าในใบเสร็จ

                oSql = new StringBuilder(); //'*Em 63-05-27  ค่า Defualt

                oSql.AppendLine("SELECT FTSysCode, FTSysSeq, FTSysKey, FTSysStaUsrValue, FTSysStaUsrRef");
                oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                //oSql.Append("WHERE FTSysApp IN ('ALL', 'POS'");
                oSql.Append("WHERE FTSysApp IN ('ALL','CN','MB'"); //*Em 63-08-17 //*Arm 63-08-17 เพิ่ม  MB

                switch (cVB.tVB_PosType)
                {
                    case "1": // Store
                        //oSql.Append(", 'POS'");
                        oSql.Append(", 'PS'");  //*Em 63-08-17
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
                oSql.AppendLine("   'cPS_MaxChg','cPS_SmallBill','nVB_BrwTopWin',");    //*Em 62-10-10 //*Arm 63-08-28 เพิ่ม nVB_BrwTopWin
                oSql.AppendLine("   'tPS_PSysRnd', 'tPS_ModeSale', 'tPS_MaxData', 'tPS_ReadWriteWB', 'nPS_Refund',"); //*Arm 62-12-27  เพิ่ม nPS_Refund
                oSql.AppendLine("   'bPS_AlwPrnVoid','nPS_PrnRefund','nPS_PrnTax','AMQMember','bPS_AlwShw2ndScreen',");    //*Net 63-02-25 เพิ่มการเลือกพิมพ์รายการ Void , จำนวนการพิมพ์ ต้นฉบับ/สำเนา คืนบิล และใบกำกับภาษี   //*Arm 63-03-30 เพิ่ม AMQMember
                oSql.AppendLine("   'nVB_ChkShowBarQR', 'ACstPnt', 'nPS_ChkShowPdtBar',"); //*Net 63-04-03 //*Arm 63-04-03 เพิ่ม ACstPnt //*Arm 63-04-13 เพิ่ม nPS_ChkShowPdtBar
                oSql.AppendLine("   'nPS_StaSumPdt',"); //*Net 63-04-20
                oSql.AppendLine("   'tPS_TimeClearLog',"); //*Zen 63-05-15
                oSql.AppendLine("   'bPS_AlwPrintHoldBill', 'nPS_PrnSlip','bPS_StaChkPosReg',"); //*Net 63-04-20 //*Arm 63-06-11 เพิ่ม nPS_PrnSlip ยกมาจาห SKC เดิม(CR P1-005 การพิมพ์สำเนาใบเสร็จ) // *Arm 63-07-10 เพิ่ม bPS_StaChkPosReg
                //*Net 63-07-30 ยกมาจาก Moshi
                oSql.AppendLine("   'bPS_AlwChkShfRCV',"); //*Net 63-07-07
                oSql.AppendLine("   'bPS_AlwShwCstScreen',"); //*Net 63-07-10
                oSql.AppendLine("   'bPS_PrnRownum',"); //*Net 63-07-16
                oSql.AppendLine("   'nPS_DelayTimeChg',");  //*Em 63-0726
                oSql.AppendLine("   'nPS_FixDocTimeout',");  //*Net 63-0726
                oSql.AppendLine("   'bCN_AlwPmtDisAvg',");  //*Em 63-08-27
                oSql.AppendLine("   'ADecPntSav','ADecPntShw')"); //*Net 63-06-23

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
                            //cVB.tVB_API2PSMaster = oConfig.FTSysStaUsrValue;
                            
                            //*Arm 63-08-17
                            // ถ้าไม่มีข้อมูล
                            if(string.IsNullOrEmpty(oConfig.FTSysStaUsrValue))
                            {
                                // หาจากสาขา
                                oSql.Clear();
                                oSql.AppendLine("SELECT TOP 1 FTUrlAddress");
                                oSql.AppendLine("FROM TCNTUrlObject WITH(NOLOCK)");
                                oSql.AppendLine("WHERE FNUrlType = '4'");
                                oSql.AppendLine("AND FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "'");
                                cVB.tVB_API2PSMaster = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                                if (string.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                                {
                                    // ถ้าไม่มี หาจาก CENTER
                                    oSql.Clear();
                                    oSql.AppendLine("SELECT TOP 1 FTUrlAddress");
                                    oSql.AppendLine("FROM TCNTUrlObject WITH(NOLOCK)");
                                    oSql.AppendLine("WHERE FNUrlType = '4'");
                                    oSql.AppendLine("AND FTUrlTable = 'TCNMComp' AND FTUrlRefID = 'CENTER'");
                                    cVB.tVB_API2PSMaster = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                                }

                                if (!string.IsNullOrEmpty(cVB.tVB_API2PSMaster))
                                {
                                    // มีข้อมูลบันทึกลง TSysConfig
                                    oSql.Clear();
                                    oSql.AppendLine("UPDATE TSysConfig WITH(ROWLOCK) SET");
                                    oSql.AppendLine("FTSysStaUsrValue = '" + cVB.tVB_API2PSMaster + "'");
                                    oSql.AppendLine("WHERE FTSysCode = 'tCN_API2PSMaster'");
                                    new cDatabase().C_SETxDataQuery(oSql.ToString());
                                }
                            }
                            else
                            {
                                // ถ้ามีข้อมูล ใช้ตาม TSysConfig
                                cVB.tVB_API2PSMaster = oConfig.FTSysStaUsrValue;
                            }
                            //++++++++++++++

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

                            //cVB.tVB_API2PSSale = oConfig.FTSysStaUsrValue;

                            //*Arm 63-08-17
                            // ถ้าไม่มีข้อมูล
                            if (string.IsNullOrEmpty(oConfig.FTSysStaUsrValue))
                            {
                                // หาจากสาขา
                                oSql.Clear();
                                oSql.AppendLine("SELECT TOP 1 FTUrlAddress");
                                oSql.AppendLine("FROM TCNTUrlObject WITH(NOLOCK)");
                                oSql.AppendLine("WHERE FNUrlType = '5'");
                                oSql.AppendLine("AND FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "'");
                                cVB.tVB_API2PSSale = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());

                                if (string.IsNullOrEmpty(cVB.tVB_API2PSSale))
                                {
                                    // ถ้าไม่มี หาจาก CENTER
                                    oSql.Clear();
                                    oSql.AppendLine("SELECT TOP 1 FTUrlAddress");
                                    oSql.AppendLine("FROM TCNTUrlObject WITH(NOLOCK)");
                                    oSql.AppendLine("WHERE FNUrlType = '5'");
                                    oSql.AppendLine("AND FTUrlTable = 'TCNMComp' AND FTUrlRefID = 'CENTER'");
                                    cVB.tVB_API2PSSale = new cDatabase().C_GEToDataQuery<string>(oSql.ToString());
                                }

                                if (!string.IsNullOrEmpty(cVB.tVB_API2PSSale))
                                {
                                    // มีข้อมูลบันทึกลง TSysConfig
                                    oSql.Clear();
                                    oSql.AppendLine("UPDATE TSysConfig WITH(ROWLOCK) SET");
                                    oSql.AppendLine("FTSysStaUsrValue = '" + cVB.tVB_API2PSSale + "'");
                                    oSql.AppendLine("WHERE FTSysCode = 'tCN_API2PSSale'");
                                    new cDatabase().C_SETxDataQuery(oSql.ToString());
                                }
                            }
                            else
                            {
                                // ถ้ามีข้อมูล ใช้ตาม TSysConfig
                                cVB.tVB_API2PSSale = oConfig.FTSysStaUsrValue;
                            }
                            //+++++++++++++
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
                                case "4":
                                    cVB.bVB_PrnShiftSumRedeem = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false; //*Arm 63-05-27
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
                        case "nPS_PrnSlip": //*Net 63-05-21 //*Arm 63-06-11 ยกมาจาห SKC เดิม(CR P1-005 การพิมพ์สำเนาใบเสร็จ)
                            cVB.nVB_PrnSlipMaster = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            cVB.nVB_PrnSlipCopy = Convert.ToInt32(oConfig.FTSysStaUsrRef);
                            break;
                        case "bPS_AlwPrintHoldBill": //*Net 63-05-27
                            cVB.bPS_PrintHoldBill = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
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
                            //if (cVB.nPS_StaSumPdt == 0) cVB.nPS_StaSumPdt = 1;
                            break;
                        case "tPS_TimeClearLog":  //*Zen 63-05-15 เวลาที่ใช้ในการเก็บ Log
                            cVB.nVB_TimeClearLog = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            if (cVB.nVB_TimeClearLog == 0) cVB.nVB_TimeClearLog = 100;
                            break;

                        case "bPS_StaChkPosReg": //*Arm 63-07-10 option Check Pos Register
                            if (!string.IsNullOrEmpty(oConfig.FTSysStaUsrValue))
                            {
                                cVB.bVB_ChkPosRegister = oConfig.FTSysStaUsrValue == "1" ? true : false;
                            }
                            break;

                       //*Net 63-07-30 ยกมาจาก Moshi
                        case "ADecPntSav": //*Net 63-06-23
                            cVB.nVB_DecSave = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            break;
                        case "ADecPntShw": //*Net 63-06-23
                            cVB.nVB_DecShow = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            break;
                        case "bPS_AlwChkShfRCV": //*Net 63-07-07
                            cVB.bVB_AlwChkShfRCV = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                            break;
                        case "bPS_AlwShwCstScreen":  //*Net 63-07-10 แสดงหน้าจอลูกค้า
                            cVB.bVB_AlwShwCstScreen = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                            break;
                        case "bPS_PrnRownum": //*Net 63-07-16 ย้ายมาจาก Setting
                            cVB.bVB_PrnRownum = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                            break;
                        case "nPS_DelayTimeChg":    //*Em 63-07-26
                            cVB.nVB_DelayTimeChg = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            break;

                        case "nPS_FixDocTimeout":
                            cVB.nVB_CountDown = Convert.ToInt32(oConfig.FTSysStaUsrValue);
                            break;
                        //++++++++++++++++++++++++++++

                        case "bCN_AlwPmtDisAvg":    //*Em 63-08-27
                            cVB.bVB_AlwPmtDisAvg = (string.Equals(oConfig.FTSysStaUsrValue, "1")) ? true : false;
                            break;

                        case "nVB_BrwTopWin": //*Arm 63-08-28 แสดงจำนวนรายการ
                            if (!string.IsNullOrEmpty(oConfig.FTSysStaUsrValue)){ cVB.nVB_BrwTop = Convert.ToInt32(oConfig.FTSysStaUsrValue); }
                            break;
                    }
                    //cVB.nVB_ChkShowBarQR = 1; //*Arm 62-10-31 -กำหนดเงื่อนไขการแสดงบิล 1 : แสดง Barcode, 2 : แสดง QRcode, 3 : แสดงทั้ง Barcode และ QRcode
                }

                if (cVB.nVB_BrwTop == 0) cVB.nVB_BrwTop = 1000; //*Arm 63-08-28 ถ้าไม่ได้กำหนด Config  Defualt แสดงจำนวนรายการ = 1000 

                //*Net 63-07-30 ยกมาจาก Moshi
                if (cVB.nVB_CountDown == 0) cVB.nVB_CountDown = 3; //*Net 63-07-25
                if (cVB.nVB_DecSave == 0) cVB.nVB_DecSave = 4; //*Net 63-06-23
                if (cVB.nVB_DecShow == 0) cVB.nVB_DecShow = 2; //*Net 63-06-23
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++
                if (cVB.nPS_StaSumPdt == 0) cVB.nPS_StaSumPdt = 1;
                if (cVB.nVB_ChkShowBarQR == 0) cVB.nVB_ChkShowBarQR = 1; //*Net 63-04-03
                if (cVB.nVB_MaxData == 0) cVB.nVB_MaxData = 100;
                if (cVB.nVB_TimeClearLog == 0) cVB.nVB_TimeClearLog = 7;    //*Em 63-05-15
                if (cVB.nVB_PrnSlipMaster == 0) cVB.nVB_PrnSlipMaster = 1;  //*Em 63-06-21
                if (cVB.nVB_PrnRefundMaster == 0) cVB.nVB_PrnRefundMaster = 1;  //*Em 63-06-21

                //*Em 63-01-08
                cVB.tVB_HQBchCode = new cDatabase().C_GETtFunction("TOP 1", "FTBchCode", "TCNMBranch", "WHERE ISNULL(FTBchStaHQ,'') = '1'");
                cVB.tVB_API2FNWallet = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "' AND FNUrlType = 8");
                cVB.tVB_API2FNWalletHQ = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_HQBchCode + "' AND FNUrlType = 8");
                //++++++++++++++

                //*Arm 63-02-19
                cVB.tVB_API2ARDoc = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "' AND FNUrlType = 12"); //*Arm 63-03-30 แก้ไข FNUrlType = 12"

                //*Arm 63-08-19 
                if (string.IsNullOrEmpty(cVB.tVB_API2ARDoc)) //*Arm 63-08-19 API2ARDoc ถ้าไม่มีของสาขา ให้ดึงจาก Center
                {
                    cVB.tVB_API2ARDoc = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMComp' AND FTUrlRefID = 'CENTER' AND FNUrlType = 12"); //*Arm 63-08-19
                }
                if (string.IsNullOrEmpty(cVB.tVB_API2FNWallet)) //*Arm 63-08-19 API2FNWallet ถ้าไม่มีของสาขา ให้ดึงจาก Center
                {
                    cVB.tVB_API2FNWallet = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMComp' AND FTUrlRefID = 'CENTER' AND FNUrlType = 8"); //*Arm 63-08-19
                }
                if (string.IsNullOrEmpty(cVB.tVB_API2FNWalletHQ)) //*Arm 63-08-19 API2FNWallet(HQ) ถ้าไม่มีของสาขา ให้ดึงจาก Center
                {
                    cVB.tVB_API2FNWalletHQ = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMComp' AND FTUrlRefID = 'CENTER' AND FNUrlType = 8"); //*Arm 63-08-19
                }
                //++++++++++++++

                //*Arm 63-06-26
                //cVB.tVB_API2PSSale = new cDatabase().C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "' AND FNUrlType = 5"); //*Arm 63-06-26

                ////Get URL Check Stock
                //string tChkStk = C_GETxCfgApiKADS("00001"); 
                //string[] aChkStk = tChkStk.Split(',');
                //cVB.tVB_KADSAuth_ChkStock = aChkStk[0].ToString();
                //cVB.tVB_APIKADS_ChkStock = aChkStk[1].ToString();

                ////Get URL Search Customer
                //string tSchCst = C_GETxCfgApiKADS("00002"); 
                //string[] aSchCst = tSchCst.Split(',');
                //cVB.tVB_KADSAuth = aSchCst[0].ToString();
                //cVB.tVB_APIKADS = aSchCst[1].ToString();

                ////Get URL Vehicle
                //string tVehicle = C_GETxCfgApiKADS("00003"); 
                //string[] aVehicle = tVehicle.Split(',');
                //cVB.tVB_KADSAuth_Vehicle = aVehicle[0].ToString();
                //cVB.tVB_APIKADS_Vehicle = aVehicle[1].ToString();
                ////+++++++++++++

                ////Get URL Get Token
                //string tGetToken = C_GETxCfgApiKADS("00004");
                //string[] aGetToken = tGetToken.Split(',');
                //cVB.tVB_KADSAuth_ReqToken = aGetToken[0].ToString();
                //cVB.tVB_APIKADS_ReqToken_Url = aGetToken[1].ToString();
                ////+++++++++++++

                C_GETxCfgTxnApi("00001");
                C_GETxCfgTxnApi("00002");
                C_GETxCfgTxnApi("00003");
                C_GETxCfgTxnApi("00004");
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cConfig", "C_GETxConfig : " + oEx.Message); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptBchCode"></param>
        /// <returns></returns>
        public List<cmlGetURL> C_GETaCfgUrl(string ptBchCode)
        {
            cDatabase oDB;
            cmlGetURL oGetURL;
            List<cmlGetURL> aoURL = new List<cmlGetURL>();
            try
            {
                oDB = new cDatabase();
                
                oGetURL = new cmlGetURL();
                oGetURL.tUrlGroup = "ALL";
                oGetURL.tUrlName = "API2FNWallet";
                //oGetURL.tUrl = oDB.C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "' AND FNUrlType = 8"); ;
                oGetURL.tUrl = cVB.tVB_API2FNWallet; //*Arm 63-08-19
                aoURL.Add(oGetURL);

                oGetURL = new cmlGetURL();
                oGetURL.tUrlGroup = "ALL";
                oGetURL.tUrlName = "API2FNWallet(HQ)";
                //oGetURL.tUrl = oDB.C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_HQBchCode + "' AND FNUrlType = 8"); ;
                oGetURL.tUrl = cVB.tVB_API2FNWalletHQ; //*Arm 63-08-19
                aoURL.Add(oGetURL);

                oGetURL = new cmlGetURL();
                oGetURL.tUrlGroup = "ALL";
                oGetURL.tUrlName = "API2ARDoc";
                //oGetURL.tUrl = oDB.C_GETtFunction("TOP 1", "FTUrlAddress", "TCNTUrlObject", "WHERE FTUrlTable = 'TCNMBranch' AND FTUrlRefID = '" + cVB.tVB_BchCode + "' AND FNUrlType = 12"); ;
                oGetURL.tUrl = cVB.tVB_API2ARDoc; //*Arm 63-08-19
                aoURL.Add(oGetURL);
                
                //*Arm 63-08-09
                oGetURL = new cmlGetURL();
                if(cVB.tVB_WahStaChkStk != "3")
                {
                    oGetURL.tUrlGroup = "ALL";
                    oGetURL.tUrlName = "Check Stock Online";
                    oGetURL.tUrl = cVB.oVB_GBResource.GetString("tStaChkStk");
                }
                else
                {
                    oGetURL.tUrlGroup = "ALL";
                    oGetURL.tUrlName = "Check Stock Online(" + cVB.tVB_ApiChkStk_Fmt + ")";
                    oGetURL.tUrl = cVB.tVB_ApiChkStk;
                }
                aoURL.Add(oGetURL);
                

                oGetURL = new cmlGetURL();
                oGetURL.tUrlGroup = "ALL";
                if(string.IsNullOrEmpty(cVB.tVB_ApiCstSch_Fmt))
                {
                    oGetURL.tUrlName = "Search Customer";
                }
                else
                {
                    oGetURL.tUrlName = "Search Customer(" + cVB.tVB_ApiCstSch_Fmt + ")";
                }
                oGetURL.tUrl = cVB.tVB_ApiCstSch;
                aoURL.Add(oGetURL);

                oGetURL = new cmlGetURL();
                oGetURL.tUrlGroup = "ALL";
                if (string.IsNullOrEmpty(cVB.tVB_ApiVehicle_Fmt))
                {
                    oGetURL.tUrlName = "Search Vehicle";
                }
                else
                {
                    oGetURL.tUrlName = "Search Vehicle(" + cVB.tVB_ApiVehicle_Fmt + ")";
                }
                oGetURL.tUrl = cVB.tVB_ApiVehicle;
                aoURL.Add(oGetURL);

                oGetURL = new cmlGetURL();
                oGetURL.tUrlGroup = "ALL";
                if (string.IsNullOrEmpty(cVB.tVB_ApiGetToken_Fmt))
                {
                    oGetURL.tUrlName = "Get Token";
                }
                else
                {
                    oGetURL.tUrlName = "Get Token(" + cVB.tVB_ApiGetToken_Fmt + ")";
                }
                oGetURL.tUrl = cVB.tVB_ApiGetToken;
                aoURL.Add(oGetURL);

                //+++++++++++++

                //*Arm 63-08-19 GET Service MQSale
                oGetURL = new cmlGetURL();
                oGetURL.tUrlGroup = "MQ";
                oGetURL.tUrlName = "MQSale";
                if(!string.IsNullOrEmpty(cVB.tVB_BCHMQHost))
                {
                    oGetURL.tUrl = "S:" + cVB.tVB_BCHMQHost + ", Port:" + cVB.tVB_BCHMQPort + ", U:" + cVB.tVB_BCHMQUsr + ", V:" + cVB.tVB_BCHMQVirtual;
                }
                else
                {
                    oGetURL.tUrl = "S:" + cVB.tVB_HQMQHost + ", Port:" + cVB.tVB_HQMQPort + ", U:" + cVB.tVB_HQMQUsr + ", V:" + cVB.tVB_HQMQVirtual;
                }
                aoURL.Add(oGetURL);

                //*Arm 63-08-19 Service MQ AdaMember
                oGetURL = new cmlGetURL();
                oGetURL.tUrlGroup = "MQ";
                oGetURL.tUrlName = "MQMember";
                oGetURL.tUrl = "S:" + cVB.tVB_MemberMQHost + ", Port:" + cVB.tVB_MemberMQPort + ", U:" + cVB.tVB_MemberMQUsr + ", V:" + cVB.tVB_MemberMQVirtual;
                aoURL.Add(oGetURL);
                //+++++++++++++

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cConfig", "C_GETaCfgUrl : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oGetURL = null;
            }
            return aoURL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptApiCode"></param>
        public void C_GETxCfgTxnApi(string ptApiCode)
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;

            string tUsername = "";
            string tPassword = "";
            string tAth = "";
            string tUrl = "";
            string tType = "";
            string tTxnType = "";
            string tFmtApi = "";
            try
            {
                // ptApiCode Disciption 
                // 00001 : API เช็คสต๊อกออนไลน์
                // 00002 : API ตรวจสอบข้อมูล Privilege ของสมาชิก 
                // 00003 : API เรียกข้อมูลรถของสมาชิก
                // 00004 : API ขอข้อมูล token เพื่อใช้ส่งใน sale transaction

                
                //*กรณี ถ้าคลังกำหนดไม่เช็คสต๊อกออนไลน์ ไม่ต้องหา api 00001 API เช็คสต๊อกออนไลน์
                if (ptApiCode == "00001" && cVB.tVB_WahStaChkStk != "3") return;

                oSql = new StringBuilder();
                oDB = new cDatabase();

                
                for (int n = 1; n <= 2; n++)  // รอบที่ 1 Link ,รอบที่ 2 STD
                {
                    switch (n)
                    {
                        case 1: // หาข้อมูล Api (LINK)
                            tTxnType = "4";
                            tFmtApi = "00001"; // รูปแบบ KADS
                            break;
                        case 2: // หาข้อมูล Api ADA (STD)
                            tTxnType = "3";
                            tFmtApi = "00003"; // รูปแบบ ADA
                            break;
                    }

                    //หาข้อมูล Api check stock จาก TCNMTxnSpcAPI where FTApiFmtCode 4
                    oSql.Clear();
                    oSql.AppendLine("SELECT ISNULL(FTApiURL,'') AS FTApiURL , ISNULL(FTSpaUsrCode,'') AS FTSpaUsrCode, ISNULL(FTSpaUsrPwd,'') AS FTSpaUsrPwd ");
                    oSql.AppendLine("FROM TCNMTxnSpcAPI ");
                    oSql.AppendLine("WHERE ISNULL(FTApiCode, '') = '" + ptApiCode + "' ");
                    oSql.AppendLine("AND(ISNULL(FTCmpCode, '') = '' OR ISNULL(FTCmpCode, '') = '" + cVB.tVB_CmpCode + "') ");
                    oSql.AppendLine("AND(ISNULL(FTAgnCode, '') = '' OR ISNULL(FTAgnCode, '') = '" + cVB.tVB_AgnCode + "') ");
                    oSql.AppendLine("AND(ISNULL(FTBchCode, '') = '' OR ISNULL(FTBchCode, '') = '" + cVB.tVB_BchCode + "') ");
                    oSql.AppendLine("AND(ISNULL(FTMerCode, '') = '' OR ISNULL(FTMerCode, '') = '" + cVB.tVB_Merchart + "') ");
                    oSql.AppendLine("AND(ISNULL(FTShpCode, '') = '' OR ISNULL(FTShpCode, '') = '" + cVB.tVB_ShpCode + "') ");
                    oSql.AppendLine("AND(ISNULL(FTPosCode, '') = '' OR ISNULL(FTPosCode, '') = '" + cVB.tVB_PosCode + "') ");
                    oSql.AppendLine("AND FTApiFmtCode = '" + tFmtApi + "' ");
                    odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        //ถ้าเจอ เก็บลงตัวแปล
                        tUsername = odtTmp.Rows[0].Field<string>("FTSpaUsrCode");
                        if (!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTSpaUsrPwd")))
                        {
                            tPassword = new cEncryptDecrypt("2").C_CALtDecrypt(odtTmp.Rows[0].Field<string>("FTSpaUsrPwd")); //*Arm 63-08-07
                        }
                        tAth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(tUsername + ":" + tPassword)));
                        tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
                        if (n == 1)
                        {
                            tType = "SKC";
                        }
                        else
                        {
                            tType = "ADA";
                        }

                        if (ptApiCode == "00001" && tType == "SKC") //Check stock online
                        {
                            // หา Sloc จาก TLKMWahouse.FTWahRefNo
                            oSql.Clear();
                            oSql.AppendLine("SELECT TOP 1 ISNULL(FTWahRefNo,'') AS FTWahRefNo FROM TLKMWaHouse WITH(NOLOCK) ");
                            //oSql.AppendLine("WHERE ISNULL(FTBchCode,'') = '" + cVB.tVB_BchCode + "' AND ISNULL(FTAgnCode,'') = '" + cVB.tVB_AgnCode + "' AND ISNULL(FTWahCode,'') = '" + cVB.tVB_WahCode + "' AND ISNULL(FTWahStaChannel,'') != '3' "); //*Arm 63-08-15
                            oSql.AppendLine("WHERE ISNULL(FTBchCode,'') = '" + cVB.tVB_BchCode + "' AND ISNULL(FTAgnCode,'') = '" + cVB.tVB_AgnCode + "' AND ISNULL(FTWahCode,'') = '" + cVB.tVB_WahCode + "' "); //*Arm 63-08-16
                            cVB.tVB_WahRefNo = oDB.C_GEToDataQuery<string>(oSql.ToString()); //Sloc
                        }
                    }
                    else
                    {
                        // ถ้าไม่เจอ
                        oSql.AppendLine("SELECT TOP 1 FTApiCode, ISNULL(FTApiURL,'') AS FTApiURL , ISNULL(FTApiLoginUsr,'') AS FTApiLoginUsr, ISNULL(FTApiLoginPwd,'') AS FTApiLoginPwd ");
                        oSql.AppendLine("FROM TCNMTxnAPI WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTApiTxnType = '" + tTxnType + "' AND FTApiFmtCode = '" + tFmtApi + "'");
                        oSql.AppendLine("AND FTApiCode = '" + ptApiCode + "'");
                        odtTmp = null;
                        odtTmp = new DataTable();
                        odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                        if (odtTmp != null && odtTmp.Rows.Count > 0)
                        {
                            if (string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTApiURL")) &&
                                string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTApiLoginUsr")) &&
                                string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTApiLoginPwd"))
                                )
                            {
                                tUsername = odtTmp.Rows[0].Field<string>("FTApiLoginUsr");
                                if (!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTApiLoginPwd")))
                                {
                                    tPassword = new cEncryptDecrypt("2").C_CALtDecrypt(odtTmp.Rows[0].Field<string>("FTApiLoginPwd")); //*Arm 63-08-07
                                }
                                
                                tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");

                                if (n == 1)
                                {
                                    tAth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(tUsername + ":" + tPassword)));
                                    tType = "SKC";
                                }
                                else
                                {
                                    tType = "ADA";
                                }

                                if (ptApiCode == "00001" && tType == "SKC") //Check stock online
                                {
                                    // หา Sloc จาก TLKMWahouse.FTWahRefNo
                                    oSql.Clear();
                                    oSql.AppendLine("SELECT TOP 1 ISNULL(FTWahRefNo,'') AS FTWahRefNo FROM TLKMWaHouse WITH(NOLOCK) ");
                                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTAgnCode = '" + cVB.tVB_AgnCode + "' AND FTWahCode = '" + cVB.tVB_WahCode + "' AND FTWahStaChannel != '3' ");
                                    cVB.tVB_WahRefNo = oDB.C_GEToDataQuery<string>(oSql.ToString()); //Sloc
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(tType))
                    {
                        //ถ้าเจอให้หยุด
                        break;
                    }
                }

                switch(ptApiCode)
                {
                    case "00001":
                        cVB.tVB_ApiChkStk = tUrl;
                        cVB.tVB_ApiChkStk_Auth = tAth;
                        cVB.tVB_ApiChkStk_Fmt = tType;
                        break;
                    case "00002":
                        cVB.tVB_ApiCstSch = tUrl;
                        cVB.tVB_ApiCstSch_Auth = tAth;
                        cVB.tVB_ApiCstSch_Fmt = tType;
                        break;
                    case "00003":
                        cVB.tVB_ApiVehicle = tUrl;
                        cVB.tVB_ApiVehicle_Auth = tAth;
                        cVB.tVB_ApiVehicle_Fmt = tType;
                        break;
                    case "00004":
                        cVB.tVB_ApiGetToken = tUrl;
                        cVB.tVB_ApiGetToken_Auth = tAth;
                        cVB.tVB_ApiGetToken_Fmt = tType;
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cConfig", "C_GETxCfgApiKADS : " + oEx.Message);
            }
            finally
            {
                ptApiCode = "";
                oSql = null;
                oDB = null;
                odtTmp = null;
            }
        }

        ///// <summary>
        ///// *Arm 63-06-25 GET Config Api KADS
        ///// </summary>
        ///// <param name="ptApiCode">00001:Check Stock, 2:Search Customer</param>
        //public string C_GETxCfgApiKADS(string ptApiCode)
        //{
        //    StringBuilder oSql;
        //    cDatabase oDB;
        //    DataTable odtTmp;

        //    string tUsername = "";
        //    string tPassword = "";
        //    string tAth = "";
        //    string tUrl = "";
        //    string tResult = "";
        //    cEncryptDecrypt oEncryptDecrypt = new cEncryptDecrypt("2");
        //    try
        //    {
        //        oSql = new StringBuilder();
        //        oDB = new cDatabase();
        //        odtTmp = new DataTable();
                
        //        // GET ข้อมูล Transaction API Type 4
        //        oSql.AppendLine("SELECT TOP 1 FTApiCode, ISNULL(FTApiURL,'') AS FTApiURL , ISNULL(FTApiLoginUsr,'') AS FTApiLoginUsr, ISNULL(FTApiLoginPwd,'') AS FTApiLoginPwd ");
        //        oSql.AppendLine("FROM TCNMTxnAPI WITH(NOLOCK)");
        //        oSql.AppendLine("WHERE FTApiTxnType = 4");
        //        oSql.AppendLine("AND FTApiCode = '"+ ptApiCode + "'");
                
        //        odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

        //        // ตรวจสอบมีข้อมูล  Transaction API Type 4 ?
        //        if (odtTmp != null && odtTmp.Rows.Count > 0)
        //        {
        //            tUsername = odtTmp.Rows[0].Field<string>("FTApiLoginUsr");
        //            //tPassword = odtTmp.Rows[0].Field<string>("FTApiLoginPwd");
        //            if (!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTApiLoginPwd")))
        //            {
        //                tPassword = oEncryptDecrypt.C_CALtDecrypt(odtTmp.Rows[0].Field<string>("FTApiLoginPwd")); //*Arm 63-08-07
        //            }
        //            tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
        //        }
        //        else
        //        {
        //            // มีข้อมูล  Transaction API Type 4 ให้ Select จาก Type 3
        //            odtTmp = null;
        //            odtTmp = new DataTable();
        //            oSql.Clear();
        //            oSql.AppendLine("SELECT TOP 1 FTApiCode, ISNULL(FTApiURL,'') AS FTApiURL , ISNULL(FTApiLoginUsr,'') AS FTApiLoginUsr, ISNULL(FTApiLoginPwd,'') AS FTApiLoginPwd ");
        //            oSql.AppendLine("FROM TCNMTxnAPI WITH(NOLOCK)");
        //            oSql.AppendLine("WHERE FTApiTxnType = 3");
        //            oSql.AppendLine("AND FTApiCode = '" + ptApiCode + "'");
        //            odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
        //            if (odtTmp != null && odtTmp.Rows.Count > 0)
        //            {
        //                tUsername = odtTmp.Rows[0].Field<string>("FTApiLoginUsr");
        //                //tPassword = odtTmp.Rows[0].Field<string>("FTApiLoginPwd");
        //                if (!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTApiLoginPwd")))
        //                {
        //                    tPassword = oEncryptDecrypt.C_CALtDecrypt(odtTmp.Rows[0].Field<string>("FTApiLoginPwd")); //*Arm 63-08-07
        //                }
        //                tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
        //            }
        //        }

                
        //        if (odtTmp != null && odtTmp.Rows.Count > 0)
        //        {
        //            if (!string.IsNullOrEmpty(tUsername) && !string.IsNullOrEmpty(tPassword))
        //            {
        //                tAth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(tUsername + ":" + tPassword)));
                        
        //            }
        //            else
        //            {
        //                // ถ้าไม่มี User name และ Password 
        //                odtTmp = null;
        //                odtTmp = new DataTable();
        //                // หาข้อมูลจาก TCNMTxnSpcAPI
        //                oSql.Clear();
        //                oSql.AppendLine("SELECT ISNULL(FTApiURL,'') AS FTApiURL , ISNULL(FTSpaUsrCode,'') AS FTSpaUsrCode, ISNULL(FTSpaUsrPwd,'') AS FTSpaUsrPwd ");
        //                oSql.AppendLine("FROM TCNMTxnSpcAPI ");
        //                oSql.AppendLine("WHERE ISNULL(FTApiCode, '') = '" + ptApiCode + "' ");
        //                oSql.AppendLine("AND(ISNULL(FTCmpCode, '') = '' OR ISNULL(FTCmpCode, '') = '" + cVB.tVB_CmpCode + "') ");
        //                oSql.AppendLine("AND(ISNULL(FTAgnCode, '') = '' OR ISNULL(FTAgnCode, '') = '" + cVB.tVB_AgnCode + "') ");
        //                oSql.AppendLine("AND(ISNULL(FTBchCode, '') = '' OR ISNULL(FTBchCode, '') = '" + cVB.tVB_BchCode + "') ");
        //                oSql.AppendLine("AND(ISNULL(FTMerCode, '') = '' OR ISNULL(FTMerCode, '') = '" + cVB.tVB_Merchart + "') ");
        //                oSql.AppendLine("AND(ISNULL(FTShpCode, '') = '' OR ISNULL(FTShpCode, '') = '" + cVB.tVB_ShpCode + "') ");
        //                oSql.AppendLine("AND(ISNULL(FTPosCode, '') = '' OR ISNULL(FTPosCode, '') = '" + cVB.tVB_PosCode + "') ");

        //                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
        //                if (odtTmp != null && odtTmp.Rows.Count > 0)
        //                {
        //                    tUsername = odtTmp.Rows[0].Field<string>("FTSpaUsrCode");
        //                    //tPassword = odtTmp.Rows[0].Field<string>("FTSpaUsrPwd");
        //                    if (!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTSpaUsrPwd")))
        //                    {
        //                        tPassword = oEncryptDecrypt.C_CALtDecrypt(odtTmp.Rows[0].Field<string>("FTSpaUsrPwd")); //*Arm 63-08-07

        //                    }
        //                    tAth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(tUsername + ":" + tPassword)));
        //                    tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
                            
        //                }
        //            }

        //            tResult = tAth + "," + tUrl;
        //        }
                
        //    }
        //    catch(Exception oEx)
        //    {
        //        new cLog().C_WRTxLog("cConfig", "C_GETxCfgApiKADS : " + oEx.Message);
        //    }
        //    finally
        //    {
        //        ptApiCode = "";
        //        oSql = null;
        //        oDB = null;
        //        odtTmp = null;
        //    }
        //    return tResult;
        //}

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
                //oSql.AppendLine("AND CFG.FTSysCode = 'tCN_API2PSMaster'"); //*Arm 63-06-26
                oSql.AppendLine("AND CFG.FTSysCode in ('tCN_API2PSMaster','tCN_API2PSSale')"); //*Arm 63-08-06
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
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem 
            }
        }
    }
}
