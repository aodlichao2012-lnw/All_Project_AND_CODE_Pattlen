using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductPromotion
{
    public class cmlResInfoPdtPmtCB
    {

        //*Arm 63-03-26 ปรับแก้ Standard 

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string rtPmhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> rnPbySeq { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string rtPmdGrpName { get; set; }

        /// <summary>
        ///1:เฉพาะกลุ่ม 2:ทุกกลุ่ม 3:ทั้งร้าน 4:ไม่กำหนด
        /// </summary>
        public string rtPbyStaCalSum { get; set; }

        /// <summary>
        ///1:ครบจำนวน 2:ครบมูลค่า 3:ตามช่วงจำนวน 4:ตามช่วงมูลค่า 5:ตามช่วงเวลา
        /// </summary>
        public string rtPbyStaBuyCond { get; set; }

        /// <summary>
        ///ประเภทสินค้าร่วมรายการ 1:Product 2:Brand
        /// </summary>
        public string rtPbyStaPdtDT { get; set; }

        /// <summary>
        ///%เฉลี่ย รวมกันต้องเท่ากับ 100
        /// </summary>
        public Nullable<decimal> rcPbyPerAvgDis { get; set; }

        /// <summary>
        ///กำหนด ราคาขั้นต่ำ ต่อหน่วย  0: default ไมมีขั้นต่ำ
        /// </summary>
        public Nullable<decimal> rcPbyMinSetPri { get; set; }

        /// <summary>
        ///จาก Qty/Amt ค่า
        /// </summary>
        public Nullable<decimal> rcPbyMinValue { get; set; }

        /// <summary>
        ///ถึง Qty/Amt ค่า
        /// </summary>
        public Nullable<decimal> rcPbyMaxValue { get; set; }

        /// <summary>
        ///จากเวลา ขั้นต้ำที่มีผล
        /// </summary>
        public string rtPbyMinTime { get; set; }

        /// <summary>
        ///ถึงเวลา  ไม่เกิน 24:00:00 และไม่น้อยกว่า จากเวลา
        /// </summary>
        public string rtPbyMaxTime { get; set; }

        //+++++++++++++


        ///// <summary>
        /////รหัสสาขา
        ///// </summary>
        //public string rtFTBchCode { get; set; }

        ///// <summary>
        /////รหัสโปรโมชั่น XXYY-######
        ///// </summary>
        //public string rtFTPmhDocNo { get; set; }

        ///// <summary>
        /////ลำดับ
        ///// </summary>
        //public Nullable<Int64> rnFNPbySeq { get; set; }

        ///// <summary>
        /////ชื่อกลุ่มจัดรายการ
        ///// </summary>
        //public string rtFTPmdGrpName { get; set; }

        ///// <summary>
        /////1:เฉพาะกลุ่ม 2:ทุกกลุ่ม 3:ทั้งร้าน 4:ไม่กำหนด
        ///// </summary>
        //public string rtFTPbyStaCalSum { get; set; }

        ///// <summary>
        /////1:ครบจำนวน 2:ครบมูลค่า 3:ตามช่วงจำนวน 4:ตามช่วงมูลค่า 5:ตามช่วงเวลา
        ///// </summary>
        //public string rtFTPbyStaBuyCond { get; set; }

        ///// <summary>
        /////ประเภทสินค้าร่วมรายการ 1:Product 2:Brand
        ///// </summary>
        //public string rtFTPbyStaPdtDT { get; set; }

        ///// <summary>
        /////%เฉลี่ย รวมกันต้องเท่ากับ 100
        ///// </summary>
        //public Nullable<decimal> rcFCPbyPerAvgDis { get; set; }

        ///// <summary>
        /////กำหนด ราคาขั้นต่ำ ต่อหน่วย  0: default ไมมีขั้นต่ำ
        ///// </summary>
        //public Nullable<decimal> rcFCPbyMinSetPri { get; set; }

        ///// <summary>
        /////จาก Qty/Amt ค่า
        ///// </summary>
        //public Nullable<decimal> rcFCPbyMinValue { get; set; }

        ///// <summary>
        /////ถึง Qty/Amt ค่า
        ///// </summary>
        //public Nullable<decimal> rcFCPbyMaxValue { get; set; }

        ///// <summary>
        /////จากเวลา ขั้นต้ำที่มีผล
        ///// </summary>
        //public string rtFTPbyMinTime { get; set; }

        ///// <summary>
        /////ถึงเวลา  ไม่เกิน 24:00:00 และไม่น้อยกว่า จากเวลา
        ///// </summary>
        //public string rtFTPbyMaxTime { get; set; }
    }
}
