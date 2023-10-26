using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductBrand
{
    public class cmlResPdtBrandDwn
    {
        public List<cmlResInfoPdtBrand> raPdtBrand { get; set; }
        public List<cmlResInfoPdtBrandLng> raPdtBrandLng { get; set; }
    }
}
