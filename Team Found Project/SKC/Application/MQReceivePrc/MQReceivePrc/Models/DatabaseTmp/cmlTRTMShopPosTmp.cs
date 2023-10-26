using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopPosTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัส Shop
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง Pos/ตู้   [Refer Address]
        /// <summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///S/N เครื่อง/Posตู้
        /// <summary>
        public string FTPshPosSN { get; set; }

        /// <summary>
        ///IP Address เครื่องจุดขาย
        /// <summary>
        public string FTPshNetIP { get; set; }

        /// <summary>
        ///พอร์ทสั่งงาน เครื่องจุดขาย IP:Port
        /// <summary>
        public string FTPshNetPort { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งานผู้รับ 1:Pin 2:RFID 3:QR  Default ตาม model locker
        /// <summary>
        public string FTPshSLRcvType { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// <summary>
        public string FTPshStaUse { get; set; }

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
