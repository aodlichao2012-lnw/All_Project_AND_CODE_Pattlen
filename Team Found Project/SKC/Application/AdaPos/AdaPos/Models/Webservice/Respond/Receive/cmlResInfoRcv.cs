using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Receive
{
    public class cmlResInfoRcv
    {
        public string rtRcvCode { get; set; }
        public string rtFmtCode { get; set; }
        public string rtRcvStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
