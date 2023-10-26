using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPdtSpcWahTmp
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// </summary>
        public string FTPdtCode { get; set; }

        /// <summary>
        ///สาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสคลัง
        /// </summary>
        public string FTWahCode { get; set; }

        /// <summary>
        ///จำนวนต่ำสุด
        /// </summary>
        public Nullable<decimal> FCSpwQtyMin { get; set; }

        /// <summary>
        ///จำนวนสูงสุด
        /// </summary>
        public Nullable<decimal> FCSpwQtyMax { get; set; }

        /// <summary>
        ///หมายเหตุ (Refer external)
        /// </summary>
        public string FTSpwRmk { get; set; }
    }
}
