using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.System
{
    class cmlTSysRcvConfig
    {
        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// <summary>
        public string rtFmtCode { get; set; }

        /// <summary>
        ///รหัสลำดับ
        /// <summary>
        public Nullable<int> rnSysSeq { get; set; }

        /// <summary>
        ///ตัวแปร
        /// <summary>
        public string rtSysKey { get; set; }

        /// <summary>
        ///ค่า ตัวแปร
        /// <summary>
        public string rtSysStaUsrValue { get; set; }

        /// <summary>
        ///ค่า อ้างอิง
        /// <summary>
        public string rtSysStaUsrRef { get; set; }

        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string rtBchCode { get; set; }
    }
}
