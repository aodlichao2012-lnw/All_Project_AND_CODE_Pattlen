using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCourieMan_L
    {
        /// <summary>
        ///รหัสบริษัทขนส่ง
        /// <summary>
        public string FTCryCode { get; set; }

        /// <summary>
        ///รหัสบัตรประชาชน / พาสปอต
        /// <summary>
        public string FTManCardID { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อพนักงาน
        /// <summary>
        public string FTManName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTCryRmk { get; set; }
    }
}
