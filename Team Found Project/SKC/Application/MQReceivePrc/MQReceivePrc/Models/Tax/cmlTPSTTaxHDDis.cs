using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Tax
{
    public class cmlTPSTTaxHDDis
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///วัน/เวลาทำรายการ [dd/mm/yyyy H:mm:ss]
        /// <summary>
        public Nullable<DateTime> FDXhdDateIns { get; set; }

        /// <summary>
        ///ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// <summary>
        public string FTXhhDisChgTxt { get; set; }

        /// <summary>
        ///ประเภทลดชาร์จ 1:ลดบาท 2: ลด % 3: ชาร์จบาท 4: ชาร์จ %
        /// <summary>
        public string FTXhdDisChgType { get; set; }

        /// <summary>
        ///ยอดรวมหลังลด (FCXshTotalAfDisChgV+FCXshTotalAfDisChgNV)
        /// <summary>
        public Nullable<decimal> FCXhdTotalAfDisChg { get; set; }

        /// <summary>
        ///ยอดลด/ชาร์จ
        /// <summary>
        public Nullable<decimal> FCXhdDisChg { get; set; }

        /// <summary>
        ///มูลค่าลด/ชาร์จ
        /// <summary>
        public Nullable<decimal> FCXhdAmt { get; set; }
    }
}
