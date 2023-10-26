using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTFNMCrdCpnListTmp
    {
        public string FTCclCode { get; set; }
        public Nullable<double> FCCclAmt { get; set; }
        public Nullable<DateTime> FDCclStartDate { get; set; }
        public Nullable<DateTime> FDCclEndDate { get; set; }
        public string FTCclStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
