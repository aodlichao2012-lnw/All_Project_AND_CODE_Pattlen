using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rate
{
    //[Serializable]
    public class cmlResInfoRateUnit
    {
        public string rtRteCode { get; set; }
        public int rnRtuSeq { get; set; }
        public Nullable<decimal> rcRtuFac { get; set; }
    }
}