using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cmlRcvConsolidate
    {
        /// <summary>
        /// Connect string of shop.
        /// </summary>
        public string ptConnStr { get; set; }

        /// <summary>
        /// Doccument no.
        /// </summary>
        public string ptDocNo { get; set; }
    }
}
