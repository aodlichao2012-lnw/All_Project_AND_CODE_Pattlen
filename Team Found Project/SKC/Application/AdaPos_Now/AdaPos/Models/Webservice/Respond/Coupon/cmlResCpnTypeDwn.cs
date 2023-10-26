using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Coupon
{
    public class cmlResCpnTypeDwn
    {
        //*Net 63-03-06 Coupon Type Download
        //public List<cmlResInfoCpn> raCpn { get; set; }
        //public List<cmlResInfoCpnLng> raCpnLng { get; set; }
        public List<cmlResInfoCpnType> raCpnType { get; set; }
        public List<cmlResInfoCpnType_L> raCpnTypeLng { get; set; }
    }
}
