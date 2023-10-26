using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Models.DatabaseTmp;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Respond.SaleOrder;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Resources;

namespace AdaPos.Class
{
    public class cVB
    {
        #region String

        public static string tVB_KbdCallByName;     // FTKbdCallByName : Call by name
        public static string tVB_UsrCode;           // FTUsrCode : User code
        public static string tVB_UsrName;           // FTUsrName : User name
        public static string tVB_KbdScreen;         // FTKbdScreen : Keyboard Screen
        public static string tVB_CstCode;           // FTCstCode : Customer Code
        public static string tVB_SaleDate;          // Openshift : Sale date
        public static string tVB_Passcode;          // Code : tCN_UsrSignInType, Key : Password, Seq : 3
        public static string tVB_RolCode;           // FTRolCode : Role code, User
        public static string tVB_RteCode;           // FTRteCode : Rate code
        public static string tVB_OpenShiftHD;       // Open shift HD
        public static string tVB_OpenShiftDT;       // Open shift DT
        public static string tVB_PosCode;           // FTPosCode : Pos code
        public static string tVB_PosStaShift;       // FTPosStaShift : Pos status shift
        public static string tVB_PosRegNo;          // FTPosRegNo : Pos Register No.
        public static string tVB_SmgCode;           // FTSmgCode : Slip Msg Code
        public static string tVB_BchCode;           // FTBchCode : Branch code
        public static string tVB_BchRefID;          // FTBchRefID : Branch Ref ID
        public static string tVB_BchName;           // FTBchName : Bracnch Name
        public static string tVB_ShfCode;           // FTShfCode : Shift code
        public static string tVB_ShpCode;           // FTShpCode : Shop code
        public static string tVB_ShpName;           // FTShpName : Shop Name
        public static string tVB_EvnCode;           // FTEvnCode : Event code
        public static string tVB_CardNo;            // Wristband / Card No.
        public static string tVB_CmpCode;           // FTCmpCode : Company Code
        public static string tVB_CmpName;           // FTCmpName : Company Name
        public static string tVB_WahCode;           // FTWahCode : WaHouse Code
        public static string tVB_PosType;           // FTPosType : Pos Type (1 : Store, 2 : Cashier)
        public static string tVB_StoreBack;         // Store Back
        public static string tVB_API2PSMaster;      // API2PSMaster
        public static string tVB_API2FNWallet;      // API2FNWallet
        public static string tVB_API2FNWalletHQ;    // API2FNWallet     //*Em 62-12-24
        public static string tVB_API2PSSale;        // API2PSSale
        public static string tVB_SgnRPosSrv;        // Signal R Pos Server
        public static string tVB_AgnKeyAPI;         // Agency API Key
        public static string tVB_APIHeader;         // Header API
        public static string tVB_SgnAIBoardcast = "C_SETxBroadcast";    // Function Name : Boardcast Signal R
        public static string tVB_SgnAISend = "C_SETxSendData";          // Function Name : Send data Signal R
        public static string tVB_RateCode;          // สกุลเงิน
        public static string tVB_RateType;          // สกุลเงิน
        public static string tVB_DocNo;             // เลขที่เอกกสาร
        public static string tVB_PrnConn;           // IP,Mac address,Port      //*Em 62-01-04
        public static string tVB_VATInOrEx;         //Include VAT or Exclude VAT    1:Inclusive(Def), 2:Exclusive
        public static string tVB_DptCode;           //Department
        public static string tVB_VatCode;           //Vat Code
        public static string tVB_RefDocNo;           //Refer document number
        public static string tVB_CstTel;            //Telephon number for customer     
        public static string tVB_CstName;            //Name for customer  
        public static string tVB_ParcelCode;        //รหัสพัสดุ
        public static string tVB_PriceGroup;        //กลุ่มราคา
        public static string tVB_BchPriceGroup;     //กลุ่มราคา สาขา
        public static string tVB_PdtCodeSrv;        //รหัสสินค้าบริการ
        public static string tVB_Merchart;          //FTMerCode
        public static string tVB_CstDef;            //ลูกค้าทั่วไป
        public static string tVB_CstDefName;        //ชื่อลูกค้าทั่วไป
        public static string tVB_MQ_KEY = "Sk$4dF#z"; //สำหรับถอดรหัส MQ
        public static string tVB_TaxID;             //เลขประจำตัวผู้เสียภาษี
        public static string tVB_MerName;           //ชื่อกลุ่มธุรกิจ     //*Em 62-10-04
        public static string tVB_QMemMsgID;         //Queue Member MsgID     //*Arm 62-10-27
        public static string tVB_PathLogo;          //ตำแหน่งรูป Logo สำหรับพิมพ์บนสลิป
        public static string tVB_HQBchCode;         //รหัสสาขาสำนักงานใหญ่
        public static string tVB_DisChgTxt;
        public static string tVB_API2ARDoc;         //API2ARDoc     //*Arm 63-02-19
        public static string tVB_CstStaAlwPosCalSo="1";  //FTCstStaAlwPosCalSo อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต (default) //*Arm 63-02-20
        public static string tVB_MemCgpCode;           //*Arm 63-03-30 Company กลุ่มบริษัท Member
        public static string tVB_MemBchCode;        //*Arm 63-03-30 รหัสสาขา Member
        public static string tVB_MemberCard;        //*Arm 63-04-03 เลขที่บัตรสมาชิก
        public static string tVB_ExpiredDate;        //*Arm 63-04-03 วันหมดอายุบัตร 
        public static string tVB_CstCardID;        //*Arm 63-04-04 เลขที่บัตรประชาชน

