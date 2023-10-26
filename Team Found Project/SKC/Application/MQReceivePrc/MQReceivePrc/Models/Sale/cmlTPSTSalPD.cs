using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Sale
{
    public class cmlTPSTSalPD
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string FTPmhDocNo { get; set; }

        /// <summary>
        ///ลำดับรายการจาก DT
        /// </summary>
        public Nullable<int> FNXsdSeqNo { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string FTPmdGrpName { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// </summary>
        public string FTPunCode { get; set; }

        /// <summary>
        ///จำนวนชื้น
        /// </summary>
        public Nullable<decimal> FCXsdQty { get; set; }

        /// <summary>
        ///จำนวนชื้นทั้งจาก DT
        /// </summary>
        public Nullable<decimal> FCXsdQtyAll { get; set; }

        /// <summary>
        ///ราคาสินค้า
        /// </summary>
        public Nullable<decimal> FCXsdSetPrice { get; set; }

        /// <summary>
        ///มูลค่ารวม
        /// </summary>
        public Nullable<decimal> FCXsdNet { get; set; }

        /// <summary>
        ///จำนวนชุดที่ได้รับโปรโมชั่น
        /// </summary>
        public Nullable<decimal> FCXpdGetQtyDiv { get; set; }

        /// <summary>
        ///1:ลดบาท 2:ลด% 3:ปรับราคา 4:.ใช้กลุ่มราคา 5:แถม(Free) 6:ไม่กำหนด
        /// </summary>
        public string FTXpdGetType { get; set; }

        /// <summary>
        ///มูลค่า แปรผันตามรูปแบบส่วนลด
        /// </summary>
        public Nullable<decimal> FCXpdGetValue { get; set; }

        /// <summary>
        ///ส่วนลดที่ได้รับจากโปรโมชั่น
        /// </summary>
        public Nullable<decimal> FCXpdDis { get; set; }

        /// <summary>
        ///เปอร์เซ็นส่วนลดที่จะได้รับ
        /// </summary>
        public Nullable<decimal> FCXpdPerDisAvg { get; set; }

        /// <summary>
        ///ส่วนลดที่ได้รับ
        /// </summary>
        public Nullable<decimal> FCXpdDisAvg { get; set; }

        /// <summary>
        ///จำนวนแต้มที่ได้รับ
        /// </summary>
        public Nullable<decimal> FCXpdPoint { get; set; }

        /// <summary>
        ///สถานะรับของแถม 1:จุดขายคำนวนอัตโนมัติ 2:จุดขายเลือกได้ 3:จุดบริการ
        /// </summary>
        public string FTXpdStaRcv { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string FTPplCode { get; set; }

        /// <summary>
        ///ข้อความ ท้ายบิล
        /// </summary>
        public string FTXpdCpnText { get; set; }

        /// <summary>
        ///รหัสคูปอง
        /// </summary>
        public string FTCpdBarCpn { get; set; }
    }
}
