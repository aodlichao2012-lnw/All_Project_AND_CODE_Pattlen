using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Product
{
    public class cmlResInfoPdtBar
    {
        public string rtPdtCode { get; set; }
        public string rtBarCode { get; set; }
        public string rtPunCode { get; set; }
        public string rtBarStaUse { get; set; }
        public string rtBarStaAlwSale { get; set; }
        public string rtBarStaByGen { get; set; }
        public string rtPlcCode { get; set; }
        public Nullable<long> rnPldSeq { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
