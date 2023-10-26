using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CardCouponList
{
    public class cmlResCrdCpnListDwn
    {
        public List<cmlResInfoCrdCpnList> raCrdCpnList { get; set; }
        public List<cmlResInfoCrdCpnListLng> raCrdCpnListLng { get; set; }
    }
}
