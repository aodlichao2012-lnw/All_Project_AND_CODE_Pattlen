using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Models
{
    public class cmlRabbitMQ
    {
        public string tHostName { get; set; }
        public string tHostPort { get; set; }
        public string tVirtual { get; set; }
        public string tUserName { get; set; }
        public string tPassword { get; set; }
        public string tQueueName { get; set; }
        public string tExchangeName { get; set; }
        public string tRoutingKey { get; set; }
        public string tMessage { get; set; }
        public string tMessageCount { get; set; }
        public bool bUseExchange { get; set; }
        public bool bAutoAck { get; set; }
        public bool bAutoDelete { get; set; }
        public bool bExclusive { get; set; }
        public bool bDurable { get; set; }
    }
}
