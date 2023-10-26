using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPdtBarTmp
    {
        public string FTPdtCode { get; set; }
        public string FTBarCode { get; set; }
        public string FTPunCode { get; set; }
        public string FTBarStaUse { get; set; }
        public string FTBarStaAlwSale { get; set; }
        public string FTBarStaByGen { get; set; }
        public string FTPlcCode { get; set; }
        public Nullable<long> FNPldSeq { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
