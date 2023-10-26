using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPdtRentalTmp
    {
        public string FTPdtCode { get; set; }
        public string FTPdtStaReqRet { get; set; }
        public string FTPdtRentType { get; set; }
        public Nullable<double> FCPdtRentTimeQty { get; set; }
        public Nullable<double> FCPdtQtyAvai { get; set; }
        public Nullable<double> FCPdtRent { get; set; }
        public Nullable<double> FCPdtDeposit { get; set; }
        public Nullable<double> FCPdtFee { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
