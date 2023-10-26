using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopLayoutTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มร้านค้า
        /// <summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///ลำดับช่อง
        /// <summary>
        public Nullable<Int64> FNLayNo { get; set; }

        /// <summary>
        ///สัดส่วน ขนาด Layout  แนวนอน
        /// <summary>
        public Nullable<Int64> FNLayScaleX { get; set; }

        /// <summary>
        ///สัดส่วน ขนาด Layout แนวตั้ง
        /// <summary>
        public Nullable<Int64> FNLayScaleY { get; set; }

        /// <summary>
        ///ชั้นที่
        /// <summary>
        public Nullable<Int64> FNLayRow { get; set; }

        /// <summary>
        ///คอลัมน์
        /// <summary>
        public Nullable<Int64> FNLayCol { get; set; }

        /// <summary>
        ///รหัส ขนาด เพื่อแยกราคา
        /// <summary>
        public string FTPzeCode { get; set; }

        /// <summary>
        ///รหัส rack/ตู้ /กลุ่ม
        /// <summary>
        public string FTRakCode { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// <summary>
        public string FTLayStaUse { get; set; }

        /// <summary>
        ///วันที่ Update ล่าสุด
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
        ///วันที่สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }
    }
}
