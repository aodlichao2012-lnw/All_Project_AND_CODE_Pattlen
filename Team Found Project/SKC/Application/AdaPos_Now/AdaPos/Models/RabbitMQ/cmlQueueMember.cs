using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.RabbitMQ
{
    class cmlQueueMember
    {
        public string ptFunction { get; set; }
        public string ptSource { get; set; }
        public string ptDest { get; set; }
        public string ptFilter { get; set; }
        public cmlCustomer ptCustomer { get; set; }
        
    }
    public class cmlCustomer
    {
        public string ptFTCstCode { get; set; }
        public string ptFTCstName { get; set; }
        public string ptFTCstTel { get; set; }
        public string ptFTCstPriGrp { get; set; }
    }
}
