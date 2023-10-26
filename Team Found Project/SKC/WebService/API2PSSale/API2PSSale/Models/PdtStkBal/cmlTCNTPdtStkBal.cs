using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API2PSSale.Models.PdtStkBal
{
    public class cmlTCNTPdtStkBal
    {
        public string FTBchCode { get; set; }
        public string FTWahCode { get; set; }
        public string FTPdtCode { get; set; }
        public decimal FCStkQty { get; set; }
    }
}
