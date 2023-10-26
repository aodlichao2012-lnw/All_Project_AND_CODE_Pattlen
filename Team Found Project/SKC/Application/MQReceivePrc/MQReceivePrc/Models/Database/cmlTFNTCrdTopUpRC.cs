using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTFNTCrdTopUpRC
    {
        public string FTBchCode { get; set; }
        public string FTCthDocNo { get; set; }
        public int FNTxnID { get; set; }
        public int FNXrcSeqNo { get; set; }
        public string FTRcvCode { get; set; }
        public string FTRcvName { get; set; }
        public string FTXrcRefNo1 { get; set; }
        public string FTXrcRefNo2 { get; set; }
        public Nullable<DateTime>  FDXrcRefDate { get; set; }
        public string FTXrcRefDesc { get; set; }
        public string FTBnkCode { get; set; }
        public string FTRteCode { get; set; }
        public double FCXrcRteFac { get; set; }
        public double FCXrcFrmLeftAmt { get; set; }
        public double FCXrcUsrPayAmt { get; set; }
        public double FCXrcNet  { get; set; }
        public double FCXrcChg { get; set; }
        public string FTXrcRmk { get; set; }
        public string FTPhwCode { get; set; }
        public string FTXrcRetDocRef { get; set; }
    }
}
