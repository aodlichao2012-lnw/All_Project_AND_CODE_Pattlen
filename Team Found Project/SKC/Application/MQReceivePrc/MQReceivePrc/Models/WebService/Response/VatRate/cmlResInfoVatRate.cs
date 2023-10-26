using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.VatRate
{
    public class cmlResInfoVatRate
    {
        public string rtVatCode { get; set; }
        public DateTime rdVatStart { get; set; }
        public Nullable<double> rcVatRate { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
