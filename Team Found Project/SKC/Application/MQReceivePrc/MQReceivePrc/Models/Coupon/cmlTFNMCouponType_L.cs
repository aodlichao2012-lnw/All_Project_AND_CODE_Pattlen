using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Coupon
{
    public class cmlTFNMCouponType_L
    {
        ///<summary>
        ///รหัสประเภทคูปอง
        ///</summary>
        public string FTCptCode { get; set; }

        ///<summary>
        ///รหัสภาษา
        ///</summary>
        public Int64 FNLngID { get; set; }

        ///<summary>
        ///ชื่อประเภท
        ///</summary>
        public string FTCptName { get; set; }

        ///<summary>
        ///หมายเหตุ
        ///</summary>
        public string FTCptRemark { get; set; }


    }
}
