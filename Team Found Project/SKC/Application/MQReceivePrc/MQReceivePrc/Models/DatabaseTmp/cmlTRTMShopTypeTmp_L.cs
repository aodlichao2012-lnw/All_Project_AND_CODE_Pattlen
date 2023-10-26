using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTRTMShopTypeTmp_L
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<int> FNLngID { get; set; }

        /// <summary>
        ///ประเภทตู้ 1 : ปกติ , 2 : ควบคุมอุณหภูมิ
        /// <summary>
        public string FTShtType { get; set; }

        /// <summary>
        ///ชื่อประเภท Shop
        /// <summary>
        public string FTShtName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string FTShtRemark { get; set; }
    }
}
