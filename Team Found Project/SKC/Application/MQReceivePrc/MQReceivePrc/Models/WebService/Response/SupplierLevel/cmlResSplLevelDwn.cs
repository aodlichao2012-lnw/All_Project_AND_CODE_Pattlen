using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SupplierLevel
{
    public class cmlResSplLevelDwn
    {
        public List<cmlResInfoSplLev> raSplLev { get; set; }
        public List<cmlResInfoSplLevLng> raSplLevLng { get; set; }
    }
}
