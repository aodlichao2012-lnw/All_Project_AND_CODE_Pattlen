using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNTPdtPmtCBTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// <summary>
        public string FTPmhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<Int64> FNPbySeq { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// <summary>
        public string FTPmdGrpName { get; set; }

        /// <summary>
        ///1:เฉพาะกลุ่ม 2:ทุกกลุ่ม 3:ทั้งร้าน 4:ไม่กำหนด
        /// <summary>
        public string FTPbyStaCalSum { get; set; }

        /// <summary>
        ///1:ครบจำนวน 2:ครบมูลค่า 3:ตามช่วงจำนวน 4:ตามช่วงมูลค่า 5:ตามช่วงเวลา
        /// <summary>
        public string FTPbyStaBuyCond { get; set; }

        /// <summary>
        ///ประเภทสินค้าร่วมรายการ 1:Product 2:Brand
        /// <summary>
        public string FTPbyStaPdtDT { get; set; }

        /// <summary>
        ///%เฉลี่ย รวมกันต้องเท่ากับ 100
        /// <summary>
        public Nullable<decimal> FCPbyPerAvgDis { get; set; }

        /// <summary>
        ///กำหนด ราคาขั้นต่ำ ต่อหน่วย  0: default ไมมีขั้นต่ำ
        /// <summary>
        public Nullable<decimal> FCPbyMinSetPri { get; set; }

        /// <summary>
        ///จาก Qty/Amt ค่า
        /// <summary>
        public Nullable<decimal> FCPbyMinValue { get; set; }

        /// <summary>
        ///ถึง Qty/Amt ค่า
        /// <summary>
        public Nullable<decimal> FCPbyMaxValue { get; set; }

        /// <summary>
        ///จากเวลา ขั้นต้ำที่มีผล
        /// <summary>
        public string FTPbyMinTime { get; set; }

        /// <summary>
        ///ถึงเวลา  ไม่เกิน 24:00:00 และไม่น้อยกว่า จากเวลา
        /// <summary>
        public string FTPbyMaxTime { get; set; }

    }
}
