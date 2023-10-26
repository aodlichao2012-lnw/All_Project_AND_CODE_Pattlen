using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTSysPmtTmp
    {
        public string FTSpmCode { get; set; }
        public string FTSpmType { get; set; }
        public string FTSpmStaGrpBuy { get; set; }
        public string FTSpmStaBuy { get; set; }
        public string FTSpmStaGrpRcv { get; set; }
        public string FTSpmStaRcv { get; set; }
        public string FTSpmStaGrpBoth { get; set; }
        public string FTSpmStaGrpReject { get; set; }
        public string FTSpmStaAllPdt { get; set; }
        public string FTSpmStaExceptPmt { get; set; }
        public string FTSpmStaGetNewPri { get; set; }
        public string FTSpmStaGetDisAmt { get; set; }
        public string FTSpmStaGetDisPer { get; set; }
        public string FTSpmStaGetPoint { get; set; }
        public string FTSpmStaRcvFree { get; set; }
        public string FTSpmStaAlwOffline { get; set; }
        public string FTSpmStaChkLimitGet { get; set; }
        public string FTSpmStaChkCst { get; set; }
        public string FTSpmStaChkCstDOB { get; set; }
        public string FTSpmStaUseRange { get; set; }
        public Nullable<long> FNSpmLimitGrpRcv { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
