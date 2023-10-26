using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTFNMRate
    {
        public string FTRteCode { get; set; }
        public Nullable<decimal> FCRteRate { get; set; }
        public Nullable<decimal> FCRteFraction { get; set; }
        public string FTRteType { get; set; }
        public string FTRteTypeChg { get; set; }
        public string FTRteSign { get; set; }
        public string FTRteStaLocal { get; set; }
        public string FTRteStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }
        public int FNLngID { get; set; }
        public string FTRteName { get; set; }
        public string FTRteShtName { get; set; }
        public string FTRteNameText { get; set; }
        public string FTRteDecText { get; set; }
    }
}
