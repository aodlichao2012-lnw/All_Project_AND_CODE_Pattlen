using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Customer
{
    public class cmlResInfoCstCredit
    {
        public string rtCstCode { get; set; }
        public Nullable<long> rnCstCrTerm { get; set; }
        public Nullable<double> rcCstCrLimit { get; set; }
        public string rtCstStaAlwOrdSun { get; set; }
        public string rtCstStaAlwOrdMon { get; set; }
        public string rtCstStaAlwOrdTue { get; set; }
        public string rtCstStaAlwOrdWed { get; set; }
        public string rtCstStaAlwOrdThu { get; set; }
        public string rtCstStaAlwOrdFri { get; set; }
        public string rtCstStaAlwOrdSat { get; set; }

        /// <summary>
        /// อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต (default)
        /// </summary>
        public string rtCstStaAlwPosCalSo { get; set; } //*Arm 63-02-20

        public Nullable<DateTime> rdCstLastCta { get; set; }
        public Nullable<DateTime> rdCstLastPay { get; set; }
        public string rtCstPayRmk { get; set; }
        public string rtCstBillRmk { get; set; }
        public Nullable<long> rnCstViaTime { get; set; }
        public string rtCstViaRmk { get; set; }
        public string rtViaCode { get; set; }
        public string rtCstTspPaid { get; set; }
        public string rtCstStaApv { get; set; }
    }
}
