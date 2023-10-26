using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MQReceivePrc.Models.Shift
{
    public class cmlTPSTShiftSRatePdt
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
        ///รหัสอ้างอิง สินค้า หรือรหัสสกุลเงิน
        /// <summary>
        public string FTSrpCodeRef { get; set; }

        /// <summary>
        ///ประเภท Summary 1:สกุลเงิน  2:สินค้าควบคุม
        /// <summary>
        public string FTSrpType { get; set; }

        /// <summary>
        ///ชื่อสินค้า หรือชื่อสกุลเงิน
        /// <summary>
        public string FTSrpNameRef { get; set; }

        /// <summary>
        ///จำนวน
        /// <summary>
        public Nullable<decimal> FCSrpQty { get; set; }

        /// <summary>
        ///มูลค่า
        /// <summary>
        public Nullable<decimal> FCSrpAmt { get; set; }
    }
}