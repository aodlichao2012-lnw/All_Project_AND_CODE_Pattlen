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
    class cPrice
    {
        DataTable oDbTblPri = new DataTable();
        int nQtyAll = 0;
        int nQtyDone = 0;
        List<string> aFileXml;
        List<string> aFileXmlError;
        List<string> aFileXmlDul;
        public void C_PRCxPrice(List<string> paFileXml)
        {
            Console.WriteLine("");
            Console.WriteLine("Import Employee Process Start...");
            cSP oSP = new cSP();
            aFileXml = new List<string>();
            aFileXmlError = new List<string>();
            aFileXmlDul = new List<string>();
            oDbTblPri = new DataTable();
            //cVB.tVB_MasBackUP = null;
            try
            {
                    //cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (paFileXml.Count > 0)
                    {
                        oDbTblPri = C_SEToColumnPrice();
                        foreach (string tXmlname in paFileXml)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Check File Price        {0}", tXmlname);
                            if (new cSP().C_CHKbHistory(tXmlname) == true)
                            {
                                if (C_CHKxFileXml(tXmlname) == true)
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("Get Data From XML To DataTable    ======> Success");
                                    aFileXml.Add(tXmlname);
                                    if (aFileXml.Count > 0)
                                    {
                                        //Console.WriteLine("Process Insert Master         {0} ======> Start", tXmlname);
                                        Console.WriteLine("Process Insert Price          {0} ======> Start", tXmlname);
                                        C_INSxDatabase(tXmlname);
                                        Console.WriteLine("");
                                        Console.WriteLine("Process Insert Price          {0} ======> End", tXmlname);
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
                                new cSP().C_PRCxDataTableToFile(null, "PRICE(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error", "FileError", tXmlname);
                                aFileXmlDul.Add(tXmlname);
                            }
                            
                        }

                        if (aFileXmlError.Count > 0)
                        {
                            new cSP().C_PRCxMoveFile(aFileXmlError, "Error");
                        }
                        if(aFileXmlDul.Count > 0)
                        {
                            new cSP().C_PRCxMoveFile(aFileXmlDul, "Error");                        
                            new cSP().C_SAVxHistory("1", "ใบปรับราคา", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2");
                            
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
                                new cSP().C_SAVxHistory("1", "ใบปรับราคา", cVB.tVB_MasBackUP + ".zip", "2", nQtyAll, nQtyDone, "2");
                                Console.WriteLine("Process Insert Log History          ====> End");
                            }
                            else if (nQtyAll == nQtyDone)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Process Insert Log History          ====> Start");
                                new cSP().C_SAVxHistory("1", "ใบปรับราคา", cVB.tVB_MasBackUP + ".zip", "1", nQtyAll, nQtyDone, "2");
                                Console.WriteLine("Process Insert Log History          ====> End");
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Process Insert Log History          ====> Start");
                                new cSP().C_SAVxHistory("1", "ใบปรับราคา", cVB.tVB_MasBackUP + ".zip", "2", nQtyAll, nQtyDone, "2");
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
                    new cSP().C_SAVxHistory("1", "ใบปรับราคา", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2");
                    }
                
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cPrice", "C_PRCxPrice : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cPrice","C_PRCxPrice : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
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
            oDbTbl.Columns.Add("FTFlag", typeof(string));
            oDbTbl.Columns.Add("FTDesc", typeof(string));
            return oDbTbl;
        }

        public bool C_CHKxFileXml(string ptXmlName)
        {
            bool bStatus = true;
            string tPathXml = "";


            try
            {
                new cSP().C_CHKxPathFile(cVB.tVB_PathIN);

                tPathXml = cVB.tVB_PathIN  + ptXmlName;

                XmlDocument oXmlDoc = new XmlDocument();
                oXmlDoc.Load(tPathXml);
                XmlNodeList oXmlList = oXmlDoc.SelectNodes("/TCNTPdtAdjPriHD/TCNTPdtAdjPriDT");
                int nCount = 0;

                Console.WriteLine("Get Data From XML To DataTable    ======> Start");
                foreach (XmlNode oXml in oXmlList)
                {
                    if (string.IsNullOrEmpty(oXml["FCXpdPriceRet"].InnerText))
                    {
                        //new cLog().C_PRCxLogError(ptXmlName, "รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีราคาสินค้า");
                        new cSP().C_PRCxLogFile("ไฟล์ : " + ptXmlName + " > รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีราคาสินค้า");
                        bStatus = false;
                    }
                    else if (string.IsNullOrEmpty(oXml["FTPunCode"].InnerText))
                    {
                        new cSP().C_PRCxLogFile("ไฟล์ : " + ptXmlName + " > รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีค่า FTPunCode");
                        //new cLog().C_PRCxLogError(ptXmlName, "รหัสสินค้า : " + oXml["FTPdtCode"].InnerText + " : ไม่มีค่า FTPunCode");
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
                Console.WriteLine("Get Price   From XML Total        ======> {0}", nQtyAll.ToString());
                if (!bStatus)
                {
                    return bStatus = false;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cPrice","C_CHKxFileXml : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cPrice", "C_CHKxFileXml : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return bStatus;
        }

        public void C_INSxDatabase(string ptFilename)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            bool bPrc = false;
            DataTable oDbTblError = new DataTable();
            DataTable oDbTblSucces = new DataTable();
            //string tDocNo = "";
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
                oSql.AppendLine("        [FCXpdPriceRet] [nvarchar] (50) NULL,");
                oSql.AppendLine("        [FTFlag] [nvarchar] (1) NULL,");
                oSql.AppendLine("        [FTDesc] [nvarchar] (255) NULL");
                oSql.AppendLine("    ) ON [PRIMARY]");
                oSql.AppendLine("END");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());
                               
                oSql.Clear();
                oSql.AppendLine("TRUNCATE TABLE TTmpLinkAdjPri");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                Console.WriteLine("Process Copy DataTable To Temp    ======> Start");
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
                Console.WriteLine("Process Copy DataTable To Temp    ======> End");
                if (bPrc)
                {
                    DataTable oDbTblMQ = new DataTable();
                    DataTable oDbTblCountAdj = new DataTable();
                    DataTable oDbTblGrp = new DataTable();
                    oDbTblMQ = C_SEToColApprove();
                    int nAdjSum = 0;
                    //oSql.Clear();
                    //oSql.AppendLine("DELETE  Pri from TTmpLinkAdjPri Pri");
                    //oSql.AppendLine("where Pri.FTPdtCode NOT IN (SELECT FTPdtCode FROM TCNMPdt pdt)");
                    //oDb.C_GEToSQLToDatatable(oSql.ToString());

                    ////ข้อมูล INSERT
                    //oSql.Clear();
                    //oSql.AppendLine("UPDATE TTmpLinkAdjPri SET FTFlag = '1' ");
                    //oSql.AppendLine("WHERE FTPdtCode NOT IN ( select FTPdtCode from TCNMPdt)");
                    //oSql.AppendLine("");
                    //oDb.C_GEToSQLToDatatable(oSql.ToString());
                    ////ข้อมูล UPDATE
                    //oSql.Clear();
                    //oSql.AppendLine("UPDATE TTmpLinkAdjPri SET FTFlag = '2' ");
                    //oSql.AppendLine("WHERE FTPdtCode IN ( select FTPdtCode from TCNMPdt)");
                    //oSql.AppendLine("");
                    //oDb.C_GEToSQLToDatatable(oSql.ToString());
                    //ข้อมูล Error
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TTmpLinkAdjPri SET FTDesc = 'ไม่พบข้อมูลสินค้าในตาราง TCNMPdt ' ");
                    oSql.AppendLine("WHERE FTPdtCode NOT IN (SELECT FTPdtCode FROM TCNMPdt)");
                    oSql.AppendLine("");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("select distinct FDXphDStart from TTmpLinkAdjPri");
                    oSql.AppendLine("where FTDesc IS NULL");
                    oSql.AppendLine("order by FDXphDStart desc");
                    oDbTblGrp = oDb.C_GEToSQLToDatatable(oSql.ToString());

                    for (int n =0; n < oDbTblGrp.Rows.Count; n++)
                    {
                            oSql.Clear();
                            oSql.AppendLine("SELECT COUNT(FDXphDStart) As AdjCount FROM TTmpLinkAdjPri");
                            oSql.AppendLine("Where FDXphDStart = '"+ oDbTblGrp.Rows[n]["FDXphDStart"].ToString() + "'");
                            oSql.AppendLine("and FTDesc IS NULL");
                            oDbTblCountAdj = oDb.C_GEToSQLToDatatable(oSql.ToString());

                            Console.WriteLine("");
                            Console.WriteLine("Process Check Price Total     ======> {0} ", oDbTblCountAdj.Rows[0]["AdjCount"].ToString());

                            int nAdjCount = Convert.ToInt32(oDbTblCountAdj.Rows[0]["AdjCount"].ToString());
                            int nMod = 0;
                            nMod = nAdjCount / 10000;
                            if (nMod == 0)
                            {
                                nMod = 1;
                            }
                            else
                            {
                                int nSpit2 = nAdjCount % 10000;
                                if (nSpit2 == 0)
                                {
                                    nMod = nMod + 0;
                                }
                                else
                                {
                                    nMod = nMod + 1;
                                }

                            }
                            int nCount = 0;
                            int nAdjPri = 0;
                            for (int i = 0; i < nMod; i++)
                            {
                                nAdjPri = i + 1;
                                Console.WriteLine("Process AdjPri No.{0} ", nAdjPri.ToString());

                                Console.WriteLine("Process Insert Data TCNTPdtAdjPriHD       ======> Start");
                                string tDocNo = "";
                                tDocNo = new cSP().C_PRCtDocNoImport();
                                oSql.Clear();
                                oSql.AppendLine("INSERT INTO TCNTPdtAdjPriHD  (FTBchCode,FTXphDocNo,FTXphDocType,FTXphStaAdj,FDXphDocDate,FTXphDocTime,FTPplCode");
                                oSql.AppendLine(",FDXphDStart,FTXphTStart,FDXphDStop,FTXphTStop,FTXphPriType,FTXphStaDoc,FTXphStaApv,FTXphStaPrcDoc,FNXphStaDocAct,FTUsrCode,FTXphUsrApv,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                                oSql.AppendLine("SELECT distinct '" + cVB.tVB_Branch + "','" + tDocNo + "','1','1',CONVERT(varchar(10),GETDATE(),121),CONVERT(varchar(10),GETDATE(),108),FTPplCode");
                                oSql.AppendLine(",Convert(Date,FDXphDStart),'00:00',Convert(Date,'99991231'),'23:59','1','1',NULL,NULL,1,'AdaLink',NULL,GETDATE(),'AdaLink',GETDATE(),'AdaLink' FROM TTmpLinkAdjPri With(NOLOCK)");
                                oSql.AppendLine("Where FDXphDStart = '" + oDbTblGrp.Rows[n]["FDXphDStart"].ToString() + "'");
                                oSql.AppendLine("and FTDesc IS NULL");
                                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());
                                //string tDocNoZKP1 = new cSP().C_PRCtDocNoImport();
                                Console.WriteLine("Process Insert Data TCNTPdtAdjPriHD       ======> End");

                                Console.WriteLine("Process Insert Data TCNTPdtAdjPriDT       ======> Start");

                                oSql.Clear();
                                oSql.AppendLine("INSERT INTO TCNTPdtAdjPriDT  (FNXpdSeq,FTBchCode,FTXphDocNo,FTPdtCode,FTPunCode,FCXpdPriceRet,FCXpdPriceWhs,FCXpdPriceNet,FTXpdShpTo,FTXpdBchTo,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                                oSql.AppendLine("SELECT ROW_NUMBER() OVER (ORDER BY A.FTBchCode),A.* FROM (");
                                oSql.AppendLine("SELECT '" + cVB.tVB_Branch + "' FTBchCode,'" + tDocNo + "' FTXphDocNo,FTPdtCode,FTPunCode,CONVERT(DECIMAL(18,4),FCXpdPriceRet) FCXpdPriceRet,0 FCXpdPriceWhs,0 FCXpdPriceNet,'' FTXpdShpTo,'' FTXpdBchTo,GETDATE() FDLastUpdOn,'AdaLink' FTLastUpdBy,GETDATE() FDCreateOn,'AdaLink' FTCreateBy");
                                oSql.AppendLine("FROM TTmpLinkAdjPri With(NOLOCK)");
                                oSql.AppendLine(" WHERE CONVERT(DECIMAL(18,4),FCXpdPriceRet) > 0");
                                oSql.AppendLine(" AND FDXphDStart = '" + oDbTblGrp.Rows[n]["FDXphDStart"].ToString() + "'");
                                oSql.AppendLine("and FTDesc IS NULL");
                                oSql.AppendLine("ORDER BY FDXphDstart ");
                                oSql.AppendLine("OFFSET " + nCount + " ROWS FETCH NEXT 10000 ROWS ONLY ) A");
                                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());
                                DataRow oRow = oDbTblMQ.NewRow();
                                oRow["FTBchCode"] = cVB.tVB_Branch;
                                oRow["FTXphDocNo"] = tDocNo;
                                oRow["FTXphDocType"] = "1";
                                oDbTblMQ.Rows.Add(oRow);
                                Console.WriteLine("Process Insert Data TCNTPdtAdjPriDT       ======> End");
                                nCount = nCount + 10000;
                            }
                        nAdjSum = nAdjSum + nAdjCount;


                        ////new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > แก้ไขรายการสินค้าทั้งสิ้น        " + nPdtUpdate + " รายการ");
                        //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > บันทึกใบปรับราคาของวันที่ " + oDbTblGrp.Rows[n]["FDXphDStart"].ToString() + " ");
                        //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > รายการสินค้าในใบปรับราคาทั้งสิ้น  " + nAdjCount + " รายการ");
                        //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > บันทึกรายการใบปรับราคาวันที่ " + oDbTblGrp.Rows[n]["FDXphDStart"].ToString() + " สำเร็จ ");

                    }

                    

                    cVB.oTblAdj = oDbTblMQ;
                    nQtyDone = nQtyDone + nAdjSum;
                    oSql.Clear();
                    oSql.AppendLine("select ROW_NUMBER() OVER ( ORDER BY FTPdtCode ) As Seq,Convert(varchar,GETDATE(),103) As InsertDate, FTPdtCode As Code,FTPunCode As Unit,REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,FCXpdPriceRet),1), '.00','') As Price,'Insert' As Remark from TTmpLinkAdjPri ");
                    oSql.AppendLine("where FTDesc IS NULL");
                    oDbTblSucces = oDb.C_GEToSQLToDatatable(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("select ROW_NUMBER() OVER ( ORDER BY FTPdtCode ) As Seq,Convert(varchar,GETDATE(),103) As InsertDate, FTPdtCode As Code,FTPunCode As Unit,REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,FCXpdPriceRet),1), '.00','') As Price,FTDesc As Remark from TTmpLinkAdjPri ");
                    oSql.AppendLine("where FTDesc IS NOT NULL");
                    oDbTblError = oDb.C_GEToSQLToDatatable(oSql.ToString());

                    new cSP().C_PRCxDataTableToFile(oDbTblSucces, "PRICE(Success)-" + DateTime.Now.ToString("yyyyMMdd"), "Success","PRICE");
                    new cSP().C_PRCxDataTableToFile(oDbTblError, "PRICE(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error", "PRICE");
                    //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > รวมใบปรับราคาที่บันทึกทั้งสิ้น " + oDbTblGrp.Rows.Count + " ใบ จำนวนสินค้าที่ปรับราคาทั้งสิ้น " + nQtyDone + " ");
                }
                else
                {
                    new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > อ่านไฟล์ XML ไม่สำเร็จ");
                }


            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cPrice","C_INSxDatabase : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cPrice", "C_INSxDatabase : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
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
