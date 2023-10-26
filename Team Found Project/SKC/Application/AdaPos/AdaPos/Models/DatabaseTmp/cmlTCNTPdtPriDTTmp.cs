using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNTPdtPriDTTmp
    {
        public string FTBchCode { get; set; }
        public string FTPghDocNo { get; set; }
        public long FNPgdSeq { get; set; }
        public string FTPdtCode { get; set; }
        public string FTPunCode { get; set; }
        public Nullable<double> FCPgdPriceRet { get; set; }
        public Nullable<double> FCPgdPriceWhs { get; set; }
        public Nullable<double> FCPgdPriceNet { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
