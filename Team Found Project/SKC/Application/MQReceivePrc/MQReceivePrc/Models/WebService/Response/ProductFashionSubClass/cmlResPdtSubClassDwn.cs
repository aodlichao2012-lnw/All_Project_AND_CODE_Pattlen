using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductFashionSubClass
{
    public class cmlResPdtSubClassDwn
    {
        public List<cmlResInfoPdtSubClass> raPdtSubClass { get; set; }
        public List<cmlResInfoPdtSubClassLng> raPdtSubClassLng { get; set; }
    }
}
