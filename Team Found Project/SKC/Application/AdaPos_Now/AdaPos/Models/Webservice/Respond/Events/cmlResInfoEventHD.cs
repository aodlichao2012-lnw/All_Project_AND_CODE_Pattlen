using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Events
{
    public class cmlResInfoEventHD
    {
        /// <summary>
        ///รหัส
        /// <summary>
        public string rtEvhCode { get; set; }

        /// <summary>
        ///สถานะ 1 :ใช้งาน 2:ไม่ใช้งาน
        /// <summary>
        public string rtEvhStaActive { get; set; }

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
