using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.System
{
    public class cmlResInfoSysScaleMap
    {
        public string rtSslCode { get; set; }
        public int rnSsmSeq { get; set; }
        public string rtSsmDesc { get; set; }
        public Nullable<long> rnSsmFixLen { get; set; }
        public string rtSsmChrSplit { get; set; }
        public string rtSsmChrExtra { get; set; }
        public string rtsmKey { get; set; }
    }
}
