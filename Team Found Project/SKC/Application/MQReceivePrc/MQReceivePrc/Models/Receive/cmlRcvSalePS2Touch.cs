using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cmlRcvSalePS2Touch
    {
        public string ptFunction { get; set; }
        public string ptSource { get; set; }
        public string ptDest { get; set; }
        public cmlData ptData { get; set; }
    }
    public class cmlData
    {
      public string ptDocNo { get; set; }
    }
}
