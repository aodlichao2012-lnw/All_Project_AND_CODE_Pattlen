using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNTPdtPrice4PDTTmp
    {
        public string FTPdtCode { get; set; }
        public string FTPunCode { get; set; }
        public DateTime FDPghDStart { get; set; }
        public string FTPghTStart { get; set; }
        public Nullable<DateTime> FDPghDStop { get; set; }
        public string FTPghTStop { get; set; }
        public string FTPghDocNo { get; set; }
        public string FTPghDocType { get; set; }
        public string FTPghStaAdj { get; set; }
        public Nullable<double> FCPgdPriceRet { get; set; }
        //public Nullable<double> FCPgdPriceWhs { get; set; } //*Arm 63-06-16 Comment Code
        //public Nullable<double> FCPgdPriceNet { get; set; } //*Arm 63-06-16 Comment Code
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        public string FTLastUpdBy { get; set; }     //*Arm 63-03-26
        public Nullable<DateTime> FDCreateOn { get; set; }      //*Arm 63-03-26
        public string FTCreateBy { get; set; }      //*Arm 63-03-26

        /// <summary>
        /// รหัสกลุ่มราคา
        /// </summary>
        public string FTPplCode { get; set; }   //*Arm 63-03-26

    }
}
