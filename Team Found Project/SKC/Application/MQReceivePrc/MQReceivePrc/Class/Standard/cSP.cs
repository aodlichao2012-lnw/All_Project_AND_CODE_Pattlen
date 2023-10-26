using MQReceivePrc.Class;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Webservice.Response;
using MQReceivePrc.Models.Webservice.Response.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class.Standard
{
    public class cSP
    {
        /// <summary>
        /// Clear Memory
        /// </summary>
        public void SP_CLExMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_CLExMemory : " + oEx.Message); }
        }

        /// <summary>
        /// นำตัวเลขมาตัดทศนิยมตามตำหน่งที่กำหนดไว้ใน TSysConfig
        /// </summary>
        /// <param name="ptType">บอกชนิดว่านำไปใช้ทำอะไร (1.Show, 2.Save)</param>
        /// <param name="pcNumber">ตัวเลขที่ต้องการโชว์หรือเซฟ</param>
        /// <param name="pnDigit">ตัวเลขที่ระบุจำนวนทศนิยม</param>
        /// <returns>String ที่ทำการปัดเศษแล้ว</returns>
        public string SP_SETtDecShwSve(int ptType, decimal pcNumber, int pnDigit)
        {
            string tFormat = "";

            try
            {
                switch (ptType)
                {
                    case 1:
                        tFormat = string.Format("{0:N" + pnDigit + "}", pcNumber);
                        break;
                    case 2:
                        tFormat = string.Format("{0:F" + pnDigit + "}", pcNumber);
                        break;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSP", "SP_SETtDecShwSve : " + oEx.Message); }
            finally
            {
                //SP_CLExMemory();
            }

            return tFormat;
        }

        public cmlRabbitMQ SP_GEToMQBranch(string ptBchCode,string ptUrlKey, string ptConnStr, int pnCmdTime)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            cmlRabbitMQ oMQ = new cmlRabbitMQ();
            try
            {
                oSql.AppendLine("SELECT UOL.FTUrlAddress,UOL.FTUolUser,UOL.FTUolPassword,UOL.FTUolVhost,UOJ.FTUrlPort");
                oSql.AppendLine("FROM TCNTUrlObjectLogin UOL WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TCNTUrlObject UOJ WITH(NOLOCK) ON UOJ.FTUrlRefID = UOL.FTUrlRefID AND UOJ.FTUrlAddress = UOL.FTUrlAddress");
                oSql.AppendLine("WHERE UOJ.FNUrlType = '3' AND UOJ.FTUrlTable = 'TCNMBranch' ");
                oSql.AppendLine("AND UOL.FTUolKey = '"+ ptUrlKey +"' AND UOL.FTUolStaActive = '1'");
                oSql.AppendLine("AND UOJ.FTUrlRefID = '" + ptBchCode + "'");
                odtTmp = oDB.C_DAToExecuteQuery(ptConnStr, oSql.ToString(), pnCmdTime);
                if (odtTmp != null)
                {
                    oMQ.tMQHostName = odtTmp.Rows[0].Field<string>("FTUrlAddress");
                    oMQ.tMQUserName = odtTmp.Rows[0].Field<string>("FTUolUser");
                    oMQ.tMQPassword = odtTmp.Rows[0].Field<string>("FTUolPassword");
                    oMQ.tMQVirtualHost = odtTmp.Rows[0].Field<string>("FTUolVhost");
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_GEToMQBranch : " + oEx.Message);
            }
            return oMQ;
        }

        /// <summary>
        /// Get url API
        /// </summary>
        /// <param name="ptConnStr">Connection string</param>
        /// <param name="pnCmdTime">timeout</param>
        /// <param name="ptBchCode">branch code</param>
        /// <param name="pnUrlType">
        /// 1:URL
        /// 2:URL + Authorized
        /// 3:URL + MQ
        /// 4:API2PSMaster
        /// 5:API2PSSale
        /// 6:API2RTMaster
        /// 7:API2RTSale
        /// 8:API2FNWallet
        /// 9:API2CNMember
        /// 10:API2RDFace
        /// 11:API2RDFaceRegis
        /// </param>
        /// <returns></returns>
        public string SP_GETtUrlAPI(string ptConnStr,int pnCmdTime,string ptBchCode, int pnUrlType,ref string ptXKey,ref string ptAPIHeader)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            DataTable odtTmp = new DataTable();
            string tUrlAPI = "";

            try
            {
                oSql.AppendLine("SELECT TOP 1 FTUrlAddress");
                oSql.AppendLine("FROM TCNTUrlObject WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTUrlRefID = '"+ ptBchCode + "' AND FTUrlTable = 'TCNMBranch' AND FNUrlType = " + pnUrlType);
                tUrlAPI = oDB.C_DAToExecuteQuery<string>(ptConnStr,oSql.ToString(),pnCmdTime);

                oSql.Clear();
                oSql.AppendLine("SELECT FTSysStaUsrValue,FTSysStaUsrRef FROM TSysConfig WITH(NOLOCK) WHERE FTSysCode = 'tCN_AgnKeyAPI'");
                odtTmp = oDB.C_DAToExecuteQuery(ptConnStr, oSql.ToString(), pnCmdTime);
                if (odtTmp != null)
                {
                    if (odtTmp.Rows.Count > 0)
                    {
                        ptXKey = odtTmp.Rows[0].Field<string>("FTSysStaUsrValue");
                        ptAPIHeader = odtTmp.Rows[0].Field<string>("FTSysStaUsrRef");
                    }
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_GETtUrlAPI : " + oEx.Message);
            }
            return tUrlAPI;
        }

        /// <summary>
        /// ตรวจสอบว่าเป็นสาขาสำนักงานใหญ่หรือไม่
        /// </summary>
        /// <param name="ptConnStr"></param>
        /// <param name="pnCmdTime"></param>
        /// <returns></returns>
        public bool SP_CHKbIsHQBch(string ptConnStr, int pnCmdTime)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tResult = "";

            try
            {
                oSql.AppendLine("SELECT");
                oSql.AppendLine("	CASE WHEN ISNULL((SELECT TOP 1 FTBchcode FROM TCNMComp WITH(NOLOCK)),'') = ");
                oSql.AppendLine("	ISNULL((SELECT TOP 1 FTBchCode FROM TCNMBranch WITH(NOLOCK) WHERE ISNULL(FTBchStaHQ,'') = '1'),'')");
                oSql.AppendLine("	THEN '1' ELSE '2' END AS FTStaHQ");
                tResult = oDB.C_DAToExecuteQuery<string>(ptConnStr, oSql.ToString(), pnCmdTime);

                if (string.IsNullOrEmpty(tResult))
                {
                    return true;
                }
                else
                {
                    if (tResult == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_CHKbIsHQBch : " + oEx.Message);
                return false;
            }
        }

        /// <summary>
        /// Get branch code HQ.
        /// </summary>
        /// <param name="ptConnStr"></param>
        /// <param name="pnCmdTime"></param>
        /// <returns></returns>
        public string SP_GETtBchHQ(string ptConnStr, int pnCmdTime)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tBchCode = "";
            try
            {
                oSql.AppendLine("SELECT TOP 1 FTBchCode FROM TCNMBranch WITH(NOLOCK) WHERE ISNULL(FTBchStaHQ,'') = '1'");
                tBchCode = oDB.C_DAToExecuteQuery<string>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_CHKbIsHQBch : " + oEx.Message);
            }
            return tBchCode;
        }

        /// <summary>
        /// Get branch code HQ.
        /// </summary>
        /// <param name="ptConnStr"></param>
        /// <param name="pnCmdTime"></param>
        /// <returns></returns>
        public string SP_GETtLocalBch(string ptConnStr, int pnCmdTime)
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            string tBchCode = "";
            try
            {
                oSql.AppendLine("SELECT TOP 1 FTBchcode FROM TCNMComp WITH(NOLOCK)");
                tBchCode = oDB.C_DAToExecuteQuery<string>(ptConnStr, oSql.ToString(), pnCmdTime);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_GETtLocalBch : " + oEx.Message);
            }
            return tBchCode;
        }

        /// <summary>
        /// Check เชื่อมต่อ Internet
        /// </summary>
        /// <returns></returns>
        public bool SP_CHKbConnection(string ptUrl = "https://www.google.com/")
        {
            bool bChkNet = false;

            try
            {
                using (WebClient oClient = new WebClient())
                using (oClient.OpenRead(ptUrl))
                    bChkNet = true;
            }
            catch
            {
                bChkNet = false;
            }

            return bChkNet;
        }

        /// <summary>
        /// Create image from base64
        /// </summary>
        /// <param name="ptBase64"></param>
        /// <returns></returns>
        public string SP_PRCtBase64ImageObj(cmlResInfoImgObj poImgObj)
        {
            string tPathImag = "";
            try
            {
                byte[] abytes = Convert.FromBase64String(poImgObj.rtImgObj);
                tPathImag = Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName).ToString() + @"\AdaImage\Others";
                if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                tPathImag += @"\" + poImgObj.rtImgRefID + poImgObj.rnImgSeq.ToString() + poImgObj.rtImgTable.ToString() + (string.IsNullOrEmpty(poImgObj.rtImgKey) ? "" : "_" + poImgObj.rtImgKey) + ".jpg";
                if (!File.Exists(tPathImag)) File.Delete(tPathImag);
                using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                {
                    oImageFile.Write(abytes, 0, abytes.Length);
                    oImageFile.Flush();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_PRCtBase64ImageObj : " + oEx.Message);
                tPathImag = "";
            }
            return tPathImag;
        }

        /// <summary>
        /// Create image from base64
        /// </summary>
        /// <param name="ptBase64"></param>
        /// <returns></returns>
        public string SP_PRCtBase64ImagePerson(cmlResInfoImgPerson poImgPsn)
        {
            string tPathImag = "";
            try
            {
                byte[] abytes = Convert.FromBase64String(poImgPsn.rtImgObj);
                tPathImag = Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName).ToString() + @"\AdaImage\User";
                if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                tPathImag += @"\" + poImgPsn.rtImgRefID + poImgPsn.rnImgSeq.ToString() + poImgPsn.rtImgTable.ToString() + (string.IsNullOrEmpty(poImgPsn.rtImgKey) ? "" : "_" + poImgPsn.rtImgKey) + ".jpg";
                if (!File.Exists(tPathImag)) File.Delete(tPathImag);
                using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                {
                    oImageFile.Write(abytes, 0, abytes.Length);
                    oImageFile.Flush();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_PRCtBase64ImagePerson : " + oEx.Message);
                tPathImag = "";
            }
            return tPathImag;
        }

        /// <summary>
        /// Create image from base64
        /// </summary>
        /// <param name="ptBase64"></param>
        /// <param name="ptFldName"></param>
        /// <returns></returns>
        public string SP_PRCtBase64Image(string ptBase64, string ptFileName, string ptFldName)
        {
            string tPathImag = "";
            try
            {
                if (string.IsNullOrEmpty(ptBase64)) return tPathImag;

                byte[] abytes = Convert.FromBase64String(ptBase64);
                tPathImag = Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName).ToString() + @"\AdaImage\" + ptFldName;
                if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                tPathImag += @"\" + ptFileName + ".jpg";
                if (!File.Exists(tPathImag)) File.Delete(tPathImag);
                using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                {
                    oImageFile.Write(abytes, 0, abytes.Length);
                    oImageFile.Flush();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_PRCtBase64Image : " + oEx.Message);
                tPathImag = "";
            }
            return tPathImag;
        }

        /// <summary>
        /// Create image from base64
        /// </summary>
        /// <param name="ptBase64"></param>
        /// <returns></returns>
        public string SP_PRCtBase64ToImagePath(cmlResInfoImgPdt poImgPdt)
        {
            string tPathImag = "";
            try
            {
                byte[] abytes = Convert.FromBase64String(poImgPdt.rtImgObj);
                tPathImag = Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName).ToString() + @"\AdaImage\Product";
                if (!Directory.Exists(tPathImag)) Directory.CreateDirectory(tPathImag);
                tPathImag += @"\" + poImgPdt.rtImgRefID + (string.IsNullOrEmpty(poImgPdt.rtImgKey) ? "" : "_" + poImgPdt.rtImgKey) + ".jpg";
                if (!File.Exists(tPathImag)) File.Delete(tPathImag);
                using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                {
                    oImageFile.Write(abytes, 0, abytes.Length);
                    oImageFile.Flush();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_PRCtBase64ToImagePath : " + oEx.Message);
                tPathImag = "";
            }
            return tPathImag;
        }

        /// <summary>
        /// *Arm 63-02-25
        /// สำหรับ Download File ผ่าน URL (แบบ Default Path)
        /// </summary>
        /// <param name="ptUrlFile"></param>
        /// <param name="ptFolderName"></param>
        /// <param name="ptMsgErrDwn"></param>
        /// <returns></returns>
        public string SP_PRCtDownloadFileDefault(string ptUrlFile, string ptFolderName, ref string ptMsgErrDwn)
        {
            string tPathFile = "";
            string tFileName = "";
            string tAppPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            try
            {
                if (string.IsNullOrEmpty(ptUrlFile)) return tPathFile;

                using (WebClient oClient = new WebClient())
                {
                    Uri oUri = new Uri(ptUrlFile);
                    if (oUri.IsAbsoluteUri)
                    {
                        tFileName = Path.GetFileName(oUri.LocalPath);
                        
                        tPathFile = tAppPath + @"\FileReceive\" + ptFolderName;

                        if (!Directory.Exists(tPathFile))
                        {
                            Directory.CreateDirectory(tPathFile);
                        }
                        else
                        {
                            // เช็ค Path มีไฟล์ RedeemPoint.json อยู่ใน Path   
                            if (File.Exists(Path.Combine(tPathFile, tFileName)))
                            {
                                // ถ้ามี มีไฟล์ RedeemPoint.json , ลบทิ้ง
                                File.Delete(Path.Combine(tPathFile, tFileName));
                            }

                        }

                        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                        oClient.DownloadFile(ptUrlFile, tPathFile + @"\" + tFileName);
                        tPathFile = tPathFile + @"\" + tFileName;
                    }
                }
            }
            catch (Exception oEx)
            {
                ptMsgErrDwn = oEx.Message;
                new cLog().C_WRTxLog("cSP", "SP_PRCtDownloadFileDefault : " + oEx.Message);
            }

            return tPathFile;
        }


        /// <summary>
        /// *Arm 63-02-25
        /// สำหรับ Download File ผ่าน URL (แบบกำหนด Path เอง)
        /// </summary>
        /// <param name="ptUrlFile"></param>
        /// <param name="ptPathReceive"></param>
        /// <param name="ptMsgErrDwn"></param>
        /// <returns></returns>
        public string SP_PRCtDownloadFile(string ptUrlFile, string ptPathReceive, ref string ptMsgErrDwn)
        {
            string tPathFile = "";
            string tFileName = "";
            try
            {
                if (string.IsNullOrEmpty(ptUrlFile)) return tPathFile;

                using (WebClient oClient = new WebClient())
                {
                    Uri oUri = new Uri(ptUrlFile);
                    if (oUri.IsAbsoluteUri)
                    {
                        tFileName = Path.GetFileName(oUri.LocalPath);

                        tPathFile = ptPathReceive;

                        if (!Directory.Exists(tPathFile))
                        {
                            Directory.CreateDirectory(tPathFile);
                        }
                        else
                        {
                            // เช็ค Path มีไฟล์ RedeemPoint.json อยู่ใน Path   
                            if (File.Exists(Path.Combine(tPathFile, tFileName)))
                            {
                                // ถ้ามี มีไฟล์ RedeemPoint.json , ลบทิ้ง
                                File.Delete(Path.Combine(tPathFile, tFileName));
                            }

                        }

                        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                        oClient.DownloadFile(ptUrlFile, tPathFile + @"\" + tFileName);
                        tPathFile = tPathFile + @"\" + tFileName;
                    }
                }
            }
            catch (Exception oEx)
            {
                ptMsgErrDwn = oEx.Message;
                new cLog().C_WRTxLog("cSP", "C_PRCtDownloadFile : " + oEx.Message);
            }

            return tPathFile;
        }
    }
}
