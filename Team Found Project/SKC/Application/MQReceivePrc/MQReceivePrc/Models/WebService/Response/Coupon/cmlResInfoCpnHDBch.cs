using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Coupon
{
    public class cmlResInfoCpnHDBch
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
        ///รหัสสาขา
        /// </summary>
        public string rtCphBchTo { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string rtCphMerTo { get; set; }

        /// <summary>
        ///ร้านค้า
        /// </summary>
        public string rtCphShpTo { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string rtCphStaType { get; set; }
    }
}
