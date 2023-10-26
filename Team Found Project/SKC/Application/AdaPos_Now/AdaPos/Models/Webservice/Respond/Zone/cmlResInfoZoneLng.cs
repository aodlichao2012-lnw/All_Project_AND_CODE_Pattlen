using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Zone
{
    public class cmlResInfoZoneLng
    {
        public string rtZneCode { get; set; }
        public int rnLngID { get; set; }
        public string rtZneName { get; set; }
        public string rtZneRmk { get; set; }
        public string rtZneChain { get; set; } //*Net 62-12-30 Add ZneChain follow API Response
        public string rtZneChainName { get; set; }

    }
}
