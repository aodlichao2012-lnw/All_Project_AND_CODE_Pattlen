using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.ProductPromotion
{
    public class cmlResInfoPdtPmtHD_L
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
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่น
        /// </summary>
        public string rtPmhName { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่น(แบบย่อ)
        /// </summary>
        public string rtPmhNameSlip { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtPmhRmk { get; set; }

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
        /////รหัสภาษา
        ///// </summary>
        //public Nullable<Int64> rnFNLngID { get; set; }

        ///// <summary>
        /////ชื่อโปรโมชั่น
        ///// </summary>
        //public string rtFTPmhName { get; set; }

        ///// <summary>
        /////ชื่อโปรโมชั่น(แบบย่อ)
        ///// </summary>
        //public string rtFTPmhNameSlip { get; set; }

        ///// <summary>
        /////หมายเหตุ
        ///// </summary>
        //public string rtFTPmhRmk { get; set; }
    }
}
