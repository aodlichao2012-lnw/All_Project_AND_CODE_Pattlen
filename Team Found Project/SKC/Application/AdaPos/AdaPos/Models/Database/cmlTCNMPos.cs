using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNMPos
    {
        public string FTPosCode { get; set; }
        public string FTPosType { get; set; }
        public string FTPosRegNo { get; set; }
        public string FTSmgCode { get; set; }
        public string FTPosStaRorW { get; set; }
        public string FTPosStaPrnEJ { get; set; }
        public string FTPosStaVatSend { get; set; }
        public string FTPosStaUse { get; set; }
        public string FTPosStaShift { get; set; }
        public Nullable<DateTime> FDDateUpd { get; set; }
        public string FTTimeUpd { get; set; }
        public string FTWhoUpd { get; set; }
        public Nullable<DateTime> FDDateIns { get; set; }
        public string FTTimeIns { get; set; }
        public string FTWhoIns { get; set; }

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนสแกน 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string FTPosStaSumScan { get; set; }     //*Arm 63-05-05

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string FTPosStaSumPrn { get; set; }    //*Arm 63-05-05
    }
}
