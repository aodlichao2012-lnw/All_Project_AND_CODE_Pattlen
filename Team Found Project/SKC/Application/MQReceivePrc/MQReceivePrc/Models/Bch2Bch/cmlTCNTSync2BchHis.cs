using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Bch2Bch
{
    public class cmlTCNTSync2BchHis
    {
        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNSynSeqNo { get; set; }

        /// <summary>
        ///วันที่ส่ง/เวลา
        /// <summary>
        public Nullable<DateTime> FDSynDate { get; set; }

        /// <summary>
        ///สาขาต้นทาง
        /// <summary>
        public string FTSynBchFrm { get; set; }

        /// <summary>
        ///สาขาปลายทาง
        /// <summary>
        public string FTSynBchTo { get; set; }

        /// <summary>
        ///ชื่อไฟล์ file.text
        /// <summary>
        public string FTSynFileName { get; set; }

        /// <summary>
        ///URL ไฟล์สำหรับดาวน์โหลด เช่น https://1.1.1.1/xxx/xxx/
        /// <summary>
        public string FTSynFileURL { get; set; }

        /// <summary>
        ///วันที่รับ/เวลา
        /// <summary>
        public Nullable<DateTime> FDSynRcvDate { get; set; }

        /// <summary>
        ///สถานะ sync  1 : สำหรับ,2:ไม่สำเร็จ,3:ซ่อมแล้ว
        /// <summary>
        public string FTSynStatus { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTSynRmk { get; set; }
    }
}
