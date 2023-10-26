using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTARTRedeemHDCstPriTmp
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
        ///รหัสกลุ่มราคา
        /// </summary>
        public string FTPplCode { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string FTRdhStaType { get; set; }
    }
}
