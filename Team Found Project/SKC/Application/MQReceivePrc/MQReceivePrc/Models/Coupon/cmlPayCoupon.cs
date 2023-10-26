using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Coupon
{
    public class cmlPayCoupon
    {
        /// <summary>
        /// เลขที่เอกสารคูปอง
        /// </summary>
        public string ptCphDocNo { get; set; }

        /// <summary>
        /// รหัสคูปอง
        /// </summary>
        public string ptCpdBarCpn { get; set; }

        /// <summary>
        /// ลำดับรายการของเอกสารคูปอง
        /// </summary>
        public int pnCpdSeqNo { get; set; }

        /// <summary>
        /// เลขที่บิลขายที่จะใช้อ้างอิง
        /// </summary>
        public string ptSaleDocNo { get; set; }

        /// <summary>
        /// สาขาที่ทำรายการ
        /// </summary>
        public string ptCpbFrmBch { get; set; }

        /// <summary>
        /// จุดขายที่ทำรายการ
        /// </summary>
        public string ptCpbFrmPos { get; set; }
    }
}
