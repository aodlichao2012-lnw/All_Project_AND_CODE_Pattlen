using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CardType
{
    public class cmlResInfoCardType
    {
        public string rtCtyCode { get; set; }
        public Nullable<double> rcCtyDeposit { get; set; }
        public Nullable<double> rcCtyTopupAuto { get; set; }
        public Nullable<long> rnCtyExpirePeriod { get; set; }
        public Nullable<int> rnCtyExpiredType { get; set; }
        public string rtCtyStaAlwRet { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }

        /// <summary>
        ///สถานะการชำระ 1:เติมเงินก่อน 2: จ่ายทีหลัง
        /// </summary>
        public string rtCtyStaPay { get; set; }                 //*Arm 63-01-30

        /// <summary>
        ///วงเงิน
        /// </summary>
        public Nullable<decimal> rcCtyCreditLimit { get; set; } //*Arm 63-01-30
    }
}
