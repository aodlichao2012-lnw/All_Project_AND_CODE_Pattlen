using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.Database
{
    public class cmlTPSTSalPD
    {
        /// <summary>
        /// สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }
        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }
        /// <summary>
        /// รหัสโปรโมชั่น
        /// </summary>
        public string FTPmhDocNo { get; set; }
        /// <summary>
        /// ลำดับ
        /// </summary>
        public Nullable<int> FNXsdSeqNo { get; set; }
        /// <summary>
        /// ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string FTPmdGrpName { get; set; }
        /// <summary>
        /// รหัสสินค้า
        /// </summary>
        public string FTPdtCode { get; set; }
        /// <summary>
        /// รหัสหน่วย
        /// </summary>
        public string FTPunCode { get; set; }
        /// <summary>
        /// จำนวนชื้น ตาม หน่วย
        /// </summary>
        //public Nullable<int> FCXsdQty { get; set; }
        public Nullable<decimal> FCXsdQty { get; set; }         //*Arm 63-03-27  -แก้ DataType

        /// <summary>
        /// จำนวนรวมหน่วย
        /// </summary>
        //public Nullable<int> FCXsdQtyAll { get; set; }
        public Nullable<decimal> FCXsdQtyAll { get; set; }      //*Arm 63-03-27  -แก้ DataType

        /// <summary>
        /// ราคาซื้อ ตาม หน่วย * อัตราแลกเปลี่ยน
        /// </summary>
        public Nullable<decimal> FCXsdSetPrice { get; set; }
        /// <summary>
        /// มูลค่าสุทธิก่อนท้ายบิล 
        /// </summary>
        public Nullable<decimal> FCXsdNet { get; set; }
        /// <summary>
        /// จำนวนชุดที่ได้รับโปรโมชั่น
        /// </summary>
        //public Nullable<int> FCXpdGetQtyDiv { get; set; }
        public Nullable<decimal> FCXpdGetQtyDiv { get; set; }   //*Arm 63-03-27  -แก้ DataType


        /// <summary>
        /// 1:ลดบาท 2:ลด% 3:ปรับราคา 4:.ใช้กลุ่มราคา 5:แถม(Free) 6:ไม่กำหนด
        /// </summary>
        public string FTXpdGetType { get; set; }
        /// <summary>
        /// มูลค่า แปรผันตามรูปแบบส่วนลด
        /// </summary>
        public Nullable<decimal> FCXpdGetValue { get; set; }
        /// <summary>
        /// ส่วนลดที่ได้รับจากโปรโมชั่น
        /// </summary>
        public Nullable<decimal> FCXpdDis { get; set; }
        /// <summary>
        /// เปอร์เซ็นส่วนลดที่จะได้รับ
        /// </summary>
        public Nullable<decimal> FCXpdPerDisAvg { get; set; }
        /// <summary>
        /// ส่วนลดที่ได้รับ
        /// </summary>
        public Nullable<decimal> FCXpdDisAvg { get; set; }
        /// <summary>
        /// จำนวนแต้มที่ได้รับ
        /// </summary>
        //public Nullable<int> FCXpdPoint { get; set; }
        public Nullable<decimal> FCXpdPoint { get; set; }       //*Arm 63-03-27  -แก้ DataType

        /// <summary>
        /// สถานะรับของแถม 1:จุดขายคำนวนอัตโนมัติ 2:จุดขายเลือกได้ 3:จุดบริการ
        /// </summary>
        public string FTXpdStaRcv { get; set; }
        /// <summary>
        /// รหัสกลุ่มราคา
        /// </summary>
        public string FTPplCode { get; set; }
        /// <summary>
        /// ข้อความ ท้ายบิล
        /// </summary>
        public string FTXpdCpnText { get; set; }
        /// <summary>
        /// รหัสคูปอง
        /// </summary>
        public string FTCpdBarCpn { get; set; }
    }
}