using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNMCreditCardTmp
    {
        public string FTCrdCode { get; set; }
        public string FTBnkCode { get; set; }
        public Nullable<double> FCCrdChgPer { get; set; }
        public string FTCrdCrdFmt { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
