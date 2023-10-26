using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CardType
{
    public class cmlResCardTypeDwn
    {
        public List<cmlResInfoCardType> raCardType { get; set; }
        public List<cmlResInfoCardTypeLng> raCardTypeLng { get; set; }
    }
}
