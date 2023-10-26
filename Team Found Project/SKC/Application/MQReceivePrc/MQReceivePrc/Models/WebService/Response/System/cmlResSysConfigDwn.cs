using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.System
{
    public class cmlResSysConfigDwn
    {
        public List<cmlResInfoSysConfig> raConfig { get; set; }
        public List<cmlResInfoSysConfigLng> raConfigLng { get; set; }
    }
}
