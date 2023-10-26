using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CreditCard
{
    public class cmlResCreditCardDwn
    {
        public List<cmlResInfoCreditCard> raCreditCard { get; set; }
        public List<cmlResInfoCreditCardLng> raCreditCardLng { get; set; }
    }
}
