using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MQReceivePrc.Models.Shift
{
    public class cmlTPSTShiftEvent
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
        ///วันที่ เวลา
        /// <summary>
        public Nullable<DateTime> FDHisDateTime { get; set; }

        /// <summary>
        ///รหัสประเภทประวัติแคชเชียร์
        /// <summary>
        public string FTEvnCode { get; set; }

        /// <summary>
        ///รวมจำนวนครั้ง
        /// <summary>
        public Nullable<int> FNSvnQty { get; set; }

        /// <summary>
        ///รวมจำนวนครั้ง
        /// <summary>
        public Nullable<decimal> FCSvnAmt { get; set; }

        /// <summary>
        ///รหัสเหตุผล
        /// <summary>
        public string FTRsnCode { get; set; }

        /// <summary>
        ///รหัสผู้อนุมัติ
        /// <summary>
        public string FTSvnApvCode { get; set; }

        /// <summary>
        ///หมายเหตุเพิ่มเติม
        /// <summary>
        public string FTSvnRemark { get; set; }
    }
}