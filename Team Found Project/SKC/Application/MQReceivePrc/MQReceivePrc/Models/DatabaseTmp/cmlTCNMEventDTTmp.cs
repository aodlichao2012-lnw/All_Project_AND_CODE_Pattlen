using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMEventDTTmp
    {
        /// <summary>
        ///รหัส
        /// <summary>
        public string FTEvhCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNEvdSeqNo { get; set; }

        /// <summary>
        ///ประเภทเหตุการณ์ 1:ตามช่วงเวลา 2:ตามช่วงวันที่
        /// <summary>
        public string FTEvdType { get; set; }

        /// <summary>
        ///เวลาเริ่มต้น
        /// <summary>
        public string FTEvdTStart { get; set; }

        /// <summary>
        ///วันที่เริ่มต้น
        /// <summary>
        public Nullable<DateTime> FDEvdDStart { get; set; }

        /// <summary>
        ///เวลาสิ้นสุด
        /// <summary>
        public string FTEvdTFinish { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด
        /// <summary>
        public Nullable<DateTime> FDEvdDFinish { get; set; }

        /// <summary>
        ///สถานะ 1 :ใช้งาน 2:ไม่ใช้งาน
        /// <summary>
        public string FTEvdStaUse { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }
    }
}
