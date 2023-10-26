using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SlipMsg
{
    public class cmlResSlipMsgDwn
    {
        public List<cmlResInfoSlipMsgHDLng> raSlipMsgHDLng { get; set; }
        public List<cmlResInfoSlipMsgDTLng> raSlipMsgDTLng { get; set; }
    }
}
