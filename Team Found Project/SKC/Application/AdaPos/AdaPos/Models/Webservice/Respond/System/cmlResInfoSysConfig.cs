using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.System
{
    public class cmlResInfoSysConfig
    {
        public string rtSysCode { get; set; }
        public string rtSysApp { get; set; }
        public string rtSysKey { get; set; }
        public string rtSysSeq { get; set; }
        public string rtGmnCode { get; set; }
        public string rtSysStaAlwEdit { get; set; }
        public string rtSysStaDataType { get; set; }
        public Nullable<int> rnSysMaxLength { get; set; }
        public string rtSysStaDefValue { get; set; }
        public string rtSysStaDefRef { get; set; }
        public string rtSysStaUsrValue { get; set; }
        public string rtSysStaUsrRef { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
