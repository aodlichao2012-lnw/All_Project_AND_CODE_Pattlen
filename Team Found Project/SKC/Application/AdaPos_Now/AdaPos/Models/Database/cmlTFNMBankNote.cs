using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTFNMBankNote
    {
        public string FTRteCode { get; set; }
        public string FTBntCode { get; set; }
        public string FTBntStaShw { get; set; }
        public Nullable<decimal> FCBntRateAmt { get; set; }
        public Nullable<DateTime> FDDateUpd { get; set; }
        public string FTTimeUpd { get; set; }
        public string FTWhoUpd { get; set; }
        public Nullable<DateTime> FDDateIns { get; set; }
        public string FTTimeIns { get; set; }
        public string FTWhoIns { get; set; }
        public int FNLngID { get; set; }
        public string FTBntName { get; set; }
        public string FTBntRmk { get; set; }
    }
}
