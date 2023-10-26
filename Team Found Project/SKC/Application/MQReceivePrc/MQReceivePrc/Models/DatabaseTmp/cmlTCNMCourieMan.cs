using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCourieMan
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
        ///เบอร์โทร
        /// <summary>
        public string FTManTel { get; set; }

        /// <summary>
        ///รหัสพนักงาน
        /// <summary>
        public string FTManCardEmp { get; set; }

        /// <summary>
        ///เพศ 1:ชาย, 2:หญิง
        /// <summary>
        public string FTManSex { get; set; }

        /// <summary>
        ///วันเกิด
        /// <summary>
        public Nullable<DateTime> FDManDob { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:Active 2:Disable
        /// <summary>
        public string FTManStaActive { get; set; }
    }
}
