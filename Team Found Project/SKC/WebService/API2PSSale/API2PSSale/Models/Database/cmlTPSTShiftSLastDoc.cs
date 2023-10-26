using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.Database
{
    public class cmlTPSTShiftSLastDoc
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
        /////ประเภทเอกสาร
        ///// <summary>
        //public Nullable<int> FNLstDocType { get; set; }

        ///// <summary>
        /////จาก เลขที่เอกสารล่าสุด
        ///// <summary>
        //public string FTLstDocNoFrm { get; set; }

        ///// <summary>
        /////ถึง เลขที่เอกสารล่าสุด
        ///// <summary>
        //public string FTLstDocNoTo { get; set; }

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
        ///ประเภทเอกสาร
        /// </summary>
        public Nullable<int> FNLstDocType { get; set; }

        /// <summary>
        ///จาก เลขที่เอกสารล่าสุด
        /// </summary>
        public string FTLstDocNoFrm { get; set; }

        /// <summary>
        ///ถึง เลขที่เอกสารล่าสุด
        /// </summary>
        public string FTLstDocNoTo { get; set; }
    }
}