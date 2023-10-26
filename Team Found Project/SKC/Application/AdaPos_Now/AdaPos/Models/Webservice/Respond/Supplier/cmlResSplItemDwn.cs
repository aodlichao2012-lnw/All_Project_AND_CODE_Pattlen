using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Supplier
{
    public class cmlResSplItemDwn
    {
        public List<cmlResInfoSpl> raSpl { get; set; }
        public List<cmlResInfoSplLng> raSplLng { get; set; }
        public List<cmlResInfoSplCard> raSplCard { get; set; }
        public List<cmlResInfoSplCredit> raSplCredit { get; set; }
    }
}
