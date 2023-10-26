using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMPdtNoSleByEvnTmp
    {
        public string FTEvnCode { get; set; }
        public int FNEvnSeqNo { get; set; }
        public string FTEvnType { get; set; }
        public string FTEvnStaAllDay { get; set; }
        public string FTEvnTStart { get; set; }
        public Nullable<DateTime> FDEvnDStart { get; set; }
        public string FTEvnTFinish { get; set; }
        public Nullable<DateTime> FDEvnDFinish { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
