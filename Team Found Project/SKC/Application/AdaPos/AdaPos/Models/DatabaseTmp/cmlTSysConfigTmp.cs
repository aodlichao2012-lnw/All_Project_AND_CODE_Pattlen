using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTSysConfigTmp
    {
        public string FTSysCode { get; set; }
        public string FTSysApp { get; set; }
        public string FTSysKey { get; set; }
        public string FTSysSeq { get; set; }
        public string FTGmnCode { get; set; }
        public string FTSysStaAlwEdit { get; set; }
        public string FTSysStaDataType { get; set; }
        public Nullable<int> FNSysMaxLength { get; set; }
        public string FTSysStaDefValue { get; set; }
        public string FTSysStaDefRef { get; set; }
        public string FTSysStaUsrValue { get; set; }
        public string FTSysStaUsrRef { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
