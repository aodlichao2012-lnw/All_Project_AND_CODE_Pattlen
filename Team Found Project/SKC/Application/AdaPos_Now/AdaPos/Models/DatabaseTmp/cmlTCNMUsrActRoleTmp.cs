using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMUsrActRoleTmp
    {
        ///<summary>
        ///รหัสหน้าที่
        ///</summary>
        public string FTRolCode { get; set; }

        ///<summary>
        ///รหัสผู้ใช้
        ///</summary>
        public string FTUsrCode { get; set; }

        ///<summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        ///</summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        ///<summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        ///</summary>
        public string FTLastUpdBy { get; set; }

        ///<summary>
        ///วันที่สร้างรายการ
        ///</summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        ///<summary>
        ///ผู้สร่างรายการ
        ///</summary>
        public string FTCreateBy { get; set; }


    }
}
