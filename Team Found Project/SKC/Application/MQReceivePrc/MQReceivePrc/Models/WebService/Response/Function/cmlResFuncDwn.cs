using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Function
{
    public class cmlResFuncDwn
    {
        public List<cmlResInfoFuncHD> raFuncHD { get; set; }
        public List<cmlResInfoFuncDT> raFuncDT { get; set; }
        public List<cmlResInfoFuncDTLng> raFuncDTLng { get; set; }
    }
}
