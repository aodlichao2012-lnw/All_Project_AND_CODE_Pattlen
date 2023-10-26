using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.UpdProductStk
{
    public class cmlTCNTPdtStkBal
    {
        //public string FTBchCode { get; set; }
        //public string FTWahCode { get; set; }
        //public string FTPdtCode { get; set; }
        //public decimal FCStkQty { get; set; }
        //public DateTime? FDLastUpdOn { get; set; }
        //public string FTLastUpdBy { get; set; }
        //public DateTime? FDCreateOn { get; set; }
        //public string FTCreateBy { get; set; }

        // +++++++++++++++++++++++++++++++


        //*Arm 63-01-24 - ปรับโครงสร้าง Database ใหม่

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสคลังสินค้า
        /// </summary>
        public string FTWahCode { get; set; }

        /// <summary>
        ///รหัสควบคุมสต๊อก
        /// </summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///จำนวนสินค้า
        /// </summary>
        public Nullable<decimal> FCStkQty { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

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