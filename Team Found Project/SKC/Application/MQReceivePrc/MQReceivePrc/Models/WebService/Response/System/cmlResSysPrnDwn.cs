using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.System
{
    public class cmlResSysPrnDwn
    {
        public List<cmlResInfoSysPrn> raSysPrn { get; set; }
        public List<cmlResInfoSysPrnLng> raSysPrnLng { get; set; }
    }
}
