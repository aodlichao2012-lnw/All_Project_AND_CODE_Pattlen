using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SlipMsg
{
    public class cmlTCNMSlipMsgDT_L
    {
        /// <summary>
        ///รหัสหัวท้ายใบเสร็จ
        /// <summary>
        public string FTSmgCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ประเภทหัว/ท้าย 0:HD/ฃื่อ  1:หัว 2:ท้าย
        /// <summary>
        public string FTSmgType { get; set; }

        /// <summary>
        ///ลำดับหัวท้ายใบเสร็จ
        /// <summary>
        public Nullable<int> FNSmgSeq { get; set; }

        /// <summary>
        ///ข้อความ
        /// <summary>
        public string FTSmgName { get; set; }
    }
}
