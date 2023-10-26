using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPosHWTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; } //*Arm 63-01-30

        public string FTPhwCode { get; set; }
        public string FTPosCode { get; set; }
        public string FTShwCode { get; set; }
        public string FTPhwCodeRef { get; set; }
        public long FNPhwSeq { get; set; }
        public string FTPhwName { get; set; }
        public string FTPhwConnType { get; set; }
        public string FTPhwConnRef { get; set; }
        public string FTPhwCustom { get; set; }
        public Nullable<int> FNPhwTimeOut { get; set; }

    }
}
