using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Customer
{
    public class cmlResInfoCstCard
    {
        public string rtCstCode { get; set; }
        public Nullable<DateTime> rdCstApply { get; set; }
        public string rtCstCrdNo { get; set; }
        public string rtBchCode { get; set; }
        public Nullable<DateTime> rdCstCrdIssue { get; set; }
        public Nullable<DateTime> rdCstCrdExpire { get; set; }
        public string rtCstStaAge { get; set; }
    }
}
