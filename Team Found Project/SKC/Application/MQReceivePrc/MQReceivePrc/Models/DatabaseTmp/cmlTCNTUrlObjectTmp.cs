using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTUrlObjectTmp
    {
        /// <summary>
        ///(AUTONUMBER)รหัส
        /// <summary>
        public Nullable<Int64> FNUrlID { get; set; }

        /// <summary>
        ///รหัสอ้างอิงข้อมูลหลัก
        /// <summary>
        public string FTUrlRefID { get; set; }

        /// <summary>
        ///ลำดับ URL
        /// <summary>
        public Nullable<int> FNUrlSeq { get; set; }

        /// <summary>
        ///ประเภท URL
        /// <summary>
        public Nullable<int> FNUrlType { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// <summary>
        public string FTUrlTable { get; set; }

        /// <summary>
        ///Key filter ระบุข้อมูล กรณีมีหลาย Seq
        /// <summary>
        public string FTUrlKey { get; set; }

        /// <summary>
        ///Path URL
        /// <summary>
        public string FTUrlAddress { get; set; }

        /// <summary>
        ///Port สำหรับเชื่อต่อ
        /// <summary>
        public string FTUrlPort { get; set; }

        /// <summary>
        ///เก็บรูป icon ภาพเป็น Path ..\
        /// <summary>
        public string FTUrlLogo { get; set; }

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
