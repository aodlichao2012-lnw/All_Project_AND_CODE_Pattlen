using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Shift
{
    public class cmlShiftSumRcv
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
        ///สถานะ อ้างอิงตามตาราง TSysDocType
        /// <summary>
        public string FTRcvDocType { get; set; }

        /// <summary>
        ///ยอดชำระ
        /// <summary>
        public Nullable<decimal> FCRcvPayAmt { get; set; }

        /// <summary>
        ///ชื่อประเภทการชำระเงิน
        /// <summary>
        public string FTRcvName { get; set; }

        /// <summary>
        /// รูปแบบการรับชำระ
        /// <summary>
        public string FTFmtCode { get; set; } //*Net 63-08-03
    }
}
