using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Coupon
{
    public class cmlResInfoCpnType
    {
        /// <summary>
        ///รหัสประเภทคูปอง
        /// </summary>
        public string rtCptCode { get; set; }

        /// <summary>
        ///ประเภท 1:คูปองเงินสด 2:คูปองส่วนลด
        /// </summary>
        public string rtCptType { get; set; }

        /// <summary>
        ///สถานะตรวจสอบคูปอง 1:ตรวจสอบ 2:ไม่ตรวจสอบ
        /// </summary>
        public string rtCptStaChk { get; set; }

        /// <summary>
        ///ประเภท :HQ 2:Branch Def: Branch  ใช้ตรวจสอบคูปอง
        /// </summary>
        public string rtCptStaChkHQ { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:ใช้งาน 2: ไม่ใช้งาน
        /// </summary>
        public string rtCptStaUse { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }

        //*Net 63-03-06 Gen Model from script
        /*
        public string rtCptCode { get; set; }
        public string rtCptStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string rtCptType { get; set; }       //*Arm 62-12-20

        /// <summary>
        ///
        /// <summary>
        public string rtCptStaChk { get; set; }     //*Arm 62-12-20

        /// <summary>
        ///ประเภท 1:HQ 2:Branch Def: Branch  ใช้ตรวจสอบคูปอง
        /// <summary>
        public string rtCptStaChkHQ { get; set; }   //*Arm 62-12-20*/
    }
}
