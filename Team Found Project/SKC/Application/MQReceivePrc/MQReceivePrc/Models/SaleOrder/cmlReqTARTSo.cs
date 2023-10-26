using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleOrder
{
    public class cmlReqTARTSo
    {
        [JsonProperty("ptFTCstCode")] public string tFTCstCode { get; set; }
        [JsonProperty("ptFTCstName")] public string tFTCstName { get; set; }
        [JsonProperty("ptFDXshDocDate")] public string tFDXshDocDate { get; set; }
        [JsonProperty("ptFDXshTime")] public string tFDXshTime { get; set; }
        [JsonProperty("ptFTDptCode")] public string tFTDptCode { get; set; }
        [JsonProperty("ptFTDptName")] public string tFTDptName { get; set; }
        [JsonProperty("ptFTPosCode")] public string tFTPosCode { get; set; }
        [JsonProperty("ptFTPosName")] public string tFTPosName { get; set; }
        [JsonProperty("ptFTXshRefExt")] public string tFTXshRefExt { get; set; }
        [JsonProperty("ptFNXsdSeqNo")] public string tFNXsdSeqNo { get; set; }
        [JsonProperty("ptFTPdtCode")] public string tFTPdtCode { get; set; }
        [JsonProperty("ptFTXsdPdtName")] public string tFTXsdPdtName { get; set; }
        [JsonProperty("ptFCXsdQty")] public string tFCXsdQty { get; set; }
        [JsonProperty("ptFTXsdForm")] public string tFTXsdForm { get; set; }
        [JsonProperty("ptFTXsdGnr")] public string tFTXsdGnr { get; set; }
        [JsonProperty("ptFTXsdLbl")] public string tFTXsdLbl { get; set; }
    }
}
