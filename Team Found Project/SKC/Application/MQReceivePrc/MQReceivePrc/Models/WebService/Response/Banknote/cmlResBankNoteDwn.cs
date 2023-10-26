using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Banknote
{
    public class cmlResBankNoteDwn
    {
        public List<cmlResInfoBankNote> raBankNote { get; set; }
        public List<cmlResInfoBankNoteLng> raBankNoteLng { get; set; }
    }
}
