using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtHDCstTmp
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
        ///เงื่อนไขอายุสมาชิก 1: น้อยกว่า 2: น้อยกว่าหรือเท่ากับ 3: เท่ากับ 4:มากกว่า หรือเท่ากับ 5:มากกว่า
        /// <summary>
        public string FTSpmStaLimitCst { get; set; }

        /// <summary>
        ///อายุการเป็นสมาชิก(day) เช่น 365 :  ภายใน1 ปี  ,0: ไม่จำกัด
        /// <summary>
        public string FNSpmMemAgeLT { get; set; }

        /// <summary>
        ///เงื่อนไขเฉพาะสมาชิกที่ตรงกับวันเกิด (1:ใช้งาน,2:ไม่ใช้งาน)
        /// <summary>
        public string FTSpmStaChkCstDOB { get; set; }

        /// <summary>
        ///เก็บจำนวนเดือนก่อนหน้าเดือนเกิด ที่จะเริ่มต้นได้รับโปรโมชั่น
        /// <summary>
        public Nullable<Int64> FNPmhCstDobPrev { get; set; }

        /// <summary>
        ///เก็บจำนวนเดือนหลังจากเดือนเกิด ที่จะสิ้นสุดได้รับโปรโมชั่น
        /// <summary>
        public Nullable<Int64> FNPmhCstDobNext { get; set; }
    }
}
