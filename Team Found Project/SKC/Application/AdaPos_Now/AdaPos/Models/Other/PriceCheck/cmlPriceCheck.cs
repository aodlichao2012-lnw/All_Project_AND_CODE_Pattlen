using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other.PriceCheck
{
    public class cmlPriceCheck
    {
        public string FTPdtCode { get; set; }
        public string FTPdtName { get; set; }
        public string FTBarCode { get; set; }
        public string FTPunName { get; set; }
        public string FCPdtUnitFact { get; set; }
        public DateTime FDPghDStart { get; set; }
        public DateTime FDPghDStop { get; set; }
        public string FCPgdPriceRet { get; set; }
        public string FTPmhName { get; set; }

        public string FTPplName { get; set; } //*Arm 63-04-13 กลุ่มราคา
        public string FTPghDocNo { get; set; } //*Arm 63-04-13 เลขที่เอกสาร


        public string FTPmhDocNo { get; set; }      //*Arm 63-04-13 เลขที่เอกสาร Promotion 
        public DateTime FDPmhDStart { get; set; }   //*Arm 63-04-13  วันที่เริ่ม Promotion
        public DateTime FDPmhDStop { get; set; }    //*Arm 63-04-13  วันที่หยุด  Promotion
        public string FTPmdBarCode { get; set; }    //*Arm 63-04-13 Barcoe Promotion
    }
}
