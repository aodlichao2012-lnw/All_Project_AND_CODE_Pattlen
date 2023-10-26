using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CustomerOccupation
{
    public class cmlResCstOcpDwn
    {
        public List<cmlResInfoCstOcp> raCstOcp { get; set; }
        public List<cmlResInfoCstOcpLng> raCstOcpLng { get; set; }
    }
}
