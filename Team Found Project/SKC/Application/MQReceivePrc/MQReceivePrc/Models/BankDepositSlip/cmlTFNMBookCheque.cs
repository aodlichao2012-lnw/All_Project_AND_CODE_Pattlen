using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.BankDepositSlip
{
    public class cmlTFNMBookCheque
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่สมุดเช็ค
        /// </summary>
        public string FTChqCode { get; set; }

        /// <summary>
        ///รหัสสมุดบัญชี
        /// </summary>
        public string FTBbkCode { get; set; }

        /// <summary>
        ///เลขที่เช็คต่ำสุด
        /// </summary>
        public Nullable<Int64> FNChqMin { get; set; }

        /// <summary>
        ///เลขที่เช็คสูงสุด
        /// </summary>
        public Nullable<Int64> FNChqMax { get; set; }

        /// <summary>
        ///สถานะการใช้งาน 1: ใช้งาน
        /// </summary>
        public string FTChqStaAct { get; set; }

        /// <summary>
        ///สถานะอนุมัติ 1: อนุมัติแล้ว  ว่าง null ยังไม่อนุมัติ
        /// </summary>
        public string FTChqStaPrcDoc { get; set; }

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
