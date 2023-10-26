using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTPSTShiftSKeyRcv
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง POS
        /// <summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///รหัสรอบแคชเชียร์  YYMMDDNN
        /// <summary>
        public string FTShfCode { get; set; }

        /// <summary>
        ///ลำดับการ SignIn DT
        /// <summary>
        public Nullable<int> FNSdtSeqNo { get; set; }

        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// <summary>
        public string FTRcvCode { get; set; }

        /// <summary>
        ///ยอดชำระ
        /// <summary>
        public Nullable<decimal> FCRcvPayAmt { get; set; }

        /// <summary>
        ///ยอดปัดเศษ
        /// <summary>
        public Nullable<decimal> FCRcvRndAmt { get; set; }
    }
}
