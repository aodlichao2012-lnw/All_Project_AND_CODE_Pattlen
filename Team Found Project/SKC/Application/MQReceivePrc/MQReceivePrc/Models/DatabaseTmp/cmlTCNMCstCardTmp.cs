using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCstCardTmp
    {
        public string FTCstCode { get; set; }
        public Nullable<DateTime> FDCstApply { get; set; }
        public string FTCstCrdNo { get; set; }
        public string FTBchCode { get; set; }
        public Nullable<DateTime> FDCstCrdIssue { get; set; }
        public Nullable<DateTime> FDCstCrdExpire { get; set; }
        public string FTCstStaAge { get; set; }

    }
}
