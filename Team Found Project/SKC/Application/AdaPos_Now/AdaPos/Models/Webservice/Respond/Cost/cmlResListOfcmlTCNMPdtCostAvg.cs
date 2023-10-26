using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Cost
{
    public class cmlResListOfcmlTCNMPdtCostAvg
    {
        public List<cmlTCNMPdtCostAvg> raItems { get; set; }
        public int rnCurrentPage { get; set; }
        public int rnAllPage { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }
}
