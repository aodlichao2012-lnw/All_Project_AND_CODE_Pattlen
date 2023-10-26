using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTFNTCrdTopUpDT
    {
        public string FTBchCode { get; set; }
        public string FTCthDocNo { get; set; }
        public int FNCtdSeqNo { get; set; }
        public string FTCrdCode { get; set; }
        public double FCCtdCrdTP { get; set; }
        public string FTCtdStaCrd { get; set; }
        public string FTCtdStaPrc { get; set; }
        public string FTCtdRmk { get; set; }
        public Nullable<DateTime>  FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public Nullable<DateTime>  FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }
    }
}
