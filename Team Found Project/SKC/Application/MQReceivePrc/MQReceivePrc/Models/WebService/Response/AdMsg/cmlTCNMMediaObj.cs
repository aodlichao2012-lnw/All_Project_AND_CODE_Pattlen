using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.AdMsg
{
    public class cmlTCNMMediaObj
    {
        /// <summary>
        ///(AUTONUMBER)รหัส
        /// <summary>
        public Nullable<Int64> rnMedID { get; set; }

        /// <summary>
        ///รหัสอ้างอิงข้อมูลหลัก
        /// <summary>
        public string rnMedRefID { get; set; }

        /// <summary>
        ///ลำดับรูป
        /// <summary>
        public Nullable<int> rnMedSeq { get; set; }

        /// <summary>
        ///1: Sound 2:VDO
        /// <summary>
        public Nullable<int> rnMedType { get; set; }

        /// <summary>
        ///ประเภท File AVI or SWF or MPG or WMF or MP4
        /// <summary>
        public string rtMedFileType { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// <summary>
        public string rtMedTable { get; set; }

        /// <summary>
        ///Key filter ระบุข้อมูล กรณีมีหลาย Seq
        /// <summary>
        public string rtMedKey { get; set; }

        /// <summary>
        ///เก็บรูปภาพเป็น Path ..\
        /// <summary>
        public string rtMedPath { get; set; }

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
