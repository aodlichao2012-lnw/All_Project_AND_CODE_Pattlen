using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Doc.PdtTnfBch
{
    public class cmlTCNTPdtTbx
    {
        public cmlTCNTPdtTbxHD oTCNTPdtTbxHD { get; set; }
        public cmlTCNTPdtTbxHDRef oTCNTPdtTbxHDRef { get; set; }
        public List<cmlTCNTPdtTbxDT> aoTCNTPdtTbxDT { get; set; }
    }
}
