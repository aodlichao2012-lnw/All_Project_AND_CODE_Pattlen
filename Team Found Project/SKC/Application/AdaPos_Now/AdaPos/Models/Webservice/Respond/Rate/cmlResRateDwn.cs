﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Rate
{
    public class cmlResRateDwn
    {
        public List<cmlResInfoRate> raRate { get; set; }
        public List<cmlResInfoRateLng> raRateLng { get; set; }
        public List<cmlResInfoRateUnit> raRateUnit { get; set; }
    }
}
