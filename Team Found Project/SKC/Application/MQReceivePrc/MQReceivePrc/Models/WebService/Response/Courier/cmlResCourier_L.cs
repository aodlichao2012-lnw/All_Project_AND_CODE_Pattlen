using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Courier
{
    public class cmlResCourier_L
    {
        /// <summary>
        ///รหัสบริษัทขนส่ง
        /// <summary>
        public string rtCryCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อบริษัทขนส่ง
        /// <summary>
        public string rtCryName { get; set; }

        /// <summary>
        ///ชื่ออื่น
        /// <summary>
        public string rtCryNameOth { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtCryRmk { get; set; }
    }
}
