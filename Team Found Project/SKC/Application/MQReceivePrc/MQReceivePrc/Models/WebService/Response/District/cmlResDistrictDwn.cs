using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.District
{
    public class cmlResDistrictDwn
    {
        public List<cmlResInfoDistrict> raDistrinct { get; set; }
        public List<cmlResInfoDistrictLng> raDistrinctLng { get; set; }
    }
}
