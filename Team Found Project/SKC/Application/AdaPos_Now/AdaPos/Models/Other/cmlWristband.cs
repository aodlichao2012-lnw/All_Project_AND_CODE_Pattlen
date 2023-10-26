using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
    public class cmlWristband
    {
        /// <summary>
        /// เหมายเลขบัตร
        /// </summary>
        public string tUID { get; set; }

        /// <summary>
        /// วันที่เปิดบัตร
        /// </summary>
        public Nullable<DateTime> dDateCreate { get; set; }

        /// <summary>
        /// เวลาเปิดบัตร
        /// </summary>
        public string tTimeCreate { get; set; }

        /// <summary>
        /// ยอดมัดจำ wristband
        /// </summary>
        public Nullable<decimal> cDeposit { get; set; }

        /// <summary>
        /// ยอดมัดจำสินค้า
        /// </summary>
        public Nullable<decimal> cDepositItem { get; set; }

        /// <summary>
        /// ยอดใช้ได้
        /// </summary>
        public Nullable<decimal> cAvailable { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ทำรายการ offline
        /// </summary>
        public int nTxnOffline { get; set; }

        /// <summary>
        /// วันที่อัพเดทข้อมูล Wristband
        /// </summary>
        public Nullable<DateTime> dDateUpdate { get; set; }

        /// <summary>
        /// เวลาอัพเดทข้อมูล Wristband
        /// </summary>
        public string tTimeUpdate { get; set; }

        /// <summary>
        /// วันที่เช่า จาก
        /// </summary>
        public Nullable<DateTime> dDateStart { get; set; }

        /// <summary>
        /// เวลาเช่า จาก
        /// </summary>
        public string tTimeStart { get; set; }

        /// <summary>
        /// วันที่เช่า ถึง *ชั่วคราวไม่มีการบันทึก (Locker)
        /// </summary>
        public Nullable<DateTime> dDateFinish { get; set; }

        /// <summary>
        /// เวลาเช่า ถึง *ชั่วคราวไม่มีการบันทึก (Locker)
        /// </summary>
        public string tTimeFinish { get; set; }

        /// <summary>
        /// Message Error
        /// </summary>
        public string tMsgError { get; set; }
    }
}
