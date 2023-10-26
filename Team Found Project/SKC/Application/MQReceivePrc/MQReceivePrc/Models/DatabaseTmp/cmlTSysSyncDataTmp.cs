using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTSysSyncDataTmp
    {
        public int FNSynSeqNo { get; set; }
        public string FTSynGroup { get; set; }
        public string FTSynTable { get; set; }
        public string FTSynTable_L { get; set; }
        public string FTSynType { get; set; }
        public Nullable<DateTime> FDSynLast { get; set; }
        public Nullable<int> FNSynSchedule { get; set; }
        public string FTSynStaUse { get; set; }
        public string FTSynUriDwn { get; set; }
        public string FTSynUriUld { get; set; }

    }
}
