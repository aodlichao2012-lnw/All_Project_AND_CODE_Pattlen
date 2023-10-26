using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMPrinterTmp
    {
        public string FTPrnCode { get; set; }
        public string FTPrnSrcType { get; set; }
        public string FTSppCode { get; set; }
        public string FTPrnDriver { get; set; }
        public string FTPrnType { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
