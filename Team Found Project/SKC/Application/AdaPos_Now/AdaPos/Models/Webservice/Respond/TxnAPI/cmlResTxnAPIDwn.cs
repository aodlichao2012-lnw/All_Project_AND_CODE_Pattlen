using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.TxnAPI
{
    public class cmlResTxnAPIDwn
    {
        public List<cmlResInfoTxnAPI> raTCNMTxnAPI { get; set; }
        public List<cmlResInfoTxnAPILng> raTCNMTxnAPILng { get; set; }
        public List<cmlResInfoTxnSpcAPI> raTCNMTxnSpcAPI { get; set; }
    }
}
