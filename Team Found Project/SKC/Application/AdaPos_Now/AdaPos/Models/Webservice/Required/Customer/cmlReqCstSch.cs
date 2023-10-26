using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Required.Customer
{
    public class cmlReqCstSch
    {
        /// <summary>
        /// รหัสสมาชิก
        /// </summary>
        public string ptCstCode { get; set; }

        /// <summary>
        /// ชื่อลูกค้า
        /// </summary>
        public string ptCstName { get; set; }

        /// <summary>
        /// เบอร์โทร
        /// </summary>
        public string ptCstTel { get; set; }

        /// <summary>
        /// หมายเลขบัตรประชาชน
        /// </summary>
        public string ptCstCardID { get; set; }

        /// <summary>
        /// หมายเลขบัตรสมาชิก
        /// </summary>
        public string ptCstCrdNo { get; set; }

        /// <summary>
        /// หมายเลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string ptCstTaxNo { get; set; }
    }
}
