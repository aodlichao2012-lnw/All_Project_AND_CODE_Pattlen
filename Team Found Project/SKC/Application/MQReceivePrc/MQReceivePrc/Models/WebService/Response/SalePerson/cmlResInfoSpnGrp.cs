using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.SalePerson
{
    public class cmlResInfoSpnGrp
    {
        public string rtSpnCode { get; set; }
        public string rtBchCode { get; set; }
        public string rtSpnStaShop { get; set; }
        public string rtShpCode { get; set; }
        public Nullable<DateTime> rdSpnStart { get; set; }
        public Nullable<DateTime> rdSpnStop { get; set; }
    }
}
