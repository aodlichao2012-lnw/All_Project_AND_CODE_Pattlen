using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.RabbitMQ
{
    public class cmlReqUpdRefund
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// เลขที่เอกสารคืน
        /// </summary>
        public string ptDocNo { get; set; }

        /// <summary>
        /// เลขที่เอกสารขาย
        /// </summary>
        public string ptRefDocNo { get; set; }

        /// <summary>
        /// ข้อมูล SaleDT (คืน)
        /// </summary>
        public string tData { get; set; }
    }
}
