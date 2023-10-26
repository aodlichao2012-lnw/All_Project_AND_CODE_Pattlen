using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Config
{
    public class cmlDatabase
    {
        /// <summary>
        /// Server.
        /// </summary>
        public string tServer { get; set; }

        /// <summary>
        /// User.
        /// </summary>
        public string tUser { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string tPassword { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string tDatabase { get; set; }

        /// <summary>
        /// Authentication mode.
        /// </summary>
        public string tAuthenMode { get; set; }

        /// <summary>
        /// Connection time out.
        /// </summary>
        public Nullable<int> nConnectTimeOut { get; set; }

        /// <summary>
        /// Command execute time out.
        /// </summary>
        public Nullable<int> nCommandTimeOut { get; set; }
    }
}
