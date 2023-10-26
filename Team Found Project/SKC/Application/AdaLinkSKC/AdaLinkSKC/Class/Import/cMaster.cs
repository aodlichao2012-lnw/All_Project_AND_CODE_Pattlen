using AdaLinkSKC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdaLinkSKC.Class.Import
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
        public void C_PRCxMaster(List<string> paFileXml)
        {
            
            cSP oSP = new cSP();
            aFileXml = new List<string>();
            aFileXmlError = new List<string>();
            
            oDbTblPdt = new DataTable();
            oDbTblPdtBar = new DataTable();
            oDbTblPdtPackSize = new DataTable();
            cVB.tVB_MasBackUP = null;

            try
            {
                    cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (paFileXml.Count > 0)
                    {
                        oDbTblPdt = C_SEToColumnPdt();                       
                        foreach (string tXmlname in paFileXml)
                        {
                            if (C_CHKxFileXml(tXmlname) == true)
                            {
                                aFileXml.Add(tXmlname);
                                
                                if (aFileXml.Count > 0)
                                {
                                    C_INSxDatabase();
                                }
                                
                            }
                            else
                            {
                                aFileXmlError.Add(tXmlname);
                            }
                        }
                        
                        if (aFileXmlError.Count > 0)
                        {
                            new cSP().C_PRCxMoveFile(aFileXmlError, "Error");
                        }
                        if (aFileXml.Count > 0)
                        {
                            new cSP().C_PRCxMoveFile(aFileXml, "Success");
                            new cSP().C_PRCxBackUPFile(cVB.tVB_MasBackUP);
                            new cSP().C_SAVxHistory("1", "ข้อมูลสินค้า", cVB.tVB_MasBackUP + ".zip", "1", nQtyAll, nQtyDone, "2");
                        }
                    }
                    else
                    {
                        new cSP().C_SAVxHistory("1", "ข้อมูลสินค้า", "ไม่มี File Import", "2", nQtyAll, nQtyDone, "2");
                    }
                    
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_PRCxMaster",oEx.Message.ToString()); }
            finally
            {
                paFileXml = new List<string>();
                new cSP().SP_CLExMemory();                
            }
           
        }

        public void C_INSxDatabase()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            DataTable oDbTbl = new DataTable();
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
                oSql.AppendLine("        [FTPunCode] [varchar] (50) NULL");
                oSql.AppendLine("    ) ON[PRIMARY]");
                oSql.AppendLine("END");     
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("TRUNCATE TABLE TTmpLinkPDT");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

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
                if (bPrc)
                {
                    //Update
                    oSql.Clear();
                    oSql.AppendLine("BEGIN TRANSACTION");
                    oSql.AppendLine("  BEGIN TRY");

                    oSql.AppendLine("UPDATE PDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTPdtType = TMP.FTPdtType");
                    oSql.AppendLine(", FTPdtStaAlwDis = CASE ISNULL(TMP.FTPdtStaAlwDis,'') WHEN '' THEN '1' ELSE TMP.FTPdtStaAlwDis END ");
                    oSql.AppendLine(", FTPdtStaVat = '1'");
                    oSql.AppendLine(", FTPdtStaVatBuy = '1'");
                    oSql.AppendLine(", FTPbnCode = TMP.FTPbnCode");
                    oSql.AppendLine(", FDPdtSaleStart = TMP.FDPdtSaleStart");
                    oSql.AppendLine(", FDPdtSaleStop = '99991231'");
                    oSql.AppendLine("FROM TCNMPdt PDT");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT TMP WITH(NOLOCK) ON TMP.FTPdtCode = PDT.FTPdtCode");
                    
                    // Pdt_L
                    //Update
                    oSql.AppendLine("UPDATE PDT WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTPdtName = TMP.FTPdtName");
                    oSql.AppendLine(", FTPdtNameOth = TMP.FTPdtNameOth");
                    oSql.AppendLine(", FTPdtNameABB = SUBSTRING(TMP.FTPdtName,1," + cVB.nVB_SubStr+")");
                    oSql.AppendLine("FROM TCNMPdt_L PDT");
                    oSql.AppendLine("INNER JOIN TTmpLinkPDT TMP WITH(NOLOCK) ON TMP.FTPdtCode = PDT.FTPdtCode AND PDT.FNLngID = 1");

                    oSql.AppendLine("  SELECT DISTINCT COUNT(*) AS NumInsert FROM TTmpLinkPDT");

                    //Delete from temp
                    oSql.AppendLine("DELETE TMP WITH(ROWLOCK)");
                    oSql.AppendLine("FROM TTmpLinkPDT TMP");
                    oSql.AppendLine("INNER JOIN TCNMPdt PDT WITH(NOLOCK) ON TMP.FTPdtCode = PDT.FTPdtCode");
                    

                    //Insert
                    oSql.AppendLine("INSERT INTO TCNMPdt(FTPdtCode, FTPdtStkControl,FTPdtGrpControl, FTPdtForSystem, FCPdtQtyOrdBuy, FCPdtCostDef, FCPdtCostOth, FCPdtCostStd, FCPdtMin, FCPdtMax, FTPdtPoint, FCPdtPointTime");
                    oSql.AppendLine(", FTPdtType, FTPdtSaleType, FTPdtSetOrSN, FTPdtStaSetPri, FTPdtStaSetShwDT, FTPdtStaAlwDis, FTPdtStaAlwReturn, FTPdtStaVatBuy, FTPdtStaVat, FTPdtStaActive");
                    oSql.AppendLine(", FTPdtStaAlwReCalOpt, FTPdtStaCsm, FTTcgCode, FTPgpChain, FTPtyCode, FTPbnCode, FTPmoCode, FTVatCode,FTEvhCode");
                    oSql.AppendLine(", FDPdtSaleStart, FDPdtSaleStop, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT DISTINCT FTPdtCode,'1' AS FTPdtStkControl,NULL As FTPdtGrpControl,'1' FTPdtForSystem,0 FCPdtQtyOrdBuy,0 FCPdtCostDef, 0 FCPdtCostOth,0 FCPdtCostStd,0 FCPdtMin,0 FCPdtMax,FTPdtPoint,0 FCPdtPointTime");
                    oSql.AppendLine(", FTPdtType,'1' FTPdtSaleType,'1' FTPdtSetOrSN,NULL FTPdtStaSetPri,NULL FTPdtStaSetShwDT,CASE ISNULL(FTPdtStaAlwDis,'') WHEN '' THEN '1' ELSE FTPdtStaAlwDis END  AS FTPdtStaAlwDis,'1' FTPdtStaAlwReturn,'1'  AS FTPdtStaVatBuy,(CASE ISNULL(FTPdtStaVat,'') WHEN '' THEN '1' ELSE FTPdtStaVat END) AS FTPdtStaVat,'1' FTPdtStaActive");
                    oSql.AppendLine(", '1' FTPdtStaAlwReCalOpt,'1' FTPdtStaCsm,'' As FTTcgCode,FTPgpChain,'' FTPtyCode,ISNULL(FTPbnCode,'') AS FTPbnCode,'' FTPmoCode,FTVatCode,'' FTEvhCode");
                    oSql.AppendLine(", GETDATE() AS FDPdtSaleStart,'99991231' AS FDPdtSaleStop,GETDATE() FDLastUpdOn,'AdaLink' FTLastUpdBy,GETDATE() FDCreateOn,'AdaLink' FTCreateBy");
                    oSql.AppendLine("FROM TTmpLinkPDT WITH(NOLOCK)");
                                        
                    //Insert
                    oSql.AppendLine("INSERT INTO TCNMPdt_L(FTPdtCode, FNLngID, FTPdtName, FTPdtNameABB, FTPdtNameOth, FTPdtRmk)");
                    oSql.AppendLine("SELECT DISTINCT FTPdtCode,1 FNLngID,FTPdtName,SUBSTRING(FTPdtName,1,"+ cVB.nVB_SubStr + ") As FTPdtNameABB,FTPdtNameOth,'' FTPdtRmk");
                    oSql.AppendLine("FROM TTmpLinkPDT WITH(NOLOCK)");
                                                         
                    //Insert
                    oSql.AppendLine("INSERT INTO TCNMPdtBar(FTPdtCode, FTBarCode, FTPunCode, FTBarStaUse, FTBarStaAlwSale,FTBarStaByGen,FTPlcCode,FNPldSeq, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT DISTINCT FTPdtCode,FTPdtCode,MIN(FTPunCode),'1' FTBarStaUse,'1' FTBarStaAlwSale,NULL As FTBarStaByGen,");
                    oSql.AppendLine("NULL As FTPlcCode,0 As FNPldSeq,");
                    oSql.AppendLine("GETDATE() FDLastUpdOn,'AdaLink' FTLastUpdBy,GETDATE() FDCreateOn,'AdaLink' FTCreateBy");
                    oSql.AppendLine("FROM TTmpLinkPDT WITH(NOLOCK)");
                    oSql.AppendLine("GROUP BY FTPdtCode");
                    
                    //Insert
                    oSql.AppendLine("INSERT INTO TCNMPdtPackSize(FTPdtCode, FTPunCode, FCPdtUnitFact,FCPdtPriceRET,FCPdtPriceWHS,FCPdtPriceNET,FTPdtGrade, FCPdtWeight,FTClrCode,FTPszCode,FTPdtUnitDim,FTPdtPkgDim,FTPdtStaAlwPick,FTPdtStaAlwPoHQ,FTPdtStaAlwBuy,FTPdtStaAlwSale, FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    oSql.AppendLine("SELECT DISTINCT FTPdtCode,FTPunCode,1 As FCPdtUnitFact,0,0,0,NULL FTPdtGrade,0 FCPdtWeight,NULL FTClrCode,NULL FTPszCode,'','',2,2,2,2,GETDATE() FDLastUpdOn,'AdaLink' FTLastUpdBy,GETDATE() FDCreateOn,'AdaLink' FTCreateBy");
                    oSql.AppendLine("FROM TTmpLinkPDT WITH(NOLOCK)");

                    oSql.AppendLine("  COMMIT");
                    
                    oSql.AppendLine("END TRY");
                    oSql.AppendLine("BEGIN CATCH");
                    oSql.AppendLine("  ROLLBACK");
                    oSql.AppendLine("END CATCH");

                    oDbTbl = oDb.C_GEToSQLToDatatable(oSql.ToString());
                    nQtyDone = nQtyDone + Convert.ToInt32(oDbTbl.Rows[0]["NumInsert"].ToString());
                }
                  
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_INSxDatabase",oEx.Message.ToString()); }
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
                new cSP().C_CHKxPathFile(cVB.tVB_PathIN);

                tPathXml = cVB.tVB_PathIN + @"\" + ptXmlName;

                XmlDocument oXmlDoc = new XmlDocument();
                oXmlDoc.Load(tPathXml);
                XmlNodeList oXmlList = oXmlDoc.SelectNodes("/TCNMPDT/PDTS");
                int nCount = 0;

                foreach (XmlNode oXml in oXmlList)
                {
                    
                    if (string.IsNullOrEmpty(oXml["FTPdtName"].InnerText))
                    {
                        new cLog().C_PRCxLogError(ptXmlName, "รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีชื่อสินค้า");
                        bStatus = false;
                    }
                    else if (string.IsNullOrEmpty(oXml["FTPdtStaVat"].InnerText))
                    {
                        new cLog().C_PRCxLogError(ptXmlName, "รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีค่า FTPdtStaVat");
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
                        oDbTblPdt.Rows.Add(oDbRow);
                        
                    }
                    nCount++;

                }
                nQtyAll = nQtyAll + nCount;
                if (!bStatus)
                {
                    return bStatus = false;
                }
                
            }
            catch(Exception oEx) { new cLog().C_PRCxLog("cMaster", "C_CHKxPathFile : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return bStatus;
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
            return oDbTbl;
        }

        
       
    }
}
