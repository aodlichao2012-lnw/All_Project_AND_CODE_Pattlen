using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.BankDepositSlip
{
    public class cmlTFNMBookBank
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสสมุดบัญชี
        /// </summary>
        public string FTBbkCode { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///ประเภทสมุด 1:ออมทรัพย์,2:กระแสรายวัน,3:ประจำ
        /// </summary>
        public string FTBbkType { get; set; }

        /// <summary>
        ///เลขที่บัญชี
        /// </summary>
        public string FTBbkAccNo { get; set; }

        /// <summary>
        ///รหัสธนาคาร
        /// </summary>
        public string FTBnkCode { get; set; }

        /// <summary>
        ///วันที่เปิดบัญชี
        /// </summary>
        public Nullable<DateTime> FDBbkOpen { get; set; }

        /// <summary>
        ///ยอดคงเหลือล่าสุด
        /// </summary>
        public Nullable<decimal> FCBbkBalance { get; set; }

        /// <summary>
        ///ปรับปรุงล่าสุด ณ. วันที่
        /// </summary>
        public Nullable<DateTime> FDBbkUpd { get; set; }

        /// <summary>
        ///สถานะการใช้งานของเลขที่บัญชี  1:ใช้งาน 2 :ไม่ใช้งาน
        /// </summary>
        public string FTBbkStaActive { get; set; }

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
