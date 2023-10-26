using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNTPdtPrice4CSTTmp
    {
        public string FTPplCode { get; set; }
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
        public Nullable<double> FCPgdPriceWhs { get; set; }
        public Nullable<double> FCPgdPriceNet { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }

    }
}
