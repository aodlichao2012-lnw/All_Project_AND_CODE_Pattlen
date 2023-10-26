using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required.User
{
    public class cmlReqPwdChange
    {
        /// <summary>
        /// รหัสผู้ใช้
        /// </summary>
        public string ptUsrCode { get; set; }

        /// <summary>
        /// User Login
        /// </summary>
        public string ptUsrLogin { get; set; }

        /// <summary>
        /// รหัสผ่านเดิม
        /// </summary>
        public string ptOldPwd { get; set; }

        /// <summary>
        /// รหัสผ่านใหม่
        /// </summary>
        public string ptNewPwd { get; set; }

        /// <summary>
        /// ประเภทการเข้าใช้งาน 1:Password 2:Pin 3:RFID  4:QR
        /// </summary>
        public string ptLoginType { get; set; }
    }
}
