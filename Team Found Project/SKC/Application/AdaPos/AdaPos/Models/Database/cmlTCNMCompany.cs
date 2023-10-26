using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNMCompany
    {
        public string FTCmpCode { get; set; }
        public string FTCmpTel { get; set; }
        public string FTCmpFax { get; set; }
        public string FTBchcode { get; set; }
        public string FTCmpWhsInOrEx { get; set; }
        public string FTCmpRetInOrEx { get; set; }
        public string FTCmpEmail { get; set; }
        public string FTRteCode { get; set; }
        public string FTVatCode { get; set; }
        public Nullable<DateTime> FDDateUpd { get; set; }
        public string FTTimeUpd { get; set; }
        public string FTWhoUpd { get; set; }
        public Nullable<DateTime> FDDateIns { get; set; }
        public string FTTimeIns { get; set; }
        public string FTWhoIns { get; set; }
        public int FNLngID { get; set; }
        public string FTCmpName { get; set; }
        public string FTCmpShop { get; set; }
        public string FTCmpDirector { get; set; }
        public string FTWahCode { get; set; }
        public string FTBchName { get; set; }
        public decimal FCVatRate { get; set; }

        public string FTPplCode { get; set; } //*Net 63-03-24
    }
}
