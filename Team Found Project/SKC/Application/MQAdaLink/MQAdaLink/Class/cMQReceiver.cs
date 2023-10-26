using MQAdaLink.Class.Export;
using MQAdaLink.Class.Import;
using MQAdaLink.Model;
using MQAdaLink.Model.Receive;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQAdaLink.Class
{
    public class cMQReceiver
    {
        ConnectionFactory oFactory;
        IConnection oConn;
        IModel oChannel;
        EventingBasicConsumer oConsumer;
        cConfig oC_Config;
        List<string> aStr = new List<string>();
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
        /// *Arm 63-07-02
        /// </summary>
        public void C_PRCxMQProcess()
        {
            ConnectionFactory oFactory;
            ThreadStart oStart;
            Thread oTherad;
            IConnection oConn;
            IModel oChannel;
            cMS oMsg;
            string[] atQueue;
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
                    Console.WriteLine("");
                    Console.WriteLine("Server Name:     {0}", cVB.oVB_Config.tServerDB);
                    Console.WriteLine("DataBase Name:   {0}", oC_Config.tC_StaConnDB);
                    Console.WriteLine("========================================================================");
                    
                    oFactory = new ConnectionFactory();
                    oFactory.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                    oFactory.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                    oFactory.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                    oFactory.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;

                    atQueue = oC_Config.oC_RabbitMQ.tMQListQueue.Split(',');
                    foreach (string tQueue in atQueue)
                    {
                        if (string.Equals(tQueue, ""))
                        {
                            continue;
                        }
                        oConn = oFactory.CreateConnection();
                        oChannel = oConn.CreateModel();
                        oStart = () => C_PRCxMessage(oChannel, tQueue);
                        oTherad = new Thread(oStart);
                        oTherad.Name = tQueue;
                        oTherad.IsBackground = true;
                        oTherad.Start();
                    }
                    Thread.Sleep(500);
                    Console.ReadLine();
                }
                else
                {
                    new cLog().C_PRCxLog("C_PRCxMQProcess", oMsg.tMS_CfgLoadFalse + tMsgErr);
                    Console.WriteLine(oMsg.tMS_CfgLoadFalse + tMsgErr);
                    Console.ReadLine();
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("C_PRCxMQProcess", oEx.Message.ToString());
                Console.WriteLine(oEx.Message.ToString());
                Console.ReadLine();
            }
            finally
            {
                oFactory = null;
                oStart = null;
                oTherad = null;
                oConn = null;
                oChannel = null;
                oMsg = null;
                atQueue = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private void C_PRCxMessage(IModel poChannel, string ptQueue)
        {
            EventingBasicConsumer oConsumer;
            string tMessage, tFmtDateTime, tQueueID;
            byte[] aoBody;
            bool bStaConsume;
            bool bPrc = true;
            string tErrMsg = "";

            try
            {
                tFmtDateTime = "yyyy-MM-dd HH:mm:ss";
                bStaConsume = false;
                while (bStaConsume == false)
                {
                    try
                    {
                        // Declare
                        switch (ptQueue)
                        {
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

                            switch (ptQueue)
                            {
                                case "LK_QPdtStkTnf": //*Arm 63-08-24 ใบโอนสินค้าระหว่าคลัง
                                    try
                                    {
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Process stock transfer Start.");
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Process stock transfer Start...");

                                        cmlRcvData oRcv = JsonConvert.DeserializeObject<cmlRcvData>(tMessage);
                                        cStockTransfer oStock = new cStockTransfer();
                                        tErrMsg = "";

                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Start C_PRCbExportSale");
                                        // Call C_PRCbExportSale
                                        bPrc = oStock.C_PRCbPdtStockTransfer(oRcv, ref tErrMsg);
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "End C_PRCbExportSale");

                                        if (bPrc)
                                        {
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Process stock transfer Success.");
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Process stock transfer Success...");

                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Delete QueueID[" + tQueueID + "]");
                                        }
                                        else
                                        {
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QPdtStkTnf] / QueueID[" + tQueueID + "] : " + "Process stock transfer false / " + tErrMsg);
                                            if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + "Receive Queue Name[LK_QPdtStkTnf] / QueueID[" + tQueueID + "] : " + tErrMsg);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QPdtStkTnf] / QueueID[" + tQueueID + "] : " + "Process stock transfer false...");

                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Process stock transfer Error : " + oEx.Message.ToString());
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Process stock transfer Error : " + oEx.Message.ToString()); //*Arm 63-08-27
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QPdtStkTnf]/QueueID[" + tQueueID + "] : " + "Process Estock transfer Error : " + oEx.Message.ToString());
                                    }
                                    Thread.Sleep(500);
                                    break;

                                case "LK_QImportMaster":
                                    try
                                    {
                                        new cLog().C_PRCxLog("C_PRCxMessage", DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " LK_QImportMaster Process Start...");
                                        cmlRabMQData oRabMQData = JsonConvert.DeserializeObject<cmlRabMQData>(tMessage);
                                        string tDate = "";
                                        string tBchCode = "";
                                        List<string> aFileXml = new List<string>();
                                        cVB.tVB_Branch = new cSP().C_GETtBranchCode();
                                        // รอมีการส่งค่า Date To Form
                                        if (C_CHKbDataFormTo(oRabMQData, ref tDate, ref tBchCode))
                                        {
                                            try
                                            {
                                                switch (oRabMQData.ptFunction)
                                                {
                                                    
                                                    case "STKTF":
                                                        C_PRCxStockTranster(oRabMQData.ptData.ptFilter);
                                                        break;
                                                    case "MASTER":
                                                        Console.WriteLine("");                                                        
                                                        Console.WriteLine("Process LK_QImportMaster Queue Master  ===> Start");
                                                        C_IMPxMaster(aFileXml);
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process LK_QImportMaster Queue Master  ===> End");
                                                        System.Threading.Thread.Sleep(5000);
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process Send Email Master ....   ===> Start");
                                                        new cSendMail().C_PRCxSendMail(cVB.tVB_MasBackUP + ".zip", "1");
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process Send Email Master ....   ===> End");
                                                        aFileXml = new List<string>();
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process LK_QImportMaster Queue Price   ===> Start");
                                                        C_IMPxPrice(aFileXml);
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process LK_QImportMaster Queue Price   ===> End");
                                                        
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process ADJUSTPRICE ....               ===> Start");
                                                        ConnectionFactory oFactoryADJ;
                                                        oFactoryADJ = new ConnectionFactory();
                                                        oFactoryADJ.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                                                        oFactoryADJ.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                                                        oFactoryADJ.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                                                        oFactoryADJ.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;

                                                        if (C_PRCxADJ(cVB.oTblAdj, oFactoryADJ, "ADJUSTPRICE"))
                                                        {

                                                            aStr = new List<string>();

                                                            if (cVB.oTblAdj.Rows.Count > 0)
                                                            {
                                                                foreach (DataRow oRow in cVB.oTblAdj.Rows)
                                                                {
                                                                    string tDocNO = string.Format("RESAJP_{0}_{1}", Convert.ToString(oRow["FTXphDocNo"]), "AdaLink");
                                                                    aStr.Add(tDocNO);
                                                                }
                                                            }
                                                            C_MQRxPrcApp(oFactoryADJ, poChannel, aStr);
                                                        }
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process ADJUSTPRICE ....               ===> End");
                                                        //if (oRabMQData.ptData.ptFilter == "1")
                                                        //{
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process Send Email Price ....   ===> Start");
                                                        System.Threading.Thread.Sleep(5000);
                                                        new cSendMail().C_PRCxSendMail(cVB.tVB_MasBackUP + ".zip", "2");
                                                        Console.WriteLine("Process Send Email Price ....   ===> End");
                                                        //}
                                                        break;
                                                    case "EMPLO":
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process LK_QImportMaster Queue Employee  => Start");
                                                        C_IMPxEmployee(aFileXml);
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process LK_QImportMaster Queue Employee  => End");
                                                        //if (oRabMQData.ptData.ptFilter == "1")
                                                        //{
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process Send Email Employee ....       ===> Start");
                                                        System.Threading.Thread.Sleep(5000);
                                                        new cSendMail().C_PRCxSendMail(cVB.tVB_MasBackUP + ".zip", "3");
                                                        Console.WriteLine("Process Send Email Employee ....       ===> End");
                                                        //}
                                                        break;
                                                    case "PRICE":
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process LK_QImportMaster Queue Price   ===> Start");
                                                        C_IMPxPrice(aFileXml);
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process LK_QImportMaster Queue Price   ===> End");

                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process ADJUSTPRICE ....               ===> Start");
                                                        ConnectionFactory oFactoryADJ2;
                                                        oFactoryADJ2 = new ConnectionFactory();
                                                        oFactoryADJ2.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                                                        oFactoryADJ2.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                                                        oFactoryADJ2.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                                                        oFactoryADJ2.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;

                                                        if (C_PRCxADJ(cVB.oTblAdj, oFactoryADJ2, "ADJUSTPRICE"))
                                                        {                                                           

                                                            aStr = new List<string>();

                                                            if (cVB.oTblAdj.Rows.Count > 0)
                                                            {
                                                                foreach (DataRow oRow in cVB.oTblAdj.Rows)
                                                                {
                                                                    string tDocNO = string.Format("RESAJP_{0}_{1}", Convert.ToString(oRow["FTXphDocNo"]), "AdaLink");
                                                                    aStr.Add(tDocNO);
                                                                }
                                                            }                                                            
                                                            C_MQRxPrcApp(oFactoryADJ2, poChannel, aStr);                                                           
                                                        }
                                                        Console.WriteLine("");
                                                        Console.WriteLine("Process ADJUSTPRICE ....               ===> End");
                                                        break;
                                                    default:
                                                        C_IMPxALL(aFileXml);
                                                        ConnectionFactory oFactoryADJAll;
                                                        oFactoryADJAll = new ConnectionFactory();
                                                        oFactoryADJAll.HostName = oC_Config.oC_RabbitMQ.tMQHostName;
                                                        oFactoryADJAll.UserName = oC_Config.oC_RabbitMQ.tMQUserName;
                                                        oFactoryADJAll.Password = oC_Config.oC_RabbitMQ.tMQPassword;
                                                        oFactoryADJAll.VirtualHost = oC_Config.oC_RabbitMQ.tMQVirtualHost;
                                                        if (C_PRCxADJ(cVB.oTblAdj, oFactoryADJAll, "ADJUSTPRICE"))
                                                        {

                                                            aStr = new List<string>();

                                                            if (cVB.oTblAdj.Rows.Count > 0)
                                                            {
                                                                foreach (DataRow oRow in cVB.oTblAdj.Rows)
                                                                {
                                                                    string tDocNO = string.Format("RESAJP_{0}_{1}", Convert.ToString(oRow["FTXphDocNo"]), "AdaLink");
                                                                    aStr.Add(tDocNO);
                                                                }
                                                            }
                                                            C_MQRxPrcApp(oFactoryADJAll, poChannel, aStr);
                                                        }
                                                        break;
                                                }
                                                poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            }
                                            catch (Exception oEx) { new cLog().C_PRCxLog("C_PRCxMQProcess", oEx.Message.ToString()); }
                                            finally
                                            {
                                                new cSP().SP_CLExMemory();
                                            }
                                        }
                                    }
                                    catch(Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        new cLog().C_PRCxLog("C_PRCxMessage", DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " LK_QImportMaster Process Error :" + oEx.Message.ToString());
                                    }
                                    Thread.Sleep(500);
                                    break;

                                case "LK_QSale2Vender":
                                    try
                                    {

                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Start.", "Vender");
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Start...");

                                        cmlRcvData oRcv = JsonConvert.DeserializeObject<cmlRcvData>(tMessage);
                                        cSale oSale = new cSale();
                                        tErrMsg = "";
                                        
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Start C_PRCbExportSale", "Vender");
                                        // Call C_PRCbExportSale
                                        bPrc = oSale.C_PRCbExportSale(oRcv, ref tErrMsg);
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "End C_PRCbExportSale", "Vender");

                                        if (bPrc)
                                        {
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Success.", "Vender");
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Success...");

                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Delete QueueID[" + tQueueID + "]", "Vender");
                                        }
                                        else
                                        {
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender] / QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS false / " + tErrMsg, "Vender");
                                            if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + "Receive Queue Name[LK_QSale2Vender] / QueueID[" + tQueueID + "] : " + tErrMsg);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Vender] / QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS false...");

                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Error : " + oEx.Message.ToString());
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Error : " + oEx.Message.ToString(), "Vender");
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Error : " + oEx.Message.ToString());
                                    }
                                    Thread.Sleep(500);
                                    break;
                                    
                                case "LK_QSale2Pending":
                                    try
                                    {
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Start...", "Pending");
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Start...");

                                        cPendingExp oPendingExp = new cPendingExp();
                                        tErrMsg = "";
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Start C_PRCbPendingExp.", "Pending");
                                        //Call C_PRCbPendingExp
                                        bPrc = oPendingExp.C_PRCbPendingExp(tMessage, ref tErrMsg);
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "End C_PRCbPendingExp.", "Pending");

                                        if (bPrc)
                                        {
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Success...", "Pending");
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Success...");

                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Delete QueueID[" + tQueueID + "]", "Pending");
                                            
                                        }
                                        else
                                        {
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending false / " + tErrMsg, "Pending");

                                            if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + tErrMsg);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending false...");
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Error : " + oEx.Message.ToString());
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Error : " + oEx.Message.ToString(), "Pending"); //*Arm 63-08-27
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Error : " + oEx.Message.ToString());
                                    }
                                    Thread.Sleep(500);
                                    break;

                                case "LK_QSale2Mail":
                                    try
                                    {
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Start...", "SendMail");
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Start...");

                                        cSendMail oSndMail = new cSendMail();
                                        tErrMsg = "";
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Start C_PRCbSendMail", "SendMail");

                                        //Call C_PRCbSendMail
                                        bPrc = oSndMail.C_PRCbSendMail(ref tErrMsg);

                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "End C_PRCbSendMail", "SendMail");

                                        if (bPrc)
                                        {
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Success...", "SendMail");
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Success...");

                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Delete QueueID[" + tQueueID + "]", "SendMail");
                                        }
                                        else
                                        {
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail false / " + tErrMsg, "SendMail");

                                            if (!string.IsNullOrEmpty(tErrMsg)) Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + tErrMsg);
                                            Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail false...");

                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Error :" + oEx.Message.ToString());
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Error :" + oEx.Message.ToString(), "SendMail"); //*Arm 63-08-27
                                        Console.WriteLine(DateTime.Now.ToString(tFmtDateTime) + " Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Error :" + oEx.Message.ToString());

                                    }
                                    Thread.Sleep(500);
                                    break;
                            }
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
                        
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_PRCxMessage", oEx.Message.ToString());
                Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + ptQueue + " : Exception");
                Console.ReadLine();
            }
        }
        

        public bool C_CHKbDataFormTo(cmlRabMQData poData,ref string ptDate,ref string ptBchCode)
        {
            string tDateFrm = "", tDateTo="", tDate = "", tBchCode="";
            bool bStatus = false;
            try
            {
                if (!string.IsNullOrEmpty(poData.ptData.ptDateFrm) && !string.IsNullOrEmpty(poData.ptData.ptDateTo))
                {                    
                    tDateFrm = DateTime.ParseExact(poData.ptData.ptDateFrm, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    tDateTo = DateTime.ParseExact(poData.ptData.ptDateTo, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    tDate = string.Format("BETWEEN '{0}' AND '{1}'", "" + tDateFrm + "", "" + tDateTo + "");
                    tBchCode = poData.ptData.ptFilter;
                    bStatus = true;
                    ptDate = tDate;
                    ptBchCode = tBchCode;

                }
                else
                {
                    new cLog().C_PRCxLog("C_PRCxMQData","DateFrom : "+ tDateFrm + 
                        " DateTo : "+ tDateTo +
                        " DataWhere : " + tDate +
                        " Branch : " + tBchCode);
                    bStatus = true;
                }                
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_PRCxMQData",oEx.Message.ToString()); }
            finally
            {
                tDateFrm = "";
                tDateTo = "";
                tDate = "";
                tBchCode = "";
            }

            return bStatus;

        }

        public void C_IMPxMaster(List<string> paFileXml)
        {            
            try
            {
                cVB.tVB_MasBackUP = null;
                cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                Console.WriteLine("");
                Console.WriteLine("Download File XML Master  =====> Start");
                new cLog().C_PRCxLogMonitor("cMQReceiver", "C_SAVxDownloadXML : Download file start."); //*Arm 63-08-21
                new cSP().C_SAVxDownloadXML(paFileXml, "Master");
                new cLog().C_PRCxLogMonitor("cMQReceiver", "C_SAVxDownloadXML : Download file end."); //*Arm 63-08-21
                Console.WriteLine("Download File XML Master  =====> End");
                if (paFileXml.Count > 0)
                {
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : if have file."); //*Arm 63-08-21
                    //cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : CreateDirectory/ "+ cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : C_PRCxMaster start."); //*Arm 63-08-21
                    new cMaster().C_PRCxMaster(paFileXml);
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : C_PRCxMaster end."); //*Arm 63-08-21
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : if file not found."); //*Arm 63-08-21

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed");

                    //เอาข้อมูลจาก Data Table ลงไฟล์
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : C_PRCxDataTableToFile start."); //*Arm 63-08-21
                    new cSP().C_PRCxDataTableToFile(null, "MASTER(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error");
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : C_PRCxDataTableToFile end."); //*Arm 63-08-21

                    //เก็บ backup
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : C_PRCxBackUPFile start."); //*Arm 63-08-21
                    new cSP().C_PRCxBackUPFile(cVB.tVB_MasBackUP);
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : C_PRCxBackUPFile end."); //*Arm 63-08-21

                    Console.WriteLine("");
                    Console.WriteLine("Process Insert Log History          ====> Start");

                    //เก็บ history
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : C_SAVxHistory start."); //*Arm 63-08-21
                    new cSP().C_SAVxHistory("1", "ข้อมูลสินค้า", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2");
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxMaster : C_SAVxHistory start."); //*Arm 63-08-21

                    Console.WriteLine("Process Insert Log History          ====> End");
                    Console.WriteLine("File XML Not Found ........");
                }
                
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("cMQReceiver","C_IMPxMaster : Error/"+oEx.Message.ToString());
                new cLog().C_PRCxLog("cMQReceiver", "C_IMPxMaster : Error/" + oEx.Message.ToString());
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        public void C_IMPxPrice(List<string> paFileXml)
        {
            try
            {
                
                cVB.tVB_MasBackUP = null;
                cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                Console.WriteLine("");
                Console.WriteLine("Download File XML Price  =====> Start");
                new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : Download file start."); //*Arm 63-08-21
                new cSP().C_SAVxDownloadXML(paFileXml, "Price");
                new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : Download file end."); //*Arm 63-08-21
                Console.WriteLine("Download File XML Price  =====> End");
                if (paFileXml.Count > 0)
                {
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : if have file."); //*Arm 63-08-21
                    //cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : C_PRCxPrice start."); //*Arm 63-08-21
                    new cPrice().C_PRCxPrice(paFileXml);
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : C_PRCxPrice end."); //*Arm 63-08-21
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : if file not found."); //*Arm 63-08-21

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed");

                    // เอาข้อมูลจาก Data Table ลงไฟล์
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : C_PRCxDataTableToFile start."); //*Arm 63-08-21
                    new cSP().C_PRCxDataTableToFile(null, "PRICE(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error");
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : C_PRCxDataTableToFile end."); //*Arm 63-08-21

                    //back up
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : C_PRCxBackUPFile start."); //*Arm 63-08-21
                    new cSP().C_PRCxBackUPFile(cVB.tVB_MasBackUP);
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : C_PRCxBackUPFile end."); //*Arm 63-08-21

                    Console.WriteLine("");
                    Console.WriteLine("Process Insert Log History          ====> Start");

                    //บันทึกลง logHis
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : C_SAVxHistory start."); //*Arm 63-08-21
                    new cSP().C_SAVxHistory("1", "ใบปรับราคา", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2");
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : C_SAVxHistory end."); //*Arm 63-08-21

                    Console.WriteLine("Process Insert Log History          ====> End");
                    Console.WriteLine("File XML Not Found ........");
                }
                    
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cMQReceiver", "C_IMPxPrice : Error/"+ oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxPrice : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        public void C_IMPxEmployee(List<string> paFileXml)
        {
            try
            {
                Console.WriteLine("");
                Console.WriteLine("Download File XML Employee  =====> Start");
                cVB.tVB_MasBackUP = null;
                cVB.tVB_MasBackUP = DateTime.Now.ToString("yyyyMMddHHmmss");
                new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : Download file start."); //*Arm 63-08-21
                new cSP().C_SAVxDownloadXML(paFileXml, "Employee");
                new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : Download file end."); //*Arm 63-08-21
                Console.WriteLine("Download File XML Employee  =====> End");

                if (paFileXml.Count > 0)
                {
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : if have file."); //*Arm 63-08-21

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : C_PRCxEmployee start."); //*Arm 63-08-21
                    new cEmployee().C_PRCxEmployee(paFileXml);
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : C_PRCxEmployee end."); //*Arm 63-08-21
                }
                else
                {
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : if file not found."); //*Arm 63-08-21

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Success");

                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : CreateDirectory/ " + cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed"); //*Arm 63-08-21
                    Directory.CreateDirectory(cVB.tVB_PathIN + cVB.tVB_MasBackUP + @"\Failed");

                    //เอาข้อมูลจาก Data Table ลงไฟล์
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : C_PRCxDataTableToFile start."); //*Arm 63-08-21
                    new cSP().C_PRCxDataTableToFile(null, "EMPLO(Failed)-" + DateTime.Now.ToString("yyyyMMdd"), "Error");
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : C_PRCxDataTableToFile end."); //*Arm 63-08-21

                    //Back up
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : C_PRCxBackUPFile start."); //*Arm 63-08-21
                    new cSP().C_PRCxBackUPFile(cVB.tVB_MasBackUP);
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : C_PRCxBackUPFile end."); //*Arm 63-08-21

                    Console.WriteLine("");
                    Console.WriteLine("Process Insert Log History          ====> Start");
                    //บันทึก logHis
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : C_SAVxHistory start."); //*Arm 63-08-21
                    new cSP().C_SAVxHistory("1", "ข้อมูลพนักงาน", cVB.tVB_MasBackUP + ".zip", "2", 0, 0, "2");
                    new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : C_SAVxHistory end."); //*Arm 63-08-21

                    Console.WriteLine("Process Insert Log History          ====> End");

                    Console.WriteLine("File XML Not Found ........");
                }
                    
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("cMQReceiver", "C_IMPxEmployee : Error/"+ oEx.Message.ToString());
                new cLog().C_PRCxLogMonitor("cMQReceiver", "C_IMPxEmployee : Error/" + oEx.Message.ToString()); //*Arm 63-08-27
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        public void C_PRCxStockTranster(string ptData)
        {
            try
            {
                //new cStockTransfer().C_PRCxStockTransfer(ptData);
            }
            catch (Exception oEx) { }
        }
        public void C_IMPxALL(List<string> paFileXml)
        {
            List<string> aFileMaster = new List<string>();
            List<string> aFileEmplo = new List<string>();
            List<string> aFilePrice = new List<string>();
            try
            {                
                new cSP().C_SAVxDownloadXML(paFileXml, "");
                foreach (var oFile in paFileXml)
                {
                    string[] tNameSplit = null;
                    tNameSplit = oFile.Split('_');
                    switch (tNameSplit[0].Trim())
                    {
                        case "MASTER":
                            aFileMaster.Add(oFile);
                            break;
                        case "EMPLO":
                            aFileEmplo.Add(oFile);
                            break;
                        case "PRICE":
                            aFilePrice.Add(oFile);
                            break;
                    }                   
                }
                if(aFileMaster.Count > 0)
                {
                    new cMaster().C_PRCxMaster(aFileMaster);
                }
                if (aFileEmplo.Count > 0)
                {
                    new cEmployee().C_PRCxEmployee(aFileEmplo);
                }
                if (aFilePrice.Count > 0)
                {
                    new cPrice().C_PRCxPrice(aFilePrice);
                }
            }
            catch(Exception oEx) { new cLog().C_PRCxLog("C_IMPxALL", oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }

        public bool C_PRCxADJ(DataTable poTblDocNOHD, ConnectionFactory poFactory, string ptQueue)
        {
            try
            {

                foreach (DataRow oRow in poTblDocNOHD.Rows)
                {
                    cmlRequestAdj oC_ResData = new cmlRequestAdj();
                    oC_ResData.ptBchCode = Convert.ToString(oRow["FTBchCode"]);
                    oC_ResData.ptDocNo = Convert.ToString(oRow["FTXphDocNo"]);
                    oC_ResData.ptDocType = Convert.ToString(oRow["FTXphDocType"]);
                    oC_ResData.ptUser = "AdaLink";
                    oC_ResData.ptConnStr = cVB.tVB_ConnMQ;
                    
                    string tJson = JsonConvert.SerializeObject(oC_ResData, Formatting.Indented);

                    using (var oConnection = poFactory.CreateConnection())
                    using (var oChannel = oConnection.CreateModel())
                    {
                        oChannel.QueueDeclare(queue: ptQueue,
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var oBody = Encoding.UTF8.GetBytes(tJson);

                        oChannel.BasicPublish(exchange: "",
                                             routingKey: ptQueue,
                                             basicProperties: null,
                                             body: oBody);
                    }
                }
                return true;
            }
            catch (Exception oEx)
            {
                return false;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void C_MQRxPrcApp(ConnectionFactory poFactory, IModel poChannel, List<string> paValue)
        {
            EventingBasicConsumer oConsumer;
            byte[] oBody;
            string tMes = "";
            IConnection oConn;
            IModel oChannel;
            string tMsgErr, tPath;
            
            try
            {
                
                for (int nRow = 0; nRow < paValue.Count; nRow++)
                {
                    System.Threading.Thread.Sleep(5000);
                    string[] aWords = null;
                    using (IConnection oCon = poFactory.CreateConnection())
                    {
                        using (IModel oChannels = oCon.CreateModel())
                        {
                            oChannels.QueueDeclare(paValue[nRow].Trim(), true, false, false, null);
                            var oConsumers = new EventingBasicConsumer(oChannels);
                            

                            BasicGetResult oResult = oChannels.BasicGet(paValue[nRow].Trim(), true);
                            if (oResult != null)
                            {
                                string tData =
                                Encoding.UTF8.GetString(oResult.Body);
                                if (tData == "10")
                                {
                                    aWords = paValue[nRow].Trim().Split('_');
                                    //oC_Logfile.C_SAVxHistory("ImpAdj", "1", "", "Approve DOCNO: " + aWords[1].Trim() + " Success");
                                    //paValue.Remove(paValue[nRow].Trim());
                                    oChannels.QueueDelete(paValue[nRow].Trim());
                                }
                            }
                            

                        }
                    }
                    

                }

               
            }
            catch (Exception oEx)
            {
                
            }
        }
    }
}
