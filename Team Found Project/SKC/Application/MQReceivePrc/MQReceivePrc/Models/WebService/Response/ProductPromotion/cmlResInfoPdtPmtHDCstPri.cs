using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.ProductPromotion
{
    public class cmlResInfoPdtPmtHDCstPri
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string rtPmhDocNo { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string rtPmhStaType { get; set; }
    }
}
