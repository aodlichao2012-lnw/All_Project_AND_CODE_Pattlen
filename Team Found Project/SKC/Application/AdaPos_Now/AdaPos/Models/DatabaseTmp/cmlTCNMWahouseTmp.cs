using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMWahouseTmp
    {
        public string FTBchCode { get; set; }
        public string FTWahCode { get; set; }
        public string FTWahStaType { get; set; }
        public string FTWahRefCode { get; set; }

        /// <summary>
        ///ใช้ตรวจสอบในขั้นตอนการขาย 1: ไม่เช็ค (ขายติดลบได้)  2: เช็ค Local  (Vending)  3: Check Online   API (Ada,3Party)
        /// </summary>
        public string FTWahStaChkStk { get; set; }      //*Arm 63-06-24

        /// <summary>
        ///ใช้ตรวจสอบในขั้นตอนประมวลผลตอนจบบิล 1: ไม่ตัดสต๊อก  2.ดัดสต๊อก
        /// </summary>
        public string FTWahStaPrcStk { get; set; }      //*Arm 63-06-24

        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }

    }
}
