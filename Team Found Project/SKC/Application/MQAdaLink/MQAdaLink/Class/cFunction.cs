using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Class
{
    
    class cFunction
    {
        /// <summary>
        /// Function Public Message MQ (*Arm 63-07-03)
        /// </summary>
        /// <param name="ptQueueName"> Queue Name</param>
        /// <param name="durable"> durable: true/false </param>
        /// <param name="ptMessage">Message</param>
        /// <param name="ptErrMsg"></param>
        /// <param name="ptTask"></param>
        public static void C_PRCxMQPublish(string ptQueueName, bool durable, string ptMessage, out string ptErrMsg, string ptTask ="")
        {
            ConnectionFactory oFactory;
            cFunction oFunc = new cFunction();
            string tQueueName = ptQueueName;
            try
            {
                oFactory = new ConnectionFactory();
                oFactory.HostName = cVB.oVB_RabbitMQ.tMQHostName;
                oFactory.UserName = cVB.oVB_RabbitMQ.tMQUserName;
                oFactory.Password = cVB.oVB_RabbitMQ.tMQPassword;
                oFactory.VirtualHost = cVB.oVB_RabbitMQ.tMQVirtualHost;
                using (var oConn = oFactory.CreateConnection())
                {
                    using (var oChannel = oConn.CreateModel())
                    {
                        var body = Encoding.UTF8.GetBytes(ptMessage);
                        oChannel.QueueDeclare(tQueueName, false, false, false, null);
                        oChannel.BasicPublish("", tQueueName, false, null, body);
                        ptErrMsg = "";
                    }
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_PRCxLog("cFunction","C_PRCxMQPublish : Error/" + ptErrMsg);
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxMQPublish : Error/" + ptErrMsg, ptTask); //*Arm 63-08-27
            }
            finally
            {
                oFactory = null;
            }
        }

        public static void C_PRCxMQPublishExchange(string ptExchange, string ptRoute, string ptExchangeMode, string ptMessage, out string ptErrMsg, string ptTask ="")
        {
            ConnectionFactory oFactory;
            cFunction oFunc = new cFunction();
            string tExchangeName = ptExchange;
            string tRoute = ptRoute;
            string tExchangeMode = ptExchangeMode;
            try
            {
                oFactory = new ConnectionFactory();
                oFactory.HostName = "";
                oFactory.UserName = "";
                oFactory.Password = "";
                oFactory.VirtualHost = "";
                using (var oConn = oFactory.CreateConnection())
                {
                    using (var oChannel = oConn.CreateModel())
                    {
                        var body = Encoding.UTF8.GetBytes(ptMessage);
                        oChannel.ExchangeDeclare(tExchangeName, tExchangeMode, false, false, null);
                        oChannel.BasicPublish(tExchangeName, ptRoute, false, null, body);
                        ptErrMsg = "";
                    }
                }
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                new cLog().C_PRCxLog("cFunction", "C_PRCxMQPublishExchange : Error/" + ptErrMsg);
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxMQPublishExchange : Error/" + ptErrMsg, ptTask); //*Arm 63-08-27
            }
            finally
            {
                oFactory = null;
            }
        }


        /// <summary>
        /// Function Send Mail
        /// </summary>
        /// <param name="ptMailSender">Mail Sender</param>
        /// <param name="ptMailReceive">Mail Receive</param>
        /// <param name="ptCC">Mail CC</param>
        /// <param name="ptBCC">Mail BCC</param>
        /// <param name="ptSubj">Subject</param>
        /// <param name="ptBody">Content</param>
        /// <param name="ptPassword">Password Mail Sender </param>
        /// <param name="ptSmtp"></param>
        /// <param name="pnPort"></param>
        public static bool C_PRCbFuncSendMail(string ptMailSender, string ptMailReceive, string ptCC, string ptBCC, string ptSubj, string ptBody,string ptPassword, string ptSmtp, int pnPort, string ptTask = "")
        {
            MailMessage oMail;
            SmtpClient oSmtpServer;
            try
            {
                //log monitor
                new cLog().C_PRCxLogMonitor("cFunction","C_PRCbFuncSendMail : Function Send Email Start...", ptTask);
                oMail = new MailMessage();
                oSmtpServer = new SmtpClient(ptSmtp);

                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Add Sender : " + ptMailSender, ptTask);
                oMail.From = new MailAddress(ptMailSender);

                // To.
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Add To : " + ptMailReceive, ptTask);
                if (!string.IsNullOrEmpty(ptMailReceive))
                {
                    string[] aTo = ptMailReceive.Split(',');
                    foreach (string tTo in aTo)
                    {
                        oMail.To.Add(tTo);
                    }
                }

                // CC.
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Add Cc. : " + ptCC, ptTask);
                if (!string.IsNullOrEmpty(ptCC))
                {
                    string[] aCC = ptCC.Split(',');
                    foreach(string tCC in aCC)
                    {
                        oMail.CC.Add(tCC);
                    }
                }

                // BCC.
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Add Bcc. : " + ptBCC, ptTask);
                if (!string.IsNullOrEmpty(ptBCC)) //Mail BCC
                {
                    string[] aBCC = ptBCC.Split(',');
                    foreach (string tBCC in aBCC)
                    {
                        oMail.Bcc.Add(tBCC);
                    }
                }

                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Add Subjedt. : " + ptSubj, ptTask);
                oMail.Subject = ptSubj; // Subject
                oMail.IsBodyHtml = true;
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Add Content. : " + ptBody, ptTask);
                oMail.Body = ptBody; //Content


                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : SmtpClient smtp : " + ptSmtp, ptTask);
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : SmtpClient Port. : " + pnPort.ToString(), ptTask);
                oSmtpServer.Port = pnPort;
                //new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : SmtpClient User/password. : " + ptMailSender + "/ "+ ptPassword);
                oSmtpServer.Credentials = new System.Net.NetworkCredential(ptMailSender, ptPassword); // "username", "password"
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : SmtpClient Enable SSL = true", ptTask);
                oSmtpServer.EnableSsl = true;
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Sending...", ptTask);
                oSmtpServer.Send(oMail);
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Send Success", ptTask);
                return true;
            }
            catch(Exception oEx)
            {
                oEx.Message.ToString();
                new cLog().C_PRCxLog("cFunction", "C_PRCbFuncSendMail : Error / " + oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCbFuncSendMail : Error / " + oEx.Message.ToString(), ptTask); //*Arm 63-08-27
                return false;
            }
            finally
            {
                oMail = null;
                oSmtpServer = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process Export
        /// </summary>
        /// <param name="pnDocType"></param>
        /// <param name="ptMessage"></param>
        /// <param name="ptError"></param>
        /// <returns></returns>
        public static string C_PRCxExport(int pnDocType, string ptMessage, out string ptError, string ptSO = "")
        {
            CookieContainer oCookieJar;
            string tResult = "";
            string tResToken;
            string tTask = "Vender";
            try
            {
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : Process (Api) export to KADS start.", tTask);
                ptError = "";
                // Setup network credentials object to be used for requests to Gateway server
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(cVB.tVB_ApiToken_UserName, cVB.tVB_ApiToken_Password);

                // Create Gateway objects
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : request api get token/url : " + cVB.tVB_ApiToken_Url, tTask); //*Arm 63-08-10
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : equest api get token/user : " + cVB.tVB_ApiToken_UserName, tTask); //*Arm 63-08-10
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : request api get token/password : " + cVB.tVB_ApiToken_Password, tTask); //*Arm 63-08-10
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : request api get token/X-CSRF-Token : " + cVB.tVB_ApiToken_Token, tTask);

                HttpWebRequest oReq = (HttpWebRequest)HttpWebRequest.Create(cVB.tVB_ApiToken_Url);
                HttpWebResponse oResp;
                // Add custom header request to fetch the CSRF token
                oReq.Credentials = credentials;
                oReq.Method = "GET";
                if (!string.IsNullOrEmpty(cVB.tVB_ApiToken_Token))
                {
                    oReq.Headers.Add("X-CSRF-Token", cVB.tVB_ApiToken_Token);
                }
                else
                {
                    ptError = "";
                    return tResult;
                }

                // Setup cookie jar to capture cookies coming back from Gateway server. These cookies are needed along with the CSRF token for modifying requests.
                oCookieJar = new CookieContainer();
                oReq.CookieContainer = oCookieJar;
                try
                {
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : Call api get token start.", tTask); //*Arm 63-08-10
                    oResp = (HttpWebResponse)oReq.GetResponse();
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : Call api get token end.", tTask); //*Arm 63-08-10
                }
                catch (System.Net.WebException oEx)
                {
                    // Add your error handling here
                    new cLog().C_PRCxLog("cFunction", "C_PRCxExport : Call api get token error : " + oEx.Message.ToString()); //*Arm 63-08-10
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : Call api get token error : " + oEx.Message.ToString(), tTask); //*Arm 63-08-27
                    ptError = oEx.Message.ToString();
                    return tResult;
                }
                catch (Exception oEx)
                {
                    // Add your error handling here
                    new cLog().C_PRCxLog("cFunction", "C_PRCxExport : Call api get token error : " + oEx.Message.ToString()); //*Arm 63-08-10
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : Call api get token error : " + oEx.Message.ToString(), tTask); //*Arm 63-08-27
                    ptError = oEx.Message.ToString();
                    return tResult;
                }

                // Assign values from response to class variables.
                tResToken = oResp.Headers.Get("X-CSRF-Token");
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : Response token for send Api6 : " + tResToken, tTask); //*Arm 63-08-10

                if (!string.IsNullOrEmpty(tResToken))
                {
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : C_PRCtExportApi6 start.", tTask); //*Arm 63-08-10
                    tResult = C_PRCtExportApi6(pnDocType, oCookieJar, tResToken, ptMessage, out ptError, ptSO); //*Arm 63-08-18 เพิ่ม DocType
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : C_PRCtExportApi6 end.", tTask); //*Arm 63-08-10

                    if (ptError != "")
                    {
                        //*Arm 63-08-21
                        if (pnDocType == 9)
                        {
                            if(ptError == "E")
                            {
                                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : C_PRCtExportApi6 Respont Error/" + ptError +":"+ tResult, tTask); //*Arm 63-08-10
                                return tResult;
                            }
                            else
                            {
                                return tResult;
                            }
                        }
                        else
                        {
                            new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : C_PRCtExportApi6 process fail/" + ptError, tTask); //*Arm 63-08-10
                            return tResult;
                        }
                    }
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : token for send Api6 is empty.", tTask); //*Arm 63-08-10
                }
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : Process (Api) export to KADS end.", tTask); //*Arm 63-08-10
            }
            catch(Exception oEx)
            {
                ptError = oEx.Message.ToString();
                new cLog().C_PRCxLog("cFunction", "C_PRCxExport : Error/ " + ptError); //*Arm 63-08-10
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCxExport : Error/ " + ptError, tTask); //*Arm 63-08-27
                return tResult;
            }
            ptError = "";
            return tResult;

        }

        /// <summary>
        /// call api 6
        /// </summary>
        /// <param name="pnDocType"></param>
        /// <param name="poCookieJar"></param>
        /// <param name="ptToken"></param>
        /// <param name="ptBody"></param>
        /// <param name="ptError"></param>
        /// <returns></returns>
        public static string C_PRCtExportApi6(int pnDocType, CookieContainer poCookieJar, string ptToken, string ptBody, out string ptError, string ptSO = "")
        {
            string tMessage = "";
            string tUrl = "";
            ptError = "";
            HttpWebRequest oReq;
            string tTask = "Vender";

            try
            {
                if (pnDocType == 9) //*Arm 63-08-21
                {
                    tUrl = cVB.tVB_ApiExport_Url + "('" + ptSO + "')";
                }
                else
                {
                    tUrl = cVB.tVB_ApiExport_Url;
                }
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : request Api6(Export)/url : " + tUrl, tTask);            //*Arm 63-08-10
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : request Api6(Export)/User : " + cVB.tVB_ApiExport_UserName, tTask);      //*Arm 63-08-10
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : request Api6(Export)/Password : " + cVB.tVB_ApiExport_Password, tTask);   //*Arm 63-08-10
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : equest Api6(Export)/X-CSRF-Token : " + ptToken, tTask);                 //*Arm 63-08-10

                // Setup network credentials object to be used for requests to Gateway server
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(cVB.tVB_ApiExport_UserName, cVB.tVB_ApiExport_Password);

                // Create Gateway objects
                //oReq = (HttpWebRequest)HttpWebRequest.Create(cVB.tVB_ApiExport_Url);
                oReq = (HttpWebRequest)HttpWebRequest.Create(tUrl); //*Arm 63-08-21

                oReq.Credentials = credentials;

                //*Arm 63-08-18 Check Type
                if (pnDocType == 9)
                {
                    //คืน
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : equest Api6(Export)/Method : DELETE", tTask); 
                    oReq.Method = "DELETE";
                }
                else
                {
                    //ขาย
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : equest Api6(Export)/Method : POST", tTask);
                    oReq.Method = "POST";
                }
                //++++++++++++++

                oReq.ContentType = "application/json";
                oReq.Headers.Add("X-CSRF-Token", ptToken);
                oReq.Accept = "application/json";

                string tCookie = "";
                CookieCollection oCookies;
                Uri oUri = new Uri(cVB.tVB_ApiToken_Url);

                oCookies = poCookieJar.GetCookies(oUri);
                foreach (Cookie cookie in oCookies)
                {
                    tCookie = tCookie + ";" + cookie.ToString();
                }
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : request api get token/Cookie : " + tCookie.Substring(1), tTask);  //*Arm 63-08-10
                oReq.Headers.Add("Cookie", tCookie.Substring(1));

                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : Call Api6(Export) start.", tTask); //*Arm 63-08-10
                if (pnDocType == 9) //*Arm 63-08-21
                {
                    //คืน
                }
                else
                {
                    //ขาย
                    var oData = Encoding.UTF8.GetBytes(ptBody);
                    using (var oStream = oReq.GetRequestStream())
                    {
                        oStream.Write(oData, 0, oData.Length);
                    }
                }
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : Call Api6(Export) end.", tTask); //*Arm 63-08-10

                try
                {
                    using (HttpWebResponse oResp = (HttpWebResponse)oReq.GetResponse())
                    {
                        using (StreamReader oRd = new StreamReader(oResp.GetResponseStream()))
                        {
                            if (pnDocType == 9)
                            {
                                try
                                {
                                    string tRetType = "";
                                    tRetType = oResp.Headers.Get("ret-type");
                                    ptError = tRetType;
                                    tMessage = oResp.Headers.Get("ret-message");
                                    
                                }
                                catch (Exception oEx)
                                {
                                    ptError = "";
                                    tMessage = "";
                                }
                            }
                            else
                            {
                                tMessage = oRd.ReadToEnd();
                            }
                        }
                    }
                }
                catch (System.Net.WebException oEx)
                {
                    // Add your error handling here
                    new cLog().C_PRCxLog("cFunction", "C_PRCtExportApi6 : Call Api6(Export) error/ " + oEx.Message.ToString()); //*Arm 63-08-10
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : Call Api6(Export) error/ " + oEx.Message.ToString(), tTask); //*Arm 63-08-27
                    ptError = oEx.Message.ToString();
                    return tMessage;
                }
                catch (Exception oEx)
                {
                    new cLog().C_PRCxLog("cFunction", "C_PRCtExportApi6 : Call Api6(Export) error/" + oEx.Message.ToString()); //*Arm 63-08-10
                    new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : Call Api6(Export) error/ " + oEx.Message.ToString(), tTask); //*Arm 63-08-27
                    ptError = oEx.Message.ToString();
                    return tMessage;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cFunction", "C_PRCtExportApi6 : Error/ " + oEx.Message.ToString()); //*Arm 63-08-10
                new cLog().C_PRCxLogMonitor("cFunction", "C_PRCtExportApi6 : Error/ " + oEx.Message.ToString(), tTask); //*Arm 63-08-27
                ptError = oEx.Message.ToString();
                return tMessage;
            }
            finally
            {

            }
            //ptError = "";
            return tMessage;
        }
    }
}
