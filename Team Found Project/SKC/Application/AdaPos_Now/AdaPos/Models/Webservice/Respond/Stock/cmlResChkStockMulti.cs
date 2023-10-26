using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Stock
{
    public class cmlResChkStockMulti
    {
        //API Error Code
        //001	success.
        //700	all parameter is null.
        //701	validate parameter model false.
        //706	product retail price not allow less than 0.
        //800	data not found.
        //905	cannot connect database.

        //WebService Error Code
        //200	OK
        //202	Acepted
        //401	Unauthorized
        //403	Forbidden
        //408	Request Timeout
        //500	Internal Server Error
        //504	Gateway Timeout
        //525	SSL Handshake Failed

        /// <summary>
        /// ErrorCode
        /// </summary>
        [JsonProperty("ErrorCode")]
        public string tErrorCode { get; set; }

        /// <summary>
        /// Y = ผ่าน  , N=ไม่ผ่าน
        /// </summary>
        [JsonProperty("Flag")]
        public string tFlag { get; set; }

        /// <summary>
        /// Stocks เฉพาะรายการที่ไม่ผ่าน
        /// </summary>
        [JsonProperty("Stocks")]
        public List<cmlResChkStockMultiStorage> aoStocks { get; set; }
    }
    public class cmlResChkStockMultiStorage
    {
        /// <summary>
        /// รหัส Partno
        /// </summary>
        [JsonProperty("MatNo")]
        public string tPartNo { get; set; }

        /// <summary>
        /// จำนวนคงเหลือ
        /// </summary>
        [JsonProperty("MatQty")]
        public decimal cQty { get; set; }
    }
}
