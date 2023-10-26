using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Reason
{
    public class cmlResRsnDwn
    {
        public List<cmlResInfoRsn> raRsn { get; set; }
        public List<cmlResInfoRsnLng> raRsnLng { get; set; }
        public List<cmlResInfoRsnGrpLng> raRsnGrpLng { get; set; }
    }
}
