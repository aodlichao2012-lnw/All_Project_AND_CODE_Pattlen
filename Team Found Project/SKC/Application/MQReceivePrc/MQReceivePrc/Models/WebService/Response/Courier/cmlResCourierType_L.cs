using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Courier
{
    public class cmlResCourierType_L
    {
        /// <summary>
        ///รหัสประเภทบริษัทส่งพัสดุ
        /// <summary>
        public string rtCtyCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อประเภทบริษัทส่งพัสดุ
        /// <summary>
        public string rtCtyName { get; set; }

        /// <summary>
        ///Remark
        /// <summary>
        public string rtCtyRmk { get; set; }
    }
}
