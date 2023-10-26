using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Pos
{
    public class cmlResInfoEdc
    {
        public string rtEdcCode { get; set; }
        public string rtSedCode { get; set; }
        public string rtBnkCode { get; set; }
        public string rtEdcShwFont { get; set; }
        public string rtEdcShwBkg { get; set; }
        public string rtEdcOther { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}
