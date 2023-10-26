using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCourierGrp_L
    {
        /// <summary>
        ///รหัสกลุ่มบริษัทส่งพัสดุ
        /// <summary>
        public string FTCgpCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อกลุ่มลูกค้า
        /// <summary>
        public string FTCgpName { get; set; }

        /// <summary>
        ///Remark
        /// <summary>
        public string FTCgpRmk { get; set; }
    }
}
