using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Courier
{
    public class cmlResCourierGrp_L
    {
        /// <summary>
        ///รหัสกลุ่มบริษัทส่งพัสดุ
        /// <summary>
        public string rtCgpCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อกลุ่มลูกค้า
        /// <summary>
        public string rtCgpName { get; set; }

        /// <summary>
        ///Remark
        /// <summary>
        public string rtCgpRmk { get; set; }
    }
}
