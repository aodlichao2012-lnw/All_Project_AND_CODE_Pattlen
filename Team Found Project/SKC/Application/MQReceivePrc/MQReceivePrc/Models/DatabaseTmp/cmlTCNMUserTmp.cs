using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMUserTmp
    {
        public string FTUsrCode { get; set; }
        public string FTDptCode { get; set; }
        public string FTRolCode { get; set; }
        public string FTUsrTel { get; set; }
        public string FTUsrPwd { get; set; }
        public string FTUsrEmail { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งาน 1:Password 2:Pin 3:RFID
        /// </summary>
        public string FTUsrLoginType { get; set; }  //*Arm 63-01-30
        
        /// <summary>
        ///วันที่เริ่มต้น
        /// </summary>
        public Nullable<DateTime> FDUsrStart { get; set; }  //*Arm 63-01-30

        /// <summary>
        ///วันที่สิ้นสุด
        /// </summary>
        public Nullable<DateTime> FDUsrFinish { get; set; } //*Arm 63-01-30

        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }

    }
}
