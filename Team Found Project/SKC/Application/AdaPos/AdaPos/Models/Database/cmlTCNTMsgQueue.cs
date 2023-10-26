using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    class cmlTCNTMsgQueue
    {
        public int FNMsgID { get; set; }
        public string FTMsgQName{ get; set; }
        public string FTMsgQData { get; set; }
        public string FTMsgStaActive { get; set; }
        public string FTMsgStaPrc { get; set; }
        public string FTMsgRemark { get; set; }
        public DateTime FDCreateOn { get; set; }
        public string FTCreateBy { get; set; }
    }
}
