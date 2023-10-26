using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTCNMPdtSpcBchTmp
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///สาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของดำเนินการ
        /// <summary>
        public string FTMerCode { get; set; }

        /// <summary>
        ///รหัสกลุ่ม ตาม Merchant
        /// <summary>
        public string FTMgpCode { get; set; }

        /// <summary>
        ///จำนวนคงเหลือต่ำสุด
        /// <summary>
        public Nullable<decimal> FCPdtMin { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///หมายเหตุ (Refer external)
        /// <summary>
        public string FTPdtRmk { get; set; }
    }
}
