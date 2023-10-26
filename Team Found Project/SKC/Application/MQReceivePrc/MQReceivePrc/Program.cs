using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MQReceivePrc.Class;
using MQReceivePrc.Models;
using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Receive;
using Standard;
using System.Windows.Forms;

namespace MQReceivePrc
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                cConsole.DisableQuickEdit(); //*Net 63-08-18 ป้องกันไม่ให้คลิกเมาส์
                cVB.tVB_UniqueTimeCre = DateTime.Now.ToString("yyyyMMddHHmmssfff"); //*Net 63-09-02 วันเวลาที่เปิดโปรแกรม
                while (true) //*Net 63-09-02 สำหรับ Restart
                {
                    new cMQReceiver().C_MQRxProcess();
                }
            }
            catch (Exception)
            {

            }
        }

    }

}
