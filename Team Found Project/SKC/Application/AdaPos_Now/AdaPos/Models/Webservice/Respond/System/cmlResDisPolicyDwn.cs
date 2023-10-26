using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.System
{
    public class cmlResDisPolicyDwn
    {
        public List<cmlResInfoDisPolicy> raDisPolicy { get; set; }
        public List<cmlResInfoDisPolicyLng> raDisPolicyLng { get; set; }
        public List<cmlResInfoTPSTDiscPolicy> raTPSTDiscPolicy { get; set; }
    }
}
