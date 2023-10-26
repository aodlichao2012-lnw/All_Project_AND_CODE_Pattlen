using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.WebService.Response.Base
{
    /// <summary>
    /// Class model list Item response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class cmlResItem<T> : cmlResBase
    {
        /// <summary>
        /// list Item response
        /// </summary>
        public T roItem;
    }
}