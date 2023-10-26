using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.CardCouponList
{
    public class cmlResCrdCpnListDwn
    {
        public List<cmlResInfoCrdCpnList> raCrdCpnList { get; set; }
        public List<cmlResInfoCrdCpnListLng> raCrdCpnListLng { get; set; }
    }
}
