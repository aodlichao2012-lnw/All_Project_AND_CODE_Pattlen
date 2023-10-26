using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.WebService.Request.GenDocNo
{
    public class cmlReqGenDocNo
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }
        
        /// <summary>
        /// ประเภท
        /// </summary>
        public int pnSaleType { get; set; }
    }
}