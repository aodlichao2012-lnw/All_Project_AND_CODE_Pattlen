using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    class cmlTCNMDocApvRoleTmp
    {
        /// <summary>
        ///(AUTONUMBER)รหัส
        /// </summary>
        public Nullable<Int64> FNDarID { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// </summary>
        public string FTDarTable { get; set; }

        /// <summary>
        ///FNXshDocType
        /// </summary>
        public string FTDarRefType { get; set; }

        /// <summary>
        ///ลำดับ การอนุมัติ
        /// </summary>
        public Nullable<int> FNDarApvSeq { get; set; }

        /// <summary>
        ///Role ที่ใช้สำหรับอนุมัติ อ้างอิง Table TCNMUser
        /// </summary>
        public string FTDarUsrRole { get; set; }

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
