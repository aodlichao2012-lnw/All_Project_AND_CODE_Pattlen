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
using AdaPos.Models.RabbitMQ;
using AdaPos.Models.Webservice.Required.SaleDocRefer;
using AdaPos.Models.Webservice.Respond;
using AdaPos.Models.Webservice.Respond.Base;

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
        public static string tC_SaleDocNum;
        public static string tC_TxnRefCode;     //*Arm 63-03-31
        public static string tC_TblTxnSal;  //*Arm 63-06-04
        public static string tC_TblTxnRD;   //*Arm 63-06-04

        //*Arm 63-06-01 ตารางอ้างอิง สำหรับ บิลขายอ้างบิลคืน บิลคืนอ้างบิลขาย
        public static string tC_Ref_TblSalHD;       //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblSalHDDis;    //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblSalHDCst;    //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblSalDT;       //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblSalDTDis;    //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblSalRC;       //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblSalRD;       //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblSalPD;       //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblTxnSale;     //*Arm 63-06-01 ตารางอ้างอิง
        public static string tC_Ref_TblTxnRedeem;   //*Arm 63-06-01 ตารางอ้างอิง

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

        public static bool bC_PrnCopy;          //*Net 63-02-25 ตรวจสอบการพิมพ์สำเนา
        public static bool bC_SetComplete;      //*Em 63-04-22

        public static cmlTPSTSalHD oC_SalHD;

        public static Thread oUpload;
        public static Thread oPrn;
        public static Thread oNewDoc;
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

                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("BEGIN TRY");
                //HD
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalHD + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalHD + " FROM [dbo].[TPSTSalHD] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHD') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalHD + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalHD + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalHD + " FROM TPSTSalHD WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");
                oSql.AppendLine("");

                //HDCst
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalHDCst + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalHDCst + " FROM [dbo].[TPSTSalHDCst] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDCst') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalHDCst + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalHDCst + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalHDCst + " FROM TPSTSalHDCst WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");
                oSql.AppendLine("");

                //HDDis
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalHDDis + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalHDDis + " FROM [dbo].[TPSTSalHDDis] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalHDDis') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalHDDis + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalHDDis + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalHDDis + " FROM TPSTSalHDDis WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");
                oSql.AppendLine("");

                //DT
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalDT + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalDT + " FROM [dbo].[TPSTSalDT] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDT') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalDT + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalDT + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDT + " FROM TPSTSalDT WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");
                oSql.AppendLine("");

                //DTDis
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalDTDis + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalDTDis + " FROM [dbo].[TPSTSalDTDis] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTDis') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalDTDis + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalDTDis + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDTDis + " FROM TPSTSalDTDis WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");
                oSql.AppendLine("");

                //DTPmt
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalDTPmt + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("	        SELECT TOP 0 * INTO " + tC_TblSalDTPmt + " FROM [dbo].[TPSTSalDTPmt] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalDTPmt') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalDTPmt + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalDTPmt + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalDTPmt + " FROM TPSTSalDTPmt WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");
                oSql.AppendLine("");

                //RC
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalRC + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO " + tC_TblSalRC + " FROM [dbo].[TPSTSalRC] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalRC') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalRC + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalRC + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalRC + " FROM TPSTSalRC WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");

                //RD    //*Arm 63-03-12
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalRD + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO " + tC_TblSalRD + " FROM [dbo].[TPSTSalRD] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalRD') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalRD + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalRD + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalRD + " FROM TPSTSalRD WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");

                //PD    //*Zen 63-03-24
                oSql.AppendLine("   IF OBJECT_ID('" + tC_TblSalPD + "', 'U') IS NULL BEGIN");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO " + tC_TblSalPD + " FROM [dbo].[TPSTSalPD] WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   ELSE BEGIN");
                oSql.AppendLine("	    IF (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTSalPD') <> (SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tC_TblSalPD + "') BEGIN");
                oSql.AppendLine("		    DROP TABLE " + tC_TblSalPD + "");
                oSql.AppendLine("		    SELECT TOP 0 * INTO " + tC_TblSalPD + " FROM TPSTSalPD WITH(NOLOCK)");
                oSql.AppendLine("	    END");
                oSql.AppendLine("   END");

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

                oSql.AppendLine("   COMMIT");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   ROLLBACK");
                oSql.AppendLine("END CATCH");

                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRCxCheckAndCreateTableTemp : " + oEx.Message); }
            finally
            {
                oSql = null;
                oDB = null;
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + nC_DocRuningLength + ") AS FTMax");
                oSql.AppendLine("FROM " + tTblName + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + nC_DocRuningLength));
                oSql.AppendLine("AND " + tFedDocType + " = " + pnDocType);
                //oSql.AppendLine("AND FTXshStaDoc = '1'");
                nMax = oDB.C_GEToDataQuery<int>(oSql.ToString());

                oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + nC_DocRuningLength + ") AS FTMax");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + nC_DocRuningLength));
                oSql.AppendLine("AND " + tFedDocType + " = " + pnDocType);
                //oSql.AppendLine("AND FTXshStaDoc = '1'");
                nMaxTmp = oDB.C_GEToDataQuery<int>(oSql.ToString());
                if (nMaxTmp > nMax) nMax = nMaxTmp;
                return nMax;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_GETxLastDocNum : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxGenNewDoc : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        public static void C_DATxInsHD()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("DELETE FROM " + tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + oC_SalHD.FTBchCode + "' AND FTXshDocNo = '" + oC_SalHD.FTXshDocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO " + tC_TblSalHD + " WITH(ROWLOCK)(");
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
                oSql.AppendLine("GETDATE(),'" + cVB.tVB_UsrCode + "',GETDATE(),'" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine(")");

                oDB.C_SETxDataQuery(oSql.ToString());

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxInsHD : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalHDCst + " WITH(ROWLOCK)(FTBchCode,FTXshDocNo, FTXshCardID, FTXshCstTel, FTXshCstName)");
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
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalHDCst + " WITH(ROWLOCK)(FTBchCode,FTXshDocNo, FTXshCardID, FTXshCstTel, FTXshCstName, FTXshCardNo)");
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
                new cSP().SP_CLExMemory();
            }
        }
        public static void C_DATxInsDT(cmlPdtOrder poOrder, string ptStaPdt = "1")
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("INSERT INTO " + tC_TblSalDT + " WITH(ROWLOCK) (");
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
                oSql.AppendLine("SELECT TOP 1 '" + cVB.tVB_BchCode + "' AS FTBchCode,'" + cVB.tVB_DocNo + "' AS FTXshDocNo," + cSale.nC_DTSeqNo + " AS FNXsdSeqNo, PDT.FTPdtCode, '" + poOrder.tPdtName + "' AS FTXsdPdtName, ");
                //oSql.AppendLine("PDT.FTPdtStkCode AS FTXsdStkCode, BAR.FTPunCode AS FTPunCode, UNIT.FTPunName AS FTPunName,"+ poOrder.cFactor +" AS FCXsdFactor, ");
                //oSql.AppendLine("BAR.FTPunCode AS FTPunCode, '" + poOrder.tUnit + "' AS FTPunName,'" + cVB.tVB_PriceGroup + "' AS FTPplCode," + poOrder.cFactor + " AS FCXsdFactor, ");    //*Em 62-06-26
                oSql.AppendLine("PDT.FTPunCode AS FTPunCode, '" + poOrder.tUnit + "' AS FTPunName,'" + cVB.tVB_PriceGroup + "' AS FTPplCode," + poOrder.cFactor + " AS FCXsdFactor, ");     //*Em 63-05-15
                oSql.AppendLine("'" + poOrder.tBarcode + "' AS FTXsdBarCode,'' AS FTSrnCode,PDT.FTPdtStaVat AS FTXsdVatType, '" + cVB.tVB_VatCode + "' AS FTVatCode," + cVB.cVB_VatRate + " AS FCXsdVatRate, ");
                //oSql.AppendLine("PDT.FTPdtSaleType AS FTXsdSaleType, " + poOrder.cSetPrice + " AS FCXsdSalePrice," + poOrder.cQty + " AS FCXsdQty,(" + (poOrder.cQty * poOrder.cFactor) + "* PKS.FCPdtUnitFact) AS FCXsdQtyAll," + poOrder.cSetPrice + " AS FCXsdSetPrice, ");
                oSql.AppendLine("PDT.FTPdtSaleType AS FTXsdSaleType, " + poOrder.cSetPrice + " AS FCXsdSalePrice," + poOrder.cQty + " AS FCXsdQty,(" + (poOrder.cQty * poOrder.cFactor) + "* PDT.FCPdtUnitFact) AS FCXsdQtyAll," + poOrder.cSetPrice + " AS FCXsdSetPrice, ");  //*Em 63-05-15
                oSql.AppendLine("(" + poOrder.cSetPrice * poOrder.cQty + ") AS FCXsdAmtB4DisChg, '' AS FTXsdDisChgTxt, 0 AS FCXsdDis, 0 AS FCXsdChg, (" + poOrder.cSetPrice * poOrder.cQty + ") AS FCXsdNet, ");
                oSql.AppendLine("(" + poOrder.cSetPrice * poOrder.cQty + ") AS FCXsdNetAfHD, 0 AS FCXsdVat, 0 AS FCXsdVatable, ");
                //oSql.AppendLine("FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                oSql.AppendLine("0 AS FCXsdCostIn, 0 AS FCXsdCostEx, '" + ptStaPdt + "' AS FTXsdStaPdt, " + poOrder.cQty + " AS FCXsdQtyLef, ");
                oSql.AppendLine("0 AS FCXsdQtyRfn, PDT.FTPdtStaAlwDis AS FTXsdStaAlwDis, ");
                ////oSql.AppendLine("FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet,  ");
                oSql.AppendLine("'1' AS FTPdtStaSet, '' AS FTXsdRmk, ");
                oSql.AppendLine("GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, ");
                oSql.AppendLine("GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK)");
                //oSql.AppendLine("INNER JOIN TCNMPdtBar BAR WITH(NOLOCK) ON PDT.FTPdtCode = BAR.FTPdtCode");
                //oSql.AppendLine("INNER JOIN TCNMPdtPackSize PKS WITH(NOLOCK) ON PDT.FTPdtCode = PKS.FTPdtCode AND BAR.FTPunCode = PKS.FTPunCode");
                oSql.AppendLine("FROM TPSMPdt PDT WITH(NOLOCK)");   //*Em 63-05-15
                //oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L UNIT WITH(NOLOCK) ON BAR.FTPunCode = UNIT.FTPunCode AND UNIT.FNLngID = " + cVB.nVB_Language);
                //oSql.AppendLine("WHERE PDT.FTPdtCode = '" + poOrder.tPdtCode + "' AND BAR.FTBarCode = '" + poOrder.tBarcode + "'");
                oSql.AppendLine("WHERE PDT.FTPdtCode = '" + poOrder.tPdtCode + "' AND PDT.FTBarCode = '" + poOrder.tBarcode + "'"); //*Em 63-05-15
                oSql.AppendLine("");

                oDB.C_SETxDataQuery(oSql.ToString());

                ////Update vat/vatable
                //C_DATxUpdVat();

                //Update cost
                C_DATxUpdCost();


            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxInsDT : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_DATxUpdCost : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                //Clear RD //*Arm 63-03-12
                oSql.AppendLine("DELETE FROM " + tC_TblSalRD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());

                //Clear PD
                oSql.AppendLine("DELETE FROM " + tC_TblSalPD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());

                //Clear RC
                oSql.AppendLine("DELETE FROM " + tC_TblSalRC + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());

                //Clear DT
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());

                //Clear DTDis
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalDTDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());

                //Clear DTPmt
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalDTPmt + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());

                //Clear HDCst
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalHDCst + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());

                //Clear HDDis
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalHDDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oDB.C_SETxDataQuery(oSql.ToString());

                //Clear HD
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE FROM " + tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
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
                new cSP().SP_CLExMemory();
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

                C_DATxUpdVat();
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
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdQty = " + pcQty);
                oSql.AppendLine(",FCXsdQtyAll = FCXsdFactor * " + pcQty);
                oSql.AppendLine(",FCXsdAmtB4DisChg = FCXsdSetPrice * " + pcQty);
                oSql.AppendLine(",FCXsdNet = FCXsdSetPrice * " + pcQty);
                oSql.AppendLine(",FCXsdNetAfHD = FCXsdSetPrice * " + pcQty);
                oSql.AppendLine(",FCXsdQtyLef =  " + pcQty);
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);
                //*Net 63-04-10 เอาราคาก่อนลดออกมา
                oSql.AppendLine();
                oSql.AppendLine();
                oSql.AppendLine($"SELECT FCXsdAmtB4DisChg FROM {tC_TblSalDT}");
                oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");

                //oDB.C_SETxDataQuery(oSql.ToString());
                cAmtB4DisChg = new cDatabase().C_GETaDataQuery<decimal>(oSql.ToString()).FirstOrDefault();

                if (cVB.cVB_PriceAfEditQty < 0)
                {
                    oSql.AppendLine($"DELETE {tC_TblSalDTDis}");
                    oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                    oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                    oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");

                    oSql.AppendLine("UPDATE " + tC_TblSalDT + " WITH(ROWLOCK)");
                    oSql.AppendLine("SET FCXsdDis = 0.00,");
                    oSql.AppendLine("FCXsdChg = 0.00");
                    oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                    oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                    oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");
                    new cDatabase().C_GEToDataQuery(oSql.ToString());
                }
                else
                {
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

                    oSql.Clear();
                    foreach (cmlTPSTSalDTDis oDisChg in aoDisChg)
                    {
                        oSql.AppendLine($"UPDATE {tC_TblSalDTDis}");
                        oSql.AppendLine($"SET FCXddNet={cAmtB4DisChg},");
                        switch (oDisChg.FTXddDisChgType)
                        {
                            case "1":
                                cDis = Convert.ToDecimal(oDisChg.FTXddDisChgTxt);
                                cAmtB4DisChg -= cDis;
                                break;
                            case "2":
                                cDis = cAmtB4DisChg * (Convert.ToDecimal(oDisChg.FTXddDisChgTxt.Replace("%", "")) / 100.00M);
                                cAmtB4DisChg -= cDis;
                                break;
                            case "3":
                                cChg = Convert.ToDecimal(oDisChg.FTXddDisChgTxt);
                                cAmtB4DisChg += cChg;
                                break;
                            case "4":
                                cChg = cAmtB4DisChg * (Convert.ToDecimal(oDisChg.FTXddDisChgTxt.Replace("%", "")) / 100.00M);
                                cAmtB4DisChg += cChg;
                                break;
                        }
                        cDisSum += cDis;
                        oSql.AppendLine($"FCXddValue={new cSP().SP_SETtDecShwSve(2, cDis, cVB.nVB_DecSave)}");
                        oSql.AppendLine($"WHERE FTBchCode = '{cVB.tVB_BchCode}' ");
                        oSql.AppendLine($"AND FTXshDocNo = '{cVB.tVB_DocNo}' ");
                        oSql.AppendLine($"AND FNXsdSeqNo = {nC_DTSeqNo} ");
                        oSql.AppendLine($"AND CONVERT(varchar,FDXddDateIns,120) = '{Convert.ToDateTime(oDisChg.FDXddDateIns).ToString("yyyy-MM-dd HH:mm:ss")}'");
                        oSql.AppendLine("");
                        oSql.AppendLine("");
                    }
                    new cDatabase().C_SETxDataQuery(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE " + tC_TblSalDT + " SET ");
                    oSql.AppendLine(" FCXsdDis = " + cDisSum + " ");
                    oSql.AppendLine(",FCXsdChg = " + cChgSum + " ");
                    oSql.AppendLine(",FCXsdNet = FCXsdAmtB4DisChg - " + cDisSum + " + " + cChgSum + " ");
                    oSql.AppendLine(",FCXsdNetAfHD = FCXsdAmtB4DisChg - " + cDisSum + " + " + cChgSum + " ");
                    oSql.AppendLine("WHERE FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FNXsdSeqNo = " + nC_DTSeqNo + "");
                    new cDatabase().C_SETxDataQuery(oSql.ToString());
                }
                C_DATxUpdVat();
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
                new cSP().SP_CLExMemory();
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

                C_DATxUpdVat();
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
                new cSP().SP_CLExMemory();
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
                C_DATxUpdVat();
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
                    if (odtPromo != null && odtPromo.Rows.Count > 0 && Convert.ToInt32(odtPromo.Rows[0]["FCXshTotal"]) > 0)
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
                    oSql.AppendLine(",FCXshAmtV = " + oC_SalHD.FCXshAmtV);
                    oSql.AppendLine(",FCXshAmtNV = " + oC_SalHD.FCXshAmtNV);
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
                new cSP().SP_CLExMemory();
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
                // *Zen 63-03-25 
                oSql.AppendLine("    INSERT INTO TPSTSalPD WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT PD.*");
                oSql.AppendLine("    FROM " + tC_TblSalPD + " PD WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON PD.FTBchCode = HD.FTBchCode AND PD.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                //++++++++++++++
                // *Arm 63-03-13 
                oSql.AppendLine("    INSERT INTO TPSTSalRD WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT RD.*");
                oSql.AppendLine("    FROM " + tC_TblSalRD + " RD WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON RD.FTBchCode = HD.FTBchCode AND RD.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                //++++++++++++++
                oSql.AppendLine("    INSERT INTO TPSTSalRC WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT RC.*");
                oSql.AppendLine("    FROM " + tC_TblSalRC + " RC WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON RC.FTBchCode = HD.FTBchCode AND RC.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalDT WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT DT.*");
                oSql.AppendLine("    FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DT.FTBchCode = HD.FTBchCode AND DT.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalDTDis WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT DTDis.*");
                oSql.AppendLine("    FROM " + tC_TblSalDTDis + " DTDis WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DTDis.FTBchCode = HD.FTBchCode AND DTDis.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalDTPmt WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT DTPmt.*");
                oSql.AppendLine("    FROM " + tC_TblSalDTPmt + " DTPmt WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DTPmt.FTBchCode = HD.FTBchCode AND DTPmt.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalHDDis WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT HDDis.*");
                oSql.AppendLine("    FROM " + tC_TblSalHDDis + " HDDis WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON HDDis.FTBchCode = HD.FTBchCode AND HDDis.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalHDCst WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT HDCst.*");
                oSql.AppendLine("    FROM " + tC_TblSalHDCst + " HDCst WITH(NOLOCK)");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON HDCst.FTBchCode = HD.FTBchCode AND HDCst.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
                oSql.AppendLine("");
                oSql.AppendLine("    INSERT INTO TPSTSalHD WITH(ROWLOCK)");
                oSql.AppendLine("    SELECT *");
                oSql.AppendLine("    FROM " + tC_TblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("    WHERE ISNULL(FTXshStaDoc, '') = '1'");
                oSql.AppendLine("");
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
                oSql.AppendLine("    DELETE DTPmt WITH(ROWLOCK)");
                oSql.AppendLine("    FROM " + tC_TblSalDTPmt + " DTPmt");
                oSql.AppendLine("    INNER JOIN " + tC_TblSalHD + " HD WITH(NOLOCK) ON DTPmt.FTBchCode = HD.FTBchCode AND DTPmt.FTXshDocNo = HD.FTXshDocNo");
                oSql.AppendLine("    WHERE ISNULL(HD.FTXshStaDoc,'') = '1'");
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
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("    ROLLBACK TRAN");
                oSql.AppendLine("END CATCH");
                oDB.C_SETxDataQuery(oSql.ToString());

                //*Em 63-05-16
                oUpload = new Thread(new cSyncData().C_PRCxSyncUld);
                oUpload.IsBackground = true;
                oUpload.Priority = ThreadPriority.Highest;
                oUpload.Start();
                //++++++++++++++++++
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxTemp2Transaction : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("INSERT INTO TPSTVoidDT WITH(ROWLOCK)( ");
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
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("INSERT INTO TPSTVoidDT WITH(ROWLOCK)(FTBchCode, FNVidNo, FNXidSeqNo, FTVidType, FTRsnCode, FTXihDocNo, FTXihDocType, FDXihDocDate, FTXihDocTime, FTPdtCode,");
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
                oSql.AppendLine("INSERT INTO TPSTVoidDTDis WITH(ROWLOCK)(FTBchCode, FNVidNo, FNXidSeqNo, FDXddDateIns, FTXihDocNo, FTXddDisChgTxt, FNXddStaDis, FTXddDisChgType, FCXddNet, FCXddValue)");
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

                C_PRCxPrintVoldBill(); //*Arm 63-03-05 [Comment code] Update ตาม Baseline

                ////*Em 63-03-02 , *Arm 63-03-05 Update ตาม Baseline
                //Thread oPrn = new Thread(new ThreadStart(C_PRCxPrintVoldBill));
                //oPrn.Start();
                ////+++++++++++++
                nC_LastDocVoid = nC_LastDocVoid + 1;  //*Em 63-05-15
                C_PRCxAdd2TmpLogChg(82, nLastSeq.ToString()); //*Em 62-08-23
                new cSyncData().C_PRCxSyncUld();    //*Em 62-08-24


                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSale);
                //new wSale(3).Show();


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
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("INSERT INTO TPSTVoidDT WITH(ROWLOCK)(FTBchCode, FNVidNo, FNXidSeqNo, FTVidType, FTRsnCode, FTXihDocNo, FTXihDocType, FDXihDocDate, FTXihDocTime, FTPdtCode,");
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
                oSql.AppendLine("");
                oSql.AppendLine("INSERT INTO TPSTVoidDTDis WITH(ROWLOCK)(FTBchCode, FNVidNo, FNXidSeqNo, FDXddDateIns, FTXihDocNo, FTXddDisChgTxt, FNXddStaDis, FTXddDisChgType, FCXddNet, FCXddValue)");
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
                oSql.AppendLine("INSERT INTO " + tC_TblSalDT + " WITH(ROWLOCK) (");
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
                oSql.AppendLine("FCXsdSetPrice AS cSetPrice, '4' AS tStaPdt");
                oSql.AppendLine("FROM " + tC_TblSalDT + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "' ");
                oSql.AppendLine("AND FNXsdSeqNo = " + nC_DTSeqNo);
                cmlPdtOrder oPdtOrder = oDB.C_GEToDataQuery<cmlPdtOrder>(oSql.ToString());
                cVB.oVB_Sale.W_ADDxPdtToOrder(oPdtOrder);
                nC_LastDocVoid = nC_LastDocVoid + 1;  //*Em 63-05-15
                C_PRCxAdd2TmpLogChg(82, nLastSeq.ToString()); //*Em 62-08-23
                new cSyncData().C_PRCxSyncUld();    //*Em 62-08-24
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
                new cSP().SP_CLExMemory();
            }
        }

        public static void C_PRCxSetComplete()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            Form oFormShow = null;
            string tTblSalHD = ""; //*Arm 63-06-01
            string tTblSalDT = ""; //*Arm 63-06-01
            try
            {
                C_PRCxSummary2HD();
                cVB.dVB_TimeStamp = DateTime.Now;  //*Net 63-05-18 - มอนิเตอร์วันที่บิล
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FTXshStaDoc = '1'");
                oSql.AppendLine(",FTXshStaPaid = '3'");
                oSql.AppendLine(",FCXshGrand = " + (decimal)(oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff));
                oSql.AppendLine(",FCXshPaid = " + (decimal)(oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff));  //*Net 63-03-28 ยกมาจาก baseline
                oSql.AppendLine(",FCXshRnd = " + cVB.cVB_RoundDiff);
                oSql.AppendLine(",FTXshGndText = '" + (cVB.nVB_Language == 1 ? C_GETtGndTextTH(((decimal)oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff).ToString()) : C_GETtGndTextEN((decimal)oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff).ToString()) + "'");
                oSql.AppendLine(",FDLastUpdOn = '" + cVB.dVB_TimeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                oSql.AppendLine(",FNXshDocPrint = '1'");
                cVB.cVB_MonitorSalGrand = (decimal)(oC_SalHD.FCXshGrand + cVB.cVB_RoundDiff); //*Net 63-05-18 - มอนิเตอร์ยอดขายต่อบิล

                ////*Arm 63-02-19
                //if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                //{
                //    if (nC_DocType != 9)
                //    {
                //        oSql.AppendLine(",FTXshRefExt = '" + cVB.tVB_RefDocNo + "'");   //*Arm 63-03-05 - เลขที่เอกสาร SO
                //    }
                //}
                ////+++++++++++++++

                //*Arm 63-06-04
                if (nC_DocType != 9 && !string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                {
                    if (cVB.oVB_ReferSO != null)
                    {
                        oSql.AppendLine(",FTXshRefExt = '" + cVB.tVB_RefDocNo + "'");  //อ้างอิงเลขที่เอกสาร SO
                    }
                    else
                    {
                        oSql.AppendLine(",FTXshRefInt = '" + cVB.tVB_RefDocNo + "'"); //อ้างอิงเลขที่เอกสารบิลคืน
                    }
                }
                //+++++++++++++++
                
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND FNXshDocType = " + oC_SalHD.FNXshDocType);
                oDB.C_SETxDataQuery(oSql.ToString());

                ////*Arm 63-04-16 คำนวณแต้ม
                //if (nC_DocType != 9 && !string.IsNullOrEmpty(cVB.tVB_CstCode))
                //{
                //    C_PRCxSalePoint();
                //}
                //// ++++++++++++
                
                //if (nC_DocType == 9)
                //{
                //    if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                //    {
                //        oSql = new StringBuilder();
                //        oSql.AppendLine("UPDATE " + cSale.tC_TblSalHD + " WITH(ROWLOCK)");
                //        oSql.AppendLine("SET FTXshRefInt = '" + cVB.tVB_RefDocNo + "'");
                //        oSql.AppendLine(",FDXshRefIntDate = GETDATE()");
                //        oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                //        oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //        oSql.AppendLine("AND FNXshDocType = " + oC_SalHD.FNXshDocType);
                //        oDB.C_SETxDataQuery(oSql.ToString());

                //        //oSql = new StringBuilder();
                //        //oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK)");
                //        //oSql.AppendLine("SET FTXshStaRefund = '2'");
                //        //oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                //        //oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                //        //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        //oDB.C_SETxDataQuery(oSql.ToString());

                //        ////*Arm 62-12-19 UPDATE Qty
                //        ////C_UPDxUpdateQty();
                //        //if (cVB.nVB_ReturnType == 3) C_UPDxUpdateQty();  //*Em 63-05-15

                //        //*Arm 63-06-01
                //        if (cVB.bVB_RefundDataFrom == true)
                //        {
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("UPDATE TPSTSalHD WITH(ROWLOCK)");
                //            oSql.AppendLine("SET FTXshStaRefund = '2'");
                //            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                //            oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                //            oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //            oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //            oDB.C_SETxDataQuery(oSql.ToString());

                //            //UPDATE Qty
                //            if (cVB.nVB_ReturnType == 3) C_UPDxUpdateQty();  //*Em 63-05-15
                //        }
                //        else
                //        {
                //            //Thread Cal API2ARDoc
                //        }
                //        //+++++++++++++

                //        cPayment.C_PRCbCancelCoupon(2, cVB.tVB_RefDocNo);   //*63-01-09

                //        //*Arm 63-03-21  คืนแต้ม
                //        if (!string.IsNullOrEmpty(cVB.tVB_CstCode))
                //        {
                //            C_PRCxReturnSalePoint();
                //        }
                //        //++++++++++++++

                //        C_PRCxVoidRefun(); //*Arm 63-03-24

                //        cVB.cVB_MonitorSalGrand *= (-1); //*Net 63-05-18 - มอนิเตอร์ยอดขายต่อบิล

                //    }

                //}


                //*Arm 63-06-01
                if(nC_DocType == 9)
                {
                    if (!string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                    {
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

                        //# กรณีมีการอ้างอิงบิลคืน Update บิลคืนที่อ้างอิง StaRef = 2 (*Arm 63-06-01)
                        if (cVB.bVB_RefundDataFrom == true)
                        {
                            
                            // Update บิลคืนที่อ้างอิง FTXshStaRefund = 2
                            oSql.Clear();
                            oSql.AppendLine("UPDATE "+ tC_Ref_TblSalHD );
                            oSql.AppendLine("SET FTXshStaRefund = '2'");
                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                            oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                            oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                            oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                            if (cVB.nVB_ReturnType == 3) C_UPDxUpdateQty();  //*Em 63-05-15 //*Arm 63-06-01 เพิ่มส่ง Parameter tTblSalDT
                            else C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, true);
                        }
                        else
                        {
                            //Thread Cal API2ARDoc
                            Thread oUpdSalRefer = new Thread(C_PRCxUploadUpdateSaleRefer);
                            oUpdSalRefer.IsBackground = true;
                            oUpdSalRefer.Priority = ThreadPriority.Highest;
                            oUpdSalRefer.Start();
                        }
                        //# End กรณีมีการอ้างอิงบิลคืน Update บิลคืนที่อ้างอิง StaRef = 2 (*Arm 63-06-01)

                        cPayment.C_PRCbCancelCoupon(2, cVB.tVB_RefDocNo);   //*63-01-09
                        
                        //if (!string.IsNullOrEmpty(cVB.tVB_CstCode))C_PRCxReturnSalePoint(); //*Arm 63-03-21  คืนแต้ม

                        C_PRCxVoidRefun(); //*Arm 63-03-24

                        cVB.cVB_MonitorSalGrand *= (-1); //*Net 63-05-18 - มอนิเตอร์ยอดขายต่อบิล
                    }
                }
                else
                {
                    //# คำนวณแต้ม
                    new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxSalePoint Start");
                    if (!string.IsNullOrEmpty(cVB.tVB_CstCode)) C_PRCxSalePoint(); //คำนวณแต้ม
                    new cLog().C_WRTxLog("cSale", "C_PRCxSetComplete : C_PRCxSalePoint End");
                    
                    //# กรณีมีการอ้างอิงบิลคืน Update บิลคืนที่อ้างอิง StaRef = 2 (*Arm 63-06-01)
                    if (cVB.oVB_ReferSO == null && !string.IsNullOrEmpty(cVB.tVB_RefDocNo))
                    {
                        if (cVB.bVB_RefundDataFrom == true) //bVB_RefundDataFrom = true:ข้อมูลบิลคืนอ้างอิงจากเครื่อง, false:ข้อมูลบิลคืนอ้างอิงจากหลัง(API2ARDoc)
                        {
                            // Update บิลคืนที่อ้างอิง StaRef = 2
                            oSql = new StringBuilder();
                            oSql.AppendLine("UPDATE "+ tC_Ref_TblSalHD);
                            oSql.AppendLine("SET FNXshStaRef = '2'");
                            oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                            oSql.AppendLine(",FTLastUpdBy = '" + cVB.tVB_UsrCode + "'");
                            oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                            oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                            C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, true);

                        }
                        else
                        {
                            //Thread Cal API2ARDoc
                            Thread oUpdSalRefer = new Thread(C_PRCxUploadUpdateSaleRefer);
                            oUpdSalRefer.IsBackground = true;
                            oUpdSalRefer.Priority = ThreadPriority.Highest;
                            oUpdSalRefer.Start();
                            
                        }
                        
                    } //# End กรณีมีการอ้างอิงบิลคืน Update บิลคืนที่อ้างอิง StaRef = 2 (*Arm 63-06-01)
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
                C_PRCxPrintSlip();
                //oPrn = new Thread(C_PRCxPrintSlip);
                //oPrn.IsBackground = true;
                //oPrn.Priority = ThreadPriority.Highest;
                //oPrn.Start();
                //+++++++++++++++++

                C_OPNxCashDrawer(); //*Em 62-08-16
                //C_PRCxAdd2TmpLogChg(80, cVB.tVB_DocNo, true);     //*Em 62-08-05, Arm 63-03-05 - เพิ่ม ส่ง parameter true 
                C_PRCxAdd2TmpLogChg(80, cVB.tVB_DocNo, false);      //*Arm 63-05-17
                //C_PRCxTemp2Transaction();
                //new cSyncData().C_PRCxSyncUld();    //*Em 62-08-05

                //*Em 63-05-16
                Thread oTran = new Thread(C_PRCxTemp2Transaction);
                oTran.IsBackground = true;
                oTran.Priority = ThreadPriority.Highest;
                oTran.Start();
                //++++++++++++++++++++

                ////*Em 63-04-17
                //oUpload = new Thread(new cSyncData().C_PRCxSyncUld);
                //oUpload.IsBackground = true;
                //oUpload.Priority = ThreadPriority.Highest;
                //oUpload.Start();
                ////++++++++++++++++

                //*Arm 62-10-27 - Update Queue Member
                if (!string.IsNullOrEmpty(cVB.tVB_QMemMsgID))
                {
                    new cMsgQueue().C_UPDxPrcMsg();
                }

                ////*Arm 63-03-31 ClrarPara
                cVB.aoVB_PdtRdDocType1 = null;  //*Arm 63-03-31
                cVB.aoVB_PdtRdDocType2 = null;  //*Arm 63-03-31
                cVB.aoVB_PdtRefund = null;  //*Arm 63-03-31
                cVB.aoVB_PdtReferSO = null; //*Arm 63-03-31
                cVB.aoVB_PdtDisChgRefund = null; //*Arm 63-03-31

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
                cVB.oVB_Sale.W_SETxNewDoc();    //*Arm 63-03-05

                cVB.oVB_Sale.bW_Activate = false;
                cVB.oVB_Sale.Show();

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
            }
            finally
            {
                oDB = null;
                oSql = null;
                oFormShow = null;
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
            }

        }

        /// <summary>
        /// Upload Update Sale Refer
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
                new cSP().SP_CLExMemory();
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

            decimal cPointUsed = 0; // Point ที่ใช้
            decimal cPointRcv = 0;  // point ที่รับ
            decimal cSumPoint = 0;  // Point Active
            decimal cSumGrand = 0;  // Amount total
            decimal cSumPointPmt = 0; // *Arm 63-04-14 Point Promotion 
            string tRefDoc = "";

            try
            {
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT * ");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB = new cDatabase();
                oHD = oDB.C_GEToDataQuery<cmlTPSTSalHD>(oSql.ToString());

                // DocNo
                tRefDoc = oHD.FTXshDocNo;
                tC_TxnRefCode = tRefDoc; //*Arm 63-03-31
                // Amount total
                cSumGrand = Convert.ToDecimal(oHD.FCXshGrand);
                // Point ที่ใช้ 
                cPointUsed = cVB.nVB_CstPiontB4Used - cVB.nVB_CstPoint;
                // Point ที่ได้รับ
                cPointRcv = (Math.Floor(Convert.ToDecimal(oHD.FCXshGrand) / cVB.cVB_PntOptBuyAmt) * cVB.cVB_PntOptGetQty);
                // Point Active = point ก่อนใช้ + Point ที่ได้รับ - Point ที่ใช้
                cSumPoint = cVB.nVB_CstPiontB4Used + cPointRcv - cPointUsed;


                //*Arm 63-04-15  point ที่ได้รับ promotion จาก PD
                //========================================================================

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


                // #Process TxnSale
                // **************************************
                oSql.Clear();
                oSql.AppendLine("INSERT INTO TCNTMemTxnSale WITH(ROWLOCK) (");
                oSql.AppendLine("FTCgpCode, FTMemCode, FTTxnRefDoc, FTTxnRefInt, FTTxnRefSpl");
                oSql.AppendLine(", FDTxnRefDate, FCTxnRefGrand, FCTxnPntOptBuyAmt, FCTxnPntOptGetQty, FCTxnPntB4Bill");
                oSql.AppendLine(", FDTxnPntStart, FDTxnPntExpired, FCTxnPntBillQty, FCTxnPntUsed, FCTxnPntExpired");
                oSql.AppendLine(", FTTxnPntStaClosed, FTTxnPntDocType, FTTxnStaSend"); //*Arm 63-04-15 เพิ่ม FTTxnStaSend
                oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES (");
                oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + oHD.FTCstCode + "', '" + tRefDoc + "', '', '' "); //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "', '" + cSumGrand + "', '" + cVB.cVB_PntOptBuyAmt + "', '" + cVB.cVB_PntOptGetQty + "', '" + (cVB.nVB_CstPiontB4Used - cPointUsed) + "' ");  //*Arm 63-04-29

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


                if (cPointUsed > 0)
                {

                    // #Process TxnRedeem
                    // **************************************

                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNTMemTxnRedeem WITH(ROWLOCK) (");
                    oSql.AppendLine("FTCgpCode, FTMemCode, FTRedRefDoc, FTRedRefSpl, FTRedRefInt, FTRedPntDocType"); //*Arm 63-03-21 FTRedRefInt , //*Arm 63-04-01 เพิ่ม FTRedPntDocType
                    oSql.AppendLine(", FDRedRefDate, FCRedPntB4Bill, FCRedPntBillQty, FTRedPntStaClosed");
                    oSql.AppendLine(", FDRedPntStart, FDRedPntExpired, FTRedStaSend "); //*Arm 63-04-15 เพิ่ม FTRedStaSend
                    oSql.AppendLine(", FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES (");
                    oSql.AppendLine("'" + cVB.tVB_MemCgpCode + "', '" + oHD.FTCstCode + "', '" + tRefDoc + "', '','', '1' ");  //*Arm 63-03-21 FTRedRefInt = '', //*Arm 63-03-30 เพิ่ม cVB.tVB_MemCgpCode
                    oSql.AppendLine(",'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(oHD.FDXshDocDate)) + "', '" + cVB.nVB_CstPiontB4Used + "', '" + cPointUsed + "', '1' ");

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

                oSql.Clear();
                oSql.AppendLine("UPDATE " + cSale.tC_TblSalHDCst + " with(rowlock) SET");
                oSql.AppendLine("FCXshCstPnt = '" + (cPointRcv - cPointUsed) + "' ");
                oSql.AppendLine(", FCXshCstPntPmt = '" + cSumPointPmt + "'");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oDB.C_SETxDataQuery(oSql.ToString());

                //========================================================================



                //*Arm 63-04-14 กวาดข้อมูล Transaction ที่ยังไม่ถูกส่ง ส่งไป Center และ Update Status
                //========================================================================


                // 1.ส่ง Transation Redeem
                oSql.Clear();
                oSql.AppendLine("SELECT FTRedRefDoc FROM TCNTMemTxnRedeem with(nolock)");
                oSql.AppendLine("WHERE ISNULL(FTRedStaSend,'1') != '2' ");
                oSql.AppendLine("ORDER BY FDRedRefDate ASC ");
                List<cmlTCNTMemTxnRedeem> aoSendTxnRdm = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                if (aoSendTxnRdm.Count > 0)
                {
                    foreach (cmlTCNTMemTxnRedeem oData in aoSendTxnRdm)
                    {
                        if (C_PRCbPublishTxn2AdaMember("2", oData.FTRedRefDoc) == true)
                        {
                            oSql.Clear();
                            oSql.AppendLine("UPDATE TCNTMemTxnRedeem with(rowlock) SET");
                            oSql.AppendLine("FTRedStaSend = '2' ");
                            oSql.AppendLine("WHERE FTRedRefDoc = '" + oData.FTRedRefDoc + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }
                    }
                }

                // 2.ส่ง Transation Sale :
                oSql.Clear();
                oSql.AppendLine("SELECT FTTxnRefDoc FROM TCNTMemTxnSale with(nolock)");
                oSql.AppendLine("WHERE ISNULL(FTTxnStaSend,'1') != '2' ");
                oSql.AppendLine("ORDER BY FDTxnRefDate ASC ");
                List<cmlTCNTMemTxnSale> aoSendTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                if (aoSendTxnSale.Count > 0)
                {
                    foreach (cmlTCNTMemTxnSale oData in aoSendTxnSale)
                    {
                        if (C_PRCbPublishTxn2AdaMember("1", oData.FTTxnRefDoc) == true)
                        {
                            oSql.Clear();
                            oSql.AppendLine("UPDATE TCNTMemTxnSale with(rowlock) SET");
                            oSql.AppendLine("FTTxnStaSend = '2' ");
                            oSql.AppendLine("WHERE FTTxnRefDoc = '" + oData.FTTxnRefDoc + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }
                    }
                }
                //========================================================================

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxSalePoint : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                oHD = null;
                new cSP().SP_CLExMemory();

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
                        oSql.AppendLine("SELECT * FROM TCNTMemTxnSale WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTTxnRefDoc = '" + ptRefDoc + "'");
                        oTxnSale = new cmlTxnSale();
                        oTxnSale.aoTCNTMemTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());

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
                        }

                        break;
                    case "2":
                        //# TxnRedeem : Send TxnRedeemto Center
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT * FROM TCNTMemTxnRedeem WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTRedRefDoc = '" + ptRefDoc + "'");
                        oTxnRedeem = new cmlTxnRedeem();
                        oTxnRedeem.aoTCNTMemTxnRedeem = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());

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
                new cSP().SP_CLExMemory();
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
                //Arm 63-05-01
                oSql.AppendLine("SELECT FDXshDocDate FROM " + cSale.tC_TblSalHD + " with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                dDocDate = oDB.C_GEToDataQuery<DateTime>(oSql.ToString());
                //+++++++++++++


                //*Arm 63-04-04 เงื่อนไขการได้แต้มจากบิลขาย 
                oSql.Clear();
                oSql.AppendLine("SELECT  FCTxnPntOptBuyAmt, FCTxnPntOptGetQty FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + cVB.tVB_RefDocNo + "'");
                
                cmlTCNTMemTxnSale oTxnSale = oDB.C_GEToDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());

                if (oTxnSale != null)
                {
                    cPntOptBuyAmt = (decimal)oTxnSale.FCTxnPntOptBuyAmt;
                    cPntOptGetQty = (decimal)oTxnSale.FCTxnPntOptGetQty;
                }
                else
                {
                    cPntOptBuyAmt = 0;
                    cPntOptGetQty = 0;
                }


                //ยอดรวมบิลขาย
                oSql.Clear();
                oSql.AppendLine("SELECT COUNT(*) AS nRCount, SUM(FCXshGrand) AS cGetGrandRtn, ");
                oSql.AppendLine("FLOOR(ISNULL(SUM(FCXshGrand), 0.00) / " + cPntOptBuyAmt + ") * " + cPntOptGetQty + " AS cGetPoint");
                oSql.AppendLine("FROM TPSTSalHD with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                oHDSale = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());


                // ยอดบิลคืนปัจจุบัน
                oSql.Clear();
                oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FCXshGrand AS cGetGrandRtn, FLOOR(ISNULL(FCXshGrand, 0.00) / " + cPntOptBuyAmt + ") * " + cPntOptGetQty + " AS cGetPoint ");
                oSql.AppendLine("FROM " + cSale.tC_TblSalHD + " with(nolock) ");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oHDSaleRtn = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());


                //ยอดรวมคืนสะสมก่อนหน้า
                oSql.Clear();
                oSql.AppendLine("SELECT COUNT(*) AS nRCount, SUM(FCXshGrand) AS cGetGrandRtn, ");
                oSql.AppendLine("FLOOR(ISNULL(SUM(FCXshGrand), 0.00) / " + cPntOptBuyAmt + ") * " + cPntOptGetQty + " AS cGetPoint");
                oSql.AppendLine("FROM TPSTSalHD with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshRefInt = '" + cVB.tVB_RefDocNo + "'");
                oHDSaleRtnBfr = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());
                
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
                oSql.AppendLine("SELECT ISNULL(FCXshCstPntPmt,0) FROM TPSTSalHDCst with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "' ");
                cSumPointPmtSale = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                // 2.หาแต้มโปรโมชั่นที่เคยคืนก่อนหน้า
                oSql.Clear();
                oSql.AppendLine("SELECT SUM(ISNULL(HDCst.FCXshCstPntPmt,0)) FROM " + cSale.tC_TblSalHD + " HDTmp ");
                oSql.AppendLine("INNER JOIN TPSTSalHD HD with(nolock) ON HDTmp.FTBchCode = HD.FTBchCode AND HDTmp.FTXshRefInt = HD.FTXshRefInt  ");
                oSql.AppendLine("INNER JOIN TPSTSalHDCst HDCst  with(nolock) ON HD.FTBchCode = HDCst.FTBchCode AND HD.FTXshDocNo = HDCst.FTXshDocNo  ");
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
                    oSql.AppendLine("FROM TCNTMemTxnRedeem with(nolock) ");
                    oSql.AppendLine("WHERE FTMemCode = '" + cVB.tVB_CstCode + "' AND FTRedRefDoc = '" + cVB.tVB_RefDocNo + "' ");
                    cmlSalReturn oRd = oDB.C_GEToDataQuery<cmlSalReturn>(oSql.ToString());


                    // ยอดการคืนแต้มรวมก่อนหน้า
                    oSql.Clear();
                    oSql.AppendLine("SELECT COUNT(*) AS nRCount, ISNULL(SUM(FCRedPntBillQty), 0) AS cGetPoint ");
                    oSql.AppendLine("FROM TCNTMemTxnRedeem with(nolock) ");
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
                    oSql.AppendLine("INSERT INTO TCNTMemTxnRedeem WITH(ROWLOCK) (");
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
                oSql.AppendLine("INSERT INTO TCNTMemTxnSale WITH(ROWLOCK) (");
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



                //*Arm 63-04-15 กวาดข้อมูล Transaction ที่ยังไม่ถูกส่ง ส่งไป Center และ Update Status
                //========================================================================

                // 1.ส่ง Transation Redeem
                oSql.Clear();
                oSql.AppendLine("SELECT FTRedRefDoc FROM TCNTMemTxnRedeem with(nolock)");
                oSql.AppendLine("WHERE ISNULL(FTRedStaSend,'1') != '2' ");
                oSql.AppendLine("ORDER BY FDRedRefDate ASC ");
                List<cmlTCNTMemTxnRedeem> aoSendTxnRdm = oDB.C_GETaDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                if (aoSendTxnRdm.Count > 0)
                {
                    foreach (cmlTCNTMemTxnRedeem oData in aoSendTxnRdm)
                    {
                        if (C_PRCbPublishTxn2AdaMember("2", oData.FTRedRefDoc) == true)
                        {
                            oSql.Clear();
                            oSql.AppendLine("UPDATE TCNTMemTxnRedeem with(rowlock) SET");
                            oSql.AppendLine("FTRedStaSend = '2' ");
                            oSql.AppendLine("WHERE FTRedRefDoc = '" + oData.FTRedRefDoc + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }
                    }
                }

                // 2.ส่ง Transation Sale 
                oSql.Clear();
                oSql.AppendLine("SELECT FTTxnRefDoc FROM TCNTMemTxnSale with(nolock)");
                oSql.AppendLine("WHERE ISNULL(FTTxnStaSend,'1') != '2' ");
                oSql.AppendLine("ORDER BY FDTxnRefDate ASC ");
                List<cmlTCNTMemTxnSale> aoSendTxnSale = oDB.C_GETaDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());
                if (aoSendTxnSale.Count > 0)
                {
                    foreach (cmlTCNTMemTxnSale oData in aoSendTxnSale)
                    {
                        if (C_PRCbPublishTxn2AdaMember("1", oData.FTTxnRefDoc) == true)
                        {
                            oSql.Clear();
                            oSql.AppendLine("UPDATE TCNTMemTxnSale with(rowlock) SET");
                            oSql.AppendLine("FTTxnStaSend = '2' ");
                            oSql.AppendLine("WHERE FTTxnRefDoc = '" + oData.FTTxnRefDoc + "'");
                            oDB.C_SETxDataQuery(oSql.ToString());
                        }
                    }
                }
                //========================================================================


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
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Update Qty บิลคืนที่อ้าอิง
        /// </summary>
        public static void C_UPDxUpdateQty()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            int nQtyLef = 0;
            try
            {
                oSql = new StringBuilder();
                ////oSql.AppendLine("UPDATE TPSTSalDT WITH(ROWLOCK)");
                //oSql.AppendLine("UPDATE " + tC_Ref_TblSalDT + " WITH(ROWLOCK)"); //*Arm 63-06-01
                //oSql.AppendLine("SET FCXsdQtyLef = DT.FCXsdQtyLef - Rfn.FCXsdQty, ");
                //oSql.AppendLine("FCXsdQtyRfn = DT.FCXsdQtyRfn + Rfn.FCXsdQty ");
                ////oSql.AppendLine("FROM TPSTSalDT DT INNER JOIN (SELECT * FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "') Rfn ON DT.FTBchCode = Rfn.FTBchCode AND DT.FTPdtCode = Rfn.FTPdtCode AND DT.FTxsdBarCode = Rfn.FTxsdBarCode");
                ////*Em 63-05-15
                ////oSql.AppendLine("FROM TPSTSalDT DT ");
                //oSql.AppendLine("FROM " + tC_Ref_TblSalDT + " DT "); //*Arm 63-06-01
                //oSql.AppendLine("INNER JOIN (SELECT FTBchCode,FTPdtCode,FTxsdBarCode,FCXsdQty FROM " + cSale.tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "') Rfn ");
                //oSql.AppendLine("ON DT.FTBchCode = Rfn.FTBchCode AND DT.FTPdtCode = Rfn.FTPdtCode AND DT.FTxsdBarCode = Rfn.FTxsdBarCode");
                ////++++++++++++++++++
                //oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND DT.FTXsdStaPdt = '1'");
                //oDB.C_SETxDataQuery(oSql.ToString());

                //*Arm 63-06-02 Update โดยเช็คจาก  ตารางเก็บ Refund
                oSql.AppendLine("UPDATE " + tC_Ref_TblSalDT + " WITH(ROWLOCK)");
                oSql.AppendLine("SET FCXsdQtyLef = DT.FCXsdQtyLef - Rfn.FCXsdQtyLef, ");
                oSql.AppendLine("FCXsdQtyRfn = DT.FCXsdQtyRfn + Rfn.FCXsdQtyLef ");
                oSql.AppendLine("FROM " + tC_Ref_TblSalDT + " DT "); 
                oSql.AppendLine("INNER JOIN " + tC_TblRefund + " Rfn WITH(NOLOCK) ON DT.FNXsdSeqNo = Rfn.FNXsdSeqNoOld");
                oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + cVB.tVB_RefDocNo + "' AND DT.FTXsdStaPdt = '1'");
                oDB.C_SETxDataQuery(oSql.ToString());
                //+++++++++++++

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT SUM(FCXsdQtyLef) FROM " + tC_Ref_TblSalDT + " WITH(NOLOCK) WHERE FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                nQtyLef = oDB.C_GEToDataQuery<int>(oSql.ToString());

                //UPDATE FNXshStaRef in TPSTSalHD กรณีคืนสิ้นค้าหมดทั้งบิลแล้ว 
                oSql = new StringBuilder();
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

                //Sync Upload การเปลี่ยนบิลขาย g,njv,u,udki8nolbo8hk  
                C_PRCxAdd2TmpLogChg(80, cVB.tVB_RefDocNo, true);  //*Arm 62-12-25
                
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_UPDxUpdateQty() : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
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
                ptMonney = String.Format("{0:0000000000000000000.00}", ptMonney); // to set  format get 22 char
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
                new cSP().SP_CLExMemory();
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

            try
            {
                //if (new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tFuncError"), 4) )


                if (cSale.nC_CntItem == 0)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgNoItem"), 1);
                    return;
                }
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : Start");
                if (MessageBox.Show(cVB.oVB_GBResource.GetString("tMsgHoldBill"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : C_PRCxSummary2HD");
                C_PRCxSummary2HD();
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : nLastSeq");
                int nLastSeq = 0;
                oSql.AppendLine("SELECT MAX(FNHldNo) AS FNHldNo FROM TPSTHoldHD WITH(NOLOCK)");
                nLastSeq = oDB.C_GEToDataQuery<int>(oSql.ToString());

                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : Insert hold");
                nLastSeq = nLastSeq + 1;
                //Insert hold table
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO TPSTHoldHD WITH(ROWLOCK)(FNHldNo, FTBchCode, FTXshDocNo, FTShpCode, FNXshDocType, FDXshDocDate, FTXshCshOrCrd, FTXshVATInOrEx, FTDptCode, FTWahCode, FTPosCode, FTShfCode, FNSdtSeqNo, FTUsrCode, FTSpnCode,");
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
                oSql.AppendLine("INSERT INTO TPSTHoldHDCst WITH(ROWLOCK)(FNHldNo, FTBchCode, FTXshDocNo, FTXshCardID, FTXshCardNo, FNXshCrTerm, FDXshDueDate, FDXshBillDue, FTXshCtrName, FDXshTnfDate, FTXshRefTnfID, FNXshAddrShip, FNXshAddrTax, FTXshCstTel, FTXshCstName)");
                oSql.AppendLine("SELECT " + nLastSeq + " AS FNHldNo, FTBchCode, FTXshDocNo, FTXshCardID, FTXshCardNo, FNXshCrTerm, FDXshDueDate, FDXshBillDue, FTXshCtrName, FDXshTnfDate, FTXshRefTnfID, FNXshAddrShip, FNXshAddrTax, FTXshCstTel, FTXshCstName");
                oSql.AppendLine("FROM " + tC_TblSalHDCst + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("");
                //oSql.AppendLine("INSERT INTO TPSTHoldDT WITH(ROWLOCK)(FNHldNo, FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTXsdStkCode, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");
                oSql.AppendLine("INSERT INTO TPSTHoldDT WITH(ROWLOCK)(FNHldNo, FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FTPplCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, ");   //*Em 62-06-29
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
                oSql.AppendLine("INSERT INTO TPSTHoldDTDis WITH(ROWLOCK)(FNHldNo, FTBchCode, FTXihDocNo, FNXidSeqNo, FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                oSql.AppendLine("SELECT " + nLastSeq + " AS FNHldNo, DTD.FTBchCode, DTD.FTXshDocNo, DTD.FNXsdSeqNo, DTD.FDXddDateIns, DTD.FNXddStaDis, DTD.FTXddDisChgTxt, DTD.FTXddDisChgType, DTD.FCXddNet, DTD.FCXddValue");
                oSql.AppendLine("FROM " + tC_TblSalDTDis + " DTD WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN " + tC_TblSalDT + " DT WITH(NOLOCK) ON DT.FTBchCode = DTD.FTBchCode AND DT.FTXshDocNo = DTD.FTXshDocNo AND DT.FNXsdSeqNo = DTD.FNXsdSeqNo");
                oSql.AppendLine("WHERE DTD.FTBchCode = '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND DTD.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND DT.FTXsdStaPdt <> '4'");  //*Em 63-05-14
                oDB.C_SETxDataQuery(oSql.ToString());

                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : C_INSxShiftEvent");
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
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : C_PRCxPrintHoldBill");
                C_PRCxPrintHoldBill();

                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSale);
                //new wSale(3).Show();
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : W_SETxNewDoc");
                cVB.oVB_Sale.W_SETxNewDoc();    //*Em 63-03-02, Arm 63-03-05 ปรับตาม Baseline
                cVB.oVB_Sale.bW_Activate = false;
                cVB.oVB_Sale.Show();    //*Em 63-04-23
                //oFormShow = Application.OpenForms.Cast<Form>().LastOrDefault(oForm => oForm is wSale);
                //new wSale(3).Show();
                new cLog().C_WRTxLog("cSale", "C_PRCxHoldBill : End");
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
                new cSP().SP_CLExMemory();
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
                new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : Start");
                if (nC_HoldNo != 0)
                {
                    new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : C_GETxHoldHDCst");
                    C_GETxHoldHDCst();  //*Em 62-12-18
                    new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : C_GETxHoldDT2Sale");
                    C_GETxHoldDT2Sale();
                }
                new cLog().C_WRTxLog("cSale", "C_PRCxRetriveBill : End");
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
                new cSP().SP_CLExMemory();
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
                //}*/
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
                new cSP().SP_CLExMemory();
            }
        }

        private static void C_GETxHoldHDCst()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            cmlTPSTSalHDCst oHDCst = new cmlTPSTSalHDCst();
            cmlResCst oCstSch;
            cmlReqCstSch oReq;
            try
            {
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
                cClientService oCall = new cClientService();
                oCall = new cClientService(cVB.tVB_APIHeader, cVB.tVB_AgnKeyAPI);

                HttpResponseMessage oRep = new HttpResponseMessage();
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
                oCall = null;
                tJSonCall = null;
                oReq = null;
                oRep = null;

                cVB.oVB_Sale.W_SETxTextCst();
                cSale.C_DATxInsHDCst(cVB.tVB_CstCode);

                //+++++++++++++++

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
                new cSP().SP_CLExMemory();
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
                cVB.cVB_QRPayAmt = 0;
                cVB.bVB_ScanQR = false;

                cVB.nVB_CstPoint = 0;   //*Arm 63-03-12
                cVB.nVB_CstPiontB4Used = 0; //*Arm 63-03-16
                nC_RDSeqNo = 0; //*Arm 63-03-20 - Clear ลำดับ TPSTSalRD

                cVB.tVB_PriceGroup = cVB.tVB_BchPriceGroup; //* Net 63-03-24
                cVB.bVB_PriceConfirm = false;   //*Em 63-05-06

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxClearPara : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        public static void C_PRCxAdd2TmpLogChg(int pnType, string ptDocNo, bool pbTrans = false)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                switch (pnType)
                {
                    case 80: //การขาย-คืน
                        oSql.AppendLine("INSERT INTO TCNTTmpLogChg with(rowlock)(FTLogCode, FTLogDocNo, FNLogType, FTWahCode, FDCreateOn)");
                        oSql.AppendLine("SELECT FTShfCode, FTXshDocNo, 80 AS FNLogType, FTWahCode, GETDATE() AS FDCreateOn");
                        if (pbTrans)
                        {
                            oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)");
                        }
                        else
                        {
                            oSql.AppendLine("FROM " + tC_TblSalHD + " WITH(NOLOCK)");
                        }
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptDocNo + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());
                        break;
                    case 81:    //รอบการขาย
                        oSql.AppendLine("INSERT INTO TCNTTmpLogChg with(rowlock)(FTLogCode, FTLogDocNo, FNLogType, FTWahCode, FDCreateOn)");
                        oSql.AppendLine("SELECT FTShfCode, FTPosCode, 81 AS FNLogType, '' AS FTWahCode, GETDATE() AS FDCreateOn");
                        oSql.AppendLine("FROM TPSTShiftHD WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTShfCode = '" + ptDocNo + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());
                        break;
                    case 82:    //ยกเลิกบิล-ยกเลิกรายการ
                        oSql.AppendLine("INSERT INTO TCNTTmpLogChg with(rowlock)(FTLogCode, FTLogDocNo, FNLogType, FTWahCode, FDCreateOn)");
                        oSql.AppendLine("SELECT DISTINCT FNVidNo, '' AS FTLogDocNo, 82 AS FNLogType, '' AS FTWahCode, GETDATE() AS FDCreateOn");
                        oSql.AppendLine("FROM TPSTVoidDT WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FNVidNo = '" + ptDocNo + "'");
                        oDB.C_SETxDataQuery(oSql.ToString());
                        break;
                    case 90:    //ใบกำกำกับภาษี       //*Em 62-08-13
                        oSql.AppendLine("INSERT INTO TCNTTmpLogChg with(rowlock)(FTLogCode, FTLogDocNo, FNLogType, FTWahCode, FDCreateOn)");
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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

                    C_DATxUpdVat(); //*Em 62-10-08
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
            }
        }

        public static void C_PRCxClearCallBack()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                //*Em 63-05-08
                if (cSale.nC_DocType == 9)
                {
                    oSql.Clear();
                    oSql.AppendLine("DELETE FROM " + tC_TblSalHDDis + " WITH(ROWLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    oSql.AppendLine();
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

                //*Em 63-04-27 ไม่ต้องเคลียร์โปรโมชั่นตอน back กลับ
                ////*Em 63-03-29
                //oSql.AppendLine("DELETE FROM " + tC_TblSalPD + " WITH(ROWLOCK)");
                //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oSql.AppendLine();
                ////+++++++++++++++

                oSql.AppendLine("DELETE FROM " + tC_TblSalHDDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine();
                oSql.AppendLine("DELETE FROM " + tC_TblSalDTDis + " WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                //oSql.AppendLine("AND FNXddStaDis = 2"); //*Arm 63-06-08 Comment Code
                oSql.AppendLine("AND FNXddStaDis in(2,3)"); // *Arm 63-06-08 แก้ไข Code ให้ลบข้อมูล FNXddStaDis = 3(ส่วนลดท้ายบิลตามรายการ) ด้วย
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
                C_DATxUpdVat();
                C_PRCxSummary2HD();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxClearCallBack : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
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

                // CalVat
                C_DATxUpdVat();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                oSql.AppendLine("(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                oSql.AppendLine("SELECT FTBchCode, FTXihDocNo, FNXidSeqNo, FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                oSql.AppendLine("FROM TPSTHoldDTDis WITH(NOLOCK)");
                oSql.AppendLine("WHERE FNHldNo = " + cSale.nC_HoldNo);

                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_PRCxInsertRetriveBill : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
            }
            return bStaChk;

        }

        /// <summary>
        /// *Arm 63-05-15
        /// List SoDT
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
                new cSP().SP_CLExMemory();
            }

            return aoData;
        }
        /// <summary>
        /// *Arm 63-05-15
        /// List SoDTDis
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
                new cSP().SP_CLExMemory();
            }

            return aoData;
        }

        /// <summary>
        /// *Arm 63-05-15
        /// Insert SO
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

                new cLog().C_WRTxLog("wSale", "Created & Insert to Temp : Start... ");
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
                new cLog().C_WRTxLog("wSale", "Created & Insert to Temp : End... ");

                new cLog().C_WRTxLog("wSale", "Insert to Sale Temp : Start... ");

                if (ptStaAlwPosCalSo == "1")
                {
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("   FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");

                    //*Arm 63-05-13
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, DTTmp.FNXsdSeqNo, DTTmp.FTPdtCode, DTTmp.FTXsdPdtName, ");
                    oSql.AppendLine("    DTTmp.FTPunCode, DTTmp.FTPunName, DTTmp.FCXsdFactor,DTTmp.FTXsdBarCode, DTTmp.FTSrnCode, ");
                    oSql.AppendLine("    DTTmp.FTXsdVatType, DTTmp.FTVatCode, PRI.FTPplCode AS FTPplCode, DTTmp.FCXsdVatRate, DTTmp.FTXsdSaleType, ");
                    oSql.AppendLine("    ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0)) AS FCXsdSalePrice,");
                    oSql.AppendLine("    DTTmp.FCXsdQty, (FCXsdFactor * DTTmp.FCXsdQty) AS FCXsdQtyAll,");
                    oSql.AppendLine("    ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0)) AS FCXsdSetPrice,");
                    oSql.AppendLine("    (DTTmp.FCXsdQty * ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0))) AS FCXsdAmtB4DisChg,");
                    oSql.AppendLine("    '' AS FTXsdDisChgTxt,0 AS FCXsdDis, 0 AS FCXsdChg,");
                    oSql.AppendLine("    (DTTmp.FCXsdQty * ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0))) ASFCXsdNet, ");
                    oSql.AppendLine("    (DTTmp.FCXsdQty * ISNULL(PRI.FCpdtPrice, ISNULL(DTTmp.FCXsdSalePrice, 0))) AS FCXsdNetAfHD,");
                    oSql.AppendLine("    0 AS FCXsdVat, 0 AS FCXsdVatable, DTTmp.FCXsdWhtAmt, DTTmp.FTXsdWhtCode, DTTmp.FCXsdWhtRate, ");
                    oSql.AppendLine("    0 AS FCXsdCostIn, 0 AS FCXsdCostEx, '1' AS FTXsdStaPdt, DTTmp.FCXsdQtyLef, DTTmp.FCXsdQtyRfn, ");
                    oSql.AppendLine("    DTTmp.FTXsdStaPrcStk, DTTmp.FTXsdStaAlwDis, DTTmp.FNXsdPdtLevel, DTTmp.FTXsdPdtParent, DTTmp.FCXsdQtySet, ");
                    oSql.AppendLine("    DTTmp.FTPdtStaSet, DTTmp.FTXsdRmk, ");
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM TARTSoDTTmp DTTmp WITH(NOLOCK)");
                    oSql.AppendLine("LEFT JOIN  TPSTPdtPrice PRI WITH(NOLOCK) ON DTTmp.FTPdtCode = PRI.FTPdtCode AND PRI.FTPriType = '1' ");
                    oSql.AppendLine("AND DTTmp.FTPunCode = PRI.FTPunCode AND(ISNULL(PRI.FTPplCode, '') = '" + cVB.tVB_PriceGroup + "')");
                    oSql.AppendLine("WHERE DTTmp.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DTTmp.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine();

                }
                else
                {
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("   FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, '" + cVB.tVB_PriceGroup + "' AS FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, ABS(FCXsdDis) AS FCXsdDis, ABS(FCXsdChg) AS FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, '1' AS FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
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
                oDB.C_SETxDataQuery(oSql.ToString());

                new cLog().C_WRTxLog("wSale", "Insert to Sale Temp : End... ");

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
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Insert ราการอ้างอิงบิลคืน (*Arm 63-06-04)
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
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * FCXsdQty) AS FCXsdAmtB4DisChg, '' AS FTXsdDisChgTxt, 0 AS FCXsdDis, 0 AS FCXsdChg, (FCXsdSetPrice * FCXsdQty) AS FCXsdNet, (FCXsdSetPrice * FCXsdQty) AS FCXsdNetAfHD, 0 AS FCXsdVat, 0 AS FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    0 AS FCXsdCostIn, 0 AS FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("AND FTXsdStaPdt <> '4'");

                    oDB.C_SETxDataQuery(oSql.ToString());

                }
                else
                {
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, DT.FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * DT.FCXsdQty) AS FCXsdAmtB4DisChg, '' AS FTXsdDisChgTxt, 0 AS FCXsdDis, 0 AS FCXsdChg, (FCXsdSetPrice * DT.FCXsdQty) AS FCXsdNet, (FCXsdSetPrice * DT.FCXsdQty) AS FCXsdNetAfHD, 0 AS FCXsdVat, 0 AS FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    0 AS FCXsdCostIn, 0 AS FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " DT WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
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
                new cSP().SP_CLExMemory();
            }
        }

        public static bool C_PRCbInsRefund()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            //string tTblSalDT = "";
            //string tTblSalRC = "";
            //string tTblSalPD = "";
            //string tTblSalRD = "";
            //string tTblSalDTDis = "";
            //string tTblSalHDDis = "";
            try
            {
                //*Arm 63-06-01 Set Table ที่ต้องใช้ Query ตามเงื่อนไข
                //if (cVB.bVB_RefundDataFrom == true) // bVB_RefundDataFrom = ข้อมูลมาจาก True: ข้อมูลในเครื่อง, False: ข้อมูลการขายมาจากหลังบ้านโดย Call API2ARDoc
                //{
                //    // ถ้าเป็นข้อมูลการขายมาภายในเครื่องจุดขายเดิม
                //    if (cVB.bVB_RefundTrans == true) // bVB_RefundTrans = true: ตาราง Transaction, false: ตาราง Temp
                //    {
                //        //ตาราง Transaction
                //        tTblSalDT = "TPSTSalDT";
                //        tTblSalRC = "TPSTSalRC";
                //        tTblSalPD = "TPSTSalPD";
                //        tTblSalRD = "TPSTSalPD";
                //        tTblSalDTDis = "TPSTSalDTDis";
                //        tTblSalHDDis = "TPSTSalHDDis";
                //    }
                //    else
                //    {
                //        //ตาราง Temp
                //        tTblSalDT = tC_TblSalDT;
                //        tTblSalRC = tC_TblSalRC;
                //        tTblSalPD = tC_TblSalPD;
                //        tTblSalRD = tC_TblSalRD;
                //        tTblSalDTDis = tC_TblSalDTDis;
                //        tTblSalHDDis = tC_TblSalHDDis;
                        
                //        //tTblSalDT = "TSDT" + cVB.tVB_PosCode;
                //        //tTblSalRC = "TSRC" + cVB.tVB_PosCode;
                //        //tTblSalPD = "TSPD" + cVB.tVB_PosCode;
                //        //tTblSalRD = "TSRD" + cVB.tVB_PosCode;
                //        //tTblSalDTDis = "TSDTDis" + cVB.tVB_PosCode;
                //        //tTblSalHDDis = "TSHDDis" + cVB.tVB_PosCode;
                //    }
                //}
                //else
                //{
                //    // ถ้าเป็นข้อมูลการขายข้ามเครื่องจุดขาย (Call API2ARDoc)
                //    tTblSalDT = "TPSTSalDTTmp";
                //    tTblSalRC = "TPSTSalRCTmp";
                //    tTblSalPD = "TPSTSalPDTmp";
                //    tTblSalRD = "TPSTSalRDTmp";
                //    tTblSalDTDis = "TPSTSalDTDisTmp";
                //    tTblSalHDDis = "TPSTSalHDDisTmp";
                //}
                
                if (cVB.bVB_RefundFullBill) // bVB_RefundFullBill = true : คืนเต็มบิล , false : คืนบางรายการ
                {
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    //oSql.AppendLine("FROM " + tTblSalDT + " WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                    //oSql.AppendLine("FROM " + tTblSalDTDis + " WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDTDis + " WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                    //oSql.AppendLine("FROM " + tTblSalPD + " WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalPD + " WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                    //oSql.AppendLine("FROM " + tTblSalRD + " WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalRD + " WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oDB.C_SETxDataQuery(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                    oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt , FTXhdRefCode "); //*Arm 63-04-16
                    //oSql.AppendLine("FROM " + tTblSalHDDis + " WITH(NOLOCK) ");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalHDDis + " WITH(NOLOCK) ");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'  ");
                    oSql.AppendLine("AND (ISNULL(FTXhdRefCode,'') ='' OR FTXhdDisChgType = '5'  OR FTXhdDisChgType = '6')");
                    oSql.AppendLine("ORDER BY FDXhdDateIns ASC");
                    cVB.aoVB_PdtHDDisRefund = new cDatabase().C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());
                }
                else
                {
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                    oSql.AppendLine("    FCXsdSalePrice, RF.FCXsdQtyRfn AS FCXsdQty, (RF.FCXsdQtyRfn * FCXsdFactor) AS FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdNet, (ISNULL(DT.FCXsdNetAfHD,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                    //oSql.AppendLine("FROM " + tTblSalDT + " DT WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDT + " DT WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //oSql.AppendLine("AND FNXsdSeqNo IN ("+ string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, (ISNULL(DT.FCXddNet,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddNet, (ISNULL(DT.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddValue");
                    //oSql.AppendLine("FROM " + tTblSalDTDis + " DT WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDTDis + " DT WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //oSql.AppendLine("AND FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, PD.FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                    //oSql.AppendLine("FROM " + tTblSalPD + " PD WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalPD + " PD WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = PD.FNXsdSeqNo");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //oSql.AppendLine("AND FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    oSql.AppendLine();
                    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                    //oSql.AppendLine("FROM " + tTblSalRD + " RD WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalRD + " RD WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = RD.FNXrdRefSeq");
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    //oSql.AppendLine("AND FNXrdRefSeq IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    oDB.C_SETxDataQuery(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("SELECT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice,");
                    oSql.AppendLine("DTDis.FNXddStaDis AS pnStaDis, DTDis.FTXddDisChgType AS ptDisChgType, DTDis.FCXddNet AS pcNet,  ");
                    oSql.AppendLine("(ISNULL(DTDis.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS pcValue, DTDis.FTXddRefCode AS ptRefCode");
                    //oSql.AppendLine("FROM " + tTblSalDTDis + " DTDis WITH(NOLOCK)");
                    //oSql.AppendLine("INNER JOIN " + tTblSalDT + " DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                    oSql.AppendLine("FROM " + cSale.tC_Ref_TblSalDTDis + " DTDis WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_Ref_TblSalDT + " DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                    oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                    oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                    oSql.AppendLine("AND DTDis.FNXddStaDis = 2 ");
                    //oSql.AppendLine("AND DT.FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                    oSql.AppendLine("ORDER BY DTDis.FDXddDateIns ASC");
                    cVB.aoVB_PdtDisChgRefund = new cDatabase().C_GETaDataQuery<cmlPdtDisChg>(oSql.ToString());
                }
                //++++++++++++++++++




                //if (cVB.bVB_RefundFullBill)
                //{
                //    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    //oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    //oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    //oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                //    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    //oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    //oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    //oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //    //oSql.AppendLine("FROM TPSTSalDT WITH(NOLOCK)");
                //    //oSql.AppendLine("WHERE FTBchCode = '"+ cVB.tVB_BchCode +"'");
                //    //oSql.AppendLine("AND FTXshDocNo = '"+ cVB.tVB_RefDocNo +"'");
                //    //oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                //    //oSql.AppendLine();
                //    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                //    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                //    //oSql.AppendLine("FROM TPSTSalDTDis WITH(NOLOCK)");
                //    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //oSql.AppendLine();
                //    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                //    //oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                //    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                //    //oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                //    //oSql.AppendLine("FROM TPSTSalPD WITH(NOLOCK)");
                //    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //oSql.AppendLine();
                //    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                //    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                //    //oSql.AppendLine("FROM TPSTSalRD WITH(NOLOCK)");
                //    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //oDB.C_SETxDataQuery(oSql.ToString());

                //    //oSql.Clear();
                //    //oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                //    //oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt , FTXhdRefCode "); //*Arm 63-04-16
                //    //oSql.AppendLine("FROM TPSTSalHDDis WITH(NOLOCK) ");
                //    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                //    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'  ");
                //    //oSql.AppendLine("AND (ISNULL(FTXhdRefCode,'') ='' OR FTXhdDisChgType = '5'  OR FTXhdDisChgType = '6')");
                //    //oSql.AppendLine("ORDER BY FDXhdDateIns ASC");
                //    //cVB.aoVB_PdtHDDisRefund = new cDatabase().C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());

                //    ////*Arm 63-05-20 - เช็คกรณีข้อมูลมาจากหลังบ้านให้ Select ที่ตาราง Temp
                //    //if (cVB.bVB_RefundTrans == true)
                //    //{
                //    //    //ข้อมูลจากเครื่อง
                //    //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    //    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    //    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    //    oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                //    //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    //    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    //    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    //    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //    //    oSql.AppendLine("FROM TPSTSalDT WITH(NOLOCK)");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //    oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                //    //    oSql.AppendLine();
                //    //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                //    //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                //    //    oSql.AppendLine("FROM TPSTSalDTDis WITH(NOLOCK)");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //    oSql.AppendLine();
                //    //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                //    //    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                //    //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                //    //    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                //    //    oSql.AppendLine("FROM TPSTSalPD WITH(NOLOCK)");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //    oSql.AppendLine();
                //    //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                //    //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                //    //    oSql.AppendLine("FROM TPSTSalRD WITH(NOLOCK)");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //    oDB.C_SETxDataQuery(oSql.ToString());

                //    //    oSql.Clear();
                //    //    oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                //    //    oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt , FTXhdRefCode "); //*Arm 63-04-16
                //    //    oSql.AppendLine("FROM TPSTSalHDDis WITH(NOLOCK) ");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'  ");
                //    //    oSql.AppendLine("AND (ISNULL(FTXhdRefCode,'') ='' OR FTXhdDisChgType = '5'  OR FTXhdDisChgType = '6')");
                //    //    oSql.AppendLine("ORDER BY FDXhdDateIns ASC");
                //    //    cVB.aoVB_PdtHDDisRefund = new cDatabase().C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());
                //    //}
                //    //else
                //    //{
                //    //    //ข้อมูลจากหลังบ้านผ่าน API2ARDoc
                //    //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    //    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    //    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    //    oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                //    //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    //    oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    //    oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    //    oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //    //    oSql.AppendLine("FROM TPSTSalDTTmp WITH(NOLOCK)");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //    oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                //    //    oSql.AppendLine();
                //    //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                //    //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                //    //    oSql.AppendLine("FROM TPSTSalDTDisTmp WITH(NOLOCK)");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //    oSql.AppendLine();
                //    //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                //    //    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                //    //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                //    //    oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                //    //    oSql.AppendLine("FROM TPSTSalPDTmp WITH(NOLOCK)");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //    oSql.AppendLine();
                //    //    oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                //    //    oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                //    //    oSql.AppendLine("FROM TPSTSalRDTmp WITH(NOLOCK)");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //    oDB.C_SETxDataQuery(oSql.ToString());

                //    //    oSql.Clear();
                //    //    oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                //    //    oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt , FTXhdRefCode ");
                //    //    oSql.AppendLine("FROM TPSTSalHDDisTmp WITH(NOLOCK) ");
                //    //    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                //    //    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'  ");
                //    //    oSql.AppendLine("AND (ISNULL(FTXhdRefCode,'') ='' OR FTXhdDisChgType = '5'  OR FTXhdDisChgType = '6')");
                //    //    oSql.AppendLine("ORDER BY FDXhdDateIns ASC");
                //    //    cVB.aoVB_PdtHDDisRefund = new cDatabase().C_GETaDataQuery<cmlTPSTSalHDDis>(oSql.ToString());
                //    //}
                //    ////++++++++ End *Arm 63-05-20 - เช็คกรณีข้อมูลมาจากหลังบ้านให้ Select ที่ตาราง Temp ++++++++
                //}
                //else
                //{
                //    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    //oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    //oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    //oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                //    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //    //oSql.AppendLine("    FCXsdSalePrice, RF.FCXsdQtyRfn AS FCXsdQty, (RF.FCXsdQtyRfn * FCXsdFactor) AS FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdNet, (ISNULL(DT.FCXsdNetAfHD,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //    //oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //    //oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //    //oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                //    //oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                //    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    ////oSql.AppendLine("AND FNXsdSeqNo IN ("+ string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //    //oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                //    //oSql.AppendLine();
                //    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                //    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, (ISNULL(DT.FCXddNet,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddNet, (ISNULL(DT.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddValue");
                //    //oSql.AppendLine("FROM TPSTSalDTDis DT WITH(NOLOCK)");
                //    //oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                //    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    ////oSql.AppendLine("AND FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //    //oSql.AppendLine();
                //    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                //    //oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                //    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, PD.FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                //    //oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                //    //oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
                //    //oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = PD.FNXsdSeqNo");
                //    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    ////oSql.AppendLine("AND FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //    //oSql.AppendLine();
                //    //oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                //    //oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                //    //oSql.AppendLine("FROM TPSTSalRD RD WITH(NOLOCK)");
                //    //oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = RD.FNXrdRefSeq");
                //    //oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    ////oSql.AppendLine("AND FNXrdRefSeq IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //    //oDB.C_SETxDataQuery(oSql.ToString());

                //    //oSql.Clear();
                //    //oSql.AppendLine("SELECT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice,");
                //    //oSql.AppendLine("DTDis.FNXddStaDis AS pnStaDis, DTDis.FTXddDisChgType AS ptDisChgType, DTDis.FCXddNet AS pcNet,  ");
                //    //oSql.AppendLine("(ISNULL(DTDis.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS pcValue, DTDis.FTXddRefCode AS ptRefCode");
                //    //oSql.AppendLine("FROM TPSTSalDTDis DTDis WITH(NOLOCK)");
                //    //oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                //    //oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                //    //oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                //    //oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //    //oSql.AppendLine("AND DTDis.FNXddStaDis = 2 ");
                //    ////oSql.AppendLine("AND DT.FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //    //oSql.AppendLine("ORDER BY DTDis.FDXddDateIns ASC");
                //    //cVB.aoVB_PdtDisChgRefund = new cDatabase().C_GETaDataQuery<cmlPdtDisChg>(oSql.ToString());

                //    //*Arm 63-05-20 - เช็คกรณีข้อมูลมาจากหลังบ้านให้ Select ที่ตาราง Temp
                //    if (cVB.bVB_RefundTrans == true)
                //    {
                //        //ข้อมูลจากเครื่อง
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
                //        //ข้อมูลจากหลังบ้านผ่าน API2ARDoc
                //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDT + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //        oSql.AppendLine("    FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //        oSql.AppendLine("	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FTPplCode, FCXsdVatRate, FTXsdSaleType, ");
                //        oSql.AppendLine("    FCXsdSalePrice, RF.FCXsdQtyRfn AS FCXsdQty, (RF.FCXsdQtyRfn * FCXsdFactor) AS FCXsdQtyAll, FCXsdSetPrice, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, FCXsdChg, (FCXsdSetPrice * RF.FCXsdQtyRfn) AS FCXsdNet, (ISNULL(DT.FCXsdNetAfHD,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, FCXsdWhtRate, ");
                //        oSql.AppendLine("    FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, 0 AS FCXsdQtyRfn, FTXsdStaPrcStk, FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //        oSql.AppendLine("	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //        oSql.AppendLine("FROM TPSTSalDTTmp DT WITH(NOLOCK)");
                //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        //oSql.AppendLine("AND FNXsdSeqNo IN ("+ string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //        oSql.AppendLine("AND FTXsdStaPdt <> '4'");
                //        oSql.AppendLine();
                //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalDTDis + "(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, RF.FNXsdSeqNo, FDXddDateIns, FTXddRefCode, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, (ISNULL(DT.FCXddNet,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddNet, (ISNULL(DT.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS FCXddValue");
                //        oSql.AppendLine("FROM TPSTSalDTDisTmp DT WITH(NOLOCK)");
                //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        //oSql.AppendLine("AND FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //        oSql.AppendLine();
                //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalPD + " (FTBchCode, FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis,");
                //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn)");
                //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FTPmhDocNo, RF.FNXsdSeqNo, FTPmdGrpName, FTPdtCode, FTPunCode, PD.FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdNet, FCXpdGetQtyDiv, FTXpdGetType, FCXpdGetValue, FCXpdDis, ");
                //        oSql.AppendLine("	FCXpdPerDisAvg, FCXpdDisAvg, FCXpdPoint, FTXpdStaRcv, FTPplCode, FTXpdCpnText, FTCpdBarCpn");
                //        oSql.AppendLine("FROM TPSTSalPDTmp PD WITH(NOLOCK)");
                //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = PD.FNXsdSeqNo");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        //oSql.AppendLine("AND FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //        oSql.AppendLine();
                //        oSql.AppendLine("INSERT INTO " + cSale.tC_TblSalRD + " (FTBchCode, FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse)");
                //        oSql.AppendLine("SELECT '" + cVB.tVB_BchCode + "' AS FTBchCode, '" + cVB.tVB_DocNo + "' AS FTXshDocNo, FNXrdSeqNo, FTRdhDocType, FNXrdRefSeq, FTXrdRefCode, FCXrdPdtQty, FNXrdPntUse");
                //        oSql.AppendLine("FROM TPSTSalRDTmp RD WITH(NOLOCK)");
                //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = RD.FNXrdRefSeq");
                //        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        //oSql.AppendLine("AND FNXrdRefSeq IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //        oDB.C_SETxDataQuery(oSql.ToString());

                //        oSql.Clear();
                //        oSql.AppendLine("SELECT DT.FTPdtCode AS ptPdtCode, DT.FTXsdBarCode AS ptBarcode, DT.FCXsdSetPrice AS pcSetPrice,");
                //        oSql.AppendLine("DTDis.FNXddStaDis AS pnStaDis, DTDis.FTXddDisChgType AS ptDisChgType, DTDis.FCXddNet AS pcNet,  ");
                //        oSql.AppendLine("(ISNULL(DTDis.FCXddValue,0.00)/RF.FCXsdQty) * RF.FCXsdQtyRfn AS pcValue, DTDis.FTXddRefCode AS ptRefCode");
                //        oSql.AppendLine("FROM TPSTSalDTDisTmp DTDis WITH(NOLOCK)");
                //        oSql.AppendLine("INNER JOIN TPSTSalDTTmp DT WITH(NOLOCK) ON DTDis.FTBchCode = DT.FTBchCode AND DTDis.FTXshDocNo = DT.FTXshDocNo AND DTDis.FNXsdSeqNo = DT.FNXsdSeqNo");
                //        oSql.AppendLine("INNER JOIN " + cSale.tC_TblRefund + " RF WITH(NOLOCK) ON RF.FNXsdSeqNoOld = DT.FNXsdSeqNo");
                //        oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                //        oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_RefDocNo + "'");
                //        oSql.AppendLine("AND DTDis.FNXddStaDis = 2 ");
                //        //oSql.AppendLine("AND DT.FNXsdSeqNo IN (" + string.Join(",", cVB.anVB_PdtSeqNoRefund) + ")");
                //        oSql.AppendLine("ORDER BY DTDis.FDXddDateIns ASC");
                //        cVB.aoVB_PdtDisChgRefund = new cDatabase().C_GETaDataQuery<cmlPdtDisChg>(oSql.ToString());

                //    }
                //    //++++++++ End *Arm 63-05-20 - เช็คกรณีข้อมูลมาจากหลังบ้านให้ Select ที่ตาราง Temp ++++++++
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
                new cSP().SP_CLExMemory();
            }
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

                oSql.AppendLine("SELECT MAX(FNVidNo) AS FNVidNo FROM TPSTVoidDT WITH(NOLOCK)");
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
                new cSP().SP_CLExMemory();
            }
        }
        #endregion Function


        #region Print
        /// <summary>
        /// Process Print
        /// </summary>
        private static void C_PRCxPrintSlip()
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
                oDoc.PrintPage += C_PRNxSlip;

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
                    //*Net 63-05-21  print ต้นฉบับ สำเนา ตาม option
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
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRCxPrintSlip : " + oEx.Message); }
            finally
            {
                if (oDoc != null)
                    oDoc.Dispose();

                oDoc = null;
                oSettings = null;
                oSP.SP_CLExMemory();
            }
        }

        //*Net 63-05-21 Gen หน้า slip ปรับเป็น Public
        public static void C_PRNxSlip(object sender, PrintPageEventArgs e)
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
                            oGraphic.DrawImage(oLogo, new Rectangle((nWidth - 200) / 2, nStartY, 200, 200));
                            nStartY += 200;
                        }

                    }
                }
                //+++++++++++++++++

                oMsg = new cSlipMsg();
                nStartY = oMsg.C_GETnSlipMsg("1", oGraphic, nWidth, nStartY);    // Header Slip Msg

                // Get ค่า ID เครื่อง จาก DB
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 18;
                oGraphic.DrawString(Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm") + " BNO:" + cVB.tVB_DocNoPrn,
                                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                //Print DT
                C_PRNxPrintDT(ref oGraphic, ref nStartY, nWidth, false); //*Net 63-03-28 ยกมาจาก baseline
                nStartY += 30;

                //Total
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FCXshTotal,FCXshTotalNV,FCXshTotalNoDis,FCXshTotalB4DisChgV,FCXshTotalB4DisChgNV,FTXshDisChgTxt,");
                oSql.AppendLine("FCXshDis,FCXshChg,FCXshTotalAfDisChgV,FCXshTotalAfDisChgNV,FCXshRefAEAmt,FCXshAmtV,FCXshAmtNV,");
                oSql.AppendLine("FCXshVat,FCXshVatable,FCXshGrand,FTXshRmk,FCXshRnd,FTCstCode, FTXshRefExt,FTUsrCode"); //*Arm 63-04-03 FTCstCode //Arm 63-04-08 FTXshRefExt //*Arm 63-05-08 FTUsrCode
                oSql.AppendLine("FROM " + tC_TblSalHD + " WITH(NOLOCK)");
                //oSql.AppendLine("FROM TPSTSalHD WITH(NOLOCK)"); //*Arm 63-03-02
                oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
                cmlTPSTSalHD oHD = oDB.C_GEToDataQuery<cmlTPSTSalHD>(oSql.ToString());
                if (oHD != null)
                {
                    if (oHD.FCXshDis != 0 || oHD.FCXshChg != 0 || oHD.FCXshRnd != 0)
                    {
                        oGraphic.DrawString("Subtotal", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshTotal, cVB.nVB_DecShow);
                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                        nStartY += 18;

                        //*Arm 63-04-03 - Print HDDis
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgTxt , FTXhdDisChgType,");
                        //oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg AS FCXpdDisChg, FCXhdAmt AS FCXpdAmt , FTXhdRefCode ");
                        oSql.AppendLine("FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt, FTXhdRefCode "); //*Arm 63-04-16
                        //oSql.AppendLine("FROM TPSTSalHDDis WITH(NOLOCK) ");
                        oSql.AppendLine("FROM " + tC_TblSalHDDis + " WITH(NOLOCK) "); //*Em 63-05-16
                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'  ");
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
                                        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXpdAmt, cVB.nVB_DecShow);
                                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, cVB.nVB_DecShow); //*Arm 63-04-16
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                        nStartY += 18;
                                    }
                                    //ชาจน์
                                    if (oHDDis.FTXhdDisChgType == "3" || oHDDis.FTXhdDisChgType == "4")
                                    {
                                        oGraphic.DrawString(cVB.oVB_GBResource.GetString("tChg") + "(" + oHDDis.FTXhdDisChgTxt + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXpdAmt, cVB.nVB_DecShow); 
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
                                        //oSql.AppendLine("FROM TPSTSalRD WITH(NOLOCK) ");
                                        oSql.AppendLine("FROM " + tC_TblSalRD + " WITH(NOLOCK) ");    //*Em 63-05-16
                                        oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                                        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'  ");
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
                                        //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXpdAmt, cVB.nVB_DecShow);
                                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, cVB.nVB_DecShow); //*Arm 63-04-16
                                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                                        nStartY += 18;
                                    }
                                }

                                //Coupon
                                if (oHDDis.FTXhdDisChgType == "5" || oHDDis.FTXhdDisChgType == "6")
                                {
                                    oGraphic.DrawString(cVB.oVB_GBResource.GetString("tCpnRd") + "(" + oHDDis.FTXhdDisChgTxt + ")", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXpdAmt, cVB.nVB_DecShow);
                                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHDDis.FCXhdAmt, cVB.nVB_DecShow); //*Arm 63-04-16
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

                        if ((decimal)oHD.FCXshRnd != (decimal)0)
                        {
                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshRnd, cVB.nVB_DecShow);
                            oGraphic.DrawString("Round Rcv: " + tAmt, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oHD.FCXshTotal - oHD.FCXshDis + oHD.FCXshChg), cVB.nVB_DecShow);
                            oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                            nStartY += 18;
                        }

                        aoHDDis = null; //Clear
                    }

                    //*Arm 63-04-20  แสดงจำนวนสินค้ารวม
                    oSql.Clear();
                    //oSql.AppendLine("SELECT SUM(ISNULL(FCXsdQty,0)) FROM TPSTSalDT  with(nolock)");
                    oSql.AppendLine("SELECT SUM(ISNULL(FCXsdQty,0)) FROM " + tC_TblSalDT + " with(nolock)"); //*Em 63-05-16
                    oSql.AppendLine("WHERE FTBchCode = '" + cVB.tVB_BchCode + "' ");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' ");
                    oSql.AppendLine("AND FTXsdStaPdt <> '4' ");
                    cTotalQty = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                    oGraphic.DrawString("TOTAL " + oSP.SP_SETtDecShwSve(1, (decimal)cTotalQty, cVB.nVB_DecShow) + " Items", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshGrand, cVB.nVB_DecShow);
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);

                    //++++++++++++++++++

                    //*Arm 63-04-20 Comment Code
                    //nStartY += 18;
                    //oGraphic.DrawString("TOTAL (VAT Included)", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY); //*Arm 62-10-27 -เพิ่ม Wording  (VAT Included) ใบกำกับภาษีอย่างย่อ
                    //tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshGrand, cVB.nVB_DecShow);
                    //oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);

                    nStartY += 18;
                    tAmt = "Vatable : " + oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshVatable, cVB.nVB_DecShow);
                    tAmt += " " + "VAT : " + oSP.SP_SETtDecShwSve(1, (decimal)oHD.FCXshVat, cVB.nVB_DecShow);
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                }


                //Print Payment
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RCV.FTRcvCode,(CASE WHEN ISNULL(RCVL.FTRcvName,'') = '' THEN (SELECT TOP 1 FTRcvName FROM TFNMRcv_L with(NOLOCK) WHERE FTRcvCode = RCV.FTRcvCode) ELSE RCVL.FTRcvName END) AS FTRcvName,");
                oSql.AppendLine("RCV.FCXrcUsrPayAmt,FCXrcDep,FCXrcChg,RCV.FTXrcRefNo1,RCV.FTXrcRefNo2 "); //*Net 63-03-28 ยกมาจาก baseline
                oSql.AppendLine("FROM " + tC_TblSalRC + " RCV WITH(NOLOCK)"); //*Em 63-05-16
                //oSql.AppendLine("FROM TPSTSalRC RCV WITH(NOLOCK)"); //*Arm 63-03-02
                oSql.AppendLine("LEFT JOIN TFNMRcv_L RCVL WITH(NOLOCK) ON RCV.FTRcvCode = RCVL.FTRcvCode AND RCVL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("INNER JOIN TFNMRcv RCVM WITH(NOLOCK) ON RCV.FTRcvCode = RCVM.FTRcvCode AND RCVM.FTFmtCode <> '020'");   //*Em 62-12-27
                oSql.AppendLine("WHERE RCV.FTBchCode =  '" + cVB.tVB_BchCode + "'");
                //oSql.AppendLine("AND RCV.FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                oSql.AppendLine("AND RCV.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
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
                        oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[7], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                        cChange = (decimal)oRC.FCXrcChg;
                    }
                    nStartY += 18;
                    oGraphic.DrawString("Change", cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    tAmt = oSP.SP_SETtDecShwSve(1, cChange, cVB.nVB_DecShow);
                    oGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(140, nStartY, nWidth - 150, 18), oFormatFar);
                }

                //*Arm 63-04-08 อ้างอิง SO
                if (!string.IsNullOrEmpty(oHD.FTXshRefExt))
                {
                    nStartY += 18;
                    oGraphic.DrawString("อ้างอิง : " + oHD.FTXshRefExt, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                }
                //++++++++++++++++++++++

                //*Arm 63-04-03 - Print Point
                if (!string.IsNullOrEmpty(oHD.FTCstCode))
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTBchCode, FTXshDocNo,FTXshCardID,FTXshCardNo,FTXshCstName,");
                    oSql.AppendLine("FTXshCstTel,ISNULL(FCXshCstPnt,0) AS FCXshCstPnt, ISNULL(FCXshCstPntPmt,0) AS FCXshCstPntPmt");
                    //oSql.AppendLine("FROM TPSTSalHDCst WITH(NOLOCK) ");
                    oSql.AppendLine("FROM " + tC_TblSalHDCst + " WITH(NOLOCK) "); //*Em 63-05-16
                    oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    oHDCst = oDB.C_GEToDataQuery<cmlTPSTSalHDCst>(oSql.ToString());

                    if (oHDCst != null) //*Net 63-04-03
                    {
                        tCstTel = oHDCst.FTXshCstTel == null ? "" : oHDCst.FTXshCstTel; //*Arm 63-04-20 ใช้ใน QR Code

                        decimal cCstPiontB4Used = 0; //*Arm 63-05-09
                        decimal cSumPnt = 0; //*Arm 63-05-09

                        if (!string.IsNullOrEmpty(oHDCst.FTXshDocNo)) //*Arm 63-04-03 
                        {
                            //หา Transaction การขาย
                            oSql = new StringBuilder();
                            oSql.AppendLine("SELECT * FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + tC_TxnRefCode + "'");
                            cmlTCNTMemTxnSale oTxnSale = oDB.C_GEToDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());

                            //Print ข้อมมูล Member
                            if (!string.IsNullOrEmpty(oTxnSale.FTMemCode))
                            {
                                nStartY += 30;
                                oGraphic.DrawString("Mem ID : " + oTxnSale.FTMemCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                                nStartY += 18;
                                oSql = new StringBuilder();
                                oGraphic.DrawString(oHDCst.FTXshCstName, cVB.aoVB_PInvLayout[1], Brushes.Black, 10, nStartY);

                                string tCardNo = oHDCst.FTXshCardNo == null ? " - " : oHDCst.FTXshCardNo;
                                nStartY += 18;
                                oGraphic.DrawString("Card No. : " + tCardNo, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                                nStartY += 18;
                                if (oTxnSale.FDTxnPntExpired != null)
                                {
                                    oGraphic.DrawString("Expired : " + string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oTxnSale.FDTxnPntExpired)), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                                }
                                else
                                {
                                    oGraphic.DrawString("Expired : -", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                                }

                                nStartY += 18;
                                oGraphic.DrawString("Last Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                                oGraphic.DrawString("Reg. Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                oGraphic.DrawString("Promo Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                oGraphic.DrawString("Total Point", cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);

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
                                    oSql.Clear();
                                    oSql.AppendLine("SELECT * FROM TCNTMemTxnRedeem WHERE FTRedRefDoc = '" + tC_TxnRefCode + "'");
                                    cmlTCNTMemTxnRedeem oTxnRedeem = oDB.C_GEToDataQuery<cmlTCNTMemTxnRedeem>(oSql.ToString());
                                    if (oTxnRedeem != null)
                                    {
                                        nStartY += 18;
                                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)oTxnRedeem.FCRedPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                        decimal cRedPnt = (decimal)(oTxnRedeem.FCRedPntBillQty * (-1));  //*Arm 63-05-09
                                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRedPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, 0, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                        //cPntRcv = (decimal)(oTxnRedeem.FCRedPntB4Bill + (oTxnRedeem.FCRedPntBillQty * (-1)));
                                        cSumPnt = (decimal)(oTxnRedeem.FCRedPntB4Bill + cRedPnt); //*Arm 63-05-09
                                        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29
                                    }
                                    nStartY += 18;
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)oTxnSale.FCTxnPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    decimal cSalePnt = (decimal)(oTxnSale.FCTxnPntBillQty - oHDCst.FCXshCstPntPmt);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSalePnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPntPmt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                    //cSumPnt = (decimal)(oTxnSale.FCTxnPntB4Bill + oTxnSale.FCTxnPntBillQty);
                                    cSumPnt = (decimal)(oTxnSale.FCTxnPntB4Bill + (cSalePnt + oHDCst.FCXshCstPntPmt)); //*Arm 63-05-09
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);
                                }
                                else
                                {
                                    //หา Point ก่อนใช้

                                    oSql.Clear();
                                    oSql.AppendLine("SELECT count(*) FROM TCNTMemTxnRedeem with(nolock) WHERE FTRedRefDoc = '" + tC_TxnRefCode + "'");
                                    if (oDB.C_GEToDataQuery<int>(oSql.ToString()) > 0)
                                    {
                                        oSql.Clear();
                                        oSql.AppendLine("SELECT FCRedPntB4Bill FROM TCNTMemTxnRedeem with(nolock) WHERE FTRedRefDoc = '" + tC_TxnRefCode + "'");

                                        cCstPiontB4Used = oDB.C_GEToDataQuery<decimal>(oSql.ToString());
                                    }
                                    else
                                    {
                                        cCstPiontB4Used = (decimal)oTxnSale.FCTxnPntB4Bill;
                                    }

                                    decimal cRefundRed = 0;
                                    oSql.Clear();
                                    //oSql.AppendLine("SELECT SUM(ISNULL(FNXrdPntUse,0)) AS FNXrdPntUse  FROM TPSTSalRD WITH(NOLOCK) WHERE FTBchCode =  '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                                    oSql.AppendLine("SELECT SUM(ISNULL(FNXrdPntUse,0)) AS FNXrdPntUse  FROM " + tC_TblSalRD + " WITH(NOLOCK) WHERE FTBchCode =  '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                                    cRefundRed = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                                    //# แต้มที่ต้องคืนร้าน
                                    nStartY += 18;
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)cCstPiontB4Used, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY); //*Arm 63-04-29
                                    decimal cRefundPnt = ((cRefundRed - oHDCst.FCXshCstPnt) * (-1)); //*Arm 63-05-09
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cRefundPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, oHDCst.FCXshCstPntPmt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                                    cSumPnt = (cCstPiontB4Used + (cRefundPnt + oHDCst.FCXshCstPntPmt)); //*Arm 63-05-09
                                    oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cSumPnt, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY); //*Arm 63-04-29

                                    cCstPiontB4Used = cSumPnt;

                                    //# แต้มที่ได้รับคืน
                                    //cPntRcv = cRefundRed;

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
                                //+++++++++++++++++++++
                            }

                            oTxnSale = null;
                        }

                        //if (!string.IsNullOrEmpty(oHDCst.FTXshDocNo)) //*Arm 63-04-03 
                        //{
                        //    //*Arm 63-04-03 Point ทีใช้ทั้งหมด
                        //    cPntUse = 0;
                        //    oSql = new StringBuilder();
                        //    oSql.AppendLine("select ISNULL(SUM(FNXrdPntUse),0) From TPSTSalRD WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'  ");
                        //    cPntUse = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                        //    //หา Transaction การขาย
                        //    oSql = new StringBuilder();
                        //    oSql.AppendLine("SELECT * FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + tC_TxnRefCode + "'");
                        //    cmlTCNTMemTxnSale oTxnSale = oDB.C_GEToDataQuery<cmlTCNTMemTxnSale>(oSql.ToString());

                        //    //Print ข้อมมูล Member
                        //    if (!string.IsNullOrEmpty(oTxnSale.FTMemCode))
                        //    {
                        //        nStartY += 18;
                        //        oGraphic.DrawString("Mem ID : " + oTxnSale.FTMemCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                        //        nStartY += 18;
                        //        oSql = new StringBuilder();
                        //        oGraphic.DrawString(oHDCst.FTXshCstName, cVB.aoVB_PInvLayout[1], Brushes.Black, 10, nStartY);

                        //        string tCardNo = oHDCst.FTXshCardNo == null ? " - " : oHDCst.FTXshCardNo;
                        //        nStartY += 18;
                        //        oGraphic.DrawString("Card No. : " + tCardNo, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

                        //        nStartY += 18;
                        //        if (oTxnSale.FDTxnPntExpired != null)
                        //        {
                        //            oGraphic.DrawString("Expired : " + string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oTxnSale.FDTxnPntExpired)), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        //        }
                        //        else
                        //        {
                        //            oGraphic.DrawString("Expired : -", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        //        }

                        //        nStartY += 18;
                        //        oGraphic.DrawString("Last Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        //        oGraphic.DrawString("Reg. Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                        //        oGraphic.DrawString("Promo Pt.", cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                        //        oGraphic.DrawString("Total Point", cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);


                        //        nStartY += 18;
                        //        if (nC_PrnDocType == 9)
                        //        {
                        //            //คำนวณ Point Req. ถ้าเป็นบิลคืน คำนวณแต้มที่ได้รับ โดยใช้เงื่อนไขการได้จากบิลขายที่อ้างอิง
                        //            oSql = new StringBuilder();
                        //            oSql.AppendLine("SELECT FCTxnPntOptBuyAmt FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + oTxnSale.FTTxnRefInt + "'");
                        //            decimal cPntOptBuyAmt = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                        //            oSql = new StringBuilder();
                        //            oSql.AppendLine("SELECT FCTxnPntOptGetQty FROM TCNTMemTxnSale WHERE FTTxnRefDoc = '" + oTxnSale.FTTxnRefInt + "'");
                        //            decimal cPntOptGetQty = oDB.C_GEToDataQuery<decimal>(oSql.ToString());

                        //            if (oHD.FCXshGrand > 0)
                        //            {
                        //                cPntRcv = cPntUse - (Math.Floor((decimal)oHD.FCXshGrand / cPntOptBuyAmt) * cPntOptGetQty);
                        //            }
                        //            else
                        //            {
                        //                cPntRcv = 0;
                        //            }

                        //        }
                        //        else
                        //        {
                        //            //คำนวณ Point Req. ถ้าเป็นบิลขายเอาข้อมูล Pont ที่ได้รับมาลบกับ Point ใช้แลกไป
                        //            cPntRcv = (decimal)oTxnSale.FCTxnPntBillQty - cPntUse;
                        //        }
                        //        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, (decimal)oTxnSale.FCTxnPntB4Bill, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                        //        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, cPntRcv, 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 70, nStartY);
                        //        oGraphic.DrawString("-", cVB.aoVB_PInvLayout[1], Brushes.Black, 140, nStartY);
                        //        oGraphic.DrawString(oSP.SP_SETtDecShwSve(1, Convert.ToDecimal(oTxnSale.FCTxnPntB4Bill + cPntRcv), 0), cVB.aoVB_PInvLayout[1], Brushes.Black, 210, nStartY);

                        //    }
                        //    oTxnSale = null;
                        //}
                    }
                }

                //oHDCst = null; //*Arm 63-04-20 Comment Code
                ////+++++++++++++

                //*Arm 63-05-08 Print ชื่อผู้ขาย
                oSql.Clear();
                oSql.AppendLine("SELECT FTUsrName FROM TCNMUser_L with(nolock) WHERE FTUsrCode ='" + oHD.FTUsrCode + "' ");
                string tUserName = oDB.C_GEToDataQuery<string>(oSql.ToString());
                nStartY += 30;
                oGraphic.DrawString("User : " + tUserName, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                //+++++++++++++

                //if (nC_DocType == 9)
                if (nC_PrnDocType == 9)
                {
                    nStartY += 30;
                    tMsg = cVB.oVB_GBResource.GetString("tRefundPdt");
                    if (!string.IsNullOrEmpty(tC_RefDocNoPrn))
                    {
                        tMsg += cVB.oVB_GBResource.GetString("tRefer") + ":" + tC_RefDocNoPrn;
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
                if (bC_PrnCopy) //*Net 63-02-25 เมื่อพิมพ์บิลคืน ให้พิมพ์ สำเนา
                {
                    //สำเนา
                    nStartY += 18;
                    tPrint = "!!! " + cVB.oVB_GBResource.GetString("tCopy") + " " + oHD.FNXshDocPrint + " !!!";
                    oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                    //++++++++++++++
                }

                //Remark
                if (!string.IsNullOrEmpty(oHD.FTXshRmk))
                {
                    nStartY += 18;
                    tPrint = oHD.FTXshRmk;
                    aRmk = tPrint.Split((char)10);
                    foreach (string tStr in aRmk)
                    {
                        nStartY += 18;
                        oGraphic.DrawString(tStr, cVB.aoVB_PInvLayout[4], Brushes.Black, 0, nStartY);
                    }
                }
                //*Arm 62-10-31  เงื่อนไขการแสดง Barcode และ QRCode
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
                        tDataB = cVB.tVB_DocNoPrn + "|" + oHD.FTCstCode == null ? "" : oHD.FTCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //*Arm 63-04-20
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
                        tDataB = cVB.tVB_DocNoPrn + "|" + oHD.FTCstCode == null ? "" : oHD.FTCstCode + "|" + tCstTel + "|" + string.Format("{0:##########.00}", oHD.FCXshGrand) + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //*Arm 63-04-20
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

                //*Em 63-04-09
                //พิมพ์สิทธิ์
                oSql.Clear();
                oSql.AppendLine("SELECT DISTINCT FTPmhDocNo,FTXpdCpnText,FCXpdGetQtyDiv ");
                //oSql.AppendLine("FROM TPSTSalPD WITH(NOLOCK)");
                oSql.AppendLine("FROM " + tC_TblSalPD + " WITH(NOLOCK)"); //*Em 63-05-16
                oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                oSql.AppendLine("AND ISNULL(FTXpdCpnText,'') <> ''");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                if (odtTmp != null)
                {
                    foreach (DataRow oRow in odtTmp.Rows)
                    {
                        nStartY += 18;
                        oGraphic.DrawString(tLine, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);

                        nStartY += 18;
                        tPrint = oRow.Field<string>("FTXpdCpnText") + " " + Convert.ToInt16(oRow.Field<Decimal>("FCXpdGetQtyDiv")).ToString() + " " + cVB.oVB_GBResource.GetString("tPrivilege");
                        oGraphic.DrawString(tPrint, cVB.aoVB_PInvLayout[5], Brushes.Black, new RectangleF(0, nStartY, nWidth, 18), oFormatCenter);
                    }
                }

                //พิมพ์คูปอง
                oSql.Clear();
                //oSql.AppendLine("SELECT DISTINCT FTCpdBarCpn FROM TPSTSalPD WITH(NOLOCK)");
                oSql.AppendLine("SELECT DISTINCT FTCpdBarCpn FROM " + tC_TblSalPD + " WITH(NOLOCK)"); //*Em 63-05-16
                oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                oSql.AppendLine("AND ISNULL(FTCpdBarCpn,'') <> ''");
                odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT DISTINCT PD.FTPmhDocNo,PD.FTCpdBarCpn,PD.FCXpdGetQtyDiv ,ISNULL(IMG.FTImgObj,'') AS FTImgPath");
                    //oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
                    oSql.AppendLine("FROM " + tC_TblSalPD + " PD WITH(NOLOCK)"); //*Em 63-05-16 
                    oSql.AppendLine("LEFT JOIN TCNTPdtPmtCG CG WITH(NOLOCK) ON CG.FTPmhDocNo = PD.FTPmhDocNo AND FTPgtStaGetType = '6'");
                    oSql.AppendLine("LEFT JOIN TCNMImgObj IMG WITH(NOLOCK) ON IMG.FTImgTable = 'TFNTCouponHD' AND IMG.FTImgRefID = CG.FTCphDocNo");
                    oSql.AppendLine("WHERE PD.FTBchCode =  '" + cVB.tVB_BchCode + "'");
                    oSql.AppendLine("AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    oSql.AppendLine("AND ISNULL(PD.FTCpdBarCpn,'') <> ''");
                    odtTmp = oDB.C_GEToDataQuery(oSql.ToString());
                    if (odtTmp != null)
                    {
                        foreach (DataRow oRow in odtTmp.Rows)
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
                }
                //++++++++++++++++++++

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
                if (oGraphic != null)
                    oGraphic.Dispose();

                if (oFormatFar != null)
                    oFormatFar.Dispose();

                if (oFormatCenter != null)
                    oFormatCenter.Dispose();
                oLogo = null;
                oGraphic = null;
                oFormatFar = null;
                oFormatCenter = null;
                oMsg = null;
                tLine = null;
                odtTmp = null;  //*Em 63-04-09
                oHDCst = null; //*Arm 63-04-20
                oSP.SP_CLExMemory();
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
            try
            {
                oFormatFar = new StringFormat() { Alignment = StringAlignment.Far };
                oFormatCenter = new StringFormat() { Alignment = StringAlignment.Center };

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


                //++++++ *Arm 63-05-05  สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต +++++++
                if (cVB.nVB_StaSumPrn == 1) // 1:อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                {
                    oSql.Clear();
                    if (pbTrans)
                    {
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
                        //oSql.AppendLine("                            WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' AND FTXpdGetType = '4'");
                        //oSql.AppendLine("                            GROUP BY FTBchCode, FTXshDocNo, FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                        //oSql.AppendLine("                WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' AND PD.FTXpdGetType = '4') PD");
                        //oSql.AppendLine("        ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                        oSql.AppendLine("    WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("    AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' ");

                    }
                    else
                    {
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
                        oSql.AppendLine("FROM(SELECT(SELECT TOP 1 FNXsdSeqNo FROM " + tC_TblSalDT + " WITH(NOLOCK) WHERE FTBchCode = DT.FTBchCode AND FTXshDocNo = DT.FTXshDocNo AND FTPdtCode = DT.FTPdtCode AND  FTXsdBarCode = DT.FTXsdBarCode ORDER BY FNXsdSeqNo ASC) AS FNXsdSeqNo,");
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
                        oSql.AppendLine("    FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                        //oSql.AppendLine("    LEFT JOIN(SELECT PD.FTBchCode, PD.FTXshDocNo, PD.FNXsdSeqNo, PD.FCXsdQty, PD.FCXsdSetPrice");
                        //oSql.AppendLine("                FROM " + tC_TblSalPD + " PD WITH(NOLOCK)");
                        //oSql.AppendLine("                INNER JOIN(SELECT FTBchCode, FTXshDocNo, FNXsdSeqNo, MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                        //oSql.AppendLine("                            FROM " + tC_TblSalPD + " WITH(NOLOCK)");
                        //oSql.AppendLine("                            WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' AND FTXpdGetType = '4'");
                        //oSql.AppendLine("                            GROUP BY FTBchCode, FTXshDocNo, FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                        //oSql.AppendLine("                WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' AND PD.FTXpdGetType = '4') PD");
                        //oSql.AppendLine("        ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                        oSql.AppendLine("    WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("    AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' ");

                    }

                    if (cVB.bVB_AlwPrnVoid == false)
                    {
                        oSql.AppendLine(" AND DT.FTXsdStaPdt <> '4' ");
                    }

                    oSql.AppendLine(") AS DT1 ");
                    oSql.AppendLine("GROUP BY DT1.FNXsdSeqNo, DT1.FTPdtCode,DT1.FTXsdPdtName,DT1.FTXsdBarCode, ");
                    oSql.AppendLine("DT1.FCXsdSetPrice,DT1.FTXsdVatType ");
                    oSql.AppendLine("ORDER BY DT1.FNXsdSeqNo ASC ");

                    aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());


                    if (pbTrans) //*Arm 63-05-17
                    {
                        oSql.Clear();
                        oSql.AppendLine("SELECT (SELECT FTXsdBarCode FROM TPSTSalDT WHERE FTBchCode = DTDis.FTBchCode AND FTXshDocNo = DTDis.FTXshDocNo AND FNXsdSeqNo = DTDis.FNXsdSeqNo ) AS FTXsdBarCode,");
                        oSql.AppendLine("DTDis.FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                        oSql.AppendLine("FROM TPSTSalDTDis DTDis");
                        oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                        oSql.AppendLine("AND FNXddStaDis = 1");
                        oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
                        oSql.AppendLine("ORDER BY FDXddDateIns");
                    }
                    else
                    {
                        oSql.Clear();
                        oSql.AppendLine("SELECT (SELECT FTXsdBarCode FROM " + tC_TblSalDT + " WHERE FTBchCode = DTDis.FTBchCode AND FTXshDocNo = DTDis.FTXshDocNo AND FNXsdSeqNo = DTDis.FNXsdSeqNo ) AS FTXsdBarCode,");
                        oSql.AppendLine("DTDis.FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                        oSql.AppendLine("FROM " + tC_TblSalDTDis + " DTDis"); //*Arm 63-05-17
                        oSql.AppendLine("WHERE DTDis.FTBchCode = '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("AND DTDis.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                        oSql.AppendLine("AND FNXddStaDis = 1");
                        oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");
                        oSql.AppendLine("ORDER BY FDXddDateIns");
                    }
                    //+++++++++++++++

                    aoPrnDTDis = oDB.C_GETaDataQuery<cmlPrnSplipDTDis>(oSql.ToString());

                    if (aoDT != null)
                    {
                        foreach (cmlTPSTSalDT oDT in aoDT)
                        {
                            switch (cVB.nVB_ChkShowPdtBarCode)
                            {
                                case 1:
                                    pnStartY += 18;
                                    //poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));     //*Arm 63-05-12
                                    break;
                                case 2:
                                    pnStartY += 18;
                                    //poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));  //*Arm 63-05-12
                                    break;

                                default:
                                    pnStartY += 18;
                                    //poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));   //*Arm 63-05-12
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
                            //*Arm 63-05-12
                            if (oDT.FCXsdQty > 1)
                            {
                                pnStartY += 18;
                                poGraphic.DrawString("  " + oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            else
                            {
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            //+++++++++++++

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
                                        pnStartY += 18;
                                        poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " (" + cDis + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                        cAmt = (cAmt - cDis);
                                        tAmt = oSP.SP_SETtDecShwSve(1, cAmt, cVB.nVB_DecShow);
                                        //poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                        poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                    }

                                    if (cChg > 0)   // แสดงชาจน์
                                    {
                                        pnStartY += 18;
                                        poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tChg") + " (" + cChg + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
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
                    // 2 :ไม่อนุญาตให้รวมรายการสินค้าตอนพิมพ์
                    //*Em 63-04-29
                    if (pbTrans)
                    {
                        oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FCXsdQty,");
                        oSql.AppendLine("DT.FTPdtCode,DT.FTXsdBarCode,");
                        oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdNet ,DT.FTXsdVatType,DT.FCXsdAmtB4DisChg");
                        //oSql.AppendLine("ISNULL(PD.FCXsdQty,DT.FCXsdQty) AS FCXsdQtyAll,ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice) AS FCXsdSalePrice");
                        oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                        //oSql.AppendLine("LEFT JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdQty,PD.FCXsdSetPrice");
                        //oSql.AppendLine("   FROM TPSTSalPD PD WITH(NOLOCK)");
                        //oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                        //oSql.AppendLine("   	FROM TPSTSalPD WITH(NOLOCK)");
                        //oSql.AppendLine("   	WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' AND FTXpdGetType = '4'");
                        //oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                        //oSql.AppendLine("   WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' AND PD.FTXpdGetType = '4') PD");
                        //oSql.AppendLine("   ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                        oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    }
                    else
                    {
                        oSql.AppendLine("SELECT DT.FNXsdSeqNo,DT.FTPdtCode,DT.FTXsdPdtName,DT.FTXsdBarCode,DT.FCXsdQty,");
                        oSql.AppendLine("DT.FTPdtCode,DT.FTXsdBarCode,");
                        oSql.AppendLine("DT.FCXsdSetPrice,DT.FCXsdNet ,DT.FTXsdVatType,DT.FCXsdAmtB4DisChg");
                        //oSql.AppendLine("ISNULL(PD.FCXsdQty,DT.FCXsdQty) AS FCXsdQtyAll,ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice) AS FCXsdSalePrice");
                        oSql.AppendLine("FROM " + tC_TblSalDT + " DT WITH(NOLOCK)");
                        //oSql.AppendLine("LEFT JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdQty,PD.FCXsdSetPrice");
                        //oSql.AppendLine("   FROM " + tC_TblSalPD + " PD WITH(NOLOCK)");
                        //oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                        //oSql.AppendLine("   	FROM " + tC_TblSalPD + " WITH(NOLOCK)");
                        //oSql.AppendLine("   	WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' AND FTXpdGetType = '4'");
                        //oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                        //oSql.AppendLine("   WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "' AND PD.FTXpdGetType = '4') PD");
                        //oSql.AppendLine("   ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                        oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("AND DT.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    }
                    //+++++++++++++++++

                    //*Em 63-03-28
                    if (cVB.bVB_AlwPrnVoid == false)
                    {
                        oSql.AppendLine(" AND DT.FTXsdStaPdt <> '4' ");
                    }
                    //+++++++++++++++++++++

                    oSql.AppendLine($"ORDER BY DT.FNXsdSeqNo");
                    ///////////////////////////////////////////////////////
                    aoDT = oDB.C_GETaDataQuery<cmlTPSTSalDT>(oSql.ToString());


                    if (pbTrans) //*Arm 63-05-17
                    {
                        oSql.Clear();
                        oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                        oSql.AppendLine("FROM TPSTSalDTDis WITH(NOLOCK)"); //*Arm 63-03-05
                        oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
                        oSql.AppendLine("AND FNXddStaDis = 1");
                        oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");    //*Em 63-03-30
                        oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");
                    }
                    else
                    {
                        oSql.Clear();
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                        oSql.AppendLine("FROM " + tC_TblSalDTDis + " DTDis WITH(NOLOCK)"); //*Arm 63-05-17
                        oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                        oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
                        oSql.AppendLine("AND FNXddStaDis = 1");
                        oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");    //*Em 63-03-30
                        oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");

                    }

                    //oSql = new StringBuilder();
                    //oSql.AppendLine("SELECT FNXsdSeqNo,FTXddDisChgTxt,FTXddDisChgType,FCXddNet,FCXddValue");
                    ////oSql.AppendLine("FROM " + tC_TblSalDTDis);
                    //oSql.AppendLine("FROM TPSTSalDTDis WITH(NOLOCK)"); //*Arm 63-03-05
                    //oSql.AppendLine("WHERE FTBchCode =  '" + cVB.tVB_BchCode + "'");
                    ////oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNo + "'");
                    //oSql.AppendLine("AND FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'"); //*Arm 63-03-05 
                    //oSql.AppendLine("AND FNXddStaDis = 1");
                    //oSql.AppendLine("AND ISNULL(FTXddRefCode,'') = ''");    //*Em 63-03-30
                    //oSql.AppendLine("ORDER BY FNXsdSeqNo,FDXddDateIns");

                    aoDTDis = oDB.C_GETaDataQuery<cmlTPSTSalDTDis>(oSql.ToString());

                    if (aoDT != null)
                    {
                        foreach (cmlTPSTSalDT oDT in aoDT)
                        {
                            //*Arm 63-04-13 Comment Code
                            //pnStartY += 18;
                            //poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);

                            ////if (oDT.FTXsdVatType.ToString() == "1")
                            //if (oDT.FTXsdVatType != null || oDT.FTXsdVatType == "1")     //*Em 63-04-05
                            //{
                            //    tVat = "V";
                            //}
                            //else
                            //{
                            //    tVat = " ";
                            //}

                            //if (oDT.FCXsdQty > 1)
                            //{
                            //    pnStartY += 18;
                            //    poGraphic.DrawString(oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                            //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                            //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //}
                            //else
                            //{
                            //    tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                            //    poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                            //    poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140 + pnWidth - 150, pnStartY, 10, 18), oFormatCenter);
                            //}

                            //+++++++++++++++

                            //*Arm 63-04-13 พิมพ์รหัสสินค้าหรือบาร์โค้ดในแสดงสลิป 0:ไม่แสดง, 1:แสดง Product Code, 2:แสดง Barcode
                            switch (cVB.nVB_ChkShowPdtBarCode)
                            {
                                case 1:
                                    pnStartY += 18;
                                    //poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    poGraphic.DrawString(oDT.FTPdtCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));     //*Arm 63-05-12
                                    break;
                                case 2:
                                    pnStartY += 18;
                                    //poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    poGraphic.DrawString(oDT.FTXsdBarCode + " " + oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));     //*Arm 63-05-12
                                    break;

                                default:
                                    pnStartY += 18;
                                    //poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                    poGraphic.DrawString(oDT.FTXsdPdtName, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 70, 18));     //*Arm 63-05-12
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

                            //*Arm 63-05-12
                            if (oDT.FCXsdQty > 1)
                            {
                                pnStartY += 18;
                                poGraphic.DrawString("  " + oDT.FCXsdQty + " x " + oDT.FCXsdSetPrice, cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            else
                            {
                                tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oDT.FCXsdAmtB4DisChg, cVB.nVB_DecShow);
                                poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);
                                poGraphic.DrawString(tVat, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF((pnWidth - 70) + 60, pnStartY, 10, 18), oFormatCenter);
                            }
                            //+++++++++++++

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

                            //+++++++++++++
                            if (aoDTDis != null)
                            {
                                foreach (cmlTPSTSalDTDis oDTDis in aoDTDis.Where(c => c.FNXsdSeqNo == oDT.FNXsdSeqNo))
                                {
                                    pnStartY += 18;
                                    switch (oDTDis.FTXddDisChgType)
                                    {
                                        case "1":
                                        case "2":
                                            poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " (" + oDTDis.FTXddDisChgTxt + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDTDis.FCXddNet - oDTDis.FCXddValue), cVB.nVB_DecShow);
                                            //poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(140, pnStartY, pnWidth - 150, 18), oFormatFar);
                                            poGraphic.DrawString(tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar);      //*Arm 63-05-12
                                            break;
                                        case "3":
                                        case "4":
                                            poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tChg") + " (" + oDTDis.FTXddDisChgTxt + ")", cVB.aoVB_PInvLayout[2], Brushes.Black, 0, pnStartY);
                                            tAmt = oSP.SP_SETtDecShwSve(1, (decimal)(oDTDis.FCXddNet + oDTDis.FCXddValue), cVB.nVB_DecShow);
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
                oSql.Clear();
                if (pbTrans)
                {
                    oSql.AppendLine("SELECT (CASE WHEN ISNULL(HDL.FTPmhNameSlip,'') = '' THEN HDL.FTPmhName ELSE HDL.FTPmhNameSlip END) AS FTPmdGrpName,PD.FCXpdDis");
                    oSql.AppendLine("FROM TPSTSalPD PD WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TPMTPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo");
                    oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    oSql.AppendLine("AND PD.FTXpdCpnText = '' AND PD.FTCpdBarCpn = ''");
                    oSql.AppendLine("AND PD.FCXpdDis <> 0 ");   //*Em 63-04-12
                    oSql.AppendLine("AND PD.FTXpdGetType <> '4' "); //*Em 63-04-29
                    oSql.AppendLine("GROUP BY HDL.FTPmhNameSlip,HDL.FTPmhName,PD.FCXpdDis");
                }
                else
                {
                    oSql.AppendLine("SELECT (CASE WHEN ISNULL(HDL.FTPmhNameSlip,'') = '' THEN HDL.FTPmhName ELSE HDL.FTPmhNameSlip END) AS FTPmdGrpName,PD.FCXpdDis");
                    oSql.AppendLine("FROM " + tC_TblSalPD + " PD WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TPMTPmtHD_L HDL WITH(NOLOCK) ON HDL.FTPmhDocNo = PD.FTPmhDocNo");
                    oSql.AppendLine("WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + cVB.tVB_DocNoPrn + "'");
                    oSql.AppendLine("AND PD.FTXpdCpnText = '' AND PD.FTCpdBarCpn = ''");
                    oSql.AppendLine("AND PD.FCXpdDis <> 0 ");   //*Em 63-04-12
                    oSql.AppendLine("AND PD.FTXpdGetType <> '4' "); //*Em 63-04-29
                    oSql.AppendLine("GROUP BY HDL.FTPmhNameSlip,HDL.FTPmhName,PD.FCXpdDis");
                }
                aoPD = oDB.C_GETaDataQuery<cmlTPSTSalPD>(oSql.ToString());
                if (aoPD != null && aoPD.Count > 0)
                {
                    foreach (cmlTPSTSalPD oPD in aoPD)
                    {
                        pnStartY += 18;
                        poGraphic.DrawString("  " + cVB.oVB_GBResource.GetString("tDis") + " " + oPD.FTPmdGrpName, cVB.aoVB_PInvLayout[2], Brushes.Black, new RectangleF(0, pnStartY, pnWidth - 100, 18));

                        tAmt = oSP.SP_SETtDecShwSve(1, (decimal)oPD.FCXpdDis, cVB.nVB_DecShow);
                        //poGraphic.DrawString("-" + tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 100, pnStartY, 90, 18), oFormatFar);
                        poGraphic.DrawString("-" + tAmt, cVB.aoVB_PInvLayout[3], Brushes.Black, new RectangleF(pnWidth - 70, pnStartY, 60, 18), oFormatFar); //*Arm 63-05-12

                    }
                }
                //+++++++++++++++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSale", "C_PRNxPrintDT : " + oEx.Message); }
            finally
            {
                oDB = null;
                oSql = null;
                new cSP().SP_CLExMemory();
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
                oSP.SP_CLExMemory();
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
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 15;
                oGraphic.DrawString(Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm") + " BNO:" + cVB.tVB_DocNo,
                                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

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
                oSP.SP_CLExMemory();
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
                oSP.SP_CLExMemory();
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
                oGraphic.DrawString("ID:" + cVB.tVB_PosRegNo.PadRight(30) + "USR: " + cVB.tVB_UsrCode + " T: " + cVB.tVB_PosCode, cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);
                nStartY += 15;
                oGraphic.DrawString(Convert.ToDateTime(cVB.tVB_SaleDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm") + " BNO:" + cVB.tVB_DocNo,
                                    cVB.aoVB_PInvLayout[1], Brushes.Black, 0, nStartY);

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
                oSP.SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
            }
            return 0m;
        }
        #endregion Print
    }
}
