using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Request
{
    public class cmlReqTopupList
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
        /// สถานะเติมเงิน Auto or manual 0: manual, 1: Auto
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
        /// เลขที่เอกสารอ้างอิง
        /// </summary>
        public string ptDocNoRef { get; set; }
    }
}
