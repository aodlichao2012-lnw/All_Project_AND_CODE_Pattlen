using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Doc.PdtTnfBch
{
    public class cmlTCNTPdtTbxDT
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string FTXthDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNXtdSeqNo { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ชื่อสินค้า
        /// <summary>
        public string FTXtdPdtName { get; set; }

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
        public Nullable<decimal> FCXtdFactor { get; set; }

        /// <summary>
        ///บาร์โค้ด
        /// <summary>
        public string FTXtdBarCode { get; set; }

        /// <summary>
        ///ประเภทภาษี 1:มีภาษี, 2:ไม่มีภาษี
        /// <summary>
        public string FTXtdVatType { get; set; }

        /// <summary>
        ///รหัสภาษี ณ. ซื้อ
        /// <summary>
        public string FTVatCode { get; set; }

        /// <summary>
        ///อัตราภาษี ณ. ซื้อ
        /// <summary>
        public Nullable<decimal> FCXtdVatRate { get; set; }

        /// <summary>
        ///จำนวนชื้น ตาม หน่วย
        /// <summary>
        public Nullable<decimal> FCXtdQty { get; set; }

        /// <summary>
        ///จำนวนรวมหน่วยเล็กสุด (FCXpdQty*FCXpdFactor)
        /// <summary>
        public Nullable<decimal> FCXtdQtyAll { get; set; }

        /// <summary>
        ///ราคาซื้อ ตาม หน่วย * อัตราแลกเปลี่ยน(HD.FCXphRteFac)
        /// <summary>
        public Nullable<decimal> FCXtdSetPrice { get; set; }

        /// <summary>
        ///มูลค่ารวมก่อนลด (Qty*SetPrice) ทุกกรณี (ไม่เปลี่ยน)
        /// <summary>
        public Nullable<decimal> FCXtdAmt { get; set; }

        /// <summary>
        ///มูลค่าภาษี IN: Net-((Net*100)/(100+VatRate)) ,EX: ((Net*(100+VatRate))/100)-Net
        /// <summary>
        public Nullable<decimal> FCXtdVat { get; set; }

        /// <summary>
        ///มูลค่าแยกภาษี (Net-FCXpdVat)
        /// <summary>
        public Nullable<decimal> FCXtdVatable { get; set; }

        /// <summary>
        ///มูลค่าสุทธิก่อนท้ายบิล (FCXpdVat+FCXpdVatable)
        /// <summary>
        public Nullable<decimal> FCXtdNet { get; set; }

        /// <summary>
        ///ต้นทุนรวมใน (FCXpdVat+FCXpdVatable)
        /// <summary>
        public Nullable<decimal> FCXtdCostIn { get; set; }

        /// <summary>
        ///ต้นทุนแยกนอก (FCXpdVatable)
        /// <summary>
        public Nullable<decimal> FCXtdCostEx { get; set; }

        /// <summary>
        ///สถานะตัดสต๊อก ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// <summary>
        public string FTXtdStaPrcStk { get; set; }

        /// <summary>
        ///ระดับสิน(ค้าชุด)
        /// <summary>
        public Nullable<int> FNXtdPdtLevel { get; set; }

        /// <summary>
        ///รหัสสินค้าชุด
        /// <summary>
        public string FTXtdPdtParent { get; set; }

        /// <summary>
        ///จำนวนต่อชุด
        /// <summary>
        public Nullable<decimal> FCXtdQtySet { get; set; }

        /// <summary>
        ///สถานะ สินค้าชุด 1:ทั่วไป, 2:สินค้าประกอบ, 3:สินค้าชุด
        /// <summary>
        public string FTXtdPdtStaSet { get; set; }

        /// <summary>
        ///หมายเหตุรายการ
        /// <summary>
        public string FTXtdRmk { get; set; }

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
