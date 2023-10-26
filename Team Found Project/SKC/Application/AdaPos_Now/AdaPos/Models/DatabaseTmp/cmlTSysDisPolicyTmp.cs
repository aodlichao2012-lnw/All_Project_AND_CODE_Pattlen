using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTSysDisPolicyTmp
    {
        /// <summary>
        ///รหัส Discount policy
        /// </summary>
        public string FTDisCode { get; set; }

        /// <summary>
        ///กลุ่มส่วยลด  1=ราย Item 2=Promotion 3=ท้ายบิล
        /// </summary>
        public string FTDisGroup { get; set; }

        /// <summary>
        ///ระดับขอ Policy ส่วนลด ถ้า Level ที่ลดแล้ว > Level Function ปัจจุบัน ต้องทำงานไม่ได้
        /// </summary>
        public Nullable<Int64> FNDisLevel { get; set; }

        /// <summary>
        ///สถานะการใช้งาน  1=ใช้งาน  2=ไม่ใช้งาน
        /// </summary>
        public string FTDisStaUse { get; set; }

        /// <summary>
        ///ชื่อ Function ใน POS
        /// </summary>
        public string FTDisPosFunc { get; set; }

        /// <summary>
        ///ยอดที่ใช้คำนวน  1= Full Price/Set Price   2=Net Peice
        /// </summary>
        public string FTDisStaPrice { get; set; }

        /// <summary>
        ///รหัสอ้างอิง
        /// </summary>
        public string FTDisCodeRef { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }
    }
}
