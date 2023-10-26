using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Sale
{
    public class cmlTCNTMemTxnRedeem
    {
        /// <summary>
        ///รหัสกลุ่มบริษัท
        /// <summary>
        public string FTCgpCode { get; set; }

        /// <summary>
        ///รหัสสมาชิก / เลขที่บัตรประจำตัวประชาชน/Passport
        /// <summary>
        public string FTMemCode { get; set; }

        /// <summary>
        ///เอกสารอ้างอิง
        /// <summary>
        public string FTRedRefDoc { get; set; }

        /// <summary>
        ///รหัสผู้จำหน่าย
        /// <summary>
        public string FTRedRefSpl { get; set; }

        /// <summary>
        ///อ้างอิงเอกสาร เช่น กรณีคืนอ้างอิงบิลขาย , บิลขายเก็บค่าเป็นว่าง
        /// </summary>
        public string FTRedRefInt { get; set; }     //*Arm 63-03-21

        /// <summary>
        ///วันที่เอกสาร
        /// <summary>
        public Nullable<DateTime> FDRedRefDate { get; set; }

        /// <summary>
        ///จำนวนแต้มสะสม
        /// <summary>
        public Nullable<decimal> FCRedPntB4Bill { get; set; }

        /// <summary>
        ///จำนวนแต้มที่ใช้
        /// <summary>
        public Nullable<decimal> FCRedPntBillQty { get; set; }

        /// <summary>
        ///สถานะ 1:คำนวนได้ 2:ไม่นำไปคำนวนแล้ว priority สูงกว่าช่วง start-expired
        /// <summary>
        public string FTRedPntStaClosed { get; set; }

        /// <summary>
        ///วันที่เริ่มใช้งานแต้มได้
        /// <summary>
        public Nullable<DateTime> FDRedPntStart { get; set; }

        /// <summary>
        /// วันที่แต้มหมดอายุ
        /// </summary>
        public Nullable<DateTime> FDRedPntExpired { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }

        /// <summary>
        ///ประเภทบิล   1 = บิลขาย , 2:บิล Void   Def: 1
        /// </summary>
        public string FTRedPntDocType { get; set; } //*Arm 63-03-31
    }
}
