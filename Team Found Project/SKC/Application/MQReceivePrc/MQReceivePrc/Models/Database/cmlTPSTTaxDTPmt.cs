using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTPSTTaxDTPmt
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// <summary>
        public string FTPmhCode { get; set; }

        /// <summary>
        ///กลุ่มโปรโมชั่น
        /// <summary>
        public string FTXdpGrpName { get; set; }

        /// <summary>
        ///รหัสบาร์โค๊ด อ้างอิง DT
        /// <summary>
        public string FTXsdBarCode { get; set; }

        /// <summary>
        ///ลำดับอ้างอิง DT
        /// <summary>
        public Nullable<int> FNXsdSeqNo { get; set; }

        /// <summary>
        ///จำนวน/หน่วย
        /// <summary>
        public Nullable<decimal> FCXdpQtyAll { get; set; }

        /// <summary>
        ///มูลค่ารวม
        /// <summary>
        public Nullable<decimal> FCXdpNet { get; set; }

        /// <summary>
        ///ราคา/หน่วย
        /// <summary>
        public Nullable<decimal> FCXdpSetPrice { get; set; }

        /// <summary>
        ///จำนวนชุดเข้าเงื่อนไข
        /// <summary>
        public Nullable<decimal> FCXdpGetQtyDiv { get; set; }

        /// <summary>
        ///รูปแบบส่วนลด 1:ลดบาท 2:ลด % 3: ปรับราคา 4: ได้แต้ม
        /// <summary>
        public Nullable<decimal> FCXdpGetCond { get; set; }

        /// <summary>
        ///มูลค้า แปรผันตามรูปแบบส่วนลด
        /// <summary>
        public Nullable<decimal> FCXdpGetValue { get; set; }

        /// <summary>
        ///ส่วนลดโปรโมชั่น
        /// <summary>
        public Nullable<decimal> FCXdpDis { get; set; }

        /// <summary>
        ///ส่วนลดโปรโมชั่นเฉลี่ยตาม %
        /// <summary>
        public Nullable<decimal> FCXdpDisAvg { get; set; }

        /// <summary>
        ///แต้มที่ได้รับ
        /// <summary>
        public Nullable<decimal> FCXdpPoint { get; set; }

        /// <summary>
        ///คำนวนรวมสินค้าโปรโมชั่น (1:รวมสินค้าโปรโมชั่น,2:ไม่รวมสินค้า โปรโมชั่น)
        /// <summary>
        public string FTXdpStaExceptPmt { get; set; }

        /// <summary>
        ///สถานะรับสินค้า (รับที่เครื่องจุดขายเป็น 1 เสมอ Else 2)
        /// <summary>
        public string FTXdpStaRcv { get; set; }
    }
}
