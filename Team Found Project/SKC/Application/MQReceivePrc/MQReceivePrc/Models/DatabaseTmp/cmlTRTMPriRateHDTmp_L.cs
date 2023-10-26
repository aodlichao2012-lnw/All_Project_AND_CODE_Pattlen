using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMPriRateHDTmp_L
    {
        /// <summary>
        ///รหัสราคาค่าเช่า ตามขนาด
        /// <summary>
        public string FTRthCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อ ราคาค่าเช่า
        /// <summary>
        public string FTRthName { get; set; }
    }
}
