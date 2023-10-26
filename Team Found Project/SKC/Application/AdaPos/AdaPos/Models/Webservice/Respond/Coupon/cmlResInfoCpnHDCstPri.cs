using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdaPos.Models.Webservice.Respond.Coupon
{
    /// <summary>
    /// ตาราง TFNTCouponHDCstPri
    /// </summary>
    public class cmlResInfoCpnHDCstPri
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
        ///รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string rtCphStaType { get; set; }



    }
}