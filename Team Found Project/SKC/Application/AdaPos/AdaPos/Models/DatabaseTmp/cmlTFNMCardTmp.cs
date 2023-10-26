using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTFNMCardTmp
    {
        public string FTCrdCode { get; set; }
        public Nullable<DateTime> FDCrdStartDate { get; set; }
        public Nullable<DateTime> FDCrdExpireDate { get; set; }
        public Nullable<DateTime> FDCrdResetDate { get; set; }
        public Nullable<DateTime> FDCrdLastTopup { get; set; }
        public string FTCtyCode { get; set; }
        public Nullable<double> FCCrdValue { get; set; }
        public Nullable<double> FCCrdDeposit { get; set; }
        public string FTCrdHolderID { get; set; }
        public string FTCrdRefID { get; set; }
        public string FTCrdStaType { get; set; }
        public string FTCrdStaLocate { get; set; }
        public string FTCrdStaActive { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }
    }
}
