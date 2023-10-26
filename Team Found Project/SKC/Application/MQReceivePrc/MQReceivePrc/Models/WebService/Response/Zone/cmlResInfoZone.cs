using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Zone
{
    public class cmlResInfoZone
    {
        public string rtZneChain { get; set; }
        public string rtZneCode { get; set; }
        public int rnZneLevel { get; set; }
        public string rtZneParent { get; set; }
        public string rtAreCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
