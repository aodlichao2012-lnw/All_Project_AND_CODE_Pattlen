using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopLayoutTmp_L
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///ลำดับช่อง
        /// <summary>
        public Nullable<Int64> FNLayNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อ Layout
        /// <summary>
        public string FTLayName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTLayRemark { get; set; }
    }
}
