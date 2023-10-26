using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.Download
{
    public class cmlResInfoSyncModule
    {
        /// <summary>
        /// รหัส Application
        /// </summary>
        public string rtAppCode { get; set; }

        /// <summary>
        /// ลำดับรายการจากตาราง TSysSyncData
        /// </summary>
        public Nullable<int> rnSynSeqNo { get; set; }
    }
}
