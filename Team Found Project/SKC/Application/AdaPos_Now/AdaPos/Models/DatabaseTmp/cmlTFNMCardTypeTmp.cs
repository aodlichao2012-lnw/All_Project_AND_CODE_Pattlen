using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTFNMCardTypeTmp
    {
        public string FTCtyCode { get; set; }
        public Nullable<double> FCCtyDeposit { get; set; }
        public Nullable<double> FCCtyTopupAuto { get; set; }
        public Nullable<long> FNCtyExpirePeriod { get; set; }
        public Nullable<int> FNCtyExpiredType { get; set; }
        public string FTCtyStaAlwRet { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
