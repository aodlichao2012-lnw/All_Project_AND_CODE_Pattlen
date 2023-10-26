using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Courier
{
    public class cmlResCourieMan_L
    {
        /// <summary>
        ///รหัสบริษัทขนส่ง
        /// <summary>
        public string rtCryCode { get; set; }

        /// <summary>
        ///รหัสบัตรประชาชน / พาสปอต
        /// <summary>
        public string rtManCardID { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อพนักงาน
        /// <summary>
        public string rtManName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtCryRmk { get; set; }
    }
}
