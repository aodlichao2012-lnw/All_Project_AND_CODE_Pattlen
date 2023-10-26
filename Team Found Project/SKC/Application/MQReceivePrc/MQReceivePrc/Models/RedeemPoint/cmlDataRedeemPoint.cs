using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.RedeemPoint
{
    public class cmlDataRedeemPoint
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }
        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string ptDocNo { get; set; }
    }

    public class cmlDataUrl
    {
        public string ptURLJson { get; set; }
    }

    
}
