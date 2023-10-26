using API2PSSale.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.WebService.Request
{

    /// <summary>
    /// Class request Sale DT
    /// </summary>
    public class cmlReqSalDT
    {
        /// <summary>
        /// Branch code
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// Document no.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptXshDocNo  { get; set; }

        /// <summary>
        /// Shop code
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptShpCode  { get; set; }
    }
}