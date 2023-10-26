using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.Database
{
    public class cmlTPSTTaxDTDis
    {
        ///// <summary>
        /////สาขาสร้าง
        ///// <summary>
        //public string FTBchCode { get; set; }

        ///// <summary>
        /////เลขที่เอกสาร
        ///// <summary>
        //public string FTXshDocNo { get; set; }

        ///// <summary>
        /////ลำดับ
        ///// <summary>
        //public Nullable<int> FNXsdSeqNo { get; set; }

        ///// <summary>
        /////วัน/เวลาทำรายการ [dd/mm/yyyy H:mm:ss]
        ///// <summary>
        //public Nullable<DateTime> FDXddDateIns { get; set; }

        ///// <summary>
        /////สถานะส่วนลด 1: ลดรายการ ,2: ลดท้ายบิล
        ///// <summary>
        //public Nullable<int> FNXddStaDis { get; set; }

        ///// <summary>
        /////ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        ///// <summary>
        //public string FTXddDisChgTxt { get; set; }

        ///// <summary>
        /////ประเภทลดชาร์จ 1:ลดบาท 2: ลด % 3: ชาร์จบาท 4: ชาร์จ %
        ///// <summary>
        //public string FTXddDisChgType { get; set; }

        ///// <summary>
        /////มูลค่าสุทธิก่อนลดชาร์จ
        ///// <summary>
        //public Nullable<decimal> FCXddNet { get; set; }

        ///// <summary>
        /////ยอดลด/ชาร์จ
        ///// <summary>
        //public Nullable<decimal> FCXddValue { get; set; }

        // +++++++++++++++++++++++++++++++




        //*Arm 63-01-24 - ปรับโครงสร้าง Database ใหม่

        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<int> FNXsdSeqNo { get; set; }

        /// <summary>
        ///วัน/เวลาทำรายการ [dd/mm/yyyy H:mm:ss]
        /// </summary>
        public Nullable<DateTime> FDXddDateIns { get; set; }

        /// <summary>
        ///สถานะส่วนลด 1: ลดรายการ ,2: ลดท้ายบิล
        /// </summary>
        public Nullable<int> FNXddStaDis { get; set; }

        /// <summary>
        ///ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// </summary>
        public string FTXddDisChgTxt { get; set; }

        /// <summary>
        ///ประเภทลดชาร์จ 1:ลดบาท 2: ลด % 3: ชาร์จบาท 4: ชาร์จ %
        /// </summary>
        public string FTXddDisChgType { get; set; }

        /// <summary>
        ///มูลค่าสุทธิก่อนลดชาร์จ
        /// </summary>
        public Nullable<decimal> FCXddNet { get; set; }

        /// <summary>
        ///ยอดลด/ชาร์จ
        /// </summary>
        public Nullable<decimal> FCXddValue { get; set; }
    }
}