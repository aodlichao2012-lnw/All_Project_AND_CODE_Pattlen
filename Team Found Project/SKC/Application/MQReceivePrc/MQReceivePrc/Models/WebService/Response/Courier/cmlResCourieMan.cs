using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Courier
{
    public class cmlResCourieMan
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
        ///เบอร์โทร
        /// <summary>
        public string rtManTel { get; set; }

        /// <summary>
        ///รหัสพนักงาน
        /// <summary>
        public string rtManCardEmp { get; set; }

        /// <summary>
        ///เพศ 1:ชาย, 2:หญิง
        /// <summary>
        public string rtManSex { get; set; }

        /// <summary>
        ///วันเกิด
        /// <summary>
        public Nullable<DateTime> rdManDob { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:Active 2:Disable
        /// <summary>
        public string rtManStaActive { get; set; }
    }
}
