using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductPromotion
{
    public class cmlResInfoPdtPmtHDBch
    {
        //*Arm 63-03-26 ปรับแก้ Standard

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string rtPmhDocNo { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtPmhBchTo { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string rtPmhMerTo { get; set; }

        /// <summary>
        ///ร้านค้า
        /// </summary>
        public string rtPmhShpTo { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string rtPmhStaType { get; set; }

        //+++++++++++++


        ///// <summary>
        /////รหัสสาขา
        ///// </summary>
        //public string rtFTBchCode { get; set; }

        ///// <summary>
        /////รหัสโปรโมชั่น XXYY-######
        ///// </summary>
        //public string rtFTPmhDocNo { get; set; }

        ///// <summary>
        /////รหัสสาขา
        ///// </summary>
        //public string rtFTPmhBchTo { get; set; }

        ///// <summary>
        /////รหัสตัวแทน/เจ้าของกำเนินการ
        ///// </summary>
        //public string rtFTPmhMerTo { get; set; }

        ///// <summary>
        /////ร้านค้า
        ///// </summary>
        //public string rtFTPmhShpTo { get; set; }

        ///// <summary>
        /////1:Include 2:ยกเว้น
        ///// </summary>
        //public string rtFTPmhStaType { get; set; }
    }
}
