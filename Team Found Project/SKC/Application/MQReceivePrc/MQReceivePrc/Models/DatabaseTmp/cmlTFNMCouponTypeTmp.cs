using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTFNMCouponTypeTmp
    {
        public string FTCptCode { get; set; }
        public string FTCptStaUse { get; set; }
        public Nullable<DateTime> FDLastUpdOn { get; set; }
        public Nullable<DateTime> FDCreateOn { get; set; }
        public string FTLastUpdBy { get; set; }
        public string FTCreateBy { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string FTCptType { get; set; }       //*Arm 62-12-24

        /// <summary>
        ///
        /// <summary>
        public string FTCptStaChk { get; set; }     //*Arm 62-12-24

        /// <summary>
        ///ประเภท 1:HQ 2:Branch Def: Branch  ใช้ตรวจสอบคูปอง
        /// <summary>
        public string FTCptStaChkHQ { get; set; }   //*Arm 62-12-24

    }
}
