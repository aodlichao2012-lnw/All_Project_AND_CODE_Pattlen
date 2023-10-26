using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cTax
    {
        public string C_GENtDocNo(int pnSaleType)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql;
            string tRunning;
            int nMax = 0;
            int nMaxTmp = 0;
            string tDocNo = "";
            try
            {
                string tTblName = "TPSTTaxHD";
                string tFedDocNo = "FTXshDocNo";
                string tDocLeft = "";
                int nDocType = (pnSaleType == 9?5:4);

                //cSale.C_GETtFormatDoc(tTblName, nDocType, DateTime.Now.Date, cVB.tVB_PosCode, cVB.tVB_ShpCode);
                cSale.C_GETtFormatDoc(tTblName, nDocType, DateTime.Now.Date, cVB.tVB_PosCode, cVB.tVB_ShpCode); //*Em 63-02-28

                string tFmt = new string('0', cSale.nC_DocRuningLength);
                tDocLeft = cSale.tC_DocFmtLeft;
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + cSale.nC_DocRuningLength + ") AS FTMax");
                oSql.AppendLine("FROM " + tTblName + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + cSale.nC_DocRuningLength));
                nMax = oDB.C_GEToDataQuery<int>(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RIGHT(ISNULL(MAX(" + tFedDocNo + "),0)," + cSale.nC_DocRuningLength + ") AS FTMax");
                oSql.AppendLine("FROM " + tTblName + " WITH(NOLOCK)");
                oSql.AppendLine("WHERE SUBSTRING(" + tFedDocNo + ",1," + tDocLeft.Length + ") = '" + tDocLeft + "' AND FTBchCode = '" + cVB.tVB_BchCode + "' ");
                oSql.AppendLine("AND LEN(" + tFedDocNo + ") = " + (int)(tDocLeft.Length + cSale.nC_DocRuningLength));
                nMaxTmp = oDB.C_GEToDataQuery<int>(oSql.ToString());
                if (nMaxTmp > nMax) nMax = nMaxTmp;

                tDocNo = cSale.tC_DocFmtLeft + string.Format("{0:" + tFmt + "}", nMax + 1);
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cTax", "C_GENtDocNo : " + oEx.Message); }
            return tDocNo;
        }

        public bool C_DOCbInsertTax(cmlTPSTTaxHD poHD,cmlTPSTTaxHDCst poHDCst,List<cmlTPSTTaxHDDis> paoHDDis,List<cmlTPSTTaxDT> paoDT,List<cmlTPSTTaxDTDis> paoDTDis
                                    ,List<cmlTPSTTaxDTPmt> paoDTPmt,List<cmlTPSTTaxRC> paoRC)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            SqlTransaction oTrans;
            cDataReaderAdapter<cmlTPSTTaxHD> oHD;
            cDataReaderAdapter<cmlTPSTTaxHDCst> oHDCst;
            cDataReaderAdapter<cmlTPSTTaxHDDis> oHDDis;
            cDataReaderAdapter<cmlTPSTTaxDT> oDT;
            cDataReaderAdapter<cmlTPSTTaxDTDis> oDTDis;
            cDataReaderAdapter<cmlTPSTTaxDTPmt> oDTPmt;
            cDataReaderAdapter<cmlTPSTTaxRC> oRC;

            bool bSuccess = false;
            try
            {
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxHDTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxHDTmp FROM TPSTTaxHD with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHD' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxHDTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxHDTmp FROM TPSTTaxHD with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxHDTmp");

                oSql.AppendLine("");
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxHDCstTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxHDCstTmp FROM TPSTTaxHDCst with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDCstTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDCst' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxHDCstTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxHDCstTmp FROM TPSTTaxHDCst with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxHDCstTmp");

                oSql.AppendLine("");
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxHDDisTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxHDDisTmp FROM TPSTTaxHDDis with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxHDDis' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxHDDisTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxHDDisTmp FROM TPSTTaxHDDis with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxHDDisTmp");

                oSql.AppendLine("");
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxDTTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxDTTmp FROM TPSTTaxDT with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDT' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxDTTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxDTTmp FROM TPSTTaxDT with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxDTTmp");

                oSql.AppendLine("");
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxDTDisTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxDTDisTmp FROM TPSTTaxDTDis with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTDisTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTDis' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxDTDisTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxDTDisTmp FROM TPSTTaxDTDis with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxDTDisTmp");

                oSql.AppendLine("");
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxDTPmtTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxDTPmtTmp FROM TPSTTaxDTPmt with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTPmtTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxDTPmt' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxDTPmtTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxDTPmtTmp FROM TPSTTaxDTPmt with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxDTPmtTmp");

                oSql.AppendLine("");
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TPSTTaxRCTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TPSTTaxRCTmp FROM TPSTTaxRC with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxRCTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TPSTTaxRC' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TPSTTaxRCTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TPSTTaxRCTmp FROM TPSTTaxRC with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TPSTTaxRCTmp");
                oDB.C_SETxDataQuery(oSql.ToString());

                oTrans = cVB.oVB_ConnDB.BeginTransaction();
                List<cmlTPSTTaxHD> aHD = new List<cmlTPSTTaxHD>();
                aHD.Add(poHD);
                oHD = new cDataReaderAdapter<cmlTPSTTaxHD>(aHD);
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTrans))
                {
                    foreach (string tColName in oHD.ColumnNames)
                    {
                        oBulkCopy.ColumnMappings.Add(tColName, tColName);
                    }
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDTmp";

                    try
                    {
                        oBulkCopy.WriteToServer(oHD);
                    }
                    catch (Exception oEx)
                    {
                        oTrans.Rollback();
                        new cLog().C_WRTxLog("cTax", "C_DOCbInsertTax/TPSTTaxHDTmp : " + oEx.Message); 
                        return false;
                    }
                }

                if (poHDCst != null)
                {
                    List<cmlTPSTTaxHDCst> aHDCst = new List<cmlTPSTTaxHDCst>();
                    aHDCst.Add(poHDCst);
                    oHDCst = new cDataReaderAdapter<cmlTPSTTaxHDCst>(aHDCst);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTrans))
                    {
                        foreach (string tColName in oHDCst.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }
                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDCstTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(oHDCst);
                        }
                        catch (Exception oEx)
                        {
                            oTrans.Rollback();
                            new cLog().C_WRTxLog("cTax", "C_DOCbInsertTax/TPSTTaxHDCstTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                if (paoHDDis.Count > 0)
                {
                    oHDDis = new cDataReaderAdapter<cmlTPSTTaxHDDis>(paoHDDis);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTrans))
                    {
                        foreach (string tColName in oHDDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }
                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TPSTTaxHDDisTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(oHDDis);
                        }
                        catch (Exception oEx)
                        {
                            oTrans.Rollback();
                            new cLog().C_WRTxLog("cTax", "C_DOCbInsertTax/TPSTTaxHDDisTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                if (paoDT.Count > 0)
                {
                    oDT = new cDataReaderAdapter<cmlTPSTTaxDT>(paoDT);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTrans))
                    {
                        foreach (string tColName in oDT.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }
                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(oDT);
                        }
                        catch (Exception oEx)
                        {
                            oTrans.Rollback();
                            new cLog().C_WRTxLog("cTax", "C_DOCbInsertTax/TPSTTaxDTTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                if (paoDTDis.Count > 0)
                {
                    oDTDis = new cDataReaderAdapter<cmlTPSTTaxDTDis>(paoDTDis);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTrans))
                    {
                        foreach (string tColName in oDTDis.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }
                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTDisTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(oDTDis);
                        }
                        catch (Exception oEx)
                        {
                            oTrans.Rollback();
                            new cLog().C_WRTxLog("cTax", "C_DOCbInsertTax/TPSTTaxDTDisTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                if (paoDTPmt.Count > 0)
                {
                    oDTPmt = new cDataReaderAdapter<cmlTPSTTaxDTPmt>(paoDTPmt);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTrans))
                    {
                        foreach (string tColName in oDTPmt.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }
                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TPSTTaxDTPmtTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(oDTPmt);
                        }
                        catch (Exception oEx)
                        {
                            oTrans.Rollback();
                            new cLog().C_WRTxLog("cTax", "C_DOCbInsertTax/TPSTTaxDTPmtTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }

                if (paoRC.Count > 0)
                {
                    oRC = new cDataReaderAdapter<cmlTPSTTaxRC>(paoRC);
                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.oVB_ConnDB, SqlBulkCopyOptions.Default, oTrans))
                    {
                        foreach (string tColName in oRC.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }
                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TPSTTaxRCTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(oRC);
                        }
                        catch (Exception oEx)
                        {
                            oTrans.Rollback();
                            new cLog().C_WRTxLog("cTax", "C_DOCbInsertTax/TPSTTaxRCTmp : " + oEx.Message);
                            return false;
                        }
                    }
                }
                oTrans.Commit();


                // JOIN DELETE & Insert & Drop Table
                oSql.Clear();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                // TPSTTaxHD
                oSql.AppendLine("DELETE HD ");
                oSql.AppendLine("FROM TPSTTaxHD HD WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN TPSTTaxHDTmp TMP WITH(NOLOCK) ON HD.FTBchCode = TMP.FTBchCode AND HD.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO TPSTTaxHD");
                oSql.AppendLine("SELECT * FROM TPSTTaxHDTmp WITH(NOLOCK) ");
                oSql.AppendLine();

                // TPSTTaxHDCst
                oSql.AppendLine("DELETE HDCst ");
                oSql.AppendLine("FROM TPSTTaxHDCst HDCst WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN TPSTTaxHDCstTmp TMP WITH(NOLOCK) ON HDCst.FTBchCode = TMP.FTBchCode AND HDCst.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO TPSTTaxHDCst");
                oSql.AppendLine("SELECT * FROM TPSTTaxHDCstTmp WITH(NOLOCK) ");
                oSql.AppendLine();

                // TPSTTaxHDDis
                oSql.AppendLine("DELETE HDDis ");
                oSql.AppendLine("FROM TPSTTaxHDDis HDDis WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN TPSTTaxHDDisTmp TMP WITH(NOLOCK) ON HDDis.FTBchCode = TMP.FTBchCode AND HDDis.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO TPSTTaxHDDis");
                oSql.AppendLine("SELECT * FROM TPSTTaxHDDisTmp WITH(NOLOCK) ");
                oSql.AppendLine();

                // TPSTTaxDT
                oSql.AppendLine("DELETE DT ");
                oSql.AppendLine("FROM TPSTTaxDT DT WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN TPSTTaxDTTmp TMP WITH(NOLOCK) ON DT.FTBchCode = TMP.FTBchCode AND DT.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO TPSTTaxDT");
                oSql.AppendLine("SELECT * FROM TPSTTaxDTTmp WITH(NOLOCK) ");
                oSql.AppendLine();

                // TPSTTaxDTDis
                oSql.AppendLine("DELETE DTDis ");
                oSql.AppendLine("FROM TPSTTaxDTDis DTDis WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN TPSTTaxDTDisTmp TMP WITH(NOLOCK) ON DTDis.FTBchCode = TMP.FTBchCode AND DTDis.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO TPSTTaxDTDis");
                oSql.AppendLine("SELECT * FROM TPSTTaxDTDisTmp WITH(NOLOCK) ");
                oSql.AppendLine();

                // TPSTTaxDTPmt
                oSql.AppendLine("DELETE DTPmt ");
                oSql.AppendLine("FROM TPSTTaxDTPmt DTPmt WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN TPSTTaxDTPmtTmp TMP WITH(NOLOCK) ON DTPmt.FTBchCode = TMP.FTBchCode AND DTPmt.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO TPSTTaxDTPmt");
                oSql.AppendLine("SELECT * FROM TPSTTaxDTPmtTmp WITH(NOLOCK) ");
                oSql.AppendLine();

                // TPSTTaxRC
                oSql.AppendLine("DELETE RC ");
                oSql.AppendLine("FROM TPSTTaxRC RC WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN TPSTTaxRCTmp TMP WITH(NOLOCK) ON RC.FTBchCode = TMP.FTBchCode AND RC.FTXshDocNo = TMP.FTXshDocNo");
                oSql.AppendLine();
                oSql.AppendLine("INSERT INTO TPSTTaxRC");
                oSql.AppendLine("SELECT * FROM TPSTTaxRCTmp WITH(NOLOCK) ");
                oSql.AppendLine();

                oSql.AppendLine("COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_SETxDataQuery(oSql.ToString());
                bSuccess = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cTax", "C_DOCbInsertTax : " + oEx.Message); }
            return bSuccess;
        }

        //public bool C_DOCbInsertTaxWithSQL(string ptTaxDocNo, string ptSaleDocNo)
        public bool C_DOCbInsertTaxWithSQL(string ptTaxDocNo, string ptSaleDocNo,bool pbIsTemp) //*Net 63-05-26 เพิ่ม Check ว่าไปเอามาจาก Trans หรือ Temp
        {
            string tTblSalHD;
            string tTblSalHDCst;
            string tTblSalHDDis;
            string tTblSalDT;
            string tTblSalDTDis;
            string tTblSalDTPmt;
            string tTblSalRC;
            string tTblSalRD;
            string tTblSalPD;
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            bool bSuccess = false;
            try
            {
                if(pbIsTemp)
                {
                    tTblSalHD = "TSHD" + cVB.tVB_PosCode;
                    tTblSalHDCst = "TSHDCst" + cVB.tVB_PosCode;
                    tTblSalHDDis = "TSHDDis" + cVB.tVB_PosCode;
                    tTblSalDT = "TSDT" + cVB.tVB_PosCode;
                    tTblSalDTDis = "TSDTDis" + cVB.tVB_PosCode;
                    tTblSalDTPmt = "TSDTPmt" + cVB.tVB_PosCode;
                    tTblSalRC = "TSRC" + cVB.tVB_PosCode;
                    tTblSalRD = "TSRD" + cVB.tVB_PosCode;
                    tTblSalPD = "TSPD" + cVB.tVB_PosCode;
                }
                else
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
                }

        oSql.Clear();//*Net 63-05-26
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                // TPSTTaxHD
                oSql.AppendLine("   INSERT INTO TPSTTaxHD ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("   	(FTBchCode, FTXshDocNo, FTShpCode, FNXshDocType, FDXshDocDate, FTXshCshOrCrd, FTXshVATInOrEx,");
                oSql.AppendLine("   	FTDptCode, FTWahCode, FTPosCode, FTShfCode, FNSdtSeqNo, FTUsrCode, FTSpnCode, ");
                oSql.AppendLine("   	FTXshApvCode, FTCstCode, FTXshDocVatFull, FTXshRefExt, FDXshRefExtDate, FTXshRefInt, FDXshRefIntDate, ");
                oSql.AppendLine("   	FTXshRefAE, FNXshDocPrint, FTRteCode, FCXshRteFac, FCXshTotal, FCXshTotalNV, FCXshTotalNoDis, ");
                oSql.AppendLine("   	FCXshTotalB4DisChgV, FCXshTotalB4DisChgNV, FTXshDisChgTxt, FCXshDis, FCXshChg, FCXshTotalAfDisChgV, FCXshTotalAfDisChgNV, ");
                oSql.AppendLine("   	FCXshRefAEAmt, FCXshAmtV, FCXshAmtNV, FCXshVat, FCXshVatable, FTXshWpCode, FCXshWpTax, ");
                oSql.AppendLine("   	FCXshGrand, FCXshRnd, FTXshGndText, FCXshPaid, FCXshLeft, FTXshRmk, FTXshStaRefund, ");
                oSql.AppendLine("   	FTXshStaDoc, FTXshStaApv, FTXshStaPrcStk, FTXshStaPaid, FNXshStaDocAct, FNXshStaRef, ");
                oSql.AppendLine("   	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("   SELECT FTBchCode, '"+ ptTaxDocNo + "' AS FTXshDocNo, FTShpCode, (CASE WHEN FNXshDocType = 9 THEN 5 ELSE 4 END) AS FNXshDocType, GETDATE() AS FDXshDocDate, FTXshCshOrCrd, FTXshVATInOrEx, ");
                oSql.AppendLine("   	FTDptCode, FTWahCode, FTPosCode, FTShfCode, FNSdtSeqNo, FTUsrCode, FTSpnCode, ");
                oSql.AppendLine("   	FTXshApvCode, FTCstCode, '"+ ptTaxDocNo +"' AS FTXshDocVatFull, FTXshDocNo AS FTXshRefExt, FDXshRefExtDate, FTXshRefInt, FDXshRefIntDate, ");
                oSql.AppendLine("   	FTXshRefAE, 0 AS FNXshDocPrint, FTRteCode, FCXshRteFac, FCXshTotal, FCXshTotalNV, FCXshTotalNoDis, ");
                oSql.AppendLine("   	FCXshTotalB4DisChgV, FCXshTotalB4DisChgNV, FTXshDisChgTxt, FCXshDis, FCXshChg, FCXshTotalAfDisChgV, FCXshTotalAfDisChgNV, ");
                oSql.AppendLine("   	FCXshRefAEAmt, FCXshAmtV, FCXshAmtNV, FCXshVat, FCXshVatable, FTXshWpCode, FCXshWpTax, ");
                oSql.AppendLine("   	FCXshGrand, FCXshRnd, FTXshGndText, FCXshPaid, FCXshLeft, FTXshRmk, FTXshStaRefund, ");
                oSql.AppendLine("   	FTXshStaDoc, FTXshStaApv, FTXshStaPrcStk, FTXshStaPaid, FNXshStaDocAct, FNXshStaRef, ");
                oSql.AppendLine("   	GETDATE() AS FDLastUpdOn, '"+ cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '"+ cVB.tVB_UsrCode +"' AS FTCreateBy");
                //oSql.AppendLine("   FROM TPSTSalHD WITH(NOLOCK)");
                oSql.AppendLine($"   FROM {tTblSalHD} WITH(NOLOCK)"); //*Net 63-05-26
                oSql.AppendLine("   WHERE FTBchCode = '"+ cVB.tVB_BchCode +"' AND FTXshDocNo = '"+ ptSaleDocNo +"'");
                oSql.AppendLine();

                // TPSTTaxHDCst
                oSql.AppendLine("   INSERT INTO TPSTTaxHDCst ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("   	(FTBchCode, FTXshDocNo, FTXshCardID, FTXshCardNo, FNXshCrTerm, FDXshDueDate, FDXshBillDue,");
                oSql.AppendLine("   	FTXshCtrName, FDXshTnfDate, FTXshRefTnfID, FNXshAddrShip, FNXshAddrTax, FTXshCstName, FTXshCstTel)");
                oSql.AppendLine("   SELECT FTBchCode, '"+ ptTaxDocNo +"' AS FTXshDocNo, FTXshCardID, FTXshCardNo, FNXshCrTerm, FDXshDueDate, FDXshBillDue,");
                oSql.AppendLine("   	FTXshCtrName, FDXshTnfDate, FTXshRefTnfID, FNXshAddrShip, FNXshAddrTax, FTXshCstName, FTXshCstTel");
                //oSql.AppendLine("   FROM TPSTSalHDCst WITH(NOLOCK)");
                oSql.AppendLine($"   FROM {tTblSalHDCst} WITH(NOLOCK)"); //*Net 63-05-26
                oSql.AppendLine("   WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "'");
                oSql.AppendLine();

                // TPSTTaxHDDis
                oSql.AppendLine("   INSERT INTO TPSTTaxHDDis ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("   	(FTBchCode, FTXshDocNo, FDXhdDateIns, FTXhdDisChgType, FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt)");
                oSql.AppendLine("   SELECT FTBchCode, '" + ptTaxDocNo + "' AS FTXshDocNo, FDXhdDateIns, FTXhdDisChgType, FCXhdTotalAfDisChg, FCXhdDisChg, FCXhdAmt");
                //oSql.AppendLine("   FROM TPSTSalHDDis WITH(NOLOCK)");
                oSql.AppendLine($"   FROM {tTblSalHDDis} WITH(NOLOCK)"); //*Net 63-05-26
                oSql.AppendLine("   WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "'");
                oSql.AppendLine();

                // TPSTTaxDT
                oSql.AppendLine("   INSERT INTO TPSTTaxDT ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("   	(FTBchCode, FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, ");
                oSql.AppendLine("   	FTPplCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, FTXsdSaleType, ");
                oSql.AppendLine("   	FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, ");
                oSql.AppendLine("   	FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode,");
                oSql.AppendLine("   	FCXsdWhtRate, FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk,");
                oSql.AppendLine("   	FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk,");
                oSql.AppendLine("   	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("   SELECT FTBchCode, '" + ptTaxDocNo + "' AS FTXshDocNo, FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, ");
                oSql.AppendLine("   	FTPplCode, FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, FTXsdSaleType, ");
                oSql.AppendLine("   	FCXsdSalePrice, FCXsdQty, FCXsdQtyAll, FCXsdSetPrice, FCXsdAmtB4DisChg, FTXsdDisChgTxt, FCXsdDis, ");
                oSql.AppendLine("   	FCXsdChg, FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, ");
                oSql.AppendLine("   	FCXsdWhtRate, FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, ");
                oSql.AppendLine("   	FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                oSql.AppendLine("   	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //oSql.AppendLine("   FROM TPSTSalDT WITH(NOLOCK)");
                oSql.AppendLine($"   FROM {tTblSalDT} WITH(NOLOCK)"); //*Net 63-05-26
                oSql.AppendLine("   WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "'");
                ////*Em 63-04-30
                //oSql.AppendLine("SELECT DT.FTBchCode, '" + ptTaxDocNo + "' AS FTXshDocNo, DT.FNXsdSeqNo, FTPdtCode, FTXsdPdtName, FTPunCode, FTPunName, ");
                //oSql.AppendLine(" FTPplCode,FCXsdFactor, FTXsdBarCode, FTSrnCode, FTXsdVatType, FTVatCode, FCXsdVatRate, FTXsdSaleType, ");
                //oSql.AppendLine("FCXsdSalePrice, DT.FCXsdQty, FCXsdQtyAll,ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice) AS FCXsdSetPrice, ");
                //oSql.AppendLine("(DT.FCXsdQty * ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice)) AS FCXsdAmtB4DisChg, FTXsdDisChgTxt,ISNULL(DTD.FCXddValue,0) AS FCXsdDis, ");
                //oSql.AppendLine(" ISNULL(DTC.FCXddValue,0) AS FCXsdChg, (DT.FCXsdQty * ISNULL(PD.FCXsdSetPrice,DT.FCXsdSetPrice))- ISNULL(DTD.FCXddValue,0) + ISNULL(DTC.FCXddValue,0) AS FCXsdNet, FCXsdNetAfHD, FCXsdVat, FCXsdVatable, FCXsdWhtAmt, FTXsdWhtCode, ");
                //oSql.AppendLine(" FCXsdWhtRate, FCXsdCostIn, FCXsdCostEx, FTXsdStaPdt, FCXsdQtyLef, FCXsdQtyRfn, FTXsdStaPrcStk, ");
                //oSql.AppendLine(" FTXsdStaAlwDis, FNXsdPdtLevel, FTXsdPdtParent, FCXsdQtySet, FTPdtStaSet, FTXsdRmk, ");
                //oSql.AppendLine("   	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //oSql.AppendLine("FROM TPSTSalDT DT WITH(NOLOCK)");
                //oSql.AppendLine("LEFT JOIN (SELECT PD.FTBchCode,PD.FTXshDocNo,PD.FNXsdSeqNo,PD.FCXsdQty,PD.FCXsdSetPrice");
                //oSql.AppendLine("   FROM TPSTSalPD PD WITH(NOLOCK)");
                //oSql.AppendLine("   INNER JOIN (SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,MIN(FCXsdSetPrice) AS FCXsdSetPrice");
                //oSql.AppendLine("   	FROM TPSTSalPD WITH(NOLOCK)");
                //oSql.AppendLine("   	WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "' AND FTXpdGetType = '4'");
                //oSql.AppendLine("	    GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) TMP ON PD.FTBchCode = TMP.FTBchCode AND PD.FTXshDocNo = TMP.FTXshDocNo AND PD.FNXsdSeqNo = TMP.FNXsdSeqNo AND PD.FCXsdSetPrice = TMP.FCXsdSetPrice");
                //oSql.AppendLine("   WHERE PD.FTBchCode = '" + cVB.tVB_BchCode + "' AND PD.FTXshDocNo = '" + ptSaleDocNo + "' AND PD.FTXpdGetType = '4') PD");
                //oSql.AppendLine("   ON DT.FTBchCode = PD.FTBchCode AND DT.FTXshDocNo = PD.FTXshDocNo AND DT.FNXsdSeqNo = PD.FNXsdSeqNo");
                //oSql.AppendLine("LEFT JOIN(SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,SUM(FCXddValue) AS FCXddValue");
                //oSql.AppendLine("			FROM TPSTSalDTDis WITH(NOLOCK)");
                //oSql.AppendLine("			WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "'");
                //oSql.AppendLine("			AND ((FTXddDisChgType IN('1','2') AND FNXddStaDis = 1)");
                //oSql.AppendLine("			OR (FTXddDisChgType IN('1','2') AND FNXddStaDis = 2 AND FTXddRefCode <> ''");
                //oSql.AppendLine("           AND FTXddRefCode NOT IN (SELECT DISTINCT FTPmhDocNo FROM TPSTSalPD WITH(NOLOCK) WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + cVB.tVB_DocNo + "' AND FTXpdGetType = '4')))");
                //oSql.AppendLine("			GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) DTD ON DTD.FTBchCode = DT.FTBchCode AND DTD.FTXshDocNo = DT.FTXshDocNo AND DTD.FNXsdSeqNo = DT.FNXsdSeqNo ");
                //oSql.AppendLine("LEFT JOIN(SELECT FTBchCode,FTXshDocNo,FNXsdSeqNo,SUM(FCXddValue) AS FCXddValue");
                //oSql.AppendLine("			FROM TPSTSalDTDis WITH(NOLOCK)");
                //oSql.AppendLine("			WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "'");
                //oSql.AppendLine("			AND (FTXddDisChgType IN('3','4') AND FNXddStaDis = 1)");
                //oSql.AppendLine("			GROUP BY FTBchCode,FTXshDocNo,FNXsdSeqNo) DTC ON DTC.FTBchCode = DT.FTBchCode AND DTC.FTXshDocNo = DT.FTXshDocNo AND DTC.FNXsdSeqNo = DT.FNXsdSeqNo ");
                //oSql.AppendLine("WHERE DT.FTBchCode = '" + cVB.tVB_BchCode + "' AND DT.FTXshDocNo = '" + ptSaleDocNo + "' ");
                ////+++++++++++++++++++++++++
                oSql.AppendLine();

                // TPSTTaxDTDis
                oSql.AppendLine("   INSERT INTO TPSTTaxDTDis ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("   	(FTBchCode, FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue)");
                oSql.AppendLine("   SELECT FTBchCode, '" + ptTaxDocNo + "' AS FTXshDocNo, FNXsdSeqNo, FDXddDateIns, FNXddStaDis, FTXddDisChgTxt, FTXddDisChgType, FCXddNet, FCXddValue");
                //oSql.AppendLine("   FROM TPSTSalDTDis WITH(NOLOCK)");
                oSql.AppendLine($"   FROM {tTblSalDTDis} WITH(NOLOCK)"); //*Net 63-05-26
                oSql.AppendLine("   WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "'");
                oSql.AppendLine();

                // TPSTTaxDTPmt
                oSql.AppendLine("   INSERT INTO TPSTTaxDTPmt ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("   	(FTBchCode, FTXshDocNo, FTPmhCode, FTXdpGrpName, FTXsdBarCode, FNXsdSeqNo, FCXdpQtyAll,");
                oSql.AppendLine("   	FCXdpNet, FCXdpSetPrice, FCXdpGetQtyDiv, FCXdpGetCond, FCXdpGetValue, FCXdpDis, FCXdpDisAvg, ");
                oSql.AppendLine("   	FCXdpPoint, FTXdpStaExceptPmt, FTXdpStaRcv)");
                oSql.AppendLine("   SELECT FTBchCode, '" + ptTaxDocNo + "' AS FTXshDocNo, FTPmhCode, FTXdpGrpName, FTXsdBarCode, FNXsdSeqNo, FCXdpQtyAll, ");
                oSql.AppendLine("   	FCXdpNet, FCXdpSetPrice, FCXdpGetQtyDiv, FCXdpGetCond, FCXdpGetValue, FCXdpDis, FCXdpDisAvg, ");
                oSql.AppendLine("   	FCXdpPoint, FTXdpStaExceptPmt, FTXdpStaRcv");
                //oSql.AppendLine("   FROM TPSTSalDTPmt WITH(NOLOCK)");
                oSql.AppendLine($"   FROM {tTblSalDTPmt} WITH(NOLOCK)"); //*Net 63-05-26
                oSql.AppendLine("   WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "'");
                oSql.AppendLine();

                // TPSTTaxRC
                oSql.AppendLine("   INSERT INTO TPSTTaxRC ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("   	(FTBchCode, FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, FTXrcRefNo1, FTXrcRefNo2, ");
                oSql.AppendLine("   	FDXrcRefDate, FTXrcRefDesc, FTBnkCode, FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt, ");
                oSql.AppendLine("   	FCXrcDep, FCXrcNet, FCXrcChg, FTXrcRmk, FTPhwCode, FTXrcRetDocRef, FTXrcStaPayOffline, ");
                oSql.AppendLine("   	FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                oSql.AppendLine("   SELECT FTBchCode, '" + ptTaxDocNo + "' AS FTXshDocNo, FNXrcSeqNo, FTRcvCode, FTRcvName, FTXrcRefNo1, FTXrcRefNo2, ");
                oSql.AppendLine("   	FDXrcRefDate, FTXrcRefDesc, FTBnkCode, FTRteCode, FCXrcRteFac, FCXrcFrmLeftAmt, FCXrcUsrPayAmt,");
                oSql.AppendLine("   	FCXrcDep, FCXrcNet, FCXrcChg, FTXrcRmk, FTPhwCode, FTXrcRetDocRef, FTXrcStaPayOffline, ");
                oSql.AppendLine("   	GETDATE() AS FDLastUpdOn, '" + cVB.tVB_UsrCode + "' AS FTLastUpdBy, GETDATE() AS FDCreateOn, '" + cVB.tVB_UsrCode + "' AS FTCreateBy");
                //oSql.AppendLine("   FROM TPSTSalRC WITH(NOLOCK)");
                oSql.AppendLine($"   FROM {tTblSalRC} WITH(NOLOCK)"); //*Net 63-05-26
                oSql.AppendLine("   WHERE FTBchCode = '" + cVB.tVB_BchCode + "' AND FTXshDocNo = '" + ptSaleDocNo + "'");
                oSql.AppendLine();

                oSql.AppendLine("COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_SETxDataQuery(oSql.ToString());
                bSuccess = true;
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cTax", "C_DOCbInsertTaxWithSQL : " + oEx.Message); }
            return bSuccess;
        }

        public void C_DATxInsertHDCst(cmlTPSTTaxHDCst poHDCst)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("INSERT INTO TPSTTaxHDCst ");//*Arm 63-05-26 เอา WITH(ROWLOCK) ออก
                oSql.AppendLine("(FTBchCode,FTXshDocNo,FTXshCardID,FNXshAddrTax,FTXshCstName,FTXshCstTel)");
                oSql.AppendLine("VALUES('"+ poHDCst.FTBchCode +"','"+ poHDCst.FTXshDocNo +"','"+ poHDCst.FTXshCardID +"'," + poHDCst.FNXshAddrTax + ",'"+ poHDCst.FTXshCstName +"','"+ poHDCst.FTXshCstTel +"')");
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cTax", "C_DATxInsertHDCst : " + oEx.Message); }
        }
    }
}
