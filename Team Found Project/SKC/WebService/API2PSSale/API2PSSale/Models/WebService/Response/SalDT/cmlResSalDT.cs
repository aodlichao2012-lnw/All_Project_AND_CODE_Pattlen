using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.WebService.Response.SalDT
{
    /// <summary>
    /// Class model response SalDT
    /// </summary>
    public class cmlResSalDT
    {
        /// <summary>
        /// List Model TPSTSalDT
        /// </summary>
        public List<cmlResTPSTSalDT> raSalDT { get; set; }
    }
}