        public static string tVB_CardBin; // เก็บค่า 6 ตัวหน้า หรือ 4 ตัวหน้าบัตรเครดิต
        //*Em 63-01-09
        public static string tVB_BCHMQHost;
        public static string tVB_BCHMQUsr;
        public static string tVB_BCHMQPwd;
        public static string tVB_BCHMQVirtual;
        public static string tVB_BCHMQPort;
        public static string tVB_HQMQHost;
        public static string tVB_HQMQUsr;
        public static string tVB_HQMQPwd;
        public static string tVB_HQMQVirtual;
        public static string tVB_HQMQPort;
        //++++++++++++++++++++++
        public static string tVB_CNStaPrnTax;       //1:กระดาษ Thermal  2:กระดาษ A4 //*Em 63-02-20
        public static string tVB_CNTaxPrnDriver;    //ไดรเวอร์เครื่องพิมพ์       //*Em 63-02-20
        public static string tVB_DocNoPrn;  //*Em 63-03-02

        //*Arm 63-03-17 Connect RabbitMQ AdaMember(Center)
        public static string tVB_MemberMQHost;
        public static string tVB_MemberMQUsr;
        public static string tVB_MemberMQPwd;
        public static string tVB_MemberMQVirtual;
        public static string tVB_MemberMQPort;
        //+++++++++++++++++++++

        public static string tVB_SaleOrg;       //*Arm 63-05-18  Sales Organization(SKC)
        public static string tVB_APIKADS;       //*Arm 63-05-18  AIP KADS (SKC)
        #endregion End String

        #region int

