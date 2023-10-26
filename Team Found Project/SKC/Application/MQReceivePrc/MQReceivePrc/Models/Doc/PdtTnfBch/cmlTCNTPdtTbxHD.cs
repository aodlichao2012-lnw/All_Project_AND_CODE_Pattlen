using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Doc.PdtTnfBch
{
    public class cmlTCNTPdtTbxHD
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร  XXYYMM-1234567
        /// <summary>
        public string FTXthDocNo { get; set; }

        /// <summary>
        ///วันที่/เวลา เอกสาร dd/mm/yyyy H:mm:ss
        /// <summary>
        public Nullable<DateTime> FDXthDocDate { get; set; }

        /// <summary>
        ///ภาษีมูลค่าเพิ่ม 1:รวมใน, 2:แยกนอก
        /// <summary>
        public string FTXthVATInOrEx { get; set; }

        /// <summary>
        ///แผนก
        /// <summary>
        public string FTDptCode { get; set; }

        /// <summary>
        ///รหัสสาขาต้นทาง
        /// <summary>
        public string FTXthBchFrm { get; set; }

        /// <summary>
        ///รหัสสาขาปลายทาง
        /// <summary>
        public string FTXthBchTo { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// <summary>
        public string FTXthMerchantFrm { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// <summary>
        public string FTXthMerchantTo { get; set; }

        /// <summary>
        ///ร้านค้า
        /// <summary>
        public string FTXthShopFrm { get; set; }

        /// <summary>
        ///ร้านค้า
        /// <summary>
        public string FTXthShopTo { get; set; }

        /// <summary>
        ///รหัสคลัง จาก
        /// <summary>
        public string FTXthWhFrm { get; set; }

        /// <summary>
        ///รหัสคลัง ไปยัง
        /// <summary>
        public string FTXthWhTo { get; set; }

        /// <summary>
        ///พนักงาน Key
        /// <summary>
        public string FTUsrCode { get; set; }

        /// <summary>
        ///พนักงานขาย
        /// <summary>
        public string FTSpnCode { get; set; }

        /// <summary>
        ///ผู้อนุมัติ
        /// <summary>
        public string FTXthApvCode { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่เอกสาร ภายนอก
        /// <summary>
        public string FTXthRefExt { get; set; }

        /// <summary>
        ///อ้างอิง วันที่เอกสาร ภายนอก
        /// <summary>
        public Nullable<DateTime> FDXthRefExtDate { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่เอกสาร ภายใน
        /// <summary>
        public string FTXthRefInt { get; set; }

        /// <summary>
        ///อ้างอิง วันที่เอกสาร ภายใน
        /// <summary>
        public Nullable<DateTime> FDXthRefIntDate { get; set; }

        /// <summary>
        ///จำนวนครั้งที่พิมพ์
        /// <summary>
        public Nullable<int> FNXthDocPrint { get; set; }

        /// <summary>
        ///ยอดรวมก่อนลด
        /// <summary>
        public Nullable<decimal> FCXthTotal { get; set; }

        /// <summary>
        ///ยอดภาษี
        /// <summary>
        public Nullable<decimal> FCXthVat { get; set; }

        /// <summary>
        ///ยอดแยกภาษี
        /// <summary>
        public Nullable<decimal> FCXthVatable { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTXthRmk { get; set; }

        /// <summary>
        ///สถานะ เอกสาร  1:สมบูรณ์, 2:ไม่สมบูรณ์, 3:ยกเลิก
        /// <summary>
        public string FTXthStaDoc { get; set; }

        /// <summary>
        ///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        /// <summary>
        public string FTXthStaApv { get; set; }

        /// <summary>
        ///สถานะ ประมวลผลสต๊อก ว่าง หรือ Null:ยังไม่ทำ, 1:ทำแล้ว
        /// <summary>
        public string FTXthStaPrcStk { get; set; }

        /// <summary>
        ///สถานะ ลบ MQ   Null:ยังไม่ทำ, 1:ทำแล้ว
        /// <summary>
        public string FTXthStaDelMQ { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        /// <summary>
        public Nullable<int> FNXthStaDocAct { get; set; }

        /// <summary>
        ///สถานะ อ้างอิง 0:ไม่เคยอ้างอิง, 1:อ้างอิงบางส่วน, 2:อ้างอิงหมดแล้ว
        /// <summary>
        public Nullable<int> FNXthStaRef { get; set; }

        /// <summary>
        ///รหัสเหตุผล
        /// <summary>
        public string FTRsnCode { get; set; }

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
