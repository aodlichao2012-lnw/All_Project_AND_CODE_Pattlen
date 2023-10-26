using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.RabbitMQ
{
    public class cmlResFunc
    {
        public string ptFunction { get; set; }
        public string ptSource { get; set; }
        public string ptDest { get; set; }
        public string ptData { get; set; }
    }
    public class cmlResData
    {
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }

    public class cmlResCoupon
    {
        public List<cmlCoupon> raoCoupon { get; set; }
        public string rtCode { get; set; }
        public string rtDesc { get; set; }
    }

    public class cmlCoupon
    {
        public string rtBchCode { get; set; }
        public string rtCphDocNo { get; set; }
        public string rtCphDisType { get; set; }
        public decimal rcCphDisValue { get; set; }
        public string rtCpnName { get; set; }
        public string rtCpnMsg1 { get; set; }
        public string rtCpnMsg2 { get; set; }
        public string rtCpnCond { get; set; }
        public string rtCpdBarCpn { get; set; }
        public int rnCpdSeqNo { get; set; }
        public string rtCptName { get; set; }
        public string rtCptType { get; set; }
        public int rnQtyAvailable { get; set; }
        public int rnQtyLef { get; set; }
        public Nullable<DateTime> rdCphDateStart { get; set; }
        public Nullable<DateTime> rdCphDateStop { get; set; }
        public string rtCphTimeStart { get; set; }
        public string rtCphTimeStop { get; set; }
    }
}