        public static int nVB_Language;
        public static int nVB_DecShow = 2;
        public static int nVB_DecSave = 2;
        public static int nVB_MaxLenPasscode;       // Code : tCN_UsrSignInType, Key : Password, Seq : 2
        public static int nVB_AddressTypeCst;       // Address type customer : 1. แสดงที่อยู่แบบแยกรายละเอียด, 2. แสดงที่อยู่แบบรวม
        public static int nVB_ReferBy;              // 0 : No Refer to bill, 1 : Wristband, 2 : Document
        public static int nVB_ShfSeq;               // Shift Sequence no
        public static int nVB_SettingFrom;          // 0:Start Program, 1:Splash Screen, 2:Home
        public static int nVB_PaperSize;            // Paper Size 1 : Small, 2 : Big
        public static int nVB_MaxData;              // Max data limit
        public static int nVB_DisplayOrder;         // Order Display Format
        public static int nVB_CNTheme;              // Theme 0:Custom 1: Green 2:Orange 3:Sky 4:Brown 5:Pink
        public static int nVB_PrnType;              // 1:TCP/IP 2:Bluetooth 3.Driver        //*Em 62-01-04
        public static int nVB_TxnOffline;           // [Pong][2019-01-22][จำนวนครั้งที่ใช้งาน offline จาก บัตร/wristband]  
        public static int nVB_QRTimeout;            // อายุของ QRCode หน่วย : นาที
        public static int nVB_RolLevel;             // ระดับการใช้งาน
        public static int nVB_ChkShowBarQR;         // 1 : แสดง Barcode, 2 : แสดง QRcode, 3 : แสดงทั้ง Barcode และ QRcode
        public static int nVB_SyncSuccess = 0;      // นับจำนวนรายชื่อ Sync สำเร็จ
        public static int nVB_SyncError = 0;        // นับจำนวนรายชื่อ Sync ที่ไม่สำเร็จ
        public static int nVB_PdtPerPage = 12;      // จำนวนรายการสินค้าต่อหน้า
        public static int nVB_GrpPerPage = 5;       // จำนวนกลุ่มต่อหน้า
        public static int nVB_MenuPerPage = 5;      // จำนวนเมนูต่อหน้า
        public static int nVB_ReturnType;           // Option การคืน 1;คืนได้ครังเดียว เต็มบิลเท่านัน,2:คืนได้ครังเดียว บางรายการได้ ,3:คืนได้หลายครัง ตรวจสอบจํานวน ,4:คืนได้หลายครัง ไม่ตรวจสอบจํานวน ,5:ห้ามคืน
        public static int nVB_PrnRefundMaster;            // จำนวนการพิมพ์ต้นฉบับ บิลคืน
        public static int nVB_PrnRefundCopy;         // จำนวนการพิมพ์ต้นสำเนา บิลคืน
        public static int nVB_PrnSlipMaster;            // จำนวนการพิมพ์ต้นฉบับ บิลขาย
        public static int nVB_PrnSlipCopy;         // จำนวนการพิมพ์ต้นสำเนา บิลขาย
        public static int nVB_PrnTaxMaster;               // จำนวนการพิมพ์ต้นฉบับ ใบกำกับภาษี
        public static int nVB_PrnTaxCopy;            // จำนวนการพิมพ์ต้นสำเนา ใบกำกับภาษี
        public static int nVB_CstPoint;             // แต้มสะสม  //*Arm 63-03-09
        public static int nVB_CstPiontB4Used;       // แต้มสะสมก่อนใช้  //*Arm 63-03-16       
        public static int nVB_ChkShowPdtBarCode;    // *Arm 63-04-13  พิมพ์รหัสสินค้าหรือบาร์โค้ดในแสดงสลิป 0:ไม่แสดง, 1:แสดง Product Code, 2:แสดง Barcode 
        public static int nVB_CstPiontB4UsedPrn;    // แต้มสะสมก่อนใช้ Print   //*Arm 63-04-29
        public static int nVB_StartY;               // *Arm 63-05-03

        //*Zen 63-01-29
        public static int nVB_Check2nd;
        public static int nVB_Setting2nd; // check : 0 ไม่โชว์หน้า 2 : 1 โชว์หน้า 2

        public static int nVB_SaleModeStd; // check : 1 Standard Mode , 2 Touch Mode
        public static int nPS_StaSumPdt;            // จำนวนสินค้าหน้าจอขาย 1:แยกรายการ 2:รวมรายการ
        public static int nVB_StaSumScan;           // *Arm 63-05-05 สถานะรวมรายการสินค้าตอนสแกน 1:อนุญาต 2: ไม่อนุญาต
        public static int nVB_StaSumPrn;            // *Arm 63-05-05 สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต
        //public static int nVB_SaleModeStd;      //โหมดการแสดงผลหน้าจอขาย ยกมาจาก baseline

        public static int nVB_TimeClearLog; // ตัวแปรที่ใช้ในการเก็บ Log คิดเป็น วัน
        #endregion End int

        #region double / float

