using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.CustomerType
{
    public class cmlResCstTypeDwn
    {
        public List<cmlResInfoCstType> raCstType { get; set; }
        public List<cmlResInfoCstTypeLng> raCstTypeLng { get; set; }
    }
}
