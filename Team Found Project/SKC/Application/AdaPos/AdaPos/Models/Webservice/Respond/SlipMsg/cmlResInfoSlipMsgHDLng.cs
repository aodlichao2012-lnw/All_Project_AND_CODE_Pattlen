using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.SlipMsg
{
    public class cmlResInfoSlipMsgHDLng
    {
        public string rtSmgCode { get; set; }
        public long rnLngID { get; set; }
        public string rtSmgTitle { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
