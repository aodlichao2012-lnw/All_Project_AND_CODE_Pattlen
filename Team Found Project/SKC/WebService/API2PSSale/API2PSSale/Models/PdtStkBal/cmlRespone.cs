using API2PSSale.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.PdtStkBal
{
    public class cmlRespone : cmlResBase
    {
        public List<cmlPdtFail> aoPdtFail { get; set; }
    }
}