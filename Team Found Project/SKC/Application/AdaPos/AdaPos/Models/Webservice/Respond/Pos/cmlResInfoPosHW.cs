using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Pos
{
    public class cmlResInfoPosHW
    {
        public string rtBchCode { get; set; } //*Arm 63-04-08
        public string rtPhwCode { get; set; }
        public string rtPosCode { get; set; }
        public string rtShwCode { get; set; }
        public string rtPhwCodeRef { get; set; }
        public long rnPhwSeq { get; set; }
        public string rtPhwName { get; set; }
        public string rtPhwConnType { get; set; }
        public string rtPhwConnRef { get; set; }
        public string rtPhwCustom { get; set; }
        public Nullable<int> rnPhwTimeOut { get; set; }
    }
}
