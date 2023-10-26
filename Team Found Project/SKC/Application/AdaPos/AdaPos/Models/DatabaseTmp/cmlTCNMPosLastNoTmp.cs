using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMPosLastNoTmp
    {
        public string FTPosCode { get; set; }
        public int FNPosDocType { get; set; }
        public string FTPosComName { get; set; }
        public Nullable<long> FNPosLastNo { get; set; }
        public Nullable<DateTime> FDPosLastSale { get; set; }

    }
}
