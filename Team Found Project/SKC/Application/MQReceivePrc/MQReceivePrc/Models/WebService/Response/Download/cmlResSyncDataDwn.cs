using MQReceivePrc.Models.Webservice.Required;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Download
{
    public class cmlResSyncDataDwn
    {
        public List<cmlResInfoSyncData> raSyncData { get; set; }
        public List<cmlResInfoSyncDataLng> raSyncDataLng { get; set; }
    }
}
