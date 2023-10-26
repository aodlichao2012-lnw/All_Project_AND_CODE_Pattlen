using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductFashionSeason
{
    public class cmlResInfoPdtSeason
    {
        public string rtPgpCode { get; set; }
        public long rnPgpLevel { get; set; }
        public string rtPgpParent { get; set; }
        public string rtPgpChain { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
