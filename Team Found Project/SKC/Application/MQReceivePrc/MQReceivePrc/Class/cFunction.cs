using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using MQReceivePrc.Class;
using System.Data.Common;
using System.Net.Http;
using RabbitMQ.Client;
using System.Configuration;
using MQReceivePrc.Models.Receive;

namespace MQReceivePrc.Class
{
    class cFunction
    {
        
        #region Encrypt - Decrypt
        private static TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
        public string tCS_CNEncDec = "SOFTXada";
        public static string tVB_CNQueueName;
        //public string tCS_CNEncDec = "adasoft";
        private static byte[] TruncateHash(string ptkey, int piLength)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            // Hash the key.
            byte[] keyBytes = System.Text.Encoding.Unicode.GetBytes(ptkey);
            byte[] hash = sha1.ComputeHash(keyBytes);
            var oldHash = hash;
            hash = new byte[piLength - 1 + 1];

            // Truncate or pad the hash.
            if (oldHash != null)
                Array.Copy(oldHash, hash, Math.Min(piLength - 1 + 1, oldHash.Length));
            return hash;
        }
        public string SP_EncryptData(string ptPlaintext, string ptkey)
        {

            // Convert the plaintext string to a byte array.
            byte[] plaintextBytes = System.Text.Encoding.Unicode.GetBytes(ptPlaintext);

            // Initialize the crypto provider.
            TripleDes.Key = TruncateHash(ptkey, TripleDes.KeySize / 8);
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize / 8);

            // Create the stream.
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            // Create the encoder to write to the stream.
            CryptoStream encStream = new CryptoStream(ms, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

            // Use the crypto stream to write the byte array to the stream.
            encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
            encStream.FlushFinalBlock();

            // Convert the encrypted stream to a printable string.
            return Convert.ToBase64String(ms.ToArray());
        }
        public string SP_DecryptData(string ptEncryptedtext, string ptkey)
        {

            // Convert the encrypted text string to a byte array.

            // Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            // Initialize the crypto provider.
            TripleDes.Key = TruncateHash(ptkey, TripleDes.KeySize / 8);
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize / 8);

            byte[] encryptedBytes;
            try
            {
                encryptedBytes = Convert.FromBase64String(ptEncryptedtext);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

            // Create the stream.
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            // Create the decoder to write to the stream.
            CryptoStream decStream = new CryptoStream(ms, TripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

            // Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decStream.FlushFinalBlock();

            // Convert the plaintext stream to a string.
            return System.Text.Encoding.Unicode.GetString(ms.ToArray());
        }
        #endregion

        public static void C_LOGxKeepLogErr(string ptMsg,string ptFunction)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + tVB_CNQueueName + " "+ ptFunction +" : Error = " + ptMsg);
        }

        public static void C_PRCxMQResponsce(string ptResMQ,string ptDocNo,string ptUser, string ptProg ,out string ptErrMsg)
        {
            ConnectionFactory oFactory;
            cFunction oFunc = new cFunction();
            string tQueueName = ptResMQ + "_" + ptDocNo + "_" + ptUser;
            try
            {
                oFactory = new ConnectionFactory();
                oFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                oFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                oFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                oFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
                using (var oConn = oFactory.CreateConnection())
                {
                    using (var oChannel = oConn.CreateModel())
                    {
                        //string tJson = JsonConvert.SerializeObject(ptProg);
                        var body = Encoding.UTF8.GetBytes(ptProg);
                        oChannel.QueueDeclare(tQueueName, true, false, false, null);
                        oChannel.BasicPublish("", tQueueName, false, null, body);
                        Console.WriteLine("Response Queues : " + tQueueName + " Progress " + ptProg + " %");

                        ptErrMsg = "";
                    }
                }
                

            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
            }
            finally
            {
                oFactory = null;
            }
        }

        public static void C_PRCxMQPublish(string ptQueueName, string ptMessage, out string ptErrMsg)
        {
            ConnectionFactory oFactory;
            cFunction oFunc = new cFunction();
            string tQueueName = ptQueueName;
            try
            {
                oFactory = new ConnectionFactory();
                oFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                oFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                oFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                oFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
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
            }
            finally
            {
                oFactory = null;
            }
        }

        /// <summary>
        /// *Arm 63-02-25
        /// Publish Message to Exchange
        /// </summary>
        /// <param name="ptExchange">Exchange Name</param>
        /// <param name="ptRoute">Routing</param>
        /// <param name="ptExchangeMode">direct,fanout,topic,headers</param>
        /// <param name="ptMessage">Message to send out</param>
        /// <param name="ptErrMsg">Error Message</param>
        
        //*Net 63-02-27 แก้ให้เหมือนกับ ver statdose
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
                oFactory.HostName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQHostName;
                oFactory.UserName = cMQReceiver.oC_Config.oC_RabbitMQ.tMQUserName;
                oFactory.Password = cMQReceiver.oC_Config.oC_RabbitMQ.tMQPassword;
                oFactory.VirtualHost = cMQReceiver.oC_Config.oC_RabbitMQ.tMQVirtualHost;
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
            }
            finally
            {
                oFactory = null;
            }
        }


        /// <summary>
        /// Clear Memory
        /// </summary>
        public void C_CLExMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception oEx)
            { }
        }
    }
}
