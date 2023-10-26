using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.PdtStkCrd
{
    public class cmlTCNTPdtStkCrd
    {
        //public int? FNStkCrdID { get; set; }
        //public string FTBchCode { get; set; }
        //public DateTime? FDStkDate { get; set; }
        //public string FTStkDocNo { get; set; }
        //public string FTWahCode { get; set; }
        //public string FTPdtCode { get; set; }
        //public string FTStkType { get; set; }
        //public decimal FCStkQty { get; set; }
        //public decimal FCStkSetPrice { get; set; }
        //public decimal FCStkCostIn { get; set; }
        //public decimal FCStkCostEx { get; set; }
        //public DateTime? FDCreateOn { get; set; }
        //public string FTCreateBy { get; set; }

        // +++++++++++++++++++++++++++++++


        //*Arm 63-01-24 - ปรับโครงสร้าง Database ใหม่

        /// <summary>
        ///(AUTONUMBER)รหัสข้อมูล
        /// </summary>
        public Nullable<Int64> FNStkCrdID { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///วันที่เคลื่อนไหว
        /// </summary>
        public Nullable<DateTime> FDStkDate { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTStkDocNo { get; set; }

        /// <summary>
        ///รหัสคลัง
        /// </summary>
        public string FTWahCode { get; set; }

        /// <summary>
        ///รหัสสต้อกสินค้า
        /// </summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///สถานะสินค้า 1:เข้า/ซื้อ, 2:ออก 3:ขาย FullSlip/DN, 4:คืนใบ ABB/CN  ,5:Adjust
        /// </summary>
        public string FTStkType { get; set; }

        /// <summary>
        ///จำนวน (* Factor แล้ว)
        /// </summary>
        public Nullable<decimal> FCStkQty { get; set; }

        /// <summary>
        ///ราคาขาย
        /// </summary>
        public Nullable<decimal> FCStkSetPrice { get; set; }

        /// <summary>
        ///ราคาต้นทุน รวมใน
        /// </summary>
        public Nullable<decimal> FCStkCostIn { get; set; }

        /// <summary>
        ///ราคาต้นทุน แยกนอก
        /// </summary>
        public Nullable<decimal> FCStkCostEx { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }
    }
}