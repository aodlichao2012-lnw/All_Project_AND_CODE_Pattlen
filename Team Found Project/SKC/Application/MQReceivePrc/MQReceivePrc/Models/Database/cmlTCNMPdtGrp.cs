using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTCNMPdtGrp
    {
        public string FTPgpCode { get; set; }
        public Nullable<long> FNPgpLevel { get; set; }
        public string FTPgpParent { get; set; }
        public string FTPgpChain { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }
        public long FNLngID { get; set; }
        public string FTPgpName { get; set; }
        public string FTPgpChainName { get; set; }
        public string FTPgpRmk { get; set; }
    }
}
