using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Receive
{
    public class cmlResInfoRcv
    {
        public string rtRcvCode { get; set; }
        public string rtFmtCode { get; set; }
        public string rtRcvStaUse { get; set; }

        /// <summary>
        /// สถานะใฃ้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// </summary>
        public string rtRcvStaShwInSlip { get; set; }       //*Arm 63-01-30

        /// <summary>
        /// รหัสรูปแบบการรับชำระเงินกรณีทำรายการ คืน
        /// </summary>
        public string rtRcv4Ret { get; set; }               //*Arm 63-01-30

        /// <summary>
        /// รหัสรูปแบบการรับชำระเงินกรณีทำรายการ checkout
        /// </summary>
        public string rtRcv4ChkOut { get; set; }            //*Arm 63-01-30

        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
        
    }
}
