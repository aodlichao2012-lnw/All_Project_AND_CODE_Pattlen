using AdaTask.Class;
using AdaTask.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Event
{
    class cEvent4
    {
        /// <summary>
        /// send mail (*Arm 63-07-06)
        /// </summary>
        public void C_PROxReqSendMailErrExpSale()
        {
            new cLog().C_WRTxLog("cEvent1", "C_PROxReqSendMailErrExpSale : RabbitPublish");
            string tQueueName = "LK_QSale2Mail";
            try
            {
                Console.WriteLine("cEvent4 > C_PROxReqSendMailErrExpSale : Send an ExportSell error notification");
                new cLog().C_WRTxLog("cEvent4", "C_PROxReqSendMailErrExpSale : Send an ExportSell error notification ");

                cmlReqSendMail oReq = new cmlReqSendMail();
                oReq.ptFunction = "MailNoti";
                oReq.ptSource = "AdaTask";
                oReq.ptDest = "MQAdaLink";
                string tJson = JsonConvert.SerializeObject(oReq);
                
                new cSP().C_PRCxMQPublish(tQueueName, tJson, cVB.tVB_VHostSale);
                new cLog().C_WRTxLog("cEvent4", "C_PROxReqSendMailErrExpSale : Send VisualHost/Queue Name: " + cVB.tVB_VHostSale + "/"+ tQueueName);
                Console.WriteLine("cEvent4 > C_PROxReqSendMailErrExpSale : Send an ExportSell error notification Success");
                new cLog().C_WRTxLog("cEvent4", "C_PROxReqSendMailErrExpSale : Send an ExportSell error notification Success");
            }
            catch(Exception oEx)
            {
                Console.WriteLine("cEvent4 > C_PROxReqSendMailErrExpSale : " + oEx.Message.ToString());
                new cLog().C_WRTxLog("cEvent4", "C_PROxReqSendMailErrExpSale : " + oEx.Message.ToString());
            }
        }
    }
}
