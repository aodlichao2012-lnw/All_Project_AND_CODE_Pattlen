using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cmlRcvSalePos
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสเครื่องจุดขาย
        /// </summary>
        public string ptPosCode { get; set; }

        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string ptXihDocNo { get; set; }

        /// <summary>
        /// Connection String Database for process.
        /// </summary>
        public string ptConnStr { get; set; }

        /// <summary>
        /// ประเภทเอกสาร 1:บิลขาย 2:เอกสารรอบ
        /// </summary>
        public int pnXihDocType { get; set; }
    }
}
