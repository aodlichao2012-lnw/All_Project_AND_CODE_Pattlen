using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Bank
{
    public class cmlResBankDwn
    {
        public List<cmlResInfoBank> raBank { get; set; }

        public List<cmlResInfoBankLng> raBankLng { get; set; }
    }
}
