using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Coupon
{
    public class cmlResInfoCpn
    {
        public string rtCpnCode { get; set; }
        public string rtCpnBarCode { get; set; }
        public Nullable<DateTime> rdCpnExpired { get; set; }
        public string rtCptCode { get; set; }
        public Nullable<double> rcCpnValue { get; set; }
        public Nullable<double> rcCpnSalePri { get; set; }
        public Nullable<double> rcCpnBalance { get; set; }
        public string rtCpnComBook { get; set; }
        public string rtCpnStaBook { get; set; }
        public string rtCpnStaSale { get; set; }
        public string rtCpnStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
