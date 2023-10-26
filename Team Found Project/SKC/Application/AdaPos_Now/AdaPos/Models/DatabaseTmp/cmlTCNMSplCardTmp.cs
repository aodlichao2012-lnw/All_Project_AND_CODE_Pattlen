using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMSplCardTmp
    {
        public string FTSplCode { get; set; }
        public Nullable<DateTime> FDSplApply { get; set; }
        public string FTSplRefExCrdNo { get; set; }
        public Nullable<DateTime> FDSplCrdIssue { get; set; }
        public Nullable<DateTime> FDSplCrdExpire { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
