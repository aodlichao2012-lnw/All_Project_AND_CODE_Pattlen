using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPdtFhnTmp
    {
        public string FTPdtCode { get; set; }
        public string FTPgpChain { get; set; }
        public string FTDcsCode { get; set; }
        public string FTClrCode { get; set; }
        public string FTPszCode { get; set; }
        public string FTPdtArticle { get; set; }
        public string FTDepCode { get; set; }
        public string FTClsCode { get; set; }
        public string FTSclCode { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

    }
}
