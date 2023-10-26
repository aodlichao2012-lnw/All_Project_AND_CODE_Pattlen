using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Class.Standard
{
    /// <summary>
    /// Class constant
    /// </summary>
    public class cCS
    {
        /// <summary>
        /// // Version Main API.
        /// </summary>
        public const string tCS_APIVer = "V4";

        // Message attribute model.
        public const string tCS_MsgAtrRequired = "The {0} field is required.";
        public const string tCS_MsgAtrMaxLength = "{0} cannot be greater than {1}.";
        public const string tCS_MsgAtrMinLength = "{0} cannot be less then {1}.";
        public const string tCS_MsgAtrArrayMaxLength = "{0} size cannot be greater than {1}.";

        public const string tCS_AESKey = "BJHhj?AW=c7-4#vF"; // Key of more+
        public const string tCS_AESIV = "bh_B4Hu?L4@CV6pb";  // IV of more+
        public const string tCS_SHA1Key1 = "AdaSoft";
        public const string tCS_SHA1Key2 = "Sk$4dF#z"; //"SOFTXada";
    }
}