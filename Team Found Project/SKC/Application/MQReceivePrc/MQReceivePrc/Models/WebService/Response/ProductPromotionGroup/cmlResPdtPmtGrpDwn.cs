using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductPromotionGroup
{
    public class cmlResPdtPmtGrpDwn
    { 
        public List<cmlResInfoPdtPmtGrp> raPdtPmtGrp { get; set; }
        public List<cmlResInfoPdtPmtGrpLng> raPdtPmtGrpLng { get; set; }
    }
}
