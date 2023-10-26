using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MQReceivePrc.Models.Shift
{
    public class cmlTPSTShiftSKeyBN
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
        ///รหัสธนบัตร
        /// <summary>
        public string FTBntCode { get; set; }

        /// <summary>
        ///มูลค่า ธนบัตร
        /// <summary>
        public Nullable<decimal> FCKbnRateAmt { get; set; }

        /// <summary>
        ///จำนวนแบงค์
        /// <summary>
        public Nullable<Int64> FNKbnQty { get; set; }

        /// <summary>
        ///มูลค่าแบงค์
        /// <summary>
        public Nullable<decimal> FCKbnAmt { get; set; }
    }
}