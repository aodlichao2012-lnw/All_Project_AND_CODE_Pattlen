using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNMRcvTmp
    {
        public string FTRcvCode { get; set; }
        public string FTFmtCode { get; set; }
        public string FTRcvStaUse { get; set; }

        /// <summary>
        /// สถานะใฃ้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// </summary>
        public string FTRcvStaShwInSlip { get; set; }       //*Arm 63-01-30

        /// <summary>
        /// รหัสรูปแบบการรับชำระเงินกรณีทำรายการ คืน
        /// </summary>
        public string FTRcv4Ret { get; set; }               //*Arm 63-01-30

        /// <summary>
        /// รหัสรูปแบบการรับชำระเงินกรณีทำรายการ checkout
        /// </summary>
        public string FTRcv4ChkOut { get; set; }            //*Arm 63-01-30

        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
