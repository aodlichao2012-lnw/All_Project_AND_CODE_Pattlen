using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPdtCostAvgTmp
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///ต้นทุน/หน่วยเล็ก ไม่รวมภาษี(ปรับตอนประมวลผลต้นทุน)
        /// <summary>
        public Nullable<decimal> FCPdtCostEx { get; set; }

        /// <summary>
        ///ต้นทุน/หน่วยเล็ก รวมภาษี(ปรับตอนประมวลผลต้นทุน)
        /// <summary>
        public Nullable<decimal> FCPdtCostIn { get; set; }

        /// <summary>
        ///จำนวนคงเหลือ(ปรับตอนประมวลผลสต๊อก)
        /// <summary>
        public Nullable<decimal> FCPdtQtyBal { get; set; }

        /// <summary>
        ///ต้นทุนรวม ตามจำนวนคงเหลือ(ปรับตอนประมวลผลสต๊อก,ต้นทุน)
        /// <summary>
        public Nullable<decimal> FCPdtCostAmt { get; set; }

        /// <summary>
        ///ราคาต้นทุน ซื้อครั้งสุดท้าย
        /// <summary>
        public Nullable<decimal> FCPdtCostLast { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }
    }
}
