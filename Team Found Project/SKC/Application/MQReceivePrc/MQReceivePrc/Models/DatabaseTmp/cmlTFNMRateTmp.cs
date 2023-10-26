using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNMRateTmp
    {
        public string FTRteCode { get; set; }
        public Nullable<double> FCRteRate { get; set; }
        public Nullable<double> FCRteFraction { get; set; }
        public string FTRteType { get; set; }
        public string FTRteTypeChg { get; set; }
        public string FTRteSign { get; set; }
        public string FTRteStaLocal { get; set; }
        public string FTRteStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
