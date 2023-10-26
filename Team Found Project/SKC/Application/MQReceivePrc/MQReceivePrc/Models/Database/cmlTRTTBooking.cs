using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    class cmlTRTTBooking
    {
        public Nullable<int> FNBkgDocID { get; set; }
        public string FTAgnCode { get; set; }
        public string FTBchCode { get; set; }
        public string FTBkgProducer { get; set; }
        public string FTUsrCode { get; set; }
        public string FTBkgToBch { get; set; }
        public string FTBkgToPos { get; set; }
        public Nullable<int> FNBkgToLayNo { get; set; }
        public string FTBkgToSize { get; set; }
        public string FTBkgToRate { get; set; }
        public Nullable<decimal> FDBkgToStart { get; set; }
        public string FTBkgRefCst { get; set; }
        public string FTBkgRefCstLogin { get; set; }
        public string FTBkgRefCstDoc { get; set; }
        public string FTBkgRefSale { get; set; }
        public string FTBkgStaBook { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }
    }
}
