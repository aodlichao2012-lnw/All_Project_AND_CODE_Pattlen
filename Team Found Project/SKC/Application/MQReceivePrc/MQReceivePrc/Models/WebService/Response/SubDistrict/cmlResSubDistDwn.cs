using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SubDistrict
{
    public class cmlResSubDistDwn
    {
        public List<cmlResInfoSubDist> raSubDist { get; set; }
        public List<cmlResInfoSubDistLng> raSubDistLng { get; set; }
    }
}
