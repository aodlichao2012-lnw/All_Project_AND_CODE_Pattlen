using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.WebService.Request.DownloadSale
{
    public class cmlReqSaleDwn
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string ptFTXshDocNo{ get; set; }
    }
}