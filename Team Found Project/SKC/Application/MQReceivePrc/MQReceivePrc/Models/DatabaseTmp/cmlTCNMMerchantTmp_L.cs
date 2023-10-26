using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMMerchantTmp_L
    {
        /// <summary>
        ///รหัสคู้ค้า
        /// <summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อ
        /// <summary>
        public string FTMerName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTMerRmk { get; set; }
    }
}
