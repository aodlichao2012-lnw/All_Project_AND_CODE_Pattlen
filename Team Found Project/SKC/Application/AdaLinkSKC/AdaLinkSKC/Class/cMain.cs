using AdaLinkSKC.Class.Export;
using AdaLinkSKC.Class.Import;
using AdaLinkSKC.Model;
using AdaLinkSKC.Model.Config;
using AdaLinkSKC.Model.Receive;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class
{
    public class cMain
    {
        ConnectionFactory oFactory;
        IConnection oConn;
        IModel oChannel;
        EventingBasicConsumer oConsumer;
        cConfig oC_Config;
        List<string> aStr = new List<string>();
        //ThreadStart oStart;
        //Thread oTherad;
        public cMain()
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

                //(Defualt) log App
                new cLog().C_PRCxLog("C_PRCxMQProcess", "======================= Start Program AdaLink ==========================");
                
                if (oC_Config.C_CFGbLoadConfig(out tMsgErr))
                {
                    //(Defualt) log App
                    new cLog().C_PRCxLog("C_PRCxMQProcess", "========================================================================");
                    new cLog().C_PRCxLog("C_PRCxMQProcess", "Host name:       " + oC_Config.oC_RabbitMQ.tMQHostName);
                    new cLog().C_PRCxLog("C_PRCxMQProcess", "User name:       " + oC_Config.oC_RabbitMQ.tMQUserName);
                    new cLog().C_PRCxLog("C_PRCxMQProcess", "Virtual host:    " + oC_Config.oC_RabbitMQ.tMQVirtualHost);
                    new cLog().C_PRCxLog("C_PRCxMQProcess", "========================================================================");
                    
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
                        
                        //(Defualt) log App
                        new cLog().C_PRCxLog("C_PRCxMQProcess", "Consumer Queue Name : "+ tQueue + " Start");
                        
                        oStart = () => C_PRCxMessage(oChannel, tQueue);
                        oTherad = new Thread(oStart);
                        oTherad.Name = tQueue;
                        oTherad.IsBackground = true;
                        oTherad.Start();
                    }
                    Thread.Sleep(500);
                }
                else
                {
                    new cLog().C_PRCxLog("C_PRCxMQProcess", oMsg.tMS_CfgLoadFalse + tMsgErr);
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_PRCxLog("C_PRCxMQProcess", oEx.Message.ToString());
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
                                case "LK_QImportMaster":
                                    try
                                    {
                                        new cLog().C_PRCxLog("C_PRCxMessage", DateTime.Now.ToString(tFmtDateTime) + " [" + tQueueID + "]" + " LK_QImportMaster Process Start...");
                                        cmlRabMQData oRabMQData = JsonConvert.DeserializeObject<cmlRabMQData>(tMessage);
                                        string tDate = "";
                                        string tBchCode = "";
                                        List<string> aFileXml = new List<string>();
                                        //cVB.tVB_Branch = new cSP().C_GETtBranchCode();
                                        cVB.tVB_Branch = cVB.tVB_BchCode; //*Arm 63-07-06
                                        // รอมีการส่งค่า Date To Form
                                        if (C_CHKbDataFormTo(oRabMQData, ref tDate, ref tBchCode))
                                        {
                                            try
                                            {
                                                switch (oRabMQData.ptFunction)
                                                {
                                                    case "MASTER":
                                                        C_IMPxMaster(aFileXml);
                                                        break;
                                                    case "EMPLO":
                                                        C_IMPxEmployee(aFileXml);
                                                        break;
                                                    case "PRICE":
                                                        C_IMPxPrice(aFileXml);
                                                        if (C_PRCxADJ(cVB.oTblAdj, oFactory, "ADJUSTPRICE"))
                                                        {

                                                            aStr = new List<string>();

                                                            if (cVB.oTblAdj.Rows.Count > 0)
                                                            {
                                                                foreach (DataRow oRow in cVB.oTblAdj.Rows)
                                                                {
                                                                    string tDocNO = string.Format("RESAJP_{0}_{1}", Convert.ToString(oRow["FTXphDocNo"]), "Interface");
                                                                    aStr.Add(tDocNO);
                                                                }
                                                            }
                                                            C_MQRxPrcApp(oFactory, oChannel, aStr);
                                                        }
                                                        break;
                                                    default:
                                                        C_IMPxALL(aFileXml);
                                                        if (C_PRCxADJ(cVB.oTblAdj, oFactory, "ADJUSTPRICE"))
                                                        {

                                                            aStr = new List<string>();

                                                            if (cVB.oTblAdj.Rows.Count > 0)
                                                            {
                                                                foreach (DataRow oRow in cVB.oTblAdj.Rows)
                                                                {
                                                                    string tDocNO = string.Format("RESAJP_{0}_{1}", Convert.ToString(oRow["FTXphDocNo"]), "Interface");
                                                                    aStr.Add(tDocNO);
                                                                }
                                                            }
                                                            C_MQRxPrcApp(oFactory, oChannel, aStr);
                                                        }
                                                        break;
                                                }
                                                oChannel.BasicAck(oEevntArgs.DeliveryTag, false);
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
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Start.");

                                        cmlRcvData oRcv = JsonConvert.DeserializeObject<cmlRcvData>(tMessage);
                                        cSale oSale = new cSale();
                                        tErrMsg = "";
                                        
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Start C_PRCbExportSale");
                                        
                                        // Call C_PRCbExportSale
                                        bPrc = oSale.C_PRCbExportSale(oRcv, ref tErrMsg);
                                        
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "End C_PRCbExportSale");

                                        if (bPrc)
                                        {
                                            
                                            //(Defualt) log App
                                            new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Success.");
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Delete QueueID[" + tQueueID + "]");

                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //(Defualt) log App
                                            new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender] / QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS false / " + tErrMsg);
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        
                                        //(Defualt) log App
                                        new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Vender]/QueueID[" + tQueueID + "] : " + "Process Export Sale to KADS Error : " + oEx.Message.ToString());
                                    }
                                    Thread.Sleep(500);
                                    break;
                                    
                                case "LK_QSale2Pending":
                                    try
                                    {
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Start.");
                                        
                                        cPendingExp oPendingExp = new cPendingExp();
                                        tErrMsg = "";

                                        //// Therad Pending Re-send
                                        //oStart = () => oPendingExp.C_PRCbPendingExp(tMessage);
                                        //oTheradPending = new Thread(oStart);
                                        //oTheradPending.IsBackground = true;
                                        //oTheradPending.Start();

                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Start C_PRCbPendingExp.");

                                        //Call C_PRCbPendingExp
                                        bPrc = oPendingExp.C_PRCbPendingExp(tMessage, ref tErrMsg);
                                        
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "End C_PRCbPendingExp.");
                                        

                                        if (bPrc)
                                        {
                                            
                                            //(Defualt) log App
                                            new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Success.");
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Delete QueueID[" + tQueueID + "]");

                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //(Defualt) log App
                                            new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending false / " + tErrMsg);
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        //(Defualt) log App
                                        new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Pending]/QueueID[" + tQueueID + "] : " + "Process Pending Re-Sending Error : " + oEx.Message.ToString());
                                    }
                                    Thread.Sleep(500);
                                    break;

                                case "LK_QSale2Mail":
                                    try
                                    {
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Start.");
                                        
                                        cSendMail oSndMail = new cSendMail();
                                        tErrMsg = "";
                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Start C_PRCbSendMail");

                                        //Call C_PRCbSendMail
                                        bPrc = oSndMail.C_PRCbSendMail(ref tErrMsg);

                                        //log monitor
                                        new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "End C_PRCbSendMail");

                                        if (bPrc)
                                        {
                                            //(Defualt) log App
                                            new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Success...");
                                            new cLog().C_PRCxLogMonitor("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Delete QueueID[" + tQueueID + "]");

                                            poChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                                        }
                                        else
                                        {
                                            //(Defualt) log App
                                            new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail false / " + tErrMsg);
                                            
                                        }
                                    }
                                    catch (Exception oEx)
                                    {
                                        // ต้องเขียน log ลง text file ด้วย
                                        //(Defualt) log App
                                        new cLog().C_PRCxLog("C_PRCxMessage", "Receive Queue Name[LK_QSale2Mail]/QueueID[" + tQueueID + "] : " + "Process Send E-mail Error :" + oEx.Message.ToString());
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
                        //new cLog().C_PRCxLog("C_PRCxMessage", oEx.Message.ToString());
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_PRCxLog("C_PRCxMessage", oEx.Message.ToString());
            }
        }

        public void C_PRCxMQProcessXXX()
        {
            byte[] aBody;
            string tMessage = "";
            cmlRabMQData oRabMQData = new cmlRabMQData();
            string tDate = "", tBchCode = "";
            List<string> aFileXml = new List<string>();
            try
            {
                oC_Config.C_GETxConfig();
                oFactory = new ConnectionFactory();
                oFactory.HostName = cVB.tVB_MQHost;
                oFactory.UserName = cVB.tVB_MQUser;
                oFactory.Password = cVB.tVB_MQPass;
                oFactory.VirtualHost = cVB.tVB_MQVB;
                oFactory.Port = cVB.nVB_MQPort;

                oConn = oFactory.CreateConnection();
                oChannel = oConn.CreateModel();                
                oChannel.QueueDeclare(queue: cVB.tVB_MQQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                oConsumer = new EventingBasicConsumer(oChannel);
                oConsumer.Received += (oModel, oEevntArgs) =>
                {
                    aBody = oEevntArgs.Body;
                    tMessage = Encoding.UTF8.GetString(aBody);
                    oRabMQData = Newtonsoft.Json.JsonConvert.DeserializeObject<cmlRabMQData>(tMessage);
                    aFileXml = new List<string>();
                    cVB.tVB_Branch = new cSP().C_GETtBranchCode();
                    // รอมีการส่งค่า Date To Form
                    if (C_CHKbDataFormTo(oRabMQData,ref tDate,ref tBchCode))
                    {
                        try
                        {
                            switch (oRabMQData.ptFunction)
                            {
                                case "MASTER":
                                    C_IMPxMaster(aFileXml);                                    
                                    break;
                                case "EMPLO":
                                    C_IMPxEmployee(aFileXml);
                                    break;
                                case "PRICE":
                                    C_IMPxPrice(aFileXml);
                                    if(C_PRCxADJ(cVB.oTblAdj,oFactory, "ADJUSTPRICE"))
                                    {
                                        
                                        aStr = new List<string>();
                                        
                                        if (cVB.oTblAdj.Rows.Count > 0)
                                        {
                                            foreach (DataRow oRow in cVB.oTblAdj.Rows)
                                            {
                                                string tDocNO = string.Format("RESAJP_{0}_{1}", Convert.ToString(oRow["FTXphDocNo"]), "Interface");
                                                aStr.Add(tDocNO);
                                            }
                                        }
                                        C_MQRxPrcApp(oFactory, oChannel, aStr);
                                    }
                                    break;
                                default:
                                    C_IMPxALL(aFileXml);
                                    if (C_PRCxADJ(cVB.oTblAdj, oFactory, "ADJUSTPRICE"))
                                    {

                                        aStr = new List<string>();

                                        if (cVB.oTblAdj.Rows.Count > 0)
                                        {
                                            foreach (DataRow oRow in cVB.oTblAdj.Rows)
                                            {
                                                string tDocNO = string.Format("RESAJP_{0}_{1}", Convert.ToString(oRow["FTXphDocNo"]), "Interface");
                                                aStr.Add(tDocNO);
                                            }
                                        }
                                        C_MQRxPrcApp(oFactory, oChannel, aStr);
                                    }
                                    break;
                            }
                            oChannel.BasicAck(oEevntArgs.DeliveryTag, false);
                        }
                        catch(Exception oEx) { new cLog().C_PRCxLog("C_PRCxMQProcess",oEx.Message.ToString()); }
                        finally
                        {
                            new cSP().SP_CLExMemory();
                        }
                    }

                };
                oChannel.BasicConsume(queue: cVB.tVB_MQQueue, autoAck: false, consumer: oConsumer);
            }
            catch(Exception oEx) { new cLog().C_PRCxLog("C_PRCxMQProcess", oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
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
                new cSP().C_SAVxDownloadXML(paFileXml, "Master");
                new cMaster().C_PRCxMaster(paFileXml);
            }
            catch(Exception oEx) { new cLog().C_PRCxLog("C_IMPxMaster",oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        public void C_IMPxPrice(List<string> paFileXml)
        {
            try
            {
                new cSP().C_SAVxDownloadXML(paFileXml, "Price");
                new cPrice().C_PRCxPrice(paFileXml);
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_IMPxPrice", oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
        public void C_IMPxEmployee(List<string> paFileXml)
        {
            try
            {
                new cSP().C_SAVxDownloadXML(paFileXml, "Employee");
                new cEmployee().C_PRCxEmployee(paFileXml);
            }
            catch (Exception oEx) { new cLog().C_PRCxLog("C_IMPxEmployee", oEx.Message.ToString()); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
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
                    oC_ResData.ptUser = "Interface";
                    oC_ResData.ptConnStr = cVB.tVB_Conn;
                    
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
