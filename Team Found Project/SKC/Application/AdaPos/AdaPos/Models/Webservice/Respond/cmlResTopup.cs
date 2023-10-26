using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond
{
    public class cmlResTopup
    {
        /// <summary>
        /// ยอดทั้งหมด
        /// </summary>
        public double rcTxnValue { get; set; }

        /// <summary>
        /// เงินมัดจำบัตร
        /// </summary>
        public double rcCtyDeposit { get; set; }

        /// <summary>
        /// เงินมัดจำสินค้า
        /// </summary>
        public double rcCrdDeposit { get; set; }

        /// <summary>
        /// ยอดใช้ได้
        /// </summary>
        public double rcTxnValueAvb { get; set; }

        /// <summary>
        /// ประเภทบัตร
        /// </summary>
        public string rtCtyCode { get; set; }

        /// <summary>
        /// วันที่บัตรหมดอายุ
        /// </summary>
        public DateTime rdCrdExpireDate { get; set; }

        /// <summary>
        /// ชื่อบัตร
        /// </summary>
        public string rtCrdName { get; set; }

        /// <summary>
        /// System process status.
        /// </summary>
        public string rtCode { get; set; }

        /// <summary>
        /// System process description.
        /// </summary>
        public string rtDesc { get; set; }

        /// <summary>
        /// ลำดับอ้างอิง
        /// </summary>
        public int rnTxnID { get; set; }
    }
}
