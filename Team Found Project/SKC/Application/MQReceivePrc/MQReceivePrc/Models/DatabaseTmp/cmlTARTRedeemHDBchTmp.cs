using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTARTRedeemHDBchTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่นแลกคะแนน XXYY-######
        /// </summary>
        public string FTRdhDocNo { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTRdhBchTo { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string FTRdhMerTo { get; set; }

        /// <summary>
        ///ร้านค้า
        /// </summary>
        public string FTRdhShpTo { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string FTRdhStaType { get; set; }
    }
}
