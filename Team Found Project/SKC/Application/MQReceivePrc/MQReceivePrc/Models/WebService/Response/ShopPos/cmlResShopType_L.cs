using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopType_L
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
        ///รหัสภาษา
        /// <summary>
        public Nullable<int> rnLngID { get; set; }

        /// <summary>
        ///ประเภทตู้ 1 : ปกติ , 2 : ควบคุมอุณหภูมิ
        /// <summary>
        public string rtShtType { get; set; }

        /// <summary>
        ///ชื่อประเภท Shop
        /// <summary>
        public string rtShtName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtShtRemark { get; set; }
    }
}
