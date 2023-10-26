using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cmlRcvFCBch2HQSale
    {
        /// <summary>
        /// Branch code.
        /// </summary>
        public string ptBchCode { get; set; }
        
        /// <summary>
        /// Pos code.
        /// </summary>
        public string ptPosCode { get; set; }

        /// <summary>
        /// Doccument no.
        /// </summary>
        public string ptDocNo { get; set; }
    }
}
