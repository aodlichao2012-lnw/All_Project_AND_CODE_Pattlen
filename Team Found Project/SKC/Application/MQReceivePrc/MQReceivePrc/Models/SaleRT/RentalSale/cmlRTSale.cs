using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.SaleRT.RentalSale
{
    class cmlRTSale
    {
        /// <summary>
        /// Class model TRTTSalHD
        /// </summary>
        public cmlTRTTSalHD oTRTTSalHD { get; set; }

        /// <summary>
        ///  Class model list TRTTSalDT
        /// </summary>

        public List<cmlTRTTSalDT> aoTRTTSalDT { get; set; }

        /// <summary>
        ///  Class model list TRTTSalDTSN
        /// </summary>

        public List<cmlTRTTSalDTSN> aoTRTTSalDTSN { get; set; }

        /// <summary>
        /// Class model list TRTTSalDTSL
        /// </summary>

        public List<cmlTRTTSalDTSL> aoTRTTSalDTSL { get; set; }
    }
}
