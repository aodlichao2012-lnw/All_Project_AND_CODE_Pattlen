using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Request
{
    public class cmlReqChangeSta
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
        /// สถานะบัตร
        /// </summary>
        public string ptCrdSta { get; set; }

        /// <summary>
        /// เลขที่เอกสารอ้างอิง
        /// </summary>
        public string ptDocNoRef { get; set; }
    }
}
