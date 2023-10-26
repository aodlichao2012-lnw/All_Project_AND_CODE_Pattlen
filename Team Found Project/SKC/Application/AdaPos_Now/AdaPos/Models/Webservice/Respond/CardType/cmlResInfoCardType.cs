using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.CardType
{
    public class cmlResInfoCardType
    {
        public string rtCtyCode { get; set; }
        public Nullable<double> rcCtyDeposit { get; set; }
        public Nullable<double> rcCtyTopupAuto { get; set; }
        public Nullable<long> rnCtyExpirePeriod { get; set; }
        public Nullable<int> rnCtyExpiredType { get; set; }
        public string rtCtyStaAlwRet { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
