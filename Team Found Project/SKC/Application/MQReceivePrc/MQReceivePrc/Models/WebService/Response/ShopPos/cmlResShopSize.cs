using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopSize
    {
        /// <summary>
        ///รหัส Size
        /// <summary>
        public string rtPzeCode { get; set; }

        /// <summary>
        ///ความลึก
        /// <summary>
        public Nullable<decimal> rcPzeDim { get; set; }

        /// <summary>
        ///ความสูง
        /// <summary>
        public Nullable<decimal> rcPzeHigh { get; set; }

        /// <summary>
        ///ความกว้าง
        /// <summary>
        public Nullable<decimal> rcPzeWide { get; set; }

        /// <summary>
        ///วันที่ Update ล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string rtCreateBy { get; set; }
    }
}
