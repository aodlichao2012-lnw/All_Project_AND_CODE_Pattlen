using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Coupon
{
    public class cmlTFNTCoupon
    {
        public List<cmlTFNTCouponHD> aoTFNTCouponHD { get; set; }
        public List<cmlTFNTCouponHD_L> aoTFNTCouponHD_L { get; set; }
        public List<cmlTFNMCouponType> aoTFNMCouponType { get; set; }
        public List<cmlTFNMCouponType_L> aoTFNMCouponType_L { get; set; }
        public List<cmlTFNTCouponDT> aoTFNTCouponDT { get; set; }
        public List<cmlTFNTCouponDTHis> aoTFNTCouponDTHis { get; set; }
        public List<cmlTFNTCouponHDBch> aoTFNTCouponHDBch { get; set; }
        public List<cmlTFNTCouponHDCstPri> aoTFNTCouponHDCstPri { get; set; }
        public List<cmlTFNTCouponHDPdt> aoTFNTCouponHDPdt { get; set; }
    }
}
