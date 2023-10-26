using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopRackTmp_L
    {
        /// <summary>
        ///รหัส rack/ตู้ /กลุ่ม
        /// <summary>
        public string FTRakCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อ ขนาด
        /// <summary>
        public string FTRakName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTRakRmk { get; set; }
    }
}
