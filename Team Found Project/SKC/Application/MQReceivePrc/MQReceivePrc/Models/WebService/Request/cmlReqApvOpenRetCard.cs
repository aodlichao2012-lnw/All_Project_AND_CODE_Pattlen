using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Request
{
    public class cmlReqApvOpenRetCard
    {
        /// <summary>
        /// รหัสบัตร.
        /// </summary>
        public string ptCrdCode {get;set;}

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
