using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Doc
{
    public class cmlNotiTARTDocApvTxn
    {
        [JsonProperty("ptFTBchCode")] public string tFTBchCode { get; set; }
        [JsonProperty("ptFTXshDocNo")] public string tFTXshDocNo { get; set; }
        [JsonProperty("ptFDXshDocDate")] public string tFDXshDocDate { get; set; }
        [JsonProperty("ptFTXshApvCode")] public string tFTXshApvCode { get; set; }
        [JsonProperty("ptFTHNCode")] public string tFTHNCode { get; set; }
        [JsonProperty("ptFTDptName")] public string tFTDptName { get; set; }
        [JsonProperty("ptFTStaAct")] public string tFTStaAct { get; set; }
        [JsonProperty("ptFTStaDoc")] public string tFTStaDoc { get; set; }
        [JsonProperty("ptFTImgBase64")] public string tFTImgBase64 { get; set; }
    }
}
