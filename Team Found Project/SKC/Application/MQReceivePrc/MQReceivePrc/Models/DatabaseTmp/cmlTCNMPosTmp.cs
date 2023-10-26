using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPosTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; } //*Arm 63-01-30

        public string FTPosCode { get; set; }
        public string FTPosType { get; set; }
        public string FTPosRegNo { get; set; }
        public string FTSmgCode { get; set; }
        public string FTPosStaRorW { get; set; }
        public string FTPosStaPrnEJ { get; set; }
        public string FTPosStaVatSend { get; set; }
        public string FTPosStaUse { get; set; }
        public string FTPosStaShift { get; set; }

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนสแกน 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string FTPosStaSumScan { get; set; }     //*Arm 63-05-06

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string FTPosStaSumPrn { get; set; }    //*Arm 63-05-06

        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
