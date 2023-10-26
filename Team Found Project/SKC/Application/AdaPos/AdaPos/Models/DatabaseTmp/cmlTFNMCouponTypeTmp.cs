using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTFNMCouponTypeTmp
    {
        ///<summary>
        ///รหัสประเภทคูปอง
        ///</summary>
        public string FTCptCode { get; set; }

        ///<summary>
        ///ประเภท 1:คูปองเงินสด 2:คูปองส่วนลด
        ///</summary>
        public string FTCptType { get; set; }

        ///<summary>
        ///สถานะตรวจสอบคูปอง 1:ตรวจสอบ 2:ไม่ตรวจสอบ
        ///</summary>
        public string FTCptStaChk { get; set; }

        ///<summary>
        ///ประเภท :HQ 2:Branch Def: Branch  ใช้ตรวจสอบคูปอง
        ///</summary>
        public string FTCptStaChkHQ { get; set; }

        ///<summary>
        ///สถานะใช้งาน 1:ใช้งาน 2: ไม่ใช้งาน
        ///</summary>
        public string FTCptStaUse { get; set; }

        ///<summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        ///</summary>
        public DateTime? FDLastUpdOn { get; set; }

        ///<summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        ///</summary>
        public string FTLastUpdBy { get; set; }

        ///<summary>
        ///วันที่สร้างรายการ
        ///</summary>
        public DateTime? FDCreateOn { get; set; }

        ///<summary>
        ///ผู้สร้างรายการ
        ///</summary>
        public string FTCreateBy { get; set; }


        //*Net 63-03-07 Gen Model
        /*
        public string FTCptCode { get; set; }
        public string FTCptStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string FTCptType { get; set; }       //*Arm 62-12-20

        /// <summary>
        ///
        /// <summary>
        public string FTCptStaChk { get; set; }     //*Arm 62-12-20

        /// <summary>
        ///ประเภท 1:HQ 2:Branch Def: Branch  ใช้ตรวจสอบคูปอง
        /// <summary>
        public string FTCptStaChkHQ { get; set; }   //*Arm 62-12-20*/

    }
}
