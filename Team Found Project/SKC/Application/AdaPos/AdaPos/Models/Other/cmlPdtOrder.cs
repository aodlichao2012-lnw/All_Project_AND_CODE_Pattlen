using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
    public class cmlPdtOrder
    {
        public decimal cQty { get; set; }
        public decimal cFactor { get; set; }
        public string tUnit { get; set; }
        public string tPdtCode { get; set; }
        public string tBarcode { get; set; }
        public string tPdtName { get; set; }
        public decimal cSetPrice { get; set; }
        public string tStaPdt { get; set; }
        public string tStaAlwDis { get; set; }

        /// <summary>
        /// ใช้ราคาขาย 1:บังคับ, 2:แก้ไข, 3:เครื่องชั่ง, 4:น้ำหนัก 6:สินค้ารายการซ่อม
        /// </summary>
        public string tSaleType { get; set; }   //*Arm 63-03-03
    }
}
