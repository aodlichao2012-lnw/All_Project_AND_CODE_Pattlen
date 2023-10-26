using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMUsrLoginTmp
    {
        /// <summary>
        ///รหัสผู้ใช้
        /// <summary>
        public string FTUsrCode { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งาน 1:Password 2:Pin 3:RFID  4:QR
        /// <summary>
        public string FTUsrLogType { get; set; }

        /// <summary>
        ///วันที่เริมใช้งาน
        /// <summary>
        public Nullable<DateTime> FDUsrPwdStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด  Start + 3 M
        /// <summary>
        public Nullable<DateTime> FDUsrPwdExpired { get; set; }

        /// <summary>
        ///Account ห้ามซ้ำ เช่น รหัสพนักงาน email เบอร์ โทร
        /// <summary>
        public string FTUsrLogin { get; set; }

        /// <summary>
        ///Type 2: 6 digit,3:รหัส RFID,4 QR {FTUsrCode|Passoword เข้ารหัส}/ถ้าไม่เข้า Format เช็ค LoginPwd)
        /// <summary>
        public string FTUsrLoginPwd { get; set; }

        /// <summary>
        ///Remark,Hint
        /// <summary>
        public string FTUsrRmk { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:Active 2:Disable
        /// <summary>
        public string FTUsrStaActive { get; set; }
    }
}
