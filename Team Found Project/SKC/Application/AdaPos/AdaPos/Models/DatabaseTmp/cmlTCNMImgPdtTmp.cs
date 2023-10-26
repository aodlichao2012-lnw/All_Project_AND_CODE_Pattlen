using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMImgPdtTmp
    {
        /// <summary>
        ///(AUTONUMBER)รหัส
        /// <summary>
        public Nullable<Int64> FNImgID { get; set; }

        /// <summary>
        ///รหัสอ้างอิงข้อมูลหลัก
        /// <summary>
        public string FTImgRefID { get; set; }

        /// <summary>
        ///ลำดับรูป
        /// <summary>
        public Nullable<int> FNImgSeq { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// <summary>
        public string FTImgTable { get; set; }

        /// <summary>
        ///Key filter ระบุข้อมูล กรณีมีหลาย Seq
        /// <summary>
        public string FTImgKey { get; set; }

        /// <summary>
        ///เก็บรูปภาพเป็น Path ..\
        /// <summary>
        public string FTImgObj { get; set; }

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
