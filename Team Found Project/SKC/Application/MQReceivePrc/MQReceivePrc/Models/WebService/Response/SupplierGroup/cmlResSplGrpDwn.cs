using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SupplierGroup
{
    public class cmlResSplGrpDwn
    {
        public List<cmlResInfoSplGrp> raSplGrp { get; set; }
        public List<cmlResInfoSplGrpLng> raSplGrpLng { get; set; }
    }
}