        public static decimal cVB_Amount;            // Amount for payment
        public static decimal cVB_Rate;              // สกุลเงิน
        public static decimal cVB_Available;         // [Pong][2019-01-22][ยอดเงินใช้ได้จาก บัตร/wristband]
        public static decimal cVB_VatRate;          //Vat Rate
        public static decimal cVB_Change;           // เงินทอน
        public static decimal cVB_QRPayAmt;         //ยอดชำระจากคิวอาร์โค้ด
        public static decimal cVB_RoundDiff;        //ยอดปัดเศษ      //*Em 62-10-08
        public static decimal cVB_MaxChg;           //เงินทอนสูงสุด
        public static decimal cVB_SmallBill;        //ยอดต่ำสุดหลังหักส่วนลดลูกค้า
        public static decimal cVB_PntOptBuyAmt;     //เงื่อนไข อัตราส่วนมูลค่าซื้อ  //*Arm 63-03-09
        public static decimal cVB_PntOptGetQty;      //เงื่อนไข อัตราแต้มที่จะได้   //*Arm 63-03-09
        public static decimal cVB_PriceAfEditQty;   //ราคาสินค้า หลังจากแก้ไขจำนวน

        public static Nullable<decimal> cVB_MonitorSalGrand;  //มอนิเตอร์ยอดขายต่อบิล

        #endregion End double / float

        #region Boolean

        public static bool bVB_StaLockScr;
        public static bool bVB_SplashScreen;        // Check open form splash screen
        public static bool bVB_ChkPrint;            // ตรวจสอบว่าให้เปิดการพิมพ์ไหม
        public static bool bVB_PSysRnd;             // อนุญาตให้ปัดเศษ
        public static bool bVB_ModeScan;            // Mode Scan
        public static bool bVB_ReadWriteWB;         // Read Wrist Wristband
        public static bool bVB_PrnQRCode;           //     
        public static bool bVB_ScanQR;              //มาจากการ ScanQRCode
        public static bool bVB_PrnShiftRCV;         // พิมพ์ใบสรุปยอด
        public static bool bVB_PrnShiftBNK;         // พิมพ์ใบนำส่งเงิน
        public static bool bVB_PrnShiftSUM;         // พิมพ์ใบปิดรอบ
        public static bool bVB_AlwPrnVoid;          // พิมพ์รายการ Void ในสลิป
        public static bool bVB_PrnTaxInvoiceCopy;   // ตรวจสอบการพิมพ์สำเนาของใบกำกับภาษี

        public static bool bVB_AlwShw2Screen;       // อนุญาตแสดงหน้าจอโฆษณา
        public static bool bVB_Flag;                // *Arm 63-05-18 - สถาณะอนุญาตให้ใช้ e Couponc หรือ Loyalty หรือไม่ --> true:อนุญาติ, False:ไม่อนุญาตให้ใช้
        // Edit By Zen 2020-02-19
        public static bool bVB_PrnShiftRCVRef;      // ตรวจสอบ พิมพ์ใบสรุปยอด null ไม่ต้องตัวยอดขาย | 1 ตรวจสอบยอดขาย | 2 ไม่ต้องตรวจสอบยอดขาย
        public static bool bVB_PrnShiftBNKRef;      // ตรวจสอบ พิมพ์ใบนำส่งเงิน null ไม่ต้องตัวยอดขาย | 1 ตรวจสอบยอดขาย | 2 ไม่ต้องตรวจสอบยอดขาย
        //public static bool bVB_PrnShiftSUMRef;    // ตรวจสอบ พิมพ์ใบปิดรอบ null ไม่ต้องตัวยอดขาย | 1 ตรวจสอบยอดขาย | 2 ไม่ต้องตรวจสอบยอดขาย
        public static bool bVB_RetriveBill;         //*Em 63-04-25
        public static bool bVB_PriceConfirm;        // ยืนยันราคาใหม่      //*Em 63-05-06
        public static bool bVB_RefundFullBill;      //*Em 63-05-07
        public static bool bVB_RefundTrans;         //*Arm 63-05-20 ดึงข้อมูลจากตาราง  True: ดึงข้อมูลจากตารางการขายจริง, False:ดึงข้อมูลการขายจาก ตาราง Temp
        public static bool bVB_RefundDataFrom;      //*Arm 63-06-01 ที่มาข้อมูล True: ข้อมูลในเครื่อง, False: ข้อมูลการขายมาจากหลังบ้านโดย Call API2ARDoc

