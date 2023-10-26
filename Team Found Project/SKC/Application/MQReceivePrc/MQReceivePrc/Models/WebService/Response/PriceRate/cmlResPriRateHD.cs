using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.PriceRate
{
    public class cmlResPriRateHD
    {
        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด ผูกตอนปรับราคา
        /// <summary>
        public string rtRthCode { get; set; }

        /// <summary>
        ///การปัดเศษหน่วยเวลา 1:ปัดขึ้น(Df) ;2ปัดลง;3:เฉลี่ย(ใช้ราคาจาก Rate Seq ก่อนหน้า)
        /// <summary>
        public string rtRthCalType { get; set; }

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
