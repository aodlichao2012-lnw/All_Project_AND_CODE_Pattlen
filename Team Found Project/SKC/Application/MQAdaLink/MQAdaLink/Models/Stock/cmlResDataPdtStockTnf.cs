using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Models.Stock
{
    public class cmlResDataPdtStockTnf
    {
        /// <summary>
        /// Data(List)
        /// </summary>
        public List<cmlPdtStockData> data { get; set; }
    }
    public class cmlPdtStockData
    {
        /// <summary>
        /// Material document
        /// </summary>
        public string MatDocNo { get; set; }

        /// <summary>
        /// Material document year
        /// </summary>
        public string MatDocYear { get; set; }

        /// <summary>
        /// Material document date
        /// </summary>
        public string MatDocDate { get; set; }

        /// <summary>
        /// Plant
        /// </summary>
        public string Plant { get; set; }

        /// <summary>
        /// Receiving Plant
        /// </summary>
        public string PlantReceived { get; set; }

        /// <summary>
        /// Storage location
        /// </summary>
        public string Sloc { get; set; }

        /// <summary>
        /// Receiving stor. Loc.
        /// </summary>
        public string SlocReceived { get; set; }

        /// <summary>
        /// Material document item
        /// </summary>
        public string MatDocItem { get; set; }

        /// <summary>
        /// Receiving Material
        /// </summary>
        public string MaterialReceived { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public Nullable<decimal> Qty { get; set; }
    }
}
