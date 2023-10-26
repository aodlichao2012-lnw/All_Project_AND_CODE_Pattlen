using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rate
{
    //[Serializable]
    public class cmlResRateDwn
    {
        public List<cmlResInfoRate> raRate { get; set; }
        public List<cmlResInfoRateLng> raRateLng { get; set; }
        public List<cmlResInfoRateUnit> raRateUnit { get; set; }
    }
}