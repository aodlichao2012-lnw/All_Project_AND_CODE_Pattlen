using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.UpdSaleVD
{
    /// <summary>
    /// Class model Sale vending
    /// </summary>
    public class cmlSaleVD
    {
        /// <summary>
        /// TVDTSalHD
        /// </summary>
        public List<cmlTVDTSalHD> aoTVDTSalHD { get; set; }

        /// <summary>
        /// TVDTSalDT
        /// </summary>
        public List<cmlTVDTSalDT> aoTVDTSalDT { get; set; }

        /// <summary>
        /// TVDTSalDTVD
        /// </summary>
        public List<cmlTVDTSalDTVD> aoTVDTSalDTVD { get; set; }

        /// <summary>
        /// TVDTSalRC 
        /// </summary>
        public List<cmlTVDTSalRC> aoTVDTSalRC { get; set; }
     
    }
}