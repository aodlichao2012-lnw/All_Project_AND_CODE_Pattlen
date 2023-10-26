using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.BankDepositSlip
{
    public class cmlTFNTBnkDplHD
    {
        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่ใบนำฝากDef : XYYPOS-1234567 Gen ตาม TCNTAuto
        /// </summary>
        public string FTBdhDocNo { get; set; }

        /// <summary>
        ///รหัสประเภทใบนำฝาก
        /// </summary>
        public string FTBdtCode { get; set; }

        /// <summary>
        ///วันที่นำฝาก
        /// </summary>
        public Nullable<DateTime> FDBdhDate { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///ร้านค้า
        /// </summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///ผู้บันทึก
        /// </summary>
        public string FTUsrCode { get; set; }

        /// <summary>
        ///ผู้นำฝาก
        /// </summary>
        public string FTBdhUsrSender { get; set; }

        /// <summary>
        ///รหัสผู้อนุมัติ
        /// </summary>
        public string FTBdhUsrApv { get; set; }

        /// <summary>
        ///รหัสสมุดบัญชี
        /// </summary>
        public string FTBbkCode { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่เอกสาร ภายนอก
        /// </summary>
        public string FTBdhRefExt { get; set; }

        /// <summary>
        ///อ้างอิง วันที่เอกสาร ภายนอก
        /// </summary>
        public Nullable<DateTime> FDBdhRefExtDate { get; set; }

        /// <summary>
        ///จำนวนเงินสด
        /// </summary>
        public Nullable<decimal> FCBdhTotCash { get; set; }

        /// <summary>
        ///จำนวนเงินเช็ค
        /// </summary>
        public Nullable<decimal> FCBdhTotCheque { get; set; }

        /// <summary>
        ///จำนวนเงินเช็ค-ชาร์จ
        /// </summary>
        public Nullable<decimal> FCBdhTotChqChg { get; set; }

        /// <summary>
        ///จำนวนเงินเช็ค-ภาษี
        /// </summary>
        public Nullable<decimal> FCBdhTotChqVat { get; set; }

        /// <summary>
        ///จำนวนเงินรวม
        /// </summary>
        public Nullable<decimal> FCBdhTotal { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string FTBdhRmk { get; set; }

        /// <summary>
        ///สถานะ เอกสาร  1:สมบูรณ์, 2:ไม่สมบูรณ์, 3:ยกเลิก
        /// </summary>
        public string FTBdhStaDoc { get; set; }

        /// <summary>
        ///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        /// </summary>
        public string FTBdhStaApv { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        /// </summary>
        public Nullable<int> FNBdhStaDocAct { get; set; }

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
