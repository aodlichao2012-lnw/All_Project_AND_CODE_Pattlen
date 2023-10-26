using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Courier
{
    public class cmlResCourieLogin
    {
        /// <summary>
        ///รหัสบริษัทส่งพัสดุ
        /// <summary>
        public string rtCryCode { get; set; }

        /// <summary>
        ///เลขที่บัตรประจำตัวประชาชน/Passport
        /// <summary>
        public string rtManCardID { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งาน 1:Password 2:Pin 3:RFID  4:QR
        /// <summary>
        public string rtManLogType { get; set; }

        /// <summary>
        ///วันที่เริมใช้งาน
        /// <summary>
        public Nullable<DateTime> rdManPwdStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด  Start + 12 M
        /// <summary>
        public Nullable<DateTime> rdManPwdExpired { get; set; }

        /// <summary>
        ///Account ห้ามซ้ำ เช่น รหัสพนักงาน email เบอร์ โทร
        /// <summary>
        public string rtManLogin { get; set; }

        /// <summary>
        ///Type 2: 6 digit,3:รหัส RFID,4 QR {คุย Format}/ถ้าไม่เข้า Format เช็ค LoginPwd)
        /// <summary>
        public string rtManLoginPwd { get; set; }

        /// <summary>
        ///Remark,Hint
        /// <summary>
        public string rtManRmk { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:Active 2:Disable
        /// <summary>
        public string rtManStaActive { get; set; }
    }
}
