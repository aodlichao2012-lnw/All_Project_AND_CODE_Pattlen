using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.UserDepart
{
    public class cmlResUsrDepDwn
    {
        public List<cmlResInfoUsrDep> raUsrDep { get; set; }
        public List<cmlResInfoUsrDepLng> raUsrDepLng { get; set; }
    }
}
