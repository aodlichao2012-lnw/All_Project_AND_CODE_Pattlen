using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cmlRcvPrcWallet
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
        /// ผู้บันทึก
        /// </summary>
        public string ptUsrCode { get; set; }

        /// <summary>
        /// Connection String Database SQL Server for process.
        /// </summary>
        public string ptConnStr { get; set; }
    }
}
