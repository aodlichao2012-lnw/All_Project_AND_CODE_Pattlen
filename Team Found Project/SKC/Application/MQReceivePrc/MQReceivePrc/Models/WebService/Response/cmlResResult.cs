using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response
{
    public class cmlResResult
    {
        public int rnCurrentPage { get; set; }
        public int rnAllPage { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
        public string rtMsg { get; set; }
    }
}
