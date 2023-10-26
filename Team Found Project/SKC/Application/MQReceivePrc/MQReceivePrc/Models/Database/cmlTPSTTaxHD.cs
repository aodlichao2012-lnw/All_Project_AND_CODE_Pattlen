using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTPSTTaxHD
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร  Def : XYYPOS-1234567 Gen ตาม TCNTAuto
        /// <summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///ร้านค้า
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///ประเภทเอกสาร ดูจาก ตาราง TSysDocType
        /// <summary>
        public Nullable<int> FNXshDocType { get; set; }

        /// <summary>
        ///วันที่/เวลา เอกสาร dd/mm/yyyy H:mm:ss
        /// <summary>
        public Nullable<DateTime> FDXshDocDate { get; set; }

        /// <summary>
        ///สด/เครดิต 1:สด 2:credit
        /// <summary>
        public string FTXshCshOrCrd { get; set; }

        /// <summary>
        ///ภาษีมูลค่าเพิ่ม 1:รวมใน, 2:แยกนอก
        /// <summary>
        public string FTXshVATInOrEx { get; set; }

        /// <summary>
        ///แผนก
        /// <summary>
        public string FTDptCode { get; set; }

        /// <summary>
        ///คลัง
        /// <summary>
        public string FTWahCode { get; set; }

        /// <summary>
        ///เครื่องจุดขาย
        /// <summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///รอบ
        /// <summary>
        public string FTShfCode { get; set; }

        /// <summary>
        ///ลำดับการ SignIn DT
        /// <summary>
        public Nullable<int> FNSdtSeqNo { get; set; }

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
        public string FTXshApvCode { get; set; }

        /// <summary>
        ///ลูกค้า
        /// <summary>
        public string FTCstCode { get; set; }

        /// <summary>
        ///เลขที่ใบกำกับ
        /// <summary>
        public string FTXshDocVatFull { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่เอกสาร ภายนอก
        /// <summary>
        public string FTXshRefExt { get; set; }

        /// <summary>
        ///อ้างอิง วันที่เอกสาร ภายนอก
        /// <summary>
        public Nullable<DateTime> FDXshRefExtDate { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่เอกสาร ภายใน
        /// <summary>
        public string FTXshRefInt { get; set; }

        /// <summary>
        ///อ้างอิง วันที่เอกสาร ภายใน
        /// <summary>
        public Nullable<DateTime> FDXshRefIntDate { get; set; }

        /// <summary>
        ///อ้างถึงเอกสาร มัดจำ
        /// <summary>
        public string FTXshRefAE { get; set; }

        /// <summary>
        ///จำนวนครั้งที่พิมพ์
        /// <summary>
        public Nullable<int> FNXshDocPrint { get; set; }

        /// <summary>
        ///รหัสสกุลเงิน
        /// <summary>
        public string FTRteCode { get; set; }

        /// <summary>
        ///อัตราแลกเปลี่ยน
        /// <summary>
        public Nullable<decimal> FCXshRteFac { get; set; }

        /// <summary>
        ///ยอดรวม
        /// <summary>
        public Nullable<decimal> FCXshTotal { get; set; }

        /// <summary>
        ///ยอดรวมสินค้าไม่มีภาษี
        /// <summary>
        public Nullable<decimal> FCXshTotalNV { get; set; }

        /// <summary>
        ///ยอดรวมสินค้าห้ามลด
        /// <summary>
        public Nullable<decimal> FCXshTotalNoDis { get; set; }

        /// <summary>
        ///ยอมรวมสินค้าลดได้ และมีภาษี
        /// <summary>
        public Nullable<decimal> FCXshTotalB4DisChgV { get; set; }

        /// <summary>
        ///ยอมรวมสินค้าลดได้ และไม่มีภาษี
        /// <summary>
        public Nullable<decimal> FCXshTotalB4DisChgNV { get; set; }

        /// <summary>
        ///ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// <summary>
        public string FTXshDisChgTxt { get; set; }

        /// <summary>
        ///มูลค่ารวมส่วนลด SUM(HDis.FCXddDisVat+HDis.FCXddDisNoVat)
        /// <summary>
        public Nullable<decimal> FCXshDis { get; set; }

        /// <summary>
        ///มูลค่ารวมส่วนชาร์จ SUM(HDis.FCXddChgVat+HDis.FCXddChgNoVat)
        /// <summary>
        public Nullable<decimal> FCXshChg { get; set; }

        /// <summary>
        ///ยอดรวมหลังลด และมีภาษี
        /// <summary>
        public Nullable<decimal> FCXshTotalAfDisChgV { get; set; }

        /// <summary>
        ///ยอดรวมหลังลด และไม่มีภาษี
        /// <summary>
        public Nullable<decimal> FCXshTotalAfDisChgNV { get; set; }

        /// <summary>
        ///ยอดมัดจำ
        /// <summary>
        public Nullable<decimal> FCXshRefAEAmt { get; set; }

        /// <summary>
        ///ยอดรวมเฉพาะภาษี (FCXshTotal-FCXshTotalNV-(FCXshTotalB4DisChgV-FCXshTotalAfDisChgV))
        /// <summary>
        public Nullable<decimal> FCXshAmtV { get; set; }

        /// <summary>
        ///ยอดรวมเฉพาะไม่มีภาษี (FCXshTotalNV-(FCXshTotalB4DisChgNV-FCXshTotalAfDisChgNV))
        /// <summary>
        public Nullable<decimal> FCXshAmtNV { get; set; }

        /// <summary>
        ///ยอดภาษี
        /// <summary>
        public Nullable<decimal> FCXshVat { get; set; }

        /// <summary>
        ///ยอดแยกภาษี (FCXshAmtV-FCXshVat)+FCXshAmt์NV
        /// <summary>
        public Nullable<decimal> FCXshVatable { get; set; }

        /// <summary>
        ///รหัสอัตราภาษี ณ ที่จ่าย
        /// <summary>
        public string FTXshWpCode { get; set; }

        /// <summary>
        ///ภาษีหัก ณ ที่จ่าย SUM(FCXpdWhtAmt)  /Key In
        /// <summary>
        public Nullable<decimal> FCXshWpTax { get; set; }

        /// <summary>
        ///ยอดรวม FCXshAmtV+FCXshAmtNV+FCXshRnd
        /// <summary>
        public Nullable<decimal> FCXshGrand { get; set; }

        /// <summary>
        ///ยอดปัดเศษ (เมื่อชำระด้วยเงินสดเฉพาะขายปลีก)
        /// <summary>
        public Nullable<decimal> FCXshRnd { get; set; }

        /// <summary>
        ///ข้อความ ยอดรวมสุทธิ(FCXphGrand)
        /// <summary>
        public string FTXshGndText { get; set; }

        /// <summary>
        ///ยอดจ่าย
        /// <summary>
        public Nullable<decimal> FCXshPaid { get; set; }

        /// <summary>
        ///ยอดค้าง Default: 0
        /// <summary>
        public Nullable<decimal> FCXshLeft { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTXshRmk { get; set; }

        /// <summary>
        ///สถานะ การคืน 1:ไม่เคยคืน, 2:เคยคืน
        /// <summary>
        public string FTXshStaRefund { get; set; }

        /// <summary>
        ///สถานะ เอกสาร  1:สมบูรณ์, 2:ไม่สมบูรณ์, 3:ยกเลิก
        /// <summary>
        public string FTXshStaDoc { get; set; }

        /// <summary>
        ///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        /// <summary>
        public string FTXshStaApv { get; set; }

        /// <summary>
        ///สถานะ ประมวลผลสต๊อก ว่าง หรือ Null:ยังไม่ทำ, 1:ทำแล้ว
        /// <summary>
        public string FTXshStaPrcStk { get; set; }

        /// <summary>
        ///สถานะ รับ/จ่ายเงิน 1:ยังไม่จ่าย 2:บางส่วน, 3:ครบ
        /// <summary>
        public string FTXshStaPaid { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        /// <summary>
        public Nullable<int> FNXshStaDocAct { get; set; }

        /// <summary>
        ///สถานะ อ้างอิง 0:ไม่เคยอ้างอิง, 1:อ้างอิงบางส่วน, 2:อ้างอิงหมดแล้ว
        /// <summary>
        public Nullable<int> FNXshStaRef { get; set; }

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
