using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductNoSaleByEvent
{
    public class cmlResPdtNoSleByEvnDwn
    {
        public List<cmlResInfoPdtNoSleByEvn> raPdtNoSleByEvn { get; set; }
        public List<cmlResInfoPdtNoSleByEvnLng> raPdtNoSleByEvnLng { get; set; }
    }
}
