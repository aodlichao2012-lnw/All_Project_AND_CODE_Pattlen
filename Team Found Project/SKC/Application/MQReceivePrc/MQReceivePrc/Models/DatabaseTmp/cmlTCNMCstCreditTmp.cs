using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
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

        /// <summary>
        /// อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต (default)
        /// </summary>
        public string FTCstStaAlwPosCalSo { get; set; } //*Arm 63-02-20

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
