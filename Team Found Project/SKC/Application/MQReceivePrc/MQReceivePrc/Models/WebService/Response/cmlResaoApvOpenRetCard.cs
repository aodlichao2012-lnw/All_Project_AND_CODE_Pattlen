using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response
{
    public class cmlResaoApvOpenRetCard
    {
        /// <summary>
        /// list card information.
        /// </summary>
        public List<cmlResApvOpenRetCard> roApvOpenCard { get; set; }

        /// <summary>
        /// System process status.
        /// </summary>
        public string rtCode { get; set; }

        /// <summary>
        /// System process description.
        /// </summary>
        public string rtDesc { get; set; }
    }
}
