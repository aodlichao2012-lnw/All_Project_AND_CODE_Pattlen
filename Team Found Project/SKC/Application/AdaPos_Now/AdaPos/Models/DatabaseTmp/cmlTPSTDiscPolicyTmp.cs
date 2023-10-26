using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTPSTDiscPolicyTmp
    {
        /// <summary>
        ///รหัสลำดับการให้ส่วนลดแนวแกน X
        /// </summary>
        public string FTDpcDisCodeX { get; set; }

        /// <summary>
        ///รหัสลำดับการให้ส่วนลดแนวแกน Y
        /// </summary>
        public string FTDpcDisCodeY { get; set; }

        /// <summary>
        ///สถานะการอนุญาตใช้งาน 1=Yes  2=No
        /// </summary>
        public string FTDpcStaAlw { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }
    }
}
