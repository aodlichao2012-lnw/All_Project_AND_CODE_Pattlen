using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMAdMsgTmp
    {
        public string FTAdvCode { get; set; }
        public string FTAdvType { get; set; }
        public Nullable<int> FNAdvSeqNo { get; set; }
        public string FTAdvStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }
        public Nullable<DateTime> FDAdvStart { get; set; }
        public Nullable<DateTime> FDAdvStop { get; set; }

    }
}
