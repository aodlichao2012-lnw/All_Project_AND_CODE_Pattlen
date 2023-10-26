using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Request
{
    public class cmlReqChangeCard
    {
        /// <summary>
        /// จากรหัสบัตร
        /// </summary>
        public string ptFrmCrdCode { get; set; }

        /// <summary>
        /// ถึงรหัสบัตร
        /// </summary>
        public string ptToCrdCode { get; set; }

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสารอ้างอิง
        /// </summary>
        public string ptDocNoRef { get; set; }
    }
}
