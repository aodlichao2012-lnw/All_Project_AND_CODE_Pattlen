using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.BankDepositSlip
{
    public class cmlTFNTBnkDplDT
    {
        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่ใบนำฝากDef : XYYPOS-1234567 Gen ตาม TCNTAuto
        /// </summary>
        public string FTBdhDocNo { get; set; }

        /// <summary>
        ///ลำดับรายการ
        /// </summary>
        public Nullable<int> FNBddSeq { get; set; }

        /// <summary>
        ///ประเภทรายการฝาก 1:เงินสด 2:เช็ค
        /// </summary>
        public string FTBddType { get; set; }

        /// <summary>
        ///อ้างอิงเลขที่เช็ค
        /// </summary>
        public string FTBddRefNo { get; set; }

        /// <summary>
        ///ลงวันที่/จากวันที่
        /// </summary>
        public Nullable<DateTime> FDBddRefDate { get; set; }

        /// <summary>
        ///ยอดเงิน
        /// </summary>
        public Nullable<decimal> FCBddRefAmt { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }
    }
}
