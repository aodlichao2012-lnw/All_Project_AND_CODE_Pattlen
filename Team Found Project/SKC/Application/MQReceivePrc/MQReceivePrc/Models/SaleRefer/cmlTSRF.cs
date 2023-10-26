using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleRefer
{
    public class cmlTSRF
    {
        public Nullable<int> FNXsdSeqNo { get;set;}
        public Nullable<int> FNXsdSeqNoOld { get; set; }
        public Nullable<decimal> FCXsdQty { get; set; }
        public Nullable<decimal> FCXsdQtyRfn { get; set; }
    }
}
