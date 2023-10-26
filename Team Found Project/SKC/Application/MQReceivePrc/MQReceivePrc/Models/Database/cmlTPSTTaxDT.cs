using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTPSTTaxDT
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNXsdSeqNo { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ชื่อสินค้า
        /// <summary>
        public string FTXsdPdtName { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// <summary>
        public string FTPunCode { get; set; }

        /// <summary>
        ///ชื่อหน่วยสินค้า
        /// <summary>
        public string FTPunName { get; set; }

        /// <summary>
        ///อัตราส่วนต่อหน่วย
        /// <summary>
        public Nullable<decimal> FCXsdFactor { get; set; }

        /// <summary>
        ///บาร์โค้ด
        /// <summary>
        public string FTXsdBarCode { get; set; }

        /// <summary>
        ///รหัส ซีเรียล
        /// <summary>
        public string FTSrnCode { get; set; }

        /// <summary>
        ///ประเภทภาษี 1:มีภาษี, 2:ไม่มีภาษี
        /// <summary>
        public string FTXsdVatType { get; set; }

        /// <summary>
        ///รหัสภาษี ณ. ซื้อ
        /// <summary>
        public string FTVatCode { get; set; }

        /// <summary>
        ///อัตราภาษี ณ. ซื้อ
        /// <summary>
        public Nullable<decimal> FCXsdVatRate { get; set; }

        /// <summary>
        ///ใช้ราคาขาย 1:บังคับ, 2:แก้ไข, 3:เครื่องชั่ง,4: นน.
        /// <summary>
        public string FTXsdSaleType { get; set; }

        /// <summary>
        ///จากราคาซื้อ ตาม หน่วย * อัตราแลกเปลี่ยน(HD.FCXphRteFac)
        /// <summary>
        public Nullable<decimal> FCXsdSalePrice { get; set; }

        /// <summary>
        ///จำนวนชื้น ตาม หน่วย
        /// <summary>
        public Nullable<decimal> FCXsdQty { get; set; }

        /// <summary>
        ///จำนวนรวมหน่วยเล็กสุด (FCXpdQty*FCXpdFactor)
        /// <summary>
        public Nullable<decimal> FCXsdQtyAll { get; set; }

        /// <summary>
        ///ราคาซื้อ ตาม หน่วย * อัตราแลกเปลี่ยน(HD.FCXphRteFac)
        /// <summary>
        public Nullable<decimal> FCXsdSetPrice { get; set; }

        /// <summary>
        ///มูลค่ารวมก่อนลด
        /// <summary>
        public Nullable<decimal> FCXsdAmtB4DisChg { get; set; }

        /// <summary>
        ///ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// <summary>
        public string FTXsdDisChgTxt { get; set; }

        /// <summary>
        ///มูลค่ารวมส่วนลด
        /// <summary>
        public Nullable<decimal> FCXsdDis { get; set; }

        /// <summary>
        ///มูลค่ารวมส่วนชาร์จ
        /// <summary>
        public Nullable<decimal> FCXsdChg { get; set; }

        /// <summary>
        ///มูลค่าสุทธิก่อนท้ายบิล (FCXpdAmt-FCXpdDis+FCXpdChg)
        /// <summary>
        public Nullable<decimal> FCXsdNet { get; set; }

        /// <summary>
        ///มูลค่าสุทธิหลังท้ายบิล (Net-SUM(Disท้ายบิล))
        /// <summary>
        public Nullable<decimal> FCXsdNetAfHD { get; set; }

        /// <summary>
        ///มูลค่าภาษี IN: NetAfHD-((NetAfHD*100)/(100+VatRate)) ,EX: ((NetAfHD*(100+VatRate))/100)-NetAfHD
        /// <summary>
        public Nullable<decimal> FCXsdVat { get; set; }

        /// <summary>
        ///มูลค่าแยกภาษี (NetAfHD-FCXpdVat)
        /// <summary>
        public Nullable<decimal> FCXsdVatable { get; set; }

        /// <summary>
        ///มูลค่าภาษี ณ. ที่จ่าย (FCXpdVatable* FCXpdWhtRate%)
        /// <summary>
        public Nullable<decimal> FCXsdWhtAmt { get; set; }

        /// <summary>
        ///รหัส ภาษี ณ. ที่จ่าย
        /// <summary>
        public string FTXsdWhtCode { get; set; }

        /// <summary>
        ///อัตราภาษี ณ. ที่จ่าย
        /// <summary>
        public Nullable<decimal> FCXsdWhtRate { get; set; }

        /// <summary>
        ///ต้นทุนรวมใน (FCXpdVat+FCXpdVatable)
        /// <summary>
        public Nullable<decimal> FCXsdCostIn { get; set; }

        /// <summary>
        ///ต้นทุนแยกนอก (FCXpdVatable)
        /// <summary>
        public Nullable<decimal> FCXsdCostEx { get; set; }

        /// <summary>
        ///สถานะ สินค้า 1:ขาย, 2:คืน, 3:แถม, 4: ยกเลิก (Void)
        /// <summary>
        public string FTXsdStaPdt { get; set; }

        /// <summary>
        ///จำนวนคงเหลือ ตามหน่วย (Default:FCXpdQty)
        /// <summary>
        public Nullable<decimal> FCXsdQtyLef { get; set; }

        /// <summary>
        ///จำนวนคืนตามหน่วย (Default:0)
        /// <summary>
        public Nullable<decimal> FCXsdQtyRfn { get; set; }

        /// <summary>
        ///สถานะตัดสต๊อก ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// <summary>
        public string FTXsdStaPrcStk { get; set; }

        /// <summary>
        ///สถานะอนุญาต ลด/ชาร์จ  1:อนุญาต , 2:ไม่อนุญาต
        /// <summary>
        public string FTXsdStaAlwDis { get; set; }

        /// <summary>
        ///ระดับสิน(ค้าชุด)
        /// <summary>
        public Nullable<int> FNXsdPdtLevel { get; set; }

        /// <summary>
        ///รหัสสินค้าชุด
        /// <summary>
        public string FTXsdPdtParent { get; set; }

        /// <summary>
        ///จำนวนต่อชุด
        /// <summary>
        public Nullable<decimal> FCXsdQtySet { get; set; }

        /// <summary>
        ///สถานะ สินค้าชุด 1:ทั่วไป, 2:สินค้าประกอบ, 3:สินค้าชุด
        /// <summary>
        public string FTPdtStaSet { get; set; }

        /// <summary>
        ///หมายเหตุรายการ
        /// <summary>
        public string FTXsdRmk { get; set; }

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
