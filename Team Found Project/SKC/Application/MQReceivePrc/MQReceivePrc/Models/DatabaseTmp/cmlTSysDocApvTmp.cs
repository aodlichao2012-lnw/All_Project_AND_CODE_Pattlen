using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTSysDocApvTmp
    {
        /// <summary>
        ///(AUTONUMBER)รหัส
        /// </summary>
        public Nullable<Int64> FNDapID { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// </summary>
        public string FTDapTable { get; set; }

        /// <summary>
        ///FNXshDocType
        /// </summary>
        public string FTDapRefType { get; set; }

        /// <summary>
        ///ลำดับ การอนุมัติ
        /// </summary>
        public Nullable<int> FNDapSeq { get; set; }

        /// <summary>
        ///ชื่อผู้อนุมัติ
        /// </summary>
        public string FTDapName { get; set; }

        /// <summary>
        ///ชื่อผู้อนุมัติ
        /// </summary>
        public string FTDapNameOth { get; set; }

    }
}
