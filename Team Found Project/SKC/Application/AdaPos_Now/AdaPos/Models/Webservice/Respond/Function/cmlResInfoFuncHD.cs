using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Function
{
    public class cmlResInfoFuncHD
    {
        public string rtGhdCode { get; set; }
        public string rtGhdApp { get; set; }
        public string rtKbdScreen { get; set; }
        public string rtKbdGrpName { get; set; }
        public Nullable<int> rnGhdMaxPerPage { get; set; }
        public string rtGhdLayOut { get; set; }
        public Nullable<int> rnGhdMaxLayOutX { get; set; }
        public Nullable<int> rnGhdMaxLayOutY { get; set; }
        public string rtGhdStaAlwChg { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
