﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResSysPmtDwn
    {
        public List<cmlResInfoSysPmt> raSysPmt { get; set; }
        public List<cmlResInfoSysPmtLng> raSysPmtLng { get; set; }
    }
}