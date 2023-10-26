using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResSysConfigDwn
    {
        public List<cmlResInfoSysConfig> raConfig { get; set; }
        public List<cmlResInfoSysConfigLng> raConfigLng { get; set; }
    }
}