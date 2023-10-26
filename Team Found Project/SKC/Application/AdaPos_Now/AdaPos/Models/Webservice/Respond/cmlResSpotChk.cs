using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond
{
    public class cmlResSpotChk
    {
        /// <summary>
        /// ยอดทั้งหมด
        /// </summary>
        public Nullable<decimal> rcTxnValue { get; set; }

        /// <summary>
        /// เงินมัดจำบัตร
        /// </summary>
        public Nullable<decimal> rcCrdDeposit { get; set; }

        /// <summary>
        /// ยอดใช้ได้
        /// </summary>
        public Nullable<decimal> rcTxnValueAvb { get; set; }

        /// <summary>
        /// ประเภทบัตร
        /// </summary>
        public string rtCtyName { get; set; }

        /// <summary>
        /// วันที่บัตรหมดอายุ
        /// </summary>
        public Nullable<DateTime> rdCrdExpireDate { get; set; }

        /// <summary>
        /// ชื่อบัตร
        /// </summary>
        public string rtCrdName { get; set; }

        /// <summary>
        /// เงินมัดจำสินค้า
        /// </summary>
        public Nullable<decimal> rcCrdDepositPdt { get; set; }

        /// <summary>
        /// วันที่เติมเงินล่าสุด
        /// </summary>
        public Nullable<DateTime> rdCrdLastTopup { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ใช้งาน offline จาก บัตร/wristband
        /// </summary>
        public Nullable<int> rnTxnOffline { get; set; }

        /// <summary>
        /// System Process Status
        /// 1 : Success
        /// 701 : Validate parameter model false
        /// 800 : Not found card
        /// 802 : format data in correct
        /// 900 : service process false
        /// 904 : Key not allowed to use method
        /// 905 : Cannot connect database
        /// 906 : This time not allowed to use method
        /// </summary>
        public string rtCode { get; set; }

        /// <summary>
        /// System Process Description
        /// </summary>
        public string rtDesc { get; set; }

        /// <summary>
        /// สถานะบัตร 1: เคลื่อนไหว, 2: ไม่เคลื่อนไหว, 3: ยกเลิก
        /// </summary>
        public string rtCrdStaActive { get; set; }

        /// <summary>
        /// ประเภทหมดอายุบัตร 1:hour 2:day 3:month 4:year
        /// </summary>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-04-07] - add new property.
        /// </remarks>
        public int rnCtyExpiredType { get; set; }

        /// <summary>
        /// จำนวนอายุ บัตร
        /// </summary>
        ///         
        /// <remarks>
        /// *[AnUBiS][][2019-04-07] - add new property.
        /// </remarks>
        public int rnCtyExpirePeriod { get; set; }

    }
}
