using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopTypeTmp
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
        ///ประเภทตู้ 1 : ปกติ , 2 : ควบคุมอุณหภูมิ
        /// <summary>
        public string FTShtType { get; set; }

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
        ///วันที่สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }
    }
}
