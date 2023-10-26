using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTSysSyncModuleTmp
    {
        /// <summary>
        /// รหัส Application
        /// </summary>
        public string FTAppCode { get; set; }

        /// <summary>
        /// ลำดับรายการจากตาราง TSysSyncData
        /// </summary>
        public Nullable<int> FNSynSeqNo { get; set; }
    }
}
