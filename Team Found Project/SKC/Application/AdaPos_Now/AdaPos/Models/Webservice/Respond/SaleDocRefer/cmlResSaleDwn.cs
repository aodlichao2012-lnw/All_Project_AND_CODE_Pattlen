using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.SaleDocRefer
{
    public class cmlResItemSaleDwn
    {
        public cmlResSaleDwn roItem;
        /// <summary>
        /// System process status.
        /// </summary>
        public string rtCode;

        /// <summary>
        /// System process description.
        /// </summary>
        public string rtDesc;
    }

    public class cmlResSaleDwn
    {
        public List<cmlResInfoSalHD> aoTPSTSalHD { get; set; }
        public List<cmlResInfoSalHDCst> aoTPSTSalHDCst { get; set; }
        public List<cmlResInfoSalHDDis> aoTPSTSalHDDis { get; set; }
        public List<cmlResInfoSalDT> aoTPSTSalDT { get; set; }
        public List<cmlResInfoSalDTDis> aoTPSTSalDTDis { get; set; }
        public List<cmlResInfoSalDTPmt> aoTPSTSalDTPmt { get; set; }
        public List<cmlResInfoSalRC> aoTPSTSalRC { get; set; }
        public List<cmlResInfoSalRD> aoTPSTSalRD { get; set; }
        public List<cmlResInfoSalPD> aoTPSTSalPD { get; set; }
        public List<cmlResInfoTxnSale> aoTCNTMemTxnSale { get; set; }
        public List<cmlResInfoTxnRedeem> aoTCNTMemTxnRedeem { get; set; }
    }
}
