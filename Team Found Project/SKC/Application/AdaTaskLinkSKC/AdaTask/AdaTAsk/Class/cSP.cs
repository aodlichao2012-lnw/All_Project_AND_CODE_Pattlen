using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using AdaTask.Models;
using System.Data.SqlClient;
using AdaHash.Class;
using RabbitMQ.Client;

namespace AdaTask.Class
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
                GC.Collect();
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "SP_CLExMemory : " + oEx.Message);
            }
        }

        /// <summary>
        /// Create sql connection
        /// </summary>
        public void cSP_GETxSQLConnectionString()
        {
            cSecurity_ oSecurity = new cSecurity_();
            cmlSQLConfig oSQL = new cmlSQLConfig();
            NameValueCollection oSqlCfg, oKeyCfg;
            int nConTme, nComTme;
            try
            {
                oKeyCfg = (NameValueCollection)ConfigurationManager.GetSection("keySettings");
                oSqlCfg = (NameValueCollection)ConfigurationManager.GetSection("DBSetting");

                if (oSqlCfg != null)
                {
                    oSQL.tServer = cSecurity.C_Decrypt(oSqlCfg["Server"].Trim(), oKeyCfg["Key"].Trim());
                    oSQL.tUsername = cSecurity.C_Decrypt(oSqlCfg["User"].Trim(), oKeyCfg["Key"].Trim());
                    oSQL.tPassword = cSecurity.C_Decrypt(oSqlCfg["Password"].Trim(), oKeyCfg["Key"].Trim());
                    oSQL.tDatabase = cSecurity.C_Decrypt(oSqlCfg["Database"].Trim(), oKeyCfg["Key"].Trim());
                    oSQL.tAuthenMode = oSqlCfg["AuthenMode"].Trim();
                    int.TryParse(oSqlCfg["ConnectTimeOut"].Trim(), out nConTme);
                    int.TryParse(oSqlCfg["CommandTimeOut"].Trim(), out nComTme);
                    oSQL.nConnectTimeOut = nConTme;
                    oSQL.nCommandTimeOut = nComTme;

                    if (oSQL.tAuthenMode == "0")
                    {
                        cVB.tVB_SQLCon = "Data Source = " + oSQL.tServer + "; Initial Catalog = " + oSQL.tDatabase + "; Persist Security Info = True; User ID = " + oSQL.tUsername + "; Password = " + oSQL.tPassword + ";";
                    }
                    else
                    {
                        cVB.tVB_SQLCon = "Data Source = " + oSQL.tServer + "; Initial Catalog = " + oSQL.tDatabase + "; Integrated Security = True";
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "cSP_GETxSQLConnectionString : " + oEx.Message);
                Console.WriteLine("cSP", "cSP_GETxSQLConnectionString : " + oEx.Message);
            }
        }

        /// <summary>
        /// Get rabbitMQ Config.
        /// </summary>
        public void cSP_GETxRabbitMQConfig()
        {
            cSecurity_ oSecurity = new cSecurity_();
            cmlRabbitMQ oRabbit = new cmlRabbitMQ();
            NameValueCollection oRabbitMQCfg, oKeyCfg;
            try
            {
                oKeyCfg = (NameValueCollection)ConfigurationManager.GetSection("keySettings");
                oRabbitMQCfg = (NameValueCollection)ConfigurationManager.GetSection("rabbitMQSettings");
                if (oRabbitMQCfg == null)
                {
                    string tErr = string.Format("Config {0} not found.", "rabbit MQ");
                    Console.WriteLine(tErr);
                    new cLog().C_WRTxLog("cSP", "cSP_GETxRabbitMQConfig : " + tErr);
                    return;
                }
                //oRabbit.tHostName = oRabbitMQCfg["HostName"].Trim(); //cSecurity.C_Decrypt(oRabbitMQCfg["HostName"].Trim(), oKeyCfg["Key"].Trim());
                //oRabbit.tUserName = oRabbitMQCfg["UserName"].Trim(); //cSecurity.C_Decrypt(oRabbitMQCfg["UserName"].Trim(), oKeyCfg["Key"].Trim());
                //oRabbit.tPassword = oRabbitMQCfg["Password"].Trim(); //cSecurity.C_Decrypt(oRabbitMQCfg["Password"].Trim(), oKeyCfg["Key"].Trim());
                //oRabbit.tVirtual = oRabbitMQCfg["VirtualHost"].Trim();///cSecurity.C_Decrypt(oRabbitMQCfg["VirtualHost"].Trim(), oKeyCfg["Key"].Trim());
                //oRabbit.tHostPort = oRabbitMQCfg["HostPort"].Trim();//cSecurity.C_Decrypt(oRabbitMQCfg["HostPort"].Trim(), oKeyCfg["Key"].Trim());
                //oRabbit.tRoutingKey = oKeyCfg["Key"].Trim();
                //oRabbit.tHostName = oSecurity.C_DATtTripleDESDecryptData(oRabbit.tHostName, oRabbit.tRoutingKey);

                //*Arm 62-10-24
                oRabbit.tHostName = oRabbitMQCfg["HostName"].Trim();
                oRabbit.tUserName = oRabbitMQCfg["UserName"].Trim();
                oRabbit.tPassword = oRabbitMQCfg["Password"].Trim();
                //oRabbit.tVirtual = oRabbitMQCfg["VirtualHost"].Trim();
                oRabbit.tHostPort = oRabbitMQCfg["HostPort"].Trim();
                oRabbit.tQueueName = oRabbitMQCfg["QueueName"].Trim();

                cVB.oVB_RabbitConfig = oRabbit;

                cVB.tVB_VHostMaster = oRabbitMQCfg["VirtualHostMaster"].Trim(); //*Arm 63-07-06
                cVB.tVB_VHostSale = oRabbitMQCfg["VirtualHostSale"].Trim();     //*Arm 63-07-06
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "cSP_GETxRabbitMQConfig : " + oEx.Message);
                Console.WriteLine("cSP", "cSP_GETxRabbitMQConfig : " + oEx.Message);
            }
            finally
            {
                oRabbit = null;
                oSecurity = null;
                oRabbitMQCfg = null;
                oKeyCfg = null;
            }
        }

        /// <summary>
        /// Get shopname for send rabbitmq.
        /// </summary>
        /// <returns></returns>
        public List<string> cSP_GEToShop()
        {
            List<string> aoShop = new List<string>();
            string tSQL;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(cVB.tVB_SQLCon))
                {
                    oConnection.Open();
                    tSQL = string.Empty;
                    //tSQL = "SELECT DISTINCT(FTShpCode) as FTShpCode  FROM TPSTSalHD  WITH (NOLOCK) WHERE CONVERT(VARCHAR, FDLastUpdOn,23) = CONVERT(VARCHAR, (GETDATE()), 23)";
                    tSQL = "SELECT DISTINCT(FTShpCode) as FTShpCode  FROM TCNMShop  WITH (NOLOCK) WHERE FTShpStaActive ='1'";
                    using (SqlCommand command = new SqlCommand(tSQL, oConnection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                aoShop.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                return aoShop;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "cSP_GEToShop : " + oEx.Message);
                Console.WriteLine("cSP", "cSP_GEToShop : " + oEx.Message);
                return null;
            }
            finally
            {
                aoShop = null;
                tSQL = null;
            }
        }

        /// <summary>
        /// Get Locker for send rabbitmq.
        /// </summary>
        /// <returns></returns>
        public List<string> cSP_GEToLoker()
        {
            List<string> aoLoker = new List<string>();
            StringBuilder oSql = new StringBuilder();
            try
            {
                using (SqlConnection oConnection = new SqlConnection(cVB.tVB_SQLCon))
                {
                    oSql.AppendLine("SELECT DISTINCT(FTShpCode) as FTShpCode  FROM TCNMShop  WITH (NOLOCK) ");
                    oSql.AppendLine(" WHERE FTShpStaActive = '1'");
                    using (SqlCommand command = new SqlCommand(oSql.ToString(), oConnection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                aoLoker.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                return aoLoker;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cSP", "cSP_GEToLoker : " + oEx.Message);
                Console.WriteLine("cSP", "cSP_GEToLoker : " + oEx.Message);
                return null;
            }
            finally
            {
                oSql = null;
                aoLoker = null;
            }
        }

        /// <summary>
        /// Send message to rabbitmq.
        /// </summary>
        /// <param name="ptExchangeName"></param>
        /// <param name="ptMessage"></param>
        public void cSP_RabbitPublish(string ptExchangeName, string ptMessage)
        {
            ConnectionFactory oFactory = new ConnectionFactory();
            IConnection oConnection;
            IModel oChannel;

            try
            {
                oFactory.AutomaticRecoveryEnabled = true;
                oFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
                oFactory.TopologyRecoveryEnabled = true;
                oFactory.RequestedConnectionTimeout = 30000;

                oFactory.HostName = cVB.oVB_RabbitConfig.tHostName;
                oFactory.UserName = cVB.oVB_RabbitConfig.tUserName;
                oFactory.Password = cVB.oVB_RabbitConfig.tPassword;
                oFactory.VirtualHost = cVB.oVB_RabbitConfig.tVirtual;
                oFactory.Port = 5672;

                oConnection = oFactory.CreateConnection();

                oChannel = oConnection.CreateModel();
                oChannel.ExchangeDeclare(exchange: ptExchangeName, type: "fanout"); //*Arm 62-10-24
                //oChannel.ExchangeDeclare(ptExchangeName, "fanout");

                var body = Encoding.UTF8.GetBytes(ptMessage);

                oChannel.BasicPublish(exchange: ptExchangeName,
                              routingKey: "",
                              basicProperties: null,
                              body: body);

                new cLog().C_WRTxLog("cEvent1", "cSP_RabbitPublish : "+ string.Format("Publish to rabbit ExchangeName:{0},Message:{1}", ptExchangeName, ptMessage));
                Console.WriteLine(string.Format("Publish to rabbit ExchangeName:{0},Message:{1}", ptExchangeName, ptMessage));
            }
            catch (Exception oEx)
            {
                Console.WriteLine(string.Format("cSP > Exception:{0}", oEx.Message));
            }
            finally
            {
                oFactory = null;
            }
        }

        /// <summary>
        /// Send message to rabbitmq. (QueueName)
        /// </summary>
        /// <param name="ptQueueName"></param>
        /// <param name="ptMessage"></param>
        /// <param name="ptVHost">Visual host (Arm 63-07-06)</param>
        public void C_PRCxMQPublish(string ptQueueName, string ptMessage, string ptVHost)
        {
            //*Arm 62-11-25
            ConnectionFactory oFactory = new ConnectionFactory();
            string tQueueName = ptQueueName;
            try
            {
                oFactory = new ConnectionFactory();
                oFactory.HostName = cVB.oVB_RabbitConfig.tHostName;
                oFactory.UserName = cVB.oVB_RabbitConfig.tUserName;
                oFactory.Password = cVB.oVB_RabbitConfig.tPassword;
                //oFactory.VirtualHost = cVB.oVB_RabbitConfig.tVirtual;
                oFactory.VirtualHost = ptVHost; //*Arm 63-07-06

                using (var oConn = oFactory.CreateConnection())
                {
                    using (var oChannel = oConn.CreateModel())
                    {
                        var body = Encoding.UTF8.GetBytes(ptMessage);
                        oChannel.QueueDeclare(tQueueName, false, false, false, null);
                        oChannel.BasicPublish("", tQueueName, false, null, body);
                        
                    }
                }
                Console.WriteLine(string.Format("Publish to rabbit QueueName:{0},Message:{1}", ptQueueName, ptMessage));
            }
            catch (Exception oEx)
            {
                Console.WriteLine(string.Format("cSP > Exception:{0}", oEx.Message));
            }
            finally
            {
                oFactory = null;
            }
        }
    }
}

