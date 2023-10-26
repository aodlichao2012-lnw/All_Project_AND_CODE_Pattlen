using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Sale
{
    public class cmlTPSTSal
    {
        public List<cmlTPSTSalHD> aoTPSTSalHD { get; set; }
        public List<cmlTPSTSalHDCst> aoTPSTSalHDCst { get; set; }
        public List<cmlTPSTSalHDDis> aoTPSTSalHDDis { get; set; }
        public List<cmlTPSTSalDT> aoTPSTSalDT { get; set; }
        public List<cmlTPSTSalDTDis> aoTPSTSalDTDis { get; set; }
        public List<cmlTPSTSalDTPmt> aoTPSTSalDTPmt { get; set; }
        public List<cmlTPSTSalRC> aoTPSTSalRC { get; set; }
        public List<cmlTPSTSalRD> aoTPSTSalRD { get; set; } //*Arm 63-03-13
        public List<cmlTPSTSalPD> aoTPSTSalPD { get; set; } //*Arm 63-03-26
        public List<cmlTCNTMemTxnSale> aoTCNTMemTxnSale { get; set; } //*Arm 63-05-07
        public List<cmlTCNTMemTxnRedeem> aoTCNTMemTxnRedeem { get; set; } //*Arm 63-05-07
        public string ptWahStaPrcStk { get; set; } //*Arm 63-08-04 flag เช็ค Process Stock
    }
}
