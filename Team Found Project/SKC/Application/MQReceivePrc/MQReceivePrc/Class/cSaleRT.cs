using MQReceivePrc.Class;
using MQReceivePrc.Models.Webservice.Response;
using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.SaleRT.AdminHis;
using MQReceivePrc.Models.SaleRT.RentalPay;
using MQReceivePrc.Models.SaleRT.RentalSale;
using MQReceivePrc.Models.SaleRT.RentalBooking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    class cSaleRT
    {
        private cmlShopDB oC_ShopDB;

        /// <summary>
        /// Function Upload Booking
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbUploadBooking(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)       //*BOY 62-10-31
        {
            cDataReader<cmlTRTTBooking> aoBooking;
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlRTBooking oRTBooking;
            SqlTransaction oTransaction;
            SqlConnection oConn;

            try
            {
                if (poData == null) return false; // ถ้า poData = ค่าว่าง return false -->  
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oRTBooking = JsonConvert.DeserializeObject<cmlRTBooking>(poData.ptData);     // แปลงข้อมูล JSON  แล้วเก็บใน clase cmlRTSale
                //Create Temp
                //BOOKING
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTBookingTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTBookingTmp FROM TRTTBooking WITH(NOLOCK)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTBookingTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTBooking' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTBookingTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTBookingTmp FROM TRTTBooking WITH(NOLOCK)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTBookingTmp");
                oSql.AppendLine("");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTransaction = oConn.BeginTransaction();

                //insert to DB
                if (oRTBooking.aoTRTTBooking != null)
                {
                    aoBooking = new cDataReader<cmlTRTTBooking>(oRTBooking.aoTRTTBooking);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoBooking.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.[TRTTBookingTmp]";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoBooking);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                oTransaction.Commit();

                oSql = new StringBuilder();
                // StringBuilder
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE Booking ");
                oSql.AppendLine("   FROM TRTTBooking Booking WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTBookingTmp TMPBooking WITH(NOLOCK) ON Booking.FTBchCode = TMPBooking.FTBchCode");
                oSql.AppendLine("   AND Booking.FTBkgToBch = TMPBooking.FTBkgToBch AND Booking.FNBkgToLayNo = TMPBooking.FNBkgToLayNo ");
                oSql.AppendLine("   AND Booking.FTBkgToSize = TMPBooking.FTBkgToSize AND Booking.FTBkgToRate = TMPBooking.FTBkgToRate ");
                //oSql.AppendLine("   AND Booking.FTBkgRefCstLogin = TMPBooking.FTBkgRefCstLogin AND Booking.FTBkgRefSale = TMPBooking.FTBkgRefSale ");
                oSql.AppendLine("   AND Booking.FTBkgRefCstLogin = TMPBooking.FTBkgRefCstLogin ");  //*Em 62-12-02
                oSql.AppendLine("   AND Booking.FTBkgProducer = TMPBooking.FTBkgProducer AND Booking.FTUsrCode = TMPBooking.FTUsrCode");    //*Em 62-12-02
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTBooking");
                oSql.AppendLine("   SELECT FTAgnCode, FTBchCode, FTBkgProducer, FTUsrCode, FTBkgToBch, FTBkgToPos, FNBkgToLayNo, FTBkgToSize, FTBkgToRate, FDBkgToStart, FTBkgRefCst, FTBkgRefCstLogin, FTBkgRefCstDoc, FTBkgRefSale, FTBkgStaBook, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy FROM TRTTBookingTmp WITH(NOLOCK)");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");

                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                return true;

            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadBooking");      // ส่งไป Class cFunction ที่ Function C_LOGxKeepLogErr เพื่อ แสดง Log
                return false;
            }
            finally
            {
                oSql = null;
                oTransaction = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }
        }



        /// <summary>
        /// Function Upload Sale RT
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbUploadSaleRT(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)        //*Arm 62-09-25
        {
            cDataReader<cmlTRTTSalHD> aoHD;
            cDataReader<cmlTRTTSalDT> aoDT;
            cDataReader<cmlTRTTSalDTSN> aoDTSN;
            cDataReader<cmlTRTTSalDTSL> aoDTSL;
            
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlRTSale oSalePos;
            SqlTransaction oTransaction;
            SqlConnection oConn;
            cSP oSP = new cSP();

            try
            {
                if (poData == null) return false; // ถ้า poData = ค่าว่าง return false -->
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oSalePos = JsonConvert.DeserializeObject<cmlRTSale>(poData.ptData);     // แปลงข้อมูล JSON  แล้วเก็บใน clase cmlRTSale

                //Create Temp
                #region Create Temp
                //HD
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTSalHDTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTSalHDTmp FROM TRTTSalHD with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTSalHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTSalHD' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTSalHDTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTSalHDTmp FROM TRTTSalHD with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTSalHDTmp");
                oSql.AppendLine("");

                //DT
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTSalDTTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTSalDTTmp FROM TRTTSalDT with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTSalDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTSalDT' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTSalDTTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTSalDTTmp FROM TRTTSalDT with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTSalDTTmp");
                oSql.AppendLine("");

                //DTSN
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTSalDTSNTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTSalDTSNTmp FROM TRTTSalDTSN with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTSalDTSNTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTSalDTSN' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTSalDTSNTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTSalDTSNTmp FROM TRTTSalDTSN with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTSalDTSNTmp");
                oSql.AppendLine("");

                //DTSL
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTSalDTSLTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTSalDTSLTmp FROM TRTTSalDTSL with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTSalDTSLTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTSalDTSL' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTSalDTSLTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTSalDTSLTmp FROM TRTTSalDTSL with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTSalDTSLTmp");
                oSql.AppendLine("");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion Create Temp

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTransaction = oConn.BeginTransaction();

                //insert to DB
                if (oSalePos.oTRTTSalHD != null)
                {
                    List<cmlTRTTSalHD> aoRcvHD = new List<cmlTRTTSalHD>();
                    aoRcvHD.Add(oSalePos.oTRTTSalHD);
                    aoHD = new cDataReader<cmlTRTTSalHD>(aoRcvHD);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.TRTTSalHDTmp";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoHD);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTRTTSalDT != null)
                {
                    aoDT = new cDataReader<cmlTRTTSalDT>(oSalePos.aoTRTTSalDT);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.TRTTSalDTTmp";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoDT);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTRTTSalDTSN != null)
                {
                    aoDTSN = new cDataReader<cmlTRTTSalDTSN>(oSalePos.aoTRTTSalDTSN);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoDTSN.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.TRTTSalDTSNTmp";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoDTSN);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTRTTSalDTSL != null)
                {
                    aoDTSL = new cDataReader<cmlTRTTSalDTSL>(oSalePos.aoTRTTSalDTSL);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoDTSL.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.TRTTSalDTSLTmp";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoDTSL);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                oTransaction.Commit();

                oSql = new StringBuilder();
                // StringBuilder
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE HD ");
                oSql.AppendLine("   FROM TRTTSalHD HD WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTSalHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTSalHD");
                oSql.AppendLine("   SELECT * FROM TRTTSalHDTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TRTTSalDT DT WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTSalDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo AND DT.FNXsdSeqNo = TMPDT.FNXsdSeqNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTSalDT");
                oSql.AppendLine("   SELECT * FROM TRTTSalDTTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTSN ");
                oSql.AppendLine("   FROM TRTTSalDTSN DTSN WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTSalDTSNTmp TMPDTSN WITH(NOLOCK) ON DTSN.FTBchCode = TMPDTSN.FTBchCode AND DTSN.FTXshDocNo = TMPDTSN.FTXshDocNo AND DTSN.FNXsdSeqNo = TMPDTSN.FNXsdSeqNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTSalDTSN");
                oSql.AppendLine("   SELECT * FROM TRTTSalDTSNTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DTSL ");
                oSql.AppendLine("   FROM TRTTSalDTSL DTSL WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTSalDTSLTmp TMPDTSL WITH(NOLOCK) ON DTSL.FTBchCode = TMPDTSL.FTBchCode AND DTSL.FTXshDocNo = TMPDTSL.FTXshDocNo AND DTSL.FNXsdSeqNo = TMPDTSL.FNXsdSeqNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTSalDTSL");
                oSql.AppendLine("   SELECT * FROM TRTTSalDTSLTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);        // ส่งไป Insert

                List<cmlTRTTSalHD> aoTRTTSalHD = new List<cmlTRTTSalHD>();
                aoTRTTSalHD.Add(oSalePos.oTRTTSalHD);
                
                //for (int nRow = 0; nRow < aoTRTTSalHD.Count; nRow++)
                //{
                //    //if (aoTRTTSalHD[nRow].FTBchCode == cVB.tVB_BchCode)     //*Arm 62-11-18 Check ถ้าเป็นสาขาตัวเองให้ทำ Process
                //    if (aoTRTTSalHD[nRow].FTBchCode == cVB.tVB_BchCode || cVB.bVB_StaUseCentralized == true) //*Net 63-04-07 ถ้าเป็น centalized ให้ทำด้วย
                //    {
                //        if (C_PRCbUpdStaLocker(aoTRTTSalHD[nRow].FTXshDocNo, aoTRTTSalHD[nRow].FTBchCode.ToString(), poData.ptConnStr) == false)
                //        {
                //            return false;
                //        }
                //    }
                //}

                //*Arm 63-04-08 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == true) // ใช้งานระบบ Centralized
                {
                    for (int nRow = 0; nRow < aoTRTTSalHD.Count; nRow++)
                    {
                        if (C_PRCbUpdStaLocker(aoTRTTSalHD[nRow].FTXshDocNo, aoTRTTSalHD[nRow].FTBchCode.ToString(), poData.ptConnStr) == false)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // ไม่ใช้งานระบบ Centralized

                    for (int nRow = 0; nRow < aoTRTTSalHD.Count; nRow++)
                    {
                        if (aoTRTTSalHD[nRow].FTBchCode == cVB.tVB_BchCode)     //*Arm 62-11-18 Check ถ้าเป็นสาขาตัวเองให้ทำ Process
                        {
                            if (C_PRCbUpdStaLocker(aoTRTTSalHD[nRow].FTXshDocNo, aoTRTTSalHD[nRow].FTBchCode.ToString(), poData.ptConnStr) == false)
                            {
                                return false;
                            }
                        }
                    }

                    //*Em 62-10-18
                    if (oSP.SP_CHKbIsHQBch(poData.ptConnStr, (int)poShopDB.nCommandTimeOut) == false)
                    {
                        string tAPIUrl = "";
                        string tUrlFunc = "/Service/Sale";
                        string tAPIHeader = "";
                        string tXKey = "";
                        string tBchHQ = "";
                        tBchHQ = oSP.SP_GETtBchHQ(poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                        tAPIUrl = oSP.SP_GETtUrlAPI(poData.ptConnStr, (int)poShopDB.nCommandTimeOut, tBchHQ, 7, ref tXKey, ref tAPIHeader);

                        if (!string.IsNullOrEmpty(tAPIUrl))
                        {
                            string tJSonCall = JsonConvert.SerializeObject(oSalePos);
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(tAPIHeader, tXKey);
                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tAPIUrl + tUrlFunc, tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("cTax", "C_PRCbUploadSaleRT : " + oEx.Message);
                            }

                            if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                                cmlResResult oRes = JsonConvert.DeserializeObject<cmlResResult>(tJSonRes);
                                if (oRes.rtCode == "001")
                                {
                                    //
                                }
                                else
                                {
                                    new cLog().C_WRTxLog("cTax", "C_PRCbUploadSaleRT/ToHQ : " + oRes.rtMsg);
                                }
                            }
                        }
                    }
                    //+++++++++++++++++++
                }

                return true;
                
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadSaleRT");
                return false;
            }
            finally
            {
                aoHD = null;
                aoDT = null;
                aoDTSN = null;
                aoDTSL = null;
                oSql = null;
                oSalePos = null;
                oTransaction = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }
        }

        /// <summary>
        /// Function Upload Sale pay
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbUploadSalePay(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)       //*Arm 62-09-25
        {
            cDataReader<cmlTRTTPayHD> aoHD;
            cDataReader<cmlTRTTPayDT> aoDT;
            cDataReader<cmlTRTTPayRC> aoRC;

            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlRTPay oSalePos;
            SqlTransaction oTransaction;
            SqlConnection oConn;

            try
            {
                if (poData == null) return false; // ถ้า poData = ค่าว่าง return false -->
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oSalePos = JsonConvert.DeserializeObject<cmlRTPay>(poData.ptData);     // แปลงข้อมูล JSON  แล้วเก็บใน clase cmlRTSale

                //Create Temp
                //HD
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTPayHDTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTPayHDTmp FROM TRTTPayHD with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTPayHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTPayHD' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTSalHDTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTPayHDTmp FROM TRTTPayHD with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTPayHDTmp");
                oSql.AppendLine("");

                //DT
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTPayDTTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTPayDTTmp FROM TRTTPayDT with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTPayDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTPayDT' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTPayDTTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTPayDTTmp FROM TRTTPayDT with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTPayDTTmp");
                oSql.AppendLine("");

                //RC
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTPayRCTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTPayRCTmp FROM TRTTPayRC with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTPayRCTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTPayRC' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTPayRCTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTPayRCTmp FROM TRTTPayRC with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTPayRCTmp");
                oSql.AppendLine("");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTransaction = oConn.BeginTransaction();

                //insert to DB
                if (oSalePos.oTRTTPayHD != null)
                {
                    List<cmlTRTTPayHD> aoRvcHD = new List<cmlTRTTPayHD>();
                    aoRvcHD.Add(oSalePos.oTRTTPayHD);
                    aoHD = new cDataReader<cmlTRTTPayHD>(aoRvcHD);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoHD.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.[TRTTPayHDTmp]";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoHD);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTRTTPayDT != null)
                {
                    aoDT = new cDataReader<cmlTRTTPayDT>(oSalePos.aoTRTTPayDT);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.[TRTTPayDTTmp]";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoDT);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                if (oSalePos.aoTRTTPayRC != null)
                {
                    aoRC = new cDataReader<cmlTRTTPayRC>(oSalePos.aoTRTTPayRC);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoRC.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.[TRTTPayRCTmp]";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoRC);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                oTransaction.Commit();

                oSql = new StringBuilder();
                // StringBuilder
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE HD ");
                oSql.AppendLine("   FROM TRTTPayHD HD WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTPayHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTPayHD");
                oSql.AppendLine("   SELECT * FROM TRTTPayHDTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE DT ");
                oSql.AppendLine("   FROM TRTTPayDT DT WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTPayDTTmp TMPDT WITH(NOLOCK) ON DT.FTBchCode = TMPDT.FTBchCode AND DT.FTXshDocNo = TMPDT.FTXshDocNo AND DT.FNXsdSeqNo = TMPDT.FNXsdSeqNo AND DT.FTXsdRefDocNo = TMPDT.FTXsdRefDocNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTPayDT");
                oSql.AppendLine("   SELECT * FROM TRTTPayDTTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   DELETE RC ");
                oSql.AppendLine("   FROM TRTTPayRC RC WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTPayRCTmp TMPRC WITH(NOLOCK) ON RC.FTBchCode = TMPRC.FTBchCode AND RC.FTXshDocNo = TMPRC.FTXshDocNo AND RC.FNXsrSeqNo = TMPRC.FNXsrSeqNo");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTPayRC");
                oSql.AppendLine("   SELECT * FROM TRTTPayRCTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");

                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                
                return true;

            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadSalePay");      // ส่งไป Class cFunction ที่ Function C_LOGxKeepLogErr เพื่อ แสดง Log
                return false;
            }
            finally
            {
                aoHD = null;
                aoDT = null;
                aoRC = null;
                oSql = null;
                oSalePos = null;
                oTransaction = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }
        }

        /// <summary>
        /// Function Upload AdminHis
        /// </summary>
        /// <param name="poData"></param>
        /// <param name="poShopDB"></param>
        /// <param name="ptErrMsg"></param>
        /// <returns></returns>
        public bool C_PRCbUploadAdminHis (cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)     //*Arm 62-09-25
        {
            cDataReader<cmlTRTTAdminHis> aoAH;
            
            StringBuilder oSql = new StringBuilder();
            cDatabase oDB = new cDatabase();
            int nRowAffect = 0;
            cmlRTAdminHis oSalePos;
            SqlTransaction oTransaction;
            SqlConnection oConn;

            try
            {
                if (poData == null) return false; // ถ้า poData = ค่าว่าง return false -->
                if (string.IsNullOrEmpty(poData.ptData)) return false;
                oSalePos = JsonConvert.DeserializeObject<cmlRTAdminHis>(poData.ptData);     // แปลงข้อมูล JSON  แล้วเก็บใน clase cmlRTSale

                //Create Temp
                //AH
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TRTTAdminHisTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TRTTAdminHisTmp FROM TRTTAdminHis with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTAdminHisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TRTTAdminHis' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TRTTAdminHisTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TRTTAdminHisTmp FROM TRTTAdminHis with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TRTTAdminHisTmp");
                oSql.AppendLine("");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTransaction = oConn.BeginTransaction();

                //insert to DB
                if (oSalePos.aoTRTTAdminHis != null)
                {
                    aoAH = new cDataReader<cmlTRTTAdminHis>(oSalePos.aoTRTTAdminHis);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoAH.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;      // Insert ทีละ 100 rows
                        oBulkCopy.DestinationTableName = "dbo.TRTTAdminHisTmp";     // ชื่อตารางใน DB ที่จะ insert ข้อมูลเข้าไป

                        try
                        {
                            oBulkCopy.WriteToServer(aoAH);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                oTransaction.Commit();

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE AH ");
                oSql.AppendLine("   FROM TRTTAdminHis AH WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TRTTAdminHisTmp TMPAH WITH(NOLOCK) ON AH.FTBchCode = TMPAH.FTBchCode AND AH.FTShpCode = TMPAH.FTShpCode AND AH.FTPosCode = TMPAH.FTPosCode AND AH.FDHisDateTime = TMPAH.FDHisDateTime AND AH.FNHisLayNo = TMPAH.FNHisLayNo");// แก้
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TRTTAdminHis");
                oSql.AppendLine("   SELECT * FROM TRTTAdminHisTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUploadAdminHis");      // ส่งไป Class cFunction ที่ Function C_LOGxKeepLogErr เพื่อ แสดง Log
                return false;
            }
            finally
            {
                aoAH = null;
                oSql = null;
                oSalePos = null;
                oTransaction = null;
                oDB = null;
                oConn = null;
                //new cFunction().C_CLExMemory();
            }

        }

        /// <summary>
        /// Function Update Status locker
        /// </summary>
        /// <param name="ptFTXshDocNo"></param>
        /// <param name="pdFDXshDatePick"></param>
        /// <param name="ptFTBchCode"></param>
        /// <param name="ptFTPosCode"></param>
        /// <param name="paSalDTSL"></param>
        /// <returns></returns>
        public bool C_PRCbUpdStaLocker(string ptFTXshDocNo, string ptFTBchCode, string ptConStr)   //*Arm 62-09-25  เพิ่ม function C_PRCbUpdStaLocker
        {
            StringBuilder oSql;
            cDatabase oDatabase;
            //string tStaSL;
            SqlConnection oConn;
            SqlTransaction oTrans = null;
            SqlCommand oCmd;
            Exception oExRef = new Exception();
            //string[] atResult;
            //string tConnStr;
            try
            {
                oSql = new StringBuilder();
                oDatabase = new cDatabase();
                oConn = new SqlConnection();
                oCmd = new SqlCommand();

                //*Em 62-09-18
                SqlParameter[] oPara = new SqlParameter[] {
                    new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = ptFTBchCode},
                    new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = ptFTXshDocNo},
                    new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = "MQReceivePrc"},
                    new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                //tConnStr = ConfigurationManager.ConnectionStrings["ptConStr"].ConnectionString;
                if (oDatabase.C_DATbExecuteStoreProcedure(ptConStr, "STP_PRCxLockerStatus", ref oPara, 60, "@FNResult"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //++++++++++++++++++++++
            }
            catch (Exception oEx)
            {
                if (oTrans != null) { oTrans.Rollback(); }
                return false;
            }
        }
    }
    
}
