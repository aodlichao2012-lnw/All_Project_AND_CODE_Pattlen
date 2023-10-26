using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Supplier
{
    public class cmlResInfoSplCredit
    {
        public string rtSplCode { get; set; }
        public Nullable<long> rnSplCrTerm { get; set; }
        public Nullable<double> rcSplCrLimit { get; set; }
        public string rtSplDayCta { get; set; }
        public Nullable<DateTime> rdSplLastCta { get; set; }
        public Nullable<DateTime> rdSplLastPay { get; set; }
        public Nullable<long> rnSplLimitRow { get; set; }
        public Nullable<double> rcSplLeadTime { get; set; }
        public string rtSplViaRmk { get; set; }
        public string rtViaCode { get; set; }
        public string rtSplTspPaid { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
