using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.System
{
    public class cmlResInfoSysEdc
    {
        public string rtSedCode { get; set; }
        public string rtSedModel { get; set; }
        public Nullable<long> rnSedAck { get; set; }
        public string rtSedDllVer { get; set; }
        public Nullable<long> rnSedTimeOut { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
