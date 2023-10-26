using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNMBranch
    {
        public string FTBchCode { get; set; }
        public string FTWahCode { get; set; }
        public string FTBchType { get; set; }
        public string FTBchPriority { get; set; }
        public string FTBchRegNo { get; set; }
        public string FTBchRefID { get; set; }
        public Nullable<DateTime> FDBchStart { get; set; }
        public Nullable<DateTime> FDBchStop { get; set; }
        public Nullable<DateTime> FDBchSaleStart { get; set; }
        public Nullable<DateTime> FDBchSaleStop { get; set; }
        public string FTBchStaActive { get; set; }
        public Nullable<DateTime> FDDateUpd { get; set; }
        public string FTTimeUpd { get; set; }
        public string FTWhoUpd { get; set; }
        public Nullable<DateTime> FDDateIns { get; set; }
        public string FTTimeIns { get; set; }
        public string FTWhoIns { get; set; }
        public int FNLngID { get; set; }
        public string FTBchName { get; set; }
        public string FTBchRmk { get; set; }

    }
}
