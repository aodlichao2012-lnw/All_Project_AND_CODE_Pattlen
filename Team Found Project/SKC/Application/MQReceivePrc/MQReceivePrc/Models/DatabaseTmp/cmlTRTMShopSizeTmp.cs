using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopSizeTmp
    {
        /// <summary>
        ///รหัส Size
        /// <summary>
        public string FTPzeCode { get; set; }

        /// <summary>
        ///ความลึก
        /// <summary>
        public Nullable<decimal> FCPzeDim { get; set; }

        /// <summary>
        ///ความสูง
        /// <summary>
        public Nullable<decimal> FCPzeHigh { get; set; }

        /// <summary>
        ///ความกว้าง
        /// <summary>
        public Nullable<decimal> FCPzeWide { get; set; }

        /// <summary>
        ///วันที่ Update ล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }
    }
}
