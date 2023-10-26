using AdaPos.Models.Database;
using AdaPos.Models.RabbitMQ;
using AdaPos.Resources_String.Global;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cRabbitMQ
    {

        public event EventHandler oEv_Jump;
        private ResourceManager oC_Resource;

        EventingBasicConsumer oC_Even_consumer;
        ConnectionFactory oC_SubFactory;
        IConnection oC_Connection;
        IModel oC_Channel;

        public string ctMsg_Pos;
        public string ctMsg_Pdt;
        public string ctMsg_Qty;
        public string ctMsg_Row;
        public string ctMsg_Col;
        public string tW_ptQueueName { get; set; }

        public cRabbitMQ(string ptQueueName, string ptExchangeName)
        {
            try
            {
                W_SETxText();
                tW_ptQueueName = ptQueueName;
                C_SUBxRabbitMQ(C_GETxConsumerReceived, ptQueueName, ptExchangeName);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "cRabbitMQ : " + oEx.Message);
            }

        }

        /// <summary>
        /// Set Text
        /// </summary>
        private void W_SETxText()
        {
            try
            {
                switch (cVB.nVB_Language)
                {
                    case 1:     // TH
                        oC_Resource = new ResourceManager(typeof(resGlobal_TH));
                        break;

                    default:    // EN
                        oC_Resource = new ResourceManager(typeof(resGlobal_EN));
                        break;
                }

                ctMsg_Pos = oC_Resource.GetString("tMsg_Pos");
                ctMsg_Pdt = oC_Resource.GetString("tMsg_Pdt");
                ctMsg_Qty = oC_Resource.GetString("tMsg_Qty");
                ctMsg_Row = oC_Resource.GetString("tMsg_Row");
                ctMsg_Col = oC_Resource.GetString("tMsg_Col");
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "W_consumer_Received : " + oEx.Message);
            }
        }

        public void C_SUBxRabbitMQ(EventHandler<BasicDeliverEventArgs> C_GETxConsumerReceived, string ptQueueName, string ptExchangeName)
        {
            try
            {
                string tQueueName = ptQueueName;
                string tExchangeName = ptExchangeName;
                //string tQueueName = "STOCKVD2POS";
                //string tExchangeName = "STOCKVD2POS";
                //string tRoutingKey = cVB.oVB_RabbitMQConfig.tRoutingKey;

                oC_SubFactory = new ConnectionFactory();
                //oC_SubFactory.AutomaticRecoveryEnabled = true;
                //oC_SubFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
                //oC_SubFactory.TopologyRecoveryEnabled = true;
                //oC_SubFactory.RequestedConnectionTimeout = 30000; //30 วินาที

                if (string.IsNullOrEmpty(cVB.oVB_RabbitMQConfig.tHostName)) return; //*Em 62-09-03
                if (string.IsNullOrEmpty(cVB.oVB_RabbitMQConfig.tUserName)) return; //*Em 62-09-03
                if (string.IsNullOrEmpty(cVB.oVB_RabbitMQConfig.tPassword)) return; //*Em 62-09-03
                if (string.IsNullOrEmpty(cVB.oVB_RabbitMQConfig.tVirtual)) return; //*Em 62-09-03

                oC_SubFactory.HostName = cVB.oVB_RabbitMQConfig.tHostName;
                //oW_SubFactory.Port = int.Parse(cVB.oVB_RabbitMQConfig.tHostPort);
                oC_SubFactory.UserName = cVB.oVB_RabbitMQConfig.tUserName;
                oC_SubFactory.Password = cVB.oVB_RabbitMQConfig.tPassword;
                oC_SubFactory.VirtualHost = cVB.oVB_RabbitMQConfig.tVirtual;

                oC_Connection = oC_SubFactory.CreateConnection();

                oC_Channel = oC_Connection.CreateModel();

                
                oC_Channel.QueueDeclare(queue:tQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                if (!string.IsNullOrEmpty(tExchangeName)) //ถ้าไม่มี Exchange ไม่ต้อง Declare
                {
                    oC_Channel.ExchangeDeclare(exchange: tExchangeName, type: "fanout");
                    oC_Channel.QueueBind(tQueueName, tExchangeName, "", null);
                }

                oC_Even_consumer = new EventingBasicConsumer(oC_Channel);
                
                oC_Even_consumer.Received += C_GETxConsumerReceived;

                //oC_Channel.BasicConsume(
                //    queue: tQueueName,
                //    autoAck: cVB.oVB_RabbitMQConfig.bAutoAck,
                //    consumer: oC_Even_consumer
                //);
                oC_Channel.BasicConsume(queue: tQueueName,autoAck: false,consumer: oC_Even_consumer);   //*Em 62-09-14
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_SUBxRabbitMQ : " + oEx.Message);
            }
        }

        private void C_GETxConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                string tSubMessage = Encoding.UTF8.GetString(e.Body);
                if (tW_ptQueueName == "STOCKVD2POS")
                {
                    C_INSxMsgRemind(tSubMessage);
                    oC_Channel.BasicAck(e.DeliveryTag, false);
                }
                if (tW_ptQueueName == "PS_QMember" + cVB.tVB_ShpCode)
                {
                    C_INSxQMember(tSubMessage);
                    oC_Channel.BasicAck(e.DeliveryTag, false);
                }
                //*Em 63-01-06
                if (tW_ptQueueName == "FN_PayRetCoupon" + cVB.tVB_BchCode + cVB.tVB_PosCode)
                {
                    C_PRCxCoupon(tSubMessage);
                    oC_Channel.BasicAck(e.DeliveryTag, false);
                }
                //++++++++++++

                C_PRCxONJump();
                //oC_Channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_GETxConsumerReceived : " + oEx.Message);
            }
        }

        public void C_INSxMsgRemind(string ptSubMessage)
        {
            StringBuilder oSql = new StringBuilder();
            string tFTPdtName;
            
            try
            {
                //oSql = new StringBuilder();
                //oSql.AppendLine("INSERT INTO TCNTMsgRemind (");
                //oSql.AppendLine("FTMsgStaRead,");
                ////oSql.AppendLine("FNMsgSeq,");
                //oSql.AppendLine("FNMsgType,");
                //oSql.AppendLine("FTMsgGroup,");
                //oSql.AppendLine("FTMsgDocRef,");
                //oSql.AppendLine("FDCreateOn,");
                //oSql.AppendLine("FTCreateBy)");
                //oSql.AppendLine(" VALUES (");
                //oSql.AppendLine("'0',");
                ////oSql.AppendLine(",<FNMsgSeq, int,>");
                //oSql.AppendLine("3,");
                //oSql.AppendLine("'TVDTPdtStkbal',");
                //oSql.AppendLine("'',");
                //oSql.AppendLine("GETDATE(),");
                //oSql.AppendLine("'MQReceivePrc')");
                //new cDatabase().C_SETxDataQuery(oSql.ToString());

                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT MAX(FNMsgID) AS FNMsgID FROM TCNTMsgRemind");
                //cmlTCNTMsgRemind oMsgRm = new cDatabase().C_GEToDataQuery<cmlTCNTMsgRemind>(oSql.ToString());

                cmlNotification oNoti = JsonConvert.DeserializeObject<cmlNotification>(ptSubMessage);
                string tMsg_TH = string.Empty;
                if (oNoti != null)
                {
                    //tMsg_TH = string.Format(ctMsg_Pos + "{0}", oNoti.ptPOSCode);
                    if (oNoti.aoDetail != null)
                    {
                        tMsg_TH += Environment.NewLine;
                        int nNum = 1;
                        foreach (var oItem in oNoti.aoDetail)
                        {
                            oSql = new StringBuilder();  //BOY 62/11/05
                            tMsg_TH = string.Format(ctMsg_Pos + " {0}", oNoti.ptPOSCode);
                            tMsg_TH += Environment.NewLine;
                            oSql.Clear();
                            oSql.AppendLine("INSERT INTO TCNTMsgRemind (");
                            oSql.AppendLine("FTMsgStaRead,");
                            //oSql.AppendLine("FNMsgSeq,");
                            oSql.AppendLine("FNMsgType,");
                            oSql.AppendLine("FTMsgGroup,");
                            oSql.AppendLine("FTMsgDocRef,");
                            oSql.AppendLine("FDCreateOn,");
                            oSql.AppendLine("FTCreateBy)");
                            oSql.AppendLine(" VALUES (");
                            oSql.AppendLine("'0',");
                            //oSql.AppendLine(",<FNMsgSeq, int,>");
                            oSql.AppendLine("3,");
                            oSql.AppendLine("'TVDTPdtStkbal',");
                            oSql.AppendLine("'',");
                            oSql.AppendLine("GETDATE(),");
                            oSql.AppendLine("'MQReceivePrc')");
                            new cDatabase().C_SETxDataQuery(oSql.ToString());

                            oSql.Clear();
                            oSql.AppendLine("SELECT MAX(FNMsgID) AS FNMsgID FROM TCNTMsgRemind");
                            cmlTCNTMsgRemind oMsgRm = new cDatabase().C_GEToDataQuery<cmlTCNTMsgRemind>(oSql.ToString());
                            string Cut;
                            oSql.Clear();
                            oSql.AppendLine("SELECT ISNULL(PDTL.FTPdtName,(SELECT TOP 1 FTPdtName FROM TCNMPdt_L WITH(NOLOCK)");
                            oSql.AppendLine("WHERE FTPdtCode = PDT.FTPdtCode)) AS FTPdtName");
                            oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK)");
                            oSql.AppendLine("LEFT JOIN TCNMPdt_L PDTL WITH(NOLOCK) ON PDT.FTPdtCode = PDTL.FTPdtCode");
                            oSql.AppendLine("AND PDTL.FNLngID = "+ cVB.nVB_Language + "");
                            oSql.AppendLine("WHERE PDT.FTPdtCode = '"+ oItem.ptFTPdtCode + "'");
                            tFTPdtName = new cDatabase().C_GEToDataQuery<string>(oSql.ToString()).ToString();
                            if(tFTPdtName.Length >= 28)
                            {
                                tFTPdtName = tFTPdtName.Substring(0, 28);
                                tMsg_TH += string.Format(" {0}. " + ctMsg_Pdt + " {1}... ", nNum, tFTPdtName);
                            }
                            else
                            {
                                tMsg_TH += string.Format(" {0}. " + ctMsg_Pdt + " {1} ", nNum, tFTPdtName);
                            }
                            //tMsg_TH += string.Format(" {0}. " + ctMsg_Pdt + " {1}... ", nNum, tFTPdtName);
                            tMsg_TH += Environment.NewLine;
                            tMsg_TH += string.Format(""+ ctMsg_Qty + "{0} " + ctMsg_Row + "{1} " + ctMsg_Col + "{2}", oItem.pnFCStkQty, oItem.pnFNLayRow, oItem.pnFNLayCol);

                            oSql.Clear();
                            oSql.AppendLine("INSERT INTO TCNTMsgRemind_L (");
                            oSql.AppendLine("FNMsgID,");
                            oSql.AppendLine("FNLngID,");
                            oSql.AppendLine("FTMsgData,");
                            oSql.AppendLine("FTMsgRmk)");
                            oSql.AppendLine(" VALUES (");
                            oSql.AppendLine("'" + oMsgRm.FNMsgID + "',");
                            oSql.AppendLine("" + cVB.nVB_Language + ",");
                            oSql.AppendLine("'" + tMsg_TH + "',");
                            oSql.AppendLine("'')");
                            new cDatabase().C_SETxDataQuery(oSql.ToString());

                            tMsg_TH = "";
                            nNum += 1;

                            //tMsg_TH += string.Format(" {0}. "+ ctMsg_Pdt + "{1} "+ ctMsg_Qty + "{2} "+ ctMsg_Row + "{3} "+ ctMsg_Col + "{4}", nNum, tFTPdtName, oItem.pnFCStkQty, oItem.pnFNLayRow, oItem.pnFNLayCol);
                            //tMsg_TH += Environment.NewLine;
                        }
                    }
                }

                //oSql = new StringBuilder();
                //oSql.AppendLine("INSERT INTO TCNTMsgRemind_L (");
                //oSql.AppendLine("FNMsgID,");
                //oSql.AppendLine("FNLngID,");
                //oSql.AppendLine("FTMsgData,");
                //oSql.AppendLine("FTMsgRmk)");
                //oSql.AppendLine(" VALUES (");
                //oSql.AppendLine("'" + oMsgRm.FNMsgID + "',");
                //oSql.AppendLine(""+ cVB.nVB_Language + ",");
                //oSql.AppendLine("'" + tMsg_TH + "',");
                //oSql.AppendLine("'')");
                //new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_INSxMsgRemind : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void C_INSxQMember(string ptSubMessage)  //*Arm 62-10-26
        {
            cmlQueueMember aoQueue = JsonConvert.DeserializeObject<cmlQueueMember>(ptSubMessage);
            string tQData = aoQueue.ptCustomer.ptFTCstCode + "|" + aoQueue.ptCustomer.ptFTCstName + "|" + aoQueue.ptCustomer.ptFTCstTel + "|" + aoQueue.ptCustomer.ptFTCstPriGrp ;
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO TCNTMsgQueue(");
                oSql.AppendLine("FTMsgQName,");
                oSql.AppendLine("FTMsgQData,");
                oSql.AppendLine("FTMsgStaActive,");
                oSql.AppendLine("FTMsgStaPrc,");
                oSql.AppendLine("FTMsgRemark,");
                oSql.AppendLine("FDCreateOn,");
                oSql.AppendLine("FTCreateBy)");
                oSql.AppendLine(" VALUES (");
                oSql.AppendLine("'" + tW_ptQueueName + "',");
                oSql.AppendLine("'"+tQData+"',");
                oSql.AppendLine("'1',");
                oSql.AppendLine("'',");
                oSql.AppendLine("'',");
                oSql.AppendLine("GETDATE(),");
                oSql.AppendLine("'MQReceivePrc'");
                oSql.AppendLine(" )");
                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_INSxQMember : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void C_PRCxCoupon(string ptSubMsg)
        {
            try
            {
                cmlResFunc oRes = JsonConvert.DeserializeObject<cmlResFunc>(ptSubMsg);
                cmlResData oResData = JsonConvert.DeserializeObject<cmlResData>(oRes.ptData);

                if (oResData.rtCode == "200")
                {
                    cPayment.tC_XrcRef1 = "OK";
                    cPayment.tC_XrcRef2 = "";
                }
                else
                {
                    cPayment.tC_XrcRef1 = oResData.rtCode;
                    cPayment.tC_XrcRef2 = oResData.rtDesc;
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_PRCxCoupon : " + oEx.Message);
            }
        }
        public void C_PRCxONJump()
        {
            try
            {
                EventHandler handler = oEv_Jump;
                if (null != handler) handler(this, EventArgs.Empty);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_ONxJump : " + oEx.Message);
            }
        }

        public void C_DISxDisConnect()
        {
            try
            {
                oC_Channel.Close();
                oC_Connection.Close();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_DISxDisConnect : " + oEx.Message);
            }
        }

        /// <summary>
        /// Set MQ Connection Endpoint
        /// </summary>
        /// <param name="pbHQEndpoint">true = HQ Endpoint || false = Bch Endpoint</param>
        public static void C_SETxMQEnpoint(bool pbHQEndpoint)
        {
            if (pbHQEndpoint)
            {
                cVB.oVB_RabbitMQConfig.tHostName = cVB.tVB_HQMQHost;
                cVB.oVB_RabbitMQConfig.tUserName = cVB.tVB_HQMQUsr;
                cVB.oVB_RabbitMQConfig.tPassword = cVB.tVB_HQMQPwd;
                cVB.oVB_RabbitMQConfig.tVirtual = cVB.tVB_HQMQVirtual;
            }
            else
            {
                cVB.oVB_RabbitMQConfig.tHostName = cVB.tVB_BCHMQHost;
                cVB.oVB_RabbitMQConfig.tUserName = cVB.tVB_BCHMQUsr;
                cVB.oVB_RabbitMQConfig.tPassword = cVB.tVB_BCHMQPwd;
                cVB.oVB_RabbitMQConfig.tVirtual = cVB.tVB_BCHMQVirtual;
            }
            cVB.oVB_MQFactory = new ConnectionFactory();
            cVB.oVB_MQFactory.HostName = cVB.oVB_RabbitMQConfig.tHostName;
            cVB.oVB_MQFactory.UserName = cVB.oVB_RabbitMQConfig.tUserName;
            cVB.oVB_MQFactory.Password = cVB.oVB_RabbitMQConfig.tPassword;
            cVB.oVB_MQFactory.VirtualHost = cVB.oVB_RabbitMQConfig.tVirtual;
        }

        /// <summary>
        /// Net 63-06-04
        /// Publish to Queue
        /// </summary>
        /// <param name="pbPub2HQ">true = Pub to HQ || false = Pub to Bch</param>
        /// <param name="ptMsg">String Message</param>
        /// <param name="ptQueue">Queue Name with Declare</param>
        /// <param name="pbQDurable">Queue isDurable</param>
        //public static void C_PRCxPub2Queue(bool pbPub2HQ, string ptMsg, string ptQueue,bool pbQDurable=false)
        public static bool C_PRCxPub2Queue(bool pbPub2HQ, string ptMsg, string ptQueue, bool pbQDurable = false) //*Net 63-07-13 return success
        {
            try
            {
                C_SETxMQEnpoint(pbPub2HQ);

                cVB.oVB_MQFactory = new ConnectionFactory();
                cVB.oVB_MQFactory.HostName = cVB.oVB_RabbitMQConfig.tHostName;
                cVB.oVB_MQFactory.UserName = cVB.oVB_RabbitMQConfig.tUserName;
                cVB.oVB_MQFactory.Password = cVB.oVB_RabbitMQConfig.tPassword;
                cVB.oVB_MQFactory.VirtualHost = cVB.oVB_RabbitMQConfig.tVirtual;
                using (var oMQConn = cVB.oVB_MQFactory.CreateConnection())
                {
                    using (var oMQChannel = oMQConn.CreateModel())
                    {
                        oMQChannel.QueueDeclare(ptQueue, pbQDurable, false, false, null);
                        oMQChannel.BasicPublish("", ptQueue, false, null, Encoding.UTF8.GetBytes(ptMsg));
                    }
                }
                return true;//*Net 63-07-13 return success
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_PRCxPub2Queue : " + oEx.Message);
            }
            return false;//*Net 63-07-13 return fail
        }

        /// <summary>
        /// Net 63-06-04
        /// Publish to Exchange
        /// </summary>
        /// <param name="pbPub2HQ">true = Pub to HQ || false = Pub to Bch</param>
        /// <param name="ptMsg">String Message</param>
        /// <param name="ptExchange">Exchange Name with Decleare</param>
        /// <param name="ptExType">Exchange Type : "direct","fanout","topic",headers</param>
        /// <param name="ptRoute">Routing of Exchange</param>
        /// <param name="ptQueueBind">Binding Ex and Q with Routing then Pub to Exchange with Routing. || if null/empty no Binding then Pub to Exchange with Routing</param>
        /// <param name="pbExDurable">Exchange isDurable</param>
        /// <param name="pbQDurable">Queue isDurable</param>
        public static void C_PRCxPub2Exchange(bool pbPub2HQ, string ptMsg, string ptExchange, string ptExType, string ptRoute, string ptQueueBind = "", bool pbExDurable = false,bool pbQDurable = false)
        {
            try
            {
                C_SETxMQEnpoint(pbPub2HQ);

                cVB.oVB_MQFactory = new ConnectionFactory();
                cVB.oVB_MQFactory.HostName = cVB.oVB_RabbitMQConfig.tHostName;
                cVB.oVB_MQFactory.UserName = cVB.oVB_RabbitMQConfig.tUserName;
                cVB.oVB_MQFactory.Password = cVB.oVB_RabbitMQConfig.tPassword;
                cVB.oVB_MQFactory.VirtualHost = cVB.oVB_RabbitMQConfig.tVirtual;
                using (var oMQConn = cVB.oVB_MQFactory.CreateConnection())
                {
                    using (var oMQChannel = oMQConn.CreateModel())
                    {
                        oMQChannel.ExchangeDeclare(ptExchange, ptExType, pbExDurable, false, null);
                        if(!String.IsNullOrEmpty(ptQueueBind))
                        {
                            oMQChannel.QueueDeclare(ptQueueBind, pbQDurable, false, false, null);
                            oMQChannel.QueueBind(ptQueueBind, ptExchange, ptRoute, null);
                        }
                        oMQChannel.BasicPublish(ptExchange, ptRoute, false, null, Encoding.UTF8.GetBytes(ptMsg));
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cRabbitMQ", "C_PRCxPub2Exchange : " + oEx.Message);
            }
        }
    }
}
