using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.Database
{
    public class cmlTCNTMemTxnSale
    {
        /// <summary>
        ///รหัสกลุ่มบริษัท
        /// </summary>
        public string FTCgpCode { get; set; }

        /// <summary>
        ///รหัสสมาชิก / เลขที่บัตรประจำตัวประชาชน/Passport
        /// </summary>
        public string FTMemCode { get; set; }

        /// <summary>
        ///เอกสารอ้างอิง
        /// </summary>
        public string FTTxnRefDoc { get; set; }

        /// <summary>
        ///อ้างอิงเอกสาร เช่น กรณีคืนอ้างอิงบิลขาย , บิลขายเก็บค่าเป็นว่าง
        /// </summary>
        public string FTTxnRefInt { get; set; }

        /// <summary>
        ///รหัสผู้จำหน่าย
        /// </summary>
        public string FTTxnRefSpl { get; set; }

        /// <summary>
        ///วันที่เอกสาร
        /// </summary>
        public Nullable<DateTime> FDTxnRefDate { get; set; }

        /// <summary>
        ///ยอดซื้อสุทธิ
        /// </summary>
        public Nullable<decimal> FCTxnRefGrand { get; set; }

        /// <summary>
        ///เงื่อนไข อัตราส่วนมูลค่าซื้อ
        /// </summary>
        public Nullable<decimal> FCTxnPntOptBuyAmt { get; set; }

        /// <summary>
        ///เงื่อนไข อัตราแต้มที่จะได้
        /// </summary>
        public Nullable<decimal> FCTxnPntOptGetQty { get; set; }

        /// <summary>
        ///จำนวนแต้มสะสมขณะสร้างเอกสาร
        /// </summary>
        public Nullable<decimal> FCTxnPntB4Bill { get; set; }

        /// <summary>
        ///วันที่เริ่มใช้งานแต้มได้
        /// </summary>
        public Nullable<DateTime> FDTxnPntStart { get; set; }

        /// <summary>
        ///วันที่เริ่มใช้งานแต้มได้
        /// </summary>
        public Nullable<DateTime> FDTxnPntExpired { get; set; }

        /// <summary>
        ///จำนวนแต้มที่ได้
        /// </summary>
        public Nullable<decimal> FCTxnPntBillQty { get; set; }

        /// <summary>
        ///แต้มที่ใช้ไปแล้ว
        /// </summary>
        public Nullable<decimal> FCTxnPntUsed { get; set; }

        /// <summary>
        ///แต้มที่หมดอายุ
        /// </summary>
        public Nullable<decimal> FCTxnPntExpired { get; set; }

        /// <summary>
        ///สถานะ 1:คำนวนได้ 2:ไม่นำไปคำนวนแล้ว priority สูงกว่าช่วง start-expired
        /// </summary>
        public string FTTxnPntStaClosed { get; set; }

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

        /// <summary>
        ///ประเภทบิล   1 = บิลขาย , 2:บิล Void   Def: 1
        /// </summary>
        public string FTTxnPntDocType { get; set; }
    }
}