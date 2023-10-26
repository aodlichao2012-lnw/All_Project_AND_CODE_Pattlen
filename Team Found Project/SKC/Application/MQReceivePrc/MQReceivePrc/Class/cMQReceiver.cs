using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Bch2Bch;
using MQReceivePrc.Models.Database;
using MQReceivePrc.Models.Pos;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.SaleVD;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cMQReceiver
    {
        public static cConfig oC_Config;

        private IConnection oC_MQConn;//*Net 63-09-02 เอา Connection มาเป็น Global

        /// <summary>
        /// Constructor.
        /// </summary>
        public cMQReceiver()
        {
            try
            {
                if (oC_Config == null)
                {
                    oC_Config = new cConfig();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Process receive message RabbitMQ.
        /// </summary>
        public void C_MQRxProcess()
        {
            ConnectionFactory oFactory;
            ThreadStart oStart;
            Thread oTherad;
            //IConnection oConn; //*Net 63-09-02 ย้ายไป Global
            IModel oChannel;
            cMS oMsg;
            //string[] atQueue;
            List<string> atQueue; //*Net 63-09-02 เปลี่ยนไปใช้ List
            string tMsgErr;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oMsg = new cMS();
                if (oC_Config.C_CFGbLoadConfig(out tMsgErr))
                {
                    Console.WriteLine("========================================================================");
                    Console.WriteLine("Host name:       {0}", oC_Config.oC_RabbitMQ.tMQHostName);
                    Console.WriteLine("User name:       {0}", oC_Config.oC_RabbitMQ.tMQUserName);
                    Console.WriteLine("Virtual host:    {0}", oC_Config.oC_RabbitMQ.tMQVirtualHost);
                    Console.WriteLine("========================================================================");

                    oFactory = new ConnectionFactory();
                    oFactory.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                    oFactory.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                    oFactory.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                    oFactory.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;

                    //*Net 63-09-02 แสดงชื่อ Console
                    Console.Title = oFactory.VirtualHost + "[" + Assembly.GetExecutingAssembly().GetName().Version + "]_" + cVB.tVB_UniqueTimeCre;

                    atQueue = oC_Config.oC_RabbitMQ.tMQListQueue.Split(',').ToList();
                    atQueue.Add("QRestart"); //*Net 63-09-02 บังคับเพิ่ม Q สำหรับ Restart
                    oC_MQConn = oFactory.CreateConnection(); //*Net 63-09-02 สร้าง 1 Connection พอ

                    foreach (string tQueue in atQueue)
                    {
                        if (string.Equals(tQueue, ""))
                        {
                            continue;
                        }

                        //oConn = oFactory.CreateConnection(); //*Net 63-09-02 สร้าง 1 Connection พอ แต่สร้าง Channel ตามจำนวน Q
                        //oChannel = oConn.CreateModel();
                        oChannel = oC_MQConn.CreateModel(); //*Net 63-09-02

                        oStart = () => C_PRCxMessage(oChannel, tQueue);
                        oTherad = new Thread(oStart);
                        oTherad.Name = tQueue;
                        oTherad.IsBackground = true;
                        oTherad.Start();
                    }

                    //*Net 63-09-02 รอการตรวจสอบ Connection แทนการรอกดปุ่ม
                    while (oC_MQConn.IsOpen)
                    {
                        Task.Delay(3000).Wait();
                    }
                    //Console.ReadLine();
                }
                else
                {
                    Console.WriteLine(oMsg.tMS_CfgLoadFalse + tMsgErr);
                    Console.ReadLine();
                }
            }
            catch (Exception oExn)
            {
                Console.WriteLine(oExn.Message.ToString());
                Console.ReadLine();
            }
            finally
            {
                oFactory = null ;
                oStart = null;
                oTherad = null;
                //oConn = null;
                oChannel = null;
                oMsg = null; 
                atQueue = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        /// Process receive message RabbitMQ by queue name.
        /// </summary>
        /// <param name="poChannel">Channel.</param>
        /// <param name="ptQueue">Queue name.</param>
        private void C_PRCxMessage(IModel poChannel, string ptQueue)
        {
            EventingBasicConsumer oConsumer;
            string tMessage, tFmtDateTime, tQueueID;
            byte[] aoBody;
            bool bStaConsume;
            string tErrMsg = "";    //*Em 62-01-30  Pandora
            bool bPrc = true;
            string tQTransfer = "";
            string tQPayCoupon = "";
            string tQAR_DocApprove = "";
            try
            {
                tFmtDateTime = "yyyy-MM-dd HH:mm:ss";
                tQTransfer = "";
                // วนรอกรณีที่ message broker ยังไม่มี queue name ตาม config.
                bStaConsume = false;
                while (bStaConsume == false)
                {
                    try
                    {
                        //*Em 62-10-28
                        //if (string.Equals(ptQueue, "BR_QTransfer"))
                        //{
                        //    tQTransfer = ptQueue + cVB.tVB_BchCode;
                        //    poChannel.QueueDeclare(queue: tQTransfer, durable: false, exclusive: false, autoDelete: false, arguments: null);
                        //    poChannel.ExchangeDeclare(exchange: "BR_XTransfer", type: "fanout");
                        //    poChannel.QueueBind(tQTransfer, "BR_XTransfer", "", null);
                        //    ptQueue = tQTransfer;
                        //}
                        //else
                        //{
                        //    poChannel.QueueDeclare(queue: ptQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                        //}
                        //++++++++++++++++++

                        //*Arm 62-12-26
                        switch (ptQueue)
                        {
                            case "BR_QTransfer":
                                //*Arm 63-02-26 - [Comment Code]
                                //tQTransfer = ptQueue + cVB.tVB_BchCode;
                                //poChannel.QueueDeclare(queue: tQTransfer, durable: false, exclusive: false, autoDelete: false, arguments: null);
                                //poChannel.ExchangeDeclare(exchange: "BR_XTransfer", type: "fanout");
                                //poChannel.QueueBind(tQTransfer, "BR_XTransfer", "", null);

                                ////*Arm 63-01-30  Bindings BR_XDownload ++++
                                //poChannel.ExchangeDeclare(exchange: "BR_XDownload", type: "fanout");
                                //poChannel.QueueBind(tQTransfer, "BR_XDownload", "", null);
                                ////++++++++++++++++++

                                //ptQueue = tQTransfer;
                                //++++++++++++++++++++++++++++++++++++++++++++++++++


                                //*Arm 63-02-26
                                //Declare Exchange
                                poChannel.ExchangeDeclare(exchange: "BR_XTransfer", type: "fanout");
                                poChannel.ExchangeDeclare(exchange: "BR_XDownload", type: "fanout");

                                if (cVB.tVB_BchCode == cVB.tVB_BchHQ)   //เป็น HQ
                                {
                                    StringBuilder oSql = new StringBuilder();
                                    DataTable oDbTbl = new DataTable();
                                    cDatabase oDB = new cDatabase();

                                    oSql.AppendLine("SELECT FTBchcode FROM TCNMBranch WITH(NOLOCK)");
                                    oDbTbl = oDB.C_DAToExecuteQuery(cVB.tVB_ConnStr, oSql.ToString(), cVB.nVB_CmdTime);

                                    if (oDbTbl != null && oDbTbl.Rows.Count > 0)
                                    {
                                        
                                        foreach (DataRow oRow in oDbTbl.Rows)
                                        {
                                            string tQueueName = "";
                                            //QueueName
                                            tQueueName = "BR_QTransfer" + oRow["FTBchcode"].ToString();

                                            //Declare Queue 
                                            poChannel.QueueDeclare(queue: tQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                                            //Bindings BR_XTransfer & BR_XDownload กรณีสาขา ไม่ใช่ HQ
                                            if (oRow["FTBchcode"].ToString() != cVB.tVB_BchHQ)
                                            {
                                                poChannel.QueueBind(tQueueName, "BR_XTransfer", "", null);
                                                poChannel.QueueBind(tQueueName, "BR_XDownload", "", null);
                                            }
                                        }
                                    }

                                    tQTransfer = ptQueue + cVB.tVB_BchCode;
                                    ptQueue = tQTransfer;
                                }
                                else
                                {
                                    string tQueueName = "";
                                    tQueueName = "BR_QTransfer" + cVB.tVB_BchCode;
                                    //Declare Queue 
                                    poChannel.QueueDeclare(queue: tQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                                    //Bindings BR_XTransfer & BR_XDownload กรณีสาขา ไม่ใช่ HQ
                                    if (cVB.tVB_BchCode != cVB.tVB_BchHQ)
                                    {
                                        poChannel.QueueBind(tQueueName, "BR_XTransfer", "", null);
                                        poChannel.QueueBind(tQueueName, "BR_XDownload", "", null);
                                    }
                                    
                                    tQTransfer = ptQueue + cVB.tVB_BchCode;
                                    ptQueue = tQTransfer;
                                }
                                
                                //+++++++++++++++++++++++++

                                break;

                            case "FN_PayReqCoupon":
                                // *Arm 63-01-07 
                                //*Net 63-05-18 ตรวจสอบ Centralized
                                if (cVB.bVB_StaUseCentralized == true)
                                {
                                    tQPayCoupon = ptQueue;
                                }
                                else
                                {
                                    tQPayCoupon = ptQueue + cVB.tVB_BchCode;
                                }
                                poChannel.QueueDeclare(queue: tQPayCoupon, durable: false, exclusive: false, autoDelete: false, arguments: null);
                                ptQueue = tQPayCoupon;

                                //if (cVB.tVB_BchCode != cVB.tVB_BchHQ)
                                //{
                                //    tQTransfer = ptQueue + cVB.tVB_BchCode;
                                //    poChannel.QueueDeclare(queue: tQTransfer, durable: false, exclusive: false, autoDelete: false, arguments: null);
                                //    ptQueue = tQTransfer;
                                //}
                                //else
                                //{
                                //    StringBuilder oSql = new StringBuilder();
                                //    DataTable oDbTbl = new DataTable();
                                //    cDatabase oDB = new cDatabase();
                                //    oSql.AppendLine("SELECT FTBchcode FROM TCNMBranch WITH(NOLOCK)");
                                //    oDbTbl = oDB.C_DAToExecuteQuery(cVB.tVB_ConnStr, oSql.ToString(), cVB.nVB_CmdTime);

                                //    if(oDbTbl != null && oDbTbl.Rows.Count > 0 )
                                //    {
                                //        foreach(DataRow oRow in oDbTbl.Rows)
                                //        {
                                //            tQTransfer = ptQueue + oRow["FTBchcode"].ToString();
                                //            poChannel.QueueDeclare(queue: tQTransfer, durable: false, exclusive: false, autoDelete: false, arguments: null);
                                //            ptQueue = tQTransfer;
                                //        }
                                //    }
                                //}
                                break;

                            case "AR_QGenTARTSO":
                                if (String.IsNullOrEmpty(cVB.tVB_BchCode)) Environment.Exit(0);
                                tQTransfer = ptQueue + cVB.tVB_BchCode;
                                poChannel.QueueDeclare(queue: tQTransfer, durable: true, exclusive: false, autoDelete: false, arguments: null);
                                ptQueue = tQTransfer;
                                break;
                            case "AR_QNotiMsgPrc":
                                if (String.IsNullOrEmpty(cVB.tVB_BchCode)) Environment.Exit(0);
                                tQTransfer = ptQueue + cVB.tVB_BchCode;
                                poChannel.QueueDeclare(queue: tQTransfer, durable: true, exclusive: false, autoDelete: false, arguments: null);
                                ptQueue = tQTransfer;
                                break;

                            case "AR_DocApprove":
                                //*Arm 63-02-26
                                tQAR_DocApprove = ptQueue + cVB.tVB_BchCode;
                                poChannel.QueueDeclare(queue: tQAR_DocApprove, durable: false, exclusive: false, autoDelete: false, arguments: null);
                                ptQueue = tQAR_DocApprove;
                                //++++++++++++++++
                                break;

                            case "FN_DocApprove":
                                //*Net 63-03-13
                                ptQueue = ptQueue + cVB.tVB_BchCode;
                                poChannel.QueueDeclare(queue: ptQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                                //++++++++++++++++
                                break;
                            case "QRestart":
                                //*Net 63-09-02 สร้างคิวสำหรับ Restart
                                ptQueue = ptQueue + "_" + cVB.tVB_UniqueTimeCre;
                                poChannel.ExchangeDeclare(exchange: "AR_XReStartConsumer", type: "fanout");
                                poChannel.QueueDeclare(queue: ptQueue, durable: false, exclusive: true, autoDelete: true, arguments: null);
                                poChannel.QueueBind(ptQueue, "AR_XReStartConsumer", "", null);

                                //++++++++++++++++
                                break;
                            default:
                                poChannel.QueueDeclare(queue: ptQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                                break;
                        }

                        oConsumer = new EventingBasicConsumer(poChannel);
                        oConsumer.Received += (oModel, oEevntArgs) =>
                        {
                            aoBody = oEevntArgs.Body;
                            tMessage = Encoding.UTF8.GetString(aoBody);
                            tQueueID = oEevntArgs.ConsumerTag.ToString().Substring(9) + oEevntArgs.DeliveryTag.ToString();

                            //if(ptQueue.Length >= 15 && string.Equals(ptQueue.Substring(0, 15), "FN_PayReqCoupon"))
                            if(ptQueue.Contains("FN_PayReqCoupon")) //*Net 63-03-13
                            {
                                ptQueue = "FN_PayReqCoupon";
                            }
                            //if(ptQueue.Replace("AR_QGenTARTSO","")!=ptQueue)
                            if (ptQueue.Contains("AR_QGenTARTSO")) //*Net 63-03-13
                            {
                                ptQueue = "AR_QGenTARTSO";
                            }
                            //if (ptQueue.Replace("AR_QNotiMsgPrc", "") != ptQueue)
                            if (ptQueue.Contains("AR_QNotiMsgPrc")) //*Net 63-03-13
                            {
                                ptQueue = "AR_QNotiMsgPrc";
                            }

                            //*Arm 63-02-26
                            //if (ptQueue.Replace("AR_DocApprove", "") != ptQueue)
                            if (ptQueue.Contains("AR_DocApprove")) //*Net 63-03-13
                            {
                                ptQueue = "AR_DocApprove";
                            }
                            //+++++++++++++++++

                            //*Net 63-03-11
                            //if (ptQueue.Replace("FN_DocApprove", "") != ptQueue)
                            if (ptQueue.Contains("FN_DocApprove")) //*Net 63-03-13
                            {
                                ptQueue = "FN_DocApprove";
                            }
                            //+++++++++++++++++

                            //*Net 63-09-02
                            if (ptQueue.Contains("QRestart"))
                            {
                                ptQueue = "QRestart";
                            }
                            //+++++++++++++++++

                            switch (ptQueue)
                            {
                                case "SALEPOS": //*Em 61-11-29  Pandora
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SalePos Start...");

                                        if (C_PRCbSalePos(tQueueID, tMessage))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SalePos Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SalePos Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SalePos Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/SALEPOS");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "CONSOLIDATE":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Consolidate Start...");

                                        if (C_PRCbConsolidate(tMessage))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Consolidate Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Consolidate Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Consolidate Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CONSOLIDATE");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                case "FCBCH2HQSALE":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Food Court Sale Start...");

                                        if (C_PRCbFCBch2HQSale(tMessage))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Food Court Sale Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Food Court Sale Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Food Court Sale Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/FCBCH2HQSALE");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "CARDREQUEST": //*Em 61-11-29  Pandora
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Request Process Start...");
                                        cPrcWallet oPrcWallet = new cPrcWallet();
                                        tErrMsg = "";   //*Em 62-01-30  Pandora
                                        if (oPrcWallet.C_PRCbCrdRequest(tMessage, oC_Config.oC_ShopDB, ref tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Request Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Request Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Request Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CARDREQUEST");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "CARDRETURN":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Return Process Start...");
                                        cPrcWallet oPrcWallet = new cPrcWallet();
                                        tErrMsg = "";   //*Em 62-01-30  Pandora
                                        if (oPrcWallet.C_PRCbCrdReturn(tMessage, oC_Config.oC_ShopDB, ref tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Return Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Return Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Return Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CARDRETURN");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "CARDTOPUP":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Topup Process Start...");
                                        cPrcWallet oPrcWallet = new cPrcWallet();
                                        tErrMsg = "";   //*Em 62-01-30  Pandora
                                        if (oPrcWallet.C_PRCbCrdTopUp(tMessage, oC_Config.oC_ShopDB, ref tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Topup Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Topup Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Topup Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CARDTOPUP");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "CARDVOIDTOPUP":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Void Topup Process Start...");
                                        cPrcWallet oPrcWallet = new cPrcWallet();
                                        tErrMsg = "";   //*Em 62-01-30  Pandora
                                        if (oPrcWallet.C_PRCbCrdReturnTopUp(tMessage, oC_Config.oC_ShopDB, ref tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Void Topup Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Void Topup Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Void Topup Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CARDVOIDTOPUP");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "CARDADJSTATUS":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Adjust Status Process Start...");
                                        cPrcWallet oPrcWallet = new cPrcWallet();
                                        tErrMsg = "";   //*Em 62-01-30  Pandora
                                        if (oPrcWallet.C_PRCbCrdAdjStatus(tMessage, oC_Config.oC_ShopDB, ref tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Adjust Status Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Adjust Status Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Adjust Status Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CARDADJSTATUS");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "CARDSWAP":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Swap Process Start...");
                                        cPrcWallet oPrcWallet = new cPrcWallet();
                                        tErrMsg = "";   //*Em 62-01-30  Pandora
                                        if (oPrcWallet.C_PRCbCrdSwap(tMessage, oC_Config.oC_ShopDB, ref tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Swap Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Swap Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Swap Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CARDSWAP");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "CARDNEW":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card New Process Start...");
                                        cPrcWallet oPrcWallet = new cPrcWallet();
                                        tErrMsg = "";   //*Em 62-01-30  Pandora
                                        if (oPrcWallet.C_PRCbCrdNew(tMessage, oC_Config.oC_ShopDB, ref tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card New Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card New Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card New Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CARDNEW");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "CARDCLEAR":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Clear Process Start...");
                                        cPrcWallet oPrcWallet = new cPrcWallet();
                                        tErrMsg = "";   //*Em 62-01-30  Pandora
                                        if (oPrcWallet.C_PRCbCrdClear(tMessage, oC_Config.oC_ShopDB, ref tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Clear Process Success.");
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);  //*Em 62-01-30  Pandora
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Clear Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Card Clear Error.");
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CARDCLEAR");
                                    }
                                    Thread.Sleep(500);
                                    break;
                                case "ADJUSTPRICE":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust Price Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cAdjPrice oAdjPrice = new cAdjPrice();
                                        bPrc = oAdjPrice.C_PRCbPdtPrice(oDocApv, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust Price Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust Price Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust Price Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/ADJUSTPRICE");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                case "TNFWAREHOSE":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPdtTnfWarehouse oPdtTnfWah = new cPdtTnfWarehouse();
                                        bPrc = oPdtTnfWah.C_PRCbTnfWah(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/TNFWAREHOSE");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                case "TNFWAREHOSEOUT":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Out Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPdtTnfWarehouse oPdtTnfWahOut = new cPdtTnfWarehouse();
                                        bPrc = oPdtTnfWahOut.C_PRCbTnfWahOut(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Out Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Out Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + "  Transfer Warehouse Out Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/TNFWAREHOSEOUT");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                case "TNFWAREHOSEIN":
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + "  Transfer Warehouse In Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPdtTnfWarehouse oPdtTnfWahIn = new cPdtTnfWarehouse();
                                        bPrc = oPdtTnfWahIn.C_PRCbTnfWahIn(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse In Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse In Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse In Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/TNFWAREHOSEIN");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "PURCHASEINV":     //*Em 62-06-14
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + "  Purchase invoice In Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPurchase oPurchase = new cPurchase();
                                        bPrc = oPurchase.C_PRCbPurchaseInvoice(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Purchase invoice In Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Purchase invoice In Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Purchase invoice In Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/PURCHASEINV");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "PURCHASECN":      //*Em 62-06-17
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + "  Purchase credit note In Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPurchase oPurchase = new cPurchase();
                                        bPrc = oPurchase.C_PRCbPurchaseCN(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Purchase credit note In Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Purchase credit note In Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Purchase credit note In Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/PURCHASECN");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "ADJUSTSTOCK":     //*Em 62-07-03
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust stock Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cAdjStock oAdjStock = new cAdjStock();
                                        bPrc = oAdjStock.C_PRCbAdjustStock(oDocApv, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust stock Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust stock Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust stock Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/ADJUSTSTOCK");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "TNFWAREHOSEVD":   //*Em 62-07-22
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Vending Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPdtTnfWarehouse oPdtTnfWah = new cPdtTnfWarehouse();
                                        bPrc = oPdtTnfWah.C_PRCbTnfWahVD(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Vending Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Vending Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Warehouse Vending Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/TNFWAREHOSEVD");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                case "ADJUSTPRICERT":   //*Em 62-07-24
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust Price Rental Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cAdjPrice oAdjPrice = new cAdjPrice();
                                        bPrc = oAdjPrice.C_PRCbPdtPriceRT(oDocApv, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust Price Rental Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust Price Rental Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Adjust Price Rental Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/ADJUSTPRICERT");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "RTSALESTATUS":    //*Em 62-07-25
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update status Rental Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cRental oRental = new cRental();
                                        bPrc = oRental.C_PRCbUpdateStatus(oDocApv, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update status Rental Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update status Rental Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update status Rental Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/RTSALESTATUS");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "SALEPOSVD":   //*Em 62-07-25
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Sale Vending Process Start...");
                                        cmlRcvSalePos oSalePos = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvSalePos>(tMessage);
                                        cSaleVD oSaleVD = new cSaleVD();
                                        bPrc = oSaleVD.C_PRCbSalePos(oSalePos, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Sale Vending  Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Sale Vending  Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Sale Vending  Rental Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/SALEPOSVD");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADSALEVD":   //*Em 62-07-25
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Vending Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cSaleVD oSaleVD = new cSaleVD();
                                        //bPrc = oSaleVD.C_PRCbUploadSaleVD(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        bPrc = oSaleVD.C_PRCbUploadSaleVD(tQueueID,oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg); //*Em 63-08-17
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Vending  Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Vending  Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Vending Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADSALEVD");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADSALE":  //*Em 62-07-25
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cSale oSale = new cSale();
                                        //bPrc = oSale.C_PRCbUploadSale(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        bPrc = oSale.C_PRCbUploadSale(tQueueID, oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg); //*Arm 63-05-30 //*Net 63-08-03 ยกมาจาก Moshi
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADSALE");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADTAX":  //*Em 62-08-13
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Tax Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cTax oTax = new cTax();
                                        //bPrc = oTax.C_PRCbUploadTax(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        bPrc = oTax.C_PRCbUploadTax(tQueueID, oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg); // Net 63-08-03 ยกมาจาก Moshi
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Tax Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Tax Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Tax Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADTAX");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADSHIFT": //*Em 62-08-24
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Shift Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cShift oShift = new cShift();
                                        //bPrc = oShift.C_PRCbUploadShift(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        bPrc = oShift.C_PRCbUploadShift(tQueueID, oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg); //*Net 63-06-09 add para tQueueID // Net 63-08-03 ยกมาจาก Moshi
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Shift Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Shift Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Shift Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADSHIFT");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADVOID":  //*Em 62-08-24
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Void Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cVoid oVoid = new cVoid();
                                        //bPrc = oVoid.C_PRCbUploadVoid(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        bPrc = oVoid.C_PRCbUploadVoid(tQueueID, oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg); //*Net 63-06-09 Add tQueueID // Net 63-08-03 ยกมาจาก Moshi
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Void Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Void Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Void Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADVOID");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "TNFBRANCH":    //*Em 62-08-27
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Branch Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPdtTnfBch oPdtTnfBch = new cPdtTnfBch();
                                        bPrc = oPdtTnfBch.C_PRCbTnfBch(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Branch Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Branch Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Branch Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/TNFBRANCH");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "POSEJ":   //*Em 62-09-16
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Pos EJ Process Start...");
                                        cmlRcvSalePos oSalePos = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvSalePos>(tMessage);
                                        cSale oSale = new cSale();
                                        //bPrc = oSale.C_PRCbCrateEJ(oSalePos, oC_Config.oC_ShopDB, ref tErrMsg);
                                        bPrc = oSale.C_PRCbCreateEJ(oSalePos, oC_Config.oC_ShopDB, ref tErrMsg); // Net 63-08-03 ยกมาจาก Moshi
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Pos EJ Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Pos EJ Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Pos EJ Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/POSEJ");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADSALERT":   //*Arm 62-09-24
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale RT Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cSaleRT oSaleRT = new cSaleRT();
                                        bPrc = oSaleRT.C_PRCbUploadSaleRT(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale RT Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale RT Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale RT Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADSALERT");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADSALEPAY":   //*Arm 62-09-24
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale Pay Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cSaleRT oSaleRT = new cSaleRT();
                                        bPrc = oSaleRT.C_PRCbUploadSalePay(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale Pay Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale Pay Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Sale Pay Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADSALEPAY");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADADMINHIS":   //*Arm 62-09-24
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update AdminHis Process Start...");

                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cSaleRT oSaleRT = new cSaleRT();
                                        bPrc = oSaleRT.C_PRCbUploadAdminHis(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update AdminHis Process Success.");

                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update AdminHis Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload AdminHis Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADADMINHIS");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "CALLSENDBCH":     //*Em 62-10-13
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Call Send Branch Process Start...");

                                        cmlCallSendBch oCallSendBch = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlCallSendBch>(tMessage);
                                        cPrcBch2Bch oPrcBch = new cPrcBch2Bch();
                                        bPrc = oPrcBch.C_PRCbDataBch2Bch(oCallSendBch, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Call Send Branch Process Success.");

                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Call Send Branch Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Call Send Branch Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CALLSENDBCH");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "BCHRECEIVE":     //*Em 62-10-13
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Branch received Process Start...");

                                        cmlTCNTSync2BchHis oBchReceive = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlTCNTSync2BchHis>(tMessage);
                                        cPrcBch2Bch oPrcBch = new cPrcBch2Bch();
                                        bPrc = oPrcBch.C_PRCbBchReceive(oBchReceive, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Branch received Process Success.");

                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Branch received Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Branch received Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/BCHRECEIVE");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPDRECEIVE":     //*Em 62-10-16
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update branch received Process Start...");

                                        cmlTCNTSync2BchHis oBchReceive = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlTCNTSync2BchHis>(tMessage);
                                        cPrcBch2Bch oPrcBch = new cPrcBch2Bch();
                                        bPrc = oPrcBch.C_PRCbUpdBchReceive(oBchReceive, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update branch received Process Success.");

                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update branch received Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update branch received Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPDRECEIVE");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPLOADBOOKING":   //*BOY 62-11-1
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Booking Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cSaleRT oSaleRT = new cSaleRT();
                                        bPrc = oSaleRT.C_PRCbUploadBooking(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Booking Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Booking Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Booking Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADBOOKING");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                    
                                case "UPLOADSTKCRD": //*Arm 62-11-19  UPLOADSTKCRD
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Stock Card Process Start...");
                                        cmlRcvDocApv oDocApv =  Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cStock oStock = new cStock();
                                        bPrc = oStock.C_PRCbGetStkCrd(oDocApv, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Stock Card Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Stock Card Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Stock Card Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADSTKCRD");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                    
                                case "UPLOADSTKBAL":    //*Arm 62-11-20  UPLOADSTKBAL
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Stock Balance Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cStock oStock = new cStock();
                                        bPrc = oStock.C_PRCbGetStkBal(oDocApv, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Stock Balance Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Stock Balance Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Stock Balance Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPLOADSTKBAL");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                    
                                case "DOWNLOADSTKCRD":  //*Arm 62-11-19  DOWNLOADSTKCRD
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Download Stock Card Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cStock oStock = new cStock();
                                        bPrc = oStock.C_PRCbDownloadStkCrd(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Download Stock Card Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Download Stock Card Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Download Stock Card Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/DOWNLOADSTKCRD");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                    
                                case "DOWNLOADSTKBAL":  //*Arm 62-11-19  DOWNLOADSTKBAL
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Download Stock Balance Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cStock oStock = new cStock();
                                        bPrc = oStock.C_PRCbDownloadStkBal(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Download Stock Balance Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Download Stock Balance Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Download Stock Card Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/DOWNLOADSTKBAL");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "FN_PayReqCoupon":  //*Arm 62-12-27 ชำระคูปองแบบ จำกัดจำนวน
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Pay Coupon Process Start...");
                                        cmlRcvData oRcvData = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvData>(tMessage);
                                        cPayCoupon oPayCoupon = new cPayCoupon();
                                        bPrc = oPayCoupon.C_PRCbPayCoupon(oRcvData, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Pay Coupon Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Pay Coupon Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Pay Coupon Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/FN_PayReqCoupon");
                                    }
                                    Thread.Sleep(500);
                                    break;

                                case "AR_QGenTARTSO":  //*Net รอรับข้อมูลการสร้างใบ SO
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " GenSO Process Start...");
                                        cmlRcvData oRcvData = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvData>(tMessage);
                                        cGenSO oGenSO = new cGenSO();

                                        bPrc = oGenSO.C_PRCbGenSO(oRcvData, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " GenSO Process Success.");
                                        }
                                        else
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " GenSO Process false.");
                                        }
                                        Console.WriteLine();
                                    }
                                    catch (Exception oEx)
                                    {
                                        poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " GenSO Error. " + oEx.Message.ToString());
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " GenSO Process false.");
                                        Console.WriteLine();
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/AR_QGenTARTSO");
                                    }
                                    Thread.Sleep(500);
                                    break;

                                case "AR_QNotiMsgPrc":  //*Net รอรับเอกสาร SO
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " NotiDocSO Process Start...");
                                        cmlRcvData oRcvData = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvData>(tMessage);
                                        cNotiDocPrc oNotiDoc = new cNotiDocPrc();

                                        bPrc = oNotiDoc.C_PRCbNotiDoc(oRcvData, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " NotiDocSO Process Success.");
                                        }
                                        else
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " NotiDocSO Process false.");
                                        }
                                        Console.WriteLine();
                                    }
                                    catch (Exception oEx)
                                    {
                                        poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " NotiDocSO Error. " + oEx.Message.ToString());
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + ptQueue + "]" + " NotiDocSO Process false.");
                                        Console.WriteLine();
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/AR_QNotiMsgPrc");
                                    }
                                    Thread.Sleep(500);
                                    break;

                                case "AR_DocApprove":    //*Arm 63-02-26  Transfer Redeem Point
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Document Redeem Point Process Start...");
                                        cmlBchDownload oBchDwn = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlBchDownload>(tMessage);

                                        cTnfRedeemPoint2Bch oTnfRedeemPoint2Bch = new cTnfRedeemPoint2Bch();
                                        bPrc = oTnfRedeemPoint2Bch.C_PRCbCreateUrlRedeemPoint(oBchDwn, oC_Config.oC_ShopDB, ref tErrMsg);
                                        
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Document Redeem Point Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Document Redeem Point Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Document Redeem Point Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/AR_DocApprove");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "FN_DocApprove":    //*Net 63-03-13 Transfer Coupon GiftVoucher
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Document Coupon GiftVoucher Process Start...");
                                        cmlBchDownload oBchDwn = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlBchDownload>(tMessage);

                                        cTnfCouponGiftVoucher2Bch oTnfCouponGiftVoucher2Bch = new cTnfCouponGiftVoucher2Bch();
                                        bPrc = oTnfCouponGiftVoucher2Bch.C_PRCbCreateUrlCouponGiftVoucher(oBchDwn, oC_Config.oC_ShopDB, ref tErrMsg);

                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Document Coupon GiftVoucher Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Document Coupon GiftVoucher Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Document Coupon GiftVoucher Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/FN_DocApprove");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "TNFBRANCHOUT":    //*Em 63-03-25
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Out Branch Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPdtTnfBch oPdtTnfBch = new cPdtTnfBch();
                                        bPrc = oPdtTnfBch.C_PRCbTnfBchOut(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Out Branch Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Out Branch Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer Out Branch Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/TNFBRANCHOUT");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "TNFBRANCHIN":    //*Em 63-03-25
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer In Branch Process Start...");
                                        cmlRcvDocApv oDocApv = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDocApv>(tMessage);
                                        cPdtTnfBch oPdtTnfBch = new cPdtTnfBch();
                                        bPrc = oPdtTnfBch.C_PRCbTnfBchIn(oDocApv, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer In Branch Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer In Branch Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Transfer In Branch Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/TNFBRANCHIN");
                                    }

                                    Thread.Sleep(500);
                                    break;

                                case "UPDATEREFER":    //*Arm 63-05-21 อัพเดตอ้างอิงบิลคืน-ขาย กรณีคืนข้ามเครื่อง
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale Refer Process Start...");
                                        cmlRcvDataUpload oDataUpld = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvDataUpload>(tMessage);
                                        cUpdSaleRefer oSaleRefer = new cUpdSaleRefer();
                                        bPrc = oSaleRefer.C_UPDbUpddateQtyRefer(oDataUpld, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale Refer Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale Refer Process false.");

                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Update Sale Refer Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/UPDATEREFER");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                // Net 63-08-03 ยกมาจาก Moshi
                                case "CN_QReqGenTaxNo":    //*Arm 63-05-22
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Gen. TaxNo. Process Start...");
                                        cRcvGenTexNo oRcvGenTexNo = Newtonsoft.Json.JsonConvert.DeserializeObject<cRcvGenTexNo>(tMessage);
                                        cGenDocNo oGenDocNo = new cGenDocNo();
                                        bPrc = oGenDocNo.C_PRCbGENTexNo(oRcvGenTexNo, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Gen. TaxNo. Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Gen. TaxNo. Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Gen. TaxNo. Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/CN_QReqGenTaxNo");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                case "PS_QListDocByShift":    //*Net 63-06-04
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Fix. Doc. Process Start...");
                                        cmlCheckListSalDoc oListDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlCheckListSalDoc>(tMessage);
                                        cFixDoc oFixDoc = new cFixDoc();
                                        bPrc = oFixDoc.C_PRCbFixDoc(oListDoc, oC_Config.oC_ShopDB, out tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Fix. Doc. Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Fix. Doc. Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Fix. Doc. Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/PS_QListDocByShift");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                case "PS_QSaleExpress":    //*Net 63-06-16
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Express Process Start...");
                                        cExpress oExpress = new cExpress();
                                        bPrc = oExpress.C_PRCbUploadExpress(tQueueID, tMessage, oC_Config.oC_ShopDB, ref tErrMsg);
                                        if (bPrc)
                                        {
                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Express Process Success.");
                                        }
                                        else
                                        {
                                            //*Net 63-09-02 ถ้า Error จาก SQL SERVER ให้เอา Msg ส่งกลับเข้า Queue
                                            if (C_CHKbSQlServErr(tErrMsg))
                                            {
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " SQL Server Error");
                                                Thread.Sleep(10000);
                                                poChannel.BasicNack(oEevntArgs.DeliveryTag, false, true);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                            }
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Express Process false.");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Upload Express Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/PS_QSaleExpress");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                case "QRestart":    //*Net 63-09-02
                                    try
                                    {
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Restart Process Start...");

                                        poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        if (C_PRCxRestart(tMessage, out tErrMsg))
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Restart Process Success.");
                                        }
                                        else
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Restart Error. " + oEx.Message.ToString());
                                        cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/QRestart");
                                    }

                                    Thread.Sleep(500);
                                    break;
                                //++++++++++++++++++++++++ Net 63-08-03 ยกมาจาก Moshi
                                default:
                                    //*Em 62-10-28
                                    if (ptQueue == tQTransfer)
                                    {
                                        try
                                        {
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Branch download Process Start...");

                                            cmlBchDownload oBchDwn = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlBchDownload>(tMessage);
                                            
                                            switch (oBchDwn.ptFunction)
                                            {
                                                case "BankDepositSlip":

                                                    cTnfBankDepositSlip2BchHQ oTnfBnkDpl2BchHQ = new cTnfBankDepositSlip2BchHQ();
                                                    if (oBchDwn.ptDest =="HQ") 
                                                    {
                                                        // HQ Download Tranfer Bank Deposit Slip to Database
                                                        bPrc = oTnfBnkDpl2BchHQ.C_PRCbDownloadBankDepositSlip2BchHQ(oBchDwn, oC_Config.oC_ShopDB, ref tErrMsg);
                                                    }
                                                    else
                                                    {
                                                        // Barnch Upload Tranfer Bank Deposit Slip to HQ
                                                        bPrc = oTnfBnkDpl2BchHQ.C_PRCbTnfBankDepositSlip2BchHQ(oBchDwn, oC_Config.oC_ShopDB, ref tErrMsg);
                                                    }
                                                    break;

                                                case "RedeemPoint":     //*Arm 63-02-25

                                                    cTnfRedeemPoint2Bch oTnfRedeemPoint2Bch = new cTnfRedeemPoint2Bch();
                                                    if (cVB.tVB_BchCode == oBchDwn.ptFilter)  // เป็น HQ
                                                    {
                                                        bPrc = true;
                                                    }
                                                    else
                                                    {
                                                        // Branch Doenload ข้อมูลเอกสารแลกแต้มจาก HQ ตาม URL ที่ส่งมา
                                                        bPrc = oTnfRedeemPoint2Bch.C_PRCbDownloadRedeemPoint(oBchDwn, oC_Config.oC_ShopDB, ref tErrMsg);
                                                    }
                                                    break;

                                                case "CouponGiftVoucher":     //*Net 63-03-13

                                                    cTnfCouponGiftVoucher2Bch oTnfCouponGiftVoucher2Bch = new cTnfCouponGiftVoucher2Bch();
                                                    if (cVB.tVB_BchCode == oBchDwn.ptFilter)  // เป็น HQ
                                                    {
                                                        bPrc = true;
                                                    }
                                                    else
                                                    {
                                                        // Branch Doenload ข้อมูลเอกสารแลกแต้มจาก HQ ตาม URL ที่ส่งมา
                                                        bPrc = oTnfCouponGiftVoucher2Bch.C_PRCbDownloadCouponGiftVoucher(oBchDwn, oC_Config.oC_ShopDB, ref tErrMsg);
                                                    }
                                                    break;

                                                default:

                                                    // Download Master
                                                    cPrcBch2Bch oPrcBch = new cPrcBch2Bch();
                                                    bPrc = oPrcBch.C_PRCbBchDownload(oBchDwn, ref tErrMsg);
                                                    break;
                                            }
                                            
                                            if (bPrc)
                                            {
                                                poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Branch download  Process Success.");
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "] " + tErrMsg);
                                                Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Branch download  Process false.");
                                            }
                                        }
                                        catch (Exception oEx)
                                        {
                                            // ต้องเขียน log ลง text file ด้วย
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " Branch download  Error. " + oEx.Message.ToString());
                                            cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxMessage/BR_QDownload");
                                        }
                                    }
                                    Thread.Sleep(500);
                                    break;
                            };

                            //*Net 63-03-03 ถ้ารับค่ามาจาก Q ที่มีชื่ออยู่ ให้ Check Min Stock
                            //string tListQNoti = "AR_QGenTARTSO,CARDREQUEST,CARDRETURN,CARDTOPUP,CARDVOIDTOPUP,CARDADJSTATUS," +
                            //"CARDSWAP,CARDNEW,CARDCLEAR,ADJUSTPRICE,TNFWAREHOSE,TNFWAREHOSEOUT," +
                            //"TNFWAREHOSEIN,PURCHASEINV,PURCHASECN,ADJUSTSTOCK,TNFWAREHOSEVD,ADJUSTPRICERT" +
                            //"SALEPOS,CONSOLIDATE,FCBCH2HQSALE,SALEPOSVD,POSEJ,FN_PayReqCoupon";

                            //if (tListQNoti.Trim().Split(',').Contains(ptQueue))
                            //{
                            //    //cNotiMinStk oNotiMinStk = new cNotiMinStk();
                            //    //oNotiMinStk.C_PRCxNotiMinStk(oC_Config.oC_ShopDB, ref tErrMsg);
                            //}
                            //----------------------------------
                        };
                        poChannel.BasicConsume(queue: ptQueue, autoAck: false, consumer: oConsumer);
                        bStaConsume = true;
                        Thread.Sleep(500);
                    }
                    catch (RabbitMQ.Client.Exceptions.RabbitMQClientException oRMQExn)
                    {
                        switch (oRMQExn.HResult)
                        {
                            case -2146233088: // Queue name not found.
                                System.Threading.Thread.Sleep(20000); // 1000 milliseconds = 1 second.
                                break;
                        }
                    }
                    catch (Exception oEx)
                    {
                        tMessage = oEx.Message.ToString();
                    }
                }
            }
            catch (Exception)
            {
                // ต้องเขียน log ลง text file ด้วย
                Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + ptQueue + " : Exception");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Process stock sale.
        /// </summary>
        /// <param name="ptMessage">Data message from RabbitMQ.</param>
        /// <returns>
        /// true: process success.
        /// false: process false.
        /// </returns>
        private bool C_PRCbSalePos(string ptComName, string ptMessage)
        {
            DataTable odtTmp;
            StringBuilder oSql;
            cDatabase oDB = new cDatabase();
            cmlRcvSalePos oSalePos;
            string tConnStr = "";
            int nRowAffect = 0;
            string tErrMsg = "";    //*Arm 62-11-19
            try
            {
                oSalePos = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvSalePos>(ptMessage);
                
                //1.ตรวจสอบข้อมูล
                if (oSalePos == null) return false;
                tConnStr = oDB.C_GETtConnectString4PrcConsolidate(oSalePos.ptConnStr, (int)oC_Config.oC_ShopDB.nConnectTimeOut, oC_Config.oC_ShopDB.tAuthenMode);

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ISNULL(FTXshStaPrcStk,'') AS FTXshStaPrcStk");
                oSql.AppendLine("FROM TPSTSalHD with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '" + oSalePos.ptBchCode + "'");
                oSql.AppendLine("AND FTPosCode = '" + oSalePos.ptPosCode + "'");
                oSql.AppendLine("AND FTXshDocNo = '" + oSalePos.ptXihDocNo + "'");
                odtTmp = oDB.C_DAToExecuteQuery(tConnStr,oSql.ToString(),(int)oC_Config.oC_ShopDB.nCommandTimeOut);
                if (odtTmp == null)
                {
                    return false;
                }
                else
                {
                    if (odtTmp.Rows.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (odtTmp.Rows[0]["FTXshStaPrcStk"].ToString() == "1")
                        {
                            return true;
                        }
                        else
                        {
                            SqlParameter[] aPara = new SqlParameter[] {
                            new SqlParameter ("@ptBchCode",SqlDbType.Char,5){ Value = oSalePos.ptBchCode},
                            //new SqlParameter ("@ptComName",SqlDbType.Char,50){ Value = ptComName},
                            new SqlParameter ("@ptDocNo",SqlDbType.Char,30){ Value = oSalePos.ptXihDocNo},
                            new SqlParameter ("@ptWho",SqlDbType.Char,50){ Value = "MQReceivePrc"},
                            new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                            };
                            if (oDB.C_DATbExecuteStoreProcedure(tConnStr, "STP_DOCxSalePrcStk", ref aPara,(int)oC_Config.oC_ShopDB.nCommandTimeOut, "@FNResult"))
                            {
                                oSql = new StringBuilder();
                                oSql.AppendLine("UPDATE TPSTSalDT with(rowlock)");
                                oSql.AppendLine("SET FTXsdStaPrcStk='1'");
                                oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                oSql.AppendLine(",FTLastUpdBy = 'MQReceivePrc'");
                                oSql.AppendLine("WHERE FTBchCode = '" + oSalePos.ptBchCode + "'");
                                oSql.AppendLine("AND FTXshDocNo = '" + oSalePos.ptXihDocNo + "'");
                                oSql.AppendLine("AND ISNULL(FTXsdStaPdt,'') NOT IN('4','5')");
                                oDB.C_DATbExecuteNonQuery(tConnStr,oSql.ToString(),(int)oC_Config.oC_ShopDB.nCommandTimeOut, out nRowAffect);

                                oSql = new StringBuilder();
                                oSql.AppendLine("UPDATE TPSTSalHD with(rowlock)");
                                oSql.AppendLine("SET FTXshStaPrcStk='1'");
                                oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                                oSql.AppendLine(",FTLastUpdBy = 'MQReceivePrc'");
                                oSql.AppendLine("WHERE FTBchCode = '" + oSalePos.ptBchCode + "'");
                                oSql.AppendLine("AND FTPosCode = '" + oSalePos.ptPosCode + "'");
                                oSql.AppendLine("AND FTXshDocNo = '" + oSalePos.ptXihDocNo + "'");
                                oDB.C_DATbExecuteNonQuery(tConnStr, oSql.ToString(), (int)oC_Config.oC_ShopDB.nCommandTimeOut, out nRowAffect);

                                //*Arm 63-03-31  Check StaUseCentralized
                                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                                {
                                    //*Arm 62-11-19 
                                    cmlRcvDocApv oDocApv = new cmlRcvDocApv();
                                    oDocApv.ptBchCode = oSalePos.ptBchCode;
                                    oDocApv.ptDocNo = oSalePos.ptXihDocNo;
                                    oDocApv.ptDocType = "SALE";
                                    oDocApv.ptUser = "MQReceivePrc";
                                    oDocApv.ptConnStr = tConnStr;

                                    string tMsgStock = Newtonsoft.Json.JsonConvert.SerializeObject(oDocApv);
                                    cFunction.C_PRCxMQPublish("UPLOADSTKCRD", tMsgStock, out tErrMsg);
                                    cFunction.C_PRCxMQPublish("UPLOADSTKBAL", tMsgStock, out tErrMsg);

                                    //++++++++++++++++++++++++++++++
                                }

                                //var oFactory = new ConnectionFactory();
                                //oFactory.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                                //oFactory.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                                //oFactory.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                                //oFactory.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;

                                //using (var oConnection = oFactory.CreateConnection())
                                //{
                                //    using (var oChannal = oConnection.CreateModel())
                                //    {
                                //        oChannal.QueueDeclare(queue: "", durable: false, exclusive: false, autoDelete: false, arguments: null);
                                //        string tMsg = Newtonsoft.Json.JsonConvert.SerializeObject(oDocApv);
                                //        var oBody = Encoding.UTF8.GetBytes(tMsg);

                                //        oChannal.BasicPublish(exchange: "", routingKey: "", basicProperties: null, body: oBody);
                                //    }
                                //}

                                //oFactory = null;
                                ////++++++++++++++++++++
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Process copy sale transaction from shop to headquarters consolidate.
        /// </summary>
        /// 
        /// <param name="ptMessage">Data message from RabbitMQ.</param>
        /// 
        /// <returns>
        /// true: process success.
        /// false: process false.
        /// </returns>
        private bool C_PRCbConsolidate(string ptMessage)
        {
            DataTable oDbTblSchemaRC, oDbTblSchemaDT, oDbTblSchemaHD, oDbTblSHD, oDbTblSDT, oDbTblSRC;
            StringBuilder oSql;
            SqlParameter[] aoSqlParam;
            cDatabase oDatabase;
            cmlRcvConsolidate oRcvConsolidate;
            string tConStrCsd, tConStrShop, tSchemaRC, tSchemaDT, tSchemaHD;
            int nRowEff;
            bool bStaPrc;

            try
            {
                oDatabase = new cDatabase();

                // convert message Json to class model.
                oRcvConsolidate = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvConsolidate>(ptMessage);

                if (oRcvConsolidate == null)
                {
                    // convert message Json false.
                    return false;
                }

                // connection database consolidate.
                tConStrCsd = oDatabase.C_GETtConnectString(
                    oC_Config.oC_ConsolidateDB.tServer,
                    oC_Config.oC_ConsolidateDB.tUser,
                    oC_Config.oC_ConsolidateDB.tPassword,
                    oC_Config.oC_ConsolidateDB.tDatabase,
                    oC_Config.oC_ConsolidateDB.nConnectTimeOut.GetValueOrDefault(),
                    oC_Config.oC_ConsolidateDB.tAuthenMode);

                // connection data shop.
                tConStrShop = oDatabase.C_GETtConnectString4PrcConsolidate(
                    oRcvConsolidate.ptConnStr,
                    oC_Config.oC_ConsolidateDB.nConnectTimeOut.GetValueOrDefault(),
                    oC_Config.oC_ConsolidateDB.tAuthenMode);

                oSql = new StringBuilder();

                // get schema table destination RC, DT, HD.
                // table receive.
                oSql.AppendLine("EXEC STP_SALoGetSchemaTable @ptTblName = 'TPXTSalRC'");
                oDbTblSchemaRC = oDatabase.C_DAToExecuteQuery(
                    tConStrCsd, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());
                tSchemaRC = oDbTblSchemaRC.Rows[0][0].ToString()
                    .Replace(", ", ", SRC.").Replace("SRC.FTBchCode", "CMP.FTBchCode").Replace("FTCmpCode", "CMP.FTCmpCode");

                // table detail.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_SALoGetSchemaTable @ptTblName = 'TPXTSalDT'");
                oDbTblSchemaDT = oDatabase.C_DAToExecuteQuery(
                    tConStrCsd, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());
                tSchemaDT = oDbTblSchemaDT.Rows[0][0].ToString()
                    .Replace(", ", ", SDT.").Replace("FTCmpCode", "CMP.FTCmpCode");

                // table header.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_SALoGetSchemaTable @ptTblName = 'TPXTSalHD'");
                oDbTblSchemaHD = oDatabase.C_DAToExecuteQuery(
                    tConStrCsd, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());
                tSchemaHD = oDbTblSchemaHD.Rows[0][0].ToString()
                    .Replace(", ", ", SHD.").Replace("FTCmpCode", "CMP.FTCmpCode");

                // sale receive.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_SALoGetSaleRCByDocNo ");
                oSql.AppendLine("   @ptColumnRC = '" + tSchemaRC + "', ");
                oSql.AppendLine("   @ptDocNo = '" + oRcvConsolidate.ptDocNo + "'");
                oDbTblSRC = oDatabase.C_DAToExecuteQuery(
                    tConStrShop, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());

                // sale detail.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_SALoGetSaleDTByDocNo ");
                oSql.AppendLine("   @ptColumnDT = '" + tSchemaDT + "', ");
                oSql.AppendLine("   @ptDocNo = '" + oRcvConsolidate.ptDocNo + "'");
                oDbTblSDT = oDatabase.C_DAToExecuteQuery(
                    tConStrShop, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());

                // sale header.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_SALoGetSaleHDByDocNo ");
                oSql.AppendLine("   @ptColumnHD = '" + tSchemaHD + "', ");
                oSql.AppendLine("   @ptDocNo = '" + oRcvConsolidate.ptDocNo + "'");
                oDbTblSHD = oDatabase.C_DAToExecuteQuery(
                    tConStrShop, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());

                // create table temp.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_SALxCreateTempSale");
                oDatabase.C_DATbExecuteNonQuery(
                    tConStrCsd, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault(), out nRowEff);

                // clear data in table temp.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_SALxClearTempSaleByDocNo @ptDocNo = '" + oRcvConsolidate.ptDocNo + "'");
                oDatabase.C_DATbExecuteNonQuery(
                    tConStrCsd, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault(), out nRowEff);

                // bulk copy sale receive.
                bStaPrc = oDatabase.C_DATbBulkCopyTable(
                    tConStrCsd, "TTmpTPSTSalRC", oDbTblSRC, oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());

                if (!bStaPrc)
                {
                    // bulk sale reveive false.
                    return false;
                }

                // bulk copy sale detail.
                bStaPrc = oDatabase.C_DATbBulkCopyTable(
                    tConStrCsd, "TTmpTPSTSalDT", oDbTblSDT, oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());

                if (!bStaPrc)
                {
                    // bulk sale detail false.
                    return false;
                }

                // bulk copy sale header.
                bStaPrc = oDatabase.C_DATbBulkCopyTable(
                    tConStrCsd, "TTmpTPSTSalHD", oDbTblSHD, oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());

                if (!bStaPrc)
                {
                    // bulk sale header false.
                    return false;
                }

                // insert temp to consolidate.
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptColumnRC", SqlDbType.VarChar, 8000){ Value = oDbTblSchemaRC.Rows[0][0].ToString()},
                    new SqlParameter ("@ptColumnDT", SqlDbType.VarChar, 8000){ Value = oDbTblSchemaDT.Rows[0][0].ToString()},
                    new SqlParameter ("@ptColumnHD", SqlDbType.VarChar, 8000){ Value = oDbTblSchemaHD.Rows[0][0].ToString()},
                    new SqlParameter ("@ptDocNo", SqlDbType.VarChar, 50){ Value = oRcvConsolidate.ptDocNo},
                    new SqlParameter ("@rnErrNum", SqlDbType.Int){ Direction = ParameterDirection.Output},
                    new SqlParameter ("@rtErrMsg", SqlDbType.VarChar, 8000){ Direction = ParameterDirection.Output},
                };

                bStaPrc = oDatabase.C_DATbExecuteStoreProcedure(
                    tConStrCsd, "STP_SALnTempSaleToConsolidate", ref aoSqlParam, oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault());

                if (bStaPrc)
                {
                    // clear data in table temp.
                    oSql.Clear();
                    oSql.AppendLine("EXEC STP_SALxClearTempSaleByDocNo @ptDocNo = '" + oRcvConsolidate.ptDocNo + "'");
                    oDatabase.C_DATbExecuteNonQuery(
                        tConStrCsd, oSql.ToString(), oC_Config.oC_ConsolidateDB.nCommandTimeOut.GetValueOrDefault(), out nRowEff);

                    // process success.
                    return true;
                }
                else
                {
                    // process fasle.
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                oDbTblSchemaRC = null;
                oDbTblSchemaDT = null;
                oDbTblSchemaHD = null;
                oDbTblSHD = null;
                oDbTblSDT = null;
                oDbTblSRC = null;
                oSql = null;
                aoSqlParam = null;
                oDatabase = null;
                oRcvConsolidate = null;
                new cSP().SP_CLExMemory();

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        /// Process copy sale food court transaction from branch to headquarters.
        /// </summary>
        /// 
        /// <param name="ptMessage">Data message from RabbitMQ.</param>
        /// 
        /// <returns>
        /// true: process success.
        /// false: process false.
        /// </returns>
        private bool C_PRCbFCBch2HQSale(string ptMessage)
        {
            DataTable oDbTblSchema, oDbTblSale;
            StringBuilder oSql;
            SqlParameter[] aoSqlParam;
            cDatabase oDatabase;
            cmlRcvFCBch2HQSale oRcvFBBch2HQSale;
            string tConStrFCBch, tConStrFCHQ, tSchema ;
            int nRowEff;
            bool bStaPrc;

            try
            {
                // check config database food court branch.
                if (oC_Config.oC_BchFoodCourtDB == null)
                {
                    // config database food court branch not set.
                    return false;
                }

                oDatabase = new cDatabase();

                // convert message Json to class model.
                oRcvFBBch2HQSale = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRcvFCBch2HQSale>(ptMessage);

                if (oRcvFBBch2HQSale == null)
                {
                    // convert message Json false.
                    return false;
                }

                // connection database food court branch.
                tConStrFCBch = oDatabase.C_GETtConnectString(
                    oC_Config.oC_BchFoodCourtDB.tServer,
                    oC_Config.oC_BchFoodCourtDB.tUser,
                    oC_Config.oC_BchFoodCourtDB.tPassword,
                    oC_Config.oC_BchFoodCourtDB.tDatabase,
                    oC_Config.oC_BchFoodCourtDB.nConnectTimeOut.GetValueOrDefault(),
                    oC_Config.oC_BchFoodCourtDB.tAuthenMode);

                // connection database food court HQ.
                tConStrFCHQ = oDatabase.C_GETtConnectString(
                    oC_Config.oC_HQFoodCourtDB.tServer,
                    oC_Config.oC_HQFoodCourtDB.tUser,
                    oC_Config.oC_HQFoodCourtDB.tPassword,
                    oC_Config.oC_HQFoodCourtDB.tDatabase,
                    oC_Config.oC_HQFoodCourtDB.nConnectTimeOut.GetValueOrDefault(),
                    oC_Config.oC_HQFoodCourtDB.tAuthenMode);

                oSql = new StringBuilder();

                // get schema table destination.
                // table card history.
                oSql.AppendLine("EXEC STP_FCToGetSchemaTable @ptTblName = 'TFNTCrdHisBch'");
                oDbTblSchema = oDatabase.C_DAToExecuteQuery(
                    tConStrFCHQ, oSql.ToString(), oC_Config.oC_HQFoodCourtDB.nCommandTimeOut.GetValueOrDefault());
                tSchema = oDbTblSchema.Rows[0][0].ToString();

                // sale transaction.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_FCToGetTransaction ");
                oSql.AppendLine("   @ptColumn = '" + tSchema + "', ");
                oSql.AppendLine("   @ptBchCode = '" + oRcvFBBch2HQSale.ptBchCode + "',");
                oSql.AppendLine("   @ptPosCode = '" + oRcvFBBch2HQSale.ptPosCode + "',");
                oSql.AppendLine("   @ptDocNo = '" + oRcvFBBch2HQSale.ptDocNo + "'");
                oDbTblSale = oDatabase.C_DAToExecuteQuery(
                    tConStrFCBch, oSql.ToString(), oC_Config.oC_BchFoodCourtDB.nCommandTimeOut.GetValueOrDefault());

                // create table temp.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_FCTxCreateTempTransaction");
                oDatabase.C_DATbExecuteNonQuery(
                    tConStrFCHQ, oSql.ToString(), oC_Config.oC_HQFoodCourtDB.nCommandTimeOut.GetValueOrDefault(), out nRowEff);

                // clear data in table temp.
                oSql.Clear();
                oSql.AppendLine("EXEC STP_FCTxClearTempTransaction ");
                oSql.AppendLine("   @ptBchCode = '" + oRcvFBBch2HQSale.ptBchCode + "',");
                oSql.AppendLine("   @ptPosCode = '" + oRcvFBBch2HQSale.ptPosCode + "',");
                oSql.AppendLine("   @ptDocNo = '" + oRcvFBBch2HQSale.ptDocNo + "'");
                oDatabase.C_DATbExecuteNonQuery(
                    tConStrFCHQ, oSql.ToString(), oC_Config.oC_HQFoodCourtDB.nCommandTimeOut.GetValueOrDefault(), out nRowEff);

                // bulk copy sale transaction.
                bStaPrc = oDatabase.C_DATbBulkCopyTable(
                    tConStrFCHQ, "TTmpTFNTCrdSale", oDbTblSale, oC_Config.oC_HQFoodCourtDB.nCommandTimeOut.GetValueOrDefault());

                if (!bStaPrc)
                {
                    // bulk sale reveive false.
                    return false;
                }

                // insert temp to card history.
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptColumn", SqlDbType.VarChar, 8000){ Value = oDbTblSchema.Rows[0][0].ToString()},
                    new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = oRcvFBBch2HQSale.ptBchCode},
                    new SqlParameter ("@ptPosCode", SqlDbType.VarChar, 3){ Value = oRcvFBBch2HQSale.ptPosCode},
                    new SqlParameter ("@ptDocNo", SqlDbType.VarChar, 30){ Value = oRcvFBBch2HQSale.ptDocNo},
                    new SqlParameter ("@rnErrNum", SqlDbType.Int){ Direction = ParameterDirection.Output},
                    new SqlParameter ("@rtErrMsg", SqlDbType.VarChar, 8000){ Direction = ParameterDirection.Output},
                };

                bStaPrc = oDatabase.C_DATbExecuteStoreProcedure(
                    tConStrFCHQ, "STP_FCTnTempTransactionBCHToHQ", ref aoSqlParam, oC_Config.oC_HQFoodCourtDB.nCommandTimeOut.GetValueOrDefault());

                if (bStaPrc)
                {
                    // clear data in table temp.
                    oSql.Clear();
                    oSql.AppendLine("EXEC STP_FCTxClearTempTransaction ");
                    oSql.AppendLine("   @ptBchCode = '" + oRcvFBBch2HQSale.ptBchCode + "',");
                    oSql.AppendLine("   @ptPosCode = '" + oRcvFBBch2HQSale.ptPosCode + "',");
                    oSql.AppendLine("   @ptDocNo = '" + oRcvFBBch2HQSale.ptDocNo + "'");
                    oDatabase.C_DATbExecuteNonQuery(
                        tConStrFCHQ, oSql.ToString(), oC_Config.oC_HQFoodCourtDB.nCommandTimeOut.GetValueOrDefault(), out nRowEff);

                    // process success.
                    return true;
                }
                else
                {
                    // process fasle.
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                oDbTblSchema = null;
                oDbTblSale = null;
                oSql = null;
                aoSqlParam = null;
                oDatabase = null;
                oRcvFBBch2HQSale = null;
                new cSP().SP_CLExMemory();

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        public void C_PRCxReponseStatus(string ptMsg,string ptRounting,string ptQueue,string ptExchange)
        {
            ConnectionFactory oFactory;
            try
            {
                oFactory = new ConnectionFactory();
                oFactory.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                oFactory.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                oFactory.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                oFactory.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;

                using (var oConn = oFactory.CreateConnection())
                using (var oChannel = oConn.CreateModel())
                {
                    if (!string.IsNullOrEmpty(ptExchange))
                    {
                        oChannel.ExchangeDeclare(exchange:ptExchange, type: "direct");
                    }

                    oChannel.QueueDeclare(queue: ptMsg, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    string tMessage = ptMsg;
                    var body = Encoding.UTF8.GetBytes(tMessage);

                    oChannel.BasicPublish(exchange: ptExchange, routingKey: ptRounting, basicProperties: null, body: body);
                }
            }
            catch { }
            finally
            { }
        }
        //*Net 63-09-02
        public bool C_CHKbSQlServErr(string ptErr)
        {
            try
            {
                foreach (string tSQLErr in cVB.tVB_ErrContain.Split(','))
                {
                    if (ptErr.ToLower().Contains(tSQLErr))
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        //*Net 63-09-02
        public bool C_PRCxRestart(string ptMessage, out string ptErr)
        {
            cmlRcvDataHD oDataHD;
            try
            {
                oDataHD = JsonConvert.DeserializeObject<cmlRcvDataHD>(ptMessage);
                if (oDataHD.ptFunction == "Restart MQ")
                {
                    Console.Clear();
                    oC_MQConn.Close();
                    Task.Delay(3000).Wait();
                    //C_MQRxProcess();
                }
                else
                {
                    throw new Exception("Invalid Function");
                }
                ptErr = "";
                return true;
            }
            catch (Exception oEx)
            {
                ptErr = oEx.Message;
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCxRestart");
            }
            return false;
        }
    }
}
