using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Doc;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Webservice.Response;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    class cNotiDocPrc
    {
        cDatabaseSP oDBSP;
        cmlShopDB oShopDB;
        private string t_ConnStr;
        private string tUsrUpdate;
        private string tPubEx = "AR_XSaleOrder";
        public bool C_PRCbNotiDoc(cmlRcvData poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlNotiTARTDocApvTxn oNotiDoc;
            oShopDB = poShopDB;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                CultureInfo oCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                oCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
                oCulture.DateTimeFormat.LongTimePattern = "";
                Thread.CurrentThread.CurrentCulture = oCulture;

                t_ConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                oDBSP = new cDatabaseSP(t_ConnStr, poShopDB);
                oNotiDoc = JsonConvert.DeserializeObject<cmlNotiTARTDocApvTxn>(poData.ptData);
                switch (oNotiDoc.tFTStaDoc)
                {
                    case "1":
                        /*string tMsgNotiJson = JsonConvert.SerializeObject(poData);
                        if (C_PRCbMQDeclareExchange(tPubEx + cVB.tVB_BchCode, ExMode.fanout) &&
                        C_PRCbMQDeclareQueue($"AR_QNotiMsgPrc{cVB.tVB_BchCode}") &&
                        C_PRCbMQDeclareQueue($"AR_QNotiMsg{cVB.tVB_BchCode}") &&
                        C_PRCbMQDeclareQueue($"AR_QNotiMsg{cVB.tVB_BchCode}{poData.ptFilter}") &&
                        C_PRCbMQBindRouting(tPubEx + cVB.tVB_BchCode, $"AR_QNotiMsgPrc{cVB.tVB_BchCode}", "") &&
                        C_PRCbMQBindRouting(tPubEx + cVB.tVB_BchCode, $"AR_QNotiMsg{cVB.tVB_BchCode}", "") &&
                        C_PRCbMQBindRouting(tPubEx + cVB.tVB_BchCode, $"AR_QNotiMsg{cVB.tVB_BchCode}{poData.ptFilter}", "") &&
                        C_PRCbMQPublish2Queue($"AR_QNotiMsg{cVB.tVB_BchCode}{poData.ptFilter}", tMsgNotiJson))
                        {
                            ptErrMsg = "success";
                        }
                        else
                        {
                            ptErrMsg = "Publish Fail";
                            throw new Exception();
                        }*/
                        break;
                    case "2":
                        tUsrUpdate = oNotiDoc.tFTXshApvCode;
                        if (tUsrUpdate == "HIS")
                        {
                            if (C_PRCbDBUpdateDocApvTxn(oNotiDoc.tFTBchCode, oNotiDoc.tFTXshDocNo, 1, "1", oNotiDoc.tFTXshApvCode, oNotiDoc.tFDXshDocDate) &&
                                C_PRCbDBUpdateDocApvTxn(oNotiDoc.tFTBchCode, oNotiDoc.tFTXshDocNo, 2, "1", oNotiDoc.tFTXshApvCode, oNotiDoc.tFDXshDocDate))
                            {
                                ptErrMsg = "success";
                            }
                            else
                            {
                                ptErrMsg = $"Process StaDoc {oNotiDoc.tFTStaDoc} {oNotiDoc.tFTXshDocNo} Fail";
                                throw new Exception();
                            }
                        }
                        else
                        {
                            if (C_PRCbDBUpdateDocApvTxn(oNotiDoc.tFTBchCode, oNotiDoc.tFTXshDocNo, 2, "1", oNotiDoc.tFTXshApvCode, oNotiDoc.tFDXshDocDate))
                            {
                                ptErrMsg = "success";
                            }
                            else
                            {
                                ptErrMsg = $"Process StaDoc {oNotiDoc.tFTStaDoc} {oNotiDoc.tFTXshDocNo} Fail";
                                throw new Exception();
                            }
                        }
                        break;
                    case "3":
                        tUsrUpdate = oNotiDoc.tFTXshApvCode;
                        if (C_PRCbDBUpdateDocApvTxn(oNotiDoc.tFTBchCode, oNotiDoc.tFTXshDocNo, 3, "1", oNotiDoc.tFTXshApvCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                        {
                            ptErrMsg = "success";
                        }
                        else
                        {
                            ptErrMsg = $"Process StaDoc {oNotiDoc.tFTStaDoc} {oNotiDoc.tFTXshDocNo} Fail";
                            throw new Exception();
                        }
                        break;
                    case "4":
                        tUsrUpdate = oNotiDoc.tFTXshApvCode;
                        if (C_PRCbDBDeleteImgDocNo(oNotiDoc.tFTXshDocNo) &&
                        C_PRCbSaveImg(oNotiDoc.tFTImgBase64, oNotiDoc.tFTXshDocNo, oNotiDoc.tFTXshApvCode))
                        {
                            if (C_PRCbDBUpdateDocApvTxn(oNotiDoc.tFTBchCode, oNotiDoc.tFTXshDocNo, 4, "1", oNotiDoc.tFTXshApvCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                            {
                                ptErrMsg = "success";
                            }
                            else
                            {
                                ptErrMsg = $"Process StaDoc {oNotiDoc.tFTStaDoc} {oNotiDoc.tFTXshDocNo} Fail";
                                throw new Exception();
                            }
                        }
                        else
                        {
                            ptErrMsg = $"Process StaDoc {oNotiDoc.tFTStaDoc} {oNotiDoc.tFTXshDocNo} Fail";
                            throw new Exception();
                        }
                        break;
                    case "5":
                        tUsrUpdate = oNotiDoc.tFTXshApvCode;
                        if (C_CHKbSODTApv(oNotiDoc.tFTBchCode, oNotiDoc.tFTXshDocNo))
                        {
                            if (C_PRCbDBUpdateDocApvTxn(oNotiDoc.tFTBchCode, oNotiDoc.tFTXshDocNo, 5, "1", oNotiDoc.tFTXshApvCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                            {
                                ptErrMsg = "success";
                            }
                            else
                            {
                                ptErrMsg = $"Process StaDoc {oNotiDoc.tFTStaDoc} {oNotiDoc.tFTXshDocNo} Fail";
                                throw new Exception();
                            }
                        }
                        else
                        {
                            ptErrMsg = $"Process StaDoc {oNotiDoc.tFTStaDoc} {oNotiDoc.tFTXshDocNo} Waiting for SODT Apv";
                            throw new Exception();
                        }
                        break;
                    case "6":
                        tUsrUpdate = oNotiDoc.tFTXshApvCode;
                        if (C_PRCbDBUpdateDocApvTxn(oNotiDoc.tFTBchCode, oNotiDoc.tFTXshDocNo, 6, "1", oNotiDoc.tFTXshApvCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                        {
                            ptErrMsg = "success";
                        }
                        else
                        {
                            ptErrMsg = $"Process StaDoc {oNotiDoc.tFTStaDoc} {oNotiDoc.tFTXshDocNo} Fail";
                            throw new Exception();
                        }
                        break;
                    default:
                        break;
                }
                return true;
            }
            catch (Exception oEx)
            {
                if (ptErrMsg == "") ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbUplSO");
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }

        private bool C_PRCbSaveImg(string ptImgBase64, string ptDocNo, string ptApvCode)
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect;
            string tPathFile = "";
            try
            {
                tPathFile = new cSP().SP_PRCtBase64Image(ptImgBase64, ptDocNo, "PdtImg");
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine($"INSERT INTO [dbo].[TCNMImgObj]");
                oSql.AppendLine($"([FTImgRefID]");
                oSql.AppendLine($",[FNImgSeq]");
                oSql.AppendLine($",[FTImgTable]");
                oSql.AppendLine($",[FTImgKey]");
                oSql.AppendLine($",[FTImgObj]");
                oSql.AppendLine($",[FDLastUpdOn]");
                oSql.AppendLine($",[FTLastUpdBy]");
                oSql.AppendLine($",[FDCreateOn]");
                oSql.AppendLine($",[FTCreateBy])");
                oSql.AppendLine($"VALUES");
                oSql.AppendLine($"('{ptDocNo}'");
                oSql.AppendLine($",1");
                oSql.AppendLine($",'TARTSoHD'");
                oSql.AppendLine($",'main'");
                oSql.AppendLine($",'{tPathFile.Replace("\\", "/")}'");
                oSql.AppendLine($",'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
                oSql.AppendLine($",'{ptApvCode}'");
                oSql.AppendLine($",'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
                oSql.AppendLine($",'{ptApvCode}')");

                oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowAffect);
                if (nRowAffect > 0) return true;
                else return false;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        private bool C_PRCbDBUpdateDocApvTxn(string ptBchCode, string ptDocNo, int pnApvSeq, string ptStaPrc, string ptUsrApv = null, string ptDateApv = null)
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine("SELECT FTDatRefCode ");
                oSql.AppendLine("FROM TARTDocApvTxn ");
                oSql.AppendLine($"WHERE FTBchCode='{ptBchCode}' AND FTDatRefCode='{ptDocNo}'");

                if (oDB.C_GETaDataQuery<string>(t_ConnStr, oSql.ToString(), oShopDB.nConnectTimeOut.Value).Count > 0)
                {
                    oSql.AppendLine($"UPDATE [TARTDocApvTxn]");
                    oSql.AppendLine($" SET [FTDatStaPrc] = '{ptStaPrc}'");
                    if (!(String.IsNullOrEmpty(ptUsrApv) || String.IsNullOrEmpty(ptDateApv)))
                    {
                        oSql.AppendLine($" ,[FTDatUsrApv] = '{ptUsrApv}'");
                        oSql.AppendLine($" ,[FDDatDateApv] = '{ptDateApv}'");
                    }
                    oSql.AppendLine($" ,[FTLastUpdBy] = '{tUsrUpdate}'");
                    oSql.AppendLine($" ,[FDLastUpdOn] = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
                    oSql.AppendLine($" WHERE FNDatApvSeq={pnApvSeq} AND FTBchCode='{ptBchCode}' AND FTDatRefCode='{ptDocNo}'");
                    oSql.AppendLine($"");

                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowAffect);
                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oSql = null;
                oDB = null;
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        private bool C_PRCbDBDeleteImgDocNo(string ptDocNo)
        {
            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect;
            try
            {
                if (!String.IsNullOrEmpty(ptDocNo))
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();

                    oSql.AppendLine($" DELETE FROM [dbo].[TCNMImgObj]");
                    oSql.AppendLine($" WHERE [FTImgTable] = 'TARTSoHD' AND [FTImgRefID] = '{ptDocNo}';");

                    oDB.C_DATbExecuteNonQuery(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut, out nRowAffect);


                    return true;
                }
                else new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + "Error AutoDocNo");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return false;
        }
        private bool C_CHKbSODTApv(string ptBchCode, string ptDocNo)
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<string> oSODTStaPrcStk;
            try
            {
                if (!String.IsNullOrEmpty(ptBchCode) && !String.IsNullOrEmpty(ptDocNo))
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();
                    oSODTStaPrcStk = new List<string>();
                    oSql.AppendLine($"SELECT ISNULL([FTXsdStaPrcStk],'0')");
                    oSql.AppendLine($"FROM [TARTSoDT]");
                    oSql.AppendLine($"WHERE [FTBchCode]='{ptBchCode}' AND [FTXshDocNo]='{ptDocNo}'");

                    oSODTStaPrcStk = oDB.C_GETaDataQuery<string>(t_ConnStr, oSql.ToString(), (int)oShopDB.nCommandTimeOut);
                    if (oSODTStaPrcStk.Count == oSODTStaPrcStk.Count(x => !String.IsNullOrEmpty(x)) && oSODTStaPrcStk.Count != 0)
                    {
                        return true;
                    }
                    else return false;
                }
                else new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + "Error AutoDocNo");

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            return false;
        }


        #region RabbitMQ
        static ConnectionFactory oMQFactory;
        static IConnection oMQConn;
        static IModel oMQPubChannel;
        static IBasicProperties oMQPubProp;
        public enum ExMode
        {
            direct, fanout, headers, topic
        }
        public static bool C_PRCbMQDeclareExchange(string ptExchange, ExMode nMode)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                if (!String.IsNullOrEmpty(ptExchange))
                {
                    oMQPubChannel.ExchangeDeclare(ptExchange, Enum.GetName(typeof(ExMode), nMode), true, false, null);
                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static bool C_PRCbMQDeclareQueue(string ptQueue)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                if (!String.IsNullOrEmpty(ptQueue))
                {
                    oMQPubChannel.QueueDeclare(ptQueue, true, false, false, null);
                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static bool C_PRCbMQBindRouting(string ptExchange, string ptQueue, string ptRoutekey)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                if (!String.IsNullOrEmpty(ptExchange) && !String.IsNullOrEmpty(ptQueue))
                {
                    oMQPubChannel.QueueBind(ptQueue, ptExchange, ptRoutekey, null);
                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static bool C_PRCbMQPublish2Queue(string ptQueueName, string ptMessage)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                var body = Encoding.UTF8.GetBytes(ptMessage);
                oMQPubChannel.BasicPublish("", ptQueueName, false, oMQPubProp, body);

                return true;

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }
        public static bool C_PRCbMQPublish2Exchange(string ptExchangeName, string ptRouting, string ptMessage)
        {
            try
            {
                if (oMQConn == null || oMQFactory == null || oMQPubChannel == null)
                {
                    oMQFactory = new ConnectionFactory();
                    oMQFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                    oMQFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                    oMQFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                    oMQFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                    oMQConn = oMQFactory.CreateConnection();
                    oMQPubChannel = oMQConn.CreateModel();
                    oMQPubProp = oMQPubChannel.CreateBasicProperties();
                    oMQPubProp.DeliveryMode = 2;

                }

                if (!String.IsNullOrEmpty(ptExchangeName) && !String.IsNullOrEmpty(ptRouting))
                {
                    var body = Encoding.UTF8.GetBytes(ptMessage);
                    oMQPubChannel.BasicPublish(ptExchangeName, ptRouting, false, oMQPubProp, body);

                    return true;
                }

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cNotiDocPrc", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return false;
        }

        #endregion
    }
}
