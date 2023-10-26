using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
   public  class cmlDiscountItem
    {
        public string tSaleNo { get; set; }
        public string tPdtCode { get; set; }
        public double cOlePrice { get; set; }
        public double cDisPrice { get; set; }
        public double cNewPrice { get; set; }

    }
}
