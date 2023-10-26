using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Warehouse
{
    public class cmlResInfoWah
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; } //*Arm 63-03-27

        public string rtWahCode { get; set; }
        public string rtWahStaType { get; set; }
        public string rtWahRefCode { get; set; }

        /// <summary>
        ///ใช้ตรวจสอบในขั้นตอนการขาย 1: ไม่เช็ค (ขายติดลบได้)  2: เช็ค Local  (Vending)  3: Check Online   API (Ada,3Party)
        /// </summary>
        public string rtWahStaChkStk { get; set; }      //*Arm 63-06-24

        /// <summary>
        ///ใช้ตรวจสอบในขั้นตอนประมวลผลตอนจบบิล 1: ไม่ตัดสต๊อก  2.ดัดสต๊อก
        /// </summary>
        public string rtWahStaPrcStk { get; set; }      //*Arm 63-06-24
        
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
