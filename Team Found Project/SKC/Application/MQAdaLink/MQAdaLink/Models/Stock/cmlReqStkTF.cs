using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Models.Stock
{
    class cmlReqStkTF
    {
        public string MatDocNo { get; set; }
        public string MatDocYear { get; set; }
        public string MatDocDate { get; set; }
        public string Plant { get; set; }
        public string PlantReceive { get; set; }
        public string Sloc { get; set; }
        public string SlocReceive { get; set; }
        public List<cmlMatDocItem> MatDocItemSet { get; set; }

        
    }
    public class cmlMatDocItem
    {
        public string MatDocItem { get; set; }
        public string MaterialReceived { get; set; }
        public string Unit { get; set; }
        public Int32 Qty { get; set; }
    }
}
