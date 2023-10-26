using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Webservice.Response.Coupon
{
    public class cmlResInfoCpnType
    {
        public string rtCptCode { get; set; }
        public string rtCptStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string rtCptType { get; set; }       //*Arm 62-12-24

        /// <summary>
        ///
        /// <summary>
        public string rtCptStaChk { get; set; }     //*Arm 62-12-24

        /// <summary>
        ///ประเภท 1:HQ 2:Branch Def: Branch  ใช้ตรวจสอบคูปอง
        /// <summary>
        public string rtCptStaChkHQ { get; set; }   //*Arm 62-12-24
    }
}
