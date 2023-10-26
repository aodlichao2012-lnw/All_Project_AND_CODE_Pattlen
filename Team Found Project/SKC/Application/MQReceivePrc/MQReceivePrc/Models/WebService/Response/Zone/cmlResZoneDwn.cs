using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Zone
{
    public class cmlResZoneDwn
    {
        public List<cmlResInfoZone> raZone { get; set; }
        public List<cmlResInfoZoneLng> raZoneLng { get; set; }
    }
}
