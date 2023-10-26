using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Province
{
    public class cmlResPvnDwn
    {
        public List<cmlResInfoPvn> raPvn { get; set; }
        public List<cmlResInfoPvnLng> raPvnLng { get; set; }
    }
}
