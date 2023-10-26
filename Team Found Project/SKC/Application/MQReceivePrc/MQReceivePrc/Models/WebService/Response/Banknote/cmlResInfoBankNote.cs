using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Banknote
{
    public class cmlResInfoBankNote
    {
        public string rtRteCode { get; set; }
        public string rtBntCode { get; set; }
        public string rtBntStaShw { get; set; }
        public Nullable<double> rcBntRateAmt { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
