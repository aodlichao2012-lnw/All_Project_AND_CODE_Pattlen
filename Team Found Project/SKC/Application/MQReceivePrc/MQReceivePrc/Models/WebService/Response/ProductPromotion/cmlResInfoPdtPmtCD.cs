using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductPromotion
{
    public class cmlResInfoPdtPmtCD
    {
        public string rtBchCode { get; set; }
        public string rtPmhCode { get; set; }
        public long rnPmcSeq { get; set; }
        public string rtSpmCode { get; set; }
        public string rtPmcGrpName { get; set; }
        public string rtPmcStaGrpCond { get; set; }
        public Nullable<double> rcPmcPerAvgDis { get; set; }
        public Nullable<double> rcPmcBuyAmt { get; set; }
        public Nullable<double> rcPmcBuyQty { get; set; }
        public Nullable<double> rcPmcBuyMinQty { get; set; }
        public Nullable<double> rcPmcBuyMaxQty { get; set; }
        public Nullable<DateTime> rdPmcBuyMinTime { get; set; }
        public Nullable<DateTime> rdPmcBuyMaxTime { get; set; }
        public Nullable<double> rcPmcGetCond { get; set; }
        public Nullable<double> rcPmcGetValue { get; set; }
        public Nullable<double> rcPmcGetQty { get; set; }
        public string rtSpmStaBuy { get; set; }
        public string rtSpmStaRcv { get; set; }
        public string rtSpmStaAllPdt { get; set; }
        public string rtPmcGrpCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
