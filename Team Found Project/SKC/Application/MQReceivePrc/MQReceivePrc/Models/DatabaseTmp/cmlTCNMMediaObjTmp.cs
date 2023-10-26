using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMMediaObjTmp
    {
        /// <summary>
        ///(AUTONUMBER)รหัส
        /// <summary>
        public Nullable<Int64> FNMedID { get; set; }

        /// <summary>
        ///รหัสอ้างอิงข้อมูลหลัก
        /// <summary>
        public string FTMedRefID { get; set; }

        /// <summary>
        ///ลำดับรูป
        /// <summary>
        public Nullable<int> FNMedSeq { get; set; }

        /// <summary>
        ///1: Sound 2:VDO
        /// <summary>
        public Nullable<int> FNMedType { get; set; }

        /// <summary>
        ///ประเภท File AVI or SWF or MPG or WMF or MP4
        /// <summary>
        public string FTMedFileType { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// <summary>
        public string FTMedTable { get; set; }

        /// <summary>
        ///Key filter ระบุข้อมูล กรณีมีหลาย Seq
        /// <summary>
        public string FTMedKey { get; set; }

        /// <summary>
        ///เก็บรูปภาพเป็น Path ..\
        /// <summary>
        public string FTMedPath { get; set; }

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
