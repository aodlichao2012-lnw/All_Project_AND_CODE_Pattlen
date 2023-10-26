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
    class cPrice
    {
        DataTable oDbTblPri = new DataTable();
        int nQtyAll = 0;
        int nQtyDone = 0;
        List<string> aFileXml;
        List<string> aFileXmlError;
        public void C_PRCxPrice(List<string> paFileXml)
        {
            cSP oSP = new cSP();
            aFileXml = new List<string>();
            aFileXmlError = new List<string>();
            oDbTblPri = new DataTable();
            cVB.tVB_MasBackUP = null;
            try
            {
                    cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (paFileXml.Count > 0)
                    {
                        oDbTblPri = C_SEToColumnPrice();
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
                            new cSP().C_SAVxHistory("1", "ข้อมูลใบปรับราคา", cVB.tVB_MasBackUP + ".zip", "1", nQtyAll, nQtyDone, "2");
                        }
                    }
                    else
                    {
                    new cSP().C_SAVxHistory("1", "ข้อมูลใบปรับราคา", "ไม่มี File Import", "2", nQtyAll, nQtyDone, "2");
                }
                
            }
            catch(Exception oEx) { new cLog().C_PRCxLog("C_PRCxPrice",oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        public DataTable C_SEToColumnPrice()
        {
            DataTable oDbTbl = new DataTable();

            oDbTbl.Columns.Add("FTPdtCode", typeof(string));
            oDbTbl.Columns.Add("FTPplCode", typeof(string));
            oDbTbl.Columns.Add("FDXphDStart", typeof(string));
            oDbTbl.Columns.Add("FTXphTStart", typeof(string));
            oDbTbl.Columns.Add("FTPunCode", typeof(string));
            oDbTbl.Columns.Add("FCXpdPriceRet", typeof(string));
            return oDbTbl;
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
                XmlNodeList oXmlList = oXmlDoc.SelectNodes("/TCNTPdtAdjPriHD/TCNTPdtAdjPriDT");
                int nCount = 0;

                foreach (XmlNode oXml in oXmlList)
                {
                    if (string.IsNullOrEmpty(oXml["FCXpdPriceRet"].InnerText))
                    {
                        new cLog().C_PRCxLogError(ptXmlName, "รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีราคาสินค้า");
                        bStatus = false;
                    }
                    else if (string.IsNullOrEmpty(oXml["FTPunCode"].InnerText))
                    {
                        new cLog().C_PRCxLogError(ptXmlName, "รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีค่า FTPunCode");
                        bStatus = false;
                    }
                    else
                    {
                        DataRow oDbRow = oDbTblPri.NewRow();
                        oDbRow["FTPdtCode"] = oXml["FTPdtCode"].InnerText;
                        oDbRow["FTPplCode"] = oXml["FTPplCode"].InnerText;
                        oDbRow["FDXphDStart"] = oXml["FDXphDStart"].InnerText;
                        oDbRow["FTXphTStart"] = oXml["FTXphTStart"].InnerText;
                        oDbRow["FTPunCode"] = oXml["FTPunCode"].InnerText;
                        oDbRow["FCXpdPriceRet"] = oXml["FCXpdPriceRet"].InnerText;
                        oDbTblPri.Rows.Add(oDbRow);
                    }                    
                                                          
                    nCount++;
                }
                nQtyAll = nQtyAll + nCount;
                if (!bStatus)
                {
                    return bStatus = false;
                }
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_CHKxFileXml",oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return bStatus;
        }

        public void C_INSxDatabase()
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            bool bPrc = false;
            string tDocNo = "";
            try
            {
                oSql.Clear();
                oSql.AppendLine("DROP TABLE TTmpLinkAdjPri");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("IF OBJECT_ID(N'TTmpLinkAdjPri') IS NULL BEGIN");
                oSql.AppendLine("    CREATE TABLE[dbo].[TTmpLinkAdjPri](");
                oSql.AppendLine("        [FTPdtCode][nvarchar](50) NULL,");
                oSql.AppendLine("        [FTPplCode][nvarchar](10) NULL,");
                oSql.AppendLine("        [FDXphDStart][nvarchar](50) NULL,");
                oSql.AppendLine("        [FTXphTStart] [nvarchar] (50) NULL,");
                oSql.AppendLine("        [FTPunCode] [nvarchar] (50) NULL,");
                oSql.AppendLine("        [FCXpdPriceRet] [nvarchar] (50) NULL");
                oSql.AppendLine("    ) ON [PRIMARY]");
                oSql.AppendLine("END");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());
                               
                oSql.Clear();
                oSql.AppendLine("TRUNCATE TABLE TTmpLinkAdjPri");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_Conn, SqlBulkCopyOptions.Default))
                {

                    foreach (DataColumn oColName in oDbTblPri.Columns)
                    {
                        oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
                    }
                    //+++++++++++++++
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TTmpLinkAdjPri";

                    try
                    {
                        oBulkCopy.WriteToServer(oDbTblPri);
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
                    DataTable oDbTblMQ = new DataTable();
                    oDbTblMQ = C_SEToColApprove();

                    tDocNo = new cSP().C_PRCtDocNoImport();
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNTPdtAdjPriHD(FTBchCode,FTXphDocNo,FTXphDocType,FTXphStaAdj,FDXphDocDate,FTXphDocTime,FTPplCode");
                    oSql.AppendLine(",FDXphDStart,FTXphTStart,FDXphDStop,FTXphTStop,FTXphPriType,FTXphStaDoc,FTXphStaApv,FTXphStaPrcDoc,FNXphStaDocAct,FTUsrCode,FTXphUsrApv,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("SELECT distinct '"+cVB.tVB_Branch+"','" + tDocNo + "','1','1',CONVERT(varchar(10),GETDATE(),121),CONVERT(varchar(10),GETDATE(),108),FTPplCode");
                    oSql.AppendLine(",Convert(Date,GETDATE()),CONVERT(varchar(10),GETDATE(),108),Convert(Date,'99991231'),'23:59','1','1',NULL,NULL,1,'Interface',NULL,GETDATE(),'Interface',GETDATE(),'Interface' FROM TTmpLinkAdjPri");                    
                    oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());
                    //string tDocNoZKP1 = new cSP().C_PRCtDocNoImport();
                                                                             
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNTPdtAdjPriDT(FTBchCode,FTXphDocNo,FNXpdSeq,FTPdtCode,FTPunCode,FCXpdPriceRet,FCXpdPriceWhs,FCXpdPriceNet,FTXpdShpTo,FTXpdBchTo,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("SELECT distinct '" + cVB.tVB_Branch + "','" + tDocNo + "',ROW_NUMBER() OVER (ORDER BY FTPdtCode,FDXphDStart),FTPdtCode,FTPunCode,CONVERT(DECIMAL(18,4),FCXpdPriceRet),0,0,'','',GETDATE(),'Interface',GETDATE(),'Interface'");
                    oSql.AppendLine("FROM TTmpLinkAdjPri");
                    oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());
                    DataRow oRow = oDbTblMQ.NewRow();
                    oRow["FTBchCode"] = cVB.tVB_Branch;
                    oRow["FTXphDocNo"] = tDocNo;
                    oRow["FTXphDocType"] = "1";
                    oDbTblMQ.Rows.Add(oRow);
                    cVB.oTblAdj = oDbTblMQ;
                }
                

            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_INSxDatabase", oEx.Message.ToString()); }
            finally
            {

            }
        }
        public DataTable C_SEToColApprove()
        {
            DataTable oDbTbl = new DataTable();
            oDbTbl.Columns.Add("FTBchCode", typeof(string));
            oDbTbl.Columns.Add("FTXphDocNo", typeof(string));
            oDbTbl.Columns.Add("FTXphDocType", typeof(string));
            return oDbTbl;
        }
    }
}
