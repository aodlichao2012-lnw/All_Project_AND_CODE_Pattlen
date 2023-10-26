using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Base
{
    public class cmlResItem<T> : cmlResBase
    {
        /// <summary>
        /// list Item response
        /// </summary>
        public T roItem;
    }
}
