using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2ARDoc.Models.Webservice.Request.PdtStockTransfer
{
    public class cmlReqPdtTwxDwn
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }
        
        /// <summary>
        /// เลขที่เอกสารใบโอนสินค้าระหว่างคลัง
        /// </summary>
        public string ptDocNo { get; set; }
    }
}