using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCourieLogin
    {
        /// <summary>
        ///รหัสบริษัทส่งพัสดุ
        /// <summary>
        public string FTCryCode { get; set; }

        /// <summary>
        ///เลขที่บัตรประจำตัวประชาชน/Passport
        /// <summary>
        public string FTManCardID { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งาน 1:Password 2:Pin 3:RFID  4:QR
        /// <summary>
        public string FTManLogType { get; set; }

        /// <summary>
        ///วันที่เริมใช้งาน
        /// <summary>
        public Nullable<DateTime> FDManPwdStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด  Start + 12 M
        /// <summary>
        public Nullable<DateTime> FDManPwdExpired { get; set; }

        /// <summary>
        ///Account ห้ามซ้ำ เช่น รหัสพนักงาน email เบอร์ โทร
        /// <summary>
        public string FTManLogin { get; set; }

        /// <summary>
        ///Type 2: 6 digit,3:รหัส RFID,4 QR {คุย Format}/ถ้าไม่เข้า Format เช็ค LoginPwd)
        /// <summary>
        public string FTManLoginPwd { get; set; }

        /// <summary>
        ///Remark,Hint
        /// <summary>
        public string FTManRmk { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:Active 2:Disable
        /// <summary>
        public string FTManStaActive { get; set; }
    }
}
