using API2ARDoc.Class.Standard;
using API2ARDoc.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace API2ARDoc.Class
{
    public class cRabbitMQ
    {
        public static string tC_HostName { get; set; }
        public static string tC_UsrName { get; set; }
        public static string tC_Pwd { get; set; }
        public static string tC_VirtualHost { get; set; }
        public static string tC_QueueUpdSaleRF { get; set; }

        /// <summary>
        /// Get config RabbitMQ
        /// </summary>
        /// <returns></returns>
        public bool C_GETbLoadConfigMQ()
        {
            try
            {
                var oAppSetting = ConfigurationManager.AppSettings;
                tC_HostName = cSP.SP_DATtTripleDESDecryptData(oAppSetting.Get("RQHost"), cCS.tCS_SHA1Key2);
                tC_UsrName = cSP.SP_DATtTripleDESDecryptData(oAppSetting.Get("RQUsr"), cCS.tCS_SHA1Key2);
                tC_Pwd = cSP.SP_DATtTripleDESDecryptData(oAppSetting.Get("RQPwd"), cCS.tCS_SHA1Key2);
                tC_VirtualHost = cSP.SP_DATtTripleDESDecryptData(oAppSetting.Get("RQVirtual"), cCS.tCS_SHA1Key2);

                tC_QueueUpdSaleRF = "UPDATEREFER"; 
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Send Message to Server RabbitMQ
        /// </summary>
        /// <param name="ptMsg"></param>
        /// <returns></returns>
        public bool C_PRCbSendData2Srv(string ptMsg, string ptQueue)
        {
            try
            {
                ConnectionFactory oFactory = new ConnectionFactory();
                oFactory.HostName = tC_HostName;
                oFactory.UserName = tC_UsrName;
                oFactory.Password = tC_Pwd;
                oFactory.VirtualHost = tC_VirtualHost;
                using (IConnection oConn = oFactory.CreateConnection())
                {
                    using (IModel oChannel = oConn.CreateModel())
                    {
                        oChannel.QueueDeclare(ptQueue, false, false, false, null);
                        var oBody = Encoding.UTF8.GetBytes(ptMsg);
                        oChannel.BasicPublish("", ptQueue, false, null, oBody);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Create Message queue
        /// </summary>
        /// <param name="ptDataUpld">Json data</param>
        /// <param name="ptConnStr">Connection String</param>
        /// <returns>
        /// ข้อความ JSon
        /// </returns>
        public string C_CRTtMsgDataUpload(string ptDataUpld, String ptConnStr)
        {
            try
            {
                cmlRcvDataUpload oData = new cmlRcvDataUpload();
                string tMsg = "";

                oData.ptData = ptDataUpld;
                oData.ptConnStr = ptConnStr;

                tMsg = Newtonsoft.Json.JsonConvert.SerializeObject(oData);

                return tMsg;
            }
            catch
            {
                return "";
            }
        }
    }
}