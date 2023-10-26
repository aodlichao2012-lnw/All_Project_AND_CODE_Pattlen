using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.AdMsg
{
    public class cmlResInfoAdvMsg
    {
        public string rtAdvCode { get; set; }
        public string rtAdvType { get; set; }
        public Nullable<int> rnAdvSeqNo { get; set; }
        public string rtAdvStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
        public Nullable<DateTime> rdAdvStart { get; set; }
        public Nullable<DateTime> rdAdvStop { get; set; }
    }
}
