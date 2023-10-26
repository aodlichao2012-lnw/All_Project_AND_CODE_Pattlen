using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Events
{
    public class cmlResEventDwn
    {
        public List<cmlResInfoEventHD> raEvnHD { get; set; }
        public List<cmlResInfoEventHDLng> raEvnHDLng { get; set; }
        public List<cmlResInfoEventDT> raEvnDT { get; set; }
        public List<cmlResInfoEventDTLng> raEvnDTLng { get; set; }
    }
}
