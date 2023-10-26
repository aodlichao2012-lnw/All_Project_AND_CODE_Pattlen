using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNMCardTmp
    {
        public string FTCrdCode { get; set; }
        public Nullable<DateTime> FDCrdStartDate { get; set; }
        public Nullable<DateTime> FDCrdExpireDate { get; set; }
        public Nullable<DateTime> FDCrdResetDate { get; set; }
        public Nullable<DateTime> FDCrdLastTopup { get; set; }
        public string FTCtyCode { get; set; }
        public Nullable<double> FCCrdValue { get; set; }
        public Nullable<double> FCCrdDeposit { get; set; }
        public string FTCrdHolderID { get; set; }
        public string FTCrdRefID { get; set; }
        public string FTCrdStaType { get; set; }
        public string FTCrdStaLocate { get; set; }
        public string FTCrdStaActive { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

        /// <summary>
        ///รหัสแผนก
        /// </summary>
        public string FTDptCode { get; set; }                   //*Arm 63-01-30

        /// <summary>
        ///มูลค่ายอดมัดจำสินค้า
        /// </summary>
        public Nullable<decimal> FCCrdDepositPdt { get; set; }  //*Arm 63-01-30

        /// <summary>
        ///สถานะเบิกบัตร 1 : ยังไม่ถูกเบิก, 2 : เบิกไปแล้ว
        /// </summary>
        public string FTCrdStaShift { get; set; }               //*Arm 63-01-30

        /// <summary>
        ///จำนวน Transaction ทำรายการ offline (ได้จาก wrishband)
        /// </summary>
        public Nullable<Int64> FNCrdTxnOffline { get; set; }    //*Arm 63-01-30

        /// <summary>
        ///จำนวน Transaction ที่ ประมวลผลตัดยอดแล้ว  (ได้จาก การประมวลผลตัดยอดที่ละรายการ) 
        /// </summary>
        public Nullable<Int64> FNCrdTxnPrcAdj { get; set; }     //*Arm 63-01-30
    }
}
