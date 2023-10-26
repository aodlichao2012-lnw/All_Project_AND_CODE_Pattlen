using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class
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
        public static void C_PRCxMQPublish(string ptQueueName, bool durable, string ptMessage, out string ptErrMsg)
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
                new cLog().C_PRCxLog("C_PRCxMQPublish", ptErrMsg);
            }
            finally
            {
                oFactory = null;
            }
        }

        public static void C_PRCxMQPublishExchange(string ptExchange, string ptRoute, string ptExchangeMode, string ptMessage, out string ptErrMsg)
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
                new cLog().C_PRCxLog("C_PRCxMQPublishExchange", ptErrMsg);
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
        public static bool C_PRCbFuncSendMail(string ptMailSender, string ptMailReceive, string ptCC, string ptBCC, string ptSubj, string ptBody,string ptPassword, string ptSmtp, int pnPort)
        {
            
            try
            {
                //log monitor
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Function Send Email Start...");

                MailMessage oMail = new MailMessage();

                SmtpClient oSmtpServer = new SmtpClient(ptSmtp);

                
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Add Sender : " + ptMailSender);
                oMail.From = new MailAddress(ptMailSender);

                // To.
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Add To : " + ptMailReceive);
                if (!string.IsNullOrEmpty(ptMailReceive))
                {
                    string[] aTo = ptMailReceive.Split(',');
                    foreach (string tTo in aTo)
                    {
                        oMail.To.Add(tTo);
                    }
                }

                // CC.
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Add Cc. : " + ptCC);
                if (!string.IsNullOrEmpty(ptCC))
                {
                    string[] aCC = ptCC.Split(',');
                    foreach(string tCC in aCC)
                    {
                        oMail.CC.Add(tCC);
                    }
                }

                // BCC.
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Add Bcc. : " + ptBCC);
                if (!string.IsNullOrEmpty(ptBCC)) //Mail BCC
                {
                    string[] aBCC = ptBCC.Split(',');
                    foreach (string tBCC in aBCC)
                    {
                        oMail.Bcc.Add(tBCC);
                    }
                }

                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Add Subjedt. : " + ptSubj);
                oMail.Subject = ptSubj; // Subject
                oMail.IsBodyHtml = true;
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Add Content. : " + ptBody);
                oMail.Body = ptBody; //Content


                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "SmtpClient smtp : "+ ptSmtp);
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "SmtpClient Port. : " + pnPort.ToString());
                oSmtpServer.Port = pnPort;
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "SmtpClient User/password. : " + ptMailSender + "/ "+ ptPassword);
                oSmtpServer.Credentials = new System.Net.NetworkCredential(ptMailSender, ptPassword); // "username", "password"
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "SmtpClient Enable SSL = true");
                oSmtpServer.EnableSsl = true;
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Sending...");
                oSmtpServer.Send(oMail);
                new cLog().C_PRCxLogMonitor("C_PRCbFuncSendMail", "Send Success");
                return true;
            }
            catch(Exception oEx)
            {
                oEx.Message.ToString();
                new cLog().C_PRCxLog("C_PRCxFuncSendMail", "Error : " + oEx.Message.ToString());
                return false;
            }
        }
    }
}
