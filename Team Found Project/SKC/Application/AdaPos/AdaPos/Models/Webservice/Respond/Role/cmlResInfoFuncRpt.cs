﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Role
{
    public class cmlResInfoFuncRpt
    {
        public string rtRolCode { get; set; }
        public string rtUfrType { get; set; }
        public string rtUfrGrpRef { get; set; }
        public string rtUfrRef { get; set; }
        public string rtUfrStaAlw { get; set; }
        public string rtUfrStaFavorite { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
