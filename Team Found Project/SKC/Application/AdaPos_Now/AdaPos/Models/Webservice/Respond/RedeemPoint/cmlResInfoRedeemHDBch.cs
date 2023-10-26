using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.RedeemPoint
{
    public class cmlResInfoRedeemHDBch
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
        ///รหัสสาขา
        /// </summary>
        public string rtRdhBchTo { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string rtRdhMerTo { get; set; }

        /// <summary>
        ///ร้านค้า
        /// </summary>
        public string rtRdhShpTo { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string rtRdhStaType { get; set; }
    }
}
