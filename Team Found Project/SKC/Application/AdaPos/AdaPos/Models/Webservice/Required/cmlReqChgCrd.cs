using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required
{
    /// <summary>
    /// Parameter change card or wristband information.
    /// </summary>
    public class cmlReqChgCrd
    {
        /// <summary>
        /// Change card or wristband from.
        /// </summary>
        public string ptFrmCrdCode { get; set; }

        /// <summary>
        /// Change card or wristband to.
        /// </summary>
        public string ptToCrdCode { get; set; }

        /// <summary>
        /// Branch code.
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// Document no. referent.
        /// </summary>
        public string ptDocNoRef { get; set; }
    }
}
