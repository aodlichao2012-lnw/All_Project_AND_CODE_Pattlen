using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Request
{
    public class cmlReqMonitorSrv
    {
        [JsonProperty("ptBchCode")]
        public string tBchCode { get; set; }

        [JsonProperty("ptPosCode")]
        public string tPosCode { get; set; }

        [JsonProperty("ptShiftCode")]
        public string tShiftCode { get; set; }

        [JsonProperty("ptDateRequest")]
        public string tDateRequest { get; set; }

        [JsonProperty("pnType")]
        public string tType { get; set; }

        [JsonProperty("pcPosGrand")]
        public decimal cPosGrand { get; set; }

        [JsonProperty("ptPosLastSync")]
        public string tPosLastSync { get; set; }

        [JsonProperty("ptQGetPepairing")]
        public string tQGetPepairing { get; set; }

        [JsonProperty("ptUser")]
        public string tUser { get; set; }

        [JsonProperty("ptConnStr")]
        public string tConnStr { get; set; }

        [JsonProperty("ptPosLastDocNo")]
        public string tPosLastDocNo { get; set; }
    }
}
