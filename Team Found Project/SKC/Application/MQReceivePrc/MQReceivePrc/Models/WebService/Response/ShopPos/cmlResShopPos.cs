using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopPos
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัส Shop
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง Pos/ตู้   [Refer Address]
        /// <summary>
        public string rtPosCode { get; set; }

        /// <summary>
        ///S/N เครื่อง/Posตู้
        /// <summary>
        public string rtPshPosSN { get; set; }

        /// <summary>
        ///IP Address เครื่องจุดขาย
        /// <summary>
        public string rtPshNetIP { get; set; }

        /// <summary>
        ///พอร์ทสั่งงาน เครื่องจุดขาย IP:Port
        /// <summary>
        public string rtPshNetPort { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งานผู้รับ 1:Pin 2:RFID 3:QR  Default ตาม model locker
        /// <summary>
        public string rtPshSLRcvType { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// <summary>
        public string rtPshStaUse { get; set; }

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
