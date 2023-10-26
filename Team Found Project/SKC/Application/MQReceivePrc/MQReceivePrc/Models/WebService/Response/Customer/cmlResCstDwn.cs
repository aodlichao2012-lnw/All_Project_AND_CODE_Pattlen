using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Customer
{
    public class cmlResCstDwn
    {
        public List<cmlResInfoCst> raCst { get; set; }
        public List<cmlResInfoCstLng> raCstLng { get; set; }
        public List<cmlResInfoCstCard> raCstCard { get; set; }
        public List<cmlResInfoCstContactLng> raCstContactLng { get; set; }
        public List<cmlResInfoCstCredit> raCstCredit { get; set; }
        public List<cmlResInfoCstRFIDLng> raCstRFIDLng { get; set; }
        public List<cmlResInfoCstPoint> raCstPoint { get; set; }
    }
}
