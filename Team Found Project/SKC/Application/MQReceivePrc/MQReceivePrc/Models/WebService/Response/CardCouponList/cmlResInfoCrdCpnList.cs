using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CardCouponList
{
    public class cmlResInfoCrdCpnList
    {
        public string rtCclCode { get; set; }
        public Nullable<double> rcCclAmt { get; set; }
        public Nullable<DateTime> rdCclStartDate { get; set; }
        public Nullable<DateTime> rdCclEndDate { get; set; }
        public string rtCclStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
