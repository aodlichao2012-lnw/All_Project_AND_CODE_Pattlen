﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rate
{
    //[Serializable]
    public class cmlResInfoRate
    {
        public string rtRteCode { get; set; }
        public Nullable<decimal> rcRteRate { get; set; }
        public Nullable<decimal> rcRteFraction { get; set; }
        public string rtRteType { get; set; }
        public string rtRteTypeChg { get; set; }
        public string rtRteSign { get; set; }
        public string rtRteStaLocal { get; set; }
        public string rtRteStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}