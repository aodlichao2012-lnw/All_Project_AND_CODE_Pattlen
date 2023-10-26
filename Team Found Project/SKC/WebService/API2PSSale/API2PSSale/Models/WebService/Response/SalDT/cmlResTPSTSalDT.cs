using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.WebService.Response.SalDT
{
    /// <summary>
    /// Class model response TPSTSalDT
    /// </summary>
    public class cmlResTPSTSalDT
    {
        /// <summary>
        /// สาขาสร้าง
        /// </summary>
        [JsonProperty(PropertyName = "rtBchCode")]
        public string  FTBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        [JsonProperty(PropertyName = "rtXshDocNo")]
        public string  FTXshDocNo { get; set; }

        /// <summary>
        /// ลำดับ
        /// </summary>
        [JsonProperty(PropertyName = "rnXsdSeqNo")]
        public int FNXsdSeqNo { get; set; }

        /// <summary>
        /// รหัสสินค้า
        /// </summary>
        [JsonProperty(PropertyName = "rtPdtCode")]
        public string FTPdtCode { get; set; }

        /// <summary>
        /// ชื่อสินค้า
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdPdtName")]
        public string FTXsdPdtName { get; set; }

        /// <summary>
        /// รหัสหน่วย
        /// </summary>
        [JsonProperty(PropertyName = "rtPunCode")]
        public string FTPunCode { get; set; }

        /// <summary>
        /// ชื่อหน่วยสินค้า
        /// </summary>
        [JsonProperty(PropertyName = "rtPunName")]
        public string FTPunName { get; set; }

        /// <summary>
        /// อัตราส่วนต่อหน่วย
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdFactor")]
        public double FCXsdFactor { get; set; }

        /// <summary>
        /// บาร์โค้ด
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdBarCode")]
        public string FTXsdBarCode { get; set; }

        /// <summary>
        /// รหัส ซีเรียล
        /// </summary>
        [JsonProperty(PropertyName = "rtSrnCode")]
        public string FTSrnCode { get; set; }

        /// <summary>
        /// ประเภทภาษี 1:มีภาษี, 2:ไม่มีภาษี
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdVatType")]
        public string FTXsdVatType { get; set; }

        /// <summary>
        /// รหัสภาษี ณ. ซื้อ
        /// </summary>
        [JsonProperty(PropertyName = "rtVatCode")]
        public string FTVatCode { get; set; }

        /// <summary>
        /// อัตราภาษี ณ. ซื้อ
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdVatRate")]
        public double FCXsdVatRate { get; set; }

        /// <summary>
        /// ใช้ราคาขาย 1:บังคับ, 2:แก้ไข, 3:เครื่องชั่ง,4: นน.
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdSaleType")]
        public string FTXsdSaleType { get; set; }

        /// <summary>
        /// จากราคาซื้อ ตาม หน่วย * อัตราแลกเปลี่ยน(HD.FCXphRteFac)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdSalePrice")]
        public double FCXsdSalePrice { get; set; }

        /// <summary>
        /// จำนวนชื้น ตาม หน่วย
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdQty")]
        public double FCXsdQty { get; set; }

        /// <summary>
        /// จำนวนรวมหน่วยเล็กสุด (FCXpdQty*FCXpdFactor)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdQtyAll")]
        public double FCXsdQtyAll { get; set; }

        /// <summary>
        /// ราคาซื้อ ตาม หน่วย * อัตราแลกเปลี่ยน(HD.FCXphRteFac)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdSetPrice")]
        public double FCXsdSetPrice { get; set; }

        /// <summary>
        /// มูลค่ารวมก่อนลด
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdAmtB4DisChg")]
        public double FCXsdAmtB4DisChg { get; set; }

        /// <summary>
        /// ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdDisChgTxt")]
        public string FTXsdDisChgTxt { get; set; }

        /// <summary>
        /// มูลค่ารวมส่วนลด
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdDis")]
        public double FCXsdDis { get; set; }

        /// <summary>
        /// มูลค่ารวมส่วนชาร์จ
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdChg")]
        public double FCXsdChg { get; set; }

        /// <summary>
        /// มูลค่าสุทธิก่อนท้ายบิล (FCXpdAmt-FCXpdDis+FCXpdChg)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdNet")]
        public double FCXsdNet { get; set; }

        /// <summary>
        /// มูลค่าสุทธิหลังท้ายบิล (Net-SUM(Disท้ายบิล))
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdNetAfHD")]
        public double FCXsdNetAfHD { get; set; }

        /// <summary>
        /// มูลค่าภาษี IN: NetAfHD-((NetAfHD*100)/(100+VatRate)) ,EX: ((NetAfHD*(100+VatRate))/100)-NetAfHD
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdVat")]
        public double FCXsdVat { get; set; }

        /// <summary>
        /// มูลค่าแยกภาษี (NetAfHD-FCXpdVat)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdVatable")]
        public double FCXsdVatable { get; set; }

        /// <summary>
        /// มูลค่าภาษี ณ. ที่จ่าย (FCXpdVatable* FCXpdWhtRate%)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdWhtAmt")]
        public double FCXsdWhtAmt { get; set; }

        /// <summary>
        /// รหัส ภาษี ณ. ที่จ่าย
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdWhtCode")]
        public string FTXsdWhtCode { get; set; }

        /// <summary>
        /// อัตราภาษี ณ. ที่จ่าย
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdWhtRate")]
        public double FCXsdWhtRate { get; set; }

        /// <summary>
        /// ต้นทุนรวมใน (FCXpdVat+FCXpdVatable)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdCostIn")]
        public double FCXsdCostIn { get; set; }

        /// <summary>
        /// ต้นทุนแยกนอก (FCXpdVatable)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdCostEx")]
        public double FCXsdCostEx { get; set; }

        /// <summary>
        /// สถานะ สินค้า 1:ขาย, 2:คืน, 3:แถม, 4: ยกเลิก (Void)
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdStaPdt")]
        public string FTXsdStaPdt { get; set; }

        /// <summary>
        /// จำนวนคงเหลือ ตามหน่วย (Default:FCXpdQty)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdQtyLef")]
        public double FCXsdQtyLef { get; set; }

        /// <summary>
        /// จำนวนคืนตามหน่วย (Default:0)
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdQtyRfn")]
        public double FCXsdQtyRfn { get; set; }

        /// <summary>
        /// สถานะตัดสต๊อก ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdStaPrcStk")]
        public string FTXsdStaPrcStk { get; set; }

        /// <summary>
        /// สถานะอนุญาต ลด/ชาร์จ  1:อนุญาต , 2:ไม่อนุญาต
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdStaAlwDis")]
        public string FTXsdStaAlwDis { get; set; }

        /// <summary>
        /// ระดับสิน(ค้าชุด)
        /// </summary>
        [JsonProperty(PropertyName = "rnXsdPdtLevel")]
        public int FNXsdPdtLevel { get; set; }

        /// <summary>
        /// รหัสสินค้าชุด
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdPdtParent")]
        public string FTXsdPdtParent { get; set; }

        /// <summary>
        /// จำนวนต่อชุด
        /// </summary>
        [JsonProperty(PropertyName = "rcXsdQtySet")]
        public double FCXsdQtySet { get; set; }

        /// <summary>
        /// สถานะ สินค้าชุด 1:ทั่วไป, 2:สินค้าประกอบ, 3:สินค้าชุด
        /// </summary>
        [JsonProperty(PropertyName = "rtPdtStaSet")]
        public string FTPdtStaSet { get; set; }

        /// <summary>
        /// หมายเหตุรายการ
        /// </summary>
        [JsonProperty(PropertyName = "rtXsdRmk")]
        public string FTXsdRmk { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        [JsonProperty(PropertyName = "rdLastUpdOn")]
        public DateTime? FDLastUpdOn { get; set; }

        /// <summary>
        /// ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        [JsonProperty(PropertyName = "rtLastUpdBy")]
        public string FTLastUpdBy { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        [JsonProperty(PropertyName = "rdCreateOn")]
        public DateTime? FDCreateOn { get; set; }

        /// <summary>
        /// ผู้สร้างรายการ
        /// </summary>
        [JsonProperty(PropertyName = "rtCreateBy")]
        public string FTCreateBy { get; set; }
    }
}