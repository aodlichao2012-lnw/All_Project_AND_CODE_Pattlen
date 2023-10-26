using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Receive
{
    public class cmlResRcvDwn
    {
        public List<cmlResInfoRcv> raRcv { get; set; }
        public List<cmlResInfoRcvLng> raRcvLng { get; set; }
        public List<cmlResInfoRcvSpc> raRcvSpc { get; set; }    //*Arm 63-04-24
    }
}
