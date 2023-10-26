using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.RedeemPoint
{
    public class cmlResInfoRedeemHDLng
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสแลกคะแนน XXYY-######
        /// </summary>
        public string rtRdhDocNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่นแลกคะแนน
        /// </summary>
        public string rtRdhName { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่นแลกคะแนน(แบบย่อ)
        /// </summary>
        public string rtRdhNameSlip { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtRdhRmk { get; set; }
    }
}
