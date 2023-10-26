using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required.Stock
{
    public class cmlReqChkStockMulti
    {

        /// <summary>
        /// รหัส AD
        /// </summary>
        [JsonProperty("SaleOrg")]
        public string tSaleOrg { get; set; }

        /// <summary>
        /// รหัสสาขาของ AD
        /// </summary>
        [JsonProperty("PlantCode")]
        public string tPlantCode { get; set; }

        /// <summary>
        /// รหัสคลังสินค้า
        /// </summary>
        [JsonProperty("SLoc")]
        public string tStorage { get; set; }

        /// <summary>
        /// Stocks
        /// </summary>
        [JsonProperty("Stocks")]
        public List<cmlReqChkStockMultiStorage> aoStocks { get; set; }

    }
    public class cmlReqChkStockMultiStorage
    {
        /// <summary>
        /// รหัส Partno
        /// </summary>
        [JsonProperty("MatNo")]
        public string tPartNo { get; set; }

        /// <summary>
        /// จำนวนที่ลูกค้าจะซื้อ
        /// </summary>
        [JsonProperty("MatQty")]
        public decimal cQty { get; set; }
    }
}
