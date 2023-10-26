using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopLayout
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มร้านค้า
        /// <summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///ลำดับช่อง
        /// <summary>
        public Nullable<Int64> rnLayNo { get; set; }

        /// <summary>
        ///สัดส่วน ขนาด Layout  แนวนอน
        /// <summary>
        public Nullable<Int64> rnLayScaleX { get; set; }

        /// <summary>
        ///สัดส่วน ขนาด Layout แนวตั้ง
        /// <summary>
        public Nullable<Int64> rnLayScaleY { get; set; }

        /// <summary>
        ///ชั้นที่
        /// <summary>
        public Nullable<Int64> rnLayRow { get; set; }

        /// <summary>
        ///คอลัมน์
        /// <summary>
        public Nullable<Int64> rnLayCol { get; set; }

        /// <summary>
        ///รหัส ขนาด เพื่อแยกราคา
        /// <summary>
        public string rtPzeCode { get; set; }

        /// <summary>
        ///รหัส rack/ตู้ /กลุ่ม
        /// <summary>
        public string rtRakCode { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// <summary>
        public string rtLayStaUse { get; set; }

        /// <summary>
        ///วันที่ Update ล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public string rtCreateBy { get; set; }
    }
}
