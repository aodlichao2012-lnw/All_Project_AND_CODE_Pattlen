using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.UpdSaleVD
{
    /// <summary>
    /// Class model TVDTSalDTVD
    /// </summary>
    public class cmlTVDTSalDTVD
    {
        ///// <summary>
        ///// สาขาสร้าง
        ///// </summary>
        //public string FTBchCode { get; set; }

        ///// <summary>
        ///// เลขที่เอกสาร
        ///// </summary>
        //public string FTXshDocNo { get; set; }

        ///// <summary>
        ///// ลำดับ
        ///// </summary>
        //public int FNXsdSeqNo { get; set; }

        ///// <summary>
        ///// บาร์โค้ด
        ///// </summary>
        //public string FTXsdBarcode { get; set; }

        ///// <summary>
        ///// รหัสสินค้า
        ///// </summary>
        //public string FTPdtCode { get; set; }

        ///// <summary>
        ///// ชั้นที่
        ///// </summary>
        //public long FNLayRow { get; set; }

        ///// <summary>
        ///// คอลัมน์
        ///// </summary>
        //public long FNLayCol { get; set; }

        ///// <summary>
        ///// สถานะการจ่ายสินค้า  1:จ่ายสำเร็จ 2:ไม่จ่ายสำเร็จ
        ///// </summary>
        //public string FTXsvStaPayItem { get; set; }



        //*Arm 63-01-24 - ปรับโครงสร้าง Database ใหม่

        /// <summary>
        /// ลำดับ Cabinet
        /// </summary>
        public Nullable<int> FNCabSeq { get; set; }

        /// <summary>
        /// คลังตัดจ่าย (ตาม Planogram)
        /// </summary>
        public string FTWahCode { get; set; }   //*Arm 63-01-28

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
        ///บาร์โค้ด
        /// </summary>
        public string FTXsdBarcode { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ชั้นที่
        /// </summary>
        public Nullable<Int64> FNLayRow { get; set; }

        /// <summary>
        ///คอลัมน์
        /// </summary>
        public Nullable<Int64> FNLayCol { get; set; }

        /// <summary>
        ///สถานะการจ่ายสินค้า  1:จ่ายสำเร็จ 2:ไม่จ่ายสำเร็จ
        /// </summary>
        public string FTXsvStaPayItem { get; set; }


    }
}