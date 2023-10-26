using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.System
{
    public class cmlResInfoSysPmt
    {
        public string rtSpmCode { get; set; }
        public string rtSpmType { get; set; }
        public string rtSpmStaGrpBuy { get; set; }
        public string rtSpmStaBuy { get; set; }
        public string rtSpmStaGrpRcv { get; set; }
        public string rtSpmStaRcv { get; set; }
        public string rtSpmStaGrpBoth { get; set; }
        public string rtSpmStaGrpReject { get; set; }
        public string rtSpmStaAllPdt { get; set; }
        public string rtSpmStaExceptPmt { get; set; }
        public string rtSpmStaGetNewPri { get; set; }
        public string rtSpmStaGetDisAmt { get; set; }
        public string rtSpmStaGetDisPer { get; set; }
        public string rtSpmStaGetPoint { get; set; }
        public string rtSpmStaRcvFree { get; set; }
        public string rtSpmStaAlwOffline { get; set; }
        public string rtSpmStaChkLimitGet { get; set; }
        public string rtSpmStaChkCst { get; set; }
        public string rtSpmStaChkCstDOB { get; set; }
        public string rtSpmStaUseRange { get; set; }
        public Nullable<long> rnSpmLimitGrpRcv { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
