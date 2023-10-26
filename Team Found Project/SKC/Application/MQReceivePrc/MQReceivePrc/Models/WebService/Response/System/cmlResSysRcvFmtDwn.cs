using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.System
{
    public class cmlResSysRcvFmtDwn
    {
        public List<cmlResInfoSysRcvFmt> raSysRcvFmt { get; set; }
        public List<cmlResInfoSysRcvFmtLng> raSysRcvFmtLng { get; set; }
    }
}
