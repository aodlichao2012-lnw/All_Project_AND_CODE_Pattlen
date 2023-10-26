using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.Stock.Stockbalance
{
    public class cmlTCNTPdtStkBal
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสคลังสินค้า
        /// <summary>
        public string FTWahCode { get; set; }

        /// <summary>
        ///รหัสควบคุมสต๊อก
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///จำนวนสินค้า
        /// <summary>
        public Nullable<decimal> FCStkQty { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }
    }
}
