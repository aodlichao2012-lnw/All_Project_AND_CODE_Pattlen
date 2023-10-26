using AdaPos.Forms;
using AdaPos.Models.Database;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarcodeLib;
using ZXing.QrCode;
using ZXing;
using ZXing.Common;
using AdaPos.Popup.wPayment;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using AdaPos.Models.Webservice.Required.Customer;
using System.Net.Http;
using AdaPos.Models.Webservice.Respond.Customer;
using System.Data.SqlClient;
using AdaPos.Models.Webservice.Respond.SaleOrder;
using AdaPos.Models.DatabaseTmp;
using System.Reflection;
using AdaPos.Models.Webservice.Respond.Base;
using AdaPos.Models.Webservice.Required.SaleDocRefer;
using AdaPos.Models.Webservice.Respond.KADS.Customer;
using AdaPos.Popup.All;
using AdaPos.Models.Webservice.Required.KADS.Customer;
using AdaPos.Popup.wSale;

namespace AdaPos.Class
{
    public class cSale
    {
        #region Variable
        public static string tC_TblSalHD;
        public static string tC_TblSalHDCst;
        public static string tC_TblSalHDDis;
        public static string tC_TblSalDT;
        public static string tC_TblSalDTDis;
        public static string tC_TblSalDTPmt;
        public static string tC_TblSalDTPmtHis;
        public static string tC_TblSalRC;
        public static string tC_TblSalRD;   //*Arm 63-03-12
        public static string tC_TblSalPD;   //*Zen 63-03-24
        public static string tC_TblRefund;  //*Em 63-05-14
        public static string tC_TblTxnSal;  //*Net 63-06-01
        public static string tC_TblTxnRD;  //*Net 63-06-01
        public static string tC_SaleDocNum;
        public static string tC_TxnRefCode;     //*Arm 63-03-31
        //Format
        public static string tC_DocFmtLeft;
        public static string tC_DocFmtChr;      //char
        public static string tC_DocFmtBch;      //Branch
        public static string tC_DocFmtPosShp;   //Pos/Shop
        public static string tC_DocFmtYear;     //Year
        public static string tC_DocFmtMonth;    //Month
        public static string tC_DocFmtDay;      //Day
        public static string tC_DocFmtSep;      //Sep
        public static string tC_DocFmtNum;      //Num
        public static string tC_RefDocNoPrn;
        public static string tC_DocFmtLeftSal;  //*Em 63-05-15
        public static string tC_DocFmtLeftRfn;  //*Em 63-05-15

        //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูลสำหรับ บิลขายอ้างบิลคืน บิลคืนอ้างบิลขาย
        public static string tC_Ref_TblSalHD;       //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblSalHDDis;    //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblSalHDCst;    //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblSalDT;       //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblSalDTDis;    //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblSalRC;       //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblSalRD;       //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblSalPD;       //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblTxnSale;     //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล
        public static string tC_Ref_TblTxnRedeem;   //*Arm 63-06-09 ชื่อตารางอ้างอิงข้อมูล

        public static int nC_LastDocSale;    //*Em 63-05-15
        public static int nC_LastDocRefund;  //*Em 63-05-15
        public static int nC_LastDocVoid;    //*Em 63-05-15
        public static int nC_DocSalLength;    //*Em 63-05-15
        public static int nC_DocRfnLength;    //*Em 63-05-15
        public static int nC_SaleLastNum;
        public static int nC_SaleRetLastNum;
        public static int nC_DocRuningLength;
        public static int nC_DocType;
        public static int nC_PrnDocType;
        public static int nC_DTSeqNo;
        public static int nC_CntItem;
        public static int nC_HoldNo;
        public static int nC_RDSeqNo;             //*Arm 62-03-12

        public static decimal cC_DTQty; //*Arm 62-10-03
        public static decimal cCstPiontB4Used = 0; //*Arm 63-05-09
        public static decimal cRefundRed = 0;
        public static decimal cC_QuotaLimit;    //*Em 63-07-16
        public static decimal cC_QtyLimit;      //*Em 63-09-16
        public static bool bC_PrnCopy;          //*Net 63-02-25 ตรวจสอบการพิมพ์สำเนา
        public static bool bC_SetComplete;      //*Em 63-04-22

        public static cmlTPSTSalHD oC_SalHD;

        public static Thread oUpload;
        public static Thread oPrn;
        public static Thread oNewDoc;
        public static Thread oTxnSale;      //*Arm 63-05-24
        public static Thread oTxnRfnSale;      //*Arm 63-05-24

        public static DataSet oDbSetPri;
        public static DataTable oDbTblHD = new DataTable();
        public static DataTable oDbTblHDDis = new DataTable();
        public static DataTable oDbTblRD = new DataTable();
        public static DataTable oDbTblQty = new DataTable();
        public static DataTable oDbTblRC = new DataTable();
        public static DataTable oDbTblHDCst = new DataTable();
        public static DataTable oDbTblMem = new DataTable();
        public static DataTable oDbTblMemTxnRed = new DataTable();
        public static DataTable oDbTblMemRD = new DataTable();
        public static DataTable oDbTblPD2 = new DataTable();
        public static DataTable oDbTblPDCpn = new DataTable();

        #endregion

        #region Function


        public void C_Initial()
        {
            C_PRCxCheckAndCreateTableTemp();

        }

        /// <summary>
        /// Check and create table template.
        /// </summary>
        public void C_PRCxCheckAndCreateTableTemp()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();

            try
            {
                tC_TblSalHD = "TSHD" + cVB.tVB_PosCode;
                tC_TblSalHDCst = "TSHDCst" + cVB.tVB_PosCode;
                tC_TblSalHDDis = "TSHDDis" + cVB.tVB_PosCode;
                tC_TblSalDT = "TSDT" + cVB.tVB_PosCode;
                tC_TblSalDTDis = "TSDTDis" + cVB.tVB_PosCode;
                tC_TblSalDTPmt = "TSDTPmt" + cVB.tVB_PosCode;
                tC_TblSalDTPmtHis = "TSDTPmtHis" + cVB.tVB_PosCode;
                tC_TblSalRC = "TSRC" + cVB.tVB_PosCode;
                tC_TblSalRD = "TSRD" + cVB.tVB_PosCode;     // *Arm 63-03-12
                tC_TblSalPD = "TSPD" + cVB.tVB_PosCode;     // *Arm 63-03-12
                tC_TblRefund = "TSRF" + cVB.tVB_PosCode;    //*Em 63-05-14
                tC_TblTxnSal = "TSHDPsl" + cVB.tVB_PosCode;    //*Net 63-06-01 Tmp ตาราง MemTxnSal
                tC_TblTxnRD = "TSHDPrd" + cVB.tVB_PosCode;    //*Net 63-06-01 Tmp ตาราง MemTxnRedeem

                //*Net 63-07-30 ยกมาจาก Moshi
                C_PRCxChekAndCreateFileGroup(); //*Em 63-07-11

                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("BEGIN TRY");
                //HD
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalHD + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalHD + " FROM [dbo].[TPSTSalHD] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblSalHD + " ADD PRIMARY KEY(FTBchCode,FTXshDocNo) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHD') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalHD + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalHD + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalHD + " FROM TPSTSalHD WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");
                oSql.AppendLine("");

                //HDCst
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalHDCst + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalHDCst + " FROM [dbo].[TPSTSalHDCst] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblSalHDCst + " ADD PRIMARY KEY(FTBchCode,FTXshDocNo) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDCst') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalHDCst + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalHDCst + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalHDCst + " FROM TPSTSalHDCst WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");
                oSql.AppendLine("");

                //HDDis
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalHDDis + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalHDDis + " FROM [dbo].[TPSTSalHDDis] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblSalHDDis + " ADD PRIMARY KEY(FTBchCode,FTXshDocNo,FDXhdDateIns) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDDis') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalHDDis + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalHDDis + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalHDDis + " FROM TPSTSalHDDis WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");
                oSql.AppendLine("");

                //DT
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalDT + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalDT + " FROM [dbo].[TPSTSalDT] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblSalDT + " ADD PRIMARY KEY(FTBchCode,FTXshDocNo,FNXsdSeqNo) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDT') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalDT + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalDT + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDT + " FROM TPSTSalDT WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");
                oSql.AppendLine("");

                //DTDis
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalDTDis + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalDTDis + " FROM [dbo].[TPSTSalDTDis] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblSalDTDis + " ADD PRIMARY KEY(FTBchCode,FTXshDocNo,FNXsdSeqNo,FDXddDateIns) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTDis') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalDTDis + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalDTDis + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDTDis + " FROM TPSTSalDTDis WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");
                oSql.AppendLine("");

                //*Net 63-07-07 ไม่ใช้ตารางแล้ว ยกมาจาก Moshi
                //DTPmt
                //oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalDTPmt + "', 'U') IS NULL BEGIN");
                //oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalDTPmt + " FROM [dbo].[TPSTSalDTPmt] WITH(NOLOCK)");
                //oSql.AppendLine("       END");
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTPmt') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalDTPmt + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalDTPmt + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDTPmt + " FROM TPSTSalDTPmt WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");
                //oSql.AppendLine("");

                //RC
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalRC + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO " + tC_TblSalRC + " FROM [dbo].[TPSTSalRC] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblSalRC + " ADD PRIMARY KEY(FTBchCode,FTXshDocNo,FNXrcSeqNo) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalRC') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalRC + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalRC + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalRC + " FROM TPSTSalRC WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");

                //RD    //*Arm 63-03-12
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalRD + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO " + tC_TblSalRD + " FROM [dbo].[TPSTSalRD] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblSalRD + " ADD PRIMARY KEY(FTBchCode,FTXshDocNo,FNXrdSeqNo) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalRD') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalRD + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalRD + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalRD + " FROM TPSTSalRD WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");

                //PD    //*Zen 63-03-24
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalPD + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO " + tC_TblSalPD + " FROM [dbo].[TPSTSalPD] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblSalPD + " ADD PRIMARY KEY(FTBchCode,FTXshDocNo,FTPmhDocNo,FNXsdSeqNo,FTPmdGrpName) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalPD') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalPD + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblSalPD + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalPD + " FROM TPSTSalPD WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");

                //Refund    //*Em 63-05-14
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblRefund + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("       CREATE TABLE [dbo].[" + tC_TblRefund + "](");
                oSql.AppendLine("       	[FNXsdSeqNo] [int] NOT NULL,");
                oSql.AppendLine("       	[FNXsdSeqNoOld] [int] NOT NULL,");
                oSql.AppendLine("       	[FCXsdQty] [numeric](18, 2) NULL,");
                oSql.AppendLine("       	[FCXsdQtyRfn] [numeric](18, 2) NULL");
                oSql.AppendLine("       ) ON [PRIMARY]");
                oSql.AppendLine("   END");
                //+++++++++++++++++++++++++

                //MemTxnSal    //*Net 63-06-01
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblTxnSal + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO " + tC_TblTxnSal + " FROM [dbo].[TCNTMemTxnSale] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblTxnSal + " ADD PRIMARY KEY(FTCgpCode,FTMemCode,FTTxnRefDoc,FTTxnRefInt,FTTxnRefSpl) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTMemTxnSale') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblTxnSal + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblTxnSal + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblTxnSal + " FROM TCNTMemTxnSale WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");

                //MemTxnRedeem    //*Net 63-06-01
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblTxnRD + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO " + tC_TblTxnRD + " FROM [dbo].[TCNTMemTxnRedeem] WITH(NOLOCK)");
                //*Net 63-07-10 สร้าง primary key ยกมาจาก Moshi
                oSql.AppendLine("	       ALTER TABLE " + tC_TblTxnRD + " ADD PRIMARY KEY(FTCgpCode,FTMemCode,FTRedRefDoc) ON FG_PSSale");
                //+++++++++++++++++++++++++++++++++
                oSql.AppendLine("       END");
                //*Net 63-07-30 ย้ายการ drop ไปไว้ตอนปิดรอบ ยกมาจาก Moshi
                //oSql.AppendLine("   ELSE BEGIN");
                //oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTMemTxnRedeem') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblTxnRD + "') BEGIN");
                //oSql.AppendLine("		    DROP TABLE " + tC_TblTxnRD + "");
                //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblTxnRD + " FROM TCNTMemTxnRedeem WITH(NOLOCK)");
                //oSql.AppendLine("	    END");
                //oSql.AppendLine("   END");

                oSql.AppendLine("   COMMIT");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   ROLLBACK");
                oSql.AppendLine("END CATCH");

                oDB.C_SETxDataQuery(oSql.ToString());

                //*Em 63-05-25
                oSql.Clear();
                //TPSTSalHD
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTBchCode')  DROP INDEX IND_" + tC_TblSalHD + "_FTBchCode ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTXshDocNo')  DROP INDEX IND_" + tC_TblSalHD + "_FTXshDocNo ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTShpCode')  DROP INDEX IND_" + tC_TblSalHD + "_FTShpCode ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FNXshDocType')  DROP INDEX IND_" + tC_TblSalHD + "_FNXshDocType ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTPosCode')  DROP INDEX IND_" + tC_TblSalHD + "_FTPosCode ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTShfCode')  DROP INDEX IND_" + tC_TblSalHD + "_FTShfCode ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTUsrCode')  DROP INDEX IND_" + tC_TblSalHD + "_FTUsrCode ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTCstCode')  DROP INDEX IND_" + tC_TblSalHD + "_FTCstCode ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTXshRefExt')  DROP INDEX IND_" + tC_TblSalHD + "_FTXshRefExt ON " + tC_TblSalHD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalHD + "_FTXshRefInt')  DROP INDEX IND_" + tC_TblSalHD + "_FTXshRefInt ON " + tC_TblSalHD + ";");
                //*Net 63-07-30 ยกมาจาก Moshi
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTBchCode ON " + tC_TblSalHD + "(FTBchCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTXshDocNo ON " + tC_TblSalHD + "(FTXshDocNo);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTShpCode ON " + tC_TblSalHD + "(FTShpCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FNXshDocType ON " + tC_TblSalHD + "(FNXshDocType);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTPosCode ON " + tC_TblSalHD + "(FTPosCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTShfCode ON " + tC_TblSalHD + "(FTShfCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTUsrCode ON " + tC_TblSalHD + "(FTUsrCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTCstCode ON " + tC_TblSalHD + "(FTCstCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTXshRefExt ON " + tC_TblSalHD + "(FTXshRefExt);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTXshRefInt ON " + tC_TblSalHD + "(FTXshRefInt);");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTBchCode ON " + tC_TblSalHD + "(FTBchCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTXshDocNo ON " + tC_TblSalHD + "(FTXshDocNo) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTShpCode ON " + tC_TblSalHD + "(FTShpCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FNXshDocType ON " + tC_TblSalHD + "(FNXshDocType) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTPosCode ON " + tC_TblSalHD + "(FTPosCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTShfCode ON " + tC_TblSalHD + "(FTShfCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTUsrCode ON " + tC_TblSalHD + "(FTUsrCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTCstCode ON " + tC_TblSalHD + "(FTCstCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTXshRefExt ON " + tC_TblSalHD + "(FTXshRefExt) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalHD + "_FTXshRefInt ON " + tC_TblSalHD + "(FTXshRefInt) ON FG_PSSale;");
                //+++++++++++++++++++++++++++++++++++++++

                //DT
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTBchCode')  DROP INDEX IND_" + tC_TblSalDT + "_FTBchCode ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTXshDocNo')  DROP INDEX IND_" + tC_TblSalDT + "_FTXshDocNo ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FNXsdSeqNo')  DROP INDEX IND_" + tC_TblSalDT + "_FNXsdSeqNo ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTPdtCode')  DROP INDEX IND_" + tC_TblSalDT + "_FTPdtCode ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTPunCode')  DROP INDEX IND_" + tC_TblSalDT + "_FTPunCode ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTXsdBarCode')  DROP INDEX IND_" + tC_TblSalDT + "_FTXsdBarCode ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTXsdVatType')  DROP INDEX IND_" + tC_TblSalDT + "_FTXsdVatType ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTPplCode')  DROP INDEX IND_" + tC_TblSalDT + "_FTPplCode ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTXsdSaleType')  DROP INDEX IND_" + tC_TblSalDT + "_FTXsdSaleType ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTXsdStaPdt')  DROP INDEX IND_" + tC_TblSalDT + "_FTXsdStaPdt ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTXsdStaPrcStk')  DROP INDEX IND_" + tC_TblSalDT + "_FTXsdStaPrcStk ON " + tC_TblSalDT + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalDT + "_FTXsdStaAlwDis')  DROP INDEX IND_" + tC_TblSalDT + "_FTXsdStaAlwDis ON " + tC_TblSalDT + ";");
                //*Net 63-07-30 ยกมาจาก Moshi
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTBchCode ON " + tC_TblSalDT + "(FTBchCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXshDocNo ON " + tC_TblSalDT + "(FTXshDocNo);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FNXsdSeqNo ON " + tC_TblSalDT + "(FNXsdSeqNo);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTPdtCode ON " + tC_TblSalDT + "(FTPdtCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTPunCode ON " + tC_TblSalDT + "(FTPunCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdBarCode ON " + tC_TblSalDT + "(FTXsdBarCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdVatType ON " + tC_TblSalDT + "(FTXsdVatType);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTPplCode ON " + tC_TblSalDT + "(FTPplCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdSaleType ON " + tC_TblSalDT + "(FTXsdSaleType);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdStaPdt ON " + tC_TblSalDT + "(FTXsdStaPdt);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdStaPrcStk ON " + tC_TblSalDT + "(FTXsdStaPrcStk);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdStaAlwDis ON " + tC_TblSalDT + "(FTXsdStaAlwDis);");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTBchCode ON " + tC_TblSalDT + "(FTBchCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXshDocNo ON " + tC_TblSalDT + "(FTXshDocNo) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FNXsdSeqNo ON " + tC_TblSalDT + "(FNXsdSeqNo) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTPdtCode ON " + tC_TblSalDT + "(FTPdtCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTPunCode ON " + tC_TblSalDT + "(FTPunCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdBarCode ON " + tC_TblSalDT + "(FTXsdBarCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdVatType ON " + tC_TblSalDT + "(FTXsdVatType) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTPplCode ON " + tC_TblSalDT + "(FTPplCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdSaleType ON " + tC_TblSalDT + "(FTXsdSaleType) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdStaPdt ON " + tC_TblSalDT + "(FTXsdStaPdt) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdStaPrcStk ON " + tC_TblSalDT + "(FTXsdStaPrcStk) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalDT + "_FTXsdStaAlwDis ON " + tC_TblSalDT + "(FTXsdStaAlwDis) ON FG_PSSale;");
                //+++++++++++++++++++++++++++++++

                //RC
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalRC + "_FTBchCode')  DROP INDEX IND_" + tC_TblSalRC + "_FTBchCode ON " + tC_TblSalRC + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalRC + "_FTXshDocNo')  DROP INDEX IND_" + tC_TblSalRC + "_FTXshDocNo ON " + tC_TblSalRC + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalRC + "_FTRcvCode')  DROP INDEX IND_" + tC_TblSalRC + "_FTRcvCode ON " + tC_TblSalRC + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalRC + "_FTBnkCode')  DROP INDEX IND_" + tC_TblSalRC + "_FTBnkCode ON " + tC_TblSalRC + ";");
                //*Net 63-07-30 ยกมาจาก Moshi
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalRC + "_FTBchCode ON " + tC_TblSalRC + "(FTBchCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalRC + "_FTXshDocNo ON " + tC_TblSalRC + "(FTXshDocNo);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalRC + "_FTRcvCode ON " + tC_TblSalRC + "(FTRcvCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalRC + "_FTBnkCode ON " + tC_TblSalRC + "(FTBnkCode);");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalRC + "_FTBchCode ON " + tC_TblSalRC + "(FTBchCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalRC + "_FTXshDocNo ON " + tC_TblSalRC + "(FTXshDocNo) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalRC + "_FTRcvCode ON " + tC_TblSalRC + "(FTRcvCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalRC + "_FTBnkCode ON " + tC_TblSalRC + "(FTBnkCode) ON FG_PSSale;");
                //++++++++++++++++++++++++++++++++++++++

                //PD
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalPD + "_FTBchCode')  DROP INDEX IND_" + tC_TblSalPD + "_FTBchCode ON " + tC_TblSalPD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalPD + "_FTXshDocNo')  DROP INDEX IND_" + tC_TblSalPD + "_FTXshDocNo ON " + tC_TblSalPD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalPD + "_FNXsdSeqNo')  DROP INDEX IND_" + tC_TblSalPD + "_FNXsdSeqNo ON " + tC_TblSalPD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalPD + "_FTPmhDocNo')  DROP INDEX IND_" + tC_TblSalPD + "_FTPmhDocNo ON " + tC_TblSalPD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalPD + "_FTPmdGrpName')  DROP INDEX IND_" + tC_TblSalPD + "_FTPmdGrpName ON " + tC_TblSalPD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalPD + "_FTPdtCode')  DROP INDEX IND_" + tC_TblSalPD + "_FTPdtCode ON " + tC_TblSalPD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalPD + "_FTPunCode')  DROP INDEX IND_" + tC_TblSalPD + "_FTPunCode ON " + tC_TblSalPD + ";");
                oSql.AppendLine("IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IND_" + tC_TblSalPD + "_FTPplCode')  DROP INDEX IND_" + tC_TblSalPD + "_FTPplCode ON " + tC_TblSalPD + ";");
                //*Net 63-07-30 ยกมาจาก Moshi
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTBchCode ON " + tC_TblSalPD + "(FTBchCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTXshDocNo ON " + tC_TblSalPD + "(FTXshDocNo);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FNXsdSeqNo ON " + tC_TblSalPD + "(FNXsdSeqNo);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPmhDocNo ON " + tC_TblSalPD + "(FTPmhDocNo);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPmdGrpName ON " + tC_TblSalPD + "(FTPmdGrpName);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPdtCode ON " + tC_TblSalPD + "(FTPdtCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPunCode ON " + tC_TblSalPD + "(FTPunCode);");
                //oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPplCode ON " + tC_TblSalPD + "(FTPplCode);");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTBchCode ON " + tC_TblSalPD + "(FTBchCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTXshDocNo ON " + tC_TblSalPD + "(FTXshDocNo) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FNXsdSeqNo ON " + tC_TblSalPD + "(FNXsdSeqNo) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPmhDocNo ON " + tC_TblSalPD + "(FTPmhDocNo) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPmdGrpName ON " + tC_TblSalPD + "(FTPmdGrpName) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPdtCode ON " + tC_TblSalPD + "(FTPdtCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPunCode ON " + tC_TblSalPD + "(FTPunCode) ON FG_PSSale;");
                oSql.AppendLine("CREATE NONCLUSTERED INDEX IND_" + tC_TblSalPD + "_FTPplCode ON " + tC_TblSalPD + "(FTPplCode) ON FG_PSSale;");
                //+++++++++++++++++++++++++++++++++++++++++
                oDB.C_SETxDataQuery(oSql.ToString());
                //++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRCxCheckAndCreateTableTemp : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// '*Em 63-07-11
        /// </summary>
        public void C_PRCxChekAndCreateFileGroup()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine(@"IF NOT EXISTS(SELECT name FROM sys.filegroups WHERE name = 'FG_PSTax') BEGIN");
                oSql.AppendLine(@"	DECLARE @nIndex int");
                oSql.AppendLine(@"	DECLARE @tFullpath varchar(200)");
                oSql.AppendLine(@"	SET @tFullpath = (SELECT physical_name FROM sys.database_files where data_space_id = '1')");
                oSql.AppendLine(@"	SET @nIndex = CHARINDEX('\', REVERSE(@tFullpath))");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"	DECLARE @tDBName VARCHAR(50)");
                oSql.AppendLine(@"	DECLARE @tSql NVARCHAR(1000)");
                oSql.AppendLine(@"	SET @tDBName = DB_NAME()");
                oSql.AppendLine(@"	SET @tSql = 'ALTER DATABASE ' + @tDBName + ' ADD FILEGROUP FG_PSTax'");
                oSql.AppendLine(@"	EXECUTE sp_executesql @tSql");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"	SET @tSql = 'ALTER DATABASE ' + @tDBName + CHAR(13) +");
                //oSql.AppendLine(@"				'ADD FILE (NAME=''FG_PSTax_Data'',FILENAME = '''+ LEFT(@tFullpath, @nIndex) + 'FG_PSTax_' + @tDBName +''')' + CHAR(13) +");
                oSql.AppendLine(@"				'ADD FILE (NAME=''FG_PSTax_Data'',FILENAME = '''+ REVERSE(SUBSTRING(REVERSE(@tFullpath), @nIndex, LEN(@tFullpath))) + 'FG_PSTax_' + @tDBName +'.ndf'')' + CHAR(13) +");     //*Em 63-08-07
                oSql.AppendLine(@"				'TO FILEGROUP FG_PSTax'");
                oSql.AppendLine(@"	EXECUTE sp_executesql @tSql");
                oSql.AppendLine(@"END");
                oDB.C_SETxDataQuery(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine(@"IF NOT EXISTS(SELECT name FROM sys.filegroups WHERE name = 'FG_PSSale') BEGIN");
                oSql.AppendLine(@"	DECLARE @nIndex int");
                oSql.AppendLine(@"	DECLARE @tFullpath varchar(200)");
                oSql.AppendLine(@"	SET @tFullpath = (SELECT physical_name FROM sys.database_files where data_space_id = '1')");
                oSql.AppendLine(@"	SET @nIndex = CHARINDEX('\', REVERSE(@tFullpath))");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"	DECLARE @tDBName VARCHAR(50)");
                oSql.AppendLine(@"	DECLARE @tSql NVARCHAR(1000)");
                oSql.AppendLine(@"	SET @tDBName = DB_NAME()");
                oSql.AppendLine(@"	SET @tSql = 'ALTER DATABASE ' + @tDBName + ' ADD FILEGROUP FG_PSSale'");
                oSql.AppendLine(@"	EXECUTE sp_executesql @tSql");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"	SET @tSql = 'ALTER DATABASE ' + @tDBName + CHAR(13) +");
                //oSql.AppendLine(@"				'ADD FILE (NAME=''FG_PSSale_Data'',FILENAME = '''+ LEFT(@tFullpath, @nIndex) + 'FG_PSSale_' + @tDBName +''')' + CHAR(13) +");
                oSql.AppendLine(@"				'ADD FILE (NAME=''FG_PSSale_Data'',FILENAME = '''+ REVERSE(SUBSTRING(REVERSE(@tFullpath), @nIndex, LEN(@tFullpath))) + 'FG_PSSale_' + @tDBName +'.ndf'')' + CHAR(13) +");   //*Em 63-08-07
                oSql.AppendLine(@"				'TO FILEGROUP FG_PSSale'");
                oSql.AppendLine(@"	EXECUTE sp_executesql @tSql");
                oSql.AppendLine(@"END");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxChekAndCreateFileGroup : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// '*Em 63-07-11
        /// </summary>
        public void C_PRCxMoveTemp2FileGroup()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine(@"DECLARE @tTblName VARCHAR(50)");
                oSql.AppendLine(@"DECLARE @tColKey VARCHAR(300)");
                oSql.AppendLine(@"DECLARE @tIndexName VARCHAR(50)");
                oSql.AppendLine(@"DECLARE @tSql NVARCHAR(250)");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"DECLARE Col_Cursor CURSOR FOR");
                oSql.AppendLine(@"SELECT o.[name] AS TableName, i.[name] AS IndexName");
                oSql.AppendLine(@"FROM sys.indexes i");
                oSql.AppendLine(@"INNER JOIN sys.filegroups fg ON i.data_space_id = fg.data_space_id");
                oSql.AppendLine(@"INNER JOIN sys.all_objects o ON i.[object_id] = o.[object_id]");
                oSql.AppendLine(@"WHERE i.data_space_id = fg.data_space_id AND o.type = 'U' ");
                oSql.AppendLine(@"AND o.[name] like '%" + cVB.tVB_PosCode + "'");
                oSql.AppendLine(@"AND i.[type_desc] = 'CLUSTERED'");
                oSql.AppendLine(@"AND ISNULL(i.[name],'') <> ''");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"OPEN Col_Cursor");
                oSql.AppendLine(@"FETCH NEXT FROM Col_Cursor INTO @tTblName,@tIndexName");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"WHILE @@FETCH_STATUS = 0");
                oSql.AppendLine(@"BEGIN");
                oSql.AppendLine(@"	SET @tColKey = (SELECT COLUMN_NAME + ',' FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE");
                oSql.AppendLine(@"					WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1 AND TABLE_NAME = @tTblName");
                oSql.AppendLine(@"					FOR XML PATH(''))");
                oSql.AppendLine(@"	SET @tColKey = SUBSTRING(@tColKey,1,LEN(@tColKey)-1)");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"	SET @tSql = 'ALTER TABLE '+ @tTblName +' DROP CONSTRAINT '+ @tIndexName +' WITH (MOVE TO FG_PSSale)' ");
                oSql.AppendLine(@"	EXECUTE sp_executesql  @tsql");
                oSql.AppendLine(@"");
                oSql.AppendLine(@"	SET @tSql = 'ALTER TABLE '+ @tTblName +' ADD CONSTRAINT '+ @tIndexName +' PRIMARY KEY ('+ @tColKey +') ' ");
                oSql.AppendLine(@"	EXECUTE sp_executesql  @tsql");
                oSql.AppendLine(@"	FETCH NEXT FROM Col_Cursor INTO @tTblName,@tIndexName");
                oSql.AppendLine(@"END");
                oSql.AppendLine(@"CLOSE Col_Cursor");
                oSql.AppendLine(@"DEALLOCATE Col_Cursor");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxMoveTemp2FileGroup : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// '*Em 63-07-11
        /// </summary>
        public static void C_PRCxUpdateStatistics()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine(@"DECLARE @tSql nvarchar(2000)");
                oSql.AppendLine(@"SET @tSql = 'USE ['+ DB_NAME() +']; exec sp_updatestats'");
                oSql.AppendLine(@"EXEC sp_MSforeachdb @tSql");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxUpdateStatistics : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get Last Max Document.
        /// </summary>
        /// <param name="pnDocType">
        /// 1:Sale.
        /// 9:Return.
        /// 2:Rental sale.
        /// 3:Rental return.
        /// 5:Ticket.
        /// 6:Sale full tax.
        /// 7:Return full tax.
        /// </param>
        private static int C_GETnLastDocNum(int pnDocType)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            int nMax = 0;
            int nMaxTmp = 0;
            try
            {
                string tTblName = "";
                string tFedDocNo = "";
                string tFedDocType = "";
                string tDocLeft = "";
                string tFedStaDoc = "";

                tDocLeft = tC_DocFmtLeft;
                switch (cVB.tVB_PosType)
                {
                    case "1":   //Pos
                        switch (pnDocType)
                        {
                            case 1:  //ขาย
                            case 9: //คืน
                                tTblName = "TPSTSalHD";
                                tFedDocNo = "FTXshDocNo";
                                tFedDocType = "FNXshDocType";
                                tFedStaDoc = "FTXshStaDoc"; //*Em 63-05-25
                                break;
                            case 2: //ขายปลีก - ขายเช่า
                            case 3: //ขายปลีก - คืนเช่า
                                tTblName = "TRTTSalHD";
                                tFedDocNo = "FTXshDocNo";
                                tFedDocType = "FNXshDocType";
                                break;
                        }
                        break;
                    case "2":   //Cashier
                        switch (pnDocType)
                        {
                            case 1:  //ใบเบิกบัตร
                            case 2: //ใบคืนบัตร
                                tTblName = "TFNTCrdShiftHD";
                                tFedDocNo = "FTCshDocNo";
                                tFedDocType = "FTCshDocType";
                                break;
                            case 3: //ใบเติมเงิน
                            case 4: //ใบยกเลิกเติมเงิน
                                tTblName = "TFNTCrdTopUpHD";
                                tFedDocNo = "FTCthDocNo";
                                tFedDocType = "FTCthDocType";
                                break;
                            case 5: //ใบเปลี่ยนสถานะบัตร
                            case 6: //ใบเปลี่ยนบัตร
                                tTblName = "TFNTCrdVoidHD";
                                tFedDocNo = "FTCvhDocNo";
                                tFedDocType = "FTCvhDocType";
                                break;
                        }
                        break;
                }

                //Sale SYYMMBBBBPPP-NNNNNNN
                oSql.Clear(); //*Net 63-05-25
                oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + nC_DocRuningLength + ") AS FTMax");
                oSql.AppendLine("FROM " + tTblName + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + nC_DocRuningLength));
                oSql.AppendLine("AND " + tFedDocType + " = " + pnDocType);
                oSql.AppendLine("AND " + tFedStaDoc + " = '1'");    //*Em 63-05-25
                //oSql.AppendLine("AND FTXshStaDoc = '1'");
                nMax = oDB.C_GEToDataQuery<int>(oSql.ToString());

                oSql.Clear(); //*Net 63-05-25
                oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + nC_DocRuningLength + ") AS FTMax");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + nC_DocRuningLength));
                oSql.AppendLine("AND " + tFedDocType + " = " + pnDocType);
                oSql.AppendLine("AND " + tFedStaDoc + " = '1'");    //*Em 63-05-25
                //oSql.AppendLine("AND FTXshStaDoc = '1'");
                nMaxTmp = oDB.C_GEToDataQuery<int>(oSql.ToString());

                //*Net 63-07-30 ยกมาจาก Moshi
                //*Net 63-06-10 เช็คจากค่าใน TSysSetting ด้วย กรณี Manual เลขบิล
                switch (pnDocType)
                {
                    case 1:
                        nMax = Math.Max(nMax, cVB.nVB_LastBillS);
                        break;
                    case 9:
                        nMax = Math.Max(nMax, cVB.nVB_LastBillR);
                        break;
                }
                nMax = Math.Max(nMax, nMaxTmp);
                //if (nMaxTmp > nMax) nMax = nMaxTmp;
                //++++++++++++++++++++++++++++++++
                return nMax;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_GETxLastDocNum : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return nMax;
        }

        //public static void C_GETxFormatDoc(string ptTblName, int pnDocType, DateTime pdDate, string ptPosCode, string ptShpCode)
        public static string C_GETtFormatDoc(string ptTblName, int pnDocType, DateTime pdDate, string ptPosCode, string ptShpCode)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            DataTable odtTmp;
            string tDocFmt = "";
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT * FROM TCNTAuto WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSatTblName = '" + ptTblName + "'");
                oSql.AppendLine("AND FTSatStaDocType = '" + pnDocType + "'");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        if (odtTmp.Rows[0].Field<string>("FTSatStaDefUsage").ToString() == "1")
                        {
                            //char
                            tC_DocFmtChr = odtTmp.Rows[0].Field<string>("FTSatDefChar").ToString();
                            //Branch
                            tC_DocFmtBch = odtTmp.Rows[0].Field<string>("FTSatDefBch").ToString() == "1" ? cVB.tVB_BchCode : "";

                            //Pos/Shop
                            switch (odtTmp.Rows[0].Field<string>("FTSatDefPosShp").ToString())
                            {
                                case "1":   //Pos
                                    tC_DocFmtPosShp = ptPosCode;
                                    break;
                                case "2":   //Shop
                                    tC_DocFmtPosShp = ptShpCode;
                                    break;
                                default:
                                    tC_DocFmtPosShp = "";
                                    break;
                            }

                            //Year
                            tC_DocFmtYear = odtTmp.Rows[0].Field<string>("FTSatDefYear").ToString() == "1" ? string.Format("{0:yy}", pdDate) : "";
                            //Month
                            tC_DocFmtMonth = odtTmp.Rows[0].Field<string>("FTSatDefMonth").ToString() == "1" ? string.Format("{0:MM}", pdDate) : "";
                            //Day
                            tC_DocFmtDay = odtTmp.Rows[0].Field<string>("FTSatDefDay").ToString() == "1" ? string.Format("{0:dd}", pdDate) : "";
                            //Sep
                            tC_DocFmtSep = odtTmp.Rows[0].Field<string>("FTSatDefSep").ToString() == "1" ? "-" : "";
                            //Num
                            tC_DocFmtNum = odtTmp.Rows[0].Field<string>("FTSatDefNum").ToString();
                        }
                        else
                        {
                            //char
                            tC_DocFmtChr = odtTmp.Rows[0].Field<string>("FTSatUsrChar").ToString();
                            //Branch
                            tC_DocFmtBch = odtTmp.Rows[0].Field<string>("FTSatUsrBch").ToString() == "1" ? cVB.tVB_BchCode : "";

                            //Pos/Shop
                            switch (odtTmp.Rows[0].Field<string>("FTSatUsrPosShp").ToString())
                            {
                                case "1":   //Pos
                                    tC_DocFmtPosShp = ptPosCode;
                                    break;
                                case "2":   //Shop
                                    tC_DocFmtPosShp = ptShpCode;
                                    break;
                                default:
                                    tC_DocFmtPosShp = "";
                                    break;
                            }

                            //Year
                            tC_DocFmtYear = odtTmp.Rows[0].Field<string>("FTSatUsrYear").ToString() == "1" ? string.Format("{0:yy}", pdDate) : "";
                            //Month
                            tC_DocFmtMonth = odtTmp.Rows[0].Field<string>("FTSatUsrMonth").ToString() == "1" ? string.Format("{0:MM}", pdDate) : "";
                            //Day
                            tC_DocFmtDay = odtTmp.Rows[0].Field<string>("FTSatUsrDay").ToString() == "1" ? string.Format("{0:dd}", pdDate) : "";
                            //Sep
                            tC_DocFmtSep = odtTmp.Rows[0].Field<string>("FTSatUsrSep").ToString() == "1" ? "-" : "";
                            //Num
                            tC_DocFmtNum = odtTmp.Rows[0].Field<string>("FTSatUsrNum").ToString();
                        }

                        //tC_DocFmtLeft = tC_DocFmtChr + tC_DocFmtBch + tC_DocFmtPosShp + tC_DocFmtYear + tC_DocFmtMonth + tC_DocFmtDay + tC_DocFmtSep;
                        tC_DocFmtLeft = tC_DocFmtChr + tC_DocFmtYear + tC_DocFmtMonth + tC_DocFmtDay + tC_DocFmtBch + tC_DocFmtPosShp + tC_DocFmtSep;   //*Arm 63-02-17 - ปรับ Fomat DocNo
                        nC_DocRuningLength = tC_DocFmtNum.Length;

                        tDocFmt = tC_DocFmtLeft + new string('#', nC_DocRuningLength); ; //*Em 63-02-28
                    }
                }
            }
            catch
            { }
            finally
            {
                //*Net 63-07-30 ยกมาจาก Moshi
            }
            return tDocFmt;
        }

        public static string C_GENtDocNo(int pnDocType)
        {
            string tDocNo = "";
            //Int32 nLastDoc = 0;
            string tFmt = new string('0', nC_DocRuningLength);
            try
            {
                //nLastDoc = C_GETnLastDocNum(pnDocType);

                //tDocNo = tC_DocFmtLeft + string.Format("{0:" + tFmt + "}", nLastDoc + 1);

                switch (pnDocType)
                {
                    case 1:
                        tFmt = new string('0', nC_DocSalLength);
                        tDocNo = tC_DocFmtLeftSal + string.Format("{0:" + tFmt + "}", nC_LastDocSale + 1);

                        break;
                    case 9:
                        tFmt = new string('0', nC_DocSalLength);
                        tDocNo = tC_DocFmtLeftRfn + string.Format("{0:" + tFmt + "}", nC_LastDocRefund + 1);
                        break;
                }
                return tDocNo;
            }
            catch
            {
                return "";
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

        }

        public static void C_DATxGenNewDoc()
        {
            try
            {
                cVB.tVB_DocNo = C_GENtDocNo(nC_DocType);
                C_PRCxClearGenNew();
                C_PRCxClearPara();

                oC_SalHD = new cmlTPSTSalHD();
                oC_SalHD.FTBchCode = cVB.tVB_BchCode;
                oC_SalHD.FTXshDocNo = cVB.tVB_DocNo;
                oC_SalHD.FTShpCode = cVB.tVB_ShpCode;
                oC_SalHD.FNXshDocType = nC_DocType;
                oC_SalHD.FDXshDocDate = Convert.ToDateTime(cVB.tVB_SaleDate);
                oC_SalHD.FTXshCshOrCrd = "1";
                oC_SalHD.FTXshVATInOrEx = cVB.tVB_VATInOrEx;
                oC_SalHD.FTDptCode = cVB.tVB_DptCode;
                oC_SalHD.FTWahCode = cVB.tVB_WahCode;
                oC_SalHD.FTPosCode = cVB.tVB_PosCode;
                oC_SalHD.FTShfCode = cVB.tVB_ShfCode;
                oC_SalHD.FNSdtSeqNo = cVB.nVB_ShfSeq;
                oC_SalHD.FTUsrCode = cVB.tVB_UsrCode;
                oC_SalHD.FTRteCode = cVB.tVB_RteCode;
                oC_SalHD.FCXshRteFac = cVB.cVB_Rate;
                oC_SalHD.FCXshTotal = 0;
                oC_SalHD.FCXshTotalNV = 0;
                oC_SalHD.FCXshTotalNoDis = 0;
                oC_SalHD.FCXshTotalB4DisChgV = 0;
                oC_SalHD.FCXshTotalB4DisChgNV = 0;
                oC_SalHD.FCXshDis = 0;
                oC_SalHD.FCXshChg = 0;
                oC_SalHD.FCXshTotalAfDisChgV = 0;
                oC_SalHD.FCXshTotalAfDisChgNV = 0;
                oC_SalHD.FCXshAmtV = 0;
                oC_SalHD.FCXshAmtNV = 0;
                oC_SalHD.FCXshVat = 0;
                oC_SalHD.FCXshVatable = 0;
                oC_SalHD.FCXshGrand = 0;
                oC_SalHD.FCXshRnd = 0;
                oC_SalHD.FTXshGndText = "";
                oC_SalHD.FCXshPaid = 0;
                oC_SalHD.FCXshLeft = 0;
                oC_SalHD.FTXshStaRefund = "1";
                oC_SalHD.FTXshStaDoc = "2";
                oC_SalHD.FTXshStaPaid = "1";
                oC_SalHD.FNXshStaDocAct = 1;
                oC_SalHD.FNXshStaRef = 0;
                C_DATxInsHD();

                //*Net 63-07-31 sync header กับหน้าจอ 2
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxClearDoc();
                    cVB.oVB_CstScreen.W_SETxHeader(ptDocNo: cVB.tVB_DocNo);
                }
                //+++++++++++++++++++++++++++++++++++++
            
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxGenNewDoc : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_DATxInsHD()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tErr = "";
            try
            {
                oSql.AppendLine("DELETE FROM " + tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + oC_SalHD.FTBchCode + "' AND FTXshDocNo = '" + oC_SalHD.FTXshDocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + tC_TblSalHD + " (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("FTBchCode,FTXshDocNo,FTShpCode,FNXshDocType,");
                oSql.AppendLine("FDXshDocDate,FTXshCshOrCrd,FTXshVATInOrEx,");
                oSql.AppendLine("FTDptCode,FTWahCode,FTPosCode,FTShfCode,");
                oSql.AppendLine("FNSdtSeqNo,FTUsrCode,FTRteCode,FCXshRteFac,");
                oSql.AppendLine("FCXshTotal,FCXshTotalNV,FCXshTotalNoDis,FCXshTotalB4DisChgV,");
                oSql.AppendLine("FCXshTotalB4DisChgNV,FCXshDis,FCXshChg,FCXshTotalAfDisChgV,");
                oSql.AppendLine("FCXshTotalAfDisChgNV,FCXshAmtV,FCXshAmtNV,FCXshVat,");
                oSql.AppendLine("FCXshVatable,FCXshGrand,FCXshRnd,FTXshGndText,");
                oSql.AppendLine("FCXshPaid,FCXshLeft,FTXshStaRefund,FTXshStaDoc,");
                oSql.AppendLine("FTXshStaPaid,FNXshStaDocAct,FNXshStaRef,");
                oSql.AppendLine("FTXshAppVer,"); //*Net 63-07-02 เพิ่ม field Appversion ที่ขาย ปรับตาม Moshi
                oSql.AppendLine("FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES (");
                oSql.AppendLine("'" + oC_SalHD.FTBchCode + "','" + oC_SalHD.FTXshDocNo + "','" + oC_SalHD.FTShpCode + "'," + oC_SalHD.FNXshDocType + ",");
                oSql.AppendLine("'" + oC_SalHD.FDXshDocDate.Value.ToString("yyyy-MM-dd") + " ' + CONVERT(VARCHAR(8), GETDATE(),108),'" + oC_SalHD.FTXshCshOrCrd + "','" + oC_SalHD.FTXshVATInOrEx + "',");
                oSql.AppendLine("'" + oC_SalHD.FTDptCode + "','" + oC_SalHD.FTWahCode + "','" + oC_SalHD.FTPosCode + "','" + oC_SalHD.FTShfCode + "',");
                oSql.AppendLine(oC_SalHD.FNSdtSeqNo + ",'" + oC_SalHD.FTUsrCode + "','" + oC_SalHD.FTRteCode + "'," + oC_SalHD.FCXshRteFac + ",");
                oSql.AppendLine(oC_SalHD.FCXshTotal + "," + oC_SalHD.FCXshTotalNV + "," + oC_SalHD.FCXshTotalNoDis + "," + oC_SalHD.FCXshTotalB4DisChgV + ",");
                oSql.AppendLine(oC_SalHD.FCXshTotalB4DisChgNV + "," + oC_SalHD.FCXshDis + "," + oC_SalHD.FCXshChg + "," + oC_SalHD.FCXshTotalAfDisChgV + ",");
                oSql.AppendLine(oC_SalHD.FCXshTotalAfDisChgNV + "," + oC_SalHD.FCXshAmtV + "," + oC_SalHD.FCXshAmtNV + "," + oC_SalHD.FCXshVat + ",");
                oSql.AppendLine(oC_SalHD.FCXshVatable + "," + oC_SalHD.FCXshGrand + "," + oC_SalHD.FCXshRnd + ",'" + oC_SalHD.FTXshGndText + "',");
                oSql.AppendLine(oC_SalHD.FCXshPaid + "," + oC_SalHD.FCXshLeft + ",'" + oC_SalHD.FTXshStaRefund + "','" + oC_SalHD.FTXshStaDoc + "',");
                oSql.AppendLine("'" + oC_SalHD.FTXshStaPaid + "'," + oC_SalHD.FNXshStaDocAct + "," + oC_SalHD.FNXshStaRef + ",");
                oSql.AppendLine("'" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "',"); //*Net 63-07-30 ปรับตาม Moshi
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine(")");

                //*Net 63-07-30 ปรับตาม Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); 
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxInsHD : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// '*Em 63-06-09
        /// </summary>
        public static void C_DATxStamDateTimeHD()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("UPDATE " + tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FDXshDocDate = '" + oC_SalHD.FDXshDocDate.Value.ToString("yyyy-MM-dd") + " ' + CONVERT(VARCHAR(8), GETDATE(),108)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxStamDateTimeHD : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_DATxInsHDCst(string ptCstCode)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("DELETE " + cSale.tC_TblSalHDCst);
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("");
                if (cVB.bVB_ScanQR == true)
                {
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalHDCst + " (FTBchCode,FTXshDocNo, FTXshCardID, FTXshCstTel, FTXshCstName)");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo,'" + ptCstCode + "' AS FTCstCardID");
                    oSql.AppendLine(",'" + cVB.tVB_CstTel + "' AS FTXshCstTel, '" + cVB.tVB_CstName + "' AS FTXshCstName");   //*Em 62-12-18
                }
                else
                {
                    //*Arm 63-04-04 Comment Code
                    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalHDCst + " WITH(ROWLOCK)(FTBchCode,FTXshDocNo, FTXshCardID, FTXshCstTel, FTXshCstName, FTXshCardNo)"); //*Arm 63-04-03 เพิ่ม FTXshCardNo
                    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, ISNULL(FTCstCardID,'')");
                    //oSql.AppendLine(",'" + cVB.tVB_CstTel + "' AS FTXshCstTel, '" + cVB.tVB_CstName + "' AS FTXshCstName, '" + cVB.tVB_MemberCard + "' AS FTXshCardNo");   //*Em 62-12-18, //*Arm 63-04-03 เพิ่ม FTXshCardNo
                    //oSql.AppendLine("FROM TCNMCst WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTCstCode = '" + ptCstCode + "'");

                    //*Arm 63-04-04 ใช้ข้อมูลที่ได้มาจาก API2PSMaster
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalHDCst + " (FTBchCode,FTXshDocNo, FTXshCardID, FTXshCstTel, FTXshCstName, FTXshCardNo)"); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine("VALUES (");
                    oSql.AppendLine(" '" + cVB.tVB_BchCode + "' , '" + cVB.tVB_DocNo + "' , '" + cVB.tVB_CstCardID + "' ");
                    oSql.AppendLine(",'" + cVB.tVB_CstTel + "' , '" + cVB.tVB_CstName + "' , '" + cVB.tVB_MemberCard + "' )");
                    //+++++++++++++
                }
                oDB.C_SETxDataQuery(oSql.ToString());

                //Arm 62-11-08 Update CstCode
                oSql.Clear();
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK) SET");
                oSql.AppendLine("FTCstCode ='" + cVB.tVB_CstCode + "'");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND FNXshDocType = " + oC_SalHD.FNXshDocType);
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxInsHDCst : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        public static void C_DATxInsDT(cmlPdtOrder poOrder, int pnSeqNo, string ptStaPdt = "1")
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tErr = ""; //*Net 63-07-02 check error insert
            try
            {
                new cLog().C_WRTxLog("cSale", "C_DATxInsDT : Start Seq " + pnSeqNo, cVB.bVB_AlwPrnLog);
                oSql.AppendLine("INSERT INTO " + tC_TblSalDT + " (");
                oSql.AppendLine("FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, ");
                //oSql.AppendLine("FTXsdStkCode, FTPunCode, FTPunName, FCXsdFactor,");
                oSql.AppendLine("FTPunCode, FTPunName,FTPplCode, FCXsdFactor,");  //*Em 62-06-26  
                oSql.AppendLine("FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate,");
                oSql.AppendLine("FTXsdSaleType, FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, ");
                oSql.AppendLine("FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet,");
                oSql.AppendLine("FCXsdNetAfHD, FCXsdVat, FCXsdVatable, ");
                //oSql.AppendLine("FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                oSql.AppendLine("FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, ");
                oSql.AppendLine("FCXsdQtyRfn, FTXsdStaAlwDis, ");
                //oSql.AppendLine("FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, ");
                oSql.AppendLine("FTPdtStaSet, FTXsdRmk, ");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, ");
                oSql.AppendLine("FDCreateOn, FTCreateBy");
                oSql.AppendLine(")");
                //oSql.AppendLine("SELECT TOP 1 '" + cVB.tVB_BchCode + "' AS FTBchCode,'" + cVB.tVB_DocNo + "' AS FTXshDocNo," + cSale.nC_DTSeqNo + " AS FNXsdSeqNo, PDT.FTPdtCode, '" + poOrder.tPdtName + "' AS FTXsdPdtName, ");
                oSql.AppendLine("SELECT TOP 1 '" + cVB.tVB_BchCode + "' AS FTBchCode,'" + cVB.tVB_DocNo + "' AS FTXshDocNo," + pnSeqNo + " AS FNXsdSeqNo, PDT.FTPdtCode, '" + poOrder.tPdtName + "' AS FTXsdPdtName, ");
                //oSql.AppendLine("PDT.FTPdtStkCode AS FTXsdStkCode, BAR.FTPunCode AS FTPunCode, UNIT.FTPunName AS FTPunName,"+ poOrder.cFactor +" AS FCXsdFactor, ");
                //oSql.AppendLine("BAR.FTPunCode AS FTPunCode, '" + poOrder.tUnit + "' AS FTPunName,'" + cVB.tVB_PriceGroup + "' AS FTPplCode," + poOrder.cFactor + " AS FCXsdFactor, ");    //*Em 62-06-26
                oSql.AppendLine("PDT.FTPunCode AS FTPunCode, '" + poOrder.tUnit + "' AS FTPunName,'" + cVB.tVB_PriceGroup + "' AS FTPplCode," + poOrder.cFactor + " AS FCXsdFactor, ");     //*Em 63-05-15
                oSql.AppendLine("'" + poOrder.tBarcode + "' AS FTXsdBarCode,'' AS FTSrnCode,PDT.FTPdtStaVat AS FTXsdVatType, '" + cVB.tVB_VatCode + "' AS FTVatCode," + cVB.cVB_VatRate + " AS FCXsdVatRate, ");
                //oSql.AppendLine("PDT.FTPdtSaleType AS FTXsdSaleType, " + poOrder.cSetPrice + " AS FCXsdSalePrice," + poOrder.cQty + " AS FCXsdQty,(" + (poOrder.cQty * poOrder.cFactor) + "* PKS.FCPdtUnitFact) AS FCXsdQtyAll," + poOrder.cSetPrice + " AS FCXsdSetPrice, ");
                oSql.AppendLine("PDT.FTPdtSaleType AS FTXsdSaleType, " + poOrder.cSetPrice + " AS FCXsdSalePrice," + poOrder.cQty + " AS FCXsdQty,(" + (poOrder.cQty * poOrder.cFactor) + ") AS FCXsdQtyAll," + poOrder.cSetPrice + " AS FCXsdSetPrice, ");  //*Em 63-05-15 //*Net 63-07-31 เอา * PKS.FCPdtUnitFact ออก
                oSql.AppendLine("(" + poOrder.cSetPrice * poOrder.cQty + ") AS FCXsdAmtB4DisChg, '' AS FTXsdDisChgTxt, 0 AS FCXsdDis, 0 AS FCXsdChg, (" + poOrder.cSetPrice * poOrder.cQty + ") AS FCXsdNet, ");
                oSql.AppendLine("(" + poOrder.cSetPrice * poOrder.cQty + ") AS FCXsdNetAfHD, 0 AS FCXsdVat, 0 AS FCXsdVatable, ");
                //oSql.AppendLine("FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //oSql.AppendLine("0 AS FCXsdCostIn, 0 AS FCXsdCostEx, '" + ptStaPdt + "' AS FTXsdStaPdt, " + poOrder.cQty + " AS FCXsdQtyLef, ");
                oSql.AppendLine("ISNULL(PCA.FCPdtCostIn,0) AS FCXsdCostIn, ISNULL(PCA.FCPdtCostEx,0) AS FCXsdCostEx, '" + ptStaPdt + "' AS FTXsdStaPdt, " + poOrder.cQty + " AS FCXsdQtyLef, ");    //*Em 63-05-30
                oSql.AppendLine("0 AS FCXsdQtyRfn, PDT.FTPdtStaAlwDis AS FTXsdStaAlwDis, ");
                ////oSql.AppendLine("FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet,  ");
                oSql.AppendLine("'1' AS FTPdtStaSet, '" + poOrder.tRemark +"' AS FTXsdRmk, ");  //*Arm 63-08-08 เพิ่มรับตัวแปลเก็บค่า Remark
                oSql.AppendLine("GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, ");
                oSql.AppendLine("GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK)");
                //oSql.AppendLine("INNER JOIN TCNMPdtBar BAR WITH(NOLOCK) ON PDT.FTPdtCode = BAR.FTPdtCode");
                //oSql.AppendLine("INNER JOIN TCNMPdtPackSize PKS WITH(NOLOCK) ON PDT.FTPdtCode = PKS.FTPdtCode AND BAR.FTPunCode = PKS.FTPunCode");
                oSql.AppendLine("FROM TPSMPdt PDT WITH(NOLOCK)");   //*Em 63-05-15
                oSql.AppendLine("LEFT JOIN TCNMPdtCostAvg PCA WITH(NOLOCK) ON PCA.FTPdtCode = PDT.FTPdtCode");  //*Em 63-05-30
                //oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L UNIT WITH(NOLOCK) ON BAR.FTPunCode = UNIT.FTPunCode AND UNIT.FNLngID = " + cVB.nVB_Language);
                //oSql.AppendLine("WHERE PDT.FTPdtCode = '" + poOrder.tPdtCode + "' AND BAR.FTBarCode = '" + poOrder.tBarcode + "'");
                oSql.AppendLine("WHERE PDT.FTPdtCode = '" + poOrder.tPdtCode + "' AND PDT.FTBarCode = '" + poOrder.tBarcode + "'"); //*Em 63-05-15
                oSql.AppendLine("");

                //*Net 63-07-30 ปรับตาม Moshi
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-07-02
                oDB.C_SETxDataQuery(oSql.ToString(), out tErr);
                if (!String.IsNullOrEmpty(tErr))
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgErrNotSale"), 2);
                    Environment.Exit(1);
                }

                ////Update vat/vatable
                //C_DATxUpdVat();

                ////Update cost
                //C_DATxUpdCost();

                new cLog().C_WRTxLog("cSale", "C_DATxInsDT : End Seq " + pnSeqNo, cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxInsDT : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_DATxInsPDRefund()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tListPmt = "";
            try
            {
                tListPmt = "'" + string.Join("','", cVB.atVB_PmtRefund) + "'";

                if (tListPmt != "")
                {
                    oSql.AppendLine("DELETE FROM " + tC_TblSalPD + " WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("");
                    oSql.AppendLine("INSERT " + tC_TblSalPD + "(FTBchCode, FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, ");
                    oSql.AppendLine("	FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                    oSql.AppendLine("SELECT DISTINCT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, PD.FTPmhDocNo, PD.FNXsdSeqNo, PD.FTPmdGrpName, PD.FTPdtCode, PD.FTPunCode, PD.FCXsdQty, PD.FCXsdQtyAll, PD.FCXsdSetPrice, PD.FCXsdNet, ");
                    oSql.AppendLine("PD.FCXpdGetQtyDiv, PD.FTXpdGetType, PD.FCXpdGetValue, PD.FCXpdDis, PD.FCXpdPerDisAvg, PD.FCXpdDisAvg, PD.FCXpdPoint, PD.FTXpdStaRcv, PD.FTPplCode, PD.FTXpdCpnText, PD.FTCpdBarCpn");
                    oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + tC_TblSalDT + " DT WITH(NOLOCK) ON PD.FTPdtCode = DT.FTPdtCode AND DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND PD.FTPmhDocNo IN (" + tListPmt + ")");
                    oDB.C_SETxDataQuery(oSql.ToString());

                    new cPdtPmt().C_PRCxPmtDisProratePD();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxInsDT : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        /// <summary>
        /// Update vat/vatable.
        /// </summary>
        public static void C_DATxUpdVat()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                //*Net แก้ CASE WHEN เป็น Where
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdVat = ROUND( CASE WHEN FTXsdVatType = '1' THEN ((FCXsdNetAfHD*" + cVB.cVB_VatRate + ")/(100+" + cVB.cVB_VatRate + ")) ELSE 0 END," + cVB.nVB_DecSave + ")");
                oSql.AppendLine(",FCXsdVatable = FCXsdNetAfHD - ROUND( CASE WHEN FTXsdVatType = '1' THEN ((FCXsdNetAfHD*" + cVB.cVB_VatRate + ")/(100+" + cVB.cVB_VatRate + ")) ELSE 0 END," + cVB.nVB_DecSave + ")");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");

                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxUpdVat : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        //*Net 63-06-01 ย้ายการ Update Vat รายการสุดท้ายมาเป็น Fn
        /// <summary>
        /// Update vat/vatable.
        /// </summary>
        public static void C_DATxProrateVatFrmHD()
        {
            DataTable oSalDT;
            decimal cSumVatDT = 0;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT FNXsdSeqNo,FCXsdVat,FCXsdVatable");
                oSql.AppendLine($"FROM {cSale.tC_TblSalDT}");
                oSql.AppendLine($"	WHERE FTXsdVatType = '1' AND FTXsdStaPdt <> '4'");
                oSql.AppendLine($"   AND FTBchCode = '{cVB.tVB_BchCode}'");
                oSql.AppendLine($"   AND FTXshDocNo = '{cVB.tVB_DocNo}'");
                oSql.AppendLine("ORDER BY FNXsdSeqNo");
                oSalDT = oDB.C_GEToDataQuery(oSql.ToString());

                if (oSalDT == null || oSalDT.Rows.Count == 0) return;

                for (int nRec = 0; nRec < oSalDT.Rows.Count - 1; nRec++)
                {
                    cSumVatDT += oSalDT.Rows[nRec].Field<decimal>("FCXsdVat");
                }

                oSql.Clear();
                oSql.AppendLine($"DECLARE @HDVat numeric(18,{cVB.nVB_DecSave}) = (SELECT TOP 1 FCXshVat FROM {tC_TblSalHD} WHERE FTBchCode='{cVB.tVB_BchCode}' AND FTXshDocNo='{cVB.tVB_DocNo}' );");
                oSql.AppendLine("UPDATE DT WITH(ROWLOCK)");
                //oSql.AppendLine($"SET FCXsdVat = ROUND( {oC_SalHD.FCXshVat} - {cSumVatDT} , {cVB.nVB_DecSave})"); //*Net 63-07-22 ต้องใช้ Vat จาก HD จริง
                //oSql.AppendLine($"SET FCXsdVat = ROUND( (SELECT TOP 1 FCXshVat FROM {tC_TblSalHD} WHERE FTBchCode='{cVB.tVB_BchCode}' AND FTXshDocNo='{cVB.tVB_DocNo}' ) - {cSumVatDT} , {cVB.nVB_DecSave})");
                oSql.AppendLine($"SET FCXsdVat = ROUND( @HDVat - {cSumVatDT} , {cVB.nVB_DecSave})");
                //oSql.AppendLine($",FCXsdVatable = ROUND( FCXsdNetAfHD - ({oC_SalHD.FCXshVat} - {cSumVatDT}), {cVB.nVB_DecSave})"); //*Net 63-08-05 ต้องใช้ Vat จาก HD จริง
                oSql.AppendLine($",FCXsdVatable = ROUND( FCXsdNetAfHD - (@HDVat - {cSumVatDT}), {cVB.nVB_DecSave})");
                oSql.AppendLine($"FROM {cSale.tC_TblSalDT} DT");
                oSql.AppendLine($"WHERE DT.FTBchCode = '{cVB.tVB_BchCode}'");
                oSql.AppendLine($"AND DT.FTXshDocNo = '{cVB.tVB_DocNo}'");
                oSql.AppendLine($"AND FNXsdSeqNo={oSalDT.Rows[oSalDT.Rows.Count - 1].Field<int>("FNXsdSeqNo")}");
                oDB.C_SETxDataQuery(oSql.ToString());
                oSalDT.Dispose();

                //oSql.AppendLine("UPDATE DT WITH(ROWLOCK)");
                //oSql.AppendLine("SET FCXsdVat = HD.FCXshVat - ISNULL((SELECT SUM(FCXsdVat) FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo <> DT.FNXsdSeqNo AND FTXsdVatType = '1' AND FTXsdStaPdt <> '4'),0)");
                //oSql.AppendLine($"SET FCXsdVat = {oC_SalHD.FCXshVat} - ISNULL((SELECT SUM(FCXsdVat) FROM {cSale.tC_TblSalDT} WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo <> DT.FNXsdSeqNo AND FTXsdVatType = '1' AND FTXsdStaPdt <> '4'),0)");
                //oSql.AppendLine($",FCXsdVatable = FCXsdNetAfHD - ({oC_SalHD.FCXshVat} - ISNULL((SELECT SUM(FCXsdVat) FROM {cSale.tC_TblSalDT} WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo <> DT.FNXsdSeqNo AND FTXsdVatType = '1' AND FTXsdStaPdt <> '4'),0))");
                //oSql.AppendLine($"FROM {cSale.tC_TblSalDT} DT");
                //oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalHD + " HD WITH(NOLOCK) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                //oSql.AppendLine($"INNER JOIN (SELECT FTBchCode,FTXshDocNo,MAX(FNXsdSeqNo) FNXsdSeqNo");
                //oSql.AppendLine($"	FROM {cSale.tC_TblSalDT} WITH(NOLOCK)");
                //oSql.AppendLine($"	WHERE FTXsdVatType = '1' AND FTXsdStaPdt <> '4'");
                //oSql.AppendLine($"   AND FTBchCode = '{cVB.tVB_BchCode}'");
                //oSql.AppendLine($"   AND FTXshDocNo = '{cVB.tVB_DocNo}'");
                //oSql.AppendLine($"	GROUP BY FTBchCode,FTXshDocNo) Tmp ON Tmp.FTBchCode = DT.FTBchCode AND Tmp.FTXshDocNo = DT.FTXshDocNo AND Tmp.FNXsdSeqNo = DT.FNXsdSeqNo");
                //oSql.AppendLine($"WHERE DT.FTBchCode = '{cVB.tVB_BchCode}'");
                //oSql.AppendLine($"AND DT.FTXshDocNo = '{cVB.tVB_DocNo}'");
                //oSql.AppendLine($"AND FNXsdSeqNo = (SELECT TOP 1 MAX(FNXsdSeqNo) FROM {cSale.tC_TblSalDT} WITH(NOLOCK) WHERE FTXsdVatType = '1' AND FTXsdStaPdt <> '4' AND FTBchCode = '{cVB.tVB_BchCode}' AND FTXshDocNo = '{cVB.tVB_DocNo}')");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxUpdVat : " + oEx.Message);
            }
            finally
            {
                oSalDT = null;
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Update cost.
        /// </summary>
        public static void C_DATxUpdCost()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {

                //oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                //oSql.AppendLine("SET FCXsdCostIn = FCXsdVat + FCXsdVatable");
                //oSql.AppendLine(",FCXsdCostEx = FCXsdVatable");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");

                new cLog().C_WRTxLog("cSale", "C_DATxUpdCost : Start", cVB.bVB_AlwPrnLog);
                //*Em 63-04-17
                oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdCostIn = ISNULL(FCPdtCostIn,0)");
                oSql.AppendLine(",FCXsdCostEx = ISNULL(FCPdtCostEx,0)");
                oSql.AppendLine("FROM " + tC_TblSalDT + " DT");
                oSql.AppendLine("LEFT JOIN TCNMPdtCostAvg PCA WITH(NOLOCK) ON PCA.FTPdtCode = DT.FTPdtCode");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //++++++++++++++++++
                oDB.C_SETxDataQuery(oSql.ToString());
                new cLog().C_WRTxLog("cSale", "C_DATxUpdCost : End", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxUpdCost : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Clear data in temp.
        /// </summary>
        public static void C_PRCxClearGenNew()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {


                //Clear RD /Arm 63-03-12
                oSql.AppendLine("DELETE FROM " + tC_TblSalRD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //Clear PD
                oSql.AppendLine("DELETE FROM " + tC_TblSalPD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //Clear RC
                oSql.AppendLine("DELETE FROM " + tC_TblSalRC + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //Clear DT
                //oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //Clear DTDis
                //oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalDTDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //oDB.C_SETxDataQuery(oSql.ToString());

                ////Clear DTPmt //*Net 63-07-07 ไม่ใช้ตารางแล้ว ยกมาจาก Moshi
                ////oSql = new StringBuilder();
                //oSql.AppendLine("DELETE FROM " + tC_TblSalDTPmt + " WITH(ROWLOCK)");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                ////oDB.C_SETxDataQuery(oSql.ToString());

                //Clear HDCst
                //oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalHDCst + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //Clear HDDis
                //oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalHDDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //Clear HD
                //oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");

                //oSql.AppendLine("TRUNCATE TABLE " + tC_TblRefund);  //*Em 63-07-13, //*Arm 63-09-11 Comment Code เนื่องจากการแบบเลือกรายการจะไม่สามารถทำได้
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxClearGenNew : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Update qty after scan product duplicate.
        /// </summary>
        public static void C_PRCxUpdateQty(decimal pcQty = 1)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {

                oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdQty = FCXsdQty + " + pcQty);
                oSql.AppendLine(",FCXsdQtyAll = FCXsdFactor * (FCXsdQty + " + pcQty + ")");
                oSql.AppendLine(",FCXsdAmtB4DisChg = FCXsdSetPrice * (FCXsdQty + " + pcQty + ")");
                oSql.AppendLine(",FCXsdNet = FCXsdSetPrice * (FCXsdQty + " + pcQty + ")");
                oSql.AppendLine(",FCXsdNetAfHD = FCXsdSetPrice * (FCXsdQty + " + pcQty + ")");
                oSql.AppendLine(",FCXsdQtyLef = (FCXsdQty + " + pcQty + ")");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);
                oDB.C_SETxDataQuery(oSql.ToString());

                //*Net 63-07-30 ปรับตาม Moshi
                //C_DATxUpdVat();
                //C_DATxUpdCost();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxUpdateQty : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Net 63-06-10
        /// Update Last DocNum in TSysSetting
        /// </summary>
        public static void C_PRCxUpdateLstDocNum()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();

                oSql.AppendLine("UPDATE TSysSetting WITH(ROWLOCK)");
                oSql.AppendLine($"SET FTSysStaUsrValue = '{nC_LastDocSale}'");
                oSql.AppendLine("WHERE FTSysCode = 'tPS_LastBillS' ");

                oSql.AppendLine("UPDATE TSysSetting WITH(ROWLOCK)");
                oSql.AppendLine($"SET FTSysStaUsrValue = '{nC_LastDocRefund}'");
                oSql.AppendLine("WHERE FTSysCode = 'tPS_LastBillR' ");

                oDB.C_SETxDataQuery(oSql.ToString());
                cVB.nVB_LastBillS = nC_LastDocSale;
                cVB.nVB_LastBillR = nC_LastDocRefund;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxUpdateLstDocNum : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Net 63-07-02
        /// Update RsnCode in Bill
        /// </summary>
        public static void C_PRCxUpdateRsnCode()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();


                //*Net 63-07-02 อัพเดตเหตุผล
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FDLastUpdOn = GETDATE()");
                oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine(",FTRsnCode = '" + cVB.oVB_Reason.FTRsnCode + "'");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());
                //++++++++++++++++++++++++++++++++++++++++

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxUpdateRsnCode : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Update qty after Chang Qty duplicate.
        /// </summary>
        public static void C_PRCxChangeQty(decimal pcQty) //*Arm 62-10-07
        {
            //cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            List<cmlTPSTSalDTDis> aoDisChg; //*Net 63-04-10
            decimal cDisSum = 0, cDis = 0, cChgSum = 0, cChg = 0;
            decimal cAmtB4DisChg;
            try
            {
                oSql = new StringBuilder();
                //*Arm 63-09-18 
                
                //Clear ส่วนลด
                oSql.AppendLine("DELETE " + tC_TblSalDTDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);
                oSql.AppendLine(" ");
                // Update DT
                oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdQty = " + pcQty);
                oSql.AppendLine(",FCXsdQtyAll = FCXsdFactor * " + pcQty);
                oSql.AppendLine(",FCXsdAmtB4DisChg = FCXsdSetPrice * " + pcQty);
                oSql.AppendLine(",FCXsdDis = 0.00 ");
                oSql.AppendLine(",FCXsdChg = 0.00 ");
                oSql.AppendLine(",FCXsdNet = FCXsdSetPrice * " + pcQty);
                oSql.AppendLine(",FCXsdNetAfHD = FCXsdSetPrice * " + pcQty);
                oSql.AppendLine(",FCXsdQtyLef =  " + pcQty);
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);

                new cDatabase().C_SETxDataQuery(oSql.ToString());
                //+++++++++++++

                //oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                //oSql.AppendLine("SET FCXsdQty = " + pcQty);
                //oSql.AppendLine(",FCXsdQtyAll = FCXsdFactor * " + pcQty);
                //oSql.AppendLine(",FCXsdAmtB4DisChg = FCXsdSetPrice * " + pcQty);
                //oSql.AppendLine(",FCXsdNet = FCXsdSetPrice * " + pcQty);
                //oSql.AppendLine(",FCXsdNetAfHD = FCXsdSetPrice * " + pcQty);
                //oSql.AppendLine(",FCXsdQtyLef =  " + pcQty);
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                //oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);

                ////*Net 63-04-10 เอาราคาก่อนลดออกมา
                //oSql.AppendLine();
                //oSql.AppendLine();
                //oSql.AppendLine($"SELECT FCXsdAmtB4DisChg FROM {tC_TblSalDT}");
                //oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                //oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                //oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");

                ////oDB.C_SETxDataQuery(oSql.ToString());
                //cAmtB4DisChg = new cDatabase().C_GETaDataQuery<decimal>(oSql.ToString()).FirstOrDefault();

                //if (cVB.cVB_PriceAfEditQty < 0)
                //{
                //    oSql.AppendLine($"DELETE {tC_TblSalDTDis}");
                //    oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                //    oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                //    oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");

                //    oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                //    oSql.AppendLine("SET FCXsdDis = 0.00,");
                //    oSql.AppendLine("FCXsdChg = 0.00");
                //    oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                //    oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                //    oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");
                //    new cDatabase().C_GEToDataQuery(oSql.ToString());
                //}
                //else
                //{
                //    //*Net 63-04-10 คำนวนส่วนลดใหม่ +++++++++++++++++++++++++++++++
                //    oSql.Clear();
                //    oSql.AppendLine("SELECT FTBchCode ");
                //    oSql.AppendLine(",FTXshDocNo");
                //    oSql.AppendLine(",FNXsdSeqNo");
                //    oSql.AppendLine(",CONVERT(varchar,FDXddDateIns,120) AS FDXddDateIns");
                //    oSql.AppendLine(",FNXddStaDis");
                //    oSql.AppendLine(",FTXddDisChgTxt");
                //    oSql.AppendLine(",FTXddDisChgType");
                //    oSql.AppendLine(",FCXddNet");
                //    oSql.AppendLine(",FCXddValue ");
                //    oSql.AppendLine("FROM " + tC_TblSalDTDis + " WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXsdSeqNo = " + nC_DTSeqNo);

                //    aoDisChg = new cDatabase().C_GETaDataQuery<cmlTPSTSalDTDis>(oSql.ToString());

                //    oSql.Clear();
                //    foreach (cmlTPSTSalDTDis oDisChg in aoDisChg)
                //    {
                //        oSql.AppendLine($"UPDATE {tC_TblSalDTDis}");
                //        oSql.AppendLine($"SET FCXddNet={cAmtB4DisChg},");
                //        switch (oDisChg.FTXddDisChgType)
                //        {
                //            case "1":
                //                cDis = Convert.ToDecimal(oDisChg.FTXddDisChgTxt);
                //                cAmtB4DisChg -= cDis;
                //                break;
                //            case "2":
                //                cDis = cAmtB4DisChg * (Convert.ToDecimal(oDisChg.FTXddDisChgTxt.Replace("%", "")) / 100.00M);
                //                cAmtB4DisChg -= cDis;
                //                break;
                //            case "3":
                //                cChg = Convert.ToDecimal(oDisChg.FTXddDisChgTxt);
                //                cAmtB4DisChg += cChg;
                //                break;
                //            case "4":
                //                cChg = cAmtB4DisChg * (Convert.ToDecimal(oDisChg.FTXddDisChgTxt.Replace("%", "")) / 100.00M);
                //                cAmtB4DisChg += cChg;
                //                break;
                //        }
                //        cDisSum += cDis;
                //        oSql.AppendLine($"FCXddValue={new cSP().SP_SETtDecShwSve(2, cDis, cVB.nVB_DecSave)}");
                //        oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                //        oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                //        oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");
                //        oSql.AppendLine($"AND CONVERT(varchar,FDXddDateIns,120) = '{Convert.ToDateTime(oDisChg.FDXddDateIns).ToString("yyyy-MM-dd HH:mm:ss")}'");
                //        oSql.AppendLine("");
                //        oSql.AppendLine("");
                //    }
                //    if (!string.IsNullOrEmpty(oSql.ToString())) new cDatabase().C_SETxDataQuery(oSql.ToString());

                //    oSql = new StringBuilder();
                //    oSql.AppendLine("UPDATE " + tC_TblSalDT + " SET ");
                //    oSql.AppendLine(" FCXsdDis = " + cDisSum + " ");
                //    oSql.AppendLine(",FCXsdChg = " + cChgSum + " ");
                //    oSql.AppendLine(",FCXsdNet = FCXsdAmtB4DisChg - " + cDisSum + " + " + cChgSum + " ");
                //    oSql.AppendLine(",FCXsdNetAfHD = FCXsdAmtB4DisChg - " + cDisSum + " + " + cChgSum + " ");
                //    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXsdSeqNo = " + nC_DTSeqNo + "");
                //    new cDatabase().C_SETxDataQuery(oSql.ToString());
                //}

                //*Net 63-07-30 ปรับตาม Moshi
                //C_DATxUpdVat();
                //C_DATxUpdCost();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxChangeQty : " + oEx.Message);
            }
            finally
            {
                //oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxClearDisItem(int pnDTSeqNo) //*Net 63-09-17
        {
            StringBuilder oSql;
            try
            {
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine($"DELETE {tC_TblSalDTDis}");
                oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                oSql.AppendLine($"AND FNXsdSeqNo = {pnDTSeqNo} ");

                oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdDis = 0.00,");
                oSql.AppendLine("FCXsdChg = 0.00");
                oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                oSql.AppendLine($"AND FNXsdSeqNo = {pnDTSeqNo} ");
                new cDatabase().C_GEToDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxClearDisItem : " + oEx.Message);
            }
            finally
            {
                oSql = null;
            }
        }

        /// <summary>
        /// Update หลังเลือกสินค้าฟรี
        /// </summary>
        public static void C_PRCxItemFree(decimal pcQty) //*Arm 62-10-07
        {
            //*Arm 62-10-08 แก้ไข
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdQty = " + pcQty);
                oSql.AppendLine(",FTXsdStaPdt = '3' ");
                oSql.AppendLine(",FCXsdSetPrice = 0");  //*Em 62-10-08
                oSql.AppendLine(",FTXsdPdtName = 'Free-'+FTXsdPdtName");
                oSql.AppendLine(",FCXsdQtyAll = FCXsdFactor * " + pcQty);
                oSql.AppendLine(",FCXsdAmtB4DisChg = 0 * " + pcQty);
                oSql.AppendLine(",FCXsdNet = 0 * " + pcQty);
                oSql.AppendLine(",FCXsdNetAfHD = 0 * " + pcQty);
                oSql.AppendLine(",FCXsdQtyLef =  " + pcQty);
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);
                oDB.C_SETxDataQuery(oSql.ToString());

                //*Net 63-07-30 ปรับตาม Moshi
                //C_DATxUpdVat();
                //C_DATxUpdCost();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxItemFree : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

        }

        /// <summary>
        /// Summary to update HD.
        /// </summary>
        public static void C_PRCxSummary2HD()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtPromo = new DataTable();
            DataTable odtTemp = new DataTable();
            DataSet odsTmp = new DataSet();
            try
            {
                //*Net 63-07-30 ปรับตาม Moshi
                //C_DATxUpdVat();
                //C_DATxUpdCost();
                //*Net 63-04-13 Check is NUll = 0
                oSql.AppendLine("SELECT ISNULL(SUM(FCXsdNet),0) AS FCXshTotal, ");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdVatType = '1' THEN 0 ELSE FCXsdNet END),0) AS FCXshTotalNV,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' THEN 0 ELSE FCXsdNet END),0) AS FCXshTotalNoDis,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' AND FTXsdVatType = '1' THEN FCXsdNet ELSE 0 END),0) AS FCXshTotalB4DisChgV,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' AND FTXsdVatType = '2' THEN FCXsdNet ELSE 0 END),0) AS FCXshTotalB4DisChgNV,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' AND FTXsdVatType = '1' THEN FCXsdNetAfHD ELSE 0 END),0) AS FCXshTotalAfDisChgV,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' AND FTXsdVatType = '2' THEN FCXsdNetAfHD ELSE 0 END),0) AS FCXshTotalAfDisChgNV,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdVatType = '1' THEN FCXsdNet ELSE 0 END),0) AS FCXshAmtV,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdVatType = '2' THEN FCXsdNet ELSE 0 END),0) AS FCXshAmtNV,");
                oSql.AppendLine("ISNULL(SUM(FCXsdVat),0) AS FCXshVat, ISNULL(SUM(FCXsdVatable),0) AS FCXshVatable,");
                oSql.AppendLine("ISNULL(SUM(FCXsdNetAfHD),0) AS FCXshGrand");
                oSql.AppendLine("FROM " + tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FTXsdStaPdt != '4' "); // *Arm 62-10-15
                //DataTable odtTemp = oDB.C_GEToDataQuery(oSql.ToString());
                //*Em 63-04-25
                oSql.AppendLine();
                oSql.AppendLine("SELECT ISNULL(SUM(ISNULL(PD.FCXpdDisAvg,0)),0) AS FCXshTotal,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdVatType = '1' THEN 0 ELSE ISNULL(PD.FCXpdDisAvg,0) END),0) AS FCXshTotalNV,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' THEN 0 ELSE ISNULL(PD.FCXpdDisAvg,0) END),0) AS FCXshTotalNoDis,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' AND FTXsdVatType = '1' THEN ISNULL(PD.FCXpdDisAvg,0) ELSE 0 END),0) AS FCXshTotalB4DisChgV,");
                oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' AND FTXsdVatType = '2' THEN ISNULL(PD.FCXpdDisAvg,0) ELSE 0 END),0) AS FCXshTotalB4DisChgNV");
                oSql.AppendLine("FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN " + tC_TblSalPD + " PD WITH(NOLOCK) ON PD.FTBchCode = DT.FTBchCode AND PD.FTXshDocNo = DT.FTXshDocNo AND PD.FNXsdSeqNo = DT.FNXsdSeqNo");
                oSql.AppendLine("   AND PD.FTXpdGetType <> '4'");   //*Em 63-05-05
                oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND DT.FTXsdStaPdt != '4'");
                odsTmp = oDB.C_GEToDataSetQuery(oSql.ToString());
                odtTemp = odsTmp.Tables[0];
                odtPromo = odsTmp.Tables[1];
                //+++++++++++++++++++++++++++
                if (odtTemp != null && odtTemp.Rows.Count > 0)
                {
                    ////*Em 63-03-29
                    ////*Net 63-04-13 Check is NUll = 0
                    //oSql.Clear();
                    //oSql.AppendLine("SELECT ISNULL(SUM(ISNULL(PD.FCXpdDisAvg,0)),0) AS FCXshTotal,");
                    //oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdVatType = '1' THEN 0 ELSE ISNULL(PD.FCXpdDisAvg,0) END),0) AS FCXshTotalNV,");
                    //oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' THEN 0 ELSE ISNULL(PD.FCXpdDisAvg,0) END),0) AS FCXshTotalNoDis,");
                    //oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' AND FTXsdVatType = '1' THEN ISNULL(PD.FCXpdDisAvg,0) ELSE 0 END),0) AS FCXshTotalB4DisChgV,");
                    //oSql.AppendLine("ISNULL(SUM(CASE WHEN FTXsdStaAlwDis = '1' AND FTXsdVatType = '2' THEN ISNULL(PD.FCXpdDisAvg,0) ELSE 0 END),0) AS FCXshTotalB4DisChgNV");
                    //oSql.AppendLine("FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                    //oSql.AppendLine("LEFT JOIN " + tC_TblSalPD + " PD WITH(NOLOCK) ON PD.FTBchCode = DT.FTBchCode AND PD.FTXshDocNo = DT.FTXshDocNo AND PD.FNXsdSeqNo = DT.FNXsdSeqNo");
                    //oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    //oSql.AppendLine("AND DT.FTXsdStaPdt != '4'");
                    //odtPromo = oDB.C_GEToDataQuery(oSql.ToString());
                    ////+++++++++++++++++++++++
                    oC_SalHD.FCXshTotal = odtTemp.Rows[0].Field<decimal>("FCXshTotal");
                    oC_SalHD.FCXshTotalNV = odtTemp.Rows[0].Field<decimal>("FCXshTotalNV");
                    oC_SalHD.FCXshTotalNoDis = odtTemp.Rows[0].Field<decimal>("FCXshTotalNoDis");
                    oC_SalHD.FCXshTotalB4DisChgV = odtTemp.Rows[0].Field<decimal>("FCXshTotalB4DisChgV");
                    oC_SalHD.FCXshTotalB4DisChgNV = odtTemp.Rows[0].Field<decimal>("FCXshTotalB4DisChgNV");
                    oC_SalHD.FCXshTotalAfDisChgV = odtTemp.Rows[0].Field<decimal>("FCXshTotalAfDisChgV");
                    oC_SalHD.FCXshTotalAfDisChgNV = odtTemp.Rows[0].Field<decimal>("FCXshTotalAfDisChgNV");
                    oC_SalHD.FCXshAmtV = odtTemp.Rows[0].Field<decimal>("FCXshAmtV");
                    oC_SalHD.FCXshAmtNV = odtTemp.Rows[0].Field<decimal>("FCXshAmtNV");
                    oC_SalHD.FCXshVat = odtTemp.Rows[0].Field<decimal>("FCXshVat");
                    oC_SalHD.FCXshVatable = odtTemp.Rows[0].Field<decimal>("FCXshVatable");
                    oC_SalHD.FCXshGrand = odtTemp.Rows[0].Field<decimal>("FCXshGrand");

                    //*Em 63-03-29
                    if (odtPromo != null && odtPromo.Rows.Count > 0)
                    {
                        oC_SalHD.FCXshTotal = odtTemp.Rows[0].Field<decimal>("FCXshTotal") - odtPromo.Rows[0].Field<decimal>("FCXshTotal");
                        oC_SalHD.FCXshTotalNV = odtTemp.Rows[0].Field<decimal>("FCXshTotalNV") - odtPromo.Rows[0].Field<decimal>("FCXshTotalNV");
                        oC_SalHD.FCXshTotalNoDis = odtTemp.Rows[0].Field<decimal>("FCXshTotalNoDis") - odtPromo.Rows[0].Field<decimal>("FCXshTotalNoDis");
                        oC_SalHD.FCXshTotalB4DisChgV = odtTemp.Rows[0].Field<decimal>("FCXshTotalB4DisChgV") - odtPromo.Rows[0].Field<decimal>("FCXshTotalB4DisChgV");
                        oC_SalHD.FCXshTotalB4DisChgNV = odtTemp.Rows[0].Field<decimal>("FCXshTotalB4DisChgNV") - odtPromo.Rows[0].Field<decimal>("FCXshTotalB4DisChgNV");
                    }
                    //+++++++++++++++++++++++++

                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE " + tC_TblSalHD + " WITH(ROWLOCK)");
                    oSql.AppendLine("SET FCXshTotal = " + oC_SalHD.FCXshTotal);
                    oSql.AppendLine(",FCXshTotalNV = " + oC_SalHD.FCXshTotalNV);
                    oSql.AppendLine(",FCXshTotalNoDis = " + oC_SalHD.FCXshTotalNoDis);
                    oSql.AppendLine(",FCXshTotalB4DisChgV = " + oC_SalHD.FCXshTotalB4DisChgV);
                    oSql.AppendLine(",FCXshTotalB4DisChgNV = " + oC_SalHD.FCXshTotalB4DisChgNV);
                    oSql.AppendLine(",FCXshTotalAfDisChgV = " + oC_SalHD.FCXshTotalAfDisChgV);
                    oSql.AppendLine(",FCXshTotalAfDisChgNV = " + oC_SalHD.FCXshTotalAfDisChgNV);
                    //oSql.AppendLine(",FCXshAmtV = " + oC_SalHD.FCXshAmtV); //*Net 63-08-06 ปรับสูตรคำนวน
                    //oSql.AppendLine(",FCXshAmtNV = " + oC_SalHD.FCXshAmtNV); //*Net 63-08-06 ปรับสูตรคำนวน
                    oSql.AppendLine(",FCXshAmtV = " + (oC_SalHD.FCXshTotal - oC_SalHD.FCXshTotalNV - (oC_SalHD.FCXshTotalB4DisChgV - oC_SalHD.FCXshTotalAfDisChgV)));
                    oSql.AppendLine(",FCXshAmtNV = " + (oC_SalHD.FCXshTotalNV - (oC_SalHD.FCXshTotalB4DisChgNV - oC_SalHD.FCXshTotalAfDisChgNV)));
                    oSql.AppendLine(",FCXshVat = " + oC_SalHD.FCXshVat);
                    oSql.AppendLine(",FCXshVatable = " + oC_SalHD.FCXshVatable);
                    oSql.AppendLine(",FCXshGrand = " + oC_SalHD.FCXshGrand);
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                    oDB.C_SETxDataQuery(oSql.ToString());

                    //cVB.cVB_Amount = (decimal)oC_SalHD.FCXshGrand;
                    cVB.cVB_Amount = (decimal)oC_SalHD.FCXshTotal;  //*Em 63-05-09
                }

                odtTemp = null;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxSummary2HD : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Net 63-07-02 ยกมาจาก Moshi
        /// drop ตาราง Temp เมื่อไม่มีบิลสมบูรณ เหลือแล้ว
        /// </summary>
        public static void C_PRCxCheckDropTemp()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine("SELECT COUNT(*) FROM " + tC_TblSalHD + " WHERE FTXshStaDoc='1'");
                if (oDB.C_GEToDataQuery<int>(oSql.ToString()) == 0)
                {

                    oSql.Clear();
                    oSql.AppendLine("BEGIN TRY");
                    oSql.AppendLine("    BEGIN TRAN");

                    //HD
                    oSql.AppendLine("		DROP TABLE " + tC_TblSalHD + "");
                    //oSql.AppendLine("		SELECT TOP 0 * INTO " + tC_TblSalHD + " FROM TPSTSalHD WITH(NOLOCK)");

                    //HDCst

                    oSql.AppendLine("		DROP TABLE " + tC_TblSalHDCst + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalHDCst + " FROM TPSTSalHDCst WITH(NOLOCK)");
                    oSql.AppendLine("");

                    //HDDis
                    oSql.AppendLine("		    DROP TABLE " + tC_TblSalHDDis + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalHDDis + " FROM TPSTSalHDDis WITH(NOLOCK)");
                    oSql.AppendLine("");

                    //DT

                    oSql.AppendLine("		    DROP TABLE " + tC_TblSalDT + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDT + " FROM TPSTSalDT WITH(NOLOCK)");

                    oSql.AppendLine("");

                    //DTDis

                    oSql.AppendLine("		    DROP TABLE " + tC_TblSalDTDis + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDTDis + " FROM TPSTSalDTDis WITH(NOLOCK)");

                    oSql.AppendLine("");

                    //DTPmt //*Net 63-07-07 ไม่ใช้ตาราง นี้แล้ว
                    //oSql.AppendLine("		    DROP TABLE " + tC_TblSalDTPmt + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDTPmt + " FROM TPSTSalDTPmt WITH(NOLOCK)");
                    oSql.AppendLine("");

                    //RF
                    oSql.AppendLine("		    DROP TABLE " + tC_TblRefund + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalRC + " FROM TPSTSalRC WITH(NOLOCK)");

                    //RC
                    oSql.AppendLine("		    DROP TABLE " + tC_TblSalRC + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalRC + " FROM TPSTSalRC WITH(NOLOCK)");

                    //RD    //*Arm 63-03-12
                    oSql.AppendLine("		    DROP TABLE " + tC_TblSalRD + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalRD + " FROM TPSTSalRD WITH(NOLOCK)");

                    //PD    //*Zen 63-03-24
                    oSql.AppendLine("		    DROP TABLE " + tC_TblSalPD + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalPD + " FROM TPSTSalPD WITH(NOLOCK)");

                    //MemTxnSal  
                    oSql.AppendLine("		    DROP TABLE " + tC_TblTxnSal + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblTxnSal + " FROM TCNTMemTxnSale WITH(NOLOCK)");

                    //MemTxnRedeem   
                    oSql.AppendLine("		    DROP TABLE " + tC_TblTxnRD + "");
                    //oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblTxnRD + " FROM TCNTMemTxnRedeem WITH(NOLOCK)");

                    oSql.AppendLine("    COMMIT TRAN");
                    oSql.AppendLine("    SELECT 1"); //*Net 63-06-05 Return when Success
                    oSql.AppendLine("END TRY");
                    oSql.AppendLine("BEGIN CATCH");
                    oSql.AppendLine("    ROLLBACK TRAN");
                    oSql.AppendLine("    SELECT 0"); //*Net 63-06-05 Return when Fail
                    oSql.AppendLine("END CATCH");

                    if (oDB.C_GEToDataQuery<int>(oSql.ToString()) == 0)
                    {
                        new cLog().C_WRTxLog("cSale", $"C_PRCxCheckDropTemp : Cannot Drop Temp !!!!"); //*Net Stamp
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxCheckDropTemp : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Transfer data from template to transaction after bill complete.
        /// </summary>
        public static void C_PRCxTemp2Transaction()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("    BEGIN TRAN");
                // **Net 63-06-01
                oSql.AppendLine("    INSERT INTO TCNTMemTxnRedeem "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT TRD.*");
                oSql.AppendLine("    FROM " + tC_TblTxnRD + " TRD WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON TRD.FTMemCode = HD.FTCstCode AND TRD.FTRedRefInt = HD.FTXshRefInt");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1' AND HD.FNXshDocType<>'1'");
                oSql.AppendLine("");
                // *Net 63-06-01
                oSql.AppendLine("    INSERT INTO TCNTMemTxnSale "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT TSL.*");
                oSql.AppendLine("    FROM " + tC_TblTxnSal + " TSL WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON TSL.FTMemCode = HD.FTCstCode AND TSL.FTTxnRefInt = HD.FTXshRefInt");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1' AND HD.FNXshDocType<>'1'");
                oSql.AppendLine("");
                // **Net 63-06-01
                oSql.AppendLine("    INSERT INTO TCNTMemTxnRedeem "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT TRD.*");
                oSql.AppendLine("    FROM " + tC_TblTxnRD + " TRD WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON TRD.FTMemCode = HD.FTCstCode AND TRD.FTRedRefDoc = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1' AND HD.FNXshDocType='1'");
                oSql.AppendLine("");
                // *Net 63-06-01
                oSql.AppendLine("    INSERT INTO TCNTMemTxnSale "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT TSL.*");
                oSql.AppendLine("    FROM " + tC_TblTxnSal + " TSL WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON TSL.FTMemCode = HD.FTCstCode AND TSL.FTTxnRefDoc = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1' AND HD.FNXshDocType='1'");
                oSql.AppendLine("");
                // *Zen 63-03-25 
                oSql.AppendLine("    INSERT INTO TPSTSalPD "); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT PD.*");
                oSql.AppendLine("    FROM " + tC_TblSalPD + " PD WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON PD.FTBchCode = HD.FTBchCode AND PD.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                //++++++++++++++
                // *Arm 63-03-13 
                oSql.AppendLine("    INSERT INTO TPSTSalRD ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT RD.*");
                oSql.AppendLine("    FROM " + tC_TblSalRD + " RD WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON RD.FTBchCode = HD.FTBchCode AND RD.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                //++++++++++++++
                oSql.AppendLine("    INSERT INTO TPSTSalRC ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT RC.*");
                oSql.AppendLine("    FROM " + tC_TblSalRC + " RC WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON RC.FTBchCode = HD.FTBchCode AND RC.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalDT ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT DT.*");
                oSql.AppendLine("    FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalDTDis ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT DTDis.*");
                oSql.AppendLine("    FROM " + tC_TblSalDTDis + " DTDis WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DTDis.FTBchCode = HD.FTBchCode AND DTDis.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                //*Net 63-07-30 ปรับตาม Moshi ไม่ใช้ตารางแล้ว
                //oSql.AppendLine("    INSERT INTO TPSTSalDTPmt ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                //oSql.AppendLine("    SELECT DTPmt.*");
                //oSql.AppendLine("    FROM " + tC_TblSalDTPmt + " DTPmt WITH(NOLOCK)");
                //oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DTPmt.FTBchCode = HD.FTBchCode AND DTPmt.FTXshDocNo = HD.FTXshDocNo");
                //oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalHDDis ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT HDDis.*");
                oSql.AppendLine("    FROM " + tC_TblSalHDDis + " HDDis WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON HDDis.FTBchCode = HD.FTBchCode AND HDDis.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalHDCst ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT HDCst.*");
                oSql.AppendLine("    FROM " + tC_TblSalHDCst + " HDCst WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON HDCst.FTBchCode = HD.FTBchCode AND HDCst.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalHD ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("    SELECT *");
                oSql.AppendLine("    FROM " + tC_TblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("    WHERE ISNULL(FTXshStaDoc, '') = '1'");
                oSql.AppendLine("");
                // *Net 63-06-01
                oSql.AppendLine("    DELETE TRD WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblTxnRD + " TRD");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON TRD.FTMemCode = HD.FTCstCode AND TRD.FTRedRefInt = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1' AND HD.FNXshDocType<>'1'");
                oSql.AppendLine("");
                //++++++++++++++
                // *Net 63-06-01
                oSql.AppendLine("    DELETE TSL WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblTxnSal + " TSL");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON TSL.FTMemCode = HD.FTCstCode AND TSL.FTTxnRefInt = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1' AND HD.FNXshDocType<>'1'");
                oSql.AppendLine("");
                //++++++++++++++
                // *Net 63-06-01
                oSql.AppendLine("    DELETE TRD WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblTxnRD + " TRD");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON TRD.FTMemCode = HD.FTCstCode AND TRD.FTRedRefDoc = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1' AND HD.FNXshDocType='1'");
                oSql.AppendLine("");
                //++++++++++++++
                // *Net 63-06-01
                oSql.AppendLine("    DELETE TSL WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblTxnSal + " TSL");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON TSL.FTMemCode = HD.FTCstCode AND TSL.FTTxnRefDoc = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1' AND HD.FNXshDocType='1'");
                oSql.AppendLine("");
                //++++++++++++++
                // *Zen 63-03-25
                oSql.AppendLine("    DELETE PD WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblSalPD + " PD");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON PD.FTBchCode = HD.FTBchCode AND PD.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                //++++++++++++++
                // *Arm 63-03-13
                oSql.AppendLine("    DELETE RD WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblSalRD + " RD");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON RD.FTBchCode = HD.FTBchCode AND RD.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                //++++++++++++++
                oSql.AppendLine("    DELETE RC WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblSalRC + " RC");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON RC.FTBchCode = HD.FTBchCode AND RC.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    DELETE DT WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblSalDT + " DT");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    DELETE DTDis WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblSalDTDis + " DTDis");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DTDis.FTBchCode = HD.FTBchCode AND DTDis.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                //*Net 63-07-30 ปรับตาม Moshi ไม่ใช้ตารางแล้ว
                //oSql.AppendLine("    DELETE DTPmt WITH(ROWLOCK)");
                //oSql.AppendLine("    FROM " + tC_TblSalDTPmt + " DTPmt");
                //oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DTPmt.FTBchCode = HD.FTBchCode AND DTPmt.FTXshDocNo = HD.FTXshDocNo");
                //oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    DELETE HDDis WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblSalHDDis + " HDDis");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON HDDis.FTBchCode = HD.FTBchCode AND HDDis.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    DELETE HDCst WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblSalHDCst + " HDCst");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(ROWLOCK) ON HDCst.FTBchCode = HD.FTBchCode AND HDCst.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    DELETE FROM " + tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("    WHERE ISNULL(FTXshStaDoc, '') = '1'");
                oSql.AppendLine("    COMMIT TRAN");
                oSql.AppendLine("    SELECT 1"); //*Net 63-06-05 Return when Success
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("    ROLLBACK TRAN");
                oSql.AppendLine("    SELECT 0"); //*Net 63-06-05 Return when Fail
                oSql.AppendLine("END CATCH");
                //oDB.C_SETxDataQuery(oSql.ToString()); //*Net 63-06-05 Write Log When Fail
                if (oDB.C_GEToDataQuery<int>(oSql.ToString()) == 0)
                {
                    new cLog().C_WRTxLog("cSale", $"C_PRCxTemp2Transaction : Cannot Move Temp 2 Transaction !!!!"); //*Net Stamp
                }

                //*Net 63-07-30 ปรับตาม Moshi
                C_PRCxCheckDropTemp(); //*Net 63-07-02 drop ตาราง Tmp ทิ้ง

                //*Em 63-05-16
                //oUpload = new Thread(new cSyncData().C_PRCxSyncUld);
                //oUpload = new Thread(() => new cSyncData().C_PRCxSyncUld()); //*Net 63-05-24
                //oUpload.IsBackground = true;
                //oUpload.Priority = ThreadPriority.Highest;
                //oUpload.Start();
                //++++++++++++++++++


                //*Em 63-07-11
                oSql.Clear();
                oSql.AppendLine("DBCC DBREINDEX ('TPSTSalHD', '',70)");
                oSql.AppendLine("DBCC DBREINDEX ('TPSTSalDT', '',70)");
                oSql.AppendLine("DBCC DBREINDEX ('TPSTSalRC', '',70)");
                oDB.C_SETxDataQuery(oSql.ToString());
                //+++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxTemp2Transaction : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        /// <summary>
        /// *Arm 63-03-24
        /// </summary>
        public static void C_PRCxVoidRefun()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();

            try
            {

                int nLastSeq = 0;

                //oSql.Clear();
                //oSql.AppendLine("SELECT MAX(FNVidNo) AS FNVidNo FROM TPSTVoidDT WITH(NOLOCK)");
                //nLastSeq = oDB.C_GEToDataQuery<int>(oSql.ToString());

                //nLastSeq = nLastSeq + 1;
                nLastSeq = nC_LastDocVoid + 1;  //*Em 63-05-15

                oSql.Clear();
                oSql.AppendLine("INSERT INTO TPSTVoidDT ( ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("FTBchCode,	FNVidNo, FNXidSeqNo, FTVidType, FTRsnCode");
                oSql.AppendLine(", FTXihDocNo,	FTXihDocType,	FDXihDocDate,	FTXihDocTime,	FCXidB4DisChg ");
                oSql.AppendLine(", FCXidNet, FCXidNetTotal, FCXidVat, FCXidVatable, FTXidRmk ");
                oSql.AppendLine(",FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy ");
                oSql.AppendLine(") ");
                oSql.AppendLine("SELECT FTBchCode,	'" + nLastSeq + "' AS FNVidNo,	'1', '3' AS FTVidType, '" + cVB.oVB_Reason.FTRsnCode + "' AS FTRsnCode");
                oSql.AppendLine(", FTXshDocNo, FNXshDocType, FDXshDocDate, CONVERT(VARCHAR(8), FDXshDocDate, 108) AS FTXihDocTime, FCXshTotalB4DisChgV");
                oSql.AppendLine(", FCXshTotalAfDisChgV, FCXshGrand, FCXshVat, FCXshVatable, FTXshRmk");
                oSql.AppendLine(", GETDATE(),  FTLastUpdBy,	GETDATE(), FTCreateBy ");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

                nC_LastDocVoid = nC_LastDocVoid + 1;  //*Em 63-05-15
                C_PRCxAdd2TmpLogChg(82, nLastSeq.ToString());
                //new cSyncData().C_PRCxSyncUld(); //*Net 63-05-14 ไปส่งทีเดียวตอนเสร็จ SetComplete

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxVoidRefun : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        public static void C_PRCxVoidBill()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            wReason oReason = null;
            Form oFormShow = null;
            cmlTPSTShiftEvent oEvent;
            try
            {
                //if (new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 4) )


                if (cSale.nC_CntItem == 0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                    return;
                }

                if (MessageBox.Show(cVB.oVB_GBResource.GetString("tMsgVoidBill"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                oReason = new wReason("002");
                oReason.ShowDialog();
                if (cVB.oVB_Reason == null) return;
                int nLastSeq = 0;
                //oSql.AppendLine("SELECT MAX(FNVidNo) AS FNVidNo FROM TPSTVoidDT WITH(NOLOCK)");
                //nLastSeq = oDB.C_GEToDataQuery<int>(oSql.ToString());

                //nLastSeq = nLastSeq + 1;
                nLastSeq = nC_LastDocVoid + 1;  //*Em 63-05-15

                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO TPSTVoidDT (FTBchCode, FNVidNo, FNXidSeqNo, FTVidType, FTRsnCode, FTXihDocNo, FTXihDocType, FDXihDocDate, FTXihDocTime, FTPdtCode,"); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                //oSql.AppendLine("FTXidPdtName, FTXidStkCode, FTPunCode, FCXidFactor, FTXidBarCode, FTSrnCode, FTXidVatType, FTVatCode, FCXidVatRate,");
                oSql.AppendLine("FTXidPdtName, FTPunCode, FCXidFactor, FTXidBarCode, FTSrnCode, FTXidVatType, FTVatCode, FCXidVatRate,");   //*Em 62-06-26
                oSql.AppendLine("FTXidSaleType, FCXidSalePrice, FCXidQty, FCXidSetPrice, FCXidB4DisChg, FCXidQtyAll, FCXidNet, FCXidNetTotal, FCXidVat, FCXidVatable,");
                oSql.AppendLine("FCXidCostIn, FCXidCostEx, FTXidStaPdt, FCXidQtyLef, FCXidQtyRfn, FTXidStaPrcStk, FNXidPdtLevel, FTXidPdtParent, FCXidQtySet, FTPdtStaSet,");
                oSql.AppendLine("FTXidRmk, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("SELECT DT.FTBchCode," + nLastSeq + " AS FNVidNo, FNXsdSeqNo,'2' AS FTVidType, '" + cVB.oVB_Reason.FTRsnCode + "' AS FTRsnCode, DT.FTXshDocNo, HD.FNXshDocType, FDXshDocDate, CONVERT(VARCHAR(8), FDXshDocDate, 108) AS FTXihDocTime, DT.FTPdtCode, ");
                //oSql.AppendLine("FTXsdPdtName, FTXsdStkCode, FTPunCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");
                oSql.AppendLine("FTXsdPdtName, FTPunCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");
                oSql.AppendLine("FTXsdSaleType, FCXsdSalePrice, FCXsdQty, FCXsdSetPrice, (FCXsdQty * FCXsdSetPrice) AS FCXsdB4DisChg, FCXsdQtyAll, FCXsdNet, FCXsdNet AS FCXsdNetTotal, FCXsdVat, FCXsdVatable,");
                oSql.AppendLine("FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet,");
                oSql.AppendLine("FTXsdRmk, GETDATE() AS FDLastUpdOn, DT.FTLastUpdBy, GETDATE() AS FDCreateOn, DT.FTCreateBy");
                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " DT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalHD + " HD WITH(NOLOCK) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("WHERE HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("");
                oSql.AppendLine("INSERT INTO TPSTVoidDTDis (FTBchCode, FNVidNo, FNXidSeqNo, FDXddDateIns, FTXihDocNo, FTXddDisChgTxt, FNXddStaDis, FTXddDisChgType, FCXddNet, FCXddValue)"); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("SELECT DT.FTBchCode," + nLastSeq + " AS FNVidNo, DT.FNXsdSeqNo, DT.FDXddDateIns, DT.FTXshDocNo, DT.FTXddDisChgTxt, DT.FNXddStaDis, DT.FTXddDisChgType, DT.FCXddNet, DT.FCXddValue");
                oSql.AppendLine("FROM " + tC_TblSalDTDis + " DT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalHD + " HD WITH(NOLOCK) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("WHERE HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

                oEvent = new cmlTPSTShiftEvent();
                oEvent.FTBchCode = cVB.tVB_BchCode;
                oEvent.FTShfCode = cVB.tVB_ShfCode;
                oEvent.FTPosCode = cVB.tVB_PosCode; //*Em 62-01-03  เพิ่ม FTPosCode
                oEvent.FNSdtSeqNo = cVB.nVB_ShfSeq; //*Em 62-08-15
                oEvent.FDHisDateTime = DateTime.Now;
                oEvent.FTEvnCode = "006";
                oEvent.FNSvnQty = 1;
                oEvent.FCSvnAmt = 0;
                oEvent.FTRsnCode = cVB.oVB_Reason.FTRsnCode;
                oEvent.FTSvnApvCode = cVB.tVB_UsrCode;
                oEvent.FTSvnRemark = "";
                new cShiftEvent().C_INSxShiftEvent(oEvent);

                //*Net 63-06-17 ตั้งค่า DocNoPrint
                cVB.tVB_DocNoPrn = cVB.tVB_DocNo;
                cSale.bC_PrnCopy = true;
                cSale.tC_RefDocNoPrn = cVB.tVB_RefDocNo;
                cSale.tC_TxnRefCode = (String.IsNullOrEmpty(cVB.tVB_RefDocNo)) ? cVB.tVB_DocNo : cVB.tVB_RefDocNo;
                cSale.nC_PrnDocType = cSale.nC_DocType;
                //+++++++++++++++++++++++
                C_PRCxPrintVoldBill(); //*Arm 63-03-05 [Comment code] Update ตาม Baseline

                ////*Em 63-03-02 , *Arm 63-03-05 Update ตาม Baseline
                //Thread oPrn = new Thread(new ThreadStart(C_PRCxPrintVoldBill));
                //oPrn.Start();
                ////+++++++++++++
                nC_LastDocVoid = nC_LastDocVoid + 1;  //*Em 63-05-15
                C_PRCxAdd2TmpLogChg(82, nLastSeq.ToString()); //*Em 62-08-23
                new cSyncData().C_PRCxSyncUld();    //*Em 62-08-24 //*Net 63-05-30 อัพโหลดตอนยกเลิกบิล


                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSale);
                //new wSale(3).Show();

                //*Net 63-07-30 ปรับตาม Moshi
                cSale.C_PRCxClearGenNew();  //*Em 63-07-11
                cVB.oVB_Sale.W_SETxNewDoc();    //*Em 63-02-03, *Arm 63-03-05 Update ตาม Baseline
                cVB.oVB_Sale.bW_Activate = false;
                cVB.oVB_Sale.Show();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxVoidBill : " + oEx.Message);
            }
            finally
            {
                if (oFormShow != null)
                    oFormShow.Close();

                if (oReason != null)
                    oReason.Close();

                oDB = null;
                oSql = null;
                oFormShow = null;
                oReason = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

        }

        public static void C_PRCxVoidItem()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            wReason oReason = null;
            Form oFormShow = null;

            try
            {
                // 63-02-06 Zen แก้เรื่อง Void สินค้าโดยไม่ต้องเปิดหน้า ถามสาเหตุ แต่ให้ บันทึกลง Void
                //oReason = new wReason("001");
                //oReason.ShowDialog();
                //if (cVB.oVB_Reason == null) return;
                int nLastSeq = 0;
                //oSql.AppendLine("SELECT MAX(FNVidNo) AS FNVidNo FROM TPSTVoidDT WITH(NOLOCK)");
                //nLastSeq = oDB.C_GEToDataQuery<int>(oSql.ToString());

                //nLastSeq = nLastSeq + 1;
                nLastSeq = nC_LastDocVoid + 1;  //*Em 63-05-15
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO TPSTVoidDT (FTBchCode, FNVidNo, FNXidSeqNo, FTVidType, FTRsnCode, FTXihDocNo, FTXihDocType, FDXihDocDate, FTXihDocTime, FTPdtCode,"); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                //oSql.AppendLine("FTXidPdtName, FTXidStkCode, FTPunCode, FCXidFactor, FTXidBarCode, FTSrnCode, FTXidVatType, FTVatCode, FCXidVatRate,");
                oSql.AppendLine("FTXidPdtName, FTPunCode, FCXidFactor, FTXidBarCode, FTSrnCode, FTXidVatType, FTVatCode, FCXidVatRate,");   //*Em 62-06-26
                oSql.AppendLine("FTXidSaleType, FCXidSalePrice, FCXidQty, FCXidSetPrice, FCXidB4DisChg, FCXidQtyAll, FCXidNet, FCXidNetTotal, FCXidVat, FCXidVatable,");
                oSql.AppendLine("FCXidCostIn, FCXidCostEx, FTXidStaPdt, FCXidQtyLef, FCXidQtyRfn, FTXidStaPrcStk, FNXidPdtLevel, FTXidPdtParent, FCXidQtySet, FTPdtStaSet,");
                oSql.AppendLine("FTXidRmk, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("SELECT DT.FTBchCode," + nLastSeq + " AS FNVidNo, FNXsdSeqNo,'1' AS FTVidType, '001' AS FTRsnCode, DT.FTXshDocNo, HD.FNXshDocType, FDXshDocDate, CONVERT(VARCHAR(8), FDXshDocDate, 108) AS FTXihDocTime, DT.FTPdtCode, ");
                // บรรทัดเก่า Void Item โดยการเลือก สาเหตุการ Void อันใหม่ Fix ไว้ที่ 001 
                //oSql.AppendLine("SELECT DT.FTBchCode," + nLastSeq + " AS FNVidNo, FNXsdSeqNo,'1' AS FTVidType, '" + cVB.oVB_Reason.FTRsnCode + "' AS FTRsnCode, DT.FTXshDocNo, HD.FNXshDocType, FDXshDocDate, CONVERT(VARCHAR(8), FDXshDocDate, 108) AS FTXihDocTime, DT.FTPdtCode, ");
                //oSql.AppendLine("FTXsdPdtName, FTXsdStkCode, FTPunCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");
                oSql.AppendLine("FTXsdPdtName, FTPunCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");  //*Em 62-06-26
                oSql.AppendLine("FTXsdSaleType, FCXsdSalePrice, FCXsdQty, FCXsdSetPrice, (FCXsdQty * FCXsdSetPrice) AS FCXsdB4DisChg, FCXsdQtyAll, FCXsdNet, FCXsdNet AS FCXsdNetTotal, FCXsdVat, FCXsdVatable,");
                oSql.AppendLine("FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet,");
                oSql.AppendLine("FTXsdRmk, GETDATE() AS FDLastUpdOn, DT.FTLastUpdBy, GETDATE() AS FDCreateOn, DT.FTCreateBy");
                oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " DT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalHD + " HD WITH(NOLOCK) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("WHERE HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND DT.FNXsdSeqNo = " + nC_DTSeqNo); //*Net 63-06-16 where seq ด้วย
                oSql.AppendLine("");
                oSql.AppendLine("INSERT INTO TPSTVoidDTDis (FTBchCode, FNVidNo, FNXidSeqNo, FDXddDateIns, FTXihDocNo, FTXddDisChgTxt, FNXddStaDis, FTXddDisChgType, FCXddNet, FCXddValue)"); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("SELECT DT.FTBchCode," + nLastSeq + " AS FNVidNo, DT.FNXsdSeqNo, DT.FDXddDateIns, DT.FTXshDocNo, DT.FTXddDisChgTxt, DT.FNXddStaDis, DT.FTXddDisChgType, DT.FCXddNet, DT.FCXddValue");
                oSql.AppendLine("FROM " + tC_TblSalDTDis + " DT WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalHD + " HD WITH(NOLOCK) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("WHERE HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND DT.FNXsdSeqNo = " + nC_DTSeqNo);
                oDB.C_SETxDataQuery(oSql.ToString());

                //update รายการ void
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FTXsdStaPdt = '4'");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);
                oDB.C_SETxDataQuery(oSql.ToString());

                //insert dt รายการ void
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + tC_TblSalDT + "  (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, ");
                //oSql.AppendLine("FTXsdStkCode, FTPunCode, FTPunName, FCXsdFactor,");
                oSql.AppendLine("FTPunCode, FTPunName, FCXsdFactor,");  //*Em 62-06-26
                oSql.AppendLine("FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate,");
                oSql.AppendLine("FTXsdSaleType, FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, ");
                oSql.AppendLine("FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet,");
                oSql.AppendLine("FCXsdNetAfHD, FCXsdVat, FCXsdVatable, ");
                oSql.AppendLine("FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, ");
                oSql.AppendLine("FCXsdQtyRfn, FTXsdStaAlwDis, ");
                oSql.AppendLine("FTPdtStaSet, FTXsdRmk, ");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, ");
                oSql.AppendLine("FDCreateOn, FTCreateBy");
                oSql.AppendLine(")");
                oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, " + (cSale.nC_CntItem + 1) + " AS FNXsdSeqNo, FTPdtCode, 'Void-' + FTXsdPdtName AS  FTXsdPdtName, ");
                //oSql.AppendLine("FTXsdStkCode, FTPunCode, FTPunName, FCXsdFactor,");
                oSql.AppendLine("FTPunCode, FTPunName, FCXsdFactor,");  //*Em 62-06-26
                oSql.AppendLine("FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate,");
                oSql.AppendLine("FTXsdSaleType, FCXsdSalePrice, (-1)*FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, ");
                oSql.AppendLine("FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, (-1)*FCXsdNet,");
                oSql.AppendLine("FCXsdNetAfHD, FCXsdVat, FCXsdVatable, ");
                oSql.AppendLine("FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, ");
                oSql.AppendLine("FCXsdQtyRfn, FTXsdStaAlwDis, ");
                oSql.AppendLine("FTPdtStaSet, FTXsdRmk, ");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, ");
                oSql.AppendLine("FDCreateOn, FTCreateBy");
                oSql.AppendLine("FROM " + tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);
                oDB.C_SETxDataQuery(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT (-1)*FCXsdQty AS cQty, FCXsdFactor AS cFactor, FTPunName AS tUnit,");
                oSql.AppendLine("FTPdtCode AS tPdtCode, FTXsdBarCode AS tBarcode, 'Void-' + FTXsdPdtName AS tPdtName,");
                oSql.AppendLine("FCXsdSetPrice AS cSetPrice, '4' AS tStaPdt,FCXsdDis AS cDis, FCXsdChg AS cChg"); //*Net 63-06-05 เอาลดชาร์จออกมาด้วย
                oSql.AppendLine("FROM " + tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);
                cmlPdtOrder oPdtOrder = oDB.C_GEToDataQuery<cmlPdtOrder>(oSql.ToString());
                cVB.oVB_Sale.W_ADDxPdtToOrder(oPdtOrder);
                nC_LastDocVoid = nC_LastDocVoid + 1;  //*Em 63-05-15
                C_PRCxAdd2TmpLogChg(82, nLastSeq.ToString()); //*Em 62-08-23
                new cSyncData().C_PRCxSyncUld();    //*Em 62-08-24 //*Net 63-05-30 อัพโหลดตอนยกเลิกรายการ
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxVoidItem : " + oEx.Message);
            }
            finally
            {
                if (oFormShow != null)
                    oFormShow.Close();

                if (oReason != null)
                    oReason.Close();

                oDB = null;
                oSql = null;
                oFormShow = null;
                oReason = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static bool C_PRCbSetComplete()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            //Form oFormShow = null;
            try
            {
                //*Net 63-07-30 ปรับตาม Moshi
                //*Net 63-07-31 แสดงข้อความขอบคุณเมื่อจบบิล
                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_SETxThanks();
                }

                C_DATxUpdVat(); //*Em 63-07-22

                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxSummary2HD Start", cVB.bVB_AlwPrnLog);
                C_PRCxSummary2HD();
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxSummary2HD End", cVB.bVB_AlwPrnLog);

                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update hd Start", cVB.bVB_AlwPrnLog);
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FTXshStaDoc = '1'");
                oSql.AppendLine(",FTXshStaPaid = '3'");
                //*Net 63-07-30 ปรับตาม Moshi
                ////*Em 63-05-21
                //oSql.AppendLine(",FCXshVat = ROUND(((" + oC_SalHD.FCXshGrand + "*" + cVB.cVB_VatRate + ")/(100+" + cVB.cVB_VatRate + "))," + cVB.nVB_DecSave + ")");
                //oSql.AppendLine(",FCXshVatable = " + oC_SalHD.FCXshGrand + " - ROUND(((" + oC_SalHD.FCXshGrand + "*" + cVB.cVB_VatRate + ")/(100+" + cVB.cVB_VatRate + "))," + cVB.nVB_DecSave + ")");
                ////++++++++++++++++++
                //*Em 63-07-26 //*Net 63-08-06 เปลี่ยนจาก FCXshTotalAfDisChgV เป็น FCXshAmtV
                oSql.AppendLine($",FCXshVat = ");
                oSql.AppendLine($"  CASE WHEN FTXshVATInOrEx='1' THEN ROUND( ( (FCXshAmtV) *{cVB.cVB_VatRate}/{100m + cVB.cVB_VatRate}) ,{cVB.nVB_DecSave})");
                oSql.AppendLine($"  ELSE ROUND( (FCXshAmtV)*{cVB.cVB_VatRate / 100m} ,{cVB.nVB_DecSave})");
                oSql.AppendLine($"  END ");
                oSql.AppendLine($",FCXshVatable = ");
                oSql.AppendLine($"  CASE WHEN FTXshVATInOrEx='1' THEN FCXshAmtV- ROUND( ( (FCXshAmtV) *{cVB.cVB_VatRate}/{100m + cVB.cVB_VatRate}) ,{cVB.nVB_DecSave})");
                oSql.AppendLine($"  ELSE FCXshAmtV- ROUND( (FCXshAmtV)*{cVB.cVB_VatRate / 100m} ,{cVB.nVB_DecSave})");
                oSql.AppendLine($"  END ");
                //+++++++++++++++++
                //oSql.AppendLine(",FCXshGrand = " + (decimal)(oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff)); //*Arm 63-09-01 Comment Code
                //oSql.AppendLine(",FCXshPaid = " + (decimal)(oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff));  //*Net 63-03-28 ยกมาจาก baseline //*Arm 63-09-01 Comment Code
                oSql.AppendLine(",FCXshGrand = " + Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, (decimal)(oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff), 2))); //*Arm 63-09-01 ปัดเศษทศนิยม 2 ตำแหน่ง
                oSql.AppendLine(",FCXshPaid = " + Convert.ToDecimal(new cSP().SP_SETtDecShwSve(2, (decimal)(oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff), 2)));  //*Arm 63-09-01 ปัดเศษทศนิยม 2 ตำแหน่ง

                oSql.AppendLine(",FCXshRnd = " + cVB.cVB_RoundDiff);
                oSql.AppendLine(",FTXshGndText = '" + (cVB.nVB_Language == 1 ? C_GETtGndTextTH(((decimal)oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff).ToString()) : C_GETtGndTextEN((decimal)oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff).ToString()) + "'");
                oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine(",FNXshDocPrint = '1'");

                //*Arm 63-02-19
                if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                {
                    if (nC_DocType != 9)
                    {
                        oSql.AppendLine(",FTXshRefExt = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-03-05 - เลขที่เอกสาร SO
                    }
                }
                //+++++++++++++++

                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND FNXshDocType = " + oC_SalHD.FNXshDocType);
                oDB.C_SETxDataQuery(oSql.ToString());


                cVB.cVB_MonitorSalGrand = (decimal)(oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff); //*Net 63-05-18 - มอนิเตอร์ยอดขายต่อบิล
                cVB.dVB_TimeStamp = DateTime.Now;  //*Net 63-05-18 - มอนิเตอร์วันที่บิล
                cVB.tVB_MonitorDocNo = cVB.tVB_DocNo;//*Net 63-05-18 - มอนิเตอร์เลขที่บิล

                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update hd End", cVB.bVB_AlwPrnLog);

                //*Net 63-06-01 ย้าย
                //*Em 63-05-21
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update vat last start", cVB.bVB_AlwPrnLog);
                C_DATxProrateVatFrmHD();
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update vat last End", cVB.bVB_AlwPrnLog);

                //*Em 63-05-27
                //ตรวจสอบบิลสมบูรณ์
                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : Check bill complete start");
                //oSql.Clear();
                //oSql.AppendLine("SELECT HD.FTXshDocNo FROM " + tC_TblSalHD + " HD WITH(NOLOCK)");
                //oSql.AppendLine("LEFT JOIN (SELECT FTBchCode,FTXshDocNo,SUM(FCXsdNetAfHD) FCXsdNetAfHD  FROM " + tC_TblSalDT + " WITH(NOLOCK) ");
                //oSql.AppendLine("			WHERE FTXsdStaPdt <> '4' AND FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oSql.AppendLine("			GROUP BY FTBchCode,FTXshDocNo ) DT ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo ");
                //oSql.AppendLine("LEFT JOIN (SELECT FTBchCode ,FTXshDocNo, SUM(FCXrcNet) FCXrcNet FROM " + tC_TblSalRC + " WITH(NOLOCK)");
                //oSql.AppendLine("			WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oSql.AppendLine("			AND FTRcvCode NOT IN (SELECT FTRcvCode FROM TFNMRcv WITH(NOLOCK) WHERE FTFmtCode IN('020','022'))");
                //oSql.AppendLine("			GROUP BY FTBchCode,FTXshDocNo ) RC ON RC.FTBchCode = HD.FTBchCode AND RC.FTXshDocNo = HD.FTXshDocNo ");
                //oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "' AND HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oSql.AppendLine("AND ((HD.FCXshGrand - HD.FCXshRnd) <> ISNULL(DT.FCXsdNetAfHD,0) OR HD.FCXshGrand <> ISNULL(RC.FCXrcNet,0))");
                //if (!string.IsNullOrEmpty(oDB.C_GEToDataQuery<string>(oSql.ToString())))
                //{
                //    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgBillNotComplete"), 2);
                //    oSql.Clear();
                //    oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK)");
                //    oSql.AppendLine("SET FTXshStaDoc = '2'");
                //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //    oSql.AppendLine("AND FNXshDocType = " + oC_SalHD.FNXshDocType);
                //    oDB.C_SETxDataQuery(oSql.ToString());
                //    return false;
                //}

                oSql.Clear();
                //oSql.AppendLine("SELECT FNCntHD,FNCntDT,FNCntRC");
                oSql.AppendLine("SELECT FNCntDT,FNCntRC"); //*Net 63-06-01 ตัดการ Select HD ออก
                oSql.AppendLine("FROM");
                //oSql.AppendLine("(SELECT COUNT(FTXshDocNo) AS FNCntHD FROM " + tC_TblSalHD + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "') HD,"); //*Net 63-06-01
                oSql.AppendLine("(SELECT COUNT(FTXshDocNo) AS FNCntDT FROM " + tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "') DT,");
                oSql.AppendLine("(SELECT COUNT(FTXshDocNo) AS FNCntRC FROM " + tC_TblSalRC + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "') RC");
                DataTable odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    if (odtTmp.Rows[0].Field<int>("FNCntDT") == 0 || odtTmp.Rows[0].Field<int>("FNCntRC") == 0)
                    {
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgBillNotComplete"), 2);
                        oSql.Clear();
                        oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK)");
                        oSql.AppendLine("SET FTXshStaDoc = '2'");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                        oSql.AppendLine("AND FNXshDocType = " + oC_SalHD.FNXshDocType);
                        oDB.C_SETxDataQuery(oSql.ToString());
                        return false;
                    }
                }
                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : Check bill complete End");
                //+++++++++++++++++

                ////*Em 63-05-21 //*Net 63-06-01 ย้ายไปเป็น C_DATxProrateVatFrmHD
                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update vat start");
                //oSql.Clear();
                //oSql.AppendLine("UPDATE DT WITH(ROWLOCK)");
                //oSql.AppendLine("SET FCXsdVat = HD.FCXshVat - ISNULL((SELECT SUM(FCXsdVat) FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo <> DT.FNXsdSeqNo AND FTXsdVatType = '1' AND FTXsdStaPdt <> '4'),0)");
                //oSql.AppendLine(",FCXsdVatable = FCXsdNetAfHD - (HD.FCXshVat - ISNULL((SELECT SUM(FCXsdVat) FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FNXsdSeqNo <> DT.FNXsdSeqNo AND FTXsdVatType = '1' AND FTXsdStaPdt <> '4'),0))");
                //oSql.AppendLine("FROM " + cSale.tC_TblSalDT + " DT");
                //oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalHD + " HD WITH(NOLOCK) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo");
                //oSql.AppendLine("INNER JOIN (SELECT FTBchCode,FTXshDocNo,MAX(FNXsdSeqNo) FNXsdSeqNo");
                //oSql.AppendLine("	FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK)");
                //oSql.AppendLine("	WHERE FTXsdVatType = '1' AND FTXsdStaPdt <> '4'");
                //oSql.AppendLine("   AND FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("   AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oSql.AppendLine("	GROUP BY FTBchCode,FTXshDocNo) Tmp ON Tmp.FTBchCode = DT.FTBchCode AND Tmp.FTXshDocNo = DT.FTXshDocNo AND Tmp.FNXsdSeqNo = DT.FNXsdSeqNo");
                //oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oDB.C_SETxDataQuery(oSql.ToString());
                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update vat End");

                //*Arm 63-04-16 คำนวณแต้ม
                if (nC_DocType != 9 && !string.IsNullOrEmpty(cVB.tVB_CstCode))
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxSalePoint Start", cVB.bVB_AlwPrnLog);
                    if (cVB.cVB_PntOptBuyAmt > 0 && cVB.cVB_PntOptGetQty > 0) //*Arm 63-08-17 ถ้าอัตราการให้แต้มเป็น 0 ไม่ต้องคำนวณ
                    {
                        C_PRCxSalePoint();
                    }
                    new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxSalePoint End", cVB.bVB_AlwPrnLog);
                }
                // ++++++++++++

                if (nC_DocType == 9)
                {
                    if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                    {
                        new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update refer table temp Start", cVB.bVB_AlwPrnLog);
                        oSql = new StringBuilder();
                        oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK)");
                        oSql.AppendLine("SET FTXshRefInt = '" + cVB.tVB_RefDocNo + "'");
                        oSql.AppendLine(",FDXshRefIntDate = GETDATE()");
                        oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                        oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                        oSql.AppendLine("AND FNXshDocType = " + oC_SalHD.FNXshDocType);
                        oDB.C_SETxDataQuery(oSql.ToString());
                        new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update refer table temp end", cVB.bVB_AlwPrnLog);

                        new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update refer table txn Start", cVB.bVB_AlwPrnLog);
                        oSql = new StringBuilder();
                        //if (cVB.bVB_RefundTrans)
                        //{
                        //    oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK)");
                        //    oSql.AppendLine("SET FTXshStaRefund = '2'");
                        //    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                        //    oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                        //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                        //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        //    oDB.C_SETxDataQuery(oSql.ToString());
                        //}
                        //else
                        //{
                        //    oSql.AppendLine("UPDATE " + tC_TblSalHD + " WITH(ROWLOCK)");
                        //    oSql.AppendLine("SET FTXshStaRefund = '2'");
                        //    oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                        //    oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                        //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                        //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                        //    oDB.C_SETxDataQuery(oSql.ToString());
                        //}

                        //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : update refer table txn end");
                        //*Arm 62-12-19 UPDATE Qty
                        //C_UPDxUpdateQty();
                        /*if (cVB.nVB_ReturnType == 3) C_UPDxUpdateQty();*/  //*Em 63-05-15

                        //# *Arm 63-06-10 Update บิลคืนที่อ้างอิง
                        if (cVB.bVB_RefundDataFrom == true)  //bVB_RefundDataFrom  true:คืนภายในเครื่องจุดขาย, false:คืนข้ามเครื่องจุดขาย
                        {
                            // Update บิลคืนที่อ้างอิง FTXshStaRefund = 2
                            oSql.Clear();
                            oSql.AppendLine("UPDATE " + tC_Ref_TblSalHD);
                            oSql.AppendLine("SET FTXshStaRefund = '2'");
                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                            oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                            oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                            oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                            if (cVB.nVB_ReturnType == 3)
                            {
                                C_UPDxUpdateQty();
                            }
                            else
                            {
                                // bVB_RefundTrans true:Transaction, false:Temp 
                                if (cVB.bVB_RefundTrans == true) 
                                {
                                    C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, true);
                                }
                                else
                                {
                                    C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, false);
                                }
                            }
                        }
                        else
                        {
                            //Thread Cal API2ARDoc
                            Thread oUpdSalRefer = new Thread(C_PRCxUploadUpdateSaleRefer);
                            oUpdSalRefer.IsBackground = true;
                            oUpdSalRefer.Priority = ThreadPriority.Highest;
                            oUpdSalRefer.Start();
                        }
                        //# End *Arm 63-06-10 Update บิลคืนที่อ้างอิง


                        new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCbCancelCoupon Start", cVB.bVB_AlwPrnLog);
                        cPayment.C_PRCbCancelCoupon(2, cVB.tVB_RefDocNo);   //*63-01-09
                        new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCbCancelCoupon End", cVB.bVB_AlwPrnLog);
                        
                        //*Arm 63-03-21  คืนแต้ม
                        if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                        {
                            new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxReturnSalePoint Start", cVB.bVB_AlwPrnLog);
                            //case KADS
                            //if คืนเต็มบิล เพิ่ม function
                            // else Call Ada  C_PRCxReturnSalePoint();
                            //case ADA
                            if (cVB.cVB_PntOptBuyAmt > 0 && cVB.cVB_PntOptGetQty > 0)  //*Arm 63-08-17 ถ้าอัตราการให้แต้มเป็น 0 ไม่ต้องคำนวณ
                            {
                                C_PRCxReturnSalePoint();
                            }
                            new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxReturnSalePoint end", cVB.bVB_AlwPrnLog);
                        }
                        //++++++++++++++

                        new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxVoidRefun Start", cVB.bVB_AlwPrnLog);
                        //C_PRCxVoidRefun(); //*Arm 63-03-24 //*Net 63-06-25 ไม่ต้องเอาลง VoidDT แล้ว ยกมาจาก Moshi
                        new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxVoidRefun end", cVB.bVB_AlwPrnLog);

                        cVB.cVB_MonitorSalGrand *= (-1); //*Net 63-05-18 - มอนิเตอร์ยอดขายต่อบิล

                    }

                }
                else
                {
                    //# กรณีมีการอ้างอิงบิลคืน Update บิลคืนที่อ้างอิง StaRef = 2 (*Arm 63-06-10)
                    if (cVB.oVB_ReferSO == null && !string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                    {
                        if (cVB.bVB_RefundDataFrom == true) //bVB_RefundDataFrom = true:ข้อมูลบิลคืนอ้างอิงจากเครื่อง, false:ข้อมูลบิลคืนอ้างอิงข้ามเครื่อง
                        {
                            // Update บิลคืนที่อ้างอิง StaRef = 2
                            oSql = new StringBuilder();
                            oSql.AppendLine("UPDATE " + tC_Ref_TblSalHD);
                            oSql.AppendLine("SET FNXshStaRef = '2'");
                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                            oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                            oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                            oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());

                            // bVB_RefundTrans true:Transaction, false:Temp
                            if (cVB.bVB_RefundTrans == true)
                            {
                                C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, true);
                            }
                            else
                            {
                                C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, false);
                            }

                        }
                        else
                        {
                            //Thread Cal API2ARDoc
                            Thread oUpdSalRefer = new Thread(C_PRCxUploadUpdateSaleRefer);
                            oUpdSalRefer.IsBackground = true;
                            oUpdSalRefer.Priority = ThreadPriority.Highest;
                            oUpdSalRefer.Start();
                        }

                    } //# End กรณีมีการอ้างอิงบิลคืน Update บิลคืนที่อ้างอิง StaRef = 2 (*Arm 63-06-10)
                }
                //C_PRCxPrintSlip();
                //C_PRCxTemp2Transaction(); //*Arm 63-03-05


                ////*Arm 63-03-21 คำนวณแต้ม
                //if (nC_DocType != 9 && !string.IsNullOrEmpty(cVB.tVB_CstCode))
                //{
                //    C_PRCxSalePoint();
                //}
                //// ++++++++++++

                cVB.nVB_CstPiontB4UsedPrn = cVB.nVB_CstPiontB4Used;

                nC_PrnDocType = nC_DocType;
                tC_RefDocNoPrn = cVB.tVB_RefDocNo;
                //*Em 63-03-02
                cVB.tVB_DocNoPrn = cVB.tVB_DocNo;

                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_OPNxCashDrawer start", cVB.bVB_AlwPrnLog);
                C_OPNxCashDrawer(); //*Arm 63-05-24 ย้ายมาทำก่อน PrintSlip
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_OPNxCashDrawer end", cVB.bVB_AlwPrnLog);

                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxPrintSlip Start");
                //C_PRCxPrintSlip();
                //oPrn = new Thread(C_PRCxPrintSlip);
                //oPrn = new Thread(() => C_PRCxPrintSlip(cVB.nVB_CstPiontB4UsedPrn, cVB.tVB_ExpiredDate));
                //oPrn.IsBackground = true;
                //oPrn.Priority = ThreadPriority.Highest;
                //oPrn.Start();
                //*Net 63-07-30 ปรับตาม Moshi
                C_PRCxPrintSlip(cVB.nVB_CstPiontB4UsedPrn, cVB.tVB_ExpiredDate);    //*Em 63-07-23
                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxPrintSlip end");
                //+++++++++++++++++
                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_OPNxCashDrawer start");
                //C_OPNxCashDrawer(); //*Em 62-08-16
                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_OPNxCashDrawer end");
                //C_PRCxAdd2TmpLogChg(80, cVB.tVB_DocNo, true);     //*Em 62-08-05, Arm 63-03-05 - เพิ่ม ส่ง parameter true 
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxAdd2TmpLogChg start", cVB.bVB_AlwPrnLog);
                C_PRCxAdd2TmpLogChg(80, cVB.tVB_DocNo, false);      //*Arm 63-05-17
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxAdd2TmpLogChg end", cVB.bVB_AlwPrnLog);
                //C_PRCxTemp2Transaction();
                //new cSyncData().C_PRCxSyncUld();    //*Em 62-08-05

                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : thread C_PRCxTemp2Transaction start");
                ////*Em 63-05-16
                //Thread oTran = new Thread(C_PRCxTemp2Transaction);
                //oTran.IsBackground = true;
                //oTran.Priority = ThreadPriority.Highest;
                //oTran.Start();
                ////++++++++++++++++++++
                //new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : thread C_PRCxTemp2Transaction end");

                //*Net 63-06-01 ย้ายมาจาก CalPoint
                new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : C_PRCxSendTxnSale Start", cVB.bVB_AlwPrnLog);
                oTxnSale = new Thread(C_PRCxSendTxnSale);
                oTxnSale.IsBackground = true;
                oTxnSale.Priority = ThreadPriority.Normal; //*Net 63-06-01 
                oTxnSale.Start();
                new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : C_PRCxSendTxnSale end", cVB.bVB_AlwPrnLog);
                //+++++++++++++++++++++++++++++++++++

                ////*Em 63-04-17 //*Net 63-05-30 อัพโหลดตอนจบบิล
                oUpload = new Thread(() => new cSyncData().C_PRCxSyncUld(true)); //*Net 63-05-24 Sync ตาราง Sal จาก Temp
                oUpload.IsBackground = true;
                oUpload.Priority = ThreadPriority.Normal;
                oUpload.Start();
                ////++++++++++++++++

                //*Arm 62-10-27 - Update Queue Member
                if (!string.IsNullOrEmpty(cVB.tVB_QMemMsgID))
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_UPDxPrcMsg start", cVB.bVB_AlwPrnLog);
                    new cMsgQueue().C_UPDxPrcMsg();
                    new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_UPDxPrcMsg start", cVB.bVB_AlwPrnLog);
                }

                ////*Arm 63-03-31 ClrarPara
                cVB.aoVB_PdtRdDocType1 = null;  //*Arm 63-03-31
                cVB.aoVB_PdtRdDocType2 = null;  //*Arm 63-03-31
                cVB.aoVB_PdtRefund = null;  //*Arm 63-03-31
                cVB.aoVB_PdtReferSO = null; //*Arm 63-03-31
                cVB.aoVB_PdtDisChgRefund = null; //*Arm 63-03-31

                //*Arm 63-09-11 -Clear ตาราง Refund
                bool bClrRf = false;
                if (nC_DocType == 9 && cVB.nVB_ReturnType != 1) bClrRf = true;
                if (nC_DocType == 1 && !string.IsNullOrEmpty(tC_RefDocNoPrn)) bClrRf = true;
                if (bClrRf == true)
                {
                    C_PRCxClearTmpRef();
                    //oSql.Clear();
                    //oSql.AppendLine("TRUNCATE TABLE " + tC_TblRefund);
                    ////*Arm 63-09-17 Clear Temp บิลอ้างอิง
                    //oSql.AppendLine("TRUNCATE TABLE TPSTSalHDTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TPSTSalHDCstTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TPSTSalHDDisTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TPSTSalDTTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TPSTSalDTDisTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TPSTSalPDTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TPSTSalRDTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TPSTSalRCTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TCNTMemTxnSaleTmp");
                    //oSql.AppendLine("TRUNCATE TABLE TCNTMemTxnRedeemTmp");
                    ////+++++++++++++
                    //oDB.C_SETxDataQuery(oSql.ToString());
                }
                //+++++++++++++

                //*Em 63-05-15
                if (nC_DocType == 9)
                {
                    nC_LastDocRefund++;
                }
                else
                {
                    nC_LastDocSale++;
                }
                //+++++++++++++++++++

                cVB.tVB_KbdScreen = "SALE";
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : W_SETxNewDoc start", cVB.bVB_AlwPrnLog);
                cVB.oVB_Sale.W_SETxNewDoc();    //*Arm 63-03-05
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : W_SETxNewDoc end", cVB.bVB_AlwPrnLog);

                cVB.oVB_Sale.bW_Activate = false;
                cVB.oVB_Sale.Show();
                return true;

                ////*Arm 62-10-01 -ย้ายมาทำงานหลังการ upload
                //if (cVB.cVB_Change > 0)
                //{
                //    new wChange().ShowDialog();
                //    cVB.oVB_Sale.bW_Activate = false;
                //    cVB.oVB_Sale.Show();
                //}


                ////*Em 63-04-22
                //oNewDoc = new Thread(() =>
                //{
                //    try
                //    {
                //        cVB.oVB_Sale.W_SETxNewDoc();
                //    }
                //    catch (Exception oEx)
                //    {
                //        new cLog().C_WRTxLog("cSale", "Thread oNewDoc : " + oEx.Message);
                //    }
                //    finally
                //    {
                //        C_PRCxCloseThread(oNewDoc);
                //    }
                //});
                //oNewDoc.IsBackground = true;
                //oNewDoc.Priority = ThreadPriority.Highest;
                //oNewDoc.Start();
                ////+++++++++++++++++++++++

                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wPayment);
                //if (oFormShow != null)
                //    oFormShow.Close();


                //new wSale(3).Show();

                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wPayment);
                //if (oFormShow != null)
                //    oFormShow.Close();

                ////*Net 63-04-01 Comment from baseline
                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSale);
                //oFormShow.Show();

                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wPayment);
                //if (oFormShow != null)
                //    oFormShow.Close();




                //new wSale(3).Show();
                //if (oFormShow != null)
                //    oFormShow.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : " + oEx.Message);
                return false;
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxCloseThread(Thread poThread)
        {
            try
            {
                poThread.Abort();
                //cVB.oVB_Payment.Dispose();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxCloseThread : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

        }

        /// <summary>
        /// Upload Update Sale Refer(*Arm 63-06-10)
        /// </summary>
        public static void C_PRCxUploadUpdateSaleRefer()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalHD> aHD = new List<cmlTPSTSalHD>();
            cmlReqUpdSaleRefer oReq;
            cmlResBase oRes;
            cClientService oCall;
            HttpResponseMessage oRep;
            string tJSonCall;
            string tUrl;

            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_API2ARDoc)) // Check Url API2ArDoc
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxUploadUpdateSaleRefer/URL API2ARDoc is null or empty ...");
                    return;
                }

                tUrl = cVB.tVB_API2ARDoc + "/UpdSaleRefer/Data";
                oReq = new cmlReqUpdSaleRefer();
                oRes = new cmlResBase();

                // 2. Set Request Parameter 
                oReq.ptBchCode = cVB.tVB_BchCode;
                oReq.ptDocNo = cVB.tVB_DocNo;
                oReq.ptRefDocNo = cVB.tVB_RefDocNo;
                oReq.pnSaleType = nC_DocType;
                oReq.pnOptionRfn = cVB.nVB_ReturnType;

                //*Arm 63-09-16
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTBchCode FROM " + tC_Ref_TblSalHD + " WITH(NOLOCK)");
                if (nC_DocType == 9)oSql.AppendLine("WHERE FNXshDocType = '1'");
                else oSql.AppendLine("WHERE FNXshDocType = '9'");
                
                oReq.ptRefBchCode = oDB.C_GEToDataQuery<string>(oSql.ToString());
                //++++++++++++++

                if (nC_DocType == 9) //ถ้าเป็นการคืนให้ส่ง ตารางเก็บการคืนไปด้วย
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT FNXsdSeqNo AS pnSeqNo, FNXsdSeqNoOld AS pnSeqNoOld, ISNULL(FCXsdQty,0) AS pcQty, ISNULL(FCXsdQtyRfn,0) AS pcQtyRfn FROM " + tC_TblRefund + " WITH(NOLOCK)");
                    oReq.aoRfn = oDB.C_GETaDataQuery<cmlTblRefund>(oSql.ToString());
                }

                tJSonCall = JsonConvert.SerializeObject(oReq);

                new cLog().C_WRTxLog("cSale", " C_PRCxUploadUpdateSaleRefer : Call API2ARDoc start...");
                oCall = new cClientService();
                oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);
                oRep = new HttpResponseMessage();
                try
                {
                    oRep = oCall.C_POSToInvoke(tUrl, tJSonCall);
                }
                catch (Exception oEx)
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxUploadUpdateSaleRefer/Call API2ARDoc Error : " + oEx.Message);
                    return;
                }
                new cLog().C_WRTxLog("cSale", " C_PRCxUploadUpdateSaleRefer : Call API2ARDoc End...");

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    oRes = JsonConvert.DeserializeObject<cmlResBase>(tJSonRes);

                    if (oRes.rtCode == "001")
                    {
                        new cLog().C_WRTxLog("cSale", "C_PRCxUploadUpdateSaleRefer/API2ARDoc Response Code " + oRes.rtCode + " " + oRes.rtDesc);
                    }
                    else
                    {
                        new cLog().C_WRTxLog("cSale", "C_PRCxUploadUpdateSaleRefer/API2ARDoc Response Code " + oRes.rtCode + " " + oRes.rtDesc);
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxUploadUpdateSaleRefer : " + oEx.Message.ToString());
            }
            finally
            {
                aHD = null;
                oSql = null;
                oDB = null;
                oReq = null;
                oRep = null;
                oRes = null;
                oCall = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }


        /// <summary>
        /// *Arm 63-03-17
        /// คำนวณแต้ม และเก็บ Transaction การขายและการแลกแต้ม ของบิลขาย 
        /// </summary>
        public static void C_PRCxSalePoint()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            cmlTPSTSalHD oHD;
            DataTable oSalHD;

            decimal cPointUsed = 0; // Point ที่ใช้
            decimal cPointRcv = 0;  // point ที่รับ
            decimal cSumPoint = 0;  // Point Active
            decimal cSumGrand = 0;  // Amount total
            decimal cSumPointPmt = 0; // *Arm 63-04-14 Point Promotion 
            string tRefDoc = "";

            try
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : Get data hd", cVB.bVB_AlwPrnLog);
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSalHD = new DataTable();



                //*Net 63-06-01 Note join dt with tcnmpdt where pdtpoint=1 -> Sum NetAfHD
                oSql.Clear();
                //oSql.AppendLine("SELECT * ");
                oSql.AppendLine("SELECT SUM(ISNULL(DT.FCXsdNetAfHD,0)) AS FCXshGrand ,HD.FTCstCode,HD.FDXshDocDate "); //*Net 63-06-01 ไม่ใช้ Star *
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " HD WITH(NOLOCK)");
                oSql.AppendLine($"INNER JOIN {cSale.tC_TblSalDT} DT WITH(NOLOCK) ON HD.FTBchCode=DT.FTBchCode AND HD.FTXshDocNo=DT.FTXshDocNo AND DT.FTXsdStaPdt <> '4'"); //*Net 63-06-01 join DT Sum เฉพาะสินค้าที่ให้แต้มได้
                oSql.AppendLine($"INNER JOIN TCNMPdt PDT WITH(NOLOCK) ON DT.FTPdtCode=PDT.FTPdtCode AND PDT.FTPdtPoint='1' "); //*Net 63-06-01 join TCNMPDT เฉพาะสินค้าที่อนุญาตให้แต้ม
                oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("GROUP BY HD.FTCstCode,HD.FDXshDocDate"); //*Net 63-06-01
                //oHD = oDB.C_GEToDataQuery<cmlTPSTSalHD>(oSql.ToString());
                oSalHD = oDB.C_GEToDataQuery(oSql.ToString()); //*Net 63-06-01

                if (oSalHD == null || oSalHD.Rows.Count == 0)
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT HD.FTCstCode,HD.FDXshDocDate ");
                    oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " HD WITH(NOLOCK)");
                    oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSalHD = oDB.C_GEToDataQuery(oSql.ToString()); //*Net 63-06-01
                    cSumGrand = 0m;
                    cPointRcv = 0m;
                }
                else
                {
                    // Amount total
                    //cSumGrand = Convert.ToDecimal(oHD.FCXshGrand);
                    cSumGrand = oSalHD.Rows[0].Field<decimal>("FCXshGrand");
                    // Point ที่ได้รับ
                    //cPointRcv = (Math.Floor(Convert.ToDecimal(oHD.FCXshGrand) / cVB.cVB_PntOptBuyAmt) * cVB.cVB_PntOptGetQty);
                    cPointRcv = (Math.Floor(cSumGrand / cVB.cVB_PntOptBuyAmt) * cVB.cVB_PntOptGetQty);
                }
                // DocNo
                //tRefDoc = oHD.FTXshDocNo;
                tRefDoc = cVB.tVB_DocNo;
                tC_TxnRefCode = tRefDoc; //*Arm 63-03-31
                // Point ที่ใช้ 
                cPointUsed = cVB.nVB_CstPiontB4Used - cVB.nVB_CstPoint;
                // Point Active = point ก่อนใช้ + Point ที่ได้รับ - Point ที่ใช้
                cSumPoint = cVB.nVB_CstPiontB4Used + cPointRcv - cPointUsed;

                new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : Recal point promotion", cVB.bVB_AlwPrnLog);
                C_PRCxReCalPntPmt();  //*Em 63-06-04

                //*Arm 63-04-15  point ที่ได้รับ promotion จาก PD
                //========================================================================
                new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : Get point from pd", cVB.bVB_AlwPrnLog);
                oSql.Clear();
                oSql.AppendLine("SELECT SUM(ISNULL(FCXpdPoint,0)) AS FCXpdPoint ");
                oSql.AppendLine("FROM (SELECT DISTINCT FTBchCode, FTXshDocNo, FTPmhDocNo, FCXpdPoint");
                oSql.AppendLine("       FROM " + cSale.tC_TblSalPD + " ");
                oSql.AppendLine("       WHERE ISNULL(FCXpdPoint,0) > 0 ");
                oSql.AppendLine("       AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("       AND FTXshDocNo = '" + cVB.tVB_DocNo + "') PD");
                oSql.AppendLine("GROUP BY FTBchCode, FTXshDocNo");
                cSumPointPmt = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                //==========================================================================

                if (cPointRcv != 0m || cSumPointPmt != 0m)
                {

                    new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : insert data to TCNTMemTxnSale", cVB.bVB_AlwPrnLog);
                    // #Process TxnSale
                    // **************************************
                    oSql.Clear();
                    //oSql.AppendLine("INSERT INTO TCNTMemTxnSale (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine($"INSERT INTO {tC_TblTxnSal} (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine("FTCgpCode, FTMemCode, FTTxnRefDoc, FTTxnRefInt, FTTxnRefSpl");
                    oSql.AppendLine(", FDTxnRefDate, FCTxnRefGrand, FCTxnPntOptBuyAmt, FCTxnPntOptGetQty, FCTxnPntB4Bill");
                    oSql.AppendLine(", FDTxnPntStart, FDTxnPntExpired, FCTxnPntBillQty, FCTxnPntUsed, FCTxnPntExpired");
                    oSql.AppendLine(", FTTxnPntStaClosed, FTTxnPntDocType, FTTxnStaSend"); //*Arm 63-04-15 เพิ่ม FTTxnStaSend
                    oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES (");
                    //oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + oHD.FTCstCode + "', '" + tRefDoc + "', '', '' "); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                    oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + oSalHD.Rows[0].Field<string>("FTCstCode") + "', '" + tRefDoc + "', '', '' "); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                                                                                                                                                      //oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "', '" + cSumGrand + "', '" + cVB.cVB_PntOptBuyAmt + "', '" + cVB.cVB_PntOptGetQty + "', '" + (cVB.nVB_CstPiontB4Used - cPointUsed) + "' ");  //*Arm 63-04-29
                    oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oSalHD.Rows[0].Field<DateTime>("FDXshDocDate"))) + "', '" + cSumGrand + "', '" + cVB.cVB_PntOptBuyAmt + "', '" + cVB.cVB_PntOptGetQty + "', '" + (cVB.nVB_CstPiontB4Used - cPointUsed) + "' ");  //*Arm 63-04-29

                    if (!string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm 63-04-03
                    {
                        oSql.AppendLine(", GETDATE(), '" + cVB.tVB_ExpiredDate + "', '" + (cPointRcv + cSumPointPmt) + "', '0', '0'"); //*Arm 63-04-03
                    }
                    else
                    {
                        oSql.AppendLine($", GETDATE(), NULL, '{(cPointRcv + cSumPointPmt)}', '0', '0'");
                    }
                    oSql.AppendLine(", '1', '1', '1' ");
                    oSql.AppendLine(", GETDATE(), '" + cVB.tVB_UsrCode + "', GETDATE(), '" + cVB.tVB_UsrCode + "'");
                    oSql.AppendLine(")");
                    oDB.C_SETxDataQuery(oSql.ToString());

                }
                if (cPointUsed > 0)
                {

                    // #Process TxnRedeem
                    // **************************************
                    new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : insert data to TCNTMemTxnRedeem", cVB.bVB_AlwPrnLog);
                    oSql.Clear();
                    //oSql.AppendLine("INSERT INTO TCNTMemTxnRedeem (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine($"INSERT INTO {tC_TblTxnRD} (");//*Net 63-06-01
                    oSql.AppendLine("FTCgpCode, FTMemCode, FTRedRefDoc, FTRedRefSpl, FTRedRefInt, FTRedPntDocType"); //*Arm 63-03-21 FTRedRefInt , //*Arm 63-04-01 เพิ่ม FTRedPntDocType
                    oSql.AppendLine(", FDRedRefDate, FCRedPntB4Bill, FCRedPntBillQty, FTRedPntStaClosed");
                    oSql.AppendLine(", FDRedPntStart, FDRedPntExpired, FTRedStaSend "); //*Arm 63-04-15 เพิ่ม FTRedStaSend
                    oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES (");
                    //oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + oHD.FTCstCode + "', '" + tRefDoc + "', '','', '1' ");  //*Arm 63-03-21 FTRedRefInt = '', //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                    oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + cVB.tVB_CstCode + "', '" + tRefDoc + "', '','', '1' ");  //*Arm 63-03-21 FTRedRefInt = '', //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                    //oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "', '" + cVB.nVB_CstPiontB4Used + "', '" + cPointUsed + "', '1' ");
                    oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oSalHD.Rows[0].Field<DateTime>("FDXshDocDate"))) + "', '" + cVB.nVB_CstPiontB4Used + "', '" + cPointUsed + "', '1' ");

                    if (!string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm 63-04-03
                    {
                        oSql.AppendLine(",GETDATE(), '" + cVB.tVB_ExpiredDate + "','1' "); //*Arm 63-04-03
                    }
                    else
                    {
                        oSql.AppendLine(",GETDATE(), NULL,'1' ");
                    }
                    oSql.AppendLine(",GETDATE(), '" + cVB.tVB_UsrCode + "', GETDATE(), '" + cVB.tVB_UsrCode + "'");
                    oSql.AppendLine(")");
                    oDB.C_SETxDataQuery(oSql.ToString());

                }

                //*Arm 63-04-14 UpdatePoint ใน TPSTSaleHDCst
                //========================================================================
                new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : update hdcst", cVB.bVB_AlwPrnLog);
                oSql.Clear();
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalHDCst + " with(rowlock) SET");
                oSql.AppendLine("FCXshCstPnt = '" + (cPointRcv - cPointUsed) + "' ");
                oSql.AppendLine(", FCXshCstPntPmt = '" + cSumPointPmt + "'");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

                //========================================================================


                //*Net Note 63-06-01 ย้ายไปอัพโหลดพร้อม SyncUpd
                //*Arm 63-05-24 ปรับส่งข้อมูล Transaction แบบ Thread
                //new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : C_PRCxSendTxnSale Start");
                //oTxnSale = new Thread(C_PRCxSendTxnSale);
                //oTxnSale.IsBackground = true;
                //oTxnSale.Priority = ThreadPriority.Highest;
                //oTxnSale.Start();
                //new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : C_PRCxSendTxnSale end");

                #region Comment
                ////*Arm 63-04-14 กวาดข้อมูล Transaction ที่ยังไม่ถูกส่ง ส่งไป Center และ Update Status
                ////========================================================================

                //new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : get data TCNTMemTxnRedeem");
                //// 1.ส่ง Transation Redeem
                //oSql.Clear();
                //oSql.AppendLine("SELECT FTRedRefDoc FROM TCNTMemTxnRedeem with(nolock)");
                //oSql.AppendLine("WHERE ISNULL(FTRedStaSend,'1') != '2' ");
                //oSql.AppendLine("ORDER BY FDRedRefDate ASC ");
                //List<cmlTCNTMemTxnRedeem> aoSendTxnRdm = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                //if (aoSendTxnRdm.Count > 0)
                //{
                //    new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : C_PRCbPublishTxn2AdaMember start");
                //    foreach (cmlTCNTMemTxnRedeem oData in aoSendTxnRdm)
                //    {
                //        if (C_PRCbPublishTxn2AdaMember("2", oData.FTRedRefDoc) == true)
                //        {
                //            oSql.Clear();
                //            oSql.AppendLine("UPDATE TCNTMemTxnRedeem with(rowlock) SET");
                //            oSql.AppendLine("FTRedStaSend = '2' ");
                //            oSql.AppendLine("WHERE FTRedRefDoc = '" + oData.FTRedRefDoc + "'");
                //            oDB.C_SETxDataQuery(oSql.ToString());
                //        }
                //    }
                //    new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : C_PRCbPublishTxn2AdaMember end");
                //}

                //new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : get data TCNTMemTxnSale");
                //// 2.ส่ง Transation Sale :
                //oSql.Clear();
                //oSql.AppendLine("SELECT FTTxnRefDoc FROM TCNTMemTxnSale with(nolock)");
                //oSql.AppendLine("WHERE ISNULL(FTTxnStaSend,'1') != '2' ");
                //oSql.AppendLine("ORDER BY FDTxnRefDate ASC ");
                //List<cmlTCNTMemTxnSale> aoSendTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                //if (aoSendTxnSale.Count > 0)
                //{
                //    new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : C_PRCbPublishTxn2AdaMember start");
                //    foreach (cmlTCNTMemTxnSale oData in aoSendTxnSale)
                //    {
                //        if (C_PRCbPublishTxn2AdaMember("1", oData.FTTxnRefDoc) == true)
                //        {
                //            oSql.Clear();
                //            oSql.AppendLine("UPDATE TCNTMemTxnSale with(rowlock) SET");
                //            oSql.AppendLine("FTTxnStaSend = '2' ");
                //            oSql.AppendLine("WHERE FTTxnRefDoc = '" + oData.FTTxnRefDoc + "'");
                //            oDB.C_SETxDataQuery(oSql.ToString());
                //        }
                //    }
                //    new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : C_PRCbPublishTxn2AdaMember end");
                //}
                ////========================================================================
                #endregion
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : " + oEx.Message);
            }
            finally
            {
                oSalHD = null;
                oDB = null;
                oSql = null;
                oHD = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem

            }
        }

        /// <summary>
        /// //*Arm 63-05-24 
        /// </summary>
        private static void C_PRCxSendTxnSale()
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                //*Arm 63-04-14 กวาดข้อมูล Transaction ที่ยังไม่ถูกส่ง ส่งไป Center และ Update Status
                //========================================================================

                new cLog().C_WRTxLog("cSale", "C_PRCxSendTxnSale : get data TCNTMemTxnRedeem", cVB.bVB_AlwPrnLog);
                // 1.ส่ง Transation Redeem
                oSql.Clear();
                //oSql.AppendLine("SELECT FTRedRefDoc FROM TCNTMemTxnRedeem with(nolock)");
                oSql.AppendLine($"SELECT FTRedRefDoc FROM {tC_TblTxnRD} with(nolock)");
                oSql.AppendLine("WHERE ISNULL(FTRedStaSend,'1') != '2' ");
                oSql.AppendLine("ORDER BY FDRedRefDate ASC ");
                List<cmlTCNTMemTxnRedeem> aoSendTxnRdm = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                if (aoSendTxnRdm == null || aoSendTxnRdm.Count == 0)
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTRedRefDoc FROM TCNTMemTxnRedeem with(nolock)");
                    oSql.AppendLine("WHERE ISNULL(FTRedStaSend,'1') != '2' ");
                    oSql.AppendLine("ORDER BY FDRedRefDate ASC ");
                    aoSendTxnRdm = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                }
                if (aoSendTxnRdm.Count > 0)
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxSendTxnSale : C_PRCbPublishTxn2AdaMember start", cVB.bVB_AlwPrnLog);
                    foreach (cmlTCNTMemTxnRedeem oData in aoSendTxnRdm)
                    {
                        if (C_PRCbPublishTxn2AdaMember("2", oData.FTRedRefDoc) == true)
                        {
                            oSql.Clear();
                            //oSql.AppendLine("UPDATE TCNTMemTxnRedeem with(rowlock) SET");
                            oSql.AppendLine($"UPDATE {tC_TblTxnRD} with(rowlock) SET");
                            oSql.AppendLine("FTRedStaSend = '2' ");
                            oSql.AppendLine("WHERE FTRedRefDoc = '" + oData.FTRedRefDoc + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }
                    }
                    new cLog().C_WRTxLog("cSale", "C_PRCxSendTxnSale : C_PRCbPublishTxn2AdaMember end", cVB.bVB_AlwPrnLog);
                }

                new cLog().C_WRTxLog("cSale", "C_PRCxSendTxnSale : get data TCNTMemTxnSale", cVB.bVB_AlwPrnLog);
                // 2.ส่ง Transation Sale :
                oSql.Clear();
                //oSql.AppendLine("SELECT FTTxnRefDoc FROM TCNTMemTxnSale with(nolock)");
                oSql.AppendLine($"SELECT FTTxnRefDoc FROM {tC_TblTxnSal} with(nolock)"); //*Net 63-06-01
                oSql.AppendLine("WHERE ISNULL(FTTxnStaSend,'1') != '2' ");
                oSql.AppendLine("ORDER BY FDTxnRefDate ASC ");
                List<cmlTCNTMemTxnSale> aoSendTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                if (aoSendTxnSale.Count > 0)
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxSendTxnSale : C_PRCbPublishTxn2AdaMember start", cVB.bVB_AlwPrnLog);
                    foreach (cmlTCNTMemTxnSale oData in aoSendTxnSale)
                    {
                        if (C_PRCbPublishTxn2AdaMember("1", oData.FTTxnRefDoc) == true)
                        {
                            oSql.Clear();
                            //oSql.AppendLine("UPDATE TCNTMemTxnSale with(rowlock) SET");
                            oSql.AppendLine($"UPDATE {tC_TblTxnSal} with(rowlock) SET"); //*Net 63-06-01
                            oSql.AppendLine("FTTxnStaSend = '2' ");
                            oSql.AppendLine("WHERE FTTxnRefDoc = '" + oData.FTTxnRefDoc + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }
                    }
                    new cLog().C_WRTxLog("cSale", "C_PRCxSendTxnSale : C_PRCbPublishTxn2AdaMember end", cVB.bVB_AlwPrnLog);
                }
                //========================================================================
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxSendTxnSale : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        ///// <summary>
        ///// *Arm 63-03-17
        ///// คำนวณแต้ม และเก็บ Transaction การขายและการแลกแต้ม ของบิลขาย 
        ///// </summary>
        //public static void C_PRCxSalePoint()
        //{
        //    cDatabase oDB = new cDatabase();
        //    StringBuilder oSql;
        //    cmlTPSTSalHD oHD;

        //    decimal cPointUsed = 0; // Point ที่ใช้
        //    decimal cPointRcv = 0;  // point ที่รับ
        //    decimal cSumPoint = 0;  // Point Active
        //    decimal cSumGrand = 0;  // Amount total
        //    decimal cSumPointPmt = 0; // *Arm 63-04-14 Point Promotion 
        //    string tRefDoc = "";

        //    try
        //    {
        //        oSql = new StringBuilder();
        //        oSql.Clear();
        //        oSql.AppendLine("SELECT * ");
        //        oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
        //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
        //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
        //        oDB = new cDatabase();
        //        oHD = oDB.C_GEToDataQuery<cmlTPSTSalHD>(oSql.ToString());

        //        // DocNo
        //        tRefDoc = oHD.FTXshDocNo;
        //        tC_TxnRefCode = tRefDoc; //*Arm 63-03-31
        //        // Amount total
        //        cSumGrand = Convert.ToDecimal(oHD.FCXshGrand);
        //        // Point ที่ใช้ 
        //        cPointUsed = cVB.nVB_CstPiontB4Used - cVB.nVB_CstPoint;
        //        // Point ที่ได้รับ
        //        cPointRcv = (Math.Floor(Convert.ToDecimal(oHD.FCXshGrand) / cVB.cVB_PntOptBuyAmt) * cVB.cVB_PntOptGetQty);
        //        // Point Active = point ก่อนใช้ + Point ที่ได้รับ - Point ที่ใช้
        //        cSumPoint = cVB.nVB_CstPiontB4Used + cPointRcv - cPointUsed;


        //        //*Arm 63-04-15  point ที่ได้รับ promotion จาก PD
        //        //========================================================================

        //        oSql.Clear();
        //        oSql.AppendLine("SELECT SUM(ISNULL(FCXpdPoint,0)) AS FCXpdPoint ");
        //        oSql.AppendLine("FROM (SELECT DISTINCT FTBchCode, FTXshDocNo, FTPmhDocNo, FCXpdPoint");
        //        oSql.AppendLine("       FROM TPSTSalPD ");
        //        oSql.AppendLine("       WHERE ISNULL(FCXpdPoint,0) > 0 ");
        //        oSql.AppendLine("       AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
        //        oSql.AppendLine("       AND FTXshDocNo = '" + cVB.tVB_DocNo + "') PD");
        //        oSql.AppendLine("GROUP BY FTBchCode, FTXshDocNo");
        //        cSumPointPmt = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

        //        //==========================================================================


        //        // #Process TxnSale
        //        // **************************************
        //        oSql.Clear();
        //        oSql.AppendLine("INSERT INTO TCNTMemTxnSale WITH(ROWLOCK) (");
        //        oSql.AppendLine("FTCgpCode, FTMemCode, FTTxnRefDoc, FTTxnRefInt, FTTxnRefSpl");
        //        oSql.AppendLine(", FDTxnRefDate, FCTxnRefGrand, FCTxnPntOptBuyAmt, FCTxnPntOptGetQty, FCTxnPntB4Bill");
        //        oSql.AppendLine(", FDTxnPntStart, FDTxnPntExpired, FCTxnPntBillQty, FCTxnPntUsed, FCTxnPntExpired");
        //        oSql.AppendLine(", FTTxnPntStaClosed, FTTxnPntDocType, FTTxnStaSend"); //*Arm 63-04-15 เพิ่ม FTTxnStaSend
        //        oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
        //        oSql.AppendLine(")");
        //        oSql.AppendLine("VALUES (");
        //        oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + oHD.FTCstCode + "', '" + tRefDoc + "', '', '' "); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
        //        oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "', '" + cSumGrand + "', '" + cVB.cVB_PntOptBuyAmt + "', '" + cVB.cVB_PntOptGetQty + "', '" + cVB.nVB_CstPiontB4Used + "' ");

        //        if (!string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm 63-04-03
        //        {
        //            oSql.AppendLine(", GETDATE(), '" + cVB.tVB_ExpiredDate + "', '" + (cPointRcv + cSumPointPmt) + "', '0', '0'"); //*Arm 63-04-03
        //        }
        //        else
        //        {
        //            oSql.AppendLine($", GETDATE(), NULL, '{(cPointRcv + cSumPointPmt)}', '0', '0'");
        //        }
        //        oSql.AppendLine(", '1', '1', '1' ");
        //        oSql.AppendLine(", GETDATE(), '" + cVB.tVB_UsrCode + "', GETDATE(), '" + cVB.tVB_UsrCode + "'");
        //        oSql.AppendLine(")");
        //        oDB.C_SETxDataQuery(oSql.ToString());

        //        //C_PRCbPublishTxn2AdaMember("1", tRefDoc); 

        //        if (cPointUsed > 0)
        //        {

        //            // #Process TxnRedeem
        //            // **************************************

        //            oSql.Clear();
        //            oSql.AppendLine("INSERT INTO TCNTMemTxnRedeem WITH(ROWLOCK) (");
        //            //oSql.AppendLine("FTCgpCode, FTMemCode, FTRedRefDoc, FTRedRefSpl");
        //            oSql.AppendLine("FTCgpCode, FTMemCode, FTRedRefDoc, FTRedRefSpl, FTRedRefInt, FTRedPntDocType"); //*Arm 63-03-21 FTRedRefInt , //*Arm 63-04-01 เพิ่ม FTRedPntDocType
        //            oSql.AppendLine(", FDRedRefDate, FCRedPntB4Bill, FCRedPntBillQty, FTRedPntStaClosed");
        //            oSql.AppendLine(", FDRedPntStart, FDRedPntExpired, FTRedStaSend "); //*Arm 63-04-15 เพิ่ม FTRedStaSend
        //            oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
        //            oSql.AppendLine(")");
        //            oSql.AppendLine("VALUES (");
        //            oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + oHD.FTCstCode + "', '" + tRefDoc + "', '','', '1' ");  //*Arm 63-03-21 FTRedRefInt = '', //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
        //            oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "', '" + cVB.nVB_CstPiontB4Used + "', '" + cPointUsed + "', '1' ");

        //            if (!string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm 63-04-03
        //            {
        //                oSql.AppendLine(",GETDATE(), '" + cVB.tVB_ExpiredDate + "','1' "); //*Arm 63-04-03
        //            }
        //            else
        //            {
        //                oSql.AppendLine(",GETDATE(), NULL,'1' ");
        //            }
        //            oSql.AppendLine(",GETDATE(), '" + cVB.tVB_UsrCode + "', GETDATE(), '" + cVB.tVB_UsrCode + "'");
        //            oSql.AppendLine(")");
        //            oDB.C_SETxDataQuery(oSql.ToString());

        //            //C_PRCbPublishTxn2AdaMember("2", tRefDoc);
        //        }

        //        //// #Point Active
        //        //// **************************************
        //        //oSql.Clear();
        //        //oSql.AppendLine("SELECT Count(FTMemCode) FROM TCNTMemPntActive WHERE FTMemCode = '" + oHD.FTCstCode + "'");
        //        //if (oDB.C_GEToDataQuery<int>(oSql.ToString()) > 0)
        //        //{
        //        //    oSql.Clear();
        //        //    oSql.AppendLine("Update TCNTMemPntActive SET");
        //        //    oSql.AppendLine("FCTxnPntQty = '" + cSumPoint + "'");
        //        //    oSql.AppendLine(", FDTxnPntLast = GETDATE()");
        //        //    oSql.AppendLine("WHERE FTCgpCode = '" + cVB.tVB_MemCgpCode + "' AND FTMemCode = '" + oHD.FTCstCode + "'"); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
        //        //    oDB.C_SETxDataQuery(oSql.ToString());
        //        //}
        //        //else
        //        //{
        //        //    oSql.Clear();
        //        //    oSql.AppendLine("INSERT INTO TCNTMemPntActive WITH(ROWLOCK) (");
        //        //    oSql.AppendLine("FTCgpCode ");
        //        //    oSql.AppendLine(",FTMemCode ");
        //        //    oSql.AppendLine(",FCTxnPntQty ");
        //        //    oSql.AppendLine(",FCTxnPnt2ExpYear ");
        //        //    oSql.AppendLine(",FDTxnPntLast )");
        //        //    oSql.AppendLine("VALUES (");
        //        //    oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "'");  //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
        //        //    oSql.AppendLine(", '" + oHD.FTCstCode + "' ");
        //        //    oSql.AppendLine(", '" + cSumPoint + "' ");
        //        //    oSql.AppendLine(", 0 ");
        //        //    oSql.AppendLine(", GETDATE() )");
        //        //    oDB.C_SETxDataQuery(oSql.ToString());
        //        //}

        //        //// #Amount Active
        //        //// **************************************
        //        //oSql.Clear();
        //        //oSql.AppendLine("SELECT Count(FTMemCode) FROM TCNTMemAmtActive WHERE FTMemCode = '" + oHD.FTCstCode + "'");
        //        //if (oDB.C_GEToDataQuery<int>(oSql.ToString()) > 0)
        //        //{
        //        //    oSql.Clear();
        //        //    oSql.AppendLine("Update TCNTMemAmtActive SET");
        //        //    oSql.AppendLine("FCTxnBuyTotal = ISNULL(FCTxnBuyTotal,0) +'" + cSumGrand + "'");
        //        //    oSql.AppendLine(", FDTxnBuyLast = GETDATE()");
        //        //    oSql.AppendLine("WHERE FTCgpCode = '" + cVB.tVB_MemCgpCode + "' AND FTMemCode = '" + oHD.FTCstCode + "'"); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
        //        //    oDB.C_SETxDataQuery(oSql.ToString());
        //        //}
        //        //else
        //        //{
        //        //    oSql.Clear();
        //        //    oSql.AppendLine("INSERT INTO TCNTMemAmtActive WITH(ROWLOCK) (");
        //        //    oSql.AppendLine("FTCgpCode ");
        //        //    oSql.AppendLine(",FTMemCode ");
        //        //    oSql.AppendLine(",FCTxnBuyTotal ");
        //        //    oSql.AppendLine(",FDTxnBuyLast )");
        //        //    oSql.AppendLine("VALUES (");
        //        //    oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "' "); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
        //        //    oSql.AppendLine(",'" + oHD.FTCstCode + "'");
        //        //    oSql.AppendLine(", '" + cSumGrand + "' ");
        //        //    oSql.AppendLine(", GETDATE() )");
        //        //    oDB.C_SETxDataQuery(oSql.ToString());
        //        //}

        //        //*Arm 63-04-14 UpdatePoint ใน TPSTSaleHDCst
        //        //========================================================================

        //        oSql.Clear();
        //        oSql.AppendLine("UPDATE TPSTSalHDCst with(rowlock) SET");
        //        oSql.AppendLine("FCXshCstPnt = '" + (cPointRcv - cPointUsed) + "' ");
        //        oSql.AppendLine(", FCXshCstPntPmt = '"+cSumPointPmt+"'");
        //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
        //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
        //        oDB.C_SETxDataQuery(oSql.ToString());

        //        //========================================================================



        //        //*Arm 63-04-14 กวาดข้อมูล Transaction ที่ยังไม่ถูกส่ง ส่งไป Center และ Update Status
        //        //========================================================================

        //        // 1.ส่ง Transation Sale :
        //        oSql.Clear();
        //        oSql.AppendLine("SELECT FTTxnRefDoc FROM TCNTMemTxnSale with(nolock)");
        //        oSql.AppendLine("WHERE ISNULL(FTTxnStaSend,'1') != '2' ");
        //        oSql.AppendLine("ORDER BY FDTxnRefDate ASC ");
        //        List <cmlTCNTMemTxnSale> aoSendTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
        //        if(aoSendTxnSale.Count >0)
        //        {
        //            foreach(cmlTCNTMemTxnSale oData in aoSendTxnSale)
        //            {
        //                if (C_PRCbPublishTxn2AdaMember("1", oData.FTTxnRefDoc) == true)
        //                {
        //                    oSql.Clear();
        //                    oSql.AppendLine("UPDATE TCNTMemTxnSale with(rowlock) SET");
        //                    oSql.AppendLine("FTTxnStaSend = '2' ");
        //                    oSql.AppendLine("WHERE FTTxnRefDoc = '" + oData.FTTxnRefDoc + "'");
        //                    oDB.C_SETxDataQuery(oSql.ToString());
        //                }
        //            }
        //        }

        //        // 2.ส่ง Transation Redeem
        //        oSql.Clear();
        //        oSql.AppendLine("SELECT FTRedRefDoc FROM TCNTMemTxnRedeem with(nolock)");
        //        oSql.AppendLine("WHERE ISNULL(FTRedStaSend,'1') != '2' ");
        //        oSql.AppendLine("ORDER BY FDRedRefDate ASC ");
        //        List<cmlTCNTMemTxnRedeem> aoSendTxnRdm = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
        //        if (aoSendTxnRdm.Count > 0)
        //        {
        //            foreach (cmlTCNTMemTxnRedeem oData in aoSendTxnRdm)
        //            {
        //                if (C_PRCbPublishTxn2AdaMember("2", oData.FTRedRefDoc) == true)
        //                {
        //                    oSql.Clear();
        //                    oSql.AppendLine("UPDATE TCNTMemTxnRedeem with(rowlock) SET");
        //                    oSql.AppendLine("FTRedStaSend = '2' ");
        //                    oSql.AppendLine("WHERE FTRedRefDoc = '" + oData.FTRedRefDoc + "'");
        //                    oDB.C_SETxDataQuery(oSql.ToString());
        //                }
        //            }
        //        }

        //        //========================================================================


        //        ////*Arm 63-04-01
        //        //C_PRCbPublishTxn2AdaMember("1", tRefDoc);
        //        //if (cPointUsed > 0)
        //        //{
        //        //    C_PRCbPublishTxn2AdaMember("2", tRefDoc);
        //        //}
        //        ////+++++++++++++
        //    }
        //    catch (Exception oEx)
        //    {
        //        new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : " + oEx.Message);
        //    }
        //    finally
        //    {
        //        oDB = null;
        //        oSql = null;
        //        oHD = null;

        //    }
        //}

        /// <summary>
        /// *Arm 63-03-17
        /// ส่งข้อมูล Transaction การขายและการแลกแต้มไป AdaMember(Center)
        /// </summary>
        /// <param name="ptTxnMode">1:TxnSale, 2:TxnRedeem</param>
        /// <param name="ptRefDoc">เลขที่เอกสาร</param>
        /// <returns></returns>
        public static bool C_PRCbPublishTxn2AdaMember(string ptTxnMode, string ptRefDoc)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            cmlTransaction oTrans;
            cmlTxnSale oTxnSale;
            cmlTxnRedeem oTxnRedeem;
            string tQueueName = "QMemberTrans";
            try
            {
                if (string.IsNullOrEmpty(cVB.tVB_MemberMQHost)) return false;
                if (string.IsNullOrEmpty(cVB.tVB_MemberMQUsr)) return false;
                if (string.IsNullOrEmpty(cVB.tVB_MemberMQPwd)) return false;
                if (string.IsNullOrEmpty(cVB.tVB_MemberMQVirtual)) return false;

                cVB.oVB_RabbitMQConfig.tHostName = cVB.tVB_MemberMQHost;
                cVB.oVB_RabbitMQConfig.tUserName = cVB.tVB_MemberMQUsr;
                cVB.oVB_RabbitMQConfig.tPassword = cVB.tVB_MemberMQPwd;
                cVB.oVB_RabbitMQConfig.tVirtual = cVB.tVB_MemberMQVirtual;

                cVB.oVB_MQFactory = new ConnectionFactory();
                cVB.oVB_MQFactory.HostName = cVB.oVB_RabbitMQConfig.tHostName;
                cVB.oVB_MQFactory.UserName = cVB.oVB_RabbitMQConfig.tUserName;
                cVB.oVB_MQFactory.Password = cVB.oVB_RabbitMQConfig.tPassword;
                cVB.oVB_MQFactory.VirtualHost = cVB.oVB_RabbitMQConfig.tVirtual;

                cVB.oVB_MQConn = cVB.oVB_MQFactory.CreateConnection();
                cVB.oVB_MQModel = cVB.oVB_MQConn.CreateModel();
                cVB.oVB_MQModel.QueueDeclare(tQueueName, true, false, false, null);

                switch (ptTxnMode)
                {
                    case "1":

                        // #TxnSale : Send TxnSale to Center
                        oSql = new StringBuilder();
                        //oSql.AppendLine("SELECT * FROM TCNTMemTxnSale WITH(NOLOCK)");
                        oSql.AppendLine($"SELECT * FROM {tC_TblTxnSal} WITH(NOLOCK)"); //*Net 63-06-01
                        oSql.AppendLine("WHERE FTTxnRefDoc = '" + ptRefDoc + "'");
                        oTxnSale = new cmlTxnSale();
                        oTxnSale.aoTCNTMemTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                        if (oTxnSale.aoTCNTMemTxnSale == null || oTxnSale.aoTCNTMemTxnSale.Count == 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine($"SELECT * FROM TCNTMemTxnSale WITH(NOLOCK)"); //*Net 63-06-01
                            oSql.AppendLine("WHERE FTTxnRefDoc = '" + ptRefDoc + "'");
                            oTxnSale = new cmlTxnSale();
                            oTxnSale.aoTCNTMemTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                        }

                        if (oTxnSale.aoTCNTMemTxnSale.Count > 0)
                        {
                            // Send TxnSale to Center
                            oTrans = new cmlTransaction();
                            oTrans.ptFunction = "TXNSALE";
                            oTrans.ptSource = cVB.tVB_MemBchCode;   //*Arm 63-03-30
                            oTrans.ptDest = "CENTER";
                            oTrans.ptData = JsonConvert.SerializeObject(oTxnSale);

                            //Public MQ Center
                            string tMgsJson = JsonConvert.SerializeObject(oTrans);
                            var oBody = Encoding.UTF8.GetBytes(tMgsJson);
                            cVB.oVB_MQModel.BasicPublish("", tQueueName, false, null, oBody);

                            //*Net 63-07-30 ปรับตาม Moshi
                            //*Em 63-07-17
                            cVB.oVB_MQModel.Close();
                            cVB.oVB_MQConn.Close();
                            cVB.oVB_MQFactory = null;
                            //+++++++++++++
                        }

                        break;
                    case "2":
                        //# TxnRedeem : Send TxnRedeemto Center
                        oSql = new StringBuilder();
                        //oSql.AppendLine("SELECT * FROM TCNTMemTxnRedeem WITH(NOLOCK)");
                        oSql.AppendLine($"SELECT * FROM {tC_TblTxnRD} WITH(NOLOCK)"); //*Net 63-06-01
                        oSql.AppendLine("WHERE FTRedRefDoc = '" + ptRefDoc + "'");
                        oTxnRedeem = new cmlTxnRedeem();
                        oTxnRedeem.aoTCNTMemTxnRedeem = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                        if (oTxnRedeem.aoTCNTMemTxnRedeem == null || oTxnRedeem.aoTCNTMemTxnRedeem.Count == 0)
                        {
                            oSql.Clear();
                            oSql.AppendLine("SELECT * FROM TCNTMemTxnRedeem WITH(NOLOCK)");
                            oSql.AppendLine("WHERE FTRedRefDoc = '" + ptRefDoc + "'");
                            oTxnRedeem = new cmlTxnRedeem();
                            oTxnRedeem.aoTCNTMemTxnRedeem = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                        }

                        if (oTxnRedeem.aoTCNTMemTxnRedeem.Count > 0)
                        {
                            // Send TxnRedeem to Center
                            oTrans = new cmlTransaction();
                            oTrans.ptFunction = "TXNREDEEM";
                            oTrans.ptSource = cVB.tVB_MemBchCode;   //*Arm 63-03-30
                            oTrans.ptDest = "CENTER";
                            oTrans.ptData = JsonConvert.SerializeObject(oTxnRedeem);

                            //Public MQ Center
                            string tMgsJson = JsonConvert.SerializeObject(oTrans);
                            var oBody = Encoding.UTF8.GetBytes(tMgsJson);
                            cVB.oVB_MQModel.BasicPublish("", tQueueName, false, null, oBody);

                            //*Net 63-07-30 ปรับตาม Moshi
                            //*Em 63-07-17
                            cVB.oVB_MQModel.Close();
                            cVB.oVB_MQConn.Close();
                            cVB.oVB_MQFactory = null;
                            //+++++++++++++
                        }

                        break;
                }

                return true;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCbPublishTxn2AdaMember : " + oEx.Message);
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                oTrans = null;
                oTxnSale = null;
                oTxnRedeem = null;
                ptRefDoc = "";
                ptTxnMode = "";
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// *Arm 63-03-17
        /// คำนวณแต้ม และเก็บ Transaction การขายและการแลกแต้ม ของบิลคืน 
        /// </summary>
        public static void C_PRCxReturnSalePoint()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            //cmlTPSTSalHD oHDSale;
            cmlSalReturn oHDSaleRtn;    // ยอดบิลคืนปัจจุบัน
            cmlSalReturn oHDSaleRtnBfr; // ยอดรวมคืนสะสมก่อนหน้า
            cmlSalReturn oHDSale;       // ยอดรวมบิลขาย

            // เงื่อนไขแต้ม ตามบิลขาย
            decimal cPntOptBuyAmt = 0;  // Arm 63-04-04
            decimal cPntOptGetQty = 0;  // Arm 63-04-04
            //+++++++++

            decimal cPointRcv = 0;      // point ที่ได้รับคำนวณใหม่
            //decimal cGrandSale = 0;   // ยอดขายรวมบิลขาย
            //decimal cPointSale = 0;   // แต้มคำนนวณจากบิลขาย
            decimal cGrandBalance = 0;  // ยอดขายคงเหลือ

            //string tCstCode = "";
            string tTxnRefDoc = "";
            string tTxnRefInt = "";

            string tTblSalHD;
            string tTblSalHDCst;
            string tTblSalDT;
            string tTblSalPD;           //*Arm 63-09-17
            string tTblSalRD;           //*Arm 63-09-17
            string tTblTxnSale;         //*Arm 63-09-17
            string tTblTxnRedeem;       //*Arm 63-09-17

            decimal cPointDif = 0;      // ผลต่าง(ใช้คำนวณแต้มสะสม)
            decimal cGrandDif = 0;      // ผลต่าง(ใช้คำนวณยอดสะสม)

            // Redeem
            string tRedRefDoc = "";
            string tRedRefInt = "";
            decimal cPointBalance = 0;  // แต้มที่ใช้คงเหลือหลังจากคืนแล้ว
            decimal cRdPointRtn = 0;    // แต้มที่ต้องคืน

            decimal cPntRcv = 0;            // *Arm 63-04-16 หาแต้มทีที่ต้องคืน เก็บลง HDCst
            decimal cSumPointPmtDif = 0;    // *Arm 63-04-16 ผลต่าง(ใช้คำนวณ)
            decimal cSumPointPmt = 0;       // *Arm 63-04-15 คืนแต้มที่ได้จาก Promotion
            decimal cSumPointPmtSale = 0;    // *Arm 63-04-16 คืนแต้มที่ได้จาก Promotion ทั้งหมดของบิลขาย
            decimal cSumPointPmtRtnBfr = 0; // *Arm 63-04-16 คืนแต้มที่ได้จาก Promotion ที่คืนไปแล้วบางส่วน (ผลรวมจากบิคืนก่อนหน้า)

            bool bStaSndAdaMember = true;  //*Arm 63-09-17 สถานะอนุญาตให้ส่ง Transation การขายไป AdaMember หรือไม่ True:อนุญาตให้ส่ง, false:ไม่อนุญาตให้ส่ง

            DateTime dDocDate;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                //*Arm 63-03-21
                oHDSale = new cmlSalReturn();           // ยอดรวมบิลขาย
                oHDSaleRtnBfr = new cmlSalReturn();    // ยอดรวมคืนสะสมก่อนหน้า
                oHDSaleRtn = new cmlSalReturn();        // ยอดบิลคืนปัจจุบัน

                oSql = new StringBuilder();

                //tTblSalHD = cVB.bVB_RefundTrans ? "TPSTSalHD" : cSale.tC_TblSalHD;
                //tTblSalHDCst = cVB.bVB_RefundTrans ? "TPSTSalHDCst" : cSale.tC_TblSalHDCst;
                //tTblSalDT = cVB.bVB_RefundTrans ? "TPSTSalDT" : cSale.tC_TblSalDT;
                

                //Arm 63-05-01
                oSql.AppendLine("SELECT FDXshDocDate FROM " + cSale.tC_TblSalHD + " with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                dDocDate = oDB.C_GEToDataQuery<DateTime>(oSql.ToString());
                //+++++++++++++

                //*Arm 63-04-04 เงื่อนไขการได้แต้มจากบิลขาย
                oSql.Clear();
                //oSql.AppendLine("SELECT  FCTxnPntOptBuyAmt, FCTxnPntOptGetQty FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + cVB.tVB_RefDocNo + "'");
                //oSql.AppendLine($"SELECT  FCTxnPntOptBuyAmt, FCTxnPntOptGetQty FROM {tC_TblTxnSal} WHERE FTTxnRefDoc = '" + cVB.tVB_RefDocNo + "'"); //*Net 63-06-01
                oSql.AppendLine($"SELECT  FCTxnPntOptBuyAmt, FCTxnPntOptGetQty FROM {tC_Ref_TblTxnSale} WHERE FTTxnRefDoc = '" + cVB.tVB_RefDocNo + "'"); //*Arm 63-09-17
                cmlTCNTMemTxnSale oTxnSale = oDB.C_GEToDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                ////*Net 63-06-01
                //if (oTxnSale == null)
                //{
                //    oSql.AppendLine($"SELECT  FCTxnPntOptBuyAmt, FCTxnPntOptGetQty FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + cVB.tVB_RefDocNo + "'"); //*Net 63-06-01
                //    oTxnSale = oDB.C_GEToDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                //}

                if (oTxnSale != null)
                {
                    cPntOptBuyAmt = (decimal)oTxnSale.FCTxnPntOptBuyAmt;
                    cPntOptGetQty = (decimal)oTxnSale.FCTxnPntOptGetQty;
                    
                }
                else
                {
                    //cPntOptBuyAmt = 0;
                    //cPntOptGetQty = 0;
                    return; //*Arm 63-09-17
                }

                //+++++++++++++

                //ยอดรวมบิลขาย
                oSql.Clear();
                oSql.AppendLine("SELECT COUNT(*) AS nRCount, SUM(DT.FCXsdNetAfHD) AS cGetGrandRtn, ");
                oSql.AppendLine("FLOOR(ISNULL(SUM(DT.FCXsdNetAfHD), 0.00) / " + cPntOptBuyAmt + ") * " + cPntOptGetQty + " AS cGetPoint");
                //oSql.AppendLine("FROM TPSTSalHD with(nolock)");
                oSql.AppendLine("FROM " + tC_Ref_TblSalHD + " HD with(nolock)");
                oSql.AppendLine("INNER JOIN " + tC_Ref_TblSalDT + " DT WITH(NOLOCK) ON HD.FTBchCode=DT.FTBchCode AND HD.FTXshDocNo=DT.FTXshDocNo AND DT.FTXsdStaPdt <> '4'"); //*Net 63-06-01 join DT Sum เฉพาะสินค้าที่ให้แต้มได้
                oSql.AppendLine("INNER JOIN TCNMPdt PDT WITH(NOLOCK) ON DT.FTPdtCode=PDT.FTPdtCode AND PDT.FTPdtPoint='1' "); //*Net 63-06-01 join TCNMPDT เฉพาะสินค้าที่อนุญาตให้แต้ม
                //oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND HD.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                oSql.AppendLine("WHERE HD.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'"); //*Arm 63-09-17
                oHDSale = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());
                if (oHDSale == null)
                {
                    oHDSale = new cmlSalReturn();
                    oHDSale.cGetGrandRtn = 0m;
                    oHDSale.cGetPoint = 0m;
                }

                // ยอดบิลคืนปัจจุบัน
                oSql.Clear();
                //oSql.AppendLine("SELECT HD.FTBchCode, HD.FTXshDocNo, FCXshGrand AS cGetGrandRtn, FLOOR(ISNULL(FCXshGrand, 0.00) / " + cPntOptBuyAmt + ") * " + cPntOptGetQty + " AS cGetPoint ");
                oSql.AppendLine("SELECT HD.FTBchCode, HD.FTXshDocNo, SUM(DT.FCXsdNetAfHD) AS cGetGrandRtn, FLOOR(ISNULL(SUM(DT.FCXsdNetAfHD), 0.00) / " + cPntOptBuyAmt + ") * " + cPntOptGetQty + " AS cGetPoint ");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " HD with(nolock) ");
                oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalDT + " DT WITH(NOLOCK) ON HD.FTBchCode=DT.FTBchCode AND HD.FTXshDocNo=DT.FTXshDocNo AND DT.FTXsdStaPdt <> '4'"); //*Net 63-06-01 join DT Sum เฉพาะสินค้าที่ให้แต้มได้
                oSql.AppendLine("INNER JOIN TCNMPdt PDT WITH(NOLOCK) ON DT.FTPdtCode=PDT.FTPdtCode AND PDT.FTPdtPoint='1' "); //*Net 63-06-01 join TCNMPDT เฉพาะสินค้าที่อนุญาตให้แต้ม
                oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("GROUP BY HD.FTBchCode, HD.FTXshDocNo");
                oHDSaleRtn = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());
                if (oHDSaleRtn == null)
                {
                    oHDSaleRtn = new cmlSalReturn();
                    oHDSaleRtn.cGetGrandRtn = 0m;
                    oHDSaleRtn.cGetPoint = 0m;
                }

                //ยอดรวมคืนสะสมก่อนหน้า
                oSql.Clear();
                //oSql.AppendLine("SELECT COUNT(*) AS nRCount, SUM(FCXshGrand) AS cGetGrandRtn, ");
                oSql.AppendLine("SELECT COUNT(*) AS nRCount, SUM(DT.FCXsdNetAfHD) AS cGetGrandRtn, ");
                //oSql.AppendLine("FLOOR(ISNULL(SUM(FCXshGrand), 0.00) / " + cPntOptBuyAmt + ") * " + cPntOptGetQty + " AS cGetPoint");
                oSql.AppendLine("FLOOR(ISNULL(SUM(DT.FCXsdNetAfHD), 0.00) / " + cPntOptBuyAmt + ") * " + cPntOptGetQty + " AS cGetPoint");
                //oSql.AppendLine("FROM TPSTSalHD with(nolock)");
                oSql.AppendLine("FROM " + tC_Ref_TblSalHD + " HD with(nolock)");
                oSql.AppendLine("INNER JOIN " + tC_Ref_TblSalDT + " DT WITH(NOLOCK) ON HD.FTBchCode=DT.FTBchCode AND HD.FTXshDocNo=DT.FTXshDocNo AND DT.FTXsdStaPdt <> '4'"); //*Net 63-06-01 join DT Sum เฉพาะสินค้าที่ให้แต้มได้
                oSql.AppendLine("INNER JOIN TCNMPdt PDT WITH(NOLOCK) ON DT.FTPdtCode=PDT.FTPdtCode AND PDT.FTPdtPoint='1' "); //*Net 63-06-01 join TCNMPDT เฉพาะสินค้าที่อนุญาตให้แต้ม
                //oSql.AppendLine("WHERE HD.FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND HD.FTXshRefInt = '" + cVB.tVB_RefDocNo + "'");
                oSql.AppendLine("WHERE HD.FTXshRefInt = '" + cVB.tVB_RefDocNo + "'"); //*Arm 63-09-17
                oHDSaleRtnBfr = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());
                if (oHDSaleRtnBfr == null)
                {
                    oHDSaleRtnBfr = new cmlSalReturn();
                    oHDSaleRtnBfr.cGetGrandRtn = 0m;
                    oHDSaleRtnBfr.cGetPoint = 0m;
                }

                if (oHDSaleRtnBfr.nRCount == 0) //ถ้าไม่ยอดการคืนก่อนหน้า
                {
                    tTxnRefDoc = cVB.tVB_RefDocNo + "_1";
                    cGrandBalance = oHDSale.cGetGrandRtn - oHDSaleRtn.cGetGrandRtn;
                }
                else
                {
                    tTxnRefDoc = cVB.tVB_RefDocNo + "_" + ((int)oHDSaleRtnBfr.nRCount + 1).ToString();
                    cGrandBalance = oHDSale.cGetGrandRtn - (oHDSaleRtnBfr.cGetGrandRtn + oHDSaleRtn.cGetGrandRtn);

                }

                if (cGrandBalance <= 0) // ยอดขาย หักลบยอดคืนทั้งหมดแล้ว คงเหลือ 0 บาท
                {
                    cGrandBalance = 0;
                    cPointRcv = 0;
                    cGrandDif = 0;
                    cPointDif = 0;
                }
                else
                {
                    // คำนวณแต้มใหม่จากยอดขายหักลบยอดคืนทั้งหมดแล้ว คงเหลือ
                    //cPointRcv = (Math.Floor(Convert.ToDecimal(cGrandBalance) / cVB.cVB_PntOptBuyAmt) * cVB.cVB_PntOptGetQty);
                    cPointRcv = (Math.Floor(Convert.ToDecimal(cGrandBalance) / cPntOptBuyAmt) * cPntOptGetQty);   //*Arm 63-04-04

                    // ยอดคืน จากยอดบิลคืนปัจุบัน (ใช้นำไปหักกับยอดซื้อสะสมทั้งหมด)
                    cGrandDif = oHDSaleRtn.cGetGrandRtn;

                    // คำนวณแต้ม จากยอดคืน ของบิลคืนปัจุบัน (ใช้นำไปหักกับแต้มสะสมทั้งหมด)
                    //cPointDif = (Math.Floor(Convert.ToDecimal(cGrandDif) / cVB.cVB_PntOptBuyAmt) * cVB.cVB_PntOptGetQty);
                    cPointDif = (Math.Floor(Convert.ToDecimal(cGrandDif) / cPntOptBuyAmt) * cPntOptGetQty);

                }
                tC_TxnRefCode = tTxnRefDoc; //*Arm 63-03-31
                tTxnRefInt = cVB.tVB_RefDocNo;

                //*Arm 63-04-15 หา point promotion จาก PD 
                //========================================================================
                // 1.หาแต้มโปรโมชั่นที่ได้ทั้งหมดจากบิลขาย
                // 2.หาแต้มโปรโมชั่นที่เคยคืนก่อนหน้า
                // 3.หาแต้มโปรโมชั่นที่ต้องคืนในบิลปัจจุบัน
                // 4.ข้อ 1 -(ข้อ 2 + ข้อ 3)
                //   - ผลลัพธ์ <0  แต้มโปรโมชั่นที่ต้องคืน = 0
                //   - ผลลัพธ์ >=0   แต้มโปรโมชั่นที่ต้องคืน = ข้อ 3
                //=========================================================================

                // 1.หาแต้มโปรโมชั่นที่ได้ทั้งหมดจากบิลขาย
                oSql.Clear();
                //oSql.AppendLine("SELECT ISNULL(FCXshCstPntPmt,0) FROM TPSTSalHDCst with(nolock)");
                oSql.AppendLine("SELECT ISNULL(FCXshCstPntPmt,0) FROM " + tC_Ref_TblSalHDCst + " with(nolock)");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "' ");
                oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "' "); //*Arm 63-09-17
                cSumPointPmtSale = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                // 2.หาแต้มโปรโมชั่นที่เคยคืนก่อนหน้า
                oSql.Clear();
                //oSql.AppendLine("SELECT SUM(ISNULL(HDCst.FCXshCstPntPmt,0)) FROM " + cSale.tC_TblSalHD + " HDTmp ");
                oSql.AppendLine("SELECT ISNULL(SUM(ISNULL(HDCst.FCXshCstPntPmt,0)),0) FROM " + cSale.tC_TblSalHD + " HDTmp ");
                //oSql.AppendLine("INNER JOIN TPSTSalHD HD with(nolock) ON HDTmp.FTBchCode = HD.FTBchCode AND HDTmp.FTXshRefInt = HD.FTXshRefInt  ");
                //oSql.AppendLine("INNER JOIN " + tTblSalHD + " HD with(nolock) ON HDTmp.FTBchCode = HD.FTBchCode AND HDTmp.FTXshRefInt = HD.FTXshRefInt  ");
                oSql.AppendLine("INNER JOIN " + tC_Ref_TblSalHD + " HD with(nolock) ON HDTmp.FTXshRefInt = HD.FTXshRefInt  "); //*Arm 63-09-17
                //oSql.AppendLine("INNER JOIN TPSTSalHDCst HDCst  with(nolock) ON HD.FTBchCode = HDCst.FTBchCode AND HD.FTXshDocNo = HDCst.FTXshDocNo  ");
                //oSql.AppendLine("INNER JOIN " + tTblSalHDCst + " HDCst  with(nolock) ON HD.FTBchCode = HDCst.FTBchCode AND HD.FTXshDocNo = HDCst.FTXshDocNo  ");
                oSql.AppendLine("INNER JOIN " + tC_Ref_TblSalHDCst + " HDCst  with(nolock) ON HD.FTXshDocNo = HDCst.FTXshDocNo  "); //*Arm 63-09-17
                oSql.AppendLine("WHERE HDTmp.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND HDTmp.FTXshDocNo = '" + cVB.tVB_DocNo + "' ");

                cSumPointPmtRtnBfr = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
                cSumPointPmtRtnBfr = cSumPointPmtRtnBfr * (-1); //ไม่ให้ค่าติดลบ

                // 3.หาแต้มโปรโมชั่นที่ต้องคืนในบิลปัจจุบัน
                oSql.Clear();
                oSql.AppendLine("SELECT SUM(ISNULL(FCXpdPoint,0)) AS FCXpdPoint ");
                oSql.AppendLine("FROM (SELECT DISTINCT FTBchCode, FTXshDocNo, FTPmhDocNo, FCXpdPoint");
                oSql.AppendLine("       FROM  " + cSale.tC_TblSalPD + " ");
                oSql.AppendLine("       WHERE ISNULL(FCXpdPoint,0) > 0 ");
                oSql.AppendLine("       AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("       AND FTXshDocNo = '" + cVB.tVB_DocNo + "') PD");
                oSql.AppendLine("GROUP BY FTBchCode, FTXshDocNo");
                cSumPointPmt = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                // 4.ข้อ 1 -(ข้อ 2 + ข้อ 3) **รองรับการคืนแบบ Option 4
                if (cSumPointPmtSale - (cSumPointPmtRtnBfr + cSumPointPmt) < 0)
                {
                    cSumPointPmt = 0;
                    cSumPointPmtDif = 0;
                }
                else
                {
                    cSumPointPmtDif = cSumPointPmtSale - (cSumPointPmtRtnBfr + cSumPointPmt);
                }
                //========================================================================


                //// #Process TxnSale
                //// **************************************
                //oSql.Clear();
                //oSql.AppendLine("INSERT INTO TCNTMemTxnSale WITH(ROWLOCK) (");
                //oSql.AppendLine("FTCgpCode, FTMemCode, FTTxnRefDoc, FTTxnRefInt, FTTxnRefSpl");
                //oSql.AppendLine(", FDTxnRefDate, FCTxnRefGrand, FCTxnPntOptBuyAmt, FCTxnPntOptGetQty, FCTxnPntB4Bill");
                //oSql.AppendLine(", FDTxnPntStart, FDTxnPntExpired, FCTxnPntBillQty, FCTxnPntUsed, FCTxnPntExpired");
                //oSql.AppendLine(", FTTxnPntStaClosed, FTTxnPntDocType, FTTxnStaSend "); //*Arm 63-04-15 เพิ่ม FTTxnStaSend
                //oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                //oSql.AppendLine(")");
                //oSql.AppendLine("VALUES (");
                //oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + cVB.tVB_CstCode + "', '" + tTxnRefDoc + "', '" + tTxnRefInt + "', '' "); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                //oSql.AppendLine(",GETDATE(), '" + cGrandBalance + "', '" + cPntOptBuyAmt + "', '" + cPntOptGetQty + "', '" + cVB.nVB_CstPiontB4Used + "' ");

                //if (!string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm 63-04-03
                //{
                //    oSql.AppendLine(", GETDATE(), '" + cVB.tVB_ExpiredDate + "', '" + (cPointRcv + cSumPointPmtDif) + "', '0', '0'"); //*Arm 63-04-03
                //}
                //else
                //{
                //    oSql.AppendLine($", GETDATE(), NULL, '{(cPointRcv + cSumPointPmtDif)}', '0', '0'");
                //}
                //oSql.AppendLine(", '1', '1', '1'");
                //oSql.AppendLine(", GETDATE(), '" + cVB.tVB_UsrCode + "', GETDATE(), '" + cVB.tVB_UsrCode + "'");
                //oSql.AppendLine(")");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //// ** ส่งการ TxnSale ไป Center
                ////C_PRCbPublishTxn2AdaMember("1", tTxnRefDoc);


                //*Arm 63-03-21
                // #Process TxnRedeem
                // **************************************

                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(SUM(FNXrdPntUse),0) FROM " + cSale.tC_TblSalRD + " with(nolock) ");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                cRdPointRtn = oDB.C_GEToDataQuery<int>(oSql.ToString());

                if (cRdPointRtn > 0)
                {

                    // ยอดใช้การแลกแต้มทั้งหมด
                    oSql.Clear();
                    oSql.AppendLine("SELECT COUNT(*) AS nRCount, ISNULL(SUM(FCRedPntBillQty), 0) AS cGetPoint ");
                    //oSql.AppendLine("FROM TCNTMemTxnRedeem with(nolock) ");
                    oSql.AppendLine($"FROM {tC_TblTxnRD} with(nolock) "); //*Net 63-06-01
                    oSql.AppendLine("WHERE FTMemCode = '" + cVB.tVB_CstCode + "' AND FTRedRefDoc = '" + cVB.tVB_RefDocNo + "' ");
                    cmlSalReturn oRd = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());

                    // ยอดการคืนแต้มรวมก่อนหน้า
                    oSql.Clear();
                    oSql.AppendLine("SELECT COUNT(*) AS nRCount, ISNULL(SUM(FCRedPntBillQty), 0) AS cGetPoint ");
                    //oSql.AppendLine("FROM TCNTMemTxnRedeem with(nolock) ");
                    oSql.AppendLine($"FROM {tC_TblTxnRD} with(nolock) "); //*Net 63-06-01
                    oSql.AppendLine("WHERE FTMemCode = '" + cVB.tVB_CstCode + "' AND FTRedRefInt = '" + cVB.tVB_RefDocNo + "' ");
                    cmlSalReturn oRdRtnBfr = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());

                    if (oRdRtnBfr.nRCount == 0)
                    {
                        tRedRefDoc = cVB.tVB_RefDocNo + "_1";
                        cPointBalance = oRd.cGetPoint - cRdPointRtn;
                    }
                    else
                    {
                        tRedRefDoc = cVB.tVB_RefDocNo + "_" + ((int)oRdRtnBfr.nRCount + 1).ToString();

                        cPointBalance = oRd.cGetPoint - (oRdRtnBfr.cGetPoint + cRdPointRtn);

                    }


                    // ** ลองรับการคืน Option 4
                    if (cPointBalance < 0) // ถ้าแต้มที่ใช้ ลบกับแต้วที่คืมรวมทั้งหมด ได้น้อยกว่า 0
                    {
                        if ((oRd.cGetPoint - oRdRtnBfr.cGetPoint) > 0) // ให้เช็คแต้มที่ใช้ ลบกันแต้วที่คืมก่อนหน้ารวมทั้งหมด
                        {
                            //ถ้ามากว่า 0 ให้คืนแต้มที่เหลือทั้งหมด
                            cRdPointRtn = oRd.cGetPoint - (oRd.cGetPoint - oRdRtnBfr.cGetPoint);
                            cPointBalance = 0;
                        }
                        else
                        {
                            cPointBalance = 0;
                            cRdPointRtn = 0;
                        }
                    }

                    tRedRefInt = cVB.tVB_RefDocNo;

                    oSql.Clear();
                    //oSql.AppendLine("INSERT INTO TCNTMemTxnRedeem (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine($"INSERT INTO {tC_TblTxnRD} (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                    oSql.AppendLine("FTCgpCode, FTMemCode, FTRedRefDoc, FTRedRefInt, FTRedRefSpl, FTRedPntDocType "); //*Arm 63-03-21 FTRedRefInt //*Arm 63-04-01 เพิ่ม FTRedPntDocType
                    oSql.AppendLine(", FDRedRefDate, FCRedPntB4Bill, FCRedPntBillQty, FTRedPntStaClosed");
                    oSql.AppendLine(", FDRedPntStart, FDRedPntExpired, FTRedStaSend "); //*Arm 63-04-15 เพิ่ม FTRedStaSend
                    oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES (");
                    oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + cVB.tVB_CstCode + "', '" + tRedRefDoc + "', '" + tRedRefInt + "','', '1' ");  //*Arm 63-03-21 FTRedRefInt = '', Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                    //oSql.AppendLine(",GETDATE(), '" + cVB.nVB_CstPiontB4Used + "', '" + cPointBalance + "', '1' ");
                    oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", dDocDate) + "', '" + cVB.nVB_CstPiontB4Used + "', '" + cPointBalance + "', '1' "); //*Arm 63-05-01

                    if (!string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm 63-04-03
                    {
                        oSql.AppendLine(",GETDATE(), '" + cVB.tVB_ExpiredDate + "' , '1' "); //*Arm 63-04-03
                    }
                    else
                    {
                        oSql.AppendLine(",GETDATE(), NULL, '1' ");
                    }
                    oSql.AppendLine(",GETDATE(), '" + cVB.tVB_UsrCode + "', GETDATE(), '" + cVB.tVB_UsrCode + "'");
                    oSql.AppendLine(")");
                    oDB.C_SETxDataQuery(oSql.ToString());

                    // ** ส่งการ Redeem ไป Center
                    //C_PRCbPublishTxn2AdaMember("2", tRedRefDoc);

                }


                // +++++++++++++


                // #Process TxnSale
                // **************************************
                oSql.Clear();
                //oSql.AppendLine("INSERT INTO TCNTMemTxnSale (");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine($"INSERT INTO {tC_TblTxnSal} (");//*Net 63-06-01
                oSql.AppendLine("FTCgpCode, FTMemCode, FTTxnRefDoc, FTTxnRefInt, FTTxnRefSpl");
                oSql.AppendLine(", FDTxnRefDate, FCTxnRefGrand, FCTxnPntOptBuyAmt, FCTxnPntOptGetQty, FCTxnPntB4Bill");
                oSql.AppendLine(", FDTxnPntStart, FDTxnPntExpired, FCTxnPntBillQty, FCTxnPntUsed, FCTxnPntExpired");
                oSql.AppendLine(", FTTxnPntStaClosed, FTTxnPntDocType, FTTxnStaSend "); //*Arm 63-04-15 เพิ่ม FTTxnStaSend
                oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES (");
                oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + cVB.tVB_CstCode + "', '" + tTxnRefDoc + "', '" + tTxnRefInt + "', '' "); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                //oSql.AppendLine(",GETDATE(), '" + cGrandBalance + "', '" + cPntOptBuyAmt + "', '" + cPntOptGetQty + "', '" + (cVB.nVB_CstPiontB4Used + cRdPointRtn) + "' ");
                oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", dDocDate) + "', '" + cGrandBalance + "', '" + cPntOptBuyAmt + "', '" + cPntOptGetQty + "', '" + (cVB.nVB_CstPiontB4Used + cRdPointRtn) + "' ");//*Arm 63-05-01

                if (!string.IsNullOrEmpty(cVB.tVB_ExpiredDate)) //*Arm 63-04-03
                {
                    oSql.AppendLine(", GETDATE(), '" + cVB.tVB_ExpiredDate + "', '" + (cPointRcv + cSumPointPmtDif) + "', '0', '0'"); //*Arm 63-04-03
                }
                else
                {
                    oSql.AppendLine($", GETDATE(), NULL, '{(cPointRcv + cSumPointPmtDif)}', '0', '0'");
                }
                oSql.AppendLine(", '1', '1', '1'");
                oSql.AppendLine(", GETDATE(), '" + cVB.tVB_UsrCode + "', GETDATE(), '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine(")");
                oDB.C_SETxDataQuery(oSql.ToString());




                //// #Point Active
                //// **************************************
                //oSql.Clear();
                //oSql.AppendLine("Update TCNTMemPntActive SET");
                ////oSql.AppendLine("FCTxnPntQty = ISNULL(FCTxnPntQty,0) - '" + cPointDif + "'");
                //oSql.AppendLine("FCTxnPntQty = (ISNULL(FCTxnPntQty,0) + " + cRdPointRtn + ") -" + cPointDif + " ");
                //oSql.AppendLine(", FDTxnPntLast = GETDATE()");
                //oSql.AppendLine("WHERE FTCgpCode ='" + cVB.tVB_MemCgpCode + "' AND FTMemCode = '" + cVB.tVB_CstCode + "'"); //*Arm 63-03-30 เพิ่ม FTMemCgpCode ='" +cVB.tVB_MemCgpCode+ "'
                //oDB.C_SETxDataQuery(oSql.ToString());

                //// #Amount Active
                //// **************************************
                //oSql.Clear();
                //oSql.AppendLine("Update TCNTMemAmtActive SET");
                //oSql.AppendLine("FCTxnBuyTotal = ISNULL(FCTxnBuyTotal,0.00) -" + cGrandDif + "");
                //oSql.AppendLine(", FDTxnBuyLast = GETDATE()");
                //oSql.AppendLine("WHERE FTCgpCode ='" + cVB.tVB_MemCgpCode + "' AND FTMemCode = '" + cVB.tVB_CstCode + "'");     //*Arm 63-03-30 เพิ่ม FTMemCgpCode ='" +cVB.tVB_MemCgpCode+ "'
                //oDB.C_SETxDataQuery(oSql.ToString());


                //*Arm 63-04-15 UpdatePoint ใน TPSTSaleHDCst (ยังไม่พร้อมใช้)
                //========================================================================

                // 1.หาแต้มทีที่ต้องคืน
                cPntRcv = cRdPointRtn - (decimal)oHDSaleRtn.cGetPoint;



                // 2.Update Point ใน SaleHDCst
                oSql.Clear();
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalHDCst + " with(rowlock) SET");
                oSql.AppendLine("FCXshCstPnt = '" + cPntRcv + "' ");
                oSql.AppendLine(", FCXshCstPntPmt = '" + (0 - cSumPointPmt) + "'");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

                //========================================================================
                //*Arm 63-09-17 ตรวสสอบจะให้ส่ง Transaction การขายไปที่ AdaMember หรือไม่
                switch(cVB.tVB_ApiCstSch_Fmt)
                {
                    case "SKC":
                        bStaSndAdaMember = false;
                        break;
                    default:
                        bStaSndAdaMember = true;
                        break;
                }
                //++++++++++++++

                //*Arm 63-05-24 ปรับส่งข้อมูล Transaction แบบ Thread
                if (bStaSndAdaMember)
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxReturnSalePoint : C_PRCxSendTxnSale Start", cVB.bVB_AlwPrnLog);
                    oTxnRfnSale = new Thread(C_PRCxSendTxnSale);
                    oTxnRfnSale.IsBackground = true;
                    oTxnRfnSale.Priority = ThreadPriority.Highest;
                    oTxnRfnSale.Start();
                    new cLog().C_WRTxLog("cSale", "C_PRCxReturnSalePoint : C_PRCxSendTxnSale end", cVB.bVB_AlwPrnLog);
                }
                ////*Arm 63-04-15 กวาดข้อมูล Transaction ที่ยังไม่ถูกส่ง ส่งไป Center และ Update Status
                ////========================================================================

                //// 1.ส่ง Transation Redeem
                //oSql.Clear();
                //oSql.AppendLine("SELECT FTRedRefDoc FROM TCNTMemTxnRedeem with(nolock)");
                //oSql.AppendLine("WHERE ISNULL(FTRedStaSend,'1') != '2' ");
                //oSql.AppendLine("ORDER BY FDRedRefDate ASC ");
                //List<cmlTCNTMemTxnRedeem> aoSendTxnRdm = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                //if (aoSendTxnRdm.Count > 0)
                //{
                //    foreach (cmlTCNTMemTxnRedeem oData in aoSendTxnRdm)
                //    {
                //        if (C_PRCbPublishTxn2AdaMember("2", oData.FTRedRefDoc) == true)
                //        {
                //            oSql.Clear();
                //            oSql.AppendLine("UPDATE TCNTMemTxnRedeem with(rowlock) SET");
                //            oSql.AppendLine("FTRedStaSend = '2' ");
                //            oSql.AppendLine("WHERE FTRedRefDoc = '" + oData.FTRedRefDoc + "'");
                //            oDB.C_SETxDataQuery(oSql.ToString());
                //        }
                //    }
                //}

                //// 2.ส่ง Transation Sale 
                //oSql.Clear();
                //oSql.AppendLine("SELECT FTTxnRefDoc FROM TCNTMemTxnSale with(nolock)");
                //oSql.AppendLine("WHERE ISNULL(FTTxnStaSend,'1') != '2' ");
                //oSql.AppendLine("ORDER BY FDTxnRefDate ASC ");
                //List<cmlTCNTMemTxnSale> aoSendTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                //if (aoSendTxnSale.Count > 0)
                //{
                //    foreach (cmlTCNTMemTxnSale oData in aoSendTxnSale)
                //    {
                //        if (C_PRCbPublishTxn2AdaMember("1", oData.FTTxnRefDoc) == true)
                //        {
                //            oSql.Clear();
                //            oSql.AppendLine("UPDATE TCNTMemTxnSale with(rowlock) SET");
                //            oSql.AppendLine("FTTxnStaSend = '2' ");
                //            oSql.AppendLine("WHERE FTTxnRefDoc = '" + oData.FTTxnRefDoc + "'");
                //            oDB.C_SETxDataQuery(oSql.ToString());
                //        }
                //    }
                //}
                ////========================================================================


                ////*Arm 63-04-01
                //C_PRCbPublishTxn2AdaMember("1", tTxnRefDoc);
                //if (cRdPointRtn > 0)
                //{
                //    C_PRCbPublishTxn2AdaMember("2", tRedRefDoc);
                //}
                ////+++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxReturnSalePoint : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                oHDSale = null;
                oHDSaleRtn = null;
                oHDSaleRtnBfr = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }


        public static void C_UPDxUpdateQty()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            int nQtyLef = 0;
            try
            {
                new cLog().C_WRTxLog("cSale", "C_UPDxUpdateQty :update qty Start", cVB.bVB_AlwPrnLog);
                oSql = new StringBuilder();
                //oSql.AppendLine("UPDATE TPSTSalDT WITH(ROWLOCK)");
                //oSql.AppendLine("SET FCXsdQtyLef = DT.FCXsdQtyLef - Rfn.FCXsdQty, ");
                //oSql.AppendLine("FCXsdQtyRfn = DT.FCXsdQtyRfn + Rfn.FCXsdQty ");
                ////oSql.AppendLine("FROM TPSTSalDT DT INNER JOIN (SELECT * FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "') Rfn ON DT.FTBchCode = Rfn.FTBchCode AND DT.FTPdtCode = Rfn.FTPdtCode AND DT.FTxsdBarCode = Rfn.FTxsdBarCode");
                ////*Em 63-05-15
                //oSql.AppendLine("FROM TPSTSalDT DT ");
                //oSql.AppendLine("INNER JOIN (SELECT FTBchCode,FTPdtCode,FTxsdBarCode,FCXsdQty FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "') Rfn ");
                //oSql.AppendLine("ON DT.FTBchCode = Rfn.FTBchCode AND DT.FTPdtCode = Rfn.FTPdtCode AND DT.FTxsdBarCode = Rfn.FTxsdBarCode");
                ////++++++++++++++++++
                //oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND DT.FTXsdStaPdt = '1'");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT SUM(FCXsdQtyLef) FROM TPSTSalDT WITH(NOLOCK) WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //nQtyLef = oDB.C_GEToDataQuery<int>(oSql.ToString());

                ////UPDATE FNXshStaRef in TPSTSalHD กรณีคืนสิ้นค้าหมดทั้งบิลแล้ว 
                //oSql = new StringBuilder();
                //oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK) SET ");
                //if (nQtyLef > 0)
                //{
                //    oSql.AppendLine(" FNXshStaRef = '1'");
                //}
                //else
                //{
                //    oSql.AppendLine(" FNXshStaRef = '2'");
                //}
                //oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                //oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //oDB.C_SETxDataQuery(oSql.ToString());


                //*Arm 63-06-10 Start

                //Update โดยเช็คจาก  ตารางเก็บ Refund
                oSql.AppendLine("UPDATE " + tC_Ref_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdQtyLef = DT.FCXsdQtyLef - Rfn.FCXsdQtyLef, ");
                oSql.AppendLine("FCXsdQtyRfn = DT.FCXsdQtyRfn + Rfn.FCXsdQtyLef ");
                oSql.AppendLine("FROM " + tC_Ref_TblSalDT + " DT ");
                oSql.AppendLine("INNER JOIN " + tC_TblRefund + " Rfn WITH(NOLOCK) ON DT.FNXsdSeqNo = Rfn.FNXsdSeqNoOld");
                oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND DT.FTXsdStaPdt = '1'");
                oDB.C_SETxDataQuery(oSql.ToString());

                //หาจำนวนที่เหลือ
                oSql.Clear();
                oSql.AppendLine("SELECT SUM(FCXsdQtyLef) FROM " + tC_Ref_TblSalDT + " WITH(NOLOCK) WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                nQtyLef = oDB.C_GEToDataQuery<int>(oSql.ToString());


                //UPDATE FNXshStaRef in TPSTSalHD กรณีคืนสิ้นค้าหมดทั้งบิลแล้ว 
                oSql.Clear();
                oSql.AppendLine("UPDATE " + tC_Ref_TblSalDT + " WITH(ROWLOCK) SET ");
                if (nQtyLef > 0)
                {
                    oSql.AppendLine(" FNXshStaRef = '1'");
                }
                else
                {
                    oSql.AppendLine(" FNXshStaRef = '2'");
                }
                oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

                //*Arm 63-06-10 End
                
                new cLog().C_WRTxLog("cSale", "C_UPDxUpdateQty : update qty end", cVB.bVB_AlwPrnLog);
                
                //Sync Upload การเปลี่ยนบิลขาย g,njv,u,udki8nolbo8hk  
                new cLog().C_WRTxLog("cSale", "C_UPDxUpdateQty : C_PRCxAdd2TmpLogChg start", cVB.bVB_AlwPrnLog);

                //*Arm 63-06-10
                if (cVB.bVB_RefundTrans == true)
                {
                    C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, true);
                }
                else
                {
                    C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, false);
                }
                //+++++++++++++

                //C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, true);  //*Arm 62-12-25
                new cLog().C_WRTxLog("cSale", "C_UPDxUpdateQty : C_PRCxAdd2TmpLogChg end", cVB.bVB_AlwPrnLog);

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_UPDxUpdateQty() : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

        }
        public static string C_GETtGndTextTH(string ptMonney)
        {
            try
            {
                string tBath = "";
                string[] tValue = new string[23];
                string tMonney = "";
                string tStang, tDigit, tTemp;
                int i, j;

                tTemp = ptMonney;
                //ptMonney = String.Format("{0:0000000000000000000.00}", ptMonney); // to set  format get 22 char
                ptMonney = String.Format("{0:0000000000000000000.00}", new cSP().SP_SETtDecShwSve(2, Convert.ToDecimal(ptMonney), 2)); // *Arm 63-08-26 ทำให้ทศนิยมเหลือ 2 ตำแหน่ง ถ้ามากว่า 2 ตำแหน่ง เศษสตางค์จะเพี้ยน

                tValue[1] = ""; tValue[2] = "สิบ"; tValue[3] = "ร้อย"; tValue[4] = "พัน";
                tValue[5] = "หมื่น"; tValue[6] = "แสน"; tValue[7] = "ล้าน"; tValue[8] = "สิบล้าน";
                tValue[9] = "ร้อยล้าน"; tValue[10] = "พันล้าน"; tValue[11] = "หมื่นล้าน";
                tValue[12] = "แสนล้าน"; tValue[13] = "ล้านล้าน"; tValue[14] = "สิบล้านล้าน"; tValue[15] = "ร้อยล้านล้าน";
                tValue[16] = "พันล้านล้าน"; tValue[17] = "หมื่นล้านล้าน"; tValue[18] = "แสนล้านล้าน"; tValue[19] = "ล้านล้านล้าน";
                tMonney = ""; j = 0; // Clear parameter
                tStang = "";
                for (i = ptMonney.Length - 1; i >= 0; i += -1)
                {
                    // for i= max downto min do
                    tDigit = ptMonney.Substring(i, 1);
                    j = j + 1;
                    switch (tDigit)
                    {
                        case "1":
                            {
                                if (j == 1 & tTemp.Length > 1)
                                    tBath = "เอ็ด";
                                else if (j != 8)
                                    tBath = "หนึ่ง";
                                if (j == 2)
                                    tBath = "";
                                break;
                            }

                        case "2":
                            {
                                if (j == 2)
                                    tBath = "ยี่";
                                else
                                    tBath = "สอง";
                                break;
                            }

                        case "3":
                            {
                                tBath = "สาม";
                                break;
                            }

                        case "4":
                            {
                                tBath = "สี่";
                                break;
                            }

                        case "5":
                            {
                                tBath = "ห้า";
                                break;
                            }

                        case "6":
                            {
                                tBath = "หก";
                                break;
                            }

                        case "7":
                            {
                                tBath = "เจ็ด";
                                break;
                            }

                        case "8":
                            {
                                tBath = "แปด";
                                break;
                            }

                        case "9":
                            {
                                tBath = "เก้า";
                                break;
                            }

                        case ".":
                            {
                                tStang = tMonney + "สตางค์"; tMonney = ""; j = 0;
                                break;
                            }
                    }
                    if (tDigit != "0")
                        tMonney = tBath + tValue[j] + tMonney;
                    tBath = "";
                }
                if (String.IsNullOrEmpty(tMonney))
                    tMonney = "ศูนย์";
                if (tStang == "สตางค์")
                    return tMonney + "บาทถ้วน";
                else
                    return tMonney + "บาท" + tStang;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_GETtGndTextTH : " + oEx.Message); return "";
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

        }

        public static string C_GETtGndTextEN(decimal pcMoney)
        {
            // ---------------------------------------------------------
            // Call:  pcMoney is money for convert
            // Ret:   money in Eng wording
            // ---------------------------------------------------------
            string tTemp, tTempFull;
            string tString;
            int i;
            int j;

            tTempFull = String.Format("{0:000000000000.00}", pcMoney);
            tString = "";

            tTemp = tTempFull.Substring(0, tTempFull.IndexOf("."));
            // Opps... it  s more than 999 trillion
            // One could easily add bigger number
            // support.
            if (tTemp.Length > 12)
            {
                return "";
            }

            // zero is a special case.
            // you may want to change this to "no"
            // as in "no dollars and 12/100" for writing
            // checks.
            if (Convert.ToInt32(tTemp) == 0)
            {
                return "zero";
            }

            i = Convert.ToInt32(tTemp.Substring(0, 3));
            if (i != 0)
            {
                tString = C_STRtHundreds(tString, i);
                tString = tString + " trillion";
            }

            i = Convert.ToInt32(tTemp.Substring(4, 3));
            if (i != 0)
            {
                tString = C_STRtHundreds(tString, i);
                tString = tString + " million";
            }

            i = Convert.ToInt32(tTemp.Substring(7, 3));
            if (i != 0)
            {
                tString = C_STRtHundreds(tString, i);
                tString = tString + " thousand";
            }

            i = Convert.ToInt32(tTemp.Substring(tTemp.Length - 3, 3));
            if (i != 0)
                tString = C_STRtHundreds(tString, i);

            tTemp = tTempFull.Substring(tTempFull.IndexOf(".") + 1, 2);
            if (tTemp != "00")
            {
                tString = tString + " and ";
                tString = C_STRtTens(tString, Convert.ToInt32(tTemp));
            }
            return tString;
        }

        private static string C_STRtHundreds(string ptStr, int pIn)
        {
            int j;
            if (pIn > 99)
            {
                j = pIn;
                pIn = pIn / 100;
                ptStr = C_STRtTens(ptStr, pIn);
                ptStr = ptStr + " hundred";
                pIn = j % 100;
            }

            if (pIn != 0)
                ptStr = C_STRtTens(ptStr, pIn);
            return ptStr;
        }

        private static string C_STRtTens(string ptStr, int pIn)
        {
            switch (pIn % 100)
            {
                case object _ when 90 <= pIn % 100 && pIn % 100 <= 99:
                    {
                        ptStr = ptStr + " ninety";
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }

                case object _ when 80 <= pIn % 100 && pIn % 100 <= 89:
                    {
                        ptStr = ptStr + " eighty";
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }

                case object _ when 70 <= pIn % 100 && pIn % 100 <= 79:
                    {
                        ptStr = ptStr + " seventy";
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }

                case object _ when 60 <= pIn % 100 && pIn % 100 <= 69:
                    {
                        ptStr = ptStr + " sixty";
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }

                case object _ when 50 <= pIn % 100 && pIn % 100 <= 59:
                    {
                        ptStr = ptStr + " fifty";
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }

                case object _ when 40 <= pIn % 100 && pIn % 100 <= 49:
                    {
                        ptStr = ptStr + " fourty";
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }

                case object _ when 30 <= pIn % 100 && pIn % 100 <= 39:
                    {
                        ptStr = ptStr + " thirty";
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }

                case object _ when 20 <= pIn % 100 && pIn % 100 <= 29:
                    {
                        ptStr = ptStr + " twenty";
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }

                case 19:
                    {
                        ptStr = ptStr + " nineteen";
                        break;
                    }

                case 18:
                    {
                        ptStr = ptStr + " eighteen";
                        break;
                    }

                case 17:
                    {
                        ptStr = ptStr + " seventeen";
                        break;
                    }

                case 16:
                    {
                        ptStr = ptStr + " sixteen";
                        break;
                    }

                case 15:
                    {
                        ptStr = ptStr + " fifteen";
                        break;
                    }

                case 14:
                    {
                        ptStr = ptStr + " fourteen";
                        break;
                    }

                case 13:
                    {
                        ptStr = ptStr + " thirteen";
                        break;
                    }

                case 12:
                    {
                        ptStr = ptStr + " twelve";
                        break;
                    }

                case 11:
                    {
                        ptStr = ptStr + " eleven";
                        break;
                    }

                case 10:
                    {
                        ptStr = ptStr + " ten";
                        break;
                    }

                default:
                    {
                        ptStr = C_STRtOnes(ptStr, pIn);
                        break;
                    }
            }
            return ptStr;
        }

        private static string C_STRtOnes(string ptStr, int pIn)
        {
            if (pIn < 10 | pIn % 10 == 0)
                ptStr = ptStr + " ";
            else
                ptStr = ptStr + " ";// "-"

            switch (pIn % 10)
            {
                case 9:
                    {
                        ptStr = ptStr + "nine";
                        break;
                    }

                case 8:
                    {
                        ptStr = ptStr + "eight";
                        break;
                    }

                case 7:
                    {
                        ptStr = ptStr + "seven";
                        break;
                    }

                case 6:
                    {
                        ptStr = ptStr + "six";
                        break;
                    }

                case 5:
                    {
                        ptStr = ptStr + "five";
                        break;
                    }

                case 4:
                    {
                        ptStr = ptStr + "four";
                        break;
                    }

                case 3:
                    {
                        ptStr = ptStr + "three";
                        break;
                    }

                case 2:
                    {
                        ptStr = ptStr + "two";
                        break;
                    }

                case 1:
                    {
                        ptStr = ptStr + "one";
                        break;
                    }
            }

            return ptStr;
        }

        public static void C_PRCxHoldBill()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            wReason oReason = null;
            Form oFormShow = null;
            cmlTPSTShiftEvent oEvent;
            string tReturnBillCode = ""; //*Arm 63-09-13 รหัสค้นคืน
            try
            {
                //if (new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 4) )


                if (cSale.nC_CntItem == 0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                    return;
                }

                //*Arm 63-09-13 รหัสค้นคืน
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : Enter Return bill code/Tel.", cVB.bVB_AlwPrnLog);
                wReturnBillCode oReturnBillCode = new wReturnBillCode();
                if (oReturnBillCode.ShowDialog() == DialogResult.OK)
                {
                    tReturnBillCode = oReturnBillCode.rtReturnBillCode;
                    new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : Return bill code/Tel. = " + tReturnBillCode, cVB.bVB_AlwPrnLog);
                }
                else
                {
                    return;
                }

                //+++++++++++++

                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : Start", cVB.bVB_AlwPrnLog);
                //if (MessageBox.Show(cVB.oVB_GBResource.GetString("tMsgHoldBill"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : C_PRCxSummary2HD", cVB.bVB_AlwPrnLog);
                C_PRCxSummary2HD();
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : nLastSeq", cVB.bVB_AlwPrnLog);
                int nLastSeq = 0;
                //*Net 63-07-30 ปรับตาม Moshi
                //oSql.AppendLine("SELECT MAX(FNHldNo) AS FNHldNo FROM TPSTHoldHD WITH(NOLOCK)");
                oSql.AppendLine("SELECT ISNULL(MAX(FNHldNo),0) AS FNHldNo FROM TPSTHoldHD WITH(NOLOCK)"); //*Net 63-07-07 ใส่ ISNULL
                nLastSeq = oDB.C_GEToDataQuery<int>(oSql.ToString());

                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : Insert hold", cVB.bVB_AlwPrnLog);
                nLastSeq = nLastSeq + 1;
                //Insert hold table
                oSql = new StringBuilder();
                //*Arm 63-09-13
                oSql.AppendLine("UPDATE " + tC_TblSalHD + " WITH(ROWLOCK) SET ");
                oSql.AppendLine("FTXshDisChgTxt = '" + tReturnBillCode + "' ");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("");
                //+++++++++++++
                oSql.AppendLine("INSERT INTO TPSTHoldHD (FNHldNo, FTBchCode, FTXshDocNo, FTShpCode, FNXshDocType, FDXshDocDate, FTXshCshOrCrd, FTXshVATInOrEx, FTDptCode, FTWahCode, FTPosCode, FTShfCode, FNSdtSeqNo, FTUsrCode, FTSpnCode,");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("FTXshApvCode, FTCstCode, FTXshDocVatFull, FTXshRefExt, FDXshRefExtDate, FTXshRefInt, FDXshRefIntDate, FTXshRefAE, FNXshDocPrint, FTRteCode, FCXshRteFac, FCXshTotal, FCXshTotalNV, FCXshTotalNoDis,");
                oSql.AppendLine("FCXshTotalB4DisChgV, FCXshTotalB4DisChgNV, FTXshDisChgTxt, FCXshDis, FCXshChg, FCXshTotalAfDisChgV, FCXshTotalAfDisChgNV, FCXshRefAEAmt, FCXshAmtV, FCXshAmtNV, FCXshVat, FCXshVatable, FTXshWpCode,");
                oSql.AppendLine("FCXshWpTax, FCXshGrand, FCXshRnd, FTXshGndText, FCXshPaid, FCXshLeft, FTXshRmk, FTXshStaRefund, FTXshStaDoc, FTXshStaApv, FTXshStaPrcStk, FTXshStaPaid, FNXshStaDocAct, FNXshStaRef, FDLastUpdOn,");
                oSql.AppendLine("FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("SELECT " + nLastSeq + " AS FNHldNo, FTBchCode, FTXshDocNo, FTShpCode, FNXshDocType, FDXshDocDate, FTXshCshOrCrd, FTXshVATInOrEx, FTDptCode, FTWahCode, FTPosCode, FTShfCode, FNSdtSeqNo, FTUsrCode, FTSpnCode,");
                oSql.AppendLine("FTXshApvCode, FTCstCode, FTXshDocVatFull, FTXshRefExt, FDXshRefExtDate, FTXshRefInt, FDXshRefIntDate, FTXshRefAE, FNXshDocPrint, FTRteCode, FCXshRteFac, FCXshTotal, FCXshTotalNV, FCXshTotalNoDis,");
                oSql.AppendLine("FCXshTotalB4DisChgV, FCXshTotalB4DisChgNV, FTXshDisChgTxt, FCXshDis, FCXshChg, FCXshTotalAfDisChgV, FCXshTotalAfDisChgNV, FCXshRefAEAmt, FCXshAmtV, FCXshAmtNV, FCXshVat, FCXshVatable, FTXshWpCode,");
                oSql.AppendLine("FCXshWpTax, FCXshGrand, FCXshRnd, FTXshGndText, FCXshPaid, FCXshLeft, FTXshRmk, FTXshStaRefund, FTXshStaDoc, FTXshStaApv, FTXshStaPrcStk, FTXshStaPaid, FNXshStaDocAct, FNXshStaRef, FDLastUpdOn,");
                oSql.AppendLine("FTLastUpdBy, FDCreateOn, FTCreateBy");
                oSql.AppendLine("FROM " + tC_TblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("");
                oSql.AppendLine("INSERT INTO TPSTHoldHDCst (FNHldNo, FTBchCode, FTXshDocNo, FTXshCardID, FTXshCardNo, FNXshCrTerm, FDXshDueDate, FDXshBillDue, FTXshCtrName, FDXshTnfDate, FTXshRefTnfID, FNXshAddrShip, FNXshAddrTax, FTXshCstTel, FTXshCstName)");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("SELECT " + nLastSeq + " AS FNHldNo, FTBchCode, FTXshDocNo, FTXshCardID, FTXshCardNo, FNXshCrTerm, FDXshDueDate, FDXshBillDue, FTXshCtrName, FDXshTnfDate, FTXshRefTnfID, FNXshAddrShip, FNXshAddrTax, FTXshCstTel, FTXshCstName");
                oSql.AppendLine("FROM " + tC_TblSalHDCst + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("");
                //oSql.AppendLine("INSERT INTO TPSTHoldDT WITH(ROWLOCK)(FNHldNo, FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTXsdStkCode, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");
                oSql.AppendLine("INSERT INTO TPSTHoldDT (FNHldNo, FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FTPplCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");   //*Em 62-06-29 //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("FTXsdSaleType, FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, ");
                oSql.AppendLine("FCXsdWhtRate, FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                //oSql.AppendLine("SELECT " + nLastSeq + " AS FNHldNo, FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTXsdStkCode, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");
                oSql.AppendLine("SELECT " + nLastSeq + " AS FNHldNo, FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FTPplCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, "); //*Em 62-06-26
                oSql.AppendLine("FTXsdSaleType, FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode,");
                oSql.AppendLine("FCXsdWhtRate, FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                oSql.AppendLine("FROM " + tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND FTXsdStaPdt <> '4'");  //*Em 63-05-14
                oSql.AppendLine("");
                oSql.AppendLine("INSERT INTO TPSTHoldDTDis (FNHldNo, FTBchCode, FTXihDocNo, FNXidSeqNo, FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue, FTXddRefCode, FTDisCode, FTRsnCode)");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก , *Arm 63-09-16 -เพิ่ม FTXddRefCode, FTDisCode, FTRsnCode
                oSql.AppendLine("SELECT " + nLastSeq + " AS FNHldNo, DTD.FTBchCode, DTD.FTXshDocNo, DTD.FNXsdSeqNo, DTD.FDXddDateIns, DTD.FNXddStaDis, DTD.FTXddDisChgTxt, DTD.FTXddDisChgType, DTD.FCXddNet, DTD.FCXddValue, DTD.FTXddRefCode, DTD.FTDisCode, DTD.FTRsnCode"); //*Arm 63-09-16 -เพิ่ม FTXddRefCode, FTDisCode, FTRsnCode
                oSql.AppendLine("FROM " + tC_TblSalDTDis + " DTD WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN " + tC_TblSalDT + " DT WITH(NOLOCK) ON DT.FTBchCode = DTD.FTBchCode AND DT.FTXshDocNo = DTD.FTXshDocNo AND DT.FNXsdSeqNo = DTD.FNXsdSeqNo");
                oSql.AppendLine("WHERE DTD.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND DTD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND DT.FTXsdStaPdt <> '4'");  //*Em 63-05-14
                oDB.C_SETxDataQuery(oSql.ToString());

                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : C_INSxShiftEvent", cVB.bVB_AlwPrnLog);
                oEvent = new cmlTPSTShiftEvent();
                oEvent.FTBchCode = cVB.tVB_BchCode;
                oEvent.FTShfCode = cVB.tVB_ShfCode;
                oEvent.FTPosCode = cVB.tVB_PosCode; //*Em 62-01-03  เพิ่ม FTPosCode
                oEvent.FNSdtSeqNo = cVB.nVB_ShfSeq; //*Em 62-08-15
                oEvent.FDHisDateTime = DateTime.Now;
                oEvent.FTEvnCode = "003";
                oEvent.FNSvnQty = 1;
                oEvent.FCSvnAmt = 0;
                oEvent.FTRsnCode = "";
                oEvent.FTSvnApvCode = cVB.tVB_UsrCode;
                oEvent.FTSvnRemark = "";
                new cShiftEvent().C_INSxShiftEvent(oEvent);

                //C_PRCxPrintHoldBill();

                //*Em 63-03-02 , Arm 63-03-05 ปรับตาม Baseline
                //Thread oPrn = new Thread(new ThreadStart(C_PRCxPrintHoldBill));
                //oPrn.Start();
                //++++++++++++++++++++
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : C_PRCxPrintHoldBill", cVB.bVB_AlwPrnLog);
                if (cVB.bPS_PrintHoldBill) //*Net 63-05-27 ตรวจสอบ option ว่าต้องพิมพ์หรือไม่
                {
                    //*Net 63-06-17 ตั้งค่า DocNoPrint
                    cVB.tVB_DocNoPrn = cVB.tVB_DocNo;
                    cSale.bC_PrnCopy = true;
                    cSale.tC_RefDocNoPrn = cVB.tVB_RefDocNo;
                    cSale.tC_TxnRefCode = (String.IsNullOrEmpty(cVB.tVB_RefDocNo)) ? cVB.tVB_DocNo : cVB.tVB_RefDocNo;
                    cSale.nC_PrnDocType = cSale.nC_DocType;
                    //+++++++++++++++++++++++
                    C_PRCxPrintHoldBill();
                }

                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSale);
                //new wSale(3).Show();
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : W_SETxNewDoc", cVB.bVB_AlwPrnLog);
                cVB.oVB_Sale.W_SETxNewDoc();    //*Em 63-03-02, Arm 63-03-05 ปรับตาม Baseline
                cVB.oVB_Sale.bW_Activate = false;
                cVB.oVB_Sale.Show();    //*Em 63-04-23
                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSale);
                //new wSale(3).Show();
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : End", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : " + oEx.Message);
            }
            finally
            {
                if (oFormShow != null)
                    oFormShow.Close();

                if (oReason != null)
                    oReason.Close();

                oDB = null;
                oSql = null;
                oFormShow = null;
                oReason = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxRetriveBill()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            Form oFormShow = null;

            try
            {

                if (cSale.nC_CntItem > 0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgHaveItem"), 1);
                    return;
                }

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FNHldNo FROM TPSTHoldHD WITH(NOLOCK)");
                DataTable odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                if (odtTmp == null || odtTmp.Rows.Count == 0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoData"), 1);
                    return;
                }

                nC_HoldNo = 0;

                oFormShow = new wRetriveBill();
                oFormShow.ShowDialog();
                new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : Start", cVB.bVB_AlwPrnLog);
                if (nC_HoldNo != 0)
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : C_GETxHoldHDCst", cVB.bVB_AlwPrnLog);
                    C_GETxHoldHDCst();  //*Em 62-12-18
                    new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : C_GETxHoldDT2Sale", cVB.bVB_AlwPrnLog);
                    C_GETxHoldDT2Sale();
                }
                new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : End", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : " + oEx.Message);
            }
            finally
            {
                if (oFormShow != null)
                    oFormShow.Close();

                oDB = null;
                oSql = null;
                oFormShow = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

        }

        private static void C_GETxHoldDT2Sale()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalDTDis> aoDTDis;

            try
            {
                cVB.bVB_RetriveBill = true;
                C_PRCxInsertRetriveBill();

                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FCXsdQty AS cQty, FCXsdFactor AS cFactor, FTPunName AS tUnit,");
                //oSql.AppendLine("FTPdtCode AS tPdtCode, FTXsdBarCode AS tBarcode, FTXsdPdtName AS tPdtName,");
                //oSql.AppendLine("FCXsdSetPrice AS cSetPrice, FTXsdStaPdt AS tStaPdt, FTXsdStaAlwDis AS tStaAlwDis");
                //oSql.AppendLine("FROM TPSTHoldDT WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FNHldNo = '" + nC_HoldNo + "' ");
                ////oSql.AppendLine("AND FTXsdStaPdt<>4"); //*Net 63-03-28 ยกมาจาก baseline ไม่เอารายการ void
                //List<cmlPdtOrder> aoPdtOrder = oDB.C_GETaDataQuery<cmlPdtOrder>(oSql.ToString());

                //oSql.Clear();
                //oSql.AppendLine("SELECT FTBchCode,'" + cVB.tVB_DocNo + "' AS FTXshDocNo,FNXidSeqNo AS FNXsdSeqNo,FDXddDateIns,");
                //oSql.AppendLine("FNXddStaDis,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                //oSql.AppendLine("FROM TPSTHoldDTDis WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FNHldNo = '" + nC_HoldNo + "' ");
                //aoDTDis = oDB.C_GETaDataQuery<cmlTPSTSalDTDis>(oSql.ToString());

                ///*foreach (cmlPdtOrder oPdtOrder in aoPdtOrder) 
                //{
                //    cVB.oVB_Sale.W_ADDxPdtToOrder(oPdtOrder);
                //    foreach (cmlTPSTSalDTDis oDTDis in aoDTDis.Where(o => o.FNXsdSeqNo == nC_DTSeqNo))
                //    {
                //        cVB.oVB_OrderRowIndex = nC_DTSeqNo - 1;
                //        new cSale().C_PRCxDisChgItem(oDTDis);
                //    }
                //}
                ////*Net 63-04-08
                //int nSeq = 0;
                //for (int nIndex = 0; nIndex < aoPdtOrder.Count; nIndex++)
                //{
                //    if (aoPdtOrder[nIndex].tStaPdt != "4")
                //    {
                //        cVB.oVB_Sale.W_ADDxPdtToOrder(aoPdtOrder[nIndex]);
                //        nSeq++;
                //        if (aoDTDis.Count > 0)
                //        {
                //            foreach (cmlTPSTSalDTDis oDTDis in aoDTDis.Where(o => o.FNXsdSeqNo == nIndex + 1))
                //            {
                //                cVB.oVB_OrderRowIndex = nSeq - 1;
                //                new cSale().C_PRCxDisChgItem(oDTDis);
                //            }
                //        }
                //    }
                //}

                //*Em 63-05-15
                cVB.oVB_Sale.W_PRCxLoadItemRetriveBill();
                cVB.oVB_Sale.bW_CalPmtPrice = true;  //*Em 63-05-05
                cVB.oVB_Sale.W_PRCxCoPmt();
                //+++++++++++++++++++

                //Delect from table Hold
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM TPSTHoldHD WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FNHldNo = '" + nC_HoldNo + "' ");
                oSql.AppendLine("DELETE FROM TPSTHoldHDCst WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FNHldNo = '" + nC_HoldNo + "' ");
                oSql.AppendLine("DELETE FROM TPSTHoldDT WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FNHldNo = '" + nC_HoldNo + "' ");
                oSql.AppendLine("DELETE FROM TPSTHoldDTDis WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FNHldNo = '" + nC_HoldNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());
                cVB.bVB_RetriveBill = false;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_GETxHoldDT2Sale : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        private static void C_GETxHoldHDCst()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            cmlTPSTSalHDCst oHDCst = new cmlTPSTSalHDCst();
            cmlResCst oCstSch;
            cmlReqCstSch oReq;

            cClientService oCall;
            HttpResponseMessage oRep;
            //cmlResCstKAD aoCstKAD;
            cmlResCstKAD oResCst;
            //DataTable oDbTbl;
            DataTable odtPrivil;
            try
            {
                new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : start.", cVB.bVB_AlwPrnLog);

                if (string.IsNullOrEmpty(cVB.tVB_CstCode)) return;

                //*Arm 63-05-01 Comment Code
                //oSql.AppendLine("SELECT FTXshCstName, FTXshCstTel");
                //oSql.AppendLine("FROM TPSTHoldHDCst WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FNHldNo = '" + nC_HoldNo + "' ");
                //oHDCst = oDB.C_GEToDataQuery<cmlTPSTSalHDCst>(oSql.ToString());
                //if (oHDCst != null)
                //{
                //    cVB.tVB_CstName = oHDCst.FTXshCstName;
                //    cVB.tVB_CstTel = oHDCst.FTXshCstTel;
                //    cVB.tVB_PriceGroup = oDB.C_GEToDataQuery<string>("SELECT TOP 1 FTPplCodeRet FROM TCNMCst WITH(NOLOCK) WHERE FTCstCode = '" + cVB.tVB_CstCode + "'");
                //    cVB.tVB_QMemMsgID = "";

                //    cVB.oVB_Sale.W_SETxTextCst();
                //    //cSale.C_DATxInsHDCst(cVB.tVB_CstCode);
                //}


                if (!string.IsNullOrEmpty(cVB.tVB_ApiCstSch) && cVB.tVB_ApiCstSch_Fmt == "SKC")
                {
                    #region SKC(KADS)

                    //new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : get CstTel & CardID.", cVB.bVB_AlwPrnLog);

                    //// หา ข้อมูลเบอร์โทร หรือเลขบัตรประชาชน
                    //oSql.Clear();
                    //oSql.AppendLine("SELECT FTXshCstTel,FTXshCardID");
                    //oSql.AppendLine("FROM TPSTHoldHDCst WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FNHldNo  = '"+ cSale.nC_HoldNo + "' ");

                    //DataTable odtTmp = new DataTable();
                    //odtTmp = oDB.C_GEToDataQuery(oSql.ToString());

                    //if (odtTmp != null && odtTmp.Rows.Count >0)
                    //{
                    //    cVB.tVB_CstTel = odtTmp.Rows[0].Field<string>("FTXshCstTel");
                    //    cVB.tVB_CstCardID = odtTmp.Rows[0].Field<string>("FTXshCardID");
                    //}

                    //new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Result CstTel : "+ cVB.tVB_CstTel + "/ CardID : " + cVB.tVB_CstCardID, cVB.bVB_AlwPrnLog);
                    //new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : search customer SKC start.", cVB.bVB_AlwPrnLog);

                    //cServiceKADS.C_GETxHDCst(cVB.tVB_CstTel, cVB.tVB_CstCardID); //*Arm 63-08-15

                    //*Arm 63-09-11
                    new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : CstCode : " + cVB.tVB_CstCode , cVB.bVB_AlwPrnLog);
                    new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : search customer SKC start.", cVB.bVB_AlwPrnLog);
                    cServiceKADS.C_GETxHDCst(cVB.tVB_CstCode); 
                    //+++++++++++++


                    //cClientService oCallX;
                    //HttpResponseMessage oRepX;
                    //cmlResCstKAD aoCstKAD;
                    //DataTable oDbTbl;
                    //string tJSonRes = "";
                    //string tFunc = "";
                    //if (string.IsNullOrEmpty(cVB.tVB_CstTel) && string.IsNullOrEmpty(cVB.tVB_CstCardID)) return;

                    //if (!string.IsNullOrEmpty(cVB.tVB_CstTel))
                    //{
                    //    tFunc = "?$filter=PhoneSearch eq '" + cVB.tVB_CstTel + "'&$expand=PrivilegePointSet"; //*Arm 63-06-26
                    //}
                    //else
                    //{
                    //    tFunc = "?$filter=TaxID eq '" + cVB.tVB_CstCardID + "'&$expand=PrivilegePointSet";
                    //}
                    //new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api search customer/url : " + cVB.tVB_ApiCstSch + tFunc, cVB.bVB_AlwPrnLog);
                    //new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api search customer/url : " + cVB.tVB_ApiCstSch_Auth, cVB.bVB_AlwPrnLog);
                    //oCallX = new cClientService();
                    //oCallX = new cClientService("Authorization", cVB.tVB_ApiCstSch_Auth);

                    //new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api search customer start.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03

                    //oRepX = new HttpResponseMessage();
                    //try
                    //{
                    //    oRepX = oCallX.C_GEToInvoke(cVB.tVB_ApiCstSch + tFunc);
                    //    oRepX.EnsureSuccessStatusCode();
                    //}
                    //catch (HttpRequestException oEx)
                    //{
                    //    new cSP().SP_SHWxMsg("Error : " + oEx.Message.ToString(), 2);
                    //    new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api search customer Error /" + oEx.Message.ToString()); //*Arm 63-08-03
                    //    return;
                    //}
                    //catch (Exception oEx)
                    //{
                    //    new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api search customer Error /" + oEx.Message.ToString()); //*Arm 63-08-03
                    //    return;
                    //}
                    //new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api search customer End.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03

                    //if (oRepX.StatusCode == System.Net.HttpStatusCode.OK)
                    //{
                    //    tJSonRes = oRepX.Content.ReadAsStringAsync().Result;
                    //    new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api search customer /Response : " + tJSonRes, cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //}

                    //if (string.IsNullOrEmpty(tJSonRes))
                    //{
                    //    //
                    //}
                    //else
                    //{
                    //    aoCstKAD = new cmlResCstKAD();
                    //    aoCstKAD = JsonConvert.DeserializeObject<cmlResCstKAD>(tJSonRes);
                    //    oDbTbl = new DataTable();

                    //    if (aoCstKAD != null && aoCstKAD.d != null && aoCstKAD.d.results.Count > 0)
                    //    {
                    //        if (aoCstKAD.d.results.Count > 1)
                    //        {
                    //            foreach (cmlResKunnr oResKunnr in aoCstKAD.d.results)
                    //            {
                    //                if (oResKunnr.CustomerCode == cVB.tVB_CstCode)
                    //                {
                    //                    if (!string.IsNullOrEmpty(oResKunnr.BUGroup))
                    //                    {
                    //                        // BUGroup = ZAR6
                    //                        if (oResKunnr.BUGroup == "ZAR6")
                    //                        {
                    //                            new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : BUGroup == ZAR6 (Customer No CstCode)", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                            new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api create customer Start", cVB.bVB_AlwPrnLog); //*Arm 63-08-03

                    //                            //*Arm 63-07-20
                    //                            cmlReqCustomerCreate oReqCst = new cmlReqCustomerCreate();
                    //                            oReqCst.CustomerCode = oResKunnr.CustomerCode;
                    //                            oReqCst.Title = oResKunnr.Title;
                    //                            oReqCst.FirstName = oResKunnr.FirstName;
                    //                            oReqCst.LastName = oResKunnr.LastName;
                    //                            oReqCst.Titlee = oResKunnr.Titlee;
                    //                            oReqCst.Namee = oResKunnr.Namee;
                    //                            oReqCst.Surnamee = oResKunnr.Surnamee;
                    //                            oReqCst.Addr = oResKunnr.Addr;
                    //                            oReqCst.Soi = oResKunnr.Soi;
                    //                            oReqCst.Street = oResKunnr.Street;
                    //                            oReqCst.District = oResKunnr.District;
                    //                            oReqCst.City = oResKunnr.City;
                    //                            oReqCst.Province = oResKunnr.Province;
                    //                            oReqCst.Gender = oResKunnr.Gender;
                    //                            oReqCst.Birth = oResKunnr.Birth;
                    //                            oReqCst.Email = oResKunnr.Email;
                    //                            oReqCst.Mobile = oResKunnr.Mobile;
                    //                            oReqCst.Remark = oResKunnr.Remark;
                    //                            string tJsonCall = JsonConvert.SerializeObject(oReqCst);

                    //                            new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : C_PRCxCreateCustomer Start.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                            new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : C_PRCxCreateCustomer call create Customer/Request : " + tJsonCall, cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                            tJSonRes = wCstSearch.C_PRCxCreateCustomer(tJsonCall);
                    //                            new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : C_PRCxCreateCustomer call create Customer/Response : " + tJSonRes, cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                            new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : C_PRCxCreateCustomer End.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03

                    //                            if (!string.IsNullOrEmpty(tJSonRes))
                    //                            {
                    //                                cmlResCreateCustomerCode oCst = new cmlResCreateCustomerCode();
                    //                                oCst = JsonConvert.DeserializeObject<cmlResCreateCustomerCode>(tJSonRes);

                    //                                if (oCst != null && oCst.d != null && oCst.d.BUGroup == "ZAR1")
                    //                                {
                    //                                    if (oCst.d.PrivilegePointSet != null && oCst.d.PrivilegePointSet.results != null && oCst.d.PrivilegePointSet.results.Count > 0)
                    //                                    {
                    //                                        string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oCst.d.PrivilegePointSet.results);
                    //                                        oDbTbl = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);
                    //                                        wCstSearch.W_INSxPrivilege(oDbTbl);
                    //                                    }

                    //                                    cVB.nVB_CstPoint = oCst.d.Point;
                    //                                    cVB.nVB_CstPiontB4Used = oCst.d.Point;
                    //                                    cVB.tVB_CstName = oCst.d.Title + " " + oCst.d.FirstName + " " + oCst.d.LastName;
                    //                                    cVB.tVB_KubotaID = oCst.d.KubotaID;
                    //                                    cVB.tVB_CstCode = oCst.d.CustomerCode;
                    //                                    cVB.tVB_CstTel = oCst.d.PhoneNo;
                    //                                    cVB.tVB_CstCardID = oCst.d.TaxID;

                    //                                    if (string.IsNullOrEmpty(oCst.d.Membership))
                    //                                    {
                    //                                        cVB.tVB_MemCode = "";
                    //                                        cVB.bVB_Flag = false;
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        cVB.tVB_MemCode = oCst.d.Membership;   //*Arm 63-08-11
                    //                                        cVB.bVB_Flag = true;
                    //                                    }

                    //                                    cVB.oVB_Sale.W_SETxTextCst();
                    //                                    C_DATxInsHDCst(cVB.tVB_CstCode);
                    //                                }
                    //                                else
                    //                                {
                    //                                    new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : BUGroup = ZAR6 (After call api create customer)", cVB.bVB_AlwPrnLog);
                    //                                }
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            // BUGroup = ZAR 1
                    //                            string tJsonPrivil = Newtonsoft.Json.JsonConvert.SerializeObject(oResKunnr.PrivilegePointSet.results);
                    //                            oDbTbl = JsonConvert.DeserializeObject<DataTable>(tJsonPrivil);
                    //                            wCstSearch.W_INSxPrivilege(oDbTbl);

                    //                            cVB.nVB_CstPoint = oResKunnr.Point;
                    //                            cVB.nVB_CstPiontB4Used = oResKunnr.Point;
                    //                            cVB.tVB_CstName = oResKunnr.Title + " " + oResKunnr.FirstName + " " + oResKunnr.LastName;
                    //                            cVB.tVB_KubotaID = oResKunnr.KubotaID;
                    //                            cVB.tVB_CstCode = oResKunnr.CustomerCode;
                    //                            cVB.tVB_CstTel = oResKunnr.PhoneNo;
                    //                            cVB.tVB_CstCardID = oResKunnr.TaxID;

                    //                            if (string.IsNullOrEmpty(oResKunnr.Membership))
                    //                            {
                    //                                cVB.tVB_MemCode = "";
                    //                                cVB.bVB_Flag = false;
                    //                            }
                    //                            else
                    //                            {
                    //                                cVB.tVB_MemCode = oResKunnr.Membership;   //*Arm 63-08-11
                    //                                cVB.bVB_Flag = true;
                    //                            }

                    //                            cVB.oVB_Sale.W_SETxTextCst();
                    //                            C_DATxInsHDCst(cVB.tVB_CstCode);
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : BUGroup = '' (call api search customer)", cVB.bVB_AlwPrnLog);
                    //                    }

                    //                    break;
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            // Respons 1 ราย
                    //            foreach (cmlResKunnr oResKunnr in aoCstKAD.d.results)
                    //            {
                    //                if (!string.IsNullOrEmpty(oResKunnr.BUGroup))
                    //                {
                    //                    // BUGroup = ZAR6
                    //                    if (oResKunnr.BUGroup == "ZAR6")
                    //                    {
                    //                        new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : BUGroup == ZAR6 (Customer No CstCode)", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                        new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : Call api create customer Start", cVB.bVB_AlwPrnLog); //*Arm 63-08-03

                    //                        //*Arm 63-07-20
                    //                        cmlReqCustomerCreate oReqCst = new cmlReqCustomerCreate();
                    //                        oReqCst.CustomerCode = oResKunnr.CustomerCode;
                    //                        oReqCst.Title = oResKunnr.Title;
                    //                        oReqCst.FirstName = oResKunnr.FirstName;
                    //                        oReqCst.LastName = oResKunnr.LastName;
                    //                        oReqCst.Titlee = oResKunnr.Titlee;
                    //                        oReqCst.Namee = oResKunnr.Namee;
                    //                        oReqCst.Surnamee = oResKunnr.Surnamee;
                    //                        oReqCst.Addr = oResKunnr.Addr;
                    //                        oReqCst.Soi = oResKunnr.Soi;
                    //                        oReqCst.Street = oResKunnr.Street;
                    //                        oReqCst.District = oResKunnr.District;
                    //                        oReqCst.City = oResKunnr.City;
                    //                        oReqCst.Province = oResKunnr.Province;
                    //                        oReqCst.Gender = oResKunnr.Gender;
                    //                        oReqCst.Birth = oResKunnr.Birth;
                    //                        oReqCst.Email = oResKunnr.Email;
                    //                        oReqCst.Mobile = oResKunnr.Mobile;
                    //                        oReqCst.Remark = oResKunnr.Remark;
                    //                        string tJsonCall = JsonConvert.SerializeObject(oReqCst);

                    //                        new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : C_PRCxCreateCustomer Start.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                        new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : C_PRCxCreateCustomer call create Customer/Request : " + tJsonCall, cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                        tJSonRes = wCstSearch.C_PRCxCreateCustomer(tJsonCall);
                    //                        new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : C_PRCxCreateCustomer call create Customer/Response : " + tJSonRes, cVB.bVB_AlwPrnLog); //*Arm 63-08-03
                    //                        new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : C_PRCxCreateCustomer End.", cVB.bVB_AlwPrnLog); //*Arm 63-08-03

                    //                        if (!string.IsNullOrEmpty(tJSonRes))
                    //                        {
                    //                            cmlResCreateCustomerCode oCst = new cmlResCreateCustomerCode();
                    //                            oCst = JsonConvert.DeserializeObject<cmlResCreateCustomerCode>(tJSonRes);

                    //                            if (oCst != null && oCst.d != null && oCst.d.BUGroup == "ZAR1")
                    //                            {
                    //                                if (oCst.d.PrivilegePointSet != null && oCst.d.PrivilegePointSet.results != null && oCst.d.PrivilegePointSet.results.Count > 0)
                    //                                {
                    //                                    string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oCst.d.PrivilegePointSet.results);
                    //                                    oDbTbl = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);
                    //                                    wCstSearch.W_INSxPrivilege(oDbTbl);
                    //                                }

                    //                                cVB.nVB_CstPoint = oCst.d.Point;
                    //                                cVB.nVB_CstPiontB4Used = oCst.d.Point;
                    //                                cVB.tVB_CstName = oCst.d.Title + " " + oCst.d.FirstName + " " + oCst.d.LastName;
                    //                                cVB.tVB_KubotaID = oCst.d.KubotaID;
                    //                                cVB.tVB_CstCode = oCst.d.CustomerCode;
                    //                                cVB.tVB_CstTel = oCst.d.PhoneNo;
                    //                                cVB.tVB_CstCardID = oCst.d.TaxID;

                    //                                if (string.IsNullOrEmpty(oCst.d.Membership))
                    //                                {
                    //                                    cVB.tVB_MemCode = "";
                    //                                    cVB.bVB_Flag = false;
                    //                                }
                    //                                else
                    //                                {
                    //                                    cVB.tVB_MemCode = oCst.d.Membership;
                    //                                    cVB.bVB_Flag = true;
                    //                                }
                    //                                cVB.oVB_Sale.W_SETxTextCst();
                    //                                C_DATxInsHDCst(cVB.tVB_CstCode);
                    //                            }
                    //                            else
                    //                            {
                    //                                new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : BUGroup = ZAR6 (After call api create customer)", cVB.bVB_AlwPrnLog);
                    //                            }
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        // BUGroup = ZAR 1
                    //                        string tJsonPrivil = Newtonsoft.Json.JsonConvert.SerializeObject(oResKunnr.PrivilegePointSet.results);
                    //                        oDbTbl = JsonConvert.DeserializeObject<DataTable>(tJsonPrivil);
                    //                        wCstSearch.W_INSxPrivilege(oDbTbl);

                    //                        cVB.nVB_CstPoint = oResKunnr.Point;
                    //                        cVB.nVB_CstPiontB4Used = oResKunnr.Point;
                    //                        cVB.tVB_CstName = oResKunnr.Title + " " + oResKunnr.FirstName + " " + oResKunnr.LastName;
                    //                        cVB.tVB_KubotaID = oResKunnr.KubotaID;
                    //                        cVB.tVB_CstCode = oResKunnr.CustomerCode;
                    //                        cVB.tVB_CstTel = oResKunnr.PhoneNo;
                    //                        cVB.tVB_CstCardID = oResKunnr.TaxID;

                    //                        if (string.IsNullOrEmpty(oResKunnr.Membership))
                    //                        {
                    //                            cVB.tVB_MemCode = "";
                    //                            cVB.bVB_Flag = false;
                    //                        }
                    //                        else
                    //                        {
                    //                            cVB.tVB_MemCode = oResKunnr.Membership;   //*Arm 63-08-11
                    //                            cVB.bVB_Flag = true;
                    //                        }
                    //                        cVB.oVB_Sale.W_SETxTextCst();
                    //                        C_DATxInsHDCst(cVB.tVB_CstCode);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : BUGroup = '' (call api search customer)", cVB.bVB_AlwPrnLog);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion END SKC(KADS)
                }
                else
                {
                    #region ADA (STD)
                    //*Arm 63-05-01 เอาข้อมูลแต้มลูกค้าล่าสุดมาจาก หลังบ้าน
                    oReq = new cmlReqCstSch();
                    oReq.ptCstName = "";
                    oReq.ptCstCode = cVB.tVB_CstCode;
                    oReq.ptCstTel = "";
                    oReq.ptCstCardID = "";
                    oReq.ptCstCrdNo = "";
                    oReq.ptCstTaxNo = "";

                    string tJSonCall = JsonConvert.SerializeObject(oReq);
                    string tUrl = cVB.tVB_API2PSMaster;
                    oCall = new cClientService();
                    oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                    oRep = new HttpResponseMessage();
                    try
                    {
                        oRep = oCall.C_POSToInvoke(tUrl + "/Customer/CstSearch", tJSonCall);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst/Call API Error : " + oEx.Message);
                    }
                    if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                        oCstSch = new cmlResCst();
                        oCstSch = JsonConvert.DeserializeObject<cmlResCst>(tJSonRes);

                        switch (oCstSch.rtCode)
                        {
                            case "1":
                                if (oCstSch.raItems.Count > 0)
                                {
                                    foreach (cmlResCstSch oData in oCstSch.raItems)
                                    {
                                        cVB.nVB_CstPoint = Convert.ToInt32(oData.rtTxnPntQty);
                                        cVB.tVB_CstName = oData.rtCstName;
                                        cVB.tVB_CstTel = oData.rtCstTel;
                                        cVB.tVB_CstStaAlwPosCalSo = oData.rtCstStaAlwPosCalSo;
                                        cVB.tVB_PriceGroup = oData.rtPplCodeRet;
                                        cVB.tVB_CstCardID = oData.rtCstCardID;
                                        cVB.tVB_MemberCard = oData.rtCstCrdNo;
                                        cVB.tVB_ExpiredDate = oData.rdCstCrdExpire == null ? string.Empty : string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(oData.rdCstCrdExpire)); //*Arm 63-04-29
                                        cVB.tVB_MemCode = oData.rtCstCode; //*Arm 63-08-11
                                    }
                                }
                                break;

                            case "800":
                                break;
                            default:
                                //ERROR
                                new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst/API Response Error : " + oCstSch.rtDesc);
                                break;
                        }
                        cVB.nVB_CstPiontB4Used = cVB.nVB_CstPoint;  // Point ก่อนใช้
                    }

                    //*Net 63-07-30 ปรับตาม Moshi
                    oCall.C_PRCxCloseConn();    //*Em 63-07-18

                    oCall = null;
                    tJSonCall = null;
                    oReq = null;
                    oRep = null;
                    //*Net 63-07-30 ปรับตาม Moshi
                    if (cVB.tVB_PriceGroup == null) cVB.tVB_PriceGroup = "";    //*Em 63-06-17

                    cVB.oVB_Sale.W_SETxTextCst();
                    cSale.C_DATxInsHDCst(cVB.tVB_CstCode);

                    //+++++++++++++++
                    #endregion  ADA (STD)
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_GETxHoldHDCst : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                oCstSch = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxClearPara()
        {
            try
            {
                cVB.tVB_CstCode = "";
                cVB.tVB_ParcelCode = "";
                cVB.tVB_CstTel = "";
                cVB.tVB_CstName = "";
                cVB.tVB_PriceGroup = "";
                cVB.tVB_QMemMsgID = "";//*Arm 62-10-27
                cVB.tVB_MemCode = "";   //*Em 63-08-14
                cVB.tVB_CstCardID = ""; //*Arm 63-08-17

                cVB.cVB_QRPayAmt = 0;
                cVB.bVB_ScanQR = false;

                cVB.nVB_CstPoint = 0;   //*Arm 63-03-12
                cVB.nVB_CstPiontB4Used = 0; //*Arm 63-03-16
                nC_RDSeqNo = 0; //*Arm 63-03-20 - Clear ลำดับ TPSTSalRD

                cVB.tVB_PriceGroup = cVB.tVB_BchPriceGroup; //* Net 63-03-24
                cVB.bVB_PriceConfirm = false;   //*Em 63-05-06

                //*Net 63-06-01 Clear ตารางเก่า
                if (cVB.oVB_GetPmt != null)
                {
                    cVB.oVB_GetPmt.Clear();
                    cVB.oVB_GetPmt.Dispose();
                    cVB.oVB_GetPmt = null;
                }
                if (cVB.oVB_PmtSug != null)
                {
                    cVB.oVB_PmtSug.Clear();
                    cVB.oVB_PmtSug.Dispose();
                    cVB.oVB_PmtSug = null;
                }
                cVB.oVB_GetPmt = new DataTable();   //*Em 63-05-27
                cVB.oVB_PmtSug = new DataTable();   //*Em 63-05-27
                cVB.oVB_CstPrivilege = new DataTable(); //*Em 63-09-16

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxClearPara : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxAdd2TmpLogChg(int pnType, string ptDocNo, bool pbTrans = false)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tTblSalHD; //*Net 63-07-25
            try
            {
                //*Net 63-07-30
                if (pbTrans) tTblSalHD = "TPSTSalHD";
                else tTblSalHD = "TSHD" + cVB.tVB_PosCode;
                //++++++++++++++++++++++++++++++++++
                switch (pnType)
                {
                    case 80: //การขาย-คืน
                        //*Net 63-07-25 ถ้าเป็น Tmp ให้ check ตารางก่อน
                        oSql.AppendLine($"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tTblSalHD}')) BEGIN");
                        oSql.AppendLine("INSERT INTO TCNTTmpLogChg (FTLogCode, FTLogDocNo, FNLogType, FTWahCode, FDCreateOn)");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                        oSql.AppendLine("SELECT FTShfCode, FTXshDocNo, 80 AS FNLogType, FTWahCode, GETDATE() AS FDCreateOn");
                        if (pbTrans)
                        {
                            oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                        }
                        else
                        {
                            //oSql.AppendLine("FROM " + tC_TblSalHD + " WITH(NOLOCK)");
                            oSql.AppendLine("FROM TSHD" + cVB.tVB_PosCode + " WITH(NOLOCK)"); //*Net 63-06-05
                        }
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptDocNo + "'");
                        oSql.AppendLine("END"); //*Net 63-07-25
                        oDB.C_SETxDataQuery(oSql.ToString());
                        break;
                    case 81:    //รอบการขาย
                        oSql.AppendLine("INSERT INTO TCNTTmpLogChg (FTLogCode, FTLogDocNo, FNLogType, FTWahCode, FDCreateOn)");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                        oSql.AppendLine("SELECT FTShfCode, FTPosCode, 81 AS FNLogType, '' AS FTWahCode, GETDATE() AS FDCreateOn");
                        oSql.AppendLine("FROM TPSTShiftHD WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTShfCode = '" + ptDocNo + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());
                        break;
                    case 82:    //ยกเลิกบิล-ยกเลิกรายการ
                        oSql.AppendLine("INSERT INTO TCNTTmpLogChg (FTLogCode, FTLogDocNo, FNLogType, FTWahCode, FDCreateOn)"); //*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                        oSql.AppendLine("SELECT DISTINCT FNVidNo, '' AS FTLogDocNo, 82 AS FNLogType, '' AS FTWahCode, GETDATE() AS FDCreateOn");
                        oSql.AppendLine("FROM TPSTVoidDT WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FNVidNo = '" + ptDocNo + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());
                        break;
                    case 90:    //ใบกำกำกับภาษี       //*Em 62-08-13
                        oSql.AppendLine("INSERT INTO TCNTTmpLogChg (FTLogCode, FTLogDocNo, FNLogType, FTWahCode, FDCreateOn)");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                        oSql.AppendLine("SELECT FTShfCode, FTXshDocNo, 90 AS FNLogType, FTWahCode, GETDATE() AS FDCreateOn");
                        oSql.AppendLine("FROM TPSTTaxHD WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptDocNo + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());
                        break;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxClearPara : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Open Cash Drawer
        /// </summary>
        public static void C_OPNxCashDrawer()
        {
            try
            {
                PrintDocument oDoc = new PrintDocument();
                PrinterSettings oSettings = new PrinterSettings();
                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;

                byte[] aCodeOpenCashDrawer = new byte[] { 27, 112, 48, 55, 121 };
                IntPtr nUnmanagedBytes = new IntPtr(0);
                nUnmanagedBytes = Marshal.AllocCoTaskMem(5);
                Marshal.Copy(aCodeOpenCashDrawer, 0, nUnmanagedBytes, 5);
                cRawPrinterHelper.SendBytesToPrinter(oSettings.PrinterName, nUnmanagedBytes, 5);
                Marshal.FreeCoTaskMem(nUnmanagedBytes);
                aCodeOpenCashDrawer = null;

                oDoc.Dispose();
                oDoc = null;
                oSettings = null;
                aCodeOpenCashDrawer = null;
                GC.Collect();
            }
            catch (Exception ex) { new cLog().C_WRTxLog("uMenuOne", "C_OPNxCashDrawer " + ex.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxOpenDrawer()
        {
            cmlTPSTShiftEvent oEvent;
            try
            {
                C_OPNxCashDrawer();

                oEvent = new cmlTPSTShiftEvent();
                oEvent.FTBchCode = cVB.tVB_BchCode;
                oEvent.FTShfCode = cVB.tVB_ShfCode;
                oEvent.FTPosCode = cVB.tVB_PosCode; //*Em 62-01-03  เพิ่ม FTPosCode
                oEvent.FNSdtSeqNo = cVB.nVB_ShfSeq; //*Em 62-08-15
                oEvent.FDHisDateTime = DateTime.Now;
                oEvent.FTEvnCode = "004";
                oEvent.FNSvnQty = 1;
                oEvent.FCSvnAmt = 0;
                oEvent.FTRsnCode = cVB.oVB_Reason.FTRsnCode;
                oEvent.FTSvnApvCode = cVB.tVB_UsrCode;
                oEvent.FTSvnRemark = "";
                new cShiftEvent().C_INSxShiftEvent(oEvent);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxOpenDrawer : " + oEx.Message);
            }
            finally
            {
                oEvent = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public void C_PRCxDisChgItem(cmlTPSTSalDTDis poSalDis)
        {
            StringBuilder oSql;
            decimal cDis;
            decimal cChg;
            List<cmlTPSTSalDTDis> aoDisChg;
            try
            {
                if (cVB.bVB_RetriveBill == false)
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("INSERT INTO " + tC_TblSalDTDis + " (");
                    oSql.AppendLine("FTBchCode");
                    oSql.AppendLine(",FTXshDocNo");
                    oSql.AppendLine(",FNXsdSeqNo");
                    oSql.AppendLine(",FDXddDateIns");
                    oSql.AppendLine(",FNXddStaDis");
                    oSql.AppendLine(",FTXddDisChgTxt");
                    oSql.AppendLine(",FTXddDisChgType");
                    oSql.AppendLine(",FCXddNet");
                    oSql.AppendLine(",FCXddValue");
                    oSql.AppendLine(",FTXddRefCode");  //*Net 63-03-17 Add Field
                    oSql.AppendLine(",FTDisCode");  //*Net 63-03-17 Add Field
                    oSql.AppendLine(",FTRsnCode)");  //*Net 63-03-17 Add Field
                    oSql.AppendLine(" VALUES (");
                    oSql.AppendLine("'" + poSalDis.FTBchCode + "'");
                    oSql.AppendLine(",'" + poSalDis.FTXshDocNo + "'");
                    oSql.AppendLine(",'" + nC_DTSeqNo + "'");
                    oSql.AppendLine(",GETDATE()");
                    oSql.AppendLine(",'" + poSalDis.FNXddStaDis + "'");
                    oSql.AppendLine(",'" + poSalDis.FTXddDisChgTxt + "'");
                    oSql.AppendLine(",'" + poSalDis.FTXddDisChgType + "'");
                    oSql.AppendLine(",'" + poSalDis.FCXddNet + "'");
                    oSql.AppendLine(",'" + poSalDis.FCXddValue + "'");
                    oSql.AppendLine(",'" + poSalDis.FTXddRefCode + "'");
                    oSql.AppendLine(",'" + poSalDis.FTDisCode + "'");
                    oSql.AppendLine(",'" + poSalDis.FTRsnCode + "')");
                    new cDatabase().C_SETxDataQuery(oSql.ToString());
                }


                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode ");
                oSql.AppendLine(",FTXshDocNo");
                oSql.AppendLine(",FNXsdSeqNo");
                oSql.AppendLine(",FDXddDateIns");
                oSql.AppendLine(",FNXddStaDis");
                oSql.AppendLine(",FTXddDisChgTxt");
                oSql.AppendLine(",FTXddDisChgType");
                oSql.AppendLine(",FCXddNet");
                oSql.AppendLine(",FCXddValue ");
                oSql.AppendLine("FROM " + tC_TblSalDTDis + " WHERE FTXshDocNo = '" + poSalDis.FTXshDocNo + "' AND FNXsdSeqNo = " + nC_DTSeqNo);
                oSql.AppendLine("ORDER BY FDXddDateIns ASC");
                aoDisChg = new List<cmlTPSTSalDTDis>();
                aoDisChg = new cDatabase().C_GETaDataQuery<cmlTPSTSalDTDis>(oSql.ToString());

                cDis = aoDisChg.Where(x => x.FTXddDisChgType == "1" || x.FTXddDisChgType == "2")
                    .Select(x => x.FCXddValue).Sum() ?? 0;

                cChg = aoDisChg.Where(x => x.FTXddDisChgType == "3" || x.FTXddDisChgType == "4")
                    .Select(x => x.FCXddValue).Sum() ?? 0;

                if (cVB.bVB_RetriveBill == false)
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE " + tC_TblSalDT + " SET ");
                    oSql.AppendLine(" FCXsdDis = " + cDis + " ");
                    oSql.AppendLine(",FCXsdChg = " + cChg + " ");
                    oSql.AppendLine(",FCXsdNet = FCXsdAmtB4DisChg - " + cDis + " + " + cChg + " ");
                    oSql.AppendLine(",FCXsdNetAfHD = FCXsdAmtB4DisChg - " + cDis + " + " + cChg + " ");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXsdSeqNo = " + nC_DTSeqNo + "");
                    new cDatabase().C_SETxDataQuery(oSql.ToString());

                    //*Net 63-07-30 ปรับตาม Moshi
                    //C_DATxUpdVat(); //*Em 62-10-08
                }

                //C_DATxUpdCost(); //*Em 62-10-08
                cVB.oVB_Sale.W_PRCxOrder(cVB.oVB_OrderRowIndex, cDis, cChg);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxDisItem : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                aoDisChg = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// เช็ค ชาทร์ราคา
        /// </summary>
        /// <returns></returns>
        public static bool C_PRCxCheckChg()
        {
            bool bResult = true;
            StringBuilder oSql;
            List<cmlTPSTSalDTDis> aoDisChg;
            int nChg;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode ");
                oSql.AppendLine(",FTXshDocNo");
                oSql.AppendLine(",FNXsdSeqNo");
                oSql.AppendLine(",FDXddDateIns");
                oSql.AppendLine(",FNXddStaDis");
                oSql.AppendLine(",FTXddDisChgTxt");
                oSql.AppendLine(",FTXddDisChgType");
                oSql.AppendLine(",FCXddNet");
                oSql.AppendLine(",FCXddValue ");
                oSql.AppendLine("FROM " + tC_TblSalDTDis + " WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXsdSeqNo = " + nC_DTSeqNo);
                aoDisChg = new List<cmlTPSTSalDTDis>();
                aoDisChg = new cDatabase().C_GETaDataQuery<cmlTPSTSalDTDis>(oSql.ToString());

                nChg = aoDisChg.Where(x => x.FTXddDisChgType == "3" || x.FTXddDisChgType == "4")
                    .Select(x => x.FCXddValue).Count();
                if (nChg > 0)
                {
                    bResult = false;
                }
                return bResult;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxCheckChg : " + oEx.Message);
                return bResult;
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxClearCallBack()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {

                if (cVB.oVB_CstScreen != null)
                {
                    cVB.oVB_CstScreen.W_CLExClearBillGrid();
                }
                //*Em 63-05-08
                if (cSale.nC_DocType == 9)
                {
                    oSql.Clear();
                    oSql.AppendLine("DELETE FROM " + tC_TblSalHDDis + " WITH(ROWLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine();
                    //*Net 63-07-30 ปรับตาม Moshi
                    //*Net 63-07-07 เมื่อย้อนกลับ บิลคืนลบตาราง RC ด้วย
                    oSql.AppendLine("DELETE FROM " + tC_TblSalRC + " WITH(ROWLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine();
                    //++++++++++++++++++++++++++++
                    oSql.AppendLine("UPDATE " + tC_TblSalHD + " WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTXshDisChgTxt = ''");
                    oSql.AppendLine(",FCXshDis = 0");
                    oSql.AppendLine(",FCXshChg = 0");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oDB.C_SETxDataQuery(oSql.ToString());
                    return;
                }
                //+++++++++++++++

                oSql.Clear();
                oSql.AppendLine("DELETE FROM " + tC_TblSalRC + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine();

                //*Arm 63-03-12
                oSql.AppendLine("DELETE FROM " + tC_TblSalRD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine();
                //+++++++++++++

                //*Em 63-07-11 ให้เคลียร์เฉพาะโปรโมชั่นที่เป็นส่วนลดตอน back กลับ
                //*Em 63-04-27 ไม่ต้องเคลียร์โปรโมชั่นตอน back กลับ
                ////*Em 63-03-29
                oSql.AppendLine("DELETE FROM " + tC_TblSalPD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND FTXpdGetType <> '4'");
                oSql.AppendLine();
                ////+++++++++++++++

                oSql.AppendLine("DELETE FROM " + tC_TblSalHDDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine();
                oSql.AppendLine("DELETE FROM " + tC_TblSalDTDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oSql.AppendLine("AND FNXddStaDis = 0 OR FNXddStaDis = 2");  //0:ลดโปรโมชั่น 1:ลดรายการ 2:ลดท้ายบิล
                oSql.AppendLine("AND (FNXddStaDis = 0 OR FNXddStaDis = 2)");  //0:ลดโปรโมชั่น 1:ลดรายการ 2:ลดท้ายบิล //*Arm 63-08-28
                oSql.AppendLine();
                oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdNetAfHD = FCXsdNet");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine();
                oSql.AppendLine("UPDATE " + tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FTXshDisChgTxt = ''");
                oSql.AppendLine(",FCXshDis = 0");
                oSql.AppendLine(",FCXshChg = 0");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());
                //*Net 63-07-30 ปรับตาม Moshi
                //C_DATxUpdVat();
                C_PRCxSummary2HD();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxClearCallBack : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        /// <summary>
        /// *Arm 63-03-12
        /// Process ส่วนลด Redeem แต้ม+เงิน  
        /// </summary>
        /// <param name="poSalDis"></param>
        public void C_PRCxRedeemDiscountItem(cmlRdSalDTDis poSalDis)
        {
            StringBuilder oSql;
            cDatabase oDB;
            cmlRdSalRD oSalRD;

            try
            {
                oDB = new cDatabase();
                oSql = new StringBuilder();

                oSql.Clear();
                oSql.AppendLine("INSERT INTO " + tC_TblSalDTDis + " (");
                oSql.AppendLine("FTBchCode");
                oSql.AppendLine(",FTXshDocNo");
                oSql.AppendLine(",FNXsdSeqNo");
                oSql.AppendLine(",FDXddDateIns");
                oSql.AppendLine(",FNXddStaDis");
                oSql.AppendLine(",FTXddDisChgTxt");
                oSql.AppendLine(",FTXddDisChgType");
                oSql.AppendLine(",FCXddNet");
                oSql.AppendLine(",FCXddValue");
                oSql.AppendLine(",FTXddRefCode)");
                oSql.AppendLine(" VALUES (");
                oSql.AppendLine("'" + poSalDis.FTBchCode + "'");
                oSql.AppendLine(",'" + poSalDis.FTXshDocNo + "'");
                oSql.AppendLine(",'" + poSalDis.FNXsdSeqNo + "'");
                oSql.AppendLine(",GETDATE()");
                oSql.AppendLine(",'" + poSalDis.FNXddStaDis + "'");
                oSql.AppendLine(",'" + poSalDis.FTXddDisChgTxt + "'");
                oSql.AppendLine(",'" + poSalDis.FTXddDisChgType + "'");
                oSql.AppendLine(",'" + poSalDis.FCXddNet + "'");
                oSql.AppendLine(",'" + poSalDis.FCXddValue + "'");
                oSql.AppendLine(",'" + poSalDis.FTXddRefCode + "')");
                oDB.C_SETxDataQuery(oSql.ToString());


                //// Insert SalRD
                //oSalRD = new cmlRdSalRD();
                //oSalRD.FTBchCode = poSalDis.FTBchCode;
                //oSalRD.FTXshDocNo = poSalDis.FTXshDocNo;
                //oSalRD.FTXrdRefCode = poSalDis.FTXddRefCode;
                //oSalRD.FTRdhDocType = poSalDis.FTRdhDocType;
                //oSalRD.FNXrdRefSeq = poSalDis.FNXsdSeqNo;
                //oSalRD.FCXrdPdtQty = poSalDis.FCXrdPdtQty;
                //oSalRD.FNXrdPntUse = poSalDis.FNXrdPntUse;
                //C_PRCxInsertSalRD(oSalRD);

                // Set NetAfHD
                oSql.Clear();
                oSql.AppendLine("UPDATE " + tC_TblSalDT + " SET ");
                oSql.AppendLine("FCXsdNetAfHD = " + poSalDis.FCXddNet + " - " + poSalDis.FCXddValue + " ");
                oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXsdSeqNo = " + poSalDis.FNXsdSeqNo + "");
                oDB.C_SETxDataQuery(oSql.ToString());

                //*Net 63-07-30 ปรับตาม Moshi
                // CalVat
                //C_DATxUpdVat();
                //C_DATxUpdCost();

                cVB.oVB_Payment.W_ADDxDisChgBill("8", poSalDis.FTXddDisChgTxt, (decimal)poSalDis.FCXddValue); //*Arm 63-03-22

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxRedeemDiscountItem : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                oSalRD = null;
                poSalDis = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// *Arm 63-03-12
        /// Insert  SalRD
        /// </summary>
        /// <param name="poSalRD"></param>
        public void C_PRCxInsertSalRD(cmlRdSalRD poSalRD)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                nC_RDSeqNo += 1;

                oDB = new cDatabase();
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + tC_TblSalRD + " (");
                oSql.AppendLine("FTBchCode");
                oSql.AppendLine(",FTXshDocNo");
                oSql.AppendLine(",FNXrdSeqNo");
                oSql.AppendLine(",FTRdhDocType");
                oSql.AppendLine(",FNXrdRefSeq");
                oSql.AppendLine(",FTXrdRefCode");
                oSql.AppendLine(",FCXrdPdtQty");
                oSql.AppendLine(",FNXrdPntUse)");
                oSql.AppendLine("VALUES (");
                oSql.AppendLine("'" + poSalRD.FTBchCode + "'");
                oSql.AppendLine(",'" + poSalRD.FTXshDocNo + "'");
                oSql.AppendLine(",'" + nC_RDSeqNo + "'");
                oSql.AppendLine(",'" + poSalRD.FTRdhDocType + "'");
                oSql.AppendLine(",'" + poSalRD.FNXrdRefSeq + "'");
                oSql.AppendLine(",'" + poSalRD.FTXrdRefCode + "'");
                oSql.AppendLine(",'" + poSalRD.FCXrdPdtQty + "'");
                oSql.AppendLine(",'" + poSalRD.FNXrdPntUse + "')");

                oDB.C_SETxDataQuery(oSql.ToString());

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxInsertSalRD : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                poSalRD = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRCxInsertRetriveBill()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                //Delete
                oSql.AppendLine("DELETE FROM " + tC_TblSalHDCst + " WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("DELETE FROM " + tC_TblSalDT + " WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("DELETE FROM " + tC_TblSalDTDis + " WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");

                //*Arm 63-09-14 HD
                oSql.AppendLine();
                oSql.AppendLine("UPDATE HD SET");
                oSql.AppendLine("HD.FTXshRmk = HoldHD.FTXshRmk");
                oSql.AppendLine("FROM " + tC_TblSalHD + " HD WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPSTHoldHD HoldHD WITH(NOLOCK) ON HoldHD.FNHldNo = " + cSale.nC_HoldNo);
                oSql.AppendLine("WHERE HD.FTBchCode = '"+ cVB.tVB_BchCode +"'");
                oSql.AppendLine("AND HD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //+++++++++++++

                //HDCst
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO " + tC_TblSalHDCst);
                oSql.AppendLine("(FTBchCode, FTXshDocNo, FTXshCardID, FTXshCardNo, FNXshCrTerm, FDXshDueDate, FDXshBillDue, FTXshCtrName, FDXshTnfDate, FTXshRefTnfID, FNXshAddrShip, FNXshAddrTax, FTXshCstName, FTXshCstTel)");
                oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTXshCardID, FTXshCardNo, FNXshCrTerm, FDXshDueDate, FDXshBillDue, FTXshCtrName, FDXshTnfDate, FTXshRefTnfID, FNXshAddrShip, FNXshAddrTax, FTXshCstName, FTXshCstTel");
                oSql.AppendLine("FROM TPSTHoldHDCst WITH(NOLOCK)");
                oSql.AppendLine("WHERE FNHldNo = " + cSale.nC_HoldNo);

                //DT
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO " + tC_TblSalDT);
                oSql.AppendLine("(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, FTXsdSaleType,");
                oSql.AppendLine("FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate,");
                oSql.AppendLine("FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy, FTPplCode)");
                oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo,ROW_NUMBER() OVER(ORDER BY FNXsdSeqNo) FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, FTXsdSaleType,");
                oSql.AppendLine("FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate,");
                oSql.AppendLine("FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                oSql.AppendLine("GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy, FTPplCode");
                oSql.AppendLine("FROM TPSTHoldDT WITH(NOLOCK)");
                oSql.AppendLine("WHERE FNHldNo = " + cSale.nC_HoldNo);

                //DTDis
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO " + tC_TblSalDTDis);
                oSql.AppendLine("(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue, FTXddRefCode, FTDisCode, FTRsnCode)"); //*Arm 63-09-16 เพิ่ม FTXddRefCode, FTDisCode, FTRsnCode
                //oSql.AppendLine("SELECT FTBchCode, FTXihDocNo, FNXidSeqNo, FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                oSql.AppendLine("SELECT FTBchCode, FTXihDocNo, DT.FNSeqNew AS FNXidSeqNo, GETDATE() AS FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue, FTXddRefCode, FTDisCode, FTRsnCode");   //*Em 63-06-08, *Arm 63-09-16 เพิ่ม FTXddRefCode, FTDisCode, FTRsnCode
                oSql.AppendLine("FROM TPSTHoldDTDis DTDis WITH(NOLOCK)");
                //*Em 63-06-08
                oSql.AppendLine("INNER JOIN (SELECT ROW_NUMBER() OVER(ORDER BY FNXsdSeqNo) AS FNSeqNew,FNXsdSeqNo ");
                oSql.AppendLine("       FROM TPSTHoldDT WITH(NOLOCK)");
                oSql.AppendLine("       WHERE FNHldNo = " + cSale.nC_HoldNo + ") DT ON DT.FNXsdSeqNo = DTDis.FNXidSeqNo ");
                //++++++++++++++
                oSql.AppendLine("WHERE FNHldNo = " + cSale.nC_HoldNo);

                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxInsertRetriveBill : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// *Arm 63-04-28
        /// เช็คข้อมูลพักบิล
        /// </summary>
        /// <returns></returns>
        public static bool C_PRCbCheckHoldBill()
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nCount = 0;
            bool bStaChk = false;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                oSql.AppendLine("SELECT COUNT(*) FROM TPSTHoldHD with(nolock) WHERE FTBchCode ='" + cVB.tVB_BchCode + "' AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                nCount = oDB.C_GEToDataQuery<int>(oSql.ToString());

                if (nCount > 0)
                {
                    if (new cSP().SP_SHWoMsg(cVB.oVB_GBResource.GetString("tMsgChkHoldBill"), 1) == DialogResult.Yes)
                    {
                        oSql.Clear();
                        oSql.AppendLine("DELETE HDCst");
                        oSql.AppendLine("FROM TPSTHoldHDCst HDCst with(rowlock) ");
                        oSql.AppendLine("INNER JOIN TPSTHoldHD HD with(nolock) ON HDCst.FTBchCode = HD.FTBchCode AND HDCst.FTXshDocNo = HD.FTXshDocNo ");
                        oSql.AppendLine("WHERE HD.FTBchCode ='" + cVB.tVB_BchCode + "' AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' ");
                        oSql.AppendLine();
                        oSql.AppendLine("DELETE DTDis ");
                        oSql.AppendLine("FROM TPSTHoldDTDis DTDis with(rowlock)");
                        oSql.AppendLine("INNER JOIN TPSTHoldDT DT with(nolock) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXihDocNo = DT.FTXshDocNo AND DTDis.FNXidSeqNo = DT.FNXsdSeqNo");
                        oSql.AppendLine("INNER JOIN TPSTHoldHD HD with(nolock) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo");
                        oSql.AppendLine("WHERE HD.FTBchCode ='" + cVB.tVB_BchCode + "' AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' ");
                        oSql.AppendLine();
                        oSql.AppendLine("DELETE DT ");
                        oSql.AppendLine("FROM TPSTHoldDT DT with(rowlock) ");
                        oSql.AppendLine("INNER JOIN TPSTHoldHD HD with(nolock) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo ");
                        oSql.AppendLine("WHERE HD.FTBchCode ='" + cVB.tVB_BchCode + "' AND HD.FTPosCode = '" + cVB.tVB_PosCode + "' ");
                        oSql.AppendLine();
                        oSql.AppendLine("DELETE FROM TPSTHoldHD with(rowlock) WHERE FTBchCode ='" + cVB.tVB_BchCode + "' AND FTPosCode = '" + cVB.tVB_PosCode + "'");
                        oSql.AppendLine();
                        oDB.C_SETxDataQuery(oSql.ToString());

                        bStaChk = true;
                    }
                    else
                    {
                        bStaChk = false;
                    }
                }
                else
                {
                    bStaChk = true;
                }
            }
            catch (Exception oEx)
            {
                bStaChk = false;
                new cLog().C_WRTxLog("cSale", "C_PRCbCheckHoldBill() : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return bStaChk;

        }

        /// <summary>
        /// *Arm 63-05-11
        /// </summary>
        /// <param name="paoSoDT"></param>
        /// <returns></returns>
        public List<cmlTARTSoDTTmp> C_PRCaListSoDT(List<cmlResInfoTARTSoDT> paoSoDT)
        {
            List<cmlTARTSoDTTmp> aoData = new List<cmlTARTSoDTTmp>();

            try
            {
                aoData = paoSoDT.Select(oItem => new cmlTARTSoDTTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FNXsdSeqNo = oItem.rnXsdSeqNo,
                    FTPdtCode = oItem.rtPdtCode,
                    FTXsdPdtName = oItem.rtXsdPdtName,
                    FTPunCode = oItem.rtPunCode,
                    FTPunName = oItem.rtPunName,
                    FCXsdFactor = oItem.rcXsdFactor,
                    FTXsdBarCode = oItem.rtXsdBarCode,
                    FTSrnCode = oItem.rtSrnCode,
                    FTXsdVatType = oItem.rtXsdVatType,
                    FTVatCode = oItem.rtVatCode,
                    FCXsdVatRate = oItem.rcXsdVatRate,
                    FTXsdSaleType = oItem.rtXsdSaleType,
                    FCXsdSalePrice = oItem.rcXsdSalePrice,
                    FCXsdQty = oItem.rcXsdQty,
                    FCXsdQtyAll = oItem.rcXsdQtyAll,
                    FCXsdSetPrice = oItem.rcXsdSetPrice,
                    FCXsdAmtB4DisChg = oItem.rcXsdAmtB4DisChg,
                    FTXsdDisChgTxt = oItem.rtXsdDisChgTxt,
                    FCXsdDis = oItem.rcXsdDis,
                    FCXsdChg = oItem.rcXsdChg,
                    FCXsdNet = oItem.rcXsdNet,
                    FCXsdNetAfHD = oItem.rcXsdNetAfHD,
                    FCXsdVat = oItem.rcXsdVat,
                    FCXsdVatable = oItem.rcXsdVatable,
                    FCXsdWhtAmt = oItem.rcXsdWhtAmt,
                    FTXsdWhtCode = oItem.rtXsdWhtCode,
                    FCXsdWhtRate = oItem.rcXsdWhtRate,
                    FCXsdCostIn = oItem.rcXsdCostIn,
                    FCXsdCostEx = oItem.rcXsdCostEx,
                    FTXsdStaPdt = oItem.rtXsdStaPdt,
                    FCXsdQtyLef = oItem.rcXsdQtyLef,
                    FCXsdQtyRfn = oItem.rcXsdQtyRfn,
                    FTXsdStaPrcStk = oItem.rtXsdStaPrcStk,
                    FTXsdStaAlwDis = oItem.rtXsdStaAlwDis,
                    FNXsdPdtLevel = oItem.rnXsdPdtLevel,
                    FTXsdPdtParent = oItem.rtXsdPdtParent,
                    FCXsdQtySet = oItem.rcXsdQtySet,
                    FTPdtStaSet = oItem.rtPdtStaSet,
                    FTXsdRmk = oItem.rtXsdRmk,
                    FDLastUpdOn = oItem.rdLastUpdOn,
                    FTLastUpdBy = oItem.rtLastUpdBy,
                    FDCreateOn = oItem.rdCreateOn,
                    FTCreateBy = oItem.rtCreateBy

                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRCaListSoDT : " + oEx.Message); }
            finally
            {
                paoSoDT = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoData;
        }
        /// <summary>
        /// *Arm 63-05-11
        /// </summary>
        /// <param name="paoSoDTDis"></param>
        /// <returns></returns>
        public List<cmlTARTSoDTDisTmp> C_PRCaListSoDTDis(List<cmlResInfoTARTSoDTDis> paoSoDTDis)
        {
            List<cmlTARTSoDTDisTmp> aoData = new List<cmlTARTSoDTDisTmp>();

            try
            {
                aoData = paoSoDTDis.Select(oItem => new cmlTARTSoDTDisTmp()
                {
                    FTBchCode = oItem.rtBchCode,
                    FTXshDocNo = oItem.rtXshDocNo,
                    FNXsdSeqNo = oItem.rnXsdSeqNo,
                    FDXddDateIns = oItem.rdXddDateIns,
                    FNXddStaDis = oItem.rnXddStaDis,
                    FTXddDisChgTxt = oItem.rtXddDisChgTxt,
                    FTXddDisChgType = oItem.rtXddDisChgType,
                    FCXddNet = oItem.rcXddNet,
                    FCXddValue = oItem.rcXddValue
                }).ToList();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRCaListSoDTDis: " + oEx.Message); }
            finally
            {
                paoSoDTDis = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoData;
        }

        /// <summary>
        /// *Arm 63-05-11
        /// </summary>
        /// <returns></returns>
        public bool C_PRCbInsSO(string ptStaAlwPosCalSo = "")
        {
            StringBuilder oSql;
            cDatabase oDB;
            SqlTransaction oTranscation;
            List<cmlTARTSoDTTmp> aoSoDT;
            List<cmlTARTSoDTDisTmp> aoSoDTDis;
            cDataReaderAdapter<cmlTARTSoDTTmp> oSoDT;
            cDataReaderAdapter<cmlTARTSoDTDisTmp> oSoDTDis;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();

                new cLog().C_WRTxLog("wSale", "Created & Insert to Temp : Start... ", cVB.bVB_AlwPrnLog);
                //Created Tmp
                oDB.C_PRCxCreateDatabaseTmp("TARTSoDT", "TARTSoDTTmp");
                oDB.C_PRCxCreateDatabaseTmp("TARTSoDTDis", "TARTSoDTDisTmp");

                oTranscation = cVB.oVB_ConnDB.BeginTransaction();

                // Bulk Copy : TARTSoDT

                aoSoDT = C_PRCaListSoDT(cVB.oVB_ReferSO.aoTARTSoDT);
                oSoDT = new cDataReaderAdapter<cmlTARTSoDTTmp>(aoSoDT);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oSoDT.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TARTSoDTTmp";
                    try
                    {
                        oBulkCopy.WriteToServer(oSoDT);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wSale", "W_DATxLoadSO2Order/TARTSoDTTmp : " + oEx.Message);
                        return false;
                    }
                }
                // Bulk Copy : TARTSoDTDis
                aoSoDTDis = C_PRCaListSoDTDis(cVB.oVB_ReferSO.aoTARTSoDTDis);
                oSoDTDis = new cDataReaderAdapter<cmlTARTSoDTDisTmp>(aoSoDTDis);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTranscation))
                {
                    foreach (string tColName in oSoDTDis.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }

                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TARTSoDTDisTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oSoDTDis);
                    }
                    catch (Exception oEx)
                    {
                        oTranscation.Rollback();
                        new cLog().C_WRTxLog("wSale", "W_DATxLoadSO2Order/TARTSoDTDisTmp : " + oEx.Message);
                        return false;
                    }
                }

                oTranscation.Commit();
                new cLog().C_WRTxLog("wSale", "Created & Insert to Temp : End... ", cVB.bVB_AlwPrnLog);

                new cLog().C_WRTxLog("wSale", "Insert to Sale Temp : Start... ", cVB.bVB_AlwPrnLog);

                //if (ptStaAlwPosCalSo == "1")
                //{
                //    oSql.Clear();
                //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    oSql.AppendLine("   FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");

                //    //*Arm 63-05-13
                //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, DTTmp.FNXsdSeqNo, DTTmp.FTPdtCode, DTTmp.FTXsdPdtName, ");
                //    oSql.AppendLine("    DTTmp.FTPunCode, DTTmp.FTPunName, DTTmp.FCXsdFactor,DTTmp.FTXsdBarCode, DTTmp.FTSrnCode, ");
                //    oSql.AppendLine("    DTTmp.FTXsdVatType, DTTmp.FTVatCode, PRI.FTPplCode AS FTPplCode, DTTmp.FCXsdVatRate, DTTmp.FTXsdSaleType, ");
                //    oSql.AppendLine("    ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0)) AS FCXsdSalePrice,");
                //    oSql.AppendLine("    DTTmp.FCXsdQty, (FCXsdFactor * DTTmp.FCXsdQty) AS FCXsdQtyAll,");
                //    oSql.AppendLine("    ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0)) AS FCXsdSetPrice,");
                //    oSql.AppendLine("    (DTTmp.FCXsdQty * ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0))) AS FCXsdAmtB4DisChg,");
                //    oSql.AppendLine("    '' AS FTXsdDisChgTxt,0 AS FCXsdDis, 0 AS FCXsdChg,");
                //    oSql.AppendLine("    (DTTmp.FCXsdQty * ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0))) ASFCXsdNet, ");
                //    oSql.AppendLine("    (DTTmp.FCXsdQty * ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0))) AS FCXsdNetAfHD,");
                //    oSql.AppendLine("    0 AS FCXsdVat, 0 AS FCXsdVatable, DTTmp.FCXsdWhtAmt, DTTmp.FTXsdWhtCode, DTTmp.FCXsdWhtRate, ");
                //    oSql.AppendLine("    0 AS FCXsdCostIn, 0 AS FCXsdCostEx, '1' AS FTXsdStaPdt, DTTmp.FCXsdQtyLef, DTTmp.FCXsdQtyRfn, ");
                //    oSql.AppendLine("    DTTmp.FTXsdStaPrcStk, DTTmp.FTXsdStaAlwDis, DTTmp.FNXsdPdtLevel, DTTmp.FTXsdPdtParent, DTTmp.FCXsdQtySet, ");
                //    oSql.AppendLine("    DTTmp.FTPdtStaSet, DTTmp.FTXsdRmk, ");
                //    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //    oSql.AppendLine("FROM TARTSoDTTmp DTTmp WITH(NOLOCK)");
                //    oSql.AppendLine("LEFT JOIN  TPSTPdtPrice PRI WITH(NOLOCK) ON DTTmp.FTPdtCode = PRI.FTPdtCode AND PRI.FTPriType = '1' ");
                //    oSql.AppendLine("AND DTTmp.FTPunCode = PRI.FTPunCode AND(ISNULL(PRI.FTPplCode, '') = '" + cVB.tVB_PriceGroup + "')");
                //    oSql.AppendLine("WHERE DTTmp.FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    oSql.AppendLine("AND DTTmp.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    oSql.AppendLine();

                //}
                //else
                //{
                //    oSql.Clear();
                //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    oSql.AppendLine("   FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                //    oSql.AppendLine("SELECT FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, '" + cVB.tVB_PriceGroup + "' AS FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, ABS(FCXsdDis) AS FCXsdDis, ABS(FCXsdChg) AS FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, '1' AS FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //    oSql.AppendLine("FROM TARTSoDTTmp WITH(NOLOCK)");
                //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    oSql.AppendLine();
                //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                //    oSql.AppendLine("SELECT FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, '' AS FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                //    oSql.AppendLine("FROM TARTSoDTDisTmp WITH(NOLOCK)");
                //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //}

                //*Arm 63-06-24 ยกมาจาก Moshi
                if (ptStaAlwPosCalSo == "1")
                {
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaAlwDis, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("   FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    
                    //*Arm 63-06-18 - แก้ไขเรื่องการดึงราคา
                    oSql.AppendLine("SELECT DISTINCT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, DTTmp.FNXsdSeqNo, DTTmp.FTPdtCode, DTTmp.FTXsdPdtName, ");
                    oSql.AppendLine("   DTTmp.FTPunCode, DTTmp.FTPunName, DTTmp.FCXsdFactor,DTTmp.FTXsdBarCode, DTTmp.FTSrnCode, ");
                    oSql.AppendLine("   PDT.FTPdtStaVat AS FTXsdVatType, '" + cVB.tVB_VatCode + "' AS FTVatCode, ISNULL(PRI.FTPplCode,'') AS FTPplCode, " + cVB.cVB_VatRate + " AS FCXsdVatRate, PDT.FTPdtSaleType AS FTXsdSaleType, ");
                    oSql.AppendLine("   ISNULL(PRI.FCPdtPrice, ISNULL(PDT.FCPdtPrice, 0)) AS FCXsdSalePrice,");
                    oSql.AppendLine("   DTTmp.FCXsdQty, DTTmp.FCXsdQtyAll,");
                    oSql.AppendLine("   ISNULL(PRI.FCPdtPrice, ISNULL(PDT.FCPdtPrice, 0)) AS FCXsdSetPrice,");
                    oSql.AppendLine("   (ISNULL(DTTmp.FCXsdQty,0) * ISNULL(PRI.FCPdtPrice, ISNULL(PDT.FCPdtPrice, 0))) AS FCXsdAmtB4DisChg,");
                    oSql.AppendLine("   '' AS FTXsdDisChgTxt,0 AS FCXsdDis, 0 AS FCXsdChg,");
                    oSql.AppendLine("   (ISNULL(DTTmp.FCXsdQty,0) * ISNULL(PRI.FCPdtPrice, ISNULL(PDT.FCPdtPrice, 0))) AS FCXsdNet,");
                    oSql.AppendLine("   (ISNULL(DTTmp.FCXsdQty,0) * ISNULL(PRI.FCPdtPrice, ISNULL(PDT.FCPdtPrice, 0))) AS FCXsdNetAfHD,");
                    oSql.AppendLine("   0 AS FCXsdVat, 0 AS FCXsdVatable, DTTmp.FCXsdWhtAmt, DTTmp.FTXsdWhtCode, DTTmp.FCXsdWhtRate,");
                    oSql.AppendLine("   ISNULL(DTTmp.FCXsdCostIn,ISNULL(PCA.FCPdtCostIn, 0)) AS FCXsdCostIn, ISNULL(DTTmp.FCXsdCostEx,ISNULL(PCA.FCPdtCostEx, 0)) AS FCXsdCostEx, '1' AS FTXsdStaPdt, ISNULL(DTTmp.FCXsdQty,0) AS FCXsdQtyLef, 0 AS FCXsdQtyRfn, ");
                    //oSql.AppendLine("   DTTmp.FTXsdStaPrcStk, PDT.FTPdtStaAlwDis AS FTXsdStaAlwDis, DTTmp.FNXsdPdtLevel, DTTmp.FTXsdPdtParent, DTTmp.FCXsdQtySet, ");

                    oSql.AppendLine("   ISNULL(PDT.FTPdtStaAlwDis,'') AS FTXsdStaAlwDis, '1' AS FTPdtStaSet, DTTmp.FTXsdRmk, ");
                    oSql.AppendLine("   GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM TARTSoDTTmp DTTmp WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TPSMPdt PDT ON DTTmp.FTPdtCode = PDT.FTPdtCode AND DTTmp.FTXsdBarCode = PDT.FTBarCode ");
                    oSql.AppendLine("LEFT JOIN  TPSTPdtPrice PRI WITH(NOLOCK) ON DTTmp.FTPdtCode = PRI.FTPdtCode AND PRI.FTPriType = '1'");
                    oSql.AppendLine("AND DTTmp.FTPunCode = PRI.FTPunCode AND(ISNULL(PRI.FTPplCode, '') = '" + cVB.tVB_PriceGroup + "')");
                    oSql.AppendLine("LEFT JOIN TCNMPdtCostAvg PCA WITH(NOLOCK) ON PCA.FTPdtCode = DTTmp.FTPdtCode");
                    oSql.AppendLine("WHERE DTTmp.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DTTmp.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                }
                else
                {
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("   FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, '" + cVB.tVB_PriceGroup + "', FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, ABS(FCXsdDis) AS FCXsdDis, ABS(FCXsdChg) AS FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, '1' AS FTXsdStaPdt, ISNULL(FCXsdQty,0) AS FCXsdQtyLef, ISNULL(FCXsdQtyRfn,0) AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM TARTSoDTTmp WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                    oSql.AppendLine("SELECT FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, '' AS FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                    oSql.AppendLine("FROM TARTSoDTDisTmp WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                }
                //+++++++++++++++
                oDB.C_SETxDataQuery(oSql.ToString());
                
                new cLog().C_WRTxLog("wSale", "Insert to Sale Temp : End... ", cVB.bVB_AlwPrnLog);

                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCbInsSO : " + oEx.Message);
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                aoSoDT = null;
                aoSoDTDis = null;
                oSoDT = null;
                oSoDTDis = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// Insert รายการขายอ้างอิงบิลคืน (*Arm 63-06-10)
        /// </summary>
        /// <returns></returns>
        public static bool C_PRCbInsReferBill()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                if (cVB.bVB_RefundFullBill) // bVB_RefundFullBill = true : อ้างอิงเต็มบิล , false : อ้างอิงบางรายการ
                {
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, ROW_NUMBER() OVER(ORDER BY FNXsdSeqNo ASC) AS FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, "); //*Arm 63-09-09 แก้ไขจัด Seq ใหม่
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * FCXsdQty) AS FCXsdAmtB4DisChg, '' AS FTXsdDisChgTxt, 0 AS FCXsdDis, 0 AS FCXsdChg, (FCXsdSetPrice * FCXsdQty) AS FCXsdNet, (FCXsdSetPrice * FCXsdQty) AS FCXsdNetAfHD, 0 AS FCXsdVat, 0 AS FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    0 AS FCXsdCostIn, 0 AS FCXsdCostEx, '1' AS FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, "); //*Arm 63-08-17 set StaPdt = 1
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "' "); //*Arm 63-09-11
                    oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    oDB.C_SETxDataQuery(oSql.ToString());

                }
                else
                {
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, ROW_NUMBER() OVER(ORDER BY RF.FNXsdSeqNo ASC) AS FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, "); //*Arm 63-09-09 แก้ไขจัด Seq ใหม่
                    oSql.AppendLine("    FCXsdSalePrice, DT.FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * DT.FCXsdQty) AS FCXsdAmtB4DisChg, '' AS FTXsdDisChgTxt, 0 AS FCXsdDis, 0 AS FCXsdChg, (FCXsdSetPrice * DT.FCXsdQty) AS FCXsdNet, (FCXsdSetPrice * DT.FCXsdQty) AS FCXsdNetAfHD, 0 AS FCXsdVat, 0 AS FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    0 AS FCXsdCostIn, 0 AS FCXsdCostEx, '1' AS FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, "); //*Arm 63-08-17 set StaPdt = 1
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " DT WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "' "); //*Arm 63-09-11
                    oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    oSql.AppendLine();
                    oDB.C_SETxDataQuery(oSql.ToString());
                }
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCbInsReferBill : " + oEx.Message);
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        

        public static bool C_PRCbInsRefund()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                //*Arm 63-06-10 Start
                if (cVB.bVB_RefundFullBill)     
                {
                    //*cVB.bVB_RefundFullBill = true คืนทั้งบิล

                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, '2' AS FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, "); //*Net 63-06-17 บิลคืน stapdt=2
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11
                    oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue, FTDisCode, FTRsnCode)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue, FTDisCode, FTRsnCode");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDTDis + " WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalPD + " WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalRD + " WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11
                    oDB.C_SETxDataQuery(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                    oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt , FTXhdRefCode ");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalHDDis + " WITH(NOLOCK) ");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'  ");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "' ");   //*Arm 63-09-11
                    oSql.AppendLine("AND (ISNULL(FTXhdRefCode,'') ='' OR FTXhdDisChgType = '5'  OR FTXhdDisChgType = '6')");
                    oSql.AppendLine("ORDER BY FDXhdDateIns ASC");
                    cVB.aoVB_PdtHDDisRefund = new cDatabase().C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());
                }
                else
                {
                    //*cVB.bVB_RefundFullBill = false คืนบางรายการ

                    //*Arm 63-09-17 Comment Code
                    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    //oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    //oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    ////oSql.AppendLine("    FCXsdSalePrice, RF.FCXsdQtyRfn AS FCXsdQty, (RF.FCXsdQtyRfn * FCXsdFactor) AS FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdNet, (ISNULL(DT.FCXsdNetAfHD,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //oSql.AppendLine("    FCXsdSalePrice, RF.FCXsdQtyRfn AS FCXsdQty, (RF.FCXsdQtyRfn * FCXsdFactor) AS FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, (ISNULL(DT.FCXsdNet,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXsdNet, (ISNULL(DT.FCXsdNetAfHD,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    ////oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    //oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, '2' AS FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, "); //*Net 63-06-17 การคืน stapdt=2
                    //oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    //oSql.AppendLine("FROM " + tC_Ref_TblSalDT + " DT WITH(NOLOCK)");
                    //oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    ////oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    ////oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11
                    //oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    //oSql.AppendLine();


                    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue, FTDisCode, FTRsnCode)"); //*Arm 63-09-17 เพิ่ม FTDisCode, FTRsnCode
                    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, (ISNULL(DT.FCXddNet,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddNet, (ISNULL(DT.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddValue, FTDisCode, FTRsnCode"); //*Arm 63-09-17 เพิ่ม FTDisCode, FTRsnCode
                    //oSql.AppendLine("FROM " + tC_Ref_TblSalDTDis + " DT WITH(NOLOCK)");
                    //oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    ////oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    ////oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11

                    //*Arm 63-09-17
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	 FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, DT.FTPdtCode, DT.FTXsdPdtName, ");
                    oSql.AppendLine("   DT.FTPunCode, DT.FTPunName, DT.FCXsdFactor, DT.FTXsdBarCode, DT.FTSrnCode, ");
                    oSql.AppendLine("   DT.FTXsdVatType, DT.FTVatCode, DT.FTPplCode, DT.FCXsdVatRate, DT.FTXsdSaleType, ");
                    oSql.AppendLine("   DT.FCXsdSalePrice, RF.FCXsdQtyRfn AS FCXsdQty, (RF.FCXsdQtyRfn * FCXsdFactor) AS FCXsdQtyAll, DT.FCXsdSetPrice, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdAmtB4DisChg,");
                    oSql.AppendLine("   DT.FTXsdDisChgTxt, CASE WHEN FCXsdDis = 0 THEN 0 ELSE(DT.FCXsdDis / DT.FCXsdQtyAll) * RF.FCXsdQtyRfn END AS FCXsdDis,");
                    oSql.AppendLine("   DT.FCXsdChg, (ISNULL(DT.FCXsdNet, 0.00) / DT.FCXsdQtyAll) * RF.FCXsdQtyRfn AS FCXsdNet, (ISNULL(DT.FCXsdNetAfHD, 0.00) / DT.FCXsdQtyAll) *RF.FCXsdQtyRfn AS FCXsdNetAfHD,");
                    oSql.AppendLine("   DT.FCXsdVat, DT.FCXsdVatable, DT.FCXsdWhtAmt, DT.FTXsdWhtCode, DT.FCXsdWhtRate, ");
                    oSql.AppendLine("   DT.FCXsdCostIn, DT.FCXsdCostEx, '2' AS FTXsdStaPdt, RF.FCXsdQtyRfn AS FCXsdQtyLef, 0 AS FCXsdQtyRfn,");
                    oSql.AppendLine("   DT.FTXsdStaPrcStk, DT.FTXsdStaAlwDis, DT.FNXsdPdtLevel, DT.FTXsdPdtParent, DT.FCXsdQtySet, ");
                    oSql.AppendLine("   DT.FTPdtStaSet, DT.FTXsdRmk,");
                    oSql.AppendLine("   GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM " + tC_Ref_TblSalDT + " DT WITH(NOLOCK) ");
                    oSql.AppendLine("INNER JOIN " + tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo ");
                    oSql.AppendLine("WHERE DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' ");
                    oSql.AppendLine("AND DT.FTXsdStaPdt <> '4' ");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue, FTDisCode, FTRsnCode)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, DTDis.FDXddDateIns, DTDis.FTXddRefCode, DTDis.FNXddStaDis, ");
                    oSql.AppendLine("CAST((ISNULL(DTDis.FCXddValue, 0.00) / DT.FCXsdQtyAll) * RF.FCXsdQtyRfn AS decimal(18, 2)) AS FTXddDisChgTxt, DTDis.FTXddDisChgType, ");
                    oSql.AppendLine("(ISNULL(DTDis.FCXddNet, 0.00) / DT.FCXsdQtyAll) * RF.FCXsdQtyRfn AS FCXddNet, (ISNULL(DTDis.FCXddValue, 0.00) / DT.FCXsdQtyAll) *RF.FCXsdQtyRfn AS FCXddValue, ");
                    oSql.AppendLine("DTDis.FTDisCode, DTDis.FTRsnCode");
                    oSql.AppendLine("FROM  " + tC_Ref_TblSalDTDis + " DTDis WITH(NOLOCK) ");
                    oSql.AppendLine("INNER JOIN " + tC_Ref_TblSalDT + " DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                    oSql.AppendLine("INNER JOIN " + tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    oSql.AppendLine("WHERE DTDis.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //+++++++++++++

                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, PD.FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                    oSql.AppendLine("FROM " + tC_Ref_TblSalPD + " PD WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = PD.FNXsdSeqNo");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                    oSql.AppendLine("FROM " + tC_Ref_TblSalRD + " RD WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = RD.FNXrdRefSeq");
                    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11
                    oDB.C_SETxDataQuery(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("SELECT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice,");
                    oSql.AppendLine("DTDis.FNXddStaDis AS pnStaDis, DTDis.FTXddDisChgType AS ptDisChgType, DTDis.FCXddNet AS pcNet,  ");
                    //oSql.AppendLine("(ISNULL(DTDis.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS pcValue, DTDis.FTXddRefCode AS ptRefCode");
                    oSql.AppendLine("(ISNULL(DTDis.FCXddValue,0.00)/DT.FCXsdQtyAll) * RF.FCXsdQtyRfn AS pcValue, DTDis.FTXddRefCode AS ptRefCode"); //*Arm 63-09-17
                    oSql.AppendLine("FROM " + tC_Ref_TblSalDTDis + " DTDis WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + tC_Ref_TblSalDT + " DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    //oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("WHERE DTDis.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-09-11
                    oSql.AppendLine("AND DTDis.FNXddStaDis = 2 ");
                    oSql.AppendLine("ORDER BY DTDis.FDXddDateIns ASC");
                    cVB.aoVB_PdtDisChgRefund = new cDatabase().C_GETaDataQuery<cmlPdtDisChg>(oSql.ToString());
                }
                //*Arm 63-06-10 End 
                
                //if (cVB.bVB_RefundFullBill)
                //{
                //    if (cVB.bVB_RefundTrans)
                //    {
                //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //        oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //        oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //        oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //        oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //        oSql.AppendLine("FROM TPSTSalDT WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                //        oSql.AppendLine();
                //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                //        oSql.AppendLine("FROM TPSTSalDTDis WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        oSql.AppendLine();
                //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                //        oSql.AppendLine("FROM TPSTSalPD WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        oSql.AppendLine();
                //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                //        oSql.AppendLine("FROM TPSTSalRD WITH(NOLOCK)");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        oDB.C_SETxDataQuery(oSql.ToString());

                    //        oSql.Clear();
                    //        oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                    //        oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt , FTXhdRefCode "); //*Arm 63-04-16
                    //        oSql.AppendLine("FROM TPSTSalHDDis WITH(NOLOCK) ");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'  ");
                    //        oSql.AppendLine("AND (ISNULL(FTXhdRefCode,'') ='' OR FTXhdDisChgType = '5'  OR FTXhdDisChgType = '6')");
                    //        oSql.AppendLine("ORDER BY FDXhdDateIns ASC");
                    //        cVB.aoVB_PdtHDDisRefund = new cDatabase().C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());
                    //    }
                    //    else
                    //    {
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    //        oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    //        oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    //        oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    //        oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    //        oSql.AppendLine("FROM " + tC_TblSalDT + " WITH(NOLOCK)");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                    //        oSql.AppendLine("FROM " + tC_TblSalDTDis + " WITH(NOLOCK)");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                    //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                    //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                    //        oSql.AppendLine("FROM " + tC_TblSalPD + " WITH(NOLOCK)");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                    //        oSql.AppendLine("FROM " + tC_TblSalRD + " WITH(NOLOCK)");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oDB.C_SETxDataQuery(oSql.ToString());

                    //        oSql.Clear();
                    //        oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                    //        oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt , FTXhdRefCode "); //*Arm 63-04-16
                    //        oSql.AppendLine("FROM " + tC_TblSalHDDis + " WITH(NOLOCK) ");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'  ");
                    //        oSql.AppendLine("AND (ISNULL(FTXhdRefCode,'') ='' OR FTXhdDisChgType = '5'  OR FTXhdDisChgType = '6')");
                    //        oSql.AppendLine("ORDER BY FDXhdDateIns ASC");
                    //        cVB.aoVB_PdtHDDisRefund = new cDatabase().C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());
                    //    }
                    //}
                    //else
                    //{
                    //    if (cVB.bVB_RefundTrans)
                    //    {
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    //        oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    //        oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    //        oSql.AppendLine("    FCXsdSalePrice, RF.FCXsdQtyRfn AS FCXsdQty, (RF.FCXsdQtyRfn * FCXsdFactor) AS FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdNet, (ISNULL(DT.FCXsdNetAfHD,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    //        oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    //        oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        //oSql.AppendLine("AND FNXsdSeqNo IN ("+ string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    //        oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, (ISNULL(DT.FCXddNet,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddNet, (ISNULL(DT.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddValue");
                    //        oSql.AppendLine("FROM TPSTSalDTDis DT WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        //oSql.AppendLine("AND FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                    //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, PD.FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                    //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                    //        oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = PD.FNXsdSeqNo");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        //oSql.AppendLine("AND FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                    //        oSql.AppendLine("FROM TPSTSalRD RD WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = RD.FNXrdRefSeq");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        //oSql.AppendLine("AND FNXrdRefSeq IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    //        oDB.C_SETxDataQuery(oSql.ToString());

                    //        oSql.Clear();
                    //        oSql.AppendLine("SELECT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice,");
                    //        oSql.AppendLine("DTDis.FNXddStaDis AS pnStaDis, DTDis.FTXddDisChgType AS ptDisChgType, DTDis.FCXddNet AS pcNet,  ");
                    //        oSql.AppendLine("(ISNULL(DTDis.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS pcValue, DTDis.FTXddRefCode AS ptRefCode");
                    //        oSql.AppendLine("FROM TPSTSalDTDis DTDis WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    //        oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oSql.AppendLine("AND DTDis.FNXddStaDis = 2 ");
                    //        //oSql.AppendLine("AND DT.FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    //        oSql.AppendLine("ORDER BY DTDis.FDXddDateIns ASC");
                    //        cVB.aoVB_PdtDisChgRefund = new cDatabase().C_GETaDataQuery<cmlPdtDisChg>(oSql.ToString());
                    //    }
                    //    else
                    //    {
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    //        oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    //        oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    //        oSql.AppendLine("    FCXsdSalePrice, RF.FCXsdQtyRfn AS FCXsdQty, (RF.FCXsdQtyRfn * FCXsdFactor) AS FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdNet, (ISNULL(DT.FCXsdNetAfHD,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    //        oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    //        oSql.AppendLine("FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, (ISNULL(DT.FCXddNet,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddNet, (ISNULL(DT.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddValue");
                    //        oSql.AppendLine("FROM " + tC_TblSalDTDis + " DT WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                    //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, PD.FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                    //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                    //        oSql.AppendLine("FROM " + tC_TblSalPD + " PD WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = PD.FNXsdSeqNo");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oSql.AppendLine();
                    //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                    //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                    //        oSql.AppendLine("FROM " + tC_TblSalRD + " RD WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = RD.FNXrdRefSeq");
                    //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oDB.C_SETxDataQuery(oSql.ToString());

                    //        oSql.Clear();
                    //        oSql.AppendLine("SELECT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice,");
                    //        oSql.AppendLine("DTDis.FNXddStaDis AS pnStaDis, DTDis.FTXddDisChgType AS ptDisChgType, DTDis.FCXddNet AS pcNet,  ");
                    //        oSql.AppendLine("(ISNULL(DTDis.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS pcValue, DTDis.FTXddRefCode AS ptRefCode");
                    //        oSql.AppendLine("FROM " + tC_TblSalDTDis + " DTDis WITH(NOLOCK)");
                    //        oSql.AppendLine("INNER JOIN " + tC_TblSalDT + " DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                    //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    //        oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //        oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //        oSql.AppendLine("AND DTDis.FNXddStaDis = 2 ");
                    //        oSql.AppendLine("ORDER BY DTDis.FDXddDateIns ASC");
                    //        cVB.aoVB_PdtDisChgRefund = new cDatabase().C_GETaDataQuery<cmlPdtDisChg>(oSql.ToString());
                    //    }

                    //}
                    return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCbInsRefund : " + oEx.Message);
                return false;
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static string C_GETxSalDocNoFrmRefund(string ptRefundDocNo)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            string tDocNo = "";
            try
            {
                oSql.Clear();
                oSql.AppendLine($"SELECT TOP 1 FTXshRefInt FROM {cSale.tC_TblSalHD} WHERE FTXshDocNo='{ptRefundDocNo}'");
                tDocNo = oDB.C_GEToDataQuery<string>(oSql.ToString());
                if (String.IsNullOrEmpty(tDocNo))
                {
                    oSql.Clear();
                    oSql.AppendLine($"SELECT TOP 1 FTXshRefInt FROM TPSTSalHD WHERE FTXshDocNo='{ptRefundDocNo}'");
                    tDocNo = oDB.C_GEToDataQuery<string>(oSql.ToString());
                }
                return tDocNo;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_GETxDocNoFrmRefDoc : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return "";
        }
        public static void C_GETxLastDocNo()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                C_GETtFormatDoc("TPSTSalHD", 1, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);
                tC_DocFmtLeftSal = tC_DocFmtLeft;
                nC_DocSalLength = nC_DocRuningLength;
                nC_LastDocSale = C_GETnLastDocNum(1);

                C_GETtFormatDoc("TPSTSalHD", 9, Convert.ToDateTime(cVB.tVB_SaleDate), cVB.tVB_PosCode, cVB.tVB_ShpCode);
                tC_DocFmtLeftRfn = tC_DocFmtLeft;
                nC_DocRfnLength = nC_DocRuningLength;
                nC_LastDocRefund = C_GETnLastDocNum(9);

                oSql.AppendLine("SELECT ISNULL(MAX(FNVidNo),0) AS FNVidNo FROM TPSTVoidDT WITH(NOLOCK)");
                nC_LastDocVoid = oDB.C_GEToDataQuery<int>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_GETxLastDocNo : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        /// <summary>
        /// '*Em 63-06-04
        /// </summary>
        public static void C_PRCxReCalPntPmt()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                //update ยอด net
                oSql.AppendLine("UPDATE PD");
                oSql.AppendLine("SET FCXsdNet = DT.FCXsdNetAfHD ");
                oSql.AppendLine("FROM " + cSale.tC_TblSalPD + " PD ");
                oSql.AppendLine("INNER JOIN " + cSale.tC_TblSalDT + " DT WITH(NOLOCK) ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND PD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND ISNULL(PD.FCXpdPoint,0) > 0");
                oDB.C_SETxDataQuery(oSql.ToString());

                //ReCalPnt
                oSql.Clear();
                oSql.AppendLine("UPDATE PD");
                oSql.AppendLine("SET FCXpdPoint = TMP.FCXsdPoint");
                oSql.AppendLine("FROM " + cSale.tC_TblSalPD + " PD");
                oSql.AppendLine("INNER JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FTPmhDocNo,FLOOR(ISNULL(SUM(FCXsdNet),0)/PMT.FNPgtPntBuy) * PMT.FNPgtPntGet AS FCXsdPoint");
                oSql.AppendLine("	FROM " + cSale.tC_TblSalPD + " PD");
                oSql.AppendLine("	INNER JOIN (SELECT DISTINCT FTPmhDocNo,FNPgtPntGet,FNPgtPntBuy FROM TPMTPmt WITH(NOLOCK) WHERE FTPgtStaPoint = '2' AND FTPgtStaPntCalType = '1') PMT ON PMT.FTPmhDocNo = PD.FTPmhDocNo ");
                oSql.AppendLine("	WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNo + "' AND ISNULL(PD.FCXpdPoint,0) > 0");
                oSql.AppendLine("   AND EXISTS(SELECT FTPdtCode FROM TCNMPdt WITH(NOLOCK) WHERE FTPdtPoint = '1' and FTPdtCode = PD.FTPdtCode)");
                oSql.AppendLine("	GROUP BY PD.FTBchCode,PD.FTXshDocNo,PD.FTPmhDocNo,PMT.FNPgtPntGet,PMT.FNPgtPntBuy) TMP ");
                oSql.AppendLine("	ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FTPmhDocNo = TMP.FTPmhDocNo");
                oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNo + "' AND ISNULL(PD.FCXpdPoint,0) > 0");
                oDB.C_SETxDataQuery(oSql.ToString());

                //ลบรายการ Pro แต้ม ได้ 0 แต้ม
                oSql.Clear();
                oSql.AppendLine("DELETE FROM " + cSale.tC_TblSalPD + "");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FTXpdGetType = '6'  AND ISNULL(FCXpdPoint,0) = 0 ");
                oSql.AppendLine("AND ISNULL(FTXpdCpnText,'') = '' AND ISNULL(FTCpdBarCpn,'') = ''");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxReCalPntPmt : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        #endregion Function

        /// <summary>
        /// '*Em 63-06-09
        /// </summary>
        /// <param name="pbSignOut"></param>
        public static void C_DATxUsrLog(bool pbSignOut = false)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                if (pbSignOut)
                {
                    oSql.AppendLine("UPDATE TOP(1) Usr");
                    oSql.AppendLine("SET FDShdSignOut = GETDATE()");
                    oSql.AppendLine("FROM TPSTUsrLog Usr");
                    oSql.AppendLine("	WHERE FTComName = '" + Environment.MachineName + "' AND FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("	AND FTPosCode = '" + cVB.tVB_PosCode + "' AND FTUsrCode = '" + cVB.tVB_UsrCode + "'");
                    oSql.AppendLine("	AND FDShdSignOut IS NULL");
                    oSql.AppendLine("AND FDShdSignIn = ISNULL((SELECT TOP 1 FDShdSignIn ");
                    oSql.AppendLine("	FROM TPSTUsrLog");
                    oSql.AppendLine("	WHERE FTComName = '" + Environment.MachineName + "' AND FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("	AND FTPosCode = '" + cVB.tVB_PosCode + "' AND FTUsrCode = '" + cVB.tVB_UsrCode + "'");
                    oSql.AppendLine("	AND FDShdSignOut IS NULL");
                    oSql.AppendLine("	ORDER BY FDShdSignIn DESC),'')");
                    oDB.C_SETxDataQuery(oSql.ToString());
                }
                else
                {
                    oSql.AppendLine("INSERT INTO TPSTUsrLog");
                    oSql.AppendLine("(FTComName,FTBchCode,FTPosCode,FTUsrCode,FDShdSignIn,FTShfCode,FTAppCode,FTAppVersion)");
                    oSql.AppendLine("VALUES");
                    oSql.AppendLine("('" + Environment.MachineName + "','" + cVB.tVB_BchCode + "','" + cVB.tVB_PosCode + "',");
                    oSql.AppendLine("'" + cVB.tVB_UsrCode + "',GETDATE(),'" + (string.IsNullOrEmpty(cVB.tVB_ShfCode) ? "" : cVB.tVB_ShfCode) + "','" + (cVB.tVB_PosType == "1" ? "PS" : "FC") + "',");
                    oSql.AppendLine("'" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "')");
                    oDB.C_SETxDataQuery(oSql.ToString());
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxUsrLog : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }
        #region Print

        public static void C_GETxDataPrint(bool pbTrans = false)
        {
            string tTblSalHD = "TSHD" + cVB.tVB_PosCode;
            string tTblSalHDCst = "TSHDCst" + cVB.tVB_PosCode;
            string tTblSalHDDis = "TSHDDis" + cVB.tVB_PosCode;
            string tTblSalDT = "TSDT" + cVB.tVB_PosCode;
            string tTblSalDTDis = "TSDTDis" + cVB.tVB_PosCode;
            string tTblSalDTPmt = "TSDTPmt" + cVB.tVB_PosCode;
            string tTblSalRC = "TSRC" + cVB.tVB_PosCode;
            string tTblSalRD = "TSRD" + cVB.tVB_PosCode;
            string tTblSalPD = "TSPD" + cVB.tVB_PosCode;
            string tTblTxnSal = "TSHDPsl" + cVB.tVB_PosCode;
            string tTblTxnRD = "TSHDPrd" + cVB.tVB_PosCode;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                //*Net 63-06-01 ตารางจริง
                if (pbTrans)
                {
                    tTblSalHD = "TPSTSalHD";
                    tTblSalHDCst = "TPSTSalHDCst";
                    tTblSalHDDis = "TPSTSalHDDis";
                    tTblSalDT = "TPSTSalDT";
                    tTblSalDTDis = "TPSTSalDTDis";
                    tTblSalDTPmt = "TPSTSalDTPmt";
                    tTblSalRC = "TPSTSalRC";
                    tTblSalRD = "TPSTSalRD";
                    tTblSalPD = "TPSTSalPD";
                    tTblTxnSal = "TCNTMemTxnSale";
                    tTblTxnRD = "TCNTMemTxnRedeem";

                }
                //+++++++++++++++++++++++++++++++++++
                if (cVB.nVB_StaSumPrn == 1)
                {
                    // 1 :อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                    oSql.Clear();
                    //*Net 63-06-01 select ตาราง Trans/Temp จาก ตัวแปร
                    // Table 1
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
                    oSql.AppendLine("    WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("    AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' ");

                    #region Comment
                    //if (pbTrans)
                    //{
                    //    // Table 1
                    //    oSql.AppendLine("SELECT DT1.FTPdtCode, ");
                    //    oSql.AppendLine("DT1.FTXsdPdtName, ");
                    //    oSql.AppendLine("DT1.FTXsdBarCode, ");
                    //    oSql.AppendLine("SUM(DT1.FCXsdQty) AS FCXsdQty,");
                    //    oSql.AppendLine("DT1.FCXsdSetPrice,");
                    //    oSql.AppendLine("SUM(DT1.FCXsdNet) AS FCXsdNet,");
                    //    oSql.AppendLine("DT1.FTXsdVatType,");
                    //    oSql.AppendLine("SUM(DT1.FCXsdAmtB4DisChg) AS FCXsdAmtB4DisChg");
                    //    oSql.AppendLine("FROM(SELECT(SELECT TOP 1 FNXsdSeqNo FROM TPSTSalDT WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FTPdtCode = DT.FTPdtCode AND  FTXsdBarCode = DT.FTXsdBarCode ORDER BY FNXsdSeqNo ASC) AS FNXsdSeqNo,");
                    //    oSql.AppendLine("        DT.FTPdtCode,");
                    //    oSql.AppendLine("        DT.FTXsdPdtName,");
                    //    oSql.AppendLine("        DT.FTXsdBarCode,");
                    //    oSql.AppendLine("        DT.FCXsdQty AS FCXsdQty,");
                    //    oSql.AppendLine("        DT.FCXsdSetPrice,");
                    //    oSql.AppendLine("        DT.FCXsdNet AS FCXsdNet,");
                    //    oSql.AppendLine("        DT.FTXsdVatType,");
                    //    oSql.AppendLine("        DT.FCXsdAmtB4DisChg AS FCXsdAmtB4DisChg");
                    //    oSql.AppendLine("    FROM TPSTSalDT DT WITH(NOLOCK)");
                    //    oSql.AppendLine("    WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //    oSql.AppendLine("    AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' ");
                    //}
                    //else
                    //{
                    //    // Table 1
                    //    oSql.AppendLine("SELECT DT1.FTPdtCode, ");
                    //    oSql.AppendLine("DT1.FTXsdPdtName, ");
                    //    oSql.AppendLine("DT1.FTXsdBarCode, ");
                    //    oSql.AppendLine("SUM(DT1.FCXsdQty) AS FCXsdQty,");
                    //    oSql.AppendLine("DT1.FCXsdSetPrice,");
                    //    oSql.AppendLine("SUM(DT1.FCXsdNet) AS FCXsdNet,");
                    //    oSql.AppendLine("DT1.FTXsdVatType,");
                    //    oSql.AppendLine("SUM(DT1.FCXsdAmtB4DisChg) AS FCXsdAmtB4DisChg");
                    //    oSql.AppendLine("FROM(SELECT(SELECT TOP 1 FNXsdSeqNo FROM " + tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FTPdtCode = DT.FTPdtCode AND  FTXsdBarCode = DT.FTXsdBarCode ORDER BY FNXsdSeqNo ASC) AS FNXsdSeqNo,");
                    //    oSql.AppendLine("        DT.FTPdtCode,");
                    //    oSql.AppendLine("        DT.FTXsdPdtName,");
                    //    oSql.AppendLine("        DT.FTXsdBarCode,");
                    //    oSql.AppendLine("        DT.FCXsdQty AS FCXsdQty,");
                    //    oSql.AppendLine("        DT.FCXsdSetPrice,");
                    //    oSql.AppendLine("        DT.FCXsdNet AS FCXsdNet,");
                    //    oSql.AppendLine("        DT.FTXsdVatType,");
                    //    oSql.AppendLine("        DT.FCXsdAmtB4DisChg AS FCXsdAmtB4DisChg");
                    //    oSql.AppendLine("    FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                    //    oSql.AppendLine("    WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //    oSql.AppendLine("    AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' ");
                    //}
                    #endregion
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++

                    if (cVB.bVB_AlwPrnVoid == false)
                    {
                        oSql.AppendLine(" AND DT.FTXsdStaPdt <> '4' ");
                    }

                    oSql.AppendLine(") AS DT1 ");
                    oSql.AppendLine("GROUP BY DT1.FNXsdSeqNo, DT1.FTPdtCode,DT1.FTXsdPdtName,DT1.FTXsdBarCode, ");
                    oSql.AppendLine("DT1.FCXsdSetPrice,DT1.FTXsdVatType ");
                    oSql.AppendLine("ORDER BY DT1.FNXsdSeqNo ASC ");

                    //*Net 63-06-01 select ตาราง Trans/Temp จาก ตัวแปร
                    // Table 2 ส่วนลดรายการ
                    oSql.AppendLine("SELECT (SELECT FTXsdBarCode FROM " + tTblSalDT + " WHERE FTBchCode = DTDis.FTBchCode AND FTXshDocNo = DTDis.FTXshDocNo AND FNXsdSeqNo = DTDis.FNXsdSeqNo ) AS FTXsdBarCode,");
                    oSql.AppendLine("DTDis.FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    oSql.AppendLine("FROM " + tTblSalDTDis + " DTDis"); //*Arm 63-05-17
                    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    oSql.AppendLine("AND FNXddStaDis = 1");
                    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
                    oSql.AppendLine("ORDER BY FDXddDateIns");

                    #region Comment
                    //if (pbTrans) //*Arm 63-05-17
                    //{
                    //    // Table 2
                    //    oSql.AppendLine("SELECT (SELECT FTXsdBarCode FROM TPSTSalDT WHERE FTBchCode = DTDis.FTBchCode AND FTXshDocNo = DTDis.FTXshDocNo AND FNXsdSeqNo = DTDis.FNXsdSeqNo ) AS FTXsdBarCode,");
                    //    oSql.AppendLine("DTDis.FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    //    oSql.AppendLine("FROM TPSTSalDTDis DTDis");
                    //    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    //    oSql.AppendLine("AND FNXddStaDis = 1");
                    //    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
                    //    oSql.AppendLine("ORDER BY FDXddDateIns");
                    //}
                    //else
                    //{
                    //    // Table 2
                    //    oSql.AppendLine("SELECT (SELECT FTXsdBarCode FROM " + tC_TblSalDT + " WHERE FTBchCode = DTDis.FTBchCode AND FTXshDocNo = DTDis.FTXshDocNo AND FNXsdSeqNo = DTDis.FNXsdSeqNo ) AS FTXsdBarCode,");
                    //    oSql.AppendLine("DTDis.FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    //    oSql.AppendLine("FROM " + tC_TblSalDTDis + " DTDis"); //*Arm 63-05-17
                    //    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    //    oSql.AppendLine("AND FNXddStaDis = 1");
                    //    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
                    //    oSql.AppendLine("ORDER BY FDXddDateIns");
                    //}

                    //oDbSetPri = oDB.C_GEToDataSetQuery(oSql.ToString());
                    #endregion
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++
                }
                else
                {
                    // 2 :ไม่อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                    //*Net 63-06-01 select ตาราง Trans/Temp จาก ตัวแปร
                    // Table 1
                    oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FCXsdQty,");
                    oSql.AppendLine("DT.FTPdtCode,DT.FTXsdBarCode,");
                    oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdNet ,DT.FTXsdVatType,DT.FCXsdAmtB4DisChg");
                    oSql.AppendLine("FROM " + tTblSalDT + " DT WITH(NOLOCK)");
                    oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");

                    #region Comment
                    //*Em 63-04-29
                    //if (pbTrans)
                    //{
                    //    // Table 1
                    //    oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FCXsdQty,");
                    //    oSql.AppendLine("DT.FTPdtCode,DT.FTXsdBarCode,");
                    //    oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdNet ,DT.FTXsdVatType,DT.FCXsdAmtB4DisChg");
                    //    oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                    //    oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //    oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    //}
                    //else
                    //{
                    //    // Table 1
                    //    oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FCXsdQty,");
                    //    oSql.AppendLine("DT.FTPdtCode,DT.FTXsdBarCode,");
                    //    oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdNet ,DT.FTXsdVatType,DT.FCXsdAmtB4DisChg");
                    //    oSql.AppendLine("FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                    //    oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    //    oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    //}
                    //+++++++++++++++++
                    #endregion
                    //++++++++++++++++++++++++++++++++++++++++++++++++++

                    //*Em 63-03-28
                    if (cVB.bVB_AlwPrnVoid == false)
                    {
                        oSql.AppendLine(" AND DT.FTXsdStaPdt <> '4' ");
                    }
                    //+++++++++++++++++++++

                    oSql.AppendLine($"ORDER BY DT.FNXsdSeqNo");
                    ///////////////////////////////////////////////////////

                    //*Net 63-06-01 select ตาราง Trans/Temp จาก ตัวแปร
                    // Table 2 ส่วนลดรายการ
                    oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    oSql.AppendLine("FROM " + tTblSalDTDis + " DTDis WITH(NOLOCK)"); //*Arm 63-05-17
                    oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
                    oSql.AppendLine("AND FNXddStaDis = 1");
                    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");    //*Em 63-03-30
                    oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");

                    #region Comment
                    //if (pbTrans) //*Arm 63-05-17
                    //{
                    //    // Table 2
                    //    oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    //    oSql.AppendLine("FROM TPSTSalDTDis WITH(NOLOCK)"); //*Arm 63-03-05
                    //    oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
                    //    oSql.AppendLine("AND FNXddStaDis = 1");
                    //    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");    //*Em 63-03-30
                    //    oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");
                    //}
                    //else
                    //{
                    //    // Table 2
                    //    oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    //    oSql.AppendLine("FROM " + tC_TblSalDTDis + " DTDis WITH(NOLOCK)"); //*Arm 63-05-17
                    //    oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
                    //    oSql.AppendLine("AND FNXddStaDis = 1");
                    //    oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");    //*Em 63-03-30
                    //    oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");

                    //}
                    #endregion


                }

                //*Net 63-06-01 select ตาราง Trans/Temp จาก ตัวแปร
                // Table 3 โปรโมชั่น
                oSql.AppendLine("SELECT (CASE WHEN ISNULL(HDL.FTPmhNameSlip,'') = '' THEN HDL.FTPmhName ELSE HDL.FTPmhNameSlip END) AS FTPmdGrpName,PD.FCXpdDis");
                oSql.AppendLine("FROM " + tTblSalPD + " PD WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TPMTPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo");
                oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                //oSql.AppendLine("AND PD.FTXpdCpnText = '' AND PD.FTCpdBarCpn = ''");
                oSql.AppendLine("AND PD.FCXpdDis <> 0 ");   //*Em 63-04-12
                oSql.AppendLine("AND PD.FTXpdGetType <> '4' "); //*Em 63-04-29
                oSql.AppendLine("GROUP BY HDL.FTPmhNameSlip,HDL.FTPmhName,PD.FCXpdDis");

                #region Comment
                //if (pbTrans)
                //{
                //    // Table 3
                //    oSql.AppendLine("SELECT (CASE WHEN ISNULL(HDL.FTPmhNameSlip,'') = '' THEN HDL.FTPmhName ELSE HDL.FTPmhNameSlip END) AS FTPmdGrpName,PD.FCXpdDis");
                //    oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
                //    oSql.AppendLine("INNER JOIN TPMTPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo");
                //    oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                //    oSql.AppendLine("AND PD.FTXpdCpnText = '' AND PD.FTCpdBarCpn = ''");
                //    oSql.AppendLine("AND PD.FCXpdDis <> 0 ");   //*Em 63-04-12
                //    oSql.AppendLine("AND PD.FTXpdGetType <> '4' "); //*Em 63-04-29
                //    oSql.AppendLine("GROUP BY HDL.FTPmhNameSlip,HDL.FTPmhName,PD.FCXpdDis");
                //}
                //else
                //{
                //    // Table 3
                //    oSql.AppendLine("SELECT (CASE WHEN ISNULL(HDL.FTPmhNameSlip,'') = '' THEN HDL.FTPmhName ELSE HDL.FTPmhNameSlip END) AS FTPmdGrpName,PD.FCXpdDis");
                //    oSql.AppendLine("FROM " + tC_TblSalPD + " PD WITH(NOLOCK)");
                //    oSql.AppendLine("INNER JOIN TPMTPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo");
                //    oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                //    oSql.AppendLine("AND PD.FTXpdCpnText = '' AND PD.FTCpdBarCpn = ''");
                //    oSql.AppendLine("AND PD.FCXpdDis <> 0 ");   //*Em 63-04-12
                //    oSql.AppendLine("AND PD.FTXpdGetType <> '4' "); //*Em 63-04-29
                //    oSql.AppendLine("GROUP BY HDL.FTPmhNameSlip,HDL.FTPmhName,PD.FCXpdDis");
                //}
                #endregion
                //+++++++++++++++++++++++++++++++++++++++++++++

                // Get Price HD // Table 4 รวมเงินท้ายบิล
                //oSql.AppendLine("SELECT HD.FCXshTotal,HD.FCXshTotalNV,HD.FCXshTotalNoDis,HD.FCXshTotalB4DisChgV,HD.FCXshTotalB4DisChgNV,HD.FTXshDisChgTxt,");
                oSql.AppendLine("SELECT CONVERT(VARCHAR,HD.FDXshDocDate,120) AS FDXshDocDate,HD.FCXshTotal,HD.FCXshTotalNV,HD.FCXshTotalNoDis,HD.FCXshTotalB4DisChgV,HD.FCXshTotalB4DisChgNV,HD.FTXshDisChgTxt,"); //*Net 63-06-17 เพิ่ม FDXshDocDate
                oSql.AppendLine("HD.FCXshDis,HD.FCXshChg,HD.FCXshTotalAfDisChgV,HD.FCXshTotalAfDisChgNV,HD.FCXshRefAEAmt,HD.FCXshAmtV,HD.FCXshAmtNV,");
                oSql.AppendLine("HD.FCXshVat,HD.FCXshVatable,HD.FCXshGrand,HD.FTXshRmk,HD.FCXshRnd,HD.FTCstCode, HD.FTXshRefExt,HD.FTUsrCode,ISNULL(User_L.FTUsrName, (SELECT TOP 1 FTUsrName FROM TCNMUser_L WHERE FTUsrCode = HD.FTUsrCode)) AS FTUsrName, HD.FNXshDocPrint"); //*Arm 63-04-03 FTCstCode //Arm 63-04-08 FTXshRefExt //*Arm 63-05-08 FTUsrCode //*Net FNXshDocPrint //*Arm 63-08-03 ถ้า  FTUsrName = NULL ให้ Select ข้อมูล FTUsrName มาใหม่
                //oSql.AppendLine("FROM " + tC_TblSalHD + " HD WITH(NOLOCK)");
                oSql.AppendLine("FROM " + tTblSalHD + " HD WITH(NOLOCK)"); //*Net 63-06-07
                oSql.AppendLine("LEFT JOIN TCNMUser_L User_L ON HD.FTUsrCode = User_L.FTUsrCode AND User_L.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE HD.FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND HD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 

                //*Arm 63-04-03 - Print HDDis // Table 5 ส่วนลดท้ายบิล
                oSql.AppendLine("Declare @FTXhdRefCode varchar(30);");
                oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt, FTXhdRefCode = @FTXhdRefCode "); //*Arm 63-04-16                                                                                             
                //oSql.AppendLine("FROM " + tC_TblSalHDDis + " WITH(NOLOCK) "); //*Em 63-05-16
                oSql.AppendLine("FROM " + tTblSalHDDis + " WITH(NOLOCK) "); //*Net 63-06-07
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'  ");

                // Table 6
                oSql.AppendLine("SELECT FTRdhDocType");
                //oSql.AppendLine("FROM TPSTSalRD WITH(NOLOCK) ");
                //oSql.AppendLine("FROM " + tC_TblSalRD + " WITH(NOLOCK) ");    //*Em 63-05-16
                oSql.AppendLine("FROM " + tTblSalRD + " WITH(NOLOCK) ");   //*Net 63-06-07
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'  ");
                oSql.AppendLine("AND FTXrdRefCode = @FTXhdRefCode  ");

                // Table 7
                //หาจำนวนสินค้าทั้งหมด
                //oSql.AppendLine("SELECT SUM(ISNULL(FCXsdQty,0)) AS FCXsdQty FROM " + tC_TblSalDT + " with(nolock)"); //*Em 63-05-16
                oSql.AppendLine("SELECT SUM(ISNULL(FCXsdQty,0)) AS FCXsdQty FROM " + tTblSalDT + " with(nolock)"); //*Net 63-06-07
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' ");
                oSql.AppendLine("AND FTXsdStaPdt <> '4' ");

                //ชำระเงิน
                // Table 8
                oSql.AppendLine("SELECT RCV.FTRcvCode,(CASE WHEN ISNULL(RCVL.FTRcvName,'') = '' THEN (SELECT TOP 1 FTRcvName FROM TFNMRcv_L with(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode) ELSE RCVL.FTRcvName END) AS FTRcvName,"); 
                oSql.AppendLine("RCV.FCXrcUsrPayAmt,FCXrcDep,FCXrcChg,RCV.FTXrcRefNo1,RCV.FTXrcRefNo2 "); //*Net 63-03-28 ยกมาจาก baseline
                //oSql.AppendLine("FROM " + tC_TblSalRC + " RCV WITH(NOLOCK)"); //*Em 63-05-16
                oSql.AppendLine("FROM " + tTblSalRC + " RCV WITH(NOLOCK)"); //*Net 63-06-07
                //oSql.AppendLine("FROM TPSTSalRC RCV WITH(NOLOCK)"); //*Arm 63-03-02
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TFNMRcv RCVM WITH(NOLOCK) ON RCV.FTRcvCode = RCVM.FTRcvCode AND RCVM.FTFmtCode <> '020'");   //*Em 62-12-27
                oSql.AppendLine("WHERE RCV.FTBchCode =  '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND RCV.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND RCV.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
                oSql.AppendLine("ORDER BY RCV.FNXrcSeqNo");

                // ข้อมูลลูกค้า
                // Table 9
                oSql.AppendLine("SELECT FTBchCode, FTXshDocNo,FTXshCardID,FTXshCardNo,FTXshCstName,");
                oSql.AppendLine("FTXshCstTel,ISNULL(FCXshCstPnt,0) AS FCXshCstPnt, ISNULL(FCXshCstPntPmt,0) AS FCXshCstPntPmt");
                //oSql.AppendLine("FROM TPSTSalHDCst WITH(NOLOCK) ");
                //oSql.AppendLine("FROM " + tC_TblSalHDCst + " WITH(NOLOCK) "); //*Em 63-05-16
                oSql.AppendLine("FROM " + tTblSalHDCst + " WITH(NOLOCK) "); //*Net 63-06-07
                oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");

                //หา Transaction แต้มการขาย
                // Table 10
                //oSql.AppendLine("SELECT * FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + tC_TxnRefCode + "'");
                oSql.AppendLine($"SELECT FCTxnPntBillQty,FTMemCode FROM {tTblTxnSal} WHERE FTTxnRefDoc LIKE '%{tC_TxnRefCode}%'"); //*Net 63-06-02

                //if (nC_PrnDocType != 9)
                //{
                //    // Table 11
                //    //oSql.AppendLine("SELECT * FROM TCNTMemTxnRedeem WHERE FTRedRefDoc = '" + tC_TxnRefCode + "'");
                //    oSql.AppendLine($"SELECT FCRedPntBillQty FROM {tTblTxnRD} WHERE FTRedRefDoc = '" + tC_TxnRefCode + "'");//*Net 63-06-01
                //}
                //else
                //{
                //    // Table 11
                //    //oSql.AppendLine("SELECT FCRedPntB4Bill FROM TCNTMemTxnRedeem with(nolock) WHERE FTRedRefDoc = '" + tC_TxnRefCode + "'");
                //    oSql.AppendLine($"SELECT FCRedPntBillQty FROM {tTblTxnRD} with(nolock) WHERE FTRedRefDoc = '" + tC_TxnRefCode + "'");
                //}
                // Table 11
                oSql.AppendLine($"SELECT FCRedPntBillQty,FTMemCode FROM {tTblTxnRD} WHERE FTRedRefDoc LIKE '%{tC_TxnRefCode}%'");//*Net 63-06-01

                // Table 12
                //oSql.AppendLine("SELECT ISNULL(SUM(ISNULL(FNXrdPntUse,0)),0) AS FNXrdPntUse  FROM " + tC_TblSalRD + " WITH(NOLOCK) WHERE FTBchCode =  '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                oSql.AppendLine("SELECT ISNULL(SUM(ISNULL(FNXrdPntUse,0)),0) AS FNXrdPntUse  FROM " + tTblSalRD + " WITH(NOLOCK) WHERE FTBchCode =  '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");//*Net 63-06-07

                //พิมพ์สิทธิ์
                // Table 13
                oSql.AppendLine("SELECT DISTINCT FTPmhDocNo,FTXpdCpnText,FCXpdGetQtyDiv ");
                //oSql.AppendLine("FROM TPSTSalPD WITH(NOLOCK)");
                //oSql.AppendLine("FROM " + tC_TblSalPD + " WITH(NOLOCK)"); //*Em 63-05-16
                oSql.AppendLine("FROM " + tTblSalPD + " WITH(NOLOCK)"); //*Net 63-06-07
                oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                oSql.AppendLine("AND ISNULL(FTXpdCpnText,'') <> ''");

                //พิมพ์คูปอง Table 14
                oSql.AppendLine("SELECT DISTINCT PD.FTPmhDocNo,PD.FTCpdBarCpn,PD.FCXpdGetQtyDiv ,ISNULL(IMG.FTImgObj,'') AS FTImgPath");
                //oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
                //oSql.AppendLine("FROM " + tC_TblSalPD + " PD WITH(NOLOCK)"); //*Em 63-05-16 
                oSql.AppendLine("FROM " + tTblSalPD + " PD WITH(NOLOCK)"); //*Net 63-06-07
                oSql.AppendLine("LEFT JOIN TCNTPdtPmtCG CG WITH(NOLOCK) ON CG.FTPmhDocNo = PD.FTPmhDocNo AND FTPgtStaGetType = '6'");
                oSql.AppendLine("LEFT JOIN TCNMImgObj IMG WITH(NOLOCK) ON IMG.FTImgTable = 'TFNTCouponHD' AND IMG.FTImgRefID = CG.FTCphDocNo");
                oSql.AppendLine("WHERE PD.FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                oSql.AppendLine("AND ISNULL(PD.FTCpdBarCpn,'') <> ''");

                //*Net 63-06-01 Clear ตารางเก่า
                if (oDbSetPri != null)
                {
                    oDbSetPri.Clear();
                    oDbSetPri.Dispose();
                    oDbSetPri = null;
                }
                oDbSetPri = oDB.C_GEToDataSetQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_GETxDataPrint : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
            }
        }

        /// <summary>
        /// Process Print
        /// </summary>
        private static void C_PRCxPrintSlip(decimal pcPntB4Bill = 0m, string ptDateExpire = "")
        {
            PrintDocument oDoc = null;
            PrinterSettings oSettings;
            cSP oSP = new cSP();
            try
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Start", cVB.bVB_AlwPrnLog);
                oDoc = new PrintDocument();
                oSettings = new PrinterSettings();


                oDoc.DocumentName = cVB.tVB_DocNoPrn; //*Net 63-06-02
                oDoc.PrinterSettings.PrinterName = oSettings.PrinterName;
                oDoc.PrintController = new StandardPrintController();
                oDoc.PrintPage += (sender, e) =>
                {
                    C_PRNxSlip(e.Graphics, pcPntB4Bill, ptDateExpire);
                };

                bC_PrnCopy = false;
                //if (nC_DocType == 9) //*Net 63-02-25 การพิมพ์บิลคืน ให้พิมพ์ต้นฉบับ/สำเนา
                if (nC_PrnDocType == 9)
                {
                    for (int nMaster = 0; nMaster < cVB.nVB_PrnRefundMaster; nMaster++)
                    {
                        oDoc.Print();
                    }
                    bC_PrnCopy = true;
                    for (int nCopy = 0; nCopy < cVB.nVB_PrnRefundCopy; nCopy++)
                    {
                        oDoc.Print();
                    }
                }
                else
                {
                    //oDoc.Print();

                    //*Net 63-05-21  print ต้นฉบับ สำเนา ตาม option (CR P1-005 การพิมพ์สำเนาใบเสร็จ) //*Arm 63-06-11 ยกมาจาก SKC เดิม 
                    for (int nMaster = 0; nMaster < cVB.nVB_PrnSlipMaster; nMaster++)
                    {
                        oDoc.Print();
                    }
                    bC_PrnCopy = true;
                    for (int nCopy = 0; nCopy < cVB.nVB_PrnSlipCopy; nCopy++)
                    {
                        oDoc.Print();
                    }

                }
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : End", cVB.bVB_AlwPrnLog);
                //*Em 63-05-24
                //new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : C_PRCxTemp2Transaction start");
                //C_PRCxTemp2Transaction();
                //new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : C_PRCxTemp2Transaction end");
                //++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                //oSP.SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        //private static void C_PRNxSlip(object sender, PrintPageEventArgs e, decimal pcPntB4Bill = 0m, string ptDateExpire = "")
        public static void C_PRNxSlip(Graphics poGraphic, decimal pcPntB4Bill = 0m, string ptDateExpire = "", bool pbTrans = false)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine, tDeposit;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tAmt, tMsg;
            decimal cChange = 0;
            string tDataA = "";
            string tDataB = "";
            string tPrint = ""; //*Em 62-09-06
            string[] aRmk;  //*Em 62-09-06
            Image oLogo;
            DataTable odtTmp = new DataTable(); //*Em 63-04-09
            string tPicPath = "";   //*Em 63-04-10
            decimal cPntUse = 0;
            decimal cPntRcv = 0;
            cmlTPSTSalHDCst oHDCst; //*Arm 63-04-20
            decimal cTotalQty = 0;  //*Arm 63-04-20
            string tCstTel = "";    //*Arm 63-04-20

            try
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Start GETDATA ", cVB.bVB_AlwPrnLog);
                //C_GETxDataPrint();
                C_GETxDataPrint(pbTrans); //*Net 63-06-02
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : End GETDATA", cVB.bVB_AlwPrnLog);

                //Total //*Net 63-06-17 ย้ายมาจากข้างล่าง
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Set DataSet to DataTable Footer", cVB.bVB_AlwPrnLog);
                //oSql = new StringBuilder();
                oDbTblHD = oDbSetPri.Tables[3];
                oDbTblHDDis = oDbSetPri.Tables[4];
                oDbTblRD = oDbSetPri.Tables[5];
                oDbTblQty = oDbSetPri.Tables[6];
                oDbTblRC = oDbSetPri.Tables[7];
                oDbTblHDCst = oDbSetPri.Tables[8];
                oDbTblMem = oDbSetPri.Tables[9];
                oDbTblMemTxnRed = oDbSetPri.Tables[10];
                oDbTblMemRD = oDbSetPri.Tables[11];
                oDbTblPD2 = oDbSetPri.Tables[12];
                oDbTblPDCpn = oDbSetPri.Tables[13];
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Set DataSet to DataTable Footer end", cVB.bVB_AlwPrnLog);


                //nWidth = Convert.ToInt32(e.Graphics.VisibleClipBounds.Width);
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
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - 200) / 2, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }
                //+++++++++++++++++
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Print HD start", cVB.bVB_AlwPrnLog);
                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg

                // Get ค่า ID เครื่อง จาก DB
                //oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                //nStartY += 18;
                //oGraphic.DrawString(Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm") + " BNO:" + cVB.tVB_DocNoPrn,
                //                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                if (oDbTblHD != null && oDbTblHD.Rows.Count > 0)
                {
                    //*Net 63-06-17 พิมพ์ User ด้วย UsrCode ของบิล
                    oGraphic.DrawString("ID: " + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + oDbTblHD.Rows[0].Field<string>("FTUsrCode") + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                    nStartY += 18;
                    //*Net 63-06-17 พิมพ์วันที่โดยใช้ Docdate
                    string tFullDate = oDbTblHD.Rows[0].Field<string>("FDXshDocDate");
                    string tDate = DateTime.ParseExact(tFullDate, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm");
                    oGraphic.DrawString(tDate + " BNO:" + cVB.tVB_DocNoPrn,
                                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                }


                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Print HD End", cVB.bVB_AlwPrnLog);
                //Print DT
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : C_PRNxPrintDT start", cVB.bVB_AlwPrnLog);
                C_PRNxPrintDT(ref oGraphic, ref nStartY, nWidth, false); //*Net 63-03-28 ยกมาจาก baseline
                nStartY += 30;
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : C_PRNxPrintDT end", cVB.bVB_AlwPrnLog);

                ////Total Net 63-06-17 ย้ายไปย้ายบน
                //new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Set DataSet to DataTable Footer", cVB.bVB_AlwPrnLog);
                ////oSql = new StringBuilder();
                //oDbTblHD = oDbSetPri.Tables[3];
                //oDbTblHDDis = oDbSetPri.Tables[4];
                //oDbTblRD = oDbSetPri.Tables[5];
                //oDbTblQty = oDbSetPri.Tables[6];
                //oDbTblRC = oDbSetPri.Tables[7];
                //oDbTblHDCst = oDbSetPri.Tables[8];
                //oDbTblMem = oDbSetPri.Tables[9];
                //oDbTblMemTxnRed = oDbSetPri.Tables[10];
                //oDbTblMemRD = oDbSetPri.Tables[11];
                //oDbTblPD2 = oDbSetPri.Tables[12];
                //oDbTblPDCpn = oDbSetPri.Tables[13];
                //new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Set DataSet to DataTable Footer end", cVB.bVB_AlwPrnLog);

                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Print Footer Start", cVB.bVB_AlwPrnLog);

                if (oDbTblHD != null)
                {
                    //if (Convert.ToInt32(oDbTblHD.Rows[0]["FCXshDis"]) != 0 || Convert.ToInt32(oDbTblHD.Rows[0]["FCXshChg"]) != 0 || Convert.ToInt32(oDbTblHD.Rows[0]["FCXshRnd"]) != 0)
                    if (Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshDis"]) != (decimal)0 || Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshChg"]) != (decimal)0 || Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshRnd"]) != (decimal)0)    //*Em 63-06-24
                    {
                        //oGraphic.DrawString("Subtotal", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                        oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpSubTotal"), cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);   //*Arm 63-07-22 - ภาษา
                        tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshTotal"]), cVB.nVB_DecShow);
                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                        nStartY += 18;

                        if (oDbTblHDDis.Rows.Count > 0)
                        {
                            foreach (DataRow oHDDis in oDbTblHDDis.Rows)
                            {
                                if (string.IsNullOrEmpty(oHDDis.Field<string>("FTXhdRefCode")))
                                {
                                    //ส่วนลด 
                                    if (oHDDis.Field<string>("FTXhdDisChgType") == "1" || oHDDis.Field<string>("FTXhdDisChgType") == "2")
                                    {
                                        oGraphic.DrawString(cVB.oVB_GBResource.GetString("tDis") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXpdAmt, cVB.nVB_DecShow);
                                        tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oHDDis.Field<Decimal>("FCXhdAmt")), cVB.nVB_DecShow); //*Arm 63-04-16
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                        nStartY += 18;
                                    }
                                    //ชาจน์
                                    if (oHDDis.Field<string>("FTXhdDisChgType") == "3" || oHDDis.Field<string>("FTXhdDisChgType") == "4")
                                    {
                                        oGraphic.DrawString(cVB.oVB_GBResource.GetString("tChg") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXpdAmt, cVB.nVB_DecShow); 
                                        tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oHDDis.Field<Decimal>("FCXhdAmt")), cVB.nVB_DecShow); //*Arm 63-04-16
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                        nStartY += 18;
                                    }
                                }
                                else
                                {
                                    //Redeem
                                    if (oHDDis.Field<string>("FTXhdDisChgType") == "1")
                                    {

                                        if (oDbTblRD.Rows[0]["FTRdhDocType"].ToString() == "1")
                                        {
                                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tRdPdt") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        }
                                        else
                                        {
                                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tRdDis") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        }
                                        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXpdAmt, cVB.nVB_DecShow);
                                        tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oHDDis.Field<Decimal>("FCXhdAmt")), cVB.nVB_DecShow); //*Arm 63-04-16
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                        nStartY += 18;
                                    }
                                }

                                //Coupon
                                if (oHDDis.Field<string>("FTXhdDisChgType") == "5" || oHDDis.Field<string>("FTXhdDisChgType") == "6")
                                {
                                    oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCpnRd") + "(" + oHDDis.Field<string>("FTXhdDisChgTxt") + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXpdAmt, cVB.nVB_DecShow);
                                    tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oHDDis.Field<Decimal>("FCXhdAmt")), cVB.nVB_DecShow); //*Arm 63-04-16
                                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                    nStartY += 18;
                                }

                            }
                        }
                        //++++++++++++++

                        //*Arm 63-04-03 Comment Code
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

                        if (Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshRnd"]) != 0m)
                        {
                            tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshRnd"]), cVB.nVB_DecShow);
                            //oGraphic.DrawString("Round Rcv: " + tAmt, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpRound") + " : " + tAmt, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);   //*Arm 63-07-22 - ภาษา
                            tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshTotal"]) - Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshDis"]) + Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshChg"])), cVB.nVB_DecShow);
                            oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                            nStartY += 18;
                        }

                        //aoHDDis = null; //Clear
                    }

                    
                    //oGraphic.DrawString("TOTAL " + oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblQty.Rows[0]["FCXsdQty"]), cVB.nVB_DecShow) + " Items", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpTotal")+"("+ cVB.oVB_GBResource.GetString("tSlpVatIncluded") + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);   //*Arm 63-07-22 - ภาษา
                    tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshGrand"]), cVB.nVB_DecShow);
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);

                    //++++++++++++++++++

                    //*Arm 63-04-20 Comment Code
                    //nStartY += 18;
                    //oGraphic.DrawString("TOTAL (VAT Included)", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY); //*Arm 62-10-27 -เพิ่ม Wording  (VAT Included) ใบกำกับภาษีอย่างย่อ
                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshGrand, cVB.nVB_DecShow);
                    //oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);

                    nStartY += 18;
                    //tAmt = "Vatable : " + oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshVatable"]), cVB.nVB_DecShow);
                    //tAmt += " " + "VAT : " + oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshVat"]), cVB.nVB_DecShow);
                    tAmt = cVB.oVB_GBResource.GetString("tSlpVatable") +" : " + oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshVatable"]), cVB.nVB_DecShow);  //*Arm 63-07-22 - ภาษา
                    tAmt += " " + cVB.oVB_GBResource.GetString("tSlpVat") + " : " + oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHD.Rows[0]["FCXshVat"]), cVB.nVB_DecShow);  //*Arm 63-07-22 - ภาษา
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                }

                if (oDbTblRC != null)
                {

                    foreach (DataRow oRC in oDbTblRC.Rows)
                    {
                        nStartY += 18;
                        //oGraphic.DrawString(oRC.FTRcvName, cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                        //*Net 63-03-28 ยกมาจาก baseline
                        if (string.IsNullOrEmpty(oRC.Field<string>("FTXrcRefNo1")))
                        {
                            oGraphic.DrawString(oRC.Field<string>("FTRcvName"), cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(oRC.Field<string>("FTXrcRefNo2").Split(';')[oRC.Field<string>("FTXrcRefNo2").Split(';').Length - 1]))
                            {
                                oGraphic.DrawString(oRC.Field<string>("FTRcvName") + "(" + oRC.Field<string>("FTXrcRefNo1") + ")", cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                            }
                            else
                            {
                                oGraphic.DrawString(oRC.Field<string>("FTXrcRefNo2").Split(';')[oRC.Field<string>("FTXrcRefNo2").Split(';').Length - 1] + " (" + oRC.Field<string>("FTXrcRefNo1") + ")", cVB.aoVB_PInvLayout[6], Brushes.Black, 0, nStartY);
                            }

                        }
                        //++++++++++++++++++++++++++++++++
                        tAmt = oSP.SP_SETtDecShwSve(1, oRC.Field<Decimal>("FCXrcUsrPayAmt"), cVB.nVB_DecShow);
                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[7], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                        cChange = oRC.Field<Decimal>("FCXrcChg");
                    }


                    nStartY += 18;
                    //oGraphic.DrawString("Change", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpChange"), cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);  //*Arm 63-07-22 - ภาษา
                    tAmt = oSP.SP_SETtDecShwSve(1, cChange, cVB.nVB_DecShow);
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);

                }

                //*Arm 63-04-08 อ้างอิง SO
                if (!string.IsNullOrEmpty(oDbTblHD.Rows[0]["FTXshRefExt"].ToString()))
                {
                    nStartY += 18;
                    oGraphic.DrawString("อ้างอิง : " + oDbTblHD.Rows[0]["FTXshRefExt"].ToString(), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                }
                //++++++++++++++++++++++

                //*Arm 63-04-03 - Print Point
                if (!string.IsNullOrEmpty(oDbTblHD.Rows[0]["FTCstCode"].ToString()))
                {

                    if (oDbTblHDCst != null) //*Net 63-04-03
                    {
                        tCstTel = string.IsNullOrEmpty(oDbTblHDCst.Rows[0]["FTXshCstTel"].ToString()) ? "" : oDbTblHDCst.Rows[0]["FTXshCstTel"].ToString(); //*Arm 63-04-20 ใช้ใน QR Code


                        decimal cSumPnt = 0; //*Arm 63-05-09

                        if (!string.IsNullOrEmpty(oDbTblHDCst.Rows[0]["FTXshDocNo"].ToString())) //*Arm 63-04-03 
                        {

                            nStartY += 30;
                            //oGraphic.DrawString("Mem ID : " + oDbTblHD.Rows[0]["FTCstCode"].ToString(), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpMemCode") + " : " + oDbTblHD.Rows[0]["FTCstCode"].ToString(), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);  //*Arm 63-07-22 - ภาษา
                            nStartY += 18;
                            oSql = new StringBuilder();
                            oGraphic.DrawString(oDbTblHDCst.Rows[0]["FTXshCstName"].ToString(), cVB.aoVB_PInvLayout[1], Brushes.Black, 10, nStartY);

                            string tCardNo = string.IsNullOrEmpty(oDbTblHDCst.Rows[0]["FTXshCardNo"].ToString()) ? " - " : oDbTblHDCst.Rows[0]["FTXshCardNo"].ToString();
                            nStartY += 18;
                            //oGraphic.DrawString("Card No. : " + tCardNo, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpMemCard") + " : " + tCardNo, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                            nStartY += 18;
                            if (!string.IsNullOrEmpty(ptDateExpire))
                            {
                                //oGraphic.DrawString("Expired : " + DateTime.ParseExact(ptDateExpire, "yyyy-MM-dd", null).ToString("dd-MM-yyyy"), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpMemExpire") + " : " + DateTime.ParseExact(ptDateExpire, "yyyy-MM-dd", null).ToString("dd-MM-yyyy"), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);   //*Arm 63-07-22 - ภาษา
                            }
                            else
                            {
                                //oGraphic.DrawString("Expired : -", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpMemExpire") + " : -", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);  //*Arm 63-07-22 - ภาษา
                            }

                            nStartY += 18;
                            //oGraphic.DrawString("Last Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                            //oGraphic.DrawString("Reg. Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                            //oGraphic.DrawString("Promo Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                            //oGraphic.DrawString("Total Point", cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);

                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpMemLastPt"), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);      //*Arm 63-07-22 - ภาษา
                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpMemRegPt"), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);      //*Arm 63-07-22 - ภาษา
                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpMemPmtPt"), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);     //*Arm 63-07-22 - ภาษา
                            oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpMemTotalPt"), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);   //*Arm 63-07-22 - ภาษา

                            //Print ข้อมมูล Member
                            //if (oDbTblMem!=null && oDbTblMem.Rows.Count>0 && !string.IsNullOrEmpty(oDbTblMem.Rows[0]["FTMemCode"].ToString()))
                            //{
                            //nStartY += 30;
                            //oGraphic.DrawString("Mem ID : " + oDbTblMem.Rows[0]["FTMemCode"].ToString(), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                            //nStartY += 18;
                            //oSql = new StringBuilder();
                            //oGraphic.DrawString(oDbTblHDCst.Rows[0]["FTXshCstName"].ToString(), cVB.aoVB_PInvLayout[1], Brushes.Black, 10, nStartY);

                            //string tCardNo = string.IsNullOrEmpty(oDbTblHDCst.Rows[0]["FTXshCardNo"].ToString()) ? " - " : oDbTblHDCst.Rows[0]["FTXshCardNo"].ToString();
                            //nStartY += 18;
                            //oGraphic.DrawString("Card No. : " + tCardNo, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                            //nStartY += 18;
                            //if (!string.IsNullOrEmpty(oDbTblMem.Rows[0]["FDTxnPntExpired"].ToString()))
                            //{
                            //    oGraphic.DrawString("Expired : " + string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oDbTblMem.Rows[0]["FDTxnPntExpired"].ToString())), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                            //}
                            //else
                            //{
                            //    oGraphic.DrawString("Expired : -", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                            //}

                            //nStartY += 18;
                            //oGraphic.DrawString("Last Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                            //oGraphic.DrawString("Reg. Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                            //oGraphic.DrawString("Promo Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                            //oGraphic.DrawString("Total Point", cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);

                            //nStartY += 18;
                            ////oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)oTxnSale.FCTxnPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                            //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)cVB.nVB_CstPiontB4UsedPrn, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                            //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                            //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPntPmt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                            //cPntRcv = oHDCst.FCXshCstPnt + oHDCst.FCXshCstPntPmt;
                            ////oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oTxnSale.FCTxnPntB4Bill + cPntRcv), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);
                            //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal((decimal)cVB.nVB_CstPiontB4UsedPrn + cPntRcv), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29


                            //*Arm 63-05-07 
                            if (nC_PrnDocType != 9)
                            {
                                //แต้มที่ใช้

                                if (oDbTblMemTxnRed != null && oDbTblMemTxnRed.Rows.Count > 0) //*Net 63-05-29 เช็คว่ามีข้อมูล TxnRedeem หรือไม่
                                {
                                    nStartY += 18;
                                    //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblMemTxnRed.Rows[0]["FCRedPntB4Bill"]), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, pcPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    decimal cRedPnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMemTxnRed.Rows[0]["FCRedPntBillQty"]) * (-1));  //*Arm 63-05-09
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRedPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                    //cPntRcv = (decimal)(oTxnRedeem.FCRedPntB4Bill + (oTxnRedeem.FCRedPntBillQty * (-1)));
                                    //cSumPnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMemTxnRed.Rows[0]["FCRedPntB4Bill"]) + cRedPnt); //*Arm 63-05-09
                                    cSumPnt = Convert.ToDecimal(pcPntB4Bill + cRedPnt); //*Arm 63-05-09
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29
                                    pcPntB4Bill = cSumPnt;
                                }
                                if (oDbTblMem != null && oDbTblMem.Rows.Count > 0) //*Net 63-05-29 เช็คว่ามีข้อมูล TxnSal หรือไม่
                                {
                                    nStartY += 18;
                                    //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntB4Bill"]), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, pcPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    //decimal cSalePnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntBillQty"]) - Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]));
                                    decimal cSalePnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntBillQty"]) - Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]));
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSalePnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                    //cSumPnt = (decimal)(oTxnSale.FCTxnPntB4Bill + oTxnSale.FCTxnPntBillQty);
                                    //cSumPnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntB4Bill"]) + (cSalePnt + Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]))); //*Arm 63-05-09
                                    cSumPnt = Convert.ToDecimal(pcPntB4Bill + (cSalePnt + Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]))); //*Arm 63-05-09
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);
                                }

                                //*Net 63-06-02 ถ้าไม่มีข้อมูลเลย
                                if ((oDbTblMemTxnRed == null || oDbTblMemTxnRed.Rows.Count == 0) && (oDbTblMem == null || oDbTblMem.Rows.Count == 0))
                                {
                                    nStartY += 18;
                                    //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntB4Bill"]), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, pcPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29

                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                    //cSumPnt = (decimal)(oTxnSale.FCTxnPntB4Bill + oTxnSale.FCTxnPntBillQty);
                                    //cSumPnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntB4Bill"]) + (cSalePnt + Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]))); //*Arm 63-05-09
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, pcPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);
                                }
                            }
                            else
                            {
                                //หา Point ก่อนใช้

                                //if (oDbTblMemTxnRed.Rows.Count > 0)
                                //{

                                //    cCstPiontB4Used = Convert.ToDecimal(oDbTblMemTxnRed.Rows[0]["FCRedPntB4Bill"]);
                                //}
                                //else
                                //{
                                //    cCstPiontB4Used = Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntB4Bill"]);
                                //}
                                cCstPiontB4Used = pcPntB4Bill;
                                cRefundRed = Convert.ToDecimal(oDbTblMemRD.Rows[0]["FNXrdPntUse"]);
                                decimal cRefundPnt = ((cRefundRed - Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPnt"])) * (-1)); //*Arm 63-05-09

                                if (Math.Abs(cRefundPnt) > 0) //*Net 63-06-02
                                {
                                    //# แต้มที่ต้องคืนร้าน
                                    nStartY += 18;
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(cCstPiontB4Used), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRefundPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                    cSumPnt = (cCstPiontB4Used + (cRefundPnt + Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]))); //*Arm 63-05-09
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29

                                    cCstPiontB4Used = cSumPnt;
                                }

                                //# แต้มที่ได้รับคืน
                                //cPntRcv = cRefundRed;

                                if (cRefundRed > 0)
                                {
                                    nStartY += 18;
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(cCstPiontB4Used), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRefundRed, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                    cSumPnt = Convert.ToDecimal(cCstPiontB4Used + cRefundRed);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);
                                }
                            }
                            //+++++++++++++++++++++

                            //if (oDbTblMemTxnRed != null && oDbTblMemTxnRed.Rows.Count > 0) //*Net 63-05-29 เช็คว่ามีข้อมูลหรือไม่
                            //{
                            //    nStartY += 18;
                            //    //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblMemTxnRed.Rows[0]["FCRedPntB4Bill"]), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                            //    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, pcPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                            //    decimal cRedPnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMemTxnRed.Rows[0]["FCRedPntBillQty"]) * (-1));  //*Arm 63-05-09
                            //    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRedPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                            //    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                            //    //cPntRcv = (decimal)(oTxnRedeem.FCRedPntB4Bill + (oTxnRedeem.FCRedPntBillQty * (-1)));
                            //    //cSumPnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMemTxnRed.Rows[0]["FCRedPntB4Bill"]) + cRedPnt); //*Arm 63-05-09
                            //    cSumPnt = Convert.ToDecimal(pcPntB4Bill + cRedPnt); //*Arm 63-05-09
                            //    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29
                            //    pcPntB4Bill = cSumPnt;
                            //}

                            //nStartY += 18;
                            ////oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntB4Bill"]), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                            //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, pcPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29

                            //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                            //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                            ////cSumPnt = (decimal)(oTxnSale.FCTxnPntB4Bill + oTxnSale.FCTxnPntBillQty);
                            ////cSumPnt = Convert.ToDecimal(Convert.ToDecimal(oDbTblMem.Rows[0]["FCTxnPntB4Bill"]) + (cSalePnt + Convert.ToDecimal(oDbTblHDCst.Rows[0]["FCXshCstPntPmt"]))); //*Arm 63-05-09
                            //oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, pcPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);


                            oTxnSale = null;
                        }


                    }
                }

                //*Arm 63-05-08 Print ชื่อผู้ขาย
                string tUserName = oDbTblHD.Rows[0]["FTUsrName"].ToString();
                nStartY += 30;
                //oGraphic.DrawString("User : " + tUserName, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tSlpUsrName") +" : " + tUserName, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                //+++++++++++++

                if (nC_PrnDocType == 9)
                {
                    nStartY += 30;
                    tMsg = cVB.oVB_GBResource.GetString("tRefundPdt");
                    if (!string.IsNullOrEmpty(tC_RefDocNoPrn))
                    {
                        tMsg += cVB.oVB_GBResource.GetString("tRefer") + ":" + tC_RefDocNoPrn;
                    }
                    oGraphic.DrawString(tMsg, cVB.aoVB_PInvLayout[8], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                }

                if (bC_PrnCopy) //*Net 63-02-25 เมื่อพิมพ์สำเนา
                {
                    //สำเนา
                    nStartY += 18;
                    tPrint = "!!! " + cVB.oVB_GBResource.GetString("tCopy") + " " + Convert.ToInt32(oDbTblHD.Rows[0]["FNXshDocPrint"]) + " !!!";
                    oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                    //++++++++++++++
                }

                //Remark
                if (!string.IsNullOrEmpty(oDbTblHD.Rows[0]["FTXshRmk"].ToString()))
                {
                    nStartY += 18;
                    tPrint = oDbTblHD.Rows[0]["FTXshRmk"].ToString();
                    aRmk = tPrint.Split((char)10);
                    foreach (string tStr in aRmk)
                    {
                        nStartY += 18;
                        oGraphic.DrawString(tStr, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    }
                }

                //*Arm 62-10-31  เงื่อนไขการแสดง Barcode และ QRCode
                string tCstCode = oDbTblHD.Rows[0]["FTCstCode"].ToString();
                switch (cVB.nVB_ChkShowBarQR)
                {
                    case 1:    //1 : แสดง Barcode
                        nStartY += 30;
                        nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                        nStartY += 18;
                        C_PRNxBarcode(ref oGraphic, cVB.tVB_DocNoPrn, nWidth, nStartY);
                        break;

                    case 2:     //2 : แสดง QRCode,
                        nStartY += 30;
                        nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                        nStartY += 18;
                        tDataA = cVB.tVB_DocNoPrn;
                        //tDataB = cVB.tVB_DocNoPrn + "|" + cVB.tVB_CstCode + "|" + cVB.tVB_CstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //*Arm 63-04-20 Comment Code
                        tDataB = cVB.tVB_DocNoPrn + "|" + tCstCode == null ? "" : tCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", (decimal)(oDbTblHD.Rows[0]["FCXshGrand"])) + " | " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //*Arm 63-04-20
                        tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
                        C_PRNxQRCode(ref oGraphic, tDataA + "|" + tDataB, nWidth, nStartY);
                        nStartY += 138;
                        break;

                    case 3:     //3 : แสดงทั้ง Barcode และ QRcode
                        nStartY += 30;
                        nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                        nStartY += 18;
                        C_PRNxBarcode(ref oGraphic, cVB.tVB_DocNoPrn, nWidth, nStartY);
                        nStartY += 30;
                        tDataA = cVB.tVB_DocNoPrn;
                        //tDataB = cVB.tVB_DocNoPrn + "|" + cVB.tVB_CstCode + "|" + cVB.tVB_CstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //*Arm 63-04-20 Comment Code
                        tDataB = cVB.tVB_DocNoPrn + "|" + tCstCode == null ? "" : tCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", (decimal)(oDbTblHD.Rows[0]["FCXshGrand"])) + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //*Arm 63-04-20
                        tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
                        C_PRNxQRCode(ref oGraphic, tDataA + "|" + tDataB, nWidth, nStartY);
                        nStartY += 138;
                        break;

                    default:    // ไม่แสดง
                        nStartY += 30;
                        nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                        nStartY += 18;
                        break;
                }
                //+++++++++++++++++++

                if (oDbTblPD2.Rows.Count > 0)
                {

                    foreach (DataRow oRow in oDbTblPD2.Rows)
                    {
                        nStartY += 18;
                        oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                        nStartY += 18;
                        tPrint = oRow.Field<string>("FTXpdCpnText") + " " + Convert.ToInt16(oRow.Field<Decimal>("FCXpdGetQtyDiv")).ToString() + " " + cVB.oVB_GBResource.GetString("tPrivilege");
                        oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                    }

                }

                if (oDbTblPDCpn != null && oDbTblPDCpn.Rows.Count > 0)
                {
                    foreach (DataRow oRow in oDbTblPDCpn.Rows)
                    {
                        nStartY += 18;
                        oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                        nStartY += 18;
                        tPrint = cVB.oVB_GBResource.GetString("tCoupon");
                        oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                        tPicPath = oRow.Field<string>("FTImgPath");
                        if (!string.IsNullOrEmpty(tPicPath))
                        {
                            if (File.Exists(tPicPath))
                            {
                                oLogo = Image.FromFile(tPicPath);
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

                        nStartY += 20;
                        tPrint = oRow.Field<string>("FTCpdBarCpn");
                        C_PRNxBarcode(ref oGraphic, tPrint, nWidth, nStartY);

                        nStartY += 20;
                        tPrint = Convert.ToInt16(oRow.Field<Decimal>("FCXpdGetQtyDiv")).ToString() + " " + cVB.oVB_GBResource.GetString("tPrivilege");
                        oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                    }

                }
                //++++++++++++++++++++
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Print Footer End", cVB.bVB_AlwPrnLog);
                //nStartY += 30;
                //nStartY = oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
                //nStartY += 15;
                //C_PRNxBarcode(ref oGraphic, cVB.tVB_DocNo, nWidth, nStartY);
                //nStartY += 30;
                //tDataA = cVB.tVB_DocNo;
                //tDataB = cVB.tVB_DocNo + "|" + cVB.tVB_CstCode + "|" + cVB.tVB_CstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //tDataB = new cEncryptDecrypt("1").C_CALtEncrypt(tDataB, "Adasoft");
                //C_PRNxQRCode(ref oGraphic, tDataA + "|" + tDataB, nWidth, nStartY);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRNxSlip : " + oEx.Message); }
            finally
            {
                //*Net 63-06-06
                if (oDbTblHD != null) oDbTblHD.Dispose();
                if (oDbTblHDDis != null) oDbTblHDDis.Dispose();
                if (oDbTblRD != null) oDbTblRD.Dispose();
                if (oDbTblQty != null) oDbTblQty.Dispose();
                if (oDbTblRC != null) oDbTblRC.Dispose();
                if (oDbTblHDCst != null) oDbTblHDCst.Dispose();
                if (oDbTblMem != null) oDbTblMem.Dispose();
                if (oDbTblMemTxnRed != null) oDbTblMemTxnRed.Dispose();
                if (oDbTblMemRD != null) oDbTblMemRD.Dispose();
                if (oDbTblPD2 != null) oDbTblPD2.Dispose();
                if (oDbTblPDCpn != null) oDbTblPDCpn.Dispose();
                if (oDbSetPri != null) oDbSetPri.Dispose();
                //++++++++++++++++++++++++++++++++

                if (oGraphic != null)
                    oGraphic.Dispose();

                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();

                //*Net 63-06-06
                oDbTblHD = null;
                oDbTblHDDis = null;
                oDbTblRD = null;
                oDbTblQty = null;
                oDbTblRC = null;
                oDbTblHDCst = null;
                oDbTblMem = null;
                oDbTblMemTxnRed = null;
                oDbTblMemRD = null;
                oDbTblPD2 = null;
                oDbTblPDCpn = null;
                oDbSetPri = null;
                //+++++++++++++++++++++
                oLogo = null;
                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                oMsg = null;
                tLine = null;
                odtTmp = null;  //*Em 63-04-09
                oHDCst = null; //*Arm 63-04-20
                //oSP.SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
                oSP = null;
            }
        }

        private static void C_PRNxPrintDT(ref Graphics poGraphic, ref int pnStartY, int pnWidth, bool pbTrans = false) //*Net 63-03-28 pTrans ยกมาจาก baseline
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            List<cmlTPSTSalDT> aoDT;
            List<cmlTPSTSalDTDis> aoDTDis;
            List<cmlPrnSplipDTDis> aoPrnDTDis;
            List<cmlTPSTSalPD> aoPD;    //*Em 63-03-29
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
                oDbTblDT = oDbSetPri.Tables[0];
                oDbTblDTDis = oDbSetPri.Tables[1];
                oDbTblPD = oDbSetPri.Tables[2];

                #region Comment
                //oSql.AppendLine("SELECT FNXsdSeqNo,FTPdtCode,FTXsdPdtName,FTXsdBarCode,FCXsdQty,FCXsdSetPrice,FCXsdNet,FTXsdVatType");
                //oSql.AppendLine(",FCXsdAmtB4DisChg");   //*Em 62-10-08
                //oSql.AppendLine("FROM " + tC_TblSalDT);
                //oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oSql.AppendLine("ORDER BY FNXsdSeqNo");

                ////*Net 63-02-25 เช็ก Config ว่าให้พิมพ์รายการ void หรือไม่
                //oSql.AppendLine($"SELECT FNXsdSeqNo,FTPdtCode,FTXsdPdtName,FTXsdBarCode,FCXsdQty,");
                //oSql.AppendLine($"FTPdtCode,FTXsdBarCode,"); //*Arm 63-04-13
                //oSql.AppendLine($"FCXsdSetPrice,FCXsdNet ,FTXsdVatType,FCXsdAmtB4DisChg");
                ////oSql.AppendLine($"FROM {tC_TblSalDT} SDT");
                ////oSql.AppendLine($"FROM TPSTSalDT SDT WITH(NOLOCK)"); //*Arm 63-03-05
                ////*Net 63-03-28 ยกมาจาก baseline
                //if (pbTrans)
                //{
                //    oSql.AppendLine("FROM TPSTSalDT WITH(NOLOCK)"); //*Arm 63-03-02
                //}
                //else
                //{
                //    oSql.AppendLine("FROM " + tC_TblSalDT + " WITH(NOLOCK)");
                //}
                ////+++++++++++++++++++++++++++++++++++
                ////oSql.AppendLine($"LEFT JOIN (SELECT [FTPdtCode] FROM TSDT00013");
                ////oSql.AppendLine($"LEFT JOIN (SELECT [FTPdtCode] FROM TPSTSalDT"); //*Arm 63-03-05
                ////oSql.AppendLine($"            WHERE (");
                ////oSql.AppendLine($"            CASE WHEN ('{cVB.bVB_AlwPrnVoid.ToString().ToLower()}' ='false' AND [FCXsdQty]<0) THEN");
                ////oSql.AppendLine($"                             [FTPdtCode] ");
                ////oSql.AppendLine($"                 ELSE NULL");
                ////oSql.AppendLine($"            END) IS NOT NULL) SDT_V ON SDT.FTPdtCode=SDT_V.FTPdtCode");
                //oSql.AppendLine($" WHERE FTBchCode =  '{cVB.tVB_BchCode}'");
                ////oSql.AppendLine($" AND FTXshDocNo = '{cVB.tVB_DocNo}'");
                //oSql.AppendLine($" AND FTXshDocNo = '{cVB.tVB_DocNoPrn}'"); //*Arm 63-03-05 
                ////oSql.AppendLine($" AND SDT_V.FTPdtCode IS NULL");
                #endregion

                //++++++ *Arm 63-05-05  สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต +++++++
                if (cVB.nVB_StaSumPrn == 1) // 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                {


                    //aoDT = oDbTblDT.AsEnumerable().ToList<cmlTPSTSalDT>();

                    if (oDbTblDT != null)
                    {
                        foreach (DataRow oDT in oDbTblDT.Rows)
                        {
                            string tPdt = "";   //*Arm 63-07-24
                            string tRownum = "";
                            //cVB.bVB_PrnRownum = false;
                            if (cVB.bVB_PrnRownum)
                            {
                                nRownum = oDbTblDT.Rows.IndexOf(oDT) + 1;
                                tRownum = nRownum + ".";
                            }
                            else
                            {
                                tRownum = "";
                            }

                            switch (cVB.nVB_ChkShowPdtBarCode)
                            {
                                case 1:
                                    pnStartY += 18;
                                    ////poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    //poGraphic.DrawString(tRownum + oDT["FTPdtCode"].ToString() + " " + oDT["FTXsdPdtName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));     //*Arm 63-05-12

                                    //*Arm 63-07-24
                                    tPdt = cPrint.C_GETxPdtLeft(oDT["FTPdtCode"].ToString(), 15) + " " + cPrint.C_GETxPdtLeft(oDT["FTXsdPdtName"].ToString(), 15);
                                    poGraphic.DrawString(tRownum + tPdt, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    //+++++++++++++
                                    break;
                                case 2:
                                    pnStartY += 18;
                                    ////poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    //poGraphic.DrawString(tRownum + oDT["FTXsdBarCode"].ToString() + " " + oDT["FTXsdPdtName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));  //*Arm 63-05-12

                                    //*Arm 63-07-24
                                    tPdt = cPrint.C_GETxPdtLeft(oDT["FTXsdBarCode"].ToString(), 15) + " " + cPrint.C_GETxPdtLeft(oDT["FTXsdPdtName"].ToString(), 15);
                                    poGraphic.DrawString(tRownum + tPdt, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    //+++++++++++++
                                    break;

                                default:
                                    pnStartY += 18;
                                    //poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    //poGraphic.DrawString(tRownum + oDT["FTXsdPdtName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));   //*Arm 63-05-12

                                    //*Arm 63-07-24
                                    tPdt = cPrint.C_GETxPdtLeft(oDT["FTXsdPdtName"].ToString(), 30);
                                    poGraphic.DrawString(tRownum + tPdt, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    //+++++++++++++
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
                            //*Arm 63-05-12
                            if (Convert.ToInt32(oDT["FCXsdQty"]) > 1)
                            {
                                pnStartY += 18;
                                //poGraphic.DrawString("  " + oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                poGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDT["FCXsdQty"]), cVB.nVB_DecShow) + " x " + oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDT["FCXsdSetPrice"]), cVB.nVB_DecShow), cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);    //*Em 63-05-25
                                tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDT["FCXsdAmtB4DisChg"]), cVB.nVB_DecShow);
                                poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            else
                            {
                                tAmt = oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oDT["FCXsdAmtB4DisChg"]), cVB.nVB_DecShow);
                                poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            //+++++++++++++

                            #region Comment
                            //+++++++++++++
                            //if (cVB.nVB_ChkShowPdtBarCode > 0)
                            //{
                            //    pnStartY += 18;
                            //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                            //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);

                            //    //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
                            //    //{
                            //    //    pnStartY += 18;
                            //    //    poGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);

                            //    //    switch (cVB.nVB_ChkShowPdtBarCode)
                            //    //    {
                            //    //        case 1:
                            //    //            pnStartY += 18;
                            //    //            poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //            break;
                            //    //        case 2:
                            //    //            pnStartY += 18;
                            //    //            poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //            break;

                            //    //        default:
                            //    //            pnStartY += 18;
                            //    //            poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //            break;
                            //    //    }

                            //    //    pnStartY += 18;
                            //    //    poGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //    //}
                            //    //else
                            //    //{
                            //    //    pnStartY += 18;
                            //    //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //    //}
                            //}
                            //else
                            //{
                            //    if (oDT.FCXsdQty > 1)
                            //    {
                            //        pnStartY += 18;
                            //        poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                            //        poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //        //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
                            //        //{
                            //        //    pnStartY += 18;
                            //        //    poGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //        //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);

                            //        //    switch (cVB.nVB_ChkShowPdtBarCode)
                            //        //    {
                            //        //        case 1:
                            //        //            pnStartY += 18;
                            //        //            poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //            break;
                            //        //        case 2:
                            //        //            pnStartY += 18;
                            //        //            poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //            break;

                            //        //        default:
                            //        //            pnStartY += 18;
                            //        //            poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //            break;
                            //        //    }

                            //        //    pnStartY += 18;
                            //        //    poGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //        //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //        //}
                            //        //else
                            //        //{
                            //        //    pnStartY += 18;
                            //        //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //        //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //        //}
                            //    }
                            //    else
                            //    {
                            //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                            //        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdSalePrice, cVB.nVB_DecShow);    //*Em 63-05-01
                            //        poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //    }
                            //}
                            #endregion

                            if (oDbTblDTDis != null) //Zen
                            {
                                decimal cAmt = (decimal)(oDT["FCXsdAmtB4DisChg"]);
                                decimal cDis = 0; //เก็บผลรวมส่วนลด
                                decimal cChg = 0; //เก็บผลรวมชาจน์
                                int nRow = 0;
                                foreach (DataRow oDTDis in oDbTblDTDis.Rows)// aoPrnDTDis.Where(c => c.FTXsdBarCode == oDT.FTXsdBarCode))
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
                                        //poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " (" + cDis + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                        poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " (" + oSP.SP_SETtDecShwSve(1, cDis, cVB.nVB_DecShow) + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY); //*Arm 63-08-12 ปรับการแสดงทศนิยม
                                        cAmt = (cAmt - cDis);
                                        tAmt = oSP.SP_SETtDecShwSve(1, cAmt, cVB.nVB_DecShow);
                                        //poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                        poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                    }

                                    if (cChg > 0)   // แสดงชาจน์
                                    {
                                        pnStartY += 18;
                                        //poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tChg") + " (" + cChg + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                        poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tChg") + " (" + oSP.SP_SETtDecShwSve(1, cChg, cVB.nVB_DecShow) + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY); //*Arm 63-08-12 ปรับการแสดงทศนิยม
                                        cAmt = (cAmt + cChg);
                                        tAmt = oSP.SP_SETtDecShwSve(1, cAmt, cVB.nVB_DecShow);
                                        //poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                        poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                    }
                                }//End  Transaction ส่วนลดรายการ
                            }//End if (aoDTDis != null)
                        }//End foreach (cmlTPSTSalDT oDT in aoDT)
                    }//End if (aoDT != null)

                }//End 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์ *Arm 63-05-05 
                else
                {

                    if (oDbTblDT != null) //Zen
                    {
                        foreach (DataRow oDT in oDbTblDT.Rows)
                        {
                            string tPdt = "";   //*Arm 63-07-24
                            string tRownum = "";
                            //cVB.bVB_PrnRownum = false;
                            if (cVB.bVB_PrnRownum)
                            {
                                nRownum = oDbTblDT.Rows.IndexOf(oDT) + 1;
                                tRownum = nRownum + ".";
                            }
                            else
                            {
                                tRownum = "";
                            }
                            //+++++++++++++++

                            //*Arm 63-04-13 พิมพ์รหัสสินค้าหรือบาร์โค้ดในแสดงสลิป 0:ไม่แสดง, 1:แสดง Product Code, 2:แสดง Barcode
                            switch (cVB.nVB_ChkShowPdtBarCode)
                            {
                                case 1:
                                    pnStartY += 18;
                                    ////poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    //poGraphic.DrawString(oDT["FTPdtCode"].ToString() + " " + oDT["FTXsdPdtName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));     //*Arm 63-05-12

                                    //*Arm 63-07-24
                                    tPdt = cPrint.C_GETxPdtLeft(oDT["FTPdtCode"].ToString(), 15) + " " + cPrint.C_GETxPdtLeft(oDT["FTXsdPdtName"].ToString(), 15);
                                    poGraphic.DrawString(tRownum + tPdt, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    //+++++++++++++
                                    break;
                                case 2:
                                    pnStartY += 18;
                                    ////poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    //poGraphic.DrawString(oDT["FTXsdBarCode"].ToString() + " " + oDT["FTXsdPdtName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));     //*Arm 63-05-12

                                    //*Arm 63-07-24
                                    tPdt = cPrint.C_GETxPdtLeft(oDT["FTXsdBarCode"].ToString(), 15) + " " + cPrint.C_GETxPdtLeft(oDT["FTXsdPdtName"].ToString(), 15);
                                    poGraphic.DrawString(tRownum + tPdt, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    //+++++++++++++
                                    break;

                                default:
                                    pnStartY += 18;
                                    ////poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    //poGraphic.DrawString(oDT["FTXsdPdtName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));     //*Arm 63-05-12

                                    //*Arm 63-07-24
                                    tPdt = cPrint.C_GETxPdtLeft(oDT["FTXsdPdtName"].ToString(), 30);
                                    poGraphic.DrawString(tRownum + tPdt, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));
                                    //+++++++++++++
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

                            //*Arm 63-05-12
                            if (Convert.ToInt32(oDT["FCXsdQty"]) > 1)
                            {
                                pnStartY += 18;
                                //poGraphic.DrawString("  " + oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                poGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)(oDT["FCXsdQty"]), cVB.nVB_DecShow) + " x " + oSP.SP_SETtDecShwSve(1, (decimal)(oDT["FCXsdSetPrice"]), cVB.nVB_DecShow), cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);    //*Em 63-05-25
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT["FCXsdAmtB4DisChg"]), cVB.nVB_DecShow);
                                poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            else
                            {
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT["FCXsdAmtB4DisChg"]), cVB.nVB_DecShow);
                                poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            //+++++++++++++

                            #region Comment
                            //if (cVB.nVB_ChkShowPdtBarCode > 0)
                            //{
                            //    pnStartY += 18;
                            //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                            //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);

                            //    ////*Em 63-04-29
                            //    //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
                            //    //{
                            //    //    pnStartY += 18;
                            //    //    poGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);

                            //    //    switch (cVB.nVB_ChkShowPdtBarCode)
                            //    //    {
                            //    //        case 1:
                            //    //            pnStartY += 18;
                            //    //            poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //            break;
                            //    //        case 2:
                            //    //            pnStartY += 18;
                            //    //            poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //            break;

                            //    //        default:
                            //    //            pnStartY += 18;
                            //    //            poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //            break;
                            //    //    }

                            //    //    pnStartY += 18;
                            //    //    poGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //    //}
                            //    //else
                            //    //{
                            //    //    pnStartY += 18;
                            //    //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //    //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //    //}
                            //    ////++++++++++++++++
                            //}
                            //else
                            //{
                            //    if (oDT.FCXsdQty > 1)
                            //    {
                            //        pnStartY += 18;
                            //        poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                            //        poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //        ////*Em 63-04-29
                            //        //if (oDT.FCXsdQty != oDT.FCXsdQtyAll)
                            //        //{
                            //        //    pnStartY += 18;
                            //        //    poGraphic.DrawString(oDT.FCXsdQtyAll + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQtyAll * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //        //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);

                            //        //    switch (cVB.nVB_ChkShowPdtBarCode)
                            //        //    {
                            //        //        case 1:
                            //        //            pnStartY += 18;
                            //        //            poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //            break;
                            //        //        case 2:
                            //        //            pnStartY += 18;
                            //        //            poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //            break;

                            //        //        default:
                            //        //            pnStartY += 18;
                            //        //            poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //            break;
                            //        //    }

                            //        //    pnStartY += 18;
                            //        //    poGraphic.DrawString((oDT.FCXsdQty - oDT.FCXsdQtyAll) + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((oDT.FCXsdQty - oDT.FCXsdQtyAll) * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //        //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //        //}
                            //        //else
                            //        //{
                            //        //    pnStartY += 18;
                            //        //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSalePrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //        //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDT.FCXsdQty * oDT.FCXsdSalePrice), cVB.nVB_DecShow);
                            //        //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //        //}
                            //        ////++++++++++++++++
                            //    }
                            //    else
                            //    {
                            //        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                            //        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdSalePrice, cVB.nVB_DecShow);    //*Em 63-05-01
                            //        poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //        poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //    }
                            //}
                            #endregion

                            //+++++++++++++
                            if (oDbTblDTDis != null)
                            {
                                DataRow[] oDR = oDbTblDTDis.Select("FNXsdSeqNo = " + (int)oDT["FNXsdSeqNo"]);    //*Em 63-06-21
                                foreach (DataRow oDTDis in oDR) // aoDTDis.Where(c => c.FNXsdSeqNo == oDT.FNXsdSeqNo))
                                {
                                    pnStartY += 18;
                                    switch (oDTDis["FTXddDisChgType"].ToString())
                                    {
                                        case "1":
                                        case "2":
                                            poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " (" + oDTDis["FTXddDisChgTxt"].ToString() + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((decimal)(oDTDis["FCXddNet"]) - (decimal)(oDTDis["FCXddValue"])), cVB.nVB_DecShow);
                                            //poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                            poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                            break;
                                        case "3":
                                        case "4":
                                            poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tChg") + " (" + oDTDis["FTXddDisChgTxt"].ToString() + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)((decimal)(oDTDis["FCXddNet"]) + (decimal)(oDTDis["FCXddValue"])), cVB.nVB_DecShow);
                                            //poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                            poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }// End 2 :ไม่อนุญาตให้รวมรายการสินค้าตอนพิมพ์


                //*Em 63-03-29 ส่วนลดโปรโมชั่น
                //oSql.Clear();

                if (oDbTblPD != null && oDbTblPD.Rows.Count > 0)
                {
                    foreach (DataRow oPD in oDbTblPD.Rows)
                    {
                        pnStartY += 18;
                        //poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " " + oPD["FTPmdGrpName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 100, 18));

                        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oPD["FCXpdDis"]), cVB.nVB_DecShow);
                        ////poGraphic.DrawString("-" + tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 100, pnStartY, 90, 18), oFormatFar);
                        //poGraphic.DrawString("-" + tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar); //*Arm 63-05-12

                        //*Em 63-08-25
                        if (Convert.ToDecimal(oPD["FCXpdDis"]) < Convert.ToDecimal(0))
                        {
                            poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tChg") + " " + oPD["FTPmdGrpName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 100, 18));
                        }
                        else
                        {
                            poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " " + oPD["FTPmdGrpName"].ToString(), cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 100, 18));
                        }

                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oPD["FCXpdDis"]) * (-1), cVB.nVB_DecShow);
                        poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                        //+++++++++++++
                    }
                }
                //+++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRNxPrintDT : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        private static void C_PRCxPrintVoldBill()
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
                oDoc.PrintPage += C_PRNxVoidBill;
                oDoc.Print();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRCxPrintVoldBill : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                //oSP.SP_CLExMemory();
            }
        }
        private static void C_PRNxVoidBill(object sender, PrintPageEventArgs e)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine, tDeposit;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tAmt;
            decimal cChange = 0;
            Image oLogo;
            try
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Start GETDATA ", cVB.bVB_AlwPrnLog);
                C_GETxDataPrint();
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : End GETDATA", cVB.bVB_AlwPrnLog);

                oDbTblHD = oDbSetPri.Tables[3]; //*Net 63-06-17

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

                oGraphic = e.Graphics;
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

                // Get ค่า ID เครื่อง จาก DB
                //oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                //nStartY += 15;
                //oGraphic.DrawString(Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm") + " BNO:" + cVB.tVB_DocNo,
                //                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                if (oDbTblHD != null && oDbTblHD.Rows.Count > 0)
                {
                    //*Net 63-06-17 พิมพ์ User ด้วย UsrCode ของบิล
                    oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + oDbTblHD.Rows[0].Field<string>("FTUsrCode") + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                    nStartY += 18;
                    //*Net 63-06-17 พิมพ์วันที่โดยใช้ Docdate
                    string tFullDate = oDbTblHD.Rows[0].Field<string>("FDXshDocDate");
                    string tDate = DateTime.ParseExact(tFullDate, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm");
                    oGraphic.DrawString(tDate + " BNO:" + cVB.tVB_DocNoPrn,
                                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                }

                //Print DT
                cVB.tVB_DocNoPrn = cVB.tVB_DocNo;       //*Net 63-03-28 ยกมาจาก baseline
                C_PRNxPrintDT(ref oGraphic, ref nStartY, nWidth);
                nStartY += 30;

                //ยกเลิกบิล
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCS_VoidBill"), cVB.aoVB_PInvLayout[8], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15),
                                          new StringFormat
                                          {
                                              FormatFlags = StringFormatFlags.NoWrap,
                                              Trimming = StringTrimming.EllipsisCharacter,
                                              Alignment = StringAlignment.Center
                                          });

                nStartY += 30;
                oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRNxVoidBill : " + oEx.Message); }
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

        private static void C_PRCxPrintHoldBill()
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
                oDoc.PrintPage += C_PRNxHoldBill;
                oDoc.Print();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRCxPrintHoldBill : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                //oSP.SP_CLExMemory();
            }
        }
        private static void C_PRNxHoldBill(object sender, PrintPageEventArgs e)
        {
            Graphics oGraphic = null;
            StringFormat oFormatFar = null, oFormatCenter = null;
            cSlipMsg oMsg;
            string tLine, tDeposit;
            int nStartY = 0, nWidth;
            cSP oSP = new cSP();
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tAmt;
            decimal cChange = 0;
            Image oLogo;
            try
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : Start GETDATA ", cVB.bVB_AlwPrnLog);
                C_GETxDataPrint();
                new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : End GETDATA", cVB.bVB_AlwPrnLog);

                oDbTblHD = oDbSetPri.Tables[3];//*Net 63-06-17

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

                oGraphic = e.Graphics;
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

                // Get ค่า ID เครื่อง จาก DB
                //oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                //nStartY += 15;
                //oGraphic.DrawString(Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm") + " BNO:" + cVB.tVB_DocNo,
                //                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                if (oDbTblHD != null && oDbTblHD.Rows.Count > 0)
                {
                    //*Net 63-06-17 พิมพ์ User ด้วย UsrCode ของบิล
                    oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + oDbTblHD.Rows[0].Field<string>("FTUsrCode") + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                    nStartY += 18;
                    //*Net 63-06-17 พิมพ์วันที่โดยใช้ Docdate
                    string tFullDate = oDbTblHD.Rows[0].Field<string>("FDXshDocDate");
                    string tDate = DateTime.ParseExact(tFullDate, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm");
                    oGraphic.DrawString(tDate + " BNO:" + cVB.tVB_DocNoPrn,
                                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                }

                //Print DT
                cVB.tVB_DocNoPrn = cVB.tVB_DocNo;       //*Net 63-03-28 ยกมาจาก baseline
                C_PRNxPrintDT(ref oGraphic, ref nStartY, nWidth);
                nStartY += 30;

                //ยกเลิกบิล
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCS_HoldBill"), cVB.aoVB_PInvLayout[8], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15),
                                          new StringFormat
                                          {
                                              FormatFlags = StringFormatFlags.NoWrap,
                                              Trimming = StringTrimming.EllipsisCharacter,
                                              Alignment = StringAlignment.Center
                                          });
                //*Arm 63-09-13
                nStartY += 18;
                oGraphic.DrawString(cVB.oVB_GBResource.GetString("tReturnBillCode")+" : " + oDbTblHD.Rows[0].Field<string>("FTXshDisChgTxt"), cVB.aoVB_PInvLayout[8], Brushes.Black, new RectangleF(0, nStartY, nWidth, 15),
                                          new StringFormat
                                          {
                                              FormatFlags = StringFormatFlags.NoWrap,
                                              Trimming = StringTrimming.EllipsisCharacter,
                                              Alignment = StringAlignment.Center
                                          });
                //++++++++++++

                nStartY += 30;
                
                oMsg.C_GETnSlipMsg("2", oGraphic, nWidth, nStartY); // Footer Slip Msg

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRNxHoldBill : " + oEx.Message); }
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

        public static void C_PRNxBarcode(ref Graphics poGraphic, string ptStrBar, int pnWidht, int pnY)
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
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        public static void C_PRNxQRCode(ref Graphics poGraphic, string ptStrBar, int pnWidth, int pnY)
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
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        //*Net 63-04-10
        public static decimal C_GETcSalDTSeqPrice(int pnSeq)
        {
            StringBuilder oSql;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FCXsdNet FROM {tC_TblSalDT}");
                oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                oSql.AppendLine($"AND FNXsdSeqNo = {pnSeq} ");

                //oDB.C_SETxDataQuery(oSql.ToString());
                return new cDatabase().C_GETaDataQuery<decimal>(oSql.ToString()).FirstOrDefault();

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_GETcSalDTSeqPrice : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return 0m;
        }

        //*Net 63-04-10
        public static decimal C_GETcSalDTPriceEditQty(int pnSeq)
        {
            StringBuilder oSql;
            decimal cSetPrice;
            List<cmlTPSTSalDTDis> aoDisChg; //*Net 63-04-10
            try
            {
                oSql = new StringBuilder();
                //*Net 63-04-10 เอาราคาก่อนลดออกมา
                oSql.AppendLine($"SELECT FCXsdSetPrice FROM {tC_TblSalDT}");
                oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");

                //oDB.C_SETxDataQuery(oSql.ToString());
                cSetPrice = new cDatabase().C_GETaDataQuery<decimal>(oSql.ToString()).FirstOrDefault();
                cSetPrice *= cSale.cC_DTQty;

                //*Net 63-04-10 คำนวนส่วนลดใหม่ +++++++++++++++++++++++++++++++
                oSql.Clear();
                oSql.AppendLine("SELECT FTBchCode ");
                oSql.AppendLine(",FTXshDocNo");
                oSql.AppendLine(",FNXsdSeqNo");
                oSql.AppendLine(",CONVERT(varchar,FDXddDateIns,120) AS FDXddDateIns");
                oSql.AppendLine(",FNXddStaDis");
                oSql.AppendLine(",FTXddDisChgTxt");
                oSql.AppendLine(",FTXddDisChgType");
                oSql.AppendLine(",FCXddNet");
                oSql.AppendLine(",FCXddValue ");
                oSql.AppendLine("FROM " + tC_TblSalDTDis + " WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXsdSeqNo = " + nC_DTSeqNo);

                aoDisChg = new cDatabase().C_GETaDataQuery<cmlTPSTSalDTDis>(oSql.ToString());

                foreach (cmlTPSTSalDTDis oDisChg in aoDisChg)
                {
                    switch (oDisChg.FTXddDisChgType)
                    {
                        case "1":
                            cSetPrice -= Convert.ToDecimal(oDisChg.FTXddDisChgTxt);
                            break;
                        case "2":
                            cSetPrice -= cSetPrice * (Convert.ToDecimal(oDisChg.FTXddDisChgTxt.Replace("%", "")) / 100.00M);
                            break;
                        case "3":
                            cSetPrice += Convert.ToDecimal(oDisChg.FTXddDisChgTxt);
                            break;
                        case "4":
                            cSetPrice += cSetPrice * (Convert.ToDecimal(oDisChg.FTXddDisChgTxt.Replace("%", "")) / 100.00M);
                            break;
                    }
                }

                //oDB.C_SETxDataQuery(oSql.ToString());
                return cSetPrice;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_GETcSalDTSeqPrice : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return 0m;
        }
        #endregion Print

        public static void C_PRCxClearTmpRef()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            try
            {
                //*Arm 63-09-17 Clear Temp บิลอ้างอิง
                oSql.AppendLine("TRUNCATE TABLE " + tC_TblRefund);
                oSql.AppendLine("TRUNCATE TABLE TPSTSalHDTmp");
                oSql.AppendLine("TRUNCATE TABLE TPSTSalHDCstTmp");
                oSql.AppendLine("TRUNCATE TABLE TPSTSalHDDisTmp");
                oSql.AppendLine("TRUNCATE TABLE TPSTSalDTTmp");
                oSql.AppendLine("TRUNCATE TABLE TPSTSalDTDisTmp");
                oSql.AppendLine("TRUNCATE TABLE TPSTSalPDTmp");
                oSql.AppendLine("TRUNCATE TABLE TPSTSalRDTmp");
                oSql.AppendLine("TRUNCATE TABLE TPSTSalRCTmp");
                oSql.AppendLine("TRUNCATE TABLE TCNTMemTxnSaleTmp");
                oSql.AppendLine("TRUNCATE TABLE TCNTMemTxnRedeemTmp");
                //+++++++++++++
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxClearTmpRef : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
            }
        }
    }
}
