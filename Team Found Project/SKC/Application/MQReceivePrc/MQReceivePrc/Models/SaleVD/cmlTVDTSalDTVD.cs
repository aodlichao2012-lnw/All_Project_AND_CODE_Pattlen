using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleVD
{
    public class cmlTVDTSalDTVD
    {
        /// <summary>
        /// ลำดับ Cabinet
        /// </summary>
        public Nullable<int> FNCabSeq { get; set; }     //*Arm 63-01-28
        
        /// <summary>
        /// คลังตัดจ่าย (ตาม Planogram)
        /// </summary>
        public string FTWahCode { get; set; }   //*Arm 63-01-28

        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNXsdSeqNo { get; set; }

        /// <summary>
        ///บาร์โค้ด
        /// <summary>
        public string FTXsdBarcode { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ชั้นที่
        /// <summary>
        public Nullable<Int64> FNLayRow { get; set; }

        /// <summary>
        ///คอลัมน์
        /// <summary>
        public Nullable<Int64> FNLayCol { get; set; }

        /// <summary>
        ///สถานะการจ่ายสินค้า  1:จ่ายสำเร็จ 2:ไม่จ่ายสำเร็จ
        /// <summary>
        public string FTXsvStaPayItem { get; set; }
    }
}
