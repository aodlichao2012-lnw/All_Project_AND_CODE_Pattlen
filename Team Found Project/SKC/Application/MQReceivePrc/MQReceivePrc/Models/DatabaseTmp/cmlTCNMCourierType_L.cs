using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMCourierType_L
    {
        /// <summary>
        ///รหัสประเภทบริษัทส่งพัสดุ
        /// <summary>
        public string FTCtyCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อประเภทบริษัทส่งพัสดุ
        /// <summary>
        public string FTCtyName { get; set; }

        /// <summary>
        ///Remark
        /// <summary>
        public string FTCtyRmk { get; set; }
    }
}
