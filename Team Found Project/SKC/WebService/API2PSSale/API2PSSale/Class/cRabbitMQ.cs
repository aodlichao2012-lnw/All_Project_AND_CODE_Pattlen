using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using RabbitMQ.Client;
using System.Text;
using API2PSSale.Class.Standard;
using API2PSSale.Models;

namespace API2PSSale.Class
{
    public class cRabbitMQ
    {
        public static string tC_HostName { get; set; }
        public static string tC_UsrName { get; set; }
        public static string tC_Pwd { get; set; }
        public static string tC_VirtualHost { get; set; }
        public static string tC_QueueName { get; set; }
        public static string tC_QueueSale { get; set; }
        public static string tC_QueueShift { get; set; }
        public static string tC_QueueVoid { get; set; }
        public static string tC_QueueTax { get; set; }  //*Em 62-08-13
        public static string tC_QueueStkCrd { get; set; }
        public static string tC_QueueStkBal { get; set; }

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
                tC_QueueSale = "UPLOADSALE";
                tC_QueueShift = "UPLOADSHIFT";
                tC_QueueVoid = "UPLOADVOID";
                tC_QueueTax = "UPLOADTAX";    //*Em 62-08-13
                tC_QueueStkCrd = "DOWNLOADSTKCRD";   //*BOY 62-11-19
                tC_QueueStkBal = "DOWNLOADSTKBAL";   //*BOY 62-11-19
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check MQ Connection (Arm 63-07-31 ยกมาจาก Moshi)
        /// </summary>
        /// <returns></returns>
        public bool C_bCHKMQConnection()
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
                        return true;
                    }
                }
            }
            catch { }
            return false;
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
        /// <param name="ptBchCode">รหัสสาขา</param>
        /// <param name="ptPosCode">รหัสเครื่องจุดขาย</param>
        /// <param name="ptDocNo">เลขที่เอกสาร</param>
        /// <returns>
        /// ข้อความ Json
        /// </returns>
        public string C_CRTtMsgSale(string ptBchCode, string ptPosCode, string ptDocNo, string ptConnStr)
        {
            try
            {
                cmlRcvSalePos oSalePos = new cmlRcvSalePos();
                string tMsg = "";

                oSalePos.ptBchCode = ptBchCode;
                oSalePos.ptPosCode = ptPosCode;
                oSalePos.ptXihDocNo = ptDocNo;
                oSalePos.ptConnStr = ptConnStr;

                tMsg = Newtonsoft.Json.JsonConvert.SerializeObject(oSalePos);

                return tMsg;
            }
            catch
            {
                return "";
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