using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Coupon
{
    public class cmlResInfoCpnHDPdt
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสเอกสาร
        /// </summary>
        public string rtCphDocNo { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// </summary>
        public string rtPunCode { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        /// </summary>
        public string rtCphStaType { get; set; }
    }
}
