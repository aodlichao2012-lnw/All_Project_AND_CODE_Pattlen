using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleRT.RentalPay
{
    class cmlTRTTPayHD
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสาร XXYY-######
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        /// วันที่เอกสาร
        /// </summary>
        public DateTime? FDXshDocDate { get; set; }

        /// <summary>
        /// เวลาที่เกิดเอกสาร
        /// </summary>
        public string FTXshDocTime { get; set; }

        /// <summary>
        /// รหัสผู้บันทึก
        /// </summary>
        public string FTUsrCode { get; set; }

        /// <summary>
        /// รหัสลูกค้า
        /// </summary>
        public string FTCstCode { get; set; }

        /// <summary>
        /// หมายเลขบัตรสมาชิก /Wristband
        /// </summary>
        public string FTXshCardNo { get; set; }

        /// <summary>
        /// จำนวนครั้งในการพิมพ์เอกสาร
        /// </summary>
        public int FNXshDocPrint { get; set; }

        /// <summary>
        /// ยอดรวม ก่อนชำระ
        /// </summary>
        public double FCXshTotal { get; set; }

        /// <summary>
        /// ยอด หักภาษี ณ ที่จ่าย
        /// </summary>
        public double FCXshWht { get; set; }

        /// <summary>
        /// ยอดรวมหลัง หักภาษี ณ ที่จ่าย
        /// </summary>
        public double FCXshAfWht { get; set; }

        /// <summary>
        /// ยอด ค่าปรับ/ค่าธรรมเนียม
        /// </summary>
        public double FCXshFee { get; set; }

        /// <summary>
        /// ยอด ส่วนลด
        /// </summary>
        public double FCXshDisc { get; set; }

        /// <summary>
        /// ยอดรวมหลัง หัก ส่วนลด
        /// </summary>
        public double FCXshAfDisc { get; set; }

        /// <summary>
        /// ยอดรวม ทั้งสิ้น (ชำระ - หักภาษี + ค่าปรับ - ส่วนลด)
        /// </summary>
        public double FCXshAmt { get; set; }

        /// <summary>
        /// ยอดรวม ชำระ
        /// </summary>
        public double FCXshPay { get; set; }

        /// <summary>
        /// ยอดรวม จ่ายชำระจริง (ยอดรวม ทั้งสิ้น+ชาร์จบัตร)
        /// </summary>
        public double FCXshGrand { get; set; }

        /// <summary>
        /// สถานะ รับ/จ่ายเงิน 1:ยังไม่จ่าย 2:บางส่วน, 3:ครบ
        /// </summary>
        public string FTXshStaPaid { get; set; }

        /// <summary>
        /// สถานะ เอกสาร  1:สมบูรณ์, 2:ไม่สมบูรณ์, 3:ยกเลิก
        /// </summary>
        public string FTXshStaDoc { get; set; }

        /// <summary>
        /// สถานะ prc เอกสาร  ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// </summary>
        public string FTXshStaPrcDoc { get; set; }

        /// <summary>
        /// สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        /// </summary>
        public int FNXshStaDocAct { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string FTXshRmk { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public DateTime? FDLastUpdOn { get; set; }

        /// <summary>
        /// ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        public DateTime? FDCreateOn { get; set; }

        /// <summary>
        /// ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }
    }
}
