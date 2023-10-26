using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Config
{
    public class cmlShopDB : cmlDatabase
    {
        /// <summary>
        /// Server.
        /// </summary>
        //[Obsolete("Don't use this", true)]
        public new string tServer { get; set; }

        /// <summary>
        /// User.
        /// </summary>
        //[Obsolete("Don't use this", true)]
        public new string tUser { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        //[Obsolete("Don't use this", true)]
        public new string tPassword { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        //[Obsolete("Don't use this", true)]
        public new string tDatabase { get; set; }

        /// <summary>
        /// Url API2FNWallet.
        /// </summary>
        public string tUrlAPI2FNWallet { get; set; }

        /// <summary>
        /// Record per round for send to api.
        /// </summary>
        public int nRecordPerRound { get; set; }

        /// <summary>
        /// Url API2PSMaster
        /// </summary>
        public string tUrlAPI2PSMaster { get; set; }
    }
}
