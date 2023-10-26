using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductRental
{
    public class cmlResInfoPdtRental
    {
        public string rtPdtCode { get; set; }
        public string rtPdtStaReqRet { get; set; }
        public string rtPdtRentType { get; set; }
        public Nullable<double> rcPdtRentTimeQty { get; set; }
        public Nullable<double> rcPdtQtyAvai { get; set; }
        public Nullable<double> rcPdtRent { get; set; }
        public Nullable<double> rcPdtDeposit { get; set; }
        public Nullable<double> rcPdtFee { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
