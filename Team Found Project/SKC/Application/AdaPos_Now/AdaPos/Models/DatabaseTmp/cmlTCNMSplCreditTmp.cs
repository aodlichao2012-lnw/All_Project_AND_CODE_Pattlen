using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMSplCreditTmp
    {
        public string FTSplCode { get; set; }
        public Nullable<long> FNSplCrTerm { get; set; }
        public Nullable<double> FCSplCrLimit { get; set; }
        public string FTSplDayCta { get; set; }
        public Nullable<DateTime> FDSplLastCta { get; set; }
        public Nullable<DateTime> FDSplLastPay { get; set; }
        public Nullable<long> FNSplLimitRow { get; set; }
        public Nullable<double> FCSplLeadTime { get; set; }
        public string FTSplViaRmk { get; set; }
        public string FTViaCode { get; set; }
        public string FTSplTspPaid { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
