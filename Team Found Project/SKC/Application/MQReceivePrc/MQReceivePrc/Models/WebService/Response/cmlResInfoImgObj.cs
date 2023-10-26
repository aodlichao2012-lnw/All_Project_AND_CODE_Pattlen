using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response
{
    public class cmlResInfoImgObj
    {
        public long rnImgID { get; set; }
        public string rtImgRefID { get; set; }
        public int rnImgSeq { get; set; }
        public string rtImgTable { get; set; }
        public string rtImgKey { get; set; }
        public string rtImgObj { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
