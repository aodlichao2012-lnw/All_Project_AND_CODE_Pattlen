using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCourier_L
    {
        /// <summary>
        ///รหัสบริษัทขนส่ง
        /// <summary>
        public string FTCryCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อบริษัทขนส่ง
        /// <summary>
        public string FTCryName { get; set; }

        /// <summary>
        ///ชื่ออื่น
        /// <summary>
        public string FTCryNameOth { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTCryRmk { get; set; }
    }
}
