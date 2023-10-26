using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Supplier
{
    public class cmlResSplItemDwn
    {
        public List<cmlResInfoSpl> raSpl { get; set; }
        public List<cmlResInfoSplLng> raSplLng { get; set; }
        public List<cmlResInfoSplCard> raSplCard { get; set; }
        public List<cmlResInfoSplCredit> raSplCredit { get; set; }
    }
}
