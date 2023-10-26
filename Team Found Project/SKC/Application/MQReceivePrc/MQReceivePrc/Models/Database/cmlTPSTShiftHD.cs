using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTPSTShiftHD
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง POS
        /// <summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///รหัสรอบแคชเชียร์  YYMMDDNN
        /// <summary>
        public string FTShfCode { get; set; }

        /// <summary>
        ///รหัสผู้เปิดรอบ/ชื่อผู้ใช้
        /// <summary>
        public string FTUsrCode { get; set; }

        /// <summary>
        ///วันที่การขาย
        /// <summary>
        public Nullable<DateTime> FDShdSaleDate { get; set; }

        /// <summary>
        ///วันที่และเวลา Sign In
        /// <summary>
        public Nullable<DateTime> FDShdSignIn { get; set; }

        /// <summary>
        ///วันที่และเวลา Sign Out
        /// <summary>
        public Nullable<DateTime> FDShdSignOut { get; set; }

        /// <summary>
        ///รหัสผู้ปิดรอบ/ชื่อผู้ใช้
        /// <summary>
        public string FTShdUsrClosed { get; set; }

        /// <summary>
        ///สถานะ prc เอกสาร 1:ยังไม่ทำ, 2:ทำแล้ว
        /// <summary>
        public string FTShdStaPrc { get; set; }

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
