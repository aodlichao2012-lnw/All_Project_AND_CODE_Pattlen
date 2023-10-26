using MQAdaLink.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MQAdaLink.Class.Import
{
    class cMaster
    {
        
        DataTable oDbTblPdt;
        DataTable oDbTblPdtBar;
        DataTable oDbTblPdtPackSize;
        int nSuccess = 0;
        int nError = 0;
        int nQtyAll = 0;
        int nQtyDone = 0;
        List<string> aFileXml;
        List<string> aFileXmlError;
        List<string> aFileXmlErrDesc;
        List<string> aFileXmlDul;
        public void C_PRCxMaster(List<string> paFileXml)
        {
            Console.WriteLine("");
            Console.WriteLine("Import Master Process Start...");
            cSP oSP = new cSP();
            aFileXml = new List<string>();
            aFileXmlError = new List<string>();
            aFileXmlErrDesc = new List<string>();
            aFileXmlDul = new List<string>();
            
            oDbTblPdt = new DataTable();
            oDbTblPdtBar = new DataTable();
            oDbTblPdtPackSize = new DataTable();
            //cVB.tVB_MasBackUP = null;

            try
            {
                new cLog().C_PRCxLogMonitor("cMaster", "C_PRCxMaster : start."); //*Arm 63-08-27
                if (paFileXml.Count > 0)
                {
                    oDbTblPdt = C_SEToColumnPdt();
                    foreach (string tXmlname in paFileXml)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Check File Master       {0}", tXmlname);
                        if (new cSP().C_CHKbHistory(tXmlname) == true)
                        {
                            if (C_CHKxFileXml(tXmlname) == true)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Get Data From XML To DataTable    ======> Success");
                                aFileXml.Add(tXmlname);

                                if (aFileXml.Count > 0)
                                {
                                    Console.WriteLine("Process Insert Master         {0} ======> Start", tXmlname);
                                    C_INSxDatabase(tXmlname);
                                    Console.WriteLine("");
                                    Console.WriteLine("Process Insert Master         {0} ======> Success", tXmlname);
                                }
                                Console.WriteLine("");
                                Console.WriteLine("Insert History    ======> Start");
                                new cSP().C_PRCxInsertHistory(tXmlname);
                                Console.WriteLine("Insert History    ======> End");

                            }
                            else
                            {
                                Console.WriteLine("Get Data From XML To DataTable    ======> False");
                                aFileXmlError.Add(tXmlname);
                            }

                        }
                        else
                        {
                            Console.WriteLine("File XML Duplicate     ======> False");
                            new cSP().C_PRCxDataTableToFile(null, "MASTER(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error", "FileError", tXmlname);
                            aFileXmlDul.Add(tXmlname);
                        }

                    }

                    if (aFileXmlError.Count > 0)
                    {
                        new cSP().C_PRCxMoveFile(aFileXmlError, "Error");
                    }
                    if (aFileXmlDul.Count > 0)
                    {
                        new cSP().C_PRCxMoveFile(aFileXmlDul, "Error");
                        new cSP().C_SAVxHistory("1", "ข้อมูลสินค้า", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2");

                    }
                    if (aFileXml.Count > 0)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Process Move File Success To BackUP ====> Start");
                        new cSP().C_PRCxMoveFile(aFileXml, "Success");
                        Console.WriteLine("Process Move File Success To BackUP ====> End");



                        if (nQtyDone == 0)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Process Insert Log History          ====> Start");
                            new cSP().C_SAVxHistory("1", "ข้อมูลสินค้า", cVB.tVB_MasBackUP + ".zip", "2", nQtyAll, nQtyDone, "2");
                            Console.WriteLine("Process Insert Log History          ====> End");
                        }
                        else if (nQtyAll == nQtyDone)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Process Insert Log History          ====> Start");
                            new cSP().C_SAVxHistory("1", "ข้อมูลสินค้า", cVB.tVB_MasBackUP + ".zip", "1", nQtyAll, nQtyDone, "2");
                            Console.WriteLine("Process Insert Log History          ====> End");
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Process Insert Log History          ====> Start");
                            new cSP().C_SAVxHistory("1", "ข้อมูลสินค้า", cVB.tVB_MasBackUP + ".zip", "2", nQtyAll, nQtyDone, "2");
                            Console.WriteLine("Process Insert Log History          ====> End");
                        }

                    }
                    Console.WriteLine("");
                    Console.WriteLine("Process Zip File Success            ====> Start");
                    new cSP().C_PRCxBackUPFile(cVB.tVB_MasBackUP);
                    Console.WriteLine("Process Zip File Success            ====> End");
                }
                else
                {
                    new cSP().C_SAVxHistory("1", "ข้อมูลสินค้า", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2");
                }
                new cLog().C_PRCxLogMonitor("cMaster", "C_PRCxMaster : end."); //*Arm 63-08-27
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cMaster","C_PRCxMaster : Error/" +oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cMaster", "C_PRCxMaster : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
            finally
            {
                paFileXml = new List<string>();
                new cSP().SP_CLExMemory();                
            }
           
        }

        public void C_INSxDatabase(string ptFilename)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            DataTable oDbTbl = new DataTable();
            DataTable oDbTblTotal = new DataTable();
            DataTable oDbTblUpdate = new DataTable();
            DataTable oDbTblError = new DataTable();
            DataTable oDbTblSucces = new DataTable();
            int nPdtUpdate = 0;
            bool bPrc = false;
            try
            {
                oSql.Clear();
                oSql.AppendLine("DROP TABLE TTmpLinkPDT");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());
                
                oSql.Clear();
                oSql.AppendLine("IF OBJECT_ID(N'TTmpLinkPDT') IS NULL BEGIN");
                oSql.AppendLine("    CREATE TABLE[dbo].[TTmpLinkPDT](");
                oSql.AppendLine("        [FTPdtCode][varchar](50) NULL,");
                oSql.AppendLine("        [FTPdtName] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTPdtPoint] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTPdtType] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTPdtStaAlwDis] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTPdtStaVat] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTPdtNameOth] [varchar] (50) NULL,");                
                oSql.AppendLine("        [FTPbnCode] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTVatCode] [varchar] (50) NULL,");
                oSql.AppendLine("        [FDPdtSaleStart] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTPgpChain] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTPunCode] [varchar] (50) NULL,");
                oSql.AppendLine("        [FTFlag] [nvarchar] (1) NULL,");
                oSql.AppendLine("        [FTDesc] [nvarchar] (255) NULL,");
                oSql.AppendLine("        [FTPdtNameABB] [nvarchar] (50) NULL"); // *Arm 63-08-21
                oSql.AppendLine("    ) ON[PRIMARY]");
                oSql.AppendLine("END");     
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("TRUNCATE TABLE TTmpLinkPDT");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                
                Console.WriteLine("Process Copy DataTable To Temp    ======> Start");
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_Conn, SqlBulkCopyOptions.Default))
                {
                    
                    foreach (DataColumn oColName in oDbTblPdt.Columns)
                    {
                        oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
                    }
                    //+++++++++++++++
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TTmpLinkPDT";

                    try
                    {
                        oBulkCopy.WriteToServer(oDbTblPdt);
                        bPrc = true;
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_PRCxLog("C_INSxDatabase", oEx.Message.ToString());
                        bPrc = false;
                    }
                }
                Console.WriteLine("Process Copy DataTable To Temp    ======> Success");
                if (bPrc)
                {

                    C_PRCxVatRate();

                    //ข้อมูล INSERT
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TTmpLinkPDT SET FTFlag = '1' ");
                    oSql.AppendLine("WHERE FTPdtCode NOT IN ( select FTPdtCode from TCNMPdt)");
                    oSql.AppendLine("");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    //ข้อมูล UPDATE
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TTmpLinkPDT SET FTFlag = '2' ");
                    oSql.AppendLine("WHERE FTPdtCode IN ( select FTPdtCode from TCNMPdt)");
                    oSql.AppendLine("");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    //oSql.Clear();
                    //oSql.AppendLine("SELECT count(FTPdtCode) As FInsert FROM TTmpLinkPDT With(NOLOCK)");
                    //oDbTblTotal = oDb.C_GEToSQLToDatatable(oSql.ToString());
                    //Console.WriteLine("");
                    //Console.WriteLine("Process Check Product Total       ======> {0} ", oDbTblTotal.Rows[0]["FInsert"].ToString());

                    Console.WriteLine("Process Check TCNMPdt Duplicate   ======> Start");
                    //Update
                    oSql.Clear();
                   
                    oSql.AppendLine("UPDATE PDT With(ROWLOCK)");
                    oSql.AppendLine("SET FTPdtType = TMP.FTPdtType");
                    oSql.AppendLine(", FTPdtStaAlwDis = CASE ISNULL(TMP.FTPdtStaAlwDis,'') WHEN '' THEN '1' ELSE TMP.FTPdtStaAlwDis END ");
                    oSql.AppendLine(", FTPdtStaVat = '1'");
                    oSql.AppendLine(", FTPdtStaVatBuy = '1'");
                    oSql.AppendLine(", FTPbnCode = TMP.FTPbnCode");
                    oSql.AppendLine(", FDPdtSaleStart = TMP.FDPdtSaleStart");
                    oSql.AppendLine(", FDPdtSaleStop = '99991231'");
                    oSql.AppendLine(", FDLastUpdOn = GETDATE()");
                    oSql.AppendLine(", FTLastUpdBy = 'AdaLink'");
                    oSql.AppendLine("FROM TCNMPdt PDT");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT TMP With(NOLOCK) ON TMP.FTPdtCode = PDT.FTPdtCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Check TCNMPdt Duplicate   ======> End");

                    Console.WriteLine("Process Check TCNMPdt_L Duplicate ======> Start");
                    // Pdt_L
                    //Update
                    oSql.Clear();
                    oSql.AppendLine("UPDATE PDT With(ROWLOCK)");
                    oSql.AppendLine("SET FTPdtName = TMP.FTPdtName");
                    oSql.AppendLine(", FTPdtNameOth = TMP.FTPdtNameOth");
                    //oSql.AppendLine(", FTPdtNameABB = SUBSTRING(TMP.FTPdtName,1," + cVB.nVB_SubStr+")");
                    oSql.AppendLine(", FTPdtNameABB = TMP.FTPdtNameABB"); //*Arm 63-08-21
                    oSql.AppendLine("FROM TCNMPdt_L PDT");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT TMP With(NOLOCK) ON TMP.FTPdtCode = PDT.FTPdtCode AND PDT.FNLngID = 1");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Check TCNMPdt_L Duplicate ======> End");

                    Console.WriteLine("Process Check TCNMPdtPackSize Duplicate=> Start");

                    oSql.Clear();
                    oSql.AppendLine("UPDATE PDT With(ROWLOCK)");
                    oSql.AppendLine("SET PDT.FTPunCode = TMP.FTPunCode");
                    oSql.AppendLine(", PDT.FTPdtStaAlwPick = 1");
                    oSql.AppendLine(", PDT.FTPdtStaAlwPoHQ = 1");
                    oSql.AppendLine(", PDT.FTPdtStaAlwBuy = 1");
                    oSql.AppendLine(", PDT.FTPdtStaAlwSale = 1");
                    oSql.AppendLine("FROM TCNMPdtPackSize PDT");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT TMP With(NOLOCK) ON TMP.FTPdtCode = PDT.FTPdtCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Check TCNMPdtPackSize Duplicate=> End");

                    Console.WriteLine("Process Check TCNMPdtBrand Duplicate ===> Start");
                    oSql.Clear();
                    oSql.AppendLine("UPDATE PDTbn SET ");
                    oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("FTLastUpdBy = 'AdaLink'");
                    oSql.AppendLine("FROM TCNMPdtBrand PDTbn");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT TMP  ON TMP.FTPbnCode = PDTbn.FTPbnCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Check TCNMPdtBrand Duplicate ===> End");

                    Console.WriteLine("Process Check TCNMPdtBrand_L Duplicate => Start");
                    oSql.Clear();
                    oSql.AppendLine("UPDATE PDTbn SET ");
                    oSql.AppendLine("FTPbnName = TMP.FTPbnCode");
                    oSql.AppendLine("FROM TCNMPdtBrand_L PDTbn");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT TMP  ON TMP.FTPbnCode = PDTbn.FTPbnCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    
                    Console.WriteLine("Process Check TCNMPdtBrand_L Duplicate => End");

                    Console.WriteLine("Process Check PdtUnit PdtUnit_L Duplicate => Start");

                    oSql.Clear();
                    oSql.AppendLine("UPDATE PdtUnit");
                    oSql.AppendLine("Set PdtUnit.FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("PdtUnit.FTLastUpdBy = 'AdaLink'");
                    oSql.AppendLine("From TCNMPdtUnit PdtUnit");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT Tmp ON PdtUnit.FTPunCode = Tmp.FTPunCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    oSql.AppendLine("UPDATE PdtUnit_L");
                    oSql.AppendLine("Set PdtUnit_L.FTPunName = Tmp.FTPunCode");
                    oSql.AppendLine("From TCNMPdtUnit_L PdtUnit_L");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT Tmp ON PdtUnit_L.FTPunCode = Tmp.FTPunCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Check PdtUnit PdtUnit_L Duplicate => End");

                    //Console.WriteLine("Process Delete Data Duplicate     ======> Start");
                    //oSql.Clear();
                    //oSql.AppendLine(" ");
                    ////Delete from temp
                    //oSql.AppendLine("DELETE TMP ");
                    //oSql.AppendLine("FROM TTmpLinkPDT TMP");
                    //oSql.AppendLine("INNER JOIN TCNMPdt PDT With(NOLOCK) ON TMP.FTPdtCode = PDT.FTPdtCode");
                    //oSql.AppendLine(" ");
                    //oDb.C_GEToSQLToDatatable(oSql.ToString());
                    //Console.WriteLine("Process Delete Data Duplicate     ======> End");

                    //oSql.Clear();
                    //oSql.AppendLine("SELECT count(FTPdtCode) As FInsert FROM TTmpLinkPDT With(NOLOCK)");
                    //oDbTblUpdate = oDb.C_GEToSQLToDatatable(oSql.ToString());

                    //nPdtUpdate = Convert.ToInt32(oDbTblTotal.Rows[0]["FInsert"].ToString()) - Convert.ToInt32(oDbTblUpdate.Rows[0]["FInsert"].ToString());

                    
                    //Console.WriteLine("Process Product Update Total      ======> {0} ", nPdtUpdate.ToString());                    

                    Console.WriteLine("Process Insert Data TCNMPdt       ======> Start");
                    
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMPdt With(ROWLOCK) (FTPdtCode, FTPdtStkControl,FTPdtGrpControl, FTPdtForSystem, FCPdtQtyOrdBuy, FCPdtCostDef, FCPdtCostOth, FCPdtCostStd, FCPdtMin, FCPdtMax, FTPdtPoint, FCPdtPointTime");
                    oSql.AppendLine(", FTPdtType, FTPdtSaleType, FTPdtSetOrSN, FTPdtStaSetPri, FTPdtStaSetShwDT, FTPdtStaAlwDis, FTPdtStaAlwReturn, FTPdtStaVatBuy, FTPdtStaVat, FTPdtStaActive");
                    oSql.AppendLine(", FTPdtStaAlwReCalOpt, FTPdtStaCsm, FTTcgCode, FTPgpChain, FTPtyCode, FTPbnCode, FTPmoCode, FTVatCode,FTEvhCode");
                    oSql.AppendLine(", FDPdtSaleStart, FDPdtSaleStop, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT distinct FTPdtCode,'1' AS FTPdtStkControl,NULL As FTPdtGrpControl,'1' FTPdtForSystem,0 FCPdtQtyOrdBuy,0 FCPdtCostDef, 0 FCPdtCostOth,0 FCPdtCostStd,0 FCPdtMin,0 FCPdtMax,FTPdtPoint,0 FCPdtPointTime");
                    oSql.AppendLine(", FTPdtType,CASE WHEN FTPdtType = '2' THEN '2' ELSE '1' END FTPdtSaleType,'1' FTPdtSetOrSN,NULL FTPdtStaSetPri,NULL FTPdtStaSetShwDT,CASE ISNULL(FTPdtStaAlwDis,'') WHEN '' THEN '1' ELSE FTPdtStaAlwDis END  AS FTPdtStaAlwDis,'1' FTPdtStaAlwReturn,'1'  AS FTPdtStaVatBuy,(CASE ISNULL(FTPdtStaVat,'') WHEN '' THEN '1' ELSE FTPdtStaVat END) AS FTPdtStaVat,'1' FTPdtStaActive");
                    oSql.AppendLine(", '1' FTPdtStaAlwReCalOpt,'1' FTPdtStaCsm,'' As FTTcgCode,FTPgpChain,'' FTPtyCode,ISNULL(FTPbnCode,'') AS FTPbnCode,'' FTPmoCode,FTVatCode,'' FTEvhCode");
                    oSql.AppendLine(", GETDATE() AS FDPdtSaleStart,'99991231' AS FDPdtSaleStop,GETDATE() FDLastUpdOn,'AdaLink' FTLastUpdBy,GETDATE() FDCreateOn,'AdaLink' FTCreateBy");
                    oSql.AppendLine("FROM TTmpLinkPDT With(NOLOCK)");
                    oSql.AppendLine("WHERE FTDesc IS NULL AND FTFlag = 1");
                    //oSql.AppendLine("ORDER BY FTPdtCode");
                    //oSql.AppendLine("OFFSET " + nCount + " ROWS FETCH NEXT 10000 ROWS ONLY ");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Insert Data TCNMPdt       ======> End");

                    Console.WriteLine("Process Insert Data TCNMPdt_L     ======> Start");                    
                    //Insert
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMPdt_L With(ROWLOCK) (FTPdtCode, FNLngID, FTPdtName, FTPdtNameABB, FTPdtNameOth, FTPdtRmk)");
                    //oSql.AppendLine("SELECT distinct FTPdtCode,1 FNLngID,FTPdtName,SUBSTRING(FTPdtName,1," + cVB.nVB_SubStr + ") As FTPdtNameABB,FTPdtNameOth,'' FTPdtRmk");
                    oSql.AppendLine("SELECT distinct FTPdtCode,1 FNLngID,FTPdtName,FTPdtNameABB,FTPdtNameOth,'' FTPdtRmk"); //*Arm 63-08-21
                    oSql.AppendLine("FROM TTmpLinkPDT With(NOLOCK)");
                    oSql.AppendLine("WHERE FTDesc IS NULL AND FTFlag = 1");
                    //oSql.AppendLine("ORDER BY FTPdtCode");
                    //oSql.AppendLine("OFFSET " + nCount + " ROWS FETCH NEXT 10000 ROWS ONLY ");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    //End
                    Console.WriteLine("Process Insert Data TCNMPdt_L     ======> End");

                    Console.WriteLine("Process Insert Data TCNMPdtPackSize ====> Start");                    
                    //Insert
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMPdtPackSize With(ROWLOCK) (FTPdtCode, FTPunCode, FCPdtUnitFact,FCPdtPriceRET,FCPdtPriceWHS,FCPdtPriceNET,FTPdtGrade, FCPdtWeight,FTClrCode,FTPszCode,FTPdtUnitDim,FTPdtPkgDim,FTPdtStaAlwPick,FTPdtStaAlwPoHQ,FTPdtStaAlwBuy,FTPdtStaAlwSale, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT distinct FTPdtCode,FTPunCode,1 As FCPdtUnitFact,0,0,0,NULL FTPdtGrade,0 FCPdtWeight,NULL FTClrCode,NULL FTPszCode,'','',1,1,1,1,GETDATE() FDLastUpdOn,'AdaLink' FTLastUpdBy,GETDATE() FDCreateOn,'AdaLink' FTCreateBy");
                    oSql.AppendLine("FROM TTmpLinkPDT With(NOLOCK)");
                    oSql.AppendLine("WHERE FTDesc IS NULL AND FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Insert Data TCNMPdtPackSize ====> End");

                    Console.WriteLine("Process Insert Data TCNMPdtBar      ======> Start");
                    
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMPdtBar With(ROWLOCK) (FTPdtCode, FTBarCode, FTPunCode, FTBarStaUse, FTBarStaAlwSale,FTBarStaByGen,FTPlcCode,FNPldSeq, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT DISTINCT FTPdtCode,FTPdtCode,MIN(FTPunCode),'1' FTBarStaUse,'1' FTBarStaAlwSale,NULL As FTBarStaByGen,");
                    oSql.AppendLine("NULL As FTPlcCode,0 As FNPldSeq,");
                    oSql.AppendLine("GETDATE() FDLastUpdOn,'AdaLink' FTLastUpdBy,GETDATE() FDCreateOn,'AdaLink' FTCreateBy");
                    oSql.AppendLine("FROM TTmpLinkPDT With(NOLOCK)");
                    oSql.AppendLine("WHERE FTDesc IS NULL AND FTFlag = 1");
                    oSql.AppendLine("GROUP BY FTPdtCode");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Insert Data TCNMPdtBar      ======> End");

                    Console.WriteLine("Process Insert Data TCNMPdtBrand    ======> Start");
                    
                    oSql.Clear();
                    oSql.AppendLine(" ");
                    oSql.AppendLine("INSERT INTO TCNMPdtBrand(FTPbnCode,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("SELECT DISTINCT FTPbnCode,GETDATE(),'AdaLink',GETDATE(),'AdaLink' FROM TTmpLinkPDT");
                    oSql.AppendLine("WHERE FTPbnCode <> ''");
                    oSql.AppendLine("AND FTDesc IS NULL AND FTFlag = 1");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Insert Data TCNMPdtBrand    ======> End");

                    Console.WriteLine("Process Insert Data TCNMPdtBrand_L  ======> Start");
                    
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMPdtBrand_L(FTPbnCode,FNLngID,FTPbnName,FTPbnRmk)");
                    oSql.AppendLine("SELECT DISTINCT FTPbnCode,1,FTPbnCode,'' FROM TTmpLinkPDT");
                    oSql.AppendLine("WHERE FTPbnCode <> ''");
                    oSql.AppendLine("AND FTDesc IS NULL AND FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Insert Data TCNMPdtBrand_L  ======> End");

                    Console.WriteLine("Process Insert Data TCNMPdtUnit  ======> Start");
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMPdtUnit(FTPunCode,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("select DISTINCT FTPunCode,GETDATE(),'AdaLink',GETDATE(),'AdaLink' from TTmpLinkPDT");
                    oSql.AppendLine("WHERE FTDesc IS NULL AND FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Insert Data TCNMPdtUnit  ======> End");

                    Console.WriteLine("Process Insert Data TCNMPdtUnit_L  ======> Start");
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMPdtUnit_L(FTPunCode,FNLngID,FTPunName)");
                    oSql.AppendLine("select DISTINCT FTPunCode,1,FTPunCode from TTmpLinkPDT");
                    oSql.AppendLine("WHERE FTDesc IS NULL AND FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Insert Data TCNMPdtUnit_L  ======> End");

                    //nQtyDone = nQtyDone + (nPdtUpdate + Convert.ToInt32(oDbTblUpdate.Rows[0]["FInsert"].ToString()));

                    oSql.Clear();
                    oSql.AppendLine("select ROW_NUMBER() OVER ( ORDER BY FTPdtCode ) As Seq,Convert(varchar,GETDATE(),103) As InsertDate, FTPdtCode As Code,FTPdtName As FName,CASE WHEN FTFlag = 1 THEN 'Insert' ELSE 'Update' END As Remark from TTmpLinkPDT ");
                    oSql.AppendLine("where FTDesc IS NULL");
                    oDbTblSucces = oDb.C_GEToSQLToDatatable(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("select ROW_NUMBER() OVER ( ORDER BY FTPdtCode ) As Seq,Convert(varchar,GETDATE(),103) As InsertDate, FTPdtCode As Code,FTPdtName As FName,FTDesc As Remark from TTmpLinkPDT ");
                    oSql.AppendLine("where FTDesc IS NOT NULL");
                    oDbTblError = oDb.C_GEToSQLToDatatable(oSql.ToString());

                    nQtyDone = Convert.ToInt32(oDbTblSucces.Rows.Count);
                    //nQtyError = nQtyError + Convert.ToInt32(oDbTblError.Rows.Count);

                    new cSP().C_PRCxDataTableToFile(oDbTblSucces, "MASTER(Success)-" + DateTime.Now.ToString("yyyyMMdd"), "Success","MASTER");
                    new cSP().C_PRCxDataTableToFile(oDbTblError, "MASTER(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error", "MASTER");
                    //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > แก้ไขรายการสินค้าทั้งสิ้น        " + nPdtUpdate + " รายการ");
                    //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > บันทึกรายการสินค้าสำเร็จทั้งสิ้น    " + Convert.ToInt32(oDbTblUpdate.Rows[0]["FInsert"].ToString()) + " รายการ");
                    //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > บันทึกรายการสินค้าไม่สำเร็จทั้งสิ้น  " + (nQtyAll - nQtyDone) + " รายการ");

                    //bPrc = false;
                }
                else
                {
                    new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > อ่านไฟล์ XML ไม่สำเร็จ");
                }

                
                

            }
            catch (Exception oEx) {
                Console.WriteLine("");
                Console.WriteLine("Process Insert Data Error ...");
                new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > เกิดข้อผิดพลาดในการบันทึกข้อมูล " + oEx.Message.ToString());
                new cLog().C_PRCxLog("cMaster","C_INSxDatabase : Error/" + oEx.Message.ToString()); 
            }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }
        }
        public bool C_CHKxFileXml(string ptXmlName)
        {
            bool bStatus = true;
            string tPathXml = "";
            
            
            try
            {
                new cLog().C_PRCxLogMonitor("cMaster", "C_CHKxFileXml : start."); //*Arm 63-08-27
                new cSP().C_CHKxPathFile(cVB.tVB_PathIN);

                tPathXml = cVB.tVB_PathIN  + ptXmlName;

                XmlDocument oXmlDoc = new XmlDocument();
                oXmlDoc.Load(tPathXml);
                XmlNodeList oXmlList = oXmlDoc.SelectNodes("/TCNMPDT/PDTS");
                int nCount = 0;

                
                Console.WriteLine("Get Data From XML To DataTable    ======> Start");
                foreach (XmlNode oXml in oXmlList)
                {
                    
                    if (string.IsNullOrEmpty(oXml["FTPdtName"].InnerText))
                    {
                        new cSP().C_PRCxLogFile("ไฟล์ : " + ptXmlName + " > รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีชื่อสินค้า");
                        //new cLog().C_PRCxLogError(ptXmlName, "รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีชื่อสินค้า");
                        bStatus = false;
                    }
                    else if (string.IsNullOrEmpty(oXml["FTPdtStaVat"].InnerText))
                    {
                        new cSP().C_PRCxLogFile("ไฟล์ : " + ptXmlName + " > รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีค่า FTPdtStaVat");
                        //new cLog().C_PRCxLogError(ptXmlName, "รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีค่า FTPdtStaVat");
                        bStatus = false;
                    }
                    else
                    {
                        DataRow oDbRow = oDbTblPdt.NewRow();
                        oDbRow["FTPdtCode"] = oXml["FTPdtCode"].InnerText;
                        oDbRow["FTPdtName"] = oXml["FTPdtName"].InnerText;
                        oDbRow["FTPdtPoint"] = oXml["FTPdtPoint"].InnerText;
                        oDbRow["FTPdtType"] = oXml["FTPdtType"].InnerText;
                        oDbRow["FTPdtStaAlwDis"] = oXml["FTPdtStaAlwDis"].InnerText;
                        oDbRow["FTPdtStaVat"] = oXml["FTPdtStaVat"].InnerText;
                        oDbRow["FTPdtNameOth"] = oXml["FTPdtNameOth"].InnerText;
                        oDbRow["FTPbnCode"] = oXml["FTPbnCode"].InnerText;
                        oDbRow["FTVatCode"] = oXml["FTVatCode"].InnerText;
                        oDbRow["FDPdtSaleStart"] = oXml["FDPdtSaleStart"].InnerText;
                        oDbRow["FTPgpChain"] = oXml["FTPgpChain"].InnerText;
                        oDbRow["FTPunCode"] = oXml["FTPunCode"].InnerText;
                        oDbRow["FTPdtNameABB"] = cSP.SP_SUBtSubString(oXml["FTPdtName"].InnerText,15); //*Arm 63-08-21 ตัดคำเหลือ 15 ตำแหน่ง
                        oDbTblPdt.Rows.Add(oDbRow);
                        
                    }
                    nCount++;

                }
                nQtyAll = nQtyAll + nCount;
                
                Console.WriteLine("Get Product From XML Total        ======> {0}", nQtyAll.ToString());
                if (!bStatus)
                {
                    return bStatus = false;
                }
                new cLog().C_PRCxLogMonitor("cMaster", "C_CHKxFileXml : end."); //*Arm 63-08-27
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cMaster", "C_CHKxPathFile : Error/" + oEx.Message);
                new cLog().C_PRCxLogMonitor("cMaster", "C_CHKxPathFile : Error/" + oEx.Message); //*Arm 63-08-27
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return bStatus;
        }

        public void C_PRCxVatRate()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            try
            {
                new cLog().C_PRCxLogMonitor("cMaster", "C_PRCxVatRate : start."); //*Arm 63-08-27
                Console.WriteLine("Process Mapping VatRate To VatCode======> Start");
                oSql.Clear();
                oSql.AppendLine(" UPDATE PDT With(ROWLOCK) SET ");
                oSql.AppendLine(" PDT.FTVatCode = Vat.FTVatCode ");
                oSql.AppendLine(" FROM TTmpLinkPDT  PDT ");
                oSql.AppendLine(" INNER JOIN TCNMVatRate Vat With(NOLOCK) ON Convert(int,Vat.FCVatRate) = Convert(int,Case When PDT.FTVatCode = 'O7' Then 7 else PDT.FTVatCode end) ");
                oSql.AppendLine(" ");
                new cLog().C_PRCxLogMonitor("cMaster", "C_PRCxVatRate : SQL : " + oSql.ToString()); //*Arm 63-08-27

                oDb.C_GEToSQLToDatatable(oSql.ToString());
                Console.WriteLine("Process Mapping VatRate To VatCode======> Success");
                new cLog().C_PRCxLogMonitor("cMaster", "C_PRCxVatRate : end."); //*Arm 63-08-27
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cMaster", "C_PRCxVatRate : Error/" + oEx.Message);
                new cLog().C_PRCxLogMonitor("cMaster", "C_PRCxVatRate : Error/" + oEx.Message); //*Arm 63-08-27
            }
            finally
            {
                oSql = null;    //*Arm 63-08-27
                oDb = null;     //*Arm 63-08-27
            }
        }

        public DataTable C_SEToColumnPdt()
        {
            DataTable oDbTbl = new DataTable();
            oDbTbl.Columns.Add("FTPdtCode", typeof(string));
            oDbTbl.Columns.Add("FTPdtName", typeof(string));
            oDbTbl.Columns.Add("FTPdtPoint", typeof(string));
            oDbTbl.Columns.Add("FTPdtType", typeof(string));
            oDbTbl.Columns.Add("FTPdtStaAlwDis", typeof(string));
            oDbTbl.Columns.Add("FTPdtStaVat", typeof(string));
            oDbTbl.Columns.Add("FTPdtNameOth", typeof(string));            
            oDbTbl.Columns.Add("FTPbnCode", typeof(string));
            oDbTbl.Columns.Add("FTVatCode", typeof(string));
            oDbTbl.Columns.Add("FDPdtSaleStart", typeof(string));
            oDbTbl.Columns.Add("FTPgpChain", typeof(string));       
            oDbTbl.Columns.Add("FTPunCode", typeof(string));
            oDbTbl.Columns.Add("FTFlag", typeof(string));
            oDbTbl.Columns.Add("FTDesc", typeof(string));
            oDbTbl.Columns.Add("FTPdtNameABB", typeof(string)); //*Arm 63-08-21
            return oDbTbl;
        }

        
       
    }
}
