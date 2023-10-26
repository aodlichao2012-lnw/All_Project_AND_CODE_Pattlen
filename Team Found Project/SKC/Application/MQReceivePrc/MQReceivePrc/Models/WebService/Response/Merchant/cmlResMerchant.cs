using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Merchant
{
    public class cmlResMerchant
    {
        public List<cmlTCNMMerchant> raTCNMMerchant { get; set; }
        public List<cmlTCNMMerchant_L> raTCNMMerchant_L { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }  //*Em 62-10-04
    }
}
