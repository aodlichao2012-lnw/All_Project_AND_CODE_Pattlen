using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.PdtStkBal
{
    public class cmlPdtStk
    {
        public string tBachCode { get; set; }
        public string tMerchant { get; set; }
        public List<cmlDT> aoDt { get; set; }
    }
}