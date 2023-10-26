using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.User
{
    /// <summary>
    /// ตาราง TCNMUsrActRole
    /// </summary>
    public class cmlResInfoTCNMUsrActRole
    {
        /// <summary>
        ///รหัสหน้าที่
        /// </summary>
        public string rtRolCode { get; set; }

        /// <summary>
        ///รหัสผู้ใช้
        /// </summary>
        public string rtUsrCode { get; set; }

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
        ///ผู้สร่างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
    }
}