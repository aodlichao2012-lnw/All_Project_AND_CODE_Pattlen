using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Model.ExportSale
{
    public class cmlOrderMessageSet
    {
        /// <summary>
        /// Message Type
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// Message ID
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// Message Number
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Message Text 
        /// </summary>
        public string ErrorDesc { get; set; }
    }
}
