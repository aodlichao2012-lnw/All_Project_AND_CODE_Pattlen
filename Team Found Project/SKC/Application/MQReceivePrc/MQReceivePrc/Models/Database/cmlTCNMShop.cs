using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTCNMShop
    {
        public string FTBchCode { get; set; }
        public string FTShpCode { get; set; }
        public string FTWahCode { get; set; }
        public string FTShpType { get; set; }
        public string FTShpRegNo { get; set; }
        public string FTShpRefID { get; set; }
        public Nullable<DateTime> FDShpStart { get; set; }
        public Nullable<DateTime> FDShpStop { get; set; }
        public Nullable<DateTime> FDShpSaleStart { get; set; }
        public Nullable<DateTime> FDShpSaleStop { get; set; }
        public string FTShpStaActive { get; set; }
        public string FTShpStaClose { get; set; }
        public Nullable<DateTime> FDDateUpd { get; set; }
        public string FTTimeUpd { get; set; }
        public string FTWhoUpd { get; set; }
        public Nullable<DateTime> FDDateIns { get; set; }
        public string FTTimeIns { get; set; }
        public string FTWhoIns { get; set; }
        public int FNLngID { get; set; }
        public string FTShpName { get; set; }
        public string FTShpRmk { get; set; }

    }
}
