using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNTPdtPrice4PDTTmp
    {
        //*Arm 63-03-27

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// </summary>
        public string FTPunCode { get; set; }

        /// <summary>
        ///วันที่เริ่ม
        /// </summary>
        public Nullable<DateTime> FDPghDStart { get; set; }

        /// <summary>
        ///เวลาเริ่มมีผล
        /// </summary>
        public string FTPghTStart { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string FTPplCode { get; set; }

        /// <summary>
        ///วันที่หมดอายุ
        /// </summary>
        public Nullable<DateTime> FDPghDStop { get; set; }

        /// <summary>
        ///เวลาหมดอายุ
        /// </summary>
        public string FTPghTStop { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTPghDocNo { get; set; }

        /// <summary>
        ///ประเภทราคา 1:BasePrice(FTPghStaAdj=1), 2:Price Off
        /// </summary>
        public string FTPghDocType { get; set; }

        /// <summary>
        ///ประเภทการปรับ 1:New Price 2:ปรับลด % 3:ปรับลด มูลค่า 4: ปรับเพิ่ม % 5 ปรับเพิ่ม มูลค่า
        /// </summary>
        public string FTPghStaAdj { get; set; }

        /// <summary>
        ///ราคาขายปลีก ปกติ
        /// </summary>
        public Nullable<decimal> FCPgdPriceRet { get; set; }

        /// <summary>
        ///ราคาขายส่ง ปกติ
        /// </summary>
        public Nullable<decimal> FCPgdPriceWhs { get; set; }

        /// <summary>
        ///ราคาขาย online ปกติ
        /// </summary>
        public Nullable<decimal> FCPgdPriceNet { get; set; }

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
