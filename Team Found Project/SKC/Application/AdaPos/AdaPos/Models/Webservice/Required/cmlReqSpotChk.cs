using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required
{
    public class cmlReqSpotChk
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        public string ptCrdCode { get; set; }   

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public long pnLngID { get; set; }

        /// <summary>
        /// ยอดเงินใช้ได้จาก บัตร/wristband
        /// </summary>
        public decimal pcAvailable { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ใช้งาน offline จาก บัตร/wristband
        /// </summary>
        public int pnTxnOffline { get; set; }
    }
}
