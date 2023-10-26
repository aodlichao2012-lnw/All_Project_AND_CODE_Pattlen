using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.User
{
    public class cmlResInfoUser
    {
        public string rtUsrCode { get; set; }
        public string rtDptCode { get; set; }
        public string rtRolCode { get; set; }
        public string rtUsrTel { get; set; }
        public string rtUsrPwd { get; set; }
        public string rtUsrEmail { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งาน 1:Password 2:Pin 3:RFID
        /// </summary>
        public string rtUsrLoginType { get; set; }  //*Arm 63-01-24
        
        /// <summary>
        ///วันที่เริ่มต้น
        /// </summary>
        public Nullable<DateTime> rdUsrStart { get; set; }  //*Arm 63-01-24

        /// <summary>
        ///วันที่สิ้นสุด
        /// </summary>
        public Nullable<DateTime> rdUsrFinish { get; set; } //*Arm 63-01-24

        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
