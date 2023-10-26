using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Province
{
    public class cmlResInfoPvn
    {
        public string rtPvnCode { get; set; }
        public string rtZneCode { get; set; }
        public string rtPvnLatitude { get; set; }
        public string rtPvnLongitude { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
