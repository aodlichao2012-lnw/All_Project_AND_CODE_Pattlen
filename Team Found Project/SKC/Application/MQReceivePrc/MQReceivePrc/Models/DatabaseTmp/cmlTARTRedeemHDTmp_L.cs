using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTARTRedeemHDTmp_L
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสแลกคะแนน XXYY-######
        /// </summary>
        public string FTRdhDocNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> FNLngID { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่นแลกคะแนน
        /// </summary>
        public string FTRdhName { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่นแลกคะแนน(แบบย่อ)
        /// </summary>
        public string FTRdhNameSlip { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string FTRdhRmk { get; set; }
    }
}
