using MQAdaLink.Class.Export;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Class
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
            DataTable oDbTbl = new DataTable();
            DataTable oDbTblLogId = new DataTable();
            try
            {
                oDb = new cDatabase();
                oSql = new StringBuilder();

                //oSql.Clear();
                //oSql.AppendLine("SELECT FTApiName FROM TCNMTxnAPI API LEFT JOIN TCNMTxnAPI_L APL");
                //oSql.AppendLine("ON API.FTApiCode=APL.FTApiCode AND APL.FNLngID=1");
                //oSql.AppendLine("WHERE  API.FTApiCode='"+ ptLogTask + "' AND FTApiPrcType='1'");
                //oDbTbl = oDb.C_GEToSQLToDatatable(oSql.ToString());

                // FTLogType 1=นำเข้า  2=ส่งออก
                // FTLogTaskRef ไฟล์ Zip ทั้ง Success และ Error
                // ptStaPrc สถานะงานที่ทำ  1=สำเร็จ  2=ไม่สำเร็จ


                oSql.Clear();
                oSql.AppendLine(" DECLARE @LogId bigint;");
                oSql.AppendLine(" SELECT @LogId = MAX(FNLogID) + 1  FROM TLKTLogHis");
                oSql.AppendLine(" INSERT INTO");
                oSql.AppendLine(" TLKTLogHis");
                oSql.AppendLine(" (");
                oSql.AppendLine(" FDLogCreate, FTLogType, FTLogTask, FTLogTaskRef, FTLogStaPrc, FNLogQtyAll,");
                oSql.AppendLine(" FNLogQtyDone, FTLogStaSend, FDCreateOn, FTCreateBy, FDLastUpdOn, FTLastUpdBy");
                oSql.AppendLine(" )");
                oSql.AppendLine(" VALUES");
                oSql.AppendLine(" (");
                oSql.AppendLine(" GETDATE(), '" + ptLogType + "', '"+ ptLogTask + "', '"+ ptLogTaskRef + "', '"+ ptStaPrc + "', "+ pnQtyAll + ",");
                oSql.AppendLine(" "+ pnQtyDone + ", '"+ ptSendMail + "', GETDATE(), 'Interface', GETDATE(), 'Interface'");               
                oSql.AppendLine(" )");
                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxHistory : SQL/" + oSql.ToString());
                if (oDb.C_GETnSQLExecuteWithConnStr(oSql.ToString(), cVB.tVB_Conn) == 0)
                {
                    return false;
                }
                
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cSP", "C_SAVxHistory : Error/" + oEx.Message.ToString()); //*Arm 63-08-21
                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxHistory : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
                return false;
            }
        }
        public void HandleKeyEvent(Object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = cVB.tVB_PassSFTP;
                }
            }
        }

        private string DeleteFile(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileName);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(cVB.tVB_UserSFTP, cVB.tVB_PassSFTP);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusDescription;
            }
        }
        public bool C_SAVxDownloadXML(List<string> paFileName,string tMode)
        {
            string tDirectory = "";
            //string tpath = "";
            IEnumerable<Renci.SshNet.Sftp.SftpFile> aFile;
            string[] aFilePaths = null;
            string[] aFileSplit = null;
            List<string> aFileName = new List<string>();
            try
            {
#if DEBUG
    //            //string tPath = Directory.GetCurrentDirectory();
    //            //tPath = string.Format(@"{0}\", tPath);
    //            C_CHKxPathFile(cVB.tVB_PathIN);
    //            string uri = "ftp://" + cVB.tVB_HostSFTP + ":" + cVB.nVB_PortSFTP + "/";
    //            try
    //            {
    //                //Create FTP Request.
    //                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
    //                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

    //                //Enter FTP Server credentials.
    //                request.Credentials = new NetworkCredential(cVB.tVB_UserSFTP, cVB.tVB_PassSFTP);
    //                request.UsePassive = true;
    //                request.UseBinary = true;
    //                request.EnableSsl = false;

    //                //Fetch the Response and read it using StreamReader.
    //                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
    //                List<string> entries = new List<string>();
    //                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
    //                {
    //                    //Read the Response as String and split using New Line character.
    //                    entries = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        
    //                    foreach (string entry in entries)
    //                    {
    //                        string[] splits = entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);
    //                        string[] aNameSplip = null;
    //                        FtpWebRequest oRequest =
    //(FtpWebRequest)WebRequest.Create(uri+ splits[8].Trim());
    //                        oRequest.Credentials = new NetworkCredential(cVB.tVB_UserSFTP, cVB.tVB_PassSFTP);
    //                        oRequest.Method = WebRequestMethods.Ftp.DownloadFile;
    //                        switch (tMode)
    //                        {
    //                            case "Master":
    //                                aNameSplip = splits[8].Trim().Split('_');
    //                                if (aNameSplip[0].Trim() == "MASTER")
    //                                {
    //                                    paFileName.Add(splits[8].Trim());
    //                                    FtpWebResponse oRes = (FtpWebResponse)oRequest.GetResponse();
    //                                    Stream responseStream = oRes.GetResponseStream();
    //                                    FileStream file = File.Create(cVB.tVB_PathIN  + splits[8].Trim());
    //                                    byte[] buffer = new byte[32 * 1024];
    //                                    int read;
    //                                    //reader.Read(

    //                                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
    //                                    {
    //                                        file.Write(buffer, 0, read);
    //                                    }
    //                                    System.GC.Collect();
    //                                    System.GC.WaitForPendingFinalizers();
    //                                    DeleteFile(uri + splits[8].Trim());
    //                                }
    //                                break;
    //                            case "Price":
    //                                aNameSplip = splits[8].Trim().Split('_');
    //                                if (aNameSplip[0].Trim() == "PRICE")
    //                                {
    //                                    paFileName.Add(splits[8].Trim());
    //                                    FtpWebResponse oRes = (FtpWebResponse)oRequest.GetResponse();
    //                                    Stream responseStream = oRes.GetResponseStream();
    //                                    FileStream file = File.Create(cVB.tVB_PathIN  + splits[8].Trim());
    //                                    byte[] buffer = new byte[32 * 1024];
    //                                    int read;
    //                                    //reader.Read(

    //                                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
    //                                    {
    //                                        file.Write(buffer, 0, read);
    //                                    }
    //                                    System.GC.Collect();
    //                                    System.GC.WaitForPendingFinalizers();
    //                                    DeleteFile(uri + splits[8].Trim());
    //                                }
    //                                break;
    //                            case "Employee":
    //                                aNameSplip = splits[8].Trim().Split('_');
    //                                if (aNameSplip[0].Trim() == "EMPLO")
    //                                {
    //                                    paFileName.Add(splits[8].Trim());
    //                                    FtpWebResponse oRes = (FtpWebResponse)oRequest.GetResponse();
    //                                    Stream responseStream = oRes.GetResponseStream();
    //                                    FileStream file = File.Create(cVB.tVB_PathIN  + splits[8].Trim());
    //                                    byte[] buffer = new byte[32 * 1024];
    //                                    int read;
    //                                    //reader.Read(

    //                                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
    //                                    {
    //                                        file.Write(buffer, 0, read);
    //                                    }
    //                                    System.GC.Collect();
    //                                    System.GC.WaitForPendingFinalizers();
    //                                    DeleteFile(uri + splits[8].Trim());
    //                                }
    //                                break;
    //                        }
                            
                            
    //                        //using (Stream ftpStream = request.GetResponse().GetResponseStream())
    //                        //using (Stream fileStream = File.Create(cVB.tVB_PathIN + @"\\" + splits[8].Trim()))
    //                        //{

    //                        //}

    //                    }

    //                }
                    
    //                response.Close();
    //            }
    //            catch (Exception oEx)
    //            {
                    
    //            }

                //switch (tMode)
                //    {
                //        case "Master":
                //            aFilePaths = Directory.GetFiles(cVB.tVB_PathIN);
                //            foreach (var oFile in aFilePaths)
                //            {
                //                string[] aNameSplip = null;
                //                aFileSplit = oFile.Split('\\');
                //                aNameSplip = aFileSplit[cVB.nVB_SplitURL].Split('_');
                //                if (aNameSplip[0].Trim() == "MASTER")
                //                {
                //                    paFileName.Add(aFileSplit[cVB.nVB_SplitURL].ToString());
                //                }                                
                //            }
                //        break;
                //        case "Price":
                //            aFilePaths = Directory.GetFiles(cVB.tVB_PathIN);
                //            foreach (var oFile in aFilePaths)
                //            {
                //                string[] aNameSplip = null;
                //                aFileSplit = oFile.Split('\\');
                //                aNameSplip = aFileSplit[cVB.nVB_SplitURL].Split('_');
                //                if (aNameSplip[0].Trim() == "PRICE")
                //                {
                //                    paFileName.Add(aFileSplit[cVB.nVB_SplitURL].ToString());
                //                }
                //            }
                //        break;
                //        case "Employee":
                //            aFilePaths = Directory.GetFiles(cVB.tVB_PathIN);
                //            foreach (var oFile in aFilePaths)
                //            {
                //                string[] aNameSplip = null;
                //                aFileSplit = oFile.Split('\\');
                //                aNameSplip = aFileSplit[cVB.nVB_SplitURL].Split('_');
                //                if (aNameSplip[0].Trim() == "EMPLO")
                //                {
                //                    paFileName.Add(aFileSplit[cVB.nVB_SplitURL].ToString());
                //                }
                //            }
                //        break;
                //        default:
                //            aFilePaths = Directory.GetFiles(cVB.tVB_PathIN);
                //            foreach (var oFile in aFilePaths)
                //            {
                //                string[] aNameSplip = null;
                //                aFileSplit = oFile.Split('\\');
                //                //aNameSplip = aFileSplit[cVB.nVB_SplitURL].Split('_');
                //                paFileName.Add(aFileSplit[cVB.nVB_SplitURL].ToString());
                //            }
                //        break;
                //    }


#else
                //new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : MODE/" + tMode); //*Arm 63-08-20

                //try //*Arm 63-08-20 ครอบ try{}catch(){}
                //{
                //    new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP."); //*Arm 63-08-20
                //    tDirectory = "/";
                //    KeyboardInteractiveAuthenticationMethod oKauth = new KeyboardInteractiveAuthenticationMethod(cVB.tVB_UserSFTP);
                //    PasswordAuthenticationMethod oPauth = new PasswordAuthenticationMethod(cVB.tVB_UserSFTP, cVB.tVB_PassSFTP);

                //    oKauth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                //    var tConnectionInfo = new ConnectionInfo(cVB.tVB_HostSFTP, cVB.nVB_PortSFTP, cVB.tVB_UserSFTP, oPauth, oKauth);

                //    //using (SftpClient oSFTP = new SftpClient(tConnectionInfo))
                //    using (SftpClient oSFTP = new SftpClient(tConnectionInfo))
                //    {
                //        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Connect."); //*Arm 63-08-20
                //        oSFTP.Connect();
                //        aFile = oSFTP.ListDirectory(tDirectory);
                //        foreach (var tFile in aFile)
                //        {
                //            new cLog().C_PRCxLog("C_SAVxDownloadXML Path", tFile.FullName.Trim());
                //            if (tFile.FullName.Length > 10)
                //            {
                //                new cLog().C_PRCxLog("C_SAVxDownloadXML", tFile.FullName.Substring(4));
                //                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Check Path start..."); //*Arm 63-08-20
                //                //Check Path
                //                C_CHKxPathFile(cVB.tVB_PathIN);
                //                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Check Path end..."); //*Arm 63-08-20

                //                //Download File
                //                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Download File " + tFile.FullName + " start..."); //*Arm 63-08-20
                //                using (Stream oFileStream = File.Create(cVB.tVB_PathIN + tFile.FullName.Substring(4)))
                //                {
                //                    oSFTP.BufferSize = 4 * 1024;
                //                    oSFTP.DownloadFile(tFile.FullName, oFileStream);
                //                    paFileName.Add(tFile.FullName.Substring(4));
                //                }
                //                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Download File " + tFile.FullName + " end..."); //*Arm 63-08-20

                //                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Delete File " + tFile.FullName + " start..."); //*Arm 63-08-20
                //                //Delete File
                //                oSFTP.DeleteFile(tFile.FullName);
                //                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Delete File " + tFile.FullName + " end..."); //*Arm 63-08-20
                //            }
                //        }
                //        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Disconnect."); //*Arm 63-08-20
                //        oSFTP.Dispose();
                //        //oSFTP.Disconnect();
                //    }
                //}
                //catch(Exception oEx)
                //{
                //    new cLog().C_PRCxLog("cSP","C_SAVxDownloadXML : SFTP/Error : " + oEx.Message.ToString());
                //}
#endif

                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : MODE/" + tMode); //*Arm 63-08-20

                try //*Arm 63-08-20 ครอบ try{}catch(){}
                {
                    new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP."); //*Arm 63-08-20
                    //tDirectory = "/";

                    //*Arm 63-08-27
                    if (string.IsNullOrEmpty(cVB.tVB_FTPFolder)) 
                    {
                        tDirectory = "/";
                    }
                    else
                    {
                        tDirectory = "/" + cVB.tVB_FTPFolder + "/"; 
                    }
                    //+++++++++++++

                    KeyboardInteractiveAuthenticationMethod oKauth = new KeyboardInteractiveAuthenticationMethod(cVB.tVB_UserSFTP);
                    PasswordAuthenticationMethod oPauth = new PasswordAuthenticationMethod(cVB.tVB_UserSFTP, cVB.tVB_PassSFTP);

                    oKauth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                    var tConnectionInfo = new ConnectionInfo(cVB.tVB_HostSFTP, cVB.nVB_PortSFTP, cVB.tVB_UserSFTP, oPauth, oKauth);

                    //using (SftpClient oSFTP = new SftpClient(tConnectionInfo))
                    using (SftpClient oSFTP = new SftpClient(tConnectionInfo))
                    {
                        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Connect."); //*Arm 63-08-20
                        oSFTP.Connect();
                        aFile = oSFTP.ListDirectory(tDirectory);
                        foreach (var tFile in aFile)
                        {
                            new cLog().C_PRCxLog("C_SAVxDownloadXML Path", tFile.FullName.Trim());
                            if (tFile.FullName.Length > 10)
                            {
                                new cLog().C_PRCxLog("C_SAVxDownloadXML", tFile.FullName.Substring(4));
                                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Check Path start..."); //*Arm 63-08-20
                                //Check Path
                                C_CHKxPathFile(cVB.tVB_PathIN);
                                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Check Path end..."); //*Arm 63-08-20

                                //Download File
                                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Download File " + tFile.FullName + " start..."); //*Arm 63-08-20
                                using (Stream oFileStream = File.Create(cVB.tVB_PathIN + tFile.FullName.Substring(4)))
                                {
                                    oSFTP.BufferSize = 4 * 1024;
                                    oSFTP.DownloadFile(tFile.FullName, oFileStream);
                                    paFileName.Add(tFile.FullName.Substring(4));
                                }
                                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Download File " + tFile.FullName + " end..."); //*Arm 63-08-20

                                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Delete File " + tFile.FullName + " start..."); //*Arm 63-08-20
                                //Delete File
                                oSFTP.DeleteFile(tFile.FullName);
                                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Delete File " + tFile.FullName + " end..."); //*Arm 63-08-20
                            }
                        }
                        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Disconnect."); //*Arm 63-08-20
                        oSFTP.Dispose();
                        //oSFTP.Disconnect();
                    }
                }
                catch (Exception oEx)
                {
                    new cLog().C_PRCxLog("cSP", "C_SAVxDownloadXML : SFTP/Error : " + oEx.Message.ToString());
                    new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/Error : " + oEx.Message.ToString()); //*Arm 63-08-27
                }

                if (paFileName.Count > 0)
                {
                    //มี File
                    new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : Download file success."); //*Arm 63-08-20
                    return true;
                }
                else
                {
                    // กรณีไม่มี file บน FTP ให้หา file จาก Local
                    new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : SFTP/file not found."); //*Arm 63-08-20
                    new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : find locall."); //*Arm 63-08-20

                    if(!Directory.Exists(cVB.tVB_PathIN)) //*Arm 63-08-21
                    {
                        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : Local/CreateDirectory : " + cVB.tVB_PathIN); //*Arm 63-08-21
                        Directory.CreateDirectory(cVB.tVB_PathIN);
                    }

                    string[] files = Directory.GetFiles(cVB.tVB_PathIN, "*.XML");

                    if (files != null && files.Length > 0) //*Arm 63-08-21
                    {
                        //ถ้ามี file

                        string[] aNameSplips = null;
                        for (int i = 0; i < files.Length; i++)
                        {
                            files[i] = Path.GetFileName(files[i]);
                            switch (tMode)
                            {
                                case "Master":
                                    aNameSplips = files[i].Trim().Split('_');
                                    if (aNameSplips[0].Trim() == "MASTER")
                                    {
                                        paFileName.Add(files[i].Trim());
                                        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : Locall/file name : " + files[i].Trim()); //*Arm 63-08-20
                                    }
                                    break;
                                case "Price":
                                    aNameSplips = files[i].Trim().Split('_');
                                    if (aNameSplips[0].Trim() == "PRICE")
                                    {
                                        paFileName.Add(files[i].Trim());
                                        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : Locall/file name : " + files[i].Trim()); //*Arm 63-08-20
                                    }
                                    break;
                                case "Employee":
                                    //aNameSplips = files[8].Trim().Split('_');
                                    aNameSplips = files[i].Trim().Split('_');
                                    if (aNameSplips[0].Trim() == "EMPLO")
                                    {
                                        paFileName.Add(files[i].Trim());
                                        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : Locall/file name : " + files[i].Trim()); //*Arm 63-08-20
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //ถ้าไม่มี file
                        new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : Local/file not found."); //*Arm 63-08-21
                    }
                    new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : Download file success."); //*Arm 63-08-20
                    return true;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cSP", "C_SAVxDownloadXML : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cSP", "C_SAVxDownloadXML : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
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
                    new cLog().C_PRCxLogMonitor("cSP", "C_CHKxPathFile : Created path/" + ptPath); //*Arm 63-08-21
                }
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("cSP","C_CHKxPathFile : Error/" + oEx.Message.ToString()); }
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
                oSql.AppendLine("SELECT TOP 1 (FTXphDocNo) FROM TCNTPdtAdjPriHD ORDER BY RIGHT(FTXphDocNo,6) DESC");
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
                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxBackUPFile : start");
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                string tZipPath = cVB.tVB_PathBackUP  + ptFilename + ".zip";
                if (!Directory.Exists(cVB.tVB_PathBackUP))
                {
                    new cLog().C_PRCxLogMonitor("cSP", "C_PRCxBackUPFile : CreateDirectory" );
                    Directory.CreateDirectory(cVB.tVB_PathBackUP);
                }
                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxBackUPFile : Zip File.");
                ZipFile.CreateFromDirectory(cVB.tVB_PathIN  + ptFilename, tZipPath);

                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxBackUPFile : Delete File.");
                Directory.Delete(cVB.tVB_PathIN  + ptFilename,true);

                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxBackUPFile : end");
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cSP", "C_PRCxBackUPFile : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxBackUPFile : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
            finally
            {

            }
        }

        public void C_PRCxMoveFile(List<string> paFileMove,string ptStatus)
        {
            try
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                foreach (string oFileMove in paFileMove)
                {
                    if (!File.Exists(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\" + oFileMove))
                    {
                        File.Move(cVB.tVB_PathIN + oFileMove, cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\" + oFileMove);
                    }

                }

                //if (ptStatus == "Success")
                //{

                //    foreach (string oFileMove in paFileMove)
                //    {
                //        File.Move(cVB.tVB_PathIN  + oFileMove, cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success\" + oFileMove);
                //    }                    
                //}
                //else
                //{

                //    foreach (string oFileMove in paFileMove)
                //    {
                //        if (!File.Exists(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Error\" + oFileMove))
                //        {
                //            File.Move(cVB.tVB_PathIN + oFileMove, cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Error\" + oFileMove);
                //        }

                //    }
                //}                
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cSP", "C_PRCxMoveFile : Error/" + oEx.Message.ToString());
            }
            finally
            {

            }
        }

        public void C_PRCxDataTableToFile(DataTable poDbTbl, string ptFileName, string ptStatus,string ptPrice = "",string ptName = "")
        {
            try
            {
                string tPath = "";
                string tFileName;
                tFileName = @"\" + ptFileName + ".txt";
                if (ptStatus == "Success")
                {
                    tPath = cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success\" + tFileName;
                }
                else if (ptStatus == "Error")
                {
                    tPath = cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed\" + tFileName;
                }
                if (ptPrice == "PRICE")
                {
                    using (System.IO.StreamWriter oOutputFile = new System.IO.StreamWriter(tPath, true))
                    {

                        oOutputFile.Write(string.Format("{0}\t\t{1}\t\t{2}\t\t{3}\t\t{4}\t\t{5}", "No.", "Date", "Code", "Unit", "Price", "Remark"));
                        oOutputFile.WriteLine();
                        foreach (DataRow row in poDbTbl.Rows)
                        {
                            bool firstCol = true;
                            foreach (DataColumn col in poDbTbl.Columns)
                            {
                                if (!firstCol) oOutputFile.Write("\t\t");
                                oOutputFile.Write(row[col].ToString());
                                firstCol = false;
                            }
                            oOutputFile.WriteLine();
                        }
                        oOutputFile.Dispose();
                    }
                }
                else if (ptPrice == "MASTER" || ptPrice == "EMPLO")
                {
                    using (System.IO.StreamWriter oOutputFile = new System.IO.StreamWriter(tPath, true))
                    {

                        oOutputFile.Write(string.Format("{0}\t\t{1}\t\t{2}\t\t{3}\t\t{4}", "No.", "Date", "Code", "Name/Description", "Remark"));
                        oOutputFile.WriteLine();
                        foreach (DataRow row in poDbTbl.Rows)
                        {
                            bool firstCol = true;
                            foreach (DataColumn col in poDbTbl.Columns)
                            {
                                if (!firstCol) oOutputFile.Write("\t\t");
                                oOutputFile.Write(row[col].ToString());
                                firstCol = false;
                            }
                            oOutputFile.WriteLine();
                        }
                        oOutputFile.Dispose();
                    }
                }
                else if (ptPrice == "FileError")
                {
                    using (System.IO.StreamWriter oOutputFile = new System.IO.StreamWriter(tPath, true))
                    {

                        oOutputFile.Write("ไม่สามารถนำเข้าไฟล์ " + ptName + " ซ้ำ ได้");
                        oOutputFile.WriteLine();
                        oOutputFile.Dispose();
                    }
                }
                else
                {
                    using (System.IO.StreamWriter oOutputFile = new System.IO.StreamWriter(tPath, true))
                    {

                        oOutputFile.Write("ไม่พบไฟล์ที่นำเข้า");
                        oOutputFile.WriteLine();
                        oOutputFile.Dispose();
                    }
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cSP", "C_PRCxDataTableToFile : Error/" + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxDataTableToFile : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
            finally
            {

            }
            
        }
        public void C_PRCxLogFile(string ptFunction)
        {
            string tPath = "";
            string tFileName;
            try
            {

                //#region Check Directory Log

                //tPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //if (!System.IO.Directory.Exists(tPath + @"\xLog"))
                //    System.IO.Directory.CreateDirectory(tPath + @"\xLog");

                //#endregion

                #region Create File Name Log

                tFileName = @"\Readme.txt";
                tPath = cVB.tVB_PathIN + cVB.tVB_MasBackUP + tFileName;


                #endregion

                #region Check File in Log

                if (!System.IO.File.Exists(tPath))
                    System.IO.File.Create(tPath).Dispose();

                #endregion

                #region Write Data To File Log

                using (System.IO.StreamWriter oOutputFile = new System.IO.StreamWriter(tPath, true))
                {
                    oOutputFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " > " + ptFunction);
                    oOutputFile.Dispose();
                }

                #endregion

            }
            catch (Exception oEx) { System.Diagnostics.Debug.WriteLine(oEx.ToString()); }
            finally
            {
                tPath = null;
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
            string tToken = "";
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                odtTmp = new DataTable();

                // GET ข้อมูล Transaction API Type 4
                oSql.AppendLine("SELECT TOP 1 API.FTApiCode, ISNULL(API.FTApiURL,'') AS FTApiURL , ISNULL(API.FTApiLoginUsr,'') AS FTApiLoginUsr, ISNULL(API.FTApiLoginPwd,'') AS FTApiLoginPwd, APIL.FTApiName, FTApiToken");
                oSql.AppendLine("FROM TCNMTxnAPI API WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNMTxnAPI_L APIL WITH(NOLOCK) ON API.FTApiCode = APIL.FTApiCode");
                oSql.AppendLine("WHERE API.FTApiTxnType = 4");
                oSql.AppendLine("AND API.FTApiCode = '" + ptApiCode + "'");

                odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());

                // ตรวจสอบมีข้อมูล  Transaction API Type 4 ?
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    tUsername = odtTmp.Rows[0].Field<string>("FTApiLoginUsr");

                    //*Arm 63-08-09
                    if(!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTApiLoginPwd")))
                    {
                        tPassword = new cEncryptDecrypt("2").C_CALtDecrypt(odtTmp.Rows[0].Field<string>("FTApiLoginPwd"));
                    }
                    //+++++++++++++
                    
                    tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
                    tApiName = odtTmp.Rows[0].Field<string>("FTApiName");
                    tToken = odtTmp.Rows[0].Field<string>("FTApiToken");
                }
                else
                {
                    // มีข้อมูล  Transaction API Type 4 ให้ Select จาก Type 3
                    odtTmp = null;
                    odtTmp = new DataTable();
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 API.FTApiCode, ISNULL(API.FTApiURL,'') AS FTApiURL , ISNULL(API.FTApiLoginUsr,'') AS FTApiLoginUsr, ISNULL(API.FTApiLoginPwd,'') AS FTApiLoginPwd, APIL.FTApiName, FTApiToken ");
                    oSql.AppendLine("FROM TCNMTxnAPI API WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TCNMTxnAPI_L APIL WITH(NOLOCK) ON API.FTApiCode = APIL.FTApiCode");
                    oSql.AppendLine("WHERE API.FTApiTxnType = 3");
                    oSql.AppendLine("AND API.FTApiCode = '" + ptApiCode + "'");
                    odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        tUsername = odtTmp.Rows[0].Field<string>("FTApiLoginUsr");
                        //tPassword = odtTmp.Rows[0].Field<string>("FTApiLoginPwd");
                        //*Arm 63-08-09
                        if (!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTApiLoginPwd")))
                        {
                            tPassword = new cEncryptDecrypt("2").C_CALtDecrypt(odtTmp.Rows[0].Field<string>("FTApiLoginPwd"));
                        }
                        //+++++++++++++
                        tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
                        tApiName = odtTmp.Rows[0].Field<string>("FTApiName");
                        tToken = odtTmp.Rows[0].Field<string>("FTApiToken");
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
                        oSql.AppendLine("SELECT ISNULL(FTApiURL,'') AS FTApiURL , ISNULL(FTSpaUsrCode,'') AS FTSpaUsrCode, ISNULL(FTSpaUsrPwd,'') AS FTSpaUsrPwd, FTSpaApiKey ");
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
                            //tPassword = odtTmp.Rows[0].Field<string>("FTSpaUsrPwd");
                            //*Arm 63-08-09
                            if (!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTSpaUsrPwd")))
                            {
                                tPassword = new cEncryptDecrypt("2").C_CALtDecrypt(odtTmp.Rows[0].Field<string>("FTSpaUsrPwd"));
                            }
                            //+++++++++++++
                            tAth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(tUsername + ":" + tPassword)));
                            tUrl = odtTmp.Rows[0].Field<string>("FTApiURL");
                            tToken = odtTmp.Rows[0].Field<string>("FTSpaApiKey");
                        }
                    }

                    tResult = tAth + "," + tUrl + "," + tApiName+","+ tToken + "," + tUsername + "," + tPassword;
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

        /// <summary>
        /// Get Config Api Interface
        /// </summary>
        public void C_GETxServiceApi()
        {
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;

            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                odtTmp = new DataTable();

                for (int n = 4; n <= 5; n++)
                {
                    string tApiCode = "";
                    tApiCode = "0000" + n.ToString();

                    odtTmp = new DataTable();

                    // GET ข้อมูล Transaction APISPC
                    oSql.Clear();
                    oSql.AppendLine("SELECT API.FTBchCode AS tBchCode, API.FTAgnCode AS tAgnCode, API.FTCmpCode AS tCmpCode,");
                    oSql.AppendLine("ISNULL(API.FTApiURL, '') AS tApiURL, ISNULL(API.FTSpaUsrCode, '') AS tApiUser, ISNULL(API.FTSpaUsrPwd, '') AS tApiPwd,");
                    oSql.AppendLine("API.FTSpaApiKey AS tApiToken, APIL.FTApiName AS tApiName");
                    oSql.AppendLine("FROM TCNMTxnSpcAPI API WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TCNMTxnAPI_L APIL WITH(NOLOCK) ON API.FTApiCode = APIL.FTApiCode");
                    oSql.AppendLine("WHERE ISNULL(API.FTApiCode, '') = '" + tApiCode + "' AND API.FTApiFmtCode = '00001'");
                    odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        // GET ข้อมูล Transaction API Type 4
                        odtTmp = null;
                        oSql.Clear();
                        oSql.AppendLine("SELECT TOP 1 '' AS tBchCode, '' AS tAgnCode, '' AS tCmpCode,");
                        oSql.AppendLine("ISNULL(API.FTApiURL, '') AS tApiURL, ISNULL(API.FTApiLoginUsr, '') AS tApiUser, ISNULL(API.FTApiLoginPwd, '') AS tApiPwd,");
                        oSql.AppendLine("FTApiToken AS tApiToken, APIL.FTApiName AS tApiName");
                        oSql.AppendLine("FROM TCNMTxnAPI API WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMTxnAPI_L APIL WITH(NOLOCK) ON API.FTApiCode = APIL.FTApiCode");
                        oSql.AppendLine("WHERE API.FTApiCode = '" + tApiCode + "' AND API.FTApiTxnType = '4'");
                        odtTmp = new DataTable();
                        odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                        if(odtTmp != null && odtTmp.Rows.Count > 0)
                        {
                            if(!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("tApiURL")))
                            {
                                if (string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("tApiUser")) && string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("tApiPwd")))
                                {
                                    odtTmp = null;
                                }
                            }
                            else
                            {
                                odtTmp = null;
                            }
                        }
                        else
                        {
                            odtTmp = null;
                        }
                    }

                    if (tApiCode == "00004")
                    {
                        cVB.oVB_UrlGetToken = odtTmp;
                    }

                    if (tApiCode == "00005")
                    {
                        cVB.oVB_UrlExport = odtTmp;
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cConfig", "C_GETxServiceApi : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                odtTmp = null;
            }
        }

        public string C_GETxCfgApiKADS2(string ptApiCode)
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
            string tToken = "";
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                odtTmp = new DataTable();

                // GET ข้อมูล Transaction API Type 4
                oSql.AppendLine("select UrlObj.FTUrlAddress,UrlLogin.FTUolUser,UrlLogin.FTUolPassword,UrlObj.FTUrlKey from  TCNTUrlObject UrlObj");
                oSql.AppendLine("INNER JOIN TCNTUrlObjectLogin UrlLogin ON UrlObj.FTUrlRefID = UrlLogin.FTUrlRefID");
                oSql.AppendLine("WHERE UrlObj.FTUrlRefID = 'CENTER' and UrlObj.FNUrlType = 2 and UrlLogin.FTUolVhost = '"+ ptApiCode + "'");
              

                odtTmp = oDB.C_GEToSQLToDatatable(oSql.ToString());
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    tUsername = odtTmp.Rows[0].Field<string>("UrlLogin");
                    //tPassword = odtTmp.Rows[0].Field<string>("FTSpaUsrPwd");
                    //*Arm 63-08-09
                    if (!string.IsNullOrEmpty(odtTmp.Rows[0].Field<string>("FTUolPassword")))
                    {
                        tPassword = odtTmp.Rows[0].Field<string>("FTUolPassword");
                    }
                    //+++++++++++++
                    tAth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(tUsername + ":" + tPassword)));
                    tUrl = odtTmp.Rows[0].Field<string>("FTUrlAddress");
                    tToken = odtTmp.Rows[0].Field<string>("FTUrlKey");
                }
                tResult = tAth + "," + tUrl + "," + tApiName + "," + tToken + "," + tUsername + "," + tPassword;

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

        public void C_PRCxInsertHistory(string tFilename)
        {
            StringBuilder oSql;
            cDatabase oDB;
            try
            {
                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxInsertHistory : insert TLKTHistory start."); //*Arm 63-08-21
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.Clear();
                oSql.AppendLine("insert into TLKTHistory(FTHisName, FDHisDate, FDHisTime, FTStaDone, FTHisDesc)");
                oSql.AppendLine("VALUES('"+ tFilename + "', GETDATE(), CONVERT(TIME, GETDATE()), '1', '')");

                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxInsertHistory : insert TLKTHistory/Sql/ " + oSql.ToString()); //*Arm 63-08-21
                oDB.C_GEToSQLToDatatable(oSql.ToString());
                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxInsertHistory : insert TLKTHistory end."); //*Arm 63-08-21
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLogMonitor("cSP", "C_PRCxInsertHistory : Error/" + oEx.Message); //*Arm 63-08-21
            }
            finally
            {
                oSql = null;
                oDB = null;
            }
            //insert into TLKTHistory(FTHisName, FDHisDate, FDHisTime, FTStaDone, FTHisDesc)
            //    VALUES('', GETDATE(), CONVERT(TIME, GETDATE()), '1', '')
        }

        public bool C_CHKbHistory(string tFilename)
        {
            bool bStatus = false;
            StringBuilder oSql;
            cDatabase oDB;
            DataTable oDbTbl = new DataTable();
            try
            {
                new cLog().C_PRCxLogMonitor("cSP", "C_CHKbHistory : check history file " + tFilename + " start."); //*Arm 63-08-21
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.Clear();
                oSql.AppendLine("SELECT * FROM TLKTHistory ");
                oSql.AppendLine("WHERE FTHisName = '"+ tFilename + "'");

                new cLog().C_PRCxLogMonitor("cSP", "C_CHKbHistory : check history SQL/"+ oSql.ToString()); //*Arm 63-08-21
                oDbTbl = oDB.C_GEToSQLToDatatable(oSql.ToString());

                if (oDbTbl.Rows.Count > 0)
                {
                    //มีข้อมูลใน History
                    bStatus = false;
                    new cLog().C_PRCxLogMonitor("cSP", "C_CHKbHistory : check history result = false"); //*Arm 63-08-21
                }
                else
                {
                    //ไม่มีข้อมูลใน History
                    bStatus = true;
                    new cLog().C_PRCxLogMonitor("cSP", "C_CHKbHistory : check history result = true"); //*Arm 63-08-21
                }
                new cLog().C_PRCxLogMonitor("cSP", "C_CHKbHistory : check history file " + tFilename + " end."); //*Arm 63-08-21
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cSP", "C_CHKbHistory : Error/"+ oEx.Message); //*Arm 63-08-21
                new cLog().C_PRCxLogMonitor("cSP", "C_CHKbHistory : Error/" + oEx.Message); //*Arm 63-08-27
            }
            return bStatus;
        }


        /// <summary>
        /// ฟังก์ชั่นตัดคำ ตามจำนวนตำแหน่งอักษร
        /// </summary>
        /// <param name="ptPdtName"></param>
        /// <param name="pnPdtWidth">จำนวนที่จะตัดให้เหลือ</param>
        /// <returns></returns>
        public static string SP_SUBtSubString(string ptPdtName, int pnPdtWidth)
        {
            int nCountSara;
            string tResult = "";
            try
            {
                nCountSara = SP_PRCnThaAbsoluteCount(ptPdtName);

                if (ptPdtName.Length < (pnPdtWidth + nCountSara))
                {
                    for (int ni = ptPdtName.Length; ni <= (pnPdtWidth + nCountSara); ni++)
                    {
                        ptPdtName += " ";
                    }
                }

                ptPdtName = ptPdtName.Substring(0, (pnPdtWidth + nCountSara));

                nCountSara = SP_PRCnThaAbsoluteCount(ptPdtName);
                while (ptPdtName.Length > (pnPdtWidth + nCountSara))
                {
                    ptPdtName = ptPdtName.Substring(0, ptPdtName.Length - 1);
                    nCountSara = SP_PRCnThaAbsoluteCount(ptPdtName);
                }

                tResult = ptPdtName;
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cConfig", "SP_SUBtSubString : " + oEx.Message);
            }
            return tResult;
        }


        /// <summary>
        /// ฟังก์ชั้นนับสระ
        /// </summary>
        /// <param name="ptPrint"></param>
        /// <returns></returns>
        public static int SP_PRCnThaAbsoluteCount(string ptPrint)
        {
            int ni, nByte = 0, nBytep = 0, nP = 0;
            int nT1 = 0, nT2 = 0, nT3 = 0;
            bool bJust = false;
            bool bTFill = false;
            bool bBFill = false;
            string tText = ptPrint;
            string tT1 = "", tT2 = "", tT3 = "", tBlank = "";
            int nResult = 0;

            try
            {
                // เช็คก่อนว่าต้องตัดคำหรือไม่
                for (ni = 0; ni < tText.Length; ni++)
                {
                    var tSub = tText.Substring(ni, 1);
                    byte[] abyte = Encoding.Default.GetBytes(tSub);
                    foreach (var b in abyte)
                    {
                        nByte = Convert.ToInt32(b);
                    }

                    switch (nByte)
                    {
                        case 209:   // ั
                        case 212:   // ิ
                        case 213:   // ี
                        case 214:   // ึ
                        case 215:   // ื
                        case 216:   // ุ
                        case 217:   // ู
                        case 218:   // .
                        case 231:   // ็
                        case 232:   // ่
                        case 233:   // ้
                        case 234:   // ๊
                        case 235:   // ๋
                        case 236:   // ์
                        case 237:   // ํ
                            bJust = true;
                            break;
                        default:
                            break;
                    }

                    if (bJust)
                    {
                        break;
                    }
                }

                //ต้องตัดคำ
                if (bJust)
                {
                    for (ni = 0; ni < tText.Length; ni++)
                    {
                        var tSub = tText.Substring(ni, 1);
                        byte[] abyte = Encoding.Default.GetBytes(tSub);
                        foreach (var b in abyte)
                        {
                            nByte = Convert.ToInt32(b);
                        }
                        switch (nByte)
                        {
                            case 209:   // ั
                            case 212:   // ิ
                            case 213:   // ี
                            case 214:   // ึ
                            case 215:   // ื
                                tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                nT1 = nT1 + 1;
                                bTFill = true;
                                nP = 1;
                                break;

                            case 216:   // ุ
                            case 217:   // ู
                                tT3 = tT3 + Encoding.Default.GetString(abyte).ToString();
                                nT3 = nT3 + 1;
                                bTFill = true;
                                nP = 5;
                                break;

                            case 231:   // ็
                                tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                nT1 = nT1 + 1;
                                bTFill = true;
                                nP = 2;
                                break;

                            case 236:   // ์
                                if (nP == 1 && nBytep == 212)
                                {
                                    // ถ้าก่อนหน้านี้เป็นสระอิ ต้องลบสระอิก่อนหน้านี้ก่อน แล้วเปลี่ยนเป็น  ->  ิ์
                                    tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                    nT1 = nT1 + 1; //*Arm 63-08-05
                                }
                                else
                                {
                                    tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                    nT1 = nT1 + 1;
                                    bTFill = true;
                                    nP = 3;
                                }
                                break;

                            case 232:   // ่
                            case 233:   // ้
                            case 234:   // ๊
                            case 235:   // ๋
                                if (nP == 1)
                                {
                                    // ถ้าก่อนหน้านี้เป็น ชุดที่ 1 ต้องลบตัวก่อนหน้านี้ก่อนเปลี่ยนให้เป็น -> สระติดกัน แยกตาม case ตัวเดิม
                                    tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                    nT1 = nT1 + 1; //*Arm 63-08-05
                                }
                                else
                                {
                                    tT1 = tT1 + Encoding.Default.GetString(abyte).ToString();
                                    nT1 = nT1 + 1;
                                    bTFill = true;
                                    nP = 4;
                                }
                                break;

                            default:
                                tT2 = tT2 + Encoding.Default.GetString(abyte).ToString();
                                switch (nP)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                    case 4:
                                    case 5:
                                        if (bTFill == false)
                                        {
                                            tT1 = tT1 + tBlank;
                                        }
                                        if ((bBFill == false))
                                        {
                                            tT3 = tT3 + tBlank;
                                        }
                                        break;

                                    case 6:
                                        tT1 = tT1 + tBlank;
                                        tT3 = tT3 + tBlank;
                                        break;
                                }
                                bBFill = false;
                                bTFill = false;
                                nP = 6;
                                break;
                        }

                        nBytep = nByte; //เก็บค่าเก่า
                    }
                    nResult = nT1 + nT3;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cConfig", "SP_PRCnThaAbsoluteCount : " + oEx.Message);
            }
            return nResult;
        }


    }
}
