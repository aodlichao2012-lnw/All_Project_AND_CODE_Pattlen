using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Pos
{
    class cmlResPosRegister
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสเครื่องจุดขาย
        /// </summary>
        public string rtPosCode { get; set; }

        /// <summary>
        /// Mac Address
        /// </summary>
        public string rtMacAddr { get; set; }

        /// <summary>
        /// Computer Name
        /// </summary>
        public string rtCompName { get; set; }

        /// <summary>
        /// สถานะ
        /// </summary>
        public string rtStaApv { get; set; }

        /// <summary>
        /// System process status.
        /// </summary>
        public string rtCode;

        /// <summary>
        /// System process description.
        /// </summary>
        public string rtDesc;
    }
}
