using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtHD_LTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// <summary>
        public string FTPmhDocNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่น
        /// <summary>
        public string FTPmhName { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่น(แบบย่อ)
        /// <summary>
        public string FTPmhNameSlip { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTPmhRmk { get; set; }
    }
}
