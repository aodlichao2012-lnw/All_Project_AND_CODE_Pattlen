using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTLKMWaHouseTmp
    {
        /// <summary>
        ///รหัสตัวแทนขาย
        /// </summary>
        public string FTAgnCode { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสคลัง
        /// </summary>
        public string FTWahCode { get; set; }

        /// <summary>
        ///รหัสอ้างอิงคลังจาก SKC
        /// </summary>
        public string FTWahRefNo { get; set; }

        /// <summary>
        ///Channel 1:Counter , 2:Event , 3:Vansale
        /// </summary>
        public string FTWahStaChannel { get; set; }

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
