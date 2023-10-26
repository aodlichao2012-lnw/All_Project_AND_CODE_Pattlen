using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CreditCard
{
    public class cmlResInfoCreditCard
    {
        public string rtCrdCode { get; set; }
        public string rtBnkCode { get; set; }
        public Nullable<double> rcCrdChgPer { get; set; }
        public string rtCrdCrdFmt { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
