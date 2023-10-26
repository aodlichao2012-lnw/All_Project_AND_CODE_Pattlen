using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopSizeTmp_L
    {
        /// <summary>
        ///รหัส Size
        /// <summary>
        public string FTSizCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อ ขนาด
        /// <summary>
        public string FTSizName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTSizRemark { get; set; }
    }
}
