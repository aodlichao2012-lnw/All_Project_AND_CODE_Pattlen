using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.ProductNoSaleByEvent
{
    public class cmlResInfoPdtNoSleByEvn
    {
        public string rtEvnCode { get; set; }
        public int rnEvnSeqNo { get; set; }
        public string rtEvnType { get; set; }
        public string rtEvnStaAllDay { get; set; }
        public string rtEvnTStart { get; set; }
        public Nullable<DateTime> rdEvnDStart { get; set; }
        public string rtEvnTFinish { get; set; }
        public Nullable<DateTime> rdEvnDFinish { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
