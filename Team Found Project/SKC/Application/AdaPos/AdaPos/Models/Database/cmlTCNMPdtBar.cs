using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNMPdtBar
    {
        public string FTPdtCode { get; set; }
        public string FTBarCode { get; set; }
        public string FTPunCode { get; set; }
        public string FTBarStaUse { get; set; }
        public string FTBarStaAlwSale { get; set; }
        public string FTBarStaByGen { get; set; }
        public string FTPlcCode { get; set; }
        public string FNPldSeq { get; set; }
        public string FDLastUpdOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }
    }
}
