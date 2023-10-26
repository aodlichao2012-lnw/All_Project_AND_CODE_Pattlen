using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Required
{
    public class cmlReqSyncData
    {
        public int pnSynSeqNo { get; set; }
        public string ptSynTable { get; set; }
        public Nullable<DateTime> pdSynLast { get; set; }
    }
}
