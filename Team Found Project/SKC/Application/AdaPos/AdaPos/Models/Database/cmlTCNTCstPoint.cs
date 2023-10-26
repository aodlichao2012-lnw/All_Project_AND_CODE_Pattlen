using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNTCstPoint
    {
        public string FTBchCode { get; set; }
        public string FTCstCode { get; set; }
        public string FTPntRefDoc { get; set; }
        public string FTSplCode { get; set; }
        public Nullable<DateTime> FDPntDate { get; set; }
        public Nullable<double> FCPntOptBuyAmt { get; set; }
        public Nullable<double> FCPntOptGetAmt { get; set; }
        public Nullable<long> FNPntB4Bill { get; set; }
        public Nullable<double> FCPntBillAmt { get; set; }
        public Nullable<long> FNPntBillQty { get; set; }
        public string FTPntExpired { get; set; }
        public string FTPntStaPrcDoc { get; set; }
        public string FTPntCardType { get; set; }
        public string FTCptJDate { get; set; }
        public string FTCptTime { get; set; }
        public Nullable<DateTime> FDPntSplStart { get; set; }
        public Nullable<DateTime> FDPntSplExpired { get; set; }
        public string FTPntStaExpired { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
