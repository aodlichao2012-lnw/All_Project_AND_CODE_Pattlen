using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSSale.Models.Database
{

    public class cmlTPSTUsrLog
    {
        /// <summary>
        ///ชื่อเครื่องคอมพิวเตอร์
        /// <summary>
        public string FTComName { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสเครื่องจุดขาย
        /// <summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///รหัสผู้ใช้
        /// <summary>
        public string FTUsrCode { get; set; }

        /// <summary>
        ///วันเวลาที่เข้าระบบ
        /// <summary>
        public Nullable<DateTime> FDShdSignIn { get; set; }

        /// <summary>
        ///วันเวลาที่ออกจากระบบ
        /// <summary>
        public Nullable<DateTime> FDShdSignOut { get; set; }

        /// <summary>
        ///รหัสรอบการขาย
        /// <summary>
        public string FTShfCode { get; set; }

        /// <summary>
        ///รหัส Application 
        /// <summary>
        public string FTAppCode { get; set; }

        /// <summary>
        ///Version Application
        /// <summary>
        public string FTAppVersion { get; set; }
    }
}