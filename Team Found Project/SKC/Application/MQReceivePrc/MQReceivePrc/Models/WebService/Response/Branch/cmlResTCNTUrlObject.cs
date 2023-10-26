using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MQReceivePrc.Models.Webservice.Response.Branch
{
    public class cmlResTCNTUrlObject
    {
        /// <summary>
        ///(AUTONUMBER)รหัส
        /// <summary>
        public Nullable<Int64> rnUrlID { get; set; }

        /// <summary>
        ///รหัสอ้างอิงข้อมูลหลัก
        /// <summary>
        public string rtUrlRefID { get; set; }

        /// <summary>
        ///ลำดับ URL
        /// <summary>
        public Nullable<int> rnUrlSeq { get; set; }

        /// <summary>
        ///ประเภท URL
        /// <summary>
        public Nullable<int> rnUrlType { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// <summary>
        public string rtUrlTable { get; set; }

        /// <summary>
        ///Key filter ระบุข้อมูล กรณีมีหลาย Seq
        /// <summary>
        public string rtUrlKey { get; set; }

        /// <summary>
        ///Path URL
        /// <summary>
        public string rtUrlAddress { get; set; }

        /// <summary>
        ///Port สำหรับเชื่อต่อ
        /// <summary>
        public string rtUrlPort { get; set; }

        /// <summary>
        ///เก็บรูป icon ภาพเป็น Path ..\
        /// <summary>
        public string rtUrlLogo { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
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
        ///ผู้สร้างรายการ
        /// <summary>
        public string rtCreateBy { get; set; }
    }
}