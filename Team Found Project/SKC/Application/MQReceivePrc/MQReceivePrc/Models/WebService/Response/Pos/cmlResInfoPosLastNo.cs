using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Pos
{
    public class cmlResInfoPosLastNo
    {
        public string rtPosCode { get; set; }
        public int rnPosDocType { get; set; }
        public string rtPosComName { get; set; }
        public Nullable<long> rnPosLastNo { get; set; }
        public Nullable<DateTime> rdPosLastSale { get; set; }
    }
}
