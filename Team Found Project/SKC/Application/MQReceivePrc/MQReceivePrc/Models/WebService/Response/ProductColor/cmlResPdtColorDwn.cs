using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductColor
{
    public class cmlResPdtColorDwn
    {
        public List<cmlResInfoPdtColor> raPdtColor { get; set; }
        public List<cmlResInfoPdtColorLng> raPdtColorLng { get; set; }
    }
}
