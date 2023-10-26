using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNMCouponTmp
    {
        public string FTCpnCode { get; set; }
        public string FTCpnBarCode { get; set; }
        public Nullable<DateTime> FDCpnExpired { get; set; }
        public string FTCptCode { get; set; }
        public Nullable<double> FCCpnValue { get; set; }
        public Nullable<double> FCCpnSalePri { get; set; }
        public Nullable<double> FCCpnBalance { get; set; }
        public string FTCpnComBook { get; set; }
        public string FTCpnStaBook { get; set; }
        public string FTCpnStaSale { get; set; }
        public string FTCpnStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
