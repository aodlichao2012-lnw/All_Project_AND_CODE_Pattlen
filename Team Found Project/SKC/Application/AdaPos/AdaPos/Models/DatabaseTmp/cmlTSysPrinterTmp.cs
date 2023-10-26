using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTSysPrinterTmp
    {
        public string FTSppCode { get; set; }
        public string FTSppValue { get; set; }
        public string FTSppRef { get; set; }
        public string FTSppType { get; set; }
        public string FTSppStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
