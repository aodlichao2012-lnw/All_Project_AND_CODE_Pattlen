using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond
{
    public class cmlResIsOnline
    {
        /// <summary>
        /// System process status.
        /// </summary>
        public string rtCode;

        /// <summary>
        /// System process description.
        /// </summary>
        public string rtDesc;

        /// <summary>
        /// Response result
        /// </summary>
        public string rtResult { get; set; }
    }
}
