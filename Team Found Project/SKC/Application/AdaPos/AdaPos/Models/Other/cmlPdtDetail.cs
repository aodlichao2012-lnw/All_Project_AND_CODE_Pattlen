using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
    public class cmlPdtDetail
    {
        public string tPdtCode { get; set; }
        public string tBarcode { get; set; }
        public string tPdtName { get; set; }
        public string tUnitName { get; set; }
        public decimal cUnitFactor { get; set; }
        public decimal cPdtPrice { get; set; }
        public int nRowCount { get; set; }
        public string tPicPath { get; set; }

        /// <summary>
        /// ใช้ราคาขาย 1:บังคับ, 2:แก้ไข, 3:เครื่องชั่ง, 4:น้ำหนัก 6:สินค้ารายการซ่อม
        /// </summary>
        public string tSaleType { get; set; }

        /// <summary>
        /// Status อนุญาติให้ลดราคา 1:อนุญาติให้ลดราคา ,0:ไม่อนุญาติให้ลดราคา
        /// </summary>
        public string tStaAlwDis { get; set; }

        public string tPdtNameOth { get; set; } // *Arm 63-03-03

        public string tPunCode { get; set; } // Zen 63-03-11
    }
}
