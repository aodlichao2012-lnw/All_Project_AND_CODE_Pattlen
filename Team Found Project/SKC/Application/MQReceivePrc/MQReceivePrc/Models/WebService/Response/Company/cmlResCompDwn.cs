using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Company
{
    public class cmlResCompDwn
    {
        public List<cmlResInfoComp> raComp { get; set; }
        public List<cmlResInfoCompLng> raCompLng { get; set; }
        public List<cmlResInfoImgObj> raImage { get; set; }
    }
}
