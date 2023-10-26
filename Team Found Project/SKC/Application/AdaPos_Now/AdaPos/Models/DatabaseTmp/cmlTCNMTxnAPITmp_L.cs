using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMTxnAPITmp_L
    {
        /// <summary>
        ///รหัสเส้น API ที่ Interface
        /// </summary>
        public string FTApiCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<int> FNLngID { get; set; }

        /// <summary>
        ///ชื่อเส้น Interface
        /// </summary>
        public string FTApiName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string FTApiRmk { get; set; }
    }
}
