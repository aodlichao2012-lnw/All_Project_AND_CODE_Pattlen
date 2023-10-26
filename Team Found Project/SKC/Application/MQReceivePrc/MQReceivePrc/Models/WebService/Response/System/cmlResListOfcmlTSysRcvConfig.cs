using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQReceivePrc.Models.Webservice.Response.System;
namespace MQReceivePrc.Models.Webservice.Response.System
{
    class cmlResListOfcmlTSysRcvConfig
    {
        public List<cmlTSysRcvConfig> raItems { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
}
