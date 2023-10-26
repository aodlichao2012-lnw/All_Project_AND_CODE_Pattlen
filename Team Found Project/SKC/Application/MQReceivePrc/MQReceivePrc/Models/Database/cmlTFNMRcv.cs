using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTFNMRcv
    {
        public string FTRcvCode { get; set; }
        public string FTFmtCode { get; set; }
        public string FTRcvStaUse { get; set; }
        public string FTRcvStaShwInSlip { get; set; }
        public string FTRcv4Ret { get; set; }
        public string FTRcv4ChkOut { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }
        public int FNLngID { get; set; }
        public string FTRcvName { get; set; }
        public string FTRcvRmk { get; set; }
    }
}
