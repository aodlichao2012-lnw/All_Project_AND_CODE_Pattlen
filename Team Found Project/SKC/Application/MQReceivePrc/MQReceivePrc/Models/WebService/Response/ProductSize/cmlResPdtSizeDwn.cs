using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductSize
{
    public class cmlResPdtSizeDwn
    {
        public List<cmlResInfoPdtSize> raPdtSize { get; set; }
        public List<cmlResInfoPdtSizeLng> raPdtSizeLng { get; set; }
    }
}
