using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Pos
{
    public class cmlResInfoPos
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; } //*Arm 63-01-30

        public string rtPosCode { get; set; }
        public string rtPosType { get; set; }
        public string rtPosRegNo { get; set; }
        public string rtSmgCode { get; set; }
        public string rtPosStaRorW { get; set; }
        public string rtPosStaPrnEJ { get; set; }
        public string rtPosStaVatSend { get; set; }
        public string rtPosStaUse { get; set; }
        public string rtPosStaShift { get; set; }

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนสแกน 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string rtPosStaSumScan { get; set; }     //*Arm 63-05-06

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string rtPosStaSumPrn { get; set; }    //*Arm 63-05-06
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
        
    }
}
