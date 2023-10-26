using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTPSTVoidDT
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///ลำดับการ Void
        /// <summary>
        public Nullable<Int64> FNVidNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNXidSeqNo { get; set; }

        /// <summary>
        ///ประเภทการ Void 1:ยกเลิกสินค้า 2:ยกเลิกบิล
        /// <summary>
        public string FTVidType { get; set; }

        /// <summary>
        ///รหัสเหตุผล
        /// <summary>
        public string FTRsnCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string FTXihDocNo { get; set; }

        /// <summary>
        ///ประเภทเอกสาร
        /// <summary>
        public string FTXihDocType { get; set; }

        /// <summary>
        ///วันที่เอกสาร
        /// <summary>
        public Nullable<DateTime> FDXihDocDate { get; set; }

        /// <summary>
        ///เวลาเอกสาร
        /// <summary>
        public string FTXihDocTime { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ชื่อสินค้า
        /// <summary>
        public string FTXidPdtName { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// <summary>
        public string FTPunCode { get; set; }

        /// <summary>
        ///อัตราส่วนต่อหน่วย
        /// <summary>
        public Nullable<decimal> FCXidFactor { get; set; }

        /// <summary>
        ///บาร์โค้ด
        /// <summary>
        public string FTXidBarCode { get; set; }

        /// <summary>
        ///รหัส ซีเรียล
        /// <summary>
        public string FTSrnCode { get; set; }

        /// <summary>
        ///ประเภทภาษี 1:มีภาษี, 2:ไม่มีภาษี
        /// <summary>
        public string FTXidVatType { get; set; }

        /// <summary>
        ///รหัสภาษี ณ. ขาย
        /// <summary>
        public string FTVatCode { get; set; }

        /// <summary>
        ///อัตราภาษี ณ. ขาย
        /// <summary>
        public Nullable<decimal> FCXidVatRate { get; set; }

        /// <summary>
        ///ใช้ราคาขาย 1:บังคับ, 2:แก้ไข, 3:เครื่องชั่ง,4: นน.
        /// <summary>
        public string FTXidSaleType { get; set; }

        /// <summary>
        ///จากราคาขาย ตาม หน่วย
        /// <summary>
        public Nullable<decimal> FCXidSalePrice { get; set; }

        /// <summary>
        ///จำนวนชื้น ตาม หน่วย
        /// <summary>
        public Nullable<decimal> FCXidQty { get; set; }

        /// <summary>
        ///ราคาขาย ตาม หน่วย
        /// <summary>
        public Nullable<decimal> FCXidSetPrice { get; set; }

        /// <summary>
        ///มูลค่ารวมก่อนลด (Qty*SetPrice)
        /// <summary>
        public Nullable<decimal> FCXidB4DisChg { get; set; }

        /// <summary>
        ///จำนวนรวมหน่วยเล็กสุด (Qty*Factor)
        /// <summary>
        public Nullable<decimal> FCXidQtyAll { get; set; }

        /// <summary>
        ///มูลค่าสุทธิก่อนท้ายบิล
        /// <summary>
        public Nullable<decimal> FCXidNet { get; set; }

        /// <summary>
        ///มูลค่าสุทธิหลังท้ายบิล
        /// <summary>
        public Nullable<decimal> FCXidNetTotal { get; set; }

        /// <summary>
        ///มูลค่าภาษี
        /// <summary>
        public Nullable<decimal> FCXidVat { get; set; }

        /// <summary>
        ///มูลค่าแยกภาษี
        /// <summary>
        public Nullable<decimal> FCXidVatable { get; set; }

        /// <summary>
        ///ต้นทุนรวมใน
        /// <summary>
        public Nullable<decimal> FCXidCostIn { get; set; }

        /// <summary>
        ///ต้นทุนแยกนอก
        /// <summary>
        public Nullable<decimal> FCXidCostEx { get; set; }

        /// <summary>
        ///สถานะ สินค้า 1:ขาย, 2:คืน, 3:แถม, 4: ยกเลิก (Void)
        /// <summary>
        public string FTXidStaPdt { get; set; }

        /// <summary>
        ///จำนวนคงเหลือ ตามหน่วย
        /// <summary>
        public Nullable<decimal> FCXidQtyLef { get; set; }

        /// <summary>
        ///จำนวนคืนตามหน่วย
        /// <summary>
        public Nullable<decimal> FCXidQtyRfn { get; set; }

        /// <summary>
        ///สถานะตัดสต๊อก ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// <summary>
        public string FTXidStaPrcStk { get; set; }

        /// <summary>
        ///ระดับสิน(ค้าชุด)
        /// <summary>
        public Nullable<int> FNXidPdtLevel { get; set; }

        /// <summary>
        ///รหัสสินค้าชุด
        /// <summary>
        public string FTXidPdtParent { get; set; }

        /// <summary>
        ///จำนวนต่อชุด
        /// <summary>
        public Nullable<decimal> FCXidQtySet { get; set; }

        /// <summary>
        ///สถานะ สินค้าชุด 1:ทั่วไป, 2:สินค้าประกอบ, 3:สินค้าชุด
        /// <summary>
        public string FTPdtStaSet { get; set; }

        /// <summary>
        ///หมายเหตุรายการ
        /// <summary>
        public string FTXidRmk { get; set; }

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
    }
}
