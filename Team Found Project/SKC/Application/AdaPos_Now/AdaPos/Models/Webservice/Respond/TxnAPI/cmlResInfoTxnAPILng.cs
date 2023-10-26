using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.TxnAPI
{
    public class cmlResInfoTxnAPILng
    {
        /// <summary>
        ///รหัสเส้น API ที่ Interface
        /// </summary>
        public string rtApiCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<int> rnLngID { get; set; }

        /// <summary>
        ///ชื่อเส้น Interface
        /// </summary>
        public string rtApiName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtApiRmk { get; set; }
    }
}
