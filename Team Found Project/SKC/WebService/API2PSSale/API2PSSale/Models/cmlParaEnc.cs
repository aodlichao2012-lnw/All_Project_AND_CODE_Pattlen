using API2PSSale.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSSale.Models
{
    /// <summary>
    /// Class model parameter Encrypt
    /// </summary>
    public class cmlParaEnc
    {
        /// <summary>
        /// tUnknown
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string tUnknown { get; set; }
    }
}