using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.WebService.Response.Product
{
    public class cmlResInfoPdtSpcWah
    {
        /// <summary>
        /// รหัสสินค้า/รหัสควบคุมสต๊อก
        /// </summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        /// สาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสคลัง
        /// </summary>
        public string rtWahCode { get; set; }

        /// <summary>
        /// จำนวนต่ำสุด
        /// </summary>
        public Nullable<decimal> rcSpwQtyMin { get; set; }

        /// <summary>
        /// จำนวนสูงสุด
        /// </summary>
        public Nullable<decimal> rcSpwQtyMax { get; set; }

        /// <summary>
        /// หมายเหตุ (Refer external)
        /// </summary>
        public string rtSpwRmk { get; set; }
    }
}
