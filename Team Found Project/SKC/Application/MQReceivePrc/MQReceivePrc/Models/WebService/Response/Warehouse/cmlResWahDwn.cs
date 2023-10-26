using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Warehouse
{
    public class cmlResWahDwn
    {
        public List<cmlResInfoWah> raWah { get; set; }
        public List<cmlResInfoWahLng> raWahLng { get; set; }
    }
}
