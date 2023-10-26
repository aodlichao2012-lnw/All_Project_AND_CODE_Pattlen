using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTSysDisPolicyTmp_L
    {
        /// <summary>
        ///รหัส Discount policy
        /// </summary>
        public string FTDisCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///คำอธิบาย
        /// </summary>
        public string FTDisName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string FTDisRemark { get; set; }
    }
}
