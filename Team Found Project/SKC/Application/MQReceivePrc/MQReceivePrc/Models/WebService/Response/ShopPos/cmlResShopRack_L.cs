using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ShopPos
{
    public class cmlResShopRack_L
    {
        /// <summary>
        ///รหัส rack/ตู้ /กลุ่ม
        /// <summary>
        public string rtRakCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ ขนาด
        /// <summary>
        public string rtRakName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtRakRmk { get; set; }
    }
}
