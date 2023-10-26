using AdaPos.Models.Webservice.Required;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Download
{
    public class cmlResSyncDataDwn
    {
        public List<cmlResInfoSyncData> raSyncData { get; set; }
        public List<cmlResInfoSyncDataLng> raSyncDataLng { get; set; }
    }
}
