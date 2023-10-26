﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.Database
{
    public class cmlTPSTShiftSKeyBN
    {
        ///// <summary>
        /////รหัสสาขา
        ///// <summary>
        //public string FTBchCode { get; set; }

        ///// <summary>
        /////รหัสเครื่อง POS
        ///// <summary>
        //public string FTPosCode { get; set; }

        ///// <summary>
        /////รหัสรอบแคชเชียร์  YYMMDDNN
        ///// <summary>
        //public string FTShfCode { get; set; }

        ///// <summary>
        /////ลำดับการ SignIn DT
        ///// <summary>
        //public Nullable<int> FNSdtSeqNo { get; set; }

        ///// <summary>
        /////รหัสธนบัตร
        ///// <summary>
        //public string FTBntCode { get; set; }

        ///// <summary>
        /////มูลค่า ธนบัตร
        ///// <summary>
        //public Nullable<decimal> FCKbnRateAmt { get; set; }

        ///// <summary>
        /////จำนวนแบงค์
        ///// <summary>
        //public Nullable<Int64> FNKbnQty { get; set; }

        ///// <summary>
        /////มูลค่าแบงค์
        ///// <summary>
        //public Nullable<decimal> FCKbnAmt { get; set; }

        // +++++++++++++++++++++++++++++++




        //*Arm 63-01-24 - ปรับโครงสร้าง Database ใหม่

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง POS
        /// </summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///รหัสรอบแคชเชียร์  YYMMDDNN
        /// </summary>
        public string FTShfCode { get; set; }

        /// <summary>
        ///ลำดับการ SignIn DT
        /// </summary>
        public Nullable<int> FNSdtSeqNo { get; set; }

        /// <summary>
        ///รหัสธนบัตร
        /// </summary>
        public string FTBntCode { get; set; }

        /// <summary>
        ///มูลค่า ธนบัตร
        /// </summary>
        public Nullable<decimal> FCKbnRateAmt { get; set; }

        /// <summary>
        ///จำนวนแบงค์
        /// </summary>
        public Nullable<Int64> FNKbnQty { get; set; }

        /// <summary>
        ///มูลค่าแบงค์
        /// </summary>
        public Nullable<decimal> FCKbnAmt { get; set; }
        
    }
}