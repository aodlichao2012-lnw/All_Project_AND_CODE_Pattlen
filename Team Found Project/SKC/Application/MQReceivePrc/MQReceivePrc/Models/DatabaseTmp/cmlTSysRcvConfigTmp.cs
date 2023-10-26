using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    class cmlTSysRcvConfigTmp
    {
        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// <summary>
        public string FTFmtCode { get; set; }

        /// <summary>
        ///รหัสลำดับ
        /// <summary>
        public Nullable<int> FNSysSeq { get; set; }

        /// <summary>
        ///ตัวแปร
        /// <summary>
        public string FTSysKey { get; set; }

        /// <summary>
        ///ค่า ตัวแปร
        /// <summary>
        public string FTSysStaUsrValue { get; set; }

        /// <summary>
        ///ค่า อ้างอิง
        /// <summary>
        public string FTSysStaUsrRef { get; set; }

        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }
    }
}
