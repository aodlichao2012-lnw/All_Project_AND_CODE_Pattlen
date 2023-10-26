using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.RedeemPoint
{
    public class cmlResInfoRedeemHDCstPri
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่นแลกคะแนน XXYY-######
        /// </summary>
        public string rtRdhDocNo { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string rtRdhStaType { get; set; }
    }
}
