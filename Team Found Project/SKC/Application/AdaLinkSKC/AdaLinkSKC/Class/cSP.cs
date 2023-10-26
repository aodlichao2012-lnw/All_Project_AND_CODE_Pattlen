using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class
{
    class cSP
    {
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        public void SP_CLExMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
                GC.Collect();
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("SP_CLExMemory",oEx.Message.ToString()); }
            finally
            {

            }
        }

        public bool C_SAVxHistory(string ptLogType, 
            string ptLogTask, 
            string ptLogTaskRef, 
            string ptStaPrc, 
            int pnQtyAll,
            int pnQtyDone,
            string ptSendMail)
        {
            cDatabase oDb;
            StringBuilder oSql;
            string tFTInfCode = "";
            try
            {
                oDb = new cDatabase();
                oSql = new StringBuilder();
                // FTLogType 1=นำเข้า  2=ส่งออก
                // FTLogTaskRef ไฟล์ Zip ทั้ง Success และ Error
                // ptStaPrc สถานะงานที่ทำ  1=สำเร็จ  2=ไม่สำเร็จ
                oSql.Clear();
                oSql.AppendLine(" DECLARE @LogId bigint;");
                oSql.AppendLine(" SELECT @LogId = MAX(FNLogID) + 1  FROM TLKTLogHis");
                oSql.AppendLine(" INSERT INTO");
                oSql.AppendLine(" TLKTLogHis");
                oSql.AppendLine(" (");
                oSql.AppendLine(" FNLogID,FDLogCreate, FTLogType, FTLogTask, FTLogTaskRef, FTLogStaPrc, FNLogQtyAll,");
                oSql.AppendLine(" FNLogQtyDone, FTLogStaSend, FDCreateOn, FTCreateBy, FDLastUpdOn, FTLastUpdBy");
                oSql.AppendLine(" )");
                oSql.AppendLine(" VALUES");
                oSql.AppendLine(" (");
                oSql.AppendLine(" @LogId,GETDATE(), '" + ptLogType + "', '"+ ptLogTask + "', '"+ ptLogTaskRef + "', '"+ ptStaPrc + "', "+ pnQtyAll + ",");
                oSql.AppendLine(" "+ pnQtyDone + ", '"+ ptSendMail + "', GETDATE(), 'Interface', GETDATE(), 'Interface'");               
                oSql.AppendLine(" )");
                if (oDb.C_GETnSQLExecuteWithConnStr(oSql.ToString(), cVB.tVB_Conn) == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception oEx)
            {
                return false;
            }
        }

        public bool C_SAVxDownloadXML(List<string> paFileName,string tMode)
        {
            string tDirectory = "";
            //string tpath = "";
            IEnumerable<Renci.SshNet.Sftp.SftpFile> aFile;
            string[] aFilePaths = null;
            string[] aFileSplit = null;
            
            try
            {
                #if DEBUG
                //string tPath = Directory.GetCurrentDirectory();
                //tPath = string.Format(@"{0}\", tPath);
                
                switch (tMode)
                    {
                        case "Master":
                            aFilePaths = Directory.GetFiles(cVB.tVB_PathIN);
                            foreach (var oFile in aFilePaths)
                            {
                                string[] aNameSplip = null;
                                aFileSplit = oFile.Split('\\');
                                aNameSplip = aFileSplit[cVB.nVB_SplitURL].Split('_');
                                if (aNameSplip[0].Trim() == "MASTER")
                                {
                                    paFileName.Add(aFileSplit[cVB.nVB_SplitURL].ToString());
                                }                                
                            }
                        break;
                        case "Price":
                            aFilePaths = Directory.GetFiles(cVB.tVB_PathIN);
                            foreach (var oFile in aFilePaths)
                            {
                                string[] aNameSplip = null;
                                aFileSplit = oFile.Split('\\');
                                aNameSplip = aFileSplit[cVB.nVB_SplitURL].Split('_');
                                if (aNameSplip[0].Trim() == "PRICE")
                                {
                                    paFileName.Add(aFileSplit[cVB.nVB_SplitURL].ToString());
                                }
                            }
                        break;
                        case "Employee":
                            aFilePaths = Directory.GetFiles(cVB.tVB_PathIN);
                            foreach (var oFile in aFilePaths)
                            {
                                string[] aNameSplip = null;
                                aFileSplit = oFile.Split('\\');
                                aNameSplip = aFileSplit[cVB.nVB_SplitURL].Split('_');
                                if (aNameSplip[0].Trim() == "EMPLO")
                                {
                                    paFileName.Add(aFileSplit[cVB.nVB_SplitURL].ToString());
                                }
                            }
                        break;
                        default:
                            aFilePaths = Directory.GetFiles(cVB.tVB_PathIN);
                            foreach (var oFile in aFilePaths)
                            {
                                string[] aNameSplip = null;
                                aFileSplit = oFile.Split('\\');
                                //aNameSplip = aFileSplit[cVB.nVB_SplitURL].Split('_');
                                paFileName.Add(aFileSplit[cVB.nVB_SplitURL].ToString());
                            }
                        break;
                    }
                    

                #else
                    tDirectory = "/OUT/";
                    KeyboardInteractiveAuthenticationMethod oKauth = new KeyboardInteractiveAuthenticationMethod(cVB.tVB_UserSFTP);
                    PasswordAuthenticationMethod oPauth = new PasswordAuthenticationMethod(cVB.tVB_UserSFTP, cVB.tVB_PassSFTP);

                    oKauth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                    var tConnectionInfo = new ConnectionInfo(cVB.tVB_HostSFTP, 22, cVB.tVB_UserSFTP, oPauth, oKauth);

                    using (SftpClient oSFTP = new SftpClient(tConnectionInfo))
                    {
                        oSFTP.Connect();
                    
                        aFile = oSFTP.ListDirectory(tDirectory);
                        foreach (var tFile in aFile)
                        {
                        
                            File.WriteAllText(@"D:\AdaLinkMoshi\Import\LOG_Pdt2.txt", tFile.FullName.Substring(5));
                            if (tFile.FullName.Length > 10)
                            {

                                //Check Path
                                C_CHKxCheckFolder(cVB.tVB_PathXML);

                                //Download File
                                //using (Stream oFileStream = File.Create(tpath + @"\Import\DownloadXML\" + tFile))
                                using (Stream oFileStream = File.Create(cVB.tVB_PathXML + @"\"+tFile.FullName.Substring(5)))
                                {
                                    //File.WriteAllText(@"D:\AdaLinkMoshi\Import\LOG_Pdt.txt", cVB.tVB_PathXML + @"\" + tFile.FullName.Substring(5));
                                    oSFTP.BufferSize = 4 * 1024;
                                    //oSFTP.DownloadFile(tDirectory + tFile, oFileStream);
                                    oSFTP.DownloadFile( tFile.FullName, oFileStream);
                                    paFileName.Add(tFile.FullName.Substring(5));
                                }
                                //Delete File In Server FTP
                                //oSFTP.DeleteFile(tDirectory + tFile);
                                oSFTP.DeleteFile( tFile.FullName );
                            }
                        }
                        oSFTP.Dispose();
                        //oSFTP.Disconnect();
                    }
                #endif


                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_SAVxDownloadXML", oEx.Message.ToString());
                return false;
            }
        }

        public void C_CHKxPathFile(string ptPath)
        {            
            try
            {
                if (!Directory.Exists(ptPath))
                {
                    Directory.CreateDirectory(ptPath);                    
                }
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_CHKxPathFile",oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }            
        }

        public string C_PRCtDocNoImport()
        {
            cDatabase oDb = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tResult = "", tSubyear = "", tYear = "", tDocNo = "";
            string tDocNoImp = "";
            int nResult = 0, nDocNo = 1;
            try
            {
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 (FTXphDocNo) FROM TCNTPdtAdjPriHD WITH(NOLOCK) ORDER BY RIGHT(FTXphDocNo,6) DESC");
                //oSql.AppendLine("SELECT 'This is row = '" + nRow);
                tResult = oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());
                tYear = DateTime.Now.ToString("yyyy").Substring(2);
                if (!string.IsNullOrEmpty(tResult))
                {
                    tResult = tResult.Substring(7);
                    tSubyear = tResult.Substring(0, 2);
                    tResult = tResult.Substring(2);
                    if (tSubyear != tYear)
                    {
                        tResult = "1";
                    }
                }
                else
                {
                    tResult = "0";
                }
                nResult = Convert.ToInt32(tResult);
                nResult = nResult + nDocNo;
                if (nResult != 0)
                {
                    int nNum = Convert.ToInt32(Convert.ToString(nResult).Length);
                    tDocNo = "000000";
                    tDocNo = tDocNo.Substring(nNum);
                    tDocNoImp = string.Format("IP{0}{1}{2}{3}", cVB.tVB_Branch, tYear, tDocNo, nResult.ToString());
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_PRCtDocNoImport", oEx.Message.ToString()); ;
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return tDocNoImp;
        }

        public string C_GETtBranchCode()
        {
            string tBchCode = "";
            cDatabase oDb = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTBchcode FROM TCNMComp");
                //oSql.AppendLine("SELECT 'This is row = '" + nRow);
                tBchCode = oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

            }
            catch(Exception oEx) { new cLog().C_PRCxLog("C_GETtBranchCode", oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return tBchCode;
        }

        public void C_GETtBchProperties()
        {
            cDatabase oDb = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            try
            {
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 ISNULL(BCH.FTBchRefID,'') AS FTBchRefID, ISNULL(BCH.FTAgnCode,'') AS FTAgnCode, ISNULL(BCH.FTWahCode,'') AS FTWahCode, ISNULL(AGN.FTAgnRefCode,'') AS FTAgnRefCode, ISNULL(WAH.FTWahRefNo,'') AS FTWahRefNo, ISNULL(FTWahStaChannel,'') AS FTWahStaChannel");
                oSql.AppendLine("FROM TCNMBranch BCH WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMAgency AGN WITH(NOLOCK) ON BCH.FTAgnCode = AGN.FTAgnCode");
                oSql.AppendLine("LEFT JOIN TLKMWaHouse WAH  WITH(NOLOCK) ON  BCH.FTAgnCode =WAH.FTAgnCode AND BCH.FTBchCode = WAH.FTBchCode AND BCH.FTWahCode = WAH.FTWahCode");
                oSql.AppendLine("WHERE BCH.FTBchCode = '" + cVB.tVB_BchCode + "' ");
                odtTmp = oDb.C_GEToSQLToDatatable(oSql.ToString());
                if(odtTmp != null && odtTmp.Rows.Count >0)
                {
                    cVB.tVB_BchRefID = odtTmp.Rows[0].Field<string>("FTBchRefID");
                    cVB.tVB_AgnCode = odtTmp.Rows[0].Field<string>("FTAgnCode");
                    cVB.tVB_WahCode = odtTmp.Rows[0].Field<string>("FTWahCode");
                    cVB.tVB_SaleOrg = odtTmp.Rows[0].Field<string>("FTAgnRefCode");
                    cVB.tVB_Sloc = odtTmp.Rows[0].Field<string>("FTWahRefNo");
                    cVB.tVB_Channel = odtTmp.Rows[0].Field<string>("FTWahStaChannel");
                }
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_GETtBranchCode", oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get CmpCode (*Arm 63-07-03)
        /// </summary>
        /// <returns></returns>
        public string C_GETtCompany()
        {
            string tCmpCode = "";
            cDatabase oDb = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTCmpCode FROM TCNMComp WITH(NOLOCK)");
                tCmpCode = oDb.C_GETtSQLScalarWithConnStr(oSql.ToString());

            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_GETtCompany", oEx.Message.ToString());
            }
            finally
            {
                oDb = null;
                oSql = null;
                new cSP().SP_CLExMemory();
            }
            return tCmpCode;
        }

        public void C_PRCxBackUPFile(string ptFilename)
        {
            try
            {
                string tZipPath = cVB.tVB_PathBackUP + @"\" + ptFilename + ".zip";
                if (!Directory.Exists(cVB.tVB_PathBackUP))
                {
                    Directory.CreateDirectory(cVB.tVB_PathBackUP);
                }
                ZipFile.CreateFromDirectory(cVB.tVB_PathIN + @"\" + ptFilename, tZipPath);
                Directory.Delete(cVB.tVB_PathIN + @"\" + ptFilename,true);
            }
            catch (Exception oEx) { }
            finally
            {

            }
        }

        public void C_PRCxMoveFile(List<string> paFileMove,string ptStatus)
        {
            try
            {
                if(ptStatus == "Success")
                {
                    if (!Directory.Exists(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success"))
                    {
                        Directory.CreateDirectory(cVB.tVB_PathIN + @"\" + cVB.tVB_MasBackUP + @"\Success");
                    }
                    foreach (string oFileMove in paFileMove)
                    {
                        File.Move(cVB.tVB_PathIN + @"\" + oFileMove, cVB.tVB_PathIN + @"\" + cVB.tVB_MasBackUP + @"\Success\" + oFileMove);
                    }                    
                }
                else
                {
                    if (!Directory.Exists(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Error"))
                    {
                        Directory.CreateDirectory(cVB.tVB_PathIN + @"\" + cVB.tVB_MasBackUP + @"\Error");
                    }
                    foreach (string oFileMove in paFileMove)
                    {
                        File.Move(cVB.tVB_PathIN + @"\" + oFileMove, cVB.tVB_PathIN + @"\" + cVB.tVB_MasBackUP + @"\Error\" + oFileMove);
                    }
                }                
            }
            catch (Exception oEx) { }
            finally
            {

            }
        }

        /// <summary>
        /// Get API Interface (*Arm 63-07-03)
        /// </summary>
        /// <param name="ptApiCode"></param>
        /// <returns></returns>
        public string C_GETxCfgApiKADS(string ptApiCode, string ptBchCode, string ptCmpCode, string ptAgnCode, string ptMerchart, string ptShpCode, string ptPosCode)
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;

            string tUsername = "";
            string tPassword = "";
            string tAth = "";
            string tUrl = "";
            string tResult = "";
            string tApiName = "";

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                odtTmp = new DataTable();

                // GET ข้อมูล Transaction API Type 4
                oSql.AppendLine("SELECT TOP 1 API.FTApiCode, ISNULL(API.FTApiURL,'') AS FTApiURL , ISNULL(API.FTApiLoginUsr,'') AS FTApiLoginUsr, ISNULL(API.FTApiLoginPwd,'') AS FTApiLoginPwd, APIL.FTApiName ");
                oSql.AppendLine("FROM TCNMTxnAPI API WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNMTxnAPI_L APIL WITH(NOLOCK) ON API.FTApiCode = APIL.FTApiCode");
                oSql.AppendLine("WHERE API.FTApiTxnType = 4");
                oSql.AppendLine("AND API.FTApiCode = '" + ptApiCode + "'");

                odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());

                // ตรวจสอบมีข้อมูล  Transaction API Type 4 ?
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    tUsername = odtTmp.Rows[0].Field<string>("FTApiLoginUsr");
                    tPassword = odtTmp.Rows[0].Field<string>("FTApiLoginPwd");
                    tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
                    tApiName = odtTmp.Rows[0].Field<string>("FTApiName");
                }
                else
                {
                    // มีข้อมูล  Transaction API Type 4 ให้ Select จาก Type 3
                    odtTmp = null;
                    odtTmp = new DataTable();
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 API.FTApiCode, ISNULL(API.FTApiURL,'') AS FTApiURL , ISNULL(API.FTApiLoginUsr,'') AS FTApiLoginUsr, ISNULL(API.FTApiLoginPwd,'') AS FTApiLoginPwd, APIL.FTApiName ");
                    oSql.AppendLine("FROM TCNMTxnAPI API WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TCNMTxnAPI_L APIL WITH(NOLOCK) ON API.FTApiCode = APIL.FTApiCode");
                    oSql.AppendLine("WHERE API.FTApiTxnType = 3");
                    oSql.AppendLine("AND API.FTApiCode = '" + ptApiCode + "'");
                    odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        tUsername = odtTmp.Rows[0].Field<string>("FTApiLoginUsr");
                        tPassword = odtTmp.Rows[0].Field<string>("FTApiLoginPwd");
                        tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
                        tApiName = odtTmp.Rows[0].Field<string>("FTApiName");
                    }
                }


                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(tUsername) && !string.IsNullOrEmpty(tPassword))
                    {
                        tAth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(tUsername + ":" + tPassword)));

                    }
                    else
                    {
                        // ถ้าไม่มี User name และ Password 
                        odtTmp = null;
                        odtTmp = new DataTable();
                        // หาข้อมูลจาก TCNMTxnSpcAPI
                        oSql.Clear();
                        oSql.AppendLine("SELECT ISNULL(FTApiURL,'') AS FTApiURL , ISNULL(FTSpaUsrCode,'') AS FTSpaUsrCode, ISNULL(FTSpaUsrPwd,'') AS FTSpaUsrPwd ");
                        oSql.AppendLine("FROM TCNMTxnSpcAPI ");
                        oSql.AppendLine("WHERE ISNULL(FTApiCode, '') = '" + ptApiCode + "' ");
                        oSql.AppendLine("AND(ISNULL(FTCmpCode, '') = '' OR ISNULL(FTCmpCode, '') = '" + ptCmpCode + "') ");
                        oSql.AppendLine("AND(ISNULL(FTAgnCode, '') = '' OR ISNULL(FTAgnCode, '') = '" + ptAgnCode + "') ");
                        oSql.AppendLine("AND(ISNULL(FTBchCode, '') = '' OR ISNULL(FTBchCode, '') = '" + ptBchCode + "') ");
                        oSql.AppendLine("AND(ISNULL(FTMerCode, '') = '' OR ISNULL(FTMerCode, '') = '" + ptMerchart + "') ");
                        oSql.AppendLine("AND(ISNULL(FTShpCode, '') = '' OR ISNULL(FTShpCode, '') = '" + ptShpCode + "') ");
                        oSql.AppendLine("AND(ISNULL(FTPosCode, '') = '' OR ISNULL(FTPosCode, '') = '" + ptPosCode + "') ");

                        odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                        if (odtTmp != null && odtTmp.Rows.Count > 0)
                        {
                            tUsername = odtTmp.Rows[0].Field<string>("FTSpaUsrCode");
                            tPassword = odtTmp.Rows[0].Field<string>("FTSpaUsrPwd");
                            tAth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(tUsername + ":" + tPassword)));
                            tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");

                        }
                    }

                    tResult = tAth + "," + tUrl + "," + tApiName;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cConfig", "C_GETxCfgApiKADS : " + oEx.Message);
            }
            finally
            {
                ptApiCode = "";
                oSql = null;
                oDB = null;
                odtTmp = null;
            }
            return tResult;
        }

    }
}
