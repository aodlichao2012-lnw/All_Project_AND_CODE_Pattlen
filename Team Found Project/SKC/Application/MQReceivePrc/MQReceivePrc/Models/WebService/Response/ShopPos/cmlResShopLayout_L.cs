using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopLayout_L
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///ลำดับช่อง
        /// <summary>
        public Nullable<Int64> rnLayNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ Layout
        /// <summary>
        public string rtLayName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtLayRemark { get; set; }
    }
}
