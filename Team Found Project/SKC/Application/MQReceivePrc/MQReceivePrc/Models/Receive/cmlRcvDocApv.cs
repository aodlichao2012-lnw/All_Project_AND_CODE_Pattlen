using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cmlRcvDocApv
    {
       /// <summary>
       /// รหัสสาขา
       /// </summary>
        public string ptBchCode { get; set; }
        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string ptDocNo { get; set; }
        /// <summary>
        /// ประเภทเอกสาร
        /// </summary>
        public string ptDocType { get; set; }
        /// <summary>
        /// ผู้อนุมัติ
        /// </summary>
        public string ptUser { get; set; }
        /// <summary>
        /// Connection String Database for process.
        /// </summary>
        public string ptConnStr { get; set; }
    }
}
