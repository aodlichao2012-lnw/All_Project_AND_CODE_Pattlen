using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace API2Wallet.Class.Log
{
    public class cLog
    {
        /// <summary>
        /// Write log to file.
        /// </summary>
        /// <param name="ptTaskName">
        /// Task name ex. 100, 101 (Company code).
        /// </param>
        /// <param name="ptPrcType">
        /// Process type ex. A = Auto, M = Manual.
        ///</param>
        /// <param name="ptFucn">
        /// Function name.
        /// </param>
        /// <param name="ptMsg">
        /// Message log.
        ///</param>
        /// <param name="pbStaEndLine">
        /// Status write end line.
        /// </param>
        /// <remarks></remarks>
        public void C_CALxWriteLog(string ptTaskName, string ptPrcType, string ptFucn, string ptMsg,ref bool pbStaEndLine)
        {
            string tPathLog, tPathFol;
            DateTime dDateNow;
            pbStaEndLine = false;
            try
            {
                // Force date in format english
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");

                // check folder
                tPathFol = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Log\";
                if ((Directory.Exists(tPathFol) == false))
                    Directory.CreateDirectory(tPathFol);

                dDateNow = DateTime.Now;
                tPathLog = tPathFol + ptPrcType + String.Format("{0:yyyyMMdd}", dDateNow) + "_" + ptTaskName + ".txt";
                // create file
                if ((File.Exists(tPathLog) == false))
                {
                    using (StreamWriter oSw = File.CreateText(tPathLog))
                    {
                        oSw.Close();
                    }
                }

                // write log
                using (StreamWriter oStreamWriter = File.AppendText(tPathLog))
                {
                    {
                        if ((string.IsNullOrEmpty(ptFucn) == true))
                            oStreamWriter.WriteLine("[" + String.Format("{0:HH:mm:ss:ffff}", dDateNow) + "] : " + ptMsg);
                        else
                            oStreamWriter.WriteLine("[" + String.Format("{0:HH:mm:ss:ffff}", dDateNow) + "] : " + ptMsg + ", [" + ptFucn + "]");

                        if ((pbStaEndLine == true))
                            oStreamWriter.WriteLine("------------------------------------------------");

                        oStreamWriter.Flush();
                        oStreamWriter.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// Delete file log.
        /// </summary>
        ///<param name="ptTaskName">
        /// Task Name.
        /// </param>
        /// <param name="ptPrcType">
        /// Process type.
        /// </param>
        /// <remarks></remarks>
        public void C_CALxDeleteLog(string ptTaskName, string ptPrcType)
        {
            string tPathLog, tPathFol;
            DateTime dDateNow;
            try
            {
                // check folder
                tPathFol = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Log\";
                if ((Directory.Exists(tPathFol) == false))
                    return;

                dDateNow = DateTime.Now;
                tPathLog = tPathFol + ptPrcType + String.Format("{0:yyyyMMdd}", dDateNow) + "_" + ptTaskName + ".txt";

                // check file
                if ((File.Exists(tPathLog) == false))
                    return;
                else
                    File.Delete(tPathLog);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///  Delete file log.
        /// </summary>
        /// <remarks></remarks>
        public void C_CALxDeleteLog()
        {
            string tPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Log\";
            DirectoryInfo oDirInfoLog = new DirectoryInfo(tPath);
            FileInfo[] aFilesLog;
            string tFileName;
            DateTime dFileNameDate, dCompareDate;

            try
            {
                // *[ANUBIS][][2017-04-05] - ปรับเงื่อนไขการตัดชื้อไฟล์ มาเพื่อลบ ลบไฟล์ย้อนหลัง 30 วัน
                dCompareDate = DateTime.Now.AddDays(-30);
                aFilesLog = oDirInfoLog.GetFiles("*.txt");
                foreach (var oFile in aFilesLog)
                {
                    tFileName = Path.GetFileNameWithoutExtension(oFile.Name); // File name without extension.
                    tFileName = tFileName.Substring(tFileName.Length - 8, 8);    // ตัดเอาทางขวาของชื้อไฟล์มา 8 ตัว

                    dFileNameDate = DateTime.ParseExact(tFileName, "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if ((dFileNameDate.CompareTo(dCompareDate) < 0))
                    {
                        if ((File.Exists(oFile.FullName)))
                            File.Delete(oFile.FullName);
                    }
                }
            }
            catch (Exception ex) {

            } finally {
                aFilesLog = null;
                oDirInfoLog = null;
            }
        }

        public void C_CALxWriteLogClient(string ptClientName, string ptClientIP, string ptFucn, string ptMsg, bool pbStaErr = false, bool pbStaEndLine = false)
        {
            string tPathLog, tPathFol, tFileName;
            DateTime dDateNow;
            string tStaErr = "";
            try
            {
                // Force date in format english
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");

                // check folder
                tPathFol = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Log\";
                if ((Directory.Exists(tPathFol) == false))
                    Directory.CreateDirectory(tPathFol);

                dDateNow = DateTime.Now;
                tFileName = ptClientName + "_" + ptClientIP + "_" + String.Format("{0:yyyyMMdd}", dDateNow) + ".txt";
                tPathLog = tPathFol + tFileName;

                // create file
                if ((File.Exists(tPathLog) == false))
                {
                    using (StreamWriter oSw = File.CreateText(tPathLog))
                    {
                        oSw.Close();
                    }
                }

                if (pbStaErr) { tStaErr = "[Error] "; } else { tStaErr = ""; } 

                // write log
                using (StreamWriter oStreamWriter = File.AppendText(tPathLog))
                {
                    {
                        if ((string.IsNullOrEmpty(ptFucn) == true))
                            oStreamWriter.WriteLine("[" + String.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}", dDateNow) + "] : " + tStaErr + ptMsg);
                        else
                            oStreamWriter.WriteLine("[" + String.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}", dDateNow) + "] : [" + ptFucn + "] " + tStaErr + ptMsg);
                        
                        if ((pbStaEndLine == true))
                            oStreamWriter.WriteLine("-------------------------------------------------------------------------------------------");
                        oStreamWriter.Flush();
                        oStreamWriter.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void C_CALxWriteLogClient(string ptMsg, bool pbStaErr = false, bool pbStaEndLine = false, string ptPosCode = "")
        {
            string tClientName, tClientIP, tFunc;
            string tPathLog, tPathFol, tFileName, tStaErr;
            DateTime dDateNow;

            try
            {
                // Force date in format english
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");

                // check folder
                tPathFol = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\Log\";
                if ((Directory.Exists(tPathFol) == false))
                    Directory.CreateDirectory(tPathFol);

                // *[ANUBIS][][2017-01-24]
                // delete file log > 30 day.
                C_CALxDeleteLog();

                tClientName = HttpContext.Current.Request.UserHostName;
                tClientIP = HttpContext.Current.Request.UserHostAddress;
                tFunc = HttpContext.Current.Request.Path;

                dDateNow = DateTime.Now;
                // Not yet use PosCode Create LogFile '*CH 19-09-2017
                // tFileName = ptPosCode & "_" & tClientName & "_" & tClientIP & "_" & Format(dDateNow, "yyyyMMdd") & ".txt"
                tFileName = tClientName + "_" + tClientIP + "_" + String.Format("{0:yyyyMMdd}", dDateNow) + ".txt";
                tPathLog = tPathFol + tFileName;

                // create file
                if ((File.Exists(tPathLog) == false))
                {
                    using (StreamWriter oSw = File.CreateText(tPathLog))
                    {
                        oSw.Close();
                    }
                }

                if (pbStaErr) { tStaErr = "[Error] "; } else { tStaErr = ""; }

                // write log
                using (StreamWriter oStreamWriter = File.AppendText(tPathLog))
                {
                    {
                        var withBlock = oStreamWriter;
                        if ((string.IsNullOrEmpty(tFunc) == true))
                            withBlock.WriteLine("[" + String.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}", dDateNow) + "] : " + tStaErr + ptMsg);
                        else
                            withBlock.WriteLine("[" + String.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}", dDateNow) + "] : [" + tFunc + "] " + tStaErr + ptMsg);

                        if ((pbStaEndLine == true))
                            withBlock.WriteLine("-------------------------------------------------------------------------------------------");
                       
                        withBlock.Flush();
                        withBlock.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}