using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Card
{
    public class cmlResCardDwn
    {
        public List<cmlResInfoCard> raCard { get; set; }
        public List<cmlResInfoCardLng> raCardLng { get; set; }
    }
}
