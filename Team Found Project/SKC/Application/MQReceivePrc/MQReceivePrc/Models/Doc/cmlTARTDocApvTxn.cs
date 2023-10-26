using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Doc
{
    public class cmlTARTDocApvTxn
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///อ้างอิงเลขที่    FTXshDocNo
        /// </summary>
        public string FTDatRefCode { get; set; }

        /// <summary>
        ///FNXshDocType      อ้างอิงประเภทเอกสาร
        /// </summary>
        public string FTDatRefType { get; set; }

        /// <summary>
        ///สถานะใบสั่งจ่าย    2 = รอเภสัชยืนยัน  3 = รอพยาบาลจ่ายของ 4 = รอพยาบาลถ่ายสินค้า
        /// </summary>
        public Nullable<int> FNDatApvSeq { get; set; }

        /// <summary>
        ///User ที่อนุมัติ
        /// </summary>
        public string FTDatUsrApv { get; set; }

        /// <summary>
        ///วันที่อนุมัติ
        /// </summary>
        public Nullable<DateTime> FDDatDateApv { get; set; }

        /// <summary>
        ///สถานะประมวลผล  null ยังไม่ประมวลผล 1 ประมวลผลแล้ว
        /// </summary>
        public string FTDatStaPrc { get; set; }

        /// <summary>
        ///หมายเหตุการอนุมัติ
        /// </summary>
        public string FTDatRmk { get; set; }

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
