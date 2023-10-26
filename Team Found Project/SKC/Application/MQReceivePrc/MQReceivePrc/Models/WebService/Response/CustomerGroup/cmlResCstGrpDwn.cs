using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CustomerGroup
{
    public class cmlResCstGrpDwn
    {
        public List<cmlResInfoCstGrp> raCstGrp { get; set; }
        public List<cmlResInfoCstGrpLng> raCstGrpLng { get; set; }
    }
}
