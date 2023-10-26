using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMMerchantTmp
    {
        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// <summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///Email
        /// <summary>
        public string FTMerEmail { get; set; }

        /// <summary>
        ///เบอร์โทรศัพท์
        /// <summary>
        public string FTMerTel { get; set; }

        /// <summary>
        ///เบอร์โทรสาร
        /// <summary>
        public string FTMerFax { get; set; }

        /// <summary>
        ///เบอร์โทรศัพท์
        /// <summary>
        public string FTMerMo { get; set; }

        /// <summary>
        ///สถานะติดต่อ 1:ติดต่อ, 2:เลิกติดต่อ
        /// <summary>
        public string FTMerStaActive { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
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
