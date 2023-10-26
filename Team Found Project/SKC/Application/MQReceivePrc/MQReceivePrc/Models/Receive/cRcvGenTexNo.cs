using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Receive
{
    public class cRcvGenTexNo
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// ประเภท
        /// </summary>
        public Nullable<int> pnSaleType { get; set; }

        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string ptDocNo { get; set; } //*Arm 63-06-07

        /// <summary>
        /// รหัสผู้ใช้
        /// </summary>
        public string ptUser { get; set; } //*Arm 63-06-07
    }
    public class cResGenTexNo
    {
        /// <summary>
        /// เลขที่เอกสาร
        /// </summary>
        public string rtDocNo { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }

}
