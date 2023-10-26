using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Sync
{
    public class cmlSyncHistory
    {
        public string FNSynSeqNo { get; set; }
        public string FTSynTable { get; set; }  //*Em 62-09-04
        public string FTSynTable_L { get; set; }
        public string FDSynLast { get; set; }
        public string FTSynUriDwn { get; set; }
        public string FTSynUriUld { get; set; }
        public string FTSynName { get; set; }
    }
}
