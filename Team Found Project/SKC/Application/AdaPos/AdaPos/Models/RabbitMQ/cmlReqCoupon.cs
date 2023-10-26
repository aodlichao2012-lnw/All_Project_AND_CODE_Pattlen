using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.RabbitMQ
{
    public class cmlReqFunc
    {
        public string ptFucntion { get; set; }
        public string ptSource { get; set; }
        public string ptDest { get; set; }
        public string ptData { get; set; }
    }

    public class cmlReqData
    {
        public string ptCphDocNo { get; set; }
        public string ptCpdBarCpn { get; set; }
        public string ptCpdSeqNo { get; set; }
        public string ptSaleDocNo { get; set; }
        public string ptCpbFrmBch { get; set; }
        public string ptCpbFrmPos { get; set; }
    }

    public class cmlReqCoupon
    {
        public string ptCouponType { get; set; }
        public string ptCpnDocNo { get; set; } //*Net 63-03-12 Create
        public string ptBarCpn { get; set; }  //*Net 63-03-12 Rename
        public string ptBranch { get; set; }
        public string ptPriceGroup { get; set; }
        public string ptMerchant { get; set; }
        public int pnLangID { get; set; }
        public string ptCstCode { get; set; } //*Net 63-03-21 Create
    }

    public class cmlReqCancelCoupon
    {
        public string ptCpbFrmBch { get; set; }
        public string ptCpbFrmPos { get; set; }
        public string ptCpbFrmSaleRef { get; set; }
    }
}
