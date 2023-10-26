using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Voucher
{
    public class cmlResInfoVch
    {
        public string rtVocCode { get; set; }
        public string rtVocBarCode { get; set; }
        public Nullable<DateTime> rdVocExpired { get; set; }
        public string rtVotCode { get; set; }
        public Nullable<double> rcVocValue { get; set; }
        public Nullable<double> rcVocSalePri { get; set; }
        public Nullable<double> rcVocBalance { get; set; }
        public string rtVocComBook { get; set; }
        public string rtVocStaBook { get; set; }
        public string rtVocStaSale { get; set; }
        public string rtVocStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
