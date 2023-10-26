using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CustomerLevel
{
    public class cmlResCstLevDwn
    {
        public List<cmlResInfoCstLev> raCstLev { get; set; }
        public List<cmlResInfoCstLevLng> raCstLevLng { get; set; }
    }
}
