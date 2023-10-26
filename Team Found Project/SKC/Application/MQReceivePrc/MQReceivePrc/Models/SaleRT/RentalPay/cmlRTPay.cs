using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleRT.RentalPay
{
    class cmlRTPay
    {
        /// <summary>
        /// Class model TRTTPayHD
        /// </summary>
        public cmlTRTTPayHD oTRTTPayHD { get; set; }

        /// <summary>
        /// Class model list TRTTPayDT
        /// </summary>
        public List<cmlTRTTPayDT> aoTRTTPayDT { get; set; }

        /// <summary>
        /// Class model list TRTTPayRC
        /// </summary>
        public List<cmlTRTTPayRC> aoTRTTPayRC { get; set; }
    }
}
