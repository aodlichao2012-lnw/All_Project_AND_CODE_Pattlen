using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.Database
{
    public class cmlTPSTShiftDT
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
        /////รหัสร้านค้า
        ///// <summary>
        //public string FTShpCode { get; set; }

        ///// <summary>
        /////รหัสผู้เปิดรอบ/ชื่อผู้ใช้
        ///// <summary>
        //public string FTUsrCode { get; set; }

        ///// <summary>
        /////วันที่และเวลา Sign In
        ///// <summary>
        //public Nullable<DateTime> FDSdtSignIn { get; set; }

        ///// <summary>
        /////วันที่และเวลา Sign Out
        ///// <summary>
        //public Nullable<DateTime> FDSdtSignOut { get; set; }

        ///// <summary>
        /////รหัสผู้ปิดรอบ/ชื่อผู้ใช้
        ///// <summary>
        //public string FTSdtUsrClosed { get; set; }

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
        ///รหัสร้านค้า
        /// </summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัสผู้เปิดรอบ/ชื่อผู้ใช้
        /// </summary>
        public string FTUsrCode { get; set; }

        /// <summary>
        ///วันที่และเวลา Sign In
        /// </summary>
        public Nullable<DateTime> FDSdtSignIn { get; set; }

        /// <summary>
        ///วันที่และเวลา Sign Out
        /// </summary>
        public Nullable<DateTime> FDSdtSignOut { get; set; }

        /// <summary>
        ///รหัสผู้ปิดรอบ/ชื่อผู้ใช้
        /// </summary>
        public string FTSdtUsrClosed { get; set; }
    }
}