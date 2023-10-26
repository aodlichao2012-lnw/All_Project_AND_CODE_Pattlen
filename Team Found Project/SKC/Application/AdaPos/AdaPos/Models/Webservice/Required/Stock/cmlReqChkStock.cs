using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AdaPos.Models.Webservice.Required.Stock
{
    public class cmlReqChkStock
    {
        /// <summary>
        /// รหัส PartNo
        /// </summary>
        [JsonProperty("MatNo")]
        public string tPartNo { get; set; }

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
    }
}
