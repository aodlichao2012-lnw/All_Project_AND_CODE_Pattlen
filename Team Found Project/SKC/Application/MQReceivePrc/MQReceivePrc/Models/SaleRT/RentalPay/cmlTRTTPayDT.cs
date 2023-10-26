using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleRT.RentalPay
{
    class cmlTRTTPayDT
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
        /// ลำดับรายการ
        /// </summary>
        public int FNXsdSeqNo { get; set; }

        /// <summary>
        /// เลขที่เอกสาร  SalHD
        /// </summary>
        public string FTXsdRefDocNo { get; set; }

        /// <summary>
        /// ประเภทเอกสาร ดูจาก ตาราง TSysDocType  เช่า/คืนเช่า
        /// </summary>
        public string FTXsdRefDocType { get; set; }

        /// <summary>
        /// วันที่เอกสารใบที่อ้าง
        /// </summary>
        public DateTime? FDXsdRefDocDate { get; set; }

        /// <summary>
        /// ยอดเอกสาร
        /// </summary>
        public double FCXsdRefGrand { get; set; }

        /// <summary>
        /// ยอดต้องจ่ายล่วงหน้า ทุกรายการ
        /// </summary>
        public double FCXsdRefPrePaid { get; set; }

        /// <summary>
        /// ยอดจ่าย  กรณีเช่า จะเท่ากับยอด Prepaid
        /// </summary>
        public double FCXsdRefPaid { get; set; }

        /// <summary>
        /// ยอดค้าง FCXshGrand-FCXshPaid
        /// </summary>
        public double FCXsdRefLeft { get; set; }

        /// <summary>
        /// ระดับความลึก (Outline)
        /// </summary>
        public int FNXsdLevel { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string FTXsdRmk { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public DateTime? FDLastUpdOn { get; set; }

        /// <summary>
        /// ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }
    }
}
