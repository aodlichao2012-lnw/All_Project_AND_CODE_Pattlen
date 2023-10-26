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
    class cEmployee
    {
        DataTable oDbTblEmp = new DataTable();
        int nQtyAll = 0;
        int nQtyDone = 0;
        List<string> aFileXml;
        List<string> aFileXmlError;
        public void C_PRCxEmployee(List<string> paFileXml)
        {
            cSP oSP = new cSP();
            aFileXml = new List<string>();
            aFileXmlError = new List<string>();
            oDbTblEmp = new DataTable();
            cVB.tVB_MasBackUP = null;

            try
            {
                cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (paFileXml.Count > 0)
                {
                    oDbTblEmp = C_SEToColumnEmp();
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
                        new cSP().C_SAVxHistory("1", "ข้อมูลลูกค้า", cVB.tVB_MasBackUP + ".zip", "1", nQtyAll, nQtyDone, "2");
                    }

                }
                else
                {
                    new cSP().C_SAVxHistory("1", "ข้อมูลลูกค้า", cVB.tVB_MasBackUP + ".zip", "1", nQtyAll, nQtyDone, "2");
                }
                
            }
            catch(Exception oEx) { new cLog().C_PRCxLog("C_PRCxEmployee", oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        public DataTable C_SEToColumnEmp()
        {
            DataTable oDbTbl = new DataTable();

            oDbTbl.Columns.Add("FTUsrCode", typeof(string));
            oDbTbl.Columns.Add("FDCreateOn", typeof(string));
            oDbTbl.Columns.Add("FTCreateBy", typeof(string));
            oDbTbl.Columns.Add("FTMerCode", typeof(string));
            oDbTbl.Columns.Add("FTUsrName", typeof(string));
            return oDbTbl;
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
                oSql.AppendLine("DROP TABLE TTmpLinkUser");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("IF OBJECT_ID(N'TTmpLinkUser') IS NULL BEGIN");
                oSql.AppendLine("    CREATE TABLE[dbo].[TTmpLinkUser](");
                oSql.AppendLine("        [FTUsrCode][nvarchar](50) NULL,");
                oSql.AppendLine("        [FDCreateOn][nvarchar](50) NULL,");
                oSql.AppendLine("        [FTCreateBy][nvarchar](50) NULL,");
                oSql.AppendLine("        [FTMerCode] [nvarchar] (50) NULL,");
                oSql.AppendLine("        [FTUsrName] [nvarchar] (50) NULL");
                oSql.AppendLine("    ) ON [PRIMARY]");
                oSql.AppendLine("END");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("TRUNCATE TABLE TTmpLinkUser");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_Conn, SqlBulkCopyOptions.Default))
                {

                    foreach (DataColumn oColName in oDbTblEmp.Columns)
                    {
                        oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
                    }
                    //+++++++++++++++
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TTmpLinkUser";

                    try
                    {
                        oBulkCopy.WriteToServer(oDbTblEmp);
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
                    oSql.Clear();
                    oSql.AppendLine("BEGIN TRANSACTION");
                    oSql.AppendLine("  BEGIN TRY");

                    oSql.AppendLine("UPDATE USR");
                    oSql.AppendLine("SET FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("FTLastUpdBy = TMP.FTCreateBy");
                    oSql.AppendLine("FROM TCNMUser USR");
                    oSql.AppendLine("INNER JOIN TTmpLinkUser TMP ON USR.FTUsrCode = TMP.FTUsrCode");

                    oSql.AppendLine("UPDATE USRLogin");
                    oSql.AppendLine("SET FDUsrPwdStart = GETDATE(),");
                    oSql.AppendLine("FTUsrStaActive = '3'");
                    oSql.AppendLine("FROM TCNMUsrLogin USRLogin");
                    oSql.AppendLine("INNER JOIN TTmpLinkUser TMP ON USRLogin.FTUsrCode = TMP.FTUsrCode");

                    oSql.AppendLine("UPDATE Usr_L");
                    oSql.AppendLine("SET FTUsrName = TMP.FTUsrName");
                    oSql.AppendLine("FROM TCNMUser_L Usr_L");
                    oSql.AppendLine("INNER JOIN TTmpLinkUser TMP ON Usr_L.FTUsrCode = TMP.FTUsrCode");

                    oSql.AppendLine("UPDATE UsrGroup");
                    oSql.AppendLine("SET FTBchCode = Tmp.FTMerCode,");
                    oSql.AppendLine("FTMerCode = TMP.FTMerCode,");
                    oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("FTLastUpdBy = TMP.FTCreateBy");
                    oSql.AppendLine("FROM TCNTUsrGroup UsrGroup");
                    oSql.AppendLine("INNER JOIN TTmpLinkUser TMP ON UsrGroup.FTUsrCode = TMP.FTUsrCode");

                    oSql.AppendLine("  SELECT DISTINCT COUNT(*) AS NumInsert FROM TTmpLinkUser");

                    oSql.AppendLine("DELETE TMP WITH(ROWLOCK)");
                    oSql.AppendLine("FROM TTmpLinkUser TMP");
                    oSql.AppendLine("INNER JOIN TCNMUser Usr WITH(NOLOCK) ON TMP.FTUsrCode = Usr.FTUsrCode");
                    
                    oSql.AppendLine("INSERT INTO TCNMUser(FTUsrCode,FTDptCode,FTUsrEmail,FTUsrTel,");
                    oSql.AppendLine("FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("SELECT distinct Tmp.FTUsrCode,'' As FTDptCode,'' AS FTUsrEmail,'' AS FTUsrTel,");                   
                    oSql.AppendLine(" CONVERT(DATETIME,Tmp.FDCreateOn) AS FDLastUpdOn,Tmp.FTCreateBy AS FTLastUpdBy,");
                    oSql.AppendLine(" CONVERT(DATETIME,Tmp.FDCreateOn) AS FDCreateOn,Tmp.FTCreateBy AS FTCreateBy FROM TTmpLinkUser Tmp");
                    
                    oSql.AppendLine("INSERT INTO TCNMUsrLogin(FTUsrCode,FTUsrLogType,FDUsrPwdStart,FDUsrPwdExpired,FTUsrLogin,");
                    oSql.AppendLine("FTUsrLoginPwd,FTUsrRmk,FTUsrStaActive)");
                    oSql.AppendLine("SELECT DISTINCT Tmp.FTUsrCode,'1' As FTUsrLogType,GETDATE() As FDUsrPwdStart,CONVERT(DATETIME,'99991231') As FDUsrPwdExpired,");
                    oSql.AppendLine("Tmp.FTUsrCode,'' As FTUsrLoginPwd,'' As FTUsrRmk,'3' As FTUsrStaActive  FROM TTmpLinkUser Tmp");
                    
                    oSql.AppendLine("INSERT INTO TCNMUser_L(FTUsrCode,FNLngID,FTUsrName,FTUsrRmk)");
                    oSql.AppendLine("SELECT DISTINCT Tmp.FTUsrCode,1 AS FNLngID, Tmp.FTUsrName AS FTUsrName, '' AS FTUsrRmk  FROM TTmpLinkUser Tmp");
                    
                    oSql.AppendLine("INSERT INTO TCNTUsrGroup(FTUsrCode,FTBchCode,FTShpCode,FTMerCode,FTAgnCode,FDCreateOn,FTCreateBy,FDLastUpdOn,FTLastUpdBy)");
                    oSql.AppendLine("SELECT DISTINCT Tmp.FTUsrCode,Tmp.FTMerCode,'' As FTShpCode,Tmp.FTMerCode,'',");
                    oSql.AppendLine("CONVERT(DATETIME,Tmp.FDCreateOn) AS FDLastUpdOn, Tmp.FTCreateBy, ");
                    oSql.AppendLine(" CONVERT(DATETIME,Tmp.FDCreateOn) AS FDCreateOn, Tmp.FTCreateBy FROM TTmpLinkUser Tmp ");

                    oSql.AppendLine("  COMMIT");                    
                    oSql.AppendLine("END TRY");
                    oSql.AppendLine("BEGIN CATCH");
                    oSql.AppendLine("  ROLLBACK");
                    oSql.AppendLine("END CATCH");

                    oDbTbl = oDb.C_GEToSQLToDatatable(oSql.ToString());
                    nQtyDone = nQtyDone + Convert.ToInt32(oDbTbl.Rows[0]["NumInsert"].ToString());
                }
            }
            catch (Exception oEx)
            {

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
                XmlNodeList oXmlList = oXmlDoc.SelectNodes("/EMPLOYEE/TCNMUser");
                int nCount = 0;

                foreach (XmlNode oXml in oXmlList)
                {
                    if (string.IsNullOrEmpty(oXml["FTUsrName"].InnerText))
                    {
                        new cLog().C_PRCxLogError(ptXmlName, "รหัสลูกค้า : " + oXml["FTUsrCode"].InnerText + " : ไม่มีได้ระบุชื่อลูกค้า");
                        bStatus = false;
                    }
                    else
                    {
                        DataRow oDbRow = oDbTblEmp.NewRow();
                        oDbRow["FTUsrCode"] = oXml["FTUsrCode"].InnerText;
                        oDbRow["FDCreateOn"] = oXml["FDCreateOn"].InnerText;
                        oDbRow["FTCreateBy"] = oXml["FTCreateBy"].InnerText;
                        oDbRow["FTMerCode"] = oXml["FTMerCode"].InnerText;
                        oDbRow["FTUsrName"] = oXml["FTUsrName"].InnerText;
                        oDbTblEmp.Rows.Add(oDbRow);
                    }
                    

                    nCount++;
                }
                nQtyAll = nQtyAll + nCount;
                if (!bStatus)
                {
                    return bStatus = false;
                }
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("cEmployee", "C_CHKxFileXml : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return bStatus;
        }
    }
}
