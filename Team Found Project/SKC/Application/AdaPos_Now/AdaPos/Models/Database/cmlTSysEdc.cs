using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTSysEdc
    {
        public string FTSedCode { get; set; }
        public string FTSedModel { get; set; }
        public Nullable<long> FNSedAck { get; set; }
        public string FTSedDllVer { get; set; }
        public Nullable<long> FNSedTimeOut { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
