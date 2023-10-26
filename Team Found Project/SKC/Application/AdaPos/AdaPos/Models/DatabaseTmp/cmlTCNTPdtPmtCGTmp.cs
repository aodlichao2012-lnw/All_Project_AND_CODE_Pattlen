using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtCGTmp
    {
        //*Arm 63-03-26
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string FTPmhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> FNPgtSeq { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string FTPmdGrpName { get; set; }

        /// <summary>
        ///1:ตามคำนวน 2:ตามช่วง 3:ตามกลุ่ม 4:ทุกกลุ่ม
        /// </summary>
        public string FTPgtStaGetEffect { get; set; }

        /// <summary>
        ///1:ลดบาท 2:ลด% 3:ปรับราคา 4:.ใช้กลุ่มราคา 5:แถม(Free) 6:ไม่กำหนด
        /// </summary>
        public string FTPgtStaGetType { get; set; }

        /// <summary>
        ///เงื่อนไขการเลือกสินค้ากรณี>1รายการในบิล 1:ราคามากกว่า 2:ราคาน้อยกว่า 3:user เลือก
        /// </summary>
        public string FTPgtStaGetPdt { get; set; }

        /// <summary>
        ///กลุ่มที่มีสิทธิอนุมัติ  ว่าง: ได้ Auto  ไม่ว่าง: popup user login
        /// </summary>
        public string FTRolCode { get; set; }

        /// <summary>
        ///มูลค่า แปรผันตามรูปแบบส่วนลด
        /// </summary>
        public Nullable<decimal> FCPgtGetvalue { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string FTPplCode { get; set; }

        /// <summary>
        ///จำนวนที่จะได้รับ
        /// </summary>
        public Nullable<decimal> FCPgtGetQty { get; set; }

        /// <summary>
        ///%เฉลี่ย รวมกันต้องเท่ากับ 100
        /// </summary>
        public Nullable<decimal> FCPgtPerAvgDis { get; set; }

        /// <summary>
        ///การให้แต้ม 1:ไม่กำหนด 2:.ให้แต้ม
        /// </summary>
        public string FTPgtStaPoint { get; set; }

        /// <summary>
        ///การคำนวณตาม 1:มูลค่า 2:จำนวน
        /// </summary>
        public string FTPgtStaPntCalType { get; set; }

        /// <summary>
        ///ประเภทสินค้าร่วมรายการ 1:Product 2:Brand
        /// </summary>
        public string FTPgtStaPdtDT { get; set; }

        /// <summary>
        ///อัตราส่วนแต้มที่จะได้รับ  (ต้นทางจากBuy)
        /// </summary>
        public Nullable<Int64> FNPgtPntGet { get; set; }

        /// <summary>
        ///อัตราส่วนแต้มที่จะได้รับ
        /// </summary>
        public Nullable<Int64> FNPgtPntBuy { get; set; }

        /// <summary>
        ///การให้สิทธิ์ 1:ไม่กำหนด 2:.ให้สิทธิ์คูปอง 3:ข้อความ
        /// </summary>
        public string FTPgtStaCoupon { get; set; }

        /// <summary>
        ///ข้อความ ท้ายบิล
        /// </summary>
        public string FTPgtCpnText { get; set; }

        /// <summary>
        ///รหัสเอกสารคูปอง (Bar in DT)
        /// </summary>
        public string FTCphDocNo { get; set; }
        //+++++++++++++


        ///// <summary>
        /////รหัสสาขา
        ///// <summary>
        //public string FTBchCode { get; set; }

        ///// <summary>
        /////รหัสโปรโมชั่น XXYY-######
        ///// <summary>
        //public string FTPmhDocNo { get; set; }

        ///// <summary>
        /////ลำดับ
        ///// <summary>
        //public Nullable<Int64> FNPgtSeq { get; set; }

        ///// <summary>
        /////ชื่อกลุ่มจัดรายการ
        ///// <summary>
        //public string FTPmdGrpName { get; set; }

        ///// <summary>
        /////1:ตามคำนวน 2:ตามช่วง 3:ตามกลุ่ม 4:ทุกกลุ่ม
        ///// <summary>
        //public string FTPgtStaGetEffect { get; set; }

        ///// <summary>
        /////1:ลดบาท 2:ลด% 3:ปรับราคา 4:.ใช้กลุ่มราคา 5:แถม(Free) 6:ไม่กำหนด
        ///// <summary>
        //public string FTPgtStaGetType { get; set; }

        ///// <summary>
        /////เงื่อนไขการเลือกสินค้ากรณี>1รายการในบิล 1:ราคามากกว่า 2:ราคาน้อยกว่า 3:user เลือก
        ///// <summary>
        //public string FTPgtStaGetPdt { get; set; }

        ///// <summary>
        /////กลุ่มที่มีสิทธิอนุมัติ  ว่าง: ได้ Auto  ไม่ว่าง: popup user login
        ///// <summary>
        //public string FTRolCode { get; set; }

        ///// <summary>
        /////มูลค่า แปรผันตามรูปแบบส่วนลด
        ///// <summary>
        //public Nullable<double> FCPgtGetvalue { get; set; }

        ///// <summary>
        /////รหัสกลุ่มราคา
        ///// <summary>
        //public string FTPplCode { get; set; }

        ///// <summary>
        /////จำนวนที่จะได้รับ
        ///// <summary>
        //public Nullable<double> FCPgtGetQty { get; set; }

        ///// <summary>
        /////%เฉลี่ย รวมกันต้องเท่ากับ 100
        ///// <summary>
        //public Nullable<double> FCPgtPerAvgDis { get; set; }

        ///// <summary>
        /////การให้แต้ม 1:ไม่กำหนด 2:.ให้แต้ม
        ///// <summary>
        //public string FTPgtStaPoint { get; set; }

        ///// <summary>
        /////การคำนวณตาม 1:มูลค่า 2:จำนวน
        ///// <summary>
        //public string FTPgtStaPntCalType { get; set; }

        ///// <summary>
        /////ประเภทสินค้าร่วมรายการ 1:Product 2:Brand
        ///// <summary>
        //public string FTPgtStaPdtDT { get; set; }

        ///// <summary>
        /////อัตราส่วนแต้มที่จะได้รับ  (ต้นทางจากBuy)
        ///// <summary>
        //public Nullable<Int64> FNPgtPntGet { get; set; }

        ///// <summary>
        /////อัตราส่วนแต้มที่จะได้รับ
        ///// <summary>
        //public Nullable<Int64> FNPgtPntBuy { get; set; }

        ///// <summary>
        /////การให้สิทธิ์ 1:ไม่กำหนด 2:.ให้สิทธิ์คูปอง 3:ข้อความ
        ///// <summary>
        //public string FTPgtStaCoupon { get; set; }

        ///// <summary>
        /////ข้อความ ท้ายบิล
        ///// <summary>
        //public string FTPgtCpnText { get; set; }

        ///// <summary>
        /////รหัสเอกสารคูปอง (Bar in DT)
        ///// <summary>
        //public string FTCphDocNo { get; set; }
    }
}
