using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductGroup
{
    public class cmlResPdtGrpDwn
    {
        public List<cmlResInfoPdtGrp> raPdtGrp { get; set; }
        public List<cmlResInfoPdtGrpLng> raPdtGrpLng { get; set; }
    }
}
