using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductUnit
{
    public class cmlResPdtUnitDwn
    {
        public List<cmlResInfoPdtUnit> raPdtUnit { get; set; }
        public List<cmlResInfoPdtUnitLng> raPdtUnitLng { get; set; }
    }
}
