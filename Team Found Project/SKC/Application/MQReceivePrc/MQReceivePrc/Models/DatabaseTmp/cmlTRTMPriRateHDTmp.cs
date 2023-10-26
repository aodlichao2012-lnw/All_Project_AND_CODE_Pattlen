using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMPriRateHDTmp
    {
        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด ผูกตอนปรับราคา
        /// <summary>
        public string FTRthCode { get; set; }

        /// <summary>
        ///การปัดเศษหน่วยเวลา 1:ปัดขึ้น(Df) ;2ปัดลง;3:เฉลี่ย(ใช้ราคาจาก Rate Seq ก่อนหน้า)
        /// <summary>
        public string FTRthCalType { get; set; }

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