        public static bool bVB_AlwSaleChkStk;           //อนุญาตให้การขายตรวจสอบสต๊อกสินค้าก่อน Net 63-05-13
        #endregion End Boolean

        #region Datetime
        public static DateTime dVB_QRDateChkExpire;   //วันที่จาก QR Code
        public static DateTime dVB_TimeStamp;         //mark วันที่และแวลา เพิ่มเทส
        #endregion Datetime
        #region List / Array

        public static List<Font> aoVB_PInvLayout;               // Set font for print
        public static List<cmlTCNMPosHW> aoVB_PosHW;            // Pos HW
        public static List<cmlTFNMRateUnit> aoVB_RateUnit;      // Rate Unit        //*Em 62-10-08
        public static List<cmlPdtOrder> aoVB_PdtReferSO;        // Product จาก SO //*Arm 63-02-19

        public static string[] atVB_PmtRefund; //*Em 63-04-17
        public static int[] anVB_PdtSeqNoRefund;  //*Em 63-05-07
        #endregion End List / Array

        #region object

        public static Color oVB_ColNormal;
        public static Color oVB_ColDark;
        public static Color oVB_ColLight;
        public static ResourceManager oVB_GBResource;
        public static wSale oVB_Sale;
        public static SqlConnection oVB_ConnDB;
        public static cmlConfigDB oVB_Config;
        public static cmlTCNMRsn oVB_Reason;
        public static wPayment oVB_Payment;
        //public static wTaxInvoice oVB_TaxInvoice;   //*Em 62-08-08
        public static wTax oVB_TaxInvoice;   //*Em 63-02-28
        public static cmlCstCard oVB_CstCard; //*Net 63-03-31

        public static List<cmlPdtOrder> aoVB_PdtRefund; //*Em 62-08-15
        public static List<cmlTPSTSalHDDis> aoVB_PdtHDDisRefund; //*Arm 63-04-03
        public static List<cmlPdtDisChg> aoVB_PdtDisChgRefund; //*Arm 63-03-20
        public static List<cmlPdtRedeem> aoVB_PdtRdDocType1; //*Arm 63-03-20
        public static List<cmlPdtRedeem> aoVB_PdtRdDocType2; //*Arm 63-03-20

        public static List<cmlPdtDisChg> aoVB_PdtDisChgCoupon; //*Net 63-03-23

        public static cmlRabbitMQConfig oVB_RabbitMQConfig;

        public static cRabbitMQ oVB_MQ;
        public static cRabbitMQ oVB_MQ_Member; //*Arm 62-10-25
        public static cmlPdtOrder oVB_PdtOrder;
        public static int oVB_OrderRowIndex;
        

        //*Em 63-01-06  Coupon
        public static ConnectionFactory oVB_MQFactory;
        public static IConnection oVB_MQConn;
        public static IModel oVB_MQModel;
        public static EventingBasicConsumer oVB_MQEvent;
        public static cRabbitMQ oVB_Coupon;

        //*Zen 63-01-26
        public static wShw2ndScreen oVB_2ndScreen;
        public static cmlTCNMTaxAddress oVB_TaxAddr;    //*Em 63-02-29

        //SO
        public static cmlDataOrdersByDoc oVB_ReferSO; //*Arm 6-03-05
        //Promotion
        public static wPmtGetorSug oVB_PmtGetorSug; //*Arm 6-03-05

        public static DataTable oVB_GetPmt;
        public static DataTable oVB_PmtSug;

        //*Em 63-04-25
        public static List<cmlTPSMFunc> oVB_PayType;
        public static List<cmlTPSMFunc> oVB_PayMenuBar;
        public static List<cmlTFNMBankNote> oVB_QuickAmt;
        public static List<cmlTCNMSlipMsgDTTmp_L> oVB_SlipMsg;  //*Em 63-05-16
        #endregion End object 

    }
}
