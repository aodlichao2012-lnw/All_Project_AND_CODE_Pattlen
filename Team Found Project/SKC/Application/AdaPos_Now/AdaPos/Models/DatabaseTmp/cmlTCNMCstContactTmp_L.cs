using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMCstContactTmp_L
    {
        public string FTCstCode { get; set; }
        public long FNLngID { get; set; }
        public long FNCtrSeq { get; set; }
        public string FTCtrName { get; set; }
        public string FTCtrFax { get; set; }
        public string FTCtrTel { get; set; }
        public string FTCtrEmail { get; set; }
        public string FTCtrRmk { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
