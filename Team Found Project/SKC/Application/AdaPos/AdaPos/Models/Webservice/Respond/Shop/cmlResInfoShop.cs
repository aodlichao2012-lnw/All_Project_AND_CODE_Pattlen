using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Shop
{
    public class cmlResInfoShop
    {
        public string rtBchCode { get; set; }
        public string rtShpCode { get; set; }
        public string rtWahCode { get; set; }
        public string rtMerCode { get; set; }
        public string rtShpType { get; set; }
        public string rtShpRegNo { get; set; }
        public string rtShpRefID { get; set; }
        public Nullable<DateTime> rdShpStart { get; set; }
        public Nullable<DateTime> rdShpStop { get; set; }
        public Nullable<DateTime> rdShpSaleStart { get; set; }
        public Nullable<DateTime> rdShpSaleStop { get; set; }
        public string rtShpStaActive { get; set; }
        public string rtShpStaClose { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
