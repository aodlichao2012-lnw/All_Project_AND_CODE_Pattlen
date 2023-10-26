using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMPosTmp
    {
        public string FTBchCode { get; set; }   //*Arm 63-04-08
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
        public string FTPosStaSumScan { get; set; }     //*Arm 63-05-05

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string FTPosStaSumPrn { get; set; }    //*Arm 63-05-05

        /// <summary>
        ///สถานะวันที่เปิดรอบเครื่องจุดขาย 1:เลือกวันที่ได้, 2:System Date
        /// </summary>
        public string FTPosStaDate { get; set; }    //*Arm 63-06-016 เพิ่มตามโครงสร้าง SKC

        /// <summary>
        /// Token
        /// </summary>
        public string FTPrgRegToken { get; set; }

        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
