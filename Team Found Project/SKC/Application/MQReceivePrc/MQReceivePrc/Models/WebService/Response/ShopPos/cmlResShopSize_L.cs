using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopSize_L
    {
        /// <summary>
        ///รหัส Size
        /// <summary>
        public string rtSizCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ ขนาด
        /// <summary>
        public string rtSizName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtSizRemark { get; set; }
    }
}
