using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMCstCreditTmp
    {
        public string FTCstCode { get; set; }
        public Nullable<long> FNCstCrTerm { get; set; }
        public Nullable<double> FCCstCrLimit { get; set; }
        public string FTCstStaAlwOrdSun { get; set; }
        public string FTCstStaAlwOrdMon { get; set; }
        public string FTCstStaAlwOrdTue { get; set; }
        public string FTCstStaAlwOrdWed { get; set; }
        public string FTCstStaAlwOrdThu { get; set; }
        public string FTCstStaAlwOrdFri { get; set; }
        public string FTCstStaAlwOrdSat { get; set; }
        public Nullable<DateTime> FDCstLastCta { get; set; }
        public Nullable<DateTime> FDCstLastPay { get; set; }
        public string FTCstPayRmk { get; set; }
        public string FTCstBillRmk { get; set; }
        public Nullable<long> FNCstViaTime { get; set; }
        public string FTCstViaRmk { get; set; }
        public string FTViaCode { get; set; }
        public string FTCstTspPaid { get; set; }
        public string FTCstStaApv { get; set; }

    }
}
