using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
    public class cmlPdtDisChg
    {
        /// <summary>
        /// PdtCode
        /// </summary>
        public string ptPdtCode { get; set; }

        /// <summary>
        /// Barcode
        /// </summary>
        public string ptBarcode { get; set; }

        /// <summary>
        /// ราคา
        /// </summary>
        public decimal pcSetPrice { get; set; }  //*Arm 63-03-23

        /// <summary>
        /// ลำดับการ Dis/Chg
        /// </summary>
        public int pnSeqNo { get; set; }

        /// <summary>
        /// สถานะส่วนลด 1: ลดรายการ ,2: ลดท้ายบิล
        /// </summary>
        public int pnStaDis { get; set; }

        /// <summary>
        /// ประเภทลดชาร์จ 1:ลดบาท 2: ลด % 3: ชาร์จบาท 4: ชาร์จ %
        /// </summary>
        public string ptDisChgType { get; set; }

       
        public string ptDisChgTxt { get; set; }

        /// <summary>
        /// มูลค่าสุทธิก่อนลดชาร์จ
        /// </summary>
        public decimal pcNet { get; set; }

        /// <summary>
        /// ยอดลด/ชาร์จ
        /// </summary>
        public decimal pcValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ptRefCode { get; set; }
        
    }

    public class cmlPdtRedeem
    {
        /// <summary>
        /// สาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// เอกสารขาย
        /// </summary>
        public string ptDocNo { get; set; }

        /// <summary>
        /// PdtCode
        /// </summary>
        public string ptPdtCode { get; set; }

        /// <summary>
        /// Barcode
        /// </summary>
        public string ptBarcode { get; set; }

        /// <summary>
        /// ราคา
        /// </summary>
        public decimal pcSetPrice { get; set; }  //*Arm 63-03-23

        /// <summary>
        /// ลำดับการ Dis/Chg
        /// </summary>
        //public string pnSeqNo { get; set; }

        /// <summary>
        /// สถานะส่วนลด 1: ลดรายการ ,2: ลดท้ายบิล
        /// </summary>
        public string pnStaDis { get; set; }

        /// <summary>
        /// ประเภทลดชาร์จ 1:ลดบาท 2: ลด % 3: ชาร์จบาท 4: ชาร์จ %
        /// </summary>
        public string ptDisChgType { get; set; }

        /// <summary>
        /// มูลค่าสุทธิก่อนลดชาร์จ
        /// </summary>
        //public decimal pcNet { get; set; }

        /// <summary>
        /// ยอดลด/ชาร์จ
        /// </summary>
        public decimal pcValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ptRefCode { get; set; }

        /// <summary>
        /// จำนวนสินค้า Redeem
        /// </summary>
        public decimal pcPdtQty { get; set; }

        /// <summary>
        /// จำนวนแต้มที่ใช้
        /// </summary>
        public int pnUsePnt { get; set; }

        /// <summary>
        /// จำนวนเงินที่ใช้
        /// </summary>
        public decimal pcUseMny { get; set; }

        /// <summary>
        /// ประเภทเอกสาร 1: Redeem สินค้า แต้ม+เงิน 2: Redeem ส่วนลด
        /// </summary>
        public string ptDocType { get; set; }

        /// <summary>
        ///  1: Discount 2: ชำระเงินสด
        /// </summary>
        public string ptCalType { get; set; }

    }
}
