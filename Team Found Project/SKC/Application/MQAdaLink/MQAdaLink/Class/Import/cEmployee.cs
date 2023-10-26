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
    class cEmployee
    {

        DataTable oDbTblEmp = new DataTable();
        int nQtyAll = 0;
        int nQtyDone = 0;
        int nQtyError = 0;
        List<string> aFileXml;
        List<string> aFileXmlError;
        List<string> aFileXmlDul;
        public void C_PRCxEmployee(List<string> paFileXml)
        {
            Console.WriteLine("");
            Console.WriteLine("Import Employee Process Start...");
            cSP oSP = new cSP();
            aFileXml = new List<string>();
            aFileXmlError = new List<string>();
            aFileXmlDul = new List<string>();
            oDbTblEmp = new DataTable();
            //cVB.tVB_MasBackUP = null; //*Arm 63-08-21 Comment code
            try
            {
                new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : start."); //*Arm 63-08-21
                //cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss"); //*Arm 63-08-21 Comment code
                if (paFileXml.Count > 0)
                {
                    new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : FileXml > 0"); //*Arm 63-08-21

                    new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : C_SEToColumnEmp start."); //*Arm 63-08-21
                    oDbTblEmp = C_SEToColumnEmp();
                    new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : C_SEToColumnEmp end."); //*Arm 63-08-21

                    new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : loop."); //*Arm 63-08-21
                    foreach (string tXmlname in paFileXml)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Check File Employee       {0}", tXmlname);
                        new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Check File Employee "+ tXmlname); //*Arm 63-08-21

                        if (new cSP().C_CHKbHistory(tXmlname) == true) //Check History (TLKTHistory)
                        {
                            //ยังไม่มีใน History (TLKTHistory)
                            new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : " + tXmlname + " not found in TLKTHistory");
                            if (C_CHKxFileXml(tXmlname) == true) //check path file
                            {
                                new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Check file " + tXmlname + " = true");
                                Console.WriteLine("");
                                Console.WriteLine("Get Data From XML To DataTable    ======> Success");
                                aFileXml.Add(tXmlname);
                                if (aFileXml.Count > 0)
                                {
                                    Console.WriteLine("Process Insert Employee       {0} ======> Start", tXmlname);
                                    new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : C_INSxDatabase start."); //*Arm 63-08-21
                                    C_INSxDatabase(tXmlname);
                                    new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : C_INSxDatabase end."); //*Arm 63-08-21
                                    Console.WriteLine("");
                                    Console.WriteLine("Process Insert Employee       {0} ======> Success", tXmlname);
                                }
                                Console.WriteLine("");
                                Console.WriteLine("Insert History    ======> Start");

                                new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : C_PRCxInsertHistory start."); //*Arm 63-08-21
                                new cSP().C_PRCxInsertHistory(tXmlname);
                                new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : C_PRCxInsertHistory end."); //*Arm 63-08-21

                                Console.WriteLine("Insert History    ======> End");
                            }
                            else
                            {
                                new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : check file Error."); //*Arm 63-08-21
                                Console.WriteLine("Get Data From XML To DataTable    ======> False");
                                aFileXmlError.Add(tXmlname);
                            }
                        }
                        else
                        {
                            //มีข้อมูลใน History (TLKTHistory) แล้ว
                            new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Duplicate"); //*Arm 63-08-21

                            Console.WriteLine("File XML Duplicate     ======> False");
                            //new cSP().C_PRCxLogFile("ไฟล์ : " + tXmlname + " > ไฟล์ซ้ำ ");
                            new cSP().C_PRCxDataTableToFile(null, "EMPLO(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error","FileError", tXmlname);
                            aFileXmlDul.Add(tXmlname);
                        }
                                
                    }

                    if (aFileXmlError.Count > 0)
                    {
                        new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Move file."); //*Arm 63-08-21
                        new cSP().C_PRCxMoveFile(aFileXmlError, "Error");
                    }
                    if (aFileXmlDul.Count > 0)
                    {
                        new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Move file."); //*Arm 63-08-21
                        new cSP().C_PRCxMoveFile(aFileXmlDul, "Error");

                        new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : insert to TLKTLogHis."); //*Arm 63-08-21
                        new cSP().C_SAVxHistory("1", "ข้อมูลพนักงาน", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2");
                       
                    }
                    
                    if (aFileXml.Count > 0)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Process Move File Success To BackUP ====> Start");

                        new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Move file to BackUP."); //*Arm 63-08-21
                        new cSP().C_PRCxMoveFile(aFileXml, "Success");

                        Console.WriteLine("Process Move File Success To BackUP ====> End");
                        
                        if (nQtyDone == 0)
                        {
                            new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Case Done =0."); //*Arm 63-08-22
                            Console.WriteLine("");
                            Console.WriteLine("Process Insert Log History          ====> Start");
                            new cSP().C_SAVxHistory("1", "ข้อมูลพนักงาน", cVB.tVB_MasBackUP + ".zip", "2", nQtyAll, nQtyDone, "2");
                            Console.WriteLine("Process Insert Log History          ====> End");
                        }
                        else if(nQtyAll == nQtyDone)
                        {
                            new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Case Done = All."); //*Arm 63-08-22
                            Console.WriteLine("");
                            Console.WriteLine("Process Insert Log History          ====> Start");
                            new cSP().C_SAVxHistory("1", "ข้อมูลพนักงาน", cVB.tVB_MasBackUP + ".zip", "1", nQtyAll, nQtyDone, "2");
                            Console.WriteLine("Process Insert Log History          ====> End");
                        }
                        else
                        {
                            new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Case other"); //*Arm 63-08-22
                            Console.WriteLine("");
                            Console.WriteLine("Process Insert Log History          ====> Start");
                            new cSP().C_SAVxHistory("1", "ข้อมูลพนักงาน", cVB.tVB_MasBackUP + ".zip", "2", nQtyAll, nQtyDone, "2");
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
                    new cSP().C_SAVxHistory("1", "ข้อมูลพนักงาน", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2"); // ไม่มีไฟล์
                }

                new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : end."); //*Arm 63-08-21

            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cEmployee", "C_PRCxEmployee : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cEmployee", "C_PRCxEmployee : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
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
            oDbTbl.Columns.Add("FTFlag", typeof(string));
            oDbTbl.Columns.Add("FTDesc", typeof(string));
            return oDbTbl;
        }

        public void C_INSxDatabase(string ptFilename)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            DataTable oDbTbl = new DataTable();
            DataTable oDbTblError = new DataTable();
            DataTable oDbTblSucces = new DataTable();
            //DataTable oDbTblTotal = new DataTable();
            //DataTable oDbTblUpdate = new DataTable();
            int nPdtUpdate = 0;
            bool bPrc = false;
            try
            {
                oSql.Clear();
                oSql.AppendLine("DROP TABLE TTmpLinkActRole");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("IF OBJECT_ID(N'TTmpLinkActRole') IS NULL BEGIN");
                oSql.AppendLine("    CREATE TABLE[dbo].[TTmpLinkActRole](");
                oSql.AppendLine("        [FTUsrCode][nvarchar](50) NULL,");
                oSql.AppendLine("        [FDCreateOn][nvarchar](50) NULL,");
                oSql.AppendLine("        [FTCreateBy][nvarchar](50) NULL,");
                oSql.AppendLine("        [FTMerCode] [nvarchar] (50) NULL,");
                oSql.AppendLine("        [FTUsrName] [nvarchar] (50) NULL,");
                oSql.AppendLine("        [FTFlag] [nvarchar] (1) NULL,");
                oSql.AppendLine("        [FTDesc] [nvarchar] (255) NULL");
                oSql.AppendLine("    ) ON [PRIMARY]");
                oSql.AppendLine("END");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("TRUNCATE TABLE TTmpLinkActRole");
                new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : (ActRole) create TTmpLinkActRole/SQL/ " + oSql.ToString()); //*Arm 63-08-21
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : (ActRole) BulkCopy."); //*Arm 63-08-21
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_Conn, SqlBulkCopyOptions.Default))
                {

                    foreach (DataColumn oColName in oDbTblEmp.Columns)
                    {
                        oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
                    }
                    //+++++++++++++++
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TTmpLinkActRole";

                    try
                    {
                        oBulkCopy.WriteToServer(oDbTblEmp);
                        bPrc = true;
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_PRCxLog("cEmployee", "C_INSxDatabase : (ActRole) BulkCopy/TTmpLinkActRole/ " + oEx.Message.ToString());
                        new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : (ActRole) BulkCopy/TTmpLinkActRole/ " + oEx.Message.ToString()); //*Arm 63-08-27
                        bPrc = false;
                    }
                }
                if (bPrc)
                {
                    oSql.Clear();
                    oSql.AppendLine("DELETE Tmp WITH(ROWLOCK)");
                    oSql.AppendLine("FROM TTmpLinkActRole Tmp");
                    oSql.AppendLine("WHERE FTMerCode NOT IN (SELECT Tmp.FTMerCode FROM TCNMAgency Agn WHERE Agn.FTAgnRefCode = Tmp.FTMerCode)");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("UPDATE Usr SET");
                    oSql.AppendLine("FDLastUpdOn = GETDATE(),FTLastUpdBy = 'AdaLink'");
                    oSql.AppendLine("FROM TCNMUsrActRole Usr");
                    oSql.AppendLine("INNER JOIN TTmpLinkActRole TMP With(NOLOCK) ON TMP.FTUsrCode = Usr.FTUsrCode");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("DELETE TMP ");
                    oSql.AppendLine("FROM TTmpLinkActRole TMP");
                    oSql.AppendLine("INNER JOIN TCNMUsrActRole Usr With(NOLOCK) ON TMP.FTUsrCode = Usr.FTUsrCode");
                    oSql.AppendLine(" ");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMUsrActRole(FTRolCode,FTUsrCode,FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("SELECT DISTINCT '"+cVB.tVB_DefRole+ "',TMP.FTUsrCode,GETDATE(),'AdaLink',GETDATE(),'AdaLink'");
                    oSql.AppendLine("FROM TTmpLinkActRole TMP With(NOLOCK) ");
                    oSql.AppendLine(" ");

                    new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : (ActRole) DELETE-INSERT to TCNMUsrActRole/SQL/" + oSql.ToString());//*Arm 63-08-21
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                }

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
                oSql.AppendLine("        [FTUsrName] [nvarchar] (50) NULL,");
                oSql.AppendLine("        [FTFlag] [nvarchar] (1) NULL,");
                oSql.AppendLine("        [FTDesc] [nvarchar] (255) NULL");
                oSql.AppendLine("    ) ON [PRIMARY]");
                oSql.AppendLine("END");
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine("TRUNCATE TABLE TTmpLinkUser");

                new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : (User) create TTmpLinkUser/SQL/ " + oSql.ToString()); //*Arm 63-08-21
                oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

                Console.WriteLine("Process Copy DataTable To Temp    ======> Start");

                new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : (User) BulkCopy."); //*Arm 63-08-21
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
                        new cLog().C_PRCxLog("cEmployee", "C_INSxDatabase : (User) BulkCopy/TTmpLinkUser/ " + oEx.Message.ToString()); //*Arm 63-08-21
                        //new cLog().C_PRCxLog("C_INSxDatabase", oEx.Message.ToString());
                        bPrc = false;
                    }
                }
                Console.WriteLine("Process Copy DataTable To Temp    ======> Success");
                if (bPrc)
                {
                    new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : (User) Insert ");//*Arm 63-08-21
                    //ข้อมูล INSERT
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TTmpLinkUser SET FTFlag = '1' ");
                    oSql.AppendLine("WHERE FTUsrCode NOT IN ( select FTUsrCode from TCNMUser)");
                    oSql.AppendLine("");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    //ข้อมูล UPDATE
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TTmpLinkUser SET FTFlag = '2' ");
                    oSql.AppendLine("WHERE FTUsrCode IN ( select FTUsrCode from TCNMUser)");
                    oSql.AppendLine("");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    //ข้อมูล Error
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TTmpLinkUser SET FTDesc = CONCAT('ไม่พบข้อมูล AD ',FTMerCode) ");
                    oSql.AppendLine("WHERE FTMerCode NOT IN (SELECT FTAgnRefCode FROM TCNMAgency Agn WHERE Agn.FTAgnRefCode = FTMerCode)");
                    oSql.AppendLine("");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    //ข้อมูล AD
                    oSql.Clear();
                    oSql.AppendLine("UPDATE Tmp SET Tmp.FTMerCode = Agn.FTAgnCode ");
                    oSql.AppendLine("FROM TTmpLinkUser Tmp INNER JOIN TCNMAgency Agn ON Agn.FTAgnRefCode = Tmp.FTMerCode");
                    oSql.AppendLine("WHERE Tmp.FTDesc IS NULL AND Tmp.FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("");
                    Console.WriteLine("Process Check TCNMUser Duplicate  ======> Start");
                    oSql.Clear();
                    oSql.AppendLine("UPDATE USR");
                    oSql.AppendLine("SET FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("FTLastUpdBy = TMP.FTCreateBy");
                    oSql.AppendLine("FROM TCNMUser USR");
                    oSql.AppendLine("INNER JOIN TTmpLinkUser TMP ON USR.FTUsrCode = TMP.FTUsrCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Check TCNMUser Duplicate  ======> End");

                    
                    Console.WriteLine("Process Check TCNMUser_L Duplicate  ====> Start");

                    oSql.Clear();
                    oSql.AppendLine("UPDATE Usr_L");
                    oSql.AppendLine("SET FTUsrName = TMP.FTUsrName");
                    oSql.AppendLine("FROM TCNMUser_L Usr_L");
                    oSql.AppendLine("INNER JOIN TTmpLinkUser TMP ON Usr_L.FTUsrCode = TMP.FTUsrCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Check TCNMUser_L Duplicate  ====> End");
                    
                    Console.WriteLine("Process Check TCNTUsrGroup Duplicate  ==> Start");

                    oSql.Clear();
                    oSql.AppendLine("UPDATE UsrGroup");
                    oSql.AppendLine("SET ");
                    oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("FTLastUpdBy = TMP.FTCreateBy");
                    oSql.AppendLine("FROM TCNTUsrGroup UsrGroup");
                    oSql.AppendLine("INNER JOIN TTmpLinkUser TMP ON UsrGroup.FTUsrCode = TMP.FTUsrCode");
                    oSql.AppendLine("AND TMP.FTDesc IS NULL AND TMP.FTFlag = 2");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());

                    Console.WriteLine("Process Check TCNTUsrGroup Duplicate  ==> End");

                    
                    Console.WriteLine("");
                    Console.WriteLine("Process Insert Data TCNMUser      ======> Start");
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMUser(FTUsrCode,FTDptCode,FTUsrEmail,FTUsrTel,");
                    oSql.AppendLine("FDLastUpdOn,FTLastUpdBy,FDCreateOn,FTCreateBy)");
                    oSql.AppendLine("SELECT distinct Tmp.FTUsrCode,'' As FTDptCode,'' AS FTUsrEmail,'' AS FTUsrTel,");                   
                    oSql.AppendLine(" CONVERT(DATETIME,GETDATE()) AS FDLastUpdOn,Tmp.FTCreateBy AS FTLastUpdBy,");
                    oSql.AppendLine(" CONVERT(DATETIME,Tmp.FDCreateOn) AS FDCreateOn,Tmp.FTCreateBy AS FTCreateBy FROM TTmpLinkUser Tmp");
                    oSql.AppendLine("WHERE Tmp.FTDesc IS NULL AND Tmp.FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Insert Data TCNMUser      ======> End");

                    Console.WriteLine("Process Insert Data TCNMUsrLogin  ======> Start");
                    
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMUsrLogin(FTUsrCode,FTUsrLogType,FDUsrPwdStart,FDUsrPwdExpired,FTUsrLogin,");
                    oSql.AppendLine("FTUsrLoginPwd,FTUsrRmk,FTUsrStaActive)");
                    oSql.AppendLine("SELECT DISTINCT Tmp.FTUsrCode,'2' As FTUsrLogType,GETDATE() As FDUsrPwdStart,CONVERT(DATETIME,'99991231') As FDUsrPwdExpired,");
                    oSql.AppendLine("Tmp.FTUsrCode,'"+ cVB.tVB_DefPwd + "' As FTUsrLoginPwd,'' As FTUsrRmk,'3' As FTUsrStaActive  FROM TTmpLinkUser Tmp");
                    oSql.AppendLine("WHERE Tmp.FTDesc IS NULL AND Tmp.FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Insert Data TCNMUsrLogin  ======> End");

                    Console.WriteLine("Process Insert Data TCNMUser_L    ======> Start");
                    
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNMUser_L(FTUsrCode,FNLngID,FTUsrName,FTUsrRmk)");
                    oSql.AppendLine("SELECT DISTINCT Tmp.FTUsrCode,1 AS FNLngID, Tmp.FTUsrName AS FTUsrName, '' AS FTUsrRmk  FROM TTmpLinkUser Tmp");
                    oSql.AppendLine("WHERE Tmp.FTDesc IS NULL AND Tmp.FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Insert Data TCNMUser_L    ======> End");

                    Console.WriteLine("Process Insert Data TCNTUsrGroup  ======> Start");
                    
                    oSql.Clear();
                    oSql.AppendLine("INSERT INTO TCNTUsrGroup(FTUsrCode,FTBchCode,FTShpCode,FTMerCode,FTAgnCode,FDCreateOn,FTCreateBy,FDLastUpdOn,FTLastUpdBy)");
                    oSql.AppendLine("SELECT DISTINCT Tmp.FTUsrCode,'' As FTMerCode,'' As FTShpCode,'' As FTMerCode,Tmp.FTMerCode,");
                    oSql.AppendLine("CONVERT(DATETIME,GETDATE()) AS FDLastUpdOn, Tmp.FTCreateBy, ");
                    oSql.AppendLine(" CONVERT(DATETIME,Tmp.FDCreateOn) AS FDCreateOn, Tmp.FTCreateBy FROM TTmpLinkUser Tmp ");
                    //oSql.AppendLine(" INNER JOIN TCNMAgency Agn ON Tmp.FTMerCode = Agn.FTAgnRefCode ");
                    oSql.AppendLine("WHERE Tmp.FTDesc IS NULL AND Tmp.FTFlag = 1");
                    oDb.C_GEToSQLToDatatable(oSql.ToString());
                    Console.WriteLine("Process Insert Data TCNTUsrGroup  ======> End");

                    

                 
                    oSql.Clear();
                    oSql.AppendLine("select ROW_NUMBER() OVER ( ORDER BY FTUsrCode ) As Seq,Convert(varchar,GETDATE(),103) As InsertDate, FTUsrCode As Code,FTUsrName As FName,CASE WHEN FTFlag = 1 THEN 'Insert' ELSE 'Update' END As Remark from TTmpLinkUser ");
                    oSql.AppendLine("where FTDesc IS NULL");
                    new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : Find Csae Success/SQL/ " + oSql.ToString()); //*Arm 63-08-22
                    oDbTblSucces = oDb.C_GEToSQLToDatatable(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("select ROW_NUMBER() OVER ( ORDER BY FTUsrCode ) As Seq,Convert(varchar,GETDATE(),103) As InsertDate, FTUsrCode As Code,FTUsrName As FName,FTDesc As Remark from TTmpLinkUser ");
                    oSql.AppendLine("where FTDesc IS NOT NULL");
                    new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : Find Csae Error/SQL/ " + oSql.ToString()); //*Arm 63-08-22
                    oDbTblError = oDb.C_GEToSQLToDatatable(oSql.ToString());

                    nQtyDone = Convert.ToInt32(oDbTblSucces.Rows.Count);
                    new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : Done =" + nQtyDone.ToString()); //*Arm 63-08-22

                    nQtyError = nQtyError + Convert.ToInt32(oDbTblError.Rows.Count);
                    new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : Error =" + nQtyError.ToString()); //*Arm 63-08-22

                    new cSP().C_PRCxDataTableToFile(oDbTblSucces, "EMPLO(Success)-"+ DateTime.Now.ToString("yyyyMMdd"), "Success","EMPLO");
                    new cSP().C_PRCxDataTableToFile(oDbTblError, "EMPLO(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error", "EMPLO");
                    //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > แก้ไขข้อมูลพนักงานทั้งสิ้น        " + nPdtUpdate + " รายการ");
                    //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > บันทึกข้อมูลพนักงานสำเร็จทั้งสิ้น    " + Convert.ToInt32(oDbTblUpdate.Rows[0]["FInsert"].ToString()) + " รายการ");
                    //new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > บันทึกข้อมูลพนักงานไม่สำเร็จทั้งสิ้น  " + nQtyError + " รายการ");

                }
                else
                {
                    new cSP().C_PRCxLogFile("ไฟล์ : " + ptFilename + " > อ่านไฟล์ XML ไม่สำเร็จ");
                }
            }
            catch (Exception oEx)
            {
                Console.WriteLine("");
                Console.WriteLine("Process Insert Data Error ...");
                new cLog().C_PRCxLog("cEmployee","C_INSxDatabase : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cEmployee", "C_INSxDatabase : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
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
                new cLog().C_PRCxLogMonitor("cEmployee", "C_CHKxFileXml : ["+ ptXmlName + "] start."); //*Arm 63-08-21

                new cLog().C_PRCxLogMonitor("cEmployee", "C_CHKxFileXml : Check Path/" + cVB.tVB_PathIN); //*Arm 63-08-21
                new cSP().C_CHKxPathFile(cVB.tVB_PathIN);

                tPathXml = cVB.tVB_PathIN  + ptXmlName;

                XmlDocument oXmlDoc = new XmlDocument();
                oXmlDoc.Load(tPathXml);
                XmlNodeList oXmlList = oXmlDoc.SelectNodes("/EMPLOYEE/TCNMUser");
                int nCount = 0;

                Console.WriteLine("Get Data From XML To DataTable    ======> Start");
                foreach (XmlNode oXml in oXmlList)
                {
                    if (string.IsNullOrEmpty(oXml["FTUsrName"].InnerText))
                    {
                        new cSP().C_PRCxLogFile("ไฟล์ : " + ptXmlName + " > รหัสลูกค้า : " + oXml["FTUsrCode"].InnerText + " : ไม่มีได้ระบุชื่อลูกค้า");
                        //new cLog().C_PRCxLogError(ptXmlName, "รหัสลูกค้า : " + oXml["FTUsrCode"].InnerText + " : ไม่มีได้ระบุชื่อลูกค้า");
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
                Console.WriteLine("Get Employee From XML Total      ======> {0}", nQtyAll.ToString());
                if (!bStatus)
                {
                    return bStatus = false;
                }
                new cLog().C_PRCxLogMonitor("cEmployee", "C_CHKxFileXml : [" + ptXmlName + "] end."); //*Arm 63-08-21
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cEmployee", "C_CHKxFileXml : " + oEx.Message);
                new cLog().C_PRCxLogMonitor("cEmployee", "C_CHKxFileXml : " + oEx.Message); //*Arm 63-08-27
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return bStatus;
        }
    }
}
