using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Bch2Bch
{
    public class cmlCallSendBch
    {
        /// <summary>
        /// สาขาต้นทาง
        /// </summary>
        public string ptBchFrm { get; set; }

        /// <summary>
        /// สาขาปลายทาง
        /// </summary>
        public string ptBchTo { get; set; }
        
        /// <summary>
        /// ประเภทเอกสาร
        /// 1:ใบโอนระหว่างสาขา, 2:ใบปรับราคา
        /// </summary>
        public string ptDocType { get; set; }

        /// <summary>
        /// เลชที่เอกสาร
        /// </summary>
        public string ptDocNo { get; set; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string ptConnStr { get; set; }
    }
}
