using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMShopTmp
    {
        public string FTBchCode { get; set; }
        public string FTShpCode { get; set; }
        public string FTWahCode { get; set; }
        public string FTMerCode { get; set; }
        public string FTShpType { get; set; }
        public string FTShpRegNo { get; set; }
        public string FTShpRefID { get; set; }
        public Nullable<DateTime> FDShpStart { get; set; }
        public Nullable<DateTime> FDShpStop { get; set; }
        public Nullable<DateTime> FDShpSaleStart { get; set; }
        public Nullable<DateTime> FDShpSaleStop { get; set; }
        public string FTShpStaActive { get; set; }
        public string FTShpStaClose { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
