using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required
{
    public class cmlReqTopup
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
        /// มูลค่า
        /// </summary>
        public decimal pcTxnValue { get; set; }

        /// <summary>
        /// สถานะเติมเงิน Auto or manual
        /// </summary>
        public string ptAuto { get; set; }

        /// <summary>
        /// รหัสเครื่อง POS
        /// </summary>
        public string ptTxnPosCode { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        public string ptShpCode { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public int pnLngID { get; set; }

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
