using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTFNMBankNoteTmp
    {
        public string FTRteCode { get; set; }
        public string FTBntCode { get; set; }
        public string FTBntStaShw { get; set; }
        public Nullable<double> FCBntRateAmt { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
