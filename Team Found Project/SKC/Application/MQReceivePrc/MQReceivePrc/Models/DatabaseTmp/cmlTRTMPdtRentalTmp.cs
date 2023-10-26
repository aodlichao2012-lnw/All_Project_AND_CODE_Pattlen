using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMPdtRentalTmp
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ประเภทสินค้าเช่า 1: สินค้าทั่วไป 2: Locker (ShopLayOut)
        /// <summary>
        public string FTPdtRentType { get; set; }

        /// <summary>
        ///สถานะคืนสินค้า 1: ต้องทำคืน 2:ไม่ต้องทำคืน
        /// <summary>
        public string FTPdtStaReqRet { get; set; }

        /// <summary>
        ///ระยะวลาให้เช่า 1:ไม่ระบุ 2:รายชั่วโมง 3:รายวัน 4:รายเดือน 5:รายปี 6: Custom(days)
        /// <summary>
        public string FTPdtRentCond { get; set; }

        /// <summary>
        ///สถานะการชำระ 1:จ่ายขั้นต่ำตอนเช่า 2: จ่ายตอนคืน
        /// <summary>
        public string FTPdtStaPay { get; set; }

        /// <summary>
        ///ค่ามัดจำ
        /// <summary>
        public Nullable<decimal> FCPdtDeposit { get; set; }

        /// <summary>
        ///ค่าปรับ ต่อหน่วยเล็กสุด
        /// <summary>
        public Nullable<decimal> FCPdtFee { get; set; }

        /// <summary>
        ///ตู้Locker//model 1:1  บังคับกรณี FTPdtRentType=2
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }
    }
}
