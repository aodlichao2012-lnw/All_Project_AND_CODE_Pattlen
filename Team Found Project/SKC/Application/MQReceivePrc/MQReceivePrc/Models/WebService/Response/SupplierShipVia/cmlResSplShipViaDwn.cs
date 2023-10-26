using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SupplierShipVia
{
    public class cmlResSplShipViaDwn
    {
        public List<cmlResInfoSplShipVia> raSplShipVia { get; set; }
        public List<cmlResInfoSplShipViaLng> raSplShipViaLng { get; set; }
    }
}
