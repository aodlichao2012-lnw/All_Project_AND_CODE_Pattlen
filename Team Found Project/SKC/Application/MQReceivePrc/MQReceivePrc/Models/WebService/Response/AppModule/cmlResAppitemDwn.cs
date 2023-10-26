using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.AppModule
{
    public class cmlResAppitemDwn
    {
        public List<cmlResAppModule> raTCNMAppModule { get; set; }
        public List<cmlResSysApp> raTSysApp { get; set; }
        public List<cmlResSysApp_L> raTSysApp_L { get; set; }
    }
}
