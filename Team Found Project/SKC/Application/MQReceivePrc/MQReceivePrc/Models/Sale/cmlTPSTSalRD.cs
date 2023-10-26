using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Sale
{
    public class cmlTPSTSalRD
    {
        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///ลำดับการ Redeem
        /// </summary>
        public Nullable<int> FNXrdSeqNo { get; set; }

        /// <summary>
        ///ประเภทเอกสาร 1: Redeem สินค้า แต้ม+เงิน 2: Redeem ส่วนลด
        /// </summary>
        public string FTRdhDocType { get; set; }

        /// <summary>
        ///RD Type 1 อ้างอิงลำดับใน DT ,RD Type 2 อ้างอิงลำดับใน RC
        /// </summary>
        public Nullable<int> FNXrdRefSeq { get; set; }

        /// <summary>
        ///เลขที่อ้างอิง
        /// </summary>
        public string FTXrdRefCode { get; set; }

        /// <summary>
        ///จำนวนสินค้าที่ Redeem
        /// </summary>
        public Nullable<decimal> FCXrdPdtQty { get; set; }

        /// <summary>
        ///จำนวนแต้มที่ใช้แลก
        /// </summary>
        public Nullable<Int64> FNXrdPntUse { get; set; }
    }
}
