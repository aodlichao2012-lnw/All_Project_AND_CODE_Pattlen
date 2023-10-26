using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Pos
{
    public class cmlResItemOfcmlResPosDwn
    {
        public cmlResPosDwn roItem { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
}
