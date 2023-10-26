using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Database
{
    public class cmlTCNTMsgRemind
    {
        public int FNMsgID { get; set; }
        public DateTime FDCreateOn { get; set; }
        public string FTMsgStaRead { get; set; }
        public Nullable<int> FNMsgSeq { get; set; }
        public int FNMsgType { get; set; }
        public string FTMsgGroup { get; set; }
        public string FTMsgDocRef { get; set; }
        public string FTCreateBy { get; set; }
        public string FTMsgData { get; set; }
        public string FTMsgRmk { get; set; }
        public int FNLngID { get; set; }

        /// <summary>
        /// TSysSyncData_L 
        /// </summary>
        public string FTSynName { get; set; }
    }
}
